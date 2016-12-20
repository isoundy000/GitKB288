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
/// 修正半半波,精英杀料开奖判断
/// 
/// 黄国军 20160311
/// </summary>
public partial class bbs_Gsopen : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";
    protected string xmlPath2 = "/Controls/six49.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "高手参赛管理";
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
        { }
        else
        {
            Utils.Error("不存在的记录", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "open":                //开奖填写
                OpenPage(forumid);
                break;
            case "opensave":            //确定开奖
                OpenSavePage(forumid);
                break;
            default:                    //开奖首页
                ReloadPage(forumid);
                break;
        }
    }

    #region 开奖填写页 OpenPage
    /// <summary>
    /// 开奖填写页
    /// </summary>
    /// <param name="forumid"></param>
    private void OpenPage(int forumid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string GsAdminID = ub.GetSub("BbsGsAdminID", xmlPath);
        if (!("#" + GsAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("权限不足", "");
        }
        DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
        int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
        if (GsqiNum == 0)
        {
            Utils.Error("本期尚未设置", "");
        }
        if (GsStopTime > DateTime.Now)
        {
            //Utils.Error("本期第" + GsqiNum + "期截止时间" + GsStopTime + "，截止时间未到不能开奖", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>");
        builder.Append("&gt;&gt;开奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>第" + ub.GetSub("BbsGsqiNum", xmlPath) + "期开奖</b>");
        builder.Append(Out.Tab("</div>", ""));

        string qiNum = Utils.GetRequest("qiNum", "all", 2, @"^[0-9]\d*$", "期数错误");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            string strText = "平.码1:,平.码2:,平.码3:,平.码4:,平.码5:,平.码6:,特别号码:,,,,";
            string strName = "pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,sNum,forumid,qiNum,act,info";
            string strType = "num,num,num,num,num,num,num,hidden,hidden,hidden,hidden";
            string strValu = "'''''''" + forumid + "'" + qiNum + "'open'ok";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖|reset,Gsopen.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("Gsopen.aspx?forumid=" + forumid + "") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">&lt;&lt;返回版务</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string sNum = Utils.GetRequest("sNum", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "特别号码填写错误");
            string pNum1 = Utils.GetRequest("pNum1", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码1填写错误");
            string pNum2 = Utils.GetRequest("pNum2", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码2填写错误");
            string pNum3 = Utils.GetRequest("pNum3", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码3填写错误");
            string pNum4 = Utils.GetRequest("pNum4", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码4填写错误");
            string pNum5 = Utils.GetRequest("pNum5", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码5填写错误");
            string pNum6 = Utils.GetRequest("pNum6", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码6填写错误");
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请确定第" + qiNum + "期开奖结果:<br />" + pNum1 + "-" + pNum2 + "-" + pNum3 + "-" + pNum4 + "-" + pNum5 + "-" + pNum6 + "特" + sNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "sNum,pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,forumid,act,qiNum,backurl";
            string strValu = "" + sNum + "'" + pNum1 + "'" + pNum2 + "'" + pNum3 + "'" + pNum4 + "'" + pNum5 + "'" + pNum6 + "'" + forumid + "'opensave'" + qiNum + "'" + Utils.getPage(0) + "";
            string strOthe = "确认开奖,Gsopen.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));

            builder.Append(Out.Tab("<div>", " "));
            builder.Append(" <a href=\"" + Utils.getUrl("Gsopen.aspx?act=open&amp;forumid=" + forumid + "&amp;qiNum=" + qiNum + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    #endregion

    #region 确定开奖 OpenSavePage
    private void OpenSavePage(int forumid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string GsAdminID = ub.GetSub("BbsGsAdminID", xmlPath);
        if (!("#" + GsAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("权限不足", "");
        }
        DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
        int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
        if (GsqiNum == 0)
        {
            Utils.Error("本期尚未设置", "");
        }
        if (GsStopTime > DateTime.Now)
        {
            //Utils.Error("本期第" + GsqiNum + "期截止时间" + GsStopTime + "，截止时间未到不能开奖", "");
        }
        string qiNum = Utils.GetRequest("qiNum", "post", 2, @"^[0-9]\d*$", "期数错误");
        string sNum = Utils.GetRequest("sNum", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "特别号码填写错误");
        string pNum1 = Utils.GetRequest("pNum1", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码1填写错误");
        string pNum2 = Utils.GetRequest("pNum2", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码2填写错误");
        string pNum3 = Utils.GetRequest("pNum3", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码3填写错误");
        string pNum4 = Utils.GetRequest("pNum4", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码4填写错误");
        string pNum5 = Utils.GetRequest("pNum5", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码5填写错误");
        string pNum6 = Utils.GetRequest("pNum6", "post", 2, @"^(0)?[1-9]$|^[1-4]([0-9])?$", "平.码6填写错误");

        DataSet ds = null;//ID,Types,qiNum,ForumID,BID,UsID,UsName,Notes,AddTime,IsWin,state
        if (new BCW.User.ForumInc().IsForum113(forumid) == true)//六肖
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0 and ','+Notes+',' Like '%," + GetSX(sNum) + ",%'");

        }
        else if (new BCW.User.ForumInc().IsForum114(forumid) == true)//波色
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0");

        }
        else if (new BCW.User.ForumInc().IsForum115(forumid) == true)//大小
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0 and Notes='" + GetDX(sNum) + "'");
        }
        else if (new BCW.User.ForumInc().IsForum121(forumid) == true)//单双
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0 and Notes='" + GetDS(sNum) + "'");
        }
        else if (new BCW.User.ForumInc().IsForum122(forumid) == true)//家野
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0 and Notes='" + GetQS(sNum) + "'");
        }
        else if (new BCW.User.ForumInc().IsForum116(forumid) == true)//平特一肖
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0 and (Notes='" + GetSX(sNum) + "' OR Notes='" + GetSX(pNum1) + "' OR Notes='" + GetSX(pNum2) + "' OR Notes='" + GetSX(pNum3) + "' OR Notes='" + GetSX(pNum4) + "' OR Notes='" + GetSX(pNum5) + "' OR Notes='" + GetSX(pNum6) + "')");
        }
        else if (new BCW.User.ForumInc().IsForum117(forumid) == true)//五不中
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0");
        }
        else if (new BCW.User.ForumInc().IsForum119(forumid) == true)//五尾
        {
            ds = new BCW.BLL.Forumvote().GetList("ID,BID,UsID,UsName,Notes", "ForumID=" + forumid + " and qiNum=" + qiNum + " and state=0");
        }
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int BID = int.Parse(ds.Tables[0].Rows[i]["BID"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                string Notes = ds.Tables[0].Rows[i]["Notes"].ToString();

                bool IsWin = false;
                if (new BCW.User.ForumInc().IsForum113(forumid) == true)//六肖
                {
                    IsWin = true;
                }
                else if (new BCW.User.ForumInc().IsForum114(forumid) == true)//波色
                {
                    string bs = "";
                    if (Notes.Contains("波"))
                        bs = GetBS(sNum);
                    else
                        bs = GetBBS(sNum);

                    if (Notes.Contains(bs))
                        IsWin = true;
                }
                else if (new BCW.User.ForumInc().IsForum115(forumid) == true)//大小
                {
                    IsWin = true;
                }
                else if (new BCW.User.ForumInc().IsForum121(forumid) == true)//单双
                {
                    IsWin = true;
                }
                else if (new BCW.User.ForumInc().IsForum122(forumid) == true)//家野
                {
                    IsWin = true;
                }
                else if (new BCW.User.ForumInc().IsForum116(forumid) == true)//平特一肖
                {
                    IsWin = true;
                }
                else if (new BCW.User.ForumInc().IsForum117(forumid) == true)//五不中
                {
                    IsWin = true;
                    string BVote = "," + Convert.ToInt32(sNum) + "," + Convert.ToInt32(pNum1) + "," + Convert.ToInt32(pNum2) + "," + Convert.ToInt32(pNum3) + "," + Convert.ToInt32(pNum4) + "," + Convert.ToInt32(pNum5) + "," + Convert.ToInt32(pNum6) + ",";
                    string[] temp = Notes.Split(',');
                    for (int k = 0; k < temp.Length; k++)
                    {
                        if (BVote.Contains("," + Convert.ToInt32(temp[k]) + ","))
                        {
                            IsWin = false;
                            break;
                        }
                    }
                }
                else if (new BCW.User.ForumInc().IsForum119(forumid) == true)//五尾
                {
                    string ws = Utils.Right(sNum, 1) + "尾";
                    if (("," + Notes + ",").Contains("," + ws + ","))
                    {
                        IsWin = true;
                    }

                }
                //------------公共部分---------------
                if (IsWin)
                {
                    int glznum = 1;
                    //if (new BCW.BLL.Forumvote().Exists(forumid, BID, UsID))
                    //{
                    //    glznum = 1;
                    //}
                    //更新中奖
                    new BCW.BLL.Forumvote().UpdateIsWin(ID, 1);
                    //增加总中奖记录数和连中、月中
                    int MonthCount = new BCW.BLL.Forumvote().GetMonthCount(forumid, BID, UsID);
                    new BCW.BLL.Text().UpdategGsNum(BID, 1, glznum, MonthCount);

                }
                //------------5连中或近6期中5加精,近6期错2取消精华------------

                int winnum = 0;
                int lostnum = 0;
                DataSet ds2 = new BCW.BLL.Forumvote().GetList("TOP 6 IsWin", "ForumID=" + forumid + " and BID=" + BID + " and UsID=" + UsID + " order by id desc");
                if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                    {
                        int iswin = int.Parse(ds2.Tables[0].Rows[k]["IsWin"].ToString());
                        if (iswin == 1)
                            winnum++;
                        else
                            lostnum++;
                    }

                }
                //帖子加精状态
                int IsGood = new BCW.BLL.Text().GetIsGood(BID);
                //加精操作，前提是非精华的帖子
                if (IsGood == 0)
                {
                    int Getglznum = new BCW.BLL.Text().Getglznum(BID);
                    if (Getglznum >= 5 || winnum >= 5)
                    {
                        new BCW.BLL.Text().UpdateIsGood(BID, 1);
                        //new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_GoodText, UsID);
                        string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + BID + "]《" + new BCW.BLL.Text().GetTitle(BID) + "》[/url]6期中5，被系统加为精华!";
                        new BCW.BLL.Forumlog().Add(1, forumid, BID, "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "[/url]的" + strLog);
                        new BCW.BLL.Guest().Add(0, UsID, UsName, "您的" + strLog);
                    }
                }
                else
                {
                    //去精操作
                    if (lostnum >= 2)
                    {
                        new BCW.BLL.Text().UpdateIsGood(BID, 0);
                        string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + BID + "]《" + new BCW.BLL.Text().GetTitle(BID) + "》[/url]6期错2，被系统去掉精华!";
                        new BCW.BLL.Forumlog().Add(1, forumid, BID, "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "[/url]的" + strLog);
                        new BCW.BLL.Guest().Add(0, UsID, UsName, "您的" + strLog);
                    }
                }

                //------------5连中或近6期中5加精,近6期错2取消精华------------
            }

        }
        //更新本论坛本期全部开奖结束
        string stat = "特" + sNum + ",平" + pNum1 + "-" + pNum2 + "-" + pNum3 + "-" + pNum4 + "-" + pNum5 + "-" + pNum6 + "";
        new BCW.BLL.Forumvote().UpdateState(Convert.ToInt32(qiNum), forumid, 1, stat);
        Utils.Success("开奖", "本坛开奖成功..", Utils.getUrl("Gsopen.aspx?forumid=" + forumid + ""), "1");

    }
    #endregion

    #region 开奖首页 ReloadPage
    private void ReloadPage(int forumid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string GsAdminID = ub.GetSub("BbsGsAdminID", xmlPath);
        if (!("#" + GsAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("权限不足", "");
        }

        int qiNum = int.Parse(Utils.GetRequest("qiNum", "all", 1, @"^[0-9]\d*$", "0"));
        if (qiNum == 0)
            qiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>");
        builder.Append("&gt;&gt;高手");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>本期第" + qiNum + "期,截止时间" + ub.GetSub("BbsGsStopTime", xmlPath) + "</b>");
        string sNum = new BCW.BLL.Forumvote().GetsNum(forumid, qiNum);
        if (sNum != "")
            builder.Append("<br />本期已开奖:" + sNum + "");
        else
            builder.Append("<br /><a href=\"" + Utils.getUrl("Gsopen.aspx?act=open&amp;forumid=" + forumid + "&amp;qiNum=" + qiNum + "") + "\">马上开奖&gt;&gt;</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("==第" + qiNum + "期参赛帖子==");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "forumid", "qiNum", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Forumid=" + forumid + " and Types=8 and IsDel=0 and Gqinum=" + qiNum + "";
        //排序条件
        strOrder = "Istop Desc,ReTime Desc";
        // 开始读取列表
        IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTexts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listText.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Text n in listText)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append(BCW.User.AppCase.CaseIsTop(n.IsTop));
                builder.Append(BCW.User.AppCase.CaseIsGood(n.IsGood));
                builder.Append(BCW.User.AppCase.CaseIsRecom(n.IsRecom));
                builder.Append(BCW.User.AppCase.CaseIsLock(n.IsLock));
                builder.Append(BCW.User.AppCase.CaseIsOver(n.IsOver));
                builder.Append(BCW.User.AppCase.CaseText(n.Types));
                string TextTab = string.Empty;

                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid={0}&amp;bid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}.{3}{4}</a><br />", n.ForumId, n.ID, (pageIndex - 1) * pageSize + k, TextTab, n.Title);
                builder.Append("" + n.UsName);
                builder.AppendFormat("/阅{0}/回<a href=\"" + Utils.getUrl("/bbs/reply.aspx?forumid={1}&amp;bid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", n.ReadNum, n.ForumId, n.ID, n.ReplyNum);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "<br />没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">&lt;&lt;返回版务</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region 得到大小(此处49不打和) GetDX
    /// <summary>
    /// 得到大小(此处49不打和)
    /// </summary>
    private string GetDX(string sNum)
    {
        string ForDX = string.Empty;
        if (Convert.ToInt32(sNum) <= 24)
            ForDX = "小数";
        else
            ForDX = "大数";

        return ForDX;
    }
    #endregion

    #region 得到单双 GetDS
    /// <summary>
    /// 得到单双(此处49不打和)
    /// </summary>
    private string GetDS(string sNum)
    {
        string ForDS = string.Empty;

        if (Convert.ToInt32(sNum) % 2 == 0)
            ForDS = "双数";
        else
            ForDS = "单数";

        return ForDS;
    }
    #endregion

    #region 得到家禽野兽 GetQS
    /// <summary>
    /// 得到家禽野兽
    /// </summary>
    private string GetQS(string num)
    {
        string JQ = "牛马羊鸡狗猪";
        string YS = "鼠猴兔虎龙蛇";
        string SX = GetSX(num);
        if (JQ.Contains(SX))
        {
            return "家禽";
        }
        else if (YS.Contains(SX))
        {
            return "野兽";
        }
        return "囧";
    }
    #endregion

    #region 得到红波蓝波 GetBS
    private string GetBS(string sNum)
    {
        string ForBS = string.Empty;
        if (("#" + ub.GetSub("Sixred", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForBS = "红波";
        }
        else if (("#" + ub.GetSub("Sixblue", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForBS = "蓝波";
        }
        else if (("#" + ub.GetSub("Sixgreen", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForBS = "绿波";
        }

        return ForBS;
    }
    #endregion

    #region 得到波色 GetBBS
    /// <summary>
    /// 得到波色
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    private string GetBBS(string sNum)
    {
        string ForBS = string.Empty;
        if (("#" + ub.GetSub("Sixred", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForBS = "红";
        }
        else if (("#" + ub.GetSub("Sixblue", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForBS = "蓝";
        }
        else if (("#" + ub.GetSub("Sixgreen", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForBS = "绿";
        }
        string ForDS = string.Empty;

        if (Convert.ToInt32(sNum) % 2 == 0)
            ForDS = "双";
        else
            ForDS = "单";

        return ForBS + ForDS;
    }
    #endregion

    #region 得到生肖 GetSX
    /// <summary>
    /// 得到生肖
    /// </summary>
    private string GetSX(string sNum)
    {
        //生肖
        string ForSX = string.Empty;
        if (("#" + ub.GetSub("Sixsx1", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "鼠";
        }
        else if (("#" + ub.GetSub("Sixsx2", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "牛";
        }
        else if (("#" + ub.GetSub("Sixsx3", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "虎";
        }
        else if (("#" + ub.GetSub("Sixsx4", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "兔";
        }
        else if (("#" + ub.GetSub("Sixsx5", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "龙";
        }
        else if (("#" + ub.GetSub("Sixsx6", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "蛇";
        }
        else if (("#" + ub.GetSub("Sixsx7", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "马";
        }
        else if (("#" + ub.GetSub("Sixsx8", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "羊";
        }
        else if (("#" + ub.GetSub("Sixsx9", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "猴";
        }
        else if (("#" + ub.GetSub("Sixsx10", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "鸡";
        }
        else if (("#" + ub.GetSub("Sixsx11", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "狗";
        }
        else if (("#" + ub.GetSub("Sixsx12", xmlPath2) + "#").Contains("#" + Convert.ToInt32(sNum) + "#"))
        {
            ForSX = "猪";
        }
        return ForSX;
    }
    #endregion

}