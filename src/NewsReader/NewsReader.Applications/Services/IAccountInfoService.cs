using Jbe.NewsReader.Domain;
using System.ComponentModel;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IAccountInfoService : INotifyPropertyChanged
    {
        UserAccount CurrentAccount { get; }
    }
}
