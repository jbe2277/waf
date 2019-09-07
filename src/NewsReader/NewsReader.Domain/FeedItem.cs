using System;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.NewsReader.Domain
{
    [DataContract]
    public class FeedItem : Model
    {
        [DataMember] private readonly Uri uri;
        [DataMember] private DateTimeOffset date;
        [DataMember] private string name;
        [DataMember] private string description;
        [DataMember] private bool markAsRead;


        public FeedItem(Uri uri, DateTimeOffset date, string name, string description)
        {
            // Note: Serializer does not call the constructor.
            this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Date = date;
            Name = name;
            Description = description;
        }


        public Uri Uri => uri;

        public DateTimeOffset Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value?.Truncate(200).Trim());
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value?.Truncate(500).Trim());
        }

        public bool MarkAsRead
        {
            get => markAsRead;
            set => SetProperty(ref markAsRead, value);
        }


        public void ApplyValuesFrom(FeedItem item, bool excludeMarkAsRead = false)
        {
            if (Uri != item.Uri) { throw new InvalidOperationException("The Uri must be the same."); }
            Date = item.Date;
            Name = item.Name;
            Description = item.Description;
            if (!excludeMarkAsRead) { MarkAsRead = item.MarkAsRead; }
        }

        public FeedItem Clone()
        {
            return new FeedItem(Uri, Date, Name, Description)
            {
                MarkAsRead = MarkAsRead
            };
        }
    }
}
