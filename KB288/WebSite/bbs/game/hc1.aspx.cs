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
/// <summary>
/// 姚志光 20160621 增加活跃抽奖入口控制限额
/// 邵广林 20160801 增加兑奖防刷
/// 邵广林 20160908 增加ID记录酷币
/// 邵广林 20161103 增加快捷下注并修改排版
/// </summary>
public partial class bbs_game_hc1 : System.Web.UI.Page
{
    /// <summary>
    /// 吴帝元 20160513
    /// </summary>
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/hc1.xml";
    protected string GameName = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//游戏名字
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("Hc1Status", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "help":
                HelpPage();
                break;
            case "top":
                TopPage();
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
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "pay":
                PayPage();
                break;
            case "paysave":
                PaySavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("Hc1Name", xmlPath);
        int meid = new BCW.User.Users().GetUsId();

        //内测ID
        string DemoIDS = ub.GetSub("Hc1DemoIDS", xmlPath);
        if (DemoIDS != "")
        {
            if (!("#" + DemoIDS + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("游戏内测中，您还不是内测会员", "");
            }
        }

        string Logo = ub.GetSub("Hc1Logo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        string TopUbb = ub.GetSub("Hc1TopUbb", xmlPath);
        if (TopUbb != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(TopUbb) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcListNew(0);
        if (model != null)
        {
            if (model.EndTime > DateTime.Now)
            {
                //sgl 20161025 修改倒计时js
                string hc1 = new BCW.JS.somejs().newDaojishi("xk2", model.EndTime.AddMinutes(10).AddSeconds(-10));
                builder.Append("第" + model.CID + "期投注进行中,距离截止时间还有" + hc1 + "<br />本期已下注" + model.payCent + "" + ub.Get("SiteBz") + "(" + model.payCount + "注)");
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
        //BCW.Model.Game.HcList model2 = new BCW.BLL.Game.HcList().GetHcListNew(1);
        //if (model2 != null)
        //{
        //    builder.Append("<br />上期" + model2.CID + "期开奖结果:" + model2.Result + "");
        //}
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【选择玩法】<a href=\"" + Utils.getUrl("hc1.aspx?act=help") + "\">规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=1") + "\">选号玩法</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=2") + "\">生肖玩法</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=3") + "\">方位玩法</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=4") + "\">四季玩法</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=5") + "\">大小单双</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=6") + "\">六肖中奖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=7") + "\">尾数大小</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=8") + "\">尾数单双</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=9") + "\">家禽野兽</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=10") + "\">自选不中</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【往期开奖】");
        builder.Append(Out.Tab("</div>", "<br />"));
        IList<BCW.Model.Game.HcList> listHcList = new BCW.BLL.Game.HcList().GetHcLists(3, "State=1");
        if (listHcList.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            int k = 1;
            foreach (BCW.Model.Game.HcList n in listHcList)
            {

                builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("hc1.aspx?act=listview&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.Result + "," + OutSx(n.Result) + "," + OutSj(n.Result) + "," + OutFw(n.Result) + "</b></a><br />");

                k++;
            }
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看历史开奖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【我的投注】<a href=\"" + Utils.getUrl("hc1.aspx?act=case") + "\">兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=top") + "\">游戏排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=list") + "\">历史开奖</a>");
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(24, "hc1.aspx", 5, 0)));

        string FootUbb = ub.GetSub("Hc1FootUbb", xmlPath);
        if (FootUbb != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(Out.SysUBB(FootUbb) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;下注");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我要下注";
        BCW.Model.Game.HcList hc = new BCW.BLL.Game.HcList().GetHcListNew(0);
        if (hc == null)
        {
            Utils.Error("请等待管理员开通下期。。。", "");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + hc.CID + "期.");


        if (hc.EndTime < DateTime.Now)
            builder.Append("系统正在开奖。。。");
        else
        {
            string hc1 = new BCW.JS.somejs().newDaojishi("xk2", hc.EndTime.AddMinutes(10).AddSeconds(-10));
            builder.Append("距离截止还有" + hc1 + " ");
        }
        builder.Append(Out.Tab("</div>", ""));
        long gold = new BCW.BLL.User().GetGold(meid);
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]$", "类型选择错误"));
        long PayCent = Utils.ParseInt64(Utils.GetRequest("PayCent", "all", 1, @"^[1-9]\d*$", "0"));
        if (ptype == 1)
        {
            string pText = "01#02#03#04#05#06#07#08#09#10#11#12#13#14#15#16#17#18#19#20#21#22#23#24#25#26#27#28#29#30#31#32#33#34#35#36";
            string pValue = "1#2#3#4#5#6#7#8#9#10#11#12#13#14#15#16#17#18#19#20#21#22#23#24#25#26#27#28#29#30#31#32#33#34#35#36";

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【选号玩法】");
            builder.Append("赔率:" + ub.GetSub("Hc1odds1", xmlPath) + "<br />");
            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");

            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (("," + choose + ",").Contains("," + pvTemp[i] + ","))
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");
                if ((i + 1) % 6 == 0 && i != 35)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注数字(1-36):/,每数字押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + choose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + choose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【生肖玩法】");
            builder.Append("赔率:" + ub.GetSub("Hc1odds2", xmlPath) + "");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "选择生肖:/,每生肖押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "multiple,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "鼠'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "鼠''" + ptype + "'paysave";
            string strEmpt = "鼠|鼠" + OutSxNum("鼠") + "|牛|牛" + OutSxNum("牛") + "|虎|虎" + OutSxNum("虎") + "|兔|兔" + OutSxNum("兔") + "|龙|龙" + OutSxNum("龙") + "|蛇|蛇" + OutSxNum("蛇") + "|马|马" + OutSxNum("马") + "|羊|羊" + OutSxNum("羊") + "|猴|猴" + OutSxNum("猴") + "|鸡|鸡" + OutSxNum("鸡") + "|狗|狗" + OutSxNum("狗") + "|猪|猪" + OutSxNum("猪") + ",false,false,false";

            string strIdea = "/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (ptype == 3)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【方位玩法】");
            builder.Append("赔率:" + ub.GetSub("Hc1odds3", xmlPath) + "");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "选择方位:/,每方位押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "multiple,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "东'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "东''" + ptype + "'paysave";
            string strEmpt = "东|东|南|南|西|西|北|北,false,false,false";

            string strIdea = "/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("=方位说明=<br />");
            builder.Append("东:01,03,05,07,09,11,13,15,17<br />南:02,04,06,08,10,12,14,16,18<br />西:19,21,23,25,27,29,31,33,35<br />北:20,22,24,26,28,30,32,34,36");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 4)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【四季玩法】");
            builder.Append("赔率:" + ub.GetSub("Hc1odds4", xmlPath) + "");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "选择季度:/,每季度押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "multiple,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "春'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "春''" + ptype + "'paysave";
            string strEmpt = "春|春01 -- 09|夏|夏10 -- 18|秋|秋19 -- 27|冬|冬28 -- 36,false,false,false";

            string strIdea = "/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else if (ptype == 5)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【大小单双玩法】<br />");

            string pText = "大(" + ub.GetSub("Hc1da", xmlPath) + ")#小(" + ub.GetSub("Hc1xiao", xmlPath) + ")#单(" + ub.GetSub("Hc1dan", xmlPath) + ")#双(" + ub.GetSub("Hc1shuang", xmlPath) + ")";
            string pValue = "1#2#3#4";

            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");
            string getChoose = "";
            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (choose != "")
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");
            }
            if (choose != "")
            {
                string[] pbTemp = choose.Split(",".ToCharArray());
                for (int i = 0; i < pbTemp.Length; i++)
                {
                    getChoose += ptTemp[Convert.ToInt32(pbTemp[i]) - 1];

                }
                getChoose = Regex.Replace(getChoose, @"\(.+?\)", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注选项:/,押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + getChoose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + getChoose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(@"大为19-36<br />
小为1-18<br />
单为1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,33,35<br />
双为2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36
");
            builder.Append(Out.Tab("</div>", ""));

        }
        else if (ptype == 6)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【六肖中奖玩法】");
            builder.Append("赔率:" + ub.GetSub("Hc1odds9", xmlPath) + "<br />");
            builder.Append("请选择六个生肖<br />");
            string pText = "鼠#牛#虎#兔#龙#蛇#马#羊#猴#鸡#狗#猪";
            string pValue = "1#2#3#4#5#6#7#8#9#10#11#12";

            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");
            string getChoose = "";
            int cNum = Utils.GetStringNum(choose, ",");
            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (("," + choose + ",").Contains("," + pvTemp[i] + ",") || cNum >= 5)
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");

                if ((i + 1) % 4 == 0 && i != 11)
                    builder.Append("<br />");
            }
            if (choose != "")
            {
                string[] pbTemp = choose.Split(",".ToCharArray());
                for (int i = 0; i < pbTemp.Length; i++)
                {
                    getChoose += "," + ptTemp[Convert.ToInt32(pbTemp[i]) - 1];
                }
                getChoose = Utils.Mid(getChoose, 1, getChoose.Length);
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));


            string strText = "下注选项:/,押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + getChoose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + getChoose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(@"=生肖说明=<br />
鼠01、13、25  牛02、14、26<br />
虎03、15、27  兔04、16、28<br />
龙05、17、29  蛇06、18、30<br />
马07、19、31  羊08、20、32<br />
猴09、21、33  鸡10、22、34<br />
狗11、23、35  猪12、24、36
");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 7)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【尾数大小玩法】<br />");

            string pText = "大(" + ub.GetSub("Hc1wsda", xmlPath) + ")#小(" + ub.GetSub("Hc1wsxiao", xmlPath) + ")";
            string pValue = "1#2";

            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");
            string getChoose = "";
            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (choose != "")
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");
            }
            if (choose != "")
            {
                string[] pbTemp = choose.Split(",".ToCharArray());
                for (int i = 0; i < pbTemp.Length; i++)
                {
                    getChoose += ptTemp[Convert.ToInt32(pbTemp[i]) - 1];
                }
                getChoose = Regex.Replace(getChoose, @"\(.+?\)", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注选项:/,押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + getChoose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + getChoose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(@"0到4小,5到9大<br />例如开36，取尾数6为尾数大小来定输赢");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 8)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【尾数单双玩法】<br />");

            string pText = "单(" + ub.GetSub("Hc1wsdan", xmlPath) + ")#双(" + ub.GetSub("Hc1wsshuang", xmlPath) + ")";
            string pValue = "1#2";

            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");
            string getChoose = "";
            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (choose != "")
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");
            }
            if (choose != "")
            {
                string[] pbTemp = choose.Split(",".ToCharArray());
                for (int i = 0; i < pbTemp.Length; i++)
                {
                    getChoose += ptTemp[Convert.ToInt32(pbTemp[i]) - 1];
                }
                getChoose = Regex.Replace(getChoose, @"\(.+?\)", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注选项:/,押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + getChoose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + getChoose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(@"单1,3,5,7,9<br />双0,2,4,6,8<br />例如开36，取尾数6为尾数单双来定输赢");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 9)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【家禽野兽玩法】<br />");

            string pText = "家禽(" + ub.GetSub("Hc1odds14", xmlPath) + ")#野兽(" + ub.GetSub("Hc1odds15", xmlPath) + ")";
            string pValue = "1#2";

            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");
            string getChoose = "";
            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (choose != "")
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");
            }
            if (choose != "")
            {
                string[] pbTemp = choose.Split(",".ToCharArray());
                for (int i = 0; i < pbTemp.Length; i++)
                {
                    getChoose += ptTemp[Convert.ToInt32(pbTemp[i]) - 1];
                }
                getChoose = Regex.Replace(getChoose, @"\(.+?\)", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注选项:/,押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + getChoose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + getChoose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(@"家禽:牛马羊鸡狗猪.<br />野兽:鼠猴兔虎龙蛇.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 10)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【自选不中玩法】<br />");
            builder.Append("自选不中赔率:<br />五不中(" + ub.GetSub("Hc1odds16", xmlPath) + ")#六不中(" + ub.GetSub("Hc1odds17", xmlPath) + ")#七不中(" + ub.GetSub("Hc1odds18", xmlPath) + ")<br />八不中(" + ub.GetSub("Hc1odds19", xmlPath) + ")#九不中(" + ub.GetSub("Hc1odds20", xmlPath) + ")#十不中(" + ub.GetSub("Hc1odds21", xmlPath) + ")<br />");
            builder.Append("请选择5-10号码<br />");
            string pText = "01#02#03#04#05#06#07#08#09#10#11#12#13#14#15#16#17#18#19#20#21#22#23#24#25#26#27#28#29#30#31#32#33#34#35#36";
            string pValue = "1#2#3#4#5#6#7#8#9#10#11#12#13#14#15#16#17#18#19#20#21#22#23#24#25#26#27#28#29#30#31#32#33#34#35#36";

            string[] ptTemp = pText.Split("#".ToCharArray());
            string[] pvTemp = pValue.Split("#".ToCharArray());

            string choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,48}$", "");
            int cNum = Utils.GetStringNum(choose, ",");
            for (int i = 0; i < pvTemp.Length; i++)
            {
                string choose2 = pvTemp[i];
                if (choose != "")
                    choose2 = choose + "," + choose2;

                if (("," + choose + ",").Contains("," + pvTemp[i] + ",") || cNum >= 9)
                    builder.Append("<b>" + ptTemp[i] + "</b> ");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + choose2 + "") + "\">" + ptTemp[i] + "</a> ");
                if ((i + 1) % 6 == 0 && i != 35)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(3, ptype, choose);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注数字(选择5-10个):/,押多少" + ub.Get("SiteBz") + ":/,,,";
            string strName = "Vote,PayCent,ptype,act";
            string strType = "text,num,hidden,hidden";
            string strValu = string.Empty;
            if (PayCent > 0)
                strValu = "" + choose + "'" + PayCent + "'" + ptype + "'paysave";
            else
                strValu = "" + choose + "''" + ptype + "'paysave";
            string strEmpt = "false,false,false,false";

            string strIdea = "<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>'''|/";
            string strOthe = "确定下注,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">返回</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void PaySavePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;下注确认");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.Model.Game.HcList hc = new BCW.BLL.Game.HcList().GetHcListNew(0);
        if (hc == null)
        {
            Utils.Error("请等待管理员开通下期。。。", "");
        }
        if (hc.EndTime < DateTime.Now)
        {
            Utils.Error("截止时间已到，请下期下注", "");
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-9]$", "类型选择错误"));
        long PayCent = int.Parse(Utils.GetRequest("PayCent", "post", 2, @"^[1-9]\d*$", "下注额填写错误"));


        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info != "ok")
        {
            string Vote = Utils.GetRequest("Vote", "post", 1, "", "");
            Master.Title = "押注确认";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + hc.CID + "期.");
            if (hc.EndTime < DateTime.Now)
                builder.Append("系统正在开奖。。。");
            else
            {
                string hc1 = new BCW.JS.somejs().newDaojishi("xk2", hc.EndTime.AddMinutes(10).AddSeconds(-10));
                builder.Append("距离截止还有" + hc1 + " ");
            }

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=押注确认=<br />");
            builder.Append("押注项目:" + Vote + "<br />");
            if (ptype == 1)
                builder.Append("每数字下注:" + PayCent + "" + ub.Get("SiteBz") + "<br />");
            else if (ptype == 2)
                builder.Append("每生肖下注:" + PayCent + "" + ub.Get("SiteBz") + "<br />");
            else if (ptype == 3)
                builder.Append("每方位下注:" + PayCent + "" + ub.Get("SiteBz") + "<br />");
            else if (ptype == 4)
                builder.Append("每季度下注:" + PayCent + "" + ub.Get("SiteBz") + "<br />");
            else
                builder.Append("下注:" + PayCent + "" + ub.Get("SiteBz") + "<br />");


            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append("你目前自带:" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));

            string strName = "Vote,PayCent,ptype,act,info";
            string strValu = "" + Vote + "'" + PayCent + "'" + ptype + "'paysave'ok";
            string strOthe = "确定下注,hc1.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string Vote = "";
            int iNum = 0;
            if (ptype == 1)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[\d]{1,2}(?:(,|，)[\d]{1,2}){0,35}$", "输入号码有误");
                Vote = Vote.Replace("，", ",");
                string[] vTemp = Vote.Split(',');
                for (int i = 0; i < vTemp.Length; i++)
                {
                    int BuyNum = Convert.ToInt32(vTemp[i]);
                    if (BuyNum < 1 || BuyNum > 36)
                    {
                        Utils.Error("下注数字限1-36", "");
                    }
                }
                iNum = vTemp.Length;
            }
            else if (ptype == 2)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\^](?:(,|;)[^\^]){0,12}$", "选择生肖有误");
                Vote = Vote.Replace(";", ",");
                string[] vTemp = Vote.Split(',');
                for (int i = 0; i < vTemp.Length; i++)
                {
                    string sVote = vTemp[i];

                    string strSX = "#鼠#牛#虎#兔#龙#蛇#马#羊#猴#鸡#狗#猪#";
                    if (!strSX.Contains(sVote))
                    {
                        Utils.Error("生肖填写错误", "");
                    }
                    int cNum = Utils.GetStringNum("," + Vote.Replace(",", ",,") + ",", "," + sVote + ",");
                    if (cNum > 1)
                    {
                        Utils.Error("生肖“" + sVote + "”填写重复", "");
                    }
                }
                iNum = vTemp.Length;
            }
            else if (ptype == 3)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\^](?:(,|;)[^\^]){0,4}$", "选择方位有误");
                Vote = Vote.Replace(";", ",");
                string[] vTemp = Vote.Split(',');
                for (int i = 0; i < vTemp.Length; i++)
                {
                    string sVote = vTemp[i];

                    string strSX = "#东#南#西#北#";
                    if (!strSX.Contains(sVote))
                    {
                        Utils.Error("方位填写错误", "");
                    }
                    int cNum = Utils.GetStringNum("," + Vote.Replace(",", ",,") + ",", "," + sVote + ",");
                    if (cNum > 1)
                    {
                        Utils.Error("方位“" + sVote + "”填写重复", "");
                    }
                }
                iNum = vTemp.Length;
            }
            else if (ptype == 4)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\^](?:(,|;)[^\^]){0,4}$", "选择季度有误");
                Vote = Vote.Replace(";", ",");
                string[] vTemp = Vote.Split(',');
                for (int i = 0; i < vTemp.Length; i++)
                {
                    string sVote = vTemp[i];

                    string strSX = "#春#夏#秋#冬#";
                    if (!strSX.Contains(sVote))
                    {
                        Utils.Error("季度填写错误", "");
                    }
                    int cNum = Utils.GetStringNum("," + Vote.Replace(",", ",,") + ",", "," + sVote + ",");
                    if (cNum > 1)
                    {
                        Utils.Error("季度“" + sVote + "”填写重复", "");
                    }
                }
                iNum = vTemp.Length;
            }
            else if (ptype == 5)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^大$|^小$|^单$|^双$", "选择大小单双有误");
                iNum = 1;
            }
            else if (ptype == 6)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\^](?:,[^\^]){5}$", "选择生肖有误，请选择6个生肖");
                Vote = Vote.Replace(";", ",");
                string[] vTemp = Vote.Split(',');

                for (int i = 0; i < vTemp.Length; i++)
                {
                    string sVote = vTemp[i];

                    string strSX = "#鼠#牛#虎#兔#龙#蛇#马#羊#猴#鸡#狗#猪#";
                    if (!strSX.Contains(sVote))
                    {
                        Utils.Error("生肖填写错误", "");
                    }
                    int cNum = Utils.GetStringNum("," + Vote.Replace(",", ",,") + ",", "," + sVote + ",");
                    if (cNum > 1)
                    {
                        Utils.Error("生肖“" + sVote + "”填写重复", "");
                    }
                }
                iNum = 1;
            }
            else if (ptype == 7)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^大$|^小$", "选择尾数大小有误");
                iNum = 1;
            }
            else if (ptype == 8)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^单$|^双$", "选择尾数单双有误");
                iNum = 1;
            }
            else if (ptype == 9)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^家禽$|^野兽$", "选择家禽野兽有误");
                iNum = 1;
            }
            else if (ptype == 10)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[\d]{1,2}(?:(,|，)[\d]{1,2}){4,9}$", "输入号码有误，请选择5-10个号码");
                Vote = Vote.Replace("，", ",");
                string[] vTemp = Vote.Split(',');
                for (int i = 0; i < vTemp.Length; i++)
                {
                    int BuyNum = Convert.ToInt32(vTemp[i]);
                    if (BuyNum < 1 || BuyNum > 36)
                    {
                        Utils.Error("下注数字限1-36", "");
                    }
                }
                iNum = 1;
            }

            //支付安全提示
            string[] p_pageArr = { "ac", "act", "ptype", "PayCent", "Vote", "info" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            //是否刷屏
            long big = Convert.ToInt64(ub.GetSub("Hc1BigPay", xmlPath));
            long small = Convert.ToInt64(ub.GetSub("Hc1SmallPay", xmlPath));
            string appName = "LIGHT_HC1";
            int Expir = Utils.ParseInt(ub.GetSub("Hc1Expir", xmlPath));

            BCW.User.Users.IsFresh(appName, Expir, PayCent, small, big);

            long gold = new BCW.BLL.User().GetGold(meid);
            long PayCents = Convert.ToInt64(PayCent * iNum);
            if (PayCents > gold)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }

            long IDPayCents = 0;
            long AllPayCents = 0;
            string bzText = ub.Get("SiteBz");
            long Cent = PayCent;

            long myPayCents = new BCW.BLL.Game.HcPay().GetPayCent(meid, hc.CID);
            IDPayCents = Utils.ParseInt64(ub.GetSub("Hc1Max", xmlPath));
            AllPayCents = Utils.ParseInt64(ub.GetSub("Hc1Max2", xmlPath));


            //每ID每期下注金币上限:
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
            //每期总下注金币上限:
            if (AllPayCents > 0)
            {
                if (Cent + hc.payCent > AllPayCents)
                {
                    if (hc.payCent >= AllPayCents)
                    {
                        Utils.Error("系统限制每期下注" + AllPayCents + "" + bzText + "，欢迎在下期下注", "");
                    }
                    else
                    {
                        Utils.Error("系统限制每期下注" + AllPayCents + "" + bzText + "，你现在最多可以下注" + (AllPayCents - hc.payCent) + "" + bzText + "", "");
                    }
                }
            }
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.Game.HcPay model = new BCW.Model.Game.HcPay();
            model.CID = hc.CID;
            model.Types = ptype;
            model.UsID = meid;
            model.UsName = mename;
            model.PayCent = PayCent;
            model.PayCents = PayCents;
            model.Vote = Vote;
            model.Result = "";
            model.WinCent = 0;
            model.State = 0;
            model.IsSpier = 0;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Game.HcPay().Add(model);
            //遍历标识id
            DataSet ds = new BCW.BLL.Game.HcPay().GetList("Top 1 ID ", "UsID=" + meid + " order by addtime desc");
            int maxid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            //扣币
            new BCW.BLL.User().UpdateiGold(meid, mename, -PayCents, "" + GameName + "第" + hc.CID + "期" + ForType(ptype) + "投注:" + Vote + "|标识ID" + maxid);
            //邵广林 20160908 增加ID记录投注
            new BCW.BLL.User().UpdateiGold(106, new BCW.BLL.User().GetUsName(106), PayCents, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第" + hc.CID + "期" + ForType(ptype) + "投注:" + Vote + "|标识ID" + maxid);

            //动态 邵广林 增加游戏动态
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/hc1.aspx]" + GameName + "[/url]下注**" + ub.Get("SiteBz") + "";
            new BCW.BLL.Action().Add(1015, maxid, meid, "", wText);

            //更新本期数据
            new BCW.BLL.Game.HcList().Update1(hc.CID, PayCents, 1);
            //活跃抽奖入口_20160621姚志光
            try
            {
                //表中存在大小掷骰记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName("天天好彩"))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (PayCents > new BCW.BLL.tb_WinnersGame().GetPrice("天天好彩"))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "天天好彩", 3);
                        if (hit == 1)
                        {
                            //内线开关 1开
                            if (WinnersGuessOpen == "1")
                            {
                                //发内线到该ID
                                new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                            }
                        }
                    }
                }
            }
            catch { }
            Utils.Success("下注", "下注成功，花费" + PayCents + "" + ub.Get("SiteBz") + "<br /><a href=\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("hc1.aspx"), "2");

        }
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
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;" + strTitle + "");
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

        string Hcqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.HcPay> listHcPay = new BCW.BLL.Game.HcPay().GetHcPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.HcPay n in listHcPay)
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

                if (n.CID.ToString() != Hcqi)
                    builder.Append("=第" + n.CID + "期=<br />");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                if (n.State == 0)
                    builder.Append("<b>[" + ForType(n.Types) + "]</b>押" + n.Vote + "/每项下注" + n.PayCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                else
                {
                    builder.Append("<b>[" + ForType(n.Types) + "]</b>押" + n.Vote + "/每项下注" + n.PayCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }

                Hcqi = n.CID.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        Master.Title = "历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;历史开奖");
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
        IList<BCW.Model.Game.HcList> listHcList = new BCW.BLL.Game.HcList().GetHcLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcList.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.HcList n in listHcList)
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

                builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("hc1.aspx?act=listview&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.Result + "," + OutSx(n.Result) + "," + OutSj(n.Result) + "," + OutFw(n.Result) + "</b></a>");

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
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.CID + "期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "CID=" + model.CID + " and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.HcPay> listHcPay = new BCW.BLL.Game.HcPay().GetHcPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "," + OutSx(model.Result) + "," + OutSj(model.Result) + "," + OutFw(model.Result) + "</b>");
            builder.Append("<br />共" + recordCount + "注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.HcPay n in listHcPay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>获得" + n.WinCent + "" + ub.Get("SiteBz") + "");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "," + OutSx(model.Result) + "," + OutSj(model.Result) + "," + OutFw(model.Result) + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.HcPay().ExistsState(pid, meid))
        {
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.Game.HcPay().GetWinCent(pid));
            //税率
            long SysTax = 0;
            //期号
            BCW.Model.Game.HcPay model = new BCW.BLL.Game.HcPay().GetHcPay(pid);
            long number = Convert.ToInt64(model.CID);
            winMoney = winMoney - SysTax;
            BCW.User.Users.IsFresh("hc1", 1);//防刷
            new BCW.BLL.Game.HcPay().UpdateState(pid);
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "兑奖-期号-" + number + "-标识ID" + pid + "");
            //邵广林 20160908 增加ID记录投注
            new BCW.BLL.User().UpdateiGold(106, new BCW.BLL.User().GetUsName(106), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第" + number + "期兑奖" + winMoney + "|标识ID" + pid);
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("hc1.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("hc1.aspx?act=case"), "1");
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
        BCW.User.Users.IsFresh("hc1", 1);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BLL.Game.HcPay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.HcPay().UpdateState(pid);
                //操作币
                winMoney = Convert.ToInt64(new BCW.BLL.Game.HcPay().GetWinCent(pid));
                //税率
                long SysTax = 0;
                winMoney = winMoney - SysTax;
                //期号
                BCW.Model.Game.HcPay model = new BCW.BLL.Game.HcPay().GetHcPay(pid);
                long number = Convert.ToInt64(model.CID);
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "兑奖-期号-" + number + "-标识ID" + pid + "");
                //邵广林 20160908 增加ID记录投注
                new BCW.BLL.User().UpdateiGold(106, new BCW.BLL.User().GetUsName(106), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第" + number + "期兑奖" + winMoney + "|标识ID" + pid);
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("hc1.aspx?act=case"), "1");
    }


    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;兑奖中心");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and WinCent>0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<BCW.Model.Game.HcPay> listHcPay = new BCW.BLL.Game.HcPay().GetHcPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.HcPay n in listHcPay)
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
                builder.Append("[第" + n.CID + "期].");
                builder.Append("<b>[" + ForType(n.Types) + "]</b>押" + n.Vote + "/每项下注" + n.PayCent + "" + ub.Get("SiteBz") + "/赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 13) + "]<a href=\"" + Utils.getUrl("hc1.aspx?act=caseok&amp;pid=" + n.id + "") + "\">兑奖</a>");

                arrId = arrId + " " + n.id;
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
            string strOthe = "本页兑奖,hc1.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void HelpPage()
    {
        Master.Title = "" + GameName + "玩法规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;玩法规则");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(ub.GetSub("Hc1Rule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage()
    {
        Master.Title = "" + GameName + "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "PayCents>0";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        IList<BCW.Model.Game.HcPay> listHcPay = new BCW.BLL.Game.HcPay().GetHcPaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.HcPay n in listHcPay)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private string ForType(int Types)
    {

        string TyName = string.Empty;
        if (Types == 1)
            TyName = "选号玩法";
        else if (Types == 2)
            TyName = "生肖玩法";
        else if (Types == 3)
            TyName = "方位玩法";
        else if (Types == 4)
            TyName = "四季玩法";
        else if (Types == 5)
            TyName = "大小单双";
        else if (Types == 6)
            TyName = "六肖中奖";
        else if (Types == 7)
            TyName = "尾数大小";
        else if (Types == 8)
            TyName = "尾数单双";
        else if (Types == 9)
            TyName = "家禽野兽";
        else if (Types == 10)
            TyName = "自选不中";

        return TyName;
    }

    private string OutSxNum(string sx)
    {
        string sNum = "";
        if (sx == "鼠")
            sNum = "01#13#25";
        else if (sx == "牛")
            sNum = "02#14#26";
        else if (sx == "虎")
            sNum = "03#15#27";
        else if (sx == "兔")
            sNum = "04#16#28";
        else if (sx == "龙")
            sNum = "05#17#29";
        else if (sx == "蛇")
            sNum = "06#18#30";
        else if (sx == "马")
            sNum = "07#19#31";
        else if (sx == "羊")
            sNum = "08#20#32";
        else if (sx == "猴")
            sNum = "09#21#33";
        else if (sx == "鸡")
            sNum = "10#22#34";
        else if (sx == "狗")
            sNum = "11#23#35";
        else if (sx == "猪")
            sNum = "12#24#36";

        return sNum;
    }

    private string OutSx(string Result)
    {
        string sx = "";
        if ((",1,13,25,").Contains("," + Result + ","))
            sx = "鼠";
        else if ((",2,14,26,").Contains("," + Result + ","))
            sx = "牛";
        else if ((",3,15,27,").Contains("," + Result + ","))
            sx = "虎";
        else if ((",4,16,28,").Contains("," + Result + ","))
            sx = "兔";
        else if ((",5,17,29,").Contains("," + Result + ","))
            sx = "龙";
        else if ((",6,18,30,").Contains("," + Result + ","))
            sx = "蛇";
        else if ((",7,19,31,").Contains("," + Result + ","))
            sx = "马";
        else if ((",8,20,32,").Contains("," + Result + ","))
            sx = "羊";
        else if ((",9,21,33,").Contains("," + Result + ","))
            sx = "猴";
        else if ((",10,22,34,").Contains("," + Result + ","))
            sx = "鸡";
        else if ((",11,23,35,").Contains("," + Result + ","))
            sx = "狗";
        else if ((",12,24,36,").Contains("," + Result + ","))
            sx = "猪";

        return sx;
    }

    private string OutFw(string Result)
    {
        string fw = "";
        if ((",1,3,5,7,9,11,13,15,17,").Contains("," + Result + ","))
            fw = "东";
        else if ((",2,4,6,8,10,12,14,16,18,").Contains("," + Result + ","))
            fw = "南";
        else if ((",19,21,23,25,27,29,31,33,35,").Contains("," + Result + ","))
            fw = "西";
        else if ((",20,22,24,26,28,30,32,34,36,").Contains("," + Result + ","))
            fw = "北";

        return fw;
    }

    private string OutSj(string Result)
    {
        string sj = "";
        if ((",1,2,3,4,5,6,7,8,9,").Contains("," + Result + ","))
            sj = "春";
        if ((",10,11,12,13,14,15,16,17,18,").Contains("," + Result + ","))
            sj = "夏";
        if ((",19,20,21,22,23,24,25,26,27,").Contains("," + Result + ","))
            sj = "秋";
        if ((",28,29,30,31,32,33,34,35,36,").Contains("," + Result + ","))
            sj = "冬";

        return sj;
    }


    //快捷下注
    private void kuai(int type, int ptype, string Num1)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!new BCW.XinKuai3.BLL.Public_User().Exists(meid, type))//添加默认的快捷下注
        {
            BCW.XinKuai3.Model.Public_User model = new BCW.XinKuai3.Model.Public_User();
            model.UsID = meid;
            model.UsName = new BCW.BLL.User().GetUsName(meid);
            model.Type = 3;//1新快3、2幸运28、3好彩一、4进球彩、5百家乐
            model.Settings = "100#500#1000#10000#100000#0#0#0#0#0";
            new BCW.XinKuai3.BLL.Public_User().Add(model);
        }

        //查询数据库对应的快捷
        DataSet ds = new BCW.XinKuai3.BLL.Public_User().GetList("*", "UsID=" + meid + " and type=" + type + "");
        int tt = int.Parse(ds.Tables[0].Rows[0]["type"].ToString());
        if (tt != type)
        {
            Utils.Error("抱歉,你不能修改其他游戏的快捷.", "");
        }
        string kuai = ds.Tables[0].Rows[0]["Settings"].ToString();
        string gold = string.Empty;
        string[] k = kuai.Split('#');//取出对应的快捷下注
        for (int i = 0; i < k.Length; i++)
        {
            if (k[i] != "0")
            {
                gold = ChangeToWan(k[i]);
                builder.Append("<a href =\"" + Utils.getUrl("hc1.aspx?act=pay&amp;ptype=" + ptype + "&amp;choose=" + Num1 + "&amp;PayCent=" + Convert.ToInt64(k[i]) + "") + "\">" + gold + "</a>" + "|");
            }
        }

        builder.Append("<a href=\"" + Utils.getUrl("Public_set.aspx?type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");
    }

    private string ChangeToWan(string str)
    {
        string CW = string.Empty;
        try
        {
            if (str != "")
            {
                long first = 0;
                first = Convert.ToInt64(str.Trim());
                if (first >= 10000)
                {
                    if (first % 10000 == 0)
                    {
                        CW = (first / 10000) + "万";
                    }
                    else
                    {
                        CW = (first / 10000) + ".X万";
                    }
                }
                else
                {
                    CW = first.ToString();
                }
            }
        }
        catch { }
        return CW;
    }
}
