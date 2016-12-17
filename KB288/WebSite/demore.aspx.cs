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

public partial class demore : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/front.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "down":
                DownPage();
                break;
            case "downtext":
                DownTextPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "适用机型";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = new BCW.BLL.Detail().GetTitle(id);
        if (Title == "")
        {
            Utils.Error("不存在的记录", "");
        }
        string Model = new BCW.BLL.Detail().GetPhoneModel(id);
        builder.Append(Out.Tab("<div class=\"title\">适用机型</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(Model));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">.." + Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void DownTextPage()
    {
        Master.Title = "功能箱";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = new BCW.BLL.Detail().GetTitle(id);
        if (Title == "")
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("选择下载选项：");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?act=downtxt&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下载txt</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?act=downjar&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">jar</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?act=downumd&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">umd</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?act=downpdf&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">pdf</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?act=downchm&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">chm</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?act=downword&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">word</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">.." + Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DownPage()
    {
        if (ub.GetSub("FtFileIsUser", xmlPath) == "2")
        {
            Utils.Error("下载已关闭", "");
        }
        int meid = 0;
        if (ub.GetSub("FtFileIsUser", xmlPath) == "1")
        {
            meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.File m = new BCW.BLL.File().GetFile(id);
        if (m == null)
        {
            Utils.Error("不存在的文件", "");
        }

        //下载收费
        BCW.Model.Detail model = new BCW.BLL.Detail().GetDetail(m.NodeId);
        if (model.Cent > 0)
        {
            if (meid == 0)
            {
                meid = new BCW.User.Users().GetUsId();
                if (meid == 0)
                    Utils.Login();
            }
            string Bz = string.Empty;
            long megold = 0;
            if (model.BzType == 0)
            {
                megold = new BCW.BLL.User().GetGold(meid);
                Bz = ub.Get("SiteBz");
            }
            else
            {
                megold = new BCW.BLL.User().GetMoney(meid);
                Bz = ub.Get("SiteBz2");
            }
            string info = Utils.GetRequest("info", "get", 1, "", "");
            string payIDs = "|" + model.PayId + "|";
            if (info != "ok" && payIDs.IndexOf("|" + meid + "|") == -1)
            {
                new Out().head(Utils.ForWordType("温馨提示"));
                Response.Write(Out.Tab("<div class=\"text\">", ""));
                Response.Write("下载收费" + model.Cent + "" + Bz + "，确定要下载吗？扣费一次，永久下载");
                Response.Write(Out.Tab("</div>", "<br />"));
                Response.Write(Out.Tab("<div>", ""));
                Response.Write("您自带" + megold + "" + Bz + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay") + "\">[充值]</a><br />");
                Response.Write("<a href=\"" + Utils.getUrl("demore.aspx?act=down&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">马上进入下载</a><br />");
                Response.Write("<a href=\"" + Utils.getPage("detail.aspx?id=" + model.NodeId + "") + "\">返回上级</a>");
                Response.Write(Out.Tab("</div>", ""));
                Response.Write(new Out().foot());
                Response.End();
            }
            if (payIDs.IndexOf("|" + meid + "|") == -1)
            {
                if (megold < model.Cent)
                {
                    Utils.Error("您的" + Bz + "不足", "");
                }
                //扣币
                if (model.BzType == 0)
                    new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(model.Cent), "下载收费文件");
                else
                    new BCW.BLL.User().UpdateiMoney(meid, -Convert.ToInt64(model.Cent), "下载收费文件");

                //更新
                payIDs = model.PayId + "|" + meid;
                new BCW.BLL.Detail().UpdatePayId(model.NodeId, payIDs);
            }
        }
        //更新下载次数
        new BCW.BLL.File().UpdateDownNum(id, 1);
        BCW.User.Down.ShowMsg(m.Files);
    }
}
