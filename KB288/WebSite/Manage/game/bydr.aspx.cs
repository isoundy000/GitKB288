using System;
using System.Collections;
using System.Collections.Generic;
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
public partial class Manage_game_bydr : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected string xmlPath = "/Controls/BYDR.xml";
    protected string GameName = ub.GetSub("bydrName", "/Controls/BYDR.xml");//游戏名字

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view"://------------捕鱼游戏数据设置
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "del1":
                Del1Page();
                break;
            case "robot":
                robotPage();//捕鱼机器人20160813邵广林
                break;
            case "delall":
                DelallPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "reward1":
                reward1Page();
                break;
            case "user":
                UserPage();//................捕鱼特殊用户管理
                break;
            case "userset":
                UsersetPage();
                break;
            case "Record":
            case "Record1":
                RecordPage(act);//.............记录管理
                break;
            case "userAdd":
                UserAddPage();
                break;
            case "stat":
                StatPage();//。。。。。。。。盈利分析
                break;
            case "back":
                BackPage();//。。。。。。....。。返负功能
                break;
            case "backsave":
                BackSavePage();
                break;
            case "list":
                listPage();
                break;
            case "top":
                topPage();
                break;
            case "dongtai":
                dongtaiPage();//.............捕鱼动态管理
                break;
            case "daoju":
                DaojuPage();//...................道具
                break;
            case "daojuset":
                DaojusetPage();//................道具修改
                break;
            case "daojuset1":
                Daojuset1Page();//................道具修改
                break;
            case "daojuadd":
                DaojuaddPage();//................道具添加
                break;
            case "reward":
                RewardPage();
                break;
            case "addJ":
                addJPage();       //。。。。。。兑奖
                break;
            case "alllistadd":
                alllistaddPage(); //。。。。.....。单个兑换
                break;
            case "backsave1":
                BackSave1Page();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "捕鱼管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("根据日期搜索排行榜前10名：");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", "20150000"));
        int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", "20159999"));
        string rewardid = "";

        if (Utils.ToSChinese(ac) == "确认搜索")
        {
            string searchday1 = Utils.GetRequest("sTime", "all", 2, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
            string searchday2 = Utils.GetRequest("oTime", "all", 2, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
            //查询条件  

            if (searchday1 == "")
            {
                searchday1 = DateTime.Now.ToString("yyyyMMdd");
            }
            if (searchday2 == "")
            {
                searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            }
            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "num,num,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br/>"));
            DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("Top 10 usID,sum(AllcolletGold)as bbb", "jID=1 and Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'GROUP BY usID order by Sum(AllcolletGold) DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;
                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    builder.Append("Top" + (i + 1) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["usID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["usID"].ToString())) + "</a>" + "净赚" + (Convert.ToInt64(ds.Tables[0].Rows[i]["bbb"]) + ub.Get("SiteBz") + "<br />"));

                }
            }
            else
                builder.Append("没有相关记录.");
            builder.Append(Out.Tab("</div>", ""));
            string strText2 = ",,,,";
            string strName2 = "stime,otime,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden";
            string strValu2 = searchday1 + "'" + searchday2 + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = "奖励发放,bydr.aspx?act=reward,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));

        }
        else
        {
            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "num,num,hidden";
            string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div>", "<br/>"));
            DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("Top 10 usID,sum(AllcolletGold)as bbb", "jID=1 and Time>='" + (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'GROUP BY usID order by Sum(AllcolletGold) DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;
                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    builder.Append("Top" + (i + 1) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["usID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["usID"].ToString())) + "</a>" + "净赚" + (Convert.ToInt64(ds.Tables[0].Rows[i]["bbb"]) + ub.Get("SiteBz") + "<br />"));
                }
            }
            builder.Append(Out.Tab("</div>", ""));

            string strText2 = ",,,,";
            string strName2 = "stime,otime,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden";
            string strValu2 = DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = "奖励发放,bydr.aspx?act=reward,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));

        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=stat") + "\">营业分析</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=back") + "\">返负设置</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("../xml/bydrset.aspx") + "\">游戏配置</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=reset") + "\">重置游戏</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=user") + "\">上帝之手</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=view") + "\">数据设置</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=daoju") + "\">道具管理</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=top") + "\">排行管理</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=dongtai") + "\">动态管理</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=Record") + "\">记录管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=addJ") + "\">兑奖管理</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=robot") + "\">机器管理</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //机器人
    private void robotPage()
    {
        Master.Title = "" + GameName + "_机器人";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/BYDR.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ROBOTID = Utils.GetRequest("ROBOTID", "post", 1, @"^[^\^]{1,2000}$", xml.dss["ROBOTID"].ToString());
            string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["IsBot"].ToString());
            string ROBOTBUY = Utils.GetRequest("ROBOTBUY", "post", 1, @"^[0-9]$", xml.dss["ROBOTBUY"].ToString());
            string ROBOTMIAO = Utils.GetRequest("ROBOTMIAO", "post", 1, @"^[1-9]\d*$", xml.dss["ROBOTMIAO"].ToString());
            string ROBOTcishu = Utils.GetRequest("ROBOTcishu", "post", 1, @"^[1-9]\d*$", xml.dss["ROBOTcishu"].ToString());
            string zu = Utils.GetRequest("zu", "post", 1, @"^[1-9]$", "1");
            string zuid = Utils.GetRequest("zuid", "post", 1, @"^[^\^]{1,2000}$", "");

            xml.dss["ROBOTID"] = ROBOTID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["IsBot"] = IsBot;
            xml.dss["ROBOTBUY"] = ROBOTBUY;
            xml.dss["ROBOTMIAO"] = ROBOTMIAO;
            xml.dss["ROBOTcishu"] = ROBOTcishu;
            xml.dss["zu"] = zu;
            xml.dss["zuid"] = zuid.Trim();
            System.IO.File.WriteAllText(Server.MapPath(xmlPath.Trim()), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "_机器人", "机器人设置成功，正在返回..", Utils.getUrl("bydr.aspx?act=robot"), "1");
        }
        else
        {
            ///[捕鱼一次需要5秒，刷新间隔>=(捕鱼次数+1)*5]
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("bydr.aspx") + "\">" + GameName + "</a>&gt;机器人"));
            string strText = "机器人状态:/,机器人ID:(多个机器人ID请用#分隔)/,机器人每天玩游戏的次数:（0为不限制）/,机器人每次刷新间隔:(秒)/,机器人每次刷新时的捕鱼次数:(大于0)/,每组机器人个数:(每组个数不可以超过机器人个数)/,组上的机器人:(只作查看，不用随意修改)/";
            string strName = "IsBot,ROBOTID,ROBOTBUY,ROBOTMIAO,ROBOTcishu,zu,zuid";
            string strType = "select,big,text,text,text,text,big";
            string strValu = "" + xml.dss["IsBot"].ToString() + "'" + xml.dss["ROBOTID"].ToString().Trim() + "'" + xml.dss["ROBOTBUY"].ToString() + "'" + xml.dss["ROBOTMIAO"].ToString() + "'" + xml.dss["ROBOTcishu"].ToString() + "'" + xml.dss["zu"].ToString() + "'" + xml.dss["zuid"].ToString().Trim() + "";
            string strEmpt = "0|关闭|1|开启,true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,bydr.aspx?act=robot,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        string aa1 = xml.dss["ROBOTID"].ToString();
        string[] sNum1 = Regex.Split(aa1, "#");
        int bb1 = sNum1.Length;


        builder.Append("机器人：《" + bb1 + "》个<br/>");
        string ROBOTID2 = Convert.ToString(ub.GetSub("ROBOTID", xmlPath));
        string[] name1 = ROBOTID2.Split('#');
        for (int n = 0; n < name1.Length; n++)
        {
            if ((n + 1) % 5 == 0)
            {
                builder.Append(name1[n] + "," + "<br />");
            }
            else
            {
                builder.Append(name1[n] + ",");
            }
        }
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void RewardPage()
    {
        Master.Title = "奖励发放";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼</a>&gt;");
        builder.Append("奖励发放<br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append("发放id：");

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string searchday1 = Utils.GetRequest("stime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
        string searchday2 = Utils.GetRequest("otime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
        //查询条件  


        DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("Top 10 usID,sum(AllcolletGold)as bbb", "jID=1 and Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'GROUP BY usID order by Sum(AllcolletGold) DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int TodayBuy = 0;
                try
                {
                    TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                }
                catch
                {
                    continue;
                }
                builder.Append(" " + ds.Tables[0].Rows[i]["usID"] + "#");
            }
        }
        //查询条件  
        if (Utils.ToSChinese(ac) == "确认")
        {
            int[] IdRe = new int[10];
            int[] Top = new int[10];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;

                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    Top[i] = TodayBuy;
                }

                IdRe[0] = int.Parse(Utils.GetRequest("top1", "post", 1, "", ""));
                IdRe[1] = int.Parse(Utils.GetRequest("top2", "post", 1, "", ""));
                IdRe[2] = int.Parse(Utils.GetRequest("top3", "post", 1, "", ""));
                IdRe[3] = int.Parse(Utils.GetRequest("top4", "post", 1, "", ""));
                IdRe[4] = int.Parse(Utils.GetRequest("top5", "post", 1, "", ""));
                IdRe[5] = int.Parse(Utils.GetRequest("top6", "post", 1, "", ""));
                IdRe[6] = int.Parse(Utils.GetRequest("top7", "post", 1, "", ""));
                IdRe[7] = int.Parse(Utils.GetRequest("top8", "post", 1, "", ""));
                IdRe[8] = int.Parse(Utils.GetRequest("top9", "post", 1, "", ""));
                IdRe[9] = int.Parse(Utils.GetRequest("top10", "post", 1, "", ""));
                builder.Append(Out.Tab("<div>", ""));


                builder.Append(Out.Tab("</div>", "<br />"));
                for (int i = 0; i < 10; i++)
                {
                    if (IdRe[i] != 0)
                    {
                        builder.Append("【第" + (i + 1) + "名】：");
                        builder.Append(Top[i] + " 获得：");
                        builder.Append(IdRe[i] + "币<br />");
                    }
                    else
                    {
                        builder.Append("【第" + (i + 1) + "名】：获得0币，请返回重新输入！<br />");
                    }
                }
            }
            string strText = ":/," + ":/," + "Top" + (1) + ":/," + "Top" + (2) + ":/," + "Top" + (3) + ":/," + "Top" + (4) + ":/," + "Top" + (5) + ":/," + "Top" + (6) + ":/," + "Top" + (7) + ":/," + "Top" + (8) + ":/," + "Top" + (9) + ":/," + "Top" + (10) + ":/,";
            string strName = "stime,otime,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,backurl";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = searchday1 + "'" + searchday2 + "'" + IdRe[0] + "'" + IdRe[1] + "'" + IdRe[2] + "'" + IdRe[3] + "'" + IdRe[4] + "'" + IdRe[5] + "'" + IdRe[6] + "'" + IdRe[7] + "'" + IdRe[8] + "'" + IdRe[9] + "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "ture,ture,true,true,true,true,true,true,true,true,true,true,false";
            string strIdea = "/";
            string strOthe = "确认发放,bydr.aspx?act=reward1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            strText = ":/," + ":/," + "Top" + (1) + ":/," + "Top" + (2) + ":/," + "Top" + (3) + ":/," + "Top" + (4) + ":/," + "Top" + (5) + ":/," + "Top" + (6) + ":/," + "Top" + (7) + ":/," + "Top" + (8) + ":/," + "Top" + (9) + ":/," + "Top" + (10) + ":/,";
            strName = "stime,otime,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,backurl";
            strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = searchday1 + "'" + searchday2 + "'" + IdRe[0] + "'" + IdRe[1] + "'" + IdRe[2] + "'" + IdRe[3] + "'" + IdRe[4] + "'" + IdRe[5] + "'" + IdRe[6] + "'" + IdRe[7] + "'" + IdRe[8] + "'" + IdRe[9] + "'" + "'" + Utils.getPage(0) + "";
            strEmpt = "ture,ture,true,true,true,true,true,true,true,true,true,true,false";
            strIdea = "/";
            strOthe = "返回,bydr.aspx?act=reward,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");
        }

        else
        {
            int[] IdRe = new int[10];
            int[] Top = new int[10];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;

                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    Top[i] = TodayBuy;
                }
                string strText = ":/," + ":/," + "Top" + (1) + ":/," + "Top" + (2) + ":/," + "Top" + (3) + ":/," + "Top" + (4) + ":/," + "Top" + (5) + ":/," + "Top" + (6) + ":/," + "Top" + (7) + ":/," + "Top" + (8) + ":/," + "Top" + (9) + ":/," + "Top" + (10) + ":/,";
                string strName = "stime,otime,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,backurl";
                string strType = "hidden,hidden,num,num,num,num,num,num,num,num,num,num,hidden";
                string strValu = searchday1 + "'" + searchday2 + "'" + IdRe[0] + "'" + IdRe[1] + "'" + IdRe[2] + "'" + IdRe[3] + "'" + IdRe[4] + "'" + IdRe[5] + "'" + IdRe[6] + "'" + IdRe[7] + "'" + IdRe[8] + "'" + IdRe[9] + "'" + "'" + Utils.getPage(0) + "";
                string strEmpt = "ture,ture,true,true,true,true,true,true,true,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确认,bydr.aspx?act=reward,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append("<br />");
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void reward1Page()
    {
        string searchday1 = Utils.GetRequest("stime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
        string searchday2 = Utils.GetRequest("otime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
        //查询条件  

        DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("Top 10 usID,sum(AllcolletGold)as bbb", "jID=1 and Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'GROUP BY usID order by Sum(AllcolletGold) DESC");

        int[] IdRe = new int[10];
        int[] Top = new int[10];

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int TodayBuy = 0;

                try
                {
                    TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                }
                catch
                {
                    continue;
                }
                Top[i] = TodayBuy;
            }

            IdRe[0] = int.Parse(Utils.GetRequest("top1", "post", 1, "", ""));
            IdRe[1] = int.Parse(Utils.GetRequest("top2", "post", 1, "", ""));
            IdRe[2] = int.Parse(Utils.GetRequest("top3", "post", 1, "", ""));
            IdRe[3] = int.Parse(Utils.GetRequest("top4", "post", 1, "", ""));
            IdRe[4] = int.Parse(Utils.GetRequest("top5", "post", 1, "", ""));
            IdRe[5] = int.Parse(Utils.GetRequest("top6", "post", 1, "", ""));
            IdRe[6] = int.Parse(Utils.GetRequest("top7", "post", 1, "", ""));
            IdRe[7] = int.Parse(Utils.GetRequest("top8", "post", 1, "", ""));
            IdRe[8] = int.Parse(Utils.GetRequest("top9", "post", 1, "", ""));
            IdRe[9] = int.Parse(Utils.GetRequest("top10", "post", 1, "", ""));
            builder.Append(Out.Tab("<div>", ""));


            builder.Append(Out.Tab("</div>", "<br />"));
            for (int i = 0; i < 10; i++)
            {
                if (IdRe[i] != 0)
                {
                    builder.Append("【第" + (i + 1) + "名】：");
                    builder.Append(Top[i] + " 获得：");
                    builder.Append(IdRe[i] + "币<br />");
                    new BCW.BLL.User().UpdateiGold(Top[i], IdRe[i], "捕鱼排行榜奖励");
                    //发内线
                    string strLog = "您在" + DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "到" + DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "的捕鱼排行榜" + "上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + IdRe[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/cmg.aspx]进入捕鱼游戏[/url]";
                    new BCW.BLL.Guest().Add(0, Top[i], new BCW.BLL.User().GetUsName(Top[i]), strLog);
                    //动态
                    string mename = new BCW.BLL.User().GetUsName(Top[i]);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + Top[i] + "]" + mename + "[/url]在[url=/bbs/game/cmg.aspx]捕鱼游戏[/url]" + "上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + IdRe[i] + "" + ub.Get("SiteBz");
                    new BCW.BLL.Action().Add(26, 0, Top[i], mename, wText);
                }
            }
        }
        Utils.Success("捕鱼游戏奖励派发", "派发成功，正在返回..", Utils.getUrl("bydr.aspx?backurl=" + Utils.getPage(0) + ""), "1");
    }
    private void ViewPage()
    {
        Master.Title = "游戏数据设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("游戏数据设置");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "ID错误"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
        string strWhere = string.Empty;
        strWhere = "ID>=300 and ID<=329";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.bydr.Model.CmgDaoju> listcmglist1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaojus(pageIndex, pageSize, strWhere, out recordCount);
        {
            if (listcmglist1.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.CmgDaoju n in listcmglist1)
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
                    string sText = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=daojuset1&amp;id1=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                    if (wd <= 5)
                    {
                        builder.Append("场景1获得币" + " " + n.Changjing + "");
                    }
                    else if (wd > 5 && wd <= 10)
                    {
                        builder.Append("场景2获得币" + " " + n.Changjing + "");
                    }
                    else if (wd > 10 && wd <= 15)
                    {
                        builder.Append("场景3获得币" + " " + n.Changjing + "");
                    }
                    else if (wd > 15 && wd <= 20)
                    {
                        builder.Append("场景4获得币" + " " + n.Changjing + "");
                    }
                    else if (wd > 20 && wd <= 25)
                    {
                        builder.Append("场景5获得币" + " " + n.Changjing + "");
                    }
                    else if (wd > 25 && wd <= 30)
                    {
                        builder.Append("场景6获得币" + " " + n.Changjing + "");
                    }

                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
        }
        //读取随机次数

        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (Utils.ToSChinese(ac) == "确认修改")
        {
            BCW.bydr.Model.CmgDaoju m = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
            string changj2 = Utils.GetRequest("changj2", "post", 2, @"^[^\^]{1,100}$", "格式填写错误");
            m.changj2 = Convert.ToInt64(changj2);
            new BCW.bydr.BLL.CmgDaoju().Update(m);

            Utils.Success("捕鱼游戏设置", "设置成功，正在返回..", Utils.getUrl("bydr.aspx?act=view" + ""), "1");
        }
        else
        {
            BCW.bydr.Model.CmgDaoju n = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
            string strText = "循环次数(大于3次):/,,";
            string strName = "changj2,backurl";
            string strType = "num,hidden";
            string strValu = "" + n.changj2 + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确认修改,bydr.aspx?act=view,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("注意：修改循环次数要相应修改上面的场景获得币，如现在30次循环，上面场景获得币就有30个.另外此修改只适用于由多改少，如需要增加循环次数请联系技术人员<br />");
        builder.Append("功能说明：场景获得币是重循环次数中随机取完的，如场景获得币为233,344,677，循环次数是3次，那么捕鱼的币就是从233,344,677中随机不重复取完，3次一轮回<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void topPage()
    {
        Master.Title = "排行管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("排行管理");
        builder.Append(Out.Tab("</div>", ""));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "1"));


        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小


        string ac = Utils.GetRequest("ac", "all", 1, "", "");


        if (Utils.ToSChinese(ac) == "确认搜索")
        {
            string searchday1 = Utils.GetRequest("stime", "all", 2, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
            string searchday2 = Utils.GetRequest("otime", "all", 2, @"^^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
            //查询条件  

            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "text,text,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br/>"));
            string monthwdy1 = (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd 00:00:00"));
            string monthwdy2 = (DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd 23:59:59"));

            string[] pageValUrl = { "act", "ptype", "backurl", "ac", "stime", "otime" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            DataSet bang = new BCW.bydr.BLL.Cmg_Top().GetListByPage(monthwdy1, monthwdy2);
            recordCount = Convert.ToInt32(bang.Tables[1].Rows[0][0]);
            if (recordCount > 0)
            {
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

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][0]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][1]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("" + wd + ".<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净捕了" + "" + usmoney + "" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    // rewardid = rewardid + usid.ToString() + " ";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "num,num,hidden";
            string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            string monthwdy1 = "";
            string monthwdy2 = "";

            builder.Append(Out.Tab("<div>", "<br/>"));
            string[] pageValUrl = { "act", "ptype", "backurl", "ac" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            DataSet bang = new BCW.bydr.BLL.Cmg_Top().GetListByPage(monthwdy1, monthwdy2);
            recordCount = Convert.ToInt32(bang.Tables[1].Rows[0][0]);
            if (recordCount > 0)
            {
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

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][0]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][1]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("" + wd + ".<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净捕了" + "" + usmoney + "" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    // rewardid = rewardid + usid.ToString() + " ";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void DelPage()
    {
        Master.Title = "确认删除";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=dongtai") + "\">动态管理</a>&gt;删除");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        int id4 = Utils.ParseInt(Utils.GetRequest("id4", "get", 1, @"^[1-9]\d*$", "0"));
        try
        {

            BCW.bydr.Model.Cmg_notes m = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(id);
            builder.Append("您即将删除" + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + m.usID + "") + "\">" + m.usID + "</a>" + "在" + m.Stime.ToString("yyyy-MM-dd HH:mm:ss") + "的动态<br />" + "");

        }
        catch
        {
            builder.Append("没有数据<br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;info=ok&amp;id1=" + id + "&amp;id2=" + id4) + "\">确认删除</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=dongtai") + "\">在看看吧</a><br />");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id1 = Utils.ParseInt(Utils.GetRequest("id1", "get", 1, @"^[1-9]\d*$", "0"));
        int id2 = Utils.ParseInt(Utils.GetRequest("id2", "get", 1, @"^[1-9]\d*$", "0"));
        if (info == "ok")
        {
            if (!new BCW.bydr.BLL.Cmg_notes().Exists(id1))
            {
                builder.Append("不存在的id！！！！<br />");
                Utils.Success("", "正在返回...", Utils.getUrl("bydr.aspx?act=dongtai" + ""), "2");
            }
            else
            {
                new BCW.bydr.BLL.Cmg_notes().Delete(id1);
                Utils.Success("", "删除成功！！正在返回...", Utils.getUrl("bydr.aspx?act=dongtai" + ""), "2");
            }

        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void Del1Page()
    {
        Master.Title = "确认删除";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=top") + "\">排行管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id2 = Utils.ParseInt(Utils.GetRequest("id2", "get", 1, @"^[1-9]\d*$", "0"));
        try
        {
            BCW.bydr.Model.CmgToplist m = new BCW.bydr.BLL.CmgToplist().GetCmgToplist(id2);
            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(m.usID));
            builder.Append("您即将删除" + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + m.usID + "") + "\">" + mename + "</a>" + "的排行记录<br />" + "");
        }
        catch
        {
            builder.Append("没有数据<br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del1&amp;info=ok&amp;id3=" + id2) + "\">确认删除</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=top") + "\">在看看吧</a><br />");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id3 = Utils.ParseInt(Utils.GetRequest("id3", "get", 1, @"^[1-9]\d*$", "0"));
        if (info == "ok")
        {

            if (!new BCW.bydr.BLL.CmgToplist().Exists(id3))
            {
                Utils.Error("不存在id", "");
            }
            else
            {
                new BCW.bydr.BLL.CmgToplist().Delete(id3);
                Utils.Success("", "删除成功！！正在返回...", Utils.getUrl("bydr.aspx?act=top" + ""), "2");
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void DelallPage()
    {
        Master.Title = "确认删除";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=dongtai") + "\">动态管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string stime = Utils.GetRequest("stime", "all", 2, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "开始时间错误");
        string otime = Utils.GetRequest("otime", "all", 2, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "结束时间错误");

        builder.Append("您确认要删除从" + DateTime.ParseExact(stime, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "到" + DateTime.ParseExact(otime, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "的数据？<br />");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=delall&amp;info=ok&amp;stime=" + stime + "&amp;otime=" + otime) + "\">确认删除</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=dongtai") + "\">在看看吧</a><br />");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.bydr.BLL.Cmg_notes().Delete1(stime, otime);
            Utils.Success("", "删除成功！！正在返回...", Utils.getUrl("bydr.aspx?act=dongtai" + ""), "2");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void addJPage()
    {
        Master.Title = "管理兑奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("兑奖管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = string.Empty;
        strWhere = "Bid=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        // 开始读取列表
        IList<BCW.bydr.Model.Cmg_Top> listcmglist = new BCW.bydr.BLL.Cmg_Top().GetCmg_Tops(pageIndex, pageSize, strWhere, out recordCount);
        if (listcmglist.Count > 0)
        {
            int k = 1;

            foreach (BCW.bydr.Model.Cmg_Top n in listcmglist)
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
                int wd = (pageIndex - 1) * 10 + k;
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                builder.Append(wd + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "." + "</a>在" + Convert.ToDateTime(n.Time).ToString("yyyy-MM-dd HH:mm:ss") + "采到了" + Convert.ToInt64(n.ColletGold) + "" + ub.Get("SiteBz") + "," + "编号" + (n.ID) + "<a href=\"" + Utils.getUrl("bydr.aspx?act=alllistadd&amp;pid=" + (n.ID) + "&amp;usid=" + n.usID) + "\">帮TA兑奖</a>");

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



        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void alllistaddPage()
    {
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[0-9]\d*$", "获取id失败"));
        BCW.bydr.Model.Cmg_Top model = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(pid);
        if (model.Bid == 1)
        {
            builder.Append(model.ColletGold);

            new BCW.BLL.User().UpdateiGold(usid, +Convert.ToInt64((model.ColletGold)), "捕鱼达人兑奖(管)—标识id" + pid + "");

            new BCW.bydr.BLL.Cmg_Top().UpdateBid(0, pid, usid);

            Utils.Success("兑奖", "恭喜，成功兑奖" + Convert.ToInt64((model.ColletGold)) + "" + ub.Get("SiteBz") + "", Utils.getUrl("bydr.aspx?act=addJ"), "2");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("bydr.aspx?act=addJ"), "2");
        }
    }

    private void RecordPage(string act)
    {
        Master.Title = "记录管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("记录管理");
        builder.Append(Out.Tab("</div>", ""));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (act == "Record")
        {
            int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

            string strText = "输入用户ID:/,,";
            string strName = "usid,act";
            string strType = "num,hidden";
            string strValu = "" + usid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx?act=Record,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            string strText1 = "输入编号ID:/,,";
            string strName1 = "id,act";
            string strType1 = "num,hidden";
            string strValu1 = "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,false";
            string strIdea1 = "/";
            string strOthe1 = "确认搜索,bydr.aspx?act=list,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));


            strText = "选择场景:/,,";
            strName = "id1,act";
            strType = "select,hidden";
            strValu = "'" + Utils.getPage(0) + "";
            strEmpt = "0|场景1|1|场景2|2|场景3|3|场景4|4|场景5|5|场景6,false";
            strIdea = "/";
            strOthe = "确认搜索,bydr.aspx?act=Record1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
            string strWhere = string.Empty;
            if (usid != 0)
            {
                strWhere = "usID=" + usid + "and jID=1";
            }
            else
            {
                strWhere = " jID=1";
            }

            string[] pageValUrl = { "act", "usid", "ac", "id1", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            builder.Append(Out.Tab("<div>", "<br/>"));
            // 开始读取列表
            IList<BCW.bydr.Model.Cmg_Top> listcmglist = new BCW.bydr.BLL.Cmg_Top().GetCmg_Tops(pageIndex, pageSize, strWhere, out recordCount);
            if (listcmglist.Count > 0)
            {
                int k = 1;
                foreach (BCW.bydr.Model.Cmg_Top n in listcmglist)
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
                    string sText = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                    sText = wd + "." + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + "在" + n.Changj + "捕到了" + n.ColletGold + "" + ub.Get("SiteBz") + "," + "编号" + n.ID;
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("bydr.aspx?act=Top&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + sText);
                    builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=list&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">查看&gt;</a>");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int id1 = int.Parse(Utils.GetRequest("id1", "all", 1, @"^[1-9]\d*$", "0"));
            string strText = "输入用户ID:/,,";
            string strName = "usid,act";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx?act=Record,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            string strText1 = "输入编号ID:/,,";
            string strName1 = "id,act";
            string strType1 = "num,hidden";
            string strValu1 = "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,false";
            string strIdea1 = "/";
            string strOthe1 = "确认搜索,bydr.aspx?act=list,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));


            strText1 = "选择场景:/,,";
            strName1 = "id1,act";
            strType1 = "select,hidden";
            strValu1 = "" + id1 + "'" + Utils.getPage(0) + "";
            strEmpt1 = "0|场景1|1|场景2|2|场景3|3|场景4|4|场景5|5|场景6,false";
            strIdea1 = "/";
            strOthe1 = "确认搜索,bydr.aspx?act=Record1,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));

            string s = "";
            DataSet ds = new BCW.bydr.BLL.CmgDaoju().GetList("Changjing", "Xiaoxi=0 and Tianyuan=0 ORDER BY changj2 ASC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                s = Convert.ToString(ds.Tables[0].Rows[id1][0]);
            }

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
            string strWhere = string.Empty;
            strWhere = "Changj='" + s + "'";
            string[] pageValUrl = { "act", "usid", "id1", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            builder.Append(Out.Tab("<div>", "<br/>"));
            // 开始读取列表
            IList<BCW.bydr.Model.Cmg_Top> listcmglist = new BCW.bydr.BLL.Cmg_Top().GetCmg_Tops(pageIndex, pageSize, strWhere, out recordCount);
            if (listcmglist.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.Cmg_Top n in listcmglist)
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
                    string sText = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                    sText = wd + "." + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + "在" + n.Changj + "捕到了" + n.ColletGold + "" + ub.Get("SiteBz") + "," + "编号" + n.ID + "循环次数" + n.randnum;
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("bydr.aspx?act=Top&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + sText);
                    builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=list&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">查看&gt;</a>");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void listPage()
    {
        Master.Title = "记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=Record") + "\">记录管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.bydr.Model.Cmg_Top model = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.bydr.Model.Cmg_Top model1 = null;
        string[] randaoju = new string[] { };
        string[] randyu = new string[] { };
        try
        {
            model1 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id);
            randaoju = model1.randdaoju.Split(',');
            randyu = model1.randyuID.Split(',');
        }
        catch
        {
            builder.Append("数据获取异常！");
        }

        int s = 0;
        try
        {
            for (int i = 0; i < 10; i++)
            {
                s += Convert.ToInt32(randaoju[i]);
            }
        }
        catch
        {
            builder.Append(" ");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=捕鱼详情=" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model1.usID));
        builder.Append(" " + "玩家：" + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + model1.usID + "") + "\">" + mename + "(" + model1.usID + ")" + "</a>" + "<br />");
        builder.Append(" " + "编号：" + id + "<br />");
        builder.Append(" " + "场景：" + model1.Changj + "<br />");
        builder.Append(" " + "结束时间：" + Convert.ToDateTime(model1.Time).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append(" " + "共采酷币：" + model1.ColletGold + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=采集到的物品=" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        try
        {
            for (int i = 0; i < 10; i++)
            {
                s += Convert.ToInt32(randaoju[i]);
            }
        }
        catch
        {
            try
            {
                for (int i = 1; i < 11; i++)
                {
                    DataSet rows91 = null;
                    if (i == 10)
                    {
                        rows91 = new BCW.bydr.BLL.Cmg_notes().GetList("ID", "usID=" + model1.usID + "and stype=0 and cxid=" + model1.Bid);
                    }
                    else
                    {
                        rows91 = new BCW.bydr.BLL.Cmg_notes().GetList("ID", "usID=" + model1.usID + "and stype=" + i + "and cxid=" + model1.Bid);
                    }
                    BCW.bydr.Model.Cmg_notes model2 = null;
                    int id21 = 0;
                    try
                    {
                        id21 = Convert.ToInt32(rows91.Tables[0].Rows[0][0]);
                        model2 = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(Convert.ToInt32(id21));
                    }
                    catch
                    {
                        DataSet rows9 = new BCW.bydr.BLL.Cmg_notes().GetList("ID", "usID=" + model1.usID + "and stype=11 and cxid=" + model1.Bid);
                        id21 = Convert.ToInt32(rows9.Tables[0].Rows[0][0]);
                        model2 = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(Convert.ToInt32(id21));
                    }
                    if (model2 == null)
                        Utils.Error("不存在的记录", "");
                    else
                    {
                        builder.Append(model2.coID + "+" + (model2.AcolletGold) + ub.Get("SiteBz") + "<br />");
                    }
                }
            }
            catch
            {
                builder.Append("没有数据！");
            }
        }
        for (int i = 0; i < 10; i++)
        {
            if (randaoju[i] == "" || randyu[i] == "")
            {
                builder.Append("没有数据！！");
                break;
            }
            else
            {
                if (Convert.ToInt32(randaoju[i]) == 0)
                    continue;
                builder.Append(new BCW.bydr.BLL.CmgDaoju().GetyuName(Convert.ToInt32(randyu[i])) + "+" + randaoju[i] + ub.Get("SiteBz") + "<br />");
            }
        }
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=Record") + "\">返回<br /></a>");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置捕鱼游戏吗，重置后，将重新从新开始，所有记录和排行全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.bydr.BLL.CmgToplist().ClearTable("tb_Cmg_notes");
            new BCW.bydr.BLL.CmgToplist().ClearTable("tb_Cmg_Top");
            new BCW.bydr.BLL.CmgToplist().ClearTable("tb_CmgTicket");
            new BCW.bydr.BLL.CmgToplist().ClearTable("tb_CmgToplist");
            new BCW.bydr.BLL.CmgToplist().ClearTable("tb_Cmg_buyuDonation");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("bydr.aspx"), "1");
        }
    }

    //盈利分析
    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        double Tar1 = Convert.ToDouble(ub.GetSub("bydrTar1", xmlPath));
        double Tar2 = Convert.ToDouble(ub.GetSub("bydrTar2", xmlPath));
        double Tar3 = Convert.ToDouble(ub.GetSub("bydrTar3", xmlPath));
        double buyuprofit1 = Convert.ToDouble(ub.GetSub("buyuprofit", "/Controls/BYDR.xml"));
        double buyuprofit = 0;
        try
        {
            buyuprofit = buyuprofit1 / 1000;
        }
        catch
        {
            buyuprofit = 0;
        }
        //今天赢利
        long Tar = new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGoldday(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
        if (Tar > 0)
            Tar1 = Convert.ToInt32(buyuprofit * Tar / (1 - buyuprofit));
        else
            Tar1 = Convert.ToInt32(buyuprofit * (1 - Tar + 1) / (1 - buyuprofit));

        //本月赢利
        string strdatetime1 = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00";
        string strdatetime2 = DateTime.Parse(strdatetime1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
        long Tar4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGoldmonth1(strdatetime1, strdatetime2);
        if (Tar > 0)
            Tar2 = Convert.ToInt32(buyuprofit * Tar4 / (1 - buyuprofit));
        else
            Tar2 = Convert.ToInt32(buyuprofit * (1 - Tar4 + 1) / (1 - buyuprofit));

        //总赢利

        long Tar5 = new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold2();
        if (Tar > 0)
            Tar3 = Convert.ToInt32(buyuprofit * Tar5 / (1 - buyuprofit));
        else
            Tar3 = Convert.ToInt32(buyuprofit * (1 - Tar5 + 1) / (1 - buyuprofit));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利：" + (Tar1 - Tar) + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("本月赢利：" + (Tar2 - Tar4) + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("赢利总计：" + "" + (Tar3 - Tar5) + ub.Get("SiteBz") + "<br />");


        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //上帝之手
    private void UserPage()
    {
        Master.Title = "上帝之手";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("上帝之手");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：" + "添加特殊ID需要玩一次捕鱼游戏.<br />");
        builder.Append(Out.Tab("</div>", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
        string strWhere = string.Empty;
        strWhere = "sid=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.bydr.Model.CmgToplist> listcmglist1 = new BCW.bydr.BLL.CmgToplist().GetCmgToplists(pageIndex, pageSize, strWhere, out recordCount);
        {
            if (listcmglist1.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.CmgToplist n in listcmglist1)
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
                    string sText = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                    sText = wd + "." + mename + "(" + n.usID + ") ";
                    builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=userset&amp;id=" + n.usID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[删除]</a>");
                    builder.AppendFormat(sText + "<a href=\"" + Utils.getUrl("bydr.aspx?act=userset&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
            int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
            string strText = "输入添加特殊ID:/,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确认添加,bydr.aspx?act=userAdd,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //上帝之手删除
    private void UsersetPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        bool yy = new BCW.bydr.BLL.CmgToplist().ExistsusID(id);
        if (yy == true)
        {
            new BCW.bydr.BLL.CmgToplist().Updatesid(id, 0);
            Utils.Success("上帝之手删除", "删除成功，正在返回..", Utils.getUrl("bydr.aspx?act=user" + ""), "1");
        }
        else
            Utils.Error("不存在的ID", "");
    }
    //上帝之手添加
    private void UserAddPage()
    {
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "ID错误"));
        bool yy = new BCW.bydr.BLL.CmgToplist().ExistsusID(uid);
        if (yy == true)
        {
            new BCW.bydr.BLL.CmgToplist().Updatesid(uid, 1);
            Utils.Success("上帝之手添加", "添加成功，正在返回..", Utils.getUrl("bydr.aspx?act=user" + ""), "1");
        }
        else
            Utils.Error("不存在的ID", "");
    }

    //返负设置
    private void BackPage()
    {
        Master.Title = "返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("返负");
        builder.Append(Out.Tab("</div>", ""));


        string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "date,date,num,num,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返负,bydr.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BackSavePage()
    {
        Master.Title = "返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("返负");
        builder.Append(Out.Tab("</div>", ""));
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("usID,sum(AllcolletGold) as AllcolletGold", "Time>='" + sTime + "'and Time<'" + oTime + "' and Jid=1 group by usID");
        builder.Append(Out.Tab("<div>", "<br />"));

        builder.Append("返负时间段：" + sTime + "到" + oTime + "<br />");
        builder.Append("返负千分比：" + iTar + "<br />");
        builder.Append("至少：" + iPrice + "币返<br />");

        builder.Append(Out.Tab("</div>", ""));
        string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + DT.FormatDate(sTime.AddDays(-10), 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave1";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返负,bydr.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackSave1Page()
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        string xiaoxi = Convert.ToString(ub.GetSub("bydrxiaoxi", xmlPath));
        DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("usID,sum(AllcolletGold) as AllcolletGold", "Time>='" + sTime + "'and Time<'" + oTime + "' and Jid=1 group by usID");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["AllcolletGold"]);
            if (Cents < 0 && Cents < (-iPrice))
            {
                int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["usID"]);
                long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                new BCW.BLL.User().UpdateiGold(usid, cent, "捕鱼达人返负");
                //发内线           
                string strLog = xiaoxi + cent + "" + ub.Get("SiteBz");
                string mename = new BCW.BLL.User().GetUsName(usid);
                new BCW.BLL.Guest().Add(0, usid, mename, strLog);
            }
        }
        Utils.Success("返负操作", "返负操作成功", Utils.getUrl("bydr.aspx"), "1");
    }
    private void DaojuPage()
    {
        Master.Title = "道具管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("道具管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
        string strWhere = string.Empty;

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("普通用户道具管理|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=daoju&amp;ptype=0&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">普通用户道具管理</a>|");
        if (ptype == 1)
            builder.Append("特殊用户道具管理");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=daoju&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">特殊用户道具管理</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));


        if (ptype == 0)
        {
            strWhere = "ID>12";
        }
        else
            strWhere = "ID>=7 and ID<=12";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.bydr.Model.CmgDaoju> listcmglist1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaojus(pageIndex, pageSize, strWhere, out recordCount);
        {
            if (listcmglist1.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.CmgDaoju n in listcmglist1)
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
                    string sText = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=daojuset&amp;id1=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=daoju&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + wd + n.Daoju + " " + n.Tianyuan + " " + n.Xiaoxi + " " + n.Heliu + " " + n.Caoc + " " + n.Huayuan + " " + n.Senlin + " " + n.changj1 + " " + n.Changjing + "");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
        }
        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getPage("bydr.aspx?act=daojuadd") + "\">添加</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DaojusetPage()
    {

        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (Utils.ToSChinese(ac) == "确认修改")
        {

            int id1 = Utils.ParseInt(Utils.GetRequest("id1", "post", 1, @"^[0-9]\d*$", "ID错误"));
            BCW.bydr.Model.CmgDaoju m = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(id1);

            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,200}$", "道具名称限1-200字内");
            string Tianyuan = Utils.GetRequest("Tianyuan", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Xiaoxi = Utils.GetRequest("Xiaoxi", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Heliu = Utils.GetRequest("Heliu", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Caoc = Utils.GetRequest("Caoc", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Huayuan = Utils.GetRequest("Huayuan", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Senlin = Utils.GetRequest("Senlin", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string changj1 = Utils.GetRequest("changj1", "post", 2, @"^[^\^]{1,100}$", "格式填写错误");
            string Changjing = Utils.GetRequest("Changjing", "post", 2, @"^[^\^]{1,200}$", "口号限2000字内");

            m.Daoju = Name;
            m.Tianyuan = Convert.ToInt64(Tianyuan);
            m.Xiaoxi = Convert.ToInt64(Xiaoxi);
            m.Heliu = Convert.ToInt64(Heliu);
            m.Caoc = Convert.ToInt64(Caoc);
            m.Huayuan = Convert.ToInt64(Huayuan);
            m.Senlin = Convert.ToInt64(Senlin);
            m.changj1 = changj1;
            m.Changjing = Changjing;
            new BCW.bydr.BLL.CmgDaoju().Update(m);

            Utils.Success("捕鱼游戏设置", "设置成功，正在返回..", Utils.getUrl("bydr.aspx?act=daoju" + ""), "1");
        }
        else
        {
            Master.Title = "道具修改";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;<a href=\"" + Utils.getUrl("bydr.aspx?act=daoju") + "\">道具管理</a>&gt;");
            builder.Append("道具修改");
            builder.Append(Out.Tab("</div>", ""));

            int id1 = Utils.ParseInt(Utils.GetRequest("id1", "all", 1, @"^[0-9]\d*$", "ID错误"));
            BCW.bydr.Model.CmgDaoju n = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(id1);
            string strText = "道具名称:/,场景1:/,场景2:/,场景3:/,场景4:/,场景5:/,场景6:/,图片名称：注意图片名称不能相同(x.gif):/,口号语句:/,,";
            string strName = "Name,Tianyuan,Xiaoxi,Heliu,Caoc,Huayuan,Senlin,changj1,Changjing,id1,backurl";
            string strType = "text,text,text,text,text,num,text,text,textarea,hidden,hidden";
            string strValu = "" + n.Daoju + "'" + n.Tianyuan + "'" + n.Xiaoxi + "'" + n.Heliu + "'" + n.Caoc + "'" + n.Huayuan + "'" + n.Senlin + "'" + n.changj1 + "'" + n.Changjing + "'" + id1 + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,true,false";
            string strIdea = "/";
            string strOthe = "确认修改,bydr.aspx?act=daojuset,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void Daojuset1Page()
    {

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确认修改")
        {
            int id1 = Utils.ParseInt(Utils.GetRequest("id1", "post", 1, @"^[0-9]\d*$", "ID错误"));
            BCW.bydr.Model.CmgDaoju m = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(id1);


            string Changjing = Utils.GetRequest("Changjing", "post", 2, @"^[^\^]{1,200}$", "口号限2000字内");
            m.Changjing = Changjing;
            new BCW.bydr.BLL.CmgDaoju().Update(m);

            Utils.Success("捕鱼游戏设置", "设置成功，正在返回..", Utils.getUrl("bydr.aspx?act=view" + ""), "1");
        }
        else
        {
            Master.Title = "游戏数据管理";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;<a href=\"" + Utils.getUrl("bydr.aspx?act=view") + "\">游戏数据设置</a>&gt;管理");
            builder.Append(Out.Tab("</div>", ""));

            int id1 = Utils.ParseInt(Utils.GetRequest("id1", "all", 1, @"^[0-9]\d*$", "ID错误"));
            BCW.bydr.Model.CmgDaoju n = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(id1);
            string strText = "捕鱼币设置:/,,";
            string strName = "Changjing,id1,backurl";
            string strType = "big,hidden,hidden";
            string strValu = "" + n.Changjing + "'" + id1 + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,false";
            string strIdea = "/";
            string strOthe = "确认修改,bydr.aspx?act=daojuset1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DaojuaddPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (Utils.ToSChinese(ac) == "确认添加")
        {
            int maxID = new BCW.bydr.BLL.CmgDaoju().GetMaxId();
            BCW.bydr.Model.CmgDaoju m = new BCW.bydr.Model.CmgDaoju();

            string Name = Utils.GetRequest("Name", "post", 1, @"^[^\^]{1,200}$", "道具名称限1-200字内");
            string Tianyuan = Utils.GetRequest("Tianyuan", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Xiaoxi = Utils.GetRequest("Xiaoxi", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Heliu = Utils.GetRequest("Heliu", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Caoc = Utils.GetRequest("Caoc", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Huayuan = Utils.GetRequest("Huayuan", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string Senlin = Utils.GetRequest("Senlin", "post", 2, @"^[0-9]\d*$", "价值填写错误");
            string changj1 = Utils.GetRequest("changj1", "post", 2, @"^[^\^]{1,100}$", "格式填写错误");
            string Changjing = Utils.GetRequest("Changjing", "post", 2, @"^[^\^]{1,200}$", "口号限2000字内");
            m.Daoju = Name;
            m.Tianyuan = Convert.ToInt64(Tianyuan);
            m.Xiaoxi = Convert.ToInt64(Xiaoxi);
            m.Heliu = Convert.ToInt64(Heliu);
            m.Caoc = Convert.ToInt64(Caoc);
            m.Huayuan = Convert.ToInt64(Huayuan);
            m.Senlin = Convert.ToInt64(Senlin);
            m.changj1 = changj1;
            m.Changjing = Changjing;
            new BCW.bydr.BLL.CmgDaoju().Add(m);

            Utils.Success("捕鱼游戏设置", "添加成功，正在返回..", Utils.getUrl("bydr.aspx?act=daoju" + ""), "1");
        }
        else
        {
            int maxID = new BCW.bydr.BLL.CmgDaoju().GetMaxId();
            BCW.bydr.Model.CmgDaoju n = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(maxID - 1);
            string strText = "道具名称:/,场景1:/,场景2:/,场景3:/,场景4:/,场景5:/,场景6:/,图片名称：注意图片名称不能相同(x.gif):/,口号语句:/,,";
            string strName = "Name,Tianyuan,Xiaoxi,Heliu,Caoc,Huayuan,Senlin,changj1,Changjing,id1,backurl";
            string strType = "text,text,text,text,text,num,text,text,textarea,hidden,hidden";
            string strValu = "" + n.Daoju + "'" + n.Tianyuan + "'" + n.Xiaoxi + "'" + n.Heliu + "'" + n.Caoc + "'" + n.Huayuan + "'" + n.Senlin + "'" + n.changj1 + "'" + n.Changjing + "'" + (maxID - 1) + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认添加,bydr.aspx?act=daojuadd,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");
        }
        builder.Append("<a href=\"" + Utils.getPage("bydr.aspx?act=daoju") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void dongtaiPage()
    {
        Master.Title = "动态管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx") + "\">捕鱼管理</a>&gt;");
        builder.Append("动态管理");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");


        if (Utils.ToSChinese(ac) == "确认搜索")
        {
            string searchday1 = Utils.GetRequest("stime", "all", 2, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
            string searchday2 = Utils.GetRequest("otime", "all", 2, @"^^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
            //查询条件  

            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "num,num,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx?act=dongtai,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            int pageIndex = 0;//当前页
            int recordCount;//记录总条数
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
            string strWhere = "AcolletGold!=0 and Stime>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Stime<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'";
            //  string strWhere1 = "jID=1 and DcolletGold=10";
            string[] pageValUrl = { "act", "stime", "otime", "ac", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            // 开始读取列表
            IList<BCW.bydr.Model.Cmg_notes> listcmglist = new BCW.bydr.BLL.Cmg_notes().GetCmg_notess(pageIndex, pageSize, strWhere, out recordCount);
            if (listcmglist.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.Cmg_notes n in listcmglist)
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
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                    TimeSpan time = DateTime.Now - n.Stime;
                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;

                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                        else if (d == 0 && e == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                        else if (d == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append(e + "分前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                        else
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append(d + "小时前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在:" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                        builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                    }
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
        }
        else
        {
            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "num,num,hidden";
            string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,bydr.aspx?act=dongtai,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br/>"));
            int pageIndex = 0;//当前页
            int recordCount;//记录总条数
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
            string strWhere = "AcolletGold!=0 and Stime>='" + (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Stime<='" + (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // 开始读取列表
            IList<BCW.bydr.Model.Cmg_notes> listcmglist = new BCW.bydr.BLL.Cmg_notes().GetCmg_notess(pageIndex, pageSize, strWhere, out recordCount);
            if (listcmglist.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.Cmg_notes n in listcmglist)
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
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                    TimeSpan time = DateTime.Now - n.Stime;
                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;

                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                        else if (d == 0 && e == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                        else if (d == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append(e + "分前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                        else
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                            builder.Append(d + "小时前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在:" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                        }
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=del&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">[管理]</a>");
                        builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + n.coID + n.AcolletGold + "币" + "");

                    }
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
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("bydr.aspx?act=delall&amp;stime=" + DateTime.Now.ToString("yyyyMMdd") + "&amp;otime=" + DateTime.Now.ToString("yyyyMMdd")) + "\">管理全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
