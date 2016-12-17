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
using System.Text.RegularExpressions;
using BCW.Common;

/// <summary>
/// 邵广林 friend 加农场跳转20160428
/// 陈志基 修改密码，支付，管理密码需要发短信
/// 20160910
/// </summary>

public partial class bbs_myedit : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/reg.xml";
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
        if (meid == 0)
            Utils.Login();
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "city":
                CityPage(meid);
                break;
            case "more":
                MorePage(meid);
                break;
            case "forum":
                ForumPage(meid);
                break;
            case "name":
                NamePage(meid);
                break;
            case "editname":
                NameSavePage(meid);
                break;
            case "getByPhone":
                getByPhone(meid);
                break;
            case "qm":
                QmPage(meid);
                break;
            case "editqm":
                QmSavePage(meid);
                break;
            case "state":
                StatePage(meid);
                break;
            case "getpwdbyphone":
                getpwdbyphone(meid);
                break;
            case "savapwd":
                savapwd(meid);
                break;
            case "reSetPwd":
                reSetPwd();
                break;
            case "editstate":
                StateSavePage(meid);
                break;
            case "basic":
                BasicPage(meid);
                break;
            case "editbasic":
                BasicSavePage(meid);
                break;
            case "pwd":
                PwdPage(meid);
                break;
            case "editpwd":
                EditPwdPage(meid);
                break;
            case "paypwd":
                PayPwdPage(meid);
                break;
            case "editpaypwd":
                EditPayPwdPage(meid);
                break;
            case "adminpwd":
                AdminPwdPage(meid);
                break;
            case "editadminpwd":
                EditAdminPwdPage(meid);
                break;
            case "regyz":
                RegYzPage(meid);
                break;
            case "checkpwd":
                CheckPwdPage(meid);
                break;
            case "bind":
                BindPage(meid);
                break;
            case "bindok":
                BindOkPage(meid);
                break;
            case "exit":
                ExitPage(meid);
                break;
            case "photo":
                PhotoPage(meid);
                break;
            case "photos":
                PhotosPage(meid);
                break;
            case "friend":
                FriendPage(meid);
                break;
            case "guest":
                GuestPage(meid);
                break;
            case "mebook":
                MebookPage(meid);
                break;
            case "groupchat":
                GroupChatPage(meid);
                break;
            case "loginsafe":
                LoginSafePage(meid);
                break;
            case "scene":
                ScenePage(meid);
                break;
            case "bankuser":
                BankUserPage(meid);
                break;
            case "bankusersave":
                BankUserSavePage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int meid)
    {
        Master.Title = "个人设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择修改选项");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        //  builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?backurl=" + Utils.PostPage(1) + "") + "\">设置</a>");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?backurl=" + Utils.PostPage(1) + "&amp;ptype=1") + "\">1.基本设置</a><br/>");
        if (ptype == 1 || ptype == 1)
        {
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            {
                builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=bankuser&amp;backurl=" + Utils.getPage(0) + "") + "\">设置银行资料</a><br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=name&amp;backurl=" + Utils.getPage(0) + "") + "\">修改我的昵称</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=state&amp;backurl=" + Utils.getPage(0) + "") + "\">更改我的状态</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=basic&amp;backurl=" + Utils.getPage(0) + "") + "\">修改基本资料</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=qm&amp;backurl=" + Utils.getPage(0) + "") + "\">修改个性签名</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=scene&amp;backurl=" + Utils.getPage(0) + "") + "\">修改我的情景</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=photo&amp;backurl=" + Utils.getPage(0) + "") + "\">更换我的头像</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?backurl=" + Utils.PostPage(1) + "&amp;ptype=2") + "\">2.登录</a><br/>");
        if (ptype == 2 || ptype == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=pwd&amp;backurl=" + Utils.getPage(0) + "") + "\">修改登录密码</a><br />");
            // if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=loginsafe&amp;backurl=" + Utils.getPage(0) + "") + "\">设置登录安全</a><br />");
            }
        }

        builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?backurl=" + Utils.PostPage(1) + "&amp;ptype=3") + "\">3.支付</a><br/>");
        if (ptype == 3 || ptype == 3)
        {
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.getPage(0) + "") + "\">修改支付密码</a><br />");
            //   if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=paysafe&amp;backurl=" + Utils.getPage(0) + "") + "\">设置支付安全</a><br />");
            }
            // if (new BCW.BLL.Role().IsAllMode(meid) || new BCW.BLL.Group().GetForumId(meid) > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=adminpwd&amp;backurl=" + Utils.getPage(0) + "") + "\">修改管理密码</a><br />");
            }
        } 
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/myedit.aspx?backurl=" + Utils.PostPage(1) + "&amp;ptype=4") + "\">4.其他</a><br/>");
        if (ptype == 4 || ptype == 4)
        {
            builder.Append("<a href=\"" + Utils.getUrl("pwd/MyPhone.aspx") + "\">更绑手机号码</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=bind&amp;backurl=" + Utils.getPage(0) + "") + "\">更换手机绑定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">修改系统设置</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("/bbs/pwd/GetPwd.aspx") + "\">密保相关设置</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=payactive&amp;backurl=" + Utils.getPage(0) + "") + "\">财产隐藏与显示</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=exit&amp;backurl=" + Utils.getPage(0) + "") + "\">退出登录..</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BankUserPage(int meid)
    {
        Master.Title = "设置银行资料";

        BCW.Model.BankUser model = new BCW.BLL.BankUser().GetBankUser(meid);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改银行");
        builder.Append(Out.Tab("</div>", ""));

        if (model != null)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("真实姓名:" + model.BankName + "");
            if (model.ZFBName != "" && model.ZFBNo != "")
            {
                builder.Append("<br />支付宝名称:" + model.ZFBName + "");
                builder.Append("<br />真实帐号:" + model.ZFBNo + "");
            }
            builder.Append(Out.Tab("</div>", Out.LHr()));
        }

        strText = "真实姓名(与银行帐号一致，不可修改):/,银行名称1(如工商银行):/,银行卡帐号1:/,开户行地址1(可留空):/,银行名称2(可留空):/,银行卡帐号2(可留空):/,开户行地址2(可留空):/,银行名称3(可留空):/,银行卡帐号3(可留空):/,开户行地址3(可留空):/,银行名称4(可留空):/,银行卡帐号4(可留空):/,开户行地址4(可留空):/,支付宝名称(填写后不可修改，可留空):/,支付宝帐号(填写后不可修改，可留空):/,";
        strName = "BankName,BankTitle1,BankNo1,BankAdd1,BankTitle2,BankNo2,BankAdd2,BankTitle3,BankNo3,BankAdd3,BankTitle4,BankNo4,BankAdd4,ZFBName,ZFBNo,act";

        if (model != null)
        {
            strType = "hidden,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden";
            strValu = "" + model.BankName + "'" + model.BankTitle1 + "'" + model.BankNo1 + "'" + model.BankAdd1 + "'" + model.BankTitle2 + "'" + model.BankNo2 + "'" + model.BankAdd2 + "'" + model.BankTitle3 + "'" + model.BankNo3 + "'" + model.BankAdd3 + "'" + model.BankTitle4 + "'" + model.BankNo4 + "'" + model.BankAdd4 + "'" + model.ZFBName + "'" + model.ZFBNo + "'bankusersave";
            if (model.ZFBName != "" && model.ZFBNo != "")
            {
                strType = "hidden,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden,hidden";
                strValu = "" + model.BankName + "'" + model.BankTitle1 + "'" + model.BankNo1 + "'" + model.BankAdd1 + "'" + model.BankTitle2 + "'" + model.BankNo2 + "'" + model.BankAdd2 + "'" + model.BankTitle3 + "'" + model.BankNo3 + "'" + model.BankAdd3 + "'" + model.BankTitle4 + "'" + model.BankNo4 + "'" + model.BankAdd4 + "'" + model.ZFBName + "'" + model.ZFBNo + "'bankusersave";
            }

        }
        else
        {
            strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden";
            strValu = "'''''''''''''''bankusersave";
        }
        strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BankUserSavePage(int meid)
    {
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
        model.UsID = meid;
        BCW.Model.BankUser m = new BCW.BLL.BankUser().GetBankUser(meid);
        if (m != null)
        {
            model.BankName = m.BankName;
        }
        else
        {
            model.BankName = BankName;
        }
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

        if (m != null && m.ZFBName != "" && m.ZFBNo != "")
        {
            model.ZFBName = m.ZFBName;
            model.ZFBNo = m.ZFBNo;
        }
        else
        {
            model.ZFBName = ZFBName;
            model.ZFBNo = ZFBNo;
        }
        model.State = 0;
        if (!new BCW.BLL.BankUser().Exists(meid))
        {
            if (new BCW.BLL.BankUser().ExistsBankName(BankName))
            {
                Utils.Error("数据库已存在姓名:" + BankName + "", "");
            }
            if (new BCW.BLL.BankUser().ExistsZFBName(ZFBName))
            {
                Utils.Error("数据库已存在支付宝名称:" + ZFBName + "", "");
            }
            new BCW.BLL.BankUser().Add(model);
        }
        else
        {
            new BCW.BLL.BankUser().Update(model);
        }


        Utils.Success("修改银行资料", "恭喜，修改银行资料成功，正在返回..", Utils.getUrl("myedit.aspx?act=bankuser&amp;backurl=" + Utils.getPage(0) + ""), "2");
    }

    private void MorePage(int meid)
    {
        Master.Title = "系统设置";
        builder.Append(Out.Tab("<div class=\"title\">系统设置</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=forum&amp;backurl=" + Utils.getPage(0) + "") + "\">论坛个性化</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=friend&amp;backurl=" + Utils.getPage(0) + "") + "\">好友验证设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;backurl=" + Utils.getPage(0) + "") + "\">内线设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=smsmail&amp;backurl=" + Utils.getPage(0) + "") + "\">短信提醒内线</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=mebook&amp;backurl=" + Utils.getPage(0) + "") + "\">空间留言设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=groupchat&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.GetSub("GroupName", "/Controls/group.xml") + "提醒设置</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void NamePage(int meid)
    {
        Master.Title = "修改昵称";
        string mename = new BCW.BLL.User().GetUsName(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改昵称");
        builder.Append(Out.Tab("</div>", ""));
        strText = "昵称(10字内)/,";
        strName = "Name,act";
        strType = "text,hidden";
        strValu = "" + mename + "'editname";
        strEmpt = "false,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void NameSavePage(int meid)
    {
        string Name = Utils.GetRequest("Name", "post", 2, @"^[\s\S]{1,10}$", "昵称最多10个字符");
        //昵称限特殊字符
        string strVal = ub.GetSub("RegKeepChar", xmlPath);
        if (strVal != "")
        {
            Name = Regex.Replace(Name, @"[^\u4e00-\u9fa5A-Za-z0-9 " + strVal + "]", string.Empty);
        }
        if (Name.Contains(" ") || Name.Contains("　"))
        {
            Utils.Error("昵称填写错误", "");
        }
        if (Name.Contains("##"))
        {
            Utils.Error("昵称不能使用##", "");
        }
        if (Name.Contains(">") || Name.Contains("&gt;"))
        {
            Utils.Error("昵称命名不规范", "");
        }
        if (Name.Contains("<") || Name.Contains("&lt;"))
        {
            Utils.Error("昵称命名不规范", "");
        }
        if (Name.Contains("下页") || Name.Contains("下一页"))
        {
            Utils.Error("昵称命名不规范", "");
        }

        //保留昵称限制
        string strName = ub.GetSub("RegKeepName", xmlPath);
        string[] sTemp = Regex.Split(strName, "#");
        for (int i = 0; i < sTemp.Length; i++)
        {
            if (Name.Contains(sTemp[i]))
                Utils.Error("系统保留昵称：" + Name + "，请重新修改昵称", "");
        }

        //昵称是否限制相同
        if (ub.GetSub("RegNameSame", xmlPath) == "1")
        {
            if (new BCW.BLL.User().ExistsUsName(Name, meid))
            {
                Utils.Error("昵称：" + Name + "已被使用，请重新修改", "");
            }
        }
        if (Name.Length < 1)
        {
            Utils.Error("昵称填写错误", "");
        }
        new BCW.BLL.User().UpdateUsName(meid, Name);
        Utils.Success("修改昵称", "恭喜，修改昵称成功，正在返回..", Utils.getUrl("uinfo.aspx?uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
    }

    private void QmPage(int meid)
    {
        Master.Title = "修改个性签名";
        string Sign = new BCW.BLL.User().GetSign(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改签名");
        builder.Append(Out.Tab("</div>", ""));
        strText = "个性签名(100字内)/,,";
        strName = "Sign,act,backurl";
        strType = "text,hidden,hidden";
        strValu = "" + Sign + "'editqm'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void QmSavePage(int meid)
    {
        string Sign = Utils.GetRequest("Sign", "post", 2, @"^[\s\S]{1,100}$", "签名最多100个字符");

        new BCW.BLL.User().UpdateSign(meid, Sign);
        Utils.Success("修改个性签名", "恭喜，修改签名成功，正在返回..", Utils.getPage("myedit.aspx"), "1");
    }

    private void StatePage(int meid)
    {
        Master.Title = "修改我的状态";
        int State = new BCW.BLL.User().GetState(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;更改状态");
        builder.Append(Out.Tab("</div>", ""));

        strText = "选择状态/,";
        strName = "State,act";
        strType = "select,hidden";
        strValu = "" + State + "'editstate";
        strEmpt = "0|正常状态|1|隐身状态,false";
        strIdea = "/选择隐身则状态为离线/只有VIP会员才可以选择隐身哦/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void StateSavePage(int meid)
    {
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-1]$", "请正确选择状态"));
        int VipLeven = BCW.User.Users.VipLeven(meid);
        if (VipLeven == 0)
        {
            Utils.Error("只有VIP会员才可以选择隐身哦", "");
        }
        //if (new BCW.BLL.Role().IsAllMode(meid))
        //{
        //    Utils.Error("拥有版主以上的权限禁止隐身", "");
        //}
        new BCW.BLL.User().UpdateState(meid, State);
        Utils.Success("修改状态", "恭喜，修改状态成功，正在返回..", Utils.getUrl("uinfo.aspx?uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
    }

    private void BasicPage(int meid)
    {
        int pid = int.Parse(Utils.GetRequest("pid", "all", 1, @"^[0-9]\d*$", "-1"));
        int cid = int.Parse(Utils.GetRequest("cid", "all", 1, @"^[0-9]\d*$", "-1"));
        string city = "";
        if (pid != -1 && cid != -1)
        {
            city = BCW.User.City.city[pid][cid].ToString();
        }
        Master.Title = "修改基本资料";
        BCW.Model.User model = new BCW.BLL.User().GetEditBasic(meid);
        string SexEmpt = string.Empty;
        int VipLeven = BCW.User.Users.VipLeven(meid);

        if (ub.GetSub("RegEditSex", xmlPath) == "1" || VipLeven > 0)//如果性别修改不限制次数
        {
            if (model.Sex == 0)
                SexEmpt = "0|请选择|1|美女|2|帅哥";
            else
                SexEmpt = "1|美女|2|帅哥";
        }
        else
        {
            if (model.Sex == 0)
                SexEmpt = "0|请选择|1|美女|2|帅哥";
            else
                SexEmpt = "1|已设置";
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改资料");
        builder.Append(Out.Tab("</div>", ""));
        strText = "昵称(10字内):/,性别:,生日(格式:1980-01-01):/,所在城市:/,个性签名:/,邮箱(找回密码用):/,";
        strName = "Name,Sex,Birth,City,Sign,Email,act";
        strType = "text,select,date,text,text,text,hidden";
        strValu = "" + model.UsName + "'" + model.Sex + "'" + DT.FormatDate(model.Birth, 11) + "'" + ((city == "") ? model.City : city) + "'" + model.Sign + "'" + model.Email + "'editbasic";
        strEmpt = "false," + SexEmpt + ",false,false,false,true,false";
        strIdea = "'''<a href=\"" + Utils.getUrl("myedit.aspx?act=city&amp;backurl=" + Utils.getPage(0) + "") + "\">选择城市<／a>'''|/";
        strOthe = "修改资料,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BasicSavePage(int meid)
    {
        string Name = Utils.GetRequest("Name", "post", 2, @"^[\s\S]{1,10}$", "昵称最多10个字符");
        int Sex = int.Parse(Utils.GetRequest("Sex", "post", 2, @"^[1-2]$", "性别选择错误"));
        DateTime Birth = Utils.ParseTime(Utils.GetRequest("Birth", "post", 2, DT.RegexDate, "生日填写错误或者不合理"));
        string City = Utils.GetRequest("City", "post", 2, @"^[^\^]{1,10}$", "城市最多10个字符");
        string Sign = Utils.GetRequest("Sign", "post", 2, @"^[\s\S]{1,100}$", "个性签名最多100个字符");
        string Email = Utils.GetRequest("Email", "post", 1, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "@");
        //昵称限特殊字符
        string strVal = ub.GetSub("RegKeepChar", xmlPath);
        if (strVal != "")
        {
            Name = Regex.Replace(Name, @"[^\u4e00-\u9fa5A-Za-z0-9 " + strVal + "]", string.Empty);
        }
        if (Name.Contains(" ") || Name.Contains("　"))
        {
            Utils.Error("昵称填写错误", "");
        }
        if (Name.Contains("##"))
        {
            Utils.Error("昵称不能使用##", "");
        }
        if (Name.Contains(">") || Name.Contains("&gt;"))
        {
            Utils.Error("昵称命名不规范", "");
        }
        if (Name.Contains("<") || Name.Contains("&lt;"))
        {
            Utils.Error("昵称命名不规范", "");
        }
        if (Name.Contains("下页") || Name.Contains("下一页"))
        {
            Utils.Error("昵称命名不规范", "");
        }
        //保留昵称限制
        string strName = ub.GetSub("RegKeepName", xmlPath);
        string[] sTemp = Regex.Split(strName, "#");
        for (int i = 0; i < sTemp.Length; i++)
        {
            if (Name.Contains(sTemp[i]))
                Utils.Error("系统保留昵称：" + Name + "，请重新修改昵称", "");
        }

        //昵称是否限制相同
        if (ub.GetSub("RegNameSame", xmlPath) == "1")
        {
            if (new BCW.BLL.User().ExistsUsName(Name, meid))
            {
                Utils.Error("昵称：" + Name + "已被使用，请重新修改", "");
            }
        }
        if (Name.Length < 1)
        {
            Utils.Error("昵称填写错误", "");
        }
        int VipLeven = BCW.User.Users.VipLeven(meid);
        if (ub.GetSub("RegEditSex", xmlPath) == "0" && VipLeven == 0)//如果性别修改不限制次数
        {
            int OldSex = new BCW.BLL.User().GetSex(meid);
            if (OldSex > 0)
                Sex = OldSex;
        }
        if (DateTime.Now.Year - Birth.Year < 10 || DateTime.Now.Year - Birth.Year > 100)
        {
            Utils.Error("生日填写错误或者不合理", "");
        }
        string allCity = "#" + BCW.User.City.GetCity() + "#";
        if (allCity.IndexOf("#" + City + "#") == -1)
        {
            Utils.Error("不存在的城市名称“" + City + "”", "");
        }

        BCW.Model.User model = new BCW.Model.User();
        model.ID = meid;
        model.UsName = Name;
        model.Email = Email;
        model.Sex = Sex;
        model.Birth = Birth;
        model.City = City;
        model.Sign = Sign;
        new BCW.BLL.User().UpdateEditBasic(model);
        Utils.Success("修改资料", "恭喜，修改资料成功，正在返回..", Utils.getUrl("myedit.aspx"), "2");
    }

    private void CityPage(int meid)
    {
        int pid = int.Parse(Utils.GetRequest("pid", "get", 1, @"^[0-9]\d*$", "-1"));
        if (pid == -1)
        {
            Master.Title = "省份列表";
            builder.Append(Out.Tab("<div class=\"title\">省份列表</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i <= 33; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=city&amp;pid=" + (i) + "") + "&amp;backurl=" + Utils.getPage(0) + "\">" + BCW.User.AppCase.CaseSheng(i) + "</a> ");
                if (i > 0 && (i + 1) % 4 == 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            Master.Title = "城市列表";
            builder.Append(Out.Tab("<div class=\"title\">城市列表</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string[] city = BCW.User.City.city[pid];
            for (int i = 0; i < city.Length; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=basic&amp;pid=" + pid + "&amp;cid=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + city[i] + "</a> ");
                if (i > 0 && (i + 1) % 4 == 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=basic&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PwdPage(int meid)
    {
        Master.Title = "修改密码";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改密码");
        builder.Append(Out.Tab("</div>", "<br/>"));
        BCW.Model.tb_Validate getmo = null;
        //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 2))//修改密码验证码
        //{
        //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 2);
        //    string Reg = string.Empty;
        //    DateTime endtime = getmo.codeTime;
        //    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        //    {
        //        Reg = new BCW.JS.somejs().daojishi2("Reg", endtime);
        //    }
        //    else
        //    {
        //        Reg = new BCW.JS.somejs().daojishi("Reg", endtime);
        //    }
        //    if (endtime > DateTime.Now)
        //    {
        //        builder.Append(Out.Tab("<div class=\"\">", ""));
        //        builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码<br/>");
        //        builder.Append(Out.Tab("</div>", "<br/>"));
        //    }
        //}
        //strText = "原密码:/,新密码:/,确认密码:/,*请输入手机验证码:/,*请输入验证码:/,";
        //strName = "oPwd,nPwd,rPwd,phoneCode,code,act";
        //strType = "password,password,password,text,text,hidden";
        //strValu = "'''''editpwd";
        //strEmpt = "false,false,false,false,false,false";
        //strIdea = "'''<a href=\"" + Utils.getUrl("myedit.aspx?act=editpwd&amp;ac=ok") + "\">获取手机验证码</a>'<img src=\"pwd/Code.aspx\"/>'|/修改密码将会重新登录/";
        //strOthe = "确定修改,myedit.aspx,post,1,red";
        strText = "原密码:/,新密码:/,确认密码:/,*请输入验证码:/,";
        strName = "oPwd,nPwd,rPwd,code,act";
        strType = "password,password,password,text,hidden";
        strValu = "''''editpwd";
        strEmpt = "false,false,false,false,false";
        //strIdea = "'''<img src=\"pwd/Code.aspx\"/>'|/修改密码将会重新登录-<a href=\"" + Utils.getUrl("pwd\\GetPwd.aspx?act=fangfa") + "\">忘记密码</a>";
        strIdea = "<a href=\"" + Utils.getUrl("pwd\\GetPwd.aspx?act=fangfa") + "\">忘记密码</a>'''<img src=\"pwd/Code.aspx\"/>'|/修改密码将会重新登录/";
        strOthe = "确定修改,myedit.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditPwdPage(int meid)
    {

        string mycode = "";
        try
        {
            mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();// 图形验证码
        }
        catch { }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string mobile = new BCW.BLL.User().GetMobile(meid);
        #region
        //if (Utils.ToSChinese(ac).Contains("获取手机验证码") || ac == "ok")    //判断哪一个按键
        //{
        //    #region  获取手机验证码
        //    //if (!code.Equals(mycode))//验证码相等
        //    //{
        //    //    Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
        //    //}
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 2))//存在修改密码验证码
        //    {
        //        BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 2);
        //        if (getmo.codeTime > DateTime.Now)//验证码存在切是新发的
        //        {
        //            string dateDiff = null;
        //            TimeSpan x = getmo.codeTime - DateTime.Now;
        //            dateDiff = x.TotalSeconds.ToString();
        //            Utils.Error("很抱歉,请在" + dateDiff.Split('.')[0] + "秒之后再次获取手机验证码", "");
        //        }
        //    }
        //    char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
        //    string mesCode = string.Empty; //手机验证码
        //    Random rnd = new Random();
        //    //生成验证码字符串
        //    for (int i = 0; i < 4; i++)
        //    {
        //        mesCode += character[rnd.Next(character.Length)];
        //    }
        //    //int tm = 2;//短信过期时间
        //    //int total = 15;//每天可以发的总短信量
        //    //int ipCount = 10;
        //    //int phoneCount = 10;
        //    int tm = int.Parse(ub.GetSub("msgTime", "/Controls/guestlist.xml"));
        //    int total = int.Parse(ub.GetSub("dayCount", "/Controls/guestlist.xml"));
        //    int ipCount = int.Parse(ub.GetSub("IPCount", "/Controls/guestlist.xml"));
        //    int phoneCount = int.Parse(ub.GetSub("phoneCount", "/Controls/guestlist.xml"));
        //    int msgremain = int.Parse(ub.GetSub("msgremain", "/Controls/guestlist.xml"));
        //    int callID = int.Parse(ub.GetSub("callID", "/Controls/guestlist.xml"));
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(mobile))//不是第一次获取短信
        //    {
        //        DataSet data = new BCW.BLL.tb_Validate().GetList(" Top 1 *", "Phone=" + mobile + " order by time desc");
        //        DateTime changeTime = Convert.ToDateTime(data.Tables[0].Rows[0]["Time"].ToString());
        //        int changeday = changeTime.DayOfYear;
        //        if ((DateTime.Now.DayOfYear - changeday) >= 1)//上一条短信不是在当天
        //        {
        //            BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //            validate.Phone = mobile;
        //            validate.IP = Utils.GetUsIP();
        //            validate.Time = DateTime.Now.AddMinutes(0);
        //            validate.Flag = 1;
        //            validate.mesCode = mesCode;
        //            validate.codeTime = DateTime.Now.AddMinutes(tm);
        //            validate.type = 2;
        //            string result = "";
        //            Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //            result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //            string[] results = result.Split('#');
        //            if (results[8] != "0")
        //            {
        //                Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //            }
        //            if ((int.Parse(results[2]) / 80) < msgremain)
        //            {
        //                new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //            }
        //            if (results[8] == "0")
        //            {
        //                new BCW.BLL.tb_Validate().Add(validate);
        //                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=pwd"), "2");
        //            }
        //        }
        //        else//当天时间内
        //        {
        //            DataSet dt2 = new BCW.BLL.tb_Validate().GetList("*", "Phone=" + mobile + " and time>='" + DateTime.Now.ToShortDateString() + "' order by time desc");
        //            if (dt2.Tables[0].Rows.Count >= total)//当天时间内超过特定数
        //            {
        //                Utils.Error("抱歉！当天时间内过于频繁获取短信，请明天再试！", "");
        //            }
        //            DateTime check = DateTime.Now.AddMinutes(-30);
        //            if (check.DayOfYear < DateTime.Now.DayOfYear)
        //            {
        //                check = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //            }
        //            else
        //            {
        //                check = DateTime.Now.AddMinutes(-30);
        //            }
        //            //Utils.Error("check:"+ check, "");
        //            string str = "Phone=" + mobile + " and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
        //            DataSet dt1 = new BCW.BLL.tb_Validate().GetList("*", str);
        //            if (data.Tables[0].Rows[0]["Flag"].ToString() == "0")//最新一条显示当天不能发送了
        //            {
        //                Utils.Error("抱歉！由于之前你存在频繁获取短信，请明天再试！", "");
        //            }
        //            string IP = Utils.GetUsIP();
        //            //查看限制IP
        //            string str1 = "IP= '" + IP + "' and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
        //            DataSet dt3 = new BCW.BLL.tb_Validate().GetList("*", str1);
        //            if (dt3.Tables[0].Rows.Count >= ipCount)//半小时内超过10条
        //            {
        //                Utils.Error("当前IP过于频繁获取短信，请明天再试！" + dt3.Tables[0].Rows.Count, "");
        //            }
        //            if (dt1.Tables[0].Rows.Count >= phoneCount)//半小时内超过10条
        //            {
        //                //跟新标示
        //                int ID = int.Parse(dt1.Tables[0].Rows[0]["ID"].ToString());
        //                new BCW.BLL.tb_Validate().UpdateFlag(0, ID);
        //                Utils.Error("请勿频繁获取短信，请明天再试！", "");
        //            }
        //            else
        //            {
        //                BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //                validate.Phone = mobile;
        //                validate.IP = Utils.GetUsIP();
        //                validate.Time = DateTime.Now.AddMinutes(0);
        //                validate.Flag = 1;
        //                validate.mesCode = mesCode;
        //                validate.codeTime = DateTime.Now.AddMinutes(tm);
        //                validate.type = 2;
        //                string result = "";
        //                Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //                result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //                string[] results = result.Split('#');
        //                if (results[8] != "0")
        //                {
        //                    Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //                }
        //                if ((int.Parse(results[2]) / 80) < msgremain)
        //                {
        //                    new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //                }
        //                if (results[8] == "0")
        //                {
        //                    new BCW.BLL.tb_Validate().Add(validate);
        //                    Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=pwd"), "2");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //        validate.Phone = mobile;
        //        validate.IP = Utils.GetUsIP();
        //        validate.Time = DateTime.Now.AddMinutes(0);
        //        validate.Flag = 1;
        //        validate.mesCode = mesCode;
        //        validate.codeTime = DateTime.Now.AddMinutes(tm);
        //        validate.type = 2;
        //        string result = "";
        //        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //        result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //        string[] results = result.Split('#');
        //        if (results[8] != "0")
        //        {
        //            Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //        }
        //        if ((int.Parse(results[2]) / 80) < msgremain)
        //        {
        //            new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //        }
        //        if (results[8] == "0")
        //        {
        //            new BCW.BLL.tb_Validate().Add(validate);
        //            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=pwd"), "2");
        //        }
        //    }


        //    #endregion
        //}
        //else//修改密码

        #endregion
        {
            string code = Utils.GetRequest("code", "post", 2, @"^[0-9]{4}$", "请输入验证码!");  //界面图形验证码            
           // string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
            string oPwd = Utils.GetRequest("oPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "原密码限6-20位,必须由字母或数字组成");
            string nPwd = Utils.GetRequest("nPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "新密码限6-20位,必须由字母或数字组成");
            string rPwd = Utils.GetRequest("rPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
            if (!code.Equals(mycode))//验证码相等
            {
                Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
            }
            BCW.Model.tb_Validate getmo = null;
            //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 2))//存在修改密码验证码
            //{
            //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 2);
            //    if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
            //    {
            //        if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
            //        {
            //            Utils.Error("很抱歉,您输入手机验证码不对222", "");
            //        }
            //    }
            //    else { Utils.Error("手机验证码过期，请重新获取", ""); }
            //}
            //else//没发送过修改密码验证码
            //{
            //    Utils.Error("很抱歉,您输入手机验证码不对123", "");
            //}
            if (!nPwd.Equals(rPwd))
            {
                Utils.Error("新密码与确认密码不相符", "");
            }
            if (Utils.IsRegex(nPwd, @"^[0-9]\d*$"))
            {
                Utils.Error("新密码不能是纯数字密码", "");
            }
            string ordPwd = new BCW.BLL.User().GetUsPwd(meid);
            if (!Utils.MD5Str(oPwd).Equals(ordPwd))
            {
                Utils.Error("原密码不正确", "");
            }
            new BCW.BLL.User().UpdateUsPwd(meid, Utils.MD5Str(nPwd));
            Utils.Success("修改密码", "恭喜，修改密码成功，正在返回登录..", Utils.getUrl("/login.aspx"), "2");
        }


    }
    /// <summary>
    /// 修改支付密码
    /// </summary>
    /// <param name="meid"></param>
    private void PayPwdPage(int meid)
    {
        Master.Title = "修改支付密码";
        string PayPwd = new BCW.BLL.User().GetUsPled(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改支付密码");
        builder.Append(Out.Tab("</div>", ""));
        if (string.IsNullOrEmpty(PayPwd))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("您是首次设置支付密码:");
            builder.Append(Out.Tab("</div>", ""));
            strText = "支付密码:/,确认密码:/,,";
            strName = "nPwd,rPwd,act,backurl";
            strType = "password,password,hidden,hidden";
            strValu = "''editpaypwd'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
        }
        else
        {
            #region show time 
            //BCW.Model.tb_Validate getmo = null;
            //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 6))//修改密码验证码
            //{
            //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 6);
            //    string Reg = string.Empty;
            //    DateTime endtime = getmo.codeTime;
            //    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //    {
            //        Reg = new BCW.JS.somejs().daojishi2("Reg", endtime);
            //    }
            //    else
            //    {
            //        Reg = new BCW.JS.somejs().daojishi("Reg", endtime);
            //    }
            //    if (endtime > DateTime.Now)
            //    {
            //        builder.Append(Out.Tab("<div class=\"\">", ""));
            //        builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码<br/>");
            //        builder.Append(Out.Tab("</div>", "<br/>"));
            //    }
            //}
            #endregion
            //strText = "原密码:/,新密码:/,确认密码:/,请输入手机验证码:/,,";
            //strName = "oPwd,nPwd,rPwd,phoneCode,act,backurl";
            //strType = "password,password,password,text,hidden,hidden";
            //strValu = "''''editpaypwd'" + Utils.getPage(0) + "";
            //strEmpt = "false,false,false,false,false,false,false";
            strText = "原密码:/,新密码:/,确认密码:/,,";
            strName = "oPwd,nPwd,rPwd,act,backurl";
            strType = "password,password,password,hidden,hidden";
            strValu = "'''editpaypwd'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false";
        }
        //strIdea = "'''<a href=\"" + Utils.getUrl("myedit.aspx?act=editpaypwd&amp;ac=ok") + "\">获取手机验证码</a>''|/";
        strIdea = "''''|/";
        strOthe = "确定设置,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=getByPhone") + "\">忘记支付密码(?)</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditPayPwdPage(int meid)
    {

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        #region
        //if (ac == "ok")
        //{
        //    #region 获取支付密码短信
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 6))//存在修改支付密码验证码
        //    {
        //        BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 6);
        //        if (getmo.codeTime > DateTime.Now)//验证码存在切是新发的
        //        {
        //            string dateDiff = null;
        //            TimeSpan x = getmo.codeTime - DateTime.Now;
        //            dateDiff = x.TotalSeconds.ToString();
        //            Utils.Error("很抱歉,请在" + dateDiff.Split('.')[0] + "秒之后再次获取手机验证码", "");
        //        }
        //    }
        //    string mobile = new BCW.BLL.User().GetMobile(meid);
        //    char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
        //    string mesCode = string.Empty; //手机验证码
        //    Random rnd = new Random();
        //    //生成验证码字符串
        //    for (int i = 0; i < 4; i++)
        //    {
        //        mesCode += character[rnd.Next(character.Length)];
        //    }
        //    //int tm = 2;//短信过期时间
        //    //int total = 15;//每天可以发的总短信量
        //    //int ipCount = 10;
        //    //int phoneCount = 10;
        //    int tm = int.Parse(ub.GetSub("msgTime", "/Controls/guestlist.xml"));
        //    int total = int.Parse(ub.GetSub("dayCount", "/Controls/guestlist.xml"));
        //    int ipCount = int.Parse(ub.GetSub("IPCount", "/Controls/guestlist.xml"));
        //    int phoneCount = int.Parse(ub.GetSub("phoneCount", "/Controls/guestlist.xml"));
        //    int msgremain = int.Parse(ub.GetSub("msgremain", "/Controls/guestlist.xml"));
        //    int callID = int.Parse(ub.GetSub("callID", "/Controls/guestlist.xml"));
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(mobile))//不是第一次获取短信
        //    {
        //        DataSet data = new BCW.BLL.tb_Validate().GetList(" Top 1 *", "Phone=" + mobile + " order by time desc");
        //        DateTime changeTime = Convert.ToDateTime(data.Tables[0].Rows[0]["Time"].ToString());
        //        int changeday = changeTime.DayOfYear;
        //        if ((DateTime.Now.DayOfYear - changeday) >= 1)//上一条短信不是在当天
        //        {
        //            BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //            validate.Phone = mobile;
        //            validate.IP = Utils.GetUsIP();
        //            validate.Time = DateTime.Now.AddMinutes(0);
        //            validate.Flag = 1;
        //            validate.mesCode = mesCode;
        //            validate.codeTime = DateTime.Now.AddMinutes(tm);
        //            validate.type = 6;
        //            string result = "";
        //            Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //            result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //            string[] results = result.Split('#');
        //            if (results[8] != "0")
        //            {
        //                Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //            }
        //            if ((int.Parse(results[2]) / 80) < msgremain)
        //            {
        //                new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //            }
        //            if (results[8] == "0")
        //            {
        //                new BCW.BLL.tb_Validate().Add(validate);
        //                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=paypwd"), "2");
        //            }
        //        }
        //        else//当天时间内
        //        {
        //            DataSet dt2 = new BCW.BLL.tb_Validate().GetList("*", "Phone=" + mobile + " and time>='" + DateTime.Now.ToShortDateString() + "' order by time desc");
        //            if (dt2.Tables[0].Rows.Count >= total)//当天时间内超过特定数
        //            {
        //                Utils.Error("抱歉！当天时间内过于频繁获取短信，请明天再试！", "");
        //            }
        //            DateTime check = DateTime.Now.AddMinutes(-30);
        //            if (check.DayOfYear < DateTime.Now.DayOfYear)
        //            {
        //                check = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //            }
        //            else
        //            {
        //                check = DateTime.Now.AddMinutes(-30);
        //            }
        //            //Utils.Error("check:"+ check, "");
        //            string str = "Phone=" + mobile + " and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
        //            DataSet dt1 = new BCW.BLL.tb_Validate().GetList("*", str);
        //            if (data.Tables[0].Rows[0]["Flag"].ToString() == "0")//最新一条显示当天不能发送了
        //            {
        //                Utils.Error("抱歉！由于之前你存在频繁获取短信，请明天再试！", "");
        //            }
        //            string IP = Utils.GetUsIP();
        //            //查看限制IP
        //            string str1 = "IP= '" + IP + "' and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
        //            DataSet dt3 = new BCW.BLL.tb_Validate().GetList("*", str1);
        //            if (dt3.Tables[0].Rows.Count >= ipCount)//半小时内超过10条
        //            {
        //                Utils.Error("当前IP过于频繁获取短信，请明天再试！" + dt3.Tables[0].Rows.Count, "");
        //            }
        //            if (dt1.Tables[0].Rows.Count >= phoneCount)//半小时内超过10条
        //            {
        //                //跟新标示
        //                int ID = int.Parse(dt1.Tables[0].Rows[0]["ID"].ToString());
        //                new BCW.BLL.tb_Validate().UpdateFlag(0, ID);
        //                Utils.Error("请勿频繁获取短信，请明天再试！", "");
        //            }
        //            else
        //            {
        //                BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //                validate.Phone = mobile;
        //                validate.IP = Utils.GetUsIP();
        //                validate.Time = DateTime.Now.AddMinutes(0);
        //                validate.Flag = 1;
        //                validate.mesCode = mesCode;
        //                validate.codeTime = DateTime.Now.AddMinutes(tm);
        //                validate.type = 6;
        //                string result = "";
        //                Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //                result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //                string[] results = result.Split('#');
        //                if (results[8] != "0")
        //                {
        //                    Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //                }
        //                if ((int.Parse(results[2]) / 80) < msgremain)
        //                {
        //                    new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //                }
        //                if (results[8] == "0")
        //                {
        //                    new BCW.BLL.tb_Validate().Add(validate);
        //                    Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=paypwd"), "2");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //        validate.Phone = mobile;
        //        validate.IP = Utils.GetUsIP();
        //        validate.Time = DateTime.Now.AddMinutes(0);
        //        validate.Flag = 1;
        //        validate.mesCode = mesCode;
        //        validate.codeTime = DateTime.Now.AddMinutes(tm);
        //        validate.type = 6;
        //        string result = "";
        //        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //        result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //        string[] results = result.Split('#');
        //        if (results[8] != "0")
        //        {
        //            Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //        }
        //        if ((int.Parse(results[2]) / 80) < msgremain)
        //        {
        //            new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //        }
        //        if (results[8] == "0")
        //        {
        //            new BCW.BLL.tb_Validate().Add(validate);
        //            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=paypwd"), "2");
        //        }
        //    }
        //    #endregion
        //}
        //else
        #endregion
        {
            #region 设置支付密码的
            string oPwd = Utils.GetRequest("oPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "原密码限6-20位,必须由字母或数字组成");
            string nPwd = Utils.GetRequest("nPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "新密码限6-20位,必须由字母或数字组成");
            string rPwd = Utils.GetRequest("rPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");

            if (!nPwd.Equals(rPwd))
            {
                Utils.Error("新密码与确认密码不相符", "");
            }
            string ordPwd = new BCW.BLL.User().GetUsPled(meid);
            if (!string.IsNullOrEmpty(ordPwd))//修改支付密码
            {
               // string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
                if (!Utils.MD5Str(oPwd).Equals(ordPwd))
                {
                    Utils.Error("原支付密码不正确", "");
                }
                //BCW.Model.tb_Validate getmo = null;
                //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 6))//存在修改手机验证码
                //{
                //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 6);
                //    if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
                //    {
                //        if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
                //        {
                //            Utils.Error("很抱歉,您输入手机验证码不对222", "");
                //        }
                //    }
                //    else { Utils.Error("手机验证码过期，请重新获取", ""); }
                //}
                //else//没发送过修改手机验证码
                //{
                //    Utils.Error("很抱歉,您输入手机验证码不对123", "");
                //}
            }
            new BCW.BLL.User().UpdateUsPled(meid, Utils.MD5Str(nPwd));

            //-------------同时修改支付安全设置为10分钟
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                string ForumSet = new BCW.BLL.User().GetForumSet(meid);
                DateTime dt = DateTime.Now;
                int Times = 10;
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
                new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
            }
            //-------------同时修改支付安全设置为10分钟
            Utils.Success("修改支付密码", "恭喜，修改支付密码成功，正在返回..", Utils.getPage("myedit.aspx"), "2");
            #endregion
        }
    }
    /// <summary>
    /// 使用短信重置支付密码，管理密码
    /// </summary>
    private void getByPhone(int meid)
    {
        string type =  Utils.GetRequest("type", "all", 1, @"^[6-7]", "6");//6 支付，7，管理
        int sendtype = 6;
        string name = "";
        if (type == "6")
        {
            name = "支付密码设置";
        }
        else
        {
            name = "管理密码设置";
            sendtype = 7;
        }
        Master.Title = name;
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-"+ name);
        //  builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
        #region show time 
        BCW.Model.tb_Validate getmo = null;
        if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), sendtype))//修改密码验证码
        {
            getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), sendtype);
            string Reg = string.Empty;
            DateTime endtime = getmo.codeTime;
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                Reg = new BCW.JS.somejs().daojishi2("Reg", endtime);
            }
            else
            {
                Reg = new BCW.JS.somejs().daojishi("Reg", endtime);
            }
            if (endtime > DateTime.Now)
            {
                builder.Append(Out.Tab("<div class=\"\">", ""));
                builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码<br/>");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        #endregion
        strText = "请输入手机验证码:/,,,";
        strName = "phoneCode,sendtype,act,backurl";
        strType = "text,hidden,hidden,hidden";
        strValu = "'"+ sendtype + "'getpwdbyphone'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false";
        strIdea = "<a href=\"" + Utils.getUrl("myedit.aspx?act=getpwdbyphone&amp;ac=ok&amp;sendtype="+ sendtype) + "\">获取手机验证码</a>'''|/";
        strOthe = "确定设置,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级-</a>"+ name);
       // builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 忘记支付管理密码
    /// </summary>
    private void getpwdbyphone(int meid)
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int sendtype =int.Parse( Utils.GetRequest("sendtype", "all", 1, @"^[6-7]$", "6"));
        //Utils.Error("sendtype:" + sendtype+",ac:"+ac, "");
        if (ac == "ok")
        {
            #region get msg
            if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), sendtype))//存在修改支付密码验证码
            {
                BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), sendtype);
                if (getmo.codeTime > DateTime.Now)//验证码存在切是新发的
                {
                    string dateDiff = null;
                    TimeSpan x = getmo.codeTime - DateTime.Now;
                    dateDiff = x.TotalSeconds.ToString();
                    Utils.Error("很抱歉,请在" + dateDiff.Split('.')[0] + "秒之后再次获取手机验证码", "");
                }
            }
            string mobile = new BCW.BLL.User().GetMobile(meid);
            char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
            string mesCode = string.Empty; //手机验证码
            Random rnd = new Random();
            //生成验证码字符串
            for (int i = 0; i < 4; i++)
            {
                mesCode += character[rnd.Next(character.Length)];
            }
            //int tm = 2;//短信过期时间
            //int total = 15;//每天可以发的总短信量
            //int ipCount = 10;
            //int phoneCount = 10;
            int tm = int.Parse(ub.GetSub("msgTime", "/Controls/guestlist.xml"));
            int total = int.Parse(ub.GetSub("dayCount", "/Controls/guestlist.xml"));
            int ipCount = int.Parse(ub.GetSub("IPCount", "/Controls/guestlist.xml"));
            int phoneCount = int.Parse(ub.GetSub("phoneCount", "/Controls/guestlist.xml"));
            int msgremain = int.Parse(ub.GetSub("msgremain", "/Controls/guestlist.xml"));
            int callID = int.Parse(ub.GetSub("callID", "/Controls/guestlist.xml"));
            if (new BCW.BLL.tb_Validate().ExistsPhone(mobile))//不是第一次获取短信
            {
                DataSet data = new BCW.BLL.tb_Validate().GetList(" Top 1 *", "Phone=" + mobile + " order by time desc");
                DateTime changeTime = Convert.ToDateTime(data.Tables[0].Rows[0]["Time"].ToString());
                int changeday = changeTime.DayOfYear;
                if ((DateTime.Now.DayOfYear - changeday) >= 1)//上一条短信不是在当天
                {
                    BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
                    validate.Phone = mobile;
                    validate.IP = Utils.GetUsIP();
                    validate.Time = DateTime.Now.AddMinutes(0);
                    validate.Flag = 1;
                    validate.mesCode = mesCode;
                    validate.codeTime = DateTime.Now.AddMinutes(tm);
                    validate.type = sendtype;
                    string result = "";
                    Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                    result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
                    string[] results = result.Split('#');
                    if (results[8] != "0")
                    {
                        Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
                    }
                    if ((int.Parse(results[2]) / 80) < msgremain)
                    {
                        new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
                    }
                    if (results[8] == "0")
                    {
                        new BCW.BLL.tb_Validate().Add(validate);
                        Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=getByPhone&amp;type="+ sendtype), "2");
                    }
                }
                else//当天时间内
                {
                    DataSet dt2 = new BCW.BLL.tb_Validate().GetList("*", "Phone=" + mobile + " and time>='" + DateTime.Now.ToShortDateString() + "' order by time desc");
                    if (dt2.Tables[0].Rows.Count >= total)//当天时间内超过特定数
                    {
                        Utils.Error("抱歉！当天时间内过于频繁获取短信，请明天再试！", "");
                    }
                    DateTime check = DateTime.Now.AddMinutes(-30);
                    if (check.DayOfYear < DateTime.Now.DayOfYear)
                    {
                        check = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    }
                    else
                    {
                        check = DateTime.Now.AddMinutes(-30);
                    }
                    //Utils.Error("check:"+ check, "");
                    string str = "Phone=" + mobile + " and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
                    DataSet dt1 = new BCW.BLL.tb_Validate().GetList("*", str);
                    if (data.Tables[0].Rows[0]["Flag"].ToString() == "0")//最新一条显示当天不能发送了
                    {
                        Utils.Error("抱歉！由于之前你存在频繁获取短信，请明天再试！", "");
                    }
                    string IP = Utils.GetUsIP();
                    //查看限制IP
                    string str1 = "IP= '" + IP + "' and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
                    DataSet dt3 = new BCW.BLL.tb_Validate().GetList("*", str1);
                    if (dt3.Tables[0].Rows.Count >= ipCount)//半小时内超过10条
                    {
                        Utils.Error("当前IP过于频繁获取短信，请明天再试！" + dt3.Tables[0].Rows.Count, "");
                    }
                    if (dt1.Tables[0].Rows.Count >= phoneCount)//半小时内超过10条
                    {
                        //跟新标示
                        int ID = int.Parse(dt1.Tables[0].Rows[0]["ID"].ToString());
                        new BCW.BLL.tb_Validate().UpdateFlag(0, ID);
                        Utils.Error("请勿频繁获取短信，请明天再试！", "");
                    }
                    else
                    {
                        BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
                        validate.Phone = mobile;
                        validate.IP = Utils.GetUsIP();
                        validate.Time = DateTime.Now.AddMinutes(0);
                        validate.Flag = 1;
                        validate.mesCode = mesCode;
                        validate.codeTime = DateTime.Now.AddMinutes(tm);
                        validate.type = sendtype;
                        string result = "";
                        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                        result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
                        string[] results = result.Split('#');
                        if (results[8] != "0")
                        {
                            Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
                        }
                        if ((int.Parse(results[2]) / 80) < msgremain)
                        {
                            new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
                        }
                        if (results[8] == "0")
                        {
                            new BCW.BLL.tb_Validate().Add(validate);
                            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=getByPhone&amp;type=" + sendtype), "2");
                        }
                    }
                }
            }
            else
            {
                BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
                validate.Phone = mobile;
                validate.IP = Utils.GetUsIP();
                validate.Time = DateTime.Now.AddMinutes(0);
                validate.Flag = 1;
                validate.mesCode = mesCode;
                validate.codeTime = DateTime.Now.AddMinutes(tm);
                validate.type = sendtype;
                string result = "";
                Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
                string[] results = result.Split('#');
                if (results[8] != "0")
                {
                    Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
                }
                if ((int.Parse(results[2]) / 80) < msgremain)
                {
                    new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
                }
                if (results[8] == "0")
                {
                    new BCW.BLL.tb_Validate().Add(validate);
                    Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=getByPhone&amp;type=" + sendtype), "2");
                }
            }
            #endregion
        }
        else
        {
            #region reset pwd
            string name = "";
            if (sendtype == 6)
            {
                name = "重置支付密码";
            }
            else
            {
                name = "重置管理密码";
                sendtype = 7;
            }
            Master.Title = name;
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("myedit.aspx?act=getByPhone&amp;type=" + sendtype) + "\">上级-</a>" + name);
            // builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
            builder.Append(Out.Tab("</div>", ""));
            string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
            BCW.Model.tb_Validate getmo = null;
            if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), sendtype))//存在修改手机验证码
            {
                getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), sendtype);
                if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
                {
                    if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
                    {
                        Utils.Error("很抱歉,您输入手机验证码不对222", "");
                    }
                }
                else { Utils.Error("手机验证码过期，请重新获取", ""); }
            }
            else//没发送过修改手机验证码
            {
                Utils.Error("很抱歉,您输入手机验证码不对123", "");
            }
            strText = "新密码:/,确认密码:/,,,";
            strName = "nPwd,rPwd,sendtype,act,backurl";
            strType = "password,password,hidden,hidden,hidden";
            strValu = "''"+ sendtype + "'savapwd'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,myedit.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //  Utils.Success("", "", Utils.getUrl("myedit.aspx?act=reSetPwd&amp;flag=1&amp;sendtype="+ sendtype), "0");
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("myedit.aspx?act=getByPhone&amp;type=" + sendtype) + "\">上级-</a>" + name);
            // builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
    }
    /// <summary>
    /// 保存重置支付管理密码
    /// </summary>
    private void savapwd(int meid)
    {
        string nPwd = Utils.GetRequest("nPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "新密码限6-20位,必须由字母或数字组成");
        string rPwd = Utils.GetRequest("rPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
        string sendtype = Utils.GetRequest("sendtype", "post", 1, @"^[6-7]$", "6");
        if (!nPwd.Equals(rPwd))
        {
            Utils.Error("新密码与确认密码不相符", "");
        }
        if (sendtype == "6")
        {
            new BCW.BLL.User().UpdateUsPled(meid, Utils.MD5Str(nPwd));
        }
        else
        {
            new BCW.BLL.User().UpdateUsAdmin(meid, Utils.MD5Str(nPwd));
        }
        Utils.Success("修改密码", "恭喜，修改密码成功，正在返回..", Utils.getPage("myedit.aspx"), "2");
    }
    private void reSetPwd()
    {
        int sendtype = int.Parse(Utils.GetRequest("sendtype", "all", 1, @"^[6-7]$", "6"));
        int flag = int.Parse(Utils.GetRequest("flag", "all", 1, @"^[0-1]$", "0"));
        string name = "";
        if (sendtype == 6)
        {
            name = "重置支付密码";
        }
        else
        {
            name = "重置管理密码";
            sendtype = 7;
        }
        Master.Title = name;
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx?act=getByPhone&amp;type="+ sendtype) + "\">上级-</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
        strText = "新密码:/,确认密码:/,,";
        strName = "nPwd,rPwd,act,backurl";
        strType = "password,password,hidden,hidden";
        strValu = "''editadminpwd'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "确定设置,myedit.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx?act=getByPhone&amp;type="+ sendtype) + "\">上级-</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void AdminPwdPage(int meid)
    {
        if (!new BCW.BLL.Role().IsAllMode(meid) && !new BCW.BLL.Group().ExistsUsID(meid))
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "修改管理密码";
        string AdminPwd = new BCW.BLL.User().GetUsAdmin(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;修改管理密码");
        builder.Append(Out.Tab("</div>", ""));
        if (string.IsNullOrEmpty(AdminPwd))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("您是首次设置管理密码:");
            builder.Append(Out.Tab("</div>", ""));
            strText = "管理密码:/,确认密码:/,,";
            strName = "nPwd,rPwd,act,backurl";
            strType = "password,password,hidden,hidden";
            strValu = "''editadminpwd'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
        }
        else
        {
            //BCW.Model.tb_Validate getmo = null;
            //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 7))//修改密码验证码
            //{
            //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 7);
            //    string Reg = string.Empty;
            //    DateTime endtime = getmo.codeTime;
            //    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //    {
            //        Reg = new BCW.JS.somejs().daojishi2("Reg", endtime);
            //    }
            //    else
            //    {
            //        Reg = new BCW.JS.somejs().daojishi("Reg", endtime);
            //    }
            //    if (endtime > DateTime.Now)
            //    {
            //        builder.Append(Out.Tab("<div class=\"\">", ""));
            //        builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码<br/>");
            //        builder.Append(Out.Tab("</div>", "<br/>"));
            //    }
            //}
            //strText = "原密码:/,新密码:/,确认密码:/,请输入手机验证码:/,,";
            //strName = "oPwd,nPwd,rPwd,phoneCode,act,backurl";
            //strType = "password,password,password,text,hidden,hidden";
            //strValu = "''''editadminpwd'" + Utils.getPage(0) + "";
            //strEmpt = "false,false,false,false,false,false";
            strText = "原密码:/,新密码:/,确认密码:/,,";
            strName = "oPwd,nPwd,rPwd,act,backurl";
            strType = "password,password,password,hidden,hidden";
            strValu = "'''editadminpwd'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";
        }
        //strIdea = "'''<a href=\"" + Utils.getUrl("myedit.aspx?act=editadminpwd&amp;ac=ok") + "\">获取手机验证码</a>''|/";
        strIdea = "''''|/";
        strOthe = "确定设置,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=getByPhone&amp;type=7") + "\">忘记管理密码(?)</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditAdminPwdPage(int meid)
    {
        if (!new BCW.BLL.Role().IsAllMode(meid) && !new BCW.BLL.Group().ExistsUsID(meid))
        {
            Utils.Error("你的权限不足", "");
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        #region
        //if (ac == "ok")
        //{
        //    #region 获取管理短信
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 7))//存在修改支付密码验证码
        //    {
        //        BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 7);
        //        if (getmo.codeTime > DateTime.Now)//验证码存在切是新发的
        //        {
        //            string dateDiff = null;
        //            TimeSpan x = getmo.codeTime - DateTime.Now;
        //            dateDiff = x.TotalSeconds.ToString();
        //            Utils.Error("很抱歉,请在" + dateDiff.Split('.')[0] + "秒之后再次获取手机验证码", "");
        //        }
        //    }
        //    string mobile = new BCW.BLL.User().GetMobile(meid);
        //    char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
        //    string mesCode = string.Empty; //手机验证码
        //    Random rnd = new Random();
        //    //生成验证码字符串
        //    for (int i = 0; i < 4; i++)
        //    {
        //        mesCode += character[rnd.Next(character.Length)];
        //    }
        //    //int tm = 2;//短信过期时间
        //    //int total = 15;//每天可以发的总短信量
        //    //int ipCount = 10;
        //    //int phoneCount = 10;
        //    int tm = int.Parse(ub.GetSub("msgTime", "/Controls/guestlist.xml"));
        //    int total = int.Parse(ub.GetSub("dayCount", "/Controls/guestlist.xml"));
        //    int ipCount = int.Parse(ub.GetSub("IPCount", "/Controls/guestlist.xml"));
        //    int phoneCount = int.Parse(ub.GetSub("phoneCount", "/Controls/guestlist.xml"));
        //    int msgremain = int.Parse(ub.GetSub("msgremain", "/Controls/guestlist.xml"));
        //    int callID = int.Parse(ub.GetSub("callID", "/Controls/guestlist.xml"));
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(mobile))//不是第一次获取短信
        //    {
        //        DataSet data = new BCW.BLL.tb_Validate().GetList(" Top 1 *", "Phone=" + mobile + " order by time desc");
        //        DateTime changeTime = Convert.ToDateTime(data.Tables[0].Rows[0]["Time"].ToString());
        //        int changeday = changeTime.DayOfYear;
        //        if ((DateTime.Now.DayOfYear - changeday) >= 1)//上一条短信不是在当天
        //        {
        //            BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //            validate.Phone = mobile;
        //            validate.IP = Utils.GetUsIP();
        //            validate.Time = DateTime.Now.AddMinutes(0);
        //            validate.Flag = 1;
        //            validate.mesCode = mesCode;
        //            validate.codeTime = DateTime.Now.AddMinutes(tm);
        //            validate.type = 7;
        //            string result = "";
        //            Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //            result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //            string[] results = result.Split('#');
        //            if (results[8] != "0")
        //            {
        //                Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //            }
        //            if ((int.Parse(results[2]) / 80) < msgremain)
        //            {
        //                new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //            }
        //            if (results[8] == "0")
        //            {
        //                new BCW.BLL.tb_Validate().Add(validate);
        //                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=adminpwd"), "2");
        //            }
        //        }
        //        else//当天时间内
        //        {
        //            DataSet dt2 = new BCW.BLL.tb_Validate().GetList("*", "Phone=" + mobile + " and time>='" + DateTime.Now.ToShortDateString() + "' order by time desc");
        //            if (dt2.Tables[0].Rows.Count >= total)//当天时间内超过特定数
        //            {
        //                Utils.Error("抱歉！当天时间内过于频繁获取短信，请明天再试！", "");
        //            }
        //            DateTime check = DateTime.Now.AddMinutes(-30);
        //            if (check.DayOfYear < DateTime.Now.DayOfYear)
        //            {
        //                check = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //            }
        //            else
        //            {
        //                check = DateTime.Now.AddMinutes(-30);
        //            }
        //            //Utils.Error("check:"+ check, "");
        //            string str = "Phone=" + mobile + " and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
        //            DataSet dt1 = new BCW.BLL.tb_Validate().GetList("*", str);
        //            if (data.Tables[0].Rows[0]["Flag"].ToString() == "0")//最新一条显示当天不能发送了
        //            {
        //                Utils.Error("抱歉！由于之前你存在频繁获取短信，请明天再试！", "");
        //            }
        //            string IP = Utils.GetUsIP();
        //            //查看限制IP
        //            string str1 = "IP= '" + IP + "' and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
        //            DataSet dt3 = new BCW.BLL.tb_Validate().GetList("*", str1);
        //            if (dt3.Tables[0].Rows.Count >= ipCount)//半小时内超过10条
        //            {
        //                Utils.Error("当前IP过于频繁获取短信，请明天再试！" + dt3.Tables[0].Rows.Count, "");
        //            }
        //            if (dt1.Tables[0].Rows.Count >= phoneCount)//半小时内超过10条
        //            {
        //                //跟新标示
        //                int ID = int.Parse(dt1.Tables[0].Rows[0]["ID"].ToString());
        //                new BCW.BLL.tb_Validate().UpdateFlag(0, ID);
        //                Utils.Error("请勿频繁获取短信，请明天再试！", "");
        //            }
        //            else
        //            {
        //                BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //                validate.Phone = mobile;
        //                validate.IP = Utils.GetUsIP();
        //                validate.Time = DateTime.Now.AddMinutes(0);
        //                validate.Flag = 1;
        //                validate.mesCode = mesCode;
        //                validate.codeTime = DateTime.Now.AddMinutes(tm);
        //                validate.type = 7;
        //                string result = "";
        //                Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //                result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //                string[] results = result.Split('#');
        //                if (results[8] != "0")
        //                {
        //                    Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //                }
        //                if ((int.Parse(results[2]) / 80) < msgremain)
        //                {
        //                    new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //                }
        //                if (results[8] == "0")
        //                {
        //                    new BCW.BLL.tb_Validate().Add(validate);
        //                    Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=adminpwd"), "2");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
        //        validate.Phone = mobile;
        //        validate.IP = Utils.GetUsIP();
        //        validate.Time = DateTime.Now.AddMinutes(0);
        //        validate.Flag = 1;
        //        validate.mesCode = mesCode;
        //        validate.codeTime = DateTime.Now.AddMinutes(tm);
        //        validate.type = 7;
        //        string result = "";
        //        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //        result = MesExt.Submit("000379", "123456", "1069032239089369", "【"+ ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
        //        string[] results = result.Split('#');
        //        if (results[8] != "0")
        //        {
        //            Utils.Error("请确认手机号的正确性，如不能为空号!" + results[8], "");
        //        }
        //        if ((int.Parse(results[2]) / 80) < msgremain)
        //        {
        //            new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
        //        }
        //        if (results[8] == "0")
        //        {
        //            new BCW.BLL.tb_Validate().Add(validate);
        //            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("myedit.aspx?act=adminpwd"), "2");
        //        }
        //    }
        //    #endregion
        //}
        //else
        #endregion
        {
            #region 确认提交
            string oPwd = Utils.GetRequest("oPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "原密码限6-20位,必须由字母或数字组成");
            string nPwd = Utils.GetRequest("nPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "新密码限6-20位,必须由字母或数字组成");
            string rPwd = Utils.GetRequest("rPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
            if (!nPwd.Equals(rPwd))
            {
                Utils.Error("新密码与确认密码不相符", "");
            }

            string ordPwd = new BCW.BLL.User().GetUsAdmin(meid);
            if (!string.IsNullOrEmpty(ordPwd))
            {
               // string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
                if (!Utils.MD5Str(oPwd).Equals(ordPwd))
                {
                    Utils.Error("原管理密码不正确", "");
                }
                //BCW.Model.tb_Validate getmo = null;
                //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 7))//存在修改手机验证码
                //{
                //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 7);
                //    if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
                //    {
                //        if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
                //        {
                //            Utils.Error("很抱歉,您输入手机验证码不对222", "");
                //        }
                //    }
                //    else { Utils.Error("手机验证码过期，请重新获取", ""); }
                //}
                //else//没发送过修改手机验证码
                //{
                //    Utils.Error("很抱歉,您输入手机验证码不对123", "");
                //}

            }
            new BCW.BLL.User().UpdateUsAdmin(meid, Utils.MD5Str(nPwd));

            Utils.Success("修改管理密码", "恭喜，修改管理密码成功，正在返回..", Utils.getPage("myedit.aspx"), "2");
            #endregion
        }
    }


    private void RegYzPage(int meid)
    {
        string postId = Utils.GetRequest("postId", "get", 1, @"^[\w\d]{1,}$", "错误的请求");
        string UsKey = DESEncrypt.Decrypt(postId, "ct201200108");
        BCW.Model.User model = new BCW.BLL.User().GetKey(meid);
        if (model == null)
        {
            Utils.Error("错误的请求", "");
        }
        if (model.UsKey != UsKey)
        {
            Utils.Error("错误的请求", "");
        }
        if (model.IsVerify != 2)
        {
            Utils.Success("重复验证", "您的ID已经通过验证，正在转到您的空间", Utils.getUrl("uinfo.aspx"), "5");
        }
        string mobile = new BCW.BLL.User().GetMobile(meid);
        new BCW.BLL.User().UpdateIsVerify(mobile, 0);
        Utils.Success("完成注册", "恭喜，完成注册成功！正在转到您的空间", Utils.getUrl("uinfo.aspx"), "5");
    }

    private void CheckPwdPage(int meid)
    {
        string postId = Utils.GetRequest("postId", "get", 1, @"^[\w\d]{1,}$", "错误的请求");
        string UsKey = DESEncrypt.Decrypt(postId, "ct201200108");
        BCW.Model.User model = new BCW.BLL.User().GetKey(meid);
        if (model == null)
        {
            Utils.Error("错误的请求", "");
        }
        if (model.UsKey != UsKey)
        {
            Utils.Error("错误的请求", "");
        }

        string mobile = new BCW.BLL.User().GetMobile(meid);
        new BCW.BLL.User().UpdateUsPwd(meid, Utils.MD5Str(Utils.Right(mobile, 6)));
        new BCW.BLL.User().UpdateUsPled(meid, Utils.MD5Str(Utils.Right(mobile, 6)));
        Utils.Success("重置密码", "恭喜，重置密码成功！<br />您的ID：" + meid + "<br />登录密码：" + Utils.Right(mobile, 6) + "<br />支付密码：" + Utils.Right(mobile, 6) + "<br /><a href=\"" + Utils.getUrl("/reg.aspx") + "\">马上登录&gt;&gt;</a>", Utils.getUrl("myedit.aspx"), "20");
    }

    private void BindPage(int meid)
    {
        Master.Title = "更换绑定手机";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;更换绑定");
        builder.Append(Out.Tab("</div>", ""));

        if (info == "ok")
        {
            int oneId = int.Parse(Utils.GetRequest("oneId", "post", 2, @"^[1-9]\d*$", "ID填写错误"));
            int twoId = int.Parse(Utils.GetRequest("twoId", "post", 2, @"^[1-9]\d*$", "ID填写错误"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("我要将ID" + oneId + "与ID" + twoId + "的手机号互换");
            builder.Append(Out.Tab("</div>", ""));

            strText = "ID" + oneId + "登录密码:/,ID" + oneId + "的支付密码:/,ID" + oneId + "的手机号码:/,-----------/ID" + twoId + "登录密码:/,ID" + twoId + "的支付密码:/,ID" + twoId + "的手机号码:/,,,,";
            strName = "oneIdPwd,oneIdPled,oneIdMobile,twoIdPwd,twoIdPled,twoIdMobile,oneId,twoId,act,backurl";
            strType = "text,text,text,text,text,text,hidden,hidden,hidden,hidden";
            strValu = "''''''" + oneId + "'" + twoId + "'bindok'" + Utils.getPage(0) + "";
            strEmpt = "false,true,false,false,true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定更换,myedit.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />如某ID支付密码未设置则先要设置支付密码");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            strText = "我要将ID:/,与另一ID:/,,,";
            strName = "oneId,twoId,act,info,backurl";
            strType = "num,num,hidden,hidden,hidden";
            strValu = "''bind'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "/的手机号码互换/";
            strOthe = "确定更换,myedit.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />每个手机号对应一个ID,可通过ID密码和支付密码互换手机号");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BindOkPage(int meid)
    {
        int oneId = int.Parse(Utils.GetRequest("oneId", "post", 2, @"^[1-9]\d*$", "ID填写错误"));
        int twoId = int.Parse(Utils.GetRequest("twoId", "post", 2, @"^[1-9]\d*$", "ID填写错误"));
        string oneIdPwd = Utils.GetRequest("oneIdPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "登录密码限6-20位,必须由字母或数字组成");
        string oneIdPled = Utils.GetRequest("oneIdPled", "post", 2, @"^[A-Za-z0-9]{6,20}$", "支付密码限6-20位,必须由字母或数字组成,未设置可留空");
        string oneIdMobile = Utils.GetRequest("oneIdMobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string twoIdPwd = Utils.GetRequest("twoIdPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "登录密码限6-20位,必须由字母或数字组成");
        string twoIdPled = Utils.GetRequest("twoIdPled", "post", 2, @"^[A-Za-z0-9]{6,20}$", "支付密码限6-20位,必须由字母或数字组成,未设置可留空");
        string twoIdMobile = Utils.GetRequest("twoIdMobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");

        if (!new BCW.BLL.User().Exists(oneId))
        {
            Utils.Error("会员ID" + oneId + "不存在", "");
        }
        if (!new BCW.BLL.User().Exists(twoId))
        {
            Utils.Error("会员ID" + twoId + "不存在", "");
        }
        if (new BCW.BLL.User().GetIsVerify(oneId) == 0)
        {
            Utils.Error("会员ID" + oneId + "尚未通过短信验证，不能进行改绑", "");
        }
        if (new BCW.BLL.User().GetIsVerify(twoId) == 0)
        {
            Utils.Error("会员ID" + twoId + "尚未通过短信验证，不能进行改绑", "");
        }

        string OldoneIdPled = new BCW.BLL.User().GetUsPled(oneId, oneIdMobile);
        if (string.IsNullOrEmpty(OldoneIdPled))
        {
            Utils.Error("更换失败，请检查登录和支付密码是否填写正确，如支付密码未设置则要先设置", "");
        }
        string OldtwoIdPled = new BCW.BLL.User().GetUsPled(twoId, twoIdMobile);
        if (string.IsNullOrEmpty(OldtwoIdPled))
        {
            Utils.Error("更换失败，请检查登录和支付密码是否填写正确，如支付密码未设置则要先设置", "");
        }
        string OldoneIdPwd = new BCW.BLL.User().GetUsPwd(oneId, oneIdMobile);
        string OldtwoIdPwd = new BCW.BLL.User().GetUsPwd(twoId, twoIdMobile);

        if (Utils.MD5Str(oneIdPwd).Equals(OldoneIdPwd) && Utils.MD5Str(oneIdPled).Equals(OldoneIdPled) && Utils.MD5Str(twoIdPwd).Equals(OldtwoIdPwd) && Utils.MD5Str(twoIdPled).Equals(OldtwoIdPled))
        {
            new BCW.BLL.User().UpdateMobile(oneId, twoIdMobile);
            new BCW.BLL.User().UpdateMobile(twoId, oneIdMobile);
            Utils.Success("更换绑定手机", "恭喜，更换绑定手机成功，正在返回..", Utils.getPage("myedit.aspx"), "2");
        }
        else
        {
            Utils.Error("更换失败，请检查登录和支付密码是否填写正确", "");
        }
    }

    private void ExitPage(int uid)
    {
        //清Cookie
        HttpCookie Cookie = new HttpCookie("LoginComment");
        Cookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(Cookie);
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        //取随机识别串
        string UsKey = new Rand().RandNum(10);
        new BCW.BLL.User().UpdateUsKey(uid, UsKey);

        Utils.Success("退出登录", "正在退出登录..", "/bbs/default.aspx?" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + new Rand().RandNume(21) + "", "2");
    }

    private void PhotoPage(int meid)
    {
        Master.Title = "更换头像";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;更换头像");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("addfile.aspx?act=photo&amp;backurl=" + Utils.PostPage(1) + "") + "\">手机直接上传</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("addfilegc2.aspx?act=photo&amp;backurl=" + Utils.PostPage(1) + "") + "\">手机直接上传(低端机)</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?uid=" + meid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">从相册内挑选</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=photos&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">精美头像一</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=photos&amp;ptype=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">精美头像二</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PhotosPage(int meid)
    {
        Master.Title = "更换头像";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        int vs = int.Parse(Utils.GetRequest("vs", "get", 1, @"^[0-9]\d*$", "0"));
        if (vs > 0 && vs < 61)
        {
            //删除之前的自定义头像文件
            string PhotoFile = new BCW.BLL.User().GetPhoto(meid);
            if (PhotoFile != "" && PhotoFile.Contains("/tx/"))
            {
                BCW.Files.FileTool.DeleteFile(PhotoFile);
            }
            new BCW.BLL.User().UpdatePhoto(meid, "/Files/Avatar/image" + vs + ".gif");
            //动态记录
            new BCW.BLL.Action().Add(meid, "在空间设置了[URL=/bbs/uinfo.aspx?uid=" + meid + "]新的头像[/URL]");
            Utils.Success("修改头像", "恭喜，修改头像成功，正在返回..", Utils.getPage("uinfo.aspx?uid=" + meid + ""), "2");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">个人设置</a>&gt;更换头像");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        if (ptype == 1)
            builder.Append("头像列表一|<a href=\"" + Utils.getUrl("myedit.aspx?act=photos&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">头像列表二</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=photos&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">头像列表一</a>|头像列表二");

        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = 6;
        string[] pageValUrl = { "act", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //总记录数
        if (ptype == 1)
            recordCount = 40;
        else
            recordCount = 20;

        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        for (int i = 0; i < recordCount; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                if ((k + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                int j = i + 1;
                if (ptype == 2)
                    j = 41 + i;

                builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=photos&amp;info=ok&amp;vs=" + j + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/Avatar/image" + j + ".gif\" alt=\"load\"/></a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ForumPage(int meid)
    {
        Master.Title = "论坛个性化设置";
        string info = Utils.GetRequest("info", "post", 1, "", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">系统设置</a>&gt;论坛个性化");
        builder.Append(Out.Tab("</div>", ""));
        string sforumsets = string.Empty;
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        string sText = string.Empty;
        string sName = string.Empty;
        string sValue = string.Empty;
        string[] fs = ForumSet.Split(",".ToCharArray());

        int iLength = 0;
        if (info == "ok")
            iLength = fs.Length - 1;
        else
            iLength = 8;

        for (int i = 0; i <= iLength; i++)
        {
            string[] sfs = fs[i].ToString().Split("|".ToCharArray());

            if (info == "ok" && i <= 8)
            {
                if (string.IsNullOrEmpty(Request.Form["Name" + i + ""]))
                {
                    Utils.Error("请正确设置各个选项", "");
                }
                if (!Utils.IsRegex(Request.Form["Name" + i + ""].ToString(), @"^[0-9]\d*$"))
                {
                    Utils.Error("请正确设置各个选项", "");
                }
                sforumsets += "," + sfs[0] + "|" + Request.Form["Name" + i + ""];
            }
            else
            {
                sforumsets += "," + sfs[0] + "|" + sfs[1];
                sText += "," + sfs[0] + ":";
                sName += ",Name" + i;
                sValue += "'" + sfs[1];
            }
        }
        if (info == "ok")
        {
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
            Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getPage("myedit.aspx?act=forum"), "1");
        }
        sText = Utils.Mid(sText, 1, sText.Length);
        sName = Utils.Mid(sName, 1, sName.Length);
        sValue = Utils.Mid(sValue, 1, sValue.Length);

        strText = "" + sText + ",,,";
        strName = "" + sName + ",info,act,backurl";
        strType = "stext,stext,stext,stext,select,stext,select,select,select,hidden,hidden,hidden";
        strValu = "" + sValue + "'ok'forum'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,0|显示|1|不显示,false,0|显示|1|不显示,0|显示|1|不显示,0|闭合|1|展开,false,false,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FriendPage(int meid)
    {
        Master.Title = "好友验证设置";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string name = Utils.GetRequest("name", "all", 1, "", "");//lin20160428农场
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        if (info == "ok")
        {
            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-3]$", "选择方式错误"));
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 11)
                {
                    sforumsets += "," + sfs[0] + "|" + Types;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
            if (name == "name")
                Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getUrl("game/farm.aspx?act=toucai&amp;backurl=" + Utils.getPage(0) + ""), "1");
            else
                Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getUrl("myedit.aspx?act=friend&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">系统设置</a>&gt;好友验证");
        builder.Append(Out.Tab("</div>", ""));
        int friSet = BCW.User.Users.GetForumSet(ForumSet, 11);

        strText = "选择加好友方式:/,,,,";
        strName = "Types,info,act,name,backurl";
        strType = "select,hidden,hidden,hidden,hidden";
        strValu = "" + friSet + "'ok'friend'" + name + "'" + Utils.getPage(0) + "";
        strEmpt = "0|允许任何人|1|不允许任何人|2|需要验证才允许|3|只有VIP才能,false,false,false,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GuestPage(int meid)
    {
        Master.Title = "内线设置";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        int friSet = 0;
        if (ptype > 0)
        {
            if (ptype < 12 && ptype > 18)
            {
                Utils.Error("选择屏蔽选项错误", "");
            }
            int Types = 0;
            friSet = BCW.User.Users.GetForumSet(ForumSet, ptype);
            if (friSet == 0)
                Types = 1;

            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == ptype)
                {
                    sforumsets += "," + sfs[0] + "|" + Types;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
            Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getUrl("myedit.aspx?act=guest&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }


        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">系统设置</a>&gt;短内线设置");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>好友群发</b>");
        builder.Append("<br />包含:您在对方的好友里面才会收到");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 12);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");

        builder.Append(Out.Tab("</div>", ""));

        string xmlPath2 = "/Controls/group.xml";
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>" + ub.GetSub("GroupName", xmlPath2) + "群发</b>");
        builder.Append("<br />包含:由" + ub.GetSub("GroupzName", xmlPath2) + "群发出来的内线");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 13);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>推荐邀请提醒</b>");
        builder.Append("<br />包含:如会员推荐帖子,邀请PK,回帖提醒等");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 14);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>系统内线</b>");
        builder.Append("<br />包含:系统内线,系统提醒,不含游戏内线");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 15);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>游戏系统内线</b>");
        builder.Append("<br />包含:所有游戏PK结果的内线提醒");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 16);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=16&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=16&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>非好友内线</b>");
        builder.Append("<br />屏蔽后只能收到好友信息,请谨慎使用");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 17);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=17&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=17&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>未验证手机号</b>");
        builder.Append("<br />屏蔽后非验证用户须是自己好友才能给自己发内线");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 18);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=18&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=18&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>闲聊对聊提醒</b>");
        builder.Append("<br />屏蔽后将不能收到闲聊对聊提醒内线");
        friSet = BCW.User.Users.GetForumSet(ForumSet, 33);
        if (friSet == 0)
            builder.Append("<br />当前设置:接受|<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=33&amp;backurl=" + Utils.getPage(0) + "") + "\">屏蔽</a>");
        else
            builder.Append("<br />当前设置:<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;ptype=33&amp;backurl=" + Utils.getPage(0) + "") + "\">接受</a>|屏蔽");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MebookPage(int meid)
    {
        Master.Title = "留言本设置";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        if (info == "ok")
        {
            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            int addBook = int.Parse(Utils.GetRequest("addBook", "post", 2, @"^[0-2]$", "选择留言权限错误"));
            int isGuest = int.Parse(Utils.GetRequest("isGuest", "post", 2, @"^[0-1]$", "选择通知错误"));
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 19)
                {
                    sforumsets += "," + sfs[0] + "|" + addBook;
                }
                else if (i == 20)
                {
                    sforumsets += "," + sfs[0] + "|" + isGuest;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
            Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getUrl("myedit.aspx?act=mebook&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">系统设置</a>&gt;留言本验证");
        builder.Append(Out.Tab("</div>", ""));
        int addBookSet = BCW.User.Users.GetForumSet(ForumSet, 19);
        int isGuestSet = BCW.User.Users.GetForumSet(ForumSet, 20);

        strText = "设置空间留言权限:,当有人留言时:,,,";
        strName = "addBook,isGuest,info,act,backurl";
        strType = "select,select,hidden,hidden,hidden";
        strValu = "" + addBookSet + "'" + isGuestSet + "'ok'mebook'" + Utils.getPage(0) + "";
        strEmpt = "0|所有人|1|限好友|2|禁止留言,0|通知我|1|不通知我,false,false,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupChatPage(int meid)
    {
        string xmlPath2 = "/Controls/group.xml";
        Master.Title = "" + ub.GetSub("GroupName", xmlPath2) + "聊天提醒设置";
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        if (info == "ok")
        {
            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-3]$", "选择方式错误"));
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 21)
                {
                    sforumsets += "," + sfs[0] + "|" + Types;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
            Utils.Success("更新设置", "更新设置成功，正在返回..", Utils.getUrl("myedit.aspx?act=groupchat&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">系统设置</a>&gt;" + ub.GetSub("GroupName", xmlPath2) + "聊天提醒");
        builder.Append(Out.Tab("</div>", ""));
        int chatSet = BCW.User.Users.GetForumSet(ForumSet, 21);

        strText = "" + ub.GetSub("GroupName", xmlPath2) + "聊天提醒设置:/,,,";
        strName = "Types,info,act,backurl";
        strType = "select,hidden,hidden,hidden";
        strValu = "" + chatSet + "'ok'groupchat'" + Utils.getPage(0) + "";
        strEmpt = "0|开启|1|关闭,false,false,false";
        strIdea = "/";
        strOthe = "确定修改,myedit.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />当选择开启状态并您在线上就会收到" + ub.GetSub("GroupName", xmlPath2) + "聊天提醒.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("myedit.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + meid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void LoginSafePage(int uid)
    {

        Master.Title = "设置登录安全";
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int Times = int.Parse(Utils.GetRequest("Times", "post", 2, @"^[0-9]\d*$", "选择超时时间错误"));
            if (Times < 0 || Times > 1440)
                Utils.Error("选择超时时间错误", "");

            DateTime dt = DateTime.Now;
            if (Times > 0)
                dt = dt.AddMinutes(Times);

            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 25)
                {
                    sforumsets += "," + sfs[0] + "|" + dt;
                }
                else if (i == 26)
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
            Utils.Success("设置登录安全", "设置登录安全成功，正在返回..", Utils.getUrl("myedit.aspx?act=loginsafe&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            int iLogin = BCW.User.Users.GetForumSet(ForumSet, 26);

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("设置登录安全");
            builder.Append(Out.Tab("</div>", ""));
            strText = "IP异常超时时间:/,,,";
            strName = "Times,info,act,backurl";
            strType = "select,hidden,hidden,hidden";
            strValu = "" + iLogin + "'ok'loginsafe'" + Utils.getPage(0) + "";
            strEmpt = "0|关闭异常|1|一直打开异常|10|10分钟内允许异常|20|20分钟内允许异常|30|30分钟内允许异常|60|1小时内允许异常|120|2小时内允许异常|1440|24小时内允许异常,false,false,false";
            strIdea = "/";
            strOthe = "&gt;确定设置,myedit.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />选择打开异常，则您的IP发生明显变化时则会提示输入密码登录.<br />选择N分钟内异常时，则N分钟内存在IP异常时不需要密码登录，否则需要输入密码登录.");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">设置</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    /// <summary>
    /// 情景选择
    /// </summary>
    private void ScenePage(int uid)
    {
        Master.Title = "设置我的情景";
        builder.Append(Out.Tab("<div class=\"title\">设置我的情景</div>", ""));
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        int scene = BCW.User.Users.GetForumSet(ForumSet, 32);
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info == "ok")
        {
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "选择情景错误"));
            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 32)
                {
                    sforumsets += "," + sfs[0] + "|" + id;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(uid, sforumsets);
            ForumSet = new BCW.BLL.User().GetForumSet(uid);
            scene = BCW.User.Users.GetForumSet(ForumSet, 32);
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("我正在使用的情景:");
        if (scene == -1)
            builder.Append("暂无");
        else
            builder.Append("<img src=\"/files/scene/" + BCW.User.Scene.GetScene[1][scene] + ".gif\" alt=\"load\"/>" + BCW.User.Scene.GetScene[0][scene] + "");

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));

        if (ptype == 0)
            builder.Append("我的场景|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=scene&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">我的场景</a>|");

        if (ptype == 1)
            builder.Append("心情<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=scene&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">心情</a><br />");


        if (ptype == 0)
            builder.Append("点击修改我在…");
        else
            builder.Append("点击修改我的此时心情?");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string[] SceneTitle = BCW.User.Scene.GetScene[0];
        string[] SceneNum = BCW.User.Scene.GetScene[1];

        int t = 0;
        int k = 0;
        if (ptype == 0)
        {
            t = 0;
            k = 16;
        }
        else
        {
            t = 16;
            k = 33;
        }
        for (int i = t; i < k; i++)
        {
            builder.Append("<img src=\"/files/scene/" + i + ".gif\" alt=\"load\"/><a href=\"" + Utils.getUrl("myedit.aspx?act=scene&amp;info=ok&amp;id=" + i + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + SceneTitle[i] + "</a>");
            if ((i + 1) % 4 == 0 || i == 32)
                builder.Append("<br />");

            if (i >= k)
                break;

        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.RHr()));
        builder.Append("温馨提示:<br />空间表情是以图标的形式还表现您当前的心情或位置,根据您的喜好可随时变更.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?backurl=" + Utils.getPage(0) + "") + "\">设置</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}