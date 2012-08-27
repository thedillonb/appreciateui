using System;
using System.Collections.Generic;
using System.Net;

namespace MobilePatterns.Data
{
    public class MobilePatterns : PatternSource
    {
        public static Uri BaseUrl = new Uri("http://mobile-patterns.com/");

        public List<WebMenu> GetMenus()
        {
            var w = new HttpWebRequest(BaseUrl);
            var response = w.GetResponse();
            var models = new List<WebMenu>();

            using (var r = response.GetResponseStream())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(r);

                var nodes = html.DocumentNode.SelectNodes("//div[@id='site_nav']/ul/li/span/a");
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

                var nodes = html.DocumentNode.SelectNodes("//div[@class='media photo portrait']/p/img");
                foreach (var n in nodes)
                {
                    try
                    {
                        //Hidden in the params
                        var param = n.Attributes["params"].Value;
                        param = param.Substring(23);
                        param = param.Substring(0, param.Length - 7);

                        Uri url = new Uri(param);
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

