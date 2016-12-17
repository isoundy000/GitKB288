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
using System.Text.RegularExpressions;
using BCW.Common;
/// <summary>
/// 提交到指定域名充值 黄国军 20161024
/// 充值商品跳转 黄国军 20160704
/// 增加充值提醒 黄国军 16/06/20
/// 增加RMB商品 类别28 黄国军 16/05/18
/// 修改排行榜 陈志基 16/5/12
/// 蒙宗将 16/8/29 增加实物类
/// </summary>
public partial class bbs_bbsshop : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbsshop.xml";
    protected string xmlPath2 = "/Controls/bbs.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("BbsshopStatus", xmlPath) == "1")
        {
            Utils.Safe("社区商城");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "medal":
                MedalPage();
                break;
            case "memedal":
                MeMedalPage();
                break;
            case "delmedal":
                DelMedalPage();
                break;
            case "medallog":
                MedalLogPage();
                break;
            case "delmedallog":
                DelMedalLogPage();
                break;
            case "medalview":
                MedalViewPage();
                break;
            case "medalset":
                MedalSetPage();
                break;
            case "grant":
                GrantPage();
                break;
            case "grantsave":
                GrantSavePage();
                break;
            case "list":                //商城导购
                ListPage();
                break;
            case "gift":                //更多新品
                GiftPage();
                break;
            case "giftview":            //商品详情
                GiftViewPage();
                break;
            case "giftsell":            //购买输入页
                GiftSellPage();
                break;
            case "MerBillPay":          //环迅订单支付页面
                MerBillPayPage();
                break;
            case "store":               //我的储物箱
                StorePage();
                break;
            case "goods":
                GoodsPage();            //实物类商品
                break;
            case "storemy":
                StoreMyPage();
                break;
            case "goodsadd":
                GoodsaddPage();//补填收货信息 
                break;
            case "goodsview":
                GoodsviewPage();//查看商品信息
                break;
            case "search":
                SearchPage();
                break;
            case "giftinfo":
                GiftInfoPage();
                break;
            case "proxy":
                ProxyPage();
                break;
            case "proxyok":
                ProxyOkPage();
                break;
            case "send":
                SendPage();
                break;
            case "top":
                TopPage();
                break;
            case "topuser":
                TopUserPage();
                break;
            case "mygift":
                MyGiftPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "giftmenu":
                GiftMenuPage();
                break;
            case "medaltop":
                MedalTopPage();
                break;
            default:
                ReloadPage();   //商城首页
                break;
        }
    }

    #region 商城首页 ReloadPage
    /// <summary>
    /// 商城首页
    /// </summary>
    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int NewNum = Utils.ParseInt(ub.GetSub("BbsshopNewNum", xmlPath));
        int GoodNum = Utils.ParseInt(ub.GetSub("BbsshopGoodNum", xmlPath));
        int HotNum = Utils.ParseInt(ub.GetSub("BbsshopHotNum", xmlPath));
        int ActNum = Utils.ParseInt(ub.GetSub("BbsshopActNum", xmlPath));

        Master.Title = "社区商城";
        string Logo = ub.GetSub("BbsshopLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(3));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=list&amp;backurl=" + Utils.getPage(0) + "") + "\">导购</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=top&amp;backurl=" + Utils.getPage(0) + "") + "\">排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=search&amp;backurl=" + Utils.getPage(0) + "") + "\">搜索</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">充值" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz") + "区</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz2") + "区</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">免费区</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">网银类</a>");
        builder.Append(Out.Tab("</div>", ""));
        DataSet ds = null;
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【新品上架】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        ds = new BCW.BLL.Shopgift().GetList("TOP " + NewNum + " ID,Title,Pic,BzType,Price", "NodeId>0 AND NodeId<>28 ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<img src=\"" + ds.Tables[0].Rows[i]["Pic"] + "\" alt=\"load\"/><br />");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "") + "\">" + ds.Tables[0].Rows[i]["Title"] + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[购买]</a><br />");
                if (ds.Tables[0].Rows[i]["BzType"].ToString() == "0")
                    builder.Append("售价:" + ds.Tables[0].Rows[i]["Price"] + "" + ub.Get("SiteBz") + "<br />");
                else
                    builder.Append("售价:" + ds.Tables[0].Rows[i]["Price"] + "" + ub.Get("SiteBz2") + "<br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;ptype=3") + "\">&gt;&gt;更多新品</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【精品推荐】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        ds = new BCW.BLL.Shopgift().GetList("TOP " + GoodNum + " ID,Title,Pic,BzType,Price", "IsRecom=1 ORDER BY NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<img src=\"" + ds.Tables[0].Rows[i]["Pic"] + "\" alt=\"load\"/><br />");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "") + "\">" + ds.Tables[0].Rows[i]["Title"] + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[购买]</a><br />");
                if (ds.Tables[0].Rows[i]["BzType"].ToString() == "0")
                    builder.Append("售价:" + ds.Tables[0].Rows[i]["Price"] + "" + ub.Get("SiteBz") + "<br />");
                else
                    builder.Append("售价:" + ds.Tables[0].Rows[i]["Price"] + "" + ub.Get("SiteBz2") + "<br />");
            }
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【热销商品】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        ds = new BCW.BLL.Shopgift().GetList("TOP " + HotNum + " ID,Title,PrevPic", "NodeId>0 AND IsRecom=1 ORDER BY PayCount DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<img src=\"" + ds.Tables[0].Rows[i]["PrevPic"] + "\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "") + "\">" + ds.Tables[0].Rows[i]["Title"] + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[购买]</a><br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;ptype=4") + "\">&gt;&gt;更多热销</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【友友们在干嘛】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        ds = new BCW.BLL.Action().GetList("TOP " + ActNum + " Notes,AddTime", "Types=12 AND (NodeId <> 609) Order by id desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.AppendFormat("{0}前{1}<br />", DT.DateDiff2(DateTime.Now, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString())), Out.SysUBB(ds.Tables[0].Rows[i]["Notes"].ToString()));
            }
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?ptype=12&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;更多动态</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【商品导购】<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medal") + "\">勋章</a>");
        builder.Append(Out.Tab("</div>", ""));

        ds = new BCW.BLL.Shoplist().GetList("ID,Title", "Types=0 and ID<>27");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "") + "\">" + ds.Tables[0].Rows[i]["Title"] + "</a> ");
                if ((i + 1) % 4 == 0)
                    builder.Append("<br />");

            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        #region 更新默认收货时间
        ds = new BCW.Shop.BLL.Shopgoods().GetList("ID,SendTime,ReceiveTime,Title,UsID", "SendTime!='" + Convert.ToDateTime("2000-10-10 00:00:00") + "' and ReceiveTime='" + Convert.ToDateTime("2000-10-10 00:00:00") + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DateTime dt2 = DateTime.Now;
                DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["SendTime"]);
                TimeSpan ts = dt2 - dt1;
                if (ts.Days > Convert.ToInt32(ub.GetSub("BbsshopReceiveTime", xmlPath)))
                {
                    new BCW.Shop.BLL.Shopgoods().UpdateReceivebyID(Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]), DateTime.Now);
                    //发内线
                    string strLog = "根据你在商城购买的单号为" + Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]) + "的商品" + Convert.ToString(ds.Tables[0].Rows[i]["Title"]) + "，由于你的商品已经发货，且时间超过默认确认收货时间(" + Convert.ToInt32(ub.GetSub("BbsshopReceiveTime", xmlPath)) + "天)也没收到你确认收货信息，系统已经自动更新你的商品为送达状态，若有问题，请与客服联系，谢谢！" + "[url=/bbs/bbsshop.aspx]进入商城[/url]";
                    new BCW.BLL.Guest().Add(0, Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]), new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"])), strLog);
                }
            }
        }
        #endregion

        builder.Append(Out.Tab("<div class=\"text\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">储物箱</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=send") + "\">送礼记录</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">充值</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 商城导购 ListPage
    private void ListPage()
    {
        Master.Title = "商城导购";
        int ptype = 0;
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(3));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;导购");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = 20;
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=" + ptype + " and ID<>27";

        // 开始读取专题
        IList<BCW.Model.Shoplist> listShoplist = new BCW.BLL.Shoplist().GetShoplists(pageIndex, pageSize, strWhere, out recordCount);
        if (listShoplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shoplist n in listShoplist)
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
                if (n.Types == 0)
                    builder.Append("<img src=\"/Files/sys/gift.gif\" alt=\"load\"/>");
                else if (n.Types == 1)
                    builder.Append("<img src=\"/Files/sys/props.gif\" alt=\"load\"/>");

                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 更多新品 GiftPage
    /// <summary>
    /// 更多新品
    /// </summary>
    private void GiftPage()
    {
        int meid = new BCW.User.Users().GetUsId();

        int id = 0;
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-5]$", "-1"));
        if (ptype == -1)
            id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        string Title = string.Empty;
        if (id > 0)
        {
            if (id == 27)
            {
                Utils.Error("不存在的分类", "");
            }
            Title = new BCW.BLL.Shoplist().GetTitle(id);
            if (Title == "")
            {
                Utils.Error("不存在的分类", "");
            }
        }
        else
        {
            if (ptype == 0)
                Title = "" + ub.Get("SiteBz") + "区";
            else if (ptype == 1)
                Title = "" + ub.Get("SiteBz2") + "区";
            else if (ptype == 2)
                Title = "免费区";
            else if (ptype == 3)
                Title = "新品";
            else if (ptype == 4)
                Title = "热销";
            else if (ptype == 5)
                Title = "网银类";
        }
        Master.Title = "礼物商城";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(3));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;" + Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = 5;
        string[] pageValUrl = { "act", "ptype", "id" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == -1)
            strWhere = "NodeId=" + id + "";
        else if (ptype == 0)
            strWhere = "BzType=0";
        else if (ptype == 1)
            strWhere = "BzType=1";
        else if (ptype == 2)
            strWhere = "Price=0";
        else if (ptype == 5)
            strWhere = "NodeId=28";

        if (ptype == 4)
            strOrder = "PayCount,ID DESC";
        else
            strOrder = "ID DESC";

        // 开始读取专题
        IList<BCW.Model.Shopgift> listShopgift = new BCW.BLL.Shopgift().GetShopgifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listShopgift.Count > 0)
        {
            int k = 1;
            int j = 0;
            foreach (BCW.Model.Shopgift n in listShopgift)
            {
                if (n.NodeId == 28)
                {
                    if (n.IDS != "" && n.IDS != null)
                    {
                        string ids = "#" + n.IDS + "#";
                        if (!ids.Contains("#" + meid + "#"))
                        {
                            j++;
                            continue;
                        }
                    }
                }
                k = k - j;
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<img src=\"" + n.Pic + "\" alt=\"load\"/>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[购买]</a>");

                if (n.NodeId != 28)
                {
                    builder.Append("<br />" + OutMei(n.Para));

                    builder.Append(" 售价:" + n.Price + "");
                    if (n.BzType == 0)
                        builder.Append(ub.Get("SiteBz"));
                    else
                        builder.Append(ub.Get("SiteBz2"));
                }
                else
                {
                    builder.Append("<br />+" + n.Para + ub.Get("SiteBz"));
                    builder.Append(" 售价:" + n.Price + "");
                    builder.Append("元");
                }

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            recordCount = recordCount - j;
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            if (recordCount == 0)
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("", "<br />"));

        if (ptype == 5)
        {
            if (!Utils.GetDomain().Contains("dyj6"))
            {
                builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
                builder.Append(Out.SysUBB("[红]站方只提供一个虚拟娱乐平台," + ub.Get("SiteBz") + "只能通过[url=/bbs/bbsshop.aspx?act=gift&ptype=5]商城[/url]充值[br]站内不以银行卡等方式出售虚拟" + ub.Get("SiteBz") + ",同时不支持回收[br]对论坛个人交易、中介交易不支持也不反对,发生的纠纷也不予处理.[/红]"));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }

        string strText = ",,,,";
        string strName = "keyword,act,backurl";
        string strType = "stext,hidden,hidden";
        string strValu = "'search'" + Utils.PostPage(1) + "";
        string strEmpt = "true,false,false";
        string strIdea = "";
        string strOthe = "搜索,bbsshop.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 商品详情 GiftViewPage
    /// <summary>
    /// 商品详情
    /// </summary>
    private void GiftViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Shopgift model = new BCW.BLL.Shopgift().GetShopgift(id);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }

        if (model.NodeId == 28)
        {
            if (model.IDS != "" && model.IDS != null)
            {
                string ids = "#" + model.IDS + "#";
                if (!ids.Contains("#" + meid + "#"))
                {
                    string url = Utils.getUrl("bbsshop.aspx?act=giftview&id=610");
                    url = Server.HtmlDecode(url);
                    Response.Redirect(url);
                }
            }
        }

        Master.Title = "礼物商城";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(3));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=gift&amp;id=" + model.NodeId + "") + "\">" + new BCW.BLL.Shoplist().GetTitle(model.NodeId) + "</a>&gt;商品详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("<img src=\"" + model.Pic + "\" alt=\"load\"/><br />");
        builder.Append(model.Notes);
        if (model.IsSex == 1)
            builder.Append("<br />本礼物只能送给女生哦!");
        else if (model.IsSex == 2)
            builder.Append("<br />本礼物只能送给男生哦!");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));

        if (model.Total != -1)
        {
            if (model.NodeId == 29)
            {
                builder.Append("<h style=\"color:red\">此商品为实物，购买需要填写收货信息</h><br />");
                builder.Append("商品当前库存" + model.Total + "个<br />");
            }
            else
            {
                builder.Append("商品当前库存" + model.Total + "个<br />");
            }
        }
        if (model.NodeId != 28)
        {
            if (model.BzType == 0)
                builder.Append("售价:" + model.Price + "" + ub.Get("SiteBz") + "");
            else
                builder.Append("售价:" + model.Price + "" + ub.Get("SiteBz2") + "");

            if (model.IsVip == 1)
            {
                builder.Append("<br />VIP折扣:" + ub.GetSub("BbsshopVipSec", xmlPath) + "折");
            }

            builder.Append("<br />" + OutMei(model.Para));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[购买]</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("你自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("你自带:" + new BCW.BLL.User().GetMoney(meid) + "" + ub.Get("SiteBz2") + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">[我要充值]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append("售价:" + model.Price + "元");

            builder.Append("<br />+" + (model.Para) + ub.Get("SiteBz"));
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("你自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("温馨提示:该商品通过网银直接购买可立即获得" + ub.Get("SiteBz") + "<br />获得数量:购买数量*" + model.Para + "=获得总币数<br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[购买]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=gift&amp;id=" + model.NodeId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 购买输入页 GiftSellPage
    /// <summary>
    /// 购买输入页
    /// </summary>
    private void GiftSellPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Shopgift model = new BCW.BLL.Shopgift().GetShopgift(id);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        Master.Title = "购买商品";
        string info = Utils.GetRequest("info", "post", 1, "", "");

        if (info == "ok")
        {
            int num = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "购买个数错误"));
            #region 确认购买 普通商品
            if (model.NodeId != 28)
            {
                if (model.NodeId == 29)
                {
                    if (num > Convert.ToInt32(ub.GetSub("BbsshopMaxnum", xmlPath)))
                    {
                        Utils.Error("购买个数限1-" + Convert.ToInt32(ub.GetSub("BbsshopMaxnum", xmlPath)) + "个", "");
                    }
                }
                else
                {
                    if (num > 1000)
                    {
                        Utils.Error("购买个数限1-1000个", "");
                    }
                }
                if (model.Total != -1 && model.Total < num)
                {
                    Utils.Error("商品库存不足，目前只可购买" + model.Total + "个", "");
                }
                //免费区的礼物每天限每ID只可以购买1份|" + ub.Get("SiteBz2") + "区的礼物则是3份（可后台配置）
                int mfTotal = Utils.ParseInt(ub.GetSub("BbsshopMfTotal", xmlPath));
                int bgTotal = Utils.ParseInt(ub.GetSub("BbsshopBgTotal", xmlPath));
                int aTotal = new BCW.BLL.Shopkeep().GetTodayCount(meid, id);
                if (model.Price == 0)
                {
                    if (aTotal + num > mfTotal)
                    {
                        Utils.Error("免费区的礼物每人每天只可以购买" + mfTotal + "份，您今天已购买" + aTotal + "份", "");
                    }
                }
                else if (model.BzType == 1)
                {
                    if (aTotal + num > bgTotal)
                    {
                        Utils.Error("" + ub.Get("SiteBz2") + "区的礼物每人每天只可以购买" + bgTotal + "份，您今天已购买" + aTotal + "份", "");
                    }
                }

                //得到昵称
                string mename = new BCW.BLL.User().GetUsName(meid);
                //操作币
                long AcPrice = Convert.ToInt64(model.Price * num);
                double VipSec = Convert.ToDouble(ub.GetSub("BbsshopVipSec", xmlPath));
                if (model.IsVip == 1)
                {
                    int VipLeven = BCW.User.Users.VipLeven(meid);
                    if (VipLeven > 0)
                        AcPrice = Convert.ToInt64(AcPrice * VipSec * 0.1);
                }
                if (model.BzType == 0)
                {
                    if (new BCW.BLL.User().GetGold(meid) < AcPrice)
                    {
                        Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                    }
                    //支付安全提示
                    string[] p_pageArr = { "act", "info", "id", "num", "backurl" };
                    BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                    new BCW.BLL.User().UpdateiGold(meid, mename, -AcPrice, "购买商品" + model.Title + "" + num + "个");
                }
                else
                {
                    if (new BCW.BLL.User().GetMoney(meid) < AcPrice)
                    {
                        Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
                    }
                    new BCW.BLL.User().UpdateiMoney(meid, mename, -AcPrice, "购买商品" + model.Title + "" + num + "个");
                }

                BCW.Model.Shopkeep keep = new BCW.Model.Shopkeep();
                keep.GiftId = id;
                keep.Title = model.Title.Trim();
                keep.Pic = model.Pic;
                keep.PrevPic = model.PrevPic;
                keep.Notes = model.Notes;
                keep.IsSex = model.IsSex;
                keep.Para = model.Para;
                keep.UsID = meid;
                keep.UsName = mename;
                keep.Total = num;
                keep.TopTotal = num;
                keep.AddTime = DateTime.Now;

                //产品分类ID28为在线支付商品
                keep.NodeId = model.NodeId;
                keep.MerBillNo = "Shop" + DT.getDateTimeNum();              //商户订单号 必填字母及数字
                keep.State = 0;                                             //未处理
                keep.Amount = decimal.Parse(AcPrice.ToString("0.00"));      //充值金额 2位小数
                keep.GatewayType = "";                                      //默认未支付为空
                keep.Attach = "";                                           //默认未支付为空
                keep.BillEXP = 3;                                           //默认3小时自动取消订单
                keep.GoodsName = model.Title.Trim();                        //商品名称            
                keep.IsCredit = "1";                                        //直连选项 1
                keep.BankCode = "";                                         //IPS唯一标识指定的银行编号 00018
                keep.ProductType = "-1";                                    //产品类型 -1未填 1个人银行 2企业银行
                keep.AddTime = DateTime.Now;

                int KeepId = new BCW.BLL.Shopkeep().GetID(id, meid);
                if (KeepId == 0)
                {
                    KeepId = new BCW.BLL.Shopkeep().Add(keep);
                }
                else
                {
                    new BCW.BLL.Shopkeep().Update(keep);
                }
                //购买库存与出售数量
                int num2 = -num;
                if (model.Total == -1)
                {
                    num2 = 0;
                }
                new BCW.BLL.Shopgift().Update(id, num, num2);
                //更新此分类出售数量
                new BCW.BLL.Shoplist().Update(model.NodeId, num);
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/bbsshop.aspx]礼物商城[/url]购买了[url=/bbs/bbsshop.aspx?act=giftview&amp;id=" + id + "]" + model.Title.Trim() + "[img]" + model.PrevPic + "[/img][/url]";
                new BCW.BLL.Action().Add(12, id, meid, mename, wText);
                if (model.NodeId == 29)
                {
                    //确认信息
                    BCW.Shop.Model.Shopgoods addgoods = new BCW.Shop.Model.Shopgoods();
                    addgoods.Title = model.Title.Trim();
                    addgoods.GiftId = model.NodeId;
                    addgoods.PrevPic = model.PrevPic;
                    addgoods.Num = num;//??
                    addgoods.BuyTime = DateTime.Now;
                    addgoods.SendTime = Convert.ToDateTime("2000-10-10 00:00:00");//表示没有发货
                    addgoods.ReceiveTime = Convert.ToDateTime("2000-10-10 00:00:00");
                    addgoods.UsID = meid;
                    addgoods.RealName = "";
                    addgoods.Address = "";
                    addgoods.Phone = "";
                    addgoods.Email = "";
                    addgoods.Message = "";
                    addgoods.ShopGiftId = id;
                    addgoods.Express = "";
                    addgoods.Expressnum = "";
                    int ID = new BCW.Shop.BLL.Shopgoods().Add(addgoods);

                    //发内线
                    string strLog = "礼物商城会员(" + meid + ")购买实物商品购买单号为" + model.ID + "的" + model.Title + "，请等待用户录入收货信息，收到收货信息填写成功后，请前往【商城管理-商品派送】查看并派送..";
                    new BCW.BLL.Guest().Add(0, 10086, new BCW.BLL.User().GetUsName(10086), strLog);
                    Utils.Success("购买商品", "购买商品成功,前往填写收货信息", Utils.getUrl("bbsshop.aspx?act=goods&amp;id=" + ID + ""), "2");
                }
                else
                {
                    Utils.Success("购买商品", "购买商品成功<br /><a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/bbsshop.aspx?act=proxy&amp;id=" + KeepId + "") + "") + "\">马上赠送&gt;&gt;</a><br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">&lt;&lt;我的储物箱</a>", Utils.getPage("bbsshop.aspx"), "5");
                }
            }
            #endregion

            #region 确认充值商品
            else
            {
                if (num > 100000)
                {
                    Utils.Error("购买个数限1-100000个", "");
                }
                if (model.Total != -1 && model.Total < num)
                {
                    Utils.Error("商品库存不足，目前只可购买" + model.Total + "个", "");
                }

                //得到昵称
                string mename = new BCW.BLL.User().GetUsName(meid);
                //操作金额
                long AcPrice = Convert.ToInt64(model.Price * num);

                BCW.Model.Shopkeep keep = new BCW.Model.Shopkeep();
                keep.GiftId = id;
                keep.Title = model.Title.Trim();
                keep.Pic = model.Pic;
                keep.PrevPic = model.PrevPic;
                keep.Notes = model.Notes;
                keep.IsSex = model.IsSex;
                keep.Para = model.Para;
                keep.UsID = meid;
                keep.UsName = mename;
                keep.Total = num;
                keep.TopTotal = num;
                //产品分类ID28为在线支付商品
                keep.NodeId = model.NodeId;
                keep.MerBillNo = "Shop" + DT.getDateTimeNum();              //商户订单号 必填字母及数字
                keep.State = 0;                                             //未处理
                keep.Amount = decimal.Parse(AcPrice.ToString("0.00"));      //充值金额 2位小数
                keep.GatewayType = "";                                      //默认未支付为空
                keep.Attach = "";                                           //默认未支付为空
                keep.BillEXP = 3;                                           //默认3小时自动取消订单
                keep.GoodsName = model.Title.Trim();                        //商品名称            
                keep.IsCredit = "1";                                        //直连选项 1
                keep.BankCode = "";                                         //IPS唯一标识指定的银行编号 00018
                keep.ProductType = "-1";                                    //产品类型 -1未填 1个人银行 2企业银行
                keep.AddTime = DateTime.Now;
                int KeepId = 0;
                KeepId = new BCW.BLL.Shopkeep().Add(keep);

                //购买库存与出售数量
                int num2 = -num;
                if (model.Total == -1)
                {
                    num2 = 0;
                }
                new BCW.BLL.Shopgift().Update(id, num, num2);
                //更新此分类出售数量
                new BCW.BLL.Shoplist().Update(model.NodeId, num);
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/bbsshop.aspx]礼物商城[/url]购买了[url=/bbs/bbsshop.aspx?act=giftview&amp;id=" + id + "]" + model.Title.Trim() + "[img]" + model.PrevPic + "[/img]订单号:" + keep.MerBillNo + "[/url]";
                new BCW.BLL.Action().Add(12, id, meid, mename, wText);
                if (keep.NodeId == 28)
                {
                    //new BCW.BLL.Guest().Add(10086, "客服", "ID：" + keep.UsID + "商城购物支付,生成订单成功,单号:" + keep.MerBillNo);
                    Utils.Success("商品订单生成成功", "正在为你转向支付页面,请稍后", Utils.getUrl("bbsshop.aspx?act=MerBillPay&amp;KeepId=" + KeepId), "2");
                }
                //Utils.Success("购买商品", "购买商品成功<br /><a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/bbsshop.aspx?act=proxy&amp;id=" + KeepId + "") + "") + "\">马上赠送&gt;&gt;</a><br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">&lt;&lt;我的储物箱</a>", Utils.getPage("bbsshop.aspx"), "5");
            }
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;购买商品");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您选择了“<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Title + "</a>”");
        #region  输入购买数量
        if (model.NodeId != 28)
        {
            if (model.BzType == 0)
                builder.Append("每个需支付" + model.Price + "" + ub.Get("SiteBz") + "");
            else
                builder.Append("每个需支付" + model.Price + "" + ub.Get("SiteBz2") + "");

            if (model.IsVip == 1)
            {
                builder.Append("(VIP" + ub.GetSub("BbsshopVipSec", xmlPath) + "折优惠)");
            }

            if (model.Total != -1)
            {
                if (model.NodeId == 29)
                {
                    builder.Append("<br /><h style=\"color:red\">此商品为实物，购买需要填写收货信息</h>");
                    builder.Append("<br />商品当前库存" + model.Total + "个");
                }
                else
                {
                    builder.Append("<br />商品当前库存" + model.Total + "个");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
            string strText = string.Empty;
            if (model.NodeId == 29)
            {
                strText = "输入个数:(最少1个|最多" + Convert.ToInt32(ub.GetSub("BbsshopMaxnum", xmlPath)) + "个):/,,,,";
            }
            else
            {
                strText = "输入个数:(最少1个|最多1000个):/,,,,";
            }
            string strName = "num,id,act,info,backurl";
            string strType = "snum,hidden,hidden,hidden,hidden";
            string strValu = "1'" + id + "'giftsell'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "个''''|";
            string strOthe = "购买,bbsshop.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("你自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("你自带:" + new BCW.BLL.User().GetMoney(meid) + "" + ub.Get("SiteBz2") + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">[我要充值]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append("每个需支付" + model.Price + "元");

            if (model.Total != -1)
            {
                builder.Append("<br />商品当前库存" + model.Total + "个");
            }
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入个数:(最少1个|最多100000个):/,,,,";
            string strName = "num,id,act,info,backurl";
            string strType = "snum,hidden,hidden,hidden,hidden";
            string strValu = "1'" + id + "'giftsell'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "个''''|";
            string strOthe = "购买,bbsshop.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("你目前有:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=gift&amp;id=" + model.NodeId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 环迅支付页面 MerBillPayPage
    private void MerBillPayPage()
    {
        int meid = 0;
        if (!Utils.GetDomain().Contains("ips.rdfmy.top"))
        {
            meid = new BCW.User.Users().GetUsId();
        }
        else
        {
            meid = BCW.User.Users.userId();
        }
        if (!Utils.GetDomain().Contains(BCW.IPSPay.IPSPayMent.IPS_URL_CHN.Replace("http://", "")))
        {
            if (meid == 0)
                Utils.Login();
        }

        Master.Title = "支付订单";
        string addBzType = string.Empty;
        //if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
        addBzType = ub.Get("SiteBz");

        int KeepId = int.Parse(Utils.GetRequest("KeepId", "get", 2, @"^[0-9]\d*$", "订单错误"));
        string ok = (Utils.GetRequest("ok", "get", 3, "", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store&amp;backurl=" + Utils.getPage(0) + "") + "\">储物箱</a>&gt;支付订单");
        builder.Append(Out.Tab("</div>", "<br />"));
        string PostUrl = "http://" + Request.UrlReferrer.Authority;
        //if (!Utils.Isie() || Utils.GetUA().ToLower().Contains("opera"))
        //{
        //    Utils.Error("为了你的资金安全,本类别商品只支持IE9或以上浏览器<br />Opera或旧版浏览器将无法完成支付", Utils.getUrl("bbsshop.aspx?act=store"));
        //}

        if (KeepId != 0)
        {
            #region 获取订单信息
            BCW.Model.Shopkeep model = new BCW.BLL.Shopkeep().GetShopkeep(KeepId);
            if (model != null)
            {
                if (model.NodeId == 28 && model.State == 0)
                {
                    #region 处理订单
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("你正在支付订单" + model.MerBillNo + "<br />" + model.GoodsName + " " + model.Amount.ToString("0.00"));
                    builder.Append(Out.Tab("</div>", ""));
                    string strText = string.Empty;
                    string strName = string.Empty;
                    string strType = string.Empty;
                    string strValu = string.Empty;
                    string strEmpt = string.Empty;
                    string strIdea = string.Empty;
                    string strOthe = string.Empty;
                    if (ok != "sure")
                    {
                        string BankCodestr = "";
                        for (int i = 0; i < BCW.IPSPay.IPSPayMent.BankCodes.Length; i++)
                        {
                            if (BankCodestr != "") { BankCodestr += "|"; }
                            BankCodestr += BCW.IPSPay.IPSPayMent.BankCodes[i] + "|" + BCW.IPSPay.IPSPayMent.BankNames[i];
                        }

                        strText = "选择支付银行:/,,,,";
                        strName = "BankCode,GatewayType,verifyKey,act";
                        strType = "select,hidden,hidden,hidden";
                        strValu = "''''ipspaysave";
                        strEmpt = BankCodestr + ",01,false,false";
                        strOthe = "支付￥" + model.Amount.ToString("0.00") + "," + BCW.IPSPay.IPSPayMent.IPS_POST_URL + Utils.getUrl("bbsshop.aspx?act=MerBillPay&amp;ok=sure&amp;KeepId=" + KeepId) + ",post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        string reBankCode = Utils.GetRequest("BankCode", "post", 2, "", "银行码错误");
                        string GatewayType = "01";// Utils.GetRequest("GatewayType", "post", 3, "", "01");
                        string iSignature = "";
                        string bodystr = "";
                        //得到加密签名
                        //返回的地址
                        string usUrl = PostUrl + Utils.getUrl("bbs/bbsshop.aspx?act=store&amp;backurl=" + Utils.getPage(0) + "");
                        bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByShop(PostUrl, model, reBankCode, GatewayType, usUrl, ref iSignature);
                        string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost(KeepId.ToString(), DateTime.Now.ToString("yyyyMMddHHmmss"), iSignature, bodystr);
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append(BCW.IPSPay.IPSPayMent.GetBankNameByCode(reBankCode));
                        builder.Append(Out.Tab("</div>", ""));
                        strText = "";
                        strName = "pGateWayReq";
                        strType = "hidden";
                        strValu = Server.HtmlEncode(pGateWayReqstr);
                        strEmpt = "false";
                        strIdea = "";
                        strOthe = "确定支付￥" + model.Amount.ToString("0.00") + "," + BCW.IPSPay.IPSPayMent.IPS_URL_IPS + ",post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    #endregion
                }
                else
                {
                    Utils.Error("订单支付出错,标识:" + KeepId, "");
                }
            }
            else
            {
                Utils.Error("查无此订单", "");
            }
            #endregion
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Request.UrlReferrer.Authority + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的储物箱 StorePage
    /// <summary>
    /// 我的储物箱
    /// </summary>
    private void StorePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //将回礼的会员ID记录到Cookie
        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[1-9]\d*$", "0"));
        if (hid > 0)
        {
            HttpCookie Cookie = new HttpCookie("GiftComment");
            Cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(Cookie);

            HttpCookie cookie = new HttpCookie("GiftComment");
            cookie.Expires = DateTime.Now.AddMinutes(10);
            cookie.Values.Add("GiftUsId", "" + hid + "");
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        Master.Title = "我的储物箱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;储物箱");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = 5;
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsID=" + meid + " and Total>0";

        // 开始读取专题
        IList<BCW.Model.Shopkeep> listShopkeep = new BCW.BLL.Shopkeep().GetShopkeeps(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopkeep.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopkeep n in listShopkeep)
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
                builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftinfo&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "x" + n.Total + "</a>");
                if (n.NodeId == 28)
                {
                    builder.Append("<br />订单号:" + n.MerBillNo);
                    if (n.State == 0)
                    {
                        if (n.AddTime.AddHours(n.BillEXP) < DateTime.Now)
                        {
                            n.State = 2;
                            n.GatewayType = n.GatewayType;
                            n.Attach = n.Attach;
                            n.BankCode = n.BankCode;
                            n.ProductType = n.ProductType;
                            new BCW.BLL.Shopkeep().Update_ips(n);
                        }
                        //cn.com.ips.newpay.WSOrderQuery WSorder = new cn.com.ips.newpay.WSOrderQuery();
                        //string SignatureOrder = "", bodystr = "", Resultstr = ""; ;
                        //bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByChkOrderByShop(n, ref SignatureOrder);
                        //string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost_ByOrder(DateTime.Now.ToString("yyyyMMddHHmmss"), SignatureOrder, bodystr);
                        //Resultstr = WSorder.getOrderByMerBillNo(pGateWayReqstr);
                        //BCW.IPSPay.IPSPayMent.updateorder(Resultstr, false);
                        builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=MerBillPay&amp;KeepId=" + n.ID + "") + "\">[付款]</a>");
                    }
                    else
                    {
                        if (n.State == 1)
                        {
                            builder.Append("<br />[交易成功]");
                            string nBankName = BCW.IPSPay.IPSPayMent.GetBankNameByCode(n.BankCode);
                            builder.Append("(" + nBankName + ")");
                        }
                        else
                        {
                            builder.Append("<br />[交易超时]");
                        }
                    }
                    builder.Append(" " + n.AddTime.ToString());
                }
                if (n.NodeId == 29)
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=storemy&amp;ID=" + n.GiftId + "") + "\">[查看]</a>");
                }
                else
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/bbsshop.aspx?act=proxy&amp;id=" + n.ID + "") + "") + "\">[送礼]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 实物类商品 GoodsPage
    /// <summary>
    /// 实物类商品信息填写
    /// </summary>
    private void GoodsPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods(id);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (model.GiftId != 29)
        {
            Utils.Error("不存在的商品记录", "");
        }

        Master.Title = "礼物商城";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + model.ShopGiftId + "") + "\">" + model.Title + "</a>&gt;信息填写");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("<img src=\"" + model.PrevPic + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string Address = Utils.GetRequest("Address", "all", 1, @"^[^\^]{1,200}$", "地址填写出错");
        string Phone = Utils.GetRequest("Phone", "all", 1, @"^[^\^]{1,200}$", "号码填写错误");
        string Email = Utils.GetRequest("Email", "all", 1, @"^[^\^]{1,200}$", "邮箱未填写");
        string RealName = Utils.GetRequest("RealName", "all", 1, @"^[^\^]{1,200}$", "姓名填写错误");
        string Message = Utils.GetRequest("Message", "all", 1, @"^[^\^]{1,200}$", "留言未填写");
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (ptype == 3)
        {
            int KeepId = new BCW.BLL.Shopkeep().GetID(id, meid);
            //确认信息
            new BCW.Shop.BLL.Shopgoods().UpdateMessagebyid(id, RealName, Address, Phone, Email, Message);

            //发内线
            string strLog = "礼物商城会员(" + meid + ")购买实物商品购买单号为" + model.ID + "的" + model.Title + "已经确认收货信息，请前往【商城管理-商品派送】查看并派送..";
            new BCW.BLL.Guest().Add(0, 10086, new BCW.BLL.User().GetUsName(10086), strLog);

            Utils.Success("购买商品", "购买商品收货信息填写成功<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">&lt;&lt;我的储物箱</a>", Utils.getPage("bbsshop.aspx"), "5");
        }

        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请确认您的收货信息：<br />");
            builder.Append("收件姓名：<b style=\"color:red\">" + RealName + "</b><br />");
            builder.Append("收货地址：<b style=\"color:red\">" + Address + "</b><br />");
            builder.Append("联系方式：<b style=\"color:red\">" + Phone + "</b><br />");
            builder.Append("联系邮箱：<b style=\"color:red\">" + Email + "</b><br />");
            builder.Append("留言要求：<b style=\"color:red\">" + Message + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string strText = ",,,,,";
            string strName = "RealName,Address,Phone,Email,Message";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + RealName + "'" + Address + "'" + Phone + "'" + Email + "'" + Message + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,true,true";
            string strIdea = "/";
            string strOthe = "确认信息,bbsshop.aspx?act=goods&amp;id=" + id + "&amp;ptype=" + 3 + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=goods&amp;id=" + id + "&amp;ptype=" + 3 + "") + "\">确定信息</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=goods&amp;id=" + id + "&amp;ptype=" + 1 + "") + "\">返回修改</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("实物商品请填写收货信息以便商品派送<br />");
            builder.Append("==信息填写==");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "姓名:/,收货地址:/,联系方式:/,邮箱（选填）:/,留言（选填）:/,";
            string strName = "RealName,Address,Phone,Email,Message";
            string strType = "text,text,text,text,text";
            string strValu = "" + xml.dss["RealName"] + "'" + xml.dss["Address"] + "'" + xml.dss["Phone"] + "'" + xml.dss["Email"] + "'" + xml.dss["Message"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,true,true";
            string strIdea = "/";
            string strOthe = "确认信息,bbsshop.aspx?act=goods&amp;id=" + id + "&amp;ptype=" + 2 + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("注意：信息填写也可以到商城-储物箱找到已经购买的相应商品栏填写");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=gift&amp;id=" + model.GiftId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 实物类商品 GoodsaddPage
    /// <summary>
    /// 实物类商品信息补充填写
    /// </summary>
    private void GoodsaddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods(id);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (model.GiftId != 29)
        {
            Utils.Error("不存在的商品记录", "");
        }

        Master.Title = "礼物商城";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + "") + "\">" + model.Title + "</a>&gt;信息填写");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("<img src=\"" + model.PrevPic + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string Address = Utils.GetRequest("Address", "all", 1, @"^[^\^]{1,200}$", "地址填写出错");
        string Phone = Utils.GetRequest("Phone", "all", 1, @"^[^\^]{1,200}$", "号码填写错误");
        string Email = Utils.GetRequest("Email", "all", 1, @"^[^\^]{1,200}$", "邮箱未填写");
        string RealName = Utils.GetRequest("RealName", "all", 1, @"^[^\^]{1,200}$", "姓名填写错误");
        string Message = Utils.GetRequest("Message", "all", 1, @"^[^\^]{1,200}$", "留言未填写");
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (ptype == 3)
        {
            int KeepId = new BCW.BLL.Shopkeep().GetID(id, meid);
            //确认信息
            new BCW.Shop.BLL.Shopgoods().UpdateMessagebyid(id, RealName, Address, Phone, Email, Message);

            //发内线
            string strLog = "礼物商城会员(" + meid + ")购买实物商品购买单号为" + model.ID + "的" + model.Title + "已经确认收货信息，请前往【商城管理-商品派送】查看并派送..";
            new BCW.BLL.Guest().Add(0, 10086, new BCW.BLL.User().GetUsName(10086), strLog);

            Utils.Success("信息填写", "商品收货信息填写成功<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + "") + "\">&lt;&lt;我的储物箱</a>", Utils.getUrl("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + ""), "2");
        }

        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请确认您的收货信息：<br />");
            builder.Append("收件姓名：<b style=\"color:red\">" + RealName + "</b><br />");
            builder.Append("收货地址：<b style=\"color:red\">" + Address + "</b><br />");
            builder.Append("联系方式：<b style=\"color:red\">" + Phone + "</b><br />");
            builder.Append("联系邮箱：<b style=\"color:red\">" + Email + "</b><br />");
            builder.Append("留言要求：<b style=\"color:red\">" + Message + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string strText = ",,,,,";
            string strName = "RealName,Address,Phone,Email,Message";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + RealName + "'" + Address + "'" + Phone + "'" + Email + "'" + Message + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,true,true";
            string strIdea = "/";
            string strOthe = "确认信息,bbsshop.aspx?act=goodsadd&amp;id=" + id + "&amp;ptype=" + 3 + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=goods&amp;id=" + id + "&amp;ptype=" + 3 + "") + "\">确定信息</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=goodsadd&amp;id=" + id + "&amp;ptype=" + 1 + "") + "\">返回修改</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string strText = "姓名:/,收货地址:/,联系方式:/,邮箱（选填）:/,留言（选填）:/,";
            string strName = "RealName,Address,Phone,Email,Message";
            string strType = "text,text,text,text,text";
            string strValu = "" + xml.dss["RealName"] + "'" + xml.dss["Address"] + "'" + xml.dss["Phone"] + "'" + xml.dss["Email"] + "'" + xml.dss["Message"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,true,true";
            string strIdea = "/";
            string strOthe = "确认信息,bbsshop.aspx?act=goodsadd&amp;id=" + id + "&amp;ptype=" + 2 + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的储物箱实物 StoreMyPage
    /// <summary>
    /// 我的储物箱
    /// </summary>
    private void StoreMyPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods1(ID);
        if (!new BCW.Shop.BLL.Shopgoods().Exists(1))
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (!new BCW.Shop.BLL.Shopgoods().Existsgd(ID, meid))
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (!new BCW.BLL.Shopkeep().Exists(ID, meid))
        {
            Utils.Error("不存在的商品记录", "");
        }
        Master.Title = "我的储物箱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">储物箱</a>&gt;" + model.Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = 5;
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsID=" + meid + " and ShopGiftId =" + ID + " ";
        strOrder = "ID Desc";

        // 开始读取专题
        IList<BCW.Shop.Model.Shopgoods> listShopkeep = new BCW.Shop.BLL.Shopgoods().GetShopgoodss(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listShopkeep.Count > 0)
        {
            int k = 1;
            foreach (BCW.Shop.Model.Shopgoods n in listShopkeep)
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
                builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/>");
                builder.Append("<br />" + "购买单号：" + n.ID + "（" + n.Title + "x" + n.Num + "）");

                if (n.RealName == "" || n.Phone == "" || n.Address == "")//为空填写信息
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=goodsadd&amp;ID=" + n.ID + "") + "\">[填写信息]</a>");
                }
                else
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=goodsview&amp;ID=" + n.ID + "") + "\">[查看详情]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的实物信息 GoodsviewPage
    /// <summary>
    /// 我的实物信息
    /// </summary>
    private void GoodsviewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //  int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[1-9]\d*$", "0"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.Shop.BLL.Shopgoods().Exists(meid, ID))
        {
            Utils.Error("不存在的商品记录", "");
        }
        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods(ID);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (model.GiftId != 29)
        {
            Utils.Error("不存在的商品记录", "");
        }
        Master.Title = "我的储物箱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">储物箱</a>&gt;" + model.Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("==商品详情==");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("<img src=\"" + model.PrevPic + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("商品 ID ：" + model.ShopGiftId + "<br />");
        builder.Append("商品名称：" + model.Title + "<br />");
        builder.Append("商品类型：" + new BCW.BLL.Shoplist().GetTitle(model.GiftId) + "<br />");
        builder.Append("购买单号：" + model.ID + "<br />");
        builder.Append("购买数量：" + model.Num + "<br />");
        builder.Append("购买时间：" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append("收件姓名：" + model.RealName + "<br />");
        builder.Append("收件地址：" + model.Address + "<br />");
        builder.Append("联系方式：" + model.Phone + "<br />");
        if (model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
        {
            if (model.SendTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
            {
                builder.Append("商品状态：商品正在准备中...");
            }
            else
            {
                builder.Append("商品状态：商品已发货<br />");
                builder.Append("发货时间：" + Convert.ToDateTime(model.SendTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                builder.Append("快递公司：" + model.Express + "<br />");
                builder.Append("快递单号：" + model.Expressnum + "<br />");

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("【信息录入】<br />");
                builder.Append("若确定收到商品，请确认收货，谢谢");
                builder.Append(Out.Tab("</div>", "<br />"));

                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                if (Utils.ToSChinese(ac) == "确定收货")
                {

                    try
                    {
                        new BCW.Shop.BLL.Shopgoods().UpdateReceivebyID(model.ID, DateTime.Now);
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("确认收货成功!" + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><br />" + model.Title + "</a>");
                        builder.Append(Out.Tab("</div>", ""));

                    }
                    catch
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("修改失败!" + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=goodsview&amp;ID=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                        builder.Append(Out.Tab("</div>", "<br/>"));
                    }

                }
                else
                {
                    builder.Append(Out.Tab("</div>", ""));
                    string Text = ",";
                    string Name = "hidden";
                    string Type = "hidden";
                    string Valu = "" + "" + "'" + Utils.getPage(0) + "";
                    string Empt = "1|派送中|2|已送达";
                    string Idea = "/";
                    string Othe = "确定收货,bbsshop.aspx?act=goodsview&amp;ID=" + ID + "&amp;,post,1,red";
                    builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));

                }
            }
        }
        else
        {
            builder.Append("商品状态：商品已送达<br />");
            builder.Append("快递公司：" + model.Express + "<br />");
            builder.Append("快递单号：" + model.Expressnum + "<br />");
            builder.Append("发货时间：" + Convert.ToDateTime(model.SendTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
            builder.Append("送达时间：" + Convert.ToDateTime(model.ReceiveTime).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    private void GiftInfoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Shopkeep model = new BCW.BLL.Shopkeep().GetShopkeep(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (!new BCW.BLL.Shopkeep().Exists1(id, meid))
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看储物箱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">储物箱</a>&gt;" + model.Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img src=\"" + model.Pic + "\" alt=\"load\"/><br />");
        builder.Append(model.Notes);
        if (model.IsSex == 1)
            builder.Append("<br />本礼物只能送给女生哦!");
        else if (model.IsSex == 2)
            builder.Append("<br />本礼物只能送给男生哦!");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("" + OutMei(model.Para));
        if (model.Total != -1)
        {
            builder.Append("<br />库存" + model.Total + "个");
        }
        if (model.NodeId != 28)
        {
            if (model.NodeId == 29)
            {
                builder.Append("<br />此商品为实物，不支持送礼，敬请谅解");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/bbsshop.aspx?act=proxy&amp;id=" + id + "") + "") + "\">[送礼]</a>");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=store") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ProxyPage()
    {
        Master.Title = "送礼给好友";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[1-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Shopkeep model = new BCW.BLL.Shopkeep().GetShopkeep(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("送礼给好友");
        builder.Append(Out.Tab("</div>", ""));
        if (hid == 0)
        {
            builder.Append(Out.Tab("", "<br />"));
            string strText = "输入ID:(限一个):/,,";
            string strName = "hid,id,act";
            string strType = "snum,hidden,hidden";
            string strValu = "'" + id + "'proxy";
            if (HttpContext.Current.Request.Cookies["GiftComment"] != null)
            {
                if (HttpContext.Current.Request.Cookies["GiftComment"]["GiftUsId"] != null)
                {
                    strValu = HttpContext.Current.Request.Cookies["GiftComment"]["GiftUsId"].ToString() + "'" + id + "'proxy";
                }
            }
            string strEmpt = "true,false,false";
            string strIdea = "/";
            string strOthe = "提交,bbsshop.aspx,get,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("你的储物箱有" + model.Total + "个<img src=\"" + model.PrevPic + "\" alt=\"load\"/><a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + model.GiftId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "准备送,,,";
            string strName = "num,hid,id,act";
            string strType = "snum,hidden,hidden,hidden";
            string strValu = "'" + hid + "'" + id + "'proxyok";
            string strEmpt = "true,false,false,false";
            string strIdea = "个'''|/";
            string strOthe = "下一步,bbsshop.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=store") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ProxyOkPage()
    {
        Master.Title = "送礼给好友";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int num = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "赠送数量填写错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Shopkeep model = new BCW.BLL.Shopkeep().GetShopkeep(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员", "");
        }
        if (model.NodeId == 28)
        {
            Utils.Error("抱歉,此物品不能赠送他人", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,100}$", "赠言限100字内");
            if (model.Total < num)
            {
                Utils.Error("库存只有" + model.Total + "个哦", "");
            }
            //检测男生女生
            if (model.IsSex == 1)
            {
                if (new BCW.BLL.User().GetSex(hid) != 1)
                {
                    Utils.Error("本礼物只能送给女生哦!", "");
                }
            }
            else if (model.IsSex == 2)
            {
                if (new BCW.BLL.User().GetSex(hid) != 2)
                {
                    Utils.Error("本礼物只能送给男生哦!", "");
                }
            }
            string mename = new BCW.BLL.User().GetUsName(meid);
            string toname = new BCW.BLL.User().GetUsName(hid);
            //送礼记录
            BCW.Model.Shopsend send = new BCW.Model.Shopsend();
            send.Title = model.Title;
            send.GiftId = model.GiftId;
            send.PrevPic = model.PrevPic;
            send.Message = Content;
            send.UsID = meid;
            send.UsName = mename;
            send.ToID = hid;
            send.ToName = toname;
            send.Total = num;
            send.AddTime = DateTime.Now;
            send.PIC = "0";//邵广林 20160607 增加与农场的标识
            new BCW.BLL.Shopsend().Add(send);
            //主人礼物记录
            BCW.Model.Shopuser user = new BCW.Model.Shopuser();
            user.GiftId = model.GiftId;
            user.UsID = hid;
            user.UsName = toname;
            user.PrevPic = model.PrevPic;
            user.GiftTitle = model.Title;
            user.Total = num;
            user.AddTime = DateTime.Now;
            user.PIC = "0";//邵广林 20160607 增加与农场的标识
            if (!new BCW.BLL.Shopuser().Exists(hid, model.GiftId))
                new BCW.BLL.Shopuser().Add(user);
            else
                new BCW.BLL.Shopuser().Update(user);

            //减库存或删除
            if (model.Total == num)
            {
                if (model.AddTime < DateTime.Now.AddDays(-7))//超7天外则删除
                    new BCW.BLL.Shopkeep().Delete(id);
                else
                    new BCW.BLL.Shopkeep().UpdateTotal(id, -num);

            }
            else
                new BCW.BLL.Shopkeep().UpdateTotal(id, -num);

            //奖魅力等属性
            string[] temp = model.Para.Split("|".ToCharArray());
            long Score = Convert.ToInt64(Utils.ParseInt(temp[0]));
            int Tl = Utils.ParseInt(temp[1]);
            int Ml = Utils.ParseInt(temp[2]);
            int Zh = Utils.ParseInt(temp[3]);
            int Ww = Utils.ParseInt(temp[4]);
            int Xe = Utils.ParseInt(temp[5]);

            if (Score != 0)
                new BCW.BLL.User().UpdateiScore(hid, Score);

            string sParas = new BCW.BLL.User().GetParas(hid);
            //计算属性与值
            if (Tl != 0)
                sParas = BCW.User.Users.GetParaData(sParas, Tl, 0);
            if (Ml != 0)
                sParas = BCW.User.Users.GetParaData(sParas, Ml, 1);
            if (Zh != 0)
                sParas = BCW.User.Users.GetParaData(sParas, Zh, 2);
            if (Ww != 0)
                sParas = BCW.User.Users.GetParaData(sParas, Ww, 3);
            if (Xe != 0)
                sParas = BCW.User.Users.GetParaData(sParas, Xe, 4);
            //更新属性值
            new BCW.BLL.User().UpdateParas(hid, sParas);
            //内线给对方
            new BCW.BLL.Guest().Add(0, hid, toname, "恭喜！[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]赠送" + num + "个[url=/bbs/bbsshop.aspx?act=mygift]" + model.Title + "[/url]，你的" + OutMei(model.Para) + "");
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]赠送" + num + "个[url=/bbs/bbsshop.aspx?act=giftview&amp;id=" + model.GiftId + "]" + model.Title + "[img]" + model.PrevPic + "[/img][/url]给[url=/bbs/uinfo.aspx?uid=" + hid + "]" + toname + "[/url]";
            new BCW.BLL.Action().Add(12, id, meid, mename, wText);
            Utils.Success("赠送礼物", "赠送礼物成功..", Utils.getUrl("bbsshop.aspx?act=store"), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("送礼给好友");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("送" + num + "个<img src=\"" + model.PrevPic + "\" alt=\"load\"/>" + model.Title + "给<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "送礼赠言:(100字以内|会在对方空间中显示哦!)/,,,,,";
            string strName = "Content,num,hid,id,act,info";
            string strType = "text,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + num + "'" + hid + "'" + id + "'proxyok'ok";
            string strEmpt = "true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定赠送,bbsshop.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=store") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void SendPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的送礼记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;送礼记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = 5;
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsID=" + meid + "";

        // 开始读取专题
        IList<BCW.Model.Shopsend> listShopsend = new BCW.BLL.Shopsend().GetShopsends(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopsend.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopsend n in listShopsend)
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
                builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + n.GiftId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
                builder.Append("送给<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a> ");
                builder.Append(DT.FormatDate(n.AddTime, 5));
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        Master.Title = "七日排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;七日排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("热销排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=top&amp;ptype=0") + "\">热销排行</a>|");

        if (ptype == 1)
            builder.Append("血拼狂人");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=top&amp;ptype=1") + "\">血拼狂人</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = 5;
        string[] pageValUrl = { "act", "ptype" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 0)
        {
            strWhere = "AddTime>='" + DateTime.Now.AddDays(-7) + "' AND (NodeId <> 28) ";
            // 开始读取专题
            IList<BCW.Model.Shopkeep> listShopkeep = new BCW.BLL.Shopkeep().GetShopkeepsTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listShopkeep.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Shopkeep n in listShopkeep)
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
                    builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/>");
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + n.GiftId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
                    builder.Append("<br />本周已售" + n.TopTotal + "个");
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
        }
        else
        {
            pageSize = 10;
            strWhere = "AddTime>='" + DateTime.Now.AddDays(-7) + "' AND (NodeId <> 28) ";
            // 开始读取专题
            IList<BCW.Model.Shopkeep> listShopkeep = new BCW.BLL.Shopkeep().GetShopkeepsTop2(pageIndex, pageSize, strWhere, out recordCount);
            if (listShopkeep.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Shopkeep n in listShopkeep)
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
                    builder.Append((pageIndex - 1) * pageSize + k);
                    builder.Append(".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                    builder.Append("购物" + n.TopTotal + "件");
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

        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=topuser&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;礼物达人榜</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopUserPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        Master.Title = "礼物达人榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;礼物达人榜");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取专题
        IList<BCW.Model.Shopuser> listShopuser = new BCW.BLL.Shopuser().GetShopusersTop(pageIndex, pageSize, out recordCount);
        if (listShopuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopuser n in listShopuser)
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
                builder.Append((pageIndex - 1) * pageSize + k);
                builder.Append(".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>");
                builder.Append("(收礼" + n.Total + "份)");
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

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=top&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;七天排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SearchPage()
    {
        string keyword = Utils.GetRequest("keyword", "all", 3, @"^[\s\S]{1,10}$", "搜索关键字限10字内");
        Master.Title = "商品搜索";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;搜索结果");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (keyword != "")
        {
            int pageIndex;
            int recordCount;
            string strWhere = string.Empty;
            string strOrder = string.Empty;
            int pageSize = 5;
            string[] pageValUrl = { "act", "keyword" };
            pageIndex = Utils.ParseInt(Request["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "Title LIKE '%" + keyword + "%'";
            strOrder = "ID Desc";
            // 开始读取专题
            IList<BCW.Model.Shopgift> listShopgift = new BCW.BLL.Shopgift().GetShopgifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listShopgift.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Shopgift n in listShopgift)
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
                    builder.Append("<img src=\"" + n.Pic + "\" alt=\"load\"/>");
                    builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftsell&amp;id=" + n.ID + "") + "\">[购买]</a>");
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
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("输入搜索关键字:");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",,,,";
        string strName = "keyword,act,backurl";
        string strType = "text,hidden,hidden";
        string strValu = "'search'" + Utils.PostPage(1) + "";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "搜索商品,bbsshop.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyGiftPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "0"));
        if (hid == 0)
            hid = meid;

        int type = int.Parse(Utils.GetRequest("type", "get", 1, @"^[0-1]\d*$", "0"));//邵广林 20160607 type为0是系统原有1为农场

        Master.Title = "收到的礼物";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>&gt;收到的礼物");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("+收到礼物");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = 20;
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (type == 0)
        {
            strWhere = "UsID=" + hid + " and pic=0";//邵广林 20160607 增加收到礼物的判断
        }
        else
        {
            strWhere = "UsID=" + hid + " and pic=1";
        }

        // 开始读取专题
        IList<BCW.Model.Shopuser> listShopuser = new BCW.BLL.Shopuser().GetShopusers(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopuser.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            int k = 0;
            foreach (BCW.Model.Shopuser n in listShopuser)
            {
                if (k % 4 == 0 && k > 0)
                    builder.Append("<br />");
                else
                    builder.Append(" ");
                if (type == 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftmenu&amp;giftid=" + n.GiftId + "&amp;hid=" + hid + "&amp;type=0&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + n.PrevPic + "\" alt=\"" + n.GiftTitle + "\"/>x" + n.Total + "</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftmenu&amp;giftid=" + n.GiftId + "&amp;hid=" + hid + "&amp;type=1&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + n.PrevPic + "\" alt=\"" + n.GiftTitle + "\"/>x" + n.Total + "</a>");
                }
                k++;

            }
            builder.Append(Out.Tab("</div>", ""));
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("+送礼清单");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // 开始读取清单列表
        int SizeNum = 3;
        int ab = 0;//邵广林 20160607 增加送礼清单的判断
        if (type == 0)
        {
            ab = 0;
        }
        else
        {
            ab = 1;
        }
        IList<BCW.Model.Shopsend> listShopsend = new BCW.BLL.Shopsend().GetShopsends(SizeNum, "ToID=" + hid + " and pic=" + ab + "");
        if (listShopsend.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopsend n in listShopsend)
            {

                builder.Append("" + k + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                builder.Append("送" + n.Title + "" + n.Total + "个 " + DT.FormatDate(n.AddTime, 1) + "");
                if (type == 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store&amp;hid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;回礼</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=3") + "\">&gt;&gt;回礼</a><br />");
                }
                if (!string.IsNullOrEmpty(n.Message))
                    builder.Append("赠言:" + n.Message + "<br />");
                k++;
            }
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=mylist&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部清单&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=topuser&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;礼物达人榜</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "0"));
        if (hid == 0)
            hid = meid;

        Master.Title = "送礼清单";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=mygift&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">收到的礼物</a>&gt;送礼清单");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("+送礼清单");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "ToID=" + hid + "";
        // 开始读取专题
        IList<BCW.Model.Shopsend> listShopsend = new BCW.BLL.Shopsend().GetShopsends(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopsend.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopsend n in listShopsend)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                builder.Append("送" + n.Title + "" + n.Total + "个 " + DT.FormatDate(n.AddTime, 1) + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store&amp;hid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;回礼</a>");
                if (!string.IsNullOrEmpty(n.Message))
                    builder.Append("<br />赠言:" + n.Message + "");

                builder.Append(Out.Tab("</div>", ""));

                k++;

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=mygift&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GiftMenuPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "0"));
        int giftid = int.Parse(Utils.GetRequest("giftid", "get", 1, @"^[1-9]\d*$", "礼物ID错误"));
        if (hid == 0)
            hid = meid;

        int type = int.Parse(Utils.GetRequest("type", "get", 1, @"^[0-1]\d*$", "0"));//邵广林 20160607 type为0是系统原有1为农场

        Master.Title = "礼物清单";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=mygift&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">收到的礼物</a>&gt;礼物清单");
        builder.Append(Out.Tab("</div>", ""));

        BCW.Model.Shopuser model = new BCW.BLL.Shopuser().GetShopuser(hid, giftid);
        if (model == null)
        {
            Utils.Error("不存在的礼物记录", "");
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (type == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + giftid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + model.PrevPic + "\" alt=\"" + model.GiftTitle + "\"/>" + model.GiftTitle + "</a>x" + model.Total + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftview&amp;id=" + giftid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[送Ta" + model.GiftTitle + "]</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?act=buycase&amp;ptype=1&amp;id=" + giftid + "") + "\"><img src=\"" + model.PrevPic + "\" alt=\"" + model.GiftTitle + "\"/>" + model.GiftTitle + "</a>x" + model.Total + "");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?act=buycase&amp;ptype=1&amp;id=" + giftid + "") + "\">[送Ta" + model.GiftTitle + "]</a>");
        }
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("+收到礼物");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "hid", "giftid", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "ToID=" + hid + " and GiftId=" + giftid + " ";
        // 开始读取专题
        IList<BCW.Model.Shopsend> listShopsend = new BCW.BLL.Shopsend().GetShopsends(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopsend.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopsend n in listShopsend)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                builder.Append("送" + n.Title + "" + n.Total + "个 " + DT.FormatDate(n.AddTime, 1) + "");
                if (!string.IsNullOrEmpty(n.Message))
                    builder.Append("<br />赠言:" + n.Message + "");

                builder.Append(Out.Tab("</div>", ""));

                k++;

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=mygift&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MedalPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "勋章列表";
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        if (id > 0)
        {

            BCW.Model.Medal model = new BCW.BLL.Medal().GetMedal(id);
            if (model == null)
            {
                Utils.Error("不存在的勋章记录", "");
            }
            if (model.Types != 1)
            {
                Utils.Error("不存在的勋章记录", "");
            }
            if (new BCW.BLL.Medal().Exists(id, meid))
            {
                Utils.Error("重复购买", "");
            }
            if (model.iCount <= 0)
            {
                Utils.Error("库存不足了", "");
            }
            if (info == "ok")
            {
                long gold = new BCW.BLL.User().GetGold(meid);
                if (gold < Convert.ToInt64(model.iCent))
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                //支付安全提示
                string[] p_pageArr = { "act", "info", "id" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

                string PayID = model.PayID + "#" + meid + "#";
                string ExTime = DT.FormatDate(DateTime.Now.AddDays(model.iDay), 11);
                string PayExTime = model.PayExTime + "#" + ExTime + "#";
                new BCW.BLL.Medal().UpdatePayID(id, PayID, PayExTime);
                //扣除库存
                new BCW.BLL.Medal().UpdateiCount(id, -1);
                //清缓存
                string CacheKey = CacheName.App_UserMedal(meid);
                DataCache.RemoveByPattern(CacheKey);
                //操作币
                new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(model.iCent), "购买勋章");
                //记录动态
                string mename = new BCW.BLL.User().GetUsName(meid);
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]购买了勋章[url=/bbs/bbsshop.aspx?act=medalview&amp;id=" + id + "]" + model.Title + "[IMG]" + model.ImageUrl + "[/IMG][/url]";
                new BCW.BLL.Action().Add(12, id, meid, mename, wText);

                Utils.Success("购买勋章", "恭喜，购买勋章成功，正在返回..", Utils.getUrl("bbsshop.aspx?act=medal&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("购买此勋章需花费" + model.iCent + "" + ub.Get("SiteBz") + "");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medal&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定购买</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medal&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("勋章列表");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = 4;
            string strWhere = "";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "Types=1";

            // 开始读取列表
            IList<BCW.Model.Medal> listMedal = new BCW.BLL.Medal().GetMedals(pageIndex, pageSize, strWhere, out recordCount);
            if (listMedal.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Medal n in listMedal)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    builder.AppendFormat("{0}<img src=\"{1}\" alt=\"load\"/>", n.Title, n.ImageUrl);
                    builder.AppendFormat("<br />{0}", n.Notes);
                    builder.AppendFormat("<br />库存剩:{0}件", n.iCount);
                    builder.AppendFormat("<br />有效期:{0}天", n.iDay);
                    builder.AppendFormat("<br />价格:￥{0}{1} ", n.iCent, ub.Get("SiteBz"));
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medal&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">购买</a>");
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
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void MedalLogPage()
    {
        Master.Title = "获授勋章日志";
        int meid = new BCW.User.Users().GetUsId();
        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (hid > 0)
            builder.Append("" + new BCW.BLL.User().GetUsName(hid) + "的荣誉记录");
        else
            builder.Append("所有荣誉记录");

        builder.Append(Out.Tab("</div>", "<br />"));
        bool IsAdmin = false;
        string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
        if (("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
        {
            IsAdmin = true;
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=0";
        if (hid > 0)
            strWhere += " and UsID=" + hid + "";

        // 开始读取列表
        IList<BCW.Model.Medalget> listMedalget = new BCW.BLL.Medalget().GetMedalgets(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedalget.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medalget n in listMedalget)
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
                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "]" + DT.FormatDate(n.AddTime, 11) + " ");
                if (hid == 0)
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>");

                builder.Append("获授<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=medalview&amp;id=" + n.MedalId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.SysUBB(n.Notes) + "</a>");

                if (IsAdmin == true)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=delmedallog&amp;id=" + n.ID + "&amp;hid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (hid > 0)
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medallog") + "\">查看更多荣誉&gt;&gt;</a><br />获授的勋章记录越多，将有希望获得站方年终嘉奖！<br />");

        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medaltop") + "\">荣誉勋章排行榜&gt;&gt;</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medal") + "\">购买个性勋章&gt;&gt;</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelMedalLogPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除勋章日志";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此勋章日志记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?info=ok&amp;act=delmedallog&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=medallog&amp;hid=" + hid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int meid = new BCW.User.Users().GetUsId();
            string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
            if (!("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你的权限不足", "");
            }

            if (!new BCW.BLL.Medalget().Exists(hid, id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Medalget().Delete(hid, id);
            Utils.Success("删除勋章日志", "删除勋章日志成功..", Utils.getPage("medal.aspx?act=medallog&amp;hid=" + hid + ""), "1");
        }
    }

    private void MedalViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Medal model = new BCW.BLL.Medal().GetMedal(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看勋章说明";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + model.Title + "<img src=\"" + model.ImageUrl + "\" alt=\"load\"/><br />");
        builder.Append(model.Notes);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=medal") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MeMedalPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
        if (!("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "管理会员勋章";

        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + new BCW.BLL.User().GetUsName(hid) + "的勋章");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "hid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "PayID LIKE '%#" + hid + "#%' and Types>=0";

        // 开始读取列表
        IList<BCW.Model.Medal> listMedal = new BCW.BLL.Medal().GetMedals(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedal.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medal n in listMedal)
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
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=delmedal&amp;id=" + n.ID + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤]</a>" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.AppendFormat("{0}<img src=\"{1}\" alt=\"load\"/>", n.Title, n.ImageUrl);
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
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medalset&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">授予勋章&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + hid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelMedalPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
        if (!("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (info == "")
        {
            Master.Title = "撤销记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定撤销此记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?info=ok&amp;act=delmedal&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤销</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=memedal&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Medal().Exists(id, hid))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.Model.Medal model = new BCW.BLL.Medal().GetMedalMe(id);
            string sPayID = string.Empty;
            sPayID = Utils.Mid(model.PayID, 1, model.PayID.Length);
            sPayID = Utils.DelLastChar(sPayID, "#");
            string[] temp = Regex.Split(sPayID, "##");
            //得到位置
            int strPn = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].ToString() == hid.ToString())
                {
                    strPn = i;
                    break;
                }
            }
            string sPayExTime = string.Empty;
            sPayExTime = Utils.Mid(model.PayExTime, 1, model.PayExTime.Length);
            sPayExTime = Utils.DelLastChar(sPayExTime, "#");
            string[] temp2 = Regex.Split(sPayExTime, "##");
            string PayExTime = string.Empty;
            for (int i = 0; i < temp2.Length; i++)
            {
                if (i != strPn)
                {
                    PayExTime += "#" + temp2[i] + "#";
                }

            }
            string PayID = model.PayID.Replace("#" + hid + "#", "");
            //更新
            new BCW.BLL.Medal().UpdatePayID(id, PayID, PayExTime);
            //清缓存
            string CacheKey = CacheName.App_UserMedal(hid);
            DataCache.RemoveByPattern(CacheKey);
            Utils.Success("撤销", "撤销成功..", Utils.getPage("bbsshop.aspx?act=memedal&amp;hid=" + hid + ""), "1");
        }
    }

    private void MedalSetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
        if (!("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }

        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        Master.Title = "授予勋章";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("授予勋章");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=0 and ID>22";

        // 开始读取列表
        IList<BCW.Model.Medal> listMedal = new BCW.BLL.Medal().GetMedals(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedal.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medal n in listMedal)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.AppendFormat("{0}<img src=\"{1}\" alt=\"load\"/>", n.Title, n.ImageUrl);
                builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=grant&amp;id=" + n.ID + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[选择]</a>");

                builder.AppendFormat("<br />{0}", n.Notes);
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + hid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GrantPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
        if (!("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }

        Master.Title = "授予勋章";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "勋章ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        string ImageUrl = new BCW.BLL.Medal().GetImageUrl(id);
        if (ImageUrl == "")
        {
            Utils.Error("不存在的勋章记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("授予《" + new BCW.BLL.User().GetUsName(hid) + "》<img src=\"" + ImageUrl + "\" alt=\"load\"/>勋章");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",勋章有效天数(填0则无限):/,,";
        string strName = "ID,iDay,hid,act";
        string strType = "hidden,num,hidden,hidden";
        string strValu = "" + id + "'0'" + hid + "'grantsave";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定授予,bbsshop.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=medal") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GrantSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
        if (!("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }

        int ID = int.Parse(Utils.GetRequest("ID", "post", 2, @"^[1-9]\d*$", "勋章ID填写错误"));
        int iDay = int.Parse(Utils.GetRequest("iDay", "post", 2, @"^[0-9]\d*$", "勋章有效天数填写错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.Medal().Exists(ID))
        {
            Utils.Error("不存在的勋章记录", "");
        }
        if (new BCW.BLL.Medal().Exists(ID, hid))
        {
            Utils.Error("此会员已存在这个勋章，如要修改期限请先撤销再授予", "");
        }

        BCW.Model.Medal model = new BCW.BLL.Medal().GetMedal(ID);
        string PayID = model.PayID + "#" + hid + "#";

        string ExTime = string.Empty;
        if (iDay == 0)
            ExTime = "1990-1-1";
        else
            ExTime = DT.FormatDate(DateTime.Now.AddDays(iDay), 11);

        string PayExTime = model.PayExTime + "#" + ExTime + "#";
        new BCW.BLL.Medal().UpdatePayID(ID, PayID, PayExTime);
        //清缓存
        string CacheKey = CacheName.App_UserMedal(hid);
        DataCache.RemoveByPattern(CacheKey);

        //记录获授勋章日志
        string DayTime = "" + iDay + "天";
        if (iDay == 0)
            DayTime = "永久";

        BCW.Model.Medalget m = new BCW.Model.Medalget();
        m.Types = 0;
        m.UsID = hid;
        m.MedalId = ID;
        m.Notes = "" + model.Title + "[IMG]" + model.ImageUrl + "[/IMG][有效期" + DayTime + "]";
        m.AddTime = DateTime.Now;
        new BCW.BLL.Medalget().Add(m);
        //记录动态
        string UsName = new BCW.BLL.User().GetUsName(hid);
        string wText = "[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]获授勋章[url=/bbs/bbsshop.aspx?act=medalview&amp;id=" + ID + "]" + model.Title + "[IMG]" + model.ImageUrl + "[/IMG][/url],恭喜恭喜!";
        new BCW.BLL.Action().Add(12, ID, 0, "", wText);
        //内线
        new BCW.BLL.Guest().Add(0, hid, UsName, wText.Replace("[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]", "您"));

        Utils.Success("授予勋章", "授予勋章并记录授日志成功..", Utils.getPage("bbsshop.aspx?act=medalset&amp;hid=" + hid + ""), "1");
    }

    private string OutMei(string Para)
    {
        //属性参数（积分|体力|魅力|智慧|威望|邪恶)写入如:0|0|0|0|0|0）
        string str = string.Empty;
        if (Para != "")
        {
            try
            {
                string[] name = { "积分", "体力", "魅力", "智慧", "威望", "邪恶" };
                string[] temp = Para.Split("|".ToCharArray());
                for (int i = 0; i < temp.Length; i++)
                {
                    int ii = Convert.ToInt32(temp[i]);
                    if (ii != 0)
                    {
                        if (ii > 0)
                            str += "," + name[i] + "+" + temp[i];
                        else
                            str += "," + name[i] + "" + temp[i];
                    }
                }
            }
            catch
            {
                str = ",参数错误";
            }
        }
        if (!string.IsNullOrEmpty(str))
            str = Utils.Mid(str, 1, str.Length);

        return str;
    }

    /// <summary>
    /// 陈志基 16/5/12
    /// 修改排行榜
    /// </summary>

    private void MedalTopPage()
    {
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));

        Master.Title = "荣誉勋章排行榜";
        int nowyear = DateTime.Now.Year;
        int lastyear = nowyear - 1;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (showtype == 0)
            builder.Append(nowyear + "年荣誉勋章排行榜");
        else
            builder.Append(lastyear + "年荣誉勋章排行榜");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        if (showtype == 0)
            strWhere = "AddTime>'" + nowyear + "-1-1' AND AddTime<'" + DateTime.Now + "'";
        else
            strWhere = "AddTime>'" + lastyear + "-1-1' AND AddTime<'" + nowyear + "-1-1'";

        string[] pageValUrl = { "act", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Medalget> listMedalget = new BCW.BLL.Medalget().GetMedalgetsTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedalget.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medalget n in listMedalget)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>:获授" + n.Types + "枚勋章");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        if (showtype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medaltop&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + lastyear + "年排行榜</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medaltop&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">" + nowyear + "年排行榜</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medallog&amp;backurl=" + Utils.getPage(0) + "") + "\">所有荣誉记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
}
