using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Applications.Data;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Waf.BookLibrary.Library.Applications.Controllers
{
    /// <summary>
    /// Responsible for the database connection and the save operation.
    /// </summary>
    [Export(typeof(IEntityController))]
    internal class EntityController : IEntityController
    {
        private const string ResourcesDirectoryName = "Resources";
        
        private readonly EntityService entityService;
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly Lazy<ShellViewModel> shellViewModel;
        private readonly DelegateCommand saveCommand;
        private BookLibraryContext bookLibraryContext;

        [ImportingConstructor]
        public EntityController(EntityService entityService, IMessageService messageService, IShellService shellService, 
            Lazy<ShellViewModel> shellViewModel)
        {
            this.entityService = entityService;
            this.messageService = messageService;
            this.shellService = shellService;
            this.shellViewModel = shellViewModel;
            saveCommand = new DelegateCommand(() => Save(), CanSave);
        }
        
        private ShellViewModel ShellViewModel => shellViewModel.Value;

        public void Initialize()
        {
            // Create directory for the database.
            string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ApplicationInfo.Company, ApplicationInfo.ProductName);
            if (!Directory.Exists(Path.Combine(dataDirectory, ResourcesDirectoryName)))
            {
                Directory.CreateDirectory(Path.Combine(dataDirectory, ResourcesDirectoryName));
            }

            // Set |DataDirectory| macro to our own path. This macro is used within the connection string.
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

            var connectionString = ConfigurationManager.ConnectionStrings["BookLibraryContext"].ConnectionString;

            // Copy the template database file into the DataDirectory when it doesn't exists.
            string dataSourcePath = connectionString.Split(';').Single(x => x.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                .Replace("|DataDirectory|", dataDirectory).Substring(12);
            if (!File.Exists(dataSourcePath))
            {
                string dbFile = Path.GetFileName(dataSourcePath);
                File.Copy(Path.Combine(ApplicationInfo.ApplicationPath, ResourcesDirectoryName, dbFile), dataSourcePath);
            }

            bookLibraryContext = new BookLibraryContext("Data Source=" + dataSourcePath);
            entityService.BookLibraryContext = bookLibraryContext;

            PropertyChangedEventManager.AddHandler(ShellViewModel, ShellViewModelPropertyChanged, "");
            ShellViewModel.SaveCommand = saveCommand;
            ShellViewModel.DatabasePath = dataSourcePath;
        }

        public void Shutdown()
        {
            bookLibraryContext.Dispose();
        }

        public bool HasChanges()
        {
            return bookLibraryContext != null && bookLibraryContext.HasChanges();
        }

        public bool CanSave() { return ShellViewModel.IsValid; }
        
        public bool Save()
        {
            if (!CanSave()) 
            { 
                throw new InvalidOperationException("You must not call Save when CanSave returns false."); 
            }

            var entities = bookLibraryContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).Select(x => x.Entity).ToArray();
            var errors = entities.OfType<ValidatableModel>().Where(x => x.HasErrors).ToArray();
            if (errors.Any())
            {
                var errorMessages = errors.Select(x => string.Format(CultureInfo.CurrentCulture, Resources.EntityInvalid, EntityToString(x), 
                    string.Join(Environment.NewLine, x.Errors)));
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    Resources.SaveErrorInvalidEntities, string.Join(Environment.NewLine, errorMessages)));
                return false;
            }

            bookLibraryContext.SaveChanges();
            return true;
        }

        private void ShellViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShellViewModel.IsValid))
            {
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        internal static string EntityToString(object entity)
        {
            return entity is IFormattable formattableEntity ? formattableEntity.ToString(null, CultureInfo.CurrentCulture) : entity.ToString();
        }
    }
}
