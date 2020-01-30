using System.Linq;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData
{
    public class SampleNewEmailViewModel : NewEmailViewModel
    {
        public SampleNewEmailViewModel() : base(new MockNewEmailView())
        {
            EmailAccounts = new[] { SampleDataProvider.CreateEmailAccount() };
            SelectedEmailAccount = EmailAccounts.First();
            Email = SampleDataProvider.CreateSentEmails().ElementAt(1);
        }
        

        private class MockNewEmailView : INewEmailView
        {
            public object? DataContext { get; set; }
            
            public void Show(object owner) { }
            
            public void Close() { }
        }
    }
}
