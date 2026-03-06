using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services;

public class NetworkInfoService : Model, INetworkInfoService
{
    public NetworkInfoService()
    {
        Connectivity.ConnectivityChanged += ConnectivityChanged;
        InternetAccess = Connectivity.NetworkAccess == NetworkAccess.Internet;
    }

    public bool InternetAccess { get; set => SetProperty(ref field, value); }

    private void ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => InternetAccess = e.NetworkAccess == NetworkAccess.Internet);
    }
}
