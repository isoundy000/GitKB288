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
/// 陈志基，修改问题需要短信 20160910
/// </summary>
public partial class bbs_pwd_SetQuestion : System.Web.UI.Page
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
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "addsave":
                SavePage();
                break;
            case "secondsave":
                SecondSave();
                break;
            case "designquestion":
                DesignQuestion();
                break;
            case "design2":
                Design2();
                break;
            case "savepageagain":
                SavePageAgain();
                break;
            case "forPhone":
                forPhone();
                break;
            case "formibao":
                Formibao();
                break;

            default:
                ReloadPage();//忘记密码管理，就是一开始出现的页面
                break;
        }
    }

    private void ReloadPage()   //初始界面
    {

        Master.Title = "我的密码保护问题管理";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Mymobile = new BCW.BLL.User().GetMobile(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        bool isExist = new BCW.BLL.tb_Question().Exists(meid);
        if (!isExist)     //如果Question表中没有改号码的数据,还没有设置密码保护问题
        {
            #region  设置密码保护问
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>注意</b>：你没有设置你的密码保护问题，现在请设置你的问题保护问题，下次获取密码时需要回答问题答案，请牢记！！！！！" + "<br />");
            builder.Append("<b>★可选问题方式（自定义问题）:</b>");
            //    builder.Append("<b>★:<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=designquestion") + "\">自定义问题</a></b>"); 
            builder.Append(Out.Tab("</div>", ""));
            strText = ",";
            strName = "design:/,act";
            strType = "hideen,hidden";
            strValu = 1 + "'designquestion";
            strEmpt = "false,false";
            strIdea = "/";
            strOthe = "自定义问题,SetQuestion.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //

            strText = "首次设置密保需要验证手机号码:/,请选择密码保护问题:/,问题答案/,,";
            strName = "Mymobile,Myquestion,Myanswer,hid,act";
            strType = "text,select,text,hidden,hidden";
            strValu = "'''" + 1 + "'addsave";
            strEmpt = "false,你就读第一所学校的名字?|你就读第一所学校的名字?|你最喜欢的电影名称是什么?|你最喜欢的电影名称是什么?|你最喜欢的书是什么?|你最喜欢的书是什么?|你最喜欢的歌曲名称是什么?|你最喜欢的歌曲名称是什么?|你最喜欢的食物是什么?|你最喜欢的食物是什么?,false,false,false";
            strIdea = "/";
            strOthe = "提交,SetQuestion.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("<b>注意：当忘记密码，或者账号被盗的时候，可以通过本功能重新获取密码" + "</b><br />");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else        //Question表中拥有号码的数据，已经设置了密码保护问题
        {
            string Myquestion = new BCW.BLL.tb_Question().GetQuestion(meid);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("修改密码保护问题时需要先回答你的问题答案" + "<br />");
            builder.Append("<b>你的问题是：" + Myquestion + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            BCW.Model.tb_Validate getmo = null;
            if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 3))//修改密保验证码
            {
                getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 3);
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
                    builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            strText = "你的问题答案是:/,*请输入手机验证码:/,输入验证码:/,,";
            strName = "Myanswer,phoneCode,code,hid,act";
            strType = "text,text,text,hidden,hidden";
            strValu = "'''" + 1 + "'secondsave";
            strEmpt = "false,false,false,false,false";
            strIdea = "'<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=secondsave&amp;info=ok&amp;ac=ok") + "\">获取手机验证码</a>'<img src=\"Code.aspx\"/>''|/";
            strOthe = "确定提交|刷新验证码,SetQuestion.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=formibao&amp;ve=2a") + "\">【忘记密保】</a><br/>");
        #region 
        int SizeNum = 3;
        string strWhere = "";
        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().GetHelps(SizeNum, strWhere);
        if (listHelp.Count > 0)
        {
            builder.Append(Out.Div("div", "相关帮助.." + "<br/>"));

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
        #endregion
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<br/><a href=\"" + Utils.getUrl("MoreQuestion.aspx") + "\">&gt;&gt;更多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx?id=7") + "\">客服帮助</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));


    }

    private void SecondSave()    //验证问题答案，修改问题
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string MyMobile = new BCW.BLL.User().GetMobile(meid);

        string answer = Utils.GetRequest("Myanswer", "post", 1, @"^[\s\S]{1,20}$", "");  //获取输入的答案
        string Myanser = new BCW.BLL.tb_Question().GetAnswer(meid);
        string code = Utils.GetRequest("code", "post", 1, @"^[0-9]{4}$", "");  //
        //string code = Utils.GetRequest("code", "post", 0, "", "");  //获取输入的答案
        string mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        if (Utils.ToSChinese(ac).Contains("刷新验证码"))    //判断哪一个按键
        {
            #region
            Master.Title = "我的密码保护问题管理";
            string Myquestion = new BCW.BLL.tb_Question().GetQuestion(meid);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            // builder.Append("<b>验证码错误</b>");
            builder.Append("修改密码保护问题时需要先回答你的问题答案" + "<br />");
            builder.Append("<b>你的问题是：" + Myquestion + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Model.tb_Validate getmo = null;
            if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 3))//修改密保验证码
            {
                getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 3);
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
            strText = "你的问题答案是:/,*请输入手机验证码:/,输入验证码:/,,";
            strName = "Myanswer,phoneCode,code,hid,act";
            strType = "text,text,text,hidden,hidden";
            strValu = "'''" + 1 + "'secondsave";
            strEmpt = "false,false,false,false,false";
            strIdea = "'<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=secondsave&amp;info=ok&amp;ac=ok") + "\">获取手机验证码</a>'<img src=\"Code.aspx\"/>''|/";
            strOthe = "确定提交|刷新验证码,SetQuestion.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=formibao") + "\">【忘记密保】</a><br/>");
            #region
            int SizeNum = 3;
            string strWhere = "";
            IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().GetHelps(SizeNum, strWhere);
            if (listHelp.Count > 0)
            {
                builder.Append(Out.Div("div", "相关帮助.." + "<br/>"));

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
            #endregion
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<br/><a href=\"" + Utils.getUrl("MoreQuestion.aspx") + "\">&gt;&gt;更多</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx?id=7") + "\">客服帮助</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else if (Utils.ToSChinese(ac).Contains("获取手机验证码") || ac == "ok" || ac == "ok1")    //判断哪一个按键
        {
            #region 获取手机验证码
            //if (ac == "ok")
            //{
            //    if (!code.Equals(mycode))//验证码相等
            //    {
            //     //   Utils.Error("很抱歉,您输入图形验证按不对，请刷新2页面", "");
            //    }
            //}
            if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 3))//存在修改密保验证码
            {
                BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 3);
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
                    validate.type = 3;
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
                        if (ac == "ok1")
                        {
                            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("SetQuestion.aspx?act=formibao&amp;mobile=" + mobile), "2");
                        }
                        else {
                            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("SetQuestion.aspx"), "2");
                        }

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
                        validate.type = 3;
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
                            if (ac == "ok1")
                            {
                                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("SetQuestion.aspx?act=formibao&amp;mobile=" + mobile), "2");
                            }
                            else
                            {
                                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("SetQuestion.aspx"), "2");
                            }
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
                validate.type = 3;
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
                    if (ac == "ok1")
                    {
                        Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("SetQuestion.aspx?act=formibao&amp;mobile=" + mobile), "2");
                    }
                    else
                    {
                        Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("SetQuestion.aspx"), "2");
                    }
                }
            }
            #endregion
        }
        else   //确定提交
        {
            #region 确定提交
            if (!code.Equals(mycode))//验证码相等
            {
                Utils.Error("输入的图形验证码错误，请刷新..", "");
            }
            if (Utils.ToSChinese(ac).Contains("确定提交"))
            {
                if (!Myanser.Equals(answer))    //答案相等
                {
                    Utils.Error("输入的问题答案错误，请重新输入.", "");
                }
            }      
            string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
            BCW.Model.tb_Validate getmo = null;
            string mobile = new BCW.BLL.User().GetMobile(meid);
            if (new BCW.BLL.tb_Validate().ExistsPhone(mobile, 3))//存在修改密保验证码
            {
                getmo = new BCW.BLL.tb_Validate().Gettb_Validate(mobile, 3);
                if (getmo.codeTime > DateTime.Now)//验证码存在且是新发的
                {
                    if (!phoneCode.Equals(getmo.mesCode))//验证码不相等
                    {
                        Utils.Error("很抱歉,您输入手机验证码不对222", "");
                    }
                }
                else { Utils.Error("手机验证码过期，请重新获取", ""); }
            }
            else//没发送过修改密保验证码
            {
                Utils.Error("很抱歉,您输入手机验证码不对123", "");
            }

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>★可选问题方式（自定义问题）:</b>");
            // builder.Append("<b>★:<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=secondsave&amp;type1=" + 1) + "\">自定义问题</a></b>");
            builder.Append(Out.Tab("</div>", ""));
            // builder.Append(Out.Tab("</div>", ""));
            strText = ",";
            strName = "type1:/,act";
            strType = "hideen,hidden";
            strValu = 1 + "'design2";
            strEmpt = "false,false";
            strIdea = "/";
            strOthe = "自定义问题,SetQuestion.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //
            strText = "选择问题:/,问题答案/,,";
            strName = "Myquestion,Myanswer,hid,act";
            strType = "select,text,hidden,hidden";
            strValu = "''" + 1 + "'savepageagain";
            strEmpt = "你就读第一所学校的名字?|你就读第一所学校的名字?|你最喜欢的电影名称是什么?|你最喜欢的电影名称是什么?|你最喜欢的书是什么?|你最喜欢的书是什么?|你最喜欢的歌曲名称是什么?|你最喜欢的歌曲名称是什么?|你最喜欢的食物是什么?|你最喜欢的食物是什么?,false,false,false";
            strIdea = "/";
            strOthe = "提交,SetQuestion.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void forPhone()
    {

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>★可选问题方式（自定义问题）:</b>");
        // builder.Append("<b>★:<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=secondsave&amp;type1=" + 1) + "\">自定义问题</a></b>");
        builder.Append(Out.Tab("</div>", ""));
        // builder.Append(Out.Tab("</div>", ""));
        strText = ",";
        strName = "type1:/,act";
        strType = "hideen,hidden";
        strValu = 1 + "'design2";
        strEmpt = "false,false";
        strIdea = "/";
        strOthe = "自定义问题,SetQuestion.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        strText = "选择问题:/,问题答案/,,";
        strName = "Myquestion,Myanswer,hid,act";
        strType = "select,text,hidden,hidden";
        strValu = "''" + 1 + "'savepageagain";
        strEmpt = "你就读第一所学校的名字?|你就读第一所学校的名字?|你最喜欢的电影名称是什么?|你最喜欢的电影名称是什么?|你最喜欢的书是什么?|你最喜欢的书是什么?|你最喜欢的歌曲名称是什么?|你最喜欢的歌曲名称是什么?|你最喜欢的食物是什么?|你最喜欢的食物是什么?,false,false,false";
        strIdea = "/";
        strOthe = "提交,SetQuestion.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));



        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void Formibao()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        string mobile1 = Utils.GetRequest("mobile", "all", 1, "", "");
        builder.Append("<b>使用手机获取验证码设置密码保护" + "</b>");
        //   builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB("使用绑定手机号拨打24小时客服手机:[c]15992310086[/c]请客服人员帮忙清空密码保护")));
        builder.Append("<style type=\"text/css\"> ");
        //builder.Append("div.panel {  display:none;}");
        builder.Append("</style>");
        //  builder.Append(Out.Tab("<div class=\"panel\"id=\"dialog_show\"> ", ""));
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        BCW.Model.tb_Validate getmo = null;
        if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 3))//修改密保验证码
        {
            getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 3);
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
                builder.Append("请在<b style=\"color:red\">" + Reg + "</b>秒后再次获取手机验证码");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append("<form id=\"form2\" method=\"post\" action=\"SetQuestion.aspx\">");
        //builder.Append("<input type=\"hidden\" name=\"mobile\"  id = \"mobile\" value=\"" + mobile1 + "\" />");
        //builder.Append("<input  type=\"button\" id =\"mobile11\" value=\"点击获取\"/><br/>");
        builder.Append("*请输入手机验证码:<br/><input type=\"text\" name=\"phoneCode\" value=\"\" />&nbsp;");
        //< a href =\"" + Utils.getUrl("SetQuestion.aspx?act=secondsave&amp;info=ok&amp;ac=ok")
        builder.Append("<a href=\"" + Utils.getUrl("SetQuestion.aspx?act=secondsave&amp;ac=ok1") + "\">获取验证码</a>");
        builder.Append("<br/>*请输入图形码:<br/><input type=\"text\" name=\"code\"   />&nbsp;");
        builder.Append("<img src=\"Code.aspx\"/><br/>");
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"secondsave\"/>");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append("<br/><input class=\"btn-red\" type=\"submit\" value=\"提交确认\"/>");
        builder.Append("</form>");
        builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("SetQuestion.aspx") + "\">【返回上一级】</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        int SizeNum = 3;
        string strWhere = "";
        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().GetHelps(SizeNum, strWhere);
        if (listHelp.Count > 0)
        {
            builder.Append(Out.Div("div", "其他相关帮助.." + "<br/>"));

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
        builder.Append("<br/><a href=\"" + Utils.getUrl("MoreQuestion.aspx") + "\">&gt;&gt;更多</a><br />");
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void Design2()             // （ 不用验证手机）自定义问题   
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>请认真填写你的问题和答案!</b>");
        builder.Append(Out.Tab("</div>", ""));
        strText = "自定义的问题:/,问题答案/,,";
        strName = "Myquestion,Myanswer,hid,act";
        strType = "text,text,hidden,hidden";
        strValu = "''" + 1 + "'savepageagain";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "提交,SetQuestion.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void DesignQuestion()   //自己定义的问题
    {

        Master.Title = "我的密码保护问题管理";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string design = Utils.GetRequest("design", "post", 1, "", "");
        string Mymobile = new BCW.BLL.User().GetMobile(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("<b>注意：你没有设置你的密码保护问题，现在请设置你的问题保护问题，下次获取密码时需要回答问题答案，请牢记！！！！！" + "</b><br />");
        builder.Append(Out.Tab("</div>", ""));
        strText = "首次设置密保需要验证手机号码:/,自定义的问题:/,问题答案/,,";
        strName = "Mymobile,Myquestion,Myanswer,hid,act";
        strType = "text,text,text,hidden,hidden";
        strValu = "'''" + 1 + "'addsave";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "提交,SetQuestion.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<br/><b>注意：当忘记密码，或者账号被盗的时候，可以通过本功能重新获取密码" + "</b><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("<br/>" + "<a href=\"" + Utils.getUrl("GetPwd.aspx") + "\">【返回上一级】</a><br />");
        int SizeNum = 3;
        string strWhere = "";
        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().GetHelps(SizeNum, strWhere);
        if (listHelp.Count > 0)
        {
            builder.Append(Out.Div("div", "相关帮助.." + "<br/>"));

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
        builder.Append("<br/><a href=\"" + Utils.getUrl("MoreQuestion.aspx") + "\">&gt;&gt;更多</a><br />");
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavePageAgain()     //修改密码保护问题，再次保存到数据库
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Mypmobile = new BCW.BLL.User().GetMobile(meid);
        Master.Title = "密保管理问题成功";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("密码保护设置" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        string Myquestion = Utils.GetRequest("Myquestion", "post", 2, @"^[\s\S]{1,20}$", "请输入问题！");
        string Myanswer = Utils.GetRequest("Myanswer", "post", 2, @"^[\s\S]{1,20}$", "请输入问题答案");
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("  恭喜！你的密码保护设置成功" + "<br/>");
        builder.Append("你的问题是:" + Myquestion + "<br />");
        builder.Append("你的答案是:" + Myanswer + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.tb_Question model = new BCW.Model.tb_Question();


        model.Mobile = Mypmobile;
        model.Question = Myquestion;
        model.Answer = Myanswer;
        model.state = 1;
        model.ID = meid;
        model.lastchange = new BCW.BLL.tb_Question().GetLastChange(meid);
        model.changecount = new BCW.BLL.tb_Question().GetChangeCount(meid);
        model.code = string.Empty;
        new BCW.BLL.tb_Question().Update(model);   //更新一条数据进入数据库         
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请您牢记答案.当您忘记密码或ID被盗,可使用本答案取回密码!" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("5秒后自动跳转到主页<br />");
        Master.Refresh = 5;//5秒后跳转到以下地址(可缺省)
        Master.Gourl = Utils.getUrl("GetPwd.aspx");//跳到的地址(可缺省)
        builder.Append("<a href=\"" + Utils.getUrl("SetQuestion.aspx") + "\">如果还没有跳转，请点击该链接返回主页</a>");

    }

    private void SavePage()   //第一次设置密码保护问题保存到数据库的
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Mypmobile = new BCW.BLL.User().GetMobile(meid);

        string mobile = Utils.GetRequest("Mymobile", "post", 2, @"^[\d]+$", "请输入正确的手机号码");
        string Myanswer = Utils.GetRequest("Myanswer", "post", 2, @"^[\s\S]+$", "请输入问题答案");

        if (Mypmobile.Equals(mobile))
        {
            Master.Title = "密保管理问题成功";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("密码保护设置" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            string Myquestion = Utils.GetRequest("Myquestion", "post", 2, @"^[\s\S]{1,50}$", "请输入问题");
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("  恭喜！你的密码保护设置成功" + "<br/>");
            builder.Append("你的问题是:" + Myquestion + "<br />");
            //}

            //  string Myanswer = Utils.GetRequest("Myanswer", "post", 2, @"^[\s\S]{1,20}$", "问题答案限1-20字");
            builder.Append("你的答案是:" + Myanswer + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Model.tb_Question model = new BCW.Model.tb_Question();


            model.Mobile = Mypmobile;
            model.Question = Myquestion;
            model.Answer = Myanswer;
            model.state = 1;
            model.ID = meid;
            model.lastchange = 0;
            model.changecount = 0;
            model.code = string.Empty;
            new BCW.BLL.tb_Question().Add(model);   //添加一条数据进入数据库        
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("5秒后自动跳转到主页<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请您牢记答案.当您忘记密码或ID被盗,可使用本答案取回密码!" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            Master.Refresh = 5;//5秒后跳转到以下地址(可缺省)
            Master.Gourl = Utils.getUrl("SetQuestion.aspx");//跳到的地址(可缺省)
            builder.Append("<a href=\"" + Utils.getUrl("SetQuestion.aspx") + "\">如果还没有跳转，请点击该链接返回主页</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            Utils.Success("温馨提示", "抱歉，验证手机号码失败，请重新输入..", Utils.getUrl("SetQuestion.aspx"), "3");

        }
    }

}
