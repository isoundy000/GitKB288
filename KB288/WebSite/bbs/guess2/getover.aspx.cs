using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR2.Common;
using TPR2.Common.Guess;

/// <summary>
/// 修改人工开奖的球赛不自动开奖,需要人工开奖
/// 
/// 黄国军 20160323
/// </summary>
public partial class bbs_guess2_getover : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {

        //检查消费日志有没有重复兑奖的

        //DataSet ds = BCW.Data.SqlHelper.Query("select UsID,AcText from tb_Goldlog where IsCheck=0 and (Purl='/bbs/guess2/caseGuess.aspx' OR Purl='/bbs/guess2/supercase.aspx') and AcText<>'" + ub.Get("SiteGqText") + "下注失败退回' Group by UsID,AcText having count(*)>1");

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        int meid = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
        //        string AcText = ds.Tables[0].Rows[i]["AcText"].ToString();
        //        new BCW.BLL.Guest().Add(10086, "客服", "会员ID" + meid + "|" + AcText + "重复兑奖，请马上进入后台处理");
        //    }
        //}

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, "", "1"));
        string strWhere = "";
        int recordCount = 0;

        if (ptype == 1)
        {
            #region 足球开奖
            //足球
            strWhere = "p_type=1 and p_active=0 and p_result_one IS NOT NULL and p_result_two IS NOT NULL and p_TPRtime< '" + DateTime.Now.AddMinutes(-60) + "'";
            IList<TPR2.Model.guess.BaList> listBasket = new TPR2.BLL.guess.BaList().GetBaListBF(strWhere, out recordCount);
            if (listBasket.Count > 0)
            {
                foreach (TPR2.Model.guess.BaList n in listBasket)
                {
                    //=0为自动开奖
                    if (n.p_dr == 0)
                    {
                        #region 滚球或正常模式 0为正常 开奖
                        //滚球或正常模式 0为正常 开奖
                        if (n.p_ison == 0)
                        {
                            //正常开奖
                            new TPR2.BLL.guess.BaList().Updatep_active(n.ID, 1);
                            //遍历所有下注更新开奖
                            UpdateCase(Convert.ToInt32(n.p_result_one), Convert.ToInt32(n.p_result_two), n.ID, 1);

                        }
                        else
                        {
                            //滚球模式开奖
                            //" + ub.Get("SiteGqText") + "开奖
                            if (ub.GetSub("SiteonceType", xmlPath) == "0")//如果系统设置为自动开" + ub.Get("SiteGqText") + "
                            {
                                new TPR2.BLL.guess.BaList().Updatep_active(n.ID, 1);
                                //遍历所有下注更新开奖(" + ub.Get("SiteGqText") + "开奖)
                                UpdateCaseOnce(Convert.ToInt32(n.p_result_one), Convert.ToInt32(n.p_result_two), n.ID, 1);

                            }
                        }
                        #endregion
                    }
                }
                Response.Write("足球开奖完成");
            }
            else
            {
                Response.Write("暂无开奖数据");
            }
            #endregion
        }
        else if (ptype == 2)
        {
            #region 篮球开奖
            //篮球
            strWhere = "p_type=2 and p_active=0 and p_basketve=0 and p_result_one IS NOT NULL and p_result_two IS NOT NULL and p_TPRtime< '" + DateTime.Now.AddMinutes(-60) + "'";
            IList<TPR2.Model.guess.BaList> listBasket = new TPR2.BLL.guess.BaList().GetBaListBF(strWhere, out recordCount);
            if (listBasket.Count > 0)
            {
                foreach (TPR2.Model.guess.BaList n in listBasket)
                {
                    //=0为自动开奖
                    if (n.p_dr == 0)
                    {
                        #region 滚球或正常模式 0为正常 开奖
                        //滚球或正常模式 0为正常 开奖
                        if (n.p_ison == 0)
                        {
                            UpdateCase(Convert.ToInt32(n.p_result_one), Convert.ToInt32(n.p_result_two), n.ID, 2);
                            new TPR2.BLL.guess.BaList().Updatep_active(n.ID, 1);
                        }
                        else
                        {
                            //" + ub.Get("SiteGqText") + "开奖
                            if (ub.GetSub("SiteonceType", xmlPath) == "0")//如果系统设置为自动开" + ub.Get("SiteGqText") + "
                            {
                                UpdateCase(Convert.ToInt32(n.p_result_one), Convert.ToInt32(n.p_result_two), n.ID, 2);
                                new TPR2.BLL.guess.BaList().Updatep_active(n.ID, 1);
                            }
                        }
                        #endregion
                    }
                }
                Response.Write("篮球开奖完成");
            }
            else
            {
                Response.Write("暂无开奖数据");
            }
            #endregion
        }
        else if (ptype == 3)
        {
            #region 半场单节开奖
            //篮球
            strWhere = "p_type=2 and p_active=0 and p_basketve>0 and p_TPRtime< '" + DateTime.Now.AddMinutes(-10) + "'";
            IList<TPR2.Model.guess.BaList> listBasket = new TPR2.BLL.guess.BaList().GetBaListBF(strWhere, out recordCount);
            if (listBasket.Count > 0)
            {
                foreach (TPR2.Model.guess.BaList n in listBasket)
                {
                    try
                    {
                        //=0为自动开奖
                        if (n.p_dr == 0)
                        {
                            //篮球比分分析
                            basket(n.p_basketve, Convert.ToDateTime(n.p_TPRtime), Convert.ToInt32(n.ID), Convert.ToInt32(n.p_id));
                        }
                    }
                    catch { }
                }
                Response.Write("半场单节开奖完成");
            }
            else
            {
                Response.Write("暂无开奖数据");
            }
            #endregion
        }
    }

    #region 篮球比分分析 basket
    /// <summary>
    /// 篮球比分分析
    /// </summary>
    /// <param name="p_basketve"></param>
    /// <param name="p_TPRtime"></param>
    /// <param name="ID"></param>
    /// <param name="p_id"></param>
    private void basket(int p_basketve, DateTime p_TPRtime, int ID, int p_id)
    {

        int Types = p_basketve;
        int bf1 = -1;
        int bf2 = -1;

        string p_xml = GetSourceTextByUrl("http://bf.win007.com/nba_date.aspx?time=" + p_TPRtime.ToString("yyyy-MM-dd") + "", "GB2312");

        TPR2.Model.guess.Tempbakbf n = new TPR2.Model.guess.Tempbakbf();
        using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
        {
            while (reader.ReadToFollowing("h"))
            {
                string p_str = reader.GetElementValue();

                string[] Temp = Regex.Split(p_str, @"\^");
                int id = 0;
                int state = 0;

                id = Convert.ToInt32(Temp[0]);
                state = Convert.ToInt32(Temp[5]);//状态:0:未开赛,1:一节,2:二节,5:1'OT，以此类推，-1:完场, -2:待定,-3:中断,-4:取消,-5:推迟,50中场
                #region 获取状态
                if (id == p_id)
                {
                    if (Types == 0)
                    {
                        if (state == -1)
                        {
                            bf1 = Convert.ToInt32(Temp[11]);
                            bf2 = Convert.ToInt32(Temp[12]);
                        }
                    }
                    else if (Types == 1)
                    {
                        if (state > 1 || state == -1)
                        {
                            bf1 = Convert.ToInt32(Temp[13]);
                            bf2 = Convert.ToInt32(Temp[14]);
                        }
                    }
                    else if (Types == 2)
                    {
                        if (state > 2 || state == -1)
                        {
                            bf1 = Convert.ToInt32(Temp[15]);
                            bf2 = Convert.ToInt32(Temp[16]);
                        }
                    }
                    else if (Types == 3)
                    {
                        if (state > 2 || state == -1)
                        {
                            bf1 = Convert.ToInt32(Temp[13]) + Convert.ToInt32(Temp[15]);
                            bf2 = Convert.ToInt32(Temp[14]) + Convert.ToInt32(Temp[16]);
                        }
                    }
                    else if (Types == 4)
                    {
                        if ((state > 3 && state < 50) || state == -1)
                        {
                            bf1 = Convert.ToInt32(Temp[17]);
                            bf2 = Convert.ToInt32(Temp[18]);
                        }
                    }
                    break;
                }
                #endregion
            }
        }

        #region 开奖
        if (bf1 != -1 && bf2 != -1)
        {
            bool IsTrue = true;
            if (Types == 0 && bf1 == bf2)
                IsTrue = false;

            if (Types > 0 && bf1 == 0 && bf2 == 0)
                IsTrue = false;

            if (IsTrue)
            {
                TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                model.p_id = p_id;
                model.p_result_one = bf1;
                model.p_result_two = bf2;
                model.p_once = "";
                model.p_active = 1;
                model.p_basketve = p_basketve;
                int upid = new TPR2.BLL.guess.BaList().UpdateZDResult2(model);
                if (upid > 0)
                {
                    //正常开奖
                    UpdateCase(bf1, bf2, ID, 2);
                }
            }
        }
        #endregion
    }
    #endregion

    #region 遍历所有下注更新开奖(" + ub.Get("SiteGqText") + "开奖) UpdateCaseOnce
    /// <summary>
    /// 遍历所有下注更新开奖(" + ub.Get("SiteGqText") + "开奖)
    /// </summary>
    /// <param name="resultone"></param>
    /// <param name="resulttwo"></param>
    /// <param name="gid"></param>
    /// <param name="p_type"></param>
    private void UpdateCaseOnce(int resultone, int resulttwo, int gid, int p_type)
    {
        int OnceMin = Convert.ToInt32(ub.GetSub("SiteOnce", xmlPath));//" + ub.Get("SiteGqText") + "时间限制

        string strWhere = "";
        int recordCount = 0;
        int p_intWin = 0;
        decimal p_intDuVal = 0;
        strWhere = "bcid=" + gid + "";
        TPR2.Model.guess.BaOrder objBaOrder = new TPR2.Model.guess.BaOrder();
        //取得比分时间段
        string stronce = new TPR2.BLL.guess.BaList().Getonce(gid);
        // 开始查询并更新之
        IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPays(1, 5000, strWhere, out recordCount);
        if (listBaPay.Count > 0)
        {
            foreach (TPR2.Model.guess.BaPay n in listBaPay)
            {
                int Iszd = 0;
                //------------------------------------------------------
                n.p_result_one = resultone;
                n.p_result_two = resulttwo;
                n.p_active = 1;

                //币种
                string bzTypes = string.Empty;
                if (n.Types == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                if (p_type == 1)
                {
                    //是否可以平盘了
                    if (n.PayType <= 4)
                    {
                        //if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
                        //{
                        //    stronce = new TPR2.BLL.guess.BaList().Getp_temptimes(gid);
                        //}
                        //if (!string.IsNullOrEmpty(stronce))
                        //{
                        //    string[] Sonce = stronce.Split("|".ToCharArray());

                        //    for (int i = 0; i < Sonce.Length; i++)
                        //    {
                        //        if (Convert.ToDateTime(Sonce[i]).AddSeconds(OnceMin) > Convert.ToDateTime(n.paytimes) && Convert.ToDateTime(Sonce[i]).AddSeconds(-OnceMin) < Convert.ToDateTime(n.paytimes))
                        //        {
                        //            Iszd = 2;//平盘标识
                        //        }
                        //    }
                        //}
                    }
                    if (Iszd == 0)
                    {
                        if (n.PayType == 1 || n.PayType == 2)//足球让球盘" + ub.Get("SiteGqText") + "开奖
                        {

                            //比分变化前n秒投注平盘返彩
                            bool Ispp = false;
                            //if (Utils.GetTopDomain() == "tl88.cc")
                            //{
                            //    string p_temptimes = new TPR2.BLL.guess.BaList().Getp_temptimes(gid);
                            //    if (p_temptimes != "")
                            //    {
                            //        string[] ptemp = p_temptimes.Split("|".ToCharArray());
                            //        for (int i = 0; i < ptemp.Length; i++)
                            //        {

                            //            TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(n.paytimes).Ticks);
                            //            TimeSpan ts2 = new TimeSpan(Convert.ToDateTime(ptemp[i]).Ticks);
                            //            TimeSpan ts = ts2.Subtract(ts1);
                            //            if (ts.Seconds <= Utils.ParseInt(ub.GetSub("SiteStemp2", xmlPath)))
                            //            {
                            //                Iszd = 3;//平盘标识
                            //                Ispp = true;
                            //                break;
                            //            }

                            //        }
                            //    }
                            //}
                            if (!Ispp)
                            {
                                //算出" + ub.Get("SiteGqText") + "（总分减下注时的比分）
                                n.p_result_one = resultone - Convert.ToInt32(n.p_result_temp1);
                                n.p_result_two = resulttwo - Convert.ToInt32(n.p_result_temp2);
                                string p_strVal = ZqClass.getZqsxCase(n);
                                //重新取值
                                n.p_result_one = resultone;
                                n.p_result_two = resulttwo;
                                new TPR2.BLL.guess.BaPay().UpdateCase(n, p_strVal, out p_intDuVal, out p_intWin);
                                Iszd = 1;//" + ub.Get("SiteGqText") + "标识
                            }

                        }
                        else if (n.PayType == 3 || n.PayType == 4)
                        {
                            new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqdxCase(n), out p_intDuVal, out p_intWin);
                        }
                        else if (n.PayType == 5 || n.PayType == 6 || n.PayType == 7)
                        {
                            new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqbzCase(n), out p_intDuVal, out p_intWin);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqScoreCase(n), out p_intDuVal, out p_intWin);
                        }
                    }
                }
                else
                {
                    if (n.PayType == 1 || n.PayType == 2)
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqsxCase(n), out p_intDuVal, out p_intWin);
                    else
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqdxCase(n), out p_intDuVal, out p_intWin);
                }
                if (Iszd != 2 && Iszd != 3)
                {
                    if (p_intWin == 1)
                    {
                        //更新排行榜:赢

                        objBaOrder.Orderusid = n.payusid;
                        objBaOrder.Orderusname = n.payusname;

                        if (p_intDuVal == n.payCent)
                        {
                            objBaOrder.Orderbanum = 0;
                            objBaOrder.Orderjbnum = 0;
                        }
                        else
                        {
                            objBaOrder.Orderbanum = 1;
                            objBaOrder.Orderjbnum = p_intDuVal - n.payCent;
                        }
                        objBaOrder.Orderfanum = 0;

                        objBaOrder.Orderstats = n.pType;
                        new TPR2.BLL.guess.BaOrder().UpdateOrder(objBaOrder);

                        //发送内线
                        if (Convert.ToInt32(n.itypes) == 0)
                        {
                            string strLog = string.Empty;
                            if (Iszd == 1)  //" + ub.Get("SiteGqText") + "的内线提醒
                                strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "(下注" + n.p_result_temp1 + ":" + n.p_result_temp2 + ")，赢了" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";
                            else
                                strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "，返了" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";

                            new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                        }
                    }
                    else
                    {
                        //更新排行榜:输

                        objBaOrder.Orderusid = n.payusid;
                        objBaOrder.Orderusname = n.payusname;
                        objBaOrder.Orderbanum = 0;

                        objBaOrder.Orderfanum = 1;

                        objBaOrder.Orderjbnum = -n.payCent;
                        objBaOrder.Orderbanum = 0;

                        objBaOrder.Orderstats = n.pType;
                        new TPR2.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                    }
                }
                else
                {
                    //平盘
                    n.p_result_one = resultone;
                    n.p_result_two = resulttwo;
                    n.p_active = 2;
                    n.p_getMoney = n.payCent;
                    new TPR2.BLL.guess.BaPay().UpdatePPCase(n);
                    //发送内线
                    if (Convert.ToInt32(n.itypes) == 0)
                    {
                        string strLog = string.Empty;
                        //if (Iszd == 2)
                        strLog = "" + n.payview + "[br]结果平盘，原因：" + ub.Get("SiteGqText") + "赛事，系统将比分变动前后" + OnceMin + "秒钟的下注作平盘处理，返还" + Convert.ToDouble(n.payCent) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url][br]本场赛事变动时间如下:[br]" + stronce.Replace("|", "[br]") + "";
                        //else
                        //strLog = "" + n.payview + "[br]结果平盘，原因：" + ub.Get("SiteGqText") + "赛事，系统将比分变动前" + ub.GetSub("SiteStemp2", xmlPath) + "秒的下注作平盘处理，返还" + Convert.ToDouble(n.payCent) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";

                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                    }
                }
                //------------------------------------------------------

            }
        }
    }
    #endregion

    #region 遍历所有下注更新开奖 UpdateCase
    /// <summary>
    /// 遍历所有下注更新开奖
    /// </summary>
    /// <param name="resultone"></param>
    /// <param name="resulttwo"></param>
    /// <param name="gid"></param>
    /// <param name="p_type"></param>
    private void UpdateCase(int resultone, int resulttwo, int gid, int p_type)
    {
        string strWhere = "";
        int recordCount = 0;
        int p_intWin = 0;
        decimal p_intDuVal = 0;
        strWhere = "bcid=" + gid + "";
        TPR2.Model.guess.BaOrder objBaOrder = new TPR2.Model.guess.BaOrder();

        // 开始查询并更新之
        IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPays(1, 5000, strWhere, out recordCount);
        if (listBaPay.Count > 0)
        {
            foreach (TPR2.Model.guess.BaPay n in listBaPay)
            {
                //------------------------------------------------------
                n.p_result_one = resultone;
                n.p_result_two = resulttwo;
                n.p_active = 1;

                //币种
                string bzTypes = string.Empty;
                if (n.Types == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                if (p_type == 1)
                {
                    if (n.PayType == 1 || n.PayType == 2)
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqsxCase(n), out p_intDuVal, out p_intWin);
                    else if (n.PayType == 3 || n.PayType == 4)
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqdxCase(n), out p_intDuVal, out p_intWin);
                    else if (n.PayType == 5 || n.PayType == 6 || n.PayType == 7)
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqbzCase(n), out p_intDuVal, out p_intWin);
                    else
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqScoreCase(n), out p_intDuVal, out p_intWin);
                }
                else
                {
                    if (n.PayType == 1 || n.PayType == 2)
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqsxCase(n), out p_intDuVal, out p_intWin);
                    else
                        new TPR2.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqdxCase(n), out p_intDuVal, out p_intWin);
                }

                if (p_intWin == 1)
                {
                    //更新排行榜:赢

                    objBaOrder.Orderusid = n.payusid;
                    objBaOrder.Orderusname = n.payusname;

                    if (p_intDuVal == n.payCent)
                    {
                        objBaOrder.Orderbanum = 0;
                        objBaOrder.Orderjbnum = 0;
                    }
                    else
                    {
                        objBaOrder.Orderbanum = 1;
                        objBaOrder.Orderjbnum = p_intDuVal - n.payCent;
                    }
                    objBaOrder.Orderfanum = 0;

                    objBaOrder.Orderstats = n.pType;
                    new TPR2.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                    //发送内线
                    if (Convert.ToInt32(n.itypes) == 0)
                    {
                        string strLog = string.Empty;
                        strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "，返了" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";
                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                    }
                }
                else
                {
                    //更新排行榜:输

                    objBaOrder.Orderusid = n.payusid;
                    objBaOrder.Orderusname = n.payusname;
                    objBaOrder.Orderbanum = 0;

                    objBaOrder.Orderfanum = 1;

                    objBaOrder.Orderjbnum = -n.payCent;
                    objBaOrder.Orderbanum = 0;

                    objBaOrder.Orderstats = n.pType;
                    new TPR2.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                }
                //------------------------------------------------------

            }
        }
    }
    #endregion

    #region 转圈图标
    private void binddata()
    {
        FileStream fs = new FileStream(Server.MapPath("/Files/sys/loading.gif"), FileMode.Open, FileAccess.Read);
        byte[] mydata = new byte[fs.Length];
        int Length = Convert.ToInt32(fs.Length);
        fs.Read(mydata, 0, Length);
        fs.Close();
        Response.Clear();
        Response.ContentType = "image/gif";
        Response.OutputStream.Write(mydata, 0, Length);
        Response.End();
    }
    #endregion

    #region 抓取一个网页源码
    /// <summary>
    /// 抓取一个网页源码
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetSourceTextByUrl(string url, string Encoding)
    {
        try
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            request.Timeout = 20000;
            System.Net.WebResponse response = request.GetResponse();

            System.IO.Stream resStream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(resStream, System.Text.Encoding.GetEncoding(Encoding));
            return sr.ReadToEnd();
        }
        catch
        {
            return "";
        }
    }
    #endregion
}