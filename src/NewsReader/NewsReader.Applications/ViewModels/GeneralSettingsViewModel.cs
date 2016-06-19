using Jbe.NewsReader.Applications.Views;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Waf.Applications;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class GeneralSettingsViewModel : ViewModel<IGeneralSettingsView>
    {
        private DisplayAppTheme selectedAppTheme;


        [ImportingConstructor]
        public GeneralSettingsViewModel(IGeneralSettingsView view) : base(view)
        {
            AppThemes = Enum.GetValues(typeof(DisplayAppTheme)).Cast<DisplayAppTheme>().ToArray();
        }


        public IReadOnlyList<DisplayAppTheme> AppThemes { get; }

        public DisplayAppTheme SelectedAppTheme
        {
            get { return selectedAppTheme; }
            set { SetProperty(ref selectedAppTheme, value); }
        }
    }
}
