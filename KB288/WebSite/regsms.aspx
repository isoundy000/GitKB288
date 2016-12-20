<%@ Page Language="C#" %>
<%@ Import NameSpace="BCW.Common" %>

<%
    //〓〓〓燃点WAPCMS短信API〓〓〓
    //==相关变量==
    //--Mobile手机号
    //--Content短信内容
    //--backResult返回 1 为成功, 0 为失败。
    //--backResultInfo返回你的业务处理说明，如：注册成功、密码更新成功
    //〓〓方法〓〓
    //1)注册<成功返回true/失败返回false>
    //mobile手机号/pwd密码明文(至少6位)/rd推荐会员ID(没有写0)
    //BCW.User.Reg.RegSms(mobile, pwd, rd);


    //2)找回登录密码<成功返回true/失败返回false>(登录密码重置为手机后6位)
    //mobile手机号
    //BCW.User.Reg.GetPwdSms(mobile);

    //3)找回支付密码<成功返回true/失败返回false>(支付密码重置为手机后6位)
    //mobile手机号
    //BCW.User.Reg.GetPledSms(mobile);

    //4)判断数据库是否存在该手机号<存在返回true/不存在返回false>(可用来判断找回密码依据)
    //new BCW.BLL.User().Exists(mobile)

    //5)判断数据库是否存在该会员ID<存在返回true/不存在返回false>(可用来判断推荐ID是否存在)
    //new BCW.BLL.User().Exists(ID)


    string mobile = Request.QueryString["Mobile"];
    string content = Request.QueryString["Content"];
    string xmlPath = "/Controls/reg.xml";
    if (string.IsNullOrEmpty(Request.ServerVariables["HTTP_Checkgo"]) || Request.ServerVariables["HTTP_Checkgo"] != Request.QueryString["go"])
    {
        Response.Write("非法访问！");
        Response.End();
    }

    int backResult = 0;
    string backResultInfo = "";

    if (ub.GetSub("RegLeibie", xmlPath) == "1" || ub.GetSub("RegLeibie", xmlPath) == "3")
    {
        backResult = 0;
        backResultInfo = "sms is close";
    }
    
    if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(content))
    {
        //是否有#指令
        content = content.ToUpper().Trim();
        if (content.Contains("#") && Utils.IsRegex(content, @"^[a-zA-Z\d]{1,20}#[a-zA-Z\d]{1,20}$"))
        {
            content = content.Split("#".ToCharArray())[1];
        }

        //业务开始:
        if (content == "PAY")//找回支付密码
        {
            if (BCW.User.Reg.GetPledSms(mobile))
            {
                backResult = 1;
                backResultInfo = "get paypwd ok";
            }
        }
        //业务开始:
        else if (content == "YZ")//验证会员
        {
            if (BCW.User.Reg.GetVerifySms(mobile))
            {
                backResult = 1;
                backResultInfo = "get yz ok";
            }
        }
        else if (Utils.IsRegex(content, @"^[0-9]\d*$") && new BCW.BLL.User().Exists(Utils.ParseInt(content)))//推荐ID注册
        {
            if (!new BCW.BLL.User().Exists(mobile))//正常注册
            {
                if (BCW.User.Reg.RegSms(mobile, Utils.Right(mobile, 6), Utils.ParseInt(content)))
                {
                    backResult = 1;
                    backResultInfo = "reg ok|tj ID" + content + "";
                }
            }
            else
            {

                if (BCW.User.Reg.GetPwdSms(mobile))//找回登录密码
                {
                    backResult = 1;
                    backResultInfo = "get loginpwd ok";
                }
            }
        }
        else
        {
            if (!new BCW.BLL.User().Exists(mobile))//正常注册
            {
                if (BCW.User.Reg.RegSms(mobile, Utils.Right(mobile, 6), 0))
                {
                    backResult = 1;
                    backResultInfo = "reg ok";
                }
            }
            else
            {
                if (BCW.User.Reg.GetPwdSms(mobile))//找回登录密码
                {
                    backResult = 1;
                    backResultInfo = "get loginpwd ok";
                }
            }
        }
    }
    Response.Write("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
    Response.Write("<mob version=\"1.0\">");
    Response.Write("<result>" + backResult + "</result>");
    Response.Write("<resultInfo>" + backResultInfo + "</resultInfo>");
    Response.Write("</mob>");
 %>
