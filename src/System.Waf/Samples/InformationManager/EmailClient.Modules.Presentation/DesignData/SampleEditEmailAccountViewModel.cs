using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Presentation.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData;

public class SampleEditEmailAccountViewModel : EditEmailAccountViewModel
{
    public SampleEditEmailAccountViewModel() : base(new MockEditEmailAccountView())
    {
        ContentView = new SampleBasicEmailAccountViewModel(new BasicEmailAccountView()).View;
    }


    private sealed class MockEditEmailAccountView : IEditEmailAccountView
    {
        public object? DataContext { get; set; }

        public void Close() { }

        public void ShowDialog(object owner) { }
    }
}
