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

public partial class utility_weather : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string city = string.Empty;
        if (act == "ok")
        {
            city = Utils.GetRequest("city", "all", 2, "", "请正确输入城市名称，如“广州”");
        }
        else
        {
            if (Request.Cookies["WeatherCityComment"] != null)
            {
                city = HttpUtility.UrlDecode(Request.Cookies["WeatherCityComment"]["CityName"]);
            }
            else
            {
                city = "广州";
            }
            city = Utils.GetRequest("city", "get", 1, "", city);
            int pid = int.Parse(Utils.GetRequest("pid", "all", 1, @"^[0-9]\d*$", "-1"));
            int cid = int.Parse(Utils.GetRequest("cid", "all", 1, @"^[0-9]\d*$", "-1"));
            if (pid != -1 && cid != -1)
            {
                city = BCW.User.City.city[pid][cid].ToString();

            }
        }
        switch (act)
        {
            case "city":
                CityPage();
                break;
            case "more":
                MorePage();
                break;
            case "okcity":
                OkCityPage(city);
                break;
            default:
                ReloadPage(city);
                break;
        }
    }

    private void ReloadPage(string city)
    {
        Master.Title = "天气预报";
        string weather = new BCW.Service.GetWeather3gcn().GetWeather3gcnXML(city);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img src=\"/Files/sys/weather/logo.png\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>"+city + "天气预报</b>");
        builder.Append("<a href=\"" + Utils.getUrl("weather.aspx?act=city&amp;backurl=" + Utils.getPage(0) + "") + "\">[切换]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(weather);
        builder.Append(Out.Tab("</div>", ""));

        string strText = "请输入城市名称:/,,,";
        string strName = "city,act,backurl";
        string strType = "stext,hidden,hidden";
        string strValu = "广州'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false";
        string strIdea = "";
        string strOthe = "查询,weather.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("提示:您可以查询后,定制天气主页");
        builder.Append("<br />本站目前支持400多个国内外主要城市的五天内天气预报");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("weather.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("weather.aspx?act=okcity&amp;city=" + Server.UrlEncode(city) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设为默认</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CityPage()
    {
        int pid = int.Parse(Utils.GetRequest("pid", "get", 1, @"^[0-9]\d*$", "-1"));
        if (pid == -1)
        {
            Master.Title = "省份列表";
            builder.Append(Out.Tab("<div class=\"title\">省份列表</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i <= 33; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("weather.aspx?act=city&amp;pid=" + (i) + "") + "&amp;backurl=" + Utils.getPage(0) + "\">" + BCW.User.AppCase.CaseSheng(i) + "</a> ");
                if (i > 0 && (i + 1) % 4 == 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Master.Title = "城市列表";
            builder.Append(Out.Tab("<div class=\"title\">城市列表</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string[] city = BCW.User.City.city[pid];
            for (int i = 0; i < city.Length; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("weather.aspx?pid=" + pid + "&amp;cid=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + city[i] + "</a> ");
                if (i > 0 && (i + 1) % 4 == 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string strText = "请输入城市名称:/,,,";
        string strName = "city,act,backurl";
        string strType = "stext,hidden,hidden";
        string strValu = "广州'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false";
        string strIdea = "";
        string strOthe = "查询,weather.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("提示:您可以查询后,定制天气主页");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("weather.aspx?backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void OkCityPage(string city)
    {
        //写入Cookies
        HttpCookie cookie = new HttpCookie("WeatherCityComment");
        cookie.Expires = DateTime.Now.AddDays(365);
        cookie.Values.Add("CityName", HttpUtility.UrlEncode(city));
        Response.Cookies.Add(cookie);

        Utils.Success("设默认天气", "设置默认天气成功..", Utils.getUrl("weather.aspx?backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void MorePage()
    {
        int cityid = int.Parse(Utils.GetRequest("cityid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-9]\d*$", "类型错误"));
        string city = Utils.GetRequest("city", "get", 2, @"^[^\^]{1,}$", "城市错误");
        Master.Title = "" + city + "天气指数";
        string weather = new BCW.Service.GetWeather3gcn().GetWeather3gcnXML2(cityid, ptype, city);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(weather);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("weather.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("weather.aspx?city=" + city + "") + "\">天气首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
