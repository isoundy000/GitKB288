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
public partial class shopbuy : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/buylist.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                SaveBuylist();
                break;
            case "info":
                InfoBuylist();
                break;
            case "payment":
                PaymentBuylist();
                break;
            case "recom":
                ReComUser();
                break;
            case "relist":
                ReListPage();
                break;
            case "getfc":
                GetFcPage();
                break;
            case "getfcok":
                GetFcOkPage();
                break;
            case "getfclist":
                GetFcListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        //用户资料
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Goods().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.Model.Goods model = new BCW.BLL.Goods().GetGoods(id);
        Master.Title = "购买宝贝";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("购买宝贝");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("单价：");
        if (model.PostType == 0)
            builder.Append("￥" + Convert.ToDouble(model.VipMoney) + "元");
        else if (model.PostType == 1)
            builder.Append(Convert.ToDouble(model.VipMoney) + "" + ub.Get("SiteBz"));
        else
            builder.Append(Convert.ToDouble(model.VipMoney) + "" + ub.Get("SiteBz2"));

        builder.Append("<br />库存：" + (model.StockCount - model.SellCount) + "件");
        if (model.PayType == 0 || model.PayType == 2)
        {
            if (model.PayType == 0)
                builder.Append("<br />送货：货到付款");
            else
                builder.Append("<br />送货：先付款后发货");
        }
        else
        {
            builder.Append("<br />送货方式:当面交易");
        }
        builder.Append(Out.Tab("</div>", ""));
        string postMoney = string.Empty;
        if (!string.IsNullOrEmpty(model.PostMoney))
        {
            string[] sTemp = model.PostMoney.Split("|".ToCharArray());
            int k = 1;
            for (int j = 0; j < sTemp.Length; j++)
            {
                if (j % 2 == 0)
                {
                    postMoney += "|" + k + "|" + sTemp[j + 1] + "" + sTemp[j].ToString() + "元";
                }
                k++;
            }
            postMoney = "0|选择邮寄" + postMoney;
        }
        else
        {
            postMoney = "0|卖家包邮";
        }
        string strText = "购买数量:/,您的姓名:/,您的手机号或固话:/,收货地址(详细到门牌号):/,邮寄方式:/,备注(如颜色和尺寸):/,,,";
        string strName = "Paycount,RealName,Mobile,Address,pType,Notes,id,act,backurl";
        string strType = "num,text,text,text,select,text,hidden,hidden,hidden";
        string strValu = "1''''0''" + id + "'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false," + postMoney + ",true,,,";
        string strIdea = "/";
        string strOthe = "立即下单|reset,shopbuy.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + model.Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SaveBuylist()
    {
        //用户资料
        int meid = new BCW.User.Users().GetUsId();
        string mename = new BCW.BLL.User().GetUsName(meid);
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Goods().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //是否刷屏
        string appName = "BUYLIST";
        int Expir = Convert.ToInt32(ub.GetSub("BuylistExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        int Paycount = int.Parse(Utils.GetRequest("Paycount", "post", 2, @"^[0-9]\d*$", "购买数量错误"));
        string RealName = Utils.GetRequest("RealName", "post", 2, @"^[\u4e00-\u9fa5]{2,8}$", "姓名填写错误");
        string Mobile = Utils.GetRequest("Mobile", "post", 2, @"^(?:13|15|18)\d{9}$|^(\d{3}-|\d{4}-)?(\d{8}|\d{7})?(-\d+)?$", "联系电话不正确,必填固话或者手机号");
        string Address = Utils.GetRequest("Address", "post", 2, @"^[\s\S]{8,}$", "收货地址请尽量详细到门牌号");
        int pType = int.Parse(Utils.GetRequest("pType", "post", 2, @"^[0-9]\d*$", "邮寄方式选择错误"));
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[\s\S]{1,200}$", "备注限200字,可以留空");

        BCW.Model.Goods modelGoods = new BCW.BLL.Goods().GetGoods(id);
        Master.Title = "购买宝贝";
        builder.Append(Out.Tab("<div class=\"title\">购买宝贝</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if ((modelGoods.StockCount - modelGoods.SellCount) < Paycount)
        {
            Utils.Error("对不起,目前最多可以购买"+(modelGoods.StockCount - modelGoods.SellCount)+"件", "");
        }
        int pMoney = 0;
        if (!string.IsNullOrEmpty(modelGoods.PostMoney))
        {
            string[] sTemp = modelGoods.PostMoney.Split("|".ToCharArray());
            int k = 1;
            pMoney = -1;
            for (int j = 0; j < sTemp.Length; j++)
            {
                if (j % 2 == 0)
                {
                    if (k == pType)
                    {
                        pMoney = Convert.ToInt32(sTemp[j].ToString());
                        break;
                    }
                }
                k++;
            }
            if (pMoney == -1)
            {
                Utils.Error("邮寄方式选择错误", "");
            }
        }

        //扣币与内线
        if (modelGoods.PostType > 0)
        {
            if (modelGoods.PostType == 1)
            {
                if (new BCW.BLL.User().GetGold(meid) < Convert.ToInt64(modelGoods.VipMoney))
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                }
                else if (modelGoods.PostType == 2)
                {
                    if (new BCW.BLL.User().GetMoney(meid) < Convert.ToInt64(modelGoods.VipMoney))
                    {
                        Utils.Error("您的" + ub.Get("SiteBz2") + "不足", "");
                    }
                }

                if (modelGoods.PostType == 1)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(modelGoods.VipMoney), "购买商品《" + modelGoods.Title + "》");
                    //发信给下订单的会员
                    new BCW.BLL.Guest().Add(meid, mename, "您已成功购买[url=/shopdetail.aspx?id=" + id + "]" + modelGoods.Title + "[/url]，扣除您的" + Convert.ToInt64(modelGoods.VipMoney) + "" + ub.Get("SiteBz") + "，多谢惠顾！[br][url=/myshop.aspx]&gt;我的订单[/url]");
                }
                else if (modelGoods.PostType == 2)
                {
                    new BCW.BLL.User().UpdateiMoney(meid, mename, -Convert.ToInt64(modelGoods.VipMoney), "购买商品《" + modelGoods.Title + "》");
                    //发信给下订单的会员
                    new BCW.BLL.Guest().Add(meid, mename, "您已成功购买[url=/shopdetail.aspx?id=" + id + "]" + modelGoods.Title + "[/url]，扣除您的" + Convert.ToInt64(modelGoods.VipMoney) + "" + ub.Get("SiteBz2") + "，多谢惠顾！[br][url=/myshop.aspx]&gt;我的订单[/url]");

                }
            }
        }
        else
        {
            //发信给下订单的会员
            new BCW.BLL.Guest().Add(meid, mename, "您已成功购买[url=/shopdetail.aspx?id=" + id + "]" + modelGoods.Title + "[/url]，如果商品要求先付款再发货，请尽快付款，多谢惠顾！[br][url=/myshop.aspx]&gt;我的订单[/url]");
        }

        //生成订单号
        string Ding = DT.getDateTimeNum();
        //写入订单
        BCW.Model.Buylist model = new BCW.Model.Buylist();
        model.TingNo = Ding;
        model.NodeId = new BCW.BLL.Goods().GetNodeId(id);
        model.GoodsId = id;
        model.Title = modelGoods.Title;
        model.Price = modelGoods.VipMoney;
        model.Paycount = Paycount;

        if (modelGoods.PayType == 1)
            model.PostMoney = -1;
        else
            model.PostMoney = pMoney;

        model.SellId = 0;//系统默认ID，说明是系统发布的商品
        model.UserId = meid;
        model.UserName = mename;
        model.RealName = RealName;
        model.Mobile = Mobile;
        model.Address = Address;
        model.Notes = Notes;
        model.AcPrice = Convert.ToInt32(Paycount * modelGoods.VipMoney);
        model.AcStats = 0;
        model.AcText = "";
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;
        new BCW.BLL.Buylist().Add(model);
        //更新购买人数
        new BCW.BLL.Goods().UpdatePaycount(id, 1);
        //更新出售数量
        new BCW.BLL.Goods().UpdateSellCount(id, Paycount);

        builder.Append("购买宝贝成功");
        builder.Append("<br />订单号:" + Ding + "");
        builder.Append("<br />进入:<a href=\"" + Utils.getUrl("myshop.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我的订单</a>");     
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    private void InfoBuylist()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Goods().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        string sTitle = string.Empty;
        if (ptype == 0)
            sTitle = "成交列表";
        else
            sTitle = "评价列表";

        Master.Title = sTitle;
        builder.Append(Out.Tab("<div class=\"title\">" + sTitle + "</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += " GoodsId=" + id + "";
        if (ptype == 1)
        {
            strWhere += " and LaNotes IS NOT NULL";
        }
        // 开始读取列表
        IList<BCW.Model.Buylist> listBuylist = new BCW.BLL.Buylist().GetBuylists(pageIndex, pageSize, strWhere, out recordCount);
        if (listBuylist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Buylist n in listBuylist)
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
                //recordCount - (((pageIndex - 1) * pageSize) + (k - 1))
                if (ptype == 0)
                    builder.AppendFormat("{0}{1}购买{2}件[{3}]", "☆", BCW.User.Users.FormatMobile(n.Mobile), n.Paycount, DT.FormatDate(n.AddTime, 5));
                else
                    builder.AppendFormat("{0}{1}{2}[{3}]", "☆", BCW.User.Users.FormatMobile(n.Mobile), n.LaNotes, DT.FormatDate(n.AddTime, 5));

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
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">.." + new BCW.BLL.Goods().GetTitle(id) + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void PaymentBuylist()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Goods().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = "付款方式";
        builder.Append(Out.Tab("<div class=\"title\">付款方式</div>", ""));
        builder.Append(Out.Div("div", Out.SysUBB(ub.GetSub("BuylistpText", xmlPath))));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReComUser()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Goods().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = "有奖推荐商品";
        builder.Append(Out.Tab("<div class=\"title\">有奖推荐商品</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本商品推荐地址：<br />http://" + Utils.GetDomain() + "/reg-" + meid + ".aspx?backurl=" + Server.UrlEncode("shopdetail.aspx?id=" + id + "") + "");
        
        
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Div("div", Out.SysUBB(ub.GetSub("BuylistpText2", xmlPath))));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReListPage()
    {
        //用户资料
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的推广记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我的推广记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += "ZrID=" + meid + "";
        // 开始读取列表
        IList<BCW.Model.Shoptg> listShoptg = new BCW.BLL.Shoptg().GetShoptgs(pageIndex, pageSize, strWhere, out recordCount);
        if (listShoptg.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shoptg n in listShoptg)
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
                builder.Append("[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                builder.Append("" + n.Notes + "<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + n.DetailId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">详情&gt;&gt;</a>");
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
        builder.Append("<a href=\"" + Utils.getPage("/bbs/uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/shopbuy.aspx?act=getfc") + "\">我要结算</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GetFcPage()
    {        
        //用户资料
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "申请结算";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("申请结算");
        builder.Append(Out.Tab("</div>", "<br />"));
        long fcgold = new BCW.BLL.User().GetFcGold(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我的推广拥金:" + fcgold + "元");
        builder.Append(Out.Tab("</div>", "<br />"));
        long fcmax = Utils.ParseInt64(ub.GetSub("BuylistReMax", xmlPath));

        if (fcgold < fcmax)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("拥金未达" + fcmax + "元,你还不可以申请结算");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //上一次交易资料
            BCW.Model.Appbank model = new BCW.BLL.Appbank().GetAppbankLast(0, meid);

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("拥金已达" + fcmax + "元,你目前可以申请结算");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "结算数量:/,您的手机号:/,银行卡号:/,你的卡号姓名:/,开户银行名称:/,备注(可空，50字内):/,,";
            string strName = "GoldNum,Mobile,CardNum,CardName,CardAddress,Notes,act,backurl";
            string strType = "num,num,text,text,text,text,hidden,hidden";
            string strValu = "";
            if (model == null)
                strValu = "''''''getfcok'" + Utils.getPage(0) + "";
            else
                strValu = "'" + model.Mobile + "'" + model.CardNum + "'" + model.CardName + "'" + model.CardAddress + "''getfcok'" + Utils.getPage(0) + "";

            string strEmpt = "false,false,false,false,false,true,,,";
            string strIdea = "/";
            string strOthe = "立即结算|reset,shopbuy.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("注意开户银行名称例子:<br />中国工商银行广州天河支行");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/shopbuy.aspx?act=getfclist") + "\">结算记录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GetFcOkPage()
    {
        //用户资料
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        long GoldNum = Int64.Parse(Utils.GetRequest("GoldNum", "post", 4, @"^[1-9]\d*$", "结算数量"));
        string Mobile = Utils.GetRequest("Mobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string CardNum = Utils.GetRequest("CardNum", "post", 2, @"^[0-9]{19}$", "请正确输入银行卡号");
        string CardName = Utils.GetRequest("CardName", "post", 2, @"^[^\^]{2,10}$", "请正确输入卡号姓名");
        string CardAddress = Utils.GetRequest("CardAddress", "post", 2, @"^[^\^]{2,100}$", "请正确输入开户银行名称");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "备注限50字内，可以留空");

        BCW.Model.Appbank model = new BCW.Model.Appbank();
        model.Types = 0;
        model.AddGold = GoldNum;
        model.UsID = meid;
        model.UsName = new BCW.BLL.User().GetUsName(meid);
        model.Mobile = Mobile;
        model.CardNum = CardNum;
        model.CardName = CardName;
        model.CardAddress = CardAddress;
        model.Notes = Notes;
        model.State = 0;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Appbank().Add(model);
        //扣除拥金
        new BCW.BLL.User().UpdateiFcGold(meid, -GoldNum);


        Master.Title = "申请结算";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("申请结算成功");
        builder.Append("<br />申请结算:" + GoldNum + "元");
        builder.Append("<br />拥金余额:" + new BCW.BLL.User().GetFcGold(meid) + "元");
        builder.Append("<br />请耐心等待管理员进行审核并支付，你可以通过结算记录功能进行查看本次结算状态.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/shopbuy.aspx?act=getfclist") + "\">结算记录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GetFcListPage()
    {
        //用户资料
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的结算记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我的结算记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += "Types=0 and UsID=" + meid + "";
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

                builder.Append("<b>[" + sText + "]</b>申请" + n.AddGold + "元[" + DT.FormatDate(n.AddTime, 1) + "]");
                if (!string.IsNullOrEmpty(n.Notes))
                    builder.Append("<br />备注:" + n.Notes + "");

                if (n.State > 0)
                {
                    builder.Append("<br />受理时间:" + n.ReTime + "");
                    if (!string.IsNullOrEmpty(n.AdminNotes))
                        builder.Append("<br />管理员备注:" + n.AdminNotes + "");
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/shopbuy.aspx?act=getfc") + "\">我要结算</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

}