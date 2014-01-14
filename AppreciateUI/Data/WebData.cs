using System.Collections.Generic;

namespace AppreciateUI.Data
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
        public string FullUrl { get; set; }
        public int AppIp { get; set; }
        public string App { get; set; }
        public int CatIp { get; set; }
        public string Cat { get; set; }
        public string Ext { get; set; }
    }


    public class Icon
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string App { get; set; }
        public string Ext { get; set; }
        public string FullUrl { get; set; }
    }

    public static class RequestFactory
    {
        public static List<Category> GetCategories()
        {
            var client = new RestSharp.RestClient();
			var request = new RestSharp.RestRequest("http://www.appreciateui.com/api/categories.php");
			return client.Execute<List<Category>>(request).Data;
        }

		public static List<Screenshot> GetRecentScreenshots()
		{
			var client = new RestSharp.RestClient();
			var request = new RestSharp.RestRequest("http://www.appreciateui.com/api/screenshots.php?recent=true");
			return client.Execute<List<Screenshot>>(request).Data;
		}

        public static List<Screenshot> GetScreenshots(int category)
        {
            var client = new RestSharp.RestClient();
			var request = new RestSharp.RestRequest("http://www.appreciateui.com/api/screenshots.php?cat=" + category);
			return client.Execute<List<Screenshot>>(request).Data;
        }

        public static List<Icon> GetIcons()
        {
            var client = new RestSharp.RestClient();
			var request = new RestSharp.RestRequest("http://www.appreciateui.com/api/icons.php");
			return client.Execute<List<Icon>>(request).Data;
        }
    }
}

