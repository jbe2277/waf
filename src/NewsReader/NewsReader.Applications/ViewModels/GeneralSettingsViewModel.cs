using Jbe.NewsReader.Applications.Views;
using System.Composition;
using System.Waf.Applications;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class GeneralSettingsViewModel : ViewModel<IGeneralSettingsView>
    {
        [ImportingConstructor]
        public GeneralSettingsViewModel(IGeneralSettingsView view) : base(view)
        {
        }
    }
}
