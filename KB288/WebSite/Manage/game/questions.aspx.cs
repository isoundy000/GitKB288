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
using System.Threading;
using System.Text;
using System.Diagnostics;

public partial class Manage_game_questions : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected int GameId = 45;
    protected string xmlPath = "/Controls/questions.xml";
    protected string GameName = Convert.ToString(ub.GetSub("GameName", "/Controls/questions.xml"));
    protected string QuestionsStatue = Convert.ToString(ub.GetSub("QuestionsStatue", "/Controls/questions.xml"));
    string[] question_deficult_list = { "新手", "初级", "中级", "高级", "困难", "大师" };//难度
    protected string q_deficult = "0|新手|1|初级|2|中级|3|高级|4|困难|5|大师";//难度
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule"://规则介绍
                Rule();
                break;
            case "question"://问题管理
                QuestionPage();
                break;
            case "style"://问题类型
                QuestionStylePage();
                break;
            case "checkanswer"://回答查看
                QuestionCheckAnswerPage();
                break;
            case "answer"://答题记录
                AnswerPage();
                break;
            case "ansctr"://每一轮的ID列
                AnswerCtrPage();
                break;
            case "manage"://一个问题管理
                AQuestionPage(); 
                break;
            case "editacontr"://一个控制器问题管理
                EditAContrPage(); 
                break;
            case "set"://配置管理
                SetPage();
                break;
            case "edit"://编辑问题
                EditPage();
                break;
            case "del"://删除问题
                DelPage();
                break;
            case "delstyle"://删除问题类型
                DelStylePage();
                break;
            case "goto"://发布问题
                GotoPage();
                break;
            case "paijiang"://派奖
                PaiJiangPage();
                break;
            case "select"://选择发布
                SelectPage();
                break;
            case "paihang"://排行榜单
                TopListPage();
                break;
            case "data"://数据分析
                DataPage();
                break;
            case "reset"://重置游戏
                ResetPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    #region 获得等级中文
    //获得等 一二三...
    private string getDengJiChinese(int i)
    {
        switch (i)
        {
            case 0:
                return "0";
            case 1:
                return "一";
            case 2:
                return "二";
            case 3:
                return "三";
            case 4:
                return "四";
            case 5:
                return "五";
            case 6:
                return "六";
            case 7:
                return "七";
            case 8:
                return "八";
            case 9:
                return "九";
            case 10:
                return "十";
            case 11:
                return "十一";
            case 12:
                return "十二";
            case 13:
                return "十三";
            case 14:
                return "十四";
            case 15:
                return "十五";
            case 16:
                return "十六";
            case 17:
                return "十七";
            case 18:
                return "十八";
            case 19:
                return "十九";
            case 20:
                return "二十";
            default:
                return "一";
        }
    }
    #endregion

    #region 获得称谓
    //获得称谓
    private string getChengWei(int truecount)
    {
        string QuestionsChengWei = ub.GetSub("QuestionsChengWei", xmlPath);
        int QuestionsChengWeiDengji = Convert.ToInt32(ub.GetSub("QuestionsChengWeiDengji", xmlPath));
        string[] QuestionsChengWei2 = QuestionsChengWei.Split('#');
        string chengwei = QuestionsChengWei2[0].ToString();
        for (int j = 1; j < QuestionsChengWei2.Length; j++)
        {
            if ((truecount / (QuestionsChengWeiDengji * (j))) > 0)
            {
                chengwei = QuestionsChengWei2[j - 1].ToString();
            }
            else
            { continue; }
        }
        return chengwei;
    }
    #endregion

    //返回ishit的类型
    private string IsHit(int ishit)
    {
        //0 已领 1未领 2过期 3答题超时
        string text = "";
        switch (ishit)
        {
            case 1:
                text = "领取";
                break;
            case 2:
                text = "已过期";
                break;
            case 3:
                text = "答题超时";
                break;
            case 4:
                text = "答错无奖励";
                break;
            case 5:
                text = "已取消";
                break;
            case 0:
                text = "已领取";
                break;
            default:
                text = "暂无";
                break;
        }
        return text;
    }

    //主页
    protected void ReloadPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(GameName);
        builder.Append(Out.Tab("</div>", "<br/>"));
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[0-5]\d*$", "5"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【游戏管理】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=goto") + "\">发布问题</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=answer") + "\">答题记录</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question") + "\">问题管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=style") + "\">问题类型</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paihang") + "\">榜单管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=data") + "\">数据分析</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【游戏设置】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=set") + "\">游戏配置</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=rule") + "\">查看规则</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=reset") + "\">重置游戏</a>");
        builder.Append(Out.Tab("</div>", ""));
        //底部
        Bottom();
    }

    //style 问题类型
    protected void QuestionStylePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;类型管理");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string name = (Utils.GetRequest("name", "all", 2, @"^[^\^]{1,20}$", "游戏名字不能为空"));
            int price = Convert.ToInt32(Utils.GetRequest("price", "all", 2, @"^[0-9]\d*$", "最低下注不能为空"));
            BCW.Model.tb_QuestionsType model = new BCW.Model.tb_QuestionsType();
            //int id = new BCW.BLL.tb_QuestionsType().g();
            model.AddTime = DateTime.Now;
            model.ident = 1;
            model.Name = name;
            model.rank = price;
            model.Remark = "";
            model.statue = 1;
            model.type = "1";
            new BCW.BLL.tb_QuestionsType().Add(model);
            Utils.Success("设置", "添加成功，正在返回..", Utils.getUrl("questions.aspx?act=style&amp;backurl=" + Utils.getPage(0) + ""), "3");
        }
        else
        {
            string name = (Utils.GetRequest("name", "all", 1, @"^[^\^]{1,20}$", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));

            builder.Append(Out.Tab("</div>", ""));
            string strText1 = "问题类型:,总体难度:,";
            string strName1 = "name,price,backurl";
            string strType1 = "text,select,hidden";
            string strValu1 = "" + name + "'" + "'" + Utils.getPage(0);
            string strEmpt1 = "true," + q_deficult + ",hidden";
            string strIdea1 = "/";
            string strOthe1 = "确定添加|reset,questions.aspx?act=style,post,1,red|blue";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            DataSet ds = new BCW.BLL.tb_QuestionsType().GetList("*", "ID>0 order by ID desc");
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("[已有类型(" + ds.Tables[0].Rows.Count + ")][总体难度]");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("" + "<a href=\"" + Utils.getUrl("questions.aspx?act=delstyle&amp;ID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]) + "") + "\">[删]</a>");
                    string s = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                    builder.Append(s + "：" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(ds.Tables[0].Rows[i]["rank"])] + "</font>");
                    builder.Append("<br/>");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
        }

        Bottom();
    }

    //check 回答查看
    protected void QuestionCheckAnswerPage()
    {
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;回答管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        // BCW.Model.tb_QuestionAnswer model = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID);
        if (!new BCW.BLL.tb_QuestionAnswer().Exists(ID))
        {
            Utils.Error("不存在的问题记录！", "");
        }
        else
        {
            BCW.Model.tb_QuestionAnswer model = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID);
            BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(Convert.ToInt32(model.questID));
            //if (model.usid != meid)
            //{
            //    Utils.Error("不存在的回答记录！", "");
            //}
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("[回答记录] " + "<font color=\"green\">" + model.ID + "</font><br/>");
            builder.Append("[回答会员] " + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + model.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<font color=\"green\">" + model.usname + "</font>" + "</a>");
            string qtype = "";
            try
            { qtype = new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.type)); }
            catch { qtype = "随机题目"; }
            builder.Append("<br/>[题目类型]  " + "<font color=\"green\">" + qtype + "</font>");
            builder.Append("<br/>[难度系数] " + "<font color=\"green\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font>");
            builder.Append("<br/>[问题阐述] " + "<font color=\"green\">" + n.question + "</font>");
            if (n.img.Length > 5)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (n.style == 1)//4选1
            {
                builder.Append("(选择4选1)" + "<br/>[选项] ");
                if (n.answer == "A")
                {
                    builder.Append("<font color=\"green\">" + " A: " + n.chooseA + " </font>");
                }
                else
                    builder.Append(" A: " + n.chooseA);
                if (n.answer == "B")
                {

                    builder.Append("<font color=\"green\">" + " B: " + n.chooseB + " </font>");
                }
                else
                    builder.Append(" B: " + n.chooseB);
                if (n.answer == "C")
                {

                    builder.Append("<font color=\"green\">" + " C: " + n.chooseC + " </font>");
                }
                else
                    builder.Append(" C: " + n.chooseC);
                if (n.answer == "D")
                {

                    builder.Append("<font color=\"green\">" + " D: " + n.chooseD + " </font>");
                }
                else
                    builder.Append(" D: " + n.chooseD);
                builder.Append("<br/>[正确答案]  " + "<font color=\"green\">" + n.answer + "</font>");
            }
            else if (n.style == 2)//2选1
            {
                builder.Append("(选择2选1)" + "<br/>");
                builder.Append("[选项]  ");
                if (n.answer == "A")
                {
                    builder.Append("<font color=\"green\">" + " A: " + n.chooseA + " </font>");
                }
                else
                    builder.Append(" A: " + n.chooseA);
                if (n.answer == "B")
                {
                    builder.Append("<font color=\"green\">" + " B: " + n.chooseB + " </font>");
                }
                else
                    builder.Append(" B: " + n.chooseB);
                builder.Append("<br/>[正确答案] " + "<font color=\"green\">" + n.answer + "</font>");
            }
            else if (n.style == 3)//3选1
            {
                builder.Append("(选择3选1)" + "<br/>[选项] ");
                if (n.answer == "A")
                {
                    builder.Append("<font color=\"green\">" + " A: " + n.chooseA + " </font>");
                }
                else
                    builder.Append(" A: " + n.chooseA);
                if (n.answer == "B")
                {

                    builder.Append("<font color=\"green\">" + " B: " + n.chooseB + " </font>");
                }
                else
                    builder.Append(" B: " + n.chooseB);
                if (n.answer == "C")
                {

                    builder.Append("<font color=\"green\">" + " C: " + n.chooseC + " </font>");
                }
                else
                    builder.Append(" C: " + n.chooseC);
                builder.Append("<br/>[正确答案] " + "<font color=\"green\">" + n.answer + "</font>");
            }
            else if (n.style == 4)//判断
            {
                builder.Append("(判断题)" + "<br/>");
                if (n.answer.Contains("正确"))
                {
                    builder.Append("[正确答案] " + "<font color=\"green\">" + "√" + "</font>");
                }
                else
                {
                    builder.Append("[正确答案] " + "<font color=\"green\">" + "×" + "</font>");
                }
            }
            else if (n.style == 5)//填空
            {
                builder.Append("(填空题)" + "<br/>");
                builder.Append("[正确答案] " + "<font color=\"green\">" + n.answer + "</font>");
            }
            builder.Append("<br/>[ta的答案] " + "<font color=\"green\">" + model.answer + "</font>");
            if (model.isTrue == 1)
            { builder.Append("<font color=\"green\">" + "(正确)" + "</font>"); }
            else
            { builder.Append("<font color=\"red\">" + "(错误)" + "</font>"); }
            double percent = Convert.ToDouble(n.trueAnswer) / Convert.ToDouble((n.trueAnswer + n.falseAnswer));
            // builder.Append("<br/>答对: " + "<font color=\"red\">" + n.trueAnswer + "</font>");
            //if (percent > 0)
            //    builder.Append(" <br/>[题正确率] " + "<font color=\"red\">" + (percent * 100).ToString() + "%</font>");
            builder.Append(" <br/>[回答时间] " + "<font color=\"black\">" + Convert.ToDateTime(model.addtime).ToString("yyyy-MM-dd HH:mm") + "</font>");
            if (n.remarks == "")
                n.remarks = "暂无";
            builder.Append("<br/>[问题解析] " + "<font color=\"green\">" + n.remarks + "</font>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入回答记录:/,";
            string strName = "ID,backurl";
            string strType = "num,hidden";
            string strValu = ID + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜问题记录,questions.aspx?act=checkanswer&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        Bottom();

    }

    //数据分析
    private void DataPage()
    {
        Master.Title = "数据分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;数据分析");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strWhere = "";
        string outputtext = "";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("按答题数 | ");
            strWhere = "ID>0 group by usid order by count desc ";
            outputtext = "总答";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=data&amp;ptype=1&amp;") + "\">按答题数</a> | ");

        }
        if (ptype == 2)
        {
            builder.Append("按领奖额  ");
            strWhere = "ID>0 and isTrue=1 group by usid order by count desc ";
            outputtext = "答对";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=data&amp;ptype=2&amp;") + "\">按领奖额</a>  ");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));

        #region 按领奖额
        if (ptype == 2)
        {
            try
            {
                //今日
                string str = "ishit=0  and Year(overtime) = " + DateTime.Now.Year + "" + " and Month(overtime) = " + DateTime.Now.Month + "and Day(overtime) = " + DateTime.Now.Day;
                DataSet ds = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allToday", str);
                DataSet ds1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as allTodayMan", str);
                builder.Append("【今日领奖】" + ds.Tables[0].Rows[0]["allToday"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch
            { builder.Append("【今日领奖】" + 0 + ub.Get("SiteBz")); }
          //  try
            {
                //昨日
                // string strYesterday = "ishit=0 and Year(overtime) = " + (DateTime.Now.Year) + "and Month(overtime) = " + DateTime.Now.Month + " and Day(overtime) = " + (DateTime.Now.Day - 1) + "";
                string strYesterday = "   ishit=0 and DateDiff(dd,addtime,getdate())=1";
                DataSet dsYesterday = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allYesterday", strYesterday);
                if (dsYesterday.Tables[0].Rows.Count <1)
                    builder.Append("【昨日领奖】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【昨日领奖】" + dsYesterday.Tables[0].Rows[0]["allYesterday"] + ub.Get("SiteBz"));
            }
          //  catch { builder.Append("【昨日领奖】" + 0 + ub.Get("SiteBz")); }
            try
            {
                //本周
                builder.Append("<br/>");
                string strWeek = "ishit=0 and datediff(week,[overtime],getdate())=0  ";// Year(AddTime) =+(DateTime.Now.Year) + " and Week(AddTime) = " + (DateTime.Now.DayOfWeek) + "";
                DataSet dsWeek = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allWeek", strWeek);
                DataSet dsWeek1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as allWeekMan", strWeek);
                if (dsWeek.Tables[0].Rows.Count < 1)
                    builder.Append("【本周领奖】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【本周领奖】" + dsWeek.Tables[0].Rows[0]["allWeek"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch { builder.Append("【本周领奖】" + 0 + ub.Get("SiteBz")); }
            try
            {
                //本月
                string strMonth = "ishit=0 and Year(addtime) = " + (DateTime.Now.Year) + " and Month(addtime) = " + (DateTime.Now.Month) + "";
                DataSet dsMonth = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allMonth", strMonth);
                DataSet dsMonth1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as allMonthMan", strMonth);
                if (dsMonth.Tables[0].Rows.Count < 1)
                    builder.Append("【本月领奖】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【本月领奖】" + dsMonth.Tables[0].Rows[0]["allMonth"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch { builder.Append("【本月领奖】" + 0 + ub.Get("SiteBz")); }
            try
            {
                string strAllnot = "ishit=1";
                DataSet dsAllnot = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allALLnot", strAllnot);
                DataSet dsAllnot1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as strAllnotMan", strAllnot);
                if (dsAllnot.Tables[0].Rows.Count < 1)
                    builder.Append("【未领取额】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【未领取额】" + dsAllnot.Tables[0].Rows[0]["allALLnot"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch
            { builder.Append("【未领取额】" + 0 + ub.Get("SiteBz")); }
            try
            {
                string strAllnot = "ishit=5";
                DataSet dsAllnot = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allALLnot", strAllnot);
                DataSet dsAllnot1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as strAllnotMan", strAllnot);
                if (dsAllnot.Tables[0].Rows.Count < 1)
                    builder.Append("【已取消额】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【已取消额】" + dsAllnot.Tables[0].Rows[0]["allALLnot"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch
            { builder.Append("【已取消额】" + 0 + ub.Get("SiteBz")); }
            try
            {
                string strAllPass = "ishit=2";
                DataSet dsAllPass = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allALLPass", strAllPass);
                DataSet dsAllPass1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as strAllPassMan", strAllPass);
                if (dsAllPass.Tables[0].Rows[0]["allALLPass"] == null)
                    builder.Append("【已过期额】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【已过期额】" + dsAllPass.Tables[0].Rows[0]["allALLPass"] + ub.Get("SiteBz"));
                // builder.Append("【已过期额】" + dsAllPass.Tables[0].Rows[0]["allALLPass"] + ub.Get("SiteBz") + "[" + dsAllPass1.Tables[0].Rows[0]["strAllPassMan"] + "人]");
                builder.Append("<br/>");
            }
            catch { builder.Append("【已过期额】" + 0 + ub.Get("SiteBz")); }
            try
            {
                string strAllPass3 = "ishit=3";
                DataSet dsAllPass3 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allALLPass", strAllPass3);
                if (dsAllPass3.Tables[0].Rows.Count < 1)
                    builder.Append("【答题超时】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【答题超时】" + dsAllPass3.Tables[0].Rows[0]["allALLPass"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch { builder.Append("【答题超时】" + 0 + ub.Get("SiteBz")); }
            try
            {
                string strAllPass = "ishit=4";
                DataSet dsAllPass = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allALLPass", strAllPass);
                DataSet dsAllPass1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as strAllPassMan", strAllPass);
                if (dsAllPass.Tables[0].Rows.Count < 1)
                    builder.Append("【答错金额】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【答错金额】" + dsAllPass.Tables[0].Rows[0]["allALLPass"] + ub.Get("SiteBz"));
                builder.Append("<br/>");
            }
            catch { builder.Append("【答错金额】" + 0 + ub.Get("SiteBz")); }
            try
            {
                //总
                string strAll = "ishit=0";
                if (start != "0")
                {
                    strAll = strAll + " and addtime> '" + Convert.ToDateTime(start) + "' and addtime< '" + Convert.ToDateTime(down) + "'";
                }
                else
                {
                    start = DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd HH:mm:ss");
                    down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DataSet dsAll = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("sum(awardgold) as allALL", strAll);
                DataSet dsAll1 = new BCW.BLL.tb_QuestionsAnswerCtr().GetList("count(*) as allALLMan", strAll);
                if (dsAll.Tables[0].Rows.Count < 1)
                    builder.Append("【总领奖额】" + 0 + ub.Get("SiteBz"));
                else
                    builder.Append("【总领奖额】" + dsAll.Tables[0].Rows[0]["allALL"] + ub.Get("SiteBz"));
                builder.Append(Out.Tab("</div>", ""));
            }
            catch
            { builder.Append("【总领奖额】" + 0 + ub.Get("SiteBz")); }
        }
        #endregion


        #region 按答题数
        else
        {
            try
            {
                //今日
                // builder.Append("<br/>");
                string str = "  Year(addtime) = " + DateTime.Now.Year + "" + " and Month(addtime) = " + DateTime.Now.Month + "and Day(addtime) = " + DateTime.Now.Day;
                DataSet ds1 = new BCW.BLL.tb_QuestionAnswer().GetList("count(*) as allTodayMan", str);
                builder.Append("【今日】" + ds1.Tables[0].Rows[0]["allTodayMan"]);

            }
            catch
            { builder.Append("【今日】" + 0); }
            try
            {
                //昨日
                builder.Append("<br/>");
                string strYesterday = " Year(addtime) = " + (DateTime.Now.Year) + "and Month(addtime) = " + DateTime.Now.Month + " and Day(addtime) = " + (DateTime.Now.Day - 1) + "";
                DataSet dsYesterday = new BCW.BLL.tb_QuestionAnswer().GetList("sum(awardgold) as allYesterday", strYesterday);
                if (dsYesterday.Tables[0].Rows.Count <= 1)
                    builder.Append("【昨日】" + 0);
                else
                    builder.Append("【昨日】" + dsYesterday.Tables[0].Rows[0]["allYesterday"]);
            }
            catch { builder.Append("【昨日】" + 0); }
            try
            {
                //本周
                builder.Append("<br/>");
                string strWeek = "datediff(week,[addtime],getdate())=0  ";
                DataSet dsWeek = new BCW.BLL.tb_QuestionAnswer().GetList("count(*) as allWeekMan", strWeek);
                if (dsWeek.Tables[0].Rows[0]["allWeekMan"] == null)
                    builder.Append("【本周】" + 0);
                else
                    builder.Append("【本周】" + dsWeek.Tables[0].Rows[0]["allWeekMan"]);
            }
            catch { builder.Append("【本周】" + 0); }
            try
            {
                //本月
                builder.Append("<br/>");
                string strMonth = "ishit=0 and Year(addtime) = " + (DateTime.Now.Year) + " and Month(addtime) = " + (DateTime.Now.Month) + "";
                DataSet dsMonth = new BCW.BLL.tb_QuestionAnswer().GetList("count(*) as allMonthMan", strMonth);
                if (dsMonth.Tables[0].Rows[0]["allMonthMan"] == null)
                    builder.Append("【本月】" + 0);
                else
                    builder.Append("【本月】" + dsMonth.Tables[0].Rows[0]["allMonthMan"]);
            }
            catch { builder.Append("【本月】" + 0); }
            //try
            //{
            //    builder.Append("<br/>");
            //    string strAllPass3 = "ishit=3";
            //    DataSet dsAllPass3 = new BCW.BLL.tb_QuestionAnswer().GetList("count(*) as allALLPass", strAllPass3);
            //    if (dsAllPass3.Tables[0].Rows[0]["allALLPass"] == null)
            //        builder.Append("【答题超时数】" + 0);
            //    else
            //        builder.Append("【答题超时数】" + dsAllPass3.Tables[0].Rows[0]["allALLPass"]);
            //}
            //catch { builder.Append("【答题超时数】" + 0);  }
            //try
            //{
            //    builder.Append("<br/>");
            //    string strAllPass = "ishit=4";
            //    DataSet dsAllPass = new BCW.BLL.tb_QuestionAnswer().GetList("sum(awardgold) as allALLPass", strAllPass);
            //    DataSet dsAllPass1 = new BCW.BLL.tb_QuestionAnswer().GetList("count(*) as strAllPassMan", strAllPass);
            //    if (dsAllPass.Tables[0].Rows[0]["allALLPass"] == null)
            //        builder.Append("【答题错误数】" + 0 );
            //    else
            //        builder.Append("【答题错误数】" + dsAllPass.Tables[0].Rows[0]["allALLPass"] );              
            //}
            //catch { builder.Append("【答题错误数】" + 0 );  }
            try
            {
                //总
                builder.Append("<br/>");
                string strAll = " ";
                if (start != "0")
                {
                    strAll = "  addtime> '" + Convert.ToDateTime(start) + "' and addtime< '" + Convert.ToDateTime(down) + "'";
                }
                else
                {
                    start = DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd HH:mm:ss");
                    down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DataSet dsAll1 = new BCW.BLL.tb_QuestionAnswer().GetList("count(*) as allALLMan", strAll);
                if (dsAll1.Tables[0].Rows[0]["allALLMan"] == null)
                    builder.Append("【总答题数】" + 0);
                else
                    builder.Append("【总答题数】" + dsAll1.Tables[0].Rows[0]["allALLMan"]);
                builder.Append(Out.Tab("</div>", ""));
            }
            catch
            { builder.Append("【总答题数】" + 0); }

        }

        #endregion
        string strText1 = "开始时间:,结束时间:,,";
        string strName1 = "start,down,ptype,backurl";
        string strType1 = "text,text,hidden,hidden";
        string strValu1 = start + "'" + down + "'" + ptype + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,true,true,false";
        string strIdea1 = "/";
        string strOthe1 = "按时间搜索,questions.aspx?act=data&amp;ptype=" + ptype + "&amp;,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("温馨提示:时间搜索仅搜索该段时间的总数或总额.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //paihang 排行榜单
    protected void TopListPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>" + "&gt;" + "排行榜");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        string str = "ID>0 group by usid order by count desc ";
        string outputtext = "";
        if (ptype == 1)
        {
            builder.Append("总量榜 | ");
            str = "ID>0 group by usid order by count desc ";
            outputtext = "总答";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paihang&amp;ptype=1&amp;") + "\">总量榜</a> | ");

        }
        if (ptype == 2)
        {
            builder.Append("答对榜 | ");
            str = "ID>0 and isTrue=1 group by usid order by count desc ";
            outputtext = "答对";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paihang&amp;ptype=2&amp;") + "\">答对榜</a> | ");
        }
        if (ptype == 3)
        {
            builder.Append("称谓榜");
            str = "ID>0 and isTrue=1 group by usid order by count desc ";
            outputtext = "答对";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paihang&amp;ptype=3&amp;") + "\">称谓榜</a>");
        }

        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        //  try
        {
            string QuestionsChengWei = ub.GetSub("QuestionsChengWei", xmlPath);
            int QuestionsChengWeiDengji = Convert.ToInt32(ub.GetSub("QuestionsChengWeiDengji", xmlPath));
            string[] QuestionsChengWei2 = QuestionsChengWei.Split('#');
            string chengwei = "";
            int dengji = 0;
            int sum;
            DataSet ds = new BCW.BLL.tb_QuestionAnswer().GetList("usid,count(usid)as count", str);
            sum = 9;
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string sOrder = "";
            string[] pageValUrl = { "act", "ptype", "id", "backurl" };
            int truecount = 0;
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
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
                    { builder.Append(Out.Tab("<div >", "<br/>")); }
                    else
                    {
                        if (i == 1)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        else
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));

                    }
                    if (ptype == 3)
                    {
                        builder.Append("<font color=\"red\">" + "[" + (koo + i + 1) + "]" + "</font>");
                    }
                    else
                        builder.Append("<font color=\"red\">" + "[TOP " + (koo + i + 1) + "]" + "</font>");
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]));
                    builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"])) + "</a>");
                    //  builder.Append("(" + ds.Tables[0].Rows[koo + i]["UserId"] + ")");
                    if (ptype == 3)
                    {
                        truecount = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["count"].ToString());
                        if (i < 3)
                        {
                            builder.Append(" <b><font  color=\"#DD22DD\">" + getChengWei(truecount) + "</font></b>");
                        }
                        else
                            builder.Append(" <font  color=\"green\">" + getChengWei(truecount) + "</font>");
                    }
                    else
                    {
                        builder.Append(outputtext);
                        builder.Append(ds.Tables[0].Rows[koo + i]["count"] + "题");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("暂无相关排行");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        //  catch { builder.Append("暂无相关排行,快快去加入动态吧！"); }

        Bottom();
    }

    //paijiang 派奖
    protected void PaiJiangPage()
    {
        Master.Title = GameName;
        //int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("兑奖");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "0"));//ID列
        string ac= Utils.GetRequest("ac", "all", 1, @"", "0");
        BCW.Model.tb_QuestionsAnswerCtr model = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(ID);
        int sendGold = Convert.ToInt32(model.awardgold);
        #region 算出奖金多少 弃用
        //if (model.awardtype == 0)//0 固定奖金 1红包形式 已注释
        //{
        //    sendGold = Convert.ToInt32(model.awardgold);
        //}
        //else if (model.awardtype == 1)
        //{
        //    //采用红包算法算出金额
        //    Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
        //    //  if()
        //    int index = ran1.Next(1, Convert.ToInt32(model.awardgold) + 1);
        //    sendGold = index;
        //}
        #endregion

        if (ac == "ok")
        {
            #region
            string OutText = Convert.ToString(ub.GetSub("QuestionsSelectText", "/Controls/questions.xml"));
            int checkhour = Convert.ToInt32(ub.GetSub("QuestionsPassTime", "/Controls/questions.xml"));
            string[] getText = OutText.Split('#');
            Random index_n = new Random();
            string Print_text = "";
            Print_text = getText[index_n.Next(0, getText.Length)];
            TimeSpan ts = new TimeSpan();
            ts = Convert.ToDateTime(model.overtime) - Convert.ToDateTime(model.addtime);
            DateTime check = Convert.ToDateTime(model.addtime).AddHours(checkhour);
            int actionid = 0;
            if (sendGold > 0 && model.ishit == 1)//1未领 0已领
            {
                if (ts.Minutes < 48 * 60 && check > DateTime.Now)//答题时限内，并且在有效领取时间内
                {
                    if (QuestionsStatue == "2")//测试
                    {
                        if (!new BCW.SWB.BLL().ExistsUserID(Convert.ToInt32(model.uid), GameId))//不存在用户记录直接领
                        {
                            BCW.SWB.Model swbs = new BCW.SWB.Model();
                            swbs.UserID = Convert.ToInt32(model.uid);
                            swbs.UpdateTime = DateTime.Now;
                            swbs.Money = 0 + sendGold;
                            swbs.GameID = GameId;
                            swbs.Permission = 1;
                            try
                            {
                                BCW.User.Users.IsFresh("questions", 1);//防刷
                                int id = new BCW.SWB.BLL().Add(swbs);

                            }
                            catch { builder.Append("兑奖失败！请重新兑奖！"); }
                        }
                        else
                        {
                            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(Convert.ToInt32(model.uid), GameId);
                            builder.Append("当前测试币：" + swb.Money + "<BR/>");
                            swb.UpdateTime = DateTime.Now;
                            swb.Money += sendGold;
                            swb.Permission += 1;
                            builder.Append("领取后测试币：" + swb.Money + "<BR/>");
                            try
                            {
                                BCW.User.Users.IsFresh("questions", 1);//防刷
                                new BCW.SWB.BLL().Update(swb);
                            }
                            catch { builder.Append("兑奖失败！请重新兑奖！"); }
                        }
                        actionid = new BCW.BLL.Action().Add(Convert.ToInt32(model.uid), "在[URL=/bbs/game/questions.aspx]" + GameName + "[/URL]" + "回答奖励" + sendGold + "问币");
                        model.awardgold = sendGold;
                        model.ishit = 0;//成功领取                     
                    }
                    else//开放
                    {
                        new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.uid), sendGold, GameName + "第" + model.ID + "轮兑奖");
                        string uname = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.uid));
                        actionid = new BCW.BLL.Action().Add(Convert.ToInt32(model.uid), "在[URL=/bbs/game/questions.aspx]" + GameName + "[/URL]" + "回答奖励" + sendGold + ub.Get("SiteBz"));
                        model.awardgold = sendGold;
                        model.ishit = 0;//成功领取
                    }
                }
                else
                {
                    if (check < DateTime.Now)
                    {
                        model.ishit = 2;//过期
                    }
                    else if (ts.Minutes < 48 * 60)
                    {
                        model.ishit = 3;//答题超时
                    }
                    // model.awardgold = 0;
                }
                //model.overtime = DateTime.Now;
                //更新获取标识        
                new BCW.BLL.tb_QuestionsAnswerCtr().Update(model);
                if (actionid > 0)
                    Utils.Success("派奖成功！", "派奖成功,3s后跳转到..", Utils.getUrl("questions.aspx?act=ansctr&amp;ID=" + ID + "&amp;"), "2");
                else
                    Utils.Success("派奖失败！", "派奖失败,该答题时间已过期或者答题超时！3s后跳转回上级..", Utils.getUrl("questions.aspx?act=ansctr&amp;ID=" + ID + "&amp;"), "2");
            }
            else
            {
                builder.Append("【兑奖提示】 <font color =\"#FF0000\">" + "恭喜,本次已成功兑奖！" + "</font>" + "<br/>");
            }
            builder.Append("【本轮记录】 第<font color =\"green\">" + model.contrID + "</font>轮");
            builder.Append("<br/>");
            builder.Append("【题目总数】 " + model.count + " 题");
            builder.Append("<br/>");
            builder.Append("【答对题数】 " + model.trueCount + " 题");
            builder.Append("<br/>");
            builder.Append("【回答时间】 " + Convert.ToDateTime(model.addtime).ToString("yyyy-MM-dd HH:mm") + "");
            builder.Append("<br/>");
            builder.Append("【结束时间】 " + Convert.ToDateTime(model.overtime).ToString("yyyy-MM-dd HH:mm") + "");
            builder.Append("<br/>");
            builder.Append("【花费时间】 <font color =\"green\">" + ts.Minutes + "</font> 分钟");
            builder.Append("<br/>");
            builder.Append("【奖金价值】  <font color =\"green\">" + sendGold + " </font>" + ub.Get("SiteBz") + "");
            builder.Append("<br/>");
            if (model.ishit == 0)
                builder.Append("【领取状态】 " + "已领取" + "");
            else if (model.ishit == 1)
                builder.Append("【领取状态】 " + "未领取" + "");
            else if (model.ishit == 2)
                builder.Append("【领取状态】 " + "已过期" + "");
            else if (model.ishit == 3)
                builder.Append("【领取状态】 " + "答题超时" + "");
            else if (model.ishit == 3)
                builder.Append("【领取状态】 " + "答错无奖励" + "");
            if (Print_text != "")
                builder.Append("<br/>【温馨提示】 <font color =\"#FF0000\">" + Print_text + "</font>" + "<br/>");
            #endregion
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=ansctr&amp;ptype=2&amp;ID=" + ID + "&amp;") + "\">" + "返回上级" + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("是否确定派奖？");
            builder.Append(" <br/> <a href=\"" + Utils.getUrl("questions.aspx?act=paijiang&amp;ID=" + ID + "&amp;ac=ok&amp;") + "\">" + "是,进行派奖" + "</a>");
            builder.Append("<br/><a href=\"" + Utils.getUrl("questions.aspx?act=ansctr&amp;ptype=2&amp;ID=" + ID + "&amp;") + "\">" + "否,返回上级" + "</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
          
        }
       
        Bottom();
    }

    //goto 发布问题
    protected void GotoPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 11)
        //{
        //    Utils.Error("只有系统管理员1号才可以进行此操作", "");
        //}
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;发布问题");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string[] typed = { "选择题", "填空题", "判断题" };
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[^\^]{0,20}$", "1"));
        string info = Utils.GetRequest("info", "all", 1, @"", "");
        string infoNext = Utils.GetRequest("infoNext", "all", 1, @"", "");
        string question = "";//问题
        int type = 0;//问题类型
        string answer = "";//问题答案
        string choose = "";
        //问题输入完毕 保存问题
        if (info == "ok")
        {
            BCW.Model.tb_QuestionsList model = new BCW.Model.tb_QuestionsList();//记录每个问题的数据
            BCW.Model.tb_QuestionController controller = new BCW.Model.tb_QuestionController();//记录到问题控制器
            if (infoNext == "ok")
            {
                string idList = "";
                string[] an;
                int style = 0;
                int id = 0;//记录数据库中的问题ID
                string title = "随机题目";

                #region 记录问题到数据库
                for (int i = 0; i < num; i++)
                {
                    question = Utils.GetRequest("num" + i.ToString(), "all", 1, @"^[^\^]{0,20000}$", "1");
                    type = int.Parse(Utils.GetRequest("type" + i.ToString(), "all", 1, @"^[^\^]{0,20000}$", "1"));
                    answer = (Utils.GetRequest("answer" + i.ToString(), "all", 1, @"^[^\^]{0,20000}$", "1"));
                    choose = (Utils.GetRequest("choose" + i.ToString(), "all", 1, @"^[^\^]{0,20000}$", "1"));
                    if (new BCW.BLL.tb_QuestionsType().GetName(type) != "")
                    {
                        title = new BCW.BLL.tb_QuestionsType().GetName(type);
                    }
                    else
                    {
                        title = "";
                    }
                    builder.Append("问题" + (i + 1) + "：" + question);
                    builder.Append("<br/>");
                    builder.Append("问题类型：" + "<font color=\"green\">" + title + "</font>");
                    builder.Append("<br/>");
                    builder.Append("正确答案：" + answer);
                    builder.Append("<br/>");
                    builder.Append("问题" + (i + 1) + "." + "保存成功！");
                    builder.Append("<br/>");
                    an = choose.Split('#');
                    model.addtime = DateTime.Now;
                    model.answer = answer;//正确答案
                    model.awardGold = 0;//奖品
                    model.awardId = 1;//奖品ID
                    model.awardType = "1";//奖品类型  
                    model.chooseA = "";
                    model.chooseB = "";
                    model.chooseC = "";
                    model.chooseD = "";
                    if (choose.Contains("#"))
                    {
                        model.chooseA = an[0];//选项A                    
                        if (an.Length >= 2)
                        {
                            model.chooseB = an[1];
                            style = 2;
                        }
                        if (an.Length >= 3)
                        {
                            model.chooseC = an[2];
                            style = 3;
                        }
                        if (an.Length >= 4)
                        {
                            model.chooseD = an[3];
                            style = 1;//4选一
                        }
                    }
                    else
                    {
                        style = 5;//填空题
                    }
                    if (choose == "0")
                    {
                        style = 5;//填空题
                    }
                    if (choose == "")
                    {
                        style = 4;
                    }
                    // Utils.Error("" + style + "", "");
                    model.deficult = 5;//难度
                    model.falseAnswer = 3;//答错次数
                    model.hit = 0;//点击次数
                    model.img = "";//是否存在图片
                    model.indent = "1";//
                    model.question = question;//问题
                    model.remarks = "";
                    model.statue = 1;
                    model.style = style;//问题类型 1 2  4 5
                    model.styleID = type.ToString();//类型ID
                    model.title = title;//
                    model.trueAnswer = 0;
                    model.type = "";
                    id = new BCW.BLL.tb_QuestionsList().Add(model);
                    idList += id.ToString() + "#";
                    //   builder.Append("***" + "A:" + model.chooseA + "B:" + model.chooseB + "C:" + model.chooseC + "D:" + model.chooseD + "***");
                }
                #endregion 

                //builder.Append(idList);
                controller.acount = 0;
                controller.addtime = DateTime.Now;
                controller.award = 0;
                controller.awardptype = 0;
                controller.awardtype = 0;
                controller.count = num;
                controller.List = idList;
                controller.muid = ManageId;
                controller.overtime = DateTime.Now.AddDays(7);
                controller.passtime = 0;
                controller.remark = "";
                controller.type = 0;
                controller.ubbtext = "";
                controller.uid = "";
                controller.uided = "";
                controller.wcount = 0;
                controller.ycount = "0";
                int qId = 0;
                qId = new BCW.BLL.tb_QuestionController().Add(controller); ;
                //加入控制器
                builder.Append("");
                builder.Append("<font color=\"red\">问题添加成功！</font>");
                // builder.Append("<br/><a href=\"" + Utils.getUrl("questions.aspx?act=select&amp;id=" + Id + "") + "\">" + "进行选择发布" + "</a>");
                if (qId == 0)
                {
                    Utils.Error("问题添加失败,请返回继续添加" + "<a href=\"" + Utils.getUrl("questions.aspx?act=goto&amp;qId=" + qId + "&amp;") + "\">" + "返回继续" + "</a>", "");
                }
                else
                    Utils.Success("问题添加成功！", "问题添加成功!3s后进入选择发布..", Utils.getUrl("questions.aspx?act=select&amp;qId=" + qId + ""), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">");
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"goto\"/>");
                builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok\"/>");
                builder.Append("<input type=\"hidden\" name=\"infoNext\" Value=\"ok\"/>");
                builder.Append("<input type=\"hidden\" name=\"num\" Value=\"" + num + "\"/>");
                for (int i = 0; i < num; i++)
                {
                    question = Utils.GetRequest("num" + i.ToString(), "all", 1, @"^[^\^]{0,2000}$", "");
                    type = int.Parse(Utils.GetRequest("type" + i.ToString(), "all", 1, @"^[^\^]{0,2000}$", "1"));
                    answer = (Utils.GetRequest("answer" + i.ToString(), "all", 1, @"^[^\^]{0,2000}$", ""));
                    choose = (Utils.GetRequest("choose" + i.ToString(), "all", 1, @"^[^\^]{0,2000}$", "1"));
                    if (question == "")
                    { Utils.Error("问题" + (i + 1) + "不能为空!", ""); }
                    if (answer == "")
                    { Utils.Error("问题" + (i + 1) + "的回答不能为空!", ""); }
                    builder.Append("问题" + (i + 1) + "：" + question);
                    builder.Append("<br/>");
                    builder.Append("问题类型：" + type + "<font color=\"green\">" + new BCW.BLL.tb_QuestionsType().GetName(type) + "</font>");
                    builder.Append("<br/>");
                    builder.Append("问题选项：" + choose);
                    builder.Append("<br/>");
                    builder.Append("正确答案：" + answer);
                    builder.Append("<br/>--------<br/>");
                    builder.Append("<input type=\"hidden\" name=\"" + "num" + i.ToString() + "\" Value=\"" + question + "\"/>");
                    builder.Append("<input type=\"hidden\" name=\"" + "type" + i.ToString() + "\" Value=\"" + type + "\"/>");
                    builder.Append("<input type=\"hidden\" name=\"" + "answer" + i.ToString() + "\" Value=\"" + answer + "\"/>");
                    builder.Append("<input type=\"hidden\" name=\"" + "choose" + i.ToString() + "\" Value=\"" + choose + "\"/>");
                    // System.Threading.Thread.Sleep(100);
                    if (i < num - 1)
                        builder.Append("<br/>");
                }
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定填写\"/>");
                builder.Append("</form>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            DataSet ds = new BCW.BLL.tb_QuestionsType().GetList(" * ", " ID>0 ");
            string question_type = "0|随机题目|";//问题类型（语文数学脑筋急转弯等）
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //题目类型
                    question_type += ds.Tables[0].Rows[i]["ID"].ToString() + "|" + ds.Tables[0].Rows[i]["Name"].ToString() + "|";
                }
            }
            question_type = question_type.Substring(0, question_type.Length - 1);
            builder.Append("温馨提示:为问答有奖添加问题,问题|选项支持UBB" + "<br/>");
            builder.Append("1.题目:直接填写题目.格式如:<font color=\"green\">\"大煮干丝\"是哪个菜系的代表菜之一().</font>");
            builder.Append("<br/>");
            builder.Append("2.填写选项:选择题填格式如:<font color=\"green\">选项A(内容)#选项B#C#D,填空则直接填写0</font>");
            builder.Append("<br/>");
            builder.Append("如:<font color=\"red\">(4选1)四川菜系#山东菜系#广东菜系#淮扬菜系</font>"); builder.Append("<br/>");
            builder.Append("如:<font color=\"red\">(3选1)四川菜系#山东菜系#广东菜系</font>"); builder.Append("<br/>");
            builder.Append("如:<font color=\"red\">(2选1)四川菜系#山东菜系</font>");
            builder.Append("<br/>");
            builder.Append("3.填写答案:<font color=\"green\">选择题填写ABCD之一，填空则直接填写答案</font>");
            builder.Append("<br/>");
            builder.Append("选择填写如:<font color=\"red\">A</font>填空题填写:<font color=\"red\">四川菜系</font>");
            builder.Append(Out.Tab("</div>", "<br/>"));
            int max = 10;
            if (num > max)
                num = max;
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("问题:");
            for (int i = 1; i <= max; i++)
            {
                if (num == i)
                { builder.Append(" <font color=\"red\"><b>" + i + " </b></font>"); }
                else
                    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=goto&amp;id=" + 3 + "&amp;num=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>" + i + "</b></a> ");
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
                    ssText = ssText + "问题" + (i + 1) + ":/,问题" + (i + 1) + "类型:/,问题" + (i + 1) + "选项(格式如下A#B#C#D):/,问题" + (i + 1) + "正确答案:/,-----/";
                }
                else
                {
                    ssText = ssText + "问题" + (i + 1) + ":/,问题" + (i + 1) + "类型:/,问题" + (i + 1) + "选项(格式如下A#B#C#D):/,问题" + (i + 1) + "正确答案:/,-----/";
                }
                ssName = ssName + ("num" + i.ToString()) + "," + ("type" + i.ToString()) + "," + ("choose" + i.ToString()) + "," + ("answer" + i.ToString()) + ",";
                ssType = ssType + "text,select,text,text,";
                ssValu = ssValu + "'" + ds.Tables[0].Rows[0]["ID"].ToString() + "'''";
                ssEmpt = ssEmpt + y + question_type + "," + y + y;
            }
            string strUpgroup = string.Empty;
            strUpgroup = "" + strUpgroup;
            ssText = ssText + Utils.Mid(strText, 1, strText.Length) + "," + ",,,";
            ssName = ssName + Utils.Mid(strName, 1, strName.Length) + "info,act,num,";
            ssType = ssType + Utils.Mid(strType, 1, strType.Length) + "hidden,hidden,hidden,";
            ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "ok" + "'goto'" + num + "'";
            ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,";
            ssIdea = "";
            ssOthe = "确定添加|reset,questions.aspx?id=" + 2 + "&amp;,post,2,red|blue";
            builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
        }
        builder.Append(Out.Tab("", "<br/>"));
        Bottom();
    }

    //select 选择发布
    protected void SelectPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;继续发布");
        builder.Append(Out.Tab("</div>", ""));
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        string info = Utils.GetRequest("info", "all", 1, @"", "");
        string infonext = Utils.GetRequest("infonext", "all", 1, @"", "");
        ub xml = new ub();
        string xmlPath = "/Controls/questions.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        //发布开始
        if (info == "ok")
        {
            int type = int.Parse(Utils.GetRequest("type", "all", 1, @"", "0"));
            string IdList = (Utils.GetRequest("IdList", "all", 1, @"", "0"));
            string count = Utils.GetRequest("count", "all", 1, @"", "0");
            int time =Convert.ToInt32(xml.dss["QuestionsPassTime"].ToString());//int.Parse(Utils.GetRequest("time", "all", 1, @"", "0"));
            int suretype = int.Parse(Utils.GetRequest("suretype", "all", 1, @"", "0"));
            int awardtype = int.Parse(Utils.GetRequest("awardtype", "all", 1, @"", "0"));
            string award = Utils.GetRequest("award", "all", 1, @"", "0");
            string ubbtext = Utils.GetRequest("ubbtext", "all", 1, @"", "0");
            string[] type1 = { "在线会员", "隐身会员", "所有会员", "指定会员" };
            string[] suretype1 = { "全答有奖(无论对不对,只要全答了)", "全答对才有奖(全对)", "答了就有奖(无论对不对)" };
            string[] awardtype1 = { "固定奖金", "随机红包" };
            string question_deficult = "0|新手|1|初级|2|中级|3|高级|4|困难|5|大师";//难度
            string[] question_deficult_list = { "新手", "初级", "中级", "高级", "困难", "大师" };//难度
            if (infonext == "ok")
            {
                BCW.Model.tb_QuestionController controller = new BCW.BLL.tb_QuestionController().Gettb_QuestionController(qId);//记录到问题控制器
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("最终发布将同时向<font color=\"red\">" + type1[type] + "</font>发送内线:" + "<br/>");
                builder.Append("当前选着发布的类型:" + type1[type] + "<br/>");
                builder.Append("答对的奖励形式:" + suretype1[suretype] + "<br/>");
                builder.Append("红包奖励形式:" + awardtype1[awardtype] + "<br/>");
                builder.Append("(红包)奖金大小(范围0-值):" + award + "<br/>");
                builder.Append("最大可回答人数:" + count + "<br/>");
                builder.Append("内线语句:<font color=\"green\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(ubbtext)) + "</font>");
                builder.Append(Out.Tab("</div>", "<br/>"));
                //  QuestionsUbbText
                //存起xml 发送的内线
                //ub xml = new ub();
                //string xmlPath = "/Controls/questions.xml";
                //Application.Remove(xmlPath);//清缓存
                //xml.ReloadSub(xmlPath); //加载配置
                xml.dss["QuestionsUbbText"] = ubbtext;
                xml.dss["IdList"] = IdList;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                //存起数据 控制器
                string strWhere = "EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ";
                string replaceUbb = ubbtext.Replace("questions.aspx", "questions.aspx?act=contrl&amp;qId=" + qId + "");
                DataSet ds = new BCW.BLL.User().GetList(" * ", strWhere);
                int usid = 0;
                string usname = "";
                int guessCount = 0;
                string uidlist = "";
                if (type == 0)//在线会员
                {
                    ds = new BCW.BLL.User().GetList(" * ", strWhere);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            usid = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                            usname = Convert.ToString(ds.Tables[0].Rows[i]["UsName"]);
                            new BCW.BLL.Guest().Add(0, usid, usname, replaceUbb);
                            uidlist += usid.ToString() + "#";
                            guessCount++;
                        }
                        controller.addtime = DateTime.Now;
                        controller.acount = int.Parse(ds.Tables[0].Rows.Count.ToString());
                        controller.award = int.Parse(award);
                        controller.awardptype = awardtype;
                        controller.overtime = DateTime.Now.AddHours(time);
                        controller.passtime = time;
                        controller.remark = "";
                        controller.awardtype = suretype;
                        controller.ycount = count;//最大可答人数
                        controller.wcount = 0;//当前回答人数
                        controller.type = type;
                        controller.ubbtext = ubbtext;
                        controller.uid = uidlist;
                        controller.uided = "";
                        new BCW.BLL.tb_QuestionController().Update(controller);
                        Utils.Success("发送成功！", "向在线会员发送问答内线成功，当前已发送内线" + guessCount + "条,3s后返回", Utils.getUrl("questions.aspx?act=goto&amp;"), "4");
                        builder.Append("操作完成！");
                    }
                    else
                    {
                        builder.Append("当前在线人数:" + ds.Tables[0].Rows.Count + "<br/>");
                        // Utils.Error("发布错误！当前没有人在线，内线发布失败", "");
                    }
                }
                else
                    if (type == 1)//隐身会员
                {
                    Utils.Error("隐身会员过多,暂不支持隐身会员回答！", "");
                }
                else
                    if (type == 2)//所有会员
                {
                    Utils.Error("所有会员过多,暂不支持所有会员回答！", "");
                }
                else
                    if (type == 3)//指定会员
                {
                    // Utils.Error("出错啦","");
                    // Utils.Error(IdList,"");
                    string[] guessid = IdList.Split('#');
                    for (int i = 0; i < guessid.Length; i++)
                    {
                        usid = Convert.ToInt32(guessid[i]);
                        usname = new BCW.BLL.User().GetUsName(Convert.ToInt32(guessid[i]));
                        new BCW.BLL.Guest().Add(0, usid, usname, replaceUbb);
                        uidlist += usid.ToString() + "#";
                        guessCount++;//已发送内线的id
                    }
                    controller.addtime = DateTime.Now;
                    controller.acount = guessid.Length;
                    controller.award = int.Parse(award);
                    controller.awardptype = awardtype;
                    controller.overtime = DateTime.Now.AddHours(time);
                    controller.passtime = time;
                    controller.remark = "";
                    controller.type = type; //0|在线会员 |1|隐身会员|2| 所有会员|3|指定会员
                    controller.ubbtext = ubbtext;
                    controller.uid = uidlist;
                    controller.uided = "";
                    new BCW.BLL.tb_QuestionController().Update(controller);
                    Utils.Success("发送成功！", "向指定会员发送问答内线成功，当前已发送内线" + guessCount + "条,3s后返回", Utils.getUrl("questions.aspx?act=goto&amp;"), "4");
                    builder.Append("操作完成！");
                }
            }
            else
            {
            
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                builder.Append("<th>");
                builder.Append("发布将同时向<font color=\"green\">" + type1[type] + "</font>发送内线:" + "</th>");
                builder.Append("<tr><td>");
                builder.Append("当前选着发布的类型:</td><td><font color=\"green\">" + type1[type] + "</font></td></tr>");
                builder.Append("<tr><td>");
                builder.Append("答对的奖励形式:</td><td><font color=\"green\">" + suretype1[suretype] + "</font></td>");
                builder.Append("</tr><tr><td>");
                builder.Append("红包奖励形式:</td><td><font color=\"green\">" + awardtype1[awardtype] + "</font></td>");
                builder.Append("</tr><tr><td>");
                builder.Append("(红包)奖金大小(范围0-值):</td><td><font color=\"green\">" + award + "</font></td>");
                builder.Append("</tr><tr><td>");
                builder.Append("最大可回答人数:</td><td><font color=\"green\">" + count + "</font></td></tr>");
                builder.Append("<tr><td>");
                builder.Append("内线语句:</td><td><font color=\"green\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(ubbtext)) + "</font></td></tr>");
                builder.Append("</table>");
                //设置
                builder.Append(Out.Tab("</div>", ""));
                string strText = ":/,:/,:/,:/,:/,:/,:/,:/,";
                string strName = "type,IdList,count,time,suretype,awardtype,award,ubbtext,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,";
                string strValu = type + "'" + IdList + "'" + count + "'" + time + "'" + suretype + "'" + awardtype + "'" + award + "'" + ubbtext + "'" + Utils.getPage(0) + "";
                string strEmpt = "0|在线会员|1|隐身会员|2|所有会员|3|指定会员,true,,true,true,0|全答才有奖|1|全答对才有奖|2|答了就有奖,0|固定奖金|1|随机红包,true,false,false,";
                string strIdea = "";
                string strOthe = "确定发送,questions.aspx?act=select&amp;info=ok&amp;infonext=ok&amp;qId=" + qId + "&amp;,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else
        {
            //ub xml = new ub();
            //string xmlPath = "/Controls/questions.xml";
            //Application.Remove(xmlPath);//清缓存
            //xml.ReloadSub(xmlPath); //加载配置
            string strText = "选择发布:/,指定ID(#分隔|无则忽略):/,最大可回答人数(无限制为0):/,答对奖励形式:/,红包奖励形式:/,(红包)奖金大小(范围0-值):/,输入内线语句:/,";
            string strName = "type,IdList,count,suretype,awardtype,award,ubbtext,backurl";
            string strType = "select,big,text,select,select,text,big,hidden,";
            string strValu = 0 + "'" + xml.dss["IdList"] + "'300'0'0'500'" + xml.dss["QuestionsUbbText"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|在线会员|1|隐身会员|2|所有会员|3|指定会员,true,true,0|全答才有奖|1|全答对才有奖|2|答了就有奖,0|固定奖金|1|随机红包,true,false,true,";
            string strIdea = "/";
            string strOthe = "确定,questions.aspx?act=select&amp;info=ok&amp;qId=" + qId + "&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("", "<br/>"));

        Bottom();
    }

    //answer 答题记录
    protected void AnswerPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;答题记录");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[1-2]\d*$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "1"));
        int getID = Utils.ParseInt(Utils.GetRequest("getID", "all", 1, @"^[0-9]\d*$", "0"));//正误查找 
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        string strWhere = "";
        string str;
        string outputtext;
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 2)
        {
            builder.Append("按每轮 | ");
            str = "ID>0 and isTrue=1 group by usid order by count desc ";
            outputtext = "答对";
        }
        else
        {

            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=answer&amp;ptype=2&amp;") + "\">按每轮</a> | ");
        }
        if (ptype == 1)
        {
            builder.Append("按答题<br/>");
            str = "ID>0 group by usid order by count desc ";
            outputtext = "总答 ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=answer&amp;ptype=1&amp;") + "\">按答题</a> <br/>");

        }
        builder.Append(Out.Tab("</div>", ""));
        //if (ptype == 3)
        //{
        //    builder.Append("按个人 <br/> ");
        //    str = "ID>0 and isTrue=1 group by usid order by count desc ";
        //    outputtext = "个人";
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=answer&amp;ptype=3&amp;") + "\">按个人</a><br/>");
        //}
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "backurl", "uid", "start", "down", "ptype", "getID" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        #region 答题
        if (ptype == 1)
        {
            if (start.Length > 1 || uid > 0)
            {
                if (uid > 0)
                {
                    strWhere = "usid=" + uid + "and addtime> '" + Convert.ToDateTime(start) + "' and addtime< '" + Convert.ToDateTime(down) + "'";
                    if (getID >= 0 && getID != 2)
                    {
                        strWhere += " and isTrue=" + getID;
                    }
                }
                else
                {
                    strWhere = "addtime> '" + Convert.ToDateTime(start) + "' and addtime< '" + Convert.ToDateTime(down) + "'";
                    if (getID >= 0 && getID != 2)
                    {
                        strWhere += " and isTrue=" + getID;
                    }
                }
            }
            else
            {
                start = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss");
                down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }



            // 开始读取列表
            IList<BCW.Model.tb_QuestionAnswer> listPayrmb = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswers(pageIndex, pageSize, strWhere, out recordCount);
            if (listPayrmb.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.tb_QuestionAnswer n in listPayrmb)
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
                    builder.Append(k + "." + "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.usname + "</a>" + "答了" + "<a href =\"" + Utils.getUrl("questions.aspx?act=" + "manage" + "&amp;ID=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(n.questtion)) + "</a>.");
                    if (n.isTrue == 1)
                    { builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=" + "checkanswer" + "&amp;ID=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><font color =\"green\">" + "回答正确" + "</font></a>"); }
                    else
                    { builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=" + "checkanswer" + "&amp;ID=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><font color =\"red\">" + "回答错误" + "</font></a>"); }
                    builder.Append("[" + Convert.ToDateTime(n.addtime).ToString("MM-dd HH:mm") + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("没有相关记录.");
                builder.Append(Out.Tab("</div>", ""));
            }
            string strText1 = "用户记录:,答题正误:,开始时间:,结束时间:,,";
            string strName1 = "uid,getID,start,down,ptype,backurl";
            string strType1 = "num,select,text,text,hidden,hidden";
            string strValu1 = uid + "'" + getID + "'" + start + "'" + down + "'" + ptype + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,2|全部回答|1|正确错误|0|回答错误,true,true,true,false";
            string strIdea1 = "/";
            string strOthe1 = "确定搜索,questions.aspx?act=answer&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        #endregion

        #region 按每轮数
        if (ptype == 2)
        {
            string arrId = "";
            strWhere = " ID>0 ";
            if (uid > 0)
                strWhere += " and uid =" + uid;
            try
            {
                if (start.Length > 1)
                {
                    strWhere += " and addtime> '" + Convert.ToDateTime(start) + "' and addtime< '" + Convert.ToDateTime(down) + "'";
                }
                else
                {
                    start = DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd HH:mm:ss");
                    down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch { Utils.Error("输入时间有误！", ""); }
            //控制器列表
            //开始读取列表
            IList<BCW.Model.tb_QuestionsAnswerCtr> listSSCpay = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtrs(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.tb_QuestionsAnswerCtr n in listSSCpay)
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
                    if (n.ishit == 1)
                    {
                        arrId = arrId + " " + n.ID;
                    }
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.uid));
                    builder.AppendFormat("{0}前{2}回答{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.addtime)), n.uid, "<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>", "<a href=\"" + Utils.getUrl("questions.aspx?act=ansctr&amp;ID=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><font color=\"green\"> 第" + Out.SysUBB(n.contrID.ToString()) + "轮</font></a>问题!");
                    if (n.now != n.count)
                    {
                        if (n.ishit == 5||n.ishit==4 || n.ishit == 3||n.ishit==2)
                        { builder.Append("(<font color=\"green\">" + IsHit(Convert.ToInt32(n.ishit)) + "</font>)"); }
                        else
                        {
                            builder.Append("(" + n.now + "|" + n.trueCount + "|" + n.count + ")");
                        }
                        //builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=next&amp;ID=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[继续作答]</a>");
                    }
                    else
                    {
                        if (n.ishit == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paijiang&amp;ID=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[兑奖]</a>");
                        }
                        else
                        {
                            if (n.ishit == 0)
                            {
                                builder.Append("(<font color=\"green\">" + n.awardgold + ub.Get("SiteBz") + "</font>)");
                            }
                            builder.Append(" (<font color=\"green\">" + IsHit(Convert.ToInt32(n.ishit)) + "</font>)");
                            
                        }
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

                //if (!string.IsNullOrEmpty(arrId))
                //{
                //    builder.Append(Out.Tab("", "<br />"));
                //    arrId = arrId.Trim();
                //    arrId = arrId.Replace(" ", ",");
                //    string strName = "arrId,act";
                //    string strValu = "" + arrId + "'casepost";
                //    string strOthe = "本页兑奖,questions.aspx?act=caseok&amp;,post,0,red";
                //    builder.Append(Out.wapform(strName, strValu, strOthe));
                //}

                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("温馨提示:未答完数字表示（已答数|正确数|总问题数）.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无相关记录.");
                builder.Append(Out.Tab("</div>", ""));
            }
            string strText1 = "用户记录:,开始时间:,结束时间:,,";
            string strName1 = "uid,start,down,ptype,backurl";
            string strType1 = "text,text,text,hidden,hidden,";
            string strValu1 = uid + "'" + start + "'" + down + "'" + ptype + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,true,true,true,false";
            string strIdea1 = "/";
            string strOthe1 = "按时间搜索,questions.aspx?act=answer&amp;ptype=" + ptype + "&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));


        }

        #endregion

        #region 按个人 注释
        //else
        //{
        //    try
        //    {
        //        //int usid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^", "0"));
        //        //if (usid > 0)
        //        //    meid = usid;
        //        builder.Append(Out.Tab("<div>", ""));
        //        int meid = 727;
        //        int allcount = new BCW.BLL.tb_QuestionAnswer().GetAllCounts(meid);
        //        int truecount = new BCW.BLL.tb_QuestionAnswer().GetTrueCounts(meid);
        //        double percent = Convert.ToDouble(truecount) / Convert.ToDouble(allcount);
        //        //获取称谓
        //        string QuestionsChengWei = ub.GetSub("QuestionsChengWei", xmlPath);
        //        string[] QuestionsChengWei2 = QuestionsChengWei.Split('#');
        //        string chengwei = "";
        //        int dengji = 0;
        //        //truecount = 1;
        //        for (int i = 0; i < QuestionsChengWei2.Length; i++)
        //        {
        //            if (i > 0)
        //            {
        //                if (truecount / (3000 * i * i) > 0)
        //                {
        //                    chengwei = QuestionsChengWei2[i].ToString();
        //                    dengji = i;
        //                }
        //            }
        //            else
        //            {
        //                chengwei = QuestionsChengWei2[0].ToString();
        //                dengji = 0;
        //            }
        //        }
        //        string strw = " group by usid order by count desc ";
        //        DataSet ds = new BCW.BLL.tb_QuestionAnswer().GetList(" top 6 usid,count(usid)as count", strw);
        //        string mingci = " 0 ";
        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                if (i > 500)
        //                {
        //                    mingci = "大隐隐于市,是华丽的低调(500名之后..)";
        //                }
        //                else
        //                {
        //                    //if (ds.Tables[0].Rows[0]["usid"].ToString() == meid.ToString())
        //                    //{
        //                    //    mingci = "第" + (i + 1) + "名";
        //                    //}
        //                    //else
        //                    //{
        //                    //    mingci = "大隐隐于市,是华丽的低调(暂找不到你的排名)";
        //                    //}
        //                }
        //            }
        //        }
        //        else
        //        {
        //            mingci = "大隐隐于市,是华丽的低调(暂找不到你的排名)";
        //        }
        //        //战果
        //        string zhanguo = "";
        //        // if (percent > 0.1)
        //        {
        //            zhanguo = "你已击败全国<b>" + percent * 100 + "%</b>的玩家了!";
        //        }

        //        builder.Append("<font color=\"red\">" + "昵称: </font>" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">" + new BCW.BLL.User().GetUsName(meid) + " </a><br/>");
        //        builder.Append("<font color=\"red\">当前称谓:</font> " + "<font color=\"green\">" + chengwei + "</font> (" + getDengJiChinese(dengji) + "级称谓)");
        //        builder.Append("<br/>");
        //        builder.Append("<font color=\"red\">我的排名:</font> " + "<font color=\"green\">" + mingci + " </font>");
        //        builder.Append("<br/>");
        //        builder.Append("总答题数: <font color=\"green\">" + allcount + " </font>题");
        //        builder.Append("<br/>");
        //        builder.Append("答对题数: <font color=\"green\">" + truecount + " </font>题");
        //        builder.Append("<br/>");
        //        if ((percent).ToString("p").Contains("%"))
        //        {
        //            builder.Append("正确率: <font color=\"green\">" + (percent).ToString("p") + "</font>");
        //            builder.Append("<br/>");
        //        }
        //        builder.Append("获得总奖励: " + "<font color=\"green\">" + new BCW.BLL.tb_QuestionsAnswerCtr().GetAllAwardGold(meid) + "</font>" + " " + ub.Get("SiteBz"));
        //        builder.Append("<br/>");
        //        builder.Append("战果: " + zhanguo);
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //    catch
        //    {
        //        builder.Append(Out.Tab("<div>", ""));
        //        builder.Append("暂无更多记录！");
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //    #region 查找功能 已屏蔽
        //    //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //    //builder.Append("=输入ID查找=");
        //    //builder.Append(Out.Tab("</div>", ""));

        //    //string strText = ",,,";
        //    //string strName = "uid,ptype,act";
        //    //string strType = "num,hidden,hidden";
        //    //string strValu = usid+"'3'mylist";
        //    //string strEmpt = "false,false";
        //    //string strIdea = "";
        //    //string strOthe = "查清单,questions.aspx,get,1,red";
        //    //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //    #endregion
        //}
        #endregion

        Bottom();


    }

    //ansctr 每一轮的id列输出
    protected void AnswerCtrPage()
    {
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[^\^]{0,200}$", "1"));
        BCW.Model.tb_QuestionsAnswerCtr model = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(ID);
        Master.Title = GameName;
        //int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        //if (meid != model.uid)
        //{ Utils.Error("不存在的答题结果！", ""); }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>" + "&gt;" + "第" + model.contrID + "轮答题结果");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string[] qlist = model.List.Split('#');
        string[] qexplain = model.explain.Split('#');
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("[本轮答题] " + "第" + "<a href =\"" + Utils.getUrl("questions.aspx?act=editacontr&amp;ID=" + model.ID + "&amp;") + "\">" + model.contrID + "</a>" + "轮"); 
        double percent = Convert.ToDouble(model.trueCount) / Convert.ToDouble((model.count));
        //if (model.isDone == 0)
        //{ builder.Append("(已答完)"); }
        //else if (model.isDone == 1)
        //{ builder.Append("(<a href=\"" + Utils.getUrl("questions.aspx?act=next&amp;cid=" + 0 + "&amp;") + "\">" + "未答完" + "</a>)"); }
        //else
        //{ builder.Append("(已过期)"); }
        builder.Append("<br/>");
        builder.Append("[答题时间] " + Convert.ToDateTime(model.addtime).ToString("yyyy-MM-dd HH:mm"));
        builder.Append("<br/>");
        builder.Append("[截至时间] " + Convert.ToDateTime(model.overtime).ToString("yyyy-MM-dd HH:mm"));
        builder.Append("<br/>");
        TimeSpan ts = Convert.ToDateTime(model.overtime) - Convert.ToDateTime(model.addtime);
        builder.Append("[答题耗时] " + ts.Minutes + "分钟");
        builder.Append("<br/>");
        builder.Append("[题目数量] " + model.count);
        //builder.Append("(正" + "<font color=\"green\">" + model.trueCount + "</font>");
        //builder.Append(" 误" + "<font color=\"red\">" + model.flaseCount + "</font>" + ")");
        //builder.Append("(正确率" + (percent * 100).ToString() + "%)");
        builder.Append("<br/>");
        builder.Append("[题目记录] ");
        for (int i = 0; i < qlist.Length; i++)
        {
            if (i % 6 == 0 && i > 0)
            { builder.Append("<br/>"); }
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=manage&amp;ID=" + qlist[i] + "&amp;") + "\">" + qlist[i] + "</a>" + "  ");
        }
        builder.Append("<br/>");
        builder.Append("[回答记录] ");
        for (int i = 0; i < qexplain.Length; i++)
        {
            if (i % 6 == 0 && i > 0)
            { builder.Append("<br/>"); }
            builder.Append("<font color=\"green\">" + qexplain[i] + "</font>" + "  ");
        }
        builder.Append("<br/>");
        builder.Append("[回答ID] ");
        string usname = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.uid));
        builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + model.uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + usname + "</a>");
        //for (int i = 0; i < qexplain.Length; i++)
        //{
        //    //builder.Append(""+qexplain[i]);
        //    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=askresult&amp;ID=" + qexplain[i] + "&amp;") + "\">" + qexplain[i] + "</a>" + "  ");
        //    if (i == 4)
        //    { builder.Append("<br/>"); }
        //}
        builder.Append("<br/>");
        string textgold = ub.Get("SiteBz");
        if (QuestionsStatue == "2")
        {
            textgold = " 问币";
        }
        if (model.awardtype == 0)//0 固定奖金 1红包形式
        {
            // sendGold = Convert.ToInt32(ctr.award);
            if (model.awardgold > 0)
                builder.Append("[固定奖金] " + model.awardgold + textgold);
            else
                builder.Append("[奖金状态] " + "未能获取");
        }
        else if (model.awardtype == 1)
        {
            if (model.awardgold > 0)
                builder.Append("[随机红包] " + model.awardgold + textgold + " ");
            else
                builder.Append("[奖金状态] " + "未能获取");
        }
        if (model.now == model.count)
        {
            BCW.Model.tb_QuestionsAnswerCtr model1 = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(ID);
            BCW.Model.tb_QuestionController ctr = new BCW.BLL.tb_QuestionController().Gettb_QuestionController(Convert.ToInt32((model1.contrID)));
            string[] index = model1.List.Split('#');
            string[] index1 = model1.explain.Split('#');
            string q_type = "";
            try
            {
                q_type = new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(ctr.type));
                if (q_type == "")
                { q_type = "随机题目"; }
            }
            catch
            { q_type = "随机题目"; }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("题目类型:" + "<font color=\"red\">" + q_type + "</font><br/>");
            //  builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font><br/>");
            builder.Append("本轮记录: " + model1.ID + " 题数:" + "<font color=\"red\">" + model1.count + "</font>" + "<br/>");
            builder.Append("本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>");
            builder.Append("<br/>");
            builder.Append("正确数:<font color=\"red\">" + model1.trueCount + " </font>");
            builder.Append("错误数:<font color=\"red\">" + model1.flaseCount + "</font>");
            percent = Convert.ToDouble(model1.trueCount) / Convert.ToDouble(model1.count);
            int sendGold = 0;
            if (model1.flaseCount == 0)
            {
                builder.Append("正确率:<font color=\"red\">" + "100%</font>");
            }
            else
                builder.Append(" 正确率:<font color=\"red\">" + (percent * 100).ToString() + "%</font>");
            #region 自动派取奖励

            if (ctr.awardtype == 0)//0全答才有奖励（无论对不对） 1全对 2答了
            {
                builder.Append("<br/>获得奖金: " + "<font color=\"green\">" + ctr.award + " </font>" + textgold);
            }
            else
            if (ctr.awardtype == 1)// 1全对 2答了
            {
                if (model1.flaseCount == 0)//全对 派奖
                {
                    builder.Append("恭喜你获得本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>" + textgold);
                }
                else//答错一题拿奖失败
                {
                    builder.Append("<font color=\"green\">" + "Oh~~,你答错题了,大奖与你擦边而过啦(到嘴的肉飞走了)..." + "</font>");
                }
            }
            else
                if (ctr.awardtype == 2)//答了
            {
                builder.Append("恭喜你获得本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>" + textgold);
            }
            if (model1.ishit == 1)//0 已领 1未领 2过期 3答题超时 4回答错误不能领奖
            {
                builder.Append("  <a href=\"" + Utils.getUrl("questions.aspx?act=paijiang&amp;ID=" + model1.ID + "&amp;") + "\">" + "派奖" + "</a>");
            }
            else
               if (model1.ishit == 2)//0 已领 1未领 2过期 3答题超时
            {
                builder.Append(" (已过期)" + "");
            }
            else
              if (model1.ishit == 3)//0 已领 1未领 2过期 3答题超时
            {
                builder.Append(" (答题超时)" + "");
            }
            else if (model1.ishit == 0)
            { builder.Append("  " + "（已领取）" + ""); }
            #endregion

            string OutText = Convert.ToString(ub.GetSub("QuestionsSelectText", "/Controls/questions.xml"));
            string[] getText = OutText.Split('#');
            Random index_n = new Random();
            string Print_text = getText[index_n.Next(0, getText.Length)];
            // Utils.Error(""+Print_text+"","");
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //{
            //    Print_text = "<br/><font color=\"#FF0000\">" + Print_text + "</font>" + "<br/>";
            //}
            // builder.Append("<br/><font color =\"#FF0000\">" + Print_text + "</font>" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Model.tb_QuestionsList n;
            BCW.Model.tb_QuestionAnswer n1;
            builder.Append(Out.Tab("<div>", "<br/>"));
            for (int i = 0; i < model1.count; i++)
            {
                int ID0 = Convert.ToInt32(index[i]);
                int ID1 = Convert.ToInt32(index1[i]);
                n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID0);
                n1 = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID1);
                //  builder.Append(Out.Tab("<div>", ""));
                builder.Append(n1.ID + ".问题: " + "<font color =\"green\">" + n.question + "</font>");
                if (n.img.Length > 5)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (n.style == 1)//4选1
                {
                    builder.Append("(选择4选1)" + "<br/>");
                    builder.Append("A." + n.chooseA + " ");
                    builder.Append(" B." + n.chooseB);
                    builder.Append(" C." + n.chooseC);
                    builder.Append(" D." + n.chooseD + "");
                }
                else if (n.style == 2)//2选1
                {
                    builder.Append("(选择2选1)" + "<br/>");
                    builder.Append("问:" + n.question + "<br/>");
                    builder.Append("A." + n.chooseA + " ");
                    builder.Append(" B." + n.chooseB);
                }
                else if (n.style == 3)//3选1
                {
                    builder.Append("(选择3选1)" + "<br/>");
                    builder.Append("A." + n.chooseA + " ");
                    builder.Append(" B." + n.chooseB);
                    builder.Append(" C." + n.chooseC);
                }
                else if (n.style == 4)//判断
                {
                    builder.Append("(判断题)" + "<br/>");
                    builder.Append("" + "对" + " ");
                    builder.Append(" " + "错");
                }
                else if (n.style == 5)//填空
                {
                    builder.Append("(填空题)" + "");
                }

                builder.Append("<br/>Ta的答案:" + n1.answer);
                if (n1.isTrue == 1)
                {
                    builder.Append(" " + "(" + "<font color=\"green\">" + "正确" + "</font>" + ")");
                }
                else
                { builder.Append(" " + "(" + "<font color=\"red\">" + "错误" + "</font>" + ")"); }
                builder.Append("<br/>" + "正确答案:" + n.answer + "<br/>");
                //  builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        // builder.Append(" <br/>[前往查看] <a href=\"" + Utils.getUrl("questions.aspx?act=sviewpt&amp;ID=" + model.ID + "&amp;") + "\">" + "详细记录>>" + "</a>");
        else
        {
            builder.Append(" <br/>[答题状态]  当前第" + (model.now + 1) + "题");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("");

        // builder.Append("<br/>");
        Bottom();
    }

    //question 问题管理/增加问题
    protected void QuestionPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[1-2]\d*$", "1"));//1
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]\d*$", "1"));//2
        int choose = int.Parse(Utils.GetRequest("choose", "all", 1, @"^[0-5]\d*$", "1"));//3;
        int style = int.Parse(Utils.GetRequest("style", "all", 1, @"^[0-5]\d*$", "1")); ;//题目类型 123 选择 4 判断 5填空 
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        string strWhere = " ";
        if (game == 1)
        {
            builder.Append("全部问题");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=1&amp;") + "\">全部问题</a>");
        //if (game == 2)
        //{ builder.Append(" | 添加问题"); }
        //else
        //    builder.Append(" | <a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;") + "\">添加问题</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        if (game == 1)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (style == 1 || style == 2 || style == 3)
            {
                builder.Append("选择题 | ");
                strWhere = " (style=1 or style=2 or style=3) ";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=1&amp;style=1&amp;") + "\">选择题</a> | ");
            if (style == 4)
            {
                builder.Append("判断题 | ");
                strWhere = " (style=4) ";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=1&amp;style=4&amp;") + "\">判断题</a> | ");
            if (style == 5)
            {
                builder.Append("填空题");
                strWhere = " (style=5) ";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=1&amp;style=5&amp;") + "\">填空题</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        #region 全部问题
        //全部问题
        if (game == 1)
        {
            // builder.Append("全部问题");
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "uid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.tb_QuestionsList> listSSCpay = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsLists(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.tb_QuestionsList n in listSSCpay)
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
                    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=manage&amp;game=2&amp;ID=" + n.ID + "") + "\">管理</a>&gt;");
                    builder.Append(n.ID + ".问:<font color=\"green\">" + n.question + "</font><br/>");
                    if (n.style == 1)//4选1
                    {
                        builder.Append("A: " + n.chooseA);
                        builder.Append(" B: " + n.chooseB);
                        builder.Append(" C: " + n.chooseC);
                        builder.Append(" D: " + n.chooseD);
                    }
                    else
                        if (n.style == 2)//2选1
                    {
                        builder.Append("A: " + n.chooseA);
                        builder.Append(" B: " + n.chooseB);
                    }
                    else if (n.style == 3)//3选1
                    {
                        builder.Append("A: " + n.chooseA);
                        builder.Append(" B: " + n.chooseB);
                        builder.Append(" C: " + n.chooseC);
                    }
                    else
                        if (n.style == 4)//判断
                    {
                        builder.Append("判断题");
                    }
                    else
                        if (n.style == 5)//填空
                    {
                        builder.Append("填空题");
                    }

                    builder.Append("<br/><font color=\"red\">正确答案: " + n.answer + "</font>");
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
            string strText = "输入问题ID:/,";
            string strName = "ID,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜问题记录,questions.aspx?act=manage&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br/>"));

        }
        #endregion

        #region  添加问题
        //添加问题
        else if (game == 2)
        {
            string strText = "问题:/,游戏Logo(可留空):/,游戏公告:/,游戏规则:/,总奖池:/,当前剩余总奖池:/,每人每天最大获奖次数:/,";
            string strName = "QuestionsGameName,QuestionsImg,QuestionsTop,QuestionsRule,AllAwardCount,AllAwardCountNow,maxGet,backurl";
            string strType = "text,text,textarea,big,text,text,text,hidden";
            string strValu = "'" + "'''''";
            string strEmpt = "false,true,true,true,true,true,true,";
            string strIdea = "/";
            string strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            DataSet ds = new BCW.BLL.tb_QuestionsType().GetList(" * ", " ID>0 ");
            string question_type = "";//问题类型（语文数学脑筋急转弯等）
            string question_deficult = "0|新手|1|初级|2|中级|3|高级|4|困难|5|大师";//难度
            string chooses = "A| A |B| B |C| C |D| D ";//答案
            string awardType = "1| 酷币 |2| 点值 |3| 农场种子 |4| 无奖励 ";//答案
            string url = "questions.aspx?act=question&amp;game=2&amp;";
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //0|游戏区奖池1|1|游戏区奖池2|2|游戏区奖池3|3|论坛奖池4|4|奖池5
                    question_type += ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Name"] + "|";
                }
            }
            question_type = question_type.Substring(0, question_type.Length - 1);
            // question_type = "";
            if (question_type == "")
            { question_type = " 0|请先前往添加问题分类|1|请先前往添加问题分类"; }
            //  Utils.Error(""+question_type+"","");
            if (type == 1)
            {
                builder.Append("选择题 | ");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=1&amp;") + "\">选择题</a> | ");
            }
            if (type == 2)//判断题
            {
                url = "questions.aspx?act=question&amp;game=2&amp;type=2&amp;";
                builder.Append("判断题 | ");
                style = 4;
                strText = "问题:/,正确答案:/,问题类型:/,图片地址(无图为0):/,难度级别:/,答对奖励类型:/,答对赏金[红包](无奖励为0):/,";
                strName = "name,sure,type,img,deficult,awardType,gold,backurl";
                strType = "text,select,select,text,select,select,text,hidden";
                strValu = "'" + "'" + "'" + 0 + "'" + "'" + 1 + "'" + style + "'" + Utils.getPage(0) + "";
                strEmpt = "false," + "正确|正确|错误|错误" + "," + question_type + ",true," + question_deficult + "," + awardType + ",true,true";
                strIdea = "/";
                strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=2&amp;") + "\">判断题</a> | ");
            }
            if (type == 3)//填空题
            {
                style = 5;
                builder.Append("填空题  ");
                url = "questions.aspx?act=question&amp;game=2&amp;type=3&amp;";
                strText = "问题:/,正确答案:/,问题类型:/,图片地址(无图为0):/,难度级别:/,答对奖励类型:/,答对赏金[红包](无奖励为0):/,";
                strName = "name,sure,type,img,deficult,awardType,gold,backurl";
                strType = "text,text,select,text,select,select,text,hidden";
                strValu = "'" + "'" + "'" + 0 + "'" + "'" + 1 + "'" + style + "'" + Utils.getPage(0) + "";
                strEmpt = "false," + "true" + "," + question_type + ",true," + question_deficult + "," + awardType + ",true,true";
                strIdea = "/";
                strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=3&amp;") + "\">填空题</a>");
            }
            if (type == 1)
            {
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            else
                builder.Append(Out.Tab("</div>", ""));
            if (type == 1)
            {
                url = "questions.aspx?act=question&amp;game=2&amp;type=1&amp;backurl=" + Utils.getPage(1) + "";
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                if (choose == 1) //选择题(4选1)
                {
                    url = "questions.aspx?act=question&amp;game=2&amp;type=1&amp;choose=1&amp;backurl=" + Utils.getPage(1) + "";
                    builder.Append("选择题(4选1) | ");
                    style = 1;
                    strText = "问题:/,答案A:/,答案B:/,答案C:/,答案D:/,正确答案:/,问题类型:/,图片地址(无图为0):/,难度级别:/,答对奖励类型:/,答对赏金[红包](无奖励为0):/,";
                    strName = "name,a,b,c,d,sure,type,img,deficult,awardType,gold,backurl";
                    strType = "text,text,text,text,text,select,select,text,select,select,text,hidden";
                    strValu = "'" + "'" + "'" + "'" + "'" + "'" + "'" + 0 + "'" + "'" + 1 + "'" + "'" + Utils.getPage(0) + "";
                    strEmpt = "false,true,true,true,true," + chooses + "," + question_type + ",true," + question_deficult + "," + awardType + ",true";
                    strIdea = "/";
                    strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=1&amp;choose=1&amp;") + "\">选择题(4选1)</a> | ");
                }
                if (choose == 2)//选择题(3选1)
                {
                    url = "questions.aspx?act=question&amp;game=2&amp;type=1&amp;choose=2&amp;backurl=" + Utils.getPage(1) + "";
                    style = 3;
                    chooses = "A| A |B| B |C| C ";
                    builder.Append("选择题(3选1) | ");
                    strText = "问题:/,答案A:/,答案B:/,答案C:/,正确答案:/,问题类型:/,图片地址(无图为0):/,难度级别:/,答对奖励类型:/,答对赏金[红包](无奖励为0):/,";
                    strName = "name,a,b,c,sure,type,img,deficult,awardType,gold,backurl";
                    strType = "text,text,text,text,select,select,text,select,select,text,hidden";
                    strValu = "'" + "'" + "'" + "'" + "'" + "'" + 0 + "'" + "'" + 1 + "'" + 1 + "'" + Utils.getPage(0) + "";
                    strEmpt = "true,true,true,true," + chooses + "," + question_type + ",true," + question_deficult + "," + awardType + ",true";
                    strIdea = "/";
                    strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=1&amp;choose=2&amp;") + "\">选择题(3选1)</a> | ");
                }
                if (choose == 3)//选择题(2选1)
                {
                    url = "questions.aspx?act=question&amp;game=2&amp;type=1&amp;choose=3&amp;backurl=" + Utils.getPage(1) + "";
                    style = 2;
                    chooses = "A| A |B| B  ";
                    builder.Append("选择题(2选1)  ");
                    strText = "问题:/,答案A:/,答案B:/,正确答案:/,问题类型:/,图片地址(无图为0):/,难度级别:/,答对奖励类型:/,答对赏金[红包](无奖励为0):/,";
                    strName = "name,a,b,sure,type,img,deficult,awardType,gold,backurl";
                    strType = "text,text,text,select,select,text,select,select,text,hidden";
                    strValu = "'" + "'" + "'" + "'" + "'" + 0 + "'" + "'" + 1 + "'" + style + "'" + "'" + Utils.getPage(0) + "";
                    strEmpt = "false,true,true," + chooses + "," + question_type + ",true," + question_deficult + "," + awardType + ",true";
                    strIdea = "/";
                    strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=1&amp;choose=3&amp;") + "\">选择题(2选1)</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定修改")
            {

                int style1 = int.Parse(Utils.GetRequest("style", "all", 1, @"^[0-5]\d*$", "1")); ;//题目类型 123 选择 4 判断 5填空 ;
                                                                                                  //   Utils.Error("" + style1 + "", "");
                string name = Utils.GetRequest("name", "post", 2, @"^[^\^]{1,500}$", "问题提问限500字内");
                string a = Utils.GetRequest("a", "post", 3, @"^[^\^]{1,20}$", "选项A限10字内");
                string b = Utils.GetRequest("b", "post", 3, @"^[^\^]{1,20}$", "选项B限10字内");
                string c = "0";
                string d = "0";
                if (style1 == 1)//4选1
                {
                    c = Utils.GetRequest("c", "post", 3, @"^[^\^]{1,20}$", "选项C限10字内");
                    d = Utils.GetRequest("d", "post", 2, @"^[^\^]{1,20}$", "选项D限10字内");
                }
                else
                    if (style1 == 3)//3选1
                {
                    c = Utils.GetRequest("c", "post", 3, @"^[^\^]{1,10}$", "选项C限10字内");
                }
                else
                    if (style1 == 4)//判断
                {
                    //  sure = Utils.GetRequest("sure", "post", 3, @"^[^\^]{1,10}$", "判断对错选择错误");

                }
                else
                    if (style1 == 5)//填空
                {
                    // sure = Utils.GetRequest("sure", "post", 3, @"^[^\^]{1,10}$", "选项C限10字内");
                    // 填空
                }
                string sure = Utils.GetRequest("sure", "post", 2, @"^[^\^]{0,20}$", "正确答案限20字内");
                string img = Utils.GetRequest("img", "post", 2, @"^[^\^]{0,100}$", "图片地址长度错误");
                int deficult = int.Parse(Utils.GetRequest("deficult", "post", 2, @"^[^\^]{1,20}$", "难度填写错误"));
                string awardType1 = Utils.GetRequest("awardType", "post", 2, @"^[^\^]{1,20}$", "奖品类型填写错误");
                string gold = Utils.GetRequest("gold", "post", 2, @"^[^\^]{1,20}$", "奖品金额填写错误");
                string type1 = Utils.GetRequest("type", "post", 2, @"^[^\^]{1,20}$", "奖品金额填写错误");
                BCW.Model.tb_QuestionsList model = new BCW.Model.tb_QuestionsList();
                model.addtime = DateTime.Now;
                model.answer = sure;
                model.awardGold = Convert.ToInt32(gold);
                model.awardId = Convert.ToInt32(awardType1);
                model.awardType = "";
                model.chooseA = a;
                model.chooseB = b;
                model.chooseC = c;
                model.chooseD = d;
                model.deficult = deficult;
                model.falseAnswer = 0;
                model.hit = 0;
                model.img = img;
                model.indent = "0";
                model.question = name;
                model.remarks = "0";
                model.statue = 1;
                model.style = style1;//题目类型 123 选择 4 判断 5填空 ;
                model.styleID = "0";
                model.title = "0";
                model.trueAnswer = 0;
                model.type = type1;
                //  Utils.Error(""+ choose + "","");
                Utils.Error("" + 2 + "", "");
                int id = new BCW.BLL.tb_QuestionsList().Add(model);
                Utils.Success("增加问题", "增加问题成功，正在返回..", Utils.getUrl("questions.aspx?act=question&amp;game=2&amp;type=" + type + "&amp;choose=" + choose + "&amp;backurl=" + Utils.getPage(1) + ""), "2");
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getPage("questions.aspx") + "\">返回上级</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
                builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                //  Utils.Error(""+ choose + "","");
                //strText += ",";
                //strName += ",choose";
                //strType += ",hidden";
                //strValu += "'" +choose+ "";
                //strEmpt += ",true";
                url = "questions.aspx?act=question&amp;game=" + game + "&amp;type=" + type + "&amp;choose=" + choose + "&amp;style=" + style + "&amp;backurl=" + Utils.getPage(1) + "";
                strOthe = "确定修改|reset," + url + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:" + "" + "<br />");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getPage("questions.aspx") + "\">返回上级</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
                builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
                builder.Append(Out.Tab("</div>", "<br />"));
            }


            #endregion
        }



        Bottom();
    }

    //manage 一个问题管理
    protected void AQuestionPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("问题管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "1"));//1
        string action = (Utils.GetRequest("action", "all", 1, @"", "1"));
        string run = (Utils.GetRequest("run", "all", 1, @"", "1"));
        if (!new BCW.BLL.tb_QuestionsList().Exists(ID))
        {
            Utils.Error("不存在的问题记录！", "");
        }
        else
        {
            if (action == "update")
            {
                if (run == "ok")//更新
                {

                }
                else
                {

                }

            }
            else
            {
                BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("问题:" + "<font color=\"red\">" + n.question + "</font>");
                if (n.img.Length > 5)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (n.style == 1)//4选1
                {
                    builder.Append("(选择4选1)" + "<br/>");
                    builder.Append("A: " + n.chooseA);
                    builder.Append(" B: " + n.chooseB);
                    builder.Append(" C: " + n.chooseC);
                    builder.Append(" D: " + n.chooseD);
                    builder.Append("<br/>正确答案: " + "<font color=\"red\">" + n.answer + "</font>");
                }
                else if (n.style == 2)//2选1
                {
                    builder.Append("(选择2选1)" + "<br/>");
                    builder.Append("问:" + n.question + "<br/>");
                    builder.Append("A: " + n.chooseA);
                    builder.Append(" B: " + n.chooseB);
                    builder.Append("<br/>正确答案: " + "<font color=\"red\">" + n.answer + "</font>");
                }
                else if (n.style == 3)//3选1
                {
                    builder.Append("(选择3选1)" + "<br/>");
                    builder.Append("A: " + n.chooseA);
                    builder.Append(" B: " + n.chooseB);
                    builder.Append(" C: " + n.chooseC);
                    builder.Append("<br/>正确答案: " + "<font color=\"red\">" + n.answer + "</font>");
                }
                else if (n.style == 4)//判断
                {
                    builder.Append("(判断题)" + "<br/>");
                    if (n.answer.Contains("正确"))
                    {
                        builder.Append("正确答案: " + "<font color=\"red\">" + "√" + "</font>");
                    }
                    else
                    {
                        builder.Append("正确答案: " + "<font color=\"red\">" + "×" + "</font>");
                    }
                }
                else if (n.style == 5)//填空
                {
                    builder.Append("(填空题)" + "<br/>");
                    builder.Append("正确答案: " + "<font color=\"red\">" + n.answer + "</font>");
                }
                // Utils.Error(""+n.type+"",""); new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32
                builder.Append("<br/>题目类型: " + "<font color=\"red\">" + (n.type) + "</font>");
                builder.Append("<br/>问题ID: " + n.ID + "");
                builder.Append("<br/>难度: " + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font>");
                builder.Append("<br/>答对: " + "<font color=\"red\">" + n.trueAnswer + "</font>");
                builder.Append(" 答错: " + "<font color=\"red\">" + n.trueAnswer + "</font>");
                builder.Append("<br/>问题解析: " + "<font color=\"red\">" + n.remarks + "</font>");
                //builder.Append(Out.Tab("</div>", "<br/>"));
                //builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<br/>〓管理〓<br/>");
                //  builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=manage&amp;action=update&amp;ID=" + ID + "") + "\">编辑</a>  ");
                builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=edit&amp;ID=" + ID + "&amp;") + "\">编辑</a>  ");
                builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=del&amp;ID=" + ID + "&amp;") + "\">删除</a>  ");
                //if (n.statue == 1) { builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=edit&amp;ID=" + ID + "") + "\">停用</a><br />"); }
                //else { builder.Append("<a href=\"" + Utils.getPage("questions.aspx") + "\">启用</a><br />"); }
                builder.Append(Out.Tab("</div>", ""));
                string strText = "输入问题ID:/,";
                string strName = "ID,backurl";
                string strType = "num,hidden";
                string strValu = ID + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,false";
                string strIdea = "/";
                string strOthe = "搜问题记录,questions.aspx?act=manage&amp;,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        Bottom();
    }

    //editacontr 编辑问题
    protected void EditAContrPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("问题编辑");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, @"", "1");
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "1"));//1
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[1-2]\d*$", "1"));//1
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]\d*$", "1"));//2
        int choose = int.Parse(Utils.GetRequest("choose", "all", 1, @"^[0-5]\d*$", "1"));//3;
        int style = int.Parse(Utils.GetRequest("style", "all", 1, @"^[0-5]\d*$", "1")); ;//题目类型 123 选择 4 判断 5填空 
        DataSet ds = new BCW.BLL.tb_QuestionsType().GetList(" * ", " ID>0 ");
        string question_type = "";//问题类型（语文数学脑筋急转弯等）
        string question_deficult = "0|新手|1|初级|2|中级|3|高级|4|困难|5|大师";//难度
        string chooses = "A| A |B| B |C| C |D| D ";//答案
        string awardType = "1| 酷币 |2| 点值 |3| 农场种子 |4| 无奖励 ";//答案
        string url = "questions.aspx?act=question&amp;game=2&amp;";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                question_type += ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Name"] + "|";
            }
        }
        question_type = question_type.Substring(0, question_type.Length - 1);
        if (question_type == "")
        { question_type = " 0|请先前往添加问题分类|1|请先前往添加问题分类"; }
        BCW.Model.tb_QuestionsAnswerCtr n = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(ID);
        if (ac == "确定修改")
        {
            string uid = Utils.GetRequest("uid", "all", 1, @"", "1");
            string contrID = Utils.GetRequest("contrID", "all", 1, @"", "1");
            string List = Utils.GetRequest("List", "all", 1, @"", "1");
            string answerList = Utils.GetRequest("answerList", "all", 1, @"", "1");
            string explain = Utils.GetRequest("explain", "all", 1, @"", "1");
            string count = Utils.GetRequest("count", "all", 1, @"", "1");
            string awardgold = Utils.GetRequest("awardgold", "all", 1, @"", "1");
            string now = Utils.GetRequest("now", "all", 1, @"", "1");
            string awardtype = Utils.GetRequest("awardtype", "all", 1, @"", "1");
            string trueCount = Utils.GetRequest("trueCount", "all", 1, @"", "1");
            string flaseCount = Utils.GetRequest("flaseCount", "all", 1, @"", "1");
            string ishit = Utils.GetRequest("ishit", "all", 1, @"", "1");
            string addtime = Utils.GetRequest("addtime", "all", 1, @"", "1");
            string overtime = Utils.GetRequest("overtime", "all", 1, @"", "1");
            //string flaseAnswer = Utils.GetRequest("flaseAnswer", "all", 1, @"", "1");
            //string statue = Utils.GetRequest("statue", "all", 1, @"", "1");
            n.trueCount = Convert.ToInt32(trueCount);
            n.answerList = answerList;
            n.awardtype = Convert.ToInt32(awardtype);
            n.contrID = Convert.ToInt32(contrID);
            n.awardgold = Convert.ToInt32(awardgold);
            n.count = Convert.ToInt32(count);
            n.explain = explain;
            n.List = List;
            n.overtime = Convert.ToDateTime(overtime);
            n.uid = Convert.ToInt32(uid);
            n.flaseCount = Convert.ToInt32(flaseCount);
            new BCW.BLL.tb_QuestionsAnswerCtr().Update(n);
            Utils.Success("修改成功,3s后返回", "修改成功,3s后返回...", "questions.aspx?act=editacontr&amp;ID=" + ID + "&amp;backurl=" + Utils.getPage(0) + "&amp;", "3");
        }
        else
        {
            strText = "当前记录:,会员记录:,答题轮次:,已回答列:,当前已答:,回答记录:,总问题数:,当前答数:,奖金类型:,获奖金额:,答对题数:,答错题数:,当前状态:,回答时间:,结束时间:,";
            strName = "ID,uid,contrID,List,answerList,explain,count,now,awardtype,awardgold,trueCount,flaseCount,ishit,addtime,overtime,backurl";
            strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,select,text,hidden";
            strValu = n.ID + "'" + n.uid + "'" + n.contrID + "'" + n.List + "'" + n.answerList + "'" + n.explain + "'" + n.count + "'" + n.now + "'" + n.awardtype + "'" + n.awardgold + "'" + n.trueCount + "'" + n.flaseCount + "'" + n.ishit + "'" + n.addtime + "'" + n.overtime + "'" + Utils.getPage(0) + "";
            strEmpt = "true,false,true,true,true,true," + "true" + "," + "true" + ",true,true,true," + "true" + "," + "true" + "" + ",0|已领取|1|未领|2|已过期|3|答题超时|4|回答错误|5|回答已取消,true,true";
            strIdea = "/";
            strOthe = "确定修改|reset,questions.aspx?act=editacontr,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append("<br/>");
    }

    //edit 编辑问题
    protected void EditPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("问题编辑");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, @"", "1");
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "1"));//1
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[1-2]\d*$", "1"));//1
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]\d*$", "1"));//2
        int choose = int.Parse(Utils.GetRequest("choose", "all", 1, @"^[0-5]\d*$", "1"));//3;
        int style = int.Parse(Utils.GetRequest("style", "all", 1, @"^[0-5]\d*$", "1")); ;//题目类型 123 选择 4 判断 5填空 
        DataSet ds = new BCW.BLL.tb_QuestionsType().GetList(" * ", " ID>0 ");
        string question_type = "";//问题类型（语文数学脑筋急转弯等）
        string question_deficult = "0|新手|1|初级|2|中级|3|高级|4|困难|5|大师";//难度
        string chooses = "A| A |B| B |C| C |D| D ";//答案
        string awardType = "1| 酷币 |2| 点值 |3| 农场种子 |4| 无奖励 ";//答案
        string url = "questions.aspx?act=question&amp;game=2&amp;";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //
                question_type += ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Name"] + "|";
            }
        }
        question_type = question_type.Substring(0, question_type.Length - 1);
        if (question_type == "")
        { question_type = " 0|请先前往添加问题分类|1|请先前往添加问题分类"; }
        BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
        if (ac == "确定修改")
        {
            string name = Utils.GetRequest("name", "all", 1, @"", "1");
            string a = Utils.GetRequest("a", "all", 1, @"", "1");
            string b = Utils.GetRequest("b", "all", 1, @"", "1");
            string c = Utils.GetRequest("c", "all", 1, @"", "1");
            string d = Utils.GetRequest("d", "all", 1, @"", "1");
            string sure = Utils.GetRequest("sure", "all", 1, @"", "1");
            //  string type = Utils.GetRequest("type", "all", 1, @"", "1");
            string img = Utils.GetRequest("img", "all", 1, @"", "1");
            string deficult = Utils.GetRequest("deficult", "all", 1, @"", "1");

            string awardType1 = Utils.GetRequest("awardType", "all", 1, @"", "1");
            string gold = Utils.GetRequest("gold", "all", 1, @"", "1");
            string hit = Utils.GetRequest("hit", "all", 1, @"", "1");
            string trueAnswer = Utils.GetRequest("trueAnswer", "all", 1, @"", "1");
            string flaseAnswer = Utils.GetRequest("flaseAnswer", "all", 1, @"", "1");
            string statue = Utils.GetRequest("statue", "all", 1, @"", "1");
            n.trueAnswer = Convert.ToInt32(trueAnswer);
            n.answer = sure;
            n.awardGold = Convert.ToInt32(gold);
            n.awardType = awardType1;
            n.chooseA = a;
            n.chooseB = b;
            n.chooseC = c;
            n.chooseD = d;
            n.deficult = int.Parse(deficult);
            n.falseAnswer = int.Parse(flaseAnswer);
            n.hit = int.Parse(hit);
            n.img = img;
            n.question = name;

            new BCW.BLL.tb_QuestionsList().Update(n);
            Utils.Success("修改成功,3s后返回", "修改成功,3s后返回...", "questions.aspx?act=edit&amp;ID=" + ID + "&amp;backurl=" + Utils.getPage(0) + "&amp;", "3");

        }
        else
        {
            strText = "问题:/,答案A:/,答案B:/,答案C:/,答案D:/,正确答案:/,问题类型:/,图片地址(无图为0):/,难度级别:/,答对奖励类型:/,答对赏金[红包](无奖励为0):/,当前点击:/,答对人数:/,答错人数:/,问题状态:/,";
            strName = "name,a,b,c,d,sure,type,img,deficult,awardType,gold,hit,trueAnswer,flaseAnswer,statue,backurl";
            strType = "text,text,text,text,text,select,select,text,select,select,text,text,text,text,text,hidden";
            strValu = n.question + "'" + n.chooseA + "'" + n.chooseB + "'" + n.chooseC + "'" + n.chooseD + "'" + n.answer + "'" + n.type + "'" + n.img + "'" + n.deficult + "'" + n.awardType + "'" + n.awardGold + "'" + n.hit + "'" + n.trueAnswer + "'" + n.falseAnswer + "'" + n.statue + "'" + Utils.getPage(0) + "";
            strEmpt = "false,true,true,true,true," + chooses + "," + question_type + ",true," + question_deficult + "," + awardType + ",true" + ",true,true,true,true";
            strIdea = "/";
            strOthe = "确定修改|reset,questions.aspx?act=edit,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append("<br/>");
    }

    //set 配置管理
    protected void SetPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[1-2]\d*$", "1"));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/questions.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string QuestionsGameName = Utils.GetRequest("QuestionsGameName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string QuestionsImg = Utils.GetRequest("QuestionsImg", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string QuestionsTop = Utils.GetRequest("QuestionsTop", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string QuestionsRule = Utils.GetRequest("QuestionsRule", "post", 3, @"^[^\^]{1,2000}$", "设置内线提示的文字口号限2000字内");
            string QuestionsPassTime = Utils.GetRequest("QuestionsPassTime", "post", 2, @"^[^\^]{1,20}$", "发布的问题过期时间填写错误");
            string QuestionsAnswerPassTime = Utils.GetRequest("QuestionsAnswerPassTime", "post", 2, @"^[^\^]{1,20}$", "回答超时时间填写错误");
            string QuestionsStatue = Utils.GetRequest("QuestionsStatue", "post", 2, @"^[^\^]{1,20}$", "游戏状态选择错误");
            string QuestionstestID = Utils.GetRequest("QuestionstestID", "post", 2, @"^[^\^]{1,20}$", "内测ID填写错误");
            string QuestionsChengWei = Utils.GetRequest("QuestionsChengWei", "post", 2, @"^[^\^]{1,20000}$", "称谓填写错误！");
            string QuestionsChengWeiDengji = Utils.GetRequest("QuestionsChengWeiDengji", "post", 2, @"^[^\^]{1,200}$", "称谓梯度值填写错误！");
            string QuestionsGundong = Utils.GetRequest("QuestionsGundong", "post", 2, @"^[^\^]{1,200}$", "动态滚动开关选择错误！");
            string QuestionsTopTwo = Utils.GetRequest("QuestionsTopTwo", "post", 2, @"^[^\^]{1,200}$", "动态语句填写错误！");
            xml.dss["QuestionsGameName"] = QuestionsGameName;//游戏名称
            xml.dss["QuestionsImg"] = QuestionsImg;//Logo
            xml.dss["QuestionsTop"] = QuestionsTop; // 公告        
            xml.dss["QuestionsRule"] = QuestionsRule;//规则
            xml.dss["QuestionsPassTime"] = QuestionsPassTime;//过期时间
            xml.dss["QuestionsAnswerPassTime"] = QuestionsAnswerPassTime;//过期时间
            xml.dss["QuestionsStatue"] = QuestionsStatue;//状态
            xml.dss["QuestionstestID"] = QuestionstestID;//测试id
            xml.dss["QuestionsChengWei"] = QuestionsChengWei;//称谓填写错误
            xml.dss["QuestionsChengWeiDengji"] = QuestionsChengWeiDengji;//称谓梯度值填写错误
            xml.dss["QuestionsGundong"] = QuestionsGundong;//动态滚动开关选择错误
            xml.dss["QuestionsTopTwo"] = QuestionsTopTwo;//动态语句填写错误
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("questions.aspx?act=set&amp;backurl=" + Utils.getPage(1) + ""), "2");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=addaward") + "\">生成新奖池</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=addaward") + "\">返回游戏配置</a><br/>");
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("questions.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            string strText = "游戏名称:/,游戏Logo(可留空):/,游戏公告:/,游戏规则:/,发布的问题过期时间(小时):/,回答超时时间(分钟):/,答题称谓(每个等级称谓#分隔):/,称谓梯度值(公式:等级*等级*梯度值):/,游戏状态:/,内测ID(#分隔):/,动态滚动开关:/,动态语句语句:/,";
            string strName = "QuestionsGameName,QuestionsImg,QuestionsTop,QuestionsRule,QuestionsPassTime,QuestionsAnswerPassTime,QuestionsChengWei,QuestionsChengWeiDengji,QuestionsStatue,QuestionstestID,QuestionsGundong,QuestionsTopTwo,backurl";
            string strType = "text,text,big,big,text,text,big,text,select,big,select,text,hidden,";
            string strValu = xml.dss["GameName"] + "'" + xml.dss["img"] + "'" + xml.dss["QuestionsTop"] + "'" + xml.dss["QuestionsRule"] + "'" + xml.dss["QuestionsPassTime"] + "'" + xml.dss["QuestionsAnswerPassTime"] + "'" + xml.dss["QuestionsChengWei"] + "'" + xml.dss["QuestionsChengWeiDengji"] + "'" + xml.dss["QuestionsStatue"] + "'" + xml.dss["QuestionstestID"] + "'" + xml.dss["QuestionsGundong"] + "'" + xml.dss["QuestionsTopTwo"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,true,true,true,true,true,0|维护|1|正常|2|测试,true,0|关闭|1|滚动形式|2|停靠形式,true,true,";
            string strIdea = "/";
            string strOthe = "确定修改|reset,questions.aspx?act=set,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:" + "称谓的计算方式:一级称谓所需答对题数=1*1*梯度值;二级称谓所需答对题数=2*2*梯度值;以此类推" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("questions.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //del 删除问题
    protected void DelPage()
    {
        string ac = (Utils.GetRequest("ac", "all", 1, @"^", "1"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "1"));
        if (ac == "ok")
        {
            new BCW.BLL.tb_QuestionsList().Delete(ID);
            Utils.Success("删除成功", "删除成功，正在返回..", Utils.getUrl("questions.aspx?act=question&amp;"), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该问题吗?" + "<br/>");
            builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=del&amp;ac=ok&amp;game=2&amp;ID=" + ID + "") + "\">是</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=manage&amp;game=2&amp;ID=" + ID + "") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
    }

    //delstyle 删除问题类型
    protected void DelStylePage()
    {
        string ac = (Utils.GetRequest("ac", "all", 1, @"^", "1"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "1"));
        if (!new BCW.BLL.tb_QuestionsType().Exists(ID))
        {
            Utils.Error("该类型记录不存在！", "");
        }
        if (ac == "ok")
        {
            new BCW.BLL.tb_QuestionsType().Delete(ID);
            Utils.Success("删除成功", "删除成功，正在返回..", Utils.getUrl("questions.aspx?act=style&amp;"), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该问题类型吗?" + "<br/>");
            builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=delstyle&amp;ac=ok&amp;game=2&amp;ID=" + ID + "") + "\">是</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=style&amp;game=2&amp;ID=" + ID + "") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
    }

    //重置游戏
    private void ResetPage()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(GameName + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定重置" + GameName + "吗，重置后将删除所有记录！");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_QuestionAnswer");
            new BCW.Data.SqlUp().ClearTable("tb_QuestionController");
            new BCW.Data.SqlUp().ClearTable("tb_QuestionsAnswerCtr");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("questions.aspx"), "1");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    #region 底部
    //底部
    private void Bottom()
    {
        //底部
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    
    #region 问答规则
    private void Rule()
    {
        Master.Title = GameName + "规则";
        string QuestionsRule = Convert.ToString(ub.GetSub("QuestionsRule", "/Controls/questions.xml"));
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("规则查看");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(QuestionsRule)));
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //// builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>");
        //builder.Append(Out.Tab("</div>", ""));
        Bottom();
    }
    #endregion
}