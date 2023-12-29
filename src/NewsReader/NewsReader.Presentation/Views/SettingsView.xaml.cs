using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Presentation.Views;

public partial class SettingsView : TabbedPage, ISettingsView
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private async void ShareLogFileClicked(object sender, EventArgs e)
    {
        try
        {
            await Share.RequestAsync(new ShareFileRequest(new ShareFile(App.LogFileName)));
        }
        catch (Exception ex)
        {
            Log.Default.TrackError(ex, "ShareLogFileClicked");
        }
    }
}