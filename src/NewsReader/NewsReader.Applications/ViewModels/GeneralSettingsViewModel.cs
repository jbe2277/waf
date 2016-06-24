using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Domain;
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
        private DisplayItemLifetime selectedItemLifetime;
        private DisplayMaxItemsLimit selectedMaxItemsLimit;
        private FeedManager feedManager;


        [ImportingConstructor]
        public GeneralSettingsViewModel(IGeneralSettingsView view) : base(view)
        {
            AppThemes = Enum.GetValues(typeof(DisplayAppTheme)).Cast<DisplayAppTheme>().ToArray();
            ItemLifetimes = Enum.GetValues(typeof(DisplayItemLifetime)).Cast<DisplayItemLifetime>().ToArray();
            MaxItemsLimits = Enum.GetValues(typeof(DisplayMaxItemsLimit)).Cast<DisplayMaxItemsLimit>().ToArray();
        }


        public IReadOnlyList<DisplayAppTheme> AppThemes { get; }

        public DisplayAppTheme SelectedAppTheme
        {
            get { return selectedAppTheme; }
            set { SetProperty(ref selectedAppTheme, value); }
        }

        public IReadOnlyList<DisplayItemLifetime> ItemLifetimes { get; }
        
        public DisplayItemLifetime SelectedItemLifetime
        {
            get { return selectedItemLifetime; }
            set
            {
                if (SetProperty(ref selectedItemLifetime, value))
                {
                    FeedManager.ItemLifetime = value.ToValue();
                }
            }
        }

        public IReadOnlyList<DisplayMaxItemsLimit> MaxItemsLimits { get; }
        
        public DisplayMaxItemsLimit SelectedMaxItemsLimit
        {
            get { return selectedMaxItemsLimit; }
            set
            {
                if (SetProperty(ref selectedMaxItemsLimit, value))
                {
                    FeedManager.MaxItemsLimit = value.ToValue();
                }
            }
        }

        internal FeedManager FeedManager
        {
            get { return feedManager; }
            set
            {
                feedManager = value;
                SelectedItemLifetime = DisplayItemLifetimeHelper.FromValue(feedManager.ItemLifetime);
                SelectedMaxItemsLimit = DisplayMaxItemsLimitHelper.FromValue(feedManager.MaxItemsLimit);
            }
        }
    }
}
