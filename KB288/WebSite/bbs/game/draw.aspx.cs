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
using BCW.farm;
using System.Threading;

/// <summary>
/// 蒙宗将 2016/5/9
/// 蒙宗将 2016/5/10 去掉道具价值的添加与显示
/// 蒙宗将 2016/5/12 增加奖品库存设置
/// 蒙宗将 2016/5/14 兑换道具管理 中奖算法修改
/// 蒙宗将 2016/5/16 完善换币功能、增加球彩下注点值生成 、增加农场链接
/// 蒙宗将 2016/5/20 简化兑奖功能 图片缩放完善
/// 蒙宗将 2016/6/1  换币改为换点值，修改换点值操作
/// 蒙宗将 2016/8/26 全面升级
/// 蒙宗将 2016/9/10  增加聊吧发言，游戏名称管理修复统一
/// 蒙宗将 2016/09/18 奖品显示（list修改）
/// mzj  20160919 
/// /// 蒙宗将 2016/9/21  抽奖与兑换分开
/// 蒙宗将 2016/9/29 前台增加几等奖显示
/// 蒙宗将 2016/11/12 农场道具增加参数 11/14
/// 蒙宗将 2016/11/15 增加显示快递信息
/// </summary>

public partial class bbs_game_draw : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/draw.xml";
    protected string xmlPath2 = "/Controls/gamezdid.xml";
    protected string GameName = Convert.ToString(ub.GetSub("drawName", "/Controls/draw.xml"));
    protected int chance = Convert.ToInt32(ub.GetSub("chance", "/Controls/draw.xml"));//概率
    protected int jifen = Convert.ToInt32(ub.GetSub("jifen", "/Controls/draw.xml"));//积分兑换的大小
    protected string drawJifen = Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml"));//兑换的积分名字
    protected long farm = Convert.ToInt64(ub.GetSub("farm", "/Controls/draw.xml"));//农场金币换币值
    protected string ceshi = ub.GetSub("ceshi", "/Controls/draw.xml");//测试状态
    protected string ceshiID = ub.GetSub("CeshiQualification", "/Controls/draw.xml");
    protected int chuzhi = Convert.ToInt32(ub.GetSub("chuzhi", "/Controls/draw.xml"));
    protected int JJ = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    string[] f = new String[] { "幸运幸福来敲门，快快试试好运气", "常把笑容挂脸上，常把大奖带回家", "快来试试运气，大奖等你来拿", "天降大奖，还等什么，快快行动起来", "好运当头彩，抽奖咱就来" };
    string[] f1 = new String[] { "哎呀，运气不怎么好哟，什么也没抽到...下次加油", "欧欧，什么也拿不到，再试试看", "笑一笑啊，换个姿势试试", "抽奖抽到手抽筋，下一个肯定是大奖", "就差一点点了，再试一试，一定能中奖" };

    protected void Page_Load(object sender, EventArgs e)
    {
        //int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();

        //内测判断 0,内测是否开启1，是否为内测账号
        if (ceshi == "1")//内测
        {
            string[] sNum = Regex.Split(ceshiID, "#");
            int sbsy = 0;
            for (int a = 0; a < sNum.Length; a++)
            {
                if (new BCW.User.Users().GetUsId() == int.Parse(sNum[a].Trim()))
                {
                    sbsy++;
                }
            }
            if (sbsy == 0)
            {
                Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
            }
        }
        //维护提示
        if (ub.GetSub("drawStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":
                RulePage(); //规则
                break;
            case "case":
                CasePage(); //兑奖
                break;
            case "paihang":
                PaiHangPage();//排行
                break;
            case "jiangpin":
                JiangPinPage();//奖品
                break;
            case "lishi":
                LishiPage();//历史
                break;
            case "neixian":
                NeixianPage();//提醒管理员添加奖品
                break;
            case "choujiang":
                ChouJiangPage();//积分抽奖
                break;
            case "realcj":
                RealCJPage();
                break;
            case "dui":
                DuiPage();//其他币种兑换为积分
                break;
            case "duiq":
                DuiqPage();//其他币种全部兑换为积分
                break;
            case "duis":
                DuisPage();//其他币种输入兑换为积分
                break;
            case "trun":
                TrunPage();//把其他币换成抽奖的积分
                break;
            case "tru":
                TruPage();//把其他币换成抽奖的积分
                break;
            case "conlist":
                ConListPage();//积分消费记录
                break;
            case "duihuan":
                DuiHuanPage();//积分兑换首页
                break;
            case "isduihuan":
                IsDuiHuanPage();//积分兑换面页
                break;
            case "willduihuan":
                WillDuihuanPage();
                break;
            case "goduihuan":
                GoDuiHuanPage();//确认兑换面页
                break;
            case "myjiangpin":
                MyJiangpinPage();//我的奖品
                break;
            case "myrealjiangpin":
                MyRealJiangpinPage();//我的奖品具体信息
                break;
            case "myrealjiangpin1":
                MyRealJiangpin1Page();//我的奖品具体信息1
                break;
            case "message":
                MessagePage();//确认个人信息
                break;
            case "kefu":
                KefuPage();//联系客服
                break;
            case "trends":
                TrendsPage();//动态
                break;
            case "xinxi":
                XinxiPage();//动态奖品信息
                break;
            case "qd":
                QdPage();//签到
                break;
            case "qdp":
                QdpPage();//签到判断
                break;
            case "qdjl":
                QdjlPage();//签到奖励说明
                break;
            case "qdtop":
                QdtopPage();//签到排行榜
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //主页
    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        //游戏头部Ubb
        string Head = ub.GetSub("drawHead", xmlPath);
        if (Head != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Head)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //口号
        string Notes = ub.GetSub("drawNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        string Logo = ub.GetSub("drawLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" height=\"100\" weight=\"60\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));//<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id="+meid+"") + "\">兑奖</a>
        builder.Append("【" + GameName + "】<a href=\"" + Utils.getUrl("draw.aspx?act=rule") + "\">规则</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=trun") + "\">农场</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品一览</a>.<a href=\"" + Utils.getUrl("draw.aspx?act=qd") + "\">每日签到</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paihang") + "\">游戏排行</a>.<a href=\"" + Utils.getUrl("draw.aspx?act=lishi") + "\">" + GameName + "历史</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin") + "\">我的奖品</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=conlist") + "\">" + drawJifen + "记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=choujiang") + "\">" + Jifen + "抽奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Jifen + "兑换</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        int xmllun = Convert.ToInt32(ub.GetSub("lun", xmlPath));
        int lun = 0;
        try { lun = xmllun; }
        catch { lun = 0; }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【本轮抽奖奖池最新动态】");
        builder.Append(Out.Tab("</div>", "<br />"));
        int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
        string str = string.Empty;
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<table>");
        for (int i = 0; i < j; i++)
        {
            string sssss = string.Empty;
            if (i < 20)
            {
                if (i == 0) { sssss = "一等奖"; }
                if (i == 1) { sssss = "二等奖"; }
                if (i == 2) { sssss = "三等奖"; }
                if (i == 3) { sssss = "四等奖"; }
                if (i == 4) { sssss = "五等奖"; }
                if (i == 5) { sssss = "六等奖"; }
                if (i == 6) { sssss = "七等奖"; }
                if (i == 7) { sssss = "八等奖"; }
                if (i == 8) { sssss = "九等奖"; }
                if (i == 9) { sssss = "十等奖"; }
                if (i == 10) { sssss = "十一等奖"; }
                if (i == 11) { sssss = "十二等奖"; }
                if (i == 12) { sssss = "十三等奖"; }
                if (i == 13) { sssss = "十四等奖"; }
                if (i == 14) { sssss = "十五等奖"; }
                if (i == 15) { sssss = "十六等奖"; }
                if (i == 16) { sssss = "十七等奖"; }
                if (i == 17) { sssss = "十八等奖"; }
                if (i == 18) { sssss = "十九等奖"; }
                if (i == 19) { sssss = "二十等奖"; }
                str = sssss;
            }
            else
            {
                str = (i + 1) + "等奖";
            }
            string jiang = string.Empty;
            int jiangnum = 0;
            jiangnum = new BCW.Draw.BLL.DrawBox().GetGoodsNum(new BCW.Draw.BLL.DrawTen().GetCounts(i));
            if (new BCW.Draw.BLL.DrawTen().GetCounts(i) == 0)
            {
                jiang = "奖品下架";
            }
            else
            {
                jiang = "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + new BCW.Draw.BLL.DrawTen().GetCounts(i) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.Draw.BLL.DrawBox().GetGoodsName(new BCW.Draw.BLL.DrawTen().GetCounts(i)) + "</a>";
            }

            builder.Append("<tr><td>" + str + ":</td><td>" + jiang + "</td><td>|剩余:" + jiangnum + "</td></tr>");
        }

        try
        {
            builder.Append("<tr><td>总奖池数:</td><td>" + new BCW.Draw.BLL.DrawBox().GetAllNum(lun) + "</td><td>|剩余:" + new BCW.Draw.BLL.DrawBox().GetAllNumS(lun) + "</td></tr>");
        }
        catch
        {

        }


        builder.Append("</table>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        //5条实时动态
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【" + GameName + "动态】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int rm = new BCW.Draw.BLL.DrawUser().GetMaxId();
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        BCW.Draw.Model.DrawUser model5 = new BCW.Draw.BLL.DrawUser().GetDrawUser(rm - 1);

        try
        {
            DataSet ds = new BCW.Draw.BLL.DrawUser().GetList("top 3 *", "MyGoodsStatue!='88' order by id desc");
            int p = 0;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    p = int.Parse(ds.Tables[0].Rows[i]["MyGoodsStatue"].ToString());
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    string MyGoods = ds.Tables[0].Rows[i]["MyGoods"].ToString();
                    string OnTime = ds.Tables[0].Rows[i]["OnTime"].ToString();
                    int GoodsCounts = int.Parse(ds.Tables[0].Rows[i]["GoodsCounts"].ToString());
                    int R = int.Parse(ds.Tables[0].Rows[i]["R"].ToString());
                    int Num = int.Parse(ds.Tables[0].Rows[i]["Num"].ToString());
                    TimeSpan time = DateTime.Now - Convert.ToDateTime(OnTime);
                    int rrank = 0;
                    try
                    {
                        rrank = new BCW.Draw.BLL.DrawBox().GetRank(GoodsCounts);
                    }
                    catch { }

                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;

                    if (R != 888)
                    {
                        if (d1 == 0)
                        {
                            if (d == 0 && e == 0 && f == 0)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                            }
                            else if (d == 0 && e == 0)
                                builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                            else if (d == 0)
                                builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                            else
                                builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                        }
                        else
                            builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                    }
                    else
                    {
                        if (d1 == 0)
                        {
                            if (d == 0 && e == 0 && f == 0)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                            }
                            else if (d == 0 && e == 0)
                                builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                            else if (d == 0)
                                builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                            else
                                builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                        }
                        else
                            builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
                    }
                }
            }
            else
            {
                builder.Append("没有更多数据...<br />");
            }
        }
        catch
        {
            builder.Append("没有数据");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=trends&amp;backurl=" + Utils.PostPage(1) + "") + "\">>>更多动态</a>");
        builder.Append(Out.Tab("</div>", ""));

        //游戏底部Ubb
        string Foot = ub.GetSub("drawFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(37, "draw.aspx", 5, 0)));
        builder.Append(Out.Tab("</div>", ""));

        #region 判断用户是否进入过抽奖
        if (new BCW.Draw.BLL.DrawJifen().ExistsUsID(meid))//判断用户是否存在，若不存在，自动添加用户
        {

        }
        else
        {
            BCW.Draw.Model.DrawJifen addjifen = new BCW.Draw.Model.DrawJifen();
            addjifen.UsID = meid;
            addjifen.Jifen = 0;
            addjifen.Qd = 0;
            addjifen.Qdweek = 0;
            addjifen.QdTime = DateTime.Now.AddDays(-1);
            new BCW.Draw.BLL.DrawJifen().Add(addjifen);

            //发动态
            string name = new BCW.BLL.User().GetUsName(meid);
            int id = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            string wText = "首次进入[url=/bbs/game/Draw.aspx]" + GameName + "[/url]，获得" + chuzhi + drawJifen + "";
            new BCW.BLL.Action().Add(1013, id, meid, name, wText);

            //发内线
            string strLog = "首次进入" + GameName + "，获得" + chuzhi + drawJifen + "" + "[url=/bbs/game/draw.aspx]进入" + GameName + "[/url]";
            new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);

            new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, chuzhi, "" + GameName + "首次进入游戏获得");
        }
        #endregion

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //规则
    private void RulePage()
    {
        string Rule = ub.GetSub("drawRule", xmlPath);
        string Xiangqing = ub.GetSub("drawXiangqing", xmlPath);
        string shuoming = ub.GetSub("drawshuoming", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=" + GameName + "规则=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(Rule));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=活动详情=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(Xiangqing));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=兑奖说明=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(shuoming));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //排行记录
    private void CasePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "1"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=paihang&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行</a>&gt;排行记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        //builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        int iSCounts = 0;

        if (ptypec == 1)
        {
            strWhere = "UsID=" + id + " and MyGoodsStatue!=88 and R!=888";
        }
        if (ptypec == 2)
        {
            strWhere = "UsID=" + id + " and MyGoodsStatue!=88 and R='888'";
        }

        strOrder = "OnTime desc ";
        string[] pageValUrl = { "act", "ptypec", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listdrawuser = new BCW.Draw.BLL.DrawUser().GetDrawUsers1(pageIndex, 10, strWhere, strOrder, iSCounts, out recordCount);

        if (listdrawuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listdrawuser)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                if (ptypec == 1)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + id + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "抽中奖品:" + n.MyGoods + "";
                }
                if (ptypec == 2)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + id + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "" + drawJifen + "兑换奖品:" + n.MyGoods + "";
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("Draw.aspx?act=case&amp;id=" + id + "") + "\"></a>" + d + sText);
                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paihang&amp;ptypec=" + ptypec + "") + "\">返回排行</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //排行
    private void PaiHangPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div", ""));
        builder.Append("【排行榜】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "1"));
        if (ptypec == 1)
            builder.Append("<b style=\"color:black\">抽奖" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=paihang&amp;ptypec=1&amp;id1=" + ptypec + "") + "\">抽奖</a>" + "|");
        if (ptypec == 2)
            builder.Append("<b style=\"color:black\">兑换" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=paihang&amp;ptypec=2&amp;id1=" + ptypec + "") + "\">兑换</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount = 50;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string _where = "";
        string strWhere = "";
        string[] pageValUrl = { "act", "ptypec", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);

        if (ptypec == 1)
        {
            _where = "and R!=888";
            strWhere = "MyGoodsStatue!=88 and R!=888";
        }
        if (ptypec == 2)
        {
            _where = "and R='888'";
            strWhere = "MyGoodsStatue!=88 and R='888'";
        }

        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listUserTop = new BCW.Draw.BLL.DrawUser().GetUserTop(pageIndex, pageSize, _where, strWhere, out recordCount);
        if (listUserTop.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listUserTop)
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
                if (ptypec == 1)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>中奖" + "<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id=" + n.UsID + "&amp;ptypec=" + ptypec + "") + "\">" + n.aa + "</a>次");
                    builder.Append(".<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id=" + n.UsID + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>");
                }
                if (ptypec == 2)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>" + drawJifen + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id=" + n.UsID + "&amp;ptypec=" + ptypec + "") + "\">" + n.aa + "</a>次");
                    builder.Append(".<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id=" + n.UsID + "&amp;ptypec=" + ptypec + "") + "\">查看</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //签到
    private void QdPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Rule = ub.GetSub("drawRule", xmlPath);
        string Xiangqing = ub.GetSub("drawXiangqing", xmlPath);
        string shuoming = ub.GetSub("drawshuoming", xmlPath);

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;签到");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=签到说明=<br />");
        builder.Append("坚持签到获得抽奖" + drawJifen + "，坚持连续每天签到有更大收获");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        DateTime QdTime = DateTime.Now;
        QdTime = new BCW.Draw.BLL.DrawJifen().getQdTime(meid);
        if (QdTime.Day == DateTime.Now.Day && QdTime.Year == DateTime.Now.Year && QdTime.Month == DateTime.Now.Month)
        {
            builder.Append("您今天已经签到过了");
        }
        else
        {
            builder.Append("您今天未签到&gt;&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=qdp") + "\">签到</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=qdjl") + "\">我的签到奖励</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=qdtop") + "\">签到排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //签到判断
    private void QdpPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;签到");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int Qdstate = 0; int Qdweekget = 0; DateTime QdTime = DateTime.Now;
        Qdstate = new BCW.Draw.BLL.DrawJifen().getsQd(meid);
        Qdweekget = new BCW.Draw.BLL.DrawJifen().getsQdweek(meid);
        QdTime = new BCW.Draw.BLL.DrawJifen().getQdTime(meid);
        //识别会员vip
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;
        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime("2000-01-01 01:01:01");
        }
        int Qd = Convert.ToInt32(ub.GetSub("Qd", xmlPath));
        int Qdvip = Convert.ToInt32(ub.GetSub("Qdvip", xmlPath));
        int Qdweek = Convert.ToInt32(ub.GetSub("Qdweek", xmlPath));
        int Qdkg = Convert.ToInt32(ub.GetSub("Qdkg", xmlPath));
        int Qdvipkg = Convert.ToInt32(ub.GetSub("Qdvipkg", xmlPath));

        if (QdTime.Day == DateTime.Now.Day && QdTime.Year == DateTime.Now.Year && QdTime.Month == DateTime.Now.Month)
        {
            builder.Append("您今天已经签到了");
        }
        else
        {
            if (DateTime.Now < viptime)//VIP会员
            {
                if (QdTime > DateTime.Parse(DateTime.Now.ToLongDateString()).AddDays(-1))//上次签到为昨天
                {
                    if (Qdweekget < Qdweek)
                    {
                        new BCW.Draw.BLL.DrawJifen().UpdateQdweek(meid, (Qdweekget + 1));
                        new BCW.Draw.BLL.DrawJifen().UpdateQd(meid, (Qdstate + 1));
                        new BCW.Draw.BLL.DrawJifen().UpdateQdTime(meid, DateTime.Now);
                        new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), (Qdvip * (Qdweekget + 1)), "连续签到" + (Qdweekget + 1) + "天奖励");//XXX得到抽奖值
                        Utils.Success("签到", "连续签到" + (Qdweekget + 1) + "天，奖励" + (Qdvip * (Qdweekget + 1)) + drawJifen + "", Utils.getUrl("draw.aspx?act=qd"), "5");
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawJifen().UpdateQdweek(meid, 1);
                        new BCW.Draw.BLL.DrawJifen().UpdateQd(meid, (Qdstate + 1));
                        new BCW.Draw.BLL.DrawJifen().UpdateQdTime(meid, DateTime.Now);
                        new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), (Qdvip * 1), "连续签到" + 1 + "天奖励");//XXX得到抽奖值
                        Utils.Success("签到", "连续签到1天，奖励" + (Qdvip * 1) + drawJifen + "", Utils.getUrl("draw.aspx?act=qd"), "5");
                    }
                }
                else
                {
                    new BCW.Draw.BLL.DrawJifen().UpdateQdweek(meid, 1);
                    new BCW.Draw.BLL.DrawJifen().UpdateQd(meid, (Qdstate + 1));
                    new BCW.Draw.BLL.DrawJifen().UpdateQdTime(meid, DateTime.Now);
                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), (Qdvip * 1), "连续签到" + 1 + "天奖励");//XXX得到抽奖值
                    Utils.Success("签到", "连续签到1天，奖励" + (Qdvip * 1) + drawJifen + "", Utils.getUrl("draw.aspx?act=qd"), "5");
                }

            }
            else//普通会员
            {
                if (QdTime > DateTime.Parse(DateTime.Now.ToLongDateString()).AddDays(-1))//上次签到为昨天
                {
                    if (Qdweekget < Qdweek)
                    {
                        new BCW.Draw.BLL.DrawJifen().UpdateQdweek(meid, (Qdweekget + 1));
                        new BCW.Draw.BLL.DrawJifen().UpdateQd(meid, (Qdstate + 1));
                        new BCW.Draw.BLL.DrawJifen().UpdateQdTime(meid, DateTime.Now);
                        new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), (Qd * (Qdweekget + 1)), "连续签到" + (Qdweekget + 1) + "天奖励");//XXX得到抽奖值
                        Utils.Success("签到", "连续签到" + (Qdweekget + 1) + "天，奖励" + (Qd * (Qdweekget + 1)) + drawJifen + "", Utils.getUrl("draw.aspx?act=qd"), "5");
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawJifen().UpdateQdweek(meid, 1);
                        new BCW.Draw.BLL.DrawJifen().UpdateQd(meid, (Qdstate + 1));
                        new BCW.Draw.BLL.DrawJifen().UpdateQdTime(meid, DateTime.Now);
                        new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), (Qd * 1), "连续签到" + 1 + "天奖励");//XXX得到抽奖值
                        Utils.Success("签到", "连续签到1天，奖励" + (Qd * 1) + drawJifen + "", Utils.getUrl("draw.aspx?act=qd"), "5");
                    }
                }
                else
                {
                    new BCW.Draw.BLL.DrawJifen().UpdateQdweek(meid, 1);
                    new BCW.Draw.BLL.DrawJifen().UpdateQd(meid, (Qdstate + 1));
                    new BCW.Draw.BLL.DrawJifen().UpdateQdTime(meid, DateTime.Now);
                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), (Qd * 1), "连续签到" + 1 + "天奖励");//XXX得到抽奖值
                    Utils.Success("签到", "连续签到1天，奖励" + (Qd * 1) + drawJifen + "", Utils.getUrl("draw.aspx?act=qd"), "5");
                }
            }
        }

        builder.Append(Out.Tab("</div>", ""));

    }
    //我的签到奖励说明
    private void QdjlPage()
    {
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=qd") + "\">签到</a>" + "&gt;我的签到奖励说明");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int Qdweek = new BCW.Draw.BLL.DrawJifen().getsQdweek(meid);
        int Qd = new BCW.Draw.BLL.DrawJifen().getsQd(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您已经连续签到" + Qdweek + "天,累计签到" + Qd + "次.再接再厉哦.<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("我的签到奖励：<br/>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);

        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsID=" + meid + " and Types=1";//签到types=1
        IList<BCW.Draw.Model.DrawJifenlog> listJf = new BCW.Draw.BLL.DrawJifenlog().GetJifenlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listJf.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawJifenlog n in listJf)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.AcText) + n.AcGold + drawJifen + "(" + (n.AddTime) + ")");
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

        //识别会员vip
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;
        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime("2000-01-01 01:01:01");
        }
        int Qda = Convert.ToInt32(ub.GetSub("Qd", xmlPath));
        int Qdvip = Convert.ToInt32(ub.GetSub("Qdvip", xmlPath));
        int Qdweeks = Convert.ToInt32(ub.GetSub("Qdweek", xmlPath));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("签到说明：<br/>");
        builder.Append(Out.Tab("</div>", ""));
        if (DateTime.Now < viptime)//VIP会员
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("1.每天签到，获得" + drawJifen + "奖励.<br/>");
            builder.Append("2.连续" + Qdweeks + "签到，根据连续签到天数，获得N*" + Qdvip + drawJifen + "奖励.(N连续签到的天数)");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("1.每天签到，获得" + drawJifen + "奖励.<br/>");
            builder.Append("2.连续" + Qdweeks + "签到，根据连续签到天数，获得N*" + Qda + drawJifen + "奖励.(N连续签到的天数)");
            builder.Append(Out.Tab("</div>", ""));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=qdtop") + "\">>签到排行榜&lt;</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    //签到排行榜
    private void QdtopPage()
    {
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=qd") + "\">签到</a>&gt;签到排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("==签到排行==");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;

        string strWhere = " ";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = 10;
        string strOrder = "";
        strWhere = "Qd>0";
        strOrder = "Qd Desc";
        // 开始读取列表
        IList<BCW.Draw.Model.DrawJifen> listJf = new BCW.Draw.BLL.DrawJifen().GetDrawJifens(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listJf.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawJifen n in listJf)
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
                string OutText = string.Empty;
                OutText = "(" + n.Qd + "次)";
                string mename = new BCW.BLL.User().GetUsName(n.UsID);//用户姓名
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "" + OutText + "</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=qd") + "\">返回签到</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    //奖品
    private void JiangPinPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;奖品");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>精美奖品，等你来拿：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("<b style=\"color:red\">抽奖奖品</b>|<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;ptype=1") + "\">兑换奖品</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;ptype=0") + "\">抽奖奖品</a>|<b style=\"color:red\">兑换奖品</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        try
        {
            //商品最大Id
            int newGoods = new BCW.Draw.BLL.DrawBox().GetMaxId();
            // 路径
            string Logo = ub.GetSub("img", xmlPath);
            // 显示最大图片量
            string imgCount = ub.GetSub("imgCount", xmlPath);
            DataSet ds = null;
            if (ptype == 0)
                ds = new BCW.Draw.BLL.DrawBox().GetList("ID", " Statue=0 and Points=0 Order by Rank Asc");
            else
                ds = new BCW.Draw.BLL.DrawBox().GetList("ID", " Statue=0 and Points!=0  Order by GoodsCounts Asc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int lists = 0; lists < ds.Tables[0].Rows.Count; lists++)
                {
                    int uu = int.Parse(ds.Tables[0].Rows[lists][0].ToString());
                    DateTime nowTime = DateTime.Now;
                    BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(uu);
                    string type = "";
                    if (model.GoodsType == 0) { type = "酷币"; }
                    if (model.GoodsType == 1) { type = "实物"; }
                    if (model.GoodsType == 2) { type = "道具"; }
                    if (model.GoodsType == 3) { type = "属性"; }

                    //是否为进行中商品   
                    if (model.Statue == 0)
                    {
                        builder.Append((lists + 1) + ".");
                        builder.Append("奖品编号：" + model.GoodsCounts + "<br />");
                        if (model.GoodsType == 0)
                        {
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a><br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;酷币价值：" + model.GoodsValue + " " + "<br />");
                        }
                        if (model.GoodsType == 1)
                        {
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a><br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                            //builder.Append("&nbsp;&nbsp;&nbsp;奖品价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                        }
                        if (model.GoodsType == 2)
                        {
                            string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a><br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "(" + GN + ")" + "<br />");
                            //builder.Append("&nbsp;&nbsp;&nbsp;道具价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                        }
                        if (model.GoodsType == 3)
                        {
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>" + "+" + model.GoodsValue + " <br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                        }
                        if (model.points == 0)
                            builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + "：" + "不可兑换" + "<br />");
                        else
                            builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + "：" + model.points + "<br />");
                        builder.Append("&nbsp;&nbsp;&nbsp;奖品余量：" + model.GoodsNum + "");
                        // lists++;
                        if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                        {
                            builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述：");
                            builder.Append("<br/>");
                            string[] imgNum = model.GoodsImg.Split(',');
                            for (int c = 0; c < imgNum.Length - 1; c++)
                            {
                                if (model.GoodsType == 2)
                                {
                                    builder.Append("&nbsp;&nbsp;&nbsp;" + "<img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/>" + "&nbsp;&nbsp;");
                                }
                                else
                                {
                                    if (id != 0)
                                    {
                                        if (id == model.GoodsCounts)
                                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + 0 + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                        else
                                            builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                    }
                                    else
                                    {
                                        builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                    }
                                }
                            }
                        }
                        builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");
                        if (model.rank == 1000)
                        {
                            builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Jifen + "兑换</a><br/>");
                        }
                        else
                        {
                            builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=choujiang") + "\">" + Jifen + "抽奖</a><br />");
                        }

                        //builder.Append(Out.Tab("<div>", ""));
                        builder.Append("" + "-----------" + "<br />");
                        //builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂时没有更多奖品...");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=choujiang") + "\">前去" + Jifen + "抽奖</a>");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //抽奖历史
    private void LishiPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;" + GameName + "历史");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>抽奖历史</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = " MyGoodsStatue!='88'";
        strOrder = "Num desc";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        IList<BCW.Draw.Model.DrawUser> GetDrawUsers1 = new BCW.Draw.BLL.DrawUser().GetDrawUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (GetDrawUsers1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser model1 in GetDrawUsers1)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                int rrank = 0;
                try
                {
                    rrank = new BCW.Draw.BLL.DrawBox().GetRank(model1.GoodsCounts);
                }
                catch { }
                if (model1.R != 888)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + model1.GoodsCounts + "&amp;num=" + model1.Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + model1.MyGoods + "</a>" + "|" + Convert.ToDateTime(model1.OnTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + model1.GoodsCounts + "&amp;num=" + model1.Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + model1.MyGoods + "</a>" + "|" + Convert.ToDateTime(model1.OnTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("draw.aspx?act=trends&amp;ptype=" + ptype + "&amp;id=" + model1.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //兑换为抽奖积分
    private void TrunPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;换" + drawJifen + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        long gold = Utils.ParseInt64(Utils.GetRequest("gold", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div>", ""));
        long gjifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
        builder.Append("拥有" + drawJifen + "：" + gjifen);
        builder.Append(Out.Tab("</div>", "<br />"));

        string game1 = string.Empty;
        string gname1 = string.Empty;

        long gold1 = 0;
        game1 = "开心农场";
        gname1 = "金币";

        gold1 = new BCW.farm.BLL.NC_user().GetGold(meid);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + game1 + "</a>" + "：" + gold1 + gname1 + " ");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?info=1&amp;act=trun&amp;ptype=" + 0 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">换" + drawJifen + "</a>" + "");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                Utils.Success("换币", "银行经理正在计算中，请稍等", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "1");
            }
            else
            {
                Utils.Success("换币", "银行经理正在计算中，请稍等", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "0");
            }
        }
        else if (info == "2")
        {
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                Utils.Success("换币", "银行经理正在计算中，请稍等", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "1");
            }
            else
            {
                Utils.Success("换币", "银行经理正在计算中，请稍等", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "0");
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //判断传值修改问题
    private void TruPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[0-9]$", "0"));
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        xml.dss["ptype"] = ptype;
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("换币", "银行经理正在计算中，请稍等", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "1");
        }
        else
        {
            Utils.Success("换币", "银行经理正在计算中，请稍等", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "0");
        }
    }
    //其他币种兑换为积分
    private void DuiPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[0-1]$", "请勿做非法操作"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=trun") + "\">" + drawJifen + "</a>&gt;换" + drawJifen + "确认");
        builder.Append(Out.Tab("</div>", "<br />"));

        string game = string.Empty;
        string gname = string.Empty;
        long gold = 0;
        long change = 0;
        if (ptype == 0)
        {
            game = "农场";
            gname = "金币";
            gold = new BCW.farm.BLL.NC_user().GetGold(meid);
            change = farm;
        }
        if (ptype == 1)
        {
            game = "农场";
            gname = "经验";
            gold = new BCW.farm.BLL.NC_user().Getjingyan(meid);
            change = farm;
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("你当前拥有" + gold + gname + ",来自" + game);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("" + change + gname + "可兑换为1" + drawJifen + "" + "<br />");
        builder.Append("你的金币可兑换" + gold / farm + drawJifen + "<br />");
        //builder.Append("你想兑换多少？");
        builder.Append(Out.Tab("</div>", ""));

        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getPage("draw.aspx?act=duiq&amp;ptype=" + ptype + "") + "\">兑换" + gold / change + drawJifen + "</a>");
        //builder.Append(Out.Tab("</div>", ""));

        //输入框
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string strText = "输入金币进行兑换:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确定,draw.aspx?act=duis&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //}

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=trun") + "\">返回换" + drawJifen + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //换币全部
    private void DuiqPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "请勿做非法操作"));
        string game = string.Empty;
        string gname = string.Empty;
        long gold = 0;
        long change = 0;
        if (ptype == 0)
        {
            game = "农场";
            gname = "金币";
            gold = new BCW.farm.BLL.NC_user().GetGold(meid);
            change = farm;
        }
        if (ptype == 1)
        {
            game = "农场";
            gname = "经验";
            gold = new BCW.farm.BLL.NC_user().Getjingyan(meid);
            change = farm;
        }

        if (gold > 0)
        {
            if (ptype == 0)
            {
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -((gold / change) * change), "在" + drawJifen + "换币为" + drawJifen + ",花费了" + ((gold / change) * change) + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - ((gold / change) * change)) + "金币.", 23);
            }
            if (ptype == 1)
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -((gold / change) * change));

            }
            new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, Convert.ToInt32(gold / change), game + gname + "兑换" + GameName + drawJifen + "");

            BCW.Draw.Model.Drawnotes notes = new BCW.Draw.Model.Drawnotes();
            notes.UsID = meid;
            notes.game = game;
            notes.gname = gname;
            notes.gvalue = ((gold / change) * change);
            notes.jifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
            notes.jvalue = (gold / change);
            notes.change = change;
            notes.date = DateTime.Now;
            new BCW.Draw.BLL.Drawnotes().Add(notes);

            Utils.Success("换币成功", "恭喜你成功换币", Utils.getUrl("draw.aspx?act=trun"), "2");
        }
        else
        {
            Utils.Success("换币失败", "抱歉，你的" + gname + "不足以兑换" + drawJifen + "", Utils.getUrl("draw.aspx?act=trun"), "2");
        }
    }
    //换币输入值
    private void DuisPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "请勿做非法操作"));
        string game = string.Empty;
        string gname = string.Empty;
        long gold = 0;
        long change = 0;
        if (ptype == 0)
        {
            game = "农场";
            gname = "金币";
            gold = new BCW.farm.BLL.NC_user().GetGold(meid);
            change = farm;
        }
        if (ptype == 1)
        {
            game = "农场";
            gname = "经验";
            gold = new BCW.farm.BLL.NC_user().Getjingyan(meid);
            change = farm;
        }

        if (uid <= gold && uid >= change && gold > 0)
        {
            if (ptype == 0)
            {
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -((uid / change) * change), "在" + GameName + "换金币为" + drawJifen + ",花费了" + (uid / change) * change + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - (uid / change) * change) + "金币.", 23);
            }
            if (ptype == 1)
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -uid * change);

            }
            new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, Convert.ToInt32(uid / change), game + gname + "兑换" + GameName + drawJifen + "");

            BCW.Draw.Model.Drawnotes notes = new BCW.Draw.Model.Drawnotes();
            notes.UsID = meid;
            notes.game = game;
            notes.gname = gname;
            notes.gvalue = ((uid / change) * change);
            notes.jifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
            notes.jvalue = uid / change;
            notes.change = change;
            notes.date = DateTime.Now;
            new BCW.Draw.BLL.Drawnotes().Add(notes);

            Utils.Success("换" + drawJifen + "成功", "恭喜你成功兑换" + uid / change + drawJifen + ",花费了" + (uid / change) * change + "金币", Utils.getUrl("draw.aspx?act=trun"), "5");
        }
        else
        {
            if (uid > gold)
            {
                Utils.Success("换" + drawJifen + "失败", "金币不足，请输入正确的金币值", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "3");
            }
            if (uid <= 0)
            {
                Utils.Success("换" + drawJifen + "失败", "请输入正确的金币值", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "3");
            }
            if (uid < change)
            {
                Utils.Success("换" + drawJifen + "失败", "你输入的金币值不足以兑换一个" + drawJifen + "", Utils.getUrl("draw.aspx?act=dui&amp;ptype=" + ptype + ""), "5");
            }
        }

    }
    //积分消费记录
    private void ConListPage()
    {
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;" + drawJifen + "记录");
        builder.Append(Out.Tab("</div >", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【" + drawJifen + "记录】");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + meid + " ";

        // 开始读取列表
        IList<BCW.Draw.Model.DrawJifenlog> listJifenlog = new BCW.Draw.BLL.DrawJifenlog().GetJifenlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listJifenlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawJifenlog n in listJifenlog)
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
                builder.AppendFormat("{0}.{1}|操作{2}|结{3}({4})", (pageIndex - 1) * pageSize + k, Out.SysUBB(n.AcText), n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 1));

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

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //积分抽奖
    private void ChouJiangPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int R = Utils.ParseInt(Utils.GetRequest("R", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int Counts = Utils.ParseInt(Utils.GetRequest("GoodsCounts", "all", 0, @"^[0-9]\d*$", ""));
        int rank = new BCW.Draw.BLL.DrawTen().GetRank(Counts);
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        string mygoods = new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num);
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;" + drawJifen + "抽奖");
        builder.Append(Out.Tab("</div >", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img src=\"" + " img/Draw_img/cj4.jpg" + "\" height=\"30\" weight=\"60\" alt=\"load\"/>" + "&nbsp;【" + drawJifen + "抽奖】");
        builder.Append(Out.Tab("</div >", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("精美奖品等你来拿");
        builder.Append(Out.Tab("</div >", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("每次抽奖将花掉" + jifen + drawJifen);
        builder.Append(Out.Tab("</div >", "<br />"));

        int Jifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("你的当前" + drawJifen + "：" + Jifen);
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet das = new BCW.Draw.BLL.DrawTen().GetList("ID", "GoodsCounts>'0'");
        if (das != null && das.Tables[0].Rows.Count > 0)
        {
            DataSet ds = new BCW.Draw.BLL.DrawTen().GetList("COUNT(GoodsCounts) as aa", "GoodsCounts='0'");
            int uu = 0;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                uu = int.Parse(ds.Tables[0].Rows[0]["aa"].ToString());
            }

            if (Jifen < jifen)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你的" + drawJifen + "不足以抽奖");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                if (uu == JJ)
                {

                }
                else
                {
                    if (Convert.ToInt32(ub.GetSub("huitie", xmlPath)) == 0)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=realcj") + "\">开始抽奖</a>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    else
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("等待管理员开放抽奖活动...");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                }
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + " img/Draw_img/cj4.jpg" + "\" height=\"100\" weight=\"60\" alt=\"load\"/>" + "<br />");
            builder.Append("抽奖箱奖品为空，请等待管理员添加奖品<br />");
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=neixian") + "\">提醒管理员添加奖品</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        if (R <= chance && R != 0 && Counts != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b style=\"color:red\">运气真好，恭喜抽中" + GetRank(rank) + "：</b><a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;Counts=" + Counts + "&amp;Num=" + num + "") + "\">" + mygoods + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + " img/Draw_img/zjl.gif" + "\" height=\"100\" weight=\"100\" alt=\"load\"/>" + "&nbsp;&nbsp;<br />");
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin") + "\">前去领奖--></a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        else if (R == 0)
        {
            Random hy = new Random();
            int index = hy.Next(0, 5);
            string haoyun = f[index];
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(haoyun);
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (R == 688)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("抽奖结束，稍后再来" + R);
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            Random huy = new Random();
            int index1 = huy.Next(0, 5);
            string huaiyun = f1[index1];

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(huaiyun);
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + " img/Draw_img/link.gif" + "\" height=\"100\" weight=\"100\" alt=\"load\"/>" + "&nbsp;&nbsp;");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", ""));

        //builder.Append(Out.Tab("<div>", "<br/>"));
        //builder.Append("" + "-----------" + "<br />");
        //builder.Append(Out.Tab("</div>", ""));

        #region 动态
        //builder.Append(Out.Tab("<div>", ""));
        //int rm = new BCW.Draw.BLL.DrawUser().GetMaxId();
        //BCW.Draw.Model.DrawUser model5 = new BCW.Draw.BLL.DrawUser().GetDrawUser(rm - 1);
        //try
        //{
        //    DataSet dds = new BCW.Draw.BLL.DrawUser().GetList("top 5 *", "MyGoodsStatue!='88' order by id desc");
        //    int p = 0;
        //    if (dds != null && dds.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dds.Tables[0].Rows.Count; i++)
        //        {
        //            p = int.Parse(dds.Tables[0].Rows[i]["MyGoodsStatue"].ToString());
        //            int UsID = int.Parse(dds.Tables[0].Rows[i]["UsID"].ToString());
        //            string UsName = dds.Tables[0].Rows[i]["UsName"].ToString();
        //            string MyGoods = dds.Tables[0].Rows[i]["MyGoods"].ToString();
        //            string OnTime = dds.Tables[0].Rows[i]["OnTime"].ToString();
        //            int GoodsCounts = int.Parse(dds.Tables[0].Rows[i]["GoodsCounts"].ToString());
        //            R = int.Parse(dds.Tables[0].Rows[i]["R"].ToString());
        //            int Num = int.Parse(dds.Tables[0].Rows[i]["Num"].ToString());
        //            TimeSpan time = DateTime.Now - Convert.ToDateTime(OnTime);

        //            int d1 = time.Days;
        //            int d = time.Hours;
        //            int e = time.Minutes;
        //            int f = time.Seconds;

        //            if (R != 888)
        //            {
        //                if (d1 == 0)
        //                {
        //                    if (d == 0 && e == 0 && f == 0)
        //                    {
        //                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                    }
        //                    else if (d == 0 && e == 0)
        //                        builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                    else if (d == 0)
        //                        builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                    else
        //                        builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                }
        //                else
        //                    builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "抽中奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //            }
        //            else
        //            {
        //                if (d1 == 0)
        //                {
        //                    if (d == 0 && e == 0 && f == 0)
        //                    {
        //                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                    }
        //                    else if (d == 0 && e == 0)
        //                        builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                    else if (d == 0)
        //                        builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                    else
        //                        builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //                }
        //                else
        //                    builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + GoodsCounts + "&amp;num=" + Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + MyGoods + "</a>" + "<br />");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        builder.Append("没有更多动态了...");
        //    }
        //}
        //catch
        //{
        //    builder.Append("");
        //}
        #endregion

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //确定用积分抽奖
    private void RealCJPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        if (meid == 0)
            Utils.Login();
        int R = Utils.ParseInt(Utils.GetRequest("R", "post", 0, @"^[1-9]\d*$", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int GoodsCounts = Utils.ParseInt(Utils.GetRequest("GoodsCounts", "post", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);

        if (Convert.ToInt32(ub.GetSub("huitie", xmlPath)) != 0)
            Utils.Error("等待管理员开放抽奖...", "");

        int Jifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
        int getR = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));//奖箱总共有几等奖
        int getchance = Convert.ToInt32(ub.GetSub("chance", xmlPath));
        int xmllun = Convert.ToInt32(ub.GetSub("lun", xmlPath));
        int lun = 0;
        try { lun = xmllun; }
        catch { }
        int getJiangAll = Convert.ToInt32(new BCW.Draw.BLL.DrawBox().GetAllNum(lun));//奖箱奖品总数量
        string[] kg = ub.GetSub("jiangjikg", xmlPath).Split('#');//奖级开关

        DataSet das = new BCW.Draw.BLL.DrawTen().GetList("ID", "GoodsCounts>'0'");
        if (das != null && das.Tables[0].Rows.Count > 0)
        {
            DataSet dsy = new BCW.Draw.BLL.DrawTen().GetList("COUNT(GoodsCounts) as aa", "GoodsCounts='0'");
            int uuy = 0;
            if (dsy != null && dsy.Tables[0].Rows.Count > 0)
            {
                uuy = int.Parse(dsy.Tables[0].Rows[0]["aa"].ToString());
            }

            if (Jifen < jifen)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你的" + drawJifen + "不足以抽奖");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                if (uuy == getR)//如果奖箱奖品为空
                {

                }
                else
                {


                    BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();

                    #region 中奖算法
                    Random ran = new Random();
                    DataSet ds = new BCW.Draw.BLL.DrawTen().GetList("COUNT(GoodsCounts) as aa", "GoodsCounts='0'");
                    int uu = 0;
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        uu = int.Parse(ds.Tables[0].Rows[0]["aa"].ToString());
                    }

                    if (uu == JJ)
                    {
                        R = 688;//奖箱为空
                    }
                    else
                    {
                        R = ran.Next(1, 101);//从100中随机出一个数字，百分比为多少则小于多少以下的都是中奖
                    }

                    #endregion


                    if (R <= chance)//中奖,百分比中随机的数小于等于百分比的值，则中奖
                    {
                        int rank = 0;
                        if (getR <= 26)//当奖箱奖级为26以下时
                        {
                            #region 计算对应的奖级
                            //从集合中随机抽取个数，即为从奖箱中随机抽取一个奖级
                            const ushort COUNT = 1;
                            //循环次数
                            int FOR_COUNT = 1000;//奖箱奖品总数量getJiangAll
                            RandomController rc = new RandomController(COUNT);
                            //随机数生成器
                            Random rand = new Random();
                            //循环生成随机数
                            char itemt = Convert.ToChar("1");

                            for (int i = 0; i < FOR_COUNT; i++)
                            {
                                char[] rands = rc.ControllerRandomExtract(rand);//a
                                for (int j = 0; j < COUNT; j++)
                                {
                                    itemt = rands[j];
                                }
                                // Thread.Sleep(5);

                                if (itemt.ToString() == "A")
                                {
                                    rank = 0;//一等奖                          
                                }
                                if (itemt.ToString() == "B")
                                {
                                    rank = 1;//二等奖                          
                                }
                                if (itemt.ToString() == "C")
                                {
                                    rank = 2;//三等奖                          
                                }
                                if (itemt.ToString() == "D")
                                {
                                    rank = 3;//四等奖                          
                                }
                                if (itemt.ToString() == "E")
                                {
                                    rank = 4;//五等奖                          
                                }
                                if (itemt.ToString() == "F")
                                {
                                    rank = 5;//六等奖                          
                                }
                                if (itemt.ToString() == "G")
                                {
                                    rank = 6;//七等奖                          
                                }
                                if (itemt.ToString() == "H")
                                {
                                    rank = 7;//八等奖                          
                                }
                                if (itemt.ToString() == "I")
                                {
                                    rank = 8;//九等奖                          
                                }
                                if (itemt.ToString() == "J")
                                {
                                    rank = 9;//十等奖                          
                                }
                                if (itemt.ToString() == "K")
                                {
                                    rank = 10;//十一等奖                          
                                }
                                if (itemt.ToString() == "L")
                                {
                                    rank = 11;//十二等奖                          
                                }
                                if (itemt.ToString() == "M")
                                {
                                    rank = 12;//十三等奖                          
                                }
                                if (itemt.ToString() == "N")
                                {
                                    rank = 13;//十四等奖                          
                                }
                                if (itemt.ToString() == "O")
                                {
                                    rank = 14;//十五等奖                          
                                }
                                if (itemt.ToString() == "P")
                                {
                                    rank = 15;//十六等奖                          
                                }
                                if (itemt.ToString() == "Q")
                                {
                                    rank = 16;//十七等奖                          
                                }
                                if (itemt.ToString() == "R")
                                {
                                    rank = 17;//十八等奖                          
                                }
                                if (itemt.ToString() == "S")
                                {
                                    rank = 18;//十九等奖                          
                                }
                                if (itemt.ToString() == "T")
                                {
                                    rank = 19;//二十等奖                          
                                }
                                if (itemt.ToString() == "U")
                                {
                                    rank = 20;//21等奖                          
                                }
                                if (itemt.ToString() == "V")
                                {
                                    rank = 21;//22等奖                          
                                }
                                if (itemt.ToString() == "W")
                                {
                                    rank = 22;//23等奖                          
                                }
                                if (itemt.ToString() == "X")
                                {
                                    rank = 23;//24等奖                          
                                }
                                if (itemt.ToString() == "Y")
                                {
                                    rank = 24;//25等奖                          
                                }
                                if (itemt.ToString() == "Z")
                                {
                                    rank = 25;//26等奖                          
                                }
                                //else
                                //{
                                //    break;
                                //}

                                if (Convert.ToInt32(new BCW.Draw.BLL.DrawTen().GetCounts(rank)) == 0 || Convert.ToInt32(kg[rank]) == 1)
                                    continue;
                                else
                                    break;
                            }
                            #endregion
                        }
                        else//当奖箱为26以上时随机抽奖箱，被抽中的概率都一样，但可以通过奖级开关控制某些等奖的参与
                        {
                            for (int i = 0; i < 1000; i++)
                            {
                                Random rankkk = new Random();
                                rank = rankkk.Next(0, (getR + 1));
                                if (new BCW.Draw.BLL.DrawTen().GetCounts(rank) == 0 || Convert.ToInt32(kg[rank]) == 1)
                                    continue;
                                else
                                    break;
                            }
                        }

                        GoodsCounts = new BCW.Draw.BLL.DrawTen().GetCounts(rank);
                        string MyGoods = new BCW.Draw.BLL.DrawBox().GetGoodsName(GoodsCounts);
                        string Explain = new BCW.Draw.BLL.DrawBox().GetExplain(GoodsCounts);
                        string MyGoodsImg = new BCW.Draw.BLL.DrawBox().GetGoodsImg(GoodsCounts);
                        int MyGoodsType = new BCW.Draw.BLL.DrawBox().GetGoodsType(GoodsCounts);
                        int MyGoodsValue = new BCW.Draw.BLL.DrawBox().GetGoodsValue(GoodsCounts);
                        int MyGoodsNum = new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts);
                        int MyGoodsStatue = new BCW.Draw.BLL.DrawBox().GetStatue(GoodsCounts);


                        if (new BCW.Draw.BLL.DrawUser().Exists(1))//存在第一条记录
                        {
                            int Id = new BCW.Draw.BLL.DrawUser().GetMaxId() - 1;
                            BCW.Draw.Model.DrawUser N = new BCW.Draw.BLL.DrawUser().GetDrawUser(Id);
                            model.UsID = meid;
                            model.UsName = name;
                            //model.GoodsCounts = Jifen - jifen;
                            model.GoodsCounts = GoodsCounts;
                            model.MyGoods = MyGoods;
                            model.Explain = Explain;
                            model.MyGoodsImg = MyGoodsImg;
                            model.MyGoodsType = MyGoodsType;
                            model.MyGoodsValue = MyGoodsValue;

                            model.MyGoodsNum = 1;
                            model.OnTime = DateTime.Now;
                            model.InTime = DateTime.Now;
                            model.Address = "";
                            model.Phone = "";
                            model.Email = "";
                            if (GoodsCounts == 0)
                            {
                                model.R = 888;
                                model.MyGoodsStatue = 88;
                            }
                            else
                            {
                                model.MyGoodsStatue = MyGoodsStatue;
                                model.R = R;
                            }
                            model.Num = N.Num + 1;
                            model.RealName = "";
                            model.Express = "";
                            model.Numbers = "";
                            new BCW.Draw.BLL.DrawUser().Add(model);

                            BCW.Draw.Model.DrawTen G = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                            G.ID = G.ID;
                            G.Rank = G.Rank;
                            if (new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts) == 1)
                            {
                                G.GoodsCounts = 0;
                            }
                            else
                            {
                                G.GoodsCounts = G.GoodsCounts;
                            }
                            new BCW.Draw.BLL.DrawTen().Update(G);

                            //new BCW.Draw.BLL.DrawBox().UpdateOverTime(GoodsCounts, DateTime.Now);
                            //BCW.Draw.Model.DrawBox B = new BCW.Draw.BLL.DrawBox().GetDrawBox(rank);
                            //new BCW.Draw.BLL.DrawBox().UpdateStatue(GoodsCounts, 3);

                        }
                        else//表不存在记录
                        {
                            //  int Id = new BCW.Draw.BLL.DrawUser().GetMaxId() - 1;
                            model.UsID = meid;
                            model.UsName = name;
                            //model.GoodsCounts = Jifen - jifen;
                            model.GoodsCounts = GoodsCounts;
                            model.MyGoods = MyGoods;
                            model.Explain = Explain;
                            model.MyGoodsImg = MyGoodsImg;
                            model.MyGoodsType = MyGoodsType;
                            model.MyGoodsValue = MyGoodsValue;

                            model.MyGoodsNum = 1;
                            model.OnTime = DateTime.Now;
                            model.InTime = DateTime.Now;
                            model.Address = "";
                            model.Phone = "";
                            model.Email = "";
                            if (GoodsCounts == 0)
                            {
                                model.R = 888;
                                model.MyGoodsStatue = 88;
                            }
                            else
                            {
                                model.MyGoodsStatue = MyGoodsStatue;
                                model.R = R;
                            }
                            model.Num = 1;
                            model.RealName = "";
                            model.Express = "";
                            model.Numbers = "";
                            new BCW.Draw.BLL.DrawUser().Add(model);

                            BCW.Draw.Model.DrawTen G = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                            G.ID = G.ID;
                            G.Rank = G.Rank;
                            if (new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts) == 1)
                            {
                                G.GoodsCounts = 0;
                            }
                            else
                            {
                                G.GoodsCounts = G.GoodsCounts;
                            }
                            new BCW.Draw.BLL.DrawTen().Update(G);

                            //new BCW.Draw.BLL.DrawBox().UpdateOverTime(GoodsCounts, DateTime.Now);
                            //new BCW.Draw.BLL.DrawBox().UpdateStatue(GoodsCounts, 3);
                        }

                        //int numm = new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts);
                        if (new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts) > 0)
                        {
                            new BCW.Draw.BLL.DrawBox().UpdateGoodsNum(GoodsCounts, (new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts) - 1));
                        }

                        if (new BCW.Draw.BLL.DrawBox().GetGoodsNum(GoodsCounts) == 0)
                        {
                            new BCW.Draw.BLL.DrawBox().UpdateStatue(GoodsCounts, 1);//当奖箱奖品用光之后用数字1表示
                        }

                        BCW.Draw.Model.Drawlist lists = new BCW.Draw.Model.Drawlist();
                        lists.UsID = meid;
                        lists.Time = DateTime.Now;
                        if (GoodsCounts != 0)
                        {
                            lists.Type = 1;//1表示为抽奖中奖
                        }
                        else
                        {
                            lists.Type = 0;//1表示为抽奖中奖
                        }
                        lists.GoodsCounts = model.Num;
                        lists.Jifen = jifen;
                        new BCW.Draw.BLL.Drawlist().Add(lists);//记录写进记录表

                        //发内线
                        int id = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
                        string wText = "在[url=/bbs/game/Draw.aspx]抽奖[/url]抽中奖品：" + MyGoods + "";
                        if (GoodsCounts != 0)
                        {
                            new BCW.BLL.Action().Add(1013, id, meid, name, wText);
                        }

                        BCW.Draw.Model.DrawJifen jf = new BCW.Draw.Model.DrawJifen();
                        jf.Jifen = jf.Jifen - jifen;
                        if (GoodsCounts != 0)
                        {
                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, jf.Jifen, "" + GameName + drawJifen + "抽中" + MyGoods + "|奖品ID：" + model.Num);
                        }
                        else
                        {
                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, jf.Jifen, "" + GameName + drawJifen + "抽奖");
                        }

                    }
                    else//不中奖
                    {
                        if (new BCW.Draw.BLL.DrawUser().Exists(1))//存在第一条记录
                        {
                            int Id = new BCW.Draw.BLL.DrawUser().GetMaxId() - 1;
                            BCW.Draw.Model.DrawUser N = new BCW.Draw.BLL.DrawUser().GetDrawUser(Id);
                            model.UsID = meid;
                            model.UsName = name;
                            //model.GoodsCounts = Jifen - jifen;
                            model.GoodsCounts = 0;
                            model.MyGoods = "";
                            model.Explain = "";
                            model.MyGoodsImg = "";
                            model.MyGoodsType = 88;
                            model.MyGoodsValue = 88;
                            model.MyGoodsStatue = 88;
                            model.MyGoodsNum = 0;
                            model.OnTime = DateTime.Now;
                            model.InTime = DateTime.Now;
                            model.Address = "";
                            model.Phone = "";
                            model.Email = "";
                            model.R = R;
                            model.Num = N.Num + 1;
                            model.RealName = "";
                            model.Express = "";
                            model.Numbers = "";
                            new BCW.Draw.BLL.DrawUser().Add(model);
                        }
                        else//表不存在记录
                        {
                            int Id = new BCW.Draw.BLL.DrawUser().GetMaxId() - 1;

                            model.UsID = meid;
                            model.UsName = name;
                            //model.GoodsCounts = Jifen - jifen;
                            model.GoodsCounts = 0;
                            model.MyGoods = "";
                            model.Explain = "";
                            model.MyGoodsImg = "";
                            model.MyGoodsType = 88;
                            model.MyGoodsValue = 88;
                            model.MyGoodsStatue = 88;
                            model.MyGoodsNum = 0;
                            model.OnTime = DateTime.Now;
                            model.InTime = DateTime.Now;
                            model.Address = "";
                            model.Phone = "";
                            model.Email = "";
                            model.R = R;
                            model.Num = 1;
                            model.RealName = "";
                            model.Express = "";
                            model.Numbers = "";
                            new BCW.Draw.BLL.DrawUser().Add(model);
                        }

                        BCW.Draw.Model.Drawlist lists = new BCW.Draw.Model.Drawlist();
                        lists.UsID = meid;
                        lists.Time = DateTime.Now;
                        lists.Type = 0;//0表示为抽奖不中奖
                        lists.GoodsCounts = 0;
                        lists.Jifen = jifen;
                        new BCW.Draw.BLL.Drawlist().Add(lists);//记录写进记录表

                        BCW.Draw.Model.DrawJifen jf = new BCW.Draw.Model.DrawJifen();
                        jf.Jifen = jf.Jifen - jifen;
                        new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, jf.Jifen, "" + GameName + drawJifen + "抽奖");
                    }
                    Utils.Success("抽奖", "花掉" + jifen + drawJifen + ",正在抽奖...", Utils.getUrl("draw.aspx?act=choujiang&amp;Num=" + model.Num + "&amp;R=" + R + "&amp;GoodsCounts=" + GoodsCounts + ""), "3");

                }
            }
        }
        else
        {
            Utils.Error("请不要非法操作...", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("花掉" + jifen + drawJifen + ",正在抽奖...");
        builder.Append(Out.Tab("</div>", ""));
    }
    //提醒管理员添加奖品
    private void NeixianPage()
    {
        //发内线
        string strLog = " " + GameName + "奖箱为空，请添加奖品..";
        new BCW.BLL.Guest().Add(0, 10086, new BCW.BLL.User().GetUsName(10086), strLog);
        Utils.Success("" + GameName + "", "你的消息已经发送给管理员，谢谢你的提醒，请耐心等待，祝你愉快..", Utils.getUrl("draw.aspx"), "3");
    }
    //积分兑换首页
    private void DuiHuanPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;" + drawJifen + "兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img src=\"" + " img/Draw_img/cj4.jpg" + "\" height=\"30\" weight=\"auto\" alt=\"load\"/>" + "&nbsp;&nbsp;");
        builder.Append("<b>" + drawJifen + "兑换</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        DataSet ds = new BCW.Draw.BLL.DrawTen().GetList("ID", "GoodsCounts>'0'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                //商品最大Id
                int newGoods = new BCW.Draw.BLL.DrawBox().GetMaxId();
                //路径
                //string Logo = ub.GetSub("img", xmlPath);
                //显示最大图片量
                string imgCount = ub.GetSub("imgCount", xmlPath);

                DataSet das = new BCW.Draw.BLL.DrawBox().GetList("ID", "Statue=0 and Points!=0 Order by GoodsCounts Desc");
                if (das != null && das.Tables[0].Rows.Count > 0)
                {
                    for (int lists = 0; lists < das.Tables[0].Rows.Count; lists++)
                    {
                        int uu = int.Parse(das.Tables[0].Rows[lists][0].ToString());
                        DateTime nowTime = DateTime.Now;
                        BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(uu);
                        string type = "";
                        if (model.GoodsType == 0) { type = "酷币"; }
                        if (model.GoodsType == 1) { type = "实物"; }
                        if (model.GoodsType == 2) { type = "道具"; }
                        if (model.GoodsType == 3) { type = "属性"; }

                        //是否为进行中商品   
                        if (model.Statue == 0)
                        {
                            builder.Append((lists + 1) + ".");
                            builder.Append("奖品编号：" + model.GoodsCounts + "<br />");
                            if (model.GoodsType == 0)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a><br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;酷币价值：" + model.GoodsValue + " " + "<br />");
                            }
                            if (model.GoodsType == 1)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a><br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                //builder.Append("&nbsp;&nbsp;&nbsp;奖品价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                            }
                            if (model.GoodsType == 2)
                            {
                                string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a><br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "(" + GN + ")" + "<br />");
                                //builder.Append("&nbsp;&nbsp;&nbsp;道具价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                            }
                            if (model.GoodsType == 3)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品等级：" + GetRank(model.rank) + " " + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsName + "</a>" + "+" + model.GoodsValue + " <br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                            }
                            if (model.points == 0)
                                builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + "：" + "不可兑换" + "<br />&nbsp;&nbsp;&nbsp;奖品余量：" + model.GoodsNum + "");
                            else
                                builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + "：" + model.points + "<br />&nbsp;&nbsp;&nbsp;奖品余量：" + model.GoodsNum + "");
                            // lists++;
                            if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                            {
                                builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述:");
                                builder.Append("<br/>");
                                string[] imgNum = model.GoodsImg.Split(',');
                                for (int c = 0; c < imgNum.Length - 1; c++)
                                {
                                    if (model.GoodsType == 2)
                                    {
                                        builder.Append("&nbsp;&nbsp;&nbsp;" + "<img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/>" + "&nbsp;&nbsp;");
                                    }
                                    else
                                    {
                                        if (id != 0)
                                        {
                                            if (id == model.GoodsCounts)
                                                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;id=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                            else
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                        }
                                        else
                                        {
                                            builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                        }
                                    }
                                }
                            }
                            builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");
                            if (model.points != 0)
                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">马上兑换</a><br/>");

                        }
                    }
                }
            }
            catch
            {
                builder.Append("就这么多奖品了，赶快去试试运气吧...<br/>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + " img/Draw_img/cj4.jpg" + "\" height=\"100\" weight=\"60\" alt=\"load\"/>" + "");
            builder.Append(Out.Tab("</div >", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("奖品箱为空，请等待管理员添加奖品再来兑换哦！<br />");
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=neixian") + "\">提醒管理员添加奖品</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getPage("game.aspx&amp;backurl=" + Utils.PostPage(1)+"") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //积分兑换面页
    private void IsDuiHuanPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        //string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;奖品兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime nowTime = DateTime.Now;
        BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);
        string type = "";
        if (model.GoodsType == 0) { type = "酷币"; }
        if (model.GoodsType == 1) { type = "实物"; }
        if (model.GoodsType == 2) { type = "道具"; }
        if (model.GoodsType == 3) { type = "属性"; }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>奖品兑换</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        //是否为进行中商品   
        if (model.Statue == 0)
        {

            builder.Append("奖品编号：" + model.GoodsCounts + "<br />");
            if (model.GoodsType == 0)
            {
                builder.Append("奖品等级：" + GetRank(model.rank) + " " + "<br />");
                builder.Append("奖品名称：" + model.GoodsName + "<br />");
                builder.Append("奖品类型：" + type + "<br />");
                builder.Append("酷币价值：" + model.GoodsValue + " " + "<br />");
            }
            if (model.GoodsType == 1)
            {
                builder.Append("奖品等级：" + GetRank(model.rank) + " " + "<br />");
                builder.Append("奖品名称：" + model.GoodsName + "<br />");
                builder.Append("奖品类型：" + type + "<br />");
                //builder.Append("奖品价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
            }
            if (model.GoodsType == 2)
            {
                string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
                builder.Append("奖品等级：" + GetRank(model.rank) + " " + "<br />");
                builder.Append("奖品名称：" + model.GoodsName + "<br />");
                builder.Append("奖品类型：" + type + "(" + GN + ")" + "<br />");
                //builder.Append("道具价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
            }
            if (model.GoodsType == 3)
            {
                builder.Append("奖品等级：" + GetRank(model.rank) + " " + "<br />");
                builder.Append("奖品名称：" + model.GoodsName + "+" + model.GoodsValue + " <br />");
                builder.Append("奖品类型：" + type + "<br />");
            }

            if (model.points == 0)
                builder.Append("兑换" + drawJifen + "：" + "不可兑换" + "<br />奖品余量：" + model.GoodsNum + "");
            else
                builder.Append("兑换" + drawJifen + "：" + model.points + "<br />奖品余量：" + model.GoodsNum + "");

            if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
            {
                builder.Append("<br/>" + "图片描述：");
                builder.Append("<br/>");
                string[] imgNum = model.GoodsImg.Split(',');
                // foreach (string n in imgNum)
                for (int c = 0; c < imgNum.Length - 1; c++)
                {
                    if (model.GoodsType == 2)
                    {
                        builder.Append("" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "&nbsp;&nbsp;");
                    }
                    else
                    {
                        if (img != 0)
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                        else
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=isduihuan&amp;id=" + model.GoodsCounts + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                    }
                }
            }
            builder.Append("<br/>" + "奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");
            builder.Append(Out.Tab("</div>", "<br />"));

            int Jifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的当前" + drawJifen + "：" + Jifen);
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if ((Jifen > model.points || Jifen == model.points))
            {
                if (model.points != 0)
                    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=willduihuan&amp;id=" + id + "") + "\">马上兑换</a> ");
                else
                    builder.Append("此奖品不支持" + drawJifen + "兑换，请前去<a href=\"" + Utils.getUrl("draw.aspx?act=choujiang") + "\">" + drawJifen + "抽奖</a>参与活动");
            }
            else
            {
                builder.Append("由于你的" + drawJifen + "不足以兑换奖品，很抱歉不能进行" + drawJifen + "兑换");
            }
            builder.Append(Out.Tab("</div>", " "));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        // builder.Append("<a href=\"" + Utils.getPage("game.aspx&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //确认花费积分兑换
    private void WillDuihuanPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[1-9]\d*$", "1"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;奖品兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);

        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("兑换一件此奖品将花费：" + model.points + "" + drawJifen + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            if (model.points != 0)
            {
                string strText = ",你要兑换的数量:/,,";
                string strName = "id,num,ptype,backurl";
                string strType = "hidden,num,hidden,hidden";
                string strValu = "" + id + "'" + 1 + "'" + 1 + "'" + Utils.getPage(0) + "";
                string strEmpt = ",false,false,false,false";
                string strIdea = "/";
                string strOthe = "确认兑换,draw.aspx?act=willduihuan,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;backurl=" + Utils.PostPage(1) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("兑换一件此奖品将花费：" + model.points + "" + drawJifen + "<br />");
            builder.Append("您选择兑换数量：" + num + "<br />");
            builder.Append("您的选择需要花费" + drawJifen + "：" + model.points * num + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = ",,";
            string strName = "id,num,backurl";
            string strType = "hidden,hidden,hidden";
            string strValu = "" + id + "'" + num + "'" + Utils.getPage(0) + "";
            string strEmpt = ",false,false,false";
            string strIdea = "/";
            string strOthe = "确认兑换,draw.aspx?act=goduihuan,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;backurl=" + Utils.PostPage(1) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=duihuan&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回奖品兑换</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //确认兑换页面
    private void GoDuiHuanPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int number = int.Parse(Utils.GetRequest("num", "all", 1, @"^[1-9]\d*$", "1"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;奖品兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int Jifen = new BCW.Draw.BLL.DrawJifen().GetJifen(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("你的当前" + drawJifen + "：" + Jifen);
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);
        if (number * model.points > Jifen)
            Utils.Error("您的" + drawJifen + "不足，请重新选择", "");
        if (number > model.GoodsNum)
            Utils.Error("库存不足，请重新选择", "");
        if (model.points == 0)
            Utils.Error("此奖品不支持" + drawJifen + "兑换", "");
        if (model.points != 0 && (Jifen > model.points || Jifen == model.points))
        {
            int Statue = new BCW.Draw.BLL.DrawUser().GetMyStatue(id);
            if (model.GoodsNum <= 0)
            {
                Utils.Error("此奖品已经为空，不能再兑换...", Utils.getUrl("draw.aspx?act=duihuan"));
            }
            else
            {
                int num = new BCW.Draw.BLL.DrawBox().GetGoodsNum(id);
                BCW.Draw.Model.DrawUser model2 = new BCW.Draw.Model.DrawUser();
                if (new BCW.Draw.BLL.DrawUser().Exists(1))//存在第一条记录
                {
                    int Id = new BCW.Draw.BLL.DrawUser().GetMaxId() - 1;
                    BCW.Draw.Model.DrawUser N = new BCW.Draw.BLL.DrawUser().GetDrawUser(Id);

                    model2.UsID = meid;
                    model2.UsName = name;
                    model2.GoodsCounts = id;
                    model2.MyGoods = model.GoodsName;
                    model2.Explain = model.Explain;
                    model2.MyGoodsImg = model.GoodsImg;
                    model2.MyGoodsType = model.GoodsType;
                    model2.MyGoodsValue = model.GoodsValue;
                    model2.MyGoodsStatue = 4;//用数字4表示兑换
                    model2.MyGoodsNum = number;
                    model2.OnTime = DateTime.Now;
                    model2.InTime = DateTime.Now;
                    model2.Address = "";
                    model2.Phone = "";
                    model2.Email = "";
                    model2.R = 888;//这个数毫无意义
                    model2.Num = N.Num + 1;
                    model2.RealName = "";
                    model2.Express = "";
                    model2.Numbers = "";
                    if (!new BCW.Draw.BLL.DrawUser().Existsnum(model2.Num))
                        new BCW.Draw.BLL.DrawUser().Add(model2);

                    //int Rank = new BCW.Draw.BLL.DrawTen().GetRank(id);
                    //BCW.Draw.Model.DrawTen G = new BCW.Draw.BLL.DrawTen().GetDrawTen(Rank);
                    //G.ID = G.ID;
                    //G.Rank = G.Rank;
                    //if (num == 1)
                    //{
                    //    G.GoodsCounts = 0;
                    //}
                    //else
                    //{
                    //    G.GoodsCounts = G.GoodsCounts;
                    //}
                    //new BCW.Draw.BLL.DrawTen().Update(G);

                    //new BCW.Draw.BLL.DrawBox().UpdateOverTime(id, DateTime.Now);

                }
                else//表不存在记录
                {
                    int Id = new BCW.Draw.BLL.DrawUser().GetMaxId() - 1;
                    BCW.Draw.Model.DrawUser N = new BCW.Draw.BLL.DrawUser().GetDrawUser(Id);

                    model2.UsID = meid;
                    model2.UsName = name;
                    model2.GoodsCounts = id;
                    model2.MyGoods = model.GoodsName;
                    model2.Explain = model.Explain;
                    model2.MyGoodsImg = model.GoodsImg;
                    model2.MyGoodsType = model.GoodsType;
                    model2.MyGoodsValue = model.GoodsValue;
                    model2.MyGoodsStatue = 4;//用数字4表示兑换
                    model2.MyGoodsNum = number;
                    model2.OnTime = DateTime.Now;
                    model2.InTime = DateTime.Now;
                    model2.Address = "";
                    model2.Phone = "";
                    model2.Email = "";
                    model2.R = 888;//这个数毫无意义
                    model2.Num = 1;
                    model2.RealName = "";
                    model2.Express = "";
                    model2.Numbers = "";
                    if (!new BCW.Draw.BLL.DrawUser().Existsnum(model2.Num))
                        new BCW.Draw.BLL.DrawUser().Add(model2);

                    //int Rank = new BCW.Draw.BLL.DrawTen().GetRank(id);
                    //BCW.Draw.Model.DrawTen G = new BCW.Draw.BLL.DrawTen().GetDrawTen(Rank);
                    //G.ID = G.ID;
                    //G.Rank = G.Rank;
                    //if (num == 1)
                    //{
                    //    G.GoodsCounts = 0;
                    //}
                    //else
                    //{
                    //    G.GoodsCounts = G.GoodsCounts;
                    //}
                    //new BCW.Draw.BLL.DrawTen().Update(G);

                    //new BCW.Draw.BLL.DrawBox().UpdateOverTime(id, DateTime.Now);

                }
                new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, (-(model.points * number)), "" + GameName + drawJifen + "兑换" + model.GoodsName + "|奖品ID:" + model2.Num + "");

                if (num == number)
                {
                    new BCW.Draw.BLL.DrawBox().UpdateStatue(id, 1);//当奖箱奖品用光之后用数字1表示
                }
                new BCW.Draw.BLL.DrawBox().UpdateGoodsNum(id, model.GoodsNum - number);

                BCW.Draw.Model.Drawlist lists = new BCW.Draw.Model.Drawlist();
                lists.UsID = meid;
                lists.Time = DateTime.Now;
                lists.Type = 2;//2表示为兑换的
                lists.GoodsCounts = model2.Num;
                lists.Jifen = (model.points * number);
                new BCW.Draw.BLL.Drawlist().Add(lists);//记录写进记录表

                //动态
                int id2 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
                string wText = "在[url=/bbs/game/Draw.aspx]抽奖[/url]" + drawJifen + "兑换奖品：" + model.GoodsName + "";
                new BCW.BLL.Action().Add(1013, id2, meid, name, wText);


                Utils.Success("" + drawJifen + "兑换", "" + drawJifen + "兑换成功，正在返回我的奖品", Utils.getUrl("draw.aspx?act=myjiangpin"), "2");
            }
        }
        else
        {
            builder.Append("你的" + drawJifen + "不足以兑换奖品");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //我的奖品
    private void MyJiangpinPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;我的奖品");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">请及时【查看】奖品并确认信息，以便奖品及时派送</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>我的奖品：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("<b style=\"color:black\">未确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">未确认信息</a>" + "|");
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">已确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">已确认信息</a>" + "|");

        if (ptype == 2)
            builder.Append("<b style=\"color:black\">已发货" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已发货</a>" + "|");
        if (ptype == 3)
            builder.Append("<b style=\"color:black\">已送达" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">已送达</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件
        if (ptype == 0)
        {
            strWhere = "UsID=" + meid + " and( MyGoodsStatue='0' or MyGoodsStatue='4')";
            strOrder = "OnTime Desc";
        }
        if (ptype == 1)
        {
            strWhere = "UsID=" + meid + " and MyGoodsStatue='3'";
            strOrder = "OnTime Desc";
        }
        if (ptype == 2)
        {
            strWhere = "UsID=" + meid + " and MyGoodsStatue='1'";
            strOrder = "OnTime Desc";
        }
        if (ptype == 3)
        {
            strWhere = "UsID=" + meid + " and MyGoodsStatue='2'";
            strOrder = " InTime Desc";
        }

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listdrawuser = new BCW.Draw.BLL.DrawUser().GetDrawUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdrawuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listdrawuser)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string duikan = string.Empty;//查看兑奖
                if (ptype == 0)
                {
                    duikan = "兑奖";
                }
                else
                {
                    duikan = "查看";
                }

                if (n.R != 888)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "抽中(ID:" + n.Num + ")奖品" + n.MyGoods + ".<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;ptype=" + ptype + "&amp;counts=" + n.GoodsCounts + "&amp;num=" + n.Num + "") + "\">" + duikan + "</a>";
                }
                else
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "兑换(ID:" + n.Num + ")奖品" + n.MyGoods + ".<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;ptype=" + ptype + "&amp;counts=" + n.GoodsCounts + "&amp;num=" + n.Num + "") + "\">" + duikan + "</a>";
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;ptype=" + ptype + "&amp;counts=" + n.GoodsCounts + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //我的奖品具体信息
    private void MyRealJiangpinPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 2, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("Draw.aspx?act=myjiangpin&amp;backurl=" + Utils.PostPage(1) + "") + "\">我的奖品</a>&gt;我的奖品详细信息");
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.Draw.Model.DrawUser model = new BCW.Draw.BLL.DrawUser().GetDrawUserbyCounts(counts);
        BCW.Draw.Model.DrawUser model1 = new BCW.Draw.BLL.DrawUser().GetDrawUserbynum(num);
        string type = "";
        if (model1.MyGoodsType == 0) { type = "酷币"; }
        if (model1.MyGoodsType == 1) { type = "实物"; }
        if (model1.MyGoodsType == 2) { type = "道具"; }
        if (model1.MyGoodsType == 3) { type = "属性"; }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>我的奖品详细信息：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //是否为进行中商品   
        if (model1.MyGoodsStatue == 0 || model1.MyGoodsStatue == 4)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品还没有确认信息，为了能使你及时领取奖品，请确认信息");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (model1.MyGoodsStatue == 3)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品已经确认信息，奖品很快就能送到你手中，请耐心等待...");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (model1.MyGoodsStatue == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品正在发送中，请耐心等待...<br />");
            builder.Append("【快递信息】<br />");
            builder.Append("快递公司：<h style=\"color:red\">" + model.Express + "<br /></h>");
            builder.Append("快递单号：<h style=\"color:red\">" + model.Numbers + "<br /></h>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (model1.MyGoodsStatue == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品已经送达，谢谢回馈");
            builder.Append(Out.Tab("</div>", "<br />"));

            try
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("【快递信息】<br />");
                builder.Append("快递公司：" + model.Express + "<br />");
                builder.Append("快递单号：" + model.Numbers + "<br />");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            catch { }
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("奖 品 ID：" + num + "<br />");
        if (model1.MyGoodsType == 0)
        {
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(counts)) + " " + "<br />");
            builder.Append("奖品名称：" + model1.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "<br />");
            builder.Append("酷币价值：" + model1.MyGoodsValue + " " + "<br />");
        }
        if (model1.MyGoodsType == 1)
        {
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(counts)) + " " + "<br />");
            builder.Append("奖品名称：" + model1.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "<br />");
            //builder.Append("奖品价值：" + model.MyGoodsValue + " " + "<br />");
        }
        if (model1.MyGoodsType == 2)
        {
            string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(counts)) + " " + "<br />");
            builder.Append("奖品名称：" + model1.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "(" + GN + ")" + "<br />");
            //builder.Append("道具价值：" + model.MyGoodsValue + " " + "<br />");
        }
        if (model1.MyGoodsType == 3)
        {
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(counts)) + " " + "<br />");
            builder.Append("奖品名称：" + model1.MyGoods + "+" + model1.MyGoodsValue + " <br />");
            builder.Append("奖品类型：" + type + "<br />");
        }
        builder.Append(" 奖品个数：" + model1.MyGoodsNum + "");

        if (model1.MyGoodsImg.ToString() != "0" && model1.MyGoodsImg.ToString() != "100" && model1.MyGoodsImg.ToString() != "5" && model1.MyGoodsImg.ToString() != "10" && model1.MyGoodsImg.ToString() != "1")
        {
            builder.Append("<br/>" + "图片描述：");
            builder.Append("<br/>");
            string[] imgNum = model1.MyGoodsImg.Split(',');
            // foreach (string n in imgNum)
            for (int c = 0; c < imgNum.Length - 1; c++)
            {
                if (model1.MyGoodsType == 2)
                {
                    builder.Append("&nbsp;&nbsp;&nbsp;" + "<img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/>" + "&nbsp;&nbsp;");
                }
                else
                {
                    if (img != 0)
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;id=" + counts + "&amp;num=" + num + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;id=" + counts + "&amp;num=" + num + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");

                }
            }
        }
        builder.Append("<br/>" + "奖品描述：" + Out.SysUBB(model1.Explain) + "<br/>");


        BCW.Draw.Model.DrawUser ontime = new BCW.Draw.BLL.DrawUser().GetOnTimebynum(num);
        BCW.Draw.Model.DrawUser intime = new BCW.Draw.BLL.DrawUser().GetInTimebynum(num);
        string statue = null;
        if (model1.MyGoodsStatue == 0) { statue = "待确认信息"; }
        if (model1.MyGoodsStatue == 3) { statue = "奖品准备中"; }
        if (model1.MyGoodsStatue == 1) { statue = "奖品发货中"; }
        if (model1.MyGoodsStatue == 2) { statue = "奖品已送达"; }

        if (model1.R < 101)
        {
            builder.Append("中奖时间：" + Convert.ToDateTime(ontime.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        }
        else
        {
            builder.Append("兑换时间：" + Convert.ToDateTime(ontime.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        }
        if (model1.MyGoodsStatue == 2)
        {
            if (model1.MyGoodsValue == 1)
            {
                builder.Append("收货时间：" + Convert.ToDateTime(intime.InTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
            }
            else
            {
                builder.Append("兑奖时间：" + Convert.ToDateTime(intime.InTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
            }
        }
        builder.Append("奖品状态：" + statue + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (model1.MyGoodsStatue == 0 || model1.MyGoodsStatue == 4)
        {
            string queren = string.Empty;
            if (model1.MyGoodsType == 1)
            {
                queren = "确认个人信息";
            }
            else
            {
                queren = "确认兑奖";
            }
            builder.Append("<br /><a href=\"" + Utils.getUrl("draw.aspx?act=message&amp;id=" + counts + "&amp;num=" + num + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + queren + "</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        if (model1.MyGoodsStatue == 1 && model1.MyGoodsType == 1)
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定收货")
            {

                string MyGoodsStatue = Utils.GetRequest("MyGoodsStatue", "all", 2, @"^[0-9]\d*$", "奖品状态填写错误");

                try
                {
                    model1.MyGoodsStatue = Convert.ToInt32(MyGoodsStatue);
                    new BCW.Draw.BLL.DrawUser().UpdateMyGoodsStatuebynum(num, 2);
                    //new BCW.Draw.BLL.DrawBox().UpdateStatue(counts, 2);//更新box表的奖品的状态
                    new BCW.Draw.BLL.DrawUser().UpdateIntimebynum(num, DateTime.Now);//更新user表的收货时间
                    //new BCW.Draw.BLL.DrawBox().UpdateReceiveTime(counts,DateTime.Now);

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改成功!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin") + "\"><br />返回我的奖品</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));

                }
                catch
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改失败!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;counts=" + counts + "&amp;num=" + num + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }

            }
            else
            {
                //BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(id);
                builder.Append(Out.Tab("</div>", ""));
                string Text = "奖品状态:/,";
                string Name = "MyGoodsStatue";
                string Type = "select";
                string Valu = "" + model.MyGoodsStatue + "'" + Utils.getPage(0) + "";
                string Empt = "2|已送达";
                string Idea = "/";
                string Othe = "确定收货,draw.aspx?act=myrealjiangpin&amp;counts=" + counts + "&amp;num=" + num + ",post,1,red";
                builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));
            }
        }

        // builder.Append(Out.Tab("<div>", Out.Hr()));
        //// builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin&amp;ptype=" + ptype + "") + "\">返回上级</a>");
        // builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //确认个人信息
    private void MessagePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int type = int.Parse(Utils.GetRequest("MyGoodsType", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 2, @"^[0-9]\d*$", "8"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;确认个人信息");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>确认个人信息:</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int MyGoodsType = new BCW.Draw.BLL.DrawUser().GetMyGoodsTypebynum(num);
        int Statue = new BCW.Draw.BLL.DrawUser().GetMyStatuebynum(num);

        if (Statue == 2 || Statue == 3)
        {
            Utils.Error("已经兑奖成功，请不要重复操作...", Utils.getUrl("draw.aspx?act=myjiangpin"));
        }
        else
        {
            if (MyGoodsType == 1)
            {

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("请填写相关信息，以便奖品能够发送达你的手上");
                builder.Append(Out.Tab("</div>", "<br />"));
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                ub xml = new ub();
                //string xmlPath = "/Controls/Dawnlife.xml";
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                if (Utils.ToSChinese(ac) == "确定信息")
                {
                    string Address = Utils.GetRequest("Address", "post", 2, @"^[^\^]{1,200}$", "地址填写出错");
                    string Phone = Utils.GetRequest("Phone", "post", 2, @"^[^\^]{1,200}$", "号码填写错误");
                    string Email = Utils.GetRequest("Email", "post", 1, @"^[^\^]{1,200}$", "邮箱未填写");
                    string RealName = Utils.GetRequest("RealName", "post", 2, @"^[^\^]{1,200}$", "姓名填写错误");
                    //int Statue =int.Parse(Utils.GetRequest("Statue","post",2,@"^[0-9]\d*$","0"));
                    int counts = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));

                    //  int Statue = 3;//确认信息
                    new BCW.Draw.BLL.DrawUser().UpdateMessagebynum(num, Address, Phone, Email, RealName, 3);//根据编号更新信息
                    //new BCW.Draw.BLL.DrawBox().UpdateStatue(counts, 3);

                    //发内线
                    string strLog = "" + GameName + "会员获得实物奖品已经确认信息，请查看并派送..";
                    new BCW.BLL.Guest().Add(0, 10086, new BCW.BLL.User().GetUsName(10086), strLog);

                    Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=myjiangpin&amp;num=" + num + "&amp;ptype=" + 1 + ""), "2");
                }
                else
                {
                    string strText = "姓名:/,收货地址:/,联系方式:/,邮箱:/,";
                    string strName = "RealName,Address,Phone,Email";
                    string strType = "text,text,text,text";
                    string strValu = "" + xml.dss["RealName"] + "'" + xml.dss["Address"] + "'" + xml.dss["Phone"] + "'" + xml.dss["Email"] + "'" + Utils.getPage(0) + "";
                    string strEmpt = "false,false,false,true";
                    string strIdea = "/";
                    string strOthe = "确定信息,draw.aspx?act=message&amp;num=" + num + "&amp;type=" + 1 + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                }
            }
            if (MyGoodsType == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("请确认兑奖，以便酷币及时派送");
                builder.Append(Out.Tab("</div>", ""));
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                ub xml = new ub();
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                string UsID = "" + meid;
                int ID = Convert.ToInt32(UsID);

                int MyGoodsValue = (new BCW.Draw.BLL.DrawUser().GetMyGoodsNumbynum(num)) * (new BCW.Draw.BLL.DrawUser().GetMyGoodsValuebynum(num));
                string MyGoods = new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num);
                DateTime ontime = new BCW.Draw.BLL.DrawUser().Getontimebynum(num);

                long gold = new BCW.BLL.User().GetGold(meid);
                new BCW.BLL.User().UpdateiGold(meid, MyGoodsValue, "" + ontime.ToString("MM月dd日") + GameName + "兑奖|标识ID" + num + "");//根据用户名更新酷币
                new BCW.Draw.BLL.DrawUser().UpdateMessagebynum(num, "", "", "", "", 2);//根据编号更新信息
                new BCW.Draw.BLL.DrawUser().UpdateIntimebynum(num, DateTime.Now); //更新送达时间

                //发内线
                string strLog = "根据你" + ontime.ToString("MM月dd日") + "在" + GameName + "获得" + MyGoods + "×" + new BCW.Draw.BLL.DrawUser().GetMyGoodsNumbynum(num) + "，奖励你" + MyGoodsValue + "酷币" + "[url=/bbs/game/draw.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, ID, new BCW.BLL.User().GetUsName(ID), strLog);

                Utils.Success("游戏设置", "确认信息成功，成功兑奖，系统已经返" + MyGoodsValue + "酷币，正在返回..", Utils.getUrl("draw.aspx?act=myjiangpin&amp;id=" + id + "&amp;ptype=" + 3 + ""), "2");

            }
            if (MyGoodsType == 2)
            {
                int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你即将获得" + new BCW.Draw.BLL.DrawDS().GetGN(id) + "道具" + new BCW.Draw.BLL.DrawDS().GetDS(id) + "<br />");
                builder.Append("请确认兑奖，以便道具及时派送");
                builder.Append(Out.Tab("</div>", ""));
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                ub xml = new ub();
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                string UsID = "" + meid;
                int ID = Convert.ToInt32(UsID);

                int MyGoodsValue = new BCW.Draw.BLL.DrawUser().GetMyGoodsValuebynum(num);
                int number = new BCW.Draw.BLL.DrawUser().GetMyGoodsNumbynum(num);
                string MyGoods = new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num);
                DateTime ontime = new BCW.Draw.BLL.DrawUser().Getontimebynum(num);

                new BCW.Draw.BLL.DrawUser().UpdateMessagebynum(num, "", "", "", "", 2);//根据编号更新信息
                new BCW.Draw.BLL.DrawUser().UpdateIntimebynum(num, DateTime.Now); //更新送达时间

                if (new BCW.Draw.BLL.DrawDS().GetGN(id) == "农场")
                {
                    int DSID = new BCW.Draw.BLL.DrawDS().GetDSID(id);
                    string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
                    string hg = string.Empty;
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(DSID, meid))
                    {
                        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, DSID, 1);//查询化肥数量
                        hg = aaa.name;//名称
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, DSID, 1);
                    }
                    else
                    {
                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(DSID, meid, 1))
                        {
                            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, DSID, 1);//查询化肥数量
                            hg = aaa.name;//名称
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, DSID, 1);
                        }
                        else
                        {
                            BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(DSID);
                            BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                            hg = t.name;//名称
                            w.name = t.name;
                            w.name_id = 0;
                            w.num = number;
                            w.type = 2;
                            w.usid = meid;
                            w.zhonglei = 0;
                            w.huafei_id = DSID;
                            w.picture = t.picture;
                            w.iszengsong = 1;
                            new BCW.farm.BLL.NC_mydaoju().Add(w);
                        }
                    }
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "抽奖获得" + hg + "×" + number + ".", 101);//消息
                }

                //发内线
                string strLog = "根据你" + ontime.ToString("MM月dd日") + "在" + GameName + "获得" + MyGoods + "×" + number + "，奖励你" + new BCW.Draw.BLL.DrawDS().GetGN(id) + "道具" + new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num) + "" + "[url=/bbs/game/draw.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, ID, new BCW.BLL.User().GetUsName(ID), strLog);

                Utils.Success("游戏兑奖", "确认信息成功，成功兑奖，正在返回..", Utils.getUrl("draw.aspx?act=myjiangpin&amp;id=" + id + "&amp;ptype=" + 3 + ""), "2");

            }
            if (MyGoodsType == 3)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你即将获得属性" + new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num) + "+" + new BCW.Draw.BLL.DrawUser().GetMyGoodsValuebynum(num) + "<br />");
                builder.Append("请确认兑奖，以便属性及时派送");
                builder.Append(Out.Tab("</div>", ""));
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                ub xml = new ub();
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                string UsID = "" + meid;
                int ID = Convert.ToInt32(UsID);

                int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
                int MyGoodsValue = (new BCW.Draw.BLL.DrawUser().GetMyGoodsValuebynum(num)) * (new BCW.Draw.BLL.DrawUser().GetMyGoodsNumbynum(num));
                string MyGoods = new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num);
                DateTime ontime = new BCW.Draw.BLL.DrawUser().Getontimebynum(num);

                new BCW.Draw.BLL.DrawUser().UpdateIntimebynum(num, DateTime.Now); //更新送达时间

                new BCW.Draw.BLL.DrawUser().UpdateMessagebynum(num, "", "", "", "", 2);//根据编号更新信息

                if (MyGoods == "体力")
                {
                    //奖魅力等属性
                    string mp = new BCW.BLL.User().GetParas(meid);
                    string[] temp = mp.Split("|".ToCharArray());
                    int Tl = Utils.ParseInt(temp[1]) + MyGoodsValue;
                    int Ml = Utils.ParseInt(temp[2]);
                    int Zh = Utils.ParseInt(temp[3]);
                    int Ww = Utils.ParseInt(temp[4]);
                    int Xe = Utils.ParseInt(temp[5]);
                    string sParas = new BCW.BLL.User().GetParas(meid);
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
                    new BCW.BLL.User().UpdateParas(meid, sParas);
                }
                if (MyGoods == "魅力")
                {
                    //奖魅力等属性
                    string mp = new BCW.BLL.User().GetParas(meid);
                    string[] temp = mp.Split("|".ToCharArray());
                    long Score = Convert.ToInt64(Utils.ParseInt(temp[0]));
                    int Tl = Utils.ParseInt(temp[1]);
                    int Ml = Utils.ParseInt(temp[2]) + MyGoodsValue;
                    int Zh = Utils.ParseInt(temp[3]);
                    int Ww = Utils.ParseInt(temp[4]);
                    int Xe = Utils.ParseInt(temp[5]);
                    string sParas = new BCW.BLL.User().GetParas(meid);
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
                    new BCW.BLL.User().UpdateParas(meid, sParas);
                }
                if (MyGoods == "智慧")
                {
                    //奖魅力等属性
                    string mp = new BCW.BLL.User().GetParas(meid);
                    string[] temp = mp.Split("|".ToCharArray());
                    long Score = Convert.ToInt64(Utils.ParseInt(temp[0]));
                    int Tl = Utils.ParseInt(temp[1]);
                    int Ml = Utils.ParseInt(temp[2]);
                    int Zh = Utils.ParseInt(temp[3]) + MyGoodsValue;
                    int Ww = Utils.ParseInt(temp[4]);
                    int Xe = Utils.ParseInt(temp[5]);
                    string sParas = new BCW.BLL.User().GetParas(meid);
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
                    new BCW.BLL.User().UpdateParas(meid, sParas);
                }
                if (MyGoods == "威望")
                {
                    //奖魅力等属性
                    string mp = new BCW.BLL.User().GetParas(meid);
                    string[] temp = mp.Split("|".ToCharArray());
                    long Score = Convert.ToInt64(Utils.ParseInt(temp[0]));
                    int Tl = Utils.ParseInt(temp[1]);
                    int Ml = Utils.ParseInt(temp[2]);
                    int Zh = Utils.ParseInt(temp[3]);
                    int Ww = Utils.ParseInt(temp[4]) + MyGoodsValue;
                    int Xe = Utils.ParseInt(temp[5]);
                    string sParas = new BCW.BLL.User().GetParas(meid);
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
                    new BCW.BLL.User().UpdateParas(meid, sParas);
                }
                if (MyGoods == "邪恶")
                {
                    //奖魅力等属性
                    string mp = new BCW.BLL.User().GetParas(meid);
                    string[] temp = mp.Split("|".ToCharArray());
                    long Score = Convert.ToInt64(Utils.ParseInt(temp[0]));
                    int Tl = Utils.ParseInt(temp[1]);
                    int Ml = Utils.ParseInt(temp[2]);
                    int Zh = Utils.ParseInt(temp[3]);
                    int Ww = Utils.ParseInt(temp[4]);
                    int Xe = Utils.ParseInt(temp[5]) + (-MyGoodsValue);
                    string sParas = new BCW.BLL.User().GetParas(meid);
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
                    new BCW.BLL.User().UpdateParas(meid, sParas);
                }

                //发内线
                string strLog = "根据你" + ontime.ToString("MM月dd日") + "在" + GameName + "获得的" + MyGoods + "(+" + new BCW.Draw.BLL.DrawUser().GetMyGoodsValuebynum(num) + ")" + "×" + new BCW.Draw.BLL.DrawUser().GetMyGoodsNumbynum(num) + "，奖励你属性" + new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num) + "(+" + MyGoodsValue + ")" + "[url=/bbs/game/draw.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, ID, new BCW.BLL.User().GetUsName(ID), strLog);

                Utils.Success("游戏兑奖", "确认信息成功，成功兑奖，正在返回..", Utils.getUrl("draw.aspx?act=myjiangpin&amp;id=" + id + "&amp;ptype=" + 3 + ""), "2");

            }
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("game.aspx&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //联系客服（奖品兑换）
    private void KefuPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;奖品兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>奖品兑奖：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("中奖如果不懂怎样兑奖请查看规则说明 ");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=rule") + "\">规则说明</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">未确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=kefu&amp;ptype=1") + "\">未确认信息</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">已确认信息" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=kefu&amp;ptype=2") + "\">已确认信息</a>" + "");

        //if (ptype== 3)
        //    builder.Append("<b style=\"color:black\">已发货" + "</b>" + "|");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=kefu&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">已发货</a>" + "|");
        //if (ptype == 4)
        //    builder.Append("<b style=\"color:black\">已送达" + "</b>" + "");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=kefu&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">已送达</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件
        if (ptype == 1)
        {
            strWhere = "UsID=" + meid + " and( MyGoodsStatue='0' or MyGoodsStatue='4')";
            strOrder = "Num Desc";
        }
        if (ptype == 2)
        {
            strWhere = "UsID=" + meid + " and MyGoodsStatue='3'";
            strOrder = "Num Desc";
        }
        //if (ptype == 3)
        //{
        //    strWhere = "UsID=" + meid + " and MyGoodsStatue='1'";
        //    strOrder = "Num Desc";
        //}
        //if (ptype == 4)
        //{
        //    strWhere = "UsID=" + meid + " and MyGoodsStatue='2'";
        //    strOrder = "Num Desc";
        //}

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listdrawuser = new BCW.Draw.BLL.DrawUser().GetDrawUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdrawuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listdrawuser)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;

                if (n.R != 888)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "抽中奖品" + n.MyGoods + ".<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin1&amp;ptype=" + ptype + "&amp;counts=" + n.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>";
                }
                else
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "兑换奖品" + n.MyGoods + ".<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin1&amp;ptype=" + ptype + "&amp;counts=" + n.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>";
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("draw.aspx?act=kefu&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页

            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //我的奖品具体信息1
    private void MyRealJiangpin1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("Draw.aspx?act=kefu&amp;backurl=" + Utils.PostPage(1) + "") + "\">联系客服</a>&gt;我的奖品详细信息");
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.Draw.Model.DrawUser model = new BCW.Draw.BLL.DrawUser().GetDrawUserbyCounts(counts);
        string type = "";
        if (model.MyGoodsType == 0) { type = "酷币"; }
        if (model.MyGoodsType == 1) { type = "实物"; }
        if (model.MyGoodsType == 2) { type = "道具"; }
        if (model.MyGoodsType == 3) { type = "属性"; }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>我的奖品详细信息：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //是否为进行中商品   
        if (model.MyGoodsStatue == 0 || model.MyGoodsStatue == 4)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品还没有确认信息，为了能使你及时领取奖品，请确认信息");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (model.MyGoodsStatue == 3)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品已经确认信息，奖品很快就能送到你手中，请耐心等待...");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (model.MyGoodsStatue == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品正在发送中，请耐心等待...<br />");
            builder.Append("【快递信息】<br />");
            builder.Append("快递公司：<h style=\"color:red\">" + model.Express + "<br /></h>");
            builder.Append("快递单号：<h style=\"color:red\">" + model.Numbers + "<br /></h>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (model.MyGoodsStatue == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的奖品已经送达，谢谢回馈");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("奖品编号：" + model.GoodsCounts + "<br />");
        if (model.MyGoodsType == 0)
        {
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawTen().GetRank(model.GoodsCounts)) + " " + "<br />");
            builder.Append("奖品名称：" + model.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "<br />");
            builder.Append("酷币价值：" + model.MyGoodsValue + " " + "<br />");
        }
        if (model.MyGoodsType == 1)
        {
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawTen().GetRank(model.GoodsCounts)) + " " + "<br />");
            builder.Append("奖品名称：" + model.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "<br />");
            //builder.Append("奖品价值：" + model.MyGoodsValue + " " + "<br />");
        }
        if (model.MyGoodsType == 2)
        {
            string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawTen().GetRank(model.GoodsCounts)) + " " + "<br />");
            builder.Append("奖品名称：" + model.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "(" + GN + ")" + "<br />");
            //builder.Append("道具价值：" + model.MyGoodsValue + " " + "<br />");
        }
        if (model.MyGoodsType == 3)
        {
            builder.Append("奖品等级：" + GetRank(new BCW.Draw.BLL.DrawTen().GetRank(model.GoodsCounts)) + " " + "<br />");
            builder.Append("奖品名称：" + model.MyGoods + "+" + model.MyGoodsValue + " <br />");
            builder.Append("奖品类型：" + type + "<br />");
        }
        builder.Append(" 奖品个数：" + model.MyGoodsNum + "");

        if (model.MyGoodsImg.ToString() != "0" && model.MyGoodsImg.ToString() != "100" && model.MyGoodsImg.ToString() != "5" && model.MyGoodsImg.ToString() != "10" && model.MyGoodsImg.ToString() != "1")
        {
            builder.Append("<br/>" + "图片描述：");
            builder.Append("<br/>");
            string[] imgNum = model.MyGoodsImg.Split(',');
            // foreach (string n in imgNum)
            for (int c = 0; c < imgNum.Length - 1; c++)
            {
                if (model.MyGoodsType == 2)
                {
                    builder.Append("&nbsp;&nbsp;&nbsp;" + "<img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/>" + "&nbsp;&nbsp;");
                }
                else
                {
                    if (img != 0)
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;id=" + counts + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;id=" + counts + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");

                }
            }
        }
        builder.Append("<br/>" + "奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");

        BCW.Draw.Model.DrawUser ontime = new BCW.Draw.BLL.DrawUser().GetOnTime(counts);
        string statue = null;
        if (model.MyGoodsStatue == 0) { statue = "待确认信息"; }
        if (model.MyGoodsStatue == 3) { statue = "奖品准备中"; }
        if (model.MyGoodsStatue == 1) { statue = "奖品发货中"; }
        if (model.MyGoodsStatue == 2) { statue = "奖品已送达"; }

        builder.Append("中奖时间：" + Convert.ToDateTime(ontime.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append("奖品状态：" + statue + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (model.MyGoodsStatue == 0 || model.MyGoodsStatue == 4)
        {
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=message&amp;id=" + counts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">确认个人信息</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        if (model.MyGoodsStatue == 1)
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定收货")
            {

                string MyGoodsStatue = Utils.GetRequest("MyGoodsStatue", "all", 2, @"^[0-9]\d*$", "奖品状态填写错误");

                try
                {
                    model.MyGoodsStatue = Convert.ToInt32(MyGoodsStatue);
                    new BCW.Draw.BLL.DrawUser().UpdateMyGoodsStatue(counts, model.MyGoodsStatue);
                    //new BCW.Draw.BLL.DrawBox().UpdateStatue(counts, model.MyGoodsStatue);

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改成功!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=myjiangpin&amp;backurl=" + Utils.PostPage(1) + "") + "\"><br />返回奖品派送</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));

                }
                catch
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改失败!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=myrealjiangpin&amp;counts=" + counts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }

            }
            else
            {
                //BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(id);
                builder.Append(Out.Tab("</div>", ""));
                string Text = "奖品状态:/,";
                string Name = "MyGoodsStatue";
                string Type = "select";
                string Valu = "" + model.MyGoodsStatue + "'" + Utils.getPage(0) + "";
                string Empt = "1|派送中|2|已送达";
                string Idea = "/";
                string Othe = "确定收货|reset,draw.aspx?act=myrealjiangpin&amp;counts=" + counts + ",post,1,red|blue";
                builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));

            }
        }

        //builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=kefu&amp;ptype=" + ptype + "") + "\">返回上级</a>");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //动态
    private void TrendsPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;游戏动态");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>动态：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = " MyGoodsStatue!='88'";
        strOrder = "Num desc";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        IList<BCW.Draw.Model.DrawUser> GetDrawUsers1 = new BCW.Draw.BLL.DrawUser().GetDrawUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (GetDrawUsers1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser model1 in GetDrawUsers1)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                int rrank = 0;
                try
                {
                    rrank = new BCW.Draw.BLL.DrawBox().GetRank(model1.GoodsCounts);
                }
                catch { }

                if (model1.R != 888)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "抽中" + GetRank(rrank) + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + model1.GoodsCounts + "&amp;num=" + model1.Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + model1.MyGoods + "</a>" + "|" + Convert.ToDateTime(model1.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "";
                }
                else
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "兑换奖品" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + model1.GoodsCounts + "&amp;num=" + model1.Num + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + model1.MyGoods + "</a>" + "|" + Convert.ToDateTime(model1.OnTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("draw.aspx?act=trends&amp;ptype=" + ptype + "&amp;id=" + model1.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //动态奖品信息
    private void XinxiPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        string Jifen = ub.GetSub("drawJifen", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "</a>&gt;奖品信息");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>奖品信息</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            DateTime nowTime = DateTime.Now;

            BCW.Draw.Model.DrawBox model1 = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);
            BCW.Draw.Model.DrawUser model = new BCW.Draw.BLL.DrawUser().GetDrawUserbynum(num);
            string type = "";
            if (model.MyGoodsType == 0) { type = "酷币"; }
            if (model.MyGoodsType == 1) { type = "实物"; }
            if (model.MyGoodsType == 2) { type = "道具"; }
            if (model.MyGoodsType == 3) { type = "属性"; }


            builder.Append("奖 品 ID：" + model.Num + "<br />");
            if (model.MyGoodsType == 0)
            {
                if (model1.rank == 1000)
                {
                    builder.Append(" 奖品等级：" + GetRank(model1.rank) + " " + "<br />");
                }
                else
                {
                    builder.Append(" 奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(model.GoodsCounts)) + " " + "<br />");
                }
                builder.Append("奖品名称：" + model.MyGoods + "<br />");
                builder.Append("奖品类型：" + type + "<br />");
                builder.Append("酷币价值：" + model.MyGoodsValue + " " + "<br />");
            }
            if (model.MyGoodsType == 1)
            {
                if (model1.rank == 1000)
                {
                    builder.Append(" 奖品等级：" + GetRank(model1.rank) + " " + "<br />");
                }
                else
                {
                    builder.Append(" 奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(model.GoodsCounts)) + " " + "<br />");
                }
                builder.Append("奖品名称：" + model.MyGoods + "<br />");
                builder.Append("奖品类型：" + type + "<br />");
                //builder.Append("奖品价值：" + model.MyGoodsValue + " " + model. + "<br />");
            }
            if (model.MyGoodsType == 2)
            {
                string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
                if (model1.rank == 1000)
                {
                    builder.Append(" 奖品等级：" + GetRank(model1.rank) + " " + "<br />");
                }
                else
                {
                    builder.Append(" 奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(model.GoodsCounts)) + " " + "<br />");
                }
                builder.Append("奖品名称：" + model.MyGoods + "<br />");
                builder.Append("奖品类型：" + type + "(" + GN + ")" + "<br />");
                //builder.Append("道具价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
            }
            if (model.MyGoodsType == 3)
            {
                if (model1.rank == 1000)
                {
                    builder.Append(" 奖品等级：" + GetRank(model1.rank) + " " + "<br />");
                }
                else
                {
                    builder.Append(" 奖品等级：" + GetRank(new BCW.Draw.BLL.DrawBox().GetRank(model.GoodsCounts)) + " " + "<br />");
                }
                builder.Append("奖品名称：" + model.MyGoods + "+" + model.MyGoodsValue + " <br />");
                builder.Append("奖品类型：" + type + "<br />");
            }
            if (model1.points == 0)
                builder.Append("兑换" + drawJifen + "：" + "不可兑换" + "<br />奖品个数：" + model.MyGoodsNum + "");
            else
                builder.Append("兑换" + drawJifen + "：" + model1.points + "<br />奖品个数：" + model.MyGoodsNum + "");

            if (model1.GoodsImg.ToString() != "0" && model1.GoodsImg.ToString() != "100" && model1.GoodsImg.ToString() != "5" && model1.GoodsImg.ToString() != "10" && model1.GoodsImg.ToString() != "1")
            {
                builder.Append("<br/>" + "图片描述：");
                builder.Append("<br/>");
                string[] imgNum = model1.GoodsImg.Split(',');
                // foreach (string n in imgNum)
                for (int c = 0; c < imgNum.Length - 1; c++)
                {
                    if (model1.GoodsType == 2)
                    {
                        builder.Append("" + "<img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/>" + "&nbsp;&nbsp;");
                    }
                    else
                    {
                        if (img != 0)
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + id + "&amp;num=" + num + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                        else
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=xinxi&amp;id=" + id + "&amp;num=" + num + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");

                    }
                }
            }
            builder.Append("<br/>" + "奖品描述：" + Out.SysUBB(model.Explain) + "");

        }
        catch
        {
            //  builder.Append("就这么多奖品了，赶快去试试运气吧...<br/>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        //  builder.Append("<a href=\"" + Utils.getPage("game.aspx&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //奖级
    public string GetRank(int i)
    {
        string str = string.Empty;
        string sssss = string.Empty;
        if (i < 20)
        {
            if (i == 0) { sssss = "一等奖"; }
            if (i == 1) { sssss = "二等奖"; }
            if (i == 2) { sssss = "三等奖"; }
            if (i == 3) { sssss = "四等奖"; }
            if (i == 4) { sssss = "五等奖"; }
            if (i == 5) { sssss = "六等奖"; }
            if (i == 6) { sssss = "七等奖"; }
            if (i == 7) { sssss = "八等奖"; }
            if (i == 8) { sssss = "九等奖"; }
            if (i == 9) { sssss = "十等奖"; }
            if (i == 10) { sssss = "十一等奖"; }
            if (i == 11) { sssss = "十二等奖"; }
            if (i == 12) { sssss = "十三等奖"; }
            if (i == 13) { sssss = "十四等奖"; }
            if (i == 14) { sssss = "十五等奖"; }
            if (i == 15) { sssss = "十六等奖"; }
            if (i == 16) { sssss = "十七等奖"; }
            if (i == 17) { sssss = "十八等奖"; }
            if (i == 18) { sssss = "十九等奖"; }
            if (i == 19) { sssss = "二十等奖"; }
            if (i == 1000) { sssss = "兑换奖品"; }
            str = sssss;
            return str;
        }
        else
        {
            if (i == 1000) { str = "兑换奖品"; }
            else
            {
                str = (i + 1) + "等奖";
            }
            return str;
        }
    }
    private bool Isbz()
    {
        return true;

        //if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        //    return true;
        //else
        //    return false;
    }
    private void ExpirDelPage()
    {
        DataSet ds = new BCW.BLL.Game.Brag().GetList("ID", "StopTime<'" + DateTime.Now + "' and State=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                new BCW.BLL.Game.Brag().UpdateState(id, 3);
            }
        }
    }

    #region 随机值计算方法
    public class RandomController
    {
        //待随机抽取数据集合
        public List<char> datas1 = new List<char>(new char[] { 'A' });
        public List<char> datas2 = new List<char>(new char[] { 'A', 'B' });
        public List<char> datas3 = new List<char>(new char[] { 'A', 'B', 'C' });
        public List<char> datas4 = new List<char>(new char[] { 'A', 'B', 'C', 'D' });
        public List<char> datas5 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E' });
        public List<char> datas6 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F' });
        public List<char> datas7 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });
        public List<char> datas8 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' });
        public List<char> datas9 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' });
        public List<char> datas10 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' });
        public List<char> datas11 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K' });
        public List<char> datas12 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' });
        public List<char> datas13 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' });
        public List<char> datas14 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N' });
        public List<char> datas15 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' });
        public List<char> datas16 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P' });
        public List<char> datas17 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q' });
        public List<char> datas18 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' });
        public List<char> datas19 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S' });
        public List<char> datas20 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T' });
        public List<char> datas21 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U' });
        public List<char> datas22 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V' });
        public List<char> datas23 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' });
        public List<char> datas24 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X' });
        public List<char> datas25 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' });
        public List<char> datas26 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' });

        static string[] Rq = ub.GetSub("Rq", "/Controls/draw.xml").Split('#');
        //权值
        static ushort a = Convert.ToUInt16(Rq[0]);
        static ushort[] temp = new ushort[] {
           Convert.ToUInt16(Rq[0]), Convert.ToUInt16(Rq[1]), Convert.ToUInt16(Rq[2]), Convert.ToUInt16(Rq[3]), Convert.ToUInt16(Rq[4]),
           Convert.ToUInt16(Rq[5]), Convert.ToUInt16(Rq[6]), Convert.ToUInt16(Rq[7]), Convert.ToUInt16(Rq[8]), Convert.ToUInt16(Rq[9]),
           Convert.ToUInt16(Rq[10]), Convert.ToUInt16(Rq[11]), Convert.ToUInt16(Rq[12]), Convert.ToUInt16(Rq[13]), Convert.ToUInt16(Rq[14]),
           Convert.ToUInt16(Rq[15]), Convert.ToUInt16(Rq[16]), Convert.ToUInt16(Rq[17]), Convert.ToUInt16(Rq[18]), Convert.ToUInt16(Rq[19]),
           Convert.ToUInt16(Rq[20]), Convert.ToUInt16(Rq[21]), Convert.ToUInt16(Rq[22]), Convert.ToUInt16(Rq[23]), Convert.ToUInt16(Rq[24]) };
        public List<ushort> weights = new List<ushort>(temp);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count">随机抽取个数</param>

        public RandomController(ushort count)
        {
            int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
            if (count > jiangji)
                throw new Exception("抽取个数不能超过数据集合大小！！");
            _Count = count;
        }

        /// <summary>
        /// 随机抽取
        /// </summary>
        /// <param name="rand">随机数生成器</param>
        /// <returns></returns>
        public char[] RandomExtract(Random rand)
        {
            int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
            char datas = '0';
            if (jiangji == 1) { datas = datas1[rand.Next((jiangji))]; }
            if (jiangji == 2) { datas = datas2[rand.Next((jiangji))]; }
            if (jiangji == 3) { datas = datas3[rand.Next((jiangji))]; }
            if (jiangji == 4) { datas = datas4[rand.Next((jiangji))]; }
            if (jiangji == 5) { datas = datas5[rand.Next((jiangji))]; }
            if (jiangji == 6) { datas = datas6[rand.Next((jiangji))]; }
            if (jiangji == 7) { datas = datas7[rand.Next((jiangji))]; }
            if (jiangji == 8) { datas = datas8[rand.Next((jiangji))]; }
            if (jiangji == 9) { datas = datas9[rand.Next((jiangji))]; }
            if (jiangji == 10) { datas = datas10[rand.Next((jiangji))]; }
            if (jiangji == 11) { datas = datas11[rand.Next((jiangji))]; }
            if (jiangji == 12) { datas = datas12[rand.Next((jiangji))]; }
            if (jiangji == 13) { datas = datas13[rand.Next((jiangji))]; }
            if (jiangji == 14) { datas = datas14[rand.Next((jiangji))]; }
            if (jiangji == 15) { datas = datas15[rand.Next((jiangji))]; }
            if (jiangji == 16) { datas = datas16[rand.Next((jiangji))]; }
            if (jiangji == 17) { datas = datas17[rand.Next((jiangji))]; }
            if (jiangji == 18) { datas = datas18[rand.Next((jiangji))]; }
            if (jiangji == 19) { datas = datas19[rand.Next((jiangji))]; }
            if (jiangji == 20) { datas = datas20[rand.Next((jiangji))]; }
            if (jiangji == 21) { datas = datas21[rand.Next((jiangji))]; }

            List<char> result = new List<char>();
            if (rand != null)
            {
                for (int i = Count; i > 0; )
                {
                    char item = datas;
                    if (result.Contains(item))
                        continue;
                    else
                    {
                        result.Add(item);
                        i--;
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 随机抽取
        /// </summary>
        /// <param name="rand">随机数生成器</param>
        /// <returns></returns>
        public char[] ControllerRandomExtract(Random rand)
        {
            int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
            string[] Rq = ub.GetSub("Rq", "/Controls/draw.xml").Split('#');
            List<char> result = new List<char>();
            if (rand != null)
            {
                //临时变量
                Dictionary<char, int> dict = new Dictionary<char, int>(jiangji);
                if (jiangji == 1)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas1.Count - 1; i >= 0; i--)
                    {
                        dict.Add(datas1[i], rand.Next(100) * weights[i]);
                    }
                }
                if (jiangji == 2)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas2.Count - 1; i >= 0; i--)
                    {
                        dict.Add(datas2[i], rand.Next(100) * weights[i]);
                    }
                }
                if (jiangji == 3)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas3.Count - 1; i >= 0; i--)
                    {
                        dict.Add(datas3[i], rand.Next(100) * weights[i]);
                    }
                }
                if (jiangji == 4)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas4.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas4[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 5)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas5.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas5[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 6)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas6.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas6[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 7)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas7.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas7[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 8)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas8.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas8[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 9)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas9.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas9[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 10)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas10.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas10[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 11)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas11.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas11[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 12)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas12.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas12[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 13)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas13.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas13[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 14)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas14.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas14[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 15)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas15.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas15[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 16)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas16.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas16[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 17)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas17.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas17[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 18)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas18.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas18[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 19)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas19.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas19[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 20)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas20.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas20[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 21)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas21.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas21[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 22)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas22.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas22[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 23)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas23.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas23[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 24)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas24.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas24[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 25)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas25.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas25[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 26)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas26.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas26[i], rand.Next(100) * weights[i]);


                    }
                }
                //排序

                List<KeyValuePair<char, int>> listDict = SortByValue(dict);

                //拷贝抽取权值最大的前Count项

                foreach (KeyValuePair<char, int> kvp in listDict.GetRange(0, Count))
                {

                    result.Add(kvp.Key);

                }

            }

            return result.ToArray();

        }

        /// <summary>

        /// 排序集合

        /// </summary>

        /// <param name="dict"></param>

        /// <returns></returns>

        private List<KeyValuePair<char, int>> SortByValue(Dictionary<char, int> dict)
        {

            List<KeyValuePair<char, int>> list = new List<KeyValuePair<char, int>>();

            if (dict != null)
            {

                list.AddRange(dict);



                list.Sort(

                  delegate(KeyValuePair<char, int> kvp1, KeyValuePair<char, int> kvp2)
                  {

                      return kvp2.Value - kvp1.Value;

                  });

            }

            return list;

        }


        private int _Count;

        /// <summary>

        /// 随机抽取个数

        /// </summary>

        public int Count
        {

            get
            {

                return _Count;

            }

            set
            {

                _Count = value;

            }

        }

    }
    #endregion
}