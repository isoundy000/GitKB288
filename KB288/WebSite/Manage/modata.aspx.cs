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

public partial class Manage_modata : System.Web.UI.Page
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
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "brand":
                BrandPage();
                break;
            case "system":
                SystemPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "机型管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));
        string keyword = Utils.GetRequest("keyword", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("机型管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (keyword != "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("modata.aspx") + "\">全部</a>|搜索机型");
        }
        else if (ptype > 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("modata.aspx") + "\">全部</a>|分类机型");
        }
        else
        {
            builder.Append("全部机型|");
            builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?act=brand") + "\">品牌</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?act=system") + "\">平台</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "keyword" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (!string.IsNullOrEmpty(keyword))
            strWhere = "(PhoneModel like '%" + keyword + "%' OR PhoneSystem like '%" + keyword + "%')";
        else if (ptype > 0)
            strWhere = "Types =" + ptype + "";

        // 开始读取列表
        IList<BCW.Model.Modata> listModata = new BCW.BLL.Modata().GetModatas(pageIndex, pageSize, strWhere, out recordCount);
        if (listModata.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Modata n in listModata)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("modata.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;{1}.{2}{3}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.PhoneBrand, n.PhoneModel);
                builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
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
        string strText = "输入机型:如N73/,,";
        string strName = "keyword,backurl";
        string strType = "text,hidden,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "搜索,modata.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?act=add") + "\">添加机型</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void BrandPage()
    {
        Master.Title = "手机品牌列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("手机品牌列表");
        builder.Append(Out.Tab("</div>", ""));

        //列出手机品牌
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        DataSet ds = new BCW.BLL.Modata().GetList("Types,PhoneBrand,COUNT(*)", "Types > 0 GROUP BY Types,PhoneBrand ORDER BY COUNT(*) DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                    string Brand = ds.Tables[0].Rows[i]["PhoneBrand"].ToString();
                    builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?ptype=" + Types + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Brand + "</a>");
                    builder.Append(Out.Tab("</div>", ""));

                }
                if (k == endIndex)
                    break;
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("modata.aspx") + "\">机型管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SystemPage()
    {
        Master.Title = "手机平台列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("手机平台列表");
        builder.Append(Out.Tab("</div>", ""));

        //列出手机平台
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        DataSet ds = new BCW.BLL.Modata().GetList("PhoneSystem,COUNT(*)", "Types > 0 and PhoneSystem<>'不支持JAVA' GROUP BY PhoneSystem ORDER BY COUNT(*) DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string System = ds.Tables[0].Rows[i]["PhoneSystem"].ToString();
                    builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?keyword=" + System + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + System + "</a>");
                    builder.Append(Out.Tab("</div>", ""));

                }
                if (k == endIndex)
                    break;
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("modata.aspx") + "\">机型管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }




    private void AddPage()
    {
        Master.Title = "添加手机型号";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加手机型号");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "手机品牌:/,型号:/,操作系统:/,主屏分辨率:/,,";
        string strName = "Brand,Model,System,Size,act,backurl";
        string strType = "text,text,text,text,hidden,hidden";
        string strValu = "''''save'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定添加|reset,modata.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("modata.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SavePage()
    {
        string Brand = Utils.GetRequest("Brand", "post", 2, @"^[\s\S]{1,50}$", "手机品牌填写错误");
        string Model = Utils.GetRequest("Model", "post", 2, @"^[\s\S]{1,50}$", "手机型号填写错误");
        string System = Utils.GetRequest("System", "post", 2, @"^[\s\S]{1,50}$", "操作系统填写错误");
        string Size = Utils.GetRequest("Size", "post", 2, @"^[\s\S]{1,50}$", "主屏分辨率分填写错误");

        int Types = new BCW.BLL.Modata().GetTypes(Brand);
        if (Types == 0)
        {
            Types = new BCW.BLL.Modata().GetMaxTypes();
        }
        BCW.Model.Modata model = new BCW.Model.Modata();
        model.Types = Types;
        model.PhoneBrand = Brand;
        model.PhoneModel = Model;
        model.PhoneSystem = System;
        model.PhoneSize = Size;
        new BCW.BLL.Modata().Add(model);

        Utils.Success("添加机型", "添加手机型号成功..", Utils.getUrl("modata.aspx"), "2");
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Modata().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Modata model = new BCW.BLL.Modata().GetModata(id);

        Master.Title = "修改手机型号";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改手机型号");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "手机品牌:/,型号:/,操作系统:/,主屏分辨率:/,,,";
        string strName = "Brand,Model,System,Size,id,act,backurl";
        string strType = "text,text,text,text,hidden,hidden,hidden";
        string strValu = "" + model.PhoneBrand + "'" + model.PhoneModel + "'" + model.PhoneSystem + "'" + model.PhoneSize + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定修改|reset,modata.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("modata.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string Brand = Utils.GetRequest("Brand", "post", 2, @"^[\s\S]{1,50}$", "手机品牌填写错误");
        string Model = Utils.GetRequest("Model", "post", 2, @"^[\s\S]{1,50}$", "手机型号填写错误");
        string System = Utils.GetRequest("System", "post", 2, @"^[\s\S]{1,50}$", "操作系统填写错误");
        string Size = Utils.GetRequest("Size", "post", 2, @"^[\s\S]{1,50}$", "主屏分辨率分填写错误");

        if (!new BCW.BLL.Modata().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Modata model = new BCW.Model.Modata();
        model.ID = id;
        model.PhoneBrand = Brand;
        model.PhoneModel = Model;
        model.PhoneSystem = System;
        model.PhoneSize = Size;
        new BCW.BLL.Modata().Update(model);

        Utils.Success("修改机型", "修改手机型号成功..", Utils.getPage("modata.aspx"), "2");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除手机型号";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此手机型号吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("modata.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("modata.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Modata().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Modata().Delete(id);
            Utils.Success("删除机型", "删除手机型号成功..", Utils.getPage("modata.aspx"), "1");
        }
    }
}
