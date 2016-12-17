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

public partial class Manage_vbook : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "save":
                SavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "ok":
                OkPage();
                break;
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "管理网友留言";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理留言");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
        {
            builder.Append("等待回复|");
            builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">已回复</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">等待回复</a>");
            builder.Append("|已回复");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype" , "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += "Types=0";
        if (ptype == 0)
            strWhere += " AND (ReText IS NULL OR ReText='')";
        else
            strWhere += " AND ReText<>''";

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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("vbook.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>{1}{2}.<a href=\"" + Utils.getUrl("vbook.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", n.ID, sFace, (pageIndex - 1) * pageSize + k, n.Title);
                builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?act=add") + "\">添加留言</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    {
        Master.Title = "添加留言";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加留言");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "用户ID(非会员填0):/,昵称(填写ID后可不填):/,标题:/,内容:/,保密内容:(管理员可见)/,查看密码:(留空不加密)/,回复昵称:/,回复内容:/,回复时间:/,,";
        string strName = "UsID,UsName,Title,Content,SyText,VPwd,ReName,ReText,ReTime,act,backurl";
        string strType = "num,text,text,textarea,text,text,text,text,date,hidden,hidden";
        string strValu = "0''''''''" +DT.FormatDate(DateTime.Now,0) + "'save'" + Utils.getPage(0) + "";
        string strEmpt = "false,true,false,false,true,true,true,true,false,false,false";
        string strIdea = "/";
        string strOthe = "添加留言|reset,vbook.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SavePage()
    {
        string UsName = string.Empty;
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[0-9]\d*$", "用户ID填写错误，非会员请填写0"));
        if (UsID > 0)
        {
            UsName = Utils.GetRequest("UsName", "post", 3, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,7}$", "请输入不超过7个字的昵称，不能有特殊符号");
            if (UsName == "")
            {
                UsName = new BCW.BLL.User().GetUsName(UsID);
                if (UsName == "")
                    Utils.Error("用户ID不存在", "");
            }
        }
        else
        {
            UsName = Utils.GetRequest("UsName", "post", 1, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,7}$", "请输入不超过7个字的昵称，不能有特殊符号");
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{2,20}$", "请输入2-20位的标题");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{0,500}$", "请输入不超过500字的内容");
        string SyText = Utils.GetRequest("SyText", "post", 3, @"^[\s\S]{0,200}$", "请输入不超过200字的保密内容，可以留空");
        string ReName = Utils.GetRequest("ReName", "post", 1, @"^[\s\S]{1,20}$", "管理员");
        string VPwd = Utils.GetRequest("VPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "查看密码由6-20位数字或字母组成，可以留空");
        string ReText = Utils.GetRequest("ReText", "post", 3, @"^[\s\S]{0,200}$", "请输入不超过200字的回复内容，可以留空");
        DateTime ReTime = Utils.ParseTime(Utils.GetRequest("ReTime", "post", 1, DT.RegexTime, DateTime.Now.ToString()));

        BCW.Model.Vbook model = new BCW.Model.Vbook();
        model.Types = 0;
        model.UsName = UsName;
        model.UsID = UsID;
        model.Title = Title;
        model.Content = Content;
        model.SyText = SyText;
        model.Face = 0;
        model.VPwd = VPwd;
        model.ReName = ReName;
        model.ReText = ReText;
        model.ReTime = ReTime;
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;
        new BCW.BLL.Vbook().Add2(model);
        Utils.Success("添加留言", "添加留言成功，正在返回..", Utils.getPage("vbook.aspx"), "2");

    }

    private void EditPage()
    {
        Master.Title = "管理留言";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Vbook().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Vbook model = new BCW.BLL.Vbook().GetVbook(id);
        //回复时间
        DateTime reTime = DateTime.Now;
        if (!string.IsNullOrEmpty(model.ReTime.ToString()))
            reTime = Convert.ToDateTime(model.ReTime);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理" + model.Title);
        builder.Append(Out.Tab("</div>", ""));
        string strText = "您的昵称:/,标题:/,内容:/,保密内容:(管理员可见)/,查看密码:(留空不加密)/,回复昵称:/,回复内容:/,回复时间:/,,,";
        string strName = "UsName,Title,Content,SyText,VPwd,ReName,ReText,ReTime,id,act,backurl";
        string strType = "text,text,textarea,text,text,text,text,date,hidden,hidden,hidden";
        string strValu = "" + model.UsName + "'" + model.Title + "'" + model.Content + "'" + model.SyText + "'" + model.VPwd + "'" + model.ReName + "'" + model.ReText + "'" + reTime + "'" + id + "'ok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,true,true,false,false,false";
        string strIdea = "/";
        string strOthe = "管理留言|reset,vbook.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void OkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Vbook().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,7}$", "请输入不超过7个字的昵称，不能有特殊符号");
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{2,20}$", "请输入2-20位的标题");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{0,500}$", "请输入不超过500字的内容");
        string SyText = Utils.GetRequest("SyText", "post", 3, @"^[\s\S]{0,200}$", "请输入不超过200字的保密内容，可以留空");
        string ReName = Utils.GetRequest("ReName", "post", 1, @"^[\s\S]{1,20}$", "管理员");
        string VPwd = Utils.GetRequest("VPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "查看密码由6-20位数字或字母组成，可以留空");
        string ReText = Utils.GetRequest("ReText", "post", 3, @"^[\s\S]{0,200}$", "请输入不超过200字的回复内容，可以留空");
        DateTime ReTime = Utils.ParseTime(Utils.GetRequest("ReTime", "post", 1, DT.RegexTime, DateTime.Now.ToString()));

        BCW.Model.Vbook model = new BCW.Model.Vbook();
        model.ID = id;
        model.Types = 0;
        model.UsName = UsName;
        model.Title = Title;
        model.Content = Content;
        model.SyText = SyText;
        model.Face = 0;
        model.VPwd = VPwd;
        model.ReName = ReName;
        model.ReText = ReText;
        model.ReTime = ReTime;
        new BCW.BLL.Vbook().Update(model);
        Utils.Success("管理留言", "管理留言成功，正在返回..", Utils.getPage("vbook.aspx"), "2");

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
        //if (model.Face != 0)
            //builder.Append("<img src=\"/Files/face/" + model.Face + ".gif\" alt=\"load\"/>");

        builder.Append(model.Content);
        builder.Append("<br />作者:" + model.UsName);
        if (model.UsID != 0)
            builder.Append("(ID:" + model.UsID + ")");

        builder.Append("<br />提交IP:" + model.AddUsIP);
        builder.Append("<br />时间:" + model.AddTime);
        builder.Append("<br />查看密码:" + model.VPwd);
        builder.Append("<br />保密内容:" + model.SyText);
        if (!string.IsNullOrEmpty(model.ReText))
        {
            builder.Append("<br />~~~~~~");
            builder.Append("<br />" + model.ReName + "回复:" + model.ReText);
            builder.Append("<br />回复时间:" + model.ReTime);
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("vbook.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除留言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此留言记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("vbook.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Vbook().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Vbook().Delete(id);
            Utils.Success("删除留言", "删除留言成功..", Utils.getPage("vbook.aspx"), "1");
        }
    }
}

