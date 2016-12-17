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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class cplist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/cplist.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "bm":
                BmPage();
                break;
            case "list":
                ListPage();
                break;
            case "view":
                ViewPage();
                break;
            case "hz":
                HzPage();
                break;
            case "listb":
                ListBPage();
                break;
            case "viewb":
                ViewBPage();
                break;
            case "st":
                STPage();
                break;
            case "stview":
                StViewPage();
                break;
            default:
                ReloadPage();
                break;
        }
        builder.Append(new Out().foot());
    }

    private void ReloadPage()
    {
        new Out().head(Utils.ForWordType("最全最早的资料"), "left");
       
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("【最全最早的资料】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=bm&amp;backurl=" + Utils.getPage(0)) + "\">快速报码</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=33&amp;backurl=" + Utils.getPage(0)) + "\">历史开奖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=6&amp;backurl=" + Utils.getPage(0)) + "\">香港挂牌</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=8&amp;backurl=" + Utils.getPage(0)) + "\">高手解牌</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=9&amp;backurl=" + Utils.getPage(0)) + "\">综合资料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=11&amp;backurl=" + Utils.getPage(0)) + "\">九龙内幕</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=25&amp;backurl=" + Utils.getPage(0)) + "\">报刊大全</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=31&amp;backurl=" + Utils.getPage(0)) + "\">彩图诗句</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=22&amp;backurl=" + Utils.getPage(0)) + "\">外站猛料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=50&amp;backurl=" + Utils.getPage(0)) + "\">高手猛料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=12&amp;backurl=" + Utils.getPage(0)) + "\">曾道人料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=13&amp;backurl=" + Utils.getPage(0)) + "\">黄大仙网</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=14&amp;backurl=" + Utils.getPage(0)) + "\">白小姐网</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=15&amp;backurl=" + Utils.getPage(0)) + "\">赛马会料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=16&amp;backurl=" + Utils.getPage(0)) + "\">香港赌圣</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=17&amp;backurl=" + Utils.getPage(0)) + "\">天线宝宝</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=18&amp;backurl=" + Utils.getPage(0)) + "\">惠泽社群</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=19&amp;backurl=" + Utils.getPage(0)) + "\">蓝月亮网</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=20&amp;backurl=" + Utils.getPage(0)) + "\">管家婆网</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=21&amp;backurl=" + Utils.getPage(0)) + "\">各坛精料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=23&amp;backurl=" + Utils.getPage(0)) + "\">综合统计</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=24&amp;backurl=" + Utils.getPage(0)) + "\">站推荐料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=26&amp;backurl=" + Utils.getPage(0)) + "\">精选玄机</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=27&amp;backurl=" + Utils.getPage(0)) + "\">高手解迷</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=41&amp;backurl=" + Utils.getPage(0)) + "\">博发世家</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=42&amp;backurl=" + Utils.getPage(0)) + "\">十虎权威</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=43&amp;backurl=" + Utils.getPage(0)) + "\">天顺总坛</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=46&amp;backurl=" + Utils.getPage(0)) + "\">马会绝杀</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=68&amp;backurl=" + Utils.getPage(0)) + "\">彩坛至尊</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=69&amp;backurl=" + Utils.getPage(0)) + "\">任我發料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=29&amp;backurl=" + Utils.getPage(0)) + "\">平肖平码</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=38&amp;backurl=" + Utils.getPage(0)) + "\">神奇公式</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=36&amp;backurl=" + Utils.getPage(0)) + "\">高级彩图</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=37&amp;backurl=" + Utils.getPage(0)) + "\">幸运彩图</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=4&amp;backurl=" + Utils.getPage(0)) + "\">图库a区</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=5&amp;backurl=" + Utils.getPage(0)) + "\">图库b区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=32&amp;backurl=" + Utils.getPage(0)) + "\">属性参考</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=list&amp;id=34&amp;backurl=" + Utils.getPage(0)) + "\">全年资料</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, "", "ID错误"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "1"));
        string str = new BCW.Service.GetCplist().GetCplistXML(id, page);
        if (!string.IsNullOrEmpty(str))
        {
            string[] temp = Regex.Split(str, "!@#!@#");
            new Out().head(Utils.ForWordType(temp[0].ToString()), "left");
            if (ub.GetSub("CPListTop", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("CPListTop", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(temp[0].ToString());
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(temp[1].ToString());
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new Out().head(Utils.ForWordType("操作超时"), "left");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("CPListFoot", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("CPListFoot", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回资料首页</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        int pid = int.Parse(Utils.GetRequest("pid", "get", 2, "", "ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, "", "ID错误"));
        int i = int.Parse(Utils.GetRequest("i", "get", 1, @"^[0-9]\d*$", "0"));
        int o = int.Parse(Utils.GetRequest("o", "get", 1, @"^[0-9]\d*$", "0"));
        if (i == 0)
        {
            i = int.Parse(Utils.GetRequest("ii", "get", 1, @"^[0-9]\d*$", "0"));
            if (i >= 1)
                i = i - 1;
        }
        string str = new BCW.Service.GetCplist().GetCplistXML2(pid, id, i, o);
        if (!string.IsNullOrEmpty(str))
        {
            string[] temp = Regex.Split(str, "!@#!@#");
            new Out().head(Utils.ForWordType(temp[0].ToString()), "left");
            if (ub.GetSub("CPDetailTop", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("CPDetailTop", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(temp[1].ToString());
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new Out().head(Utils.ForWordType("操作超时"), "left");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("CPDetailFoot", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("CPDetailFoot", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回资料首页</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void BmPage()
    {
        new Out().head(Utils.ForWordType("WAP最快报码室"), "left");
        string str = new BCW.Service.GetCplist().GetCplistXML3();
        if (!string.IsNullOrEmpty(str))
        {
            if (ub.GetSub("CPBmTop", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("CPBmTop", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【最快报码室】<a href=\"" + Utils.getUrl("cplist.aspx?act=bm") + "\">刷新</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(str);
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", ""));
        }
        if (ub.GetSub("CPBmFoot", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("CPBmFoot", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回资料首页</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void HzPage()
    {
        new Out().head(Utils.ForWordType("免费资料24h更新"), "left");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("免费资料24h更新");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=20&amp;backurl=" + Utils.getPage(0)) + "\">原版正料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=29&amp;backurl=" + Utils.getPage(0)) + "\">正版挂牌</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=251&amp;Nclassid=258&amp;backurl=" + Utils.getPage(0)) + "\">彩色图库</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=91&amp;backurl=" + Utils.getPage(0)) + "\">互联网料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=50&amp;backurl=" + Utils.getPage(0)) + "\">玄机爆料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=194&amp;backurl=" + Utils.getPage(0)) + "\">文字彩图</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=49&amp;backurl=" + Utils.getPage(0)) + "\">惠泽社群</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;Nclassid=44&amp;backurl=" + Utils.getPage(0)) + "\">金明世家</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=273&amp;backurl=" + Utils.getPage(0)) + "\">博彩傳芳</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=58&amp;backurl=" + Utils.getPage(0)) + "\">公式规律</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=32&amp;backurl=" + Utils.getPage(0)) + "\">平码平肖</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=271&amp;backurl=" + Utils.getPage(0)) + "\">高手杀料</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=31&amp;backurl=" + Utils.getPage(0)) + "\">精版资料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=66&amp;backurl=" + Utils.getPage(0)) + "\">会员集合</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=28&amp;backurl=" + Utils.getPage(0)) + "\">极限统计</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=10&amp;nclassid=28&amp;backurl=" + Utils.getPage(0)) + "\">历史挂牌</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=43&amp;nclassid=45&amp;backurl=" + Utils.getPage(0)) + "\">全年资料</a>|<a href=\"" + Utils.getUrl("cplist.aspx?act=listb&amp;classid=43&amp;nclassid=48&amp;backurl=" + Utils.getPage(0)) + "\">开奖纪录</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListBPage()
    {
        int classid = int.Parse(Utils.GetRequest("classid", "get", 2, "", "ID错误"));
        int nclassid = int.Parse(Utils.GetRequest("nclassid", "get", 2, "", "ID错误"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "1"));
        string str = new BCW.Service.GetCplist2().GetCplist2XML(classid, nclassid, page);
        if (!string.IsNullOrEmpty(str))
        {
            string[] temp = Regex.Split(str, "!@#!@#");
            new Out().head(Utils.ForWordType(temp[0].ToString()), "left");
            if (ub.GetSub("CPListTop", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("CPListTop", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div class=\"title\">" + temp[0].ToString() + "</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(temp[1].ToString());
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new Out().head(Utils.ForWordType("操作超时"), "left");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("CPListFoot", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("CPListFoot", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        //builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=hz&amp;backurl=" + Utils.getPage(0) + "") + "\">返回资料首页</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewBPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, "", "ID错误"));
        int nclassid = int.Parse(Utils.GetRequest("nclassid", "get", 2, "", "ID错误"));
        int p = int.Parse(Utils.GetRequest("p", "get", 1, @"^[1-9]\d*$", "1"));
        string str = new BCW.Service.GetCplist2().GetCplist2XML2(id, nclassid, p);
        if (!string.IsNullOrEmpty(str))
        {
            string[] temp = Regex.Split(str, "!@#!@#");
            new Out().head(Utils.ForWordType(temp[0].ToString()), "left");
            if (ub.GetSub("CPDetailTop", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("CPDetailTop", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(temp[1].ToString());
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new Out().head(Utils.ForWordType("操作超时"), "left");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("CPDetailFoot", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("CPDetailFoot", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        //builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=hz&amp;backurl=" + Utils.getPage(0) + "") + "\">返回资料首页</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void STPage()
    {
        new Out().head(Utils.ForWordType("神童免费资料区"), "left");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("神童免费资料区");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/8xao.asp&amp;backurl=" + Utils.getPage(0)) + "\">镇站资料单双四禽</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/jia.asp&amp;backurl=" + Utils.getPage(0)) + "\">大仙预测野兽家畜</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/2t2w.asp&amp;backurl=" + Utils.getPage(0)) + "\">传真内部二头二尾</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/5mping.asp&amp;backurl=" + Utils.getPage(0)) + "\">管家婆五号中平特</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/58.asp&amp;backurl=" + Utils.getPage(0)) + "\">精准正版波色诗</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/5000.asp&amp;backurl=" + Utils.getPage(0)) + "\">5000做本月赚2万</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/nao.asp&amp;backurl=" + Utils.getPage(0)) + "\">脑筋转弯</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/3we.asp&amp;backurl=" + Utils.getPage(0)) + "\">金牌三尾</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/shatou.asp&amp;backurl=" + Utils.getPage(0)) + "\">至尊杀头</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/257xao.asp&amp;backurl=" + Utils.getPage(0)) + "\">二五七禽</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/sa3xao.asp&amp;backurl=" + Utils.getPage(0)) + "\">绝杀三禽</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=stview&amp;gourl=/ads/ad/3tou.asp&amp;backurl=" + Utils.getPage(0)) + "\">大仙三头</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void StViewPage()
    {
        string gourl = Utils.GetRequest("gourl", "get", 2, @"^[\s\S]{5,}$", "地址错误");
        string str = new BCW.Service.GetCplist2().GetCplist2XML3(gourl);
        if (!string.IsNullOrEmpty(str))
        {
            string[] temp = Regex.Split(str, "!@#!@#");
            new Out().head(Utils.ForWordType(temp[0].ToString()), "left");
            if (ub.GetSub("CPDetailTop", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("CPDetailTop", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(temp[1].ToString());
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new Out().head(Utils.ForWordType("操作超时"), "left");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("CPDetailFoot", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("CPDetailFoot", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        //builder.Append("<a href=\"" + Utils.getUrl("cplist.aspx?act=st&amp;backurl=" + Utils.getPage(0) + "") + "\">返回资料首页</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回网站首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    
    }
}
