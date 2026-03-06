using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels;

public class AddEditFeedViewModel : ViewModelCore<IAddEditFeedView>
{
    private readonly DelegateCommand useTitleAsNameCommand;

    public AddEditFeedViewModel(IAddEditFeedView view) : base(view, false)
    {
        useTitleAsNameCommand = new(() => Feed!.Name = Feed.Title, () => !string.IsNullOrEmpty(Feed?.Title) && Feed!.Name != Feed.Title);
    }

    public ICommand LoadFeedCommand { get; internal set; } = null!;

    public ICommand AddUpdateCommand { get; internal set; } = null!;

    public ICommand UseTitleAsNameCommand => useTitleAsNameCommand;

    public event PropertyChangedEventHandler? FeedChanged;

    public bool IsEditMode { get; set => SetProperty(ref field, value); }

    public string? FeedUrl
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            Feed = null;
            LoadErrorMessage = null;
        }
    }

    public Feed? OldFeed { get; set => SetProperty(ref field, value); }

    public Feed? Feed
    {
        get;
        set
        {
            var oldFeed = field;
            if (!SetProperty(ref field, value)) return;
            oldFeed?.PropertyChanged -= FeedPropertyChanged;
            field?.PropertyChanged += FeedPropertyChanged;
        }
    }

    public string? LoadErrorMessage { get; set => SetProperty(ref field, value); }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName is nameof(Feed)) useTitleAsNameCommand.RaiseCanExecuteChanged();
    }

    private void FeedPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        FeedChanged?.Invoke(sender, e);
        if (e.PropertyName is nameof(Feed.Title) or nameof(Feed.Name)) useTitleAsNameCommand.RaiseCanExecuteChanged();
    }
}
