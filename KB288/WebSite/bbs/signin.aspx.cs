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

public partial class bbs_signin : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "每日签到";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.Model.User model = new BCW.BLL.User().GetSignData(meid);
        if (string.IsNullOrEmpty(model.SignTime.ToString()))
        {
            model.SignTime = DateTime.Now.AddDays(-1);
        }
        if (model.SignTime > DateTime.Parse(DateTime.Now.ToLongDateString()))
        {
            Utils.Error("今天你已签到过了！<br /><a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=1") + "\">签到排行榜</a>", "");
        }
        int SignKeep = 1;
        int SignTotal = model.SignTotal + 1;
        if (model.SignTime >= DateTime.Parse(DateTime.Now.ToLongDateString()).AddDays(-1))
        {
            SignKeep = model.SignKeep + 1;
        }
        //更新签到信息
        new BCW.BLL.User().UpdateSingData(meid, SignTotal, SignKeep);
        builder.Append(Out.Tab("<div class=\"title\">欢迎签到</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("签到成功，奖励" + BCW.User.Users.GetWinCent(12, meid) + "");
        //积分操作
        new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Sign, meid);

        if (SignKeep % 30 == 0)
        {
            builder.Append("<br />连续一个月签到，额外追加奖励" + BCW.User.Users.GetWinCent(14, meid) + "");
            //动态记录
            new BCW.BLL.Action().Add(meid, "在空间连续一个月签到获得奖励");
            //积分操作
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Sign3, meid);
        }
        else if (SignKeep % 7 == 0)
        {
            builder.Append("<br />连续一周签到，额外追加奖励" + BCW.User.Users.GetWinCent(13, meid) + "");
            //动态记录
            new BCW.BLL.Action().Add(meid, "在空间连续一周签到获得奖励");
            //积分操作
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Sign2, meid);
        }
        else
        {
            //动态记录
            new BCW.BLL.Action().Add(meid, "在空间签到获得奖励");
        }
        if (SignKeep > 1)
        {
            builder.Append("<br />您已经连续签到" + SignKeep + "天，累计签到" + SignTotal + "次");
        }
        else
        {
            builder.Append("<br />每天坚持签到将获得意外的惊喜哦！");
        }

        //银行利息更新
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        object BankTime = BCW.User.Users.GetForumSet2(ForumSet, 10);
        if (BankTime != null)
        {
            int iDay = 1;
            if (ub.GetSub("FinanceBankType", "/Controls/finance.xml") == "0")
            {
                iDay = 7;
            }
            DateTime getBankTime = Convert.ToDateTime(BankTime);
            if (DT.TwoDateDiff(DateTime.Now, getBankTime) >= iDay)
            {
                long iBank = new BCW.BLL.User().GetBank(meid);
                double iTar = Convert.ToDouble(ub.GetSub("FinanceBankTar", "/Controls/finance.xml"));
                long intBank = Convert.ToInt64(iBank * (iTar / 1000));
                new BCW.BLL.User().UpdateiBank(meid, intBank);
                string GetForumSet = BCW.User.Users.GetForumSetData(ForumSet, DateTime.Now.ToString(), 10);
                new BCW.BLL.User().UpdateForumSet(meid, GetForumSet);
            }
        }

        //VIP成长更新
        BCW.Model.User vip = new BCW.BLL.User().GetVipData(meid);
        if (vip != null)
        {
            if (string.IsNullOrEmpty(vip.UpdateDayTime.ToString()) || DT.TwoDateDiff(DateTime.Now, vip.UpdateDayTime) >= 1)
            {
                if (vip.VipDate > DateTime.Now)
                {
                    new BCW.BLL.User().UpdateVipGrow(meid, vip.VipDayGrow);
                }
            }
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=1") + "\">排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
