using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData;

public class SampleEmailListViewModel : EmailListViewModel
{
    public SampleEmailListViewModel() : this(new MockEmailListView())
    {
    }

    public SampleEmailListViewModel(IEmailListView view) : base(view)
    {
        var root = new EmailClientRoot();
        foreach (var x in SampleDataProvider.CreateInboxEmails()) root.Inbox.AddEmail(x);
        Emails = root.Inbox.Emails;
        FilterText = "My filter text";
    }


    private sealed class MockEmailListView : IEmailListView
    {
        public object? DataContext { get; set; }

        public void FocusItem() { }
    }
}
