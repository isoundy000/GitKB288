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

public partial class Manage_spaceapp_sellnum : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "靓号管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "priceok":
                PriceOkPage();
                break;
            case "priceno":
                PriceNoPage();
                break;
            case "priceno2":
                PriceNo2Page();
                break;
            case "replyok":
                ReplyOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("靓号管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("查询中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=1") + "\">查询中</a>|");

        if (ptype == 2)
            builder.Append("已报价|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=2") + "\">已报价</a>|");

        if (ptype == 3)
            builder.Append("已成交|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=3") + "\">已成交</a>|");

        if (ptype == 4)
            builder.Append("已撤销");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=4") + "\">已撤销</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "uid","backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=0";

        if (ptype == 4)
            strWhere += " and State=9";//已撤销
        else
        {
            if (ptype > 0 && ptype < 3)
                strWhere += " and State=" + ptype + "";
            else if (ptype >= 3)
                strWhere += " and State>=3 and State<>9";

        }

        if (uid > 0)
            strWhere += " and usid=" + uid + "";

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
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");

                if (n.State == 1)
                {
                    builder.Append("<b>查询中</b>ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "");
                    builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=priceok&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[报价]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=priceno&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销]</a>");
                }
                else if (n.State == 2)
                {
                    builder.Append("<b>已报价</b>ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                    builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=priceno&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销]</a>");
                }
                else if (n.State == 9)
                {
                    builder.Append("<b>已撤销</b>ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                }
                else if (n.State == 3)
                {
                    builder.Append("<b>已兑换</b>ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                    builder.Append("<br />绑定手机号:" + n.Mobile + "");
                    builder.Append("<br />兑换时间" + DT.FormatDate(n.PayTime, 5) + "");
                    builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=replyok&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[回复]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=priceno2&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销]</a>");
               
                }
                else
                {
                    builder.Append("<b>已成交</b>ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                    builder.Append("<br />绑定手机号:" + n.Mobile + "");
                    builder.Append("<br />备注:" + n.Notes + "");
                    builder.Append("<br />成交时间" + DT.FormatDate(n.PayTime, 5) + "");

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
        string strText = "输入用户ID:/,";
        string strName = "uid,ptype";
        string strType = "num,hidden";
        string strValu = "'" + ptype + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜记录,sellnum.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">应用中心</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void PriceOkPage()
    {

        Master.Title = "靓号报价";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State >= 2)
        {
            Utils.Error("系统已报价", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            long Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "报价填写错误"));

            new BCW.BLL.SellNum().UpdateState2(id, Price);

            //发信息通知查询报价的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您查询的ID" + model.BuyUID + "报价为" + Price + "" + ub.Get("SiteBz") + "[url=/bbs/spaceapp/sellnum.aspx?act=uidlist]进入我的靓号记录[/url]");
            Utils.Success("靓号报价", "ID:" + model.BuyUID + "报价" + Price + "" + ub.Get("SiteBz") + "成功...", Utils.getUrl("sellnum.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">靓号报价</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("报价ID:" + model.BuyUID + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入价格:/,,,,";
            string strName = "Price,id,act,info,backurl";
            string strType = "num,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'priceok'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定报价,sellnum.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

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

    private void PriceNoPage()
    {

        Master.Title = "撤销靓号提交";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State > 2)
        {
            Utils.Error("已成交的记录请在成交记录中撤销", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,300}$", "撤销原因限300字内,可以留空");

            new BCW.BLL.SellNum().UpdateState9(id);
            if (Content != "")
                Content = "原因:" + Content + "。";


            Content = "您查询报价的ID" + model.BuyUID + "已被撤销，" + Content + "如需再继续请进入[url=/bbs/spaceapp/sellnum.aspx?act=uidbuy]查询报价[/url]，有疑问请内线[url=/bbs/uinfo.aspx?uid=10086]客服10086[/url]查询";

            //发信息通知查询报价的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, Content);
            Utils.Success("撤销靓号提交", "撤销成功...", Utils.getUrl("sellnum.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">靓号报价</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("报价ID:" + model.BuyUID + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "撤销原因:/,,,,";
            string strName = "Content,id,act,info,backurl";
            string strType = "textarea,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'priceno'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "撤销并内线,sellnum.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

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

    private void PriceNo2Page()
    {

        Master.Title = "撤销靓号提交";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types != 0 || model.State != 3)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,300}$", "内线内容限300字内,可以留空");

            new BCW.BLL.SellNum().UpdateState9(id);

            if (Content != "")
                Content = "原因:" + Content + "。";

            Content = "您兑换的ID" + model.BuyUID + "已被撤销并退回本金，" + Content + "如需再继续请进入[url=/bbs/spaceapp/sellnum.aspx?act=uidbuy]查询报价[/url]，有疑问请内线[url=/bbs/uinfo.aspx?uid=10086]客服10086[/url]查询";


            //退币
            new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, model.Price, "撤销兑换ID" + model.BuyUID + "退回");

            //发信息通知查询报价的会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, Content);
            Utils.Success("撤销靓号提交", "撤销成功...", Utils.getUrl("sellnum.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">靓号报价</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("报价ID:" + model.BuyUID + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "撤销原因:/,,,,";
            string strName = "Content,id,act,info,backurl";
            string strType = "textarea,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'priceno2'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "撤销并内线,sellnum.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

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
    private void ReplyOkPage()
    {

        Master.Title = "靓号回复";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types != 0 || model.State != 3)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,200}$", "注册内容限1-200字");

            new BCW.BLL.SellNum().UpdateState4(id, Notes);

            //发信息通知兑换的会员
            //new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您的ID" + model.BuyUID + "（" + model.Price + "" + ub.Get("SiteBz") + "）已成功兑换[url=/bbs/spaceapp/sellnum.aspx?act=uidlist]进入我的靓号记录[/url]");

            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "" + Notes + "[url=/bbs/spaceapp/sellnum.aspx?act=uidlist]进入我的靓号记录[/url]");
            
            
            Utils.Success("靓号回复", "ID:" + model.BuyUID + "（" + model.Price + "" + ub.Get("SiteBz") + "）回复注册内容成功...", Utils.getUrl("sellnum.aspx"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">靓号回复</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("靓号ID:" + model.BuyUID + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入内线内容(同时作为备注):/,,,,";
            string strName = "Notes,id,act,info,backurl";
            string strType = "textarea,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'replyok'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定并内线,sellnum.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

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
