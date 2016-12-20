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
using BCW.Yeepay.Utils;
using BCW.Yeepay.cmbn;

/// <summary>
/// 提交到指定域名充值 黄国军 20161024
/// 系统号不能过币给用户 陈志基 20161006
/// 过币手续费功能 黄国军 20160815
/// 增加提醒字符 黄国军 20160520 
/// 增加中介充值专用链 黄国军 20160526
/// 增加网上支付 环迅 接口 黄国军 20160512/// 
/// 邵广林 接收农场传值并返回 addvip 20160511
/// </summary>
public partial class bbs_finance : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/finance.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("FinanceStatus", xmlPath) == "1")
        {
            Utils.Safe("金融服务");
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Finance, meid);//会员本身权限
        if (new BCW.BLL.User().GetIsFreeze(meid) == 1)
        {
            Utils.Error("你的帐户已被冻结，无法继续...", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");

        #region 判断是否中介充值链
        string key = Utils.GetRequest("key", "all", 1, "", "");
        string keyid = "";
        GetAgenceID(ref act, key, ref keyid);
        #endregion

        switch (act)
        {
            case "addvip":          //VIP成长更新
                AddVipPage(meid);
                break;
            case "viphelp":
                VipHelpPage(meid);
                break;
            case "bank":
                BankPage(meid);
                break;
            case "paytype":
                PayTypePage(meid);
                break;
            case "pay":
                PayPage(meid);
                break;
            case "pay2":
                Pay2Page(meid);
                break;
            case "vippay":              //充值入口
                VipPayPage(meid, keyid);
                break;
            case "paysave":
                PaySavePage(meid);
                break;
            case "ipspaysave":          //网上环迅支付
                IPaySavePage(meid);
                break;
            case "MerBillPay":          //环迅订单支付页面
                MerBillPayPage(meid);
                break;
            case "conlist":
                ConListPage(meid);
                break;
            case "paylist":
                PayListPage(meid);
                break;
            case "exchange":
                ExChangePage(meid);
                break;
            case "exchange2":
                ExChange2Page(meid);
                break;
            case "viplist":             //充值记录
                VipListPage(meid);
                break;
            case "paysafe":
                PaySafePage(meid);
                break;
            case "payactive":
                PayActivePage(meid);
                break;
            case "checkplist":
                CheckPListPage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    #region 判断时候中介链接 GetAgenceID
    /// <summary>
    /// 判断时候中介链接
    /// </summary>
    /// <param name="act"></param>
    /// <param name="key"></param>
    /// <param name="keyid"></param>
    private void GetAgenceID(ref string act, string key, ref string keyid)
    {
        if (key != null && key != "")
        {
            Uri p_Url = Request.UrlReferrer;
            if (p_Url != null)
            {
                builder.Append(p_Url);
            }
            if (p_Url == null || p_Url.ToString() == "")
            {
                Utils.Error("非法提交地址", Utils.getUrl("/default.aspx"));
            }
            else if (!p_Url.ToString().Contains("kb288") && !p_Url.ToString().Contains("localhost") && !p_Url.ToString().Contains("192.168.1.50") && !p_Url.ToString().Contains("kb88"))
            {
                Utils.Error("本站禁止外链", Utils.getUrl("/default.aspx"));
            }

            string ekeys = "";
            string[] keys = { "kb288.com", "kb288.net", "kb288.cc", "wap.kb288.cn", "localhost", "localhost", "192.168.1.50" };
            for (int i = 0; i < keys.Length; i++)
            {
                try
                {
                    ekeys = BCW.Common.DESEncrypt.Decrypt(key, keys[i]);
                    if (ekeys != "")
                    {
                        keyid = ekeys.Split(',')[1].Replace("CNEGA", "");
                        break;
                    }
                }
                catch { }
            }

            if (keyid != "")
            {
                DataSet dssRole = new BCW.BLL.Role().GetList("RoleName", "UsID=" + keyid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 AND ForumID=2 ORDER BY FORUMID ASC");
                if (dssRole.Tables[0].Rows.Count <= 0)
                {
                    Utils.Error("很抱歉,此链接已失效", Utils.getUrl("/default.aspx"));
                }
                else
                {
                    if (act != "ipspaysave")
                    {
                        act = "vippay";
                    }
                }
            }
            else
            {
                Utils.Error("链接存在错误,无法获得付款信息", Utils.getUrl("/default.aspx"));
            }
        }
    }
    #endregion

    #region 金融服务 ReloadPage
    private void ReloadPage(int uid)
    {
        Master.Title = "金融服务";
        string addBzType = string.Empty;
        if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
            addBzType = ub.Get("SiteBz");
        else
            addBzType = ub.Get("SiteBz2");

        builder.Append(Out.Tab("<div class=\"title\">金融服务</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.GetTopDomain().Contains("kb288.net"))
        {
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=bankuser") + "\">银行资料</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + "") + "\">我的VIP</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/chathb.aspx?act=myhb&amp;backurl=" + Utils.PostPage(1) + "") + "\">我的红包</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=bank&amp;backurl=" + Utils.getPage(0) + "") + "\">中央银行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=pay&amp;backurl=" + Utils.getPage(0) + "") + "\">过户" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=pay2&amp;backurl=" + Utils.getPage(0) + "") + "\">过户" + ub.Get("SiteBz2") + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.getPage(0) + "") + "\">充值" + addBzType + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=viplist&amp;backurl=" + Utils.getPage(0) + "") + "\">充值记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=conlist&amp;backurl=" + Utils.getPage(0) + "") + "\">消费记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=paylist&amp;backurl=" + Utils.getPage(0) + "") + "\">过户记录</a><br />");
        if (ub.GetSub("FinanceBzMoveSet", xmlPath) == "1" || ub.GetSub("FinanceBzMoveSet", xmlPath) == "2")
        {
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=exchange&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz2") + "兑换" + ub.Get("SiteBz") + "</a><br />");
        }
        if (ub.GetSub("FinanceBzMoveSet", xmlPath) == "0" || ub.GetSub("FinanceBzMoveSet", xmlPath) == "2")
        {
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=exchange2&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz") + "兑换" + ub.Get("SiteBz2") + "</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">修改支付密码</a>");
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("finance.aspx?act=paysafe&amp;backurl=" + Utils.getPage(0) + "") + "\">设置支付安全</a>");
        }
        builder.Append("<br /><a href=\"" + Utils.getUrl("finance.aspx?act=payactive&amp;backurl=" + Utils.getPage(0) + "") + "\">财产隐藏与显示</a>");
        builder.Append(Out.Tab("</div>", ""));
        if (!Utils.GetDomain().Contains("dyj6"))
        {
            builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
            builder.Append(Out.SysUBB("[红]站方只提供一个虚拟娱乐平台," + ub.Get("SiteBz") + "只能通过[url=/bbs/bbsshop.aspx?act=gift&ptype=5]商城[/url]充值[br]站内不以银行卡等方式出售虚拟酷币,同时不支持回收[br]对论坛个人交易、中介交易不支持也不反对,发生的纠纷也不予处理.[/红]"));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region 我要转币 PayTypePage
    private void PayTypePage(int uid)
    {
        Master.Title = "我要转币";
        string hid = Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择转币类型");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=pay&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">过户" + ub.Get("SiteBz") + "</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=pay2&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">过户" + ub.Get("SiteBz2") + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 设置支付安全 PaySafePage
    private void PaySafePage(int uid)
    {
        Master.Title = "设置支付安全";
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int Times = int.Parse(Utils.GetRequest("Times", "post", 2, @"^[0-9]\d*$", "选择超时时间错误"));
            if (Times < 0 || Times > 1440)
                Utils.Error("选择错误", "");

            //支付安全提示
            string[] p_pageArr = { "act", "info", "Times", "backurl" };
            BCW.User.PaySafe.PaySafePage(uid, Utils.getPageUrl(), p_pageArr, "post", false);

            DateTime dt = DateTime.Now;
            if (Times > 0)
                dt = dt.AddMinutes(Times);

            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 22)
                {
                    sforumsets += "," + sfs[0] + "|" + dt;
                }
                else if (i == 23)
                {
                    sforumsets += "," + sfs[0] + "|" + Times;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(uid, sforumsets);
            Utils.Success("设置支付安全", "设置支付安全成功，正在返回..", Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            string paypwd = new BCW.BLL.User().GetUsPled(uid);
            if (paypwd == "")
            {
                Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
            }
            int iPayTime = BCW.User.Users.GetForumSet(ForumSet, 23);

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("设置支付安全");
            builder.Append(Out.Tab("</div>", ""));
            strText = "支付超时时间:/,,,";
            strName = "Times,info,act,backurl";
            strType = "select,hidden,hidden,hidden";
            strValu = "" + iPayTime + "'ok'paysafe'" + Utils.getPage(0) + "";
            strEmpt = "0|关闭|3|3分钟|10|10分钟|20|20分钟|30|30分钟|60|1小时|120|2小时|720|12小时|1440|24小时,false,false,false";
            strIdea = "/";
            strOthe = "&gt;确定设置,finance.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />在关闭时,在网站内消费都不需要支付密码即可以消费.<br />如设置超时时间，那么，在你消费时每次超过你设置的时间都需要输入支付密码一次才能进行消费.<br />为了你的财产安全，建议你打开此功能.");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    #endregion

    #region 财产隐藏与显示 PayActivePage
    private void PayActivePage(int uid)
    {
        Master.Title = "财产隐藏与显示";
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int IsActive = int.Parse(Utils.GetRequest("IsActive", "post", 2, @"^[0-1]$", "是否显示财产选择错误"));

            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 24)
                {
                    sforumsets += "," + sfs[0] + "|" + IsActive;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(uid, sforumsets);
            Utils.Success("设置财产", "设置财产隐藏与显示成功，正在返回..", Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            int IsActive = BCW.User.Users.GetForumSet(ForumSet, 24);

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("设置财产隐藏与显示");
            builder.Append(Out.Tab("</div>", ""));
            strText = "是否显示财产:/,,,";
            strName = "IsActive,info,act,backurl";
            strType = "select,hidden,hidden,hidden";
            strValu = "" + IsActive + "'ok'payactive'" + Utils.getPage(0) + "";
            strEmpt = "0|显示|1|隐藏,false,false,false";
            strIdea = "/";
            strOthe = "&gt;确定设置,finance.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />此功能可以控制别人查看您的空间时是否显示您的财产，财产包含" + ub.Get("SiteBz") + "与" + ub.Get("SiteBz2") + "");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    #endregion

    #region VIP成长更新 AddVipPage
    private void AddVipPage(int uid)
    {

        //VIP成长更新
        BCW.Model.User vip = new BCW.BLL.User().GetVipData(uid);
        if (vip != null)
        {
            if (string.IsNullOrEmpty(vip.UpdateDayTime.ToString()) || DT.TwoDateDiff(DateTime.Now, vip.UpdateDayTime) >= 1)
            {
                if (vip.VipDate > DateTime.Now)
                {
                    new BCW.BLL.User().UpdateVipGrow(uid, vip.VipDayGrow);
                }
            }
        }

        Master.Title = "我的VIP";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "0"));
        //接收农场传值 邵广林20160510
        int farm = int.Parse(Utils.GetRequest("farm", "all", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">我的VIP</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        long bbNum = 0;
        string bzTypes = string.Empty;
        int i = 0;
        string outType = string.Empty;
        string xmlPath = "/Controls/vip.xml";
        if (ub.GetSub("VipbzType", xmlPath) == "0")
        {
            bzTypes = ub.Get("SiteBz");
            bbNum = new BCW.BLL.User().GetGold(uid);
        }
        else
        {
            bzTypes = ub.Get("SiteBz2");
            bbNum = new BCW.BLL.User().GetMoney(uid);
        }
        BCW.Model.User model = new BCW.BLL.User().GetVipData(uid);
        if (!string.IsNullOrEmpty(model.VipDate.ToString()) && model.VipDate > DateTime.Now)
        {
            int vip8 = Utils.ParseInt(ub.GetSub("VipVGrow8", xmlPath));
            int vip7 = Utils.ParseInt(ub.GetSub("VipVGrow7", xmlPath));
            int vip6 = Utils.ParseInt(ub.GetSub("VipVGrow6", xmlPath));
            int vip5 = Utils.ParseInt(ub.GetSub("VipVGrow5", xmlPath));
            int vip4 = Utils.ParseInt(ub.GetSub("VipVGrow4", xmlPath));
            int vip3 = Utils.ParseInt(ub.GetSub("VipVGrow3", xmlPath));
            int vip2 = Utils.ParseInt(ub.GetSub("VipVGrow2", xmlPath));
            if (model.VipGrow >= vip8)
                i = 8;
            else if (model.VipGrow >= vip7)
                i = 7;
            else if (model.VipGrow >= vip6)
                i = 6;
            else if (model.VipGrow >= vip5)
                i = 5;
            else if (model.VipGrow >= vip4)
                i = 4;
            else if (model.VipGrow >= vip3)
                i = 3;
            else if (model.VipGrow >= vip2)
                i = 2;
            else
                i = 1;

        }
        if (i == 0)
        {
            outType = "购买";
        }
        else
        {
            outType = "续费";
        }
        int VipCent1 = Utils.ParseInt(ub.GetSub("VipCent1", xmlPath));
        int VipCent2 = Utils.ParseInt(ub.GetSub("VipCent2", xmlPath));
        int VipCent3 = Utils.ParseInt(ub.GetSub("VipCent3", xmlPath));
        int VipCent4 = Utils.ParseInt(ub.GetSub("VipCent4", xmlPath));
        int VipGrow1 = Utils.ParseInt(ub.GetSub("VipGrow1", xmlPath));
        int VipGrow2 = Utils.ParseInt(ub.GetSub("VipGrow2", xmlPath));
        int VipGrow3 = Utils.ParseInt(ub.GetSub("VipGrow3", xmlPath));
        int VipGrow4 = Utils.ParseInt(ub.GetSub("VipGrow4", xmlPath));
        if (ptype == 0)
        {
            if (i > 0)
            {
                builder.Append("您是VIP" + i + "级会员(每天" + model.VipDayGrow + "成长点)<br />");
                builder.Append("您当前的成长值:" + model.VipGrow + "点<br />");
                builder.Append("VIP到期:" + model.VipDate + "<br />");

            }

            builder.Append("您共有" + bbNum + "" + bzTypes + "<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">(充值)</a>");
            builder.Append("<br />=选择" + outType + "VIP类型=<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=1&amp;farm=" + farm + "&amp;backurl=" + Utils.getPage(0) + "") + "\">1个月VIP会员需" + VipCent1 + "" + bzTypes + "(" + VipGrow1 + "点成长值)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=2&amp;farm=" + farm + "&amp;backurl=" + Utils.getPage(0) + "") + "\">3个月VIP会员需" + VipCent2 + "" + bzTypes + "(" + VipGrow2 + "点成长值)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=3&amp;farm=" + farm + "&amp;backurl=" + Utils.getPage(0) + "") + "\">6个月VIP会员需" + VipCent3 + "" + bzTypes + "(" + VipGrow3 + "点成长值)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=4&amp;farm=" + farm + "&amp;backurl=" + Utils.getPage(0) + "") + "\">1年VIP会员需" + VipCent4 + "" + bzTypes + "(" + VipGrow4 + "点成长值)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=viphelp&amp;ptype=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">什么是ＶＩＰ？</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=viphelp&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">VIP会员服务条款</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=viphelp&amp;ptype=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看VIP等级制度</a>");
        }
        else
        {
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info != "ok")
            {
                int farm2 = int.Parse(Utils.GetRequest("farm", "all", 1, @"^[0-1]$", "0"));
                builder.Append("您选择" + outType + "");
                if (ptype == 1)
                {
                    builder.Append("一个月VIP会员,需要支付" + VipCent1 + "" + bzTypes + "");
                }
                else if (ptype == 2)
                {
                    builder.Append("3个月VIP会员,需要支付" + VipCent2 + "" + bzTypes + "");
                }
                else if (ptype == 3)
                {
                    builder.Append("6个月VIP会员,需要支付" + VipCent3 + "" + bzTypes + "");
                }
                else if (ptype == 4)
                {
                    builder.Append("1年VIP会员,需要支付" + VipCent4 + "" + bzTypes + "");
                }
                builder.Append("<br />");
                strName = "ptype,act,info,backurl,farm";
                strValu = "" + ptype + "'addvip'ok'" + Utils.getPage(0) + "'" + farm2 + "";
                strOthe = "确定" + outType + ",finance.aspx,post,1,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(" <a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            }
            else
            {
                int farm3 = int.Parse(Utils.GetRequest("farm", "all", 1, @"^[0-1]$", "0"));
                int Cent = 0;
                int Grow = 0;
                int Day = 0;
                if (ptype == 1)
                {
                    Cent = VipCent1;
                    Grow = VipGrow1;
                    Day = 30;
                }
                else if (ptype == 2)
                {
                    Cent = VipCent2;
                    Grow = VipGrow2;
                    Day = 90;
                }
                else if (ptype == 3)
                {
                    Cent = VipCent3;
                    Grow = VipGrow3;
                    Day = 180;
                }
                else if (ptype == 4)
                {
                    Cent = VipCent4;
                    Grow = VipGrow4;
                    Day = 360;
                }
                long payCent = Convert.ToInt64(Cent);
                if (bbNum < payCent)
                {
                    Utils.Error("您的" + bzTypes + "不足" + payCent + "", "");
                }
                if (model.VipDate != null && model.VipDate > DateTime.Now)
                    new BCW.BLL.User().UpdateVipData(uid, Grow, model.VipDate.AddDays(Day));
                else
                    new BCW.BLL.User().UpdateVipData(uid, Grow, DateTime.Now.AddDays(Day));

                //扣币/元
                if (ub.GetSub("VipbzType", xmlPath) == "0")
                {
                    new BCW.BLL.User().UpdateiGold(uid, -payCent, "" + outType + "VIP");
                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(uid, -payCent, "" + outType + "VIP");
                }
                //清缓存
                string CacheKey = CacheName.App_UserVip(uid);
                DataCache.RemoveByPattern(CacheKey);
                if (farm3 == 0)
                    Utils.Success("" + outType + "VIP", "恭喜，" + outType + "VIP成功，正在返回..", Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + ""), "2");
                else
                    Utils.Success("" + outType + "VIP", "恭喜，" + outType + "VIP成功，正在返回..", Utils.getUrl("game/farm.aspx?act=toucai"), "1");
            }

        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region VIP会员服务条款 VipHelpPage
    private void VipHelpPage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string sTitle = string.Empty;
        if (ptype == 1)
            sTitle = "VIP会员服务条款";
        else if (ptype == 2)
            sTitle = "VIP等级制度";
        else
            sTitle = "什么是ＶＩＰ";

        Master.Title = sTitle;
        string xmlPath = "/Controls/vip.xml";
        builder.Append(Out.Tab("<div class=\"title\">" + sTitle + "</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
        {
            builder.Append(ub.GetSub("VipRule", xmlPath));
        }
        else if (ptype == 2)
        {
            int vip8 = Utils.ParseInt(ub.GetSub("VipVGrow8", xmlPath));
            int vip7 = Utils.ParseInt(ub.GetSub("VipVGrow7", xmlPath));
            int vip6 = Utils.ParseInt(ub.GetSub("VipVGrow6", xmlPath));
            int vip5 = Utils.ParseInt(ub.GetSub("VipVGrow5", xmlPath));
            int vip4 = Utils.ParseInt(ub.GetSub("VipVGrow4", xmlPath));
            int vip3 = Utils.ParseInt(ub.GetSub("VipVGrow3", xmlPath));
            int vip2 = Utils.ParseInt(ub.GetSub("VipVGrow2", xmlPath));
            int vip1 = Utils.ParseInt(ub.GetSub("VipVGrow1", xmlPath));
            int VipGrow1 = Utils.ParseInt(ub.GetSub("VipGrow1", xmlPath));
            int VipGrow2 = Utils.ParseInt(ub.GetSub("VipGrow2", xmlPath));
            int VipGrow3 = Utils.ParseInt(ub.GetSub("VipGrow3", xmlPath));
            int VipGrow4 = Utils.ParseInt(ub.GetSub("VipGrow4", xmlPath));
            builder.Append("成长值每天增加相应的点数<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">开通或续费1个月</a>期间每天成长" + VipGrow1 + "点<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">开通或续费3个月</a>期间每天成长" + VipGrow2 + "点<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">开通或续费6个月</a>期间每天成长" + VipGrow3 + "点<br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">开通或续费1年</a>期间每天成长" + VipGrow4 + "点<br />");
            builder.Append("会员成长制度<br />");
            builder.Append("<img src=\"/Files/Medal/vip1.gif\" alt=\"\"/>VIP1=" + vip1 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip2.gif\" alt=\"\"/>VIP2=" + vip2 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip3.gif\" alt=\"\"/>VIP3=" + vip3 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip4.gif\" alt=\"\"/>VIP4=" + vip4 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip5.gif\" alt=\"\"/>VIP5=" + vip5 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip6.gif\" alt=\"\"/>VIP6=" + vip6 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip7.gif\" alt=\"\"/>VIP7=" + vip7 + "成长值<br />");
            builder.Append("<img src=\"/Files/Medal/vip8.gif\" alt=\"\"/>VIP8=" + vip8 + "成长值");
        }
        else
        {
            builder.Append("什么是VIP?<br />");
            builder.Append("VIP是本网为会员开发的一类特殊道具,可以享有VIP尊贵的身份和特权.<br />");
            builder.Append("VIP在哪里可以购买?<br />");
            builder.Append("VIP在空间主页和在金融都可以购买.<br />");
            builder.Append("VIP有什么特权?<br />");
            builder.Append(Out.SysUBB(@"1.在[url=/bbs/bbsshop.aspx]商城[/url]购物打8折.<br />2.隐身登录尊重隐私<br />在空间设置[url=/bbs/myedit.aspx?act=state]更改我的状态[/url]即可隐身<br />3.可在设置过性别后再次[url=/bbs/myedit.aspx?act=basic]修改[/url].<br />4." + ub.Get("SiteBz") + "[url=/bbs/finance.aspx]过户[/url]不限额、不收手续费.<br />5.可同时加入20个[url=/bbs/group.aspx]圈子[/url],非vip只能加入5个.<br />6.站内某些页面拥有vip才能进入."));

        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 2)
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=viphelp&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">VIP会员服务条款</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=viphelp&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">查看VIP等级制度</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 银行利息更新 BankPage
    private void BankPage(int uid)
    {
        //银行利息更新
        string bForumSet = new BCW.BLL.User().GetForumSet(uid);
        object BankTime = BCW.User.Users.GetForumSet2(bForumSet, 10);
        if (BankTime != null)
        {
            int iDay = 1;
            if (ub.GetSub("FinanceBankType", xmlPath) == "0")
            {
                iDay = 7;
            }
            DateTime getBankTime = Convert.ToDateTime(BankTime);
            if (DT.TwoDateDiff(DateTime.Now, getBankTime) >= iDay)
            {
                long iBank = new BCW.BLL.User().GetBank(uid);
                double iTar = Convert.ToDouble(ub.GetSub("FinanceBankTar", xmlPath));
                long intBank = Convert.ToInt64(iBank * (iTar / 1000));
                new BCW.BLL.User().UpdateiBank(uid, intBank);
                string GetForumSet = BCW.User.Users.GetForumSetData(bForumSet, DateTime.Now.ToString(), 10);
                new BCW.BLL.User().UpdateForumSet(uid, GetForumSet);
            }
        }

        Master.Title = "中央银行";
        long iGold = new BCW.BLL.User().GetGold(uid);
        string info = Utils.GetRequest("info", "all", 1, "", "");

        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        if (ac == "确定存入")
            info = "in";
        else if (ac == "确定取出")
            info = "out";

        long payCent = 0;
        if (info == "in" || info == "out")
        {
            string meverify = new BCW.BLL.User().GetVerifys(uid);
            string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
            string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
            if (!string.IsNullOrEmpty(meverify))
            {
                if (verify.Equals(meverify))
                {
                    Utils.Error("验证码填写错误", "");
                }
            }
            //更新验证码
            new BCW.BLL.User().UpdateVerifys(uid, verify);
            if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
            {
                Utils.Error("验证码填写错误", "");
            }
            long payBig = Convert.ToInt64(ub.GetSub("FinancePaybig", xmlPath));

            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "支付密码填写错误");
            payCent = Convert.ToInt64(Utils.GetRequest("inCent", "post", 2, @"^[0-9]\d*$", "数额填写错误"));
            if (payCent == 0)
            {
                Utils.Error("数额填写错误", "");
            }
            if (payCent > payBig)
            {
                Utils.Error("每次最多存取" + payBig + "" + ub.Get("SiteBz") + "", "");
            }
            string paypwd = new BCW.BLL.User().GetUsPled(uid);
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }

            //检测上一个取存款是否一样
            DataSet ds = new BCW.BLL.Goldlog().GetList("TOP 1 PUrl,AcGold,AddTime", "UsId=" + uid + " ORDER BY ID DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string PUrl = ds.Tables[0].Rows[0]["PUrl"].ToString();
                long AcGold = Int64.Parse(ds.Tables[0].Rows[0]["AcGold"].ToString());
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());

                if (PUrl == Utils.getPageUrl() && Math.Abs(AcGold) == payCent)
                {
                    if (AddTime > DateTime.Now.AddSeconds(-5))
                    {
                        new BCW.BLL.Guest().Add(10086, "管理员", "ID:" + uid + "在银行有刷币嫌疑，请进后台查询消费日志并处理。");
                    }
                }
            }

            //是否刷屏
            string appName = "LIGHT_BANK";
            int Expir = 60;
            BCW.User.Users.IsFresh(appName, Expir);
        }
        if (info == "ok")
        {
            long khCent = Convert.ToInt64(ub.GetSub("FinanceBankKh", xmlPath));
            if (khCent > iGold)
            {
                Utils.Error("你自带的" + ub.Get("SiteBz") + "不足", "");
            }
            string ForumSet = new BCW.BLL.User().GetForumSet(uid);
            string GetForumSet = BCW.User.Users.GetForumSetData(ForumSet, "1", 9);
            new BCW.BLL.User().UpdateForumSet(uid, GetForumSet);
            new BCW.BLL.User().UpdateiGold(uid, -khCent, "银行开户");
            Utils.Success("银行开户", "银行开户成功，正在返回..", Utils.getUrl("finance.aspx?act=bank&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else if (info == "in")
        {
            if (payCent > iGold)
            {
                Utils.Error("你自带的" + ub.Get("SiteBz") + "不足", "");
            }
            new BCW.BLL.User().UpdateiBank(uid, payCent);
            long vBank = new BCW.BLL.User().GetBank(uid);

            new BCW.BLL.User().UpdateiGold(uid, -payCent, "存入银行，银行当前余额" + vBank + "");

            Utils.Success("存款", "存款" + payCent + "" + ub.Get("SiteBz") + "成功，正在返回..", Utils.getUrl("finance.aspx?act=bank&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else if (info == "out")
        {
            long iBank = new BCW.BLL.User().GetBank(uid);
            if (payCent > iBank)
            {
                Utils.Error("你存款" + ub.Get("SiteBz") + "不足", "");
            }
            new BCW.BLL.User().UpdateiBank(uid, -payCent);
            long vBank = new BCW.BLL.User().GetBank(uid);

            new BCW.BLL.User().UpdateiGold(uid, payCent, "从银行取出，银行当前余额" + vBank + "");

            Utils.Success("取款", "取款" + payCent + "" + ub.Get("SiteBz") + "成功，正在返回..", Utils.getUrl("finance.aspx?act=bank&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
        else
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(uid);
            int FsIsBank = BCW.User.Users.GetForumSet(ForumSet, 9);
            if (FsIsBank == 0)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("您目前尚没有银行账户，请先开户，开户费" + ub.GetSub("FinanceBankKh", xmlPath) + "" + ub.Get("SiteBz") + "：");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=bank&amp;info=ok&amp;backurl=" + Utils.getPage(0) + "") + "\">确定开户</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;中央银行");
                builder.Append(Out.Tab("</div>", "<br />"));

                string paypwd = new BCW.BLL.User().GetUsPled(uid);
                if (paypwd == "")
                {
                    Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
                }
                builder.Append(Out.Tab("<div>", ""));
                long iBank = new BCW.BLL.User().GetBank(uid);
                string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
                builder.Append("存款:" + iBank + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("自带:" + iGold + "" + ub.Get("SiteBz") + "");
                builder.Append(Out.Tab("</div>", ""));
                strText = "输入" + ub.Get("SiteBz") + ":/,支付密码:/,输入验证码:/,,,";
                strName = "inCent,Pwd,verify,verifyKey,info,act";
                strType = "num,num,snum,hidden,hidden,hidden";
                strValu = "'''" + verifyKey + "'no'bank";
                strEmpt = "false,false,false,false,false,false";
                strIdea = "''\n<a href=\"" + Utils.getUrl("finance.aspx?act=bank&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''|/";
                strOthe = "确定存入|确定取出,finance.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                if (ub.GetSub("FinanceBankType", xmlPath) == "0")
                    builder.Append("*银行利息按周结算<br />");
                else
                    builder.Append("*银行利息按日结算<br />");

                builder.Append("*当前利率:" + ub.GetSub("FinanceBankTar", xmlPath) + "‰<br />");
                builder.Append("*存取" + ub.Get("SiteBz") + "最多" + ub.GetSub("FinancePaybig", xmlPath) + "/次");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
    }
    #endregion

    #region 过户 PayPage
    private void PayPage(int uid)
    {
        long MaxCent = 0;
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            MaxCent = 5000000;
        else
            MaxCent = 5000000;

        BCW.User.Users.ShowVerifyRole("d", uid);//非验证会员提示
        Master.Title = "过户" + ub.Get("SiteBz") + "";
        string paypwd = new BCW.BLL.User().GetUsPled(uid);
        string PayTar = ub.GetSub("FinancePayTar", xmlPath);
        if (PayTar == "")
        {
            Utils.Error("系统不开放" + ub.Get("SiteBz") + "过户", "");
        }
        //内部ID过户软禁
        //string SysID2 = ub.GetSub("FinanceSysID2", xmlPath);
        //SysID2 += "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");

        //if (("#" + SysID2 + "#").Contains("#" + uid + "#"))
        //{
        //   Utils.Error("过户权限不足，请联系客服！", "");
        // }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string verify = "";
        string verifyKey = "";
        int vipLeven = BCW.User.Users.VipLeven(uid);

        #region 过币
        if (info == "ok" || info == "ok2")
        {
            #region 过币判断
            int toid = int.Parse(Utils.GetRequest("toid", "post", 2, @"^[0-9]\d*$", "过户ID填写错误"));
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,50}$", "附言限50字内，可留空");
            if (Content == "")
                Content = "无";
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "支付密码填写错误");

            if (!new BCW.BLL.User().Exists(toid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (toid == uid)
            {
                Utils.Error("不能对自己进行过户", "");
            }
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("kb288.net"))
            {
                verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
                verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
            }
            //对方是否被禁了金融系统
            bool isAllowAccess = new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Finance, toid);
            if (isAllowAccess)
            {
                Utils.Error("对方已被禁止使用金融功能，不能对其进行过户", "");
            }
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[0-9]\d*$", "数额填写错误"));

            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th") && !Utils.GetTopDomain().Contains("kb288.net"))
            {
                if (vipLeven == 0)
                {
                    if (payCent > MaxCent)
                    {
                        Utils.Error("非VIP会员过户不能大于" + MaxCent + "" + ub.Get("SiteBz") + "<br /><a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;马上开通VIP</a>", "");
                    }

                }
            }

            //得到当天过币次数
            int countss = new BCW.BLL.Transfer().GetCount("FromId=" + uid + " and datediff(day,AddTime,getdate())=0");

            long Cm = 0;        //手续费
            bool dRole = false; //是否中介
            #region 判断中介权限
            DataSet dssRole = new BCW.BLL.Role().GetList("RoleName", "UsID=" + uid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 AND ForumID=2 ORDER BY FORUMID ASC");
            if (dssRole.Tables.Count > 0)
            {
                if (dssRole.Tables[0].Rows.Count > 0)
                {
                    dRole = true;
                }
            }
            #endregion

            #region 判断是否需要收取手续费
            if (!dRole)
            {
                int SiteCm = int.Parse(ub.Get("SiteCm"));
                if (vipLeven == 0)
                {
                    if ((3 - countss) <= 0)
                    {
                        if (ub.Get("SiteCmOnf") == "1")
                        {
                            if (!("#" + ub.Get("SiteIds") + "#").Contains("#" + uid + "#"))
                            {
                                Cm = long.Parse((0.001 * SiteCm * payCent).ToString("0"));
                                if (Cm <= 0) { Cm = 1; }
                            }
                        }
                    }
                }
                else
                {
                    if ((5 - countss) <= 0)
                    {
                        if (ub.Get("SiteCmOnf") == "1")
                        {
                            if (!("#" + ub.Get("SiteIds") + "#").Contains("#" + uid + "#"))
                            {
                                Cm = long.Parse((0.001 * SiteCm * payCent).ToString("0"));
                                if (Cm <= 0) { Cm = 1; }
                            }
                        }
                    }
                }
            }

            if (Utils.GetDomain().Contains("dyj6"))
            {
                if (uid == 666888)
                {
                    Cm = 0;
                }
            }
            #endregion

            // 加上手续费
            // payCent = payCent + Cm;
            long iGold = new BCW.BLL.User().GetGold(uid);
            if (payCent + Cm > iGold)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }
            //内部ID只允许互相过户，对外限20000币
            int Types = 0;
            //  if (!Utils.GetTopDomain().Contains("kb288.net"))
            {
                string SysID = ub.GetSub("FinanceSysID", xmlPath);
                string xmlPath2 = "/Controls/xiaohao.xml";
                SysID += "#" + ub.GetSub("XiaoHao", xmlPath2);
                if (("#" + SysID + "#").Contains("#" + uid + "#"))
                {
                    Types = 3;//内部互相转币标识
                    if (!("#" + SysID + "#").Contains("#" + toid + "#"))
                    {
                        // Utils.Error("过户权限不足，请联系客服！", "");
                        //统计该ID今天过户了多少币
                        long AcCents = new BCW.BLL.Transfer().GetAcCents(uid, 4);
                        //  if (AcCents + payCent > 20000)
                        if (payCent > 20000)
                        {
                            Utils.Error("过户权限不足，请联系客服！", "");
                        }
                        Types = 4;//内部对外转币标识
                    }
                }
            }
            #endregion

            if (info == "ok")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;过户" + ub.Get("SiteBz") + "");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("您自带" + new BCW.BLL.User().GetGold(uid) + "" + ub.Get("SiteBz") + "<br />");
                if (payCent > 1000000)
                {
                    int odds = Convert.ToInt32(payCent / 1000000 * 100);
                    builder.Append("<b>注意:过户币已超过" + odds + "万</b><br />");
                }
                builder.Append("您要将" + Utils.ConvertGold(payCent) + "" + ub.Get("SiteBz") + "转帐给<b><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + toid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(toid) + "(" + toid + ")</a></b>");
                builder.Append(Out.Tab("</div>", ""));


                if (!Utils.GetTopDomain().Contains("kb288.net"))
                    strText = "需要附言吗?(限50字内，附言将花费1" + ub.Get("SiteBz") + "，VIP免费):/,,,,,,,";
                else
                    strText = "需要附言吗?(限50字内):/,,,,,,,";

                strName = "Content,toid,payCent,Pwd,verify,verifyKey,info,act";
                strType = "textarea,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                strValu = "'" + toid + "'" + payCent + "'" + Pwd + "'" + verify + "'" + verifyKey + "'ok2'pay";
                strEmpt = "true,false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "&gt;确定过户," + Utils.getUrl("finance.aspx") + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                if (!dRole)
                {
                    if (ub.Get("SiteCmOnf") == "1")
                    {
                        if (!("#" + ub.Get("SiteIds") + "#").Contains("#" + uid + "#"))
                        {
                            if (vipLeven == 0)
                            {

                                if ((3 - countss) >= 0)
                                {
                                    builder.Append("您今天 剩下" + (3 - countss).ToString() + "次免费过币<br />");
                                }
                                else
                                {
                                    builder.Append(Out.SysUBB("您今天免费过币次数已用完,本次过币扣除手续费[红]" + Utils.ConvertGold(Cm) + "[/红]" + ub.Get("SiteBz") + "<br />"));
                                }
                            }
                            else
                            {
                                if ((5 - countss) >= 0)
                                {
                                    builder.Append("您今天剩下" + (5 - countss).ToString() + "次免费过币<br />");
                                }
                                else
                                {
                                    builder.Append(Out.SysUBB("您今天免费过币次数已用完,本次过币扣除手续费[红]" + Utils.ConvertGold(Cm) + "[/红]" + ub.Get("SiteBz") + "<br />"));
                                }
                            }
                        }
                    }
                }
                else
                {
                    builder.Append(Out.SysUBB("[B]中介账号过币免除手续费[/B]<br />"));
                }
                //builder.Append("每次过户手续费1" + ub.Get("SiteBz") + "<br />");
                builder.Append("为了保护你的个人虚拟财产,请设置<a href=\"" + Utils.getUrl("finance.aspx?act=paysafe&amp;backurl=" + Utils.getPage(0) + "") + "\">支付安全</a>,以保障你的利益！");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("kb288.net"))
                {
                    string meverify = new BCW.BLL.User().GetVerifys(uid);
                    if (!string.IsNullOrEmpty(meverify))
                    {
                        if (verify.Equals(meverify))
                        {
                            Utils.Error("验证码填写错误", "");
                        }
                    }
                    //更新验证码
                    new BCW.BLL.User().UpdateVerifys(uid, verify);
                    if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
                    {
                        Utils.Error("验证码填写错误", "");
                    }
                }

                //是否刷屏
                string appName = "LIGHT_FINANCEPAY";
                int Expir = Convert.ToInt32(ub.GetSub("FinancePayExpir", xmlPath));
                BCW.User.Users.IsFresh(appName, Expir);

                //计算手续费
                long SpayCent = 0;
                //if (vipLeven == 0 && !Utils.GetTopDomain().Contains("kb288.net"))
                //{
                //    SpayCent = payCent + 1;
                //    if (Content != "无")
                //        SpayCent = SpayCent + 1;

                //}
                //else
                //{
                SpayCent = payCent + Cm;
                //}

                if (SpayCent > iGold)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                new BCW.BLL.User().UpdateiGold(uid, -SpayCent, "过户给ID" + toid + "");
                new BCW.BLL.User().UpdateiGold(toid, payCent, "来自ID" + uid + "过户");

                string fromName = new BCW.BLL.User().GetUsName(uid);
                string toName = new BCW.BLL.User().GetUsName(toid);
                BCW.Model.Transfer model = new BCW.Model.Transfer();
                model.Types = Types;
                model.FromId = uid;
                model.FromName = fromName;
                model.ToId = toid;
                model.ToName = toName;
                model.AcCent = payCent;
                model.AddTime = DateTime.Now;
                new BCW.BLL.Transfer().Add(model);
                //消息
                new BCW.BLL.Guest().Add(toid, toName, "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + fromName + "[/url]过户" + payCent + "" + ub.Get("SiteBz") + "给您，附言:" + Content + "[br][url=/bbs/finance.aspx?act=paylist]查看过户记录&gt;&gt;[/url]");
                Utils.Success("过户" + ub.Get("SiteBz") + "", "过户" + payCent + "" + ub.Get("SiteBz") + "至ID" + toid + "成功，系统收取" + (Cm) + "" + ub.Get("SiteBz") + "手续费，正在返回..", Utils.getUrl("finance.aspx?act=pay&amp;backurl=" + Utils.getPage(0) + ""), "3");
            }
        }
        else
        {
            if (paypwd == "")
            {
                Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;过户" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("您自带" + new BCW.BLL.User().GetGold(uid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            string toid = Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "");

            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("kb288.net"))
            {
                verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
                strText = "对方ID:/,过户多少" + ub.Get("SiteBz") + ":/,支付密码:/,输入验证码:/,,,,";
                strName = "toid,payCent,Pwd,verify,verifyKey,info,act";
                strType = "num,num,text,text,hidden,hidden,hidden";
                strValu = "" + toid + "''''" + verifyKey + "'ok'pay";
                strEmpt = "false,false,false,false,false,false,false";
                strIdea = "<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Utils.PostPage(1) + "") + "\">从好友选择<／a>'''\n<a href=\"" + Utils.getUrl("finance.aspx?act=pay&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''|/";
                strOthe = "&gt;确定继续,finance.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            }
            else
            {
                strText = "对方ID:/,过户多少" + ub.Get("SiteBz") + ":/,支付密码:/,,";
                strName = "toid,payCent,Pwd,info,act";
                strType = "num,num,text,hidden,hidden";
                strValu = "" + toid + "'''ok'pay";
                strEmpt = "false,false,false,false,false";
                strIdea = "/";
                strOthe = "&gt;确定继续,finance.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th") && !Utils.GetTopDomain().Contains("kb288.net"))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("每次过户最多" + MaxCent + "" + ub.Get("SiteBz") + ",<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + "") + "\">VIP</a>过户无限制！");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
    }
    #endregion

    #region 过户2 Pay2Page
    private void Pay2Page(int uid)
    {

        long MaxCent = 0;
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            MaxCent = 5000000;
        else
            MaxCent = 1000000;

        BCW.User.Users.ShowVerifyRole("d", uid);//非验证会员提示
        Master.Title = "过户" + ub.Get("SiteBz2") + "";
        string paypwd = new BCW.BLL.User().GetUsPled(uid);
        string PayTar = ub.GetSub("FinancePayTar", xmlPath);
        if (PayTar == "")
        {
            Utils.Error("系统不开放" + ub.Get("SiteBz2") + "过户", "");
        }
        //内部ID过户软禁
        //string SysID2 = ub.GetSub("FinanceSysID2", xmlPath);
        // SysID2 += "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");

        //if (("#" + SysID2 + "#").Contains("#" + uid + "#"))
        //{
        //Utils.Error("过户权限不足，请联系客服！", "");
        // }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string verify = "";
        string verifyKey = "";
        int vipLeven = BCW.User.Users.VipLeven(uid);
        if (info == "ok" || info == "ok2")
        {
            int toid = int.Parse(Utils.GetRequest("toid", "post", 2, @"^[0-9]\d*$", "过户ID填写错误"));
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,50}$", "附言限50字内，可留空");
            if (Content == "")
                Content = "无";
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "支付密码填写错误");

            if (!new BCW.BLL.User().Exists(toid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (toid == uid)
            {
                Utils.Error("不能对自己进行过户", "");
            }
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("kb288.net"))
            {
                verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
                verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
            }
            //对方是否被禁了金融系统
            bool isAllowAccess = new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Finance, toid);
            if (isAllowAccess)
            {
                Utils.Error("对方已被禁止使用金融功能，不能对其进行过户", "");
            }
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[0-9]\d*$", "数额填写错误"));
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th") && !Utils.GetTopDomain().Contains("kb288.net"))
            {
                if (vipLeven == 0)
                {
                    if (payCent > MaxCent)
                    {
                        Utils.Error("非VIP会员过户不能大于" + MaxCent + "" + ub.Get("SiteBz2") + "<br /><a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;马上开通VIP</a>", "");
                    }

                }
            }
            long iMoney = new BCW.BLL.User().GetMoney(uid);
            if (payCent > iMoney)
            {
                Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
            }
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }
            int Types = 1;
            if (!Utils.GetTopDomain().Contains("kb288.net"))
            {
                //内部ID只允许互相过户，对外限20000币

                string SysID = ub.GetSub("FinanceSysID", xmlPath);
                string xmlPath2 = "/Controls/xiaohao.xml";
                SysID += "#" + ub.GetSub("XiaoHao", xmlPath2);
                if (("#" + SysID + "#").Contains("#" + uid + "#"))
                {
                    Types = 3;//内部互相转币标识
                    if (!("#" + SysID + "#").Contains("#" + toid + "#"))
                    {
                        //统计该ID今天过户了多少币
                        long AcCents = new BCW.BLL.Transfer().GetAcCents(uid, 4);
                        if (AcCents + payCent > 20000)
                        {
                            Utils.Error("过户权限不足，请联系客服！", "");
                        }
                        Types = 4;//内部对外转币标识
                    }

                }

            }
            if (info == "ok")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;过户" + ub.Get("SiteBz2") + "");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("您自带" + new BCW.BLL.User().GetMoney(uid) + "" + ub.Get("SiteBz2") + "<br />");
                if (payCent > 1000000)
                {
                    int odds = Convert.ToInt32(payCent / 1000000 * 100);
                    builder.Append("<b>注意:过户币已超过" + odds + "万</b><br />");
                }
                builder.Append("您要将" + Utils.ConvertGold(payCent) + "" + ub.Get("SiteBz2") + "转帐给<b><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + toid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(toid) + "(" + toid + ")</a></b>");
                builder.Append(Out.Tab("</div>", ""));

                if (!Utils.GetTopDomain().Contains("kb288.net"))
                    strText = "需要附言吗?(限50字内，附言将花费1" + ub.Get("SiteBz") + "，VIP免费):/,,,,,,,";
                else
                    strText = "需要附言吗?(限50字内):/,,,,,,,";

                strName = "Content,toid,payCent,Pwd,verify,verifyKey,info,act";
                strType = "textarea,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                strValu = "'" + toid + "'" + payCent + "'" + Pwd + "'" + verify + "'" + verifyKey + "'ok2'pay2";
                strEmpt = "true,false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "&gt;确定过户,finance.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                if (!Utils.GetTopDomain().Contains("kb288.net"))
                {
                    builder.Append("每次过户手续费1" + ub.Get("SiteBz2") + "<br />");
                }
                builder.Append("为了保护你的个人虚拟财产,请设置<a href=\"" + Utils.getUrl("finance.aspx?act=paysafe&amp;backurl=" + Utils.getPage(0) + "") + "\">支付安全</a>,以保障你的利益！");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("kb288.net"))
                {
                    string meverify = new BCW.BLL.User().GetVerifys(uid);
                    if (!string.IsNullOrEmpty(meverify))
                    {
                        if (verify.Equals(meverify))
                        {
                            Utils.Error("验证码填写错误", "");
                        }
                    }
                    //更新验证码
                    new BCW.BLL.User().UpdateVerifys(uid, verify);
                    if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
                    {
                        Utils.Error("验证码填写错误", "");
                    }
                }
                //是否刷屏
                string appName = "LIGHT_FINANCEPAY";
                int Expir = Convert.ToInt32(ub.GetSub("FinancePayExpir", xmlPath));
                BCW.User.Users.IsFresh(appName, Expir);

                //计算手续费，VIP免费
                long SpayCent = 0;
                if (vipLeven == 0 && !Utils.GetTopDomain().Contains("kb288.net"))
                {
                    SpayCent = payCent + 1;
                    if (Content != "无")
                        SpayCent = SpayCent + 1;

                }
                else
                {
                    SpayCent = payCent;
                }

                if (SpayCent > iMoney)
                {
                    Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
                }
                new BCW.BLL.User().UpdateiMoney(uid, -SpayCent, "过户给ID" + toid + "");
                new BCW.BLL.User().UpdateiMoney(toid, payCent, "来自ID" + uid + "过户");

                string fromName = new BCW.BLL.User().GetUsName(uid);
                string toName = new BCW.BLL.User().GetUsName(toid);
                BCW.Model.Transfer model = new BCW.Model.Transfer();
                model.Types = Types;
                model.FromId = uid;
                model.FromName = fromName;
                model.ToId = toid;
                model.ToName = toName;
                model.AcCent = payCent;
                model.AddTime = DateTime.Now;
                new BCW.BLL.Transfer().Add(model);
                //消息
                new BCW.BLL.Guest().Add(toid, toName, "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + fromName + "[/url]过户" + payCent + "" + ub.Get("SiteBz2") + "给您，附言:" + Content + "[br][url=/bbs/finance.aspx?act=paylist]查看过户记录&gt;&gt;[/url]");
                Utils.Success("过户" + ub.Get("SiteBz2") + "", "过户" + payCent + "" + ub.Get("SiteBz2") + "至ID" + toid + "成功，系统收取" + (SpayCent - payCent) + "" + ub.Get("SiteBz2") + "手续费，正在返回..", Utils.getUrl("finance.aspx?act=pay2&amp;backurl=" + Utils.getPage(0) + ""), "3");
            }
        }
        else
        {
            if (paypwd == "")
            {
                Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;过户" + ub.Get("SiteBz2") + "");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("您自带" + new BCW.BLL.User().GetMoney(uid) + "" + ub.Get("SiteBz2") + "");
            builder.Append(Out.Tab("</div>", ""));
            string toid = Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "");
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("kb288.net"))
            {
                verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
                strText = "对方ID:/,过户多少" + ub.Get("SiteBz2") + ":/,支付密码:/,输入验证码:/,,,,";
                strName = "toid,payCent,Pwd,verify,verifyKey,info,act";
                strType = "num,num,text,text,hidden,hidden,hidden";
                strValu = "" + toid + "''''" + verifyKey + "'ok'pay2";
                strEmpt = "false,false,false,false,false,false,false";
                strIdea = "<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Utils.PostPage(1) + "") + "\">从好友选择<／a>'''\n<a href=\"" + Utils.getUrl("finance.aspx?act=pay2&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''|/";
                strOthe = "&gt;确定继续,finance.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                strText = "对方ID:/,过户多少" + ub.Get("SiteBz2") + ":/,支付密码:/,,";
                strName = "toid,payCent,Pwd,info,act";
                strType = "num,num,text,hidden,hidden";
                strValu = "" + toid + "'''ok'pay2";
                strEmpt = "false,false,false,false,false";
                strIdea = "/";
                strOthe = "&gt;确定继续,finance.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th") && !Utils.GetTopDomain().Contains("kb288.net"))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("每次过户最多" + MaxCent + "" + ub.Get("SiteBz2") + ",<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.getPage(0) + "") + "\">VIP</a>过户无限制！");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    #endregion

    #region 消费记录 ConListPage
    private void ConListPage(int uid)
    {
        Master.Title = "金融服务";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;消费记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=conlist&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz") + "记录</a>&gt;" + ub.Get("SiteBz2") + "记录");
        else
            builder.Append("" + ub.Get("SiteBz") + "记录&gt;<a href=\"" + Utils.getUrl("finance.aspx?act=conlist&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz2") + "记录</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=" + ptype + " and BbTag<=1";

        // 开始读取列表
        IList<BCW.Model.Goldlog> listGoldlog = new BCW.BLL.Goldlog().GetGoldlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listGoldlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listGoldlog)
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
                builder.AppendFormat("{0}.{1}|操作{2}|结{3}({4})", (pageIndex - 1) * pageSize + k, Out.SysUBB(n.AcText), n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 1));

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 过户记录 PayListPage
    private void PayListPage(int uid)
    {
        Master.Title = "金融服务";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9\d*]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;过户记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("转入记录/");
        else
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=paylist&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">转入</a>/");

        if (ptype == 2)
            builder.Append("转出记录");
        else
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=paylist&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">转出</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1)
            strWhere = "ToId=" + uid + "";
        else
            strWhere = "FromId=" + uid + "";

        // 开始读取列表
        IList<BCW.Model.Transfer> listTransfer = new BCW.BLL.Transfer().GetTransfers(pageIndex, pageSize, strWhere, out recordCount);
        if (listTransfer.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Transfer n in listTransfer)
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
                if (n.Types == 1)
                    bzType = ub.Get("SiteBz2");
                else
                    bzType = ub.Get("SiteBz");

                if (ptype == 1)
                    builder.AppendFormat("{0}.{1}(" + n.FromId + ")转入{2}" + bzType + "({3})", (pageIndex - 1) * pageSize + k, n.FromName, n.AcCent, DT.FormatDate(n.AddTime, 1));
                else
                    builder.AppendFormat("{0}.向{1}(" + n.ToId + ")转出{2}" + bzType + "({3})", (pageIndex - 1) * pageSize + k, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 1));
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 兑换 ExChangePage
    private void ExChangePage(int uid)
    {
        if (ub.GetSub("FinanceBzMoveSet", xmlPath) != "1" && ub.GetSub("FinanceBzMoveSet", xmlPath) != "2")
        {
            Utils.Error("" + ub.Get("SiteBz2") + "兑换" + ub.Get("SiteBz") + "已关闭", "");
        }
        Master.Title = "" + ub.Get("SiteBz2") + "兑换" + ub.Get("SiteBz") + "";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string Tar = ub.GetSub("FinanceBzTar", xmlPath);
        string Tar2 = ub.GetSub("FinanceBzTar2", xmlPath);
        if (info == "ok")
        {
            string meverify = new BCW.BLL.User().GetVerifys(uid);
            string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
            string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
            if (!string.IsNullOrEmpty(meverify))
            {
                if (verify.Equals(meverify))
                {
                    Utils.Error("验证码填写错误", "");
                }
            }
            //更新验证码
            new BCW.BLL.User().UpdateVerifys(uid, verify);
            if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
            {
                Utils.Error("验证码填写错误", "");
            }

            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "数额填写错误"));
            long iMoney = new BCW.BLL.User().GetMoney(uid);
            if (payCent > iMoney)
            {
                Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
            }
            if (payCent < Convert.ToInt64(Tar2))
            {
                Utils.Error("兑换" + ub.Get("SiteBz2") + "额必须是" + Tar2 + "的倍数", "");
            }
            if (payCent % Convert.ToInt64(Tar2) != 0)
            {
                Utils.Error("兑换" + ub.Get("SiteBz2") + "额必须是" + Tar2 + "的倍数", "");
            }
            long iTar = Convert.ToInt64(Utils.ParseInt(Tar2));
            long iTar2 = Convert.ToInt64(Utils.ParseInt(Tar));
            long LostGold = 0;
            if (iTar == 1)
                LostGold = Convert.ToInt64(payCent * iTar2);
            else
                LostGold = Convert.ToInt64(payCent / iTar);

            //检测上一个是否一样
            DataSet ds = new BCW.BLL.Goldlog().GetList("TOP 1 PUrl,AcGold,AddTime", "UsId=" + uid + " and Types=0 ORDER BY ID DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string PUrl = ds.Tables[0].Rows[0]["PUrl"].ToString();
                long AcGold = Int64.Parse(ds.Tables[0].Rows[0]["AcGold"].ToString());
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());

                if (PUrl == Utils.getPageUrl() && Math.Abs(AcGold) == LostGold)
                {
                    if (AddTime > DateTime.Now.AddSeconds(-5))
                    {
                        new BCW.BLL.Guest().Add(10086, "管理员", "ID:" + uid + "在兑换" + ub.Get("SiteBz") + "嫌疑，请进后台查询消费日志并处理。");
                    }
                }
            }
            //是否刷屏
            string appName = "LIGHT_ExChange";
            int Expir = 60;
            BCW.User.Users.IsFresh(appName, Expir);

            new BCW.BLL.User().UpdateiMoney(uid, -payCent, "兑换了" + LostGold + "" + ub.Get("SiteBz") + "");
            new BCW.BLL.User().UpdateiGold(uid, LostGold, "花费" + payCent + "" + ub.Get("SiteBz2") + "兑换");

            Utils.Success("兑换" + ub.Get("SiteBz") + "", "兑换" + LostGold + "" + ub.Get("SiteBz") + "成功，花费了" + payCent + "" + ub.Get("SiteBz2") + "，正在返回..", Utils.getUrl("finance.aspx?act=exchange&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;兑换" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
            strText = "支出多少" + ub.Get("SiteBz2") + "：/,输入验证码:/,,,,";
            strName = "payCent,verify,verifyKey,info,act";
            strType = "num,text,hidden,hidden,hidden";
            strValu = "''" + verifyKey + "'ok'exchange";
            strEmpt = "false,true,false,false,false";
            strIdea = "'\n<a href=\"" + Utils.getUrl("finance.aspx?act=exchange&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''''|/";
            strOthe = "&gt;兑换" + ub.Get("SiteBz") + ",finance.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            long iGold = new BCW.BLL.User().GetGold(uid);
            long iMoney = new BCW.BLL.User().GetMoney(uid);
            builder.Append("当前汇率:<br />" + ub.Get("SiteBz2") + "兑换" + ub.Get("SiteBz") + "" + Tar2 + ":" + Tar + "");
            builder.Append("<br />自带" + iMoney + "" + ub.Get("SiteBz2") + "," + iGold + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 兑换2 ExChange2Page
    private void ExChange2Page(int uid)
    {
        if (ub.GetSub("FinanceBzMoveSet", xmlPath) != "0" && ub.GetSub("FinanceBzMoveSet", xmlPath) != "2")
        {
            Utils.Error("" + ub.Get("SiteBz") + "兑换" + ub.Get("SiteBz2") + "已关闭", "");
        }
        Master.Title = "" + ub.Get("SiteBz") + "兑换" + ub.Get("SiteBz2") + "";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string Tar = ub.GetSub("FinanceBzTar", xmlPath);
        string Tar2 = ub.GetSub("FinanceBzTar2", xmlPath);
        if (info == "ok")
        {
            string meverify = new BCW.BLL.User().GetVerifys(uid);
            string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
            string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
            if (!string.IsNullOrEmpty(meverify))
            {
                if (verify.Equals(meverify))
                {
                    Utils.Error("验证码填写错误", "");
                }
            }
            //更新验证码
            new BCW.BLL.User().UpdateVerifys(uid, verify);
            if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
            {
                Utils.Error("验证码填写错误", "");
            }
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "数额填写错误"));
            long iMoney = new BCW.BLL.User().GetGold(uid);
            if (payCent > iMoney)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            if (payCent < Convert.ToInt64(Tar))
            {
                Utils.Error("兑换" + ub.Get("SiteBz") + "额必须是" + Tar + "的倍数", "");
            }
            if (payCent % Convert.ToInt64(Tar) != 0)
            {
                Utils.Error("兑换" + ub.Get("SiteBz") + "额必须是" + Tar + "的倍数", "");
            }

            long iTar = Convert.ToInt64(Utils.ParseInt(Tar));
            long iTar2 = Convert.ToInt64(Utils.ParseInt(Tar2));
            long LostGold = 0;
            if (iTar == 1)
                LostGold = Convert.ToInt64(payCent * iTar2);
            else
                LostGold = Convert.ToInt64(payCent / iTar);

            //检测上一个是否一样
            DataSet ds = new BCW.BLL.Goldlog().GetList("TOP 1 PUrl,AcGold,AddTime", "UsId=" + uid + " and Types=1 ORDER BY ID DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string PUrl = ds.Tables[0].Rows[0]["PUrl"].ToString();
                long AcGold = Int64.Parse(ds.Tables[0].Rows[0]["AcGold"].ToString());
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());

                if (PUrl == Utils.getPageUrl() && Math.Abs(AcGold) == LostGold)
                {
                    if (AddTime > DateTime.Now.AddSeconds(-5))
                    {
                        new BCW.BLL.Guest().Add(10086, "管理员", "ID:" + uid + "在兑换" + ub.Get("SiteBz") + "嫌疑，请进后台查询消费日志并处理。");
                    }
                }
            }
            //是否刷屏
            string appName = "LIGHT_ExChange2";
            int Expir = 60;
            BCW.User.Users.IsFresh(appName, Expir);

            new BCW.BLL.User().UpdateiGold(uid, -payCent, "兑换了" + LostGold + "" + ub.Get("SiteBz2") + "");
            new BCW.BLL.User().UpdateiMoney(uid, LostGold, "花费" + payCent + "" + ub.Get("SiteBz") + "兑换");

            Utils.Success("兑换" + ub.Get("SiteBz2") + "", "兑换" + LostGold + "" + ub.Get("SiteBz2") + "成功，花费了" + payCent + "" + ub.Get("SiteBz") + "，正在返回..", Utils.getUrl("finance.aspx?act=exchange2&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;兑换" + ub.Get("SiteBz2") + "");
            builder.Append(Out.Tab("</div>", ""));

            string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
            strText = "支出多少" + ub.Get("SiteBz2") + "：/,输入验证码:/,,,,";
            strName = "payCent,verify,verifyKey,info,act";
            strType = "num,text,hidden,hidden,hidden";
            strValu = "''" + verifyKey + "'ok'exchange2";
            strEmpt = "false,true,false,false,false";
            strIdea = "'\n<a href=\"" + Utils.getUrl("finance.aspx?act=exchange2&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''''|/";
            strOthe = "&gt;兑换" + ub.Get("SiteBz2") + ",finance.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            long iGold = new BCW.BLL.User().GetGold(uid);
            long iMoney = new BCW.BLL.User().GetMoney(uid);
            builder.Append("当前汇率:<br />" + ub.Get("SiteBz") + "兑换" + ub.Get("SiteBz2") + "" + Tar + ":" + Tar2 + "");
            builder.Append("<br />自带" + iMoney + "" + ub.Get("SiteBz2") + "," + iGold + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 充值页面 VipPayPage
    /// <summary>
    /// 充值入口
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="AgencID">中介ID</param>
    private void VipPayPage(int uid, string AgencID)
    {
        string addBzType = string.Empty;
        if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
            addBzType = ub.Get("SiteBz");
        else
            addBzType = ub.Get("SiteBz2");

        Master.Title = "充值" + addBzType + "";
        string ptype = Utils.GetRequest("ptype", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (AgencID != "")
        {
            ptype = "0";
            info = "Gateway";
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;充值" + addBzType + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == "")
        {
            #region 充值卡模式
            string Amt1 = ub.GetSub("FinanceAmt1", xmlPath);
            if (Amt1 != "")
            {
                #region 神州行
                builder.Append(Out.Tab("<div>", ""));
                if (info == "SZX")
                {
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.getPage(0) + "") + "\">-神州行充值</a><br />");
                    if (("#" + Amt1 + "#").Contains("#10#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">10元卡</a><br />");
                    if (("#" + Amt1 + "#").Contains("#30#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;ptype=30&amp;backurl=" + Utils.getPage(0) + "") + "\">30元卡</a><br />");
                    if (("#" + Amt1 + "#").Contains("#50#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;ptype=50&amp;backurl=" + Utils.getPage(0) + "") + "\">50元卡</a><br />");
                    if (("#" + Amt1 + "#").Contains("#100#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;ptype=100&amp;backurl=" + Utils.getPage(0) + "") + "\">100元卡</a><br />");
                    if (("#" + Amt1 + "#").Contains("#300#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;ptype=300&amp;backurl=" + Utils.getPage(0) + "") + "\">300元卡</a><br />");
                    if (("#" + Amt1 + "#").Contains("#500#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;ptype=300&amp;backurl=" + Utils.getPage(0) + "") + "\">500元卡</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=SZX&amp;backurl=" + Utils.getPage(0) + "") + "\">+神州行充值</a><br />");
                }
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
            string Amt2 = ub.GetSub("FinanceAmt2", xmlPath);
            if (Amt2 != "")
            {
                #region 联通卡
                builder.Append(Out.Tab("<div>", ""));
                if (info == "UNICOM")
                {
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.getPage(0) + "") + "\">-联通卡充值</a><br />");
                    if (("#" + Amt2 + "#").Contains("#20#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;ptype=20&amp;backurl=" + Utils.getPage(0) + "") + "\">20元卡</a><br />");
                    if (("#" + Amt2 + "#").Contains("#30#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;ptype=30&amp;backurl=" + Utils.getPage(0) + "") + "\">30元卡</a><br />");
                    if (("#" + Amt2 + "#").Contains("#40#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;ptype=40&amp;backurl=" + Utils.getPage(0) + "") + "\">40元卡</a><br />");
                    if (("#" + Amt2 + "#").Contains("#50#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;ptype=50&amp;backurl=" + Utils.getPage(0) + "") + "\">50元卡</a><br />");
                    if (("#" + Amt2 + "#").Contains("#100#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;ptype=100&amp;backurl=" + Utils.getPage(0) + "") + "\">100元卡</a><br />");
                    if (("#" + Amt2 + "#").Contains("#300#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;ptype=300&amp;backurl=" + Utils.getPage(0) + "") + "\">300元卡</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=UNICOM&amp;backurl=" + Utils.getPage(0) + "") + "\">+联通卡充值</a><br />");
                }
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
            string Amt3 = ub.GetSub("FinanceAmt3", xmlPath);
            if (Amt3 != "")
            {
                #region 电信卡
                builder.Append(Out.Tab("<div>", ""));
                if (info == "TELECOM")
                {
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.getPage(0) + "") + "\">-电信卡充值</a><br />");
                    if (("#" + Amt3 + "#").Contains("#30#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=TELECOM&amp;ptype=30&amp;backurl=" + Utils.getPage(0) + "") + "\">30元卡</a><br />");
                    if (("#" + Amt3 + "#").Contains("#50#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=TELECOM&amp;ptype=50&amp;backurl=" + Utils.getPage(0) + "") + "\">50元卡</a><br />");
                    if (("#" + Amt3 + "#").Contains("#100#"))
                        builder.Append("--<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=TELECOM&amp;ptype=100&amp;backurl=" + Utils.getPage(0) + "") + "\">100元卡</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=TELECOM&amp;backurl=" + Utils.getPage(0) + "") + "\">+电信卡充值</a><br />");
                }
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
            #endregion

            if (!Utils.GetDomain().Contains("dyj6"))
            {
                #region 网上支付
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB("[url=finance.aspx?act=vippay&amp;ptype=0&amp;info=Gateway&amp;backurl= " + Utils.getPage(0) + "][红]>网上支付[/红][/url]<br />"));
                builder.Append(Out.Tab("</div>", ""));
                #endregion

                builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
                builder.Append(Out.SysUBB("[红]站方只提供一个虚拟娱乐平台,酷币只能通过[url=/bbs/bbsshop.aspx?act=gift&ptype=5]商城[/url]充值[br]站内不以银行卡等方式出售虚拟酷币,同时不支持回收[br]对论坛个人交易、中介交易不支持也不反对,发生的纠纷也不予处理.[/红]"));
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        }
        else
        {
            if (!info.Contains("Gateway"))
            {
                #region 电话卡充值输入
                builder.Append(Out.Tab("<div>", ""));
                if (info == "SZX")
                {
                    if (!("#" + ub.GetSub("FinanceAmt1", xmlPath) + "#").Contains("#" + ptype + "#"))
                    {
                        Utils.Error("系统不开放此面额的神州行充值卡进行充值", "");
                    }
                    builder.Append("神州行" + ptype + "元充值");
                }
                else if (info == "UNICOM")
                {
                    if (!("#" + ub.GetSub("FinanceAmt2", xmlPath) + "#").Contains("#" + ptype + "#"))
                    {
                        Utils.Error("系统不开放此面额的联通充值卡进行充值", "");
                    }
                    builder.Append("联通" + ptype + "元充值");
                }
                else
                {
                    if (!("#" + ub.GetSub("FinanceAmt3", xmlPath) + "#").Contains("#" + ptype + "#"))
                    {
                        Utils.Error("系统不开放此面额的电信充值卡进行充值", "");
                    }
                    builder.Append("电信" + ptype + "元充值");
                }

                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;重选</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("提示:1元=" + ub.GetSub("FinanceSZXTar", xmlPath) + "" + addBzType + "");
                builder.Append(Out.Tab("</div>", ""));

                string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));

                strText = "充值卡序列号:/,充值卡密码:/,输入验证码:/,,,,";
                strName = "cardNum,cardPwd,verify,verifyKey,cardAmt,info,act";
                strType = "num,num,snum,hidden,hidden,hidden,hidden";
                strValu = "'''" + verifyKey + "'" + ptype + "'" + info + "'paysave";
                strEmpt = "false,false,false,false,false,false,false";
                strIdea = "''\n<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=" + info + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>''''|/";
                strOthe = "立即充值," + Utils.getUrl("finance.aspx") + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                #endregion
            }
            else
            {
                #region 网上支付
                if (AgencID == "")
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;重选</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("充值" + ub.Get("SiteBz") + "-来自ID" + AgencID);
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("提示:1元=" + ub.GetSub("FinanceSZXTar", xmlPath) + "" + addBzType + "");
                builder.Append(Out.Tab("</div>", ""));

                string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
                string key = Utils.GetRequest("key", "get", 1, "", "");
                strText = "金额整数(大于1元):/,输入验证码:/,,,,";
                strName = "Amount,verify,verifyKey,key,info,act";
                strType = "num,snum,hidden,hidden,hidden,hidden";
                strValu = "''" + verifyKey + "'" + key + "'" + info + "'ipspaysave";
                strEmpt = "false,false,false,false,false,false";
                strIdea = "'\n<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;info=" + info + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>''''|/";
                strOthe = "立即充值," + Utils.getUrl("finance.aspx") + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                #endregion
            }
        }

        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 电话卡充值保存订单 PaySavePage
    /// <summary>
    /// 电话卡充值保存订单
    /// </summary>
    /// <param name="uid"></param>
    private void PaySavePage(int uid)
    {
        Master.Title = "充值";
        string meverify = new BCW.BLL.User().GetVerifys(uid);
        string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
        string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
        string info = Utils.GetRequest("info", "post", 2, @"^SZX|UNICOM|TELECOM$", "充值卡类型错误");
        if (!string.IsNullOrEmpty(meverify))
        {
            if (verify.Equals(meverify))
            {
                Utils.Error("验证码填写错误", "");
            }
        }
        //更新验证码
        new BCW.BLL.User().UpdateVerifys(uid, verify);
        if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
        {
            Utils.Error("验证码填写错误", "");
        }

        int cardAmt = int.Parse(Utils.GetRequest("cardAmt", "post", 2, @"^[0-9]\d*$", "面值选择错误"));
        string cardNum = Utils.GetRequest("cardNum", "post", 4, @"^[0-9]\d*$", "序列号填写错误");
        string cardPwd = Utils.GetRequest("cardPwd", "post", 4, @"^[0-9]\d*$", "密码填写错误");

        int Types = 0;
        if (info == "SZX")
        {
            if (!("#" + ub.GetSub("FinanceAmt1", xmlPath) + "#").Contains("#" + cardAmt + "#"))
            {
                Utils.Error("系统不开放此面额的神州行充值卡进行充值", "");
            }
            Types = 1;
        }
        else if (info == "UNICOM")
        {
            if (!("#" + ub.GetSub("FinanceAmt2", xmlPath) + "#").Contains("#" + cardAmt + "#"))
            {
                Utils.Error("系统不开放此面额的联通充值卡进行充值", "");
            }
            Types = 2;
        }
        else
        {
            if (!("#" + ub.GetSub("FinanceAmt3", xmlPath) + "#").Contains("#" + cardAmt + "#"))
            {
                Utils.Error("系统不开放此面额的电信充值卡进行充值", "");
            }
            Types = 3;
        }

        //写入数据库
        BCW.Model.Payrmb model = new BCW.Model.Payrmb();
        model.UsID = uid;
        model.UsName = new BCW.BLL.User().GetUsName(uid);
        model.State = 0;
        model.Types = Types;
        model.CardAmt = cardAmt;
        model.CardNum = cardNum;
        model.CardPwd = cardPwd;
        model.CardOrder = DT.getDateTimeNum();
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;

        model.MerBillNo = model.CardOrder;                      //商户订单号 必填字母及数字
        model.Amount = 0;                                       //充值金额 2位小数
        model.GatewayType = "";                                 //默认未支付为空
        model.Attach = "";                                      //默认未支付为空
        model.BillEXP = 3;                                      //默认3小时自动取消订单
        model.GoodsName = ub.Get("SiteBz");                     //商品名称            
        model.IsCredit = "1";                                   //直连选项 1
        model.BankCode = "";                                    //IPS唯一标识指定的银行编号 00018
        model.ProductType = "-1";                                 //产品类型 -1未填 1个人银行 2企业银行
        model.AddUsIP = Utils.GetUsIP();                        //提交IP
        model.AddTime = DateTime.Now;                           //提交时间

        new BCW.BLL.Payrmb().Add(model);
        //发内线给客服10086
        new BCW.BLL.Guest().Add(10086, "客服", "ID：" + uid + "进行充值操作，请到后台充值日志进行处理");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("支付请求提交成功，稍后系统会将充值结果用内线方式发送给您，请留意。");
        builder.Append("<br /><a href=\"" + Utils.getUrl("finance.aspx?act=viplist&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;查看充值结果</a>");
        builder.Append(Out.Tab("</div>", ""));


        // 设置 Response编码格式为UTF-8
        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        //string p2_Order, p3_Amt, p4_verifyAmt, p5_Pid, p6_Pcat, p7_Pdesc, p8_Url, pa8_cardNo, pa9_cardPwd, pa_MP, pa7_cardAmt, pd_FrpId, pr_NeedResponse, pz_userId, pz1_userRegTime;
        //p2_Order = DT.getDateTimeNum();//订单号
        //p3_Amt = cardAmt.ToString();//订单金额
        //p4_verifyAmt = "true";//是否较验订单金额
        //p5_Pid = "";//产品名称
        //p6_Pcat = "";//产品类型
        //p7_Pdesc = "";//产品描述
        //p8_Url = "http://" + Request.Url.Host + ":" + Request.Url.Port.ToString() + "/callback.aspx";//接收支付结果通知地址
        //pa_MP = "";//扩展信息
        //pa7_cardAmt = cardAmt.ToString();//卡面额
        //pa8_cardNo = cardNum;//卡序列号
        //pa9_cardPwd = cardPwd;//卡密码
        //pd_FrpId = info;//支付通道编码
        //pr_NeedResponse = "1";
        //pz_userId = uid.ToString();//用户唯一标识
        //pz1_userRegTime = DateTime.Now.ToString(); //用户的注册时间 

        //try
        //{
        //    //写入数据库
        //    BCW.Model.Payrmb model = new BCW.Model.Payrmb();
        //    model.UsID = uid;
        //    model.UsName = new BCW.BLL.User().GetUsName(uid);
        //    model.State = 0;
        //    model.Types = Types;
        //    model.CardAmt = cardAmt;
        //    model.CardNum = cardNum;
        //    model.CardPwd = cardPwd;
        //    model.CardOrder = p2_Order;
        //    model.AddUsIP = Utils.GetUsIP();
        //    model.AddTime = DateTime.Now;
        //    new BCW.BLL.Payrmb().Add(model);

        //    //非银行卡专业版正式使用
        //    SZXResult result = SZX.AnnulCard(p2_Order, p3_Amt, p4_verifyAmt, p5_Pid, p6_Pcat, p7_Pdesc, p8_Url,
        //    pa_MP, pa7_cardAmt, pa8_cardNo, pa9_cardPwd, pd_FrpId, pr_NeedResponse, pz_userId, pz1_userRegTime);


        //    if (result.R1_Code == "1")
        //    {
        //        //builder.Append("非银行卡支付请求提交成功");
        //        //builder.Append("<br />商户订单号:" + result.R6_Order);
        //        //builder.Append("<br /><br />" + result.ReqResult);
        //        builder.Append(Out.Tab("<div>", ""));
        //        builder.Append("支付请求提交成功");
        //        builder.Append("<br /><a href=\"" + Utils.getUrl("finance.aspx?act=viplist&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;查看充值结果</a>");
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //    else
        //    {
        //        builder.Append(Out.Tab("<div>", ""));
        //        //builder.Append("非银行卡支付请求提交失败 [" + result.R1_Code + "]" + result.Rq_ReturnMsg);
        //        builder.Append("支付请求提交失败");

        //        if (result.R1_Code == "11")
        //        {
        //            builder.Append("-订单号重复");
        //        }
        //        else if (result.R1_Code == "7")
        //        {
        //            builder.Append("-卡密无效");
        //        }
        //        else if (result.R1_Code == "61")
        //        {
        //            builder.Append("-账户未开通");
        //        }
        //        builder.Append(Out.Tab("</div>", ""));
        //        //builder.Append("<br /><br />" + result.ReqUrl);
        //        //builder.Append("<br /><br />" + result.ReqResult);

        //    }
        //}
        //catch (Exception ex)
        //{
        //    builder.Append(ex.ToString());
        //}


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 网上支付保存订单 IPaySavePage
    /// <summary>
    /// 网上支付保存订单
    /// </summary>
    /// <param name="meid"></param>
    private void IPaySavePage(int uid)
    {
        Master.Title = "网上支付";
        string meverify = new BCW.BLL.User().GetVerifys(uid);
        string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
        string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
        string info = Utils.GetRequest("info", "post", 2, @"^Gateway$", "支付类型错误");

        #region 判断是否中介充值链
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string key = Utils.GetRequest("key", "post", 1, "", "");
        string keyid = "";
        GetAgenceID(ref act, key, ref keyid);
        string MerKey = "";
        if (keyid != "")
        {
            MerKey = keyid + "AGENC";
        }
        #endregion

        if (!string.IsNullOrEmpty(meverify))
        {
            if (verify.Equals(meverify))
            {
                Utils.Error("验证码填写错误", "");
            }
        }
        //更新验证码
        new BCW.BLL.User().UpdateVerifys(uid, verify);
        if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
        {
            Utils.Error("验证码填写错误", "");
        }

        long Amount = long.Parse(Utils.GetRequest("Amount", "post", 2, @"^[0-9]\d*$", "金额填写错误"));

        #region  增加订单写入数据库
        //int Types = 0;
        if (info == "Gateway")
        {
            //如果是中介链接,则判断中介账号是否够币    
            if (keyid != "")
            {
                long mycent = new BCW.BLL.User().GetGold(int.Parse(keyid));
                long outcent = Convert.ToInt64(Amount);
                if (outcent > mycent)
                {
                    Utils.Error("帐户币值不足完成这次支付", "");
                }
            }

            //增加订单写入数据库
            BCW.Model.Payrmb model = new BCW.Model.Payrmb();
            model.UsID = uid;
            model.UsName = new BCW.BLL.User().GetUsName(uid);
            model.MerBillNo = MerKey + DT.getDateTimeNum();         //商户订单号 必填字母及数字
            model.State = 0;                                        //未处理
            model.Types = 100;                                      //100标记
            model.Amount = Amount;                                  //充值金额 2位小数
            model.GatewayType = "";                                 //默认未支付为空
            model.Attach = "";                                      //默认未支付为空
            model.BillEXP = 3;                                      //默认3小时自动取消订单
            model.GoodsName = ub.Get("SiteBz");    //商品名称            
            model.IsCredit = "1";                                   //直连选项 1
            model.BankCode = "";                                    //IPS唯一标识指定的银行编号 00018
            model.ProductType = "-1";                                 //产品类型 -1未填 1个人银行 2企业银行
            model.AddUsIP = Utils.GetUsIP();                        //提交IP
            model.AddTime = DateTime.Now;                           //提交时间
            model.CardAmt = 0;
            model.CardNum = "";
            model.CardPwd = "";
            model.CardOrder = "";
            int MsgId = 0;
            MsgId = new BCW.BLL.Payrmb().Add(model);
            if (MsgId > 0)
            {
                new BCW.BLL.Guest().Add(10086, "客服", "ID：" + uid + "进行网上支付,生成订单成功,单号:" + model.MerBillNo);
                Utils.Success("订单生成成功", "正在为你转向支付页面,请稍后", Utils.getUrl("finance.aspx?act=MerBillPay&amp;MsgId=" + MsgId), "2");
            }
        }
        #endregion

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 环迅支付页面 MerBillPayPage
    private void MerBillPayPage(int meid)
    {
        Master.Title = "支付订单";
        string addBzType = string.Empty;
        if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
            addBzType = ub.Get("SiteBz");
        else
            addBzType = ub.Get("SiteBz2");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;充值" + addBzType + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //if (!Utils.Isie() || Utils.GetUA().ToLower().Contains("opera"))
        //{
        //    Utils.Error("为了你的资金安全,本类别商品只支持IE9或以上浏览器<br />Opera或旧版浏览器将无法完成支付", Utils.getUrl("finance.aspx?act=viplist"));
        //}
        string PostUrl = "http://" + Request.UrlReferrer.Authority;
        int MsgId = int.Parse(Utils.GetRequest("MsgId", "get", 2, @"^[0-9]\d*$", "金额填写错误"));
        string ok = (Utils.GetRequest("ok", "get", 3, "", ""));
        if (MsgId != 0)
        {
            #region 获取订单信息
            BCW.Model.Payrmb model = new BCW.BLL.Payrmb().GetPayrmb(MsgId);
            if (model != null)
            {
                if (model.Types == 100 && model.State == 0)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    builder.Append("你正在支付订单" + model.MerBillNo + "<br />" + model.GoodsName + " " + model.Amount.ToString("0.00"));
                    builder.Append(Out.Tab("</div>", ""));
                    if (ok != "sure")
                    {
                        string BankCodestr = "";
                        for (int i = 0; i < BCW.IPSPay.IPSPayMent.BankCodes.Length; i++)
                        {
                            if (BankCodestr != "") { BankCodestr += "|"; }
                            BankCodestr += BCW.IPSPay.IPSPayMent.BankCodes[i] + "|" + BCW.IPSPay.IPSPayMent.BankNames[i];
                        }

                        strText = "选择支付银行:/,,,,";
                        strName = "BankCode,GatewayType,verifyKey,act";
                        strType = "select,hidden,hidden,hidden";
                        strValu = "''''ipspaysave";
                        strEmpt = BankCodestr + ",01,false,false";
                        strOthe = "支付￥" + model.Amount.ToString("0.00") + "," + BCW.IPSPay.IPSPayMent.IPS_POST_URL + Utils.getUrl("finance.aspx?act=MerBillPay&amp;ok=sure&amp;MsgId=" + MsgId) + ",post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        string reBankCode = Utils.GetRequest("BankCode", "post", 2, "", "银行码错误");
                        string GatewayType = "01";// Utils.GetRequest("GatewayType", "post", 3, "", "01");
                        string iSignature = "";
                        string bodystr = "";
                        //得到加密签名
                        string usUrl = PostUrl + Utils.getUrl("bbs/finance.aspx?act=viplist&amp;backurl=" + Utils.getPage(0) + "");
                        bodystr = BCW.IPSPay.IPSPayMent.GetSignature(PostUrl, model, reBankCode, GatewayType, usUrl, ref iSignature);
                        string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost(MsgId.ToString(), DateTime.Now.ToString("yyyyMMddHHmmss"), iSignature, bodystr);
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append(BCW.IPSPay.IPSPayMent.GetBankNameByCode(reBankCode));
                        builder.Append(Out.Tab("</div>", ""));
                        strText = "";
                        strName = "pGateWayReq";
                        strType = "hidden";
                        strValu = pGateWayReqstr;
                        strEmpt = "false";
                        strIdea = "";
                        strOthe = "确定支付￥" + model.Amount.ToString("0.00") + "," + BCW.IPSPay.IPSPayMent.IPS_URL_IPS + ",post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                }
                else
                {
                    Utils.Error("订单支付出错,标识:" + MsgId, "");
                }
            }
            else
            {
                Utils.Error("查无此订单", "");
            }
            #endregion
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"http://" + Request.UrlReferrer.Authority + "\">首页</a>-");
        builder.Append("<a href=\"http://" + Request.UrlReferrer.Authority + "\\bbs\\" + Utils.getPage("finance.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"http://" + Request.UrlReferrer.Authority + "\\bbs\\" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 充值记录 VipListPage
    private void VipListPage(int uid)
    {
        Master.Title = "充值记录";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>&gt;充值记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + "";

        // 开始读取列表
        IList<BCW.Model.Payrmb> listPayrmb = new BCW.BLL.Payrmb().GetPayrmbs(pageIndex, pageSize, strWhere, out recordCount);
        if (listPayrmb.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Payrmb n in listPayrmb)
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

                string sType = string.Empty;
                if (n.Types == 1)
                    sType = "神州行";
                else if (n.Types == 2)
                    sType = "联通";
                else if (n.Types == 3)
                    sType = "电信";
                else if (n.Types == 100)
                    sType = "网上支付";

                string sState = string.Empty;
                string ChkUrl = string.Empty;
                if (n.State == 0)
                {
                    if (n.Types == 100)
                    {
                        sState = "待付款";
                        ChkUrl = Out.SysUBB("[url=finance.aspx?act=MerBillPay&amp;MsgId=" + n.ID + "][红]付款[/红][/url]");
                    }
                    else
                        sState = "未回应";
                }
                else if (n.State == 1)
                {
                    sState = "已成功";
                    //ChkUrl= "<a href=" + Utils.getUrl("finance.aspx?act=view&amp;id=" + n.ID) + ">查看</a>";
                }
                else
                {
                    sState = "已失败";
                }

                int p = (pageIndex - 1) * pageSize + k;

                if (n.Types == 100)
                {
                    builder.AppendFormat("{0}.[{1}]订单:<b>{2}</b>-{3} {4}({5}) {6}", p, sState, n.MerBillNo, n.GoodsName, n.Amount.ToString("0.00"), DT.FormatDate(n.AddTime, 1), ChkUrl);
                    if (n.State == 1)
                    {
                        builder.Append("<br />");
                        if (n.BankCode != "")
                        {
                            string nBankName = BCW.IPSPay.IPSPayMent.GetBankNameByCode(n.BankCode);
                            builder.Append("(" + nBankName + ")");
                        }
                        //if (n.GoodsName != "")
                        //{
                        //    builder.Append(n.GoodsName);
                        //}
                        builder.Append(" " + n.AddUsIP);
                    }
                    else
                    {
                        if (n.State == 0)
                        {
                            if (n.AddTime.AddHours(n.BillEXP) < DateTime.Now)
                            {
                                n.State = 2;
                                new BCW.BLL.Payrmb().Update_ips(n);
                            }
                        }
                        //cn.com.ips.newpay.WSOrderQuery WSorder = new cn.com.ips.newpay.WSOrderQuery();
                        //string SignatureOrder = "", bodystr = "", Resultstr = ""; ;
                        //bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByChkOrder(n, ref SignatureOrder);
                        //string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost_ByOrder(DateTime.Now.ToString("yyyyMMddHHmmss"), SignatureOrder, bodystr);
                        //Resultstr = WSorder.getOrderByMerBillNo(pGateWayReqstr);
                        //BCW.IPSPay.IPSPayMent.updateorder(Resultstr, false);
                        builder.Append("<br />请支付订单,若已成功扣费,请尝试刷新页面查看状态");
                    }
                }
                else
                {
                    builder.AppendFormat("{0}.[" + sState + "]" + sType + "{1}卡({2})", p, n.CardAmt, DT.FormatDate(n.AddTime, 1));
                    builder.Append("<br />序列号" + n.CardNum + "");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.getPage(0) + "") + "\">金融服务</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 查看list CheckPListPage
    private void CheckPListPage(int uid)
    {
        string CheckID = ub.GetSub("BbsCheckID", "/Controls/bbs.xml");
        if (!("#" + CheckID + "#").Contains("#" + uid + "#"))
        {
            Utils.Error("地址错误", "");
        }
        Master.Title = "金融服务";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看对象:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=checkplist&amp;hid=" + hid + "&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz") + "记录</a>&gt;" + ub.Get("SiteBz2") + "记录");
        else
            builder.Append("" + ub.Get("SiteBz") + "记录&gt;<a href=\"" + Utils.getUrl("finance.aspx?act=checkplist&amp;hid=" + hid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz2") + "记录</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "hid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + hid + " and Types=" + ptype + " and BbTag<=1";

        // 开始读取列表
        IList<BCW.Model.Goldlog> listGoldlog = new BCW.BLL.Goldlog().GetGoldlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listGoldlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listGoldlog)
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
                builder.AppendFormat("{0}.{1}|操作{2}|结{3}({4})", (pageIndex - 1) * pageSize + k, n.AcText, n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 1));

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + hid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "") + "\">TA的空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion
}