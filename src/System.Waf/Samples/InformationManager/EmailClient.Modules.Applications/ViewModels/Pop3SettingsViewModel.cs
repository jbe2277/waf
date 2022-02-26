using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Foundation;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class Pop3SettingsViewModel : ViewModel<IPop3SettingsView>
    {
        private Pop3Settings model = null!;
        private bool useSameUserCredits;

        [ImportingConstructor]
        public Pop3SettingsViewModel(IPop3SettingsView view) : base(view)
        {
        }

        public Pop3Settings Model
        {
            get => model;
            set
            {
                if (!SetProperty(ref model, value)) return;
                WeakEvent.PropertyChanged.Add(model.Pop3UserCredits, Pop3UserCreditsPropertyChanged);
            }
        }

        public bool UseSameUserCredits
        {
            get => useSameUserCredits;
            set
            {
                if (!SetProperty(ref useSameUserCredits, value)) return;
                if (useSameUserCredits)
                {
                    Model.SmtpUserCredits.UserName = Model.Pop3UserCredits.UserName;
                    Model.SmtpUserCredits.Password = Model.Pop3UserCredits.Password;
                }
            }
        }

        private void Pop3UserCreditsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserCredits.UserName))
            {
                if (UseSameUserCredits) Model.SmtpUserCredits.UserName = Model.Pop3UserCredits.UserName;
            }
            else if (e.PropertyName == nameof(UserCredits.Password))
            {
                if (UseSameUserCredits) Model.SmtpUserCredits.Password = Model.Pop3UserCredits.Password;
            }
        }
    }
}
