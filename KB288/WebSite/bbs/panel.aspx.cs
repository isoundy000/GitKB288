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

public partial class bbs_panel : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "控制面板";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "change":
                ChangePage(meid);
                break;
            case "add":
                AddPage(meid);
                break;
            case "del":
                DelPage(meid);
                break;
            case "sort":
                SortPage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }
    private void ReloadPage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        int pn = int.Parse(Utils.GetRequest("pn", "get", 1, @"^[0-1]$", "-1"));
        if (pn != -1)
            new BCW.BLL.Panel().UpdateIsBr(uid, pn);

        builder.Append(Out.Tab("<div class=\"title\">控制面板</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        bool Isps = false;
        DataSet ds = new BCW.BLL.Panel().GetList("ID,Title,PUrl,IsBr", "UsID=" + uid + " Order By Paixu asc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl(ds.Tables[0].Rows[i]["PUrl"].ToString()) + "\">" + ds.Tables[0].Rows[i]["Title"].ToString() + "</a>");
                if (ptype == 1)
                    builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?act=del&amp;id=" + ds.Tables[0].Rows[i]["ID"].ToString() + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");

                if (ds.Tables[0].Rows[i]["IsBr"].ToString() == "1")
                    Isps = true;
                builder.Append("<br />");
            }
        }
        else
        {
            builder.Append("请在底部功能处增加面板..<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;管理模式</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换普通</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?act=sort&amp;backurl=" + Utils.getPage(0) + "") + "\">调整顺序</a> ");

        if (Isps == true)
            builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?pn=0&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">横向</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?pn=1&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">竖向</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ChangePage(int uid)
    {
        string info = Utils.GetRequest("info", "get", 1, "", "");
        string ichange = "0";
        if (info == "on")
            ichange = "1";
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        string GetForumSet = BCW.User.Users.GetForumSetData(ForumSet, ichange, 8);
        new BCW.BLL.User().UpdateForumSet(uid, GetForumSet);
        Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getPage(1), "1");
    }

    private void AddPage(int uid)
    {
        Master.Title = "添加面板";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string Purl = Out.UBB(Utils.removeUVe(Utils.getPage(1)));
        if (info == "ok")
        {
            string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,10}$", "面板名称限1-10字");
            BCW.Model.Panel model = new BCW.Model.Panel();
            model.UsId = uid;
            model.Title = Title;
            model.PUrl = Purl;
            model.IsBr = 0;
            model.Paixu = 0;
            new BCW.BLL.Panel().Add(model);
            Utils.Success("增加面板", "增加面板成功，正在返回..<br /><a href=\"" + Utils.getUrl("panel.aspx?backurl=" + Utils.getPage(0) + "") + "\">&gt;回控制面板</a>", Utils.getPage("panel.aspx"), "3");
        }
        else
        {
            string Title = string.Empty;
            string Purls = "http://" + Utils.GetDomain() + "" + Purl + "";
            Title = Utils.GetSourceTextByUrl(Utils.getUrl(Purls).Replace("&amp;", "&"));
            Title = Utils.GetTitle(Title);

            builder.Append(Out.Tab("<div class=\"title\">加入控制面板</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("输入面板名称");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",,,,";
            strName = "Title,act,info,backurl";
            strType = "text,hidden,hidden,hidden";
            strValu = "" + Title + "'add'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            strOthe = "&gt;确定添加,panel.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append(Out.back("取消"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?backurl=" + Utils.getPage(0) + "") + "\">控制面板</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Panel().Exists(id,uid))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除面板";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此面板记录吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?act=del&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("panel.aspx") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Panel().Delete(id);

            Utils.Success("删除面板", "删除面板成功，正在返回..", Utils.getUrl("panel.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    //private void AdelPage(int uid)
    //{
    //    string info = Utils.GetRequest("info", "all", 1, "", "");
    //    int pid = int.Parse(Utils.GetRequest("pid", "get", 1, @"^[0-9]\d*$", "0"));
    //    builder.Append(Out.Tab("<div class=\"title\">控制面板</div>", ""));
    //    builder.Append(Out.Tab("<div>", ""));
    //    builder.Append("请选择增/删控件面板:");
    //    builder.Append(Out.Tab("</div>", ""));
    //    string[] sName = { "空间", "短消息", "好友", "足迹" };
    //    string[] sUrl = { "uinfo.aspx", "sms.aspx", "friends.aspx", "his.aspx" };
    //    if (info == "del" && pid > 0)
    //    {
    //        new BCW.BLL.Panel().Delete(uid, sName[pid - 1].ToString(), sUrl[pid - 1].ToString());
    //        Utils.Success("删除面板", "删除面板成功，正在返回..", Utils.getUrl("panel.aspx?act=adel&amp;backurl=" + Utils.getPage(0) + ""), "1");
    //    }
    //    else if (info == "add" && pid > 0)
    //    {
    //        BCW.Model.Panel model = new BCW.Model.Panel();
    //        model.UsId = uid;
    //        model.Title = sName[pid - 1].ToString();
    //        model.PUrl = sUrl[pid - 1].ToString();
    //        model.IsBr = 0;
    //        model.Paixu = 0;
    //        new BCW.BLL.Panel().Add(model);
    //        Utils.Success("增加面板", "增加面板成功，正在返回..", Utils.getUrl("panel.aspx?act=adel&amp;backurl=" + Utils.getPage(0) + ""), "1");
    //    }
    //    else
    //    {
    //        int pageIndex;
    //        int recordCount;
    //        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
    //        string[] pageValUrl = { "act" };
    //        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
    //        if (pageIndex == 0)
    //            pageIndex = 1;

    //        //总记录数
    //        recordCount = sName.Length;

    //        int stratIndex = (pageIndex - 1) * pageSize;
    //        int endIndex = pageIndex * pageSize;
    //        int k = 0;
    //        for (int i = 0; i < sName.Length; i++)
    //        {
    //            if (k >= stratIndex && k < endIndex)
    //            {
    //                if ((k + 1) % 2 == 0)
    //                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
    //                else
    //                    builder.Append(Out.Tab("<div>", "<br />"));

    //                builder.Append("" + sName[i].ToString() + "");
    //                if (new BCW.BLL.Panel().Exists(uid, sName[i].ToString(), sUrl[i].ToString()))
    //                {
    //                    builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?act=adel&amp;info=del&amp;pid=" + (i + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">-删除</a>");
    //                }
    //                else
    //                {
    //                    builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?act=adel&amp;info=add&amp;pid=" + (i + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">+增加</a>");
    //                }
    //                builder.Append(Out.Tab("</div>", ""));
    //            }
    //            if (k == endIndex)
    //                break;
    //            k++;
    //        }

    //        // 分页
    //        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
    //    }
    //    builder.Append(Out.Tab("<div>", "<br />"));
    //    builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?backurl=" + Utils.getPage(0) + "") + "\">&gt;控制面板</a><br />");
    //    builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">&gt;返回上一级</a>");
    //    builder.Append(Out.Tab("</div>", ""));
    //}

    private void SortPage(int uid)
    {
        builder.Append(Out.Tab("<div class=\"title\">控制面板</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入编号:");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        DataSet ds = new BCW.BLL.Panel().GetList("ID,Title,PUrl,Paixu", "UsID=" + uid + " Order By Paixu asc");

        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("你还没有添加任何控制..", "");
        }
        if (info != "ok")
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string y = ",";
                strText = strText + y + ds.Tables[0].Rows[i]["Paixu"].ToString() + "." + ds.Tables[0].Rows[i]["Title"].ToString();
                strName = strName + y + "Norder" + i;
                strType = strType + y + "snum";
                strValu = strValu + "'" + ds.Tables[0].Rows[i]["Paixu"].ToString();
                strEmpt = strEmpt + y + "false";
            }
            strText = Utils.Mid(strText, 1, strText.Length) + ",,";
            strName = Utils.Mid(strName, 1, strName.Length) + ",info,act";
            strType = Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden";
            strValu = Utils.Mid(strValu, 1, strValu.Length) + "'ok'sort";
            strEmpt = Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,";
            strIdea = "/";
            strOthe = "&gt;提交|reset,panel.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                new BCW.BLL.Panel().UpdatePaixu(Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]), Convert.ToInt32(Request["Norder" + i + ""]));
            }
            Utils.Success("排序", "恭喜，排序成功，正在返回..", Utils.getUrl("panel.aspx?act=sort&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("*数字越小，排位越靠前");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("panel.aspx?backurl=" + Utils.getPage(0) + "") + "\">&gt;控制面板</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">&gt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}