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

public partial class MasterPage : BCW.Common.BaseMaster
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected System.Text.StringBuilder builder2 = new System.Text.StringBuilder("");
    protected System.Text.StringBuilder builder3 = new System.Text.StringBuilder("");
    protected string _Title;
    protected bool _IsFoot = true;
    protected int _Refresh = 0;
    protected string _Gourl;
    public override string Title
    {
        set { _Title = value; }
    }
    public override bool IsFoot
    {
        set { _IsFoot = value; }
    }
    public override int Refresh
    {
        set { _Refresh = value; }
    }
    public override string Gourl
    {
        set { _Gourl = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = BCW.User.Users.userId();
        if (meid > 0)
        {
            meid = new BCW.User.Users().GetUsId();
        }
        BCW.User.Master.ShowMaster(meid, _Title);

        //底部
        builder.Append(Out.Tab("<div class=\"ft\">", "<br />"));
        if (Utils.PostPage(0).ToLower() == "/default.aspx")
        {
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.Get("SiteIndexUbb")).Replace("[@功能]", Utils.Function(_IsFoot))));
        }
        else
        {
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.Get("SiteWapUbb")).Replace("[@功能]", Utils.Function(_IsFoot))));
        }
        builder.Append(Out.Tab("</div>", ""));

        //版本
        builder.Append("<!--Powered by kubao.cc " + ub.Get("SiteVersion") + " @author Light-->");

        string strAlign = ub.Get("SiteAlign");
        if (meid > 0)
        {

            //短消息提示
            if (string.IsNullOrEmpty(Request.Form.ToString()))
            {

                int smsCount = new BCW.BLL.Guest().GetCount(meid);
                int smsXCount = new BCW.BLL.Guest().GetXCount(meid);
                if (smsCount > 0 || smsXCount > 0)
                {
                    string actUrl = string.Empty;
                    string actUrl2 = string.Empty;
                    if (smsCount > 0)
                        actUrl = "act=newlist&amp;";

                    if (smsXCount > 0)
                        actUrl2 = "act=view&amp;hid=0&amp;";
                    else
                        actUrl2 = "ptype=1&amp;";

                    string GuestText = "<b>新内线<a href=\"" + Utils.getUrl("/bbs/guest.aspx?" + actUrl + "backurl=" + Utils.PostPage(1) + "") + "\">(" + smsCount + ")</a>系统<a href=\"" + Utils.getUrl("/bbs/guest.aspx?" + actUrl2 + "backurl=" + Utils.PostPage(1) + "") + "\">(" + smsXCount + ")</a></b>";

                    builder2.Append(Out.Tab("<div>", ""));
                    builder2.Append(GuestText);
                    builder2.Append(Out.Tab("</div>", "<br />"));
                }
            }
        }
        //头部
        if (_Refresh == 0)
        {
            new Out().head(Utils.ForWordType(_Title), strAlign);
        }
        else
        {
            new Out().head(Utils.ForWordType(_Title), _Gourl, _Refresh.ToString(), strAlign);
        }
        //顶部Ubb
        string SiteTopUbb = string.Empty;
        if (ub.Get("SiteIndexTopUbb") != "")
        {
            SiteTopUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(ub.Get("SiteIndexTopUbb")));
            if (SiteTopUbb.IndexOf("</div>") == -1)
            {
                builder3.Append(Out.Tab("<div>", ""));
                builder3.Append(SiteTopUbb);
                builder3.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder3.Append(SiteTopUbb);
            }
        }

        //消息提示
        Response.Write(Utils.ForWordType(builder2.ToString()));
        //顶部Ubb
        Response.Write(Utils.ForWordType(builder3.ToString()));
        //尾部
        builder.Append(new Out().foot());

    }
}