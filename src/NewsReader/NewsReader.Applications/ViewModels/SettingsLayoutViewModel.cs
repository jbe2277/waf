using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using System.Waf.Applications;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class SettingsLayoutViewModel : ViewModelCore<ISettingsLayoutView>
    {
        private bool developerSettingsEnabled;


        [ImportingConstructor]
        public SettingsLayoutViewModel(ISettingsLayoutView view) : base(view)
        {
        }


        public Lazy<object> LazyGeneralSettingsView { get; set; }
        
        public Lazy<object> LazyInfoSettingsView { get; set; }

        public Lazy<object> LazyDeveloperSettingsView { get; set; }
        
        public bool DeveloperSettingsEnabled
        {
            get => developerSettingsEnabled;
            set => SetProperty(ref developerSettingsEnabled, value);
        }
    }
}
