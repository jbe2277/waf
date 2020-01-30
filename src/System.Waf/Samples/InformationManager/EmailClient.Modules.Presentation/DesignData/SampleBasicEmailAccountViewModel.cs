using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData
{
    public class SampleBasicEmailAccountViewModel : BasicEmailAccountViewModel
    {
        public SampleBasicEmailAccountViewModel() : this(new MockBasicEmailAccountView())
        {
        }

        public SampleBasicEmailAccountViewModel(IBasicEmailAccountView view) : base(view)
        {
            EmailAccount = SampleDataProvider.CreateEmailAccount();
        }


        private class MockBasicEmailAccountView : IBasicEmailAccountView
        {
            public object? DataContext { get; set; }
        }
    }
}
