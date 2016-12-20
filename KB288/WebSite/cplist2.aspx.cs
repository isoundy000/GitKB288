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

public partial class cplist2 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/cplist.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "kzlist":
                KZlistPage();
                break;
            case "nolist":
                NOlistPage();
                break;
            case "fxlist":
                FXlistPage();
                break;
            default:
                ReloadPage();
                break;
        }
 
    }

    private void ReloadPage()
    {
   
        Master.Title = "六彩数据分析";


        string str = new BCW.Service.Getmbsix().GetmbsixXML();
        if (!string.IsNullOrEmpty(str))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/files/shuju.png\" alt=\"数据\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("数据分析-提高中奖几率");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(str);
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", ""));
        }

        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/bbs/forum.aspx?id=13202") + "\">【财经论坛资料大全】</a>");
        //builder.Append(Out.Tab("</div>", "<br />"));

        


        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/game/six49.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/six49.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void KZlistPage()
    {
        Master.Title = "六彩开奖历史";
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "1"));
        string str = new BCW.Service.Getmbsix().GetmbsixXML2(page);
        if (!string.IsNullOrEmpty(str))
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("〓六彩开奖历史〓");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(str);
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/six49.aspx") + "\">虚拟</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("cplist2.aspx?backurl=" + Utils.getPage(0) + "") + "\">分析</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void NOlistPage()
    {
        Master.Title = "六彩未出分析";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-1]\d*$", "0"));


        string str = new BCW.Service.Getmbsix().GetmbsixXML3(ptype, showtype);
        if (!string.IsNullOrEmpty(str))
        {
            //builder.Append(Out.Tab("<div class=\"title\">", ""));
            //builder.Append("〓六彩开奖历史〓");
            //builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(str);
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/six49.aspx") + "\">虚拟</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("cplist2.aspx?backurl=" + Utils.getPage(0) + "") + "\">分析</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FXlistPage()
    {
        Master.Title = "六彩数据分析";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "1"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "5"));


        string str = new BCW.Service.Getmbsix().GetmbsixXML4(ptype,pn);
        if (!string.IsNullOrEmpty(str))
        {


            string strText = "查询多期:(最大200期):/,,,";
            string strName = "pn,act,ptype";
            string strType = "stext,hidden,hidden";
            string strValu = "'fxlist'"+ptype+"";
            string strEmpt = "true,false,false";
            string strIdea = "";
            string strOthe = "查询,cplist2.aspx,post,3,red";
            string rep = Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe);
            str = str.Replace("[@搜索调用]", rep+"<br />");

            //builder.Append(Out.Tab("<div class=\"title\">", ""));
            //builder.Append("〓六彩开奖历史〓");
            //builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(str);
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/six49.aspx") + "\">虚拟</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("cplist2.aspx?backurl=" + Utils.getPage(0) + "") + "\">分析</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
