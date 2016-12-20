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

public partial class bbs_game_Public_set : System.Web.UI.Page
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

        string GameName = string.Empty;
        string gameurl = string.Empty;
        long BigPay = 0;
        int type = Convert.ToInt32(Utils.GetRequest("type", "all", 2, @"^[1-9]\d*$", "找不到此游戏"));
        int ptype = Convert.ToInt32(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "找不到此下注类型"));
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");

        if (type == 1)//新快3
        {
            GameName = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//游戏名
            gameurl = "xk3.aspx";//游戏地址
            BigPay = Convert.ToInt64(ub.GetSub("xBigPay", "/Controls/xinkuai3.xml"));//单注最大上限
        }
        else if (type == 2)//幸运二八
        {
            GameName = ub.GetSub("Luck28Name", "/Controls/luck28.xml");//游戏名
            gameurl = "luck28.aspx";//游戏地址
            BigPay = Convert.ToInt64(ub.GetSub("Luck28BigPay", "/Controls/luck28.xml"));//单注最大上限
        }
        else if (type == 3)//好彩一
        {
            GameName = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//游戏名
            gameurl = "hc1.aspx";//游戏地址
            BigPay = Convert.ToInt64(ub.GetSub("Hc1BigPay", "/Controls/hc1.xml"));//单注最大上限
        }
        //else if (type == 4)//进球彩
        //{
        //    GameName = ub.GetSub("GameName", "/Controls/jqc.xml");//游戏名
        //    gameurl = "jqc.aspx";//游戏地址

        //    long BigPay1 = Convert.ToInt64(ub.GetSub("BigPrice", "/Controls/jqc.xml"));//单期个人最大上限
        //    long BigPay2 = Convert.ToInt64(ub.GetSub("zhuPrice", "/Controls/jqc.xml"));//单注金额
        //    BigPay = (BigPay1 / BigPay2);//最大下注倍数
        //}
        //else if (type == 5)//百家乐
        //{
        //    GameName = ub.GetSub("baccaratName", "/Controls/bjl.xml");//游戏名
        //    gameurl = "bjl.aspx";//游戏地址
        //    BigPay = Convert.ToInt64(ub.GetSub("baccaratHigherPay", "/Controls/bjl.xml"));//单注最大上限
        //}
        else
            Utils.Error("抱歉,输入有误.", "");

        Master.Title = "快捷下注设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("" + gameurl + "") + "\">" + GameName + "</a>&gt;快捷下注设置");
        builder.Append(Out.Tab("</div>", "<br />"));


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "确定设置")
        {
            int GameID = Convert.ToInt32(Utils.GetRequest("type", "post", 1, @"^[1-9]\d*$", "1"));
            if (GameID != type)
            {
                Utils.Error("抱歉,不能设置其它游戏的快捷.", "");
            }
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

            string Bet = Bet1 + "#" + Bet2 + "#" + Bet3 + "#" + Bet4 + "#" + Bet5 + "#" + Bet6 + "#" + Bet7 + "#" + Bet8 + "#" + Bet9 + "#" + Bet10;
            new BCW.XinKuai3.BLL.Public_User().Update_1(meid, Bet, type);
            Utils.Success("游戏快捷下注设置", "设置成功，当前快捷下注：" + Bet + "，正在返回..", Utils.getUrl(Out.UBB(backurl)), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>[" + GameName + "]</b>快捷下注设置:");
            builder.Append(Out.Tab("</div>", ""));

            DataSet ds = new BCW.XinKuai3.BLL.Public_User().GetList("*", "UsID=" + meid + " and Type=" + type + "");
            int tt = 0;
            try
            {
                tt = int.Parse(ds.Tables[0].Rows[0]["type"].ToString());
            }
            catch { }
            if (tt != type)
            {
                Utils.Error("抱歉,你不能修改其他游戏的快捷.", "");
            }
            string bet2 = ds.Tables[0].Rows[0]["Settings"].ToString();

            string[] bet3 = bet2.Split('#');
            string strText = "快捷下注1:,快捷下注2:,快捷下注3:,快捷下注4:,快捷下注5:,快捷下注6:,快捷下注7:,快捷下注8:,快捷下注9:,快捷下注10:,,,";
            string strName = "Bet1,Bet2,Bet3,Bet4,Bet5,Bet6,Bet7,Bet8,Bet9,Bet10,type,backurl";
            string strType = "text,text,text,text,text,text,text,text,text,text,hidden,hidden";
            string strValu = "" + bet3[0] + "'" + bet3[1] + "'" + bet3[2] + "'" + bet3[3] + "'" + bet3[4] + "'" + bet3[5] + "'" + bet3[6] + "'" + bet3[7] + "'" + bet3[8] + "'" + bet3[9] + "'" + type + "'" + backurl + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea = "/";
            string strOthe = "确定设置,public_set.aspx?ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl(Out.UBB(backurl)) + "\">[返回游戏页面]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<b style=\"color:red\">说明：自己可以添加自己想要的快捷下注数目或者删除已经添加的快捷下注数目,最多添加10个快捷,不能设置超过单注限额数量,以酷币数字输入,留空或添0即为不添加快捷,设置成功后本游戏即能显示支持快捷方式的页面</b>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("" + gameurl + "") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

}
