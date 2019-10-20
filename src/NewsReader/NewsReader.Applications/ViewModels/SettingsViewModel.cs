using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        private readonly IMessageService messageService;
        private DisplayItemLifetime selectedItemLifetime;
        private DisplayMaxItemsLimit selectedMaxItemsLimit;
        private FeedManager feedManager;
        private bool setSelectedItemLifetimeRunning;
        private bool setSelectedMaxItemsLimitRunning;
        private bool developerSettingsEnabled;
        private string selectedLanguage;

        public SettingsViewModel(ISettingsView view, IMessageService messageService, IWebStorageService webStorageService, 
            IAppInfoService appInfoService) : base(view)
        {
            this.messageService = messageService;
            AppInfo = appInfoService;
            WebStorageService = webStorageService;
            ItemLifetimes = Enum.GetValues(typeof(DisplayItemLifetime)).Cast<DisplayItemLifetime>().ToArray();
            MaxItemsLimits = Enum.GetValues(typeof(DisplayMaxItemsLimit)).Cast<DisplayMaxItemsLimit>().ToArray();
        }

        public IAppInfoService AppInfo { get; }

        public IWebStorageService WebStorageService { get; }

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

        public ICommand SignInCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        public ICommand EnableDeveloperSettingsCommand { get; set; }

        public bool DeveloperSettingsEnabled
        {
            get => developerSettingsEnabled;
            set => SetProperty(ref developerSettingsEnabled, value);
        }

        public IReadOnlyList<string> Languages { get; set; }

        public string SelectedLanguage
        {
            get => selectedLanguage;
            set => SetProperty(ref selectedLanguage, value);
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
                || await messageService.ShowYesNoQuestion(Resources.ReduceFeedItemLifetimeQuestion);
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
                || await messageService.ShowYesNoQuestion(Resources.ReduceMaxItemsLimitQuestion);
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
