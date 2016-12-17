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

/// <summary>
/// 修改注册协议
/// 黄国军 20160713
/// 陈志基修改注册，添加短信验证 20160910
/// 陈志基 注册去COOKIE 20161010
/// </summary>
public partial class reg : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/email.xml";
    protected string xmlPath2 = "/Controls/reg.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "免费注册";
        Master.IsFoot = false;
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                OkPage();
                break;
            //case "pwd":
            //    PwdPage();
            //    break;
            //case "pwdok":
            //    PwdOkPage();
            //    break;
            case "validate":
                validate();
                break;
            case "agreement":
                ReloadPage();
                break;
            case "recommended":
                recommended();
                break;
            case "regpwd":
                RegPwdPage();
                break;
            case "regpwdok":
                RegPwdOkPage();
                break;
            case "reemail":
                ReEmailPage();
                break;
            case "reemailok":
                ReEmailOkPage();
                break;
            default:
                AgreementPage();
                break;
        }
    }

    private void ReloadPage()
    {

        int rd = int.Parse(Utils.GetRequest("rd", "get", 1, @"^[1-9]\d*$", "0"));
        string mobile = Utils.GetRequest("mobile", "get", 1, @"^(?:13|14|15|18)\d{9}$", "");

       // builder.Append(ub.GetSub("SiteName", "/Controls/wap.xml") );
        //if (Utils.GetTopDomain().Contains("kb288.net"))
        //{
        //    Master.Title = "注册会员账号";
        //    builder.Append(Out.Tab("<div>", ""));
        //    builder.Append("手机号(找回密码用):");
        //    builder.Append(Out.Tab("</div>", ""));
        //    string strText = ",设置登陆密码:/,确认登陆密码:/,,,";
        //    string strName = "mobile,Pwd,Pwdr,rd,act,backurl";
        //    string strType = "text,password,password,hidden,hidden,hidden";
        //    string strValu = "'''" + rd + "'ok'" + Utils.getPage(0) + "";
        //    string strEmpt = "false,false,false,false,false,false";
        //    string strIdea = "/";
        //    string strOthe = "确认注册,reg.aspx,post,0,red";
        //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //    builder.Append(Out.Tab("<div>", "<br />"));
        //    builder.Append("（确认注册后即代表你已同意本站的一切条款和规定)<br />");
        //    builder.Append("注册成功后，请编写短信888到15992310086即可成为验证会员<br />");
        //    builder.Append("注册成功达7天后还未进行验证的ID，系统将进行回收处理<br />");
        //    builder.Append("注册后请在个人空间设置银行资料");
        //    builder.Append(Out.Tab("</div>", "<br />"));

        //}
        //else
        {
            builder.Append(Out.Tab("<div>", ""));
            // builder.Append("<a href=\"" + Utils.getUrl("RegPage.aspx") + "\">手工免费注册</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">免费注册会员</div>", ""));

            if (ub.GetSub("RegLeibie", xmlPath2) == "1" || ub.GetSub("RegLeibie", xmlPath2) == "2")
            {
                if (ub.GetSub("RegEmailPost", xmlPath2) == "1")
                {
                    Master.Title = "免费注册-邮箱验证";
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("*手机号(身份认证重要信息):");
                    builder.Append(Out.Tab("</div>", ""));
                    string strText = ",*设定密码(6-20字符):/,*确认密码:/,*邮箱地址:/,,,";
                    string strName = "mobile,Pwd,Pwdr,Email,rd,act,backurl";
                    string strType = "text,password,password,text,hidden,hidden,hidden";
                    string strValu = "''''" + rd + "'ok'" + Utils.getPage(0) + "";
                    string strEmpt = "false,false,false,false,false,false,false";
                    string strIdea = "/";
                    string strOthe = "提交注册,reg.aspx,post,0,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("", "<br />"));
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("*手机号(找回密码用):");
                    builder.Append(Out.Tab("</div>", ""));
                    string strText = ",*设定密码(6-20字符):/,*确认密码:/,*请输入验证码:/,,,";
                    string strName = "mobile,Pwd,Pwdr,code,rd,act,backurl";
                    string strType = "text,password,password,text,hidden,hidden,hidden";
                    string strValu = "''''" + rd + "'ok'" + Utils.getPage(0) + "";
                    string strEmpt = "false,false,false,false,false,false,false";
                    string strIdea = "'''<img src=\"bbs/pwd/Code.aspx\"/>'''|/";
                    string strOthe = "注册(并自动登陆),reg.aspx,post,0,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("", "<br />"));
                }
            }
            if (ub.GetSub("RegLeibie", xmlPath2) == "0" || ub.GetSub("RegLeibie", xmlPath2) == "2")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.BasUBB(ub.GetSub("RegSmsContent", xmlPath2)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            DataSet ds = new BCW.BLL.User().GetList("TOP 5 Mobile", "Isverify>=0 ORDER BY RegTime DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("=最新注册前五位=<a href=\"" + Utils.getUrl("reg.aspx") + "\">刷新</a>");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("<br />" + Utils.Left(ds.Tables[0].Rows[i]["Mobile"].ToString(), 5) + "******");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("reg.aspx") + "\">站内协议</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //---------------------------------------------knnnk.com--------------------------------------------
    private void RegPwdPage()
    {

        //权限
        if (Utils.GetTopDomain() == "knnnk.com")
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid != 1 && meid != 2)
            {
                Utils.Error("权限不足", "");
            }
            Master.Title = "免费注册/重置密码";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("免费注册/重置密码");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "手机号:/,,";
            string strName = "mobile,act,backurl";
            string strType = "text,hidden,hidden";
            string strValu = "'regpwdok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "注册会员|重置密码,reg.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RegPwdOkPage()
    {
        Master.Title = "免费注册/重置密码";
        if (Utils.GetTopDomain() == "knnnk.com")
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid != 1 && meid != 2)
            {
                Utils.Error("权限不足", "");
            }

            string mobile = Utils.GetRequest("mobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            builder.Append(Out.Tab("<div class=\"title\">免费注册/重置密码</div>", ""));

            if (Utils.ToSChinese(ac) == "注册会员")
            {
                if (new BCW.BLL.User().Exists(mobile))
                {
                    Utils.Error("很抱歉,此手机号已经注册", "");
                }
                //取得会员ID
                int maxId = BCW.User.Reg.GetRandId();
                if (maxId == 0)
                {
                    Utils.Error("很抱歉,服务器繁忙，请稍后注册..", "");
                }
                //加密用户密码
                string strPwd = Utils.MD5Str(Utils.Right(mobile, 6));
                //取随机识别串
                string UsKey = new Rand().RandNum(10);

                string newName = ub.GetSub("RegName", "/Controls/reg.xml");
                if (newName == "")
                    newName = "新会员";

                //写入注册表
                BCW.Model.User model = new BCW.Model.User();
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
                model.InviteNum = 0;
                model.IsVerify = 0;
                model.Email = "";
                new BCW.BLL.User().Add(model);
                //发送内线
                new BCW.BLL.Guest().Add(model.ID, model.UsName, ub.GetSub("RegGuest", "/Controls/reg.xml"));
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, maxId);

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("注册成功！<br />");
                builder.Append("注册ID:" + maxId + "<br />注册手机号:" + mobile + "<br />登录密码:" + Utils.Right(mobile, 6) + "");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (Utils.ToSChinese(ac) == "重置密码")
            {
                if (BCW.User.Reg.GetPwdSms(mobile))//找回登录密码
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("重置密码成功！<br />");
                    builder.Append("手机号:" + mobile + "<br />登录密码:" + Utils.Right(mobile, 6) + "");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }


    }
    //---------------------------------------------knnnk.com--------------------------------------------

    /// <summary>
    /// 取消积分操作
    /// </summary>
    private void OkPage()
    {
        if (ub.GetSub("RegLeibie", xmlPath2) == "0" || ub.GetSub("RegLeibie", xmlPath2) == "3")
        {
            Utils.Error("手工注册已关闭", "");
        }
        Master.IsFoot = false;
        string mobile = Utils.GetRequest("mobile", "post", 2, @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$", "请正确输入十一位数的手机号码");
        string code = Utils.GetRequest("code", "post", 2, @"^[0-9]{4}$", "请输入验证码!");  //界面图形验证码                                                                      ////   string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入手机验证码");  //手机验证码
        string mycode = "";
        try
        {
            mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();// 图形验证码
        }
        catch { }
        string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
        string Pwdr = Utils.GetRequest("Pwdr", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
        int rd = int.Parse(Utils.GetRequest("rd", "post", 1, @"^[1-9]\d*$", "0"));
        code = Utils.GetRequest("code", "post", 1, @"^[0-9]{4}$", "");  //界面图形验证码
        mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();// 图形验证码
        if (!code.Equals(mycode))//验证码相等
        {
            Utils.Error("很抱歉,您输入图形验证码不对，请刷新页面", "");
        }
        if (!Pwd.Equals(Pwdr))
        {
            Utils.Error("很抱歉,您输入的两次密码不相符", "");
        }
        if (new BCW.BLL.User().Exists(mobile))
        {
            Utils.Error("很抱歉,此手机号已经注册", "");
        }
        if (rd > 0)
        {
            if (!new BCW.BLL.User().Exists(rd))
            {
                Utils.Success("注册会员", "注册地址失效，请<a href=\"/bbs/reg.aspx\">重新注册</a>", "reg.aspx", "2");
            }
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
        // int maxId = new BCW.BLL.User().GetMaxId() + 1;
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
        ////积分操作
        //new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, maxId);
        //if (rd > 0 && model.IsVerify == 1)
        //{
        //    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RecomUser, rd);
        //}
        builder.Append(Out.Tab("<div class=\"title\">免费注册会员</div>", ""));

        if (ub.GetSub("RegEmailPost", xmlPath2) == "1")
        {
            #region 邮箱
            //设置keys
            string keys = "";
            keys = BCW.User.Users.SetUserKeys(model.ID, model.UsPwd, model.UsKey);
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            string strUrl = "http://" + Utils.GetDomain() + "/bbs/myedit.aspx?act=regyz&amp;postId=" + DESEncrypt.Encrypt(UsKey, "ct201200108") + "&" + VE + "=2a&" + SID + "=" + keys;

            //设定参数
            string EmailFrom = "nowtx.net@gmail.com";
            string EmailFromUser = "nowtx.net";
            string EmailFromPwd = "bichengwei";
            string EmailFromHost = "smtp.gmail.com";
            string EmailFromPort = "587";
            if (ub.GetSub("EmailFrom", xmlPath) != "" && ub.GetSub("EmailFromUser", xmlPath) != "" && ub.GetSub("EmailFromPwd", xmlPath) != "" && ub.GetSub("EmailFromHost", xmlPath) != "" && ub.GetSub("EmailFromPort", xmlPath) != "")
            {
                EmailFrom = ub.GetSub("EmailFrom", xmlPath);
                EmailFromUser = ub.GetSub("EmailFromUser", xmlPath);
                EmailFromPwd = ub.GetSub("EmailFromPwd", xmlPath);
                EmailFromHost = ub.GetSub("EmailFromHost", xmlPath);
                EmailFromPort = ub.GetSub("EmailFromPort", xmlPath);

            }
            // 发件人地址
            string From = EmailFrom;
            // 收件人地址
            string To = Email;
            // 邮件主题
            string Subject = "来自" + ub.Get("SiteName").ToString() + "-注册系统";
            // 邮件正文
            string Body = "尊敬的" + model.UsName + "您好:<br>你的手机号:" + mobile + "<br>会员ID号:" + model.ID + "<br>登录密码:" + Pwd.Trim() + "<br>点击以下链接马上完成本次注册:<br><a href=\"" + strUrl + "\">" + strUrl + "</a><br>如果无法点击链接，请复制进地址栏再进入即可<br>此信件属系统自动发送，请勿回复";
            // 邮件主机地址
            string Host = EmailFromHost;
            // 邮件主机端口
            int Port = Utils.ParseInt(EmailFromPort);
            // 登录帐号
            string UserName = EmailFromUser;
            // 登录密码
            string Password = EmailFromPwd;
            //附件地址
            string FilePath = "";
            new SendMail().Send(From, To, Subject, Body, Host, Port, UserName, Password, FilePath);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("恭喜您提交成功！<br />");
            builder.Append("系统已经将你的注册信息发送至您的邮箱，请登录邮箱验证链接即可完成注册.<br />您可能收不到邮件？请进入垃圾箱找找看.");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region 手工注册
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("恭喜您注册成功！<br />");
            builder.Append("注册ID:" + maxId + "<br />注册手机号:" + mobile + "<br />登录密码:" + Pwd.Trim() + "");
          //  builder.Append("<br />重要:请编写短信<a href=\"sms:15992310086?body=111\" >111</a>到<a href=\"wtai://wp/mc;15992310086\" >15992310086</a>即可成为验证会员，未进行验证的ID系统将在7天后回收");
            builder.Append("<br/><a href=\"" + Utils.getUrl("reg.aspx?act=validate") + "\">马上验证为会员！&gt;&gt;</a>");
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
            //Cookie.Values.Add("userkeys", DESEncrypt.Encrypt(Utils.Mid(keys, 0, keys.Length - 4)));
            //Response.Cookies.Add(Cookie);

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
            #endregion
        }

    }
    /// <summary>
    /// 手机验证
    /// </summary>
    private void validate()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
        {
            Utils.Login();
        }
        string keys = "";
        BCW.Model.User model = new BCW.BLL.User().GetKey(meid);
        keys = BCW.User.Users.SetUserKeys(meid, model.UsPwd, model.UsKey);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string mobile = new BCW.BLL.User().GetMobile(meid);
        string code = Utils.GetRequest("code", "all", 1, @"^[0-9]{4}$", "请输入验证码!");  //界面图形验证码                                                                      ////   string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入手机验证码");  //手机验证码
        string mycode = "";
        try
        {
            mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();// 图形验证码
        }
        catch { }

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"/bbs/uinfo.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">空间</a>");
            //  builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">-完善资料</a>");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Model.tb_Validate getmo = null;
            DateTime endtime = DateTime.Now.AddMinutes(-1);
            if (new BCW.BLL.tb_Validate().ExistsPhone(mobile, 1))//注册验证码
            {
                getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 1);//获取注册验证码
                endtime = getmo.codeTime;
                string Reg = string.Empty;
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
                    builder.Append(Out.Tab("<div class=\"\">", Out.Hr()));
                    builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            string strText = "*请输入手机验证码:/,*请输入验证码:/,,,,";
            string strName = "phoneCode,code,info,mobile,act,backurl";
            string strType = "text,text,hidden,hidden,hidden,hidden";
            string strValu = "''ok'" + mobile + "'validate'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "<a href=\"" + Utils.getUrl("reg.aspx?act=validate&amp;info=ok&amp;ac=ok") + "\">获取手机验证码</a>'<img src=\"bbs/pwd/Code.aspx\"/>''''|/";
            string strOthe = "马上验证,reg.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"/bbs/uinfo.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">空间</a>");
            //  builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">-完善资料</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac).Contains("获取手机验证码") || ac == "ok")    //判断哪一个按键
            {
                #region 获取手机验证码
                if (!code.Equals(mycode))//验证码相等
                {
                 //   Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
                }
                if (new BCW.BLL.tb_Validate().ExistsPhone(mobile, 1))//存在注册验证
                {
                    BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 1);
                    if (getmo.codeTime > DateTime.Now)//验证码存在切是新发的
                    {
                        string dateDiff = null;
                        TimeSpan x = getmo.codeTime - DateTime.Now;
                        dateDiff = x.TotalSeconds.ToString();
                        Utils.Error("很抱歉,请在" + dateDiff.Split('.')[0] + "秒之后再次获取手机验证码", "");
                    }
                }
                char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
                string mesCode = string.Empty; //手机验证码
                Random rnd = new Random();
                //生成验证码字符串
                for (int i = 0; i < 4; i++)
                {
                    mesCode += character[rnd.Next(character.Length)];
                }

                //int tm = 2;//短信过期时间分钟
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
                        validate.type = 1;
                        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                        string result = "";
                        result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
                        string[] results = result.Split('#');
                        if (results[8] != "0")
                        {
                            Utils.Error("请确认手机号的正确性，如不能为空号!", "");
                        }
                        if ((int.Parse(results[2]) / 80) < msgremain)
                        {
                            new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
                        }
                        if (results[8] == "0")
                        {
                            new BCW.BLL.tb_Validate().Add(validate);
                            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("reg.aspx?act=validate"), "2");
                        }
                    }
                    else//当天时间内
                    {
                        //获取当天的短信数量
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
                        //获取最近半小时的短信量
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
                            ////跟新标示
                            //int ID = int.Parse(dt3.Tables[0].Rows[0]["ID"].ToString());
                            //new BCW.BLL.tb_Validate().UpdateFlag(0, ID);
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
                            validate.type = 1;
                            Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                            string result = "";
                            result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
                            string[] results = result.Split('#');
                            if (results[8] != "0")
                            {
                                Utils.Error("请确认手机号的正确性，如不能为空号!", "");
                            }
                            if ((int.Parse(results[2]) / 80) < msgremain)
                            {
                                new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
                            }
                            if (results[8] == "0")
                            {
                                new BCW.BLL.tb_Validate().Add(validate);
                                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("reg.aspx?act=validate"), "2");
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
                    validate.type = 1;
                    Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                    string result = "";
                    result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
                    string[] results = result.Split('#');
                    if (results[8] != "0")
                    {
                        Utils.Error("请确认手机号的正确性，如不能为空号!", "");
                    }
                    if ((int.Parse(results[2]) / 80) < msgremain)
                    {
                        new BCW.BLL.Guest().Add(0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!");
                    }
                    if (results[8] == "0")
                    {
                        new BCW.BLL.tb_Validate().Add(validate);
                        Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("reg.aspx?act=validate"), "2");
                    }
                }


                #endregion
            }
            else
            {
              //  Utils.Error("come here", "");
                string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");  //手机验证码
                if (!code.Equals(mycode))//验证码相等
                {
                    Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
                }
                BCW.Model.tb_Validate getmo = null;
                if (new BCW.BLL.tb_Validate().ExistsPhone(mobile, 1))//有发送注册过验证码
                {
                    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 1);
                    if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
                    {
                        if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
                        {
                            Utils.Error("很抱歉,您输入手机验证码不对222", "");
                        }
                    }
                    else { Utils.Error("手机验证码过期，请重新获取", ""); }
                }
                else//没发送过注册验证码
                {
                    Utils.Error("很抱歉,您输入手机验证码不对111", "");
                }
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, meid);
                //if (rd > 0 && model.IsVerify == 1)
                //{
                //    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RecomUser, rd);
                //}
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>");
                //   builder.Append("<a href=\"/bbs/uinfo.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">空间</a>");
                //  builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">-完善资料</a>");
                builder.Append(Out.Tab("</div>", ""));
                new BCW.BLL.User().UpdateIsVerify(mobile, 1);
                //string strText = "";
                //string strName = "act";
                //string strType = "hidden";
                //string strValu = "recommended";
                //string strEmpt = "false";
                //string strIdea = "/";
                //string strOthe = "马上填写推荐人ID,reg.aspx,post,0,red";
                //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                Utils.Success("验证成功，请填写推荐人ID", "验证成功,请填写推荐人ID", Utils.getUrl("reg.aspx?act=recommended"), "3");
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>");
                //   builder.Append("<a href=\"/bbs/uinfo.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">空间</a>");
                //  builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">-完善资料</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

        }
    }
    /// <summary>
    /// 填写推荐人ID
    /// </summary>
    private void recommended()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
        {
            Utils.Login();
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>");
         //   builder.Append("<a href=\"/bbs/uinfo.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">空间</a>");
            //  builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">-完善资料</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "*请填写推荐人ID:/,,,";
            string strName = "recomID,info,act,backurl";
            string strType = "text,hidden,hidden,hidden";
            string strValu = "'ok'recommended'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认,reg.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>");
         //   builder.Append("<a href=\"/bbs/uinfo.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">空间</a>");
            //  builder.Append("<a href=\"/bbs/myedit.aspx?act=basic&amp;ve=" + Utils.getstrVe() + "&amp;u=" + keys + "\">-完善资料</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int InviteNum =new BCW.BLL.User().GetInviteNum(meid); //得到推荐自己的ID
            if (InviteNum == 0)
            {
                //处理推荐人ID
                string recomID = Utils.GetRequest("recomID", "all", 3, @"^[0-9]\d+$", "");
                // Utils.Error("recomID:" + recomID, "");
                int rd = int.Parse(recomID);
                if (rd > 0)
                {
                    if (!new BCW.BLL.User().Exists(rd))
                    {
                        Utils.Error("很抱歉,推荐ID不存在", "");
                    }
                }
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RecomUser, rd);
             //   Utils.Error("meid+"+ meid, "");
                new BCW.BLL.User().UpdateInviteNum(meid, rd);
                Utils.Success("填写推荐人ID成功", "填写推荐人ID成功", Utils.getUrl("default.aspx"), "3");
            }
            else
            {
                Utils.Error("你已经填写过推荐人ID啦！","");
            }
        }
    }
    private void ReEmailPage()
    {
        Master.Title = "重发邮箱验证";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("重发邮箱验证");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "注册手机号:/,您在资料填写的邮箱:/,,";
        string strName = "mobile,Email,act,backurl";
        string strType = "text,text,hidden,hidden";
        string strValu = "''reemailok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "重发验证,reg.aspx,post,0,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("login.aspx") + "\">重新登录</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ReEmailOkPage()
    {
        string mobile = Utils.GetRequest("mobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string Email = Utils.GetRequest("Email", "post", 2, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "请输入正确的邮箱地址");

        if (!new BCW.BLL.User().Exists(mobile, Email))
        {
            Utils.Error("重发验证失败，请检查手机号和邮箱是否填写正确", "");
        }
        BCW.Model.User model = new BCW.BLL.User().GetPwdBasic(mobile);
        //设置keys
        string keys = "";
        keys = BCW.User.Users.SetUserKeys(model.ID, model.UsPwd, model.UsKey);
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string strUrl = "http://" + Utils.GetDomain() + "/bbs/myedit.aspx?act=regyz&postId=" + DESEncrypt.Encrypt(model.UsKey, "ct201200108") + "&" + VE + "=2a&" + SID + "=" + keys;
        string strUrl2 = "http://" + Utils.GetDomain() + "/bbs/myedit.aspx?act=checkpwd&postId=" + DESEncrypt.Encrypt(model.UsKey, "ct201200108") + "&" + VE + "=2a&" + SID + "=" + keys;

        //设定参数
        string EmailFrom = "nowtx.net@gmail.com";
        string EmailFromUser = "nowtx.net";
        string EmailFromPwd = "bichengwei";
        string EmailFromHost = "smtp.gmail.com";
        string EmailFromPort = "587";
        if (ub.GetSub("EmailFrom", xmlPath) != "" && ub.GetSub("EmailFromUser", xmlPath) != "" && ub.GetSub("EmailFromPwd", xmlPath) != "" && ub.GetSub("EmailFromHost", xmlPath) != "" && ub.GetSub("EmailFromPort", xmlPath) != "")
        {
            EmailFrom = ub.GetSub("EmailFrom", xmlPath);
            EmailFromUser = ub.GetSub("EmailFromUser", xmlPath);
            EmailFromPwd = ub.GetSub("EmailFromPwd", xmlPath);
            EmailFromHost = ub.GetSub("EmailFromHost", xmlPath);
            EmailFromPort = ub.GetSub("EmailFromPort", xmlPath);

        }
        // 发件人地址
        string From = EmailFrom;
        // 收件人地址
        string To = Email;
        // 邮件主题
        string Subject = "来自" + ub.Get("SiteName").ToString() + "-注册系统";
        // 邮件正文
        string Body = "尊敬的" + model.UsName + "您好:<br>你的手机号:" + mobile + "<br>会员ID号:" + model.ID + "<br>点击以下链接马上完成本次注册:<br><a href=\"" + strUrl + "\">" + strUrl + "</a><br>如忘记密码请验证后点击以下链接找回:<br><a href=\"" + strUrl2 + "\">" + strUrl2 + "</a><br>如果无法点击链接，请复制进地址栏再进入即可<br>此信件属系统自动发送，请勿回复";
        // 邮件主机地址
        string Host = EmailFromHost;
        // 邮件主机端口
        int Port = Utils.ParseInt(EmailFromPort);
        // 登录帐号
        string UserName = EmailFromUser;
        // 登录密码
        string Password = EmailFromPwd;
        //附件地址
        string FilePath = "";
        new SendMail().Send(From, To, Subject, Body, Host, Port, UserName, Password, FilePath);

        Utils.Success("重发验证邮件", "重发验证邮件成功，请登录您的邮箱进行相关操作", "login.aspx", "3");
    }

    private void PwdPage()
    {
        Master.Title = "找回密码";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ub.GetSub("RegLeibie", xmlPath2) == "0" || ub.GetSub("RegLeibie", xmlPath2) == "2")
        {
            builder.Append("<a href=\"" + Utils.getUrl("reg.aspx") + "\">短信找回密码</a>");
        }
        else
        {
            builder.Append("找回我的密码");
        }
        builder.Append(Out.Tab("</div>", ""));
        string strText = "注册手机号:/,您在资料填写的邮箱:/,,";
        string strName = "mobile,Email,act,backurl";
        string strType = "text,text,hidden,hidden";
        string strValu = "''pwdok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "找回密码,reg.aspx,post,0,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("login.aspx") + "\">重新登录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PwdOkPage()
    {
        string mobile = Utils.GetRequest("mobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string Email = Utils.GetRequest("Email", "post", 2, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "请输入正确的邮箱地址");

        if (!new BCW.BLL.User().Exists(mobile, Email))
        {
            Utils.Error("找回密码失败，请检查手机号和邮箱是否填写正确", "");
        }
        BCW.Model.User model = new BCW.BLL.User().GetPwdBasic(mobile);
        //设置keys
        string keys = "";
        keys = BCW.User.Users.SetUserKeys(model.ID, model.UsPwd, model.UsKey);
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string strUrl = "http://" + Utils.GetDomain() + "/bbs/myedit.aspx?act=checkpwd&postId=" + DESEncrypt.Encrypt(model.UsKey, "ct201200108") + "&" + VE + "=2a&" + SID + "=" + keys;

        //设定参数
        string EmailFrom = "nowtx.net@gmail.com";
        string EmailFromUser = "nowtx.net";
        string EmailFromPwd = "bichengwei";
        string EmailFromHost = "smtp.gmail.com";
        string EmailFromPort = "587";
        if (ub.GetSub("EmailFrom", xmlPath) != "" && ub.GetSub("EmailFromUser", xmlPath) != "" && ub.GetSub("EmailFromPwd", xmlPath) != "" && ub.GetSub("EmailFromHost", xmlPath) != "" && ub.GetSub("EmailFromPort", xmlPath) != "")
        {
            EmailFrom = ub.GetSub("EmailFrom", xmlPath);
            EmailFromUser = ub.GetSub("EmailFromUser", xmlPath);
            EmailFromPwd = ub.GetSub("EmailFromPwd", xmlPath);
            EmailFromHost = ub.GetSub("EmailFromHost", xmlPath);
            EmailFromPort = ub.GetSub("EmailFromPort", xmlPath);

        }
        // 发件人地址
        string From = EmailFrom;
        // 收件人地址
        string To = Email;
        // 邮件主题
        string Subject = "来自" + ub.Get("SiteName").ToString() + "-找回密码系统";
        // 邮件正文
        string Body = "尊敬的" + model.UsName + "您好:<br>你的手机号:" + mobile + "<br>会员ID号:" + model.ID + "<br>点击以下链接进入修改您的密码:<br><a href=\"" + strUrl + "\">" + strUrl + "</a><br>如果无法点击链接，请复制进地址栏再进入即可<br>此信件属系统自动发送，请勿回复";
        // 邮件主机地址
        string Host = EmailFromHost;
        // 邮件主机端口
        int Port = Utils.ParseInt(EmailFromPort);
        // 登录帐号
        string UserName = EmailFromUser;
        // 登录密码
        string Password = EmailFromPwd;
        //附件地址
        string FilePath = "";
        new SendMail().Send(From, To, Subject, Body, Host, Port, UserName, Password, FilePath);

        Utils.Success("找回密码", "找回密码成功，您的密码已发送到你的邮箱", "login.aspx", "3");
    }

    private void AgreementPage()
    {
        Master.Title = "注册协议";
        builder.Append(Out.Tab("<div class=\"title\">会员注册协议</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        string xmlPath = "/Controls/reg.xml";
        string Agreement = ub.GetSub("RegAgreement", xmlPath);
        builder.Append(Out.SysUBB(Agreement));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("reg.aspx?act=agreement") + "\">&gt;同意注册协议</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">&gt;不同意注册协议</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}