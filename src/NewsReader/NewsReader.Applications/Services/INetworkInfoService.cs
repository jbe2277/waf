using System.ComponentModel;

namespace Jbe.NewsReader.Applications.Services
{
    public interface INetworkInfoService : INotifyPropertyChanged
    {
        bool InternetAccess { get; }
    }
}
