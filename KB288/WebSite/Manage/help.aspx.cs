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

public partial class Manage_help : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "系统链接帮助";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("系统链接帮助");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("常用内置链接：<br />");
        builder.Append("留言本:/vbook.aspx<br />");
        builder.Append("友情链接:/link.aspx<br />");
        builder.Append("投票链接:/votes.aspx<br />");
        builder.Append("机型链接:/model.aspx<br />");
        builder.Append("站内搜索:/search.aspx<br />");
        builder.Append("订单链接:/myshop.aspx<br />");
        builder.Append("天气链接:/utility/weather.aspx<br />");
        builder.Append("广播链接:/bbs/network.aspx<br />");
        builder.Append("聊吧链接:/bbs/chat.aspx<br />");
        builder.Append("日记链接:/bbs/diary.aspx<br />");
        builder.Append("相册链接:/bbs/albums.aspx<br />");
        builder.Append("圈子链接:/bbs/group.aspx<br />");
        builder.Append("论坛链接:/bbs/forum.aspx<br />");
        builder.Append("社区链接:/bbs/default.aspx<br />");
        builder.Append("游戏链接:/bbs/game/default.aspx<br />");
        builder.Append("动态链接:/bbs/action.aspx<br />");
        builder.Append("在线链接:/bbs/online.aspx<br />");
        builder.Append("签到链接:/bbs/signin.aspx<br />");
        builder.Append("排行链接:/bbs/usertop.aspx<br />");
        builder.Append("帖子导航:/bbs/sktype.aspx<br />");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示:<br />当地址为内网地址时，身份识别由系统自动加入，请不要加任何身份识别在链接上，全站都如此<br />更多内部标签请使用“添加菜单-智能调用”或查看官方网站里面的程序帮助说明");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
