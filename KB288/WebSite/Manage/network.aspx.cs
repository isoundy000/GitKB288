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

public partial class Manage_network : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/network.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "广播管理";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "clear":
                ClearPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("广播管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("正在广播|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?uid=" + uid + "&amp;ptype=1") + "\">正在广播</a>|");

        if (ptype == 2)
            builder.Append("过期广播");
        else
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?uid=" + uid + "&amp;ptype=2") + "\">过期广播</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype","uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types<=1 and ";
        //查询条件
        if (uid != 0)
        {
            strWhere += "UsID=" + uid + "and ";
        }
        if (ptype == 1)
            strWhere += "OverTime>='" + DateTime.Now + "'";
        else
            strWhere += "OverTime<'" + DateTime.Now + "'";

        // 开始读取列表
        IList<BCW.Model.Network> listNetwork = new BCW.BLL.Network().GetNetworks(pageIndex, pageSize, strWhere, out recordCount);
        if (listNetwork.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Network n in listNetwork)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("<a href=\"" + Utils.getUrl("network.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>{1}.{2}", n.ID, (pageIndex - 1) * pageSize + k, Out.SysUBB(n.Title));
                if (ptype == 1)
                    builder.Append("(" + DT.DateDiff2(DateTime.Now, n.OverTime) + "后过期)");
                else
                    builder.Append("(" + DT.DateDiff2(n.OverTime, DateTime.Now) + "前过期)");

                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>(" + DT.FormatDate(n.AddTime, 6) + ")");

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
        string strText = "输入用户ID:/";
        string strName = "uid";
        string strType = "num";
        string strValu = "'";
        string strEmpt = "true";
        string strIdea = "/";
        string strOthe = "搜广播,network.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=add") + "\">发布广播</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=clear") + "\">清空广播</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除广播";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此广播记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Network().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Network().Delete(id);
            Utils.Success("删除广播", "删除广播成功..", Utils.getPage("network.aspx"), "1");
        }
    }

    private void ClearPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            Master.Title = "清空广播";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空广播记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?info=ok&amp;act=clear") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            new BCW.BLL.Network().Delete();
            Utils.Success("清空留言", "清空广播成功..", Utils.getUrl("network.aspx"), "1");
        }
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Network().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        //读取实体
        BCW.Model.Network model = new BCW.BLL.Network().GetNetwork(id);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改广播");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "广播内容(500字内)/,显示时长:限0-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟/,允许其它用户延时:/,是否支持UBB:/,,,";
        string strName = "Title,Times,Types,IsUbb,id,act,backurl";
        string strType = "text,num,select,select,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'0'" + model.Types + "'"+model.IsUbb+"'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,0|不允许|1|允许,0|否|1|是,false,false,false";
        string strIdea = "/显示时长填0则不延长/";
        string strOthe = "修改|reset,network.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除广播</a><br />");
        if (Utils.getPage(1) != "")
        {
            builder.Append(" <a href=\"" + Utils.getPage(1) + "\">返回上一级</a><br />");
        }
        else
            builder.Append(" <a href=\"" + Utils.getUrl("network.aspx") + "\">返回上一级</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));

        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,400}$", "内容限400字内");
        int Times = int.Parse(Utils.GetRequest("Times", "post", 2, @"^[0-9]\d*$", "显示时长填写错误"));
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-9]\d*$", "允许其它用户延时选择出错"));
        int IsUbb = int.Parse(Utils.GetRequest("IsUbb", "post", 2, @"^[0-1]$", "是否支持UBB选择错误"));

        BCW.Model.Network model = new BCW.Model.Network();
        model.ID = id;
        model.Title = Title;
        model.Types = Types;
        model.OverTime = DateTime.Now.AddMinutes(Times);
        model.IsUbb = IsUbb;
        new BCW.BLL.Network().UpdateOther(model);
        Utils.Success("修改广播", "修改广播成功，正在返回..", Utils.getPage("network.aspx"), "1");
    }



    private void AddPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布广播");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "广播内容(500字内)/,显示时长:限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟/,发布的会员ID:/,是否支持UBB:/,";
        string strName = "Title,Times,uid,IsUbb,act";
        string strType = "text,num,num,select,hidden";
        string strValu = "'''1'addsave";
        string strEmpt = "false,false,false,0|否|1|是,false";
        string strIdea = "/";
        string strOthe = "发布|reset,network.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("network.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string info = Utils.GetRequest("info", "post", 1, "", "");

        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,400}$", "内容限400字内");
        int Times = int.Parse(Utils.GetRequest("Times", "post", 2, @"^[0-9]\d*$", "显示时长填写错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "post", 2, @"^[0-9]\d*$", "会员ID填写错误"));
        int IsUbb = int.Parse(Utils.GetRequest("IsUbb", "post", 2, @"^[0-1]$", "是否支持UBB选择错误"));

        string mename = new BCW.BLL.User().GetUsName(uid);
        if (!new BCW.BLL.User().Exists(uid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (info != "ok")
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定发布");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("发布会员：" + mename + "(" + uid + ")");
            builder.Append("<br />内容：" + Title + "");
            builder.Append("<br />时长：" + Times + "分钟");
            if (IsUbb == 1)
                builder.Append("<br />UBB语法：支持");
            else
                builder.Append("<br />BB语法：不支持");

            builder.Append(Out.Tab("</div>", "<br />"));

            string strName = "Title,Times,uid,IsUbb,info,act";
            string strValu = "" + Title + "'" + Times + "'" + uid + "'" + IsUbb + "'ok'addsave";
            string strOthe = "确定发布,network.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("network.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            BCW.Model.Network model = new BCW.Model.Network();
            model.Title = Title;
            model.Types = 0;
            model.UsID = uid;
            model.UsName = mename;
            model.OverTime = DateTime.Now.AddMinutes(Times);
            model.AddTime = DateTime.Now;
            model.OnIDs = "";
            model.IsUbb = IsUbb;
            new BCW.BLL.Network().Add(model);
            Utils.Success("发布广播", "发布成功，正在返回..", Utils.getUrl("network.aspx"), "1");
        }
    }
}