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
using BCW.Common;
using TPR2.Common;

public partial class bbs_game_fbRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/guess2.xml";
    protected string GameName = ub.GetSub("SiteName", "/Controls/guess2.xml");//游戏名字

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("IsBot", xmlPath) != "1")
        {
            Response.Write("close1");
        }
        else
        {

            Play_Robot();
            Robot_case();//机器人兑奖

        }
    }

    //机器人下注
    private void Play_Robot()
    {
        int meid = GetUsID();//得到随机的UsID
        if (meid == 0)
        {
            Response.Write("随机机器人ID出错.请检查ID是否为空.error1");
            Response.End();
        }
        long Price = GetPayCent();//得到随机投注的酷币
        new BCW.BLL.User().UpdateTime(meid, 5);//更新会员在线时长
        int buycou = Convert.ToInt32(ub.GetSub("ROBOTBUY", xmlPath));//xml限定每个机器人每场购买次数
        long gold = new BCW.BLL.User().GetGold(meid);//查询usid的金币
        string BzText = ub.Get("SiteBz");

        int zuqiupay = Convert.ToInt32(ub.GetSub("zuqiupay", xmlPath));//足球下注
        int lanqiupay = Convert.ToInt32(ub.GetSub("lanqiupay", xmlPath));//篮球下注
        string fzzhu = ub.GetSub("fzzhu", xmlPath);//分钟，投多少

        bool ok1 = false;
        bool ok2 = false;

        List<int> aq = new List<int>();
        if (zuqiupay == 1) { aq.Add(1); }
        if (lanqiupay == 1) { aq.Add(2); }

        int time_fen = 0;
        int time_miao = 0;
        if (fzzhu != "")
        {
            try
            {
                string[] pp = fzzhu.Split(',');
                if (int.Parse(pp[0]) >= int.Parse(pp[1]))//如果分钟数大于次数
                {
                    time_fen = int.Parse(pp[0]) / int.Parse(pp[1]);//相隔时间_分钟
                }
                else if (int.Parse(pp[0]) * 60 >= int.Parse(pp[1]))
                {
                    time_miao = int.Parse(pp[0]) * 60 / int.Parse(pp[1]);//相隔时间_秒
                }
                else
                {
                    Response.Write("刷新时间设置太快了!刷新暂停!close1");
                    Response.End();
                }
            }
            catch
            {
                Response.Write("刷新时间出错!刷新暂停!close1");
                Response.End();
            }
            //查询机器人最后一条下注的时间
            DataSet paylast = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP(1)*", "isrobot=1 ORDER BY paytimes DESC");
            if (paylast != null && paylast.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < paylast.Tables[0].Rows.Count; j++)
                {
                    DateTime paytimes = Convert.ToDateTime(paylast.Tables[0].Rows[0]["paytimes"]);//下注时间
                    if (time_fen > 0)
                    {
                        if (DateTime.Now > paytimes.AddMinutes(time_fen).AddSeconds(R(1, 60)))
                        {
                            ok1 = true;
                        }
                    }
                    if (time_miao > 0)
                    {
                        if (DateTime.Now > paytimes.AddSeconds(time_miao).AddSeconds(R(1, 30)))
                        {
                            ok2 = true;
                        }
                    }
                }
            }
            else
            {
                ok1 = true;
            }
            if (ok1 == true || ok2 == true)
            {
                if (aq.Count >= 1)
                {
                    Random ran = new Random();
                    int abc = aq[ran.Next(aq.Count)];//1足球2篮球--开关
                    int zhong = 1;
                    int p = 1;
                    if (abc == 1)
                    {
                        //随机3种：1滚球，2全场，3半场
                        string[] sNum = { "1", "2", "3" };
                        zhong = int.Parse((sNum[ran.Next(sNum.Length)]));
                    }
                    else
                    {
                        //随机2种：4滚球，5全场
                        string[] sNum = { "4", "5" };
                        zhong = int.Parse((sNum[ran.Next(sNum.Length)]));
                    }
                    DataSet ds;
                    if (zhong == 1)
                    {
                        try
                        {
                            ds = new TPR2.BLL.guess.BaList().GetBaListList("top(1) *", "p_type=1 AND p_active=0 and p_del=0 and p_ison=1 and p_isondel=0 and p_basketve=0 order by newid()");//滚球
                        }
                        catch
                        {
                            ds = null;
                            Response.Write("<br/>已选择第1种方式：数据查询失败...<br/>");
                            Response.End();
                        }
                        Response.Write("<br/>已选择第1种方式：足球滚球下注.ID" + meid + "等待下注...<br/>");
                        p = Get_xiazhu();//12让球盘34大小盘567标准盘
                    }
                    else if (zhong == 2)
                    {
                        try
                        {
                            ds = new TPR2.BLL.guess.BaList().GetBaListList("top(1) *", "p_type=1 AND p_active=0 and p_del=0 and p_TPRtime >= GETDATE() and p_basketve=0 order by newid()");//全场
                        }
                        catch
                        {
                            ds = null;
                            Response.Write("<br/>已选择第2种方式：数据查询失败...<br/>");
                            Response.End();
                        }
                        Response.Write("<br/>已选择第2种方式：足球全场下注.ID" + meid + "等待下注...<br/>");
                        p = Get_xiazhu();//12让球盘34大小盘567标准盘
                    }
                    else if (zhong == 3)
                    {
                        try
                        {
                            ds = new TPR2.BLL.guess.BaList().GetBaListList("top(1) *", "p_type=1 AND p_active=0 and p_del=0 and (p_TPRtime >= GETDATE() OR (p_ison=1 and p_isondel=0)) and p_basketve>0 order by newid()");//半场
                        }
                        catch
                        {
                            ds = null;
                            Response.Write("<br/>已选择第3种方式：数据查询失败...<br/>");
                            Response.End();
                        }
                        Response.Write("<br/>已选择第3种方式：足球半场下注.ID" + meid + "等待下注...<br/>");
                        p = Get_xiazhu();//12让球盘34大小盘567标准盘
                    }
                    else if (zhong == 4)
                    {
                        try
                        {
                            ds = new TPR2.BLL.guess.BaList().GetBaListList("top(1) *", "p_type=2 AND p_active=0 and p_del=0 and p_ison=1 and p_isondel=0 and p_basketve=0 order by newid()");//篮球滚球
                        }
                        catch
                        {
                            ds = null;
                            Response.Write("<br/>已选择第4种方式：数据查询失败...<br/>");
                            Response.End();
                        }
                        Response.Write("<br/>已选择第4种方式：篮球滚球下注.ID" + meid + "等待下注...<br/>");
                        p = Get_xiazhu2();//12让球盘34大小盘
                    }
                    else
                    {
                        try
                        {
                            ds = new TPR2.BLL.guess.BaList().GetBaListList("top(1) *", "p_type=2 AND p_active=0 and p_del=0 and p_TPRtime >= GETDATE() and p_basketve=0 order by newid()");//篮球全场
                        }
                        catch
                        {
                            ds = null;
                            Response.Write("<br/>已选择第5种方式：数据查询失败...<br/>");
                            Response.End();
                        }
                        Response.Write("<br/>已选择第5种方式：篮球全场下注.ID" + meid + "等待下注...<br/>");
                        p = Get_xiazhu2();//12让球盘34大小盘
                    }

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            int p_id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);//球赛ID
                            if (p_id > 0 && meid > 0)
                            {
                                //查询该usid和p_id是否已超投注
                                if (buycou == 0 || new TPR2.BLL.guess.BaList().Getzqcount(meid, p_id) <= buycou)
                                {
                                    //更新消费记录
                                    if (gold < Price)
                                    {
                                        BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                                        modelx.BbTag = 3;
                                        modelx.Types = 0;
                                        modelx.PUrl = Utils.getPageUrl();//操作的文件名
                                        modelx.UsId = meid;
                                        modelx.UsName = new BCW.BLL.User().GetUsName(meid);
                                        modelx.AcGold = Price;
                                        modelx.AfterGold = gold + Price;//更新后的币数
                                        modelx.AcText = "系统球彩机器人自动操作.";
                                        modelx.AddTime = DateTime.Now;
                                        new BCW.BLL.Goldlog().Add(modelx);
                                        BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + Price + ") where id=" + meid + "");
                                    }


                                    TPR2.BLL.guess.BaList BaListbll = new TPR2.BLL.guess.BaList();
                                    TPR2.Model.guess.BaList modelBaList;
                                    try
                                    {
                                        modelBaList = BaListbll.GetModel(p_id);
                                    }
                                    catch
                                    {
                                        modelBaList = null;
                                        Response.Write("<br/>GetModel：数据查询失败...<br/>");
                                        Response.End();
                                    }

                                    #region 封盘限制取消下注
                                    if (ub.GetSub("SiteIsbz", "/Controls/guess2.xml") == "1")
                                    {
                                        if (p > 4)
                                        {
                                            break;
                                        }
                                    }
                                    if (p == 1 || p == 2)
                                    {
                                        if ((modelBaList.p_ison == 1 && modelBaList.p_isluckone == 1) || modelBaList.p_isluck == 1)
                                        {
                                            break;
                                        }
                                    }
                                    if (p == 3 || p == 4)
                                    {
                                        if (modelBaList.p_big_lu == -1 || modelBaList.p_dx_pk == 0)
                                        {
                                            break;
                                        }
                                        if ((modelBaList.p_ison == 1 && modelBaList.p_islucktwo == 1) || modelBaList.p_isluck == 1)
                                        {
                                            break;
                                        }
                                    }
                                    if (p > 4)
                                    {
                                        if (modelBaList.p_bzs_lu == -1 || ub.GetSub("SiteIsbz", "/Controls/guess.xml") == "1")
                                        {
                                            break;
                                        }
                                        if ((modelBaList.p_ison == 1 && modelBaList.p_isluckthr == 1) || modelBaList.p_isluck == 1)
                                        {
                                            break;
                                        }
                                    }
                                    if (modelBaList.p_ison == 0)//" + ub.Get("SiteGqText") + "不限制
                                    {
                                        if (modelBaList.p_TPRtime <= DateTime.Now)
                                        {
                                            break;
                                        }
                                    }
                                    if (modelBaList.p_ison == 1)
                                    {
                                        if (modelBaList.p_once == "未")
                                        {
                                            break;
                                        }
                                        try
                                        {
                                            if (modelBaList.p_type == 1)
                                            {
                                                if (Utils.ParseInt(modelBaList.p_once.Replace("'", "").Replace("+", "")) >= 90 || modelBaList.p_once.Contains("加"))
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        catch { }

                                        if (DateTime.Now > Convert.ToDateTime(modelBaList.p_oncetime))
                                        {
                                            break;
                                        }
                                        if (modelBaList.p_once == "完")
                                        {
                                            break;
                                        }
                                        if (modelBaList.p_once.Contains("待定"))
                                        {
                                            break;
                                        }
                                        if (modelBaList.p_once.Contains("腰斩"))
                                        {
                                            break;
                                        }
                                        if (modelBaList.p_once.Contains("推迟"))
                                        {
                                            break;
                                        }
                                        if (modelBaList.p_once.Contains("中断"))
                                        {
                                            break;
                                        }
                                        if (modelBaList.p_type == 1)
                                        {
                                            decimal p_result = Convert.ToDecimal(modelBaList.p_result_temp1 + modelBaList.p_result_temp2);
                                            string p_dx = GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk));
                                            decimal dx = Convert.ToDecimal(p_dx.Split("/".ToCharArray())[0]);
                                            if (p_result >= dx)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (modelBaList.p_result_temp1 != null && modelBaList.p_result_temp2 != null && modelBaList.p_result_temp1 != 0 && modelBaList.p_result_temp2 != 0)
                                        {
                                            break;
                                        }
                                    }
                                    if (modelBaList.p_isondel == 1)
                                    {
                                        Utils.Error("已封盘，暂停下注", "");
                                    }
                                    #endregion

                                    //每场每ID下注额
                                    long setPayCents = Utils.ParseInt64(ub.GetSub("SitePayCent", xmlPath));
                                    if (setPayCents != 0)
                                    {
                                        long myPayCents = new TPR2.BLL.guess.BaPay().GetBaPayCent(p_id, Convert.ToInt32(modelBaList.p_type), meid);
                                        if (myPayCents + Convert.ToInt64(Price) > setPayCents)
                                        {
                                            if (myPayCents >= setPayCents)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    #region 浮动限制
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
                                                    Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 1, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                    Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 2, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                }
                                                else
                                                {
                                                    Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 1);
                                                    Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 2);
                                                }

                                                if (p == 1)
                                                {
                                                    if (Cent > Cent2)
                                                    {
                                                        if ((Cent + Convert.ToInt64(Price) - Cent2) > SCent)
                                                            Utils.Error("主队超出系统投注币额，请稍后再试", "");
                                                    }
                                                }
                                                else
                                                {
                                                    if (Cent2 > Cent)
                                                    {
                                                        if ((Cent2 + Convert.ToInt64(Price) - Cent) > SCent)
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
                                                    Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 3, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                    Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 4, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                }
                                                else
                                                {
                                                    Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 3);
                                                    Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 4);
                                                }

                                                if (p == 3)
                                                {
                                                    if (Cent > Cent2)
                                                    {
                                                        if ((Cent + Convert.ToInt64(Price) - Cent2) > DCent)
                                                            Utils.Error("大球超出系统投注币额，请稍后再试", "");
                                                    }
                                                }
                                                else
                                                {
                                                    if (Cent2 > Cent)
                                                    {
                                                        if ((Cent2 + Convert.ToInt64(Price) - Cent) > DCent)
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
                                                    Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 5, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                    Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 6, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                    Cent3 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 7, Convert.ToDateTime(modelBaList.p_TPRtime));
                                                }
                                                else
                                                {
                                                    Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 5);
                                                    Cent2 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 6);
                                                    Cent3 = new TPR2.BLL.guess.BaPay().GetBaPayCent2(p_id, Convert.ToInt32(modelBaList.p_type), 7);
                                                }
                                                if ((Cent + Cent2 + Cent3 + Convert.ToInt64(Price)) > BCent)
                                                {
                                                    Utils.Error("标准盘超出系统投注币额，请稍后再试", "");
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region payview和nodds
                                    string payview = "";
                                    if (modelBaList.p_type == 1)
                                    {
                                        if (p == 1 || p == 2)
                                            payview += "[url=/bbs/guess2/showguess.aspx?gid=" + p_id + "]" + modelBaList.p_one + "(" + GCK.getZqPn(Convert.ToInt32(modelBaList.p_pn)) + GCK.getPkName(Convert.ToInt32(modelBaList.p_pk)) + ")" + modelBaList.p_two + "[/url]";
                                        else if (p == 3 || p == 4)
                                            payview += "[url=/bbs/guess2/showguess.aspx?gid=" + p_id + "]" + modelBaList.p_one + "(大小球" + GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk)) + ")" + modelBaList.p_two + "[/url]";
                                        else
                                            payview += "[url=/bbs/guess2/showguess.aspx?gid=" + p_id + "]" + modelBaList.p_one + "(主" + Convert.ToDouble(modelBaList.p_bzs_lu) + "|平" + Convert.ToDouble(modelBaList.p_bzp_lu) + "|客" + Convert.ToDouble(modelBaList.p_bzx_lu) + ")" + modelBaList.p_two + "[/url]";
                                    }
                                    else
                                    {
                                        if (p == 1 || p == 2)
                                            payview += "[url=/bbs/guess2/showguess.aspx?gid=" + p_id + "]" + modelBaList.p_one + "(" + Convert.ToDouble(modelBaList.p_pk) + ")" + modelBaList.p_two + "[/url]>";
                                        else
                                            payview += "[url=/bbs/guess2/showguess.aspx?gid=" + p_id + "]" + modelBaList.p_one + "(大小球" + Convert.ToDouble(modelBaList.p_dx_pk).ToString() + ")" + modelBaList.p_two + "[/url]";
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
                                            break;
                                        }
                                        payview += "押" + modelBaList.p_one + "(" + Convert.ToDouble(modelBaList.p_one_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_one_lu);
                                        npk = Convert.ToDouble(modelBaList.p_pk);
                                    }
                                    if (p == 2)
                                    {
                                        if (Convert.ToDouble(modelBaList.p_two_lu) > 4)
                                        {
                                            break;
                                        }
                                        payview += "押" + modelBaList.p_two + "(" + Convert.ToDouble(modelBaList.p_two_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_two_lu);
                                        npk = Convert.ToDouble(modelBaList.p_pk);

                                    }
                                    if (p == 3)
                                    {
                                        if (Convert.ToDouble(modelBaList.p_big_lu) > 4)
                                        {
                                            break;
                                        }
                                        payview += "押大球(" + Convert.ToDouble(modelBaList.p_big_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_big_lu);
                                        npk = Convert.ToDouble(modelBaList.p_dx_pk);

                                    }
                                    if (p == 4)
                                    {
                                        if (Convert.ToDouble(modelBaList.p_small_lu) > 4)
                                        {
                                            break;
                                        }
                                        payview += "押小球(" + Convert.ToDouble(modelBaList.p_small_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_small_lu);
                                        npk = Convert.ToDouble(modelBaList.p_dx_pk);

                                    }
                                    if (p == 5)
                                    {
                                        if (Convert.ToDouble(modelBaList.p_bzs_lu) > 50)
                                        {
                                            break;
                                        }
                                        payview += "押主胜(" + Convert.ToDouble(modelBaList.p_bzs_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_bzs_lu);
                                    }
                                    if (p == 6)
                                    {
                                        if (Convert.ToDouble(modelBaList.p_bzp_lu) > 50)
                                        {
                                            break;
                                        }
                                        payview += "押平手(" + Convert.ToDouble(modelBaList.p_bzp_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_bzp_lu);
                                    }
                                    if (p == 7)
                                    {
                                        if (Convert.ToDouble(modelBaList.p_bzx_lu) > 50)
                                        {
                                            break;
                                        }
                                        payview += "押客胜(" + Convert.ToDouble(modelBaList.p_bzx_lu) + ")" + Sison + ",投" + Price + "" + BzText + "";
                                        nodds = Convert.ToDouble(modelBaList.p_bzx_lu);
                                    }
                                    #endregion

                                    #region iodds和ipk
                                    double iodds = 0;
                                    double ipk = 0;
                                    if (p == 1)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_one_lu);
                                        ipk = Convert.ToDouble(modelBaList.p_pk);
                                    }
                                    else if (p == 2)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_two_lu);
                                        ipk = Convert.ToDouble(modelBaList.p_pk);
                                    }
                                    else if (p == 3)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_big_lu);
                                        ipk = Convert.ToDouble(modelBaList.p_dx_pk);
                                    }
                                    else if (p == 4)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_small_lu);
                                        ipk = Convert.ToDouble(modelBaList.p_dx_pk);
                                    }
                                    else if (p == 5)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_bzs_lu);
                                    }
                                    else if (p == 6)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_bzp_lu);
                                    }
                                    else if (p == 7)
                                    {
                                        iodds = Convert.ToDouble(modelBaList.p_bzx_lu);
                                    }
                                    #endregion


                                    //写入bapay
                                    string mename = new BCW.BLL.User().GetUsName(meid);
                                    TPR2.Model.guess.BaPay model = new TPR2.Model.guess.BaPay();
                                    model.Types = 0;//默认下注酷币
                                    model.payview = payview;
                                    model.payusid = meid;
                                    model.payusname = mename;
                                    model.bcid = p_id;
                                    model.pType = modelBaList.p_type;
                                    model.PayType = p;
                                    model.payCent = Price;

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
                                    model.state = 0;

                                    model.p_TPRtime = Convert.ToDateTime(modelBaList.p_TPRtime);
                                    model.isrobot = 1;
                                    int pid = new TPR2.BLL.guess.BaPay().Add(model);

                                    new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(Price), "球彩下注记录" + p_id + "-" + pid + "|" + payview);
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/guess2/default.aspx]球彩[/url]:[url=/bbs/guess2/showguess.aspx?gid=" + p_id + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]下注**" + BzText + "";
                                    new BCW.BLL.Action().Add(5, 0, meid, "", wText);

                                    Response.Write("机器人" + meid + "下注成功种类：" + p + ".(12让球盘34大小盘567标准盘)====ok1：<br/>" + payview + "<br/>");
                                    Response.Write("机器人" + meid + "已购买ID[" + p_id + "]" + new TPR2.BLL.guess.BaList().Getzqcount(meid, p_id) + "注.<br/>当前时间:" + DateTime.Now + "");
                                }
                                else
                                    Response.Write("该机器人已下该场次.<br/>");
                            }
                            else
                                Response.Write("下注失败.不存在此球彩ID" + p_id + ".<br/>");
                        }
                    }
                    else
                        Response.Write("第" + zhong + "种暂无球彩可下注..<br/>");
                }
                else
                    Response.Write("下注已关闭.<br/>");
            }
            else
                Response.Write("机器人下注过快.请等待刷新..<br/>");
        }
        else
            Response.Write("分钟填写出错.<br/>");
    }

    //机器人兑奖
    public void Robot_case()
    {
        DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("*", "isrobot=1 and p_case=0 and p_getMoney>0 and p_active>0 and itypes=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //本地投注数据
                int pid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int meid = int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString());

                int bcid = new TPR2.BLL.guess.BaPay().Getbcid(pid);
                new TPR2.BLL.guess.BaPay().UpdateIsCase(pid);
                //操作币
                long win = Convert.ToInt64(new TPR2.BLL.guess.BaPay().Getp_getMoney(pid));
                new BCW.BLL.User().UpdateiGold(meid, win, "竞猜赛事ID" + bcid + "#记录ID" + pid + "兑奖");
            }
            Response.Write("机器人兑奖成功ok1");
        }
    }

    //随机得到投注类型：12让球盘34大小盘567标准盘
    private int Get_xiazhu()
    {
        Random rac = new Random();
        int cc = rac.Next(1, 8);
        return cc;
    }

    //随机得到投注类型：12让球盘34大小盘
    private int Get_xiazhu2()
    {
        Random rac = new Random();
        int cc = rac.Next(1, 5);
        return cc;
    }

    protected int R(int x, int y)
    {
        Random ran = new Random();
        int RandKey = ran.Next(x, y);
        return RandKey;
    }

    //随机得到下注的币数
    private long GetPayCent()
    {
        long price = 100;
        string aa = ub.GetSub("ROBOTMIAO", xmlPath);
        if (aa != "")
        {
            string[] sNum = Regex.Split(aa, "#");
            Random rd = new Random();
            try
            {
                price = Convert.ToInt64(sNum[rd.Next(sNum.Length)]);
            }
            catch { }
        }
        else
        {
            price = 100;
        }
        return price;
    }

    //随机得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("ROBOTID", "/Controls/guess2.xml");
        if (PlayUsID != "")
        {
            string[] sNum = Regex.Split(PlayUsID, "#");
            Random rd = new Random();
            try
            {
                UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
            }
            catch { }
        }
        return UsID;
    }

    #region 诸多限制
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

    #endregion

}
