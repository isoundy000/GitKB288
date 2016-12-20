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

public partial class Manage_xml_frontset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "前台相关设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/front.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string TextCover = Utils.GetRequest("TextCover", "post", 2, @"^[0-1]$", "文章列表封面选择错误");
                string PicCover = Utils.GetRequest("PicCover", "post", 2, @"^[0-1]$", "图片列表封面选择错误");
                string FileCover = Utils.GetRequest("FileCover", "post", 2, @"^[0-1]$", "文件列表封面选择错误");
                string ShopCover = Utils.GetRequest("ShopCover", "post", 2, @"^[0-1]$", "商品列表封面选择错误");
                string TextDetailNum = Utils.GetRequest("TextDetailNum", "post", 2, @"^[1-9]\d*$", "文章内容每页字数填写错误");
                string PicListNum = Utils.GetRequest("PicListNum", "post", 2, @"^[1-9]\d*$", "图片列表显示条数填写错误");
                string FileIsUser = Utils.GetRequest("FileIsUser", "post", 2, @"^[0-2]$", "文件下载限制选择错误");
                string KCset = Utils.GetRequest("KCset", "post", 3, @"^[^\#]{1,200}(?:\#[^\#]{1,200}){0,2000}$", "快速评论设置错误");
                string GNset = Utils.GetRequest("GNset", "post", 2, @"^[0-1]$", "功能区域显示选择错误");
                string CKset = Utils.GetRequest("CKset", "post", 2, @"^[0-1]$", "评论区域显示选择错误");
                string CommentLength = Utils.GetRequest("CommentLength", "post", 2, @"^([1-9][0-9]|[1-4][0-9][0-9]|500)$", "评论字值限10-500");
                string CommentIsUser = Utils.GetRequest("CommentIsUser", "post", 2, @"^[0-2]$", "评论发表限制选择错误");
                string CommentExpir = Utils.GetRequest("CommentExpir", "post", 2, @"^[0-9]\d*$", "评论防刷时间填写错误");
                string CommentAdmin = Utils.GetRequest("CommentAdmin", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "评论管理员ID请用#分开，可以留空");

                xml.dss["FtTextCover"] = TextCover;
                xml.dss["FtPicCover"] = PicCover;
                xml.dss["FtFileCover"] = FileCover;
                xml.dss["FtShopCover"] = ShopCover;
                xml.dss["FtTextDetailNum"] = TextDetailNum;
                xml.dss["FtPicListNum"] = PicListNum;
                xml.dss["FtFileIsUser"] = FileIsUser;
                xml.dss["FtKCset"] = KCset;
                xml.dss["FtGNset"] = GNset;
                xml.dss["FtCKset"] = CKset;
                xml.dss["FtCommentLength"] = CommentLength;
                xml.dss["FtCommentIsUser"] = CommentIsUser;
                xml.dss["FtCommentExpir"] = CommentExpir;
                xml.dss["FtCommentAdmin"] = CommentAdmin;
            }
            else
            {
                xml.dss["FtTextListTop"] = Request.Form["TextListTop"];
                xml.dss["FtTextListFoot"] = Request.Form["TextListFoot"];
                xml.dss["FtPicListTop"] = Request.Form["PicListTop"];
                xml.dss["FtPicListFoot"] = Request.Form["PicListFoot"];
                xml.dss["FtFileListTop"] = Request.Form["FileListTop"];
                xml.dss["FtFileListFoot"] = Request.Form["FileListFoot"];
                xml.dss["FtShopListTop"] = Request.Form["ShopListTop"];
                xml.dss["FtShopListFoot"] = Request.Form["ShopListFoot"];
                xml.dss["FtTextDetailTop"] = Request.Form["TextDetailTop"];
                xml.dss["FtTextDetailFoot"] = Request.Form["TextDetailFoot"];
                xml.dss["FtPicDetailTop"] = Request.Form["PicDetailTop"];
                xml.dss["FtPicDetailFoot"] = Request.Form["PicDetailFoot"];
                xml.dss["FtFileDetailTop"] = Request.Form["FileDetailTop"];
                xml.dss["FtFileDetailFoot"] = Request.Form["FileDetailFoot"];
                xml.dss["FtShopDetailTop"] = Request.Form["ShopDetailTop"];
                xml.dss["FtShopDetailFoot"] = Request.Form["ShopDetailFoot"];

            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("前台相关设置", "设置成功，正在返回..", Utils.getUrl("frontset.aspx?ptype=" + ptype + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "前台相关设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("基本设置|");
                builder.Append("<a href=\"" + Utils.getUrl("frontset.aspx?ptype=1") + "\">广告与调用</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("frontset.aspx?ptype=0") + "\">基本设置</a>");
                builder.Append("|广告与调用");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "文章列表封面:/,图片列表封面:/,文件列表封面:/,商品列表封面:/,文章内容每页字数:/,图片列表显示条数:/,文件下载限制:/,快速评论设置:/,功能区域显示:/,评论区域显示:/,评论最多字符(500上限):/,评论限制:/,评论防刷时间(秒):/,评论管理员(多个ID用#隔开):/";
                string strName = "TextCover,PicCover,FileCover,ShopCover,TextDetailNum,PicListNum,FileIsUser,KCset,GNset,CKset,CommentLength,CommentIsUser,CommentExpir,CommentAdmin";
                string strType = "select,select,select,select,num,num,select,text,select,select,num,select,num,text";
                string strValu = "" + xml.dss["FtTextCover"] + "'" + xml.dss["FtPicCover"] + "'" + xml.dss["FtFileCover"] + "'" + xml.dss["FtShopCover"] + "'" + xml.dss["FtTextDetailNum"] + "'" + xml.dss["FtPicListNum"] + "'" + xml.dss["FtFileIsUser"] + "'" + xml.dss["FtKCset"] + "'" + xml.dss["FtGNset"] + "'" + xml.dss["FtCKset"] + "'" + xml.dss["FtCommentLength"] + "'" + xml.dss["FtCommentIsUser"] + "'" + xml.dss["FtCommentExpir"] + "'" + xml.dss["FtCommentAdmin"] + "";
                string strEmpt = "0|显示|1|不显示,0|显示|1|不显示,0|显示|1|不显示,0|显示|1|不显示,false,false,0|不作限制|1|仅限会员|2|禁止下载,true,0|显示|1|不显示,0|显示|1|不显示,false,0|不作限制|1|仅限会员|2|禁止评论,false,true";
                string strIdea = "/";
                string strOthe = "确定修改|reset,frontset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />快速评论设置例子:支持#反对#路过<br />同时可以是图片地址,如:[IMG]1.gif[/IMG]#[IMG]2.gif[/IMG]<br />评论选项还支持自定义个数,留空则不显示");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "文章列表顶部Ubb:/,文章列表底部Ubb:/,图片列表顶部Ubb:/,图片列表底部Ubb:/,文件列表顶部Ubb:/,文件列表底部Ubb:/,商品列表顶部Ubb:/,商品列表底部Ubb:/,文章内容顶部Ubb:/,文章内容底部Ubb:/,图片内容顶部Ubb:/,图片内容底部Ubb:/,文件内容顶部Ubb:/,文件内容底部Ubb:/,商品内容顶部Ubb:/,商品内容底部Ubb:/,";
                string strName = "TextListTop,TextListFoot,PicListTop,PicListFoot,FileListTop,FileListFoot,ShopListTop,ShopListFoot,TextDetailTop,TextDetailFoot,PicDetailTop,PicDetailFoot,FileDetailTop,FileDetailFoot,ShopDetailTop,ShopDetailFoot,ptype";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden";
                string strValu = "" + xml.dss["FtTextListTop"] + "'" + xml.dss["FtTextListFoot"] + "'" + xml.dss["FtPicListTop"] + "'" + xml.dss["FtPicListFoot"] + "'" + xml.dss["FtFileListTop"] + "'" + xml.dss["FtFileListFoot"] + "'" + xml.dss["FtShopListTop"] + "'" + xml.dss["FtShopListFoot"] + "'" + xml.dss["FtTextDetailTop"] + "'" + xml.dss["FtTextDetailFoot"] + "'" + xml.dss["FtPicDetailTop"] + "'" + xml.dss["FtPicDetailFoot"] + "'" + xml.dss["FtFileDetailTop"] + "'" + xml.dss["FtFileDetailFoot"] + "'" + xml.dss["FtShopDetailTop"] + "'" + xml.dss["FtShopDetailFoot"] + "'" + ptype + "";
                string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,frontset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />顶部一般作为调用随机广告，底部可以调用某些栏目列表记录和广告或者其它UBB<br />系统提供强大的智能调用，能让你调用出各种界面与特效<br />调用代码请在智能调用功能中生成，智能调用代码可以和UBB混用");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
