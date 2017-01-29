using Jbe.NewsReader.Applications.Views;
using System.Collections.Generic;
using System.Composition;
using System.Waf.Applications;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class DeveloperSettingsViewModel : ViewModelCore<IDeveloperSettingsView>
    {
        private string selectedLanguage;


        [ImportingConstructor]
        public DeveloperSettingsViewModel(IDeveloperSettingsView view) : base(view)
        {
        }


        public IReadOnlyList<string> Languages { get; set; }

        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set { SetProperty(ref selectedLanguage, value); }
        }
    }
}
