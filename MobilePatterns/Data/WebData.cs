using System;
using System.Collections.Generic;

namespace MobilePatterns.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Screenshot
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int AppIp { get; set; }
        public string App { get; set; }

        public int CatIp { get; set; }
        public string Cat { get; set; }
        
    }

    public static class RequestFactory
    {
        public static List<Category> GetCategories()
        {
            var client = new RestSharp.RestClient();
            var request = new RestSharp.RestRequest("http://www.dillonbuchanan.com/appreciateui/categories.php");
            var response = client.Execute(request);
            return new RestSharp.Deserializers.JsonDeserializer().Deserialize<List<Category>>(response);
        }

        public static List<Screenshot> GetScreenshots(int category)
        {
            var client = new RestSharp.RestClient();
            var request = new RestSharp.RestRequest("http://www.dillonbuchanan.com/appreciateui/screenshots.php?cat=" + category);
            var response = client.Execute(request);
            var ret = new RestSharp.Deserializers.JsonDeserializer().Deserialize<List<Screenshot>>(response);
            ret.ForEach(x => x.Url = "http://www.dillonbuchanan.com/appreciateui/uploads/" + x.Url);
            return ret;
        }
    }
}

