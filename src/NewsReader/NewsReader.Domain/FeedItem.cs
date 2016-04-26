using System;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Jbe.NewsReader.Domain
{
    [DataContract]
    public class FeedItem : Model
    {
        [DataMember] private readonly Uri uri;
        [DataMember] private DateTimeOffset date;
        [DataMember] private string name;
        [DataMember] private string description;
        [DataMember] private string author;
        [DataMember] private bool markAsRead;


        public FeedItem(Uri uri, DateTimeOffset date, string name, string description, string author)
        {
            // Note: Serializer does not call the constructor.
            if (uri == null) { throw new ArgumentNullException(nameof(uri)); }
            this.uri = uri;
            Date = date;
            Name = name;
            Description = description;
            Author = author;
        }


        public Uri Uri => uri;

        public DateTimeOffset Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, (value ?? "").Truncate(200).Trim()); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, (value ?? "").Truncate(500).Trim()); }
        }

        public string Author
        {
            get { return author; }
            set { SetProperty(ref author, (value ?? "").Truncate(100).Trim()); }
        }

        public bool MarkAsRead
        {
            get { return markAsRead; }
            set { SetProperty(ref markAsRead, value); }
        }


        public void ApplyValuesFrom(FeedItem item)
        {
            if (Uri != item.Uri) { throw new InvalidOperationException("The Uri must be the same."); }
            Date = item.Date;
            Name = item.Name;
            Description = item.Description;
            Author = item.Author;
        }
    }
}
