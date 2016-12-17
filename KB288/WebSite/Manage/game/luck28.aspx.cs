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

public partial class Manage_game_luck28 : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "back":
                BackPage();
                break;
            case "top":
                TopPage();
                break;
            case "topgive":
                topgive();
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "openprize":
                OpenPrize();
                break;
            case "stat":
                StatPage();//赢利分析
                break;
            case "view":
                ViewPage();
                break;
            case "ottcents":
                OttCents();
                break;
            case "ottodds":
                OttOdds();
                break;
            case "halfcents":
                HalfCents();
                break;
            case "MMOdds":
                MaxMinOdds();
                break;
            case "lastcents":
                LastCents();
                break;
            case "buyamount":
                BuyAmount();
                break;
            case "buyodds":
                BuyOdds();
                break;
            case "sethalfminodds":
                SetHalfMinOdds();
                break;
            case "sethalfodds":
                SetHalfOdds();
                break;
            case "bjkl":
                BjklList();
                break;
            case "endodds":
                EndOdds();
                break;
            case "baseset":
                BaseSet();
                break;
            case "edit":
                EditPage();
                break;
            case "byhand":  //手动添加期数
                ByHand();
                break;
            case "robotbuy":
                RobotBuy();
                break;
            case "floatodds":
                FloatOdds();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "ontimeodds":
                ontimeodds();
                break;
            case "del":
                DelPage();
                break;
            case "reset":
                ResetPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    //查看实时赔率
    private void ontimeodds()
    {
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;设置浮动最大最小赔率");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            double FloatBig = Convert.ToDouble(ub.GetSub("FloatBig", xmlPath));//大实时赔率
            double FloatSmall = Convert.ToDouble(ub.GetSub("FloatSmall", xmlPath));//小实时赔率

            double FloatSingle = Convert.ToDouble(ub.GetSub("FloatSingle", xmlPath));//单实时赔率
            double FloatDouble = Convert.ToDouble(ub.GetSub("FloatDouble", xmlPath));//双实时赔率
            strText = "大实时赔率:/,小实时赔率:/,单实时赔率:/,双实时赔率:/,,";
            strName = "FloatBig,FloatSmall,FloatSingle,FloatDouble,info,act";
            strType = "num,num,num,num,hidden,hidden";
            strValu = FloatBig + "'" + FloatSmall + "'" + FloatSingle + "'" + FloatDouble + "'ok'ontimeodds";
            strEmpt = "false,false,false,false,false,,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string FloatBig = (Utils.GetRequest("FloatBig", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置大实时赔率填写出错"));
            string FloatSmall = (Utils.GetRequest("FloatSmall", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置小实时赔率填写出错"));
            string FloatSingle = (Utils.GetRequest("FloatSingle", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置单实时赔率填写出错"));
            string FloatDouble = (Utils.GetRequest("FloatDouble", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置双实时赔率填写出错"));
            xml.dss["FloatBig"] = FloatBig;
            xml.dss["FloatSmall"] = FloatSmall;
            xml.dss["FloatSingle"] = FloatSingle;
            xml.dss["FloatDouble"] = FloatDouble;
            //xml.dss["Luck28All"] = Luck28All1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx?act=baseset"), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void ReloadPage()
    {
        Master.Title = "幸运28管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("幸运28");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "";
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
                //  builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                {
                    builder.Append("第" + n.Bjkl8Qihao + "期开出:<a href=\"" + Utils.getUrl("luck28.aspx?act=view&amp;id=" + n.ID + "") + "\">未开</a>");
                    builder.Append("&nbsp;<a href=\"" + Utils.getUrl("luck28.aspx?act=openprize&amp;id=" + n.ID + "") + "\">开奖</a>");
                }
                else
                    builder.Append("第" + n.Bjkl8Qihao + "期开出:<a href=\"" + Utils.getUrl("luck28.aspx?act=view&amp;id=" + n.ID + "") + "\">" + n.SumNum + "=" + n.PostNum + "</a>");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=byhand") + "\">添加期数|</a>");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top") + "\">排行榜单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=back") + "\">返赢返负|</a>");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=stat") + "\">盈利分析</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=reset") + "\">重置游戏|</a>");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/luck28set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=bjkl") + "\">北京快乐8期数</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BjklList()
    {
        Master.Title = "北京快乐8期数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;北京快乐8期数");
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
                    builder.Append("快乐8第<a href=\"" + Utils.getUrl("luck28.aspx?act=view&amp;id=" + n.ID) + "\">" + n.Bjkl8Qihao + "</a>期号码:" + n.Bjkl8Nums + "=二八开奖:" + n.SumNum + "=" + n.PostNum + "");
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
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("一·北京快乐8每期开奖共开出20个数字,幸运28将这20个开奖号码按照由小到大的顺序依次排列。<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其1-6位开奖号码相加,和值的末位数作为幸运28开奖第一个数值<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其7-12位开奖号码相加,和值的末位数作为幸运28开奖第二个数值<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;取其13-18位开奖号码相加,和值的末位数作为幸运28开奖第三个数值<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;三个数值相加即为第三方28最终的开奖结果<br/>");
        builder.Append("二·如下图所示。<br/>");
        builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"/files/face/bjkl8.png\" width=\"500\" height=\"250\"  alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BaseSet()
    {
        Master.Title = "基本设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/luck28set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>&gt;基本设置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=buyamount") + "\">号码限额</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=halfcents") + "\">半数限额</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=lastcents") + "\">尾数限额</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=ottcents") + "\">分段限额</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=endodds") + "\">尾数赔率</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=sethalfodds") + "\">半数赔率</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=buyodds") + "\">号码赔率</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=ottodds") + "\">分段赔率</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=floatodds") + "\">设置浮动大小 </a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=MMOdds") + "\">最大最小的赔率</a><br />");
        //  builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=ontimeodds") + "\">实时赔率</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/luck28set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ByHand()
    {
        Master.Title = "手动添加期数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;手动添加期数");
        builder.Append(Out.Tab("</div>", ""));
        int maxid = new BCW.BLL.Game.Lucklist().GetCount();//统计数据
        if (maxid > 0)
        {
            BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetLucklistState();  //得到最新一期的状态

            string info = Utils.GetRequest("info", "all", 1, "", "");
            //if (luck.State == 0)
            //{
            //    Utils.Error("最新一期未开奖，不能添加数据", "");
            //}      
            if (info == "")
            {
                strText = "设置添加的期号:/,,";
                strName = "qihao,info,act";
                strType = "num,hidden,hidden";
                strValu = "'ok'byhand";
                strEmpt = "false,false,false";
                strIdea = "/";
                strOthe = "确定编辑,luck28.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                //DateTime BeginTime1 = Utils.ParseTime(Utils.GetRequest("begin", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
                //DateTime EndTime1 = Utils.ParseTime(Utils.GetRequest("end", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

                int qihao = Utils.ParseInt(Utils.GetRequest("qihao", "post", 2, @"^\d+$", "设置期号错误"));


                if (new BCW.BLL.Game.Lucklist().ExistsBJQH(qihao))  //如果存在该期号，不能添加
                {
                    Utils.Error("添加期号错误！添加的期号已存在！", "");
                }
                if (qihao <= luck.Bjkl8Qihao)  //如果添加期号 <  最新一期的期号
                {
                    Utils.Error("添加期号错误，添加的期号不能少于最新一期的期号", "");
                }
                string OnTime = ub.GetSub("Luck28OnTime", xmlPath);//09:00-23:56
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);//09:00
                DateTime dt2 = Convert.ToDateTime(temp[1]);//23:56
                DateTime lasttime = Convert.ToDateTime("23:55");
                string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();
                string[] time = dt3.Split('.');
                string timeone = time[0];
                BCW.Model.Game.Lucklist addluck = new BCW.Model.Game.Lucklist();
                addluck.SumNum = 0;
                addluck.PostNum = "";
                addluck.LuckCent = 0;
                addluck.Pool = 0;
                addluck.BeforePool = 1;
                addluck.State = 0;
                addluck.Bjkl8Qihao = luck.Bjkl8Qihao + 1;

                string pd = DateTime.Now.ToString();
                DateTime add = Convert.ToDateTime(pd);

                string[] a = luck.EndTime.ToString().Split(' ');
                builder.Append("数据库截取的" + a[0] + "<br/>");

                DateTime add1 = Convert.ToDateTime(a[0] + " 23:55:00");
                builder.Append("add1" + add1 + "<br/>");

                builder.Append("luck.EndTime >= add1:" + (luck.EndTime >= add1) + "<br/>");
                if (luck.EndTime >= add1)//如果最新一期是当天时间的最后一期添加为第二天的
                {
                    //  string myt = DateTime.Now.AddDays(1).ToString();
                    string panduan = (luck.Bjkl8Qihao + 1).ToString();
                    addluck.panduan = panduan;
                    addluck.BeginTime = dt1.AddDays(1);
                    addluck.EndTime = dt1.AddDays(1).AddMinutes(5);
                    //ub xml = new ub();
                    //Application.Remove(xmlPath);//清缓存
                    //xml.ReloadSub(xmlPath); //加载配置
                    //xml.dss["Luck28StartQH"] = panduan;  //修改下一天开始期号
                    //System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }

                else   //不是当天最后一期
                {
                    string panduan = luck.panduan;
                    addluck.panduan = (panduan);
                    addluck.BeginTime = luck.EndTime;

                    string[] day = luck.EndTime.ToString().Split(' ');//天；
                    string[] hm = day[1].Split(':');
                    string mt = day[0] + " " + hm[0] + ":" + hm[1] + ":00";
                    DateTime mlast = Convert.ToDateTime(mt);
                    luck.EndTime = mlast;
                    //string ms = luck.EndTime.ToString("yyyy-MM-dd hh:mm:00");
                    //luck.EndTime = Convert.ToDateTime(ms);//去除秒数
                    addluck.EndTime = luck.EndTime.AddMinutes(5);

                }
                //  builder.Append("  addluck.EndTime" + addluck.EndTime + "<br/>");
                new BCW.BLL.Game.Lucklist().Add(addluck);//增加最新一期

                Utils.Success("添加期数成功", "添加期数成功", Utils.getUrl("luck28.aspx"), "1");

            }
        }
        else
        {
            Utils.Error("暂无数据不能添加期数！", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BackPage()
    {
        Master.Title = "幸运28返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;返赢返负操作");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("返赢返负操作");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "date,date,num,num,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,luck28.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
        strName = "sTime,oTime,iTar,iPrice,act";
        strType = "date,date,num,num,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave2";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,luck28.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackSavePage(string act)
    {
        Master.Title = "幸运28返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;返赢返负操作");
        builder.Append(Out.Tab("</div>", "<br />"));


        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info1 = Utils.GetRequest("info1", "all", 1, "", "");
        if (act == "backsave")
        {
            if (info == "")
            {

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("返赢日期：<b>" + sTime.ToString().Replace('/', '-') + "</b>至<b>" + oTime.ToString().Replace('/', '-') + "</b><br/>");
                builder.Append("返赢千分比：<b>" + iTar + "</b><br/>");
                builder.Append("至少赢多少才返：<b>" + iPrice + "</b>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,,";
                string strName = "sTime,oTime,iTar,iPrice,act,info";
                string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "" + sTime.ToString().Replace('/', '-') + "'" + oTime.ToString().Replace('/', '-') + "'" + iTar + "'" + iPrice + "'" + "backsave'ok";
                string strEmpt = "false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "马上返赢,luck28.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
            else
            {
                DataSet ds = new BCW.BLL.Game.Luckpay().GetList("UsID,sum(WinCent-BuyCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<'" + oTime + "' group by UsID");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    if (Cents > 0 && Cents >= iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                        new BCW.BLL.User().UpdateiGold(usid, cent, "二八返赢");
                        //发内线
                        string strLog = "根据你上期幸运二八排行榜上的赢利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/luck28.aspx]进入幸运二八[/url]";

                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                    }
                }

                Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("luck28.aspx"), "1");
            }
        }
        else
        {
            if (info1 == "")
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("返负日期：<b>" + sTime.ToString().Replace('/', '-') + "</b>至<b>" + oTime.ToString().Replace('/', '-') + "</b><br/>");
                builder.Append("返负千分比：<b>" + iTar + "</b><br/>");
                builder.Append("至少负多少才返：<b>" + iPrice + "</b>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,,";
                string strName = "sTime,oTime,iTar,iPrice,act,info1";
                string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "" + sTime.ToString().Replace('/', '-') + "'" + oTime.ToString().Replace('/', '-') + "'" + iTar + "'" + iPrice + "'" + "backsave2'ok";
                string strEmpt = "false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "马上返负,luck28.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
            else
            {
                DataSet ds = new BCW.BLL.Game.Luckpay().GetList("UsID,sum(WinCent-BuyCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<'" + oTime + "' group by UsID");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    if (Cents < 0 && Cents < (-iPrice))
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        new BCW.BLL.User().UpdateiGold(usid, cent, "二八返负");
                        //发内线
                        string strLog = "根据你上期幸运二八排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/luck28.aspx]进入幸运二八[/url]";
                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                    }
                }

                Utils.Success("返负操作", "返负操作成功", Utils.getUrl("luck28.aspx"), "1");
            }
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private string GetLast(string buynums)
    {

        string[] aa = buynums.Split(',');
        string result = "";
        for (int i = 0; i < aa.Length; i++)
        {
            if (i == 2)
            {
                //  Utils.Error("buynums:"+ buynums, "");
            }
            int a = Utils.ParseInt(aa[i]) % 10;  //得到个位数
            if (!(result).Contains(a + "(")) //得出尾数
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
    public string GOdds(string Eng)
    {
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
    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Lucklist model = new BCW.BLL.Game.Lucklist().GetLucklist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + new BCW.BLL.Game.Lucklist().GetLucklist(id).Bjkl8Qihao + "期幸运28";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "LuckId=" + id + "";
        else
            strWhere += "LuckId=" + id + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Luckpay> listLuckpay = new BCW.BLL.Game.Luckpay().GetLuckpays(pageIndex, pageSize, strWhere, out recordCount);
        // if (!string.IsNullOrEmpty(model.PostNum))
        if (listLuckpay.Count > 0)
        {

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + new BCW.BLL.Game.Lucklist().GetLucklist(id).Bjkl8Qihao + "期开出号码:<b>" + model.SumNum + "=" + model.PostNum + "</b><br/>");
            builder.Append("时间为" + model.BeginTime + "到" + model.EndTime + "");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Luckpay n in listLuckpay)
            {
                string[] buy = { "大", "小", "单", "双", "大单", "小单", "大双", "小双", "一段", "二段", "三段", "全包", "自选", "尾数" };
                string[] eng = { "Luck28Big", "Luck28Small", "Luck28Single", "Luck28Double", "Luck28BigSingle", "Luck28SmallSingle"
                                   , "Luck28BigDouble", "Luck28SmallDouble", "Luck28First", "Luck28Secend", "Luck28Three","Luck28All", "Luck28Choose", "Luck28End"};
                string type = string.Empty;
                for (int i = 0; i < eng.Length; i++)
                {
                    if (n.BuyType == eng[i])
                    {
                        type = buy[i];
                    }
                }
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string shuoming = string.Empty;
                if (n.BuyType == "Luck28End")
                {
                    //  builder.Append("+"+model.SumNum+"+"+ n.BuyNum+"<br/>");
                    string EH = GetLast(n.BuyNum); //得到尾数
                                                   //Utils.Error("n.BuyNum:"+ n.BuyNum, "");
                    shuoming = "" + type + EH + "，每尾数" + n.BuyCent + "";
                }
                else if (n.BuyType == "Luck28Choose")
                {
                    string CH = GetChooseOdds(n.BuyNum);
                    shuoming = "" + type + CH + "，每数字" + n.BuyCent + "";
                }
                else
                {
                    shuoming = "" + type + "" + "，" + n.BuyCent + "";
                }
                string odds = "" + GOdds(n.BuyType) + "";
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 0)
                {
                    if (n.BuyType == "Luck28End")
                    {
                        builder.Append("押" + shuoming + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    }
                    else if (n.BuyType == "Luck28Choose")
                    {
                        builder.Append("押" + shuoming + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    }
                    else
                    {
                        builder.Append("押" + shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    }
                }
                else if (n.State == 1)
                {
                    if (n.BuyType == "Luck28End")
                    {
                        builder.Append("押" + shuoming + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    }
                    else if (n.BuyType == "Luck28Choose")
                    {
                        builder.Append("押" + shuoming + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    }
                    else
                    {
                        builder.Append("押" + shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                    }
                    // builder.Append("押" + shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else//已经兑奖的
                {
                    if (n.BuyType == "Luck28End")
                    {
                        builder.Append("押" + shuoming + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    }
                    else if (n.BuyType == "Luck28Choose")
                    {
                        builder.Append("押" + shuoming + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    }
                    else
                    {
                        builder.Append("押" + shuoming + "" + ub.Get("SiteBz") + "赔率为" + n.odds + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + ",标识-" + n.ID + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    }
                    // builder.Append("押" + shuoming + ub.Get("SiteBz") + "赔率为" + n.odds + odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                }
                // builder.Append("押" + shuoming + "" + ub.Get("SiteBz") + "赔率为" + n.odds + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + new BCW.BLL.Game.Lucklist().GetLucklist(id).Bjkl8Qihao + "期开出号码:<b>" + model.SumNum + "=" + model.PostNum + "</b><br/>");
            builder.Append("时间为" + model.BeginTime + "到" + model.EndTime + "");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private string paixu(int[] a)
    {
        string result = "";
        int temp = 0;

        for (int i = 0; i < a.Length - 1; i++)
        {
            for (int j = i + 1; j < a.Length; j++)
            {
                if ((a[i]) > (a[j]))
                {
                    temp = (a[i]);
                    a[i] = a[j];
                    a[j] = temp;
                }
            }
        }
        for (int k = 0; k < a.Length; k++)
        {
            if (result == "")
            {
                result = result + a[k];
            }
            else
            {
                result = result + "," + a[k];
            }
        }
        return result;

    }
    /// <summary>
    /// 计算幸运数字，和三个数值
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string[] CalNum(string str)
    {
        string[] nums = str.Split(",".ToCharArray());
        int one = 0;
        int two = 0;
        int three = 0;
        if (nums.Length > 1)
        {
            for (int i = 0; i < 6; i++)
            {
                int aa = Utils.ParseInt(nums[i]);
                one = one + aa;
            }
            one = one % 10;//算出第1个值
            for (int j = 6; j < 12; j++)
            {
                int bb = Utils.ParseInt(nums[j]);
                two = two + bb;
            }
            two = two % 10;//算出第2个值
            for (int k = 12; k < 18; k++)
            {
                three = three + Utils.ParseInt(nums[k]);
            }
            three = three % 10;//算出第3个值
        }
        int all = one + two + three;//总的数值
        string postnum = one + "+" + two + "+" + three;
        string[] getback = { postnum, all.ToString() };
        return getback;
    }

    //手动开奖
    private void OpenPrize()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误2"));
        BCW.Model.Game.Lucklist model = new BCW.BLL.Game.Lucklist().GetLucklist(id);
        string BeginTime = model.BeginTime.ToString().Replace('/', '-');
        string EndTime = model.EndTime.ToString().Replace('/', '-');
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info2 = Utils.GetRequest("info2", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", "/Controls/luck28.xml") + "</a>&gt;人工开奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("需要手动开奖的期号为:" + new BCW.BLL.Game.Lucklist().GetLucklist(id).Bjkl8Qihao + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "开盘时间:/,开奖时间:/,开奖号码(格式如:123、246、135等)/,,,";
            strName = "begin,end,opennums,info,id,act";
            strType = "text,text,text,hidden,hidden,hidden";
            strValu = "" + BeginTime + "'" + EndTime + "''ok'" + id + "'openprize";
            strEmpt = "false,false,false,fasle,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            string opennums = Utils.GetRequest("opennums", "post", 2, @"^\d{3}$", "填写数值错误！如123");
            DateTime BeginTime1 = Utils.ParseTime(Utils.GetRequest("begin", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
            DateTime EndTime1 = Utils.ParseTime(Utils.GetRequest("end", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
            char[] nums = opennums.ToCharArray();
            int sum = 0;
            string postnums = string.Empty;
            for (int i = 0; i < nums.Length; i++)
            {
                //  builder.Append(Convert.ToInt16(nums[i].ToString())+"<br/>");
                if (string.IsNullOrEmpty(postnums))
                {
                    postnums += nums[i];
                }
                else
                {
                    postnums += "+" + nums[i];
                }
                sum += Convert.ToInt16(nums[i].ToString());
                // builder.Append(sum + "<br/>");
            }
            //  builder.Append(postnums + "<br/>");
            if (sum > 27 || sum < 0)
            {
                Utils.Error("填写的三个数值和值不能大于27，请重新输入！", "");
            }
            if (!new BCW.BLL.Game.Lucklist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            if (info2 == "")
            {

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("开奖号码为:" + postnums + "<br/>");
                builder.Append("开奖结果为:" + sum + "<br/>");
                builder.Append("开盘时间:" + BeginTime + "<br/>");
                builder.Append("开奖时间:" + EndTime + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                postnums = postnums.Replace("+", "");
                strText = "开盘时间:/,开奖时间:/,开奖号码(格式如:123、246、135等)/,,,,";
                strName = "begin,end,opennums,info,info2,id,act";
                strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                strValu = "" + BeginTime + "'" + EndTime + "'" + postnums + "'ok'ok'" + id + "'openprize";
                strEmpt = "false,false,false,false,fasle,fasle,false";
                strIdea = "/";
                strOthe = "确定编辑,luck28.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                //记录日志
                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
                {
                    String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                    LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/编辑幸运28期数:" + id + "|是否预设:" + (sum) + "");
                }
                //builder.Append("id:" + id + "<br/>");
                //builder.Append("sum:" + sum + "<br/>");
                //builder.Append("postnums:" + postnums + "<br/>");
                //builder.Append("BeginTime1:" + BeginTime1 + "<br/>");
                //builder.Append("EndTime1:" + EndTime1 + "<br/>");
                BCW.Model.Game.Lucklist model1 = new BCW.Model.Game.Lucklist();
                model.ID = id;
                // model.Bjkl8Nums = bjkl;
                model.SumNum = sum;
                model.PostNum = postnums;
                model.LuckCent = new BCW.BLL.Game.Luckpay().GetSumBuyCent(model.ID, sum);
                model.State = 1;
                model.BeginTime = BeginTime1;
                model.EndTime = EndTime1;
                new BCW.BLL.Game.Lucklist().Update(model);
                duijiang(sum, id);  //兑奖  -》增加期数
                Utils.Success("编辑第" + new BCW.BLL.Game.Lucklist().GetLucklist(id).Bjkl8Qihao + "期", "编辑第" + new BCW.BLL.Game.Lucklist().GetLucklist(id).Bjkl8Qihao + "期成功..", Utils.getUrl("luck28.aspx"), "1");
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //设置机械人下注次数
    private void RobotBuy()
    {

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;设置机械人下注次数");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("设置机械人下注次数");
        //builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "机械人下注次数:/,,";
            strName = "robotbuy,info,act";
            strType = "num,hidden,hidden";
            strValu = ub.GetSub("Luck28RobotBuy", xmlPath) + "'ok'robotbuy";
            strEmpt = "false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {

            string robotbuy1 = (Utils.GetRequest("robotbuy", "post", 2, @"^\d+$", "设置机械人下注次数填写出错"));
            xml.dss["Luck28RobotBuy"] = robotbuy1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    /// <summary>
    ///   遍历数据兑奖
    /// </summary>
    /// <param name="num">开奖号码</param>
    /// <param name="ID">期数</param>
    /// <returns>返回所有玩家赢的钱</returns>
    private long duijiang(int num, int ID)
    {
        #region   遍历数据兑奖
        DataSet model = new BCW.BLL.Game.Luckpay().GetList("*", "LuckId=" + ID + " and State=0");//遍历这一期所有的下注
        long allcents = 0;//所有玩家赢的总钱数
        for (int i = 0; i < model.Tables[0].Rows.Count; i++)
        {
            //Response.Write("抓取网站最新已开奖号码是:<br/>" + getresult + "<br/>");
            long wincents = 0;
            string r = model.Tables[0].Rows[i][4].ToString();
            string types = model.Tables[0].Rows[i][10].ToString();//得到玩家买的类型
            // Response.Write("<br/>第" + (i + 1) + "注:" + r + "<br/>");
            // string[] BuyNum = r.Split(",".ToCharArray());//得到每注的下注号码
            double odds = 1; ;
            if (("," + r + ",").Contains("," + num + ","))//中奖
            {
                #region   switch(types)
                switch (types)
                {
                    case "Luck28Choose":
                        odds = float.Parse((ub.GetSub("Buy" + num + "odds", xmlPath)));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        //new BCW.BLL.Guest().Add(0, 729, "", "Luck28Choose出错了");
                        break;
                    case "Luck28End":
                        if (num % 10 == 0)//尾数0
                        {
                            odds = float.Parse(ub.GetSub("End0odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 1)//尾数1
                        {
                            odds = float.Parse(ub.GetSub("End1odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 2)//尾数2
                        {
                            odds = float.Parse(ub.GetSub("End2odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 3)//尾数3
                        {
                            odds = float.Parse(ub.GetSub("End3odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 4)//尾数4
                        {
                            odds = float.Parse(ub.GetSub("End4odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 5)//尾数5
                        {
                            odds = float.Parse(ub.GetSub("End5odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 6)//尾数6
                        {
                            odds = float.Parse(ub.GetSub("End6odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 7)//尾数7
                        {
                            odds = float.Parse(ub.GetSub("End7odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 8)//尾数8
                        {
                            odds = float.Parse(ub.GetSub("End8odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else if (num % 10 == 9)//尾数9
                        {
                            odds = float.Parse(ub.GetSub("End9odds", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][5]) * odds);  //计算赢的钱
                        }
                        else
                        {
                            //new BCW.BLL.Guest().Add(0, 729, "", "Luck28End出错了");
                        }
                        break;
                    case "Luck28Big":
                        // odds = float.Parse(ub.GetSub("Luck28Big", xmlPath));//根据类型得到XML的赔率
                        odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Big错了");
                        break;
                    case "Luck28Small":
                        odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Small出错了");
                        break;
                    case "Luck28Single":
                        //Utils.Error("" + (ub.GetSub("Luck28Single", xmlPath)) + "", "");
                        odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        //  new BCW.BLL.Guest().Add(0, 729, "", "Luck28Single出错了");
                        break;
                    case "Luck28Double":
                        odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        //new BCW.BLL.Guest().Add(0, 729, "", "Luck28Double出错了");
                        break;
                    case "Luck28BigSingle":
                        odds = float.Parse(ub.GetSub("Luck28BigSingle", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28BigSingle出错了");
                        break;
                    case "Luck28SmallSingle":
                        odds = float.Parse((ub.GetSub("Luck28SmallSingle", xmlPath)));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28SmallSingle出错了");
                        break;
                    case "Luck28BigDouble":
                        odds = float.Parse(ub.GetSub("Luck28BigDouble", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28BigDouble出错了");
                        break;
                    case "Luck28SmallDouble":
                        odds = float.Parse(ub.GetSub("Luck28SmallDouble", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28SmallDouble出错了");
                        break;
                    case "Luck28Secend":
                        odds = float.Parse(ub.GetSub("Luck28Secend", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Secend出错了");
                        break;
                    case "Luck28First":
                        odds = float.Parse(ub.GetSub("Luck28First", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28First出错了");
                        break;
                    case "Luck28Three":
                        odds = float.Parse(ub.GetSub("Luck28Three", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Three出错了");
                        break;
                    case "Luck28All":
                        odds = float.Parse(ub.GetSub("Luck28All", xmlPath));//根据类型得到XML的赔率
                        wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i][6]) * odds);  //计算赢的钱
                        //  new BCW.BLL.Guest().Add(0, 729, "", "Luck28All出错了");
                        break;

                }
                #endregion
                //发系统内线
                new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.Tables[0].Rows[i][2]), model.Tables[0].Rows[i][3].ToString(), "恭喜你!你在幸运28第" + model.Tables[0].Rows[i][1] + "期赢的了" + wincents + ub.Get("SiteBz") + "[url=/bbs/game/luck28.aspx?act=case]马上兑奖[/url]");
                new BCW.BLL.Game.Luckpay().Update(Convert.ToInt32(model.Tables[0].Rows[i][0]), wincents, 1);
                // Response.Write("赢的钱：" + wincents + "<br/>");
                allcents = allcents + wincents;//计算玩家一共赢了多少钱
            }
            else
            //if (flag == 0)//不中奖
            {
                new BCW.BLL.Game.Luckpay().Update(Convert.ToInt32(model.Tables[0].Rows[i][0]), 0, 1);
            }

        } //遍历下注
        return allcents;

        #endregion

    }
    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "幸运28ID错误"));
        Master.Title = "编辑幸运28";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑第" + id + "期幸运28");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Lucklist model = new BCW.BLL.Game.Lucklist().GetLucklist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("当开出数字非0时，开奖时则自动产生组合数字并作为开奖结果");
            builder.Append(Out.Tab("</div>", ""));
        }
        string BeginTime = model.BeginTime.ToString().Replace('/', '-');
        string EndTime = model.EndTime.ToString().Replace('/', '-');
        string strText = "幸运数字:/,三个数值(如1+2+3):/,开盘时间:/,开奖时间:/,,,";
        string strName = "SumNum,PostNum,BeginTime,EndTime,id,act,backurl";
        string strType = "num,text,date,date,hidden,hidden,hidden";
        string strValu = "" + model.SumNum + "'" + model.PostNum + "'" + BeginTime + "'" + EndTime + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,luck28.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string postnum = Utils.GetRequest("PostNum", "post", 2, @"^([0-9]+[+]?){1,500}$", "填写数值错误！如1+2+3");
        int SumNum = int.Parse(Utils.GetRequest("SumNum", "post", 2, @"^[0-9]\d*$", "幸运数字填写错误"));
        if (SumNum < 0 || SumNum > 27)
        {
            Utils.Error("幸运数字填写错误", "");
        }
        string[] nums = postnum.Split('+');
        int result = 0;
        if (nums[0] != "0" || nums[1] != "0" || nums[2] != "0")//如果三个数只要有一个是0
        {
            for (int i = 0; i < nums.Length; i++)
            {
                result = result + Utils.ParseInt(nums[i]);  //算出总和
            }
        }
        if (result != SumNum)
        {
            Utils.Error("填写的三个数值和值与幸运数字不相等，请重新输入！", "");
        }
        DateTime BeginTime = Utils.ParseTime(Utils.GetRequest("BeginTime", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

        if (!new BCW.BLL.Game.Lucklist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //记录日志
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
            LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/编辑幸运28期数:" + id + "|是否预设:" + SumNum + "");
        }
        BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
        model.ID = id;
        model.SumNum = SumNum;
        model.PostNum = postnum;
        model.State = 1;
        model.BeginTime = BeginTime;
        model.EndTime = EndTime;
        new BCW.BLL.Game.Lucklist().Update(model);

        Utils.Success("编辑第" + id + "期", "编辑第" + id + "期成功..", Utils.getUrl("luck28.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    //设置单个号码的赔率
    private void BuyOdds()
    {
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        //string[] odds = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        //string[] save = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        ////for (int i = 0; i < 28; i++)
        //{
        //    odds[i] = ub.GetSub("Buy"+i+"odds", xmlPath);//获取0-27号码的赔率
        //}


        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;编辑单个号码赔率");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("编辑单个号码赔率");
        //builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "号码0赔率:";
            strName = "Buy0odds";
            strType = "text";
            strValu = ub.GetSub("Buy0odds", xmlPath);
            strEmpt = "false";
            for (int i = 1; i < 28; i++)
            {
                strText = strText + ",号码" + i + "赔率:";
                strName = strName + ",Buy" + i + "odds";
                strType = strType + ",text";
                strValu = strValu + "'" + ub.GetSub("Buy" + i + "odds", xmlPath);
                strEmpt = strEmpt + ",false";
            }
            strText = strText + ",,";
            strName = strName + ",info,act";
            strType = strType + ",hidden,hidden";
            strValu = strValu + "'ok'buyodds";
            strEmpt = strEmpt + ",false,false";

            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string[] odds = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //string[] save = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            for (int i = 0; i < 28; i++)
            {
                odds[i] = Utils.GetRequest("Buy" + i + "odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置号码" + i + "赔率出错");

            }
            for (int i = 0; i < 28; i++)
            {
                xml.dss["Buy" + i + "odds"] = odds[i];
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


    }
    private void StatPage()
    {
        Master.Title = "" + "" + "_赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        //今天投入数+今天兑奖数
        // new BCW.BLL.Game.Luckpay().GetPrice

        long TodayTou = new BCW.BLL.Game.Luckpay().ManGetPrice("BuyCents", "DateDiff(dd,AddTime,getdate())=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long TodayDui = new BCW.BLL.Game.Luckpay().ManGetPrice("WinCent", "DateDiff(dd,AddTime,getdate())=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //昨天投入数+昨天兑奖数
        long yesTou = new BCW.BLL.Game.Luckpay().ManGetPrice("BuyCents", "DateDiff(dd,AddTime,getdate()-1)=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long yesDui = new BCW.BLL.Game.Luckpay().ManGetPrice("WinCent", "DateDiff(dd,AddTime,getdate()-1)=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //本月投入数+本月兑奖数
        long MonthTou = new BCW.BLL.Game.Luckpay().ManGetPrice("BuyCents", "datediff(month,[AddTime],getdate())=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long MonthDui = new BCW.BLL.Game.Luckpay().ManGetPrice("WinCent", "datediff(month,[AddTime],getdate())=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //上月投入数+上月兑奖数
        long Month2Tou = new BCW.BLL.Game.Luckpay().ManGetPrice("BuyCents", "datediff(month,[AddTime],getdate())=1 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long Month2Dui = new BCW.BLL.Game.Luckpay().ManGetPrice("WinCent", "datediff(month,[AddTime],getdate())=1 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //今年投入+今年兑奖
        long yearTou = new BCW.BLL.Game.Luckpay().ManGetPrice("BuyCents", "datediff(YEAR,[AddTime],getdate())=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long yearDui = new BCW.BLL.Game.Luckpay().ManGetPrice("WinCent", "datediff(YEAR,[AddTime],getdate())=0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //总投入+总兑奖
        long allTou = new BCW.BLL.Game.Luckpay().ManGetPrice("BuyCents", "State>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long allDui = new BCW.BLL.Game.Luckpay().ManGetPrice("WinCent", "State>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利：" + (TodayTou - TodayDui) + "" + ub.Get("SiteBz") + "<br/>今天收支：收" + TodayTou + "，支" + TodayDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利：" + (yesTou - yesDui) + "" + ub.Get("SiteBz") + "<br/>昨天收支：收" + yesTou + "，支" + yesDui + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("本月赢利：" + (MonthTou - MonthDui) + "" + ub.Get("SiteBz") + "<br/>本月收支：收" + MonthTou + "，支" + MonthDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利：" + (Month2Tou - Month2Dui) + "" + ub.Get("SiteBz") + "<br/>上月收支：收" + Month2Tou + "，支" + Month2Dui + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("今年赢利：" + (yearTou - yearDui) + "" + ub.Get("SiteBz") + "<br/>今年收支：收" + yearTou + "，支" + yearDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>总赢利：" + (allTou - allDui) + "" + ub.Get("SiteBz") + "<br/>总收支：收" + allTou + "，支" + allDui + "</b>");
        builder.Append(Out.Tab("</div>", Out.Hr()));


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            long dateTou = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "AddTime>='" + searchday1 + "' and AddTime<='" + searchday2 + "'AND isRobot='0'");
            long dateDui = new BCW.BLL.Game.Luckpay().GetPrice("WinCent", "AddTime>='" + searchday1 + "' and AddTime<='" + searchday2 + "'AND isRobot='0'");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利" + (dateTou - dateDui) + "酷币.<br/>收" + dateTou + "，支" + dateDui + "</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,luck28.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利0" + ub.Get("SiteBz") + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,luck28.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("luck28.aspx") + "\">&lt;&lt;&lt;返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));


    }

    //设置浮动最大最小赔率
    private void MaxMinOdds()
    {
        double BSMin = Convert.ToDouble(ub.GetSub("Luck28BSMin", xmlPath));//大小最低赔率
        double BSMax = Convert.ToDouble(ub.GetSub("Luck28BSMax", xmlPath));//大小最高赔率

        double SDMin = Convert.ToDouble(ub.GetSub("Luck28SDMin", xmlPath));//单双最低赔率
        double SDMax = Convert.ToDouble(ub.GetSub("Luck28SDMax", xmlPath));//单双最高赔率
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;最大最小的赔率");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "设置大小最高赔率:/,设置大小最低赔率:/,设置单双最高赔率:/,设置单双最低赔率:/,,";
            strName = "BSMax,BSMin,SDMax,SDMin,info,act";
            strType = "text,text,text,text,hidden,hidden";
            strValu = BSMax + "'" + BSMin + "'" + SDMax + "'" + SDMin + "'ok'MMOdds";
            strEmpt = "false,false,false,false,false,,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string BSMax1 = (Utils.GetRequest("BSMax", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置大小最高赔率填写出错"));
            string BSMin1 = (Utils.GetRequest("BSMin", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置大小最低赔率填写出错"));
            string SDMax1 = (Utils.GetRequest("SDMax", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置单双最高赔率填写出错"));
            string SDMin1 = (Utils.GetRequest("SDMin", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置单双最低赔率填写出错"));
            xml.dss["Luck28BSMax"] = BSMax1;
            xml.dss["Luck28BSMin"] = BSMin1;
            xml.dss["Luck28SDMax"] = SDMax1;
            xml.dss["Luck28SDMin"] = SDMin1;
            //xml.dss["Luck28All"] = Luck28All1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //设置浮动大小额度
    private void FloatOdds()
    {
        string Luck28OddsSub = ub.GetSub("Luck28OddsSub", xmlPath);
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;设置浮动大小");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "设置浮动额度赔率:/,,";
            strName = "Luck28OddsSub,info,act";
            strType = "text,hidden,hidden";
            strValu = Luck28OddsSub + "'ok'floatodds";
            strEmpt = "false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string Luck28OddsSub1 = (Utils.GetRequest("Luck28OddsSub", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置浮动赔率填写出错"));
            xml.dss["Luck28OddsSub"] = Luck28OddsSub1;
            //xml.dss["Luck28All"] = Luck28All1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //设置分段赔率
    private void OttOdds()
    {
        string Luck28Three = ub.GetSub("Luck28Three", xmlPath);
        string Luck28First = ub.GetSub("Luck28First", xmlPath);
        string Luck28Secend = ub.GetSub("Luck28Secend", xmlPath);
        string Luck28All = ub.GetSub("Luck28All", xmlPath);

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;编辑分段赔率");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "设置押一段赔率:/,设置押二段赔率:/,设置押三段赔率:/,,";
            strName = "Luck28First,Luck28Secend,Luck28Three,info,act";
            strType = "text,text,text,hidden,hidden";
            strValu = Luck28First + "'" + Luck28Secend + "'" + Luck28Three + "'ok'ottodds";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string Luck28First1 = (Utils.GetRequest("Luck28First", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));
            string Luck28Secend1 = (Utils.GetRequest("Luck28Secend", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));
            string Luck28Three1 = (Utils.GetRequest("Luck28Three", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));
            //string Luck28All1 = (Utils.GetRequest("Luck28All", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));
            xml.dss["Luck28First"] = Luck28First1;
            xml.dss["Luck28Secend"] = Luck28Secend1;
            xml.dss["Luck28Three"] = Luck28Three1;
            //xml.dss["Luck28All"] = Luck28All1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //设置尾数赔率
    private void EndOdds()
    {
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
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;编辑尾数赔率");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("编辑尾数赔率");
        //builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "0尾赔率:/,1尾赔率:/,2尾赔率:/,3尾赔率:/,4尾赔率:/,5尾赔率:/,6尾赔率:/,7尾赔率:/,8尾赔率:/,9尾赔率:/,,";
            strName = "End0odds,End1odds,End2odds,End3odds,End4odds,End5odds,End6odds,End7odds,End8odds,End9odds,info,act";
            strType = "text,text,text,text,text,text,text,text,text,text,hidden,hidden";
            strValu = End0odds + "'" + End1odds + "'" + End2odds + "'" + End3odds + "'" + End4odds + "'" + End5odds + "'" + End6odds + "'" + End7odds + "'" + End8odds + "'" + End9odds + "'ok'endodds";
            strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            string End0odds1 = (Utils.GetRequest("End0odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置0尾赔率填写出错"));
            string End1odds1 = (Utils.GetRequest("End1odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置1尾赔率填写出错"));
            string End2odds1 = (Utils.GetRequest("End2odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置2尾赔率填写出错"));
            string End3odds1 = (Utils.GetRequest("End3odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置3尾赔率填写出错"));
            string End4odds1 = (Utils.GetRequest("End4odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置4尾赔率填写出错"));
            string End5odds1 = (Utils.GetRequest("End5odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置5尾赔率填写出错"));
            string End6odds1 = (Utils.GetRequest("End6odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置6尾赔率填写出错"));
            string End7odds1 = (Utils.GetRequest("End7odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置7尾赔率填写出错"));
            string End8odds1 = (Utils.GetRequest("End8odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置8尾赔率填写出错"));
            string End9odds1 = (Utils.GetRequest("End9odds", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置9尾赔率填写出错"));

            xml.dss["End0odds"] = End0odds1;
            xml.dss["End1odds"] = End1odds1;
            xml.dss["End2odds"] = End2odds1;
            xml.dss["End3odds"] = End3odds1;
            xml.dss["End4odds"] = End4odds1;
            xml.dss["End5odds"] = End5odds1;
            xml.dss["End6odds"] = End6odds1;
            xml.dss["End7odds"] = End7odds1;
            xml.dss["End8odds"] = End8odds1;
            xml.dss["End9odds"] = End9odds1;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");

        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //设置半数赔率
    private void SetHalfOdds()
    {

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;设置半数赔率");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("设置半数浮动赔率");
        //builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "押大赔率:/,押小赔率:/,押单赔率:/,押双赔率:/,押大单赔率:/,押小单赔率:/,押大双赔率:/,押小双赔率:/,,";
            strName = "Luck28Big,Luck28Small,Luck28Single,Luck28Double,Luck28BigSingle,Luck28SmallSingle,Luck28BigDouble,Luck28SmallDouble,info,act";
            strType = "text,text,text,text,text,text,text,text,hidden,hidden";
            strValu = ub.GetSub("Luck28Big", xmlPath) + "'" + ub.GetSub("Luck28Small", xmlPath) + "'" + ub.GetSub("Luck28Single", xmlPath) + "'" + ub.GetSub("Luck28Double", xmlPath) + "'" + ub.GetSub("Luck28BigSingle", xmlPath) + "'" + ub.GetSub("Luck28SmallSingle", xmlPath) + "'" + ub.GetSub("Luck28BigDouble", xmlPath) + "'" + ub.GetSub("Luck28SmallDouble", xmlPath) + "'ok'sethalfodds";
            strEmpt = "false,false,false,false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {

            string Luck28Big1 = (Utils.GetRequest("Luck28Big", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押大赔率填写出错")); ;//半数浮动设置下注额
            string Luck28Small1 = (Utils.GetRequest("Luck28Small", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押小赔率填写出错"));
            string Luck28Single1 = (Utils.GetRequest("Luck28Single", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押单赔率填写出错"));
            string Luck28Double1 = (Utils.GetRequest("Luck28Double", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押双赔率填写出错"));
            string Luck28BigSingle1 = (Utils.GetRequest("Luck28BigSingle", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押大单赔率填写出错"));
            string Luck28SmallSingle1 = (Utils.GetRequest("Luck28SmallSingle", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押小单赔率填写出错"));
            string Luck28BigDouble1 = (Utils.GetRequest("Luck28BigDouble", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押大双赔率填写出错"));
            string Luck28SmallDouble1 = (Utils.GetRequest("Luck28SmallDouble", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置押小双赔率填写出错"));

            xml.dss["Luck28Big"] = Luck28Big1;
            xml.dss["Luck28Small"] = Luck28Small1;
            xml.dss["Luck28Single"] = Luck28Single1;
            xml.dss["Luck28Double"] = Luck28Double1;
            xml.dss["Luck28BigSingle"] = Luck28BigSingle1;
            xml.dss["Luck28SmallSingle"] = Luck28SmallSingle1;
            xml.dss["Luck28BigDouble"] = Luck28BigDouble1;
            xml.dss["Luck28SmallDouble"] = Luck28SmallDouble1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");

        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //设置半数最低浮动赔率
    private void SetHalfMinOdds()
    {
        double Luck28Big = Convert.ToDouble(ub.GetSub("Luck28Big", xmlPath));//  大赔率
        double Luck28Small = Convert.ToDouble(ub.GetSub("Luck28Small", xmlPath));//小赔率

        double Luck28Single = Convert.ToDouble(ub.GetSub("Luck28Single", xmlPath));
        double Luck28Double = Convert.ToDouble(ub.GetSub("Luck28Double", xmlPath));

        double Luck28BigSingle = Convert.ToDouble(ub.GetSub("Luck28BigSingle", xmlPath));
        double Luck28SmallSingle = Convert.ToDouble(ub.GetSub("Luck28SmallSingle", xmlPath));

        double Luck28BigDouble = Convert.ToDouble(ub.GetSub("Luck28BigDouble", xmlPath));
        double Luck28SmallDouble = Convert.ToDouble(ub.GetSub("Luck28SmallDouble", xmlPath));

        double Luck28BSMin = Convert.ToDouble(ub.GetSub("Luck28BSMin", xmlPath));//大小最低赔率
        double Luck28SDMin = Convert.ToDouble(ub.GetSub("Luck28SDMin", xmlPath));
        double Luck28BSSMin = Convert.ToDouble(ub.GetSub("Luck28BSSMin", xmlPath));
        double Luck28BSDMin = Convert.ToDouble(ub.GetSub("Luck28BSDMin", xmlPath));

        double Luck28OddsSub = Convert.ToDouble(ub.GetSub("Luck28OddsSub", xmlPath));//赔率加减位数

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置     
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("设置半数最低浮动赔率");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "设置大小最低赔率:/,设置单双最低赔率:/,设置大小单最低赔率:/,设置大小双最低赔率:/,设置赔率加减位数:/,,";
            strName = "Luck28BSMin,Luck28SDMin,Luck28BSSMin,Luck28BSDMin,Luck28OddsSub,info,act";
            strType = "num,num,num,num,num,hidden,hidden";
            strValu = Luck28BSMin + "'" + Luck28SDMin + "'" + Luck28BSSMin + "'" + Luck28BSDMin + "'" + Luck28OddsSub + "'ok'sethalfminodds";
            strEmpt = "false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string Luck28BSMin1 = (Utils.GetRequest("Luck28BSMin", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置大小最低赔率填写出错")); ;//分段每个段设置下注额
            string Luck28SDMin1 = (Utils.GetRequest("Luck28SDMin", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置单双最低赔率填写出错"));
            string Luck28BSSMin1 = (Utils.GetRequest("Luck28BSSMin", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置大小单最低赔率填写出错"));
            string Luck28BSDMin1 = (Utils.GetRequest("Luck28BSDMin", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置大小双最低赔率填写出错"));
            string Luck28OddsSub1 = (Utils.GetRequest("Luck28OddsSub", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置赔率加减位数填写出错"));
            xml.dss["Luck28BSMin"] = Luck28BSMin1;  //修改半数浮动设置下注额
            xml.dss["Luck28SDMin"] = Luck28SDMin1;
            xml.dss["Luck28BSSMin"] = Luck28BSSMin1;
            xml.dss["Luck28BSDMin"] = Luck28BSDMin1;
            xml.dss["Luck28OddsSub"] = Luck28OddsSub1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");

        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //设置分段下注金额
    private void OttCents()
    {
        string Luck28FirstCents = ub.GetSub("Luck28FirstCents", xmlPath);//分段每个段设置下注额
        string Luck28SecendCents = ub.GetSub("Luck28SecendCents", xmlPath);
        string Luck28ThreeCents = ub.GetSub("Luck28ThreeCents", xmlPath);

        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;分段最大限额");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("编辑分段");
        //builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "一段限额:/,二段限额:/,三段限额::/,,";
            strName = "Luck28FirstCents,Luck28SecendCents,Luck28ThreeCents,info,act";
            strType = "text,text,text,hidden,hidden";
            strValu = Luck28FirstCents + "'" + Luck28SecendCents + "'" + Luck28ThreeCents + "'ok'ottcents";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置

            long Luck28FirstCents1 = Int64.Parse(Utils.GetRequest("Luck28FirstCents", "post", 4, @"^[0-9]\d*$", "设置一段金额填写出错")); ;//分段每个段设置下注额
            long Luck28SecendCents1 = Int64.Parse(Utils.GetRequest("Luck28SecendCents", "post", 4, @"^[0-9]\d*$", "设置二段金额填写出错"));
            long Luck28ThreeCents1 = Int64.Parse(Utils.GetRequest("Luck28ThreeCents", "post", 4, @"^[0-9]\d*$", "设置三段金额填写出错"));
            //long Luck28AllCents1 = Int64.Parse(Utils.GetRequest("Luck28AllCents", "post", 4, @"^[0-9]\d*$", "设置押全包金额填写出错"));
            //string Luck28First1 = (Utils.GetRequest("Luck28First", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));
            //string Luck28Secend1 = (Utils.GetRequest("Luck28Secend", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));
            //string Luck28Three1 = (Utils.GetRequest("Luck28Three", "post", 2, @"^[0-9]+(\.[0-9]+)?$", "设置一段赔率填写出错"));

            //xml.dss["Luck28First"] = Luck28First1;
            //xml.dss["Luck28Secend"] = Luck28Secend1;
            //xml.dss["Luck28Three"] = Luck28Three1;
            xml.dss["Luck28FirstCents"] = Luck28FirstCents1;  //修改半数浮动设置下注额
            xml.dss["Luck28SecendCents"] = Luck28SecendCents1;
            xml.dss["Luck28ThreeCents"] = Luck28ThreeCents1;
            //xml.dss["Luck28AllCents"] = Luck28AllCents1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //设置半数最大下注金额
    private void HalfCents()
    {
        string Luck28BigCents = ub.GetSub("Luck28BigCents", xmlPath);//半数浮动设置下注额
        string Luck28SmallCents = ub.GetSub("Luck28SmallCents", xmlPath);
        string Luck28SingleCents = ub.GetSub("Luck28SingleCents", xmlPath);
        string Luck28DoubleCents = ub.GetSub("Luck28DoubleCents", xmlPath);
        string Luck28BigSingleCents = ub.GetSub("Luck28BigSingleCents", xmlPath);
        string Luck28SmallSingleCents = ub.GetSub("Luck28SmallSingleCents", xmlPath);
        string Luck28BigDoubleCents = ub.GetSub("Luck28BigDoubleCents", xmlPath);
        string Luck28SmallDoubleCents = ub.GetSub("Luck28SmallDoubleCents", xmlPath);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;半数最大限额");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("编辑最大金额");
        //builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            strText = "押大限额(浮动限额):/,押小限额(浮动限额):/,押单限额(浮动限额):/,押双限额(浮动限额):/,押大单限额(浮动限额):/,押小单限额(浮动限额):/,押大双限额(浮动限额):/,押小双限额(浮动限额):/,,";
            strName = "Luck28BigCents,Luck28SmallCents,Luck28SingleCents,Luck28DoubleCents,Luck28BigSingleCents,Luck28SmallSingleCents,Luck28BigDoubleCents,Luck28SmallDoubleCents,info,act";
            strType = "text,text,text,text,text,text,text,text,hidden,hidden";
            strValu = Luck28BigCents + "'" + Luck28SmallCents + "'" + Luck28SingleCents + "'" + Luck28DoubleCents + "'" + Luck28BigSingleCents + "'" + Luck28SmallSingleCents + "'" + Luck28BigDoubleCents + "'" + Luck28SmallDoubleCents + "'ok'halfcents";
            strEmpt = "false,false,false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置

            //  long BuyCent = Int64.Parse(Utils.GetRequest("BuyCent", "post", 4, @"^[0-9]\d*$", "下注金额填写出错"));

            long Luck28BigCents1 = Int64.Parse(Utils.GetRequest("Luck28BigCents", "post", 4, @"^[0-9]\d*$", "设置押大金额填写出错")); ;//半数浮动设置下注额
            long Luck28SmallCents1 = Int64.Parse(Utils.GetRequest("Luck28SmallCents", "post", 4, @"^[0-9]\d*$", "设置押小金额填写出错"));
            long Luck28SingleCents1 = Int64.Parse(Utils.GetRequest("Luck28SingleCents", "post", 4, @"^[0-9]\d*$", "设置押单金额填写出错"));
            long Luck28DoubleCents1 = Int64.Parse(Utils.GetRequest("Luck28DoubleCents", "post", 4, @"^[0-9]\d*$", "设置押双金额填写出错"));
            long Luck28BigSingleCents1 = Int64.Parse(Utils.GetRequest("Luck28BigSingleCents", "post", 4, @"^[0-9]\d*$", "设置押大单金额填写出错"));
            long Luck28SmallSingleCents1 = Int64.Parse(Utils.GetRequest("Luck28SmallSingleCents", "post", 4, @"^[0-9]\d*$", "设置押小单金额填写出错"));
            long Luck28BigDoubleCents1 = Int64.Parse(Utils.GetRequest("Luck28BigDoubleCents", "post", 4, @"^[0-9]\d*$", "设置押大双金额填写出错"));
            long Luck28SmallDoubleCents1 = Int64.Parse(Utils.GetRequest("Luck28SmallDoubleCents", "post", 4, @"^[0-9]\d*$", "设置押小双金额填写出错"));

            xml.dss["Luck28BigCents"] = Luck28BigCents1;  //修改半数浮动设置下注额
            xml.dss["Luck28SmallCents"] = Luck28SmallCents1;
            xml.dss["Luck28SingleCents"] = Luck28SingleCents1;
            xml.dss["Luck28DoubleCents"] = Luck28DoubleCents1;
            xml.dss["Luck28BigSingleCents"] = Luck28BigSingleCents1;
            xml.dss["Luck28SmallSingleCents"] = Luck28SmallSingleCents1;
            xml.dss["Luck28BigDoubleCents"] = Luck28BigDoubleCents1;
            xml.dss["Luck28SmallDoubleCents"] = Luck28SmallDoubleCents1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");

        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


    }

    //设置尾数号码的最大下注金额
    private void LastCents()
    {
        string Luck28Last0Cents = ub.GetSub("Luck28Last0Cents", xmlPath);
        string Luck28Last1Cents = ub.GetSub("Luck28Last1Cents", xmlPath);
        string Luck28Last2Cents = ub.GetSub("Luck28Last2Cents", xmlPath);
        string Luck28Last3Cents = ub.GetSub("Luck28Last3Cents", xmlPath);
        string Luck28Last4Cents = ub.GetSub("Luck28Last4Cents", xmlPath);
        string Luck28Last5Cents = ub.GetSub("Luck28Last5Cents", xmlPath);
        string Luck28Last6Cents = ub.GetSub("Luck28Last6Cents", xmlPath);
        string Luck28Last7Cents = ub.GetSub("Luck28Last7Cents", xmlPath);
        string Luck28Last8Cents = ub.GetSub("Luck28Last8Cents", xmlPath);
        string Luck28Last9Cents = ub.GetSub("Luck28Last9Cents", xmlPath);
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置


        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;尾数最大限额");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("编辑最大金额");

        if (info == "")
        {
            strText = "0尾限额:/,1尾限额:/,2尾限额:/,3尾限额:/,4尾限额:/,5尾限额:/,6尾限额:/,7尾限额:/,8尾限额:/,9尾限额:/,,";
            strName = "Luck28Last0Cents,Luck28Last1Cents,Luck28Last2Cents,Luck28Last3Cents,Luck28Last4Cents,Luck28Last5Cents,Luck28Last6Cents,Luck28Last7Cents,Luck28Last8Cents,Luck28Last9Cents,info,act";
            strType = "text,text,text,text,text,text,text,text,text,text,hidden,hidden";
            strValu = Luck28Last0Cents + "'" + Luck28Last1Cents + "'" + Luck28Last2Cents + "'" + Luck28Last3Cents + "'" + Luck28Last4Cents + "'" + Luck28Last5Cents + "'" + Luck28Last6Cents + "'" + Luck28Last7Cents + "'" + Luck28Last8Cents + "'" + Luck28Last9Cents + "'ok'lastcents";
            strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            string Luck28Last0Cents0 = Utils.GetRequest("Luck28Last0Cents", "post", 2, @"^[0-9]\d*$", "设置0尾限额错误");
            string Luck28Last0Cents1 = Utils.GetRequest("Luck28Last1Cents", "post", 2, @"^[0-9]\d*$", "设置1尾限额错误");
            string Luck28Last0Cents2 = Utils.GetRequest("Luck28Last2Cents", "post", 2, @"^[0-9]\d*$", "设置2尾限额错误");
            string Luck28Last0Cents3 = Utils.GetRequest("Luck28Last3Cents", "post", 2, @"^[0-9]\d*$", "设置3尾限额错误");
            string Luck28Last0Cents4 = Utils.GetRequest("Luck28Last4Cents", "post", 2, @"^[0-9]\d*$", "设置4尾限额错误");
            string Luck28Last0Cents5 = Utils.GetRequest("Luck28Last5Cents", "post", 2, @"^[0-9]\d*$", "设置5尾限额错误");
            string Luck28Last0Cents6 = Utils.GetRequest("Luck28Last6Cents", "post", 2, @"^[0-9]\d*$", "设置6尾限额错误");
            string Luck28Last0Cents7 = Utils.GetRequest("Luck28Last7Cents", "post", 2, @"^[0-9]\d*$", "设置7尾限额错误");
            string Luck28Last0Cents8 = Utils.GetRequest("Luck28Last8Cents", "post", 2, @"^[0-9]\d*$", "设置8尾限额错误");
            string Luck28Last0Cents9 = Utils.GetRequest("Luck28Last9Cents", "post", 2, @"^[0-9]\d*$", "设置9尾限额错误");

            xml.dss["Luck28Last0Cents"] = Luck28Last0Cents0;
            xml.dss["Luck28Last1Cents"] = Luck28Last0Cents1;
            xml.dss["Luck28Last2Cents"] = Luck28Last0Cents2;
            xml.dss["Luck28Last3Cents"] = Luck28Last0Cents3;
            xml.dss["Luck28Last4Cents"] = Luck28Last0Cents4;
            xml.dss["Luck28Last5Cents"] = Luck28Last0Cents5;
            xml.dss["Luck28Last6Cents"] = Luck28Last0Cents6;
            xml.dss["Luck28Last7Cents"] = Luck28Last0Cents7;
            xml.dss["Luck28Last8Cents"] = Luck28Last0Cents8;
            xml.dss["Luck28Last9Cents"] = Luck28Last0Cents9;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


    }

    //设置每个号码的最大下注金额
    private void BuyAmount()
    {
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置


        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;<a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">游戏配置</a>&gt;号码最大限额");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "")
        {
            // builder.Append(Out.Tab("</div>", ""));
            strText = "0最大限额:";
            strName = "Luck28Buy0";
            strType = "text";
            strValu = ub.GetSub("Luck28Buy0", xmlPath);
            strEmpt = "false";
            for (int i = 1; i < 28; i++)
            {
                strText = strText + "," + i + "最大限额:";
                strName = strName + ",Luck28Buy" + i;
                strType = strType + ",text";
                strValu = strValu + "'" + ub.GetSub("Luck28Buy" + i, xmlPath);
                strEmpt = strEmpt + ",false";
            }
            strText = strText + ",,";
            strName = strName + ",info,act";
            strType = strType + ",hidden,hidden";
            strValu = strValu + "'ok'buyamount";
            strEmpt = strEmpt + ",false,false";

            strIdea = "/";


            strOthe = "确定编辑,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append("<br/>跳到这里");

            string[] canbuy = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            string[] save = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            for (int i = 0; i < 28; i++)
            {
                canbuy[i] = Utils.GetRequest("Luck28Buy" + i, "post", 1, "", "");
            }
            for (int i = 0; i < 28; i++)
            {
                save[i] = Utils.GetRequest("Luck28Buy" + i, "post", 1, "", "");
                xml.dss["Luck28Buy" + i] = canbuy[i];  //修改可下注金额
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置成功", "设置成功..", Utils.getPage("luck28.aspx"), "1");

        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=baseset") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            if (ManageId != 1 && ManageId != 2)
            {
                Utils.Error("权限不足", "");
            }
        }
        else if (Utils.GetTopDomain().Contains("kb288.net"))
        {
            if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
            {
                Utils.Error("权限不足", "");
            }
        }
        else
        {
            if (ManageId != 1 && ManageId != 9)
            {
                Utils.Error("权限不足", "");
            }
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除第" + id + "期";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定第" + id + "期记录吗.删除同时将会删除该期的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Lucklist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            //记录日志
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/删除幸运28期数:" + id + "");
            }

            new BCW.BLL.Game.Lucklist().Delete(id);
            new BCW.BLL.Game.Luckpay().Delete("LuckId=" + id + "");
            Utils.Success("删除第" + id + "期", "删除第" + id + "期成功..", Utils.getPage("luck28.aspx"), "1");
        }
    }

    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            if (ManageId != 1 && ManageId != 2)
            {
                Utils.Error("权限不足", "");
            }
        }
        else if (Utils.GetTopDomain().Contains("kb288.net"))
        {
            if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
            {
                Utils.Error("权限不足", "");
            }
        }
        else
        {
            if (ManageId != 1 && ManageId != 9)
            {
                Utils.Error("权限不足", "");
            }
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置幸运28游戏吗，重置后，将重新从第一期开始，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //  new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_Lucklist");
            //    new BCW.Data.SqlUp().ClearTable("tb_Lucklist");
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_Luckpay");
            //    new BCW.Data.SqlUp().ClearTable("tb_Luckpay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("luck28.aspx"), "1");
        }
    }

    //排行榜
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-6]$", "1"));
        string sTime = Utils.GetRequest("sTime", "all", 1, "", "");
        string oTime = Utils.GetRequest("oTime", "all", 1, "", "");
        Master.Title = "幸运28排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "BuyCents>0";
        string[] pageValUrl = { "act", "backurl", "ptype", "sTime", "oTime" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (sTime != "")
        {
            strWhere += "and AddTime>='" + sTime + "'and AddTime<'" + oTime + "'";
        }
        // 开始读取列表
        //string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //if (Utils.ToSChinese(ac) == "马上查询")
        //{
        //    DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        //    DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        //    string _where1 = string.Empty;
        //    string _where2 = string.Empty;
        //    _where2 = "and AddTime>='" + searchday1 + "'and AddTime<'" + searchday2 + "'";
        //    strWhere += _where2;
        //}
        // 开始读取列表
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>(" + n.UsID + ")赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
        if (sTime != "")
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("TOP用户ID:");
            int i = 0;
            string topid = string.Empty;
            foreach (BCW.Model.Game.Luckpay n in listLuckpay)
            {
                if (i < 10)
                {
                    if (topid == "" || string.IsNullOrEmpty(topid))
                    {
                        topid = topid + n.UsID;
                    }
                    else
                    {
                        topid = topid + "#" + n.UsID;
                    }
                }
                i++;
            }
            builder.Append("<b>" + topid + "</b>");
            string strText1 = ",,";
            string strName1 = "topus,act";
            string strType1 = "hidden,hidden";
            string strValu1 = topid + "'topgive";
            string strEmpt1 = "false,false";
            string strIdea1 = "/";
            string strOthe1 = "TOP10奖励发放,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            //builder.Append("<br/><a href=\"" + Utils.getPage("luck28.aspx?act=") + "\">TOP10奖励发放</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string strText = "开始日期：,结束日期：,";
        string strName = "sTime,oTime,act";
        string strType = "date,date,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddMonths(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'top";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "马上查询,luck28.aspx,post,1,red";

        if (sTime == "")
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("排行榜奖励提示：<br/>");
            builder.Append("如需发放奖励，请按日期查询.");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

    }
    private void topgive()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top") + "\">排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
        string topus = Utils.GetRequest("topus", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string[] usid = topus.Split('#');
        if (usid.Length < 10)
        {
            // Utils.Error("当页少于10人，无法发放！", "");
        }
        if (info == "")
        {
            string strText1 = "";
            string strName1 = "";
            string strType1 = "";
            string strValu1 = "";
            string strEmpt1 = "";
            for (int i = 0; i < usid.Length; i++)
            {
                strText1 += "top" + (i + 1) + ":" + usid[i] + ",";
                strName1 += "top" + (i + 1) + ",";
                strType1 += "text,";
                strValu1 += "0'";
                strEmpt1 += "false,";

            }
            strText1 += ",,";
            strName1 += "topus,act,info";
            strType1 += "hidden,hidden,hidden";
            strValu1 += topus + "'topgive'ok";
            strEmpt1 += "false,false,false";
            string strIdea1 = "/";
            string strOthe1 = "确定发放,luck28.aspx,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        else
        {
            long[] Top = new long[10];
            //   long top1 = Convert.ToInt64(Utils.GetRequest("top1", "post", 1, "", ""));
            for (int i = 0; i < usid.Length; i++)
            {
                Top[i] = Convert.ToInt64(Utils.GetRequest("top" + (i + 1), "post", 1, "", ""));
                if (Top[i] != 0)
                {
                    new BCW.BLL.User().UpdateiGold(int.Parse(usid[i]), Top[i], "二八排行榜奖励");
                    string strLog = "您在游戏幸运二八上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/luck28.aspx]进入《幸运二八》[/url]";
                    new BCW.BLL.Guest().Add(0, int.Parse(usid[i]), new BCW.BLL.User().GetUsName(int.Parse(usid[i])), strLog);
                    string mename = new BCW.BLL.User().GetUsName(int.Parse(usid[i]));
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + int.Parse(usid[i]) + "]" + mename + "[/url]在[url=/bbs/game/luck28.aspx]《幸运二八》[/url]上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz");
                    new BCW.BLL.Action().Add(1001, 0, int.Parse(usid[i]), "", wText);
                }
            }
            Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("luck28.aspx?act=top"), "1");
            //      Utils.Error("'"+ usid.Length,"");
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx") + "\">二八</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("luck28.aspx?act=top") + "\">排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
