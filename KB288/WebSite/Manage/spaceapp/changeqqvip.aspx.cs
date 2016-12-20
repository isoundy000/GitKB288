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

public partial class Manage_spaceapp_changeqqvip : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "QQ服务开通管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "config":
                ConfigPage();
                break;
            case "qqvipok":
                QqVipOkPage();
                break;
            case "qqvipno":
                QqVipNoPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("QQ服务开通管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("兑换中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=1") + "\">兑换中</a>|");

        if (ptype == 2)
            builder.Append("已完成|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=2") + "\">已完成</a>|");

        if (ptype == 3)
            builder.Append("已撤销");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=3") + "\">已撤销</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=3";

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
                int Types = n.Tags;

                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 1)
                {
                    builder.Append("开通QQ:" + n.Mobile + "|" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");
                    builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvipok&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[回复]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvipno&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销]</a>");
                }
                else if(n.State==9)
                {
                    builder.Append("<b>已撤销</b>开通QQ:" + n.Mobile + "|" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");

                }
                else
                {
                    builder.Append("<b>已成功</b>开通QQ:" + n.Mobile + "|" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=config") + "\">费用配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


    }

    private void ConfigPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/qqvip.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            for (int i = 1; i <= 19; i++)
            {
                xml.dss["QQVIPM" + i + ""] = Request["M" + i + ""];
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("费用设置", "费用设置成功，正在返回..", Utils.getUrl("changeqqvip.aspx?act=config"), "1");
        }
        else
        {
            Master.Title = "费用设置";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("开通每月费用设置");
            builder.Append(Out.Tab("</div>", ""));

            string sText = "";
            string sName = "";
            string sType = "";
            string sValu = "";
            string sEmpt = "";
            string sIdea = "";

            for (int i = 1; i <= 19; i++)
            {
                sText += "" + OutType(i) + "每月:,";
                sName += "M" + i + ",";
                sType += "num,";
                sValu += "" + xml.dss["QQVIPM" + i + ""] + "'";
                sEmpt += "true,";
                sIdea += "" + ub.Get("SiteBz") + "'";
            }
            string strText = "" + sText + ",,";
            string strName = "" + sName + "act,info";
            string strType = "" + sType + "hidden,hidden";
            string strValu = "" + sValu + "config'ok";
            string strEmpt = "" + sEmpt + "false,false";
            string strIdea = "" + sIdea + "''|/";
            string strOthe = "确定修改|reset,changeqqvip.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx") + "\">返回上一级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }


    private void QqVipOkPage()
    {

        Master.Title = "确认QQ服务";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types != 3)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int Types = model.Tags;
        if (info == "ok")
        {

            new BCW.BLL.SellNum().UpdateState2(id, model.Price);

            //发信息通知开通的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您开通QQ:" + model.Mobile + "|" + OutType(Types) + "(" + model.BuyUID + "个月),花费" + model.Price + "" + ub.Get("SiteBz") + ",已完成开通[url=/bbs/spaceapp/changeqqvip.aspx?act=mylist]查看并给个好评吧[/url]");
            Utils.Success("确认QQ服务", "确认QQ服务成功...", Utils.getUrl("changeqqvip.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">确认QQ服务</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("QQ号:" + model.Mobile + "");
            builder.Append("<br />类型:" + OutType(Types) + "(" + model.BuyUID + "个月)");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "id,act,info,backurl";
            string strValu = "" + id + "'qqvipok'ok'" + Utils.getPage(0) + "";
            string strOthe = "已开通并内线,changeqqvip.aspx,post,0,red";

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


    private void QqVipNoPage()
    {

        Master.Title = "撤销QQ服务";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types != 3)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int Types = model.Tags;

        if (info == "ok")
        {

            new BCW.BLL.SellNum().UpdateState9(id);
            //退币
            new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, model.Price, "撤销" + OutType(Types) + "" + model.BuyUID + "个月");
            //发信息通知兑换的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您开通QQ:" + model.Mobile + "|" + OutType(Types) + "(" + model.BuyUID + "个月),花费" + model.Price + "" + ub.Get("SiteBz") + "不成功已被撤销，退回本金，如需再继续请进入[url=/bbs/spaceapp/changeqqvip.aspx]开通处[/url]，有疑问请内线客服10086查询");
            Utils.Success("撤销QQ服务", "撤销QQ服务成功...", Utils.getUrl("changeqqvip.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">撤销QQ服务</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("QQ号:" + model.Mobile + "");
            builder.Append("<br />类型:" + OutType(Types) + "(" + model.BuyUID + "个月)");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "id,act,info,backurl";
            string strValu = "" + id + "'qqvipno'ok'" + Utils.getPage(0) + "";
            string strOthe = "撤销并内线,changeqqvip.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", " "));
            builder.Append("<a href=\"" + Utils.getPage("changeqqvip.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private string OutType(int Types)
    {
        string p_str = string.Empty;
        if (Types == 1)
            p_str = "QQ会员";
        else if (Types == 2)
            p_str = "黄钻贵族";
        else if (Types == 3)
            p_str = "蓝钻贵族";
        else if (Types == 4)
            p_str = "超级QQ";
        else if (Types == 5)
            p_str = "红钻贵族";
        else if (Types == 6)
            p_str = "绿钻贵族";
        else if (Types == 7)
            p_str = "QQ炫舞紫钻";
        else if (Types == 8)
            p_str = "AVA精英";
        else if (Types == 9)
            p_str = "CF会员";
        else if (Types == 10)
            p_str = "西游VIP";
        else if (Types == 11)
            p_str = "洛克王国VIP";
        else if (Types == 12)
            p_str = "DNF黑钻";
        else if (Types == 13)
            p_str = "QQ音速";
        else if (Types == 14)
            p_str = "QQ飞车紫钻";
        else if (Types == 15)
            p_str = "读书VIP";
        else if (Types == 16)
            p_str = "QQ堂紫钻";
        else if (Types == 17)
            p_str = "寻仙VIP";
        else if (Types == 18)
            p_str = "QQ宠物粉钻";
        else if (Types == 19)
            p_str = "财付通SVIP";

        return p_str;
    }
}

