
using System.Collections.Generic;

namespace BCW.JS
{
    public class ToJoson
    {
        public string code { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public List<list> list;
    }
    // 下载于www.mycodes.net
    public struct list
    {
        public string article { get; set; }
        public string source { get; set; }
        public string detailurl { get; set; }
        public string trainnum { get; set; }
        public string start { get; set; }
        public string state { get; set; }
        public string terminal { get; set; }
        public string flight { get; set; }
        public string route { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string info { get; set; }
    }
}
