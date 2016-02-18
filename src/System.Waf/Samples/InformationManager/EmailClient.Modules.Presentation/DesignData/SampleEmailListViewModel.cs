using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData
{
    public class SampleEmailListViewModel : EmailListViewModel
    {
        public SampleEmailListViewModel() : base(new MockEmailListView())
        {
            EmailClientRoot root = new EmailClientRoot();
            foreach (var email in SampleDataProvider.CreateInboxEmails()) { root.Inbox.AddEmail(email); }
            Emails = root.Inbox.Emails;
            FilterText = "My filter text";
        }

        
        private class MockEmailListView : IEmailListView
        {
            public object DataContext { get; set; }

            public void FocusItem() { }
        }
    }
}
