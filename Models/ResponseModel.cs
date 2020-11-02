using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOauth2.Models
{
    public class ResponseModel
    {
        public class RssFeeds
        {
            public string title { get; set; }
            public string description { get; set; }
            public string image { get; set; }
            public string url { get; set; }
        }
    }
}