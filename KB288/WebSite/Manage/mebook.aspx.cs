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

public partial class Manage_mebook : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "del":
                DelPage();
                break;
            case "delpage":
                DelpagePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "空间留言管理";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("空间留言管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = "MID=" + uid + "";

        // 开始读取列表
        IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex, pageSize, strWhere, out recordCount);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Mebook n in listMebook)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("mebook.aspx?act=del&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>{1}.{2}:{3}({4})", n.ID, (pageIndex - 1) * pageSize + k, n.MName, n.MContent, DT.FormatDate(n.AddTime, 1));
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
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜空间留言,mebook.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=delpage&amp;uid=" + uid + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除本页留言</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除留言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此留言记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("mebook.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Mebook().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Mebook().Delete(id);
            Utils.Success("删除成功", "删除留言成功..", Utils.getPage("mebook.aspx"), "1");
        }
    }

    private void DelpagePage()
    {

        Master.Title = "删除本页留言";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除本页留言吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=delpage&amp;info=ok&amp;uid=" + uid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("mebook.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

            string strWhere = "";
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (uid != 0)
                strWhere += "MID=" + uid + "";
 

            // 开始读取列表
            IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex, pageSize, strWhere, out recordCount);
            if (listMebook.Count > 0)
            {
                foreach (BCW.Model.Mebook n in listMebook)
                {
                    //删除
                    new BCW.BLL.Mebook().Delete(n.ID);
                }
            }

            Utils.Success("删除本页留言", "删除本页留言成功，正在返回..", Utils.getUrl("mebook.aspx"), "1");
        }
    }
}
