using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.Applications;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class InfoSettingsViewModel : ViewModel<IInfoSettingsView>
    {
        [ImportingConstructor]
        public InfoSettingsViewModel(IInfoSettingsView view) : base(view)
        {
        }
    }
}
