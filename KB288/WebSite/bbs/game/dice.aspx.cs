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

/// <summary>
/// 修改挖宝消费记录提示
/// 
/// 黄国军 20160312
/// </summary>
/// <summary>
/// 蒙宗将 20160513 抽奖值生成
/// 
/// 邵广林 20160617 动态添加usid
/// 
/// 姚志光 20160621 活跃抽奖入口控制
/// 
/// /// 蒙宗将 20160822  撤掉抽奖值生成
/// </summary>


public partial class bbs_game_dice : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/dice.xml";

    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = ub.GetSub("DiceName", xmlPath);
        //维护提示
        if (ub.GetSub("DiceStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "case":
                CasePage();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "casepost":
                CasePostPage();
                break;
            case "help":
                HelpPage();
                break;
            case "pay":
                PayPage();
                break;
            case "payok":
                PayOkPage();
                break;
            case "paysave":
                PaySavePage();
                break;
            case "paysave2":
                PaySave2Page();
                break;
            case "mylist":
                MyListPage();
                break;
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("DiceLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(7));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;挖宝");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Notes = ub.GetSub("DiceNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        BCW.Model.Game.Dicelist dice = null;
        //new BCW.User.Game.Dice().DicePage();
        dice = new BCW.BLL.Game.Dicelist().GetDicelist();
        if (IsOpen() == true)
        {
            if (dice.ID == 0)
            {
                //第一局开始
                dice.WinNum = 0;
                dice.WinCount = 0;
                dice.WinPool = 0;
                //开奖周局分钟
                int CycleMin = Utils.ParseInt(ub.GetSub("DiceCycleMin", xmlPath));
                dice.Pool = 0;
                dice.BeginTime = DateTime.Now;
                dice.EndTime = DateTime.Now.AddMinutes(Convert.ToDouble(CycleMin));
                dice.ID = new BCW.BLL.Game.Dicelist().Add(dice);
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (dice.EndTime > DateTime.Now)
            {
                long Sec = DT.DateDiff(dice.EndTime, DateTime.Now, 4);
                builder.Append("第" + dice.ID + "局开奖还有" + Sec + "秒");
            }
            else
            {
                builder.Append("系统正在开奖,请稍后..");
                new BCW.User.Game.Dice().DicePage();
            }

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("本局押注总量:" + Utils.ConvertGold(dice.Pool) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("|" + Utils.ConvertGold(dice.WinPool) + "" + ub.Get("SiteBz2") + "");

            BCW.Model.Game.Dicelist bf = new BCW.BLL.Game.Dicelist().GetDicelistBf(dice.ID);
            if (bf != null)
            {
                builder.Append("<br />上局开出(" + Utils.ConvertSeparated(bf.WinNum.ToString(), 1, ",") + ")");
                if (bf.WinCount == 0)
                    builder.Append("通吃!!!");
                else
                    builder.Append("共<a href=\"" + Utils.getUrl("dice.aspx?act=listview&amp;id=" + bf.ID + "") + "\">" + bf.WinCount + "</a>人次中");

            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("游戏开放时间:" + ub.GetSub("DiceOnTime", xmlPath) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=help") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx") + "\">刷新</a><br />");
        builder.Append("我的<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=1") + "\">未开</a>|<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=2") + "\">历史记录</a>");
        builder.Append("<br />您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("押<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=1") + "\">大小</a>|<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=2") + "\">单双</a>|<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=3") + "\">总和</a>|<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=4") + "\">对子</a>|<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=5") + "\">豹子</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string DiceList = "";
        string[] arrdice = { };
        IList<BCW.Model.Game.Dicelist> listDicelist = new BCW.BLL.Game.Dicelist().GetDicelists(10, "State=1");
        if (listDicelist.Count > 0)
        {
            foreach (BCW.Model.Game.Dicelist n in listDicelist)
            {
                DiceList += " " + n.WinNum;
            }
        }
        if (DiceList != "")
        {
            DiceList = DiceList.Trim();
            DiceList = DiceList.Replace(" ", "#");
            arrdice = DiceList.Split("#".ToCharArray());
        }

        if (DiceList != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            int max = arrdice.Length;
            if (max > 3)
                max = 3;

            for (int i = 0; i < max; i++)
            {
                string[] arrNum = Utils.ArrBySeparated(arrdice[i], 1);
                if (arrNum[0] == arrNum[1] && arrNum[1] == arrNum[2] && arrNum[0] == arrNum[2])
                    builder.Append("豹子" + arrNum[0] + "|");
                else
                    builder.Append("" + arrdice[i] + "|");
            }
            builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=list") + "\">分析</a>");

            builder.Append("|<a href=\"" + Utils.getUrl("dice.aspx?act=top") + "\">排行</a>");
            //大小分析
            string strDx = "";
            for (int i = 0; i < arrdice.Length; i++)
            {
                string[] arrNum = Utils.ArrBySeparated(arrdice[i], 1);
                if (arrNum[0] == arrNum[1] && arrNum[1] == arrNum[2] && arrNum[0] == arrNum[2])
                    strDx += "豹子" + arrNum[0] + "";
                else
                {
                    int sumNum = Convert.ToInt32(arrNum[0]) + Convert.ToInt32(arrNum[1]) + Convert.ToInt32(arrNum[2]);
                    if (sumNum > 10)
                        strDx += "大";
                    else
                        strDx += "小";
                }
            }
            if (!strDx.Equals(""))
                builder.Append("<br />→" + strDx + "→");
            //单双分析

            string strDs = "";
            for (int i = 0; i < arrdice.Length; i++)
            {
                string[] arrNum = Utils.ArrBySeparated(arrdice[i], 1);
                if (arrNum[0] == arrNum[1] && arrNum[1] == arrNum[2] && arrNum[0] == arrNum[2])
                    strDs += "豹子" + arrNum[0] + "";
                else
                {
                    int sumNum = Convert.ToInt32(arrNum[0]) + Convert.ToInt32(arrNum[1]) + Convert.ToInt32(arrNum[2]);
                    if (sumNum % 2 == 0)
                        strDs += "双";
                    else
                        strDs += "单";
                }
            }
            if (!strDs.Equals(""))
                builder.Append("<br />→" + strDs + "→");

            builder.Append(Out.Tab("</div>", ""));
        }
        if (IsOpen() == true)
        {
            //押大小
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("=<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=1") + "\">押大小</a>=1赔" + ub.GetSub("DiceDx", xmlPath) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            strText = ",,,,";
            strName = "paycent11,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'1'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押大,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 1, 1) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 1, 1) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 1, 1) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,";
            strName = "paycent12,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'1'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押小,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 1, 2) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 1, 2) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 1, 2) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            //押单双
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("=<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=2") + "\">押单双</a>=1赔" + ub.GetSub("DiceDs", xmlPath) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            strText = ",,,,";
            strName = "paycent21,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'2'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押单,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 2, 1) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 2, 1) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 2, 1) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,";
            strName = "paycent22,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'2'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押双,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 2, 2) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 2, 2) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 2, 2) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            //押总和
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("=<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=3") + "\">押总和</a>=1赔" + ub.GetSub("DiceSum9012", xmlPath) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            strText = ",,,,";
            strName = "paycent39,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'3'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押9,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, 9) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, 9) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, 9) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,";
            strName = "paycent310,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'3'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押10,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, 10) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, 10) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, 10) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,";
            strName = "paycent311,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'3'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押11,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, 11) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, 11) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, 11) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,";
            strName = "paycent312,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'3'payok'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押12,dice.aspx,post,3,other,|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, 12) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, 12) + "" + ub.Get("SiteBz") + "";
            if (Isbz())
            {
                strOthe += "|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, 12) + "" + ub.Get("SiteBz2") + "";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("=<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=4") + "\">押对子</a>=1赔" + ub.GetSub("DiceDz", xmlPath) + "|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 0) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 0) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 0) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=5") + "\">押豹子</a>=1赔" + ub.GetSub("DiceBz2", xmlPath) + "|共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 0) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 0) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 0) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        // 
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(9, "dice.aspx", 5, 0)));
        //游戏底部Ubb
        string Foot = ub.GetSub("DiceFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-5]$", "选择押注类型错误"));
        string sText = string.Empty;
        if (ptype == 1)
            sText = "押大小";
        else if (ptype == 2)
            sText = "押单双";
        else if (ptype == 3)
            sText = "押总和";
        else if (ptype == 4)
            sText = "押对子";
        else if (ptype == 5)
            sText = "押豹子";

        BCW.Model.Game.Dicelist dice = new BCW.BLL.Game.Dicelist().GetDicelist();
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(7));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>&gt;" + sText + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (dice.EndTime > DateTime.Now)
            builder.Append("本局开奖还有" + DT.DateDiff(dice.EndTime, DateTime.Now, 4) + "秒|");
        else
            builder.Append("系统正在开奖,请稍后|");

        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=pay&amp;ptype=" + ptype + "") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //押大小
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("*最低" + ub.GetSub("DiceSmallPay", xmlPath) + "" + ub.Get("SiteBz") + ",最高" + ub.GetSub("DiceBigPay", xmlPath) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (ptype == 1)
        {
            strText = ",,,";
            strName = "paycent11,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'1'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "押大,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDx", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 1, 1) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 1, 1) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 1, 1) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent12,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'1'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "押小,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDx", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 1, 2) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 1, 2) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 1, 2) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 2)
        {
            strText = ",,,";
            strName = "paycent21,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'2'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "押单,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDs", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 2, 1) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 2, 1) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 2, 1) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent22,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'2'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "押双,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDs", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 2, 2) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 2, 2) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 2, 2) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        else if (ptype == 3)
        {
            strText = ",,,";
            strName = "paycent9012,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'9012'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "9|10|11|12,dice.aspx,post,3,other|other|other|other,|赔率1:" + ub.GetSub("DiceSum9012", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, "9|10|11|12") + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, "9|10|11|12") + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, "9|10|11|12") + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent813,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'813'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "8|13,dice.aspx,post,3,other|other,|赔率1:" + ub.GetSub("DiceSum813", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, "8|13") + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, "8|13") + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, "8|13") + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent714,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'714'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "7|14,dice.aspx,post,3,other|other,|赔率1:" + ub.GetSub("DiceSum714", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, "7|14") + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, "7|14") + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, "7|14") + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent615,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'615'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "6|15,dice.aspx,post,3,other|other,|赔率1:" + ub.GetSub("DiceSum615", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, "6|15") + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, "6|15") + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, "6|15") + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent516,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'516'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "5|16,dice.aspx,post,3,other|other,|赔率1:" + ub.GetSub("DiceSum516", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, "5|16") + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, "5|16") + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, "5|16") + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent417,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'417'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "4|17,dice.aspx,post,3,other|other,|赔率1:" + ub.GetSub("DiceSum417", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 3, "4|17") + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 3, "4|17") + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 3, "4|17") + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 4)
        {
            strText = ",,,";
            strName = "paycent41,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'4'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "两个1,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 1) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 1) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 1) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent42,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'4'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "两个2,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 2) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 2) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 2) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent43,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'4'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "两个3,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 3) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 3) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 3) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent44,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'4'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "两个4,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 4) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 4) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 4) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent45,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'4'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "两个5,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 5) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 5) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 5) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent46,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'4'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "两个6,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceDz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 4, 6) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 4, 6) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 4, 6) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            strText = ",,,";
            strName = "paycent51,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "豹子1,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz2", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 1) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 1) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 1) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent52,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "豹子2,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz2", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 2) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 2) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 2) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent53,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "豹子3,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz2", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 3) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 3) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 3) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent54,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "豹子4,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz2", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 4) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 4) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 4) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent55,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "豹子5,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz2", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 5) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 5) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 5) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent56,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "豹子6,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz2", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 6) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 6) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 6) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,";
            strName = "paycent50,ptype,act";
            strType = "snum,hidden,hidden";
            strValu = "" + ub.GetSub("DiceDtPay", xmlPath) + "'5'payok";
            strEmpt = "false,false,false";
            strIdea = "";
            strOthe = "任意豹子,dice.aspx,post,3,other,|赔率1:" + ub.GetSub("DiceBz", xmlPath) + "";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("共" + new BCW.BLL.Game.Dicepay().GetCount(dice.ID, 5, 0) + "人押" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 0, 5, 0) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
            {
                builder.Append("|" + new BCW.BLL.Game.Dicepay().GetSumBuyCent(dice.ID, 1, 5, 0) + "" + ub.Get("SiteBz2") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 1 || ptype == 2)
        {
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.RHr()));

            string DiceList = "";
            string[] arrdice = { };
            IList<BCW.Model.Game.Dicelist> listDicelist = new BCW.BLL.Game.Dicelist().GetDicelists(10, "State=1");
            if (listDicelist.Count > 0)
            {
                foreach (BCW.Model.Game.Dicelist n in listDicelist)
                {
                    DiceList += " " + n.WinNum;
                }
            }
            if (DiceList != "")
            {
                DiceList = DiceList.Trim();
                DiceList = DiceList.Replace(" ", "#");
                arrdice = DiceList.Split("#".ToCharArray());
            }

            if (DiceList != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                if (ptype == 1)
                {
                    //大小分析
                    string strDx = "";
                    for (int i = 0; i < arrdice.Length; i++)
                    {
                        string aNum = Utils.ConvertSeparated(arrdice[i].ToString(), 1, ",");
                        string[] arrNum = Utils.ArrBySeparated(arrdice[i], 1);
                        if (arrNum[0] == arrNum[1] && arrNum[1] == arrNum[2] && arrNum[0] == arrNum[2])
                            strDx += aNum + "==豹子" + arrNum[0] + "<br />";
                        else
                        {
                            int sumNum = Convert.ToInt32(arrNum[0]) + Convert.ToInt32(arrNum[1]) + Convert.ToInt32(arrNum[2]);
                            if (sumNum > 10)
                                strDx += aNum + "==大<br />";
                            else
                                strDx += aNum + "==小<br />";
                        }
                    }
                    if (!strDx.Equals(""))
                        builder.Append("近10局大小结果<br />" + strDx + "");
                }
                else if (ptype == 2)
                {
                    //单双分析

                    string strDs = "";
                    for (int i = 0; i < arrdice.Length; i++)
                    {
                        string aNum = Utils.ConvertSeparated(arrdice[i].ToString(), 1, ",");
                        string[] arrNum = Utils.ArrBySeparated(arrdice[i], 1);
                        if (arrNum[0] == arrNum[1] && arrNum[1] == arrNum[2] && arrNum[0] == arrNum[2])
                            strDs += aNum + "==豹子" + arrNum[0] + "<br />";
                        else
                        {
                            int sumNum = Convert.ToInt32(arrNum[0]) + Convert.ToInt32(arrNum[1]) + Convert.ToInt32(arrNum[2]);
                            if (sumNum % 2 == 0)
                                strDs += aNum + "==双<br />";
                            else
                                strDs += aNum + "==单<br />";
                        }
                    }
                    if (!strDs.Equals(""))
                        builder.Append("近10局单双结果<br />" + strDs + "");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-5]$|^9012$|^813$|^714$|^615$|^516$|^417$", "选择押注类型错误"));

        long paycent = 0;
        if (info == "")
        {
            //变量传递
            string bName = string.Empty;
            if (ptype <= 5)
                bName = ptype.ToString() + "" + GetiType(ac);
            else
                bName = ptype.ToString();

            if (ptype > 5)
                ptype = 3;
            paycent = int.Parse(Utils.GetRequest("paycent" + bName + "", "post", 2, @"^[1-9]\d*$", "押注额填写错误"));
        }
        else
        {
            paycent = int.Parse(Utils.GetRequest("paycent", "post", 2, @"^[1-9]\d*$", "押注额填写错误"));
        }


        string sText = string.Empty;
        if (ptype == 1)
            sText = "押大小";
        else if (ptype == 2)
            sText = "押单双";
        else if (ptype == 3)
            sText = "押总和";
        else if (ptype == 4)
            sText = "押对子";
        else if (ptype == 5)
            sText = "押豹子";

        BCW.Model.Game.Dicelist dice = new BCW.BLL.Game.Dicelist().GetDicelist();
        builder.Append(Out.Tab("<div>", ""));
        if (dice.EndTime > DateTime.Now)
            builder.Append("本局开奖还有" + DT.DateDiff(dice.EndTime, DateTime.Now, 4) + "秒");
        else
            builder.Append("系统正在开奖,请稍后");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=押注确认=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("押注项目:" + sText + "<br />押" + ac.Replace("押", "") + "，共" + paycent + "<br />");
        if (Isbz())
            builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");
        else
            builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");

        builder.Append("<br />" + ub.Get("SiteBz") + "限" + ub.GetSub("DiceSmallPay", xmlPath) + "-" + Convert.ToInt64(ub.GetSub("DiceBigPay", xmlPath)) + "/" + ub.Get("SiteBz2") + "限" + ub.GetSub("DiceSmallPay2", xmlPath) + "-" + (Convert.ToInt64(ub.GetSub("DiceBigPay2", xmlPath)) * 1) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strName = "paycent,acType,id,ptype,rand,info,act";
        string strValu = "" + paycent + "'" + ac + "'" + dice.ID + "'" + ptype + "'" + new Rand().RandNumer(4) + "'ok'paysave";
        string strOthe = "" + ub.Get("SiteBz") + "押注,dice.aspx,post,0,red";
        builder.Append(Out.wapform(strName, strValu, strOthe));
        builder.Append(Out.Tab("", " "));
        if (Isbz())
        {
            strName = "paycent,acType,id,ptype,rand,info,act";
            strValu = "" + paycent + "'" + ac + "'" + dice.ID + "'" + ptype + "'" + new Rand().RandNumer(4) + "'ok'paysave2";
            strOthe = string.Empty;
            strOthe = "" + ub.Get("SiteBz2") + "押注,dice.aspx,post,0,blue";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getPage("dice.aspx?act=pay&amp;ptype=" + ptype + "") + "\">再看看吧</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PaySavePage()
    {
        if (IsOpen() == false)
        {
            Utils.Error("游戏开放时间:" + ub.GetSub("DiceOnTime", xmlPath) + "", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-5]$|^9012$|^813$|^714$|^615$|^516$|^417$", "选择押注类型错误"));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        long paycent = int.Parse(Utils.GetRequest("paycent", "post", 2, @"^[1-9]\d*$", "押注额填写错误"));
        string acType = Utils.GetRequest("acType", "post", 2, @"[\s\S]{1,4}", "选项无效");

        //是否刷屏
        long small = Convert.ToInt64(ub.GetSub("DiceSmallPay", xmlPath));
        long big = Convert.ToInt64(ub.GetSub("DiceBigPay", xmlPath));
        string appName = "LIGHT_DICE";
        int Expir = Utils.ParseInt(ub.GetSub("DiceExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir, paycent, small, big);

        if (paycent > new BCW.BLL.User().GetGold(meid))
        {
            Utils.Error("你的" + ub.Get("SiteBz") + "不足", Utils.getPage("dice.aspx"));
        }

        //支付安全提示
        string[] p_pageArr = { "act", "info", "id", "ptype", "paycent", "acType", "rand" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

        BCW.Model.Game.Dicelist dice = new BCW.BLL.Game.Dicelist().GetDicelist(id);
        if (dice.EndTime < DateTime.Now)
        {
            Utils.Error("本局已截止下注", Utils.getUrl("dice.aspx"));
        }

        int BuyNum = GetiType(acType);
        string mename = new BCW.BLL.User().GetUsName(meid);
        //加总押注额
        new BCW.BLL.Game.Dicelist().UpdatePool(dice.ID, paycent);
        new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "挖宝" + dice.ID + "局押注" + acType);

        BCW.Model.Game.Dicepay model = new BCW.Model.Game.Dicepay();
        model.DiceId = dice.ID;
        model.UsID = meid;
        model.UsName = mename;
        model.Types = ptype;
        model.BuyNum = BuyNum;
        model.BuyCent = paycent;
        model.BuyCount = 1;
        model.WinCent = 0;
        model.AddTime = DateTime.Now;
        model.State = 0;
        model.bzType = 0;
        if (!new BCW.BLL.Game.Dicepay().Exists(dice.ID, meid, 0, ptype, BuyNum))
            new BCW.BLL.Game.Dicepay().Add(model);
        else
            new BCW.BLL.Game.Dicepay().Update(model);


        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dice.aspx]挖宝第" + dice.ID + "局[/url]押注" + paycent + "" + ub.Get("SiteBz") + "";
        new BCW.BLL.Action().Add(9, 0, meid, "", wText);
        //活跃抽奖入口_20160621姚志光
        try
        {
            //表中存在点数挖宝记录
            if (new BCW.BLL.tb_WinnersGame().ExistsGameName("点数挖宝"))
            {
                //投注是否大于设定的限额，是则有抽奖机会
                if (paycent > new BCW.BLL.tb_WinnersGame().GetPrice("点数挖宝"))
                {
                    string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                    int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "点数挖宝", 3);
                    if (hit == 1)
                    {
                        //内线开关 1开
                        if (WinnersGuessOpen == "1")
                        {
                            //发内线到该ID
                            new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                        }
                    }
                }
            }
        }
        catch { }
        Utils.Success("押注", "押注成功，花费了" + paycent + "" + ub.Get("SiteBz") + "<br /><a href=\"" + Utils.getPage("dice.aspx?act=pay&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("dice.aspx"), "5");

    }

    private void PaySave2Page()
    {
        if (IsOpen() == false)
        {
            Utils.Error("游戏开放时间:" + ub.GetSub("DiceOnTime", xmlPath) + "", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-5]$|^9012$|^813$|^714$|^615$|^516$|^417$", "选择押注类型错误"));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        long paycent = int.Parse(Utils.GetRequest("paycent", "post", 2, @"^[1-9]\d*$", "押注额填写错误"));
        if (paycent < Convert.ToInt64(ub.GetSub("DiceSmallPay2", xmlPath)) || paycent > (Convert.ToInt64(ub.GetSub("DiceBigPay2", xmlPath)) * 1))
        {
            Utils.Error("押注金额限" + ub.GetSub("DiceSmallPay2", xmlPath) + "-" + (Convert.ToInt64(ub.GetSub("DiceBigPay2", xmlPath)) * 1) + "" + ub.Get("SiteBz2") + "", "");
        }
        string acType = Utils.GetRequest("acType", "post", 2, @"[\s\S]{1,4}", "选项无效");

        //是否刷屏
        string appName = "LIGHT_DICE";
        int Expir = Utils.ParseInt(ub.GetSub("DiceExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        if (paycent > new BCW.BLL.User().GetMoney(meid))
        {
            Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
        }

        BCW.Model.Game.Dicelist dice = new BCW.BLL.Game.Dicelist().GetDicelist(id);
        if (dice.EndTime < DateTime.Now)
        {
            Utils.Error("本局已截止下注", Utils.getUrl("dice.aspx"));
        }

        //每个ID每局只能下注多少" + ub.Get("SiteBz2") + "
        long myIDPay = new BCW.BLL.Game.Dicepay().GetSumBuyCent("DiceId=" + dice.ID + " and UsID=" + meid + " and bzType=1");
        long IDPay = Convert.ToInt64(ub.GetSub("DiceIDMaxPay", xmlPath));
        if ((myIDPay + paycent) > IDPay)
        {
            if (myIDPay > IDPay)
            {
                Utils.Error("系统限制每ID每期下注额最多" + IDPay + "" + ub.Get("SiteBz2") + "，欢迎在下期下注", "");
            }
            else
            {
                Utils.Error("系统限制每ID每期下注额最多" + IDPay + "" + ub.Get("SiteBz2") + "，你现在最多可以下注" + (IDPay - myIDPay) + "" + ub.Get("SiteBz2") + "", "");
            }
        }

        int BuyNum = GetiType(acType);
        string mename = new BCW.BLL.User().GetUsName(meid);
        //加总押注额
        new BCW.BLL.Game.Dicelist().UpdateWinPool(dice.ID, paycent);
        new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "挖宝" + dice.ID + "局押注" + acType);

        BCW.Model.Game.Dicepay model = new BCW.Model.Game.Dicepay();
        model.DiceId = dice.ID;
        model.UsID = meid;
        model.UsName = mename;
        model.Types = ptype;
        model.BuyNum = BuyNum;
        model.BuyCent = paycent;
        model.BuyCount = 1;
        model.WinCent = 0;
        model.AddTime = DateTime.Now;
        model.State = 0;
        model.bzType = 1;
        if (!new BCW.BLL.Game.Dicepay().Exists(dice.ID, meid, 1, ptype, BuyNum))
            new BCW.BLL.Game.Dicepay().Add(model);
        else
            new BCW.BLL.Game.Dicepay().Update(model);

        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dice.aspx]挖宝第" + dice.ID + "局[/url]押注" + paycent + "" + ub.Get("SiteBz2") + "";
        new BCW.BLL.Action().Add(9, 0, meid, "", wText);

        Utils.Success("押注", "押注成功，花费了" + paycent + "" + ub.Get("SiteBz2") + "<br /><a href=\"" + Utils.getPage("dice.aspx?act=pay&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("dice.aspx"), "5");
    }

    private void ListPage()
    {
        Master.Title = "历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=历史开奖=");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "State=1";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Dicelist> listDicelist = new BCW.BLL.Game.Dicelist().GetDicelists(pageIndex, pageSize, strWhere, out recordCount);
        if (listDicelist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dicelist n in listDicelist)
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
                string strDxs = string.Empty;
                string aNum = Utils.ConvertSeparated(n.WinNum.ToString(), 1, ",");
                string[] arrNum = Utils.ArrBySeparated(n.WinNum.ToString(), 1);
                if (arrNum[0] == arrNum[1] && arrNum[1] == arrNum[2] && arrNum[0] == arrNum[2])
                    strDxs += "豹子" + arrNum[0] + "";
                else
                {
                    int sumNum = Convert.ToInt32(arrNum[0]) + Convert.ToInt32(arrNum[1]) + Convert.ToInt32(arrNum[2]);
                    if (sumNum > 10)
                        strDxs += "大";
                    else
                        strDxs += "小";

                    if (sumNum % 2 == 0)
                        strDxs += "双";
                    else
                        strDxs += "单";
                }
                builder.Append("第" + n.ID + "期开出:<a href=\"" + Utils.getUrl("dice.aspx?act=listview&amp;id=" + n.ID + "") + "\">" + Utils.ConvertSeparated(n.WinNum.ToString(), 1, ",") + "</a>" + strDxs + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Dicelist model = new BCW.BLL.Game.Dicelist().GetDicelist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + id + "局挖宝";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("dice.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "DiceId=" + id + " and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Dicepay> listDicepay = new BCW.BLL.Game.Dicepay().GetDicepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listDicepay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "局奖池押注:" + model.Pool + "");
            builder.Append("<br />共" + recordCount + "注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Dicepay n in listDicepay)
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
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>获得" + n.WinCent + "" + bzTypes + "");

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
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "我的未开押注";
        else
            strTitle = "我的历史押注";

        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "";
        if (ptype == 1)
            strWhere += " and State=0";
        else
            strWhere += " and State>0";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string Diceqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Dicepay> listDicepay = new BCW.BLL.Game.Dicepay().GetDicepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listDicepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dicepay n in listDicepay)
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

                if (n.DiceId.ToString() != Diceqi)
                {
                    builder.Append("第" + n.DiceId + "局");
                    if (ptype == 2)
                        builder.Append("开出:" + Utils.ConvertSeparated(new BCW.BLL.Game.Dicelist().GetWinNum(n.DiceId).ToString(), 1, ",") + "");

                    builder.Append("<br />");
                }

                string sText = string.Empty;
                if (n.Types == 1)
                {
                    sText = "押大小:" + ((n.BuyNum == 1) ? "大" : "小");
                }
                else if (n.Types == 2)
                {
                    sText = "押单双:" + ((n.BuyNum == 1) ? "单" : "双");
                }
                else if (n.Types == 3)
                {
                    sText = "押总和:" + n.BuyNum.ToString();
                }
                else if (n.Types == 4)
                {
                    sText = "押对子:" + n.BuyNum.ToString();
                }
                else if (n.Types == 5)
                {
                    if (n.BuyNum == 0)
                        sText = "押任意豹子";
                    else
                        sText = "押豹子" + n.BuyNum.ToString();
                }
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                if (n.State == 0)
                    builder.Append("" + sText + "，共" + n.BuyCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                else if (n.State == 1)
                {
                    builder.Append("" + sText + "，共" + n.BuyCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                }
                else
                    builder.Append("" + sText + "，共" + n.BuyCent + "" + bzTypes + "，赢" + n.WinCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                Diceqi = n.DiceId.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=1") + "\">未开押注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=2") + "\">历史押注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage()
    {
        string strTitle = "挖宝排行榜";
        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "bzType=0 and State>0";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Dicepay> listDicepay = new BCW.BLL.Game.Dicepay().GetDicepaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listDicepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dicepay n in listDicepay)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=1") + "\">未开押注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=2") + "\">历史押注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Dicepay().ExistsState(pid, meid))
        {
            new BCW.BLL.Game.Dicepay().UpdateState(pid);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.Game.Dicepay().GetWinCent(pid));
            int bzType = new BCW.BLL.Game.Dicepay().GetbzType(pid);
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("DiceTax", xmlPath));
            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;
            if (bzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "挖宝兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("dice.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "挖宝兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("dice.aspx?act=case"), "1");
            }

        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("dice.aspx?act=case"), "1");
        }
    }

    private void CasePostPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        string[] strArrId = arrId.Split(",".ToCharArray());
        long winMoney = 0;
        long winMoney2 = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BLL.Game.Dicepay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.Dicepay().UpdateState(pid);
                //操作币
                long win = Convert.ToInt64(new BCW.BLL.Game.Dicepay().GetWinCent(pid));
                int bzType = new BCW.BLL.Game.Dicepay().GetbzType(pid);
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("DiceTax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(win * Tax * 0.01);
                }
                if (bzType == 0)
                {
                    winMoney += win - SysTax;
                    new BCW.BLL.User().UpdateiGold(meid, (win - SysTax), "挖宝兑奖-标识ID" + pid + "");
                }
                else
                {
                    winMoney2 += win - SysTax;
                    new BCW.BLL.User().UpdateiMoney(meid, (win - SysTax), "挖宝兑奖-标识ID" + pid + "");
                }
            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("dice.aspx?act=case"), "1");
    }


    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and WinCent>0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        string Diceqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Dicepay> listDicepay = new BCW.BLL.Game.Dicepay().GetDicepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listDicepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dicepay n in listDicepay)
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

                if (n.DiceId.ToString() != Diceqi)
                {
                    builder.Append("第" + n.DiceId + "局");
                    builder.Append("开出:" + Utils.ConvertSeparated(new BCW.BLL.Game.Dicelist().GetWinNum(n.DiceId).ToString(), 1, ",") + "<br />");
                }

                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                string sText = string.Empty;
                if (n.Types == 1)
                {
                    sText = "押大小:" + ((n.BuyNum == 1) ? "大" : "小");
                }
                else if (n.Types == 2)
                {
                    sText = "押单双:" + ((n.BuyNum == 1) ? "单" : "双");
                }
                else if (n.Types == 3)
                {
                    sText = "押总和:" + n.BuyNum.ToString();
                }
                else if (n.Types == 4)
                {
                    sText = "押对子:" + n.BuyNum.ToString();
                }
                else if (n.Types == 5)
                {
                    if (n.BuyNum == 0)
                        sText = "押任意豹子";
                    else
                        sText = "押豹子" + n.BuyNum.ToString();
                }
                builder.Append(sText + "，共" + n.BuyCent + "" + bzTypes + "赢" + n.WinCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("dice.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                Diceqi = n.DiceId.ToString();
                arrId = arrId + " " + n.ID;
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
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,dice.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void HelpPage()
    {
        Master.Title = "挖宝游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("挖宝游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【挖宝起源】<br />相传,骰子的发明人是三国时代的文学家植,最初用做占卜工具.后来才演变成后宫嫔妃的游戏,掷骰子点数赌酒或赌丝绸香袋等物.当时骰子的点穴上涂的是黑色,在唐代才增加描红.<br /><br />【关于挖宝】<br />挖宝每隔" + ub.GetSub("DiceCycleMin", xmlPath) + "分钟开一局,每局都由系统随机产生开奖结果,游戏厅将保证开奖的公平性.<br /><br />【押大/押小】<br />押注三个骰子的数字总和是大或小,赔率为1赔" + ub.GetSub("DiceDx", xmlPath) + ".大是指数字总和在11—17;小是指数字总和在4—10.(若开豹子,则押大或押小都输!)<br /><br />【押单/押双】<br />押注三个骰子的数字总和是单或双,赔率为1赔" + ub.GetSub("DiceDs", xmlPath) + ".单是指数字总和为5,7,9,11,13,15,17;双是指数字总和为4,6,8,10,12,14,16.(若开豹子,则押单或押双都输!)<br /><br />【押三个数字总和】<br />押注三个骰子的数字总和,可押注的总和数值从4—17.(若开豹子,则都算输!)<br />押注赔率如下:<br />数字总和/赔率<br />4,17/1赔" + ub.GetSub("DiceSum417", xmlPath) + "<br />5,16/1赔" + ub.GetSub("DiceSum516", xmlPath) + "<br />6,15/1赔" + ub.GetSub("DiceSum615", xmlPath) + "<br />7,14/1赔" + ub.GetSub("DiceSum714", xmlPath) + "<br />8,13/1赔" + ub.GetSub("DiceSum813", xmlPath) + "<br />9,10,11,12/1赔" + ub.GetSub("DiceSum9012", xmlPath) + "<br /><br />【押对子】<br />押骰子会出现2个一样的数字,押指定对子,1赔" + ub.GetSub("DiceDz", xmlPath) + ".(若开豹子,则都算输!)<br /><br />【押豹子】<br />豹子:即开出的骰子3个数都是一样的<br />押注 赔率<br />任意豹子/1赔" + ub.GetSub("DiceBz", xmlPath) + "<br />指定豹子/1赔" + ub.GetSub("DiceBz2", xmlPath) + "<br />注:当开豹子时,其他投注(大小,单双,总和,对子)皆输!");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dice.aspx") + "\">挖宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("DiceOnTime", xmlPath);
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now > dt1 && DateTime.Now < dt2)
                {
                    IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
        }
        return IsOpen;
    }

    private int GetiType(string acType)
    {
        acType = acType.Replace("押", "").Replace("两个", "").Replace("豹子", "");
        acType = acType.Replace("大", "1");
        acType = acType.Replace("小", "2");
        acType = acType.Replace("单", "1");
        acType = acType.Replace("双", "2");
        acType = acType.Replace("任意", "0");
        return Utils.ParseInt(acType);
    }

    private bool Isbz()
    {
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("wapu.so"))
            return true;
        else
            return false;
    }
}
