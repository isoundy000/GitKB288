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

public partial class bbs_guess2_help : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "规则说明";

        string act = "";
        act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "info1":
                Info1();
                break;
            case "info2":
                Info2();
                break;
            case "info3":
                Info3();
                break;
            case "info4":
                Info4();
                break;
            case "info5":
                Info5();
                break;
            case "info6":
                Info6();
                break;
            default:
                ReloadPage();
                break;
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回竞猜首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("live.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "竞猜") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("竞猜规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("help.aspx?act=info1"), "1.基本玩法诠释") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx?act=info2"), "2.让球盘玩法诠释") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx?act=info3"), "3.特殊问题附加说明") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx?act=info4"), "4." + ub.Get("SiteGqText") + "、串串规则") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx?act=info5"), "5.竞猜确认说明") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx?act=info6"), "6.球赛争议补充说明") + "<br />");
        builder.Append("以上规则解释权归本站所有！");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Info2()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("让球盘玩法诠释");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1、让半球：让球方胜一球和胜一球以上，让球方赢盘，打平及对方胜一球及胜一球以上，让球方输盘。<br />2、让平手：打平走盘，任何一方胜一球及胜一球以上，胜球方嬴盘。<br />3、让一球，让球方胜一球走盘，让球方胜二球和胜二球以上，让球方赢盘，打平及对方胜一球以上，让球方输盘。<br />4、让一球半：让球方胜二球和胜二球以上，让球方赢盘，让球方胜一球、打平及对方胜一球以上，让球方输盘。<br />5、让二球：让球方胜二球走盘，让球方胜三球和胜三球以上，让球方赢盘，让球方胜一球、打平及对方胜一球以上，让球方输盘。<br />6、让二球半：让球方胜三球和胜三球以上，让球方赢盘，让球方胜二球、打平及对方胜一球及一球以上，让球方输盘。<br />7、让平手/半球：打平,让方输半,对方赢半,让方胜一球及以上让方全嬴,让方输一球及以上为全输。<br />8、让半球/一球：打平,让方全输,对方全赢,让方胜一球让方赢半，对方输半，让方胜二球及以上让方全嬴,让方输一球及以上为全输。<br />9、 让一球/球半：让球方胜二球和胜二球以上，让球方赢盘，让球方胜一球、让方输半,对方赢半，打平及对方胜一球以上，让球方输盘。<br />10、 让两球/两球半：让球方胜三球和胜三球以上，让球方赢盘，让球方胜两球、让方输半,对方赢半，打平及对方胜一球以上，让球方输盘。<br />依此类推...<br />");

        builder.Append(Out.waplink(Utils.getUrl("help.aspx"), "&lt;&lt;返回规则列表") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Info1()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("规则说明");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("提供了足球和篮球两种竞猜游戏<br /><b>让球盘（让球盘）</b>玩法，针对比赛结果竞猜，足球比赛是按照90分钟以及补时的正赛来下注，不包括加时赛或者点球分胜负。例如：8日20时45分,曼彻斯特联队(1.9)一球半朴茨茅夫(2.0)这中间的1.9和2.0是指针对你压注对象的赔率，加入你压曼联100金币，并且上盘赢了，你会获得系统返还金币190个，如果你输了，你所压金币将被没收 ，最小竞猜金币是10个金币。另外大家一定要看好竞猜内容，这里的竞猜和彩票的胜负平可不太一样，这里是直接写明了一种结果，比如曼联让朴茨茅夫一球半，那么如果曼联只赢了一球或者打平或者曼联输球，那么竞猜曼联的都算输，只有当曼联赢对方2球以及以上才能算这次竞猜上盘获得胜利。<br />8日10时00分,菲尼克斯太阳[主](1.9)让4分犹他爵士(1.9)，篮球的道理同样，太阳必须赢爵士4分以上才 能算上盘胜利，如果正好赢4分，算平盘，返还你的竞猜额。<br /><b>比分大小和标准盘（欧盘）</b>玩法,比如湖人和凯尔特人大小球是(180分),那么双方总分总和小于180就是小,大于180就是大.标准盘的玩法就是赌最后哪一方获胜或者平手<br /><b>" + ub.Get("SiteGqText") + "（走地）</b>玩法，" + ub.Get("SiteGqText") + "让球盘是以完场比分，减去当时下注之前的比分来计算；大小盘则以完场比分计算。" + ub.Get("SiteGqText") + "赛事在比赛开始至结束都可以下注。<br /><b>赔率知识</b>：返彩赔率含本金，计算：赔率为1.9,下注100金,全赢返彩额为:100金(本金)+90金(彩金)=190金,赢半:赔率由1.9变为1.45,返彩额为:100金(本金)+45金(彩金)=145金。输半：赔率变为0.5,返回本金的一半(50金)。<br />注意玩标准盘时，排列在最前面的球队属于主队，即主胜，排列在后面的属于客胜，不论有没有主队和客队之分的中立场的比赛。<br />");

        builder.Append(Out.waplink(Utils.getUrl("help.aspx"), "&lt;&lt;返回规则列表") + "");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void Info3()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("特殊问题附加说明");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(@"
1.推迟（改期）的没显示推迟时间的即时退回本金。<br />
2.待定、腰斩、中断的比赛以比赛开始的时间算起，4小时后还没完场比分的平盘处理、平盘处理后如再有比分将不再重新返奖；若下半场才腰斩、中断的比赛，上半场竞猜按正常开奖。(如管理员处理不及时，比赛待定4小时以上，有完场比分并已按正常比分返奖的比赛，不再重新平盘返奖)。<br />
3.主客反了（中立场除外）、球队名称、盘口出错等错盘退回本金。<br />
4.比赛开始后还没处理退回本金的（让盘、大小，标准盘）谁错谁就无效，例如：让盘出错，大小球没错为有效，让盘退回本金。即哪个盘口出错，出错的盘口为无效。<br />
5.比赛提前开赛的,开赛之前已投注为有效,开赛之后的投注退回本金。<br />
6.返奖时的赔率将以系统确认的赔率为准，投注后请在未开投注中查看，有异议的竞猜3分钟内提出，超过则视为认同。<br />
7.滚球和初盘投注后请留意查看是否投注成功，完场后有异议的不受理。<br />
8系统提供的联赛名称、球队名译音，在没有明显错误(例如：欧冠杯写成中超，利物浦VS切尔西写成利物浦VS上海申花等明显错误)的情况下，所有有效投注（含滚球投注）均正常返彩。(特别是有友谊赛性质的比赛，例如：四国赛，邀请赛等，有可能统译为友谊赛，但不影响正常返奖)。<br />
9:滚球比赛进行时间，足球（篮球）滚球比分并不能确保该比赛时间，比赛比分的100%准确性，投注滚球游戏时仅供参考，请自行斟别。由于足球比分错误直接影响到返奖结果。故足球比分错误时的投注上下盘、大小盘、标准盘一律无效。恶意或多次刷比分错误，警告无效的，按利用系统漏洞刷币处理。<br />
10.对特殊的比赛，如只打45分钟半场或不足正常90分钟（如德国电信杯等）的足球，系统没说明，完场后按皇冠比分开奖。<br />
11.竞猜比赛有出错的的比赛完场后平盘处理。如当时未查处，过后同样处理扣回非正常盈利，利用非盈利再竞猜赢到的盈利系统扣回。<br />
");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx"), "&lt;&lt;返回规则列表") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Info4()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + ub.Get("SiteGqText") + "、串串规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("一、足球" + ub.Get("SiteGqText") + "规则：<br />");
        builder.Append("1.以正常比赛时间包括补时来计算、不包括加时和点球.<br />");
        builder.Append("2.让球盘是以你竞猜时的盘口赔率为准,不包括已入球的比分.<br />");
        builder.Append("3.大小盘以全场入球比分计算,包括已入球的比分.<br />");
        builder.Append("4.标准盘以全场完场比分计算.<br />");
        builder.Append("二、篮球" + ub.Get("SiteGqText") + "规则：<br />");
        builder.Append("1.以正常比赛时间包括加时赛来计算.<br />");
        builder.Append("2.让球盘、大小盘(总分盘)以全场完场比分计算.<br />");
        builder.Append("其他规则:<br />");
        builder.Append("1.其它规则按照竞猜规定执行.<br />");
        builder.Append("2.盘口赔率根据皇冠公司所得.<br />");
        builder.Append("3.按照竞猜惯例出错的竞猜退回本金.(例进球总数已经是3个了," + ub.Get("SiteGqText") + "大小盘口还是2.5球、例没" + ub.Get("SiteGqText") + "的比赛在列表上有本场" + ub.Get("SiteGqText") + "等原因出错)<br />");
        builder.Append("三、串串规则：<br />");
        builder.Append("1.每场只能选择一个盘口加入串关.如有重复同一场比赛也只能选择某一场的一个盘口,如有重复比赛或在比赛中选择了多个盘口为一条串,多出的本个盘口无效，其它比赛为有效,如减去多出的盘口少于三场比赛者本串无效。<br />");
        builder.Append("2.盘口赔率根据你竞猜时的为准.<br />");
        builder.Append("3.竞猜赔率有最低准度.<br />");
        builder.Append("4.因赛事提前、腰斩、推迟、中断、待定而导致平盘的串除外,正常完场(含走盘)的串少于三场的串关将作无效处理并退回本金.<br />");
        builder.Append("5.走盘的比赛按乘1计算.(例让平手盘比赛,串了主或客完场结果为:0比0等无输赢的比赛为走盘).<br />");
        builder.Append("6.一场输半算串关成功,盈利除半.<br />");
        builder.Append("7.二场输半或一场全输视为串关失败.<br />");
        builder.Append("8.其它规定根据球彩竞猜的规定为准.<br />");
        builder.Append("四、本解释权归本站所有！<br />");

        builder.Append(Out.waplink(Utils.getUrl("help.aspx"), "&lt;&lt;返回规则列表") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Info5()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("竞猜确认说明");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(@"
1.  虚拟游戏竞猜初盘改为0秒确认，即在没有盘口或者水位变化超过0.1的情况下即时确认成功。<br />
2.  虚拟游戏竞猜滚球改为原超过0.05水位变化就失败改为不超过0.1的变化和盘口没有变动、没有封盘、不是危险球的情况下1分钟会自动确认成功，但保留因为其他情况出现的延迟，所以滚球确认时间还是为40秒到90秒之间，正常是约1分钟。滚球竞猜改了原0.05变化就失败改为了超过0.1才失败，也就是说少于0.1的水位变化都会确认成功。<br />
对于以上第1和第2点，站方如查出系统卡住或者更新数据失败的情况下，与核对数据存在很大错误的即为无效。如有非法下注或非法手段下注的就算确认成功或者完场后都给予无效处理。<br />
");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx"), "&lt;&lt;返回规则列表") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Info6()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("球赛争议补充说明");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(@"
1.球彩滚球内线确认问题：系统确认滚球的有效时间约为60秒，在这个时间段内，不论下注是否成功都有系统内线提醒，会员要特别特别注意查看下注后的系统内线提醒。对于会员反映，刚收到系统下注成功就入球这种情况如何处理：只要下注时的比分，大小盘，让球盘这些盘口都一致，无论输赢，都以系统确认的为准。<br />
2.足球比赛进度时间的差异：如：(54'@0:2),投100000酷币[2013-9-14 16:07:35]的这个显示比赛进度54'，由于各个球赛网的这个时间存在几分钟的差异，无法统一，故核对盘口是否准确是以会员投注单后的投注时间[2013-9-14 16:07:35]来与皇冠滚球数据进行核对，而不是依据如(54'@0:2)这个显示比赛进度(54')核对。<br />
3.篮球即时比分的差异：即时比分与直播有延迟，有可能没有同步显示，只能供参考；因于无法查阅篮球滚球某一时刻的即时比分，对投注时即时比分与直播不同步的，经核查盘口数据正确的都以系统确认为准。<br />
4..系统标注是友谊赛的篮球赛果如果是打平，经核查皇冠比分也是打平的，滚球和非滚球下注的都按平盘退本处理。<br />
最后，如对球赛和返奖有疑问，希望第一时间内线客服，让客服查询，由于系统还有更多待改善的地方，还望各位多多包涵!<br />
");
        builder.Append(Out.waplink(Utils.getUrl("help.aspx"), "&lt;&lt;返回规则列表") + "");
        builder.Append(Out.Tab("</div>", ""));
    }
}
