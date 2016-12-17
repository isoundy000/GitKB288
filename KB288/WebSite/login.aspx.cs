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
/// 修改登录协议
/// 黄国军 20160713
/// </summary>
public partial class login : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "用户登录";
        Master.IsFoot = false;
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                OkPage();
                break;
            case "agreement":
                AgreementPage();
                break;
            case "vipok":
                VipOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("reg.aspx?backurl=" + Utils.getPage(0) + "") + "\">还没有注册?</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "*ID或手机号码:/,*您的密码:/,选择登录状态:/,,,";
        string strName = "Name,Pwd,State,IsMy,act,backurl";
        string strType = "text,password,select,hidden,hidden,hidden";
        string strValu = "''0'False'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,0|正常登录|1|隐身登录,false,false,false";
        string strIdea = "/";
        string strOthe = "同意协议并登录|安全登录,login.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("login.aspx?act=agreement") + "\">[阅读站内协议]</a>");
        builder.Append(Out.Tab("</div>", ""));

        //增加协议后无法使用原有表格提交
        //builder.Append(@"
        //<form name=""form"" action=""login.aspx"" method=""post""><div>*ID或手机号码:<br />
        //<input maxlength=""500"" type=""text"" emptyok=""false"" name=""Name""  /></div>
        //<div>*您的密码:<br /><input maxlength = ""500"" type = ""password"" emptyok = ""false"" name = ""Pwd"" /></div>
        //<div>选择登录状态:<br/><select name = ""State"" ><option selected = ""true"" value = ""0"">正常登录 </option>
        //<option  value = ""1"">隐身登录</option ></select></div>
        //<input type = ""hidden"" name = ""IsMy"" value = ""False"" />
        //<input type = ""hidden"" name = ""act"" value = ""ok"" />
        //<input type = ""hidden"" name = ""backurl"" value = ""%2fdefault.aspx"" />
        //<input type = ""hidden"" name = ""ve"" value = ""1a"" />
        //<input type = ""hidden"" name = ""u"" value = ""nabd1bc948cd121f0io8j"" /><div>
        //<a href = ""login.aspx?act=agreement&amp;ve=2a&amp;u=nabd1bc948cd121f0io8j"">请先阅读站内协议</a></div>
        //<div><input class=""btn-red"" type=""submit"" name=""ac"" value=""同意协议并登录""/>
        //<input class=""btn-blue"" type=""submit"" name=""ac"" value=""安全登录""/></div></form>");

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("温馨提示:<br />隐身登录功能仅限VIP会员使用<br />使用安全登录将导致该ID的全部书签失效,让您的帐号更安全可靠！");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/pwd/GetPwd.aspx?act=fangfa") + "\">忘记密码</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("/reg.aspx?act=pwd") + "\">忘记密码</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("/bbs/default.aspx?id=20") + "\">忘记密码</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    private void OkPage()
    {
        string Name = Utils.GetRequest("Name", "post", 2, @"^\d{1,10}$|^(?:13|14|15|18)\d{9}$", "请正确输入ID或者手机号");
        string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "请正确输入您的密码");
        bool IsMy = bool.Parse(Utils.GetRequest("IsMy", "post", 1, @"^True|False$", "False"));
        int State = int.Parse(Utils.GetRequest("State", "post", 1, @"^[0-1]$", "0"));

        int rowsAffected = 0;
        BCW.Model.User modellogin = new BCW.Model.User();
        modellogin.UsPwd = Utils.MD5Str(Pwd);
        if (Name.Length == 11)
        {
            modellogin.Mobile = Name;
            rowsAffected = new BCW.BLL.User().GetRowByMobile(modellogin);
        }
        else
        {
            modellogin.ID = Convert.ToInt32(Name);
            rowsAffected = new BCW.BLL.User().GetRowByID(modellogin);
        }

        if (rowsAffected > 0)
        {
            BCW.Model.User model = new BCW.Model.User();
            //if (Name.Length == 11)
            //{
            //    model = new BCW.BLL.User().GetKey(Name);
            //}
            //else
            //{
            //model = new BCW.BLL.User().GetKey(Convert.ToInt32(Name));
            //}

            model = new BCW.BLL.User().GetKey(rowsAffected);


            int UsId = model.ID;
            string UsKey = model.UsKey;
            string UsPwd = model.UsPwd;
            int IsVerify = model.IsVerify;
            if (IsVerify == 2)
            {
                Utils.Error("您的ID还没有通过邮箱验证，如您没到验证邮件，<a href=\"" + Utils.getUrl("reg.aspx?act=reemail") + "\">请重试</a>", "");
            }
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "安全登录")
            {
                //取随机识别串
                UsKey = new Rand().RandNum(10);
                new BCW.BLL.User().UpdateUsKey(UsId, UsKey);
            }

            //加密用户密码
            string strPwd = Utils.MD5Str(UsPwd.Trim());
            //设置keys
            string keys = "";
            keys = BCW.User.Users.SetUserKeys(UsId, UsPwd, UsKey);
            string bUrl = string.Empty;
            if (Utils.getPage(1) != "")
            {
                bUrl = Utils.getUrl(Utils.removeUVe(Utils.getPage(1)));
            }
            else
            {
                bUrl = Utils.getUrl("/default.aspx");
            }
            //更新识别串
            string SID = ConfigHelper.GetConfigString("SID");
            bUrl = UrlOper.UpdateParam(bUrl, SID, keys);
            //写入Cookie
            if (IsMy == true)
            {
                HttpCookie cookie = new HttpCookie("LoginComment");
                cookie.Expires = DateTime.Now.AddDays(30);//30天
                cookie.Values.Add("userkeys", DESEncrypt.Encrypt(Utils.Mid(keys, 0, keys.Length - 4)));
                Response.Cookies.Add(cookie);
            }
            else
            {
                //清Cookie
                HttpCookie Cookie = new HttpCookie("LoginComment");
                Cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
            if (Request["IsMy"] == "pwd")
            {
                string ForumSet = new BCW.BLL.User().GetForumSet(UsId);
                string[] fs = ForumSet.Split(",".ToCharArray());
                string sforumsets = string.Empty;
                for (int i = 0; i < fs.Length; i++)
                {
                    string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                    if (i == 25)
                    {
                        sforumsets += "," + sfs[0] + "|" + DateTime.Now;
                    }
                    else
                    {
                        sforumsets += "," + sfs[0] + "|" + sfs[1];
                    }
                }
                sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
                new BCW.BLL.User().UpdateForumSet(UsId, sforumsets);

            }
            //----------------------写入登录日志文件作永久保存
            try
            {
                string MyIP = Utils.GetUsIP();
                string ipCity = string.Empty;
                if (!string.IsNullOrEmpty(MyIP))
                {

                    ipCity = new IPSearch().GetAddressWithIP(MyIP);
                    if (!string.IsNullOrEmpty(ipCity))
                    {
                        ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
                    }

                    string FilePath = System.Web.HttpContext.Current.Server.MapPath("/log/loginip/" + UsId + "_" + DESEncrypt.Encrypt(UsId.ToString(), "kubaoLogenpt") + ".log");
                    LogHelper.Write(FilePath, "" + ipCity + "" + MyIP + "(登录)");
                }
            }
            catch { }
            //----------------------写入日志文件作永久保存
            new BCW.BLL.User().UpdateTime(UsId);

            //VIP隐身
            int VipLeven = BCW.User.Users.VipLeven(UsId);
            if (new BCW.BLL.Role().IsAllMode(UsId) == false && VipLeven > 0)
            {
                new BCW.BLL.User().UpdateState(UsId, 1);
            }


            Utils.Success("登录成功", "登录成功!返回登录前的页面..<br />如果没有自动跳转,请点击以下链接..", Out.SysUBB(bUrl), "2");
        }
        else
        {
            Utils.Error("登录失败，会员ID/手机号码或密码错误", "");
        }
    }

    private void VipOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //VIP隐身
        int VipLeven = BCW.User.Users.VipLeven(meid);
        if (VipLeven > 0)
        {
            new BCW.BLL.User().UpdateState(meid, 1);
            new BCW.BLL.User().UpdateTime(meid);
            Utils.Success("隐身登录", "隐身登录成功!返回登录前的页面..<br />如果没有自动跳转,请点击以下链接..", Utils.getPage("/default.aspx"), "2");
        }
        else
        {
            new BCW.BLL.User().UpdateTime(meid);
            Utils.Success("隐身登录", "您不是VIP会员，正在使用在线登录方式登录..<br />如果没有自动跳转,请点击以下链接..", Utils.getPage("/default.aspx"), "2");
        }

    }

    #region 登录协议 AgreementPage
    private void AgreementPage()
    {
        Master.Title = "登录协议";
        builder.Append(Out.Tab("<div class=\"title\">会员登录</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        string xmlPath = "/Controls/reg.xml";
        string Agreement = ub.GetSub("RegAgreement", xmlPath);
        builder.Append(Out.SysUBB(Agreement));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("login.aspx") + "\">&gt;同意登录协议</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">&gt;不同意登录协议</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion
}
