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
/// 陈志基 忘记密码使用短信20160910
/// </summary>
public partial class bbs_pwd_GetPwd : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected string MyMobile;
    protected int flag = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "mibao":
                Mibaoshuoming();
                break;
            case "forgetpsw":
                ForgetPsw();
                break;
            case "secondsave":
                SecondSave();
                break;
            case "changetoDB":
                ChangeToDB();
                break;
            case "forvalidate":
                forvalidate();
                break;
            case "fangfa":
                Fangfa();
                break;
            case "vacode":
                vacode();
                break;
            case "validate":
                validate();
                break;
            default:
                ReloadPage();//忘记密码管理，就是主页
                break;
        }


    }

    private void ReloadPage()   //初始的界面,获取密码的方法介绍          111
    {

        Master.Title = "更多功能";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=更多功能=" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SetQuestion.aspx") + "\">设置密码保护问题</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("MyPhone.aspx") + "\">设置手机号码</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("GetPwd.aspx?act=fangfa") + "\">忘记密码</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/myedit.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ChangeToDB()    //保存数据库
    {


        string newpwd = Utils.GetRequest("newpwd", "post", 2, @"^[^\^]{6,20}$", "请输入六到二十个字符的密码");
        string checkpwd = Utils.GetRequest("checkpwd", "post", 2, @"^[^\^]{6,20}$", "请输入六到二十个字符的密码");
        string account = Utils.GetRequest("account", "all", 0, "", "");
        try
        {

            account = new BCW.BLL.Security().DecryptQueryString(account);
            MyMobile = new BCW.BLL.User().GetMobile(int.Parse(account));
            if (newpwd.Equals(checkpwd))
            {
                int count = new BCW.BLL.tb_Question().GetChangeCount(int.Parse(account));
                //Utils.Error("MyMobile+" + MyMobile + "count:"+ count, "");
                int nowtime = int.Parse(DateTime.Now.DayOfYear.ToString());// 获取第几天  系统的第几天，就是当前的天数（一年中的天数）             
                int lasttime = new BCW.BLL.tb_Question().GetLastChange(int.Parse(account));   //上一次修改密码的的天数               
                if (lasttime - nowtime < 0)//上一次修改时间和当前的系统时间不在同一天，就刷新一天的修改次数
                {
                    count = 0;
                    new BCW.BLL.tb_Question().UpdateChangeCount(count, int.Parse(account));//更新每天修改密码的次数                   
                }
                if (count < 5)
                {
                    if (Utils.IsRegex(checkpwd, @"^[0-9]\d*$"))
                    {
                        Utils.Error("新密码不能是纯数字密码", "");
                        //Utils.Error("新密码不能是纯数字密码", Utils.getUrl("GetPwd.aspx?act=forgetpsw&amp;MyMobile=" + new BCW.BLL.Security().EncryptQueryString(MyMobile) + ""));
                    }
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<br/> 你的新密码是:" + newpwd + "<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                    count = new BCW.BLL.tb_Question().GetChangeCount(int.Parse(account));//
                    count = count + 1;
                    new BCW.BLL.tb_Question().UpdateChangeCount(count, int.Parse(account));//更新每天修改密码的次数
                    new BCW.BLL.User().UpdateUsPwd(int.Parse(account), Utils.MD5Str(checkpwd));
                    nowtime = int.Parse(DateTime.Now.DayOfYear.ToString());// 获取第几天  系统的第几天，就是当前的天数（一年中的天数）
                    new BCW.BLL.tb_Question().UpdateLastChange(nowtime, MyMobile);//更新最后一次修改的天数                   
                    builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
                    builder.Append("5秒后自动跳转到登陆界面<br />");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                    Master.Refresh = 4;//5秒后跳转到以下地址(可缺省)
                    //   Response.Redirect("GetPwd.aspx?act=fangfa");
                    Master.Gourl = Utils.getUrl("/login.aspx");//跳到的地址
                    builder.Append("<a href=\"" + Utils.getUrl("/login.aspx") + "\">如果还没有跳转，请点击该链接</a>");
                }
                else
                {
                    Utils.Error("非常抱歉，一天之内不能使用忘记密码功能修改密码超过5次，请在次日再使用本功能！.", "");
                    // Utils.Success("温馨提示", "非常抱歉，一天之内不能使用忘记密码功能修改密码超过5次，请在次日再使用本功能！.", Utils.getUrl("GetPwd.aspx"), "5");
                }
            }
            else
            {
                Utils.Error("两次的密码匹配是不正确的，请谨慎输入新的密码", "");
                // Utils.Success("温馨提示", "两次的密码匹配是不正确的，请谨慎输入新的密码.", Utils.getUrl("GetPwd.aspx"), "5");
            }
        }
        catch { }

    }

    private void SecondSave()   //已经设置了密码保护问题的重置密码方法
    {
        string account = Utils.GetRequest("account", "all", 1, "", "");
        //try
        {
            account = new BCW.BLL.Security().DecryptQueryString(account);

            string ac = Utils.GetRequest("ac", "post", 1, "", "");
            string code = Utils.GetRequest("code", "all", 1, @"^[0-9]{4}$", "请输入完整的验证码");
            string mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();
            string answer1 = Utils.GetRequest("Myanswer", "post", 1, @"^[\s\S]{1,20}$", "");  //获取输入的答案

            if (Utils.ToSChinese(ac).Contains("刷新验证码"))    //刷新验证码按键
            {
                #region
                Master.Title = "密保管理";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
                builder.Append(Out.Tab("</div>", "<br/>"));
                string Myquestion = new BCW.BLL.tb_Question().GetQuestion(int.Parse(account));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<b>你的问题是：" + Myquestion + "</b><br />");
                builder.Append(Out.Tab("</div>", ""));
                //MyMobile = new BCW.BLL.Security().EncryptQueryString(MyMobile);  //加密手机号码

                strText = "你的问题答案是:/,输入验证码:/,,,";
                strName = "Myanswer,code,hid,account,act";
                strType = "text,text,hidden,hidden,hidden";
                strValu = "''" + 1 + "'" + new BCW.BLL.Security().EncryptQueryString(account) + "'secondsave";
                strEmpt = "false,false,false,false,false";
                strIdea = "'<img src=\"Code.aspx\"/>'''|/";
                strOthe = "确定提交|刷新验证码,GetPwd.aspx,post,0,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append("<br /><a href=\"" + Utils.getPage("SetQuestion.aspx") + "\">返回密码保护设置（需要登录）&gt;&gt;<br/></a>");
                #endregion
            }
            else   //按了提交键
            {

                //Utils.Error("account" + account + "ac:" + ac, "");
                #region
                if (code.Equals(mycode))
                {
                    //  Utils.Error("code" + code + "mycode:" + mycode, "");
                    string answer = Utils.GetRequest("Myanswer", "post", 1, @"^[\s\S]{1,20}$", "");  //获取输入的答案
                    string Myanser = new BCW.BLL.tb_Question().GetAnswer(int.Parse(account));   //数据库里面的问题答案
                    if (Myanser.Equals(answer))
                    {
                        builder.Append(Out.Tab("<div class=\"title\">", ""));
                        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
                        builder.Append("-新密码" + "<br />");
                        builder.Append(Out.Tab("</div>", ""));
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("<b>注意： 这是你重新输入你的新密码，" + "</b><br/>");
                        builder.Append(Out.Tab("</div>", ""));

                        strText = "你输入的新密码:/,确认新密码/,,,";
                        strName = "newpwd,checkpwd,account,hid,act";
                        strType = "text,text,hidden,hidden,hidden";
                        strValu = "''" + new BCW.BLL.Security().EncryptQueryString(account) + "'" + 1 + "'changetoDB";
                        strEmpt = "false,false,false,false";
                        strIdea = "/";
                        strOthe = "确定提交,GetPwd.aspx,post,0,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    }
                    else
                    {
                        Utils.Error("很抱歉,问题答案输入错误，请重新输入", "");
                        //    Utils.Error("很抱歉,问题答案输入错误，请重新输入", Utils.getUrl("GetPwd.aspx?act=forgetpsw&amp;account=" + new BCW.BLL.Security().EncryptQueryString(account) + ""));
                        // Utils.Success("温馨提示", "问题答案输入错误，请重新输入，3秒后跳转输入界面..", Utils.getUrl("GetPwd.aspx?act=forgetpsw&amp;MyMobile=" + new BCW.BLL.Security().EncryptQueryString(MyMobile) + ""), "3");
                    }
                }
                else
                {
                    Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
                    //Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", Utils.getUrl("GetPwd.aspx?act=forgetpsw&amp;account=" + new BCW.BLL.Security().EncryptQueryString(account) + ""));
                    //  Utils.Success("温馨提示", "验证码错误，请重新输入，3秒后跳转输入界面..", Utils.getUrl("GetPwd.aspx?act=forgetpsw&amp;MyMobile=" + new BCW.BLL.Security().EncryptQueryString(MyMobile) + ""), "3");
                }
                #endregion
            }


        }
        //catch
        //{
        //    builder.Append(Out.Tab("<div class=\"title\">", ""));
        //    builder.Append("账号不存在，请重新输入" + "<br />");
        //    builder.Append(Out.Tab("</div>", ""));
        //    strText = "你的手机号码:/,,";
        //    strName = "MyMobile,hid,act";
        //    strType = "text,hidden,hidden";
        //    strValu = "'" + 1 + "'forgetpsw";
        //    strEmpt = "false,false,false";
        //    strIdea = "/";
        //    strOthe = "确定提交,GetPwd.aspx,post,0,red";
        //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //}
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));


    }

    private void ForgetPsw()   //  忘记密码管理   判断手机号码 333333333
    {
        Master.Title = "密保管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //MyMobile = Utils.GetRequest("account", "all", 2, @"^[\d]+$|^[^\^]{32}$", "请输入正确的手机号码或者ID账号");
        string account = Utils.GetRequest("account", "all", 2, @"^[\d]+$", "请输入正确ID账号");

        try
        {
            if (account.Length == 32)
            {
                account = new BCW.BLL.Security().DecryptQueryString(account);
            }
            //if (account.Length != 11)
            //{
            //    account = new BCW.BLL.User().GetMobile(int.Parse(account));
            //}
        }
        catch { }
       // Utils.Error("account:" + account + " 长度:" + account.Length, "");
        if (new BCW.BLL.User().ExistsID(long.Parse(account)))                    //如果存在ID就是存在User表中
        {           
            MyMobile = new BCW.BLL.User().GetMobile(int.Parse(account));
            if (!(new BCW.BLL.tb_Question().Exists(int.Parse(account))))   //是第一次设置密码保护问题
            {
                //int id = new BCW.BLL.User().GetID(MyMobile);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("账号:" + account + "<br />");
                builder.Append("非常抱歉:你的账号没有设置密码保护问题，请先使用手机获取密码，登陆后再设置密码保护问题" + "<br />");
                builder.Append("<br /><a href=\"" + Utils.getPage("SetQuestion.aspx") + "\">返回密码保护设置（需要登录）&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else     //不是第一次设置密码保护问题，已经设计了
            {

                int count = new BCW.BLL.tb_Question().GetChangeCount(int.Parse(account));
                int nowtime = int.Parse(DateTime.Now.DayOfYear.ToString());// 获取第几天  系统的第几天，就是当前的天数（一年中的天数）             
                int lasttime = new BCW.BLL.tb_Question().GetLastChange(int.Parse(account));   //上一次修改密码的的天数    

                if (lasttime - nowtime < 0)//上一次修改时间和当前的系统时间不在同一天，就刷新一天的修改次数
                {
                    count = 0;
                    new BCW.BLL.tb_Question().UpdateChangeCount(count, int.Parse(account));//更新每天修改密码的次数
                    count = new BCW.BLL.tb_Question().GetChangeCount(int.Parse(account));
                    //   Utils.Error("count:" + count + " nowtime:" + nowtime + " lasttime" + lasttime, "");
                }
                if (count < 5)
                {
                    string Myquestion = new BCW.BLL.tb_Question().GetQuestion(int.Parse(account));

                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<b>你的问题是：" + Myquestion + "</b><br />");
                    builder.Append(Out.Tab("</div>", ""));
                    //MyMobile = new BCW.BLL.Security().EncryptQueryString(MyMobile);  //加密手机号码
                    strText = "你的问题答案是:/,输入验证码:/,,,";
                    strName = "Myanswer,code,hid,account,act";
                    strType = "text,text,hidden,hidden,hidden";
                    strValu = "''" + 1 + "'" + new BCW.BLL.Security().EncryptQueryString(account) + "'secondsave";
                    strEmpt = "false,false,false,false,false";
                    strIdea = "'<img src=\"Code.aspx\"/>'''|/";
                    strOthe = "确定提交|刷新验证码,GetPwd.aspx,post,0,red|blue";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append("<br /><a href=\"" + Utils.getPage("SetQuestion.aspx") + "\">返回密码保护设置（需要登录）&gt;&gt;<br/></a>");
                }
                else
                {
                    Utils.Success("温馨提示", "非常抱歉，一天之内不能使用忘记密码功能修改密码超过5次，请在次日再使用本功能！.", Utils.getUrl("GetPwd.aspx"), "5");
                }

            }

        }
        else            //不存手机号码，就是User表中不存在  
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>账号不存在，请重新输入" + "</b><br />");
            builder.Append(Out.Tab("</div>", ""));

            strText = "请输入你的ID:/,,";
            strName = "account,hid,act";
            strType = "text,hidden,hidden";
            strValu = "'" + 1 + "'forgetpsw";
            strEmpt = "false,false,false";
            strIdea = "/";
            strOthe = "确定提交,GetPwd.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 获取手机码
    /// </summary>
    private void vacode()
    {
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        //string mobile = Utils.GetRequest("mobile21", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string account = Utils.GetRequest("account", "post", 2, @"^\d+$", "请正确输入号码");
        {
            #region  msg
            //string mycode = "";
            //try
            //{
            //    mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();// 图形验证码
            //}
            //catch { }
            //string code = Utils.GetRequest("code", "post", 2, @"^[0-9]{4}$", "请输入验证码!");  //界面图形验证码      
            //if (!code.Equals(mycode))//验证码相等
            //{
            //    Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
            //}
            //if (!new BCW.BLL.User().Exists(new BCW.BLL.User().GetID(mobile)))
            //{
            //    Utils.Error("不存在该手机号码账号", "");
            //}
            if (!new BCW.BLL.User().Exists(int.Parse(account)))
            {
                Utils.Error("不存在该账号", "");
            }
            string mobile = new BCW.BLL.User().GetMobile(int.Parse(account));
            if (new BCW.BLL.tb_Validate().ExistsPhone(mobile, 5))//存在忘记密码验证码
            {
                BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 5);
                if (getmo.codeTime > DateTime.Now)//验证码存在切是新发的
                {
                    string dateDiff = null;
                    TimeSpan x = getmo.codeTime - DateTime.Now;
                    dateDiff = x.TotalSeconds.ToString();
                    Utils.Error("很抱歉,请在" + dateDiff.Split('.')[0] + "秒之后再次获取手机验证码", Utils.getUrl("GetPwd.aspx?act=forvalidate&amp;account=" + (account) + ""));
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
            //int tm = 2;//短信过期时间
            //int total = 15;//每天可以发的总短信量
            //int ipCount = 10;//没IP最大发送量
            //int phoneCount = 10;//每号码最大发送量
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
                    validate.type = 5;
                    string result = "";
                    Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();

                    result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
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
                        Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("GetPwd.aspx?act=forvalidate&amp;account=" + (account) + ""), "2");
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
                        validate.type = 5;
                        string result = "";
                        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                        result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
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
                            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("GetPwd.aspx?act=forvalidate&amp;account=" + (account) + ""), "2");
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
                validate.type = 5;
                string result = "";
                Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
                result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
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
                    Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("GetPwd.aspx?act=forvalidate&amp;account=" + (account) + ""), "2");
                }
            }
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 输入新密码
    /// </summary>
    private void validate()
    {
        string account = Utils.GetRequest("account", "post", 3, @"^\d+$", "");

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx?act=forvalidate&amp;account=" + account) + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        //    string mobile = Utils.GetRequest("mobile", "post", 1, "", "");

        string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
        string mycode = "";
        try
        {
            mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();// 图形验证码
        }
        catch { }
        string code = Utils.GetRequest("code", "post", 2, @"^[0-9]{4}$", "请正确输入验证码!");  //界面图形验证码      
        if (!code.Equals(mycode))//验证码相等
        {
            Utils.Error("很抱歉,您输入图形验证码不对，请刷新页面", "");
        }
        BCW.Model.tb_Validate getmo = null;
        string mobile = new BCW.BLL.User().GetMobile(int.Parse(account));
        //Utils.Error("mobile+" + mobile, "");
        if (new BCW.BLL.tb_Validate().ExistsPhone(mobile, 5))//存在修改密码验证码
        {
            getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 5);
            if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
            {
                if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
                {
                    Utils.Error("很抱歉,您输入手机验证码不对222", "");
                }
            }
            else { Utils.Error("手机验证码过期，请重新获取", Utils.getUrl("GetPwd.aspx?act=forvalidate")); }
        }
        else//没发送过修改密码验证码
        {
            Utils.Error("很抱歉,您输入手机验证码不对123", "");
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>注意： 这是你重新输入你的新密码，" + "</b><br/>");
        builder.Append(Out.Tab("</div>", ""));
        strText = "你输入的新密码:/,确认新密码/,,,";
        strName = "newpwd,checkpwd,account,hid,act";
        strType = "text,text,hidden,hidden,hidden";
        strValu = "''" + new BCW.BLL.Security().EncryptQueryString(account) + "'" + 1 + "'changetoDB";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "确定提交,GetPwd.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx?act=forvalidate&amp;account=" + account) + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void forvalidate()
    {

        Master.Title = "使用手机获取密码";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info1 = Utils.GetRequest("info1", "all", 1, "", "");
        string account = Utils.GetRequest("account", "all", 1, "", "");
        string flag = Utils.GetRequest("flag", "all", 1, "", "");
        if (info == "")
        {
            builder.Append("<style type=\"text/css\"> ");
            // builder.Append("div.panel {  display:none;}");
            builder.Append("</style>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            int meid = new BCW.User.Users().GetUsId();
            // if (account == "")
            {
                builder.Append("<div class=\"panel\" id =\"dialog_show\">");
                builder.Append("<form id=\"form1\" method=\"post\" action=\"GetPwd.aspx?act=vacode\">");
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"vacode\"/>");
                // builder.Append("<input type=\"hidden\" name=\"mobile21\"  id = \"mobile2111\" />&nbsp;");
                //     builder.Append("*请输入验证码:<br/><input type=\"text\" name=\"code\"   />&nbsp;");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                if (meid == 0)
                    builder.Append("*请输入账号ID获取验证码:<br/><input type=\"text\" name=\"account\"  id = \"mobile\" value=\"" + account + "\" />&nbsp;");
                else
                {
                    builder.Append("*请输入账号ID获取验证码:<br/><input type=\"text\" name=\"account\"  id = \"mobile\" value=\"" + meid + "\" />&nbsp;");
                }
                builder.Append("<input class=\"btn-blue\" id=\"huoqu\" type=\"submit\" value=\"获取验证码\"/>");
                builder.Append("</form>");
                builder.Append("</div>");
            }

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<form id=\"form2\" method=\"post\" action=\"GetPwd.aspx\">");
            //if (mobile1 != "")
            //{
            builder.Append("<input type=\"hidden\" name=\"account\"  id = \"account\" value=\"" + account + "\" />&nbsp;");
            //}

            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("*请输入手机验证码:<br/><input type=\"text\" name=\"phoneCode\" value=\"\" />&nbsp;<br/>");
            builder.Append("*请输入图形码:<br/><input type=\"text\" name=\"code\"/>&nbsp;");
            builder.Append("<img src=\"Code.aspx\"/><br/>");
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"validate\"/>");
            builder.Append("<br/><input class=\"btn-red\" type=\"submit\" value=\"提交确认\"/>");
            builder.Append("</form>");

            builder.Append(Out.Tab("</div>", ""));
            //string strText = "*请输入手机号码:/,,,";
            //string strName = "mobile,info,act,backurl";
            //string strType = "text,hidden,hidden,hidden";
            //string strValu = "'ok'forvalidate'" + Utils.getPage(0) + "";
            //string strEmpt = "false,false,false,false";
            //string strIdea = "'''|/";
            //string strOthe = "获取验证码,GetPwd.aspx,post,0,red";
            //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void Fangfa()    //获取密码的两种方法
    {
        Master.Title = "获取密码说明方式";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("注意：本功能可以帮助你重置登陆密码" + "<br />");
        builder.Append("在线帮助" + "<br />");
        builder.Append("方法1：手机找回密码" + "<br />");
        builder.Append("手机获取验证码:" + "<a href=\"" + Utils.getUrl("GetPwd.aspx?act=forvalidate&amp;ve=2a") + "\">马上找回</a>" + "<br />");
        builder.Append("方法2：密保保护" + "<br />");
        builder.Append("如果有设置过密保问题的请" + "<a href=\"" + Utils.getUrl("GetPwd.aspx?act=mibao") + "\">使用密码保护获取密码</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("其他相关帮助" + "<br />");
        int SizeNum = 3;
        string strWhere = "";
        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().GetHelps(SizeNum, strWhere);
        if (listHelp.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_Help n in listHelp)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("MoreQuestion.aspx?act=shuoming&amp;id=" + n.ID + "") + "\">" + n.Title + "</a>");

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<br/><a href=\"" + Utils.getUrl("MoreQuestion.aspx") + "\">&gt;&gt;更多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx?id=7") + "\">客服帮助</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void Mibaoshuoming()   //获取手机号码         222
    {
        Master.Title = "密保说明";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>=获取登陆密码=" + "</b><br />");
        builder.Append("注意：本功能可根据你的密码保护问题帮助你重新获取登陆密码，并且一天以内不能使用本功能超过5次" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        //strText = "你的手机号码或者你的ID/,,";
        strText = "请输入你的ID/,,";
        strName = "account,hid,act";
        strType = "text,hidden,hidden";
        if (meid == 0)
        {
            strValu = "'" + 2 + "'forgetpsw";
        }
        else
        {
            strValu =meid+ "'" + 2 + "'forgetpsw";
        }
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "确定提交,GetPwd.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append("<br/>注意：当忘记密码，或者账号被盗的时候，可以通过本功能重新获取密码" + "<br />");
        builder.Append("设置了密码保护之后，当你忘记密码的时候可以回答自己在设置的密码保护问题来获取到密码。!" + "<br />");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }


}
