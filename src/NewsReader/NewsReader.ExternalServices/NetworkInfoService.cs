using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Foundation;
using Windows.Networking.Connectivity;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(INetworkInfoService)), Shared]
    internal sealed class NetworkInfoService : Model, INetworkInfoService, IDisposable
    {
        private readonly TaskScheduler taskScheduler;
        private bool internetAccess;


        public NetworkInfoService()
        {
            taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            UpdateInternetAccess();
            NetworkInformation.NetworkStatusChanged += NetworkInformationNetworkStatusChanged;
        }
        

        public bool InternetAccess
        {
            get { return internetAccess; }
            set { SetProperty(ref internetAccess, value); }
        }


        public void Dispose()
        {
            NetworkInformation.NetworkStatusChanged -= NetworkInformationNetworkStatusChanged;
        }

        private void UpdateInternetAccess()
        {
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            InternetAccess = connectionProfile?.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

        private void NetworkInformationNetworkStatusChanged(object sender)
        {
            Task.Factory.StartNew(UpdateInternetAccess, CancellationToken.None, TaskCreationOptions.DenyChildAttach, taskScheduler);
        }
    }
}
