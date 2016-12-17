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
using TPR2.Common;

/// <summary>
/// 修改串串退注不删除
/// 黄国军 20160620
/// 
/// 增加串关超过10W币提醒
/// 黄国军 20160122
/// 
/// 增加串关下注消费记录,显示ID
/// 黄国军 20160328
/// </summary>
public partial class bbs_guess2_betsuper : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "超级串串竞猜";
        string act = "";
        act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "fsview":
                FsViewPage();
                break;
            case "st":
                StPage();
                break;
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "in":
                InPage();
                break;
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "alldel":
                DelallPage();
                break;
            case "save":
                SavePage();     //串串下注保存页
                break;
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("supercase.aspx"), "兑奖中心") + " ");
        builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=top"), "超级排行") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "下注记录") + " ");
        builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=list&amp;ptype=1"), "未开记录") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 计算值
    /// </summary>
    private string GetParaData(string Para, string iPara, int iType)
    {
        string[] paras = Regex.Split(Utils.Mid(Para, 2, Para.Length), "##");
        string sParas = string.Empty;
        for (int i = 0; i < paras.Length; i++)
        {

            if (i == iType)
            {
                sParas += "##" + iPara;
            }
            else
            {
                sParas += "##" + paras[i];
            }
        }
        sParas = Utils.Mid(sParas, 2, sParas.Length);
        return sParas;
    }

    /// <summary>
    /// 删除值
    /// </summary>
    private string DelParaData(string Para, string iPara, int iType)
    {
        string[] paras = Regex.Split(Utils.Mid(Para, 2, Para.Length), "##");
        string sParas = string.Empty;
        for (int i = 0; i < paras.Length; i++)
        {

            if (i != iType)
            {
                sParas += "##" + paras[i];
            }
        }
        sParas = Utils.Mid(sParas, 2, sParas.Length);
        return sParas;
    }


    private void ReloadPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ChangeExpirPage(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("比赛场次|" + Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "下注记录"));

        int id = new TPR2.BLL.guess.Super().GetSuperID(meid);
        if (id > 0)
        {
            string BID = new TPR2.BLL.guess.Super().GetSuperBID(id);
            BID = Utils.Mid(BID, 2, BID.Length);
            string[] sBID = Regex.Split(BID, "##");
            builder.Append("|" + Out.waplink(Utils.getUrl("betsuper.aspx?act=view"), "已选" + sBID.Length + "场"));
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("请最少选择3场比赛,最终返彩以所有选择的比赛结果为准.<br />日期,场次,竞猜");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strWhere = "";
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        strWhere = "p_active=0 and p_del=0 and p_TPRtime >= '" + System.DateTime.Now + "' and p_basketve=0";

        string strOrder = "p_TPRtime ASC";
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBaList.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaList n in listBaList)
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

                builder.Append(DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 0) + "<br />");


                builder.AppendFormat("[" + n.p_title + "]{0}&nbsp;VS&nbsp;{1}", n.p_one, n.p_two);

                if (!string.IsNullOrEmpty(n.p_usid) && ("#" + n.p_usid + "#").IndexOf("#" + meid + "#") != -1)
                    builder.Append("<u>已选</u>");
                else
                    builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=st&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "选择"));

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));//邵广林 修改可以显示具体页数
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
    }

    private void ListPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx"), "比赛场次") + "|下注记录");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 0)
            builder.Append("全部|" + Out.waplink(Utils.getUrl("betsuper.aspx?act=list&amp;ptype=1"), "未返") + "");
        else
            builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "全部") + "|未返");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsID=" + meid + " and IsOpen=1";
        if (ptype == 1)
            strWhere += " and Status=0";

        // 开始读取竞猜
        IList<TPR2.Model.guess.Super> listSuper = new TPR2.BLL.guess.Super().GetSupers(pageIndex, pageSize, strWhere, out recordCount);
        if (listSuper.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.Super n in listSuper)
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

                string sWin = string.Empty;
                if (n.Status == 0)
                    sWin = "未返";
                else if (n.Status == 1)
                    sWin = "赢";
                else if (n.Status == 10)
                    sWin = "退";
                else
                    sWin = "输";


                if (n.p_isfs > 0)
                {
                    builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=fsview&amp;id=" + n.ID + ""), "[" + sWin + "]串竞猜" + Convert.ToDouble(n.PayCent) + "x" + (n.p_isfs + 1) + "" + ub.Get("SiteBz") + "") + "[复式]");

                }
                else
                {
                    builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=listview&amp;id=" + n.ID + ""), "[" + sWin + "]串竞猜" + Convert.ToDouble(n.PayCent) + "" + ub.Get("SiteBz") + "") + "");

                }

                string[] Title = Regex.Split(Utils.Mid(n.Title, 2, n.Title.Length), "##");
                for (int i = 0; i < Title.Length; i++)
                {
                    builder.Append("<br />场次" + (i + 1) + ":" + Title[i] + "");

                }
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
    }

    private void FsViewPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "ID无效"));
        if (!new TPR2.BLL.guess.Super().Exists(id, meid))
        {
            Utils.Error("不存在的记录..", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "下注记录") + "|复式详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(id);

        string[] BID = Regex.Split(Utils.Mid(model.BID, 2, model.BID.Length), "##");
        string[] PID = Regex.Split(Utils.Mid(model.PID, 2, model.PID.Length), "##");
        string[] Title = Regex.Split(Utils.Mid(model.Title, 2, model.Title.Length), "##");
        string[] Times = Regex.Split(Utils.Mid(model.Times, 2, model.Times.Length), "##");
        string[] StTitle = Regex.Split(Utils.Mid(model.StTitle, 2, model.StTitle.Length), "##");
        string[] Odds = Regex.Split(Utils.Mid(model.Odds, 2, model.Odds.Length), "##");

        string strBID = "";
        List<string> listNum = new Combination().GetCombination2(BID.Length, model.p_isfs, BID);
        if (listNum.Count > 0)
        {
            foreach (string n in listNum)
            {
                strBID += "##" + n.Replace(",", "@");
            }
        }
        string strTitle = "";
        List<string> listNum2 = new Combination().GetCombination2(Title.Length, model.p_isfs, Title);
        if (listNum2.Count > 0)
        {
            foreach (string n in listNum2)
            {
                strTitle += "##" + n.Replace(",", "@");
            }
        }
        string strOdds = "";
        List<string> listNum3 = new Combination().GetCombination2(Odds.Length, model.p_isfs, Odds);
        if (listNum3.Count > 0)
        {
            foreach (string n in listNum3)
            {
                strOdds += "##" + n.Replace(",", "@");
            }
        }
        string strPID = "";
        List<string> listNum4 = new Combination().GetCombination2(PID.Length, model.p_isfs, PID);
        if (listNum4.Count > 0)
        {
            foreach (string n in listNum4)
            {
                strPID += "##" + n.Replace(",", "@");
            }
        }
        string strTimes = "";
        List<string> listNum5 = new Combination().GetCombination2(Times.Length, model.p_isfs, Times);
        if (listNum5.Count > 0)
        {
            foreach (string n in listNum5)
            {
                strTimes += "##" + n.Replace(",", "@");
            }
        }
        string strStTitle = "";
        List<string> listNum6 = new Combination().GetCombination2(StTitle.Length, model.p_isfs, StTitle);
        if (listNum6.Count > 0)
        {
            foreach (string n in listNum6)
            {
                strStTitle += "##" + n.Replace(",", "@");
            }
        }
        string[] sBID2 = Regex.Split(Utils.Mid(strBID, 2, strBID.Length), "##");
        string[] sPID2 = Regex.Split(Utils.Mid(strPID, 2, strPID.Length), "##");
        string[] sTitle2 = Regex.Split(Utils.Mid(strTitle, 2, strTitle.Length), "##");
        string[] sOdds2 = Regex.Split(Utils.Mid(strOdds, 2, strOdds.Length), "##");
        string[] sTimes2 = Regex.Split(Utils.Mid(strTimes, 2, strTimes.Length), "##");
        string[] sStTitle2 = Regex.Split(Utils.Mid(strStTitle, 2, strStTitle.Length), "##");



        string getNum = "";
        int kk = 0;

        for (int j = 0; j < sBID2.Length; j++)
        {
            string[] sBID3 = Regex.Split(sBID2[j], "@");
            string[] sPID3 = Regex.Split(sPID2[j], "@");
            string[] sTitle3 = Regex.Split(sTitle2[j], "@");
            string[] sOdds3 = Regex.Split(sOdds2[j], "@");
            string[] sTimes3 = Regex.Split(sTimes2[j], "@");
            string[] sStTitle3 = Regex.Split(sStTitle2[j], "@");

            int k = 0;
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + (j + 1) + "串");
            for (int i = 0; i < sBID3.Length; i++)
            {
                builder.Append("<small>");
                builder.Append("<br />" + (i + 1) + "：" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + sBID3[i] + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + sTitle3[i] + "") + "<br />");
                builder.Append("" + sTimes3[i] + "<br />");
                builder.Append("" + sStTitle3[i] + ",");
                builder.Append("赔率:" + sOdds3[i]);

                double odds = Convert.ToDouble(Odds[i]);
                if (model.Status > 0)
                {
                    if (model.Status == 10)
                    {
                        builder.Append("<br />结果:已退");
                    }
                    else
                    {
                        string[] Result = Regex.Split(Utils.Mid(model.getOdds, 2, model.getOdds.Length), "##");
                        builder.Append("<br />结果:" + Result[k + (kk * model.p_isfs)]);
                    }
                }
                else
                {
                    builder.Append("<br />结果:未返");
                }

                k++;

                builder.Append("</small>");

            }
            kk++;
            builder.Append(Out.Tab("</div>", Out.Hr()));

        }
        builder.Append(Out.Tab("<div>", ""));
        if (model.Status == 1)
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent + (model.PayCent * model.p_isfs)) + "" + ub.Get("SiteBz") + "[共" + (model.p_isfs + 1) + "串]<br />");
            builder.Append("总计:赢了" + model.getMoney + "" + ub.Get("SiteBz") + "");
        }
        else if (model.Status == 2)
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent + (model.PayCent * model.p_isfs)) + "" + ub.Get("SiteBz") + "[共" + (model.p_isfs + 1) + "串]<br />");
            builder.Append("总计:输了" + Convert.ToDouble(model.PayCent + (model.PayCent * model.p_isfs)) + "" + ub.Get("SiteBz") + "");
        }
        else if (model.Status == 10)
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent + (model.PayCent * model.p_isfs)) + "" + ub.Get("SiteBz") + "[共" + (model.p_isfs + 1) + "串]<br />");
            builder.Append("总计:已退");
        }
        else
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent + (model.PayCent * model.p_isfs)) + "" + ub.Get("SiteBz") + "[共" + (model.p_isfs + 1) + "串]<br />");
            builder.Append("总计:等待返彩");
        }
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "ID无效"));
        if (!new TPR2.BLL.guess.Super().Exists(id, meid))
        {
            Utils.Error("不存在的记录..", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "下注记录") + "|查看详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(id);

        string[] BID = Regex.Split(Utils.Mid(model.BID, 2, model.BID.Length), "##");
        string[] Title = Regex.Split(Utils.Mid(model.Title, 2, model.Title.Length), "##");
        string[] Times = Regex.Split(Utils.Mid(model.Times, 2, model.Times.Length), "##");
        string[] StTitle = Regex.Split(Utils.Mid(model.StTitle, 2, model.StTitle.Length), "##");
        string[] Odds = Regex.Split(Utils.Mid(model.Odds, 2, model.Odds.Length), "##");

        string[] getOdds = null;
        if (model.Status > 0)
        {
            string forOdds = model.getOdds + "##联系客服##联系客服##联系客服##联系客服##联系客服##联系客服##联系客服";

            try
            {
                getOdds = Regex.Split(Utils.Mid(model.getOdds, 2, model.getOdds.Length), "##");

                if (Odds.Length != getOdds.Length)
                {
                    getOdds = Regex.Split(Utils.Mid(forOdds, 2, forOdds.Length), "##");
                }
            }
            catch
            {
                getOdds = Regex.Split(Utils.Mid(forOdds, 2, forOdds.Length), "##");

            }
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.AddTime);
        double pl = 1;
        int ppNum = 0;
        int sbNum = 0;
        double pCent = Convert.ToDouble(model.PayCent);
        for (int i = 0; i < Title.Length; i++)
        {
            string sResult = string.Empty;
            int bcid = Utils.ParseInt(BID[i]);
            DataSet ds = new TPR2.BLL.guess.BaList().GetBaListList("p_result_one,p_result_two", "id=" + bcid + " and p_active>0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                sResult = ds.Tables[0].Rows[0]["p_result_one"] + ":" + ds.Tables[0].Rows[0]["p_result_two"] + "";
            }
            else
            {
                sResult = "未完场";
            }

            builder.Append("<br />比赛场次" + (i + 1) + "：" + Title[i] + "(赛事ID:" + BID[i] + "/" + sResult + ")<br />");
            builder.Append("比赛时间：" + Times[i] + "<br />");
            builder.Append("" + StTitle[i] + ",");
            builder.Append("赔率:" + Odds[i]);
            double odds = Convert.ToDouble(Odds[i]);
            if (model.Status > 0)
            {
                if (model.Status == 10)
                {
                    builder.Append("<br />结果:已退");
                }
                else
                {
                    builder.Append("<br />结果:" + getOdds[i]);
                    string str = getOdds[i].ToString();

                    if (str == "全输")
                    {
                        pl = pl * 0;
                        pCent = pCent * 0;
                    }
                    else if (str == "平盘")
                    {
                        pl = pl * 1;
                        pCent = pCent * 1;
                        ppNum++;
                    }
                    else if (str == "走盘")
                    {
                        pl = pl * 1;
                        pCent = pCent * 1;
                    }
                    else if (str == "输半")
                    {
                        pl = pl * 0.5;
                        pCent = pCent * 0.5;
                        sbNum++;
                    }
                    else if (str == "全赢")
                    {
                        pl = pl * odds;
                        pCent = pCent * odds;
                    }
                    else//赢半
                    {
                        pl = pl * Convert.ToDouble((odds - 1) / 2 + 1);
                        pCent = pCent * Convert.ToDouble((odds - 1) / 2 + 1);
                    }
                }
            }
            else
            {
                builder.Append("<br />结果:未返");
            }
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (model.Status > 0 && (Title.Length - ppNum) < 3)
        {
            builder.Append("因赛事提前、腰斩、推迟、中断、待定而导致平盘的串除外,正常完场(含走盘)的串少于三场的串关将作无效处理并退回本金.<br />");
            builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计:已退回本金" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "");
        }
        else if (model.Status > 0 && sbNum >= 2)
        {
            builder.Append("此条串串存在2场或2场以上输半<br />");
            builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计:输了" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "");
        }
        else
        {
            if (model.Status == 1)
            {
                builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("总计:赢了" + pCent + "" + ub.Get("SiteBz") + "");
                builder.Append("<br />总赔率:" + Convert.ToDouble(pl) + "");
            }
            else if (model.Status == 2)
            {
                builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("总计:输了" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "");
            }
            else if (model.Status == 10)
            {
                builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("总计:已退");
            }
            else
            {
                builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("总计:等待返彩");


            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void StPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "赛事ID无效"));

        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList st = bll.GetModel(gid);

        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string bo = "";
        //-----------------------------立即更新水位---------------------------------
        if (st.p_active == 0)
        {
            if (st.p_basketve == 0)
            {
                if (st.p_type == 1)
                {
                    if (st.p_ison == 1)
                    {
                        new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), true);
                    }
                    else
                    {
                        new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), false);
                        bo = "1";
                    }
                }
                else
                {
                    if (st.p_ison == 1)
                    {
                        new TPR2.Collec.Basketbo().GetBoView(Convert.ToInt32(st.p_id), true);

                    }
                    else
                    {
                        new TPR2.Collec.Basketbo().GetBoView(Convert.ToInt32(st.p_id), false);
                        bo = "1";
                    }
                }
            }
        }

        if (bo == "")
        {
            Utils.Error("本赛事数据数据暂停,暂停下注,请谅解", "");
        }

        TPR2.Model.guess.BaList model = bll.GetModel(gid);

        if (model.p_del == 1 || model.p_ison == 1)
        {
            Utils.Error("不存在的记录", "");
        }

        builder.Append(Out.Tab("<div>", ""));

        string isselect = string.Empty;
        int SuperID = new TPR2.BLL.guess.Super().GetSuperID(meid);
        if (SuperID > 0)
        {
            TPR2.Model.guess.Super model2 = new TPR2.BLL.guess.Super().GetSuper(SuperID);
            if (("##" + model2.BID + "##").IndexOf("##" + gid + "##") != -1)
            {
                isselect = "[修改]";
            }
        }
        builder.Append("" + isselect + "" + model.p_one + "VS" + model.p_two + "");

        builder.Append("<br />开赛:" + model.p_TPRtime);

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=in&amp;gid=" + model.ID + "&amp;p=1"), model.p_one + "(" + Convert.ToDouble(model.p_one_lu) + ")"));

        if (model.p_type == 1)
            builder.Append("" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
        else
            builder.Append("" + Convert.ToDouble(model.p_pk) + "");

        builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=in&amp;gid=" + model.ID + "&amp;p=2"), model.p_two + "(" + Convert.ToDouble(model.p_two_lu) + ")"));
        if (model.p_big_lu != 0)
        {
            builder.Append("<br />" + Out.waplink(Utils.getUrl("betsuper.aspx?act=in&amp;gid=" + model.ID + "&amp;p=3"), "大(" + Convert.ToDouble(model.p_big_lu) + ")"));

            if (model.p_type == 1)
                builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
            else
                builder.Append(Convert.ToDouble(model.p_dx_pk));

            builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=in&amp;gid=" + model.ID + "&amp;p=4"), "小(" + Convert.ToDouble(model.p_small_lu) + ")"));
        }

        builder.Append(Out.Tab("</div>", ""));
    }

    private void InPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ChangeExpirPage(meid);
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "赛事ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "get", 2, @"^[1-4]$", "选择无效"));
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();

        if (bll.GetModel(gid) == null)
        {
            Utils.Error("不存在的记录", "");
        }
        TPR2.Model.guess.BaList model = bll.GetModel(gid);

        if (model.p_del == 1 || model.p_ison == 1)
        {
            Utils.Error("不存在的记录", "");
        }

        int basketve = new TPR2.BLL.guess.BaList().Getp_basketve(gid);
        if (basketve > 0)
        {
            Utils.Error("单节和半场比赛不能进行此操作", "");

        }


        //组合显示串
        string payview = "";
        if (model.p_type == 1)
        {
            if (p == 1 || p == 2)
                payview += "" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "";
            else
                payview += "大小球" + GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)) + "";
        }
        else
        {
            if (p == 1 || p == 2)
                payview += "" + model.p_pk + "";
            else
                payview += "大小球" + Convert.ToDouble(model.p_dx_pk).ToString() + "";

        }

        if (p == 1)
            payview += "压" + model.p_one + "";
        if (p == 2)
            payview += "压" + model.p_two + "";
        if (p == 3)
            payview += "压大球";
        if (p == 4)
            payview += "压小球";
        if (p == 5)
            payview += "压主胜";
        if (p == 6)
            payview += "压平手";
        if (p == 7)
            payview += "压客胜";

        string Odds = "";
        if (p == 1)
            Odds += "" + Convert.ToDouble(model.p_one_lu) + "";
        if (p == 2)
            Odds += "" + Convert.ToDouble(model.p_two_lu) + "";
        if (p == 3)
            Odds += "" + Convert.ToDouble(model.p_big_lu) + "";
        if (p == 4)
            Odds += "" + Convert.ToDouble(model.p_small_lu) + "";
        if (p == 5)
            Odds += "" + Convert.ToDouble(model.p_bzs_lu) + "";
        if (p == 6)
            Odds += "" + Convert.ToDouble(model.p_bzp_lu) + "";
        if (p == 7)
            Odds += "" + Convert.ToDouble(model.p_bzx_lu) + "";

        if (Convert.ToDouble(Odds) > 2.5)
        {
            Utils.Error("水位大于2.5，无法选择此盘口", "");
        }
        //写入bapay
        TPR2.Model.guess.BaPay model3 = new TPR2.Model.guess.BaPay();
        model3.payview = "";
        model3.payusid = meid;
        model3.payusname = new BCW.BLL.User().GetUsName(meid);
        model3.bcid = gid;
        model3.pType = model.p_type;
        model3.PayType = p;
        model3.payCent = 100;//固定金额
        if (p == 1 || p == 2)
        {
            model3.payonLuone = model.p_one_lu;
            model3.payonLutwo = model.p_two_lu;
            model3.payonLuthr = 0;
        }
        else if (p == 3 || p == 4)
        {
            model3.payonLuone = model.p_big_lu;
            model3.payonLutwo = model.p_small_lu;
            model3.payonLuthr = 0;
        }
        model3.p_pk = model.p_pk;
        model3.p_dx_pk = model.p_dx_pk;
        model3.p_pn = model.p_pn;
        model3.paytimes = DateTime.Now;
        model3.p_result_temp1 = 0;
        model3.p_result_temp2 = 0;
        model3.itypes = 1;
        model3.Types = 0;
        model3.p_TPRtime = Convert.ToDateTime(model.p_TPRtime);
        model3.p_oncetime2 = DateTime.Parse("1990-1-1");
        int pid = new TPR2.BLL.guess.BaPay().Add(model3);


        TPR2.Model.guess.Super addmodel = new TPR2.Model.guess.Super();
        addmodel.UsID = meid;
        addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
        addmodel.BID = "##" + gid;
        addmodel.PID = "##" + pid;
        addmodel.SP = "##" + p.ToString();
        addmodel.Title = "##" + model.p_one + "&nbsp;VS&nbsp;" + model.p_two + "";
        addmodel.Times = "##" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0);
        addmodel.StTitle = "##" + payview;
        addmodel.Odds = "##" + Odds;
        addmodel.PayCent = 0;
        addmodel.Status = 0;
        addmodel.IsOpen = 0;
        addmodel.AddTime = DateTime.Now;
        int SuperID = new TPR2.BLL.guess.Super().GetSuperID(meid);

        if (SuperID == 0)
        {
            SuperID = new TPR2.BLL.guess.Super().Add(addmodel);
        }
        else
        {

            addmodel.ID = SuperID;
            TPR2.Model.guess.Super model2 = new TPR2.BLL.guess.Super().GetSuper(SuperID);

            string[] stBID = Regex.Split(Utils.Mid(model2.BID, 2, model2.BID.Length), "##");
            if (stBID.Length >= 8)
            {
                Utils.Error("最多可以选8场！请你先下注吧", "");
            }


            if (("##" + model2.BID + "##").IndexOf("##" + gid + "##") == -1)
            {
                new TPR2.BLL.guess.Super().Update(addmodel);
            }
            else
            {
                string[] BID = Regex.Split(Utils.Mid(model2.BID, 2, model2.BID.Length), "##");
                string[] PID = Regex.Split(Utils.Mid(model2.PID, 2, model2.PID.Length), "##");
                int k = 0;
                for (int i = 0; i < BID.Length; i++)
                {
                    if (Convert.ToInt32(BID[i]) == gid)
                    {
                        k = i;
                        break;
                    }
                }

                addmodel.Title = "##" + GetParaData(model2.Title, model.p_one + "&nbsp;VS&nbsp;" + model.p_two, k);
                addmodel.Times = "##" + GetParaData(model2.Times, model.p_TPRtime.ToString(), k);
                addmodel.SP = "##" + GetParaData(model2.SP, p.ToString(), k);
                addmodel.BID = "##" + GetParaData(model2.BID, gid.ToString(), k);
                addmodel.PID = "##" + GetParaData(model2.PID, pid.ToString(), k);
                addmodel.StTitle = "##" + GetParaData(model2.StTitle, payview, k);
                addmodel.Odds = "##" + GetParaData(model2.Odds, Odds, k);
                new TPR2.BLL.guess.Super().Update2(addmodel);
                //删除之前的BaPay投注记录
                new TPR2.BLL.guess.BaPay().DeleteStr("id=" + Convert.ToInt32(PID[k]) + "");
                //删除标识记录
                string strBID = new TPR2.BLL.guess.BaList().Getp_usid(gid);
                string sBID = "#" + meid + "#";
                strBID = strBID.Replace(sBID, "");
                new TPR2.BLL.guess.BaList().Updatep_usid(gid, strBID);
            }

        }

        //新增标识记录
        string strBID2 = new TPR2.BLL.guess.BaList().Getp_usid(gid);
        strBID2 += "#" + meid + "#";
        new TPR2.BLL.guess.BaList().Updatep_usid(gid, strBID2);
        Utils.Success("选择赛事", "恭喜，选择成功..", Utils.getUrl("betsuper.aspx?act=view"), "1");
    }

    #region 串串下注选择 ViewPage
    /// <summary>
    /// 串串下注选择
    /// </summary>
    private void ViewPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ChangeExpirPage(meid);
        int id = new TPR2.BLL.guess.Super().GetSuperID(meid);
        if (!new TPR2.BLL.guess.Super().Exists(id, meid))
        {
            Utils.Error("不存在的记录", "");
        }
        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx"), "比赛场次"));
        builder.Append("|" + Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "下注记录"));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("请最少选择3场比赛,最终返彩以所有选择的比赛结果为准.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        string[] BID = Regex.Split(Utils.Mid(model.BID, 2, model.BID.Length), "##");
        string[] Title = Regex.Split(Utils.Mid(model.Title, 2, model.Title.Length), "##");
        string[] Times = Regex.Split(Utils.Mid(model.Times, 2, model.Times.Length), "##");
        string[] StTitle = Regex.Split(Utils.Mid(model.StTitle, 2, model.StTitle.Length), "##");
        string[] Odds = Regex.Split(Utils.Mid(model.Odds, 2, model.Odds.Length), "##");
        int k = 1;
        for (int i = 0; i < Title.Length; i++)
        {
            builder.Append("<br />比赛场次" + (i + 1) + "：" + Title[i]);
            builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=del&amp;gid=" + BID[i] + ""), "删除") + "<br />");
            builder.Append("比赛时间：" + Times[i] + "<br />");
            builder.Append("" + StTitle[i] + ",");
            builder.Append("赔率:" + Odds[i]);
            builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx?act=st&amp;gid=" + BID[i] + ""), "修改") + "");
            builder.Append("<br />-----------");
            k++;
        }

        if (k < 9)
        {
            for (int i = k; i < 9; i++)
            {
                builder.Append("<br />比赛场次" + i + ":暂空&nbsp;");
                builder.Append("" + Out.waplink(Utils.getUrl("betsuper.aspx"), "添加") + "");
                builder.Append("<br />-----------");

            }
        }
        builder.Append(Out.Tab("</div>", ""));

        if (k > 1)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=alldel"), "全部删除所选") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        if (k >= 4)
        {
            string strText = "下注,";
            string strName = "payCent,act";
            string strType = "num,hidden";
            string strValu = "'save";
            string strEmpt = "false,false";
            string strIdea = "/限" + ub.GetSub("SiteSmallPay2", xmlPath) + "-" + ub.GetSub("SiteBigPay2", xmlPath) + "" + ub.Get("SiteBz") + "/";
            string strOthe = "普通下注|复式下注,betsuper.aspx,post,0,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("您现在有" + gold + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
        }

    }
    #endregion

    private void DelallPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string info = Utils.GetRequest("info", "get", 1, "", "");

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定全部删除所选吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=alldel&amp;info=ok"), "确定删除") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=view"), "先留着吧..") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int id = new TPR2.BLL.guess.Super().GetSuperID(meid);
            if (id == 0)
            {
                Utils.Error("不存在的记录", "");
            }
            TPR2.Model.guess.Super m = new TPR2.BLL.guess.Super().GetSuper(id);

            string[] sBID = Regex.Split(Utils.Mid(m.BID, 2, m.BID.Length), "##");
            string[] sPID = Regex.Split(Utils.Mid(m.PID, 2, m.PID.Length), "##");

            //更新已确定投注的会员ID
            for (int i = 0; i < sBID.Length; i++)
            {
                int iBID = Convert.ToInt32(sBID[i]);
                string strBID = new TPR2.BLL.guess.BaList().Getp_usid(iBID);
                string sBID2 = "#" + meid + "#";
                strBID = strBID.Replace(sBID2, "");
                new TPR2.BLL.guess.BaList().Updatep_usid(iBID, strBID);
                //删除之前的BaPay投注记录
                new TPR2.BLL.guess.BaPay().DeleteStr("id=" + Convert.ToInt32(sPID[i]) + "");
            }

            new TPR2.BLL.guess.Super().Delete(id);
            Utils.Success("删除", "删除成功..", Utils.getUrl("betsuper.aspx"), "1");
        }

    }

    private void DelPage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "ID无效"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();

        if (bll.GetModel(gid) == null)
        {
            Utils.Error("不存在的记录", "");
        }
        TPR2.Model.guess.BaList model = bll.GetModel(gid);

        if (model.p_del == 1 || model.p_ison == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此场次吗");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=del&amp;gid=" + gid + "&amp;info=ok"), "确定删除") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=view"), "先留着吧..") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int id = new TPR2.BLL.guess.Super().GetSuperID(meid);

            if (!new TPR2.BLL.guess.Super().Exists(id, meid))
            {
                Utils.Error("不存在的记录", "");
            }
            TPR2.Model.guess.Super model2 = new TPR2.BLL.guess.Super().GetSuper(id);
            string[] BID = Regex.Split(Utils.Mid(model2.BID, 2, model2.BID.Length), "##");
            string[] SP = Regex.Split(Utils.Mid(model2.SP, 2, model2.SP.Length), "##");
            string[] PID = Regex.Split(Utils.Mid(model2.PID, 2, model2.PID.Length), "##");
            int k = 0;
            string pid = string.Empty;
            for (int i = 0; i < BID.Length; i++)
            {
                if (Convert.ToInt32(BID[i]) == gid)
                {
                    k = i;
                    pid = PID[i];
                    break;
                }
            }

            //组合显示串
            string payview = "";
            int p = Convert.ToInt32(SP[k]);

            if (model.p_type == 1)
            {
                if (p == 1 || p == 2)
                    payview += "" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "";
                else
                    payview += "大小球" + GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)) + "";
            }
            else
            {
                if (p == 1 || p == 2)
                    payview += "" + model.p_pk + "";
                else
                    payview += "大小球" + Convert.ToDouble(model.p_dx_pk).ToString() + "";

            }

            if (p == 1)
                payview += "压" + model.p_one + "";
            if (p == 2)
                payview += "压" + model.p_two + "";
            if (p == 3)
                payview += "压大球";
            if (p == 4)
                payview += "压小球";
            if (p == 5)
                payview += "压主胜";
            if (p == 6)
                payview += "压平手";
            if (p == 7)
                payview += "压客胜";

            string Odds = "";
            if (p == 1)
                Odds += "" + Convert.ToDouble(model.p_one_lu) + "";
            if (p == 2)
                Odds += "" + Convert.ToDouble(model.p_two_lu) + "";
            if (p == 3)
                Odds += "" + Convert.ToDouble(model.p_big_lu) + "";
            if (p == 4)
                Odds += "" + Convert.ToDouble(model.p_small_lu) + "";
            if (p == 5)
                Odds += "" + Convert.ToDouble(model.p_bzs_lu) + "";
            if (p == 6)
                Odds += "" + Convert.ToDouble(model.p_bzp_lu) + "";
            if (p == 7)
                Odds += "" + Convert.ToDouble(model.p_bzx_lu) + "";



            TPR2.Model.guess.Super addmodel = new TPR2.Model.guess.Super();

            addmodel.ID = id;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.BID = "##" + DelParaData(model2.BID, gid.ToString(), k);
            addmodel.PID = "##" + DelParaData(model2.PID, pid, k);
            addmodel.SP = "##" + DelParaData(model2.SP, p.ToString(), k);
            addmodel.Title = "##" + DelParaData(model2.Title, model.p_one + "&nbsp;VS&nbsp;" + model.p_two, k);
            addmodel.Times = "##" + DelParaData(model2.Times, DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0), k);
            addmodel.StTitle = "##" + DelParaData(model2.StTitle, payview, k);
            addmodel.Odds = "##" + DelParaData(model2.Odds, Odds, k);
            addmodel.PayCent = 0;
            addmodel.Status = 0;
            addmodel.IsOpen = 0;
            addmodel.AddTime = DateTime.Now;
            new TPR2.BLL.guess.Super().Update2(addmodel);

            bool overdel = false;
            if (addmodel.BID == "##")
            {
                new TPR2.BLL.guess.Super().Delete(id);
                overdel = true;
            }

            //删除之前的BaPay投注记录
            new TPR2.BLL.guess.BaPay().DeleteStr("id=" + Convert.ToInt32(pid) + "");
            //删除标识记录
            string strBID = new TPR2.BLL.guess.BaList().Getp_usid(gid);
            string sBID = "#" + meid + "#";
            strBID = strBID.Replace(sBID, "");
            new TPR2.BLL.guess.BaList().Updatep_usid(gid, strBID);

            if (overdel)
            {
                Utils.Success("删除", "删除成功..", Utils.getUrl("betsuper.aspx"), "1");
            }
            else
            {
                Utils.Success("删除", "删除成功..", Utils.getUrl("betsuper.aspx?act=view"), "1");
            }
        }
    }

    #region 串串下注保存页 SavePage
    /// <summary>
    /// 串串下注保存页
    /// 修改串串下注超于10万酷币发送系统提醒到10086
    /// 黄国军 20160122
    /// </summary>
    private void SavePage()
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        //会员名字
        string mename = new BCW.BLL.User().GetUsName(meid);

        if (meid == 0)
            Utils.Login();
        //下注超时判断
        ChangeExpirPage(meid);

        int id = new TPR2.BLL.guess.Super().GetSuperID(meid);
        if (id == 0)
        {
            Utils.Error("不存在的记录", "");
        }
        string BID = new TPR2.BLL.guess.Super().GetSuperBID(id);
        string[] sBID = Regex.Split(Utils.Mid(BID, 2, BID.Length), "##");
        if (sBID.Length < 3)
        {
            Utils.Error("至少选三场才可以下注", "");
        }

        for (int i = 0; i < sBID.Length; i++)
        {
            int gid = Convert.ToInt32(sBID[i]);
            TPR2.BLL.guess.BaList BaListbll = new TPR2.BLL.guess.BaList();
            TPR2.Model.guess.BaList modelBaList = BaListbll.GetModel(gid);
            if (modelBaList.p_TPRtime <= DateTime.Now)
            {
                Utils.Error("[" + modelBaList.p_one + "VS" + modelBaList.p_two + "]开赛时间已到，不能下注，请尝试删除此赛事再下注！", "");
            }
        }

        int payCent = int.Parse(Utils.GetRequest("payCent", "post", 2, @"^[0-9]\d*$", "下注填写错误"));

        if (payCent < Convert.ToInt32(ub.GetSub("SiteSmallPay2", xmlPath)) || payCent > Convert.ToInt32(ub.GetSub("SiteBigPay2", xmlPath)))
        {
            Utils.Error("下注限" + ub.Get("SiteSmallPay2") + "-" + ub.Get("SiteBigPay2") + "" + ub.Get("SiteBz") + "", "");
        }
        long gold = new BCW.BLL.User().GetGold(meid);

        int isfs = int.Parse(Utils.GetRequest("isfs", "all", 1, @"^[0-9]$", "0"));
        int cents = payCent;

        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        if (ac == "复式下注")
        {
            isfs = (sBID.Length - 1);
            cents = Convert.ToInt32(payCent * ((sBID.Length)));

            if (sBID.Length < 4)
            {
                Utils.Error("至少选四场才可以进行复式", "");
            }

            new Out().head(Utils.ForWordType("复式下注"));
            Response.Write(Out.Tab("<div class=\"text\">", ""));
            Response.Write("=复式确认=<br />");
            Response.Write("场数:" + (sBID.Length) + "场<br />");
            Response.Write("串数:" + (sBID.Length) + "串<br />");
            Response.Write("每关:" + payCent + "" + ub.Get("SiteBz") + "<br />");
            Response.Write("金额:" + cents + "" + ub.Get("SiteBz") + "<br />");

            string strName = "payCent,act,isfs";
            string strValu = "" + payCent + "'save'" + isfs + "";
            string strOthe = "确认下注,betsuper.aspx,post,0,red";

            Response.Write(Out.wapform(strName, strValu, strOthe));

            Response.Write(Out.Tab("</div>", "<br />"));
            Response.Write(Out.Tab("<div>", ""));
            Response.Write("您自带" + gold + "" + ub.Get("SiteBz") + "<br />");
            Response.Write("<a href=\"" + Utils.getUrl("betsuper.aspx?act=view") + "\">返回上级</a>");
            Response.Write(Out.Tab("</div>", ""));
            Response.Write(new Out().foot());
            Response.End();
        }

        if (isfs > 0)
        {
            isfs = (sBID.Length - 1);
            cents = Convert.ToInt32(payCent * ((sBID.Length)));
        }
        else
        {
            if (sBID.Length > 7)
            {
                Utils.Error("普通下注最多可以选七场", "");
            }

        }

        //判断金额是否够了
        if (gold < cents)
        {
            Utils.Error("你的" + ub.Get("SiteBz") + "不够此次下注", "");
        }

        GetCent(meid, id);

        TPR2.Model.guess.Super addmodel = new TPR2.Model.guess.Super();
        addmodel.ID = id;
        addmodel.PayCent = payCent;
        addmodel.IsOpen = 1;
        addmodel.AddTime = DateTime.Now;
        addmodel.p_isfs = isfs;
        new TPR2.BLL.guess.Super().Update3(addmodel);
        //更新已确定投注的会员ID
        for (int i = 0; i < sBID.Length; i++)
        {
            int iBID = Convert.ToInt32(sBID[i]);
            string strBID = new TPR2.BLL.guess.BaList().Getp_usid(iBID);
            string sBID2 = "#" + meid + "#";
            strBID = strBID.Replace(sBID2, "");
            new TPR2.BLL.guess.BaList().Updatep_usid(iBID, strBID);
        }

        //操作币
        new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(cents), "串串下注ID" + id);
        if (cents >= 100000)
        {
            //超过10W串串发送系统信息到10086
            //会员ID----在串串下注超100000酷币，请检查该（串串）括号里做这串串的链接        
            string gmname = new BCW.BLL.User().GetUsName(10086);
            new BCW.BLL.Guest().Add(10086, gmname, "会员ID" + meid + "[url=/bbs/uinfo.aspx?uid=" + meid + "] " + mename + " [/url]在串串下注超100000酷币，请检查该串关ID（" + id + "）");
        }
        Utils.Success("下注", "下注成功..", Utils.getUrl("betsuper.aspx?act=list"), "1");
    }
    #endregion

    private void TopPage()
    {
        int pageSize = 0;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string strWhere = "";

        // 开始读取排行榜
        IList<TPR2.Model.guess.SuperOrder> listSuperOrder = new TPR2.BLL.guess.SuperOrder().GetSuperOrders(pageIndex, pageSize, strWhere, out recordCount);
        if (listSuperOrder.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.SuperOrder n in listSuperOrder)
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

                builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{2}") + "盈利{3}币", (pageIndex - 1) * 10 + k, n.UsID, n.UsName, Convert.ToDouble(n.Cent));

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
    }

    #region 超时下注判断 ChangeExpirPage
    /// <summary>
    /// 超时下注判断
    /// </summary>
    /// <param name="meid"></param>
    private void ChangeExpirPage(int meid)
    {
        int id = new TPR2.BLL.guess.Super().GetSuperID(meid);
        if (id > 0)
        {
            int VoteSec = Utils.ParseInt(ub.GetSub("SiteSuperVoteSec", xmlPath));

            TPR2.Model.guess.Super m = new TPR2.BLL.guess.Super().GetSuper(id);

            if (Convert.ToDateTime(m.AddTime).AddSeconds(VoteSec) < DateTime.Now)
            {
                string[] sBID = Regex.Split(Utils.Mid(m.BID, 2, m.BID.Length), "##");
                string[] sPID = Regex.Split(Utils.Mid(m.PID, 2, m.PID.Length), "##");

                //更新已确定投注的会员ID
                for (int i = 0; i < sBID.Length; i++)
                {
                    int iBID = Convert.ToInt32(sBID[i]);
                    string strBID = new TPR2.BLL.guess.BaList().Getp_usid(iBID);
                    string sBID2 = "#" + meid + "#";
                    strBID = strBID.Replace(sBID2, "");
                    new TPR2.BLL.guess.BaList().Updatep_usid(iBID, strBID);
                    //删除之前的BaPay投注记录
                    new TPR2.BLL.guess.BaPay().DeleteStr("id=" + Convert.ToInt32(sPID[i]) + "");
                }

                new TPR2.BLL.guess.Super().Delete(id);
                Utils.Success("超时下注", "你的选择已超时，请重新选择赛事进行下注..", Utils.getUrl("betsuper.aspx"), "3");

            }
        }
    }
    #endregion

    #region 同一场并同一盘口每ID限额判断 GetCent
    /// <summary>
    /// 同一场并同一盘口每ID限额判断
    /// </summary>
    /// <param name="meid"></param>
    /// <param name="id"></param>
    private void GetCent(int meid, int id)
    {
        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(id);
        string BID = model.BID;
        string SP = model.SP;
        string Title = model.Title;
        string StTitle = model.StTitle;

        string[] sBID = Regex.Split(Utils.Mid(BID, 2, BID.Length), "##");
        string[] sSP = Regex.Split(Utils.Mid(SP, 2, SP.Length), "##");
        string[] sTitle = Regex.Split(Utils.Mid(Title, 2, Title.Length), "##");
        string[] sStTitle = Regex.Split(Utils.Mid(StTitle, 2, StTitle.Length), "##");

        DataSet ds = new TPR2.BLL.guess.Super().GetList("BID,SP,PayCent", "UsID=" + meid + " and Status=0 and IsOpen=1");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < sBID.Length; k++)
            {
                int sbid = Convert.ToInt32(sBID[k]);
                int ssp = Convert.ToInt32(sSP[k]);

                double OutCent = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string BID2 = ds.Tables[0].Rows[i]["BID"].ToString();
                    string SP2 = ds.Tables[0].Rows[i]["SP"].ToString();
                    double PayCent2 = Convert.ToDouble(ds.Tables[0].Rows[i]["PayCent"].ToString());
                    string[] sBID2 = Regex.Split(Utils.Mid(BID2, 2, BID2.Length), "##");
                    string[] sSP2 = Regex.Split(Utils.Mid(SP2, 2, SP2.Length), "##");

                    for (int j = 0; j < sBID2.Length; j++)
                    {
                        int sbid2 = Convert.ToInt32(sBID2[j]);
                        int ssp2 = Convert.ToInt32(sSP2[j]);
                        if (sbid == sbid2 && ssp == ssp2)
                        {
                            OutCent += PayCent2;
                        }
                    }

                }

                double SubCent = Convert.ToDouble(ub.GetSub("SiteSuperSubCent", xmlPath));
                if (OutCent >= SubCent)
                {
                    Utils.Error("您选择的：" + sTitle[k] + "-" + sStTitle[k] + "已达单项盘口受注" + SubCent + "" + ub.Get("SiteBz") + "上限，请选择其它盘口或赛事进行串关", "");
                }
            }
        }
    }
    #endregion
}