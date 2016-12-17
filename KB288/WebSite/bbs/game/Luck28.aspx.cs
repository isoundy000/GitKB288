using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.Data;
/// <summary>
/// 陈志基 16/8/1
/// 修改兑奖防刷
/// 
/// 姚志光  20160914
/// 重添活跃抽奖入口
/// 
/// 邵广林 20161025
/// 修改下注动态1020
/// 修改倒计时js
/// 
/// 邵广林 20161111
/// 修改赔率浮动算法
/// 增加实时赔率到消费记录
/// </summary>

public partial class bbs_game_Luck28 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/luck28.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    public static int change = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        int maxid = new BCW.BLL.Game.Lucklist().GetCount();//统计数据
                                                           //  builder.Append(maxid);
        if (maxid > 0)
        {
            //维护提示
            if (ub.GetSub("Luck28Status", xmlPath) == "1")
            {
                Utils.Safe("此游戏");
            }
            if (ub.GetSub("Luck28Status", xmlPath) == "2")//内测
            {
                int meid = new BCW.User.Users().GetUsId();
                if (meid == 0)
                    Utils.Login();
                string Luck28TestID = ub.GetSub("Luck28TestID", xmlPath);
                string[] sNum = Regex.Split(Luck28TestID, "#");
                int sbsy = 0;
                for (int a = 0; a < sNum.Length; a++)
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
            string act = Utils.GetRequest("act", "all", 1, "", "");
            switch (act)
            {
                case "case":
                    CasePage();
                    break;
                case "caseok":
                    CaseOkPage();
                    break;
                case "casepost":
                    CasePostPage();
                    break;
                case "pay":
                    PayPage();
                    break;
                case "pay2":
                    Pay2Page();
                    break;
                case "mychoose":
                    mychoose();
                    break;
                case "payok":
                    PayOkPage();
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
                case "bjkl":
                    BjklList();
                    break;
                case "top":
                    TopPage();
                    break;
                case "help":
                    HelpPage();
                    break;
                case "help2":
                    Help2Page();
                    break;
                case "goto":
                    GotoPage();
                    break;
                default:
                    ReloadPage();
                    break;
            }
        }
        else
        {
            Utils.Safe("此游戏");
        }
    }



    private void ReloadPage()
    {
        Master.Title = ub.GetSub("Luck28Name", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("Luck28Logo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;二八");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Notes = ub.GetSub("Luck28Notes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        // if (IsOpen() == true)
        {
            UpdateState11();
            BCW.Model.Game.Lucklist luck = null;
            luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();
            builder.Append(Out.Tab("<div>", ""));
            if (DateTime.Now < luck.EndTime)
            {
                string luck28 = new BCW.JS.somejs().newDaojishi("luck28", luck.EndTime);
                builder.Append("距" + luck.Bjkl8Qihao + "期投注还有<b style=\"color:red\">" + luck28 + "</b>截止");
                //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                //{
                //    string luck28 = new BCW.JS.somejs().daojishi2("luck28", luck.EndTime);
                //    builder.Append("距" + luck.Bjkl8Qihao + "期投注还有<b style=\"color:red\">" + luck28 + "</b>截止");
                //}
                //else
                //{
                //    string luck28 = new BCW.JS.somejs().daojishi("luck28", luck.EndTime);
                //    builder.Append("距" + luck.Bjkl8Qihao + "期投注还有<b style=\"color:red\">" + luck28 + "</b>截止");
                //}
            }
            // builder.Append("<br />┗奖池:" + new BCW.BLL.Game.Lucklist().GetLucklist(104490).Bjkl8Qihao + "");

            int pCount = Convert.ToInt32(BCW.Data.SqlHelper.GetSingle("Select Count(ID) from tb_Luckpay where LuckId=" + luck.ID + ""));
            builder.Append("<br />┗本期投注人数:" + pCount + "人<a href=\"" + Utils.getUrl("luck28.aspx") + "\">刷新</a><br />");
            builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            //if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th") || Utils.GetTopDomain().Contains("0.6"))
            //{
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=goto") + "\">快速下注</a>.");

            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=pay") + "\">自选</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=goto&amp;ptype=1") + "\">半数</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=goto&amp;ptype=2") + "\">大小</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=goto&amp;ptype=3") + "\">单双</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=goto&amp;ptype=4") + "\">分段</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=pay&amp;ptype=1") + "\">尾数</a>");

            builder.Append(Out.Tab("</div>", ""));

            //}
            //else
            //{
            //    builder.Append(Out.Tab("<div>", "<br />"));
            //    builder.Append("〓<a href=\"" + Utils.getUrl("luck28.aspx?act=pay") + "\">我要投注</a>〓");
            //    builder.Append(Out.Tab("</div>", ""));
            //}
        }
        //else
        //{
        //    builder.Append(Out.Tab("<div class=\"text\">", ""));
        //    builder.Append("幸运28游戏");
        //    builder.Append(Out.Tab("</div>", "<br />"));
        //    builder.Append(Out.Tab("<div>", ""));
        //    builder.Append("游戏开放时间:" + ub.GetSub("Luck28OnTime", xmlPath) + "");
        //    builder.Append("<br />目前还没到开放时间哦!");
        //    builder.Append("<br /><a href=\"" + Utils.getUrl("luck28.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">历史开奖</a> ");
        //    builder.Append(Out.Tab("</div>", ""));
        //}
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=case") + "\">兑奖</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=1") + "\">记录</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=help") + "\">玩法</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("往期开奖");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=bjkl") + "\">【北京快乐8期数】</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        IList<BCW.Model.Game.Lucklist> listLucklist = new BCW.BLL.Game.Lucklist().GetLucklists(3, "State=1");
        if (listLucklist.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            int k = 1;
            foreach (BCW.Model.Game.Lucklist n in listLucklist)
            {

                builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=listview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">第" + n.Bjkl8Qihao + "期开出:" + n.SumNum + "=" + n.PostNum + "</a><br />");

                k++;
            }
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看历史开奖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(6, "luck28.aspx", 5, 0)));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("〓玩家动态〓");
        builder.Append(Out.Tab("</div>", ""));

        // 开始读取动态列表
        int SizeNum = 3;
        string strWhere = "";
        strWhere = "Types=1020";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                //      ForNotes = ForNotes.Replace("幸运28", "");
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=1020&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        //游戏底部Ubb
        string Foot = ub.GetSub("Luck28Foot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void BjklList()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            //   Utils.Login();
            Master.Title = "北京快乐8期数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("luck28.aspx") + "\">幸运28</a>&gt;北京快乐8期数");
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
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("前为北京快乐8开奖号码，后为幸运二八开奖号码");
        // builder.Append("&gt;&gt;官网查询:<a href=\"http://www.bwlc.net/bulletin/keno.html\" target=\"_blank\"\">北京福彩网</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        // 开始读取列表
        IList<BCW.Model.Game.Lucklist> listLucklist = new BCW.BLL.Game.Lucklist().GetLucklists(pageIndex, pageSize, strWhere, out recordCount);
        if (listLucklist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Lucklist n in listLucklist)
            {
                if (n.Bjkl8Nums != "0")
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
                    builder.Append("快乐8第" + n.Bjkl8Qihao + "期号码:" + n.Bjkl8Nums + "=二八开奖:" + n.SumNum + "=" + n.PostNum + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("一·北京快乐8每期开奖共开出20个数字,幸运28将这20个开奖号码按照由小到大的顺序依次排列。<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其1-6位开奖号码相加,和值的末位数作为幸运28开奖第一个数值<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其7-12位开奖号码相加,和值的末位数作为幸运28开奖第二个数值<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其13-18位开奖号码相加,和值的末位数作为幸运28开奖第三个数值<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;三个数值相加即为第三方28最终的开奖结果<br/>");
        builder.Append("二·如下图所示。<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/files/face/bjkl8.png\" width=\"500\" height=\"250\"  alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    private void GotoPage()
    {
        //if (IsOpen() == false)
        //{
        //    Utils.Error("游戏开放时间:" + ub.GetSub("Luck28OnTime", xmlPath) + "", "");
        //}
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));

        long Luck28MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//最大奖池
        string FloatBig = ub.GetSub("FloatBig", xmlPath);
        string FloatSmall = ub.GetSub("FloatSmall", xmlPath);
        string FloatSingle = ub.GetSub("FloatSingle", xmlPath);
        string FloatDouble = ub.GetSub("FloatDouble", xmlPath);
        #region  得到XML的赔率
        string Luck28Big = ub.GetSub("Luck28Big", xmlPath);
        string Luck28Small = ub.GetSub("Luck28Small", xmlPath);
        string Luck28Single = ub.GetSub("Luck28Single", xmlPath);
        string Luck28Double = ub.GetSub("Luck28Double", xmlPath);
        string Luck28BigSingle = ub.GetSub("Luck28BigSingle", xmlPath);
        string Luck28SmallSingle = ub.GetSub("Luck28SmallSingle", xmlPath);
        string Luck28BigDouble = ub.GetSub("Luck28BigDouble", xmlPath);
        string Luck28SmallDouble = ub.GetSub("Luck28SmallDouble", xmlPath);
        string Luck28First = ub.GetSub("Luck28First", xmlPath);
        string Luck28Secend = ub.GetSub("Luck28Secend", xmlPath);
        string Luck28Three = ub.GetSub("Luck28Three", xmlPath);
        string Luck28All = ub.GetSub("Luck28All", xmlPath);
        #endregion

        #region   得到还可以下多少金币
        long Luck28BigCents = Utils.ParseInt(ub.GetSub("Luck28BigCents", xmlPath));//半数浮动设置下注额
        long Luck28SmallCents = Utils.ParseInt(ub.GetSub("Luck28SmallCents", xmlPath));
        long Luck28SingleCents = Utils.ParseInt(ub.GetSub("Luck28SingleCents", xmlPath));
        long Luck28DoubleCents = Utils.ParseInt(ub.GetSub("Luck28DoubleCents", xmlPath));
        long Luck28BigSingleCents = Utils.ParseInt(ub.GetSub("Luck28BigSingleCents", xmlPath));
        long Luck28SmallSingleCents = Utils.ParseInt(ub.GetSub("Luck28SmallSingleCents", xmlPath));
        long Luck28BigDoubleCents = Utils.ParseInt(ub.GetSub("Luck28BigDoubleCents", xmlPath));
        long Luck28SmallDoubleCents = Utils.ParseInt(ub.GetSub("Luck28SmallDoubleCents", xmlPath));

        long Luck28FirstCents = Utils.ParseInt(ub.GetSub("Luck28FirstCents", xmlPath));//分段每个段设置下注额
        long Luck28SecendCents = Utils.ParseInt(ub.GetSub("Luck28SecendCents", xmlPath));
        long Luck28ThreeCents = Utils.ParseInt(ub.GetSub("Luck28ThreeCents", xmlPath));
        long Luck28AllCents = Utils.ParseInt(ub.GetSub("Luck28AllCents", xmlPath));


        string Luck28Last0Cents = ub.GetSub("Luck28Last0Cents", xmlPath);//尾数设置下注额
        string Luck28Last1Cents = ub.GetSub("Luck28Last1Cents", xmlPath);
        string Luck28Last2Cents = ub.GetSub("Luck28Last2Cents", xmlPath);
        string Luck28Last3Cents = ub.GetSub("Luck28Last3Cents", xmlPath);
        string Luck28Last4Cents = ub.GetSub("Luck28Last4Cents", xmlPath);
        string Luck28Last5Cents = ub.GetSub("Luck28Last5Cents", xmlPath);
        string Luck28Last6Cents = ub.GetSub("Luck28Last6Cents", xmlPath);
        string Luck28Last7Cents = ub.GetSub("Luck28Last7Cents", xmlPath);
        string Luck28Last8Cents = ub.GetSub("Luck28Last8Cents", xmlPath);
        string Luck28Last9Cents = ub.GetSub("Luck28Last9Cents", xmlPath);

        string Luck28AllPoolCents = ub.GetSub("Luck28AllPoolCents", xmlPath);//整个游戏总下注总额设置


        #endregion
        UpdateState11();
        BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();

        Master.Title = "第" + luck.Bjkl8Qihao + "期下注";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("luck28.aspx") + "\">幸运28</a>&gt;快速投注");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("当前奖期:第" + luck.Bjkl8Qihao + "期<a href=\"" + Utils.getUrl("luck28.aspx?act=goto&amp;ptype=" + ptype + "") + "\">刷新</a><br />");
        string Notes = ub.GetSub("Luck28Notes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.SysUBB(Notes) + "<br />");
        }
        long gold = new BCW.BLL.User().GetGold(meid);
        if (DateTime.Now < luck.EndTime)
        {
            string luck28 = new BCW.JS.somejs().newDaojishi("luck28", luck.EndTime);
            builder.Append("距" + luck.Bjkl8Qihao + "期投注还有" + luck28 + "截止");
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //{
            //    string luck28 = new BCW.JS.somejs().daojishi2("luck28", luck.EndTime);
            //    builder.Append("距" + luck.Bjkl8Qihao + "期投注还有" + luck28 + "截止");
            //}
            //else
            //{
            //    string luck28 = new BCW.JS.somejs().daojishi("luck28", luck.EndTime);
            //    builder.Append("距" + luck.Bjkl8Qihao + "期投注还有" + luck28 + "截止");
            //}
        }
        // builder.Append("<br />当期奖池:" + Utils.ConvertGold(luck.Pool) + "" + ub.Get("SiteBz") + "");
        builder.Append("<br />┗推荐幸运数字:" + new Random().Next(0, 28) + "");
        builder.Append(Out.Tab("<br />", Out.Hr()));
        builder.Append("您目前共有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">充值</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("【半数投注】");
        else if (ptype == 2)
            builder.Append("【大小投注】");
        else if (ptype == 3)
            builder.Append("【单双投注】");
        else if (ptype == 4)
            builder.Append("【分段投注】");
        else
            builder.Append("【快速投注】");

        long gg = new BCW.BLL.Game.Luckpay().GetAllBuyCent(luck.ID);
        //  builder.Append("<br/>奖池还可以下<b>" + (Luck28MaxPool - gg) + "</b>" + ub.Get("SiteBz") + "<br/>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=goto&amp;faslInput=" + 200000 + "&amp;ptype="+ptype+"") + "\" >200000|</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=goto&amp;faslInput=" + 300000 + "&amp;ptype=" + ptype + "") + "\" >300000|</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=goto&amp;faslInput=" + 400000 + "&amp;ptype=" + ptype + "") + "\" >400000|</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=goto&amp;faslInput=" + 500000 + "&amp;ptype=" + ptype + "") + "\" >500000|</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=goto&amp;faslInput=" + 600000 + "&amp;ptype=" + ptype + "") + "\" >600000</a>");
        //builder.Append(Out.Tab("</div>", "<br />"));
        long mrCent = 100;
        string faslInput = Utils.GetRequest("faslInput", "all", 3, @"^[0-9]\d+$", "");
        if (!string.IsNullOrEmpty(faslInput))
        {
            mrCent = long.Parse(faslInput);
        }
        if (ptype == 2 || ptype == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("押大赔率<b>:" + FloatBig + "</b>倍|押小赔率<b>:" + FloatSmall + "</b>倍");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'1'" + FloatBig + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押大,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));

            builder.Append("|");
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'2'" + FloatSmall + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押小,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 3 || ptype == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("押单赔率<b>:" + FloatSingle + "</b>倍|押双赔率<b>:" + FloatDouble + "</b>倍");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'3'" + FloatSingle + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押单,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));

            builder.Append("|");
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'4'" + FloatDouble + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "押双,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 1 || ptype == 0)
        {

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("大单赔率<b>:" + Luck28BigSingle + "</b>倍|小单赔率<b>:" + Luck28SmallSingle + "</b>倍");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'5'" + Luck28BigSingle + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "大单,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));

            builder.Append("|");
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'6'" + Luck28SmallSingle + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "小单,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("大双赔率<b>:" + Luck28BigDouble + "</b>倍|小双赔率<b>:" + Luck28SmallDouble + "</b>倍");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'7'" + Luck28BigDouble + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "大双,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));

            builder.Append("|");
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'8'" + Luck28SmallDouble + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "小双,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 4 || ptype == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("一段赔率<b>:" + Luck28First + "</b>倍|二段赔率<b>:" + Luck28Secend + "</b>倍");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'9'" + Luck28First + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "一段,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));

            builder.Append("|");
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'10'" + Luck28Secend + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "二段,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));

            builder.Append(Out.Tab("</div>", "<br />"));


            builder.Append(Out.Tab("<div>", ""));
            builder.Append("三段赔率<b>:" + Luck28Three + "</b>倍");

            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            strText = ",,,,";
            strName = "BuyCent,ptype,odds,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + mrCent + "'11'" + Luck28Three + "'pay2'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "三段,luck28.aspx,post,4,other";
            builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; "));


            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">再看看吧</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Pay2Page()
    {
        //if (IsOpen() == false)
        //{
        //    Utils.Error("游戏开放时间:" + ub.GetSub("Luck28OnTime", xmlPath) + "", "");
        //}
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "post", 2, @"^[1-9]\d*$", "类型错误"));
        BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();

        Master.Title = "第" + luck.Bjkl8Qihao + "期下注";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("luck28.aspx") + "\">幸运28</a>&gt;投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        long MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//每期下注总额
        #region XML
        long Luck28BigCents = Utils.ParseInt(ub.GetSub("Luck28BigCents", xmlPath));//半数浮动设置下注额
        long Luck28SmallCents = Utils.ParseInt(ub.GetSub("Luck28SmallCents", xmlPath));

        long Luck28SingleCents = Utils.ParseInt(ub.GetSub("Luck28SingleCents", xmlPath));
        long Luck28DoubleCents = Utils.ParseInt(ub.GetSub("Luck28DoubleCents", xmlPath));

        long Luck28BigSingleCents = Utils.ParseInt(ub.GetSub("Luck28BigSingleCents", xmlPath));
        long Luck28SmallSingleCents = Utils.ParseInt(ub.GetSub("Luck28SmallSingleCents", xmlPath));

        long Luck28BigDoubleCents = Utils.ParseInt(ub.GetSub("Luck28BigDoubleCents", xmlPath));
        long Luck28SmallDoubleCents = Utils.ParseInt(ub.GetSub("Luck28SmallDoubleCents", xmlPath));

        long Luck28FirstCents = Utils.ParseInt(ub.GetSub("Luck28FirstCents", xmlPath));//分段每个段设置下注额
        long Luck28SecendCents = Utils.ParseInt(ub.GetSub("Luck28SecendCents", xmlPath));

        long Luck28ThreeCents = Utils.ParseInt(ub.GetSub("Luck28ThreeCents", xmlPath));
        long Luck28AllCents = Utils.ParseInt(ub.GetSub("Luck28AllCents", xmlPath));


        //数据库下注数据
        long Big = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Big");
        long small = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Small");

        long single = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Single");
        long doubl = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Double");

        long bigsingle = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28BigSingle");
        long smallsingle = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28SmallSingle");

        long bigdouble = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28BigDouble");
        long smalldouble = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28SmallDouble");

        long first = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28First");
        long secend = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Secend");

        long three = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Three");
        long all = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28All");


        #endregion
        long hadbuy = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + luck.ID);//得到某一期所有玩家的下注金额
        long BuyCent = Int64.Parse(Utils.GetRequest("BuyCent", "post", 4, @"^[0-9]\d*$", "下注金额填写出错"));
        string BuyNum = string.Empty;
        string Text = string.Empty;
        string EngText = string.Empty;
        long wanjia = 0;
        string kb = ub.Get("SiteBz");
        string odds = "";
        odds = Utils.GetRequest("odds", "post", 1, "", "");
        if (ptype == 1)
        {
            BuyNum = "14-27";
            Text = "大";
            EngText = "Luck28Big";
            wanjia = BuyCent;
            if ((wanjia) > Luck28BigCents - Big + small)
            {
                Utils.Error("" + Text + "还可下注" + (Luck28BigCents - Big + small) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            long big = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Big");
            long small1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Small");
            //builder.Append("luck.ID" + (luck.ID) + "<br/>");
            //builder.Append("Luck28BigCents - big + small1="+(big)+"<br/>");
            //builder.Append("Luck28SmallCents + big - small1=" + (Luck28SmallCents + big - small1) + "<br/>");
            if ((Luck28BigCents - big + small1) == 0 && (Luck28SmallCents + big - small1) != 0)
            {
                builder.Append("押大可下" + ub.Get("SiteBz") + "已达最大|押小可下<b>:" + (Luck28SmallCents + big - small1) + "</b>" + ub.Get("SiteBz"));
            }
            else if ((Luck28BigCents - big + small1) != 0 && (Luck28SmallCents + big - small1) == 0)
            {
                builder.Append("押大可下<b>:" + (Luck28BigCents - big + small1) + "</b>" + ub.Get("SiteBz") + "|押小可下" + ub.Get("SiteBz") + "已达最大");
            }
            else if ((Luck28BigCents - big + small1) != 0 && (Luck28SmallCents + big - small1) != 0)
            {
                builder.Append("押大可下<b>:" + (Luck28BigCents - big + small1) + "</b>" + ub.Get("SiteBz") + "|押小可下<b>:" + (Luck28SmallCents + big - small1) + "</b>" + ub.Get("SiteBz"));
            }
            else if ((Luck28BigCents - big + small1) == 0 && (Luck28SmallCents + big - small1) == 0)
            {
                builder.Append("押大可下" + ub.Get("SiteBz") + "已达最大|押小可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 2)
        {
            BuyNum = "0-13";
            Text = "小";
            EngText = "Luck28Small";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28SmallCents - small + Big))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28SmallCents - small + Big) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            long big = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Big");
            long small1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Small");
            if ((Luck28BigCents - big + small1) == 0 && (Luck28SmallCents + big - small1) != 0)
            {
                builder.Append("押大可下" + ub.Get("SiteBz") + "已达最大|押小可下<b>:" + (Luck28SmallCents + big - small1) + "</b>" + ub.Get("SiteBz"));
            }
            else if ((Luck28BigCents - big + small1) != 0 && (Luck28SmallCents + big - small1) == 0)
            {
                builder.Append("押大可下<b>:" + (Luck28BigCents - big + small1) + "</b>" + ub.Get("SiteBz") + "|押小可下" + ub.Get("SiteBz") + "已达最大");
            }
            else if ((Luck28BigCents - big + small1) != 0 && (Luck28SmallCents + big - small1) != 0)
            {
                builder.Append("押大可下<b>:" + (Luck28BigCents - big + small1) + "</b>" + ub.Get("SiteBz") + "|押小可下<b>:" + (Luck28SmallCents + big - small1) + "</b>" + ub.Get("SiteBz"));
            }
            else if ((Luck28BigCents - big + small1) == 0 && (Luck28SmallCents + big - small1) == 0)
            {
                builder.Append("押大可下" + ub.Get("SiteBz") + "已达最大|押小可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 3)
        {
            BuyNum = "1,3,5,7,9,11,13,15,17,19,21,23,25,27";
            Text = "单";
            EngText = "Luck28Single";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28SingleCents - single + doubl))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28SingleCents - single + doubl) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            long single1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Single");
            long doubl1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Double");
            if (Luck28SingleCents - single1 + doubl1 == 0 && Luck28DoubleCents - doubl1 + single1 != 0)
            {
                builder.Append("押单可下" + ub.Get("SiteBz") + "已达最大|押双可下<b>:" + (Luck28DoubleCents - doubl1 + single1) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28SingleCents - single1 + doubl1 != 0 && Luck28DoubleCents - doubl1 + single1 == 0)
            {
                builder.Append("押单可下<b>:" + (Luck28SingleCents - single1 + doubl1) + "</b>" + ub.Get("SiteBz") + "|押双可下" + ub.Get("SiteBz") + "已达最大");
            }
            else if (Luck28SingleCents - single1 + doubl1 != 0 && Luck28DoubleCents - doubl1 + single1 != 0)
            {
                builder.Append("押单可下<b>:" + (Luck28SingleCents - single1 + doubl1) + "</b>" + ub.Get("SiteBz") + "|押双可下<b>:" + (Luck28DoubleCents - doubl1 + single1) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28SingleCents - single1 == 0 && Luck28DoubleCents - doubl1 == 0)
            {
                builder.Append("<押单可下" + ub.Get("SiteBz") + "已达最大|押双可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        else if (ptype == 4)
        {
            BuyNum = "0,2,4,6,8,10,12,14,16,18,20,22,24,26";
            Text = "双";
            EngText = "Luck28Double";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28DoubleCents - doubl + single))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28DoubleCents - doubl + single) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            long single1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Single");
            long doubl1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Double");
            if (Luck28SingleCents - single1 + doubl1 == 0 && Luck28DoubleCents - doubl1 + single1 != 0)
            {
                builder.Append("<br/>押单可下" + ub.Get("SiteBz") + "已达最大|押双可下<b>:" + (Luck28DoubleCents - doubl1 + single1) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28SingleCents - single1 + doubl1 != 0 && Luck28DoubleCents - doubl1 + single1 == 0)
            {
                builder.Append("<br/>押单可下<b>:" + (Luck28SingleCents - single1 + doubl1) + "</b>" + ub.Get("SiteBz") + "|押双可下" + ub.Get("SiteBz") + "已达最大");
            }
            else if (Luck28SingleCents - single1 + doubl1 != 0 && Luck28DoubleCents - doubl1 + single1 != 0)
            {
                builder.Append("<br/>押单可下<b>:" + (Luck28SingleCents - single1 + doubl1) + "</b>" + ub.Get("SiteBz") + "|押双可下<b>:" + (Luck28DoubleCents - doubl1 + single1) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28SingleCents - single1 == 0 && Luck28DoubleCents - doubl1 == 0)
            {
                builder.Append("<br/>押单可下" + ub.Get("SiteBz") + "已达最大|押双可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 5)
        {
            BuyNum = "15,17,19,21,23,25,27";
            Text = "大单";
            EngText = "Luck28BigSingle";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28BigSingleCents - bigsingle + smallsingle))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28BigSingleCents - bigsingle + smallsingle) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28BigSingleCents - bigsingle + smallsingle == 0 && Luck28SmallSingleCents - smallsingle + bigsingle != 0)
            {
                builder.Append("押大单可下" + ub.Get("SiteBz") + "已达最大|押小单可下<b>:" + (Luck28SmallSingleCents - smallsingle + bigsingle) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28BigSingleCents - bigsingle + smallsingle != 0 && Luck28SmallSingleCents - smallsingle + bigsingle == 0)
            {
                builder.Append("押大单可下<b>:" + (Luck28BigSingleCents - bigsingle + smallsingle) + "</b>" + ub.Get("SiteBz") + "|押小单可下" + ub.Get("SiteBz") + "已达最大");
            }
            else if (Luck28BigSingleCents - bigsingle + smallsingle != 0 && Luck28SmallSingleCents - smallsingle + bigsingle != 0)
            {
                builder.Append("押大单可下<b>:" + (Luck28BigSingleCents - bigsingle + smallsingle) + "</b>" + ub.Get("SiteBz") + "|押小单可下<b>:" + (Luck28SmallSingleCents - smallsingle + bigsingle) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28BigSingleCents - bigsingle + smallsingle == 0 && Luck28SmallSingleCents - smallsingle + bigsingle == 0)
            {
                builder.Append("押大单可下" + ub.Get("SiteBz") + "已达最大|押小单可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 6)
        {
            BuyNum = "1,3,5,7,9,11,13";
            Text = "小单";
            EngText = "Luck28SmallSingle";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28SmallSingleCents - smallsingle + bigsingle))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28SmallSingleCents - smallsingle + bigsingle) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28BigSingleCents - bigsingle + smallsingle == 0 && Luck28SmallSingleCents - smallsingle + bigsingle != 0)
            {
                builder.Append("押大单可下" + ub.Get("SiteBz") + "已达最大|押小单可下<b>:" + (Luck28SmallSingleCents - smallsingle + bigsingle) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28BigSingleCents - bigsingle + smallsingle != 0 && Luck28SmallSingleCents - smallsingle + bigsingle == 0)
            {
                builder.Append("押大单可下<b>:" + (Luck28BigSingleCents - bigsingle + smallsingle) + "</b>" + ub.Get("SiteBz") + "|押小单可下" + ub.Get("SiteBz") + "已达最大");
            }
            else if (Luck28BigSingleCents - bigsingle + smallsingle != 0 && Luck28SmallSingleCents - smallsingle + bigsingle != 0)
            {
                builder.Append("押大单可下<b>:" + (Luck28BigSingleCents - bigsingle + smallsingle) + "</b>" + ub.Get("SiteBz") + "|押小单可下<b>:" + (Luck28SmallSingleCents - smallsingle + bigsingle) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28BigSingleCents - bigsingle + smallsingle == 0 && Luck28SmallSingleCents - smallsingle + bigsingle == 0)
            {
                builder.Append("押大单可下" + ub.Get("SiteBz") + "已达最大|押小单可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 7)
        {
            BuyNum = "14,16,18,20,22,24,26";
            Text = "大双";
            EngText = "Luck28BigDouble";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28BigDoubleCents - bigdouble + smalldouble))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28BigDoubleCents - bigdouble + smalldouble) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28BigDoubleCents - bigdouble + smalldouble == 0 && Luck28SmallDoubleCents - smalldouble + bigdouble != 0)
            {
                builder.Append("押大双可下" + ub.Get("SiteBz") + "已达最大|押小双可下<b>:" + (Luck28SmallDoubleCents - smalldouble + bigdouble) + "</b>" + ub.Get("SiteBz"));
            }

            else if (Luck28BigDoubleCents - bigdouble + smalldouble != 0 && Luck28SmallDoubleCents - smalldouble + bigdouble == 0)
            {
                builder.Append("押大双可下<b>:" + (Luck28BigDoubleCents - bigdouble + smalldouble) + "</b>" + ub.Get("SiteBz") + "|押小双可下" + ub.Get("SiteBz") + "已达最大");
            }

            else if (Luck28BigDoubleCents - bigdouble + smalldouble != 0 && Luck28SmallDoubleCents - smalldouble + bigdouble != 0)
            {
                builder.Append("押大双可下<b>:" + (Luck28BigDoubleCents - bigdouble + smalldouble) + "</b>" + ub.Get("SiteBz") + "|押小双可下<b>:" + (Luck28SmallDoubleCents - smalldouble + bigdouble) + "</b>" + ub.Get("SiteBz"));
            }

            else if (Luck28BigDoubleCents - bigdouble + smalldouble == 0 && Luck28SmallDoubleCents - smalldouble + bigdouble == 0)
            {
                builder.Append("押大双可下" + ub.Get("SiteBz") + "已达最大|押小双可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 8)
        {
            BuyNum = "0,2,4,6,8,10,12";
            Text = "小双";
            EngText = "Luck28SmallDouble";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28SmallDoubleCents - smalldouble + bigdouble))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28SmallDoubleCents - smalldouble + bigdouble) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28BigDoubleCents - bigdouble + smalldouble == 0 && Luck28SmallDoubleCents - smalldouble + bigdouble != 0)
            {
                builder.Append("押大双可下" + ub.Get("SiteBz") + "已达最大|押小双可下<b>:" + (Luck28SmallDoubleCents - smalldouble + bigdouble) + "</b>" + ub.Get("SiteBz"));
            }

            else if (Luck28BigDoubleCents - bigdouble + smalldouble != 0 && Luck28SmallDoubleCents - smalldouble + bigdouble == 0)
            {
                builder.Append("押大双可下<b>:" + (Luck28BigDoubleCents - bigdouble + smalldouble) + "</b>" + ub.Get("SiteBz") + "|押小双可下" + ub.Get("SiteBz") + "已达最大");
            }

            else if (Luck28BigDoubleCents - bigdouble + smalldouble != 0 && Luck28SmallDoubleCents - smalldouble + bigdouble != 0)
            {
                builder.Append("押大双可下<b>:" + (Luck28BigDoubleCents - bigdouble + smalldouble) + "</b>" + ub.Get("SiteBz") + "|押小双可下<b>:" + (Luck28SmallDoubleCents - smalldouble + bigdouble) + "</b>" + ub.Get("SiteBz"));
            }

            else if (Luck28BigDoubleCents - bigdouble + smalldouble == 0 && Luck28SmallDoubleCents - smalldouble + bigdouble == 0)
            {
                builder.Append("押大双可下" + ub.Get("SiteBz") + "已达最大|押小双可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 9)
        {
            BuyNum = "0-9";
            Text = "一段";
            EngText = "Luck28First";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28FirstCents - first))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28FirstCents - first) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28FirstCents - first == 0)
            {
                builder.Append("押一段可下" + ub.Get("SiteBz") + "已达最大");
            }

            else if (Luck28FirstCents - first != 0)
            {
                builder.Append("押一段可下<b>:" + (Luck28FirstCents - first) + "</b>" + ub.Get("SiteBz") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        else if (ptype == 10)
        {
            BuyNum = "10-18";
            Text = "二段";
            EngText = "Luck28Secend";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28SecendCents - secend))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28SecendCents - secend) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28SecendCents - secend != 0)
            {
                builder.Append("押二段可下<b>:" + (Luck28SecendCents - secend) + "</b>" + ub.Get("SiteBz"));
            }
            else if (Luck28SecendCents - secend == 0)
            {
                builder.Append("押二段可下" + ub.Get("SiteBz") + "已达最大");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 11)
        {
            BuyNum = "19-27";
            Text = "三段";
            EngText = "Luck28Three";
            wanjia = BuyCent;
            if ((wanjia) > (Luck28ThreeCents - three))
            {
                Utils.Error("" + Text + "还可下注" + (Luck28ThreeCents - three) + kb, "");
            }
            if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("" + Text + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
            }
            builder.Append(Out.Tab("<div>", ""));
            if (Luck28ThreeCents - three == 0)
            {
                builder.Append("押三段可下" + ub.Get("SiteBz") + "已达最大");
            }

            else if (Luck28ThreeCents - three != 0)
            {
                builder.Append("押三段可下<b>:" + (Luck28ThreeCents - three) + "</b>" + ub.Get("SiteBz") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        long paysmall = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
        long paybig = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
        if ((BuyCent) < paysmall || (BuyCent) > paybig)
        {
            Utils.Error("单注下注额为" + paysmall + "到" + paybig, "");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=押注确认=<br />押注：" + Text + "" + BuyNum + "<br />押" + Text + "：" + BuyCent + "" + ub.Get("SiteBz") + "");
        builder.Append("<br/>" + "赔率：" + odds + "");
        builder.Append("<br/>" + "期号：" + luck.Bjkl8Qihao + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append("您目前共有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        string strName = "BuyNum,Text,ChText,BuyCent,odds,Bjkl8Qihao,act";
        string strValu = "" + BuyNum + "'" + EngText + "'" + Text + "'" + BuyCent + "'" + odds + "'" + luck.Bjkl8Qihao + "'payok";
        string strOthe = "确定下注,luck28.aspx,post,0,red";
        builder.Append(Out.wapform(strName, strValu, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">再看看吧</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
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
        IList<BCW.Model.Game.Lucklist> listLucklist = new BCW.BLL.Game.Lucklist().GetLucklists(pageIndex, pageSize, strWhere, out recordCount);
        if (listLucklist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Lucklist n in listLucklist)
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

                builder.Append("第" + n.Bjkl8Qihao + "期开出:<a href=\"" + Utils.getUrl("luck28.aspx?act=listview&amp;id=" + n.ID + "") + "\">" + n.SumNum + "=" + n.PostNum + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Lucklist model = new BCW.BLL.Game.Lucklist().GetLucklist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.Bjkl8Qihao + "期幸运28";


        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("luck28.aspx") + "\">幸运28</a>&gt;中奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("第" + model.Bjkl8Qihao + "期中奖友友<br />");

        // builder.Append("┗奖池:" + model.Pool + "" + ub.Get("SiteBz") + "");
        int pCount = Convert.ToInt32(BCW.Data.SqlHelper.GetSingle("Select Count(ID) from tb_Luckpay where LuckId=" + id + ""));
        builder.Append("┗参与人数:" + pCount + "人");
        builder.Append("<br />┗幸运数字:" + model.PostNum + "=" + model.SumNum + "<br />");
        builder.Append("┗北京快乐8第:" + model.Bjkl8Qihao + "期<br />");
        builder.Append("┗号码是:" + model.Bjkl8Nums + "<br />");
        // builder.Append("┗官网查询:<a href=\"" + Utils.getUrl("http://www.bwlc.net/bulletin/keno.html\" target=\"_blank\"") + "\">北京福彩网</a><br />");
        builder.Append("┗官网查询:<a href=\"http://www.bwlc.net/bulletin/keno.html\" target=\"_blank\">北京福彩网</a><br />");
        if (model.LuckCent > 0)
        {
            //if (!Utils.GetDomain().Contains("tuhao") && !Utils.GetDomain().Contains("th"))
            //{
            //    double Odds = 0;
            //    decimal getOdds = decimal.Divide(model.Pool, model.LuckCent);
            //    if (getOdds.ToString().Contains("."))
            //    {
            //        string[] OddsTemp = getOdds.ToString().Split(".".ToCharArray());
            //        string strOdds = OddsTemp[0] + "." + Utils.Left(OddsTemp[1], 2);
            //        Odds = Convert.ToDouble(strOdds);
            //    }
            //    else
            //    {
            //        Odds = Convert.ToDouble(getOdds);
            //    }

            //    builder.Append("<br />┗赔率:" + Odds + "倍");
            //}
            //else
            //{
            //builder.Append("<br />┗赔率:" + Convert.ToDouble(model.Pool / model.LuckCent) + "倍");

            //}
        }
        else
            //  builder.Append("<br />┗赔率:0倍");

            builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "LuckId=" + id + " and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Luckpay> listLuckpay = new BCW.BLL.Game.Luckpay().GetLuckpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listLuckpay.Count > 0)
        {

            builder.Append("共" + recordCount + "注中奖<br />");

            int k = 1;
            foreach (BCW.Model.Game.Luckpay n in listLuckpay)
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
            // builder.Append(Out.Div("div", "无人中奖,奖池金币滚入下期!"));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx?act=list") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayPage()
    {
        //if (IsOpen() == false)
        //{
        //    Utils.Error("游戏开放时间:" + ub.GetSub("Luck28OnTime", xmlPath) + "", "");
        //}
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Luck28End = ub.GetSub("Luck28End", xmlPath);
        string Luck28Choose = ub.GetSub("Luck28Choose", xmlPath);
        Master.Title = "我要下注";
        UpdateState11();
        BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("luck28.aspx") + "\">幸运28</a>&gt;投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("当前奖期:第" + luck.Bjkl8Qihao + "期|<a href=\"" + Utils.getUrl("luck28.aspx?act=pay&amp;ptype=" + ptype + "") + "\">刷新</a><br />");
        string Notes = ub.GetSub("Luck28Notes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.SysUBB(Notes) + "<br />");
        }
        long gold = new BCW.BLL.User().GetGold(meid);
        if (DateTime.Now < luck.EndTime)
        {
            string luck28 = new BCW.JS.somejs().newDaojishi("luck28", luck.EndTime);
            builder.Append("距" + luck.Bjkl8Qihao + "期投注还有" + luck28 + "截止");
        }
        // builder.Append("<br />当期奖池:" + Utils.ConvertGold(luck.Pool) + "" + ub.Get("SiteBz") + "");
        builder.Append("<br />┗推荐幸运数字:" + new Random().Next(0, 28) + "");
        builder.Append(Out.Tab("<br />", Out.Hr()));
        builder.Append("您目前共有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\">充值</a>");
        builder.Append(Out.Tab("</div>", ""));
        string choose = string.Empty;
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        // string faslInput = Utils.GetRequest("faslInput", "all", 3, @"^[0-9]\d+$", xml.dss["Luck28SmallPay"].ToString());
        long faslInput = Convert.ToInt64(Utils.GetRequest("faslInput", "all", 1, @"^[0-9]\d*$", "0"));
        //if (faslInput=="0")
        //{
        //    faslInput = xml.dss["Luck28SmallPay"].ToString();
        //}
        if (ptype == 0)
        {
            #region
            //string[] canbuy = { ub.GetSub("Luck28Buy0", xmlPath),  ub.GetSub("Luck28Buy1", xmlPath),  ub.GetSub("Luck28Buy2", xmlPath),  ub.GetSub("Luck28Buy3", xmlPath),  ub.GetSub("Luck28Buy4", xmlPath),
            //                    ub.GetSub("Luck28Buy5", xmlPath),  ub.GetSub("Luck28Buy6", xmlPath),  ub.GetSub("Luck28Buy7", xmlPath),  ub.GetSub("Luck28Buy8", xmlPath),  ub.GetSub("Luck28Buy9", xmlPath), 
            //                    ub.GetSub("Luck28Buy10", xmlPath),  ub.GetSub("Luck28Buy11", xmlPath),  ub.GetSub("Luck28Buy12", xmlPath),  ub.GetSub("Luck28Buy13", xmlPath),  ub.GetSub("Luck28Buy14", xmlPath), 
            //                    ub.GetSub("Luck28Buy15", xmlPath),  ub.GetSub("Luck28Buy16", xmlPath),  ub.GetSub("Luck28Buy17", xmlPath),  ub.GetSub("Luck28Buy18", xmlPath),  ub.GetSub("Luck28Buy19", xmlPath),  
            //                    ub.GetSub("Luck28Buy20", xmlPath),  ub.GetSub("Luck28Buy21", xmlPath),  ub.GetSub("Luck28Buy22", xmlPath),  ub.GetSub("Luck28Buy23", xmlPath),  ub.GetSub("Luck28Buy24", xmlPath), 
            //                    ub.GetSub("Luck28Buy25", xmlPath),  ub.GetSub("Luck28Buy26", xmlPath), ub.GetSub("Luck28Buy27", xmlPath) };
            string[] canbuy = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            for (int i = 0; i < 28; i++)
            {
                canbuy[i] = ub.GetSub("Luck28Buy" + i, xmlPath);
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【点击选号】");
            //builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("自选号<b>:" + Luck28Choose + "</b>倍");
            //builder.Append(Out.Tab("</div>", "<br />"));
            //builder.Append(Out.Tab("<div>", ""));
            builder.Append("<form id=\"form1\" method=\"post\" action=\"Luck28.aspx\">");
            builder.Append("0<input type=\"checkbox\" name=\"Num\" value=\"0\" />&nbsp;");
            builder.Append("1<input type=\"checkbox\" name=\"Num\" value=\"1\" />&nbsp;");
            builder.Append("2<input type=\"checkbox\" name=\"Num\" value=\"2\" />&nbsp;");
            builder.Append("3<input type=\"checkbox\" name=\"Num\" value=\"3\" />&nbsp;");
            builder.Append("4<input type=\"checkbox\" name=\"Num\" value=\"4\" />&nbsp;");
            builder.Append("5<input type=\"checkbox\" name=\"Num\" value=\"5\" />&nbsp;");
            builder.Append("6<input type=\"checkbox\" name=\"Num\" value=\"6\" />&nbsp;");
            builder.Append("7<input type=\"checkbox\" name=\"Num\" value=\"7\" />&nbsp;");
            builder.Append("8<input type=\"checkbox\" name=\"Num\" value=\"8\" />&nbsp;");
            builder.Append("9<input type=\"checkbox\" name=\"Num\" value=\"9\" /><br/>");
            builder.Append("10<input type=\"checkbox\" name=\"Num\" value=\"10\" />&nbsp;");
            builder.Append("11<input type=\"checkbox\" name=\"Num\" value=\"11\" />&nbsp;");
            builder.Append("12<input type=\"checkbox\" name=\"Num\" value=\"12\" />&nbsp;");
            builder.Append("13<input type=\"checkbox\" name=\"Num\" value=\"13\" />&nbsp;");
            builder.Append("14<input type=\"checkbox\" name=\"Num\" value=\"14\" />&nbsp;");
            builder.Append("15<input type=\"checkbox\" name=\"Num\" value=\"15\" />&nbsp;");
            builder.Append("16<input type=\"checkbox\" name=\"Num\" value=\"16\" />&nbsp;");
            builder.Append("17<input type=\"checkbox\" name=\"Num\" value=\"17\" />&nbsp;");
            builder.Append("18<input type=\"checkbox\" name=\"Num\" value=\"18\" /><br/>");
            builder.Append("19<input type=\"checkbox\" name=\"Num\" value=\"19\" />&nbsp;");
            builder.Append("20<input type=\"checkbox\" name=\"Num\" value=\"20\" />&nbsp;");
            builder.Append("21<input type=\"checkbox\" name=\"Num\" value=\"21\" />&nbsp;");
            builder.Append("22<input type=\"checkbox\" name=\"Num\" value=\"22\" />&nbsp;");
            builder.Append("23<input type=\"checkbox\" name=\"Num\" value=\"23\" />&nbsp;");
            builder.Append("24<input type=\"checkbox\" name=\"Num\" value=\"24\" />&nbsp;");
            builder.Append("25<input type=\"checkbox\" name=\"Num\" value=\"25\" />&nbsp;");
            builder.Append("26<input type=\"checkbox\" name=\"Num\" value=\"26\" />&nbsp;");
            builder.Append("27<input type=\"checkbox\" name=\"Num\" value=\"27\" /><br/>");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append("=快速下注=<br/>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 200000 + "") + "\" >200000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 300000 + "") + "\" >300000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 400000 + "") + "\" >400000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 500000 + "") + "\" >500000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 600000 + "") + "\" >600000</a>");
            //builder.Append("<br/>");
            builder.Append(Out.Tab("<div>", ""));
            try
            {
                builder.Append("【快捷下注】<br />∟");
                kuai(2, ptype);
            }
            catch { }
            if (faslInput == 0)
                builder.Append("<br/>每个号码下注的金额<input type=\"text\" name=\"BuyCent\" value=\"\" />&nbsp;");
            else
                builder.Append("<br/>每个号码下注的金额<input type=\"text\" name=\"BuyCent\" value=\"" + faslInput + "\" />&nbsp;");



            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"mychoose\"/>");
            builder.Append("<input type=\"hidden\" name=\"Text\" Value=\"Luck28Choose\"/>");
            builder.Append("<input type=\"hidden\" name=\"choose\" Value=\"" + 3 + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + 0 + "\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"mm\" value=\"确认下注\"  />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清空选号</a>");
            builder.Append("<br/>号码赔率：(前为号后为赔率)<br />");
            for (int i = 0; i < 28; i++)
            {
                string odds = ub.GetSub("Buy" + i + "odds", xmlPath);
                if (i == 27)
                {
                    builder.Append("" + i + "=" + odds + "倍");
                }
                else
                {
                    builder.Append("" + i + "=" + odds + "倍、");
                }
                if (i % 9 == 0 && i != 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region
            //string pText = "0尾#1尾#2尾#3尾#4尾#5尾#6尾#7尾#8尾#9尾";
            //string pValue = "0#1#2#3#4#5#6#7#8#9";
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("【点击选号】<br />");
            builder.Append("<form id=\"form1\" method=\"post\" action=\"Luck28.aspx\">");
            builder.Append("0<input type=\"checkbox\" name=\"Last\" value=\"0\" />&nbsp;");
            builder.Append("1<input type=\"checkbox\" name=\"Last\" value=\"1\" />&nbsp;");
            builder.Append("2<input type=\"checkbox\" name=\"Last\" value=\"2\" />&nbsp;");
            builder.Append("3<input type=\"checkbox\" name=\"Last\" value=\"3\" />&nbsp;");
            builder.Append("4<input type=\"checkbox\" name=\"Last\" value=\"4\" />&nbsp;");
            builder.Append("5<input type=\"checkbox\" name=\"Last\" value=\"5\" />&nbsp;");
            builder.Append("6<input type=\"checkbox\" name=\"Last\" value=\"6\" />&nbsp;");
            builder.Append("7<input type=\"checkbox\" name=\"Last\" value=\"7\" />&nbsp;");
            builder.Append("8<input type=\"checkbox\" name=\"Last\" value=\"8\" />&nbsp;");
            builder.Append("9<input type=\"checkbox\" name=\"Last\" value=\"9\" /><br/>");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append("=快速下注=<br/>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 200000 + "&amp;ptype=1") + "\" >200000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 300000 + "&amp;ptype=1") + "\" >300000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 400000 + "&amp;ptype=1") + "\" >400000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 500000 + "&amp;ptype=1") + "\" >500000|</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;faslInput=" + 600000 + "&amp;ptype=1") + "\" >600000</a>");
            //builder.Append("<br/>");
            builder.Append(Out.Tab("<div>", ""));
            try
            {
                builder.Append("【快捷下注】<br />∟");
                kuai(2, ptype);
            }
            catch { }
            if (faslInput == 0)
                builder.Append("<br/>每个尾数下注的金额<input type=\"text\" name=\"BuyCent\" value=\"\" />&nbsp;");
            else
                builder.Append("<br/>每个尾数下注的金额<input type=\"text\" name=\"BuyCent\" value=\"" + faslInput + "\" />&nbsp;");

            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"mychoose\"/>");
            builder.Append("<input type=\"hidden\" name=\"Text\" Value=\"Luck28End\"/>");
            builder.Append("<input type=\"hidden\" name=\"ac2\" Value=\"ok\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"nn\" value=\"确认下注\"/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Luck28.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清空选号</a>");

            builder.Append("<br/>尾数赔率：(前为号后为赔率):<br />");
            for (int i = 0; i < 10; i++)
            {
                string odds = ub.GetSub("End" + i + "odds", xmlPath);
                if (i == 9)
                {
                    builder.Append("" + i + "=" + odds + "倍");
                }
                else { builder.Append("" + i + "=" + odds + "倍、"); }


            }
            builder.Append(Out.Tab("</div>", ""));

            //string[] ptTemp = pText.Split("#".ToCharArray());
            //string[] pvTemp = pValue.Split("#".ToCharArray());

            //choose = Utils.GetRequest("choose", "get", 1, @"^[\d]{1,2}(?:,[\d]{1,2}){0,27}$", "");

            //for (int i = 0; i < pvTemp.Length; i++)
            //{
            //    string choose2 = pvTemp[i];
            //    if (choose != "")
            //        choose2 = choose + "," + choose2;

            //    if (("," + choose + ",").Contains("," + pvTemp[i] + ","))
            //        builder.Append("<b>" + ptTemp[i] + "</b> ");
            //    else
            //        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=pay&amp;choose=" + choose2 + "&amp;ptype=1") + "\">" + ptTemp[i] + "</a> ");
            //    if ((i + 1) % 4 == 0 && i != 9)
            //        builder.Append("<br />");
            //}
            //builder.Append(Out.Tab("</div>", ""));
            //strText = "1个尾数以上(多个用逗号分开):/,每数字押多少" + ub.Get("SiteBz") + ":/,,,";
            //strName = "BuyNum,BuyCent,Text,act,ac2";
            //strType = "text,num,hidden,hidden,hidden";
            //strValu = "" + choose + "''Luck28End'payok'ok";
            //strEmpt = "false,false,false,false,false";

            //strIdea = "<a href=\"" + Utils.getUrl("luck28.aspx?act=pay&amp;ptype=" + ptype + "") + "\">清<／a>''''|/";


            //strOthe = "确定下注,luck28.aspx,post,1,red";
            //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        //if (ptype == 0)
        //{
        //    // builder.Append("支持多个数字：1,5,8，或数字段：8-15<br />");
        //}
        //else if (ptype == 1)
        //    builder.Append("尾数说明：如你要下注01尾的数字，只需输入0,1即下注了0,10,20,1,11,21<br />");

        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">返回幸运28</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void mychoose()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "1"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();
        long BuyCent = 0;
        //BuyCent = Int64.Parse(Utils.GetRequest("Cent", "all", 1, @"^[0-9]\d*$", "0"));

        //if (BuyCent == 0)
        //BuyCent = Int64.Parse(Utils.GetRequest("BuyCent", "all", 1, @"^[0-9]\d*$", "下注金额填写出错"));

        string BuyCent2 = Utils.GetRequest("BuyCent", "all", 1, "", "0");
        string dsq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            dsq = Utils.GetRequest("BuyCent" + i + "", "all", 1, "", "");
            if (dsq != "") BuyCent2 = dsq;
        }
        if (BuyCent2.Contains("万"))
        {
            string str = string.Empty;
            str = BuyCent2.Replace("万", "");
            BuyCent = Convert.ToInt64(Convert.ToDouble(str) * 10000);
        }
        else
        {
            BuyCent = Convert.ToInt64(BuyCent2);
        }

        //BuyCent = Convert.ToInt64(BuyCent2);

        Master.Title = "第" + luck.Bjkl8Qihao + "期下注";
        string BuyNum = "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("luck28.aspx") + "\">幸运28</a>&gt;投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        string End0odds = ub.GetSub("End0odds", xmlPath);
        string End1odds = ub.GetSub("End1odds", xmlPath);
        string End2odds = ub.GetSub("End2odds", xmlPath);
        string End3odds = ub.GetSub("End3odds", xmlPath);
        string End4odds = ub.GetSub("End4odds", xmlPath);
        string End5odds = ub.GetSub("End5odds", xmlPath);
        string End6odds = ub.GetSub("End6odds", xmlPath);
        string End7odds = ub.GetSub("End7odds", xmlPath);
        string End8odds = ub.GetSub("End8odds", xmlPath);
        string End9odds = ub.GetSub("End9odds", xmlPath);
        long hadbuy = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + luck.ID);//得到某一期所有玩家的下注金额
        long MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//每期下注总额
        //Utils.Error("" + ptype + "", "");
        if (ptype == 1)
        {
            #region
            string getBuyNum = Utils.GetRequest("Last", "post", 2, @"^[\d((,)\d)?]+$", "请选择尾数号码！");
            // BuyNum += getBuyNum;
            string[] Temp = getBuyNum.Split(",".ToCharArray());
            for (int i = 0; i < Temp.Length; i++)
            {
                int ws = Convert.ToInt32(Temp[i]);
                //int cNum = Utils.GetStringNum("," + getBuyNum + ",", "," + ws + "");
                //if (cNum > 1)
                //{
                //    Utils.Error("尾数“" + ws + "重复”", "");
                //}
                string getWS = "";
                #region  开始判断尾号下注金额是否大于最大下注金额
                if (ws == 0)
                {
                    getWS = "0,10,20";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last0Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该0尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 1)
                {
                    getWS = "1,11,21";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last1Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该1尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 2)
                {
                    getWS = "2,12,22";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last2Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该2尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 3)
                {
                    getWS = "3,13,23";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last3Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该3尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 4)
                {
                    getWS = "4,14,24";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last4Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该4尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 5)
                {
                    getWS = "5,15,25";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last5Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该5尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 6)
                {
                    getWS = "6,16,26";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last6Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该6尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 7)
                {
                    getWS = "7,17,27";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last7Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该7尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 8)
                {
                    getWS = "8,18";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last8Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该8尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                else if (ws == 9)
                {
                    getWS = "9,19";
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last9Cents", xmlPath));//得到号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                    if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("该9尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                #endregion

                BuyNum += "," + getWS + "";
            }

            BuyNum = Utils.Mid(BuyNum, 1, BuyNum.Length);
            string[] Temp1 = getBuyNum.Split(',');
            long small = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
            if ((BuyCent * Temp1.Length) < small || (BuyCent * Temp1.Length) > big)
            {
                Utils.Error("单注下注额为" + small + "到" + big, "");
            }
            if ((BuyCent * Temp1.Length + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("尾数下注超过当期最大下注总额，还可下" + (MaxPool - (hadbuy)) + ub.Get("SiteBz"), "");
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择类型：尾号<br/>");
            builder.Append("当前期号：" + luck.Bjkl8Qihao + "期<br/>");
            builder.Append("选择尾号：" + getBuyNum + "尾<br/>");
            builder.Append("单个尾号：" + BuyCent + " " + ub.Get("SiteBz") + "<br/>");
            builder.Append("总的下注：" + (BuyCent * Temp1.Length) + " " + ub.Get("SiteBz") + "<br/>");
            //builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            string strName = "act,Text,BuyCent,Last,ac2,BuyNum,ChText,weishu,odds,Bjkl8Qihao";
            string strValu = "payok'Luck28End'" + BuyCent + "'" + getBuyNum + "'ok'" + BuyNum + "'尾号'" + getBuyNum + "'1'" + luck.Bjkl8Qihao;
            string strOthe = "确定投注,Luck28.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));

            #endregion
        }
        else
        {
            #region
            BuyNum = Utils.GetRequest("Num", "post", 2, @"^[\d((,)\d)?]+$", "请选择号码");
            string[] nums = BuyNum.Split(',');
            for (int i = 0; i < nums.Length; i++)
            {
                string haoma = nums[i].ToString();//得到选的第几个号码
                long canbuy = Utils.ParseInt(ub.GetSub("Luck28Buy" + haoma, xmlPath));//得到每个号码最大下注金额
                long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCentbychoose(luck.ID, haoma);  //得到某数字的总下注金额

                if (hasbuy + BuyCent > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                {
                    Utils.Error("" + haoma + "号码还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                }
            }
            long small = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
            if ((BuyCent * nums.Length) < small || (BuyCent * nums.Length) > big)
            {
                Utils.Error("单注下注额为" + small + "到" + big, "");
            }
            if ((BuyCent * nums.Length + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
            {
                Utils.Error("自选下注超过当期最大下注总额，还可下" + (MaxPool - (hadbuy)) + ub.Get("SiteBz"), "");
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择类型：自选号码<br/>");
            builder.Append("当前期号：" + luck.Bjkl8Qihao + "<br/>");
            builder.Append("选择号码：" + BuyNum + "<br/>");
            builder.Append("单个号码：" + BuyCent + "" + ub.Get("SiteBz") + "<br/>");
            builder.Append("总的下注：" + (BuyCent * nums.Length) + "" + ub.Get("SiteBz") + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br/>");
            string strName = "act,Text,BuyCent,BuyNum,choose,ChText,odds,Bjkl8Qihao";
            string strValu = "payok'Luck28Choose'" + BuyCent + "'" + BuyNum + "'ok1'自选'1'" + luck.Bjkl8Qihao;
            string strOthe = "确定投注,Luck28.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe)); ;
            #endregion
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">再看看吧>></a>");
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    private void PayOkPage()
    {
        //if (IsOpen() == false)
        //{
        //    Utils.Error("游戏开放时间:" + ub.GetSub("Luck28OnTime", xmlPath) + "", "");
        //}
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();
        string Bjkl8Qihao = Utils.GetRequest("Bjkl8Qihao", "post", 1, "", "");

        string odds = (Utils.GetRequest("odds", "post", 1, "", ""));
        string ac2 = Utils.GetRequest("ac2", "post", 1, "", "");
        string choose = (Utils.GetRequest("choose", "post", 1, "", ""));
        string weishu = "";

        string Text = "";
        string ChText = "";

        string BuyNum = "";
        BuyNum = Utils.GetRequest("BuyNum", "post", 1, "", "");
        weishu = Utils.GetRequest("weishu", "post", 1, "", "");
        Text = Utils.GetRequest("Text", "post", 1, "", "");
        ChText = Utils.GetRequest("ChText", "post", 1, "", "");
        long BuyCent = 0;

        // if (BuyCent == 0)
        BuyCent = Int64.Parse(Utils.GetRequest("BuyCent", "post", 4, @"^[0-9]\d*$", "下注金额填写出错"));
        //  BuyCent = Int64.Parse(Utils.GetRequest("Cent", "post", 5, @"^[0-9]\d*$", "0"));
        bool check1 = false;
        bool check2 = false;
        bool check3 = false;
        bool check4 = false;
        bool check5 = false;
        if (Bjkl8Qihao != luck.Bjkl8Qihao.ToString())
        {
            check1 = true;
        }
        string FloatBig = (ub.GetSub("FloatBig", xmlPath));//得到实时浮动赔率
        string FloatSmall = (ub.GetSub("FloatSmall", xmlPath));
        string FloatSingle = (ub.GetSub("FloatSingle", xmlPath));
        string FloatDouble = (ub.GetSub("FloatDouble", xmlPath));

        if (Text == "Luck28Big")//大小单双变化
        {
            if (odds != FloatBig)
            {
                check2 = true;
            }
        }
        if (Text == "Luck28Small")//大小单双变化
        {
            if (odds != FloatSmall)
            {
                check3 = true;
            }
        }
        if (Text == "Luck28Single")//大小单双变化
        {
            if (odds != FloatSingle)
            {
                check4 = true;
            }
        }
        if (Text == "Luck28Double")//大小单双变化
        {
            if (odds != FloatDouble)
            {
                check5 = true;
            }
        }
        string temp = "";
        #region //期号，赔率变化
        if (check1 || check2 || check3 || check4 || check5)//
        {
            new Out().head(Utils.ForWordType("温馨提示"));
            Response.Write(Out.Tab("<div class=\"title\">", ""));
            Response.Write("温馨提示");
            Response.Write(Out.Tab("</div>", "<br />"));
            Response.Write(Out.Tab("<div class=\"text\">", ""));
            //Response.Write("Bjkl8Qihao:" + Bjkl8Qihao+ ",luck.Bjkl8Qihao.ToString():"+ luck.Bjkl8Qihao.ToString());
            //Response.Write("check1:" + check1 + ",check2:" + check2 + ",check3:" + check3 + ",check4:" + check4 + ",check5:"+ check5);
            if (check1 == true)//期号不同
            {
                if (check2 == true)
                {
                    Response.Write("请注意！投注期号由" + Bjkl8Qihao + "期变为" + luck.Bjkl8Qihao.ToString() + "期,大赔率由" + odds + "变为" + FloatBig + "");
                    temp = FloatBig;

                }
                else if (check3 == true)
                {
                    temp = FloatSmall;
                    Response.Write("请注意！投注期号由" + Bjkl8Qihao + "期变为" + luck.Bjkl8Qihao.ToString() + "期,小赔率由" + odds + "变为" + FloatSmall + "");
                }
                else if (check4 == true)
                {
                    temp = FloatSingle;
                    Response.Write("请注意！投注期号由" + Bjkl8Qihao + "期变为" + luck.Bjkl8Qihao.ToString() + "期,单赔率由" + odds + "变为" + FloatSingle + "");
                }
                else if (check5 == true)
                {
                    temp = FloatDouble;
                    Response.Write("请注意！投注期号由" + Bjkl8Qihao + "期变为" + luck.Bjkl8Qihao.ToString() + "期,双赔率由" + odds + "变为" + FloatDouble + "");
                }
                else
                {
                    Response.Write("请注意！投注期号由" + Bjkl8Qihao + "期变为" + luck.Bjkl8Qihao.ToString() + "期");
                    temp = odds;
                }
            }
            else
            {
                if (check2 == true)
                {
                    temp = FloatBig;
                    Response.Write("请注意！大赔率由" + odds + "变为" + FloatBig + "");
                }
                else if (check3 == true)
                {
                    temp = FloatSmall;
                    Response.Write("请注意！小赔率由" + odds + "变为" + FloatSmall + "");
                }
                else if (check4 == true)
                {
                    temp = FloatSingle;
                    Response.Write("请注意！单赔率由" + odds + "变为" + FloatSingle + "");
                }
                else if (check5 == true)
                {
                    temp = FloatDouble;
                    Response.Write("请注意！双赔率由" + odds + "变为" + FloatDouble + "");
                }
            }
            odds = temp;
            Response.Write(Out.Tab("</div>", "<br />"));
            string strName = "odds,ac2,choose,BuyNum,Text,BuyCent,weishu,ChText,Bjkl8Qihao,act";
            string strValu = "" + odds + "'" + ac2 + "'" + choose + "'" + BuyNum + "'" + Text + "'" + BuyCent + "'" + weishu + "'" + ChText + "'" + luck.Bjkl8Qihao.ToString() + "'payok";
            string strOthe = "确定投注,luck28.aspx,post,0,red";
            Response.Write(Out.wapform(strName, strValu, strOthe));

            Response.Write(Out.Tab("<div>", "<br />"));
            Response.Write("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">[取消返回]</a><br />");
            Response.Write(Out.Tab("</div>", ""));
            Response.Write(new Out().foot());
            Response.End();
        }
        #endregion
        //支付安全提示
        string[] p_pageArr = { "ac2", "act", "choose", "BuyNum", "Text", "BuyCent", "allBuyCent", "weishu", "ChText", "odds", "Bjkl8Qihao" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);


        if (luck.ID != 0)//数据库不为空
        {
            if (ac2 == "ok")
            {
            }
            else if (choose == "ok1")   //自选号码
            {
            }
            else
            {
            }
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            BuyNum = BuyNum.Replace("，", ",");
            if (BuyNum.Contains("-"))
            {
                string[] strBuyNum = BuyNum.Split("-".ToCharArray());
                int oNums = Convert.ToInt32(strBuyNum[0]);
                int tNums = Convert.ToInt32(strBuyNum[1]);
                if (oNums > tNums)
                {
                    Utils.Error("幸运数字填写，正确格式如单数字：8，或多个数字：1,5,8，或数字段：8-15", "");
                }
                BuyNum = "";
                for (int i = oNums; i <= tNums; i++)
                {
                    BuyNum += " " + i;
                }

                BuyNum = BuyNum.Trim();
                BuyNum = BuyNum.Replace(" ", ",");
            }

            int gNum = Utils.GetStringNum(BuyNum, ",") + 1;
            long allBuyCent = 0;
            if (Text == "Luck28Choose")
            {
                #region 
                long hadbuy = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + luck.ID);//得到某一期所有玩家的下注金额
                long MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//每期下注总额
                string[] nums = BuyNum.Split(',');
                for (int i = 0; i < nums.Length; i++)
                {
                    string haoma = nums[i].ToString();//得到选的第几个号码
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Buy" + haoma, xmlPath));//得到每个号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCentbychoose(luck.ID, haoma);  //得到某数字的总下注金额

                    if (hasbuy + BuyCent > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                    {
                        Utils.Error("" + haoma + "号码还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                    }
                }
                long nsmall = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
                long nbig = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
                if ((BuyCent * nums.Length) < nsmall || (BuyCent * nums.Length) > nbig)
                {
                    Utils.Error("单注下注额为" + nsmall + "到" + nbig, "");
                }
                if ((BuyCent * nums.Length + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    Utils.Error("自选下注超过当期最大下注总额，还可下" + (MaxPool - (hadbuy)) + ub.Get("SiteBz"), "");
                }
                allBuyCent = Convert.ToInt64(BuyCent * gNum);
                #endregion

            }
            else if (Text == "Luck28End")
            {
                #region
                string[] asd = weishu.Split(',');//去余的尾数
                for (int i = 0; i < asd.Length; i++)
                {
                    int ws = Convert.ToInt32(asd[i]);
                    string getWS = "";
                    #region  开始判断尾号下注金额是否大于最大下注金额
                    if (ws == 0)
                    {
                        getWS = "0,10,20";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last0Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该0尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 1)
                    {
                        getWS = "1,11,21";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last1Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该1尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 2)
                    {
                        getWS = "2,12,22";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last2Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该2尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 3)
                    {
                        getWS = "3,13,23";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last3Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该3尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 4)
                    {
                        getWS = "4,14,24";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last4Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该4尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 5)
                    {
                        getWS = "5,15,25";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last5Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该5尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 6)
                    {
                        getWS = "6,16,26";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last6Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该6尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 7)
                    {
                        getWS = "7,17,27";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last7Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该7尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 8)
                    {
                        getWS = "8,18";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last8Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该8尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    else if (ws == 9)
                    {
                        getWS = "9,19";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last9Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额                 
                        if (((hasbuy + (BuyCent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            Utils.Error("该9尾号还可以下" + (canbuy - hasbuy) + ub.Get("SiteBz") + "", "");
                        }
                    }
                    #endregion
                }

                allBuyCent = Convert.ToInt64(BuyCent * asd.Length);
                long hadbuy = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + luck.ID);//得到某一期所有玩家的下注金额
                long MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//每期下注总额
                long qsmall = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
                long qbig = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
                if ((allBuyCent) < qsmall || (allBuyCent) > qbig)
                {
                    Utils.Error("单注下注额为" + qsmall + "到" + qbig, "");
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    Utils.Error("尾数下注超过当期最大下注总额，还可下" + (MaxPool - (hadbuy)) + ub.Get("SiteBz"), "");
                }
                #endregion
            }
            else
            {
                #region XML
                long Luck28BigCents = Utils.ParseInt(ub.GetSub("Luck28BigCents", xmlPath));//半数浮动设置下注额
                long Luck28SmallCents = Utils.ParseInt(ub.GetSub("Luck28SmallCents", xmlPath));

                long Luck28SingleCents = Utils.ParseInt(ub.GetSub("Luck28SingleCents", xmlPath));
                long Luck28DoubleCents = Utils.ParseInt(ub.GetSub("Luck28DoubleCents", xmlPath));

                long Luck28BigSingleCents = Utils.ParseInt(ub.GetSub("Luck28BigSingleCents", xmlPath));
                long Luck28SmallSingleCents = Utils.ParseInt(ub.GetSub("Luck28SmallSingleCents", xmlPath));

                long Luck28BigDoubleCents = Utils.ParseInt(ub.GetSub("Luck28BigDoubleCents", xmlPath));
                long Luck28SmallDoubleCents = Utils.ParseInt(ub.GetSub("Luck28SmallDoubleCents", xmlPath));

                long Luck28FirstCents = Utils.ParseInt(ub.GetSub("Luck28FirstCents", xmlPath));//分段每个段设置下注额
                long Luck28SecendCents = Utils.ParseInt(ub.GetSub("Luck28SecendCents", xmlPath));

                long Luck28ThreeCents = Utils.ParseInt(ub.GetSub("Luck28ThreeCents", xmlPath));
                long Luck28AllCents = Utils.ParseInt(ub.GetSub("Luck28AllCents", xmlPath));
                #endregion

                #region  //数据库的下注信息
                long Big = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Big");
                long small1 = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Small");

                long single = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Single");
                long doubl = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Double");

                long bigsingle = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28BigSingle");
                long smallsingle = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28SmallSingle");

                long bigdouble = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28BigDouble");
                long smalldouble = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28SmallDouble");

                long first = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28First");
                long secend = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Secend");

                long three = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(luck.ID, "Luck28Three");

                #endregion
                string kb = ub.Get("SiteBz");
                long hadbuy = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + luck.ID);//得到某一期所有玩家的下注金额
                long MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//每期下注总额
                #region 大小单双123段
                if ((BuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    Utils.Error("" + ChText + "超过当期最大下注总额，还可下" + (MaxPool - hadbuy) + kb, "");
                }
                if (Text == "Luck28Big")//大
                {
                    if ((BuyCent) > Luck28BigCents - Big + small1)
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28BigCents - Big + small1) + kb, "");
                    }
                }
                else if (Text == "Luck28Small")//小
                {
                    if ((BuyCent) > (Luck28SmallCents - small1 + Big))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28SmallCents - small1 + Big) + kb, "");
                    }
                }
                else if (Text == "Luck28Single")//单
                {
                    if ((BuyCent) > (Luck28SingleCents - single + doubl))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28SingleCents - single + doubl) + kb, "");
                    }
                }
                else if (Text == "Luck28Double")//双
                {
                    if ((BuyCent) > (Luck28DoubleCents - doubl + single))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28DoubleCents - doubl + single) + kb, "");
                    }
                }
                else if (Text == "Luck28BigSingle")//大单
                {
                    if ((BuyCent) > (Luck28BigSingleCents - bigsingle + smallsingle))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28BigSingleCents - bigsingle + smallsingle) + kb, "");
                    }
                }
                else if (Text == "Luck28SmallSingle")//小单
                {
                    if ((BuyCent) > (Luck28SmallSingleCents - smallsingle + bigsingle))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28SmallSingleCents - smallsingle + bigsingle) + kb, "");
                    }
                }
                else if (Text == "Luck28BigDouble")//大双
                {
                    if ((BuyCent) > (Luck28BigDoubleCents - bigdouble + smalldouble))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28BigDoubleCents - bigdouble + smalldouble) + kb, "");
                    }
                }
                else if (Text == "Luck28SmallDouble")//小双
                {
                    if ((BuyCent) > (Luck28SmallDoubleCents - smalldouble + bigdouble))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28SmallDoubleCents - smalldouble + bigdouble) + kb, "");
                    }
                }
                else if (Text == "Luck28First")//一段
                {
                    if ((BuyCent) > (Luck28FirstCents - first))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28FirstCents - first) + kb, "");
                    }
                }
                else if (Text == "Luck28Secend")//二段
                {
                    if ((BuyCent) > (Luck28SecendCents - secend))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28SecendCents - secend) + kb, "");
                    }
                }
                else if (Text == "Luck28Three")//三段
                {
                    if ((BuyCent) > (Luck28ThreeCents - three))
                    {
                        Utils.Error("" + ChText + "还可下注" + (Luck28ThreeCents - three) + kb, "");
                    }
                }
                #endregion
                allBuyCent = Convert.ToInt64(BuyCent);
            }

            string NewBuyNum = BuyNum.Replace(",", "##");
            string[] BuyNumTemp = Regex.Split(NewBuyNum, "##");
            for (int i = 0; i < BuyNumTemp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + NewBuyNum + "#", "#" + BuyNumTemp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("数字“" + BuyNumTemp[i] + "”重复输入", "");
                }
            }
            if (allBuyCent > new BCW.BLL.User().GetGold(meid))
            {
                Utils.Error("需花费" + allBuyCent + "" + ub.Get("SiteBz") + "，你身上" + ub.Get("SiteBz") + "不足", "");
            }
            long Luck28MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//最大奖池
            long allhasbuy = new BCW.BLL.Game.Luckpay().GetAllBuyCent(luck.ID);//得到某期所有玩家的下注数


            long myIDPay = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + luck.ID + " and UsID=" + meid);//得到这一期该玩家的总下注额
            long XMLEveryPay = Convert.ToInt64(ub.GetSub("Luck28EveryPay", xmlPath));//限制每个ID每期下注的金额

            if (myIDPay + allBuyCent > XMLEveryPay)
            {
                Utils.Error("每期每ID最多可下" + XMLEveryPay + ub.Get("SiteBz") + ",你已下" + myIDPay + ub.Get("SiteBz"), "");
            }


            long small = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
            if (allBuyCent < small || allBuyCent > big)
            {
                Utils.Error("单注下注额为" + small + "到" + big, "");
            }
            string appName = "LIGHT_LUCK28";
            int Expir = Utils.ParseInt(ub.GetSub("Luck28Expir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir, BuyCent, small, big);



            BCW.Model.Game.Luckpay model = new BCW.Model.Game.Luckpay();
            string mename = new BCW.BLL.User().GetUsName(meid);
            model.LuckId = luck.ID;
            model.UsID = meid;
            model.UsName = mename;
            model.BuyCent = BuyCent;
            model.BuyCents = allBuyCent;
            model.BuyNum = BuyNum;
            model.BuyType = Text;
            model.State = 0;
            model.IsRobot = 0;//不是机械人下注
            model.WinCent = 0;
            model.AddTime = DateTime.Now;
            if (odds.ToString() != "")
            {
                model.odds = odds;
            }
            else
            {
                model.odds = "1";
            }
            //isSystemID
            int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
            // Utils.Error("isSystemID:"+ isSystemID, "");

            int id = new BCW.BLL.Game.Luckpay().Add(model);
            //加奖池基金
            //    new BCW.BLL.Game.Lucklist().UpdatePool(luck.ID, allBuyCent);
            //string temp = string.Empty;
            string[] buy = { "押大", "押小", "押单", "押双", "大单", "小单", "大双", "小双", "一段", "二段", "三段", "全包", "自选", "尾数" };

            if (Text == "Luck28Choose")
            {
                if (isSystemID != 0)
                {
                    new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), allBuyCent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]二八第[url=./game/luck28.aspx?act=view&amp;id=" + luck.ID + "]" + luck.Bjkl8Qihao + "[/url]期买" + ChText + BuyNum + "共" + allBuyCent + "-标识" + id + "");
                }
                //扣币
                new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -allBuyCent, "二八第[url=./game/luck28.aspx?act=view&amp;id=" + luck.ID + "]" + luck.Bjkl8Qihao + "[/url]期押" + ChText + BuyNum + "-标识" + id + "");
            }
            else if (Text == "Luck28End")
            {
                if (isSystemID != 0)
                {
                    new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), allBuyCent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]二八第[url=./game/luck28.aspx?act=view&amp;id=" + luck.ID + "]" + luck.Bjkl8Qihao + "[/url]期买" + ChText + weishu + "共" + allBuyCent + "-标识" + id + "");
                }
                //扣币
                new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -allBuyCent, "二八第[url=./game/luck28.aspx?act=view&amp;id=" + luck.ID + "]" + luck.Bjkl8Qihao + "[/url]期押" + ChText + weishu + "-标识" + id + "");
            }
            else
            {
                if (isSystemID != 0)
                {
                    //向102加钱
                    new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), allBuyCent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]二八第[url=./game/luck28.aspx?act=view&amp;id=" + luck.ID + "]" + luck.Bjkl8Qihao + "[/url]期买" + ChText + ",赔率:" + model.odds + "共" + allBuyCent + "-标识" + id + "");
                }
                //扣币
                new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -allBuyCent, "二八第[url=./game/luck28.aspx?act=view&amp;id=" + luck.ID + "]" + luck.Bjkl8Qihao + "[/url]期押" + ChText + ",赔率:" + model.odds + "-标识" + id + "");
            }
            //活跃抽奖入口_20160621姚志光
            try
            {
                string gamename = ub.GetSub("Luck28Name", xmlPath);
                //表中存在幸运28最小金额投注记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName(gamename))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (allBuyCent > new BCW.BLL.tb_WinnersGame().GetPrice(gamename))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, gamename, 3);
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

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/luck28.aspx]幸运28第" + luck.Bjkl8Qihao + "期[/url]下注**" + ub.Get("SiteBz") + "";//" + Convert.ToInt64(allBuyCent) + "
            new BCW.BLL.Action().Add(1020, id, 0, "", wText);
            Utils.Success("下注", "下注成功，花费了" + allBuyCent + "" + ub.Get("SiteBz") + "<br />", Utils.getUrl("luck28.aspx"), "2");
        }
        else
        {
            Utils.Error("请等待增加期数，再下注", "");
        }
    }
    public string GOdds(string Eng)
    {
        double FloatBig = Convert.ToDouble(ub.GetSub("FloatBig", xmlPath));//得到浮动赔率
        double FloatSmall = Convert.ToDouble(ub.GetSub("FloatSmall", xmlPath));
        double FloatSingle = Convert.ToDouble(ub.GetSub("FloatSingle", xmlPath));
        double FloatDouble = Convert.ToDouble(ub.GetSub("FloatDouble", xmlPath));
        string Luck28Big = ub.GetSub("Luck28Big", xmlPath);
        string Luck28Small = ub.GetSub("Luck28Small", xmlPath);
        string Luck28Single = ub.GetSub("Luck28Single", xmlPath);
        string Luck28Double = ub.GetSub("Luck28Double", xmlPath);
        string Luck28BigSingle = ub.GetSub("Luck28BigSingle", xmlPath);
        string Luck28SmallSingle = ub.GetSub("Luck28SmallSingle", xmlPath);
        string Luck28BigDouble = ub.GetSub("Luck28BigDouble", xmlPath);
        string Luck28SmallDouble = ub.GetSub("Luck28SmallDouble", xmlPath);
        string Luck28First = ub.GetSub("Luck28First", xmlPath);
        string Luck28Secend = ub.GetSub("Luck28Secend", xmlPath);
        string Luck28Three = ub.GetSub("Luck28Three", xmlPath);
        string Luck28All = ub.GetSub("Luck28All", xmlPath);
        if (Eng == "Luck28Big")
            return "赔率为:" + Luck28Big;

        if (Eng == "Luck28Small")
            return "赔率为" + Luck28Small;

        if (Eng == "Luck28Single")
            return "赔率为" + Luck28Single;

        if (Eng == "Luck28Double")
            return "赔率为" + Luck28Double;

        if (Eng == "Luck28BigSingle")
            return "赔率为" + Luck28BigSingle;

        if (Eng == "Luck28SmallSingle")
            return "赔率为" + Luck28SmallSingle;

        if (Eng == "Luck28BigDouble")
            return "赔率为" + Luck28BigDouble;

        if (Eng == "Luck28SmallDouble")
            return "赔率为" + Luck28SmallDouble;

        if (Eng == "Luck28First")
            return "赔率为" + Luck28First;

        if (Eng == "Luck28Secend")
            return "赔率为" + Luck28Secend;

        if (Eng == "Luck28Three")
            return "赔率为" + Luck28Three;
        else
            return "";

    }
    public string GetChooseOdds(string choose)
    {
        string[] ch = choose.Split(',');
        string result = string.Empty;
        for (int i = 0; i < ch.Length; i++)
        {
            if (string.IsNullOrEmpty(result))
            {
                result = result + ch[i] + "(" + ub.GetSub("Buy" + ch[i] + "odds", xmlPath) + ")";
            }
            else
            {
                result = result + "," + ch[i] + "(" + ub.GetSub("Buy" + ch[i] + "odds", xmlPath) + ")";
            }
        }
        return result;
    }
    public string GetEH(string End, int num)
    {
        //  builder.Append("+"+num+"+");
        string[] aa = End.Split(',');
        string result = "";
        num = num % 10;
        for (int i = 0; i < aa.Length; i++)
        {
            int a = Utils.ParseInt(aa[i]) % 10;  //得到个位数
            if (!result.Contains(a + "")) //得出尾数
            {
                if (string.IsNullOrEmpty(result))
                {
                    if (num == a)
                    {
                        result = result + a + "(" + ub.GetSub("End" + a + "odds", xmlPath) + ")";
                    }
                    else
                    {
                        result = result + a;
                    }
                }
                else
                {
                    if (num == a)
                    {
                        result = result + "," + a + "(" + ub.GetSub("End" + a + "odds", xmlPath) + ")";
                    }
                    else
                    {
                        result = result + "," + a;
                    }

                }
            }
        }
        return result;
    }
    public string GetCH(string choose, int num)
    {
        string[] ch = choose.Split(',');
        string result = string.Empty;
        for (int i = 0; i < ch.Length; i++)
        {
            if (string.IsNullOrEmpty(result))
            {
                if (ch[i] == num.ToString())
                {
                    result = result + ch[i] + "(" + ub.GetSub("Buy" + ch[i] + "odds", xmlPath) + ")";
                }
                else
                {
                    result = result + ch[i];
                }
            }
            else
            {
                if (ch[i] == num.ToString())
                {
                    result = result + "," + ch[i] + "(" + ub.GetSub("Buy" + ch[i] + "odds", xmlPath) + ")";
                }
                else
                {
                    result = result + "," + ch[i];
                }
            }
        }
        return result;
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
        IList<BCW.Model.Game.Luckpay> listLuckpay = new BCW.BLL.Game.Luckpay().GetLuckpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listLuckpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Luckpay n in listLuckpay)
            {
                string chin = GetChinese(n.BuyType);
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                int aa = n.LuckId;
                string bb = string.Empty;
                bb = n.BuyType;
                if (!string.IsNullOrEmpty(bb))    //新版数据
                {
                    try
                    {
                        if (n.LuckId.ToString() != Luckqi)
                            builder.Append("第" + new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).Bjkl8Qihao + "期,开奖号码是" + new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).SumNum + "=" + new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).PostNum + "<BR/>");
                    }
                    catch
                    {
                        builder.Append("" + (n.LuckId) + "+");
                    }
                    string shuoming = string.Empty;
                    if (n.BuyType == "Luck28End")
                    {
                        if (ptype == 1)  //未开投注
                        {
                            n.BuyNum = GetLast(n.BuyNum);
                            shuoming = "" + chin + n.BuyNum + "，每尾数" + n.BuyCent + "";
                        }
                        else  //历史投注
                        {
                            string weikai = string.Empty;
                            try
                            {
                                weikai = GetEH(n.BuyNum, new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).SumNum);
                            }
                            catch
                            {
                                builder.Append("" + (n.LuckId));
                            }
                            shuoming = "" + chin + weikai + "，每尾数" + n.BuyCent + "";
                        }
                    }
                    else if (n.BuyType == "Luck28Choose")
                    {
                        if (ptype == 1)//未开投注
                        {
                            string chooseodds = GetChooseOdds(n.BuyNum);
                            shuoming = "" + chin + chooseodds + "，每数字" + n.BuyCent + "";
                        }
                        else //历史投注
                        {
                            string HCO = GetCH(n.BuyNum, new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).SumNum);
                            shuoming = "" + chin + HCO + "，每数字" + n.BuyCent + "";
                        }
                    }
                    else
                    {
                        shuoming = "" + chin + "" + "，" + n.BuyCent + "";
                    }
                    string odds = "" + GOdds(n.BuyType) + "";
                    builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                    if (n.State == 0)
                    {
                        if (n.BuyType == "Luck28End")
                        {
                            builder.Append(shuoming + ub.Get("SiteBz") + "" + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                        }
                        else if (n.BuyType == "Luck28Choose")
                        {
                            builder.Append(shuoming + ub.Get("SiteBz") + "" + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                        }
                        else
                        {
                            builder.Append(shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                        }
                    }
                    else if (n.State == 1)
                    {
                        if (n.BuyType == "Luck28End")
                        {
                            builder.Append(shuoming + ub.Get("SiteBz") + "" + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                        }
                        else if (n.BuyType == "Luck28Choose")
                        {
                            builder.Append(shuoming + ub.Get("SiteBz") + "" + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                        }
                        else
                        {
                            builder.Append(shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                        }
                        //  builder.Append(shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                        if (n.WinCent > 0)
                        {
                            builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                        }
                    }
                    else
                    {
                        if (n.BuyType == "Luck28End")
                        {
                            builder.Append("" + shuoming + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                        }
                        else if (n.BuyType == "Luck28Choose")
                        {
                            builder.Append("" + shuoming + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                        }
                        else
                        {
                            builder.Append("" + shuoming + "" + ub.Get("SiteBz") + "赔率为" + n.odds + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                        }
                    }
                    //  builder.Append(shuoming + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                }
                else   //旧版数据
                {
                    if (n.LuckId.ToString() != Luckqi)
                        builder.Append("=第" + n.LuckId + "期(旧)=<br />");

                    builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                    if (n.State == 0)
                        builder.Append("买" + n.BuyNum + "，每数字" + n.BuyCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    else if (n.State == 1)
                    {
                        builder.Append("买" + n.BuyNum + "，每数字" + n.BuyCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                        if (n.WinCent > 0)
                        {
                            builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                        }
                    }
                    else
                        builder.Append("买" + n.BuyNum + "，每数字" + n.BuyCent + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                }
                Luckqi = n.LuckId.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private string GetLast(string buynums)
    {
        string[] aa = buynums.Split(',');
        string result = "";
        for (int i = 0; i < aa.Length; i++)
        {
            int a = Utils.ParseInt(aa[i]) % 10;  //得到个位数
            if (!result.Contains(a + "(")) //得出尾数
            {
                if (string.IsNullOrEmpty(result))
                {
                    result = result + a + "(" + ub.GetSub("End" + a + "odds", xmlPath) + ")";
                }
                else
                {
                    result = result + "," + a + "(" + ub.GetSub("End" + a + "odds", xmlPath) + ")";
                }
            }
        }
        return result;
    }
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));//pay表ID
        BCW.Model.Game.Luckpay model = new BCW.BLL.Game.Luckpay().GetLuckpay(pid);
        if (new BCW.BLL.Game.Luckpay().ExistsState(pid, meid))
        {
            new BCW.BLL.Game.Luckpay().UpdateState(pid);
            //操作币
            long winMoney = new BCW.BLL.Game.Luckpay().GetWinCent(pid);
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("Luck28Tax", xmlPath));
            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.001);
            }
            winMoney = winMoney - SysTax;
            string chinese = GetChinese(model.BuyType);
            BCW.User.Users.IsFresh("luck28", 1);//防刷
            int luckid = new BCW.BLL.Game.Lucklist().GetLucklist(model.LuckId).Bjkl8Qihao;
            new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), winMoney, "二八第[url=./game/luck28.aspx?act=view&amp;id=" + model.LuckId + "]" + luckid + "[/url]兑奖-标识ID" + pid + "");
            //102
            int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
            if (isSystemID != 0)//不是系统号
            {
                new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]二八第[url=./game/luck28.aspx?act=view&amp;id=" + model.LuckId + "]" + luckid + "[/url]期兑奖" + winMoney + "(标识ID" + model.ID + ")");
            }
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("luck28.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("luck28.aspx?act=case"), "1");
        }
    }
    private string GetChinese(string Eng)
    {
        if (Eng == "Luck28Big")
            return "押大";

        if (Eng == "Luck28Small")
            return "押小";

        if (Eng == "Luck28Single")
            return "押单";

        if (Eng == "Luck28Double")
            return "押双";

        if (Eng == "Luck28BigSingle")
            return "押大单";

        if (Eng == "Luck28SmallSingle")
            return "押小单";

        if (Eng == "Luck28BigDouble")
            return "押大双";

        if (Eng == "Luck28SmallDouble")
            return "押小双";

        if (Eng == "Luck28First")
            return "押一段";

        if (Eng == "Luck28Secend")
            return "押二段";

        if (Eng == "Luck28Three")
            return "押三段";

        if (Eng == "Luck28End")
            return "押尾数";

        if (Eng == "Luck28Choose")
            return "押自选";
        else
            return "";
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
        BCW.User.Users.IsFresh("luck28", 1);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            BCW.Model.Game.Luckpay model = new BCW.BLL.Game.Luckpay().GetLuckpay(pid);
            if (new BCW.BLL.Game.Luckpay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.Luckpay().UpdateState(pid);
                //操作币
                winMoney = new BCW.BLL.Game.Luckpay().GetWinCent(pid);
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("Luck28Tax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.001);
                }
                winMoney = winMoney - SysTax;
                int luckid = new BCW.BLL.Game.Lucklist().GetLucklist(model.LuckId).Bjkl8Qihao;
                //用户加钱
                new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), winMoney, "二八第[url=./game/luck28.aspx?act=view&amp;id=" + model.LuckId + "]" + luckid + "[/url]兑奖-标识ID" + pid + "");
                //扣102的钱 
                int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                if (isSystemID != 0)//不是系统号
                {
                    new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]二八第[url=./game/luck28.aspx?act=view&amp;id=" + model.LuckId + "]" + luckid + "[/url]期兑奖" + winMoney + "(标识ID" + model.ID + ")");
                }
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("luck28.aspx?act=case"), "1");
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
        strWhere = "UsID=" + meid + " and WinCent>0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<BCW.Model.Game.Luckpay> listLuckpay = new BCW.BLL.Game.Luckpay().GetLuckpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listLuckpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Luckpay n in listLuckpay)
            {
                int aa = n.LuckId;
                string type = n.BuyType;
                if (!string.IsNullOrEmpty(type))   //新版数据
                {
                    string chin = GetChinese(n.BuyType);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    //  builder.Append("n.LuckId:"+ n.LuckId);
                    builder.Append("[第" + new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).Bjkl8Qihao + "期].");
                    string shuoming = string.Empty;
                    if (n.BuyType == "Luck28End")
                    {
                        string weikai = GetEH(n.BuyNum, new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).SumNum);
                        shuoming = "" + chin + weikai + "，每尾数" + n.BuyCent + "";
                    }
                    else if (n.BuyType == "Luck28Choose")
                    {
                        string HCO = GetCH(n.BuyNum, new BCW.BLL.Game.Lucklist().GetLucklist(n.LuckId).SumNum);
                        shuoming = "" + chin + HCO + "，每数字" + n.BuyCent + "";
                    }

                    else
                    {

                        shuoming = "" + chin + "" + "，" + n.BuyCent;
                    }
                    string odds = "" + GOdds(n.BuyType) + "";
                    if (n.BuyType == "Luck28End")
                    {
                        builder.Append(shuoming + "" + ub.Get("SiteBz") + "*" + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("luck28.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                    }
                    else if (n.BuyType == "Luck28Choose")
                    {
                        builder.Append(shuoming + "" + ub.Get("SiteBz") + "*" + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("luck28.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                    }
                    else
                    {
                        builder.Append(shuoming + "" + ub.Get("SiteBz") + "*赔率：" + n.odds + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("luck28.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                    }
                    // builder.Append(shuoming + "" + ub.Get("SiteBz") + "*" + odds + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("luck28.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                }
                else  //旧版数据
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
                    builder.Append("[第" + n.LuckId + "期(旧)].");
                    builder.Append("买" + n.BuyNum + "，每数字" + n.BuyCent + "" + ub.Get("SiteBz") + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("luck28.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");


                }
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
            string strOthe = "本页兑奖,luck28.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-6]$", "0"));
        string sTime = Utils.GetRequest("sTime", "all", 1, "", "");
        string oTime = Utils.GetRequest("oTime", "all", 1, "", "");
        Master.Title = "幸运28排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("幸运28排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = "BuyCents>0";
        string[] pageValUrl = { "act", "backurl", "ptype", "sTime", "oTime" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (pageIndex > 10)
            pageIndex = 10;
        if (sTime != "")
        {
            strWhere += "and AddTime>='" + sTime + "'and AddTime<'" + oTime + "'";
        }
        //builder.Append("searchday1" + searchday1+"<br/>");
        //builder.Append("searchday2" + searchday2 + "<br/>");
        //string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //if (Utils.ToSChinese(ac) == "马上查询")
        //{
        //    DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 1,"", "开始时间填写无效"));
        //    DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 1, "", "结束时间填写无效"));
        //    string _where1 = string.Empty;
        //    string _where2 = string.Empty;
        //    _where2 = "and AddTime>='" + searchday1 + "'and AddTime<'" + searchday2 + "'";
        //    strWhere += _where2;

        //}
        string one = "";
        string two = "";
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 1)
        {
            builder.Append("<b style=\"color:red\">总榜" + "</b>" + "|");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">总榜</a>" + "|");
        if (ptype == 2)
        {
            builder.Append("<b style=\"color:red\">当天" + "</b>" + "|");
            one = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            two = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            strWhere += "and AddTime>='" + one + "'and AddTime<'" + two + "'";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">当天</a>" + "|");
        if (ptype == 6)
        {
            builder.Append("<b style=\"color:red\">昨天" + "</b>" + "|");
            one = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            one = DateTime.Parse(one).AddDays(-1).ToString();
            two = DateTime.Parse(one).ToString("yyyy-MM-dd") + " 23:59:59";
            strWhere += "and AddTime>='" + one + "'and AddTime<'" + two + "'";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">昨天</a>" + "|");
        if (ptype == 3)
        {
            builder.Append("<b style=\"color:red\">当月" + "</b>" + "|");
            one = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00";
            two = DateTime.Parse(one).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
            strWhere += "and AddTime>='" + one + "'and AddTime<'" + two + "'";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">当月</a>" + "|");
        if (ptype == 4)
        {
            builder.Append("<b style=\"color:red\">上月" + "</b>" + "|");
            one = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00";
            one = DateTime.Parse(one).AddMonths(-1).ToString();
            two = DateTime.Parse(one).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
            strWhere += "and AddTime>='" + one + "'and AddTime<'" + two + "'";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">上月</a>" + "|");
        if (ptype == 5)
        {
            builder.Append("<b style=\"color:red\">当年" + "</b>" + "");
            one = DateTime.Now.ToString("yyyy") + "-01-01 00:00:00";
            two = DateTime.Parse(one).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
            strWhere += "and AddTime>='" + one + "'and AddTime<'" + two + "'";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">当年</a>" + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        // 开始读取列表
        IList<BCW.Model.Game.Luckpay> listLuckpay = new BCW.BLL.Game.Luckpay().GetLuckpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listLuckpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Luckpay n in listLuckpay)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>(" + n.UsID + ")赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            if (recordCount >= 100)
            {
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = "开始日期：,结束日期：,";
        string strName = "sTime,oTime,act";
        string strType = "date,date,hidden,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'top'";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "马上查询,luck28.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void HelpPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-6]$", "0"));
        Master.Title = "幸运28游戏帮助";
        string OnTime = ub.GetSub("Luck28OnTime", xmlPath);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("游戏帮助");
        builder.Append(Out.Tab("</div>", "<br/>"));
        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("一·为保证开奖公平性，与以往的系统开奖不同，新幸运28开奖号码是北京快乐8的开奖数据来计算；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;这种开奖结果也是非常公平，无人工干涉，返奖按固定的赔率进行返奖。<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;北京快乐8每期开奖共开出20个数字，新幸运28将这20个开奖号码按照由小到大的顺序依次排列。<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其1-6位开奖号码相加，和值的末位数作为幸运28开奖第一个数值；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其7-12位开奖号码相加，和值的末位数作为幸运28开奖第二个数值；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其13-18位开奖号码相加，和值的末位数作为幸运28开奖第三个数值；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;三个数值相加即为新幸运28最终的开奖结果。游戏每天09:00-23:55开放，5分钟开一期。<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;如下图所示<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/files/face/bjkl8.png\" width=\"500\" height=\"250\"  alt=\"load\"/>");
            builder.Append("<br/>二·玩法<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;1·大：大14-27；小：0-13；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;2·单：1,3,5,7,9,11,13,15,17,19,21,23,25,27；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;3·双：0,2,4,6,8,10,12,14,16,18,20,22,24,26；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;4·大双：14,16,18,20,22,24,26；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;5·大单：15,17,19,21,23,25,27；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;6·小双：0,2,4,6,8,10,12；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;7·小单：1,3,5,7,9,11,13；<br/>");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;8·一段：0-9；二段：10-18；三段：19-27；<br/>");
            builder.Append("<b>温馨提示：大小单双返奖时，均按照当时下注时的赔率为准.</b><br/>");
            builder.Append(Out.Tab("</div>", ""));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=help") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            if (ptype == 1)
            {
                builder.Append("什么是幸运28？<br />");
                builder.Append("幸运28，来源于国家福利彩票北京快乐8(官网)开奖号码，从早上9:05至23:55，每5分钟一期不停开奖。<br />北京快乐8每期开奖共开出20个数字，幸运28将这20个开奖号码按照由小到大的顺序依次排列。<br />取其1-6位开奖号码相加，和值的末位数作为第三方28开奖第一个数值,<br /> 取其7-12位开奖号码相加，和值的末位数作为幸运28开奖第二个数值，<br />取其13-18位开奖号码相加，和值的末位数作为第三方28开奖第三个数值，<br />三个数值相加即为幸运28最终的开奖结果。<br />游戏每天" + OnTime + "开放，" + ub.GetSub("Luck28CycleMin", xmlPath) + "分钟开一期");
            }
            else if (ptype == 2)
            {
                builder.Append("幸运数字如何产生？<br />");
                builder.Append("幸运28总共开出28个数字，从0依次到27，它由三个单独的个位数字相加所得。您若买中，即可获得奖励。每个数字出现的概率并不一样。<br />比如出现0的可能只有一个，就是0+0+0，出现1的组合有三个，分别是：1+0+0，0+1+0，0+0+1。依此类推，可见越是中间的数字（比如13和14）其组合越多，比如出现13或14的可能组合有75个。");
            }
            else if (ptype == 3)
            {
                builder.Append("投注有什么技巧？<br />");
                builder.Append("最聪明：每次买四五个数字，下注适量币币，这种方式细水长流，适合多数玩家体验，玩多了也最容易赚币币，当然啦，运气也很重要。<br />最保险：28个数字全部买。。。这种方法是百分百中奖，至于赢还是输？要看大伙的投币情况喽，这种投注适合求稳的家伙。<br />最搏命：只买一个数字，孤注一掷，如果买中一把就赚翻了，当然输了就惨了，适合富翁们偶尔一试。");
            }
            //else if (ptype == 4)
            //{
            //    builder.Append("奖金是如何分配的？<br />");
            //    builder.Append("玩家中奖＝买中幸运数字的币币×赔率。<br />赔率＝奖金总额÷本期所有中奖的币币。<br />例：第1期奖金池为100万，幸运数字为10，所有玩家买10的币币总量是10万，则：赔率=100万/10万=10。如果你投注10的币币是1万，你将获得币币数=（100万/10万）*1万=10万<br />当然，实际操作中，奖金总额还要扣除" + ub.GetSub("Luck28Tax", xmlPath) + "‰的税收。");
            //}
            else if (ptype == 5)
            {
                builder.Append("如何知道我是否中奖？<br />");
                builder.Append("当您获奖后，您将收到系统发给您的消息。你可以根据消息提示或者在幸运28首页，点击“兑奖”领取以往的所有中奖币币。中奖的历史记录可能会清除，您应该尽量领取奖金。");
            }
            //else if (ptype == 6)
            //{
            //    builder.Append("如果某一期没人中奖怎么办？<br />");
            //    builder.Append("如果某一期没有玩家买中幸运数字，所有投注币币（扣除系统投注）将会滚动到下期奖金。");
            //}
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Help2Page()
    {
        Master.Title = "幸运28押注规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("幸运28押注规则");
        builder.Append(Out.Tab("</div>", "<br />"));

        string rule = @"大小:小数0-13,大数14-27<br />
单数:1,3,5,7,9,11,13,15,17,19,21,23,25,27<br />
双数:0,2,4,6,8,10,12,14,16,18,20,22,24,26<br />
分段:0-9段  10-18段  19-27段<br />
尾数:<br />
0尾0,10,20<br />
1尾1,11,21<br />
2尾2,12,22<br />
3尾3,13,23<br />
4尾4,14,24<br />
5尾5,15,25<br />
6尾6,16,26<br />
7尾7,17,27<br />
8尾8,18<br />
9尾9,19
";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(rule);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("Luck28OnTime", xmlPath);
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now > dt1 && DateTime.Now < dt2)
                {
                    IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
        }
        return IsOpen;
    }



    /// <summary>
    /// 更新期数
    /// </summary>
    /// <returns></returns>
    public string UpdateState11()
    {
        BCW.Model.Game.Lucklist last = new BCW.BLL.Game.Lucklist().GetLucklistState();

        string[] a1 = last.EndTime.ToString().Split(' ');
        DateTime dt1 = Convert.ToDateTime("09:00");
        DateTime dt2 = Convert.ToDateTime("23:57");
        DateTime now = DateTime.Now;
        DateTime time = Convert.ToDateTime(a1[0] + " 23:55:00");
        int j = 0;
        string ms = last.EndTime.ToString("yyyy-MM-dd hh:mm:00");
        int getqh = Utils.ParseInt(ub.GetSub("Luck28StartQH", xmlPath));
        int maxid = new BCW.BLL.Game.Lucklist().GetMaxId();
        if (maxid > 0)
        {
            //  if (now >= dt1 && now <= dt2)  //在 9-23：57内运行下列代码
            {
                while (DateTime.Now > last.EndTime)
                {
                    //   Response.Write("last.EndTime :" + last.EndTime + "---");
                    BCW.Model.Game.Lucklist add1 = new BCW.Model.Game.Lucklist();
                    add1.SumNum = 0;
                    add1.PostNum = "";
                    add1.BeginTime = last.EndTime;
                    //   Response.Write("add.BeginTime:" + add1.BeginTime + "---");           
                    // ms = last.EndTime.ToString("yyyy-MM-dd hh:mm:00");
                    //last.EndTime = Convert.ToDateTime(ms);//去除秒数            
                    string[] day = last.EndTime.ToString().Split(' ');//天；
                    string[] hm = day[1].Split(':');
                    string mt = day[0] + " " + hm[0] + ":" + hm[1] + ":00";
                    DateTime mlast = Convert.ToDateTime(mt);
                    // Response.Write("mlast.EndTime:" + mlast + "---");
                    last.EndTime = mlast;   //去秒数
                    add1.EndTime = last.EndTime.AddMinutes(5);
                    last.EndTime = last.EndTime.AddMinutes(5);
                    //Response.Write("DateTime.Now :" + DateTime.Now + "---");
                    //Response.Write("last.EndTime:" + last.EndTime + "---");
                    //Response.Write(" (DateTime.Now > last.EndTime):" + (DateTime.Now > last.EndTime) + "---<br/>");
                    add1.panduan = last.panduan;
                    DateTime BeginTime = Convert.ToDateTime(a1[0] + " 09:00:00");
                    if (add1.BeginTime >= time)//添加的期数是当天的最后一期
                    {
                        add1.panduan = last.Bjkl8Qihao.ToString();

                        add1.BeginTime = BeginTime.AddDays(1);
                        last.BeginTime = BeginTime.AddDays(1);
                        //    Response.Write("add.BeginTime:" + add1.BeginTime + "---");

                        add1.EndTime = BeginTime.AddDays(1).AddMinutes(5);
                        last.EndTime = BeginTime.AddDays(1).AddMinutes(5);
                        //Response.Write("add.EndTime:" + add1.EndTime + "---<br/>");

                        last.panduan = (last.Bjkl8Qihao + 1).ToString();
                        a1 = last.EndTime.ToString().Split(' ');  //第二日天数
                        time = Convert.ToDateTime(a1[0] + " 23:55:00");
                        //    Response.Write("time:" + time + "---");
                    }
                    last.Bjkl8Qihao += 1;
                    add1.Bjkl8Qihao = last.Bjkl8Qihao;
                    add1.LuckCent = 0;
                    add1.Pool = 0;
                    add1.BeforePool = 1;
                    add1.State = 0;
                    if (add1.BeginTime == BeginTime.AddDays(1))//添加的期数是当天的最新一期
                    {
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                    //  Response.Write("j:" + j + " ");
                    if (!new BCW.BLL.Game.Lucklist().ExistsBJQH(last.Bjkl8Qihao))  //如果不存在该期号，存入数据库
                    {
                        new BCW.BLL.Game.Lucklist().Add(add1);//增加最新一期   
                    }

                    // Response.Write("add.Bjkl8Qihao" + last.Bjkl8Qihao + "<br/>");
                }
            }
        }
        return "0";
    }

    /// <summary>
    /// 显示还有多少秒
    /// </summary>
    /// <param name="luck">当前期数</param>
    private void showtime(BCW.Model.Game.Lucklist luck)
    {

        if (DateTime.Now < luck.EndTime)
        {
            string luck28 = new BCW.JS.somejs().newDaojishi("luck28", luck.EndTime);
            builder.Append("距" + luck.ID + "期开奖还有" + luck28 + "开奖");
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //{
            //    string luck28 = new BCW.JS.somejs().daojishi2("luck28", luck.EndTime);
            //    builder.Append("距" + luck.ID + "期开奖还有" + luck28 + "开奖");

            //}
            //else
            //{
            //    string luck28 = new BCW.JS.somejs().daojishi("luck28", luck.EndTime);
            //    builder.Append("距" + luck.ID + "期开奖还有" + luck28 + "开奖");
            //}
        }
        else
        {
            builder.Append("距" + luck.ID + "期正在开奖，请刷新页面。。");//等刷新机刷新开奖
        }
    }


    //快捷下注
    private void kuai(int type, int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!new BCW.XinKuai3.BLL.Public_User().Exists(meid, type))//添加默认的快捷下注
        {
            BCW.XinKuai3.Model.Public_User model = new BCW.XinKuai3.Model.Public_User();
            model.UsID = meid;
            model.UsName = new BCW.BLL.User().GetUsName(meid);
            model.Type = 2;//1新快3、2幸运28、3好彩一、4进球彩、5百家乐
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
                gold = ChangeToWanSSC(k[i]);
                //builder.Append("<a href =\"" + Utils.getUrl("luck28.aspx?act=pay&amp;ptype=" + ptype + "&amp;faslInput=" + Convert.ToInt64(k[i]) + "") + "\">" + gold + "</a>" + "|");

                builder.Append("<input class=\"btn\" type=\"submit\" name=\"BuyCent" + i + "\" ptype=\"" + ptype + "\" BuyCent=\"" + (k[i]) + "\" value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
            }
        }

        builder.Append("<a href=\"" + Utils.getUrl("Public_set.aspx?type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");
    }

    private string ChangeToWanSSC(string str)
    {
        string CW = string.Empty;
        try
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
                    CW = first.ToString();
                }
            }
            else
            {
                CW = first.ToString();
            }
        }
        catch { }
        return CW;
    }
}