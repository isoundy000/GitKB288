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

public partial class Manage_filepass : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "verify":
                VerifyPage();
                break;
            case "verifypage":
                VerifypagePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "审核文件";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("未审核文件");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=13 and Hidden=1";

        // 开始读取文章
        IList<BCW.Model.Detail> listDetail = new BCW.BLL.Detail().GetDetails(pageIndex, pageSize, strWhere, out recordCount);
        if (listDetail.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Detail n in listDetail)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string IsHidden = string.Empty;
                if (n.Hidden == 1)
                    IsHidden = "＾";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("classact.aspx?act=info&amp;ptype={0}&amp;nid={1}&amp;id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a><a href=\"" + Utils.getUrl("/detail.aspx?id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + IsHidden + "{3}</a><a href=\"" + Utils.getUrl("filepass.aspx?act=verify&amp;id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核]</a><a href=\"" + Utils.getUrl("class.aspx?act=del2&amp;ptype={0}&amp;id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a><br />上传者:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>(" + DT.FormatDate(n.AddTime, 1) + ")", n.Types, n.NodeId, n.ID, n.Title);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("text", "没有相关记录"));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("filepass.aspx?act=verifypage&amp;page=" + pageIndex + "") + "\">审核本页文件</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void VerifyPage()
    {
        Master.Title = "审核文件";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定审核文件吗");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "奖励" + ub.Get("SiteBz") + "/,,,";
            string strName = "cent,id,act,info";
            string strType = "num,hidden,hidden,hidden";
            string strValu = "0'" + id + "'verify'ok";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定审核并内线,filepass.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("filepass.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("filepass.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            long cent = int.Parse(Utils.GetRequest("cent", "post", 4, @"^[1-9]\d*$", "奖励金额填写错误"));
            new BCW.BLL.Detail().UpdateHidden(id, 0);
            int UsID = new BCW.BLL.Detail().GetUsID(id);
            if (cent > 0)
            {
                string mename = new BCW.BLL.User().GetUsName(UsID);
                new BCW.BLL.User().UpdateiGold(UsID, mename, cent, "上传文件通过审核奖币");
                //发信给上传会员
                new BCW.BLL.Guest().Add(UsID, mename, "您的上传的[url=/detail.aspx?id=" + id + "]" + new BCW.BLL.Detail().GetTitle(id) + "[/url]已通过审核，恭喜你获得" + cent + "" + ub.Get("SiteBz") + "奖励！");
            }
            Utils.Success("审核文件", "审核文件成功，正在返回..", Utils.getUrl("filepass.aspx"), "1");
        }
    }

    private void VerifypagePage()
    {

        Master.Title = "审核本页文件";
        int page = int.Parse(Utils.GetRequest("page", "all", 2, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定审核本页文件吗");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "奖励" + ub.Get("SiteBz") + "/,,,";
            string strName = "cent,page,act,info";
            string strType = "num,hidden,hidden,hidden";
            string strValu = "0'" + page + "'verifypage'ok";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定审核并内线,filepass.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("filepass.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("filepass.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            long cent = int.Parse(Utils.GetRequest("cent", "post", 4, @"^[1-9]\d*$", "奖励金额填写错误"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

            string strWhere = "";
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "Types=13 and Hidden=1";

            // 开始读取列表
            IList<BCW.Model.Detail> listDetail = new BCW.BLL.Detail().GetDetails(pageIndex, pageSize, strWhere, out recordCount);
            if (listDetail.Count > 0)
            {
                foreach (BCW.Model.Detail n in listDetail)
                {
                    new BCW.BLL.Detail().UpdateHidden(n.ID, 0);
                    if (cent > 0)
                    {
                        string mename = new BCW.BLL.User().GetUsName(n.UsID);
                        new BCW.BLL.User().UpdateiGold(n.UsID, mename, cent, "上传文件通过审核奖币");
                        //发信给上传会员
                        new BCW.BLL.Guest().Add(n.UsID, mename, "您的上传的[url=/detail.aspx?id=" + n.ID + "]" + n.Title + "[/url]已通过审核，恭喜你获得" + cent + "" + ub.Get("SiteBz") + "奖励！");
                    }
                }
            }

            Utils.Success("审核本页文件", "审核本页文件成功，正在返回..", Utils.getUrl("filepass.aspx"), "1");
        }
    }
}