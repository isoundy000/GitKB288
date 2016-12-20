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
using BCW.Common;

public partial class vbook : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/vbook.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("VBookStatus", xmlPath) == "1")
        {
            Utils.Safe("留言系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "ok":
                OkPage();
                break;
            case "view":
                ViewPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("VBookName", xmlPath);
        if (ub.GetSub("VBookLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("VBookLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">" + ub.GetSub("VBookName", xmlPath) + "</div>", ""));
        }
        if (ub.GetSub("VBookNotes", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("" + ub.GetSub("VBookNotes", xmlPath) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=0";

        // 开始读取列表
        IList<BCW.Model.Vbook> listVbook = new BCW.BLL.Vbook().GetVbooks(pageIndex, pageSize, strWhere, out recordCount);
        if (listVbook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Vbook n in listVbook)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                string sFace = string.Empty;
                if (!string.IsNullOrEmpty(n.VPwd))
                    sFace = "*";
                if (string.IsNullOrEmpty(n.ReText) && ub.GetSub("VBookIsAc", xmlPath) == "1")
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".留言处理中，请稍后..");
                }
                else
                {
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("vbook.aspx?act=view&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}{3}</a>", (pageIndex - 1) * pageSize + k, n.ID, sFace, n.Title);
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?act=add&amp;backurl=" + Utils.getPage(0) + "") + "\">发表留言</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage()
    {
        Master.Title = "发表留言";
        //会员发表权限
        int meid = new BCW.User.Users().GetUsId();
        if (ub.GetSub("VBookIsUser", xmlPath) == "1")
        {
            if (meid == 0)
                Utils.Login();
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("欢迎您发表留言");
        builder.Append(Out.Tab("</div>", ""));
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;

        if (meid == 0)
        {
            sText = "您的昵称:/,";
            sName = "UsName,";
            sType = "text,";
            sValu = "'";
            sEmpt = "false,";
        }
        string strText = "" + sText + "留言标题:/,留言内容:/,保密内容:(管理员可见)/,查看密码:(留空不加密)/,,";
        string strName = "" + sName + "Title,Content,SyText,VPwd,act,backurl";
        string strType = "" + sType + "text,textarea,text,text,hidden,hidden";
        string strValu = "" + sValu + "''''ok'" + Utils.getPage(0) + "";
        string strEmpt = "" + sEmpt + "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "发表留言|reset,vbook.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    private void OkPage()
    {
        //会员发表权限
        int meid = new BCW.User.Users().GetUsId();
        if (ub.GetSub("VBookIsUser", xmlPath) == "1")
        {
            if (meid == 0)
                Utils.Login();
        }
        string mename=string.Empty;
        if (meid == 0)
        {
            mename = Utils.GetRequest("UsName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,7}$", "请输入不超过7个字的昵称，不能有特殊符号");
        }
        else
        {
            mename = new BCW.BLL.User().GetUsName(meid);
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,30}$", "请输入1-30字的标题");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{0,500}$", "请输入不超过500字的内容");
        string SyText = Utils.GetRequest("SyText", "post", 3, @"^[\s\S]{0,200}$", "请输入不超过200字的保密内容，可以留空");
        string VPwd = Utils.GetRequest("VPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "查看密码由6-20位数字或字母组成，可以留空");

        //是否刷屏
        string appName = "LIGHT_VBOOK";
        int Expir = Convert.ToInt32(ub.GetSub("VBookExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        BCW.Model.Vbook model = new BCW.Model.Vbook();
        model.Types = 0;
        model.Title = Title;
        model.Content = Content;
        model.SyText = SyText;
        model.Face = 0;
        model.UsID = meid;
        model.UsName = mename;
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;
        model.VPwd = VPwd;
        new BCW.BLL.Vbook().Add(model);
        Utils.Success("发表留言", "发表留言成功，正在返回..", Utils.getUrl("vbook.aspx?backurl=" + Utils.getPage(0) + ""), "2");

    }

    private void ViewPage()
    {
        Master.Title = "查看留言";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Vbook().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Vbook model = new BCW.BLL.Vbook().GetVbook(id);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.Title);
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));

        //----------------密码访问开始
        string pwd = Utils.GetRequest("pwd", "post", 1, "", "");
        if (!string.IsNullOrEmpty(model.VPwd) && pwd != model.VPwd)
        {
            new Out().head(Utils.ForWordType("温馨提示"));
            Response.Write(Out.Tab("<div class=\"title\">", ""));
            Response.Write("本留言内容已加密");
            Response.Write(Out.Tab("</div>", ""));
            string strText = "输入密码:/,,,";
            string strName = "pwd,id,act,backurl";
            string strType = "password,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'view'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认查看,vbook.aspx,post,1,red";

            Response.Write(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            Response.Write(Out.Tab("<div class=\"ft\">", ""));
            Response.Write(" <a href=\"" + Utils.getPage("vbook.aspx") + "\">取消</a>");
            Response.Write(Out.Tab("</div>", "<br />"));
            Response.Write(new Out().foot());
            Response.End();
        }

        //----------------密码访问结束
        //if (model.Face != 0)
            //builder.Append("<img src=\"Files/face/" + model.Face + ".gif\" alt=\"load\"/>");

        builder.Append(model.Content);
        builder.Append("<br />作者:"+model.UsName);
        builder.Append("<br />时间:" + model.AddTime);
        if (!string.IsNullOrEmpty(model.ReText))
        {
            builder.Append("<br />~~~~~~");
            builder.Append("<br />" + model.ReName + "回复:" + model.ReText);
            builder.Append("<br />回复时间:" + model.ReTime);
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("vbook.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
