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

public partial class Manage_usermanage : System.Web.UI.Page
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
        int meid = 0;

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "admin":
            case "admin2":
                AdminPage(act, meid);
                break;
            case "position":
                PositionPage(act);
                break;
            case "payset":
                PaySetPage();
                break;
            case "honor":
                HonorPage(meid);
                break;
            case "honorno":
                HonorNoPage(meid);
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
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

        string Limit = new BCW.BLL.User().GetLimit(hid);

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Lock, hid) || Limit.Contains("lock"))
        {
            builder.Append("已锁定<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">锁定</a><br />");
        }


        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Text, hid))
        {
            builder.Append("已禁发贴<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">禁发贴</a><br />");
        }


        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Reply, hid))
        {
            builder.Append("已禁回贴<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">禁回贴</a><br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Guest, hid))
        {
            builder.Append("已禁短消息<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">禁短消息</a><br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Chat, hid))
        {
            builder.Append("已禁聊天室<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">禁聊天室</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_NetWork, hid))
        {
            builder.Append("已禁发广播<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">禁发广播</a><br />");
        }


        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Comment, hid))
        {
            builder.Append("已禁全站评论<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">禁全站评论</a><br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Upfile, hid))
        {
            builder.Append("已禁上传文件<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">禁上传文件</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Finance, hid))
        {
            builder.Append("已禁金融系统<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">禁金融系统</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Game, hid))
        {
            builder.Append("已禁社区游戏<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=11&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=11&amp;backurl=" + Utils.getPage(0) + "") + "\">禁社区游戏</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Speak, hid))
        {
            builder.Append("已禁游戏闲聊<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">禁游戏闲聊</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_UsBook, hid))
        {
            builder.Append("已禁使用留言本<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">禁使用留言本</a><br />");
        }

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Diary, hid))
        {
            builder.Append("已禁使用日记<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">禁使用日记</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Albums, hid))
        {
            builder.Append("已禁使用相册<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">禁使用相册</a><br />");
        }
        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_Spaceapp, hid))
        {
            builder.Append("已禁社区应用<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin2&amp;hid=" + hid + "&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">[解]</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=admin&amp;hid=" + hid + "&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">禁社区应用</a>");
        } 

        if (Utils.GetDomain().Contains("qyh.cc"))
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("usermanage.aspx?act=payset&amp;uid=" + hid + "") + "\">游戏押注控制</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
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
        }
        else if (ptype == 2)
        {
            sName = "发帖";
            Role = "B";
        }
        else if (ptype == 3)
        {
            sName = "回贴";
            Role = "C";
        }
        else if (ptype == 4)
        {
            sName = "短消息";
            Role = "D";
        }
        else if (ptype == 5)
        {
            sName = "聊天室";
            Role = "E";
        }
        else if (ptype == 6)
        {
            sName = "留言本";
            Role = "F";
        }
        else if (ptype == 7)
        {
            sName = "发广播";
            Role = "G";
        }
        else if (ptype == 8)
        {
            sName = "全站评论";
            Role = "H";
        }
        else if (ptype == 9)
        {
            sName = "上传文件";
            Role = "I";
        }
        else if (ptype == 10)
        {
            sName = "金融系统";
            Role = "J";
        }
        else if (ptype == 11)
        {
            sName = "社区游戏";
            Role = "K";
        }
        else if (ptype == 12)
        {
            sName = "使用日记";
            Role = "L";
        }
        else if (ptype == 13)
        {
            sName = "使用相册";
            Role = "M";
        }
        else if (ptype == 14)
        {
            sName = "使用闲聊";
            Role = "N";
        }
        else if (ptype == 15)
        {
            sName = "社区应用";
            Role = "O";
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
            string strLog = "系统管理员对[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]" + sName + "";
            string strLog2 = "系统管理员将你" + sName + "";
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
                    strEmpt = "false,false,false,0|永久|1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天|90|90天,false,false,false";
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
            builder.Append(" <a href=\"" + Utils.getUrl("usermanage.aspx?hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">重选</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("usermanage.aspx?hid=" + hid + "") + "\">&gt;返回上一级</a>");
            if (ptype == 1)
                builder.Append("<br />温馨提示:<br />1.锁定会员后,该会员将不能正常使用所有功能.");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void PaySetPage()
    {
        Master.Title = "游戏押注控制";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[0-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (info == "ok")
        {
            long small = Int64.Parse(Utils.GetRequest("small", "post", 4, @"^[0-9]\d*$", "最小下注填写错误"));
            long big = Int64.Parse(Utils.GetRequest("big", "post", 4, @"^[0-9]\d*$", "最大下注填写错误"));
            //记录进文件
            string strId = BCW.Files.FileWord.ReadTxt("/Files/txt/payset.user", "utf-8");
            if (!strId.Contains("#" + hid + "|"))
            {
                if (small != 0 && big != 0)
                    BCW.Files.FileWord.CreateTxt("/Files/txt/payset.user", "#" + hid + "|" + small + "|" + big + "", true);
            }
            else
            {
                string str = string.Empty;
                strId = Utils.Mid(strId, 1, strId.Length);
                string[] strTemp = strId.Split("#".ToCharArray());
                for (int i = 0; i < strTemp.Length; i++)
                {
                    if (("#" + strTemp[i] + "|").Contains("#" + hid + "|"))
                    {
                        if (small == 0 && big == 0)
                            str += "";
                        else
                            str += "#" + hid + "|" + small + "|" + big + "";
                    }
                    else
                    {
                        str += "#" + strTemp[i];
                    }
                }
                BCW.Files.FileWord.CreateTxt("/Files/txt/payset.user", str, false);
            }
            Utils.Success("押注控制", "押注控制操作成功，正在返回..", Utils.getUrl("usermanage.aspx?act=payset&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("控制会员：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")</a>");
            builder.Append(Out.Tab("</div>", ""));

            //读取配置
            long small = 0;
            long big = 0;
            string strId = BCW.Files.FileWord.ReadTxt("/Files/txt/payset.user", "utf-8");
            if (strId.Contains("#" + hid + "|"))
            {
                strId = Utils.Mid(strId, 1, strId.Length);
                string[] strTemp = strId.Split("#".ToCharArray());
                for (int i = 0; i < strTemp.Length; i++)
                {
                    if (("#" + strTemp[i] + "|").Contains("#" + hid + "|"))
                    {
                        string[] stemp = strTemp[i].ToString().Split("|".ToCharArray());
                        small = Convert.ToInt64(stemp[1]);
                        big = Convert.ToInt64(stemp[2]);
                    }
                }
            }

            strText = "最小下注:/,最大下注:/,,,,";
            strName = "small,big,uid,act,info,backurl";
            strType = "num,num,hidden,hidden,hidden,hidden";
            strValu = "" + small + "'" + big + "'" + hid + "'payset'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,usermanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("提示：填写0则取消游戏押注限制.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.RHr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }

    private void PositionPage(string act)
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

        DataSet ds = new BCW.BLL.Role().GetList("ID,UsName,RoleName,ForumID,ForumName,Include", "UsID=" + hid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0 ORDER BY FORUMID ASC");
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

                    builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=edit&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>");
                    if (forumid == -1)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=rolece&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + RoleName + "</a> 管辖全站<br />");

                    }
                    else if (forumid == 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=rolece&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + RoleName + "</a> 管辖全区版块<br />");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=rolece&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + RoleName + "</a> 管辖<a href=\"" + Utils.getPage("forum.aspx?id=" + forumid + "") + "\">" + ForumName + "</a><br />");
                    }
                }
                string Limit = new BCW.BLL.User().GetLimit(hid);
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
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=edit&amp;id=" + GroupId + "") + "\">" + new BCW.BLL.Group().GetTitle(GroupId) + "</a> " + ub.GetSub("GroupzName", "/Controls/group.xml") + "");
            }
        }
        else
        {
            builder.Append("普通会员");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=add&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设为版主</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=add2&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设为管理员</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void HonorPage(int uid)
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));

        if (info != "ok")
        {
            Master.Title = "设置荣誉版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定设置" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")成为荣誉版主吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?info=ok&amp;act=honor&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定设置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            string Limit = new BCW.BLL.User().GetLimit(hid);
            if (Limit.Contains("honor"))
            {
                Utils.Error("该会员已经荣誉版主", "");
            }

            Limit = Limit + "|honor";
            new BCW.BLL.User().UpdateLimit(hid, Limit);

            //记录日志
            string UsName = new BCW.BLL.User().GetUsName(hid);
            string strLog = string.Empty;
            strLog = "系统管理员将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]设置成为荣誉版主";
            new BCW.BLL.Forumlog().Add(0, 0, strLog);
            //发送内线给新版主
            new BCW.BLL.Guest().Add(hid, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]", "您"));
            Utils.Success("设置荣誉版主", "设置荣誉版主成功..", Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }


    private void HonorNoPage(int uid)
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));

        if (info != "ok")
        {
            Master.Title = "撤销荣誉版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定撤销" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")的荣誉版主吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?info=ok&amp;act=honorno&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤销</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            string Limit = new BCW.BLL.User().GetLimit(hid);
            if (!Limit.Contains("honor"))
            {
                Utils.Error("该会员不是荣誉版主", "");
            }

            Limit = Limit.Replace("honor", "");
            new BCW.BLL.User().UpdateLimit(hid, Limit);

            //记录日志
            string UsName = new BCW.BLL.User().GetUsName(hid);
            string strLog = string.Empty;
            strLog = "系统管理员将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]撤销荣誉版主身份";
            new BCW.BLL.Forumlog().Add(0, 0, strLog);
            //发送内线给新版主
            new BCW.BLL.Guest().Add(hid, UsName, strLog.Replace("[url=/bbs/uinfo.aspx?uid=" + hid + "]" + UsName + "[/url]", "您"));
            Utils.Success("撤销荣誉版主", "撤销荣誉版主成功..", Utils.getUrl("usermanage.aspx?act=position&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }
}