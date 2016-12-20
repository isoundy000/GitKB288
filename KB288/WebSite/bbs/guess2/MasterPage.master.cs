using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BCW.Common;

/// <summary>
/// 修改走地下注确认
/// 
/// 黄国军 20160323
/// </summary>
public partial class bbs_game2_MasterPage : BCW.Common.BaseMaster
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected System.Text.StringBuilder builder2 = new System.Text.StringBuilder("");
    protected System.Text.StringBuilder builder3 = new System.Text.StringBuilder("");
    protected string _Title;
    protected bool _IsFoot = true;
    protected int _Refresh = 0;
    protected string _Gourl;
    public override string Title
    {
        set { _Title = value; }
    }
    public override bool IsFoot
    {
        set { _IsFoot = value; }
    }
    public override int Refresh
    {
        set { _Refresh = value; }
    }
    public override string Gourl
    {
        set { _Gourl = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {


        int meid = BCW.User.Users.userId();
        if (meid > 0)
        {
            meid = new BCW.User.Users().GetUsId();
            BCW.User.Users.ShowVerifyRole("c", meid);//非验证会员提示
        }
        try
        {
            BCW.User.Master.ShowMaster(meid, _Title);
        }
        catch { }

        #region 定期查询8波数据是否中断 注释
        ////定期查询8波数据是否中断
        //string xmlPath = "/Controls/guess2.xml";
        //Application.Remove(xmlPath);//清缓存
        //int checkgidsec = Utils.ParseInt(ub.GetSub("Sitecheckgidsec", xmlPath));
        //if (checkgidsec > 0)
        //{
        //    DateTime GuessOddsTime = new BCW.BLL.SysTemp().GetGuessOddsTime(1);//得到水位更新的最新时间

        //    string gidIDS = "";
        //    if (GuessOddsTime.AddSeconds(checkgidsec) < DateTime.Now)
        //    {
        //        DataSet ds = new TPR2.BLL.guess.BaList().GetBaListList("id", "p_active=0 and p_del=0 and p_ison=1");
        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                int gid = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
        //                //隐藏赛事
        //                TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
        //                model.ID = gid;
        //                model.p_del = 1;
        //                new TPR2.BLL.guess.BaList().Updatep_del(model);
        //                gidIDS += "#" + gid;
        //            }
        //            gidIDS = Utils.Mid(gidIDS, 1, gidIDS.Length);
        //        }
        //        if (gidIDS != "")
        //        {
        //            ub xml = new ub();
        //            xml.ReloadSub(xmlPath); //加载配置
        //            xml.dss["SitecheckgidIDS"] = gidIDS;
        //            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //        }
        //    }
        //    else
        //    {
        //        //被隐藏的走地赛事显示出来

        //        gidIDS = ub.GetSub("SitecheckgidIDS", xmlPath);
        //        if (gidIDS != "")
        //        {
        //            string[] gidTemp = gidIDS.Split("#".ToCharArray());
        //            for (int i = 0; i < gidTemp.Length; i++)
        //            {
        //                int gid = Utils.ParseInt(gidTemp[i]);
        //                //显示赛事
        //                TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
        //                model.ID = gid;
        //                model.p_del = 0;
        //                new TPR2.BLL.guess.BaList().Updatep_del(model);
        //            }
        //            ub xml = new ub();
        //            xml.ReloadSub(xmlPath); //加载配置
        //            xml.dss["SitecheckgidIDS"] = "";
        //            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //        }
        //    }
        //}


        ////定期查询8波数据是否中断
        //string xmlPath = "/Controls/guess2.xml";
        //Application.Remove(xmlPath);//清缓存
        //string checkgid = ub.GetSub("Sitecheckgid", xmlPath);
        //int checkgidsec = Utils.ParseInt(ub.GetSub("Sitecheckgidsec", xmlPath));
        //int checkgidzd = 0;
        //int getoldgid = 0;
        // TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
        // try
        // {


        //     if (checkgidsec > 0)
        //     {
        //         DataSet ds = null;
        //         int getgid = 0;
        //         DateTime gettime = DateTime.Parse("1990-1-1");



        //         //==============================判断旧一场比赛是否还可以继续参照开始====================================
        //         getoldgid = Utils.ParseInt(ub.GetSub("SitecheckgidID", xmlPath));
        //         if (getoldgid > 0)
        //         {
        //             //即时更新比赛水位
        //             int p_type = Utils.ParseInt(ub.GetSub("SitecheckgidType", xmlPath));
        //             int p_id = Utils.ParseInt(ub.GetSub("SitecheckgidPID", xmlPath));

        //             if (p_type == 1)
        //             {
        //                 new TPR2.Collec.Footbo().GetBoView(p_id, true);
        //             }
        //             else
        //             {
        //                 new TPR2.Collec.Basketbo().GetBoView(p_id, true);
        //             }
        //             model = new TPR2.BLL.guess.BaList().GetModel(getoldgid);
        //             if (model != null)
        //             {
        //                 if (model.p_ison == 1 && model.p_active == 0)
        //                 {
        //                     if (model.p_type == 1)
        //                     {
        //                         if (model.p_once.Contains("+") || model.p_once.Contains("加") || model.p_once.Contains("点") || model.p_once.Contains("中") || model.p_once.Contains("推迟") || model.p_once.Contains("腰斩") || model.p_once.Contains("取消"))
        //                             getoldgid = 0;

        //                         if (Utils.ParseInt(model.p_once) >= 85)
        //                             getoldgid = 0;
        //                     }
        //                     else
        //                     {
        //                         if (Convert.ToDateTime(model.p_TPRtime) < DateTime.Now.AddMinutes(-45))
        //                             getoldgid = 0;
        //                     }
        //                 }
        //                 else
        //                     getoldgid = 0;
        //             }
        //             else
        //             {
        //                 getoldgid = 0;
        //             }
        //         }
        //         //==============================判断旧一场比赛是否还可以继续参照结束====================================

        //         //==============================START如果旧参照比赛还可以继续使用则进行逻辑判断====================================

        //         if (getoldgid > 0)
        //         {

        //             //原数据
        //             string oldodds = ub.GetSub("SitecheckgidODDS", xmlPath);
        //             //新数据
        //             string newodds = model.p_one_lu + "_" + model.p_two_lu + "_" + model.p_big_lu + "_" + model.p_small_lu + "_" + model.p_bzs_lu + "_" + model.p_bzp_lu + "_" + model.p_bzx_lu + "";
        //             //更新时间
        //             DateTime dt = DateTime.Parse(ub.GetSub("SitecheckgidTIME", xmlPath));

        //             string gidIDS = "";
        //             if (oldodds == newodds && dt.AddSeconds(checkgidsec) < DateTime.Now)
        //             {
        //                 ds = new TPR2.BLL.guess.BaList().GetBaListList("id", "p_active=0 and p_del=0 and p_ison=1");
        //                 if (ds != null && ds.Tables[0].Rows.Count > 0)
        //                 {
        //                     for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                     {
        //                         int gid = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
        //                         //隐藏赛事
        //                         model.ID = gid;
        //                         model.p_del = 1;
        //                         new TPR2.BLL.guess.BaList().Updatep_del(model);
        //                         gidIDS += "#" + gid;
        //                     }
        //                     gidIDS = Utils.Mid(gidIDS, 1, gidIDS.Length);
        //                 }
        //                 if (gidIDS != "")
        //                 {
        //                     ub xml = new ub();
        //                     xml.ReloadSub(xmlPath); //加载配置
        //                     xml.dss["SitecheckgidIDS"] = gidIDS;
        //                     xml.dss["SitecheckgidTIME"] = DateTime.Now;
        //                     System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //                 }
        //             }
        //             if (oldodds != newodds)
        //             {

        //                 //被隐藏的走地赛事显示出来
        //                 if (checkgidzd == 0)
        //                 {
        //                     gidIDS = ub.GetSub("SitecheckgidIDS", xmlPath);
        //                     if (gidIDS != "")
        //                     {
        //                         string[] gidTemp = gidIDS.Split("#".ToCharArray());
        //                         for (int i = 0; i < gidTemp.Length; i++)
        //                         {
        //                             int gid = Utils.ParseInt(gidTemp[i]);
        //                             //显示赛事
        //                             model.ID = gid;
        //                             model.p_del = 0;
        //                             new TPR2.BLL.guess.BaList().Updatep_del(model);
        //                         }

        //                     }
        //                 }
        //                 ub xml = new ub();
        //                 xml.ReloadSub(xmlPath); //加载配置
        //                 xml.dss["SitecheckgidODDS"] = newodds;
        //                 xml.dss["SitecheckgidTIME"] = DateTime.Now;
        //                 xml.dss["SitecheckgidIDS"] = "";
        //                 System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //             }


        //         }
        //         //==============================OVER如果旧参照比赛还可以继续使用则进行逻辑判断====================================
        //         //==============================新增一场参照比赛开始====================================
        //         else
        //         {
        //             if (checkgid != "")
        //             {
        //                 string[] gidTemp = checkgid.Split("#".ToCharArray());
        //                 for (int i = 0; i < gidTemp.Length; i++)
        //                 {
        //                     int gid = Utils.ParseInt(gidTemp[i]);
        //                     ds = new TPR2.BLL.guess.BaList().GetBaListList("p_type,p_TPRTime,p_once", "id=" + gid + " and p_active=0 and p_ison=1");
        //                     if (ds != null && ds.Tables[0].Rows.Count > 0)
        //                     {
        //                         int p_type = int.Parse(ds.Tables[0].Rows[0]["p_type"].ToString());
        //                         DateTime p_TPRTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["p_TPRTime"].ToString());
        //                         string p_once = ds.Tables[0].Rows[0]["p_once"].ToString();
        //                         if (p_type == 1)
        //                         {
        //                             if (p_once.Contains("+") || p_once.Contains("加") || p_once.Contains("点") || p_once.Contains("中") || p_once.Contains("推迟") || p_once.Contains("腰斩") || p_once.Contains("取消"))
        //                                 continue;

        //                             if (Utils.ParseInt(p_once) >= 85)
        //                                 continue;
        //                         }
        //                         else
        //                         {
        //                             if (p_TPRTime < DateTime.Now.AddMinutes(-45))
        //                                 continue;
        //                         }
        //                         if (p_TPRTime > gettime)
        //                         {
        //                             gettime = p_TPRTime;
        //                             getgid = gid;
        //                         }

        //                     }
        //                 }

        //             }
        //             //如果设定的赛事取不到getgid,gettime，则随机取一场
        //             if (getgid == 0)
        //             {
        //                 ds = new TPR2.BLL.guess.BaList().GetBaListList("id", "p_type=1 and p_active=0 and p_del=0 and p_ison=1 and p_TPRTime>'" + DateTime.Now.AddMinutes(-100) + "' order by NEWID()");
        //                 if (ds != null && ds.Tables[0].Rows.Count > 0)
        //                 {
        //                     getgid = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        //                 }
        //                 else
        //                 {
        //                     ds = new TPR2.BLL.guess.BaList().GetBaListList("id", "p_type=2 and p_active=0 and p_del=0 and p_ison=1 and p_TPRTime>'" + DateTime.Now.AddMinutes(-45) + "' order by NEWID()");
        //                     if (ds != null && ds.Tables[0].Rows.Count > 0)
        //                     {
        //                         getgid = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        //                     }
        //                 }
        //             }

        //             //得到ID取实体记录到XML
        //             if (getgid > 0)
        //             {
        //                 model = new TPR2.BLL.guess.BaList().GetModel(getgid);
        //                 if (model != null)
        //                 {
        //                     string NewOdds = model.p_one_lu + "_" + model.p_two_lu + "_" + model.p_big_lu + "_" + model.p_small_lu + "_" + model.p_bzs_lu + "_" + model.p_bzp_lu + "_" + model.p_bzx_lu + "";

        //                     ub xml = new ub();
        //                     //Application.Remove(xmlPath);//清缓存
        //                     xml.ReloadSub(xmlPath); //加载配置
        //                     xml.dss["SitecheckgidID"] = getgid;
        //                     xml.dss["SitecheckgidODDS"] = NewOdds;
        //                     xml.dss["SitecheckgidTIME"] = DateTime.Now;
        //                     xml.dss["SitecheckgidType"] = model.p_type;
        //                     xml.dss["SitecheckgidPID"] = model.p_id;
        //                     System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //                 }

        //             }
        //         }
        //         //==============================新增一场参照比赛结束====================================
        //     }
        // }
        // catch { }


        // if (getoldgid == 0)
        // {
        //     //被隐藏的走地赛事显示出来
        //     if (checkgidzd == 0)
        //     {
        //         string gidIDS = ub.GetSub("SitecheckgidIDS", xmlPath);
        //         if (gidIDS != "")
        //         {
        //             string[] gidTemp = gidIDS.Split("#".ToCharArray());
        //             for (int i = 0; i < gidTemp.Length; i++)
        //             {
        //                 int gid = Utils.ParseInt(gidTemp[i]);
        //                 //显示赛事
        //                 model.ID = gid;
        //                 model.p_del = 0;
        //                 new TPR2.BLL.guess.BaList().Updatep_del(model);
        //             }
        //             ub xml = new ub();
        //             xml.ReloadSub(xmlPath); //加载配置
        //             xml.dss["SitecheckgidTIME"] = DateTime.Now;
        //             xml.dss["SitecheckgidIDS"] = "";
        //             System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //         }
        //     }
        // }
        #endregion

        #region 走地赛事下注确认
        //走地赛事下注确认
        //HttpContext.Current.Application.Lock();
        //DataSet dss = new TPR2.BLL.guess.BaPay().GetBaPayList("pType,PayType,payview,ID,bcid,payusid,payusname,paytimes,payCent,types,p_result_temp1,p_result_temp2,state,payonLuone,payonLutwo,payonLuthr,p_pk,p_dx_pk,p_oncetime2,sure", "p_active=0 and (state=1 OR state=2 OR state=3 OR state=4) order by id desc");
        //if (dss != null && dss.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
        //    {
        //        #region 变量
        //        int pType = int.Parse(dss.Tables[0].Rows[i]["pType"].ToString());
        //        int PayType = int.Parse(dss.Tables[0].Rows[i]["PayType"].ToString());
        //        int zdtime = 0;
        //        if (pType == 1)
        //            zdtime = Convert.ToInt32(ub.GetSub("SiteStemp", "/Controls/guess2.xml"));
        //        else
        //            zdtime = Convert.ToInt32(ub.GetSub("SiteStempb", "/Controls/guess2.xml"));

        //        string payview = dss.Tables[0].Rows[i]["payview"].ToString();
        //        int ID = int.Parse(dss.Tables[0].Rows[i]["ID"].ToString());
        //        int bcid = int.Parse(dss.Tables[0].Rows[i]["bcid"].ToString());
        //        int payusid = int.Parse(dss.Tables[0].Rows[i]["payusid"].ToString());
        //        string payusname = dss.Tables[0].Rows[i]["payusname"].ToString();
        //        DateTime paytimes = DateTime.Parse(dss.Tables[0].Rows[i]["paytimes"].ToString());
        //        int types = int.Parse(dss.Tables[0].Rows[i]["types"].ToString());
        //        long payCent = Convert.ToInt64(dss.Tables[0].Rows[i]["payCent"]);
        //        int p_result_temp1 = int.Parse(dss.Tables[0].Rows[i]["p_result_temp1"].ToString());
        //        int p_result_temp2 = int.Parse(dss.Tables[0].Rows[i]["p_result_temp2"].ToString());
        //        int state = int.Parse(dss.Tables[0].Rows[i]["state"].ToString());
        //        decimal payonLuone = decimal.Parse(dss.Tables[0].Rows[i]["payonLuone"].ToString());
        //        decimal payonLutwo = decimal.Parse(dss.Tables[0].Rows[i]["payonLutwo"].ToString());
        //        decimal payonLuthr = decimal.Parse(dss.Tables[0].Rows[i]["payonLuthr"].ToString());
        //        decimal p_pk = decimal.Parse(dss.Tables[0].Rows[i]["p_pk"].ToString());
        //        decimal p_dx_pk = decimal.Parse(dss.Tables[0].Rows[i]["p_dx_pk"].ToString());
        //        int sure = int.Parse(dss.Tables[0].Rows[i]["sure"].ToString());
        //        string p_oncetime2 = "";
        //        if (dss.Tables[0].Rows[i]["p_oncetime2"].ToString() != "")
        //            p_oncetime2 = dss.Tables[0].Rows[i]["p_oncetime2"].ToString();
        //        #endregion

        //        #region 自定义设置ID的下注时间
        //        ///等于1为默认确认状态,0为人工确认状态
        //        if (sure == 1)
        //        {
        //            ///自定义设置ID的下注时间
        //            string zdIDS = "#" + ub.GetSub("SitezdIDS", "/Controls/guess2.xml") + "#";
        //            if (zdIDS.Contains("#" + payusid.ToString() + "#"))
        //            {
        //                zdtime = Convert.ToInt32(ub.GetSub("SiteZdtime", "/Controls/guess2.xml"));
        //            }
        //        }
        //        else
        //        {
        //            zdtime = 1;
        //        }
        //        #endregion

        //        #region 已开赛的下注处理
        //        if (state == 1 || state == 3 || state == 4)
        //        {
        //            int p_basketve = 0;
        //            if (state == 3)
        //                p_basketve = 9;

        //            if (DateTime.Now > paytimes.AddSeconds(zdtime))
        //            {
        //                bool IsPass = true;
        //                #region 检查球赛即时数据 确认下注状态
        //                //即时更新
        //                TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().Getp_temptime_p_id(bcid);
        //                if (m != null)
        //                {
        //                    #region 下注前检查盘口数据变动
        //                    int p_id = Convert.ToInt32(m.p_id);
        //                    DateTime temptime = Convert.ToDateTime(m.p_temptime);
        //                    if (pType == 1)
        //                    {
        //                        string s = "";
        //                        if (state == 1)
        //                            new TPR2.Collec.Footbo().GetBoView(p_id, true);
        //                        //else
        //                        //    new TPR2.Collec.FootFalf().FootFalfPageHtml(p_id, true);
        //                    }
        //                    else
        //                    {
        //                        new TPR2.Collec.Basketbo().GetBoView(p_id, true);
        //                    }
        //                    if (state == 4)
        //                    {

        //                        IsPass = false;
        //                    }
        //                    else
        //                    {
        //                        #region 得到即时比分和封盘时间
        //                        //得到即时比分和封盘时间
        //                        TPR2.Model.guess.BaList n = new TPR2.BLL.guess.BaList().GetTemp(p_id, p_basketve);

        //                        if (pType == 1 && n.p_result_temp1 != null)
        //                        {
        //                            if (n.p_result_temp1 != p_result_temp1 || n.p_result_temp2 != p_result_temp2)//比分不同
        //                            {
        //                                IsPass = false;
        //                            }
        //                        }
        //                        #endregion

        //                        #region 进球更新的时间不同
        //                        if (m.p_temptime != n.p_temptime)//进球更新的时间不同
        //                        {
        //                            IsPass = false;
        //                        }
        //                        if (temptime != Convert.ToDateTime("1990-1-1") && temptime > paytimes && paytimes > temptime.AddSeconds(-zdtime))
        //                        {
        //                            IsPass = false;
        //                        }
        //                        #endregion

        //                        #region 让球封盘时间不同
        //                        //让球封盘时间不同
        //                        if (PayType == 1 || PayType == 2)
        //                        {
        //                            if (m.p_temptime1 != n.p_temptime1)
        //                            {
        //                                IsPass = false;
        //                            }
        //                            if (n.p_temptime1 != Convert.ToDateTime("1990-1-1") && n.p_temptime1 > paytimes && paytimes > n.p_temptime1.AddSeconds(-zdtime))
        //                            {
        //                                IsPass = false;
        //                            }
        //                        }
        //                        #endregion

        //                        #region 大小盘封盘时间不同
        //                        //大小盘封盘时间不同
        //                        if (PayType == 3 || PayType == 4)
        //                        {
        //                            if (m.p_temptime2 != n.p_temptime2)
        //                            {
        //                                IsPass = false;
        //                            }
        //                            if (n.p_temptime2 != Convert.ToDateTime("1990-1-1") && n.p_temptime2 > paytimes && paytimes > n.p_temptime2.AddSeconds(-zdtime))
        //                            {
        //                                IsPass = false;
        //                            }
        //                        }
        //                        #endregion

        //                        #region 标准盘封盘时间不同
        //                        //标准盘封盘时间不同
        //                        if (PayType == 5 || PayType == 6 || PayType == 7)
        //                        {
        //                            if (m.p_temptime3 != n.p_temptime3)
        //                            {
        //                                IsPass = false;

        //                            }
        //                            if (n.p_temptime3 != Convert.ToDateTime("1990-1-1") && n.p_temptime3 > paytimes && paytimes > n.p_temptime3.AddSeconds(-zdtime))
        //                            {
        //                                IsPass = false;

        //                            }
        //                        }
        //                        #endregion
        //                    }
        //                    #endregion

        //                    if (IsPass == false)
        //                    {
        //                        #region 投注失败,删除投注记录并退币
        //                        //投注失败,删除投注记录并退币
        //                        new TPR2.BLL.guess.BaPay().Delete(ID);
        //                        string bzTypes = string.Empty;
        //                        if (types == 0)
        //                        {
        //                            bzTypes = ub.Get("SiteBz");
        //                            new BCW.BLL.User().UpdateiGold(payusid, payusname, payCent, "" + ub.Get("SiteGqText") + "" + payview + "下注失败退回");
        //                        }
        //                        else
        //                        {
        //                            bzTypes = ub.Get("SiteBz2");
        //                            new BCW.BLL.User().UpdateiMoney(payusid, payusname, payCent, "" + ub.Get("SiteGqText") + "" + payview + "下注失败退回");
        //                        }
        //                        #endregion

        //                        #region 记录失败日志
        //                        //记录失败日志
        //                        new BCW.BLL.Gamelog().Add(2, "" + payusname + "(" + payusid + ")" + payview + "" + ub.Get("SiteGqText") + "下注失败，系统退回您" + payCent + "" + bzTypes + "", bcid, "自动");
        //                        new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "" + ub.Get("SiteGqText") + "下注失败，系统退回您" + payCent + "" + bzTypes + "");
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region 滚球投注成功 更新state=0
        //                        new TPR2.BLL.guess.BaPay().Updatestate(ID);
        //                        new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "" + ub.Get("SiteGqText") + "下注成功");
        //                        #endregion
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //        #endregion

        //        #region 未开赛的下注处理
        //        else if (state == 2)//未开赛需要验证水位的投注
        //        {
        //            if (DateTime.Now > paytimes.AddSeconds(0))
        //            {
        //                #region 检查即时数据
        //                bool IsPass = true;
        //                //即时更新
        //                TPR2.Model.guess.BaList modelBaList = new TPR2.BLL.guess.BaList().GetModel(bcid);
        //                if (modelBaList == null)
        //                {
        //                    modelBaList = new TPR2.BLL.guess.BaList().GetModelByp_id(bcid);
        //                }
        //                if (modelBaList != null)
        //                {
        //                    int p_id = Convert.ToInt32(modelBaList.p_id);
        //                    if (modelBaList.p_type == 1)
        //                        new TPR2.Collec.Footbo().GetBoView(p_id, false);
        //                    else
        //                        new TPR2.Collec.Basketbo().GetBoView(p_id, false);

        //                    modelBaList = new TPR2.BLL.guess.BaList().GetModel(bcid);//同步以上更新的水位
        //                    if (modelBaList == null)
        //                    {
        //                        modelBaList = new TPR2.BLL.guess.BaList().GetModelByp_id(bcid);
        //                    }

        //                    if (PayType == 1)
        //                    {

        //                        decimal oldlu = payonLuone;
        //                        decimal newlu = Convert.ToDecimal(modelBaList.p_one_lu);
        //                        decimal oldpk = p_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_pk);
        //                        if (oldlu - newlu >= Convert.ToDecimal(0.1) || newlu - oldlu >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                    else if (PayType == 2)
        //                    {

        //                        decimal oldlu2 = payonLutwo;
        //                        decimal newlu2 = Convert.ToDecimal(modelBaList.p_two_lu);
        //                        decimal oldpk = p_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

        //                        if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                    else if (PayType == 3)
        //                    {

        //                        decimal oldlu = payonLuone;
        //                        decimal newlu = Convert.ToDecimal(modelBaList.p_big_lu);
        //                        decimal oldpk = p_dx_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_dx_pk);
        //                        if (oldlu - newlu >= Convert.ToDecimal(0.1) || newlu - oldlu >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                    else if (PayType == 4)
        //                    {

        //                        decimal oldlu2 = payonLutwo;
        //                        decimal newlu2 = Convert.ToDecimal(modelBaList.p_small_lu);
        //                        decimal oldpk = p_dx_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_dx_pk);

        //                        if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                    else if (PayType == 5)
        //                    {

        //                        decimal oldlu2 = payonLuone;
        //                        decimal newlu2 = Convert.ToDecimal(modelBaList.p_bzs_lu);
        //                        decimal oldpk = p_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

        //                        if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                    else if (PayType == 6)
        //                    {

        //                        decimal oldlu2 = payonLutwo;
        //                        decimal newlu2 = Convert.ToDecimal(modelBaList.p_bzp_lu);
        //                        decimal oldpk = p_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

        //                        if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                    else if (PayType == 7)
        //                    {

        //                        decimal oldlu2 = payonLuthr;
        //                        decimal newlu2 = Convert.ToDecimal(modelBaList.p_bzx_lu);
        //                        decimal oldpk = p_pk;
        //                        decimal newpk = Convert.ToDecimal(modelBaList.p_pk);

        //                        if (oldlu2 - newlu2 >= Convert.ToDecimal(0.1) || newlu2 - oldlu2 >= Convert.ToDecimal(0.1))
        //                            IsPass = false;

        //                        if (!oldpk.Equals(newpk))
        //                            IsPass = false;

        //                    }
        //                }
        //                #endregion

        //                #region 下注确认
        //                if (IsPass == false)
        //                {
        //                    //投注失败,删除投注记录并退币
        //                    new TPR2.BLL.guess.BaPay().Delete(ID);
        //                    string bzTypes = string.Empty;
        //                    if (types == 0)
        //                    {
        //                        bzTypes = ub.Get("SiteBz");
        //                        new BCW.BLL.User().UpdateiGold(payusid, payusname, payCent, "" + payview + "下注失败退回");
        //                    }
        //                    else
        //                    {
        //                        bzTypes = ub.Get("SiteBz2");
        //                        new BCW.BLL.User().UpdateiMoney(payusid, payusname, payCent, "" + payview + "下注失败退回");
        //                    }
        //                    new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "下注失败，系统退回您" + payCent + "" + bzTypes + "");
        //                    //记录失败日志
        //                    new BCW.BLL.Gamelog().Add(2, "" + payusname + "(" + payusid + ")" + payview + "下注失败，标识ID" + ID + "", bcid, "自动");

        //                }
        //                else
        //                {
        //                    new TPR2.BLL.guess.BaPay().Updatestate(ID);
        //                    new BCW.BLL.Guest().Add(payusid, payusname, "您的" + payview + "下注成功");
        //                }
        //                #endregion
        //            }
        //        }
        //        #endregion
        //    }

        //}
        //HttpContext.Current.Application.UnLock();
        //-----------------------------------------------------------------------
        #endregion

        //DataSet ds2 = new TPR2.BLL.guess.BaList().GetBaListList("id,p_id", "p_ison=1 and p_del=0 and p_active=0 and p_updatetime < '" + DateTime.Now.AddMinutes(-10) + "'");
        //if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
        //    {
        //        int p_updateid = Convert.ToInt32(ds2.Tables[0].Rows[i]["id"].ToString());
        //        int p_updatep_id = Convert.ToInt32(ds2.Tables[0].Rows[i]["p_id"].ToString());
        //        new BCW.BLL.Guest().Add(10086, "客服", "[url=/bbs/guess2/showguess.aspx?gid=" + p_updateid + "]赛事ID" + p_updateid + "[/url]超过10分钟没有更新水位，请检查是否正常");
        //        new TPR2.BLL.guess.BaList().Updatep_updatetime(p_updatep_id);
        //    }
        //}
        #region 底部
        //底部
        builder.Append(Out.Tab("<div class=\"ft\">", "<br />"));
        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.Get("SiteGameUbb")).Replace("[@功能]", Utils.Function(_IsFoot))));
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 版本
        //版本
        builder.Append("<!-- " + ub.Get("SiteVersion") + " @author Light-->");
        #endregion


        string strAlign = ub.Get("SiteAlign");
        bool IsNetwork = new BCW.BLL.Network().Exists();
        string builderText = "";
        if (meid > 0 || IsNetwork == true)
        {
            #region 聊天 喇叭
            if (IsNetwork == true)
            {
                bool IsView = false;
                DataSet ds = new BCW.BLL.Network().GetList("TOP 1 ID,Title,UsID,UsName,OnIDs,IsUbb", "Types=2 and OverTime>='" + DateTime.Now + "' ORDER BY ID DESC");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int IsUbb = int.Parse(ds.Tables[0].Rows[0]["IsUbb"].ToString());
                    int ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    string OnIDs = ds.Tables[0].Rows[0]["OnIDs"].ToString();
                    if (OnIDs.Contains("#" + meid + "#"))
                    {
                        int usid = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());

                        if (IsUbb == 1 || new BCW.BLL.Role().IsAllMode(usid) || Utils.GetTopDomain().Contains("th"))
                        {
                            builderText += "[<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=info&amp;id=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[0]["UsName"] + "</a>]聊天：" + Out.SysUBB(ds.Tables[0].Rows[0]["Title"].ToString()) + "";
                        }
                        else
                        {
                            builderText += "[<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=info&amp;id=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[0]["UsName"] + "</a>]聊天：" + ds.Tables[0].Rows[0]["Title"] + "";
                        }

                        OnIDs = OnIDs.Replace("#" + meid + "#", "");
                        new BCW.BLL.Network().UpdateOnIDs(ID, OnIDs);
                        IsView = true;
                    }
                }
                if (!IsView)
                {
                    ds = new BCW.BLL.Network().GetList("TOP 1 Title,UsID,UsName,IsUbb", "Types<=1 and OverTime>='" + DateTime.Now + "' ORDER BY NEWID()");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string NetWorkTitle = ds.Tables[0].Rows[0]["Title"].ToString();
                        if (ds.Tables[0].Rows[0]["IsUbb"].ToString() == "1")
                            NetWorkTitle = Out.SysUBB(NetWorkTitle);

                        builderText += "[<a href=\"" + Utils.getUrl("/bbs/network.aspx?backurl=" + Utils.PostPage(1) + "") + "\">喇叭</a>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[0]["UsName"] + "</a>：" + NetWorkTitle + "";
                    }
                }
            }

            if (builderText != "")
            {
                builder2.Append(Out.Tab("<div class=\"top\">", ""));
                builder2.Append(builderText);
                builder2.Append(Out.Tab("</div>", "</p><p align=\"" + ub.Get("SiteAlign") + "\">"));
                strAlign = "center";
            }
            #endregion

            #region 短消息提示
            //短消息提示
            if (string.IsNullOrEmpty(Request.Form.ToString()))
            {

                int smsCount = new BCW.BLL.Guest().GetCount(meid);
                int smsXCount = new BCW.BLL.Guest().GetXCount(meid);
                if (smsCount > 0 || smsXCount > 0)
                {
                    string actUrl = string.Empty;
                    string actUrl2 = string.Empty;
                    if (smsCount > 0)
                    {
                        if (Utils.GetDomain().Contains("lt388"))
                        {
                            actUrl = "act=view&amp;";
                        }
                        else
                        {
                            actUrl = "act=newlist&amp;";
                        }
                    }

                    if (smsXCount > 0)
                    {
                        if (Utils.GetDomain().Contains("lt388"))
                        {
                            actUrl2 = "act=view&amp;ptype=1&amp;";
                        }
                        else
                        {
                            actUrl2 = "act=view&amp;hid=0&amp;";
                        }
                    }
                    else
                        actUrl2 = "ptype=1&amp;";

                    builderText = "<b>新内线<a href=\"" + Utils.getUrl("/bbs/guest.aspx?" + actUrl + "backurl=" + Utils.PostPage(1) + "") + "\">(" + smsCount + ")</a>系统<a href=\"" + Utils.getUrl("/bbs/guest.aspx?" + actUrl2 + "backurl=" + Utils.PostPage(1) + "") + "\">(" + smsXCount + ")</a></b>";

                    builder3.Append(Out.Tab("<div>", ""));
                    builder3.Append(builderText);
                    builder3.Append(Out.Tab("</div>", "<br />"));
                }
            }
            #endregion
        }

        #region 头部
        //头部
        if (_Refresh == 0)
        {
            new Out().head(Utils.ForWordType(_Title), strAlign);
        }
        else
        {
            new Out().head(Utils.ForWordType(_Title), _Gourl, _Refresh.ToString(), strAlign);
        }
        #endregion

        #region 顶部Ubb
        //顶部Ubb
        string GameTopUbb = string.Empty;
        if (ub.Get("SiteGameTopUbb") != "")
        {
            GameTopUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.Get("SiteGameTopUbb")));
            if (GameTopUbb.IndexOf("</div>") == -1)
            {
                builder3.Append(Out.Tab("<div>", ""));
                builder3.Append(GameTopUbb);
                builder3.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder3.Append(GameTopUbb);
            }
        }
        #endregion

        //消息提示
        Response.Write(Utils.ForWordType(builder2.ToString()));
        //顶部Ubb
        Response.Write(Utils.ForWordType(builder3.ToString()));
        //尾部
        //builder.Append(new Out().foot());
    }
}