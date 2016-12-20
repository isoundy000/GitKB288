using BCW.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class bbs_guess2_boConfirm : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 走地赛事下注确认
        //走地赛事下注确认
        HttpContext.Current.Application.Lock();
        DataSet dss = new TPR2.BLL.guess.BaPay().GetBaPayList("pType,PayType,payview,ID,bcid,payusid,payusname,paytimes,payCent,types,p_result_temp1,p_result_temp2,state,payonLuone,payonLutwo,payonLuthr,p_pk,p_dx_pk,p_oncetime2,sure", "p_active=0 and (state=1 OR state=2 OR state=3 OR state=4) order by id desc");
        if (dss != null && dss.Tables[0].Rows.Count > 0)
        {            
            for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
            {
                #region 变量
                int pType = int.Parse(dss.Tables[0].Rows[i]["pType"].ToString());
                int PayType = int.Parse(dss.Tables[0].Rows[i]["PayType"].ToString());
                int zdtime = 0;
                if (pType == 1)
                    zdtime = Convert.ToInt32(ub.GetSub("SiteStemp", "/Controls/guess2.xml"));
                else
                    zdtime = Convert.ToInt32(ub.GetSub("SiteStempb", "/Controls/guess2.xml"));

                string payview = dss.Tables[0].Rows[i]["payview"].ToString();
                int ID = int.Parse(dss.Tables[0].Rows[i]["ID"].ToString());
                int bcid = int.Parse(dss.Tables[0].Rows[i]["bcid"].ToString());
                int payusid = int.Parse(dss.Tables[0].Rows[i]["payusid"].ToString());
                string payusname = dss.Tables[0].Rows[i]["payusname"].ToString();
                DateTime paytimes = DateTime.Parse(dss.Tables[0].Rows[i]["paytimes"].ToString());
                int types = int.Parse(dss.Tables[0].Rows[i]["types"].ToString());
                long payCent = Convert.ToInt64(dss.Tables[0].Rows[i]["payCent"]);
                int p_result_temp1 = int.Parse(dss.Tables[0].Rows[i]["p_result_temp1"].ToString());
                int p_result_temp2 = int.Parse(dss.Tables[0].Rows[i]["p_result_temp2"].ToString());
                int state = int.Parse(dss.Tables[0].Rows[i]["state"].ToString());
                decimal payonLuone = decimal.Parse(dss.Tables[0].Rows[i]["payonLuone"].ToString());
                decimal payonLutwo = decimal.Parse(dss.Tables[0].Rows[i]["payonLutwo"].ToString());
                decimal payonLuthr = decimal.Parse(dss.Tables[0].Rows[i]["payonLuthr"].ToString());
                decimal p_pk = decimal.Parse(dss.Tables[0].Rows[i]["p_pk"].ToString());
                decimal p_dx_pk = decimal.Parse(dss.Tables[0].Rows[i]["p_dx_pk"].ToString());
                int sure = int.Parse(dss.Tables[0].Rows[i]["sure"].ToString());
                string p_oncetime2 = "";
                if (dss.Tables[0].Rows[i]["p_oncetime2"].ToString() != "")
                    p_oncetime2 = dss.Tables[0].Rows[i]["p_oncetime2"].ToString();
                #endregion

                #region 自定义设置ID的下注时间
                ///等于1为默认确认状态,0为人工确认状态
                if (sure == 1)
                {
                    ///自定义设置ID的下注时间
                    string zdIDS = "#" + ub.GetSub("SitezdIDS", "/Controls/guess2.xml") + "#";
                    if (zdIDS.Contains("#" + payusid.ToString() + "#"))
                    {
                        zdtime = Convert.ToInt32(ub.GetSub("SiteZdtime", "/Controls/guess2.xml"));
                    }
                }
                else
                {
                    zdtime = 1;
                }
                #endregion

                #region 已开赛的下注处理
                if (state == 1 || state == 3 || state == 4)
                {
                    int p_basketve = 0;
                    if (state == 3)
                        p_basketve = 9;

                    if (DateTime.Now > paytimes.AddSeconds(zdtime))
                    {
                        bool IsPass = true;
                        #region 检查球赛即时数据 确认下注状态
                        //即时更新
                        TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().Getp_temptime_p_id(bcid);
                        if (m != null)
                        {
                            #region 下注前检查盘口数据变动
                            int p_id = Convert.ToInt32(m.p_id);
                            DateTime temptime = Convert.ToDateTime(m.p_temptime);

                            //读取SiteViewStatus 等于0时采用即时刷新，其他值时，通过刷新机刷新
                            //黄国军 20160223
                            if (ub.GetSub("SiteUpdateOpen", "/Controls/guess2.xml") == "" || ub.GetSub("SiteUpdateOpen", "/Controls/guess2.xml") == "0")
                            {
                                #region 进入旧版更新
                                if (pType == 1)
                                {
                                    string s = "";
                                    if (state == 1)
                                        new TPR2.Collec.Footbo().GetBoView(p_id, true);
                                    else
                                        new TPR2.Collec.FootFalf().FootFalfPageHtml(p_id, true);
                                }
                                else
                                {
                                    new TPR2.Collec.Basketbo().GetBoView(p_id, true);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 进入新版更新
                                if (pType == 1)
                                {
                                    string s = "";
                                    if (state == 1)
                                        new TPR2.Collec.Footbo().GetBoView1(p_id, true);
                                    else
                                        new TPR2.Collec.FootFalf().FootFalfPageHtml1(p_id, true, ref s);
                                }
                                else
                                {
                                    new TPR2.Collec.Basketbo().GetBoView1(p_id, true);
                                }
                                #endregion
                            }

                            if (state == 4)
                            {

                                IsPass = false;
                            }
                            else
                            {
                                #region 得到即时比分和封盘时间
                                //得到即时比分和封盘时间
                                TPR2.Model.guess.BaList n = new TPR2.BLL.guess.BaList().GetTemp(p_id, p_basketve);

                                if (pType == 1 && n.p_result_temp1 != null)
                                {
                                    if (n.p_result_temp1 != p_result_temp1 || n.p_result_temp2 != p_result_temp2)//比分不同
                                    {
                                        IsPass = false;
                                    }
                                }
                                #endregion

                                #region 进球更新的时间不同
                                if (m.p_temptime != n.p_temptime)//进球更新的时间不同
                                {
                                    IsPass = false;
                                }
                                if (temptime != Convert.ToDateTime("1990-1-1") && temptime > paytimes && paytimes > temptime.AddSeconds(-zdtime))
                                {
                                    IsPass = false;
                                }
                                #endregion

                                #region 让球封盘时间不同
                                //让球封盘时间不同
                                if (PayType == 1 || PayType == 2)
                                {
                                    if (m.p_temptime1 != n.p_temptime1)
                                    {
                                        IsPass = false;
                                    }
                                    if (n.p_temptime1 != Convert.ToDateTime("1990-1-1") && n.p_temptime1 > paytimes && paytimes > n.p_temptime1.AddSeconds(-zdtime))
                                    {
                                        IsPass = false;
                                    }
                                }
                                #endregion

                                #region 大小盘封盘时间不同
                                //大小盘封盘时间不同
                                if (PayType == 3 || PayType == 4)
                                {
                                    if (m.p_temptime2 != n.p_temptime2)
                                    {
                                        IsPass = false;
                                    }
                                    if (n.p_temptime2 != Convert.ToDateTime("1990-1-1") && n.p_temptime2 > paytimes && paytimes > n.p_temptime2.AddSeconds(-zdtime))
                                    {
                                        IsPass = false;
                                    }
                                }
                                #endregion

                                #region 标准盘封盘时间不同
                                //标准盘封盘时间不同
                                if (PayType == 5 || PayType == 6 || PayType == 7)
                                {
                                    if (m.p_temptime3 != n.p_temptime3)
                                    {
                                        IsPass = false;

                                    }
                                    if (n.p_temptime3 != Convert.ToDateTime("1990-1-1") && n.p_temptime3 > paytimes && paytimes > n.p_temptime3.AddSeconds(-zdtime))
                                    {
                                        IsPass = false;

                                    }
                                }
                                #endregion
                            }
                            #endregion

                            if (IsPass == false)
                            {
                                #region 投注失败,删除投注记录并退币
                                //投注失败,删除投注记录并退币
                                new TPR2.BLL.guess.BaPay().Delete(ID);
                                string bzTypes = string.Empty;
                                if (types == 0)
                                {
                                    bzTypes = ub.Get("SiteBz");
                                    new BCW.BLL.User().UpdateiGold(payusid, payusname, payCent, "" + ub.Get("SiteGqText") + "" + payview + "下注失败退回");
                                }
                                else
                                {
                                    bzTypes = ub.Get("SiteBz2");
                                    new BCW.BLL.User().UpdateiMoney(payusid, payusname, payCent, "" + ub.Get("SiteGqText") + "" + payview + "下注失败退回");
                                }
                                #endregion

                                #region 记录失败日志
                                //记录失败日志
                                new BCW.BLL.Gamelog().Add(2, "" + payusname + "(" + payusid + ")" + payview + "" + ub.Get("SiteGqText") + "下注失败，系统退回您" + payCent + "" + bzTypes + "", bcid, "自动");
                                new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "" + ub.Get("SiteGqText") + "下注失败，系统退回您" + payCent + "" + bzTypes + "");
                                #endregion
                            }
                            else
                            {
                                #region 滚球投注成功 更新state=0
                                new TPR2.BLL.guess.BaPay().Updatestate(ID);
                                new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "" + ub.Get("SiteGqText") + "下注成功");
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region 未开赛的下注处理
                else if (state == 2)//未开赛需要验证水位的投注
                {
                    if (DateTime.Now > paytimes.AddSeconds(0))
                    {
                        #region 检查即时数据
                        bool IsPass = true;
                        //即时更新
                        TPR2.Model.guess.BaList modelBaList = new TPR2.BLL.guess.BaList().GetModel(bcid);
                        if (modelBaList == null)
                        {
                            modelBaList = new TPR2.BLL.guess.BaList().GetModelByp_id(bcid);
                        }
                        if (modelBaList != null)
                        {
                            int p_id = Convert.ToInt32(modelBaList.p_id);
                            if (modelBaList.p_type == 1)
                                new TPR2.Collec.Footbo().GetBoView(p_id, false);
                            else
                                new TPR2.Collec.Basketbo().GetBoView(p_id, false);

                            modelBaList = new TPR2.BLL.guess.BaList().GetModel(bcid);//同步以上更新的水位
                            if (modelBaList == null)
                            {
                                modelBaList = new TPR2.BLL.guess.BaList().GetModelByp_id(bcid);
                            }

                            if (PayType == 1)
                            {

                                decimal oldlu = payonLuone;
                                decimal newlu = Convert.ToDecimal(modelBaList.p_one_lu);
                                decimal oldpk = p_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_pk);
                                if (oldlu - newlu >= Convert.ToDecimal(0.1) || newlu - oldlu >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                            else if (PayType == 2)
                            {

                                decimal oldlu2 = payonLutwo;
                                decimal newlu2 = Convert.ToDecimal(modelBaList.p_two_lu);
                                decimal oldpk = p_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

                                if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                            else if (PayType == 3)
                            {

                                decimal oldlu = payonLuone;
                                decimal newlu = Convert.ToDecimal(modelBaList.p_big_lu);
                                decimal oldpk = p_dx_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_dx_pk);
                                if (oldlu - newlu >= Convert.ToDecimal(0.1) || newlu - oldlu >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                            else if (PayType == 4)
                            {

                                decimal oldlu2 = payonLutwo;
                                decimal newlu2 = Convert.ToDecimal(modelBaList.p_small_lu);
                                decimal oldpk = p_dx_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_dx_pk);

                                if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                            else if (PayType == 5)
                            {

                                decimal oldlu2 = payonLuone;
                                decimal newlu2 = Convert.ToDecimal(modelBaList.p_bzs_lu);
                                decimal oldpk = p_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

                                if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                            else if (PayType == 6)
                            {

                                decimal oldlu2 = payonLutwo;
                                decimal newlu2 = Convert.ToDecimal(modelBaList.p_bzp_lu);
                                decimal oldpk = p_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

                                if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                            else if (PayType == 7)
                            {

                                decimal oldlu2 = payonLuthr;
                                decimal newlu2 = Convert.ToDecimal(modelBaList.p_bzx_lu);
                                decimal oldpk = p_pk;
                                decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

                                if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
                                    IsPass = false;

                                if (!oldpk.Equals(newpk))
                                    IsPass = false;

                            }
                        }
                        #endregion

                        #region 下注确认
                        if (IsPass == false)
                        {
                            //投注失败,删除投注记录并退币
                            new TPR2.BLL.guess.BaPay().Delete(ID);
                            string bzTypes = string.Empty;
                            if (types == 0)
                            {
                                bzTypes = ub.Get("SiteBz");
                                new BCW.BLL.User().UpdateiGold(payusid, payusname, payCent, "" + payview + "下注失败退回");
                            }
                            else
                            {
                                bzTypes = ub.Get("SiteBz2");
                                new BCW.BLL.User().UpdateiMoney(payusid, payusname, payCent, "" + payview + "下注失败退回");
                            }
                            new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "下注失败，系统退回您" + payCent + "" + bzTypes + "");
                            //记录失败日志
                            new BCW.BLL.Gamelog().Add(2, "" + payusname + "(" + payusid + ")" + payview + "下注失败，标识ID" + ID + "", bcid, "自动");

                        }
                        else
                        {
                            new TPR2.BLL.guess.BaPay().Updatestate(ID);
                            new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "下注成功");
                        }
                        #endregion
                    }
                }
                #endregion
            }

        }
        HttpContext.Current.Application.UnLock();
        //-----------------------------------------------------------------------
        #endregion
    }
}