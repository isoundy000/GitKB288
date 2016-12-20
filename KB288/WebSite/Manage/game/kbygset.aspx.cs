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

public partial class Manage_game_kbygset: System.Web.UI.Page
{
    //开奖页面
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/myyg.xml";
    protected int GameId = 1007;
    protected void Page_Load(object sender, EventArgs e)
    {  
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            default:
                ReloadPage();
                break;
        }
        try
        {
            isTimeToOpen();//设置开奖状态,设置倒计时
            isTimeToOpenNew();//完全开奖，倒计时完毕，更改标识
        }
        catch {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("开奖异常.");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
      
    }
    private void ReloadPage()
    {
        Master.Title = "每日云购开奖页";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">每日云购开奖</a>&gt;");
        builder.Append("正在检测开奖...");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BuyLists()
    {
        Master.Title = "每日云购开奖页";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">每日云购管理</a>&gt;");
        builder.Append("购买记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (uid > 0)
            strWhere += "usid=" + uid + "";
        string[] pageValUrl = { "act", "uid", "backurl" };
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
                builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=del&amp;id=" + n.Id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                builder.Append("编号:" + n.Id + ",");
                builder.Append(new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId)) + "已于时间" + Convert.ToDateTime(n.BuyTime).ToString("yyyy-MM-dd hh:mm:ss fff") + "成功参与了" + n.Counts + "人次的" + "第" + n.GoodsNum + "期" + new BCW.BLL.GoodsList().GetGoodsName(n.GoodsNum));
                builder.Append("是否中奖:");
                if (n.Address == "1")
                { builder.Append("是" + "，" + "<a href=\"" + Utils.getUrl("kbyg.aspx?act=paijiang&amp;id=" + n.Id + "&amp;ptype=" + n.GoodsNum + ";backurl=" + Utils.PostPage(1) + "") + "\">" + "前往派奖..." + "</a>"); }
                else
                    if (n.Address == "0")
                    { builder.Append("否"); }
                    else
                        if (n.Address == "2")
                        { builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=myungou&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "是，已领奖！" + "</a>"); }
                // builder.Append(" <a href=\"" + Utils.getUrl("bbs/game/kbyg.aspx?act=yungoum&amp;ptype=" + n.GoodsNum + "") + "\">查看云购码...</a>");
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
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜云购记录,kbyg.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        Footer();
    }
    //管理底部
    private void Footer()
    {
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=unopen") + "\">查询未开奖商品</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=open") + "\">查询已开奖商品</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=add") + "\">添加云购商品</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=set&amp;backurl=" + Utils.PostPage(1) + "") + "\">云购配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx?act=reset") + "\">重置云购设置</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
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
                        int meidsell = Convert.ToInt32(n.Identification);//出售者Id
                        DateTime now = DateTime.Now;
                        DateTime dt = Convert.ToDateTime(n.RemainingTime);
                        if (DateTime.Compare(now, dt) > 0)
                        {
                            n.isDone = 0;
                            n.StockYungouma = "已完结";
                            if (n.Identification != 0)//用户出售，盈利反馈给用户
                            {
                                string menamesell = new BCW.BLL.User().GetUsName(meidsell);//出售者Id
                                long gold = Convert.ToInt64(n.statue * (n.GoodsValue - 10));
                                string bzText = ub.Get("SiteBz");
                                long UserSellGetPersan = Convert.ToInt64(ub.GetSub("UserSellGetPersan", xmlPath));//用户获得份数
                                long get = Convert.ToInt64(n.statue * UserSellGetPersan);
                                long Usergold = new BCW.BLL.User().GetGold(meidsell);
                                if (ub.GetSub("KbygStatus", xmlPath) == "0")
                                {
                                    new BCW.BLL.User().UpdateiGold(meidsell, get, "云购出售商品盈利");
                                    //发内线
                                    string str = "您在[URL=/bbs/game/kbyg.aspx]云购[/URL]发售的" + n.GoodsName + "" + "成功售出并盈利了" + get + bzText + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                                    new BCW.BLL.Guest().Add(GameId, meidsell, menamesell, str);
                                    new BCW.BLL.Action().Add(GameId, 0, meidsell, menamesell, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]获得了" + get + bzText + "第" + n.periods + "期" + (n.GoodsName) + "的收入");
                                }
                                else if (ub.GetSub("KbygStatus", xmlPath) == "2")
                                {
                                    new BCW.SWB.BLL().UpdateMoney(meidsell, get, 31);
                                    //发内线
                                    string str = "您在[URL=/bbs/game/kbyg.aspx]云购[/URL]测试发售的" + n.GoodsName + "" + "成功售出并盈利了" + get + "云币" + "[URL=/bbs/game/kbyg.aspx]去看看吧[/URL]";
                                    new BCW.BLL.Guest().Add(GameId, meidsell, menamesell, str);
                                    new BCW.BLL.Action().Add(GameId, 0, meidsell, menamesell, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]获得了" + get + bzText + "第" + n.periods + "期" + (n.GoodsName) + "的云币收入");
                                }
                            }
                            new BCW.BLL.GoodsList().Update(n);//更新获奖id，获奖码，到该商品
                            BCW.Model.yg_BuyLists model = new BCW.BLL.yg_BuyLists().Getyg_BuyLists(Convert.ToInt64(n.Winner));
                            string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
                            int meid = Convert.ToInt32(model.UserId);
                            if (ub.GetSub("KbygStatus", xmlPath) == "0")
                            {
                                new BCW.BLL.Action().Add(GameId, 0, meid, name, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]获奖了" + "第" + n.periods + "期" + (n.GoodsName));
                                //发内线
                                string sstrLog = "您在[URL=/bbs/game/kbyg.aspx]云购[/URL]购买的第" + n.periods + "期" + n.GoodsName + "" + "中奖了" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                                new BCW.BLL.Guest().Add(GameId, Convert.ToInt32(model.UserId), name, sstrLog);
                            }
                            else if (ub.GetSub("KbygStatus", xmlPath) == "2")
                            {
                                new BCW.BLL.Action().Add(GameId, 0, meid, name, "在[URL=/bbs/game/kbyg.aspx]云购[/URL]测试获奖了" + "第" + n.periods + "期" + (n.GoodsName));
                                //发内线
                                string sstrLog = "您在[URL=/bbs/game/kbyg.aspx]云购[/URL]测试购买的第" + n.periods + "期" + n.GoodsName + "" + "中奖了" + "[URL=/bbs/game/kbyg.aspx?act=geren]去看看吧[/URL]";
                                new BCW.BLL.Guest().Add(GameId, Convert.ToInt32(model.UserId), name, sstrLog);
                            }
                        }
                    }
                    catch
                    {
                        new BCW.BLL.Guest().Add(GameId, 10086, "酷爆网客服", "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "设置倒计时失败,请查看刷新机进行开奖)" + "错误码002");//向系统发内线
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
                                n.OverTime = dt;
                                n.lotteryTime = dt;
                                //n.RemainingTime = dt.AddSeconds(Opentime*60);
                                int Opentime = Convert.ToInt32(ub.GetSub("timeToOpen", xmlPath));
                                int time = Opentime * 60;
                                n.RemainingTime = dt.AddSeconds(time);
                                new BCW.BLL.GoodsList().Update(n);//更新获奖id，获奖码，到该商品
                                mod.Address = "1";//=win 成功获奖标识
                                new BCW.BLL.yg_BuyLists().UpdateAddress(mod.Id, mod.Address);//更新BuyLists 
                                if ((n.GoodsType % 2) == 0)//类型为偶数时自动生成新一期,偶数循环
                                {
                                    n.RemainingTime = DateTime.Now;
                                    n.Winner = "0";
                                    n.isDone = 1;
                                    n.periods += 1;
                                    n.Number = 0;
                                    n.lotteryTime = DateTime.Now;
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
                                builder.Append(Out.Tab("<div>", ""));
                                builder.Append("商品列" +n.Id+ n.GoodsName+"开奖成功!"+"时间:"+Convert.ToDateTime(n.lotteryTime).ToString("yyyy-MM-dd hh:mm:ss fff")+"<br/>");
                                builder.Append("正常检测中...");
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                builder.Append("商品列"+"开奖有误!");
                              
                            }
                        }
                        catch (Exception e)
                        {
                            new BCW.BLL.Guest().Add(GameId, 10086, "酷爆网客服", "商品Id" + n.Id + "名称" + n.GoodsName + "第" + n.periods + "期" + "开奖失败原因" + e + ",请查看刷新机进行开奖");//向系统发内线
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
            new BCW.BLL.Guest().Add(1, 10086, "酷爆网客服", "开奖异常:"+e+ "错误码003");//向 系统发内线
           // System.Console.WriteLine("传递过来的异常值为：{0}", e);
           // builder.Append(Out.Div("div", "开奖异常.."));
        }

    }
}