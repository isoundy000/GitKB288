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
public partial class myshop : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewBuy();
                break;
            case "ok":
                OkBuy();
                break;
            case "del":
                DelBuy();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的订单记录";
        builder.Append(Out.Tab("<div class=\"title\">我的订单记录</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UserId=" + meid + "";

        // 开始读取列表
        IList<BCW.Model.Buylist> listBuylist = new BCW.BLL.Buylist().GetBuylists(pageIndex, pageSize, strWhere, out recordCount);
        if (listBuylist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Buylist n in listBuylist)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("myshop.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}.{3}</a>(" + BCW.User.AppCase.CaseBuyStats(n.AcStats) + ")<br />共{4}元.{5}", n.ID, pageIndex, (pageIndex - 1) * pageSize + k, n.Title, Convert.ToDouble(n.AcPrice), DT.FormatDate(n.AddTime, 2));

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void ViewBuy()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!new BCW.BLL.Buylist().Exists(id, meid))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Buylist model = new BCW.BLL.Buylist().GetBuylist(id, meid);
        Master.Title = "我的订单";
        builder.Append(Out.Tab("<div class=\"title\">我的订单</div>", ""));
        //读取商品实体
        BCW.Model.Goods modelGoods = new BCW.BLL.Goods().GetGoods(model.GoodsId);
        if (modelGoods == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + model.GoodsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">.." + model.Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("订单号:" + model.TingNo + "");
        builder.Append("<br />单价:");

        if (modelGoods.PostType == 0)
            builder.Append("￥" + Convert.ToDouble(model.Price) + "元");
        else if (modelGoods.PostType == 1)
            builder.Append(Convert.ToDouble(model.Price) + "" + ub.Get("SiteBz"));
        else
            builder.Append(Convert.ToDouble(model.Price) + "" + ub.Get("SiteBz2"));

        builder.Append("<br />购买数量" + model.Paycount + "");
        if (model.PostMoney == -1)
        {
            builder.Append("送货方式:当面交易");
        }
        else
        {
            builder.Append("<br />邮费:");

            if (model.PostMoney == 0)
                builder.Append("包邮");
            else
                builder.Append(model.PostMoney + "元");
        }
        builder.Append("<br />你的姓名:" + model.RealName + "");
        builder.Append("<br />你的电话:" + model.Mobile + "");
        builder.Append("<br />送货地址:" + model.Address + "");
        builder.Append("<br />备注:" + model.Notes + "");
        builder.Append("<br />总金额:");

        if (modelGoods.PostType == 0)
            builder.Append("￥" + Convert.ToDouble(model.AcPrice) + "元");
        else if (modelGoods.PostType == 1)
            builder.Append(Convert.ToDouble(model.AcPrice) + "" + ub.Get("SiteBz"));
        else
            builder.Append(Convert.ToDouble(model.AcPrice) + "" + ub.Get("SiteBz2"));

        builder.Append("<br />状态:" + BCW.User.AppCase.CaseBuyStats(model.AcStats) + "");
        //商家信息
        if (model.AcStats != 0)
        {
            if (!string.IsNullOrEmpty(model.AcEms))
                builder.Append("<br />运单号:" + model.AcEms + "");

            if (!string.IsNullOrEmpty(model.AcText))
                builder.Append("<br />管理员附言:" + model.AcText + "");

            if (!string.IsNullOrEmpty(model.LaNotes))
                builder.Append("<br />您的评价:" + model.LaNotes + "");
        }

        builder.Append("<br />时间:" + model.AddTime + "");
        builder.Append(Out.Tab("</div>", ""));
        if (model.AcStats == 2)
        {
            string strText = "输入评价:/,,,";
            string strName = "LaNotes,stats,id,act";
            string strType = "text,select,hidden,hidden";
            string strValu = "" + model.LaNotes + "'3'" + id + "'ok";
            string strEmpt = "true,3|交易成功,false,false";
            string strIdea = "";
            string strOthe = "确定,myshop.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        if (model.AcStats == 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("myshop.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此订单</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + modelGoods.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("myshop.aspx") + "\">订单记录</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void OkBuy()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string LaNotes = Utils.GetRequest("LaNotes", "post", 2, @"^[\s\S]{1,300}$", "评价限1-300字");
        int stats = int.Parse(Utils.GetRequest("stats", "post", 2, @"^[3-4]$", "选择状态错误"));

        //是否存在未确认/评价订单记录
        if (!new BCW.BLL.Buylist().Exists(id, meid, 2))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Buylist model = new BCW.Model.Buylist();
        model.ID = id;
        model.LaNotes = LaNotes;
        //model.AcStats = stats;
        model.AcStats = 3;
        new BCW.BLL.Buylist().UpdateStats(model);
        //更新评价数
        new BCW.BLL.Goods().UpdateEvcount(id, 1);

        Utils.Success("确定交易", "确定交易/评价成功..", Utils.getUrl("myshop.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelBuy()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除订单";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此订单记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("myshop.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myshop.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Buylist().Exists(id, meid, 0))
            {
                Utils.Error("订单已通过审核或订单不存在", "");
            }
            //得到实体
            BCW.Model.Buylist model = new BCW.BLL.Buylist().GetBuylistMe(id);
            //更新购买人数
            new BCW.BLL.Goods().UpdatePaycount(model.GoodsId, -1);
            //更新出售数量
            new BCW.BLL.Goods().UpdateSellCount(model.GoodsId, model.Paycount);
            //删除
            new BCW.BLL.Buylist().Delete(id);
            //读取商品实体
            BCW.Model.Goods modelGoods = new BCW.BLL.Goods().GetGoods(model.GoodsId);
            if (modelGoods != null)
            {
                if (modelGoods.PostType == 1)
                {
                    new BCW.BLL.User().UpdateiGold(model.UserId, model.UserName, Convert.ToInt64(model.AcPrice), "购买商品《" + modelGoods.Title + "》");
                    //发信给下订单的会员
                    new BCW.BLL.Guest().Add(model.UserId, model.UserName, "您已成功删除订单[url=/shopdetail.aspx?id=" + model.GoodsId + "]" + modelGoods.Title + "[/url]，返还您" + Convert.ToInt64(model.AcPrice) + "" + ub.Get("SiteBz") + "，欢迎再次购买！[br][url=/myshop.aspx]&gt;我的订单[/url]");
                }
                else if (modelGoods.PostType == 2)
                {
                    new BCW.BLL.User().UpdateiMoney(model.UserId, model.UserName, Convert.ToInt64(model.AcPrice), "购买商品《" + modelGoods.Title + "》");
                    //发信给下订单的会员
                    new BCW.BLL.Guest().Add(model.UserId, model.UserName, "您已成功删除订单[url=/shopdetail.aspx?id=" + model.GoodsId + "]" + modelGoods.Title + "[/url]，返还您" + Convert.ToInt64(model.AcPrice) + "" + ub.Get("SiteBz2") + "，欢迎再次购买！[br][url=/myshop.aspx]&gt;我的订单[/url]");
                }
            }
            Utils.Success("删除订单", "删除订单成功..", Utils.getUrl("myshop.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
    }
}