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

//蒙宗将 20160607  闯荡第零天直接结束显示问题
//姚志光 20160621  添加活跃抽奖控制方式
//蒙宗将 20161021  闯荡记录显示银两（原为酷币为错）获奖酷友查询地区一致 ,酷币使用"+ub.Get("SiteBz")+"
public partial class bbs_game_Dawnlife : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string GameName = ub.GetSub("DawnlifeName", "/Controls/Dawnlife.xml");
    protected string xmlPath = "/Controls/Dawnlife.xml";
    protected int pay = Convert.ToInt32(ub.GetSub("DawnlifePay", "/Controls/Dawnlife.xml"));
    protected string ceshi = Convert.ToString(ub.GetSub("ceshi", "/Controls/Dawnlife.xml"));
    protected string ceshiID = Convert.ToString(ub.GetSub("CeshiQualification", "/Controls/Dawnlife.xml"));

    protected double jcd = Convert.ToInt32(ub.GetSub("jcd", "/Controls/Dawnlife.xml"));
    protected double jcx = Convert.ToInt32(ub.GetSub("jcx", "/Controls/Dawnlife.xml"));
    protected double drd = Convert.ToInt32(ub.GetSub("drd", "/Controls/Dawnlife.xml"));
    protected double drq = Convert.ToInt32(ub.GetSub("drq", "/Controls/Dawnlife.xml"));
    protected double q1 = Convert.ToInt32(ub.GetSub("q1", "/Controls/Dawnlife.xml"));
    protected double q2 = Convert.ToInt32(ub.GetSub("q2", "/Controls/Dawnlife.xml"));
    protected double q3 = Convert.ToInt32(ub.GetSub("q3", "/Controls/Dawnlife.xml"));
    protected double q4 = Convert.ToInt32(ub.GetSub("q4", "/Controls/Dawnlife.xml"));
    protected double q5 = Convert.ToInt32(ub.GetSub("q5", "/Controls/Dawnlife.xml"));
    protected double juankuan = Convert.ToInt32(ub.GetSub("juankuan", "/Controls/Dawnlife.xml"));
    protected string dq = Convert.ToString(ub.GetSub("dq", "/Controls/Dawnlife.xml"));

    string[] f = new String[] { "【温馨提示】<br />做生意要有一颗敢于取舍的心", "【疯人疯语】<br />要想致富发家，就要放手一搏", "【温馨提示】<br />盗版DVD有损正版权益，支持正版", "【温馨提示】<br />及时升级仓库，可以存更多的物品", "【温馨提示】<br />勿常走夜路，小偷强盗夜里猖狂不休", "【游戏厅某高管】<br />[曝]用山寨机登陆游戏厅可获" + ub.Get("SiteBz") + "奖励,酷友争相抢购", "【温馨提示】<br />注意保暖防晒，身体是革命本钱 ", " 【友友内线】<br />山寨手机芯片出错无法登陆游戏厅", "【靓女杂志】<br />国际名牌联手降价促销,A货无销路 ", "【天下新闻】<br />三路奶粉含有三聚氰氨,全部停产,无人敢喝", "【美食报】<br />大闸蟹美味养颜千里传,纷纷抢购", "【科技日报】<br />中国硅谷---中关村全是卖盗版DVD的村姑", "【新泯晚报】<br />纺织业突破瓶颈,A货名牌足以乱真", "【温馨提示】<br />多看新闻，了解社会大事小事", "【山寨报】<br />山寨手机获中国最佳创造奖", "【今日美食】<br />市场上充斥着各种鱼目混珠的假冒大闸蟹", "【小道消息】<br />文盲说:2009年诺贝尔文学奖?呸!不如盗版DVD港台片", "【友情提示】<br />二手笔记本里存有最新艳照,价格疯涨", "", "【小道消息】<br />市场上充斥着来自福建的走私香烟", "【友友内线】<br />山寨手机芯片出错无法登陆游戏厅", };
    string[] f1 = new String[] { "【地球物语】<br />带走的花儿生命短暂，留下的花儿才是永远。敬请脚下留青。", "【安全提示】<br />要让爱车跑，车况要良好；开车别太快，系好安全带", "【安全提示】<br />出门要提前，堵车心不烦", "【温馨箴言】<br />行车要礼让，路口多张望", "【心灵鸡汤】<br />温馨是永远的伴侣，平安是共同的心愿", "【人生哲理】<br />你慢一点我慢一点，文明的步伐快一点；你让一点我让一点,舒心的笑容多一点；你讲安全我讲安全，美好生活到永远", "【天下社会】<br />据传少林绝学--如来神掌流失民间,大受欢迎街头巷尾相互抄送,价格上涨8倍", "【经济报】<br />海关调高关税10倍,走私汽车价格猛涨", "【名人名言】<br />有一种叮咛叫滴酒莫沾，有一种感觉叫望眼欲穿，有一种期盼叫一路平安，有一种温馨叫合家团圆", " 【少林密抄】<br />传少林高僧,闭关苦练如来神掌,终得武林第一", "【人生哲理】<br />你玩游戏可以，游戏玩你不可以；游戏不是人生，人生更不是游戏", "【新泯晚报】<br />有人说:喝假酒可治感冒!", "【娱乐八卦】<br />电影院全线打折降价,盗版DVD无人问津", "【小道消息】<br />酒吧生意火爆为增利润狂卖假酒", "【娱乐八卦】<br />电影院全线打折降价,盗版DVD无人问津", "【经济报】<br />海关调高关税10倍,走私汽车价格猛涨", "【天下社会】<br />据传少林绝学--如来神掌流失民间,大受欢迎街头巷尾相互抄送,价格上涨8倍", "【今日美食】<br />市场上充斥着各种鱼目混珠的假冒大闸蟹", "【小道消息】<br />市场上充斥着来自福建的走私香烟", "【新泯晚报】<br />有人说:喝假酒可治感冒!", "【小道消息】<br />酒吧生意火爆为增利润狂卖假酒", };
    string[] f2 = new String[] { "【人生哲理】<br />礼貌使你变得高雅，助人能使你得到快乐，谦让能使你增添美德", "【小资生活】<br />你喜爱的球队冠军杯失利,你心理受到严重打击.健康值下降1点.", "【好运连连】<br />在老乡的床底翻出了两本如来神掌", "【小资生活】<br />周末,建筑工地一大早发动机器,制造噪音,吵得你不能休息.健康值下降2点.", "【人生哲理】<br />礼貌使你变得高雅，助人能使你得到快乐，谦让能使你增添美德", "【天降财神】<br />在游戏厅中了大乐透,奖励1辆走私汽车", "【至理名言】<br />水是生命之源，请节约每一滴水，如果人类不节约水，那么最后一滴水将是人们的眼泪", "【天下社会】<br />你拿着火腿肠去逗邻居家小狗,它被你惹毛,咬了你一口,你得去医院打一针.健康值下降3点.", "【地球物语】<br />人与环境手拉手，我们都是好朋友", "【天下社会】<br />你经过建筑工地,被楼上掉下的杂物砸中.幸好没砸到你的脑子!健康值下降1点.", "【物语花语】<br />爱树如尊老，护花如爱小，草坪本来少，践踏无处找", "【友友社会】<br />外地人要使用第二代暂住证,必须回趟老家更换.现金减少5%", "【励志箴言】<br />不管现实有多惨不忍睹，你都要固执的相信，这只是黎明前暂时的黑暗而已", "【天下社会】<br />警察看你拿着油漆刷子,误以为你是贴黑广告的,你被捉去局里进行教育.你好冤啊!健康值下降5点..", "【人生哲理】<br />这个世界只有回不去的，而没有什么是过不去的", "【天下社会】<br />人口普查时,居委会阿姨们很不屑的眼神打量了你一番,明显看不起你是外地人.健康值下降1点.", "【人生哲理】<br />一万个美丽的未来，抵不上一个努力的现在", "【衰!】<br />路上碰见两个壮汉要卖包给俺,俺打不过他们,只能掏钱了...现金少了5%", "【人生哲理】<br />放弃只要一句话，坚持却要一辈子", "【友友报道】<br />公交车上见义勇为替漂亮mm捉小偷,小偷同伙把你殴打了一顿.你脸肿得跟猪一样也值得!健康值下降3点!", "【人生哲理】<br />你以为永远也走不尽的长路，其实也许是一座有头有尾的短桥", "【友友气象】<br />本市经历了一场历史罕见的低温雨雪天气,你家空调又坏了,修理工打死也不肯在这鬼天上门,健康值下降3点.", "【心灵鸡汤】<br />人生没有对错，只有选择后的坚持，不后悔，走下去，就是对的。走着走着，花就开了", "【财经报道】<br />政府开始征收燃油税,你的小毛驴加油尽然还要付税.天那!现金少了5%", "【名言警句】<br />没有理所当然的成功，也没有毫无道理的平庸", "【友友气象】<br />本市气温飙上39℃,酷热难耐,你走在大街上中暑了,健康值下降3点!", "【心灵鸡汤】<br />追求一切美好的过程都是人生珍贵的财富", "【小资生活】<br />你通宵看了两场球赛,精神已经濒临崩溃,健康值降低3点.", "【人生哲理】<br />一个人的知识，通过学习可以得到;一个人的成长，必须通过磨练", "【老军医】<br />你体力透支昏迷了一天,服用了老军医的灵丹妙药后已经康复,药资胖大海已经帮你垫付了", "【情感连线】<br />泪水和汗水的化学成分相似，但前者只能为你换来同情，后则却可以为你赢得成功", "【友友报道】<br />强台风来临,你破旧的小屋经不起摧残,健康值降低1点", "【心灵鸡汤】<br />任何的一颗心灵的成熟，那都是必须去经过寂寞的洗礼和孤独的磨炼", "【财经报道】<br />政府开始征收燃油税,你的小毛驴加油尽然还要付税.天那!现金少了5%", "【励志箴言】<br />你不勇敢，没人替你坚强！", "【娱乐在线】<br />节日逛街,商场人满为患,家庭主妇们的力量太强大,你被挤伤.健康值下降2点.", "【疯人疯语】<br />自信的生命最美丽！", "【天下社会】<br />你在夜市摆地摊,两个黑社会小混混问你收取保护费.你决定去练好跆拳道再跟他们对抗!现金减少5%", "【疯人疯语】<br />心有多大，舞台就有多大", "【实事新闻】<br />朋友请客吃夜宵,你恶吃了6个大螃蟹,还偷拿了旁边水果摊2个柿子,结果食物中毒!健康值降低3点!", "【励志箴言】<br />没有伞的孩子必须努力奔跑！", "【天下社会】<br />在广场摆地摊,大声吆喝扰乱共工秩序,被工商局和派出所围追.健康下降1点", "【人生哲理】<br />只做第一个我，不做第二个谁", "【心情日记】<br />房价大跌,股票套牢,老婆跑掉,打击严重!健康值下降10点!", "【情感连线】<br />用爱生活，你会使自己幸福！用爱工作，你会使很多人幸福！", "【衰!】<br />路上碰见两个壮汉要卖包给俺,俺打不过他们,只能掏钱了...现金少了5%", "【疯人疯语】<br />人生没有彩排，每天都是现场直播", "【老军医】<br />你体力透支昏迷了一天,服用了老军医的灵丹妙药后已经康复,药资胖大海已经帮你垫付了", "【古人云】<br />业精于勤，荒于嬉；行成于思，毁于随", "【友友在线】<br />做假账,被人举报,你必须去工商局一趟,肥油局长果然心狠手辣!你花了不少钱.现金少了10%", "【至理名言】<br />志在山顶的人，不会贪念山腰的风景", "【小道消息】<br />过年了,你带了礼物回了趟家,还要给大叔二叔三叔四叔五叔...八叔的女儿儿子,孙子孙女压岁钱,现金少了5%", "【人生哲理】<br />我走得很慢，但我从不后退！", "【心情日记】<br />房价大跌,股票套牢,老婆跑掉,打击严重!健康值下降10点!", "【慧眼禅心】<br />待人对事不要太计较，如果太计较就会有悔恨！", "【老军医】<br />你体力透支昏迷了一天,服用了老军医的灵丹妙药后已经康复,药资胖大海已经帮你垫付了----------因为住院你的欠款增加了9968银两.", "【至理名言】<br />朋友是路，家是树。别迷路，靠靠树", "【金融日报】<br />金融市场最强势的华尔街实践失败,你8年前买的基金尽然变成了废纸.现金下降5% ", "【心灵鸡汤】<br />一份耕耘一份收获，未必；九份耕耘一份收获，一定", "【至理名言】<br />对的，坚持；错的，放弃！", "【励志箴言】<br />站得更高才能看得更远", "【温馨提示】<br />幸福和幸运是需要代价的，天下没有免费的午餐！", "【疯人疯语】<br />好多人做不好自己，是因为总想着做别人！", "【温馨提示】<br />选山攀崖！量力而为！", "【人生哲理】<br />没有天生的信心，只有不断培养的信心", "【爱的箴言】<br />伟人之所以伟大，是因为他与别人共处逆境时，别人失去了信心，他却下决心实现自己的目标", "【温馨提示】<br />不要等待机会，而要创造机会", "【温馨提示】<br />每一发奋努力的背后，必有加倍的赏赐", "【至理名言】<br />人生伟业的建立，不在能知，乃在能行", "【世说新语】<br />世界上那些最容易的事情中，拖延时间最不费力", "【励志箴言】<br />成功不是将来才有的，而是从决定去做的那一刻起，持续累积而成", "【温馨提示】<br />笑口常开，好彩自然来！", "【疯人疯语】<br />牢记所得到的，忘记所付出的", "【古人有云】<br />博学、正直、诚信！" };

    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //内测判断 0,内测是否开启1，是否为内测账号

        if (ceshi == "1")//内测
        {
            //int meid = new BCW.User.Users().GetUsId();
            //if (meid == 0)
            //    Utils.Login();
            string[] sNum = Regex.Split(ceshiID, "#");
            int sbsy = 0;
            for (int a = 0; a < sNum.Length - 1; a++)
            {
                if (new BCW.User.Users().GetUsId() == int.Parse(sNum[a].Trim()))
                {
                    sbsy++;
                }
            }
            if (sbsy == 0)
            {
                Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
            }

        }

        //维护提示
        if (ub.GetSub("DawnlifeStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "xz":
                WinRulePage();
                break;
            case "md":
                RolePage();
                break;
            case "join":
                JoinPage();
                break;
            case "award":
                AwardPage();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "caseok1":
                CaseOk1Page();
                break;
            case "gift":
                GiftPage();
                break;
            case "trends":
                TrendsPage();
                break;
            case "give":
                GivePage();
                break;
            case "bk":
                BackStoryPage();
                break;
            case "rule":
                RulePage();
                break;
            case "notes":
                NotesPage();//闯荡记录
                break;
            case "cdjilu":
                cdjiluPage();
                break;
            case "cl":
                StrategyPage();
                break;
            case "jiang":
                JiangPage();
                break;
            case "top":
                TopPage();
                break;
            case "ss":
                SSPage();
                break;
            case "gz":
                GZPage();
                break;
            case "awardr":
                AwardrPage();
                break;
            case "pd":
                PDPage();
                break;
            case "end":
                ENDPage();
                break;
            case "bfend":
                BfendPage();
                break;
            case "buy":
                BuyPage();
                break;
            case "buyq":
                BuyqPage();
                break;
            case "buyqu":
                BuyquPage();
                break;
            case "sell":
                SellPage();
                break;
            case "sellq":
                SellqPage();
                break;
            case "selld":
                SelldPage();
                break;
            case "bfgz":
                bfgzPage();
                break;
            case "repay":
                ReapyPage();
                break;
            case "repayp":
                ReapyPPage();
                break;
            case "repaypd":
                ReapyPdPage();
                break;
            case "hospital":
                HospitalPage();
                break;
            case "hospitalp":
                HospitalPPage();
                break;
            case "hospitalpd":
                HospitalPpdage();
                break;
            case "donation":
                DonationlPage();
                break;
            case "donationp":
                DonationlpPage();
                break;
            case "donationpd":
                DonationlpdPage();
                break;
            case "upgrade":
                UpgradePage();
                break;
            case "sjck":
                SjkuPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //闯荡首页
    private void ReloadPage()
    {
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();首页可看
        string name = new BCW.BLL.User().GetUsName(meid);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/>");
        //builder.Append("<img src=\"" + " img/Dawnlife_img/audi.jpg" + "\" height=\"50\" weight=\"60\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //游戏头部Ubb
        string Head = ub.GetSub("DawnlifeHead", xmlPath);
        if (Head != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Head)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);


        string Notes = ub.GetSub("DawnlifeNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        long coin = new BCW.BLL.User().GetGold(meid);
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + meid + "") + "\">" + name + "(" + meid + ")</a>");
        //builder.Append("自带:"+"<a href=\"" + Utils.getUrl("../finance.aspx") + "\"> " + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + " </a>" + ub.Get("SiteBz"));
        //builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("精彩的闯荡马上开始！！！");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("[请选择你要闯荡的城市]");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=ss&amp;city=1") + "\">广州</a></b> | <b><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=ss&amp;city=2") + "\">北京</a></b> | <b><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=ss&amp;city=3") + "\">上海</a></b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=xz") + "\">获奖细则.</a>");
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=bk") + "\">故事背景.</a>");
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=rule") + "\">游戏规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=md") + "\">我的闯荡.</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=join") + "\">参与记录.</a>");
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=notes") + "\">闯荡记录.</a>");
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award") + "\">获奖酷友</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        int _time_num = Convert.ToInt32(_time);
        int r_1 = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift gift = new BCW.BLL.dawnlifegift().Getdawnlifegift(r_1 - 1);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今日奖池：" + gift.gift + "" + ub.Get("SiteBz") + "" + "<br />");
        if (_time_num > 0 && _time_num < 100) { builder.Append("捐款"); }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift") + "\">捐款</a>");
        }
        builder.Append("总池：");
        builder.Append("" + gift.giftj + "" + ub.Get("SiteBz") + "");


        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(DateTime.Now.AddDays(-1).ToString("【MM月dd日获奖酷友】") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        DataSet ds = new BCW.BLL.dawnlifeTop().GetList("Top 1 UsID,money", "DateDiff(day,date,getdate())=1 Order by money Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("全国闯王：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "(" + ds.Tables[0].Rows[0]["UsID"] + ")" + "</a>" + "<img src=\"" + Logo + "\"  alt=\"load\"/>" + "<br />");
        }
        else
            builder.Append("全国闯王：暂无记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=jiang") + "\">昨日获奖酷友</a>");


        builder.Append(" | ");
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top") + "\">富人排行榜</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("闯荡动态：");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int rm = new BCW.BLL.dawnlifeTop().GetMaxId();
        BCW.Model.dawnlifeTop model5 = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(rm - 1);
        try
        {
            for (int i = 1; i < 6; i++)
            {
                BCW.Model.dawnlifeTop model1 = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(rm - i);

                if (model1 == null)
                    continue;
                else
                {
                    if (model1.UsID == 0)
                        continue;
                    TimeSpan time = DateTime.Now - Convert.ToDateTime(model1.date);

                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;

                    string city = "";
                    if (model1.city == "1")
                    {
                        city = "广州";
                    }
                    if (model1.city == "2")
                    {
                        city = "北京";
                    }
                    if (model1.city == "3")
                    {
                        city = "上海";
                    }

                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                        }
                        else if (d == 0 && e == 0)
                            builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                        else if (d == 0)
                            builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                        else
                            builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                    }
                    else
                        builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                }
            }
        }
        catch
        {
            builder.Append("没有数据");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=trends&amp;backurl=" + Utils.PostPage(1) + "") + "\">>>更多动态</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("-----------", "<br />"));
        builder.Append(Out.Tab("</div>", ""));

        //闲聊显示 
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(29, "Dawnlife.aspx", 5, 0)));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        //第一个玩家更新奖池派奖
        DataSet today = new BCW.BLL.dawnlifegift().GetList("Top 1 *", " DateDiff(day,date,getdate())=0 ");
        if (today.Tables[0].Rows.Count <= 0)
        {
            string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
            int pageIndex = 0;//当前页
            int recordCount;//记录总条数
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
            string strWhere = "";
            string strOrder = "";
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "gift Desc";
            /////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("全国前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            IList<BCW.Model.dawnlifegift> listdawnlifeTop4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop4.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n4 in listdawnlifeTop4)
                {
                    if (l <= 1)
                    {
                        int giftq = Convert.ToInt32(n4.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1";
                        strOrder = "money Desc";
                        strOrder = "money Desc";
                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;

                        //if (_time_num == 0)
                        //{
                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coinq = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(giftq * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(giftq * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(giftq * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(giftq * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(giftq * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (n.sum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coinq, money);
                                        new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }
                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));
            ///////////////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("广州地区前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "gift Desc";
            IList<BCW.Model.dawnlifegift> listdawnlifeTop1 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop1.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop1)
                {
                    if (l <= 1)
                    {
                        int giftg = Convert.ToInt32(n1.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1 and city ='1' ";
                        strOrder = "money Desc";

                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;

                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop11.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coing = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(giftg * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(giftg * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(giftg * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(giftg * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(giftg * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coing, money);
                                        new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广州榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }

                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));

            ///////////////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("北京地区前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "gift Desc";
            IList<BCW.Model.dawnlifegift> listdawnlifeTop2 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop2.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop2)
                {
                    if (l <= 1)
                    {
                        int giftb = Convert.ToInt32(n1.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1 and city ='2' ";
                        strOrder = "money Desc";

                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;
                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop11.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coinb = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(giftb * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(giftb * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(giftb * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(giftb * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(giftb * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coinb, money);
                                        new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "北京榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }

                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));

            ///////////////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上海地区前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "gift Desc";
            IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop3.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop3)
                {
                    if (l <= 1)
                    {
                        int gifts = Convert.ToInt32(n1.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1 and city ='3' ";
                        strOrder = "money Desc";

                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;

                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop11.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coins = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gifts * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gifts * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gifts * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gifts * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gifts * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coins, money);
                                        new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "上海榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }

                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));


            ////////////////////////////////


            BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
            int rp = new BCW.BLL.dawnlifegift().GetMaxId();
            BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(rp - 1);
            int gift2 = Convert.ToInt32(re.gift * jcx / 100) + Convert.ToInt32(re.giftj * juankuan / 100);
            string _time3 = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            int _time_num3 = Convert.ToInt32(_time3);

            give.date = DateTime.Now;
            give.gift = gift2;
            give.giftj = Convert.ToInt32(re.giftj * (1 - (juankuan / 100)));
            give.UsID = 1;
            give.UsName = Convert.ToInt32(re.giftj * juankuan / 100).ToString();
            give.coin = Convert.ToInt32(re.gift * jcd / 100);
            give.state = 3;
            new BCW.BLL.dawnlifegift().Add(give);

        }


        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //动态
    private void TrendsPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;动态");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append("<br />");
        builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = "";
        strOrder = "date desc";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop1 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop model1 in listdawnlifeTop1)
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
                int d2 = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string city = model1.city;
                if (model1.city == "1")
                {
                    city = "广州";
                }
                if (model1.city == "2")
                {
                    city = "北京";
                }
                if (model1.city == "3")
                {
                    city = "上海";
                }

                TimeSpan time = DateTime.Now - Convert.ToDateTime(model1.date);

                int d1 = time.Days;
                int d = time.Hours;
                int e = time.Minutes;
                int f = time.Seconds;

                if (d1 == 0)
                {
                    if (d == 0 && e == 0 && f == 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                    }
                    else if (d == 0 && e == 0)
                        builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                    else if (d == 0)
                        builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                    else
                        builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");
                }
                else
                    builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + model1.UsName + "</a>" + "在" + city + "挣了" + model1.money + "银两" + "<br />");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">>>>返回" + ub.GetSub("DawnlifeName", xmlPath) + "&lt;&lt;&lt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //获奖规则说明
    private void WinRulePage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;获奖细则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【" + ub.GetSub("DawnlifeName", xmlPath) + "奖池】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + "奖池是游戏厅增设的,志在奖励靠自己努力,荣登闯荡富人排行榜的用户.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【闯荡奖池,是酷友们在一天参与闯荡所支付的" + ub.Get("SiteBz") + "总和.】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("游戏厅将以闯荡的富人排行榜的日榜排名做出奖励.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("其中奖池的" + jcd + "%用于当日奖励，" + jcx + "%留到第二日放进奖池，作为第二日奖池的底.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("其中当日奖励的" + drq + "%奖励全国榜的前五名，余下的" + drd + "%奖励三个地区的前五名.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("全国榜前五名,将依据排名获得数额不同的奖励.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("其中第一名奖励" + q1 + "%，第二名奖励" + q2 + "%，第三名奖励" + q3 + "%，第四名奖励" + q4 + "%，第五名奖励" + q5 + "%.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("三个地区的前五名,将平分奖励" + ub.Get("SiteBz") + ".");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("特别提示:" + GameName + "中的名声值对全国榜前五名获得的奖励起到重要作用.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【注意事项】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1,获奖资格以富人排行榜的日榜为准.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("2,当天获奖的酷友们,需隔天后才能领取到奖励.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("3,获得奖励的" + ub.Get("SiteBz") + ",需酷友们到闯荡首页兑奖页领取.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("4,每次游戏需支付" + pay + "" + ub.Get("SiteBz") + ",该笔" + ub.Get("SiteBz") + "计入奖池,用于奖励获胜者.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("5,酷友们可以累计获得奖励.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("6,闯荡获奖友友,请及时领取获胜" + ub.Get("SiteBz") + ",游戏厅只保留两天的领奖记录.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("7,酷爆游戏厅保留最终解释权");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">>>>返回闯荡&lt;&lt;&lt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //我的闯荡
    private void RolePage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;我的闯荡");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int iSCounts = 0;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【我的闯荡史】");
        builder.Append(Out.Tab("</div>", "<br />"));
        //查询条件
        strWhere = "UsID=" + meid + "";
        strOrder = "date Desc";
        //iSCounts = 50;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops1(pageIndex, pageSize, strWhere, strOrder, iSCounts, out recordCount);
        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string city = n.city;
                if (n.city == "1")
                {
                    city = "广州";
                }
                if (n.city == "2")
                {
                    city = "北京";
                }
                if (n.city == "3")
                {
                    city = "上海";
                }
                sText = "." + "在" + city + "赚得" + n.money + "银两|标识ID " + n.coin + "|(" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=md&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", ""));

        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //参与记录
    private void JoinPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;参与记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【我的消费记录】");
        builder.Append(Out.Tab("</div>", "<br />"));
        //查询条件
        strWhere = "UsID=" + meid + "";
        strOrder = "coin Desc";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("游戏消费：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop1 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop n1 in listdawnlifeTop1)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string city = n1.city;
                if (n1.city == "1")
                {
                    city = "广州";
                }
                if (n1.city == "2")
                {
                    city = "北京";
                }
                if (n1.city == "3")
                {
                    city = "上海";
                }
                sText = "." + "您于" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + "在" + city + "花费" + pay + "" + ub.Get("SiteBz") + "";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=join&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //闯荡记录
    private void NotesPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;闯荡记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【我的闯荡记录】");
        builder.Append(Out.Tab("</div>", "<br />"));
        //查询条件
        strWhere = "UsID=" + meid + "";
        strOrder = "coin Desc";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("闯荡记录：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop1 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop n1 in listdawnlifeTop1)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string city = n1.city;
                if (n1.city == "1")
                {
                    city = "广州";
                }
                if (n1.city == "2")
                {
                    city = "北京";
                }
                if (n1.city == "3")
                {
                    city = "上海";
                }  //Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") 
                sText = "." + "编号：" + n1.coin + "在" + city + "赚得" + n1.money + "银两 " + "<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=cdjilu&amp;id=" + n1.coin + "") + "\">查看</a>";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=join&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //闯荡详细记录
    private void cdjiluPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=notes") + "\">闯荡记录</a>&gt;详细记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件
        strWhere = "UsID=" + meid + " and coin =" + id + " ";
        strOrder = " day , date , money ";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("闯荡编号：" + id + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        IList<BCW.Model.dawnlifenotes> listdawnlifeTop1 = new BCW.BLL.dawnlifenotes().Getdawnlifenotess2(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifenotes n1 in listdawnlifeTop1)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string city = Convert.ToString(n1.city);
                //string area = string.Empty;
                string caozuo = string.Empty;
                if (n1.buy == "buy" && n1.sell == "sell") { caozuo = "无买卖"; }
                if (n1.buy != "buy" && n1.sell == "sell") { caozuo = "购买." + n1.buy; }
                if (n1.buy == "如来神掌" && n1.sell == "sellr") { caozuo = "赠送." + n1.buy; }
                if (n1.buy == "走私汽车" && n1.sell == "sellz") { caozuo = "赠送." + n1.buy; }
                if (n1.buy == "buy" && n1.sell != "sell") { caozuo = "出售." + n1.sell; }
                if (n1.buy == "buy0.1" && n1.sell == "sell") { caozuo = "银两减少10%"; }
                if (n1.buy == "buy0.05" && n1.sell == "sell") { caozuo = "银两减少5%"; }
                if (n1.buy == "buy" && n1.sell == "sell0.05") { caozuo = "欠款增加5%"; }
                if (n1.buy == "buy" && n1.sell == "sell9968") { caozuo = "欠款增加9968"; }
                if (n1.buy == "buyq" && n1.sell == "sell") { caozuo = "还钱"; }
                if (n1.buy == "sell") { caozuo = "出售" + n1.sell; }
                if (n1.buy == "yiyuanq" && n1.sell == "yiyuanq") { caozuo = "恢复健康"; }
                if (n1.buy == "yiyuan" && n1.sell == "yiyuan") { caozuo = "恢复健康"; }
                if (n1.buy == "mingshengq" && n1.sell == "mingshengq") { caozuo = "恢复名声"; }
                if (n1.buy == "mingsheng" && n1.sell == "mingsheng") { caozuo = "恢复名声"; }
                if (n1.buy == "cksj" && n1.sell == "cksj") { caozuo = "仓库升级"; }

                BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(1);
                BCW.Model.dawnlifedaoju rb = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(2);
                BCW.Model.dawnlifedaoju rs = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(3);
                //string[] area = rg.area.Split('/');
                if (n1.city == 1)
                {
                    city = "广州";
                    string[] area = rg.area.Split('/');
                    if (n1.day == 0) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + ".数量" + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else if (n1.day == 31) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else
                    {
                        sText = "." + "[第" + n1.day + "天." + city + "." + area[n1.area - 1] + "][" + caozuo + ".数量" + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                }
                if (n1.city == 2)
                {
                    city = "北京";
                    string[] area = rb.area.Split('/');
                    if (n1.day == 0) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + ".数量" + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else if (n1.day == 31) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }

                    else
                    {
                        sText = "." + "[第" + n1.day + "天." + city + "." + area[n1.area - 1] + "][" + caozuo + ".数量" + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                }
                if (n1.city == 3)
                {
                    city = "上海";
                    string[] area = rs.area.Split('/');
                    if (n1.day == 0) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + ".数量" + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else if (n1.day == 31) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }

                    else
                    {
                        sText = "." + "[第" + n1.day + "天." + city + "." + area[n1.area - 1] + "][" + caozuo + ".数量" + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                }
                //sText = "." + "[第" + n1.day + "天." + city + "." +area [ n1.area ]+ "][" + caozuo + ".数量." + n1.num + ".价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss")+")";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=join&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("特别说明：<br/>购买减少的银两=数量×价钱<br />卖出增加的银两=数量×价钱<br />还钱、银两减少百分之几银两直接减少<br />恢复健康/恢复名声减少银两=价钱，数量为增加的值<br />仓库升级减少银两=价钱，数量为升级后的仓库库存大小");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=notes") + "\">返回闯荡记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //捐款
    private void GiftPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        long coin = new BCW.BLL.User().GetGold(meid);
        string name = new BCW.BLL.User().GetUsName(meid);
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;捐款");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + meid + "") + "\">" + name + "(" + meid + ")</a>");
        builder.Append("自带:" + "<a href=\"" + Utils.getUrl("../finance.aspx") + "\"> " + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + " </a>" + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", "<br />"));

        int r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift gift = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今日奖池：" + gift.gift + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">捐款总池：</b>");
        builder.Append("" + gift.giftj + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "3"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "2"));
        string strText = "输入捐款" + ub.Get("SiteBz") + ":/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确认,Dawnlife.aspx?act=give&amp;ptype=" + ptype + "&amp;ptyped=" + ptyped + "&amp;id1=" + ptype + "&amp;id=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=0") + "\">再考虑一下&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        recordCount = 100;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        builder.Append("特别提醒：每日凌晨一分钟时间为奖池更新，请酷友们勿进行捐款" + "<br />");
        builder.Append("特别说明：今日奖池" + jcd + "%用于当日获奖奖励，" + jcx + "%用于下一日奖池，捐款总池的" + juankuan + "%存入下一日奖池" + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + "捐款记录：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));

        if (ptyped == 1)
            builder.Append("<b style=\"color:black\">今日" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptyped=1&amp;id=" + ptyped + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">今日</a>" + "|");
        if (ptyped == 2)
            builder.Append("<b style=\"color:black\">历史" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptyped=2&amp;id=" + ptyped + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">历史</a>" + "");
        builder.Append("<br />");
        builder.Append(Out.Tab("</div>", "")); builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">我的捐款记录" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptype=1&amp;id1=" + ptype + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我的捐款记录</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">捐款记录" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptype=2&amp;id1=" + ptype + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">捐款记录</a>" + "|");
        if (ptype == 3)
            builder.Append("<b style=\"color:black\">捐款排行" + "</b>" + "<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptype=3&amp;id1=" + ptype + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">捐款排行</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        if (ptyped == 1 && ptype == 1)
        {
            strWhere = "state=1 and DateDiff(dd,date,getdate())=0 and UsID=" + meid;
            strOrder = "date Desc";
        }
        if (ptyped == 1 && ptype == 2)
        {
            strWhere = "state=1 and DateDiff(dd,date,getdate())=0";
            strOrder = "date Desc";
        }
        if (ptyped == 1 && ptype == 3)
        {
            strWhere = "state=1 and DateDiff(dd,date,getdate())=0";
            strOrder = "coin Desc , date Desc ";
        }
        if (ptyped == 2 && ptype == 1)
        {
            strWhere = "state=1 and UsID=" + meid;
            strOrder = "date Desc";
        }
        if (ptyped == 2 && ptype == 2)
        {
            strWhere = "state=1 ";
            strOrder = "date Desc";
        }
        if (ptyped == 2 && ptype == 3)
        {
            strWhere = "state=1 ";
            strOrder = "coin Desc , date Desc ";
        }
        string[] pageValUrl = { "act", "ptype", "ptyped", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.dawnlifegift> listdawnlifeTop = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;

            foreach (BCW.Model.dawnlifegift n in listdawnlifeTop)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;

                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a> " + "于" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + "捐助了" + n.coin + "" + ub.Get("SiteBz") + "";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptype=" + ptype + "&amp;ptyped=" + ptyped + "&amp;id1=" + ptype + "&amp;id=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    //确认捐款
    private void GivePage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "3"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "2"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        long Gold = new BCW.BLL.User().GetGold(meid);
        BCW.Model.User su = new BCW.Model.User();
        BCW.Model.dawnlifegift addj = new BCW.Model.dawnlifegift();
        int r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift rr = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);

        //支付安全提示
        string[] p_pageArr = { "act", "ptyped", "uid", "id", "ptype" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");
        if (Gold > 0)
        {
            if (uid <= Gold)
            {
                if (uid > 0)
                {
                    addj.date = DateTime.Now;
                    addj.gift = rr.gift;
                    addj.giftj = rr.giftj + uid;
                    addj.UsID = meid;
                    addj.UsName = name;
                    addj.coin = uid;
                    addj.state = 1;
                    su.iGold = su.iGold - uid;
                    new BCW.BLL.User().UpdateiGold(meid, su.iGold, "" + GameName + "捐款消费");
                    //活跃抽奖入口_20160621姚志光
                    try
                    {
                        //表中存在记录
                        if (new BCW.BLL.tb_WinnersGame().ExistsGameName(GameName))
                        {
                            //是否大于设定的限额，是则有抽奖机会
                            if (uid > new BCW.BLL.tb_WinnersGame().GetPrice(GameName))
                            {
                                string mename = new BCW.BLL.User().GetUsName(meid);
                                string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                                int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, GameName + "捐款", 3);
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
                    new BCW.BLL.dawnlifegift().Add(addj);

                    int id = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
                    string wText = "在[url=/bbs/game/Dawnlife.aspx]" + GameName + "[/url]捐款**" + ub.Get("SiteBz") + "，荣光满面";// + uid 
                    new BCW.BLL.Action().Add(1008, id, meid, name, wText);

                    Utils.Success("捐款成功", "只要人人都付出一点爱，世界就能越来越美好", Utils.getUrl("Dawnlife.aspx?act=gift&amp;ptype=" + ptype + "&amp;ptyped=" + ptyped + "&amp;id1=" + ptype + "&amp;id=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + ""), "2");
                }
                else { Utils.Success("捐款失败", "请输入大于0的正整数", Utils.getUrl("Dawnlife.aspx?act=gift"), "2"); }
            }
            else
            {
                Utils.Success("捐款失败", "真是大好人啊，您输入的" + ub.Get("SiteBz") + "数大于你自己的" + ub.Get("SiteBz") + "数，不能透支哦", Utils.getUrl("Dawnlife.aspx?act=gift"), "2");
            }
        }
        else
        {
            Utils.Success("捐款失败", "真是大好人啊，但是您的" + ub.Get("SiteBz") + "不足以捐款，快去充值吧", Utils.getUrl("Dawnlife.aspx?act=gift"), "2");
        }
    }
    //游戏领奖
    private void AwardPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;获奖酷友");
        builder.Append(Out.Tab("</div>", "<br />"));
        //兑奖页面

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>近五日获奖名单：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
        builder.Append("" + day + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=4&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=1&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");
        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=2&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=3&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTop3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1 and city='1'";
                    }
                    if (ptypet == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1 and city='2'";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1 and city='3'";
                    }
                    else if (ptypet == 4)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1";
                    }
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt32(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;

                                if (ptypet == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (meid == n.UsID && n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                else
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (meid == n.UsID && n.cum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        string day2 = DateTime.Now.AddDays(-2).ToString("yyyy年MM月dd日");
        builder.Append("" + day2 + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=4&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=1&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");
        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=2&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=3&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        strWhere = " DateDiff(day,date,getdate())=2";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop4.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n4 in listdawnlifeTop4)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n4.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=2 and city='1'";
                        strOrder = "money Desc";
                    }
                    if (ptypet == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=2 and city='2'";
                        strOrder = "money Desc";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=2 and city='3'";
                        strOrder = "money Desc";
                    }
                    else if (ptypet == 4)
                        strWhere = "DateDiff(day,date,getdate())=2";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop2 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop2.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n2 in listdawnlifeTop2)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt32(n2.coin);
                                int money = Convert.ToInt32(n2.money);
                                string sText = string.Empty;

                                if (ptyped == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (meid == n2.UsID && n2.sum == 0 && n2.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "赚币" + n2.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "赚币" + n2.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                else if (ptyped == 1 || ptyped == 2 || ptyped == 3)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (meid == n2.UsID && n2.cum == 0 && n2.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "赚币" + n2.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "赚币" + n2.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        day = DateTime.Now.AddDays(-3).ToString("yyyy年MM月dd日");
        builder.Append("" + day + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=4&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=1&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");
        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=2&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=3&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=3";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTopd3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTopd3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift nd3 in listdawnlifeTopd3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(nd3.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=3 and city='1'";
                    }
                    if (ptypet == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=3 and city='2'";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=3 and city='3'";
                    }
                    else if (ptypet == 4)
                    {
                        strWhere = "DateDiff(day,date,getdate())=3";
                    }
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt32(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;

                                if (ptypet == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (meid == n.UsID && n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                else
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (meid == n.UsID && n.cum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        day = DateTime.Now.AddDays(-4).ToString("yyyy年MM月dd日");
        builder.Append("" + day + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=4&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=1&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");
        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=2&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=3&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=4";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTopd4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTopd4.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTopd4)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=4 and city='1'";
                    }
                    if (ptypet == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=4 and city='2'";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=4 and city='3'";
                    }
                    else if (ptypet == 4)
                    {
                        strWhere = "DateDiff(day,date,getdate())=4";
                    }
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt32(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;

                                if (ptypet == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (meid == n.UsID && n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                else
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (meid == n.UsID && n.cum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        day = DateTime.Now.AddDays(-5).ToString("yyyy年MM月dd日");
        builder.Append("" + day + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=4&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=1&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");
        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=2&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=3&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=5";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTopd5 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTopd5.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTopd5)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=5 and city='1'";
                    }
                    if (ptypet == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=5 and city='2'";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=5 and city='3'";
                    }
                    else if (ptypet == 4)
                    {
                        strWhere = "DateDiff(day,date,getdate())=5";
                    }
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt32(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;

                                if (ptypet == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (meid == n.UsID && n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                else
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (meid == n.UsID && n.cum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚币" + n.money + "银两，" + "奖励" + award + "" + ub.Get("SiteBz") + "" + "";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("特别说明：具体奖励规则请查看获奖细则" + "<br />" + "" + "<br />" + "<b style=\"color:red\"> 【特别申明】<br />本游戏为自动派奖，玩家在游戏领奖页面可查看获奖情况，派奖操作系统能自动完成，请关注系统消息</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));



        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //自动派奖与奖池更新面页
    private void AwardrPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + GameName + "管理</a>&gt;自动派奖与奖池更新管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        //兑奖页面
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：本面页用于奖池自动更新与自动派奖，请管理员请勿随意操作");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<br />", Out.Hr()));
        builder.Append(Out.Tab("</div>", ""));
        string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        /////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("全国前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        IList<BCW.Model.dawnlifegift> listdawnlifeTop4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop4.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n4 in listdawnlifeTop4)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n4.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1";
                    strOrder = "money Desc";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                if (n.sum == 0 && n.money > 0)
                                {
                                    int usid = Convert.ToInt32(n.UsID);
                                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                    int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                    new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                                    new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + n.coin + "");
                                    //发内线
                                    string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            { ;  }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));
        ///////////////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("广州地区前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop1 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop1)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n1.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1 and city ='1' ";
                    strOrder = "money Desc";

                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop11.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (n.cum == 0 && n.money > 0)
                                {
                                    int usid = Convert.ToInt32(n.UsID);
                                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                    int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                    new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广州榜单兑奖|标识ID" + n.coin + "");
                                    //发内线
                                    string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            { ;  }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        ///////////////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("北京地区前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop2 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop2.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop2)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n1.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1 and city ='2' ";
                    strOrder = "money Desc";

                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop11.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (n.cum == 0 && n.money > 0)
                                {
                                    int usid = Convert.ToInt32(n.UsID);
                                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                    int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                    new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "北京榜单兑奖|标识ID" + n.coin + "");
                                    //发内线
                                    string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            { ;  }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        ///////////////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上海地区前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n1.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1 and city ='3' ";
                    strOrder = "money Desc";

                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop11.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        int h = 1;
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (n.cum == 0 && n.money > 0)
                                {
                                    int usid = Convert.ToInt32(n.UsID);
                                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                    int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                    new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "上海榜单兑奖|标识ID" + n.coin + "");
                                    //发内线
                                    string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "派奖";
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "" + ub.Get("SiteBz") + ":" + "已派奖";
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            { ;  }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));


        ////////////////////////////////


        BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
        int r_1 = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(r_1 - 1);
        int gift2 = Convert.ToInt32(re.gift * jcx / 100) + Convert.ToInt32(re.giftj * juankuan / 100);
        string _time3 = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        int _time_num3 = Convert.ToInt32(_time3);
        //if (_time_num3 == 0)
        //{
        give.date = DateTime.Now;
        give.gift = gift2;
        give.giftj = Convert.ToInt32(re.giftj * (1 - (juankuan / 100)));
        give.UsID = 1;
        give.UsName = Convert.ToInt32(re.giftj * juankuan / 100).ToString();
        give.coin = Convert.ToInt32(re.gift * jcd / 100);
        give.state = 3;
        new BCW.BLL.dawnlifegift().Add(give);
        //}
    }
    //上一日每单领奖
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择领奖无效"));
        int coin = Utils.ParseInt(Utils.GetRequest("coin", "get", 2, @"^[0-9]\d*$", ""));
        int money = Utils.ParseInt(Utils.GetRequest("money", "get", 2, @"^[0-9]\d*$", ""));
        int award = Utils.ParseInt(Utils.GetRequest("award", "get", 2, @"^[0-9]\d*$", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(pid, coin, money);
        BCW.Model.dawnlifeTop re = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(r);
        if (ptyped != 0)
        {
            if (ptypet == 4)
            {
                if (re.sum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.sum = 1;
                    new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                    new BCW.BLL.User().UpdateiGold(meid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    string strLog = "根据你上期" + GameName + "排行榜上的赢利情况，系统自动返还了" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);
                    Utils.Success("领奖", "恭喜，成功领奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "2");
                }
                else if (re.sum == 1)
                {
                    Utils.Success("领奖", "恭喜，重复领奖或没有可以领奖的记录", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "2");
                }
            }
            else if (ptypet == 1 || ptypet == 3 || ptypet == 2)
            {
                if (re.cum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.cum = 1;
                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                    new BCW.BLL.User().UpdateiGold(meid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "地区榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    string strLog = "根据你上期" + GameName + "排行榜上的赢利情况，系统自动返还了" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);
                    Utils.Success("兑奖", "恭喜，成功领奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "2");
                }
                else if (re.cum == 1)
                {
                    Utils.Success("兑奖", "恭喜，重复领奖或没有可以领奖的记录", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "2");
                }
            }
        }
        else
        {
            if (ptypet == 4)
            {
                if (re.sum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.sum = 1;
                    new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                    new BCW.BLL.User().UpdateiGold(meid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    string strLog = "根据你上期" + GameName + "排行榜上的赢利情况，系统自动返还了" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);
                    Utils.Success("领奖", "恭喜，成功领奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + ""), "2");
                }
                else if (re.sum == 1)
                {
                    Utils.Success("领奖", "恭喜，重复领奖或没有可以领奖的记录", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + ""), "2");
                }
            }
            else if (ptypet == 1 || ptypet == 3 || ptypet == 2)
            {
                if (re.cum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.cum = 1;
                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                    new BCW.BLL.User().UpdateiGold(meid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "地区榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    string strLog = "根据你上期" + GameName + "排行榜上的赢利情况，系统自动返还了" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);
                    Utils.Success("兑奖", "恭喜，成功领奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + ""), "2");
                }
                else if (re.cum == 1)
                {
                    Utils.Success("兑奖", "恭喜，重复领奖或没有可以领奖的记录", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptypet=" + ptypet + ""), "2");
                }
            }
        }
    }
    //每单领奖前日
    private void CaseOk1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择领奖无效"));
        int coin = Utils.ParseInt(Utils.GetRequest("coin", "get", 2, @"^[0-9]\d*$", ""));
        int money = Utils.ParseInt(Utils.GetRequest("money", "get", 2, @"^[0-9]\d*$", ""));
        int award = Utils.ParseInt(Utils.GetRequest("award", "get", 2, @"^[0-9]\d*$", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int r = new BCW.BLL.dawnlifeTop().GetRowByUsID(pid, coin, money);
        BCW.Model.dawnlifeTop re = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(r);
        if (ptyped == 4)
        {
            if (re.sum == 0)
            {
                BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                update.sum = 1;
                new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                new BCW.BLL.User().UpdateiGold(meid, award, "闯荡" + DateTime.Now.AddDays(-2).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + re.coin + "");
                //发内线
                string strLog = "根据你上期" + GameName + "排行榜上的赢利情况，系统自动返还了" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);
                Utils.Success("领奖", "恭喜，成功领奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "2");
            }
            else if (re.sum == 1)
            {
                Utils.Success("领奖", "恭喜，重复领奖或没有可以领奖的记录", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "2");
            }
        }
        else if (ptyped == 1 || ptyped == 2 || ptyped == 3)
        {
            if (re.cum == 0)
            {
                BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                update.cum = 1;
                new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                new BCW.BLL.User().UpdateiGold(meid, award, "闯荡" + DateTime.Now.AddDays(-2).ToString("MM月dd日") + "地区榜单兑奖|标识ID" + re.coin + "");
                //发内线
                string strLog = "根据你上期" + GameName + "排行榜上的赢利情况，系统自动返还了" + award + "" + ub.Get("SiteBz") + "" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, meid, new BCW.BLL.User().GetUsName(meid), strLog);
                Utils.Success("兑奖", "恭喜，成功领奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "2");
            }
            else if (re.cum == 1)
            {
                Utils.Success("兑奖", "恭喜，重复领奖或没有可以领奖的记录", Utils.getUrl("Dawnlife.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "2");
            }
        }
    }
    //故事背景
    private void BackStoryPage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;故事背景");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        builder.Append("【闯荡的故事背景】" + "<br />" + "你扮演一位从外地来到大城市谋生的青年," + "<br />" + "口袋里仅怀揣着2000银两,背着行囊来到了异乡,开始了你的闯荡生涯." + "<br />" + "来大城市前,你还欠着'胖大海'5000银两的高利贷,利息10%一天." + "<br />" + "创业初期你一定要尽快还清你的债务,否则你还有可能遭遇到'胖大海'这个流氓头子的恶劣暴行哦!" + "<br />" + "你有一个可以置放100件货物的小仓库---破旧阁楼(可以升级)," + "<br />" + "你可以任意挑选一个地方开始买卖货物,每到一处便是一天,总共一个月." + "<br />" + "当然,闯荡中你一定会遇到各种未知的突发情况:恶劣天气,流氓地痞,贪官污吏,金融危机......" + "<br />" + "依靠你的智慧和运气,说不定你还有可能荣登富人排行榜噢." + "<br />" + "相信年轻奋进的你一定能在这些海纳百川的城市中闯出自己的一片天空.");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">开始闯荡>></a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=cl") + "\">闯荡策略</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=rule") + "\">闯荡规则</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回闯荡</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    //游戏规则
    private void RulePage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=" + ub.GetSub("DawnlifeName", xmlPath) + "=" + "<br />" + "" + GameName + "是一款益智游戏,玩家通过买卖商品从中获利,从打工者一步步的转变为富人." + "<br />" + "为增加游戏难度,游戏初始玩家处于欠债状态,如何还清债务并荣登富人榜,就看各位玩家的实力和运气了." + "<br />" + "测试期间,每次游戏将收取" + pay + "" + ub.Get("SiteBz") + "." + "<br />" + "=玩法介绍=" + "<br />" + "游戏初始,玩家可以从上海、北京、广州中任选一城市,作为闯荡的开始(各个城市不相通,游戏未结束前,无法更换城市)." + "<br />" + "你只能在某个城市闯荡一个月,游戏开始为一天,每次奔走到另一个地方算一天." + "<br />" + "每一天都有可能会发生一些事件,事件的好坏就看玩家的运气了." + "<br />" + "游戏开始时,你的现金只有2000银两,而且还欠了5000银两,每过一天欠款都会增加." + "<br />" + "同时你的健康、名声和仓库都将会影响游戏的最终结果,所以一定要随时注意.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">开始闯荡>></a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=cl") + "\">闯荡策略</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=bk") + "\">故事背景</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回闯荡</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //闯荡策略
    private void StrategyPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;策略");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=策略指南=" + "<br />" + "[循序渐进 敢冒风险] ");
        builder.Append("<br />" + "一开始倒卖便宜的东西(如盗版DVD)进行原始积累,有一定经济基础后,倒卖贵东西(如走私汽车),该出手时就出手.");
        builder.Append("<br />" + "虽然有较大的风险,但是赢利空间也相应增加." + "<br />");
        builder.Append("[多种经营 降低风险]" + "<br />" + "风险时时存在.经济条件允许时,可以同时进多种货物,转化风险.");
        builder.Append("<br />" + "[勿贪勿执 勇士断臂] ");
        builder.Append("<br />" + "贪婪和执著是发财之大忌.因此,发现有价值低价商品(如汽车只要15,000银两)时,而您的仓库又充斥着无法赢利的物品,宁愿亏本也要低价把物品抛出,及时进有价值低价商品.");
        builder.Append("<br />" + "[保重身体 呵护健康]" + "<br />");
        builder.Append(" 您的健康很重要,不要牺牲在街头,健康程度下降时,要及时治疗." + "<br />");
        builder.Append("[注意名声 名利双收] " + "<br /> ");
        builder.Append("倒卖假酒和禁书【欲女心经】能够让您迅速赚钱,但是对您的名声打击很大.");
        builder.Append("<br />" + "金钱再多,名声欠佳也非成功商人,所以尽量不要买卖假酒之类商品.");
        builder.Append("<br />" + "[扩大规模 加大容量]" + "<br />");
        builder.Append(" 具体就是在经济条件允许的情况下,去租赁中介公司租用更大的仓库倒卖东西.但是,中介公司往往会骗您一些钱.");
        builder.Append("<br />" + "[不畏艰难 勤恳钻研] " + "<br />" + "游戏的特点是:入门容易 提高难.挣10万银两轻松,挣3000万银两难.");
        builder.Append("<br />" + "初学者往往难以把握赚钱策略,赚钱很少,但是不要放弃,再玩一次,您就可能赢利." + "<br />");
        builder.Append("常玩,多琢磨,一定会有更多心得,您肯定会成为赚2000万以上的高手.");
        builder.Append("<br />" + "目前已经有高手赚了1亿8百万.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">开始闯荡>></a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=rule") + "\">闯荡规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=bk") + "\">故事背景</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回闯荡</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //开始选择界面
    private void SSPage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;故事");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("一次游戏" + pay + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //用户的"+ub.Get("SiteBz")+"信息检索 //用户玩游戏获取用户信息
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        long coin = new BCW.BLL.User().GetGold(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("随身" + ub.Get("SiteBz") + "：" + "<a href=\"" + Utils.getUrl("../finance.aspx") + "\"> " + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + " </a>" + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int ptype = int.Parse(Utils.GetRequest("area", "get", 1, @"^[1-9]$", "1"));
        BCW.Model.dawnlifeDays addday = new BCW.Model.dawnlifeDays();
        addday.UsID = meid;
        addday.UsName = "name";
        addday.price = "0";
        addday.news = "news";
        addday.n = 6;
        addday.goods = "goods";
        addday.area = "6";
        addday.city = ptypecity.ToString();
        addday.coin = 0;
        addday.day = 33;
        new BCW.BLL.dawnlifeDays().Add(addday);
        if (coin >= pay)
        {
            int r = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
            BCW.Model.dawnlifeDays rd = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r);
            //if (rd.day == 33)
            //{
            //    builder.Append(Out.Tab("<div>", ""));
            //    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=bfgz&amp;city=" + ptypecity + "") + "\">[1开始闯荡]</a>");
            //    builder.Append(Out.Tab("</div>", "<br />"));
            //}
            //else
            //{
            if (rd.day < 31)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + rd.city + "&amp;area=" + rd.area + "") + "\">[继续闯荡]</a>" + "<br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=bfgz&amp;city=" + ptypecity + "") + "\">[开始闯荡]</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            //}
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您的" + ub.Get("SiteBz") + "不足以进入游戏！！！每局游戏至少花费" + pay + "" + ub.Get("SiteBz") + "！！");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">[再考虑一下]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("你扮演一位从外地来到大城市谋生的青年,口袋里仅怀揣着2000银两," + "<br />" + "背着行囊来到了异乡,开始了你的闯荡生涯." + "<br />" + "来大城市前,你还欠着'胖大海'5000银两的高利贷,利息10%一天." + "<br />" + "创业初期你一定要尽快还清你的债务,否则你还有可能遭遇到'胖大海'这个流氓头子的恶劣暴行哦!" + "<br />" + "你有一个可以置放100件货物的小仓库---破旧阁楼(可以升级)," + "<br />" + "你可以任意挑选一个地方开始买卖货物,每到一处便是一天,总共一个月." + "<br />" + "当然,闯荡中你一定会遇到各种未知的突发情况:恶劣天气,流氓地痞,贪官污吏,金融危机......" + "<br />" + "依靠你的智慧和运气,说不定你还有可能荣登富人排行榜噢." + "<br />" + "相信年轻奋进的你一定能在这些海纳百川的城市中闯出自己的一片天空.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=rule") + "\">还不会玩？看游戏规则>></a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //开始判断进入游戏
    private void bfgzPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        //////进入游戏"+ub.Get("SiteBz")+"减100 
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        string name = new BCW.BLL.User().GetUsName(meid);
        int r2 = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rd = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r2);
        if (rd.day != 0)
        {
            ////用户玩游戏获取用户信息
            string usname = new BCW.BLL.User().GetUsName(meid);
            int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            int ru = new BCW.BLL.dawnlifeUser().GetMaxId();
            BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(ru - 1);
            BCW.Model.dawnlifeUser addcs = new BCW.Model.dawnlifeUser();
            addcs.UsID = meid;
            addcs.coin = re.coin + 1;
            addcs.debt = 5000;
            addcs.money = 2000;
            addcs.health = 100;
            addcs.stock = " 100";
            addcs.storehouse = "破旧阁楼";
            addcs.reputation = 100;
            addcs.UsName = usname;
            addcs.city = ptypecity.ToString();
            new BCW.BLL.dawnlifeUser().Add(addcs);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin + 1);
            notes.UsID = meid;
            notes.money = 2000;
            notes.debt = 5000;
            notes.city = ptypecity;
            notes.area = 0;
            notes.day = 0;
            notes.buy = "buy";
            notes.sell = "sell";
            notes.price = 0;
            notes.num = 0;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);

            BCW.Model.User su = new BCW.Model.User();
            su.iGold = su.iGold - pay;
            new BCW.BLL.User().UpdateiGold(meid, su.iGold, "" + GameName + "入场消费|标识ID" + (re.coin + 1) + "");
            //活跃抽奖入口_20160621姚志光
            try
            {
                //表中存在虚拟球类记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName(GameName))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (99 < new BCW.BLL.tb_WinnersGame().GetPrice(GameName))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, usname, GameName, 3);
                        if (hit == 1)
                        {
                            //内线开关 1开
                            if (WinnersGuessOpen == "1")
                            {
                                //发内线到该ID
                                new BCW.BLL.Guest().Add(0, meid, usname, TextForUbb);
                            }
                        }
                    }
                }
            }
            catch { }
            BCW.Model.dawnlifeDays addday = new BCW.Model.dawnlifeDays();
            addday.UsID = meid;
            addday.UsName = name;
            addday.price = "0";
            addday.news = "news";
            addday.n = 6;
            addday.goods = "goods";
            addday.area = "0";
            addday.city = ptypecity.ToString();
            addday.coin = re.coin + 1;
            addday.day = 0;
            new BCW.BLL.dawnlifeDays().Add(addday);

            BCW.Model.dawnlifeDo addbuy = new BCW.Model.dawnlifeDo();
            Addall(addbuy);

            BCW.Model.dawnlifegift addj = new BCW.Model.dawnlifegift();
            int r1 = new BCW.BLL.dawnlifegift().GetMaxId();
            try
            {

            }
            catch
            {
                Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
            }
            BCW.Model.dawnlifegift rr = new BCW.BLL.dawnlifegift().Getdawnlifegift(r1 - 1);
            addj.date = DateTime.Now;
            addj.gift = rr.gift + pay;
            addj.giftj = rr.giftj;
            addj.UsID = meid;
            addj.UsName = name;
            addj.coin = pay;
            addj.state = 0;
            new BCW.BLL.dawnlifegift().Add(addj);

            //动态
            int id = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            string wText = "进入[url=/bbs/game/Dawnlife.aspx]" + GameName + "[/url]，开始精彩冒险";
            new BCW.BLL.Action().Add(1008, id, meid, name, wText);

            Utils.Success("游戏开始", "不要着急，正在前往>>>", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + ""), "1");
        }
        else
        {
            Utils.Error("请刷新一下再进入闯荡", "");
        }
    }
    //城市主操作界面
    private void GZPage()
    {

        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(1);
        BCW.Model.dawnlifedaoju rb = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(2);
        BCW.Model.dawnlifedaoju rs = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(3);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        int ptypecity = Convert.ToInt16(rc.city);

        if (ptypecity == 1)
        {
            rc.city = rg.city;

        }
        if (ptypecity == 2)
        {
            rc.city = rb.city;
        }
        if (ptypecity == 3)
        {
            rc.city = rs.city;
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>" + ">" + rc.city);
        builder.Append(Out.Tab("</div>", "<br />"));

        if (rday.day < 31)//设定游戏地区每切换一个地区就增加一天，只有31天的时间
        {
            int day = Convert.ToInt16(rday.day);
            int r_2 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
            BCW.Model.dawnlifeDays x = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r_2);
            builder.Append(Out.Tab("<div>", ""));
            //消息弹出
            if (day < 1)
            {
                builder.Append("");
            }
            else
            {
                try
                {
                    builder.Append(x.news);
                }
                catch
                {

                }

            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));

            builder.Append("【" + rc.city + "站】" + day + "/31天");
            if (ptypecity == 1)
            {
                string[] area = rg.area.Split('/');
                int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[1-9]$", "0"));
                builder.Append("<br />");
                if (ptype == 1)
                    builder.Append("" + area[0] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=1") + "\">" + area[0] + "</a>|");
                if (ptype == 2)
                    builder.Append("" + area[1] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=2") + "\">" + area[1] + "</a>|");
                if (ptype == 3)
                    builder.Append("" + area[2] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=3") + "\">" + area[2] + "</a>");
                builder.Append("<br />");
                if (ptype == 4)
                    builder.Append("" + area[3] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=4") + "\">" + area[3] + "</a>|");
                if (ptype == 5)
                    builder.Append("" + area[4] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=5") + "\">" + area[4] + "</a>|");
                if (ptype == 6)
                    builder.Append("" + area[5] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=6") + "\">" + area[5] + "</a>");
                builder.Append("<br />");
                if (ptype == 7)
                    builder.Append("" + area[6] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=7") + "\">" + area[6] + "</a>|");
                if (ptype == 8)
                    builder.Append("" + area[7] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=8") + "\">" + area[7] + "</a>|");
                if (ptype == 9)
                    builder.Append("" + area[8] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=9") + "\">" + area[8] + "</a>");
                builder.Append("<br />");
            }
            if (ptypecity == 2)
            {
                string[] area = rb.area.Split('/');
                int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[1-9]$", "0"));
                builder.Append("<br />");
                if (ptype == 1)
                    builder.Append("" + area[0] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=1") + "\">" + area[0] + "</a>|");
                if (ptype == 2)
                    builder.Append("" + area[1] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=2") + "\">" + area[1] + "</a>|");
                if (ptype == 3)
                    builder.Append("" + area[2] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=3") + "\">" + area[2] + "</a>");
                builder.Append("<br />");
                if (ptype == 4)
                    builder.Append("" + area[3] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=4") + "\">" + area[3] + "</a>|");
                if (ptype == 5)
                    builder.Append("" + area[4] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=5") + "\">" + area[4] + "</a>|");
                if (ptype == 6)
                    builder.Append("" + area[5] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=6") + "\">" + area[5] + "</a>");
                builder.Append("<br />");
                if (ptype == 7)
                    builder.Append("" + area[6] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=7") + "\">" + area[6] + "</a>|");
                if (ptype == 8)
                    builder.Append("" + area[7] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=8") + "\">" + area[7] + "</a>|");
                if (ptype == 9)
                    builder.Append("" + area[8] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=9") + "\">" + area[8] + "</a>");
                builder.Append("<br />");
            }
            if (ptypecity == 3)
            {
                string[] area = rs.area.Split('/');
                int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[1-9]$", "0"));
                builder.Append("<br />");
                if (ptype == 1)
                    builder.Append("" + area[0] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=1") + "\">" + area[0] + "</a>|");
                if (ptype == 2)
                    builder.Append("" + area[1] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=2") + "\">" + area[1] + "</a>|");
                if (ptype == 3)
                    builder.Append("" + area[2] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=3") + "\">" + area[2] + "</a>");
                builder.Append("<br />");
                if (ptype == 4)
                    builder.Append("" + area[3] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=4") + "\">" + area[3] + "</a>|");
                if (ptype == 5)
                    builder.Append("" + area[4] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=5") + "\">" + area[4] + "</a>|");
                if (ptype == 6)
                    builder.Append("" + area[5] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=6") + "\">" + area[5] + "</a>");
                builder.Append("<br />");
                if (ptype == 7)
                    builder.Append("" + area[6] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=7") + "\">" + area[6] + "</a>|");
                if (ptype == 8)
                    builder.Append("" + area[7] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=8") + "\">" + area[7] + "</a>|");
                if (ptype == 9)
                    builder.Append("" + area[8] + "");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=pd&amp;city=" + ptypecity + "&amp;area=9") + "\">" + area[8] + "</a>");
                builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));

            int act = int.Parse(Utils.GetRequest("act", "all", 1, @"^[1-3]$", "1"));
            //int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "0"));
            //BCW.Model.dawnlifedaoju dj = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(act);
            builder.Append(Out.Tab("<div>", ""));
            if (day < 1)
                builder.Append("欢迎来到" + rc.city + "!点击切换地区开始闯荡，祝你好运!");
            else
            {
                int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
                BCW.Model.dawnlifeDays xx = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
                int n1 = xx.n;
                string[] goodss = xx.goods.Split(',');
                string[] pricee = xx.price.Split(',');
                //____________________________________________________________
                int[] price1 = new int[n1];
                for (int i = 0; i < n1; i++)
                {
                    int jj = Convert.ToInt32(pricee[i]);
                    price1[i] = jj;
                }
                int tmp = 0;
                int n = price1.Length;
                ArrayList math = new ArrayList();
                for (int i = 0; i < n; i++)
                {
                    math.Add(price1[i]);
                }
                for (int i = 0; i < n; i++)
                {

                    for (int jj = 1; jj < n - i; jj++)
                    {
                        if (price1[jj] > price1[jj - 1])
                        {
                            tmp = price1[jj - 1];
                            price1[jj - 1] = price1[jj];
                            price1[jj] = tmp;
                        }
                    }
                }
                int[] position = new int[price1.Length];
                for (int i = 0; i < n; i++)
                {
                    position[i] = math.IndexOf(price1[i]);
                }

                //___________________________________________________________
                int j = Convert.ToInt16(xx.area);
                builder.Append("<table>");
                builder.Append("<tr><td>该地市场商品</td><td>黑市价</td><td>交易</td></tr>");
                for (int i = 0; i < n1; i++)
                {
                    string g1 = Convert.ToString(goodss[position[i]]);
                    string p1 = Convert.ToString(price1[i]);
                    builder.Append("<tr><td>" + g1 + "</td><td>" + p1 + "</td><td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=buy&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + position[i] + "") + "\">[买入]</a>" + "</td></tr>");

                } builder.Append("</table>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            //成功买入的物品，价钱与数量，库存等显示
            int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
            BCW.Model.dawnlifeDo buy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
            builder.Append(Out.Tab("<div>", ""));

            if (buy.stocky > 0)
            {

                BCW.Model.dawnlifeDo xxx = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
                string[] goodss = xxx.goods.Split(',');
                string[] priceee = xxx.price.Split(',');
                string[] dsgg = xxx.dsg.Split(',');
                //获取","个数，得到多少个物品
                string str = xxx.goods;
                int s = 0;
                foreach (char item in str)
                {
                    if (item != 44)
                    {
                    }
                    else
                    {
                        s++;
                    }
                }
                int ptype2 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "0"));
                builder.Append("<table>");
                builder.Append("<tr><td>库存：" + buy.stocky + "/" + buy.stock + "</td></tr>");
                builder.Append("<tr><td>商品名" + "</td><td>买入价</td>" + "<td>数量</td><td>卖</td></tr>");
                for (int i = 0; i < s; i++)
                {
                    string goods = Convert.ToString(goodss[i + 1]);
                    string dsg = Convert.ToString(dsgg[i + 1]);
                    string price = Convert.ToString(priceee[i]);
                    if (goods == "")
                    {
                        builder.Append("");
                    }
                    else
                    {
                        builder.Append("<tr><td>" + goods + "</td><td>" + price + "</td><td>" + dsg + "</td><td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=sell&amp;city=" + ptypecity + "&amp;ptype=" + i + "") + "\">卖出</a>" + "</td></tr>");
                    }
                }
                builder.Append("</table>");
            }
            else
                builder.Append("你的库存为零!");
            builder.Append(Out.Tab("</div>", ""));

            int r3 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r3);
            builder.Append(Out.Tab("<div>", "<br />"));
            if (day > 0)
            {
                builder.Append("<table>");

                builder.Append("<tr><td>现金：" + re.money + "银两 </td></tr> ");

                builder.Append("<tr><td>欠款：" + re.debt + "银两</td><td>");
                builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=repay&amp;city=" + ptypecity + "") + "\">[还钱]</a></td></tr>");
                builder.Append("<tr><td>健康：" + re.health + "</td><td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=hospital&amp;city=" + ptypecity + "") + "\">[医院]</a></td></tr>");

                if (re.health <= 60)
                {
                    builder.Append("");
                    builder.Append("你的健康值太低,请去恢复体力");
                    if (re.health <= 45)
                    {
                        Utils.Success("温馨提示", "你的健康值不足以继续游戏，请去恢复体力 ", Utils.getUrl("Dawnlife.aspx?act=hospital&amp;city=" + ptypecity + ""), "2");
                    }
                }

                builder.Append("<tr><td>名声：" + re.reputation + "</td><td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=donation&amp;city=" + ptypecity + "") + "\">[捐款]</a></td></tr>");
                if (re.reputation <= 60)
                {
                    builder.Append("");
                    builder.Append("你的名声太低,请去恢复声望。。。");
                    if (re.reputation <= 45)
                    {
                        Utils.Success("温馨提示", "你的名声值不足以继续游戏，请去恢复名声", Utils.getUrl("Dawnlife.aspx?act=donation&amp;city=" + ptypecity + ""), "2");
                    }
                }

                builder.Append("<tr><td>仓库：" + re.storehouse + "</td><td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=upgrade&amp;city=" + ptypecity + "") + "\">[升级]</a></td></tr>");
                builder.Append("</table>");


            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append("现金：" + re.money + "银两  ");
                builder.Append("<br />");
                builder.Append("欠款：" + re.debt + "银两");
                builder.Append("<br />");
                builder.Append("健康：" + re.health + "");
                builder.Append("<br />");
                builder.Append("名声：" + re.reputation + "");
                builder.Append("<br />");
                builder.Append("仓库：" + re.storehouse + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=bfend&amp;city=" + ptypecity + "") + "\">结束游戏</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (rday.day == 31)//达到第31天
        {

            ////////////////////////////////////
            if (meid == 0)
                Utils.Login();
            if (ptypecity == 1)
            {
                rc.city = rg.city;
                string[] area = rg.area.Split('/');
            }
            if (ptypecity == 2)
            {
                rc.city = rb.city;
            }
            if (ptypecity == 3)
            {
                rc.city = rs.city;
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【" + rc.city + "】" + rday.day + "/31天");
            builder.Append(Out.Tab("</div>", ""));

            string name = new BCW.BLL.User().GetUsName(meid);
            //r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            int day = Convert.ToInt16(rday.day);
            int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
            int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
            BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
            BCW.Model.dawnlifeDo rw = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
            BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
            BCW.Model.dawnlifeDays xx = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
            //成功买入的物品，价钱与数量，库存等显示
            int r6 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
            BCW.Model.dawnlifeDo buy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r6);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<table>");
            builder.Append("<tr><td>该地市场商品</td><td>黑市价</td></tr>");
            if (buy.stocky > 0)
            {
                int n1 = xx.n;
                string[] goodssx = xx.goods.Split(',');
                string[] priceex = xx.price.Split(',');
                int j = Convert.ToInt16(xx.area);
                for (int i = 0; i < n1; i++)
                {
                    string g1 = Convert.ToString(goodssx[i]);
                    string p1 = Convert.ToString(priceex[i]);

                    builder.Append("<tr><td>" + g1 + "</td><td>" + p1 + "</td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=buy&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + i + "") + "\"></a>" + "</tr>");

                    string[] googss = rw.goods.Split(',');
                    string[] dsgs = rw.dsg.Split(',');
                    //获取","个数，得到多少个物品
                    BCW.Model.dawnlifeDo xxx = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r6);
                    string[] goodss = xxx.goods.Split(',');
                    string[] priceee = xxx.price.Split(',');
                    string[] dsgg = xxx.dsg.Split(',');

                    //获取","个数，得到多少个物品
                    string str = xxx.goods;
                    int s = 0;
                    foreach (char item in str)
                    {
                        if (item != 44)
                        {
                        }
                        else
                        {
                            s++;
                        }
                    }
                    int ptype2 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "0"));
                    for (int i1 = 0; i1 < s; i1++)
                    {
                        string goods = Convert.ToString(goodss[i1 + 1]);
                        string dsg = Convert.ToString(dsgg[i1 + 1]);
                        string price = Convert.ToString(priceee[i1]);
                    }
                }

            }
            else
            {
                int n1 = xx.n;
                string[] goodssx = xx.goods.Split(',');
                string[] priceex = xx.price.Split(',');
                int j = Convert.ToInt16(xx.area);

                for (int i = 0; i < n1; i++)
                {
                    string g1 = Convert.ToString(goodssx[i]);
                    string p1 = Convert.ToString(priceex[i]);

                    builder.Append("<tr><td>" + g1 + "</td><td>" + p1 + "</td><a href=\"" + Utils.getUrl("Dawnlife.aspx?act=buy&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + i + "") + "\"></a>" + " </tr>");
                }

            }
            builder.Append(Out.Tab("</div class=\"class\">", ""));
            builder.Append("</table>");
            builder.Append(Out.Tab("", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            if (re.debt > 0)
            {
                if (re.money < 0 && re.money > -1000)
                {
                    builder.Append("海关替你把剩余的货物都卖光了!" + "<br />" + "你已经在" + rc.city + "整整闯荡了1个月,胖大海把你抓去当黑奴,许配给了芙蓉姐姐...!");
                }
                else if (re.money < -1000 && re.money > -5000)
                {
                    builder.Append("海关没收你所有的货物!" + "<br />" + "你已经在" + rc.city + "整整闯荡了1个月,胖大海把你抓去当奴隶,过着牛马的生活...!");
                }
                else if (re.money < -5000 && re.money > -20000)
                {
                    builder.Append("天命不可违啊，生意做不好，饭都吃不饱!" + "<br />" + "你已经在" + rc.city + "整整闯荡了1个月,由于你还不起高额欠款，胖大海把你抓去当码头苦工,这辈子算是只能干苦力了...!");
                }
                else
                {
                    builder.Append("哎，人生大起大落，总有失意的时候，没有过不去的坎，加油吧，奋斗总会看到希望！继续努力吧！");
                }

            }
            else
            {
                if (re.money > 0 && re.money < 100000)
                {
                    builder.Append("你终于把剩余的货物都卖光了!你已经在" + rc.city + "整整闯荡了1个月,挣了不少钱，该回去结婚生大胖儿子了!");
                }
                else if (re.money > 100000 && re.money < 1000000)
                {
                    builder.Append("你终于把剩余的货物都卖光了!你已经在" + rc.city + "整整闯荡了1个月,挣了不少钱，该回去了，都可以买辆车了!");
                }
                else if (re.money > 1000000 && re.money < 10000000)
                {
                    builder.Append("你终于把剩余的货物都卖光了!你已经在" + rc.city + "整整闯荡了1个月,挣了不少钱，衣锦还乡，买房买车逍遥多了，该回去孝敬家中父母了!");
                }
                else if (re.money > 10000000 && re.money < 100000000)
                {
                    builder.Append("你终于把剩余的货物都卖光了!你已经在" + rc.city + "整整闯荡了1个月,发大财了，你已经是千万富翁啊，再也不愁钱了，够你环游世界，享受生活了!");
                }
                else
                {
                    builder.Append("你终于把剩余的货物都卖光了!你已经在" + rc.city + "整整闯荡了1个月,挣的钱一辈子都花不完，你向世界和平基金捐款，成为名人，名利双收，可谓是人生赢家啊!");
                }

            }
            builder.Append(Out.Tab("</div>", "<br />"));

            // int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "1"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("现金：" + re.money + "");//剩余物品自动全部售出
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("健康：" + re.health + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("声望：" + re.reputation + "");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"class\">", Out.Hr()));
            builder.Append("你的闯荡已经结束了");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=ss&amp;city=" + ptypecity + "") + "\">再次闯荡</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", Out.Hr()));
        }

        else if (rday.day > 31)
        {
            Utils.Success("闯荡", "你的闯荡已经结束，再去开始新的闯荡吧", Utils.getUrl("Dawnlife.aspx"), "2");
        }
        else
            Utils.Error("非法操作.", "");


        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //判断地区切换
    private void PDPage()
    {
        ////切换地区欠款增加
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);

        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeUser add = new BCW.Model.dawnlifeUser();
        add.debt = re.debt + re.debt / 10;
        new BCW.BLL.dawnlifeUser().Updatedebt(r, add.debt);
        //随机生成的物品与价格
        int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[1-9]$", "1"));
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int r1 = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rw = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        BCW.Model.dawnlifeDays addd = new BCW.Model.dawnlifeDays();
        if (rw.day < 30)
        {
            //随机产生4-7条不重复的1-10的数
            int n = R(4, 8);//随机生成4-7条
            int[] goods = new int[10];
            for (int i = 0; i < 10; i++) goods[i] = i + 1;
            for (int j = 9; j > 0; j--)
            {
                Random ra = new Random();
                int index = ra.Next(0, j);
                int temp = goods[index];
                goods[index] = goods[j];
                goods[j] = temp;
            }
            //冒泡排序 从大到小
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (goods[j] > goods[i])
                    {
                        int temp = goods[i];
                        goods[i] = goods[j];
                        goods[j] = temp;
                    }
                }
            }
            string goodss = "";
            string pricee = "";

            int t = R(0, 21);//设置相应的连锁功能
            string news = f[t];
            string news3 = f1[t];
            int p = R(0, 72);
            string news1 = f2[p];
            for (int i = 0; i < n; i++)//遍历数组显示结果
            {
                BCW.Model.dawnlifedaoju g1 = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(goods[i]);
                goodss = goodss + g1.goods + ",";
                BCW.Model.dawnlifedaoju p1 = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(goods[i]);
                int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
                BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
                string aa = news;
                string cc = news3;
                string bb = news1;

                if (ptypecity == 1)
                {
                    switch (p1.price.Trim())
                    {
                        case "n1"://盗版DVD
                            string a1 = f[16]; //"【小道消息】<br />文盲说:2009年诺贝尔文学奖?呸!不如盗版DVD港台片";
                            string a2 = f[11]; string a3 = f1[12]; string a4 = f1[14];
                            if (aa.IndexOf(a2) > -1 || cc.IndexOf(a3) > -1 || cc.IndexOf(a4) > -1) { p1.price = Convert.ToString(R(1, 3)); }
                            else if (aa.IndexOf(a1) > -1) { p1.price = Convert.ToString(R(159, 360)); }
                            else { p1.price = Convert.ToString(R(1, 61)); }
                            break;
                        case "n2"://阳澄大闸蟹
                            string a5 = f1[17]; string a51 = f[10]; string a22 = f[15];
                            if (cc.IndexOf(a5) > -1 || aa.IndexOf(a22) > -1) { p1.price = Convert.ToString(R(2, 21)); }
                            else if (aa.IndexOf(a51) > -1) { p1.price = Convert.ToString(R(180, 491)); }
                            else { p1.price = Convert.ToString(R(2, 200)); }
                            break;
                        case "n3"://三路奶粉
                            string a6 = f[9];
                            if (aa.IndexOf(a6) > -1) { p1.price = Convert.ToString(R(5, 50)); }
                            else { p1.price = Convert.ToString(R(50, 611)); }
                            break;
                        case "n4"://走私香烟
                            string a7 = f[19]; string a8 = f1[18];
                            if (aa.IndexOf(a7) > -1 || cc.IndexOf(a8) > -1) { p1.price = Convert.ToString(R(50, 108)); }
                            else { p1.price = Convert.ToString(R(160, 1100)); }
                            break;
                        case "n5"://名牌A货
                            string a9 = f[8]; string a10 = f[12];
                            if (aa.IndexOf(a9) > -1) { p1.price = Convert.ToString(R(36, 99)); }
                            else if (aa.IndexOf(a10) > -1) { p1.price = Convert.ToString(R(1300, 1800)); }
                            else { p1.price = Convert.ToString(R(499, 1600)); }
                            break;
                        case "n6"://假酒
                            string a61 = f1[11]; string a62 = f1[13]; string a63 = f1[19]; string a64 = f1[20];
                            if (cc.IndexOf(a61) > -1 || cc.IndexOf(a62) > -1 || cc.IndexOf(a63) > -1 || cc.IndexOf(a64) > -1) { p1.price = Convert.ToString(R(4000, 8600)); }
                            else { p1.price = Convert.ToString(R(120, 2100)); }
                            break;
                        case "n7"://山寨手机
                            string a71 = f[5]; string a72 = f[7]; string a73 = f[14]; string a74 = f[20];
                            if (aa.IndexOf(a71) > -1 || aa.IndexOf(a73) > -1) { p1.price = Convert.ToString(R(5000, 10000)); }
                            else if (aa.IndexOf(a72) > -1 || aa.IndexOf(a74) > -1) { p1.price = Convert.ToString(R(200, 500)); }
                            else { p1.price = Convert.ToString(R(300, 9999)); }
                            break;
                        case "n8"://二手笔记本
                            string a13 = f[17];
                            if (aa.IndexOf(a13) > -1) { p1.price = Convert.ToString(R(6000, 36100)); }
                            else { p1.price = Convert.ToString(R(500, 6000)); }
                            break;
                        case "n9"://如来神掌
                            string a14 = f1[6]; string a15 = f[16]; string a91 = f[9];
                            if (cc.IndexOf(a14) > -1 || aa.IndexOf(a15) > -1 || aa.IndexOf(a91) > -1) { p1.price = Convert.ToString(R(30000, 99998)); }
                            else { p1.price = Convert.ToString(R(3600, 9000)); }
                            break;
                        case "n10"://走私汽车
                            string a16 = f1[7]; string a17 = f1[15];
                            if (cc.IndexOf(a16) > -1 || cc.IndexOf(a17) > -1) { p1.price = Convert.ToString(R(81000, 490000)); }
                            else { p1.price = Convert.ToString(R(7000, 50000)); }
                            break;
                    }
                    string[] ai = pricee.Split(',');
                    bool ia = ((IList)ai).Contains(p1.price);
                    if (ia)
                    {
                        p1.price = (Convert.ToInt64(p1.price) + 1).ToString();
                    }
                    pricee = pricee + p1.price + ",";
                }

                else if (ptypecity == 2)
                {
                    switch (p1.price.Trim())
                    {
                        case "n1"://盗版DVD
                            string a1 = f[16]; //"【小道消息】<br />文盲说:2009年诺贝尔文学奖?呸!不如盗版DVD港台片";
                            string a2 = f[11]; string a3 = f1[12]; string a4 = f1[14];
                            if (aa.IndexOf(a2) > -1 || cc.IndexOf(a3) > -1 || cc.IndexOf(a4) > -1) { p1.price = Convert.ToString(R(1, 6)); }
                            else if (aa.IndexOf(a1) > -1) { p1.price = Convert.ToString(R(159, 380)); }
                            else { p1.price = Convert.ToString(R(1, 56)); }
                            break;
                        case "n2"://阳澄大闸蟹
                            string a5 = f1[17]; string a51 = f[10]; string a22 = f[15];
                            if (cc.IndexOf(a5) > -1 || aa.IndexOf(a22) > -1) { p1.price = Convert.ToString(R(2, 26)); }
                            else if (aa.IndexOf(a51) > -1) { p1.price = Convert.ToString(R(190, 450)); }
                            else { p1.price = Convert.ToString(R(2, 210)); }
                            break;
                        case "n3"://三路奶粉
                            string a6 = f[9];
                            if (aa.IndexOf(a6) > -1) { p1.price = Convert.ToString(R(5, 46)); }
                            else { p1.price = Convert.ToString(R(30, 650)); }
                            break;
                        case "n4"://走私香烟
                            string a7 = f[19]; string a8 = f1[18];
                            if (aa.IndexOf(a7) > -1 || cc.IndexOf(a8) > -1) { p1.price = Convert.ToString(R(50, 100)); }
                            else { p1.price = Convert.ToString(R(160, 1100)); }
                            break;
                        case "n5"://名牌A货
                            string a9 = f[8]; string a10 = f[12];
                            if (aa.IndexOf(a9) > -1) { p1.price = Convert.ToString(R(30, 90)); }
                            else if (aa.IndexOf(a10) > -1) { p1.price = Convert.ToString(R(1300, 1800)); }
                            else { p1.price = Convert.ToString(R(360, 1500)); }
                            break;
                        case "n6"://假酒
                            string a61 = f1[11]; string a62 = f1[13]; string a63 = f1[19]; string a64 = f1[20];
                            if (cc.IndexOf(a61) > -1 || cc.IndexOf(a62) > -1 || cc.IndexOf(a63) > -1 || cc.IndexOf(a64) > -1) { p1.price = Convert.ToString(R(3600, 8200)); }
                            else { p1.price = Convert.ToString(R(100, 3600)); }
                            break;
                        case "n7"://山寨手机
                            string a71 = f[5]; string a72 = f[7]; string a73 = f[14]; string a74 = f[20];
                            if (aa.IndexOf(a71) > -1 || aa.IndexOf(a73) > -1) { p1.price = Convert.ToString(R(4000, 9800)); }
                            else if (aa.IndexOf(a72) > -1 || aa.IndexOf(a74) > -1) { p1.price = Convert.ToString(R(200, 500)); }
                            else { p1.price = Convert.ToString(R(360, 9900)); }
                            break;
                        case "n8"://二手笔记本
                            string a13 = f[17];
                            if (aa.IndexOf(a13) > -1) { p1.price = Convert.ToString(R(8000, 38000)); }
                            else { p1.price = Convert.ToString(R(540, 7000)); }
                            break;
                        case "n9"://如来神掌
                            string a14 = f1[6]; string a15 = f[16]; string a91 = f[9];
                            if (cc.IndexOf(a14) > -1 || aa.IndexOf(a15) > -1 || aa.IndexOf(a91) > -1) { p1.price = Convert.ToString(R(29000, 98888)); }
                            else { p1.price = Convert.ToString(R(3500, 9000)); }
                            break;
                        case "n10"://走私汽车
                            string a16 = f1[7]; string a17 = f1[15];
                            if (cc.IndexOf(a16) > -1 || cc.IndexOf(a17) > -1) { p1.price = Convert.ToString(R(80000, 510000)); }
                            else { p1.price = Convert.ToString(R(6000, 53000)); }
                            break;
                    }
                    string[] ai = pricee.Split(',');
                    bool ia = ((IList)ai).Contains(p1.price);
                    if (ia)
                    {
                        p1.price = (Convert.ToInt64(p1.price) + 1).ToString();
                    }
                    pricee = pricee + p1.price + ",";
                }
                else if (ptypecity == 3)
                {
                    switch (p1.price.Trim())
                    {
                        case "n1"://盗版DVD
                            string a1 = f[16]; //"【小道消息】<br />文盲说:2009年诺贝尔文学奖?呸!不如盗版DVD港台片";
                            string a2 = f[11]; string a3 = f1[12]; string a4 = f1[14];
                            if (aa.IndexOf(a2) > -1 || cc.IndexOf(a3) > -1 || cc.IndexOf(a4) > -1) { p1.price = Convert.ToString(R(1, 5)); }
                            else if (aa.IndexOf(a1) > -1) { p1.price = Convert.ToString(R(159, 366)); }
                            else { p1.price = Convert.ToString(R(1, 51)); }
                            break;
                        case "n2"://阳澄大闸蟹
                            string a5 = f1[17]; string a51 = f[10]; string a22 = f[15];
                            if (cc.IndexOf(a5) > -1 || aa.IndexOf(a22) > -1) { p1.price = Convert.ToString(R(2, 20)); }
                            else if (aa.IndexOf(a51) > -1) { p1.price = Convert.ToString(R(200, 500)); }
                            else { p1.price = Convert.ToString(R(2, 200)); }
                            break;
                        case "n3"://三路奶粉
                            string a6 = f[9];
                            if (aa.IndexOf(a6) > -1) { p1.price = Convert.ToString(R(5, 60)); }
                            else { p1.price = Convert.ToString(R(50, 590)); }
                            break;
                        case "n4"://走私香烟
                            string a7 = f[19]; string a8 = f1[18];
                            if (aa.IndexOf(a7) > -1 || cc.IndexOf(a8) > -1) { p1.price = Convert.ToString(R(50, 101)); }
                            else { p1.price = Convert.ToString(R(170, 1200)); }
                            break;
                        case "n5"://名牌A货
                            string a9 = f[8]; string a10 = f[12];
                            if (aa.IndexOf(a9) > -1) { p1.price = Convert.ToString(R(35, 110)); }
                            else if (aa.IndexOf(a10) > -1) { p1.price = Convert.ToString(R(1200, 1900)); }
                            else { p1.price = Convert.ToString(R(466, 1700)); }
                            break;
                        case "n6"://假酒
                            string a61 = f1[11]; string a62 = f1[13]; string a63 = f1[19]; string a64 = f1[20];
                            if (cc.IndexOf(a61) > -1 || cc.IndexOf(a62) > -1 || cc.IndexOf(a63) > -1 || cc.IndexOf(a64) > -1) { p1.price = Convert.ToString(R(3000, 9000)); }
                            else { p1.price = Convert.ToString(R(150, 2500)); }
                            break;
                        case "n7"://山寨手机
                            string a71 = f[5]; string a72 = f[7]; string a73 = f[14]; string a74 = f[20];
                            if (aa.IndexOf(a71) > -1 || aa.IndexOf(a73) > -1) { p1.price = Convert.ToString(R(6000, 10000)); }
                            else if (aa.IndexOf(a72) > -1 || aa.IndexOf(a74) > -1) { p1.price = Convert.ToString(R(200, 500)); }
                            else { p1.price = Convert.ToString(R(290, 9800)); }
                            break;
                        case "n8"://二手笔记本
                            string a13 = f[17];
                            if (aa.IndexOf(a13) > -1) { p1.price = Convert.ToString(R(6800, 35000)); }
                            else { p1.price = Convert.ToString(R(540, 6700)); }
                            break;
                        case "n9"://如来神掌
                            string a14 = f1[6]; string a15 = f[16]; string a91 = f[9];
                            if (cc.IndexOf(a14) > -1 || aa.IndexOf(a15) > -1 || aa.IndexOf(a91) > -1) { p1.price = Convert.ToString(R(28000, 99900)); }
                            else { p1.price = Convert.ToString(R(3200, 8600)); }
                            break;
                        case "n10"://走私汽车
                            string a16 = f1[7]; string a17 = f1[15];
                            if (cc.IndexOf(a16) > -1 || cc.IndexOf(a17) > -1) { p1.price = Convert.ToString(R(86000, 501000)); }
                            else { p1.price = Convert.ToString(R(6800, 58000)); }
                            break;
                    }
                    string[] ai = pricee.Split(',');
                    bool ia = ((IList)ai).Contains(p1.price);
                    if (ia)
                    {
                        p1.price = (Convert.ToInt64(p1.price) + 1).ToString();
                    }
                    pricee = pricee + p1.price + ",";
                }
                else
                    Utils.Error("非法操作", "");

            }
            if (f2[p] == f2[1] || f2[p] == f2[9] || f2[p] == f2[15] || f2[p] == f2[31] || f2[p] == f2[41])
            {
                new BCW.BLL.dawnlifeUser().Updatehealth(r, Convert.ToInt16(re.health - 1));
                BCW.Model.dawnlifenotes notes1 = new BCW.Model.dawnlifenotes();
                notes1.coin = Convert.ToInt32(re.coin);
                notes1.UsID = meid;
                notes1.money = re.money;
                notes1.debt = re.debt + re.debt / 10;
                notes1.city = ptypecity;
                notes1.area = ptype;
                notes1.day = Convert.ToInt32(rw.day + 1);
                notes1.buy = "buy";
                notes1.sell = "sell";
                notes1.price = 0;
                notes1.num = 0;
                notes1.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes1);
            }
            else if (f2[p] == f2[3] || f2[p] == f2[35])
            {
                new BCW.BLL.dawnlifeUser().Updatehealth(r, Convert.ToInt16(re.health - 2));
                BCW.Model.dawnlifenotes notes1 = new BCW.Model.dawnlifenotes();
                notes1.coin = Convert.ToInt32(re.coin);
                notes1.UsID = meid;
                notes1.money = re.money;
                notes1.debt = re.debt + re.debt / 10;
                notes1.city = ptypecity;
                notes1.area = ptype;
                notes1.day = Convert.ToInt32(rw.day + 1);
                notes1.buy = "buy";
                notes1.sell = "sell";
                notes1.price = 0;
                notes1.num = 0;
                notes1.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes1);
            }
            else if (f2[p] == f2[7] || f2[p] == f2[19] || f2[p] == f2[21] || f2[p] == f2[25] || f2[p] == f2[27] || f2[p] == f2[39])
            {
                new BCW.BLL.dawnlifeUser().Updatehealth(r, Convert.ToInt16(re.health - 3));
                BCW.Model.dawnlifenotes notes1 = new BCW.Model.dawnlifenotes();
                notes1.coin = Convert.ToInt32(re.coin);
                notes1.UsID = meid;
                notes1.money = re.money;
                notes1.debt = re.debt + re.debt / 10;
                notes1.city = ptypecity;
                notes1.area = ptype;
                notes1.day = Convert.ToInt32(rw.day + 1);
                notes1.buy = "buy";
                notes1.sell = "sell";
                notes1.price = 0;
                notes1.num = 0;
                notes1.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes1);
            }
            else if (f2[p] == f2[13])
            {
                new BCW.BLL.dawnlifeUser().Updatehealth(r, Convert.ToInt16(re.health - 5));
                BCW.Model.dawnlifenotes notes1 = new BCW.Model.dawnlifenotes();
                notes1.coin = Convert.ToInt32(re.coin);
                notes1.UsID = meid;
                notes1.money = re.money;
                notes1.debt = re.debt + re.debt / 10;
                notes1.city = ptypecity;
                notes1.area = ptype;
                notes1.day = Convert.ToInt32(rw.day + 1);
                notes1.buy = "buy";
                notes1.sell = "sell";
                notes1.price = 0;
                notes1.num = 0;
                notes1.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes1);
            }
            else if (f2[p] == f2[43] || f2[p] == f2[53])
            {
                new BCW.BLL.dawnlifeUser().Updatehealth(r, Convert.ToInt16(re.health - 10));
                BCW.Model.dawnlifenotes notes1 = new BCW.Model.dawnlifenotes();
                notes1.coin = Convert.ToInt32(re.coin);
                notes1.UsID = meid;
                notes1.money = re.money;
                notes1.debt = re.debt + re.debt / 10;
                notes1.city = ptypecity;
                notes1.area = ptype;
                notes1.day = Convert.ToInt32(rw.day + 1);
                notes1.buy = "buy";
                notes1.sell = "sell";
                notes1.price = 0;
                notes1.num = 0;
                notes1.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes1);
            }
            else if (f2[p] == f2[49])
            {
                new BCW.BLL.dawnlifeUser().Updatemoney(r, Convert.ToInt64(re.money - re.money * 0.1));
                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = Convert.ToInt64(re.money - re.money * 0.1);
                notes.debt = re.debt + re.debt / 10;
                notes.city = ptypecity;
                notes.area = ptype;
                notes.day = Convert.ToInt32(rw.day + 1);
                notes.buy = "buy0.1";
                notes.sell = "sell";
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
            }
            else if (f2[p] == f2[11] || f2[p] == f2[23] || f2[p] == f2[33] || f2[p] == f2[37] || f2[p] == f2[51] || f2[p] == f2[57])
            {
                new BCW.BLL.dawnlifeUser().Updatemoney(r, Convert.ToInt64(re.money - re.money * 0.05));

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = Convert.ToInt64(re.money - re.money * 0.05);
                notes.debt = re.debt + re.debt / 10;
                notes.city = ptypecity;
                notes.area = ptype;
                notes.day = Convert.ToInt32(rw.day + 1);
                notes.buy = "buy0.05";
                notes.sell = "sell";
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
            }
            else if (f2[p] == f2[17] || f2[p] == f2[45])
            {
                if (re.money > 0)
                {
                    new BCW.BLL.dawnlifeUser().Updatemoney(r, Convert.ToInt64(re.money - re.money * 0.05));
                    BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                    notes.coin = Convert.ToInt32(re.coin);
                    notes.UsID = meid;
                    notes.money = Convert.ToInt64(re.money - re.money * 0.05);
                    notes.debt = re.debt + re.debt / 10;
                    notes.city = ptypecity;
                    notes.area = ptype;
                    notes.day = Convert.ToInt32(rw.day + 1);
                    notes.buy = "buy0.05";
                    notes.sell = "sell";
                    notes.price = 0;
                    notes.num = 0;
                    notes.date = DateTime.Now;
                    new BCW.BLL.dawnlifenotes().Add(notes);
                }
                else
                {
                    new BCW.BLL.dawnlifeUser().Updatedebt(r, Convert.ToInt64(re.debt + re.debt * 0.05));
                    BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                    notes.coin = Convert.ToInt32(re.coin);
                    notes.UsID = meid;
                    notes.money = re.money;
                    notes.debt = Convert.ToInt64(re.debt + re.debt * 0.05);
                    notes.city = ptypecity;
                    notes.area = ptype;
                    notes.day = Convert.ToInt32(rw.day + 1);
                    notes.buy = "buy";
                    if (re.debt == 0) { notes.sell = "sell"; }
                    else { notes.sell = "sell0.05"; }
                    notes.price = 0;
                    notes.num = 0;
                    notes.date = DateTime.Now;
                    new BCW.BLL.dawnlifenotes().Add(notes);
                }
            }
            else if (f2[p] == f2[29] || f2[p] == f2[49])
            {
                new BCW.BLL.dawnlifeUser().Updatedebt(r, Convert.ToInt64(re.debt + re.debt * 0.05));
                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = re.money;
                notes.debt = Convert.ToInt64(re.debt + re.debt * 0.05);
                notes.city = ptypecity;
                notes.area = ptype;
                notes.day = Convert.ToInt32(rw.day + 1);
                notes.buy = "buy";
                if (re.debt == 0) { notes.sell = "sell"; }
                else { notes.sell = "sell0.05"; }
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
            }
            else if (f2[p] == f2[55])
            {
                new BCW.BLL.dawnlifeUser().Updatedebt(r, Convert.ToInt64(re.debt + 9968));
                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = re.money;
                notes.debt = Convert.ToInt64(re.debt + 9968);
                notes.city = ptypecity;
                notes.area = ptype;
                notes.day = Convert.ToInt32(rw.day + 1);
                notes.buy = "buy";
                notes.sell = "sell9968";
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
            }
            else if (f2[p] == f2[2])
            {
                int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
                BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
                BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
                addbuy.UsID = ry.UsID;
                addbuy.UsName = ry.UsName;
                addbuy.sum = ry.sum;
                addbuy.coin = ry.coin;
                addbuy.stock = ry.stock;
                addbuy.stocky = ry.stocky + 2;
                addbuy.goods = ry.goods + "," + "如来神掌";
                addbuy.price = ry.price + "赠送" + ",";
                addbuy.dsg = ry.dsg + "," + "2";
                new BCW.BLL.dawnlifeDo().Update(addbuy);

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(ry.coin);
                notes.UsID = meid;
                notes.money = re.money;
                notes.debt = re.debt + re.debt / 10;
                notes.city = ptypecity;
                notes.area = ptype;
                notes.day = Convert.ToInt32(rw.day + 1);
                notes.buy = "如来神掌";
                notes.sell = "sellr";
                notes.price = 0;
                notes.num = 2;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
            }
            else if (f2[p] == f2[5])
            {
                int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
                BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
                BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
                addbuy.UsID = ry.UsID;
                addbuy.UsName = ry.UsName;
                addbuy.sum = ry.sum;
                addbuy.coin = ry.coin;
                addbuy.stock = ry.stock;
                addbuy.stocky = ry.stocky + 1;
                addbuy.goods = ry.goods + "," + "走私汽车";
                addbuy.price = ry.price + "赠送" + ",";
                addbuy.dsg = ry.dsg + "," + "1";
                new BCW.BLL.dawnlifeDo().Update(addbuy);

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(ry.coin);
                notes.UsID = meid;
                notes.money = re.money;
                notes.debt = re.debt + re.debt / 10;
                notes.city = ptypecity;
                notes.area = ptype;
                notes.day = Convert.ToInt32(rw.day + 1);
                notes.buy = "走私汽车";
                notes.sell = "sellz";
                notes.price = 0;
                notes.num = 1;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
            }
            else
            {
                BCW.Model.dawnlifenotes notes1 = new BCW.Model.dawnlifenotes();
                notes1.coin = Convert.ToInt32(re.coin);
                notes1.UsID = meid;
                notes1.money = re.money;
                notes1.debt = re.debt + re.debt / 10;
                notes1.city = ptypecity;
                notes1.area = ptype;
                notes1.day = Convert.ToInt32(rw.day + 1);
                notes1.buy = "buy";
                notes1.sell = "sell";
                notes1.price = 0;
                notes1.num = 0;
                notes1.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes1);
            }
            addd.UsID = meid;
            addd.UsName = name;
            addd.day = rw.day + 1;
            addd.goods = goodss;
            addd.price = pricee;
            addd.city = ptypecity.ToString();
            addd.area = ptype.ToString();
            addd.coin = coin;
            addd.n = n;
            addd.news = news + "<br />" + news3 + "<br />" + news1;
            new BCW.BLL.dawnlifeDays().Add(addd);

        }
        else
        {  //int n1 = 10;//随机生成4-7条
            int[] goods1 = new int[10];
            for (int i = 0; i < 10; i++) goods1[i] = i + 1;
            for (int j = 9; j > 0; j--)
            {
                Random ra = new Random();
                int index = ra.Next(0, j);
                int temp = goods1[index];
                goods1[index] = goods1[j];
                goods1[j] = temp;
            }
            //冒泡排序 从大到小
            for (int i = 0; i < 10; i++)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    if (goods1[j] > goods1[i])
                    {
                        int temp = goods1[i];
                        goods1[i] = goods1[j];
                        goods1[j] = temp;
                    }
                }
            }
            string goodss1 = "";
            string pricee1 = "";
            for (int i = 0; i < 10; i++)//遍历数组显示结果
            {
                BCW.Model.dawnlifedaoju g1 = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(goods1[i]);
                goodss1 = goodss1 + g1.goods + ",";
                BCW.Model.dawnlifedaoju p1 = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(goods1[i]);
                switch (p1.price.Trim())
                {
                    case "n1":
                        p1.price = Convert.ToString(R(40, 90));
                        break;
                    case "n2":
                        p1.price = Convert.ToString(R(160, 280));
                        break;
                    case "n3":
                        p1.price = Convert.ToString(R(360, 660));
                        break;
                    case "n4":
                        p1.price = Convert.ToString(R(900, 1600));
                        break;
                    case "n5":
                        p1.price = Convert.ToString(R(1100, 1760));
                        break;
                    case "n6":
                        p1.price = Convert.ToString(R(3000, 6800));
                        break;
                    case "n7":
                        p1.price = Convert.ToString(R(3600, 8000));
                        break;
                    case "n8":
                        p1.price = Convert.ToString(R(4000, 9000));
                        break;
                    case "n9":
                        p1.price = Convert.ToString(R(35000, 68000));
                        break;
                    case "n10":
                        p1.price = Convert.ToString(R(101000, 360800));
                        break;
                }
                pricee1 = pricee1 + p1.price + ",";
            }
            addd.UsID = meid;
            addd.UsName = name;
            addd.day = rw.day + 1;
            addd.goods = goodss1;
            addd.price = pricee1;
            addd.city = ptypecity.ToString();
            addd.area = ptype.ToString();
            addd.coin = coin;
            addd.n = 10;
            addd.news = "666";
            new BCW.BLL.dawnlifeDays().Add(addd);


            /////////////////////////////////////////////////////////////////
            int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
            BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
            int day = Convert.ToInt16(rday.day);
            int r11 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
            int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
            BCW.Model.dawnlifeDo rw1 = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
            BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r11);
            BCW.Model.dawnlifeDays xx = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r11);
            //成功买入的物品，价钱与数量，库存等显示
            int r6 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
            BCW.Model.dawnlifeDo buy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r6);
            int dr = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
            BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(dr);
            BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(dr);
            if (buy.stocky > 0)
            {
                int n1 = xx.n;
                string[] goodssx = xx.goods.Split(',');
                string[] priceex = xx.price.Split(',');
                int j = Convert.ToInt16(xx.area);
                for (int i = 0; i < n1; i++)
                {
                    string g1 = Convert.ToString(goodssx[i]);
                    string p1 = Convert.ToString(priceex[i]);
                    builder.Append("" + g1 + p1 + "<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=buy&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + i + "") + "\"></a>" + " <br />");

                    string[] googss = rw1.goods.Split(',');
                    string[] dsgs = rw1.dsg.Split(',');
                    //获取","个数，得到多少个物品
                    BCW.Model.dawnlifeDo xxx = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r6);
                    string[] goodss = xxx.goods.Split(',');
                    string[] priceee = xxx.price.Split(',');
                    string[] dsgg = xxx.dsg.Split(',');

                    //获取","个数，得到多少个物品
                    string str = xxx.goods;
                    int s = 0;
                    foreach (char item in str)
                    {
                        if (item != 44)
                        {
                        }
                        else
                        {
                            s++;
                        }
                    }

                    int ptype2 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "0"));
                    for (int i1 = 0; i1 < s; i1++)
                    {
                        string goods = Convert.ToString(goodss[i1 + 1]);
                        string dsg = Convert.ToString(dsgg[i1 + 1]);
                        string price = Convert.ToString(priceee[i1]);
                        if (goods == g1)
                        {
                            long dsg1 = Convert.ToInt64(dsg);
                            long price1 = Convert.ToInt64(p1);
                            re.money = re.money + dsg1 * price1;
                            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

                            if (rw.day < 31)
                            {
                                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                                notes.coin = Convert.ToInt32(ry.coin);
                                notes.UsID = meid;
                                notes.money = re.money - re.debt;
                                notes.debt = 0;
                                notes.city = ptypecity;
                                notes.area = ptype;
                                notes.day = Convert.ToInt32(rw.day + 1);
                                notes.buy = "sell";
                                notes.sell = goods + "/售价" + p1 + "/数量" + dsg;
                                notes.price = 0;
                                notes.num = Convert.ToInt64(ry.stocky);
                                notes.date = DateTime.Now;
                                new BCW.BLL.dawnlifenotes().Add(notes);
                            }
                        }
                    }
                }


                //全部出售存入dawnlifeDo表
                addbuy.UsID = ry.UsID;
                addbuy.UsName = ry.UsName;
                addbuy.sum = ry.sum;
                addbuy.coin = ry.coin;
                addbuy.stock = ry.stock;
                addbuy.stocky = 0;
                addbuy.goods = "";
                addbuy.price = "";
                addbuy.dsg = "";
                new BCW.BLL.dawnlifeDo().Update(addbuy);



                re.money = re.money - re.debt;
                new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);
                if (re.money > re.debt)
                {
                    new BCW.BLL.dawnlifeUser().Updatedebt(r, 0);
                }

            }
            else
            {
                int n1 = xx.n;
                string[] goodssx = xx.goods.Split(',');
                string[] priceex = xx.price.Split(',');
                int j = Convert.ToInt16(xx.area);
                for (int i = 0; i < n1; i++)
                {
                    string g1 = Convert.ToString(goodssx[i]);
                    string p1 = Convert.ToString(priceex[i]);
                    builder.Append("" + g1 + p1 + "<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=buy&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + i + "") + "\"></a>" + " <br />");
                }
                re.money = re.money - re.debt;
                new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);
                if (re.money > re.debt)
                {
                    new BCW.BLL.dawnlifeUser().Updatedebt(r, 0);
                }
                if (rw.day < 31)
                {
                    BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                    notes.coin = Convert.ToInt32(ry.coin);
                    notes.UsID = meid;
                    notes.money = re.money - re.debt;
                    notes.debt = 0;
                    notes.city = ptypecity;
                    notes.area = ptype;
                    notes.day = Convert.ToInt32(rw.day + 1);
                    notes.buy = "buy";
                    notes.sell = "sell";
                    notes.price = 0;
                    notes.num = 0;
                    notes.date = DateTime.Now;
                    new BCW.BLL.dawnlifenotes().Add(notes);
                }

            }
            builder.Append(Out.Tab("<div class=\"class\">", Out.Hr()));

            if (!new BCW.BLL.dawnlifeTop().Existscoin(re.coin))
            {
                int r3 = new BCW.BLL.dawnlifeTop().GetMaxId();
                BCW.Model.dawnlifeTop rtop = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(r3 - 1);
                BCW.Model.dawnlifeTop addtop = new BCW.Model.dawnlifeTop();
                addtop.UsID = re.UsID;
                addtop.UsName = re.UsName;
                addtop.coin = re.coin;
                addtop.city = ptypecity.ToString();
                addtop.date = DateTime.Now;
                addtop.sum = 0;
                addtop.cum = 0;
                addtop.money = re.money;
                new BCW.BLL.dawnlifeTop().Add(addtop);
            }

            day = 0;

            int id = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            string wText = "在[url=/bbs/game/Dawnlife.aspx]" + GameName + "[/url]挣得" + (re.money - re.debt) + "银两" + "";
            new BCW.BLL.Action().Add(1008, id, meid, name, wText);

        }

        if (rw.day >= 31)
        {
            Utils.Success("闯荡", "你的闯荡已经结束，再次去闯荡出一片天地吧", Utils.getUrl("Dawnlife.aspx"), "3");//时间跳转需要1秒
        }

        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {

            Utils.Success("切换地区", "不要着急，正在前往--->", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + ptype + ""), "1");//时间跳转需要1秒
        }
        else
        {
            Utils.Success("切换地区", "不要着急，正在前往--->", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + ptype + ""), "0");//时间跳转需要1秒
        }
    }
    //确认结束游戏
    private void BfendPage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;结束游戏");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        // int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", "0"));
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        // r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("是否确定结束游戏！");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=end&amp;city=" + ptypecity + "") + "\">确认结束游戏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">点错了，继续玩</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (day == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">返回继续闯荡</a>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "返回" + area[j - 1] + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //结束游戏所以数据重置
    private void ENDPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        // 读取dawnlifeUser表的记录的money和用户信息存入dawnlifetop表
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        string name = new BCW.BLL.User().GetUsName(meid);
        long coin = new BCW.BLL.User().GetGold(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeTop().GetMaxId();
        BCW.Model.dawnlifeTop rt = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(r1 - 1);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);

        if (new BCW.BLL.dawnlifeTop().Existscoin(re.coin))
        {
            Utils.Success("结束游戏", "闯荡已经结束", Utils.getUrl("Dawnlife.aspx"), "1");
        }
        else
        {
            BCW.Model.dawnlifeTop addtop = new BCW.Model.dawnlifeTop();
            addtop.UsID = re.UsID;
            addtop.UsName = re.UsName;
            addtop.coin = re.coin;
            addtop.city = ptypecity.ToString();
            addtop.date = DateTime.Now;
            addtop.sum = 0;
            addtop.cum = 0;
            addtop.money = re.money - re.debt;
            new BCW.BLL.dawnlifeTop().Add(addtop);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money - re.debt);
            int r2 = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
            BCW.Model.dawnlifeDays rw = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
            BCW.Model.dawnlifeDays addd = new BCW.Model.dawnlifeDays();
            addd.UsID = meid;
            addd.UsName = name;
            addd.day = 33;
            addd.goods = "goods";
            addd.price = "price";
            addd.city = "city";
            addd.area = "0";
            addd.coin = re.coin;
            addd.n = 10;
            addd.news = "news";
            new BCW.BLL.dawnlifeDays().Add(addd);

            BCW.Model.dawnlifeUser model = new BCW.Model.dawnlifeUser();
            int id = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            string wText = "在[url=/bbs/game/Dawnlife.aspx]" + GameName + "[/url]挣得" + (re.money - re.debt) + "银两" + "";
            new BCW.BLL.Action().Add(1008, id, meid, name, wText);
            if (re.health < 25)
            {
                Utils.Success("结束游戏", "很遗憾，由于你的健康值不足继续游戏，就此结束闯荡，正在结束闯荡", Utils.getUrl("Dawnlife.aspx"), "2");
            }
            else
            {
                Utils.Success("结束游戏", "正在结束闯荡", Utils.getUrl("Dawnlife.aspx"), "1");
            }
        }
    }
    // 买入面页
    private void BuyPage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        //  int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string name = new BCW.BLL.User().GetUsName(meid);
        int r3 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r3);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDo rw = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">" + area[j - 1] + "</a>&gt;购买商品");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "0"));
        //读取地区出现的物品与价钱from  表
        string[] priceee = rt.price.Split(',');
        long price = Convert.ToInt64(priceee[ptype1]);
        string[] goodsss = rt.goods.Split(',');
        string goods = Convert.ToString(goodsss[ptype1]);
        if (rw.stocky < rw.stock)
        {
            string unit = string.Empty;
            if (re.money > price || re.money == price)
            {
                if (goods == "走私汽车") { unit = "辆"; }
                if (goods == "如来神掌") { unit = "本"; }
                if (goods == "假酒") { unit = "瓶"; }
                if (goods == "山寨手机") { unit = "个"; }
                if (goods == "二手笔记本") { unit = "台"; }
                if (goods == "走私香烟") { unit = "条"; }
                if (goods == "三路奶粉") { unit = "袋"; }
                if (goods == "阳澄大闸蟹") { unit = "只"; }
                if (goods == "盗版DVD") { unit = "盘"; }
                if (goods == "名牌A货") { unit = "件"; }

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("库存：" + rw.stocky + "/" + rw.stock + "");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("现金：" + re.money + "银两");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("" + goods + ":" + price + "银两/" + unit + "");
                builder.Append(Out.Tab("</div>", "<br />"));
                long dsg = re.money / price;
                int stocky = Convert.ToInt16(rw.stocky);
                int stock = Convert.ToInt16(rw.stock);
                builder.Append(Out.Tab("<div>", ""));
                if (dsg > (rw.stock - stocky))
                {
                    builder.Append("你最多可购入" + (rw.stock - rw.stocky) + unit + goods + "");
                }
                else
                {
                    builder.Append("你最多可购入" + dsg + unit + goods + "");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你想买多少");
                builder.Append(Out.Tab("</div>", "<br />"));

                if (dsg > (rw.stock - stocky))
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=buyq&amp;city=" + ptypecity + "&amp;ptype=" + ptype1 + "") + "\">购入</a>" + (stock - stocky) + unit);//能购买的全部操作
                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=buyq&amp;city=" + ptypecity + "&amp;ptype=" + ptype1 + "") + "\">购入</a>" + dsg + unit);
                }
                builder.Append(Out.Tab("</div>", ""));

                //输入框
                int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
                string strText = "输入购买数:/,,,,";
                string strName = "uid,act,city,ptype,backurl";
                string strType = "num,hidden,hidden,hidden,hidden";
                string strValu = "'buyqu'" + ptypecity + "'" + ptype1 + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定,Dawnlife.aspx,post,1,red";//?act=buyqu&amp;city=" + ptypecity + "&amp;ptype=" + ptype1 + "
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", Out.Hr()));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你的现金不足，不能购买该商品" + "<br />");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "返回" + area[j - 1] + "</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你的库存不足，不能购买该商品" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "返回" + area[j - 1] + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //全部购买判断
    private void BuyqPage()
    {
        //全部购买存入dawnlifeDo表
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r3 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r3);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        // int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string[] goodsss = rt.goods.Split(',');
        string goods = Convert.ToString(goodsss[ptype1]);
        string[] priceee = rt.price.Split(',');
        int price = Convert.ToInt32(priceee[ptype1]);
        string pricedo = Convert.ToString(priceee[ptype1]);
        long dsg = re.money / price;
        int stocky = Convert.ToInt16(ry.stocky);
        // r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser add = new BCW.Model.dawnlifeUser();
        addbuy.UsID = ry.UsID;
        addbuy.UsName = ry.UsName;
        int j = Convert.ToInt16(rt.area);

        if (ry.stocky >= ry.stock || re.money < price)
        {
            Utils.Success("出错啦", "你的仓库或银两不足，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
        }
        else
        {
            int money = Convert.ToInt32(re.money);
            addbuy.sum = ry.sum;
            addbuy.coin = ry.coin;
            addbuy.stock = ry.stock;
            if (dsg < (ry.stock - stocky))
            {

                addbuy.stocky = ry.stocky + money / price;
                addbuy.goods = ry.goods + "," + goods;
                addbuy.price = ry.price + pricedo + ",";
                addbuy.dsg = ry.dsg + "," + (money / price).ToString();
                add.money = re.money - price * (money / price);
            }
            else
            {
                //int money = Convert.ToInt32(re.money);
                addbuy.stocky = ry.stocky + (ry.stock - stocky);
                addbuy.goods = ry.goods + "," + goods;
                addbuy.price = ry.price + pricedo + ",";
                addbuy.dsg = ry.dsg + "," + (ry.stock - stocky).ToString();
                add.money = re.money - price * Convert.ToInt16((ry.stock - stocky));
            }
            new BCW.BLL.dawnlifeDo().Update(addbuy);
            //全部购买更新dawnlifeUser（钱等的变化）
            new BCW.BLL.dawnlifeUser().Updatemoney(r, add.money);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = add.money;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = goods;
            notes.sell = "sell";
            notes.price = price;
            if (dsg < (ry.stock - stocky))
            {
                notes.num = Convert.ToInt64(money / price);
            }
            else
            {
                notes.num = Convert.ToInt64(ry.stock - stocky);
            }
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);

            Utils.Success("购买成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + ptype1 + ""), "1");
        }
    }
    //买多少判断
    private void BuyquPage()
    {
        //买多少更新dawnlifedo表
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        // r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "0"));
        string[] goodsss = rt.goods.Split(',');
        string goods = Convert.ToString(goodsss[ptype1]);
        string[] priceee = rt.price.Split(',');
        int price = Convert.ToInt32(priceee[ptype1]);
        string pricedo = Convert.ToString(priceee[ptype1]);
        addbuy.UsID = ry.UsID;
        addbuy.UsName = ry.UsName;
        int uid1 = Convert.ToInt32(re.money / price);
        int j = Convert.ToInt16(rt.area);

        if (ry.stocky >= ry.stock || re.money < price)
        {
            Utils.Success("出错啦", "你的仓库或银两不足，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
        }
        else
        {

            if (uid < uid1 + 1 && uid <= (ry.stock - ry.stocky) && uid != 0)
            {
                addbuy.sum = ry.sum;
                addbuy.coin = ry.coin;
                addbuy.stock = ry.stock;
                int money = Convert.ToInt32(re.money);
                addbuy.stocky = ry.stocky + uid;
                addbuy.goods = ry.goods + "," + goods;
                addbuy.price = ry.price + pricedo + ",";
                addbuy.dsg = ry.dsg + "," + uid;
                new BCW.BLL.dawnlifeDo().Update(addbuy);
                //全部购买更新dawnlifeUser（钱等的变化）
                int r_3 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
                BCW.Model.dawnlifeUser add = new BCW.Model.dawnlifeUser();
                add.money = re.money - price * uid;
                new BCW.BLL.dawnlifeUser().Updatemoney(r_3, add.money);

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = add.money;
                notes.debt = re.debt;
                notes.city = Convert.ToInt32(re.city);
                notes.area = Convert.ToInt32(rt.area);
                notes.day = Convert.ToInt32(rt.day);
                notes.buy = goods;
                notes.sell = "sell";
                notes.price = price;
                notes.num = Convert.ToInt64(uid);
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);

                Utils.Success("购买成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + ptype1 + ""), "1");
            }
            else
            {
                Utils.Success("购买失败", "请输入小于库存的量，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "&amp;ptype=" + ptype1 + ""), "2");
            }
        }
    }
    //卖出面页
    private void SellPage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]\d*$", "1"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "0"));
        //int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[0-9]$", "0"));

        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r3 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeDo re = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r3);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);

        string[] dsgg = re.dsg.Split(',');
        if (dsgg[ptype + 1] == "")
        {
            Utils.Success("出错啦", "请不要重复操作，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
        }
        else
        {
            string ai = dsgg[ptype + 1];
            int dsg = int.Parse(ai);
            BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
            string[] area = rg.area.Split('/');
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">" + area[j - 1] + "</a>&gt;卖出商品");
            builder.Append(Out.Tab("</div>", "<br />"));
            // 读取dawnlifedo表的物品与价钱与数量


            string[] goodsss = re.goods.Split(',');
            string goods = Convert.ToString(goodsss[ptype + 1]);
            string unit = string.Empty;

            if (goods == "走私汽车") { unit = "辆"; }
            if (goods == "如来神掌") { unit = "本"; }
            if (goods == "假酒") { unit = "瓶"; }
            if (goods == "山寨手机") { unit = "个"; }
            if (goods == "二手笔记本") { unit = "台"; }
            if (goods == "走私香烟") { unit = "条"; }
            if (goods == "三路奶粉") { unit = "袋"; }
            if (goods == "阳澄大闸蟹") { unit = "只"; }
            if (goods == "盗版DVD") { unit = "盘"; }
            if (goods == "名牌A货") { unit = "件"; }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你有" + dsg + unit + goods + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你想卖出去多少？");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=sellq&amp;city=" + ptypecity + "&amp;ptype=" + ptype + "") + "\">全部卖出</a>" + dsg + unit);
            builder.Append(Out.Tab("</div>", ""));
            // 输入框
            int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
            string strText = "输入出售数:/,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确定,Dawnlife.aspx?act=selld&amp;city=" + ptypecity + "&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    //全部卖出判断
    private void SellqPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]\d*$", "1"));
        //int good = Utils.ParseInt(Utils.GetRequest("good", "all", 1, @"^[0-9]\d*$", "0"));

        //全部出售更新dawnlifeUser（钱等的变化）
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r_1 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r_1);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r_1);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        int j = Convert.ToInt16(rt.area);
        if (ry.stocky == 0)
        {
            Utils.Success("出错啦", "此地没有此商品，请不要重复操作，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
            //Utils.Error("请不要重复操作.", "");
        }
        else
        {
            BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
            string[] area = rg.area.Split('/');
            string a = ry.goods;
            char[] b = { ',' };
            string[] c = a.Split(b);
            string[] arr = c;
            ArrayList al = new ArrayList(arr);
            al.RemoveAt(ptype + 1);
            al.Add(",");
            arr = (string[])al.ToArray(typeof(string));
            string newstr = string.Join(",", (string[])al.ToArray(typeof(string)));

            string a1 = ry.dsg;
            char[] b1 = { ',' };
            string[] c1 = a1.Split(b1);
            string[] arr1 = c1;
            ArrayList al1 = new ArrayList(arr1);
            al1.RemoveAt(ptype + 1);
            al1.Add(",");
            arr1 = (string[])al1.ToArray(typeof(string));
            string newstr1 = string.Join(",", (string[])al1.ToArray(typeof(string)));

            string a2 = ry.price;
            char[] b2 = { ',' };
            string[] c2 = a2.Split(b2);
            string[] arr2 = c2;
            ArrayList al2 = new ArrayList(arr2);
            al2.RemoveAt(ptype);
            al2.Add(",");
            arr2 = (string[])al2.ToArray(typeof(string));
            string newstr2 = string.Join(",", (string[])al2.ToArray(typeof(string)));

            string[] goodss = ry.goods.Split(',');
            string goods = goodss[ptype + 1];
            string[] goodsday = rt.goods.Split(',');
            //获取","个数，得到多少个物品
            string str = rt.goods;
            int s = 0;
            foreach (char item in str)
            {
                if (item != 44)
                {
                }
                else
                {
                    s++;
                }
            }

            for (int i = 0; i < s; i++)
            {
                string daygoods = Convert.ToString(goodsday[i]);
                if (goods == daygoods)
                {
                    builder.Append(goods + "<br />" + "a");
                    builder.Append(daygoods);

                    string[] priceee = ry.price.Split(',');
                    string[] dsgg = ry.dsg.Split(',');
                    //int price = Convert.ToInt16(priceee[ptype2]);
                    int dsg = Convert.ToInt16(dsgg[ptype + 1]);
                    int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
                    BCW.Model.dawnlifeUser add = new BCW.Model.dawnlifeUser();
                    string[] priceo = rt.price.Split(',');
                    int priceout = Convert.ToInt32(priceo[i]);
                    int money = Convert.ToInt32(re.money);
                    add.money = re.money + priceout * dsg;
                    //全部出售存入dawnlifeDo表
                    addbuy.UsID = ry.UsID;
                    addbuy.UsName = ry.UsName;
                    addbuy.sum = ry.sum;
                    addbuy.coin = ry.coin;
                    addbuy.stock = ry.stock;
                    addbuy.stocky = ry.stocky - dsg;
                    addbuy.goods = newstr;
                    addbuy.price = newstr2;
                    addbuy.dsg = newstr1;
                    new BCW.BLL.dawnlifeDo().Update(addbuy);
                    new BCW.BLL.dawnlifeUser().Updatemoney(r, add.money);
                    int reputation = Convert.ToInt16(rc.reputation);

                    BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                    notes.coin = Convert.ToInt32(re.coin);
                    notes.UsID = meid;
                    notes.money = add.money;
                    notes.debt = re.debt;
                    notes.city = Convert.ToInt32(re.city);
                    notes.area = Convert.ToInt32(rt.area);
                    notes.day = Convert.ToInt32(rt.day);
                    notes.buy = "buy";
                    notes.sell = goods;
                    notes.price = priceout;
                    notes.num = Convert.ToInt64(c1[ptype + 1]);
                    notes.date = DateTime.Now;
                    new BCW.BLL.dawnlifenotes().Add(notes);

                    if (goods == "三路奶粉")
                    {
                        new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation - 5);

                        Utils.Success("全部出售成功", "【警告！】贩卖有毒奶粉名声下降5点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                    }
                    if (goods == "假酒")
                    {
                        new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation - 10);
                        Utils.Success("全部出售成功", "【警告！】贩卖假酒致使多人中毒，被警察拘留，名声扣10点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                    }
                    if (goods == "如来神掌")
                    {
                        new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation - 7);
                        Utils.Success("全部出售成功", "【警告！】如来神掌里面包含欲女心经，大量贩卖欲女心经严重影响社会风气，名声下降7点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                    }
                    if (goods == "走私香烟")
                    {
                        new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation - 3);
                        Utils.Success("全部出售成功", "【警告！】香烟解忧排愁，但有损健康，烟还是少抽，大量贩卖走私香烟，名声下降3点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                    }
                    else
                    {
                        Utils.Success("全部出售成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "1");
                    }
                }
                else
                {
                    if (i == s - 1)
                    {
                        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
                        builder.Append(Out.Tab("<div class=\"title\">", ""));
                        //int ptype = int.Parse(Utils.GetRequest("ptype", "act", 1, @"^[1-9]$", "0"));
                        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">" + area[j - 1] + "</a>&gt;卖出商品");
                        builder.Append(Out.Tab("</div>", "<br />"));
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("此地没有此商品！");
                        builder.Append(Out.Tab("</div>", "<br />"));
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">返回>></a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
        }
    }
    //卖出多少判断
    private void SelldPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]\d*$", "1"));
        //int good = Utils.ParseInt(Utils.GetRequest("good", "all", 1, @"^[0-9]\d*$", "0"));
        //多少出售更新dawnlifeUser（钱等的变化）
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r_1 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r_1);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        BCW.Model.dawnlifeDo addbuy = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        int j = Convert.ToInt16(rt.area);
        string[] dsgg = ry.dsg.Split(',');
        if (dsgg[ptype + 1] == "")
        {
            Utils.Success("出错啦", "此地没有此商品，请不要重复操作，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
        }
        else
        {
            string[] priceee = ry.price.Split(',');
            //int price = Convert.ToInt16(priceee[ptype2]);
            string ai = dsgg[ptype + 1];
            int dsg = int.Parse(ai);
            // int dsg = Convert.ToInt32(dsgg[ptype + 1]);
            //  r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
            BCW.Model.dawnlifeUser add = new BCW.Model.dawnlifeUser();
            long money = Convert.ToInt64(re.money);

            BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
            string[] area = rg.area.Split('/');
            string a = ry.goods;
            char[] b = { ',' };
            string[] c = a.Split(b);
            string[] arr = c;
            ArrayList al = new ArrayList(arr);
            al.RemoveAt(ptype + 1);
            arr = (string[])al.ToArray(typeof(string));
            string newstr = string.Join(",", (string[])al.ToArray(typeof(string)));

            string a1 = ry.dsg;
            char[] b1 = { ',' };
            string[] c1 = a1.Split(b1);
            string[] arr1 = c1;
            ArrayList al1 = new ArrayList(arr1);
            al1.RemoveAt(ptype + 1);
            arr1 = (string[])al1.ToArray(typeof(string));
            string newstr1 = string.Join(",", (string[])al1.ToArray(typeof(string)));

            string a2 = ry.price;
            char[] b2 = { ',' };
            string[] c2 = a2.Split(b2);
            string[] arr2 = c2;
            ArrayList al2 = new ArrayList(arr2);
            al2.RemoveAt(ptype);
            arr2 = (string[])al2.ToArray(typeof(string));
            string newstr2 = string.Join(",", (string[])al2.ToArray(typeof(string)));

            string aa1 = ry.dsg;
            //获取","个数，得到多少个物品
            string str = aa1;
            char[] bb1 = { ',' };
            string[] cc1 = aa1.Split(bb1);
            int s = 0;
            foreach (char item in str)
            {
                if (item != 44)
                {
                }
                else
                {
                    s++;
                }
            }
            string[] ss = aa1.Split(',');
            BCW.Model.dawnlifeDo xx = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r1);
            string p1 = Convert.ToString(dsgg[ptype + 1]);
            int ds = Convert.ToInt16(p1);
            string stri1 = (ds - uid).ToString();
            ss[ptype + 1] = stri1;
            aa1 = "";
            for (int i = 1; i <= s; i++)
            {
                aa1 = aa1 + "," + ss[i];
            }
            string[] goodss = ry.goods.Split(',');
            string goods = goodss[ptype + 1];
            string[] goodsday = rt.goods.Split(',');
            //获取","个数，得到多少个物品
            string str1 = rt.goods;
            int s1 = 0;
            foreach (char item in str1)
            {
                if (item != 44)
                {
                }
                else
                {
                    s1++;
                }
            }
            if (uid == 0)
            {
                Utils.Error("出错啦！请输入出售数量", "");
            }
            if (uid <= dsg)
            {
                for (int i = 0; i < s1; i++)
                {
                    string daygoods = Convert.ToString(goodsday[i]);
                    if (goods == daygoods)
                    {
                        string[] priceo = rt.price.Split(',');
                        int priceout = Convert.ToInt32(priceo[i]);
                        //多少出售存入dawnlifeDo表
                        if (uid < dsg)
                        {
                            addbuy.UsID = ry.UsID;
                            addbuy.UsName = ry.UsName;
                            addbuy.sum = ry.sum;
                            addbuy.coin = ry.coin;
                            addbuy.stock = ry.stock;
                            addbuy.stocky = ry.stocky - uid;
                            addbuy.goods = ry.goods;
                            addbuy.price = ry.price;
                            addbuy.dsg = aa1;
                        }
                        else
                        {
                            string aa = ry.goods;
                            char[] bb = { ',' };
                            string[] cc = aa.Split(bb);
                            string[] arrr = cc;
                            ArrayList all = new ArrayList(arrr);
                            all.RemoveAt(ptype + 1);
                            all.Add(",");
                            arrr = (string[])all.ToArray(typeof(string));
                            string newstrr = string.Join(",", (string[])all.ToArray(typeof(string)));

                            string ax1 = ry.dsg;
                            char[] bx1 = { ',' };
                            string[] cx1 = ax1.Split(bx1);
                            string[] arrx1 = cx1;
                            ArrayList alx1 = new ArrayList(arrx1);
                            alx1.RemoveAt(ptype + 1);
                            alx1.Add(",");
                            arrx1 = (string[])alx1.ToArray(typeof(string));
                            string newstrx1 = string.Join(",", (string[])alx1.ToArray(typeof(string)));

                            string ax2 = ry.price;
                            char[] bx2 = { ',' };
                            string[] cx2 = ax2.Split(bx2);
                            string[] arrx2 = cx2;
                            ArrayList alx2 = new ArrayList(arrx2);
                            alx2.RemoveAt(ptype);
                            alx2.Add(",");
                            arrx2 = (string[])alx2.ToArray(typeof(string));
                            string newstrx2 = string.Join(",", (string[])alx2.ToArray(typeof(string)));

                            addbuy.UsID = ry.UsID;
                            addbuy.UsName = ry.UsName;
                            addbuy.sum = ry.sum;
                            addbuy.coin = ry.coin;
                            addbuy.stock = ry.stock;
                            addbuy.stocky = ry.stocky - dsg;
                            addbuy.goods = newstrr;
                            addbuy.price = newstrx2;
                            addbuy.dsg = newstrx1;
                        }
                        new BCW.BLL.dawnlifeDo().Update(addbuy);
                        add.money = re.money + priceout * uid;
                        new BCW.BLL.dawnlifeUser().Updatemoney(r, add.money);

                        BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                        notes.coin = Convert.ToInt32(re.coin);
                        notes.UsID = meid;
                        notes.money = add.money;
                        notes.debt = re.debt;
                        notes.city = Convert.ToInt32(re.city);
                        notes.area = Convert.ToInt32(rt.area);
                        notes.day = Convert.ToInt32(rt.day);
                        notes.buy = "buy";
                        notes.sell = goods;
                        notes.price = priceout;
                        notes.num = Convert.ToInt64(uid);
                        notes.date = DateTime.Now;
                        new BCW.BLL.dawnlifenotes().Add(notes);
                        if (goods == "三路奶粉")
                        {
                            int reputation = Convert.ToInt32(rc.reputation - 5);
                            new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation);
                            Utils.Success("出售成功", "【警告！】贩卖有毒奶粉名声下降5点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                        }
                        if (goods == "假酒")
                        {
                            int reputation = Convert.ToInt32(rc.reputation - 10);
                            new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation);
                            Utils.Success("出售成功", "【警告！】贩卖假酒致使多人中毒，被警察拘留，名声扣10点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                        }
                        if (goods == "如来神掌")
                        {
                            int reputation = Convert.ToInt32(rc.reputation - 7);
                            new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation);
                            Utils.Success("出售成功", "【警告！】如来神掌里面包含欲女心经，大量贩卖欲女心经，严重影响社会风气，名声下降7点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                        }
                        if (goods == "走私香烟")
                        {
                            int reputation = Convert.ToInt32(rc.reputation - 3);
                            new BCW.BLL.dawnlifeUser().Updatereputation(r, reputation);
                            Utils.Success("出售成功", "【警告！】香烟解忧排愁，但有损健康，烟还是少抽，大量贩卖走私香烟，名声下降3点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
                        }
                        else
                        {
                            Utils.Success("出售成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "1");
                        }
                    }
                    else
                    {
                        if (i == s1 - 1)
                        {
                            Master.Title = ub.GetSub("DawnlifeName", xmlPath);
                            builder.Append(Out.Tab("<div class=\"title\">", ""));
                            //int ptype = int.Parse(Utils.GetRequest("ptype", "act", 1, @"^[1-9]$", "0"));
                            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">" + area[j - 1] + "</a>&gt;卖出商品");
                            builder.Append(Out.Tab("</div>", "<br />"));
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("此地没有此商品！");
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">返回>></a>");
                            builder.Append(Out.Tab("</div>", ""));

                        }
                    }
                }
            }
            else
            {
                Utils.Success("出售失败", "请输入小于库存的量，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + ""), "2");
            }
        }
    }
    //还钱界面
    private void ReapyPage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r3 = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r3);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "" + area[j - 1] + "</a>&gt;还钱");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("还给胖大海");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("胖大海说:" + "你欠老子" + re.debt + "银两,啥时候还?!" + " ");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("还他多少？");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 1"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=repayp&amp;city=" + ptypecity + "&amp;ptype=" + ptype) + "\">全部还清</a>");
        builder.Append(Out.Tab("</div>", ""));
        // 输入框
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string strText = "输入金钱:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确定,Dawnlife.aspx?act=repaypd&amp;city=" + ptypecity + "&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">取消</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //还钱全部判断界面
    private void ReapyPPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        //  int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 0"));
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        //  r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        if (re.money > 0)
        {
            if (re.money > re.debt)
            {
                re.money = re.money - re.debt;
                re.debt = 0;
                new BCW.BLL.dawnlifeUser().Updatedebt(r, re.debt);
                new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = re.money;
                notes.debt = 0;
                notes.city = Convert.ToInt32(re.city);
                notes.area = Convert.ToInt32(rt.area);
                notes.day = Convert.ToInt32(rt.day);
                notes.buy = "buyq";
                notes.sell = "sell";
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);

                Utils.Success("还钱成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
            }
            else
            {
                re.debt = re.debt - re.money;
                re.money = 0;
                new BCW.BLL.dawnlifeUser().Updatedebt(r, re.debt);
                new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = 0;
                notes.debt = re.debt;
                notes.city = Convert.ToInt32(re.city);
                notes.area = Convert.ToInt32(rt.area);
                notes.day = Convert.ToInt32(rt.day);
                notes.buy = "buyq";
                notes.sell = "sell";
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
                Utils.Success("还钱成功", "还钱给胖大海" + "<br />" + "你的金钱不足以还清，你只有这些钱了。你还欠胖大海" + re.debt + "银两，继续挣钱>>", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
            }
        }
        else
        {
            Utils.Success("还钱失败", "你的金钱为零，不能够还钱，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
        }
    }
    //还钱多少判断界面
    private void ReapyPdPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        // int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 0"));
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        // r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        if (uid <= re.debt)
        {
            if (re.money > 0 && uid <= re.money)
            {
                re.debt = re.debt - uid;
                re.money = re.money - uid;
                new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);
                new BCW.BLL.dawnlifeUser().Updatedebt(r, re.debt);

                BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
                notes.coin = Convert.ToInt32(re.coin);
                notes.UsID = meid;
                notes.money = re.money;
                notes.debt = re.debt;
                notes.city = Convert.ToInt32(re.city);
                notes.area = Convert.ToInt32(rt.area);
                notes.day = Convert.ToInt32(rt.day);
                notes.buy = "buyq";
                notes.sell = "sell";
                notes.price = 0;
                notes.num = 0;
                notes.date = DateTime.Now;
                new BCW.BLL.dawnlifenotes().Add(notes);
                Utils.Success("还钱成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
            }
            else
            {
                Utils.Success("还钱失败", "你的金钱为零，不能够还钱，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
            }
        }
        else
        { Utils.Success("还钱失败", "请输入小于欠款的金额，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2"); }
    }
    //医院界面
    private void HospitalPage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        // r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "" + area[j - 1] + "</a>&gt;医院");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("医院");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("大夫高兴的暗自偷笑" + "<br />" + "你健康点数为" + re.health + "需要治疗" + (100 - re.health) + "点,坚决抵制腐败!每个健康点数只收你3500银两红包.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("治疗点数");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=hospitalp&amp;city=" + ptypecity + "&amp;ptype=" + ptype) + "\">完全康复</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "输入要治疗点数:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strIdea = "/";
        string strEmpt = "true,false";
        string strOthe = "确定,Dawnlife.aspx?act=hospitalpd&amp;city=" + ptypecity + "&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">取消</a>");
        builder.Append("");
        if (re.health <= 45)
        {
            builder.Append("你的健康存在大大的问题，请及时治疗，当健康值小于45时是不能继续游戏的。。。");
            builder.Append("<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=end&amp;city=" + ptypecity + "") + "\">结束游戏</a>");
        }

        builder.Append(Out.Tab("</div class=\"class\">", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //医院完全康复界面
    private void HospitalPPage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        //  int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 0"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        //r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        int health = 100 - Convert.ToInt16(re.health);
        if (re.money > 3500 * health)
        {
            re.money = re.money - 3500 * health;
            new BCW.BLL.dawnlifeUser().Updatehealth(r, 100);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "yiyuanq";
            notes.sell = "yiyuanq";
            notes.price = 3500 * health;
            notes.num = health;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("治疗成功", "完全康复", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
        else
        {
            Utils.Success("治疗失败", "你的金钱不足以完全康复，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
        }
    }
    //医院确定界面
    private void HospitalPpdage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        //   int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        //r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        int health = Convert.ToInt16(re.health);
        if (re.money > 3500 * uid && uid < (101 - re.health))
        {

            re.money = re.money - 3500 * uid;
            int healthh = health + uid;
            new BCW.BLL.dawnlifeUser().Updatehealth(r, healthh);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "yiyuan";
            notes.sell = "yiyuan";
            notes.price = 3500 * uid;
            notes.num = uid;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("治疗成功", "已经完成治疗", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
        else
        {
            Utils.Success("治疗失败", "你的金钱不足以完全康复或你输入的健康值超出，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
        }
    }
    //捐款界面
    private void DonationlPage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        // r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "" + area[j - 1] + "</a>&gt;慈善捐款");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("红十字会的工作人员热情的握着你的手:" + "行行好吧,非洲的小孩子好几天都没饭吃了," + "每捐助35000银两可以加一点名声");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("加几点名声？");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=donationp&amp;city=" + ptypecity + "&amp;ptype=" + ptype) + "\">全部恢复</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "加几点名声:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确定,Dawnlife.aspx?act=donationpd&amp;city=" + ptypecity + "&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j) + "\">取消</a>");

        builder.Append("");
        if (re.reputation <= 45)
        {
            builder.Append("你的名声有损你的闯荡，请及时恢复，当名声值小于60时是不能继续闯荡的。。。");
            builder.Append("<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=end&amp;city=" + ptypecity + "") + "\">结束闯荡</a>");
        }

        builder.Append(Out.Tab("</div class=\"class\">", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //捐款全部恢复名声界面
    private void DonationlpPage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        // int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        //r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        int reputation = 100 - Convert.ToInt16(re.reputation);
        if (re.money > 35000 * reputation)
        {
            re.money = re.money - 35000 * reputation;
            new BCW.BLL.dawnlifeUser().Updatereputation(r, 100);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "mingshengq";
            notes.sell = "mingshengq";
            notes.price = 35000 * reputation;
            notes.num = reputation;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("捐款成功", "名声完全恢复，人品满点", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
        else
        {
            Utils.Success("捐款失败", "你的金钱不足以完全恢复名声，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
        }
    }
    //捐款确定界面
    private void DonationlpdPage()
    {
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        // int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]$", " 0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        //  r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        int reputation = 100 - Convert.ToInt16(re.reputation);
        if (re.money > 35000 * uid && uid < (101 - re.reputation))
        {
            re.money = re.money - 35000 * uid;
            int reputationn = Convert.ToInt16(re.reputation) + uid;
            new BCW.BLL.dawnlifeUser().Updatereputation(r, reputationn);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "mingsheng";
            notes.sell = "mingsheng";
            notes.price = 35000 * uid;
            notes.num = uid;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("捐款成功", "已经完成名声恢复", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
        }
        else
        {
            Utils.Success("捐款失败", "你的金钱不足以捐款或者健康值超出，正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "2");
        }
    }
    //仓库升级界面
    private void UpgradePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        //  r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        // int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[1-9]$", "1"));
        // int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "1"));
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + area[j - 1] + "</a>&gt;仓库升级");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int stock = Convert.ToInt16(re.stock);
        if (re.money < 20000)
        {
            builder.Append("中介说,你没有20000现金就想来租房?一边凉快去吧!" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "返回" + area[j - 1] + ">>" + "</a>");
            builder.Append("");
        }

        else if (stock == 100 && re.money > 20000)
        {
            builder.Append("欢迎来到[扁子居]租房中介" + "<br />" + "我们的理念:免费看房,成交补款,童叟全欺,无耻无畏!" + "<br />" + "想把生意做大?你现在的仓库只能放100个物品,太小拉!!" + "<br />" + "你花20000银两,就能租放110个物品的仓库!" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=sjck&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">成交！</a>");
            builder.Append("<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">怕上当，再看看</a>");
        }
        else if (stock == 110 && re.money > 30000)
        {
            builder.Append("欢迎来到[扁子居]租房中介" + "<br />" + "我们的理念:免费看房,成交补款,童叟全欺,无耻无畏!" + "<br />" + "想把生意做大?你现在的仓库只能放110个物品,太小拉!!" + "<br />" + "你花30000银两,就能租放120个物品的仓库!" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=sjck&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">成交！</a>");
            builder.Append("<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">怕上当，再看看</a>");
        }
        else if (stock == 120 && re.money > 40000)
        {
            builder.Append("欢迎来到[扁子居]租房中介" + "<br />" + "我们的理念:免费看房,成交补款,童叟全欺,无耻无畏!" + "<br />" + "想把生意做大?你现在的仓库只能放120个物品,太小拉!!" + "<br />" + "你花30000银两,就能租放130个物品的仓库!" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=sjck&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">成交！</a>");
            builder.Append("<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">怕上当，再看看</a>");
        }
        else if (stock == 130 && re.money > 70000)
        {
            builder.Append("欢迎来到[扁子居]租房中介" + "<br />" + "我们的理念:免费看房,成交补款,童叟全欺,无耻无畏!" + "<br />" + "想把生意做大?你现在的仓库只能放130个物品,太小拉!!" + "<br />" + "你花70000银两,就能租放150个物品的仓库!" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=sjck&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">成交！</a>");
            builder.Append("<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">怕上当，再看看</a>");
        }
        else if (stock == 150 && re.money > 0)
        {
            builder.Append("已经没有比江边码头更大的仓库了，再看看吧！" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">返回继续闯荡</a>");
        }
        else
        {
            builder.Append("中介说,你没有足够现金就想来租房?一边凉快去吧!" + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j + "") + "\">" + "返回" + area[j - 1] + ">>" + "</a>");
            builder.Append("");
        }
        builder.Append(Out.Tab("</div class=\"title\">", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //仓库升级成交界面
    private void SjkuPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser rc = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = 0;
        try
        {
            coin = rc.coin;
        }
        catch
        {
            Utils.Error("哎呀，天气太坏，刷新一下心情", " ");
        }
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r2 = new BCW.BLL.dawnlifeDo().GetRowByUsID(meid, coin);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        BCW.Model.dawnlifeDo ry = new BCW.BLL.dawnlifeDo().GetdawnlifeDo(r2);
        int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "all", 1, @"^[1-3]$", "1"));
        // int ptype = int.Parse(Utils.GetRequest("area", "all", 1, @"^[1-9]$", "1"));
        //  int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]$", "1"));
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);
        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        int j = Convert.ToInt16(rt.area);
        BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(ptypecity);
        string[] area = rg.area.Split('/');
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        int stock = Convert.ToInt16(re.stock);
        if (re.money > 20000 && stock == 100)
        {
            re.stock = 110.ToString();
            new BCW.BLL.dawnlifeUser().UpdateStorehouse(r, "水泥平方");
            new BCW.BLL.dawnlifeUser().UpdateStock(r, re.stock);
            new BCW.BLL.dawnlifeDo().UpdateStock(r2, 110);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money - 20000);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money - 20000;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "cksj";
            notes.sell = "cksj";
            notes.price = 20000;
            notes.num = 110;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("仓库升级成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
        else if (re.money > 30000 && stock == 110)
        {
            re.stock = 120.ToString();
            new BCW.BLL.dawnlifeUser().UpdateStorehouse(r, "废弃厂房");
            new BCW.BLL.dawnlifeUser().UpdateStock(r, re.stock);
            new BCW.BLL.dawnlifeDo().UpdateStock(r2, 120);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money - 30000);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money - 30000;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "cksj";
            notes.sell = "cksj";
            notes.price = 30000;
            notes.num = 120;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("仓库升级成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
        else if (re.money > 40000 && stock == 120)
        {
            re.stock = 130.ToString();
            new BCW.BLL.dawnlifeUser().UpdateStorehouse(r, "地下仓库");
            new BCW.BLL.dawnlifeUser().UpdateStock(r, re.stock);
            new BCW.BLL.dawnlifeDo().UpdateStock(r2, 130);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money - 40000);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money - 40000;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "cksj";
            notes.sell = "cksj";
            notes.price = 40000;
            notes.num = 130;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("仓库升级成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
        else if (re.money > 70000 && stock == 130)
        {
            re.stock = 150.ToString();
            new BCW.BLL.dawnlifeUser().UpdateStorehouse(r, "江边码头");
            new BCW.BLL.dawnlifeUser().UpdateStock(r, re.stock);
            new BCW.BLL.dawnlifeDo().UpdateStock(r2, 150);
            new BCW.BLL.dawnlifeUser().Updatemoney(r, re.money - 70000);

            BCW.Model.dawnlifenotes notes = new BCW.Model.dawnlifenotes();
            notes.coin = Convert.ToInt32(re.coin);
            notes.UsID = meid;
            notes.money = re.money - 70000;
            notes.debt = re.debt;
            notes.city = Convert.ToInt32(re.city);
            notes.area = Convert.ToInt32(rt.area);
            notes.day = Convert.ToInt32(rt.day);
            notes.buy = "cksj";
            notes.sell = "cksj";
            notes.price = 70000;
            notes.num = 150;
            notes.date = DateTime.Now;
            new BCW.BLL.dawnlifenotes().Add(notes);
            Utils.Success("仓库升级成功", "正在返回闯荡", Utils.getUrl("Dawnlife.aspx?act=gz&amp;city=" + ptypecity + "&amp;area=" + j), "1");
        }
    }
    //上一日获奖酷友
    private void JiangPage()
    {
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        // int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);

        string Notes = ub.GetSub("DawnlifeNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }


        //  int ptypecity = Utils.ParseInt(Utils.GetRequest("city", "get", 1, @"^[0-3]$", "1"));
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + GameName + "<img src=\"" + Logo + "\"/></a>&gt;上日获奖名单");

        //builder.Append("<img src=\"" + " img/Dawnlife_img/audi.jpg" + "\" height=\"50\" weight=\"60\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        long coin = new BCW.BLL.User().GetGold(meid);
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + meid + "") + "\">" + name + "(" + meid + ")</a>");
        //builder.Append("：" + coin + ""+ub.Get("SiteBz")+"");
        //builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(DateTime.Now.AddDays(-1).ToString("【MM月dd日获奖酷友】") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        DataSet ds = new BCW.BLL.dawnlifeTop().GetList("Top 1 UsID,money", "DateDiff(day,date,getdate())=1 Order by money Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("全国闯王：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "(" + ds.Tables[0].Rows[0]["UsID"] + ")" + "</a>" + "" + "");
        }
        else
            builder.Append("全国闯王：暂无记录");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=jiang&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;ptypet=4&amp;id=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=jiang&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;ptypet=1&amp;id=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");

        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=jiang&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;ptypet=2&amp;id=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=jiang&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;ptypet=3&amp;id=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件
        if (ptypet == 1)
        {
            strWhere = "DateDiff(day,date,getdate())=1 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypet == 2)
        {
            strWhere = "DateDiff(day,date,getdate())=1 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypet == 3)
        {
            strWhere = "DateDiff(day,date,getdate())=1 and city='3'";
            strOrder = "money Desc";
        }
        else if (ptypet == 4)
            strWhere = "DateDiff(day,date,getdate())=1";
        strOrder = "money Desc";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;
            int j = 1;
            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
            {
                if (j <= 5)
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
                    int d = (pageIndex - 1) * 10 + k;
                    string sText = string.Empty;
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两";
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=0&amp;id=" + ptypet + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    ;
                }
                j++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        builder.Append(Out.Tab("</div class=\"class\">", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看上一日获奖的奖励情况请前去获奖酷友处查看...");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=award") + "\">>>获奖酷友&lt;&lt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + ub.GetSub("DawnlifeName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    //富人排行榜
    private void TopPage()
    {
        Master.Title = ub.GetSub("DawnlifeName", xmlPath);
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + ub.GetSub("DawnlifeName", xmlPath) + "<img src=\"" + Logo + "\"  alt=\"load\"/></a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        int iSCounts = 0;
        builder.Append("【富人排行榜】");
        builder.Append("<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptyped == 4)
            builder.Append("<b style=\"color:black\">总" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptyped=4&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">总</a>" + "|");
        if (ptyped == 1)
            builder.Append("<b style=\"color:black\">日" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptyped=1&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">日</a>" + "|");
        if (ptyped == 2)
            builder.Append("<b style=\"color:black\">周" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptyped=2&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">周</a>" + "|");
        if (ptyped == 3)
            builder.Append("<b style=\"color:black\">月" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptyped=3&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">月</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypec == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptypec=4&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypec == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptypec=1&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");
        if (ptypec == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptypec=2&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypec == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptypec=3&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        //查询条件
        if (ptypec == 1 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 1 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 1 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 1 && ptyped == 4)
        {
            strWhere = "city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 4)
        {
            strWhere = "city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0 and city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 and city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 and city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 4)
        {
            strWhere = "city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 ";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 ";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 4)
        {
            strWhere = "";
            strOrder = "money Desc ";
        }
        iSCounts = 50;
        string[] pageValUrl = { "act", "ptyped", "ptypec", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops1(pageIndex, 10, strWhere, strOrder, iSCounts, out recordCount);

        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + "赚得" + n.money + "银两";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=top&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">返回" + ub.GetSub("DawnlifeName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //随机函数
    protected int R(int x, int y)
    {
        Random ran = new Random();
        int RandKey = ran.Next(x, y);
        return RandKey;
    }
    //存入dawnlifeDo表
    protected void Addall(BCW.Model.dawnlifeDo add)
    {
        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);
        int rd = new BCW.BLL.dawnlifeDays().GetDayByUsID(meid);
        BCW.Model.dawnlifeDays rday = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(rd);
        int day = Convert.ToInt16(rday.day);
        int r = new BCW.BLL.dawnlifeUser().GetRowByUsID(meid);
        BCW.Model.dawnlifeUser re = new BCW.BLL.dawnlifeUser().GetdawnlifeUser(r);
        long coin = re.coin;
        int r1 = new BCW.BLL.dawnlifeDays().GetRowByUsID(meid, day, coin);

        BCW.Model.dawnlifeDays rt = new BCW.BLL.dawnlifeDays().GetdawnlifeDays(r1);
        BCW.Model.dawnlifeDo addbuy = new BCW.Model.dawnlifeDo();
        addbuy.UsID = meid;
        addbuy.UsName = name;
        int sum = 1;
        addbuy.sum = sum;
        addbuy.coin = re.coin;
        addbuy.stock = 100;
        addbuy.stocky = 0;
        addbuy.goods = "";
        addbuy.price = "";
        addbuy.dsg = "";
        new BCW.BLL.dawnlifeDo().Add(addbuy);
    }
}