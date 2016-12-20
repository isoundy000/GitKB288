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
using System.Text.RegularExpressions;
using BCW.JS;
using BCW.SWB;
/// <summary>
/// 20160531_last版本
/// 20160611_修复实物添加图片与实物过期处理
/// 20160630_修复添加商品备注时文字限20字内
/// 20160816 使用百万富翁代替乐透
/// </summary>
public partial class bbs_game_kbyg : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/myyg.xml";
    protected int statue = Convert.ToInt32(ub.GetSub("KbygStatus", "/Controls/myyg.xml"));
    protected string Gamename = Convert.ToString(ub.GetSub("KbygName", "/Controls/myyg.xml"));
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    public int GameId = 1007;

    protected void Page_Load(object sender, EventArgs e)
    {

        //0正常1维护2测试  
        if (ub.GetSub("KbygStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        //if (DateTime.Now.Second%10==0)
        //{
        //    OpenGoods();
        //}
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":
                Rule();//新手指南
                break;
            case "gg":
                gonggao();//公告栏
                break;
            case "findGoods":
                FindGoods();//搜索商品结果
                break;
            case "buy":
                BuyPage();//购买商品详情页
                break;
            case "result":
                BuyResult();//购买结果页
                break;
            case "more":
                MoreGoods();//更多商品
                break;
            case "buylist":
                newBuyList();//购买记录
                break;
            case "myungou":
                myYungouList();//我的云购历史
                break;
            case "yungoum":
                SeeYungoum();//查看云购码
                break;
            case "listPeriods":
                GoodsListPeriods();//查看云购码
                break;
            case "buyOne":
                buyOneGoods();//每个商品对应的购买历史
                break;
            case "newOpen":
                newOpenList();//新揭晓
                break;
            case "sell":
                Tosell();//我要出售
                break;
            case "5":
                dafuhao();//秒变富豪
                break;
            case "daletou":
                Daletou();//天下大乐透功能页
                break;
            case "newThing"://新鲜玩意
                newThings();
                break;
            case "todayplay"://今日玩法
                TodayPlay();
                break;
            case "geren"://个人中心
                GerenZhongxin();
                break;
            case "paijiang":
                PaiJiang();//派奖
                break;
            case "chushou":
                SellResult();//出售结果
                break;
            case "addAddress":
                AddAddress();//添加快递信息
                break;
            case "seeAddress":
                SeeAddress();//查看快递信息
                break;
            case "cancel":
                SellResultToCancel();//取消商品审核
                break;
            case "updategoods":
                UpdateGoods();//商品状态修改
                break;
            case "getgold":
                GetGameGold();//获取测试币
                break;
            case "sure":
                SurePost();//确认收货
                break;
            case "count":
                CountList();//排行榜
                break;
            default:
                ReloadPage();
                break;
        }
    }

    //主页
    private void ReloadPage()
    {
        try
        {
            string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
            Master.Title = GameName;
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            //  builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
            //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
            builder.Append(GameName);
            builder.Append(Out.Tab("</div>", "<br/>"));
            int meid = new BCW.User.Users().GetUsId();
            int ceshi = Convert.ToInt32(ub.GetSub("ceshi", xmlPath));
            //顶部ubb
            string KbygTop = ub.GetSub("KbygTop", xmlPath);
            if (KbygTop != "")
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(KbygTop)));
                builder.Append(Out.Tab("</div>", ""));
            }
            if (ub.GetSub("ceshi", xmlPath) == "2")//酷币版测试
            {
                if (ub.GetSub("KbygStatus", xmlPath) == "0")//测试为2 正常0 维护1
                {
                    string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
                    string[] name = CeshiQualification.Split('#');
                    // foreach (string n in imgNum)
                    bool check = false;
                    for (int n = 0; n < name.Length - 1; n++)
                    {
                        if (name[n].ToString() == meid.ToString())
                        {
                            check = true;

                        }
                    }
                    if (!check)//未有资格
                    {
                        Utils.Error("很抱歉,您暂未有测试该游戏的权限", Utils.getUrl("kbyg.aspx"));
                    }
                }
            }

            //string Notes = ub.GetSub("KbygNotes", xmlPath);
            //if (Notes != "")
            //{
            //    builder.Append(Out.Tab("<div>", ""));
            //    builder.Append(Out.SysUBB(Notes) + "");
            //    builder.Append(Out.Tab("</div>", "<br />"));
            //}
            //  builder.Append(Out.Tab("WAP2.0显示", "WAP1.0显示")); 
            builder.Append(Out.Tab("<div>", ""));
            //if (ub.GetSub("KbygStatus", xmlPath) == "0")//正常
            //{
            //    //  builder.Append(Out.Tab("<div>", ""));
            //    builder.Append("我的财产:" + "<a href=\"" + Utils.getUrl("../finance.aspx") + "\"> " + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + " </a>" + ub.Get("SiteBz") + "<br/>");
            //    //  builder.Append(Out.Tab("</div>", ""));
            //}
            //else
            //{
            //    // builder.Append(Out.Tab("<div>", ""));
            //    builder.Append("我的财产:" + new BCW.SWB.BLL().GeUserGold(meid, GameId) + "云币");
            //    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=getgold&amp;ptype=" + 0 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + "[领]" + "</a><br/>");
            //    // builder.Append(Out.Tab("</div>", ""));
            //}

            int leiji = new BCW.BLL.yg_BuyLists().GetMaxId();
            builder.Append("本站累计参与" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">" + (leiji - 1) + "</a>" + "人次<a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\"><img src=\"" + "http://kb288.cc/FILES/kubao1/new.gif" + "\" alt=\"load\"/></a><br/>");
            int KbygIndexStyle = Convert.ToInt32(ub.GetSub("KbygIndexStyle", xmlPath));
            if (KbygIndexStyle == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=rule;backurl=" + Utils.PostPage(2) + "") + "\">新手指南</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=myungou") + "\">我的云购</a>.");
                //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=todayplay") + "\">围观开奖</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=daletou") + "\">百万富翁</a><br/>");

                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">个人中心</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=sell") + "\">我要出售</a>.");
                // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=5") + "\">天大乐透</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=count") + "\">排行榜单</a>");
            }
            else if (KbygIndexStyle == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=rule") + "\">规则</a>.");
                //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=todayplay") + "\">围观</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=sell") + "\">出售</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=daletou") + "\">乐透</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=myungou") + "\">历史</a><br/>");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">兑奖</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=newOpen") + "\">揭晓</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=more") + "\">参与</a>.");
                //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=gg") + "\">公告</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=count") + "\">排行</a>");
            }
            //builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=sell") + "\">云购夺宝</a>|");
            //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=daletou") + "\">百万富翁</a><br/>");
            builder.Append(Out.Tab("</div>", "<br/>"));
            // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=jiaoliu") + "\">玩家交流</a><br/>");
            // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=newThing") + "\">新鲜玩意</a>.");
            // builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("最新云购&gt;");
            builder.Append(Out.Tab("</div>", "<br/>"));
            //首页显示n条可设置的最新的商品
            builder.Append(Out.Tab("<div>", ""));
            try
            {
                //商品最大Id
                int newGoods = new BCW.BLL.GoodsList().GetMaxId();
                //路径
                string Logo = ub.GetSub("img", xmlPath);
                //显示商品列表记录数
                int ListCount = 3;// Convert.ToInt32(ub.GetSub("GoodsListCount", xmlPath));
                //显示最大图片量
                string imgCount = ub.GetSub("imgCount", xmlPath);
                //while (ListCount>0)
                for (int lists = 0; lists < ListCount; )
                {
                    DateTime nowTime = DateTime.Now;
                    BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(--newGoods);
                    //是否为进行中商品   
                    if (model.isDone == 1)
                    {
                        builder.Append((lists + 1) + ".");
                        builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期");
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
                        builder.Append("[进行中]");
                        lists++;
                        if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                        {
                            builder.Append("<br/>" + "图片描述:");
                            builder.Append("<br/>");
                            string[] imgNum = model.GoodsImg.Split(',');
                            // foreach (string n in imgNum)
                            for (int c = 0; c < imgNum.Length - 1; c++)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"5%\" height=\"5%\" alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                            }
                        }
                        builder.Append("<br/>" + "商品描述:" +BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
                        builder.Append("已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + model.Id + "&amp;") + "\">" + model.Number + "</a>" + "&nbsp;" + "总需:" + model.GoodsValue + "&nbsp;" + "  剩余:" + (model.GoodsValue - model.Number) + "<br/>");
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;") + "\">立即夺宝</a><br/>");
                    }
                    if (model.isDone == 2)//开奖中
                    {
                        builder.Append((lists + 1) + ".");
                        lists++;
                        builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期");
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
                        builder.Append("[开奖中].");
                        DataSet dd = new BCW.BLL.GoodsList().GetList(" CONVERT(varchar(100), GETDATE(), 120) as timess", "");
                        //  builder.Append(dd.Tables[0].Rows[0]["timess"]);
                        DateTime nowtime = Convert.ToDateTime(dd.Tables[0].Rows[0]["timess"]);
                        DateTime dtNow = Convert.ToDateTime(model.lotteryTime);
                        if (DateTime.Compare(nowtime, dtNow) <= 0)
                        {
                            builder.Append("<font  color=\"red\">");
                            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                            {
                                DateTime now = nowtime;
                                DateTime dt = Convert.ToDateTime(model.lotteryTime);
                                TimeSpan ts = dt - now;
                                if (ts.Minutes == 0)
                                { builder.Append(ts.Seconds); }
                                else
                                { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                            }
                            else
                            {
                                string ss = model.Id.ToString();
                                string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(model.lotteryTime));
                                builder.Append(time);
                            }
                            builder.Append("</font>");
                        }
                        else//倒计时完毕
                        {
                            try
                            {
                                OpenGoods();
                            }
                            catch { }
                        }
                        if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                        {
                            builder.Append("<br/>" + "图片描述:");
                            builder.Append("<br/>");
                            string[] imgNum = model.GoodsImg.Split(',');
                            //foreach (string n in imgNum)
                            for (int c = 0; c < imgNum.Length - 1; c++)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"5%\" height=\"5%\" alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                            }
                        }
                        builder.Append("<br/>" + "商品描述：" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
                        builder.Append("已参与:" + "&nbsp;" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Number + "</a>" + "&nbsp;" + "总需:" + model.GoodsValue + " 剩余:" + (model.GoodsValue - model.Number) + "<br/>");
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">立即围观</a><br/>");
                    }
                }
            }
            catch
            {
                builder.Append("无更多记录<br/>");
            }
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=more") + "\">更多云购</a>");
            string strText = "搜宝贝:/,";
            string strName = "uid,backurl";
            string strType = "text,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "";
            string strOthe = "搜云购,kbyg.aspx?act=findGoods,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("</div>", "<br/>"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("最新云购记录&gt;<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            try
            {
                int buyListCount = Convert.ToInt32(ub.GetSub("BuyListCount", xmlPath));
                int l = new BCW.BLL.yg_BuyLists().GetMaxId();
                for (int j = 0; j < buyListCount; j++)
                {
                    try
                    {
                        BCW.Model.yg_BuyLists tem = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(l--);
                        BCW.Model.GoodsList good = new BCW.BLL.GoodsList().GetGoodsList(tem.GoodsNum);
                        builder.Append(DateStringFromNow(Convert.ToDateTime(tem.BuyTime)));
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(tem.UserId));
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + tem.UserId + "") + "\">" + mename + "</a>");
                        builder.Append("参与了");
                        builder.Append("第" + good.periods + "期" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + tem.GoodsNum + "&amp;backurl=" + Utils.PostPage(0) + "") + "\">" + good.GoodsName + "</a><br/>");
                    }
                    catch { }
                }
            }
            catch //(Exception e)
            {
                builder.Append("无相关记录<br/>");
            }
            builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">更多云购记录</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("最新揭晓&gt;<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            try
            {
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "isDone=0";
                string[] pageValUrl = { "act", "backurl" };
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
                        try
                        {
                            int BuyListCount = Convert.ToInt32(ub.GetSub("BuyListCount", xmlPath));
                            if (k > BuyListCount)
                            { break; }
                            else
                            {
                                BCW.Model.yg_BuyLists mod = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt32(n.Winner));
                                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(mod.UserId));
                                builder.Append(DateStringFromNow(Convert.ToDateTime(n.RemainingTime)));
                                builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.periods + "</a>" + "期");
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(0) + "") + "\">" + n.GoodsName + "</a>");
                                builder.Append(",恭喜玩家" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + mod.UserId + "") + "\">" + mename + "</a>" + "幸运获奖" + "<br/>");
                            }
                            k++;
                        }
                        catch { }
                    }
                    // 分页  
                    //  builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录<br/>"));
                }
            }
            catch
            {
                builder.Append("无相关记录" + "<br/>");
            }
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=newOpen") + "\">更多揭晓记录</a>");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            //builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx?act=case") + "\">玩家最新留言&gt;</a><br/>");
            //builder.Append(Out.Tab("</div>", ""));
            ////闲聊显示
            //int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//
            //builder.Append(BCW.User.Users.ShowSpeak(21, "kbyg.aspx?act=0&amp;ptype=" + ptype + "", 23, 0));
            //builder.Append(Out.Tab("</div>", Out.Hr()));
            //builder.Append(Out.Tab("<hr/>", ""));
            //游戏底部Ubb

            //闲聊显示
            builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(39, "kbyg.aspx", 5, 0)));
            // builder.Append(BCW.User.Users.ShowSpeak(GameId, "kbyg.aspx", 5, 0));
            string Foot = ub.GetSub("KbygFoot", xmlPath);
            if (Foot != "")
            {
                // builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch { }
        try
        {
            OpenGoods();
        }
        catch
        {

        }
        //  Select CONVERT(varchar(100), GETDATE(), 120)
        //底部
        // string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=rule") + "\">晒单分享</a>.<a href=\"" + Utils.getUrl("brag.aspx?act=case") + "\">热门推荐.</a>");
        //  builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx?act=case") + "\">意见反馈.</a>");
    }

    //排行榜
    private void CountList()
    {
        Master.Title = Gamename;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //  builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("排行榜");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int sum = 9;
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
            try
            {
                string str = "Address<>0 group by UserId order by Address desc ";
                DataSet ds = new BCW.BLL.yg_BuyLists().GetList("UserId,count(Address)as Address", str);
                sum = 10;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (i >= ds.Tables[0].Rows.Count)
                        { break; }
                        if (i % 2 == 0)
                        { builder.Append(Out.Tab("<div >", "<br/>")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        }
                        if (sum > i)
                        {
                            builder.Append((i + 1) + ".");
                            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"]));
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UserId"] + "") + "\">" + mename + "</a>");
                            //  builder.Append("(" + ds.Tables[0].Rows[i]["UserId"] + ")");
                            builder.Append("累计获奖");
                            builder.Append(ds.Tables[0].Rows[i]["Address"] + "次");
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
            catch
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("温馨提示:暂无相关排行");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
            if (ptype == 2)//参与榜
            {
                try
                {
                    string str = "Address<3 group by UserId order by Address desc ";
                    DataSet ds = new BCW.BLL.yg_BuyLists().GetList("UserId,count(DISTINCT GoodsNum)as Address", str);//获取购买后获奖的100条记录
                    sum = 9;
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (i % 2 == 0)
                            { builder.Append(Out.Tab("<div >", "<br/>")); }
                            else
                            {
                                if (i == 1)
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                else
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            }
                            if (sum < i)
                            { break; }
                            else
                            {
                                builder.Append((i + 1) + ".");
                                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"]));
                                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UserId"] + "") + "\">" + mename + "</a>");
                                //  builder.Append("(" + ds.Tables[0].Rows[i]["UserId"] + ")");
                                builder.Append("累计参与");
                                builder.Append(ds.Tables[0].Rows[i]["Address"] + "次");
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                }
                catch
                {
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("温馨提示:暂无相关排行");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:排行榜单仅显示前10名的参与者哦");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //个人中心--兑奖
    private void GerenZhongxin()
    {
        Master.Title = "兑奖";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //  builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;兑奖" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("我的中奖消息:" + "<br/>");
        builder.Append("温馨提示:币值中奖请直接领奖,其他中奖请前往添加收货地址." + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "UserId= " + meid + "AND Address<>0 ";// +"and Address=1 ";//and Address=2";
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
                    BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(n.GoodsNum);
                    builder.Append(k + ".(编号" + n.Id + ")您");
                    builder.Append("参与了" + " <a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.Counts + "</a>" + "份的");
                    builder.Append("第" + model.periods + "期");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
                    builder.Append("时间" + Convert.ToDateTime(n.BuyTime).ToString("MM-dd HH:mm"));
                    // builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看云购码...</a><br/>");   
                    //  builder.Append("状态:");
                    if (n.Address == "1")//中奖
                    {
                        if (model.GoodsType == 8 || model.GoodsType == 9 || model.GoodsType == 4 || model.GoodsType == 5)//实物
                        {
                            if (new BCW.BLL.yg_OrderLists().ExistsGoodsId(Convert.ToInt32(model.Id)))//存在该
                            {
                                BCW.Model.yg_OrderLists order = new BCW.BLL.yg_OrderLists().Getyg_OrderListsForGoodsListId(model.Id);
                                if (order.wuliuStatue == 1 || order.Statue == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[已添地址,正在排队出货]" + "</a>" + "请留意通知!" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=seeAddress&amp;ptype=" + n.Id + "&amp;id=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[查]" + "</a>");
                                }
                                else if (order.Statue == 2 || order.wuliuStatue == 2)
                                {
                                    builder.Append("[已收货,已完结]" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=seeAddress&amp;ptype=" + n.Id + "&amp;id=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[查]" + "</a>");
                                }
                                else if (order.Statue == 0 || order.wuliuStatue == 0)
                                {
                                    builder.Append("[已添加地址]" + "请留意快递通知!" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=seeAddress&amp;ptype=" + n.Id + "&amp;id=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[查]" + "</a>");
                                }
                            }
                            else//币值
                            {
                                DateTime nowtime = DateTime.Now;
                                DateTime dtNow = Convert.ToDateTime(model.RemainingTime);
                                //  if (DateTime.Compare(nowtime, dtNow) <= 0)
                                if (model.isDone == 2)
                                {
                                    builder.Append("[开奖中]");
                                    //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                                    //{
                                    //    DateTime now = DateTime.Now;
                                    //    DateTime dt = Convert.ToDateTime(model.RemainingTime);
                                    //    TimeSpan ts = dt - now;
                                    //    if (ts.Minutes == 0)
                                    //    { builder.Append(ts.Seconds); }
                                    //    else
                                    //    { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                                    //}
                                    //else
                                    //{
                                    //    string ss = model.Id.ToString();
                                    //    string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(model.RemainingTime));
                                    //    builder.Append(time);
                                    //}
                                }
                                else
                                {
                                    if (model.GoodsType == 4 || model.GoodsType == 5 || model.GoodsType == 8 || model.GoodsType == 9)
                                    {
                                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[中][未领奖]" + "</a>" + ".请留意通知！" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=addAddress&amp;ptype=" + model.Id + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[添]" + "</a>");
                                    }
                                }
                            }
                            // builder.Append("我的收货地址:" + "<br/>");
                            // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=addAddress") + "\">添加我的收货地址>></a>" + "<br/>");
                        }
                        else//未中
                        {
                            DateTime nowtime = DateTime.Now;
                            DateTime dtNow = Convert.ToDateTime(model.RemainingTime);
                            // if (DateTime.Compare(nowtime, dtNow) <= 0)
                            if (model.isDone == 2)
                            {
                                builder.Append("[开奖中]");
                                //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                                //{
                                //    DateTime now = DateTime.Now;
                                //    DateTime dt = Convert.ToDateTime(model.RemainingTime);
                                //    TimeSpan ts = dt - now;
                                //    if (ts.Minutes == 0)
                                //    { builder.Append(ts.Seconds); }
                                //    else
                                //    { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                                //}
                                //else
                                //{
                                //    string ss = model.Id.ToString();
                                //    string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(model.RemainingTime));
                                //    builder.Append(time);
                                //}
                            }
                            else
                            {
                                if (model.GoodsType == 4 || model.GoodsType == 5 || model.GoodsType == 8 || model.GoodsType == 9)
                                {
                                    if (model.isDone == 2)
                                    { builder.Append("[开奖中]"); }
                                    else
                                    {
                                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[中][未领奖]" + "</a>" + ".请留意通知！" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=addAddress&amp;ptype=" + model.Id + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[添]" + "</a>");
                                    }
                                }
                                else
                                {
                                    if (model.isDone != 3)
                                    {
                                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=paijiang&amp;ptype=" + model.Id + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[兑奖]" + "</a>");
                                    }
                                    else
                                    {
                                        builder.Append("[已过期]" + model.StockYungouma);
                                    }
                                }
                            }
                        }
                    }
                    else
                        if (model.isDone == 1)
                        { builder.Append("[进行中]"); }
                        else
                            if (n.Address == "0" && model.isDone != 2)
                            { builder.Append("[未中]"); }
                            else
                                if (model.isDone == 2)
                                { builder.Append("[开奖中]"); }
                                else
                                    if (n.Address == "2")
                                    { builder.Append("[已兑奖]"); }

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无更多中奖记录.");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<br/>" + "暂无更多中奖记录.");
            builder.Append(Out.Tab("</div>", ""));
        }

        //底部
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        //builder.Append("单期独中奖金10--100万酷币!!!" + "<br>");
        //int isOpen = Convert.ToInt32(ub.GetSub("UserSell", xmlPath));//开放为1，关闭为0
    }

    //查看快递信息
    private void SeeAddress()
    {
        Master.Title = "查看收货地址";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">个人中心</a>-");
        builder.Append("查看收货信息" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的购买列编号
        long id = Convert.ToInt64(((Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"))));//对应的商品编号
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("注:" + "<br/>");
        builder.Append("1.请仔细填写真实的快递地址信息,,并确保该地址能正常收货." + "<br/>");
        builder.Append("2.至少填写一个能正常通讯的手机号,并确保该手机能正常接收电话通知." + "<br/>");
        builder.Append("3.系统将严格保护您的信息,请放心填写." + "<br/>");
        builder.Append("4.系统将在确认信息并审核后正式发货,请耐心等待." + "<br/>");
        builder.Append("5.如地址信息填写有误,导致快递物流有误,系统将保留最终的解析权利." + "<br/>");
        try
        {
            BCW.Model.yg_OrderLists model = new BCW.BLL.yg_OrderLists().Getyg_OrderListsForGoodsListId(id);
            BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(id));
            builder.Append("您的宝贝信息如下:" + "<br/>");
            builder.Append("名称:" + goods.GoodsName + "<br/>");
            builder.Append("期数:" + "第" + goods.periods + "期" + "<br/>");
            builder.Append("描述:" + goods.explain + "<br/>");
            builder.Append("开奖时间:" + Convert.ToDateTime(goods.RemainingTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br/>");
            builder.Append("您的快递信息如下:" + "<br/>");
            builder.Append("收货地址:" + model.Address + "<br/>");
            builder.Append("邮编:" + model.ZipCode + "<br/>");
            builder.Append("收货人姓名:" + model.Consignee + "<br/>");
            builder.Append("手机号码:" + model.PhoneMobile + "<br/>");
            builder.Append("备用手机:" + model.PhoneMobile + "<br/>");
            builder.Append("添加时间:" + Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br/>");
            builder.Append("备注:" + model.ConsigneeNotes + "<br/>");
            if (model.isDone == 0 || model.wuliu == "0")
            {
                builder.Append("温馨提示:您的获奖商品还未发货,请耐心等待." + "<br/>");
            }
            else
            {
                builder.Append("运单号:" + model.yundanhao + "<br/>");
                builder.Append("物流:" + model.wuliu + "<br/>");
                string text = "";
                if (model.wuliuStatue == 0) { text = "未发货"; }
                if (model.wuliuStatue == 1) { text = "已发货,物流运输中"; }
                if (model.wuliuStatue == 2) { text = "已收货"; }
                builder.Append("物流状态:" + text + "<br/>");
                builder.Append("发货时间:" + Convert.ToDateTime(model.PostTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br/>");
                if (model.Statue == 2 || model.wuliuStatue == 2)
                {

                    builder.Append("订单状态:" + "已完结" + "<br/>");
                    builder.Append("时间:" + Convert.ToDateTime(model.OverTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br/>");
                    builder.Append("温馨提示:" + "您已收货,祝你生活愉快,好运连连");
                }
                else
                {
                    builder.Append("温馨提示:" + "请查看您的订单号并耐心等待,您的宝贝即将到达..." + "<br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=sure&amp;id=" + model.Id + "&amp;ptyped=sure&amp;") + "\">确定收货</a>.");
                    // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=sure&amp;id=" + model.Id + "&amp;ptyped=not&amp;") + "\">催单</a>");
                }
            }

        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:暂未找到相关信息");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //   builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //确认收货
    private void SurePost()
    {
        Master.Title = "查看收货地址";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        // builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">个人中心</a>-");
        builder.Append("确认收货" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int id = Convert.ToInt32(((Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"))));//订单编号
        string ptyped = Convert.ToString(((Utils.GetRequest("ptyped", "get", 1, @"^[^\^]{1,20}$", "0"))));//方式
        //  builder.Append(ptyped);sure not
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            BCW.Model.yg_OrderLists model = new BCW.BLL.yg_OrderLists().Getyg_OrderLists(id);
            if (model.Statue == 2)
            {
                builder.Append("您已确认收货,请勿重复提交!");
                builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">返回个人中心</a>");
            }
            else
            {
                model.Statue = 2;
                model.wuliuStatue = 2;
                model.OverTime = DateTime.Now;
                model.isDone = 2;
                new BCW.BLL.yg_OrderLists().Update(model);
                Utils.Success("恭喜", "确认收货成功,祝您生活愉快." + "正在返回个人中心.", Utils.getUrl("kbyg.aspx?act=geren&amp;"), "2");
            }
        }
        catch { builder.Append("确认收货超时.请重新确定"); }
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    //添加快递信息
    private void AddAddress()
    {
        Master.Title = "添加收货地址";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("个人中心-");
        builder.Append("添加收货地址" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//购买列id.
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("注:" + "<br/>");
        builder.Append("1.请仔细填写真实的快递地址信息,,并确保该地址能正常收货." + "<br/>");
        builder.Append("2.至少填写一个能正常通讯的手机号,并确保该手机能正常接收电话通知." + "<br/>");
        builder.Append("3.系统将严格保护您的信息,请放心填写." + "<br/>");
        builder.Append("4.系统将在确认信息并审核后正式发货,请耐心等待." + "<br/>");
        builder.Append("5.如地址信息填写有误,快递物流有误,系统将保留最终的解析权利.");
        builder.Append(Out.Tab("</div>", ""));
        try
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            BCW.Model.yg_OrderLists model = new BCW.Model.yg_OrderLists();
            BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(ptype));
            if (Utils.ToSChinese(ac) == "确定添加")
            {
                if (new BCW.BLL.yg_OrderLists().ExistsGoodsId(Convert.ToInt32(goods.Id)))//存在该商品Id
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("你已添加过此商品的物流地址信息,若要修改请联系客服修改." + "<br/>");
                    builder.Append("查看我的订单快递信息" + "<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    string address = Utils.GetRequest("address", "post", 2, @"^[^\^]{1,200}$", "收货地址限1-20字内");
                    string youbian = Utils.GetRequest("youbian", "post", 3, @"^\d{6}$", "邮编填写错误");
                    string name = Utils.GetRequest("name", "post", 3, @"^[^\^]{1,20}$", "收货人填写错误");
                    string phone = Utils.GetRequest("phone", "post", 2, @"^[1]+[3,5]+\d{9}", "手机号码填写错误");
                    string HomePhone = Utils.GetRequest("HomePhone", "post", 4, @"^[0-9]\d*$", "备用手机号码填写错误(整数)");
                    string beizhu = Utils.GetRequest("beizhu", "all", 2, @"^[^\^]{1,200}$", "留言备注限200字内");
                    // DateTime now=new DateTime();
                    model.Address = address;
                    model.BuyListId = id;
                    model.GoodsListId = ptype;
                    model.brack = "a";
                    model.Consignee = name;
                    model.ConsigneeNotes = beizhu;
                    model.ConsigneeStatue = "0";
                    model.isDone = 0;
                    model.Operator = "0";
                    model.OperatorNotes = "0";
                    model.OperatorStatue = 0;
                    model.OverTime = DateTime.Now;
                    model.PhoneHome = HomePhone;
                    model.PhoneMobile = phone;
                    model.PostTime = DateTime.Now;
                    model.Spare = "0";
                    model.Statue = 0;
                    model.UserId = meid;
                    model.wuliu = "0";
                    model.wuliuStatue = 0;
                    model.yundanhao = "0";
                    model.ZipCode = Convert.ToInt32(youbian);
                    model.AddTime = DateTime.Now;
                    try
                    {
                        new BCW.BLL.yg_OrderLists().Add(model);
                        new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", "云购中奖用户Id" + meid + "中奖商品第" + new BCW.BLL.GoodsList().Getperiods(model.GoodsListId) + "期" + new BCW.BLL.GoodsList().GetGoodsName(model.GoodsListId) + "编号" + model.GoodsListId + "已填写完收货地址，请查看");//向系统发内线
                        Utils.Success("添加收货地址添加成功", "添加收货地址添加成功，正在返回中奖页面..", Utils.getUrl("kbyg.aspx?act=geren&amp;backurl=" + Utils.getPage(0) + ""), "2");
                    }
                    catch (Exception ee)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        //  builder.Append(e);
                        builder.Append(ee + "添加出错,请重新" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=addAddress&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">添加</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
            else
            {
                string strText = "收货地址(必填且真实):/,邮编(必填且真实):/,收货人姓名(必填):/,手机号码(必填且可通讯):/,备用手机(可留空):/,备注(可留空):/,";
                string strName = "address,youbian,name,phone,HomePhone,beizhu";
                string strType = "textarea,num,text,num,num,textarea";
                string strValu = "" + "例如:某某省某某市某某区某某街道等" + "'" + "666666" + "'" + "黎明" + "'" + 13800138000 + "'" + 13800138000 + "'" + "可留空" + "'";
                string strEmpt = "false,true,true,false,false,true";
                string strIdea = "/";
                string strOthe = "确定添加|reset,kbyg.aspx?act=addAddress&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "&amp;,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:每件商品物流信息仅限填写一次,请谨慎填写");
                builder.Append(Out.Tab("</div>", ""));
            }

        }
        catch { }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //取消审核商品
    private void SellResultToCancel()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>-");
        builder.Append("个人中心-");
        builder.Append("取消审核" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "1"));//对应的商品编号
        BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(ptype);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            string goodsname = new BCW.BLL.GoodsList().GetGoodsName(Convert.ToInt64(ptype));
            Master.Title = "取消审核";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定取消审核该商品吗？" + "<br/>");
            builder.Append("温馨提示:" + "<br/>" + " 你参与审核的商品:" + goodsname + "<br/>" + "所需参与人次:" + model.GoodsValue + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=cancel&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定取消</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=edit&amp;backurl=" + Utils.getPage(0) + "") + "\">先看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            try
            {
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(model.Addtime);
                if (ts.TotalHours < 12)
                {
                    Utils.Error("出售时间" + "不足12" + "小时，请12小时后再取消审核", "");
                }
                else
                {
                    if (model.isDone == 6)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("您已成功取消了审核,币值已返还到你的账户." + "<br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=chushou&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级..</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else if (model.isDone == 1)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("该出售记录已上架,取消审核失败！请等待商品出售成功！" + "<br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=chushou&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级..</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        if (ub.GetSub("KbygStatus", xmlPath) == "0")
                        {
                            int addGoodsValue = Convert.ToInt32(ub.GetSub("addGoodsValue", xmlPath));
                            long gold = Convert.ToInt64(model.GoodsSell);
                            model.isDone = 6;
                            new BCW.BLL.GoodsList().Update(model);
                            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.Identification));
                            string strLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]申请的" + model.GoodsName + "" + "已被你取消了" + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                            new BCW.BLL.Guest().Add(1, Convert.ToInt32(model.Identification), mename, strLog);
                            //返回到用户
                            new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.Identification), gold, "标识ID" + model.Id + "云购出售商品取消审核返回币值");
                            new BCW.BLL.Action().Add(GameId, 0, Convert.ToInt32(model.Identification), mename, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]取消了审核返还了" + (gold) + "币");
                            Utils.Success("恭喜", "取消成功" + "正在返回..." + "", Utils.getUrl("kbyg.aspx?act=chushou&amp;"), "2");
                        }
                        else
                            if (ub.GetSub("KbygStatus", xmlPath) == "2")
                            {
                                long gold = Convert.ToInt64(model.GoodsSell);// Convert.ToInt64((model.GoodsValue - 10) * model.statue);
                                model.isDone = 6;
                                new BCW.BLL.GoodsList().Update(model);
                                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.Identification));
                                string strLog = "您在[URL=/bbs/game/kbyg.aspx]云购[/URL]申请的" + model.GoodsName + "" + "已被你取消了" + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                                new BCW.BLL.Guest().Add(1, Convert.ToInt32(model.Identification), mename, strLog);
                                //返回到用户
                                //  new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.Identification), gold, "云购出售商品取消审核返回币值");
                                new BCW.SWB.BLL().UpdateMoney(meid, gold, GameId);
                                //  new BCW.BLL.Action().Add(GameId, 0, Convert.ToInt32(model.Identification), mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]取消了审核返还了" + (gold) + "云币");
                                Utils.Success("恭喜", "取消成功" + "正在返回..." + "", Utils.getUrl("kbyg.aspx?act=chushou&amp;"), "2");
                            }
                    }
                }
            }
            catch { }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //派奖给中奖人员
    private void PaiJiang()
    {
        Master.Title = "兑奖中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("兑奖中心");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//购买列id.
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div>", "<br/>"));
        try
        {
            BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(id));
            BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(ptype));
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
                    if (Convert.ToInt32(model.UserId) != meid || model.Id != Convert.ToInt64(goods.Winner))
                    {
                        Utils.Error("不存在的兑奖记录", "");
                    }
                    else if (goods.isDone == 3)
                    {
                        Utils.Error("该记录已过期，币值已成功返还！", "");
                    }
                    else
                    {
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                        int KbygPersan = Convert.ToInt32(ub.GetSub("KbygPersan", xmlPath));
                        long suishou = Convert.ToInt64((goods.statue * goods.GoodsValue) * KbygPersan * 0.001);//系统
                        long suishou_user = Convert.ToInt64((goods.GoodsSell) * KbygPersan * 0.001);//用户发布税收计算方式
                        long god = 0;
                        if (goods.Identification != 0)
                        {
                            god = Convert.ToInt64(goods.GoodsSell - suishou_user);//用户发布的计算方式
                        }
                        else
                        {
                            god = Convert.ToInt64((goods.statue * (goods.GoodsValue)) - suishou);//系统
                        }
                        builder.Append("商品名称:" + "第" + goods.periods + "期");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + goods.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + goods.GoodsName + "</a><br/>");
                        //  builder.Append(goods.GoodsName + "<br/>");
                        builder.Append("云购ID:" + ptype + "<br/>");
                        builder.Append("购买记录列:" + id + "<br/>");
                        builder.Append("获奖者:" + mename + "<br/>");
                        if (goods.Identification > 0)
                        {
                            builder.Append("获奖总价值:" + goods.GoodsSell + "<br/>");
                            builder.Append("扣除税收:" + suishou_user + "<br/>");
                        }
                        else
                        {
                            builder.Append("获奖总价值:" + goods.GoodsValue * goods.statue + "<br/>");
                            builder.Append("扣除税收:" + suishou + "<br/>");
                        }
                        // builder.Append("扣除税收:" + suishou + "<br/>");
                        builder.Append("最终兑奖:" + god + "<br/>");
                        builder.Append(Out.Tab("</div>", ""));
                        if (model.Address == "2")
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("此中奖记录已成功领奖！" + "<br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">返回继续兑奖</a>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            //if (goods.GoodsType == 2 || goods.GoodsType == 3 || goods.GoodsType == 1 || goods.GoodsType == 0 || goods.GoodsType == 100 || goods.GoodsType == 5|| goods.GoodsType == 10 || goods.GoodsType == 1)
                            {
                                try
                                {
                                    int userID = Convert.ToInt32(model.UserId);
                                    if (ub.GetSub("KbygStatus", xmlPath) == "0")//正常
                                    {
                                        BCW.User.Users.IsFresh("kbyg", 1);//防刷
                                        new BCW.BLL.User().UpdateiGold(userID, god, "云购参与" + ptype + "#ID" + id + "中奖兑奖");
                                        string addr = "2";
                                        long i = id;
                                        string cText = "在[URL=/bbs/game/kbyg.aspx]"+Gamename+"[/URL]" + goods.GoodsName + "中奖赢得" + (god) + "酷币";//"第" + goods.periods + "期"
                                        new BCW.BLL.yg_BuyLists().UpdateAddress(i, addr);
                                        new BCW.BLL.Action().Add(GameId, Convert.ToInt32(model.Id), Convert.ToInt32(model.UserId), mename, cText);
                                        // string strLog = "[URL=/bbs/game/kbyg.aspx]"+Gamename+"[/URL]获得了" + god + ub.Get("SiteBz") + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                                        //  string strLog = "";[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/kbyg.aspx]" +
                                        // int usid= Convert.ToInt32(model.UserId);
                                        // new BCW.BLL.Guest().Add(GameId, usid, mename, strLog);
                                    }
                                    else if (ub.GetSub("KbygStatus", xmlPath) == "2")//测试
                                    {
                                        BCW.User.Users.IsFresh("kbyg", 1);//防刷
                                        new BCW.SWB.BLL().UpdateMoney(userID, god, GameId);
                                        string addr = "2";
                                        long i = id;
                                        new BCW.BLL.yg_BuyLists().UpdateAddress(i, addr);//更新获奖标识
                                        new BCW.BLL.Action().Add(GameId, Convert.ToInt32(model.Id), Convert.ToInt32(model.UserId), mename, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]测试中奖赢得" + god + "云币");
                                        //  string strLog = "您在[URL=/bbs/game/kbyg.aspx]"+Gamename+"[/URL]领奖了" + god + "云币" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                                        //   new BCW.BLL.Guest().Add(GameId, Convert.ToInt32(model.UserId), mename, strLog);

                                    }
                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("兑奖成功！" + "<br/>");
                                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">返回继续兑奖</a>");
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                catch
                                {
                                    ////  builder.Append(e);
                                    //  builder.Append(Out.Tab("<div>", ""));
                                    //  builder.Append("兑奖错误！请确认信息后重新兑奖" + "<br/>");
                                    //  builder.Append(Out.Tab("</div>", ""));
                                }
                            }
                            //else
                            //{
                            //    builder.Append("" + "<br/>");
                            //}
                        }
                    }
                }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("没有查询到相关记录！请确认信息后重新兑奖" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //一起围观
    private void TodayPlay()
    {
        Master.Title = "一起围观";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        //头部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("围观开奖");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append("单期独中奖金10--100万酷币!!!" + "<br>");
        int isOpen = Convert.ToInt32(ub.GetSub("UserSell", xmlPath));//开放为1，关闭为0
        if (isOpen == 1)//测试暂开
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("该功能尚未开放，敬请期待！");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            try
            {
                string Logo = ub.GetSub("img", xmlPath);
                int ListCount = Convert.ToInt32(ub.GetSub("GoodsListCount", xmlPath));
                string imgCount = ub.GetSub("imgCount", xmlPath);
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "isDone=2";
                string[] pageValUrl = { "act", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                // 开始读取列表
                IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
                if (listSSCpay.Count > 0)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("围观激动时刻!!!开奖时刻即将到来！！！" + "<br>");
                    builder.Append(Out.Tab("</div>", ""));
                    int k = 1;
                    foreach (BCW.Model.GoodsList n in listSSCpay)
                    {
                        if (k % 2 == 0)
                        { builder.Append(Out.Tab("<div class=\"text\">", "<br />")); }
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append(k + ".第" + n.periods + "期");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                        if (n.isDone == 1)
                        {
                            builder.Append("(正在进行中.)");
                            builder.Append("<br/>");
                        }
                        if (n.isDone == 2)
                        {
                            builder.Append("正在计算开奖中.倒计时:");
                            DateTime nowtime = DateTime.Now;
                            DateTime dtNow = Convert.ToDateTime(n.lotteryTime);
                            if (DateTime.Compare(nowtime, dtNow) <= 0)
                            {
                                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                                {
                                    DateTime now = DateTime.Now;
                                    DateTime dt = Convert.ToDateTime(n.lotteryTime);
                                    TimeSpan ts = dt - now;
                                    if (ts.Minutes == 0)
                                    { builder.Append(ts.Seconds); }
                                    else
                                    { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                                }
                                else
                                {
                                    string ss = n.Id.ToString();
                                    string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(n.RemainingTime));
                                    builder.Append(time);
                                }
                            }
                            builder.Append("<br/>");
                        }
                        if (n.isDone == 0)
                        {
                            builder.Append("(已成功揭晓!)" + "<br/>");
                        }
                        if (n.GoodsImg.ToString() != "0")
                        {
                            builder.Append("图片描述:" + "<br/>");

                            string[] imgNum = n.GoodsImg.Split(',');
                            // foreach (string m in imgNum)
                            for (int c = 0; c < imgNum.Length - 1; c++)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c] + "\"  width=\"50\" height=\"40\" alt=\"load\" border=\"10\" border-color=\"#C0C0C0\" />" + "</a>&nbsp;&nbsp;&nbsp;");
                            }
                            builder.Append("<br/>");
                        }
                        builder.Append("商品描述：" + "<br/>" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(n.explain)) + "<br/>" + "已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Number + "</a>" + "总需人次:" + n.GoodsValue + "剩余人次:" + (n.GoodsValue - n.Number) + "<br/>" + "所需：" + n.statue + ub.Get("SiteBz") + "/每人次" + "<br/>");

                        if (n.isDone == 0)
                        {
                            string[] winner = n.Winner.Split(',');
                            //builder.Append("获奖Id，获奖云购码，获奖记录和：");
                            //  foreach (string s in winner)
                            for (int i = 0; i < 2; i++)
                            {
                                if (i == 0)
                                {
                                    builder.Append("获奖Id:");

                                }
                                else
                                    if (i == 1)
                                    {
                                        builder.Append("获奖云购码:");
                                    }
                                builder.Append(winner[i]);
                                builder.Append("&nbsp;");
                            }
                        }
                        if (n.isDone == 1)
                        {
                            builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "") + "\">立即参与&gt;&gt;</a><br/>");
                        }
                        if (n.isDone == 2)
                        {
                            builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "") + "\">立即前往围观激动时刻&gt;&gt;&gt;</a><br/>");
                        }
                        //BCW.User.Users.SetUser(n.Winner)
                        builder.Append(Out.Tab("</div>", ""));

                        k++;

                        // 分页
                        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    }

                }
                else
                {
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("暂无最新围观." + "<br/>");
                    builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            catch { builder.Append("暂无正在开奖"); }
        }
        //底部
        // string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //规则介绍
    private void Rule()
    {
        Master.Title = "新手指南";
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;新手指南" + "<br/>");
        //builder.Append("新手指南");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("规则：" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1、每件商品参考市场价平分成相应“等份”，每份对应相应酷币，1份对应1个云购码。" + "<br/>");
        builder.Append("2、同一件商品可以购买多次或一次购买多份，每购买一次则为参与一人次。" + "<br/>");
        builder.Append("3、当一件商品所有“等份”全部售出后计算出“幸运云购码”，拥有“幸运云购码”者即可获得此商品。" + "<br/>");
        builder.Append("4、幸运云购码计算方式：" + "<br/>");
        builder.Append("1)取该商品最后购买时间前网站所有商品100条购买时间记录（限时揭晓商品取截止时间前网站所有商品100条购买时间记录）。" + "<br/>");
        builder.Append("2)时间按时、分、秒、毫秒依次排列组成一组数值。" + "<br/>");
        builder.Append("3)将这100组数值之和除以商品总需参与人次后取余数，余数加上10,000,000即为“幸运云购码”。" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">"+Gamename+"</a>&gt;新手指南&gt;流程" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("流程：" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1、挑选商品" + "<br/>");
        builder.Append("分类浏览或直接搜索商品，点击");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=more") + "\">立即参与</a>");
        builder.Append("。" + "<br/>");
        builder.Append("2、支付酷币" + "<br/>");
        builder.Append("通过在线支付平台，支付1份酷币即购买1人次，获得1个“云购码”。同一件商品可购买多次或一次购买多份，购买的“云购码”越多，获得商品的几率越大。" + "<br/>");
        builder.Append("3、");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=newOpen") + "\">揭晓</a>");
        builder.Append("获得者" + "<br/>");
        builder.Append("当一件商品达到总参与人次，计算出1名商品获得者，系统会通过手机短信或系统通知您领取商品。" + "<br/>");
        builder.Append("注：" + "<br/>");
        builder.Append("1)商品揭晓后您可登录查看");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=myungou") + "\">我的云购</a>");
        builder.Append("我的云购" + "查询详情，未获得商品的用户不会收到短信或邮件通知。" + "<br/>");
        builder.Append("2)商品揭晓后，请及时登录" + "我的云购");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren") + "\">个人中心</a>");
        builder.Append("完善快递信息，以便我们能够准确无误地为您配送商品。" + "<br/>");
        builder.Append("3)所有已揭晓商品均不给予退款。" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));

        //底部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //秒变富豪
    private void dafuhao()
    {
        Master.Title = "秒变富豪";
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;秒变富豪" + "<br/>");
        //builder.Append("秒变富豪");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("这是一个快速造就富翁的游戏!!!" + "<br/>");
        builder.Append("单期独中奖金10--100万酷币!!!" + "<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=daletou") + "\">立即参与大乐透>></a>.");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int isOpen = Convert.ToInt32(ub.GetSub("isOpenMiaoBianFuhao", xmlPath));//开放为1，关闭为0
        if (isOpen != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("该功能尚未开放，敬请期待！");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //大富豪功能===大乐透
            string Logo = ub.GetSub("img", xmlPath);
            int ListCount = Convert.ToInt32(ub.GetSub("GoodsListCount", xmlPath));
            string imgCount = ub.GetSub("imgCount", xmlPath);
            try
            {
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "GoodsName like '%1%'";
                // strWhere+="and explain like" +"'%" + Title + "%'";
                string[] pageValUrl = { "act", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                // 开始读取列表
                IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
                if (listSSCpay.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.Model.GoodsList model in listSSCpay)
                    {
                        if (model.isDone == 1)
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期正在进行中.." + "<br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
                            if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                            {
                                builder.Append("<br/>");
                                string[] imgNum = model.GoodsImg.Split(',');
                                // foreach (string n in imgNum)
                                for (int c = 0; c < imgNum.Length - 1; c++)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c] + "\"  width=\"8%\" height=\"8%\" alt=\"load\"/>" + "</a>");
                                }
                            }
                            builder.Append("<br/>" + "商品描述:" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
                            builder.Append("已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Number + "</a>" + "总需人次:" + model.GoodsValue + "剩余人次:" + (model.GoodsValue - model.Number) + "<br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(0) + "") + "\">立即参与</a><br/>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        k++;
                    }// 分页  
                    //  builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("找不到关于" + Title + "的相关记录.");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            catch
            {
                // System.Console.WriteLine("传递过来的异常值为：{0}", e);
                // builder.Append(Out.Div("div", "找不到关于" + Title + "的相关记录."));
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回购物大厅...</a>");
        builder.Append(Out.Tab("</div>", ""));
        //底部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //   builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //百万富翁---天下大乐透子页
    private void Daletou()
    {
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        Master.Title = "百万富翁";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("百万富翁");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string str = "GoodsImg='100'order by AddTime desc";
        DataSet ds = new BCW.BLL.GoodsList().GetList("top 1 *", str);//获取100万最新记录
        DataSet dsTen = new BCW.BLL.GoodsList().GetList("top 1 *", "GoodsImg='10'order by AddTime desc");//获取10万最新记录
        DataSet dsFive = new BCW.BLL.GoodsList().GetList("top 1 *", "GoodsImg='5'order by AddTime desc");//获取5万最新记录
        DataSet dsOne = new BCW.BLL.GoodsList().GetList("top 1 * ", "GoodsImg='1' order by AddTime desc");//获取1万最新记录
        try
        {
            builder.Append(Out.Tab("<div>", ""));
            int meid = new BCW.User.Users().GetUsId();
            if (ub.GetSub("KbygStatus", xmlPath) == "0")//正常
            {
                builder.Append("我的财产:" + "<a href=\"" + Utils.getUrl("../finance.aspx") + "\"> " + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + " </a>" + ub.Get("SiteBz") + "<br/>");
            }
            else if (ub.GetSub("KbygStatus", xmlPath) == "2")//测试
            {
                builder.Append("我的财产:" + new BCW.SWB.BLL().GeUserGold(meid, GameId) + "云币");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=getgold&amp;ptype=" + 0 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + "[领]" + "</a><br/>");
            }
            builder.Append("这是一个快速造就富翁的游戏!!!" + "<br/>");//系统随机在本期参与用户中产生
            builder.Append("[乐透]");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ds.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 100万</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsTen.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 10万</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsFive.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 5万</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsOne.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 1万</a>" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("找不到相关记录,请耐心等待系统生成大乐透乐透单...");
            builder.Append(Out.Tab("</div>", ""));
        }
        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("最新中奖" + "&gt;");
        //builder.Append(Out.Tab("</div>", "<br/>"));
        //try
        //{
        //    string strAll = "GoodsType=12 and isDone=0 order by RemainingTime desc";
        //    DataSet dss = new BCW.BLL.GoodsList().GetList(" *", strAll);//获取最新记录
        //    if (dss != null && dss.Tables[0].Rows.Count > 0)
        //    {
        //        int k = 0;
        //        int start = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);//
        //        for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
        //        {
        //            if (k < 5)
        //            {
        //                BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(dss.Tables[0].Rows[i]["Winner"]));
        //                string username = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
        //                builder.Append(Out.Tab("<div>", ""));
        //                //sum = Convert.ToInt64(sum + (Convert.ToInt32(Convert.ToDateTime(ds.Tables[0].Rows[i]["BuyTime"]).ToString("hhmmssfff"))));
        //                if (Convert.ToInt32(dss.Tables[0].Rows[i]["GoodsImg"]) == 100)
        //                {
        //                    builder.Append("[百万富翁]" + "第" + dss.Tables[0].Rows[i]["periods"] + "期:");
        //                    // builder.Append("第" + dss.Tables[0].Rows[i]["periods"] + "期:"); 
        //                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UserId + "") + "\">" + username + "(" + model.UserId + ")" + "</a>.");
        //                    builder.Append("时间:" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff"));

        //                }
        //                else
        //                    if (Convert.ToInt32(dss.Tables[0].Rows[i]["GoodsImg"]) == 10)
        //                {
        //                    builder.Append("[十万地主]" + "第" + dss.Tables[0].Rows[i]["periods"] + "期:");
        //                    // builder.Append("第" + dss.Tables[0].Rows[i]["periods"] + "期:"); 
        //                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UserId + "") + "\">" + username + "(" + model.UserId + ")" + "</a>.");
        //                    builder.Append("时间:" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff"));

        //                }
        //                else
        //                        if (Convert.ToInt32(dss.Tables[0].Rows[i]["GoodsImg"]) == 5)
        //                {
        //                    builder.Append("[五万财主]" + "第" + dss.Tables[0].Rows[i]["periods"] + "期:");
        //                    // builder.Append("第" + dss.Tables[0].Rows[i]["periods"] + "期:"); 
        //                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UserId + "") + "\">" + username + "(" + model.UserId + ")" + "</a>.");
        //                    builder.Append("时间:" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff"));

        //                }
        //                else
        //                            if (Convert.ToInt32(dss.Tables[0].Rows[i]["GoodsImg"]) == 1)
        //                {
        //                    builder.Append("[一万小农]" + "第" + dss.Tables[0].Rows[i]["periods"] + "期:");
        //                    // builder.Append("第" + dss.Tables[0].Rows[i]["periods"] + "期:"); 
        //                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UserId + "") + "\">" + username + "(" + model.UserId + ")" + "</a>.");
        //                    builder.Append("时间:" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff"));

        //                }
        //                k++;
        //                //   builder.Append(Out.Tab("</div>", "<br/>"));
        //            }
        //        }
        //   }
        // builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + ds.Tables[0].Rows[i]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ds.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 【百万富翁房】</a><br/>");
        //+ "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + ds.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "富翁榜" + "</a>" + "<br/>");
        builder.Append("奖金:100万|1000币*1000人次" + "<br/>");
        builder.Append("第" + ds.Tables[0].Rows[0]["periods"] + "期|" + "当前奖池:" + Convert.ToInt32(ds.Tables[0].Rows[0]["Number"]) * Convert.ToInt32(ds.Tables[0].Rows[0]["statue"]) + "币" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ds.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 参与</a>" + "<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsTen.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 【十万地主房】</a><br/>");
        //+ "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + dsTen.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "地主榜" + "</a>" + "<br/>");
        builder.Append("奖金:10万|1000币*100人次" + "<br/>");
        builder.Append("第" + dsTen.Tables[0].Rows[0]["periods"] + "期|" + "当前奖池:" + Convert.ToInt32(dsTen.Tables[0].Rows[0]["Number"]) * Convert.ToInt32(dsTen.Tables[0].Rows[0]["statue"]) + "币" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsTen.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 参与</a>" + "<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsFive.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 【五万财主房】</a><br/>");
        //+ "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + dsFive.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "财主榜" + "</a>" + "<br/>");
        builder.Append("奖金:5万|500币*100人次" + "<br/>");
        builder.Append("第" + dsFive.Tables[0].Rows[0]["periods"] + "期|" + "当前奖池:" + Convert.ToInt32(dsFive.Tables[0].Rows[0]["Number"]) * Convert.ToInt32(dsFive.Tables[0].Rows[0]["statue"]) + "币" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsFive.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 参与</a>" + "<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsOne.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 【一万小农房】</a><br/>");
        //+ "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + dsOne.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "小农榜" + "</a>" + "<br/>");
        builder.Append("奖金:1万|100币*100人次" + "<br/>");
        builder.Append("第" + dsOne.Tables[0].Rows[0]["periods"] + "期|" + "当前奖池:" + Convert.ToInt32(dsOne.Tables[0].Rows[0]["Number"]) * Convert.ToInt32(dsOne.Tables[0].Rows[0]["statue"]) + "币" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + dsOne.Tables[0].Rows[0]["Id"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 参与</a>" + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buylist") + "\">最新参与记录&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        try
        {
            int buyListCount = Convert.ToInt32(ub.GetSub("BuyListCount", xmlPath));
            int l = new BCW.BLL.yg_BuyLists().GetMaxId() - 1;
            for (int j = 0; j < buyListCount; j++)
            {
                BCW.Model.yg_BuyLists tem = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(--l);
                BCW.Model.GoodsList good = new BCW.BLL.GoodsList().GetGoodsList(tem.GoodsNum);
                builder.Append(Out.Tab("<div>", ""));
                //  builder.Append(j + 1);
                builder.Append(DateStringFromNow(Convert.ToDateTime(tem.BuyTime)));
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(tem.UserId));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + tem.UserId + "") + "\">" + mename + "</a>");
                builder.Append("参与了");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + tem.GoodsNum + "&amp;backurl=" + Utils.PostPage(0) + "") + "\">" + good.GoodsName + "</a>");
                builder.Append(",数量(" + tem.Counts + ")");
                // builder.Append("时间" + Convert.ToDateTime(tem.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff"));
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        catch //(Exception e)
        {
            builder.Append("无相关记录...<br/>");
        }
        // catch { }
        // builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    //新鲜玩意
    private void newThings()
    {
        Master.Title = "新鲜玩意";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;新鲜玩意" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int isOpen = Convert.ToInt32(ub.GetSub("isOpenNewThings", xmlPath));//开放为0，关闭为1
        if (isOpen != 1)//关闭
        {
            builder.Append("该功能尚未开放，敬请期待！");
        }
        else
        {
            ;//新鲜玩意功能
        }
        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx") + "\">立即参与&gt;&gt;</a><br/>");

        //底部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //最新公告
    private void gonggao()
    {
        Master.Title = "公告栏";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //  builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;" + "公告栏" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int isOpen = Convert.ToInt32(ub.GetSub("UserSell", xmlPath));//开放为1，关闭为0
        if (isOpen == 1)//关闭
        {
            builder.Append("暂无最新公告，敬请期待！");
        }
        else
        {
            string gonggao = Convert.ToString(ub.GetSub("KbygGonggao", xmlPath));
            builder.Append("<div>" + gonggao + "</div>");
            // int meid = new BCW.User.Users().GetUsId();
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页</a><br/>");
        builder.Append(Out.Tab("</div>", ""));
        //底部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    //玩家出售商品过期处理
    private void SearchReturnGoods()
    {
        string str = " isDone<>3 and isDone<>0 and OverTime< '" + DateTime.Now + "'";
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
        // int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
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
                                string strLog = "您在[URL=/bbs/game/kbyg.aspx]"+Gamename+"[/URL]因商品第" + model.periods + "期" + model.GoodsName + "过期未开奖返还了" + gold + "" + "酷币" + "[URL=/bbs/game/kbyg.aspx?act=geren]去查看吧[/URL]";
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
                                string strLog = "您在[URL=/bbs/game/kbyg.aspx]”+Gamename+“[/URL]因商品第" + model.periods + "期" + model.GoodsName + "过期未开奖返还了" + gold + "" + "币" + "[URL=/bbs/game/kbyg.aspx?act=geren]去查看吧[/URL]";
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
                            new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.Identification), rettur, Gamename+"出售" + model.Id + "未开奖返还");
                        }
                        else if (statue == 2)
                        {
                            new BCW.SWB.BLL().UpdateMoney(Convert.ToInt32(model.Identification), rettur, 1007);
                        }
                    }
                }
                // if (statue == 2)//测试
                {
                }
            }
            catch (Exception ee)
            {
                builder.Append(ee + "没有相关过期记录..");
                new BCW.BLL.Guest().Add(727, "测试727", "未成功返还商品ID" + model.Id);
            }
        }
        catch
        {
            //Utils.Success("返还结果", "返还失败,正在返回上级.", Utils.getPage("kbyg.aspx?act=returntime"), "1"); 
        }
    }

    //我要出售
    private void Tosell()
    {
        try
        {
            SearchReturnGoods();
            OpenGoods();
        }
        catch
        {

        }
        Master.Title = "出售";
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));
        int info = Utils.ParseInt(Utils.GetRequest("info", "all", 1, @"^[1-9]\d*$", "0"));
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;出售" + "<br/>");
        // builder.Append("我要出售");
        builder.Append(Out.Tab("</div>", ""));
        int isOpen = Convert.ToInt32(ub.GetSub("isOpenIWantSell", xmlPath));
        int SellSmall = Convert.ToInt32(ub.GetSub("SellSmall", xmlPath));
        int BigSell = Convert.ToInt32(ub.GetSub("BigSell", xmlPath));
        int KbygPersan = Convert.ToInt32(ub.GetSub("KbygPersan", xmlPath));
        int addGoodsValue = Convert.ToInt32(ub.GetSub("addGoodsValue", xmlPath));
        //支付安全提示
        string[] p_pageArr = { "ptype", "act", "info" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("查看我的");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=chushou") + "\">出售记录</a>" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        try
        {
            if (isOpen == 1)
            {
                builder.Append("该功能尚未开放，敬请期待！");
                // builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回购物大厅...</a>");
            }
            else
            {
                //我要出售功能
                if (info == 0)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("请选择出售商品的类别:" + "<br/>");
                    builder.Append("1." + "<a href=\"" + Utils.getUrl("kbyg.aspx?info=1&amp;act=sell&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">币值</a>.");
                    //  builder.Append("2." + "<a href=\"" + Utils.getUrl("kbyg.aspx?info=2&amp;act=sell&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">物件</a>.<br />");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    if (info == 1 && ptype == 1)
                    {
                        //币值
                        string ac = Utils.GetRequest("ac", "all", 1, "", "");
                        BCW.Model.GoodsList model = new BCW.Model.GoodsList();
                        if (Utils.ToSChinese(ac) == "确定发布")
                        {
                            //是否刷屏
                            //string appName = "LIGHT_MRYG";
                            //int Expir = Utils.ParseInt(ub.GetSub("KbygExpir", xmlPath));
                            //BCW.User.Users.IsFresh(appName, 6000);
                            string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,20}$", "商品名称限1-20字内");
                            string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,2000}$", "商品描述限2000字内");
                            string GoodsType = Utils.GetRequest("GoodsType", "post", 2, @"^[0-9]$", "商品状态选择出错");
                            string Value = Utils.GetRequest("Value", "post", 4, @"^[1-9]\d*$", "商品总价值填写错误(整数)");
                            string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[1-9]\d*$", "商品最大购买量填写错误(整数)");
                            string liuyan = Utils.GetRequest("liuyan", "post", 3, @"^[^\^]{1,200}$", "留言限200字内");
                            string times = (Utils.GetRequest("times", "post", 2, @"^[0-3]$", "时长选择出错"));
                            if (Convert.ToInt64(Value) > BigSell)//允许1万以上1一百万以下
                            {
                                Utils.Error("输入的币值" + "过大", "");
                            }
                            if (Convert.ToInt32(Value) < SellSmall)
                            {
                                Utils.Error("输入的币值" + "过小", "");
                            }
                            DateTime over = DateTime.Now;
                            if (times == "1")
                            {
                                over = DateTime.Now.AddDays(1);
                            }
                            else
                                if (times == "2")
                                { over = DateTime.Now.AddDays(7); }
                                else
                                { over = DateTime.Now.AddDays(30); }
                            // int UserSellGetPersan = Convert.ToInt32(ub.GetSub("UserSellGetPersan", xmlPath));
                            int needGolds = Convert.ToInt32(Convert.ToInt32(Value) / Convert.ToInt32(GoodsValue));
                            // DateTime now=new DateTime();
                            model.GoodsName = GoodsName;
                            model.explain = explain;
                            model.GoodsImg = "0";//置空
                            model.ImgCounts = 0;
                            model.periods = 1;//第一期
                            model.Number = 0;//置空
                            model.GoodsValue = Convert.ToInt64(GoodsValue) + addGoodsValue;// +10;
                            model.GoodsType = Convert.ToInt32(GoodsType);
                            model.Winner = "0";//置空
                            model.lotteryTime = DateTime.Now;
                            model.RemainingTime = DateTime.Now;
                            model.OverTime = over;
                            model.isDone = 5;//申请为5
                            model.GoodsFrom = liuyan;//留言
                            model.Identification = meid;
                            model.StockYungouma = "0";
                            model.statue = needGolds;
                            model.Addtime = DateTime.Now;
                            model.GoodsSell = Convert.ToInt32(Value);
                            long needgold = Convert.ToInt32(Value); //(Convert.ToInt64(GoodsValue) * Convert.ToInt64(model.statue));
                            try
                            {
                                string mename = new BCW.BLL.User().GetUsName(meid);
                                if (ub.GetSub("KbygStatus", xmlPath) == "0")//正常
                                {
                                    //判断用户钱是否达到出售条件
                                    //bzType = 0;
                                    string bzText = ub.Get("SiteBz");
                                    long gold = new BCW.BLL.User().GetGold(meid);
                                    if (needgold > gold)
                                    {
                                        Utils.Error("你的" + bzText + "不足", "");
                                    }
                                    int gid = new BCW.BLL.GoodsList().Add(model);
                                    new BCW.BLL.User().UpdateiGold(meid, -needgold, "云购出售标识" + gid);
                                    new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + meid + "") + "\">" + mename + "</a>" + "申请出售币值" + needgold + "编号" + gid);//向系统发内线
                                    new BCW.BLL.Action().Add(GameId, gid, meid, mename, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]申请发售" + "**" + "币");
                                    //发内线
                                    // string strLog = "您在[URL=/bbs/game/kbyg.aspx]"+ Gamename +"[/URL]发布了币值" + Convert.ToInt64(GoodsValue) + "" + ub.Get("SiteBz") + "[URL=/bbs/game/kbyg.aspx?act=geren]去查看吧[/URL]";
                                    //   new BCW.BLL.Guest().Add(0, meid, mename, strLog);//异常                       
                                    Utils.Success("恭喜", "成功发布币值,审核通过后即将上架,敬请期待." + "<br/>" + "正在返回首页...", Utils.getUrl("kbyg.aspx?act=sell&amp;"), "2");
                                }
                                else//测试运行
                                {
                                    long god = new BCW.SWB.BLL().GeUserGold(meid, GameId);
                                    if (needgold > god)
                                    {
                                        Utils.Error("你的" + "云币" + "不足", "");
                                    }
                                    else
                                    {
                                        new BCW.SWB.BLL().UpdateMoney(meid, (-needgold), GameId);
                                        //向系统发内线
                                        new BCW.BLL.Guest().Add(1, 10086, "酷爆网客服", "云购玩家" + mename + "ID" + meid + "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]测试中申请出售云币值" + needgold + "消息001");
                                        new BCW.BLL.Action().Add(GameId, 0, meid, mename, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]申请发售" + "**" + "云币");
                                        //发内线
                                        // string strLog = "您在[URL=/bbs/game/kbyg.aspx]每日云购[/URL]测试发布了币值" + Convert.ToInt64(GoodsValue) + "云币" + "[URL=/bbs/game/kbyg.aspx?act=geren]去查看吧[/URL]";
                                        //  new BCW.BLL.Guest().Add(0, meid, mename, strLog);
                                        new BCW.BLL.GoodsList().Add(model);
                                    }
                                    Utils.Success("恭喜", "成功发布币值,审核通过后即将上架,敬请期待." + "<br/>" + "正在返回首页..." + "", Utils.getUrl("kbyg.aspx?act=sell&amp;info=1&amp;ptype=1&amp;"), "3");
                                }
                            }
                            catch (Exception ex)
                            {
                                builder.Append("当前人数较多,发布不成功,请返回" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=sell;backurl=" + Utils.getPage(0) + "") + "\">重新发布</a>");
                                // new BCW.BLL.Guest().Add(727, "测试727", "发布有误！" +meid+ ex.ToString());
                            }
                        }
                        else
                        {
                            //是否刷屏
                            string appName = "LIGHT_MRYG";
                            int Expir = Utils.ParseInt(ub.GetSub("KbygExpir", xmlPath));
                            BCW.User.Users.IsFresh(appName, 10);
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("注:" + "<br/>");
                            builder.Append("您选择了币值，允许范围" + SellSmall + "-" + BigSell + "请正确完成以下输入:" + "<br/>");
                            builder.Append("系统将增加10人次到你的人次中,其中" + (10 - KbygPersan) + "人次将作为你的收益." + "<br/>");
                            builder.Append("如你将出售10000币,参与人次100,则发布后总币值不变,将平均出售你的币值,参与者支付100币每人次,满足参与人次110后成功开奖,你将收益400." + "<br/>");
                            builder.Append("提交币值后系统将扣压相应的币值,并保留该记录至少12小时.");
                            // builder.Append("并收取千分之" + KbygPersan + "的手续费."+"<br/>");
                            builder.Append("出售完毕后将返回您相应的币值.");
                            builder.Append(Out.Tab("</div>", ""));
                            string strText = "输入标题(可留空):/,币值描述(可留空):/,总币值(最低" + SellSmall + "币):/,参与人次:/,单次:/,出售时长:/,留言:/,";
                            string strName = "GoodsName,explain,Value,GoodsValue,GoodsType,times,liuyan,act";
                            string strType = "text,text,num,select,select,select,textarea,act";
                            string strValu = "请输入标题" + "'" + "请输入描述" + "'" + SellSmall + "'" + 100 + "'" + 7 + "'" + 1 + "'" + "留言可留空..." + "'" + Utils.getPage(0) + "";
                            string strEmpt = "false,true,true,10|10|30|30|40|40|70|70|90|90|100|100,7|仅一次,1|一天|2|七天|3|一个月,true";
                            string strIdea = "";
                            string strOthe = "确定发布|reset,kbyg.aspx?info=1&amp;act=sell&amp;ptype=1&amp;,post,1,red|blue";
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                        }
                    }
                    else
                        if (info == 2 && ptype == 2)
                        {
                            //物件
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("您选择了物件，请正确完成以下输入:" + "<br/>");
                            builder.Append("物件功能尚未开放，敬请期待...");
                            builder.Append(Out.Tab("</div>", ""));
                        }

                }
            }
        }
        catch { }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        //底部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //出售结果
    private void SellResult()
    {
        Master.Title = "出售结果";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("出售结果");
        builder.Append(Out.Tab("</div>", "<br/>"));
        try
        {
            SearchReturnGoods();
            OpenGoods();
        }
        catch
        {
            //  builder.Append(ee);
        }
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "Identification=" + meid;
            string[] pageValUrl = { "act", "backurl" };
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
                    builder.Append(k + ".您");
                    builder.Append("出售了:" + n.GoodsName + "[" + "编号" + n.Id + "]");
                    builder.Append("时间" + Convert.ToDateTime(n.Addtime).ToString("yyyy-MM-dd HH:mm:ss"));
                    builder.Append("<br/>" + "信息:");
                    if (n.GoodsType % 2 == 0)
                    {
                        builder.Append("[自动循环]");
                        if (n.isDone == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=updategoods&amp;ptype=" + n.Id + "&amp;leixing=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">[停]</a>");
                        }
                    }
                    else
                    { builder.Append("[不循环]"); }
                    // builder.Append("<br/>");
                    //+"当前状态:");
                    if (n.isDone == 6)
                    {
                        builder.Append("[已取消,币值已返还]");
                        //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">取消审核</a><br/>");
                    }
                    if (n.isDone == 5)
                    {
                        builder.Append("[审核中]");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=cancel&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">取消审核</a>");
                    }
                    else
                        if (n.isDone == 4)
                        {
                            builder.Append("[审核不通过,币值已返还]");
                            // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看原因</a><br/>");
                        }
                        else
                            if (n.isDone == 3)
                            {
                                builder.Append("[已过期,所有币值已返还]");
                                // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看原因</a><br/>");
                            }
                            else
                                if (n.isDone == 0)
                                {

                                    BCW.Model.yg_BuyLists mod = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt32(n.Winner));
                                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(mod.UserId));
                                    builder.Append("[已售出]获得者:" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + mod.UserId + "") + "\">" + mename + "</a>");
                                    builder.Append("购买量(" + mod.Counts + ")");
                                    builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看结果</a>");
                                }
                                else
                                    if (n.isDone == 1)
                                    {
                                        builder.Append("[进行中]");
                                        builder.Append("[总人次:" + n.GoodsValue + "]");
                                        builder.Append("[当前:" + n.Number + "]");
                                        builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "查看详情" + "</a>");
                                    }
                                    else
                                        if (n.isDone == 2)
                                        {
                                            builder.Append("商品已成功售出,正在计算开奖中...");
                                            builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看开奖</a>");
                                        }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无相关出售记录."));
                builder.Append("<br/>");
            }
        }
        catch { }
        try { SearchReturnGoods(); }
        catch { }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //修改商品状态
    private void UpdateGoods()
    {
        Master.Title = "修改商品状态";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">每日云购</a>&gt;");
        //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">个人中心</a>");
        builder.Append("修改商品状态" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        long ptype = Convert.ToInt64(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int leixing = Utils.ParseInt(Utils.GetRequest("leixing", "get", 1, @"^[1-9]\d*$", "0"));//类型
        //  builder.Append(ptype +","+ leixing);
        if (leixing == 1)//玩家币值循环停止
        {
            try
            {

                new BCW.BLL.GoodsList().UpdateGoodsType(ptype, 7);
                string mename = new BCW.BLL.User().GetUsName(meid);
                //new BCW.BLL.User().UpdateiGold(meid, coast, "云购购买商品消费");
                //动态
               // new BCW.BLL.Action().Add(GameId, Convert.ToInt32(ptype), meid, mename, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]取消了下一期的币值发售,将在本期开奖后返还币值到你的账户中,请留意查看");
                //向系统发内线
                new BCW.BLL.Guest().Add(GameId, 10086, "酷爆网客服", "在[URL=/bbs/game/kbyg.aspx]云购[/URL]取消了下一期的编号" + ptype + "币值发售");
                Utils.Success("恭喜", "成功停止,下一期将停止循环." + "正在返回..." + "", Utils.getUrl("kbyg.aspx?act=geren&amp;"), "1");
            }
            catch (Exception e) { builder.Append(e); }
        }
        if (leixing == 2)//玩家商品循环停止
        {
            try
            {
                new BCW.BLL.GoodsList().UpdateGoodsType(ptype, 9);
            }
            catch (Exception e) { builder.Append(e); }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        //   builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">个人中心</a>");
        // builder.Append("修改商品状态" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //我的云购历史
    private void myYungouList()
    {

        Master.Title = "云购历史";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;云购历史" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere;
            if (uid == 0)
            {
                strWhere = "UserId= " + meid + "";
            }
            else
            {
                strWhere = "UserId= " + uid + "";
            }
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
                    //   try
                    {
                        BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt64(n.GoodsNum));
                        string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                        builder.Append(DateStringFromNow(Convert.ToDateTime(n.BuyTime)));
                        //builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + name + "</a>");
                        builder.Append("您参与了" + " <a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.Counts + "</a>" + "人次" + "(" + "编号" + n.Id + ")");
                        builder.Append("第" + model.periods + "期");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
                        builder.Append("(" + "编号" + model.Id + ")");// + "时间" + Convert.ToDateTime(n.BuyTime).ToString("MM-dd HH:mm:ss"));
                        // builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看云购码...</a><br/>");   
                        //  builder.Append("是否中奖:");
                        //1中0不中
                        if (n.Address == "1")
                        {
                            //DataSet dd = new BCW.BLL.GoodsList().GetList(" CONVERT(varchar(100), GETDATE(), 120) as timess", "");
                            //DateTime nowtime = Convert.ToDateTime(dd.Tables[0].Rows[0]["timess"]);//DateTime.Now;
                            //DateTime dtNow = Convert.ToDateTime(model.RemainingTime);
                            //   if (DateTime.Compare(nowtime, dtNow) < 0)
                            if (model.isDone == 2)
                            {
                                builder.Append("[开奖中]");//显示开奖时间
                                //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                                //{
                                //    DateTime now = DateTime.Now;
                                //    DateTime dt = Convert.ToDateTime(model.RemainingTime);
                                //    TimeSpan ts = dt - now;
                                //    if (ts.Minutes == 0)
                                //    { builder.Append(ts.Seconds); }
                                //    else
                                //    { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                                //}
                                //else
                                //{
                                //    string ss = model.Id.ToString();
                                //    string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(model.RemainingTime));
                                //    builder.Append(time);
                                //}
                            }
                            else
                            {
                                if (model.isDone == 3)
                                {
                                    builder.Append("[过期并返还]");
                                }
                                else
                                    if (model.isDone == 0)
                                    {
                                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=geren&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "[中]" + "</a>");
                                    }
                            }
                        }
                        //不中或领奖
                        else
                        {
                            if (model.isDone == 1)
                            { builder.Append("[进行中]"); }
                            if (model.isDone == 3)
                            { builder.Append("[过期并返还]"); }
                            else
                            {
                                if (n.Address == "0" && model.isDone != 2)
                                { builder.Append("[否]"); }
                                else
                                    if (model.isDone == 2)
                                    {
                                        builder.Append("[开奖中]");
                                    }
                                    else
                                        if (n.Address == "2")
                                        {
                                            builder.Append("[已兑奖]");
                                        }
                                        else if (n.Address == "1")
                                        {
                                            builder.Append("[中]");
                                        }
                            }
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // catch { }
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("<div>", "没有相关记录."));
            }
        }
        catch
        {
            //builder.Append(Out.Div("<div>", ""));
            //builder.Append("暂无相关记录...");
            //builder.Append(Out.Div("</div>", ""));
        }

        // builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx") + "\">立即参与&gt;&gt;</a><br/>");
        //底部
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //搜索商品结果
    protected void FindGoods()
    {

        string Title = Utils.GetRequest("uid", "post", 2, @"^[\s\S]{1,15}$", "搜索主题15字内");
        //int word = Utils.ParseInt(Utils.GetRequest("uid", "get", 1, @"", "0"));//对应的商品编号
        //  builder.Append("搜索结果：" + Title);
        Master.Title = "搜索" + Title + "结果";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        // builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("搜索" + Title + "结果");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int newGoods = new BCW.BLL.GoodsList().GetMaxId();
        string Logo = ub.GetSub("img", xmlPath);
        int ListCount = Convert.ToInt32(ub.GetSub("GoodsListCount", xmlPath));
        string imgCount = ub.GetSub("imgCount", xmlPath);
        string strWhere;
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            if (IsNumber(Title, 32, 0))
            {
                strWhere = "Id =" + Convert.ToInt64(Title);
            }
            else
            {
                strWhere = "GoodsName like" + "'%" + Title + "%'";
            }
            // strWhere+="and explain like" +"'%" + Title + "%'";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.GoodsList> listSSCpay = new BCW.BLL.GoodsList().GetGoodsLists(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.GoodsList model in listSSCpay)
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
                    builder.Append(k + ".");
                    if (model.isDone == 1)
                    {
                        builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期[进行中]");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>" + "<br/>");
                        // builder.Append("第" + model.periods + "期" + "正在进行中..");

                        // ListCount -= 1;
                        if (model.GoodsImg != "0")
                        {
                            builder.Append("<br/>");
                            string[] imgNum = model.GoodsImg.Split(',');
                            //  foreach (string n in imgNum)
                            for (int c = 0; c < imgNum.Length - 1; c++)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c] + "\"  width=\"50\" height=\"40\" alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp");
                            }
                        }
                        // builder.Append("名称：" + model.GoodsName + "<br/>");
                        builder.Append("云购标识：" + model.Id + "<br/>");
                        builder.Append("商品描述：" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
                        builder.Append("总币值：" + model.GoodsSell + "<br/>");
                        builder.Append("已参与:" + "&nbsp;" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Number + "</a>" + "&nbsp;" + " 总需人次:" + model.GoodsValue + " 剩余人次:" + (model.GoodsValue - model.Number) + "<br/>");
                        builder.Append("上架时间：" + model.Addtime + "<br/>");
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(0) + "") + "\">立即参与&gt;&gt;</a><br/>");
                    }
                    else
                    {
                        builder.Append("云购标识：" + model.Id + "<br/>");
                        builder.Append("名称：" + model.GoodsName + "<br/>");
                        builder.Append("商品描述：" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
                        builder.Append("总币值：" + model.GoodsSell + "<br/>");
                        builder.Append("已参与:" + "&nbsp;" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Number + "</a>" + "&nbsp;&nbsp;" + " 总需人次:" + model.GoodsValue + " 剩余人次:" + (model.GoodsValue - model.Number) + "<br/>");
                        if (model.Identification > 0)
                        {
                            builder.Append("出售方：" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.Identification + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(model.Identification)) + "</a>(" + model.Identification + ")<br/>");
                        }
                        builder.Append("上架时间：" + model.Addtime + "<br/>");
                        if (Convert.ToInt32(model.Winner) > 0)
                        {

                            BCW.Model.yg_BuyLists tem = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(model.Winner));
                            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(tem.UserId));
                            builder.Append("获奖记录列：" + model.Winner + "<br/>");
                            builder.Append("获奖者：");
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + tem.UserId + "") + "\">" + mename + "</a>");
                            builder.Append("(" + tem.UserId + ")<br/>");
                            builder.Append("开奖时间：" + model.RemainingTime + "<br/>");
                        }
                        builder.Append("搜索结果：" + "该记录已过期!" + "<br/>");
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(0) + "") + "\">立即参与&gt;&gt;</a>");
                    }
                    k++; //BCW.User.Users.SetUser(n.Winner) 
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }// 分页  
                //  builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append("找不到关于" + Title + "的相关记录." + "<br/>");
            }
        }
        catch
        {
            builder.Append("找不到关于" + Title + "的相关记录." + "<br/>");
            //System.Console.WriteLine("传递过来的异常值为：{0}", e);
            //builder.Append(Out.Div("div", "找不到关于" + Title + "的相关记录."));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?") + "\">返回首页</a><br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    //商品历史期数
    protected void GoodsListPeriods()
    {
        Master.Title = "历史期数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("历史期数");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        try
        {
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
            BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(ptype);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "GoodsName like" + "'%" + goods.GoodsName + "%'";
            string Logo = ub.GetSub("img", xmlPath);
            string[] pageValUrl = { "ptype", "act", "backurl" };
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
                    if (n.isDone == 4)
                    {
                        // builder.Append("(审核不通过...)" + "<br/>");
                        continue;
                    }
                    if (n.isDone == 5)
                    {
                        continue;
                        // builder.Append("(正在快速审核中...)" + "<br/>");
                    }
                    builder.Append(k + ".第" + n.periods + "期");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                    if (n.isDone == 1)
                    {
                        builder.Append("[进行中])");
                        builder.Append("<br/>");
                    }
                    if (n.isDone == 2)
                    {
                        builder.Append("[开奖中]倒计时:");
                        DataSet dd = new BCW.BLL.GoodsList().GetList(" CONVERT(varchar(100), GETDATE(), 120) as timess", "");
                        //  builder.Append(dd.Tables[0].Rows[0]["timess"]);
                        DateTime nowtime = Convert.ToDateTime(dd.Tables[0].Rows[0]["timess"]);
                        //  DateTime nowtime = DateTime.Now;
                        DateTime dtNow = Convert.ToDateTime(n.lotteryTime);
                        if (DateTime.Compare(nowtime, dtNow) <= 0)
                        {
                            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                            {
                                DateTime now = nowtime;
                                DateTime dt = Convert.ToDateTime(n.lotteryTime);
                                TimeSpan ts = dt - now;
                                if (ts.Minutes == 0)
                                { builder.Append(ts.Seconds); }
                                else
                                { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                            }
                            else
                            {
                                string ss = n.Id.ToString();
                                string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(n.RemainingTime));
                                builder.Append(time);
                            }
                        }
                        builder.Append("<br/>");
                    }
                    if (n.isDone == 0)
                    {
                        builder.Append("[已揭晓]" + "<br/>");
                    }
                    //if (n.GoodsImg.ToString() != "0" && n.GoodsImg.ToString() != "100" && n.GoodsImg.ToString() != "5" && n.GoodsImg.ToString() != "10" && n.GoodsImg.ToString() != "1")
                    if (n.GoodsImg.ToString() != "0")
                    {
                        if (!(n.GoodsImg.ToString() == "1" || n.GoodsImg.ToString() == "5" || n.GoodsImg.ToString() == "10" || n.GoodsImg.ToString() == "100"))
                        {
                            builder.Append("图片描述：" + "<br/>");
                            string[] imgNum = n.GoodsImg.Split(',');
                            // foreach (string m in imgNum)
                            for (int c = 0; c < imgNum.Length - 1; c++)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c] + "\"  width=\"50\" height=\"40\" alt=\"load\" border=\"10\" border-color=\"#C0C0C0\" />" + "</a>&nbsp;&nbsp;&nbsp;");
                            }
                            builder.Append("<br/>");
                        }
                    }
                    builder.Append("商品描述：" + "<br/>" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(n.explain)) + "<br/>" + "已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Number + "</a>" + "总需人次:" + n.GoodsValue + " 剩余人次:" + (n.GoodsValue - n.Number) + "<br/>" + " 所需：" + n.statue + ub.Get("SiteBz") + "/每人次" + "<br/>");
                    if (n.isDone == 0)
                    {
                        BCW.Model.yg_BuyLists mod = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt32(n.Winner));
                        string username = new BCW.BLL.User().GetUsName((Convert.ToInt32(mod.UserId)));
                        builder.Append("获奖玩家：" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + mod.UserId + "") + "\">" + username + "</a><br/>");
                        builder.Append("时间：" + Convert.ToDateTime(mod.BuyTime).ToString("yyyy-MM-dd HH:mm:ss") + "数量：" + mod.Counts);
                    }
                    if (n.isDone == 1)
                    {
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "") + "\">立即参与&gt;&gt;</a>");
                    }
                    if (n.isDone == 2)
                    {
                        builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "") + "\">立即围观&gt;&gt;</a>");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("没有相关记录");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch { builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx") + "\">没有相关记录</a><br/>"); }
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回主页&gt;&gt;</a><br/>");
        //builder.Append(Out.Tab("</div>", ""));
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //最新揭晓商品
    protected void newOpenList()
    {
        Master.Title = "最新揭晓";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;最新揭晓" + "<br/>");
        //  builder.Append("最新揭晓" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "isDone=0";
            string[] pageValUrl = { "act", "backurl" };
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
                        builder.Append(Out.Tab("<div >", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div class=\"text\">", ""));
                        else
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    }
                    builder.Append(DateStringFromNow(Convert.ToDateTime(n.lotteryTime)));
                    builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.periods + "</a>" + "期");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
                    BCW.Model.yg_BuyLists mod = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt32(n.Winner));
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(mod.UserId));
                    builder.Append(",恭喜玩家" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + mod.UserId + "") + "\">" + mename + "</a>" + "幸运获奖");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }// 分页  
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("<div>", "没有相关记录.."));
            }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("无相关记录...");
            builder.Append(Out.Tab("</div>", ""));
        }
        //builder.Append(Out.Tab("</div>", ""));
        //finally { builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页&gt;</a><br/>"); }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    //最新云购记录
    protected void newBuyList()
    {
        Master.Title = "最新云购记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("最新云购记录");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (meid == 0)
            Utils.Login();
        string strWhere;
        if (uid == 0)
        {
            strWhere = "";
        }
        else
        {
            strWhere = "UserId= " + uid + "";
        }
        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
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
                    BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(Convert.ToInt32(n.GoodsNum));
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                    builder.Append(DateStringFromNow(Convert.ToDateTime(n.BuyTime)));
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>");

                    builder.Append("参与了" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;user=" + n.UserId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.Counts + "</a>" + "人次的");
                    builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
                    //   builder.Append("时间" + Convert.ToDateTime(n.BuyTime).ToString("MM-dd HH:mm:ss fff"));
                    if (model.isDone == 0) { builder.Append("[已揭晓]"); }
                    if (model.isDone == 1) { builder.Append("[进行中]"); }
                    if (model.isDone == 2)
                    {
                        builder.Append("[开奖中]");
                        //DateTime nowtime = DateTime.Now;
                        //DateTime dtNow = Convert.ToDateTime(model.RemainingTime);
                        //if (DateTime.Compare(nowtime, dtNow) <= 0)
                        //{
                        //    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                        //    {
                        //        DateTime now = DateTime.Now;
                        //        DateTime dt = Convert.ToDateTime(model.RemainingTime);
                        //        TimeSpan ts = dt - now;
                        //        if (ts.Minutes == 0)
                        //        { builder.Append(ts.Seconds); }
                        //        else
                        //        { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                        //    }
                        //    else
                        //    {
                        //        string ss = model.Id.ToString();
                        //        string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(model.RemainingTime));
                        //        builder.Append(time);
                        //    }
                        // }
                        // builder.Append("<br/>");
                    }
                    if (model.isDone == 3) { builder.Append("[已过期]"); }
                    // builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.GoodsNum + "") + "\">立即参与&gt;&gt;</a>");
                    //  builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;user=" + n.UserId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看云购码...</a><br/>");
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
        catch { }
        string strText = "按ID搜历史:/,";
        string strName = "uid,backurl";
        string strType = "text,hidden";
        string strValu = uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "";
        string strOthe = "搜历史,kbyg.aspx?act=buylist,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        // builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx") + "\">立即参与&gt;&gt;</a><br/>");
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    //更多商品列
    protected void MoreGoods()
    {
        Master.Title = "更多云购";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;更多云购" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        try
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ub.GetSub("KbygStatus", xmlPath) == "0")//正常
            {
                builder.Append("我的财产:" + "<a href=\"" + Utils.getUrl("../finance.aspx") + "\"> " + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + " </a>" + ub.Get("SiteBz") + "<br/>");
            }
            else
            {
                builder.Append("我的财产:" + new BCW.SWB.BLL().GeUserGold(meid, GameId) + "云币");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=getgold&amp;ptype=" + 0 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + "[领]" + "</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br/>");
            //builder.Append(Out.Tab("</div>", ""));
            string Logo = ub.GetSub("img", xmlPath);
            //int newGoods = new BCW.BLL.GoodsList().GetMaxId();
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "isDone=1";
            string[] pageValUrl = { "act", "backurl" };
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
                    if (n.isDone == 1)
                    {
                        builder.Append(k + ".第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.periods + "</a>" + "期");
                    }
                    builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>" + "[进行中]");
                    if (n.GoodsImg.ToString() != "0" && n.GoodsImg.ToString() != "100" && n.GoodsImg.ToString() != "5" && n.GoodsImg.ToString() != "10" && n.GoodsImg.ToString() != "1")
                    {
                        builder.Append("<br/>" + "图片描述:" + "<br/>");
                        string[] imgNum = n.GoodsImg.Split(',');
                        //  foreach (string m in imgNum)
                        for (int c = 0; c < imgNum.Length - 1; c++)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c] + "\"  width=\"8%\" height=\"8%\" alt=\"load\" border=\"10\" border-color=\"#C0C0C0\" />" + "</a>&nbsp;&nbsp;&nbsp;");
                        }
                    }
                    builder.Append("<br/>" + "商品描述：" + "<br/>" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(n.explain)) + "<br/>" + "已参与:" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Number + "</a>" + "总需人次:" + n.GoodsValue + " 剩余人次:" + (n.GoodsValue - n.Number) + "<br/>" + " 所需:" + n.statue + ub.Get("SiteBz") + "/每人次" + "<br/>");
                    builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.Id + "") + "\">立即夺宝</a>");
                    k++; //BCW.User.Users.SetUser(n.Winner)
                    builder.Append(Out.Tab("</div>", ""));

                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录"));
            }
        }
        catch { }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //浏览商品详情页
    protected void BuyPage()
    {
        Master.Title = "云购商品详情页";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("商品详情页");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        try
        {
            SearchReturnGoods();
        }
        catch { }
        try
        {
            if (ub.GetSub("ceshi", xmlPath) == "2")//酷币版测试
            {
                if (ub.GetSub("KbygStatus", xmlPath) == "0")//测试为2 正常0 维护1
                {
                    string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
                    string[] name = CeshiQualification.Split('#');
                    // foreach (string n in imgNum)
                    bool check = false;
                    for (int n = 0; n < name.Length - 1; n++)
                    {
                        if (name[n].ToString() == meid.ToString())
                        {
                            check = true;

                        }
                    }
                    if (!check)//未有资格
                    {
                        Utils.Error("很抱歉,您暂未有测试该游戏的权限", Utils.getUrl("kbyg.aspx"));
                    }
                }
            }
            string Logo = ub.GetSub("img", xmlPath);
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
            BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(ptype);
            //if (model.Identification != 0 && model.isDone != 1)
            //{
            //    Utils.Error("请求的参数错误！", Utils.getUrl("kbyg.aspx"));
            //}
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + (model.Id - 1) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "上一云" + "</a>" + ".");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + (model.Id + 1) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "下一云" + "</a><br/>");
            builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.periods + "</a>" + "期");
            builder.Append("&nbsp;&nbsp;" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (model.isDone == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("[已揭晓]");//+ "时间:" + (Convert.ToDateTime(model.RemainingTime).ToString("yyyy-MM-dd hh:mm:ss fff")));
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (model.isDone == 1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("[进行中]"); builder.Append("[开奖进度:" + (double)(model.Number * 100 / model.GoodsValue) + "%]");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (model.isDone == 2)
            {
                //  builder.Append(Out.Tab("<div>", ""));
                builder.Append("[开奖中]倒计时 ");
                DataSet dd = new BCW.BLL.GoodsList().GetList(" CONVERT(varchar(100), GETDATE(), 120) as timess", "");
                DateTime nowtime = Convert.ToDateTime(dd.Tables[0].Rows[0]["timess"]);//DateTime.Now;
                DateTime dtNow = Convert.ToDateTime(model.lotteryTime);
                if (DateTime.Compare(nowtime, dtNow) <= 0)//正在倒计时
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    {
                        DateTime now = DateTime.Now;
                        DateTime dt = Convert.ToDateTime(model.lotteryTime);
                        TimeSpan ts = dt - now;
                        if (ts.Minutes == 0)//color : #f00;
                        { builder.Append("<font color=\"red\">" + ts.Seconds + "秒" + "</font>"); }
                        else
                        { builder.Append("<font color=\"red\">" + ts.Minutes + "分" + ts.Seconds + "秒" + "</font>"); }
                    }
                    else
                    {
                        string ss = model.Id.ToString();
                        string time = new BCW.JS.somejs().newDaojishi(ss, Convert.ToDateTime(model.lotteryTime));
                        builder.Append("<font  color=\"red\"><b>" + time + "</b></font>" + "");
                    }
                }
                else//倒计时完毕
                {
                    try
                    {
                        OpenGoods();
                        isTimeToOpenNew();
                    }
                    catch (Exception ee)
                    { new BCW.BLL.Guest().Add(727, "测试727", "倒计时后开奖有误！" + ee.ToString()); }
                    Response.AddHeader("Refresh", "0");
                }
                //  builder.Append(Out.Tab("</div>", ""));
            }
            if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("图片描述: " + "<br/>");
                string[] imgNum = model.GoodsImg.Split(',');
                // foreach (string n in imgNum)
                for (int c = 0; c < imgNum.Length - 1; c++)
                {
                    builder.Append("<a  target=\"_blank\" href=\"" + imgNum[c] + "\"><img src=\"" + imgNum[c] + "\"  width=\"12%\" height=\"30%\" alt=\"load\" />" + "</a>&nbsp;&nbsp;");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("云购标识: " + model.Id + "<br/>");
            builder.Append("商品描述: " + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.explain)) + "<br/>");
            builder.Append("总价值: " + model.GoodsSell + "<br/>");
            builder.Append("已参与: " + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buyOne&amp;ptype=" + model.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Number + "</a>" + " 总需人次:" + model.GoodsValue + "  剩余人次:" + (model.GoodsValue - model.Number) + "<br/>");
            builder.Append("所需: " + model.statue + "币" + "/每人次" + "<br/>");
            builder.Append("上架日期: " + Convert.ToDateTime(model.Addtime).ToString("yyyy-MM-dd HH:mm:ss") + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            if (model.Identification > 0)
            {
                // builder.Append("出售来源:" + model.Addtime + "<br/>");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("出售方:" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.Identification + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(model.Identification)) + "</a>(" + model.Identification + ")<br/>");
                builder.Append(Out.Tab("</div>", ""));
            }
            TimeSpan passtime = Convert.ToDateTime(model.OverTime) - Convert.ToDateTime(model.Addtime);
            double dayss = Math.Round(passtime.TotalDays, 0);
            builder.Append(Out.Tab("<div>", ""));
            if (dayss < 0)
            { builder.Append("出售时长:" + " 已过期" + "<br/>"); }
            else
            {
                if (model.Identification > 0)
                {

                    builder.Append("出售时长:" + dayss + "天" + "<br/>");
                }
                else
                {
                    builder.Append("出售时长:" + dayss + "天" + "<br/>");
                }
            }
            if (model.GoodsType == 11 || model.GoodsType == 12)
            {
                builder.Append("<b>" + "奖池:" + model.statue * model.Number + "币" + "</b>");
                //builder.Append("每人仅限参与:" + 10 + "次" + "<br/>"); 
            }
            if (model.isDone == 5 && model.Identification > 0)
            {
                builder.Append("状态:审核中...");
            }
            builder.Append(Out.Tab("</div>", ""));
            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
            if (num >= 5)
                num = 1;
            if (model.isDone == 1)
            {
                // builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("参与:");
                for (int i = 1; i <= 5; i++)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=result&amp;ptype=" + model.Id + "&amp;count=" + i + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + i + "</a> ");
                }
                builder.Append("人次" + "");
                // builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("</div>", ""));
                strText = "输入参与人次:/,,";
                strName = "count,act";
                strType = "text,hidden";
                strValu = "'" + Utils.getPage(0) + "";
                strEmpt = "true,false";
                strIdea = "/";
                strOthe = "购买,kbyg.aspx?act=result&amp;ptype=" + model.Id + ",post,0,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                //  builder.Append(Out.Tab("</div>", "<br/>"));
            }
            if (model.isDone == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("开奖时间:" + Convert.ToDateTime(model.lotteryTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br/>");
                builder.Append("获奖计算方式：");
                // builder.Append("如何计算");
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=rule") + "\">如何计算?</a>" + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                //builder.Append("1、取该商品最后购买时间前网站所有商品的最后100条购买时间记录；"+"<br/>");
                //builder.Append("2、按时、分、秒、毫秒排列取值之和，除以该商品总参与人次后取余数；"+"<br/>");
                //builder.Append("3、余数加上10000000 即为“幸运云购码”；"+"<br/>");
                //builder.Append("4、余数是指整数除法中被除数未被除尽部分， 如7÷3 = 2 ......1，1就是余数。" + "<br/>");
                //builder.Append(Out.Tab("</div>", ""));
                BCW.Model.yg_BuyLists mod = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt32(model.Winner));
                string username = new BCW.BLL.User().GetUsName((Convert.ToInt32(mod.UserId)));
                long sum = 0;
                int big = Convert.ToInt32(model.ImgCounts);
                int small = Convert.ToInt32(big - 100);
                string str = "Id>" + small + " and Id<=" + big;
                DataSet ds = new BCW.BLL.yg_BuyLists().GetList("*", str);//获取购买后获奖的100条记录
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sum += Convert.ToInt64(Convert.ToDateTime(ds.Tables[0].Rows[i]["BuyTime"]).ToString("HHmmssfff"));
                    }
                }
                long yushu = sum % Convert.ToInt64(model.GoodsValue);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("恭喜玩家" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + mod.UserId + "") + "\">" + username + "</a>" + "幸运获奖！" + "编号:" + model.Winner + "幸运数字:" + (yushu + 10000000) + "转换值总和:" + sum + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "Id>" + small + "and Id<=" + big;
                string[] pageValUrl = { "act", "ptype", "backurl", };
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        BCW.Model.GoodsList ml = new BCW.BLL.GoodsList().GetGoodsList(n.GoodsNum);
                        //if (Convert.ToInt32(ml.Winner) == Convert.ToInt32(mod.Id))
                        //{
                        //    builder.Append("<b>");
                        //}
                        builder.Append(k + ".");
                        //if (Convert.ToInt32(ml.Winner) == Convert.ToInt32(mod.Id))
                        //{
                        //    builder.Append("</b>");
                        //}
                        builder.Append("编号:" + n.Id + "|会员:");
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>");
                        builder.Append("|详情:" + "第" + ml.periods + "期|");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ml.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ml.GoodsName + "</a>");//+ml.GoodsName);
                        builder.Append("|" + "时间:" + (Convert.ToDateTime(n.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff")) + "|");
                        builder.Append("转换:" + Convert.ToInt32(Convert.ToDateTime(n.BuyTime).ToString("HHmmssfff")) + "|");
                        builder.Append("参与:");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;") + "\">" + n.Counts + "</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    long last = model.Id - 1;
                    long next = model.Id + 1;
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + last + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "上一云" + "</a>" + ".");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + next + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "下一云" + "</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("找不到相关记录.");
                    builder.Append(Out.Tab("</div>", ""));
                }
                // builder.Append("总计算和：" + sum + "<br/>" + "幸运获奖码:" + (sum % model.GoodsValue));
            }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("找不到相关记录.");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">再看看吧</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //底部
        //string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append("<a href=\"" + Utils.getPage("kbyg.aspx?ptype=" + ptype + "") + "\">再看看吧..</a>");
    }

    //指定商品的购买列
    protected void buyOneGoods()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        BCW.Model.GoodsList model = new BCW.BLL.GoodsList().GetGoodsList(ptype);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        try
        {
            Master.Title = model.GoodsName + "已参与详情";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
            builder.Append(model.GoodsName + "已参与详情<br/>");
            builder.Append(Out.Tab("</div>", ""));
            try
            {
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "GoodsNum=" + ptype;
                string[] pageValUrl = { "act", "ptype", "backurl" };
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
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                        builder.Append(k + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>");
                        builder.Append("成功参与了" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;") + "\">" + n.Counts + "</a>" + "人次的");
                        builder.Append("第" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=listPeriods&amp;ptype=" + n.GoodsNum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.GoodsList().Getperiods(n.GoodsNum) + "</a>" + "期的");
                        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.GoodsNum + "") + "\">" + new BCW.BLL.GoodsList().GetGoodsName(n.GoodsNum) + "</a> ");
                        builder.Append("时间" + Convert.ToDateTime(n.BuyTime).ToString("yyyy-MM-dd HH:mm:ss fff") + "(编号" + n.Id + ")");
                        //  builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + n.GoodsNum + "") + "\">立即参与&gt;&gt;</a>");
                        //  builder.Append(" <a href=\"" + Utils.getUrl("kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "&amp;id=" + n.Id + "&amp;") + "\">查看云购码...</a><br/>");
                        k++; //BCW.User.Users.SetUser(n.Winner)
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录." + "<br/>" + "快来分第一杯羹吧，说不定下一个大奖就是你!"));
                }
            }
            catch { }
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">立即参与</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回主页</a><br/>");
            builder.Append(Out.Tab("</div>", ""));

            string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
            //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        catch { builder.Append("无相关记录." + " <a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页</a>"); }
    }

    //查看云购码
    protected void SeeYungoum()
    {
        Master.Title = "云购码详情页";
        string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("云购码详情页");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//购买列id.
        int user = Utils.ParseInt(Utils.GetRequest("user", "get", 1, @"^[1-9]\d*$", "0"));//对应用户
        try
        {
            BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(id);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UserId + "") + "\">" + mename + "</a>");
            builder.Append("本次参与编号ID" + ptype + ",名称");
            builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.GoodsList().GetGoodsName(Convert.ToInt64(ptype)) + "</a>云购码详情");
            builder.Append(Out.Tab("</div>", "<br/>"));
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
                    // builder.Append(n.yungouma+"<br/>");
                    string[] yun = n.yungouma.Split(',');
                    //foreach (string i in yun)
                    int[] y = new int[100];
                    for (int i = 0; i < yun.Length - 1; i++)
                    {
                        // y[i] = Convert.ToInt32(yun[i].ToString().Trim());
                        builder.Append((Convert.ToInt32(yun[i].ToString().Trim()) + 10000000) + "    ");
                        if (i % 4 == 3)
                        { builder.Append("<br/>"); }
                        // builder.Append(yun[i].Trim()+","+yun[i].Length+"...");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("<div>", ""));
                builder.Append("没有相关记录..");
                builder.Append(Out.Div("</div>", ""));
            }
        }
        catch { }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //购买结果
    protected void BuyResult()
    {
        Master.Title = "购买结果";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("购买结果");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int count = Utils.ParseInt(Utils.GetRequest("count", "all", 1, @"^[1-9]\d*$", "0"));//1-5的购买数量      
        //   int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[1-9]\d*$", "0"));
        int ceshi = Convert.ToInt32(ub.GetSub("ceshi", xmlPath));
        if (ub.GetSub("ceshi", xmlPath) == "2")//酷币版测试
        {
            if (ub.GetSub("KbygStatus", xmlPath) == "0")//测试为2 正常0 维护1
            {
                string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
                string[] name = CeshiQualification.Split('#');
                // foreach (string n in imgNum)
                bool check = false;
                for (int n = 0; n < name.Length - 1; n++)
                {
                    if (name[n].ToString() == meid.ToString())
                    {
                        check = true;

                    }
                }
                if (!check)//未有资格
                {
                    Utils.Error("很抱歉,您暂未有测试该游戏的权限", Utils.getUrl("kbyg.aspx"));
                }
            }
        }
        ////是否刷屏
        ////string appName = "LIGHT_KBYG";
        ////int Expir = Utils.ParseInt(ub.GetSub("KbygExpir", xmlPath));
        ////if (Expir > 0)
        ////{
        ////    BCW.User.Users.IsFresh(appName, Expir);
        ////}
        ////支付安全提示
        //string[] p_pageArr = {"ptype", "count", "info", "act"};
        //BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
        //if (ceshi != 0)//ceshi 不为0执行
        //{
        //    if (ub.GetSub("KbygStatus", xmlPath) == "2")
        //    {
        //        string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
        //        string[] name = CeshiQualification.Split('#');
        //        // foreach (string n in imgNum)
        //        bool check = false;
        //        for (int n = 0; n < name.Length-1; n++)
        //        {
        //            if ((name[n].ToString()) == meid.ToString())
        //            {
        //                check = true;
        //            }
        //        }
        //        if (!check)
        //        {
        //            Utils.Error("您暂未有测试资格,请联系客服", Utils.getUrl("kbyg.aspx"));
        //        }
        //    }
        //}
        //if (count == 0)
        //{
        //    if (num == 0)
        //    {
        //        Utils.Success("购买数量不能为0！", "请正确输入购买数量,1s后返回首页...", Utils.getUrl("kbyg.aspx"), "1");
        //    }
        //    else
        //    { count = num; }
        //}
        if (count == 0)
        {
            Utils.Success("购买数量不能为0", "请正确输入购买数量,2s后返回首页...", Utils.getUrl("kbyg.aspx"), "2");
        }
        try
        {
            //BCW.User.Users.ShowVerifyRole("c", meid);//非验证会员提示
            string info = Utils.GetRequest("info", "all", 1, "", "");
            BCW.Model.GoodsList goods = new BCW.BLL.GoodsList().GetGoodsList(ptype);
            BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
            int value = Convert.ToInt32(goods.GoodsValue) + 1;//商品价值
            int buyCount = Convert.ToInt32(goods.Number);//当前购买量
            int shengyu = Convert.ToInt32(goods.GoodsValue) - buyCount;//剩余购买量
            //支付安全提示
            string[] p_pageArr = { "ptype", "count", "info", "act" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            try
            {
                SearchReturnGoods();
            }
            catch { new BCW.BLL.Guest().Add(727, "727", "搜索商品过期有误！"); }
            if (goods.isDone == 3)
            {
                Utils.Error("该商品已过期！请再看看其他的吧！", Utils.getUrl("kbyg.aspx"));
            }
            //string strwhere = "UserId=" + meid + "and GoodsNum=" + ptype;
            //DataSet dsCon = new BCW.BLL.yg_BuyLists().GetList("COUNT(Counts)", strwhere);    
            if (!new BCW.BLL.GoodsList().Exists(ptype))
            {
                Utils.Error("不存在的商品", Utils.getUrl("kbyg.aspx"));
            }
            if (count > Convert.ToInt32(goods.GoodsValue))
            {
                Utils.Success("购买数量过大！", "购买不成功,请正确输入购买数量,2s后返回首页...", Utils.getUrl("kbyg.aspx"), "2");
            }
            if (shengyu < count)
            {
                try { isTimeToOpen(); }
                catch { }
                Utils.Success("库存不足", "库存不足,购买不成功,2s后返回首页...", Utils.getUrl("kbyg.aspx"), "2");
            }
            if (shengyu == 0)
            {
                OpenGoods();
                Utils.Error("购买不成功,商品正在开奖.", Utils.getUrl("kbyg.aspx"));
            }
            else
            {
                if (info == "")
                {
                    string goodsname = new BCW.BLL.GoodsList().GetGoodsName(Convert.ToInt64(ptype));
                    Master.Title = "购买结果";
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("确定购买该商品吗？" + "<br/>");
                    //////////
                    ///开启限制购买
                    /////////
                    /* *
                    if (goods.GoodsImg == "1" || goods.GoodsImg == "5" || goods.GoodsImg == "10" || goods.GoodsImg == "100")
                    {
                        string strwhere = "UserId=" + meid + "and GoodsNum=" + ptype;    
                        DataSet dsCon = new BCW.BLL.yg_BuyLists().GetList("SUM(Counts) AS counts", strwhere);
                        if (dsCon != null && dsCon.Tables[0].Rows.Count > 0)
                        {

                           // for (int i = 0; i < dsCon.Tables[0].Rows.Count; i++)//
                            {
                                if (Convert.ToInt32(dsCon.Tables[0].Rows[0]["counts"]) > 5)//
                                {
                                    Utils.Error("该商品每人最大允许购买5人次,留点机会给其他人吧！", Utils.getUrl("kbyg.aspx"));
                                }
                            }
                        }
                    }
                     * */
                    builder.Append("温馨提示:" + "<br/>" + " 你参与的商品：" + goodsname + "<br/>" + "云购标识：" + goods.Id + "<br/>" + "云购数量：" + count + "<br/>");
                    builder.Append("总花费:" + (count * Convert.ToInt32(goods.statue)) + "<br/>");
                    builder.Append("[开奖进度:" + (double)(goods.Number * 100 / goods.GoodsValue) + "%]" + "<br/>");
                    builder.Append("[参与后进度:" + (double)((goods.Number + count) * 100 / goods.GoodsValue) + "%]" + "<br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?info=ok&amp;act=result&amp;ptype=" + ptype + "&amp;count=" + count + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定购买</a><br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=edit&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧</a>");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
                else
                {
                    //是否刷屏
                    string appName = "LIGHT_KBYG";
                    int Expir = Utils.ParseInt(ub.GetSub("KbygExpir", xmlPath));
                    if (Expir > 0)
                    {
                        BCW.User.Users.IsFresh(appName, Expir);
                    }
                    if (!new BCW.BLL.GoodsList().Exists(ptype))
                    {
                        Utils.Error("不存在的商品", Utils.getUrl("kbyg.aspx"));
                    }
                    //支付安全提示
                    //string[] p_pageArr = { "ptype", "num", "count", "info", "act" };
                    //BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                    if (shengyu > count || shengyu == count)
                    {
                        ArrayList yuanlaide = new ArrayList();
                        if (goods.StockYungouma == "0" || goods.StockYungouma == "" || goods.StockYungouma == "NULL")
                        {
                            goods.StockYungouma = "";
                            for (int k = 1; k <= goods.GoodsValue; k++)//商品未有人购买
                            {
                                yuanlaide.Add(k);
                            }
                        }
                        else//剩余的字符串变成arr数组
                        {
                            string[] yun = goods.StockYungouma.Split(',');
                            for (int k = 0; k < yun.Length - 1; k++)
                            {
                                yuanlaide.Add(yun[k]);
                            }
                        }
                        Random ran = new Random();
                        int NowNumber = shengyu; ;
                        string yungouma = "";//生成的云购  
                        for (int i = 1; i <= count; )
                        {
                            int r = ran.Next(0, NowNumber);
                            if (isGet(yuanlaide[r].ToString(), yungouma))//不存在云购码执行
                            {
                                yungouma += yuanlaide[r].ToString();
                                yungouma += ",";
                                i++;
                                yuanlaide.Remove(yuanlaide[r]);
                                NowNumber--;
                            }
                        }
                        goods.StockYungouma = "";
                        for (int i = 0; i < yuanlaide.Count; i++)
                        {
                            goods.StockYungouma += yuanlaide[i];
                            goods.StockYungouma += ",";
                        }
                        if (goods.StockYungouma == "")
                        {
                            goods.StockYungouma = "0";
                        }
                        yuanlaide.Clear();
                        DateTime now = DateTime.Now;
                        model.UserId = (meid).ToString();
                        model.yungouma = yungouma;
                        model.GoodsNum = ptype;
                        model.Counts = count;
                        model.Ip = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                        // model.System = System.Environment.OSVersion.Version.ToString();
                        model.System = Request.Browser.Platform;
                        model.Address = "0";//标识
                        model.BuyTime = now;
                        model.IsGet = count * (goods.statue);//总花费酷币
                        long n = goods.Number + count;
                        goods.Number += count;//当前购买量
                        string mename = new BCW.BLL.User().GetUsName(meid);
                        long coast = -(Convert.ToInt32(model.IsGet));//花费为负
                        int id = Convert.ToInt32(model.UserId); //购买者id
                        if (ub.GetSub("KbygStatus", xmlPath) == "0")// //0正常1维护提示2测试提示
                        {
                            /*正常运行*/
                            //判断用户钱是否达到购买条件
                            string bzText = ub.Get("SiteBz");
                            long gold = new BCW.BLL.User().GetGold(meid);
                            if (model.IsGet > gold)
                            {
                                Utils.Error("你的" + bzText + "不足", Utils.getUrl("kbyg.aspx"));
                            }
                            else
                            {
                                try
                                {
                                    //string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/kbyg.aspx]" +"每日云购 "+"[/url]" +"第"+goods.periods+"期"+ goods.GoodsName+"消费了" + (-coast) + "酷币";  
                                    string wText = "在[url=/bbs/game/kbyg.aspx]" +  Gamename  + "[/url]" + "参与了第" + goods.periods + "期" + goods.GoodsName;
                                    //增加购买列
                                    int ID = new BCW.BLL.yg_BuyLists().Add(model);
                                    new BCW.BLL.User().UpdateiGold(meid, coast, "云购参与" + ptype + "#ID" + ID);
                                    //更新当前数量,剩余云购码UpdateNum(goods.Id, n);/UpdateYunGouMa(goods.Id, goods.StockYungouma); 
                                    new BCW.BLL.GoodsList().Update(goods);
                                    BCW.Model.GoodsList newGoodss = new BCW.BLL.GoodsList().GetGoodsList(ptype);
                                    if (newGoodss.GoodsValue == newGoodss.Number)
                                    {
                                        try
                                        {
                                            isTimeToOpen();
                                        }
                                        catch { }
                                    }
                                    new BCW.BLL.Action().Add(GameId, ID, meid, mename, wText);
                                    //   活跃抽奖入口_20160621姚志光
                                       try
                                    {
                                        //表中存在Gamename记录
                                        if (new BCW.BLL.tb_WinnersGame().ExistsGameName(Gamename))
                                        {
                                            //投注是否大于设定的限额，是则有抽奖机会
                                            if (model.IsGet > new BCW.BLL.tb_WinnersGame().GetPrice(Gamename))
                                            {
                                                string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                                                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                                                int hit = new BCW.winners.winners().CheckActionForAll(1, ID, meid, mename, Gamename+"云购", 3);
                                                if (hit == 1)
                                                {
                                                    //内线开关 1开
                                                    if (WinnersGuessOpen == "1")
                                                    {
                                                        //发内线到该ID
                                                        new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb + "云购");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                     catch { }
                                    Utils.Success("恭喜", "成功参与" + count + "人次" + "<br/>" + "正在返回..." + "", Utils.getUrl("kbyg.aspx"), "1");
                                }
                                catch (Exception ee)
                                {
                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("当前参与人数过多,购买失败,请重新" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">购买</a>.");
                                    builder.Append(Out.Tab("</div>", "<br/>"));
                                }
                            }
                        }
                        else if (ub.GetSub("KbygStatus", xmlPath) == "2")// //0正常1维护提示2测试
                        {
                            //测试运行
                            //扣费测试币 判断测试币是否允许购买
                            if (!new BCW.SWB.BLL().ExistsUserID(meid, GameId))
                            {
                                builder.Append(Out.Tab("<div>", ""));
                                builder.Append("您未领取测试云币哦,请先领取." + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=getgold&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">领取</a>");
                                builder.Append(Out.Tab("</div>", "<br/>"));
                            }
                            else
                            {
                                BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, GameId);
                                if (model.IsGet > swb.Money)
                                {
                                    Utils.Error("您的" + "云币" + "不足", Utils.getUrl("kbyg.aspx"));
                                }
                                else
                                {
                                    try
                                    {
                                        // swb.Money -=Convert.ToInt64(model.IsGet);
                                        // string cText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/kbyg.aspx]" + "每日云购 " + "[/url]" + "第" + goods.periods + "期" + goods.GoodsName + "测试消费了" + (-coast) + "云币";
                                        string cText = "在[url=/bbs/game/kbyg.aspx]" +  Gamename + "[/url]" + "参与了第" + goods.periods + "期" + goods.GoodsName;
                                        new BCW.SWB.BLL().UpdateMoney(meid, coast, GameId);
                                        //更新当前数量,剩余云购码  
                                        new BCW.BLL.GoodsList().Update(goods);
                                        //增加购买列
                                        int ID = new BCW.BLL.yg_BuyLists().Add(model);
                                        //动态
                                        new BCW.BLL.Action().Add(GameId, ID, meid, mename, cText);
                                        Utils.Success("恭喜", "成功参与" + count + "人次" + "<br/>" + "正在返回..." + "", Utils.getUrl("kbyg.aspx"), "1");
                                    }
                                    catch
                                    {
                                        builder.Append(Out.Tab("<div>", ""));
                                        builder.Append("购买失败,请重新" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">购买</a>.");
                                        builder.Append(Out.Tab("</div>", "<br/>"));
                                    }
                                }

                            }
                        }
                        // else { Utils.Safe("此游戏"); }
                    }
                }
            }
        }
        catch (Exception e)
        {
            // builder.Append(e);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("购买失败,请重新" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">购买</a>."); ;
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        //底部
        //string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //获取测试币
    protected void GetGameGold()
    {
        //底部
        //string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        Master.Title = "领取云币";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>&gt;");
        builder.Append("领取云币");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号    
        int ceshiTimeSpan = Convert.ToInt32(ub.GetSub("ceshiTimeSpan", xmlPath));
        long ceshiGet = Convert.ToInt64(ub.GetSub("ceshiGet", xmlPath));
        if (!new BCW.SWB.BLL().ExistsUserID(meid, GameId))//不存在用户记录直接领
        {
            //  BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
            BCW.SWB.Model model = new BCW.SWB.Model();
            model.UserID = meid;
            model.UpdateTime = DateTime.Now;
            model.Money = ceshiGet;
            model.GameID = GameId;
            model.Permission = 1;
            try
            {
                new BCW.SWB.BLL().Add(model);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("初次领取成功" + "<a href=\"" + Utils.getUrl("kbyg.aspx?") + "\">马上参与</a>.");
                builder.Append(Out.Tab("</div>", ""));
            }
            catch
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("领取失败,请重新" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">领取</a>.");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else//存在记录1时间到可领，2时间未到
        {
            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, GameId);
            DateTime now = DateTime.Now;
            DateTime dt = Convert.ToDateTime(swb.UpdateTime);
            TimeSpan span = DateTime.Now - dt;//现在距上次领取时间间隔
            if (span.Minutes > ceshiTimeSpan)//10min前  可领       
            {
                swb.UpdateTime = now;
                swb.Money += ceshiGet;
                swb.Permission += 1;
                // new BCW.SWB.BLL().UpdateMoney(meid,10000,31);
                try
                {
                    new BCW.SWB.BLL().Update(swb);
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("领取成功" + "<a href=\"" + Utils.getUrl("kbyg.aspx?") + "\">马上参与</a>.");
                    builder.Append(Out.Tab("</div>", ""));
                    //Utils.Success("恭喜", "成功领取" + "云币" + "<br/>" + "2s后返回首页.." + "", Utils.getUrl("kbyg.aspx"), "2");
                }
                catch { builder.Append("领取失败" + "<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页</a>."); }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                // builder.Append("领取云币失败,每" + ceshiTimeSpan + "分钟可领取一次." + "<br/>" + "请耐心等待时间刷新." + "<br/>");     
                builder.Append("每" + ceshiTimeSpan + "分钟可领取一次." + "<br/>" + "请耐心等待时间刷新." + "<br/>");
                DateTime nowtime = DateTime.Now;
                DateTime dtNow = swb.UpdateTime;
                TimeSpan spanNow = dt.AddMinutes(ceshiTimeSpan) - DateTime.Now;//现在距上次领取时间间隔
                if (spanNow.Minutes < ceshiTimeSpan)//领取时间间隔少于10min
                {
                    builder.Append("距下次领取还有");
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    {
                        TimeSpan ts = dtNow.AddMinutes(ceshiTimeSpan) - nowtime;
                        //if (ts.Hours == 0)
                        //{ builder.Append(ts.Minutes + ":" + ts.Seconds); }
                        if (ts.Minutes == 0)
                        { builder.Append(ts.Seconds); }
                        else
                        { builder.Append(ts.Minutes + ":" + ts.Seconds); }
                    }
                    else
                    {
                        string ss = swb.ID.ToString();
                        string time = new BCW.JS.somejs().newDaojishi(ss, dtNow.AddMinutes(ceshiTimeSpan));
                        builder.Append(time);
                    }
                }
                else//可领
                {
                    builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx?act=getgold&amp;") + "\">领取</a>");
                }
                builder.Append("<br/><a href=\"" + Utils.getUrl("kbyg.aspx") + "\">返回首页</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        // builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=buy&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回继续购买</a>.");
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + Gamename + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //云购码存在返回真，否则false
    protected bool isHave(int goodsnum, int yungouma)
    {
        return new BCW.BLL.yg_BuyLists().Getyg_BuyListsForYungouma(goodsnum, yungouma);
    }

    //云购码生成是否重复
    protected bool isGet(string r, string yungouma)
    {
        bool b = true;
        if (yungouma == "")
        { return b; }
        else
        {
            string[] yun = yungouma.Split(',');
            foreach (string j in yun)
            {
                //int temp = int.Parse(i);
                if (j.ToString() == r.ToString())
                {
                    b = false;
                }
            }
            return b;
        }

    }

    //开奖
    private void OpenGoods()
    {
        try
        {
            isTimeToOpen();//设置开奖状态,设置倒计时
            isTimeToOpenNew();//完全开奖，倒计时完毕，更改标识
        }
        catch (Exception ee)
        {
            string gessText = "云购开奖失败异常！";
            new BCW.BLL.Guest().Add(0, 727, "测试727", gessText + ee.ToString());//异常报错
        }
    }
    //倒计时完毕执行该函数
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
                        int meidsell = Convert.ToInt32(n.Identification);//出售者Id
                        DateTime now = DateTime.Now;
                        DateTime dt = Convert.ToDateTime(n.lotteryTime);
                        if (DateTime.Compare(now, dt) > 0)
                        {
                            n.isDone = 0;
                            n.StockYungouma = "已完结";
                            //  n.lotteryTime = now;
                            if (n.Identification > 0)//用户出售，盈利反馈给用户
                            {
                                int addGoodsValue = Convert.ToInt32(ub.GetSub("addGoodsValue", xmlPath));
                                string menamesell = new BCW.BLL.User().GetUsName(meidsell);//出售者Id
                                long gold = Convert.ToInt64(n.GoodsSell);//用户的押金
                                string bzText = ub.Get("SiteBz");
                                long UserSellGetPersan = Convert.ToInt64(ub.GetSub("UserSellGetPersan", xmlPath));//用户获得份数
                                long get = Convert.ToInt64(n.statue * UserSellGetPersan);
                                // long goldd=
                                long Usergold = new BCW.BLL.User().GetGold(meidsell);
                                if (ub.GetSub("KbygStatus", xmlPath) == "0")//酷币版
                                {
                                    new BCW.BLL.User().UpdateiGold(meidsell, get, "云购出售商品标识" + n.Id + "盈利");
                                    new BCW.BLL.User().UpdateiGold(meidsell, gold, "云购商品出售成功后退回押金标识" + n.Id);
                                    //发内线
                                    string str = "[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]发售的" + n.GoodsName + "编号" + n.Id + "售出后盈利" + get + bzText + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                                    try
                                    {
                                        new BCW.BLL.Guest().Add(0, meidsell, menamesell, str);
                                    }
                                    catch (Exception eee)
                                    {
                                        new BCW.BLL.Guest().Add(0, 727, "测试727", str + eee.ToString() + "发内线异常");
                                        //builder.Append(eee);
                                    }
                                    new BCW.BLL.Action().Add(GameId, Convert.ToInt32(n.Id), meidsell, menamesell, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]出售后盈利了**");// + get + bzText + "第" + n.periods + "期" + (n.GoodsName) + "的收入");
                                }
                                else if (ub.GetSub("KbygStatus", xmlPath) == "2")
                                {
                                    new BCW.SWB.BLL().UpdateMoney(meidsell, get, GameId);
                                    new BCW.SWB.BLL().UpdateMoney(meidsell, gold, GameId);
                                    //发内线
                                    string str = "[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]测试发售的" + n.GoodsName + "" + "成功售出并盈利了" + get + "云币" + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                                    new BCW.BLL.Guest().Add(0, meidsell, menamesell, str);
                                    new BCW.BLL.Action().Add(GameId, Convert.ToInt32(n.Id), meidsell, menamesell, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]盈利了**");// + get + bzText + "第" + n.periods + "期" + (n.GoodsName) + "的云币收入");
                                }
                            }
                            new BCW.BLL.GoodsList().Update(n);//更新获奖id，获奖码，到该商品
                            BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(n.Winner));
                            string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                            int meid = Convert.ToInt32(model.UserId);
                            if (ub.GetSub("KbygStatus", xmlPath) == "0")
                            {
                                //   new BCW.BLL.Action().Add(0, Convert.ToInt32(n.Id), meid, name, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]的" + "第" + n.periods + "期" + n.GoodsName+"幸运中奖");
                                //发内线
                                string sstrLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]购买的第" + n.periods + "期" + n.GoodsName + "" + "中奖了" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                                new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), name, sstrLog);
                            }
                            else if (ub.GetSub("KbygStatus", xmlPath) == "2")
                            {
                                new BCW.BLL.Action().Add(GameId, Convert.ToInt32(n.Id), meid, name, "在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]幸运中奖");// + "第" + n.periods + "期" + (n.GoodsName));
                                //发内线
                                string sstrLog = "您在[URL=/bbs/game/kbyg.aspx]" + Gamename + "[/URL]测试购买的第" + n.periods + "期" + n.GoodsName + "" + "中奖了" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                                new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.UserId), name, sstrLog);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", Gamename + "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "倒计时完毕开奖失败,请查看刷新机进行开奖)" + "错误码002");//向系统发内线
                        new BCW.BLL.Guest().Add(0, 727, "酷爆网客服", Gamename + "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "倒计时完毕开奖失败,请查看刷新机进行开奖)" + "错误码002" + ee.ToString());//向系统发内线
                        //  builder.Append(ee);
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
            // System.Console.WriteLine("传递过来的异常值为：{0}", e);
            //throw
            //  builder.Append(Out.Div("div", "设置倒计时失败！.."));
        }
        //try
        //{ isTimeToOpenNew(); }
        //catch { }

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
                                    sum = Convert.ToInt64(sum + (Convert.ToInt32(Convert.ToDateTime(ds.Tables[0].Rows[i]["BuyTime"]).ToString("HHmmssfff"))));
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
                                n.RemainingTime = dt;//
                                // n.lotteryTime = dt.AddMinutes;
                                //n.RemainingTime = dt.AddSeconds(Opentime*60);
                                int Opentime = Convert.ToInt32(ub.GetSub("timeToOpen", xmlPath));
                                int time = Opentime * 60;
                                n.lotteryTime = dt.AddSeconds(time);//倒计时后实际开奖时间
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
                            new BCW.BLL.Guest().Add(0, 10086, "酷爆网客服", Gamename + "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "开奖失败原因" + e + ",请查看刷新机进行开奖");//向系统发内线
                            new BCW.BLL.Guest().Add(0, 727, "727", Gamename + "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "开奖失败原因" + e + ",请查看刷新机进行开奖");//向系统发内线
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
    //时间段
    public string DateStringFromNow(DateTime dt)
    {
        TimeSpan span = DateTime.Now - dt;
        if (span.TotalDays > 60)
        {
            return dt.ToShortDateString();
        }
        else
            if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else
                if (span.TotalDays > 14)
                {
                    return "2周前";
                }
                else
                    if (span.TotalDays > 7)
                    {
                        return "1周前";
                    }
                    else
                        if (span.TotalDays > 1)
                        {
                            return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                        }
                        else
                            if (span.TotalHours > 1)
                            {
                                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                            }
                            else
                                if (span.TotalMinutes > 1)
                                {
                                    return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                }
                                else
                                    if (span.TotalSeconds >= 1)
                                    {
                                        return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                    }
                                    else
                                    {
                                        return "1秒前";
                                    }
    }
    /// <summary>
    /// 判断一个字符串是否为合法数字(指定整数位数和小数位数)
    /// </summary>
    /// <param name="s">字符串</param>
    /// <param name="precision">整数位数</param>
    /// <param name="scale">小数位数</param>
    /// <returns></returns>
    public static bool IsNumber(string s, int precision, int scale)
    {
        if ((precision == 0) && (scale == 0))
        {
            return false;
        }
        string pattern = @"(^\d{1," + precision + "}";
        if (scale > 0)
        {
            pattern += @"\.\d{0," + scale + "}$)|" + pattern;
        }
        pattern += "$)";
        return Regex.IsMatch(s, pattern);
    }
}