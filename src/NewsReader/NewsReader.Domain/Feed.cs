using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.NewsReader.Domain
{
    [DataContract]
    public class Feed : ValidatableModel
    {
        [DataMember] private readonly Uri uri;
        [DataMember] private readonly ObservableCollection<FeedItem> items;
        [DataMember] private string name;
        private string title;
        private ReadOnlyObservableList<FeedItem> readOnlyItems;
        private int unreadItemsCount;
        private bool isLoading;
        private Exception loadError;
        private string loadErrorMessage;
        private IDataManager dataManager;

        public Feed(Uri uri)
        {
            // Note: Serializer does not call the constructor.
            this.uri = uri;
            items = new ObservableCollection<FeedItem>();
            Initialize();
        }

        public Uri Uri => uri;

        [Required]
        public string Name
        {
            get => name;
            set => SetPropertyAndValidate(ref name, value);
        }

        public string Title
        {
            get => title;
            set
            {
                if (SetProperty(ref title, value))
                {
                    if (string.IsNullOrEmpty(Name)) Name = value;
                }
            }
        }

        public IReadOnlyObservableList<FeedItem> Items => readOnlyItems ?? (readOnlyItems = new ReadOnlyObservableList<FeedItem>(items));
        
        public int UnreadItemsCount
        {
            get => unreadItemsCount;
            private set => SetProperty(ref unreadItemsCount, value);
        }
        
        public bool IsLoading
        {
            get => isLoading;
            private set => SetProperty(ref isLoading, value);
        }

        public Exception LoadError
        {
            get => loadError;
            private set => SetProperty(ref loadError, value);
        }
        
        public string LoadErrorMessage
        {
            get => loadErrorMessage;
            private set => SetProperty(ref loadErrorMessage, value);
        }

        internal IDataManager DataManager
        {
            get => dataManager;
            set
            {
                if (dataManager == value) { return; }

                if (dataManager != null)
                {
                    dataManager.PropertyChanged -= DataManagerPropertyChanged;
                }
                dataManager = value;
                if (dataManager != null)
                {
                    dataManager.PropertyChanged += DataManagerPropertyChanged;
                    TrimItemsList();
                }
            }
        }

        public void StartLoading()
        {
            IsLoading = true;
            LoadError = null;
            LoadErrorMessage = null;
        }

        public void UpdateItems(IReadOnlyList<FeedItem> newFeedItems, bool excludeMarkAsRead = false, bool cloneItemsBeforeInsert = false)
        {
            foreach (var item in newFeedItems)
            {
                var foundItem = items.FirstOrDefault(x => x.Uri == item.Uri && x.Uri.Fragment == item.Uri.Fragment);
                if (foundItem != null)
                {
                    foundItem.ApplyValuesFrom(item, excludeMarkAsRead);
                }
                else
                {
                    int i;
                    for (i = 0; i < items.Count; i++)
                    {
                        if (item.Date > items[i].Date)
                        {
                            break;
                        }
                    }
                    var itemToInsert = cloneItemsBeforeInsert ? item.Clone() : item;
                    items.Insert(i, itemToInsert);
                }
            }
            if (DataManager != null)
            {
                TrimItemsList();
            }
            IsLoading = false;
        }

        public void SetLoadError(Exception loadError, string loadErrorMessage)
        {
            LoadError = loadError;
            LoadErrorMessage = loadErrorMessage;
            IsLoading = false;
        }

        private void Initialize()
        {
            items.CollectionChanged += ItemsCollectionChanged;
            foreach (var item in items)
            {
                item.PropertyChanged += FeedItemPropertyChanged;
            }
            UpdateUnreadItemsCount();
            Validate();
        }

        private void DataManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TrimItemsList();
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.OldItems?.Cast<FeedItem>() ?? Enumerable.Empty<FeedItem>())
            {
                item.PropertyChanged -= FeedItemPropertyChanged;
            }
            foreach (var item in e.NewItems?.Cast<FeedItem>() ?? Enumerable.Empty<FeedItem>())
            {
                item.PropertyChanged += FeedItemPropertyChanged;
            }
            UpdateUnreadItemsCount();
        }

        private void FeedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FeedItem.MarkAsRead))
            {
                UpdateUnreadItemsCount();
            }
        }
        
        private void TrimItemsList()
        {
            uint i = 0;
            var minDate = DateTimeOffset.Now - DataManager.ItemLifetime;
            foreach (var item in items.ToArray())
            {
                if (i >= DataManager.MaxItemsLimit
                    || item.Date < minDate)
                {
                    items.Remove(item);
                }
                i++;
            }
        }

        private void UpdateUnreadItemsCount()
        {
            UnreadItemsCount = Items.Count(x => !x.MarkAsRead);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Initialize();
        }
    }
}
