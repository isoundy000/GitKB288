using System;
using System.Collections.Generic;
using System.Data;
using BCW.Common;
using System.Text.RegularExpressions;

public partial class bbs_game_klsf : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/klsf.xml";
    protected int gid = 32;
    protected string GameName = ub.GetSub("klsfName", "/Controls/klsf.xml");
    protected string klsfDemoIDS = ub.GetSub("klsfDemoIDS", "/Controls/klsf.xml");
    protected int klsfSec = Utils.ParseInt(ub.GetSub("klsfSec", "/Controls/klsf.xml"));
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("klsfStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        //内测判断 0,内测是否开启1，是否为内测账号
        if (ub.GetSub("klsfStatus", xmlPath) == "2")//内测
        {
            string[] sNum = Regex.Split(klsfDemoIDS, "#");
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
                Utils.Error("内测中..你没获得测试的资格，谢谢。", "");
            }
        }


        Master.Title = "" + GameName + "";
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
            case "info":
                InfoPage();
                break;
            case "pay":
                PayPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "mylistview":
                MyListViewPage();
                break;
            case "top":
                TopPage();
                break;
            case "rule":
                RulePage();
                break;
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "past":
                PastStages();
                break;
            case "getTest":
                GetMoney();
                break;
            case "order":
                OrderPage();
                break;
            case "trends":
                TrendsPage();
                break;
            //case "open":
            //    OpenPage();
            //    break;
            default:
                ReloadPage();
                break;
        }
    }

    /// <summary>
    /// 游戏首页页面
    /// 蒙宗将 20160906 排版更新
    /// 蒙宗将 20160908 增加龙虎投注
    /// 蒙宗将 20160910 修改期数错误
    /// 蒙宗将 20160913 胆拖没选情况
    /// 蒙宗将 20160923 屏蔽系统号投注统计的ID
    /// 蒙宗将 20160929 排版修改
    /// 蒙宗将 20160930 赔率调整
    /// 蒙宗将 20161004 排行榜前100
    /// 蒙宗将 20161007 修复任选四赔率
    /// 蒙宗将 20161010 排行榜前100条
    /// 蒙宗将 20161013 投注上限提示
    /// 蒙宗将 20161025 新倒计时
    /// 蒙宗将 20161027 添加快捷下注
    ///  蒙宗将 快捷下注转换成万 20161028
    ///  蒙宗将 20161103 动态修复 默认为0的投注框修改为空
    /// </summary>
    private void ReloadPage()
    {
        #region 取得数据库内最新一条的记录
        BCW.Model.klsflist model = new BCW.Model.klsflist();
        model = new BCW.BLL.klsflist().GetklsflistLast();
        int meid = new BCW.User.Users().GetUsId();
        #endregion

        #region 游戏首页顶部
        GameTop();

        #endregion

        #region 判断有没有logo存放在指定位置，有就显示
        string Logo = ub.GetSub("klsfLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\"width=\"100\" height=\"65\" alt=\"Logo\">/");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion
        #region 判断是否有标语，有就显示
        string Notes = ub.GetSub("klsfNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion

        #region 获取数据库内最后一条已开奖的记录
        builder.Append(Out.Tab("<div>", ""));
        //更新期数
        try
        {
            UpdateState();
        }
        catch
        {

        }


        BCW.Model.klsflist m = new BCW.BLL.klsflist().GetklsflistLast2();  //获得上期结果
        if (m != null)
        {
            builder.Append(GameName + "" + m.klsfId + "期开奖:<br/><a href=\"" + Utils.getUrl("klsf.aspx?act=listview&amp;id=" + m.ID + "") + "\">" + m.Result + "</a><br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 主页面的显示和页面跳转链接
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylist&amp;ptype=1") + "\">未开</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=top") + "\">排行</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=past") + "\">往期分析 </a>|");

        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=list") + "\">历史开奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        #endregion


        #region 判断现在是否在开放时间段内
        builder.Append(Out.Tab("<div>", ""));
        if (IsOpen() == true)
        {
            int Sec = Utils.ParseInt(ub.GetSub("klsfSec", xmlPath));  //xml文件里面的1秒转化成int再添加到 datetime.now 里面
            if (model.EndTime < DateTime.Now.AddSeconds(Sec))
            {
                if ("20" + Utils.Left(model.klsfId.ToString(), 6) != DateTime.Now.ToString("yyyyMMdd"))
                {
                    builder.Append("正在获取下一期信息...");
                }
                else
                {
                    if (Utils.Right(model.klsfId.ToString(), 2) == "97") //取当天期数的最后两位 判断是否等于97
                    {
                        builder.Append("每天97期,今天已开97期");
                    }
                    else
                    {
                        builder.Append("正在获取下一期信息...");
                    }
                }
            }
            else
            {
                #region 根据版本判断时间显示的形式 倒计时还是静止
                //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                //{
                //    builder.Append("第" + model.klsfId + "期投注:<br/>");
                //    string s1 = new BCW.JS.somejs().daojishi2("s1", model.EndTime.AddMinutes(0).AddSeconds(-Sec));
                //    builder.Append("还有" + s1 + "截止");
                //}
                //else
                //{
                //    builder.Append("第" + model.klsfId + "期投注:<br/>");
                //    string s2 = new BCW.JS.somejs().daojishi("s2", model.EndTime.AddMinutes(0).AddSeconds(-Sec));
                //    builder.Append("还有" + s2 + "截止");
                //}

                builder.Append("第" + model.klsfId + "期投注:<br/>");
                string s2 = new BCW.JS.somejs().newDaojishi("s2", model.EndTime.AddMinutes(0).AddSeconds(-Sec));
                builder.Append("还有" + s2 + "截止");

                #endregion
                builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx") + "\">刷新</a>");  //开放使用的时候给用户使用的
            }
        }
        else
        {
            builder.Append("游戏开放时间:" + ub.GetSub("klsfOnTime", xmlPath) + "<br />");
        }
        #endregion
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=2&amp;state=0") + "\">任五普通</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=1&amp;state=0") + "\">任五胆拖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=4&amp;state=0") + "\">任四普通</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=3&amp;state=0") + "\">任四胆拖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=6&amp;state=0") + "\">任三普通</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=5&amp;state=0") + "\">任三胆拖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=8&amp;state=0") + "\">任二普通</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=7&amp;state=0") + "\">任二胆拖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=9&amp;state=0") + "\">连二直选</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=10&amp;state=0") + "\">连二组选</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=11&amp;state=0") + "\">前一红投</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=12&amp;state=0") + "\">前一数投</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=13&amp;state=0") + "\">大小</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=14&amp;state=0") + "\">单双</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=15&amp;state=0") + "\">押注龙虎</a>");
        builder.Append(Out.Tab("</div>", ""));

        #region 读取历史开奖并使用规定的表格输出
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("〓历史开奖〓");
        builder.Append(Out.Tab("</div>", ""));

        int SizeNum = 3;  //最多取三条记录
        string strWhere = "State=1";  //只取已经开奖的记录
        IList<BCW.Model.klsflist> listSSClist = new BCW.BLL.klsflist().Getklsflists(SizeNum, strWhere);  //把符合条件的记录的期数和结果存储到泛型结构中
        if (listSSClist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsflist n in listSSClist)  //遍历泛型结构，取出所有的符合条件的  这个是一个表
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.AppendFormat("<a href=\"" + Utils.getUrl("klsf.aspx?act=listview&amp;id=" + n.ID + "") + "\">{0}期:{1}</a>", n.klsfId, n.Result);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum) //如果符合条件的记录多于想要展示的就显示“更多”链接
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=list") + "\">&gt;&gt;更多开奖记录</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        #region 游戏动态

        //3条实时动态
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("〓游戏动态〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            DataSet ds5 = new BCW.BLL.klsfpay().GetList("top 3 *", " UsID>0 order by ID desc");
            if (ds5 != null && ds5.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                {
                    int UsID = int.Parse(ds5.Tables[0].Rows[i]["UsID"].ToString());
                    string UsName = new BCW.BLL.User().GetUsName(UsID);
                    string addTime = ds5.Tables[0].Rows[i]["AddTime"].ToString();
                    int qishu = int.Parse(ds5.Tables[0].Rows[i]["klsfId"].ToString());
                    int Num = int.Parse(ds5.Tables[0].Rows[i]["Prices"].ToString());
                    TimeSpan time = DateTime.Now - Convert.ToDateTime(addTime);

                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;
                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + GameName + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num 
                        }
                        else if (d == 0 && e == 0)
                            builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + GameName + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num 
                        else if (d == 0)
                            builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + GameName + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num
                        else
                            builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + GameName + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num
                    }
                    else
                        builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + GameName + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");// + Num 

                }
            }
            else
            {
                builder.Append("没有更多数据...<br />");
            }
        }
        catch
        {
            builder.Append("没有数据");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=trends") + "\">>>更多动态</a>");
        builder.Append(Out.Tab("</div>", ""));

        #endregion

        //闲聊显示
        builder.Append(Out.Tab("", ""));
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(33, "klsf.aspx", 5, 0)));
        builder.Append(Out.Tab("", Out.Hr()));

        //游戏底部Ubb
        #region 游戏首页页面底部的显示
        string Foot = ub.GetSub("klsfFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        GameFoot();
        #endregion


    }

    #region 动态
    private void TrendsPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>&gt;游戏动态");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>〓下注动态〓</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        strWhere = " UsID>0 ";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        IList<BCW.Model.klsfpay> GetPay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);

        if (GetPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsfpay model1 in GetPay)
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
                string UsName = new BCW.BLL.User().GetUsName(Convert.ToInt32(model1.UsID));
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.UsID + "") + "\">" + UsName + "</a>" + "在" + GameName + "第" + model1.klsfId + "期下注**" + ub.Get("SiteBz") + "（" + Convert.ToDateTime(model1.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + "）";//+ model1.Prices
                builder.AppendFormat("<a href=\"" + Utils.getUrl("klsf.aspx?act=trends&amp;id=" + model1.klsfId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);


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


        GameFoot();
    }
    #endregion

    /// <summary>
    /// 总历史开奖表
    /// </summary>
    private void ListPage()
    {
        Master.Title = "历史开奖";    //标题

        #region  历史开奖表页面的头部
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;历史开奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓历史开奖〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 历史开奖表的获取和显示
        int pageIndex;  //页数
        int recordCount;  //记录
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo")); //每页显示的记录的多少
        string strWhere = string.Empty;  //声明匹配条件
        strWhere = "State=1";  //必须是已经开奖的
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.klsflist> listSSClist = new BCW.BLL.klsflist().Getklsflists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSClist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsflist n in listSSClist)
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

                builder.AppendFormat("{0}期:<a href=\"" + Utils.getUrl("klsf.aspx?act=listview&amp;id=" + n.ID + "") + "\">{1}</a>", n.klsfId, n.Result);

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
        #endregion

        #region  历史开奖表页面的底部
        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }

    /// <summary>
    /// 从历史开奖表中被选中期数的记录
    /// </summary>
    private void ListViewPage()
    {
        #region  从表格中获取该期的ID并检验是否存在
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误")); //获取跳转的页面所对应的记录的信息
        BCW.Model.klsflist model = new BCW.BLL.klsflist().Getklsflist(id);  //取得对应的记录
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));

        //当没有该条记录的情况
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        #endregion

        //页面标题
        Master.Title = GameName + "第" + model.klsfId + "期投注";

        #region  页面的头部
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;期数详情");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + "第" + model.klsfId + "期投注");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region  从数据库中调出符合条件的记录
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "klsfId=" + model.klsfId + " and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");

            builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");

                //if (IsSWB == 0)
                //    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "快乐币/共" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 1) + "]");
                //else
                //    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 1) + "]");

                if (n.WinCent > 0)
                {
                    #region 根据版本选择快乐币和酷币的获取和显示
                    if (IsSWB == 0)
                        builder.Append("中" + n.iWin + "注/赢" + n.WinCent + "快乐币");
                    else
                        builder.Append(" 赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    #endregion
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
        }
        #endregion

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=list") + "\">历史开奖</a>");
        builder.Append(Out.Tab("</div>", ""));
        #region  页面的底部
        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 我的投注记录
    /// </summary>
    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        #region 根据版本判断是否试玩
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        if (IsSWB == 0)
        {
            BCW.SWB.Model KLB = new BCW.SWB.Model();
            if (new BCW.SWB.BLL().ExistsUserID(meid, gid) == false)
            {
                KLB.UserID = meid;
                KLB.GameID = gid;
                KLB.Money = 0;
                KLB.Permission = 0;
                KLB.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(KLB);
            }
            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, gid);
            if (swb.Permission == 0)
                Utils.Success("很抱歉您没有获得游戏的测试权限！", Utils.getUrl("klsf.aspx")); //测试
        }
        #endregion

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "我的未开投注";
        else
            strTitle = "我的历史投注";

        Master.Title = strTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;" + strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));

        #region  从数据库中调出符合条件的数据
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

        string SSCqi = "";
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (n.klsfId.ToString() != SSCqi)
                {
                    if (n.Result == "")
                        builder.Append("=第" + n.klsfId + "期=<br />");
                    else
                        builder.Append("=第" + n.klsfId + "期=开出:<f style=\"color:red\">" + n.Result + "|和:" + GetHe(n.Result) + "</f><br />");
                }

                #region 根据版本选择快乐币和酷币的获取和显示
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");
                if (IsSWB == 0)
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "快乐币/共" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 1) + "]");
                else
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                #endregion

                if (n.WinCent > 0)
                {
                    #region 根据版本选择快乐币和酷币的获取和显示
                    if (IsSWB == 0)
                        builder.Append("中" + n.iWin + "注/赢" + n.WinCent + "快乐币");
                    else
                        builder.Append("中" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    #endregion
                }
                builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylistview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">详情&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
                SSCqi = n.klsfId.ToString();
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
        #endregion

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", ""));

        #region 页面底部
        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 我的某一条投注记录
    /// </summary>
    private void MyListViewPage()
    {
        #region 判断是否存在该条记录
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        #region 根据版本判断是否试玩
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        if (IsSWB == 0)
        {
            BCW.SWB.Model KLB = new BCW.SWB.Model();
            if (new BCW.SWB.BLL().ExistsUserID(meid, gid) == false)
            {
                KLB.UserID = meid;
                KLB.GameID = gid;
                KLB.Money = 0;
                KLB.Permission = 0;
                KLB.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(KLB);
            }
            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, gid);
            if (swb.Permission == 0)
                Utils.Success("很抱歉您没有获得游戏的测试权限！", Utils.getUrl("klsf.aspx")); //测试
        }
        #endregion

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.klsfpay n = new BCW.BLL.klsfpay().Getklsfpay(id);
        if (n == null || n.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        #endregion

        #region 显示该用户该期的信息
        Master.Title = GameName + "第" + n.klsfId + "期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;" + "第" + n.klsfId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + n.klsfId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (n.Result == "")
            builder.Append("开奖号码:未开奖");
        else
            builder.Append("开奖号码:" + n.Result + "|和:" + GetHe(n.Result) + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        #region 根据版本选择快乐币和酷币的获取和显示
        if (IsSWB == 0)
        {
            builder.Append("类型:<b>" + OutType(n.Types) + "</b><br />投注号码:" + n.Notes + "<br />每注:" + n.Price + "快乐币<br />注数:" + n.iCount + "注<br />花费:" + n.Prices + "快乐币");
        }
        else
        {
            builder.Append("类型:<b>" + OutType(n.Types) + "</b><br />投注号码:" + n.Notes + "<br />每注:" + n.Price + "" + ub.Get("SiteBz") + "<br />注数:" + n.iCount + "注<br />花费:" + n.Prices + "" + ub.Get("SiteBz") + "<br />赔率:" + n.Odds + "");
        }
        #endregion

        builder.Append("<br />下注:" + DT.FormatDate(n.AddTime, 0) + "");
        if (n.WinCent > 0)
        {
            builder.Append("<br />结果:你中了:" + n.iWin + "注");
        }
        else
        {
            if (n.State == 0)
                builder.Append("<br />结果:未开奖");
            else
                builder.Append("<br />结果:未中奖");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (n.WinCent != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                builder.Append("一共:" + n.WinCent + "快乐币<br />");
            }
            else
            {
                builder.Append("一共:" + n.WinCent + ub.Get("SiteBz") + "<br />");
            }
            #endregion
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion

        #region 提交出去的表单
        builder.Append("<form id=\"form1\" method=\"post\" action=\"klsf.aspx\">");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
        #region 根据类型添加
        if (n.Types == 1 || n.Types == 3 || n.Types == 5 || n.Types == 7)
        {
            string[] num = n.Notes.Split('|');
            builder.Append("<input type=\"hidden\" name=\"Num1\" Value=\"" + num[0] + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"Num2\" Value=\"" + num[1] + "\"/>");
        }
        else if (n.Types == 2 || n.Types == 4 || n.Types == 6 || n.Types == 8 || n.Types == 10)
        {
            builder.Append("<input type=\"hidden\" name=\"Num3\" Value=\"" + n.Notes + "\"/>");
        }
        else if (n.Types == 9)
        {
            string[] num = n.Notes.Split('|');
            builder.Append("<input type=\"hidden\" name=\"Num4\" Value=\"" + num[0] + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"Num5\" Value=\"" + num[1] + "\"/>");
        }
        else if (n.Types == 11)
        {
            builder.Append("<input type=\"hidden\" name=\"Num6\" Value=\"" + n.Notes + "\"/>");
        }
        else if (n.Types == 12)
        {
            builder.Append("<input type=\"hidden\" name=\"Num7\" Value=\"" + n.Notes + "\"/>");
        }
        else if (n.Types == 13)
        {
            builder.Append("<input type=\"hidden\" name=\"los\" Value=\"" + n.Notes + "\"/>");
        }
        else
        {
            builder.Append("<input type=\"hidden\" name=\"soe\" Value=\"" + n.Notes + "\"/>");
        }
        #endregion
        builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + n.Types + "\"/>");
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"再买一注\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("</form>");
        #endregion

        #region 页面底部
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx?act=mylist&amp;ptype=2") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        #region 页面顶部
        Master.Title = GameName + "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(GameName + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 从数据库调出符合条件的数据
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Prices>0 and State>0";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (pageIndex >= 10)
            pageIndex = 10;
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().GetklsfpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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
                #region 根据版本选择快乐币和酷币的获取和显示
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                if (IsSWB == 0)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>赢" + n.WinCent + "快乐币");
                }
                else
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                }
                #endregion

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            int Pcount = 0;
            if (recordCount <= 100)
            {
                Pcount = recordCount;
            }
            else
            {
                Pcount = 100;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, Pcount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion

        #region 页面底部
        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 查看发奖页面
    /// </summary>
    private void CaseOkPage()
    {
        #region 判断用户ID和游戏ID
        int meid = new BCW.User.Users().GetUsId();
        string mename = new BCW.BLL.User().GetUsName(meid);
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));
        #endregion

        #region 兑奖操作
        if (new BCW.BLL.klsfpay().ExistsState(pid, meid))
        {
            BCW.Model.klsfpay model = new BCW.BLL.klsfpay().Getklsfpay(pid);
            int guestset = Utils.ParseInt(ub.GetSub("klsfGuestSet", xmlPath));
            new BCW.BLL.klsfpay().UpdateState(pid, 2);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.klsfpay().GetWinCent(pid));
            #region 根据版本选择快乐币和酷币的获取和显示
            int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
            if (IsSWB == 0)
            {
                BCW.User.Users.IsFresh("klsf", 1);//防刷
                new BCW.SWB.BLL().UpdateMoney(meid, winMoney, gid); //快乐币
                if (guestset == 0)
                    new BCW.BLL.Guest().Add(1, meid, mename, "您在" + GameName + "第" + model.klsfId + "期的投注：" + model.Notes + "已经兑奖，获得了" + winMoney + "快乐币。");//开奖提示信息,1表示开奖信息
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "快乐币", Utils.getUrl("klsf.aspx?act=case"), "1");
            }
            else
            {
                BCW.User.Users.IsFresh("klsf", 1);//防刷
                BCW.Model.klsflist idd = new BCW.BLL.klsflist().Getklsflistbyklsfid(model.klsfId);
                new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "" + GameName + "兑奖-" + "[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.klsfId + "[/url]" + "-标识ID" + pid + "");
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.klsfId + "[/url]期兑奖" + winMoney + "|(标识ID" + pid + ")");
                if (guestset == 0)
                    new BCW.BLL.Guest().Add(1, meid, mename, "您在" + GameName + "第" + model.klsfId + "期的投注：" + model.Notes + "已经兑奖，获得了" + winMoney + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("klsf.aspx?act=case"), "1");
            }
            #endregion
        }
        else
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("klsf.aspx?act=case"), "1");
        }
        #endregion
    }

    /// <summary>
    /// 该页面兑奖
    /// </summary>
    private void CasePostPage()
    {
        #region 判断用户ID和  ID
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        #endregion

        #region 兑奖操作
        string[] strArrId = arrId.Split(",".ToCharArray());
        long getwinMoney = 0;
        long winMoney = 0;
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        int guestset = Utils.ParseInt(ub.GetSub("klsfGuestSet", xmlPath));
        BCW.User.Users.IsFresh("klsf", 1);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            BCW.Model.klsfpay model = new BCW.BLL.klsfpay().Getklsfpay(pid);
            if (new BCW.BLL.klsfpay().ExistsState(pid, meid))
            {
                new BCW.BLL.klsfpay().UpdateState(pid, 2);
                //操作币
                winMoney = Convert.ToInt64(new BCW.BLL.klsfpay().GetWinCent(pid));
                //版本判断
                #region 根据版本对快乐币和酷币进行操作
                if (IsSWB == 0)
                {
                    new BCW.SWB.BLL().UpdateMoney(meid, winMoney, gid); //快乐币
                }
                else
                {
                    BCW.Model.klsflist idd = new BCW.BLL.klsflist().Getklsflistbyklsfid(model.klsfId);
                    new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "" + GameName + "兑奖-" + "[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.klsfId + "[/url]" + "-标识ID" + pid + "");
                    if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                        new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.klsfId + "[/url]期兑奖" + winMoney + "(标识ID" + pid + ")");
                    if (guestset == 0)
                        new BCW.BLL.Guest().Add(1, meid, mename, "您在" + GameName + "第" + model.klsfId + "期的投注：" + model.Notes + "已经兑奖，获得了" + winMoney + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                }
                #endregion
            }
            getwinMoney = getwinMoney + winMoney;
        }
        #region 根据版本选择显示快乐币和酷币消息
        if (IsSWB == 0)
        {
            Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "快乐币", Utils.getUrl("klsf.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("klsf.aspx?act=case"), "1");
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 兑奖主页面
    /// </summary>
    private void CasePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = GameName + "兑奖中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;兑奖中心");
        builder.Append(Out.Tab("</div>", "<br />"));
        #region 快乐币和酷币的获取和显示
        builder.Append(Out.Tab("<div>", ""));
        #region 根据版本选择快乐币和酷币的获取和显示
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        if (IsSWB == 0)
        {
            builder.Append("你目前自带" + Utils.ConvertGold(new BCW.SWB.BLL().GeUserGold(meid, gid)) + "快乐币<br />");
        }
        else
        {
            builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br />");
        }
        #endregion
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 从数据库中调出符合条件的数据
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and WinCent>0 and State=1";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        string SSCqi = "";
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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
                if (n.klsfId.ToString() != SSCqi)
                {
                    if (n.Result == "")
                        builder.Append("=第" + n.klsfId + "期=<br />");
                    else
                        builder.Append("=第" + n.klsfId + "期=开出:<f style=\"color:red\">" + n.Result + "|和:" + GetHe(n.Result) + "</f><br />");
                }

                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");

                #region 根据版本选择快乐币和酷币的获取和显示
                if (IsSWB == 0)
                {
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "快乐币/共" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 1) + "]");
                }
                else
                {
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/" + "赔率" + n.Odds + "");//[" + DT.FormatDate(n.AddTime, 1) + "]
                }
                #endregion
                builder.Append(",共");//" + GameName + "投注统计:

                #region 根据版本选择快乐币和酷币的获取和显示
                if (IsSWB == 0)
                {
                    builder.Append("中奖" + n.iWin + "注,赢" + n.WinCent + "快乐币");
                }
                else
                {
                    builder.Append("中奖" + n.iWin + "注,赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                }
                #endregion

                builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

                SSCqi = n.klsfId.ToString();
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
        #endregion

        #region 发送表格 本页兑奖
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,klsf.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        #endregion
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 页面底部
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 中奖规则页面
    /// </summary>
    private void RulePage()
    {
        #region 页面顶部
        Master.Title = "" + GameName + "中奖规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("=" + GameName + "中奖规则=");
        //builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 提示信息
        builder.Append(Out.Tab("<div>", ""));
        // builder.Append("" + GameName + "是从01—20个号码中任选1—5个号码组合为一注彩票进行投注。<br />每期从各位上开出1个号码作为中奖号码，即开奖号码为8位数。<br />" + GameName + "玩法是竞猜8位开奖号码的全部号码、部分号码。<br />" + GameName + "10分钟开奖，每天97期，游戏开奖时间：10:00～凌晨2:00。<br />");

        // builder.Append("1.任选五胆拖：" + OutRule(1).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("2.任选五普通：" + OutRule(2).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("3.任选四胆拖：" + OutRule(3).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("4.任选四普通：" + OutRule(4).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("5.任选三胆拖：" + OutRule(5).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("6.任选三普通：" + OutRule(6).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("7.任选二胆拖：" + OutRule(7).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("8.任选二普通：" + OutRule(8).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("9.连二直选：" + OutRule(9).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("10.连二组选：" + OutRule(10).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("11.前一红投：" + OutRule(11).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("12.前一数投：" + OutRule(12).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("13.大小：" + OutRule(13).Split("===".ToCharArray())[0]).Replace("#", "");
        // builder.Append("14.单双：" + OutRule(14).Split("===".ToCharArray())[0]).Replace("#", "");
        //builder.Append("15.龙虎：取当期开奖结果的8个开奖号<br />1与8位：第1开奖号码大于第8开奖号码为龙，小于为虎；<br />2与7位：第2开奖号码大于第7开奖号码为龙，小于为虎；<br />3与6位：第3开奖号码大于第6开奖号码为龙，小于为虎；<br />4与5位：第4开奖号码大于第5开奖号码为龙，小于为虎<br />");


        builder.Append(Out.SysUBB(ub.GetSub("klsfRule", xmlPath)));

        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 页面底部
        builder.Append(Out.Tab("", Out.RHr()));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 投注页面
    /// </summary>
    private void InfoPage()
    {
        #region 判断用户ID、期数和时间是否正确
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        #region 根据版本判断是否试玩
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        if (IsSWB == 0)
        {
            BCW.SWB.Model KLB = new BCW.SWB.Model();
            if (new BCW.SWB.BLL().ExistsUserID(meid, gid) == false)
            {
                KLB.UserID = meid;
                KLB.GameID = gid;
                KLB.Money = 0;
                KLB.Permission = 0;
                KLB.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(KLB);
            }
            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, gid);
            if (swb.Permission == 0)
                Utils.Success("很抱歉您没有获得游戏的测试权限！", Utils.getUrl("klsf.aspx")); //测试
        }
        #endregion

        BCW.Model.klsflist model = new BCW.BLL.klsflist().GetklsflistLast();

        if (model.ID == 0)
        {
            Utils.Error("正在处理开奖，请稍后...", Utils.getUrl("klsf.aspx"));
        }
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 2, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$|^15$", "类型选择错误"));
        double klsfOdds13 = Utils.ParseDouble(ub.GetSub("klsfOdds13", xmlPath));//大        
        double klsfOdds14 = Utils.ParseDouble(ub.GetSub("klsfOdds14", xmlPath));//小
        double klsfOdds15 = Utils.ParseDouble(ub.GetSub("klsfOdds15", xmlPath));//单
        double klsfOdds16 = Utils.ParseDouble(ub.GetSub("klsfOdds16", xmlPath));//双
        double klsfOdds17 = Utils.ParseDouble(ub.GetSub("klsfOdds17", xmlPath));//浮动
        double klsfOdds18 = Utils.ParseDouble(ub.GetSub("klsfOdds18", xmlPath));//龙18
        double klsfOdds19 = Utils.ParseDouble(ub.GetSub("klsfOdds19", xmlPath));//虎81
        double klsfOdds20 = Utils.ParseDouble(ub.GetSub("klsfOdds20", xmlPath));//龙27
        double klsfOdds21 = Utils.ParseDouble(ub.GetSub("klsfOdds21", xmlPath));//虎72
        double klsfOdds22 = Utils.ParseDouble(ub.GetSub("klsfOdds22", xmlPath));//龙36
        double klsfOdds23 = Utils.ParseDouble(ub.GetSub("klsfOdds23", xmlPath));//虎63
        double klsfOdds24 = Utils.ParseDouble(ub.GetSub("klsfOdds24", xmlPath));//龙45
        double klsfOdds25 = Utils.ParseDouble(ub.GetSub("klsfOdds25", xmlPath));//虎54
        //int state = Utils.ParseInt(Utils.GetRequest("state", "get", 2, @"^[0-1]", "错误路径"));

        int Sec = Utils.ParseInt(ub.GetSub("klsfSec", xmlPath));
        if (model.EndTime < DateTime.Now.AddSeconds(Sec))
        {
            Utils.Error("第" + model.klsfId + "期已截止下注,等待开奖...", Utils.getUrl("klsf.aspx"));
        }
        #endregion

        #region 该期信息显示
        string TypeTitle = OutType(ptype);
        Master.Title = GameName + TypeTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;" + TypeTitle);
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 根据版本选择快乐币和酷币的获取和显示
        builder.Append(Out.Tab("<div>", ""));
        if (IsSWB == 0)
        {
            int getTime = int.Parse(ub.GetSub("GetTime", xmlPath));
            BCW.SWB.Model KLB = new BCW.SWB.Model();

            #region 判断是否存在这个ID，不存在就添加新的时间
            if (new BCW.SWB.BLL().ExistsUserID(meid, gid) == true)
                KLB = new BCW.SWB.BLL().GetModelForUserId(meid, gid);
            else
            {
                KLB.UserID = meid;
                KLB.GameID = gid;
                KLB.Money = 0;
                KLB.Permission = 0;
                KLB.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(KLB);
            }
            builder.Append("你目前自带" + Utils.ConvertGold(new BCW.SWB.BLL().GeUserGold(meid, gid)) + "快乐币");
            #endregion

            #region 判断是否到领取时间和根据版本选择时间显示形式
            DateTime tNow = DateTime.Now;
            DateTime tGet = KLB.UpdateTime.AddMinutes(getTime);
            if (tNow > tGet)
            {
                builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=getTest") + "\">|马上领取</a><br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    builder.Append("还有<b style=\"color:#f33\">" + DT.DateDiff(DateTime.Now, KLB.UpdateTime.AddMinutes(getTime)) + "</b>可以领取");
                    builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=getTest") + "\">领取</a><br />");
                }
                else
                {
                    string restTime = new BCW.JS.somejs().daojishi("kk", KLB.UpdateTime.AddMinutes(getTime));
                    builder.Append("还有<b style=\"color:#f33\">" + restTime + "</b>可以领取");
                    builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=getTest") + "\">领取</a><br />");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        else
            builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 根据版本判断时间显示的形式 倒计时还是静止
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("第" + model.klsfId + "期投注:<br/>");
        string s1 = new BCW.JS.somejs().newDaojishi("s1", model.EndTime.AddMinutes(0).AddSeconds(-Sec));
        builder.Append("还有" + s1 + "截止");

        builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=" + ptype + "") + "\">刷新</a><br />");  //开放使用的时候给用户使用的
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("类型：<b>" + TypeTitle + "</b><br />");
        string[] rules = OutRule(ptype).Split('#');
        builder.Append("" + rules[1] + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 输入框和表格提交
        builder.Append("<form id=\"form1\" method=\"post\" action=\"klsf.aspx\">");
        #region 根据投注类型的不同显示不同投注类型的输入框
        if (ptype == 11)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>19</b><input type=\"checkbox\" name=\"Num7\" value=\"19\"/> ");
            builder.Append("<b>20</b><input type=\"checkbox\" name=\"Num7\" value=\"20\"/> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("胆码: ");
            builder.Append("<b>01</b><input type=\"checkbox\" name=\"Num1\" value=\"01\" /> ");
            builder.Append("<b>02</b><input type=\"checkbox\" name=\"Num1\" value=\"02\" /> ");
            builder.Append("<b>03</b><input type=\"checkbox\" name=\"Num1\" value=\"03\" /> ");
            builder.Append("<b>04</b><input type=\"checkbox\" name=\"Num1\" value=\"04\" /> ");
            builder.Append("<b>05</b><input type=\"checkbox\" name=\"Num1\" value=\"05\" /> ");
            builder.Append("<b>06</b><input type=\"checkbox\" name=\"Num1\" value=\"06\" /> ");
            builder.Append("<b>07</b><input type=\"checkbox\" name=\"Num1\" value=\"07\" /> ");
            builder.Append("<b>08</b><input type=\"checkbox\" name=\"Num1\" value=\"08\" /> ");
            builder.Append("<b>09</b><input type=\"checkbox\" name=\"Num1\" value=\"09\" /> ");
            builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num1\" value=\"10\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            else
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            builder.Append("<b>11</b><input type=\"checkbox\" name=\"Num1\" value=\"11\" /> ");
            builder.Append("<b>12</b><input type=\"checkbox\" name=\"Num1\" value=\"12\" /> ");
            builder.Append("<b>13</b><input type=\"checkbox\" name=\"Num1\" value=\"13\" /> ");
            builder.Append("<b>14</b><input type=\"checkbox\" name=\"Num1\" value=\"14\" /> ");
            builder.Append("<b>15</b><input type=\"checkbox\" name=\"Num1\" value=\"15\" /> ");
            builder.Append("<b>16</b><input type=\"checkbox\" name=\"Num1\" value=\"16\" /> ");
            builder.Append("<b>17</b><input type=\"checkbox\" name=\"Num1\" value=\"17\" /> ");
            builder.Append("<b>18</b><input type=\"checkbox\" name=\"Num1\" value=\"18\" /> ");
            builder.Append("<b>19</b><input type=\"checkbox\" name=\"Num1\" value=\"19\" /> ");
            builder.Append("<b>20</b><input type=\"checkbox\" name=\"Num1\" value=\"20\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("---------");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("拖码: ");
            builder.Append("<b>01</b><input type=\"checkbox\" name=\"Num2\" value=\"01\" /> ");
            builder.Append("<b>02</b><input type=\"checkbox\" name=\"Num2\" value=\"02\" /> ");
            builder.Append("<b>03</b><input type=\"checkbox\" name=\"Num2\" value=\"03\" /> ");
            builder.Append("<b>04</b><input type=\"checkbox\" name=\"Num2\" value=\"04\" /> ");
            builder.Append("<b>05</b><input type=\"checkbox\" name=\"Num2\" value=\"05\" /> ");
            builder.Append("<b>06</b><input type=\"checkbox\" name=\"Num2\" value=\"06\" /> ");
            builder.Append("<b>07</b><input type=\"checkbox\" name=\"Num2\" value=\"07\" /> ");
            builder.Append("<b>08</b><input type=\"checkbox\" name=\"Num2\" value=\"08\" /> ");
            builder.Append("<b>09</b><input type=\"checkbox\" name=\"Num2\" value=\"09\" /> ");
            builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num2\" value=\"10\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            else
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            builder.Append("<b>11</b><input type=\"checkbox\" name=\"Num2\" value=\"11\" /> ");
            builder.Append("<b>12</b><input type=\"checkbox\" name=\"Num2\" value=\"12\" /> ");
            builder.Append("<b>13</b><input type=\"checkbox\" name=\"Num2\" value=\"13\" /> ");
            builder.Append("<b>14</b><input type=\"checkbox\" name=\"Num2\" value=\"14\" /> ");
            builder.Append("<b>15</b><input type=\"checkbox\" name=\"Num2\" value=\"15\" /> ");
            builder.Append("<b>16</b><input type=\"checkbox\" name=\"Num2\" value=\"16\" /> ");
            builder.Append("<b>17</b><input type=\"checkbox\" name=\"Num2\" value=\"17\" /> ");
            builder.Append("<b>18</b><input type=\"checkbox\" name=\"Num2\" value=\"18\" /> ");
            builder.Append("<b>19</b><input type=\"checkbox\" name=\"Num2\" value=\"19\" /> ");
            builder.Append("<b>20</b><input type=\"checkbox\" name=\"Num2\" value=\"20\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>01</b><input type=\"checkbox\" name=\"Num3\" value=\"01\" /> ");
            builder.Append("<b>02</b><input type=\"checkbox\" name=\"Num3\" value=\"02\" /> ");
            builder.Append("<b>03</b><input type=\"checkbox\" name=\"Num3\" value=\"03\" /> ");
            builder.Append("<b>04</b><input type=\"checkbox\" name=\"Num3\" value=\"04\" /> ");
            builder.Append("<b>05</b><input type=\"checkbox\" name=\"Num3\" value=\"05\" /> ");
            builder.Append("<b>06</b><input type=\"checkbox\" name=\"Num3\" value=\"06\" /> ");
            builder.Append("<b>07</b><input type=\"checkbox\" name=\"Num3\" value=\"07\" /> ");
            builder.Append("<b>08</b><input type=\"checkbox\" name=\"Num3\" value=\"08\" /> ");
            builder.Append("<b>09</b><input type=\"checkbox\" name=\"Num3\" value=\"09\" /> ");
            builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num3\" value=\"10\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>11</b><input type=\"checkbox\" name=\"Num3\" value=\"11\" /> ");
            builder.Append("<b>12</b><input type=\"checkbox\" name=\"Num3\" value=\"12\" /> ");
            builder.Append("<b>13</b><input type=\"checkbox\" name=\"Num3\" value=\"13\" /> ");
            builder.Append("<b>14</b><input type=\"checkbox\" name=\"Num3\" value=\"14\" /> ");
            builder.Append("<b>15</b><input type=\"checkbox\" name=\"Num3\" value=\"15\" /> ");
            builder.Append("<b>16</b><input type=\"checkbox\" name=\"Num3\" value=\"16\" /> ");
            builder.Append("<b>17</b><input type=\"checkbox\" name=\"Num3\" value=\"17\" /> ");
            builder.Append("<b>18</b><input type=\"checkbox\" name=\"Num3\" value=\"18\" /> ");
            builder.Append("<b>19</b><input type=\"checkbox\" name=\"Num3\" value=\"19\" /> ");
            builder.Append("<b>20</b><input type=\"checkbox\" name=\"Num3\" value=\"20\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 9)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("前码: ");
            builder.Append("<b>01</b><input type=\"checkbox\" name=\"Num4\" value=\"01\" /> ");
            builder.Append("<b>02</b><input type=\"checkbox\" name=\"Num4\" value=\"02\" /> ");
            builder.Append("<b>03</b><input type=\"checkbox\" name=\"Num4\" value=\"03\" /> ");
            builder.Append("<b>04</b><input type=\"checkbox\" name=\"Num4\" value=\"04\" /> ");
            builder.Append("<b>05</b><input type=\"checkbox\" name=\"Num4\" value=\"05\" /> ");
            builder.Append("<b>06</b><input type=\"checkbox\" name=\"Num4\" value=\"06\" /> ");
            builder.Append("<b>07</b><input type=\"checkbox\" name=\"Num4\" value=\"07\" /> ");
            builder.Append("<b>08</b><input type=\"checkbox\" name=\"Num4\" value=\"08\" /> ");
            builder.Append("<b>09</b><input type=\"checkbox\" name=\"Num4\" value=\"09\" /> ");
            builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num4\" value=\"10\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            else
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            builder.Append("<b>11</b><input type=\"checkbox\" name=\"Num4\" value=\"11\" /> ");
            builder.Append("<b>12</b><input type=\"checkbox\" name=\"Num4\" value=\"12\" /> ");
            builder.Append("<b>13</b><input type=\"checkbox\" name=\"Num4\" value=\"13\" /> ");
            builder.Append("<b>14</b><input type=\"checkbox\" name=\"Num4\" value=\"14\" /> ");
            builder.Append("<b>15</b><input type=\"checkbox\" name=\"Num4\" value=\"15\" /> ");
            builder.Append("<b>16</b><input type=\"checkbox\" name=\"Num4\" value=\"16\" /> ");
            builder.Append("<b>17</b><input type=\"checkbox\" name=\"Num4\" value=\"17\" /> ");
            builder.Append("<b>18</b><input type=\"checkbox\" name=\"Num4\" value=\"18\" /> ");
            builder.Append("<b>19</b><input type=\"checkbox\" name=\"Num4\" value=\"19\" /> ");
            builder.Append("<b>20</b><input type=\"checkbox\" name=\"Num4\" value=\"20\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("---------");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("后码: ");
            builder.Append("<b>01</b><input type=\"checkbox\" name=\"Num5\" value=\"01\" /> ");
            builder.Append("<b>02</b><input type=\"checkbox\" name=\"Num5\" value=\"02\" /> ");
            builder.Append("<b>03</b><input type=\"checkbox\" name=\"Num5\" value=\"03\" /> ");
            builder.Append("<b>04</b><input type=\"checkbox\" name=\"Num5\" value=\"04\" /> ");
            builder.Append("<b>05</b><input type=\"checkbox\" name=\"Num5\" value=\"05\" /> ");
            builder.Append("<b>06</b><input type=\"checkbox\" name=\"Num5\" value=\"06\" /> ");
            builder.Append("<b>07</b><input type=\"checkbox\" name=\"Num5\" value=\"07\" /> ");
            builder.Append("<b>08</b><input type=\"checkbox\" name=\"Num5\" value=\"08\" /> ");
            builder.Append("<b>09</b><input type=\"checkbox\" name=\"Num5\" value=\"09\" /> ");
            builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num5\" value=\"10\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            else
            {
                builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            builder.Append("<b>11</b><input type=\"checkbox\" name=\"Num5\" value=\"11\" /> ");
            builder.Append("<b>12</b><input type=\"checkbox\" name=\"Num5\" value=\"12\" /> ");
            builder.Append("<b>13</b><input type=\"checkbox\" name=\"Num5\" value=\"13\" /> ");
            builder.Append("<b>14</b><input type=\"checkbox\" name=\"Num5\" value=\"14\" /> ");
            builder.Append("<b>15</b><input type=\"checkbox\" name=\"Num5\" value=\"15\" /> ");
            builder.Append("<b>16</b><input type=\"checkbox\" name=\"Num5\" value=\"16\" /> ");
            builder.Append("<b>17</b><input type=\"checkbox\" name=\"Num5\" value=\"17\" /> ");
            builder.Append("<b>18</b><input type=\"checkbox\" name=\"Num5\" value=\"18\" /> ");
            builder.Append("<b>19</b><input type=\"checkbox\" name=\"Num5\" value=\"19\" /> ");
            builder.Append("<b>20</b><input type=\"checkbox\" name=\"Num5\" value=\"20\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 12)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>01</b><input type=\"checkbox\" name=\"Num6\" value=\"01\" /> ");
            builder.Append("<b>02</b><input type=\"checkbox\" name=\"Num6\" value=\"02\" /> ");
            builder.Append("<b>03</b><input type=\"checkbox\" name=\"Num6\" value=\"03\" /> ");
            builder.Append("<b>04</b><input type=\"checkbox\" name=\"Num6\" value=\"04\" /> ");
            builder.Append("<b>05</b><input type=\"checkbox\" name=\"Num6\" value=\"05\" /> ");
            builder.Append("<b>06</b><input type=\"checkbox\" name=\"Num6\" value=\"06\" /> ");
            builder.Append("<b>07</b><input type=\"checkbox\" name=\"Num6\" value=\"07\" /> ");
            builder.Append("<b>08</b><input type=\"checkbox\" name=\"Num6\" value=\"08\" /> ");
            builder.Append("<b>09</b><input type=\"checkbox\" name=\"Num6\" value=\"09\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>10</b><input type=\"checkbox\" name=\"Num6\" value=\"10\" /> ");
            builder.Append("<b>11</b><input type=\"checkbox\" name=\"Num6\" value=\"11\" /> ");
            builder.Append("<b>12</b><input type=\"checkbox\" name=\"Num6\" value=\"12\" /> ");
            builder.Append("<b>13</b><input type=\"checkbox\" name=\"Num6\" value=\"13\" /> ");
            builder.Append("<b>14</b><input type=\"checkbox\" name=\"Num6\" value=\"14\" /> ");
            builder.Append("<b>15</b><input type=\"checkbox\" name=\"Num6\" value=\"15\" /> ");
            builder.Append("<b>16</b><input type=\"checkbox\" name=\"Num6\" value=\"16\" /> ");
            builder.Append("<b>17</b><input type=\"checkbox\" name=\"Num6\" value=\"17\" /> ");
            builder.Append("<b>18</b><input type=\"checkbox\" name=\"Num6\" value=\"18\" /> ");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 13)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>买大</b><input type=\"radio\" name=\"los\" value=\"11\" /> 赔率:" + klsfOdds13 + "<br />");
            builder.Append("<b>买小</b><input type=\"radio\" name=\"los\" value=\"00\" /> 赔率:" + klsfOdds14);
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 14)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>押单</b><input type=\"radio\" name=\"soe\" value=\"11\" /> 赔率:" + klsfOdds15 + "<br />");
            builder.Append("<b>押双</b><input type=\"radio\" name=\"soe\" value=\"00\" /> 赔率:" + klsfOdds16);
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ptype == 15)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[1与8位] ");
            builder.Append("<b>押龙</b><input type=\"radio\" name=\"lh\" value=\"18\" /> ");
            builder.Append("<b>押虎</b><input type=\"radio\" name=\"lh\" value=\"81\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[2与7位] ");
            builder.Append("<b>押龙</b><input type=\"radio\" name=\"lh\" value=\"27\" /> ");
            builder.Append("<b>押虎</b><input type=\"radio\" name=\"lh\" value=\"72\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[3与6位] ");
            builder.Append("<b>押龙</b><input type=\"radio\" name=\"lh\" value=\"36\" /> ");
            builder.Append("<b>押虎</b><input type=\"radio\" name=\"lh\" value=\"63\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[4与5位] ");
            builder.Append("<b>押龙</b><input type=\"radio\" name=\"lh\" value=\"45\" /> ");
            builder.Append("<b>押虎</b><input type=\"radio\" name=\"lh\" value=\"54\" />");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion
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
        #endregion

        #region 页面底部和按钮
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=" + ptype + "") + "\">清空选号</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("类型：<b>" + TypeTitle + "</b><br />");
        //string[] rules = OutRule(ptype).Split('#');
        if (ptype != 15)
            builder.Append("玩法提示：" + rules[0].Replace("<br />", "") + "");
        else
            builder.Append("玩法提示：" + rules[0] + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 13)
        {
            builder.Append(Out.Tab("", Out.Hr()));
            DataSet ds = new BCW.BLL.klsflist().GetList("klsfId,Result", " State=1 Order by klsfId desc ");
            int[] klsfId = new int[10];
            string[] Result = new string[10];
            string kai = string.Empty;
            builder.Append("最近十期开大小情况:<br />");
            int cc = 0;
            if (ds.Tables[0].Rows.Count > 10)
            {
                cc = 10;
            }
            else
            {
                cc = ds.Tables[0].Rows.Count;
            }
            for (int i = 0; i < cc; i++)
            {
                klsfId[i] = Utils.ParseInt(ds.Tables[0].Rows[i]["klsfId"].ToString());
                Result[i] = ds.Tables[0].Rows[i]["Result"].ToString();
                string temp = Convert.ToString(GetHe(Result[i]));
                temp = temp.Substring(temp.Length - 1, 1);
                int sum = Convert.ToInt32(temp);
                if (sum >= 5)
                {
                    kai = "开大";
                }
                else if (sum <= 4)
                {
                    kai = "开小";
                }
                builder.Append(klsfId[i] + "期：和" + GetHe(Result[i]) + kai + "<br />");
            }
        }
        if (ptype == 14)
        {
            builder.Append(Out.Tab("", Out.Hr()));
            DataSet ds = new BCW.BLL.klsflist().GetList("klsfId,Result", " State=1 Order by klsfId desc ");
            int[] klsfId = new int[10];
            string[] Result = new string[10];
            string kai = string.Empty;
            builder.Append("最近十期开单双情况:<br />");
            int cc = 0;
            if (ds.Tables[0].Rows.Count > 10)
            {
                cc = 10;
            }
            else
            {
                cc = ds.Tables[0].Rows.Count;
            }
            for (int i = 0; i < cc; i++)
            {
                klsfId[i] = Utils.ParseInt(ds.Tables[0].Rows[i]["klsfId"].ToString());
                Result[i] = ds.Tables[0].Rows[i]["Result"].ToString();
                string temp = Convert.ToString(GetHe(Result[i]));
                temp = temp.Substring(temp.Length - 1, 1);
                int sum = Convert.ToInt32(temp);
                if (sum % 2 != 0)
                {
                    kai = "开单";
                }
                else if (sum % 2 == 0)
                {
                    kai = "开双";
                }
                builder.Append(klsfId[i] + "期：和" + GetHe(Result[i]) + kai + "<br />");
            }

        }
        if (ptype == 15)
        {
            builder.Append(Out.Tab("", Out.Hr()));
            DataSet ds = new BCW.BLL.klsflist().GetList("klsfId,Result", " State=1 Order by klsfId desc ");
            int[] klsfId = new int[10];
            string[] Result = new string[10];
            string kai1 = string.Empty;
            string kai2 = string.Empty;
            string kai3 = string.Empty;
            string kai4 = string.Empty;
            string kai5 = string.Empty;
            builder.Append("最近十期开龙虎情况:<br />");
            builder.Append("<table>");
            builder.Append("<tr><td>期号</td><td>1-8</td><td>2-7</td><td>3-6</td><td>4-5</td></tr>");
            int cc = 0;
            if (ds.Tables[0].Rows.Count > 10)
            {
                cc = 10;
            }
            else
            {
                cc = ds.Tables[0].Rows.Count;
            }
            for (int i = 0; i < cc; i++)
            {
                klsfId[i] = Utils.ParseInt(ds.Tables[0].Rows[i]["klsfId"].ToString());
                Result[i] = ds.Tables[0].Rows[i]["Result"].ToString();
                builder.Append("<tr><td>" + klsfId[i] + "期：" + "</td><td>" + GetLH(1, Result[i]) + "</td><td>" + GetLH(2, Result[i]) + "</td><td>" + GetLH(3, Result[i]) + "</td><td>" + GetLH(4, Result[i]) + "</td></tr>");
            }
            builder.Append("</table>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 往期分析页面
    /// </summary>
    private void PastStages()
    {
        #region 页面顶部
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append("&gt;往期分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓往期分析〓");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 表格的提交和确认
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string searchDate = DateTime.Now.ToString("yyyyMMdd");

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            searchDate = Utils.GetRequest("date", "all", 2, @"^(\d\d\d\d\d\d\d\d)$", "请输入正确的时间格式");
        }

        //输入框
        string strText = "查询日期(20160808):/,";
        string strName = "date,backurl";
        string strType = "text,hidden";
        string strValu = "" + searchDate + "'" + Utils.getPage(1) + "";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "确定修改|reset,klsf.aspx?act=past,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        #region 从数据库中调出符合条件的数据并统计
        string strWhere = string.Empty;
        strWhere += "klsfId like \'" + searchDate.Remove(0, 2) + "%\' and State <> 0";
        DataSet ds = new BCW.BLL.klsflist().GetList("Result", strWhere);
        int[] num = new int[33];

        //数据总结
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string[] Result = ds.Tables[0].Rows[i]["Result"].ToString().Split(',');
            int temp = 0;
            int sum = 0;
            string temps = string.Empty;
            for (int j = 0; j < Result.Length; j++)
            {
                temp = Convert.ToInt32(Result[j]);
                num[temp]++;
                sum += temp;
            }
            temps = Convert.ToString(sum);
            temps = temps.Substring(temps.Length - 1, 1);
            temp = Convert.ToInt32(temps);
            if (temp > 4)
                num[21]++;
            else
                num[22]++;
            if (sum % 2 != 0)
                num[23]++;
            else
                num[24]++;

            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 1) == 1)
            {
                num[25]++;
            }
            else//   if (Convert.ToInt32(resu[0]) < Convert.ToInt32(resu[7]))
            {
                num[26]++;
            }
            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 2) == 1)
            {
                num[27]++;
            }
            else// if (Convert.ToInt32(resu[6]) < Convert.ToInt32(resu[1]))
            {
                num[28]++;
            }
            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 3) == 1)
            {
                num[29]++;
            }
            else//  if (Convert.ToInt32(resu[5]) < Convert.ToInt32(resu[2]))
            {
                num[30]++;
            }
            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 4) == 1)
            {
                num[31]++;
            }
            else//  if (Convert.ToInt32(resu[4]) < Convert.ToInt32(resu[3]))
            {
                num[32]++;
            }
        }
        #endregion

        #region 显示统计结果
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">冷热奖号统计</h>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        for (int i = 1; i < 21; i++)
        {
            if (i % 4 == 0 && i != 0)
                builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b><br />");
            else
            {
                if (i < 10)
                    builder.Append("<b>" + i + "：<b style=\"color:red\">&nbsp;&nbsp;" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
                else if (i > 9 && i < 13)
                    builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
                else
                    builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
            }
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">大小单双统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>大：<b style=\"color:red\">" + num[21] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[22] + "</b>次</b><br />");
        builder.Append("<b>单：<b style=\"color:red\">" + num[23] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[24] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">龙虎统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>龙（1与8位）：<b style=\"color:red\">" + num[25] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（1与8位）：<b style=\"color:red\">" + num[26] + "</b>次</b><br />");
        builder.Append("<b>龙（2与7位）：<b style=\"color:red\">" + num[27] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（2与7位）：<b style=\"color:red\">" + num[28] + "</b>次</b><br />");
        builder.Append("<b>龙（3与6位）：<b style=\"color:red\">" + num[29] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（3与6位）：<b style=\"color:red\">" + num[30] + "</b>次</b><br />");
        builder.Append("<b>龙（4与5位）：<b style=\"color:red\">" + num[31] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（4与5位）：<b style=\"color:red\">" + num[32] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 页面底部
        builder.Append(Out.Tab("", Out.RHr()));
        GameFoot();
        #endregion
    }

    /// <summary>
    /// 投注支付页面
    /// </summary>
    private void PayPage()
    {
        #region 初始化 获取数据 判断页面状态
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        #region 根据版本判断是否试玩
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        if (IsSWB == 0)
        {
            BCW.SWB.Model KLB = new BCW.SWB.Model();
            if (new BCW.SWB.BLL().ExistsUserID(meid, gid) == false)
            {
                KLB.UserID = meid;
                KLB.GameID = gid;
                KLB.Money = 0;
                KLB.Permission = 0;
                KLB.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(KLB);
            }
            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, gid);
            if (swb.Permission == 0)
                Utils.Success("很抱歉您没有获得游戏的测试权限！", Utils.getUrl("klsf.aspx")); //测试
        }
        #endregion


        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$|^15$", "类型选择错误")); //从地址上获取投注类型的信息
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string TypeTitle = OutType(ptype);  //该投注类型的信息
        Master.Title = TypeTitle;

        double Odds = 0;
        double klsfOdds1 = Utils.ParseDouble(ub.GetSub("klsfOdds1", xmlPath));//任选五胆拖        
        double klsfOdds2 = Utils.ParseDouble(ub.GetSub("klsfOdds2", xmlPath));//任选五普通
        double klsfOdds3 = Utils.ParseDouble(ub.GetSub("klsfOdds3", xmlPath));//任选四胆拖
        double klsfOdds4 = Utils.ParseDouble(ub.GetSub("klsfOdds4", xmlPath));//任选四普通
        double klsfOdds5 = Utils.ParseDouble(ub.GetSub("klsfOdds5", xmlPath));//任选三胆拖
        double klsfOdds6 = Utils.ParseDouble(ub.GetSub("klsfOdds6", xmlPath));//任选三普通
        double klsfOdds7 = Utils.ParseDouble(ub.GetSub("klsfOdds7", xmlPath));//任选二胆拖
        double klsfOdds8 = Utils.ParseDouble(ub.GetSub("klsfOdds8", xmlPath));//任选二普通
        double klsfOdds9 = Utils.ParseDouble(ub.GetSub("klsfOdds9", xmlPath));//连二直选
        double klsfOdds10 = Utils.ParseDouble(ub.GetSub("klsfOdds10", xmlPath));//连二组选
        double klsfOdds11 = Utils.ParseDouble(ub.GetSub("klsfOdds11", xmlPath));//前一红投
        double klsfOdds12 = Utils.ParseDouble(ub.GetSub("klsfOdds12", xmlPath));//前一数投
        double klsfOdds13 = Utils.ParseDouble(ub.GetSub("klsfOdds13", xmlPath));//大        
        double klsfOdds14 = Utils.ParseDouble(ub.GetSub("klsfOdds14", xmlPath));//小
        double klsfOdds15 = Utils.ParseDouble(ub.GetSub("klsfOdds15", xmlPath));//单
        double klsfOdds16 = Utils.ParseDouble(ub.GetSub("klsfOdds16", xmlPath));//双
        double klsfOdds17 = Utils.ParseDouble(ub.GetSub("klsfOdds17", xmlPath));//浮动
        double klsfOdds18 = Utils.ParseDouble(ub.GetSub("klsfOdds18", xmlPath));//龙18
        double klsfOdds19 = Utils.ParseDouble(ub.GetSub("klsfOdds19", xmlPath));//虎81
        double klsfOdds20 = Utils.ParseDouble(ub.GetSub("klsfOdds20", xmlPath));//龙27
        double klsfOdds21 = Utils.ParseDouble(ub.GetSub("klsfOdds21", xmlPath));//虎72
        double klsfOdds22 = Utils.ParseDouble(ub.GetSub("klsfOdds22", xmlPath));//龙36
        double klsfOdds23 = Utils.ParseDouble(ub.GetSub("klsfOdds23", xmlPath));//虎63
        double klsfOdds24 = Utils.ParseDouble(ub.GetSub("klsfOdds24", xmlPath));//龙45
        double klsfOdds25 = Utils.ParseDouble(ub.GetSub("klsfOdds25", xmlPath));//虎54

        BCW.Model.klsflist model = new BCW.BLL.klsflist().GetklsflistLast();
        //数据库内还没有记录的情况
        if (model.ID == 0)
        {
            Utils.Error("正在处理开奖，请稍后...", Utils.getUrl("klsf.aspx"));
        }

        int Sec = Utils.ParseInt(ub.GetSub("klsfSec", xmlPath));
        if (model.EndTime < DateTime.Now.AddSeconds(Sec))
        {
            Utils.Error("第" + model.klsfId + "期已截止下注,等待开奖...", Utils.getUrl("klsf.aspx"));
        }

        //从info页面取来的各种值
        string Num1 = Utils.GetRequest("Num1", "all", 1, @"^[\d((,)\d)?]+$", ""); //胆拖的胆码
        string Num2 = Utils.GetRequest("Num2", "all", 1, @"^[\d((,)\d)?]+$", ""); //胆拖的拖码
        string Num3 = Utils.GetRequest("Num3", "all", 1, @"^[\d((,)\d)?]+$", ""); //任选和组选
        string Num4 = Utils.GetRequest("Num4", "all", 1, @"^[\d((,)\d)?]+$", ""); //直选的前码
        string Num5 = Utils.GetRequest("Num5", "all", 1, @"^[\d((,)\d)?]+$", ""); //直选的后码
        string Num6 = Utils.GetRequest("Num6", "all", 1, @"^[\d((,)\d)?]+$", ""); //前一数投
        string Num7 = Utils.GetRequest("Num7", "all", 1, @"^[\d((,)\d)?]+$", ""); //前一红投
        string los = Utils.GetRequest("los", "all", 1, @"^\d+$", "");//大小
        string soe = Utils.GetRequest("soe", "all", 1, @"^\d+$", "");//单双
        string lh = Utils.GetRequest("lh", "all", 1, @"^\d+$", "");//龙虎

        string accNum = string.Empty;
        string[] str0 = { "|" };
        string[] strTemp = { };
        int iZhu = 0;
        #endregion

        #region 计算注数
        if (ptype == 12) //前一数投
        {
            if (Num6 == "")
            {
                Utils.Error("如果要投注，请至少选择1个号码", "");
            }

            accNum = Num6;
            strTemp = accNum.Split(',');

            iZhu = strTemp.Length;  //注数等于选择数字的个数
            Odds = klsfOdds12;
        }
        else if (ptype == 11) //前一红投
        {
            if (Num7 == "")
            {
                Utils.Error("如果要投注，请至少选择1个号码", "");
            }

            accNum = Num7;
            strTemp = Num7.Split(',');

            iZhu = strTemp.Length; //注数等于选择数字的个数
            Odds = klsfOdds11;
        }
        else if (ptype == 10) //连二组选
        {
            accNum = Num3;
            strTemp = accNum.Split(',');

            if (strTemp.Length < 2)
            {
                Utils.Error("如果要投注，请至少选择2个号码", "");
            }

            iZhu = C(strTemp.Length, 2); //注数等于strTemp和2的组合数
            Odds = klsfOdds10;
        }
        else if (ptype == 9) //连二直选
        {
            int j = 0;
            bool match = false;
            string[] strt1 = Num4.Split(',');
            string[] strt2 = Num5.Split(',');

            if (Num4 == "" || Num5 == "")
            {
                Utils.Error("前码和后码至少各选择1个号码", "");
            }

            if (strt1.Length == 1 && strt2.Length == 1)
            {
                if (strt1[0] == strt2[0])
                {
                    match = true;
                }
                if (match == true)
                {
                    Utils.Error("前码和和后码只选一个的时候，不能相同", "");
                }
            }

            for (int i = 0; i < strt1.Length; i++)
            {
                for (int p = 0; p < strt2.Length; p++)
                {
                    if (strt1[i] != strt2[p])
                    {
                        j++;
                    }
                }
            }

            accNum = Num4 + "," + Num5;
            strTemp = accNum.Split(',');
            accNum = Num4 + "|" + Num5;

            iZhu = j;
            Odds = klsfOdds9;
        }
        else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8) //任选
        {
            accNum = Num3;
            strTemp = accNum.Split(',');

            //builder.Append(Out.Tab("<div class=\"title\">", ""));
            //builder.Append(Out.Tab(strTemp.Length + " " + jc(strTemp.Length - 2) + " " + jc(2) + "" + Num3, ""));
            //builder.Append(Out.Tab("</div>", "<br />"));//在前台显示变量得值 测试用

            if (ptype == 8)
            {
                if (strTemp.Length < 2)
                {
                    Utils.Error("至少选择2个号码", "");
                }
            }
            else if (ptype == 6)
            {
                if (strTemp.Length < 3)
                {
                    Utils.Error("至少选择3个号码", "");
                }
            }
            else if (ptype == 4)
            {
                if (strTemp.Length < 4)
                {
                    Utils.Error("至少选择4个号码", "");
                }
            }
            else if (ptype == 2)
            {
                if (strTemp.Length < 5)
                {
                    Utils.Error("至少选择5个号码", "");
                }
            }

            if (ptype == 8)
            {
                iZhu = C(strTemp.Length, 2);
                Odds = klsfOdds8;
            }
            else if (ptype == 6)
            {
                iZhu = C(strTemp.Length, 3);
                Odds = klsfOdds6;
            }
            else if (ptype == 4)
            {
                iZhu = C(strTemp.Length, 4);
                Odds = klsfOdds4;
            }
            else if (ptype == 2)
            {
                iZhu = C(strTemp.Length, 5);
                Odds = klsfOdds2;
            }
        }
        else if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7) //胆拖
        {
            string[] strt1 = Num1.Split(',');
            string[] strt2 = Num2.Split(',');
            accNum = Num1 + "|" + Num2;

            if (Num1 == "")
                Utils.Error("请选择胆码", "");
            if (Num2 == "")
                Utils.Error("请选择拖码", "");

            if (ptype == 7)
            {
                if (strt1.Length != 1 || strt2[0] == "")
                {
                    Utils.Error("只能选择1个胆码，至少选择1个拖码。", "");
                }
            }
            else if (ptype == 5)
            {
                if (strt1[0] == "" && strt2.Length < 2)
                {
                    Utils.Error("至少选择1个胆码，2个拖码。", "");
                }
                else if (strt1.Length == 1 && strt2.Length < 2)
                {
                    Utils.Error("您选择了1个胆码，请至少选择2个拖码。", "");
                }
                else if (strt1.Length == 2 && strt2[0] == "")
                {
                    Utils.Error("您选择了2个胆码，请至少选择1个拖码。", "");
                }
                else if (strt1.Length > 2)
                {
                    Utils.Error("最多只能选择2个胆码。", "");
                }
            }
            else if (ptype == 3)
            {
                if (strt1[0] == "" && strt2.Length < 3)
                {
                    Utils.Error("至少选择1个胆码，3个拖码。", "");
                }
                else if (strt1.Length == 1 && strt2.Length < 3)
                {
                    Utils.Error("您选择了1个胆码，请至少选择3个拖码。", "");
                }
                else if (strt1.Length == 2 && strt2.Length < 2)
                {
                    Utils.Error("您选择了2个胆码，请至少选择2个拖码。", "");
                }
                else if (strt1.Length == 3 && strt2[0] == "")
                {
                    Utils.Error("您选择了3个胆码，请至少选择1个拖码。", "");
                }
                else if (strt1.Length > 3)
                {
                    Utils.Error("最多只能选择3个胆码。", "");
                }
            }
            else if (ptype == 1)
            {
                if (strt1[0] == "" && strt2.Length < 4)
                {
                    Utils.Error("至少选择1个胆码，4个拖码。", "");
                }
                else if (strt1.Length == 1 && strt2.Length < 4)
                {
                    Utils.Error("您选择了1个胆码，请至少选择4个拖码。", "");
                }
                else if (strt1.Length == 2 && strt2.Length < 3)
                {
                    Utils.Error("您选择了2个胆码，请至少选择3个拖码。", "");
                }
                else if (strt1.Length == 3 && strt2.Length < 2)
                {
                    Utils.Error("您选择了3个胆码，请至少选择2个拖码。", "");
                }
                else if (strt1.Length == 4 && strt2[0] == "")
                {
                    Utils.Error("您选择了4个胆码，请至少选择1个拖码。", "");
                }
                else if (strt1.Length > 4)
                {
                    Utils.Error("最多只能选择4个胆码。", "");
                }
            }
            //查看胆码和拖码有没有相同的
            for (int i = 0; i < strt1.Length; i++)
            {
                for (int p = 0; p < strt2.Length; p++)
                {
                    if (strt1[i] == strt2[p])
                    {
                        Utils.Error("拖码不能和胆码相同,请重新选择号码", "");
                    }
                }
            }
            if (strt1[0] == "" || strt2[0] == "")
            {
                iZhu = 0;
            }

            else
            {
                if (ptype == 7)
                {
                    iZhu = strt2.Length;
                    Odds = klsfOdds7;
                }
                else if (ptype == 5)
                {
                    iZhu = C(strt2.Length, 3 - strt1.Length);
                    Odds = klsfOdds5;
                }
                else if (ptype == 3)
                {
                    iZhu = C(strt2.Length, 4 - strt1.Length);
                    Odds = klsfOdds3;
                }
                else if (ptype == 1)
                {
                    iZhu = C(strt2.Length, 5 - strt1.Length);
                    Odds = klsfOdds1;
                }
            }
        }
        else if (ptype == 13)
        {
            if (los == "")
            {
                Utils.Error("如果要投注，请选择押大或者押小", "");
            }
            if (los == "11")
            {
                accNum = "大";
                Odds = klsfOdds13;
            }
            else
            {
                accNum = "小";
                Odds = klsfOdds14;
            }
            iZhu = 1;
        }
        else if (ptype == 14)
        {
            if (soe == "")
            {
                Utils.Error("如果要投注，请选择押单或者押双", "");
            }
            if (soe == "11")
            {
                accNum = "单";
                Odds = klsfOdds15;
            }
            else
            {
                accNum = "双";
                Odds = klsfOdds16;
            }
            iZhu = 1;
        }
        else if (ptype == 15)
        {
            if (lh == "")
            {
                Utils.Error("如果要投注，请选择押龙或者押虎", "");
            }
            if (lh == "18")
            {
                accNum = "龙（1与8位）";
                Odds = klsfOdds18;
            }
            else if (lh == "81")
            {
                accNum = "虎（1与8位）";
                Odds = klsfOdds19;
            }
            else if (lh == "27")
            {
                accNum = "龙（2与7位）";
                Odds = klsfOdds20;
            }
            else if (lh == "72")
            {
                accNum = "虎（2与7位）";
                Odds = klsfOdds21;
            }
            else if (lh == "36")
            {
                accNum = "龙（3与6位）";
                Odds = klsfOdds22;
            }
            else if (lh == "63")
            {
                accNum = "虎（3与6位）";
                Odds = klsfOdds23;
            }
            else if (lh == "45")
            {
                accNum = "龙（4与5位）";
                Odds = klsfOdds24;
            }
            else if (lh == "54")
            {
                accNum = "虎（4与5位）";
                Odds = klsfOdds25;
            }
            iZhu = 1;
        }

        if (iZhu == 0)
            iZhu = strTemp.Length;

        string strNum = string.Empty;
        #endregion

        //根据获取的数据判断页面所处状态
        #region 接收最终投注信息和显示 投注第三步
        if (info == "ok2")
        {
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "投注额填写错误"));

            #region 不知道干什么用的一个操作
            //支付安全提示 定义数组
            string set_pageArr = string.Empty;

            if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
            {
                set_pageArr = "Price" + "," + "Num1" + "," + "Num2" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr1 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr1);
            }
            else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
            {
                set_pageArr = "Price" + "," + "Num3" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr10 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr10);
            }
            else if (ptype == 9)
            {
                set_pageArr = "Price" + "," + "Num4" + "," + "Num5" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr9 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr9);
            }
            else if (ptype == 11)
            {
                set_pageArr = "Price" + "," + "Num7" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr11 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr11);
            }
            else if (ptype == 12)
            {
                set_pageArr = "Price" + "," + "Num6" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr12 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr12);
            }
            else if (ptype == 13)
            {
                set_pageArr = "Price" + "," + "los" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr13 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr13);
            }
            else if (ptype == 14)
            {
                set_pageArr = "Price" + "," + "soe" + "," + "ptype" + "," + "act" + "," + "info";

                string[] p_pageArr14 = set_pageArr.Split(',');

                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr14);
            }
            #endregion

            #region 判断投注金额是否符合限定条件，符合则提交
            long gold = 0;
            if (IsSWB == 0)
            {
                gold = new BCW.SWB.BLL().GeUserGold(meid, gid);
            }
            else
            {
                gold = new BCW.BLL.User().GetGold(meid);
            }
            long prices = Convert.ToInt64(Price * iZhu);
            //是否刷屏
            long small = Convert.ToInt64(ub.GetSub("klsfSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("klsfBigPay", xmlPath));
            string appName = "LIGHT_SSC";
            int Expir = Utils.ParseInt(ub.GetSub("klsfExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir, Price, small, big);

            if (gold < prices)
            {
                #region 根据版本选择快乐币和酷币的获取和显示
                if (IsSWB == 0)
                {
                    Utils.Error("您的快乐币不足", "");
                }
                else
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                }
                #endregion
            }

            long xPrices = Utils.ParseInt64(ub.GetSub("klsfPrice", xmlPath));
            if (xPrices > 0)
            {
                long oPrices = new BCW.BLL.klsfpay().GetSumPrices(meid, model.klsfId);
                if (oPrices + prices > xPrices)
                {
                    if (oPrices >= xPrices)
                        Utils.Error("您本期下注已达上限，请等待下期...", "");
                    else
                        Utils.Error("您本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                }
            }

            long xPricesc = 0;
            xPricesc = Utils.ParseInt64(ub.GetSub("klsfOddsc" + ptype + "", xmlPath));
            if (xPricesc > 0)
            {
                long oPricesc = new BCW.BLL.klsfpay().GetSumPricebyTypes(ptype, model.klsfId);
                if (oPricesc + prices > xPricesc)
                {
                    if (oPricesc > xPricesc)
                        Utils.Error("本期" + OutType(ptype) + "下注已达上限，请等待下期或者选择其他投注...", "");
                    else
                        Utils.Error("本期" + OutType(ptype) + "最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                }
            }



            string mename = new BCW.BLL.User().GetUsName(meid);

            #endregion

            #region 添加新的数据到klsfpay
            BCW.Model.klsfpay modelpay = new BCW.Model.klsfpay();
            modelpay.klsfId = model.klsfId;
            modelpay.Types = ptype;
            modelpay.UsID = meid;
            modelpay.UsName = mename;
            modelpay.iCount = iZhu;
            modelpay.Price = Price;
            modelpay.State = 0;
            modelpay.Prices = prices;
            modelpay.WinCent = 0;
            modelpay.Result = "";
            modelpay.Notes = accNum;
            modelpay.AddTime = DateTime.Now;
            modelpay.iWin = 0;
            modelpay.isRoBot = 0;
            modelpay.Odds = Convert.ToDecimal(Odds);//赔率
            int id = new BCW.BLL.klsfpay().Add(modelpay);
            #endregion

            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                new BCW.SWB.BLL().UpdateMoney(meid, -prices, gid);
            }
            else
            {
                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + model.ID + "]" + model.klsfId + "[/url]期" + OutType(ptype) + ":" + accNum + "|赔率" + modelpay.Odds + "|标识ID" + id + ""); //酷币
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + model.ID + "]" + model.klsfId + "[/url]期买" + OutType(ptype) + ":" + accNum + "共" + prices + ub.Get("SiteBz") + "|赔率" + modelpay.Odds + "-标识ID" + id + "");
            }
            #endregion

            #region 投注后的系统操作和用户页面显示
            //动态投注后记录投注信息  

            #region 根据版本选择快乐币和酷币的获取和显示
            string wText = string.Empty;
            if (IsSWB == 0)
            {
                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + TypeTitle + "》下注" + prices + "快乐币";
            }
            else
            {
                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + TypeTitle + "》下注**" + "" + ub.Get("SiteBz") + "";//+ prices
            }
            #endregion

            new BCW.BLL.Action().Add(1006, id, meid, "", wText);
            string state = Utils.GetRequest("State", "post", 1, @"^[0-1]*$", "");

            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                Utils.Success("下注", "下注《" + TypeTitle + "》成功，花费了" + prices + "快乐币(共" + iZhu + "注)<br /><a href=\"" + Utils.getUrl("klsf.aspx") + "\">&gt;继续下注</a>", Utils.getUrl("klsf.aspx"), "3");
            }
            else
            {
                Utils.Success("下注", "下注《" + TypeTitle + "》成功，花费了" + prices + "" + ub.Get("SiteBz") + "(共" + iZhu + "注)<br /><a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("klsf.aspx"), "3");
            }
            #endregion

            #endregion
        }
        #endregion

        #region 确定每注投注金额和追号选择 投注第二步
        else if (info == "ok")
        {
            #region 投注信息
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "投注额填写错误"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
            builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
            builder.Append("&gt;" + TypeTitle);
            builder.Append(Out.Tab("</div>", "<br />"));

            #region 根据版本判断时间显示的形式 倒计时还是静止
            builder.Append(Out.Tab("<div>", ""));
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //{
            //    builder.Append("第" + model.klsfId + "期:<br/>");
            //    string s1 = new BCW.JS.somejs().daojishi2("s1", model.EndTime.AddMinutes(0).AddSeconds(-10));
            //    builder.Append("还有" + s1 + "截止");
            //}
            //else
            //{
            //    builder.Append("第" + model.klsfId + "期:<br/>");
            //    string s2 = new BCW.JS.somejs().daojishi("s2", model.EndTime.AddMinutes(0).AddSeconds(-10));
            //    builder.Append("还有" + s2 + "截止");
            //}

            builder.Append("第" + model.klsfId + "期:<br/>");
            string s1 = new BCW.JS.somejs().newDaojishi("s1", model.EndTime.AddMinutes(0).AddSeconds(-10));
            builder.Append("还有" + s1 + "截止");

            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("类型：<b>" + TypeTitle + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("位号：" + accNum + "<br />");
            builder.Append("注数：" + iZhu + "注<br />");
            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                builder.Append("每注：" + Price + "快乐币<br />");
                builder.Append("需花费：" + Price * iZhu + "快乐币<br />");
                long gold = new BCW.SWB.BLL().GeUserGold(meid, gid);
                builder.Append("您自带：" + Utils.ConvertGold(gold) + "快乐币<br />");
            }
            else
            {
                builder.Append("每注：" + Price + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("需花费：" + Price * iZhu + "" + ub.Get("SiteBz") + "<br />");
                long gold = new BCW.BLL.User().GetGold(meid);
                builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
            }
            #endregion
            builder.Append(Out.Tab("</div>", ""));
            #endregion

            #region 提交表单
            string strName = string.Empty;
            string strValu = string.Empty;

            if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
            {
                strName = "Price,Num1,Num2,ptype,act,info";
                strValu = "" + Price + "'" + Num1 + "'" + Num2 + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
            {
                strName = "Price,Num3,ptype,act,info";
                strValu = "" + Price + "'" + Num3 + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 9)
            {
                strName = "Price,Num4,Num5,ptype,act,info";
                strValu = "" + Price + "'" + Num4 + "'" + Num5 + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 11)
            {
                strName = "Price,Num7,ptype,act,info";
                strValu = "" + Price + "'" + Num7 + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 12)
            {
                strName = "Price,Num6,ptype,act,info";
                strValu = "" + Price + "'" + Num6 + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 13)
            {
                strName = "Price,los,ptype,act,info";
                strValu = "" + Price + "'" + los + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 14)
            {
                strName = "Price,soe,ptype,act,info";
                strValu = "" + Price + "'" + soe + "'" + ptype + "'pay'ok2";
            }
            else if (ptype == 15)
            {
                strName = "Price,lh,ptype,act,info";
                strValu = "" + Price + "'" + lh + "'" + ptype + "'pay'ok2";
            }
            string strOthe = "确定投注,klsf.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            #endregion

            //builder.Append("<br/>");

            #region 追号
            //string strNamez = string.Empty;
            //string strValuz = string.Empty;
            //string Notes = string.Empty;
            //#region 根据类型添加
            //if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
            //    Notes = Num1 + "|" + Num2;
            //else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
            //    Notes = Num3;
            //else if (ptype == 9)
            //    Notes = Num4 + "|" + Num5;
            //else if (ptype == 11)
            //    Notes = Num6;
            //else if (ptype == 12)
            //    Notes = Num7;
            //else if (ptype == 13)
            //    Notes = los;
            //else
            //    Notes = soe;
            //#endregion
            //strNamez = "Notes,Price,Prices,iCount,ptype,act";
            //strValuz = Notes + "'" + Price + "'" + Price * iZhu + "'" + iZhu + "'" + ptype + "'order";
            //string str0thez = "追号投注,klsf.aspx,post,0,red";
            //builder.Append(Out.wapform(strNamez, strValuz, str0thez));
            #endregion

            #region 页面底部
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=" + ptype + "") + "\">返回重新选号</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            GameFoot();
            #endregion
        }
        #endregion

        #region 输入每一注投注金额的页面 投注第一步
        else
        {
            #region 投注信息
            long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 1, @"^[1-9]\d*$", "0"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
            builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
            builder.Append("&gt;" + TypeTitle);
            builder.Append(Out.Tab("</div>", "<br />"));
            #region 根据版本判断时间显示的形式 倒计时还是静止
            builder.Append(Out.Tab("<div>", ""));

            builder.Append("第" + model.klsfId + "期:<br/>");
            string s1 = new BCW.JS.somejs().newDaojishi("s1", model.EndTime.AddMinutes(0).AddSeconds(-10));
            builder.Append("还有" + s1 + "截止");

            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("类型：<b>" + TypeTitle + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("→您选了" + iZhu + "注←");
            builder.Append(strNum);
            builder.Append(Out.Tab("</div>", Out.LHr()));
            #endregion

            #region 提交表单

            #region 快捷下注
            try
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("快捷下注<br />∟");
                if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
                {
                    kuai(meid, 2, ptype, Num1, Num2);//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
                {
                    kuai(meid, 2, ptype, Num3, "");//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 9)
                {
                    kuai(meid, 2, ptype, Num4, Num5);//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 11)
                {
                    kuai(meid, 2, ptype, Num7, "");//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 12)
                {
                    kuai(meid, 2, ptype, Num6, "");//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 13)
                {
                    kuai(meid, 2, ptype, los, "");//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 14)
                {
                    kuai(meid, 2, ptype, soe, "");//用户，游戏2，下注类型,传值1.2
                }
                else if (ptype == 15)
                {
                    kuai(meid, 2, ptype, lh, "");//用户，游戏2，下注类型,传值1.2
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            catch { }
            #endregion

            #region 根据版本选择快乐币和酷币的获取和显示
            string strText = string.Empty;
            if (IsSWB == 0)
            {
                strText = "每注投注额(" + ub.GetSub("klsfSmallPay", xmlPath) + "-" + ub.GetSub("klsfBigPay", xmlPath) + "快乐币):/,,,,,,,,";
            }
            else
            {
                strText = "每注投注额(" + ub.GetSub("klsfSmallPay", xmlPath) + "-" + ub.GetSub("klsfBigPay", xmlPath) + "" + ub.Get("SiteBz") + "):/,,,,,,,,";
            }
            #endregion

            string strName = string.Empty;
            string strValu = string.Empty;
            string strType = string.Empty;

            #region 根据投注类型提交表单
            if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
            {
                strName = "Price,Num1,Num2,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                if (Price == 0) { strValu = "" + "" + "'" + Num1 + "'" + Num2 + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + Num1 + "'" + Num2 + "'" + ptype + "'pay'ok"; }

            }
            else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
            {
                strName = "Price,Num3,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + Num3 + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + Num3 + "'" + ptype + "'pay'ok"; }
            }
            else if (ptype == 9)
            {
                strName = "Price,Num4,Num5,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + Num4 + "'" + Num5 + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + Num4 + "'" + Num5 + "'" + ptype + "'pay'ok"; }
            }
            else if (ptype == 11)
            {
                strName = "Price,Num7,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + Num7 + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + Num7 + "'" + ptype + "'pay'ok"; }
            }
            else if (ptype == 12)
            {
                strName = "Price,Num6,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + Num6 + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + Num6 + "'" + ptype + "'pay'ok"; }
            }
            else if (ptype == 13)
            {
                strName = "Price,los,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + los + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + los + "'" + ptype + "'pay'ok"; }
            }
            else if (ptype == 14)
            {
                strName = "Price,soe,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + soe + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + soe + "'" + ptype + "'pay'ok"; }
            }
            else if (ptype == 15)
            {
                strName = "Price,lh,ptype,act,info";
                strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";

                if (Price == 0) { strValu = "" + "" + "'" + lh + "'" + ptype + "'pay'ok"; }
                else { strValu = "" + Price + "'" + lh + "'" + ptype + "'pay'ok"; }
            }
            #endregion
            string strEmpt = "true,false,false,false,false,false,false,false,false";
            #region 根据版本选择快乐币和酷币的获取和显示
            string strIdea = string.Empty;
            if (IsSWB == 0)
            {
                strIdea = "快乐币''''''''|/";
            }
            else
            {
                strIdea = "" + ub.Get("SiteBz") + "''''''''|/";
            }
            #endregion

            string strOthe = "确定投注,klsf.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion

            #region 页面底部和按键
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=" + ptype + "") + "\">返回重新选号</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("", Out.Hr()));
            GameFoot();
            #endregion
        }
        #endregion
    }

    #region 预购
    /// <summary>
    /// 预购页面
    /// </summary>
    private void OrderPage()
    {
        #region 初始化 获取数据
        int meid = new BCW.User.Users().GetUsId();  //获取用户的ID
        //获取不到就跳转回登陆页面
        if (meid == 0)
            Utils.Login();

        BCW.Model.klsflist model = new BCW.BLL.klsflist().GetklsflistLast();
        if (model.EndTime < DateTime.Now.AddSeconds(10))
        {
            Utils.Error("第" + model.klsfId + "期已截止下注,等待开奖...", Utils.getUrl("klsf.aspx"));
        }

        //顶部效果
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("〓追号投注〓");
        builder.Append(Out.Tab("</div>", "<br />"));

        string info = Utils.GetRequest("info", "post", 1, "", "");//判断状态
        int Sec = Utils.ParseInt(ub.GetSub("klsfSec", xmlPath));
        int iCount = Convert.ToInt32(Utils.GetRequest("iCount", "post", 1, @"^[1-9]\d*$", "")); //每一期购买的注数
        long Price = Utils.ParseInt64(Utils.GetRequest("Price", "post", 1, @"^[1-9]\d*$", ""));
        long Prices = Convert.ToInt64(Utils.GetRequest("Prices", "post", 1, @"^[1-9]\d*$", ""));
        string Notes = Utils.GetRequest("Notes", "post", 1, @"^[\d((,)\d)?]+(\|([\d((,)\d)?]+))*$", ""); //注号
        string ptype = Utils.GetRequest("ptype", "post", 1, @"^[1-9]$|^10$|^11$|^12$|^13$|^14$", ""); //投注类型
        int pType = Convert.ToInt32(ptype);
        string TypeTitle = OutType(pType);  //该投注类型的信息
        #endregion

        #region 最终确定和显示
        if (info == "ok2")
        {
            int stageCount = Convert.ToInt32(Utils.GetRequest("stageCount", "post", 2, @"^[1-9]\d*$", ""));
            int orderType = Convert.ToInt32(Utils.GetRequest("orderType", "post", 1, @"^[0-1]$", ""));
            int SklsfId = Convert.ToInt32(Utils.GetRequest("SklsfId", "post", 1, "", ""));
            int EklsfId = Convert.ToInt32(Utils.GetRequest("EklsfId", "post", 1, "", ""));
            long sumGold = Convert.ToInt64(Utils.GetRequest("SumGold", "post", 1, "", ""));
            int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
            long gold = 0;
            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                gold = new BCW.SWB.BLL().GeUserGold(meid, 32);
            }
            else
            {
                gold = new BCW.BLL.User().GetGold(meid);
            }
            #endregion
            if (gold < sumGold)
            {
                #region 根据版本选择快乐币和酷币的获取和显示
                if (IsSWB == 0)
                {
                    Utils.Error("您的快乐币不足", "");
                }
                else
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                }
                #endregion
            }

            string mename = new BCW.BLL.User().GetUsName(meid);

            new BCW.BLL.User().UpdateiGold(meid, mename, -Prices, "" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + model.ID + "]" + model.klsfId + "[/url]期" + OutType(pType) + ":" + Notes + ""); //酷币
            //    new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), Prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=mylistview&amp;id=" + id + "]" + model.klsfId + "[/url]期买" + OutType(pType) + ":" + Notes + "共" + Prices + ub.Get("SiteBz") + "-标识" + id + "");

            //动态投注后记录投注信息

            #region 根据版本选择快乐币和酷币的获取和显示
            string wText = string.Empty;
            if (IsSWB == 0)
            {
                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + TypeTitle + "》追号" + stageCount + "期,共" + sumGold + "快乐币";
            }
            else
            {
                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + TypeTitle + "》追号" + stageCount + "期,共**" + "" + ub.Get("SiteBz") + ""; //+ sumGold
            }
            #endregion

            // new BCW.BLL.Action().Add(1006, 0, 0, "", wText);
            string state = Utils.GetRequest("State", "post", 1, @"^[0-1]*$", "");

            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                Utils.Success("下注", "追号《" + TypeTitle + "》成功，花费了" + sumGold + "快乐币(共追" + stageCount + "期)<br /><a href=\"" + Utils.getUrl("klsf.aspx") + "\">&gt;继续下注</a>", Utils.getUrl("klsf.aspx"), "4");
            }
            else
            {
                Utils.Success("下注", "追号《" + TypeTitle + "》成功，花费了" + sumGold + "" + ub.Get("SiteBz") + "(共追" + stageCount + "期)<br /><a href=\"" + Utils.getUrl("klsf.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("klsf.aspx"), "4");
            }
            #endregion
        }
        #endregion

        #region 确认追号期数
        else if (info == "ok")
        {
            int stageCount = Convert.ToInt32(Utils.GetRequest("stageCount", "post", 2, @"^[1-9]\d*$", "请输入正确的数字"));
            int orderType = Convert.ToInt32(Utils.GetRequest("orderType", "post", 1, @"^[0-1]$", ""));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("现在是第" + model.klsfId + "期");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("还有" + DT.DateDiff(DateTime.Now.AddSeconds(Sec), model.EndTime) + "截止<br />");
            builder.Append("类型：<b>" + TypeTitle + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("位号：" + Notes + "<br />");
            builder.Append("注数：" + iCount + "注<br />");

            #region 根据版本选择快乐币和酷币的获取和显示
            int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
            if (IsSWB == 0)
            {
                builder.Append("每期：" + Prices + "快乐币<br />");
            }
            else
            {
                builder.Append("每期：" + Prices + "" + ub.Get("SiteBz") + "<br />");
            }
            #endregion

            #region 追号数据
            builder.Append("追号期数：<br />");
            int stage = model.klsfId;
            string stageDate = DateTime.Now.ToString("yyMMdd");
            int stageOut = Convert.ToInt32(stageDate + "98");
            string dayNext = DateTime.Now.AddDays(1).ToString("yyMMdd");
            int stageNext = Convert.ToInt32(dayNext + "01");
            for (int i = 0; i < stageCount; i++)
            {
                if (stage == stageOut)
                {
                    stage = stageNext;
                }
                stage++;
            }
            builder.Append("从" + model.klsfId + "期<br />");
            builder.Append("到" + stage + "期<br />");
            builder.Append("共" + stageCount + "期<br />");

            if (orderType == 0)
            {
                builder.Append("类型:中奖之后停止追号<br />");
            }
            else
            {
                builder.Append("类型:中奖之后继续追号<br />");
            }
            #endregion

            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                builder.Append("共需花费：" + Prices * stageCount + "快乐币<br />");
            }
            else
            {
                builder.Append("共需花费：" + Prices * stageCount + "" + ub.Get("SiteBz") + "<br />");
            }
            #endregion

            #region 根据版本选择快乐币和酷币的获取和显示
            if (IsSWB == 0)
            {
                long gold = new BCW.SWB.BLL().GeUserGold(meid, gid);
                builder.Append("您自带：" + Utils.ConvertGold(gold) + "快乐币");
            }
            else
            {
                long gold = new BCW.BLL.User().GetGold(meid);
                builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz"));
            }
            #endregion

            builder.Append(Out.Tab("</div>", Out.LHr()));

            builder.Append("<form id=\"form1\" method=\"post\" action=\"klsf.aspx\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"order\"/>");
            builder.Append("<input type=\"hidden\" name=\"iCount\" Value=\"" + iCount + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"SklsfId\" Value=\"" + model.klsfId + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"EklsfId\" Value=\"" + stage + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok2\"/>");
            builder.Append("<input type=\"hidden\" name=\"Notes\" Value=\"" + Notes + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"Price\" Value=\"" + Price + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"Prices\" Value=\"" + Prices + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"SumGold\" Value=\"" + Prices * stageCount + "\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确认投注\"/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
        }
        #endregion

        #region 输入追号期数
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("第" + model.klsfId + "期");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("还有" + DT.DateDiff(DateTime.Now.AddSeconds(Sec), model.EndTime) + "截止<br />");
            builder.Append("类型：<b>" + TypeTitle + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("位号：" + Notes + "<br />");
            builder.Append("注数：" + iCount + "注<br />");

            #region 根据版本选择快乐币和酷币的获取和显示
            int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
            if (IsSWB == 0)
            {
                builder.Append("每注：" + Price + "快乐币");
            }
            else
            {
                builder.Append("每注：" + Price + "" + ub.Get("SiteBz"));
            }
            #endregion

            builder.Append(Out.Tab("</div>", Out.LHr()));
            string strText = "追号期数:/,,,,,,,,";
            string strName = "stageCount,orderType,act,info,Notes,iCount,ptype,Price,Prices";
            string strType = "num,select,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "''order" + "'ok'" + Notes + "'" + iCount + "'" + pType + "'" + Price + "'" + Prices;
            string strEmpt = "false,0|中奖之后停止追号|1|中奖之后继续追号,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定|reset,klsf.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br/>");
        }
        #endregion

        #region 页面底部
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    #endregion

    /// <summary>
    /// 领取试玩币
    /// </summary>
    private void GetMoney()
    {
        int time = int.Parse(ub.GetSub("GetTime", xmlPath));
        long max = Int64.Parse(ub.GetSub("GetMoneyMax", xmlPath));
        long oneget = Int64.Parse(ub.GetSub("GetMoney", xmlPath));
        Master.Title = "快乐币领取";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        #region 根据版本判断是否试玩
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));

        if (IsSWB == 0)
        {
            BCW.SWB.Model swb = new BCW.SWB.Model();
            if (new BCW.SWB.BLL().ExistsUserID(meid, gid) == true)
                swb = new BCW.SWB.BLL().GetModelForUserId(meid, gid);
            else
            {
                swb.UserID = meid;
                swb.GameID = gid;
                swb.Money = 0;
                swb.Permission = 0;
                swb.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(swb);
            }
            if (swb.Permission == 0)
                Utils.Success("很抱歉您没有获得游戏的测试权限！", Utils.getUrl("klsf.aspx")); //测试
            else
            {
                if (swb.UpdateTime.AddMinutes(time) >= DateTime.Now)
                    Utils.Error("请耐心等待倒计时完成后领取！", Utils.getUrl("klsf.aspx"));
                if (swb.Money >= max)
                    Utils.Error("您的快乐币已经很多了！请用至" + max + "快乐币以下再领取！", Utils.getUrl("klsf.aspx"));
                new BCW.SWB.BLL().UpdateMoney(meid, oneget, gid);
                new BCW.SWB.BLL().UpdateTime(meid, DateTime.Now, gid);
                Utils.Success("领钱", "恭喜你成功领取了" + oneget + "快乐币。", Utils.getUrl("klsf.aspx"), "1");
            }
        }
        #endregion
    }

    /// <summary>
    /// 投注方式
    /// </summary>
    /// <param name="Types">投注类型的编号</param>
    /// <returns>对应的类型</returns>
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "任选五胆拖投注";
        else if (Types == 2)
            pText = "任选五普通复式";
        else if (Types == 3)
            pText = "任选四胆拖投注";
        else if (Types == 4)
            pText = "任选四普通复式";
        else if (Types == 5)
            pText = "任选三胆拖投注";
        else if (Types == 6)
            pText = "任选三普通复式";
        else if (Types == 7)
            pText = "任选二胆拖投注";
        else if (Types == 8)
            pText = "任选二普通复式";
        else if (Types == 9)
            pText = "连二直选";
        else if (Types == 10)
            pText = "连二组选";
        else if (Types == 11)
            pText = "前一红投";
        else if (Types == 12)
            pText = "前一数投";
        else if (Types == 13)
            pText = "大小";
        else if (Types == 14)
            pText = "单双";
        else if (Types == 15)
            pText = "龙虎";
        return pText;
    }

    /// <summary>
    /// 选择不同的投注方式时显示的玩法提示
    /// </summary>
    /// <param name="Types">投注类型的编号</param>
    /// <returns>对应的玩法介绍</returns>
    private string OutRule(int Types)
    {
        #region 从xml中读取倍数
        double klsfOdds1 = Utils.ParseDouble(ub.GetSub("klsfOdds1", xmlPath));
        double klsfOdds2 = Utils.ParseDouble(ub.GetSub("klsfOdds2", xmlPath));
        double klsfOdds3 = Utils.ParseDouble(ub.GetSub("klsfOdds3", xmlPath));
        double klsfOdds4 = Utils.ParseDouble(ub.GetSub("klsfOdds4", xmlPath));
        double klsfOdds5 = Utils.ParseDouble(ub.GetSub("klsfOdds5", xmlPath));
        double klsfOdds6 = Utils.ParseDouble(ub.GetSub("klsfOdds6", xmlPath));
        double klsfOdds7 = Utils.ParseDouble(ub.GetSub("klsfOdds7", xmlPath));
        double klsfOdds8 = Utils.ParseDouble(ub.GetSub("klsfOdds8", xmlPath));
        double klsfOdds9 = Utils.ParseDouble(ub.GetSub("klsfOdds9", xmlPath));
        double klsfOdds10 = Utils.ParseDouble(ub.GetSub("klsfOdds10", xmlPath));
        double klsfOdds11 = Utils.ParseDouble(ub.GetSub("klsfOdds11", xmlPath));
        double klsfOdds12 = Utils.ParseDouble(ub.GetSub("klsfOdds12", xmlPath));
        double klsfOdds18 = Utils.ParseDouble(ub.GetSub("klsfOdds18", xmlPath));//龙18
        double klsfOdds19 = Utils.ParseDouble(ub.GetSub("klsfOdds19", xmlPath));//虎81
        double klsfOdds20 = Utils.ParseDouble(ub.GetSub("klsfOdds20", xmlPath));//龙27
        double klsfOdds21 = Utils.ParseDouble(ub.GetSub("klsfOdds21", xmlPath));//虎72
        double klsfOdds22 = Utils.ParseDouble(ub.GetSub("klsfOdds22", xmlPath));//龙36
        double klsfOdds23 = Utils.ParseDouble(ub.GetSub("klsfOdds23", xmlPath));//虎63
        double klsfOdds24 = Utils.ParseDouble(ub.GetSub("klsfOdds24", xmlPath));//龙45
        double klsfOdds25 = Utils.ParseDouble(ub.GetSub("klsfOdds25", xmlPath));//虎54
        #endregion

        #region 每种投注玩法的规则介绍
        string pText = string.Empty;
        if (Types == 1)
            pText = "胆码只能选择1到4个，胆码全部正确且胆码数加上正确的拖码数大于等于5个则中奖，赔率" + klsfOdds1 + "<br />#===从20个号码中任选5个===";//+ ub.GetSub("klsfOdds1", xmlPath) + "倍<br />===选号区 每位至少选择1个号码===";
        else if (Types == 2)
            pText = "至少选择5个号码，投注号码与开奖号中任意5个数字相同则中奖，赔率" + klsfOdds2 + "<br />#===从20个号码中任选5个===";//+ ub.GetSub("klsfOdds2", xmlPath) + "倍；<br />如所选号码和开奖号码，前三位或后三位一一对应，即中奖" + ub.GetSub("klsfOdds3", xmlPath) + "倍；如前二位或者后二位一一对应，即中奖" + ub.GetSub("klsfOdds4", xmlPath) + "倍<br />===选号区 每位至少选择1个号码===";
        else if (Types == 3)
            pText = "胆码只能选择1到3个，胆码全部正确时胆码数加上正确的拖码数大于等于4个即中奖，赔率" + klsfOdds3 + "<br />#===从20个号码中任选4个===";//+ ub.GetSub("klsfOdds2", xmlPath) + "倍；<br />如所选号码和开奖号码，前三位或后三位一一对应，即中奖" + ub.GetSub("klsfOdds3", xmlPath) + "倍；如前二位或者后二位一一对应，即中奖" + ub.GetSub("klsfOdds4", xmlPath) + "倍<br />===选号区 每位至少选择1个号码===";
        else if (Types == 4)
            pText = "至少选择4个号码，投注号码与开奖号中任意4个数字相同则中奖，赔率" + klsfOdds4 + "<br />#===从20个号码中任选4个==="; //+ ub.GetSub("klsfOdds2", xmlPath) + "倍；<br />如所选号码和开奖号码，前三位或后三位一一对应，即中奖" + ub.GetSub("klsfOdds3", xmlPath) + "倍；如前二位或者后二位一一对应，即中奖" + ub.GetSub("klsfOdds4", xmlPath) + "倍<br />===选号区 每位至少选择1个号码===";
        else if (Types == 5)
            pText = "胆码只能选择1到2个，胆码全部正确时胆码数加上正确的拖码数大于等于3个即中奖，赔率" + klsfOdds5 + "<br />#===从20个号码中任选3个==="; //+ ub.GetSub("klsfOdds3", xmlPath) + "倍；";// + ub.GetSub("klsfOdds3", xmlPath) + "倍；<br />所选号码与开奖号码的后四位中的连续前三位或后三位按位相符，即中奖" + ub.GetSub("klsfOdds6", xmlPath) + "倍<br />===选号区 每位至少选择1个号码===";
        else if (Types == 6)
            pText = "至少选择3个号码，投注号码与开奖号中任意3个数字相同则中奖，赔率" + klsfOdds6 + "<br />#===从20个号码中任选3个==="; //+ ub.GetSub("klsfOdds3", xmlPath) + "倍；<br />所选号码与开奖号码的后四位中的连续前三位或后三位按位相符，即中奖" + ub.GetSub("klsfOdds6", xmlPath) + "倍<br />===选号区 每位至少选择1个号码===";
        else if (Types == 7)
            pText = "胆码只能选择1个，当胆码全部正确时胆码数加上正确的拖码数大于等于2个即中奖，赔率" + klsfOdds7 + "<br />#===从20个号码中任选2个===";//" + ub.GetSub("klsfOdds4", xmlPath) + "倍
        else if (Types == 8)
            pText = "至少选择2个号码，投注号码与开奖号中任意2个数字相同则中奖，赔率" + klsfOdds8 + "<br />#===从20个号码中任选2个===";//" + ub.GetSub("klsfOdds4", xmlPath) + "倍
        else if (Types == 9)
            pText = "每位至少选择1个号码投注，投注号码与开奖号中连续2位顺序及数字都相同则中奖，赔率" + klsfOdds9 + "<br />#===从20个号码中任选连续两位===";//" + ub.GetSub("klsfOdds5", xmlPath) + "倍
        else if (Types == 10)
            pText = "至少选择2个号码投注，投注号码与开奖号中连续2位数字相同(顺序不限)则中奖，赔率" + klsfOdds10 + "<br />#===从20个号码中任选2个===";//" + ub.GetSub("klsfOdds6", xmlPath) + "倍
        else if (Types == 11)
            pText = "19，20为红号，从这两个号码任选一个投注，开奖号码第一位是红号（19或20）即中奖，赔率" + klsfOdds11 + "<br />#===19，20为红号，投注这两个号码===";//" + ub.GetSub("klsfOdds7", xmlPath) + "
        else if (Types == 12)
            pText = "从01-18中任意选择1个号码投注，投注号码与开奖号第一位相同则中奖，赔率" + klsfOdds12 + "<br />#===从01至18中任选1个===";//+ ub.GetSub("klsfOdds8", xmlPath)
        else if (Types == 13)
            pText = "当期开奖号码相加结果的尾数大小，大是5、6、7、8、9，小是0、1、2、3、4<br />#===买大或者买小===";
        else if (Types == 14)
            pText = "当期开奖结果相加结果的尾数单双，单是1、3、5、7、9，双是0、2、4、6、8<br />#===押单或者押双===";
        else if (Types == 15)
            pText = "取当期开奖结果的8个开奖号<br />1与8位：第1开奖号码大于第8开奖号码为龙（赔率" + klsfOdds18 + "），小于为虎（赔率" + klsfOdds19 + "）；<br />2与7位：第2开奖号码大于第7开奖号码为龙（赔率" + klsfOdds20 + "），小于为虎（赔率" + klsfOdds21 + "）；<br />3与6位：第3开奖号码大于第6开奖号码为龙（赔率" + klsfOdds22 + "），小于为虎（赔率" + klsfOdds23 + "）；<br />4与5位：第4开奖号码大于第5开奖号码为龙（赔率" + klsfOdds24 + "），小于为虎（赔率" + klsfOdds25 + "）；<br />#===押龙或者押虎===";
        return pText;
        #endregion
    }

    /// <summary>
    /// 判断是否在开放时间内
    /// </summary>
    /// <returns>判断结果</returns>
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("klsfOnTime", xmlPath);
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))  //从xml中提取出的开放时间
            {
                string[] temp = OnTime.Split("-".ToCharArray());   //把开放时间的开放时间和结束时间分别存放到两个字符串里面
                DateTime dt1 = Convert.ToDateTime(temp[0]);       //开放时间存放到dt1
                DateTime dt2 = Convert.ToDateTime(temp[1]);       //结束时间存放到dt2
                if (DateTime.Now < dt1 && DateTime.Now > dt2)      //判断现在是否在开放时间内
                {
                    IsOpen = false;
                }
                else
                {
                    IsOpen = true;
                }
            }
        }
        return IsOpen;
    }

    #region 更新期数
    public string UpdateState()
    {
        //计算出期号截止时间，如果抓取的开售时间有变请更改代码！！！
        DateTime dt1 = Convert.ToDateTime("10:00:00");//10点钟之后开始第15期
        DateTime dt2 = Convert.ToDateTime("02:00:00");//2.12到10点之间固定为14期的时间
        DateTime dt22 = Convert.ToDateTime("23:50:00");//10点到23.50为14到97期
        DateTime dat = Convert.ToDateTime("00:00:00");//00点判断新的开始，23.50到00点为停售时间，不做操作
        DateTime dt23 = Convert.ToDateTime("23:59:59");//10点到23.50为14到97期
        if (DateTime.Now >= dat && DateTime.Now < dt23)
        {
            BCW.Model.klsflist model = new BCW.Model.klsflist();
            string state = string.Empty;
            if (DateTime.Now > dt22 && DateTime.Now < dt23)
            {
                state = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6) + "01";
                model.EndTime = Convert.ToDateTime("" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00" + "");
                model.klsfId = Convert.ToInt32(state);
                model.Result = "";
            }
            else
            {
                string dt3 = string.Empty;
                if (DateTime.Now >= dat && DateTime.Now <= dt2)
                {
                    dt3 = DateTime.Now.AddMinutes(5).Subtract(dat).Duration().TotalMinutes.ToString();
                }
                if (DateTime.Now > dt2 && DateTime.Now < dt1)
                {

                    dt3 = "145.00";
                }
                if (DateTime.Now >= dt1 && DateTime.Now <= dt22)
                {
                    dt3 = DateTime.Now.AddMinutes(145).Subtract(dt1).Duration().TotalMinutes.ToString();
                }

                decimal dt4 = Convert.ToDecimal(dt3);
                int dt5 = Convert.ToInt32(dt4 / 10);
                string dt6 = dt5.ToString();
                if (dt6.Length == 1)
                {
                    dt6 = "0" + dt6;
                }
                if (dt6.Length == 2)
                {
                    dt6 = "" + dt6;
                }
                state = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6) + dt6;


                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 2)
                {
                    model.klsfId = Convert.ToInt32(state) + 1;
                }
                else
                {
                    model.klsfId = Convert.ToInt32(state);
                }

                model.Result = "";
                string datee = string.Empty;
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 2)
                {
                    datee = DateTime.ParseExact((("20" + state.Substring(0, 6)) + " 00:00:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DateTime.Now.Hour >= 10)
                {
                    datee = DateTime.ParseExact((("20" + state.Substring(0, 6)) + " 00:00:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                }

                //根据期号算时间,
                if (Convert.ToInt32(dt6) < 10 && Convert.ToInt32(dt6) >= 01)
                {
                    for (int i = 0; i < Convert.ToInt32(dt6); i++)
                    {
                        model.EndTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 1)))).AddSeconds(20);

                    }
                }
                else if (Convert.ToInt32(dt6) >= 10 && Convert.ToInt32(dt6) <= 13)
                {
                    model.EndTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2)))).AddSeconds(20);
                }
                else if (Convert.ToInt32(dt6) == 14)
                {
                    model.EndTime = Convert.ToDateTime(" " + DateTime.Now.ToString("yyyy-MM-dd") + " 10:00:20");
                }
                else if (Convert.ToInt32(dt6) > 14 && Convert.ToInt32(dt6) <= 97)
                {
                    for (int i = 0; i < Convert.ToInt32(dt6) - 14; i++)
                    {
                        model.EndTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2)))).AddHours(8).AddMinutes(-20).AddSeconds(20);

                    }
                }
            }

            model.State = 0;
            bool s;
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 2)
            {
                s = new BCW.BLL.klsflist().ExistsklsfId(Convert.ToInt32(state) + 1);
            }
            else
            {
                s = new BCW.BLL.klsflist().ExistsklsfId(Convert.ToInt32(state));
            }

            switch (s)
            {
                case false:
                    new BCW.BLL.klsflist().Add(model);

                    break;
                case true:
                    break;
            }
            return state;

        }
        return "0";
    }
    #endregion

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

    #region 计算组合的数量
    static long jc(int N)//阶乘
    {
        long t = 1;

        for (int i = 1; i <= N; i++)
        {
            t *= i;
        }
        return t;
    }
    static long P(int N, int R)//组合的计算公式
    {
        long t = jc(N) / jc(N - R);

        return t;
    }
    static int C(int N, int R)//组合
    {
        long i = P(N, R) / jc(R);
        int t = Convert.ToInt32(i);
        return t;
    }
    #endregion

    #region 游戏顶部
    private void GameTop()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 游戏底部
    private void GameFoot()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("klsfName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 计算开奖和值
    public int GetHe(string Result)
    {
        int He = 0;
        string[] num = Result.Split(',');
        for (int i = 0; i < num.Length; i++)
        {
            try
            {
                He += Convert.ToInt32(num[i]);
            }
            catch
            {
            }
        }
        return He;
    }
    #endregion

    #region 计算开奖龙虎
    public string GetLH(int i, string Result)
    {
        string LH = string.Empty;
        string kai1 = string.Empty;
        string kai2 = string.Empty;
        string kai3 = string.Empty;
        string kai4 = string.Empty;
        string[] num = Result.Split(',');
        try
        {
            if (Convert.ToInt32(num[0]) > Convert.ToInt32(num[7]))
            {
                kai1 = "龙";
            }
            else//   if (Convert.ToInt32(num[0]) < Convert.ToInt32(num[7]))
            {
                kai1 = "虎";
            }
            if (Convert.ToInt32(num[1]) > Convert.ToInt32(num[6]))
            {
                kai2 = "龙";
            }
            else// if (Convert.ToInt32(num[6]) < Convert.ToInt32(num[1]))
            {
                kai2 = "虎";
            }
            if (Convert.ToInt32(num[2]) > Convert.ToInt32(num[5]))
            {
                kai3 = "龙";
            }
            else//  if (Convert.ToInt32(num[5]) < Convert.ToInt32(num[2]))
            {
                kai3 = "虎";
            }
            if (Convert.ToInt32(num[3]) > Convert.ToInt32(num[4]))
            {
                kai4 = "龙";
            }
            else//  if (Convert.ToInt32(num[4]) < Convert.ToInt32(num[3]))
            {
                kai4 = "虎";
            }
            // LH = kai1 + ", " + kai2 + ", " + kai3 + ", " + kai4;
            if (i == 1) { LH = kai1; }
            if (i == 2) { LH = kai2; }
            if (i == 3) { LH = kai3; }
            if (i == 4) { LH = kai4; }
        }
        catch
        {
            return "";
        }

        return LH;
    }
    #endregion

    #region 计算开奖龙虎分析
    public int GetLHF(string Result, int i)
    {
        int LH = 0;
        string kai1 = string.Empty;
        string kai2 = string.Empty;
        string kai3 = string.Empty;
        string kai4 = string.Empty;
        string[] num = Result.Split(',');
        try
        {
            if (i == 1)
            {
                if (Convert.ToInt32(num[0]) > Convert.ToInt32(num[7]))
                {
                    LH = 1;
                }
                else//   if (Convert.ToInt32(num[0]) < Convert.ToInt32(num[7]))
                {
                    LH = 2;
                }
            }
            if (i == 2)
            {
                if (Convert.ToInt32(num[1]) > Convert.ToInt32(num[6]))
                {
                    LH = 1;
                }
                else// if (Convert.ToInt32(num[6]) < Convert.ToInt32(num[1]))
                {
                    LH = 2;
                }
            }
            if (i == 3)
            {
                if (Convert.ToInt32(num[2]) > Convert.ToInt32(num[5]))
                {
                    LH = 1;
                }
                else//  if (Convert.ToInt32(num[5]) < Convert.ToInt32(num[2]))
                {
                    LH = 2;
                }
            }
            if (i == 4)
            {
                if (Convert.ToInt32(num[3]) > Convert.ToInt32(num[4]))
                {
                    LH = 1;
                }
                else//  if (Convert.ToInt32(num[4]) < Convert.ToInt32(num[3]))
                {
                    LH = 2;
                }
            }

        }
        catch
        {
            return 0;
        }

        return LH;
    }
    #endregion

    /// <summary>
    /// 快捷下注转换成X万
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
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

    #region 快捷下注1
    private void kuai(int uid, int type, int ptype, string Num1, string Num2)//用户，游戏编号，下注类型|特殊传值1.2
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (new BCW.QuickBet.BLL.QuickBet().ExistsUsID(meid))
        {

        }
        else//给会员自动添加默认的快捷下注
        {
            BCW.QuickBet.Model.QuickBet model = new BCW.QuickBet.Model.QuickBet();
            model.UsID = meid;
            model.Game = new BCW.QuickBet.BLL.QuickBet().GetGame();//十个编号的游戏|1:时时彩|2快乐十分|3:快乐扑克3|4:6场半|5:胜负彩
            model.Bet = new BCW.QuickBet.BLL.QuickBet().GetBety();
            new BCW.QuickBet.BLL.QuickBet().Add(model);
        }

        #region 快捷下注
        try
        {
            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');
            for (int i = 0; i < game1.Length; i++)
            {
            }

            int j = 0;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)//取出对应的游戏
                {
                    j = i;
                }
            }
            string gold = string.Empty;
            string st = string.Empty;
            string str = string.Empty;
            string[] kuai = bet1[j].Split('|');//取出对应的快捷下注
            for (int i = 0; i < kuai.Length; i++)
            {
                if (kuai[i] != "0")
                {
                    //if (Convert.ToInt64(kuai[i]) >= 10000)
                    //{
                    //    if (Convert.ToInt64(kuai[i]) % 10000 == 0)
                    //    {
                    //        gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                    //    }
                    //    else
                    //    {
                    //      st = (Convert.ToInt64(kuai[i]) / 10000) + ".X万";
                    //        gold = st;
                    //    }
                    //}
                    //else
                    //{
                    //    gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                    //}

                    gold = ChangeToWan(kuai[i]);

                    if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num1=" + Num1 + "&amp;Num2=" + Num2 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//    
                    }
                    else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8 || ptype == 10)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num3=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//  
                    }
                    else if (ptype == 9)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num4=" + Num1 + "&amp;Num5=" + Num2 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//    
                    }
                    else if (ptype == 11)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num7=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//  
                    }
                    else if (ptype == 12)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;Num6=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//  
                    }
                    else if (ptype == 13)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;los=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//  
                    }
                    else if (ptype == 14)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;soe=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//  
                    }
                    else if (ptype == 15)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=pay&amp;ptype=" + ptype + "&amp;lh=" + Num1 + "&amp;Price=" + Convert.ToInt64(kuai[i]) + "") + "\">" + gold + "</a>" + "|");//  
                    }
                }
            }

            builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");

        }
        catch { }
        #endregion
    }
    #endregion

}
