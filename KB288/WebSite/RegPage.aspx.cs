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

public partial class RegPage : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/email.xml";
    protected string xmlPath2 = "/Controls/reg.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "手工免费注册";
        Master.IsFoot = false;
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                OkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {

        //自动清空7七前的未验证ID(不包括2012年10月之前未验证的ID)
        BCW.Data.SqlHelper.ExecuteSql("delete from tb_user where IsVerify=0 and RegTime>'2012-10-1 00:00:00' and RegTime<'" + DateTime.Now.AddDays(-7) + "'");


        builder.Append(Out.Tab("<div class=\"title\">免费注册会员</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("*手机号(找回密码用):");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",*设定密码(6-20字符):/,*确认密码:/,,,";
        string strName = "mobile,Pwd,Pwdr,act,backurl";
        string strType = "text,password,password,hidden,hidden";
        string strValu = "'''ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "注册(并自动登陆),RegPage.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("reg.aspx") + "\">推荐短信注册&gt;&gt;</a>");
        //        注册成功后，请编写短信111到15992310086即可成为验证会员
        //注册成功达7天后还未进行验证的ID，系统将进行回收处理
        if (!Utils.GetDomain().Contains("dyj6"))
        {
            builder.Append("<br />温馨提示:<br />注册成功后，请编写短信<a href=\"sms:15992310086?body=111\" >111</a>到<a href=\"wtai://wp/mc;15992310086\" >15992310086</a>即可成为验证会员");
            builder.Append("<br />注册成功达7天后还未进行验证的ID，系统将进行回收处理");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("reg.aspx?act=agreement") + "\">注册协议</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void OkPage()
    {
        Master.IsFoot = false;
        string mobile = Utils.GetRequest("mobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
        string Pwdr = Utils.GetRequest("Pwdr", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
        int rd = 0;

        if (!Pwd.Equals(Pwdr))
        {
            Utils.Error("很抱歉,您输入的两次密码不相符", "");
        }
        if (new BCW.BLL.User().Exists(mobile))
        {
            Utils.Error("很抱歉,此手机号已经注册", "");
        }
        //手工注册防刷
        int Expir = Utils.ParseInt(ub.GetSub("RegExpir", xmlPath2));
        string CacheKey = Utils.GetUsIP();
        object getObjCacheTime = DataCache.GetCache(CacheKey);
        if (getObjCacheTime != null)
        {
            Utils.Error(ub.GetSub("BbsGreet", "/Controls/bbs.xml"), "");
        }
        object ObjCacheTime = DateTime.Now;
        DataCache.SetCache(CacheKey, ObjCacheTime, DateTime.Now.AddSeconds(Expir), TimeSpan.Zero);

        //取得会员ID
        int maxId = BCW.User.Reg.GetRandId();
        if (maxId == 0)
        {
            Utils.Error("服务器繁忙，请稍后注册..", "");
        }
        //加密用户密码
        string strPwd = Utils.MD5Str(Pwd.Trim());
        //取随机识别串
        string UsKey = new Rand().RandNum(10);

        string newName = ub.GetSub("RegName", "/Controls/reg.xml");
        if (newName == "")
            newName = "新会员";

        string Email = string.Empty;
        BCW.Model.User model = new BCW.Model.User();
        if (ub.GetSub("RegEmailPost", xmlPath2) == "1")
        {
            Email = Utils.GetRequest("Email", "post", 2, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "邮箱填写错误");
            if (new BCW.BLL.User().ExistsEmail(Email))
            {
                Utils.Error("此邮箱已经被注册", "");
            }
            model.IsVerify = 2;
            model.Email = Email;
        }
        else
        {
            model.IsVerify = 0;
            model.Email = "";
        }
        //写入注册表
        model.ID = maxId;
        model.Mobile = mobile;
        model.UsName = "" + newName + "" + maxId + "";
        model.UsPwd = strPwd;
        model.UsKey = UsKey;
        model.Photo = "/Files/Avatar/image0.gif";
        model.Sex = 0;
        model.RegTime = DateTime.Now;
        model.RegIP = Utils.GetUsIP();
        model.EndTime = DateTime.Now;
        model.Birth = DateTime.Parse("1980-1-1");
        model.Sign = "未设置签名";
        model.InviteNum = rd;
        new BCW.BLL.User().Add(model);
        //发送内线
        new BCW.BLL.Guest().Add(model.ID, model.UsName, ub.GetSub("RegGuest", "/Controls/reg.xml"));
        //积分操作
        new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, maxId);
        if (rd > 0 && model.IsVerify == 1)
        {
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RecomUser, rd);
        }

        builder.Append(Out.Tab("<div class=\"title\">免费注册会员</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("恭喜您注册成功！<br />");
        builder.Append("注册ID:" + maxId + "<br />注册手机号:" + mobile + "<br />登录密码:" + Pwd.Trim() + "");
        if (!Utils.GetDomain().Contains("dyj6"))
        {
            builder.Append("<br />重要:请编写短信<a href=\"sms:15992310086?body=111\" >111</a>到<a href=\"wtai://wp/mc;15992310086\" >15992310086</a>即可成为验证会员，未进行验证的ID系统将在7天后回收");
        }
        builder.Append(Out.Tab("</div>", ""));
        //设置keys
        string keys = string.Empty;
        keys = BCW.User.Users.SetUserKeys(maxId, strPwd, UsKey);
        //清Cookie
        HttpCookie Cookie = new HttpCookie("LoginComment");
        Cookie.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(Cookie);
        //写入Cookie
        Cookie.Expires = DateTime.Now.AddDays(30);//30天
        Cookie.Values.Add("userkeys", DESEncrypt.Encrypt(Utils.Mid(keys, 0, keys.Length - 4)));
        Response.Cookies.Add(Cookie);

        if (Utils.getPage(1) != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">&gt;&gt;返回之前页</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"/bbs/default.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">上级</a>-");
        builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">完善资料</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

}