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

/// <summary>
/// 测试
/// 屏蔽球赛动态 黄国军 20160617
/// 最新修改 黄国军 20160127
/// 陈志基 添加 群聊权限显示 20160830
/// </summary>
public partial class bbs_uinfo : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/leven.xml";
    protected string xmlPath2 = "/Controls/bbs.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        int meid = new BCW.User.Users().GetUsId();
        int uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[1-9]\d*$", "0"));
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (uid > 0)
        {
            if (!new BCW.BLL.User().Exists(uid))
            {
                Utils.Error("此用户不存在", "");
            }
        }
        else
        {
            if (meid == 0)
            {
                Utils.Login();
            }
            uid = meid;
        }

        BCW.Model.User model = new BCW.BLL.User().GetBasic(uid);
        if (act == "bankuser")
        {
            #region 银行资料
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            {
                if (uid != meid && (!("#" + ub.GetSub("BbsBankID", xmlPath2) + "#").Contains("#" + meid + "#")))
                {
                    Utils.Error("无权查看", "");
                }
                Master.Title = "" + model.UsName + "(" + uid + ")的银行资料";
                builder.Append(Out.Tab("<div class=\"title\">" + BCW.User.Users.SetVipName(uid, model.UsName, true) + "的银行资料</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("本页面仅自己和中介可看到");
                builder.Append(Out.Tab("</div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));

                BCW.Model.BankUser m = new BCW.BLL.BankUser().GetBankUser(uid);
                if (m == null)
                {
                    if (uid != meid)
                    {
                        builder.Append("对方尚未设置银行资料");
                    }
                    else
                    {
                        builder.Append("尚未设置银行资料<a href=\"" + Utils.getUrl("myedit.aspx?act=bankuser&amp;backurl=" + Utils.getPage(0) + "") + "\">马上设置</a>");
                    }
                }
                else
                {
                    builder.Append("真实姓名:" + m.BankName + "<br />");
                    builder.Append("银行名称1:" + m.BankTitle1 + "<br />");
                    builder.Append("银行帐号1:" + m.BankNo1 + "<br />");
                    builder.Append("银行卡开户地址1:" + m.BankAdd1 + "");
                    builder.Append(Out.Hr());
                    builder.Append("银行名称2:" + m.BankTitle2 + "<br />");
                    builder.Append("银行帐号2:" + m.BankNo2 + "<br />");
                    builder.Append("银行卡开户地址2:" + m.BankAdd2 + "");
                    builder.Append(Out.Hr());
                    builder.Append("银行名称3:" + m.BankTitle3 + "<br />");
                    builder.Append("银行帐号3:" + m.BankNo3 + "<br />");
                    builder.Append("银行卡开户地址3:" + m.BankAdd3 + "");
                    builder.Append(Out.Hr());
                    builder.Append("银行名称4:" + m.BankTitle4 + "<br />");
                    builder.Append("银行帐号4:" + m.BankNo4 + "<br />");
                    builder.Append("银行卡开户地址4:" + m.BankAdd4 + "");
                    builder.Append(Out.Hr());
                    builder.Append("支付宝名称:" + m.ZFBName + "<br />");
                    builder.Append("支付宝帐号:" + m.ZFBNo + "<br />");
                    builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=bankuser&amp;backurl=" + Utils.getPage(0) + "") + "\">修改银行资料</a>");

                }
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">我的空间</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        else if (act == "view")
        {
            #region 查看会员资料
            Master.Title = "" + model.UsName + "(" + uid + ")的资料";

            builder.Append(Out.Tab("<div class=\"title\">" + BCW.User.Users.SetVipName(uid, model.UsName, true) + "的资料</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + model.Photo + "\" alt=\"load\"/><br />");
            builder.Append("ID:" + uid + "<br />");

            builder.Append("昵称:" + BCW.User.Users.SetVipName(uid, model.UsName, false) + "");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("身份:");
            int iGroupId = new BCW.BLL.Group().GetID(uid);

            DataSet dss = new BCW.BLL.Role().GetList("ID,RoleName,ForumID,ForumName", "UsID=" + uid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 ORDER BY FORUMID ASC");
            if (dss != null && dss.Tables[0].Rows.Count > 0 || iGroupId > 0 || model.Limit.Contains("hon"))
            {
                if (model.Limit.Contains("honor"))
                    builder.Append(" 荣誉版主");//<img src=\"/Files/sys/honor.gif\" alt=\"load\"/>

                if (model.Limit.Contains("hon2or"))
                    builder.Append(" 荣誉会员");

                if (model.Limit.Contains("hon"))
                    builder.Append("<br />");

                builder.Append("管辖:");

                if (dss != null && dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        int id = int.Parse(dss.Tables[0].Rows[i]["ID"].ToString());
                        int forumid = int.Parse(dss.Tables[0].Rows[i]["ForumID"].ToString());
                        string ForumName = dss.Tables[0].Rows[i]["ForumName"].ToString();
                        string RoleName = dss.Tables[0].Rows[i]["RoleName"].ToString();
                        if (forumid == -1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a>管辖全站<br />");
                        }
                        else if (forumid == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a>管辖全区版块<br />");
                        }
                        else
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a>管辖<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumName + "</a><br />");
                        }
                    }
                }
                if (iGroupId > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=view&amp;id=" + iGroupId + "") + "\">" + new BCW.BLL.Group().GetTitle(iGroupId) + "</a>" + ub.GetSub("GroupzName", "/Controls/group.xml") + "<br />");
                }
            }
            else
            {
                builder.Append("普通会员<br />");
            }

            if (model.IsVerify == 1)
            {
                builder.Append("验证:已通过短信验证");
            }
            else
            {
                builder.Append("验证:未通过验证 ");
                if (uid == meid)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/reg.aspx?act=validate") + "\">马上验证</a>");
                }
            }
            builder.Append("<br />状态:" + BCW.User.Users.UserOnline(uid, model.State, model.EndTime) + "");
            builder.Append("(累积" + BCW.User.Users.ChangeDayff(model.OnTime) + ")");
            string[] paras = model.Paras.Split(",".ToCharArray());
            int tili = Convert.ToInt32(paras[0].Split("|".ToCharArray())[1]);
            builder.Append("<br />等级:" + model.Leven + "级<br />");
            builder.Append("勋章:" + BCW.User.Users.SetMedal(uid) + "" + BCW.User.Users.SetVip(uid) + "<br />");

            if (uid == meid)
            {
                long iScore = Convert.ToInt64(ub.GetSub("LevenL" + (model.Leven + 1) + "", xmlPath));
                int itili = Convert.ToInt32(ub.GetSub("LevenR" + (model.Leven + 1) + "", xmlPath));
                if (model.iScore >= iScore && tili > itili)
                {
                    if (info == "up")
                    {
                        new BCW.BLL.User().UpdateLeven(meid, model.Leven + 1, iScore);
                        //计算属性与值
                        string sParas = BCW.User.Users.GetParaData(model.Paras, -itili, 0);
                        new BCW.BLL.User().UpdateParas(meid, sParas);
                        Utils.Success("升级", "恭喜您升" + (model.Leven + 1) + "级，正在返回..", Utils.getUrl("/bbs/uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + ""), "2");

                    }
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?act=view&amp;uid=" + uid + "&amp;info=up&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;升级至" + (model.Leven + 1) + "级</a><br />");
                }

                builder.Append("(升级至" + (model.Leven + 1) + "级需耗费" + iScore + "积分、" + itili + "体力)<br />");
            }
            builder.Append("性别:" + BCW.User.AppCase.CaseSex(model.Sex) + "<br />");
            builder.Append("年龄:" + Utils.GetAge(model.Birth) + "<br />");
            //-------------------------------------------婚姻
            DataSet ds = new BCW.BLL.Marry().GetList("ID,UsID,UsName,ReID,ReName,HomeName", "(UsID=" + uid + " or ReID=" + uid + ") and Types=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (uid == int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString()))
                {
                    builder.Append("配偶:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["ReID"].ToString() + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[0]["ReName"].ToString() + "</a><br />");
                }
                else
                {
                    builder.Append("配偶:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"].ToString() + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[0]["UsName"].ToString() + "</a><br />");
                }
                builder.Append("花园:<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id=" + ds.Tables[0].Rows[0]["ID"].ToString() + "") + "\">" + ds.Tables[0].Rows[0]["HomeName"].ToString() + "</a><br />");
            }
            else
            {
                builder.Append("婚姻:未婚");
                if (uid == meid)
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">征婚</a><br />");
                else
                    builder.Append(" <a href=\"" + Utils.getUrl("/bbs/marry.aspx?act=love&amp;ReID=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">示爱</a><br />");
            }
            //-------------------------------------------婚姻
            builder.Append("居所:" + model.City + "<br />");
            string ForumSet = new BCW.BLL.User().GetForumSet(uid);
            int IsActive = BCW.User.Users.GetForumSet(ForumSet, 24);
            bool IsSeenCent = false;
            if (("#" + ub.GetSub("BbsMoneyID", xmlPath2) + "#").Contains("#" + meid + "#"))
            {
                IsSeenCent = true;
            }
            if (IsActive == 0 || uid == meid || IsSeenCent == true)
            {
                builder.Append("" + ub.Get("SiteBz") + ":" + model.iGold + "<br />");
                builder.Append("" + ub.Get("SiteBz2") + ":" + model.iMoney + "<br />");
            }
            else
            {
                builder.Append("" + ub.Get("SiteBz") + ":已隐藏<br />");
                builder.Append("" + ub.Get("SiteBz2") + ":已隐藏<br />");
            }
            if (model.IsFreeze == 1)
            {
                builder.Append("<b>[财产已被冻结]</b><br />");
            }
            builder.Append("积分:" + model.iScore + "<br />");
            builder.Append("=社区属性值=<br />");
            for (int i = 0; i < paras.Length; i++)
            {
                string[] para = paras[i].ToString().Split("|".ToCharArray());
                builder.Append(para[0] + ":" + para[1] + "<br />");
            }
            builder.Append("签名:" + model.Sign + "");
            if (uid == meid)
            {
                builder.Append("<br />手机号:" + BCW.User.Users.FormatMobile(model.Mobile) + "");
            }
            builder.Append("<br />在线:" + BCW.User.Users.ChangeDayff(model.OnTime) + "");

            if (model.State == 1)
                builder.Append("<br />最后在线:" + DT.FormatDate(model.EndTime2, 0) + "");
            else
                builder.Append("<br />最后在线:" + DT.FormatDate(model.EndTime, 0) + "");
            builder.Append("<br />注册时间:" + DT.FormatDate(model.RegTime, 0) + "");
            builder.Append("<br />红包口令:<a href=\"" + Utils.getUrl("chathb.aspx?act=mykeys&amp;usid=" + uid) + "\">查看</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=bankuser&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看银行资料</a><br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.PostPage(1) + "") + "\">个人设置</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?backurl=" + Utils.PostPage(1) + "") + "\">金融服务</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=medal&amp;backurl=" + Utils.PostPage(1) + "") + "\">勋章商店</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=photo&amp;backurl=" + Utils.PostPage(1) + "") + "\">头像更换</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">我的VIP</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">在线充值</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=bind&amp;backurl=" + Utils.PostPage(1) + "") + "\">手机改绑</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=exit") + "\">退出登录</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">我的空间</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region 默认页
            Master.Title = "" + model.UsName + "的空间";
            model.UsName = BCW.User.Users.SetVipName(uid, model.UsName, false);
            string IsOnline = "在线";
            string setName = string.Empty;
            string sexType = string.Empty;
            string Sex = string.Empty;

            #region 性别,称呼,在线状态
            if (model.Sex <= 1)
            {
                setName = "girl";
                sexType = "她";
                Sex = "美女";
            }
            else
            {
                setName = "boy";
                sexType = "他";
                Sex = "帅哥";
            }
            if (!BCW.User.Users.IsOnline(uid, model.State, model.EndTime))
            {
                setName = setName + "2";
                IsOnline = "离线";
            }
            #endregion

            #region 情景标识
            //情景标识
            string getScene = string.Empty;
            int scene = BCW.User.Users.GetForumSet(model.ForumSet, 32);
            if (scene != -1)
            {
                getScene = "<a href=\"" + Utils.getUrl("myedit.aspx?act=scene&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/scene/" + BCW.User.Scene.GetScene[1][scene] + ".gif\" alt=\"load\"/></a>";
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + BCW.User.Users.SetForumPic(uid) + "" + getScene + "" + BCW.User.Users.SetVipName(uid, model.UsName, true) + "" + IsOnline + "");
            builder.Append("<br /><img src=\"" + model.Photo + "\" alt=\"load\"/>");

            builder.Append(Out.Tab("</div>", ""));

            if (uid != meid || Request["pw"] == "ok")
            {
                strText = ",,,,";
                strName = "Content,toid,act,backurl";
                strType = "text,hidden,hidden,hidden";
                strValu = "'" + uid + "'save'" + Utils.PostPage(1) + "";
                strEmpt = "true,false,false,false";
                strIdea = "/";
                strOthe = "发内线,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append(" <a href=\"" + Utils.getUrl("/bbs/guest.aspx?act=chat&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">记录</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?act=view&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a> ");
                builder.Append(" <a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=paytype&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">转币</a><br />");

                builder.Append(" <a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加好友</a>");
                builder.Append(" <a href=\"" + Utils.getUrl("/bbs/marry.aspx?act=love&amp;ReID=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">示爱</a>");
                builder.Append(" <a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=addblack&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加黑</a>");
                builder.Append(" <a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=addfans&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">关注</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/friend.aspx?backurl=" + Utils.PostPage(1) + "") + "\">好友</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/guest.aspx?backurl=" + Utils.PostPage(1) + "") + "\">内线</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?act=view&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?backurl=" + Utils.PostPage(1) + "") + "\">设置</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            #endregion

            #region 勋章,财产
            builder.Append("" + Sex + "/" + Utils.GetAge(model.Birth) + "岁/" + ((model.City == "") ? "未知" : model.City) + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=addvip") + "\">" + BCW.User.Users.SetVip(uid) + "</a>");

            string MedalStr = "";
            int MedalNum = new BCW.BLL.Medalget().GetCount(uid);
            if (MedalNum == 0)
                MedalStr = "勋章记录";
            else
                MedalStr = MedalNum + "个勋章";

            builder.Append("<br />荣誉:<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=medallog&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">(" + MedalStr + ")</a>" + BCW.User.Users.SetMedal(uid) + "");
            //设置勋章权限
            string MedalAdminID = ub.GetSub("BbsMedalAdminID", xmlPath2);
            if (("#" + MedalAdminID + "#").Contains("#" + meid + "#"))
            {
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=memedal&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>");
            }
            string ForumSet = new BCW.BLL.User().GetForumSet(uid);
            int IsActive = BCW.User.Users.GetForumSet(ForumSet, 24);
            bool IsSeenCent = false;
            if (("#" + ub.GetSub("BbsMoneyID", xmlPath2) + "#").Contains("#" + meid + "#"))
            {
                IsSeenCent = true;
            }
            if (IsActive == 0 || uid == meid || IsSeenCent == true)
                //builder.Append("<br />财产:" + ub.Get("SiteBz") + ":" + Utils.ConvertGold(model.iGold) + "/" + ub.Get("SiteBz2") + ":" + Utils.ConvertGold(model.iMoney) + "");
                builder.Append("<br />财产:" + ub.Get("SiteBz") + ":" + BCW.BLL.NewUtils.ConvertGold(model.iGold) + "/" + ub.Get("SiteBz2") + ":" + BCW.BLL.NewUtils.ConvertGold(model.iMoney) + "");
            else
                builder.Append("<br />财产:" + sexType + "的财产已隐藏");

            if (model.IsFreeze == 1)
            {
                builder.Append("<b>[已冻结]</b>");
            }
            //if (Utils.GetTopDomain().Contains("kb288.net"))
            //{
            //    builder.Append("<a href=\"" + Utils.getUrl("/bbs/finance.aspx?backurl=" + Utils.PostPage(1) + "") + "\">[金融]</a>.");
            //}

            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            #region 身份标识
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            string strLimt = BCW.User.Limits.GetBlackLimits(uid);
            builder.Append("自身:");
            if (strLimt != "" || model.Limit.Contains("lock"))
            {
                if (model.Limit.Contains("lock"))
                    builder.Append("已全站锁定");
                else
                    builder.Append("" + strLimt + "");

                builder.Append("<a href=\"" + Utils.getUrl("/bbs/usermanage.aspx?act=view&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[详细]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/usermanage.aspx?act=view&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">所有权限正常</a><br />");
            }
            DataSet ds = null;
            builder.Append("身份:");
            int iGroupId = new BCW.BLL.Group().GetID(uid);

            DataSet dss = new BCW.BLL.Role().GetList("ID,RoleName,ForumID,ForumName", "UsID=" + uid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 ORDER BY FORUMID ASC");
            if (dss != null && dss.Tables[0].Rows.Count > 0 || iGroupId > 0 || model.Limit.Contains("hon"))
            {
                if (model.Limit.Contains("honor"))
                    builder.Append(" 荣誉版主");//<img src=\"/Files/sys/honor.gif\" alt=\"load\"/>

                if (model.Limit.Contains("hon2or"))
                    builder.Append(" 荣誉会员");

                if (model.Limit.Contains("hon"))
                    builder.Append("<br />");

                builder.Append("管辖:");

                if (dss != null && dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        int id = int.Parse(dss.Tables[0].Rows[i]["ID"].ToString());
                        int forumid = int.Parse(dss.Tables[0].Rows[i]["ForumID"].ToString());
                        string ForumName = dss.Tables[0].Rows[i]["ForumName"].ToString();
                        string RoleName = dss.Tables[0].Rows[i]["RoleName"].ToString();
                        if (forumid == -1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a>管辖全站<br />");
                        }
                        else if (forumid == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a>管辖全区版块<br />");
                        }
                        else
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a>管辖<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumName + "</a><br />");
                        }
                    }
                }
                if (iGroupId > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=view&amp;id=" + iGroupId + "") + "\">" + new BCW.BLL.Group().GetTitle(iGroupId) + "</a>" + ub.GetSub("GroupzName", "/Controls/group.xml") + "<br />");
                }
                //if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                //{
                //    if (uid == 111 || uid == 666 || uid == 2222 || uid == 999 || uid == 9991)
                //    {
                //        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=4&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>押金中介</b></a><br />");
                //    }
                //}
                //else
                //{
                //    if (uid == 1119 || uid == 8888 || uid == 666888)
                //    {
                //        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=2&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>押金中介</b></a><br />");
                //    }
                //}

                if (model.UsUbb != "")
                {
                    builder.Append(Out.SysUBB(model.UsUbb));
                }
            }
            else
            {
                //if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                //{
                //    if (uid == 111 || uid == 666 || uid == 2222 || uid == 999 || uid == 9991)
                //    {
                //        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=4&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>押金中介</b></a><br />");
                //    }
                //    else
                //    {
                //        builder.Append("普通会员<br />");
                //    }
                //}
                //else
                //{
                //    if (uid == 1119 || uid == 8888 || uid == 666888)
                //    {
                //        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=2&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>押金中介</b></a><br />");
                //    }
                //    else
                //    {
                //        builder.Append("普通会员<br />");
                //    }
                //}

                if (model.UsUbb != "")
                {
                    builder.Append(Out.SysUBB(model.UsUbb));
                }
            }

            if (uid == 10086)
            {
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\"><b>站内客服</b></a><br />");
            }
            #endregion

            #region 组别权限
            builder.Append("" + ub.GetSub("GroupName", "/Controls/group.xml") + ":");
            bool blgroup = new BCW.BLL.Group().ExistsUsID(uid);
            string GroupId = new BCW.BLL.User().GetGroupId(uid);
            if (blgroup == true || GroupId != "")
            {
                if (blgroup == true)
                {
                    ds = new BCW.BLL.Group().GetList("ID,Title", "UsID=" + uid + " and Status=0");
                    if (ds != null && ds.Tables[0].Rows.Count != 0)
                    {
                        int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                        string Title = ds.Tables[0].Rows[0]["Title"].ToString();
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "(" + ub.GetSub("GroupzName", "/Controls/group.xml") + ")</a>");
                    }
                }
                else
                {
                    GroupId = GroupId.Replace("$", "#");
                    if (GroupId == "" || !GroupId.Contains("#"))
                    {
                        builder.Append("未加入" + ub.GetSub("GroupName", "/Controls/group.xml") + "");
                    }
                    else
                    {
                        GroupId = Utils.DelLastChar(GroupId, "#");
                        GroupId = Utils.Mid(GroupId, 1, GroupId.Length);
                        string[] sName = Regex.Split(GroupId, "##");
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=view&amp;id=" + sName[0] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Group().GetTitle(Convert.ToInt32(sName[0])) + "</a>");
                    }
                }

            }
            else
            {
                builder.Append("未加入" + ub.GetSub("GroupName", "/Controls/group.xml") + "");
            }
            #endregion

            #region  //开始修改(显示群聊权限)  陈志基
            DataSet dt = new BCW.BLL.Chat().GetList("ID,ChatName", "");
            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dt.Tables[0].Rows.Count; j++)
                {
                    int charid = int.Parse(dt.Tables[0].Rows[j]["ID"].ToString());
                    string chatname = dt.Tables[0].Rows[j]["ChatName"].ToString();
                    BCW.Model.Chat model1 = new BCW.BLL.Chat().GetChat(charid);
                    if (model == null)
                    {
                        Utils.Error("不存在的红包群", "");
                    }
                    //室主名单
                    //builder.Append("<b>室主名单</b>");
                    if (!string.IsNullOrEmpty(model1.ChatSZ))
                    {
                        string[] ChatSZ = model1.ChatSZ.Split("#".ToCharArray());
                        for (int i = 0; i < ChatSZ.Length; i++)
                        {
                            if (Convert.ToInt32(ChatSZ[i]) == uid)
                            {
                                builder.Append("<br/>室主管理 <a href=\"" + Utils.getUrl("chatroom.aspx?id=" + charid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">群聊(" + chatname + ")</a>");
                            }
                        }
                    }
                    //见习室主
                    //  builder.Append("<b>见习室主名单</b>");
                    if (!string.IsNullOrEmpty(model1.ChatJS))
                    {
                        string[] ChatJS = model1.ChatJS.Split("#".ToCharArray());

                        for (int i = 0; i < ChatJS.Length; i++)
                        {
                            if (Convert.ToInt32(ChatJS[i]) == uid)
                            {
                                builder.Append("<br/>见习室主管理 <a href=\"" + Utils.getUrl("chatroom.aspx?id=" + charid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">群聊(" + chatname + ")</a>");
                            }
                            // builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + Convert.ToInt32(ChatJS[i]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ChatJS[i])) + "</a>");
                        }
                    }

                    //临管
                    // builder.Append("<b>临管名单</b>");
                    if (!string.IsNullOrEmpty(model1.ChatLG))
                    {
                        string[] ChatLG = model1.ChatLG.Split("#".ToCharArray());
                        for (int i = 0; i < ChatLG.Length; i++)
                        {
                            if (Convert.ToInt32(ChatLG[i]) == uid)
                            {
                                builder.Append("<br/>临管管理 <a href=\"" + Utils.getUrl("chatroom.aspx?id=" + charid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">群聊(" + chatname + ")</a>");
                            }
                            // builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + Convert.ToInt32(ChatLG[i]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ChatLG[i])) + "</a>");
                        }
                    }
                }
            }
            #endregion //结束修改

            #region 应用 黄国军 20161017 屏蔽三个特权
            builder.Append("<br />应用:<a href=\"" + Utils.getUrl("/bbs/finance.aspx?backurl=" + Utils.PostPage(1) + "") + "\">金融</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/finance.aspx?backurl=" + Utils.PostPage(1) + "") + "\">VIP</a>.");

            if (!Utils.GetDomain().Contains("dyj6"))
            {
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/spaceapp/sellnum.aspx?act=uidbuy&amp;backurl=" + Utils.PostPage(1) + "") + "\">靓号</a>.");
                if (DateTime.Now < DateTime.Parse("2016-11-01"))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/spaceapp/changesim.aspx?act=sim&amp;backurl=" + Utils.PostPage(1) + "") + "\">兑话费</a>.");
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/spaceapp/changeqb.aspx?act=qb&amp;backurl=" + Utils.PostPage(1) + "") + "\">换Q币</a>.");
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/spaceapp/changeqqvip.aspx?backurl=" + Utils.PostPage(1) + "") + "\">QQ特权</a>");
                }
            }
            builder.Append("<br />------------");

            builder.Append("<br /><a href=\"" + Utils.getUrl("/bbs/diary.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">日记</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/favorites.aspx?backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/moreThread.aspx?ptype=1&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">发帖</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/moreThread.aspx?ptype=2&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回帖</a>");
            builder.Append("<br /><a href=\"" + Utils.getUrl("/bbs/albums.aspx?leibie=1&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">相册</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/albums.aspx?leibie=2&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">音乐</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/albums.aspx?leibie=3&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">视频</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/albums.aspx?leibie=4&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">资源</a>");


            builder.Append(Out.Tab("</div>", Out.Hr()));

            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/flow.aspx") + "\">【百花艳谷】</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/flow.aspx?act=putlist") + "\">我的花盆</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/flow.aspx?act=top") + "\">风云榜单</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/flow.aspx?act=put&amp;p=1") + "\">一键播种</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/flow.aspx?act=getall") + "\">一键收获</a>");
                builder.Append(Out.Tab("</div>", Out.Hr()));
            }
            builder.Append(Out.Tab("<div>", ""));
            #endregion

            #region 友友留言
            builder.Append("<img src=\"/Files/sys/space/xx.gif\" alt=\"load\"/>【友友留言】<br />");
            // 开始读取留言列表
            IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(3, "UsID=" + uid + " and type=0");//邵广林 20160524 前台留言为0，农场留言为1001
            if (listMebook.Count > 0)
            {
                foreach (BCW.Model.Mebook n in listMebook)
                {
                    string ForMContent = Regex.Replace(n.MContent, @"(\[IMG\])(.*?)(\[\/IMG\])", "", RegexOptions.IgnoreCase);
                    ForMContent = Regex.Replace(ForMContent, @"(\(IMG\))(.*?)(\(\/IMG\))", "", RegexOptions.IgnoreCase);
                    ForMContent = Regex.Replace(ForMContent, @"(\[URL=(.*?)\])(.*?)(\[\/URL\])", "$3", RegexOptions.IgnoreCase);
                    ForMContent = Regex.Replace(ForMContent, @"(\(URL=(.*?)\))(.*?)(\(\/URL\))", "$3", RegexOptions.IgnoreCase);
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.MID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.MName + "</a>" + Out.USB(TrueStrLength.cutTrueLength(ForMContent, 20, "…")) + "<br />");
                }
            }
            else
            {
                builder.Append("暂时无相关记录..<br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/mebook.aspx?hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;查看留言(" + new BCW.BLL.Mebook().GetCount(uid) + ")</a>");//修改3层 sgl
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            #region 她他的动态
            IList<BCW.Model.Action> listAction = null;
            if (Request["pw"] == "ok" || uid != meid)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/space/dt.gif\" alt=\"load\"/>【" + sexType + "的动态】<br />");
                // 开始读取动态列表
                listAction = new BCW.BLL.Action().GetActions(3, "UsID=" + uid + " AND (Types <> 5) AND (Types <> 23)");
                if (listAction.Count > 0)
                {
                    foreach (BCW.Model.Action n in listAction)
                    {
                        if (n.Notes.Contains("uinfo.aspx?uid="))
                            builder.AppendFormat("{0}前{1}<br />", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(n.Notes));
                        else
                            builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}<br />", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
                    }
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?act=me&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;更多动态</a>");
                }
                else
                {
                    builder.Append("暂时无相关记录..");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            #endregion

            #region 好友动态
            builder.Append("<img src=\"/Files/sys/space/dt.gif\" alt=\"load\"/>【好友动态】<br />");
            // 开始读取动态列表
            listAction = new BCW.BLL.Action().GetActionsFriend(0, uid, 3);
            if (listAction.Count > 0)
            {
                foreach (BCW.Model.Action n in listAction)
                {
                    if (n.Notes.Contains("uinfo.aspx?uid="))
                        builder.AppendFormat("{0}前{1}<br />", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(n.Notes));
                    else
                        builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}<br />", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
                }
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?act=friend&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;更多好友动态</a>");
            }
            else
            {
                builder.Append("暂时无相关记录..");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            #region 关注好友
            if (Request["pw"] != "ok" && uid == meid)
            {

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/space/dt.gif\" alt=\"load\"/>【关注友友】<br />");
                // 开始读取动态列表
                listAction = new BCW.BLL.Action().GetActionsFriend(2, uid, 3);
                if (listAction.Count > 0)
                {
                    foreach (BCW.Model.Action n in listAction)
                    {
                        if (n.Notes.Contains("uinfo.aspx?uid="))
                            builder.AppendFormat("{0}前{1}<br />", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(n.Notes));
                        else
                            builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}<br />", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
                    }
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?act=fans&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;更多关注动态</a><br />");
                }
                else
                {
                    builder.Append("暂时无相关记录..");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion

            #region 来访客人
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<img src=\"/Files/sys/space/fk.gif\" alt=\"load\"/>【来访客人】<br />");
            //记录来访
            if (Request["pw"] != "ok" && uid != meid && uid > 0 && meid > 0)
            {
                BCW.Model.Visitor m = new BCW.Model.Visitor();
                m.UsID = uid;
                m.UsName = model.UsName;
                m.VisitId = meid;
                m.VisitName = new BCW.BLL.User().GetUsName(meid);
                m.AddTime = DateTime.Now;

                if (!new BCW.BLL.Visitor().ExistsVisitId(uid, meid))
                {
                    new BCW.BLL.Visitor().Add(m);
                    //记录人气
                    string strId = BCW.Files.FileWord.ReadTxt("/Files/txt/" + uid + ".user", "utf-8");
                    if (!strId.Contains(meid.ToString()))
                    {
                        new BCW.BLL.User().UpdateClick(uid, 1);
                        BCW.Files.FileWord.CreateTxt("/Files/txt/" + uid + ".user", "#" + meid + "", true);
                    }

                }
                else
                {
                    new BCW.BLL.Visitor().Update(m);
                }
            }
            ds = new BCW.BLL.Visitor().GetList("Top 5 VisitId,VisitName", "UsID=" + uid + " ORDER BY AddTime DESC");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i != 0)
                        builder.Append(",");
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["VisitId"] + "") + "\">" + ds.Tables[0].Rows[i]["VisitName"] + "</a>");
                }
            }
            else
            {
                builder.Append("暂时无相关记录..");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/Files/sys/space/fk.gif\" alt=\"load\"/>【拜访客人】<br />");
            ds = new BCW.BLL.Visitor().GetList("Top 5 UsID,UsName", "VisitId=" + uid + " ORDER BY AddTime DESC");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i != 0)
                        builder.Append(",");
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"] + "") + "\">" + ds.Tables[0].Rows[i]["UsName"] + "</a>");
                }
            }
            else
            {
                builder.Append("暂时无相关记录..");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            #region 赠送的礼物
            //sgl 20161021
            if (!Utils.GetDomain().Contains("dyj6"))
            {
                //农场赠送的礼物
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/space/gift.gif\" alt=\"load\"/>【收到<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=mygift&amp;hid=" + uid + "&amp;type=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Shopuser().GetCount_nc(uid) + "</a>农场礼物】<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">送礼</a><br />");
                ds = new BCW.BLL.Shopsend().GetList("Top 2 *", "ToID=" + uid + " and pic=1 ORDER BY ID DESC");
                string GiftImg1 = string.Empty;
                string GiftNew1 = string.Empty;
                if (ds != null && ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GiftImg1 += "<img src=\"" + ds.Tables[0].Rows[i]["PrevPic"].ToString().Replace("small_", "big_") + "\" alt=\"" + ds.Tables[0].Rows[i]["Title"].ToString() + "\"/>";
                        if (i == 0)
                            GiftNew1 = "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"] + "") + "\">" + ds.Tables[0].Rows[i]["UsName"] + ":</a>送" + ds.Tables[0].Rows[i]["Title"] + "" + ds.Tables[0].Rows[i]["Total"] + "个";
                    }
                    builder.Append(GiftImg1 + "<br />" + GiftNew1 + "");
                }
                else
                {
                    builder.Append("还没收到过礼物哦,<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=3") + "\">去农场选鲜花</a>送给Ta吧..");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
            }



            //系统赠送的礼物
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/Files/sys/space/gift.gif\" alt=\"load\"/>【收到<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=mygift&amp;hid=" + uid + "&amp;type=0&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Shopuser().GetCount(uid) + "</a>礼物】<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=store&amp;backurl=" + Utils.PostPage(1) + "") + "\">送礼</a><br />");
            ds = new BCW.BLL.Shopsend().GetList("Top 2 UsID,UsName,Title,PrevPic,Total", "ToID=" + uid + " and pic=0 ORDER BY ID DESC");
            string GiftImg = string.Empty;
            string GiftNew = string.Empty;
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    GiftImg += "<img src=\"" + ds.Tables[0].Rows[i]["PrevPic"].ToString().Replace("small_", "big_") + "\" alt=\"" + ds.Tables[0].Rows[i]["Title"].ToString() + "\"/>";
                    if (i == 0)
                        GiftNew = "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"] + "") + "\">" + ds.Tables[0].Rows[i]["UsName"] + ":</a>送" + ds.Tables[0].Rows[i]["Title"] + "" + ds.Tables[0].Rows[i]["Total"] + "个";
                }
                builder.Append(GiftImg + "<br />" + GiftNew + "");
            }
            else
            {
                builder.Append("还没收到过礼物哦,<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=store&amp;backurl=" + Utils.PostPage(1) + "") + "\">买个礼物</a>送给Ta吧..");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            #region 空间状态
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<img src=\"/Files/sys/space/dl.gif\" alt=\"load\"/>【空间状态】<br />");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/usertop.aspx?act=list&amp;ptype=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">人气:" + model.Click + "/今天" + new BCW.BLL.Visitor().GetTodayCount(uid) + "/" + new BCW.BLL.User().GetClickTop(uid) + "名</a>");
            builder.Append("<br /><a href=\"" + Utils.getUrl("/bbs/usertop.aspx?act=list&amp;ptype=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">在线:" + BCW.User.Users.ChangeDayff(model.OnTime) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (uid == meid || Request["pw"] == "ok")
            {
                //预览控制
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append(Out.Tab("", ""));
                if (Request["pw"] == "ok")
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?backurl=" + Utils.getPage(0) + "") + "\">我的空间</a>.");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;pw=ok&amp;backurl=" + Utils.getPage(0) + "") + "\">预览</a>.");
                }
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/signin.aspx") + "\">签到</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?act=exit") + "\">退出登录</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">社区</a>");
            builder.Append(Out.Tab("</div>", ""));

            if (new BCW.BLL.Role().IsSumMode(meid) || new BCW.BLL.Group().GetForumId(meid) > 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/usermanage.aspx?hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;管理此会员</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion

            #endregion
        }
    }
}