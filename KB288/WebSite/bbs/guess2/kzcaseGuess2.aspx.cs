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

public partial class bbs_guess_kzcaseGuess2 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "个人庄-庄家兑奖";
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
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);

            DataSet ds = new TPR2.BLL.guess.BaPayMe().GetBaPayMeList("bcid,qrPrice", "ID=" + pid + " and usid=" + meid + " and qrPrice>0 and (isqr=1 or (isqr=0 and kjTime<'" + DateTime.Now.AddHours(-1) + "'))");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set isqr=9 where id=" + pid + "");

                int bcid = int.Parse(ds.Tables[0].Rows[0]["bcid"].ToString());
                long qrPrice = Int64.Parse(ds.Tables[0].Rows[0]["qrPrice"].ToString());

                new BCW.BLL.User().UpdateiGold(meid, qrPrice, "个人庄赛事ID" + bcid + "-" + pid + "兑奖");
                winMoney += qrPrice;
            }
 
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("kzcaseGuess2.aspx"), "1");
    }


    private void GetCase(int meid)
    {
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择赛事无效"));

        DataSet ds = new TPR2.BLL.guess.BaPayMe().GetBaPayMeList("bcid,qrPrice", "ID=" + pid + " and usid=" + meid + " and qrPrice>0 and (isqr=1 or (isqr=0 and kjTime<'" + DateTime.Now.AddHours(-1) + "'))");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            BCW.Data.SqlHelper.ExecuteSql("update tb_BaPayMe set isqr=9 where id=" + pid + "");

            int bcid = int.Parse(ds.Tables[0].Rows[0]["bcid"].ToString());
            long qrPrice = Int64.Parse(ds.Tables[0].Rows[0]["qrPrice"].ToString());

            new BCW.BLL.User().UpdateiGold(meid, qrPrice, "个人庄赛事ID" + bcid + "-" + pid + "兑奖");
            Utils.Success("兑奖", "恭喜，成功兑奖" + qrPrice+ "" + ub.Get("SiteBz") + "", Utils.getUrl("kzcaseGuess2.aspx"), "1");
        }
        else
        {
            Utils.Success("兑奖", "不存在的兑奖记录", Utils.getUrl("kzcaseGuess2.aspx"), "1");
        }
    }

    private void ReloadPage(int meid)
    {
        long gold = new BCW.BLL.User().GetGold(meid);
        long money = new BCW.BLL.User().GetMoney(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(money) + "" + ub.Get("SiteBz2") + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 1, @"^[0-9]*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("可兑奖|");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess2.aspx?ptype=0&amp;gid=" + gid + ""), "可兑奖") + "|");

        if (ptype == 1)
            builder.Append("待兑奖|");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess2.aspx?ptype=1&amp;gid=" + gid + ""), "待兑奖") + "|");

        if (ptype == 2)
            builder.Append("已兑奖|");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess2.aspx?ptype=2&amp;gid=" + gid + ""), "已兑奖") + "|");

        if (ptype == 3)
            builder.Append("纠纷");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess2.aspx?ptype=3&amp;gid=" + gid + ""), "纠纷") + "");

        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("待开奖：指开奖后的1小时内，闲家还没有确认结果，超出1小时如果还没有确认或产生纠纷则可以兑奖");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 3)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("纠纷：由管理员来裁决结果，双方可用手工进行赔付");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "gid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "usid=" + meid + " and ";
        if (ptype == 0)
            strWhere += "qrPrice>0 and (isqr=1 or (isqr=0 and kjTime<'"+DateTime.Now.AddHours(-1)+"'))";//可兑奖
        if (ptype == 1)
            strWhere += "qrPrice>0 and isqr=0 and kjTime>='" + DateTime.Now.AddHours(-1) + "'";//待兑奖
        else if (ptype == 2)
            strWhere += "qrPrice>0 and isqr=9";//已兑奖
        else if (ptype == 3)
            strWhere += "isqr=2";//纠纷

        if (gid > 0)
            strWhere += " and bcid=" + gid + "";

        // 开始读取竞猜
        string arrId = string.Empty;
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

                builder.AppendFormat(Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{0}({1})") + ":{2}[{3}]庄赢"+n.qrPrice+""+ub.Get("SiteBz")+"", n.payusname, n.payusid, Out.SysUBB(n.payview), n.paytimes);
                if (n.isqr == 0)
                {
                    if (n.kjTime < DateTime.Now.AddHours(-1))
                    {
                        builder.Append("[可兑奖]");
                        builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess2.aspx?act=get&amp;pid=" + n.ID + ""), "兑奖") + " ");

                    }
                    else
                    {
                        builder.Append("[待兑奖]");
                    }
                }
                else if (n.isqr == 1)
                {
                    builder.Append("[可兑奖]");
                    builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess2.aspx?act=get&amp;pid=" + n.ID + ""), "兑奖") + " ");
                }
                else if (n.isqr == 2)
                {
                    builder.Append("[纠纷]");
                }
                else if (n.isqr == 9)
                {
                    builder.Append("[已兑奖]");
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
        if (ptype == 0 && !string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'post";
            string strOthe = "本页兑奖,kzcaseGuess2.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }

    }
}