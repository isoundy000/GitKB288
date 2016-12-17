using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using BCW.Common;
using System.Timers;
using System.Xml;

public partial class jqcget : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string GameName = ub.GetSub("GameName", "/Controls/jqc.xml");//游戏名字
    protected long jiangchi = Convert.ToInt64(ub.GetSub("jiangchi", "/Controls/jqc.xml"));//奖池金额
    protected long jiangchi_guding = Convert.ToInt64(ub.GetSub("jiangchi_guding", "/Controls/jqc.xml"));
    protected long huishou = Convert.ToInt64(ub.GetSub("huishou", "/Controls/jqc.xml"));//系统回收金额
    string date_1 = ub.GetSub("hsdatetime", "/Controls/jqc.xml");//系统回收过期不兑奖的时间
    protected long jianchi_zuidi = Convert.ToInt64(ub.GetSub("jianchi_zuidi", "/Controls/jqc.xml"));//奖池最低回收

    protected long jqcOne = Convert.ToInt64(ub.GetSub("jqcOne", "/Controls/jqc.xml"));//1等奖
    protected long jqcTwo = Convert.ToInt64(ub.GetSub("jqcTwo", "/Controls/jqc.xml"));//2等奖
    protected long jqcThree = Convert.ToInt64(ub.GetSub("jqcThree", "/Controls/jqc.xml"));//3等奖
    protected long jqcFour = Convert.ToInt64(ub.GetSub("jqcFour", "/Controls/jqc.xml"));//4等奖

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        //抓取
        List<Matchs> list = new List<Matchs>();
        try
        {
            list = TranList();
        }
        catch
        {
            Response.Write("抓取失败,error1.");
            Response.Close();
        }
        list.Reverse();//集合反序
        foreach (Matchs a in list)
        {
            if (!new BCW.JQC.BLL.JQC_Internet().Exists_phase(int.Parse(a.phase)))
            {
                #region 判断数据库最新一期是否存在，不存在，自动添加
                string Match = string.Empty;//赛事
                string Team_Home = string.Empty;//主队
                string Team_Away = string.Empty;//客队
                string Start_Time = string.Empty;//比赛时间
                string Score = string.Empty;//比赛比分
                int i = 1;
                BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
                model.phase = int.Parse(a.phase);
                foreach (Matchs2 b in a.listMatch)
                {
                    if (i == 4)
                    {
                        Match = Match + b.match;
                        Team_Home = Team_Home + b.team_home;
                        Team_Away = Team_Away + b.team_away;
                        Start_Time = Start_Time + b.start_time;
                        Score = Score + b.score;
                    }
                    else
                    {
                        Match = Match + b.match + ",";
                        Team_Home = Team_Home + b.team_home + ",";
                        Team_Away = Team_Away + b.team_away + ",";
                        Start_Time = Start_Time + b.start_time + ",";
                        Score = Score + b.score + "#";
                    }
                    i++;
                }
                model.Match = Match;//赛事
                model.Team_Home = Team_Home;//主队
                model.Team_Away = Team_Away;//客队
                model.Start_Time = Start_Time;//比赛时间
                model.Score = Score;//比赛比分
                model.Result = a.result;//比赛结果


                if (a.sale_start != "")
                {
                    model.Sale_Start = Convert.ToDateTime(a.sale_start);
                    model.Sale_End = Convert.ToDateTime(a.sale_end);
                    new BCW.JQC.BLL.JQC_Internet().Add(model);
                    Response.Write("添加最新期数成功.JQCok1<br/>");
                }
                else
                {
                    //邵广林 20161007 修改没开售时间都添加到数据库
                    model.Sale_Start = DateTime.Now.AddYears(-1);
                    model.Sale_End = DateTime.Now.AddYears(-1);
                    new BCW.JQC.BLL.JQC_Internet().Add(model);
                    //通知系统管理员，存进了无销售时间的一期
                    new BCW.BLL.Guest().Add(1, 52784, new BCW.BLL.User().GetUsName(52784), "进球彩存进没有销售时间的一期，期号" + a.phase + ".请到后台查看.");
                    Response.Write("添加最新期数成功.【但没有销售时间.】JQCok1<br/>");
                    //Response.Write("最新期数[" + a.phase + "]已出.等待加入.JQCok1<br/>");
                }
                #endregion
            }
            else
            {
                if (!new BCW.JQC.BLL.JQC_Internet().Exists_Result(int.Parse(a.phase)))
                {
                    #region 若存在，则判断是否开奖
                    string Match = string.Empty;//赛事
                    string Team_Home = string.Empty;//主队
                    string Team_Away = string.Empty;//客队
                    string Start_Time = string.Empty;//比赛时间
                    string Score = string.Empty;//比赛比分
                    int i = 1;
                    foreach (Matchs2 b in a.listMatch)
                    {
                        if (i == 4)
                        {
                            Match = Match + b.match;
                            Team_Home = Team_Home + b.team_home;
                            Team_Away = Team_Away + b.team_away;
                            Start_Time = Start_Time + b.start_time;
                            Score = Score + b.score;
                        }
                        else
                        {
                            Match = Match + b.match + ",";
                            Team_Home = Team_Home + b.team_home + ",";
                            Team_Away = Team_Away + b.team_away + ",";
                            Start_Time = Start_Time + b.start_time + ",";
                            Score = Score + b.score + "#";
                        }
                        i++;
                    }
                    //判断是否有不足信息的球队,不足则添加信息
                    BCW.JQC.Model.JQC_Internet hj = new BCW.JQC.BLL.JQC_Internet().Get_kainum(int.Parse(a.phase));
                    try
                    {
                        string[] sNum = hj.Match.Split(',');
                        if (sNum.Length < 4)
                        {
                            new BCW.JQC.BLL.JQC_Internet().Update_Result("Match='" + Match + "',Team_Home='" + Team_Home + "',Team_Away='" + Team_Away + "',Start_Time='" + Start_Time + "'", "phase='" + a.phase + "'");
                            Response.Write("" + a.phase + "期信息已补录成功.<br/>");
                        }
                        if (a.sale_start != "")
                        {
                            new BCW.JQC.BLL.JQC_Internet().Update_Result("Sale_Start='" + a.sale_start + "',Sale_End='" + a.sale_end + "'", "phase='" + a.phase + "'");
                        }
                    }
                    catch { }
                    if (a.result != null)//如果有开奖结果，即兑奖
                    {
                        if (a.sale_start != "")
                        {
                            new BCW.JQC.BLL.JQC_Internet().Update_Result("Sale_Start='" + a.sale_start + "',Sale_End='" + a.sale_end + "'", "phase='" + a.phase + "'");
                        }
                        new BCW.JQC.BLL.JQC_Internet().Update_Result("Result='" + a.result + "',Score='" + Score + "'", "phase='" + a.phase + "'");
                        Open_price(a.phase);//兑奖
                    }
                    else//没有开奖结果，则更新比分
                    {
                        new BCW.JQC.BLL.JQC_Internet().Update_Result("Score='" + Score + "'", "phase='" + a.phase + "'");
                        if (a.sale_start != "")
                        {
                            new BCW.JQC.BLL.JQC_Internet().Update_Result("Sale_Start='" + a.sale_start + "',Sale_End='" + a.sale_end + "'", "phase='" + a.phase + "'");
                        }
                        Response.Write("" + a.phase + "期等待开奖.ok1<br/>");
                    }
                    #endregion
                }
                else
                    Response.Write("" + a.phase + "期已获取.ok1<br/>");
            }
        }
        //系统收回过期未兑奖
        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("State=2", "GetMoney>0 and Input_Time<DateAdd(dd,-" + date_1 + ",getdate())");
        //机器人兑奖
        Robot_case();
        stopwatch.Stop();
        Response.Write("<font color=\"red\">" + "<br/>刷新总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒</font><br/>");
    }

    //获得网页数据
    public XmlDocument JQCPage()
    {
        string url = "http://s.apiplus.cn/jq/?token=2bbadaee337d9127";
        XmlDocument xml = new XmlDocument();
        xml.Load(url);
        return xml;
    }

    //将xml解析成list集合
    public List<Matchs> TranList()
    {
        List<Matchs> list = new List<Matchs>();
        XmlDocument dom = JQCPage();
        XmlNodeList nodelist = dom.SelectSingleNode("/xml").ChildNodes;
        foreach (XmlNode node in nodelist)
        {
            Matchs matchs = new Matchs();
            XmlElement xe = (XmlElement)node;//将子节点类型转换为xmlelement类型

            matchs.phase = xe.GetAttribute("phase").ToString();
            matchs.result = xe.GetAttribute("result").ToString();
            matchs.sale_start = xe.GetAttribute("sale_start").ToString();
            matchs.sale_end = xe.GetAttribute("sale_end").ToString();

            foreach (XmlNode node2 in node)
            {
                foreach (XmlNode node3 in node2)
                {
                    Matchs2 matchs2 = new Matchs2();
                    XmlElement xe2 = (XmlElement)node3;

                    matchs2.match = xe2.GetAttribute("match").ToString();
                    matchs2.team_home = xe2.GetAttribute("team_home").ToString();
                    matchs2.team_away = xe2.GetAttribute("team_away").ToString();
                    matchs2.start_time = xe2.GetAttribute("start_time").ToString();
                    matchs2.score = xe2.GetAttribute("score").ToString();

                    matchs.listMatch.Add(matchs2);
                }
            }
            list.Add(matchs);
        }
        return list;
    }

    //开始返彩
    public void Open_price(string phase)
    {
        //如果这个期号前面有未开奖的期号，就算这期有开奖号码，都不开奖，内线提示10086
        //查询是否有开奖为空的期号
        if (new BCW.JQC.BLL.JQC_Internet().Exists_wei(int.Parse(phase)))
        {
            DataSet ds = new BCW.BLL.Guest().GetList("*", "Content LIKE '%进球彩%' AND ToId=52784 AND ToName='森林仔555' AND DateDiff(dd,AddTime,getdate())=0");
            if (ds == null && ds.Tables[0].Rows.Count <= 0)
                new BCW.BLL.Guest().Add(1, 52784, "森林仔555", "注意：进球彩在第" + phase + "期前存在未开奖期号，请马上去后台检查开奖.");

            DataSet ds1 = new BCW.BLL.Guest().GetList("*", "Content LIKE '%进球彩%' AND ToId=10086 AND ToName='客服10086' AND DateDiff(dd,AddTime,getdate())=0");
            if (ds1 == null && ds1.Tables[0].Rows.Count <= 0)
                new BCW.BLL.Guest().Add(1, 10086, "客服10086", "注意：进球彩在第" + phase + "期前存在未开奖期号，请马上去后台检查开奖.");
            Response.Write("<br/>在" + phase + "期前存在未开奖期号，请马上检查开奖.ok1<br/>");
        }
        else
        {
            #region
            string where = string.Empty;
            where = "where phase='" + phase + "'";
            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where);

            string Result = model.Result;
            Response.Write("<br/>等待开奖期号：[" + model.phase + "]ok1<br/>");
            if (Result != "")//如果开奖号码为空，则不返奖
            {
                //取出当前期的奖池金额
                long get_jiangchi_kou = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((int.Parse(phase)));

                #region 判断系统是否有投入和开奖是否够币
                //如果奖池大于400w，则判断是否有系统投入，若有，则先扣除系统投入，再拿去派奖和扣税
                if (get_jiangchi_kou > jiangchi_guding)
                {
                    //通过比较type=4、InPrize次数和OutPrize次数对比
                    if (new BCW.JQC.BLL.JQC_Jackpot().Getxitong_toujin() > new BCW.JQC.BLL.JQC_Jackpot().Getxitong_huishou())
                    {
                        BCW.JQC.Model.JQC_Jackpot bb = new BCW.JQC.Model.JQC_Jackpot();
                        bb.AddTime = DateTime.Now;
                        bb.BetID = 0;
                        bb.InPrize = 0;
                        bb.Jackpot = get_jiangchi_kou - jiangchi;
                        bb.OutPrize = jiangchi;
                        bb.phase = (int.Parse(phase));//开奖的那期
                        bb.type = 4;//系统
                        bb.UsID = 10086;
                        new BCW.JQC.BLL.JQC_Jackpot().Add(bb);
                    }
                }
                //判断该期最后的奖池是否大于最低的200w，如果是，则增加200w
                if (get_jiangchi_kou < jianchi_zuidi)
                {
                    BCW.JQC.Model.JQC_Jackpot aa = new BCW.JQC.Model.JQC_Jackpot();
                    aa.AddTime = DateTime.Now;
                    aa.BetID = 0;
                    aa.InPrize = jiangchi;
                    aa.Jackpot = jiangchi + get_jiangchi_kou;
                    aa.OutPrize = 0;
                    aa.phase = (int.Parse(phase));//开奖的那期
                    aa.type = 4;//系统
                    aa.UsID = 10086;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(aa);
                }
                #endregion

                //取出当前期的奖池金额——系统增加或扣除后
                long get_jiangchi = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((int.Parse(phase)));
                //把金额存到相应的期号
                new BCW.JQC.BLL.JQC_Internet().Update_Result("nowprize='" + get_jiangchi + "'", "phase='" + phase + "'");
                //系统没收的金额并奖池减少
                long moshou = Convert.ToInt64(get_jiangchi * huishou * 0.01);
                BCW.JQC.Model.JQC_Jackpot aa1 = new BCW.JQC.Model.JQC_Jackpot();
                aa1.AddTime = DateTime.Now;
                aa1.BetID = 0;
                aa1.InPrize = 0;
                aa1.Jackpot = get_jiangchi - moshou;
                aa1.OutPrize = moshou;
                aa1.phase = (int.Parse(phase));
                aa1.type = 3;//系统扣税10%
                aa1.UsID = 10086;
                new BCW.JQC.BLL.JQC_Jackpot().Add(aa1);


                //检查投注表是否存在没开奖数据
                //if (new BCW.JQC.BLL.JQC_Bet().Exists_num(model.phase))
                {
                    //得到每个奖的金额
                    long z4 = Convert.ToInt64(get_jiangchi * jqcFour * 0.01);
                    long z3 = Convert.ToInt64(get_jiangchi * jqcThree * 0.01);
                    long z2 = Convert.ToInt64(get_jiangchi * jqcTwo * 0.01);
                    long z1 = Convert.ToInt64(get_jiangchi * jqcOne * 0.01);

                    DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("*", " State=0 and Lottery_issue='" + model.phase + "'");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int count = 0;
                        string[] resultnum = Result.Split(',');

                        //中奖判断
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //投注数据
                            int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                            int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                            string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                            int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                            long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                            long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                            long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                            int VoteRate = int.Parse(ds.Tables[0].Rows[i]["VoteRate"].ToString());//投注倍率
                            DateTime Input_Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Input_Time"]);
                            int isRobot = int.Parse(ds.Tables[0].Rows[i]["isRobot"].ToString());
                            string Vote = ds.Tables[0].Rows[i]["VoteNum"].ToString();//投注号码
                            int VoteNum = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());//投注注数

                            if (VoteNum == 1)//单注
                            {
                                #region 单式中奖算法
                                string[] votenum = Vote.Split('#');
                                if (Vote != "" && Result != "")
                                {
                                    for (int k = 0; k < 8; k++)
                                    {
                                        string aa = votenum[k];
                                        string bb = resultnum[k];
                                        if (True(aa, bb))
                                        {
                                            count++;
                                        }
                                    }
                                }
                                #endregion

                                #region 单式中奖更新数据库
                                if (count == 5)//4等奖----State=4
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize4=1", "ID='" + ID + "'");
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);//0未开1中2不中3已领4过期
                                }
                                else if (count == 6)//3等奖----State=3
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize3=1", "ID='" + ID + "'");
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                }
                                else if (count == 7)//2等奖----State=2
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize2=1", "ID='" + ID + "'");
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                }
                                else if (count == 8)//1等奖----State=1
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize1=1", "ID='" + ID + "'");
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                }
                                else//不中奖
                                {
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 2);
                                }
                                count = 0;
                                #endregion
                            }
                            else//复式
                            {
                                #region 复式中奖算法
                                int num1 = 0; int num2 = 0; int num3 = 0; int num4 = 0;
                                string[] buyvote = Vote.Split('#');
                                string[] vote1 = buyvote[0].Split(',');
                                string[] vote2 = buyvote[1].Split(',');
                                string[] vote3 = buyvote[2].Split(',');
                                string[] vote4 = buyvote[3].Split(',');
                                string[] vote5 = buyvote[4].Split(',');
                                string[] vote6 = buyvote[5].Split(',');
                                string[] vote7 = buyvote[6].Split(',');
                                string[] vote8 = buyvote[7].Split(',');

                                List<string[]> list = new List<string[]>();
                                list.Add(vote1);
                                list.Add(vote2);
                                list.Add(vote3);
                                list.Add(vote4);
                                list.Add(vote5);
                                list.Add(vote6);
                                list.Add(vote7);
                                list.Add(vote8);
                                string[] totalResult;
                                string[] Finalresult = Result.Split(',');

                                totalResult = bianli(list);//得到投注数据

                                for (int iresult = 0; iresult < totalResult.Length; iresult++)
                                {
                                    //builder.Append(totalResult[iresult].Replace("#", ",") + "<br/>");
                                    string[] results = totalResult[iresult].Split('#');
                                    for (int j = 0; j < results.Length; j++)
                                    {
                                        if (results[j].Equals(Finalresult[j]))
                                        {
                                            count++;//遍历开奖数据是否相同，相同则count+1
                                        }
                                    }
                                    if (count == 5)//如果count出现的次数等于5，证明是四等奖
                                    {
                                        num4++;
                                    }
                                    if (count == 6)//如果count出现的次数等于6，证明是三等奖
                                    {
                                        num3++;

                                    }
                                    if (count == 7)//如果count出现的次数等于7，证明是二等奖
                                    {
                                        num2++;
                                    }
                                    if (count == 8)//如果count出现的次数等于8，证明是一等奖
                                    {
                                        num1 = 1;
                                    }
                                    count = 0;
                                }
                                #endregion

                                #region 单式中奖更新数据库
                                //更新各中几等奖
                                if (num1 > 0)
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize1=1", "ID='" + ID + "'");
                                }
                                if (num2 > 0)
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize2=" + num2 + "", "ID='" + ID + "'");
                                }
                                if (num3 > 0)
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize3=" + num3 + "", "ID='" + ID + "'");
                                }
                                if (num4 > 0)
                                {
                                    new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize4=" + num4 + "", "ID='" + ID + "'");
                                }
                                //更新中奖状态
                                if (num1 == 0 && num2 == 0 && num3 == 0 && num4 == 0)
                                {
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 2);
                                }
                                else
                                {
                                    new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                }
                                #endregion
                            }
                        }
                    }

                    //计算当期各等奖总注数
                    int q4 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize4");
                    int q3 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize3");
                    int q2 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize2");
                    int q1 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize1");
                    //得到每注金额
                    long z_price4, z_price3, z_price2, z_price1 = 0;
                    if (q1 != 0) { z_price1 = z1 / q1; } else { z_price1 = z1; }
                    if (q2 != 0) { z_price2 = z2 / q2; } else { z_price2 = z2; }
                    if (q3 != 0) { z_price3 = z3 / q3; } else { z_price3 = z3; }
                    if (q4 != 0) { z_price4 = z4 / q4; } else { z_price4 = z4; }

                    string ai = q1 + "," + q2 + "," + q3 + "," + q4;
                    string aii = z_price1 + "," + z_price2 + "," + z_price3 + "," + z_price4;

                    //把中奖注数和每注金额存到数据库对应的那期
                    new BCW.JQC.BLL.JQC_Internet().Update_Result("zhu='" + ai + "',zhu_money='" + aii + "'", "phase='" + phase + "'");

                    //派奖
                    DataSet ds1 = new BCW.JQC.BLL.JQC_Bet().GetList("*", " State>0 and Lottery_issue='" + model.phase + "'");
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        int ID = int.Parse(ds1.Tables[0].Rows[i]["ID"].ToString());
                        int UsID = int.Parse(ds1.Tables[0].Rows[i]["UsID"].ToString());
                        int VoteRate = int.Parse(ds1.Tables[0].Rows[i]["VoteRate"].ToString());//投注倍率
                        int isRobot = int.Parse(ds1.Tables[0].Rows[i]["isRobot"].ToString());
                        int Prize1 = int.Parse(ds1.Tables[0].Rows[i]["Prize1"].ToString());
                        int Prize2 = int.Parse(ds1.Tables[0].Rows[i]["Prize2"].ToString());
                        int Prize3 = int.Parse(ds1.Tables[0].Rows[i]["Prize3"].ToString());
                        int Prize4 = int.Parse(ds1.Tables[0].Rows[i]["Prize4"].ToString());
                        long qian = 0;

                        //奖池减少
                        BCW.JQC.Model.JQC_Jackpot bb = new BCW.JQC.Model.JQC_Jackpot();
                        bb.AddTime = DateTime.Now;
                        bb.BetID = ID;
                        bb.InPrize = 0;
                        bb.phase = int.Parse(phase);
                        bb.type = 1;
                        bb.UsID = UsID;

                        if (Prize1 > 0 || Prize2 > 0 || Prize3 > 0 || Prize4 > 0)
                        {
                            qian = z_price4 * VoteRate * Prize4 + z_price3 * VoteRate * Prize3 + z_price2 * VoteRate * Prize2 + z_price1 * VoteRate * Prize1;
                            new BCW.JQC.BLL.JQC_Bet().update_GetMoney("GetMoney='" + qian + "'", "ID=" + ID + "");
                            //奖池
                            bb.Jackpot = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(int.Parse(phase)) - qian;
                            bb.OutPrize = qian;
                            new BCW.JQC.BLL.JQC_Jackpot().Add(bb);

                            if (isRobot == 0)
                                new BCW.BLL.Guest().Add(1, UsID, new BCW.BLL.User().GetUsName(UsID), "恭喜您在" + GameName + "第" + model.phase + "期获得了" + qian + "" + ub.Get("SiteBz") + "[url=/bbs/game/jqc.aspx?act=case]马上兑奖[/url]");
                            string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在[url=/bbs/game/jqc.aspx]" + GameName + "[/url]《" + model.phase + "期》获得了" + qian + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(1014, ID, UsID, "", wText);
                            System.Threading.Thread.Sleep(5);
                        }
                    }

                    System.Threading.Thread.Sleep(5);

                    //把剩余的奖池，加到下一期去,该期奖池清空
                    BCW.JQC.Model.JQC_Jackpot aa3 = new BCW.JQC.Model.JQC_Jackpot();
                    aa3.AddTime = DateTime.Now;
                    aa3.BetID = 0;
                    aa3.InPrize = 0;
                    aa3.Jackpot = 0;
                    aa3.OutPrize = get_jiangchi - moshou - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(int.Parse(phase));
                    aa3.phase = (int.Parse(phase));
                    aa3.type = 5;//5是系统滚存
                    aa3.UsID = 10086;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(aa3);

                    BCW.JQC.Model.JQC_Jackpot aa2 = new BCW.JQC.Model.JQC_Jackpot();
                    aa2.AddTime = DateTime.Now;
                    aa2.BetID = 0;
                    aa2.InPrize = get_jiangchi - moshou - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(int.Parse(phase));
                    aa2.Jackpot = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((int.Parse(phase) + 1)) + get_jiangchi - moshou - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(int.Parse(phase));
                    aa2.OutPrize = 0;
                    aa2.phase = (int.Parse(phase) + 1);
                    aa2.type = 5;//5是系统滚存
                    aa2.UsID = 10086;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(aa2);


                    //滚存后，判断下一期是否够200w，如果不够则加200w
                    //查询滚存后的那期奖池
                    if (new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((int.Parse(phase) + 1)) < jianchi_zuidi)
                    {
                        BCW.JQC.Model.JQC_Jackpot aa = new BCW.JQC.Model.JQC_Jackpot();
                        aa.AddTime = DateTime.Now;
                        aa.BetID = 0;
                        aa.InPrize = jiangchi;
                        aa.Jackpot = jiangchi + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((int.Parse(phase) + 1));
                        aa.OutPrize = 0;
                        aa.phase = (int.Parse(phase) + 1);
                        aa.type = 4;//系统
                        aa.UsID = 10086;
                        new BCW.JQC.BLL.JQC_Jackpot().Add(aa);
                    }
                }
            }
            #endregion
        }
    }

    //机器人兑奖
    public void Robot_case()
    {
        DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("*", "GetMoney>0 AND isRobot=1 and State=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());

                new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 3);//3为中奖
                new BCW.BLL.User().UpdateiGold(UsID, GetMoney, "" + GameName + "兑奖-标识ID" + ID + "");
            }
        }
    }


    // 单式开奖号码对比
    public bool True(string votenum, string resultnum)
    {
        if ((votenum).Contains(resultnum))
            return true;
        return false;
    }

    //遍历数组，组合投注结果
    private string[] bianli(List<string[]> al)
    {
        if (al.Count == 0)
            return null;
        int size = 1;
        for (int i = 0; i < al.Count; i++)
        {
            size = size * al[i].Length;
        }
        string[] str = new string[size];
        for (long j = 0; j < size; j++)
        {
            for (int m = 0; m < al.Count; m++)
            {
                str[j] = str[j] + al[m][(j * jisuan(al, m) / size) % al[m].Length] + "#";
            }
            str[j] = str[j].Trim('#');
        }
        return str;
    }

    //计算当前产生的结果数
    private int jisuan(List<string[]> al, int m)
    {
        int result = 1;
        for (int i = 0; i < al.Count; i++)
        {
            if (i <= m)
            {
                result = result * al[i].Length;
            }
            else
            {
                break;
            }
        }
        return result;
    }


}
public class Matchs2
{
    public string match;
    public string team_home;
    public string team_away;
    public string start_time;
    public string score;
}

public class Matchs
{
    public string phase;
    public string result;
    public string sale_start;
    public string sale_end;
    public List<Matchs2> listMatch;

    public Matchs()
    {
        listMatch = new List<Matchs2>();
    }

}


