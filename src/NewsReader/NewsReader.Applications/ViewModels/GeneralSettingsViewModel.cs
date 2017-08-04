using Jbe.NewsReader.Applications.Services;
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
    public class GeneralSettingsViewModel : ViewModelCore<IGeneralSettingsView>
    {
        private readonly IResourceService resourceService;
        private readonly IMessageService messageService;
        private DisplayAppTheme selectedAppTheme;
        private DisplayItemLifetime selectedItemLifetime;
        private DisplayMaxItemsLimit selectedMaxItemsLimit;
        private FeedManager feedManager;
        private bool setSelectedItemLifetimeRunning;
        private bool setSelectedMaxItemsLimitRunning;


        [ImportingConstructor]
        public GeneralSettingsViewModel(IResourceService resourceService, IMessageService messageService, IGeneralSettingsView view) : base(view)
        {
            this.resourceService = resourceService;
            this.messageService = messageService;
            AppThemes = Enum.GetValues(typeof(DisplayAppTheme)).Cast<DisplayAppTheme>().ToArray();
            ItemLifetimes = Enum.GetValues(typeof(DisplayItemLifetime)).Cast<DisplayItemLifetime>().ToArray();
            MaxItemsLimits = Enum.GetValues(typeof(DisplayMaxItemsLimit)).Cast<DisplayMaxItemsLimit>().ToArray();
        }


        public IReadOnlyList<DisplayAppTheme> AppThemes { get; }

        public DisplayAppTheme SelectedAppTheme
        {
            get => selectedAppTheme;
            set => SetProperty(ref selectedAppTheme, value);
        }

        public IReadOnlyList<DisplayItemLifetime> ItemLifetimes { get; }
        
        public DisplayItemLifetime SelectedItemLifetime
        {
            get => selectedItemLifetime;
            set => SetSelectedItemLifetime(value);
        }

        public IReadOnlyList<DisplayMaxItemsLimit> MaxItemsLimits { get; }

        public DisplayMaxItemsLimit SelectedMaxItemsLimit
        {
            get => selectedMaxItemsLimit;
            set => SetSelectedMaxItemsLimit(value);
        }
        
        internal FeedManager FeedManager
        {
            get => feedManager;
            set
            {
                feedManager = value;
                selectedItemLifetime = DisplayItemLifetimeHelper.FromValue(feedManager.ItemLifetime);
                selectedMaxItemsLimit = DisplayMaxItemsLimitHelper.FromValue(feedManager.MaxItemsLimit);
            }
        }


        private async void SetSelectedItemLifetime(DisplayItemLifetime value)
        {
            if (selectedItemLifetime == value) { return; }

            if (setSelectedItemLifetimeRunning) { return; }
            setSelectedItemLifetimeRunning = true;

            var result = selectedItemLifetime < value
                || await messageService.ShowYesNoQuestionDialogAsync(resourceService.GetString("ReduceFeedItemLifetimeQuestion"));
            if (result)
            {
                selectedItemLifetime = value;
                FeedManager.ItemLifetime = value.ToValue();
            }

            RaisePropertyChanged(nameof(SelectedItemLifetime));
            setSelectedItemLifetimeRunning = false;
        }
        
        private async void SetSelectedMaxItemsLimit(DisplayMaxItemsLimit value)
        {
            if (selectedMaxItemsLimit == value) { return; }

            if (setSelectedMaxItemsLimitRunning) { return; }
            setSelectedMaxItemsLimitRunning = true;

            var result = selectedMaxItemsLimit < value
                || await messageService.ShowYesNoQuestionDialogAsync(resourceService.GetString("ReduceMaxItemsLimitQuestion"));
            if (result)
            {
                selectedMaxItemsLimit = value;
                FeedManager.MaxItemsLimit = value.ToValue();
            }

            RaisePropertyChanged(nameof(SelectedMaxItemsLimit));
            setSelectedMaxItemsLimitRunning = false;
        }
    }
}
