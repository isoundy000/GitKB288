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
using System.IO;
using BCW.Common;

/// <summary>
/// 增加串串人工开奖功能
/// 黄国军 20160702
/// 串串开奖过程，已转到刷新机程序刷新
/// 黄国军 20160226
/// </summary>
public partial class bbs_guess2_superover : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        DataSet ds = new TPR2.BLL.guess.Super().GetList("ID,UsID,UsName,BID,PID,Odds,PayCent,p_isfs,p_Auto", "IsOpen=1 and Status=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            #region 循环下注订单
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                #region 赋值初期
                bool isover = true;
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string BID = ds.Tables[0].Rows[i]["BID"].ToString();
                string PID = ds.Tables[0].Rows[i]["PID"].ToString();
                string Odds = ds.Tables[0].Rows[i]["Odds"].ToString();
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                double PayCent = double.Parse(ds.Tables[0].Rows[i]["PayCent"].ToString());
                int p_isfs = int.Parse(ds.Tables[0].Rows[i]["p_isfs"].ToString());

                string[] sBID = Regex.Split(Utils.Mid(BID, 2, BID.Length), "##");
                string[] sPID = Regex.Split(Utils.Mid(PID, 2, PID.Length), "##");
                string[] sOdds = Regex.Split(Utils.Mid(Odds, 2, Odds.Length), "##");
                int p_Auto = int.Parse(ds.Tables[0].Rows[i]["p_Auto"].ToString());
                #endregion

                //P不等于0的时候则视为人工开奖
                if (p_Auto != 0) { continue; }

                #region 比赛是否已结束
                for (int j = 0; j < sBID.Length; j++)
                {
                    int gid = Convert.ToInt32(sBID[j]);

                    //比赛是否已结束
                    isover = BCW.Data.SqlHelper.Exists("select count(1) from tb_TBaList where ID=" + gid + " and p_active>0");
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
                                int gid = Convert.ToInt32(sBID3[k]);
                                int pid = Convert.ToInt32(sPID3[k]);
                                double odds = Convert.ToDouble(sOdds3[k]);

                                TPR2.Model.guess.BaList qq = new TPR2.BLL.guess.BaList().GetModel(gid);

                                int p_type = new TPR2.BLL.guess.BaList().GetPtype(gid);
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
                                        Stats += "##平盘";
                                        pCent = pCent * 1;
                                        errNum++;
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
                        TPR2.Model.guess.Super model = new TPR2.Model.guess.Super();
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
                            int gid = Convert.ToInt32(sBID[k]);
                            int pid = Convert.ToInt32(sPID[k]);
                            double odds = Convert.ToDouble(sOdds[k]);
                            TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().GetModel(gid);

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
                        TPR2.Model.guess.Super model = new TPR2.Model.guess.Super();
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
                }
                #endregion
            }
            #endregion

            Response.Write("Ok!");
            Response.End();
        }

        Response.Write("No record!");
        Response.End();

    }
}
