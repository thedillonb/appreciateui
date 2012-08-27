using System;
using System.Net;
using System.Collections.Generic;

namespace MobilePatterns.Data
{
    public class InspiredUI : PatternSource
    {
        public static Uri BaseUrl = new Uri("http://inspired-ui.com/");

        public List<WebMenu> GetMenus()
        {
            var w = new HttpWebRequest(BaseUrl);
            var response = w.GetResponse();
            var models = new List<WebMenu>();

            using (var r = response.GetResponseStream())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(r);

                var nodes = html.DocumentNode.SelectNodes("//ul[@id='tags']/li/a");
                foreach (var n in nodes)
                {
                    try
                    {
                        var uri = new Uri(BaseUrl, n.Attributes["href"].Value);
                        models.Add(new WebMenu() { Name = n.InnerText, Uri = uri, Source = this });
                    }
                    catch 
                    {
                    }
                }
            }

            return models;
        }

        public List<PatternImages> GetPatterns(Uri uri)
        {
            var w = new HttpWebRequest(uri);
            var response = w.GetResponse();
            var models = new List<PatternImages>();

            using (var r = response.GetResponseStream())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(r);

                var nodes = html.DocumentNode.SelectNodes("//div[@class='post']/div/img");
                foreach (var n in nodes)
                {
                    try
                    {
                        Uri url = new Uri(n.Attributes["src"].Value);
                        models.Add(new PatternImages() { Image = url.ToString() });
                    }
                    catch 
                    {
                    }
                }
            }

            return models;
        }
    }
}

