using System;
using System.Collections.Generic;
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

public partial class link : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/link.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("LinkStatus", xmlPath) == "1")
        {
            Utils.Safe("友链系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "ok":
                OkPage();
                break;
            case "view":
                ViewPage();
                break;
            default:
                ReloadPage(act);
                break;
        }
    }

    private void ReloadPage(string act)
    {
    
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "get", 1, @"^[0-9]\d*$", "0"));
        if (id != 0)
        {
            if (!new BCW.BLL.Link().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            if (act != "info")
            {
                new Out().head(Utils.ForWordType("温馨提示"));
                Response.Write(Out.Tab("<div class=\"text\">", ""));
                Response.Write("您即将离开" + ub.Get("SiteName") + "...");
                Response.Write(Out.Tab("</div>", "<br />"));
                Response.Write(Out.Tab("<div class=\"title\">", ""));
                Response.Write("<a href=\"link.aspx?act=info&amp;id=" + id + "&amp;ve=" + Utils.getstrVe() + "\">马上进入</a>");
                Response.Write(Out.Tab("</div>", "<br />"));
                Response.Write(Out.Tab("<div>", ""));
                Response.Write(Out.back("返回上一级"));
                Response.Write(Out.Tab("</div>", ""));
                Response.Write(new Out().foot());
                Response.End();
            }
            else
            {
                //统计链出
                if (ub.GetSub("LinkIsPc", xmlPath) == "0")
                {
                    if (Utils.IsMobileUa())
                    {
                        new BCW.BLL.Link().UpdateLinkOut(id);
                    }
                }
                else
                {
                    new BCW.BLL.Link().UpdateLinkOut(id);
                }

                //链出
                Response.Redirect(new BCW.BLL.Link().GetLinkUrl(id).Replace("&amp;", "&"));
            }
        }
        Master.Title = ub.GetSub("LinkName", xmlPath);
        //Logo
        if (ub.GetSub("LinkLogo", xmlPath) != "")
        {
            builder.Append("<img src=\"" + ub.GetSub("LinkLogo", xmlPath) + "\" alt=\"load\"/><br />");
        }

        //友链口号
        if (ub.GetSub("LinkNotes", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.GetSub("LinkNotes", xmlPath))) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //友链顶部UBB
        if (ub.GetSub("LinkTopUbb", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.GetSub("LinkTopUbb", xmlPath))) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));

        //-----------------------列表版面形式
        if (ub.GetSub("LinkbmType", xmlPath) == "0" || leibie != 0)
        {

            if (ub.GetSub("LinkLogo", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
            }
            if (leibie == 0)
            {

                if (ptype == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">热门网站</a>");
                    builder.Append("|站长推荐");
                }
                else
                {
                    builder.Append("热门网站|");
                    builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">站长推荐</a>");
                }
            }
            else
            {
                //取分类名称
                string Leibie = ub.GetSub("LinkLeibie", xmlPath);
                string sName = string.Empty;
                string[] sTemp = Leibie.Split("|".ToCharArray());
                for (int j = 0; j < sTemp.Length; j++)
                {
                    if (j % 2 == 0)
                    {
                        if (Convert.ToInt32(sTemp[j]) == leibie)
                            sName = sTemp[j + 1].ToString();
                    }
                }
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx") + "\">全部</a>&gt;" + sName + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string strOrder = "";
            string[] pageValUrl = { "ptype", "leibie", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere += "Hidden=1";
            if (leibie != 0)
                strWhere += " and leibie=" + leibie + "";

            if (ptype == 1)
                strWhere += " and LinkRd=1";

            //排序条件
            string iType = ub.GetSub("LinkListType", xmlPath);
            if (iType == "0")
                strOrder = "LinkIn-LinkOut Desc";
            else if (iType == "1")
                strOrder = "LinkIn Desc";
            else if (iType == "2")
                strOrder = "LinkOut Desc";
            else if (iType == "3")
                strOrder = "LinkTime Desc";
            else
                strOrder = "LinkIn-LinkOut Desc";

            // 开始读取列表
            IList<BCW.Model.Link> listLink = new BCW.BLL.Link().GetLinks(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listLink.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Link n in listLink)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("link.aspx?act=view&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a><br />{3}", ((pageIndex - 1) * pageSize) + k, n.ID, n.LinkName, Utils.FormatString(n.LinkNotes, 20));

                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else
        {
            string strLeibie = ub.GetSub("LinkLeibie", xmlPath);
            string[] sTemp = strLeibie.Split("|".ToCharArray());
            int k = 1;
            for (int j = 0; j < sTemp.Length; j++)
            {
                if (j % 2 == 0)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("<a href=\"" + Utils.getUrl("link.aspx?leibie=" + sTemp[j] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[" + sTemp[j + 1].ToString() + "]</a>");
                    //列出每个分类下的友链
                    int TopNum = 0;
                    try
                    {
                        TopNum = int.Parse(ub.GetSub("LinkListNo", xmlPath));
                    }
                    catch
                    {
                        TopNum = 5;
                    }
                    //排序条件
                    string strOrder = string.Empty;
                    string iType = ub.GetSub("LinkLeibieType", xmlPath);
                    if (iType == "0")
                        strOrder = "LinkIn-LinkOut Desc";
                    else if (iType == "1")
                        strOrder = "LinkIn Desc";
                    else if (iType == "2")
                        strOrder = "LinkOut Desc";
                    else if (iType == "3")
                        strOrder = "LinkTime Desc";
                    else if (iType == "4")
                        strOrder = "LinkTime Desc";
                    else
                        strOrder = "LinkIn-LinkOut Desc";

                    string strWhere = " and Hidden=1";
                    if (iType == "4")
                        strWhere = " and LinkRd=1";

                    DataSet ds = new BCW.BLL.Link().GetList(TopNum, "leibie=" + sTemp[j] + " " + strWhere + " Order By " + strOrder + "");
                    if (ds == null || ds.Tables[0].Rows.Count == 0)
                    {
                        builder.Append("等待添加中..");
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sName = string.Empty;
                        if (ub.GetSub("LinkNameType", xmlPath) == "1")
                            sName = ds.Tables[0].Rows[i]["LinkNamt"].ToString();
                        else
                            sName = ds.Tables[0].Rows[i]["LinkName"].ToString();

                        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=view&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + sName + "</a>\n");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }
            }
        }
        //友链底部UBB
        if (ub.GetSub("LinkFootUbb", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.GetSub("LinkFootUbb", xmlPath))) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=add&amp;backurl=" + Utils.getPage(0) + "") + "\">发布网站</a>");
        builder.Append(Out.Tab("</div>", ""));
    

    }
    private void AddPage()
    {
        Master.Title = "发布您的网站";
        //会员发表权限
        int meid = new BCW.User.Users().GetUsId();
        if (ub.GetSub("LinkIsUser", xmlPath) == "1")
        {
            if (meid == 0)
                Utils.Login();
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布您的网站");
        builder.Append(Out.Tab("</div>", ""));

        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;

        if (ub.GetSub("LinkLeibie", xmlPath) != "")
        {
            sText = ",选择分类:/";
            sName = ",Leibie";
            sType = ",select";
            sValu = "'1";
            sEmpt = "," + ub.GetSub("LinkLeibie", xmlPath) + "";
        }

        string strText = "网站名称:限10字/,网站简称:2-3字/,网站地址:/,网站简介:限500字/,关键字:如“下载 图铃 音乐”等，用|号分隔/" + sText + ",,";
        string strName = "LinkName,LinkNamt,LinkUrl,LinkNotes,KeyWord" + sName + ",act,backurl";
        string strType = "text,text,text,textarea,text" + sType + ",hidden,hidden";
        string strValu = "''http://''" + sValu + "'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,true,false" + sEmpt + ",false,false";
        string strIdea = "/";
        string strOthe = "提交网站|reset,link.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void OkPage()
    {
        Master.Title = "发布您的网站";
        //会员发表权限
        int meid = new BCW.User.Users().GetUsId();
        if (ub.GetSub("LinkIsUser", xmlPath) == "1")
        {
            if (meid == 0)
                Utils.Login();
        }
        string LinkName = Utils.GetRequest("LinkName", "post", 2, @"^[\s\S]{2,10}$", "请输入不超过10字的网站名称");
        string LinkNamt = Utils.GetRequest("LinkNamt", "post", 2, @"^(?:[\u4E00-\u9FA5]{2,3}|[\w\-\.]{2,5})$", "请输入正确的网站简称，中文限2-3字，字母或数字限2-5字");
        //string LinkUrl = Utils.GetRequest("LinkUrl", "post", 2, @"^(?:http:\/\/)?(?:[\w\-]{1,10}\.)?[\w\-]{1,20}\.(?:com\.cn|net\.cn|com|net|org|mobi|cn|cc|us|name|me)(?:\/[\w\-]+){0,5}(?:\.aspx|\.asp|\.aspx|\.jsp|\.php|\.do|\.eu|\.action|\/)?(?:\?[\w\-]{1,20}\=[\w\.\-]{1,30})?$", "请输入合法的网址");
        string LinkUrl = Utils.GetRequest("LinkUrl", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请输入合法的网址");
        string LinkNotes = Utils.GetRequest("LinkNotes", "post", 3, @"^[\s\S]{0,500}$", "请输入不超过500字的网站简介");
        string KeyWord = Utils.GetRequest("KeyWord", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{2,9}(?:\|[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{2,9}){0,9}$", "关键词最少必须输入1个，最多10个,每个关键词最多不能超过9字，并且不能使用特殊字符");
        int Leibie = int.Parse(Utils.GetRequest("Leibie", "post", 1, @"^[0-9]\d*$", "0"));
        if (new BCW.BLL.Link().Exists(LinkUrl))
        {
            Utils.Error("您的网站已发布成功", "");
        }

        //是否刷屏
        string appName = "LIGHT_LINK";
        int Expir = Convert.ToInt32(ub.GetSub("LinkExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        //友链是否要审核
        int LinkAc = Convert.ToInt32(ub.GetSub("LinkIsAc", xmlPath));
        //友链ID
        int LID = new BCW.BLL.Link().GetMaxId();
        BCW.Model.Link model = new BCW.Model.Link();
        model.ID = LID;
        model.LinkName = LinkName;
        model.LinkNamt = LinkNamt;
        model.LinkUrl = LinkUrl;
        model.LinkNotes = LinkNotes;
        model.KeyWord = KeyWord;
        model.LinkIn = 0;
        model.LinkOut = 0;
        model.ReStats = "";
        model.ReLastIP = "";
        model.LinkTime = DateTime.Now;
        model.LinkTime2 = DateTime.Now;
        model.AddTime = DateTime.Now;
        model.Leibie = Leibie;
        model.LinkRd = 0;
        model.Hidden = LinkAc;
        new BCW.BLL.Link().Add(model);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布网站成功");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请在您的网站显眼位置投放我们的网站");
        if (LinkAc == 0)
        {
            builder.Append("<br />待管理员审核方能显示在我站友链");
        }
        builder.Append("<br />名称:" + ub.GetSub("LinkwebName", xmlPath) + "<br />");
        builder.Append("网址:" + ub.GetSub("LinkDomain", xmlPath) + "/?kid=" + LID + "<br />");
        if (!string.IsNullOrEmpty(ub.GetSub("LinkSummary", xmlPath)))
            builder.Append("简介:" + ub.GetSub("LinkSummary", xmlPath) + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=view&amp;ID=" + LID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看效果</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        Master.Title = "查看网站";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Link().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Link model = new BCW.BLL.Link().GetLink(id);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("名称:"+model.LinkName);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        if (model.Leibie != 0)
        {
            //取分类名称
            string Leibie = ub.GetSub("LinkLeibie", xmlPath);
            string sName = string.Empty;
            string[] sTemp = Leibie.Split("|".ToCharArray());
            for (int j = 0; j < sTemp.Length; j++)
            {
                if (j % 2 == 0)
                {
                    if (Convert.ToInt32(sTemp[j]) == model.Leibie)
                        sName = sTemp[j + 1].ToString();
                }
            }

            builder.Append("分类:<a href=\"" + Utils.getUrl("link.aspx?leibie=" + model.Leibie + "") + "\">" + sName + "</a><br />");
        }
        builder.Append("地址:" + model.LinkUrl + "");
        builder.Append("<br />简介:" + model.LinkNotes + "");
        builder.Append("<br />回链:http://" + Utils.GetDomain() + "/?kid=" + id + "");
        if (ub.GetSub("LinkIsView", xmlPath) == "0")
        {
            builder.Append("<br />=Pv统计=<br />");
            builder.Append("今日链入:" + model.todayIn + "");
            builder.Append(",链出:" + model.todayOut + "<br />");
            builder.Append("昨日链入:" + model.yesterdayIn + "");
            builder.Append(",链出:" + model.yesterdayOut + "<br />");
            builder.Append("前日链入:" + model.beforeIn + "");
            builder.Append(",链出:" + model.beforeOut + "<br />");
            builder.Append("总计链入:" + model.LinkIn + "");
            builder.Append(",链出:" + model.LinkOut + "<br />");
            builder.Append("=IP统计=<br />");
            builder.Append("今日链入:" + model.IPtodayIn + "");
            builder.Append(",链出:" + model.IPtodayOut + "<br />");
            builder.Append("昨日链入:" + model.IPyesterdayIn + "");
            builder.Append(",链出:" + model.IPyesterdayOut + "<br />");
            builder.Append("前日链入:" + model.IPbeforeIn + "");
            builder.Append(",链出:" + model.IPbeforeOut + "<br />");
            builder.Append("总计链入:" + model.LinkIPIn + "");
            builder.Append(",链出:" + model.LinkIPOut + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        //快速评论
        string ReStats = model.ReStats;
        if (string.IsNullOrEmpty(ReStats))
            ReStats = "0|0";
        string sStats = string.Empty;
        if (v != 0 && model.ReLastIP != Ipaddr.IPAddress)
        {
            string[] arrReStats = ReStats.Split("|".ToCharArray());
            for (int i = 0; i < arrReStats.Length; i++)
            {
                if ((v - 1) == i)
                {
                    sStats += "|" + Convert.ToInt32(Convert.ToInt32(arrReStats[i]) + 1);
                }
                else
                {
                    sStats += "|" + arrReStats[i];
                }
            }
            sStats = Utils.Mid(sStats, 1, sStats.Length);
            new BCW.BLL.Link().UpdateReStats(id, sStats, Ipaddr.IPAddress);
        }
        else
        {
            sStats = ReStats;
        }

        string ReText = "支持|路过";
        string[] arrText = ReText.Split("|".ToCharArray());
        string[] arrsStats = sStats.Split("|".ToCharArray());
        for (int i = 0; i < arrsStats.Length; i++)
        {
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=view&amp;id=" + id + "&amp;v=" + (i + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + arrText[i] + "(" + arrsStats[i] + ")</a> ");
        }
        builder.Append("<br /><a href=\"link.aspx?id=" + id + "&amp;rd=" + new Rand().RandNumer(5) + "&amp;ve=" + Utils.getstrVe() + "\">&gt;立即访问</a>");
        builder.Append("<br />最后链入:" + DT.FormatDate(model.LinkTime, 5) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}