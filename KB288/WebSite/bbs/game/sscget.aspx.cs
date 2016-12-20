using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
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
using System.Net;
using BCW.Common;
using System.Timers;

public partial class bbs_game_sscget : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/ssc.xml";
    protected int gid = 32;
    protected string GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");
    protected int lkstart = Convert.ToInt32(ub.GetSub("lkstart", "/Controls/ssc.xml"));
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    /// <summary>
    ///蒙宗将 20161029 优化 
    ///蒙宗将20161031 返奖修复 更换接口
    /// 蒙宗将 20161101 返奖优化任选 顺子开奖修复 总和大小单双
    /// 蒙宗将 20161111 修复任选2、3普通开奖
    ///        20161116 牛牛算法完善
    ///        20161121 去除有牛无牛浮动 增加连开  22
    ///        25 增加ID限额
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        Response.Write("" + GameName + "数据抓取ok1<br />");
        try
        {
            SSCLISTPAGE();//更新期数
        }
        catch { }

        ///获得最后一期信息
        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClistLast();
        int Sec = Utils.ParseInt(ub.GetSub("SSCSec", xmlPath));

        if (new BCW.ssc.BLL.SSClist().ExistsUpdateResult())
        {
            try
            {
                sscopengame(); //更新结果
                Response.Write("更新开奖结果成功<br />");
            }
            catch { }
        }

        try
        {
            CasePage();//开奖
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

        OddsPage();//浮动赔率

        try
        {
            BCW.ssc.Model.SSClist la = new BCW.ssc.BLL.SSClist().GetSSClistLast();
            BCW.ssc.Model.SSClist lar = new BCW.ssc.BLL.SSClist().GetSSClistLast2();
            Response.Write("现在是第" + la.SSCId + "期<br/>");
            Response.Write("这期的投注截至时间是" + la.EndTime + "<br/>");
            Response.Write("已开奖的最新一期是第" + lar.SSCId + "期<br/>");
            Response.Write("开奖结果是" + lar.Result + "<br />");
        }
        catch
        {

        }
        Response.Write("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
        stopwatch.Stop();
        Response.Write("总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
    }

    //返奖
    private void CasePage()
    {
        //开始返彩
        DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("ID,Types,SSCId,UsID,UsName,Price,Notes,Result,Odds", "State=0 and Result<>''");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                int SSCId = int.Parse(ds.Tables[0].Rows[i]["SSCId"].ToString());
                string Notes = ds.Tables[0].Rows[i]["Notes"].ToString();
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                string Result = ds.Tables[0].Rows[i]["Result"].ToString();
                long Price = Int64.Parse(ds.Tables[0].Rows[i]["Price"].ToString());
                decimal Odds = decimal.Parse(ds.Tables[0].Rows[i]["Odds"].ToString()); //赔率

                #region 返奖
                if (Types == 18)//任一 
                {
                    string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                    string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                    int zj_zs = 0; //统计中奖注数

                    if (BackLikeNum(Result) == 5)//开奖结果五个结果不同
                    {
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                            for (int p = 0; p < iNum_kj.Length; p++)
                                if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                    zj_zs += 1;
                    }
                    else//开奖结果有相同的，把相同的号码合并为一个号码，再比对开奖
                    {
                        string ret = string.Empty;
                        ArrayList list = new ArrayList();
                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            if (!list.Contains(iNum_kj[j]))
                            {
                                list.Add(iNum_kj[j]);
                            }
                        }
                        for (int k = 0; k < list.Count; k++)
                        {
                            ret += list[k] + " ";
                        }

                        string[] kj = ret.Split(' ');
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                            for (int p = 0; p < kj.Length; p++)
                                if (string.Compare(iNum_zj[fs], kj[p]) == 0)
                                    zj_zs += 1;

                    }



                    if (zj_zs > 0)//中奖
                    {
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 16)//任选号码
                {
                    string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                    string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                    int zj_zs = 0; //统计中奖注数

                    for (int fs = 0; fs < iNum_zj.Length; fs++)
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                zj_zs += 1;


                    if (zj_zs > 0)//中奖
                    {
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖倍数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 1 || Types == 4 || Types == 7 || Types == 10 || Types == 13)// 万、千、百、十、个位直选
                {
                    string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                    string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                    int zj_zs = 0; //统计中奖注数

                    int p = 0;
                    if (Types == 1)//万位 
                    {
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                        {
                            if (string.Compare(iNum_zj[fs], iNum_kj[0]) == 0)
                                zj_zs += 1;
                        }
                    }
                    else if (Types == 4)//千位 
                    {
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                        {
                            if (string.Compare(iNum_zj[fs], iNum_kj[1]) == 0)
                                zj_zs += 1;
                        }
                    }
                    else if (Types == 7)//百位
                    {
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                        {
                            if (string.Compare(iNum_zj[fs], iNum_kj[2]) == 0)
                                zj_zs += 1;
                        }
                    }
                    else if (Types == 10)//十位
                    {
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                        {
                            if (string.Compare(iNum_zj[fs], iNum_kj[3]) == 0)
                                zj_zs += 1;
                        }
                    }
                    else if (Types == 13)//个位 
                    {
                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                        {
                            if (string.Compare(iNum_zj[fs], iNum_kj[4]) == 0)
                                zj_zs += 1;
                        }
                    }
                    if (zj_zs > 0)//中奖
                    {
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 19 || Types == 20 || Types == 21 || Types == 22)//任二、三、四、五
                {
                    string[] iNum_kj = Result.Split(' ');  //存放结果
                    string[] iNum_zj = Notes.Split('#');  //把胆码和拖码分开存储
                    string[] iNum_zj_d = iNum_zj[0].Split(','); //胆码
                    string[] iNum_zj_t = iNum_zj[1].Split(',');  //拖码

                    if (iNum_zj[0] == "")//无胆码，为普通选
                    {
                        int zj_zs = 0; //统计中奖注数

                        if (BackLikeNum(Result) == 5)//开奖的五个号不同
                        {
                            for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                for (int p = 0; p < iNum_kj.Length; p++)
                                    if (string.Compare(iNum_zj_t[fs], iNum_kj[p]) == 0)
                                        zj_zs += 1;
                        }
                        else//开奖号有相同的
                        {
                            string ret = string.Empty;
                            ArrayList list = new ArrayList();
                            for (int j = 0; j < iNum_kj.Length; j++)
                            {
                                if (!list.Contains(iNum_kj[j]))
                                {
                                    list.Add(iNum_kj[j]);
                                }
                            }
                            for (int k = 0; k < list.Count; k++)
                            {
                                ret += list[k] + " ";
                            }

                            string[] kj = ret.Split(' ');
                            for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                for (int p = 0; p < kj.Length; p++)
                                    if (string.Compare(iNum_zj_t[fs], kj[p]) == 0)
                                        zj_zs += 1;
                        }


                        switch (Types)
                        {
                            case 22: //任选五普通
                                {
                                    if (zj_zs >= 5 && BackLikeNum(Result) == 5)
                                    {
                                        zj_zs = C(zj_zs, 5);
                                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                        string WinNotes = "中奖注数:" + zj_zs;
                                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                    }
                                    else
                                        zj_zs = 0;
                                }
                                break;
                            case 21: //任选四普通
                                {
                                    if (zj_zs >= 4 && BackLikeNum(Result) >= 4)
                                    {
                                        if (BackLikeNum(Result) == 5)
                                        {
                                            zj_zs = C(zj_zs, 4);
                                        }
                                        if (BackLikeNum(Result) == 4)
                                        {
                                            zj_zs = 1;
                                        }

                                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                        string WinNotes = "中奖注数:" + zj_zs;
                                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                    }
                                    else
                                        zj_zs = 0;
                                }
                                break;
                            case 20: //任选三普通
                                {
                                    if (zj_zs >= 3 && BackLikeNum(Result) >= 3)
                                    {
                                        if (BackLikeNum(Result) == 5)
                                        {
                                            zj_zs = C(zj_zs, 3);
                                        }
                                        if (BackLikeNum(Result) == 4)
                                        {
                                            zj_zs = C(zj_zs, 3);
                                        }
                                        if (BackLikeNum(Result) == 3)
                                        {
                                            zj_zs = 1;
                                        }

                                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                        string WinNotes = "中奖注数:" + zj_zs;
                                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                    }
                                    else
                                        zj_zs = 0;
                                }
                                break;
                            default: //任选二普通
                                {
                                    if (zj_zs >= 2 && BackLikeNum(Result) >= 2)
                                    {
                                        if (BackLikeNum(Result) == 5)
                                        {
                                            zj_zs = C(zj_zs, 2);
                                        }
                                        if (BackLikeNum(Result) == 4)
                                        {
                                            zj_zs = C(zj_zs, 2);
                                        }
                                        if (BackLikeNum(Result) == 3)
                                        {
                                            zj_zs = C(zj_zs, 2);
                                        }
                                        if (BackLikeNum(Result) == 2)
                                        {
                                            zj_zs = 1;
                                        }

                                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                        string WinNotes = "中奖注数:" + zj_zs;
                                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                    }
                                    else
                                        zj_zs = 0;
                                }
                                break;
                        }
                    }
                    else//有胆码，为胆拖
                    {
                        int zj_ds = 0; //中奖的胆码数
                        int zj_ts = 0; //中奖的拖码数
                        int zj = 0; //中奖的注数

                        if (BackLikeNum(Result) == 5)//开奖的五个号不同
                        {
                            //统计胆码的中奖数
                            for (int i1 = 0; i1 < iNum_zj_d.Length; i1++)
                            {
                                for (int p = 0; p < iNum_kj.Length; p++)
                                {
                                    if (iNum_zj_d[i1] == iNum_kj[p])
                                    {
                                        zj_ds += 1;
                                    }
                                }
                            }
                        }
                        else//开奖号有相同的
                        {
                            string ret = string.Empty;
                            ArrayList list = new ArrayList();
                            for (int j = 0; j < iNum_kj.Length; j++)
                            {
                                if (!list.Contains(iNum_kj[j]))
                                {
                                    list.Add(iNum_kj[j]);
                                }
                            }
                            for (int k = 0; k < list.Count; k++)
                            {
                                ret += list[k] + " ";
                            }

                            string[] kj = ret.Split(' ');
                            for (int fs = 0; fs < iNum_zj_d.Length; fs++)
                                for (int p = 0; p < kj.Length; p++)
                                    if (string.Compare(iNum_zj_d[fs], kj[p]) == 0)
                                        zj_ds += 1;
                        }

                        if (zj_ds == iNum_zj_d.Length) //判断胆码是否全中
                        {
                            if (BackLikeNum(Result) == 5)
                            {
                                //统计拖码的中奖数
                                for (int i1 = 0; i1 < iNum_zj_t.Length; i1++)
                                {
                                    for (int p = 0; p < iNum_kj.Length; p++)
                                    {
                                        if (iNum_zj_t[i1] == iNum_kj[p])
                                        {
                                            zj_ts += 1;
                                        }
                                    }
                                }
                            }
                            else//开奖结果有相同的，把相同的号码合并为一个号码，再比对开奖
                            {
                                string ret = string.Empty;
                                ArrayList list = new ArrayList();
                                for (int j = 0; j < iNum_kj.Length; j++)
                                {
                                    if (!list.Contains(iNum_kj[j]))
                                    {
                                        list.Add(iNum_kj[j]);
                                    }
                                }
                                for (int k = 0; k < list.Count; k++)
                                {
                                    ret += list[k] + " ";
                                }

                                string[] kj = ret.Split(' ');
                                for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                    for (int p = 0; p < kj.Length; p++)
                                        if (string.Compare(iNum_zj_t[fs], kj[p]) == 0)
                                            zj_ts += 1;

                            }

                            switch (Types)
                            {
                                case 22: //五胆拖类型
                                    {
                                        if (zj_ds + zj_ts >= 5)
                                        {
                                            zj = C(zj_ts, 5 - iNum_zj_d.Length);
                                            long WinCent = Convert.ToInt64(Price * Odds * zj);
                                            string WinNotes = "中奖注数:" + zj;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                    }
                                    break;
                                case 21: //四胆拖类型
                                    {
                                        if (zj_ds + zj_ts >= 4)
                                        {
                                            zj = C(zj_ts, 4 - iNum_zj_d.Length);
                                            long WinCent = Convert.ToInt64(Price * Odds * zj);
                                            string WinNotes = "中奖注数:" + zj;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                    }
                                    break;
                                case 20: //三胆拖类型
                                    {
                                        if (zj_ds + zj_ts >= 3)
                                        {
                                            zj = C(zj_ts, 3 - iNum_zj_d.Length);
                                            long WinCent = Convert.ToInt64(Price * Odds * zj);
                                            string WinNotes = "中奖注数:" + zj;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                    }
                                    break;
                                default: //二胆拖类型
                                    {
                                        if (zj_ds + zj_ts >= 2)
                                        {
                                            zj = C(zj_ts, 2 - iNum_zj_d.Length);
                                            long WinCent = Convert.ToInt64(Price * Odds * zj);
                                            string WinNotes = "中奖注数:" + zj;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                    }
                                    break;
                            }
                        }
                        else  //胆码没有全中则没有中奖
                        {
                            zj = 0;
                        }
                    }
                }
                else if (Types == 25)//总和大小
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    for (int j = 0; j < iNum_kj.Length; j++)
                    {
                        sum += Convert.ToInt32(iNum_kj[j]);
                    }
                    string temp = Convert.ToString(sum);
                    sum = Convert.ToInt32(temp);
                    if (sum >= 23 && zj == "大")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum <= 22 && zj == "小")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 26)//总和单双
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    for (int j = 0; j < iNum_kj.Length; j++)
                    {
                        sum += Convert.ToInt32(iNum_kj[j]);
                    }
                    string temp = Convert.ToString(sum);
                    temp = temp.Substring(temp.Length - 1, 1);
                    sum = Convert.ToInt32(temp);
                    if (sum % 2 != 0 && zj == "单")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum % 2 == 0 && zj == "双")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 2 || Types == 5 || Types == 8 || Types == 11 || Types == 14)//万千百十个大小
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;
                    int j = 0;
                    if (Types == 2) { sum = Convert.ToInt32(iNum_kj[0]); }
                    if (Types == 5) { sum = Convert.ToInt32(iNum_kj[1]); }
                    if (Types == 8) { sum = Convert.ToInt32(iNum_kj[2]); }
                    if (Types == 11) { sum = Convert.ToInt32(iNum_kj[3]); }
                    if (Types == 14) { sum = Convert.ToInt32(iNum_kj[4]); }

                    if (sum >= 5 && zj == "大")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum <= 4 && zj == "小")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 3 || Types == 6 || Types == 9 || Types == 12 || Types == 15)//万千百十个单双
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;
                    int j = 0;
                    if (Types == 3) { j = 0; }
                    if (Types == 6) { j = 1; }
                    if (Types == 9) { j = 2; }
                    if (Types == 12) { j = 3; }
                    if (Types == 15) { j = 4; }
                    sum = Convert.ToInt32(iNum_kj[j]);
                    if (sum % 2 != 0 && zj == "单")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum % 2 == 0 && zj == "双")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 17)//龙虎和
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int zj_zs = 0;

                    if (Convert.ToInt32(iNum_kj[0]) > Convert.ToInt32(iNum_kj[4]) && zj == "龙")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Convert.ToInt32(iNum_kj[0]) < Convert.ToInt32(iNum_kj[4]) && zj == "虎")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Convert.ToInt32(iNum_kj[0]) == Convert.ToInt32(iNum_kj[4]) && zj == "和")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 23)//有牛无牛
                {
                    string result = string.Empty;
                    if (Niu(Result) == "")
                    {
                        result = "无牛";
                    }
                    else
                    {
                        result = "有牛";
                    }
                    int zj_zs = 0;
                    if (result == Notes)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 24)//特定牛牛
                {
                    string a = Niu(Result);
                    int c = 0;
                    if (a != "")
                    {
                        c = Convert.ToInt32(a.Substring(a.Length - 1, 1));

                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                            if (string.Compare(iNum_zj[fs], c.ToString()) == 0)
                                zj_zs += 1;

                        if (zj_zs > 0)
                        {
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖号：牛" + c + "|中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                }
                else if (Types == 27)//总和五门
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    for (int j = 0; j < iNum_kj.Length; j++)
                    {
                        sum += Convert.ToInt32(iNum_kj[j]);
                    }
                    string temp = Convert.ToString(sum);
                    sum = Convert.ToInt32(temp);
                    if (sum <= 9 && zj == "一门")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum > 9 && sum <= 19 && zj == "二门")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum > 19 && sum <= 29 && zj == "三门")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum > 29 && sum <= 39 && zj == "四门")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    else if (sum > 39 && sum <= 45 && zj == "五门")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 28)//总和大小单双
                {
                    string[] iNum_kj = Result.Split(' ');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    for (int j = 0; j < iNum_kj.Length; j++)
                    {
                        sum += Convert.ToInt32(iNum_kj[j]);
                    }
                    string temp = Convert.ToString(sum);
                    sum = Convert.ToInt32(temp);
                    if (sum < 23)//小
                    {
                        if (sum % 2 == 0 && zj == "小双")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (sum % 2 != 0 && zj == "小单")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else//大
                    {
                        if (sum % 2 == 0 && zj == "大双")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (sum % 2 != 0 && zj == "大单")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                }
                else if (Types == 29)//梭哈炸弹
                {
                    int zj_zs = 0;
                    if (Zhadan(Result) >= 4)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 30)//梭哈葫芦
                {
                    int zj_zs = 0;
                    if (HuLu(Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 31)//梭哈顺子
                {
                    int zj_zs = 0;
                    if (SHShunzi(Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 32)//梭哈三条
                {
                    int zj_zs = 0;
                    if (SHSantiao(Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 33)//梭哈两对
                {
                    int zj_zs = 0;
                    if (SHLiangdui(Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 34)//梭哈单对
                {
                    int zj_zs = 0;
                    if (SHDandui(Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 35)//梭哈散牌
                {
                    int zj_zs = 0;
                    if (SHSanpai(Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 36)//前三大小
                {
                    int zj_zs = 0;
                    if (Qiansan(1, Result) == 1 && Notes == "大")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Qiansan(1, Result) == 2 && Notes == "小")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 37)//前三单双
                {
                    int zj_zs = 0;
                    if (Qiansan(2, Result) == 1 && Notes == "单")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Qiansan(2, Result) == 2 && Notes == "双")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 38)//前三豹子
                {
                    int zj_zs = 0;
                    if (Qiansan(3, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 39)//前三顺子
                {
                    int zj_zs = 0;
                    if (Qiansan(4, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 40)//前三对子
                {
                    int zj_zs = 0;
                    if (Qiansan(5, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 41)//前三半顺
                {
                    int zj_zs = 0;
                    if (Qiansan(6, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 42)//前三杂六
                {
                    int zj_zs = 0;
                    if (Qiansan(7, Result) != 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 43)//中三大小
                {
                    int zj_zs = 0;
                    if (Zhongsan(1, Result) == 1 && Notes == "大")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Zhongsan(1, Result) == 2 && Notes == "小")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 44)//中三单双
                {
                    int zj_zs = 0;
                    if (Zhongsan(2, Result) == 1 && Notes == "单")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Zhongsan(2, Result) == 2 && Notes == "双")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 45)//中三豹子
                {
                    int zj_zs = 0;
                    if (Zhongsan(3, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 46)//中三顺子
                {
                    int zj_zs = 0;
                    if (Zhongsan(4, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 47)//中三对子
                {
                    int zj_zs = 0;
                    if (Zhongsan(5, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 48)//中三半顺
                {
                    int zj_zs = 0;
                    if (Zhongsan(6, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 49)//中三杂六
                {
                    int zj_zs = 0;
                    if (Zhongsan(7, Result) != 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }

                else if (Types == 50)//后三大小
                {
                    int zj_zs = 0;
                    if (Housan(1, Result) == 1 && Notes == "大")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Housan(1, Result) == 2 && Notes == "小")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 51)//后三单双
                {
                    int zj_zs = 0;
                    if (Housan(2, Result) == 1 && Notes == "单")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                    if (Housan(2, Result) == 2 && Notes == "双")
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 52)//后三豹子
                {
                    int zj_zs = 0;
                    if (Housan(3, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 53)//后三顺子
                {
                    int zj_zs = 0;
                    if (Housan(4, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 54)//后三对子
                {
                    int zj_zs = 0;
                    if (Housan(5, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 55)//后三半顺
                {
                    int zj_zs = 0;
                    if (Housan(6, Result) == 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                else if (Types == 56)//后三杂六
                {
                    int zj_zs = 0;
                    if (Housan(7, Result) != 1)
                    {
                        zj_zs = 1;
                        long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                        string WinNotes = "中奖注数:" + zj_zs;
                        new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                    }
                }
                #endregion

                //更新已开奖
                new BCW.ssc.BLL.SSCpay().UpdateState(ID, 1);
            }
        }
    }

    //大小单双，龙虎赔率浮动
    private void OddsPage()
    {
        #region 大小单双赔率浮动
        try
        {
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置


            string strWheres = string.Empty;
            strWheres += "State = 1 order by ID desc";
            DataSet ds = new BCW.ssc.BLL.SSClist().GetList("Result", strWheres);

            string[] result1 = ds.Tables[0].Rows[0]["Result"].ToString().Split(' ');
            int re1 = Convert.ToInt32(result1[0]);
            int re2 = Convert.ToInt32(result1[1]);
            int re3 = Convert.ToInt32(result1[2]);
            int re4 = Convert.ToInt32(result1[3]);
            int re5 = Convert.ToInt32(result1[4]);
            int temp1 = 0;
            int sum1 = 0;
            string temps1 = string.Empty;
            for (int j = 0; j < result1.Length; j++)
            {
                temp1 = Convert.ToInt32(result1[j]);
                sum1 += temp1;
            }
            temps1 = Convert.ToString(sum1);
            temp1 = Convert.ToInt32(temps1);
            int qiansan = re1 + re2 + re3;
            int zhongsan = re2 + re3 + re4;
            int housan = re3 + re4 + re5;


            #region 万位大小、单双
            #region 万位大
            if (re1 > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[0]) > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype2", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype2"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype2", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype2"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype2"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[0]) > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype2", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype2"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype2", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype2"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype2"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 万位单
            if (re1 % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    if (Convert.ToInt32(Result[0]) % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype3", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype3"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype3", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype3"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype3"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[0]) % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype3", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype3"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype3", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype3"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype3"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 千位大小、单双
            #region 千位大
            if (re2 > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[1]) > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype5", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype5"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype5", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype5"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype5"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[1]) > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype5", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype5"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype5", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype5"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype5"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 千位单
            if (re2 % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    if (Convert.ToInt32(Result[1]) % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype6", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype6"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype6", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype6"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype6"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[1]) % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype6", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype6"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype6", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype6"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype6"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 百位大小、单双
            #region 百位大
            if (re3 > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[2]) > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype8", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype8"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype8", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype8"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype8"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[2]) > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype8", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype8"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype8", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype8"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype8"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 百位单
            if (re3 % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    if (Convert.ToInt32(Result[2]) % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype9", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype9"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype9", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype9"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype9"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[2]) % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype9", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype9"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype9", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype9"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype9"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 十位大小、单双
            #region 十位大
            if (re4 > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[3]) > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype11", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype11"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype11", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype11"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype11"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[3]) > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype11", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype11"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype11", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype11"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype11"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 十位单
            if (re4 % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    if (Convert.ToInt32(Result[3]) % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype12", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype12"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype12", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype12"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype12"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[3]) % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype12", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype12"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype12", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype12"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype12"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 个位大小、单双
            #region 个位大
            if (re5 > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[4]) > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype14", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype14"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype14", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype14"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype14"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[4]) > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype14", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype14"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype14", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype14"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype14"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 个位单
            if (re5 % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    if (Convert.ToInt32(Result[4]) % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype15", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype15"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype15", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype15"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype15"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[4]) % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype15", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype15"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype15", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype15"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype15"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 总和 大小、单双
            #region 大
            if (temp1 > 22)
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 22)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype25", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype25"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype25", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype25"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype25"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 22)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype25", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype25"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype25", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype25"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype25"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 单 大单，小双
            if (sum1 % 2 != 0)
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype26", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype26"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    string ptype1 = ub.GetSub("ptype28", xmlPath);
                    string[] ptype22f = ptype1.Split('#');
                    try { pt8 = ptype22f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype28"] = ptype22f[0] + "#" + ptype22f[4] + "|" + ptype22f[4] + "#" + ptype22f[2] + "#" + ptype22f[3] + "#" + ptype22f[4] + "#" + ptype22f[5] + "#" + ptype22f[6] + "#" + ptype22f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype26", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype26"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype26"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                    string ptype1 = ub.GetSub("ptype28", xmlPath);
                    string[] ptype22f = ptype1.Split('#');
                    double Odds5 = Convert.ToDouble(ptype22f[5]);
                    double oddschushi5 = Convert.ToDouble(ptype22f[4]);
                    double oddsmax5 = Convert.ToDouble(ptype22f[7]);
                    double oddsmin5 = Convert.ToDouble(ptype22f[6]);
                    double oddschushi25 = oddschushi5 * 2;
                    string Da5 = string.Empty; string Xiao5 = string.Empty;
                    if (oddschushi5 + Odds5 * (count2 - lkstart) > oddsmax5 || oddschushi5 + Odds5 * (count2 - lkstart) < oddsmin5)
                    {
                        if (oddschushi5 + Odds5 * (count2 - lkstart) > oddsmax5) { Da5 = oddsmax5.ToString(); Xiao5 = (oddschushi25 - oddsmax5).ToString(); }
                        if (oddschushi5 + Odds5 * (count2 - lkstart) < oddsmin5) { Da5 = oddsmin5.ToString(); Xiao5 = (oddschushi25 - oddsmin5).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype22f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype28"] = ptype22f[0] + "#" + Da5 + "|" + Xiao5 + "#" + ptype22f[2] + "#" + ptype22f[3] + "#" + ptype22f[4] + "#" + ptype22f[5] + "#" + ptype22f[6] + "#" + ptype22f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da5 = (oddschushi5 + Odds5 * (count2 - lkstart)).ToString();
                        Xiao5 = (oddschushi25 - (oddschushi5 + Odds5 * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype22f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype28"] = ptype22f[0] + "#" + Da5 + "|" + Xiao5 + "#" + ptype22f[2] + "#" + ptype22f[3] + "#" + ptype22f[4] + "#" + ptype22f[5] + "#" + ptype22f[6] + "#" + ptype22f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双 大双，小单
            else
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype26", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype26"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    string ptype1 = ub.GetSub("ptype28", xmlPath);
                    string[] ptype22f = ptype1.Split('#');
                    try { pt8 = ptype22f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype28"] = ptype22f[0] + "#" + ptype22f[4] + "|" + ptype22f[4] + "#" + ptype22f[2] + "#" + ptype22f[3] + "#" + ptype22f[4] + "#" + ptype22f[5] + "#" + ptype22f[6] + "#" + ptype22f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype26", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype26"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype26"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                    string ptype1 = ub.GetSub("ptype28", xmlPath);
                    string[] ptype22f = ptype1.Split('#');
                    double Odds5 = Convert.ToDouble(ptype22f[5]);
                    double oddschushi5 = Convert.ToDouble(ptype22f[4]);
                    double oddsmax5 = Convert.ToDouble(ptype22f[7]);
                    double oddsmin5 = Convert.ToDouble(ptype22f[6]);
                    double oddschushi25 = oddschushi5 * 2;
                    string Da5 = string.Empty; string Xiao5 = string.Empty;
                    if (oddschushi5 + Odds5 * (count2 - lkstart) > oddsmax5 || oddschushi5 + Odds5 * (count2 - lkstart) < oddsmin5)
                    {
                        if (oddschushi5 + Odds5 * (count2 - lkstart) > oddsmax5) { Xiao5 = oddsmax5.ToString(); Da5 = (oddschushi25 - oddsmax5).ToString(); }
                        if (oddschushi5 + Odds5 * (count2 - lkstart) < oddsmin5) { Xiao5 = oddsmin5.ToString(); Da5 = (oddschushi25 - oddsmin5).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype22f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype28"] = ptype22f[0] + "#" + Da5 + "|" + Xiao5 + "#" + ptype22f[2] + "#" + ptype22f[3] + "#" + ptype22f[4] + "#" + ptype22f[5] + "#" + ptype22f[6] + "#" + ptype22f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao5 = (oddschushi5 + Odds5 * (count2 - lkstart)).ToString();
                        Da5 = (oddschushi25 - (oddschushi5 + Odds5 * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype22f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype28"] = ptype22f[0] + "#" + Da5 + "|" + Xiao5 + "#" + ptype22f[2] + "#" + ptype22f[3] + "#" + ptype22f[4] + "#" + ptype22f[5] + "#" + ptype22f[6] + "#" + ptype22f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 龙 、虎
            #region 龙
            if (re1 > re5)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                    if (Convert.ToInt32(Result[0]) > Convert.ToInt32(Result[4]))
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype17", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype17"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "|" + OutOdds(17, 3) + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype17", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype17"] = ptype2f[0] + "#" + Da + "|" + Xiao + "|" + OutOdds(17, 3) + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype17"] = ptype2f[0] + "#" + Da + "|" + Xiao + "|" + OutOdds(17, 3) + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 虎
            else
            {
                if (re5 != re1)
                {
                    bool s1 = true;
                    int count1 = 1;
                    while (s1)
                    {
                        string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

                        if (Convert.ToInt32(Result[0]) > Convert.ToInt32(Result[4]))
                            s1 = false;
                        else
                            count1++;
                    }
                    if (count1 <= lkstart)
                    {
                        string ptype = ub.GetSub("ptype17", xmlPath);
                        string[] ptype2f = ptype.Split('#');
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype17"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "|" + OutOdds(17, 3) + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        string ptype = ub.GetSub("ptype17", xmlPath);
                        string[] ptype2f = ptype.Split('#');
                        double Odds = Convert.ToDouble(ptype2f[5]);
                        double oddschushi = Convert.ToDouble(ptype2f[4]);
                        double oddsmax = Convert.ToDouble(ptype2f[7]);
                        double oddsmin = Convert.ToDouble(ptype2f[6]);
                        double oddschushi2 = oddschushi * 2;
                        string Da = string.Empty; string Xiao = string.Empty;
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                        {
                            if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                            if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                            string pt8 = string.Empty;
                            try { pt8 = ptype2f[8]; }
                            catch { pt8 = "0"; }
                            xml.dss["ptype17"] = ptype2f[0] + "#" + Da + "|" + Xiao + "|" + OutOdds(17, 3) + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                        }
                        else
                        {
                            Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                            Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                            string pt8 = string.Empty;
                            try { pt8 = ptype2f[8]; }
                            catch { pt8 = "0"; }
                            xml.dss["ptype17"] = ptype2f[0] + "#" + Da + "|" + Xiao + "|" + OutOdds(17, 3) + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                        }
                    }
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
            }
            #endregion
            #endregion

            #region 前三 大小、单双
            #region 大
            if (qiansan > 13)
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[0]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 13)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype36", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype36"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype36", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype36"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype36"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[0]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 13)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype36", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype36"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype36", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype36"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype36"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 单
            if (qiansan % 2 != 0)
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[0]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype37", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype37"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype37", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype37"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype37"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[0]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype37", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype37"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype37", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype37"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype37"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 中三 大小、单双
            #region 大
            if (zhongsan > 13)
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 13)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype43", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype43"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype43", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype43"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype43"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 13)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype43", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype43"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype43", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype43"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype43"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 单
            if (zhongsan % 2 != 0)
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype44", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype44"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype44", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype44"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype44"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[1]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype44", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype44"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype44", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype44"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype44"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 后三 大小、单双
            #region 大
            if (housan > 13)
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[4]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 13)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype50", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype50"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype50", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype50"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype50"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                int count1 = 1;
                bool s1 = true;
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[4]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temp = Convert.ToInt32(temps);
                    if (temp > 13)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype50", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype50"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;

                }
                else
                {
                    string ptype = ub.GetSub("ptype50", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype50"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype50"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 单
            if (housan % 2 != 0)
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[4]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype51", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype51"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype51", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype51"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype51"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                int count2 = 1;
                bool s2 = true;
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(' ');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    sum = Convert.ToInt32(Result[3]) + Convert.ToInt32(Result[4]) + Convert.ToInt32(Result[2]);
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lkstart)
                {
                    string ptype = ub.GetSub("ptype51", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    string pt8 = string.Empty;
                    try { pt8 = ptype2f[8]; }
                    catch { pt8 = "0"; }
                    xml.dss["ptype51"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                }
                else
                {
                    string ptype = ub.GetSub("ptype51", xmlPath);
                    string[] ptype2f = ptype.Split('#');
                    double Odds = Convert.ToDouble(ptype2f[5]);
                    double oddschushi = Convert.ToDouble(ptype2f[4]);
                    double oddsmax = Convert.ToDouble(ptype2f[7]);
                    double oddsmin = Convert.ToDouble(ptype2f[6]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lkstart) > oddsmax || oddschushi + Odds * (count2 - lkstart) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype51"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lkstart)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lkstart))).ToString();
                        string pt8 = string.Empty;
                        try { pt8 = ptype2f[8]; }
                        catch { pt8 = "0"; }
                        xml.dss["ptype51"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7] + "#" + pt8;
                    }

                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 有牛无牛
            //#region 有牛
            //if (Niu(ds.Tables[0].Rows[0]["Result"].ToString()) != "")
            //{
            //    bool s1 = true;
            //    int count1 = 1;
            //    while (s1)
            //    {
            //        string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

            //        if (Niu(ds.Tables[0].Rows[count1]["Result"].ToString()) != "")
            //            count1++;
            //        else
            //            s1 = false;
            //    }
            //    if (count1 <= lkstart)
            //    {
            //        string ptype = ub.GetSub("ptype23", xmlPath);
            //        string[] ptype2f = ptype.Split('#');
            //        xml.dss["ptype23"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7];
            //    }
            //    else
            //    {
            //        string ptype = ub.GetSub("ptype23", xmlPath);
            //        string[] ptype2f = ptype.Split('#');
            //        double Odds = Convert.ToDouble(ptype2f[5]);
            //        double oddschushi = Convert.ToDouble(ptype2f[4]);
            //        double oddsmax = Convert.ToDouble(ptype2f[7]);
            //        double oddsmin = Convert.ToDouble(ptype2f[6]);
            //        double oddschushi2 = oddschushi * 2;
            //        string Da = string.Empty; string Xiao = string.Empty;
            //        if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
            //        {
            //            if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
            //            if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
            //            xml.dss["ptype23"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7];
            //        }
            //        else
            //        {
            //            Da = (oddschushi + Odds * (count1 - lkstart)).ToString();
            //            Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
            //            xml.dss["ptype23"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7];
            //        }
            //    }
            //    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            //}
            //#endregion
            //#region 无牛
            //else
            //{
            //    bool s1 = true;
            //    int count1 = 1;
            //    while (s1)
            //    {
            //        string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(' ');

            //        if (Niu(ds.Tables[0].Rows[count1]["Result"].ToString()) != "")
            //            s1 = false;
            //        else
            //            count1++;
            //    }
            //    if (count1 <= lkstart)
            //    {

            //        string ptype = ub.GetSub("ptype23", xmlPath);
            //        string[] ptype2f = ptype.Split('#');
            //        xml.dss["ptype23"] = ptype2f[0] + "#" + ptype2f[4] + "|" + ptype2f[4] + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7];
            //    }
            //    else
            //    {
            //        string ptype = ub.GetSub("ptype23", xmlPath);
            //        string[] ptype2f = ptype.Split('#');
            //        double Odds = Convert.ToDouble(ptype2f[5]);
            //        double oddschushi = Convert.ToDouble(ptype2f[4]);
            //        double oddsmax = Convert.ToDouble(ptype2f[7]);
            //        double oddsmin = Convert.ToDouble(ptype2f[6]);
            //        double oddschushi2 = oddschushi * 2;
            //        string Da = string.Empty; string Xiao = string.Empty;
            //        if (oddschushi + Odds * (count1 - lkstart) > oddsmax || oddschushi + Odds * (count1 - lkstart) < oddsmin)
            //        {
            //            if (oddschushi + Odds * (count1 - lkstart) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
            //            if (oddschushi + Odds * (count1 - lkstart) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
            //            xml.dss["ptype23"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7];
            //        }
            //        else
            //        {

            //            Xiao = (oddschushi + Odds * (count1 - lkstart)).ToString();
            //            Da = (oddschushi2 - (oddschushi + Odds * (count1 - lkstart))).ToString();
            //            xml.dss["ptype23"] = ptype2f[0] + "#" + Da + "|" + Xiao + "#" + ptype2f[2] + "#" + ptype2f[3] + "#" + ptype2f[4] + "#" + ptype2f[5] + "#" + ptype2f[6] + "#" + ptype2f[7];
            //        }
            //    }
            //    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            //}
            //#endregion
            #endregion
        }
        catch
        {

        }
        #endregion
    }

    #region 前三
    private int Qiansan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[0]) + Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[0]) + Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[0]) == Convert.ToInt32(iNum_kj[1]) && Convert.ToInt32(iNum_kj[1]) == Convert.ToInt32(iNum_kj[2]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[0]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[1]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[2]); if (n3 == 0) n3 = 10;
            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }

            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[0]);
            int n2 = Convert.ToInt32(iNum_kj[1]);
            int n3 = Convert.ToInt32(iNum_kj[2]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[0]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[1]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[2]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[0] + iNum_kj[1] + iNum_kj[2];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Qiansan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Qiansan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Qiansan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Qiansan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Qiansan(6, Result) == 1) //半顺
                zj_zs = 1;
        }
        return zj_zs;
    }
    #endregion

    #region 中三
    private int Zhongsan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[1]) == Convert.ToInt32(iNum_kj[2]) && Convert.ToInt32(iNum_kj[2]) == Convert.ToInt32(iNum_kj[3]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[1]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[2]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[3]); if (n3 == 0) n3 = 10;
            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }
            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[1]);
            int n2 = Convert.ToInt32(iNum_kj[2]);
            int n3 = Convert.ToInt32(iNum_kj[3]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[1]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[2]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[3]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[1] + iNum_kj[2] + iNum_kj[3];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Zhongsan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Zhongsan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Zhongsan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Zhongsan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Zhongsan(6, Result) == 1) //半顺
                zj_zs = 1;

        }
        return zj_zs;
    }
    #endregion

    #region 后三
    private int Housan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]) + Convert.ToInt32(iNum_kj[4]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]) + Convert.ToInt32(iNum_kj[4]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[2]) == Convert.ToInt32(iNum_kj[3]) && Convert.ToInt32(iNum_kj[3]) == Convert.ToInt32(iNum_kj[4]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[2]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[3]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[4]); if (n3 == 0) n3 = 10;
            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                int a = n1 - n2;
                int b = n1 - n3;
                int c = n2 - n3;
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }

            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[2]);
            int n2 = Convert.ToInt32(iNum_kj[3]);
            int n3 = Convert.ToInt32(iNum_kj[4]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[2]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[3]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[4]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[2] + iNum_kj[3] + iNum_kj[4];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Housan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Housan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Housan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Housan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Housan(6, Result) == 1) //半顺
                zj_zs = 1;
        }
        return zj_zs;
    }
    #endregion

    #region 梭哈散牌
    private int SHSanpai(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;
        int[] a = { Convert.ToInt32(iNum_kj[0]), Convert.ToInt32(iNum_kj[1]), Convert.ToInt32(iNum_kj[2]), Convert.ToInt32(iNum_kj[3]), Convert.ToInt32(iNum_kj[4]) };
        int equal = 0;
        for (int i = 0; i < iNum_kj.Length - 1; i++)
        {
            for (int j = i + 1; j < iNum_kj.Length; j++)
            {
                if (a[i] == a[j])
                {
                    equal = 1;
                    break;
                }
            }
        }
        if (equal == 0)//数全不相等;
        {
            zj_zs = 1;
        }

        if (SHShunzi(Result) == 1) zj_zs = 0;//不能为顺子

        return zj_zs;
    }
    #endregion

    #region 梭哈单对
    private int SHDandui(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count1 == 2) { if (count0 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count2 == 2) { if (count1 == 1 || count0 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count3 == 2) { if (count1 == 1 || count2 == 1 || count0 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count4 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count0 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count5 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count0 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count6 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count0 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count7 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count0 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count8 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count0 == 1 || count9 == 1) zj_zs = 1; }
        if (count9 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count0 == 1) zj_zs = 1; }

        if (HuLu(Result) == 1) zj_zs = 0;//不能为葫芦
        if (SHLiangdui(Result) == 1) zj_zs = 0;//不能为两对

        return zj_zs;
    }
    #endregion

    #region 梭哈两对
    private int SHLiangdui(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count1 == 2) { if (count0 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count2 == 2) { if (count1 == 2 || count0 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count3 == 2) { if (count1 == 2 || count2 == 2 || count0 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count4 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count0 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count5 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count0 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count6 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count0 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count7 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count0 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count8 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count0 == 2 || count9 == 2) zj_zs = 1; }
        if (count9 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count0 == 2) zj_zs = 1; }

        if (HuLu(Result) == 1) zj_zs = 0;//不能为葫芦

        return zj_zs;
    }
    #endregion

    #region 梭哈三条
    private int SHSantiao(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count1 == 3) { if (count0 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count2 == 3) { if (count1 == 1 || count0 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count3 == 3) { if (count1 == 1 || count2 == 1 || count0 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count4 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count0 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count5 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count0 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count6 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count0 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count7 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count0 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count8 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count0 == 1 || count9 == 1) zj_zs = 1; }
        if (count9 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count0 == 1) zj_zs = 1; }

        return zj_zs;
    }
    #endregion

    #region 梭哈顺子
    private int SHShunzi(string Result)
    {
        int a = 0;
        if (Result == "0 1 2 3 4" || Result == "1 2 3 4 5" || Result == "2 3 4 5 6" || Result == "3 4 5 6 7" || Result == "4 5 6 7 8" || Result == "5 6 7 8 9" || Result == "0 6 7 8 9" || Result == "0 1 7 8 9" || Result == "0 1 2 8 9" || Result == "0 1 2 3 9")
        {
            a = 1;
        }

        return a;
    }
    #endregion

    #region 炸弹算法
    private int Zhadan(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;
        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int f = 0;
        for (int j = 0; j < iNum_kj.Length; j++)
        {
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[0]))
                a += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[1]))
                b += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[2]))
                c += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[3]))
                d += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[4]))
                f += 1;
        }
        if (a >= 4)
            zj_zs = a;
        if (b >= 4)
            zj_zs = b;
        if (c >= 4)
            zj_zs = c;
        if (d >= 4)
            zj_zs = d;
        if (f >= 4)
            zj_zs = f;
        return zj_zs;
    }
    #endregion

    #region 葫芦算法
    private int HuLu(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count1 == 3) { if (count0 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count2 == 3) { if (count1 == 2 || count0 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count3 == 3) { if (count1 == 2 || count2 == 2 || count0 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count4 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count0 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count5 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count0 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count6 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count0 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count7 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count0 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count8 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count0 == 2 || count9 == 2) zj_zs = 1; }
        if (count9 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count0 == 2) zj_zs = 1; }

        return zj_zs;
    }
    #endregion

    #region 牛牛算法
    ///<summary>
    ///牛牛算法
    ///返回result ，为空则是无牛
    /// </summary>
    private string Niu(string Result)
    {
        string result = string.Empty;
        string a = string.Empty;
        string b = string.Empty;
        string[] num = Result.Split(' ');

        if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[2])) % 10 == 0)//012
        {
            a = "牛";
            b = ((Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[3])) % 10 == 0)//013
        {
            a = "牛";
            b = ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[4])) % 10 == 0)//014
        {
            a = "牛";
            b = ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10 == 0)//023
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10 == 0)//024
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//034
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10 == 0)//123
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10 == 0)//124
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//134
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//234
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1])) % 10).ToString();
            result = a + b;
        }

        return result;
    }
    #endregion

    #region 两个数是否相似 IsLike
    /// <summary>
    /// 两个数是否相似
    /// </summary>
    private bool IsLike(string Num1, string Num2)
    {
        bool like = true;
        try
        {
            string getNum1 = Utils.ConvertSeparated(Num1, 1, ",");
            string getNum2 = Utils.ConvertSeparated(Num2, 1, ",");

            string[] str1 = getNum1.Split(',');
            string[] str2 = getNum2.Split(',');

            for (int i = 0; i < str1.Length; i++)
            {
                int cNum = Utils.GetStringNum(Num1, str1[i]);
                int cNum2 = Utils.GetStringNum(Num2, str1[i]);

                if (cNum != cNum2)
                {
                    like = false;
                    break;
                }

            }
        }
        catch { }
        return like;
    }
    #endregion

    #region 将两个数字转化成三个数字的两组集合 OutStrNum

    /// <summary>
    /// 将两个数字转化成三个数字的两组集合
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    private string OutStrNum(string sNum)
    {
        string[] Temp = sNum.Split(',');
        string strNum1 = Temp[0] + "," + sNum;
        string strNum2 = sNum + "," + Temp[1];
        string strNum = strNum1 + "，" + strNum2;
        return strNum.Replace(",", "");
    }
    #endregion

    #region 更新期数 SSCLISTPAGE
    /// <summary>
    /// 更新期数 20160928
    /// </summary>
    public string SSCLISTPAGE()
    {
        string tmpid = "";
        DateTime EndTime = DateTime.Now;
        try
        {
            string tmpStartTime = string.Empty;
            if (DateTime.Now > Convert.ToDateTime("10:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))//10：00:00到22:00:00 025--096期 10分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 10:00:00";
            }
            if (DateTime.Now > Convert.ToDateTime("00:00:00") && DateTime.Now < Convert.ToDateTime("01:55:00"))// 00:00:00-01:55:00 001-023 期  5分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 00:00:00";
            }
            if (DateTime.Now > Convert.ToDateTime("22:00:00") && DateTime.Now <= Convert.ToDateTime("23:59:59"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 22:00:00";
            }
            if (DateTime.Now > Convert.ToDateTime("01:55:00") && DateTime.Now <= Convert.ToDateTime("10:00:00"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 10:00:00";
            }
            DateTime StartTime = Convert.ToDateTime(tmpStartTime);
            int d = 1;
            if (DateTime.Now > Convert.ToDateTime("10:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))//10：00:00到22:00:00 025--096期 10分钟每期
            {
                if (DateTime.Compare(EndTime, StartTime) > 0)
                {
                    DateTime dt1 = Convert.ToDateTime("10:00:00");
                    string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    d = Convert.ToInt32(dt4 / 10);
                }
            }
            if (DateTime.Now > Convert.ToDateTime("00:00:00") && DateTime.Now < Convert.ToDateTime("01:55:00"))// 00:00:00-01:55:00 001-023 期  5分钟每期
            {
                if (DateTime.Compare(EndTime, StartTime) > 0)
                {
                    DateTime dt1 = Convert.ToDateTime("00:00:00");
                    string dt3 = DateTime.Now.AddMinutes(2.5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    d = Convert.ToInt32(dt4 / 5);
                }
            }
            if (DateTime.Now > Convert.ToDateTime("22:00:00") && DateTime.Now <= Convert.ToDateTime("23:59:59"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                if (DateTime.Compare(EndTime, StartTime) > 0)
                {
                    DateTime dt1 = Convert.ToDateTime("22:00:00");
                    string dt3 = DateTime.Now.AddMinutes(2.5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    d = Convert.ToInt32(dt4 / 5);
                }
            }
            if (DateTime.Now > Convert.ToDateTime("01:55:00") && DateTime.Now <= Convert.ToDateTime("10:00:00"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
            }


            if (DateTime.Now > Convert.ToDateTime("10:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))//10：00:00到22:00:00 025--096期 10分钟每期
            {
                tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + (d + 24);
                EndTime = Convert.ToDateTime(tmpStartTime).AddMinutes(d * 10);
            }
            if (DateTime.Now > Convert.ToDateTime("01:55:00") && DateTime.Now < Convert.ToDateTime("10:00:00"))//1:55:00-10:00:00 为 024 期
            {
                tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + 24;
                EndTime = Convert.ToDateTime(tmpStartTime);
            }
            if (DateTime.Now > Convert.ToDateTime("22:00:00") && DateTime.Now <= Convert.ToDateTime("23:59:59"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                if (d < 4)
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + (d + 96);
                }
                else
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + (d + 96);
                }
                EndTime = Convert.ToDateTime(tmpStartTime).AddMinutes(d * 5);
            }
            if (DateTime.Now > Convert.ToDateTime("00:00:00") && DateTime.Now < Convert.ToDateTime("01:55:00"))// 00:00:00-01:55:00 001-023 期  5分钟每期
            {
                if (d < 10)
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "00" + d;
                }
                else
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + d;
                }
                EndTime = Convert.ToDateTime(tmpStartTime).AddMinutes(d * 5);
            }

            if (!new BCW.ssc.BLL.SSClist().ExistsSSCId(int.Parse(tmpid)))
            {
                if (d <= 120)
                {
                    BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
                    model.SSCId = int.Parse(tmpid);
                    model.Result = "";
                    model.Notes = "";
                    model.EndTime = EndTime;
                    model.State = 0;
                    model.StateTime = " ";
                    new BCW.ssc.BLL.SSClist().Add(model);
                }
            }

        }
        catch
        {
        }
        ////   builder.Append("更新最新期数" + tmpid + "--截止时间" + EndTime);
        return "更新最新期数" + tmpid;
    }
    #endregion

    #region 抓取
    /// <summary>
    /// 更新开奖结果
    /// </summary>
    public void sscopengame()
    {
        int SSCId = 0;
        string Result = "";
        List<Matchs> list = TranList();
        foreach (Matchs a in list)
        {
            SSCId = Convert.ToInt32(a.expect);
            Result = a.opencode.Replace(",", " ");

            try
            {
                if (true)
                {
                    bool s = new BCW.ssc.BLL.SSClist().ExistsSSCId(SSCId, 1);
                    switch (s)
                    {
                        case true:
                            new BCW.ssc.BLL.SSClist().UpdateResult(SSCId, Result);
                            new BCW.ssc.BLL.SSCpay().UpdateResult(SSCId, Result);
                            string StateTime = "0|" + DateTime.Now + "|" + Result;
                            new BCW.ssc.BLL.SSClist().UpdateStateTime(SSCId, StateTime);//0自动开奖|time0时间#1|time1手动开奖#2|time2重开奖.....
                            break;
                        case false:
                            break;
                    }
                }
            }
            catch
            { }
        }
    }
    /// <summary>
    /// 将xml解析成list集合
    /// </summary>
    /// <returns>返回list集合</returns>
    public List<Matchs> TranList()
    {
        List<Matchs> list = new List<Matchs>();
        XmlDocument dom = GetXmlData();
        XmlNodeList nodelist = dom.SelectSingleNode("/xml").ChildNodes;

        foreach (XmlNode node in nodelist)//遍历xml节点
        {
            Matchs matchs = new Matchs();
            XmlElement xe = (XmlElement)node;//将子节点类型转换为xmlelement类型

            matchs.expect = xe.GetAttribute("expect").Substring(2, 9);
            matchs.opencode = xe.GetAttribute("opencode");

            list.Add(matchs);
        }
        return list;
    }

    /// <summary>
    /// 获得xml接口数据
    /// </summary>
    /// <returns>返回xmldocument格式数据</returns>
    public XmlDocument GetXmlData()
    {
        string url = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=cqssc&rows=20";//xml接口 免费接口： http://f.apiplus.cn/cqssc-50.xml
        XmlDocument xml = new XmlDocument();
        try
        {
            xml.Load(url);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return xml;
    }
    /// <summary>
    /// 每期比赛
    /// </summary>
    public class Matchs
    {
        public string expect;
        public string opencode;
    }
    #endregion

    #region 获取后几位数
    /// 获取后几位数
    /// <param name="str">要截取的字符串</param>
    /// <param name="num">返回的具体位数</param>
    /// <returns>返回结果的字符串</returns>
    public string GetLastStr(string str, int num)
    {
        int count = 0;
        if (str.Length > num)
        {
            count = str.Length - num;
            str = str.Substring(count, num);
        }
        return str;
    }
    #endregion

    #region 下注类型 OutType
    /// <summary>
    /// 下注类型
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutType(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = payname2[i];
        }

        return pText;
    }
    #endregion

    #region 玩法提示 OutRule
    /// <summary>
    /// 玩法提示
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutRule(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = rule2[i];
        }

        return pText;
    }
    #endregion

    #region 赔率 OutOdds
    /// <summary>
    /// 赔率 如果赔率只有一个，n就是1位，赔率取第二位，n就是2，赔率n位，取N位
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOdds(int Types, int n)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            string[] odds = odds2[i].Split('|');

            if (odds.Length == 1)
            {
                if (Types == i)
                    pText = odds2[i];
            }
            else
            {
                for (int m = 0; m < odds.Length; m++)
                {
                    if (Types == i && m == (n - 1))
                    {
                        pText = odds[m];
                    }
                }
            }
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddsc
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddsc(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 计算组合的数量
    static long jc(int N)//阶乘
    {
        long t = 1;

        for (int i = 1; i <= N; i++)
        {
            t *= i;
        }
        return t;
    }
    static long P(int N, int R)//组合的计算公式
    {
        long t = jc(N) / jc(N - R);

        return t;
    }
    static int C(int N, int R)//组合
    {
        long i = P(N, R) / jc(R);
        int t = Convert.ToInt32(i);
        return t;
    }
    #endregion

    /// <summary>
    /// 统计有多少个不同数字
    /// </summary>
    /// <param name="Result"></param>
    /// <returns></returns>
    private int BackLikeNum(string Result)
    {
        int num = 0;

        if (Result.Contains("0"))
            num = num + 1;
        if (Result.Contains("1"))
            num = num + 1;
        if (Result.Contains("2"))
            num = num + 1;
        if (Result.Contains("3"))
            num = num + 1;
        if (Result.Contains("4"))
            num = num + 1;
        if (Result.Contains("5"))
            num = num + 1;
        if (Result.Contains("6"))
            num = num + 1;
        if (Result.Contains("7"))
            num = num + 1;
        if (Result.Contains("8"))
            num = num + 1;
        if (Result.Contains("9"))
            num = num + 1;

        return num;
    }
}