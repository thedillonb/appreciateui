using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace MobilePatterns.Data
{
    public static class PattrnData
    {
        public class Model
        {
            public string Image { get; set; }

            public override string ToString()
            {
                return this.Image;
            }
        }

        public static List<Model> GetData()
        {
            var w = new HttpWebRequest(new Uri("http://pttrns.com/"));
            var response = w.GetResponse();
            var models = new List<Model>();

            using (var r = response.GetResponseStream())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(response.GetResponseStream());

                var nodes = html.DocumentNode.SelectNodes("//div[@id='main']/section/a/img");
                foreach (var n in nodes)
                {
                    models.Add(new Model() { Image = n.Attributes["src"].Value });
                }
            }

            return models;
        }
    }
}

