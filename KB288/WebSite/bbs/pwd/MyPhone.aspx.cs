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
/// 陈志基，修改手机需要短信 20160910
/// </summary>
public partial class bbs_pwd_MyPhone : System.Web.UI.Page
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
            case "reset":
                ReSet();
                break;
            case "addsave":
                addSave();
                break;
            case"phone":
                phone();
                break;
            case "getphone":
                getphone();
                break;
            default:
                ReloadPage();//忘记密码管理，就是一开始出现的页面
                break;
        }
    }

    private void ReloadPage()    //初始界面
    {
        Master.Title = "我的手机号码";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("../myedit.aspx") + "\">上级</a>-");
        builder.Append("设置手机号码：");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string Mymobile = new BCW.BLL.User().GetMobile(meid);
        string showphone = Mymobile.Substring(0, 3) + "******" + Mymobile.Substring(9, 2);
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("你的手机号码是：" + showphone);
        // builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append("<br/><a href=\"" + Utils.getUrl("MyPhone.aspx?act=reset") + "\">【修改手机号码】</a><br />");
        //  builder.Append(Out.Tab("<div class=\"text\">",""));


     //   builder.Append("<br/><a href=\"" + Utils.getUrl("MyPhone.aspx?act=reset") + "\">【客服帮助】</a><br />");


        builder.Append("<b>注:防止手机号泄漏隐藏部分数字.</b>" + "<br/>");
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
                builder.Append("<a href=\"" + Utils.getUrl("MoreQuestion.aspx?act=shuoming&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(0)) + "\">" + n.Title + "</a>");

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
        builder.Append("<a href=\"" + Utils.getPage("../myedit.aspx") + "\">上级</a>-");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void addSave()    //保存数据库
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Mymobile = new BCW.BLL.User().GetMobile(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string code = Utils.GetRequest("code", "post", 1, @"^[0-9]+", "");  //获取输入的验证码
        string mycode = Request.Cookies["validateCookie"].Values["ChkCode"].ToString();
        if (Utils.ToSChinese(ac).Contains("确定提交"))      //判断哪一个按键
        {
            #region 确定提交
            if (code.Equals(mycode))
            {
                string answer = Utils.GetRequest("MyAnswer", "post", 2, @"^[\s\S]{1,30}$", "请输入问题答案");//输入的答案
                string Myanser = new BCW.BLL.tb_Question().GetAnswer(meid);   //数据库里面的问题答案
                string oldPhone = Utils.GetRequest("oldPhone", "post", 2, @"^[\d]{11}$", "请输入正确的原手机号码");//输入的旧的手机号码
                string MyoldPhone = new BCW.BLL.User().GetMobile(meid);  //数据库里面的旧手机号码
                string newPhone = Utils.GetRequest("newPhone", "post", 2, @"^[\d]{11}$", "请输入正确的新手机号码");
                string newPhone1 = Utils.GetRequest("newPhone1", "post", 2, @"^[\d]{11}$", "请输入正确的新手机号码");
                //string phoneCode = Utils.GetRequest("phoneCode", "post", 2, @"^[0-9]{4}$", "请输入正确的四位手机验证码");
                //BCW.Model.tb_Validate getmo = null;
                //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 4))//存在修改手机验证码
                //{
                //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 4);
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
                if (Myanser.Equals(answer))             //答案正确
                {
                    if (MyoldPhone.Equals(oldPhone))           //旧手机输入正确
                    {
                        if (new BCW.BLL.User().Exists(newPhone))        //新手机绑定了其他的账号了，就是数据库中有该号码，需要重新输入
                        {
                            //builder.Append("新手机设置过了其他的账号了     " + "<br/>");
                            Utils.Success("温馨提示", "新手机设置过了其他的账号了，请重新输入，3秒后跳转输入界面..", Utils.getUrl("MyPhone.aspx?act=reset"), "3");
                        }
                        else                                    //新手机可以绑定了，请牢记
                        {
                            if (newPhone.Equals(newPhone1))   //两次输入的新手机正确
                            {
                                new BCW.BLL.User().UpdateMobile(meid, newPhone);
                                new BCW.BLL.tb_Question().UpdateMobile(meid, newPhone);
                                builder.Append(Out.Tab("<div class=\"text\">", ""));
                                builder.Append("恭喜，你已经设置好了你的新手机了，请牢记：  ！！   " + "<br/>");
                                builder.Append("你的新手机号码是： " + newPhone + "");
                                builder.Append(Out.Tab("</div>", ""));
                                builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
                                builder.Append("4秒后自动跳转到主页<br />");
                                builder.Append(Out.Tab("</div>", ""));
                                Master.Refresh = 4;//5秒后跳转到以下地址(可缺省)
                                Master.Gourl = Utils.getUrl("GetPwd.aspx");//跳到的地址(可缺省)
                                builder.Append("<a href=\"" + Utils.getUrl("MyPhone.aspx") + "\">如果还没有跳转，请点击该链接返回主页</a>");

                            }
                            else
                            {

                                Utils.Success("温馨提示", "输入的两次新手机号码不一致，请重新输入，3秒后跳转输入界面..", Utils.getUrl("MyPhone.aspx?act=reset"), "3");
                            }
                        }
                    }
                    else                                          //旧手机输入错误，请重新输入
                    {

                        Utils.Success("温馨提示", "旧手机输入错误，请重新输入，3秒后跳转输入界面..", Utils.getUrl("MyPhone.aspx?act=reset"), "3");
                    }
                }
                else                               //答案输入错误，请重新输入
                {

                    Utils.Success("温馨提示", "问题答案错误，请重新输入，3秒后跳转输入界面..", Utils.getUrl("MyPhone.aspx?act=reset"), "3");
                }
            }
            else
            {
                Utils.Success("温馨提示", "验证码错误，请重新输入，3秒后跳转输入界面..", Utils.getUrl("MyPhone.aspx?act=reset"), "3");
            }
            #endregion
        }
        else if (Utils.ToSChinese(ac).Contains("刷新验证码"))    //判断哪一个按键
        {
            #region 刷新验证码
            string answer = Utils.GetRequest("MyAnswer", "post", 0, "", "");//输入的答案
            string oldPhone = Utils.GetRequest("oldPhone", "post", 0, "", "");//输入的旧的手机号码
            string newPhone = Utils.GetRequest("newPhone", "post", 0, "", "");
            string newPhone1 = Utils.GetRequest("newPhone1", "post", 0, "", "");
            Master.Title = "修改我的手机号码";
            bool account = new BCW.BLL.tb_Question().Exists(meid);
            string MyQuestion = new BCW.BLL.tb_Question().GetQuestion(meid);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>请回答以下的问题：" + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            if (account)    //设置了密码保护问题
            {
                //BCW.Model.tb_Validate getmo = null;
                //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 4))//修改密码验证码
                //{
                //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 4);
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
                //strText = MyQuestion + ":/,原手机号码:/,新手机号码/,再次输入你的新手机号码/,*请输入手机验证码:/,输入验证码:/,,";
                //strName = "MyAnswer,oldPhone,newPhone,newPhone1,phoneCode,code,act";
                //strType = "text,text,text,text,text,text,hidden";
                //strValu = "''''''addsave";
                //strEmpt = "false,false,false,false,false,false,false";
                //strIdea = "'<a href=\"" + Utils.getUrl("MyPhone.aspx?act=getphone") + "\">忘记手机号？</a>'''<a href=\"" + Utils.getUrl("MyPhone.aspx?act=addsave&amp;ac=ok") + "\">获取手机验证码</a>'<img src=\"Code.aspx\"/>'|/";
                strText = MyQuestion + ":/,原手机号码:/,新手机号码/,再次输入你的新手机号码/,输入验证码:/,,";
                strName = "MyAnswer,oldPhone,newPhone,newPhone1,code,act";
                strType = "text,text,text,text,text,hidden";
                strValu = "'''''addsave";
                strEmpt = "false,false,false,false,false,false,false";
                strIdea = "'<a href=\"" + Utils.getUrl("MyPhone.aspx?act=getphone") + "\">忘记手机号？</a>'''<img src=\"Code.aspx\"/>'|/";
                strOthe = "确定提交|刷新验证码,MyPhone.aspx,post,0,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else    //没有设置密码保护问题的
            {
                builder.Append("你的手机号码没有设置密码保护问题，请先设置密码保护问题后，再修改手机号码：     " + "<br/>");
                builder.Append("<a href=\"" + Utils.getUrl("SetQuestion.aspx") + "\">&gt;&gt;设置密码保护问题</a><br />");
            }
            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx") + "\">上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        //else//获取修改手机验证码
        //{
        //    #region 修改手机验证码
        //    if (!code.Equals(mycode))//验证码相等
        //    {
        //       // Utils.Error("很抱歉,您输入图形验证按不对，请刷新页面", "");
        //    }
        //    if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 4))//存在修改密保验证码
        //    {
        //        BCW.Model.tb_Validate getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 4);
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
        //            validate.type = 4;
        //            string result = "";
        //            Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //            result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
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
        //                Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("MyPhone.aspx?act=reset"), "2");
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
        //                validate.type = 4;
        //                string result = "";
        //                Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //                result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
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
        //                    Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("MyPhone.aspx?act=reset"), "2");
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
        //        validate.type = 4;
        //        string result = "";
        //        Mesege.Soap57ProviderService MesExt = new Mesege.Soap57ProviderService();
        //        result = MesExt.Submit("000379", "123456", "1069032239089369", "【" + ub.GetSub("SiteName", "/Controls/wap.xml") + "】亲，您的验证码是:" + mesCode, mobile);
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
        //            Utils.Success("获取手机验证码", "正在发送手机验证码，请查收", Utils.getUrl("MyPhone.aspx?act=reset"), "2");
        //        }
        //    }
        //    #endregion
        //}

    }
    private void phone()
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
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx?act=reset") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        if (Utils.ToSChinese(ac).Contains("刷新验证码"))    //判断哪一个按键
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("查询手机号码时需要先回答你的问题答案" + "<br />");
            builder.Append("密保问题:" + new BCW.BLL.tb_Question().GetQuestion(meid));
            builder.Append(Out.Tab("</div>", ""));
            strText = "你的问题答案是:/,输入验证码:/,,,";
            strName = "Myanswer,code,info,hid,act";
            strType = "text,text,hidden,hidden,hidden";
            strValu = "''ok'" + 1 + "'phone";
            strEmpt = "false,false,false,false,false";
            strIdea = "'<img src=\"Code.aspx\"/>'''|/";
            strOthe = "确定提交|刷新验证码,MyPhone.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx?act=reset") + "\">上级</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            if (code.Equals(mycode))//验证码相等
            {
                if (Myanser.Equals(answer))    //答案相等
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("你的手机号码是：" +MyMobile+ "<br />");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                    builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx?act=reset") + "\">上级</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }
                else
                {
                    Utils.Success("温馨提示", "输入的问题答案错误，请重新输入..", Utils.getUrl("MyPhone.aspx?act=getphone"), "3");
                }
            }
            else
            {
                Utils.Success("温馨提示", "输入的验证码错误，请重新输入..", Utils.getUrl("MyPhone.aspx?act=getphone"), "3");
            }
        }
    }
    private void getphone()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string phone = string.Empty;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx?act=reset") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        phone = new BCW.BLL.User().GetMobile(meid);
        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("你的手机号码是" + phone+"<br />");
        //builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查询手机号码时需要先回答你的问题答案" + "<br />");
        builder.Append("密保问题:" + new BCW.BLL.tb_Question().GetQuestion(meid));
        builder.Append(Out.Tab("</div>", ""));
        strText = "你的问题答案是:/,输入验证码:/,,,";
        strName = "Myanswer,code,info,hid,act";
        strType = "text,text,hidden,hidden,hidden";
        strValu = "''ok'" + 1 + "'phone";
        strEmpt = "false,false,false,false,false";
        strIdea = "'<img src=\"Code.aspx\"/>'''|/";
        strOthe = "确定提交|刷新验证码,MyPhone.aspx,post,0,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx?act=reset") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

    }
    private void ReSet()    //修改手机号码
    {
        Master.Title = "修改我的手机号码";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string Mymobile = new BCW.BLL.User().GetMobile(meid);
        bool account = new BCW.BLL.tb_Question().Exists(meid);
        string MyQuestion = new BCW.BLL.tb_Question().GetQuestion(meid);
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>请回答以下的问题：" + "</b>");
        builder.Append(Out.Tab("</div>", ""));
        if (account)    //设置了密码保护问题
        {
            //BCW.Model.tb_Validate getmo = null;
            //if (new BCW.BLL.tb_Validate().ExistsPhone(new BCW.BLL.User().GetMobile(meid), 4))//修改密码验证码
            //{
            //    getmo = new BCW.BLL.tb_Validate().Gettb_Validate(new BCW.BLL.User().GetMobile(meid), 4);
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
            //strText = MyQuestion + ":/,原手机号码:/,新手机号码/,再次输入你的新手机号码/,*请输入手机验证码:/,输入验证码:/,,";
            //strName = "MyAnswer,oldPhone,newPhone,newPhone1,phoneCode,code,act";
            //strType = "text,text,text,text,text,text,hidden";
            //strValu = "''''''addsave";
            //strEmpt = "false,false,false,false,false,false,false";
            //strIdea = "'<a href=\"" + Utils.getUrl("MyPhone.aspx?act=getphone") + "\">忘记手机号？</a>'''<a href=\"" + Utils.getUrl("MyPhone.aspx?act=addsave&amp;ac=ok") + "\">获取手机验证码</a>'<img src=\"Code.aspx\"/>'|/";
            strText = MyQuestion + ":/,原手机号码:/,新手机号码/,再次输入你的新手机号码/,输入验证码:/,,";
            strName = "MyAnswer,oldPhone,newPhone,newPhone1,code,act";
            strType = "text,text,text,text,text,hidden";
            strValu = "'''''addsave";
            strEmpt = "false,false,false,false,false,false";
            strIdea = "'<a href=\"" + Utils.getUrl("MyPhone.aspx?act=getphone") + "\">忘记手机号？</a>'''<img src=\"Code.aspx\"/>'|/";
            strOthe = "确定提交|刷新验证码,MyPhone.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else    //没有设置密码保护问题的
        {
            builder.Append("你的手机号码没有设置密码保护问题，请先设置密码保护问题后，再修改手机号码：     " + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("SetQuestion.aspx") + "\">&gt;&gt;设置密码保护问题</a><br />");
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/MyPhone.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
