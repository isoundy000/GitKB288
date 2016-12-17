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
using System.Net;
using System.Text;
using System.IO;
using System.Timers;
using System.Xml;
using System.Security.Cryptography;
public partial class bbs_game_questions : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected int GameId = 45;
    protected string xmlPath = "/Controls/questions.xml";
    protected string GameName = Convert.ToString(ub.GetSub("GameName", "/Controls/questions.xml"));
    string question_deficult = "0|新手|1|初级|2|中级|3|高级|4|困难|5|大师";//难度
    protected string[] question_deficult_list = { "新手", "初级", "中级", "高级", "困难", "大师" };//难度
    protected string QuestionstestID = ub.GetSub("QuestionstestID", "/Controls/questions.xml");//测试id    
    protected string QuestionsStatue = ub.GetSub("QuestionsStatue", "/Controls/questions.xml");//0|维护|1|正常|2|测试
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (QuestionsStatue == "0")
        {
            Utils.Safe("" + GameName + "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule"://规则介绍
                Rule();
                break;
            case "choose"://选着问题类型
                ChooseTypePage();
                break;
            case "all"://回答判断
                AllPage();
                break;
            case "start"://游戏开始
                StartPage();
                break;
            case "gamestart"://游戏开始
                GameStartPage();
                break;
            case "judge"://回答判断
                JudgePage();
                break;
            case "view"://回答判断
                ViewPage();
                break;
            case "alist"://查看问题
                aListPage();
                break;
            case "goods"://查看问题
                GoodsPage();
                break;
            case "contrl"://发布后回答问题
                ContrlPage();
                break;
            case "contrlanswer"://发布后回答确认问题
                ContrlAnswerPage();
                break;
            case "next"://继续回答发布问题
                ContralNextAnswer();
                break;
            case "viewt"://答题结果2
                ViewPageTwo();
                break;
            case "sviewpt"://传入控制器ID普通答题结果
                ViewPTPageTwo();
                break;
            case "paihang"://排行榜单
                TopListPage();
                break;
            case "mylist"://我的历史
                myListPage();
                break;
            case "askresult"://回答一个问题结果列表
                AskResultPage();
                break;
            case "ansctr"://每一轮的ID列
                AnswerCtrPage();
                break;
            case "returnnext"://跳转到答题
                NextPage();
                break;
            case "paijiang"://派奖
                PaiJiangPage();
                break;
            case "quit"://派奖
                QuitPage();
                break;
            case "caseok"://本页兑奖
                CaseOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    //主页
    protected void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = GameName;
        if (QuestionsStatue == "2")
        {
            if (!("#" + QuestionstestID + "#").Contains("#" + meid.ToString() + "#"))
            {
                Utils.Error("很抱歉,您暂未有测试该游戏的权限", "");
            }
        }
        else if (QuestionsStatue == "0")
        {
            Utils.Safe("此游戏");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append(GameName);
        builder.Append(Out.Tab("</div>", "<br/>"));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //// builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        //builder.Append("<b>" + GameName + "活动首页</b>");
        //builder.Append(Out.Tab("</div>", "<br/>"));

        #region  顶部ubb
        //顶部ubb
        string QuestionsTop = ub.GetSub("QuestionsTop", xmlPath);
        string QuestionsTopTwo = ub.GetSub("QuestionsTopTwo", xmlPath);
        string QuestionsGundong = ub.GetSub("QuestionsGundong", xmlPath);
        if (QuestionsTop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(QuestionsTop)));
            if (QuestionsGundong == "1")
            {
                if (QuestionsTopTwo != "")
                {
                    builder.Append("<marquee behavior=\"alternate\" scrollamount=\"1\" width=\"90%\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(QuestionsTopTwo)) + "</marquee>");
                }
            }
            else
            if (QuestionsGundong == "2")
            {
                if (QuestionsTopTwo != "")
                {
                    builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(QuestionsTopTwo)));
                }
            }
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        //顶部Logo
        string QuestionsImg = ub.GetSub("QuestionsImg", xmlPath);
        if (QuestionsImg != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + QuestionsImg + "\" alt=\"load\"  />");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion

        #region 连接 启用
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=rule") + "\">规则</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;ptype=2&amp;") + "\">兑奖</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paihang&amp;ptype=1&amp;") + "\">排行</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=all") + "\">动态</a>");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=all") + "\"><img src=\"" + "http://kb288.cc/FILES/kubao1/new.gif" + "\" alt=\"load\"/></a>");
        //builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=view&amp;game=2&amp;") + "\">历史</a>.");
        //builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=choose&amp;game=2&amp;") + "\">答题</a>.");
        //builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=goods&amp;game=2&amp;") + "\">道具</a>.");
        //builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=goods&amp;game=2&amp;") + "\">道具</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        #endregion

        #region 连接 仿拾物 弃用
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("排行: <a href=\"" + Utils.getUrl("questions.aspx?act=paihang&amp;ptype=1&amp;") + "\">总量榜</a>  ");
        //builder.Append(" <a href=\"" + Utils.getUrl("questions.aspx?act=paihang&amp;ptype=2&amp;") + "\">答对榜</a> <br/> ");
        //builder.Append(" <a href=\"" + Utils.getUrl("questions.aspx?act=mylist") + "\">我的答题</a> ");
        //builder.Append(" <a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;ptype=2&amp;") + "\">兑换奖品</a>");
        //builder.Append(Out.Tab("</div>", "<br/>"));
        #endregion

        #region 最新挑战
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【最新挑战】");
        try
        {
            BCW.Model.tb_QuestionsAnswerCtr check = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtrByUid(meid);
            if (check.count > check.now)
            {
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=returnnext&amp;qId=" + check.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[继续作答]</a>");
            }
        }
        catch { }
        builder.Append(Out.Tab("</div>", "<br/>"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_QuestionAnswer> listPayrmb = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswers(pageIndex, pageSize, strWhere, out recordCount);
        if (listPayrmb.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_QuestionAnswer n in listPayrmb)
            {
                if (k > 5)
                { break; }
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.addtime)) + "前<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.usname + "</a>");
                builder.Append("回答" + "<a href =\"" + Utils.getUrl("questions.aspx?act=alist&amp;ID=" + n.questID + "&amp;") + "\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(strSub(n.questtion))) + "</a>");//+ "[" + Convert.ToDateTime(n.addtime).ToString("HH:mm") + "]"
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无相关挑战.");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=all") + "\">更多挑战&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 排行top5
        try
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("【排行TOP 5】");
            builder.Append(Out.Tab("</div>", ""));
            int summod;
            string str = "ID>0 group by usid order by count desc ";
            DataSet ds = new BCW.BLL.tb_QuestionAnswer().GetList(" top 6 usid,count(usid)as count", str);
            summod = 4;
            builder.Append(Out.Tab("<div>", "<br/>"));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (summod < i)
                    { break; }
                    else
                    {
                        builder.Append("<font  color=\"red\">" + "[TOP " + (i + 1) + "]" + "</font>" + " ");
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[i]["usid"]));
                        builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(ds.Tables[0].Rows[i]["usid"]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ds.Tables[0].Rows[i]["usid"])) + "</a>");
                        // builder.Append("(" + ds.Tables[0].Rows[i]["usid"] + ")");
                        builder.Append("已回答");
                        builder.Append("<font  color=\"red\">" + ds.Tables[0].Rows[i]["count"] + "</font>" + "题");
                        builder.Append("<br/>");
                    }
                }
            }
            else
            {
                builder.Append("暂无相关排行." + "<br/>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        catch { builder.Append("暂无相关排行." + "<br/>"); }
        #endregion

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=输入ID查找=");
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,,";
        string strName = "uid,ptype,act";
        string strType = "num,hidden,hidden";
        string strValu = "'2'all";
        string strEmpt = "false,false";
        string strIdea = "";
        string strOthe = "查清单,questions.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        #region 选择模式 已注释
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("【选择模式】");
        //builder.Append(Out.Tab("</div>", "<br/>"));
        ////  builder.Append(Out.Tab("<div>", ""));
        ////  //builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=lists&amp;game=1&amp;") + "\">单人模式</a>|");
        //////  builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=toplist&amp;game=2&amp;") + "\">双人对战</a>");
        ////  builder.Append(Out.Tab("</div>", "<br/>"));
        //if (game == 1)
        //{
        //    builder.Append(Out.Tab("<div>", ""));
        //    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=choose&amp;game=1&amp;") + "\">选择题型</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=toplist&amp;game=2&amp;") + "\">闯关模式</a>");
        //    builder.Append(Out.Tab("</div>", "<br/>"));
        //}
        //else if (game == 2)
        //{
        //    builder.Append(Out.Tab("<div>", ""));
        //    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=choose&amp;game=1&amp;") + "\">选择题型</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=toplist&amp;game=2&amp;") + "\">闯关模式</a>");
        //    // builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=toplist&amp;game=2&amp;") + "\">双人闯关</a>");
        //    builder.Append(Out.Tab("</div>", "<br/>"));
        //}
        #endregion


        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(20, "questions.aspx", 5, 0)));


        //底部
        Bottom();
    }

    //quit 取消答题
    protected void QuitPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("取消答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        if (!new BCW.BLL.tb_QuestionsAnswerCtr().Exists(qId))
        { Utils.Error("不存在的答题记录！", ""); }
        string info = Utils.GetRequest("info", "all", 1, @"", "");
        if (info == "ok")
        {

            BCW.Model.tb_QuestionsAnswerCtr model = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(qId);
            if (meid != model.uid)
            { Utils.Error("不存在的答题记录！", ""); }
            model.ishit = 5;
            new BCW.BLL.tb_QuestionsAnswerCtr().Update(model);
            Utils.Success("取消成功", "取消成功，本轮问题将不再回答,正在返回..", Utils.getUrl("questions.aspx?act=style&amp;"), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("取消后本轮问题将不再回答,确定取消回答该问题吗?" + "<br/>");
            builder.Append("<a href=\"" + Utils.getPage("questions.aspx?act=quit&amp;ac=ok&amp;info=ok&amp;qId=" + qId + "&amp;") + "\">确认取消</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;game=2&amp;") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        Bottom();

    }

    //本页兑奖 添加时间2016 未使用
    private void CaseOkPage()
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
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BLL.tb_QuestionsAnswerCtr().Exists(pid))
            {
                BCW.Model.tb_QuestionsAnswerCtr model = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(pid);
                if (model.uid == meid)
                {
                    //if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(model.AddTime).AddHours((double)model.overTime)) > 0)
                    //{
                    //    model.Ident = 2;
                    //    new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 2);//2过期
                    //                                                                            // builder.Append("该记录因过期已无法领取,预祝好运连连！");
                    //}
                    //else
                    //{
                    //    if (model.Ident == 1)
                    //    {
                    //        //操作币
                    //        long win = Convert.ToInt64(model.winGold);
                    //        new BCW.BLL.tb_WinnersLists().UpdateIdent(pid, 0);
                    //        winMoney += win;
                    //        new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.UserId), win, "活跃抽奖本页兑奖ID" + model.awardId + "#编号" + pid + "");
                    //    }
                    //}
                }
            }
            //else
            //{ builder.Append("id不存在"); }
        }
        if (winMoney == 0)
        {
            Utils.Success("兑奖", "本页已兑奖，请勿重复提交", Utils.getUrl("questions.aspx?act=mylist&amp;ptype=2&amp;"), "1");
        }
        else
        {
            string OutText = Convert.ToString(ub.GetSub("OutText", "/Controls/winners.xml"));
            string Print_text = "";
            if (OutText != "")
            {
                string[] getText = OutText.Split('#');
                Random index_n = new Random();
                Print_text = getText[index_n.Next(0, getText.Length)];
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    Print_text = "<font color=\"#FF0000\">" + Print_text + "</font>" + "";
                }
            }
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "<br/>" + Print_text, Utils.getUrl("winners.aspx?act=lists"), "20");
        }
    }

    //returnnext 跳转回答题页
    protected void NextPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        int CII = 0;//用户回答记录控制器
        if (!new BCW.BLL.tb_QuestionController().Exists(qId))
        { Utils.Error("该问题不存在！", ""); }
        BCW.Model.tb_QuestionsAnswerCtr user = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtrByUid(meid);//该用户的控制器   
        Utils.Success("正在跳转！", "正在进入答题,3s后自动跳转到答题页..", Utils.getUrl("questions.aspx?act=next&amp;qId=" + qId + ""), "3");
    }

    //paijiang 派奖
    protected void PaiJiangPage()
    {
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("兑奖");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 2, @"^[1-9]\d*$", "不存在的兑奖记录"));
        BCW.Model.tb_QuestionsAnswerCtr model = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(ID);
        if (!new BCW.BLL.tb_QuestionsAnswerCtr().Exists(ID) || meid != model.uid)
        { Utils.Error("不存在的兑奖记录！", ""); }
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
        if (sendGold > 0 && model.ishit == 1 && model.now == model.count)//1未领 0已领
        {
            if (ts.Minutes < 48 * 60 && check > DateTime.Now)//答题时限内，并且在有效领取时间内
            {
                if (QuestionsStatue == "2")//测试
                {
                    if (!new BCW.SWB.BLL().ExistsUserID(meid, GameId))//不存在用户记录直接领
                    {
                        BCW.SWB.Model swbs = new BCW.SWB.Model();
                        swbs.UserID = meid;
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
                        BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, GameId);
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
                    new BCW.BLL.Action().Add(meid, "在[URL=/bbs/game/questions.aspx]" + GameName + "[/URL]" + "回答奖励" + sendGold + "问币");
                    model.awardgold = sendGold;
                    model.ishit = 0;//成功领取
                }
                else
                {
                    new BCW.BLL.User().UpdateiGold(meid, sendGold, GameName + "第" + model.ID + "轮兑奖");
                    string uname = new BCW.BLL.User().GetUsName(meid);
                    new BCW.BLL.Action().Add(meid, "在[URL=/bbs/game/questions.aspx]" + GameName + "[/URL]" + "回答奖励" + "**" + ub.Get("SiteBz"));
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
        string textgold = ub.Get("SiteBz");
        if (QuestionsStatue == "2")
        {
            textgold = " 问币";
        }
        builder.Append("【奖金价值】  <font color =\"green\">" + sendGold + " </font>" + textgold + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;ptype=2&amp;") + "\">" + "返回上级" + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        Bottom();
    }

    //ansctr 每一轮的id列输出
    protected void AnswerCtrPage()
    {
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        if (!new BCW.BLL.tb_QuestionsAnswerCtr().Exists(qId))
        { Utils.Error("不存在的兑奖记录！", ""); }
        BCW.Model.tb_QuestionsAnswerCtr model = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(qId);
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        if (meid != model.uid)
        { Utils.Error("不存在的答题结果！", ""); }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>" + "&gt;" + "第" + model.contrID + "轮答题结果");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string[] qlist = model.List.Split('#');
        string[] qexplain = model.explain.Split('#');
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("[本轮答题] " + "第" + model.contrID + "轮");
        double percent = Convert.ToDouble(model.trueCount) / Convert.ToDouble((model.count));
        //if (model.isDone == 0)
        //{ builder.Append("(已答完)"); }
        //else if (model.isDone == 1)
        //{ builder.Append("(<a href=\"" + Utils.getUrl("questions.aspx?act=returnnext&amp;qId=" + qId + "&amp;") + "\">" + "未答完" + "</a>)"); }
        //else
        //{ builder.Append("(已过期)"); }
        builder.Append(" (<font color=\"green\">" + IsHit(Convert.ToInt32(model.ishit)) + "</font>)");
        builder.Append("<br/>");
        builder.Append("[答题时间] " + Convert.ToDateTime(model.addtime).ToString("yyyy-MM-dd HH:mm"));
        builder.Append("<br/>");
        builder.Append("[截至时间] " + Convert.ToDateTime(model.overtime).ToString("yyyy-MM-dd HH:mm"));
        builder.Append("<br/>");
        builder.Append("[题目数量] " + model.count);
        builder.Append("(正" + "<font color=\"green\">" + model.trueCount + "</font>");
        builder.Append(" 误" + "<font color=\"red\">" + model.flaseCount + "</font>" + ")");
        builder.Append("(正确率" + (percent * 100).ToString() + "%)");
        builder.Append("<br/>");
        builder.Append("[题目记录] ");
        for (int i = 0; i < qlist.Length; i++)
        {
            if (i == 4)
            { builder.Append("<br/>"); }
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=alist&amp;ID=" + qlist[i] + "&amp;") + "\">" + qlist[i] + "</a>" + "  ");
        }
        builder.Append("<br/>");
        builder.Append("[回答记录] ");
        if (model.explain.Contains("#"))
        {
            for (int i = 0; i < qexplain.Length; i++)
            {
                //builder.Append(""+qexplain[i]);
                builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=askresult&amp;ID=" + qexplain[i] + "&amp;") + "\">" + qexplain[i] + "</a>" + "  ");
                if (i == 4)
                { builder.Append("<br/>"); }
            }
        }
        else
        {
            builder.Append("暂无回答");
        }

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
            builder.Append(" <br/>[前往查看] <a href=\"" + Utils.getUrl("questions.aspx?act=sviewpt&amp;ID=" + model.ID + "&amp;") + "\">" + "详细记录>>" + "</a>");
        else
        {
            builder.Append(" <br/>[答题状态]  当前第" + (model.now + 1) + "题");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("");

        // builder.Append("<br/>");
        Bottom();
    }

    //askresult 回答一个问题结果列表
    protected void AskResultPage()
    {
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("查看回答结果");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "0"));//题目类型
        if (!new BCW.BLL.tb_QuestionAnswer().Exists(ID))
        {
            Utils.Error("不存在的问题记录！", "");
        }
        else
        {
            BCW.Model.tb_QuestionAnswer model = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID);
            BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(Convert.ToInt32(model.questID));
            if (model.usid != meid)
            {
                Utils.Error("不存在的回答记录！", "");
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("[回答记录] " + "<font color=\"green\">" + model.ID + "</font>");
            string qtype = "";
            try
            { qtype = new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.type)); }
            catch { qtype = "随机题目"; }
            builder.Append("<br/>[题目类型]  " + "<font color=\"green\">" + qtype + "</font>");
            builder.Append("<br/>[难度系数] " + "<font color=\"green\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font>");
            builder.Append("<br/>[问题阐述] " + "<font color=\"red\">" + n.question + "</font>");
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
            builder.Append("<br/><font color=\"green\">[我的答案] " + "" + model.answer + "</font>");
            if (model.isTrue == 1)
            { builder.Append("<font color=\"green\">" + "(正确)" + "</font>"); }
            else
            { builder.Append("<font color=\"red\">" + "(错误)" + "</font>"); }
            double percent = Convert.ToDouble(n.trueAnswer) / Convert.ToDouble((n.trueAnswer + n.falseAnswer));
            // builder.Append("<br/>答对: " + "<font color=\"red\">" + n.trueAnswer + "</font>");
            if (percent > 0)
                builder.Append(" <br/>[题正确率] " + "<font color=\"red\">" + (percent * 100).ToString() + "%</font>");
            builder.Append(" <br/>[回答时间] " + "<font color=\"black\">" + Convert.ToDateTime(model.addtime).ToString("yyyy-MM-dd HH:mm") + "</font>");
            if (n.remarks == "")
                n.remarks = "暂无";
            builder.Append("<br/>[问题解析] " + "<font color=\"green\">" + n.remarks + "</font><br/>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入问题ID:/,";
            string strName = "ID,backurl";
            string strType = "num,hidden";
            string strValu = ID + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜问题记录,questions.aspx?act=askresult&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        Bottom();

    }

    //mylist 我的历史
    protected void myListPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>" + "&gt;" + "我的答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-3]$", "1"));
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

            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;ptype=2&amp;") + "\">按每轮</a> | ");
        }
        if (ptype == 1)
        {
            builder.Append("按答题 | ");
            str = "ID>0 group by usid order by count desc ";
            outputtext = "总答 ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;ptype=1&amp;") + "\">按答题</a> | ");

        }
        if (ptype == 3)
        {
            builder.Append("按个人 <br/> ");
            str = "ID>0 and isTrue=1 group by usid order by count desc ";
            outputtext = "个人";
        }
        else
        {

            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=mylist&amp;ptype=3&amp;") + "\">按个人</a><br/>");
        }

        builder.Append(Out.Tab("</div>", ""));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));


        string[] pageValUrl = { "act", "backurl", "start", "down", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        #region 按答题数
        if (ptype == 1)
        {
            string strWhere = "usid= " + meid + "";
            try
            {
                if (start.Length > 1)
                {

                    strWhere = "usid= " + meid + "and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
                }
                else
                {
                    start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch { Utils.Error("输入时间有误！", ""); }
            // 开始读取列表
            IList<BCW.Model.tb_QuestionAnswer> listSSCpay = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswers(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.tb_QuestionAnswer n in listSSCpay)
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
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usid));
                    if (n.isTrue == 1)
                        builder.AppendFormat("{0}前回答问题{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.addtime)), n.usid, mename, "<a href=\"" + Utils.getUrl("questions.aspx?act=askresult&amp;ID=" + n.ID + "&amp;") + "\"><font color=\"green\">" + Out.SysUBB(n.questtion) + "</font></a>");
                    else
                        builder.AppendFormat("{0}前回答问题{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.addtime)), n.usid, mename, "<a href=\"" + Utils.getUrl("questions.aspx?act=askresult&amp;ID=" + n.ID + "&amp;") + "\"><font color=\"red\">" + Out.SysUBB(n.questtion) + "</font></a>");

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无相关记录.");
                builder.Append(Out.Tab("</div>", ""));
            }
            string strText1 = "开始时间:,结束时间:,";
            string strName1 = "start,down,backurl";
            string strType1 = "text,text,hidden";
            string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,true，false";
            string strIdea1 = "/";
            string strOthe1 = "按时间搜索,questions.aspx?act=mylist&amp;ptype=" + ptype + "&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        #endregion

        #region 按每轮数
        else if (ptype == 2)
        {
            string strWhere = "uid = " + meid + "";
            string arrId = "";
            try
            {
                if (start.Length > 1)
                {

                    strWhere = "uid= " + meid + "and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
                }
                else
                {
                    start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch { Utils.Error("输入时间有误！", ""); }
            //控制器列表
            // 开始读取列表
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
                    builder.AppendFormat("{0}前回答{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.addtime)), n.uid, mename, "<a href=\"" + Utils.getUrl("questions.aspx?act=ansctr&amp;qId=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><font color=\"green\"> 第" + Out.SysUBB(n.contrID.ToString()) + "轮 </font></a>问题!");
                    if (n.now != n.count)
                    {
                        if (n.ishit == 5)
                        {
                            builder.Append("<font color=\"green\">[已取消]</font>");
                        }
                        else
                        {
                            //builder.Append("(未答完)");
                            // builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=next&amp;ID=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[继续作答]</a>");
                            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=returnnext&amp;qId=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[继续作答]</a>");
                            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=quit&amp;qId=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[取消]</a>");
                        }
                    }
                    else
                    {
                        if (n.ishit == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=paijiang&amp;ID=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[兑奖]</a>");
                        }
                        else
                            if (n.ishit == 0)
                        {
                            builder.Append("[已兑奖]");
                        }
                        else
                        if (n.ishit == 3)
                        {
                            builder.Append("[答题超时]");
                        }
                        else if (n.ishit == 2)
                        {
                            builder.Append("[已过期]");
                        }
                        else if (n.ishit == 4)
                        {
                            builder.Append("[答错无奖励]");
                        }
                        else if (n.ishit == 5)
                        {
                            builder.Append("<font color=\"green\">[已取消]</font>");
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
                string strText1 = "开始时间:,结束时间:,";
                string strName1 = "start,down,backurl";
                string strType1 = "text,text,hidden";
                string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
                string strEmpt1 = "true,true，false";
                string strIdea1 = "/";
                string strOthe1 = "按时间搜索,questions.aspx?act=mylist&amp;ptype=" + ptype + "&amp;,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));

            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无相关记录.");
                builder.Append(Out.Tab("</div>", ""));
            }

        }

        #endregion

        #region 按个人
        else
        {
            try
            {
                //int usid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^", "0"));
                //if (usid > 0)
                //    meid = usid;
                builder.Append(Out.Tab("<div>", ""));
                int allcount = new BCW.BLL.tb_QuestionAnswer().GetAllCounts(meid);
                int truecount = new BCW.BLL.tb_QuestionAnswer().GetTrueCounts(meid);
                double percent = Convert.ToDouble(truecount) / Convert.ToDouble(allcount);
                //获取称谓
                string QuestionsChengWei = ub.GetSub("QuestionsChengWei", xmlPath);
                string[] QuestionsChengWei2 = QuestionsChengWei.Split('#');
                string chengwei = "";
                int dengji = 0;
                //truecount = 1;
                for (int i = 0; i < QuestionsChengWei2.Length; i++)
                {
                    if (i > 0)
                    {
                        if (truecount / (3000 * i * i) > 0)
                        {
                            chengwei = QuestionsChengWei2[i].ToString();
                            dengji = i;
                        }
                    }
                    else
                    {
                        chengwei = QuestionsChengWei2[0].ToString();
                        dengji = 0;
                    }
                }
                string strw = "usid=" + meid + " group by usid order by count desc ";
                DataSet ds = new BCW.BLL.tb_QuestionAnswer().GetList(" top 6 usid,count(usid)as count", strw);
                string mingci = " 0 ";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i > 500)
                        {
                            mingci = "大隐隐于市,是华丽的低调(500名之后..)";
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["usid"].ToString() == meid.ToString())
                            {
                                mingci = "第" + (i + 1) + "名";
                            }
                            else
                            {
                                mingci = "大隐隐于市,是华丽的低调(暂找不到我的排名)";
                            }
                        }
                    }
                }
                else
                {
                    mingci = "大隐隐于市,是华丽的低调(暂找不到我的排名)";
                }
                //战果
                string zhanguo = "";
                // if (percent > 0.1)
                {
                    zhanguo = "我已击败全国<b>" + percent * 100 + "%</b>的玩家了!";
                }

                builder.Append("<font color=\"red\">" + "昵称: </font>" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">" + new BCW.BLL.User().GetUsName(meid) + " </a><br/>");
                builder.Append("<font color=\"red\">当前称谓:</font> " + "<font color=\"green\">" + chengwei + "</font> (" + getDengJiChinese(dengji) + "级称谓)");
                builder.Append("<br/>");
                builder.Append("<font color=\"red\">我的排名:</font> " + "<font color=\"green\">" + mingci + " </font>");
                builder.Append("<br/>");
                builder.Append("总答题数: <font color=\"green\">" + allcount + " </font>题");
                builder.Append("<br/>");
                builder.Append("答对题数: <font color=\"green\">" + truecount + " </font>题");
                builder.Append("<br/>");
                if ((percent).ToString("p").Contains("%"))
                {
                    builder.Append("正确率: <font color=\"green\">" + (percent).ToString("p") + "</font>");
                    builder.Append("<br/>");
                }
                builder.Append("获得总奖励: " + "<font color=\"green\">" + new BCW.BLL.tb_QuestionsAnswerCtr().GetAllAwardGold(meid) + "</font>" + " " + ub.Get("SiteBz"));
                builder.Append("<br/>");
                builder.Append("战果: " + zhanguo);
                builder.Append(Out.Tab("</div>", ""));
            }
            catch
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无更多记录！");
                builder.Append(Out.Tab("</div>", ""));
            }
            #region 查找功能 已屏蔽
            //builder.Append(Out.Tab("<div class=\"text\">", ""));
            //builder.Append("=输入ID查找=");
            //builder.Append(Out.Tab("</div>", ""));

            //string strText = ",,,";
            //string strName = "uid,ptype,act";
            //string strType = "num,hidden,hidden";
            //string strValu = usid+"'3'mylist";
            //string strEmpt = "false,false";
            //string strIdea = "";
            //string strOthe = "查清单,questions.aspx,get,1,red";
            //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }
        #endregion

        Bottom();

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

    #region  返回ishit的类型
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
    #endregion

    #region 底部
    //底部
    private void Bottom()
    {
        //底部
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

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
        if (meid == 0)
            Utils.Login();
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
                builder.Append("暂无相关排行,快快去加入动态吧！");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        //  catch { builder.Append("暂无相关排行,快快去加入动态吧！"); }
        Bottom();
    }

    //sviewpt 普通答题结果
    protected void ViewPTPageTwo()
    {
        Master.Title = GameName + "";
        string QuestionsRule = Convert.ToString(ub.GetSub("QuestionsRule", "/Controls/questions.xml"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;答题结果" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int qId = int.Parse(Utils.GetRequest("qId", "get", 1, @"^[^\^]{0,200}$", "1"));
        string q_type = "";
        //  try
        {
            if (!new BCW.BLL.tb_QuestionController().Exists(qId))
            { Utils.Error("该问题不存在！", ""); }
            if (!new BCW.BLL.tb_QuestionsAnswerCtr().ExistsID(meid, qId))
            { Utils.Error("该问题不存在！", ""); }
            if (!new BCW.BLL.tb_QuestionsAnswerCtr().Exists(qId))
            { Utils.Error("该问题不存在！", ""); }
            BCW.Model.tb_QuestionsAnswerCtr model1 = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(qId);
            BCW.Model.tb_QuestionController ctr = new BCW.BLL.tb_QuestionController().Gettb_QuestionController(Convert.ToInt32((model1.contrID)));
            string[] index = model1.List.Split('#');
            string[] index1 = model1.explain.Split('#');
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
            double percent = Convert.ToDouble(model1.trueCount) / Convert.ToDouble(model1.count);
            int sendGold = 0;
            if (model1.trueCount == model1.count)
            {
                builder.Append("正确率:<font color=\"red\">" + "100%</font>");
            }
            else
                builder.Append(" 正确率:<font color=\"red\">" + (percent * 100).ToString() + "%</font>");

            string OutText = Convert.ToString(ub.GetSub("QuestionsSelectText", "/Controls/questions.xml"));
            string[] getText = OutText.Split('#');
            Random index_n = new Random();
            string Print_text = getText[index_n.Next(0, getText.Length)];
            #region 自动派取奖励
            if (ctr.awardtype == 0)//0全答才有奖励（无论对不对） 1全对 2答了
            {
                builder.Append("<br/>获得奖金: " + "<font color=\"green\">" + ctr.award + " </font>" + ub.Get("SiteBz"));
            }
            else
            if (ctr.awardtype == 1)// 1全对 2答了
            {
                if (model1.flaseCount == 0)//全对 派奖
                {
                    builder.Append("恭喜你获得本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>" + ub.Get("SiteBz"));
                }
                else//答错一题拿奖失败
                {
                    builder.Append("<br/><font color=\"green\">" + "Oh~~,你答错题了,大奖与你擦边而过啦(到嘴的肉飞走了)..." + "</font>");
                }
            }
            else
                if (ctr.awardtype == 2)//答了
            {
                builder.Append("恭喜你获得本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>" + ub.Get("SiteBz"));
            }
            if (model1.ishit == 1)//0 已领 1未领 2过期 3答题超时 4回答错误不能领奖
            {
                builder.Append("  <a href=\"" + Utils.getUrl("questions.aspx?act=paijiang&amp;ID=" + model1.ID + "&amp;") + "\">" + "领取" + "</a>");
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

            // Utils.Error(""+Print_text+"","");
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //{
            //    Print_text = "<br/><font color=\"#FF0000\">" + Print_text + "</font>" + "<br/>";
            //}
            builder.Append("<br/><font color =\"#FF0000\">" + Print_text + "</font>" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Model.tb_QuestionsList n;
            BCW.Model.tb_QuestionAnswer n1;
            for (int i = 0; i < model1.count; i++)
            {
                int ID = Convert.ToInt32(index[i]);
                int ID1 = Convert.ToInt32(index1[i]);
                n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
                n1 = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID1);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append((i + 1) + ".问题: " + "<font color =\"green\">" + n.question + "</font>");
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

                builder.Append("<br/>我的答案:" + n1.answer);
                if (n1.isTrue == 1)
                {
                    builder.Append(" " + "(" + "<font color=\"green\">" + "正确" + "</font>" + ")");
                }
                else
                { builder.Append(" " + "(" + "<font color=\"red\">" + "错误" + "</font>" + ")"); }
                builder.Append("<br/>" + "正确答案:" + n.answer + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        //catch (Exception ee)
        //{ builder.Append(ee + "<br/>" + meid + "---" + "" + "*******" + ID); }
        Bottom();
    }

    //viewt 最新答题结果
    protected void ViewPageTwo()
    {
        Master.Title = GameName + "";
        string QuestionsRule = Convert.ToString(ub.GetSub("QuestionsRule", "/Controls/questions.xml"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;答题结果" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        string q_type = "";
        try
        {
            if (!new BCW.BLL.tb_QuestionController().Exists(qId))
            { Utils.Error("该问题不存在！", ""); }
            if (!new BCW.BLL.tb_QuestionsAnswerCtr().ExistsID(meid, qId))
            { Utils.Error("该问题不存在！", ""); }
            if (!new BCW.BLL.tb_QuestionsAnswerCtr().Exists(qId))
            { Utils.Error("该问题不存在！", ""); }
            BCW.Model.tb_QuestionsAnswerCtr model1 = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtrByUid(meid);
            BCW.Model.tb_QuestionController ctr = new BCW.BLL.tb_QuestionController().Gettb_QuestionController(Convert.ToInt32((model1.contrID)));
            string[] index = model1.List.Split('#');
            string[] index1 = model1.explain.Split('#');
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
            //builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font><br/>");
            builder.Append("本轮记录: 第" + model1.ID + "轮  题数:" + "<font color=\"red\">" + model1.count + "</font>" + "<br/>");
            builder.Append("本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>");
            builder.Append("<br/>");
            builder.Append("正确数:<font color=\"red\">" + model1.trueCount + " </font>");
            builder.Append("错误数:<font color=\"red\">" + model1.flaseCount + "</font>");
            double percent = Convert.ToDouble(model1.trueCount) / Convert.ToDouble(model1.count);
            int sendGold = 0;
            if (model1.flaseCount == 0)
            {
                builder.Append("正确率:<font color=\"red\">" + " 100%</font>");
            }
            else
                builder.Append(" 正确率:<font color=\"red\">" + (percent * 100).ToString() + "%</font>");
            #region 自动派取奖励
            string textgold = ub.Get("SiteBz");
            if (QuestionsStatue == "2")
            {
                textgold = " 问币";
            }
            if (ctr.awardtype == 0)//0全答才有奖励（无论对不对） 1全对 2答了
            {
                builder.Append("<br/>获得奖金: " + "<font color=\"green\">" + model1.awardgold + " </font>" + textgold);
            }
            else
            if (ctr.awardtype == 1)// 1全对 2答了
            {
                if (model1.flaseCount == 0)//全对 派奖
                {
                    builder.Append("恭喜你获得本轮奖金:" + "<font color=\"green\">" + model1.awardgold + "</font>" + textgold);
                }
                else//答错一题拿奖失败
                {
                    builder.Append("<br/><font color=\"green\">" + "Oh~~,你答错题了,大奖与你擦边而过啦(到嘴的肉飞走了)..." + "</font><br/>");
                }
            }
            else
                if (ctr.awardtype == 2)//答了
            {
                builder.Append("恭喜你获得本轮奖金:" + "<font color=\"green\">" + ctr.award + "</font>" + textgold);
            }
            if (model1.ishit == 1)
            {
                builder.Append("  <a href=\"" + Utils.getUrl("questions.aspx?act=paijiang&amp;ID=" + model1.ID + "&amp;") + "\">" + "领取" + "</a>");
            }
            else
            { builder.Append("  " + "[状态:<font color=\"green\">" + IsHit(Convert.ToInt32(model1.ishit)) + "</font>]" + ""); }
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
            builder.Append("<br/><font color =\"#FF0000\">" + Print_text + "</font>" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Model.tb_QuestionsList n;
            BCW.Model.tb_QuestionAnswer n1;
            int ID = 0;
            int ID1 = 0;
            for (int i = 0; i < model1.count; i++)
            {
                ID = Convert.ToInt32(index[i]);
                ID1 = Convert.ToInt32(index1[i]);
                n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
                n1 = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID1);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append((i + 1) + ".问题: " + "<font color =\"green\">" + n.question + "</font>");
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
                builder.Append("<br/><font color=\"green\">你的答案:" + n1.answer + "</font>");
                if (n1.isTrue == 1)
                {
                    builder.Append(" " + "(" + "<font color=\"green\">" + "正确" + "</font>" + ")");
                }
                else
                { builder.Append(" " + "(" + "<font color=\"red\">" + "错误" + "</font>" + ")"); }
                builder.Append("<br/>" + "正确答案:" + n.answer + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch
        { Utils.Error("暂不存在该回答记录!", ""); }
        Bottom();
    }

    //next 发布的内线继续答题
    private void ContralNextAnswer()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("继续答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        string q_type = "";//题目类型
        if (meid == 0)
            Utils.Login();
        //Utils.Error(""+ cid + "","");
        if (!new BCW.BLL.tb_QuestionController().Exists(qId))
        { Utils.Error("该问题不存在！", ""); }
        BCW.Model.tb_QuestionsAnswerCtr user = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(qId);//该用户的控制器
        if (user.ishit == 5)
        {
            Utils.Success("正在跳转！", "该问题已取消回答,3s后自动跳转到首页..", Utils.getUrl("questions.aspx"), "3");
        }
        string[] q = user.explain.Split('#');
        if (user.now == user.count || (q.Length - 1) == user.count)
        {
            Utils.Success("答题成功！", "本轮题目已答完,3s后跳转到答题结果..", Utils.getUrl("questions.aspx?act=sviewpt&amp;qId=" + qId + ""), "2");
        }
        string[] list = user.List.Split('#');
        int questionID = int.Parse(list[(Convert.ToInt32(user.now))]);//下一题
        BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(questionID);
        try
        {
            q_type = new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.styleID));
        }
        catch
        { q_type = "随机题目"; }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("当前回答:" + "第<font color=\"red\">" + user.contrID + "</font>轮");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("题目类型:" + "<font color=\"red\">" + q_type + "</font><br/>");
        builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font><br/>");
        builder.Append("题数:" + "<font color=\"red\">" + user.count + "</font><br/>");
        builder.Append("当前 " + "<font color=\"green\">" + (Convert.ToInt32(user.now) + 1) + "/" + user.count + "</font>" + " 请看题:");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">", "<form id=\"form1\" method=\"post\" action=\"questions.aspx\">"));
        builder.Append("问题:" + "<font color=\"red\">" + n.question + "</font>");
        builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + n.deficult + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"qId\" Value=\"" + qId + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"ok\"/>");
        builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + n.styleID + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + n.ID + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"contrlanswer\"/>");
        if (n.img.Length > 5)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        if (n.style == 1)//4选1
        {
            builder.Append("(选择4选1)" + "<br/>");
            builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            builder.Append("A: " + n.chooseA + " ");
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            builder.Append("B: " + n.chooseB);
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
            builder.Append("C: " + n.chooseC);
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "D" + "\"/>");
            builder.Append("D: " + n.chooseD + "");
        }
        else if (n.style == 2)//2选1
        {
            builder.Append("(选择2选1)" + "<br/>");
            builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            builder.Append("A: " + n.chooseA + " ");
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            builder.Append("B: " + n.chooseB);
        }
        else if (n.style == 3)//3选1
        {
            builder.Append("(选择3选1)" + "<br/>");
            builder.Append("<input type=\"radio\" name=\"chooseAnswer\"  style=\"border:0px;\" Value=\"" + "A" + "\"/>");
            builder.Append("A: " + n.chooseA + " ");
            builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            builder.Append("B: " + n.chooseB);
            builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
            builder.Append("C: " + n.chooseC);
        }
        else if (n.style == 4)//判断
        {
            builder.Append("(判断题)" + "<br/>");
            builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "1" + "\"/>");
            builder.Append("" + "正确" + " ");
            builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "0" + "\"/>");
            builder.Append(" " + "错误");
        }
        else if (n.style == 5)//填空
        {
            builder.Append("(填空题)" + "<br/>");
            builder.Append("请填入正确答案:<br/><input type=\"textbox\" name=\"chooseAnswer\" />");
        }
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append(Out.Tab("", ""));
        builder.Append((Out.SysUBB("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" height=\"30px\" Value=\"" + "确定选择" + "\"/> ")));
        builder.Append("<input type =\"reset\" class=\"btn-blue\"  name = \"b2\" value =\"清空\" />");
        builder.Append(Out.Tab("</form>", "</form>"));
        builder.Append(Out.Tab("</div>", ""));
        Bottom();
    }

    //contrlanswer 内线回答判断正误页面
    protected void ContrlAnswerPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("开始答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        //Utils.Error(Utils.GetRequest("qId", "all", 1, "", "1"), "");
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[0-9]\d*$", "1"));//一个问题的ID
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, "", "1"));//该用户控制器ID
        int ID1 = ID;
        int name = int.Parse(Utils.GetRequest("name", "all", 1, @"", "1"));
        int difcult = int.Parse(Utils.GetRequest("difcult", "all", 1, @"", "1"));
        string count = (Utils.GetRequest("count", "all", 1, @"", ""));
        string ace = (Utils.GetRequest("ace", "all", 1, @"", ""));
        string chooseAnswer = (Utils.GetRequest("chooseAnswer", "all", 1, @"", ""));
        if (chooseAnswer == "")
        { Utils.Error("答案不能为空！", ""); }
        if (!new BCW.BLL.tb_QuestionsList().Exists(ID))
        { Utils.Error("该问题不存在！", ""); }
        BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
        ID = Convert.ToInt32(ID);
        if (ace != "ok")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">");
            builder.Append("你答案:<font color=\"red\">  " + chooseAnswer + "</font>" + "" + "<br/>");
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"contrlanswer\"/>");
            builder.Append("<input type=\"hidden\" name=\"ace\" Value=\"ok\"/>");
            builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + name + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + ID + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"qId\" Value=\"" + qId + "\"/>");//控制器ID
            builder.Append("<input type=\"hidden\" name=\"chooseAnswer\" Value=\"" + chooseAnswer + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + difcult + "\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            builder.Append("</form>");
            builder.Append(Out.Tab("</div>", ""));
            Bottom();
        }
        else
        {
            //Utils.Error("" + cid + "", "");
            BCW.Model.tb_QuestionAnswer model = new BCW.Model.tb_QuestionAnswer();//问题回答列
            BCW.Model.tb_QuestionsAnswerCtr user = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtr(qId);//该用户的控制器
            if (("#" + user.answerList).Contains("#" + ID + "#"))
            {
                Utils.Error("本问题已作答," + "<a href=\"" + Utils.getUrl("questions.aspx?act=next&amp;qId=" + qId + "&amp;") + "\"><font color=\"greed\">" + "答下一题" + "<font></a>", "");
            }
            model.addtime = DateTime.Now;
            model.answer = chooseAnswer;
            model.getGold = 0;
            model.getType = "1";
            model.ident = user.ID;//属于他的控制器的标识 20161020启用
            model.isDone = 1;
            model.isGet = 1;
            model.isHit = 0;
            model.isOver = 1;
            model.needTime = 1;
            model.questID = ID;
            model.questtion = n.question;
            model.usid = meid;
            model.usname = new BCW.BLL.User().GetUsName(meid);
            if (chooseAnswer == n.answer)
            {
                //添加进数据库
                //判断是否有奖励
                //new BCW.BLL.tb_QuestionControl().UpdateIsTrue(model1.ID, Convert.ToInt32(model1.isTrue) + 1);
                model.isTrue = 1;
                user.trueCount += 1;
                n.trueAnswer += 1;
                builder.Append("恭喜答对了:<font color=\"red\">" + chooseAnswer + "</font><br/>");
            }
            else
            {
                n.falseAnswer += 1;
                user.flaseCount += 1;
                //new BCW.BLL.tb_QuestionControl().UpdateIsFlase(model1.ID, Convert.ToInt32(model1.isFalse) + 1);
                model.isTrue = 0;
                builder.Append("噢,答错啦,正确答案为:<font color=\"red\">" + n.answer + "co</font><br/>");
            }
            int qID = new BCW.BLL.tb_QuestionAnswer().Add(model);
            user.now += 1;
            user.answerList += ID.ToString() + "#";
            user.explain += qID.ToString() + "#";
            if (user.now == user.count)//答完了
            {
                string gessText = "";
                string usname = new BCW.BLL.User().GetUsName(meid);
                user.isDone = 0;//0答完 1未答完 2过期
                //提取总控制器获取金额
                BCW.Model.tb_QuestionController ctr1 = new BCW.BLL.tb_QuestionController().Gettb_QuestionController(Convert.ToInt32(user.contrID));//问题总控制器
                int sendGold = 0;
                if (Convert.ToDateTime(user.addtime).AddMinutes(Convert.ToInt32(user.isDone)) < DateTime.Now)
                {

                    if (ctr1.awardtype == 0)//全答了就有
                    {
                        if (ctr1.awardptype == 0)//0 固定奖金 1红包形式
                        {
                            sendGold = Convert.ToInt32(ctr1.award);
                        }
                        else if (ctr1.awardptype == 1)
                        {
                            //采用红包算法算出金额
                            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
                            int index = ran1.Next(1, Convert.ToInt32(ctr1.award) + 1);
                            sendGold = index;
                        }
                        else //其他奖励入如农场待添加
                        { sendGold = 0; }
                        gessText = "恭喜你在本轮" + "[URL=/bbs/game/questions.aspx?act=viewt&amp;qId=" + user.ID + "&amp;]问题大抽奖[/URL]中答对" + user.trueCount + "题,成功获得系统奖励" + sendGold + ub.Get("SiteBz") + ",快快去领取吧!";
                        new BCW.BLL.Guest().Add(0, meid, usname, gessText);

                    }
                    else if (ctr1.awardtype == 1)//全答对
                    {
                        if (user.trueCount == user.count)
                        {
                            if (ctr1.awardptype == 0)//0 固定奖金 1红包形式
                            {
                                sendGold = Convert.ToInt32(ctr1.award);
                            }
                            else if (ctr1.awardptype == 1)
                            {
                                //采用红包算法算出金额
                                Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
                                int index = ran1.Next(1, Convert.ToInt32(ctr1.award) + 1);
                                sendGold = index;
                            }
                            else//其他奖励入如农场待添加
                            { sendGold = 0; }
                            gessText = "恭喜你在本轮" + "[URL=/bbs/game/questions.aspx?act=viewt&amp;qId=" + user.ID + "&amp;]问题大抽奖[/URL]中答对" + user.trueCount + "题,成功获得系统奖励" + sendGold + ub.Get("SiteBz") + ",快快去领取吧!";
                            new BCW.BLL.Guest().Add(0, meid, usname, gessText);
                        }
                        else
                        {
                            user.ishit = 4;//答错无奖励
                        }
                    }
                }
                else
                {
                    if (ctr1.awardptype == 0)//0 固定奖金 1红包形式
                    {
                        sendGold = Convert.ToInt32(ctr1.award);
                    }
                    else if (ctr1.awardptype == 1)
                    {
                        //采用红包算法算出金额
                        Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
                        int index = ran1.Next(1, Convert.ToInt32(ctr1.award) + 1);
                        sendGold = index;
                    }
                    else//其他奖励入如农场待添加
                    { sendGold = 0; }
                    user.ishit = 2;//超时
                }
                user.awardgold = sendGold;
                //string gessText = "恭喜你在本轮" + "[URL=/bbs/game/questions.aspx?act=viewt&amp;qId="+user.ID+ "&amp;]问题大抽奖[/URL]中答对" + user.trueCount + "题,成功获得系统奖励" + sendGold + ub.Get("SiteBz") + ",快快去领取吧!";
                //new BCW.BLL.Guest().Add(0, meid, usname, gessText);
                user.overtime = DateTime.Now;//回答结束时间
                if (user.trueCount == user.count)
                {
                    ub xml = new ub();
                    string xmlPath = "/Controls/questions.xml";
                    Application.Remove(xmlPath);//清缓存
                    xml.ReloadSub(xmlPath); //加载配置
                    string QuestionsTopTwo = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + usname + "[/url]刚在第" + ctr1.ID + "轮[绿]全答对[/绿]啦!!";//[/红][br][绿]本轮提示:最大一等奖888888币,二等奖38888[/绿][/B][br]上轮得主:酷爆网18862在[url=/bbs/guess2/default.aspx]虚拟球类[/url]获得一等奖,价值[绿]888888[/绿]酷币[br]
                    xml.dss["QuestionsTopTwo"] = QuestionsTopTwo;//写入
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }

            }
            //判断过期。。。
            //更新该题目答对答错人次
            n.hit += 1;
            new BCW.BLL.tb_QuestionsList().Update(n);
            new BCW.BLL.tb_QuestionsAnswerCtr().Update(user);//更新控制器
            if (user.now == user.count)
            {
                Utils.Success("答题成功！", "本轮题目已答完,3s后跳转到答题结果..", Utils.getUrl("questions.aspx?act=sviewpt&amp;qId=" + user.ID + "&amp;"), "2");
            }
            else
            {
                Utils.Success("答题成功！", "答题成功,3s后跳转下一题..", Utils.getUrl("questions.aspx?act=next&amp;qId=" + qId + "&amp;"), "2");
            }
            //记录值
            //  new BCW.BLL.tb_QuestionControl().UpdateAnswerList(model1.ID, model1.answerList + qID.ToString() + "#");
            //    if (model1.qNow + 1 >= model1.qCount)
            //    {
            //        Utils.Success("题目已答完！", "题目已答完，正在返回本次游戏列表..", Utils.getUrl("questions.aspx?act=view&amp;ID=" + model1.ID + ""), "2");
            //    }
            //    else
            //        Utils.Success("答题成功！", "答题成功，正在继续进入答题游戏..", Utils.getUrl("questions.aspx?act=gamestart&amp;"), "2");
            //}
            //底部
            Bottom();
        }
    }

    //contrl 内线进入的首页
    protected void ContrlPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;答题有奖<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        int CII = 0;//用户回答记录控制器
        if (!new BCW.BLL.tb_QuestionController().Exists(qId))
        { Utils.Error("该问题不存在！", ""); }
        BCW.Model.tb_QuestionController model = new BCW.BLL.tb_QuestionController().Gettb_QuestionController(qId);//问题总控制器
        //指定id可答的类型问题
        //type0|在线会员|1|隐身会员|2|所有会员|3|指定会员
        if (model.type == 3)
        {
            if (!(("#" + model.uid).Contains("#" + meid.ToString() + "#")))
            {
                Utils.Error("不存在本轮回答的记录！", "");
            }
        }
        if (Convert.ToInt32(model.ycount) != 0)
        {
            if (Convert.ToInt32(model.wcount) > Convert.ToInt32(model.ycount))
            {
                Utils.Error("酷友们太热情啦,本次回答的人次已到达最大上限了...", "");
            }
        }
        //Utils.Error(Utils.GetRequest("qId", "all", 1, "", "1"), "");
        BCW.Model.tb_QuestionsAnswerCtr user = new BCW.Model.tb_QuestionsAnswerCtr();
        if (!new BCW.BLL.tb_QuestionsAnswerCtr().ExistsID(meid, qId))
        {
            user.addtime = DateTime.Now;
            user.overtime = DateTime.Now;
            user.answerList = "";
            user.awardgold = model.award;
            user.awardId = model.awardptype;
            user.awardtype = model.awardtype;
            user.contrID = qId;
            user.count = model.count;
            user.explain = "";
            user.flaseCount = 0;
            user.isDone = 1;
            user.ishit = 1;//是否领奖
            user.List = model.List;
            user.now = 0;
            user.trueCount = 0;
            user.uid = meid;
            qId = new BCW.BLL.tb_QuestionsAnswerCtr().Add(user);
            model.uided += meid.ToString() + "#";
            model.ycount += 1;
            new BCW.BLL.tb_QuestionController().Update(model);
        }
        else
        {
            user = new BCW.BLL.tb_QuestionsAnswerCtr().Gettb_QuestionsAnswerCtrByUidCid(meid, qId);
            qId = user.ID;
        }
        if (model.overtime < DateTime.Now)
        { Utils.Error("该问题已过期！", ""); }
        //  builder.Append("本轮题目共" + model.List + "个<br/>");
        string[] ques_id = model.List.Split('#');
        builder.Append("");
        //string[] index = model1.qList.Split('#');
        BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(Convert.ToInt32(ques_id[0]));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        string qtype = "";
        try
        { qtype = new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.styleID)); }
        catch { qtype = "随机题目"; }
        builder.Append("你选择了题目类型:" + "<font color=\"red\">" + qtype + "</font><br/>");
        builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font><br/>");
        //  builder.Append("题数:" + "<font color=\"red\">" + model.count + "</font><br/>");
        builder.Append("当前 " + "<font color=\"green\">" + 1 + "/" + model.count + "</font>" + " 请看题:");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">", "<form id=\"form1\" method=\"post\" action=\"questions.aspx\">"));
        builder.Append("问题:" + "<font color=\"red\">" + n.question + "</font>");
        builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + n.deficult + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"II\" Value=\"" + qId + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"qId\" Value=\"" + qId + "\"/>");//控制器id
        builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"ok\"/>");
        builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + n.styleID + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + n.ID + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"contrlanswer\"/>");

        if (n.img.Length > 5)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        if (n.style == 1)//4选1
        {
            builder.Append("(选择4选1)" + "<br/>");
            builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            builder.Append("A: " + n.chooseA + " ");
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            builder.Append("B: " + n.chooseB);
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
            builder.Append("C: " + n.chooseC);
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "D" + "\"/>");
            builder.Append("D: " + n.chooseD + "");
            //  builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            //  builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=start&amp;difcult=" + difcult + "&amp;name=" + name + "&amp;ac=ok&amp;chooseAnswer=D&amp;") + "\">" + " D: " + n.chooseD + "</a>");
        }
        else if (n.style == 2)//2选1
        {
            builder.Append("(选择2选1)" + "<br/>");
            builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            builder.Append("A: " + n.chooseA + " ");
            builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            builder.Append("B: " + n.chooseB);
        }
        else if (n.style == 3)//3选1
        {
            builder.Append("(选择3选1)" + "<br/>");
            builder.Append("<input type=\"radio\" name=\"chooseAnswer\"  style=\"border:0px;\" Value=\"" + "A" + "\"/>");
            builder.Append("A: " + n.chooseA + " ");
            builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            builder.Append("B: " + n.chooseB);
            builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
            builder.Append("C: " + n.chooseC);
        }
        else if (n.style == 4)//判断
        {
            builder.Append("(判断题)" + "<br/>");
            builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "1" + "\"/>");
            builder.Append("" + "正确" + " ");
            builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "0" + "\"/>");
            builder.Append(" " + "错误");
        }
        else if (n.style == 5)//填空
        {
            builder.Append("(填空题)" + "<br/>");
            builder.Append("请填入正确答案:<br/><input type=\"textbox\" name=\"chooseAnswer\" />");
        }
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append(Out.Tab("", ""));
        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
        builder.Append("<input type =\"reset\" class=\"btn-blue\"  name = \"b2\" value =\"清空\" />");
        builder.Append(Out.Tab("</form>", "</form>"));
        builder.Append(Out.Tab("</div>", ""));
        Bottom();
    }

    //超字符变省略号
    protected string strSub(string str)
    {
        int maxLength = 10;//限制最大字符数,如果str超出这个数字,则显示省略号
        str = str.Length > maxLength ? str.Substring(0, maxLength) + "..." : str;
        return str;
    }

    //all 最新答题
    protected void AllPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;最新挑战<br/>");
        //builder.Append(GameName);
        builder.Append(Out.Tab("</div>", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[^\^]{0,200}$", "0"));
        if (!new BCW.BLL.User().Exists(uid) && uid > 0)
        {
            Utils.Error("该会员ID不存在！", "");
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        if (uid > 0)
        { strWhere = " usid=" + uid; }
        string[] pageValUrl = { "act", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

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
                builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.usname + "</a>");
                builder.Append("在" + "<a href =\"" + Utils.getUrl("questions.aspx?act=alist&amp;ID=" + n.questID + "&amp;") + "\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(strSub(n.questtion))) + "</a>");
                if (n.isTrue == 1)
                { builder.Append("<font color =\"green\">" + "回答正确!" + "</font>"); }
                else
                { builder.Append("<font color =\"red\">" + "回答错误!" + "</font>"); }
                builder.Append("[" + Convert.ToDateTime(n.addtime).ToString("HH:mm") + "]");
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("=输入ID查找=");
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,,";
        string strName = "uid,ptype,act";
        string strType = "num,hidden,hidden";
        string strValu = uid + "'2'all";
        string strEmpt = "false,false";
        string strIdea = "";
        string strOthe = "查清单,questions.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        Bottom();
    }

    //alist 查看一个题目
    protected void aListPage()
    {
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("查看题目");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", "0"));//题目类型
        if (!new BCW.BLL.tb_QuestionsList().Exists(ID))
        {
            Utils.Error("不存在的问题记录！", "");
        }
        else
        {
            BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("[问题记录] " + "<font color=\"red\">" + n.ID + "</font><br/>");
            builder.Append("[问题阐述] " + "<font color=\"red\">" + n.question + "</font>");
            if (n.img.Length > 5)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", ""));
            }

            if (n.style == 1)//4选1
            {
                builder.Append("(选择4选1)" + "<br/>");
                builder.Append("[选项] ");
                builder.Append("A. " + n.chooseA);
                builder.Append(" B. " + n.chooseB);
                builder.Append(" C. " + n.chooseC);
                builder.Append(" D. " + n.chooseD);
            }
            else if (n.style == 2)//2选1
            {
                builder.Append("(选择2选1)" + "<br/>");
                builder.Append("[选项] ");
                builder.Append("A. " + n.chooseA);
                builder.Append(" B. " + n.chooseB);
            }
            else if (n.style == 3)//3选1
            {
                builder.Append("(选择3选1)" + "<br/>");
                builder.Append("[选项] ");
                builder.Append("A. " + n.chooseA);
                builder.Append(" B. " + n.chooseB);
                builder.Append(" C. " + n.chooseC);
            }
            else if (n.style == 4)//判断
            {
                builder.Append("(判断题)" + "<br/>");
                //if (n.answer.Contains("正确"))
                //{
                //    builder.Append("正确答案: " + "<font color=\"red\">" + "√" + "</font>");
                //}
                //else
                //{
                //   // builder.Append("正确答案: " + "<font color=\"red\">" + "×" + "</font>");
                //}
            }
            else if (n.style == 5)//填空
            {
                builder.Append("(填空题)" + "");
            }
            string qtype = "";
            try
            { qtype = new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.type)); }
            catch { qtype = "随机题目"; }
            builder.Append("<br/>题目类型: " + "<font color=\"red\">" + qtype + "</font>");
            builder.Append("<br/>难度: " + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font>");
            builder.Append("<br/>当前答对: " + "<font color=\"red\">" + n.trueAnswer + "</font>");
            builder.Append(" 当前答错: " + "<font color=\"red\">" + n.falseAnswer + "</font>");
            string jiexi = n.remarks;
            if (n.remarks == "")
            { jiexi = "暂无解析"; }
            builder.Append("<br/>问题解析: " + "<font color=\"red\">" + jiexi + "</font>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入问题ID:/,";
            string strName = "ID,backurl";
            string strType = "num,hidden";
            string strValu = ID + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜问题记录,questions.aspx?act=alist&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        Bottom();
    }

    #region 问答规则
    private void Rule()
    {
        Master.Title = GameName + "规则";
        string QuestionsRule = Convert.ToString(ub.GetSub("QuestionsRule", "/Controls/questions.xml"));
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;规则" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(QuestionsRule)));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region 新增自玩游戏暂不使用
    protected void GoodsPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;道具城<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【道具】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("");
        Bottom();
    }
    //choose 确认选择问题类型
    protected void ChooseTypePage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;选择题型");
        //builder.Append(GameName);
        builder.Append(Out.Tab("</div>", ""));
        string ac = (Utils.GetRequest("ac", "all", 1, @"", ""));
        if (ac == "ok")
        {
            DataSet ds = new BCW.BLL.tb_QuestionsType().GetList(" * ", " ID>0 ");
            string question_type = "";
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //0|游戏区奖池1|1|游戏区奖池2|2|游戏区奖池3|3|论坛奖池4|4|奖池5
                    question_type += ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Name"] + "|";
                }
                question_type = question_type.Substring(0, question_type.Length - 1);
            }
            if (question_type == "")
            {
                question_type = " 0|请先前往添加问题分类|1|请先前往添加问题分类";
                Utils.Error("问题正在初始化中...", "");
            }
            strText = "选择答题类型:/,难度级别:/,问题数:/,,,";
            strName = "name,difcult,count,act,backurl";
            strType = "select,select,select,hidden,hidden";
            strValu = "" + "'" + "'" + "'" + "start'" + Utils.getPage(0) + "";
            strEmpt = question_type + "," + question_deficult + ",5|5|10|10|20|20|30|30|40|40|50|50";
            strIdea = "/";
            strOthe = "开始答题,questions.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("<b>温馨提示:是否做好了答题准备?</b>" + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=choose&amp;ac=ok&amp;") + "\">开始答题</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=choose&amp;game=1&amp;") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", ""));

        }


        //底部
        Bottom();
    }
    //start 开始答题
    protected void StartPage()
    {
        builder.Append("<style type=\"text/css\">");
        builder.Append(".inputd{width:50px;color:red;}");
        builder.Append("</style>");
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("开始答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string ac = (Utils.GetRequest("ac", "all", 1, @"", ""));
        int name = int.Parse(Utils.GetRequest("name", "all", 1, @"", ""));//题目类型
        int difcult = int.Parse(Utils.GetRequest("difcult", "all", 1, @"", ""));//难度
        int count = Convert.ToInt32(Utils.GetRequest("count", "all", 1, @"", ""));//回答题目数量
        string chooseAnswer = (Utils.GetRequest("chooseAnswer", "all", 1, @"", ""));

        ///算出随机总数的题目
        ///。。。F:\kubao备份\20160602KUBAO\KUBAO\BCW.Question\Questions.cs
        ///
        BCW.Model.tb_QuestionControl model1 = new BCW.BLL.tb_QuestionControl().Gettb_QuestionControl(new BCW.BLL.tb_QuestionControl().GetIDForUsid(meid));
        if (model1 == null || model1.isDone == 0 && model1.qCount == model1.qNow)
        {
            BCW.Model.tb_QuestionControl model = new BCW.Model.tb_QuestionControl();
            model.qCount = Convert.ToInt32(count);
            model.qList = new BCW.Question.Questions().getQuestionsList(count, difcult, name);
            model.qNow = 0;
            model.usid = meid;
            model.usname = new BCW.BLL.User().GetUsName(meid);
            model.remark = "";
            model.overtime = DateTime.Now.AddMinutes(3);
            model.addtime = DateTime.Now;
            model.answerJudge = "";
            model.answerList = "";
            model.isDone = 1;
            model.isFalse = 0;
            model.isOver = 0;
            model.isTrue = 0;
            int id = new BCW.BLL.tb_QuestionControl().Add(model);
            Utils.Success("选择成功！", "选择成功，正在进入答题游戏..", Utils.getUrl("questions.aspx?act=gamestart&amp;"), "2");
            #region 注释的问题输出
            //DataSet ds = new BCW.BLL.tb_QuestionsList().GetList(" * ", " ");

            ////builder.Append(model.qList);
            //builder.Append(Out.Tab("<div class=\"text\">", ""));
            //builder.Append("你选择了题目类型:" + "<font color=\"red\">" + new BCW.BLL.tb_QuestionsType().GetName(name) + "</font><br/>");
            //builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[difcult] + "</font><br/>");
            //builder.Append("题数:" + "<font color=\"red\">" + count + "</font><br/>");
            //builder.Append("当前第" + 1 + "题,请看题:");
            //builder.Append(Out.Tab("</div>", ""));
            ////  builder.Append(Out.Tab("<div>", ""));

            //BCW.Model.tb_QuestionControl model1 = new BCW.BLL.tb_QuestionControl().Gettb_QuestionControl(new BCW.BLL.tb_QuestionControl().GetIDForUsid(meid));
            //string[] index = model1.qList.Split('#');
            //int ID = Convert.ToInt32(index[Convert.ToInt32(model1.qNow)]);
            //try
            //{
            //    BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
            //    builder.Append(Out.Tab("<div>", ""));
            //    builder.Append(Out.Tab("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">",""));
            //    builder.Append("问题:" + "<font color=\"red\">" + n.question + "</font>");

            //    if (n.img.Length > 5)
            //    {
            //        builder.Append(Out.Tab("<div>", ""));
            //        builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
            //        builder.Append(Out.Tab("</div>", ""));
            //    }
            //    if (n.style == 1)//4选1
            //    {

            //        builder.Append("(选择4选1)" + "<br/>");         
            //        builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + difcult + "\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"ok\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"ve\" Value=\"2a\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + name + "\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + n.ID + "\"/>");
            //        builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            //        builder.Append("A: " + n.chooseA + " ");
            //        builder.Append("  <input type=\"radio\"  style=\"border:0px;\"  name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            //        builder.Append("B: " + n.chooseB);
            //        builder.Append("  <input type=\"radio\"  name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
            //        builder.Append("C: " + n.chooseC);
            //        builder.Append("  <input type=\"radio\" name=\"chooseAnswer\" Value=\"" + "D" + "\"/>");
            //        builder.Append("D: " + n.chooseD + "");
            //        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"judge\"/>");
            //        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");

            //        //  builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=start&amp;difcult=" + difcult + "&amp;name=" + name + "&amp;ac=ok&amp;chooseAnswer=D&amp;") + "\">" + " D: " + n.chooseD + "</a>");
            //    }
            //    else if (n.style == 2)//2选1
            //    {
            //        builder.Append("(选择2选1)" + "<br/>");
            //        builder.Append("问:" + n.question + "<br/>");          
            //        builder.Append("<input type=\"hidden\" style=\"border:0px;\" name=\"difcult\" Value=\"" + difcult + "\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"ok\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"ve\" Value=\"2a\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + name + "\"/>");
            //        builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + n.ID + "\"/>");
            //        builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            //        builder.Append("A: " + n.chooseA + " ");
            //        builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            //        builder.Append("B: " + n.chooseB);
            //        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            //    }
            //    else if (n.style == 3)//3选1
            //    {
            //        builder.Append("(选择3选1)" + "<br/>");
            //        builder.Append("<input type=\"radio\" name=\"chooseAnswer\"  style=\"border:0px;\" Value=\"" + "A" + "\"/>");
            //        builder.Append("A: " + n.chooseA + " ");
            //        builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            //        builder.Append("B: " + n.chooseB);
            //        builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
            //        builder.Append("C: " + n.chooseC);
            //        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            //    }
            //    else if (n.style == 4)//判断
            //    {
            //        builder.Append("(判断题)" + "<br/>");
            //        builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
            //        builder.Append("" + "正确" + " ");
            //        builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
            //        builder.Append(" " +"错误");
            //        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            //    }
            //    else if (n.style == 5)//填空
            //    {
            //        builder.Append("(填空题)" + "<br/>");
            //        builder.Append("请填入正确答案:<br/><input type=\"textbox\" name=\"chooseAnswer\" />");
            //        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定提交" + "\"/><br/>");
            //    }
            //    if (chooseAnswer != "")
            //    {
            //        //builder.Append("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">");
            //        //builder.Append("你选择了答案:<font color=\"red\">  " + chooseAnswer + "</font><br/>");
            //        //builder.Append("<input type=\"hidden\" name=\"act\" Value=\"judge\"/>");
            //        //builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + name + "\"/>");
            //        //builder.Append("<input type=\"hidden\" name=\"chooseAnswer\" Value=\"" + chooseAnswer + "\"/>");
            //        //builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + difcult + "\"/>");
            //        //string VE = ConfigHelper.GetConfigString("VE");
            //        //string SID = ConfigHelper.GetConfigString("SID");
            //        //builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            //        //builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            //        //builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            //        //builder.Append("</form>");
            //        //string strText = "";
            //        //string strName = "choose";
            //        //string strType = "hidden,hidden";
            //        //string strValu = "'" + Utils.getPage(0) + "";
            //        //string strEmpt = "true,false";
            //        //string strIdea = "/";
            //        //string strOthe = "确定选择,questions.aspx?act=all&amp;,post,1,red";
            //        //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //    }


            //    string VE = ConfigHelper.GetConfigString("VE");
            //    string SID = ConfigHelper.GetConfigString("SID");
            //    builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            //    builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            //    builder.Append("<input type =\"reset\" class=\"btn-blue\"  name = \"b2\" value =\"清空\" />");
            //    builder.Append(Out.Tab("</form>",""));
            //    builder.Append(Out.Tab("</div>",""));
            //}
            //catch (Exception ee) { builder.Append(ee + "<br/>" + meid + "---" + "" + Convert.ToInt32(model1.qNow) + "*******" + ID); }
            #endregion
        }
        else
        {
            Utils.Success("选择失败！", "尚有问题未答完！，正在进入上一轮答题游戏..", Utils.getUrl("questions.aspx?act=gamestart&amp;"), "2");
        }
        Bottom();
    }
    //gamestart 正式开始答题
    protected void GameStartPage()
    {
        builder.Append("<style type=\"text/css\">");
        builder.Append(".inputd{width:50px;color:red;}");
        builder.Append("</style>");
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("开始答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //string ac = (Utils.GetRequest("ac", "all", 1, @"", ""));
        //int name = int.Parse(Utils.GetRequest("name", "all", 1, @"", ""));//题目类型
        //int difcult = int.Parse(Utils.GetRequest("difcult", "all", 1, @"", ""));//难度
        //int count = Convert.ToInt32(Utils.GetRequest("count", "all", 1, @"", ""));//回答题目数量
        //string chooseAnswer = (Utils.GetRequest("chooseAnswer", "all", 1, @"", ""));
        try
        {
            BCW.Model.tb_QuestionControl model1 = new BCW.BLL.tb_QuestionControl().Gettb_QuestionControl(new BCW.BLL.tb_QuestionControl().GetIDForUsid(meid));
            string[] index = model1.qList.Split('#');

            if (model1.qNow >= model1.qCount)
            {
                Utils.Error("当前无可答题目" + "<a href =\"" + Utils.getUrl("questions.aspx?act=view&amp;ID=" + model1.ID + "") + "\">" + "查看答题结果" + "</a>" + "", "");
            }
            int ID = Convert.ToInt32(index[Convert.ToInt32(model1.qNow)]);
            BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("你选择了题目类型:" + "<font color=\"red\">" + new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.styleID)) + "</font><br/>");
            builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font><br/>");
            builder.Append("题数:" + "<font color=\"red\">" + model1.qCount + "</font><br/>");
            builder.Append("当前第" + (model1.qNow + 1) + "题,请看题:");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">", "<form id=\"form1\" method=\"post\" action=\"questions.aspx\">"));
            builder.Append("问题:" + "<font color=\"red\">" + n.question + "</font>");
            builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + n.deficult + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"ok\"/>");
            //builder.Append("<input type=\"hidden\" name=\"ve\" Value=\"2a\"/>");
            builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + n.styleID + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + n.ID + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"judge\"/>");
            if (n.img.Length > 5)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (n.style == 1)//4选1
            {
                builder.Append("(选择4选1)" + "<br/>");
                builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
                builder.Append("A: " + n.chooseA + " ");
                builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
                builder.Append("B: " + n.chooseB);
                builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
                builder.Append("C: " + n.chooseC);
                builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "D" + "\"/>");
                builder.Append("D: " + n.chooseD + "");
                //  builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
                //  builder.Append("<a href=\"" + Utils.getUrl("questions.aspx?act=start&amp;difcult=" + difcult + "&amp;name=" + name + "&amp;ac=ok&amp;chooseAnswer=D&amp;") + "\">" + " D: " + n.chooseD + "</a>");
            }
            else if (n.style == 2)//2选1
            {
                builder.Append("(选择2选1)" + "<br/>");
                //builder.Append("问:" + n.question + "<br/>");
                builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "A" + "\"/>");
                builder.Append("A: " + n.chooseA + " ");
                builder.Append("  <input type=\"radio\" style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
                builder.Append("B: " + n.chooseB);
                //  builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            }
            else if (n.style == 3)//3选1
            {
                builder.Append("(选择3选1)" + "<br/>");
                builder.Append("<input type=\"radio\" name=\"chooseAnswer\"  style=\"border:0px;\" Value=\"" + "A" + "\"/>");
                builder.Append("A: " + n.chooseA + " ");
                builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "B" + "\"/>");
                builder.Append("B: " + n.chooseB);
                builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "C" + "\"/>");
                builder.Append("C: " + n.chooseC);
                //  builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            }
            else if (n.style == 4)//判断
            {
                builder.Append("(判断题)" + "<br/>");
                builder.Append("<input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "1" + "\"/>");
                builder.Append("" + "正确" + " ");
                builder.Append("  <input type=\"radio\"  style=\"border:0px;\" name=\"chooseAnswer\" Value=\"" + "0" + "\"/>");
                builder.Append(" " + "错误");
                //   builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
                /*builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>")*/
                ;
            }
            else if (n.style == 5)//填空
            {
                builder.Append("(填空题)" + "<br/>");
                builder.Append("请填入正确答案:<br/><input type=\"textbox\" name=\"chooseAnswer\" />");
                //  builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            }

            #region 旧版问题答案不为空
            //if (chooseAnswer != "")
            //{
            //    //builder.Append("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">");
            //    //builder.Append("你选择了答案:<font color=\"red\">  " + chooseAnswer + "</font><br/>");
            //    //builder.Append("<input type=\"hidden\" name=\"act\" Value=\"judge\"/>");
            //    //builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + name + "\"/>");
            //    //builder.Append("<input type=\"hidden\" name=\"chooseAnswer\" Value=\"" + chooseAnswer + "\"/>");
            //    //builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + difcult + "\"/>");
            //    //string VE = ConfigHelper.GetConfigString("VE");
            //    //string SID = ConfigHelper.GetConfigString("SID");
            //    //builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            //    //builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            //    //builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            //    //builder.Append("</form>");
            //    //string strText = "";
            //    //string strName = "choose";
            //    //string strType = "hidden,hidden";
            //    //string strValu = "'" + Utils.getPage(0) + "";
            //    //string strEmpt = "true,false";
            //    //string strIdea = "/";
            //    //string strOthe = "确定选择,questions.aspx?act=all&amp;,post,1,red";
            //    //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //}

            #endregion

            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("", ""));
            builder.Append("<br/><input class=\"btn-red\" type=\"submit\" name=\"name1\" Value=\"" + "确定选择" + "\"/><br/>");
            builder.Append("<input type =\"reset\" class=\"btn-blue\"  name = \"b2\" value =\"清空\" />");
            builder.Append(Out.Tab("</form>", "</form>"));

        }
        catch (Exception ee) { builder.Append(ee + "<br/>" + meid + "---" + "" + "*******" + ID); }
        Bottom();
    }
    //judge 对回答的判断
    protected void JudgePage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("开始答题");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        //Utils.Error("" + (Utils.GetRequest("ID", "all", 1, @"", "")) + "", "");
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"", ""));
        int ID1 = ID;
        int name = int.Parse(Utils.GetRequest("name", "all", 1, @"", ""));
        int difcult = int.Parse(Utils.GetRequest("difcult", "all", 1, @"", ""));
        string count = (Utils.GetRequest("count", "all", 1, @"", ""));
        string ace = (Utils.GetRequest("ace", "all", 1, @"", ""));
        string chooseAnswer = (Utils.GetRequest("chooseAnswer", "all", 1, @"", ""));
        if (chooseAnswer == "")
        { Utils.Error("答案不能为空！", ""); }
        //if (chooseAnswer == " ")
        //{ }
        BCW.Model.tb_QuestionControl model1 = new BCW.BLL.tb_QuestionControl().Gettb_QuestionControl(new BCW.BLL.tb_QuestionControl().GetIDForUsid(meid));
        BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
        if (model1.qNow >= model1.qCount)
        {
            Utils.Error("本次题目已答完！" + "<a href =\"" + Utils.getUrl("questions.aspx?act=view&amp;ID=" + model1.ID + "") + "\">" + "查看答题结果" + "</a>" + "", "");
        }
        string[] index = model1.qList.Split('#');
        // int ID1 = Convert.ToInt32(index[Convert.ToInt32(model1.qNow)]);
        ID = Convert.ToInt32(index[Convert.ToInt32(model1.qNow)]);
        // Utils.Error(""+ID1+"=="+ID+"","");
        if (ID1 != ID)
        {
            Utils.Error("" + model1.answerList + "--" + ID + "本条题目已作答,请勿重复回答！" + "<a href=\"" + Utils.getUrl("questions.aspx?act=gamestart&amp;") + "\">继续作答</a>" + "", "");
        }
        //  builder.Append("你选择了答案:<font color=\"red\">" + chooseAnswer + "</font><br/>");
        if (ace != "ok")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<form id=\"form1\" method=\"post\" action=\"questions.aspx\">");
            builder.Append("你答案:<font color=\"red\">  " + chooseAnswer + "</font>" + "" + "<br/>");
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"judge\"/>");
            builder.Append("<input type=\"hidden\" name=\"ace\" Value=\"ok\"/>");
            builder.Append("<input type=\"hidden\" name=\"name\" Value=\"" + name + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ID\" Value=\"" + ID + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"chooseAnswer\" Value=\"" + chooseAnswer + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"difcult\" Value=\"" + difcult + "\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            builder.Append("</form>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            BCW.Model.tb_QuestionAnswer model = new BCW.Model.tb_QuestionAnswer();
            model.addtime = DateTime.Now;
            model.answer = chooseAnswer;
            model.getGold = 0;
            model.getType = "1";
            model.ident = 1;
            model.isDone = 1;
            model.isGet = 1;
            model.isHit = 0;
            model.isOver = 1;
            model.needTime = 1;
            model.questID = ID;
            model.questtion = n.question;
            model.usid = meid;
            model.usname = new BCW.BLL.User().GetUsName(meid);
            if (chooseAnswer == n.answer)
            {
                //添加进数据库
                //判断是否有奖励
                new BCW.BLL.tb_QuestionControl().UpdateIsTrue(model1.ID, Convert.ToInt32(model1.isTrue) + 1);
                model.isTrue = 1;
                builder.Append("恭喜答对了:<font color=\"red\">" + chooseAnswer + "</font><br/>");
            }
            else
            {
                new BCW.BLL.tb_QuestionControl().UpdateIsFlase(model1.ID, Convert.ToInt32(model1.isFalse) + 1);
                model.isTrue = 0;
                builder.Append("噢,答错啦,正确答案为:<font color=\"red\">" + n.answer + "</font><br/>");
            }

            new BCW.BLL.tb_QuestionControl().UpdateqNow(model1.ID, Convert.ToInt32(model1.qNow) + 1);
            // new BCW.BLL.tb_QuestionControl().UpdateJudge(model1.ID, ID.ToString() + "#");
            new BCW.BLL.tb_QuestionControl().UpdateIsDone(model1.ID, 0);

            int qID = new BCW.BLL.tb_QuestionAnswer().Add(model);
            new BCW.BLL.tb_QuestionControl().UpdateAnswerList(model1.ID, model1.answerList + qID.ToString() + "#");
            if (model1.qNow + 1 >= model1.qCount)
            {
                Utils.Success("题目已答完！", "题目已答完，正在返回本次游戏列表..", Utils.getUrl("questions.aspx?act=view&amp;ID=" + model1.ID + ""), "2");
            }
            else
                Utils.Success("答题成功！", "答题成功，正在继续进入答题游戏..", Utils.getUrl("questions.aspx?act=gamestart&amp;"), "2");
        }
        //底部
        Bottom();
    }
    //view 答题结果
    protected void ViewPage()
    {
        Master.Title = GameName + "";
        string QuestionsRule = Convert.ToString(ub.GetSub("QuestionsRule", "/Controls/questions.xml"));
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("questions.aspx") + "\">" + GameName + "</a>&gt;答题结果" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));

        try
        {
            BCW.Model.tb_QuestionControl model1 = new BCW.BLL.tb_QuestionControl().Gettb_QuestionControl(new BCW.BLL.tb_QuestionControl().GetIDForUsid(meid));
            string[] index = model1.qList.Split('#');
            string[] index1 = model1.answerList.Split('#');
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            //   builder.Append("你选择了题目类型:" + "<font color=\"red\">" + new BCW.BLL.tb_QuestionsType().GetName(Convert.ToInt32(n.styleID)) + "</font><br/>");
            //  builder.Append("难度级别:" + "<font color=\"red\">" + question_deficult_list[Convert.ToInt32(n.deficult)] + "</font><br/>");
            builder.Append("题数:" + "<font color=\"red\">" + model1.qCount + "</font><br/>");
            builder.Append("正确数:<font color=\"red\">" + model1.isTrue + " </font>");
            builder.Append("错误数:<font color=\"red\">" + model1.isFalse + "</font>");
            double percent = Convert.ToDouble(model1.isTrue) / Convert.ToDouble(model1.qCount);
            if (model1.isFalse == 0)
            {
                builder.Append("正确率:<font color=\"red\">" + "100%</font>");
            }
            else
                builder.Append(" 正确率:<font color=\"red\">" + (percent * 100).ToString() + "%</font><br/>");
            builder.Append(Out.Tab("</div>", ""));
            for (int i = 0; i < model1.qCount; i++)
            {
                int ID = Convert.ToInt32(index[i]);
                int ID1 = Convert.ToInt32(index1[i]);
                //if (model1.qNow > model1.qCount)
                //{
                //    Utils.Error("当前无可答题目", "");
                //}
                BCW.Model.tb_QuestionsList n = new BCW.BLL.tb_QuestionsList().Gettb_QuestionsList(ID);
                BCW.Model.tb_QuestionAnswer n1 = new BCW.BLL.tb_QuestionAnswer().Gettb_QuestionAnswer(ID1);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(i + 1 + ".问题:" + n.question);
                if (n.img.Length > 5)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<img src=\"" + n.img + "\" alt=\"load\"/>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (n.style == 1)//4选1
                {
                    builder.Append("(选择4选1)" + "<br/>");
                    builder.Append("A:" + n.chooseA + " ");
                    builder.Append(" B:" + n.chooseB);
                    builder.Append(" C:" + n.chooseC);
                    builder.Append(" D:" + n.chooseD + "");
                }
                else if (n.style == 2)//2选1
                {
                    builder.Append("(选择2选1)" + "<br/>");
                    builder.Append("问:" + n.question + "<br/>");
                    builder.Append("A:" + n.chooseA + " ");
                    builder.Append(" B:" + n.chooseB);
                }
                else if (n.style == 3)//3选1
                {
                    builder.Append("(选择3选1)" + "<br/>");
                    builder.Append("A:" + n.chooseA + " ");
                    builder.Append(" B:" + n.chooseB);
                    builder.Append(" C:" + n.chooseC);
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

                builder.Append("<br/>你的答案:" + n1.answer);
                if (n1.isTrue == 1)
                {
                    builder.Append(" " + "(" + "<font color=\"green\">" + "正确" + "</font>" + ")");
                }
                else
                { builder.Append(" " + "(" + "<font color=\"red\">" + "错误" + "</font>" + ")"); }
                builder.Append("<br/>" + "正确答案:" + n.answer + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch (Exception ee)
        { builder.Append(ee + "<br/>" + meid + "---" + "" + "*******" + ID); }
        Bottom();
    }
    #endregion
}