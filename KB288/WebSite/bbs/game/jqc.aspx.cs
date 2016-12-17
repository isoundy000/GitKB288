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


public partial class bbs_game_jqc : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/jqc.xml";
    protected string GameName = ub.GetSub("GameName", "/Controls/jqc.xml");//游戏名字
    protected string XtestID = (ub.GetSub("testID", "/Controls/jqc.xml"));//试玩ID
    protected long jiangchi = Convert.ToInt64(ub.GetSub("jiangchi", "/Controls/jqc.xml"));//奖池金额
    protected long jiangchi_guding = Convert.ToInt64(ub.GetSub("jiangchi_guding", "/Controls/jqc.xml"));

    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("JQCStatus", xmlPath) == "1")//维护
        {
            Utils.Safe("此游戏");
        }

        if (ub.GetSub("JQCStatus", xmlPath) == "2")//内测
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
            int sbsy = 0;
            if (XtestID != "")
            {
                string[] sNum = XtestID.Split('#');
                for (int a = 0; a < sNum.Length; a++)
                {
                    int tid = 0;
                    int.TryParse(sNum[a].Trim(), out tid);
                    if (meid == tid)
                    {
                        sbsy++;
                    }
                }
                if (sbsy == 0)
                {
                    Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
                }
            }
        }


        //检查奖池
        check_jiangchi();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "pay":
                PayPage();//下注
                break;
            case "case":
                CasePage();//兑奖页面
                break;
            case "caseok":
                CaseOkPage();//单个兑奖
                break;
            case "casepost":
                CasePostPage();//多个兑奖
                break;
            case "top":
                TopPage();//排行
                break;
            case "rule":
                RulePage();//规则
                break;
            case "list":
                ListPage();//历史开奖
                break;
            case "listview":
                ListViewPage();//历史开奖——详细情况
                break;
            case "view":
                ListViewPage2();//消费记录详细
                break;
            case "mylist":
                MyListPage();//我的投注历史
                break;
            case "mylistview":
                MyListViewPage();//我的投注历史——详细情况
                break;
            default:
                ReloadPage();//首页
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\"><h style=\"color:red\">首页</h></a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist") + "\">历史</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        string where1 = "where  Result='' order by id ASC";//where phase!='' and Sale_Start < getdate() and  Sale_End > getdate() and Result='' order by id desc
        string where2 = "ORDER BY phase desc";
        BCW.JQC.Model.JQC_Internet hu = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where1);
        if (hu.phase == 0)//如果全部开奖
        {
            BCW.JQC.Model.JQC_Internet hu1 = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where2);
            builder.Append("当前奖池:" + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(hu1.phase + 1) + "" + ub.Get("SiteBz") + "<br/>");//已滚存
        }
        else//如果还有没开奖的
        {
            builder.Append("第" + hu.phase + "期的奖池:" + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(hu.phase) + "" + ub.Get("SiteBz") + "<br/>");
        }

        BCW.JQC.Model.JQC_Bet a_zj = new BCW.JQC.BLL.JQC_Bet().GetNC_suiji();
        try
        {
            builder.Append("中奖信息:<b style=\"color:red\">恭喜" + new BCW.BLL.User().GetUsName(a_zj.UsID) + "在" + a_zj.Lottery_issue + "期获得" + a_zj.GetMoney + "" + ub.Get("SiteBz") + ".</b><br/>");//(" + a_zj.UsID + ")
        }
        catch
        {
            builder.Append("中奖信息:暂无.<br/>");
        }
        builder.Append(Out.Tab("</div>", ""));

        //builder.Append("<table style=\"border-collapse:collapse;align-text:center;border:solid 1px black;padding:10px;border:solid 1px black;text-align:center;\" />");
        //builder.Append("<tr style=\"padding:10px;border:solid 1px black;text-align:center;\" />");
        //builder.Append("<td style=\"padding:10px;border:solid 1px black;text-align:center;\" />");
        builder.Append("<style>table{border-collapse:collapse;align-text:center;border:solid 1px black;}table tr td{padding:3px;border:solid 1px black;text-align:center;}</style>");

        int ye = 0;
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        DataSet qi = new BCW.JQC.BLL.JQC_Internet().GetList("*", " Sale_Start < getdate() and  Sale_End > getdate() Order by phase asc");
        if (qi != null && qi.Tables[0].Rows.Count > 0)
        {
            ye = 1;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            for (int i2 = 0; i2 < qi.Tables[0].Rows.Count; i2++)
            {
                int phase = int.Parse(qi.Tables[0].Rows[i2]["phase"].ToString());
                if (i2 == 0 && id == 0)
                {
                    id = phase;
                }
                if (id == phase)
                {
                    builder.Append("<a href =\"" + Utils.getUrl("jqc.aspx?id=" + phase + "") + "\"><b style=\"color:red\">" + phase + "期</b></a> | ");
                }
                else
                    builder.Append("<a href =\"" + Utils.getUrl("jqc.aspx?id=" + phase + "") + "\">" + phase + "期</a> | ");

            }
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        DataSet ds = new BCW.JQC.BLL.JQC_Internet().GetList("*", "phase=" + id + " and Sale_Start < getdate() and  Sale_End > getdate() Order by phase asc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
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
                int a_putgoid = 0; int a_zhushu = 0;
                int phase = int.Parse(ds.Tables[0].Rows[i]["phase"].ToString());
                DateTime Sale_Start = Convert.ToDateTime(ds.Tables[0].Rows[i]["Sale_Start"]);
                DateTime Sale_End = Convert.ToDateTime(ds.Tables[0].Rows[i]["Sale_End"]);
                builder.Append("第" + phase + "期.");
                try
                {
                    builder.Append("起售时间:" + DT.FormatDate(Sale_Start, 0) + "<br/>");
                }
                catch
                {
                    builder.Append("起售时间:[时间出错！]<br/>");
                }
                string ab = string.Empty;
                string JQC_time = string.Empty;
                try
                {
                    JQC_time = new BCW.JS.somejs().newDaojishi(ab + i, Sale_End);
                }
                catch
                {
                    JQC_time = "[时间出错！]";
                }
                builder.Append("截止还有:" + JQC_time + "<br/>");
                //该期下注注数
                DataSet zhu = new BCW.JQC.BLL.JQC_Bet().GetList("SUM(Zhu*VoteRate) as Zhu", "Lottery_issue='" + phase + "'");
                try
                {
                    a_zhushu = int.Parse(zhu.Tables[0].Rows[0]["Zhu"].ToString());
                }
                catch
                {
                    a_zhushu = 0;
                }
                //该期下注金额
                DataSet putgoid = new BCW.JQC.BLL.JQC_Bet().GetList("SUM(PutGold) as PutGold", "Lottery_issue='" + phase + "'");
                try
                {
                    a_putgoid = int.Parse(putgoid.Tables[0].Rows[0]["PutGold"].ToString());
                }
                catch
                {
                    a_putgoid = 0;
                }
                builder.Append("本期已下注:" + a_putgoid + "" + ub.Get("SiteBz") + "(" + a_zhushu + "注)");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append("<form id=\"form1\" method=\"post\" action=\"jqc.aspx\">");
                string where = "where phase=" + phase + "";
                BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where);
                string[] Match = model.Match.Split(",".ToCharArray());//赛事
                string[] Team_Home = model.Team_Home.Split(',');//主场
                string[] Team_way = model.Team_Away.Split(',');//客场
                string[] Start_Time = model.Start_Time.Split(',');//开始时间

                //判断是否为手机
                string agent = (Request.UserAgent + "").ToLower().Trim();
                if (agent == "" ||
                    agent.IndexOf("mobile") != -1 ||
                    agent.IndexOf("mobi") != -1 ||
                    agent.IndexOf("nokia") != -1 ||
                    agent.IndexOf("samsung") != -1 ||
                    agent.IndexOf("sonyericsson") != -1 ||
                    agent.IndexOf("mot") != -1 ||
                    agent.IndexOf("blackberry") != -1 ||
                    agent.IndexOf("lg") != -1 ||
                    agent.IndexOf("htc") != -1 ||
                    agent.IndexOf("j2me") != -1 ||
                    agent.IndexOf("ucweb") != -1 ||
                    agent.IndexOf("opera mini") != -1 ||
                    agent.IndexOf("mobi") != -1 ||
                    agent.IndexOf("android") != -1 ||
                    agent.IndexOf("iphone") != -1)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        try
                        {
                            builder.Append(j + 1 + ".[" + DT.FormatDate(Convert.ToDateTime(Start_Time[j]), 1) + "]");
                        }
                        catch
                        {
                            builder.Append(j + 1 + ".[时间出错！]");
                        }
                        builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "<br/>");
                        builder.Append("主队:0<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0\" /> ");
                        builder.Append("1<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1\" /> ");
                        builder.Append("2<input type=\"checkbox\" name=\"Num" + j + "\" value=\"2\" /> ");
                        builder.Append("3+<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3\" /> ");
                        builder.Append("客队:0<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"0\" /> ");
                        builder.Append("1<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"1\" /> ");
                        builder.Append("2<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"2\" /> ");
                        builder.Append("3+<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"3\" /> ");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<table>");
                    builder.Append("<tr><td>[场次]</td><td>[赛事]</td><td>[比赛时间]</td><td>[主队]VS[客队]</td><td>[主队进球数]</td><td>[客队进球数]</td></tr>");
                    for (int j = 0; j < 4; j++)
                    {
                        DateTime time1 = DateTime.Now;
                        try
                        {
                            time1 = (Convert.ToDateTime(Start_Time[j]));
                        }
                        catch
                        { }
                        builder.Append("<tr><td>" + (j + 1) + "</td><td>" + Match[j] + "</td><td>" + DT.FormatDate(time1, 1) + "</td><td>" + Team_Home[j] + "VS" + Team_way[j] + "</td><td>");
                        builder.Append("0<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0\" /> ");
                        builder.Append("1<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1\" /> ");
                        builder.Append("2<input type=\"checkbox\" name=\"Num" + j + "\" value=\"2\" /> ");
                        builder.Append("3+<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3\" /></td>");

                        builder.Append("<td>0<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"0\" /> ");
                        builder.Append("1<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"1\" /> ");
                        builder.Append("2<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"2\" /> ");
                        builder.Append("3+<input type=\"checkbox\" name=\"Num" + j + "" + j + "\" value=\"3\" /></td></tr>");
                    }
                    builder.Append("</table>");
                    builder.Append(Out.Tab("</div>", ""));
                }

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input type=\"hidden\" name=\"phase\" value=\"" + phase + "\" />");
                builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"下一步\"/><br />");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无比赛.请等待开售.");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓历史开奖〓<br />");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = 3;
        string strWhere = string.Empty;
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        strWhere = "Result!='' ";
        IList<BCW.JQC.Model.JQC_Internet> listjqclist = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internets(pageIndex, pageSize, strWhere, out recordCount);
        if (listjqclist.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Internet n in listjqclist)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.AppendFormat("{0}期:<a href=\"" + Utils.getUrl("JQC.aspx?act=listview&amp;id=" + n.ID + "") + "\">{1}</a>", n.phase, n.Result);
                builder.Append(Out.Tab("</div>", "<br />"));
                k++;
            }
            if (k > pageSize)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("JQC.aspx?act=list") + "\">更多开奖&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无开奖记录.");
            builder.Append(Out.Tab("</div>", ""));
        }


        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("〓玩家动态〓");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取动态列表
        int SizeNum_Action = 3;
        string strWhere_Action = "Types=1014";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum_Action, strWhere_Action);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum_Action)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=1014") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无动态记录.");
            builder.Append(Out.Tab("</div>", ""));
        }

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(35, "JQC.aspx", 5, 0)));
        GameFoot();
    }

    //下注
    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "post", 1, @"^[0-1]$", "0"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        long gold = new BCW.BLL.User().GetGold(meid);//个人酷币
        int zhuPrice = Convert.ToInt32(ub.GetSub("zhuPrice", xmlPath));//每注酷币

        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>&gt;投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        string Num1 = Utils.GetRequest("Num0", "post", 2, @"^[\d((,)\d)?]+$", "第1场主队未投注");
        string Num2 = Utils.GetRequest("Num00", "post", 2, @"^[\d((,)\d)?]+$", "第1场客队未投注");
        string Num3 = Utils.GetRequest("Num1", "post", 2, @"^[\d((,)\d)?]+$", "第2场主队未投注");
        string Num4 = Utils.GetRequest("Num11", "post", 2, @"^[\d((,)\d)?]+$", "第2场客队未投注");
        string Num5 = Utils.GetRequest("Num2", "post", 2, @"^[\d((,)\d)?]+$", "第3场主队未投注");
        string Num6 = Utils.GetRequest("Num22", "post", 2, @"^[\d((,)\d)?]+$", "第3场客队未投注");
        string Num7 = Utils.GetRequest("Num3", "post", 2, @"^[\d((,)\d)?]+$", "第4场主队未投注");
        string Num8 = Utils.GetRequest("Num33", "post", 2, @"^[\d((,)\d)?]+$", "第4场客队未投注");
        int phase = Utils.ParseInt(Utils.GetRequest("phase", "all", 2, @"^[\d]{7}", "选择投注期号出错"));

        string[] str1 = Num1.Split(',');
        string[] str2 = Num2.Split(',');
        string[] str3 = Num3.Split(',');
        string[] str4 = Num4.Split(',');
        string[] str5 = Num5.Split(',');
        string[] str6 = Num6.Split(',');
        string[] str7 = Num7.Split(',');
        string[] str8 = Num8.Split(',');

        //投注个数
        int zhu = str1.Length * str2.Length * str3.Length * str4.Length * str5.Length * str6.Length * str7.Length * str8.Length;

        if (info == "ok2")
        {
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "投注额填写错误"));
            int zhu2 = Utils.ParseInt(Utils.GetRequest("zhu", "post", 2, @"^[1-9]\d*$", "投注注数填写错误"));
            int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "post", 2, @"^[1-9]\d*$", "投注倍率输入错误"));

            if (gold < Price)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }
            //个人每期限投
            long xPrices = Utils.ParseInt64(ub.GetSub("BigPrice", xmlPath));
            if (xPrices > 0)
            {
                long oPrices = 0;
                DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + phase + "'");
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    int drs = int.Parse(dr[0].ToString());
                    oPrices = oPrices + drs;
                }
                if (oPrices + Price > xPrices)
                {
                    if (oPrices >= xPrices)
                        Utils.Error("您本期下注已达上限，请等待下期...", "");
                    else
                        Utils.Error("您本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                }
            }

            //支付安全提示
            string[] p_pageArr = { "Price", "zhu", "peilv", "phase", "Num0", "Num00", "Num1", "Num11", "Num2", "Num22", "Num3", "Num33", "act", "info" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            //防刷
            int Expir = Utils.ParseInt(ub.GetSub("Expir", xmlPath));
            BCW.User.Users.IsFresh("jqc", Expir);

            BCW.JQC.Model.JQC_Bet model_all = new BCW.JQC.Model.JQC_Bet();
            model_all.UsID = meid;
            model_all.Lottery_issue = phase;
            model_all.PutGold = Price;
            model_all.GetMoney = 0;
            model_all.isRobot = 0;
            model_all.State = 0;
            model_all.Zhu_money = zhuPrice;
            model_all.Zhu = zhu2;
            model_all.VoteRate = peilv;
            model_all.VoteNum = Num1 + "#" + Num2 + "#" + Num3 + "#" + Num4 + "#" + Num5 + "#" + Num6 + "#" + Num7 + "#" + Num8;
            model_all.Input_Time = DateTime.Now;
            int a = new BCW.JQC.BLL.JQC_Bet().Add(model_all);
            //[url=./game/jqc.aspx?act=view&amp;id=" + phase + "]" + phase + "[/url]
            //消费记录
            new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -Price, "进球彩第[url=./game/jqc.aspx?act=view&amp;id=" + phase + "]" + phase + "[/url]期下注[" + model_all.VoteNum + "]-标识ID" + a + "");
            //奖池
            BCW.JQC.Model.JQC_Jackpot jack = new BCW.JQC.Model.JQC_Jackpot();
            jack.UsID = meid;
            jack.InPrize = Price;
            jack.OutPrize = 0;
            jack.AddTime = DateTime.Now;
            jack.type = 0;//玩家下注
            jack.BetID = a;
            jack.phase = phase;
            jack.Jackpot = Price + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(phase);
            new BCW.JQC.BLL.JQC_Jackpot().Add(jack);
            ////判断奖池是否为开奖的那期，如果不是，则一个一个加，如果是，则累加上去
            //string where1 = "where phase!='' and Sale_Start < getdate() and  Sale_End > getdate() AND Result='' order by phase asc";//还没开奖的那期+投注时间内的那期
            //BCW.JQC.Model.JQC_Internet aui = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where1);
            //if (aui.phase == 0)
            //{
            //    Utils.Equals("抱歉,暂无下注期数.请等待系统开彩.","");
            //}
            //else
            //{
            //    if (phase == aui.phase)//如果2期期号相同
            //    {
            //        jack.Jackpot = Price + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(phase);//实时奖池
            //    }
            //    else
            //    {

            //    }
            //}
            //动态
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]在[url=/bbs/game/jqc.aspx]" + GameName + "[/url]第" + phase + "期下注**" + ub.Get("SiteBz") + "";//" + Price + "
            new BCW.BLL.Action().Add(1014, a, meid, "", wText);
            Utils.Success("下注", "下注成功，花费了" + Price + "" + ub.Get("SiteBz") + "(共" + zhu2 * peilv + "注)<br /><a href=\"" + Utils.getUrl("jqc.aspx") + "\">&gt;继续下注</a>", Utils.getUrl("jqc.aspx"), "2");
        }
        else if (info == "ok1")
        {
            int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "post", 2, @"^[1-9]\d*$", "投注倍率输入错误"));
            int Price = Utils.ParseInt(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "投注金额填写错误"));
            int zhu1 = Utils.ParseInt(Utils.GetRequest("zhu", "post", 2, @"^[1-9]\d*$", "投注注数填写错误"));

            builder.Append(Out.Tab("<div>", ""));
            string where = "where phase=" + phase + "";
            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where);
            if (model.Sale_End <= DateTime.Now)
            {
                Utils.Error("抱歉,第" + phase + "期已截止下注.请选择其他期投注.", "");
            }
            builder.Append("第" + phase + "期离");
            string JQC_time = new BCW.JS.somejs().newDaojishi("jqc1", model.Sale_End);
            builder.Append("截止还有:" + JQC_time + "<br/>");
            builder.Append("您自带：" + (gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("每一注：" + zhuPrice + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总注数：" + zhu1 + "注<br />");
            Price = Price * peilv;
            builder.Append("总倍率：" + peilv + "倍<br />");
            builder.Append("总花费：" + Price + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,,,,,,,,,,,,";
            string strName = "peilv,phase,zhu,Price,Num0,Num00,Num1,Num11,Num2,Num22,Num3,Num33,act,info";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + peilv + "'" + phase + "'" + zhu1 + "'" + Price + "'" + Num1 + "'" + Num2 + "'" + Num3 + "'" + Num4 + "'" + Num5 + "'" + Num6 + "'" + Num7 + "'" + Num8 + "'pay'ok2";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定投注,jqc.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            if (Price > gold)
            {
                builder.Append("温馨提示:你的" + ub.Get("SiteBz") + "不够该投注.<br/>");
            }
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">&lt;&lt;返回再看看</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int Price = 0;//投注总钱数
            builder.Append(Out.Tab("<div>", ""));
            string where = "where phase=" + phase + "";
            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where);
            if (model.Sale_End <= DateTime.Now)
            {
                Utils.Error("抱歉,第" + phase + "期已截止下注.请选择其他期投注.", "");
            }
            builder.Append("第" + phase + "期离");
            string JQC_time = new BCW.JS.somejs().newDaojishi("jqc1", model.Sale_End);
            builder.Append("截止还有:" + JQC_time + "<br/>");
            builder.Append("您自带：" + (gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("每一注：" + zhuPrice + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总注数：" + zhu + "注<br />");
            Price = zhu * zhuPrice;
            builder.Append("总花费：" + Price + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "投注倍数：/,,,,,,,,,,,,,";
            string strName = "peilv,phase,zhu,Price,Num0,Num00,Num1,Num11,Num2,Num22,Num3,Num33,act,info";
            string strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + phase + "'" + zhu + "'" + Price + "'" + Num1 + "'" + Num2 + "'" + Num3 + "'" + Num4 + "'" + Num5 + "'" + Num6 + "'" + Num7 + "'" + Num8 + "'pay'ok1";
            string strEmpt = "true,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定投注,jqc.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            if (Price > gold)
            {
                builder.Append("温馨提示:你的" + ub.Get("SiteBz") + "不够该投注.<br/>");
            }
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">&lt;&lt;返回再看看</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        GameFoot();
    }

    //兑奖页面
    private void CasePage()
    {
        Master.Title = GameName + "_兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;兑奖");
        builder.Append(Out.Tab("</div>", ""));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">首页</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=case") + "\"><h style=\"color:red\">兑奖</h></a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist") + "\">历史</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "and GetMoney >0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        string jqcqi = "";
        string strOrder = "Input_Time desc";
        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Bet> listjqclist = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listjqclist.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Bet n in listjqclist)
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
                if (n.Lottery_issue.ToString() != jqcqi)
                {
                    string aa = "where phase ='" + n.Lottery_issue + "'";
                    BCW.JQC.Model.JQC_Internet model_num1 = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(aa);
                    builder.Append("<b>=第" + n.Lottery_issue + "期" + model_num1.Result + "=</b><br />");
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("投注号码{" + n.VoteNum + "}共" + n.Zhu * n.VoteRate + "注,共投" + n.PutGold + ub.Get("SiteBz") + ",投注时间[" + DT.FormatDate(n.Input_Time, 1) + "].");
                builder.Append("<br/><h style=\"color:red\">赢" + n.GetMoney + "" + ub.Get("SiteBz") + "</h>");
                builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

                jqcqi = n.Lottery_issue.ToString();
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
        //多个兑奖
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,jqc.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //单个兑奖
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));
        if (new BCW.JQC.BLL.JQC_Bet().ExistsState(pid, meid))
        {
            BCW.JQC.Model.JQC_Bet model = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bet(pid);
            long winMoney = model.GetMoney;
            BCW.User.Users.IsFresh("jqc", 2);//防刷
            new BCW.JQC.BLL.JQC_Bet().Update_win(pid, 3);
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "第" + model.Lottery_issue + "期兑奖-标识ID" + pid + "");
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("jqc.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("jqc.aspx?act=case"), "1");
        }
    }

    //多个兑奖
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
        long getwinMoney = 0;
        long winMoney = 0;
        BCW.User.Users.IsFresh("jqc", 2);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.JQC.BLL.JQC_Bet().ExistsState(pid, meid))
            {
                BCW.JQC.Model.JQC_Bet model = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bet(pid);
                new BCW.JQC.BLL.JQC_Bet().Update_win(pid, 3);
                winMoney = Convert.ToInt64(model.GetMoney);
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "第" + model.Lottery_issue + "期兑奖-标识ID" + pid + "");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("jqc.aspx?act=case"), "1");
    }

    //历史开奖
    private void ListPage()
    {
        Master.Title = "" + GameName + "_历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;历史开奖");
        builder.Append(Out.Tab("</div>", "<br/>"));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere = string.Empty;
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "";//Result!='' 
        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Internet> listjqclist = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internets(pageIndex, pageSize, strWhere, out recordCount);
        if (listjqclist.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Internet n in listjqclist)
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
                if (n.Result != "")
                {
                    builder.AppendFormat("{0}期:<a href=\"" + Utils.getUrl("jqc.aspx?act=listview&amp;id=" + n.ID + "") + "\">{1}</a>", n.phase, n.Result);
                }
                else
                {
                    builder.AppendFormat("{0}期:等待系统开奖.", n.phase);
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关开奖记录.."));
        }

        GameFoot();
    }

    //历史开奖——详细页面
    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet(id);
        if (model == null || model.Result == "")
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.phase + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=list") + "\">历史开奖</a>&gt;第" + model.phase + "期投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere += "Lottery_issue='" + model.phase + "' and GetMoney>0";

        string[] pageValUrl = { "act", "id", "page1", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strOrder = "Input_Time desc";

        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Bet> listjqclist = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("第" + model.phase + "期开出：" + model.Result + ",该期奖池为:" + new BCW.JQC.BLL.JQC_Internet().get_jiangchi(model.phase) + ".");
        if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=listview&amp;id=" + id + "&amp;ptype=2") + "\">展开︾</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=listview&amp;id=" + id + "") + "\">叠起︽</a>");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        //根据期号查询每注中奖金额
        string zhu_meney = new BCW.JQC.BLL.JQC_Internet().get_zhumeney(model.phase);
        int zm1 = 0; int zm2 = 0; int zm3 = 0; int zm4 = 0;
        if (zhu_meney != "")
        {
            string[] ab = zhu_meney.Split(',');
            try { zm1 = int.Parse(ab[0]); } catch { }
            try { zm2 = int.Parse(ab[1]); } catch { }
            try { zm3 = int.Parse(ab[2]); } catch { }
            try { zm4 = int.Parse(ab[3]); } catch { }
        }
        if (ptype == 2)
        {
            //根据期号，查询开奖情况：model.phase
            string[] Match = model.Match.Split(",".ToCharArray());//赛事
            string[] Team_Home = model.Team_Home.Split(',');//主场
            string[] Team_way = model.Team_Away.Split(',');//客场
            string[] Start_Time = model.Start_Time.Split(',');//开始时间
            string[] Result = model.Result.Split(',');//比赛结果
            for (int j = 0; j < 4; j++)
            {
                try
                {
                    builder.Append(j + 1 + ".[" + DT.FormatDate(Convert.ToDateTime(Start_Time[j]), 1) + "]");
                }
                catch
                {
                    builder.Append(j + 1 + ".[时间出错！]");
                }
                int e = 0; int t = 0;
                if (j == 0) { e = int.Parse(Result[0]); t = int.Parse(Result[1]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
                if (j == 1) { e = int.Parse(Result[2]); t = int.Parse(Result[3]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
                if (j == 2) { e = int.Parse(Result[4]); t = int.Parse(Result[5]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
                if (j == 3) { e = int.Parse(Result[6]); t = int.Parse(Result[7]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
            }

            builder.Append("<hr/>");
            builder.Append("奖池记录：<br/>");
            //邵广林 20161011 增加显示该期的奖池
            int pageIndex1;
            int recordCount1;
            int pageSize1 = 5;
            string strWhere1 = string.Empty;
            string strOrder1 = "AddTime desc";
            string[] pageValUrl1 = { "act", "id", "page", "ptype" };
            pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
            if (pageIndex1 == 0)
                pageIndex1 = 1;

            strWhere1 = "phase=" + model.phase + "";
            // 开始读取列表
            IList<BCW.JQC.Model.JQC_Jackpot> listjqcpay = new BCW.JQC.BLL.JQC_Jackpot().GetJQC_Jackpots(pageIndex1, pageSize1, strWhere1, strOrder1, out recordCount1);
            if (listjqcpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.JQC.Model.JQC_Jackpot n in listjqcpay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex1 - 1) * pageSize1 + k) + ".");
                    if (n.UsID != 10086)
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "(" + n.UsID + ")</a>");
                    else
                        builder.Append("<b style=\"color:red\">系统</b>");
                    string ah = string.Empty;
                    string bi = string.Empty;
                    if (n.type == 0)
                    {
                        ah = "在第" + n.phase + "期下注";
                        bi = n.InPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 1)
                    {
                        ah = "在第" + n.phase + "期派奖";
                        bi = n.OutPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 2)
                    {
                        ah = "在第" + n.phase + "期过期回收";
                        bi = n.InPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 3)
                    {
                        ah = "在第" + n.phase + "期系统扣税";
                        bi = n.OutPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 4)
                    {
                        if (n.InPrize > 0)
                        {
                            ah = "在第" + n.phase + "期系统增加";
                            bi = n.InPrize + ub.Get("SiteBz");
                        }
                        if (n.OutPrize > 0)
                        {
                            ah = "在第" + n.phase + "期系统回收";
                            bi = n.OutPrize + ub.Get("SiteBz");
                        }
                    }
                    if (n.type == 5)
                    {
                        if (n.InPrize > 0)
                        {
                            bi = n.InPrize + ub.Get("SiteBz") + "到" + n.phase + "期";
                            ah = "在第" + (n.phase - 1) + "期滚存";
                        }
                        if (n.OutPrize > 0)
                        {
                            bi = n.OutPrize + ub.Get("SiteBz") + "到" + (n.phase + 1) + "期";
                            ah = "在第" + n.phase + "期滚存";
                        }

                    }
                    if (n.type == 3 || n.type == 4 || n.type == 5)
                    {
                        //builder.Append("<b style=\"color:red\">");
                        builder.Append(ah + bi);
                        builder.Append(".结余:" + n.Jackpot + "");
                        //builder.Append("</b>");
                    }
                    else
                    {
                        builder.Append(ah + bi);
                        builder.Append(".结余:" + n.Jackpot + "");
                    }
                    builder.Append("[" + DT.FormatDate(n.AddTime, 0) + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, recordCount1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有奖池记录.."));
            }
            builder.Append("<hr/>");
        }

        builder.Append("[一等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize1") + "注，每注金额：" + zm1 + "<br/>");
        builder.Append("[二等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize2") + "注，每注金额：" + zm2 + "<br/>");
        builder.Append("[三等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize3") + "注，每注金额：" + zm3 + "<br/>");
        builder.Append("[四等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize4") + "注，每注金额：" + zm4 + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        if (listjqclist.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("共" + new BCW.JQC.BLL.JQC_Bet().count_zhu(model.phase) + "注中奖<br/>");//查询中奖人数
            builder.Append(Out.Tab("</div>", ""));
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Bet n in listjqclist)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>：");
                if (n.GetMoney > 0)
                {
                    builder.Append("中了" + (n.Prize1 * n.VoteRate + n.Prize2 * n.VoteRate + n.Prize3 * n.VoteRate + n.Prize4 * n.VoteRate) + "注,共赢" + n.GetMoney + "" + ub.Get("SiteBz") + ".");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist&amp;ptype=2") + "\">历史投注>></a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //历史开奖——详细页面
    private void ListViewPage2()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet2(id);
        if (model == null || model.Result == "")
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.phase + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=list") + "\">历史开奖</a>&gt;第" + model.phase + "期投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere += "Lottery_issue='" + model.phase + "' and GetMoney>0";

        string[] pageValUrl = { "act", "id", "page1", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strOrder = "Input_Time desc";

        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Bet> listjqclist = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("第" + model.phase + "期开出：" + model.Result + ",该期奖池为:" + new BCW.JQC.BLL.JQC_Internet().get_jiangchi(model.phase) + ".");
        if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=view&amp;id=" + id + "&amp;ptype=2") + "\">展开︾</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=view&amp;id=" + id + "") + "\">叠起︽</a>");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        //根据期号查询每注中奖金额
        string zhu_meney = new BCW.JQC.BLL.JQC_Internet().get_zhumeney(model.phase);
        int zm1 = 0; int zm2 = 0; int zm3 = 0; int zm4 = 0;
        if (zhu_meney != "")
        {
            string[] ab = zhu_meney.Split(',');
            try { zm1 = int.Parse(ab[0]); } catch { }
            try { zm2 = int.Parse(ab[1]); } catch { }
            try { zm3 = int.Parse(ab[2]); } catch { }
            try { zm4 = int.Parse(ab[3]); } catch { }
        }
        if (ptype == 2)
        {
            //根据期号，查询开奖情况：model.phase
            string[] Match = model.Match.Split(",".ToCharArray());//赛事
            string[] Team_Home = model.Team_Home.Split(',');//主场
            string[] Team_way = model.Team_Away.Split(',');//客场
            string[] Start_Time = model.Start_Time.Split(',');//开始时间
            string[] Result = model.Result.Split(',');//比赛结果
            for (int j = 0; j < 4; j++)
            {
                try
                {
                    builder.Append(j + 1 + ".[" + DT.FormatDate(Convert.ToDateTime(Start_Time[j]), 1) + "]");
                }
                catch
                {
                    builder.Append(j + 1 + ".[时间出错！]");
                }
                int e = 0; int t = 0;
                if (j == 0) { e = int.Parse(Result[0]); t = int.Parse(Result[1]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
                if (j == 1) { e = int.Parse(Result[2]); t = int.Parse(Result[3]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
                if (j == 2) { e = int.Parse(Result[4]); t = int.Parse(Result[5]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
                if (j == 3) { e = int.Parse(Result[6]); t = int.Parse(Result[7]); builder.Append("" + Match[j] + ":" + Team_Home[j] + "VS" + Team_way[j] + "(" + e + ":" + t + ")<br/>"); }
            }

            builder.Append("<hr/>");
            builder.Append("奖池记录：<br/>");
            //邵广林 20161011 增加显示该期的奖池
            int pageIndex1;
            int recordCount1;
            int pageSize1 = 5;
            string strWhere1 = string.Empty;
            string strOrder1 = "AddTime desc";
            string[] pageValUrl1 = { "act", "id", "page", "ptype" };
            pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
            if (pageIndex1 == 0)
                pageIndex1 = 1;

            strWhere1 = "phase=" + model.phase + "";
            // 开始读取列表
            IList<BCW.JQC.Model.JQC_Jackpot> listjqcpay = new BCW.JQC.BLL.JQC_Jackpot().GetJQC_Jackpots(pageIndex1, pageSize1, strWhere1, strOrder1, out recordCount1);
            if (listjqcpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.JQC.Model.JQC_Jackpot n in listjqcpay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex1 - 1) * pageSize1 + k) + ".");
                    if (n.UsID != 10086)
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "(" + n.UsID + ")</a>");
                    else
                        builder.Append("<b style=\"color:red\">系统</b>");
                    string ah = string.Empty;
                    string bi = string.Empty;
                    if (n.type == 0)
                    {
                        ah = "在第" + n.phase + "期下注";
                        bi = n.InPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 1)
                    {
                        ah = "在第" + n.phase + "期派奖";
                        bi = n.OutPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 2)
                    {
                        ah = "在第" + n.phase + "期过期回收";
                        bi = n.InPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 3)
                    {
                        ah = "在第" + n.phase + "期系统扣税";
                        bi = n.OutPrize + ub.Get("SiteBz");
                    }
                    if (n.type == 4)
                    {
                        if (n.InPrize > 0)
                        {
                            ah = "在第" + n.phase + "期系统增加";
                            bi = n.InPrize + ub.Get("SiteBz");
                        }
                        if (n.OutPrize > 0)
                        {
                            ah = "在第" + n.phase + "期系统回收";
                            bi = n.OutPrize + ub.Get("SiteBz");
                        }
                    }
                    if (n.type == 5)
                    {
                        if (n.InPrize > 0)
                        {
                            bi = n.InPrize + ub.Get("SiteBz") + "到" + n.phase + "期";
                            ah = "在第" + (n.phase - 1) + "期滚存";
                        }
                        if (n.OutPrize > 0)
                        {
                            bi = n.OutPrize + ub.Get("SiteBz") + "到" + (n.phase + 1) + "期";
                            ah = "在第" + n.phase + "期滚存";
                        }

                    }
                    if (n.type == 3 || n.type == 4 || n.type == 5)
                    {
                        //builder.Append("<b style=\"color:red\">");
                        builder.Append(ah + bi);
                        builder.Append(".结余:" + n.Jackpot + "");
                        //builder.Append("</b>");
                    }
                    else
                    {
                        builder.Append(ah + bi);
                        builder.Append(".结余:" + n.Jackpot + "");
                    }
                    builder.Append("[" + DT.FormatDate(n.AddTime, 0) + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, recordCount1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有奖池记录.."));
            }
            builder.Append("<hr/>");
        }

        builder.Append("[一等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize1") + "注，每注金额：" + zm1 + "<br/>");
        builder.Append("[二等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize2") + "注，每注金额：" + zm2 + "<br/>");
        builder.Append("[三等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize3") + "注，每注金额：" + zm3 + "<br/>");
        builder.Append("[四等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((model.phase), "Prize4") + "注，每注金额：" + zm4 + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        if (listjqclist.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("共" + new BCW.JQC.BLL.JQC_Bet().count_zhu(model.phase) + "注中奖<br/>");//查询中奖人数
            builder.Append(Out.Tab("</div>", ""));
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Bet n in listjqclist)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>：");
                if (n.GetMoney > 0)
                {
                    builder.Append("中了" + (n.Prize1 * n.VoteRate + n.Prize2 * n.VoteRate + n.Prize3 * n.VoteRate + n.Prize4 * n.VoteRate) + "注,共赢" + n.GetMoney + "" + ub.Get("SiteBz") + ".");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist&amp;ptype=2") + "\">历史投注>></a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //我的投注历史
    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "我的未开下注";
        else
            strTitle = "我的历史下注";

        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>&gt;" + strTitle + "");
        builder.Append(Out.Tab("</div>", ""));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">首页</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist") + "\"><h style=\"color:red\">历史</h></a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));


        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">未开下注" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist&amp;ptype=1") + "\">未开下注</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">历史下注" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist&amp;ptype=2") + "\">历史下注</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
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

        string jqcqi = "";
        string strOrder = "Lottery_issue desc,Input_Time desc";
        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Bet> listjqclist = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listjqclist.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Bet n in listjqclist)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (n.Lottery_issue.ToString() != jqcqi)
                {
                    string aa = "where phase ='" + n.Lottery_issue + "'";
                    BCW.JQC.Model.JQC_Internet model_num1 = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(aa);
                    builder.Append("<b>=第" + n.Lottery_issue + "期" + model_num1.Result + "=</b><br />");
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("投注号码{" + n.VoteNum + "}共" + n.Zhu * n.VoteRate + "注,共投" + n.PutGold + ub.Get("SiteBz") + ",投注时间[" + DT.FormatDate(n.Input_Time, 1) + "].");
                if (n.GetMoney > 1)
                {
                    //0未开1中2不中3领4过期
                    if (n.State == 3)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(已领奖)");
                    }
                    else if (n.State == 4)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(过期未领奖)");
                    }
                    else
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(未领奖)");
                }
                builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylistview&amp;id=" + n.ID + "&amp;ptype=" + ptype + "") + "\">详情&gt;&gt;</a>");

                jqcqi = n.Lottery_issue.ToString();
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

        GameFoot();
    }

    //我的投注历史——详细页面
    private void MyListViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        //根据期数的id，去查询数据库是否投注
        BCW.JQC.Model.JQC_Bet n = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bet(id);
        if (n == null || n.UsID != meid)
        {
            Utils.Error("不存在投注的记录", "");
        }
        //查询该id对应的开奖号码
        BCW.JQC.Model.JQC_Internet p = new BCW.JQC.BLL.JQC_Internet().Get_kainum(n.Lottery_issue);
        if (p == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + n.Lottery_issue + "期的投注情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("JQC.aspx?act=mylist") + "\">历史投注</a>&gt;详细投注情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("第" + n.Lottery_issue + "期,");
        if (p.Result == "")
            builder.Append("(未开奖)");
        else
            builder.Append("开出：" + p.Result + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        //根据期号查询每注中奖金额
        string zhu_meney = new BCW.JQC.BLL.JQC_Internet().get_zhumeney(n.Lottery_issue);
        int zm1 = 0; int zm2 = 0; int zm3 = 0; int zm4 = 0;
        if (zhu_meney != "")
        {
            string[] ab = zhu_meney.Split(',');
            try { zm1 = int.Parse(ab[0]); } catch { }
            try { zm2 = int.Parse(ab[1]); } catch { }
            try { zm3 = int.Parse(ab[2]); } catch { }
            try { zm4 = int.Parse(ab[3]); } catch { }
        }
        builder.Append("[一等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((n.Lottery_issue), "Prize1") + "注，每注金额：" + zm1 + "<br/>");
        builder.Append("[二等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((n.Lottery_issue), "Prize2") + "注，每注金额：" + zm2 + "<br/>");
        builder.Append("[三等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((n.Lottery_issue), "Prize3") + "注，每注金额：" + zm3 + "<br/>");
        builder.Append("[四等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((n.Lottery_issue), "Prize4") + "注，每注金额：" + zm4 + "<br/>");
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("购彩:" + n.VoteNum + "<br />购彩时间:" + DT.FormatDate(n.Input_Time, 0) + "<br/>");
        builder.Append("每注:" + n.Zhu_money + "" + ub.Get("SiteBz") + "<br />共投:" + n.Zhu + "注<br />花费:" + n.PutGold + "" + ub.Get("SiteBz") + "<br/>");
        builder.Append("倍数:" + n.VoteRate + "倍<br/>中一等奖:" + n.Prize1 * n.VoteRate + "注,中二等奖:" + n.Prize2 * n.VoteRate + "注;<br/>中三等奖:" + n.Prize3 * n.VoteRate + "注,中四等奖:" + n.Prize4 * n.VoteRate + "注.");
        if (n.GetMoney > 0)//134
        {
            if (n.State == 4)
            {
                builder.Append("<br />结果:已过兑奖时间,系统自动收回所得" + ub.Get("SiteBz") + ".");
            }
            else
                builder.Append("<br />结果:<h style=\"color:red\">赢:" + n.GetMoney + "" + ub.Get("SiteBz") + "</h>");
        }
        else
        {
            if (n.State == 0)
                builder.Append("<br />结果:未开奖");
            else if (n.State == 2)
            {
                builder.Append("<br />结果:未中奖");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        if ((n.GetMoney > 0) && (n.State == 1))
            builder.Append("<br/><a href=\"" + Utils.getUrl("jqc.aspx?act=case") + "\">马上兑奖>></a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //排行榜
    private void TopPage()
    {
        Master.Title = "" + GameName + "_排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", ""));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">首页</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist") + "\">历史</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top") + "\"><h style=\"color:red\">排行</h></a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">游戏赌神" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top&amp;ptype=1&amp;id=" + ptype + "") + "\">游戏赌神</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">土 豪 榜 " + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top&amp;ptype=2&amp;id=" + ptype + "") + "\">土 豪 榜</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string strWhere = "";//UsId not IN (" + a + ")
        string strOrder = "";
        //查询条件
        if (ptype == 1)
        {
            strWhere = "UsID,SUM(GetMoney-PutGold) AS ab ";
            strOrder = "State>0 GROUP BY UsID ORDER BY ab DESC";
        }
        if (ptype == 2)
        {
            strWhere = "UsID,SUM(GetMoney) AS ab ";
            strOrder = "State>0 and GetMoney>0 GROUP BY UsID ORDER BY ab DESC";
        }

        DataSet dds = new BCW.JQC.BLL.JQC_Bet().GetList(strWhere, strOrder);
        if (dds != null && dds.Tables[0].Rows.Count > 0)
        {
            recordCount = dds.Tables[0].Rows.Count;
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
                int ab = Convert.ToInt32(dds.Tables[0].Rows[koo + i]["ab"]);
                int usid = Convert.ToInt32(dds.Tables[0].Rows[koo + i]["usid"]);

                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));// class=\"text\"
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (ptype == 1)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "(" + usid + ")</a>净赢<h style=\"color:red\">" + ab + "</h>" + ub.Get("SiteBz") + "");
                }
                if (ptype == 2)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "(" + usid + ")</a>赚币<h style=\"color:red\">" + ab + "</h>" + ub.Get("SiteBz") + "");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        //按日期查询个人所得酷币
        int meid = new BCW.User.Users().GetUsId();
        long price = 0;
        long Cents = 0;
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            string _where1 = string.Empty;
            string _where2 = string.Empty;
            _where1 = "SUM(GetMoney-PutGold) as aa ";
            _where2 = " Input_Time>='" + searchday1 + "'and Input_Time<'" + searchday2 + "' and UsID='" + meid + "'";

            DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList(_where1, _where2);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    Cents = Convert.ToInt64(ds.Tables[0].Rows[0]["aa"].ToString());
                }
                catch
                {
                    Cents = 0;
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<hr/>");
            if (Cents == 0)
            {
                builder.Append("我的战绩:(" + searchday1 + "至" + searchday2 + ")<br/>盈利0" + ub.Get("SiteBz") + "");
            }
            else
                builder.Append("我的战绩:(" + searchday1 + "至" + searchday2 + ")<br/>盈利<h style=\"color:red\">" + Cents + "</h>" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,jqc.aspx?act=top&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            if (meid != 0)
            {
                //根据id查询盈利
                DataSet di = new BCW.JQC.BLL.JQC_Bet().GetList("SUM(GetMoney-PutGold) AS a", "UsID=" + meid + "");
                try
                {
                    if (di != null && di.Tables[0].Rows.Count > 0)
                    {
                        price = Convert.ToInt64(di.Tables[0].Rows[0]["a"].ToString());
                    }
                    else
                    {
                        price = 0;
                    }
                }
                catch
                {

                    price = 0;
                }
            }

            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("<hr/>");
            //builder.Append("我的战绩:总盈利<h style=\"color:red\">" + price + "</h>" + ub.Get("SiteBz") + ".");
            //builder.Append(Out.Tab("</div>", ""));

            //string strText = "开始日期：,结束日期：,";
            //string strName = "sTime,oTime,act";
            //string strType = "date,date,hidden";
            //string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave";
            //string strEmpt = "false,false,false";
            //string strIdea = "/";
            //string strOthe = "马上查询,jqc.aspx?act=top&amp;ptype=" + ptype + ",post,1,red";
            //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        //builder.Append(Out.Tab("<div>", "<br/>"));
        //builder.Append("温馨提示：时间段格式：2016-06-11 00:00:00");
        //builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //规则
    private void RulePage()
    {
        Master.Title = "" + GameName + "_玩法规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;玩法规则");
        builder.Append(Out.Tab("</div>", ""));

        //顶部ubb
        string JQCtop = ub.GetSub("JQCTop", xmlPath);
        if (JQCtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(JQCtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">首页</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=mylist") + "\">历史</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=rule") + "\"><h style=\"color:red\">规则</h></a>|");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=top") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("一、玩法说明<br />四场进球彩对4场比赛8支球队在全场90分钟(含伤停补时)的进球数量(0、1、2、3+)进行投注<br/>");
        builder.Append("二、投注方式<br />对4场比赛8支球队各选1种进球结果为1注,每支球队最多可选4种结果,每注" + ub.GetSub("zhuPrice", "/Controls/jqc.xml") + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("三、设奖<br />一等奖(猜中8支球队进球数),二等奖(猜中7支球队进球数)<br/>三等奖(猜中6支球队进球数),四等奖(猜中5支球队进球数)<br />");
        builder.Append("四、奖金<br />");
        builder.Append("一等奖为奖池的" + ub.GetSub("jqcOne", "/Controls/jqc.xml") + "%,多人中则平分<br/>二等奖为奖池的" + ub.GetSub("jqcTwo", "/Controls/jqc.xml") + "%,多人中则平分<br/>三等奖为奖池的" + ub.GetSub("jqcThree", "/Controls/jqc.xml") + "%,多人中则平分<br/>四等奖为奖池的" + ub.GetSub("jqcFour", "/Controls/jqc.xml") + "%,多人中则平分<br/>");
        builder.Append("五、特别说明：<br/>如果某场比赛因故没有开赛,则购买这场比赛的结果一律算输.");
        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.GetSub("guize", xmlPath))));
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //游戏底部
    private void GameFoot()
    {
        //游戏底部Ubb
        string Foot = ub.GetSub("JQCFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("JQC.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //检查奖池-首期用
    private void check_jiangchi()
    {
        //查询数据库表是否有数据
        DataSet a = new BCW.JQC.BLL.JQC_Jackpot().GetList("COUNT(*) as a", "");
        int id = 0;
        if (a != null && a.Tables[0].Rows.Count > 0)
        {
            try
            {
                id = int.Parse(a.Tables[0].Rows[0]["a"].ToString());
            }
            catch
            {
                id = 0;
            }
        }
        if (id == 0)
        {
            int phase = new BCW.JQC.BLL.JQC_Internet().Get_newID();//得到最后一期可以投注的期号
            if (phase != 0)
            {
                //添加一条系统数据
                BCW.JQC.Model.JQC_Jackpot aa = new BCW.JQC.Model.JQC_Jackpot();
                aa.AddTime = DateTime.Now;
                aa.BetID = 0;
                aa.InPrize = jiangchi;
                aa.Jackpot = jiangchi;
                aa.OutPrize = 0;
                aa.phase = phase;//数据库最新一期
                aa.type = 4;//系统
                aa.UsID = 10086;
                new BCW.JQC.BLL.JQC_Jackpot().Add(aa);
            }
        }
    }



}
