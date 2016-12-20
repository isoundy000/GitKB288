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

public partial class bbs_usermanage : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "管理会员";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "admin":
            case "admin2":
                AdminPage(act, meid);
                break;
            case "position":
                PositionPage(act, meid);
                break;
            case "role":
                RolePage(act, meid);
                break;
            case "add":
            case "add2":
                AddPage(act, meid);
                break;
            case "save":
                SavePage(meid);
                break;
            case "edit":
                EditPage(meid);
                break;
            case "editsave":
                EditSavePage(meid);
                break;
            case "del":
                DelPage(meid);
                break;
            case "view":
                ViewPage(meid);
                break;
            case "honor":
            case "hon2or":
                HonorPage(act, meid);
                break;
            case "honorno":
            case "hon2orno":
                HonorNoPage(act, meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理会员：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">管理权限</a><br />");

        string Limit = new BCW.BLL.User().GetLimit(hid);


        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Lock, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Lock, hid) || Limit.Contains("lock"))
            {
                builder.Append("已锁定<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">锁定</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Text, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Text, hid))
            {
                builder.Append("已禁发贴<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">禁发贴</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Reply, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Reply, hid))
            {
                builder.Append("已禁回贴<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">禁回贴</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Guest, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Guest, hid))
            {
                builder.Append("已禁短消息<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">禁短消息</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Chat, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Chat, hid))
            {
                builder.Append("已禁聊天室<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">禁聊天室</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_NetWork, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_NetWork, hid))
            {
                builder.Append("已禁发广播<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">禁发广播</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Comment, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Comment, hid))
            {
                builder.Append("已禁全站评论<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">禁全站评论</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Upfile, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Upfile, hid))
            {
                builder.Append("已禁上传文件<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">禁上传文件</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Finance, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Finance, hid))
            {
                builder.Append("已禁金融系统<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">禁金融系统</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Game, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Game, hid))
            {
                builder.Append("已禁社区游戏<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=11&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=11&amp;backurl=" + Utils.getPage(0) + "") + "\">禁社区游戏</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Speak, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Speak, hid))
            {
                builder.Append("已禁游戏闲聊<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">禁游戏闲聊</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_UsBook, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_UsBook, hid))
            {
                builder.Append("已禁使用留言本<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">禁使用留言本</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Diary, hid))
            {
                builder.Append("已禁使用日记<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">禁使用日记</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Albums, hid))
            {
                builder.Append("已禁使用相册<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">禁使用相册</a><br />");
            }
        }
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Spaceapp, uid, 0))
        {
            if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Spaceapp, hid))
            {
                builder.Append("已禁社区应用<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">禁社区应用</a>");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + hid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage(int uid)
    {
        Master.Title = "查看会员自身权限";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>的自身权限");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        string Limit = new BCW.BLL.User().GetLimit(hid);

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Lock, hid) || Limit.Contains("lock"))
        {
            builder.Append("全站锁定(已锁定)<br />");
        }
        else
        {
            builder.Append("全站锁定(未锁定)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Text, hid))
        {
            builder.Append("发表帖子(已禁)<br />");
        }
        else
        {
            builder.Append("发表帖子(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Reply, hid))
        {
            builder.Append("发表回帖(已禁)<br />");
        }
        else
        {
            builder.Append("发表回帖(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Guest, hid))
        {
            builder.Append("社区内线(已禁)<br />");
        }
        else
        {
            builder.Append("社区内线(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Chat, hid))
        {
            builder.Append("聊天室(已禁)<br />");
        }
        else
        {
            builder.Append("聊天室(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_NetWork, hid))
        {
            builder.Append("广播喇叭(已禁)<br />");
        }
        else
        {
            builder.Append("广播喇叭(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Comment, hid))
        {
            builder.Append("全站评论(已禁)<br />");
        }
        else
        {
            builder.Append("全站评论(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Upfile, hid))
        {
            builder.Append("上传文件(已禁)<br />");
        }
        else
        {
            builder.Append("上传文件(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Finance, hid))
        {
            builder.Append("金融系统(已禁)<br />");
        }
        else
        {
            builder.Append("金融系统(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Game, hid))
        {
            builder.Append("社区游戏(已禁)<br />");
        }
        else
        {
            builder.Append("社区游戏(正常)<br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Speak, hid))
        {
            builder.Append("游戏闲聊(已禁)<br />");
        }
        else
        {
            builder.Append("游戏闲聊(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_UsBook, hid))
        {
            builder.Append("空间留言(已禁)<br />");
        }
        else
        {
            builder.Append("空间留言(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Diary, hid))
        {
            builder.Append("社区日记(已禁)<br />");
        }
        else
        {
            builder.Append("社区日记(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Albums, hid))
        {
            builder.Append("社区相册(已禁)<br />");
        }
        else
        {
            builder.Append("社区相册(正常)<br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Spaceapp, hid))
        {
            builder.Append("社区应用(已禁)");
        }
        else
        {
            builder.Append("社区应用(正常)");
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + hid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminPage(string act, int meid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "all", 2, @"^[0-9]\d*$", "会员ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "选择权限错误"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string sName = string.Empty;
        string Role = string.Empty;
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        string UsName = new BCW.BLL.User().GetUsName(hid);
        if (ptype == 1)
        {
            sName = "锁定";
            Role = "A";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Lock, meid);
        }
        else if (ptype == 2)
        {
            sName = "发帖";
            Role = "B";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Text, meid);
        }
        else if (ptype == 3)
        {
            sName = "回贴";
            Role = "C";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Reply, meid);
        }
        else if (ptype == 4)
        {
            sName = "短消息";
            Role = "D";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Guest, meid);
        }
        else if (ptype == 5)
        {
            sName = "聊天室";
            Role = "E";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Chat, meid);
        }
        else if (ptype == 6)
        {
            sName = "留言本";
            Role = "F";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_UsBook, meid);
        }
        else if (ptype == 7)
        {
            sName = "发广播";
            Role = "G";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_NetWork, meid);
        }
        else if (ptype == 8)
        {
            sName = "全站评论";
            Role = "H";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Comment, meid);
        }
        else if (ptype == 9)
        {
            sName = "上传文件";
            Role = "I";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Upfile, meid);
        }
        else if (ptype == 10)
        {
            sName = "金融系统";
            Role = "J";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Finance, meid);
        }
        else if (ptype == 11)
        {
            sName = "社区游戏";
            Role = "K";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Game, meid);
        }
        else if (ptype == 12)
        {
            sName = "使用日记";
            Role = "L";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Diary, meid);
        }
        else if (ptype == 13)
        {
            sName = "使用相册";
            Role = "M";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Albums, meid);
        }
        else if (ptype == 14)
        {
            sName = "使用闲聊";
            Role = "N";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Speak, meid);
        }
        else if (ptype == 15)
        {
            sName = "社区应用";
            Role = "O";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Spaceapp, meid);
        }
        if (act == "admin")
        {
            if (ptype == 1)
                sName = "锁定";
            else
                sName = "禁止" + sName + "";
        }
        else
        {
            if (ptype == 1)
                sName = "解锁";
            else
                sName = "解禁" + sName + "";
        }
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "hid", "ptype", "Why", "rDay", "info", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,50}$", "理由限50字内，可留空");
            int rDay = 0;
            if (act == "admin")
            {
                rDay = int.Parse(Utils.GetRequest("rDay", "post", 2, @"^0|1|2|3|5|10|15|30|90$", "期限选择错误"));
                if (ptype != 1)
                {
                    if (rDay == 0)
                    {
                        Utils.Error("只有锁定类型才可以选择永久期限", "");
                    }
                }
                //如果已存在过期加黑记录则先删除
                new BCW.BLL.Blacklist().DeleteRole(hid, Role);
                if (new BCW.BLL.Blacklist().ExistsRole(hid, Role))
                {
                    Utils.Error("此ID已被" + sName + "", "");
                }
                BCW.Model.Blacklist model = new BCW.Model.Blacklist();
                model.UsID = hid;
                model.UsName = UsName;
                model.ForumID = 0;
                model.ForumName = "";
                model.BlackRole = Role;
                model.BlackWhy = Why;
                model.BlackDay = rDay;
                model.Include = 0;
                model.AdminUsID = meid;
                model.ExitTime = DateTime.Now.AddDays(rDay);
                model.AddTime = DateTime.Now;
                new BCW.BLL.Blacklist().Add(model);
                //永久锁定
                if (ptype == 1 && rDay == 0)
                {
                    string Limit = new BCW.BLL.User().GetLimit(hid);
                    Limit = Limit + "|lock";
                    new BCW.BLL.User().UpdateLimit(hid, Limit);
                }
            }
            else
            {
                if (ptype > 1)
                {
                    if (new BCW.BLL.Blacklist().ExistsRole(hid, "A"))
                    {
                        Utils.Error("你必须解除锁定才能进行此操作", "");
                    }
                }
                string Limit = new BCW.BLL.User().GetLimit(hid);
                if (!new BCW.BLL.Blacklist().ExistsRole(hid, Role) && !Limit.Contains("lock"))
                {
                    Utils.Error("不存在的加黑记录", "");
                }
                new BCW.BLL.Blacklist().DeleteRole(hid, Role);
                //解除永久锁定
                if (ptype == 1)
                {
                    Limit = Limit.Replace("lock", "");
                    new BCW.BLL.User().UpdateLimit(hid, Limit);
                }
            }
            //记录日志
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]对[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]" + sName + "";
            string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]将你" + sName + "";
            if (act == "admin")
            {
                if (rDay == 0)
                {
                    strLog += "(永久)";
                    strLog2 += "(永久)";
                }
                else
                {
                    strLog += "" + rDay + "天";
                    strLog2 += "" + rDay + "天";
                }
            }
            if (!string.IsNullOrEmpty(Why))
            {
                strLog += ",理由:" + Why + "";
                strLog2 += ",理由:" + Why + "";
            }
            new BCW.BLL.Forumlog().Add(0, 0, strLog);
            new BCW.BLL.Guest().Add(0, hid, UsName, strLog2);
            Utils.Success("管理会员", "执行管理成功，正在返回..", Utils.getUrl("usermanage.aspx?hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("对<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + hid + ")</a>执行《" + sName + "》");
            builder.Append(Out.Tab("</div>", ""));
            if (act == "admin")
            {
                strText = ",,理由(50字内):/,期限:,,,,";
                strName = "hid,ptype,Why,rDay,act,info,backurl";
                strType = "hidden,hidden,textarea,select,hidden,hidden,hidden";
                strValu = "" + hid + "'" + ptype + "''10'admin'ok'" + Utils.getPage(0) + "";
                if (ptype == 1)
                    strEmpt = "false,false,false,0|永久|1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天|90|三个月,false,false,false";
                else
                    strEmpt = "false,false,false,1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天|90|90天,false,false,false";
            }
            else
            {
                strText = ",,理由(50字内):/,,,,";
                strName = "hid,ptype,Why,act,info,backurl";
                strType = "hidden,hidden,textarea,hidden,hidden,hidden";
                strValu = "" + hid + "'" + ptype + "''admin2'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false";
            }
            strIdea = "/";
            strOthe = "确定执行,usermanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("usermanage.aspx?hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            if (ptype == 1)
                builder.Append("<br />温馨提示:<br />1.锁定会员后,该会员将不能正常使用所有功能.");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void PositionPage(string act, int uid)
    {
        Master.Title = "查看权限";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理会员：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=当前权限=<br />");
        int GroupId = new BCW.BLL.Group().GetID(hid);
        string Limit = new BCW.BLL.User().GetLimit(hid);

        DataSet ds = new BCW.BLL.Role().GetList("ID,UsName,RoleName,ForumID,ForumName,Include", "UsID=" + hid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') ORDER BY FORUMID ASC");
        if (ds != null && ds.Tables[0].Rows.Count > 0 || GroupId > 0)
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    int Include = int.Parse(ds.Tables[0].Rows[i]["Include"].ToString());
                    int forumid = int.Parse(ds.Tables[0].Rows[i]["ForumID"].ToString());
                    string ForumName = ds.Tables[0].Rows[i]["ForumName"].ToString();
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    string RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();
                    if (forumid == -1)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=role&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a> 管辖全站<br />");

                    }
                    else if (forumid == 0)
                    {
                        if (new BCW.BLL.Role().IsAdmin(uid) && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid))
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=edit&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[管理]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=role&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a> 管辖全区版块<br />");
                    }
                    else
                    {
                        if ((new BCW.BLL.Role().IsSumMode(uid) && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) || new BCW.BLL.Group().GetForumId(uid) == forumid)
                            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=edit&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[管理]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=role&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + RoleName + "</a> 管辖<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">" + ForumName + "</a><br />");
                    }
                }

              
                if (!Limit.Contains("honor"))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=honor&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[设为荣誉版主]</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=honorno&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[撤销荣誉版主]</a>");
                }
            }
            if (GroupId > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=view&amp;id=" + GroupId + "") + "\">" + new BCW.BLL.Group().GetTitle(GroupId) + "</a> " + ub.GetSub("GroupzName", "/Controls/group.xml") + "");
            }
        }
        else
        {
            builder.Append("普通会员");
        }

        if (!Limit.Contains("hon2or"))
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("usermanage.aspx?act=hon2or&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[设为荣誉会员]</a>");
        }
        else
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("usermanage.aspx?act=hon2orno&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[撤销荣誉会员]</a>");
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + hid + "") + "\">上级</a>");
        if ((new BCW.BLL.Role().IsSumMode(uid) && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) || new BCW.BLL.Group().GetForumId(uid) > 0)
            builder.Append("-<a href=\"" + Utils.getUrl("usermanage.aspx?act=add&amp;hid=" + hid + "") + "\">任命版主</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    private void RolePage(string act, int uid)
    {
        Master.Title = "查看详细权限";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (!new BCW.BLL.Role().Exists2(id, hid))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Role model = new BCW.BLL.Role().GetRole(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + model.UsName + "权限列表:");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));
        int gNum = Utils.GetStringNum(model.Rolece, ";");
        builder.Append(Out.Tab("<div>", ""));

        string sInclude = string.Empty;
        if (model.Include == 1)
            sInclude = "(含下级版块)";

        if (model.ForumID == -1)
            builder.Append("管辖:全站");
        else if (model.ForumID == 0)
            builder.Append("管辖:全区版块");
        else
            builder.Append("管辖:<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + model.ForumID + "") + "\">" + model.ForumName + "</a>" + sInclude + "");


        builder.Append("<br />拥有权限(" + (gNum + 1) + "/40)");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + BCW.User.Role.OutRoleString(model.Rolece).Replace(" ", "<br />") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("usermanage.aspx?act=position&amp;hid=" + hid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void AddPage(string act, int uid)
    {
        Master.Title = "添加版主";
        int ForumId = new BCW.BLL.Group().GetForumId(uid);
        if ((!new BCW.BLL.Role().IsSumMode(uid) || !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) && ForumId == 0)
        {
            Utils.Error("你的权限不足", "");
        }
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理会员：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "-1"));
        if (id == -1)
        {
            if ((new BCW.BLL.Role().IsSumMode(uid) && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("请选择论坛版块");
                builder.Append(Out.Tab("</div>", "<br />"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "hid", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                if (act == "add")
                    strWhere = "GroupId=0 and IsActive=0";
                else
                    strWhere = "GroupId>0 and IsActive=0";

                // 开始读取专题
                IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
                if (listForum.Count > 0)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("usermanage.aspx?act=add2&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">圈论坛版主</a>");

                    if (new BCW.BLL.Role().IsAdmin(uid))
                    {
                        builder.AppendFormat("<br /><a href=\"" + Utils.getUrl("usermanage.aspx?act=add&amp;id=0&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">版块总版主</a>");
                    }

                    builder.Append(Out.Tab("</div>", Out.Hr()));


                    int k = 1;
                    foreach (BCW.Model.Forum n in listForum)
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
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("usermanage.aspx?act=add&amp;id={0}&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}(ID{0})</a>", n.ID, n.Title);
                        k++;
                        builder.Append(Out.Tab("</div>", ""));

                    }

                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("text", "没有相关记录"));
                }
            }
            if (ForumId > 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=add&amp;id=" + ForumId + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.Forum().GetTitle(ForumId) + "(" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛)</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

        }
        else
        {

            builder.Append(Out.Tab("<div>", ""));
            if (id == 0)
            {
                if (!new BCW.BLL.Role().IsAdmin(uid))
                {
                    Utils.Error("你的权限不足", "");
                }
                builder.Append("添加总版主");
            }
            else
            {
                string ForumName = new BCW.BLL.Forum().GetTitle(id);
                if (ForumName == "")
                    Utils.Error("不存在的版块ID", "");

                builder.Append("添加[" + ForumName + "]版主");
            }
            builder.Append(Out.Tab("</div>", ""));

            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            string RoleString = string.Empty;
            //圈坛权限列表
            if (new BCW.BLL.Forum().GetGroupId(id) > 0)
                RoleString = BCW.User.Role.GetRoleString();
            else
                RoleString = BCW.User.Role.OutRoleStringMe(uid);
            if (id > 0)
            {
                sText = "权限包括下级版:/,";
                sName = "Include,";
                sType = "select,";
                sValu = "0'";
                sEmpt = "0|不含|1|包含,";
                RoleString = RoleString.Replace("|z|设置版主", "");
                RoleString = RoleString.Replace("z|设置版主", "");
                if (RoleString == "")
                {
                    Utils.Error("你的权限不足", "");
                }
            }

            string strText = ",选择权限，可多选/,期限(天|填0则无限期):/,职称(如:正版|副版|见习版):/," + sText + ",";
            string strName = "UsID,Role,rDay,rName," + sName + "id,act";
            string strType = "hidden,multiple,num,text," + sType + "hidden,hidden";
            string strValu = "" + hid + "''0''" + sValu + "" + id + "'save";
            string strEmpt = "false," + RoleString + ",false,false," + sEmpt + "false,false";
            string strIdea = "/";
            string strOthe = "添加版主|reset,usermanage.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void SavePage(int uid)
    {
        //管理安全提示
        string[] p_pageArr = { "act", "id", "UsID", "Role", "rDay", "rName", "Include" };
        BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        string Role = Utils.GetRequest("Role", "post", 2, @"^[\w((;|,)\w)?]+$", "选择权限错误");
        Role = Role.Replace(",", ";");
        int rDay = int.Parse(Utils.GetRequest("rDay", "post", 2, @"^[0-9]\d*$", "期限填写错误"));
        string rName = Utils.GetRequest("rName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "职称限10字内，不能使用特别字符");
        int Include = int.Parse(Utils.GetRequest("Include", "post", 1, @"^[0-1]$", "0"));

        int ForumId = new BCW.BLL.Group().GetForumId(uid);
        if ((!new BCW.BLL.Role().IsSumMode(uid) || !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) && ForumId != id)
        {
            Utils.Error("你的权限不足", "");
        }
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        string ForumName = string.Empty;
        if (id > 0)
        {
            ForumName = new BCW.BLL.Forum().GetTitle(id);
            if (ForumName == "")
            {
                Utils.Error("不存在的版块ID", "");
            }
            if (new BCW.BLL.Role().Exists(UsID, id))
            {
                Utils.Error("ID:" + UsID + "已经是" + ForumName + "的版主", "");
            }
        }
        else
        {
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                Utils.Error("你的权限不足", "");
            }
            if (new BCW.BLL.Role().Exists(UsID, 0))
            {
                Utils.Error("ID:" + UsID + "已经是总版主", "");
            }
            if (new BCW.BLL.Role().Exists(UsID, -1))
            {
                Utils.Error("ID:" + UsID + "已经是管理员，管理员与总版不能同时兼职", "");
            }
        }

        //是否有此权限并任命此权限
        if (new BCW.BLL.Group().GetForumId(uid) != id)
        {
            string MyRole = new BCW.BLL.Role().GetRolece(uid);
            if (!Utils.IsValidName(Role, MyRole))
            {
                Utils.Error("任命权限错误，您并没有此权限可以任命", "");
            }
        }

        string UsName = new BCW.BLL.User().GetUsName(UsID);
        BCW.Model.Role model = new BCW.Model.Role();
        model.UsID = UsID;
        model.UsName = UsName;
        model.ForumID = id;
        model.ForumName = ForumName;
        model.Rolece = Role;
        model.RoleName = rName;
        model.StartTime = DateTime.Now;
        if (rDay == 0)
            model.OverTime = DateTime.Parse("1990-1-1 00:00:00");
        else
            model.OverTime = DateTime.Now.AddDays(rDay);

        model.Include = Include;
        model.Status = 0;
        new BCW.BLL.Role().Add(model);

        //记录日志
        string strLog = string.Empty;
        if (id > 0)
        {
            strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "[/url]设为[" + ForumName + "]论坛版主,职称:" + rName + "";
        
            //任职记录
            new BCW.BLL.Forumlog().Add(14, id, "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "(" + UsID + ")[/url]上任时间：" + DT.FormatDate(DateTime.Now, 11) + "");
        
        }
        else
            strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "[/url]设为论坛总版主,职称:" + rName + "";

        new BCW.BLL.Forumlog().Add(0, 0, strLog);
        //发送内线给新版主
        new BCW.BLL.Guest().Add(UsID, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "[/url]", "您"));

        Utils.Success("设置版主", "恭喜，设置版主成功，正在返回..", Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + UsID + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void EditPage(int uid)
    {
        Master.Title = "编辑版主";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (!new BCW.BLL.Role().Exists2(id, hid))
        {
            Utils.Error("不存在的记录", "");
        }
        int ForumId = new BCW.BLL.Group().GetForumId(uid);
        BCW.Model.Role model = new BCW.BLL.Role().GetRole(id);
        if ((!new BCW.BLL.Role().IsSumMode(uid) || !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) && ForumId != model.ForumID)
        {
            Utils.Error("你的权限不足", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (model.ForumID == -1)
        {
            Utils.Error("你的权限不足", "");
        }
        if (model.ForumID == 0)
        {
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                Utils.Error("你的权限不足", "");
            }
            builder.Append("编辑总版主权限");
        }
        else
        {
            builder.Append("编辑版主权限");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("用户:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(ID" + model.UsID + ")</a><br />");

        string sInclude = string.Empty;
        if (model.Include == 1)
            sInclude = "(含下级版块)";
        if (model.ForumID == 0)
            builder.Append("管辖:全区版块");
        else
            builder.Append("管辖:<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + model.ForumID + "") + "\">" + model.ForumName + "</a>" + sInclude + "");

        int gNum = Utils.GetStringNum(model.Rolece, ";");
        builder.Append("<br />当前拥有权限(" + (gNum + 1) + "/40)");
        builder.Append(Out.Tab("</div>", ""));

        string oTime = model.OverTime.ToString();
        if (model.OverTime.ToString() == "1990-1-1 0:00:00")
            oTime = "0";
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;
        string RoleString = string.Empty;
        //圈坛权限列表
        if (new BCW.BLL.Forum().GetGroupId(model.ForumID) > 0)
            RoleString = BCW.User.Role.GetRoleString();
        else
            RoleString = BCW.User.Role.OutRoleStringMe(uid);
        if (model.ForumID > 0)
        {
            sText = "权限包括下级版:/,";
            sName = "Include,";
            sType = "select,";
            sValu = "0'";
            sEmpt = "0|不含|1|包含,";
            RoleString = RoleString.Replace("|z|设置版主", "");
            RoleString = RoleString.Replace("z|设置版主", "");
            if (RoleString == "")
            {
                Utils.Error("你的权限不足", "");
            }
        }
        strText = "选择权限，可多选/,上任时间:/,卸任时间(填0则无限期):/,职称(如:正版|副版|见习版):/," + sText + "权限状态:/,,,,";
        strName = "Role,sDay,rDay,rName," + sName + "Status,id,hid,act,backurl";
        strType = "multiple,date,text,text," + sType + "select,hidden,hidden,hidden,hidden";
        strValu = "" + model.Rolece + "'" + model.StartTime + "'" + oTime + "'" + model.RoleName + "'" + sValu + "" + model.Status + "'" + id + "'" + hid + "'editsave'" + Utils.getPage(0) + "";
        strEmpt = "" + RoleString + ",false,false,false," + sEmpt + "0|正常|1|暂停,false,false,false,false";
        strIdea = "/";
        strOthe = "编辑版主|reset,usermanage.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />");
        builder.Append("总版权限将有所有论坛的管理区域.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=del&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">撤除此版主</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditSavePage(int uid)
    {
        //管理安全提示
        string[] p_pageArr = { "act", "id", "hid", "Role", "sDay", "rDay", "rName", "Status", "Include", "backurl" };
        BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (!new BCW.BLL.Role().Exists2(id, hid))
        {
            Utils.Error("不存在的记录", "");
        }
        int ForumID = new BCW.BLL.Role().GetForumID(id);
        int ForumId = new BCW.BLL.Group().GetForumId(uid);
        if ((!new BCW.BLL.Role().IsSumMode(uid) || !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) && ForumId != ForumID)
        {
            Utils.Error("你的权限不足", "");
        }

        if (ForumID <= 0)
        {
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        string GetRole = string.Empty;
        string Role = Utils.GetRequest("Role", "post", 2, @"^[\w((;|,)\w)?]+$", "选择权限错误");
        Role = Role.Replace(",", ";");
        GetRole = Role;

        DateTime sDay = Utils.ParseTime(Utils.GetRequest("sDay", "post", 2, DT.RegexTime, "上任时间填写错误"));
        string rDay = Utils.GetRequest("rDay", "post", 2, "", "");
        string rName = Utils.GetRequest("rName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "职称限10字内，不能使用特别字符");
        int Include = int.Parse(Utils.GetRequest("Include", "post", 1, @"^[0-1]$", "0"));
        int Status = int.Parse(Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "权限状态选择错误"));
        if (rDay == "0")
        {
            rDay = "1990-1-1 00:00:00";
        }
        else
        {
            if (!Utils.IsRegex(rDay, DT.RegexTime))
            {
                Utils.Error("卸任时间填写错误", "");
            }
        }

        //是否有此权限并任命此权限
        if (new BCW.BLL.Group().GetForumId(uid) != ForumID)
        {
            string MyRole = new BCW.BLL.Role().GetRolece(uid);
            if (!Utils.IsValidName(Role, MyRole))
            {
                Utils.Error("任命权限错误，您并没有此权限可以任命", "");
            }
        }
        BCW.Model.Role m = new BCW.BLL.Role().GetRole(id);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //取用户昵称
        string UsName = new BCW.BLL.User().GetUsName(m.UsID);
        //取论坛名称
        string ForumName = new BCW.BLL.Forum().GetTitle(m.ForumID);

        BCW.Model.Role model = new BCW.Model.Role();
        model.ID = id;
        model.UsName = UsName;
        model.Rolece = GetRole;
        model.RoleName = rName;
        model.ForumName = ForumName;
        model.StartTime = sDay;
        model.OverTime = DateTime.Parse(rDay);
        model.Include = Include;
        model.Status = Status;
        new BCW.BLL.Role().Update(model);
        //记录日志
        string strLog = string.Empty;
        if (ForumID > 0)
            strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]修改[url=/bbs/uinfo.aspx?uid=" + m.UsID + "]" + UsName + "[/url]的[" + ForumName + "]论坛的版主权限";
        else
            strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]修改[url=/bbs/uinfo.aspx?uid=" + m.UsID + "]" + UsName + "[/url]的论坛总版主权限";

        new BCW.BLL.Forumlog().Add(0, 0, strLog);

        //发送内线给版主
        new BCW.BLL.Guest().Add(m.UsID, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + m.UsID + "]" + UsName + "[/url]", "您"));
        Utils.Success("编辑版主", "恭喜，编辑版主成功，正在返回..", Utils.getPage("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage(int uid)
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));

        if (info != "ok")
        {
            Master.Title = "撤除版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定撤除此版主吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=edit&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "id", "hid", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "get");

            if (!new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (!new BCW.BLL.Role().Exists2(id, hid))
            {
                Utils.Error("不存在的记录", "");
            }
            int ForumID = new BCW.BLL.Role().GetForumID(id);
            int ForumId = new BCW.BLL.Group().GetForumId(uid);
            if ((!new BCW.BLL.Role().IsSumMode(uid) || !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ModeratorSet, uid)) && ForumId != ForumID)
            {
                Utils.Error("你的权限不足", "");
            }

            if (ForumID <= 0)
            {
                if (!new BCW.BLL.Role().IsAdmin(uid))
                {
                    Utils.Error("你的权限不足", "");
                }
            }
            //撤除
            new BCW.BLL.Role().Delete(id);
            //取用户昵称
            string UsName = new BCW.BLL.User().GetUsName(hid);
            //记录日志
            string strLog = string.Empty;
            if (ForumID > 0)
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]的[" + new BCW.BLL.Forum().GetTitle(ForumID) + "]论坛的版主权限撤除";
                //任职记录
                new BCW.BLL.Forumlog().Add(14, ForumID, "[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "(" + hid + ")[/url]离任时间：" + DT.FormatDate(DateTime.Now, 11) + "");
        
            }
            else
                strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]的论坛总版主权限撤除";

            new BCW.BLL.Forumlog().Add(0, 0, strLog);


            //发送内线给版主
            new BCW.BLL.Guest().Add(hid, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]", "您"));
            Utils.Success("撤除版主", "撤除版主成功..", Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void HonorPage(string act, int uid)
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));

        string sText = "版主";
        if (act == "hon2or")
            sText = "会员";

        if (info != "ok")
        {
            Master.Title = "设置荣誉" + sText + "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定设置" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")成为荣誉" + sText + "吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?info=ok&amp;act=" + act + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定设置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "hid", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "get");

            if (!new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                Utils.Error("你的权限不足", "");
            }
            string Limit = new BCW.BLL.User().GetLimit(hid);
            if (Limit.Contains(act))
            {
                Utils.Error("该会员已经是荣誉" + sText + "", "");
            }

            Limit = Limit + "|" + act + "";
            new BCW.BLL.User().UpdateLimit(hid, Limit);

            //记录日志
            string UsName = new BCW.BLL.User().GetUsName(hid);
            string strLog = string.Empty;
            strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]设置成为荣誉" + sText + "";
            new BCW.BLL.Forumlog().Add(0, 0, strLog);
            //发送内线给新版主
            new BCW.BLL.Guest().Add(hid, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]", "您"));
            Utils.Success("设置荣誉" + sText + "", "设置荣誉" + sText + "成功..", Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }


    private void HonorNoPage(string act, int uid)
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));

        string sText = "版主";
        if (act == "hon2orno")
            sText = "会员";

        if (info != "ok")
        {
            Master.Title = "撤销荣誉" + sText + "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定撤销" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")的荣誉" + sText + "吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?info=ok&amp;act=" + act + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤销</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "hid", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "get");

            if (!new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                Utils.Error("你的权限不足", "");
            }
            string Limit = new BCW.BLL.User().GetLimit(hid);
            if (!Limit.Contains(act.Replace("rno", "r")))
            {
                Utils.Error("该会员不是荣誉" + sText + "", "");

            }

            Limit = Limit.Replace(act.Replace("rno", "r"), "");
            new BCW.BLL.User().UpdateLimit(hid, Limit);

            //记录日志
            string UsName = new BCW.BLL.User().GetUsName(hid);
            string strLog = string.Empty;
            strLog = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]撤销荣誉" + sText + "身份";
            new BCW.BLL.Forumlog().Add(0, 0, strLog);
            //发送内线给新版主
            new BCW.BLL.Guest().Add(hid, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]", "您"));
            Utils.Success("撤销荣誉" + sText + "", "撤销荣誉" + sText + "成功..", Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

}