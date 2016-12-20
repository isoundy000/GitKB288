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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class utility_book : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "" + ub.Get("SiteText") + "书城";//hahu.net//http://wap.fenghuo123.com/index.asp?pno=0
        //int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
            //Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();
                break;
            default:
                ReloadPage();
                break;
        }

    }

    private void ReloadPage()
    {
        string url = Utils.GetRequest("url", "get", 1, "", "");

        string start = "";
        string over = "";
        string add = "";
        string replace = "";

        if (url == "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"book.gif\" alt=\"logo\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));

            url = "http://reshu8.com/index.asp";
            start = "【更新】";
            over = "\\[找回热书8";
            add = "【更新】";
            replace = "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.waplink(Utils.getUrl("book.aspx?url=sort.asp?pno=0"), "书库") + ".");
            builder.Append(Out.waplink(("book.aspx?url=girl.asp?pno=0&amp;ve=1a&amp;u=" + Utils.getstrU() + ""), "女生") + ".");
            builder.Append(Out.waplink(Utils.getUrl("book.aspx?url=gengla.asp?page=1&amp;pno=0"), "今日更新") + "<br />");

            builder.Append(Out.waplink(Utils.getUrl("book.aspx?url=toplist.asp?xu=1&amp;pno=0"), "排行") + ".");
            builder.Append(Out.waplink(Utils.getUrl("book.aspx?url=quanben.asp?tid=5&amp;pno=0"), "全本") + ".");
            builder.Append(Out.waplink(Utils.getUrl("book.aspx?url=tuijian.asp?page=1&amp;pno=0"), "推荐") + ".");
            builder.Append(Out.waplink(Utils.getUrl("book.aspx?url=newbook.asp?page=1&amp;pno=0"), "新书") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (Utils.Isie())
            {
                builder.Append("<form class=\"form\" action=\"book.aspx?url=search_out.asp?pno=0&amp;ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "\" method=\"post\">");
                builder.Append("<input name=\"s_key\" type=\"text\" maxlength=\"15\" value=\"青春\"/><br />");
                builder.Append("<input name=\"s_type\" type=\"radio\" value=\"1\" checked=\"checked\"/>书名");
                builder.Append("<input name=\"s_type\" type=\"radio\" value=\"2\"/>作者         ");
                builder.Append("<input id=\"Submit1\" type=\"submit\" value=\"搜索\" />");
                builder.Append("</form>");
            }
            else
            {
                builder.Append("<input name=\"s_key\" type=\"text\" maxlength=\"20\" value=\"青春\"/><br/>");
                builder.Append("搜<anchor title=\"书名\"><b>书名</b><go href=\"book.aspx?url=search_out.asp?pno=0&amp;ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "\" method=\"post\">");
                builder.Append("<postfield name=\"s_type\" value=\"1\" />");
                builder.Append("<postfield name=\"s_key\" value=\"$(s_key)\" /></go></anchor>\n");
                builder.Append("<anchor title=\"作者\"><b>作者</b><go href=\"book.aspx?url=search_out.asp?pno=0&amp;ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "\" method=\"post\">");
                builder.Append("<postfield name=\"s_type\" value=\"2\" />");
                builder.Append("<postfield name=\"s_key\" value=\"$(s_key)\" />");
                builder.Append("</go>");
                builder.Append("</anchor>\n");
                builder.Append("<a href=\"" + Utils.getUrl("book.aspx?url=reci.asp?class=1&amp;pno=0") + "\">热词</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            url = "http://reshu8.com/" + Utils.getPageAll();
          
            start = "<p>";
            over = "---------------<br/>";
            add = "";
            replace = "<p>";
        }
        url = Regex.Replace(url, @"ve=[^&]*&{0,}u=[^&]*&{0,}", "", RegexOptions.IgnoreCase);
        url = url.Replace("/utility/book.aspx?url=", "");

        //得到内容
        string str = new BCW.Service.GetPageUtf8().GetPageUtf8XML(url, start, over);
        //过滤
        str = Regex.Replace(str, @"<a.href=.http://cnota.cn/g.jsp[\s\S]+?>\*[\s\S]+?</a>(<br/>)*", "");
        str = Regex.Replace(str, @"<a.href=.http://cnota.cn/g.jsp[\s\S]+?.>(\*)*[\s\S]+?</a>(<br/>)*", "");
        str = Regex.Replace(str, @"images/", "http://reshu8.com/images/");
        str = Regex.Replace(str, @"(<a href=""(http://book.reshu8.net/)*([\w\d_]+?.asp))", "<a href=\"book.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "&amp;url=$3");
        str = Regex.Replace(str, @"(<a href='(http://book.reshu8.net/)*([\w\d_]+?.asp))", "<a href='book.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "&amp;url=$3");
        str = Regex.Replace(str, @"(<go href=""(http://book.reshu8.net/)*([\w\d_]+?.asp))", "<go href=\"book.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "&amp;url=$3");
        str = Regex.Replace(str, @"(<go href='(http://book.reshu8.net/)*([\w\d_]+?.asp))", "<go href='book.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "&amp;url=$3");
        
        //非内网连接全除掉
        str = Regex.Replace(str, @"<a.href=.(?:(?!book.aspx)[^>])*>[\s\S]+?</a>(<br/>)*", "");
        str = Regex.Replace(str, @"<anchor>[\s\S]+?<go.href=.(?:(?!book.aspx)[^>])*>[\s\S]+?</anchor>(<br/>)*", "");

        //垃圾过滤
        str = str.Replace("||", "").Replace(".......", "");
        str = Regex.Replace(str, @"\|<anchor>收藏<go.href=.(?:[^>])*>[\s\S]+?</anchor>", "");
        str = Regex.Replace(str, @"\|<anchor>加书签<go.href=.(?:[^>])*>[\s\S]+?</anchor>", "");
        str = Regex.Replace(str, @"\|<anchor>报错<go.href=.(?:[^>])*>[\s\S]+?</anchor>", "");
        str = Regex.Replace(str, @"<a.href=.(?:[^>])*>单</a>(<br/>)*", "");
        str = Regex.Replace(str, @"<a.href=.(?:[^>])*>\[推荐鲜花\]</a>", "");
        str = str.Replace("||", "").Replace(".......", "").Replace("|]","");
        str = str.Replace("http://wap.reshu8.net/images/mmm.gif", "mm.gif");
        str = str.Replace("votebook.asp", "tuijian.asp");
        str = str.Replace("-reshu8.com", "");
        //增与替换掉
        if (add != "")
            str = add + str;
        if (replace != "")
            str = str.Replace(replace, "");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(str);
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("/bbs/uinfo.aspx"), "上级") + "-");
        if (url != "http://reshu8.com/index.asp")
        {
            builder.Append(Out.waplink(Utils.getUrl("book.aspx"), "书城") + "");
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("/bbs/default.aspx"), "社区") + "");
        }
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ViewPage()
    {

    }

   
}
