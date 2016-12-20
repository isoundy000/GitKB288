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
using BCW.Data;

public partial class bbs_game_xk3 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/xinkuai3.xml";
    #region
    protected string GameName = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//游戏名字
    protected long Sum = Convert.ToInt64(ub.GetSub("XinKuai3Sum1", "/Controls/xinkuai3.xml"));//和值1倍数//和值10、11赔率
    protected long Sum2 = Convert.ToInt64(ub.GetSub("XinKuai3Sum2", "/Controls/xinkuai3.xml"));//和值2倍数//和值4、17赔率
    protected long XSum1 = Convert.ToInt64(ub.GetSub("XSum1", "/Controls/xinkuai3.xml"));//和值5、16赔率
    protected long XSum2 = Convert.ToInt64(ub.GetSub("XSum2", "/Controls/xinkuai3.xml"));//和值6、15赔率
    protected long XSum3 = Convert.ToInt64(ub.GetSub("XSum3", "/Controls/xinkuai3.xml"));//和值7、14赔率
    protected long XSum4 = Convert.ToInt64(ub.GetSub("XSum4", "/Controls/xinkuai3.xml"));//和值8、13赔率
    protected long XSum5 = Convert.ToInt64(ub.GetSub("XSum5", "/Controls/xinkuai3.xml"));//和值9、12赔
    protected long Three_Same_All = Convert.ToInt64(ub.GetSub("XinKuai3Three_Same_All", "/Controls/xinkuai3.xml"));//三同号通选倍数
    protected long Three_Same_Single = Convert.ToInt64(ub.GetSub("XinKuai3Three_Same_Single", "/Controls/xinkuai3.xml"));//三同号单选倍数
    protected long Three_Same_Not = Convert.ToInt64(ub.GetSub("XinKuai3Three_Same_Not", "/Controls/xinkuai3.xml"));//三不同号倍数
    protected long Three_Continue_All = Convert.ToInt64(ub.GetSub("XinKuai3Three_Continue_All", "/Controls/xinkuai3.xml"));//三连号通选倍数
    protected long Two_Same_All = Convert.ToInt64(ub.GetSub("XinKuai3Two_Same_All", "/Controls/xinkuai3.xml"));//二同号复选倍数
    protected long Two_Same_Single = Convert.ToInt64(ub.GetSub("XinKuai3Two_Same_Single", "/Controls/xinkuai3.xml"));//二同号单选倍数
    protected long Two_dissame = Convert.ToInt64(ub.GetSub("XinKuai3Two_dissame", "/Controls/xinkuai3.xml"));//二不同号倍数
    protected float Da = float.Parse(ub.GetSub("Xda1", "/Controls/xinkuai3.xml"));//大
    protected float Xiao = float.Parse(ub.GetSub("Xxiao1", "/Controls/xinkuai3.xml"));//小
    protected float Dan = float.Parse(ub.GetSub("Xdan1", "/Controls/xinkuai3.xml"));//单
    protected float Shuang = float.Parse(ub.GetSub("Xshuang1", "/Controls/xinkuai3.xml"));//双
    protected string XtestID = (ub.GetSub("XtestID", "/Controls/xinkuai3.xml"));//试玩ID
    protected int shouxu = Convert.ToInt32(ub.GetSub("shouxu", "/Controls/xinkuai3.xml"));//手续费
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        //{
        //    Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "xk3.aspx?ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        //}
        //维护提示
        if (ub.GetSub("xk3Status", xmlPath) == "1")//维护
        {
            Utils.Safe("此游戏");
        }

        if (ub.GetSub("xk3Status", xmlPath) == "2")//内测
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
            if (XtestID != "")
            {
                string[] sNum = XtestID.Split('#');
                int sbsy = 0;
                for (int a = 0; a < sNum.Length; a++)
                {
                    int tid = 0;
                    int.TryParse(sNum[a].Trim(), out tid);
                    if (meid == tid)
                    {
                        sbsy++;
                    }
                }
                if (sbsy == 0)
                {
                    Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
                }
            }
        }

        //更新浮动
        UpdateFodong();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":
                RulePage();//---------------游戏规则
                break;
            case "top":
                TopPage();//----------------排行榜
                break;
            case "info":
                InfoPagePage();//-----------投注显示
                break;
            case "pay":
                PayPage();//----------------判断投注
                break;
            case "list":
                ListPage();//---------------历史记录
                break;
            case "listview":
                ListViewPage();//-----------历史记录--详细
                break;
            case "view":
                ListViewPage2();//-----------历史记录--详细_前台消费记录进来 邵广林 20161019
                break;
            case "mylist":
                MyListPage();//-------------投注历史
                break;
            case "mylistview":
                MyListViewPage();//---------投注历史--详细
                break;
            case "case":
                CasePage();//---------------开始兑奖页面
                break;
            case "caseok":
                CaseOkPage();//-------------单个兑奖
                break;
            case "casepost":
                CasePostPage();//-----------多个兑奖
                break;
            case "analyze":
                AnalyzePage();//------------往期分析
                break;
            default:
                ReloadPage();//-------------首页
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //顶部ubb
        string Foot1 = ub.GetSub("XinKuai3top1", xmlPath);
        if (Foot1 != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot1)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        //图片gif
        builder.Append(Out.Tab("<div>", ""));
        string gif1 = ub.GetSub("logo", xmlPath);
        //string show_logo = string.Format(@"<img src=\{0}  />", gif1);
        string show_logo = "<img src=\"" + gif1 + "\" alt=\"load\"/>";
        builder.Append(show_logo);
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        //更新期数
        UpdateState();
        //读取最后一期
        string _where = "Order by Lottery_time desc";
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where);
        //读取最后一期不为空
        string _where0 = "WHERE Lottery_num!='' Order by Lottery_time desc";
        BCW.XinKuai3.Model.XK3_Internet_Data model0 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where0);
        string issue1 = string.Empty;
        try
        {
            issue1 = model.Lottery_issue;//本期开奖期号
        }
        catch
        {
            Utils.Error("系统尚未初始化...", "");
        }
        string issue3 = Utils.Right(model.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
        //int Sec = Utils.ParseInt(ub.GetSub("xSec", xmlPath));
        int dnu = 0;
        if (IsOpen() == true)
        {
            if (model.Lottery_time.AddMinutes(10) < DateTime.Now)
            {
                int count = model.Lottery_issue.Length - 2;
                if (model.Lottery_issue.Substring(count, 2) == "78")
                {
                    string dsb = string.Empty;
                    if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                    {
                        dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                    }
                    else
                    {
                        dsb = DateTime.Now.ToString("yyMMdd");
                    }
                    dnu = int.Parse(dsb + "001");
                    string xk3 = new BCW.JS.somejs().newDaojishi("xk3", model.Lottery_time.AddHours(11).AddMinutes(10));
                    builder.Append("第" + dnu + "期.投注" + xk3 + "");
                    //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    //{
                    //    string xk3 = new BCW.JS.somejs().daojishi2("xk3", model.Lottery_time.AddHours(11).AddMinutes(10));
                    //    builder.Append("第" + dnu + "期.投注" + xk3 + "");
                    //}
                    //else
                    //{
                    //    string xk3 = new BCW.JS.somejs().daojishi("xk3", model.Lottery_time.AddHours(11).AddMinutes(10));
                    //    builder.Append("第" + dnu + "期.投注" + xk3 + "");
                    //}
                }
                else
                {
                    builder.Append("正在获取下一期信息...");
                }
            }
            else
            {
                //builder.Append("==2==" + (int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue)) + "<br/>");
                int LiangTing = 2;
                if (int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue) <= LiangTing)//|| int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue) > 100
                {
                    builder.Append("第" + model.Lottery_issue + "期.投注");
                    string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", model.Lottery_time);
                    builder.Append("" + daojishi + "" + "");
                }
                else
                {
                    if (issue3 == "078")
                    {
                        string dsb = DateTime.Now.ToString("yyMMdd");
                        if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                        {
                            dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                        }
                        else
                        {
                            dsb = DateTime.Now.ToString("yyMMdd");
                        }
                        dnu = int.Parse(dsb + "001");
                        builder.Append("第" + dnu + "期.没开奖,请稍候购买");
                    }
                    else
                    {
                        if (issue3 == "001")
                        {
                            string xk9 = new BCW.JS.somejs().newDaojishi("xk9", model.Lottery_time);
                            builder.Append("第" + model.Lottery_issue + "期.投注" + xk9 + "");
                        }
                        else if (issue3 == "002")
                        {
                            builder.Append("第" + model.Lottery_issue + "期.投注");
                            string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", model.Lottery_time);
                            builder.Append("" + daojishi + "" + "");
                        }
                        else
                        {
                            if (Utils.Right(model0.Lottery_issue.ToString(), 3) == "078")
                            {
                                string dsb = DateTime.Now.ToString("yyMMdd");
                                {
                                    dsb = DateTime.Now.ToString("yyMMdd");
                                }
                                dnu = int.Parse(dsb + "001");
                                builder.Append("第" + dnu + "期.没开奖,请稍候购买");
                            }
                            else
                                builder.Append("第" + (int.Parse(model0.Lottery_issue) + 1) + "期.没开奖,请稍候购买");
                        }

                    }
                }
            }
        }
        else
        {
            //builder.Append("游戏开放时间:" + ub.GetSub("xOnTime", xmlPath) + "");
            if (issue3 == "078")
            {
                string dsb = string.Empty;
                if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                {
                    dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                }
                else
                {
                    dsb = DateTime.Now.ToString("yyMMdd");
                }
                dnu = int.Parse(dsb + "001");
                string xk4 = new BCW.JS.somejs().newDaojishi("xk4", model.Lottery_time.AddHours(11).AddMinutes(10));
                builder.Append("第" + dnu + "期.投注" + xk4 + "");
            }
            else
            {
                builder.Append("还没到销售时间.");
            }
        }
        builder.Append("<br />");
        //显示数据库最后一期有开奖号码的数据
        string _where2 = "WHERE Lottery_num!='' Order by Lottery_issue desc";
        BCW.XinKuai3.Model.XK3_Internet_Data m = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where2);
        if (m != null)
        {
            builder.Append("第" + m.Lottery_issue + "期.开奖:<a href=\"" + Utils.getUrl("xk3.aspx?act=listview&amp;id=" + m.ID + "") + "\">" + m.Lottery_num + "</a><br />");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist") + "\">未开</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=top") + "\">排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=analyze") + "\">分析</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("财产:<a href=\"" + Utils.getUrl("../finance.aspx") + "\">" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "</a>" + ub.Get("SiteBz") + "|");
        if (Utils.GetDomain().Contains("kb288"))
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3sw.aspx") + "\">试玩</a>|");
        }
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">刷新</a><br/>");
        builder.Append(Out.Tab("</div>", ""));


        //投注方式
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=9") + "\">押注大小</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=10") + "\">押注单双</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=1") + "\">总和复选</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=6") + "\">对子复选</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=2") + "\">任意豹子</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=3") + "\">直选豹子</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=4") + "\">复选任三</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=8") + "\">复选任二</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=5") + "\">三连通选</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=7") + "\">组二单选</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓历史开奖〓<br />");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取历史开奖
        int pageIndex;
        int recordCount;
        int pageSize = 3;
        string strWhere = string.Empty;
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        strWhere = "Lottery_num!='' ";
        IList<BCW.XinKuai3.Model.XK3_Internet_Data> listXK3list = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Datas(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3list.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Internet_Data n in listXK3list)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.AppendFormat("{0}期:<a href=\"" + Utils.getUrl("xk3.aspx?act=listview&amp;id=" + n.ID + "") + "\">{1}</a>", n.Lottery_issue, n.Lottery_num);
                string op = string.Empty;
                try
                {
                    string[] arrNum = Utils.ArrBySeparated(n.Lottery_num, 1);
                    op = arrNum[0];
                }
                catch { }
                string strDxs = string.Empty;
                if (n.DaXiao == "0")
                {
                    strDxs += "豹子" + op;
                }
                else
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        strDxs += "大";
                    }
                    if (n.DaXiao == "2")
                    {
                        strDxs += "小";
                    }
                    if (n.DanShuang == "1")
                    {
                        strDxs += "单";
                    }
                    if (n.DanShuang == "2")
                    {
                        strDxs += "双";
                    }
                }
                builder.Append(strDxs);
                builder.Append(Out.Tab("</div>", "<br />"));
                k++;
            }
            if (k > pageSize)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=list") + "\">更多开奖记录&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无开奖记录.");
            builder.Append(Out.Tab("</div>", ""));
        }


        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("〓玩家动态〓");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取动态列表
        int SizeNum_Action = 3;
        string strWhere_Action = "Types=1001";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum_Action, strWhere_Action);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum_Action)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=1001") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无动态记录.");
            builder.Append(Out.Tab("</div>", ""));
        }

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(27, "xk3.aspx", 5, 0)));


        //游戏底部Ubb
        string Foot = ub.GetSub("XinKuai3Foot", xmlPath);
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

    //规则
    private void RulePage()
    {
        Master.Title = "" + GameName + "_玩法规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;玩法规则");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>本" + GameName + "根据福利彩票(新快3)转换成虚拟游戏,开奖时间、结果均与《新快3》相同.</b><br />");
        builder.Append("<b>" + GameName + "每隔10分钟一期，每天78期，游戏开放时间：9:26~22:26。</b><br />");
        builder.Append("" + GameName + "游戏根据号码组合共分为“总和”、“三同号”、“二同号”、“任三”、“任二”、“豹子”、“大小单双”投注方式，具体规定如下：<br />");
        builder.Append("<b>（一）" + OutType(1) + "：</b>是指对三个号码的和值进行投注包括“和值3”至“和值18”投注；开奖号码相加之和与选择和值号码一致，<b>即中奖" + Sum + "-" + Sum2 + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：113 , 选号：和值 5.<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;和值中奖倍数分别为：4和17(" + Sum2 + ")倍、5和16(" + XSum1 + ")倍、6和15(" + XSum2 + ")倍、7和14(" + XSum3 + ")倍、8和13(" + XSum4 + ")倍、9和12(" + XSum5 + ")倍、10和11(" + Sum + ")倍.<br />");
        builder.Append("<b>（二）三同号投注：</b>是指对三个相同的号码进行投注，具体分为：<br />");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<b>1、</b>" + OutType(2) + "：是指对所有相同的三个号码（111、222、…、666）进行投注；开出三个号码相同，<b>即中奖" + Three_Same_All + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：222 , 选号：三同号通选.<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<b>2、</b>" + OutType(3) + "：是指从所有相同的三个号码（111、222、…、666）中任意选择一组号码进行投注；开出三个号码相同，且与选择号码完全一致，<b>即中奖" + Three_Same_Single + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：222  , 选号：222.<br/>");
        builder.Append("<b>（三）二同号投注：</b>是指对两个指定的相同号码进行投注，具体分为：<br />");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<b>1、</b>" + OutType(6) + "：是指对三个号码中两个指定的相同号码和一个任意号码进行投注；选择号码与开奖号码中包含的相同号码一致，<b>即中奖" + Two_Same_All + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：116 , 选号：11*.<br />");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<b>2、</b>" + OutType(7) + "：是指对三个号码中两个指定的相同号码和一个指定的不同号码进行投注；已选择的相同号码、不同号码均与开奖号码的相同号、不同号完全一致，<b>即中奖" + Two_Same_Single + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：225 ， 选号：225.<br />");
        builder.Append("<b>（四）" + OutType(4) + "：</b>是指对三个各不相同的号码进行投注；选择的3个不同号码与开奖号码的三个不同号码完全一致，<b>即中奖" + Three_Same_Not + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：开奖：245 ， 选号：245.<br />");
        builder.Append("<b>（五）" + OutType(8) + "：</b>是指对三个号码中两个指定的不同号码和一个任意号码进行投注；选择2个不同号码与开奖号码中的不同号码一致，<b>即中奖" + Two_dissame + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：156 ， 选号16.<br />");
        builder.Append("<b>（六）" + OutType(5) + "：</b>是指对所有三个相连的号码（仅限：123、234、345、456）进行投注；选择三连号通选，开号码为三个连续号码，<b>即中奖" + Three_Continue_All + "倍.</b><br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：234.<br />");
        builder.Append("<b>（七）" + OutType(9) + "：</b>是指对三个号码相加的和值进行投注；若相加少于等于10，即为小中奖" + Xiao + "倍；大于等于11，即为大中奖" + Da + "倍。<b></b>若开奖的三个号码一致，即不中奖.<br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：234,中奖为小。开奖：222，不中奖.<br />");
        builder.Append("<b>（八）" + OutType(10) + "：</b>是指对三个号码相加的和值进行投注；若和值个位数为奇数，即为单中奖" + Dan + "，若为偶数，即为双中奖" + Shuang + "倍。<b></b>若开奖的三个号码一致，即不中奖.<br />&nbsp;&nbsp;&nbsp;&nbsp;例如：开奖：234，中奖为单。开奖：222，不中奖.<br/>");
        builder.Append("<b>温馨提示：大小单双返奖时，均按照当时下注时的赔率为准.</b>");
        if (shouxu > 0)
        {
            builder.Append("<b>系统每局收取中奖的千分之" + shouxu + "手续费。</b>");
        }
        builder.Append(Out.Tab("</div>", ""));
        GameFoot();
    }

    //排行榜
    private void TopPage()
    {
        Master.Title = "" + GameName + "_排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">游戏赌神" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=top&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏赌神</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">土 豪 榜 " + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=top&amp;ptype=2&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">土 豪 榜</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] name2 = XtestID.Split('#');//测试id
        string b = "";
        if (name2.Length > 0)
        {
            for (int n = 0; n < name2.Length; n++)
            {
                if (n == name2.Length - 1)
                {
                    b = b + name2[n];
                }
                else
                {
                    b = b + name2[n] + ",";
                }
            }
        }


        string rewardid = "";
        int pageIndex = 1;
        if (ptype == 1)
        {
            #region
            int recordCount;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            string strWhere4 = string.Empty;
            int pageSize = 10;
            string[] pageValUrl = { "act", "startstate2", "endstate2", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (pageIndex > 10)
                pageIndex = 10;
            if (Utils.GetDomain().Contains("kb288"))
            {
                if (b != "")
                {
                    strWhere = "UsID not IN (" + b + ") and state>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";//and UsID not IN (" + a + "," + b + ") 
                }
                else
                {
                    strWhere = "state>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";
                }
            }
            else
                strWhere = "state>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";

            strWhere2 = "UsID,Sum(GetMoney-PutGold) as bb";//TOP(100) 
            strWhere3 = "UsID,sum(GetMoney-PutGold) AS'bb' into #bang3";
            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.XinKuai3.BLL.XK3_Bet().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.XinKuai3.BLL.XK3_Bet().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净赢<h style=\"color:red\">" + usmoney + "</h>" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                if (recordCount >= 100)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        else
        {
            #region
            int recordCount;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            string strWhere4 = string.Empty;
            int pageSize = 10;
            string[] pageValUrl = { "act", "startstate2", "endstate2", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (pageIndex > 10)
                pageIndex = 10;
            if (Utils.GetDomain().Contains("kb288"))
            {
                if (b != "")
                {
                    strWhere = "GetMoney>0 and UsID not IN (" + b + ") and state>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";//and UsID not IN (" + a + "," + b + ") 
                }
                else
                {
                    strWhere = "GetMoney>0 and state>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";
                }
            }
            else
                strWhere = "GetMoney>0 and state>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";

            strWhere2 = "UsID,Sum(GetMoney) as bb";//TOP(100) 

            strWhere3 = "UsID,sum(GetMoney) AS'bb' into #bang3";

            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.XinKuai3.BLL.XK3_Bet().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.XinKuai3.BLL.XK3_Bet().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>赚币<h style=\"color:red\">" + usmoney + "</h>" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                if (recordCount >= 100)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }

        //邵广林 20160802 修改前台排行榜为读取用户下注数据
        #region 
        //int pageIndex;
        //int recordCount;
        //int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        //string[] pageValUrl = { "act", "ptype", "backurl" };
        //pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        //if (pageIndex == 0)
        //    pageIndex = 1;
        //string strWhere = string.Empty;
        //if (Utils.GetDomain().Contains("kb288"))
        //    strWhere = "UsId not IN (" + a + ")";
        //else
        //    strWhere = "";

        //string strOrder = "";
        ////查询条件
        //if (ptype == 1)
        //{
        //    strOrder = "(WinGold-PutGold) Desc";
        //}
        //if (ptype == 2)
        //{
        //    strOrder = "(WinGold) Desc";
        //}

        //// 开始读取列表
        //IList<BCW.XinKuai3.Model.XK3_Toplist> listXK3pay = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_Toplists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        //if (listXK3pay.Count > 0)
        //{
        //    int k = 1;
        //    foreach (BCW.XinKuai3.Model.XK3_Toplist n in listXK3pay)
        //    {
        //        if (k % 2 == 0)
        //            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //        else
        //        {
        //            if (k == 1)
        //                builder.Append(Out.Tab("<div>", ""));
        //            else
        //                builder.Append(Out.Tab("<div>", "<br />"));
        //        }
        //        if (ptype == 1)
        //        {
        //            builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>净赢<h style=\"color:red\">" + (n.WinGold - n.PutGold) + "</h>" + ub.Get("SiteBz") + "");
        //        }
        //        if (ptype == 2)
        //        {
        //            builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>赚币<h style=\"color:red\">" + (n.WinGold) + "</h>" + ub.Get("SiteBz") + "");
        //        }
        //        k++;
        //        builder.Append(Out.Tab("</div>", ""));
        //    }

        //    // 分页
        //    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
        //}
        //else
        //{
        //    builder.Append(Out.Div("div", "没有相关记录.."));
        //}
        #endregion


        //我的战绩
        int meid = new BCW.User.Users().GetUsId();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "查询战绩")
        {
            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            string _where1 = string.Empty;
            string _where2 = string.Empty;
            _where1 = "(sum(GetMoney-PutGold)) as aa,UsID";
            _where2 = "Input_Time>='" + searchday1 + "'and Input_Time<'" + searchday2 + "' and UsID='" + meid + "'  GROUP BY UsID";

            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList(_where1, _where2);
            long Cents = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["aa"]);
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<hr/>");
            if (Cents == 48)
                builder.Append("我的战绩:(" + searchday1 + "至" + searchday2 + ")<br/>盈利<h style=\"color:red\">0</h>" + ub.Get("SiteBz") + "");
            else
                builder.Append("我的战绩:(" + searchday1 + "至" + searchday2 + ")<br/>盈利<h style=\"color:red\">" + Cents + "</h>" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "查询战绩,xk3.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string price = string.Empty;
            if (meid != 0)
            {
                DataSet model_1 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("sum(GetMoney-PutGold) as aa", "UsID='" + meid + "'");
                for (int i = 0; i < model_1.Tables[0].Rows.Count; i++)
                {
                    price = (model_1.Tables[0].Rows[0]["aa"].ToString());
                }
            }
            if (price == "")
            {
                price = "0";
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<hr/>");
            builder.Append("我的战绩:总盈利<h style=\"color:red\">" + price + "</h>" + ub.Get("SiteBz") + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "查询战绩,xk3.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        GameFoot();
    }

    //历史开奖
    private void ListPage()
    {
        Master.Title = "" + GameName + "_历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;历史开奖");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere = string.Empty;
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Lottery_num!='' ";
        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Internet_Data> listXK3list = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Datas(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3list.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Internet_Data n in listXK3list)
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

                builder.AppendFormat("{0}期:<a href=\"" + Utils.getUrl("xk3.aspx?act=listview&amp;id=" + n.ID + "") + "\">{1}</a>", n.Lottery_issue, n.Lottery_num);
                string op = string.Empty;
                try
                {
                    string[] arrNum = Utils.ArrBySeparated(n.Lottery_num, 1);
                    op = arrNum[0];
                }
                catch { }
                string strDxs = string.Empty;
                if (n.DaXiao == "0")
                {
                    strDxs += "豹子" + op;
                }
                else
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        strDxs += "大";
                    }
                    if (n.DaXiao == "2")
                    {
                        strDxs += "小";
                    }
                    if (n.DanShuang == "1")
                    {
                        strDxs += "单";
                    }
                    if (n.DanShuang == "2")
                    {
                        strDxs += "双";
                    }
                }
                builder.Append(strDxs);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关开奖记录.."));
        }

        GameFoot();
    }

    //历史开奖——详细页面
    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Data(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        int aa = Int32.Parse(model.Lottery_issue);

        Master.Title = "第" + model.Lottery_issue + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=list") + "\">历史开奖</a>&gt;详细");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere += "Lottery_issue='" + model.Lottery_issue + "' and GetMoney>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bets(pageIndex, pageSize, strWhere, out recordCount);

        if (listXK3pay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.Lottery_issue + "期开出:" + model.Lottery_num + "");
            string strDxs = string.Empty;
            if (model.DaXiao == "0")
            {
                strDxs += "豹子";
            }
            else
            {
                //1大2小、1单2双
                if (model.DaXiao == "1")
                {
                    strDxs += "大";
                }
                if (model.DaXiao == "2")
                {
                    strDxs += "小";
                }
                if (model.DanShuang == "1")
                {
                    strDxs += "单";
                }
                if (model.DanShuang == "2")
                {
                    strDxs += "双";
                }
            }
            builder.Append(strDxs);

            builder.Append("<br />共" + recordCount + "注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet n in listXK3pay)
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


                string Getnum = string.Empty;
                if (n.Play_Way == 1)
                {
                    Getnum = n.Sum;//和值
                }
                else if (n.Play_Way == 2)
                {
                    //Getnum = n.Three_Same_All;//三同号通选
                    Getnum = "111、222、333、444、555、666";
                }
                else if (n.Play_Way == 3)
                {
                    Getnum = n.Three_Same_Single;//三同号单选
                }
                else if (n.Play_Way == 4)
                {
                    Getnum = n.Three_Same_Not;//三不同号
                }
                else if (n.Play_Way == 5)
                {
                    //Getnum = n.Three_Continue_All;//三连号通选
                    Getnum = "123、234、345、456";
                }
                else if (n.Play_Way == 6)
                {
                    Getnum = n.Two_Same_All;//二同号复选
                }
                else if (n.Play_Way == 7)
                {
                    Getnum = n.Two_Same_Single;//二同号单选
                }
                else if (n.Play_Way == 8)
                {
                    Getnum = n.Two_dissame;//二不同号
                }
                else if (n.Play_Way == 9)//大小
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        Getnum = "大";
                    }
                    else if (n.DaXiao == "2")
                    {
                        Getnum = "小";
                    }
                }
                else if (n.Play_Way == 10)//双单
                {
                    if (n.DanShuang == "2")
                    {
                        Getnum = "双";
                    }
                    else if (n.DanShuang == "1")
                    {
                        Getnum = "单";
                    }
                }

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>：");
                //builder.Append("" + OutType(n.Play_Way) + ".");//：" + Getnum + "/每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注/共投" + n.PutGold + "酷币/投注时间:[" + DT.FormatDate(n.Input_Time, 1) + "]
                if (n.GetMoney > 0)
                {
                    builder.Append("共赢了：" + n.GetMoney + " " + ub.Get("SiteBz") + ".");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无投注记录.."));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist&amp;ptype=2") + "\">历史投注>></a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">马上投注>></a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }


    //历史开奖——详细页面
    private void ListViewPage2()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Data2(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        int aa = Int32.Parse(model.Lottery_issue);

        Master.Title = "第" + model.Lottery_issue + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=list") + "\">历史开奖</a>&gt;详细");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere += "Lottery_issue='" + model.Lottery_issue + "' and GetMoney>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bets(pageIndex, pageSize, strWhere, out recordCount);

        if (listXK3pay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.Lottery_issue + "期开出:" + model.Lottery_num + "");
            string strDxs = string.Empty;
            if (model.DaXiao == "0")
            {
                strDxs += "豹子";
            }
            else
            {
                //1大2小、1单2双
                if (model.DaXiao == "1")
                {
                    strDxs += "大";
                }
                if (model.DaXiao == "2")
                {
                    strDxs += "小";
                }
                if (model.DanShuang == "1")
                {
                    strDxs += "单";
                }
                if (model.DanShuang == "2")
                {
                    strDxs += "双";
                }
            }
            builder.Append(strDxs);

            builder.Append("<br />共" + recordCount + "注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet n in listXK3pay)
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


                string Getnum = string.Empty;
                if (n.Play_Way == 1)
                {
                    Getnum = n.Sum;//和值
                }
                else if (n.Play_Way == 2)
                {
                    //Getnum = n.Three_Same_All;//三同号通选
                    Getnum = "111、222、333、444、555、666";
                }
                else if (n.Play_Way == 3)
                {
                    Getnum = n.Three_Same_Single;//三同号单选
                }
                else if (n.Play_Way == 4)
                {
                    Getnum = n.Three_Same_Not;//三不同号
                }
                else if (n.Play_Way == 5)
                {
                    //Getnum = n.Three_Continue_All;//三连号通选
                    Getnum = "123、234、345、456";
                }
                else if (n.Play_Way == 6)
                {
                    Getnum = n.Two_Same_All;//二同号复选
                }
                else if (n.Play_Way == 7)
                {
                    Getnum = n.Two_Same_Single;//二同号单选
                }
                else if (n.Play_Way == 8)
                {
                    Getnum = n.Two_dissame;//二不同号
                }
                else if (n.Play_Way == 9)//大小
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        Getnum = "大";
                    }
                    else if (n.DaXiao == "2")
                    {
                        Getnum = "小";
                    }
                }
                else if (n.Play_Way == 10)//双单
                {
                    if (n.DanShuang == "2")
                    {
                        Getnum = "双";
                    }
                    else if (n.DanShuang == "1")
                    {
                        Getnum = "单";
                    }
                }

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>：");

                builder.Append("" + OutType(n.Play_Way) + ".");//：" + Getnum + "/每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注/共投" + n.PutGold + "酷币/投注时间:[" + DT.FormatDate(n.Input_Time, 1) + "]
                if (n.GetMoney > 0)
                {
                    builder.Append("共赢了：" + n.GetMoney + " " + ub.Get("SiteBz") + ".");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无投注记录.."));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist&amp;ptype=2") + "\">历史投注>></a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">马上投注>></a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //未开和历史投注
    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "未开投注";
        else
            strTitle = "历史投注";

        Master.Title = "" + GameName + "_" + strTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;" + strTitle + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        string Gettou = "";
        strWhere = "UsID=" + meid + "";
        if (ptype == 1)
            strWhere += " and State=0";
        else
            strWhere += " and State>0";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string XK3qi = "";
        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bets(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3pay.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet n in listXK3pay)
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
                if (n.Lottery_issue.ToString() != XK3qi)
                {
                    BCW.XinKuai3.Model.XK3_Internet_Data model_num1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(n.Lottery_issue);
                    builder.Append("=第" + n.Lottery_issue + "期" + model_num1.Lottery_num + "");
                    string strDxs = string.Empty;
                    if (model_num1.DaXiao == "0")
                    {
                        strDxs += "豹子";
                    }
                    else
                    {
                        //1大2小、1单2双
                        if (model_num1.DaXiao == "1")
                        {
                            strDxs += "大";
                        }
                        if (model_num1.DaXiao == "2")
                        {
                            strDxs += "小";
                        }
                        if (model_num1.DanShuang == "1")
                        {
                            strDxs += "单";
                        }
                        if (model_num1.DanShuang == "2")
                        {
                            strDxs += "双";
                        }
                    }
                    builder.Append(strDxs + "=<br />");
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (n.Play_Way == 1)
                {
                    Gettou = n.Sum;
                }
                if (n.Play_Way == 2)
                {
                    //Gettou = n.Three_Same_All;
                    Gettou = "111、222、333、444、555、666";
                }
                if (n.Play_Way == 3)
                {
                    Gettou = n.Three_Same_Single;
                }
                if (n.Play_Way == 4)
                {
                    Gettou = n.Three_Same_Not;
                }
                if (n.Play_Way == 5)
                {
                    //Gettou = n.Three_Continue_All;
                    Gettou = "123、234、345、456";
                }
                if (n.Play_Way == 6)
                {
                    Gettou = n.Two_Same_All;
                }
                if (n.Play_Way == 7)
                {
                    Gettou = n.Two_Same_Single;
                }
                if (n.Play_Way == 8)
                {
                    Gettou = n.Two_dissame;
                }
                if (n.Play_Way == 9)
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        Gettou = "大";
                    }
                    else if (n.DaXiao == "2")
                    {
                        Gettou = "小";
                    }
                }
                if (n.Play_Way == 10)
                {
                    if (n.DanShuang == "2")
                    {
                        Gettou = "双";
                    }
                    else if (n.DanShuang == "1")
                    {
                        Gettou = "单";
                    }
                }
                builder.Append("[" + OutType(n.Play_Way) + "]号码为{" + (Gettou) + "}每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注/共投" + n.PutGold + ub.Get("SiteBz") + "/投注时间:[" + DT.FormatDate(n.Input_Time, 1) + "].");
                if (n.GetMoney > 0)
                {
                    if (n.State == 2)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(已领奖)");
                    }
                    else if (n.State == 3)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(过期未领奖)");
                    }
                    else
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(未领奖)");
                }
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylistview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">详情&gt;&gt;</a>");

                XK3qi = n.Lottery_issue.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", ""));
        GameFoot();
    }

    //未开和历史投注——详细页面
    private void MyListViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        //根据期数的id，去查询数据库是否投注
        BCW.XinKuai3.Model.XK3_Bet n = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(id);
        if (n == null || n.UsID != meid)
        {
            Utils.Error("不存在投注的记录", "");
        }
        //查询该id对应的开奖号码
        BCW.XinKuai3.Model.XK3_Internet_Data p = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(n.Lottery_issue);
        if (p == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + n.Lottery_issue + "期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("xk3.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>&gt;第" + n.Lottery_issue + "期");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (p.Lottery_num == "")
            builder.Append("开奖号码:未开奖");
        else
        {
            builder.Append("开奖号码:" + p.Lottery_num + "");
            string strDxs = string.Empty;
            if (p.DaXiao == "0")
            {
                strDxs += "豹子";
            }
            else
            {
                //1大2小、1单2双
                if (p.DaXiao == "1")
                {
                    strDxs += "大";
                }
                if (p.DaXiao == "2")
                {
                    strDxs += "小";
                }
                if (p.DanShuang == "1")
                {
                    strDxs += "单";
                }
                if (p.DanShuang == "2")
                {
                    strDxs += "双";
                }
            }
            builder.Append(strDxs);
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string Getnum = string.Empty;
        if (n.Play_Way == 1)
        {
            Getnum = n.Sum;//和值
        }
        else if (n.Play_Way == 2)
        {
            //Getnum = n.Three_Same_All;//三同号通选
            Getnum = "111、222、333、444、555、666";
        }
        else if (n.Play_Way == 3)
        {
            Getnum = n.Three_Same_Single;//三同号单选
        }
        else if (n.Play_Way == 4)
        {
            Getnum = n.Three_Same_Not;//三不同号
        }
        else if (n.Play_Way == 5)
        {
            //Getnum = n.Three_Continue_All;//三连号通选
            Getnum = "123、234、345、456";
        }
        else if (n.Play_Way == 6)
        {
            Getnum = n.Two_Same_All;//二同号复选
        }
        else if (n.Play_Way == 7)
        {
            Getnum = n.Two_Same_Single;//二同号单选
        }
        else if (n.Play_Way == 8)
        {
            Getnum = n.Two_dissame;//二不同号
        }
        else if (n.Play_Way == 9)//大小
        {
            //1大2小、1单2双
            if (n.DaXiao == "1")
            {
                Getnum = "大";
            }
            else if (n.DaXiao == "2")
            {
                Getnum = "小";
            }
        }
        else if (n.Play_Way == 10)//双单
        {
            if (n.DanShuang == "2")
            {
                Getnum = "双";
            }
            else if (n.DanShuang == "1")
            {
                Getnum = "单";
            }
        }
        if (n.Play_Way == 9 || n.Play_Way == 10)
        {
            builder.Append("当前购买类型:" + OutType(n.Play_Way) + "<br />当前购买号码:" + Getnum + "<br />每注:" + n.Zhu_money + "" + ub.Get("SiteBz") + "<br />注数:" + n.Zhu + "注<br />");
            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "UsID=" + meid + " and id=" + id + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append("当前投注赔率:" + (ds.Tables[0].Rows[0]["Odds"].ToString()) + "<br/>");
            }
            builder.Append("花费:" + n.PutGold + "" + ub.Get("SiteBz") + "");
        }
        else
            builder.Append("当前购买类型:" + OutType(n.Play_Way) + "<br />当前购买号码:" + Getnum + "<br />每注:" + n.Zhu_money + "" + ub.Get("SiteBz") + "<br />注数:" + n.Zhu + "注<br />花费:" + n.PutGold + "" + ub.Get("SiteBz") + "");
        builder.Append("<br />投注时间:" + DT.FormatDate(n.Input_Time, 0) + "");
        if (n.GetMoney > 0)
        {
            if (n.State == 3)
            {
                builder.Append("<br />结果:已过兑奖时间。系统自动收回所得" + ub.Get("SiteBz") + "。");
            }
            else
                builder.Append("<br />结果:<h style=\"color:red\">赢:&nbsp;" + n.GetMoney + "&nbsp;" + ub.Get("SiteBz") + "</h>");
        }
        else
        {
            if (n.State == 0)
                builder.Append("<br />结果:未开奖");
            else if (n.State == 1)
            {
                if (p.DaXiao == "0")
                {
                    builder.Append("<br />结果:未中奖，原因：3同号，大小双单通食。");
                }
                else
                    builder.Append("<br />结果:未中奖");
            }


        }
        builder.Append(Out.Tab("</div>", ""));

        if (n.State == 1)
        {
            if (n.GetMoney > 0)
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=case") + "\">&lt;&lt;马上兑奖</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        GameFoot();
    }

    //投注方式
    private void InfoPagePage()
    {
        //判断登录
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //判断彩版
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 2, @"^[1-9]$|^10$|^11$|^12$", "类型选择错误"));
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            //Utils.Success("温馨提示", "使用彩版，投注更直观，更快捷！正在进入...", "xk3.aspx?act=info&amp;ptype=" + ptype + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }
        //显示标题的投注方式
        string TypeTitle = OutType(ptype);
        Master.Title = TypeTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;" + TypeTitle + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        //读取最后一期
        string _where = "Order by Lottery_time desc";
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where);
        //读取最后一期不为空
        string _where0 = "WHERE Lottery_num!='' Order by Lottery_time desc";
        BCW.XinKuai3.Model.XK3_Internet_Data model0 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where0);
        string issue1 = string.Empty;
        try
        {
            issue1 = model.Lottery_issue;//本期开奖期号
        }
        catch
        {
            Utils.Error("系统尚未初始化...", "");
        }
        string issue3 = Utils.Right(model.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
        int Sec = Utils.ParseInt(ub.GetSub("xSec", xmlPath));
        int dnu = 0;
        if (IsOpen() == true)
        {
            if (model.Lottery_time.AddMinutes(10) < DateTime.Now)
            {
                int count = model.Lottery_issue.Length - 2;
                if (model.Lottery_issue.Substring(count, 2) == "78")
                {
                    string dsb = string.Empty;
                    if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                    {
                        dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                    }
                    else
                    {
                        dsb = DateTime.Now.ToString("yyMMdd");
                    }
                    dnu = int.Parse(dsb + "001");
                    string xk3 = new BCW.JS.somejs().newDaojishi("xk3", model.Lottery_time.AddHours(11).AddMinutes(10));
                    builder.Append("第" + dnu + "期.投注" + xk3 + "");
                }
                else
                {
                    Utils.Error("正在获取下一期信息...", "");
                }
            }
            else
            {
                int LiangTing = 2;
                if (int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue) <= LiangTing)//|| int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue) > 100
                {
                    builder.Append("第" + model.Lottery_issue + "期.投注");
                    string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", model.Lottery_time);
                    builder.Append("" + daojishi + "" + "");
                }
                else
                {
                    if (issue3 == "078")
                    {
                        string dsb = DateTime.Now.ToString("yyMMdd");
                        if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                        {
                            dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                        }
                        else
                        {
                            dsb = DateTime.Now.ToString("yyMMdd");
                        }
                        dnu = int.Parse(dsb + "001");
                        Utils.Error("第" + dnu + "期.没开奖,请稍候购买", "");
                    }
                    else
                    {
                        if (issue3 == "001")
                        {
                            string xk9 = new BCW.JS.somejs().newDaojishi("xk9", model.Lottery_time);
                            builder.Append("第" + model.Lottery_issue + "期.投注" + xk9 + "");
                        }
                        else if (issue3 == "002")
                        {
                            builder.Append("第" + model.Lottery_issue + "期.投注");
                            string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", model.Lottery_time);
                            builder.Append("" + daojishi + "" + "");
                        }
                        else
                        {
                            if (Utils.Right(model0.Lottery_issue.ToString(), 3) == "078")
                            {
                                string dsb = DateTime.Now.ToString("yyMMdd");
                                {
                                    dsb = DateTime.Now.ToString("yyMMdd");
                                }
                                dnu = int.Parse(dsb + "001");
                                Utils.Error("第" + dnu + "期.没开奖,请稍候购买", "");
                            }
                            else
                                Utils.Error("第" + (int.Parse(model0.Lottery_issue) + 1) + "期.没开奖,请稍候购买", "");
                        }

                    }
                }
            }
        }
        else
        {
            //builder.Append("游戏开放时间:" + ub.GetSub("xOnTime", xmlPath) + "");
            if (issue3 == "078")
            {
                string dsb = string.Empty;
                if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                {
                    dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                }
                else
                {
                    dsb = DateTime.Now.ToString("yyMMdd");
                }
                dnu = int.Parse(dsb + "001");
                string xk4 = new BCW.JS.somejs().newDaojishi("xk4", model.Lottery_time.AddHours(11).AddMinutes(10));
                builder.Append("第" + dnu + "期.投注" + xk4 + "");
            }
            else
            {
                Utils.Error("还没到销售时间.", "");
            }
        }
        builder.Append("|<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">刷新</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        //标题类型
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("类型：" + TypeTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append("<form id=\"form1\" method=\"post\" action=\"xk3.aspx\">");

        if (ptype == 1)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少选择1组号码投注= = =<br/>", ""));
            builder.Append("4<input type=\"checkbox\" name=\"Num1\" value=\"4\" /> &nbsp;&nbsp;");
            builder.Append("5<input type=\"checkbox\" name=\"Num1\" value=\"5\" /> &nbsp;&nbsp;");
            builder.Append("6<input type=\"checkbox\" name=\"Num1\" value=\"6\" /> &nbsp;&nbsp;");
            builder.Append("7<input type=\"checkbox\" name=\"Num1\" value=\"7\" /> &nbsp;&nbsp;");
            builder.Append("8<input type=\"checkbox\" name=\"Num1\" value=\"8\" /> &nbsp;&nbsp;");
            builder.Append("9<input type=\"checkbox\" name=\"Num1\" value=\"9\" /> &nbsp;&nbsp;");
            builder.Append("10<input type=\"checkbox\" name=\"Num1\" value=\"10\" /> &nbsp;&nbsp;");
            builder.Append("11<input type=\"checkbox\" name=\"Num1\" value=\"11\" /> &nbsp;&nbsp;");
            builder.Append("12<input type=\"checkbox\" name=\"Num1\" value=\"12\" /> &nbsp;&nbsp;");
            builder.Append("13<input type=\"checkbox\" name=\"Num1\" value=\"13\" />&nbsp;&nbsp;");
            builder.Append("14<input type=\"checkbox\" name=\"Num1\" value=\"14\" /> &nbsp;&nbsp;");
            builder.Append("15<input type=\"checkbox\" name=\"Num1\" value=\"15\" /> &nbsp;&nbsp;");
            builder.Append("16<input type=\"checkbox\" name=\"Num1\" value=\"16\" /> &nbsp;&nbsp;");
            builder.Append("17<input type=\"checkbox\" name=\"Num1\" value=\"17\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少选择1个投注= = =<br/>", ""));
            builder.Append("三同号通选：三同号通选<input type=\"checkbox\" name=\"Num1\" value=\"1\" checked=\"true\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 3)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少选择1组投注= = =<br/>", ""));
            builder.Append("111<input type=\"checkbox\" name=\"Num1\" value=\"111\"/> &nbsp;");
            builder.Append("222<input type=\"checkbox\" name=\"Num1\" value=\"222\" /> &nbsp;");
            builder.Append("333<input type=\"checkbox\" name=\"Num1\" value=\"333\" /> &nbsp;");
            builder.Append("444<input type=\"checkbox\" name=\"Num1\" value=\"444\" /> &nbsp;");
            builder.Append("555<input type=\"checkbox\" name=\"Num1\" value=\"555\" /> &nbsp;");
            builder.Append("666<input type=\"checkbox\" name=\"Num1\" value=\"666\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 4)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少任意选择3个号码投注= = =<br/>", ""));
            builder.Append("1<input type=\"checkbox\" name=\"Num1\" value=\"1\" /> &nbsp;&nbsp;");
            builder.Append("2<input type=\"checkbox\" name=\"Num1\" value=\"2\" /> &nbsp;&nbsp;");
            builder.Append("3<input type=\"checkbox\" name=\"Num1\" value=\"3\" /> &nbsp;&nbsp;");
            builder.Append("4<input type=\"checkbox\" name=\"Num1\" value=\"4\" /> &nbsp;&nbsp;");
            builder.Append("5<input type=\"checkbox\" name=\"Num1\" value=\"5\" /> &nbsp;&nbsp;");
            builder.Append("6<input type=\"checkbox\" name=\"Num1\" value=\"6\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 5)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少选择1个投注= = =<br/>", ""));
            builder.Append("三连号通选<input type=\"checkbox\" name=\"Num1\" value=\"1\" checked=\"true\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 6)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少选择1组投注= = =<br/>", ""));
            builder.Append("11<input type=\"checkbox\" name=\"Num1\" value=\"11\" /> &nbsp;&nbsp;");
            builder.Append("22<input type=\"checkbox\" name=\"Num1\" value=\"22\" /> &nbsp;&nbsp;");
            builder.Append("33<input type=\"checkbox\" name=\"Num1\" value=\"33\" /> &nbsp;&nbsp;");
            builder.Append("44<input type=\"checkbox\" name=\"Num1\" value=\"44\" /> &nbsp;&nbsp;");
            builder.Append("55<input type=\"checkbox\" name=\"Num1\" value=\"55\" /> &nbsp;&nbsp;");
            builder.Append("66<input type=\"checkbox\" name=\"Num1\" value=\"66\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 7)
        {
            builder.Append(Out.Tab("<div>= = =选号区 每位至少选择1组号码投注= = =<br/>", ""));
            builder.Append("同号：11<input type=\"checkbox\" name=\"Num1\" value=\"11\"/> &nbsp;&nbsp;");
            builder.Append("22<input type=\"checkbox\" name=\"Num1\" value=\"22\" /> &nbsp;&nbsp;");
            builder.Append("33<input type=\"checkbox\" name=\"Num1\" value=\"33\" /> &nbsp;&nbsp;");
            builder.Append("44<input type=\"checkbox\" name=\"Num1\" value=\"44\" /> &nbsp;&nbsp;");
            builder.Append("55<input type=\"checkbox\" name=\"Num1\" value=\"55\" /> &nbsp;&nbsp;");
            builder.Append("66<input type=\"checkbox\" name=\"Num1\" value=\"66\" /><br/>");
            builder.Append("不同号：1<input type=\"checkbox\" name=\"Num2\" value=\"1\"/> &nbsp;&nbsp;");
            builder.Append("2<input type=\"checkbox\" name=\"Num2\" value=\"2\"/> &nbsp;&nbsp;");
            builder.Append("3<input type=\"checkbox\" name=\"Num2\" value=\"3\"/> &nbsp;&nbsp;");
            builder.Append("4<input type=\"checkbox\" name=\"Num2\" value=\"4\"/> &nbsp;&nbsp;");
            builder.Append("5<input type=\"checkbox\" name=\"Num2\" value=\"5\"/> &nbsp;&nbsp;");
            builder.Append("6<input type=\"checkbox\" name=\"Num2\" value=\"6\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 8)
        {
            builder.Append(Out.Tab("<div>= = =选号区 至少选2个号码投注= = =<br/>", ""));
            builder.Append("二不同号：1<input type=\"checkbox\" name=\"Num1\" value=\"1\" /> &nbsp;&nbsp;");
            builder.Append("2<input type=\"checkbox\" name=\"Num1\" value=\"2\" /> &nbsp;&nbsp;");
            builder.Append("3<input type=\"checkbox\" name=\"Num1\" value=\"3\" /> &nbsp;&nbsp;");
            builder.Append("4<input type=\"checkbox\" name=\"Num1\" value=\"4\" /> &nbsp;&nbsp;");
            builder.Append("5<input type=\"checkbox\" name=\"Num1\" value=\"5\" /> &nbsp;&nbsp;");
            builder.Append("6<input type=\"checkbox\" name=\"Num1\" value=\"6\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 9)
        {
            //1大2小、1单2双
            builder.Append(Out.Tab("<div>= = =选号区 选1种投注= = =<br/>", ""));
            builder.Append("大：<input type=\"checkbox\" name=\"Num1\" value=\"1\" checked=\"true\" /> 实时赔率：" + Da + "<br/>");
            builder.Append("小：<input type=\"checkbox\" name=\"Num1\" value=\"2\" /> 实时赔率：" + Xiao + "");
            //builder.Append("大：&nbsp;&nbsp;<input type=\"radio\" name=\"Num1\" value=\"1\" checked=true/>");
            //builder.Append("小：&nbsp;&nbsp;<input type=\"radio\" name=\"Num1\" value=\"2\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ptype == 10)
        {
            builder.Append(Out.Tab("<div>= = =选号区 选1种投注= = =<br/>", ""));
            builder.Append("单：<input type=\"checkbox\" name=\"Num1\" value=\"1\" checked=\"true\" /> 实时赔率：" + Dan + "<br/>");
            builder.Append("双：<input type=\"checkbox\" name=\"Num1\" value=\"2\" /> 实时赔率：" + Shuang + "");

            //builder.Append("单：&nbsp;&nbsp;<input type=\"radio\" name=\"Num1\" value=\"1\" checked=true/> &nbsp;&nbsp;实时赔率：1:" + Dan + "<br/>");
            //builder.Append("双：&nbsp;&nbsp;<input type=\"radio\" name=\"Num1\" value=\"2\" /> &nbsp;&nbsp;实时赔率：1:" + Shuang + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
        builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"下一步\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("</form>");

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 9 || ptype == 10)
        {
            string _where2 = " WHERE Lottery_num!='' AND DateDiff(dd,Lottery_time,getdate())=0 ";//20160803 邵广林 修改页面显示为今天的最近10期
            IList<BCW.XinKuai3.Model.XK3_Internet_Data> model_get = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listTop(_where2);
            foreach (BCW.XinKuai3.Model.XK3_Internet_Data n in model_get)
            {
                string op = string.Empty;
                try
                {
                    string[] arrNum = Utils.ArrBySeparated(n.Lottery_num, 1);
                    op = arrNum[0];
                }
                catch { }
                string strDxs = string.Empty;
                if (n.DaXiao == "0")
                {
                    strDxs += "豹子" + op;
                }
                else
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        strDxs += "大";
                    }
                    if (n.DaXiao == "2")
                    {
                        strDxs += "小";
                    }
                    if (n.DanShuang == "1")
                    {
                        strDxs += "单";
                    }
                    if (n.DanShuang == "2")
                    {
                        strDxs += "双";
                    }
                }
                builder.Append("" + n.Lottery_issue + "期<b>.[" + n.Lottery_num + "].</b>&nbsp;" + strDxs + "<br/>");
            }
        }
        builder.Append(Out.Tab("</div>", ""));


        if ((ptype == 1) || (ptype == 3) || (ptype == 4) || (ptype == 6) || (ptype == 7) || (ptype == 8))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">清空选号</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        //玩法介绍
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + OutRule(ptype) + "");
        builder.Append(Out.Tab("</div>", ""));
        ////返回
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">&lt;&lt;返回" + GameName + "</a>");
        //builder.Append(Out.Tab("</div>", ""));
        //底部
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    //投注下一步判断
    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[1-9]$|^10$|^11$|^12$", "类型选择错误"));
        string info = Utils.GetRequest("info", "post", 1, "", "");

        //显示标题的投注方式
        string TypeTitle = OutType(ptype);
        Master.Title = TypeTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;" + TypeTitle + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        //读取最后一期
        string _where = "Order by Lottery_time desc";
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where);
        //读取最后一期不为空
        string _where0 = "WHERE Lottery_num!='' Order by Lottery_time desc";
        BCW.XinKuai3.Model.XK3_Internet_Data model0 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where0);
        string issue1 = string.Empty;
        try
        {
            issue1 = model.Lottery_issue;//本期开奖期号
        }
        catch
        {
            Utils.Error("系统尚未初始化...", "");
        }
        string qihao_now = (Int32.Parse(issue1)).ToString();
        long issue2 = Int64.Parse(issue1);
        string issue3 = Utils.Right(model.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
        int Sec = Utils.ParseInt(ub.GetSub("xSec", xmlPath));
        int dnu = 0;
        if (IsOpen() == true)
        {
            if (model.Lottery_time.AddMinutes(10) < DateTime.Now)
            {
                int count = model.Lottery_issue.Length - 2;
                if (model.Lottery_issue.Substring(count, 2) == "78")
                {
                    string dsb = string.Empty;
                    if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                    {
                        dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                    }
                    else
                    {
                        dsb = DateTime.Now.ToString("yyMMdd");
                    }
                    dnu = int.Parse(dsb + "001");
                    string xk3 = new BCW.JS.somejs().newDaojishi("xk3", model.Lottery_time.AddHours(11).AddMinutes(10));
                    builder.Append("第" + dnu + "期.投注" + xk3 + "");
                }
                else
                {
                    Utils.Error("正在获取下一期信息...", "");
                }
            }
            else
            {
                int LiangTing = 2;
                if (int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue) <= LiangTing)//|| int.Parse(model.Lottery_issue) - int.Parse(model0.Lottery_issue) > 100
                {
                    builder.Append("第" + model.Lottery_issue + "期.投注");
                    string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", model.Lottery_time);
                    builder.Append("" + daojishi + "" + "");
                }
                else
                {
                    if (issue3 == "078")
                    {
                        string dsb = DateTime.Now.ToString("yyMMdd");
                        if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                        {
                            dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                        }
                        else
                        {
                            dsb = DateTime.Now.ToString("yyMMdd");
                        }
                        dnu = int.Parse(dsb + "001");
                        Utils.Error("第" + dnu + "期.没开奖,请稍候购买", "");
                    }
                    else
                    {
                        if (issue3 == "001")
                        {
                            string xk9 = new BCW.JS.somejs().newDaojishi("xk9", model.Lottery_time);
                            builder.Append("第" + model.Lottery_issue + "期.投注" + xk9 + "");
                        }
                        else if (issue3 == "002")
                        {
                            builder.Append("第" + model.Lottery_issue + "期.投注");
                            string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", model.Lottery_time);
                            builder.Append("" + daojishi + "" + "");
                        }
                        else
                        {
                            if (Utils.Right(model0.Lottery_issue.ToString(), 3) == "078")
                            {
                                string dsb = DateTime.Now.ToString("yyMMdd");
                                {
                                    dsb = DateTime.Now.ToString("yyMMdd");
                                }
                                dnu = int.Parse(dsb + "001");
                                Utils.Error("第" + dnu + "期.没开奖,请稍候购买", "");
                            }
                            else
                                Utils.Error("第" + (int.Parse(model0.Lottery_issue) + 1) + "期.没开奖,请稍候购买", "");
                        }

                    }
                }
            }
        }
        else
        {
            //builder.Append("游戏开放时间:" + ub.GetSub("xOnTime", xmlPath) + "");
            if (issue3 == "078")
            {
                string dsb = string.Empty;
                if (System.DateTime.Now.Date == model.Lottery_time.Date)//今天
                {
                    dsb = DateTime.Now.AddDays(1).ToString("yyMMdd");
                }
                else
                {
                    dsb = DateTime.Now.ToString("yyMMdd");
                }
                dnu = int.Parse(dsb + "001");
                string xk4 = new BCW.JS.somejs().newDaojishi("xk4", model.Lottery_time.AddHours(11).AddMinutes(10));
                builder.Append("第" + dnu + "期.投注" + xk4 + "");
            }
            else
            {
                Utils.Error("还没到销售时间.", "");
            }
        }
        builder.Append("|<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">刷新</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        //标题类型
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("类型：" + TypeTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        string name = string.Empty;//存进消费记录的名字

        string Num2 = Utils.GetRequest("Num2", "all", 1, @"^[\d((,)\d)?]+$", "");
        string Num1 = Utils.GetRequest("Num1", "all", 1, @"^[\d((,)\d)?]+$", "");
        string accNum = string.Empty;
        string accNum2 = string.Empty;
        int iZhu = 0;
        string[] strTemp = { };
        string[] str1 = Num1.Split(',');
        string[] str2 = Num2.Split(',');
        if (ptype == 1)//----------------------------和值
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1组和值进行投注", "");
            }
            int cNum = Utils.GetStringNum(Num1, ",");
            accNum = Num1;
            iZhu = str1.Length;//根据分号来判断下了多少注
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("您选择了：" + accNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle + accNum;
        }
        if (ptype == 2)//---------------------------三同号通选
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1个进行投注", "");
            }
            accNum = Num1;
            iZhu = str1.Length;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(" 您选择了：111、222、333、444、555、666,");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle;
        }
        if (ptype == 3)//---------------------------三同号单选
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1组进行投注", "");
            }
            accNum = Num1;
            iZhu = str1.Length;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("您选择了：" + accNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle + Num1;
        }
        if (ptype == 4)//---------------------------三不同号//===============================特殊
        {
            if (Num1 == "" || !Num1.Contains(","))
            {
                Utils.Error("至少任意选择3个号码进行投注", "");
            }
            int cNum = Utils.GetStringNum(Num1, ",");
            if (cNum < 2)
            {
                Utils.Error("至少任意选择3个号码进行投注", "");
            }
            accNum = Num1;
            if (str1.Length == 3)
            {
                iZhu = 1;
                accNum = str1[0] + str1[1] + str1[2];
            }
            else if (str1.Length == 4)
            {
                iZhu = 4;
                accNum = str1[0] + str1[1] + str1[2] + "," + str1[0] + str1[1] + str1[3] + "," + str1[0] + str1[2] + str1[3] + "," + str1[1] + str1[2] + str1[3];
            }
            else if (str1.Length == 5)
            {
                iZhu = 10;
                accNum = str1[0] + str1[1] + str1[2] + "," + str1[0] + str1[1] + str1[3] + "," + str1[0] + str1[1] + str1[4] + "," + str1[0] + str1[2] + str1[3] + "," + str1[0] + str1[2] + str1[4] + "," + str1[0] + str1[3] + str1[4] + "," + str1[1] + str1[2] + str1[3] + "," + str1[1] + str1[2] + str1[4] + "," + str1[1] + str1[3] + str1[4] + "," + str1[2] + str1[3] + str1[4];
            }
            else if (str1.Length == 6)
            {
                iZhu = 20;
                accNum = str1[0] + str1[1] + str1[2] + "," + str1[0] + str1[1] + str1[3] + "," + str1[0] + str1[1] + str1[4] + "," + str1[0] + str1[1] + str1[5] + "," + str1[0] + str1[2] + str1[3] + "," + str1[0] + str1[2] + str1[4] + "," + str1[0] + str1[2] + str1[5] + "," + str1[0] + str1[3] + str1[4] + "," + str1[0] + str1[3] + str1[5] + "," + str1[0] + str1[4] + str1[5] + "," +
                    str1[1] + str1[2] + str1[3] + "," + str1[1] + str1[2] + str1[4] + "," + str1[1] + str1[2] + str1[5] + "," + str1[1] + str1[3] + str1[4] + "," + str1[1] + str1[3] + str1[5] + "," + str1[1] + str1[4] + str1[5] + "," + str1[2] + str1[3] + str1[4] + "," + str1[2] + str1[3] + str1[5] + "," + str1[2] + str1[4] + str1[5] + "," + str1[3] + str1[4] + str1[5];
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("您选择了：" + accNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle + Num1;
        }
        if (ptype == 5)//---------------------------三连号通选
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1个进行投注", "");
            }
            //accNum = Num1;
            iZhu = str1.Length;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(" 您选择了：123、234、345、456,");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle;
        }
        if (ptype == 6)//---------------------------二同号复选
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1组进行投注", "");
            }
            accNum = Num1;
            iZhu = str1.Length;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("您选择了：" + accNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle + Num1;
        }
        if (ptype == 7)//--------------------------二同号单选//===============================特殊
        {
            if (Num2 == "" || Num1 == "")
            {
                Utils.Error("每位至少选择1个号码进行投注", "");
            }
            int zyr = (Num2.Length + 1) / 2;
            for (int i = 0; i < zyr;)
            {
                bool id = ((IList)str1).Contains(str2[i] + str2[i]);
                i++;
                if (id)
                    Utils.Error("每位不能选取相同号码", "");
            }
            if (str1.Length == 1 && str2.Length == 1)//------上1、下1
            {
                iZhu = 1;
                accNum = Num1 + Num2;
            }
            if (str1.Length == 1 && str2.Length == 2)//------上1、下2
            {
                iZhu = 2;
                accNum = Num1 + Num2[0] + "," + Num1 + Num2[2];
            }
            if (str1.Length == 1 && str2.Length == 3)//------上1、下3
            {
                iZhu = 3;
                accNum = Num1 + Num2[0] + "," + Num1 + Num2[2] + "," + Num1 + Num2[4];
            }
            if (str1.Length == 1 && str2.Length == 4)//------上1、下4
            {
                iZhu = 4;
                accNum = Num1 + Num2[0] + "," + Num1 + Num2[2] + "," + Num1 + Num2[4] + "," + Num1 + Num2[6];
            }
            if (str1.Length == 1 && str2.Length == 5)//------上1、下5
            {
                iZhu = 5;
                accNum = Num1 + Num2[0] + "," + Num1 + Num2[2] + "," + Num1 + Num2[4] + "," + Num1 + Num2[6] + "," + Num1 + Num2[8];
            }
            if (str1.Length == 2 && str2.Length == 1)//------上2、下1
            {
                iZhu = 2;
                accNum = str1[0] + Num2 + "," + str1[1] + Num2;
            }
            if (str1.Length == 2 && str2.Length == 2)//------上2、下2
            {
                iZhu = 4;
                accNum = str1[0] + str2[0] + "," + str1[0] + str2[1] + "," + str1[1] + str2[0] + "," + str1[1] + str2[1];
            }
            if (str1.Length == 2 && str2.Length == 3)//------上2、下3
            {
                iZhu = 6;
                accNum = str1[0] + str2[0] + "," + str1[0] + str2[1] + "," + str1[0] + str2[2] + "," + str1[1] + str2[0] + "," + str1[1] + str2[1] + "," + str1[1] + str2[2];
            }
            if (str1.Length == 2 && str2.Length == 4)//------上2、下4
            {
                iZhu = 8;
                accNum = str1[0] + str2[0] + "," + str1[0] + str2[1] + "," + str1[0] + str2[2] + "," + str1[0] + str2[3] + "," + str1[1] + str2[0] + "," + str1[1] + str2[1] + "," + str1[1] + str2[2] + "," + str1[1] + str2[3];
            }
            if (str1.Length == 3 && str2.Length == 1)//------上3、下1
            {
                iZhu = 3;
                accNum = str1[0] + Num2 + "," + str1[1] + Num2 + "," + str1[2] + Num2;
            }
            if (str1.Length == 3 && str2.Length == 2)//------上3、下2
            {
                iZhu = 6;
                accNum = str1[0] + str2[0] + "," + str1[0] + str2[1] + "," + str1[1] + str2[0] + "," + str1[1] + str2[1] + "," + str1[2] + str2[0] + "," + str1[2] + str2[1];
            }
            if (str1.Length == 3 && str2.Length == 3)//------上3、下3
            {
                iZhu = 9;
                accNum = str1[0] + str2[0] + "," + str1[0] + str2[1] + "," + str1[0] + str2[2] + "," + str1[1] + str2[0] + "," + str1[1] + str2[1] + "," + str1[1] + str2[2] + "," + str1[2] + str2[0] + "," + str1[2] + str2[1] + "," + str1[2] + str2[2];
            }
            if (str1.Length == 4 && str2.Length == 1)//------上4、下1
            {
                iZhu = 4;
                accNum = str1[0] + Num2 + "," + str1[1] + Num2 + "," + str1[2] + Num2 + "," + str1[3] + Num2;
            }
            if (str1.Length == 4 && str2.Length == 2)//------上4、下2
            {
                iZhu = 8;
                accNum = str1[0] + str2[0] + "," + str1[0] + str2[1] + "," + str1[1] + str2[0] + "," + str1[1] + str2[1] + "," + str1[2] + str2[0] + "," + str1[2] + str2[1] + "," + str1[3] + str2[0] + "," + str1[3] + str2[1];
            }
            if (str1.Length == 5 && str2.Length == 1)//------上5、下1
            {
                iZhu = 5;
                accNum = str1[0] + Num2 + "," + str1[1] + Num2 + "," + str1[2] + Num2 + "," + str1[3] + Num2 + "," + str1[4] + Num2;
            }


            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("您选择了：" + accNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle + Num1 + "," + Num2;
        }
        if (ptype == 8)//--------------------------二不同号//===============================特殊
        {
            if (Num1 == "" || !Num1.Contains(","))
            {
                Utils.Error("至少选择2个号码进行投注", "");
            }
            //accNum = Num1;
            if (str1.Length == 2)
            {
                iZhu = 1;

                accNum = str1[0] + str1[1];
            }
            if (str1.Length == 3)
            {
                iZhu = 3;
                accNum = str1[0] + str1[1] + "," + str1[0] + str1[2] + "," + str1[1] + str1[2];
            }
            if (str1.Length == 4)
            {
                iZhu = 6;
                accNum = str1[0] + str1[1] + "," + str1[0] + str1[2] + "," + str1[0] + str1[3] + "," + str1[1] + str1[2] + "," + str1[1] + str1[3] + "," + str1[2] + str1[3];
            }
            if (str1.Length == 5)
            {
                iZhu = 10;
                accNum = str1[0] + str1[1] + "," + str1[0] + str1[2] + "," + str1[0] + str1[3] + "," + str1[0] + str1[4] + "," + str1[1] + str1[2] + "," + str1[1] + str1[3] + "," + str1[1] + str1[4] + "," + str1[2] + str1[3] + "," + str1[2] + str1[4] + "," + str1[3] + str1[4];
            }
            if (str1.Length == 6)
            {
                iZhu = 15;
                accNum = str1[0] + str1[1] + "," + str1[0] + str1[2] + "," + str1[0] + str1[3] + "," + str1[0] + str1[4] + "," + str1[0] + str1[5] + "," + str1[1] + str1[2] + "," + str1[1] + str1[3] + "," + str1[1] + str1[4] + "," + str1[1] + str1[5] + "," + str1[2] + str1[3] + "," + str1[2] + str1[4] + "," + str1[2] + str1[5] + "," + str1[3] + str1[4] + "," + str1[3] + str1[5] + "," + str1[4] + str1[5];
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("您选择了：" + accNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            name = TypeTitle + Num1;
        }
        if (ptype == 9)//---------------------------大小
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1组进行投注", "");
            }
            if (Num1.Contains(","))
            {
                Utils.Error("只能选择其中一组进行投注", "");
            }
            accNum = Num1;
            iZhu = str1.Length;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            //1大2小、1单2双
            if (accNum == "1")
            {
                builder.Append("您选择了：大");
                name = TypeTitle + ":大";
            }
            else if (accNum == "2")
            {
                builder.Append("您选择了：小");
                name = TypeTitle + ":小";
            }

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 10)//---------------------------双单
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1组进行投注", "");
            }
            if (Num1.Contains(","))
            {
                Utils.Error("只能选择其中一组进行投注", "");
            }
            accNum = Num1;
            iZhu = str1.Length;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            //1大2小、1单2双
            if (accNum == "1")
            {
                builder.Append("您选择了：单");
                name = TypeTitle + ":单";
            }
            else if (accNum == "2")
            {
                builder.Append("您选择了：双");
                name = TypeTitle + ":双";
            }

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (info == "ok2")
        {
            long small = Convert.ToInt64(ub.GetSub("xSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("xBigPay", xmlPath));
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 2, @"^[1-9]\d*$", "投注额填写错误"));
            string qihao = (Utils.GetRequest("qihao", "all", 2, @"^[0-9]\d*$", "期号错误"));

            long gold = new BCW.BLL.User().GetGold(meid);
            long prices = Convert.ToInt64(Price * iZhu);
            string hj = Utils.GetRequest("hj", "all", 2, @"^(\d)*(\.(\d){1,2})?$", "实时双的赔率错误");


            if (gold < prices)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }

            #region 个人每期限投
            long xPrices = Utils.ParseInt64(ub.GetSub("XK3Price", xmlPath));
            if (xPrices > 0)
            {
                long oPrices = 0;
                DataSet ds;
                if (IsOpen() == true)
                {
                    ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + issue2 + "'");
                }
                else
                {
                    if (issue3 == "078")
                    {
                        ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + dnu + "'");
                    }
                    else
                    {
                        ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + issue2 + "'");
                    }
                }
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    int drs = int.Parse(dr[0].ToString());
                    oPrices = oPrices + drs;
                }
                if (oPrices + prices > xPrices)
                {
                    if (oPrices >= xPrices)
                        Utils.Error("您本期投注已达上限，请等待下期...", "");
                    else
                        Utils.Error("您本期最多还可以投注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                }
            }
            #endregion

            #region 大小单双平行限投
            long xMaxnum = Utils.ParseInt64(ub.GetSub("Maxnum", xmlPath));
            if (xMaxnum > 0)
            {
                DataSet d1, d2, d3, d4;
                //1大2小、1单2双
                if (IsOpen() == true)
                {
                    d1 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='9' AND DaXiao='1'  and Lottery_issue='" + issue2 + "'");
                    d2 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='9' AND DaXiao='2' and Lottery_issue='" + issue2 + "'");
                    d3 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='10' AND DanShuang='1' and Lottery_issue='" + issue2 + "'");
                    d4 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='10' AND DanShuang='2' and Lottery_issue='" + issue2 + "'");
                }
                else
                {
                    if (issue3 == "078")
                    {
                        d1 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='9' AND DaXiao='1'  and Lottery_issue='" + dnu + "'");//该期投注是大的总购买数
                        d2 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='9' AND DaXiao='2' and Lottery_issue='" + dnu + "'");//小
                        d3 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='10' AND DanShuang='1' and Lottery_issue='" + dnu + "'");//单
                        d4 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='10' AND DanShuang='2'and Lottery_issue='" + dnu + "'");//双
                    }
                    else
                    {
                        d1 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='9' AND DaXiao='1'  and Lottery_issue='" + issue2 + "'");
                        d2 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='9' AND DaXiao='2' and Lottery_issue='" + issue2 + "'");
                        d3 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='10' AND DanShuang='1' and Lottery_issue='" + issue2 + "'");
                        d4 = new BCW.XinKuai3.BLL.XK3_Bet().GetList("Sum(PutGold) as bb", "Play_Way='10' AND DanShuang='2' and Lottery_issue='" + issue2 + "'");
                    }
                }

                long Cents1 = 0, Cents2 = 0, Cents3 = 0, Cents4 = 0;
                for (int i = 0; i < d1.Tables[0].Rows.Count; i++)//大
                {
                    try
                    {
                        Cents1 = Convert.ToInt64(d1.Tables[0].Rows[i]["bb"]);
                    }
                    catch
                    {
                        Cents1 = 0;
                    }
                }
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents2 = Convert.ToInt64(d2.Tables[0].Rows[i]["bb"]);
                    }
                    catch
                    {
                        Cents2 = 0;
                    }
                }
                for (int i = 0; i < d3.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = Convert.ToInt64(d3.Tables[0].Rows[i]["bb"]);
                    }
                    catch
                    {
                        Cents3 = 0;
                    }
                }
                for (int i = 0; i < d4.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents4 = Convert.ToInt64(d4.Tables[0].Rows[i]["bb"]);
                    }
                    catch
                    {
                        Cents4 = 0;
                    }
                }
                if (ptype == 9)
                {
                    //1大2小、1单2双
                    if (Num1 == "1")
                    {
                        if (Cents1 - Cents2 >= xMaxnum)
                        {
                            Utils.Error("本期投注大已达上限，请选择投注小或等待其他人投注小或等待下期...", "");
                        }
                    }
                    if (Num1 == "2")
                    {
                        if (Cents2 - Cents1 >= xMaxnum)
                        {
                            Utils.Error("本期投注小已达上限，请选择投注大或等待其他人投注大或等待下期...", "");
                        }
                    }
                }
                if (ptype == 10)
                {
                    //1大2小、1单2双
                    if (Num1 == "1")
                    {
                        if (Cents3 - Cents4 >= xMaxnum)
                        {
                            Utils.Error("本期投注单已达上限，请选择投注双或等待其他人投注双或等待下期...", "");
                        }
                    }
                    if (Num1 == "2")
                    {
                        if (Cents4 - Cents3 >= xMaxnum)
                        {
                            Utils.Error("本期投注双已达上限，请选择投注单或等待其他人投注单或等待下期...", "");
                        }
                    }
                }
            }
            #endregion

            //支付安全提示
            string[] p_pageArr = { "Price", "Num1", "Num2", "ptype", "act", "info", "hj", "qihao" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            BCW.XinKuai3.Model.XK3_Bet modelpay = new BCW.XinKuai3.Model.XK3_Bet();
            modelpay.Play_Way = ptype;//玩法

            #region 赔率、期号变动提示
            {
                bool IsCheck1 = false;
                bool IsCheck2 = false;
                bool IsCheck3 = false;
                bool IsCheck4 = false;
                bool IsCheck5 = false;
                if (model.Lottery_issue != qihao)
                {
                    IsCheck5 = true;
                }
                if (ptype == 9)
                {
                    //1大2小、1单2双
                    if (accNum == "1")
                    {
                        if ((Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Da) * 100) > 0 || (Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Da) * 100) < 0)
                        {
                            IsCheck1 = true;
                        }
                    }
                    else if (accNum == "2")
                    {
                        if ((Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Xiao) * 100) > 0 || (Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Xiao) * 100) < 0)
                        {
                            IsCheck2 = true;
                        }
                    }
                }
                if (ptype == 10)
                {
                    //1大2小、1单2双
                    if (accNum == "2")
                    {
                        if ((Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Shuang) * 100) > 0 || (Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Shuang) * 100) < 0)
                        {
                            IsCheck4 = true;
                        }
                    }
                    else if (accNum == "1")
                    {
                        if ((Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Dan) * 100) > 0 || (Convert.ToDecimal(hj) * 100 - Convert.ToDecimal(Dan) * 100) < 0)
                        {
                            IsCheck3 = true;
                        }
                    }
                }
                if (IsCheck1 == true || IsCheck2 == true || IsCheck3 == true || IsCheck4 == true || IsCheck5 == true)
                {
                    new Out().head(Utils.ForWordType("温馨提示"));
                    Response.Write(Out.Tab("<div class=\"title\">", ""));
                    Response.Write("温馨提示");
                    Response.Write(Out.Tab("</div>", "<br />"));

                    Response.Write(Out.Tab("<div class=\"text\">", ""));
                    if (IsCheck5 == true)//如果期号不相同
                    {
                        if (IsCheck1 == true)
                        {
                            Response.Write("大的赔率由" + hj + "变为" + Da + "；投注期号由" + qihao + "期变为" + model.Lottery_issue + "期");
                            hj = Convert.ToString(Da);
                        }
                        else if (IsCheck2 == true)
                        {
                            Response.Write("小的赔率由" + hj + "变为" + Xiao + "；投注期号由" + qihao + "期变为" + model.Lottery_issue + "期");
                            hj = Convert.ToString(Xiao);
                        }
                        else if (IsCheck3 == true)
                        {
                            Response.Write("单的赔率由" + hj + "变为" + Dan + "；投注期号由" + qihao + "期变为" + model.Lottery_issue + "期");
                            hj = Convert.ToString(Dan);
                        }
                        else if (IsCheck4 == true)
                        {
                            Response.Write("双的赔率由" + hj + "变为" + Shuang + "；投注期号由" + qihao + "期变为" + model.Lottery_issue + "期");
                            hj = Convert.ToString(Shuang);
                        }
                        else
                        {
                            Response.Write("投注期号由" + qihao + "变为" + model.Lottery_issue + "");
                        }
                    }
                    else//如果期号相同
                    {
                        if (IsCheck1 == true)
                        {
                            Response.Write("大的赔率由" + hj + "变为" + Da + "");
                            hj = Convert.ToString(Da);
                        }
                        else if (IsCheck2 == true)
                        {
                            Response.Write("小的赔率由" + hj + "变为" + Xiao + "");
                            hj = Convert.ToString(Xiao);
                        }
                        else if (IsCheck3 == true)
                        {
                            Response.Write("单的赔率由" + hj + "变为" + Dan + "");
                            hj = Convert.ToString(Dan);
                        }
                        else if (IsCheck4 == true)
                        {
                            Response.Write("双的赔率由" + hj + "变为" + Shuang + "");
                            hj = Convert.ToString(Shuang);
                        }
                    }
                    Response.Write(Out.Tab("</div>", "<br />"));

                    string strName = "Price,Num2,Num1,ptype,act,infoo,hj,info,qihao";
                    string strValu = "" + Price + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok'" + hj + "'ok2'" + model.Lottery_issue + "";
                    string strOthe = "确定投注,xk3.aspx,post,0,red";
                    Response.Write(Out.wapform(strName, strValu, strOthe));

                    Response.Write(Out.Tab("<div>", "<br />"));
                    Response.Write("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">[取消返回]</a><br />");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }
            }
            #endregion


            //是否刷屏
            string appName = "XK3";
            int Expir = Utils.ParseInt(ub.GetSub("xExpir", xmlPath));//5
            BCW.User.Users.IsFresh(appName, Expir, Price, small, big);

            string a_get = string.Empty;
            if (IsOpen() == true)//开奖时间内
            {
                modelpay.Lottery_issue = qihao_now;//开奖期号
                a_get = qihao_now;
            }
            else//第二天
            {
                if (issue3 == "078")
                {
                    modelpay.Lottery_issue = dnu.ToString();
                    a_get = dnu.ToString();
                }
                else
                {
                    modelpay.Lottery_issue = qihao_now;//开奖期号
                    a_get = qihao_now;
                }
            }
            modelpay.UsID = meid;//用户ID
            modelpay.Zhu = iZhu;//注
            modelpay.Zhu_money = Price;//每注投多少钱
            modelpay.PutGold = prices;//总投了多少钱
            modelpay.Input_Time = DateTime.Now;//投奖时间
            modelpay.DanTuo = "0";//胆拖
            modelpay.State = 0;//未开奖
            modelpay.GetMoney = 0;//获得多少酷币
            if (ptype == 1)
            {
                modelpay.Sum = Num1;
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 2)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = Num1;
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 3)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = Num1;
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 4)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = accNum;
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 5)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = Num1;
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 6)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = Num1;
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 7)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = accNum;
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 8)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = accNum;
                modelpay.DaXiao = "";
                modelpay.DanShuang = "";
                modelpay.Odds = 1;
            }
            else if (ptype == 9)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = accNum;
                modelpay.DanShuang = "";
                //1大2小、1单2双
                if (accNum == "1")
                {
                    modelpay.Odds = (decimal)Da;
                }
                else if (accNum == "2")
                    modelpay.Odds = (decimal)Xiao;
            }
            else if (ptype == 10)
            {
                modelpay.Sum = "";
                modelpay.Three_Same_All = "";
                modelpay.Three_Same_Single = "";
                modelpay.Three_Same_Not = "";
                modelpay.Three_Continue_All = "";
                modelpay.Two_Same_All = "";
                modelpay.Two_Same_Single = "";
                modelpay.Two_dissame = "";
                modelpay.DaXiao = "";
                modelpay.DanShuang = accNum;
                //1大2小、1单2双
                if (accNum == "2")
                {
                    modelpay.Odds = (decimal)Shuang;
                }
                else if (accNum == "1")
                    modelpay.Odds = (decimal)Dan;
            }

            int id = new BCW.XinKuai3.BLL.XK3_Bet().Add(modelpay);

            string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
            //邵广林20160930 消费记录增加赔率显示
            string na = string.Empty;
            if (ptype == 9 || ptype == 10)
            {
                if (modelpay.Odds > 1)
                {
                    na = "赔率[" + modelpay.Odds + "]";
                }
            }

            if (IsOpen() == true)
            {
                //邵广林 20160827 增加消费记录到103
                int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                if (isSystemID != 0)//不是系统号
                {
                    new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]期" + name + na + "-标识ID" + id + "");
                }
                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]期" + name + na + "-标识ID" + id + "");//新快3----更新排行榜与扣钱
                //[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]
            }
            else
            {
                if (issue3 == "078")
                {
                    int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                    if (isSystemID != 0)//不是系统号
                    {
                        new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]期" + name + na + "-标识ID" + id + "");
                    }
                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]期" + name + na + "-标识ID" + id + "");//新快3----更新排行榜与扣钱
                }
                else
                {
                    int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                    if (isSystemID != 0)//不是系统号
                    {
                        new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]期" + name + na + "-标识ID" + id + "");
                    }
                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + qihao_now + "]" + qihao_now + "[/url]期" + name + na + "-标识ID" + id + "");//新快3----更新排行榜与扣钱
                }
            }
            //活跃抽奖入口_20160621姚志光
            try
            {
                //表中存在记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName(GameName))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (prices > new BCW.BLL.tb_WinnersGame().GetPrice(GameName))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, GameName + "新快3", 3);
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

            //取消投注加入排行榜，改回开奖加入排行榜 20160712 邵广林
            //if (!(new BCW.XinKuai3.BLL.XK3_Toplist().Exists_usid(meid)))
            //{
            //    BCW.XinKuai3.Model.XK3_Toplist model_2 = new BCW.XinKuai3.Model.XK3_Toplist();
            //    model_2.UsId = meid;
            //    model_2.UsName = mename;
            //    model_2.WinGold = 0;
            //    model_2.PutGold = prices;
            //    new BCW.XinKuai3.BLL.XK3_Toplist().Add(model_2);
            //}
            //else
            //{
            //    BCW.XinKuai3.Model.XK3_Toplist model_1 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(meid);

            //    long all_prices = model_1.PutGold + prices;

            //    new BCW.XinKuai3.BLL.XK3_Toplist().Update_gold(meid, all_prices);
            //}

            //动态
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]投注**" + ub.Get("SiteBz") + "";//" + prices + "
            new BCW.BLL.Action().Add(1001, id, meid, "", wText);

            Utils.Success("投注", "投注《" + TypeTitle + "》成功，花费了" + prices + "" + ub.Get("SiteBz") + "(共" + iZhu + "注)<br /><a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;继续投注</a>", Utils.getUrl("xk3.aspx"), "2");
        }
        else if (info == "ok")
        {
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 2, @"^[1-9]\d*$", "投注额填写错误"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("注数：" + iZhu + "注<br />");
            builder.Append("每注：" + Price + "" + ub.Get("SiteBz") + "<br />");
            float hj = 0;
            if (ptype == 9)
            {
                //1大2小、1单2双
                if (accNum == "1")
                {
                    builder.Append("当前实时赔率：" + Da + "<br />");
                    hj = Da;
                }
                else if (accNum == "2")
                {
                    builder.Append("当前实时赔率：" + Xiao + "<br />");
                    hj = Xiao;
                }

            }
            if (ptype == 10)
            {
                //1大2小、1单2双
                if (accNum == "2")
                {
                    builder.Append("当前实时赔率：" + Shuang + "<br />");
                    hj = Shuang;
                }
                else if (accNum == "1")
                {
                    builder.Append("当前实时赔率：" + Dan + "<br />");
                    hj = Dan;
                }

            }
            builder.Append("需花费：" + (Price * iZhu) + "" + ub.Get("SiteBz") + "<br />");
            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append(Out.Tab("</div>", ""));


            string strName = "Price,Num2,Num1,ptype,act,info,hj,qihao";
            string strValu = "" + Price + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok2'" + hj + "'" + model.Lottery_issue + "";
            string strOthe = "确定投注,xk3.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&lt;&lt;返回再看看</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("→您选了" + iZhu + "注←");
            builder.Append(Out.Tab("</div>", ""));


            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("快捷下注<br />∟");
            kuai(1, ptype, Num1, Num2);
            builder.Append(Out.Tab("</div>", ""));
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 1, @"^[1-9]\d*$", "0"));


            string strText = "每注投注额(" + ub.GetSub("xSmallPay", xmlPath) + "---" + ub.GetSub("xBigPay", xmlPath) + "" + ub.Get("SiteBz") + "):/,,,,,,,,";
            string strName = "Price,Num2,Num1,ptype,act,info";
            string strType = "num,hidden,hidden,hidden,hidden,hidden";
            string strValu = string.Empty;
            if (Price > 0)
                strValu = "" + Price + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok";
            else
                strValu = "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok";
            string strEmpt = "true,false,false,false,false,false";
            string strIdea = "" + "&nbsp;&nbsp;" + ub.Get("SiteBz") + "''''''''|/";
            string strOthe = "确定投注,xk3.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=info&amp;ptype=" + ptype + "") + "\">&lt;&lt;返回重新选号</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    //兑奖开始
    private void CasePage()
    {
        Master.Title = "" + GameName + "_兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "and GetMoney >0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        string XK3qi = "";
        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bets(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3pay.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet n in listXK3pay)
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
                if (n.Lottery_issue.ToString() != XK3qi)
                {
                    BCW.XinKuai3.Model.XK3_Internet_Data model_num1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(n.Lottery_issue);
                    builder.Append("=第" + n.Lottery_issue + "期" + model_num1.Lottery_num + "=<br />");
                }

                string Getnum = string.Empty;
                if (n.Play_Way == 1)
                {
                    Getnum = n.Sum;//和值
                }
                else if (n.Play_Way == 2)
                {
                    //Getnum = n.Three_Same_All;//三同号通选
                    Getnum = "111、222、333、444、555、666";
                }
                else if (n.Play_Way == 3)
                {
                    Getnum = n.Three_Same_Single;//三同号单选
                }
                else if (n.Play_Way == 4)
                {
                    Getnum = n.Three_Same_Not;//三不同号
                }
                else if (n.Play_Way == 5)
                {
                    //Getnum = n.Three_Continue_All;//三连号通选
                    Getnum = "123、234、345、456";
                }
                else if (n.Play_Way == 6)
                {
                    Getnum = n.Two_Same_All;//二同号复选
                }
                else if (n.Play_Way == 7)
                {
                    Getnum = n.Two_Same_Single;//二同号单选
                }
                else if (n.Play_Way == 8)
                {
                    Getnum = n.Two_dissame;//二不同号
                }
                if (n.Play_Way == 9)
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        Getnum = "大";
                    }
                    else if (n.DaXiao == "2")
                    {
                        Getnum = "小";
                    }
                }
                if (n.Play_Way == 10)
                {
                    //1大2小、1单2双
                    if (n.DanShuang == "2")
                    {
                        Getnum = "双";
                    }
                    else if (n.DanShuang == "1")
                    {
                        Getnum = "单";
                    }
                }

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("[" + OutType(n.Play_Way) + "]<br />当前购买号码:" + Getnum + "<br />每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注/投注时间:[" + DT.FormatDate(n.Input_Time, 1) + "]");
                builder.Append("<br/><h style=\"color:red\">赢" + n.GetMoney + "" + ub.Get("SiteBz") + "</h>");
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

                XK3qi = n.Lottery_issue.ToString();
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
        //多个兑奖
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,xk3.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", ""));
        GameFoot();
    }

    //单个兑奖
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.XinKuai3.BLL.XK3_Bet().ExistsState(pid, meid))
        {
            BCW.XinKuai3.Model.XK3_Bet model = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(pid);

            //操作币
            long winMoney = model.GetMoney;//获得该中奖的酷币

            BCW.User.Users.IsFresh("xk3", 2);//防刷

            new BCW.XinKuai3.BLL.XK3_Bet().UpdateState(pid, 2);//当state为1的时候，即为系统已开奖
            int sx = Convert.ToInt32(winMoney * shouxu / 1000);
            if ((sx >= 1) && (shouxu > 0))//扣手续费
            {
                winMoney = winMoney - sx;
                new BCW.XinKuai3.BLL.XK3_Bet().update_zd("Cost='" + sx + "'", "id=" + pid + "");
                //[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期(已扣除手续费" + sx + ")-标识ID" + pid + "");
                //扣103的钱
                int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                if (isSystemID != 0)//不是系统号
                {
                    new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期(已扣除手续费" + sx + ")(标识ID" + pid + ")");
                }
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "(已扣除手续费" + sx + ")", Utils.getUrl("xk3.aspx?act=case"), "1");
            }
            else//正常
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期-标识ID" + pid + "");
                //扣103的钱
                int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                if (isSystemID != 0)//不是系统号
                {
                    new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期(标识ID" + pid + ")");
                }
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("xk3.aspx?act=case"), "1");
            }
        }
        else
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("xk3.aspx?act=case"), "1");
        }
    }

    //多个兑奖
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
        int shoux = 0;
        BCW.User.Users.IsFresh("xk3", 2);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);

            if (new BCW.XinKuai3.BLL.XK3_Bet().ExistsState(pid, meid))
            {
                BCW.XinKuai3.Model.XK3_Bet model = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(pid);
                new BCW.XinKuai3.BLL.XK3_Bet().UpdateState(pid, 2);//当state为1的时候，即为中奖
                //操作币
                winMoney = Convert.ToInt64(model.GetMoney);//获得该中奖的酷币)

                int sx = Convert.ToInt32(winMoney * shouxu / 1000);
                if ((sx >= 1) && (shouxu > 0))//扣手续费
                {
                    winMoney = winMoney - sx;
                    shoux = shoux + sx;
                    new BCW.XinKuai3.BLL.XK3_Bet().update_zd("Cost='" + sx + "'", "id=" + pid + "");
                    new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期(已扣除手续费" + sx + ")-标识ID" + pid + "");
                    //扣103的钱
                    int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                    if (isSystemID != 0)//不是系统号
                    {
                        new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期(已扣除手续费" + sx + ")(标识ID" + pid + ")");
                    }
                }
                else//正常
                {
                    new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期-标识ID" + pid + "");
                    //扣103的钱
                    int isSystemID = Convert.ToInt32(SqlHelper.GetSingle("Select ID from tb_User where ID=" + meid + " and IsSpier=0"));
                    if (isSystemID != 0)//不是系统号
                    {
                        new BCW.BLL.User().UpdateiGold(103, new BCW.BLL.User().GetUsName(103), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]在" + GameName + "第[url=./game/xk3.aspx?act=view&amp;id=" + model.Lottery_issue + "]" + model.Lottery_issue + "[/url]期(标识ID" + pid + ")");
                    }
                }
            }
            getwinMoney = getwinMoney + winMoney;
        }
        if (shoux > 0)
        {
            Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "(已扣除手续费" + shoux + ")", Utils.getUrl("xk3.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("xk3.aspx?act=case"), "1");
        }
    }

    //往期分析
    private void AnalyzePage()
    {
        Master.Title = "" + GameName + "_数据分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;数据分析");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("按日期查询：格式（20160214）");
        builder.Append(Out.Tab("</div>", ""));
        string searchday = string.Empty;
        searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
        if (searchday == "")
        {
            searchday = DateTime.Now.ToString("yyyyMMdd");
        }
        string strText = "";
        string strName = "inputdate";
        string strType = "num";
        string strValu = searchday;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "查询,xk3.aspx?act=analyze,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br/>"));
        //builder.Append("<hr/>");

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        if (ptype == 1)
        {
            strValu = searchday;
            builder.Append("<h style=\"color:red\">今天" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=analyze&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">今天</a>" + "|");
        }
        if (ptype == 2)
        {
            searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", (DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));
            strValu = searchday;
            builder.Append("<h style=\"color:red\">昨天" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=analyze&amp;ptype=2&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">昨天</a>" + "|");
        }
        if (ptype == 3)
        {
            searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", (DateTime.Now.AddDays(-2)).ToString("yyyyMMdd"));
            strValu = searchday;
            builder.Append("<h style=\"color:red\">前天" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=analyze&amp;ptype=3&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">前天</a>" + "");
        }
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //-------1
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<h style=\"color:blue\">类型统计：</h>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //查询顺序：二同号，二不同号，三同号，三不同号，三连号
        string where_Two_Same_All = string.Empty;
        string where_Two_dissame = string.Empty;
        string where_Three_Same_All = string.Empty;
        string where_Three_Same_Not = string.Empty;
        string where_Three_Continue_All = string.Empty;

        //二同号
        where_Two_Same_All = "AND convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3Two_Same_All(where_Two_Same_All);
        //二不同号
        where_Two_dissame = "AND convert(varchar(10),Lottery_time,120)='" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a2 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3Two_dissame(where_Two_dissame);
        //三同号
        where_Three_Same_All = "AND convert(varchar(10),Lottery_time,120)='" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a3 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3Three_Same_All(where_Three_Same_All);
        //三不同号
        where_Three_Same_Not = "AND convert(varchar(10),Lottery_time,120)='" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a4 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3where_Three_Same_Not(where_Three_Same_Not);
        //三连号
        where_Three_Continue_All = "AND convert(varchar(10),Lottery_time,120)='" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a5 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3where_Three_Continue_All(where_Three_Continue_All);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("组二单选:<h style=\"color:red\">" + a1.aa + "</h>次 | 复选任二:<h style=\"color:red\">" + a2.aa + "</h>次<br/>");//二同号//二不同号
        //builder.Append("<hr/>");
        builder.Append("直选豹子:<h style=\"color:red\">" + a3.aa + "</h>次 | 复选任三:<h style=\"color:red\">" + a4.aa + "</h>次<br/>");//三同号//三不同号
        //builder.Append("<hr/>");
        builder.Append("三连通选:<h style=\"color:red\">" + a5.aa + "</h>次");//三连号
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //-------2
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<h style=\"color:blue\">和值统计：</h>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string _where = "and convert(varchar(10),Lottery_time,120)='" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "'";
        IList<BCW.XinKuai3.Model.XK3_Internet_Data> model_get = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all(_where);
        foreach (BCW.XinKuai3.Model.XK3_Internet_Data n in model_get)
        {
            builder.Append("总和" + n.Sum + ":<h style=\"color:red\">" + n.aa + "</h>次<br/>");
        }
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("</div>", ""));


        //--------3
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<h style=\"color:blue\">大小双单统计：</h>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string da = string.Empty;
        string xiao = string.Empty;
        string shuang = string.Empty;
        string dan = string.Empty;
        string tongchi = string.Empty;

        da = "AND convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a6 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3da(da);
        xiao = "AND convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a7 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3xiao(xiao);
        shuang = "AND convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a8 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3shuang(shuang);
        dan = "AND convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a9 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3dan(dan);
        tongchi = "AND convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        BCW.XinKuai3.Model.XK3_Internet_Data a10 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3tongchi(tongchi);
        //1大2小、1单2双
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("大:<h style=\"color:red\">" + a6.aa + "</h>次 | 小:<h style=\"color:red\">" + a7.aa + "</h>次 | ");
        //builder.Append("<hr/>");
        builder.Append("双:<h style=\"color:red\">" + a8.aa + "</h>次 | 单:<h style=\"color:red\">" + a9.aa + "</h>次<br/>");
        //builder.Append("<hr/>");
        builder.Append("豹子通吃:<h style=\"color:red\">" + a10.aa + "</h>次");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //显示标题的投注方式
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "总和复选";//和值投注
        else if (Types == 2)
            pText = "任意豹子";//三同号通选
        else if (Types == 3)
            pText = "直选豹子";//三同号单选
        else if (Types == 4)
            pText = "复选任三";//三不同号
        else if (Types == 5)
            pText = "三连通选";//三连号通选
        else if (Types == 6)
            pText = "对子复选";//二同号复选
        else if (Types == 7)
            pText = "组二单选";//二同号单选
        else if (Types == 8)
            pText = "复选任二";//二不同号
        else if (Types == 9)
            pText = "押注大小";//大小投注
        else if (Types == 10)
            pText = "押注单双";//单双投注
        return pText;
    }

    //显示投注规则
    private string OutRule(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "总和复选玩法提示：<br />至少选择1个号码（3个号码之和）进行投注，所选号码与开奖的3个号码的总和相同即中奖。<br/>总和中奖倍数分别为：4和17(" + Sum2 + ")倍、5和16(" + XSum1 + ")倍、6和15(" + XSum2 + ")倍、7和14(" + XSum3 + ")倍、8和13(" + XSum4 + ")倍、9和12(" + XSum5 + ")倍、10和11(" + Sum + ")倍。<br />";
        else if (Types == 2)
            pText = "任意豹子玩法提示：<br />对所有相同的三个号码（111、222、333、444、555、666）进行投注，任意号码开出即中奖，单注奖金" + Three_Same_All + "倍。<br />";
        else if (Types == 3)
            pText = "直选豹子玩法提示： <br />对相同的三个号码（111、222、333、444、555、666）中的任意一个进行投注，所选号码开出即中奖，单注奖金" + Three_Same_Single + "倍。<br />";
        else if (Types == 4)
            pText = "复选任三玩法提示： <br />从1～6中任选3个或多个号码，所选号码与开奖号码的3个号码相同即中奖，单注奖金" + Three_Same_Not + "倍。<br />";
        else if (Types == 5)
            pText = "三连通选玩法提示： <br />对所有3个相连的号码（123、234、345、456）进行投注，任意号码开出即中奖，单注奖金" + Three_Continue_All + "倍。<br />";
        else if (Types == 6)
            pText = "对子复选玩法提示： <br />从11～66中任选1个或多个号码，选号与奖号（包含11～66，不限顺序）相同，即中奖" + Two_Same_All + "倍。<br />";
        else if (Types == 7)
            pText = "组二单选玩法提示： <br />选择1对相同号码和1个不同号码投注，选号与奖号相同（顺序不限），即中奖" + Two_Same_Single + "倍。<br />";
        else if (Types == 8)
            pText = "复选任二玩法提示： <br />从1～6中任选2个或多个号码，所选号码与开奖号码任意2个号码相同，即中奖" + Two_dissame + "倍。<br />";
        else if (Types == 9)
            pText = "押注大小玩法提示： <br />根据所开号码，若相加和值少于等于10为小即中奖" + Xiao + "倍，若大于等于11为大即中奖" + Da + "倍，若开奖号码3个相同，即不中奖。<br />";
        else if (Types == 10)
            pText = "押注单双玩法提示： <br />根据所开号码，若相加和值的个位是单即中奖" + Dan + "倍，若为双即中奖" + Shuang + "倍。若开奖号码3个相同，即不中奖。<br />";
        return pText;
    }

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("xOnTime", xmlPath);//20160423
        //string OnTime = "09:26-22:26";
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
            else
                IsOpen = false;
        }
        return IsOpen;
    }

    /// 获取后几位数
    /// <param name="str">要截取的字符串</param>
    /// <param name="num">返回的具体位数</param>
    /// <returns>返回结果的字符串</returns>
    public string GetLastStr(string str, int num)
    {
        int count = 0;
        if (str.Length > num)
        {
            count = str.Length - num;
            str = str.Substring(count, num);
        }
        return str;
    }

    // 更新期数
    public string UpdateState()
    {
        string OnTime = ub.GetSub("xOnTime", xmlPath);//09:26-22:26
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                string state = string.Empty;
                if (DateTime.Now <= dt2 && DateTime.Now >= dt1)
                {
                    string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();

                    decimal dt4 = Convert.ToDecimal(dt3);
                    int dt5 = Convert.ToInt32(dt4 / 10);
                    string dt6 = dt5.ToString();
                    if (dt6.Length == 1)
                    {
                        dt6 = "00" + dt6;
                    }
                    if (dt6.Length == 2)
                    {
                        dt6 = "0" + dt6;
                    }
                    state = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6) + dt6;
                    BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
                    model.Lottery_issue = state;
                    model.Lottery_num = "";
                    string datee = DateTime.ParseExact((("20" + state.Substring(0, 6)) + " 09:26:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    //根据期号算时间,
                    if (int.Parse(GetLastStr(state, 2)) < 10)//如果少于10
                    {
                        model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 1))));
                    }
                    else
                    {
                        model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2))));
                    }
                    model.UpdateTime = DateTime.Now;

                    bool s = new BCW.XinKuai3.BLL.XK3_Internet_Data().Exists_num(state);
                    switch (s)
                    {
                        case false:
                            new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model);
                            break;
                        case true:
                            break;
                    }
                    return state;
                }
            }
        }
        return "0";
    }

    //更新浮动
    private void UpdateFodong()
    {
        //判断是否已经过了时间，过了，然后重置大小单双赔率，令第一期重置 20160516 邵广林
        //if (issue3 == "078")
        if (IsOpen() == false)//不开奖
        {
            ub xml = new ub();
            string xmlPath_update = "/Controls/xinkuai3.xml";
            Application.Remove(xmlPath_update);//清缓存
            xml.ReloadSub(xmlPath_update); //加载配置
            xml.dss["Xda1"] = xml.dss["Xda"];
            xml.dss["Xxiao1"] = xml.dss["Xxiao"];
            xml.dss["Xdan1"] = xml.dss["Xdan"];
            xml.dss["Xshuang1"] = xml.dss["Xshuang"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
    }

    //底部
    private void GameFoot()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    //快捷下注
    private void kuai(int type, int ptype, string Num1, string Num2)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!new BCW.XinKuai3.BLL.Public_User().Exists(meid, type))//添加默认的快捷下注
        {
            BCW.XinKuai3.Model.Public_User model = new BCW.XinKuai3.Model.Public_User();
            model.UsID = meid;
            model.UsName = new BCW.BLL.User().GetUsName(meid);
            model.Type = 1;//1新快3、2幸运28、3好彩一、4进球彩、5百家乐
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
                builder.Append("<a href =\"" + Utils.getUrl("xk3.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num1=" + Num1 + "&amp;Num2=" + Num2 + "&amp;Price=" + Convert.ToInt64(k[i]) + "") + "\">" + gold + "</a>" + "|");
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
