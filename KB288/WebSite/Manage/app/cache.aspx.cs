using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;

public partial class Manage_app_cache : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "缓存管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        if (act == "del")
        {
            DataCache.Clear();
            Utils.Success("清空缓存", "清空缓存，正在返回..", Utils.getUrl("cache.aspx"), "1");
        }
        else
        {

            //System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            //IDictionaryEnumerator CacheEnum = objCache.GetEnumerator();
            //while (CacheEnum.MoveNext())
                //builder.Append(CacheEnum.Key.ToString()+":"+CacheEnum.Value.ToString() + "<br />");

            builder.Append(Out.Div("title", "缓存管理"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("cache.aspx?act=del") + "\">清空系统缓存</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));

        }
    }
}