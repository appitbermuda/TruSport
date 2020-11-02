using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using OnTrackWebService.Models;

namespace OnTrackWebService.Data
{
    public class RssClient
    {
        public static async Task<IEnumerable<RssFeedItem>> LoadBernews(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    return document.Root.Descendants()
                        .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseBerNewsItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadIStats(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    //var item = 

                    return document.Root.Descendants()
                                       .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseIStatsItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadCricketIStats(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    //var item = 

                    return document.Root.Descendants()
                                       .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseIStatsCricketItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadFootballIStats(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    //var item = 

                    return document.Root.Descendants()
                                       .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseIStatsFootballItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadBowlingIStats(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    //var item = 

                    return document.Root.Descendants()
                                       .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseIStatsBowlingItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadRG(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    return document.Root.Descendants()
                        .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseRGItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadCricketRG(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    return document.Root.Descendants()
                        .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseRGCricketItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadBowlingRG(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    return document.Root.Descendants()
                        .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseRGBowlingItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        public static async Task<IEnumerable<RssFeedItem>> LoadFootballRG(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var rssFeed = await client.GetStringAsync(uri);

                    var stream = new StringReader(rssFeed);

                    //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    //xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    var xmlReaderSettings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse,
                        //XmlResolver = null
                    };

                    var reader = XmlReader.Create(stream, xmlReaderSettings);
                    var document = XDocument.Load(reader);

                    return document.Root.Descendants()
                        .Where(x => x.Name.LocalName == "item")
                        .Select(x => ParseRGFootballItem(x));
                }
                catch (Exception ex)
                {
                    return new List<RssFeedItem>();
                }
            }
        }

        private static RssFeedItem ParseBerNewsItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {

                var title = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "title").Value);
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var linkString = item.Descendants().Single(x => x.Name.LocalName == "link").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = item.Descendants().Single(x => x.Name.LocalName == "creator").Value;
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();

                var date = DateTime.Parse(dateString);

                //if (categoryString != null && categoryString != "")
                //{
                //    if (categoryString.ToLower() == "soccer")
                //        return new RssFeedItem(HttpUtility.HtmlDecode(title), HttpUtility.HtmlDecode(description), link, creator, date);
                //    else
                //        return null;
                //}
                //else
                //{
                return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                //}
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseIStatsItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {
                var title = RemoveHtmlTags(item.Descendants().FirstOrDefault(x => x.Name.LocalName == "title").Value);
                var linkString = item.Descendants().FirstOrDefault(x => x.Name.LocalName == "link").Value;
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Island Stats";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();

                //var date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                DateTime date = new DateTime();
                try
                {
                    date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                }
                catch (Exception ex)
                { }


                //if (categoryString != null && categoryString != "")
                //{
                //    return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                //}
                //else
                //{
                    return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                //}
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseIStatsCricketItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {
                var title = RemoveHtmlTags(item.Descendants().FirstOrDefault(x => x.Name.LocalName == "title").Value);
                var linkString = item.Descendants().FirstOrDefault(x => x.Name.LocalName == "link").Value;
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Island Stats";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();

                //var date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                DateTime date = new DateTime();
                try
                {
                    date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                }
                catch (Exception ex)
                { }


                if (categoryString != null && categoryString != "")
                {
                    if (categoryString.ToLower() == "cricket")
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                    else
                        return null;
                }
                else
                {
                    return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseIStatsFootballItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {
                var title = RemoveHtmlTags(item.Descendants().FirstOrDefault(x => x.Name.LocalName == "title").Value);
                var linkString = item.Descendants().FirstOrDefault(x => x.Name.LocalName == "link").Value;
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Island Stats";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();

                //var date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                DateTime date = new DateTime();
                try
                {
                    date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                }
                catch (Exception ex)
                { }


                if (categoryString != null && categoryString != "")
                {
                    if (categoryString.ToLower() == "soccer")
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                    else
                        return null;
                }
                else
                {
                    return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseIStatsBowlingItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {
                var title = RemoveHtmlTags(item.Descendants().FirstOrDefault(x => x.Name.LocalName == "title").Value);
                var linkString = item.Descendants().FirstOrDefault(x => x.Name.LocalName == "link").Value;
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Island Stats";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();

                //var date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                DateTime date = new DateTime();
                try
                {
                    date = DateTime.Parse(dateString.Replace("EST", "").TrimEnd());
                }
                catch (Exception ex)
                { }


                if (categoryString != null && categoryString != "")
                {
                    if (categoryString.ToLower() == "bowling")
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                    else
                        return null;
                }
                else
                {
                    return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, date);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseRGItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {

                var title = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "title").Value);
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var linkString = item.Descendants().Single(x => x.Name.LocalName == "link").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Royal Gazette";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                //var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();


                DateTime newsDate = new DateTime();
                var date = DateTime.TryParse(dateString.Replace("EST", "").TrimEnd(), out newsDate);

                if (linkString != null && linkString != "")
                {
                    if (linkString.ToLower().Contains("sport"))
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, newsDate);
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseRGCricketItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {

                var title = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "title").Value);
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var linkString = item.Descendants().Single(x => x.Name.LocalName == "link").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Royal Gazette";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                //var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();


                DateTime newsDate = new DateTime();
                var date = DateTime.TryParse(dateString.Replace("EST", "").TrimEnd(), out newsDate);

                if (linkString != null && linkString != "")
                {
                    if (linkString.ToLower().Contains("cricket"))
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, newsDate);
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseRGBowlingItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {

                var title = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "title").Value);
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var linkString = item.Descendants().Single(x => x.Name.LocalName == "link").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Royal Gazette";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                //var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();


                DateTime newsDate = new DateTime();
                var date = DateTime.TryParse(dateString.Replace("EST", "").TrimEnd(), out newsDate);

                if (linkString != null && linkString != "")
                {
                    if (linkString.ToLower().Contains("bowling"))
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, newsDate);
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static RssFeedItem ParseRGFootballItem(XElement item)
        {
            string RemoveHtmlTags(string html)
            {
                return Regex.Replace(html, "<[^>]+>", string.Empty);
            }

            try
            {

                var title = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "title").Value);
                //var image = item.Descendants().Single(x => x.Name.LocalName == "url").Value;
                var linkString = item.Descendants().Single(x => x.Name.LocalName == "link").Value;
                var description = RemoveHtmlTags(item.Descendants().Single(x => x.Name.LocalName == "description").Value);
                var creatorString = "Royal Gazette";
                var dateString = item.Descendants().Single(x => x.Name.LocalName == "pubDate").Value;
                //var categoryString = item.Descendants().Single(x => x.Name.LocalName == "category").Value;

                var link = new Uri(linkString);

                var creatorSplit = creatorString.Split(new[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
                var creator = creatorSplit.First().Trim();
                //var creatorEMail = creatorSplit.Skip(1).Single().Trim();


                DateTime newsDate = new DateTime();
                var date = DateTime.TryParse(dateString.Replace("EST", "").TrimEnd(), out newsDate);

                if (linkString != null && linkString != "")
                {
                    if (linkString.ToLower().Contains("soccer"))
                        return new RssFeedItem(HttpUtility.HtmlDecode(title), "", HttpUtility.HtmlDecode(description), link, creator, newsDate);
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
