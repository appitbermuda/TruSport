using System;
namespace OnTrackWebService.Models
{
    public class RssFeedItem
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public Uri Link { get; private set; }
        public string Creator { get; private set; }
        public string CreatorEmail { get; private set; }
        public DateTime Date { get; private set; }

        public RssFeedItem(string title, string image, string description, Uri link, string creator, DateTime date)
        {
            Title = title;
            Image = image;
            Description = description;
            Link = link;
            Creator = creator;
            Date = date;
        }
    }
}
