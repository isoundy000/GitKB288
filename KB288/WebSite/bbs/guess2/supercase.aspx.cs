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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class bbs_guess2_supercase : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "超级竞猜兑奖";
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        long gold = new BCW.BLL.User().GetGold(meid);
        string act = "";
        act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "get":
                GetCase(meid);
                break;
            case "post":
                PostCase(meid);
                break;
            default:
                ReloadPage(meid, gold);
                break;
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx"), "超级竞猜") + " ");
        builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=list"), "下注记录") + "<br />");
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
        long getwinMoney = 0;
        long winMoney = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new TPR2.BLL.guess.Super().ExistsIsCase(pid, meid))
            {
                int bcid = new TPR2.BLL.guess.BaPay().Getbcid(pid);

                new TPR2.BLL.guess.Super().UpdateIsCase(pid);
                //操作币
                winMoney = Convert.ToInt64(new TPR2.BLL.guess.Super().GetgetMoney(pid));
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "超级竞猜赛事ID" + bcid + "#记录ID" + pid + "兑奖");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("caseGuess.aspx?ptype=" + ptype + ""), "1");
    }


    private void GetCase(int meid)
    {
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择赛事无效"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));

        if (new TPR2.BLL.guess.Super().ExistsIsCase(pid, meid))
        {
            int bcid = new TPR2.BLL.guess.BaPay().Getbcid(pid);

            new TPR2.BLL.guess.Super().UpdateIsCase(pid);
            //操作币
            long winMoney = Convert.ToInt64(new TPR2.BLL.guess.Super().GetgetMoney(pid));
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "超级竞猜赛事ID" + bcid + "#记录ID" + pid + "兑奖");
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("caseGuess.aspx?ptype=" + ptype + ""), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("caseGuess.aspx?ptype=" + ptype + ""), "1");
        }
    }

    private void ReloadPage(int meid, long gold)
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您现在有" + gold + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strWhere = "";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsID=" + meid + " and IsOpen=1 and Status=1 and p_case=0";

        // 开始读取竞猜
        string arrId = "";
        IList<TPR2.Model.guess.Super> listSuper = new TPR2.BLL.guess.Super().GetSupers(pageIndex, pageSize, strWhere, out recordCount);
        if (listSuper.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.Super n in listSuper)
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

                string sWin = string.Empty;

                sWin = "赢";

                builder.Append(Out.waplink(Utils.getUrl("betsuper.aspx?act=listview&amp;id=" + n.ID + ""), "[" + sWin + "]串竞猜" + n.PayCent + "" + ub.Get("SiteBz") + "") + "");

                builder.AppendFormat(" 赢{0}币," + Out.waplink(Utils.getUrl("supercase.aspx?act=get&amp;pid=" + n.ID + ""), "兑奖") + "", Convert.ToDouble(n.getMoney));
                
                string[] Title = Regex.Split(Utils.Mid(n.Title, 2, n.Title.Length), "##");
                for (int i = 0; i < Title.Length; i++)
                {
                    builder.Append("<br />场次" + (i + 1) + ":" + Title[i] + "");

                }
                arrId = arrId + " " + n.ID;
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
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'post";
            string strOthe = "本页兑奖,supercase.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }
    }
}
