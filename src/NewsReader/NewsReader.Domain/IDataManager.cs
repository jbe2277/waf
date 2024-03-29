﻿namespace Waf.NewsReader.Domain;

public interface IDataManager : INotifyPropertyChanged
{
    TimeSpan? ItemLifetime { get; }

    uint? MaxItemsLimit { get; }
}
