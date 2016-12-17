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

public partial class bbs_game_Six : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/six.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("SixStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "bm":
                BmPage();
                break;
            case "fresh":
                FreshPage();
                break;
            case "case":
                CasePage();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "casepost":
                CasePostPage();
                break;
            case "add":
                AddPage();
                break;
            case "save":
                SavePage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "cardrd":
                CardRdPage();
                break;
            case "cardbs":
                CardBsPage();
                break;
            case "cardsx":
                CardSxPage();
                break;
            case "cardwx":
                CardWxPage();
                break;
            case "cardso":
                CardSoPage();
                break;
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("SixName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("SixLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(11));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;虚拟");
        builder.Append(Out.Tab("</div>", "<br />"));

        string TopUbb = ub.GetSub("SixTopUbb", xmlPath);
        if (TopUbb != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(TopUbb) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        LHC.Model.VoteNo model = new LHC.BLL.VoteNo().GetVoteNoNew(0);
        if (model != null)
        {
            if (model.ExTime > DateTime.Now)
            {
                builder.Append("第" + model.qiNo + "期投注进行中,距离开奖时间还有" + DT.DateDiff(DateTime.Now, model.ExTime) + ",开奖前" + ub.GetSub("SixBeforeMin", xmlPath) + "分钟停止下注,截至目前已下注" + model.payCent + "" + ub.Get("SiteBz") + "(" + model.payCount + "注)");
                if (Isbz())
                    builder.Append("/" + model.payCent2 + "" + ub.Get("SiteBz2") + "(" + model.payCount2 + "注)");
            }
            else
            {
                builder.Append("请等待管理员开奖。。。");
            }
        }
        else
        {
            builder.Append("请等待管理员开通下期。。。");
        }
        LHC.Model.VoteNo model2 = new LHC.BLL.VoteNo().GetVoteNoNew(1);
        if (model2 != null)
        {
            //builder.Append("<br />上期" + model2.qiNo + "期,平.码" + model2.pNum1 + "," + model2.pNum2 + "," + model2.pNum3 + "," + model2.pNum4 + "," + model2.pNum5 + "," + model2.pNum6 + "特.码" + model2.sNum + "");

            builder.Append("<br />上期" + model2.qiNo + "期开奖结果↓↓<br />");
            builder.Append("平码:<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.pNum1 + "") + "\">" + model2.pNum1 + "</a>-<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.pNum2 + "") + "\">" + model2.pNum2 + "</a>-<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.pNum3 + "") + "\">" + model2.pNum3 + "</a>-<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.pNum4 + "") + "\">" + model2.pNum4 + "</a>-<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.pNum5 + "") + "\">" + model2.pNum5 + "</a>-<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.pNum6 + "") + "\">" + model2.pNum6 + "</a><br />");
            builder.Append("特码:<a href=\"" + Utils.getUrl("six.aspx?act=cardso&amp;info=ok&amp;num=" + model2.sNum + "") + "\">" + model2.sNum + "</a>");
            builder.Append("(" + GetBS(model2.sNum).Replace("波", "") + "/" + GetWX(model2.sNum) + "/" + GetDX(model2.sNum) + "/" + GetSX(model2.sNum) + "/" + GetDS(model2.sNum) + ")");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【投注项目】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=1") + "\">特.码</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=2") + "\">单双</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=3") + "\">大小</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=4") + "\">波色</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=5") + "\">特肖</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=6") + "\">家野</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=7") + "\">特尾</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=8") + "\">特头</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=9") + "\">合数大小</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=10") + "\">合数单双</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=11") + "\">平特肖</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=12") + "\">独平</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=13") + "\">平特尾</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=14") + "\">五行</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=15") + "\">六肖</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=16") + "\">半波</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=17") + "\">特连</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=18") + "\">平三中三</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=19") + "\">三中二</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=20") + "\">二中二</a>");


            //builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=21") + "\">平特两肖</a>.");
        builder.Append("<br /><a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=22") + "\">五号码不中</a>");

        builder.Append(Out.Tab("</div>", ""));

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(7, "six.aspx", 5, 0)));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【我的投注】<a href=\"" + Utils.getUrl("six.aspx?act=case") + "\">兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【投注参考】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=top") + "\">高手排行榜</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=list") + "\">历史开奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=cardrd") + "\">随机选号</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=cardbs") + "\">波色对照</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=cardsx") + "\">生肖对照</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=cardwx") + "\">五行对照</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=cardso") + "\">号码属性</a>");
        builder.Append(Out.Tab("</div>", ""));
        string FootUbb = ub.GetSub("SixFootUbb", xmlPath);
        if (FootUbb != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(Out.SysUBB(FootUbb) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        if (meid == 10086 || meid == 19611 || meid == 112233)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("sixset.aspx") + "\">虚拟投注设置&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }


    private void BmPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "WAP最快报码室";
        if (tm > 0)
        {
            Master.Refresh = tm;
            Master.Gourl = Utils.getUrl("six.aspx?act=bm&amp;tm=" + tm + "");
        }
        string str = new BCW.Service.GetCplist().GetCplistXML3();
        if (!string.IsNullOrEmpty(str))
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("【最快报码室】");
            if (tm == 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=fresh&amp;tm=" + tm + "") + "\">设置刷新</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=bm") + "\">手工刷新</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(str.Replace("<br/>=<b>", "=<b>").Replace("<br/>↓==↓", "").Replace("=", "-"));
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">返回虚拟投注&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(7, "six.aspx?act=bm", 5, 0)));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 刷新设置
    /// </summary>
    private void FreshPage()
    {
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "刷新设置";
        builder.Append(Out.Tab("<div class=\"title\">刷新设置</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("选择秒数:");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",,";
        strName = "tm,act";
        strType = "select,hidden";
        strValu = "'bm'";
        strEmpt = "5|5秒|10|10秒|15|15秒|20|20秒|30|30秒|40|40秒|50|50秒|60|60秒|90|90秒,,";
        strIdea = "/";
        strOthe = "确定,six.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=bm&amp;tm=" + tm + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        long gold = new BCW.BLL.User().GetGold(meid);
        long money = new BCW.BLL.User().GetMoney(meid);
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-9]\d*$", "类型选择错误"));
        if (ptype < 1 || ptype > 22)
        {
            Utils.Error("类型选择错误", "");
        }
        LHC.Model.VoteNo model = new LHC.BLL.VoteNo().GetVoteNoNew(0);
        if (model != null)
            Master.Title = "第" + model.qiNo + "期投注";
        else
            Master.Title = "请等待开通下期";

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(11));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(money) + "" + ub.Get("SiteBz2") + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        if (ptype == 1)
        {
            builder.Append(Out.Div("text", "特.码投注,您一次可输入6个特.码"));
            strText = "请输入您的下注额:,请输入您选的特.码1(必填):,请输入您选的特.码2:,请输入您选的特.码3:,请输入您选的特.码4:,请输入您选的特.码5:,请输入您选的特.码6:,,";
            strName = "payCent,Vote,vote2,vote3,vote4,vote5,vote6,ptype,act";
            strType = "num,num,num,num,num,num,num,hidden,hidden";
            strValu = "'''''''" + ptype + "'save";
            strEmpt = "false,false,true,true,true,true,true,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("特.码赔率如下:<br />");
            for (int i = 1; i <= 49; i++)
            {
                string odds = ub.GetSub("SixTM" + i + "", xmlPath);
                if (odds == "")
                    odds = ub.GetSub("SixTM", xmlPath);

                builder.Append("" + i + "(1:" + odds + ")|");
                if (i % 7 == 0 && i != 49)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 2)
        {
            builder.Append(Out.Div("text", "特.码单双投注"));
            strText = "请输入您的下注额:,请选择特.码单双:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|单(1:" + ub.GetSub("SixDS", xmlPath) + ")|2|双(1:" + ub.GetSub("SixDS2", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />49不计单双(打和)");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 3)
        {
            builder.Append(Out.Div("text", "特.码大小投注"));
            strText = "请输入您的下注额:,请选择特.码大小:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|大(1:" + ub.GetSub("SixDX", xmlPath) + ")|2|小(1:" + ub.GetSub("SixDX2", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />1到24小,25到48大,49打和");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 4)
        {
            builder.Append(Out.Div("text", "特.码波色投注"));
            strText = "请输入您的下注额:,请选择特.码波色:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|红波(1:" + ub.GetSub("SixBS", xmlPath) + ")|2|蓝波(1:" + ub.GetSub("SixBS", xmlPath) + ")|3|绿波(1:" + ub.GetSub("SixBS", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 5)
        {
            builder.Append(Out.Div("text", "特.码生肖投注"));
            strText = "请输入您的下注额:,请选择特.码生肖:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("特.码生肖赔率如下:<br />");
            for (int i = 1; i <= 12; i++)
            {
                string odds = ub.GetSub("SixXX" + i + "", xmlPath);
                if (odds == "")
                    odds = ub.GetSub("SixSX", xmlPath);

                builder.Append(ForSX(i.ToString()) + "(1:" + odds + ") ");
                if (i % 6 == 0 && i != 12)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 6)
        {
            builder.Append(Out.Div("text", "特.码家野投注"));
            strText = "请输入您的下注额:,请选择特.码家野:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|家禽(1:" + ub.GetSub("SixQS", xmlPath) + ")|2|野兽(1:" + ub.GetSub("SixQS2", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />家禽:牛马羊鸡狗猪<br />野兽:鼠猴兔虎龙蛇");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 7)
        {
            builder.Append(Out.Div("text", "特.码尾数投注"));
            strText = "请输入您的下注额:,请选择特.码尾数:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,0|0尾(1:" + ub.GetSub("SixW0", xmlPath) + ")|1|1尾(1:" + ub.GetSub("SixW1", xmlPath) + ")|2|2尾(1:" + ub.GetSub("SixW2", xmlPath) + ")|3|3尾(1:" + ub.GetSub("SixW3", xmlPath) + ")|4|4尾(1:" + ub.GetSub("SixW4", xmlPath) + ")|5|5尾(1:" + ub.GetSub("SixW5", xmlPath) + ")|6|6尾(1:" + ub.GetSub("SixW6", xmlPath) + ")|7|7尾(1:" + ub.GetSub("SixW7", xmlPath) + ")|8|8尾(1:" + ub.GetSub("SixW8", xmlPath) + ")|9|9尾(1:" + ub.GetSub("SixW9", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 8)
        {
            builder.Append(Out.Div("text", "特.码头数投注"));
            strText = "请输入您的下注额:,请选择特.码头数:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,0|0头(1:" + ub.GetSub("SixT0", xmlPath) + ")|1|1头(1:" + ub.GetSub("SixT1", xmlPath) + ")|2|2头(1:" + ub.GetSub("SixT2", xmlPath) + ")|3|3头(1:" + ub.GetSub("SixT3", xmlPath) + ")|4|4头(1:" + ub.GetSub("SixT4", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 9)
        {
            builder.Append(Out.Div("text", "特.码合数大小投注"));
            strText = "请输入您的下注额:,请选择特.码合数大小:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|合大(1:" + ub.GetSub("SixHDX", xmlPath) + ")|2|合小(1:" + ub.GetSub("SixHDX2", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("合数大小：<br />即投注了本期开奖对应的合大或合小才中奖.<br />49不合大也不合小，将打和<br />");
            builder.Append("合大：<br />5,6,7,8,9,14,15,16,17,18,23,24,25,26,27,32,33,34,35,36,41,42,43,44,45<br />");
            builder.Append("合小：<br />1,2,3,4,10,11,12,13,19,20,21,22,28,29,30,31,37,38,39,40,46,47,48");

            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 10)
        {
            builder.Append(Out.Div("text", "特.码合数单双投注"));
            strText = "请输入您的下注额:,请选择特.码合数单双:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|合单(1:" + ub.GetSub("SixHDS", xmlPath) + ")|2|合双(1:" + ub.GetSub("SixHDS2", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("合数单双：<br />即投注了本期开奖对应的合单或合双才中奖.<br />49不合单也不合双，将打和<br />");
            builder.Append("合单：<br />1,3,5,7,9,10,12,14,16,18,21,23,25,27,29,30,32,34,36,38,41,43,45,47<br />");
            builder.Append("合双：<br />2,4,6,8,11,13,15,17,19,20,22,24,26,28,31,33,35,37,39,40,42,44,46,48");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 11)
        {
            builder.Append(Out.Div("text", "平特肖投注"));
            strText = "请输入您的下注额:,请选择平特肖:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|鼠(1:" + ub.GetSub("SixPTX1", xmlPath) + ")|2|牛(1:" + ub.GetSub("SixPTX2", xmlPath) + ")|3|虎(1:" + ub.GetSub("SixPTX3", xmlPath) + ")|4|兔(1:" + ub.GetSub("SixPTX4", xmlPath) + ")|5|龙(1:" + ub.GetSub("SixPTX5", xmlPath) + ")|6|蛇(1:" + ub.GetSub("SixPTX6", xmlPath) + ")|7|马(1:" + ub.GetSub("SixPTX7", xmlPath) + ")|8|羊(1:" + ub.GetSub("SixPTX8", xmlPath) + ")|9|猴(1:" + ub.GetSub("SixPTX9", xmlPath) + ")|10|鸡(1:" + ub.GetSub("SixPTX10", xmlPath) + ")|11|狗(1:" + ub.GetSub("SixPTX11", xmlPath) + ")|12|猪(1:" + ub.GetSub("SixPTX12", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("平特肖：<br />即投注的生肖在本期的七个号码中(特或平)有这个生肖即中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 12)
        {
            builder.Append(Out.Div("text", "独平投注,买中独平1赔" + ub.GetSub("SixDP", xmlPath) + ""));
            strText = "请输入您的下注额:,请输入您选的独平:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,num,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("独平：<br />即投注的号码在本期六个平码中出现才中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 13)
        {
            builder.Append(Out.Div("text", "平特尾数投注"));
            strText = "请输入您的下注额:,请选择平特尾数:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,0|0尾(1:" + ub.GetSub("SixPW0", xmlPath) + ")|1|1尾(1:" + ub.GetSub("SixPW1", xmlPath) + ")|2|2尾(1:" + ub.GetSub("SixPW2", xmlPath) + ")|3|3尾(1:" + ub.GetSub("SixPW3", xmlPath) + ")|4|4尾(1:" + ub.GetSub("SixPW4", xmlPath) + ")|5|5尾(1:" + ub.GetSub("SixPW5", xmlPath) + ")|6|6尾(1:" + ub.GetSub("SixPW6", xmlPath) + ")|7|7尾(1:" + ub.GetSub("SixPW7", xmlPath) + ")|8|8尾(1:" + ub.GetSub("SixPW8", xmlPath) + ")|9|9尾(1:" + ub.GetSub("SixPW9", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("平特尾：<br />即投注的尾数在本期的七个号码中(特或平)有这个尾即中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 14)
        {
            builder.Append(Out.Div("text", "特.码五行投注"));
            strText = "请输入您的下注额:,请选择特.码五行:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|金(1:" + ub.GetSub("SixWX", xmlPath) + ")|2|木(1:" + ub.GetSub("SixWX", xmlPath) + ")|3|水(1:" + ub.GetSub("SixWX", xmlPath) + ")|3|火(1:" + ub.GetSub("SixWX", xmlPath) + ")|3|土(1:" + ub.GetSub("SixWX", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 15)
        {
            builder.Append(Out.Div("text", "特.码六肖投注,买中特.码六肖1赔" + ub.GetSub("SixSIX", xmlPath) + "<br />请您选择特.码六肖"));
            strText = "请输入您的下注额:,请您选择六肖:/,,,,,,,";
            strName = "payCent,Vote,vote2,vote3,vote4,vote5,vote6,ptype,act";
            strType = "num,select,select,select,select,select,select,hidden,hidden";
            strValu = "'''''''" + ptype + "'save";
            strEmpt = "false,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("六肖：<br />即自选六个生肖来投注,你选择的生肖在本期特码中出现才中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 16)
        {
            builder.Append(Out.Div("text", "特.码半波投注"));
            strText = "请输入您的下注额:,请选择特.码半波:,,";
            strName = "payCent,Vote,ptype,act";
            strType = "num,select,hidden,hidden";
            strValu = "''" + ptype + "'save";
            strEmpt = "false,1|红单(1:" + ub.GetSub("SixRed0", xmlPath) + ")|2|红双(1:" + ub.GetSub("SixRed1", xmlPath) + ")|3|蓝单(1:" + ub.GetSub("SixBlue0", xmlPath) + ")|4|蓝双(1:" + ub.GetSub("SixBlue1", xmlPath) + ")|5|绿单(1:" + ub.GetSub("SixGreen0", xmlPath) + ")|6|绿双(1:" + ub.GetSub("SixGreen1", xmlPath) + "),false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 17)
        {
            builder.Append(Out.Div("text", "特连投注,买中特连1赔" + ub.GetSub("SixTL", xmlPath) + ""));
            strText = "请输入您的下注额:,请输入第1个号码:,请输入第2个号码:,,";
            strName = "payCent,Vote,vote2,ptype,act";
            strType = "num,num,num,hidden,hidden";
            strValu = "'''" + ptype + "'save";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("特连：<br />即投注的二个号码其中一个为特,一个为平码才中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 18)
        {
            builder.Append(Out.Div("text", "平.码三中三投注,三中三1赔" + ub.GetSub("SixSZS", xmlPath) + ""));
            strText = "请输入您的下注额:,请输入第一个平.码:,请输入第二个平.码:,请输入第三个平.码:,,";
            strName = "payCent,Vote,vote2,vote3,ptype,act";
            strType = "num,num,num,num,hidden,hidden";
            strValu = "''''" + ptype + "'save";
            strEmpt = "false,false,false,true,true,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("三中三：<br />投注的三个平码全中才中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 19)
        {
            builder.Append(Out.Div("text", "平.码三中二投注,三中二1赔" + ub.GetSub("SixSZE", xmlPath) + ",三中三1赔" + ub.GetSub("SixSZE2", xmlPath) + ""));
            strText = "请输入您的下注额:,请输入第一个平.码:,请输入第二个平.码:,请输入第三个平.码:,,";
            strName = "payCent,Vote,vote2,vote3,ptype,act";
            strType = "num,num,num,num,hidden,hidden";
            strValu = "''''" + ptype + "'save";
            strEmpt = "false,false,false,true,true,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("三中二：<br />投注的三个平码中二个或三个即中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 20)
        {
            builder.Append(Out.Div("text", "平.码二中二投注,二中二1赔" + ub.GetSub("SixEZE", xmlPath) + ""));
            strText = "请输入您的下注额:,请输入第一个平.码:,请输入第二个平.码:,,";
            strName = "payCent,Vote,vote2,ptype,act";
            strType = "num,num,num,hidden,hidden";
            strValu = "'''" + ptype + "'save";
            strEmpt = "false,false,false,true,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("二中二：<br />投注的二个平码都中了才得奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 21)
        {
            builder.Append(Out.Div("text", "平特两肖投注,买中平特两肖1赔" + ub.GetSub("SixSIX", xmlPath) + "<br />请您选择平特两肖"));
            strText = "请输入您的下注额:,请您选择生肖:/,,,,,,,";
            strName = "payCent,Vote,vote2,ptype,act";
            strType = "num,select,select,hidden,hidden";
            strValu = "'''" + ptype + "'save";
            strEmpt = "false,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,0|选择|1|鼠|2|牛|3|虎|4|兔|5|龙|6|蛇|7|马|8|羊|9|猴|10|鸡|11|狗|12|猪,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("平特两肖：<br />即特.码和平码分别出在这两个生肖之内即为中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 22)
        {
            builder.Append(Out.Div("text", "五号码不中投注,买五号码不中1赔" + ub.GetSub("SixSIX", xmlPath) + "<br />请您输入五个号码"));
            strText = "请输入您的下注额:,请输入第一个号码:,请输入第二个号码:,请输入第三个号码:,请输入第四个号码:,请输入第五个号码:,,,";
            strName = "payCent,Vote,vote2,vote3,vote4,vote5,ptype,act";
            strType = "num,num,num,num,num,num,hidden,hidden";
            strValu = "''''''" + ptype + "'save";
            strEmpt = "false,false,false,false,false,false,false,false";
            strIdea = "/";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",six.aspx,post,1,red|blue";
            else
                strOthe = "决定了,six.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("五号码不中：<br />即5个号码在当期特.码和平码都没有开出来即为中奖.");
            builder.Append(Out.Tab("</div>", ""));
        }





        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【我的投注】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavePage()
    {
        LHC.Model.VoteNo m = new LHC.BLL.VoteNo().GetVoteNoNew(0);
        if (m == null)
        {
            Utils.Error("已停止下注，请等待下一期再下注", "");
        }
        if (m.ExTime.AddMinutes(-Utils.ParseInt(ub.GetSub("SixBeforeMin", xmlPath))) < DateTime.Now)
        {
            Utils.Error("截止下注已到，请等待下一期再下注", "");
        }
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[0-9]\d*$", "类型选择错误"));
        if (ptype < 1 || ptype > 22)
        {
            Utils.Error("类型选择错误", "");
        }
        long payCent = Int64.Parse(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注额填写错误"));
        string Vote = "";

        string vote2 = "";
        string vote3 = "";
        string vote4 = "";
        string vote5 = "";
        string vote6 = "";
        if (ptype == 1)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "特.码填写错误");
            vote2 = Utils.GetRequest("vote2", "post", 3, @"^[1-9]$|^[1-4]([0-9])?$", "特.码2填写错误");
            vote3 = Utils.GetRequest("vote3", "post", 3, @"^[1-9]$|^[1-4]([0-9])?$", "特.码3填写错误");
            vote4 = Utils.GetRequest("vote4", "post", 3, @"^[1-9]$|^[1-4]([0-9])?$", "特.码4填写错误");
            vote5 = Utils.GetRequest("vote5", "post", 3, @"^[1-9]$|^[1-4]([0-9])?$", "特.码5填写错误");
            vote6 = Utils.GetRequest("vote6", "post", 3, @"^[1-9]$|^[1-4]([0-9])?$", "特.码6填写错误");

            //单个特别号码投注上限
            TMSX(ac, m.qiNo, Utils.ParseInt(Vote), payCent);

            //连接起来并判断号码是否重复
            if (vote2 != "")
            {
                if (("," + Vote + ",").Contains("," + vote2 + ","))
                    Utils.Error("不能投注同一号码" + vote2 + "", "");

                Vote += "," + vote2;
                TMSX(ac, m.qiNo, Utils.ParseInt(vote2), payCent);
            }
            if (vote3 != "")
            {
                if (("," + Vote + ",").Contains("," + vote3 + ","))
                    Utils.Error("不能投注同一号码" + vote3 + "", "");

                Vote += "," + vote3;
                TMSX(ac, m.qiNo, Utils.ParseInt(vote3), payCent);
            }
            if (vote4 != "")
            {
                if (("," + Vote + ",").Contains("," + vote4 + ","))
                    Utils.Error("不能投注同一号码" + vote4 + "", "");

                Vote += "," + vote4;
                TMSX(ac, m.qiNo, Utils.ParseInt(vote4), payCent);
            }
            if (vote5 != "")
            {
                if (("," + Vote + ",").Contains("," + vote5 + ","))
                    Utils.Error("不能投注同一号码" + vote5 + "", "");

                Vote += "," + vote5;
                TMSX(ac, m.qiNo, Utils.ParseInt(vote5), payCent);
            }
            if (vote6 != "")
            {
                if (("," + Vote + ",").Contains("," + vote6 + ","))
                    Utils.Error("不能投注同一号码" + vote6 + "", "");

                Vote += "," + vote6;
                TMSX(ac, m.qiNo, Utils.ParseInt(vote6), payCent);
            }
        }
        else if (ptype == 2)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-2]$", "单双选择错误");
        }
        else if (ptype == 3)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-2]$", "大小选择错误");
        }
        else if (ptype == 4)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-3]$", "波色选择错误");
        }
        else if (ptype == 5)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "生肖选择错误");
        }
        else if (ptype == 6)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-2]$", "家野选择错误");
        }
        else if (ptype == 7)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[0-9]$", "尾数选择错误");
        }
        else if (ptype == 8)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[0-4]$", "头数选择错误");
        }
        else if (ptype == 9)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-2]$", "合大小选择错误");
        }
        else if (ptype == 10)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-2]$", "合单双选择错误");
        }
        else if (ptype == 11)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "平特肖选择错误");
        }
        else if (ptype == 12)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "独平填写错误");
        }
        else if (ptype == 13)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[0-9]$", "平特尾选择错误");
        }
        else if (ptype == 14)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-5]$", "五行选择错误");
        }
        else if (ptype == 15)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "六肖选择错误");
            vote2 = Utils.GetRequest("vote2", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "六肖选择错误");
            vote3 = Utils.GetRequest("vote3", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "六肖选择错误");
            vote4 = Utils.GetRequest("vote4", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "六肖选择错误");
            vote5 = Utils.GetRequest("vote5", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "六肖选择错误");
            vote6 = Utils.GetRequest("vote6", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "六肖选择错误");
            if (Vote == vote2 || Vote == vote3 || Vote == vote4 || Vote == vote5 || Vote == vote6 || vote2 == vote3 || vote2 == vote4 || vote2 == vote5 || vote2 == vote6 || vote3 == vote4 || vote3 == vote5 || vote3 == vote6 || vote4 == vote5 || vote4 == vote6 || vote5 == vote6)
            {
                Utils.Error("不能选择同一个生肖", "");
            }

            Vote = "" + ForSX(Vote) + "," + ForSX(vote2) + "," + ForSX(vote3) + "," + ForSX(vote4) + "," + ForSX(vote5) + "," + ForSX(vote6) + "";
        }
        else if (ptype == 16)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-6]$", "半波选择错误");
        }
        else if (ptype == 17)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "特连1填写错误");
            vote2 = Utils.GetRequest("vote2", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "特连2填写错误");
            if (Vote == vote2)
            {
                Utils.Error("不能填写同一个号码", "");
            }
            Vote = Vote + "," + vote2;
        }
        else if (ptype == 18 || ptype == 19)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "第一个平.码填写错误");
            vote2 = Utils.GetRequest("vote2", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "第二个平.码填写错误");
            vote3 = Utils.GetRequest("vote3", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "第三个平.码填写错误");
            if (vote2 != "")
            {
                if (("," + Vote + ",").Contains("," + vote2 + ","))
                    Utils.Error("不能投注同一平.码" + vote2 + "", "");

                Vote += "," + vote2;
            }
            if (vote3 != "")
            {
                if (("," + Vote + ",").Contains("," + vote3 + ","))
                    Utils.Error("不能投注同一平.码" + vote3 + "", "");

                Vote += "," + vote3;
            }
        }
        else if (ptype == 20)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "第一个平.码填写错误");
            vote2 = Utils.GetRequest("vote2", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "第二个平.码填写错误");
            if (Vote == vote2)
            {
                Utils.Error("不能填写同一个平.码", "");
            }
            if (vote2 != "")
                Vote += "," + vote2;
        }
        else if (ptype == 21)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "平特两肖选择错误");
            vote2 = Utils.GetRequest("vote2", "post", 2, @"^[1-9]$|^10$|^11$|^12$", "平特两肖选择错误");
            if (Vote == vote2)
            {
                Utils.Error("不能选择同一个生肖", "");
            }

            Vote = "" + ForSX(Vote) + "," + ForSX(vote2) + "";
        }
        else if (ptype == 22)
        {
            Vote = Utils.GetRequest("Vote", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "号码1填写错误");
            vote2 = Utils.GetRequest("vote2", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "号码2填写错误");
            vote3 = Utils.GetRequest("vote3", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "号码3填写错误");
            vote4 = Utils.GetRequest("vote4", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "号码4填写错误");
            vote5 = Utils.GetRequest("vote5", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "号码5填写错误");

            //连接起来并判断号码是否重复

            if (("," + Vote + ",").Contains("," + vote2 + ","))
                Utils.Error("不能投注同一号码" + vote2 + "", "");

            Vote += "," + vote2;

            if (("," + Vote + ",").Contains("," + vote3 + ","))
                Utils.Error("不能投注同一号码" + vote3 + "", "");

            Vote += "," + vote3;

            if (("," + Vote + ",").Contains("," + vote4 + ","))
                Utils.Error("不能投注同一号码" + vote4 + "", "");

            Vote += "," + vote4;

            if (("," + Vote + ",").Contains("," + vote5 + ","))
                Utils.Error("不能投注同一号码" + vote5 + "", "");

            Vote += "," + vote5;
        }

        //转化
        if (ptype == 2)
        {
            Vote = "" + ((Vote == "1") ? "单" : "双") + "";
        }
        else if (ptype == 3)
        {
            Vote = "" + ((Vote == "1") ? "大" : "小") + "";
        }
        else if (ptype == 4)
        {
            if (Vote == "1")
                Vote = "红波";
            else if (Vote == "2")
                Vote = "蓝波";
            else
                Vote = "绿波";
        }
        else if (ptype == 5 || ptype == 11)
        {
            Vote = ForSX(Vote);
        }
        else if (ptype == 6)
        {
            Vote = "" + ((Vote == "1") ? "家禽" : "野兽") + "";
        }
        else if (ptype == 9)
        {
            Vote = "" + ((Vote == "1") ? "合大" : "合小") + "";
        }
        else if (ptype == 10)
        {
            Vote = "" + ((Vote == "1") ? "合单" : "合双") + "";
        }
        else if (ptype == 14)
        {
            if (Vote == "1")
                Vote = "金";
            else if (Vote == "2")
                Vote = "木";
            else if (Vote == "3")
                Vote = "水";
            else if (Vote == "4")
                Vote = "火";
            else
                Vote = "土";
        }
        else if (ptype == 16)
        {
            Vote = ForBB(Vote);
        }
        //扣币
        long Cent = 0;
        if (ptype == 1)
        {
            if (Vote.Contains(","))
            {
                int payNum = Utils.GetStringNum(Vote, ",") + 1;
                Cent = Convert.ToInt64(payCent * payNum);
            }
            else
            {
                Cent = payCent;
            }
        }
        else
        {
            Cent = payCent;
        }

        int bzType = 0;
        string bzText = string.Empty;
        long gold = 0;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
        {
            bzType = 1;
            bzText = ub.Get("SiteBz2");
            if (Cent < Convert.ToInt64(ub.GetSub("SixSmallPay2", xmlPath)) || Cent > Convert.ToInt64(ub.GetSub("SixBigPay2", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("SixSmallPay2", xmlPath) + "-" + ub.GetSub("SixBigPay2", xmlPath) + "" + bzText + "", "");
            }
            gold = new BCW.BLL.User().GetMoney(meid);

        }
        else
        {
            bzType = 0;
            bzText = ub.Get("SiteBz");
            //支付安全提示
            string[] p_pageArr = { "ac", "act", "ptype", "payCent", "Vote", "vote2", "vote3", "vote4", "vote5", "vote6" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            //是否刷屏
            long big = Convert.ToInt64(ub.GetSub("SixBigPay", xmlPath));
            long small = Convert.ToInt64(ub.GetSub("SixSmallPay", xmlPath));
            string appName = "LIGHT_SIX";
            int Expir = Utils.ParseInt(ub.GetSub("SixExpir", xmlPath));

            BCW.User.Users.IsFresh(appName, Expir, payCent, small, big);
            gold = new BCW.BLL.User().GetGold(meid);

        }
        if (gold < Cent)
        {
            Utils.Error("您的" + bzText + "不足", "");
        }

        long myPayCents = 0;
        long IDPayCents = 0;
        long PayCents = 0;
        if (bzType == 0)
        {
            myPayCents = new LHC.BLL.VotePay().GetPayCent(meid, m.qiNo, 0);
            IDPayCents = Utils.ParseInt64(ub.GetSub("SixIDPayCents", xmlPath));
            PayCents = Utils.ParseInt64(ub.GetSub("SixPayCents", xmlPath));
        }
        else
        {
            myPayCents = new LHC.BLL.VotePay().GetPayCent(meid, m.qiNo, 1);
            IDPayCents = Utils.ParseInt64(ub.GetSub("SixIDPayCents2", xmlPath));
            PayCents = Utils.ParseInt64(ub.GetSub("SixPayCents2", xmlPath));
        }
        //每期每ID限下注额
        if (IDPayCents > 0)
        {
            if (Cent + myPayCents > IDPayCents)
            {
                if (myPayCents >= IDPayCents)
                {
                    Utils.Error("系统限制每期每人下注" + IDPayCents + "" + bzText + "，欢迎在下期下注", "");
                }
                else
                {
                    Utils.Error("系统限制每期每人下注" + IDPayCents + "" + bzText + "，你现在最多可以下注" + (IDPayCents - myPayCents) + "" + bzText + "", "");
                }
            }
        }
        //每期下注限额
        if (PayCents > 0)
        {
            if (Cent + m.payCent > PayCents)
            {
                if (payCent >= PayCents)
                {
                    Utils.Error("系统限制每期下注" + PayCents + "" + bzText + "，欢迎在下期下注", "");
                }
                else
                {
                    Utils.Error("系统限制每期下注" + PayCents + "" + bzText + "，你现在最多可以下注" + (PayCents - m.payCent) + "" + bzText + "", "");
                }
            }
        }

        string mename = new BCW.BLL.User().GetUsName(meid);
        if (bzType == 0)
        {
            new BCW.BLL.User().UpdateiGold(meid, mename, -Cent, 5);
            //6655专用消费
            new BCW.BLL.User().UpdateiGold(6655, "6仔专用号", Cent, "ID:" + meid + "" + ForType(ptype) + "下注" + Vote + "");

            //更新本期数据
            new LHC.BLL.VoteNo().Update(m.qiNo, Cent, 1);
        }
        else
        {
            new BCW.BLL.User().UpdateiMoney(meid, mename, -Cent, "虚拟投注消费");
            //更新本期数据
            new LHC.BLL.VoteNo().Update2(m.qiNo, Cent, 1);
        }

        LHC.Model.VotePay model = new LHC.Model.VotePay();
        model.Types = ptype;
        model.qiNo = m.qiNo;
        model.UsID = meid;
        model.UsName = mename;
        model.Vote = Vote;
        model.payCent = payCent;
        model.winCent = 0;
        model.State = 0;
        model.AddTime = DateTime.Now;
        model.BzType = bzType;
        new LHC.BLL.VotePay().Add(model);

        Utils.Success("下注", "下注成功，花费" + Cent + "" + bzText + "<br /><a href=\"" + Utils.getUrl("six.aspx?act=add&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("six.aspx"), "2");
    }

    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "我的未开投注";
        else
            strTitle = "我的历史投注";

        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "";
        if (ptype == 1)
            strWhere += " and State=0";
        else
            strWhere += " and State>0";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string Luckqi = "";
        // 开始读取列表
        IList<LHC.Model.VotePay> listVotePay = new LHC.BLL.VotePay().GetVotePays(pageIndex, pageSize, strWhere, out recordCount);
        if (listVotePay.Count > 0)
        {
            int k = 1;
            foreach (LHC.Model.VotePay n in listVotePay)
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

                if (n.qiNo.ToString() != Luckqi)
                    builder.Append("=第" + n.qiNo + "期=<br />");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");

                string TyName = ForType(n.Types);
                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                if (n.State == 0)
                    builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + bzText + ")[" + DT.FormatDate(n.AddTime, 1) + "]");
                else if (n.State == 1)
                {
                    builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + bzText + ")[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.winCent > 0)
                    {
                        builder.Append("赢" + n.winCent + "" + bzText + "");
                    }
                }
                else
                    builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + ub.Get("SiteBz") + "),赢" + n.winCent + "" + bzText + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                Luckqi = n.qiNo.ToString();
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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        Master.Title = "历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=历史开奖=");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "State=1";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<LHC.Model.VoteNo> listVoteNo = new LHC.BLL.VoteNo().GetVoteNos(pageIndex, pageSize, strWhere, out recordCount);
        if (listVoteNo.Count > 0)
        {
            int k = 1;
            foreach (LHC.Model.VoteNo n in listVoteNo)
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

                builder.Append("第" + n.qiNo + "期开出:<a href=\"" + Utils.getUrl("six.aspx?act=listview&amp;id=" + n.ID + "") + "\">" + n.pNum1 + "-" + n.pNum2 + "-" + n.pNum3 + "-" + n.pNum4 + "-" + n.pNum5 + "-" + n.pNum6 + "[特" + n.sNum + "]</a>");
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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        LHC.Model.VoteNo model = new LHC.BLL.VoteNo().GetVoteNo(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.qiNo + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "qiNo=" + model.qiNo + " and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<LHC.Model.VotePay> listVotePay = new LHC.BLL.VotePay().GetVotePays(pageIndex, pageSize, strWhere, out recordCount);
        if (listVotePay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.qiNo + "期共投注:" + model.payCent + "" + ub.Get("SiteBz") + "");
            builder.Append("<br />下注总数为" + model.payCount + "注");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (LHC.Model.VotePay n in listVotePay)
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

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>获得" + n.winCent + "" + bzText + "");

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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void CardSoPage()
    {
        Master.Title = "号码属性";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看号码属性");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            int num = Utils.ParseInt(Utils.GetRequest("num", "all", 2, @"^[1-9]$|^[1-4]([0-9])?$", "号码填写错误"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("您查询的号码<img src=\"/Files/sys/ball/" + num + ".gif\" alt=\"" + num + "\"/>属性如下:");
            builder.Append("<br />所属单双：" + GetDS(num) + "");
            builder.Append("<br />所属大小：" + GetDX(num) + "");
            builder.Append("<br />所属波色：" + GetBS(num) + "");
            builder.Append("<br />所属生肖：" + GetSX(num) + "");
            builder.Append("<br />所属家野：" + GetQS(num) + "");
            builder.Append("<br />所属五行：" + GetWX(num) + "");
            builder.Append("<br />合数单双：" + GetHDS(num) + "");
            builder.Append("<br />合数大小：" + GetHDX(num) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            strText = "请输入号码(不必在号码前加0):/,,";
            strName = "num,info,act";
            strType = "num,hidden,hidden";
            strValu = "'ok'cardso";
            strEmpt = "false,false,false";
            strIdea = "/";
            strOthe = "查询,six.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CardRdPage()
    {
        Master.Title = "随机为您选号";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("随机为您选号");
        builder.Append(Out.Tab("</div>", "<br />"));

        //取随机数
        Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        int rd1 = ra.Next(1, 50);
        int rd2 = ra.Next(1, 50);
        int rd3 = ra.Next(1, 50);
        int rd4 = ra.Next(1, 50);
        int rd5 = ra.Next(1, 50);
        int rd6 = ra.Next(1, 50);
        int rd7 = ra.Next(1, 50);

        string pNum = rd1 + "-" + rd2 + "-" + rd3 + "-" + rd4 + "-" + rd5 + "-" + rd6 + "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("平：" + pNum + "<br />特：" + rd7 + "");
        builder.Append("<br />~电脑选号,仅供参与~");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void CardBsPage()
    {
        Master.Title = "波色对照表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("波色对照表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("红波：" + ub.GetSub("Sixred", xmlPath).Replace("#", ",") + "<br />蓝波：" + ub.GetSub("Sixblue", xmlPath).Replace("#", ",") + "<br />绿波：" + ub.GetSub("Sixgreen", xmlPath).Replace("#", ",") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void CardSxPage()
    {
        Master.Title = "生肖对照表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("生肖对照表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("鼠:" + ub.GetSub("Sixsx1", xmlPath).Replace("#", ",") + "<br />牛:" + ub.GetSub("Sixsx2", xmlPath).Replace("#", ",") + "<br />虎:" + ub.GetSub("Sixsx3", xmlPath).Replace("#", ",") + "<br />兔:" + ub.GetSub("Sixsx4", xmlPath).Replace("#", ",") + "<br />龙:" + ub.GetSub("Sixsx5", xmlPath).Replace("#", ",") + "<br />蛇:" + ub.GetSub("Sixsx6", xmlPath).Replace("#", ",") + "<br />马:" + ub.GetSub("Sixsx7", xmlPath).Replace("#", ",") + "<br />羊:" + ub.GetSub("Sixsx8", xmlPath).Replace("#", ",") + "<br />猴:" + ub.GetSub("Sixsx9", xmlPath).Replace("#", ",") + "<br />鸡:" + ub.GetSub("Sixsx10", xmlPath).Replace("#", ",") + "<br />狗:" + ub.GetSub("Sixsx11", xmlPath).Replace("#", ",") + "<br />猪:" + ub.GetSub("Sixsx12", xmlPath).Replace("#", ",") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void CardWxPage()
    {
        Master.Title = "五行对照表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("五行对照表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("金:" + ub.GetSub("Sixgold", xmlPath).Replace("#", ",") + "<br />木:" + ub.GetSub("Sixwood", xmlPath).Replace("#", ",") + "<br />水:" + ub.GetSub("Sixwater", xmlPath).Replace("#", ",") + "<br />火:" + ub.GetSub("Sixfire", xmlPath).Replace("#", ",") + "<br />土:" + ub.GetSub("Sixsoil", xmlPath).Replace("#", ",") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append(" <a href=\"" + Utils.getUrl("six.aspx") + "\">返回投注首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "投注排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=5";
        if (ptype == 1)
            strOrder = "(WinGold+PutGold) Desc";
        else
            strOrder = "(WinNum-PutNum) Desc";

        // 开始读取列表
        IList<BCW.Model.Toplist> listToplist = new BCW.BLL.Toplist().GetToplists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listToplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Toplist n in listToplist)
            {
                    n.UsName = BCW.User.Users.SetVipName(n.UsId, n.UsName, false);
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string sText = string.Empty;
                if (ptype == 1)
                    sText = "净赢" + (n.WinGold + n.PutGold) + "" + ub.Get("SiteBz") + "";
                else
                    sText = "胜" + (n.WinNum - n.PutNum) + "次";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}</a>{2}", n.UsId, n.UsName, sText);
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new LHC.BLL.VotePay().ExistsState(pid, meid))
        {
            new LHC.BLL.VotePay().UpdateState(pid);
            //操作币
            long winMoney = new LHC.BLL.VotePay().GetWinCent(pid);
            int bzType = new LHC.BLL.VotePay().GetBzType(pid);
            if (bzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "虚拟兑奖-标识ID" + pid + "");
                //6655专用消费
                new BCW.BLL.User().UpdateiGold(6655, "6仔专用号", -winMoney, "ID:" + meid + "六仔兑奖" + winMoney + "" + ub.Get("SiteBz") + "");

                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("six.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "虚拟兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("six.aspx?act=case"), "1");
            }
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("six.aspx?act=case"), "1");
        }
    }

    private void CasePostPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        string[] strArrId = arrId.Split(",".ToCharArray());
        long getwinMoney = 0;
        long winMoney = 0;
        long winMoney2 = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new LHC.BLL.VotePay().ExistsState(pid, meid))
            {
                new LHC.BLL.VotePay().UpdateState(pid);
                //操作币
                long win = new LHC.BLL.VotePay().GetWinCent(pid);
                int bzType = new LHC.BLL.VotePay().GetBzType(pid);
                if (bzType == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, win, "虚拟兑奖-标识ID" + pid + "");
                    //6655专用消费
                    if (pid != 13481 && pid != 13503)
                    {
                        new BCW.BLL.User().UpdateiGold(6655, "6仔专用号", -win, "ID:" + meid + "六仔兑奖" + win + "-标识ID" + pid + "" + ub.Get("SiteBz") + "");
                    }
                    winMoney += win;
                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(meid, win, "虚拟兑奖-标识ID" + pid + "");
                    winMoney2 += win;
                }

            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("six.aspx?act=case"), "1");
    }


    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and winCent>0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<LHC.Model.VotePay> listVotePay = new LHC.BLL.VotePay().GetVotePays(pageIndex, pageSize, strWhere, out recordCount);
        if (listVotePay.Count > 0)
        {
            int k = 1;
            foreach (LHC.Model.VotePay n in listVotePay)
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
                builder.Append("[第" + n.qiNo + "期].");

                string TyName = ForType(n.Types);
                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + bzText + ")赢" + n.winCent + "" + bzText + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

                arrId = arrId + " " + n.ID;
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
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,six.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("six.aspx") + "\">虚拟</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private string ForSX(string Vote)
    {
        string ForVote = string.Empty;

        if (Vote == "1")
            ForVote = "鼠";
        else if (Vote == "2")
            ForVote = "牛";
        else if (Vote == "3")
            ForVote = "虎";
        else if (Vote == "4")
            ForVote = "兔";
        else if (Vote == "5")
            ForVote = "龙";
        else if (Vote == "6")
            ForVote = "蛇";
        else if (Vote == "7")
            ForVote = "马";
        else if (Vote == "8")
            ForVote = "羊";
        else if (Vote == "9")
            ForVote = "猴";
        else if (Vote == "10")
            ForVote = "鸡";
        else if (Vote == "11")
            ForVote = "狗";
        else if (Vote == "12")
            ForVote = "猪";

        return ForVote;
    }

    private string ForBB(string Vote)
    {
        string ForVote = string.Empty;

        if (Vote == "1")
            ForVote = "红单";
        else if (Vote == "2")
            ForVote = "红双";
        else if (Vote == "3")
            ForVote = "蓝单";
        else if (Vote == "4")
            ForVote = "蓝双";
        else if (Vote == "5")
            ForVote = "绿单";
        else if (Vote == "6")
            ForVote = "绿双";

        return ForVote;
    }

    private string ForType(int Types)
    {
        string TyName = string.Empty;
        if (Types == 1)
            TyName = "特.码";
        else if (Types == 2)
            TyName = "单双";
        else if (Types == 3)
            TyName = "大小";
        else if (Types == 4)
            TyName = "波色";
        else if (Types == 5)
            TyName = "特肖";
        else if (Types == 6)
            TyName = "家野";
        else if (Types == 7)
            TyName = "特尾";
        else if (Types == 8)
            TyName = "特头";
        else if (Types == 9)
            TyName = "合数大小";
        else if (Types == 10)
            TyName = "合数单双";
        else if (Types == 11)
            TyName = "平特肖";
        else if (Types == 12)
            TyName = "独平";
        else if (Types == 13)
            TyName = "平特尾";
        else if (Types == 14)
            TyName = "五行";
        else if (Types == 15)
            TyName = "六肖";
        else if (Types == 16)
            TyName = "半波";
        else if (Types == 17)
            TyName = "特连";
        else if (Types == 18)
            TyName = "三中三";
        else if (Types == 19)
            TyName = "三中二";
        else if (Types == 20)
            TyName = "二中二";
        else if (Types == 21)
            TyName = "平特两肖";
        else if (Types == 22)
            TyName = "五号码不中";

        return TyName;
    }

    /// <summary>
    /// 得到单双
    /// </summary>
    private string GetDS(int sNum)
    {
        string ForDS = string.Empty;
        if (sNum == 49)
            return "打和";

        if (sNum % 2 == 0)
            ForDS = "双";
        else
            ForDS = "单";

        return ForDS;
    }

    /// <summary>
    /// 得到大小
    /// </summary>
    private string GetDX(int sNum)
    {
        string ForDX = string.Empty;
        if (sNum <= 24)
            ForDX = "小";
        else if (sNum >= 25 && sNum != 49)
            ForDX = "大";
        else
            ForDX = "打和";

        return ForDX;
    }

    private string GetBS(int sNum)
    {
        string ForBS = string.Empty;
        if (("#" + ub.GetSub("Sixred", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForBS = "红波";
        }
        else if (("#" + ub.GetSub("Sixblue", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForBS = "蓝波";
        }
        else if (("#" + ub.GetSub("Sixgreen", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForBS = "绿波";
        }

        return ForBS;
    }

    /// <summary>
    /// 得到生肖
    /// </summary>
    private string GetSX(int sNum)
    {
        //生肖
        string ForSX = string.Empty;
        if (("#" + ub.GetSub("Sixsx1", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "鼠";
        }
        else if (("#" + ub.GetSub("Sixsx2", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "牛";
        }
        else if (("#" + ub.GetSub("Sixsx3", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "虎";
        }
        else if (("#" + ub.GetSub("Sixsx4", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "兔";
        }
        else if (("#" + ub.GetSub("Sixsx5", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "龙";
        }
        else if (("#" + ub.GetSub("Sixsx6", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "蛇";
        }
        else if (("#" + ub.GetSub("Sixsx7", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "马";
        }
        else if (("#" + ub.GetSub("Sixsx8", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "羊";
        }
        else if (("#" + ub.GetSub("Sixsx9", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "猴";
        }
        else if (("#" + ub.GetSub("Sixsx10", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "鸡";
        }
        else if (("#" + ub.GetSub("Sixsx11", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "狗";
        }
        else if (("#" + ub.GetSub("Sixsx12", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "猪";
        }
        return ForSX;
    }

    private string GetWX(int sNum)
    {
        string ForWX = string.Empty;
        if (("#" + ub.GetSub("Sixgold", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "金";
        }
        else if (("#" + ub.GetSub("Sixwood", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "木";
        }
        else if (("#" + ub.GetSub("Sixwater", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "水";
        }
        else if (("#" + ub.GetSub("Sixfire", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "火";
        }
        else if (("#" + ub.GetSub("Sixsoil", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "土";
        }
        return ForWX;
    }

    private string GetQS(int num)
    {
        string JQ = "牛马羊鸡狗猪";
        string YS = "鼠猴兔虎龙蛇";
        string SX = GetSX(num);
        if (JQ.Contains(SX))
        {
            return "家禽";
        }
        else if (YS.Contains(SX))
        {
            return "野兽";
        }
        return "囧";
    }

    private string GetHDS(int num)
    {
        string HD = "1,3,5,7,9,10,12,14,16,18,21,23,25,27,29,30,32,34,36,38,41,43,45,47";
        string HS = "2,4,6,8,11,13,15,17,19,20,22,24,26,28,31,33,35,37,39,40,42,44,46,48";

        if (("," + HD + ",").Contains("," + num + ","))
        {
            return "合单";
        }
        else if (("," + HS + ",").Contains("," + num + ","))
        {
            return "合双";
        }
        return "打和";
    }

    private string GetHDX(int num)
    {
        string HX = "1,2,3,4,10,11,12,13,19,20,21,22,28,29,30,31,37,38,39,40,46,47,48";
        string HD = "5,6,7,8,9,14,15,16,17,18,23,24,25,26,27,32,33,34,35,36,41,42,43,44,45";

        if (("," + HD + ",").Contains("," + num + ","))
        {
            return "合大";
        }
        else if (("," + HX + ",").Contains("," + num + ","))
        {
            return "合小";
        }
        return "打和";
    }

    private bool Isbz()
    {
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("wapri"))
            return true;
        else
            return false;
    }

    private void TMSX(string ac, int qiNo, int Vote, long payCent)
    {

        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz"))))
        {
            long MaxCent = Utils.ParseInt64(ub.GetSub("SixTMSX" + Vote + "", xmlPath));
            if (MaxCent > 0)
            {
                long TzCent = new LHC.BLL.VotePay().GetPayCent(qiNo, Vote);
                if (MaxCent < (TzCent + payCent))
                {
                    if (TzCent >= MaxCent) {
                        Utils.Error("特.码" + Vote + "本期只能允许下注" + MaxCent + "" + ub.Get("SiteBz") + "，欢迎在下期下注", "");
                    }
                    else
                    {
                        Utils.Error("特.码" + Vote + "当前只能允许下注" + (MaxCent - TzCent) + "" + ub.Get("SiteBz") + "", "");
                    }
                }
            }
        }
    }
}