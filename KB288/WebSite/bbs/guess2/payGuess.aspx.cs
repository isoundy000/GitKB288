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
using BCW.Common;
using TPR2.Common;

/// <summary>
/// 黄国军 20160730 增加球赛下注提示
/// 黄国军 20160309 去除1010接收的内线信息
/// 蒙宗将 20160517 抽奖值生成
/// 邵广林 20160617 动态添加usid
/// 姚志光 20160621 活跃抽奖入口
///  蒙宗将 20160822 撤掉抽奖值生成
///  
/// </summary>
public partial class bbs_guess2_payGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        long gold = new BCW.BLL.User().GetGold(meid);
        long money = new BCW.BLL.User().GetMoney(meid);
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "all", 2, @"^[1-7]*$", "选择无效"));
        if (ub.GetSub("SiteIsbz", "/Controls/guess2.xml") == "1")
        {
            if (p > 4)
            {
                Utils.Error("标准盘暂时不能购买", "");
            }
        }
        TPR2.BLL.guess.BaList BaListbll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList st = BaListbll.GetModel(gid);
        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }

        #region 立即更新水位 访问8bo

        string bo = "";
        //-----------------------------立即更新水位---------------------------------
        if (st.p_active == 0)
        {
            //读取SiteViewStatus 等于0时采用即时刷新，其他值时，通过刷新机刷新
            //黄国军 20160223
            if (ub.GetSub("SiteUpdateOpen", "/Controls/guess2.xml") == "" || ub.GetSub("SiteUpdateOpen", "/Controls/guess2.xml") == "0")
            {
                #region 进入旧版更新
                if (st.p_basketve == 0)
                {
                    if (st.p_type == 1)
                    {
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                        }
                        else
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), false);
                            //进行波胆更新
                            if (st.p_score != "")
                            {
                                bo = new TPR2.Collec.Footbd().FootbdPageHtml_kb_old(Convert.ToInt32(st.p_id));
                            }
                        }
                    }
                    else
                    {
                        if (st.p_ison == 1)
                            bo = new TPR2.Collec.Basketbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                        else
                            bo = new TPR2.Collec.Basketbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), false);
                    }
                }
                else if (st.p_basketve == 9)
                {
                    //载入页面更新足球上半场
                    if (st.p_type == 1)
                    {
                        string s = "";
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml_kb_old(Convert.ToInt32(st.p_id), true, ref s);
                        }
                        else
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml_kb_old(Convert.ToInt32(st.p_id), false, ref s);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 进入新版更新
                if (st.p_basketve == 0)
                {
                    if (st.p_type == 1)
                    {
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView1(Convert.ToInt32(st.p_id), true);
                        }
                        else
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView1(Convert.ToInt32(st.p_id), false);
                            //进行波胆更新
                            if (st.p_score != "")
                            {
                                new TPR2.Collec.Footbd().FootbdPageHtml(Convert.ToInt32(st.p_id));
                            }
                        }
                    }
                    else
                    {
                        //if (st.p_ison == 1)
                        //    bo = new TPR2.Collec.Basketbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                        //else
                        //    bo = new TPR2.Collec.Basketbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), false);

                        if (st.p_ison == 1)
                            bo = new TPR2.Collec.Basketbo().GetBoView1(Convert.ToInt32(st.p_id), true);
                        else
                            bo = new TPR2.Collec.Basketbo().GetBoView1(Convert.ToInt32(st.p_id), false);
                    }
                }
                else if (st.p_basketve == 9)
                {
                    //载入页面更新足球上半场
                    if (st.p_type == 1)
                    {
                        string s = "";
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml1(Convert.ToInt32(st.p_id), true, ref s);
                        }
                        else
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml1(Convert.ToInt32(st.p_id), false, ref s);
                        }
                    }
                }

                #endregion
            }
            //篮球半场和单节
            if (st.p_basketve == 1 || st.p_basketve == 3) { bo = "1"; }
        }
        #endregion


        TPR2.Model.guess.BaList modelBaList = BaListbll.GetModel(gid);

        if (modelBaList.p_del == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        //联赛限制显示
        //string Levens = "";
        //if (modelBaList.p_type == 1)
        //    Levens = "#" + ub.GetSub("SiteLeven1", xmlPath) + "#" + ub.GetSub("SiteLeven2", xmlPath) + "#" + ub.GetSub("SiteLeven3", xmlPath) + "#" + ub.GetSub("SiteLeven4", xmlPath) + "#";
        //else
        //    Levens = "#" + ub.GetSub("SiteLevenb1", xmlPath) + "#" + ub.GetSub("SiteLevenb2", xmlPath) + "#" + ub.GetSub("SiteLevenb3", xmlPath) + "#" + ub.GetSub("SiteLevenb4", xmlPath) + "#";

        //if (!Levens.Contains("#" + modelBaList.p_title + "#"))
        //{
        //    Utils.Error("联赛“" + modelBaList.p_title + "”尚未设置，请联系<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=10086") + "\">客服</a>申诉，谢谢", "");
        //}

        Master.Title = modelBaList.p_one + "VS" + modelBaList.p_two;

        string ac = Utils.GetRequest("ac", "post", 1, "", "");

        if (ac != "")
        {
            //判断金额是否够了
            int types = int.Parse(Utils.GetRequest("types", "all", 1, @"^[0-1]$", "0"));
            int payCent = 0;
            string BzText = "";
            if (Utils.ToSChinese(ac) == ub.Get("SiteBz2") + "下注" || types == 1)
            {
                payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
                if (money < Convert.ToInt64(payCent))
                {
                    Utils.Error("你的" + ub.Get("SiteBz2") + "不够此次下注", "");
                }
                types = 1;
                BzText = ub.Get("SiteBz2");
            }
            else
            {
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                {
                    if (Utils.ToSChinese(ac) == "50万")
                    {
                        payCent = 500000;
                    }
                    else if (Utils.ToSChinese(ac) == "100万")
                    {
                        payCent = 1000000;
                    }
                    else if (Utils.ToSChinese(ac) == "200万")
                    {
                        payCent = 2000000;
                    }
                    else if (Utils.ToSChinese(ac) == "500万")
                    {
                        payCent = 5000000;
                    }
                    else
                    {
                        payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
                    }
                }
                else
                {
                    if (Utils.ToSChinese(ac) == "100万")
                    {
                        payCent = 1000000;
                    }
                    else if (Utils.ToSChinese(ac) == "200万")
                    {
                        payCent = 2000000;
                    }
                    else if (Utils.ToSChinese(ac) == "500万")
                    {
                        payCent = 5000000;
                    }
                    else if (Utils.ToSChinese(ac) == "1000万")
                    {
                        payCent = 10000000;
                    }
                    else
                    {
                        payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
                    }

                }
                if (gold < Convert.ToInt64(payCent))
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不够此次下注", "");
                }
                types = 0;
                BzText = ub.Get("SiteBz");
            }

            if (p == 1 || p == 2)
            {
                if ((modelBaList.p_ison == 1 && modelBaList.p_isluckone == 1) || modelBaList.p_isluck == 1)
                {
                    Utils.Error("让球盘已封，请稍后再试", "");
                }

            }
            if (p == 3 || p == 4)
            {
                if (modelBaList.p_big_lu == -1 || modelBaList.p_dx_pk == 0)
                {
                    Utils.Error("不存在的大小球下注", "");
                }
                if ((modelBaList.p_ison == 1 && modelBaList.p_islucktwo == 1) || modelBaList.p_isluck == 1)
                {
                    Utils.Error("大小盘已封，请稍后再试", "");
                }
            }
            if (p > 4)
            {
                if (modelBaList.p_bzs_lu == -1 || ub.GetSub("SiteIsbz", "/Controls/guess.xml") == "1")
                {
                    Utils.Error("不存在的标准盘下注", "");
                }
                if ((modelBaList.p_ison == 1 && modelBaList.p_isluckthr == 1) || modelBaList.p_isluck == 1)
                {
                    Utils.Error("标准盘已封，请稍后再试", "");
                }
            }

            if (modelBaList.p_ison == 0)//" + ub.Get("SiteGqText") + "不限制
            {
                if (modelBaList.p_TPRtime <= DateTime.Now)
                {
                    Utils.Error("开赛时间已到，暂停下注", "");
                }
            }
            if (modelBaList.p_ison == 1)
            {
                if (modelBaList.p_once == "未")
                {
                    Utils.Error("比赛还没有开始,请稍后再试", "");
                }

                //if (modelBaList.p_type == 1)
                //{

                //    if (modelBaList.p_temptime != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStemp", xmlPath))))
                //    {
                //        Utils.Error("比分正在变化或水位大幅度调整中，请稍后下注", "");
                //    }
                //    if ((p == 1 || p == 2) && modelBaList.p_temptime1 != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime1).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStemp", xmlPath))))
                //    {
                //        Utils.Error("上下盘水位大幅度调整中，请稍后下注", "");
                //    }
                //    if ((p == 3 || p == 4) && modelBaList.p_temptime2 != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime2).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStemp", xmlPath))))
                //    {
                //        Utils.Error("大小盘水位大幅度调整中，请稍后下注", "");
                //    }
                //    if ((p == 5 || p == 6 || p == 7) && modelBaList.p_temptime3 != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime3).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStemp", xmlPath))))
                //    {
                //        Utils.Error("标准盘水位大幅度调整中，请稍后下注", "");
                //    }
                //}
                //else
                //{
                //    if (modelBaList.p_temptime != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStempb", xmlPath))))
                //    {
                //        Utils.Error("水位大幅度调整中，请稍后下注", "");
                //    }
                //    if ((p == 1 || p == 2) && modelBaList.p_temptime1 != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime1).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStempb", xmlPath))))
                //    {
                //        Utils.Error("上下盘水位大幅度调整中，请稍后下注", "");
                //    }
                //    if ((p == 3 || p == 4) && modelBaList.p_temptime2 != null && DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime2).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStempb", xmlPath))))
                //    {
                //        Utils.Error("标准盘水位大幅度调整中，请稍后下注", "");
                //    }
                //}
                try
                {
                    if (modelBaList.p_type == 1)
                    {
                        if (Utils.ParseInt(modelBaList.p_once.Replace("'", "").Replace("+", "")) >= 90 || modelBaList.p_once.Contains("加"))
                        {
                            Utils.Error("已封盘，暂停下注!", "");
                        }
                    }
                }
                catch { }

                if (DateTime.Now > Convert.ToDateTime(modelBaList.p_oncetime))
                {
                    Utils.Error("已封盘，暂停下注", "");
                }
                if (modelBaList.p_once == "完")
                {
                    Utils.Error("已封盘，暂停下注", "");
                }
                if (modelBaList.p_once.Contains("待定"))
                {
                    modelBaList.p_del = 1;
                    new TPR2.BLL.guess.BaList().Updatep_del(modelBaList);
                    Utils.Error("本场待定，暂停下注", "");
                }
                if (modelBaList.p_once.Contains("腰斩"))
                {
                    modelBaList.p_del = 1;
                    new TPR2.BLL.guess.BaList().Updatep_del(modelBaList);
                    Utils.Error("本场腰斩，暂停下注", "");
                }
                if (modelBaList.p_once.Contains("推迟"))
                {
                    modelBaList.p_del = 1;
                    new TPR2.BLL.guess.BaList().Updatep_del(modelBaList);
                    Utils.Error("本场推迟，暂停下注", "");
                }
                if (modelBaList.p_once.Contains("中断"))
                {
                    modelBaList.p_del = 1;
                    new TPR2.BLL.guess.BaList().Updatep_del(modelBaList);
                    Utils.Error("本场中断，暂停下注", "");
                }

                //" + ub.Get("SiteGqText") + "大小盘终极限制
                if (modelBaList.p_type == 1)
                {
                    decimal p_result = Convert.ToDecimal(modelBaList.p_result_temp1 + modelBaList.p_result_temp2);
                    string p_dx = GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk));
                    decimal dx = Convert.ToDecimal(p_dx.Split("/".ToCharArray())[0]);
                    if (p_result >= dx)
                    {
                        Utils.Error("核对比分中，请稍后投注", "");
                    }
                }
            }
            else
            {
                if (modelBaList.p_result_temp1 != null && modelBaList.p_result_temp2 != null && modelBaList.p_result_temp1 != 0 && modelBaList.p_result_temp2 != 0)
                {
                    Utils.Error("开赛时间已到，暂停下注！", "");
                }
            }

            if (modelBaList.p_isondel == 1)
            {
                Utils.Error("已封盘，暂停下注", "");
            }
            if (types == 1)
            {

                if (payCent < Convert.ToInt64(ub.GetSub("SiteSmallPay3", xmlPath)) || payCent > Convert.ToInt64(ub.GetSub("SiteBigPay3", xmlPath)))
                {
                    Utils.Error("" + ub.Get("SiteBz2") + "金额限" + ub.GetSub("SiteSmallPay3", xmlPath) + "-" + ub.GetSub("SiteBigPay3", xmlPath) + "" + ub.Get("SiteBz2") + "", "");
                }

                //每场每ID下注额
                long setPayCents = Utils.ParseInt64(ub.GetSub("SiteIDMaxPay", xmlPath));
                if (setPayCents != 0)
                {
                    long myPayCents = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("bcid=" + gid + " and pType=" + Convert.ToInt32(modelBaList.p_type) + " and payusid=" + meid + " and Types=1");
                    if (myPayCents + Convert.ToInt64(payCent) > setPayCents)
                    {
                        if (myPayCents >= setPayCents)
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz2") + "，欢迎在下场下注", "");
                        }
                        else
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz2") + "，你现在最多可以下注" + (setPayCents - myPayCents) + "" + ub.Get("SiteBz2") + "", "");
                        }

                    }
                }

            }
            if (types == 0)
            {
                int setPayNum = 0;
                long setMaxNum = 0;
                string setMaxNumGid = "";
                int getPayNum = 0;
                setPayNum = Utils.ParseInt(ub.GetSub("SitePayNum", xmlPath));
                if (ub.GetSub("SiteMaxNum", xmlPath) != "")
                    setMaxNum = Convert.ToInt64(ub.GetSub("SiteMaxNum", xmlPath));

                setMaxNumGid = ub.GetSub("SiteMaxNumGid", xmlPath);

                //每场每项每ID下注限次数:(0为不限制)
                if (setPayNum != 0)
                {
                    getPayNum = new TPR2.BLL.guess.BaPay().GetCount(gid, Convert.ToInt32(modelBaList.p_type), p, meid);
                    if (getPayNum >= setPayNum)
                    {
                        Utils.Error("你在该场选项的下注已达上限", "");
                    }
                }

                //每场每ID下注额
                long setPayCents = Utils.ParseInt64(ub.GetSub("SitePayCent", xmlPath));
                if (setPayCents != 0)
                {
                    long myPayCents = new TPR2.BLL.guess.BaPay().GetBaPayCent(gid, Convert.ToInt32(modelBaList.p_type), meid);
                    if (myPayCents + Convert.ToInt64(payCent) > setPayCents)
                    {
                        if (myPayCents >= setPayCents)
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz") + "，欢迎在下场下注", "");
                        }
                        else
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz") + "，你现在最多可以下注" + (setPayCents - myPayCents) + "" + ub.Get("SiteBz") + "", "");
                        }

                    }
                }

                //" + ub.Get("SiteGqText") + "控制币额上限
                //if (modelBaList.p_ison == 1)
                //{
                //long zqgcent = 0;
                //long usergqcent = 0;
                //if (modelBaList.p_type == 1)
                //{
                //    zqgcent = Utils.ParseInt64(ub.GetSub("SitezqGcent", xmlPath));
                //    usergqcent = new TPR2.BLL.guess.BaPay().GetBaPayCent(gid, 1, Convert.ToDateTime(modelBaList.p_TPRtime));
                //}
                //else
                //{
                //    zqgcent = Utils.ParseInt64(ub.GetSub("SitelqGcent", xmlPath));
                //    usergqcent = new TPR2.BLL.guess.BaPay().GetBaPayCent(gid, 2, Convert.ToDateTime(modelBaList.p_TPRtime));
                //}
                //if (usergqcent + Convert.ToInt64(payCent) > zqgcent)
                //{
                //    if (usergqcent >= zqgcent)
                //    {
                //        Utils.Error("系统限制本场" + ub.Get("SiteGqText") + "下注上限" + zqgcent + "" + ub.Get("SiteBz") + "，欢迎在下场下注", "");
                //    }
                //    else
                //    {
                //        Utils.Error("系统限制本场" + ub.Get("SiteGqText") + "下注上限" + zqgcent + "" + ub.Get("SiteBz") + "，你现在最多可以下注" + (zqgcent - usergqcent) + "" + ub.Get("SiteBz") + "", "");
                //    }

                //}
                //}
                //else {

                int iType = 0;
                long SCent = 0;
                long DCent = 0;
                long BCent = 0;
                if (modelBaList.p_type == 1)
                {

                    if (modelBaList.p_basketve != 9)
                    {
                        if (modelBaList.p_ison == 1)
                            iType = GetLevenTypeZD(modelBaList.p_title);
                        else
                            iType = GetLevenType(modelBaList.p_title);

                        SCent = Utils.ParseInt64(ub.GetSub("SiteScent" + iType + "", xmlPath));
                        DCent = Utils.ParseInt64(ub.GetSub("SiteDcent" + iType + "", xmlPath));
                        BCent = Utils.ParseInt64(ub.GetSub("SiteBcent" + iType + "", xmlPath));
                    }
                    else
                    {
                        iType = GetLevenType3(modelBaList.p_title);

                        SCent = Utils.ParseInt64(ub.GetSub("SiteBScent" + iType + "", xmlPath));
                        DCent = Utils.ParseInt64(ub.GetSub("SiteBDcent" + iType + "", xmlPath));
                        BCent = Utils.ParseInt64(ub.GetSub("SiteBBcent" + iType + "", xmlPath));
                    }
                }
                else
                {
                    if (modelBaList.p_basketve == 0)
                    {
                        if (modelBaList.p_ison == 1)
                            iType = GetLevenType2ZD(modelBaList.p_title);
                        else
                            iType = GetLevenType2(modelBaList.p_title);

                        SCent = Utils.ParseInt64(ub.GetSub("SiteScentb" + iType + "", xmlPath));
                        DCent = Utils.ParseInt64(ub.GetSub("SiteDcentb" + iType + "", xmlPath));
                    }
                    else
                    {
                        iType = GetLevenType4(modelBaList.p_title);

                        SCent = Utils.ParseInt64(ub.GetSub("SiteBScentb" + iType + "", xmlPath));
                        DCent = Utils.ParseInt64(ub.GetSub("SiteBDcentb" + iType + "", xmlPath));

                    }
                }

                //---------------------------------浮动限制-------------------------------------

                if (iType > 0)
                {
                    if (p == 1 || p == 2)
                    {
                        if (SCent > 0)
                        {
                            long Cent = 0;
                            long Cent2 = 0;
                            if (modelBaList.p_ison == 1)
                            {
                                Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 1, Convert.ToDateTime(modelBaList.p_TPRtime));
                                Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 2, Convert.ToDateTime(modelBaList.p_TPRtime));
                            }
                            else
                            {
                                Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 1);
                                Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 2);
                            }

                            if (p == 1)
                            {
                                if (Cent > Cent2)
                                {
                                    if ((Cent + Convert.ToInt64(payCent) - Cent2) > SCent)
                                        Utils.Error("主队超出系统投注币额，请稍后再试", "");
                                }
                            }
                            else
                            {
                                if (Cent2 > Cent)
                                {
                                    if ((Cent2 + Convert.ToInt64(payCent) - Cent) > SCent)
                                        Utils.Error("客队超出系统投注币额，请稍后再试", "");
                                }

                            }
                        }
                    }
                    else if (p == 3 || p == 4)
                    {

                        if (DCent > 0)
                        {
                            long Cent = 0;
                            long Cent2 = 0;
                            if (modelBaList.p_ison == 1)
                            {
                                Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 3, Convert.ToDateTime(modelBaList.p_TPRtime));
                                Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 4, Convert.ToDateTime(modelBaList.p_TPRtime));
                            }
                            else
                            {
                                Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 3);
                                Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 4);
                            }

                            if (p == 3)
                            {
                                if (Cent > Cent2)
                                {
                                    if ((Cent + Convert.ToInt64(payCent) - Cent2) > DCent)
                                        Utils.Error("大球超出系统投注币额，请稍后再试", "");
                                }
                            }
                            else
                            {
                                if (Cent2 > Cent)
                                {
                                    if ((Cent2 + Convert.ToInt64(payCent) - Cent) > DCent)
                                        Utils.Error("小球超出系统投注币额，请稍后再试", "");
                                }

                            }
                        }
                    }
                    else if (p >= 5)
                    {
                        if (BCent > 0)
                        {
                            long Cent = 0;
                            long Cent2 = 0;
                            long Cent3 = 0;
                            if (modelBaList.p_ison == 1)
                            {
                                Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 5, Convert.ToDateTime(modelBaList.p_TPRtime));
                                Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 6, Convert.ToDateTime(modelBaList.p_TPRtime));
                                Cent3 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 7, Convert.ToDateTime(modelBaList.p_TPRtime));
                            }
                            else
                            {
                                Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 5);
                                Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 6);
                                Cent3 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 7);
                            }
                            if ((Cent + Cent2 + Cent3 + Convert.ToInt64(payCent)) > BCent)
                            {
                                Utils.Error("标准盘超出系统投注币额，请稍后再试", "");
                            }
                        }
                    }
                }

            }
            //}
            //组合显示串
            string payview = "";
            if (modelBaList.p_type == 1)
            {
                if (p == 1 || p == 2)
                    payview += "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "(" + GCK.getZqPn(Convert.ToInt32(modelBaList.p_pn)) + GCK.getPkName(Convert.ToInt32(modelBaList.p_pk)) + ")" + modelBaList.p_two + "[/url]";
                else if (p == 3 || p == 4)
                    payview += "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "(大小球" + GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk)) + ")" + modelBaList.p_two + "[/url]";
                else
                    payview += "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "(主" + Convert.ToDouble(modelBaList.p_bzs_lu) + "|平" + Convert.ToDouble(modelBaList.p_bzp_lu) + "|客" + Convert.ToDouble(modelBaList.p_bzx_lu) + ")" + modelBaList.p_two + "[/url]";
            }
            else
            {
                if (p == 1 || p == 2)
                    payview += "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "(" + Convert.ToDouble(modelBaList.p_pk) + ")" + modelBaList.p_two + "[/url]>";
                else
                    payview += "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "(大小球" + Convert.ToDouble(modelBaList.p_dx_pk).ToString() + ")" + modelBaList.p_two + "[/url]";

            }

            string Sison = string.Empty;
            if (modelBaList.p_ison == 1)//购买" + ub.Get("SiteGqText") + "赛事时显示当时比分
            {
                Sison = "(" + Convertp_once(modelBaList.p_once) + "@" + modelBaList.p_result_temp1 + ":" + modelBaList.p_result_temp2 + ")";
            }

            double nodds = 0;
            double npk = 0;
            if (p == 1)
            {
                if (Convert.ToDouble(modelBaList.p_one_lu) > 4)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押" + modelBaList.p_one + "(" + Convert.ToDouble(modelBaList.p_one_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_one_lu);
                npk = Convert.ToDouble(modelBaList.p_pk);
            }
            if (p == 2)
            {
                if (Convert.ToDouble(modelBaList.p_two_lu) > 4)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押" + modelBaList.p_two + "(" + Convert.ToDouble(modelBaList.p_two_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_two_lu);
                npk = Convert.ToDouble(modelBaList.p_pk);

            }
            if (p == 3)
            {
                if (Convert.ToDouble(modelBaList.p_big_lu) > 4)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押大球(" + Convert.ToDouble(modelBaList.p_big_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_big_lu);
                npk = Convert.ToDouble(modelBaList.p_dx_pk);

            }
            if (p == 4)
            {
                if (Convert.ToDouble(modelBaList.p_small_lu) > 4)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押小球(" + Convert.ToDouble(modelBaList.p_small_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_small_lu);
                npk = Convert.ToDouble(modelBaList.p_dx_pk);

            }
            if (p == 5)
            {
                if (Convert.ToDouble(modelBaList.p_bzs_lu) > 50)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押主胜(" + Convert.ToDouble(modelBaList.p_bzs_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_bzs_lu);
            }
            if (p == 6)
            {
                if (Convert.ToDouble(modelBaList.p_bzp_lu) > 50)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押平手(" + Convert.ToDouble(modelBaList.p_bzp_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_bzp_lu);
            }
            if (p == 7)
            {
                if (Convert.ToDouble(modelBaList.p_bzx_lu) > 50)
                {
                    Utils.Error("赔率异常，请联系客服", "");
                }
                payview += "押客胜(" + Convert.ToDouble(modelBaList.p_bzx_lu) + ")" + Sison + ",投" + payCent + "" + BzText + "";
                nodds = Convert.ToDouble(modelBaList.p_bzx_lu);
            }

            double odds = double.Parse(Utils.GetRequest("odds", "post", 2, @"", "下注无效"));
            double pk = double.Parse(Utils.GetRequest("pk", "post", 2, @"", "下注无效"));
            int pn = int.Parse(Utils.GetRequest("pn", "post", 2, @"", "下注无效"));

            //支付安全提示
            string[] p_pageArr = { "ac", "gid", "payCent", "p", "odds", "pk", "pn" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            //赔率变动提示
            if (Request["info"] != "ok")
            {

                bool IsCheck = false;
                bool IsCheck2 = false;
                bool IsCheck3 = false;
                if (Math.Abs(nodds - odds) >= 0.2)
                {
                    IsCheck = true;
                }
                if (npk != pk)
                {
                    IsCheck2 = true;
                }
                if (Convert.ToInt32(modelBaList.p_pn) != pn)
                {
                    IsCheck3 = true;
                }

                if (IsCheck == true || IsCheck2 == true || IsCheck3 == true)
                {
                    new Out().head(Utils.ForWordType("温馨提示"));
                    Response.Write(Out.Tab("<div class=\"title\">", ""));
                    Response.Write("温馨提示");
                    Response.Write(Out.Tab("</div>", "<br />"));

                    Response.Write(Out.Tab("<div class=\"text\">", ""));
                    if (IsCheck3 == true)
                    {
                        Response.Write("受让由(" + GCK.getZqPn(pn) + ")变为(" + GCK.getZqPn(Convert.ToInt32(modelBaList.p_pn)) + ")<br />");

                    }
                    if (IsCheck2 == true)
                    {
                        if (modelBaList.p_type == 1)
                        {
                            if (p == 1 || p == 2)
                                Response.Write("让球盘口由(" + GCK.getPkName(Convert.ToInt32(pk)) + ")变为(" + GCK.getPkName(Convert.ToInt32(npk)) + ")<br />");
                            else
                                Response.Write("大小盘口由(" + GCK.getDxPkName(Convert.ToInt32(pk)) + ")变为(" + GCK.getDxPkName(Convert.ToInt32(npk)) + ")<br />");
                        }
                        else
                        {
                            if (p == 1 || p == 2)
                                Response.Write("让球盘口由(" + pk + ")变为(" + npk + ")<br />");
                            else
                                Response.Write("大小盘口由(" + pk + ")变为(" + npk + ")<br />");
                        }

                    }
                    if (IsCheck == true)
                    {
                        Response.Write("水位由" + odds + "变为" + nodds + "");
                    }
                    Response.Write(Out.Tab("</div>", "<br />"));

                    string strName = "gid,payCent,p,odds,pk,pn,types,info";
                    string strValu = "" + gid + "'" + payCent + "'" + p + "'" + odds + "'" + pk + "'" + pn + "'" + types + "'ok";
                    string strOthe = "确定下注,payGuess.aspx,post,0,red";

                    Response.Write(Out.wapform(strName, strValu, strOthe));

                    Response.Write(Out.Tab("<div>", "<br />"));
                    Response.Write("<a href=\"" + Utils.getUrl("showGuess.aspx?gid=" + gid + "") + "\">[取消返回]</a><br />");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }
            }

            //是否刷屏
            long small = Convert.ToInt64(ub.GetSub("SiteSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("SiteBigPay", xmlPath));
            string appName = "LIGHT_GUESS";
            int Expir = Utils.ParseInt(ub.GetSub("SiteExpir", xmlPath));

            BCW.User.Users.IsFresh(appName, Expir, Convert.ToInt64(payCent), small, big);
            //写入bapay
            string mename = new BCW.BLL.User().GetUsName(meid);
            TPR2.Model.guess.BaPay model = new TPR2.Model.guess.BaPay();
            model.Types = types;
            model.payview = payview;
            model.payusid = meid;
            model.payusname = mename;
            model.bcid = gid;
            model.pType = modelBaList.p_type;
            model.PayType = p;
            model.payCent = payCent;

            if (p == 1 || p == 2)
            {
                model.payonLuone = modelBaList.p_one_lu;
                model.payonLutwo = modelBaList.p_two_lu;
                model.payonLuthr = 0;
                model.p_oncetime2 = modelBaList.p_temptime1;
            }
            else if (p == 3 || p == 4)
            {
                model.payonLuone = modelBaList.p_big_lu;
                model.payonLutwo = modelBaList.p_small_lu;
                model.payonLuthr = 0;
                model.p_oncetime2 = modelBaList.p_temptime2;
            }
            else
            {
                model.payonLuone = modelBaList.p_bzs_lu;
                model.payonLutwo = modelBaList.p_bzp_lu;
                model.payonLuthr = modelBaList.p_bzx_lu;
                model.p_oncetime2 = modelBaList.p_temptime3;
            }
            if (model.p_oncetime2 == null || model.p_oncetime2 < Convert.ToDateTime("1990-1-1"))
            {
                model.p_oncetime2 = Convert.ToDateTime("1990-1-1");
            }
            model.p_pk = modelBaList.p_pk;
            if (string.IsNullOrEmpty(modelBaList.p_dx_pk.ToString()))
                modelBaList.p_dx_pk = 0;

            model.p_dx_pk = modelBaList.p_dx_pk;
            model.p_pn = modelBaList.p_pn;
            model.paytimes = DateTime.Now;
            if (modelBaList.p_type == 1)
            {
                model.p_result_temp1 = modelBaList.p_result_temp1;
                model.p_result_temp2 = modelBaList.p_result_temp2;
            }
            else
            {
                model.p_result_temp1 = 0;
                model.p_result_temp2 = 0;
            }
            model.itypes = 0;
            if (modelBaList.p_ison == 1)
            {
                model.state = 1;
                if (modelBaList.p_basketve == 9)
                    model.state = 3;

            }
            else
            {
                model.state = 2;//未开赛的需要验证水位的投注
            }
            //下注受限
            if (("#" + modelBaList.xID0 + "#").Contains("#" + meid + "#"))
            {
                model.state = 4;//受限标识
            }
            if (p == 1 && ("#" + modelBaList.xID1 + "#").Contains("#" + meid + "#"))
            {
                model.state = 4;//受限标识
            }
            if (p == 2 && ("#" + modelBaList.xID2 + "#").Contains("#" + meid + "#"))
            {
                model.state = 4;//受限标识
            }
            if (p == 3 && ("#" + modelBaList.xID3 + "#").Contains("#" + meid + "#"))
            {
                model.state = 4;//受限标识
            }
            if (p == 4 && ("#" + modelBaList.xID4 + "#").Contains("#" + meid + "#"))
            {
                model.state = 4;//受限标识
            }

            model.p_TPRtime = Convert.ToDateTime(modelBaList.p_TPRtime);

            int pid = new TPR2.BLL.guess.BaPay().Add(model);
            //操作币
            if (types == 0)
                new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(payCent), "球彩下注记录" + gid + "-" + pid + "|" + payview);
            else
                new BCW.BLL.User().UpdateiMoney(meid, -Convert.ToInt64(payCent), "球彩下注记录" + gid + "-" + pid + "|" + payview);

            //   活跃抽奖入口_20160621姚志光
            try
            {
                //表中存在虚拟球类记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName("虚拟球类"))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (Convert.ToInt64(payCent) > new BCW.BLL.tb_WinnersGame().GetPrice("虚拟球类"))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, pid, meid, mename, "球彩", 3);
                        if (hit == 1)
                        {
                            //内线开关 1开
                            if (WinnersGuessOpen == "1")
                            {
                                //发内线到该ID
                                new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                            }
                        }
                    }
                }
            }
            catch { }
            string TzMaxGuest = "#" + ub.GetSub("SiteTzMaxGuest", xmlPath) + "#";
            if (!TzMaxGuest.Contains("#" + gid + "#"))
            {
                long gidPrices = new TPR2.BLL.guess.BaPay().GetBaPayCent(gid, Convert.ToInt32(modelBaList.p_type));

                if (gidPrices >= 2000000)
                {
                    //new BCW.BLL.Guest().Add(10086, "客服", "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]达200w，请检查");
                    //new BCW.BLL.Guest().Add(1010, "客服", "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]达200w，请检查");

                    new BCW.BLL.Guest().Add(10086, "10086", "球赛ID：" + gid + "超2百W，请检查");
                    //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
                    //{
                    //    new BCW.BLL.Guest().Add(1010, "1010", "球赛ID：" + gid + "超2百W，请检查");
                    //}

                    ub xml = new ub();
                    xml.ReloadSub(xmlPath); //加载配置
                    xml.dss["SiteTzMaxGuest"] = xml.dss["SiteTzMaxGuest"] + "#" + gid;

                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }

            }

            string txIDS = "#" + ub.GetSub("SitetxIDS", xmlPath) + "#";
            if (txIDS.Contains("#" + meid + "#"))
            {

                new BCW.BLL.Guest().Add(10086, "管理员", "会员ID" + meid + "在" + payview + "，请检查|[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]查看比赛[/url]");
                //new BCW.BLL.Guest().Add(1010, "管理员", "会员ID" + meid + "在" + payview + "，请检查|[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]查看比赛[/url]");


            }

            if (modelBaList.p_ison == 1)
            {


                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/guess2/default.aspx]球彩[/url]:[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]" + ub.Get("SiteGqText") + "下注**" + BzText + "";
                new BCW.BLL.Action().Add(5, 0, meid, "", wText);

                string sText = "";
                if (st.p_type == 1)
                {
                    sText = "恭喜，提交成功，请等待系统确认..<br />进入" + Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + "查看" + ub.Get("SiteGqText") + "下注是否成功";
                }
                else
                {
                    sText = "恭喜，提交成功，请等待系统确认..<br />" + Out.SysUBB("[红]成功确认后按提交时盘口水位理赔，确认失败后自动退回。[/红]") + "<br />进入" + Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + "查看" + ub.Get("SiteGqText") + "下注是否成功";
                }

                Utils.Success("" + ub.Get("SiteGqText") + "下注", sText, Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "5");
            }
            else
            {

                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/guess2/default.aspx]球彩[/url]:[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]下注**" + BzText + "";
                new BCW.BLL.Action().Add(5, 0, meid, "", wText);

                string sText = "";
                if (st.p_type == 1)
                {
                    sText = "恭喜，提交成功，请等待系统确认..<br />进入" + Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + "查看下注是否成功";
                }
                else
                {
                    sText = "恭喜，提交成功，请等待系统确认..<br />" + Out.SysUBB("[红]成功确认后按提交时盘口水位理赔，确认失败后自动退回。[/红]") + "<br />进入" + Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + "查看下注是否成功";
                }
                Utils.Success("下注", sText, Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "5");
            }
        }
        else
        {
            string getdxpk;
            if (modelBaList.p_type == 1)
                getdxpk = GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk));
            else
                getdxpk = Convert.ToDouble(modelBaList.p_dx_pk).ToString();

            builder.Append(Out.Tab("<div class=\"title\">", ""));

            double iodds = 0;
            double ipk = 0;
            if (p == 1)
            {
                builder.Append("让球盘:" + modelBaList.p_one + "赔率:" + Convert.ToDouble(modelBaList.p_one_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_one_lu);
                ipk = Convert.ToDouble(modelBaList.p_pk);
            }
            else if (p == 2)
            {
                builder.Append("让球盘:" + modelBaList.p_two + "赔率:" + Convert.ToDouble(modelBaList.p_two_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_two_lu);
                ipk = Convert.ToDouble(modelBaList.p_pk);
            }
            else if (p == 3)
            {
                builder.Append("大小盘:大&gt;" + getdxpk + "赔率:" + Convert.ToDouble(modelBaList.p_big_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_big_lu);
                ipk = Convert.ToDouble(modelBaList.p_dx_pk);
            }
            else if (p == 4)
            {
                builder.Append("大小盘:小&gt;" + getdxpk + "赔率:" + Convert.ToDouble(modelBaList.p_small_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_small_lu);
                ipk = Convert.ToDouble(modelBaList.p_dx_pk);
            }
            else if (p == 5)
            {
                builder.Append("标准盘:主胜&gt;" + modelBaList.p_one + "赔率:" + Convert.ToDouble(modelBaList.p_bzs_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_bzs_lu);

            }
            else if (p == 6)
            {
                builder.Append("标准盘:平手&gt; 赔率:" + Convert.ToDouble(modelBaList.p_bzp_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_bzp_lu);

            }
            else if (p == 7)
            {
                builder.Append("标准盘:客胜&gt;" + modelBaList.p_two + "赔率:" + Convert.ToDouble(modelBaList.p_bzx_lu) + "含本金");
                iodds = Convert.ToDouble(modelBaList.p_bzx_lu);

            }

            builder.Append(Out.Tab("</div>", ""));

            string strText = "输入,,,,,";
            string strName = "payCent,odds,pk,pn,gid,p";
            string strType = "num,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + iodds + "'" + ipk + "'" + Convert.ToInt32(modelBaList.p_pn) + "'" + gid + "'" + p + "";
            string strEmpt = "true,true,true,true,true,true";
            string strIdea = "/限" + ub.GetSub("SiteSmallPay", xmlPath) + "-" + ub.GetSub("SiteBigPay", xmlPath) + "" + ub.Get("SiteBz") + "/";

            int iType = 0;
            long SCent = 0;
            long DCent = 0;
            long BCent = 0;
            if (modelBaList.p_type == 1)
            {
                if (modelBaList.p_ison == 1)
                    iType = GetLevenTypeZD(modelBaList.p_title);
                else
                    iType = GetLevenType(modelBaList.p_title);

                SCent = Utils.ParseInt64(ub.GetSub("SiteScent" + iType + "", xmlPath));
                DCent = Utils.ParseInt64(ub.GetSub("SiteDcent" + iType + "", xmlPath));
                BCent = Utils.ParseInt64(ub.GetSub("SiteBcent" + iType + "", xmlPath));
            }
            else
            {
                if (modelBaList.p_ison == 1)
                    iType = GetLevenType2ZD(modelBaList.p_title);
                else
                    iType = GetLevenType2(modelBaList.p_title);

                SCent = Utils.ParseInt64(ub.GetSub("SiteScentb" + iType + "", xmlPath));
                DCent = Utils.ParseInt64(ub.GetSub("SiteDcentb" + iType + "", xmlPath));
            }

            if (p == 1 || p == 2)
            {
                if (SCent > 0)
                {
                    long Cent = 0;
                    long Cent2 = 0;
                    if (modelBaList.p_ison == 1)
                    {
                        Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 1, Convert.ToDateTime(modelBaList.p_TPRtime));
                        Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 2, Convert.ToDateTime(modelBaList.p_TPRtime));
                    }
                    else
                    {
                        Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 1);
                        Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 2);
                    }
                    if (p == 1)
                    {
                        if (Cent > Cent2)
                        {
                            if (Cent > SCent)
                                strIdea += "提示:主队还可以下注" + Math.Abs(Cent - Cent2 - SCent) + "" + ub.Get("SiteBz") + "/";
                            else
                                strIdea += "提示:主队还可以下注" + Math.Abs(SCent - Cent + Cent2) + "" + ub.Get("SiteBz") + "/";
                        }
                        else
                        {
                            strIdea += "提示:主队还可以下注" + Math.Abs(Cent2 - Cent + SCent) + "" + ub.Get("SiteBz") + "/";
                        }
                    }
                    else
                    {
                        if (Cent2 > Cent)
                        {
                            if (Cent > SCent)
                                strIdea += "提示:客队还可以下注" + Math.Abs(Cent2 - Cent - SCent) + "" + ub.Get("SiteBz") + "/";
                            else
                                strIdea += "提示:客队还可以下注" + Math.Abs(SCent - Cent2 + Cent) + "" + ub.Get("SiteBz") + "/";
                        }
                        else
                        {
                            strIdea += "提示:客队还可以下注" + Math.Abs(Cent - Cent2 + SCent) + "" + ub.Get("SiteBz") + "/";
                        }
                    }
                }
            }
            if (p == 3 || p == 4)
            {
                if (DCent > 0)
                {
                    long Cent = 0;
                    long Cent2 = 0;
                    if (modelBaList.p_ison == 1)
                    {
                        Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 3, Convert.ToDateTime(modelBaList.p_TPRtime));
                        Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 4, Convert.ToDateTime(modelBaList.p_TPRtime));
                    }
                    else
                    {
                        Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 3);
                        Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 4);
                    }
                    if (p == 3)
                    {
                        if (Cent > Cent2)
                        {
                            if (Cent > DCent)
                                strIdea += "提示:大球还可以下注" + Math.Abs(Cent - Cent2 - DCent) + "" + ub.Get("SiteBz") + "/";
                            else
                                strIdea += "提示:大球还可以下注" + Math.Abs(DCent - Cent + Cent2) + "" + ub.Get("SiteBz") + "/";
                        }
                        else
                        {
                            strIdea += "提示:大球还可以下注" + Math.Abs(Cent2 - Cent + DCent) + "" + ub.Get("SiteBz") + "/";
                        }
                    }
                    else
                    {
                        if (Cent2 > Cent)
                        {
                            if (Cent > DCent)
                                strIdea += "提示:小球还可以下注" + Math.Abs(Cent2 - Cent - DCent) + "" + ub.Get("SiteBz") + "/";
                            else
                                strIdea += "提示:小球还可以下注" + Math.Abs(DCent - Cent2 + Cent) + "" + ub.Get("SiteBz") + "/";
                        }
                        else
                        {
                            strIdea += "提示:小球还可以下注" + Math.Abs(Cent - Cent2 + DCent) + "" + ub.Get("SiteBz") + "/";
                        }
                    }
                }
            }

            if (p >= 5)
            {
                if (BCent > 0)
                {
                    long Cent = 0;
                    long Cent2 = 0;
                    long Cent3 = 0;
                    if (modelBaList.p_ison == 1)
                    {
                        Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 5, Convert.ToDateTime(modelBaList.p_TPRtime));
                        Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 6, Convert.ToDateTime(modelBaList.p_TPRtime));
                        Cent3 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 7, Convert.ToDateTime(modelBaList.p_TPRtime));
                    }
                    else
                    {
                        Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 5);
                        Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 6);
                        Cent3 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), 7);
                    }

                    if ((Cent + Cent2 + Cent3) > 0)
                    {
                        strIdea += "提示:标准盘还可以下注" + Math.Abs(BCent - Cent - Cent2 - Cent3) + "" + ub.Get("SiteBz") + "/";
                    }
                    else
                    {
                        strIdea += "提示:标准盘还可以下注" + BCent + "" + ub.Get("SiteBz") + "/";
                    }
                }
            }


            string strOthe = "" + ub.Get("SiteBz") + "下注|" + ub.Get("SiteBz2") + "下注,payGuess.aspx,post,0,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("=" + ub.Get("SiteBz") + "快捷下注=");
            builder.Append(Out.Tab("</div>", "<br />"));
            //快捷下注
            strText = ",,,,,";
            strName = "gid,p,odds,pk,pn";
            strType = "hidden,hidden,hidden,hidden,hidden";
            strValu = "" + gid + "'" + p + "'" + iodds + "'" + ipk + "'" + Convert.ToInt32(modelBaList.p_pn) + "";
            strEmpt = "true,true,true,true,true";
            strIdea = "";
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                strOthe = "50万|100万|200万|500万,payGuess.aspx,post,3,other|other|other|other";
            }
            else
            {
                strOthe = "100万|200万|500万|1000万,payGuess.aspx,post,3,other|other|other|other";

            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(money) + "" + ub.Get("SiteBz2") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "取消下注"));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));

    }

    /// <summary>
    /// 足球分级
    /// </summary>
    private int GetLevenType(string Title)
    {
        string Leven1 = "#" + ub.GetSub("SiteLeven1", xmlPath) + "#";
        string Leven2 = "#" + ub.GetSub("SiteLeven2", xmlPath) + "#";
        string Leven3 = "#" + ub.GetSub("SiteLeven3", xmlPath) + "#";
        string Leven4 = "#" + ub.GetSub("SiteLeven4", xmlPath) + "#";
        Title = "#" + Title + "#";
        int iType = 0;
        if (Leven1.Contains(Title))
        {
            iType = 1;
        }
        else if (Leven2.Contains(Title))
        {
            iType = 2;
        }
        else if (Leven3.Contains(Title))
        {
            iType = 3;
        }
        else if (Leven4.Contains(Title))
        {
            iType = 4;
        }
        return iType;
    }

    /// <summary>
    /// 足球分级（走地）
    /// </summary>
    private int GetLevenTypeZD(string Title)
    {
        string Leven1 = "#" + ub.GetSub("SiteLeven5", xmlPath) + "#";
        string Leven2 = "#" + ub.GetSub("SiteLeven6", xmlPath) + "#";
        string Leven3 = "#" + ub.GetSub("SiteLeven7", xmlPath) + "#";
        string Leven4 = "#" + ub.GetSub("SiteLeven8", xmlPath) + "#";
        Title = "#" + Title + "#";
        int iType = 0;
        if (Leven1.Contains(Title))
        {
            iType = 5;
        }
        else if (Leven2.Contains(Title))
        {
            iType = 6;
        }
        else if (Leven3.Contains(Title))
        {
            iType = 7;
        }
        else if (Leven4.Contains(Title))
        {
            iType = 8;
        }
        return iType;
    }

    /// <summary>
    /// 篮球分级
    /// </summary>
    private int GetLevenType2(string Title)
    {
        string Leven1 = "#" + ub.GetSub("SiteLevenb1", xmlPath) + "#";
        string Leven2 = "#" + ub.GetSub("SiteLevenb2", xmlPath) + "#";
        string Leven3 = "#" + ub.GetSub("SiteLevenb3", xmlPath) + "#";
        string Leven4 = "#" + ub.GetSub("SiteLevenb4", xmlPath) + "#";
        Title = "#" + Title + "#";
        int iType = 0;
        if (Leven1.Contains(Title))
        {
            iType = 1;
        }
        else if (Leven2.Contains(Title))
        {
            iType = 2;
        }
        else if (Leven3.Contains(Title))
        {
            iType = 3;
        }
        else if (Leven4.Contains(Title))
        {
            iType = 4;
        }
        return iType;
    }

    /// <summary>
    /// 篮球分级（走地）
    /// </summary>
    private int GetLevenType2ZD(string Title)
    {
        string Leven1 = "#" + ub.GetSub("SiteLevenb5", xmlPath) + "#";
        string Leven2 = "#" + ub.GetSub("SiteLevenb6", xmlPath) + "#";
        string Leven3 = "#" + ub.GetSub("SiteLevenb7", xmlPath) + "#";
        string Leven4 = "#" + ub.GetSub("SiteLevenb8", xmlPath) + "#";
        Title = "#" + Title + "#";
        int iType = 0;
        if (Leven1.Contains(Title))
        {
            iType = 5;
        }
        else if (Leven2.Contains(Title))
        {
            iType = 6;
        }
        else if (Leven3.Contains(Title))
        {
            iType = 7;
        }
        else if (Leven4.Contains(Title))
        {
            iType = 8;
        }
        return iType;
    }


    /// <summary>
    /// 足球分半场分级
    /// </summary>
    private int GetLevenType3(string Title)
    {
        string Leven1 = "#" + ub.GetSub("SiteBLeven1", xmlPath) + "#";
        string Leven2 = "#" + ub.GetSub("SiteBLeven2", xmlPath) + "#";
        string Leven3 = "#" + ub.GetSub("SiteBLeven3", xmlPath) + "#";
        string Leven4 = "#" + ub.GetSub("SiteBLeven4", xmlPath) + "#";
        Title = "#" + Title + "#";
        int iType = 0;
        if (Leven1.Contains(Title))
        {
            iType = 1;
        }
        else if (Leven2.Contains(Title))
        {
            iType = 2;
        }
        else if (Leven3.Contains(Title))
        {
            iType = 3;
        }
        else if (Leven4.Contains(Title))
        {
            iType = 4;
        }
        return iType;
    }

    /// <summary>
    /// 篮球单节分级
    /// </summary>
    private int GetLevenType4(string Title)
    {
        string Leven1 = "#" + ub.GetSub("SiteBLevenb1", xmlPath) + "#";
        string Leven2 = "#" + ub.GetSub("SiteBLevenb2", xmlPath) + "#";
        string Leven3 = "#" + ub.GetSub("SiteBLevenb3", xmlPath) + "#";
        string Leven4 = "#" + ub.GetSub("SiteBLevenb4", xmlPath) + "#";
        Title = "#" + Title + "#";
        int iType = 0;
        if (Leven1.Contains(Title))
        {
            iType = 1;
        }
        else if (Leven2.Contains(Title))
        {
            iType = 2;
        }
        else if (Leven3.Contains(Title))
        {
            iType = 3;
        }
        else if (Leven4.Contains(Title))
        {
            iType = 4;
        }
        return iType;
    }

    private string Convertp_once(string p_once)
    {
        string once = "";
        if (!string.IsNullOrEmpty(p_once))
        {
            if (p_once.Contains("'") && !p_once.Contains("+"))
            {
                try
                {
                    int min = Convert.ToInt32(p_once.Replace("'", ""));
                    if (min > 5)
                        once = (min - 3) + "'";
                    else
                        once = p_once;
                }
                catch
                {
                    once = p_once;
                }
            }
            else
            {
                once = p_once;
            }
        }
        return once;

        //return p_once;
    }
}
