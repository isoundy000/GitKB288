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

/// <summary>
/// 增加串串搜索人工开奖重开奖等功能
/// 黄国军 20160630
/// 修改串串退注不删除
/// 黄国军 20160620
/// 增加单场球赛连接
/// 黄国军 20160122
/// 增加串串撤销功能
/// 黄国军 20160328
/// </summary>
public partial class Manage_guess2_super : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "超级竞猜(串串)管理";
        string act = "";
        act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "fsview":
                FsViewPage();       //复式详细
                break;
            case "view":
                ViewPage();         //单场详细
                break;
            case "back":            //撤销串串
                BackPage();
                break;
            case "manual":          //人工或自动开奖开关
            case "auto":
                MatSuperPage();
                break;
            case "MatSupersave":    //保存开奖状态
                MatSupersavePage();
                break;
            case "backsave":
                BackSavePage();
                break;
            case "open":            //人工开串串
                OpenPage();
                break;
            case "reopen":          //重开奖
                ReopenPage();
                break;
            case "getlock":         //查看详情
                GetLockPage();
                break;
            default:
                ReloadPage();       //串关管理
                break;
        }
    }

    #region 人工开串串 OpenPage
    /// <summary>
    /// 人工开串串
    /// </summary>
    private void OpenPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "ALL", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "ALL", 2, @"^[0-9]\d*$", "UsID无效"));
        string ok = Utils.GetRequest("ok", "ALL", 3, "", "");
        if (!new TPR2.BLL.guess.Super().Exists(gid, uid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(gid);

        if (model.Status == 0)
        {
            if (ok == "")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("人工开奖串串(ID" + gid + ")");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("开奖串串(ID" + gid + ")<br />开奖前提必须要该串全部场次均已获得比分才可开奖成功");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "是否开奖" + Out.SysUBB("串串(ID" + model.ID + ")").Replace("/", "／").Replace(",", "，") + "竞猜/,,,,";
                string strName = "gid,uid,act,ok,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden";
                string strValu = gid + "'" + uid + "'open'ok'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确认操作,super.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                #region 赋值初期
                bool isover = true;
                int ID = model.ID;
                string BID = model.BID;
                string PID = model.PID;
                string Odds = model.Odds;
                int UsID = model.UsID;
                string UsName = model.UsName;
                double PayCent = double.Parse(model.PayCent.ToString());
                int p_isfs = model.p_isfs;

                string[] sBID = Regex.Split(Utils.Mid(BID, 2, BID.Length), "##");
                string[] sPID = Regex.Split(Utils.Mid(PID, 2, PID.Length), "##");
                string[] sOdds = Regex.Split(Utils.Mid(Odds, 2, Odds.Length), "##");
                #endregion

                #region 比赛是否已结束
                for (int j = 0; j < sBID.Length; j++)
                {
                    int cgid = Convert.ToInt32(sBID[j]);

                    //比赛是否已结束
                    isover = BCW.Data.SqlHelper.Exists("select count(1) from tb_TBaList where ID=" + cgid + " and p_active>0");
                    if (isover == false)
                        break;
                }
                #endregion

                #region 如果各场次已结束则进行开奖
                if (isover)
                {
                    if (p_isfs > 0)
                    {
                        //复式
                        #region 分析复式投注的总注数列表
                        string strBID = "";
                        List<string> listNum = new Combination().GetCombination2(sBID.Length, p_isfs, sBID);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                strBID += "##" + n.Replace(",", "@");
                            }
                        }
                        string strPID = "";
                        List<string> listNum2 = new Combination().GetCombination2(sPID.Length, p_isfs, sPID);
                        if (listNum2.Count > 0)
                        {
                            foreach (string n in listNum2)
                            {
                                strPID += "##" + n.Replace(",", "@");
                            }
                        }
                        string strOdds = "";
                        List<string> listNum3 = new Combination().GetCombination2(sOdds.Length, p_isfs, sOdds);
                        if (listNum3.Count > 0)
                        {
                            foreach (string n in listNum3)
                            {
                                strOdds += "##" + n.Replace(",", "@");
                            }
                        }
                        string[] sBID2 = Regex.Split(Utils.Mid(strBID, 2, strBID.Length), "##");
                        string[] sPID2 = Regex.Split(Utils.Mid(strPID, 2, strPID.Length), "##");
                        string[] sOdds2 = Regex.Split(Utils.Mid(strOdds, 2, strOdds.Length), "##");
                        string fsStat = "";
                        double cents = 0;
                        #endregion

                        #region J循环
                        for (int j = 0; j < sBID2.Length; j++)
                        {
                            string[] sBID3 = Regex.Split(sBID2[j], "@");
                            string[] sPID3 = Regex.Split(sPID2[j], "@");
                            string[] sOdds3 = Regex.Split(sOdds2[j], "@");
                            //-----------------------------------------------------
                            int errNum = 0;
                            int sbNum = 0;
                            string Stats = string.Empty;
                            double pCent = PayCent;

                            #region k循环 判断输赢
                            for (int k = 0; k < sBID3.Length; k++)
                            {
                                int cgid = Convert.ToInt32(sBID3[k]);
                                int pid = Convert.ToInt32(sPID3[k]);
                                double odds = Convert.ToDouble(sOdds3[k]);

                                TPR2.Model.guess.BaList qq = new TPR2.BLL.guess.BaList().GetModel(cgid);

                                int p_type = new TPR2.BLL.guess.BaList().GetPtype(cgid);
                                decimal GetMoney = 0;
                                GetMoney = new TPR2.BLL.guess.BaPay().GetBaPayMoney(pid, UsID);
                                if (p_type == 1)
                                {
                                    #region 类型为1，1为足球
                                    if (GetMoney == 0)//全输
                                    {
                                        Stats += "##全输";
                                        pCent = pCent * 0;
                                    }
                                    else if (GetMoney == 100)//平盘
                                    {

                                        if (qq.p_active == 2)
                                        {
                                            Stats += "##平盘";
                                            pCent = pCent * 1;
                                            errNum++;
                                        }
                                        else
                                        {
                                            Stats += "##走盘";
                                            pCent = pCent * 1;
                                        }
                                    }
                                    else if (GetMoney == 50)//输半
                                    {
                                        Stats += "##输半";
                                        pCent = pCent * 0.5;
                                        sbNum++;
                                    }
                                    else if (GetMoney == Convert.ToDecimal(100 * odds))//全赢
                                    {
                                        Stats += "##全赢";
                                        pCent = pCent * odds;
                                    }
                                    else//赢半
                                    {
                                        Stats += "##赢半";
                                        pCent = pCent * Convert.ToDouble((odds - 1) / 2 + 1);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 除1外的类型，篮球或其他
                                    if (GetMoney == 0)//全输
                                    {
                                        Stats += "##全输";
                                        pCent = pCent * 0;
                                    }
                                    else if (GetMoney > 100)
                                    {
                                        Stats += "##全赢";
                                        pCent = pCent * odds;
                                    }
                                    else if (GetMoney == 100)//平盘
                                    {
                                        if (qq.p_active == 2)
                                        {
                                            Stats += "##平盘";
                                            pCent = pCent * 1;
                                            errNum++;
                                        }
                                        else
                                        {
                                            Stats += "##走盘";
                                            pCent = pCent * 1;
                                        }
                                    }
                                    #endregion
                                }

                            }
                            #endregion

                            if (sBID3.Length - errNum < 3)
                            {
                                pCent = PayCent;
                            }
                            if (sbNum >= 2)
                            {
                                pCent = 0;
                            }

                            fsStat += Stats;
                            cents += pCent;

                        }
                        #endregion

                        #region 更新输赢
                        TPR2.Model.guess.Super modelnew = new TPR2.Model.guess.Super();
                        if (cents == 0)
                        {
                            //更新为输
                            modelnew.Status = 2;
                            modelnew.getMoney = Convert.ToDecimal(-(PayCent + PayCent * p_isfs));
                        }
                        else
                        {
                            //更新为赢
                            modelnew.Status = 1;
                            modelnew.getMoney = Convert.ToDecimal(cents);
                            //发送兑奖内线
                            string strLog = "超级竞猜赛事ID" + ID + "[br]恭喜赢了" + Convert.ToDouble(cents) + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/supercase.aspx]马上兑奖[/url]";
                            new BCW.BLL.Guest().Add(1, UsID, UsName, strLog);
                        }
                        #endregion

                        #region 更新为结束
                        //更新为结束
                        modelnew.ID = ID;
                        modelnew.getOdds = fsStat;
                        new TPR2.BLL.guess.Super().Update4(modelnew);
                        #endregion

                        #region 更新排行
                        //更新排行
                        TPR2.Model.guess.SuperOrder m = new TPR2.Model.guess.SuperOrder();
                        m.UsID = UsID;
                        m.UsName = UsName;
                        if (cents == 0)
                            m.Cent = Convert.ToDecimal(-(PayCent + PayCent * p_isfs));
                        else
                            m.Cent = Convert.ToDecimal(cents);

                        if (!new TPR2.BLL.guess.SuperOrder().Exists(UsID))
                        {
                            new TPR2.BLL.guess.SuperOrder().Add(m);
                        }
                        else
                        {
                            new TPR2.BLL.guess.SuperOrder().Update(m);
                        }
                        #endregion
                    }
                    else
                    {
                        //非复式 投注处理
                        int errNum = 0;
                        int sbNum = 0;
                        string Stats = string.Empty;
                        double pCent = PayCent;
                        #region k循环判断输赢
                        for (int k = 0; k < sBID.Length; k++)
                        {
                            int cgid = Convert.ToInt32(sBID[k]);
                            int pid = Convert.ToInt32(sPID[k]);
                            double odds = Convert.ToDouble(sOdds[k]);
                            TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().GetModel(cgid);

                            decimal GetMoney = 0;
                            GetMoney = new TPR2.BLL.guess.BaPay().GetBaPayMoney(pid, UsID);
                            if (GetMoney == -1)
                                break;

                            if (m.p_type == 1)
                            {
                                #region p_type类型为1的输赢
                                if (GetMoney == 0)//全输
                                {
                                    Stats += "##全输";
                                    pCent = pCent * 0;
                                }
                                else if (GetMoney == 100)//平盘
                                {
                                    if (m.p_active == 2)
                                    {
                                        Stats += "##平盘";
                                        pCent = pCent * 1;
                                        errNum++;
                                    }
                                    else
                                    {
                                        Stats += "##走盘";
                                        pCent = pCent * 1;
                                    }
                                }
                                else if (GetMoney == 50)//输半
                                {
                                    Stats += "##输半";
                                    pCent = pCent * 0.5;
                                    sbNum++;
                                }
                                else if (GetMoney == Convert.ToDecimal(100 * odds))//全赢
                                {
                                    Stats += "##全赢";
                                    pCent = pCent * odds;
                                }
                                else//赢半
                                {
                                    Stats += "##赢半";
                                    pCent = pCent * Convert.ToDouble((odds - 1) / 2 + 1);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 除1外的类型
                                if (GetMoney == 0)//全输
                                {
                                    Stats += "##全输";
                                    pCent = pCent * 0;
                                }
                                else if (GetMoney > 100)
                                {
                                    Stats += "##全赢";
                                    pCent = pCent * odds;
                                }
                                else if (GetMoney == 100)//平盘
                                {
                                    if (m.p_active == 2)
                                    {
                                        Stats += "##平盘";
                                        pCent = pCent * 1;
                                        errNum++;
                                    }
                                    else
                                    {
                                        Stats += "##走盘";
                                        pCent = pCent * 1;
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region 发送内线以及处理更新
                        TPR2.Model.guess.Super modelnew = new TPR2.Model.guess.Super();
                        if (sBID.Length - errNum < 3)
                        {
                            //无效返回本金
                            modelnew.Status = 1;
                            modelnew.getMoney = Convert.ToDecimal(PayCent);
                            //发送兑奖内线
                            string strLog = "超级竞猜赛事ID" + ID + "[br]串串无效系统退回本金" + Convert.ToDouble(PayCent) + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/supercase.aspx]马上兑奖[/url]";
                            new BCW.BLL.Guest().Add(1, UsID, UsName, strLog);
                        }
                        else
                        {
                            if (pCent == 0 || sbNum >= 2)
                            {
                                //更新为输
                                modelnew.Status = 2;
                                modelnew.getMoney = Convert.ToDecimal(-PayCent);
                            }
                            else
                            {
                                //更新为赢
                                modelnew.Status = 1;
                                modelnew.getMoney = Convert.ToDecimal(pCent);
                                //发送兑奖内线
                                string strLog = "超级竞猜赛事ID" + ID + "[br]恭喜赢了" + Convert.ToDouble(pCent) + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/supercase.aspx]马上兑奖[/url]";
                                new BCW.BLL.Guest().Add(1, UsID, UsName, strLog);

                            }
                        }
                        #endregion

                        #region 更新为结束
                        //更新为结束
                        modelnew.ID = ID;
                        modelnew.getOdds = Stats;
                        new TPR2.BLL.guess.Super().Update4(modelnew);
                        #endregion

                        #region 更新排行
                        //更新排行
                        TPR2.Model.guess.SuperOrder n = new TPR2.Model.guess.SuperOrder();
                        n.UsID = UsID;
                        n.UsName = UsName;
                        if (pCent == 0)
                            n.Cent = Convert.ToDecimal(-PayCent);
                        else
                            n.Cent = Convert.ToDecimal(pCent);

                        if (!new TPR2.BLL.guess.SuperOrder().Exists(UsID))
                        {
                            new TPR2.BLL.guess.SuperOrder().Add(n);
                        }
                        else
                        {
                            new TPR2.BLL.guess.SuperOrder().Update(n);
                        }
                        #endregion
                    }
                    Utils.Success("串串ID" + model.ID + "开奖成功", "串串开奖成功...", Utils.getPage("super.aspx"), "1");
                }
                #endregion
                else
                {
                    Utils.Success("串串ID" + model.ID + "开奖失败,还有场次没有完场.串串开奖失败,..", "");
                }

            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Utils.Error("串串ID" + model.ID + "已开奖成功", "");
        }
    }
    #endregion

    #region 人工或自动开奖开关 MatSuperPage
    /// <summary>
    /// 人工或自动开奖开关
    /// </summary>
    private void MatSuperPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "get", 2, @"^[0-9]\d*$", "UsID无效"));

        if (!new TPR2.BLL.guess.Super().Exists(gid, uid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.p_Auto == 0)
        {
            Master.Title = "更改串串(ID" + gid + ")为人工开奖";

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("更改串串(ID" + gid + ")为人工开奖");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "您正在更改" + Out.SysUBB("串串(ID" + model.ID + ")").Replace("/", "／").Replace(",", "，") + "竞猜为人工开奖模式/,,,,";
            string strName = "Content,gid,uid,act,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "'" + gid + "'" + uid + "'MatSupersave'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认操作,super.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (model.p_Auto == 1)
        {
            Master.Title = "更改串串(ID" + gid + ")为自动开奖";

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("更改串串(ID" + gid + ")为自动开奖");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "您正在更改" + Out.SysUBB("串串(ID" + model.ID + ")").Replace("/", "／").Replace(",", "，") + "竞猜为自动开奖模式/,,,,";
            string strName = "Content,gid,uid,act,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "'" + gid + "'" + uid + "'MatSupersave'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认操作,super.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 人工或自动开奖开关确认 BackSavePage
    /// <summary>
    /// 人工或自动开奖开关
    /// </summary>
    private void MatSupersavePage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "post", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "post", 2, @"^[0-9]\d*$", "UsID无效"));

        if (!new TPR2.BLL.guess.Super().Exists(gid, uid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,200}$", "原因限200字内，可以留空");
        string Msgtxt = string.Empty;
        if (model.p_Auto == 0)
        {
            Msgtxt = "串串ID(" + model.ID + ")已设为人工开奖";
            model.p_Auto = 1;
        }
        else
        {
            Msgtxt = "串串ID(" + model.ID + ")已设为自动开奖，" + Content + "";
            model.p_Auto = 0;
        }

        //new BCW.BLL.Guest().Add(Convert.ToInt32(model.UsID), model.UsName, Msgtxt);
        new TPR2.BLL.guess.Super().Update6(model);
        Utils.Success("串关设置成功", "串关设置成功...", Utils.getPage("super.aspx"), "1");
    }
    #endregion

    #region 串串重开奖 ReopenPage
    /// <summary>
    /// 人工开串串
    /// </summary>
    private void ReopenPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "ALL", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "ALL", 2, @"^[0-9]\d*$", "UsID无效"));
        string ok = Utils.GetRequest("ok", "ALL", 3, "", "");
        int ManageId = new BCW.User.Manage().IsManageLogin();

        if (!new TPR2.BLL.guess.Super().Exists(gid, uid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(gid);

        if (model.Status != 0)
        {
            if (ok == "")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("重开奖串串(ID" + gid + ")");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("重开奖串串(ID" + gid + ")<br />开奖前提必须要该串全部场次均已获得比分才可开奖成功");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "是否重开奖" + Out.SysUBB("串串(ID" + model.ID + ")").Replace("/", "／").Replace(",", "，") + "竞猜/,,,,";
                string strName = "gid,uid,act,ok,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden";
                string strValu = gid + "'" + uid + "'reopen'ok'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确认操作," + Utils.getUrl("super.aspx") + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("重要：重开奖系统自动扣回已经兑奖的币并进行新一轮的开奖,如果币不够扣，即自动禁该会员的金融系统并记录<a href=\"" + Utils.getUrl("../forumlog.aspx?act=gameowe") + "\">欠币日志</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                //游戏日志记录                
                string[] p_pageArr = { "act", "ac", "gid", "uid", "ok" };
                BCW.User.GameLog.GameLogPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号重新开奖串串ID" + gid, gid);

                #region 扣回币值和排行榜单
                if (model != null)
                {
                    #region 扣回币值和排行榜单 处理
                    int payusid = model.UsID;
                    string payusname = model.UsName;
                    long payCent = Convert.ToInt64(model.PayCent);
                    long p_getMoney = Convert.ToInt64(model.getMoney);
                    long gold = 0;
                    long cMoney = 0;    //差多少
                    long sMoney = 0;    //实扣
                    //串串貌似只能下第一个币种
                    gold = new BCW.BLL.User().GetGold(payusid);
                    if (p_getMoney > gold)
                    {
                        cMoney = p_getMoney - gold;
                        sMoney = gold;
                    }
                    else
                    {
                        sMoney = p_getMoney;
                    }

                    //重开奖的在本场没兑奖时就没显示在欠币日志，
                    //操作币并内线通知
                    new BCW.BLL.User().UpdateiGold(payusid, payusname, -sMoney, "串串ID" + gid + "重开奖，扣除已兑奖" + sMoney + ub.Get("SiteBz") + "");
                    //发送内线
                    string strGuess = "串串ID" + gid + "重开奖，你欠下系统的" + p_getMoney + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额，实扣" + sMoney + "" + ub.Get("SiteBz") + ".[br]如果您的" + ub.Get("SiteBz") + "不足，系统将您帐户冻结，直到成功扣除为止。";
                    new BCW.BLL.Guest().Add(1, payusid, payusname, strGuess);

                    //如果币不够扣则记录日志并冻结IsFreeze
                    if (cMoney > 0)
                    {
                        BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                        owe.Types = 1;
                        owe.UsID = payusid;
                        owe.UsName = payusname;
                        owe.Content = "串串ID" + gid + "重开奖,结果发生变化";
                        owe.OweCent = cMoney;
                        owe.BzType = 1;
                        owe.EnId = gid;
                        owe.AddTime = DateTime.Now;
                        new BCW.BLL.Gameowe().Add(owe);
                        //冻结账户
                        new BCW.BLL.User().UpdateIsFreeze(payusid, 1);
                    }

                    //取消得到的排行
                    TPR2.Model.guess.BaOrder objBaOrder = new TPR2.Model.guess.BaOrder();
                    objBaOrder.Orderusid = payusid;
                    objBaOrder.Orderusname = payusname;
                    objBaOrder.Orderfanum = 0;
                    objBaOrder.Orderjbnum = -(p_getMoney - payCent);
                    objBaOrder.Orderbanum = -1;
                    objBaOrder.Orderstats = 1;
                    new TPR2.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                    //恢复到未开奖状态
                    model.Status = 0;
                    new TPR2.BLL.guess.Super().Update4(model);
                    //恢复未兑奖状态
                    new TPR2.BLL.guess.Super().UpdateResetCase(model.ID);
                    #endregion
                }
                #endregion

                if (model.Status == 0)
                {
                    #region 赋值初期
                    bool isover = true;
                    int ID = model.ID;
                    string BID = model.BID;
                    string PID = model.PID;
                    string Odds = model.Odds;
                    int UsID = model.UsID;
                    string UsName = model.UsName;
                    double PayCent = double.Parse(model.PayCent.ToString());
                    int p_isfs = model.p_isfs;

                    string[] sBID = Regex.Split(Utils.Mid(BID, 2, BID.Length), "##");
                    string[] sPID = Regex.Split(Utils.Mid(PID, 2, PID.Length), "##");
                    string[] sOdds = Regex.Split(Utils.Mid(Odds, 2, Odds.Length), "##");
                    #endregion

                    #region 比赛是否已结束
                    for (int j = 0; j < sBID.Length; j++)
                    {
                        int cgid = Convert.ToInt32(sBID[j]);

                        //比赛是否已结束
                        isover = BCW.Data.SqlHelper.Exists("select count(1) from tb_TBaList where ID=" + cgid + " and p_active>0");
                        if (isover == false)
                            break;
                    }
                    #endregion

                    #region 如果各场次已结束则进行重开奖
                    if (isover)
                    {
                        #region 重开奖
                        if (p_isfs > 0)
                        {
                            //复式
                            #region 分析复式投注的总注数列表
                            string strBID = "";
                            List<string> listNum = new Combination().GetCombination2(sBID.Length, p_isfs, sBID);
                            if (listNum.Count > 0)
                            {
                                foreach (string n in listNum)
                                {
                                    strBID += "##" + n.Replace(",", "@");
                                }
                            }
                            string strPID = "";
                            List<string> listNum2 = new Combination().GetCombination2(sPID.Length, p_isfs, sPID);
                            if (listNum2.Count > 0)
                            {
                                foreach (string n in listNum2)
                                {
                                    strPID += "##" + n.Replace(",", "@");
                                }
                            }
                            string strOdds = "";
                            List<string> listNum3 = new Combination().GetCombination2(sOdds.Length, p_isfs, sOdds);
                            if (listNum3.Count > 0)
                            {
                                foreach (string n in listNum3)
                                {
                                    strOdds += "##" + n.Replace(",", "@");
                                }
                            }
                            string[] sBID2 = Regex.Split(Utils.Mid(strBID, 2, strBID.Length), "##");
                            string[] sPID2 = Regex.Split(Utils.Mid(strPID, 2, strPID.Length), "##");
                            string[] sOdds2 = Regex.Split(Utils.Mid(strOdds, 2, strOdds.Length), "##");
                            string fsStat = "";
                            double cents = 0;
                            #endregion

                            #region J循环
                            for (int j = 0; j < sBID2.Length; j++)
                            {
                                string[] sBID3 = Regex.Split(sBID2[j], "@");
                                string[] sPID3 = Regex.Split(sPID2[j], "@");
                                string[] sOdds3 = Regex.Split(sOdds2[j], "@");
                                //-----------------------------------------------------
                                int errNum = 0;
                                int sbNum = 0;
                                string Stats = string.Empty;
                                double pCent = PayCent;

                                #region k循环 判断输赢
                                for (int k = 0; k < sBID3.Length; k++)
                                {
                                    int cgid = Convert.ToInt32(sBID3[k]);
                                    int pid = Convert.ToInt32(sPID3[k]);
                                    double odds = Convert.ToDouble(sOdds3[k]);

                                    TPR2.Model.guess.BaList qq = new TPR2.BLL.guess.BaList().GetModel(cgid);

                                    int p_type = new TPR2.BLL.guess.BaList().GetPtype(cgid);
                                    decimal GetMoney = 0;
                                    GetMoney = new TPR2.BLL.guess.BaPay().GetBaPayMoney(pid, UsID);
                                    if (p_type == 1)
                                    {
                                        #region 类型为1，1为足球
                                        if (GetMoney == 0)//全输
                                        {
                                            Stats += "##全输";
                                            pCent = pCent * 0;
                                        }
                                        else if (GetMoney == 100)//平盘
                                        {

                                            if (qq.p_active == 2)
                                            {
                                                Stats += "##平盘";
                                                pCent = pCent * 1;
                                                errNum++;
                                            }
                                            else
                                            {
                                                Stats += "##走盘";
                                                pCent = pCent * 1;
                                            }
                                        }
                                        else if (GetMoney == 50)//输半
                                        {
                                            Stats += "##输半";
                                            pCent = pCent * 0.5;
                                            sbNum++;
                                        }
                                        else if (GetMoney == Convert.ToDecimal(100 * odds))//全赢
                                        {
                                            Stats += "##全赢";
                                            pCent = pCent * odds;
                                        }
                                        else//赢半
                                        {
                                            Stats += "##赢半";
                                            pCent = pCent * Convert.ToDouble((odds - 1) / 2 + 1);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 除1外的类型，篮球或其他
                                        if (GetMoney == 0)//全输
                                        {
                                            Stats += "##全输";
                                            pCent = pCent * 0;
                                        }
                                        else if (GetMoney > 100)
                                        {
                                            Stats += "##全赢";
                                            pCent = pCent * odds;
                                        }
                                        else if (GetMoney == 100)//平盘
                                        {
                                            if (qq.p_active == 2)
                                            {
                                                Stats += "##平盘";
                                                pCent = pCent * 1;
                                                errNum++;
                                            }
                                            else
                                            {
                                                Stats += "##走盘";
                                                pCent = pCent * 1;
                                            }
                                        }
                                        #endregion
                                    }

                                }
                                #endregion

                                if (sBID3.Length - errNum < 3)
                                {
                                    pCent = PayCent;
                                }
                                if (sbNum >= 2)
                                {
                                    pCent = 0;
                                }

                                fsStat += Stats;
                                cents += pCent;
                            }
                            #endregion

                            #region 更新输赢
                            if (cents == 0)
                            {
                                //更新为输
                                model.Status = 2;
                                model.getMoney = Convert.ToDecimal(-(PayCent + PayCent * p_isfs));
                            }
                            else
                            {
                                //更新为赢
                                model.Status = 1;
                                model.getMoney = Convert.ToDecimal(cents);
                                //发送兑奖内线
                                string strLog = "超级竞猜赛事ID" + ID + "[br]恭喜赢了" + Convert.ToDouble(cents) + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/supercase.aspx]马上兑奖[/url]";
                                new BCW.BLL.Guest().Add(1, UsID, UsName, strLog);
                            }
                            #endregion

                            #region 更新为结束
                            //更新为结束
                            model.ID = ID;
                            model.getOdds = fsStat;
                            new TPR2.BLL.guess.Super().Update4(model);
                            #endregion

                            #region 更新排行
                            //更新排行
                            TPR2.Model.guess.SuperOrder m = new TPR2.Model.guess.SuperOrder();
                            m.UsID = UsID;
                            m.UsName = UsName;
                            if (cents == 0)
                                m.Cent = Convert.ToDecimal(-(PayCent + PayCent * p_isfs));
                            else
                                m.Cent = Convert.ToDecimal(cents);

                            if (!new TPR2.BLL.guess.SuperOrder().Exists(UsID))
                            {
                                new TPR2.BLL.guess.SuperOrder().Add(m);
                            }
                            else
                            {
                                new TPR2.BLL.guess.SuperOrder().Update(m);
                            }
                            #endregion
                        }
                        else
                        {
                            //非复式 投注处理
                            int errNum = 0;
                            int sbNum = 0;
                            string Stats = string.Empty;
                            double pCent = PayCent;
                            #region k循环判断输赢
                            for (int k = 0; k < sBID.Length; k++)
                            {
                                int cgid = Convert.ToInt32(sBID[k]);
                                int pid = Convert.ToInt32(sPID[k]);
                                double odds = Convert.ToDouble(sOdds[k]);
                                TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().GetModel(cgid);

                                decimal GetMoney = 0;
                                GetMoney = new TPR2.BLL.guess.BaPay().GetBaPayMoney(pid, UsID);
                                if (GetMoney == -1)
                                    break;

                                if (m.p_type == 1)
                                {
                                    #region p_type类型为1的输赢
                                    if (GetMoney == 0)//全输
                                    {
                                        Stats += "##全输";
                                        pCent = pCent * 0;
                                    }
                                    else if (GetMoney == 100)//平盘
                                    {
                                        if (m.p_active == 2)
                                        {
                                            Stats += "##平盘";
                                            pCent = pCent * 1;
                                            errNum++;
                                        }
                                        else
                                        {
                                            Stats += "##走盘";
                                            pCent = pCent * 1;
                                        }
                                    }
                                    else if (GetMoney == 50)//输半
                                    {
                                        Stats += "##输半";
                                        pCent = pCent * 0.5;
                                        sbNum++;
                                    }
                                    else if (GetMoney == Convert.ToDecimal(100 * odds))//全赢
                                    {
                                        Stats += "##全赢";
                                        pCent = pCent * odds;
                                    }
                                    else//赢半
                                    {
                                        Stats += "##赢半";
                                        pCent = pCent * Convert.ToDouble((odds - 1) / 2 + 1);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 除1外的类型
                                    if (GetMoney == 0)//全输
                                    {
                                        Stats += "##全输";
                                        pCent = pCent * 0;
                                    }
                                    else if (GetMoney > 100)
                                    {
                                        Stats += "##全赢";
                                        pCent = pCent * odds;
                                    }
                                    else if (GetMoney == 100)//平盘
                                    {
                                        if (m.p_active == 2)
                                        {
                                            Stats += "##平盘";
                                            pCent = pCent * 1;
                                            errNum++;
                                        }
                                        else
                                        {
                                            Stats += "##走盘";
                                            pCent = pCent * 1;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region 发送内线以及处理更新
                            if (sBID.Length - errNum < 3)
                            {
                                //无效返回本金
                                model.Status = 1;
                                model.getMoney = Convert.ToDecimal(PayCent);
                                //发送兑奖内线
                                string strLog = "超级竞猜赛事ID" + ID + "[br]串串无效系统退回本金" + Convert.ToDouble(PayCent) + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/supercase.aspx]马上兑奖[/url]";
                                new BCW.BLL.Guest().Add(1, UsID, UsName, strLog);
                            }
                            else
                            {
                                if (pCent == 0 || sbNum >= 2)
                                {
                                    //更新为输
                                    model.Status = 2;
                                    model.getMoney = Convert.ToDecimal(-PayCent);
                                }
                                else
                                {
                                    //更新为赢
                                    model.Status = 1;
                                    model.getMoney = Convert.ToDecimal(pCent);
                                    //发送兑奖内线
                                    string strLog = "超级竞猜赛事ID" + ID + "[br]恭喜赢了" + Convert.ToDouble(pCent) + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/supercase.aspx]马上兑奖[/url]";
                                    new BCW.BLL.Guest().Add(1, UsID, UsName, strLog);

                                }
                            }
                            #endregion

                            #region 更新为结束
                            //更新为结束
                            model.ID = ID;
                            model.getOdds = Stats;
                            new TPR2.BLL.guess.Super().Update4(model);
                            #endregion

                            #region 更新排行
                            //更新排行
                            TPR2.Model.guess.SuperOrder n = new TPR2.Model.guess.SuperOrder();
                            n.UsID = UsID;
                            n.UsName = UsName;
                            if (pCent == 0)
                                n.Cent = Convert.ToDecimal(-PayCent);
                            else
                                n.Cent = Convert.ToDecimal(pCent);

                            if (!new TPR2.BLL.guess.SuperOrder().Exists(UsID))
                            {
                                new TPR2.BLL.guess.SuperOrder().Add(n);
                            }
                            else
                            {
                                new TPR2.BLL.guess.SuperOrder().Update(n);
                            }
                            #endregion
                        }
                        Utils.Success("串串ID" + model.ID + "重开奖成功", "串串重开奖成功...", Utils.getPage("super.aspx"), "1");
                        #endregion
                    }
                    #endregion
                    else
                    {
                        Utils.Success("串串ID" + model.ID + "重开奖失败,还有场次没有完场.串串开奖失败,..", "");
                    }
                }
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Utils.Error("串串ID" + model.ID + "还未开奖成功,请不要重复操作", "");
        }
    }
    #endregion

    #region 串关管理 ReloadPage
    /// <summary>
    /// 串关管理
    /// </summary>
    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("超级竞猜(串串)管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, "", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 0)
        {
            builder.Append("全部|" + Out.waplink(Utils.getUrl("super.aspx?ptype=1&amp;uid=" + uid + ""), "未返") + "|" + Out.waplink(Utils.getUrl("super.aspx?ptype=2&amp;uid=" + uid + ""), "盈利"));
        }
        else if (ptype == 1)
        {
            builder.Append("" + Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "全部") + "|未返|" + Out.waplink(Utils.getUrl("super.aspx?ptype=2&amp;uid=" + uid + ""), "盈利"));
        }
        else
            builder.Append("" + Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "全部") + "|" + Out.waplink(Utils.getUrl("super.aspx?ptype=1&amp;uid=" + uid + ""), "未返") + "|盈利");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (uid > 0)
            strWhere += "UsID=" + uid + " or Bid+'##' like '%##" + uid + "##%' and ";

        strWhere += "IsOpen=1";

        if (ptype == 1)
            strWhere += " and Status=0";
        else
            if (ptype == 2)
                strWhere += " and Status=1";

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
                    sWin = "已退";
                else
                    sWin = "输";

                //20151224 黄国军 增加
                builder.Append("会员ID:" + n.UsID + " 串关ID:" + n.ID + " 时间:" + n.AddTime + "<br />");

                if (n.p_isfs > 0)
                {
                    builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=fsview&amp;uid=" + n.UsID + "&amp;id=" + n.ID + ""), "[" + sWin + "]串竞猜" + Convert.ToDouble(n.PayCent) + "x" + (n.p_isfs + 1) + "" + ub.Get("SiteBz") + "") + "[复式]");
                }
                else
                {
                    builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=view&amp;uid=" + n.UsID + "&amp;id=" + n.ID + ""), "[" + sWin + "]串竞猜" + Convert.ToDouble(n.PayCent) + "" + ub.Get("SiteBz") + "") + "");
                }

                #region 撤销串串单 设置人工开奖
                if (n.Status != 10)
                {
                    builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=back&amp;uid=" + n.UsID + "&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[退]"));
                }

                if (n.Status == 0)
                {
                    if (n.p_Auto == 1)
                    {
                        builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=auto&amp;uid=" + n.UsID + "&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[人]"));
                    }
                    else
                    {
                        builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=manual&amp;uid=" + n.UsID + "&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[自]"));
                    }
                    builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=open&amp;uid=" + n.UsID + "&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[开]"));
                }
                else if (n.Status != 10)
                {
                    builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=reopen&amp;uid=" + n.UsID + "&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[重]"));
                }
                builder.Append("<br />盈利:" + Math.Round(double.Parse(n.getMoney.ToString()), 2).ToString());
                #endregion

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
        string strText = "输入用户或球赛ID:/,";
        string strName = "uid,backurl";
        string strType = "text,hidden";
        string strValu = uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜串串记录," + Utils.getUrl("super.aspx") + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 单场详细 ViewPage
    /// <summary>
    /// 单场详细
    /// </summary>
    private void ViewPage()
    {
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        if (!new TPR2.BLL.guess.Super().Exists(id, uid))
        {
            Utils.Error("不存在的记录..", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "串串记录") + "|查看详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(id);

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + model.UsName + "(" + model.UsID + ")") + "|" + DT.FormatDate(Convert.ToDateTime(model.AddTime), 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

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
        builder.Append("<br />串关ID：" + id + "");
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
            //增加赛事ID连接 黄国军 20160122
            //Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + sBID3[i] + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + sTitle3[i] + "")
            //builder.Append("<br />比赛场次" + (i + 1) + "：" + Title[i] + "(赛事ID:" + BID[i] + "/" + sResult + ")<br />");
            builder.Append("<br />比赛场次" + (i + 1) + "：" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + BID[i] + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + Title[i] + "") + "(赛事ID:" + BID[i] + "/" + sResult + ")<br />");
            builder.Append("比赛时间：" + Times[i] + "<br />");
            builder.Append("" + StTitle[i] + ",");
            builder.Append("赔率:" + Odds[i]);

            if (model.Status > 0)
            {
                if (model.Status == 10)
                {
                    builder.Append("<br />结果:已退");
                }
                else
                {
                    double odds = Convert.ToDouble(Odds[i]);
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
        builder.Append(Out.Tab("<div class=\"text\">", ""));
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
                if (model.p_case == 0)
                    builder.Append("(未兑奖)");
                else
                    builder.Append("(已兑奖)");

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
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "返回上一级") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 复式详细 FsViewPage
    /// <summary>
    /// 复式详细
    /// </summary>
    private void FsViewPage()
    {
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        if (!new TPR2.BLL.guess.Super().Exists(id, uid))
        {
            Utils.Error("不存在的记录..", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "串串记录") + "|复式详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(id);
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + model.UsName + "(" + model.UsID + ")") + "|" + DT.FormatDate(Convert.ToDateTime(model.AddTime), 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

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

        //string getNum = "";
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "返回上一级") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 串串撤销单处理 BackPage
    /// <summary>
    /// 串串撤销单处理
    /// </summary>
    private void BackPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "get", 2, @"^[0-9]\d*$", "UsID无效"));

        if (!new TPR2.BLL.guess.Super().Exists(gid, uid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "撤销串串(ID" + gid + ")押注并退回本金";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("撤销串串(ID" + gid + ")押注并退回本金");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "您正在撤销" + Out.SysUBB("串串(ID" + model.ID + ")").Replace("/", "／").Replace(",", "，") + "竞猜，系统将退回本金处理。/,,,,";
        string strName = "Content,gid,uid,act,backurl";
        string strType = "text,hidden,hidden,hidden,hidden";
        string strValu = "'" + gid + "'" + uid + "'backsave'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "本条撤销并退本金,super.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 串串撤销单确认 BackSavePage
    /// <summary>
    /// 撤销处理
    /// </summary>
    private void BackSavePage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "post", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "post", 2, @"^[0-9]\d*$", "UsID无效"));

        if (!new TPR2.BLL.guess.Super().Exists(gid, uid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.Super model = new TPR2.BLL.guess.Super().GetSuper(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,200}$", "原因限200字内，可以留空");
        string Msgtxt = string.Empty;
        if (Content == "")
        {
            Msgtxt = "串串ID(" + model.ID + ")失败已作退回本金处理";

        }
        else
        {
            Msgtxt = "串串ID(" + model.ID + ")失败已作退回本金处理，" + Content + "";
        }

        new BCW.BLL.Guest().Add(Convert.ToInt32(model.UsID), model.UsName, Msgtxt);
        //退本金

        new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.UsID), model.UsName, Convert.ToInt64(model.PayCent * (model.p_isfs + 1)), Msgtxt);

        //删除处理
        model.Status = 10;
        new TPR2.BLL.guess.Super().Update5(model);
        Utils.Success("撤销串关押注", "撤销串关押注并退回本金成功...", Utils.getPage("super.aspx"), "1");
    }
    #endregion

    #region 查看详情 GetLockPage
    private void GetLockPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = new TPR2.BLL.guess.Super().GetList("ID,UsID,Title,SP,getOdds", "IsOpen=1");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Title"].ToString();

                string SP = ds.Tables[0].Rows[i]["SP"].ToString();
                string getOdds = ds.Tables[0].Rows[i]["getOdds"].ToString();
                int iSP = Utils.GetStringNum(SP, "##");
                int igetOdds = Utils.GetStringNum(getOdds, "##");

                if (iSP != igetOdds)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("标识ID：" + ID + "<br />");
                    builder.Append("用户ID：" + UsID + "<br />");
                    builder.Append("标题：" + Title + "<br />");
                    builder.Append("标题：" + SP + "<br />");
                    builder.Append("标题：" + getOdds + "");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }

            }
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("super.aspx"), "返回上一级") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion
}
