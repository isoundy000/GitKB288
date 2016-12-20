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

public partial class bbs_guess_kzcaseGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "个人庄-竞猜兑奖";
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act =  Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "get":
                GetCase(meid);
                break;
            case "qr":
                QrCase(meid);
                break;
            case "post":
                PostCase(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx"), "返回个人庄&gt;&gt;") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PostCase(int meid)
    {
        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        string[] strArrId = arrId.Split(",".ToCharArray());
        long winMoney = 0;
        long winMoney2 = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new TPR2.BLL.guess.BaPayMe().ExistsIsCase(pid, meid))
            {

                BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set isqr=1 where id=" + pid + "");
                new TPR2.BLL.guess.BaPayMe().UpdateIsCase(pid);
                //操作币
                long win = Convert.ToInt64(new TPR2.BLL.guess.BaPayMe().Getp_getMoney(pid));
                int Types = new TPR2.BLL.guess.BaPayMe().GetTypes(pid);
                if (Types == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, win, "个人庄-竞猜赛事ID" + pid + "兑奖");
                    winMoney += win;
                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(meid, win, "个人庄-竞猜赛事ID" + pid + "兑奖");
                    winMoney2 += win;
                }

            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("kzcaseGuess.aspx?ptype=" + ptype + ""), "1");
    }


    private void GetCase(int meid)
    {
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择赛事无效"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));

        if (new TPR2.BLL.guess.BaPayMe().ExistsIsCase(pid, meid))
        {

            BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set isqr=1 where id=" + pid + "");

            new TPR2.BLL.guess.BaPayMe().UpdateIsCase(pid);
            //操作币
            long win = Convert.ToInt64(new TPR2.BLL.guess.BaPayMe().Getp_getMoney(pid));
            int Types = new TPR2.BLL.guess.BaPayMe().GetTypes(pid);
            if (Types == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, win, "竞猜赛事ID" + pid + "兑奖");
                Utils.Success("兑奖", "恭喜，成功兑奖" + win + "" + ub.Get("SiteBz") + "", Utils.getUrl("kzcaseGuess.aspx?ptype=" + ptype + ""), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, win, "竞猜赛事ID" + pid + "兑奖");
                Utils.Success("兑奖", "恭喜，成功兑奖" + win + "" + ub.Get("SiteBz2") + "", Utils.getUrl("kzcaseGuess.aspx?ptype=" + ptype + ""), "1");
            }
            
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("kzcaseGuess.aspx?ptype=" + ptype + ""), "1");
        }
    }

    private void QrCase(int meid)
    {
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择赛事无效"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 2, @"^[1-2]$", "类型错误"));

        DataSet ds = new TPR2.BLL.guess.BaPayMe().GetBaPayMeList("payview,bcid,usid,payusid,payusname", "ID=" + pid + " and payusid=" + meid + " and isqr=0");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string payview = ds.Tables[0].Rows[0]["payview"].ToString();
            int bcid = int.Parse(ds.Tables[0].Rows[0]["bcid"].ToString());
            int usid = int.Parse(ds.Tables[0].Rows[0]["usid"].ToString());
            int payusid = int.Parse(ds.Tables[0].Rows[0]["payusid"].ToString());
            string payusname = ds.Tables[0].Rows[0]["payusname"].ToString();

            BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set isqr=" + ptype + " where id=" + pid + "");
            if (ptype == 1)
            {
                //通知庄家
                string strLog = string.Empty;
                strLog = "闲家[url=/bbs/uinfo.aspx?uid=" + payusid + "]" + payusname + "[/url]已确认" + payview + "[br][url=/bbs/guess2/kzcaseGuess2.aspx]马上到赛事处兑奖[/url]";
                new BCW.BLL.Guest().Add(1, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                Utils.Success("同意结果", "同意结果操作成功，庄当前已可以顺利兑奖", Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "1");
            }
            else
            {
                //通知庄家
                string strLog = string.Empty;
                strLog = "闲家[url=/bbs/uinfo.aspx?uid=" + payusid + "]" + payusname + "[/url]:" + payview + "对开奖结果不同意，请双方联系管理员解决[br][url=/bbs/guess2/kzcaseGuess2.aspx?ptype=3]查看纠纷详细[/url]";
                new BCW.BLL.Guest().Add(1, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                Utils.Success("不同意结果", "不同意结果操作成功，请联系管理员解决", Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "1");
            }
        }
        else
        {
            Utils.Success("结果", "不存在的记录", Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "1");
        }
    }

    private void ReloadPage(int meid)
    {
        long gold = new BCW.BLL.User().GetGold(meid);
        long money = new BCW.BLL.User().GetMoney(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(money) + "" + ub.Get("SiteBz2") + "");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (ptype == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess.aspx?ptype=0"), "全部") + " ");

        if (ptype == 1)
            builder.Append("足球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess.aspx?ptype=1"), "足球") + " ");

        if (ptype == 2)
            builder.Append("篮球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess.aspx?ptype=2"), "篮球") + " ");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";

        strWhere += "p_active>0 and itypes=0 ";

        if (ptype != 0)
            strWhere += " and ptype=" + ptype + "";

        strWhere += " and payusid=" + meid + " and p_case=0 and p_getMoney > 0 and isqr<>2";

        // 开始读取竞猜
        string arrId = "";
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
                string bzType = string.Empty;
                if (n.Types == 0)
                    bzType = ub.Get("SiteBz");
                else
                    bzType = ub.Get("SiteBz2");

                if (n.p_active == 2)
                {
                    builder.AppendFormat("{0},平盘<br />时间:{1}", Out.SysUBB(n.payview), n.paytimes);
                    builder.AppendFormat(" 返{0}" + bzType + "," + Out.waplink(Utils.getUrl("kzcaseGuess.aspx?ptype=" + ptype + "&amp;act=get&amp;pid=" + n.ID + ""), "兑奖") + "", Convert.ToDouble(n.p_getMoney));
                }
                else
                {
                    builder.AppendFormat("{0},结果{1}:{2}<br />时间:{3}", Out.SysUBB(n.payview), n.p_result_one, n.p_result_two, n.paytimes);
                    builder.AppendFormat(" 赢{0}" + bzType + "," + Out.waplink(Utils.getUrl("kzcaseGuess.aspx?ptype=" + ptype + "&amp;act=get&amp;pid=" + n.ID + ""), "兑奖") + "", Convert.ToDouble(n.p_getMoney));
                }
                builder.Append(Out.Tab("</div>", ""));
                arrId = arrId + " " + n.ID;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'post";
            string strOthe = "本页兑奖,kzcaseGuess.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }
    }
}