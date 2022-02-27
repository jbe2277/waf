using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Presentation.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData;

public class SampleEmailLayoutViewModel : EmailLayoutViewModel
{
    public SampleEmailLayoutViewModel() : base(new MockEmailLayoutView())
    {
        EmailListView = new SampleEmailListViewModel(new EmailListView()).View;
        EmailView = new SampleEmailViewModel(new EmailView()).View;
    }


    private class MockEmailLayoutView : IEmailLayoutView
    {
        public object? DataContext { get; set; }
    }
}
