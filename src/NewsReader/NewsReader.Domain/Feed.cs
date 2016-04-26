using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain
{
    [DataContract]
    public class Feed : Model
    {
        [DataMember] private readonly Uri uri;
        [DataMember] private readonly ObservableCollection<FeedItem> items;
        [DataMember] private string name;
        private ReadOnlyObservableList<FeedItem> readOnlyItems;
        private bool isLoading;
        private Exception loadError;


        public Feed(Uri uri)
        {
            // Note: Serializer does not call the constructor.
            this.uri = uri;
            this.items = new ObservableCollection<FeedItem>();
            this.isLoading = true;
        }


        public Uri Uri => uri;

        public string Name
        {
            get { return name ?? (name = uri.ToString()); }
            set { SetProperty(ref name, value); }
        }

        public IReadOnlyObservableList<FeedItem> Items => readOnlyItems ?? (readOnlyItems = new ReadOnlyObservableList<FeedItem>(items));

        public bool IsLoading
        {
            get { return isLoading; }
            private set { SetProperty(ref isLoading, value); }
        }

        public Exception LoadError
        {
            get { return loadError; }
            set
            {
                if (SetProperty(ref loadError, value))
                {
                    IsLoading = false;
                }
            }
        }


        public void UpdateItems(IReadOnlyList<FeedItem> newFeedItems)
        {
            foreach (var item in newFeedItems)
            {
                var foundItem = items.FirstOrDefault(x => x.Uri == item.Uri);
                if (foundItem != null)
                {
                    foundItem.ApplyValuesFrom(item);
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
                    items.Insert(i, item);
                }
            }
            IsLoading = false;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            isLoading = true;
        }
    }
}
