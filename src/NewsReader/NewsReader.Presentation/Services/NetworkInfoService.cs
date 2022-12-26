using System.Waf.Foundation;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.Presentation.Services
{
    public class NetworkInfoService : Model, INetworkInfoService
    {
        private bool internetAccess;

        public NetworkInfoService()
        {
            Connectivity.ConnectivityChanged += ConnectivityChanged;
            internetAccess = Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public bool InternetAccess
        {
            get => internetAccess;
            set => SetProperty(ref internetAccess, value);
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                InternetAccess = e.NetworkAccess == NetworkAccess.Internet;
            });
        }
    }
}
