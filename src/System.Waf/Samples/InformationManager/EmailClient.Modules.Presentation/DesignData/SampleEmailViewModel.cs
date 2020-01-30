using System.Linq;
using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.DesignData
{
    public class SampleEmailViewModel : EmailViewModel
    {
        public SampleEmailViewModel() : this(new MockEmailView())
        {
        }

        public SampleEmailViewModel(IEmailView view) : base(view)
        {
            Email = SampleDataProvider.CreateInboxEmails().First();
        }


        private class MockEmailView : IEmailView
        {
            public object? DataContext { get; set; }
        }
    }
}
