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

public partial class inter : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "功能箱";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            //case "snap":
            //    SnapPage();
            //    break;
            case "recom":
                RecomPage();
                break;
            case "color":
                ColorPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.IsFoot = false;
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string pageType = Utils.getPageAll();
        if (pageType.IndexOf("" + VE + "") == -1)
        {
            pageType = Utils.getUrl(pageType);
        }
        //更新身份串
        pageType = UrlOper.UpdateParam(pageType, SID, Utils.getstrU());

        string sRight = string.Empty;
        string sLeft = string.Empty;
        string sCenter = string.Empty;
        sRight = Utils.Right(Utils.getstrVe(), 1);
        sLeft = Utils.Left(Utils.getstrVe(), 1);
        sCenter = Utils.Mid(Utils.getstrVe(), 1, 1);

        builder.Append(Out.Tab("<div class=\"title\">", ""));

        builder.Append("<a href=\"" + Utils.getPage("/default.aspx") + "\">返回之前页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("+|快捷功能<br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/favorites.aspx?backurl=" + Utils.getPage(0) + "") + "\">我的收藏夹</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/favorites.aspx?act=addin&amp;backurl=" + Utils.getPage(0) + "") + "\">+</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/panel.aspx?backurl=" + Utils.getPage(0) + "") + "\">我的控制板</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/panel.aspx?act=add&amp;backurl=" + Utils.getPage(0) + "") + "\">+</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("+|浏览设置<br />");
        builder.Append("<a href=\"" + Utils.getUrl("inter.aspx?act=color&amp;backurl=" + Utils.getPage(0) + "") + "\">界面</a>.");
        if (sCenter == "i")
        {
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "" + sRight)) + "\">有图</a>.");
        }
        else
        {
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "i" + sRight)) + "\">无图</a>.");
        }
        if (sRight == "a")
        {
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "b")) + "\">简体</a>.");
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "c")) + "\">繁体</a>.");
        }
        else if (sRight == "b")
        {
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "a")) + "\">原文</a>.");
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "c")) + "\">繁体</a>.");
        }
        else
        {
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "a")) + "\">原文</a>.");
            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, sLeft + "b")) + "\">简体</a>.");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("+|更多功能<br />");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + "") + "\">设机型</a>.");
        //builder.Append("<a href=\"" + Utils.getUrl("inter.aspx?act=snap&amp;backurl=" + Utils.getPage(0) + "") + "\">截图</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/function.aspx?act=copyl&amp;backurl=" + Utils.getPage(0) + "") + "\">复制选项</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("inter.aspx?act=recom&amp;backurl=" + Utils.getPage(0) + "") + "\">分享/举报</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/visit.aspx?backurl=" + Utils.getPage(0) + "") + "\">足迹</a>");
        builder.Append(Out.Tab("</div>", ""));

        //显示控制面板
        int FsIsPanel = 0;
        int meid = new BCW.User.Users().GetUsId();
        if (meid > 0)
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(meid);
            FsIsPanel = BCW.User.Users.GetForumSet(ForumSet, 8);
        }
        builder.Append(BCW.User.Users.ShowPanel(FsIsPanel, meid));
    }

    private void RecomPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("/default.aspx") + "\">返回之前页</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",,";
        string strName = "act,backurl";
        string strType = "hidden,hidden";
        string strValu = "recommend'" + Utils.getPage(0) + "";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "分享给好友|&gt;举报,/bbs/guest.aspx,post,1,blue|red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }

    private void ColorPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/skin.xml";
        xml.ReloadSub(xmlPath); //加载配置

        Master.IsFoot = false;
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string pageType = Utils.getPageAll();
        if (pageType.IndexOf("" + VE + "") == -1)
        {
            pageType = Utils.getUrl(pageType);
        }
        //更新身份串
        pageType = UrlOper.UpdateParam(pageType, SID, Utils.getstrU());
        string sLeft = string.Empty;
        string sRight = string.Empty;
        sLeft = Utils.Left(Utils.getstrVe(), 1);
        sRight = Utils.Right(Utils.getstrVe(), 1);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("/default.aspx") + "\">返回之前页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("彩色版仅支持WAP2.0的手机以及PC端");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, "1" + sRight)) + "\">WAP1.0</a>" + ForBs(sLeft, "1") + "");
        builder.Append(Out.Tab("</div>", ""));
        string strName = string.Empty;
        if (xml.dss["skin2"].ToString() == "1")
        {
            strName += ",2";
        }
        if (xml.dss["skin3"].ToString() == "1")
        {
            strName += ",3";
        }
        if (xml.dss["skin4"].ToString() == "1")
        {
            strName += ",4";
        }
        if (xml.dss["skin5"].ToString() == "1")
        {
            strName += ",5";
        }
        if (xml.dss["skin6"].ToString() == "1")
        {
            strName += ",6";
        }
        if (xml.dss["skin7"].ToString() == "1")
        {
            strName += ",7";
        }
        if (xml.dss["skin8"].ToString() == "1")
        {
            strName += ",8";
        }
        if (xml.dss["skin9"].ToString() == "1")
        {
            strName += ",9";
        }
        strName = Utils.Mid(strName, 1, strName.Length);
        string[] strNameTemp = strName.Split(",".ToCharArray());
        int k = 1;
        for (int i = 0; i < strNameTemp.Length; i++)
        {
            if (k % 2 == 0)
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            else
                builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("<a href=\"" + Out.UBB(UrlOper.UpdateParam(pageType, VE, "" + strNameTemp[i] + "" + sRight)) + "\">");
            builder.Append(Out.Tab("<font color=\"#" + xml.dss["div_title_color" + strNameTemp[i] + ""] + "\">■</font>", "<img src=\"/snap.aspx?act=color&amp;colorname=" + xml.dss["div_title_color" + strNameTemp[i] + ""] + "\"/>"));
            builder.Append("" + xml.dss["skinName" + strNameTemp[i] + ""] + "</a>");
            builder.Append("" + ForBs(sLeft, "" + strNameTemp[i] + "") + "");
            builder.Append(Out.Tab("</div>", ""));
            k++;
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("inter.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    //private void SnapPage()
    //{
    //    Master.IsFoot = false;
    //    string info = Utils.GetRequest("info", "post", 1, "", "");
    //    if (info != "ok")
    //    {
    //        builder.Append(Out.Tab("<div class=\"title\">", ""));
    //        string Purl = Out.UBB(Utils.removeUVe(Utils.getPage(1)));
    //        string Purls = "http://" + Utils.GetDomain() + "" + Purl + "";
    //        string Title = Utils.GetSourceTextByUrl(Utils.getUrl(Purls).Replace("&amp;", "&"));
    //        Title = Utils.GetTitle(Title);
    //        builder.Append("页面:<a href=\"" + Utils.getUrl(Purl) + "\">" + Title + "</a>");
    //        builder.Append(Out.Tab("</div>", ""));

    //        string strText = "截图样式:/,屏幕宽度:/,屏蔽高度:/,保存图片宽度:/,保存图片高度:/,水印文字(可空):/,文字:,位置:,,,";
    //        string strName = "HeadType,Snapwidth,Snapheight,width,height,word,color,Position,info,act,backurl";
    //        string strType = "select,num,num,num,num,text,select,select,hidden,hidden,hidden";
    //        string strValu = "0'240'320'240'320''#DE0000'0'ok'snap'" + Utils.getPage(0) + "";
    //        string strEmpt = "0|样式一|1|样式二,false,false,false,false,true,#DE0000|红色|#000000|黑色|#FFFFFF|白色|#0E71D4|蓝色|#0CB90D|绿色|#FD8300|黄色|#EF74DC|粉色,0|右下角|1|左上角|2|上中角|3|右上角|4|中左角|5|中心角|6|中右角|7|左下角|8|下中角,false,false,false";
    //        string strIdea = "/温馨提示:生成截图后可保存图片/";
    //        string strOthe = "生成截图|reset,inter.aspx,post,1,red|blue";
    //        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    //    }
    //    else
    //    {
    //        string GetUrl = "http://" + Utils.GetDomain() + Utils.getPage(1);
    //        string HeadType = Utils.GetRequest("HeadType", "post", 2, @"^[0-2]$", "样式类型选择错误");
    //        string Snapwidth = Utils.GetRequest("Snapwidth", "post", 2, @"^[0-9]\d*$", "屏幕宽度填写错误");
    //        string Snapheight = Utils.GetRequest("Snapheight", "post", 2, @"^[0-9]\d*$", "屏幕高度填写错误");
    //        string width = Utils.GetRequest("width", "post", 2, @"^[0-9]\d*$", "保存图片宽度填写错误");
    //        string height = Utils.GetRequest("height", "post", 2, @"^[0-9]\d*$", "保存图片高度填写错误");
    //        string word = Utils.GetRequest("word", "post", 3, @"^[^\^]{1,30}$", "水印文字限30字内");
    //        string color = Utils.GetRequest("color", "post", 1, "", "");
    //        string Position = Utils.GetRequest("Position", "post", 2, @"^[0-8]$", "水印位置选择错误");

    //        if (HeadType == "0")
    //        {
    //            Server.Transfer("/Snap.aspx?gourl=" + Server.UrlEncode("http://" + Utils.GetDomain() + "/Snap.aspx?act=WapBrowser&url=" + GetUrl + "") + "&x=" + Snapwidth + "&y=" + Snapheight + "&w=" + width + "&h=" + height + "&word=" + word + "&color=" + color + "&Position=" + Position + "");
    //        }
    //        else
    //        {
    //            Server.Transfer("/Snap.aspx?gourl=" + Server.UrlEncode(GetUrl) + "&x=" + Snapwidth + "&y=" + Snapheight + "&w=" + width + "&h=" + height + "&word=" + word + "&color=" + color + "&Position=" + Position + "");
    //        }
    //    }
    //}

    private string ForBs(string sLeft, string skinid)
    {
        if (sLeft == skinid)
        {
            return "√";
        }
        else
        {
            return "";
        }
    }
}