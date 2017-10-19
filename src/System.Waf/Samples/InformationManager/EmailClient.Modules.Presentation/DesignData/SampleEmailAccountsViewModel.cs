using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData
{
    public class SampleEmailAccountsViewModel : EmailAccountsViewModel
    {
        public SampleEmailAccountsViewModel() : base(new MockEmailAccountsView())
        {
            var root = new EmailClientRoot();
            root.AddEmailAccount(SampleDataProvider.CreateEmailAccount());
            EmailClientRoot = root;
        }
        

        private class MockEmailAccountsView : IEmailAccountsView
        {
            public object DataContext { get; set; }
            
            public void ShowDialog(object owner) { }
        }
    }
}
