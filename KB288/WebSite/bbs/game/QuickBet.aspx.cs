using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
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
using System.Net;
using BCW.Common;
using System.Timers;

/// <summary>
/// 蒙宗将 20161028 完善
/// </summary>

public partial class bbs_game_QuickBet : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "edit":
                EditPage(meid);//编辑
                break;
            default:
                ReloadPage(meid);
                break;
        }

    }

    private void ReloadPage(int uid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");


        if (new BCW.QuickBet.BLL.QuickBet().ExistsUsID(meid))
        {
            //builder.Append(Out.Tab("<div>",""));
            //builder.Append("快捷下注设置:");
            //builder.Append(Out.Tab("<div>", "<br />"));
            //string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            //string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            //builder.Append(game + ":" + bet);
            //builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit") + "\">添删</a>");
        }
        else
        {
            //   builder.Append("<br /><a href=\"" + Utils.getUrl("QuickBet.aspx") + "\">创建我的快捷下注</a>");
            BCW.QuickBet.Model.QuickBet model = new BCW.QuickBet.Model.QuickBet();
            model.UsID = meid;
            model.Game = new BCW.QuickBet.BLL.QuickBet().GetGame();//十个编号的游戏|1:时时彩|2快乐十分|3:快乐扑克3|4:6场半|5:胜负彩
            model.Bet = new BCW.QuickBet.BLL.QuickBet().GetBety();
            new BCW.QuickBet.BLL.QuickBet().Add(model);
        }

    }

    private void EditPage(int uid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string GameName = string.Empty;
        string gameurl = string.Empty;
        string info = string.Empty;
        long BigPay = 0;
        int type = Convert.ToInt32(Utils.GetRequest("type", "all", 2, @"^[1-9]\d*$", "找不到此游戏"));
        int ptype = Convert.ToInt32(Utils.GetRequest("ptype", "all", 2, @"^[1-9]\d*$", "找不到此下注类型"));
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");

        if (type == 1)//时时彩
        {
            GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");//游戏名：时时彩
            gameurl = "ssc.aspx";//游戏地址
            info = "ssc.aspx?act=info";
            BigPay = Convert.ToInt64(ub.GetSub("SSCBigPay", "/Controls/ssc.xml"));//单注最大上限
        }
        if (type == 2)//快乐十分
        {
            GameName = ub.GetSub("klsfName", "/Controls/klsf.xml");//游戏名：快乐十分
            gameurl = "klsf.aspx";//游戏地址
            info = "klsf.aspx?act=pay";
            BigPay = Convert.ToInt64(ub.GetSub("klsfBigPay", "/Controls/klsf.xml"));//单注最大上限
        }
        if (type == 3)//快乐扑克3
        {
            GameName = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");//游戏名：快乐十分
            gameurl = "HP3.aspx";//游戏地址
            info = "HP3.aspx?act=pay";
            BigPay = Convert.ToInt64(ub.GetSub("HP3BigPay", "/Controls/HappyPoker3.xml"));//单注最大上限
        }
        if (type == 4)
        {
            GameName = ub.GetSub("BQCName", "/Controls/BQC.xml");//游戏名：6场半
            gameurl = "BQC.aspx";//游戏地址
            info = "BQC.aspx?act=pay";

            long BigPay1 = Convert.ToInt64(ub.GetSub("BQCCprice", "/Controls/BQC.xml"));//单期个人最大上限
            long BigPay2 = Convert.ToInt64(ub.GetSub("BQCprice", "/Controls/BQC.xml"));//单注金额
            BigPay = (BigPay1 / BigPay2);//最大下注倍数
        }
        if (type == 5)
        {
            GameName = ub.GetSub("SFName", "/Controls/SFC.xml");//游戏名：胜负彩
            gameurl = "SFC.aspx";//游戏地址
            info = "SFC.aspx?act=pay";

            long BigPay1 = Convert.ToInt64(ub.GetSub("SFCprice", "/Controls/SFC.xml"));//单期个人最大上限
            long BigPay2 = Convert.ToInt64(ub.GetSub("SFprice", "/Controls/SFC.xml"));//单注金额
            BigPay = (BigPay1 / BigPay2);//最大下注倍数
        }

        //记录上一个足迹
        string GetPageUrl = Utils.getPage(0);
        if (!GetPageUrl.Contains("QuickBet.aspx?act=edit&amp;type=" + type + ""))
        {
            new BCW.BLL.User().UpdateVisitHy(meid, Server.UrlDecode(GetPageUrl));
        }

        Master.Title = "快捷下注设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("" + gameurl + "") + "\">" + GameName + "</a>&gt;快捷下注设置");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 快捷下注设置保存
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "确定设置")
        {
            string Bet1 = Utils.GetRequest("Bet1", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet1) > BigPay) Utils.Error("快捷下注1不能超过单注最大下注额度", "");
            string Bet2 = Utils.GetRequest("Bet2", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet2) > BigPay) Utils.Error("快捷下注2不能超过单注最大下注额度", "");
            string Bet3 = Utils.GetRequest("Bet3", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet3) > BigPay) Utils.Error("快捷下注3不能超过单注最大下注额度", "");
            string Bet4 = Utils.GetRequest("Bet4", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet4) > BigPay) Utils.Error("快捷下注4不能超过单注最大下注额度", "");
            string Bet5 = Utils.GetRequest("Bet5", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet5) > BigPay) Utils.Error("快捷下注5不能超过单注最大下注额度", "");
            string Bet6 = Utils.GetRequest("Bet6", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet6) > BigPay) Utils.Error("快捷下注6不能超过单注最大下注额度", "");
            string Bet7 = Utils.GetRequest("Bet7", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet7) > BigPay) Utils.Error("快捷下注7不能超过单注最大下注额度", "");
            string Bet8 = Utils.GetRequest("Bet8", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet8) > BigPay) Utils.Error("快捷下注8不能超过单注最大下注额度", "");
            string Bet9 = Utils.GetRequest("Bet9", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet9) > BigPay) Utils.Error("快捷下注9不能超过单注最大下注额度", "");
            string Bet10 = Utils.GetRequest("Bet10", "post", 1, @"^[1-9]\d*$", "0"); if (Convert.ToInt64(Bet10) > BigPay) Utils.Error("快捷下注10不能超过单注最大下注额度", "");
            string Game = Utils.GetRequest("type", "post", 1, @"^[1-9]\d*$", "");

            string Bet = Bet1 + "|" + Bet2 + "|" + Bet3 + "|" + Bet4 + "|" + Bet5 + "|" + Bet6 + "|" + Bet7 + "|" + Bet8 + "|" + Bet9 + "|" + Bet10;

            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');
            type = Convert.ToInt32(Game);
            int where = 0;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)
                {
                    where = i;
                }
            }

            string bet2 = string.Empty;
            string bet21 = string.Empty;
            string bet22 = string.Empty;


            if (bet1.Length >= 2)
            {
                for (int j = 0; j < where; j++)
                {
                    if (j == where - 1)
                    {
                        bet21 += bet1[j];
                    }
                    else
                    {
                        bet21 += bet1[j] + "#";
                    }
                }

                for (int k = (where + 1); k < bet1.Length; k++)
                {
                    if (k == bet1.Length - 1)
                    {
                        bet22 += bet1[k];
                    }
                    else
                    {
                        bet22 += bet1[k] + "#";
                    }
                }
            }

            if (bet1.Length == 1)
                bet2 = Bet;
            else if (bet1.Length == 2)
            {
                if (where == 0)
                {
                    bet2 = Bet + "#" + bet22;
                }
                if (where == 1)
                {
                    bet2 = bet21 + "#" + Bet;
                }
            }
            else
            {
                if (bet21 == "")
                {
                    bet2 = Bet + "#" + bet22;
                }
                if (bet22 == "")
                {
                    bet2 = bet21 + "#" + Bet;
                }
                if (bet21 != "" && bet22 != "")
                {
                    bet2 = bet21 + "#" + Bet + "#" + bet22;
                }

            }

            new BCW.QuickBet.BLL.QuickBet().UpdateBet(meid, bet2);

            if (type == 1)
            {
                string betbymeid = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
                string[] bet1bymeid = betbymeid.Split('#');
                string str = string.Empty;
                string gold = string.Empty;
                string[] kuai = new string[10];//{};
                kuai = bet1bymeid[0].Split('|');//取出对应的快捷下注,快乐十分type=2,处于第1个截取位置，第0位是type=1的时时彩
                for (int i = 0; i < kuai.Length; i++)
                {
                    gold = ChangeToWanSSC(kuai[i]);
                    if (kuai[i] != "0")
                    {

                        str += gold + " ";
                    }
                }
                //   try
                {
                    Utils.Success("游戏设置", "设置成功，当前快捷下注：" + str + "，正在返回..", Utils.getUrl(Out.UBB(backurl)), "3");
                }
                //catch
                //{
                //    Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + ""), "2");
                //}
            }
            else
            {
                string betbymeid = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
                string[] bet1bymeid = betbymeid.Split('#');
                string str = string.Empty;
                string gold = string.Empty;
                string st = string.Empty;
                string[] kuai = new string[10];//{};
                if (type == 2)
                {
                    kuai = bet1bymeid[1].Split('|');//取出对应的快捷下注,快乐十分type=2,处于第1个截取位置，第0位是type=1的时时彩
                }
                if (type == 3)
                {
                    kuai = bet1bymeid[2].Split('|');//取出对应的快捷下注,快乐十分type=2,处于第1个截取位置，第0位是type=1的时时彩
                }
                if (type == 4)
                {
                    kuai = bet1bymeid[3].Split('|');//取出对应的快捷下注,type=4为6场半,快乐十分type=2,处于第1个截取位置，第0位是type=1的时时彩
                }
                if (type == 5)
                {
                    kuai = bet1bymeid[4].Split('|');//取出对应的快捷下注,type=4为6场半,快乐十分type=2,处于第1个截取位置，第0位是type=1的时时彩|type=5:胜负彩
                }
                for (int i = 0; i < kuai.Length; i++)
                {
                    if (kuai[i] != "0")
                    {
                        //if (Convert.ToInt64(kuai[i]) >= 10000)
                        //{
                        //    if (Convert.ToInt64(kuai[i]) % 10000 == 0)
                        //    {
                        //        gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                        //    }
                        //    else
                        //    {
                        //     st = (Convert.ToInt64(kuai[i]) / 10000) + ".X万";
                        //        gold = st;
                        //    }
                        //}
                        //else
                        //{
                        //    gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                        //}

                        gold = ChangeToWan(kuai[i]);

                        str += gold + " ";
                    }
                }
                Utils.Success("游戏设置", "设置成功，当前快捷下注：" + str + "，正在返回..", Utils.getUrl(Out.UBB(backurl)), "3");
            }
        }
        #endregion
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>[" + GameName + "]</b>快捷下注设置:");
            builder.Append(Out.Tab("</div>", ""));
            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');

            string bet2 = string.Empty;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)
                {
                    bet2 = bet1[i];
                }
            }
            string[] bet3 = bet2.Split('|');
            string strText = "快捷下注1:,快捷下注2:,快捷下注3:,快捷下注4:,快捷下注5:,快捷下注6:,快捷下注7:,快捷下注8:,快捷下注9:,快捷下注10:,,,";
            string strName = "Bet1,Bet2,Bet3,Bet4,Bet5,Bet6,Bet7,Bet8,Bet9,Bet10,type,backurl";
            string strType = "text,text,text,text,text,text,text,text,text,text,hidden,hidden";
            string strValu = "" + bet3[0] + "'" + bet3[1] + "'" + bet3[2] + "'" + bet3[3] + "'" + bet3[4] + "'" + bet3[5] + "'" + bet3[6] + "'" + bet3[7] + "'" + bet3[8] + "'" + bet3[9] + "'" + type + "'" + backurl + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea = "/";
            string strOthe = "";

            strOthe = "确定设置,QuickBet.aspx?act=edit&amp;ptype=" + ptype + ",post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            string BackUrl = new BCW.BLL.User().GetVisitHy(meid);
            if (BackUrl != "")
            {
                builder.Append("<a href=\"" + Utils.getUrl(Out.UBB(BackUrl)) + "\">[返回游戏页面]</a>");
            }
            else
            {
                if (type == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl(info + "&amp;ptype=" + ptype + "") + "\">[返回游戏页面]</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl(Out.UBB(backurl)) + "\">[返回游戏页面]</a>");
                }
            }
            builder.Append(Out.Tab("</div>", ""));


        }
        #region 底部
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<b style=\"color:red\">说明：自己可以添加自己想要的快捷下注数目或者删除已经添加的快捷下注数目,最多添加10个快捷,不能设置超过单注限额数量,以酷币数字输入,留空或添0即为不添加快捷,设置成功后本游戏即能显示支持快捷方式的页面</b>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("" + gameurl + "") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }

    /// <summary>
    /// 快捷下注转换成X万
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ChangeToWan(string str)
    {
        string CW = string.Empty;
        try
        {
            if (str != "")
            {
                long first = 0;
                first = Convert.ToInt64(str.Trim());
                if (first >= 10000)
                {
                    if (first % 10000 == 0)
                    {
                        CW = (first / 10000) + "万";
                    }
                    else
                    {
                        CW = (first / 10000) + ".X万";
                    }
                }
                else
                {
                    CW = first.ToString();
                }
            }
        }
        catch { }
        return CW;
    }

    #region 时时彩快捷下注转换成万
    /// <summary>
    /// 时时彩快捷下注转换成整万
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ChangeToWanSSC(string str)
    {
        string CW = string.Empty;
        try
        {
            long first = 0;
            first = Convert.ToInt64(str.Trim());
            if (first >= 10000)
            {
                if (first % 10000 == 0)
                {
                    CW = (first / 10000) + "万";
                }
                else
                {
                    CW = first.ToString();
                }
            }
            else
            {
                CW = first.ToString();
            }
        }
        catch { }
        return CW;
    }
    #endregion
}
