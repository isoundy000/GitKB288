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

public partial class Manage_link : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/link.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "管理友情链接";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "ok":
                OkPage();
                break;
            case "edit":
                EditPage();
                break;
            case "save":
                SavePage();
                break;
            case "pass":
                PassPage();
                break;
            case "del":
                DelPage();
                break;
            case "leibie":
                LeibiePage();
                break;
            case "view":
                ViewPage();
                break;
            case "iplist":
                IpListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "get", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理友链");
        if (leibie != 0)
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
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=0&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "") + "\">友链排行</a>");
            builder.Append("|未审核");
        }
        else
        {
            builder.Append("友链排行|");
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=1&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "") + "\">未审核</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (v == 0)
                builder.Append("进出比|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=" + ptype + "&amp;leibie=" + leibie + "&amp;v=0&amp;backurl=" + Utils.getPage(0) + "") + "\">进出比</a>|");

            if (v == 1)
                builder.Append("链入|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=" + ptype + "&amp;leibie=" + leibie + "&amp;v=1&amp;backurl=" + Utils.getPage(0) + "") + "\">链入</a>|");

            if (v == 2)
                builder.Append("链出|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=" + ptype + "&amp;leibie=" + leibie + "&amp;v=2&amp;backurl=" + Utils.getPage(0) + "") + "\">链出</a>|");


            if (v == 3)
                builder.Append("链入IP|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=" + ptype + "&amp;leibie=" + leibie + "&amp;v=3&amp;backurl=" + Utils.getPage(0) + "") + "\">链入IP</a>|");

            if (v == 4)
                builder.Append("链出IP");
            else
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?ptype=" + ptype + "&amp;leibie=" + leibie + "&amp;v=4&amp;backurl=" + Utils.getPage(0) + "") + "\">链出IP</a>");


            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "ptype","id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件

        if (ptype == 1)
            strWhere = "Hidden=0";
        else
            strWhere = "Hidden=1";

        if (leibie != 0)
            strWhere += " and leibie=" + leibie + "";

        // 排序条件
        if (v == 0)
            strOrder = "LinkIn-LinkOut Desc";
        else if (v == 1)
            strOrder = "LinkIn Desc";
        else if (v == 2)
            strOrder = "LinkOut Desc";
        else if (v == 3)
            strOrder = "LinkIPIn Desc";
        else if (v == 4)
            strOrder = "LinkIPOut Desc";


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
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>");
                string strPass = string.Empty;
                if (ptype == 1)
                    strPass = "<a href=\"" + Utils.getUrl("link.aspx?act=pass&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核]</a>";
                else
                    strPass = "[<a href=\"" + Utils.getUrl("link.aspx?act=iplist&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">IP</a>]";

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("link.aspx?act=view&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>" + strPass + "<br />{3}", (pageIndex - 1) * pageSize + k, n.ID, n.LinkName, Utils.FormatString(n.LinkNotes, 20));
                builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");

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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=add&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加新网站</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=leibie&amp;backurl=" + Utils.getPage(0) + "") + "\">按分类查看</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=iplist") + "\">今天IP数据</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void IpListPage()
    {
        Master.Title = "查看IP";
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (id > 0)
        {
            builder.Append(new BCW.BLL.Link().GetLinkName(id));
        }
        builder.Append("IP记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=iplist&amp;ptype=0&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("链入IP|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=iplist&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">链入IP</a>|");

        if (ptype == 2)
            builder.Append("链出IP");
        else
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=iplist&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">链出IP</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id > 0)
            strWhere += "LinkId=" + id + "";

        if (ptype > 0)
        {
            if (strWhere != "")
                strWhere += " and Types=" + (ptype - 1) + "";
            else
                strWhere += "Types=" + (ptype - 1) + "";

        }
        // 开始读取列表
        IList<BCW.Model.LinkIp> listLinkIp = new BCW.BLL.LinkIp().GetLinkIps(pageIndex, pageSize, strWhere, out recordCount);
        if (listLinkIp.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.LinkIp n in listLinkIp)
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
                string ipCity = new IPSearch().GetAddressWithIP(n.AddUsIP);
                if (!string.IsNullOrEmpty(ipCity))
                {
                    ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
                }

                builder.Append("["+((pageIndex - 1) * pageSize + k)+"]");
                builder.Append(ipCity + "" + n.AddUsIP + "");
                builder.Append("浏览器:" + n.AddUsUA + "");

                if (!string.IsNullOrEmpty(n.AddUsPage))
                {
                    if (n.Types == 0)
                        builder.Append("来访页:" + Out.UBB(n.AddUsPage) + "");
                    else
                        builder.Append("去访页:" + Out.UBB(n.AddUsPage) + "");

                }
                builder.Append("[" + DT.FormatDate(n.AddTime, 12) + "]");
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
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:IP记录只保留当天的记录.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (id > 0)
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看友链</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("link.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布新网站");
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

        string strText = "网站名称:限10字/,网站简称:2-3字/,网站地址:/,网站简介:限500字/,关键字:如“下载 图铃 音乐”等，用|号分隔/,是否推荐:/" + sText + ",,";
        string strName = "LinkName,LinkNamt,LinkUrl,LinkNotes,KeyWord,LinkRd" + sName + ",act,backurl";
        string strType = "text,text,text,textarea,text,select" + sType + ",hidden,hidden";
        string strValu = "''http://'''0" + sValu + "'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,true,false,0|不推荐|1|推荐" + sEmpt + ",false,false";
        string strIdea = "/";
        string strOthe = "提交网站|reset,link.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void OkPage()
    {
        string LinkName = Utils.GetRequest("LinkName", "post", 2, @"^[\s\S]{2,10}$", "请输入不超过10字的网站名称");
        string LinkNamt = Utils.GetRequest("LinkNamt", "post", 2, @"^(?:[\u4E00-\u9FA5]{2,3}|[\w\-\.]{2,5})$", "请输入正确的网站简称，中文限2-3字，字母或数字限2-5字");
        string LinkUrl = Utils.GetRequest("LinkUrl", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请输入合法的网址");
        string LinkNotes = Utils.GetRequest("LinkNotes", "post", 3, @"^[\s\S]{0,500}$", "请输入不超过500字的网站简介");
        string KeyWord = Utils.GetRequest("KeyWord", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{2,9}(?:\|[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{2,9}){0,9}$", "关键词最少必须输入1个，最多10个,每个关键词最多不能超过9字，并且不能使用特殊字符");
        int Leibie = int.Parse(Utils.GetRequest("Leibie", "post", 1, @"^[0-9]\d*$", "0"));
        int LinkRd = int.Parse(Utils.GetRequest("LinkRd", "post", 2, @"^[0-1]$", "是否推荐选择错误"));
        if (new BCW.BLL.Link().Exists(LinkUrl))
        {
            Utils.Error("该网站已发布成功", "");
        }
        //友链是否要审核
        int LinkAc = 1;
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
        model.LinkRd = LinkRd;
        model.Hidden = LinkAc;
        new BCW.BLL.Link().Add(model);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布网站成功");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("添加网站成功");
        builder.Append("<br />名称:" + ub.GetSub("LinkwebName", xmlPath) + "<br />");
        builder.Append("网址:" + ub.GetSub("LinkDomain", xmlPath) + "/?kid=" + LID + "<br />");
        if (!string.IsNullOrEmpty(ub.GetSub("LinkSummary", xmlPath)))
            builder.Append("简介:" + ub.GetSub("LinkSummary", xmlPath) + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("link.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void PassPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Link().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        new BCW.BLL.Link().UpdateHidden(id);
        Utils.Success("审核成功", "审核成功，正在返回..", Utils.getUrl("link.aspx?ptype=1"), "2");
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Link().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Link model = new BCW.BLL.Link().GetLink(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改网站");
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
            sValu = "'" + model.Leibie + "";
            sEmpt = "," + ub.GetSub("LinkLeibie", xmlPath) + "";
        }

        string strText = "网站名称:限10字/,网站简称:2-3字/,网站地址:/,网站简介:限500字/,关键字:如“下载 图铃 音乐”等，用|号分隔/,链入PV总量:/,链出PV总量:/,链入IP总量:/,链出IP总量:/,是否推荐:/,审核性质:/" + sText + ",,,";
        string strName = "LinkName,LinkNamt,LinkUrl,LinkNotes,KeyWord,LinkIn,LinkOut,LinkIPIn,LinkIPOut,LinkRd,Hidden" + sName + ",id,act,backurl";
        string strType = "text,text,text,textarea,text,num,num,num,num,select,select" + sType + ",hidden,hidden,hidden";
        string strValu = "" + model.LinkName + "'" + model.LinkNamt + "'" + model.LinkUrl + "'" + model.LinkNotes + "'" + model.KeyWord + "'" + model.LinkIn + "'" + model.LinkOut + "'" + model.LinkIPIn + "'" + model.LinkIPOut + "'" + model.LinkRd + "'" + model.Hidden + "" + sValu + "'" + id + "'save'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,true,false,false,false,false,false,0|未推荐|1|已推荐,0|未审核|1|已审核" + sEmpt + ",false,false,false";
        string strIdea = "/";
        string strOthe = "确定修改|reset,link.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getPage("link.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void SavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Link().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        string LinkName = Utils.GetRequest("LinkName", "post", 2, @"^[\s\S]{2,10}$", "请输入不超过10字的网站名称");
        string LinkNamt = Utils.GetRequest("LinkNamt", "post", 2, @"^(?:[\u4E00-\u9FA5]{2,3}|[\w\-\.]{2,5})$", "请输入正确的网站简称，中文限2-3字，字母或数字限2-5字");
        string LinkUrl = Utils.GetRequest("LinkUrl", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请输入合法的网址");
        string LinkNotes = Utils.GetRequest("LinkNotes", "post", 3, @"^[\s\S]{0,500}$", "请输入不超过500字的网站简介");
        string KeyWord = Utils.GetRequest("KeyWord", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{2,9}(?:\|[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{2,9}){0,9}$", "关键词最少必须输入1个，最多10个,每个关键词最多不能超过9字，并且不能使用特殊字符");
        int LinkIn = int.Parse(Utils.GetRequest("LinkIn", "post", 2, @"^[0-9]\d*$", "链入PV填写错误"));
        int LinkOut = int.Parse(Utils.GetRequest("LinkOut", "post", 2, @"^[0-9]\d*$", "链出PV填写错误"));
        int LinkIPIn = int.Parse(Utils.GetRequest("LinkIPIn", "post", 2, @"^[0-9]\d*$", "链入IP填写错误"));
        int LinkIPOut = int.Parse(Utils.GetRequest("LinkIPOut", "post", 2, @"^[0-9]\d*$", "链出IP填写错误"));
        int LinkRd = int.Parse(Utils.GetRequest("LinkRd", "post", 2, @"^[0-1]$", "是否推荐选择错误"));
        int Leibie = int.Parse(Utils.GetRequest("Leibie", "post", 1, @"^[0-9]\d*$", "0"));
        int Hidden = int.Parse(Utils.GetRequest("Hidden", "post", 2, @"^[0-1]$", "审核性质选择错误"));

        BCW.Model.Link model = new BCW.Model.Link();
        model.ID = id;
        model.LinkName = LinkName;
        model.LinkNamt = LinkNamt;
        model.LinkUrl = LinkUrl;
        model.LinkNotes = LinkNotes;
        model.KeyWord = KeyWord;
        model.LinkIn = LinkIn;
        model.LinkOut = LinkOut;
        model.LinkIPIn = LinkIPIn;
        model.LinkIPOut = LinkIPOut;
        model.LinkRd = LinkRd;
        model.Leibie = Leibie;
        model.Hidden = Hidden;
        new BCW.BLL.Link().Update(model);
        Utils.Success("修改成功", "修改成功，正在返回..", Utils.getUrl("link.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除友链";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此友链记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("link.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Link().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Link().Delete(id);
            Utils.Success("删除友链", "删除友链成功..", Utils.getPage("link.aspx"), "1");
        }
    }

    private void LeibiePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看友链分类");
        builder.Append(Out.Tab("</div>", ""));

        string strLeibie = ub.GetSub("LinkLeibie", xmlPath);
        string[] sTemp = strLeibie.Split("|".ToCharArray());
        for (int j = 0; j < sTemp.Length; j++)
        {
            if (j % 2 == 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("" + sTemp[j] + ".<a href=\"" + Utils.getUrl("link.aspx?leibie=" + sTemp[j] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + sTemp[j + 1].ToString() + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getPage("link.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

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

        builder.Append("网站名称:" + model.LinkName);

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("地址:<a href=\"/link.aspx?act=view&amp;id=" + id + "&amp;ve=" + Utils.getstrVe() + "\">" + model.LinkUrl + "</a>");
        builder.Append("<br />简介:" + model.LinkNotes + "");
        builder.Append("<br />回链:http://" + Utils.GetDomain() + "/?kid=" + id + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("=Pv统计=<br />");
        builder.Append("今日链入:" + model.todayIn + "");
        builder.Append(",链出:" + model.todayOut + "<br />");
        builder.Append("昨日链入:" + model.yesterdayIn + "");
        builder.Append(",链出:" + model.yesterdayOut + "<br />");
        builder.Append("前日链入:" + model.beforeIn + "");
        builder.Append(",链出:" + model.beforeOut + "<br />");
        builder.Append("总计链入:" + model.LinkIn + "");
        builder.Append(",链出:" + model.LinkOut + "<br />");
        //builder.Append("总计有效链入:" + model.InOnly + "<br />");
        builder.Append("=IP统计=<br />");
        builder.Append("今日链入:" + model.IPtodayIn + "");
        builder.Append(",链出:" + model.IPtodayOut + "<br />");
        builder.Append("昨日链入:" + model.IPyesterdayIn + "");
        builder.Append(",链出:" + model.IPyesterdayOut + "<br />");
        builder.Append("前日链入:" + model.IPbeforeIn + "");
        builder.Append(",链出:" + model.IPbeforeOut + "<br />");
        builder.Append("总计链入:" + model.LinkIPIn + "");
        builder.Append(",链出:" + model.LinkIPOut + "");
        builder.Append("<br />最后链入" + model.LinkTime + "");
        builder.Append("<br />最后链出" + model.LinkTime2 + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("link.aspx?act=iplist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看详细IP</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("link.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
