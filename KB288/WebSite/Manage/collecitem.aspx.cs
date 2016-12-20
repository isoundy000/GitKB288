using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;
using BCW.Files;

public partial class Manage_collecitem : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "采集管理";

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
            case "view":
                ViewPage();
                break;
            case "listset":
                ListSetPage();
                break;
            case "listsetok":
                ListSetOkPage();
                break;
            case "linkset":
                LinkSetPage();
                break;
            case "linksetok":
                LinkSetOkPage();
                break;
            case "contentset":
                ContentSetPage();
                break;
            case "contentsetok":
                ContentSetOkPage();
                break;
            case "demo":
                DemoPage();
                break;
            case "del":
                DelPage();
                break;
            case "getdata":
                GetDataPage();
                break;
            case "dataview":
                DataViewPage();
                break;
            case "adddata":
                AddDataPage();
                break;
            case "deldata":
                DelDataPage();
                break;
            case "deldatas":
                DelDatasPage();
                break;
            case "accdata":
                AccDataPage();
                break;
            case "template":
                TemplatePage();
                break;
            case "acctemp":
                AccTempPage();
                break;
            case "acctempok":
            case "acctempok2":
                AccTempOkPage(act);
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("采集管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "";

        // 开始读取列表
        IList<BCW.Model.Collec.CollecItem> listCollecItem = new BCW.BLL.Collec.CollecItem().GetCollecItems(pageIndex, pageSize, strWhere, out recordCount);
        if (listCollecItem.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Collec.CollecItem n in listCollecItem)
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

                string sState = "(可用)";
                if (n.State == 0)
                    sState = "(不可用)";

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", (pageIndex - 1) * pageSize + k, n.ID, n.ItemName, sState);
                builder.Append("<br />描述：" + n.ItemRemark + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=acctemp") + "\">导入采集模板</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=add") + "\">添加采集项目</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AccTempPage()
    {
        Master.Title = "导入采集模板";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("导入采集模板");
        builder.Append(Out.Tab("</div>", ""));
        if (!Utils.Isie())
        {
            string strText = "模板地址:/,";
            string strName = "FileName,act";
            string strType = "text,hidden";
            string strValu = "http://'acctempok";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确定导入|reset,collecitem.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string strText = "txt模板文件:/,";
            string strName = "FileName,act";
            string strType = "file,hidden";
            string strValu = "'acctempok2";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确定导入|reset,collecitem.aspx,post,2,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />支持本地与远程txt模板文件地址.<br />切换WAP2.0可上传txt模板文件");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AccTempOkPage(string act)
    {
        string sPath = string.Empty;
        if (act == "acctempok")//采集
        {
            string FileName = Utils.GetRequest("FileName", "post", 2, @"^.+?.txt$", "请输入txt格式的模板文件");
            string Path = "/Files/collec/";
            sPath = BCW.Files.FileTool.DownloadFile(Path, 0, FileName);
        }
        else//上传
        {
            //遍历File表单元素
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    //检查是否允许上传格式
                    if (fileExtension != ".txt")
                    {
                        Utils.Error("只允许导入txt文件", "");
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = "/Files/collec/";
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        postedFile.SaveAs(SavePath);
                        sPath = DirPath + fileName;
                    }
                    else
                    {
                        Utils.Error("上传出现错误..", "");
                    }
                }
            }
        }
        //读取xml
        ub xml = new ub();
        try
        {
            Application.Remove(sPath);//清缓存
            xml.ReloadSub(sPath); //加载配置
        }
        catch {
            Utils.Error("采集/上传出现错误..", "");
        }

        int ptype = Convert.ToInt32(xml.dss["Types"]);
        DataSet ds = new BCW.BLL.Topics().GetList("ID,Title", "Types=" + (ptype + 10) + "");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            if (ptype == 1)
                Utils.Error("请先在设计中心添加文章栏目..", "");
            else
                Utils.Error("请先在设计中心添加图片栏目..", "");
        }

        BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
        model.Types = Convert.ToInt32(xml.dss["Types"]);
        model.ItemName = xml.dss["ItemName"].ToString();
        model.NodeId = 0;
        model.WebEncode = Convert.ToInt32(xml.dss["WebEncode"]);
        model.WebName = xml.dss["WebName"].ToString();
        model.WebUrl = xml.dss["WebUrl"].ToString();
        model.ItemRemark = xml.dss["ItemRemark"].ToString();
        model.Script_Html = xml.dss["Script_Html"].ToString();
        model.CollecNum = 0;
        //model.IsLikeTitle = Convert.ToInt32(xml.dss["IsLikeTitle"]);
        model.IsSaveImg = Convert.ToInt32(xml.dss["IsSaveImg"]);
        model.IsDesc = Convert.ToInt32(xml.dss["IsDesc"]);
        model.ListUrl = xml.dss["ListUrl"].ToString();
        model.ListStart = xml.dss["ListStart"].ToString();
        model.ListEnd = xml.dss["ListEnd"].ToString();
        model.NextListRegex = xml.dss["NextListRegex"].ToString();
        model.LinkStart = xml.dss["LinkStart"].ToString();
        model.LinkEnd = xml.dss["LinkEnd"].ToString();
        model.TitleStart = xml.dss["TitleStart"].ToString();
        model.TitleEnd = xml.dss["TitleEnd"].ToString();
        model.KeyWordStart = xml.dss["KeyWordStart"].ToString();
        model.KeyWordEnd = xml.dss["KeyWordEnd"].ToString();
        model.DateRegex = xml.dss["DateRegex"].ToString();
        model.ContentStart = xml.dss["ContentStart"].ToString();
        model.ContentEnd = xml.dss["ContentEnd"].ToString();
        model.RemoveBodyStart = xml.dss["RemoveBodyStart"].ToString();
        model.RemoveBodyEnd = xml.dss["RemoveBodyEnd"].ToString();
        model.RemoveTitle = xml.dss["RemoveTitle"].ToString();
        model.RemoveContent = xml.dss["RemoveContent"].ToString();
        model.NextPageRegex = xml.dss["NextPageRegex"].ToString();
        int id = new BCW.BLL.Collec.CollecItem().Add(model);

        Master.Title = "导入采集模板";

        string strEmpty = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                strEmpty += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"];
            }

        }
        strEmpty = "0|选择分类" + strEmpty;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("导入成功，请编辑");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",项目名称:/,所属栏目:/,页面编码方式:/,网站名称:/,网站地址:/,项目说明:/,过滤标签:/,保存图片:/,倒序采集:/,列表页面地址:/,列表开始标记:/,列表结束标记:/,列表下一页正则:/,链接开始标记:/,链接结束标记:/,标题开始标记:/,标题结束标记:/,关键字开始:/,关键字结束:/,获取时间正则:/,正文开始标记:/,正文结束标记:/,过滤正文开始标记(多个用" + Out.Tab("$", "$$") + "隔开):/,过滤正文结束标记(多个用" + Out.Tab("$", "$$") + "隔开):/,正文替换字符(多个用" + Out.Tab("$", "$$") + "隔开):/,正文对应替换(多个用" + Out.Tab("$", "$$") + "隔开):/,正文下一页正则:/,,,";
        string strName = "Types,ItemName,NodeId,WebEncode,WebName,WebUrl,ItemRemark,Script_Html,IsSaveImg,IsDesc,ListUrl,ListStart,ListEnd,NextListRegex,LinkStart,LinkEnd,TitleStart,TitleEnd,KeyWordStart,KeyWordEnd,DateRegex,ContentStart,ContentEnd,RemoveBodyStart,RemoveBodyEnd,RemoveTitle,RemoveContent,NextPageRegex,id,act,backurl";
        string strType = "hidden,text,select,select,text,text,text,multiple,select,select,text,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,hidden,hidden,hidden";
        string strValu = "" + model.Types + "'" + model.ItemName + "'" + model.NodeId + "'" + model.WebEncode + "'" + model.WebName + "'" + model.WebUrl + "'" + model.ItemRemark + "'" + model.Script_Html + "'" + model.IsSaveImg + "'" + model.IsDesc + "'" + model.ListUrl + "'" + model.ListStart + "'" + model.ListEnd + "'" + model.NextListRegex + "'" + model.LinkStart + "'" + model.LinkEnd + "'" + model.TitleStart + "'" + model.TitleEnd + "'" + model.KeyWordStart + "'" + model.KeyWordEnd + "'" + model.DateRegex + "'" + model.ContentStart + "'" + Out.Tab(model.ContentEnd, model.ContentEnd.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveBodyStart, model.RemoveBodyStart.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveBodyEnd, model.RemoveBodyEnd.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveTitle, model.RemoveTitle.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveContent, model.RemoveContent.Replace("$", "$$")) + "'" + model.NextPageRegex + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false," + strEmpty + ",0|unicode|1|utf-8|2|gb2312,false,false,true,Iframe|Iframe|Object|Object|Script|Script|Div|Div|Table|Table|Span|Span|Img|Img|Font|Font|A|A|Html|Html,0|否|1|是,0|否|1|是,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false";
        string strIdea = "/";
        string strOthe = "确定保存|reset,collecitem.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    {
        Master.Title = "添加采集项目";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "0"));
        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("选择采集类型");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=add&amp;ptype=1") + "\">资讯文章</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=add&amp;ptype=2") + "\">图片文件</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("添加采集项目");
            builder.Append(Out.Tab("</div>", ""));
            string strEmpty = "";

            DataSet ds = new BCW.BLL.Topics().GetList("ID,Title", "Types=" + (ptype + 10) + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    strEmpty += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"];
                }

            }
            strEmpty = "0|选择分类" + strEmpty;

            string strText = "项目名称:/,所属栏目:/,页面编码方式:/,网站名称:/,网站地址:/,项目说明:/,过滤标签:/,保存图片:/,倒序采集:/,,,";
            string strName = "ItemName,NodeId,WebEncode,WebName,WebUrl,ItemRemark,Script_Html,IsSaveImg,IsDesc,ptype,act,backurl";
            string strType = "text,select,select,text,text,text,multiple,select,select,hidden,hidden,hidden";
            string strValu = "'0'1'''''0'0'" + ptype + "'addsave'" + Utils.getPage(0) + "";
            string strEmpt = "false," + strEmpty + ",0|unicode|1|utf-8|2|gb2312,false,false,true,Iframe|Iframe|Object|Object|Script|Script|Div|Div|Table|Table|Span|Span|Img|Img|Font|Font|A|A|Html|Html,0|否|1|是,0|否|1|是,false,false,false";
            string strIdea = "/";
            string strOthe = "添加项目|reset,collecitem.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-2]$", "采集类型错误"));
        string ItemName = Utils.GetRequest("ItemName", "post", 2, @"^[\s\S]{1,30}$", "项目名称限30字内");
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[1-9]\d*$", "选择所属栏目错误"));
        int WebEncode = int.Parse(Utils.GetRequest("WebEncode", "post", 2, @"^[0-2]\d*$", "选择页面编码方式错误"));
        string WebName = Utils.GetRequest("WebName", "post", 2, @"^[\s\S]{1,30}$", "网站名称限30字内");
        string WebUrl = Utils.GetRequest("WebUrl", "post", 2, @"^[\s\S]{1,100}$", "网站地址限100字内");
        string ItemRemark = Utils.GetRequest("ItemRemark", "post", 3, @"^[\s\S]{1,50}$", "项目说明限50字内");
        string Script_Html = Utils.GetRequest("Script_Html", "post", 3, @"^[\s\S]{1,200}$", "过滤标签选择错误，可多选");
        int IsSaveImg = int.Parse(Utils.GetRequest("IsSaveImg", "post", 2, @"^[0-1]\d*$", "选择保存图片错误"));
        int IsDesc = int.Parse(Utils.GetRequest("IsDesc", "post", 2, @"^[0-1]\d*$", "选择倒序采集错误"));

        BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
        model.Types = ptype;
        model.ItemName = ItemName;
        model.NodeId = NodeId;
        model.WebEncode = WebEncode;
        model.WebName = WebName;
        model.WebUrl = WebUrl;
        model.ItemRemark = ItemRemark;
        model.Script_Html = Script_Html;
        model.CollecNum = 0;
        model.IsSaveImg = IsSaveImg;
        model.IsDesc = IsDesc;
        int id = new BCW.BLL.Collec.CollecItem().Add(model);
        Utils.Success("添加采集项目", "添加采集项目成功<br /><a href=\"" + Utils.getUrl("collecitem.aspx?act=listset&amp;id=" + id + "") + "\">下一步设置</a>", Utils.getUrl("collecitem.aspx"), "180");
    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }
        Master.Title = "管理-" + model.ItemName + "";
        builder.Append(Out.Tab("<div class=\"title\">管理-" + model.ItemName + "</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("采集类型:" + ((model.Types == 1) ? "文章" : "图片") + "<br />");
        builder.Append("采集栏目:<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + model.NodeId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Topics().GetTitle(model.NodeId) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=getdata&amp;id=" + id + "") + "\">已采集(" + new BCW.BLL.Collec.Collecdata().GetCount(id) + ")条</a>");
        builder.Append("[<a href=\"" + Utils.getUrl("collecitem.aspx?act=accdata&amp;id=" + id + "") + "\">导入</a>]");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=listset&amp;id=" + id + "") + "\">列表设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=linkset&amp;id=" + id + "") + "\">链接设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=contentset&amp;id=" + id + "") + "\">正文设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=demo&amp;id=" + id + "") + "\">采样测试</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecstart.aspx?id=" + id + "") + "\">立即采集</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=edit&amp;id=" + id + "") + "\">编辑项目</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=template&amp;id=" + id + "") + "\">导出模板</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=del&amp;id=" + id + "") + "\">删除项目</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ListSetPage()
    {
        Master.Title = "编辑列表设置";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑列表设置");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "*列表页面地址:/,*列表开始标记:/,*列表结束标记:/,列表下一页正则:/,,,";
        string strName = "ListUrl,ListStart,ListEnd,NextListRegex,id,act,backurl";
        string strType = "text,textarea,textarea,textarea,hidden,hidden,hidden";
        string strValu = "" + model.ListUrl + "'" + model.ListStart + "'" + model.ListEnd + "'" + model.NextListRegex + "'" + id + "'listsetok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,true,false,false,false";
        string strIdea = "/";
        string strOthe = "确定保存|reset,collecitem.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ListSetOkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "项目ID错误"));
        string ListUrl = Utils.GetRequest("ListUrl", "post", 2, @"^[\s\S]{1,300}$", "列表页面地址错误");
        string ListStart = Utils.GetRequest("ListStart", "post", 2, @"^[\s\S]{1,300}$", "列表开始标记错误");
        string ListEnd = Utils.GetRequest("ListEnd", "post", 2, @"^[\s\S]{1,300}$", "列表结束标记错误");
        string NextListRegex = Utils.GetRequest("NextListRegex", "post", 3, @"^[\s\S]{1,300}$", "列表下一页正则错误");
        if (!new BCW.BLL.Collec.CollecItem().Exists(id))
        {
            Utils.Error("不存在的采集项目", "");
        }
        BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
        model.ID = id;
        model.ListUrl = ListUrl;
        model.ListStart = ListStart;
        model.ListEnd = ListEnd;
        model.NextListRegex = NextListRegex;
        new BCW.BLL.Collec.CollecItem().UpdateListSet(model);
        //验证采集
        BCW.Model.Collec.CollecItem modeldemo = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(modeldemo, 0, out test);
        Utils.Success("编辑列表设置", "添加采集项目成功<br /><a href=\"" + Utils.getUrl("collecitem.aspx?act=linkset&amp;id=" + id + "") + "\">下一步设置</a>", Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + ""), "180");
    }

    private void LinkSetPage()
    {
        Master.Title = "编辑链接设置";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }

        //验证采集
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(model, 0, out test);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑链接设置");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("列表截取测试<br />" + test + "");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "*链接开始标记:/,*链接结束标记:/,,,";
        string strName = "LinkStart,LinkEnd,id,act,backurl";
        string strType = "textarea,textarea,hidden,hidden,hidden";
        string strValu = "" + model.LinkStart + "'" + model.LinkEnd + "'" + id + "'linksetok'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定保存|reset,collecitem.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void LinkSetOkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "项目ID错误"));
        string LinkStart = Utils.GetRequest("LinkStart", "post", 2, @"^[\s\S]{1,300}$", "列表开始标记错误");
        string LinkEnd = Utils.GetRequest("LinkEnd", "post", 2, @"^[\s\S]{1,300}$", "列表结束标记错误");
        if (!new BCW.BLL.Collec.CollecItem().Exists(id))
        {
            Utils.Error("不存在的采集项目", "");
        }
        BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
        model.ID = id;
        model.LinkStart = LinkStart;
        model.LinkEnd = LinkEnd;
        new BCW.BLL.Collec.CollecItem().UpdateLinkSet(model);
        //验证采集
        BCW.Model.Collec.CollecItem modeldemo = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(modeldemo, 1, out test);
        Utils.Success("编辑链接设置", "添加采集项目成功<br /><a href=\"" + Utils.getUrl("collecitem.aspx?act=contentset&amp;id=" + id + "") + "\">下一步设置</a>", Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + ""), "180");
    }

    private void ContentSetPage()
    {
        Master.Title = "编辑正文设置";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }

        //验证采集
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(model, 1, out test);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑正文设置");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("链接截取测试<br />" + test + "");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "*标题开始标记:/,*标题结束标记:/,关键字开始:/,关键字结束:/,获取时间正则:/,*正文开始标记:/,*正文结束标记:/,过滤正文开始标记(多个用" + Out.Tab("$", "$$") + "隔开):/,过滤正文结束标记(多个用" + Out.Tab("$", "$$") + "隔开):/,正文替换字符(多个用" + Out.Tab("$", "$$") + "隔开):/,正文对应替换(多个用" + Out.Tab("$", "$$") + "隔开):/,正文下一页正则:/,,,";
        string strName = "TitleStart,TitleEnd,KeyWordStart,KeyWordEnd,DateRegex,ContentStart,ContentEnd,RemoveBodyStart,RemoveBodyEnd,RemoveTitle,RemoveContent,NextPageRegex,id,act,backurl";
        string strType = "textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,hidden,hidden,hidden";
        string strValu = "";
        if (!string.IsNullOrEmpty(model.TitleStart))
            strValu = "" + model.TitleStart + "'" + model.TitleEnd + "'" + model.KeyWordStart + "'" + model.KeyWordEnd + "'" + model.DateRegex + "'" + model.ContentStart + "'" + Out.Tab(model.ContentEnd, model.ContentEnd.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveBodyStart, model.RemoveBodyStart.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveBodyEnd, model.RemoveBodyEnd.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveTitle, model.RemoveTitle.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveContent, model.RemoveContent.Replace("$", "$$")) + "'" + model.NextPageRegex + "'" + id + "'contentsetok'" + Utils.getPage(0) + "";
        else
            strValu = "" + model.TitleStart + "'" + model.TitleEnd + "'" + model.KeyWordStart + "'" + model.KeyWordEnd + "'" + model.DateRegex + "'" + model.ContentStart + "'" + model.ContentEnd + "'" + model.RemoveBodyStart+ "'" + model.RemoveBodyEnd + "'" + model.RemoveTitle + "'" + model.RemoveContent + "'" + model.NextPageRegex + "'" + id + "'contentsetok'" + Utils.getPage(0) + "";

        string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,false,false,false";
        string strIdea = "/";
        string strOthe = "确定保存|reset,collecitem.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ContentSetOkPage()
    {

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "项目ID错误"));
        string TitleStart = Utils.GetRequest("TitleStart", "post", 2, @"^[\s\S]{1,300}$", "标题开始标记错误");
        string TitleEnd = Utils.GetRequest("TitleEnd", "post", 2, @"^[\s\S]{1,300}$", "标题结束标记错误");
        string KeyWordStart = Utils.GetRequest("KeyWordStart", "post", 3, @"^[\s\S]{1,300}$", "关键字开始标记错误");
        string KeyWordEnd = Utils.GetRequest("KeyWordEnd", "post", 3, @"^[\s\S]{1,300}$", "关键字结束标记错误");
        string DateRegex = Utils.GetRequest("DateRegex", "post", 3, @"^[\s\S]{1,300}$", "时间正则错误");
        string ContentStart = Utils.GetRequest("ContentStart", "post", 2, @"^[\s\S]{1,300}$", "正文开始标记错误");
        string ContentEnd = Utils.GetRequest("ContentEnd", "post", 2, @"^[\s\S]{1,300}$", "正文结束标记错误");
        string RemoveBodyStart = Utils.GetRequest("RemoveBodyStart", "post", 3, @"^[\s\S]{1,300}$", "过滤正文开始标记错误");
        string RemoveBodyEnd = Utils.GetRequest("RemoveBodyEnd", "post", 3, @"^[\s\S]{1,300}$", "过滤正文结束标记错误");
        string RemoveTitle = Utils.GetRequest("RemoveTitle", "post", 3, @"^[\s\S]{1,300}$", "正文替换字符填写错误");
        string RemoveContent = Utils.GetRequest("RemoveContent", "post", 3, @"^[\s\S]{1,300}$", "正文对应替换填写错误");
        string NextPageRegex = Utils.GetRequest("NextPageRegex", "post", 3, @"^[\s\S]{1,300}$", "下一页正则错误");

        if (!new BCW.BLL.Collec.CollecItem().Exists(id))
        {
            Utils.Error("不存在的采集项目", "");
        }
        BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
        model.ID = id;
        model.TitleStart = TitleStart;
        model.TitleEnd = TitleEnd;
        model.KeyWordStart = KeyWordStart;
        model.KeyWordEnd = KeyWordEnd;
        model.DateRegex = DateRegex;
        model.ContentStart = ContentStart;
        model.ContentEnd = ContentEnd.Replace("$$", "$");
        model.RemoveBodyStart = RemoveBodyStart.Replace("$$", "$");
        model.RemoveBodyEnd = RemoveBodyEnd.Replace("$$", "$");
        model.RemoveTitle = RemoveTitle.Replace("$$", "$");
        model.RemoveContent = RemoveContent.Replace("$$", "$");
        model.NextPageRegex = NextPageRegex;
        new BCW.BLL.Collec.CollecItem().UpdateContentSet(model);

        BCW.Model.Collec.CollecItem modeldemo = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        //验证采集
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(modeldemo, 2, out test);
        new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 1);
        Utils.Success("编辑正文设置", "添加采集项目成功<br /><a href=\"" + Utils.getUrl("collecitem.aspx?act=demo&amp;id=" + id + "") + "\">已设置完成，采样测试</a>", Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + ""), "180");
    }

    private void DemoPage()
    {
        Master.Title = "采样测试";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }

        //验证采集
        string test = string.Empty;
        new BCW.Collec.GetText().GetTest(model, 2, out test);
        new BCW.BLL.Collec.CollecItem().UpdateState(model.ID, 1);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("采样测试");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("" + Out.SysUBB(test) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        Master.Title = "编辑采集项目";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "项目ID错误"));
        BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
        if (model == null)
        {
            Utils.Error("不存在的采集项目", "");
        }

        string strEmpty = "";

        DataSet ds = new BCW.BLL.Topics().GetList("ID,Title", "Types=" + (model.Types + 10) + "");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                strEmpty += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"];
            }

        }
        strEmpty = "0|选择分类" + strEmpty;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑采集项目");
        builder.Append(Out.Tab("</div>", ""));
        string strText = ",项目名称:/,所属栏目:/,页面编码方式:/,网站名称:/,网站地址:/,项目说明:/,过滤标签:/,保存图片:/,倒序采集:/,列表页面地址:/,列表开始标记:/,列表结束标记:/,列表下一页正则:/,链接开始标记:/,链接结束标记:/,标题开始标记:/,标题结束标记:/,关键字开始:/,关键字结束:/,获取时间正则:/,正文开始标记:/,正文结束标记:/,过滤正文开始标记(多个用" + Out.Tab("$", "$$") + "隔开):/,过滤正文结束标记(多个用" + Out.Tab("$", "$$") + "隔开):/,正文替换字符(多个用" + Out.Tab("$", "$$") + "隔开):/,正文对应替换(多个用" + Out.Tab("$", "$$") + "隔开):/,正文下一页正则:/,,,";
        string strName = "Types,ItemName,NodeId,WebEncode,WebName,WebUrl,ItemRemark,Script_Html,IsSaveImg,IsDesc,ListUrl,ListStart,ListEnd,NextListRegex,LinkStart,LinkEnd,TitleStart,TitleEnd,KeyWordStart,KeyWordEnd,DateRegex,ContentStart,ContentEnd,RemoveBodyStart,RemoveBodyEnd,RemoveTitle,RemoveContent,NextPageRegex,id,act,backurl";
        string strType = "hidden,text,select,select,text,text,text,multiple,select,select,text,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,hidden,hidden,hidden";
        string strValu = "";
        if (!string.IsNullOrEmpty(model.TitleStart))
            strValu = "" + model.Types + "'" + model.ItemName + "'" + model.NodeId + "'" + model.WebEncode + "'" + model.WebName + "'" + model.WebUrl + "'" + model.ItemRemark + "'" + model.Script_Html + "'" + model.IsSaveImg + "'" + model.IsDesc + "'" + model.ListUrl + "'" + model.ListStart + "'" + model.ListEnd + "'" + model.NextListRegex + "'" + model.LinkStart + "'" + model.LinkEnd + "'" + model.TitleStart + "'" + model.TitleEnd + "'" + model.KeyWordStart + "'" + model.KeyWordEnd + "'" + model.DateRegex + "'" + model.ContentStart + "'" + Out.Tab(model.ContentEnd, model.ContentEnd.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveBodyStart, model.RemoveBodyStart.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveBodyEnd, model.RemoveBodyEnd.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveTitle, model.RemoveTitle.Replace("$", "$$")) + "'" + Out.Tab(model.RemoveContent, model.RemoveContent.Replace("$", "$$")) + "'" + model.NextPageRegex + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        else
            strValu = "" + model.Types + "'" + model.ItemName + "'" + model.NodeId + "'" + model.WebEncode + "'" + model.WebName + "'" + model.WebUrl + "'" + model.ItemRemark + "'" + model.Script_Html + "'" + model.IsSaveImg + "'" + model.IsDesc + "'" + model.ListUrl + "'" + model.ListStart + "'" + model.ListEnd + "'" + model.NextListRegex + "'" + model.LinkStart + "'" + model.LinkEnd + "'" + model.TitleStart + "'" + model.TitleEnd + "'" + model.KeyWordStart + "'" + model.KeyWordEnd + "'" + model.DateRegex + "'" + model.ContentStart + "'" + model.ContentEnd + "'" + model.RemoveBodyStart + "'" + model.RemoveBodyEnd + "'" + model.RemoveTitle + "'" + model.RemoveContent + "'" + model.NextPageRegex + "'" + id + "'editsave'" + Utils.getPage(0) + "";

        string strEmpt = "false,false," + strEmpty + ",0|unicode|1|utf-8|2|gb2312,false,false,true,Iframe|Iframe|Object|Object|Script|Script|Div|Div|Table|Table|Span|Span|Img|Img|Font|Font|A|A|Html|Html,0|否|1|是,0|否|1|是,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false";
        string strIdea = "/";
        string strOthe = "确定保存|reset,collecitem.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int Types = int.Parse(Utils.GetRequest("Types", "post", 1, @"^[1-2]$", "1"));
        string ItemName = Utils.GetRequest("ItemName", "post", 2, @"^[\s\S]{1,30}$", "项目名称限30字内");
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[1-9]\d*$", "选择所属栏目错误"));
        int WebEncode = int.Parse(Utils.GetRequest("WebEncode", "post", 2, @"^[0-2]\d*$", "选择页面编码方式错误"));
        string WebName = Utils.GetRequest("WebName", "post", 2, @"^[\s\S]{1,30}$", "网站名称限30字内");
        string WebUrl = Utils.GetRequest("WebUrl", "post", 2, @"^[\s\S]{1,100}$", "网站地址限100字内");
        string ItemRemark = Utils.GetRequest("ItemRemark", "post", 3, @"^[\s\S]{1,50}$", "项目说明限50字内");
        string Script_Html = Utils.GetRequest("Script_Html", "post", 3, @"^[\s\S]{1,200}$", "过滤标签选择错误，可多选");
        int IsSaveImg = int.Parse(Utils.GetRequest("IsSaveImg", "post", 2, @"^[0-1]\d*$", "选择保存图片错误"));
        int IsDesc = int.Parse(Utils.GetRequest("IsDesc", "post", 2, @"^[0-1]\d*$", "选择倒序采集错误"));
        string ListUrl = Utils.GetRequest("ListUrl", "post", 2, @"^[\s\S]{1,300}$", "列表页面地址错误");
        string ListStart = Utils.GetRequest("ListStart", "post", 2, @"^[\s\S]{1,300}$", "列表开始标记错误");
        string ListEnd = Utils.GetRequest("ListEnd", "post", 2, @"^[\s\S]{1,300}$", "列表结束标记错误");
        string NextListRegex = Utils.GetRequest("NextListRegex", "post", 3, @"^[\s\S]{1,300}$", "列表下一页正则错误");
        string LinkStart = Utils.GetRequest("LinkStart", "post", 2, @"^[\s\S]{1,300}$", "列表开始标记错误");
        string LinkEnd = Utils.GetRequest("LinkEnd", "post", 2, @"^[\s\S]{1,300}$", "列表结束标记错误");
        string TitleStart = Utils.GetRequest("TitleStart", "post", 2, @"^[\s\S]{1,300}$", "标题开始标记错误");
        string TitleEnd = Utils.GetRequest("TitleEnd", "post", 2, @"^[\s\S]{1,300}$", "标题结束标记错误");
        string KeyWordStart = Utils.GetRequest("KeyWordStart", "post", 3, @"^[\s\S]{1,300}$", "关键字开始标记错误");
        string KeyWordEnd = Utils.GetRequest("KeyWordEnd", "post", 3, @"^[\s\S]{1,300}$", "关键字结束标记错误");
        string DateRegex = Utils.GetRequest("DateRegex", "post", 3, @"^[\s\S]{1,300}$", "时间正则错误");
        string ContentStart = Utils.GetRequest("ContentStart", "post", 2, @"^[\s\S]{1,300}$", "正文开始标记错误");
        string ContentEnd = Utils.GetRequest("ContentEnd", "post", 2, @"^[\s\S]{1,300}$", "正文结束标记错误");
        string RemoveBodyStart = Utils.GetRequest("RemoveBodyStart", "post", 3, @"^[\s\S]{1,300}$", "过滤正文开始标记错误");
        string RemoveBodyEnd = Utils.GetRequest("RemoveBodyEnd", "post", 3, @"^[\s\S]{1,300}$", "过滤正文结束标记错误");
        string RemoveTitle = Utils.GetRequest("RemoveTitle", "post", 3, @"^[\s\S]{1,300}$", "正文替换字符填写错误");
        string RemoveContent = Utils.GetRequest("RemoveContent", "post", 3, @"^[\s\S]{1,300}$", "正文对应替换填写错误");
        string NextPageRegex = Utils.GetRequest("NextPageRegex", "post", 3, @"^[\s\S]{1,300}$", "正文下一页正则错误");
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "项目ID错误"));
        if (!new BCW.BLL.Collec.CollecItem().Exists(id))
        {
            Utils.Error("不存在的采集项目", "");
        }

        BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
        model.ID = id;
        model.Types = Types;
        model.ItemName = ItemName;
        model.NodeId = NodeId;
        model.WebEncode = WebEncode;
        model.WebName = WebName;
        model.WebUrl = WebUrl;
        model.ItemRemark = ItemRemark;
        model.Script_Html = Script_Html;
        model.CollecNum = 0;
        model.IsSaveImg = IsSaveImg;
        model.IsDesc = IsDesc;
        model.ListUrl = ListUrl;
        model.ListStart = ListStart;
        model.ListEnd = ListEnd;
        model.NextListRegex = NextListRegex;
        model.LinkStart = LinkStart;
        model.LinkEnd = LinkEnd;
        model.TitleStart = TitleStart;
        model.TitleEnd = TitleEnd;
        model.KeyWordStart = KeyWordStart;
        model.KeyWordEnd = KeyWordEnd;
        model.DateRegex = DateRegex;
        model.ContentStart = ContentStart;
        model.ContentEnd = ContentEnd.Replace("$$", "$");
        model.RemoveBodyStart = RemoveBodyStart.Replace("$$", "$");
        model.RemoveBodyEnd = RemoveBodyEnd.Replace("$$", "$");
        model.RemoveTitle = RemoveTitle.Replace("$$", "$");
        model.RemoveContent = RemoveContent.Replace("$$", "$");
        model.NextPageRegex = NextPageRegex;
        new BCW.BLL.Collec.CollecItem().Update(model);
        Utils.Success("编辑采集项目", "编辑采集项目成功<br /><a href=\"" + Utils.getUrl("collecitem.aspx?act=demo&amp;id=" + id + "") + "\">验证项目/采样测试</a>", Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + ""), "2");

    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除采集项目";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此采集项目吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Collec.CollecItem().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Collec.CollecItem().Delete(id);
            Utils.Success("删除采集项目", "删除采集项目成功..", Utils.getUrl("collecitem.aspx"), "1");
        }
    }

    private void TemplatePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            Master.Title = "导出采集模板";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定导出此采集模板吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "id,act,info,backurl";
            string strValu = "" + id + "'template'ok'" + Utils.getPage(0) + "";
            string strOthe = "导出TXT,collecitem.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            BCW.Model.Collec.CollecItem model = new BCW.BLL.Collec.CollecItem().GetCollecItem(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            ub xml = new ub();
            string xmlPath = "/Files/collec/temp" + id + ".txt";

            if (!System.IO.File.Exists(Server.MapPath(xmlPath)))
            {
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), "<?xml version=\"1.0\" encoding=\"utf-8\"?><data></data>", System.Text.Encoding.UTF8);
            }
            xml.ReloadSub(xmlPath); //加载配置
            xml.dss["ItemName"] = model.ItemName;
            xml.dss["id"] = id;
            xml.dss["Types"] = model.Types;
            xml.dss["WebEncode"] = model.WebEncode;
            xml.dss["WebName"] = model.WebName;
            xml.dss["WebUrl"] = model.WebUrl;
            xml.dss["ItemRemark"] = model.ItemRemark;
            xml.dss["Script_Html"] = model.Script_Html;
            xml.dss["CollecNum"] = model.CollecNum;
            xml.dss["IsLikeTitle"] = 0;
            xml.dss["IsSaveImg"] = model.IsSaveImg;
            xml.dss["IsDesc"] = model.IsDesc;
            xml.dss["ListUrl"] = model.ListUrl;
            xml.dss["ListStart"] = model.ListStart;
            xml.dss["ListEnd"] = model.ListEnd;
            xml.dss["NextListRegex"] = model.NextListRegex;
            xml.dss["LinkStart"] = model.LinkStart;
            xml.dss["LinkEnd"] = model.LinkEnd;
            xml.dss["TitleStart"] = model.TitleStart;
            xml.dss["TitleEnd"] = model.TitleEnd;
            xml.dss["KeyWordStart"] = model.KeyWordStart;
            xml.dss["KeyWordEnd"] = model.KeyWordEnd;
            xml.dss["DateRegex"] = model.DateRegex;
            xml.dss["ContentStart"] = model.ContentStart;
            xml.dss["ContentEnd"] = model.ContentEnd;
            xml.dss["RemoveBodyStart"] = model.RemoveBodyStart;
            xml.dss["RemoveBodyEnd"] = model.RemoveBodyEnd;
            xml.dss["RemoveTitle"] = model.RemoveTitle;
            xml.dss["RemoveContent"] = model.RemoveContent;
            xml.dss["NextPageRegex"] = model.NextPageRegex;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("导出采集模板", "导出模板成功，文件路径http://" + Utils.GetDomain() + "/Files/collec/temp" + id + ".txt", Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + ""), "30");
        }
    }

    private void GetDataPage()
    {
        Master.Title = "查看采集内容";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看采集内容");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
    
        if (!new BCW.BLL.Collec.CollecItem().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "ItemId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Collec.Collecdata> listCollecdata = new BCW.BLL.Collec.Collecdata().GetCollecdatas(pageIndex, pageSize, strWhere, out recordCount);
        if (listCollecdata.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Collec.Collecdata n in listCollecdata)
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

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("collecitem.aspx?act=dataview&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", (pageIndex - 1) * pageSize + k, n.ID, n.Title, DT.FormatDate(n.AddTime, 0));
                builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=adddata&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[导]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=deldata&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
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
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=accdata&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[立即导入]</a>");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=deldatas&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[清空记录]</a>");        
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DataViewPage()
    {
        Master.Title = "查看采集内容";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Collec.Collecdata model = new BCW.BLL.Collec.Collecdata().GetCollecdata(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + model.Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("关键字:" + model.KeyWord + "<br />");
        builder.Append("内容:" + Out.SysUBB(model.Content) + "<br />");
        builder.Append("图片地址:" + model.Pics + "<br />");
        builder.Append("时间:" + model.AddTime + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">返回采集管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelDataPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除采集记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此采集记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?info=ok&amp;act=deldata&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Collec.Collecdata().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除附件
            string Pics = new BCW.BLL.Collec.Collecdata().GetPics(id);
            try
            {
                System.IO.File.Delete(Server.MapPath(Pics));
                System.IO.File.Delete(Server.MapPath(Pics.Replace("act", "prev")));
                System.IO.File.Delete(Server.MapPath(Pics.Replace("act/", "act/cover/")));
            }
            catch { }
            builder.Append("删除成功");
            //删除
            new BCW.BLL.Collec.Collecdata().Delete(id);
            Utils.Success("删除采集记录", "删除采集记录成功..", Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }

    private void AddDataPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "导入采集记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定导入此采集记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?info=ok&amp;act=adddata&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定导入</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Collec.Collecdata().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //导入主数据表
            DataSet ds = new BCW.BLL.Collec.Collecdata().GetList("Types,NodeId,Title,KeyWord,Content,Pics,AddTime", "id=" + id + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                //添加验证
                if (!new BCW.BLL.Detail().Exists(ds.Tables[0].Rows[i]["Title"].ToString()))
                {
                    BCW.Model.Detail model = new BCW.Model.Detail();
                    model.Title = ds.Tables[0].Rows[i]["Title"].ToString();
                    model.KeyWord = ds.Tables[0].Rows[i]["KeyWord"].ToString();
                    model.Model = "";
                    model.IsAd = true;
                    model.Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString()) + 10;
                    model.NodeId = int.Parse(ds.Tables[0].Rows[i]["NodeId"].ToString());
                    model.Content = Out.UBB(ds.Tables[0].Rows[i]["Content"].ToString().Replace("$PageNext$", ""));
                    model.TarText = "";
                    model.LanText = "";
                    model.SafeText = "";
                    model.LyText = "";
                    model.UpText = "";
                    model.IsVisa = 0;
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                    model.Readcount = 0;
                    model.Recount = 0;
                    model.Cent = 0;
                    model.BzType = 0;
                    model.Hidden = 0;
                    model.UsID = 0;
                    int newId = new BCW.BLL.Detail().Add(model);
                    //更新附件与封面
                    string Pics = ds.Tables[0].Rows[i]["Pics"].ToString();

                    if (Pics != "" && Pics.Contains("#"))
                    {
                        string[] sTemp = Pics.Split('#');
                        string sPics = string.Empty;
                        try
                        {
                            if (Pics.Contains("#"))
                            {
                                sPics = sTemp[sTemp.Length - 1];
                            }
                            else
                            {
                                sPics = Pics;
                            }
                        }
                        catch { }
                        sPics = sPics.Replace("act/", "act/cover/");
                        sPics = sPics.Replace("text/", "text/cover/");
                        new BCW.BLL.Detail().UpdateCover(newId, sPics);
                        if (model.Types == 11)
                        {
                            new BCW.BLL.Detail().UpdatePics(newId, Pics);
                        }
                        else
                        {
                            //更新附件
                            for (int k = 0; k < sTemp.Length; k++)
                            {
                                BCW.Model.File fmodel = new BCW.Model.File();
                                fmodel.Types = 1;
                                fmodel.NodeId = newId;
                                fmodel.Files = sTemp[k];
                                fmodel.PrevFiles = sTemp[k].Replace("act", "prev");
                                fmodel.FileSize = GetFileContentLength(sTemp[k]);
                                fmodel.FileExt = FileTool.GetFileExt(sTemp[k]);
                                fmodel.Content = "";
                                fmodel.DownNum = 0;
                                fmodel.AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                                new BCW.BLL.File().Add(fmodel);
                            }
                        }
                    }
                }
                //删除记录
                new BCW.BLL.Collec.Collecdata().Delete("id=" + id + "");
            }
            Utils.Success("导入采集记录", "导入采集记录成功..", Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }

    private void DelDatasPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "清空采集记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空此采集记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?info=ok&amp;act=deldatas&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Collec.CollecItem().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            DataSet ds = new BCW.BLL.Collec.Collecdata().GetList("Pics", "ItemId=" + id + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Pics = ds.Tables[0].Rows[i]["Pics"].ToString();
                    if (!string.IsNullOrEmpty(Pics))
                    {
                        //删除附件
                        try
                        {
                            System.IO.File.Delete(Server.MapPath(Pics));
                            System.IO.File.Delete(Server.MapPath(Pics.Replace("act", "prev")));
                            System.IO.File.Delete(Server.MapPath(Pics.Replace("act/", "act/cover/")));
                        }
                        catch { }
                    }
                }
            }
            //删除
            new BCW.BLL.Collec.Collecdata().Delete("ItemId=" + id + "");
            Utils.Success("清空采集记录", "清空采集记录成功..", Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }

    private void AccDataPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "导入采集记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定导入此采集记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx?info=ok&amp;act=accdata&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定导入(共" + new BCW.BLL.Collec.Collecdata().GetCount(id) + "条)</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Collec.CollecItem().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //导入主数据表
            DataSet ds = new BCW.BLL.Collec.Collecdata().GetList("Types,NodeId,Title,KeyWord,Content,Pics,AddTime", "ItemId=" + id + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //添加验证
                    if (!new BCW.BLL.Detail().Exists(ds.Tables[0].Rows[i]["Title"].ToString()))
                    {
                        BCW.Model.Detail model = new BCW.Model.Detail();
                        model.Title = Out.UBB(ds.Tables[0].Rows[i]["Title"].ToString());
                        model.KeyWord = ds.Tables[0].Rows[i]["KeyWord"].ToString();
                        model.Model = "";
                        model.IsAd = true;
                        model.Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString()) + 10;
                        model.NodeId = int.Parse(ds.Tables[0].Rows[i]["NodeId"].ToString());
                        model.Content = Out.UBB(ds.Tables[0].Rows[i]["Content"].ToString().Replace("$PageNext$", ""));
                        model.TarText = "";
                        model.LanText = "";
                        model.SafeText = "";
                        model.LyText = "";
                        model.UpText = "";
                        model.IsVisa = 0;
                        model.AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                        model.Readcount = 0;
                        model.Recount = 0;
                        model.Cent = 0;
                        model.BzType = 0;
                        model.Hidden = 0;
                        model.UsID = 0;
                        int newId = new BCW.BLL.Detail().Add(model);
                        //更新附件与封面
                        string Pics = ds.Tables[0].Rows[i]["Pics"].ToString();

                        if (Pics != "" && Pics.Contains("#"))
                        {
                            string[] sTemp = Pics.Split('#');
                            string sPics = string.Empty;
                            try
                            {
                                if (Pics.Contains("#"))
                                {
                                    sPics = sTemp[sTemp.Length - 1];
                                }
                                else
                                {
                                    sPics = Pics;
                                }
                            }
                            catch { }
                            sPics = sPics.Replace("act/", "act/cover/");
                            sPics = sPics.Replace("text/", "text/cover/");
                            new BCW.BLL.Detail().UpdateCover(newId, sPics);
                            if (model.Types == 11)
                            {
                                new BCW.BLL.Detail().UpdatePics(newId, Pics);
                            }
                            else
                            {
                                //更新附件
                                for (int k = 0; k < sTemp.Length; k++)
                                {
                                    BCW.Model.File fmodel = new BCW.Model.File();
                                    fmodel.Types = 1;
                                    fmodel.NodeId = newId;
                                    fmodel.Files = sTemp[k];
                                    fmodel.PrevFiles = sTemp[k].Replace("act", "prev");
                                    fmodel.FileSize = GetFileContentLength(sTemp[k]);
                                    fmodel.FileExt = FileTool.GetFileExt(sTemp[k]);
                                    fmodel.Content = "";
                                    fmodel.DownNum = 0;
                                    fmodel.AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                                    new BCW.BLL.File().Add(fmodel);
                                }
                            }
                        }
                    }
                }
                //删除记录
                new BCW.BLL.Collec.Collecdata().Delete("ItemId=" + id + "");
            }
            else
            {
                Utils.Error("没有任何采集记录..", "");
            }
            Utils.Success("导入采集记录", "导入采集记录成功..", Utils.getPage("collecitem.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }

    /// <summary>
    /// 得到文件大小
    /// </summary>
    public static long GetFileContentLength(string FilePath)
    {
        if (string.IsNullOrEmpty(FilePath))
            return 0;
        long fileSize = 0;
        string Path = string.Empty;
        if (FilePath.IndexOf("http://") == -1)
            Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);
        else
            Path = FilePath;
        try
        {
            System.Net.WebRequest req = WebRequest.Create(Path);
            System.Net.WebResponse rep = req.GetResponse();
            fileSize = Convert.ToInt64(rep.ContentLength);
            rep.Close();
        }
        catch { }

        return fileSize;
    }
}