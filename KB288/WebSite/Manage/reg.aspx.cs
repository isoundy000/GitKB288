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

public partial class Manage_reg : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                OkPage();
                break;
            case "RegMe":
                RegMePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    #region 批量注册
    private void RegMePage()
    {
        for (int i = 0; i < 1000; i++)
        {
            //取得会员ID
            int maxId = 0;

            maxId = GetRandId();
            if (maxId == 0)
            {
                continue;
            }

            //加密用户密码
            string strPwd = Utils.MD5Str("123abc998");
            //取随机识别串
            string UsKey = new Rand().RandNum(10);

            string newName = ub.GetSub("RegName", "/Controls/reg.xml");
            //if (newName == "")
            newName = "博艺会员";

            //写入注册表
            BCW.Model.User model = new BCW.Model.User();
            model.ID = maxId;
            model.Mobile = "13229780118";
            model.UsName = "" + newName + "" + maxId + "";
            model.UsPwd = strPwd;
            model.UsKey = UsKey;
            model.Photo = "/Files/Avatar/image0.gif";
            int sexn = new Random().Next(0, 1);
            model.Sex = sexn;
            int dn = new Random().Next(1, 20);
            model.RegTime = DateTime.Now.AddDays(-dn);
            model.RegIP = Utils.GetUsIP();
            model.EndTime = DateTime.Now.AddDays(-dn);
            model.Birth = DateTime.Parse("1980-1-1");
            model.Sign = "未设置签名";
            model.InviteNum = 0;
            model.Email = "";
            model.IsVerify = 1;
            new BCW.BLL.User().Add(model);
            //发送内线
            new BCW.BLL.Guest().Add(model.ID, model.UsName, ub.GetSub("RegGuest", "/Controls/reg.xml"));
            //积分操作
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, maxId);
            //设置keys
            string keys = string.Empty; ;
            keys = BCW.User.Users.SetUserKeys(maxId, strPwd, UsKey);
            builder.Append(i + ".注册成功:" + model.UsName + " 密码:123abc998<br />");
        }
    }
    #endregion

    #region 注册会员 OkPage()
    private void ReloadPage()
    {
        Master.Title = "注册会员";
        builder.Append(Out.Tab("<div class=\"title\">免费注册会员</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("手机号:");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",设定密码(6-20位):/,分配ID(填0随机分配):/,推荐ID(没有请填0):/,,,";
        string strName = "mobile,Pwd,Rid,rd,act,backurl";
        string strType = "text,text,num,num,hidden,hidden";
        string strValu = "''0'0'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定注册,reg.aspx,post,0,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />密码留空时则使用手机号后六位作密码");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 确认注册处理 OkPage
    private void OkPage()
    {
        Master.Title = "注册会员";
        string mobile = Utils.GetRequest("mobile", "post", 2, @"^[0-9]\d{10,10}$", "请正确输入十一位数的手机号码");
        string Pwd = Utils.GetRequest("Pwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
        int Rid = int.Parse(Utils.GetRequest("Rid", "post", 1, @"^[0-9]\d*$", "0"));
        int rd = int.Parse(Utils.GetRequest("rd", "post", 1, @"^[0-9]\d*$", "0"));
        if (Pwd == "")
            Pwd = Utils.Right(mobile, 6);

        if (new BCW.BLL.User().Exists(mobile))
        {
            Utils.Error("很抱歉,此手机号已经注册", "");
        }
        //取得会员ID
        int maxId = 0;
        if (Rid == 0)
        {
            //maxId = BCW.User.Reg.GetRandId();
            maxId = GetRandId();
            if (maxId == 0)
            {
                Utils.Error("很抱歉,服务器繁忙，请稍后注册..", "");
            }
        }
        else
        {
            if (new BCW.BLL.User().Exists(Rid))
            {
                Utils.Error("很抱歉,此ID已经注册", "");
            }
            maxId = Rid;
        }
        //加密用户密码
        string strPwd = Utils.MD5Str(Pwd.Trim());
        //取随机识别串
        string UsKey = new Rand().RandNum(10);

        string newName = ub.GetSub("RegName", "/Controls/reg.xml");
        if (newName == "")
            newName = "新会员";

        if (rd > 0)
        {
            if (!new BCW.BLL.User().Exists(rd))
            {
                Utils.Error("很抱歉,推荐ID不存在", "");
            }
        }

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
        model.InviteNum = rd;
        model.Email = "";
        model.IsVerify = 1;
        new BCW.BLL.User().Add(model);
        //发送内线
        new BCW.BLL.Guest().Add(model.ID, model.UsName, ub.GetSub("RegGuest", "/Controls/reg.xml"));
        //积分操作
        new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, maxId);
        if (rd > 0)
        {
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RecomUser, rd);
        }
        //设置keys
        string keys = string.Empty; ;
        keys = BCW.User.Users.SetUserKeys(maxId, strPwd, UsKey);
        builder.Append(Out.Tab("<div class=\"title\">注册会员</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("恭喜注册成功！<br />");
        builder.Append("注册ID:" + maxId + "<br />注册手机号:" + mobile + "<br />登录密码:" + Pwd.Trim() + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 生成ID GetRandId()
    public static int GetRandId()
    {
        int num2 = 0;
        string xmlPath = "/Controls/reg.xml";
        //起始ID
        int num3 = int.Parse(ub.GetSub("RegStartID", xmlPath));
        //保留ID
        string sub = ub.GetSub("RegKeepID", xmlPath);
        //随机分配或递增分配
        bool flag = !(ub.GetSub("RegTypes", xmlPath) == "0");
        int usCount = new BCW.BLL.User().GetCount();
        if (flag)
        {
            usCount = new BCW.BLL.User().GetCount();
            num2 = new BCW.BLL.User().GetMaxId();
        }
        else
        {
            int num6 = int.Parse(new Rand().RandNumer(7));
            num2 = num6 % 4000 - usCount / 10;
            num2 = Math.Abs(usCount + num2);
            if (num2 < num3) { num2 += num3; }//原num2 > num3

            while (num2 != 0)
            {
                if (("#" + sub + "#").IndexOf("#" + num2.ToString() + "#") == -1)
                {
                    if (!new BCW.BLL.User().Exists(num2))
                        break;
                }
                num2++;
            }
        }
        return num2;
    }
    #endregion

}