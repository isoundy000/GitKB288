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

public partial class Manage_spaceapp_changesim : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "兑换话费管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "simok":
                SimOkPage();
                break;
            case "simno":
                SimNoPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("兑换话费管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changesim.aspx?act=list&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("兑换中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changesim.aspx?act=list&amp;ptype=1") + "\">兑换中</a>|");

        if (ptype == 2)
            builder.Append("已完成|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changesim.aspx?act=list&amp;ptype=2") + "\">已完成</a>|");

        if (ptype == 3)
            builder.Append("已撤销");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changesim.aspx?act=list&amp;ptype=3") + "\">已撤销</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=1";

        if (ptype > 0)
        {
            if (ptype == 3)
                strWhere += " and State=9";//已撤销
            else
            {
                if (ptype > 1)
                    strWhere += " and State>=2 and State<>9";
                else
                    strWhere += " and State=1";
            }
        }

        // 开始读取专题
        IList<BCW.Model.SellNum> listSellNum = new BCW.BLL.SellNum().GetSellNums(pageIndex, pageSize, strWhere, out recordCount);
        if (listSellNum.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.SellNum n in listSellNum)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 1)
                {
                    builder.Append("兑换" + n.BuyUID + "元充值卡,手机号("+n.Mobile+"),花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                    builder.Append("<a href=\"" + Utils.getUrl("changesim.aspx?act=simok&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[回复]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("changesim.aspx?act=simno&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销]</a>");
                }
                else if (n.State == 9)
                {
                    builder.Append("<b>已撤销</b>兑换" + n.BuyUID + "元充值卡,手机号("+n.Mobile+"),花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                }
                else
                {
                    builder.Append("<b>已成功</b>兑换" + n.BuyUID + "元充值卡,手机号("+n.Mobile+"),花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                    if (n.Notes != null)
                        builder.Append("<br />TA的评价:" + n.Notes + "");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


    }

    private void SimOkPage()
    {

        Master.Title = "确认充值";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types!=1)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            new BCW.BLL.SellNum().UpdateState2(id, model.Price);

            //发信息通知兑换的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您兑换" + model.BuyUID + "元充值卡（花费" + model.Price + "" + ub.Get("SiteBz") + "）已成功充值[url=/bbs/spaceapp/changesim.aspx?act=mylist]查看并给个好评吧[/url]");
            Utils.Success("确认充值", "确认充值成功...", Utils.getUrl("changesim.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">确认充值</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("手机号:" + model.Mobile + "");
            builder.Append("<br />类型:" + model.BuyUID + "元充值");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "id,act,info,backurl";
            string strValu = "" + id + "'simok'ok'" + Utils.getPage(0) + "";
            string strOthe = "已充值并内线,changesim.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", " "));
            builder.Append("<a href=\"" + Utils.getPage("sellnum.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    private void SimNoPage()
    {

        Master.Title = "撤销充值";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types != 1)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            new BCW.BLL.SellNum().UpdateState9(id);
            //退币
            new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, model.Price, "撤销兑换" + model.BuyUID + "元卡退回");
            //发信息通知兑换的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您兑换" + model.BuyUID + "元充值卡（花费" + model.Price + "" + ub.Get("SiteBz") + "）不成功已被撤销，退回本金，如需再继续请进入[url=/bbs/spaceapp/changesim.aspx?act=sim]兑换处[/url]，有疑问请内线客服10086查询");
            Utils.Success("确认充值", "确认充值成功...", Utils.getUrl("changesim.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">撤销充值</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("手机号:" + model.Mobile + "");
            builder.Append("<br />类型:" + model.BuyUID + "元充值");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "id,act,info,backurl";
            string strValu = "" + id + "'simno'ok'" + Utils.getPage(0) + "";
            string strOthe = "撤销并内线,changesim.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", " "));
            builder.Append("<a href=\"" + Utils.getPage("sellnum.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
