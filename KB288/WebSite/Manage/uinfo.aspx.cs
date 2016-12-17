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
using BCW.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 增加中介专用链生成及复制功能
/// 黄国军 20160526
/// 
/// 增加30号管理发工资及查看社区用户权限
/// 黄国军 20160430
/// 
/// 邵广林 增加判断ky288域名
/// 20161103
/// </summary>
public partial class Manage_uinfo : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    #region 页面加载 Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string uid = Utils.GetRequest("uid", "all", 1, @"^[^\^]{1,19}$", "");
        if (uid != "")
        {
            if (Utils.IsRegex(uid, @"^[1-9]\d*$"))
            {
                if (!Utils.IsRegex(uid, @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$"))
                {
                    act = "view";
                }
            }
        }
        switch (act)
        {
            case "usubb":
                UsUbbPage();
                break;
            case "view":
                ViewPage();
                break;
            case "vip":
                VipPage();
                break;
            case "vipsave":
                VipSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "editcent":
                EditCentPage();
                break;
            case "editcentsave":
                EditCentSavePage();
                break;
            case "inpwd":
                InPwdPage();
                break;
            case "inuser":          //社区用户
                InUserPage();
                break;
            case "deltype":
                DelTypePage();
                break;
            case "del":
                DelPage();
                break;
            case "change":
                ChangePage();
                break;
            case "more":
                MorePage();
                break;
            case "verifyuser":
                VerifyUserPage();
                break;
            case "cent1":
            case "cent2":
            case "cent3":
                CentUserPage(act);
                break;
            case "wage":
                WagePage();
                break;
            case "wagesave":            //发工资
                WageSavePage();
                break;
            case "freeze":
                FreezePage();
                break;
            case "ipview":
                IpViewPage();
                break;
            case "bankuser":
                BankUserPage();
                break;
            case "bankusersave":
                BankUserSavePage();
                break;
            case "AGN":
                AGNLinkPage();
                break;
            case "finace":              //过币费率设置
                finaceSetPage();
                break;
            default:
                ReloadPage(uid);
                break;
        }
    }
    #endregion

    #region 中介专用链
    private void AGNLinkPage()
    {
        Master.Title = "中介专链";

        #region 判断中介权限
        string uid = Utils.GetRequest("id", "all", 1, @"^[^\^]{1,19}$", "");
        if (uid != "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("" + new BCW.BLL.User().GetUsName(int.Parse(uid)) + "(" + uid + ")的充值链接");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("复制UBB即可通过网银付款,该链接为" + uid + "专用");
            DataSet dssRole = new BCW.BLL.Role().GetList("RoleName", "UsID=" + uid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 AND ForumID=2 ORDER BY FORUMID ASC");
            if (dssRole.Tables.Count > 0)
            {
                if (dssRole.Tables[0].Rows.Count > 0)
                {
                    string key = new Random().Next(1, 999) + "," + uid + "CNEGA";
                    string p_Host = Utils.GetDomain().Split(':')[0];
                    builder.Append(p_Host);
                    string ekey = BCW.Common.DESEncrypt.Encrypt(key, p_Host);
                    string p_Url = "../bbs/finance.aspx?key=" + ekey.ToLower();
                    string p_Url_Full = "[url=" + p_Url + "][绿][" + uid + "中介专用充值链接][/绿][/url]";
                    builder.Append("<br />链接仅支持站内点击方式进入,复制或从外网进入均无效");
                    builder.Append("<br />" + p_Url_Full + "<br />");
                    if (Utils.Isie() || Utils.GetUA().ToLower().Contains("opera/8"))
                    {
                        builder.Append(new BCW.JS.somejs().CopyToClipboard());
                        builder.Append("<input id='url' name='url' type='hidden' value='" + p_Url_Full + "' />");
                        builder.Append("<input name=\"\" type=\"button\" value=\"复制\" onClick=\"copyToClipboard(url.value)\" />");
                    }
                }
                else
                {
                    Utils.Error("ID:" + uid + "尚无中介权限,链接生成失败", "");
                }
            }
            else
            {
                Utils.Error("ID:" + uid + "尚无中介权限,链接生成失败", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + uid + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Utils.Error("ID为空,页面错误", "");
        }
        #endregion
    }
    #endregion

    #region 会员管理 ReloadPage
    private void ReloadPage(string uid)
    {
        Master.Title = "会员管理";
        int otype = int.Parse(Utils.GetRequest("otype", "get", 1, @"^[0-2]$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-8]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("会员管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        string strVal = string.Empty;
        if (uid == "")
        {
            if (ptype == 0)
                builder.Append("全部|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=0") + "\">全部</a>|");

            if (ptype == 1)
                builder.Append("美女|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=1") + "\">美女</a>|");

            if (ptype == 2)
                builder.Append("帅哥|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=2") + "\">帅哥</a>|");

            if (ptype == 3)
                builder.Append("VIP|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=3") + "\">VIP</a>|");

            if (ptype == 4)
                builder.Append("在线|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=4") + "\">在线</a>|");

            if (ptype >= 5 && ptype < 8)
                builder.Append("认证|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=5") + "\">认证</a>|");

            if (ptype == 8)
                builder.Append("冻结");
            else
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=8") + "\">冻结</a>");

            if (ptype >= 5 && ptype < 8)
            {
                if (ptype == 5)
                    builder.Append("<br />已验证|");
                else
                    builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?ptype=5") + "\">已验证</a>|");

                if (ptype == 6)
                    builder.Append("未验证|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=6") + "\">未验证</a>|");

                if (ptype == 7)
                    builder.Append("待邮箱验证");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=7") + "\">待邮箱验证</a>");
            }
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?ptype=0") + "\">全部</a>|");
            if (Utils.IsRegex(uid, @"^(?:13|14|15|18)\d{9}$"))
            {
                builder.Append("手机号搜索");
                strVal = "Mobile='" + uid + "'";
            }
            else if (Ipaddr.IsIPAddress(uid))
            {
                builder.Append("最后IP搜索");
                strVal = "EndIP='" + uid + "'";
            }
            else
            {
                builder.Append("昵称搜索");
                strVal = "UsName LIKE '%" + uid + "%'";
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "uid", "ptype", "otype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid != "")
        {
            strWhere = strVal;
        }
        else
        {
            if (ptype == 1)
                strWhere = "Sex<=1";

            else if (ptype == 2)
                strWhere = "Sex=2";

            else if (ptype == 3)
                strWhere = "VipDate IS NOT NULL and VipDate>'" + DateTime.Now + "'";

            else if (ptype == 4)
                strWhere = "EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ";

            else if (ptype == 5)
                strWhere = "IsVerify=1";

            else if (ptype == 6)
                strWhere = "IsVerify=0";

            else if (ptype == 7)
                strWhere = "IsVerify=2";

            else if (ptype == 8)
                strWhere = "IsFreeze=1";
        }
        if (otype == 0)
            strOrder = "EndTime DESC";
        else if (otype == 1)
            strOrder = "RegTime ASC";
        else if (otype == 2)
            strOrder = "RegTime DESC";

        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsersManage(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.ID + ")</a>");
                if (ptype == 7)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("uinfo.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a>]");
                }
                if (ptype == 3)
                {
                    builder.Append(BCW.User.Users.SetVip(n.ID));
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

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("排序:");
        if (otype == 0)
            builder.Append("最后在线|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?otype=0&amp;ptype=" + ptype + "&amp;uid=" + uid + "") + "\">最后在线</a>|");

        if (otype == 1)
            builder.Append("注册正序|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?otype=1&amp;ptype=" + ptype + "&amp;uid=" + uid + "") + "\">注册正序</a>|");

        if (otype == 2)
            builder.Append("注册倒序");
        else
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?otype=2&amp;ptype=" + ptype + "&amp;uid=" + uid + "") + "\">注册倒序</a>");

        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入ID|手机号|最后IP:/,,";
        string strName = "uid,ptype";
        string strType = "text,hidden";
        string strValu = "'" + ptype + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜会员," + Utils.getUrl("uinfo.aspx,post") + ",1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("reg.aspx") + "\">注册新会员</a><br />");
        if (Utils.GetDomain().Contains("ky288") || Utils.GetDomain().Contains("boyi929") || Utils.GetDomain().Contains("dyj6") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
        {
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=more") + "\">快捷功能操作</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("app/setline.aspx?act=iplogin") + "\">IP异常与查币</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=wage") + "\">发放会员工资</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx") + "\">密保更改说明</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=finace") + "\">过币费率管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 查看会员 ViewPage
    private void ViewPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.User model = new BCW.BLL.User().GetBasic(uid);
        if (model == null)
        {
            Utils.Error("不存在的会员ID", "");
        }
        Master.Title = "管理" + model.UsName + "(" + uid + ")";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理:" + model.UsName + "(" + uid + ")");
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
                        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + RoleName + "</a>管辖全站<br />");
                    }
                    else if (forumid == 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + RoleName + "</a>管辖全区版块<br />");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;id=" + id + "&amp;hid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + RoleName + "</a>管辖<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumName + "</a><br />");
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

        builder.Append("VIP等级:<a href=\"" + Utils.getUrl("uinfo.aspx?act=vip&amp;hid=" + uid + "") + "\">" + BCW.User.Users.SetVip(uid) + "</a>");
        builder.Append("<br />登录状态:" + BCW.User.Users.UserOnline(uid, model.State, model.EndTime) + "");
        builder.Append("<br />最后在线:" + DT.FormatDate(model.EndTime, 0) + "");
        builder.Append("<br />帐户状态:");
        if (new BCW.BLL.User().GetIsFreeze(uid) == 1)
            builder.Append("已冻结<a href=\"" + Utils.getUrl("uinfo.aspx?act=freeze&amp;id=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[解冻]</a>");
        else
            builder.Append("正常<a href=\"" + Utils.getUrl("uinfo.aspx?act=freeze&amp;id=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[冻结]</a>");

        builder.Append("<br />日志记录:<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">过户</a>|<a href=\"" + Utils.getUrl("forumlog.aspx?act=xview&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">消费</a>");


        string ipCity = string.Empty;
        if (!string.IsNullOrEmpty(model.EndIP))
        {
            ipCity = new IPSearch().GetAddressWithIP(model.EndIP);
            if (!string.IsNullOrEmpty(ipCity))
            {
                ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
            }
        }
        builder.Append("<br />最后IP:" + ipCity + "<a href=\"" + Utils.getUrl("uinfo.aspx?act=ipview&amp;id=" + uid + "") + "\">" + model.EndIP + "</a>");
        builder.Append("<br />~~~~~~~<br />" + ub.Get("SiteBz") + ":" + model.iGold + "");
        builder.Append("<br />银行:" + model.iBank + "");
        builder.Append("<br />" + ub.Get("SiteBz2") + ":" + model.iMoney + "<br />~~~~~~~");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?uid=" + uid + "") + "\">短消息(" + new BCW.BLL.Guest().GetIDCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=1") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?uid=" + uid + "") + "\">空间留言(" + new BCW.BLL.Mebook().GetIDCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=2") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?uid=" + uid + "") + "\">论坛帖子(" + new BCW.BLL.Forumstat().GetIDCount(uid, 1) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=3") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?uid=" + uid + "") + "\">论坛回帖(" + new BCW.BLL.Forumstat().GetIDCount(uid, 2) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=4") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?uid=" + uid + "") + "\">聊天发言(" + new BCW.BLL.ChatText().GetCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=5") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + uid + "") + "\">空间日记(" + new BCW.BLL.Diary().GetCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=6") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?uid=" + uid + "") + "\">相册文件(" + new BCW.BLL.Upfile().GetCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=7") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx") + "\">网站评论(" + new BCW.BLL.Comment().GetCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=8") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx") + "\">社区评论(" + new BCW.BLL.FComment().GetCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=9") + "\">清</a>]<br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx") + "\">游戏闲聊(" + new BCW.BLL.Speak().GetCount(uid) + ")</a>[<a href=\"" + Utils.getUrl("uinfo.aspx?act=deltype&amp;hid=" + uid + "&amp;ptype=10") + "\">清</a>]");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.LHr()));

        string strText = ",,,,,";
        string strName = "Content,uid,act,info,backurl";
        string strType = "text,hidden,hidden,hidden,hidden";
        string strValu = "'" + uid + "'add'ok'" + Utils.PostPage(1) + "";
        string strEmpt = "true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "发送消息,guest.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览空间</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?act=view&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看资料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=edit&amp;hid=" + uid + "") + "\">编辑资料</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=me&amp;hid=" + uid + "") + "\">勋章</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=vip&amp;hid=" + uid + "") + "\">VIP</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?hid=" + uid + "") + "\">自身权限</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + uid + "") + "\">管理权限</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=inpwd&amp;hid=" + uid + "") + "\">重置密码</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=inuser&amp;hid=" + uid + "") + "\">用户社区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=del&amp;id=" + uid + "") + "\">删除会员</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=change&amp;id=" + uid + "") + "\">转移数据</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=usubb&amp;id=" + uid + "") + "\">前台身份</a> ");

        #region 判断中介权限
        DataSet dssRole = new BCW.BLL.Role().GetList("RoleName", "UsID=" + uid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 AND ForumID=2 ORDER BY FORUMID ASC");
        if (dssRole.Tables.Count > 0)
        {
            if (dssRole.Tables[0].Rows.Count > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=AGN&amp;id=" + uid + "") + "\">中介专链</a> ");
            }
        }
        #endregion

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 冻结会员 FreezePage
    private void FreezePage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.User().Exists(id))
        {
            Utils.Error("不存在的会员记录", "");
        }
        int IsFreeze = new BCW.BLL.User().GetIsFreeze(id);
        if (info == "")
        {
            Master.Title = "冻结会员";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("对象:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(id) + "(" + id + ")</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            if (IsFreeze == 1)
                builder.Append("帐户状态:已冻结<br /><a href=\"" + Utils.getUrl("uinfo.aspx?info=ok&amp;act=freeze&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">立即解冻</a>");
            else
                builder.Append("帐户状态:正常<br /><a href=\"" + Utils.getUrl("uinfo.aspx?info=ok&amp;act=freeze&amp;id=" + id + "&amp;dj=1&amp;backurl=" + Utils.getPage(0) + "") + "\">立即冻结</a>");

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:会员帐户被冻结后，将不能进行任何消费(币只能入不能出)");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?act=view&amp;uid=" + id + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (IsFreeze == 1)
            {
                new BCW.BLL.User().UpdateIsFreeze(id, 0);
                new BCW.BLL.Guest().Add(id, new BCW.BLL.User().GetUsName(id), "系统管理员已将您的帐户解除冻结，您恢复社区消费了，如有问题请联系管理员！");
                Utils.Success("解除冻结", "解除冻结成功..", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + id + ""), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateIsFreeze(id, 1);
                new BCW.BLL.Guest().Add(id, new BCW.BLL.User().GetUsName(id), "系统管理员已将您的帐户冻结，在你的帐户被冻结期间，你将不能进行任何消费，如有问题请联系管理员！");
                Utils.Success("冻结", "冻结成功..", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + id + ""), "1");
            }
        }
    }
    #endregion

    #region 编辑会员 EditPage
    private void EditPage()
    {
        Master.Title = "编辑会员";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.User model = new BCW.BLL.User().GetBasic(hid);
        if (model == null)
        {
            Utils.Error("不存在的会员记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑会员");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "手机号:,昵称:,性别:,注册时间:,注册IP:,城市:,生日:,邮箱:,积分:,等级:,人气:,在线时间(分钟):,状态:,签到次数:,是否验证,,,";
        string strName = "Mobile,UsName,Sex,RegTime,RegIP,City,Birth,Email,iScore,Leven,Click,OnTime,State,SignTotal,IsVerify,hid,act,backurl";
        string strType = "text,text,select,date,text,text,text,text,num,num,num,num,select,num,select,hidden,hidden,hidden";
        string strValu = "" + model.Mobile + "'" + model.UsName + "'" + model.Sex + "'" + DT.FormatDate(model.RegTime, 0) + "'" + model.RegIP + "'" + model.City + "'" + DT.FormatDate(model.Birth, 11) + "'" + model.Email + "'" + model.iScore + "'" + model.Leven + "'" + model.Click + "'" + model.OnTime + "'" + model.State + "'" + model.SignTotal + "'" + model.IsVerify + "'" + hid + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,0|未设置|1|美女|2|帅哥,false,false,false,false,true,false,false,false,false,0|正常|1|隐身,false,0|未验证|1|已验证|2|待邮箱,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,uinfo.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=bankuser&amp;hid=" + hid + "") + "\">编辑银行资料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=editcent&amp;hid=" + hid + "") + "\">编辑币额</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 保存会员 EditSavePage
    private void EditSavePage()
    {
        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        string Mobile = Utils.GetRequest("Mobile", "post", 2, @"^[0-9]\d{5,11}$", "手机号填写错误");
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[\s\S]{1,50}$", "昵称填写错误");
        if (UsName.Contains(" "))
        {
            Utils.Error("昵称填写错误", "");
        }
        if (UsName.Contains("##"))
        {
            Utils.Error("昵称不能使用##", "");
        }
        int Sex = int.Parse(Utils.GetRequest("Sex", "post", 2, @"^[0-2]$", "性别选择错误"));
        DateTime RegTime = Utils.ParseTime(Utils.GetRequest("RegTime", "post", 2, DT.RegexTime, "注册时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        string RegIP = Utils.GetRequest("RegIP", "post", 2, @"^[\s\S]{1,15}$", "注册IP填写错误");
        if (!Ipaddr.IsIPAddress(RegIP))
        {
            Utils.Error("注册IP填写错误", "");
        }
        string City = Utils.GetRequest("City", "post", 3, @"^[\s\S]{1,10}$", "城市填写错误");
        DateTime Birth = Utils.ParseTime(Utils.GetRequest("Birth", "post", 2, DT.RegexDate, "生日填写错误或者不合理"));
        string Email = Utils.GetRequest("Email", "post", 1, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "@");
        if (DateTime.Now.Year - Birth.Year < 10 || DateTime.Now.Year - Birth.Year > 100)
        {
            Utils.Error("生日填写错误或者不合理", "");
        }
        long iScore = Int64.Parse(Utils.GetRequest("iScore", "post", 4, @"^[0-9]\d*$", "积分填写错误"));
        int Leven = int.Parse(Utils.GetRequest("Leven", "post", 2, @"^[0-9]\d*$", "等级填写错误"));
        int Click = int.Parse(Utils.GetRequest("Click", "post", 2, @"^[0-9]\d*$", "人气填写错误"));
        int OnTime = int.Parse(Utils.GetRequest("OnTime", "post", 2, @"^[0-9]\d*$", "在线时间填写错误"));
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-1]$", "状态选择错误"));
        int SignTotal = int.Parse(Utils.GetRequest("SignTotal", "post", 2, @"^[0-9]\d*$", "签到次数填写错误"));
        int IsVerify = int.Parse(Utils.GetRequest("IsVerify", "post", 2, @"^[0-2]$", "会员验证选择错误"));
        BCW.Model.User model = new BCW.Model.User();
        model.ID = hid;
        model.Mobile = Mobile;
        model.UsName = UsName;
        model.Sex = Sex;
        model.RegTime = RegTime;
        model.RegIP = RegIP;
        model.City = City;
        model.Birth = Birth;
        model.Email = Email;
        model.iScore = iScore;
        model.Leven = Leven;
        model.Click = Click;
        model.OnTime = OnTime;
        model.State = State;
        model.SignTotal = SignTotal;
        model.IsVerify = IsVerify;
        new BCW.BLL.User().Update(model);

        Utils.Success("编辑会员", "编辑会员成功..", Utils.getUrl("uinfo.aspx?act=edit&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
    #endregion

    #region 设置银行资料 BankUserPage
    private void BankUserPage()
    {
        Master.Title = "设置银行资料";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.BankUser model = new BCW.BLL.BankUser().GetBankUser(hid);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改银行");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "真实姓名(与银行帐号一致，不可修改):/,银行名称1(如工商银行):/,银行卡帐号1:/,开户行地址1(可留空):/,银行名称2(可留空):/,银行卡帐号2(可留空):/,开户行地址2(可留空):/,银行名称3(可留空):/,银行卡帐号3(可留空):/,开户行地址3(可留空):/,银行名称4(可留空):/,银行卡帐号4(可留空):/,开户行地址4(可留空):/,支付宝名称(可留空):/,支付宝帐号(可留空):/,,";
        string strName = "BankName,BankTitle1,BankNo1,BankAdd1,BankTitle2,BankNo2,BankAdd2,BankTitle3,BankNo3,BankAdd3,BankTitle4,BankNo4,BankAdd4,ZFBName,ZFBNo,hid,act";
        string strType = "";
        string strValu = "";
        if (model != null)
        {
            strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
            strValu = "" + model.BankName + "'" + model.BankTitle1 + "'" + model.BankNo1 + "'" + model.BankAdd1 + "'" + model.BankTitle2 + "'" + model.BankNo2 + "'" + model.BankAdd2 + "'" + model.BankTitle3 + "'" + model.BankNo3 + "'" + model.BankAdd3 + "'" + model.BankTitle4 + "'" + model.BankNo4 + "'" + model.BankAdd4 + "'" + model.ZFBName + "'" + model.ZFBNo + "'" + hid + "'bankusersave";
        }
        else
        {
            strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
            strValu = "'''''''''''''''" + hid + "'bankusersave";
        }
        string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false";
        string strIdea = "/";
        string strOthe = "确定修改,uinfo.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("uinfo.aspx?act=edit&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    #endregion

    #region 修改银行资料 BankUserSavePage
    private void BankUserSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("你的权限不足", "");
        }

        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        string BankName = Utils.GetRequest("BankName", "post", 2, @"^[\u4e00-\u9fa5]{2,10}$", "真实姓名必须是中文，限2-10个字符");

        string BankTitle1 = Utils.GetRequest("BankTitle1", "post", 3, @"^[\s\S]{2,20}$", "银行名称1限2-20个字符");
        string BankNo1 = Utils.GetRequest("BankNo1", "post", 3, @"^[0-9]{10,30}$", "银行卡帐号1填写错误");
        string BankAdd1 = Utils.GetRequest("BankAdd1", "post", 3, @"^[\s\S]{1,50}$", "银行卡开户地址1填写错误，可留空");

        string BankTitle2 = Utils.GetRequest("BankTitle2", "post", 3, @"^[\s\S]{2,20}$", "银行名称2限2-20个字符，可留空");
        string BankNo2 = Utils.GetRequest("BankNo2", "post", 3, @"^[0-9]{10,30}$", "银行卡帐号2填写错误，可留空");
        string BankAdd2 = Utils.GetRequest("BankAdd2", "post", 3, @"^[\s\S]{1,50}$", "银行卡开户地址2填写错误，可留空");

        string BankTitle3 = Utils.GetRequest("BankTitle3", "post", 3, @"^[\s\S]{2,20}$", "银行名称3限2-20个字符，可留空");
        string BankNo3 = Utils.GetRequest("BankNo3", "post", 3, @"^[0-9]{10,30}$", "银行卡帐号3填写错误，可留空");
        string BankAdd3 = Utils.GetRequest("BankAdd3", "post", 3, @"^[\s\S]{1,50}$", "银行卡开户地址3填写错误，可留空");
        string BankTitle4 = Utils.GetRequest("BankTitle4", "post", 3, @"^[\s\S]{2,20}$", "银行名称4限2-20个字符，可留空");
        string BankNo4 = Utils.GetRequest("BankNo4", "post", 3, @"^[0-9]{10,30}$", "银行卡帐号4填写错误，可留空");
        string BankAdd4 = Utils.GetRequest("BankAdd4", "post", 3, @"^[\s\S]{1,50}$", "银行卡开户地址4填写错误，可留空");

        string ZFBName = Utils.GetRequest("ZFBName", "post", 3, @"^[\s\S]{1,30}$", "支付宝名称限30个字符内，可留空");
        string ZFBNo = Utils.GetRequest("ZFBNo", "post", 3, @"^[\s\S]{1,50}$", "支付宝名称限50个字符内，可留空");

        BCW.Model.BankUser model = new BCW.Model.BankUser();
        model.UsID = hid;
        model.BankName = BankName;
        model.BankTitle1 = BankTitle1;
        model.BankNo1 = BankNo1;
        model.BankAdd1 = BankAdd1;
        model.BankTitle2 = BankTitle2;
        model.BankNo2 = BankNo2;
        model.BankAdd2 = BankAdd2;
        model.BankTitle3 = BankTitle3;
        model.BankNo3 = BankNo3;
        model.BankAdd3 = BankAdd3;
        model.BankTitle4 = BankTitle4;
        model.BankNo4 = BankNo4;
        model.BankAdd4 = BankAdd4;
        model.ZFBName = ZFBName;
        model.ZFBNo = ZFBNo;
        model.State = 0;
        if (!new BCW.BLL.BankUser().Exists(hid))
        {
            new BCW.BLL.BankUser().Add(model);
        }
        else
        {
            new BCW.BLL.BankUser().Update(model);
        }


        Utils.Success("修改银行资料", "恭喜，修改银行资料成功，正在返回..", Utils.getUrl("uinfo.aspx?act=bankuser&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
    }
    #endregion
    private void EditCentPage()
    {
        if (Utils.GetDomain().Contains("boyi929"))
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId != 1)
            {
                Utils.Error("你的权限不足", "");
            }
        }

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]\d*$", "0"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.User model = new BCW.BLL.User().GetBasic(hid);
        if (model == null)
        {
            Utils.Error("不存在的会员记录", "");
        }
        Master.Title = "编辑" + model.UsName + "(" + hid + ")币额";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
            builder.Append("编辑" + ub.Get("SiteBz2") + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=editcent&amp;hid=" + hid + "&amp;ptype=0") + "\">编辑" + ub.Get("SiteBz2") + "</a>|");

        if (ptype == 1)
            builder.Append("编辑" + ub.Get("SiteBz") + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=editcent&amp;hid=" + hid + "&amp;ptype=1") + "\">编辑" + ub.Get("SiteBz") + "</a>|");

        if (ptype == 2)
            builder.Append("编辑银行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=editcent&amp;hid=" + hid + "&amp;ptype=2") + "\">编辑银行</a>");

        builder.Append(Out.Tab("</div>", ""));
        if (ptype == 1)
        {
            string strText = "" + ub.Get("SiteBz") + ":,是否通知会员:/,,,,";
            string strName = "iGold,IsMsg,hid,ptype,act,backurl";
            string strType = "num,select,hidden,hidden,hidden,hidden";
            string strValu = "" + model.iGold + "'1'" + hid + "'" + ptype + "'editcentsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,1|通知|0|不通知,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定编辑,uinfo.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 2)
        {
            string strText = "银行" + ub.Get("SiteBz") + ":,是否通知会员:/,,,,";
            string strName = "iBank,IsMsg,hid,ptype,act,backurl";
            string strType = "num,select,hidden,hidden,hidden,hidden";
            string strValu = "" + model.iBank + "'1'" + hid + "'" + ptype + "'editcentsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,1|通知|0|不通知,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定编辑,uinfo.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string strText = "" + ub.Get("SiteBz2") + ":,是否通知会员:/,,,,";
            string strName = "iMoney,IsMsg,hid,ptype,act,backurl";
            string strType = "num,select,hidden,hidden,hidden,hidden";
            string strValu = "" + model.iMoney + "'1'" + hid + "'" + ptype + "'editcentsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,1|通知|0|不通知,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定编辑,uinfo.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("uinfo.aspx?act=edit&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void EditCentSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();

        if (Utils.GetDomain().Contains("ky288") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            if (ManageId != 1 && ManageId != 11)
            {
                Utils.Error("你的权限不足", "");
            }
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 1, @"^[0-2]\d*$", "0"));
        int IsMsg = int.Parse(Utils.GetRequest("IsMsg", "post", 1, @"^[0-1]$", "0"));
        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        string mename = new BCW.BLL.User().GetUsName(hid);
        if (ptype == 1)
        {

            long iGold = Int64.Parse(Utils.GetRequest("iGold", "post", 4, @"^[0-9]\d*$", "" + ub.Get("SiteBz") + "填写错误"));
            long tGold = new BCW.BLL.User().GetGold(hid);
            if (!iGold.Equals(tGold))
            {
                if (IsMsg == 1)
                {
                    new BCW.BLL.Guest().Add(hid, mename, "后台管理员" + ManageId + "号编辑您的" + ub.Get("SiteBz") + "，从" + tGold + "修改为" + iGold + "，如有问题请联系管理员！");
                }
                else
                {
                    IsMsg = 2;
                }
                //更新消费记录
                BCW.Model.Goldlog model = new BCW.Model.Goldlog();
                model.BbTag = IsMsg;
                model.Types = 0;
                model.PUrl = Utils.getPageUrl();//操作的文件名
                model.UsId = hid;
                model.UsName = mename;
                model.AcGold = (iGold - tGold);
                model.AfterGold = iGold;//更新后的元数
                model.AcText = "后台管理员" + ManageId + "号操作";
                model.AddTime = DateTime.Now;
                new BCW.BLL.Goldlog().Add(model);
            }
            SqlHelper.ExecuteSql("Update tb_User set iGold=" + iGold + " where id=" + hid + "");
        }
        else if (ptype == 2)
        {

            long iBank = Int64.Parse(Utils.GetRequest("iBank", "post", 4, @"^[0-9]\d*$", "银行" + ub.Get("SiteBz") + "填写错误"));
            long tBank = new BCW.BLL.User().GetBank(hid);
            if (!iBank.Equals(tBank))
            {
                if (IsMsg == 1)
                {
                    new BCW.BLL.Guest().Add(hid, mename, "后台管理员" + ManageId + "号编辑您的银行" + ub.Get("SiteBz") + "存款，从" + tBank + "修改为" + iBank + "，如有问题请联系管理员！");
                }
                else
                {
                    IsMsg = 2;
                }
                //更新消费记录
                BCW.Model.Goldlog model = new BCW.Model.Goldlog();
                model.BbTag = IsMsg;
                model.Types = 0;
                model.PUrl = Utils.getPageUrl();//操作的文件名
                model.UsId = hid;
                model.UsName = mename;
                model.AcGold = (iBank - tBank);
                model.AfterGold = iBank;//更新后的元数
                model.AcText = "后台管理员" + ManageId + "号操作你的银行存款";
                model.AddTime = DateTime.Now;
                new BCW.BLL.Goldlog().Add(model);
            }
            SqlHelper.ExecuteSql("Update tb_User set iBank=" + iBank + " where id=" + hid + "");
        }
        else
        {
            long iMoney = Int64.Parse(Utils.GetRequest("iMoney", "post", 4, @"^[0-9]\d*$", "" + ub.Get("SiteBz2") + "填写错误"));
            long tMoney = new BCW.BLL.User().GetMoney(hid);
            if (!iMoney.Equals(tMoney))
            {
                if (IsMsg == 1)
                {
                    new BCW.BLL.Guest().Add(hid, mename, "后台管理员" + ManageId + "号编辑您的" + ub.Get("SiteBz2") + "，从" + tMoney + "修改为" + iMoney + "，如有问题请联系管理员！");
                }
                else
                {
                    IsMsg = 2;
                }
                //更新消费记录
                BCW.Model.Goldlog model = new BCW.Model.Goldlog();
                model.BbTag = IsMsg;
                model.Types = 1;
                model.PUrl = Utils.getPageUrl();//操作的文件名
                model.UsId = hid;
                model.UsName = mename;
                model.AcGold = (iMoney - tMoney);
                model.AfterGold = iMoney;//更新后的元数
                model.AcText = "后台管理员" + ManageId + "号操作";
                model.AddTime = DateTime.Now;
                new BCW.BLL.Goldlog().Add(model);
            }
            SqlHelper.ExecuteSql("Update tb_User set iMoney=" + iMoney + " where id=" + hid + "");
        }
        Utils.Success("编辑会员币额", "编辑会员币额成功..", Utils.getUrl("uinfo.aspx?act=editcent&amp;hid=" + hid + "&amp;ptype=" + ptype + ""), "1");
    }
    private void VipPage()
    {
        Master.Title = "编辑会员VIP";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.User model = new BCW.BLL.User().GetVipData(hid);
        if (model == null)
        {
            Utils.Error("不存在的会员记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑会员VIP");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "总成长值:/,到期时间:/,每天成长值:/,,,";
        string strName = "VipGrow,VipDate,VipDayGrow,hid,act,backurl";
        string strType = "num,date,num,hidden,hidden,hidden";
        string strValu = "" + model.VipGrow + "'" + model.VipDate + "'" + model.VipDayGrow + "'" + hid + "'vipsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,uinfo.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回</a>");
        builder.Append("<br />到期时间格式:" + DT.FormatDate(DateTime.Now, 0) + "<br />大于当前时间即为VIP会员，否则非VIP或过期");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void VipSavePage()
    {
        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        int VipGrow = int.Parse(Utils.GetRequest("VipGrow", "post", 2, @"^[0-9]\d*$", "总成长值填写错误"));
        //已重写原有COMM日期正则 黄国军 20160621
        string regextime = @"^(((20[0-3][0-9]-(0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|(20[0-3][0-9]-(0[2469]|11)-(0[1-9]|[12][0-9]|30))) (20|21|22|23|[0-1][0-9]):[0-5][0-9]:[0-5][0-9])$";
        DateTime VipDate = Utils.ParseTime(Utils.GetRequest("VipDate", "post", 2, regextime, "到期时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int VipDayGrow = int.Parse(Utils.GetRequest("VipDayGrow", "post", 2, @"^[0-9]\d*$", "每天成长值填写错误"));

        new BCW.BLL.User().UpdateVipData(hid, VipDayGrow, VipDate, VipGrow);
        //清缓存
        string CacheKey = CacheName.App_UserVip(hid);
        DataCache.RemoveByPattern(CacheKey);
        Utils.Success("编辑会员VIP", "编辑会员VIP成功..", Utils.getUrl("uinfo.aspx?act=vip&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
    private void InPwdPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string hid = Utils.GetRequest("hid", "get", 2, @"^(?:13|14|15|18)\d{9}$|^[0-9]\d*$", "ID或手机号错误");
        if (info == "")
        {
            Master.Title = "重置密码";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请选择重置密码类型");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok1&amp;act=inpwd&amp;hid=" + hid + "") + "\">重置登录密码(随机生成)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok2&amp;act=inpwd&amp;hid=" + hid + "") + "\">重置支付密码(随机生成)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok5&amp;act=inpwd&amp;hid=" + hid + "") + "\">重置管理密码(随机生成)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok3&amp;act=inpwd&amp;hid=" + hid + "") + "\">重置登录密码(手机后6位)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok4&amp;act=inpwd&amp;hid=" + hid + "") + "\">重置支付密码(手机后6位)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok6&amp;act=inpwd&amp;hid=" + hid + "") + "\">重置管理密码(手机后6位)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=resetques&amp;hid=" + hid + "") + "\">重置密码保护问题答案(手机后六位)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=resetques&amp;info=ok&amp;hid=" + hid + "") + "\">清除该账号密保数据（需用户重设）</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            int ihid = 0;
            if (Utils.IsRegex(hid, @"^(?:13|14|15|18)\d{9}$"))
            {
                ihid = new BCW.BLL.User().GetID(hid);

            }
            else
            {
                ihid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "ID错误"));

            }

            if (ihid == 19611 || ihid == 112233)
            {
                Utils.Error("你已越权操作", "");
            }

            if (!new BCW.BLL.User().Exists(ihid))
            {
                Utils.Error("不存在的记录", "");
            }
            string Mobile = new Rand().RandNumer(10);
            if (info == "ok3" || info == "ok4" || info == "ok6")
            {
                Mobile = new BCW.BLL.User().GetMobile(ihid);
            }

            if (info == "ok1" || info == "ok3")
            {
                new BCW.BLL.User().UpdateUsPwd(ihid, Utils.MD5Str(Utils.Right(Mobile, 6)));
                Utils.Success("重置登录密码", "重置登录密码成功，ID:" + ihid + "，新登录密码为" + Utils.Right(Mobile, 6) + "", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + ihid + ""), "3");
            }
            else if (info == "ok2" || info == "ok4")
            {
                new BCW.BLL.User().UpdateUsPled(ihid, Utils.MD5Str(Utils.Right(Mobile, 6)));
                Utils.Success("重置支付密码", "重置支付密码成功，ID:" + ihid + "，新支付密码为" + Utils.Right(Mobile, 6) + "", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + ihid + ""), "3");
            }
            else if (info == "ok5" || info == "ok6")
            {
                //判断是否为管理员/版主
                if (!new BCW.BLL.Role().IsAllMode(ihid) && new BCW.BLL.Group().GetForumId(ihid) == 0)
                {
                    Utils.Error("该会员不是管理者", "");
                }
                new BCW.BLL.User().UpdateUsAdmin(ihid, Utils.MD5Str(Utils.Right(Mobile, 6)));
                Utils.Success("重置管理密码", "重置管理密码成功，ID:" + ihid + "，新管理密码为" + Utils.Right(Mobile, 6) + "", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + ihid + ""), "3");
            }
        }
    }
    private void MorePage()
    {
        Master.Title = "快捷功能操作";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("快捷功能");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "ID或手机号:/,,,,";
        string strName = "hid,act,info,backurl";
        string strType = "num,hidden,hidden,hidden,hidden";
        string strValu = "'inpwd'ok1'" + Utils.PostPage(1) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "重置登录密码(随机生成),uinfo.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        strText = "ID或手机号:/,,,,";
        strName = "hid,act,info,backurl";
        strType = "num,hidden,hidden,hidden,hidden";
        strValu = "'inpwd'ok2'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "重置支付密码(随机生成),uinfo.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        strText = "ID或手机号:/,,,,";
        strName = "hid,act,info,backurl";
        strType = "num,hidden,hidden,hidden,hidden";
        strValu = "'inpwd'ok3'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "重置登录密码(手机后6位),uinfo.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        strText = "ID或手机号:/,,,,";
        strName = "hid,act,info,backurl";
        strType = "num,hidden,hidden,hidden,hidden";
        strValu = "'inpwd'ok4'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "重置支付密码(手机后6位),uinfo.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        strText = "ID或手机号:/,";
        strName = "uid,act";
        strType = "num,hidden";
        strValu = "'cleanbyid";
        strEmpt = "false,false";
        strIdea = "/";
        strOthe = "删除密保密码,BackMoreQuestion.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        strText = "ID或手机号:/,,,";
        strName = "hid,act,backurl";
        strType = "num,hidden,hidden,hidden";
        strValu = "'verifyuser'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "验证成为会员,uinfo.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("~金融操作~");
        builder.Append(Out.Tab("</div>", ""));

        strText = "会员ID:/," + ub.Get("SiteBz") + "数量:/,原因:/,,";
        strName = "hid,iGold,Content,act,backurl";
        strType = "num,num,textarea,hidden,hidden";
        strValu = "'''cent1'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "确定扣除,uinfo.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        strText = "会员ID:/," + ub.Get("SiteBz2") + "数量:/,原因:/,,";
        strName = "hid,iMoney,Content,act,backurl";
        strType = "num,num,textarea,hidden,hidden";
        strValu = "'''cent2'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "确定扣除,uinfo.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        //strText = "会员ID:/," + ub.Get("SiteBz2") + "数量:/,附言:/,,";
        //strName = "hid,iMoney2,Content,act,backurl";
        //strType = "num,num,textarea,hidden,hidden";
        //strValu = "'''cent3'" + Utils.PostPage(1) + "";
        //strEmpt = "false,false,false,false,false";
        //strIdea = "/";
        //strOthe = "确定赠送,uinfo.aspx,post,1,red";
        //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void VerifyUserPage()
    {
        string hid = Utils.GetRequest("hid", "post", 2, @"^(?:13|14|15|18)\d{9}$|^[0-9]\d*$", "ID或手机号错误");
        int ihid = 0;
        if (Utils.IsRegex(hid, @"^(?:13|14|15|18)\d{9}$"))
        {
            ihid = new BCW.BLL.User().GetID(hid);

        }
        else
        {
            ihid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[0-9]\d*$", "ID错误"));

        }
        if (!new BCW.BLL.User().Exists(ihid))
        {
            Utils.Error("不存在的记录", "");
        }
        int IsVerify = new BCW.BLL.User().GetIsVerify(ihid);
        if (IsVerify == 1)
        {
            Utils.Error("此会员已是验证会员", "");
        }
        string Mobile = new BCW.BLL.User().GetMobile(ihid);
        new BCW.BLL.User().UpdateIsVerify(Mobile, 1);
        //积分操作
        new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, ihid);

        Utils.Success("验证会员", "验证会员成功，ID:" + ihid + "", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + ihid + ""), "3");

    }
    private void CentUserPage(string act)
    {
        string hid = Utils.GetRequest("hid", "post", 2, @"^(?:13|14|15|18)\d{9}$|^[0-9]\d*$", "ID或手机号错误");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,50}$", "原因或留言限1-50字");
        long iGold = 0;
        long iMoney = 0;
        long iMoney2 = 0;
        if (act == "cent1")
            iGold = Int64.Parse(Utils.GetRequest("iGold", "post", 4, @"^[0-9]\d*$", "" + ub.Get("SiteBz") + "错误"));
        else if (act == "cent2")
            iMoney = Int64.Parse(Utils.GetRequest("iMoney", "post", 4, @"^[0-9]\d*$", "" + ub.Get("SiteBz2") + "错误"));
        else
            iMoney2 = Int64.Parse(Utils.GetRequest("iMoney2", "post", 4, @"^[0-9]\d*$", "" + ub.Get("SiteBz2") + "错误"));

        int ihid = 0;
        if (Utils.IsRegex(hid, @"^(?:13|14|15|18)\d{9}$"))
        {
            ihid = new BCW.BLL.User().GetID(hid);

        }
        else
        {
            ihid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[0-9]\d*$", "ID错误"));

        }
        if (!new BCW.BLL.User().Exists(ihid))
        {
            Utils.Error("不存在的记录", "");
        }
        //得到系统号余币
        long sysmoney = 0;
        if (act == "cent3")
        {
            sysmoney = new BCW.BLL.User().GetMoney(1000);
            if (sysmoney < iMoney2)
            {
                Utils.Error("系统1000帐号" + ub.Get("SiteBz2") + "不足", "");
            }
        }
        if (ihid == 1000)
        {
            Utils.Error("权限不足", "");
        }
        //开始操作币
        string mename = new BCW.BLL.User().GetUsName(ihid);
        if (act == "cent1")
        {
            new BCW.BLL.User().UpdateiGold(ihid, mename, -iGold, Content);
            new BCW.BLL.User().UpdateiGold(1000, "系统", iGold, "从ID" + ihid + "中扣除得到");
            new BCW.BLL.Guest().Add(ihid, mename, "系统扣除您的" + iGold + "" + ub.Get("SiteBz") + "，原因:" + Content + "");
        }
        else if (act == "cent2")
        {
            new BCW.BLL.User().UpdateiMoney(ihid, mename, -iMoney, Content);
            new BCW.BLL.User().UpdateiMoney(1000, "系统", iMoney, "从ID" + ihid + "中扣除得到");
            new BCW.BLL.Guest().Add(ihid, mename, "系统扣除您的" + iMoney + "" + ub.Get("SiteBz2") + "，原因:" + Content + "");
        }
        else
        {
            Utils.Error("不存在的功能", "");

            //new BCW.BLL.User().UpdateiMoney(ihid, mename, iMoney2, Content);
            //new BCW.BLL.User().UpdateiMoney(1000, "系统", -iMoney2, "赠送给ID:" + ihid + "");
            //new BCW.BLL.Guest().Add(ihid, mename, "系统赠送您" + iMoney2 + "" + ub.Get("SiteBz2") + "，附言:" + Content + "");
        }


        Utils.Success("快捷操作", "操作成功..", Utils.getPage("uinfo.aspx?act=view&amp;uid=" + ihid + ""), "3");

    }
    private void WagePage()
    {
        Master.Title = "发放会员工资";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发放会员工资");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "选择发放对象:/,或输入ID号(多个用#分开):/,附言:/,奖" + ub.Get("SiteBz") + ":,奖" + ub.Get("SiteBz2") + ":,奖积分:,奖体力:,奖魅力:,奖智慧:,奖威望:,奖邪恶:,,,,";
        string strName = "idType,hid,Content,iGold,iMoney,iScore,Tl,Ml,Zh,Ww,Xe,act,info,backurl";
        string strType = "select,textarea,textarea,num,num,num,num,num,num,num,num,hidden,hidden,hidden,hidden";
        string strValu = "0'''''''''''wagesave'ok'" + Utils.PostPage(1) + "";
        string strEmpt = "0|不选择|1|管理员|2|总版|3|版主,false,true,true,true,true,true,true,true,true,true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定发放,uinfo.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：<br />当选择发放对象时不用填写ID号，选择优先<br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void WageSavePage()
    {
        if (Utils.GetDomain().Contains("ky288") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId != 1 && ManageId != 11 && ManageId != 9 && ManageId != 30)
            {
                Utils.Error("你的权限不足", "");
            }
        }

        int ptype = 18;
        ub xml = new ub();
        string xmlPath = "/Controls/cent.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        int Gold = int.Parse(Utils.GetRequest("iGold", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Money = int.Parse(Utils.GetRequest("iMoney", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Score = int.Parse(Utils.GetRequest("iScore", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Tl = int.Parse(Utils.GetRequest("Tl", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Ml = int.Parse(Utils.GetRequest("Ml", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Zh = int.Parse(Utils.GetRequest("Zh", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Ww = int.Parse(Utils.GetRequest("Ww", "post", 1, @"^-?[0-9]\d*$", "0"));
        int Xe = int.Parse(Utils.GetRequest("Xe", "post", 1, @"^-?[0-9]\d*$", "0"));

        string hid = string.Empty;
        int idType = int.Parse(Utils.GetRequest("idType", "post", 2, @"^[0-3]$", "0"));
        if (idType == 0)
            hid = Utils.GetRequest("hid", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "多个ID请用#分开");

        string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,50}$", "附言限50字内，可留空");

        xml.dss["CentNum" + ptype + ""] = 0;
        xml.dss["CentGold" + ptype + ""] = Gold;
        xml.dss["CentMoney" + ptype + ""] = Money;
        xml.dss["CentScore" + ptype + ""] = Score;
        xml.dss["CentTl" + ptype + ""] = Tl;
        xml.dss["CentMl" + ptype + ""] = Ml;
        xml.dss["CentZh" + ptype + ""] = Zh;
        xml.dss["CentWw" + ptype + ""] = Ww;
        xml.dss["CentXe" + ptype + ""] = Xe;
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);


        //积分操作
        int inum = 0;
        if (idType == 0)
        {
            string[] temp = hid.Split("#".ToCharArray());
            for (int i = 0; i < temp.Length; i++)
            {
                try
                {
                    int ihid = Convert.ToInt32(temp[i]);
                    string mename = new BCW.BLL.User().GetUsName(ihid);
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Wage, ihid);
                    new BCW.BLL.Guest().Add(ihid, mename, "系统发放工资：系统赠送您" + BCW.User.Users.GetWinCent(18, ihid) + "，附言:" + Content + "");
                    inum++;
                }
                catch { }
            }
        }
        else
        {
            if (idType > 0 && idType <= 3)
            {
                string strWhere = string.Empty;
                if (idType == 1)
                {
                    strWhere = "ForumId=-1 and Status=0 and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00')";
                }
                else if (idType == 2)
                {
                    strWhere = "ForumId=0 and Status=0 and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00')";
                }
                else if (idType == 3)
                {
                    strWhere = "ForumId>0 and Status=0 and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00')";
                }

                DataSet ds = new BCW.BLL.Role().GetList("UsID,UsName", strWhere);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int ihid = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                        string mename = ds.Tables[0].Rows[i]["UsName"].ToString();
                        new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Wage, ihid);
                        new BCW.BLL.Guest().Add(ihid, mename, "系统发放工资：系统赠送您" + BCW.User.Users.GetWinCent(18, ihid) + "，附言:" + Content + "");
                        inum++;
                    }
                }
            }
        }
        Utils.Success("发放工资", "发放工资(" + inum + "位会员)成功", Utils.getPage("uinfo.aspx"), "3");
    }

    #region 进入用户社区 InUserPage
    private void InUserPage()
    {

        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.User model = new BCW.BLL.User().GetKey(hid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //管理员可以进入小号
        string XIAOHAO = "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml") + "#";
        string Mobile = new BCW.BLL.User().GetMobile(hid);
        if ((!("#" + XIAOHAO + "#").Contains("#" + hid + "#")) && Mobile != "15107582999")
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId != 1 && ManageId != 11 && ManageId != 13 && ManageId != 30)
            {
                Utils.Error("此功能暂停开放", "");
            }
        }
        if (hid == 19611)
        {
            Utils.Error("你已越权操作", "");
        }
        //设置keys
        string keys = "";
        keys = BCW.User.Users.SetUserKeys(hid, model.UsPwd, model.UsKey);
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string bUrl = string.Empty;
        bUrl = "/bbs/default.aspx?" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";

        //清Cookie
        HttpCookie Cookie = new HttpCookie("LoginComment");
        Cookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(Cookie);

        Utils.Success("进入用户社区", "正在进入..", bUrl, "1");
    }
    #endregion

    #region 清空操作 DelTypePage
    private void DelTypePage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型错误"));
        string sTitle = string.Empty;
        if (ptype == 1)
            sTitle = "短消息";
        else if (ptype == 2)
            sTitle = "空间留言";
        else if (ptype == 3)
            sTitle = "论坛帖子";
        else if (ptype == 4)
            sTitle = "论坛回帖";
        else if (ptype == 5)
            sTitle = "聊天发言";
        else if (ptype == 6)
            sTitle = "空间日记";
        else if (ptype == 7)
            sTitle = "相册文件";
        else if (ptype == 8)
            sTitle = "网站评论";
        else if (ptype == 9)
            sTitle = "社区评论";
        else if (ptype == 10)
            sTitle = "游戏闲聊";

        if (info != "ok")
        {
            Master.Title = "删除会员";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不可恢复！确定清空TA的" + sTitle + "吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok&amp;act=deltype&amp;ptype=" + ptype + "&amp;hid=" + hid + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            if (ptype == 1)
            {
                new BCW.BLL.Guest().DeleteStr("FromId=" + hid + "");
            }
            else if (ptype == 2)
            {
                new BCW.BLL.Mebook().DeleteStr("MID=" + hid + "");
            }
            else if (ptype == 3)
            {
                new BCW.BLL.Text().DeleteStr("UsID=" + hid + "");
            }
            else if (ptype == 4)
            {
                new BCW.BLL.Reply().DeleteStr("UsID=" + hid + "");
            }
            else if (ptype == 5)
            {
                new BCW.BLL.ChatText().DeleteStr("UsID=" + hid + "");
            }
            else if (ptype == 6)
            {
                new BCW.BLL.Diary().DeleteStr("UsID=" + hid + "");
            }
            else if (ptype == 7)
            {
                //附件一起删除
                DataSet ds = new BCW.BLL.Upfile().GetList("ID,Types,Files,PrevFiles,ReID,BID", "UsID=" + hid + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                        string Files = ds.Tables[0].Rows[i]["Files"].ToString();
                        string PrevFiles = ds.Tables[0].Rows[i]["PrevFiles"].ToString();
                        int ReID = int.Parse(ds.Tables[0].Rows[i]["ReID"].ToString());
                        int BID = int.Parse(ds.Tables[0].Rows[i]["BID"].ToString());
                        //删除评论
                        new BCW.BLL.FComment().Delete(Types, id);
                        //删除文件
                        BCW.Files.FileTool.DeleteFile(Files);
                        if (!string.IsNullOrEmpty(PrevFiles))
                            BCW.Files.FileTool.DeleteFile(PrevFiles);

                        //关联帖子回帖减去文件数
                        if (ReID > 0)
                        {
                            new BCW.BLL.Reply().UpdateFileNum(ReID, -1);
                        }
                        else if (BID > 0)
                        {
                            new BCW.BLL.Text().UpdateFileNum(BID, -1);
                            int FileNum = new BCW.BLL.Text().GetFileNum(BID);
                            if (FileNum == 0)
                            {
                                //去掉附件帖标识
                                new BCW.BLL.Text().UpdateTypes(BID, 0);
                            }
                        }
                    }
                }

                new BCW.BLL.Upfile().DeleteStr("UsID=" + hid + "");
            }
            else if (ptype == 8)
            {
                new BCW.BLL.Comment().DeleteStr("UserID=" + hid + "");
            }
            else if (ptype == 9)
            {
                new BCW.BLL.FComment().DeleteStr("UsID=" + hid + "");
            }
            else if (ptype == 10)
            {
                new BCW.BLL.Speak().Delete("UsId=" + hid + "");
            }
            Utils.Success("清空" + sTitle + "", "清空TA的" + sTitle + "成功..", Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + ""), "1");
        }
    }
    #endregion

    #region 删除会员 DelPage
    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();

        if (Utils.GetDomain().Contains("ky288") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            if (ManageId != 1 && ManageId != 11)
            {
                Utils.Error("你的权限不足", "");
            }
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除会员";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不可恢复！删除会员之前建议您清空该会员的资源记录！确定删除此会员吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?act=view&amp;uid=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.User().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.User().Delete(id);
            //删除前先保留重要数据
            SqlHelper.ExecuteSql("Update tb_Chat set UsID=0 where usid=" + id + "");
            SqlHelper.ExecuteSql("Update tb_Group set UsID=0 where usid=" + id + "");
            //关联删除
            DataSet ds = SqlHelper.Query("Select Name FROM SysObjects Where XType='U' and Name NOT IN ('dtproperties') ORDER BY Name");
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (SqlHelper.Exists("select Count(*) from syscolumns where id = object_id('" + ds.Tables[0].Rows[i]["Name"].ToString() + "') and name = 'usid'"))
                    {
                        SqlHelper.ExecuteSql("Delete from " + ds.Tables[0].Rows[i]["Name"].ToString() + " where usid=" + id + "");
                    }
                    else if (SqlHelper.Exists("select Count(*) from syscolumns where id = object_id('" + ds.Tables[0].Rows[i]["Name"].ToString() + "') and name ='userid'"))
                    {
                        SqlHelper.ExecuteSql("Delete from " + ds.Tables[0].Rows[i]["Name"].ToString() + " where userid=" + id + "");
                    }
                }
            }
            Utils.Success("删除会员", "删除会员成功..", Utils.getUrl("uinfo.aspx"), "1");
        }
    }
    #endregion

    #region 转移ID数据 ChangePage
    private void ChangePage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "转移ID数据";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("转移ID数据");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "填写转移到的ID:/,,,,";
            string strName = "toid,id,act,info,backurl";
            string strType = "num,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'change'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定转移,uinfo.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />此功能属于数据服务,操作前请先备份数据库<br />将ID" + id + "所有数据转移到另一个ID达到更换ID的目的<br />如ID" + id + "关联数据太大,转移速度将会缓慢<br />");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int toid = int.Parse(Utils.GetRequest("toid", "post", 2, @"^[0-9]\d*$", "ID错误"));
            if (!new BCW.BLL.User().Exists(id))
            {
                Utils.Error("不存在的ID" + id + "", "");
            }
            if (!new BCW.BLL.User().Exists(toid))
            {
                Utils.Error("不存在的ID" + toid + "", "");
            }
            //取昵称
            string UsName = new BCW.BLL.User().GetUsName(id);
            string ToName = new BCW.BLL.User().GetUsName(toid);
            //关联字段
            string[] IdTemp = ("usid,userid,orderusid,payusid,adminusid,sellid,toId,fromid,winid,mid").Split(",".ToCharArray());
            string[] NameTemp = ("usname,username,payusname,payname,mname,orderusname,winname,fromname,toname").Split(",".ToCharArray());

            DataSet ds = SqlHelper.Query("Select Name FROM SysObjects Where XType='U' and Name NOT IN ('dtproperties') ORDER BY Name");
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Name"].ToString().ToLower() == "tb_user")
                    {
                        SqlHelper.ExecuteSql("Update tb_User set ID=-1 where ID=" + id + "");
                        SqlHelper.ExecuteSql("Update tb_User set ID=" + id + " where ID=" + toid + "");
                        SqlHelper.ExecuteSql("Update tb_User set ID=" + toid + " where ID=-1");
                    }
                    for (int k = 0; k < IdTemp.Length; k++)
                    {
                        if (SqlHelper.Exists("select Count(*) from syscolumns where id = object_id('" + ds.Tables[0].Rows[i]["Name"].ToString() + "') and name = '" + IdTemp[k] + "'"))
                        {
                            SqlHelper.ExecuteSql("Update " + ds.Tables[0].Rows[i]["Name"].ToString() + " set " + IdTemp[k] + "=" + toid + " where " + IdTemp[k] + "=" + id + "");
                        }

                    }
                    for (int j = 0; j < NameTemp.Length; j++)
                    {
                        if (SqlHelper.Exists("select Count(*) from syscolumns where id = object_id('" + ds.Tables[0].Rows[i]["Name"].ToString() + "') and name = '" + NameTemp[j] + "'"))
                        {
                            SqlHelper.ExecuteSql("Update " + ds.Tables[0].Rows[i]["Name"].ToString() + " set " + NameTemp[j] + "='" + ToName + "' where " + NameTemp[j] + "='" + UsName + "'");
                        }

                    }
                }
            }
            Utils.Success("转移数据", "转移数据成功<br /><a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + toid + "") + "\">查看转移到的ID" + toid + "数据</a><br /><a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + "") + "\">查看原ID" + id + "数据</a>", Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + toid + ""), "3");

        }
    }
    #endregion

    #region 查看IP记录 IpViewPage
    private void IpViewPage()
    {
        Master.Title = "查看IP记录";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string UsName = new BCW.BLL.User().GetUsName(id);
        if (UsName == "")
        {
            Utils.Error("不存在的ID" + id + "", "");
        }
        Master.Title = "查看IP记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + UsName + "(" + id + ")IP记录");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //整体读取
        try
        {
            string filePath = Server.MapPath("/log/loginip/" + id + "_" + DESEncrypt.Encrypt(id.ToString(), "kubaoLogenpt") + ".log");
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string log = reader.ReadToEnd();
            if (!string.IsNullOrEmpty(log))
            {
                string[] sName = Regex.Split(log, "\r\n");
                //总记录数
                int recordCount = sName.Length - 1;

                int stratIndex = (pageIndex - 1) * pageSize;
                int endIndex = pageIndex * pageSize;
                int k = 0;
                for (int i = recordCount; i > 0; i--)
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        if ((k + 1) % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));

                        string getLine = sName[i - 1].ToString();

                        builder.AppendFormat("{0}.{1}", (recordCount - i + 1), getLine);


                        builder.Append(Out.Tab("</div>", ""));
                    }
                    if (k == endIndex)
                        break;
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            reader.Close();
            stream.Close();
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("暂无变动记录");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region UBB设置社区身份 UsUbbPage
    private void UsUbbPage()
    {
        Master.Title = "UBB设置社区身份";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string UsName = new BCW.BLL.User().GetUsName(id);
        if (UsName == "")
        {
            Utils.Error("不存在的ID" + id + "", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            string usubb = Utils.GetRequest("usubb", "all", 3, @"^[\s\S]{1,800}$", "限800字符内，可以留空");

            new BCW.BLL.User().UpdateUsUbb(id, usubb);

            Utils.Success("设置UBB身份", "设置UBB身份成功..", Utils.getUrl("uinfo.aspx?act=usubb&amp;id=" + id + ""), "1");

        }
        else
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("" + UsName + "(" + id + ")社区身份");
            builder.Append(Out.Tab("</div>", ""));

            string ubb = new BCW.BLL.User().GetUsUbb(id);
            string strText = "请填写UBB身份:/,,,,";
            string strName = "usubb,id,act,info,backurl";
            string strType = "textarea,hidden,hidden,hidden,hidden";
            string strValu = "" + ubb + "'" + id + "'usubb'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定设置,uinfo.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    #endregion

    #region 过币费率管理 finaceSetPage
    public void finaceSetPage()
    {
        Master.Title = "过币费率管理";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove("xml_wap");//清缓存
        xml.Reload(); //加载网站配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Commission = Utils.GetRequest("Commission", "post", 2, @"^[0-9]\d*$", "费率填写错误");
            string CmOnf = Utils.GetRequest("CmOnf", "post", 2, @"^[0-1]$", "功能开关选择错误");
            string ids = Utils.GetRequest("ids", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            xml.ds["SiteCm"] = Commission;
            xml.ds["SiteCmOnf"] = CmOnf;
            xml.ds["SiteIds"] = ids;
            System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
            Utils.Success("过币费率管理", "费率管理成功，正在返回..", Utils.getUrl("uinfo.aspx?act=finace"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "过币费率管理"));

            string strText = "费率(‰):/,功能开关:/,免除ID(用#号分开):/,";
            string strName = "Commission,CmOnf,ids,act";
            string strType = "text,select,big,hidden";
            string strValu = "" + xml.ds["SiteCm"] + "'" + "" + xml.ds["SiteCmOnf"] + "'" + "" + xml.ds["SiteIds"] + "'finace";
            string strEmpt = "0,0|不启用|1|启用,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset," + Utils.getUrl("uinfo.aspx") + ",post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />费率填写为0或功能关闭时,则不收取任何手续费<br />免除ID填写则除中介外的ID均能免手续费");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    #endregion
}
