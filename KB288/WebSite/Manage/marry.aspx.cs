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

public partial class Manage_marry : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "婚恋管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("婚恋管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("恋人|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?ptype=1") + "\">恋人</a>|");

        if (ptype == 2)
            builder.Append("已婚|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?ptype=2") + "\">已婚</a>|");

        if (ptype == 3)
            builder.Append("离异");
        else
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?ptype=3") + "\">离异</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1)
            strWhere = "Types=0";
        else if (ptype == 2)
            strWhere = "Types=1";
        else if (ptype == 3)
            strWhere = "Types=2";
        else
            strWhere = "Types>-1";

        if (uid > 0)
            strWhere += " and (usid=" + uid + " or reid=" + uid + ")";

        // 开始读取列表
        IList<BCW.Model.Marry> listMarry = new BCW.BLL.Marry().GetMarrys(pageIndex, pageSize, strWhere, out recordCount);
        if (listMarry.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Marry n in listMarry)
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

                string sText = string.Empty;
                if (n.AcUsID == 0 || n.UsID == n.AcUsID)
                    sText = "" + n.UsName + "与" + n.ReName + "";
                else
                    sText = "" + n.ReName + "与" + n.UsName + "";

                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">[管理]&gt;</a>" + ((pageIndex - 1) * pageSize + k) + ".");

                if (n.Types == 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + sText + "恋爱啦</a>");

                }
                else if (n.Types == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + sText + "结婚啦</a>");
                }
                else if (n.Types == 2)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + sText + "已离婚</a>");
                }
                if (n.Types == 0)
                    builder.Append("(" + DT.FormatDate(n.AddTime, 1) + ")");
                else
                    builder.Append("(" + DT.FormatDate(n.AcTime, 1) + ")");

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
        string strText = "输入用户ID:/,";
        string strName = "uid,ptype";
        string strType = "num,hidden";
        string strValu = "'" + ptype + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜记录,marry.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xml/marryset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">婚姻配置</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Marry model = new BCW.BLL.Marry().GetMarry(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types < 0)
        {
            Utils.Error("不存在的记录", "");
        }
        string sTitle = string.Empty;
        if (model.Types == 0)
            sTitle = "查看恋爱";
        else if (model.Types == 1)
            sTitle = "查看结婚";
        else
            sTitle = "查看离婚";

        Master.Title = sTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(sTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (model.Types == 0)
        {
            builder.Append("追求人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
            builder.Append("被追求人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
            builder.Append("追求时间:" + DT.FormatDate(model.AddTime, 3) + "");
        }
        else if (model.Types == 1)
        {
            if (model.AcUsID == model.UsID)
            {
                builder.Append("结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
                builder.Append("被结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
            }
            else
            {
                builder.Append("结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
                builder.Append("被结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
            }
            builder.Append("爱情誓言:" + model.Oath + "<br />");
            builder.Append("接受时间:" + DT.FormatDate(model.AcTime, 3) + "");
        }
        else
        {
            if (model.AcUsID == model.UsID)
            {
                builder.Append("离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
                builder.Append("被离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
            }
            else
            {
                builder.Append("离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
                builder.Append("被离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
            }
            builder.Append("离婚原因:" + model.Oath2 + "<br />");
            builder.Append("离婚时间:" + DT.FormatDate(model.AcTime, 3) + "");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除婚恋";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不能恢复！确定删除此婚恋记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("marry.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Marry().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Marry().Delete(id);
            Utils.Success("删除婚恋", "删除婚恋记录成功..", Utils.getUrl("marry.aspx"), "1");
        }
    }
}
