using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BCW.Common;

public partial class Manage_MasterPage : BCW.Common.BaseMaster
{
    protected System.Text.StringBuilder FootResult = new System.Text.StringBuilder("");
    protected string _Title;
    protected bool _IsFoot = true;
    public override string Title
    {
        set { _Title = value; }
    }
    public override bool IsFoot
    {
        set { _IsFoot = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string AdminPath = ConfigHelper.GetConfigString("AdminPath");
        //后台管理员权限判断
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId == 0)
        {
            Response.Redirect("/" + AdminPath + "/login.aspx");
            Response.End();
        }
        string PageUrl = Utils.getPageUrl().ToLower();

        DataSet ds = new BCW.BLL.Manage().GetManageList("sTime,sUserIP", "ID=" + ManageId + "");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("/" + AdminPath + "/login.aspx");
            Response.End();
        }
        else
        {
            ////特殊功能限制
            //if (Utils.GetTopDomain().Contains("kb288.net"))
            //{
            //    if (PageUrl.Contains("/xml/") && !PageUrl.Contains("/xml/guess"))
            //    {
            //        if (!PageUrl.Contains("/xml/stkset.aspx") && !PageUrl.Contains("/xml/six49set.aspx"))
            //        {
            //            if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
            //            {
            //                Utils.Error("权限不足", "");
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (PageUrl.Contains("/xml/") && !PageUrl.Contains("/xml/guess"))
            //    {
            //        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            //        {
            //            if (ManageId != 1 && ManageId != 2 && ManageId != 4 && ManageId != 13)
            //            {
            //                Utils.Error("权限不足", "");
            //            }
            //        }
            //        else
            //        {
            //            if (ManageId != 1 && ManageId != 9 && ManageId != 11)
            //            {
            //                Utils.Error("权限不足", "");
            //            }
            //        }
            //    }
            //}
            //if (PageUrl.Contains("/guess/") || PageUrl.Contains("/guessbc/"))
            //{
            //    if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            //    {
            //        if (ManageId == 3 || ManageId == 12)
            //        {
            //            Utils.Error("权限不足", "");
            //        }
            //    }
            //}


            DateTime LoginTime = DateTime.Parse(ds.Tables[0].Rows[0]["sTime"].ToString());
            string LoginUserIP = ds.Tables[0].Rows[0]["sUserIP"].ToString();
            int lIp = Utils.ParseInt(ub.Get("SiteVerifyIP"));
            int lTime = Utils.ParseInt(ub.Get("SiteLoginExpir"));
            string UsIP = Utils.DelLastChar(Utils.GetUsIP(), ".");

            string LoginUserIP2 = "";
            try
            {
                LoginUserIP2 = Utils.DelLastChar(LoginUserIP, ".");
            }
            catch { }

            if ((lTime > 0 && LoginTime.AddMinutes(lTime) < DateTime.Now) || (lIp > 0 && LoginUserIP2 != UsIP))
            {

                UsIP = Utils.DelLastChar(UsIP, ".");

                if (UsIP != "121.14" && UsIP != "119.147" && UsIP != "182.16" && UsIP != "119.42")
                {
                    new Out().head(Utils.ForWordType(_Title));
                    Response.Write(Out.Tab("<div class=\"title\">", ""));
                    if ((lTime > 0 && LoginTime.AddMinutes(lTime) < DateTime.Now))
                        Response.Write("登录超时，请重新登录");
                    else
                        Response.Write("您目前使用的网络IP与上次有明显不同(上次IP:" + LoginUserIP + ")，请重新登录");

                    Response.Write(Out.Tab("</div>", ""));
                    string strText = "用户,密码";
                    string strName = "userName,userPass";
                    string strType = "text,password";
                    string strValu = "''";
                    string strEmpt = "false,false";
                    string strIdea = "/";
                    string strOthe = "登录后台|reset,/" + AdminPath + "/login.aspx,post,1,red|blue";
                    Response.Write(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    Response.Write(new Out().foot());
                    Response.End();
                }
            }
        }

 
        //二级管理限制
        string ManIDS = "#" + ub.Get("SiteManIDS") + "#";
        if (ManIDS.Contains("#" + ManageId + "#"))
        {
            bool IsPass = false;
            string strUrl = "login.aspx,default.aspx,/man/,class.aspx,PK10set.aspx,classact.aspx,classok.aspx,book.aspx,forum.aspx,blacklist.aspx,moderator.aspx,mebook.aspx,thread.aspx,reply.aspx,group.aspx,chat.aspx";
            string[] Temp = strUrl.Split(",".ToCharArray());
            for (int i = 0; i < Temp.Length; i++)
            {
                if (PageUrl.Contains("" + Temp[i] + ""))
                {
                    IsPass = true;
                    break;
                }
            }
            if (!IsPass)
            {
                Utils.Error("权限不足", "");            
            }
        }


        string foot = string.Empty;
        if (_IsFoot == true)
        {
            foot += Out.Tab("<div class=\"ft\">", "");
            foot += "<a href=\"" + Utils.getUrl("/" + AdminPath + "/inter.aspx?backurl=" + Utils.PostPage(1) + "") + "\">[功能]</a>";
            foot += "<a href=\"" + Utils.getUrl("/default.aspx") + "\">返回首页</a><br />";
            foot += Out.Tab("</div>", "");
        }
        foot += "<!--Powered by kubao.cc " + ub.Get("SiteVersion") + " @author Light-->";
        //头部
        new Out().head(Utils.ForWordType("" + ub.Get("SiteName") + "-" + _Title));
        //尾部
        FootResult.Append(Utils.ForWordType(foot) + new Out().foot());
    }
}