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
public partial class Manage_shopbuy : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/buylist.xml";
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
            case "edit":
                EditBuy();
                break;
            case "save":
                SaveBuy();
                break;
            case "del":
                DelBuy();
                break;
            case "appbank":
                AppbankPage();
                break;
            case "editappbank":
                EditAppbankPage();
                break;
            case "editokappbank":
                EditOkAppbankPage();
                break;
            case "delappbank":
                DelAppbankPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "商品订单管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("商品订单管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (id != 0)
        {
            if (!new BCW.BLL.Goods().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            string Title = new BCW.BLL.Goods().GetTitle(id);
            builder.Append(Out.Tab("<div class=\"text\">", ""));

            builder.Append("<a href=\"" + Utils.getUrl("../shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">.." + Title + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("待审|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?id=" + id + "&amp;uid=" + uid + "&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">待审</a>|");

        if (ptype == 1)
            builder.Append("待发|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?id=" + id + "&amp;uid=" + uid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">待发</a>|");

        if (ptype == 2)
            builder.Append("已发|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?id=" + id + "&amp;uid=" + uid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已发</a>|");

        if (ptype == 3)
            builder.Append("成功|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?id=" + id + "&amp;uid=" + uid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">成功</a>|");

        if (ptype == 4)
            builder.Append("失败");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?id=" + id + "&amp;uid=" + uid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">失败</a>");

        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id" , "uid" , "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid != 0)
        {
            strWhere += "userid=" + uid + "and ";
        }
        strWhere += " AcStats=" + ptype + "";

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


                builder.AppendFormat("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(true) + "") + "\">{2}.{3}</a>(" + BCW.User.AppCase.CaseBuyStats(n.AcStats) + ")<br />共{4}元.{5}", n.ID, pageIndex, (pageIndex - 1) * pageSize + k, n.Title, Convert.ToDouble(n.AcPrice), DT.FormatDate(n.AddTime, 2));

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

        if (id == 0)
        {
            string strText = "输入用户ID:/";
            string strName = "uid";
            string strType = "num";
            string strValu = "'";
            string strEmpt = "true";
            string strIdea = "/";
            string strOthe = "搜订单,shopbuy.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.getPage(1) != "")
        {
            builder.Append(" <a href=\"" + Utils.getPage(1) + "\">返回上一级</a><br />");
        }
        
            
        //商品推广功能
        if (Utils.GetDomain().Contains("127.0.0.6") || Utils.GetDomain().Contains("xgbxj.net"))
        {
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=appbank") + "\">推广拥金结算</a><br />");
        }
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ViewBuy()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Buylist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Buylist model = new BCW.BLL.Buylist().GetBuylist(id);
        Master.Title = "查看订单";

        builder.Append(Out.Tab("<div class=\"title\">查看订单</div>", ""));
        //读取商品实体
        BCW.Model.Goods modelGoods = new BCW.BLL.Goods().GetGoods(model.GoodsId);

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/shopdetail.aspx?id=" + model.GoodsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">.." + model.Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("购买用户:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UserId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UserName + "(" + model.UserId + ")</a>");
        builder.Append("<br />订单号:" + model.TingNo + "");
        builder.Append("<br />单价:");

        if (modelGoods.PostType == 0)
            builder.Append("￥" + Convert.ToDouble(model.Price) + "元");
        else if (modelGoods.PostType == 1)
            builder.Append(Convert.ToDouble(model.Price) + "" + ub.Get("SiteBz"));
        else
            builder.Append(Convert.ToDouble(model.Price) + "" + ub.Get("SiteBz2"));

        builder.Append("<br />购买数量:" + model.Paycount + "");
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
        builder.Append("<br />姓名:" + model.RealName + "");
        builder.Append("<br />电话:" + model.Mobile + "");
        builder.Append("<br />送货地址:" + model.Address + "");
        builder.Append("<br />备注:" + model.Notes + "");
        builder.Append("<br />实收金额:");
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
                builder.Append("<br />买家评价:" + model.LaNotes + "");
        }
        builder.Append("<br />时间:" + model.AddTime + "");
        builder.Append(Out.Tab("</div>", ""));
        if (model.AcStats != 3 && model.AcStats != 4)
        {
            string strText = "运单号(可空):/,附言(可空):/,状态:,,,";
            string strName = "AcEms,AcText,stats,id,act,backurl";
            string strType = "text,text,select,hidden,hidden,hidden";
            string strValu = "" + model.AcEms + "'" + model.AcText + "'" + model.AcStats + "'" + id + "'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,0|" + BCW.User.AppCase.CaseBuyStats(0) + "|1|" + BCW.User.AppCase.CaseBuyStats(1) + "|2|" + BCW.User.AppCase.CaseBuyStats(2) + "|3|" + BCW.User.AppCase.CaseBuyStats(3) + "|4|" + BCW.User.AppCase.CaseBuyStats(4) + ",false,false,false";
            string strIdea = "/";
            string strOthe = "确定,shopbuy.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">修改订单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除订单</a><br />");
        if (Utils.getPage(1) != "")
        {
            builder.Append("<a href=\"" + Utils.getPage(1) + "\">返回上一级</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx") + "\">返回订单管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void OkBuy()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));
        string AcEms = Utils.GetRequest("AcEms", "post", 3, @"^[A-Za-z0-9]+$", "运单号填写错误");
        string AcText = Utils.GetRequest("AcText", "post", 3, @"^[\s\S]{1,300}$", "附言最多300字");
        int stats = int.Parse(Utils.GetRequest("stats", "post", 2, @"^[0-4]$", "选择状态错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");

        if (info != "ok" && (stats == 3 || stats == 4))
        {
            Master.Title = "确定订单";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (stats == 3)
                builder.Append("确定把订单记录确认为交易成功吗,确认后不可修改");
            else
                builder.Append("确定把订单记录确认为交易失败吗,确认后不可修改");

            builder.Append(Out.Tab("</div>", "<br />"));


            string strName = "AcEms,AcText,stats,id,act,backurl,info";
            string strValu = "" + AcEms + "'" + AcText + "'" + stats + "'" + id + "'ok'" + Utils.getPage(2) + "'ok";
            string strOthe = "确认交易,shopbuy.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));


            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(" <a href=\"" + Utils.getUrl("shopbuy.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">等待确认</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Buylist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.Model.Buylist modelBuy = new BCW.BLL.Buylist().GetBuylist(id);
            if (modelBuy.AcStats == 3 || modelBuy.AcStats == 4)
            {
                Utils.Error("此订单已经确认，无法进行更改", "");
            }
            BCW.Model.Buylist model = new BCW.Model.Buylist();
            model.ID = id;
            model.AcText = AcText;
            model.AcEms = AcEms;
            model.AcStats = stats;
            new BCW.BLL.Buylist().UpdateMStats(model);
            if (stats == 4)
            {
                //如果失败，更新出售数量
                new BCW.BLL.Goods().UpdateSellCount(modelBuy.GoodsId, -modelBuy.Paycount);
            }
            if (stats == 3)
            {
                //如果交易成功并有推荐ID，则执行
                long ReTar = Utils.ParseInt64(ub.GetSub("BuylistReTar", xmlPath));
                if (ReTar > 0)
                {
                    int InviteNum = new BCW.BLL.User().GetInviteNum(modelBuy.UserId);
                    if (InviteNum > 0)
                    {
                        //计算分成
                        long gold = Convert.ToInt64(modelBuy.Paycount * modelBuy.Price);
                        long ReFcGold = Convert.ToInt64(gold * ReTar * 0.01);
                        //更新InviteNum分成金额
                        new BCW.BLL.User().UpdateiFcGold(InviteNum, ReFcGold);

                        BCW.Model.Shoptg tg = new BCW.Model.Shoptg();
                        tg.ZrID = InviteNum;
                        tg.UsID = modelBuy.UserId;
                        tg.UsName = modelBuy.UserName;
                        tg.Notes = "购买" + modelBuy.Title + "(" + modelBuy.Paycount + "件)，分成收入" + ReFcGold + "元";
                        tg.DetailId = modelBuy.GoodsId;
                        tg.AddTime = DateTime.Now;
                        new BCW.BLL.Shoptg().Add(tg);
                        //发信给推荐ID，提醒分成收入
                        new BCW.BLL.Guest().Add(InviteNum, new BCW.BLL.User().GetUsName(InviteNum), "恭喜！您推荐的会员[url=/bbs/uinfo.aspx?uid=" + modelBuy.UserId + "]" + modelBuy.UserName + "[/url]成功购买了商品[url=/shopdetail.aspx?id=" + modelBuy.GoodsId + "]《" + modelBuy.Title + "》[/url]，你的分成收入增加" + ReFcGold + "元");

                    }
                }
            }
            //发信给下订单的会员
            new BCW.BLL.Guest().Add(modelBuy.UserId, modelBuy.UserName, "您的订单[url=/myshop.aspx?act=view&amp;id=" + id + "]" + modelBuy.Title + "[/url]已变更为" + BCW.User.AppCase.CaseBuyStats(stats) + "");

            Utils.Success("确定交易", "确定成功，订单变更为" + BCW.User.AppCase.CaseBuyStats(stats) + "，" + modelBuy.UserName + "将收到订单结果的内线..", Utils.getUrl("shopbuy.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void EditBuy()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Buylist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Buylist model = new BCW.BLL.Buylist().GetBuylist(id);
        Master.Title = "修改订单";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改用户订单");
        builder.Append(Out.Tab("</div>", ""));
        //是否已评价标识
        int Ispn = 0;
        if (string.IsNullOrEmpty(model.LaNotes))
            Ispn = 1;

        string strText = "姓名:/,电话:/,送货地址:/,用户备注:/,用户评价:/,实收金额(元):/,,,,";
        string strName = "RealName,Mobile,Address,Notes,LaNotes,AcPrice,id,backurl,act,Ispn";
        string strType = "text,text,text,text,text,num,hidden,hidden,hidden,hidden";
        string strValu = "" + model.RealName + "'" + model.Mobile + "'" + model.Address + "'" + model.Notes + "'" + model.LaNotes + "'" + Convert.ToInt32(model.AcPrice) + "'" + id + "'" + Utils.getPage(0) + "'save'" + Ispn + "";
        string strEmpt = "false,false,false,true,true,false,false";
        string strIdea = "/";
        string strOthe = "修改|reset,shopbuy.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        if (Utils.getPage(1) != "")
        {
            builder.Append(" <a href=\"" + Utils.getPage(1) + "\">返回订单管理</a><br />");
        }
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SaveBuy()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));
        string RealName = Utils.GetRequest("RealName", "post", 2, @"^[\u4e00-\u9fa5]{2,8}$", "姓名填写错误");
        string Mobile = Utils.GetRequest("Mobile", "post", 2, @"^(?:13|15|18)\d{9}$|^(\d{3}-|\d{4}-)?(\d{8}|\d{7})?(-\d+)?$", "联系电话不正确,必填固话或者手机号");
        string Address = Utils.GetRequest("Address", "post", 2, @"^[\s\S]{8,}$", "收货地址请尽量详细到门牌号");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[\s\S]{1,200}$", "备注限200字,可以留空");
        string LaNotes = Utils.GetRequest("LaNotes", "post", 3, @"^[\s\S]{1,200}$", "评价限200字,可以留空");
        int AcPrice = int.Parse(Utils.GetRequest("AcPrice", "post", 2, @"^[0-9]\d*$", "金额填写出错"));
        int Ispn = int.Parse(Utils.GetRequest("Ispn", "post", 1, @"^[0-1]\d*$", "0"));
        if (!new BCW.BLL.Buylist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Buylist model = new BCW.Model.Buylist();
        model.ID = id;
        model.RealName = RealName;
        model.Mobile = Mobile;
        model.Address = Address;
        model.Notes = Notes;
        model.LaNotes = LaNotes;
        model.AcPrice = AcPrice;
        new BCW.BLL.Buylist().UpdateBuy(model);

        //更新评价数
        if (Ispn == 1)
            new BCW.BLL.Goods().UpdateEvcount(id, 1);

        Utils.Success("修改成功", "修改订单成功..", Utils.getUrl("shopbuy.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelBuy()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        //得到实体
        BCW.Model.Buylist model = new BCW.BLL.Buylist().GetBuylistMe(id);
        if (model == null)
        {
            Utils.Error("订单不存在", "");
        }
        //读取商品实体
        BCW.Model.Goods modelGoods = new BCW.BLL.Goods().GetGoods(model.GoodsId);
        if (modelGoods == null)
        {
            Utils.Error("商品不存在", "");
        }
        if (info == "")
        {
            Master.Title = "删除订单";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此订单记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (modelGoods.PostType == 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?info=ok1&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除(退还币)</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?info=ok2&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除(不退币)</a><br />");            
            }
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //更新购买人数
            new BCW.BLL.Goods().UpdatePaycount(model.GoodsId, -1);
            //更新出售数量
            new BCW.BLL.Goods().UpdateSellCount(model.GoodsId, model.Paycount);
            //删除
            new BCW.BLL.Buylist().Delete(id);
            if (info != "ok")
            {
                if (modelGoods.PostType == 1)
                {
                    new BCW.BLL.User().UpdateiGold(model.UserId, model.UserName, Convert.ToInt64(model.AcPrice), "购买商品《" + modelGoods.Title + "》");
                    //发信给下订单的会员
                    new BCW.BLL.Guest().Add(model.UserId, model.UserName, "管理员删除您的订单[url=/shopdetail.aspx?id=" + model.GoodsId + "]" + modelGoods.Title + "[/url]，返还您" + Convert.ToInt64(model.AcPrice) + "" + ub.Get("SiteBz") + "，欢迎再次购买！[br][url=/myshop.aspx]&gt;我的订单[/url]");
                }
                else if (modelGoods.PostType == 2)
                {
                    new BCW.BLL.User().UpdateiMoney(model.UserId, model.UserName, Convert.ToInt64(model.AcPrice), "购买商品《" + modelGoods.Title + "》");
                    //发信给下订单的会员
                    new BCW.BLL.Guest().Add(model.UserId, model.UserName, "管理员删除您的订单[url=/shopdetail.aspx?id=" + model.GoodsId + "]" + modelGoods.Title + "[/url]，返还您" + Convert.ToInt64(model.AcPrice) + "" + ub.Get("SiteBz2") + "，欢迎再次购买！[br][url=/myshop.aspx]&gt;我的订单[/url]");
                }
            }
            Utils.Success("删除订单", "删除订单成功..", Utils.getUrl("shopbuy.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    /// <summary>
    /// 商品推广拥金结算
    /// </summary>
    private void AppbankPage()
    {
        Master.Title = "推广拥金结算记录";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("结算记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("未结算|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=appbank&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">未结算</a>|");

        if (ptype == 1)
            builder.Append("已结算|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=appbank&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">已结算</a>|");

        if (ptype == 2)
            builder.Append("已失败");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=appbank&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已失败</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = "";
        string[] pageValUrl = { "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += "Types=0 and State=" + ptype + "";
      
        // 开始读取列表
        IList<BCW.Model.Appbank> listAppbank = new BCW.BLL.Appbank().GetAppbanks(pageIndex, pageSize, strWhere, out recordCount);
        if (listAppbank.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Appbank n in listAppbank)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                string sText = string.Empty;
                if (n.State == 0)
                    sText = "申请中";
                else if (n.State == 1)
                    sText = "已汇款";
                else if (n.State == 1)
                    sText = "已失败";

                builder.Append("<b>[" + sText + "]</b><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>申请" + n.AddGold + "元[" + DT.FormatDate(n.AddTime, 1) + "]");
                if (!string.IsNullOrEmpty(n.Notes))
                    builder.Append("<br />备注:" + n.Notes + "");

                builder.Append("<br />银行卡号:" + n.CardNum + "");
                builder.Append("<br />银行姓名:" + n.CardName + "");
                builder.Append("<br />开户银行:" + n.CardAddress + "");
                builder.Append("<br />手机号:" + n.Mobile + "");

                if (n.State > 0)
                {
                    builder.Append("<br />受理时间:" + n.ReTime + "");
                    if (!string.IsNullOrEmpty(n.AdminNotes))
                        builder.Append("<br />备注:" + n.AdminNotes + "");
                }
                builder.Append("<br /><a href=\"" + Utils.getUrl("shopbuy.aspx?act=editappbank&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">管理此结算</a>");

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
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditAppbankPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Appbank().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Appbank model = new BCW.BLL.Appbank().GetAppbank(id);
        Master.Title = "管理结算";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理结算");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "状态管理:/,管理员备注:(50字内，可空)/,,,";
        string strName = "State,AdminNotes,id,act,backurl";
        string strType = "select,text,hidden,hidden,hidden";
        string strValu = "" + model.State + "'" + model.AdminNotes + "'" + id + "'editokappbank'" + Utils.getPage(0) + "";
        string strEmpt = "0|申请中|1|已汇款|2|已失败,true,false,false,false";
        string strIdea = "/";
        string strOthe = "修改|reset,shopbuy.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />系统将本次修改结果以内线形式通知结算的会员");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=delappbank&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此记录</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("shopbuy.aspx?act=appbank") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    
    }

    private void EditOkAppbankPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-2]\d*$", "状态选择出错"));
        string AdminNotes = Utils.GetRequest("AdminNotes", "post", 3, @"^[\s\S]{1,200}$", "备注限50字,可以留空");

        BCW.Model.Appbank n = new BCW.BLL.Appbank().GetAppbank(id);
        if (n == null)
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Appbank model = new BCW.Model.Appbank();
        model.ID = id;
        model.State = State;
        model.AdminNotes = AdminNotes;
        model.ReTime = DateTime.Now;
        new BCW.BLL.Appbank().Update(model);

        //内线通知会员
        new BCW.BLL.Guest().Add(n.UsID, n.UsName, "您申请结算的推广拥金" + n.AddGold + "元，管理员已受理，[url=/shopbuy.aspx?act=getfclist]查看详情[/url]");


        Utils.Success("修改成功", "修改结算成功..", Utils.getUrl("shopbuy.aspx?act=editappbank&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelAppbankPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除结算";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此结算记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?info=ok&amp;act=delappbank&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=editappbank&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Appbank().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Appbank().Delete(0, id);
            Utils.Success("删除结算", "删除结算成功..", Utils.getUrl("shopbuy.aspx?act=appbank"), "1");
        }
    }
}