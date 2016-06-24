using System;
using System.ComponentModel;

namespace Jbe.NewsReader.Domain
{
    public interface IDataManager : INotifyPropertyChanged
    {
        TimeSpan? ItemLifetime { get; }
        
        uint? MaxItemsLimit { get; }
    }
}
