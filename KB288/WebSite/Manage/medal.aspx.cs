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

public partial class Manage_medal : System.Web.UI.Page
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
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "me":
                MePage();
                break;
            case "grant":
                GrantPage();
                break;
            case "grantsave":
                GrantSavePage();
                break;
            case "remove":
                RemovePage();
                break;
            case "medallog":
                MedalLogPage();
                break;
            case "delmedallog":
                DelMedalLogPage();
                break;
            default:
                ReloadPage(act);
                break;
        }
    }

    private void ReloadPage(string act)
    {
        Master.Title = "勋章管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("勋章管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("系统勋章|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=" + act + "&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">系统勋章</a>|");

        if (ptype == 1)
            builder.Append("自定义｜");
        else
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=" + act + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">自定义</a>|");


        if (ptype == 2)
            builder.Append("论坛");
        else
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=" + act + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">论坛</a>");


        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=" + ptype + "";

        // 开始读取列表
        IList<BCW.Model.Medal> listMedal = new BCW.BLL.Medal().GetMedals(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedal.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medal n in listMedal)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                if (ptype == 1)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                else
                    builder.Append("" + n.ID + ".");

                builder.AppendFormat("{0}<img src=\"{1}\" alt=\"load\"/>", n.Title, n.ImageUrl);
                if (act == "pick")
                {
                    string gUrl = Server.UrlDecode(Utils.getPage(0));
                    gUrl = gUrl.Replace("&amp;", "&");
                    gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}xz=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
                    gUrl = Out.UBB(gUrl);
                    builder.Append("<a href=\"" + Utils.getUrl(gUrl + "&amp;xz=" + n.ID + "") + "\">[选择]</a>");
                }
                builder.AppendFormat("<br />{0}", n.Notes);
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
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=add") + "\">添加勋章</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=medallog") + "\">查看荣誉日志</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        Master.Title = "编辑勋章";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "勋章ID错误"));
        BCW.Model.Medal model = new BCW.BLL.Medal().GetMedal(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑勋章");
        builder.Append(Out.Tab("</div>", ""));
        string sText = string.Empty;
        string sName = string.Empty;
        string sTypes = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        if (model.Types > 0)
        {
            sText = "价格(" + ub.Get("SiteBz") + "):/,库存数量:/,勋章有效期(天):/,";
            sName = "iCent,iCount,iDay,";
            sTypes = "num,num,num,";
            sValu = "" + model.iCent + "'" + model.iCount + "'" + model.iDay + "'";
            sEmpt = "false,false,false,";
        }
        string strText = "勋章名称:/,勋章图片:/,勋章说明:/,勋章类型(选择系统勋章时，以下填写0):/,勋章所属论坛ID:/,排序:/," + sText + ",,";
        string strName = "Title,ImageUrl,Notes,Types,ForumId,Paixu," + sName + "id,act,backurl";
        string strType = "text,text,textarea,select,num,num," + sTypes + "hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.ImageUrl + "'" + model.Notes + "'" + model.Types + "'" + model.ForumId + "'" + model.Paixu + "'" + sValu + "" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,0|系统勋章|1|自定勋章|2|论坛勋章,false," + sEmpt + "false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,medal.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("medal.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此勋章</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx") + "\">勋章管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "勋章ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,10}$", "勋章名称限10字");
        string ImageUrl = Utils.GetRequest("ImageUrl", "post", 2, @"^[\s\S]{1,100}$", "图片地址限100字符内");
        string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,100}$", "勋章说明限1-100字");
        int ForumId = int.Parse(Utils.GetRequest("ForumId", "post", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int iCent = int.Parse(Utils.GetRequest("iCent", "post", 1, @"^[0-9]\d*$", "0"));
        int iCount = int.Parse(Utils.GetRequest("iCount", "post", 1, @"^[0-9]\d*$", "0"));
        int iDay = int.Parse(Utils.GetRequest("iDay", "post", 1, @"^[0-9]\d*$", "0"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));

        BCW.Model.Medal model = new BCW.Model.Medal();
        model.ID = id;
        model.Title = Title;
        model.ImageUrl = ImageUrl;
        model.Notes = Notes;
        model.iCent = iCent;
        model.iCount = iCount;
        model.iDay = iDay;
        model.Paixu = Paixu;
        model.ForumId = ForumId;
        new BCW.BLL.Medal().Update(model);

        Utils.Success("编辑勋章", "编辑勋章成功..", Utils.getUrl("medal.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }


    private void AddPage()
    {
        Master.Title = "添加勋章";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加勋章");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "勋章名称:/,勋章图片:/,勋章说明:/,勋章类型(选择系统勋章时，以下填写0):/,勋章所属论坛ID:/,价格(" + ub.Get("SiteBz") + "):/,库存数量:/,勋章有效期(天):/,排序:/,";
        string strName = "Title,ImageUrl,Notes,Types,ForumId,iCent,iCount,iDay,Paixu,act";
        string strType = "text,text,textarea,select,num,num,num,num,num,hidden";
        string strValu = "'''1'0'''''addsave";
        string strEmpt = "false,false,false,0|系统勋章|1|自定勋章|2|论坛勋章,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定添加,medal.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />添加的勋章将显示在前台,会员可以通过购买使用.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx") + "\">勋章管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,10}$", "勋章名称限10字");
        string ImageUrl = Utils.GetRequest("ImageUrl", "post", 2, @"^[\s\S]{1,100}$", "图片地址限100字符内");
        string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,100}$", "勋章说明限1-100字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]\d*$", "勋章类型选择错误"));
        int ForumId = int.Parse(Utils.GetRequest("ForumId", "post", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int iCent = int.Parse(Utils.GetRequest("iCent", "post", 1, @"^[0-9]\d*$", "0"));
        int iCount = int.Parse(Utils.GetRequest("iCount", "post", 1, @"^[0-9]\d*$", "0"));
        int iDay = int.Parse(Utils.GetRequest("iDay", "post", 1, @"^[0-9]\d*$", "0"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));

        BCW.Model.Medal model = new BCW.Model.Medal();
        model.Types = Types;
        model.Title = Title;
        model.ImageUrl = ImageUrl;
        model.Notes = Notes;
        model.iCent = iCent;
        model.iCount = iCount;
        model.iDay = iDay;
        model.Paixu = Paixu;
        model.ForumId = ForumId;
        new BCW.BLL.Medal().Add(model);

        Utils.Success("添加勋章", "添加勋章成功..", Utils.getUrl("medal.aspx?ptype=1"), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (id <= 22)
        {
            Utils.Error("系统勋章不可删除", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除勋章";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此勋章记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Medal().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Medal().Delete(id);
            Utils.Success("删除勋章", "删除勋章成功..", Utils.getUrl("medal.aspx"), "1");
        }
    }

    private void MePage()
    {
        Master.Title = "查看会员勋章";

        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + new BCW.BLL.User().GetUsName(hid) + "的勋章");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "hid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "PayID LIKE '%#" + hid + "#%' and Types>=0";

        // 开始读取列表
        IList<BCW.Model.Medal> listMedal = new BCW.BLL.Medal().GetMedals(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedal.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medal n in listMedal)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=remove&amp;id=" + n.ID + "&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤]</a>" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.AppendFormat("{0}<img src=\"{1}\" alt=\"load\"/>", n.Title, n.ImageUrl);
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
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=medallog&amp;hid=" + hid + "") + "\">查看日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=grant&amp;hid=" + hid + "") + "\">授予勋章</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=view&amp;uid=" + hid + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void MedalLogPage()
    {
        Master.Title = "获授勋章日志";

        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (hid > 0)
            builder.Append("" + new BCW.BLL.User().GetUsName(hid) + "的荣誉记录");
        else
            builder.Append("所有荣誉记录");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = {"act", "hid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=0";
        if (hid > 0)
            strWhere += " and UsID=" + hid + "";

        // 开始读取列表
        IList<BCW.Model.Medalget> listMedalget = new BCW.BLL.Medalget().GetMedalgets(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedalget.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medalget n in listMedalget)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=delmedallog&amp;id=" + n.ID + "&amp;hid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>" + ((pageIndex - 1) * pageSize + k) + "." + DT.FormatDate(n.AddTime, 11) + " ");
                if (hid == 0)
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>");

                builder.Append("获授<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=medalview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.SysUBB(n.Notes) + "</a>");

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
        if (hid == 0)
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx") + "\">返回上一级</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?act=me&amp;hid=" + hid + "") + "\">返回上一级</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelMedalLogPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除勋章日志";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此勋章日志记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?info=ok&amp;act=delmedallog&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("medal.aspx?act=medallog&amp;hid=" + hid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Medalget().Exists(hid, id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Medalget().Delete(hid, id);
            Utils.Success("删除勋章日志", "删除勋章日志成功..", Utils.getPage("medal.aspx?act=medallog&amp;hid=" + hid + ""), "1");
        }
    }

    private void GrantPage()
    {
        Master.Title = "授予勋章";
        string xz = Utils.GetRequest("xz", "get", 1, @"^[1-9]\d*$", "");
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("授予《" + new BCW.BLL.User().GetUsName(hid) + "》勋章");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "输入勋章编号:/,勋章有效天数(填0则无限):/,,";
        string strName = "ID,iDay,hid,act";
        string strType = "num,num,hidden,hidden";
        string strValu = "" + xz + "'0'" + hid + "'grantsave";
        string strEmpt = "false,false,false,false";
        string strIdea = "<a href=\"" + Utils.getUrl("medal.aspx?act=pick&amp;backurl=" + Utils.PostPage(1) + "") + "\">选择勋章<／a>'''|/";
        string strOthe = "确定授予,medal.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + hid + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    
    private void GrantSavePage()
    {
        int ID = int.Parse(Utils.GetRequest("ID", "post", 2, @"^[1-9]\d*$", "勋章编号填写错误"));
        int iDay = int.Parse(Utils.GetRequest("iDay", "post", 2, @"^[0-9]\d*$", "勋章有效天数填写错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.Medal().Exists(ID))
        {
            Utils.Error("不存在的勋章记录", "");
        }
        if (new BCW.BLL.Medal().Exists(ID, hid))
        {
            Utils.Error("此会员已存在这个勋章，如要修改期限请先撤销再授予", "");
        }

        BCW.Model.Medal model = new BCW.BLL.Medal().GetMedal(ID);
        string PayID = model.PayID + "#" + hid + "#";
        string ExTime = string.Empty;
        if (iDay == 0)
            ExTime = "1990-1-1";
        else
            ExTime = DT.FormatDate(DateTime.Now.AddDays(iDay), 11);

        string PayExTime = model.PayExTime + "#" + ExTime + "#";
        new BCW.BLL.Medal().UpdatePayID(ID, PayID, PayExTime);
        //清缓存
        string CacheKey = CacheName.App_UserMedal(hid);
        DataCache.RemoveByPattern(CacheKey);

        //记录获授勋章日志
        string DayTime = "" + iDay + "天";
        if (iDay == 0)
            DayTime = "永久";

        BCW.Model.Medalget m = new BCW.Model.Medalget();
        m.Types = 0;
        m.UsID = hid;
        m.MedalId = ID;
        m.Notes = "" + model.Title + "[IMG]" + model.ImageUrl + "[/IMG][有效期" + DayTime + "]";
        m.AddTime = DateTime.Now;
        new BCW.BLL.Medalget().Add(m);


        Utils.Success("授予勋章", "授予勋章并记录授日志成功..", Utils.getUrl("medal.aspx?act=me&amp;hid=" + hid + "&amp;ptype=1"), "1");
    }

    private void RemovePage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (info == "")
        {
            Master.Title = "撤销记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定撤销此记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("medal.aspx?info=ok&amp;act=remove&amp;id=" + id + "&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤销</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("medal.aspx?act=me&amp;hid=" + hid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Medal().Exists(id, hid))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.Model.Medal model = new BCW.BLL.Medal().GetMedalMe(id);
            string sPayID=string.Empty;
            sPayID = Utils.Mid(model.PayID, 1, model.PayID.Length);
            sPayID = Utils.DelLastChar(sPayID, "#");
            string[] temp = Regex.Split(sPayID, "##");
            //得到位置
            int strPn = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].ToString() == hid.ToString())
                {
                    strPn = i;
                    break;
                }
            }
            string sPayExTime = string.Empty;
            sPayExTime = Utils.Mid(model.PayExTime, 1, model.PayExTime.Length);
            sPayExTime = Utils.DelLastChar(sPayExTime, "#");
            string[] temp2 = Regex.Split(sPayExTime, "##");
            string PayExTime = string.Empty;
            for (int i = 0; i < temp2.Length; i++)
            {
                if (i != strPn)
                {
                    PayExTime += "#" + temp2[i] + "#";
                }

            }
            string PayID = model.PayID.Replace("#" + hid + "#", "");
            //更新
            new BCW.BLL.Medal().UpdatePayID(id, PayID, PayExTime);
            //清缓存
            string CacheKey = CacheName.App_UserMedal(hid);
            DataCache.RemoveByPattern(CacheKey);
            Utils.Success("撤销", "撤销成功..", Utils.getPage("medal.aspx?act=me&amp;hid=" + hid + ""), "1");
        }
    }
}
