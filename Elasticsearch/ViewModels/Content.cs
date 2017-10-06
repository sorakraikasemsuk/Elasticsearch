using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elasticsearch.ViewModels
{
    public class Content
    {
        [Number]
        public int content_id { get; set; }
        [Number]
        public int contenttype_id { get; set; }
        [Text]
        public string content_url { get; set; }
        [Text]
        public string content_name { get; set; }
        [Text]
        public string content_desc { get; set; }
        [Text]
        public string content_kwd { get; set; }
        [Text]
        public string content_json { get; set; }
        [Object]
        public ContentDisplay content_display { get; set; }
    }
    public class ContentDisplay
    {
        [Number]
        public int content_id { get; set; }
        [Number]
        public int contenttype_id { get; set; }
        [Text]
        public string content_url { get; set; }
        [Text]
        public string content_name { get; set; }
        [Text]
        public string content_desc { get; set; }
        [Text]
        public string content_kwd { get; set; }
        [Text]
        public string content_json { get; set; }
    }
}