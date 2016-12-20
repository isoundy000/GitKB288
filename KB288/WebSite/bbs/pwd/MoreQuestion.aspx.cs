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

public partial class bbs_pwd_MoreQuestion : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "shuoming":
                Shuoming();
                break;
            default:
                ReloadPage();//忘记密码管理，就是一开始出现的页面
                break;
        }
    }
    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;    //第几页
        int recordCount  ;   //记录的总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//一页显示多少条数据
        string strWhere = string.Empty;
        if (id > 0)    
            strWhere += "id=" + id + "";

        string[] pageValUrl = { "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);   //跳到第几页
        if (pageIndex == 0)
            pageIndex = 1;

        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().Gettb_Helps(pageIndex, pageSize, strWhere, out recordCount);
        if (listHelp.Count > 0)
        {
            builder.Append(Out.Div("div", "相关的帮助.."+"<br/>"));

            int k = 1;
            foreach (BCW.Model.tb_Help n in listHelp)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
              //  builder.Append("<a href=\"" + Utils.getUrl("MoreQuestion.aspx?act=del&amp;id=" + ((pageIndex - 1) * pageSize + k) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                builder.Append(((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("MoreQuestion.aspx?act=shuoming&amp;id=" + n.ID + "") + "\">" + n.Title + "</a>");

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = "输入问题ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜问题ID,MoreQuestion.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append("<br />" + "<a href=\"" + Utils.getUrl("GetPwd.aspx") + "\">【返回上一级】</a><br />");
        builder.Append(Out.Tab("<div class=\"title\">",""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void Shuoming()
    {
        int id = Convert.ToInt32(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.tb_Help help = new BCW.BLL.tb_Help().Gettb_Help(id);

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>"+help.Title + "</b><br/>");
        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(help.Explain + "<br/>")));
       // builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB("使用绑定手机号拨打24小时客服手机:[c]15992310086[/c]请客服人员帮忙清空密码保护")));
        if (help.HasLink == 1)
        {

            builder.Append("<a href=\"" + Utils.getUrl(help.LinkName) + "\">请点击这里跳到链接</a>" + "<br />");
        }
        builder.Append(Out.Tab("</div>", ""));


       // builder.Append("<a href=\"" + Utils.getUrl("GetPwd.aspx") + "\">【返回上一级】</a><br />");
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
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        //  builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">客服帮助</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("MoreQuestion.aspx") + "\">&gt;&gt;更多</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/pwd/GetPwd.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
