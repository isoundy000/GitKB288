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
using BCW.JS;
using BCW.SWB;
using System.Threading;
using System.Text.RegularExpressions;
/// <summary>
/// 20160524_添加本页兑奖
/// 20160524_完善排行榜（总日周月榜）
/// 20160606_去除拾物入口版
/// 20160607_修复公告
/// 20160622_过期时间小时为单位，额度控制
/// 20160801 增加兑奖防刷
/// 20160801 分5个奖池
/// 20160820 界面修复
/// 20160906 增加随机抽奖显示语句 兑奖显示
/// </summary>
public partial class bbs_game_winners : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/winners.xml";
    protected int statue = Convert.ToInt32(ub.GetSub("WinnersStatus", "/Controls/winners.xml"));
    protected string GameName = Convert.ToString(ub.GetSub("WinnersName", "/Controls/winners.xml"));
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected static int GameId = 1018;
    protected void Page_Load(object sender, EventArgs e)
    {

        //0正常1维护2测试  
        if (ub.GetSub("WinnersStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule"://规则介绍
                Rule();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "lists"://个人好运历史
                ListsPage();
                break;
            case "paijiang"://paijiang
                PaiJiang();
                break;
            case "all"://全部历史
                AllListPage();
                break;
            case "toplist"://排行榜单
                TopListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    public static ArrayList GetRandomLisint(ArrayList inputList)
    {
        //Copy to a array
        int[] copyArray = new int[inputList.Count];
        inputList.CopyTo(copyArray);

        //Add range
        ArrayList copyList = new ArrayList();
        copyList.AddRange(copyArray);

        //Set outputList and random
        ArrayList outputList = new ArrayList();
        Random rd = new Random(DateTime.Now.Millisecond);

        while (copyList.Count > 0)
        {
            //Select an index and item
            int rdIndex = rd.Next(0, copyList.Count - 1);
            int remove = Convert.ToInt32(copyList[rdIndex]);

            //remove it from copyList and add it to output
            copyList.Remove(remove);
            outputList.Add(remove);
        }
        return outputList;
    }
    //主页
    private void ReloadPage()
    {
        try
        { //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(4));
        }
        catch { }
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append(GameName);
        builder.Append(Out.Tab("</div>", "<br/>"));
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[0-5]\d*$", "5"));
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();首页可看
        int ceshi = Convert.ToInt32(ub.GetSub("ceshi", xmlPath));
        bool check = false;
        if (statue == 0)// 正常0 维护1 测试为2
        {
            if (ceshi == 0)////0酷币版测试状态 1酷币版开放
            {
                string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
                string[] name = CeshiQualification.Split('#');
                // foreach (string n in imgNum) 
                for (int n = 0; n < name.Length - 1; n++)
                {
                    if (name[n].ToString().Trim() == meid.ToString())
                    {
                        check = true;
                    }
                }
                if (!check)//未有资格 Utils.Error("游戏内测中，您还不是内测会员", "");
                {
                    Utils.Error("很抱歉,您暂未有测试该游戏的权限", "");
                }
            }
        }
        //顶部ubb    
        string WinnersGonggao = ub.GetSub("WinnersGonggao", xmlPath);
        string WinnersNotes = ub.GetSub("WinnersNotes", xmlPath);
        if (WinnersGonggao != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(WinnersGonggao)));
            if (WinnersNotes != "")
            {
                builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(WinnersNotes)));
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        //顶部Logo
        string WinnersLogo = ub.GetSub("img", xmlPath);
        if (WinnersLogo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + WinnersLogo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
        builder.Append(Out.Tab("<div>", ""));
        int leiji = new BCW.BLL.tb_WinnersLists().GetMaxId();
        builder.Append("本站累计幸运" + "<a href=\"" + Utils.getUrl("winners.aspx?act=all") + "\">" + (leiji - 1) + "</a>" + "人次<br/>");

        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=rule") + "\">规则</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=lists") + "\">兑奖</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=toplist") + "\">排行</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=all") + "\">动态</a><a href=\"" + Utils.getUrl("winners.aspx?act=all") + "\"><img src=\"" + "http://kb288.cc/FILES/kubao1/new.gif" + "\" alt=\"load\"/></a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        #region 本轮奖池最新动态
        try
        {
            int AllAwardCount = Convert.ToInt32(ub.GetSub("AllAwardCount", xmlPath));
            int AllAwardCountNow = Convert.ToInt32(ub.GetSub("AllAwardCountNow", xmlPath));
            string text = "";
            if (game == 0)
            {
                text = "奖池一";
            }
            if (game == 1)
            {
                text = "奖池二";
            }
            if (game == 2)
            {
                text = "奖池三";
            }
            if (game == 3)
            {
                text = "奖池四";
            }
            if (game == 4)
            {
                text = "奖池五";
            }
            string[] yuanlai;//规定的等奖数处理提出处理   
            string[] dengjiangshu;//数据等奖数处理提出处理   
            string[] goldlist;
            BCW.Model.tb_WinnersAward mode;
            int sumall = 0;//总额度  
            int sum = 0;//当前剩余总额
            int temp1 = 0;
            int temp2 = 0;
            int sumoneall = 0;//一个奖池总额度
            int sunone = 0;//一个奖池当前剩余总额
            int sumtwoall = 0;//一个奖池总额度
            int suntwo = 0;//一个奖池当前剩余总额
            int sumthreeall = 0;//一个奖池总额度
            int sunthree = 0;//一个奖池当前剩余总额
            int sumfourall = 0;//一个奖池总额度
            int sunfour = 0;//一个奖池当前剩余总额
            int sumfiveall = 0;//一个奖池总额度
            int sunfive = 0;//一个奖池当前剩余总额
            for (int k = 0; k < 5; k++)
            {
                awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(k.ToString());
                mode = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列
                yuanlai = mode.winNowCount.Split('#');//规定的等奖数处理提出处理   
                dengjiangshu = mode.getWinNumber.Split('#');//数据等奖数处理提出处理   
                goldlist = mode.award.Split('#');//数据等奖数处理提出处理      
                for (int j = 0; j < dengjiangshu.Length - 1; j++)
                {
                    sumall += Convert.ToInt32(yuanlai[j]) * Convert.ToInt32(goldlist[j]);
                    sum += Convert.ToInt32(dengjiangshu[j]) * Convert.ToInt32(goldlist[j]);
                    temp1 += Convert.ToInt32(yuanlai[j]) * Convert.ToInt32(goldlist[j]);
                    temp2 += Convert.ToInt32(dengjiangshu[j]) * Convert.ToInt32(goldlist[j]);
                }
                if ((k) == 0) { sumoneall = temp1; sunone = temp2; }
                if ((k) == 1) { sumtwoall = temp1; suntwo = temp2; }
                if ((k) == 2) { sumthreeall = temp1; sunthree = temp2; }
                if ((k) == 3) { sumfourall = temp1; sunfour = temp2; }
                if ((k) == 4) { sumfiveall = temp1; sunfive = temp2; }
                //builder.Append("奖池"+ (k +1)+ ": " + sumoneall);
                //builder.Append(" 剩余: " + sunone + " ");
                //builder.Append("<br/>");
                temp2 = 0;
                temp1 = 0;
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("【奖池最新动态】");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<font color=\"red\"><b>总奖池:" + AllAwardCount);
            builder.Append(" 剩余: " + AllAwardCountNow + "</b></font>");
            //  builder.Append(" 已中: " + (AllAwardCount- AllAwardCountNow) + "</font>");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            if (game == 5)
                builder.Append("<b>本轮量</b>|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=5&amp;ptype=5&amp;") + "\"><b>本轮量</b></a>|");
            if (game == 0)
                builder.Append("<b>奖池一</b>|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=0&amp;ptype=5&amp;") + "\"><b>奖池一</b></a>|");

            if (game == 1)
                builder.Append("<b>奖池二</b><br/>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=1&amp;ptype=5&amp;") + "\"><b>奖池二</b></a><br/>");
            if (game == 2)
                builder.Append("<b>奖池三</b>|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=2&amp;ptype=5&amp;") + "\"><b>奖池三</b></a>|");
            if (game == 3)
                builder.Append("<b>奖池四</b>|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=3&amp;ptype=5&amp;") + "\"><b>奖池四</b></a>|");
            if (game == 4)
                builder.Append("<b>奖池五</b>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=4&amp;ptype=5&amp;") + "\"><b>奖池五</b></a>");
            builder.Append("<br/>");
            builder.Append(Out.Tab("</div>", ""));

            //  builder.Append("<br/>");


            if (game < 5)
            {
                builder.Append(Out.Tab("<div>", ""));
                awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(game.ToString());
                mode = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列
                yuanlai = mode.winNowCount.Split('#');//规定的等奖数处理提出处理   
                dengjiangshu = mode.getWinNumber.Split('#');//数据等奖数处理提出处理   
                goldlist = mode.award.Split('#');//数据等奖数处理提出处理   
                sumall = 0;
                sum = 0;
                for (int j = 0; j < yuanlai.Length - 1; j++)
                {
                    sumall += Convert.ToInt32(yuanlai[j]) * Convert.ToInt32(goldlist[j]);
                }
                for (int j = 0; j < dengjiangshu.Length - 1; j++)
                {
                    sum += Convert.ToInt32(dengjiangshu[j]) * Convert.ToInt32(goldlist[j]);
                }
                //  builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>" + text + "</b>" + "总额: <font color=\"red\">" + sumall + "</font> ");
                builder.Append("剩余: <font color=\"red\">" + sum + "</font><br/> ");
                //  builder.Append(Out.Tab("</div>", ""));
                for (int j = 0; j < dengjiangshu.Length - 1; j++)
                {
                    // sum += Convert.ToInt32(dengjiangshu[j]) * Convert.ToInt32(goldlist[j]);
                    if (dengjiangshu[j].Contains("-"))
                    {
                        dengjiangshu[j] = "0";
                    }
                    builder.Append("<b>" + getAwardID(Convert.ToInt32(j + 1)) + "</b>" + " 等奖剩余量: <font color=\"red\">" + dengjiangshu[j] + "</font>  份| ");
                    builder.Append("每份奖金:<font color=\"red\">" + goldlist[j] + "</font>");
                    if (j < (dengjiangshu.Length - 2))
                        builder.Append("<br/>");
                }
                builder.Append(Out.Tab("</div>", ""));

            }
            else
            {
                //    if (game == 5)
                //        builder.Append("<b>总奖池</b>|");
                //    else
                //        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=5&amp;ptype=5&amp;") + "\">总奖池</a>|");
                //    if (game == 0)
                //        builder.Append("<b>奖池一</b>|");
                //    else
                //        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=0&amp;ptype=5&amp;") + "\">奖池一</a>|");

                //    if (game == 1)
                //        builder.Append("<b>奖池二</b>|");
                //    else
                //        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=1&amp;ptype=5&amp;") + "\">奖池二</a>|");
                //    if (game == 2)
                //        builder.Append("<b>奖池三</b>|");
                //    else
                //        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=2&amp;ptype=5&amp;") + "\">奖池三</a>|");
                //    if (game == 3)
                //        builder.Append("<b>奖池四</b>|");
                //    else
                //        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=3&amp;ptype=5&amp;") + "\">奖池四</a>|");
                //    if (game == 4)
                //        builder.Append("<b>奖池五</b>");
                //    else
                //        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=4&amp;ptype=5&amp;") + "\">奖池五</a>");
                //    builder.Append("<br/>");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("总奖池本轮: <font color=\"red\">" + sumall + "</font>");
                builder.Append(" 剩余: <font color=\"red\">" + sum + " " + "</font> ");
                builder.Append("<br/>");
                builder.Append("奖池一本轮: <font color=\"red\">" + sumoneall + "</font> ");
                builder.Append("剩余: <font color=\"red\">" + sunone + "</font>  ");
                builder.Append("<br/>");
                builder.Append("奖池二本轮: <font color=\"red\">" + sumtwoall + "</font>  ");
                builder.Append("剩余:<font color=\"red\">" + suntwo + "</font>  ");
                builder.Append("<br/>");
                builder.Append("奖池三本轮: <font color=\"red\">" + sumthreeall + "</font>  ");
                builder.Append("剩余: <font color=\"red\">" + sunthree + "</font>  ");
                builder.Append("<br/>");
                builder.Append("奖池四本轮: <font color=\"red\">" + sumfourall + "</font>  ");
                builder.Append("剩余: <font color=\"red\">" + sunfour + "</font>  ");
                builder.Append("<br/>");
                builder.Append("奖池五本轮: <font color=\"red\">" + sumfiveall + "</font>  ");
                builder.Append("剩余: <font color=\"red\">" + sunfive + "</font>  ");
                builder.Append(Out.Tab("</div>", ""));


                #region 旧版
                //int awardId0 = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("0");
                //int awardId1 = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("1");
                //int awardId2 = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("2");
                //int awardId3 = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("3");
                //int awardId4 = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("4");
                //BCW.Model.tb_WinnersAward mode0 = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId0);//奖池列
                //BCW.Model.tb_WinnersAward mode1 = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId1);//奖池列
                //BCW.Model.tb_WinnersAward mode2 = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId2);//奖池列
                //BCW.Model.tb_WinnersAward mode3 = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId3);//奖池列
                //BCW.Model.tb_WinnersAward mode4 = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId4);//奖池列
                //string[] yuanlai = mode0.winNowCount.Split('#');//规定的等奖数处理提出处理   
                //string[] dengjiangshu = mode0.getWinNumber.Split('#');//数据等奖数处理提出处理   
                //string[] goldlist = mode0.award.Split('#');//数据等奖数处理提出处理 
                //int sumall = 0;//总额度  
                //int sum = 0;//当前剩余总额
                //for (int j = 0; j < yuanlai.Length - 1; j++)
                //{
                //    sumall += Convert.ToInt32(yuanlai[j]) * Convert.ToInt32(goldlist[j]);
                //}
                //for (int j = 0; j < dengjiangshu.Length - 1; j++)
                //{
                //    sum += Convert.ToInt32(dengjiangshu[j]) * Convert.ToInt32(goldlist[j]);
                //    if (dengjiangshu[j].Contains("-"))
                //    {
                //        dengjiangshu[j] = "0";
                //    }
                //    builder.Append("<b>" + (j + 1) + "</b>" + " 等奖剩余量: " + dengjiangshu[j] + "  份| ");
                //    builder.Append("每份奖金:" + goldlist[j]);
                //    builder.Append("<br/>");
                //}
                #endregion
            }
        }
        catch
        {
        }
        #endregion


        #region 中奖动态
        try
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("【最新动态】");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            long listid = new BCW.BLL.tb_WinnersLists().GetMaxId() - 1;
            for (int k = 0; k <= 5; k++)
            {
                try
                {
                    BCW.Model.tb_WinnersLists n = new BCW.BLL.tb_WinnersLists().Gettb_WinnersLists(listid);
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                    builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                    builder.Append("获得 " + "<font  color=\"red\">" + getAwardID(Convert.ToInt32(n.GetId)) + "</font>" + " 等奖");
                    builder.Append("<br/>");

                }
                catch { }
                listid--;
            }
            #region 旧分页
            //int pageIndex;
            //int recordCount;
            //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            //string strWhere = "";
            //string[] pageValUrl = { "act", "backurl" };
            //pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            //if (pageIndex == 0)
            //    pageIndex = 1;
            //// 开始读取列表
            //IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
            //if (listSSCpay.Count > 0)
            //{
            //    int k = 1;
            //    foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
            //    {
            //        if (k < 6)
            //        {
            //            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
            //            BCW.Model.Action action = new BCW.BLL.Action().GetAction(Convert.ToInt32(n.FromId));
            //            string games = getGameName(action.Notes);
            //            builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
            //            builder.Append("获得 " + "<font  color=\"red\">" + n.GetId + "</font>" + " 等奖");
            //            builder.Append("<br/>");
            //        }
            //        else
            //        { break; }
            //        k++;
            //    }
            //}
            #endregion
            builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=all") + "\">" + "更多动态>>" + "</a>");//<a href=\"" + Utils.getUrl("winners.aspx?act=all") + "\"><img src=\"" + "   http://kb288.com/Files/bbs/6002/act/2012/05/18104141290.gif" + "\" alt=\"load\"/></a>"
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        catch { }
        #endregion


        #region 好运top5
        try
        {
            BCW.Model.tb_WinnersAward mod = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列
            //if (mode.awardNowCount == 0 || !new BCW.BLL.tb_WinnersAward().Exists(1))//不存在数据直接生成
            //{
            //    AddAward();//生成奖池
            //}
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("【好运TOP 5】");
            builder.Append(Out.Tab("</div>", ""));
            int summod;
            string str = "ID>0 group by UserId order by count desc ";
            DataSet ds = new BCW.BLL.tb_WinnersLists().GetList("UserId,count(UserId)as count", str);
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
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"]));
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UserId"] + "") + "\">" + mename + "</a>");
                        builder.Append("(" + ds.Tables[0].Rows[i]["UserId"] + ")");
                        builder.Append("已幸运");
                        builder.Append(ds.Tables[0].Rows[i]["count"] + "次");
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
        catch { }
        #endregion

        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("【输入ID查找】");
        //builder.Append(Out.Tab("</div>", "<br/>"));

        //string strText = "输入ID查找/,";
        //string strName = "usid";
        //string strType = "text,hidden";
        //string strValu = "'" + Utils.getPage(0) + "";
        //string strEmpt = "true,false";
        //string strIdea = "/";
        //string strOthe = "查动态,winners.aspx?act=all&amp;,post,1,red";
        //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));



        // 闲聊显示
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(38, "winners.aspx", 5, 0)));
        builder.Append(Out.Tab("</div>", ""));
        //底部
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        //  builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    #region 获得等奖中文
    //获得等奖 一二三...
    private string getAwardID(int i)
    {
        switch (i)
        {
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
            default:
                return "一";
        }
    }
    #endregion

    #region 全部好运动态历史
    // 全部好运动态历史
    private void AllListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int ceshi = Convert.ToInt32(ub.GetSub("ceshi", xmlPath));
        if (meid == 0)
            Utils.Login();
        Master.Title = "最新动态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        long id = Convert.ToInt64(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));//对应的商品编号    
        int getID = Utils.ParseInt(Utils.GetRequest("getID", "all", 1, @"^[0-9]\d*$", "0"));//等奖数查找   
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>&gt;最新动态" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        if (usid != 0)
        {
            strWhere = "UserId=" + usid;
            if (getID != 0)
            {
                strWhere += " and getID=" + getID;
            }
        }
        else
        {
            if (getID != 0)
            {
                strWhere = " getID=" + getID;
            }
        }

        string[] pageValUrl = { "act", "backurl", "usid", "getID" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                //  builder.Append(DateStringFromNow(Convert.ToDateTime(n.AddTime)));
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                //  builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>");
                //  builder.Append("在" + "<a>" + n.Remarks + "</a>" + "中获得了" + n.GetId + "等奖," + "价值" + n.winGold + "币");
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                builder.Append("获得" + "<font  color=\"red\">" + getAwardID(Convert.ToInt32(n.GetId)) + "</font>" + "等奖,");
                if (n.GetId < 4)
                {
                    builder.Append("价值" + "<font  color=\"red\">" + "<b>" + n.winGold + "</b>" + "</font>");
                }
                else
                {
                    if (n.winGold > 1000)
                    {
                        builder.Append("价值" + "<font  color=\"red\">" + "<b>" + n.winGold + "</b>" + "</font>");
                    }
                    else
                    {
                        builder.Append("价值" + n.winGold + "");
                    }
                }
                if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                { builder.Append(ub.Get("SiteBz")); }
                else
                { builder.Append("活跃值"); }
                //  builder.Append("<a href=\"" + "\">" + "<img src=\"" + "/bbs/game/img/winners_img/hit.jpg" + "\"  width=\"5%\" height=\"5%\" alt=\"load\"/>" + "</a>");
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

        string strText = "=输入ID查找=/,=按等奖数查找=/,";
        string strName = "usid,getID";
        string strType = "text,select,hidden";
        string strValu = usid + "'" + getID + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,0|全部等奖|1|一等奖|2|二等奖|3|三等奖|4|四等奖|5|五等奖|6|六等奖|7|七等奖|8|八等奖|9|九等奖|10|十等奖,false";
        string strIdea = "/";
        string strOthe = "查清单,winners.aspx?act=all&amp;,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //底部
        //  string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 判断数据是否丢失，丢失则返回真
    private bool isLost()
    {

        int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
        int ActionId = new BCW.BLL.Action().GetMaxId();
        BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);
        string[] losenum = model.getUsId.Split(',');
        string[] getwinnumber = model.getWinNumber.Split('#');
        bool boo = false;
        int num = 0;
        for (int j = 0; j < getwinnumber.Length - 1; j++)//统计等奖number 的数据剩余
        {
            num += Convert.ToInt32(getwinnumber[j].ToString().Trim());
        }
        if (num != Convert.ToInt32(model.awardNowCount))
        {
            boo = true;
        }
        for (int i = 0; i < losenum.Length - 1; i++)
        {
            if (ActionId > Convert.ToInt32(losenum[i].ToString().Trim()))
            {
                boo = true;
            }
        }
        if ((losenum.Length - 1) != model.awardNowCount)
        { boo = true; }
        if (num != losenum.Length - 1)
        {
            boo = true;
        }
        return boo;
    }
    #endregion

    #region 好运活跃抽奖排行榜
    private void TopListPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>" + "&gt;" + "排行榜");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        string str = "ID>0 group by UserId order by count desc ";
        if (ptype == 1)
        {
            builder.Append("总 榜|");
            str = "ID>0 group by UserId order by count desc ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=1&amp;") + "\">总 榜</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("今 日|");
            str = "ID>0 and  Year(AddTime) = " + DateTime.Now.Year + "" + " and Month(AddTime) = " + DateTime.Now.Month + "and Day(AddTime) = " + (DateTime.Now.Day) + "group by UserId order by count desc ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=2&amp;") + "\">今 日</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("周 榜|");
            str = "ID>0 and  datediff(week,[AddTime],getdate())=0" + "group by UserId order by count desc ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=3&amp;") + "\">周 榜</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("月 榜");
            str = "ID>0 and  Year(AddTime) = " + (DateTime.Now.Year) + " and Month(AddTime) = " + (DateTime.Now.Month) + "group by UserId order by count desc ";

        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=4&amp;") + "\">月 榜</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        try
        {
            int sum;
            //string str = "ID>0 group by UserId order by count desc ";
            DataSet ds = new BCW.BLL.tb_WinnersLists().GetList("UserId,count(UserId)as count", str);
            sum = 9;
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string sOrder = "";
            string[] pageValUrl = { "act", "ptype", "id", "backurl" };
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
                    //if (sum < i)
                    //{ break; }
                    //else
                    {
                        builder.Append("<font  color=\"red\">" + "[TOP " + (koo + i + 1) + "]" + "</font>");
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"]));
                        builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"])) + "</a>" + "抽奖");
                        //  builder.Append("(" + ds.Tables[0].Rows[koo + i]["UserId"] + ")");
                        builder.Append("累计获得运气");
                        builder.Append(ds.Tables[0].Rows[koo + i]["count"] + "次");
                        // builder.Append("<br/>");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    //  builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append("暂无相关排行,快快去加入动态吧！");
            }
        }
        catch { builder.Append("暂无相关排行,快快去加入动态吧！"); }
        //底部
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的中奖历史
    private void ListsPage()
    {
        Master.Title = GameName;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ceshi = Convert.ToInt32(ub.GetSub("ceshi", xmlPath));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        string strWhere = "UserId= " + meid + "";
        if (start.Length > 1)
        {
            strWhere = "UserId= " + meid + "and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
        }
        else
        {
            start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>&gt;我的历史" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string arrId = "";
        // 开始读取列表
        IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                if (n.Ident == 1)
                {
                    arrId = arrId + " " + n.Id;
                }

                //  builder.Append(k + ".");
                //    builder.Append(DateStringFromNow(Convert.ToDateTime(n.AddTime)) + "您在" + "<a>" + n.Remarks + "</a>" + "中成功获得了" +"<font color=\"#FF0000\">"+ n.GetId +"</font>"+ "等奖");
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                builder.AppendFormat("{0}前{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                builder.Append("获得 " + "<font  color=\"red\">" + getAwardID(Convert.ToInt32(n.GetId)) + "</font>" + " 等奖,");
                if (n.GetId < 4)
                {
                    builder.Append("价值" + "<b>" + n.winGold + "</b>");
                }
                else
                {
                    builder.Append("价值" + n.winGold);
                }
                if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                { builder.Append(ub.Get("SiteBz")); }
                else
                { builder.Append("活跃值"); }
                if (n.Ident == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=paijiang&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[兑奖]</a>");
                }
                else
                    if (n.Ident == 0)
                    {
                        // builder.Append(".价值"+n.winGold+"币.");
                        builder.Append("[已兑奖]");
                    }
                    else
                    {
                        // builder.Append(".价值" + n.winGold + "币.");
                        builder.Append("[已过期]");
                    }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            //builder.Append(Out.wapform(strName, strValu, strOthe));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">本页兑奖</a>");
            //builder.Append(Out.Tab("</div>", ""));         
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无相关记录.");
            builder.Append(Out.Tab("</div>", ""));
        }
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,winners.aspx?act=caseok&amp;,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        string strText1 = "开始时间:,结束时间:,";
        string strName1 = "start,down,backurl";
        string strType1 = "text,text,hidden";
        string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,true，false";
        string strIdea1 = "/";
        string strOthe1 = "按时间搜索,winners.aspx?act=lists&amp;,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        //底部
        //  string GameName = Convert.ToString(ub.GetSub("KbygName", xmlPath));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 本页兑奖
    //本页兑奖 添加时间20160524
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
            if (new BCW.BLL.tb_WinnersLists().Exists(pid))
            {
                BCW.Model.tb_WinnersLists model = new BCW.BLL.tb_WinnersLists().Gettb_WinnersLists(pid);
                if (model.UserId == meid)
                {
                    if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(model.AddTime).AddHours((double)model.overTime)) > 0)
                    {
                        model.Ident = 2;
                        new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 2);//2过期
                        // builder.Append("该记录因过期已无法领取,预祝好运连连！");
                    }
                    else
                    {
                        if (model.Ident == 1)
                        {
                            //操作币
                            long win = Convert.ToInt64(model.winGold);
                            new BCW.BLL.tb_WinnersLists().UpdateIdent(pid, 0);
                            winMoney += win;
                            new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.UserId), win, "活跃抽奖本页兑奖ID" + model.awardId + "#编号" + pid + "");
                        }
                    }
                }
            }
            //else
            //{ builder.Append("id不存在"); }
        }
        if (winMoney == 0)
        {
            Utils.Success("兑奖", "本页已兑奖，请勿重复提交", Utils.getUrl("winners.aspx?act=lists"), "1");
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
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "<br/>" + Print_text, Utils.getUrl("winners.aspx?act=lists"), "10");
           // Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz"), Utils.getUrl("winners.aspx?act=lists"), "20");
        }
    }
    #endregion

    #region 获得活跃抽奖规则
    private void Rule()
    {
        Master.Title = GameName + "指南";
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>&gt;规则指南" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("规则：" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1、在论坛进行发帖,回帖,加精,推荐,设滚,设专,打赏者。" + "<br/>");
        builder.Append("2、在空间留言,发言,上传照片,购买VIP,购买靓号,购买礼物,赠送礼物,签到者。" + "<br/>");
        builder.Append("3、在群聊,闲聊发言,以及发红包者,会员发喇叭,结婚,花园种花者。" + "<br/>");
        builder.Append("4、新会员注册升级,推荐注册成功者,进行加好友发内线者。" + "<br/>");
        builder.Append("5、在书城上活跃的如存书节,发书评者。" + "<br/>");
        builder.Append("6、所有游戏中下注,兑奖,互动者。" + "<br/>");
        builder.Append("7、以上参与者触发好运后,将有一定的几率获得奖励,可进入到" + "<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>" + "中获取对应的奖品。" + "<br/>");
        //   builder.Append("7、每人每天最大允许触发N次，节假日不封顶。" + "<br/>");     
        #region 更新奖池数据
        try
        {
            int MaxCount = Convert.ToInt32(ub.GetSub("MaxCount", xmlPath));//设置下一轮奖池最大份数
            int paisong = Convert.ToInt32(ub.GetSub("paisong", xmlPath));//设置下一轮奖池派送份数
            int CountList = Convert.ToInt32(ub.GetSub("CountList", xmlPath));//设置等奖数
            int baifenbi = Convert.ToInt32(ub.GetSub("baifenbi", xmlPath));//玩家中奖百分比
            string CountListNumber = (ub.GetSub("CountListNumber", xmlPath)) + "#";//设置每等奖份数
            string ListIGold = (ub.GetSub("ListIGold", xmlPath)) + "#";//设置每等奖金额
            int WinnersOpenChoose1 = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", xmlPath));//开放选择（int）
            int WinnersOpenOrClose1 = Convert.ToInt32(ub.GetSub("WinnersOpenOrClose", xmlPath));//开放选择
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            int PassTime = Convert.ToInt32(ub.GetSub("PassTime", xmlPath));//设置领取过期时间(天)
            int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
            BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列  
            if (isLost() && model.awardNowCount != 0)
            {
                //  int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
                Random ran = new Random();
                //更新当前的奖数范围
                int awardNumber = (Convert.ToInt32(model.periods) - Convert.ToInt32(model.awardNumber) + Convert.ToInt32(model.awardNowCount)); ;
                int NowNumber = Convert.ToInt32(model.awardNowCount);//此次派出剩余数量
                int nowId = new BCW.BLL.Action().GetMaxId() - 1;//当前ACTion最大ID
                string getUsId = "";//生成的中奖ID
                ArrayList lists = new ArrayList();
                for (int k = 1; k <= MaxCount; k++)
                {
                    lists.Add(k);
                }
                int r = 0;
                int num = 0;
                for (int i = 1; i <= NowNumber; )
                {
                    r = ran.Next(0, awardNumber);
                    num = Convert.ToInt32(lists[r]) + nowId;
                    getUsId += num.ToString();
                    getUsId += ",";
                    i++;
                    lists.Remove(lists[r]);
                    awardNumber--;
                }
                model.getUsId = getUsId;
                model.isGet = nowId;
                new BCW.BLL.tb_WinnersAward().Update(model);//更新中奖标识
            }
        }
        catch
        {
        }
        #endregion
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region 获取该Action记录的归属(属于哪个操作)
    public string getGameName(string notes)
    {
        string str = "";
        if (notes.Contains("抽奖活动"))
        { str = "抽奖活动"; }
        if (notes.Contains("空间签到"))
        { str = "空间签到"; }
        if (notes.Contains("回复帖子"))
        { str = "回复帖子"; }
        if (notes.Contains("发表帖子"))
        { str = "发表帖子"; }
        if (notes.Contains("获得"))
        { str = "获得"; }
        if (notes.Contains("发表了文件帖子"))
        { str = "发表了文件帖子"; }
        if (notes.Contains("发表了文件帖子"))
        { str = "发表了文件帖子"; }
        return str;
    }
    #endregion

    #region 判断记录是否属于游戏区
    public string getTypesForGameName(string notes)
    {
        // protected string[] sName = { "闲聊", "多人剪刀", "Ktv789", "猜猜乐", "欢乐竞拍", "竞猜", "幸运28", "虚拟投注", "疯狂彩球", "挖宝", "跑马", "上证", "", "大小庄", "吹牛", "", "猜拳", "苹果机", "掷骰", "拾物", "水果", "直播", "时时彩", "竞猜", "好彩一","百花谷" };

        string str = "";
        if (notes.Contains("多人剪刀"))
        { str = "多人剪刀"; }
        if (notes.Contains("Ktv789"))
        { str = "Ktv789"; }
        if (notes.Contains("猜猜乐"))
        { str = "猜猜乐"; }
        if (notes.Contains("欢乐竞拍"))
        { str = "欢乐竞拍"; }
        if (notes.Contains("竞猜"))
        { str = "竞猜"; }
        if (notes.Contains("幸运28"))
        { str = "幸运28"; }
        if (notes.Contains("虚拟投注"))
        { str = "虚拟投注"; }
        if (notes.Contains("疯狂彩球"))
        { str = "疯狂彩球"; }
        if (notes.Contains("挖宝"))
        { str = "挖宝"; }
        if (notes.Contains("跑马"))
        { str = "跑马"; }
        if (notes.Contains("上证"))
        { str = "上证"; }
        if (notes.Contains("大小庄"))
        { str = "大小庄"; }
        if (notes.Contains("吹牛"))
        { str = "吹牛"; }
        if (notes.Contains("猜拳"))
        { str = "猜拳"; }
        if (notes.Contains("苹果机"))
        { str = "苹果机"; }
        if (notes.Contains("掷骰"))
        { str = "掷骰"; }
        if (notes.Contains("拾物"))
        { str = "拾物"; }
        if (notes.Contains("水果"))
        { str = "水果"; }
        if (notes.Contains("直播"))
        { str = "直播"; }
        if (notes.Contains("时时彩"))
        { str = "时时彩"; }
        if (notes.Contains("拾物"))
        { str = "拾物"; }
        if (notes.Contains("好彩一"))
        { str = "好彩一"; }
        if (notes.Contains("百花谷"))
        { str = "百花谷"; }
        if (notes.Contains("新快3"))
        { str = "新快3"; }
        if (notes.Contains("球彩"))
        { str = "球彩"; }
        if (notes.Contains("捕鱼"))
        { str = "捕鱼游戏"; }
        if (notes.Contains("快乐十分"))
        { str = "快乐十分"; }
        if (notes.Contains("大小掷骰"))
        { str = "大小掷骰"; }
        if (notes.Contains("闯荡人生"))
        { str = "闯荡人生"; }
        if (notes.Contains("德州扑克"))
        { str = "德州扑克"; }
        if (notes.Contains("快乐扑克3"))
        { str = "快乐扑克3"; }
        if (notes.Contains("云购"))
        { str = "每日云购"; }
        if (notes.Contains("农场"))
        { str = "农场"; }
        return str;
    }
    #endregion

    #region 获取中奖时间
    public string DateStringFromNow(DateTime dt)
    {
        TimeSpan span = DateTime.Now - dt;
        if (span.TotalDays > 60)
        {
            return dt.ToShortDateString();
        }
        else
            if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else
                if (span.TotalDays > 14)
                {
                    return "2周前";
                }
                else
                    if (span.TotalDays > 7)
                    {
                        return "1周前";
                    }
                    else
                        if (span.TotalDays > 1)
                        {
                            return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                        }
                        else
                            if (span.TotalHours > 1)
                            {
                                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                            }
                            else
                                if (span.TotalMinutes > 1)
                                {
                                    return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                }
                                else
                                    if (span.TotalSeconds >= 1)
                                    {
                                        return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                    }
                                    else
                                    {
                                        return "1秒前";
                                    }
    }
    #endregion

    #region 派奖给中奖人员
    private void PaiJiang()
    {
        Master.Title = "兑奖中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("兑奖中心");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div>", "<br/>"));
        BCW.Model.tb_WinnersLists model = new BCW.BLL.tb_WinnersLists().Gettb_WinnersLists(ptype);
        //if (WinnersStatus == 1)//ceshi 不为0执行
        //{
        //if (ub.GetSub("WinnersStatus", xmlPath) == "2")//0|正常|1|维护|2|测试
        //{
        //    string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
        //    string[] name = CeshiQualification.Split('#');
        //    // foreach (string n in imgNum)
        //    bool check = false;
        //    for (int n = 0; n < name.Length - 1; n++)
        //    {
        //        if (name[n].ToString() == meid.ToString())
        //        {
        //            check = true;
        //        }
        //    }
        //    if (!check)//未有资格
        //    {
        //        Utils.Error("您暂未有测试该游戏的权限.", "");
        //    }
        //}
        //}
        if (model.Id == ptype && meid == model.UserId)
        {
            string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId));
            builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(model.AddTime)), model.UserId, mename, Out.SysUBB(model.Remarks));
            builder.Append("中触发了" + GameName + ",天神赐福" + "<font color=\"#FF0000\">" + getAwardID(Convert.ToInt32(model.GetId)) + "</font>" + "等奖！" + "<br/>");
            builder.Append("奖金价值 " + "<b>" + model.winGold + "</b>");
            if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
            { builder.Append(ub.Get("SiteBz") + "<br/>"); }
            else
            { builder.Append(" 活跃值" + "<br/>"); }
            DateTime nowtime = DateTime.Now;
            if (DateTime.Compare(nowtime, Convert.ToDateTime(model.AddTime).AddHours((double)model.overTime)) > 0)
            {
                model.Ident = 2;
                new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 2);//2过期
                builder.Append("该记录因过期已无法领取,预祝好运连连！");
            }
            else
            {            //builder.Append(model.Id + "," + ptype);            
                if (model.Ident == 0)
                { builder.Append("该记录已成功领取,预祝好运连连！"); }
                else if ((model.Ident == 2))
                { builder.Append("该记录因过期已无法领取,预祝好运连连！"); }
                else
                {
                    //增加随机抽奖语句 兑奖显示
                    string OutText = Convert.ToString(ub.GetSub("OutText", "/Controls/winners.xml"));
                    if (OutText != "")
                    {
                        string[] getText = OutText.Split('#');
                        Random index_n = new Random();
                        string Print_text = getText[index_n.Next(0, getText.Length)];
                        builder.Append("<font color=\"#FF0000\">" + Print_text + "</font>" + "<br/>");
                    }
                    long gold = Convert.ToInt64(model.winGold);
                    if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                    {
                        try
                        {
                            string usname=new BCW.BLL.User().GetUsName(meid);
                            BCW.User.Users.IsFresh("winners", 1);//防刷
                            new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 0);//0已领 活跃抽奖奖池ID11#编号287|操作500
                            new BCW.BLL.User().UpdateiGold(meid, gold, "活跃抽奖奖池ID" + model.awardId + "#编号" + model.Id);
                            string ActionOpen = Convert.ToString(ub.GetSub("ActionOpen", "/Controls/winners.xml"));//1开启动态，2关闭动态
                            if (ActionOpen == "1")
                            {
                                new BCW.BLL.Action().Add(GameId,ptype ,meid,usname,"在[URL=/bbs/game/winners.aspx]" + GameName + "[/URL]" + "成功兑换" + model.GetId + "等奖");
                            }
                            builder.Append("兑奖成功！预祝好运连连！");
                        }
                        catch { builder.Append("兑奖失败！请重新兑奖！"); }
                    }
                    else if (ub.GetSub("WinnersStatus", xmlPath) == "2")//测试
                    {
                        string usname = new BCW.BLL.User().GetUsName(meid);
                        if (!new BCW.SWB.BLL().ExistsUserID(meid, GameId))//不存在用户记录直接领
                        {
                            //  BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
                            BCW.SWB.Model swbs = new BCW.SWB.Model();
                            swbs.UserID = meid;
                            swbs.UpdateTime = DateTime.Now;
                            swbs.Money = 0 + gold;
                            swbs.GameID = GameId;
                            swbs.Permission = 1;
                            try
                            {
                                BCW.User.Users.IsFresh("winners", 1);//防刷
                                new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 0);//0已领
                                int id = new BCW.SWB.BLL().Add(swbs);
                                string ActionOpen = Convert.ToString(ub.GetSub("ActionOpen", "/Controls/winners.xml"));//1开启动态，2关闭动态
                                if (ActionOpen == "1")
                                {
                                    new BCW.BLL.Action().Add(GameId, ptype, meid, usname, "在[URL=/bbs/game/winners.aspx]" + GameName + "[/URL]" + "幸运测试成功兑换" + model.GetId + "等奖" + gold + "币");
                                }
                                builder.Append("兑奖成功！预祝好运连连！");
                            }
                            catch { builder.Append("兑奖失败！请重新兑奖！"); }
                        }
                        else
                        {
                            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, GameId);
                            builder.Append("当前测试币：" + swb.Money + "<BR/>");
                            swb.UpdateTime = DateTime.Now;
                            swb.Money += gold;
                            swb.Permission += 1;
                            builder.Append("领取后测试币：" + swb.Money + "<BR/>");
                            try
                            {
                                BCW.User.Users.IsFresh("winners", 1);//防刷
                                new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 0);//0已领
                                string ActionOpen = Convert.ToString(ub.GetSub("ActionOpen", "/Controls/winners.xml"));//1开启动态，2关闭动态
                                if (ActionOpen == "1")
                                {
                                    new BCW.BLL.Action().Add(GameId, ptype, meid, usname,"在[URL=/bbs/game/winners.aspx]" + GameName + "[/URL]" + "幸运" + "测试成功兑换" + model.GetId + "等奖" + gold + "币");
                                }
                                new BCW.SWB.BLL().Update(swb);
                                builder.Append("兑奖成功！预祝好运连连！");
                            }
                            catch { builder.Append("兑奖失败！请重新兑奖！"); }
                        }
                    }
                    else//维护
                    { Utils.Safe("此游戏"); }
                }
            }
        }
        else
        {
            builder.Append(GameName + "记录不存在，兑奖失败！");
        }
        builder.Append("<br/><a href=\"" + Utils.getUrl("winners.aspx?act=lists&amp;backurl=" + Utils.getPage(0) + "") + "\">返回继续兑奖</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 生成新一轮奖池数据
    private void AddAward()
    {
        /// <summary>
        /// 生成最新一期奖池数据
        /// </summary>
        /// 
        try
        {
            //设置下一轮奖池最大份数 
            int MaxCount = Convert.ToInt32(ub.GetSub("MaxCount", xmlPath));
            //设置下一轮奖池派送份数
            int paisong = Convert.ToInt32(ub.GetSub("paisong", xmlPath));
            //设置等奖数
            int CountList = Convert.ToInt32(ub.GetSub("CountList", xmlPath));
            //玩家中奖百分比
            int baifenbi = Convert.ToInt32(ub.GetSub("baifenbi", xmlPath));
            //设置每等奖份数
            string CountListNumber = (ub.GetSub("CountListNumber", xmlPath)) + "#";
            //设置每等奖金额
            string ListIGold = (ub.GetSub("ListIGold", xmlPath)) + "#";
            //开放选择（int）
            int WinnersOpenChoose = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", xmlPath));
            //设置内线提示的文字
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));
            //设置领取过期时间(天)
            int PassTime = Convert.ToInt32(ub.GetSub("PassTime", xmlPath));
            //当前ACTion最大ID
            int nowId = new BCW.BLL.Action().GetMaxId() - 1;
            Random ran = new Random();
            string getUsId = "";//生成的中奖ID
            int awardNumber1 = paisong;
            if (!new BCW.BLL.tb_WinnersAward().Exists(1))//不存在数据直接生成
            {
                ArrayList lists = new ArrayList();
                for (int k = 1; k <= MaxCount; k++)//
                {
                    lists.Add(k);

                }
                int r = 0;
                int num = 0;
                for (int i = 1; i <= paisong; )
                {
                    r = ran.Next(1, awardNumber1);
                    num = Convert.ToInt32(lists[r]) + nowId;
                    getUsId += num.ToString();
                    getUsId += ",";
                    i++;
                    lists.Remove(lists[r]);
                    awardNumber1--;
                }
                //   builder.Append("不存在数据直接生成");
                BCW.Model.tb_WinnersAward tempAward = new BCW.Model.tb_WinnersAward();//奖池列
                tempAward.periods = MaxCount;//奖池总数
                tempAward.addTime = DateTime.Now;//开始时间
                tempAward.awardNumber = paisong;//最大派出量
                tempAward.awardNowCount = paisong;//当前派出剩余量
                tempAward.award = ListIGold;//设置每等奖份数  100#50#20...数量
                tempAward.getRedy = "0";//获奖者
                tempAward.getUsId = getUsId;//获奖者，派送后递减ID
                tempAward.getWinNumber = CountListNumber;//设置每等奖份数  数量,便于统计剩余等奖量 统计剩余用
                tempAward.identification = PassTime;//过期标识（单位小时）
                tempAward.isDone = 0;//
                tempAward.isGet = nowId;//当前ACTion最大ID
                tempAward.overTime = DateTime.Now;
                tempAward.Remarks = TextForUbb;//发内线的语句
                tempAward.winNowCount = CountListNumber;//每等奖份数 不变
                tempAward.winNumber = CountList;//等奖份数，5即5等奖  
                int a = new BCW.BLL.tb_WinnersAward().Add(tempAward);//生成的新的中奖码保存
            }
            if (new BCW.BLL.tb_WinnersAward().Exists(1))//存在奖池数据
            {
                int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
                BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列   
                //   if (model.awardNowCount == 0 || awardId == 0 || sum==0)//上一期派完增加新一期
                //   if (sum == 0)
                {
                    int awardNumber = MaxCount;//总奖数
                    int NowNumber = paisong;//此次派出数量paisong
                    ArrayList lists = new ArrayList();
                    ArrayList yuanlaide = new ArrayList();
                    for (int k = 1; k <= MaxCount; k++)//
                    {
                        lists.Add(k);

                    }
                    //long start = System.cu();
                    int r = 0;
                    int num = 0;
                    for (int i = 1; i <= NowNumber; )
                    {
                        r = ran.Next(1, awardNumber);
                        num = Convert.ToInt32(lists[r]) + nowId;
                        getUsId += num.ToString();
                        getUsId += ",";
                        i++;
                        lists.Remove(lists[r]);
                        awardNumber--;
                    }
                    // long start = System.currentTimeMillis();
                    BCW.Model.tb_WinnersAward tempAward = new BCW.Model.tb_WinnersAward();//奖池列
                    tempAward.periods = MaxCount;//奖池总数
                    tempAward.addTime = DateTime.Now;//开始时间
                    tempAward.awardNumber = paisong;//最大派出量
                    tempAward.awardNowCount = paisong;//当前派出剩余量
                    tempAward.award = ListIGold;//设置每等奖份数  100#50#20...数量
                    tempAward.getRedy = "0";//获奖者
                    tempAward.getUsId = getUsId;//获奖者，派送后递减ID
                    tempAward.getWinNumber = CountListNumber;//设置每等奖份数  数量,便于统计剩余等奖量 统计剩余用
                    tempAward.identification = PassTime;//过期标识（单位天）
                    tempAward.isDone = 0;//
                    tempAward.isGet = nowId;//当前ACTion最大ID
                    tempAward.overTime = DateTime.Now;
                    tempAward.Remarks = TextForUbb;//发内线的语句
                    tempAward.winNowCount = CountListNumber;//每等奖份数 不变
                    tempAward.winNumber = CountList;//等奖份数，5即5等奖  
                    try
                    {
                        int a = new BCW.BLL.tb_WinnersAward().Add(tempAward);//生成的新的中奖码保存 
                        lists.Clear();
                        string gessText = "下一轮生成奖池数据成功，当前奖池ID" + a;
                        new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);
                    }
                    catch
                    {
                        string gessText = "下一轮生成奖池数据失败！002";
                        new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);
                    }
                }
                //else//未派完重新生成获奖ID更新奖池(仅修改中奖ID，其他不变)
                //{
                //    int awardNumber = MaxCount;//总奖数
                //    int NowNumber = Convert.ToInt32(model.awardNowCount);//此次派出剩余数量
                //    for (int i = 1; i <= NowNumber; )
                //    {
                //        int r = ran.Next(1, awardNumber);
                //        if (isGet(r.ToString(), getUsId))//不存在执行
                //        {
                //            int num = r + nowId;
                //            getUsId += num.ToString();
                //            getUsId += ",";
                //            i++;
                //        }
                //    }
                //    model.getUsId = getUsId;
                //    model.isGet = nowId;
                //    new BCW.BLL.tb_WinnersAward().Update(model);
                //}
            }

        }
        catch (Exception ee)
        {
            builder.Append(ee);
            string gessText = "新生成奖池数据失败！001";
            //  new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);//异常报错
        }
    }
    #endregion

    #region 获取对应的等奖金额
    protected int getGetWinNowCount(int i, int ID)
    {
        BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
        string[] getWinN = award.award.Split('#');//数据等奖数处理提出处理
        int get = 0;
        for (int k = 0; k < getWinN.Length - 1; k++)
        {
            if (k == (i - 1))//识别位置-1操作
            {
                get = (Convert.ToInt32(getWinN[k]));
            }
        }
        return get;
        // new BCW.BLL.tb_WinnersAward().Update(award);

    }
    #endregion

    #region 获取对应的等奖剩余数量，如该等奖已派完则返回false
    protected bool getGetWinNumber(int i, int ID)
    {
        BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
        string[] getWinN = award.getWinNumber.Split('#');//数据等奖数处理提出处理
        // int get = 0;
        bool boo = true;
        for (int k = 0; k < getWinN.Length - 1; k++)
        {
            if (k == (i - 1))//识别位置-1操作
            {
                //get = (Convert.ToInt32(getWinN[k]));
                if (Convert.ToInt32(getWinN[k]) <= 0)
                {
                    // return false;
                    boo = false;
                }
            }
        }
        return boo;
        // new BCW.BLL.tb_WinnersAward().Update(award);
    }
    #endregion

    #region 等奖数减一操作
    protected string losGetWinNumber(int i, int ID)
    {
        BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
        string[] getWinN = award.getWinNumber.Split('#');//数据等奖数处理提出处理
        for (int k = 0; k < getWinN.Length - 1; k++)
        {
            if (k == (i - 1))//识别位置-1操作
            {
                // if ((Convert.ToInt32(getWinN[k]) > 0))
                {
                    getWinN[k] = (Convert.ToInt32(getWinN[k]) - 1).ToString();
                }
                // else
                {
                    //     getWinN[k] = "0";
                }
            }
        }
        award.getWinNumber = "";
        for (int k = 0; k < getWinN.Length - 1; k++)
        {
            award.getWinNumber += getWinN[k] + "#";
        }
        return award.getWinNumber;
        // new BCW.BLL.tb_WinnersAward().Update(award);

    }
    #endregion

    #region 去除已派出去的一个
    protected string delStr(string one, string all)
    {
        string b = all.Replace(one, "");
        return b;
    }
    #endregion

    #region 生成是否重复
    protected bool isGet(string r, string yungouma)
    {
        bool b = true;
        if (yungouma == "")
        { return b; }
        else
        {
            string[] yun = yungouma.Split(',');
            //foreach (string j in yun)
            for (int i = 0; i < yun.Length; i++)
            {
                // if (yun[i].ToString() == (r.ToString()))
                if (yungouma.IndexOf(r.ToString()) >= 0)
                {
                    b = false;
                }
            }
            return b;
        }

    }
    #endregion

    #region 获取数据
    protected string getLists()
    {
        int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
        BCW.Model.tb_WinnersAward mode = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列
        string ss = mode.getWinNumber;
        Random m = new Random();
        string[] ssl = ss.Split(',');
        List<string> mList = new List<string>();
        for (int j = 0; j < ss.Length - 1; j++)
        {
            //if (ssl[j].ToString() != "0")
            {
                mList.Add(((j).ToString()));
            }
        }
        string a = mList[m.Next(1, mList.Count)];
        return a;

    }
    #endregion

}

