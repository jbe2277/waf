using System.Globalization;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Properties;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels;

public class SettingsViewModel(ISettingsView view) : ViewModelCore<ISettingsView>(view, false)
{
    private DisplayItemLifetime selectedItemLifetime;
    private DisplayMaxItemsLimit selectedMaxItemsLimit;
    private FeedManager feedManager = null!;
    private bool setSelectedItemLifetimeRunning;
    private bool setSelectedMaxItemsLimitRunning;
    private bool developerSettingsEnabled;
    private string selectedLanguage = null!;

    public required IAppInfoService AppInfo { get; init; }

    public required IWebStorageService WebStorageService { get; init; }

    public required IMessageService MessageService { protected get; init; }

    public IReadOnlyList<DisplayItemLifetime> ItemLifetimes { get; } = Enum.GetValues<DisplayItemLifetime>();

    public DisplayItemLifetime SelectedItemLifetime { get => selectedItemLifetime; set => SetSelectedItemLifetime(value); }

    public IReadOnlyList<DisplayMaxItemsLimit> MaxItemsLimits { get; } = Enum.GetValues<DisplayMaxItemsLimit>();

    public DisplayMaxItemsLimit SelectedMaxItemsLimit { get => selectedMaxItemsLimit; set => SetSelectedMaxItemsLimit(value); }

    public ICommand SignInCommand { get; set; } = null!;

    public ICommand SignOutCommand { get; set; } = null!;

    public ICommand EnableDeveloperSettingsCommand { get; set; } = null!;

    public bool DeveloperSettingsEnabled { get => developerSettingsEnabled; set => SetProperty(ref developerSettingsEnabled, value); }

    public IReadOnlyList<string> Languages { get; set; } = null!;

    public string SelectedLanguage { get => selectedLanguage; set => SetProperty(ref selectedLanguage, value); }

    public string UICulture => CultureInfo.CurrentUICulture.ToString();

    public string Culture => CultureInfo.CurrentCulture.ToString();

    public DateTime Now => DateTime.Now;

    public double CurrencyValue => 42800.90;

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
        if (selectedItemLifetime == value) return;
        if (setSelectedItemLifetimeRunning) return;
        setSelectedItemLifetimeRunning = true;

        var result = selectedItemLifetime < value || await MessageService.ShowYesNoQuestion(Resources.ReduceFeedItemLifetimeQuestion);
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
        if (selectedMaxItemsLimit == value) return;
        if (setSelectedMaxItemsLimitRunning) return;
        setSelectedMaxItemsLimitRunning = true;

        var result = selectedMaxItemsLimit < value || await MessageService.ShowYesNoQuestion(Resources.ReduceMaxItemsLimitQuestion);
        if (result)
        {
            selectedMaxItemsLimit = value;
            FeedManager.MaxItemsLimit = value.ToValue();
        }

        RaisePropertyChanged(nameof(SelectedMaxItemsLimit));
        setSelectedMaxItemsLimitRunning = false;
    }
}
