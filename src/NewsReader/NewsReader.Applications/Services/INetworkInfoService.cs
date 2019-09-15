using System.ComponentModel;

namespace Waf.NewsReader.Applications.Services
{
    public interface INetworkInfoService : INotifyPropertyChanged
    {
        bool InternetAccess { get; }
    }
}
