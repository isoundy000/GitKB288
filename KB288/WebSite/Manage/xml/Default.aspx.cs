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

public partial class Manage_xml_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "系统配置中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("系统配置中心");
        builder.Append(Out.Tab("</div>", ""));
        string fName = string.Empty;
        string fUrl = string.Empty;
        if (Utils.GetDomain().Contains("ky288") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
        {
            fName = ",抽奖设置,滚动设置,内部转币ID,默认闲聊管理,勋章管理员";
            fUrl = ",lotteryset,freshset,centrole,spkadminset,medalset";
        }
        if (Utils.GetDomain().Contains("6lehe") || Utils.GetDomain().Contains("3gyol") || Utils.GetDomain().Contains("127.0.0.6"))
        {
            fName += ",查币设置";
            fUrl += ",checkgoldset";
        }

        fName += ",新闻采集,系统号设置";
        fUrl += ",newsset,robotset";

        string strName = @"系统基本
                             ,皮肤模板
                             ,社区系统
                             ,注册系统
                             ,消息系统
                             ,邮箱系统
                             ,VIP系统
                             ,积分系统
                             ,上传系统
                             ,友链系统
                             ,留言系统
                             ,投票系统
                             ,前台相关
                             ,订单系统
                             ,日记系统
                             ,相册系统
                             ,圈子系统
                             ,聊吧系统
                             ,广播系统
                             ,社区商城
                             ,婚姻系统
                             ,等级设置 
                             ,金融系统
                             ,游戏配置 
                             ,高手系统 
                             ,非验证会员 
                             ,授权码配置" + fName + "";

        string strUrl = @"config
                            ,skinset
                            ,bbsset
                            ,regset
                            ,guestset
                            ,emailset
                            ,vipset
                            ,centset
                            ,upset
                            ,linkset
                            ,vbookset
                            ,votesset
                            ,frontset
                            ,buylistset
                            ,diaryset
                            ,albumsset
                            ,groupset
                            ,chatset
                            ,networkset
                            ,bbsshopset
                            ,marryset
                            ,levenset 
                            ,financeset
                            ,game
                            ,gsset
                            ,verifyset
                            ,keyset" + fUrl + "";


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string[] sName = strName.Split(",".ToCharArray());
        string[] sUrl = strUrl.Split(",".ToCharArray());
        //总记录数
        recordCount = sName.Length;

        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        for (int i = 0; i < sName.Length; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                if ((k + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl(sUrl[i].Trim().ToString() + ".aspx") + "\">" + sName[i].Trim().ToString() + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
}
