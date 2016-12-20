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
using TPR2.Common.Guess;

public partial class Manage_guess2_kzbother : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/guess2.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "个人庄-纠纷处理";

        string act =  Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "open":
                OpenPage();
                break;
            default:
                ReloadPage();
                break;
        }

    }

    private void OpenPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]*$", "ID无效"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        TPR2.BLL.guess.BaListMe bll = new TPR2.BLL.guess.BaListMe();
        TPR2.Model.guess.BaListMe model = bll.GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = "开奖赛事" + model.p_one + "VS" + model.p_two + "-记录" + pid + "";

        if (ac == "确定开奖" || ac == Utils.ToTChinese("确定开奖"))
        {
            int opentype = Utils.ParseInt(Utils.GetRequest("opentype", "post", 2, @"^[0-2]$", "开奖类型错误"));
            int resultone = Utils.ParseInt(Utils.GetRequest("resultone", "post", 2, @"^[0-9]*$", "请正确输入比分"));
            int resulttwo = Utils.ParseInt(Utils.GetRequest("resulttwo", "post", 2, @"^[0-9]*$", "请正确输入比分"));
            string jietime = "";
            if (opentype == 1)
            {
                DateTime jietime2 = Utils.ParseTime(Utils.GetRequest("jietime", "post", 2, DT.RegexTime, "请正确填写截止时间"));
                jietime = jietime2.ToString();
            }
            if (info == "ok")
            {

                //游戏日志记录
                int ManageId = new BCW.User.Manage().IsManageLogin();
                string[] p_pageArr = {"act", "ac", "opentype", "resultone", "resulttwo", "jietime", "gid", "pid", "info" };
                BCW.User.GameLog.GameLogPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员开奖个人庄赛事" + model.p_one + "VS" + model.p_two + "-记录" + gid + "_" + pid + "", gid);


                //之前比分
                int oldresultone = Convert.ToInt32(model.p_result_one);
                int oldresulttwo = Convert.ToInt32(model.p_result_two);

                //更新比分
                model.p_result_one = resultone;
                model.p_result_two = resulttwo;

                if (opentype == 3)
                    model.p_active = 2;//平盘标识
                else
                    model.p_active = 1;

                //bll.UpdateResult(model);

                if (model.p_ison == 1)
                {
                    if (opentype == 1)
                    {
                        UpdateCaseOnce(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, jietime, pid);
                        UpdateCasePP(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, jietime, pid);
                    }
                    else if (opentype == 2)
                    {
                        UpdateCasePP(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, "", pid);
                    }
                    else
                    {
                        UpdateCaseOnce(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, jietime, pid);
                    }

                }
                else
                {
                    if (opentype == 1)
                    {
                        UpdateCase(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, jietime, pid);
                        UpdateCasePP(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, jietime, pid);
                    }
                    else if (opentype == 2)
                    {
                        UpdateCasePP(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, "", pid);
                    }
                    else
                    {
                        UpdateCase(resultone, resulttwo, gid, Convert.ToInt32(model.p_type), model.usid, jietime, pid);
                    }
                }
                //发送内线
                string strLog = "管理员对个人庄纠纷《" + model.p_title + ":" + model.p_two + "VS" + model.p_two + "》进行重开奖，原开奖比分" + oldresultone + ":" + oldresulttwo + "，重新开奖比分:" + resultone + ":" + resulttwo + "[url=/bbs/guess2/kzcaseGuess.aspx]马上兑奖[/url]";
                new BCW.BLL.Guest().Add(1, model.usid, new BCW.BLL.User().GetUsName(model.usid), strLog);

                Utils.Success("开奖", "开奖" + resultone + ":" + resulttwo + "成功..", Utils.getUrl("kzbother.aspx"), "1");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                if (opentype == 0)
                    builder.Append("请确认比分" + resultone + ":" + resulttwo + "");
                else if (opentype == 1)
                    builder.Append("请确认比分" + resultone + ":" + resulttwo + "|截时时间:" + jietime + "<br />温馨提示;截时时间");
                else
                    builder.Append("请确定平盘");

                builder.Append(Out.Tab("</div>", "<br />"));
                string strName = "resultone,resulttwo,opentype,jietime,gid,pid,info,act";
                string strValu = "" + resultone + "'" + resulttwo + "'" + opentype + "'" + jietime + "'" + gid + "'" + pid + "'ok'open";
                string strOthe = "确定开奖,kzbother.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.waplink(Utils.getUrl("kzbother.aspx"), "返回上一级"));
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));

            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("开奖赛事" + model.p_one + "VS" + model.p_two + "-记录" + pid + "");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("开赛:" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0));
            builder.Append(Out.Tab("</div>", ""));

            string strText = string.Empty;
            string strName = string.Empty;
            string strType = string.Empty;
            string strValu = string.Empty;
            string strEmpt = string.Empty;

            strText = "*填写比分/,比/,开奖方式:/,截时时间(选择截时开奖时生效):/,,,";
            strName = "resultone,resulttwo,opentype,jietime,gid,pid,act";
            strType = "num,num,select,date,hidden,hidden,hidden";
            strValu = "0'0'0'" + DT.FormatDate(DateTime.Now, 0) + "'" + gid + "'" + pid + "'open";
            strEmpt = "false,false,0|正常开奖|1|截时开奖|2|平盘开奖,false,false,false";

            string strIdea = "/";
            string strOthe = "确定开奖,kzbother.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("kzbother.aspx"), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("纠纷处理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "isqr=2";//纠纷

        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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

                builder.AppendFormat(Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{0}({1})") + ":{2}[{3}]庄赢" + n.qrPrice + "" + ub.Get("SiteBz") + "", n.payusname, n.payusid, Out.SysUBB(n.payview), n.paytimes);

                builder.Append(Out.waplink(Utils.getUrl("kzbother.aspx?act=open&amp;gid=" + n.bcid + "&amp;pid=" + n.ID + ""), "重开奖") + " ");

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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    //遍历所有下注更新开奖(平盘)
    private void UpdateCasePP(int resultone, int resulttwo, int gid, int p_type, int usid, string jietime, int pid)
    {
        string strWhere = "";
        int recordCount = 0;
        strWhere = "bcid=" + gid + " and id=" + pid + "";
        if (jietime != "")
            strWhere += " and paytime<" + jietime + "";

        TPR2.Model.guess.BaOrder objBaOrder = new TPR2.Model.guess.BaOrder();

        // 开始查询并更新之
        recordCount = 0;

        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(1, 5000, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
            {
                //------------------------------------------------------
                //平盘
                n.p_result_one = resultone;
                n.p_result_two = resulttwo;
                n.p_active = 2;
                n.p_getMoney = n.payCent;
                new TPR2.BLL.guess.BaPayMe().UpdatePPCase(n);

                //币种
                string bzTypes = string.Empty;
                if (n.Types == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                //发送内线
                string strLog = string.Empty;
                strLog = "" + n.payview + "[br]庄家开出平盘，返您" + Convert.ToDouble(n.payCent) + "" + bzTypes + "，如对开奖结果无异议请[url=/bbs/guess2/kzcaseGuess.aspx]马上兑奖[/url]，如有异议请1小时内点击[url=/bbs/guess2/kzcaseGuess.aspx?act=qr&amp;pid=" + n.ID + "&amp;ptype=2]不同意开奖结果[/url]并向管理员反馈。";

                new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);

                //庄家得到币
                long DiffPrice = n.DiffPrice;
                long WinPrice = 0;

                WinPrice = DiffPrice;
                BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set qrPrice=" + WinPrice + ",kjTime='" + DateTime.Now + "',isqr=0 where id=" + n.ID + "");

                //string usname = new BCW.BLL.User().GetUsName(usid);
                //new BCW.BLL.User().UpdateiGold(usid, usname, WinPrice, "个人庄(" + gid + "-" + n.ID + ")赢利");
                //string strLog2 = string.Empty;
                //strLog2 = "个人庄:[url=/bbs/uinfo.aspx?uid=" + n.payusid + "]" + n.payusname + "[/url]:" + n.payview + "[br]结果:平盘，系统返彩您" + WinPrice + "" + bzTypes + "";
                //new BCW.BLL.Guest().Add(1, usid, usname, strLog2);
            }

        }
    }

    //遍历所有下注更新开奖
    private void UpdateCase(int resultone, int resulttwo, int gid, int p_type, int usid, string jietime, int pid)
    {
        string strWhere = "";
        int recordCount = 0;
        int p_intWin = 0;
        decimal p_intDuVal = 0;
        strWhere = "bcid=" + gid + " and id=" + pid + "";
        TPR2.Model.guess.BaOrder objBaOrder = new TPR2.Model.guess.BaOrder();

        // 开始查询并更新之
        recordCount = 0;
        p_intWin = 0;
        p_intDuVal = 0;
        if (jietime != "")
            strWhere += " and paytimes>='" + jietime + "'";

        string WinType = "";

        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(1, 5000, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzZqClass.getZqsxCase(n), out p_intDuVal, out p_intWin, out WinType);
                    else if (n.PayType == 3 || n.PayType == 4)
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzZqClass.getZqdxCase(n), out p_intDuVal, out p_intWin, out WinType);
                    else
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzZqClass.getZqbzCase(n), out p_intDuVal, out p_intWin, out WinType);
                }
                else
                {
                    if (n.PayType == 1 || n.PayType == 2)
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzLqClass.getLqsxCase(n), out p_intDuVal, out p_intWin, out WinType);
                    else
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzLqClass.getLqdxCase(n), out p_intDuVal, out p_intWin, out WinType);
                }

                if (p_intWin == 1)
                {
                    //发送内线
                    if (Convert.ToInt32(n.itypes) == 0)
                    {
                        string strLog = string.Empty;
                        strLog = "" + n.payview + "[br]庄家开出结果:" + resultone + ":" + resulttwo + "，返您" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "，如对开奖结果无异议请[url=/bbs/guess2/kzcaseGuess.aspx]马上兑奖[/url]，如有异议请1小时内点击[url=/bbs/guess2/kzcaseGuess.aspx?act=qr&amp;pid=" + n.ID + "&amp;ptype=2]不同意开奖结果[/url]并向管理员反馈。";

                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                    }
                }
                else
                {
                    //闲家输
                    if (Convert.ToInt32(n.itypes) == 0)
                    {
                        string strLog = string.Empty;
                        strLog = "" + n.payview + "[br]庄家开出结果:" + resultone + ":" + resulttwo + "，您输了" + Convert.ToDouble(n.payCent) + "" + bzTypes + "，如对开奖结果有异议请1小时内向管理员反馈，否则请在1小时内点击[url=/bbs/guess2/kzcaseGuess.aspx?act=qr&amp;pid=" + n.ID + "&amp;ptype=1]同意开奖结果[/url]，超时则自动确认。";
                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                    }
                }

                //庄家得到币
                long DiffPrice = n.DiffPrice;
                long WinPrice = 0;
                if (WinType == "赢半")
                {
                    WinPrice = Convert.ToInt64(DiffPrice / 2);
                }
                else if (WinType == "输半")
                {
                    WinPrice = DiffPrice + Convert.ToInt64(n.payCent / 2);
                }
                else if (WinType == "平盘")
                {
                    WinPrice = DiffPrice;
                }
                else if (WinType == "全输")
                {
                    WinPrice = DiffPrice + Convert.ToInt64(n.payCent);
                }

                BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set qrPrice=" + WinPrice + ",kjTime='" + DateTime.Now + "',isqr=0 where id=" + n.ID + "");

                //if (WinPrice > 0)
                //{
                //    string usname = new BCW.BLL.User().GetUsName(usid);
                //    new BCW.BLL.User().UpdateiGold(usid, usname, WinPrice, "个人庄(" + gid + "-" + n.ID + ")赢利");
                //    string strLog2 = string.Empty;
                //    strLog2 = "[url=/bbs/uinfo.aspx?uid=" + n.payusid + "]" + n.payusname + "[/url]:" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "[闲家" + WinType + "]，系统返彩您" + WinPrice + "" + bzTypes + "";
                //    new BCW.BLL.Guest().Add(1, usid, usname, strLog2);
                //}

            }
        }
    }

    //遍历所有下注更新开奖(走地开奖)
    private void UpdateCaseOnce(int resultone, int resulttwo, int gid, int p_type, int usid, string jietime, int pid)
    {
        int OnceMin = Convert.ToInt32(ub.GetSub("SiteOnce", xmlPath));//走地时间限制

        string strWhere = "";
        int recordCount = 0;
        int p_intWin = 0;
        decimal p_intDuVal = 0;
        strWhere = "bcid=" + gid + " and id=" + pid + "";
        if (jietime != "")
            strWhere += " and paytimes>='" + jietime + "'";

        TPR2.Model.guess.BaOrder objBaOrder = new TPR2.Model.guess.BaOrder();
        //取得比分时间段
        string stronce = new TPR2.BLL.guess.BaList().Getonce(gid);

        // 开始查询并更新之
        recordCount = 0;
        p_intWin = 0;
        p_intDuVal = 0;
        string WinType = "";


        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(1, 5000, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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
                    {
                        //算出走地（总分减下注时的比分）
                        n.p_result_one = resultone - Convert.ToInt32(n.p_result_temp1);
                        n.p_result_two = resulttwo - Convert.ToInt32(n.p_result_temp2);
                        string p_strVal = KzZqClass.getZqsxCase(n);
                        //重新取值
                        n.p_result_one = resultone;
                        n.p_result_two = resulttwo;

                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, p_strVal, out p_intDuVal, out p_intWin, out WinType);
                    }
                    else if (n.PayType == 3 || n.PayType == 4)
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzZqClass.getZqdxCase(n), out p_intDuVal, out p_intWin, out WinType);
                    else
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzZqClass.getZqbzCase(n), out p_intDuVal, out p_intWin, out WinType);
                }
                else
                {
                    if (n.PayType == 1 || n.PayType == 2)
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzLqClass.getLqsxCase(n), out p_intDuVal, out p_intWin, out WinType);
                    else
                        new TPR2.BLL.guess.BaPayMe().UpdateCase(n, KzLqClass.getLqdxCase(n), out p_intDuVal, out p_intWin, out WinType);
                }

                if (p_intWin == 1)
                {
                    //闲家赢（平盘或赢半、输半）
                    if (Convert.ToInt32(n.itypes) == 0)
                    {
                        string strLog = string.Empty;
                        strLog = "" + n.payview + "[br]庄家开出结果:" + resultone + ":" + resulttwo + "，返您" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "，如对开奖结果无异议请[url=/bbs/guess2/kzcaseGuess.aspx]马上兑奖[/url]，如有异议请1小时内点击[url=/bbs/guess2/kzcaseGuess.aspx?act=qr&amp;pid=" + n.ID + "&amp;ptype=2]不同意开奖结果[/url]并向管理员反馈。";
                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                    }
                }
                else
                {
                    //闲家输
                    if (Convert.ToInt32(n.itypes) == 0)
                    {
                        string strLog = string.Empty;
                        strLog = "" + n.payview + "[br]庄家开出结果:" + resultone + ":" + resulttwo + "，您输了" + Convert.ToDouble(n.payCent) + "" + bzTypes + "，如对开奖结果有异议请1小时内向管理员反馈，否则请在1小时内点击[url=/bbs/guess2/kzcaseGuess.aspx?act=qr&amp;pid=" + n.ID + "&amp;ptype=1]同意开奖结果[/url]，超时则自动确认。";
                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                    }
                }


                //庄家得到币
                long DiffPrice = n.DiffPrice;
                long WinPrice = 0;
                if (WinType == "赢半")
                {
                    WinPrice = Convert.ToInt64(DiffPrice / 2);
                }
                else if (WinType == "输半")
                {
                    WinPrice = DiffPrice + Convert.ToInt64(n.payCent / 2);
                }
                else if (WinType == "平盘")
                {
                    WinPrice = DiffPrice;
                }
                else if (WinType == "全输")
                {
                    WinPrice = DiffPrice + Convert.ToInt64(n.payCent);
                }
                BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set qrPrice=" + WinPrice + ",kjTime='" + DateTime.Now + "',isqr=0 where id=" + n.ID + "");

                //if (WinPrice > 0)
                //{
                //    string usname = new BCW.BLL.User().GetUsName(usid);
                //    new BCW.BLL.User().UpdateiGold(usid, usname, WinPrice, "个人庄(" + gid + "-" + n.ID + ")赢利");
                //    string strLog2 = string.Empty;
                //    strLog2 = "[url=/bbs/uinfo.aspx?uid=" + n.payusid + "]" + n.payusname + "[/url]:" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "[闲家" + WinType + "]，系统返彩您" + WinPrice + "" + bzTypes + "";
                //    new BCW.BLL.Guest().Add(1, usid, usname, strLog2);
                //}

            }
        }
    }
}