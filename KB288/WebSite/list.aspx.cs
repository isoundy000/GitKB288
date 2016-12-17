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
public partial class list : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/front.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Topics().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
        Master.Title = model.Title;
        //顶部调用
        string TopUbb = string.Empty;
        if (model.Types == 11)
        {
            TopUbb = ub.GetSub("FtTextListTop", xmlPath);
        }
        else if (model.Types == 12)
        {
            TopUbb = ub.GetSub("FtPicListTop", xmlPath);
        }
        else if (model.Types == 13)
        {
            TopUbb = ub.GetSub("FtFileListTop", xmlPath);
        }
        if (TopUbb != "")
        {
            TopUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(TopUbb));
            if (TopUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(TopUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(TopUbb);
            }
        }

        //机型适配
        string PhoneBrand = "";
        string PhoneModel = "";
        string PhoneSystem = "";
        if (model.Types == 13)
        {
            if (Request.Cookies["BrandComment"] != null)
            {
                PhoneBrand = HttpUtility.UrlDecode(Request.Cookies["BrandComment"]["PhoneBrand"]);
                PhoneModel = HttpUtility.UrlDecode(Request.Cookies["BrandComment"]["PhoneModel"]);
                PhoneSystem = HttpUtility.UrlDecode(Request.Cookies["BrandComment"]["PhoneSystem"]);
            }

            if (PhoneBrand != "" && PhoneModel != "")
            {
                if (Request["view"] != "all")
                {
                    Master.Title = PhoneModel + "-" + model.Title;
                    //builder.Append(Out.Tab("<div class=\"title\">" + PhoneModel + "-" + model.Title + "</div>", ""));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("list.aspx?id=" + id + "&amp;view=all&amp;backurl=" + Utils.getPage(0) + "") + "\">查看全部</a>&gt;" + PhoneModel + "系列");
                }
                else
                {
                    //builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "</div>", ""));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("查看全部&gt;<a href=\"" + Utils.getUrl("list.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + PhoneModel + "系列</a>");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        //else
        //{
        //    builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "</div>", ""));
        //}

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id", "view", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "NodeId=" + id + " and Hidden=0";
        if (model.Types == 13)
        {
            if (!string.IsNullOrEmpty(PhoneModel) && Request["view"] != "all")
                strWhere += " AND (Model LIKE '%" + (PhoneModel) + "%' Or Model LIKE '%" + PhoneSystem + "%' OR Model LIKE '%" + (PhoneBrand) + "%')";
        }
        // 开始读取列表
        IList<BCW.Model.Detail> listDetail = new BCW.BLL.Detail().GetDetails(pageIndex, pageSize, strWhere, out recordCount);
        if (listDetail.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Detail n in listDetail)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("detail.aspx?id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.Title);
                if (!string.IsNullOrEmpty(n.Cover) && IsCover(n.Types) == true)
                {
                    builder.Append("<br /><img src=\"" + n.Cover + "\" alt=\"load\"/>");
                }

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
        //上传连接
        if (model.Types == 13 && Utils.GetTopDomain().Equals("tl88.cc"))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("addfile.aspx?nid=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;我要上传</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        //底部调用
        string FootUbb = string.Empty;
        if (model.Types == 11)
        {
            FootUbb = ub.GetSub("FtTextListFoot", xmlPath);
        }
        else if (model.Types == 12)
        {
            FootUbb = ub.GetSub("FtPicListFoot", xmlPath);
        }
        else if (model.Types == 13)
        {
            FootUbb = ub.GetSub("FtFileListFoot", xmlPath);
        }
        if (FootUbb != "")
        {
            FootUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(FootUbb));
            if (FootUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(FootUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(TopUbb);
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
        if (model.NodeId != 0)
        {
            builder.Append("-<a href=\"" + Utils.getUrl("default.aspx?id=" + model.NodeId + "") + "\">" + new BCW.BLL.Topics().GetTitle(model.NodeId) + "</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

    }

    private bool IsCover(int ptype)
    {
        bool bl = false;
        if (ptype == 11)
        {
            if (ub.GetSub("FtTextCover", xmlPath) == "0")
            {
                bl = true;
            }
        }
        else if (ptype == 12)
        {
            if (ub.GetSub("FtPicCover", xmlPath) == "0")
            {
                bl = true;
            }
        }
        else if (ptype == 13)
        {
            if (ub.GetSub("FtFileCover", xmlPath) == "0")
            {
                bl = true;
            }
        }

        return bl;
    }
}
