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
public partial class Manage_classact : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/upfile.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Expires = -1;
        Response.Cache.SetNoStore();
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);

        builder.Append(Out.Tab("", ""));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string act = Utils.GetRequest("act", "get", 1, "", "");

        if (!string.IsNullOrEmpty(act))
        {
            if (id != 0)
            {
                if (ptype == 14)
                {
                    if (!new BCW.BLL.Goods().Exists(id))
                    {
                        Utils.Error("不存在的记录", "");
                    }
                }
                else
                {
                    if (!new BCW.BLL.Detail().Exists(id))
                    {
                        Utils.Error("不存在的记录", "");
                    }
                }
            }

            switch (ptype)
            {
                case 11:
                    InsertText(ptype, nid, id);
                    break;
                case 12:
                case 13:
                    InsertPicSoft(act, ptype, nid, id);
                    break;
                case 14:
                    InsertGoods(ptype, nid, id);
                    break;
                default:
                    InsertText(ptype, nid, id);
                    break;
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));

            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + nid + "") + "\">返回上一级</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回设计中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (id != 0)
            {
                if (!new BCW.BLL.Topics().ExistsIdTypes(id, ptype))
                {
                    Utils.Error("不存在的记录", "");
                }
            }
            if (leibie != 0)
            {
                if (ptype > 10)
                {
                    Utils.Error("选择类型错误", "");
                }
            }

            switch (ptype)
            {
                case 0:
                    ReloadPage(leibie, ptype, nid, id);
                    break;
                case 1:
                    Insert1(leibie, ptype, nid, id);
                    break;
                case 2:
                    Insert2(leibie, ptype, nid, id, "UBB标签");
                    break;
                case 3:
                    Insert2(leibie, ptype, nid, id, "图链显图");
                    break;
                case 4:
                    Insert2(leibie, ptype, nid, id, "超级链接");
                    break;
                case 5:
                    Insert2(leibie, ptype, nid, id, "WML标签");
                    break;
                case 6:
                    Insert4(leibie, ptype, nid, id, "论坛版块");
                    break;
                case 10:
                    Insert3(leibie, ptype, nid, id, "随机广告");
                    break;
                case 11:
                    Insert3(leibie, ptype, nid, id, "文章菜单");
                    break;
                case 12:
                    Insert3(leibie, ptype, nid, id, "图片菜单");
                    break;
                case 13:
                    Insert3(leibie, ptype, nid, id, "文件菜单");
                    break;
                case 14:
                    Insert3(leibie, ptype, nid, id, "商品菜单");
                    break;
                default:
                    Insert1(leibie, ptype, nid, id);
                    break;
            }

            if (ptype != 0)
            {
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                if (id != 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "") + "\">返回上一级</a><br />");
                }
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回设计中心</a><br />");
                builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
    }

    /// <summary>
    /// 选择菜单类型
    /// </summary>
    private void ReloadPage(int leibie, int ptype, int nid, int id)
    {
        Master.Title = "添加菜单";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加菜单{<a href=\"" + Utils.getUrl("help.aspx?backurl=" + Utils.PostPage(1) + "") + "\">帮助</a>}");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=1") + "\">页面菜单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=2") + "\">UBB标签</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=3") + "\">图链显图</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=4") + "\">超级链接</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=5") + "\">WML标签</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=6") + "\">论坛版块</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=10") + "\">随机广告</a><br />");
        if (leibie == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?nid=" + nid + "&amp;ptype=11") + "\">文章菜单</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?nid=" + nid + "&amp;ptype=12") + "\">图片菜单</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?nid=" + nid + "&amp;ptype=13") + "\">文件菜单</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?nid=" + nid + "&amp;ptype=14") + "\">商品菜单</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;backurl=" + Server.UrlEncode("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=2") + "") + "\">智能调用</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (nid == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回上一级</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + nid + "") + "\">返回上一级</a><br />");
        }
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    /// <summary>
    /// 添加/编辑页面菜单
    /// </summary>
    private void Insert1(int leibie, int ptype, int nid, int id)
    {

        if (id != 0)
        {
            Master.Title = "编辑菜单";
            builder.Append(Out.Div("title", "编辑菜单"));
        }
        else
        {
            Master.Title = "添加菜单";
            builder.Append(Out.Div("title", "添加新菜单"));
        }
        string strText = "页面菜单名称:/,栏目后面:/,浏览器限制:/,VIP等级限制:/,页面收费:/,收费币种:/,消费方式:(收费非0时有效)/,访问密码:(留空不加密)/,显示状态:/,显示顺序:/,,,,";
        string strName = "Title,IsBr,IsPc,VipLeven,Cent,BzType,SellTypes,InPwd,Hidden,Paixu,leibie,ptype,nid,id";
        string strType = "text,select,select,select,num,select,select,text,select,snum,hidden,hidden,hidden,hidden";
        string strValu = "";
        if (id != 0)
        {
            //读取菜单
            BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
            strValu = "" + model.Title + "'" + model.IsBr + "'" + model.IsPc + "'" + model.VipLeven + "'" + model.Cent + "'"+model.BzType+"'" + model.SellTypes + "'" + model.InPwd + "'" + model.Hidden + "'" + model.Paixu + "'" + model.Leibie + "'" + model.Types + "'" + model.NodeId + "'" + id + "";
        }
        else
        {
            strValu = "'1'0'0'0'0'0''0'0'" + leibie + "'" + ptype + "'" + nid + "'" + id + "";
        }
        string strEmpt = "false,0|换行|1|不换行,0|不限制|1|仅限手机,0|不限|1|一级|2|二级|3|三级|4|四级|5|五级|6|六级|7|七级|8|八级,true,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",0|按次计费|1|包周计费|2|包月计费,true,0|正常显示|1|登录可见|2|隐藏显示,false,,,,";
        string strIdea = "/";
        string strOthe = "";
        if (id != 0)
        {
            strOthe = "确定编辑|reset,classok.aspx,post,1,red|blue";
        }
        else
        {
            strOthe = "确定添加|reset,classok.aspx,post,1,red|blue";
        }
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }

    /// <summary>
    /// 添加/编辑菜单
    /// </summary>
    private void Insert2(int leibie, int ptype, int nid, int id, string pName)
    {
        if (id != 0)
        {
            Master.Title = "编辑" + pName + "";
            builder.Append(Out.Div("title", "编辑" + pName + ""));
        }
        else
        {
            Master.Title = "添加" + pName + "";
            builder.Append(Out.Div("title", "添加" + pName + ""));
        }
        string strText = "" + pName + "名称:/," + pName + "内容:/,栏目后面:/,VIP等级可见:/,显示状态:/,显示顺序:/,,,,";
        string strName = "Title,Content,IsBr,VipLeven,Hidden,Paixu,leibie,ptype,nid,id";
        string strType = "text,big,select,select,select,snum,hidden,hidden,hidden,hidden";
        string strValu = "";
        if (id != 0)
        {
            //读取菜单
            BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
            strValu = "" + model.Title + "'" + model.Content + "'" + model.IsBr + "'" + model.VipLeven + "'" + model.Hidden + "'" + model.Paixu + "'" + model.Leibie + "'" + model.Types + "'" + model.NodeId + "'" + id + "";
        }
        else
        {
            if (ptype == 2)
            {
                string content = Utils.GetRequest("content", "all", 1, "", "");
                try
                {
                    if (content.Contains("call"))
                    {
                        content = "" + BCW.User.AdminCall.CallValue[int.Parse(content.Replace("call", ""))] + "";
                    }
                }
                catch { }
                strValu = "'" + content + "''0'0'0'" + leibie + "'" + ptype + "'" + nid + "'" + id + "";
            }
            else
                strValu = "'''0'0'0'" + leibie + "'" + ptype + "'" + nid + "'" + id + "";
        }
        string strEmpt = "false,false,0|换行|1|不换行,0|不限|1|一级|2|二级|3|三级|4|四级|5|五级|6|六级|7|七级|8|八级,0|正常显示|1|登录可见|2|隐藏显示,false,,,,";
        string strIdea = "/";
        string strOthe = "";
        if (id != 0)
        {
            strOthe = "确定编辑|reset,classok.aspx,post,1,red|blue";
        }
        else
        {
            strOthe = "确定添加|reset,classok.aspx,post,1,red|blue";
        }
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }

    /// <summary>
    /// 添加/编辑子菜单
    /// </summary>
    private void Insert3(int leibie, int ptype, int nid, int id, string pName)
    {
        if (id != 0)
        {
            Master.Title = "编辑" + pName + "";
            builder.Append(Out.Div("title", "编辑" + pName + ""));
        }
        else
        {
            Master.Title = "添加" + pName + "";
            builder.Append(Out.Div("title", "添加" + pName + ""));
        }
        string strText = "" + pName + "名称:/,栏目后面:/,显示状态:/,显示顺序:/,,,,";
        string strName = "Title,IsBr,Hidden,Paixu,leibie,ptype,nid,id";
        string strType = "text,select,select,snum,hidden,hidden,hidden,hidden";
        string strValu = "";
        if (id != 0)
        {
            //读取菜单
            BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
            strValu = "" + model.Title + "'" + model.IsBr + "'" + model.Hidden + "'" + model.Paixu + "'" + model.Leibie + "'" + model.Types + "'" + model.NodeId + "'" + id + "";
        }
        else
        {
            strValu = "''0'0'" + leibie + "'" + ptype + "'" + nid + "'" + id + "";
        }
        string strEmpt = "false,0|换行|1|不换行,0|正常显示|1|登录可见|2|隐藏显示,false,,,,";
        string strIdea = "/";
        string strOthe = "";
        if (id != 0)
        {
            strOthe = "确定编辑|reset,classok.aspx,post,1,red|blue";
        }
        else
        {
            strOthe = "确定添加|reset,classok.aspx,post,1,red|blue";
        }
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

    }

    /// <summary>
    /// 添加/编辑菜单
    /// </summary>
    private void Insert4(int leibie, int ptype, int nid, int id, string pName)
    {
        if (id != 0)
        {
            Master.Title = "编辑" + pName + "";
            builder.Append(Out.Div("title", "编辑" + pName + ""));
        }
        else
        {
            Master.Title = "添加" + pName + "";
            builder.Append(Out.Div("title", "添加" + pName + ""));
        }
        string strText = "" + pName + "显示名称:/,选择论坛:/,栏目后面:/,显示状态:/,显示顺序:/,,,,";
        string strName = "Title,Content,IsBr,Hidden,Paixu,leibie,ptype,nid,id";
        string strType = "text,select,select,select,snum,hidden,hidden,hidden,hidden";

        string strForum = string.Empty;
        DataSet ds = new BCW.BLL.Forum().GetList("ID,Title", "");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("你还没有任何版块..<br /><a href=\"" + Utils.getUrl("forum.aspx") + "\">马上添加</a>", "");
        }
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strForum += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "(" + ds.Tables[0].Rows[i]["ID"] + ")";
        }
        strForum = "0|选择版块" + strForum;
        string strValu = "";
        if (id != 0)
        {
            //读取菜单
            BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
            strValu = "" + model.Title + "'" + model.Content + "'" + model.IsBr + "'" + model.Hidden + "'" + model.Paixu + "'" + model.Leibie + "'" + model.Types + "'" + model.NodeId + "'" + id + "";
        }
        else
        {
            strValu = "'''0'0'" + leibie + "'" + ptype + "'" + nid + "'" + id + "";
        }
        string strEmpt = "false," + strForum + ",0|换行|1|不换行,0|正常显示|1|登录可见|2|隐藏显示,false,,,,";
        string strIdea = "/";
        string strOthe = "";
        if (id != 0)
        {
            strOthe = "确定编辑|reset,classok.aspx,post,1,red|blue";
        }
        else
        {
            strOthe = "确定添加|reset,classok.aspx,post,1,red|blue";
        }
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }


    /// <summary>
    /// 添加/编辑文章
    /// </summary>
    private void InsertText(int ptype, int nid, int id)
    {
        if (id != 0)
        {
            Master.Title = "编辑文章";
            builder.Append(Out.Tab("<div class=\"title\">WAP2.0编辑文章</div>", "编辑文章"));
        }
        else
        {
            Master.Title = "添加文章";
            builder.Append(Out.Tab("<div class=\"title\">WAP2.0添加文章</div>", "添加文章"));
        }
        //附图数量
        int picNum = 0;
        if (!Utils.Isie())
        {
            if (id != 0)
            {
                strText = "文章标题:/,文章内容:/,搜索关键字:(多个用#分隔)/,文章收费:/,收费币种:/,文章性质:/,,,,";
                strName = "Title,Content,KeyWord,Cent,BzType,IsAd,ptype,nid,id,act";
                strType = "text,big,text,num,select,select,hidden,hidden,hidden,hidden";
                //读取实体
                BCW.Model.Detail model = new BCW.BLL.Detail().GetDetail(id);
                if (model.Pics != "")
                {
                    picNum = (Utils.GetStringNum(model.Pics, "#")) + 1;
                }
                strValu = "" + model.Title + "'" + model.Content + "'" + model.KeyWord + "'" + model.Cent + "'" + model.BzType + "'" + model.IsAd + "'" + ptype + "'" + nid + "'" + id + "'text";
                strEmpt = "false,false,true,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",True|正常文章|False|跳到广告,,,,";
            }
            else
            {
                strText = "文章标题:/,文章内容:/,搜索关键字:(多个用#分隔)/,文章收费:/,收费币种:/,文章性质:/,,,,";
                strName = "Title,Content,KeyWord,Cent,BzType,IsAd,ptype,nid,id,act";
                strType = "text,big,text,num,select,select,hidden,hidden,hidden,hidden";
                strValu = "'''0'0'True'" + ptype + "'" + nid + "'" + id + "'text";
                strEmpt = "false,false,true,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",True|正常文章|False|跳到广告,,,,";
            }
            strIdea = "/温馨提示:切换wap2.0可上传附件或续传附件./搜索关键字留空时,系统将根据标题自动生成./";
            strOthe = "";
            if (id != 0)
            {
                strOthe = "确定编辑|reset,classok.aspx,post,1,red|blue";
            }
            else
            {
                strOthe = "确定添加|reset,classok.aspx,post,1,red|blue";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("图片:");
            for (int i = 1; i < 10; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=view&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;num=" + i + "") + "\"><b>" + i + "</b></a> ");
            }
            builder.Append("张");

            builder.Append(Out.Tab("</div>", ""));
            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[0-9]\d*$", "1"));
            if (num > 10)
                num = 10;

            //编辑状态时的显示
            string sUpType = string.Empty;
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            if (id != 0)
            {
                BCW.Model.Detail model = new BCW.BLL.Detail().GetDetail(id);
                if (model.Pics != "")
                {
                    picNum = (Utils.GetStringNum(model.Pics, "#")) + 1;
                }
                sUpType = "续传";
                sText = "文章标题:/,文章内容:/,文章性质:/,搜索关键字:(多个用#分隔)/,文章收费:/,收费币种:/,";
                sName = "Title,Content,IsAd,KeyWord,Cent,BzType,";
                sType = "text,big,select,textarea,num,select,";
                sValu = "" + model.Title + "'" + model.Content + "'" + model.IsAd + "'" + model.KeyWord + "'" + model.Cent + "'" + model.BzType + "'";
                sEmpt = "false,false,True|正常文章|False|跳到广告,true,true,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",";
            }
            else
            {
                sText = "文章标题:/,文章内容:/,文章性质:/,搜索关键字:(多个用#分隔)/,文章收费:/,收费币种:/,";
                sName = "Title,Content,IsAd,KeyWord,Cent,BzType,";
                sType = "text,big,select,textarea,num,select,";
                sValu = "''True''0''";
                sEmpt = "false,false,True|正常文章|False|跳到广告,true,true,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",";
            }
            for (int i = 0; i < num; i++)
            {
                string y = ",";
                if (num == 1)
                {
                    strText = strText + y + "选择" + sUpType + "附图:/";
                }
                else
                {
                    strText = strText + y + "" + sUpType + "第" + (i + 1) + "个附图:/";
                }
                strName = strName + y + "file" + (i + 1);
                strType = strType + y + "file";
                strValu = strValu + "'";
                strEmpt = strEmpt + y;
            }

            strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
            strName = sName + Utils.Mid(strName, 1, strName.Length) + ",ptype,nid,id,act";
            strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
            strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + ptype + "'" + nid + "'" + id + "'text";
            strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
            strIdea = "/温馨提示:搜索关键字留空时,系统将根据标题自动生成./";
            strOthe = "发表文本|上传|reset,classok.aspx,post,2,red|blue|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        if (id != 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(" <a href=\"" + Utils.getUrl("../detail.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            builder.Append(" <a href=\"" + Utils.getUrl("class.aspx?act=delfile&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">删附图(" + picNum + ")</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void InsertPicSoft(string act, int ptype, int nid, int id)
    {
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        if (ptype != 12 && !Utils.Isie())
        {
            Master.Title = "温馨提示";
            builder.Append("<a href=\"classact.aspx?act=" + act + "&amp;ptype=" + ptype + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=2a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
        }
        else
        {
            string TypeName = string.Empty;
            if (ptype == 12)
                TypeName = "图片";
            else
                TypeName = "文件";

            if (id != 0)
            {
                Master.Title = "编辑" + TypeName + "";
                builder.Append(Out.Tab("<div class=\"title\">WAP2.0编辑" + TypeName + "</div>", "编辑" + TypeName + ""));
            }
            else
            {
                Master.Title = "添加" + TypeName + "";
                builder.Append(Out.Tab("<div class=\"title\">WAP2.0添加" + TypeName + "</div>", "添加" + TypeName + ""));
            }

            //上传个数
            int max = Convert.ToInt32(ub.GetSub("UpbMaxFileNum", xmlPath));
            int nn = int.Parse(Utils.GetRequest("nn", "get", 1, @"^[0-9]\d*$", "1"));
            if (nn > max)
                nn = max;
            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[0-9]\d*$", "1"));
            if (num > max)
                num = max;

            builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("" + TypeName + ":");
            for (int i = 1; i <= max; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + act + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;num=" + i + "&amp;nn=" + nn + "") + "\"><b>" + i + "</b></a> ");
            }

            builder.Append(Out.Tab("</div>", ""));

            //编辑状态时的显示
            string sUpType = string.Empty;
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;

            int picNum = 0;
            if (ptype == 12)
            {
                if (id != 0)
                {
                    BCW.Model.Detail model = new BCW.BLL.Detail().GetDetail(id);
                    sUpType = "续传";
                    sText = "" + TypeName + "标题:/," + TypeName + "性质:/,搜索关键字:(多个用#分隔)/,图片收费:/,收费币种:/,生成缩略图:/,宽度:,高度:,";
                    sName = "Title,IsAd,KeyWord,Cent,BzType,ThumbType,width,height,";
                    sType = "text,select,textarea,num,select,select,num,num,";
                    sValu = "" + model.Title + "'" + model.IsAd + "'" + model.KeyWord + "'" + model.Cent + "'" + model.BzType + "'" + ub.GetSub("UpbThumbType", xmlPath) + "'" + ub.GetSub("UpbWidth", xmlPath) + "'" + ub.GetSub("UpbHeight", xmlPath) + "'";
                    sEmpt = "false,True|正常" + TypeName + "|False|跳到广告,true,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",0|不生成|1|保持比例|2|固定尺寸,true,true,";
                }
                else
                {
                    sText = "" + TypeName + "标题:/," + TypeName + "性质:/,搜索关键字:(多个用#分隔)/,图片收费:/,收费币种:/,生成缩略图:/,宽度:,高度:,";
                    sName = "Title,IsAd,KeyWord,Cent,BzType,ThumbType,width,height,";
                    sType = "text,select,textarea,num,select,select,num,num,";
                    sValu = "'True''0'0'" + ub.GetSub("UpbThumbType", xmlPath) + "'" + ub.GetSub("UpbWidth", xmlPath) + "'" + ub.GetSub("UpbHeight", xmlPath) + "'";
                    sEmpt = "false,True|正常" + TypeName + "|False|跳到广告,true,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",0|不生成|1|保持比例|2|固定尺寸,true,true,";
                }
            }
            else
            {
                if (id != 0)
                {
                    BCW.Model.Detail model = new BCW.BLL.Detail().GetDetail(id);
                    if (model.Pics != "")
                    {
                        picNum = (Utils.GetStringNum(model.Pics, "#")) + 1;
                    }
                    sUpType = "续传";
                    sText = "" + TypeName + "标题:/," + TypeName + "描述:/," + TypeName + "性质:/,关键字:(多个用逗号分隔)/,资费说明:/,语言:/,检查说明:/,来源:/,更新说明:/,需要签证:/,下载收费:/,收费币种:/,适用机型(逗号分开):/,";
                    sName = "Title,Content,IsAd,KeyWord,TarText,LanText,SafeText,LyText,UpText,IsVisa,Cent,BzType,Model,";
                    sType = "text,big,select,textarea,text,text,text,text,text,select,num,select,textarea,";
                    sValu = "" + model.Title + "'" + model.Content + "'" + model.IsAd + "'" + model.KeyWord + "'" + model.TarText + "'" + model.LanText + "'" + model.SafeText + "'" + model.LyText + "'" + model.UpText + "'" + model.IsVisa + "'" + model.Cent + "'" + model.BzType + "'" + model.Model + "'";
                    sEmpt = "false,false,True|正常" + TypeName + "|False|跳到广告,true,true,true,true,true,true,0|不显示|1|未知|2|需要|3|不需要,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",true,";
                }
                else
                {
                    sUpType = "上传";
                    sText = "" + TypeName + "标题:/," + TypeName + "描述:/," + TypeName + "性质:/,关键字:(多个用逗号分隔)/,资费说明:/,语言:/,检查说明:/,来源:/,更新说明:/,需要签证:/,下载收费:/,收费币种:/,适用机型(逗号分开):/,";
                    sName = "Title,Content,IsAd,KeyWord,TarText,LanText,SafeText,LyText,UpText,IsVisa,Cent,BzType,Model,";
                    sType = "text,big,select,textarea,text,text,text,text,text,select,num,select,textarea,";
                    sValu = "''True''完全免费'中文'绿色,无病毒'''1'0'0''";
                    sEmpt = "false,false,True|正常" + TypeName + "|False|跳到广告,true,true,true,true,true,true,0|不显示|1|未知|2|需要|3|不需要,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",true,";
                }


                strText += ",是否" + sUpType + "截图:/";
                strName += ",blpic";
                strType += ",select";
                strValu += "'False";
                strEmpt += ",True|上传|False|不上传";
                nn = 0; //截图标识
            }
            for (int i = 0; i < num; i++)
            {
                string y = ",";
                if (ptype == 12)
                {
                    if (num == 1)
                    {
                        strText = strText + y + "第" + (i + 1) + "个图片:/";
                    }
                    else
                    {
                        strText = strText + y + "" + sUpType + "第" + (i + 1) + "个图片:/";
                    }
                    strName = strName + y + "file" + (i + 1);
                    if (!Utils.Isie() || act == "pic2" || act == "info2")
                    {
                        strType = strType + y + "text";
                    }
                    else
                    {
                        strType = strType + y + "file";
                    }
                    strValu = strValu + "'";
                    strEmpt = strEmpt + y;
                }
                else
                {
                    if (num == 1)
                    {
                        strText = strText + y + "选择" + sUpType + "附件:/," + sUpType + "附件描述:/";
                    }
                    else
                    {
                        strText = strText + y + "" + sUpType + "第" + (i + 1) + "个附件:/," + sUpType + "附件描述" + (i + 1) + ":/";
                    }
                    strName = strName + y + "file" + (i + 1) + y + "stext" + (i + 1);
                    if (!Utils.Isie())
                    {
                        strType = strType + y + "text" + y + "text";
                    }
                    else
                    {
                        strType = strType + y + "file" + y + "text";
                    }
                    strValu = strValu + "''";
                    strEmpt = strEmpt + y + y;
                }
            }

            strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,,,";
            strName = sName + Utils.Mid(strName, 1, strName.Length) + ",ptype,nid,id,act,num,nn";
            strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden,hidden,hidden";
            if (ptype == 12)
            {
                string actType = "pic";//采集标识
                if (Utils.Right(act, 1) == "2")
                    actType = "picget";

                strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + ptype + "'" + nid + "'" + id + "'" + actType + "'" + num + "'" + nn + "";
            }
            else
            {
                strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + ptype + "'" + nid + "'" + id + "'soft'" + num + "'" + nn + "";
            }
            strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,,,";
            strIdea = "";

            strIdea = "/温馨提示:搜索关键字留空时,系统将根据标题自动生成../";
            if (ptype == 13)
                strIdea += "多个机型请用逗号分隔,支持UBB,系统智能分析/资费\\语言\\检查\\来源\\更新说明留空时则不显示在前台/";
            if (ptype == 12 && !Utils.Isie())
                strIdea += "支持地址采集上传,切换WAP2.0可以使用上传功能/";

            if (!Utils.Isie() || act == "pic2" || act == "info2")
                strOthe = "上传" + TypeName + "|reset,classok.aspx,post,1,red|blue|blue";
            else
                strOthe = "上传" + TypeName + "|reset,classok.aspx,post,2,red|blue|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            if (id != 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("../detail.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");

                if (ptype == 13)
                {
                    builder.Append(" <a href=\"" + Utils.getUrl("class.aspx?act=delpic&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">删截图(" + picNum + ")</a>");
                }
                builder.Append(" <a href=\"" + Utils.getUrl("class.aspx?act=delfile&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">删文件(" + new BCW.BLL.File().GetCount(id) + ")</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            if (ptype == 12 && Utils.Isie() && act != "pic2" && act != "info2")
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + act + "2&amp;ptype=" + ptype + "&amp;nid=" + nid + "&amp;id=" + id + "") + "\">切换采集上传</a>");
            }
            else
            {
                builder.Append("<a href=\"classact.aspx?act=" + act.Replace("2", "") + "&amp;ptype=" + ptype + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;" + VE + "=2a&amp;" + SID + "=" + Utils.getstrU() + "\">切换WAP2.0上传</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void InsertGoods(int ptype, int nid, int id)
    {
        if (id != 0)
        {
            Master.Title = "编辑商品";
            builder.Append(Out.Tab("<div class=\"title\">WAP2.0编辑商品</div>", "编辑商品"));
        }
        else
        {
            Master.Title = "添加商品";
            builder.Append(Out.Tab("<div class=\"title\">WAP2.0添加商品</div>", "添加商品"));
        }
        int picNum = 0;
        if (!Utils.Isie())
        {
            if (id != 0)
            {
                strText = "商品标题:/,商品配置(支持Ubb):/,商品内容(支持Ubb):/,商品性质:/,付款币种:/,市场价格(元.可保留1-2位小数):/,会员价格(元或币.可保留1-2位小数):/,出售总量(件):/,已出售(件):/,购买人数:/,联系QQ|电话|其它(支持Ubb):/,送货方式:/,邮递邮费(留空则免邮费):/,搜索关键字:(多个用#分隔)/,,,,";
                strName = "Title,Config,Content,IsAd,PostType,CityMoney,VipMoney,StockCount,SellCount,PayCount,Mobile,PayType,PostMoney,KeyWord,ptype,nid,id,act";
                strType = "text,big,big,select,select,text,text,num,num,num,text,select,text,textarea,hidden,hidden,hidden,hidden";
                //读取实体
                BCW.Model.Goods model = new BCW.BLL.Goods().GetGoods(id);
                if (model.Files != "")
                {
                    picNum = (Utils.GetStringNum(model.Files, "#")) + 1;
                }
                strValu = "" + model.Title + "'" + model.Config + "'" + model.Content + "'" + model.IsAd + "'" + model.PostType + "'" + Convert.ToDouble(model.CityMoney) + "'" + Convert.ToDouble(model.VipMoney) + "'" + model.StockCount + "'" + model.SellCount + "'" + model.Paycount + "'" + model.Mobile + "'" + model.PayType + "'" + model.PostMoney + "'" + model.KeyWord + "'" + ptype + "'" + nid + "'" + id + "'shop";
                strEmpt = "false,true,false,True|正常文章|False|跳到广告,0|人民币|1|" + ub.Get("SiteBz") + "|2|" + ub.Get("SiteBz2") + ",false,false,false,false,false,false,0|货到付款|1|当面交易|2|先付款后发货,true,,,,";
            }
            else
            {
                strText = "商品标题:/,商品配置(支持Ubb):/,商品内容(支持Ubb):/,商品性质:/,付款币种:/,市场价格(元.可保留1-2位小数):/,会员价格(元或币.可保留1-2位小数):/,出售总量(件):/,联系QQ|电话|其它(支持Ubb):/,送货方式:/,邮递邮费(留空则免邮费):/,搜索关键字:(多个用#分隔)/,,,,";
                strName = "Title,Config,Content,IsAd,PostType,CityMoney,VipMoney,StockCount,Mobile,PayType,PostMoney,KeyWord,ptype,nid,id,act";
                strType = "text,big,big,select,select,text,text,num,text,select,text,textarea,hidden,hidden,hidden,hidden";
                strValu = "'''True'0'''''0'''" + ptype + "'" + nid + "'" + id + "'shop";
                strEmpt = "false,true,false,True|正常商品|False|跳到广告,0|人民币|1|" + ub.Get("SiteBz") + "|2|" + ub.Get("SiteBz2") + ",false,false,false,false,0|货到付款|1|当面交易|2|先付款后发货,true,,,,";

            }
            strIdea = "/";
            strOthe = "";
            if (id != 0)
            {
                strOthe = "确定编辑|reset,classok.aspx,post,1,red|blue";
            }
            else
            {
                strOthe = "确定添加|reset,classok.aspx,post,1,red|blue";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("图片:");
            for (int i = 1; i < 10; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=view&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;num=" + i + "") + "\"><b>" + i + "</b></a> ");
            }
            builder.Append("张");

            builder.Append(Out.Tab("</div>", ""));
            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[0-9]\d*$", "1"));
            if (num > 10)
                num = 10;

            //编辑状态时的显示
            string sUpType = string.Empty;
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            if (id != 0)
            {
                BCW.Model.Goods model = new BCW.BLL.Goods().GetGoods(id);
                if (model.Files != "")
                {
                    picNum = (Utils.GetStringNum(model.Files, "#")) + 1;
                }
                sUpType = "续传";
                sText = "商品标题:/,商品配置(支持Ubb):/,商品内容(支持Ubb):/,商品性质:/,付款币种:/,市场价格(元.可保留1-2位小数):/,会员价格(元.可保留1-2位小数):/,出售总量(件):/,已出售(件):/,购买人数:/,联系QQ|电话|其它(支持Ubb):/,送货方式:/,邮递邮费(留空则免邮费):/,搜索关键字:(多个用#分隔)/,";
                sName = "Title,Config,Content,IsAd,PostType,CityMoney,VipMoney,StockCount,SellCount,PayCount,Mobile,PayType,PostMoney,KeyWord,";
                sType = "text,big,big,select,select,text,text,num,num,num,text,select,text,textarea,";
                sValu = "" + model.Title + "'" + model.Config + "'" + model.Content + "'" + model.IsAd + "'" + model.PostType + "'" + Convert.ToDouble(model.CityMoney) + "'" + Convert.ToDouble(model.VipMoney) + "'" + model.StockCount + "'" + model.SellCount + "'" + model.Paycount + "'" + model.Mobile + "'" + model.PayType + "'" + model.PostMoney + "'" + model.KeyWord + "'";
                sEmpt = "false,true,false,True|正常文章|False|跳到广告,0|人民币|1|" + ub.Get("SiteBz") + "|2|" + ub.Get("SiteBz2") + ",false,false,false,false,false,false,0|货到付款|1|当面交易|2|先付款后发货,true,";
            }
            else
            {
                sText = "商品标题:/,商品配置(支持Ubb):/,商品内容(支持Ubb):/,商品性质:/,付款币种:/,市场价格(元.可保留1-2位小数):/,商城特价(元.可保留1-2位小数):/,出售总量(件):/,联系QQ|电话|其它(支持Ubb):/,送货方式:/,邮递邮费(留空则免邮费):/,搜索关键字:(多个用#分隔)/,";
                sName = "Title,Config,Content,IsAd,PostType,CityMoney,VipMoney,StockCount,Mobile,PayType,PostMoney,KeyWord,";
                sType = "text,big,big,select,select,text,text,num,text,select,text,textarea,";
                sValu = "'''True'0'''''0'''";
                sEmpt = "false,true,false,True|正常商品|False|跳到广告,0|人民币|1|" + ub.Get("SiteBz") + "|2|" + ub.Get("SiteBz2") + ",false,false,false,false,0|货到付款|1|当面交易|2|先付款后发货,true,";
            }
            for (int i = 0; i < num; i++)
            {
                string y = ",";
                if (num == 1)
                {
                    strText = strText + y + "选择" + sUpType + "附图:/";
                }
                else
                {
                    strText = strText + y + "" + sUpType + "第" + (i + 1) + "个附图:/";
                }
                strName = strName + y + "file" + (i + 1);
                strType = strType + y + "file";
                strValu = strValu + "'";
                strEmpt = strEmpt + y;
            }

            strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
            strName = sName + Utils.Mid(strName, 1, strName.Length) + ",ptype,nid,id,act";
            strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
            strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + ptype + "'" + nid + "'" + id + "'shop";
            strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
            strIdea = "/";
            strOthe = "发表文本|上传|reset,classok.aspx,post,2,red|blue|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
          
        }
        if (id != 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:搜索关键字留空时,系统将根据标题自动生成.<br />邮递邮费填写例子:15|快递|20|EMS<br />");
            builder.Append("<a href=\"" + Utils.getUrl("../shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            builder.Append(" <a href=\"" + Utils.getUrl("class.aspx?act=delfile&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">删附图(" + picNum + ")</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
}