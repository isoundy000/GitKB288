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

public partial class Manage_spaceapp_Gsadmin : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "高手系统管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "config":
                ConfigPage();
                break;
            case "top":
                TopPage();
                break;
            case "toplist":
                TopListPage();
                break;
            case "cent":
                CentPage();
                break;
            case "centlist":
                CentListPage();
                break;
            case "editcent":
                EditCentPage();
                break;
            case "editcentsave":
                EditCentSavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("高手系统管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=config") + "\">设置期数</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/Gsset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">高手设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=top") + "\">各坛排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=cent") + "\">奖励记录</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ConfigPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bbs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string GsqiNum = Utils.GetRequest("GsqiNum", "post", 2, @"^[0-9]\d*$", "期数填写错误");
            string GsStopTime = Utils.GetRequest("GsStopTime", "post", 2, DT.RegexTime, "截止发布时间填写错误");

            //全部参赛论坛还有其中的论坛版主未开奖，管理员就不能开出下期------》》》但可以修改截止时间
            int qiNum = Convert.ToInt32(ub.GetSub("BbsGsqiNum", xmlPath));
            if (GsqiNum != qiNum.ToString())
            {
                if (new BCW.BLL.Forumvote().ExistsKz())
                {
                    Utils.Error("上期还没有开奖完成不能开通下期（改变期数），可修改截止时间", "");
                }
            }

            xml.dss["BbsGsqiNum"] = GsqiNum;
            xml.dss["BbsGsStopTime"] = GsStopTime;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("社区系统设置", "设置成功，正在返回..", Utils.getUrl("Gsadmin.aspx?act=config"), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("设置期数");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "本期期数:/,本期截止发布时间:/,";
            string strName = "GsqiNum,GsStopTime,act";
            string strType = "num,date,hidden";
            string strValu = "" + xml.dss["BbsGsqiNum"] + "'" + xml.dss["BbsGsStopTime"] + "'config";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确定修改,Gsadmin.aspx,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void TopPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("分类排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=113") + "\">六肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=114") + "\">波色区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=116") + "\">平特一肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=117") + "\">五不中区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=115") + "\">大小区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=121") + "\">单双区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=122") + "\">家野区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=119") + "\">五尾区</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void TopListPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        Master.Title = "高手排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        string ForumTitle = new BCW.BLL.Forum().GetTitle(forumid);
        builder.Append("" + ForumTitle + "-排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 0)
            builder.Append("连中榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=" + forumid + "&amp;ptype=0") + "\">连中榜</a>|");

        if (ptype == 1)
            builder.Append("月中榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=" + forumid + "&amp;ptype=1") + "\">月中榜</a>|");

        if (ptype == 2)
            builder.Append("历史");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=toplist&amp;forumid=" + forumid + "&amp;ptype=2") + "\">历史</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "ForumId=" + forumid + " and Types=8";
        if (ptype == 0)
        {
            strWhere += " and Glznum>0";
            strOrder = "Glznum DESC";
        }
        else if (ptype == 1)
        {
            strWhere += " and Gmnum>0";
            strOrder = "Gmnum DESC";
        }
        else if (ptype == 2)
        {
            strWhere += " and Gwinnum>0";
            strOrder = "Gwinnum DESC";
        }

        // 开始读取列表
        IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTextsGs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listText.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Text n in listText)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (ptype == 0)
                    builder.Append("连中" + n.Glznum + "");
                else if (ptype == 1)
                    builder.Append("月中" + n.Gmnum + "");
                else
                    builder.Append("" + n.Gaddnum + "中" + n.Gwinnum + "");

                builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");

                builder.Append("|<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=" + n.ForumId + "&amp;hid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">奖励记录</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=top") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void CentPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("分类奖励记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=113") + "\">六肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=114") + "\">波色区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=116") + "\">平特一肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=117") + "\">五不中区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=115") + "\">大小区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=121") + "\">单双区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=122") + "\">家野区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=centlist&amp;forumid=119") + "\">五尾区</a>");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入用户ID:/,";
        string strName = "hid,act";
        string strType = "num,hidden";
        string strValu = "'centlist";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜奖励记录,Gsadmin.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void CentListPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));

        string Title = string.Empty;
        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[0-9]\d*$", "0"));
        if (hid > 0)
        {
            Title = new BCW.BLL.User().GetUsName(hid);
            if (Title == "")
                Utils.Error("不存在的会员记录", "");

            Title = "<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "(" + hid + ")</a>";

        }
        else
        {
            Title = new BCW.BLL.Forum().GetTitle(forumid);
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Title + "-奖励记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (hid > 0)
            strWhere = "UsID=" + hid + "";
        else
            strWhere = "ForumId=" + forumid + "";

        // 开始读取列表
        IList<BCW.Model.Forumvotelog> listForumvotelog = new BCW.BLL.Forumvotelog().GetForumvotelogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumvotelog.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Forumvotelog n in listForumvotelog)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (hid == 0)
                {
                    builder.Append("奖励会员:<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a><br />");
                }
                builder.Append("主题:<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + n.ForumId + "&amp;bid=" + n.BID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + DT.FormatDate(n.AddTime, 1) + ")");
                builder.Append("<br />" + n.Notes);
                builder.Append("<a href=\"" + Utils.getUrl("Gsadmin.aspx?act=editcent&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">【编辑】</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getPage("Gsadmin.aspx?act=cent") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditCentPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        BCW.Model.Forumvotelog model = new BCW.BLL.Forumvotelog().GetForumvotelog(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑奖励");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "用户ID:/,用户昵称:/,管理员ID:/,主题标题:/,cID:/,论坛ID:/,日志内容:/,添加时间:/,,,";
        string strName = "UsID,UsName,AdminId,Title,BID,ForumId,Notes,AddTime,id,act,backurl";
        string strType = "num,text,num,text,num,num,textarea,date,hidden,hidden,hidden";
        string strValu = "" + model.UsID + "'" + model.UsName + "'" + model.AdminId + "'" + model.Title + "'" + model.BID + "'" + model.ForumId + "'" + model.Notes + "'" + DT.FormatDate(model.AddTime, 0) + "'" + id + "'editcentsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,Gsadmin.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getPage("Gsadmin.aspx?act=cent") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditCentSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[^\^]{1,50}$", "昵称不超50字");
        int AdminId = int.Parse(Utils.GetRequest("AdminId", "post", 2, @"^[0-9]\d*$", "管理员ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,50}$", "主题标题限1-50字");
        int BID = int.Parse(Utils.GetRequest("BID", "post", 2, @"^[1-9]\d*$", "BIDID错误"));
        int ForumId = int.Parse(Utils.GetRequest("ForumId", "post", 2, @"^[1-9]\d*$", "论坛ID错误"));
        string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,500}$", "请输入不超500字的日志内容");
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "添加时间填写出错"));

        BCW.Model.Forumvotelog model = new BCW.Model.Forumvotelog();
        model.ID = id;
        model.UsID = UsID;
        model.UsName = UsName;
        model.AdminId = AdminId;
        model.Title = Title;
        model.BID = BID;
        model.ForumId = ForumId;
        model.Notes = Notes;
        model.AddTime = AddTime;
        new BCW.BLL.Forumvotelog().Update(model);

        Utils.Success("编辑奖励日志", "编辑奖励日志成功..", Utils.getUrl("Gsadmin.aspx?act=editcent&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
}
