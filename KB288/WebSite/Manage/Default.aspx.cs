using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.Data;
using TPR3.Common;
public partial class Manage_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "后台管理中心";
        string Tab = "|";
        if (Utils.Isie())
        {
            int ve = Utils.ParseInt(Utils.Left(Utils.getstrVe(), 1));
            ub xml = new ub();
            string xmlPath = "/Controls/skin.xml";
            xml.ReloadSub(xmlPath); //加载配置
            if (ve < 2 || ve > 10 || xml.dss["body_font_size" + ve + ""].ToString() == "0")
            {
                ve = 2;
            }
            Tab = "<span style=\"color:#" + xml.dss["div_title_color" + ve + ""] + ";\"> ~ </span>";
        }

        //升级提示
        if (Request["act"] == "no")
        {
            DataCache.RemoveCache("LIGHTCMSUPDATE");
        }
        else
        {
            object lightcmsupdate = DataCache.GetCache("LIGHTCMSUPDATE");
            if (lightcmsupdate != null)
            {
                builder.Append("发现新版本:" + lightcmsupdate.ToString() + "<br />");
                builder.Append("<a href=\"" + Utils.getUrl("update.aspx") + "\">立即升级&gt;&gt;</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=no") + "\">知道了</a><br />");
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("后台管理中心");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Man/default.aspx") + "\">【系统快捷管理】</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //二级管理员权限
        int ManageId = new BCW.User.Manage().IsManageLogin();
        string ManIDS = "#" + ub.Get("SiteManIDS") + "#";
        if (ManIDS.Contains("#" + ManageId + "#"))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx") + "\">设计中心</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">书城管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx") + "\">版块管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx") + "\">黑名管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx") + "\">版主管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx") + "\">空间留言</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx") + "\">帖子管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx") + "\">回帖管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">圈子管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">聊吧管理</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {


            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xml/default.aspx") + "\">系统参数</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("health.aspx") + "\">健康监测</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xml/skinset.aspx") + "\">皮肤模板</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("manage.aspx") + "\">后台帐号</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("~~主站功能~~");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx") + "\">设计中心</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">书城管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx") + "\">版块管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx") + "\">评论管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("link.aspx") + "\">友链系统</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("vbook.aspx") + "\">留言管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("modata.aspx") + "\">机型管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("votes.aspx") + "\">投票系统</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx") + "\">广告管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx") + "\">商品订单</a><br />");
            builder.Append( "<a href=\"" + Utils.getUrl( "Mobile/Default.aspx" ) + "\">APP管理</a><br />" );
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("~~快速功能~~");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("collecitem.aspx") + "\">采集管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("smart.aspx") + "\">智能调用</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?ptype=11") + "\">添加文章</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?ptype=12") + "\">上传图片</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?ptype=14") + "\">添加商品</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?ptype=13") + "\">上传文件</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("~~社区功能~~");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">会员管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx") + "\">版主管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx") + "\">黑名管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx") + "\">日志管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx") + "\">空间留言</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">消息内线</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx") + "\">帖子管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx") + "\">回帖管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx") + "\">广播喇叭</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("game/default.aspx") + "\">游戏管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx") + "\">会员动态</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx") + "\">游戏闲聊</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚恋管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">日记管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx") + "\">相册管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">圈子管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">聊吧管理</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("~~更多功能~~");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("app/databackup.aspx") + "\">数据管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("app/stat.aspx") + "\">网站流量</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx") + "\">社区统计</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("app/cache.aspx") + "\">缓存管理</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("app/browser.aspx") + "\">绿色浏览</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("login.aspx?ac=exit") + "\">安全退出</a>" + Tab + "");
            builder.Append("<a href=\"" + Utils.getUrl("app/default.aspx") + "\">更多服务»</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = "输入地址安全浏览:/,,";
            string strName = "gurl";
            string strType = "text";
            string strValu = "http://";
            string strEmpt = "false";
            string strIdea = "";
            string strOthe = "GO,app/browser.aspx,get,3,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("系统:" + ub.Get("SiteName") + "<br />");
        builder.Append("" +BCW.User.AdminCall.AdminUBB("[@经典时间]") + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("[<a href=\"" + Utils.getUrl("update.aspx") + "\">系统自助升级</a>]");
        //builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append("<img src=\"/files/logow.gif\" alt=\"hello\" onerror=\"this.src='/files/sys/admin.gif'\" />");  






    
    }






}