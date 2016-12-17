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
using BCW.Files;
/// <summary>
/// 05031_last版本（完善快递流程版本）
/// 20160611_修复实物添加图片与实物过期处理
/// 20160613_修复玩家出售获得比数
/// 20160622_控制更新
/// </summary>
public partial class Manage_game_kbyg : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/myyg.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected int GameId = 1007;
    protected int statue = Convert.ToInt32(ub.GetSub("KbygStatus", "/Controls/myyg.xml"));
    protected string Gamename = Convert.ToString(ub.GetSub("KbygName", "/Controls/myyg.xml"));
    protected void Page_Load(object sender, EventArgs e)
    {
        // isTimeToOpen();//设置开奖状态,设置倒计时
        //  isTimeToOpenNew();//完全开奖，倒计时完毕，更改标识
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "buylist":
                BuyLists();//购买记录
                break;
            case "open"://开奖商品
                OpenList();
                break;
            case "unopen"://查询未开奖商品
                UnOpenList();
                break;
            case "buyOne":
                buyOneGoods();
                break;
            case "seeYun"://查看云购码
                SeeYungoum();
                break;
            case "del"://删除
                DelPage();
                break;
            case "reset"://重置
                ResetPage();
                break;
            case "stat":
                StatPage();
                break;
            case "add"://增加商品
                AddGoods();
                break;
            case "paijiang"://币值派奖
                PaiJiang();
                break;
            case "goods"://实物派奖
                GoodsPaiJiang();
                break;
            case "goodsSend"://实物派奖-添加快递单号信息
                GoodsPaiJiangSend();
                break;
            case "setStatue"://设置
                SetStatue();
                break;
            case "set"://设置
                KbygSet();
                break;
            case "img"://增加商品图片
                AddImg();
                break;
            case "check"://审核
                CheckGoods();
                break;
            case "userpass"://用户商品通过审核
                UserPassCheck();
                break;
            case "pass"://管理商品状态UpdateGoodsAll
                PassCheck();
                break;
            case "update"://修改商品信息
                UpdateGoodsAll();
                break;
            case "returntime"://返还
                ReturnTime();
                break;
            case "returngoods"://返还货物
                ReturnGoods();
                break;
            case "selec"://返
                selectAll();
                break;
            case "seeorder"://查看订单信息
                SeeOrderList();
                break;
            case "frash"://刷新
                Frash();
                break;
            case "setceshi"://测试设置
                SetStatueCeshi();
                break;
            case "count":
                CountList();//排行榜
                break;
            default:
                BuyLists();  //主页
                break;
        }
    }
    //购买记录管理
    private void BuyLists()
    {
        Master.Title = Gamename + "开奖管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Gamename + "管理" + "&gt;");
        builder.Append("购记录");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));//DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        if (ptype == 1)
        {
            builder.Append("记录|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("出售|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">出售</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("进行|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">进行</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("结束");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">结束</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("[删除][是否中奖][列数][昵称][ID][期数][商品名称][购买量][消费][购买时间]");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (uid > 0)
            strWhere = "UserId=" + uid + "";
        try
        {
            if (start.Length > 1)
            {
                if (uid > 0)
                {
                    strWhere = "UserId=" + uid + "and BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'";
                }
                else
                {
                    strWhere = "BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'";
                }
            }
            else
            {
                start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        catch { builder.Append(Out.Div("div", "输入查询的时间格式有误！.")); }
        string[] pageValUrl = { "act", "uid", "backurl", "start", "down" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        try
        {
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.yg_BuyLists> listSSCpay = new BCW.BLL.yg_BuyLists().Getyg_BuyListss(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.yg_BuyLists n in listSSCpay)
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
                    BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(n.GoodsNum);
                    //   builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=del&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                    builder.Append(k + ".");
                    if (n.Address == "1" && goods.isDone != 2)
                    {
                        if (goods.GoodsType == 4 || goods.GoodsType == 5 || goods.GoodsType == 8 || goods.GoodsType == 9)
                        {
                            if (new BCW.BLL.yg_OrderLists().ExistsGoodsId(Convert.ToInt32(goods.Id)))
                            {
                                BCW.Model.yg_OrderLists order = new BCW.BLL.yg_OrderLists().Getyg_OrderListsForGoodsListId(goods.Id);
                                builder.Append("<font  color=\"red\">" + "[实物][中]" + "</font>" + "[已添]" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=goods&amp;id=" + n.Id + "&amp;ptype=" + n.GoodsNum + ";backurl=" + Utils.PostPage(1) + "") + "\">" + "[派]" + "</a>");
                            }
                            else
                            {
                                builder.Append("<font color=\"green\">"); builder.Append("[实物][中]" + "[未添]"); builder.Append("</font>");
                            }

                        }
                        else
                        {

                            builder.Append("<font  color=\"red\">" + "[币值][中]" + "</font>" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=paijiang&amp;id=" + n.Id + "&amp;ptype=" + n.GoodsNum + ";backurl=" + Utils.PostPage(1) + "") + "\">" + "[派]" + "</a>");

                        }
                    }
                    else
                        if (n.Address == "0" && goods.isDone != 2)
                        { builder.Append("[否]"); }
                    if (goods.isDone == 2)
                    { builder.Append("[开奖中]"); }
                    else
                        if (n.Address == "2")
                        { builder.Append("<font  color=\"red\">" + "[已领]" + "</font>"); }
                    builder.Append("(编号" + n.Id + ")");
                    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(n.UserId) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId)) + "</a>|");
                    //  builder.Append("(" + n.UserId + ")");
                    builder.Append("第" + new BCW.BLL.GoodsList().Getperiods(n.GoodsNum) + "期");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.GoodsNum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.GoodsList().GetGoodsName(n.GoodsNum) + "</a>");
                    builder.Append("[" + "编号" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.GoodsNum + "&amp;") + "\">" + goods.Id + "</a>" + "]" + "量" + n.Counts + "<font color=\"green\">" + "(消费" + n.IsGet + ")" + "</font>" + "时间" + Convert.ToDateTime(n.BuyTime).ToString("MM-dd HH:mm"));
                    // builder.Append(" <a href=\"" + Utils.getUrl("bbs/game/kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "") + "\">查看云购码...</a>");
                    k++; //BCW.User.Users.SetUser(n.Winner)
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "无查询记录."));
            }
            //builder.Append(Out.Div("div", "购买记录查询:"));
            string strText = "购买记录查询:/查询会员:,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "";
            string strOthe = "确定,kbyg.aspx?act=buylist&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            // builder.Append(Out.Tab("<div>", ""));
            string strText1 = "时间查询:/开始:,结束:,";
            string strName1 = "start,down,backurl";
            string strType1 = "text,text,hidden";
            string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,true,false";
            string strIdea1 = "";
            string strOthe1 = "确定,kbyg.aspx?act=buylist&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        catch { builder.Append("输入的时间格式有误."); }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=add") + "\">添加商品</a>.");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update") + "\">修改商品信息</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=returntime") + "\">过期返还</a><br />");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=setceshi") + "\">云购测试</a><br />");   
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=set&amp;backurl=" + Utils.PostPage(1) + "") + "\">云购配置</a>.");

        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=reset") + "\">重置云购</a><br/>"); 
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=setceshi") + "\">测试管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=count") + "\">榜单管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=stat") + "\">赢利分析</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=setStatue") + "\">测试维护</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    private void ReloadPage()
    {
        Master.Title = Gamename + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "0"));
        if (ptype == 1)
        {
            builder.Append("购记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">购记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("出售|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">出售</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("进行中|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">进行中</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("已结束|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">已结束</a>|");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=frash") + "\">刷新开奖</a>");
        //builder.Append(Out.Tab("</div>", ""));
        //  BuyLists();
        // Footer();


        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //管理底部
    private void Footer()
    {
        builder.Append(Out.Tab("<div class=class=\"text\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">购记录</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">未审核</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">进行中</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">已结束</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=frash") + "\">" + "刷" + "</a><br/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=add") + "\">添加云购商品</a><br />");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update") + "\">修改商品信息</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=returntime") + "\">币值过期返还</a><br />");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=stat") + "\">赢利分析</a><br />");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=setceshi") + "\">云购测试</a><br />");



        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=setStatue") + "\">云购维护</a><br />");


        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=set&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Gamename + "配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=reset") + "\">重置云购设置</a><br />");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //手动刷新开奖
    private void Frash()
    {
        try
        {
            isTimeToOpen(); //开奖设置倒计时
            isTimeToOpenNew(); //倒计时完毕
            SearchReturnGoods();
            Utils.Success("刷新成功", "成功刷新." + "<br/>" + "正在返回管理首页." + "", Utils.getUrl("kbyg.aspx"), "1");
        }
        catch { }
    }

    //查看订单信息
    private void SeeOrderList()
    {
        Master.Title = "查看订单快递信息";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("查看订单快递信息");
        builder.Append(Out.Tab("</div>", ""));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        string strText = "输入订单编号:/,";
        string strName = "id,backurl";
        string strType = "num,hidden";
        string strValu = "" + id + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜订单记录,kbyg.aspx?act=seeorder&amp;,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //builder.Append("购买编号:" + order.BuyListId+ "<br/>");  
        //builder.Append("物流名称:" + order.wuliu + "<br/>");
        //builder.Append("运单号:" + order.yundanhao + "<br/>");
        //builder.Append("操作员:" + order.Operator + "<br/>");
        //builder.Append("备注:" + order.OperatorNotes + "<br/>");
        //builder.Append("物流状态:" + order.wuliuStatue + "<br/>");
        //builder.Append("收货人:" + order.Consignee + "<br/>");
        //builder.Append("收货地址:" + order.Address + "<br/>");
        //builder.Append("邮编:" + order.ZipCode + "<br/>");
        //builder.Append("手机:" + order.PhoneMobile + "<br/>");
        //builder.Append("备用手机:" + order.PhoneHome + "<br/>");
        //builder.Append("玩家备注:" + order.ConsigneeNotes + "<br/>");
        //builder.Append("订单状态:" + order.isDone + "<br/>");
        //builder.Append("订单时间:" + Convert.ToDateTime(order.AddTime).ToString("yyyy-MM-dd hh:mm:ss") + "<br/>");
        //builder.Append("发出时间:" + Convert.ToDateTime(order.PostTime).ToString("yyyy-MM-dd hh:mm:ss") + "<br/>");
        // builder.Append("结束时间:" + Convert.ToDateTime(order.OverTime).ToString("yyyy-MM-dd hh:mm:ss") + "<br/>");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string wuliu = Utils.GetRequest("wuliu", "post", 2, @"^[^\^]{1,20}$", "物流名称限1-20字内");
            string yundanhao = Utils.GetRequest("yundanhao", "post", 3, @"^[1-9]\d*$", "运单号限数字");
            string Operator = Utils.GetRequest("Operator", "post", 3, @"^[^\^]{0,20}$", "操作员限1-20字内");
            string OperatorNotes = Utils.GetRequest("OperatorNotes", "post", 2, @"^[^\^]{0,200}$", "操作员备注限200字内");
            string wuliuStatue = Utils.GetRequest("wuliuStatue", "post", 4, @"^[0-9]\d*$", "物流状态限数字(整数)");
            string Consignee = Utils.GetRequest("Consignee", "post", 2, @"^[^\^]{0,20}$", "收货人姓名限10字内");
            string Address = Utils.GetRequest("Address", "post", 3, @"^[^\^]{1,200}$", "收货人地址限200字内");
            string ZipCode = Utils.GetRequest("ZipCode", "post", 2, @"^[0-9]\d*$", "邮编为数字");
            string PhoneMobile = Utils.GetRequest("PhoneMobile", "post", 2, @"^[0-9]\d*$", "手机号码输入错误");
            string PhoneHome = Utils.GetRequest("PhoneHome", "post", 2, @"^[0-9]\d*$", "备用手机填写错误");
            string ConsigneeNotes = Utils.GetRequest("ConsigneeNotes", "post", 2, @"^[^\^]{0,2000}$", "玩家备注填写错误");
            string isDone = Utils.GetRequest("isDone", "post", 2, @"^[0-9]\d*$", "订单状态错误");
            //  string AddTime = Utils.GetRequest("AddTime", "post", 2, @"^[^\^]{1,20}$", "商品名称限1-20字内");
            //  string PostTime = Utils.GetRequest("PostTime", "post", 2, @"^[^\^]{1,20}$", "商品名称限1-20字内");
            try
            {
                BCW.Model.yg_OrderLists order = new BCW.BLL.yg_OrderLists().Getyg_OrderLists(id);
                //BCW.Model.GoodsList mod = new BCW.BLL.GoodsList().GetGoodsList(id);
                order.wuliu = wuliu;
                order.yundanhao = yundanhao;
                order.Operator = Operator;
                order.wuliuStatue = Convert.ToInt32(wuliuStatue);
                order.Consignee = Consignee;
                order.Address = Address;
                order.ZipCode = Convert.ToInt32(ZipCode);
                order.PhoneMobile = PhoneMobile;
                order.PhoneHome = PhoneHome;
                order.ConsigneeNotes = ConsigneeNotes;
                order.isDone = Convert.ToInt32(isDone);
                new BCW.BLL.yg_OrderLists().Update(order);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("修改成功," + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=seeorder&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回继续修改</a>");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            catch
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("修改失败," + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=seeorder&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        else
        {
            try
            {
                BCW.Model.yg_OrderLists order = new BCW.BLL.yg_OrderLists().Getyg_OrderLists(id);
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("订单号:" + order.Id + "|");
                builder.Append("商品号:" + order.GoodsListId);
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + order.GoodsListId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.GoodsList().GetGoodsName(order.GoodsListId) + "</a>|");
                builder.Append("购买号:" + order.BuyListId);
                builder.Append(Out.Tab("</div>", "<br/>"));
                string Text = "物流名称:/,运单号:/,操作员:/,备注:/,物流状态:/,收货人:/,收货地址:/,邮编:/,手机:/,备用手机:/,玩家备注:/,订单状态:/,订单时间(仅显示):/,发出时间(仅显示):/,";
                string Name = "wuliu,yundanhao,Operator,OperatorNotes,wuliuStatue,Consignee,Address,ZipCode,PhoneMobile,PhoneHome,ConsigneeNotes,isDone,AddTime,PostTime,act";
                string Type = "text,text,text,textarea,select,text,textarea,text,text,text,textarea,select,text,text,hidden";
                string Valu = "" + order.wuliu + "'" + order.yundanhao + "'" + order.Operator + "'" + order.OperatorNotes + "'" + order.wuliuStatue + "'" + order.Consignee + "'" + order.Address + "'" + order.ZipCode + "'" + order.PhoneMobile + "'" + order.PhoneHome + "'" + order.ConsigneeNotes + "'" + order.isDone + "'" + Convert.ToDateTime(order.AddTime).ToString("yyyy-MM-dd hh:mm:ss") + "'" + Convert.ToDateTime(order.PostTime).ToString("yyyy-MM-dd hh:mm:ss") + "'" + Utils.getPage(0) + "";
                string Empt = "false,true,true,true,0|未发货|1|已发货|2|已收货,false,true ,false,true,false,false,0|未处理|1|已处理,false,false";
                string Idea = "/";
                string Othe = "确定修改|reset,kbyg.aspx?act=seeorder&amp;id=" + id + "&amp;,post,1,red|blue";
                builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示:" + "订单已填写生成." + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
            }
            catch
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("输入错误:" + "该订单号不存在." + "<br/>");
                builder.Append(Out.Tab("</div>", ""));

            }

        }

        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">返回上级</a><br />");  
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("查看订单快递信息");
        builder.Append(Out.Tab("</div>", ""));
    }

    //返还币值
    private void ReturnGoods()
    {
        Master.Title = Gamename + "返还";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append(Gamename + "返还币值");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        // int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = Gamename + "返还";
            //builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定返还该商品的购买记录吗?" + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=returngoods&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定返还</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=returntime&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.GoodsList().Exists(Convert.ToInt64(ptype)))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.Model.GoodsList modell = new BCW.BLL.GoodsList().GetGoodsList(ptype);
            if (modell.isDone == 3 || (Utils.ToSChinese(modell.StockYungouma) == "商品下架并成功返还"))
            {
                builder.Append(Out.Div("div", "提示:该商品已返还所有购买记录,请勿重复提交." + "<br/>"));
            }
            else
            {
                ReturnGoods(ptype);
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

    }

    //商品过期币值返还
    private void ReturnTime()
    {
        try
        {
            SearchReturnGoods();
        }
        catch { }
        Master.Title = "" + Gamename + "返还";
        // builder.Append(Out.Div("title", ""+Gamename+"返还"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("" + Gamename + "返还");
        builder.Append(Out.Tab("</div>", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("注:停仅停止销售商品,返将停止并同时下架该商品");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("[编号][状态][期数][名称][参与][来源]");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (uid > 0)
            strWhere = "Id=" + uid + "";
        string[] pageValUrl = { "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.GoodsList n in listSSCpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                }

                builder.Append(n.Id + ".");
                if (n.isDone == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=pass&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[停]</a>|");
                }
                if (n.isDone == 4) { builder.Append("<font color=\"red\">" + "[停售]" + "</font>"); }
                if (n.isDone == 3) { builder.Append("[已返还]"); }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=returngoods&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[返]</a>");
                }
                builder.Append("(第" + n.periods + "期)");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                //+ "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                // builder.Append("图片描述：" + "<br/>");
                // string[] imgNum = n.GoodsImg.Split(',');
                //foreach (string m in imgNum)
                //{
                //    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + Logo + m + "\"  width=\"50\" height=\"40\" alt=\"load\" border=\"10\" border-color=\"#C0C0C0\" />" + "</a>&nbsp;&nbsp;&nbsp;");
                //} 
                BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(n.Winner));
                // builder.Append("商品编号:" + n.Id + "商品价值:" + n.GoodsValue + "开奖时间:" + n.OverTime);
                builder.Append("已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "") + "\">" + n.Number + "</a>");
                // builder.Append("中奖用户ID" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UserId + "</a>");
                //  builder.Append("购买量(" + model.Counts + ")");
                if (n.isDone == 0) { builder.Append("[已完结]"); }
                else if (n.isDone == 1)
                {
                    builder.Append("[进行中]");
                    DateTime now = DateTime.Now;
                    DateTime dt = Convert.ToDateTime(n.Addtime);
                    TimeSpan span = DateTime.Now - dt;
                    if (span.TotalDays > 14)//2周前
                    {
                        builder.Append("[2周]" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=returngoods&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返还此商品的酷币</a>");
                    }
                    else if (span.TotalDays > 30)
                    {
                        builder.Append("[1个月]" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返还此商品的酷币</a>");
                    }
                }
                else if (n.isDone == 2) { builder.Append("[开奖中]"); }
                else if (n.isDone == 3) { builder.Append("[已下架]"); }
                else if (n.isDone == 4) { builder.Append("[未过审]"); }
                else if (n.isDone == 5) { builder.Append("[待审]"); }
                else if (n.isDone == 6) { builder.Append("[已取消]"); }
                // if (n.GoodsType == 8 || n.GoodsType == 9 || n.GoodsType == 6||n.GoodsType==7)
                {
                    if (n.GoodsType == 8 || n.GoodsType == 9)
                    {
                        builder.Append("[源:" + "玩家物" + "]");
                    }
                    else if (n.GoodsType == 6 || n.GoodsType == 7) { builder.Append("[源:" + "玩家币]"); }
                    else { builder.Append("[源:" + "系统" + "]"); }

                }

                k++; //BCW.User.Users.SetUser(n.Winner)
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("没有相关记录");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        string strText = "输入商品ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜商品,kbyg.aspx?act=returntime&amp;,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append(Gamename + "返还");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //游戏维护与开放
    private void SetStatue()
    {
        Master.Title = Gamename + "维护";
        builder.Append(Out.Div("title", "" + Gamename + "维护"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/myyg.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
            xml.dss["KbygStatus"] = Status;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success(Gamename + "设置", "设置成功，正在返回..", Utils.getUrl("kbyg.aspx?act=set;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            string strText = "云购维护:/,";
            string strName = "Status,backurl";
            string strType = "select,hidden";
            string strValu = "" + xml.dss["KbygStatus"] + "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|正常|1|维护|2|测试,";
            string strIdea = "/";
            string strOthe = "确定修改|reset,kbyg.aspx?act=setStatue,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            if (xml.dss["KbygStatus"].ToString() == "0")
            {
                builder.Append("<b>" + "游戏正在正常运行中..." + "</b>");
            }
            else if (xml.dss["KbygStatus"].ToString() == "1")
            {
                builder.Append("<b>" + "游戏已维护." + "</b>");
            }
            else if (xml.dss["KbygStatus"].ToString() == "2")
            {
                builder.Append("<b>" + "游戏正在测试中." + "</b>");
            }
            builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=frash") + "\">" + "刷新开奖" + "</a>");
            builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=setceshi") + "\">测试管理</a>");
            builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=reset") + "\">重置云购</a>");
            builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //测试设置测试状态
    private void SetStatueCeshi()
    {
        Master.Title = "云购设置测试状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">"+Gamename+管理</a>&gt;");
        builder.Append("云购设置测试状态");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/myyg.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ceshiTimeSpan = Utils.GetRequest("ceshiTimeSpan", "post", 2, @"^[0-9]\d*$", "云币领取时间间隔输入出错");
            string ceshi = Utils.GetRequest("ceshi", "post", 2, @"^[0-9]\d*$", "测试权限管理隔输入出错");
            string ceshiGet = Utils.GetRequest("ceshiGet", "post", 2, @"^[1-9]\d*$", "领取值大小选择出错");
            string CeshiQualification = Utils.GetRequest("CeshiQualification", "post", 2, @"^[^\^]{1,2000}$", "添加测试号输入出错");
            xml.dss["ceshiTimeSpan"] = ceshiTimeSpan;
            xml.dss["ceshi"] = ceshi;
            xml.dss["ceshiGet"] = ceshiGet;
            xml.dss["CeshiQualification"] = CeshiQualification;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + Gamename + "设置", "设置成功，正在返回..", Utils.getUrl("kbyg.aspx?act=setceshi"), "1");
        }
        else
        {

            string strText = "测试权限管理:/,云币领取时间间隔:/,领取值大小:/,添加测试号(#号结束):/,";
            string strName = "ceshi,ceshiTimeSpan,ceshiGet,CeshiQualification,backurl";
            string strType = "select,select,text,text,hidden";
            string strValu = xml.dss["ceshi"] + "'" + xml.dss["ceshiTimeSpan"] + "'" + xml.dss["ceshiGet"] + "'" + xml.dss["CeshiQualification"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|测试版会员可测试|1|测试版测试号测试|2|酷币版测试号测试|3|酷币版开放,5|5分钟|10|10分钟|30|30分钟|60|60分钟,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,kbyg.aspx?act=setceshi,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
            string[] name = CeshiQualification.Split('#');
            // foreach (string n in imgNum)
            builder.Append("当前测试号:" + "<br/>");
            for (int n = 0; n < name.Length - 1; n++)
            {
                builder.Append(name[n] + ",");
                if (n > 2 && n % 5 == 0)
                { builder.Append("<br/>"); }
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回上级</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //审核管理
    private void CheckGoods()
    {
        Master.Title = "申请出售审核管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("审核管理" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "2"));
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("出售|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">出售</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("进行|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">进行</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("结束");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">结束</a>");
        }
        builder.Append("<br/>" + "行：[出售会员][商品ID][状态类型][名称][玩家留言]");
        builder.Append(Out.Tab("</div>", "<br />"));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        if (uid > 0)
        {
            strWhere = "Identification=" + uid + "";
        }
        else
        { strWhere = "Identification<>0"; }
        try
        {
            string[] pageValUrl = { "act", "uid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.GoodsList n in listSSCpay)
                {
                    if (n.isDone != 1)
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

                        //   builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=del&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                        //   builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[改]&gt;</a>");
                        builder.Append(k + ".");
                        //    "(" + n.Identification + ")"
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(n.Identification) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(n.Identification)) + "</a>" + "[编号" + n.Id + "]");
                        if (n.GoodsType % 2 == 0)
                        {
                            builder.Append("[循]");
                        }
                        else
                        { builder.Append("[不循环]"); }
                        if (n.isDone == 5)
                        {
                            builder.Append("[未审]");
                        }
                        if (n.isDone == 4)
                        {
                            builder.Append("[未过审]");
                        }
                        if (n.isDone == 1)
                        {
                            builder.Append("[进行中]");
                        }
                        if (n.isDone == 2)
                        {
                            builder.Append("[开奖中]");
                        }
                        if (n.isDone == 3)
                        {
                            builder.Append("[已停]");
                        }
                        if (n.isDone == 6)
                        {
                            builder.Append("[已取消]");
                        }
                        if (n.isDone == 7)
                        {
                            builder.Append("[已过期]");
                        }
                        // builder.Append("编号:" + n.Id + ",");
                        //  builder.Append("申请Id:" + n.Identification + ",");

                        //  builder.Append("商品描述:" + n.explain + ",");
                        //  builder.Append("商品总价值:" + n.GoodsValue * (Convert.ToInt64(n.statue))+ ",");
                        //  builder.Append("商品总需参与人数:" + n.GoodsValue + ",");
                        //  builder.Append("商品当前人数:" + n.Number + ",");
                        //   builder.Append("商品添加时间:" + Convert.ToDateTime(n.Addtime).ToString("yyyy-MM-dd hh:mm:ss") + ",");
                        //   builder.Append("用户留言:" + n.GoodsFrom + ",");
                        if (n.isDone == 0)
                        {
                            builder.Append("[已结束]");
                        }
                        if (n.isDone == 5)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=userpass&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核]</a>");
                        }
                        //if (n.isDone == 4)
                        //{
                        //   // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=userpass&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[取消通过]</a>");
                        //}
                        if (n.isDone == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=userpass&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[停]</a>");
                        }
                        builder.Append("名称:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                        //  builder.Append("[留言:" + n.GoodsFrom + "]");
                        k++; //BCW.User.Users.SetUser(n.Winner)
                        builder.Append(Out.Tab("</div>", ""));
                    }
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
            string strOthe = "搜出售记录,kbyg.aspx?act=check&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        catch { }
        // Footer();
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>");
        //   builder.Append("审核管理" + "<br/>");
        builder.Append(Out.Tab("</div>", "<br/>"));

    }

    //用户审核商品状态管理
    private void UserPassCheck()
    {
        Master.Title = "商品状态管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("商品状态管理" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "商品状态管理";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("请选择状态结果?");
            builder.Append("<br /><a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=userpass&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">通过审核</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=not&amp;act=userpass&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">不通过审核</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先看看吧.</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (info == "ok")
        {
            if (!new BCW.BLL.GoodsList().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            try
            {
                long i = Convert.ToInt64(id);
                BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(i);
                if (model.isDone == 1)
                {
                    // Utils.Error("该记录已审核为通过,请勿重复操作！","");
                    builder.Append("该记录已审核为通过,请勿重复操作");
                }
                else
                {
                    model.isDone = 1;
                    // model.GoodsValue += 10;
                    //new BCW.BLL.GoodsList().
                    new BCW.BLL.GoodsList().Update(model);
                    // new BCW.BLL.GoodsList().UpdateisDone(i, 1);
                    string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.Identification));
                    string strs = "[URL=/bbs/game/kbyg.aspx]云购[/URL]" + "成功出售标识" + model.Id + "通过审核";
                    new BCW.BLL.Guest().Add(1, Convert.ToInt32(model.Identification), name, strs);//向会员发内线

                    //Utils.Success("审核成功", "审核通过成功.押金已正常扣压.", Utils.getPage("kbyg.aspx?act=check&amp;"), "3");
                    builder.Append("编号" + id + "<br/>" + "名称" + model.GoodsName + "<br/>");
                    //Utils.Error("该记录已审核为不通过,请勿重复操作！", "");
                    builder.Append("审核通过成功.押金已正常扣压." + "<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">返回继续审核</a>");
                }
            }
            catch (Exception ee)
            {
                new BCW.BLL.Guest().Add(727, "测试727", "审核有误！" + id + ee.ToString());
                //Utils.Success("审核错误", "审核通过失败,请重新审核..", Utils.getPage("kbyg.aspx?act=check&amp;"), "1");
                // builder.Append(ee);
            }
        }
        else if (info == "not")
        {
            if (!new BCW.BLL.GoodsList().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            try
            {
                long i = Convert.ToInt64(id);
                BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(i);
                if (model.isDone == 4)
                {
                    builder.Append("编号" + id + "<br/>" + "名称" + model.GoodsName + "<br/>");
                    //Utils.Error("该记录已审核为不通过,请勿重复操作！", "");
                    builder.Append("该记录已审核为不通过,请勿重复操作！" + "<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">返回继续审核</a>");
                }
                else
                {
                    model.isDone = 4;

                    //int num = 4;
                    //new BCW.BLL.GoodsList().UpdateisDone(i, 4);
                    new BCW.BLL.GoodsList().Update(model);
                    int addGoodsValue = Convert.ToInt32(ub.GetSub("addGoodsValue", xmlPath));
                    long gool = Convert.ToInt64((model.GoodsValue - addGoodsValue) * model.statue);
                    string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.Identification));
                    new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.Identification), gool, "云购退回押金" + model.Id + "|不通过审核");
                    new BCW.BLL.Guest().Add(1, Convert.ToInt32(model.Identification), name, "[URL=/bbs/game/kbyg.aspx]云购[/URL]" + "出售标识" + model.Id + "不通过审核,押金已成功退回");//向会员发内线
                    builder.Append("编号" + id + "名称" + model.GoodsName + "<br/>");
                    builder.Append("审核成功,押金已退回." + "<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">返回继续审核</a>");
                    // Utils.Success("审核不通过成功,押金已退回", "审核不通过成功,押金已退回", Utils.getPage("kbyg.aspx?act=check"), "3");
                }
            }
            catch (Exception ee)
            {
                Utils.Success("审核错误", "审核不通过失败,请重新审核..", Utils.getPage("kbyg.aspx?act=check"), "1");
                //  builder.Append(ee);
                new BCW.BLL.Guest().Add(727, "测试727", "审核有误！" + id + ee.ToString());
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("../kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("审核管理" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        //  builder.Append(Out.Tab("", ""));
    }

    //商品状态管理
    private void PassCheck()
    {
        Master.Title = "商品状态管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("商品状态管理");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "商品状态管理";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("请选择状态结果?");
            builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=pass&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续销售</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=not&amp;act=pass&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">停止销售</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先看看吧.</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (info == "ok")
        {
            if (!new BCW.BLL.GoodsList().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            try
            {
                long i = Convert.ToInt64(id);
                BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(i);
                builder.Append(Out.Tab("<div>", ""));
                if (model.isDone == 1)
                {
                    builder.Append("商品<font color=\"red\">" + model.GoodsName + "</font>正在销售.请勿重复提交！");
                }
                else
                {
                    model.isDone = 1;
                    // int num = 1;
                    new BCW.BLL.GoodsList().Update(model);
                    builder.Append("继续销售通过成功.");
                }
                builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=returntime&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
                builder.Append(Out.Tab("</div>", ""));
                //  Utils.Success("处理成功", "处理了过成功..", Utils.getUrl("kbyg.aspx?act=check&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
            catch
            {
                Utils.Success("处理失败", "处理失败.请重新处理.", Utils.getUrl("kbyg.aspx?act=check&amp;backurl=" + Utils.getPage(0) + ""), "5");
            }
        }
        else if (info == "not")
        {
            if (!new BCW.BLL.GoodsList().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            try
            {
                long i = Convert.ToInt64(id);
                BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(i);
                builder.Append(Out.Tab("<div>", ""));
                if (model.isDone == 4)
                {
                    builder.Append("商品<font color=\"red\">" + model.GoodsName + "</font>已停止销售.请勿重复提交！");
                }
                else
                {
                    model.isDone = 4;
                    //int num = 4;
                    //new BCW.BLL.GoodsList().UpdateisDone(i, 4);

                    new BCW.BLL.GoodsList().Update(model);
                    builder.Append("停止销售成功!");
                }
                builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=returntime&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
                builder.Append(Out.Tab("</div>", "<br/>"));
                //Utils.Success("处理成功", "处理不通过成功..", Utils.getUrl("kbyg.aspx?act=check&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
            catch { Utils.Success("处理失败", "处理失败.请重新处理.", Utils.getUrl("kbyg.aspx?act=check&amp;backurl=" + Utils.getPage(0) + ""), "5"); }
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("../kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("审核管理" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //派奖给中奖人员(币值派奖)
    private void PaiJiang()
    {
        Master.Title = "云购币值派奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("云购币值派奖" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//购买列id.
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(id));
        BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(ptype));
        if (info == "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("商品：" + "第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsNum) + "云" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsNum) + "派奖" + "<br/>");

            builder.Append("中奖购买列：" + model.Id + "<br/>" + "购买时间：" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd hh:mm:ss fff") + "<br/>" + "登录IP：" + model.Ip + "<br/>");
            builder.Append("操作系统：" + model.System + "<br/>");
            builder.Append("中奖Id：" + model.UserId + "<br/>");
            builder.Append("中奖昵称：" + new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId)) + "<br/>");
            builder.Append("获奖商品：" + "第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsNum) + "云" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsNum) + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            string goodsname = new BCW.BLL.GoodsList().GetGoodsName(Convert.ToInt64(model.GoodsNum));
            Master.Title = "派奖结果";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定派奖该商品吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=paijiang&amp;ptype=" + model.GoodsNum + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定派奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=edit&amp;backurl=" + Utils.getPage(0) + "") + "\">先看看吧..</a><br/>");
            builder.Append("温馨提示：" + "请确认获奖者信息对应的商品后派奖");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            ;
            if (!new BCW.BLL.GoodsList().Exists(Convert.ToInt64(ptype)))
            {
                Utils.Error("不存在的商品", "");
            }
            else
                if (!new BCW.BLL.yg_BuyLists().Exists(Convert.ToInt64(id)))
                {
                    Utils.Error("不存在的购买记录", "");
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    int KbygPersan = Convert.ToInt32(ub.GetSub("KbygPersan", xmlPath));
                    long suishou = Convert.ToInt64((goods.statue * goods.GoodsValue) * KbygPersan * 0.001);
                    long god = Convert.ToInt64((goods.statue * goods.GoodsValue) - suishou);//税率
                    builder.Append("商品Id:" + ptype + "<br/>");
                    builder.Append("购买记录列:" + id + "<br/>");
                    builder.Append("获奖者:" + model.UserId + "<br/>");
                    builder.Append("获奖总价值:" + goods.GoodsValue * goods.statue + "<br/>");
                    builder.Append("应派：" + god + "<br/>");
                    builder.Append("税率：" + KbygPersan * 0.001 + "<br/>");
                    builder.Append("税收：" + suishou + "<br/>");
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                    if (model.Address == "2")
                    {
                        builder.Append("此中奖记录已成功领奖！" + "<br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">返回继续派奖</a><br />");
                    }
                    else
                    // if (model.Address == "1")
                    {
                        //BCW.Model.GoodsList goods1 = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(ptype));
                        // if (goods.GoodsType == 2 || goods.GoodsType == 3 || goods.GoodsType == 1||goods.GoodsType == 0)
                        {

                            try
                            {
                                if (statue == 0)
                                {
                                    //new BCW.BLL.User().UpdateiGold(meid, coast, "云购购买商品消费");
                                    new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.UserId), god, "标识ID" + id + "云购中奖商品后台兑奖");
                                    string addr = "2";
                                    long i = id;
                                    new BCW.BLL.yg_BuyLists().UpdateAddress(i, addr);
                                    new BCW.BLL.Action().Add(GameId, 0, Convert.ToInt32(model.UserId), mename, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]中奖获得了" + god + "币");
                                    string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]获得了" + god + "" + ub.Get("SiteBz") + "[URL=/bbs/game/kbyg.aspx?act=addJ]去看看吧[/URL]";
                                    new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), mename, strLog);
                                    builder.Append("派奖成功！" + "<br/>");
                                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">返回继续派奖</a>");
                                }
                                else
                                    if (statue == 2)
                                    {
                                        //测试状态
                                        new BCW.SWB.BLL().UpdateMoney(Convert.ToInt32(model.UserId), god, 1007);
                                        string addr = "2";
                                        long i = id;
                                        new BCW.BLL.yg_BuyLists().UpdateAddress(i, addr);
                                        new BCW.BLL.Action().Add(0, 0, Convert.ToInt32(model.UserId), mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]中奖获得了" + god + "币");
                                        string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]测试获得了" + god + "" + "云币" + "[URL=/bbs/game/kbyg.aspx?act=addJ]去看看吧[/URL]";
                                        new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), mename, strLog);
                                        builder.Append("派奖成功！" + "<br/>");
                                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">返回继续派奖</a>");

                                    }

                            }
                            catch (Exception e)
                            {   //builder.Append(e); 
                                // builder.Append("派奖失败,请重新派奖00！" + "<br/>");
                                new BCW.BLL.Guest().Add(727, "测试727", "后台派奖异常！请查看,商品Id:" + ptype + "异常:" + e);
                            }
                        }
                        ////  else
                        //{
                        //    builder.Append("" + "<br/>");
                        //}
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }

        }
        // Footer();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //派奖给中奖人员(实物派奖)
    private void GoodsPaiJiang()
    {
        Master.Title = "云购货物派奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("云购货物派奖" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//购买列id.
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(id));
        BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(ptype));
        if (info == "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("实物商品：" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsNum) + "第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsNum) + "云" + "派奖" + "<br/>");
            builder.Append("中奖购买列：" + model.Id + "<br/>" + "购买时间：" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd hh:mm:ss fff") + "<br/>" + "登录IP：" + model.Ip + "<br/>");
            builder.Append("操作系统：" + model.System + "<br/>");
            builder.Append("中奖Id：" + model.UserId + "<br/>");
            builder.Append("中奖昵称：" + new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId)) + "<br/>");
            builder.Append("获奖商品：" + "第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsNum) + "云" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsNum) + "<br/>");
            builder.Append("购买份数:" + model.Counts + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("请在确定后联系收货人并正确填写快递信息与单号!" + "<br/>");
            builder.Append("确定派奖该实物商品吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=goods&amp;ptype=" + model.GoodsNum + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定派奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open&amp;backurl=" + Utils.getPage(0) + "") + "\">先看看吧..</a>" + "<br/>");
            builder.Append("温馨提示：" + "请确认获奖者信息对应的商品后派奖");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            ;
            if (!new BCW.BLL.GoodsList().Exists(Convert.ToInt64(ptype)))
            {
                Utils.Error("不存在的商品", "");
            }
            else
                if (!new BCW.BLL.yg_BuyLists().Exists(Convert.ToInt64(id)))
                {
                    Utils.Error("不存在的购买记录", "");
                }
                else
                {
                    if (!new BCW.BLL.yg_OrderLists().ExistsGoodsId(Convert.ToInt32(goods.Id)))//不存在id订单列
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("玩家未领奖,还未提交相应的快递信息.");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        if (model.Address == "2")
                        {
                            builder.Append("此中奖记录已成功领奖！" + "<br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">返回继续派奖</a><br />");
                        }
                        else
                        {

                            BCW.Model.yg_OrderLists mode = new BCW.BLL.yg_OrderLists().Getyg_OrderListsForGoodsListId(Convert.ToInt32(goods.Id));
                            int KbygPersan = Convert.ToInt32(ub.GetSub("KbygPersan", xmlPath));
                            long suishou = Convert.ToInt64((goods.statue * goods.GoodsValue) * KbygPersan * 0.001);
                            long god = Convert.ToInt64((goods.statue * goods.GoodsValue) - suishou);//税率
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<b>" + "商品信息:" + "</b><br/>");
                            builder.Append("商品Id:" + ptype + "<br/>" + "期数:" + "第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsNum) + "云" + "<br/>" + "实物商品:" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsNum) + "<br/>");
                            string[] imgNum = goods.GoodsImg.Split(',');
                            if (goods.GoodsImg.ToString() != "0" && goods.GoodsImg.ToString() != "100" && goods.GoodsImg.ToString() != "5" && goods.GoodsImg.ToString() != "10" && goods.GoodsImg.ToString() != "1")
                            {
                                builder.Append("图片描述:");
                                builder.Append("<br/>");
                                // foreach (string n in imgNum)
                                for (int c = 0; c < imgNum.Length - 1; c++)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"10%\" alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                                }
                                { builder.Append("<br/>"); }
                            }
                            // if ((imgNum.Length) % 2 == 0)

                            builder.Append("商品描述:" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(goods.explain)) + "<br/>");
                            builder.Append("商品总价值:" + goods.GoodsValue * goods.statue + "<br/>");
                            builder.Append("玩家Id:" + model.UserId + "<br/>");
                            builder.Append("中奖昵称:" + new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId)) + "<br/>");
                            builder.Append("中奖购买列:" + model.Id + "<br/>");
                            builder.Append("购买份数:" + model.Counts + "<br/>");
                            builder.Append("购买时间:" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff") + "<br/>" + "登录IP：" + model.Ip + "<br/>");
                            builder.Append("操作系统:" + model.System + "<br/>");
                            // builder.Append("获奖商品：" + "第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsNum) + "云" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsNum) + "<br/>");                    
                            builder.Append("<b>" + "订单信息:" + "</b><br/>");
                            builder.Append("收货地址:" + mode.Address + "<br/>");
                            builder.Append("邮编:" + mode.ZipCode + "<br/>");
                            builder.Append("收货人姓名:" + mode.Consignee + "<br/>");
                            builder.Append("手机:" + mode.PhoneMobile + "<br/>");
                            builder.Append("备用手机:" + mode.PhoneHome + "<br/>");
                            builder.Append("留言:" + mode.ConsigneeNotes + "<br/>");
                            builder.Append("<b>" + "请继续填写物流信息:" + "</b>");
                            builder.Append(Out.Tab("</div>", ""));
                            string strText = "物流名称:/,运单号(必填且正确):/,操作者(可为空):/,备忘(可为空):/,";
                            string strName = "name,yundanhao,doname,beizhu";
                            string strType = "text,num,text,num,textarea";
                            string strValu = "" + "中通" + "'" + 1 + "'" + 1 + "'" + 1 + "'" + "'";
                            string strEmpt = "false,true,true,false,false,true";
                            string strIdea = "/";
                            string strOthe = "确定发货|reset,kbyg.aspx?act=goodsSend&amp;order=" + mode.Id + "&amp;,post,1,red|blue";
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("温馨提示:每件商品物流信息仅限填写一次,请谨慎填写<br />");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                }
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        // builder.Append("云购货物派奖" + "<br/>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //  Footer();


    }

    // 云购货物派奖----订单填写
    protected void GoodsPaiJiangSend()
    {
        Master.Title = "云购货物派奖订单填写";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("云购货物派奖订单填写" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int order = Utils.ParseInt(Utils.GetRequest("order", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        string name = Utils.GetRequest("name", "post", 2, @"^[^\^]{1,20}$", "名称限1-20字内");
        string yundanhao = Utils.GetRequest("yundanhao", "post", 3, @"^[^\^]{1,200}$", "运单号为数字");
        string doname = Utils.GetRequest("doname", "post", 3, @"^[^\^]{0,20}$", "操作者输入错误");
        string beizhu = Utils.GetRequest("beizhu", "post", 2, @"^[^\^]{1,20}$", "备注填写错误");
        BCW.Model.yg_OrderLists model = new BCW.BLL.yg_OrderLists().Getyg_OrderLists(order);
        if (model.isDone == 1 && model.OperatorStatue == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("快递名称:" + model.wuliu + "<br/>");
            builder.Append("订单号:" + model.yundanhao + "<br/>" + "操作员:" + model.Operator + "<br/>");
            builder.Append("备注:" + model.OperatorNotes + "<br/>");
            builder.Append("该单号已存在,请勿重复提交." + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">返回继续派奖</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快递名称:" + name + "<br/>");
            builder.Append("订单号:" + yundanhao + "<br/>" + "操作员:" + doname + "<br/>");
            builder.Append("备注:" + beizhu + "<br/>");
            builder.Append(Out.Tab("</div>", "<br/>"));
            model.wuliu = name.ToString();
            model.wuliuStatue = 1;
            model.yundanhao = yundanhao;
            model.Operator = doname;
            model.OperatorNotes = beizhu;
            model.OperatorStatue = 1;
            model.PostTime = DateTime.Now;
            model.Spare = "1";
            model.Statue = 1;
            model.isDone = 1;
            model.brack = "1";
            try
            {
                if (statue == 0)//正常
                {
                    new BCW.BLL.yg_OrderLists().Update(model);//更新订单信息表
                    //  new BCW.BLL.User().UpdateiGold(meid, coast, "云购购买商品消费");
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                    // new BCW.BLL.Action().Add(0, 0, Convert.ToInt32(model.UserId), mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]中奖获得了" + god + "币");
                    string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]获奖的商品已发货,运单号" + model.wuliu + model.yundanhao + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                    new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), mename, strLog);
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("填写成功,请及时寄出商品！已发送内线！" + "<br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">返回继续派奖</a><br />");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }
                else if (statue == 2)//测试
                {
                    new BCW.BLL.yg_OrderLists().Update(model);//更新订单信息表
                    //  new BCW.BLL.User().UpdateiGold(meid, coast, "云购购买商品消费");
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                    // new BCW.BLL.Action().Add(31, 0, Convert.ToInt32(model.UserId), mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]中奖获得了" + god + "币");
                    string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]获得了测试商品的已发货,运单号" + model.yundanhao + "" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                    new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), mename, strLog);
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("填写成功,请及时寄出商品！" + "<br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">返回继续派奖</a><br />");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }
            }
            catch (Exception e) { builder.Append("订单号填写失败,请重新提交."); }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>");
        // builder.Append("云购货物派奖订单填写" + "<br/>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //查看云购码
    protected void SeeYungoum()
    {
        Master.Title = "云购码详情页";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("云购码详情页");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//购买列id.
        int user = Utils.ParseInt(Utils.GetRequest("user", "get", 1, @"^[1-9]\d*$", "0"));//对应用户
        BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(id);
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = " Id= " + id + "" + "and GoodsNum=" + ptype + "";
        // string strWhere ="GoodsNum=" + ptype + "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.yg_BuyLists> listSSCpay = new BCW.BLL.yg_BuyLists().Getyg_BuyListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.yg_BuyLists n in listSSCpay)
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
                //"[第" + ((pageIndex - 1) * pageSize + k) + "期]"+Convert.ToDateTime(n.Time).ToString("yyyy-MM-dd hh:mm:ss")
                //   builder.Append(n.yungouma + "<br/>");
                string[] yun = n.yungouma.Split(',');
                //  foreach (string i in yun)
                for (int i = 0; i < yun.Length - 1; i++)
                {
                    builder.Append((Convert.ToInt32(yun[i].ToString().Trim()) + 10000000) + ",");
                    if ((i + 1) % 4 == 0)
                    {
                        builder.Append("<br/>");
                    }
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
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("商品ID" + model.GoodsNum + "云购码详情页");
        builder.Append(Out.Tab("</div>", "<br/>"));
        // Footer();
    }

    //某商品当前购买记录
    protected void buyOneGoods()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号 
        Master.Title = "云购码详情页";
        try
        {
            BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(ptype);
            Master.Title = "商品" + model.GoodsName + "已参与详情页";

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
            builder.Append(model.GoodsName + "已参与详情页<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("云购标识:" + model.Id + "<br/>");
            builder.Append("期数:" + model.periods + "|描述:" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
            builder.Append("留言或备注:" + model.GoodsFrom + "<br/>");
            builder.Append("所需:" + model.statue + "币/每人次" + "(总:" + model.GoodsValue + ")" + "<br/>");
            builder.Append("总价值:" + model.GoodsSell + "<br/>");
            if (model.Identification > 0)
            {
                builder.Append("出售方ID:" + model.Identification + "<br/>");
            }
            {
                builder.Append("出售方:" + "系统" + "<br/>");
            }
            builder.Append("获奖记录列:" + model.Winner + "<br/>");
            //builder.Append("获奖者ID:" + new BCW.BLL.yg_BuyLists().GetUserId(Convert.ToInt64(model.Winner)) + "<br/>");
            builder.Append("上架时间:" + model.Addtime + "<br/>");
            if (model.isDone == 0)
            {
                builder.Append("开奖时间:" + model.RemainingTime + "<br/>");
            }
            builder.Append(Out.Tab("</div>", ""));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "GoodsNum=" + ptype;
            string[] pageValUrl = { "act", "backurl", "ptype" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.yg_BuyLists> listSSCpay = new BCW.BLL.yg_BuyLists().Getyg_BuyListss(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.yg_BuyLists n in listSSCpay)
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
                    string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                    builder.Append(k + "." + name + "(" + n.UserId + ")" + "参与" + n.Counts + "人次的" + "第" + new BCW.BLL.GoodsList().Getperiods(n.GoodsNum) + "期" + model.GoodsName);
                    builder.Append("[消费:" + n.IsGet + "]");
                    builder.Append("时间" + Convert.ToDateTime(n.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff"));
                    builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=seeYun&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;") + "\">" + "[查]" + "</a>");
                    builder.Append("[编号:" + n.Id + "]");
                    k++; //BCW.User.Users.SetUser(n.Winner)
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
        catch { builder.Append(Out.Div("div", "没有相关记录.")); }
        //Footer();
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //已结束商品列表
    private void OpenList()
    {
        Master.Title = "开奖详情列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("已结束");
        builder.Append(Out.Tab("</div>", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "4"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("出售|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">出售</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("进行|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">进行</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("结束");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">结束</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        string Logo = ub.GetSub("img", xmlPath);
        //try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "uid", "backurl" };
            string strWhere = " isDone = 3 ";
            if (uid > 0)
                strWhere = "Id=" + uid + "";
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("行:[期数][商品][来源][商品状态]" + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                int k = 1;
                foreach (BCW.Model.GoodsList n in listSSCpay)
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

                    builder.Append(k + ".(第" + n.periods + "期)");
                    builder.Append("<font color=\"green\">");
                    if (n.isDone == 4)
                    { builder.Append("[已停]"); }
                    if (n.isDone == 3)
                    { builder.Append("[过期|返还]"); }
                    if (n.isDone == 0)
                    { builder.Append("[完结]"); }
                    builder.Append("</font>");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>" + "(编号" + n.Id + ")");
                    BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(n.Winner));
                    //  builder.Append("商品编号:" + n.Id + "商品价值:" + n.GoodsValue + "开奖时间:" + n.OverTime );
                    // builder.Append("查看已参与玩家:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "") + "\">" + n.Number + "</a>");
                    //  builder.Append("中奖用户ID"+"<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">"+model.UserId+"</a>");
                    //  builder.Append("购买量("+model.Counts+")");
                    // if (n.GoodsType == 8 || n.GoodsType == 9 || n.GoodsType == 6||n.GoodsType==7)
                    if (model != null)
                    {
                        if (n.GoodsType == 8 || n.GoodsType == 9 && n.Identification > 0)
                        {
                            builder.Append("[源:" + "玩家物" + "]");
                        }
                        else if (n.GoodsType == 6 || n.GoodsType == 7 || n.Identification > 0) { builder.Append("[源:" + "玩家币" + "]"); }
                        else { builder.Append("[源:" + "系统" + "]"); }
                        if (model.Address == "0") { builder.Append("[未中奖]"); }
                        if (model.Address == "1")
                        {
                            //  builder.Append("(状态:");
                            if (n.GoodsType == 4 || n.GoodsType == 5 || n.GoodsType == 8 || n.GoodsType == 9)//实物商品
                            {

                                if (new BCW.BLL.yg_OrderLists().ExistsGoodsId(Convert.ToInt32(n.Id)))//存在id,玩家已填写快递地址
                                {
                                    BCW.Model.yg_OrderLists order = new BCW.BLL.yg_OrderLists().Getyg_OrderListsForGoodsListId(n.Id);
                                    if (order.isDone == 1)
                                    {
                                        builder.Append("<b>" + "已发货" + "</b>");
                                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=seeorder&amp;id=" + order.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "查看" + "</a>");
                                    }
                                    else if (order.isDone == 0)
                                    {
                                        builder.Append("<b>" + "[已填]" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=goods&amp;id=" + model.Id + "&amp;ptype=" + n.Id + ";backurl=" + Utils.PostPage(1) + "") + "\">" + "[物派]" + "</a></b>");
                                    }
                                    else if (order.Statue == 2 || order.wuliuStatue == 2)
                                    {
                                        builder.Append("[已确认收货,已完结]");
                                    }
                                }

                                else
                                {
                                    builder.Append("[未填写]");
                                }
                            }
                            else
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=paijiang&amp;id=" + model.Id + "&amp;ptype=" + n.Id + ";backurl=" + Utils.PostPage(1) + "") + "\">" + "[派]" + "</a>");
                            }
                        }
                        if (model.Address == "2")
                        {
                            builder.Append("[" + model.UserId + "]");
                            builder.Append("[已领奖]");
                        }
                    }
                    builder.Append(".<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;") + "\">" + "详" + "</a>");
                    k++; //BCW.User.Users.SetUser(n.Winner)
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            string strText = "输入商品ID:/,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜商品记录,kbyg.aspx?act=open&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        //catch { }
        //Footer();
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //进行中商品列表
    private void UnOpenList()
    {
        Master.Title = "进行中商品列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("进行中");
        builder.Append(Out.Tab("</div>", ""));
        string Logo = ub.GetSub("img", xmlPath);
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "3"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("出售|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=check") + "\">出售</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("进行|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">进行</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("结束");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">结束</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        //int newGoods = new BCW.BLL.GoodsList().GetMaxId();
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("行：[期数][名称][来源][进度]");
        builder.Append(Out.Tab("</div>", "<br />"));
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "isDone=1";
            if (uid > 0)
                strWhere = "Id=" + uid + "";
            string[] pageValUrl = { "act", "uid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.GoodsList n in listSSCpay)
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
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=pass&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[停]</a>");
                    //  builder.Append("编号:"+n.Id);
                    builder.Append(k + ".(第" + n.periods + "期)");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                    if (n.GoodsType == 8 || n.GoodsType == 9 && n.Identification > 0)
                    {
                        builder.Append("[源:" + "玩家物ID" + n.Identification + "]");
                    }
                    else if (n.GoodsType == 6 || n.GoodsType == 7 || n.Identification > 0) { builder.Append("[源:" + "玩家" + "ID" + n.Identification + "]"); }
                    else { builder.Append("[源:" + "系统" + "]"); }
                    builder.Append("(编号" + n.Id + ")[进度:" + (double)(n.Number * 100 / n.GoodsValue) + "%]");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;") + "\">" + "详情" + "</a>");
                    //   builder.Append("(第" + n.periods + "期)"+"<a href=\"" + Utils.getUrl("./kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                    //  builder.Append("商品描述:" + n.explain + "所需:" + n.statue + ub.Get("SiteBz") + "/每人次" + "<br/>" + "已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Number + "</a>" + "总需人次:" + n.GoodsValue + "剩余人次:" + (n.GoodsValue - n.Number));
                    //builder.Append("(状态:" + "未开奖)");
                    //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[改]&gt;</a>");
                    //  builder.Append("图片描述：" + "<br/>");
                    //string[] imgNum = n.GoodsImg.Split(',');
                    //foreach (string m in imgNum)
                    //{
                    //    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + Logo + m + "\"  width=\"50\" height=\"40\" alt=\"load\" border=\"10\" border-color=\"#C0C0C0\" />" + "</a>&nbsp;&nbsp;&nbsp;");
                    //}


                    k++; //BCW.User.Users.SetUser(n.Winner)
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            string strText = "输入商品ID:/,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜商品记录,kbyg.aspx?act=unopen&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        catch { }
        // Footer();
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

    }

    //填写修改页
    private void selectAll()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("填写商品参数");
        builder.Append(Out.Tab("</div>", "<br />"));
        long id = Convert.ToInt64(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        long ptype = Convert.ToInt64(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(id);
        if (ptype == 2)
        {
            builder.Append("原名称:" + model.GoodsName + "<br/>");
            builder.Append("填写新商品名称:");

        }
        string strText = "输入修改后的值:/,";
        string strName = "id,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜商品记录,kbyg.aspx?act=selsec&amp;,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //switch (ptype)
        //{
        //    case 1:
        //        UpdateGoodsAll();
        //        break;
        //    case 2://开奖商品
        //        OpenList();
        //        break;
        //    case 3://查询未开奖商品
        //        UnOpenList();
        //        break;
        //    default:
        //        UpdateGoodsAll();
        //        break;

        //}
    }

    //修改商品参数
    private void UpdateGoodsAll()
    {
        Master.Title = "修改商品参数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("修改商品参数");
        long id = Convert.ToInt64(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("</div>", ""));
        string strText = "输入商品ID:/,";
        string strName = "id,backurl";
        string strType = "num,hidden";
        string strValu = "" + id + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜商品记录,kbyg.aspx?act=update&amp;,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (id == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请输入商品的Id." + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //builder.Append(id);
            if (!new BCW.BLL.GoodsList().Exists(id))
            {
                Utils.Error("不存在该商品记录.", "");
            }
            else
            {
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                if (Utils.ToSChinese(ac) == "确定修改")
                {
                    if (info == "")
                    {
                        string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,200}$", "商品名称限1-20字内");
                        string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,2000}$", "商品描述限2000字内");
                        string GoodsImg = Utils.GetRequest("GoodsImg", "post", 3, @"^[^\^]{0,2000}$", "图片输入错误");
                        //  string GoodsValue = Utils.GetRequest("GoodsValue", "post", 2, @"^[1-9]\d*$", "所需人次输入出错");
                        // string Number = Utils.GetRequest("Number", "post", 4, @"^[0-9]\d*$", "当前参与人次错误(整数)");
                        string periods = Utils.GetRequest("periods", "post", 2, @"^[0-9]\d*$", "商品期数出错(整数)");
                        string GoodsTpye = Utils.GetRequest("GoodsTpye", "post", 3, @"^[0-9]\d*$", "商品类型出错");
                        // string statue = Utils.GetRequest("statue", "post", 2, @"^[1-9]\d*$", "所需每人次填写错误");
                        //string isDone = Utils.GetRequest("isDone", "post", 2, @"^[0-9]$", "当前状态填写错误");
                        string beizhu = Utils.GetRequest("beizhu", "post", 2, @"^[^\^]{1,20}$", "备注填写错误");
                        string overtime = Utils.GetRequest("overtime", "post", 2, @"^[0-9]\d*$", "出售时长输入错误(整数1)");
                        // string identification = Utils.GetRequest("identification", "post", 2, @"^[^\^]{1,20}$", "来源填写错误");
                        // string AddTime = Utils.GetRequest("AddTime", "post", 2, @"^[^\^]{1,20}$", "商品名称限1-20字内");
                        Master.Title = "确认修改商品参数";
                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        builder.Append("确定修改商品参数吗?");
                        builder.Append(Out.Tab("</div>", ""));
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("商品名称:" + GoodsName + "<br/>");
                        builder.Append("商品介绍:" + explain + "<br/>");
                        builder.Append("商品图片:" + GoodsImg + "<br/>");
                        builder.Append("商品时长:" + overtime + "<br/>");
                        builder.Append("商品备注:" + beizhu + "<br/>");
                        builder.Append("商品期数:" + periods + "<br/>");
                        builder.Append(Out.Tab("</div>", ""));
                        //  string Text = "当前商品名称(可修改):/,商品介绍(可修改):/,商品图片:/,商品所需人次(仅显示):/,当前参与人次(仅显示):/,商品期数(仅显示):/,商品类型(仅显示):/,所需每人次(仅显示):/,商品状态(仅显示):/,玩家留言(可修改):/,来源(仅显示):/,中奖记录列(仅显示):/,添加时间(仅显示):/,出售时长(仅显示):/,开奖时间(仅显示):/,剩余云购码(仅显示):/,";
                        string Text = ",,,,,,";
                        string Name = "GoodsName,explain,GoodsImg,GoodsTpye,overtime,beizhu,periods";
                        string Type = "hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                        string Valu = "" + GoodsName + "'" + explain + "'" + GoodsImg + "'" + statue + "'" + overtime + "'" + beizhu + "'" + periods + "'";
                        string Empt = "false,false,false,false,false,false,false,false";
                        // string Name = "GoodsName,explain,GoodsImg,GoodsValue,Number,periods,GoodsTpye,statue,isDone,beizhu,identification,getID,AddTime,overtime,GetTime,Stock";
                        //  string Type = "textarea,textarea,text,text,text,text,select,text,select,textarea,text,text,text,text,text,text";
                        // string Valu = "" + GoodsName + "'" + explain + "'" + GoodsImg + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + statue + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + "无修改" + "'" + Utils.getPage(0) + "";
                        // string Empt = "false,true,true,true,false,false,0|循环|1|不循环|2|币值循环|3|币值不循环|4|物品循环|5|物品不循环|6|玩家币值循环|7|玩家币值不循环|8|玩家实物循环|9|玩家实物不循环 ,false,0|已结束|1|进行中|2|开奖中|3|已停止|4|未审核|5|待审核|6|用户已取消出售,false,false,false,false,false,false,false";
                        string Idea = "";
                        string Othe = "确定修改,kbyg.aspx?info=ok&amp;act=update&amp;id=" + id + "&amp;,post,1,blue";
                        builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=edit&amp;id=" + id + "&amp;") + "\">先看看吧</a>");
                        builder.Append(Out.Tab("</div>", "<br/>"));
                    }
                    else
                    {
                        string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,200}$", "商品名称限1-20字内");
                        string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,2000}$", "商品描述限200字内");
                        string GoodsImg = Utils.GetRequest("GoodsImg", "post", 3, @"^[^\^]{0,2000}$", "图片输入错误");
                        //string GoodsValue = Utils.GetRequest("GoodsValue", "post", 2, @"^[1-9]\d*$", "所需人次输入出错");
                        //string Number = Utils.GetRequest("Number", "post", 4, @"^[0-9]\d*$", "当前参与人次错误(整数)");
                        string periods = Utils.GetRequest("periods", "post", 4, @"^[1-9]\d*$", "商品期数出错(整数)");
                        string GoodsTpye = Utils.GetRequest("GoodsTpye", "post", 3, @"^[0-9]\d*$", "商品类型出错");
                        //string statue = Utils.GetRequest("statue", "post", 2, @"^[1-9]\d*$", "所需每人次填写错误");
                        //string isDone = Utils.GetRequest("isDone", "post", 2, @"^[0-9]$", "当前状态填写错误");
                        string beizhu = Utils.GetRequest("beizhu", "post", 2, @"^[^\^]{1,20}$", "备注填写错误");
                        string overtime = Utils.GetRequest("overtime", "post", 4, @"^[0-9]\d*$", "出售时长输入错误(整数)");
                        // string identification = Utils.GetRequest("identification", "post", 2, @"^[^\^]{1,20}$", "来源填写错误");
                        // string AddTime = Utils.GetRequest("AddTime", "post", 2, @"^[^\^]{1,20}$", "商品名称限1-20字内");
                        try
                        {
                            BCW.Model.GoodsList mod = new BCW.BLL.GoodsList().GetGoodsList(id);
                            mod.GoodsName = GoodsName;
                            mod.explain = explain;
                            mod.GoodsImg = GoodsImg;
                            //  mod.GoodsValue =Convert.ToInt64(GoodsValue);
                            //   mod.Number = Convert.ToInt64(Number);
                            mod.periods = Convert.ToInt32(periods);
                            mod.GoodsType = Convert.ToInt32(GoodsTpye);
                            mod.OverTime = DateTime.Now.AddDays(Convert.ToInt32(overtime));
                            //  mod.statue = Convert.ToInt32(statue);
                            //  mod.isDone = Convert.ToInt32(isDone);
                            mod.GoodsFrom = beizhu;
                            //  mod.Identification =Convert.ToInt32(identification);
                            new BCW.BLL.GoodsList().Update(mod);
                            builder.Append(Out.Tab("<div>", "<br/>"));
                            builder.Append("修改成功" + "<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + id + "&amp;") + "\">返回继续修改</a>"); builder.Append(Out.Tab("</div>", "<br/>"));
                        }
                        catch
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("修改失败" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=update&amp;id=" + id + "&amp;") + "\">返回查看</a>");
                            builder.Append(Out.Tab("</div>", "<br/>"));
                        }
                    }

                }
                else
                {
                    BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(id);
                    TimeSpan passtime = Convert.ToDateTime(model.OverTime) - Convert.ToDateTime(model.Addtime);
                    double dayss = Math.Round(passtime.TotalDays, 0);
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("当前商品编号:" + id);
                    builder.Append(Out.Tab("</div>", ""));
                    string Text = "当前商品名称(可修改):/,商品介绍(可修改):/,商品图片:/,商品所需人次(仅显示):/,当前参与人次(仅显示):/,商品期数:/,商品类型(仅显示):/,所需每人次(仅显示):/,商品状态(仅显示):/,玩家留言(可修改):/,来源(仅显示):/,中奖记录列(仅显示):/,添加时间(仅显示):/,出售时长(天数):/,开奖时间(仅显示):/,剩余云购码(仅显示):/,";
                    string Name = "GoodsName,explain,GoodsImg,GoodsValue,Number,periods,GoodsTpye,statue,isDone,beizhu,identification,getID,AddTime,overtime,GetTime,Stock";
                    string Type = "textarea,textarea,text,text,text,text,select,text,select,textarea,text,text,text,text,text,text";
                    string Valu = "" + model.GoodsName + "'" + model.explain + "'" + model.GoodsImg + "'" + model.GoodsValue + "'" + model.Number + "'" + model.periods + "'" + model.GoodsType + "'" + model.statue + "'" + model.isDone + "'" + model.GoodsFrom + "'" + model.Identification + "'" + model.Winner + "'" + model.Addtime + "'" + dayss + "'" + model.RemainingTime + "'" + model.StockYungouma + "'" + Utils.getPage(0) + "";
                    string Empt = "false,true,true,true,false,true,0|循环|1|不循环|2|币值循环|3|币值不循环|4|物品循环|5|物品不循环|6|玩家币值循环|7|玩家币值不循环|8|玩家实物循环|9|玩家实物不循环 ,false,0|已结束|1|进行中|2|开奖中|3|已停止|4|未审核|5|待审核|6|用户已取消出售,false,false,false,false,false,false,false";
                    string Idea = "/";
                    string Othe = "确定修改|reset,kbyg.aspx?act=update&amp;id=" + id + "&amp;,post,1,red|blue";
                    builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=img&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">重新添加图片</a><br/>");
                    builder.Append("温馨提示:" + "来源0为系统发布." + "<br/>");
                    //   builder.Append("." + "<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //添加云购商品
    private void AddGoods()
    {
        Master.Title = "添加云购商品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("添加云购商品");
        builder.Append(Out.Tab("</div>", ""));
        // builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        BCW.Model.GoodsList model = new BCW.Model.GoodsList();
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,20}$", "商品名称限1-20字内");
            string explain = Utils.GetRequest("explain", "post", 2, @"^[^\^]{0,2000}$", "商品描述限2000字内");
            string beizhu = Utils.GetRequest("beizhu", "post", 2, @"^[^\^]{1,20}$", "商品备注限20字内");
            string GoodsType = Utils.GetRequest("GoodsType", "post", 2, @"^[0-9]$", "商品状态选择出错");
            string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[0-9]\d*$", "商品总价值填写错误(整数)");
            string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[1-9]\d*$", "商品最大购买量填写错误(整数)");
            int times = int.Parse(Utils.GetRequest("times", "post", 2, @"^[0-9]\d*$", "时长选择出错"));
            DateTime over = DateTime.Now;
            over = DateTime.Now.AddDays(times);
            // DateTime now=new DateTime();
            model.GoodsName = GoodsName;
            model.explain = explain;
            model.GoodsSell = Convert.ToInt32(GoodsValue);
            model.GoodsImg = "0";//置空
            model.ImgCounts = 0;
            model.periods = 1;//第一期
            model.Number = 0;//置空
            model.GoodsValue = Convert.ToInt64(GoodsNum);
            model.GoodsType = Convert.ToInt32(GoodsType);
            model.Winner = "0";//置空
            model.lotteryTime = DateTime.Now;
            model.RemainingTime = DateTime.Now;
            model.OverTime = over;
            model.isDone = 1;
            model.GoodsFrom = beizhu;
            model.Identification = 0;//0为系统发布，其他为用户id
            model.StockYungouma = "0";
            model.statue = Convert.ToInt32(Convert.ToInt32(GoodsValue) / Convert.ToInt32(GoodsNum));
            model.Addtime = DateTime.Now;
            model.GoodsSell = Convert.ToInt32(GoodsValue);
            try
            {
                int id = new BCW.BLL.GoodsList().Add(model);
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("添加" + "<font color=\"red\" >" + GoodsName + "</font>" + "成功,是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=img&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=add&amp;") + "\">否</a><br/>");
                builder.Append(Out.Tab("</div>", ""));
                // Utils.Success("添加商品", "添加成功，正在进入添加图片..", Utils.getUrl("kbyg.aspx?act=img&amp;ac=1;id=" + Convert.ToInt64(model.Id) + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            catch (Exception e)
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("添加出错,请重新添加");
                builder.Append(Out.Tab("</div>", ""));
                // builder.Append(e);
            }
            //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);    
        }
        else
        {
            //  builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.Div("title", ""));
            string strText = "商品名称(限20字):/,商品描述(限1000字):/,最大购买量:/,商品类型:/,商品总价值(酷币):/,上架时长:/,商品备注(限20字):/,";
            string strName = "GoodsName,explain,GoodsNum,GoodsType,GoodsValue,times,beizhu";
            string strType = "text,textarea,num,select,num,select,textarea,act";
            string strValu = "" + "标题" + "'" + "商品详情..." + "'" + 100 + "'" + 0 + "'" + 10000 + "'" + 3 + "'" + "备注" + "'" + 8 + "'";
            string strEmpt = "false,true,true,1|单次币值|0|循环币值|5|单次物品|4|循环物品,false,1|1天|7|7天|10|10天|15|15天|20|20天|25|25天|30|一个月|60|两个月,false";
            string strIdea = "/";
            string strOthe = "确定添加|reset,kbyg.aspx?act=add,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />商品循环是指商品开奖后自动生成新一期，上架后请留意第一期开奖情况<br />如果设置0则全部不限制");


            //   builder.Append(Out.Tab("</div>", ""));
            //  builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            //  builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //增加商品图片
    private void AddImg()
    {
        Master.Title = "添加商品图片";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("添加商品图片" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号   
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
        {
            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入添加图片...", "kbyg.aspx?act=img&amp;id=" + id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "2");
        }
        try
        {
            BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(id));
            // BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(291);
            int max = 5;// Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定添加")
            {
                //遍历File表单元素
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                //  builder.Append(files.Count);
                try
                {
                    //string GetFiles = string.Empty;
                    string sqlimg = "";
                    for (int iFile = 0; iFile < files.Count; iFile++)
                    {
                        //检查文件扩展名字
                        System.Web.HttpPostedFile postedFile = files[iFile];
                        string fileName, fileExtension;
                        fileName = System.IO.Path.GetFileName(postedFile.FileName);
                        if (fileName != "")
                        {
                            fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                            string DirPath = string.Empty;
                            // string prevDirPath = string.Empty;
                            string Path = "/bbs/game/img/kbyg_img/";
                            if (FileTool.CreateDirectory(Path, out DirPath))
                            {
                                //上传数量限制
                                //生成随机文件名
                                fileName = DT.getDateTimeNum() + iFile + fileExtension;
                                sqlimg += DirPath + fileName + ",";
                                string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                                postedFile.SaveAs(SavePath);
                            }
                        }
                    }
                    //builder.Append(sqlimg);
                    model.GoodsImg = sqlimg;
                    new BCW.BLL.GoodsList().Update(model);
                    Utils.Success(Gamename + "添加商品", "添加图片成功，正在返回添加商品页..", Utils.getUrl("kbyg.aspx?act=add&amp;backurl=" + Utils.getPage(0) + ""), "3");
                }
                catch
                { //builder.Append("1");
                }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你将为新增的商品:" + model.GoodsName + ",添加图片" + "<br/>");///////////////////////
                // builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("上传允许格式:" + "jpg,ogg,png" + "<br />");
                builder.Append("每个文件限" + 2 + "K");
                builder.Append(Out.Tab("</div>", "<br />"));
                int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
                if (num > max)
                    num = max;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("上传:");
                for (int i = 1; i <= max; i++)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=img&amp;id=" + id + "&amp;num=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>" + i + "</b></a> ");
                }
                builder.Append("个");
                builder.Append(Out.Tab("</div>", ""));
                string ssUpType = string.Empty;
                string ssText = string.Empty;
                string ssName = string.Empty;
                string ssType = string.Empty;
                string ssValu = string.Empty;
                string ssEmpt = string.Empty;
                string ssIdea = string.Empty;
                string ssOthe = string.Empty;
                for (int i = 0; i < num; i++)
                {
                    string y = ",";
                    if (num == 1)
                    {
                        ssText = ssText + y + "选择" + ssUpType + "附件:/,";
                    }
                    else
                    {
                        ssText = ssText + y + "" + ssUpType + "第" + (i + 1) + "个附件:/,";
                    }
                    ssName = ssName + y + "file" + (i + 1) + y;
                    ssType = ssType + y + "file" + y;
                    ssValu = ssValu + "''";
                    ssEmpt = ssEmpt + y + y;
                }
                string strUpgroup = string.Empty;
                strUpgroup = "" + strUpgroup;
                ssText = ssText + Utils.Mid(strText, 1, strText.Length) + "," + ",";
                ssName = ssName + Utils.Mid(strName, 1, strName.Length) + ",act";
                ssType = ssType + Utils.Mid(strType, 1, strType.Length) + "hidden,hidden";
                ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "ac" + "'img";
                ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,";
                ssIdea = "/";
                ssOthe = "确定添加|reset,kbyg.aspx?id=" + id + "&amp;,post,2,red|blue";
                builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">不添加照片，返回管理首页</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch (Exception e) { builder.Append(e); }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("添加商品图片" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        //  Footer();
    }

    //云购设置
    private void KbygSet()
    {
        Master.Title = Gamename + "设置";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/myyg.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string KbygGonggao = Utils.GetRequest("KbygGonggao", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            // string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string timeToOpen = Utils.GetRequest("timeToOpen", "post", 2, @"^[1-9]\d*$", "首页最大显示商品数填写错误");
            string persan = Utils.GetRequest("persan", "post", 4, @"^[1-9]\d*$", "手续费百分比%" + "填写错误(整数)");
            string UserSellGetPersan = Utils.GetRequest("UserSellGetPersan", "post", 4, @"^[0-9]\d*$", "会员出售获得人次填写错误(整数)");
            string Expir = Utils.GetRequest("Expir", "post", 4, @"^[1-9]\d*$", "购买防刷(秒)" + "填写错误");
            string addGoodsValue = Utils.GetRequest("addGoodsValue", "post", 2, @"^[1-9]\d*$", "首页最大显示商品数填写错误");
            // string imgCount = Utils.GetRequest("imgCount", "post", 2, @"^[1-9]\d*$", "首页最大显示图片数填写错误");
            string KbygTop = Utils.GetRequest("KbygTop", "post", 3, @"^[\s\S]{1,2000}$", "顶部Ubb限2000字内");
            // string daletou = Utils.GetRequest("daletou", "post", 2, @"^[0-1]$", "大乐透功能状态选择出错");
            string KbygIndexStyle = Utils.GetRequest("KbygIndexStyle", "post", 2, @"^[0-3]$", "首页风格选择出错");
            string sell = Utils.GetRequest("sell", "post", 2, @"^[0-1]$", "会员出售功能状态选择出错");
            string SellSmall = Utils.GetRequest("SellSmall", "post", 2, @"^[1-9]\d*$", "首页最大显示商品数填写错误");
            string BigSell = Utils.GetRequest("BigSell", "post", 2, @"^[1-9]\d*$", "首页最大显示商品数填写错误");
            // string newThings = Utils.GetRequest("newThings", "post", 2, @"^[0-1]$", "新鲜玩意功能状态选择出错");
            //  string ceshi = Utils.GetRequest("ceshi", "post", 2, @"^[0-1]$", "测试功能状态选择出错");
            xml.dss["KbygName"] = Name;
            xml.dss["KbygNotes"] = Notes;
            xml.dss["KbygLogo"] = Logo;
            xml.dss["KbygTop"] = KbygTop;
            xml.dss["timeToOpen"] = timeToOpen;
            xml.dss["KbygGonggao"] = KbygGonggao;
            //  xml.dss["KbygStatus"] = Status;
            xml.dss["KbygPersan"] = persan;
            xml.dss["KbygExpir"] = Expir;
            xml.dss["UserSellGetPersan"] = UserSellGetPersan;
            xml.dss["addGoodsValue"] = addGoodsValue;
            // xml.dss["imgCount"] = imgCount;
            //  xml.dss["isOpenMiaoBianFuhao"] = daletou;
            xml.dss["isOpenIWantSell"] = sell;
            xml.dss["SellSmall"] = SellSmall;
            xml.dss["BigSell"] = BigSell;
            xml.dss["KbygIndexStyle"] = KbygIndexStyle;
            // xml.dss["isOpenNewThings"] = newThings;
            //  xml.dss["ceshi"] = ceshi;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + Gamename + "设置", "设置成功，正在返回..", Utils.getUrl("kbyg.aspx?act=set&amp;backurl=" + Utils.getPage(1) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", Gamename + "设置"));
            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏公告:/,顶部Ubb:/,首页风格选择:/,设置开奖分钟数:/,系统商品手续费千分比:/,玩家购买防刷(秒):/,玩家最小出售值:/,玩家最大出售值:/,玩家出售添加人次:/,玩家出售获得人次:/,会员出售功能:/,";
            string strName = "Name,Notes,Logo,KbygGonggao,KbygTop,KbygIndexStyle,timeToOpen,persan,Expir,SellSmall,BigSell,addGoodsValue,UserSellGetPersan,sell,backurl";
            string strType = "text,text,text,text,text,select,select,num,num,text,text,text,text,select,hidden";
            string strValu = xml.dss["KbygName"] + "'" + xml.dss["KbygNotes"] + "'" + xml.dss["img"] + "'" + xml.dss["KbygGonggao"] + "'" + xml.dss["KbygTop"] + "'" + xml.dss["KbygIndexStyle"] + "'" + xml.dss["timeToOpen"] + "'" + xml.dss["KbygPersan"] + "'" + xml.dss["KbygExpir"] + "'" + xml.dss["SellSmall"] + "'" + xml.dss["BigSell"] + "'" + xml.dss["addGoodsValue"] + "'" + xml.dss["UserSellGetPersan"] + "'" + xml.dss["isOpenIWantSell"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,true,true,1|风格1|2|风格2,1|1分钟|2|2分钟|3|3分钟|5|5分钟,true,true,true,true,true,true,0|开启|1|关闭,";
            string strIdea = "/";
            string strOthe = "确定修改|reset,kbyg.aspx?act=set,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            // builder.Append("温馨提示:<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //排行榜
    private void CountList()
    {
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //  builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        // builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("榜单管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        // int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[^\^]{0,200}$", "20"));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div>", ""));
        string strWhere = string.Empty;
        int pageIndex;
        int recordCount;
        //  int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string sOrder = "";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1)
        {
            builder.Append("<b>" + "运气榜" + "</b>|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=count&amp;ptype=1&amp;") + "\"><b>运气榜</b></a>|");

        }
        if (ptype == 2)
        {
            builder.Append("<b>" + "参与榜" + "</b>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=count&amp;ptype=2&amp;") + "\"><b>参与榜</b></a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        if (ptype == 1)//运气榜
        {
            if (start.Length < 2)
            {
                strWhere = "Address<>0 group by UserId order by Address desc ";
            }
            else
            {
                strWhere = "Address<>0" + "and BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'" + "group by UserId order by Address desc";
            }
            // string str = "Address<>0 group by UserId order by Address desc ";
            string str = strWhere;
            DataSet ds = new BCW.BLL.yg_BuyLists().GetList("UserId,count(Address)as Address", str);
            //  sum = counts;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {

                    if (i % 2 == 0)
                    { builder.Append(Out.Tab("<div class=\"text\">", "<br/>")); }
                    else
                    {
                        if (i == 1)
                            builder.Append(Out.Tab("<div>", "<br/>"));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    {
                        builder.Append((i + 1) + ".");
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"]));
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[koo + i]["UserId"] + "") + "\">" + mename + "</a>");
                        builder.Append("(" + ds.Tables[0].Rows[koo + i]["UserId"] + ")");
                        builder.Append("累计获奖");
                        builder.Append(ds.Tables[0].Rows[koo + i]["Address"] + "次");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
        }
        else
            if (ptype == 2)//参与榜
            {
                if (start.Length < 2)
                {
                    strWhere = "Address<5 group by UserId order by Address desc ";
                }
                else
                {
                    strWhere = " Address<5" + "and BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'" + "group by UserId order by Address desc";
                }
                // strWhere = "Address< 3 " + "and BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'" + "group by UserId order by Address desc";
                // string str = "Address<3 group by UserId order by Address desc ";
                string str = strWhere;
                DataSet ds = new BCW.BLL.yg_BuyLists().GetList("UserId,count(DISTINCT GoodsNum)as Address", str);//00条记录
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int i = 0; i < skt; i++)
                    {
                        if (i % 2 == 0)
                        { builder.Append(Out.Tab("<div class=\"text\">", "<br/>")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div>", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        //if (sum <= i)
                        //{ break; }
                        //else
                        {
                            builder.Append((i + 1) + ".");
                            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"]));
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[koo + i]["UserId"] + "") + "\">" + mename + "</a>");
                            builder.Append("(" + ds.Tables[0].Rows[koo + i]["UserId"] + ")");
                            builder.Append("累计参与");
                            builder.Append(ds.Tables[0].Rows[koo + i]["Address"] + "次");
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                { builder.Append(Out.Tab("</div>", "无相关记录")); }
            }
        if (start.Length < 5)
        {
            start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //if (ptype == 1)
            //{
            //    strWhere = "Address<> 0 " +"and BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'"+"group by UserId order by Address desc";
            //}
            //else if (ptype == 2)
            //{
            //    strWhere = "Address< 3 " + "and BuyTime> '" + Convert.ToDateTime(start) + "' and BuyTime< '" + Convert.ToDateTime(down) + "'" + "group by UserId order by Address desc";
            //}
            //else
            //{
            //    strWhere = "Address<>0 group by UserId order by Address desc ";
            //}
        }
        string strText1 = "开始时间:,结束时间:,";
        string strName1 = "start,down,backurl";
        string strType1 = "text,text,hidden";
        string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,true,false";
        string strIdea1 = "/";
        string strOthe1 = "按时间搜索,kbyg.aspx?act=count&amp;ptype=" + ptype + "&amp;,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:输入榜单数量显示参与者");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //删除记录
    private void DelPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除购买记录";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该记录吗?");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.yg_BuyLists().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.yg_BuyLists().Delete(id);
            Utils.Success("删除记录", "删除成功..", Utils.getPage("kbyg.aspx"), "1");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", ""));
    }

    //重置游戏
    private void ResetPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }

        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置" + Gamename + "吗，重置后将删除所有记录！");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_yg_BuyLists");
            new BCW.Data.SqlUp().ClearTable("tb_yg_OrderLists");
            new BCW.Data.SqlUp().ClearTable("tb_GoodsList");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("kbyg.aspx"), "1");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(Gamename + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //盈利分析
    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "管理</a>&gt;");
        builder.Append("赢利分析" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        long KbygPersan = Convert.ToInt64(ub.GetSub("KbygPersan", xmlPath));
        try
        {
            //今天赢利        
            string str = "isDone=0  and Year(RemainingTime) = " + DateTime.Now.Year + "" + " and Month(RemainingTime) = " + DateTime.Now.Month + "and Day(RemainingTime) = " + DateTime.Now.Day;
            DataSet ds = new BCW.BLL.GoodsList().GetList("*", str);
            long sum = 0;
            // long persan = 1000;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToInt64(ds.Tables[0].Rows[i]["Identification"]) > 0)
                    {
                        sum += (Convert.ToInt64(ds.Tables[0].Rows[i]["statue"]) * (Convert.ToInt64(KbygPersan)));
                    }
                    sum += (Convert.ToInt64(ds.Tables[0].Rows[i]["GoodsSell"]) * (Convert.ToInt64(KbygPersan)) / 1000);
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("今天赢利:" + (sum) + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        catch { builder.Append(Out.Div("div", "无相关的今日赢利.")); }
        try
        {
            //昨日赢利
            string strYesterday = "isDone=0 and Year(RemainingTime) = " + (DateTime.Now.Year) + "and Month(RemainingTime) = " + DateTime.Now.Month + " and Day(RemainingTime) = " + (DateTime.Now.Day - 1) + "";
            DataSet dsYesterday = new BCW.BLL.GoodsList().GetList("*", strYesterday);
            long sumYesterday = 0;
            if (dsYesterday != null && dsYesterday.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsYesterday.Tables[0].Rows.Count; i++)
                {

                    // sumYesterday += (Convert.ToInt64(dsYesterday.Tables[0].Rows[i]["GoodsValue"])) * (Convert.ToInt64(dsYesterday.Tables[0].Rows[i]["statue"])) * (Convert.ToInt64(KbygPersan)) / 1000;
                    if (Convert.ToInt64(dsYesterday.Tables[0].Rows[i]["Identification"]) > 0)
                    {
                        sumYesterday += (Convert.ToInt64(dsYesterday.Tables[0].Rows[i]["statue"]) * (Convert.ToInt64(KbygPersan)));
                    }
                    sumYesterday += (Convert.ToInt64(dsYesterday.Tables[0].Rows[i]["GoodsSell"]) * (Convert.ToInt64(KbygPersan)) / 1000);

                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("昨日赢利:" + (sumYesterday) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        catch { builder.Append(Out.Div("div", "无相关的昨日赢利.")); }
        try
        {
            //本月赢利"
            string strMonth = "isDone=0 and Year(RemainingTime) = " + (DateTime.Now.Year) + " and Month(RemainingTime) = " + (DateTime.Now.Month) + "";
            DataSet dsMonth = new BCW.BLL.GoodsList().GetList("*", strMonth);
            long sumMonth = 0;
            if (dsMonth != null && dsMonth.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsMonth.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToInt64(dsMonth.Tables[0].Rows[i]["Identification"]) > 0)
                    {
                        sumMonth += (Convert.ToInt64(dsMonth.Tables[0].Rows[i]["statue"]) * (Convert.ToInt64(KbygPersan)));
                    }
                    sumMonth += (Convert.ToInt64(dsMonth.Tables[0].Rows[i]["GoodsSell"]) * (Convert.ToInt64(KbygPersan)) / 1000);
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本月赢利:" + (sumMonth) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        catch { builder.Append(Out.Div("div", "无相关的本月赢利.")); }
        try
        {
            //上月赢利
            string strLastMonth = "isDone=0 " + "and Year(RemainingTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(RemainingTime) = " + (DateTime.Now.Month - 1) + " ";
            DataSet dsLastMonth = new BCW.BLL.GoodsList().GetList("*", strLastMonth);
            long sumLastMonth = 0;
            if (dsLastMonth != null && dsLastMonth.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsLastMonth.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToInt64(dsLastMonth.Tables[0].Rows[i]["Identification"]) > 0)
                    {
                        sumLastMonth += (Convert.ToInt64(dsLastMonth.Tables[0].Rows[i]["statue"]) * (Convert.ToInt64(KbygPersan)));
                    }
                    sumLastMonth += (Convert.ToInt64(dsLastMonth.Tables[0].Rows[i]["GoodsSell"]) * (Convert.ToInt64(KbygPersan)) / 1000);
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上月赢利:" + (sumLastMonth) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        catch { builder.Append(Out.Div("div", "无相关的上月赢利.")); }
        string strAll = "isDone=0";
        try
        {
            if (start != "0")
            {
                strAll = strAll + " and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
            }
            else
            {
                start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        catch { builder.Append(Out.Div("div", "输入的时间格式有误,统计盈利总计失败.")); }
        try
        {

            DataSet dsAll = new BCW.BLL.GoodsList().GetList("*", strAll);
            long sumAll = 0;
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToInt64(dsAll.Tables[0].Rows[i]["Identification"]) > 0)
                    {
                        sumAll += (Convert.ToInt64(dsAll.Tables[0].Rows[i]["statue"]) * (Convert.ToInt64(KbygPersan)));
                    }
                    sumAll += (Convert.ToInt64(dsAll.Tables[0].Rows[i]["GoodsSell"]) * (Convert.ToInt64(KbygPersan)) / 1000);
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("赢利总计:" + sumAll + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        catch { builder.Append(Out.Div("div", "无相关的赢利总计.")); }
        //  builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string strText1 = "开始时间:,结束时间:,";
        string strName1 = "start,down,backurl";
        string strType1 = "text,text,hidden";
        string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,true，false";
        string strIdea1 = "/";
        string strOthe1 = "搜索总盈利,kbyg.aspx?act=stat,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getPage("kbyg.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //倒计时完毕
    private void isTimeToOpenNew()
    {
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "isDone=2";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        try
        {
            // 开始读取列表
            IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsListsForGoodsOpen(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.GoodsList n in listSSCpay)
                {
                    try
                    {
                        int meid = Convert.ToInt32(n.Identification);
                        DateTime now = DateTime.Now;
                        DateTime dt = Convert.ToDateTime(n.RemainingTime);
                        if (DateTime.Compare(now, dt) > 0)
                        {
                            n.isDone = 0;
                            n.StockYungouma = "已完结";
                            if (n.Identification != 0)//用户商品
                            {
                                string mename = new BCW.BLL.User().GetUsName(meid);
                                long gold = Convert.ToInt64(n.statue * (n.GoodsValue - 10));
                                string bzText = ub.Get("SiteBz");
                                long UserSellGetPersan = Convert.ToInt64(ub.GetSub("UserSellGetPersan", xmlPath));
                                long get = Convert.ToInt64(n.statue * UserSellGetPersan);
                                long Usergold = new BCW.BLL.User().GetGold(meid);
                                if (n.GoodsType % 2 == 0)//循环商品
                                {
                                    if (gold > Usergold)
                                    {
                                        //发内线
                                        string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]申请的" + n.GoodsName + "" + "因你的余额不足已停了" + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                                        new BCW.BLL.Guest().Add(GameId, meid, mename, strLog);
                                    }
                                    //循环扣费
                                    else
                                    {
                                        //返回盈利到用户并重新扣费
                                        // new BCW.BLL.User().UpdateiGold(meid, gold, "云购出售商品返回结果");
                                        // new BCW.BLL.User().UpdateiGold(meid, -gold, "云购出售商品消费");
                                        //  new BCW.BLL.Action().Add(31, 0, meid, mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]自动扣费消费了" + (-gold) + "币");
                                        new BCW.BLL.User().UpdateiGold(meid, get, "云购出售商品标识" + n.Id + "盈利");
                                    }
                                }
                                else
                                {
                                    new BCW.BLL.User().UpdateiGold(meid, get, "云购出售商品标识" + n.Id + "盈利");
                                }
                            }
                            //else//系统商品
                            //{ 
                            //} 
                            BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(n.Winner));
                            string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                            new BCW.BLL.GoodsList().Update(n);//更新获奖id，获奖码，到该商品                          
                            string sstrLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]购买的" + n.GoodsName + "" + "中奖了" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                            new BCW.BLL.Action().Add(GameId, GameId, meid, name, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]获奖了" + "第" + n.periods + "期" + (n.GoodsName));
                            //发内线
                            new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), name, sstrLog);
                        }
                    }
                    catch
                    {
                        new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "设置倒计时失败,请查看刷新机进行开奖)" + "错误码002");//向系统发内线
                    }
                }

                k++;
            }
            //else
            //{
            //    builder.Append("暂未有倒计时商品..");
            //}
        }
        catch (Exception e)
        {
            System.Console.WriteLine("传递过来的异常值为：{0}", e);
            //throw
            //  builder.Append(Out.Div("div", "设置倒计时失败！.."));
        }

    }

    //开奖设置倒计时
    private void isTimeToOpen()
    {
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "isDone=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        try
        {
            // 开始读取列表
            IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsListsForGoodsOpen(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.GoodsList n in listSSCpay)
                {
                    // if(n.Identification==0)//系统商品开奖
                    // if(n.Identification！=0)//用户商品开奖
                    if (n.GoodsValue == n.Number)
                    {
                        try
                        {
                            long sum = 0;
                            int start = 0;
                            string str = "";
                            DataSet ds = new BCW.BLL.yg_BuyLists().GetListTop("top 100 *", str);//获取购买的最后100条记录
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                start = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);//最后的记录开始往前数100
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    sum = Convert.ToInt64(sum + (Convert.ToInt32(Convert.ToDateTime(ds.Tables[0].Rows[i]["BuyTime"]).ToString("hhmmssfff"))));
                                }
                            }
                            int yushu = Convert.ToInt32(sum % Convert.ToInt64(n.GoodsValue));
                            long userGet = 0;
                            userGet = new BCW.BLL.yg_BuyLists().GetUserId_yg_BuyListsForYungouma(Convert.ToInt32(n.Id), (Convert.ToInt32(yushu)));//返回中奖id记录的一列
                            BCW.Model.yg_BuyLists mod = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(userGet);
                            if (userGet != 0)
                            {
                                n.Winner = userGet.ToString();// +yushu.ToString();// +"," + start; //记录中奖Id，云购吗，100条记录数最后一条记录码
                                n.ImgCounts = start;
                                n.isDone = 2;
                                DateTime dt = DateTime.Now;
                                n.RemainingTime = dt;
                                // n.lotteryTime = dt.AddMinutes;
                                //n.RemainingTime = dt.AddSeconds(Opentime*60);
                                int Opentime = Convert.ToInt32(ub.GetSub("timeToOpen", xmlPath));
                                int time = Opentime * 60;
                                n.lotteryTime = dt.AddSeconds(time);
                                new BCW.BLL.GoodsList().Update(n);//更新获奖id，获奖码，到该商品
                                mod.Address = "1";//=win 成功获奖标识
                                new BCW.BLL.yg_BuyLists().UpdateAddress(mod.Id, mod.Address);//更新BuyLists 
                                if ((n.GoodsType % 2) == 0)//类型为偶数时自动生成新一期,偶数循环
                                {
                                    TimeSpan passtime = Convert.ToDateTime(n.OverTime) - Convert.ToDateTime(n.Addtime);
                                    double dayss = Math.Round(passtime.TotalDays, 0);
                                    n.RemainingTime = DateTime.Now.AddDays(dayss);
                                    n.OverTime = DateTime.Now.AddDays(dayss);
                                    n.Winner = "0";
                                    n.isDone = 1;
                                    n.periods += 1;
                                    n.Number = 0;
                                    n.lotteryTime = DateTime.Now.AddDays(dayss);
                                    n.Addtime = DateTime.Now;
                                    n.ImgCounts = 0;
                                    new BCW.BLL.GoodsList().Add(n);
                                }
                                //else { 
                                //    //builder.Append(Out.Div("div", "商品类型不为循环！不进行新建商品！"));
                                //    //if (n.Identification != 0)//用户商品开奖并且不循环
                                //    //{
                                //    //    ;
                                //    //}

                                //}
                                //builder.Append(Out.Tab("<div>", ""));
                                //builder.Append("商品列" +n.Id+ n.GoodsName+"开奖成功!"+"时间:"+Convert.ToDateTime(n.lotteryTime).ToString("yyyy-MM-dd hh:mm:ss fff")+"<br/>");
                                //builder.Append("正常检测中...");
                                //builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                //builder.Append("商品列"+"开奖有误!");
                                new BCW.BLL.Guest().Add(0, 727, "727", "开奖设置倒计时异常,商品ID:" + n.Id + "当前获奖列:" + userGet + "错误码003");//向 系统发内线
                            }
                        }
                        catch (Exception e)
                        {
                            new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "开奖失败原因" + e + ",请查看刷新机进行开奖");//向系统发内线
                            new BCW.BLL.Guest().Add(0, 727, "727", "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "开奖失败原因" + e + ",请查看刷新机进行开奖");//向系统发内线
                        }
                    }
                    k++;
                }
            }
            else
            {
                // builder.Append(Out.Div("div", "没有需要开奖的商品..."));
            }

        }
        catch (Exception e)
        {
            new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", "开奖异常:" + e + ",错误码003");//向 系统发内线
            new BCW.BLL.Guest().Add(0, 727, "727", "开奖失败原因" + e + ",请查看刷新机进行开奖");//向系统发内线
            // System.Console.WriteLine("传递过来的异常值为：{0}", e);
            // builder.Append(Out.Div("div", "开奖异常.."));
        }

    }

    //商品过期处理
    private void SearchReturnGoods()
    {
        string str = "isDone<>3 and isDone<>0 and OverTime< '" + DateTime.Now + "'";
        DataSet ds = new BCW.BLL.GoodsList().GetList(" * ", str);//找到过期商品
        int addGoodsValue = Convert.ToInt32(ub.GetSub("addGoodsValue", xmlPath));
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            // builder.Append(ds.Tables[0].Rows.Count);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ReturnGoods(Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]));
                // int meid = Convert.ToInt32(ds.Tables[0].Rows[i]["Identification"]);
                // string mename = new BCW.BLL.User().GetUsName(meid);
                //long gold = (Convert.ToInt64(ds.Tables[0].Rows[i]["GoodsValue"]) - addGoodsValue) * Convert.ToInt64(ds.Tables[0].Rows[i]["statue"]);
                //try
                //{
                //    if (statue == 0)
                //    {
                //        new BCW.BLL.User().UpdateiGold(meid, gold, "标识ID" + Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]) + "云购商品过期未开奖返还押金");
                //    }
                //    else if (statue == 2)
                //    {
                //        new BCW.SWB.BLL().UpdateMoney(meid, gold, 1007);
                //    }
                //}
                //catch(Exception ee)
                //{
                //    builder.Append(ee + "没有相关过期记录..");
                //    new BCW.BLL.Guest().Add(727, "测试727", "未成功返还商品ID" + Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]) + "币值" + Convert.ToInt64(ds.Tables[0].Rows[i]["IsGet"]));
                //}
            }
        }
    }

    //下架并返还币值
    private void ReturnGoods(int ptype)
    {
        //int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
        try
        {
            string strWhere = string.Empty;
            //   if (uid > 0)
            strWhere = "GoodsNum=" + ptype + "";
            BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(ptype));
            DataSet ds = new BCW.BLL.yg_BuyLists().GetList("*", strWhere);
            long addGoodsValue = Convert.ToInt64(ub.GetSub("addGoodsValue", xmlPath));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (model.isDone == 3 || (Utils.ToSChinese(model.StockYungouma) == "商品下架并成功返还"))
                        {
                            // builder.Append(Out.Div("div", "提示:该商品已返还所有购买记录,请勿重复提交.."));
                            break;
                        }
                        else
                        {
                            //model.StockYungouma = "测试商品下架并成功返还";)
                            int meid = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"]);
                            string mename = new BCW.BLL.User().GetUsName(meid);
                            long gold = Convert.ToInt64(ds.Tables[0].Rows[i]["IsGet"]);
                            if (statue == 0)
                            {
                                new BCW.BLL.User().UpdateiGold(meid, gold, "云购参与" + model.Id + "#ID" + Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]) + "过期未开奖返还");
                                //向系统发内线
                                new BCW.BLL.Guest().Add(0, 727, "酷爆网客服", "商品列" + model.Id + "名称" + model.GoodsName + "商品因过期已成功返还并下架");
                                // new BCW.BLL.Action().Add(GameId, Convert.ToInt32(n.Id), meid, mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]商品未开奖返还了" + gold + "币");
                                string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]因商品第" + model.periods + "期" + model.GoodsName + "过期未开奖返还了" + gold + "" + "币" + "[URL=/bbs/game/kbyg.aspx?act=geren]去查看吧[/URL]";
                                //发内线
                                new BCW.BLL.Guest().Add(0, meid, mename, strLog);
                                //model.isDone = 3;
                                //model.StockYungouma = "商品下架并成功返还";
                                //new BCW.BLL.GoodsList().Update(model);
                                // string mename=new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                                // builder.Append(k + ".已成功返还:" + mename + "," + "币值:" + n.IsGet + "." + "<br/>");
                            }
                            else if (statue == 2)
                            {
                                new BCW.SWB.BLL().UpdateMoney(meid, gold, 1007);
                                //向系统发内线
                                new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", "商品列" + model.Id + model.GoodsName + "测试币因过期已成功返还并下架" + "消息003");
                                //动态
                                // new BCW.BLL.Action().Add(GameId, GameId, meid, mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]测试商品过期未开奖返还了" + gold + "币");
                                string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]因商品第" + model.periods + "期" + model.GoodsName + "过期未开奖返还了" + gold + "" + "云币" + "[URL=/bbs/game/kbyg.aspx?act=geren]去查看吧[/URL]";
                                //发内线
                                new BCW.BLL.Guest().Add(0, meid, mename, strLog);
                                //model.isDone = 3;
                                //model.StockYungouma = "测试商品下架并成功返还";
                                //new BCW.BLL.GoodsList().Update(model);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        // builder.Append(ee+"没有相关过期记录..");
                        new BCW.BLL.Guest().Add(727, "测试727", "未成功返还ID" + Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]) + "币值" + Convert.ToInt64(ds.Tables[0].Rows[i]["IsGet"]));
                    }

                }
            }
            else
            {
                // builder.Append(Out.Div("div", "没有相关过期记录.."));
            }
            try
            {
                // if (statue == 0)//正式
                {
                    if (model.isDone != 3)
                    {
                        model.isDone = 3;
                        model.StockYungouma = "商品下架并成功返还";
                        new BCW.BLL.GoodsList().Update(model);
                        long rettur = Convert.ToInt64(model.GoodsSell);
                        if (statue == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.Identification), rettur, "云购出售" + model.Id + "未开奖返还");
                        }
                        else if (statue == 2)
                        {
                            new BCW.SWB.BLL().UpdateMoney(Convert.ToInt32(model.Identification), rettur, 1007);
                        }
                        //  Utils.Success(Gamename + "商品下架并成功返还", "商品下架并成功返还，正在返回..", Utils.getUrl("kbyg.aspx?backurl=" + Utils.getPage(1) + ""), "1");
                        builder.Append(Out.Div("div", "提示:该商品已返还所有购买记录."));
                    }
                }
                // if (statue == 2)//测试
                {
                }
            }
            catch (Exception ee)
            {
                // builder.Append(ee + "没有相关过期记录..");
                new BCW.BLL.Guest().Add(727, "测试727", "未成功返还商品ID" + model.Id + ee.ToString());
            }
        }
        catch
        {
            //Utils.Success("返还结果", "返还失败,正在返回上级.", Utils.getPage("kbyg.aspx?act=returntime"), "1"); 
        }
    }
}


