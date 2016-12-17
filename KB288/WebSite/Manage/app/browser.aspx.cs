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
using BCW.Service;
public partial class Manage_app_browser : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "admin":
                AdminPage();
                break;
            case "view":
                ViewPage();
                break;
            case "copy":
                CopyPage();
                break;
            case "snap":
                SnapPage();
                break;
            case "config1":
                Config1Page();
                break;
            case "save1":
                Save1Page();
                break;
            case "config2":
                Config2Page();
                break;
            case "save2":
                Save2Page();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        string gurl = Utils.GetRequest("gurl", "all", 1, "", "");
        bool bl = true;
        bool Isurl = true;
        string str = string.Empty;
        if (string.IsNullOrEmpty(gurl) || gurl == "http://")
        {
            bl = false;
        }
        if (bl)
        {
            if (!Utils.IsRegex(gurl, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"))
            {
                Isurl = false;
            }

            if (Isurl == true)
            {
                try
                {
                str = new Browser().CreateUrl("");
                }
                catch
                {
                    Isurl = false;
                }
            }
        }
        if (Isurl == false || bl == false)
        {
            Master.Title = "绿色浏览器";
            builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));

            if (Isurl == false)
            {
                builder.Append(Out.Div("div", "该地址无法浏览.."));
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("输入地址安全浏览:");
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",";
            string strName = "gurl";
            string strType = "text";
            string strValu = "http://";
            string strEmpt = "false";
            string strIdea = "";
            string strOthe = "GO,browser.aspx,get,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin") + "\">高级功能</a><br /><a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        if (!string.IsNullOrEmpty(str))
        {
            string HeadType = "0";

            if (Request.Cookies["HeadAgentComment"] != null)
            {
                HeadType = Request.Cookies["HeadAgentComment"]["HeadType"];
            }

            if (HeadType == "0")
                HttpContext.Current.Response.ContentType = "text/vnd.wap.wml";
            else if (HeadType == "1")
                HttpContext.Current.Response.ContentType = "application/xhtml+xml";
            else if (HeadType == "2")
                HttpContext.Current.Response.ContentType = "text/html";
            else
                HttpContext.Current.Response.ContentType = "text/vnd.wap.wml";

            if (str.IndexOf("</body>") != -1)
            {
                str = str.Substring(0, str.LastIndexOf("</body>"));
                str += "<div class=\"hr\"></div>";
                str += "<div>";
                str += "<a href=\"" + Utils.getUrl("browser.aspx?act=admin") + "\">[功能]</a>";
                str += "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>";
                str += "</div></body></html>";
            }
            else if (str.IndexOf("</p>") != -1)
            {
                str = str.Substring(0, str.LastIndexOf("</p>"));
                str += "<br />";
                str += "<a href=\"" + Utils.getUrl("browser.aspx?act=admin") + "\">[功能]</a>";
                str += "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>";
                str += "</p></card></wml>";
            }
            else
            {
                Utils.Error("该站点无法访问..", "");
            }
            Response.Write(str);
            Response.End();

        }
    }

    private void AdminPage()
    {
        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=config1&amp;backurl=" + Utils.getPage(0)) + "\">[系统设置]</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("browser.aspx?act=config2&amp;backurl=" + Utils.getPage(0)) + "\">[传递设置]</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("browser.aspx?act=view&amp;backurl=" + Utils.getPage(0)) + "\">[查看源码]</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("browser.aspx?act=copy&amp;backurl=" + Utils.getPage(0)) + "\">[复制文本]</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("browser.aspx?act=snap&amp;backurl=" + Utils.getPage(0)) + "\">[在线截图]</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("输入地址安全浏览:");
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,";
        string strName = "gurl";
        string strType = "text";
        string strValu = "http://";
        string strEmpt = "false";
        string strIdea = "";
        string strOthe = "GO,browser.aspx,get,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        if (Utils.getPage(0) != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"ft\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ViewPage()
    {
        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));

        if (Utils.getPage(0) == "")
        {
            builder.Append("该地址无法查看源码");
        }
        else
        {
            string str = new Browser().CreateUrl(Utils.getPage(0));
            if (!Utils.Isie())
                builder.Append("<input type=\"text\" emptyok=\"true\" name=\"Content" + new Random().Next(100, 999) + "\" value=\"" + Out.WmlEncode(str) + "\" />");
            else
                builder.Append("<textarea  name=\"Content" + new Random().Next(100, 999) + "\">" + Out.WmlEncode(str) + "</textarea>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void CopyPage()
    {
        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));

        if (Utils.getPage(0) == "")
        {
            builder.Append("该地址无法查复制文本");
        }
        else
        {
            string str = new Browser().CreateUrl(Utils.getPage(0));
            if (!Utils.Isie())
                builder.Append("<input type=\"text\" emptyok=\"true\" name=\"Content" + new Random().Next(100, 999) + "\" value=\"" + Out.RemoveHtml(str) + "\" />");
            else
                builder.Append("<textarea  name=\"Content" + new Random().Next(100, 999) + "\">" + Out.WmlEncode(str) + "</textarea>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SnapPage()
    {
        string info = Utils.GetRequest("info", "post", 1, "", "");
        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("截图地址:");
            builder.Append(Out.Tab("</div>", ""));
            string strText = ",网页类型:/,屏幕宽度:/,屏蔽高度:/,保存图片宽度:/,保存图片高度:/,水印文字(可空):/,文字颜色:,位置:,,,";
            string strName = "GetUrl,HeadType,Snapwidth,Snapheight,width,height,word,color,Position,info,act,backurl";
            string strType = "text,select,num,num,num,num,text,select,select,hidden,hidden,hidden";
            string strValu = "" + Server.HtmlDecode(Request["backurl"]) + "'0'240'320'240'320''#DE0000'0'ok'snap'" + Utils.getPage(0) + "";
            string strEmpt = "false,0|wap1.0|1|wap2.0|2|电脑网页,false,false,false,false,true,#DE0000|红色|#000000|黑色|#FFFFFF|白色|#0E71D4|蓝色|#0CB90D|绿色|#FD8300|黄色|#EF74DC|粉色,0|右下角|1|左上角|2|上中角|3|右上角|4|中左角|5|中心角|6|中右角|7|左下角|8|下中角,false,false,false";
            string strIdea = "/温馨提示:生成截图后可保存图片/";
            string strOthe = "生成截图|reset,browser.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string GetUrl = Utils.GetRequest("GetUrl", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请正确输入网页地址");
            string HeadType = Utils.GetRequest("HeadType", "post", 2, @"^[0-2]$", "网页类型选择错误");
            string Snapwidth = Utils.GetRequest("Snapwidth", "post", 2, @"^[0-9]\d*$", "屏幕宽度填写错误");
            string Snapheight = Utils.GetRequest("Snapheight", "post", 2, @"^[0-9]\d*$", "屏幕高度填写错误");
            string width = Utils.GetRequest("width", "post", 2, @"^[0-9]\d*$", "保存图片宽度填写错误");
            string height = Utils.GetRequest("height", "post", 2, @"^[0-9]\d*$", "保存图片高度填写错误");
            string word = Utils.GetRequest("word", "post", 3, @"^[^\^]{1,30}$", "水印文字限30字内");
            string color = Utils.GetRequest("color", "post", 1, "", "");
            string Position = Utils.GetRequest("Position", "post", 2, @"^[0-8]$", "水印位置选择错误");

            if (HeadType == "0")
            {
                Server.Transfer("/Snap.aspx?gourl=" + Server.UrlEncode("http://" + Utils.GetDomain() + "/Snap.aspx?act=WapBrowser&url=" + GetUrl + "") + "&x=" + Snapwidth + "&y=" + Snapheight + "&w=" + width + "&h=" + height + "&word=" + word + "&color=" + color + "&Position=" + Position + "");
            }
            else
            {
                Server.Transfer("/Snap.aspx?gourl=" + Server.UrlEncode(GetUrl) + "&x=" + Snapwidth + "&y=" + Snapheight + "&w=" + width + "&h=" + height + "&word=" + word + "&color=" + color + "&Position=" + Position + "");
            }


        }
        builder.Append(Out.Tab("<div>", "<br />"));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Config1Page()
    {
        string HeadType = "0";
        string IsPic = "0";
        string TimeOut = "10000";
        string Charset = "0";

        if (Request.Cookies["HeadAgentComment"] != null)
        {
            HeadType = Request.Cookies["HeadAgentComment"]["HeadType"];
            IsPic = Request.Cookies["HeadAgentComment"]["IsPic"];
            TimeOut = Request.Cookies["HeadAgentComment"]["TimeOut"];
            Charset = Request.Cookies["HeadAgentComment"]["Charset"];
        }
        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("系统设置");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "浏览网页类型:/,网页编码:/,是否开启图片显示:/,连接超时:(秒)/,,";
        string strName = "HeadType,Charset,IsPic,TimeOut,act,backurl";
        string strType = "select,select,select,num,hidden,hidden";
        string strValu = "" + HeadType + "'" + Charset + "'" + IsPic + "'" + TimeOut + "'save1'" + Utils.getPage(0) + "";
        string strEmpt = "0|wap1.0|1|wap2.0|2|电脑网页,0|UTF-8|1|GB2312,0|开启显示|1|关闭显示,false,false,false";
        string strIdea = "/";
        string strOthe = "确定设置|reset,browser.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:细心设置可让您更好浏览网页！<br />1.设置好网页类型让你可以浏览手机网，电脑网.<br />2.网码乱码请更改网页编码方式.<br />3.同时你还可以开启与关闭图片显示，节省您的流量.<br />4.等太久无回应或者轻易超时请设置合适的连接超时时间");

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Save1Page()
    {
        string HeadType = Utils.GetRequest("HeadType", "post", 2, @"^[0-2]$", "浏览见面类型选择错误");
        string IsPic = Utils.GetRequest("IsPic", "post", 2, @"^[0-1]$", "是否开启图片显示选择错误");
        string TimeOut = Utils.GetRequest("TimeOut", "post", 3, @"^[0-9]\d*$", "连接超时填写错误");
        string Charset = Utils.GetRequest("Charset", "post", 2, @"^[0-1]$", "编码方式选择错误");

        //写入Cookies
        HttpCookie cookie = new HttpCookie("HeadAgentComment");
        cookie.Expires = DateTime.Now.AddDays(30);//30天
        cookie.Values.Add("HeadType", HeadType);
        cookie.Values.Add("IsPic", IsPic);
        cookie.Values.Add("TimeOut", TimeOut);
        cookie.Values.Add("Charset", Charset);
        Response.Cookies.Add(cookie);

        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("系统设置成功");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Config2Page()
    {
        string ProxyIP = string.Empty;
        string Agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
        string Via = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
        string Accept = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT"];
        string IsMobile = string.Empty;
        string IsForIP = string.Empty;

        if (Request.Cookies["AgentComment"] != null)
        {
            ProxyIP = Request.Cookies["AgentComment"]["ProxyIP"];
            Agent = Request.Cookies["AgentComment"]["Agent"];
            Via = Request.Cookies["AgentComment"]["Via"];
            Accept = Request.Cookies["AgentComment"]["Accept"];
            IsMobile = Request.Cookies["AgentComment"]["IsMobile"];
            IsForIP = Request.Cookies["AgentComment"]["IsForIP"];

        }

        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("传递设置");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "代理IP:(可留空)/,浏览器型号:/,VIA网关值:/,Accept值:/,是否传递随机手机号:/,是否传递随机内网IP:/,,";
        string strName = "ProxyIP,Agent,Via,Accept,IsMobile,IsForIP,act,backurl";
        string strType = "text,text,text,text,select,select,hidden,hidden";
        string strValu = "" + ProxyIP + "'" + Agent + "'" + Via + "'" + Accept + "'" + IsMobile + "'" + IsForIP + "'save2'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,true,0|传递|1|不传递,0|传递|1|不传递,false,false";
        string strIdea = "/";
        string strOthe = "确定设置|reset,browser.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:任何一项都有可能可以让您浏览畅通无阻！<br />1.代理IP可让你穿透移动IP限制.<br />2.传递手机浏览器型号可以穿透限非手机浏览器访问的限制.<br />3.VIA网关、Accept值、内网IP可以穿透限制相关参数的限制.<br />4.传递手机号可以让你可以自动登录到识别手机号登录注册的网站.");
    
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Save2Page()
    {
        string ProxyIP = Utils.GetRequest("ProxyIP", "post", 3, @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}:\d{1,4}$", "代理IP填写错误，代理IP格式：222.111.111.111:80");
        string Agent = Utils.GetRequest("Agent", "post", 3, @"^[^\^]{1,500}$", "浏览器型号限500字符");
        string Via = Utils.GetRequest("Via", "post", 3, @"^[^\^]{1,500}$", "VIA网关值限500字符");
        string Accept = Utils.GetRequest("Accept", "post", 3, @"^[^\^]{1,500}$", "Accept值限1000字符");
        string IsMobile = Utils.GetRequest("IsMobile", "post", 2, @"^[0-1]$", "是否传递手机号选择错误");
        string IsForIP = Utils.GetRequest("IsForIP", "post", 2, @"^[0-1]$", "是否传递内线IP选择错误");

        //写入Cookies
        HttpCookie cookie = new HttpCookie("AgentComment");
        cookie.Expires = DateTime.Now.AddDays(30);//30天
        cookie.Values.Add("ProxyIP", ProxyIP);
        cookie.Values.Add("Agent", Agent);
        cookie.Values.Add("Via", Via);
        cookie.Values.Add("Accept", Accept);
        cookie.Values.Add("IsMobile", IsMobile);
        cookie.Values.Add("IsForIP", IsForIP);
        Response.Cookies.Add(cookie);

        Master.Title = "绿色浏览器";
        
        builder.Append(Out.Tab("<div class=\"title\">绿色浏览器</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("传递设置成功");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.getPage(0) != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("browser.aspx") + "&amp;gurl=" + Utils.getPage(0) + "\">继续浏览</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("browser.aspx?act=admin&amp;backurl=" + Utils.getPage(0)) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"ft\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}