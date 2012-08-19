using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace MobilePatterns.Data
{
    public static class PattrnData
    {
        public static Uri BaseUrl = new Uri("http://pttrns.com");

        public static List<WebMenu> GetMenus()
        {
            var w = new HttpWebRequest(BaseUrl);
            var response = w.GetResponse();
            var models = new List<WebMenu>();

            using (var r = response.GetResponseStream())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(r);

                var nodes = html.DocumentNode.SelectNodes("//div[@id='container']/nav/ul[2]/li/a");
                foreach (var n in nodes)
                {
                    var uri = new Uri(BaseUrl, n.Attributes["href"].Value);
                    models.Add(new WebMenu() { Name = n.InnerText, Uri = uri });
                }
            }

            return models;
        }

        public static List<PatternImages> GetPatterns(Uri uri)
        {
            var w = new HttpWebRequest(uri);
            var response = w.GetResponse();
            var models = new List<PatternImages>();

            using (var r = response.GetResponseStream())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(r);

                var nodes = html.DocumentNode.SelectNodes("//div[@id='main']/section/a/img");
                foreach (var n in nodes)
                {
                    var url = new Uri(BaseUrl, n.Attributes["src"].Value);
                    models.Add(new PatternImages() { Image = url.ToString() });
                }
            }

            return models;
        }
    }
}

