using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using System.Waf.Applications;
using System.ComponentModel.Composition;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailViewModel : ViewModel<IEmailView>
    {
        private Email email;
        

        [ImportingConstructor]
        public EmailViewModel(IEmailView view) : base(view)
        {
        }


        public Email Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
    }
}
