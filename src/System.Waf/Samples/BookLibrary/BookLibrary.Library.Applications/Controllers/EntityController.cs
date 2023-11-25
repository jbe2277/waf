using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Waf.BookLibrary.Library.Applications.Controllers;

/// <summary>Responsible for the database connection and the save operation.</summary>
[Export(typeof(IEntityController)), Export]
internal class EntityController : IEntityController
{
    private readonly EntityService entityService;
    private readonly IMessageService messageService;
    private readonly IShellService shellService;
    private readonly IDBContextService dBContextService;
    private readonly Lazy<ShellViewModel> shellViewModel;
    private readonly DelegateCommand saveCommand;
    private DbContext? bookLibraryContext;

    [ImportingConstructor]
    public EntityController(EntityService entityService, IMessageService messageService, IShellService shellService, IDBContextService dBContextService, Lazy<ShellViewModel> shellViewModel)
    {
        this.entityService = entityService;
        this.messageService = messageService;
        this.shellService = shellService;
        this.dBContextService = dBContextService;
        this.shellViewModel = shellViewModel;
        saveCommand = new(() => Save(), CanSave);
    }

    private ShellViewModel ShellViewModel => shellViewModel.Value;

    public void Initialize()
    {
        bookLibraryContext = dBContextService.GetBookLibraryContext(out var dataSourcePath);
        entityService.BookLibraryContext = bookLibraryContext;

        ShellViewModel.PropertyChanged += ShellViewModelPropertyChanged;
        ShellViewModel.SaveCommand = saveCommand;
        ShellViewModel.DatabasePath = dataSourcePath;
    }

    public void Shutdown() => bookLibraryContext?.Dispose();

    public bool HasChanges() => bookLibraryContext?.ChangeTracker.HasChanges() == true;

    public bool CanSave() => ShellViewModel.IsValid;

    public bool Save()
    {
        if (!CanSave()) throw new InvalidOperationException("You must not call Save when CanSave returns false.");

        var entities = bookLibraryContext!.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).Select(x => x.Entity).ToArray();
        var errors = entities.OfType<ValidatableModel>().Where(x => x.HasErrors).ToArray();
        if (errors.Any())
        {
            var errorMessages = errors.Select(x => string.Format(CultureInfo.CurrentCulture, Resources.EntityInvalid, EntityToString(x), string.Join(Environment.NewLine, x.Errors)));
            messageService.ShowError(shellService.ShellView, Resources.SaveErrorInvalidEntities, string.Join(Environment.NewLine, errorMessages));
            return false;
        }

        bookLibraryContext.SaveChanges();
        return true;
    }

    private void ShellViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ShellViewModel.IsValid)) saveCommand.RaiseCanExecuteChanged();
    }

    internal static string EntityToString(object entity)
    {
        return entity is IFormattable formattableEntity ? formattableEntity.ToString(null, CultureInfo.CurrentCulture) : entity.ToString() ?? "";
    }
}
