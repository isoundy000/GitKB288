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
using System.Collections.Generic;
using BCW.Common;
using System.Text.RegularExpressions;

public partial class Manage_game_xk3 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/xinkuai3.xml";
    protected string GameName = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//游戏名字
    protected string klb = ub.GetSub("klb", "/Controls/xinkuai3_TRIAL_GAME.xml");//快乐币
    protected string XK3ROBOTID = (ub.GetSub("XK3ROBOTID", "/Controls/xinkuai3.xml"));//机器人ID
    protected string XtestID = (ub.GetSub("XtestID", "/Controls/xinkuai3.xml"));//试玩ID
    protected string xmlPath_SWB = "/Controls/xinkuai3_TRIAL_GAME.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    ub xml = new ub();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            //Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "xk3.aspx?ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ck_jl":
                ck_jlPage();//查看重开奖记录
                break;
            case "view":
                ViewPage();//显示详细的投注信息
                break;
            case "del":
                DelPage();//删除一条投注记录
                break;
            case "del_sj":
                del_sjPage();//删除一条开奖数据(刷新机获取的数据)
                break;
            case "ck_sj":
                ck_sjPage();//重开奖
                break;
            case "reset":
                ResetPage();//重置新快3
                break;
            case "stat":
                StatPage();//赢利分析
                break;
            case "peizhi":
                PeizhiPage();//配置管理
                break;
            case "weihu":
                WeihuPage();//游戏维护
                break;
            case "back":
                BackPage();//返赢返负
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "backsave3":
            case "backsave5":
                BackSavePage2(act);
                break;
            case "paihang":
                PaihangPage();//用户排行
                break;
            case "fenxi":
                FenxiPage();//用户购买情况和获奖数据情况
                break;
            case "add":
                AddPage();//手动添加开奖
                break;
            case "Top_add":
                Top_addPage();//紧急添加开奖数据
                break;
            case "ReWard":
                ReWard();//排行榜奖励发放--界面
                break;
            case "ReWardCase":
                ReWardCase();//排行榜奖励发放--操作
                break;
            case "ck":
                ckPage();//单期重开
                break;
            case "xg":
                xgPage();//单期修改
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "" + GameName + "_后台管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("" + GameName + "");
        builder.Append(Out.Tab("</div>", ""));

        string searchday = (Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", ""));
        string strText = "输入开奖日期：格式（20160214）/,";
        string strName = "inputdate";
        string strType = "num";
        string strValu = searchday;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "搜开奖记录,xk3.aspx?act=analyze,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<h style=\"color:blue\">" + GameName + "开奖信息：</h>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo2", xmlPath));
        string strWhere = string.Empty;
        if (searchday == "")
        {
            strWhere = "";
        }
        else
        {
            strWhere = "convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(searchday, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        }
        string[] pageValUrl = { "act", "inputdate", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Internet_Data> listXK3 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Datas(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Internet_Data n in listXK3)
            {
                if (k % 2 == 0)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                }
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                BCW.XinKuai3.Model.XK3_Bet model_get = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet_hounum(n.Lottery_issue);
                //" + ((pageIndex - 1) * pageSize + k) + "&nbsp;.&nbsp;
                builder.Append("" + n.Lottery_issue + "期");
                if (n.Lottery_num != "")
                {
                    builder.Append("<b>.[" + n.Lottery_num + "].</b>[" + string.Format("{0:T}", n.Lottery_time) + "].");
                    //builder.Append("&nbsp;&nbsp;|&nbsp;&nbsp;(时间：" + n.Lottery_time + ")");

                    if (model_get.aa > 0)
                    {
                        //builder.Append("&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + Utils.getUrl("xk3.aspx?act=view&amp;id=" + n.Lottery_issue + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>");
                        builder.Append("(<h style=\"color:red\"><a href=\"" + Utils.getUrl("xk3.aspx?act=view&amp;id=" + n.Lottery_issue + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model_get.aa + "</a></h>人投注)");
                    }
                    else
                    {
                        builder.Append("(" + model_get.aa + "注)");
                    }
                }
                else
                {
                    builder.Append("|<a href=\"" + Utils.getUrl("xk3.aspx?act=add&amp;id=" + n.Lottery_issue + "") + "\">开奖</a>");
                }
                if (Convert.ToInt32(ub.GetSub("xk3Status", xmlPath)) == 1)//维护
                {
                    builder.Append("|<a href=\"" + Utils.getUrl("xk3.aspx?act=del_sj&amp;id=" + n.ID + "") + "\">[删]</a>");
                    builder.Append("|<a href=\"" + Utils.getUrl("xk3.aspx?act=ck_sj&amp;id=" + n.ID + "") + "\">[重开]</a>");
                }
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

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【系统管理】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi") + "\">投注记录</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=paihang") + "\">用户排行</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=stat") + "\">赢利分析</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=Top_add") + "\">人工开奖</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【系统操作】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=peizhi") + "\">游戏配置</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=weihu") + "\">游戏维护</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=back") + "\">返赢返负</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //删除一条开奖数据(刷新机获取的数据)
    private void del_sjPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
        Master.Title = "" + GameName + "_删除数据";
        string info = Utils.GetRequest("info", "all", 1, "", "");

        if (info != "")
        {
            int id2 = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
            new BCW.XinKuai3.BLL.XK3_Internet_Data().Delete(id2);
            Utils.Success("删除数据", "删除成功，正在返回..", Utils.getUrl("xk3.aspx"), "1");
        }
        else
        {
            BCW.XinKuai3.Model.XK3_Internet_Data aaa = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Data(id);
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;删除数据"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：确定要删除数据吗？（只删除开奖数据,对用户购买的数据没影响.）<br/>");
            try
            {
                builder.Append("数据内容：第" + aaa.Lottery_issue + "期.<br/>");
            }
            catch
            {
                Utils.Error("该ID或期号不存在.", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=ok&amp;act=del_sj&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //重开奖
    private void ck_sjPage()
    {
        Master.Title = "" + GameName + "_重开奖";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定继续")
        {
            string num1 = Utils.GetRequest("num1", "post", 2, @"^[1-9]\d{8,8}$", "填写开奖期号出错");//重开奖期号
            string num2 = Utils.GetRequest("num2", "post", 2, @"^[1-6][1-6][1-6]$", "填写开奖号码出错");//重开奖号码
            BCW.XinKuai3.Model.XK3_Internet_Data er = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(num1);
            string ck_old_num = string.Empty;//num2、ck_old_num
            int gid = er.ID;
            ck_old_num = er.Lottery_num;
            try
            {
                ck_old_num = er.Lottery_num;//旧的开奖号码
            }
            catch
            {
                ck_old_num = "";
            }
            if (ck_old_num == "")
                Utils.Error("该期号对应的开奖数据为空，不需要重开，请到<a href=\"" + Utils.getUrl("xk3.aspx?act=Top_add") + "\">紧急开奖</a>", "");

            int y = Convert.ToInt32(num2);
            int y1 = y / 100;
            int y2 = (y - y1 * 100) / 10;
            int y3 = (y - y1 * 100 - y2 * 10);
            int[] num3 = { y1, y2, y3 };
            //冒泡排序 从小到大
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    if (num3[j] < num3[i])
                    {
                        int temp = num3[i];
                        num3[i] = num3[j];
                        num3[j] = temp;
                    }
                }
            }
            string num22 = string.Empty;//新的开奖号码
            for (int w = 0; w < 3; w++)
            {
                if (w == 2)
                    num22 = num22 + num3[w];
                else
                    num22 = num22 + num3[w] + ",";
            }

            //游戏日志记录
            int ManageId = new BCW.User.Manage().IsManageLogin();
            //string[] p_pageArr = { "ac", "num1", "num2" };
            //BCW.User.GameLog.GameLogPage(10, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号对新快3第" + num1 + "期重新开奖:" + num22 + "(原开奖:" + ck_old_num + ")", gid);
            BCW.Model.Gamelog bb = new BCW.Model.Gamelog();
            bb.AddTime = DateTime.Now;
            bb.EnId = int.Parse(num1);
            bb.Content = "后台管理员" + ManageId + "号对新快3第" + num1 + "期重新开奖:" + num22 + "(原开奖:" + ck_old_num + ")";
            bb.Notes = "1";
            bb.Types = 10;
            new BCW.BLL.Gamelog().Add(bb);

            //更新获奖数据
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            if (num22 != "")
            {
                string[] str1 = num22.Split(',');
                string t1 = str1[0];
                string t2 = str1[1];
                string t3 = str1[2];
                //大小单双
                //1大2小、1单2双
                if (((t1 == t2) && (t1 == t3)))//大小双单通食
                {
                    model.DaXiao = "0";
                    model.DanShuang = "0";
                }
                else
                {
                    if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                    {
                        model.DaXiao = "2";//和值开出是4-10,即为小

                    }
                    else
                    {
                        model.DaXiao = "1";//和值开出是11-17,即为大

                    }
                    if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                    {
                        model.DanShuang = "1";//单数
                    }
                    else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                    {
                        model.DanShuang = "2";//双数
                    }
                }

                //和值
                string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
                model.Sum = sum1;
                //三同号通选+三同号单选
                if (((t1 == t2) && (t1 == t3)))
                {
                    model.Three_Same_All = "1";
                    model.Three_Same_Single = t1 + t2 + t3;
                    //model.Three_Continue_All = "1";
                }
                else
                {
                    model.Three_Same_All = "0";
                    model.Three_Same_Single = "0";
                    //model.Three_Continue_All = "0";
                }
                //三不同号
                if ((t1 != t2) && (t1 != t3) && (t2 != t3))
                {
                    model.Three_Same_Not = t1 + t2 + t3;
                }
                else
                {
                    model.Three_Same_Not = "0";
                }
                //三连号通选
                if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))
                {
                    //model.Three_Continue_All = t1 + t2 + t3;
                    model.Three_Continue_All = "1";
                }
                else
                {
                    model.Three_Continue_All = "0";
                }
                //二同号复选
                if ((t1 == t2) || (t2 == t3))
                {
                    model.Two_Same_All = t2 + t2;

                }
                else
                {
                    model.Two_Same_All = "0";

                }
                //二同号单选
                if ((t1 == t2) || (t2 == t3))
                {
                    model.Two_Same_Single = t1 + t2 + t3;
                }
                else if ((t1 == t2) && (t1 == t3))
                {
                    model.Two_Same_Single = "0";
                }
                else
                {
                    model.Two_Same_Single = "0";
                }
                //二不同号
                if ((t1 != t2) && (t2 != t3))
                {
                    model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
                }
                else if ((t1 == t2) && (t2 != t3))
                {
                    model.Two_dissame = t2 + t3;
                }
                else if ((t1 != t2) && (t2 == t3))
                {
                    model.Two_dissame = t1 + t2;
                }
                else
                {
                    model.Two_dissame = "0";
                }
                model.UpdateTime = DateTime.Now;
                model.Lottery_issue = num1;
                model.Lottery_num = num22;
                new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num3(model);
            }
            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "Lottery_issue=" + num1 + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //本地投注数据
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                    int Play_Way = int.Parse(ds.Tables[0].Rows[i]["Play_Way"].ToString());
                    string Sum = ds.Tables[0].Rows[i]["Sum"].ToString();
                    string Three_Same_All = ds.Tables[0].Rows[i]["Three_Same_All"].ToString();
                    string Three_Same_Single = ds.Tables[0].Rows[i]["Three_Same_Single"].ToString();
                    string Three_Same_Not = ds.Tables[0].Rows[i]["Three_Same_Not"].ToString();
                    string Three_Continue_All = ds.Tables[0].Rows[i]["Three_Continue_All"].ToString();
                    string Two_Same_All = ds.Tables[0].Rows[i]["Two_Same_All"].ToString();
                    string Two_Same_Single = ds.Tables[0].Rows[i]["Two_Same_Single"].ToString();
                    string Two_dissame = ds.Tables[0].Rows[i]["Two_dissame"].ToString();
                    string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                    string DanTuo = ds.Tables[0].Rows[i]["DanTuo"].ToString();
                    int Zhu = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());
                    int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                    long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                    long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                    long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                    string DaXiao = (ds.Tables[0].Rows[i]["DaXiao"].ToString());
                    string DanShuang = (ds.Tables[0].Rows[i]["DanShuang"].ToString());
                    float _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                    string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名

                    //中奖已领,先扣钱
                    if (State == 2)
                    {
                        long gold = 0;//个人酷币
                        long cMoney = 0;//差多少
                        long sMoney = 0;//实扣

                        gold = new BCW.BLL.User().GetGold(UsID);
                        if (GetMoney > gold)
                        {
                            cMoney = GetMoney - gold;
                            //sMoney = gold;
                            sMoney = GetMoney;
                        }
                        else
                        {
                            sMoney = GetMoney;
                        }
                        //重开奖的在本场没兑奖时就没显示在欠币日志，
                        //操作币并内线通知-sMoney
                        new BCW.BLL.User().UpdateiGold(UsID, mename, -sMoney, "新快3ID:" + ID + "期号:" + num1 + "重开奖:" + num22 + "(原开奖" + ck_old_num + ")[原" + OutType(Play_Way) + "下注" + PutGold + "" + ub.Get("SiteBz") + "中" + GetMoney + "" + ub.Get("SiteBz") + "]扣除已兑奖" + ub.Get("SiteBz") + "");
                        //发送内线
                        string strGuess = "新快3ID:" + ID + "期号:" + num1 + "重开奖，你欠下系统的" + GetMoney + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额，实扣" + sMoney + "" + ub.Get("SiteBz") + ".[br]如果您的" + ub.Get("SiteBz") + "不足，系统将您帐户冻结，直到成功扣除为止。[br]你购买的" + num1 + "期" + OutType(Play_Way) + "下注" + PutGold + "" + ub.Get("SiteBz") + "中" + GetMoney + "" + ub.Get("SiteBz") + "(原开奖" + ck_old_num + "|新开奖" + num22 + ")";
                        new BCW.BLL.Guest().Add(1, UsID, mename, strGuess);

                        //如果币不够扣则记录日志并冻结IsFreeze
                        if (cMoney > 0)
                        {
                            BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                            owe.Types = 1;
                            owe.UsID = UsID;
                            owe.UsName = mename;
                            owe.Content = "" + num1 + "期" + OutType(Play_Way) + "下注" + PutGold + "" + ub.Get("SiteBz") + "(原开奖" + ck_old_num + "|新开奖" + num22 + ")";
                            owe.OweCent = cMoney;
                            owe.BzType = 10;//新快3重开奖记录type的id
                            owe.EnId = ID;
                            owe.AddTime = DateTime.Now;
                            new BCW.BLL.Gameowe().Add(owe);
                            new BCW.BLL.User().UpdateIsFreeze(UsID, 1);
                        }

                        ////取消得到的排行
                        //BCW.XinKuai3.Model.XK3_Toplist model_2 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(UsID);
                        //long all_prices = model_2.WinGold - GetMoney;
                        //new BCW.XinKuai3.BLL.XK3_Toplist().Update_getgold(UsID, all_prices);//把赢的钱，根据meid减少对应的WinGold字段。
                    }
                    //中奖未领,只取消排行榜、内线
                    if (State == 1 && GetMoney > 0)
                    {
                        //BCW.XinKuai3.Model.XK3_Toplist model_2 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(UsID);
                        //long all_prices = model_2.WinGold - GetMoney;
                        //new BCW.XinKuai3.BLL.XK3_Toplist().Update_getgold(UsID, all_prices);//把赢的钱，根据meid减少对应的WinGold字段。
                        //发送内线
                        string strGuess = "新快3ID:" + ID + "期号:" + num1 + "重开奖，你还没领奖，已自动处理.你购买的" + num1 + "期" + OutType(Play_Way) + "下注" + PutGold + "" + ub.Get("SiteBz") + "(原开奖" + ck_old_num + "|新开奖" + num22 + ")";
                        new BCW.BLL.Guest().Add(1, UsID, mename, strGuess);
                    }
                    //根据ID更新数据状态
                    new BCW.XinKuai3.BLL.XK3_Bet().update_zd("GetMoney=0,State=0", "id=" + ID + "");
                }
            }
            Open_price(num1);//返彩
            Utils.Success("重开数据", "重开成功，正在返回..", Utils.getUrl("xk3.aspx"), "1");
        }
        else
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
            BCW.XinKuai3.Model.XK3_Internet_Data aaa = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Data(id);
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;重开奖"));
            string aa = string.Empty;
            try
            {
                aa = aaa.Lottery_issue;
            }
            catch
            {
                aa = "0";
            }
            if (int.Parse(aa) > 0)
            {
                strText = "重开期号：/,重开号码：/,,";
                strName = "num1,num2,act";
                strType = "num,num,hidden";
                strValu = "" + aa + "''ck_sj";
                strEmpt = "false,false,false";
                strIdea = "/";
                strOthe = "确定继续,xk3.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
                Utils.Error("该期号不存在,请重新选择.", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //=============赢利分析=======================
    private void StatPage()
    {
        Master.Title = "" + GameName + "_赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        //今天投入数+今天兑奖数
        long TodayTou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold+Cost", "DateDiff(dd,Input_Time,getdate())=0 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long TodayDui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "DateDiff(dd,Input_Time,getdate())=0 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //昨天投入数+昨天兑奖数
        long yesTou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold+Cost", "DateDiff(dd,Input_Time,getdate()-1)=0 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long yesDui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "DateDiff(dd,Input_Time,getdate()-1)=0 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        //本月投入数+本月兑奖数
        long MonthTou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold+Cost", "datediff(month,[Input_Time],getdate())=0 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long MonthDui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "datediff(month,[Input_Time],getdate())=0 and state>0 AND isRobot='0' AND State!='3' and UsID in (select ID from tb_User where IsSpier!=1)");
        //上月投入数+上月兑奖数
        long Month2Tou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold+Cost", "datediff(month,[Input_Time],getdate())=1 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long Month2Dui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "datediff(month,[Input_Time],getdate())=1 and state>0 AND isRobot='0' AND State!='3' and UsID in (select ID from tb_User where IsSpier!=1)");
        //今年投入+今年兑奖
        long yearTou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold+Cost", "datediff(YEAR,[Input_Time],getdate())=0 and state>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long yearDui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "datediff(YEAR,[Input_Time],getdate())=0 and state>0 AND isRobot='0' AND State!='3' and UsID in (select ID from tb_User where IsSpier!=1)");
        //总投入+总兑奖
        long allTou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold+Cost", "State>0 AND isRobot='0' and UsID in (select ID from tb_User where IsSpier!=1)");
        long allDui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "State>0 AND isRobot='0' AND State!='3' and UsID in (select ID from tb_User where IsSpier!=1)");


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
            //string searchday1 = string.Empty;
            //string searchday2 = string.Empty;
            //searchday1 = Utils.GetRequest("sTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            //searchday2 = Utils.GetRequest("oTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

            //if (searchday1 == "")
            //{
            //    searchday1 = DateTime.Now.ToString("yyyyMMdd");
            //}
            //if (searchday2 == "")
            //{
            //    searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            //}

            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            long dateTou = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("PutGold", "Input_Time>='" + searchday1 + "' and Input_Time<='" + searchday2 + "'AND isRobot='0'AND State!='3'");
            long dateDui = new BCW.XinKuai3.BLL.XK3_Bet().GetPrice("GetMoney", "Input_Time>='" + searchday1 + "' and Input_Time<='" + searchday2 + "'AND isRobot='0'AND State!='3'");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利" + (dateTou - dateDui) + "" + ub.Get("SiteBz") + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,xk3.aspx?act=stat,post,1,red";
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
            string strOthe = "马上查询,xk3.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========根据期号，显示详细的投注信息======
    private void ViewPage()
    {
        Master.Title = "" + GameName + "_投注情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;投注情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Lottery_issue = (Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.XinKuai3.Model.XK3_Bet model = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet_num(Lottery_issue);
        if (model == null)
        {
            Utils.Success("不存在投注记录", "不存在投注记录.正在跳转.", Utils.getPage("xk3.aspx"), "1");
        }
        BCW.XinKuai3.Model.XK3_Internet_Data model_num = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(Lottery_issue);
        if (model_num == null)
        {
            Utils.Error("该" + Lottery_issue + "期的开奖号码不存在", "");
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("查看方式：总下注|");
        else
            builder.Append("查看方式：<a href=\"" + Utils.getUrl("xk3.aspx?act=view&amp;id=" + Lottery_issue + "&amp;ptype=1") + "\">总下注|</a>");
        if (ptype == 2)
            builder.Append("中奖情况");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=view&amp;id=" + Lottery_issue + "&amp;ptype=2") + "\">中奖情况</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo2", xmlPath));
        string strWhere = string.Empty;
        if (ptype == 1)
        {
            strWhere += "Lottery_issue='" + Lottery_issue + "'";
        }
        else if (ptype == 2)
        {
            strWhere = "Lottery_issue='" + Lottery_issue + "' and state=2";
        }


        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bets(pageIndex, pageSize, strWhere, out recordCount);

        if (listXK3pay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + Lottery_issue + "期开出：" + model_num.Lottery_num + "；");

            builder.Append("共" + recordCount + "注");

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
                    Getnum = n.Three_Same_All;//三同号通选
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
                    Getnum = n.Three_Continue_All;//三连号通选
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>");
                builder.Append("" + OutType(n.Play_Way) + "{" + Getnum + "}");//DT.FormatDate(n.Input_Time, 1)
                float _odds = 1;
                DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "id=" + n.ID + "");                                                                                                                                                                                                       //根据id，得到odds赔率
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                    }
                }
                if (_odds > 1)
                {
                    builder.Append("赔率[<h style=\"color:red\">" + _odds + "</h>]");
                }
                builder.Append("每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注/共投" + n.PutGold + ub.Get("SiteBz") + ".标识ID:" + n.ID + ".[" + n.Input_Time + "]");
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
                    //builder.Append("<b style=\"color:blue\">共赢了" + n.GetMoney + " " + ub.Get("SiteBz") + "</b>.");
                }
                builder.Append(".<a href=\"" + Utils.getUrl("xk3.aspx?act=del&amp;id=" + n.ID + "&amp;a=2&amp;l=" + n.Lottery_issue + "") + "\">[退]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========显示标题的投注方式================
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

    //==========删除一条投注记录==================
    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int a = int.Parse(Utils.GetRequest("a", "all", 1, @"^[0-9]\d*$", "1"));
            int b = int.Parse(Utils.GetRequest("b", "all", 1, @"^[0-9]\d*$", "1"));
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (a == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;b=" + b + "&amp;a=" + a + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;id=" + id + "&amp;ptype=" + b + "&amp;a=" + a + "") + "\">先留着吧..</a>");
            }
            else
            {
                int l = int.Parse(Utils.GetRequest("l", "all", 2, @"^[1-9]\d*$", "期号错误"));
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;l=" + l + "&amp;a=" + a + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=view&amp;id=" + l + "") + "\">先留着吧..</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int id2 = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            int a2 = int.Parse(Utils.GetRequest("a", "all", 1, @"^[0-9]\d*$", "1"));
            int b2 = int.Parse(Utils.GetRequest("b", "all", 1, @"^[0-9]\d*$", "1"));
            if (!new BCW.XinKuai3.BLL.XK3_Bet().Exists(id2))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                //根据id查询-购买表
                BCW.XinKuai3.Model.XK3_Bet model = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(id2);
                int meid = model.UsID;//用户名
                string mename = new BCW.BLL.User().GetUsName(meid);//获得id对应的用户名
                int state_get = model.State;//用户购买情况
                //排行榜
                //BCW.XinKuai3.Model.XK3_Toplist aa = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(meid);
                //long price_put = aa.PutGold - model.PutGold;//排行榜-投入
                //long Price2 = aa.WinGold - model.GetMoney;//排行榜-所得
                long Price = 0;
                //如果未开奖，退回本金
                if (state_get == 0)
                {
                    Price = model.PutGold;
                    new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快三第" + model.Lottery_issue + "期未开奖的" + model.PutGold + "" + ub.Get("SiteBz") + "！");//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的新快三：第" + model.Lottery_issue + "期未开奖的" + model.PutGold + "" + ub.Get("SiteBz") + "！");
                }
                //未中奖或已中奖，退回本金-所得
                //else if ((state_get == 2) || (state_get == 1))
                else
                {
                    //Price = model.PutGold - model.GetMoney;//系统-(本金-所得)
                    long gold = 0;//个人酷币
                    long cMoney = 0;//差多少
                    long sMoney = 0;//实扣
                    string ui = string.Empty;
                    gold = new BCW.BLL.User().GetGold(model.UsID);//个人酷币
                    if (model.GetMoney > gold)
                    {
                        cMoney = model.GetMoney - gold + model.PutGold;
                        sMoney = model.GetMoney;
                    }
                    else
                    {
                        sMoney = model.GetMoney;
                    }

                    //如果币不够扣则记录日志并冻结IsFreeze
                    if (cMoney > 0)
                    {
                        BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                        owe.Types = 1;
                        owe.UsID = model.UsID;
                        owe.UsName = mename;
                        owe.Content = "" + model.Lottery_issue + "期" + OutType(model.Play_Way) + "下注" + model.PutGold + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
                        owe.OweCent = cMoney;
                        owe.BzType = 10;
                        owe.EnId = model.ID;
                        owe.AddTime = DateTime.Now;
                        new BCW.BLL.Gameowe().Add(owe);
                        new BCW.BLL.User().UpdateIsFreeze(model.UsID, 1);
                        ui = "实扣" + sMoney + ",还差" + (cMoney) + ",系统已自动将您帐户冻结.";
                    }
                    string oop = string.Empty;
                    if (model.GetMoney > 0)
                    {
                        oop = "并扣除所得的" + model.GetMoney + "。";
                    }
                    new BCW.BLL.User().UpdateiGold(model.UsID, model.PutGold - model.GetMoney, "无效购奖或非法操作，系统退回新快三第" + model.Lottery_issue + "期下注的" + model.PutGold + "" + ub.Get("SiteBz") + "." + oop + "" + ui);//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回新快三第" + model.Lottery_issue + "期下注的" + model.PutGold + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                }
                ////如果过期不兑奖，退回本金
                //else if (state_get == 3)
                //{
                //    Price = model.PutGold;
                //    new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");//减少系统总的酷币
                //    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");
                //}


                //new BCW.XinKuai3.BLL.XK3_Toplist().Update_getgold(meid, Price2);//减少排行榜所得酷币
                //new BCW.XinKuai3.BLL.XK3_Toplist().Update_gold(meid, price_put);//减少排行投入的酷币
                new BCW.XinKuai3.BLL.XK3_Bet().Delete(id2);

                if (a2 == 1)
                {
                    if (state_get == 0)
                        Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3.aspx?act=fenxi&amp;ptype=5"), "2");//未开奖
                    else if ((state_get == 1))
                        Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3.aspx?act=fenxi&amp;ptype=4"), "2");//不中奖
                    else if (state_get == 2)
                        Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3.aspx?act=fenxi&amp;ptype=2"), "2");//中奖
                    else if (state_get == 3)
                        Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3.aspx?act=fenxi&amp;ptype=3"), "2");//系统回收
                }
                else
                {
                    int l2 = int.Parse(Utils.GetRequest("l", "all", 2, @"^[1-9]\d*$", "期号错误"));
                    Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3.aspx?act=view&amp;id=" + l2 + ""), "2");
                }
            }
        }
    }

    //==========重置新快3=========================
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "11")
        {
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_XK3_Internet_Data");
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_XK3_Bet");
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_XK3_Toplist");
            Utils.Success("重置游戏", "重置[所有数据]成功..", Utils.getUrl("xk3.aspx?act=reset"), "2");
        }
        else if (info == "1")
        {
            Master.Title = "重置所有表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置所有表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset&amp;info=11") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "22")
        {
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_XK3_Internet_Data");
            Utils.Success("重置游戏", "重置[开奖数据]成功..", Utils.getUrl("xk3.aspx?act=reset"), "2");
        }
        else if (info == "2")
        {
            Master.Title = "重置开奖数据表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置开奖数据表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset&amp;info=22") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "33")
        {
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_XK3_Bet");
            Utils.Success("重置游戏", "重置[投注数据]成功..", Utils.getUrl("xk3.aspx?act=reset"), "2");
        }
        else if (info == "3")
        {
            Master.Title = "重置投注数据表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置投注数据表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset&amp;info=33") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "44")
        {
            new BCW.XinKuai3.BLL.XK3_Bet().ClearTable("tb_XK3_Toplist");
            Utils.Success("重置游戏", "重置[排行榜数据]成功..", Utils.getUrl("xk3.aspx?act=reset"), "2");
        }
        else if (info == "4")
        {
            Master.Title = "重置排行榜数据表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置排行榜数据表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset&amp;info=44") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            Master.Title = "" + GameName + "_重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=1&amp;act=reset") + "\">[<b>一键全部重置</b>]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=2&amp;act=reset") + "\">[单独重置开奖表]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=3&amp;act=reset") + "\">[单独重置投注数据]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?info=4&amp;act=reset") + "\">[单独重置排行榜]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<h style=\"color:red\">注意：重置后，数据无法恢复。</h><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //==========新快3系统配置=====================
    private void PeizhiPage()
    {
        Master.Title = "" + GameName + "_游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string xmlPath = "/Controls/xinkuai3.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "游戏名称限1-20字内");
                string XinKuai3top1 = Utils.GetRequest("XinKuai3top1", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");//游戏头部标题
                string Logo = Utils.GetRequest("logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Sec = Utils.GetRequest("xSec", "post", 2, @"^[0-9]\d*$", "秒数填写出错");//等待几秒开奖
                string SiteListNo = Utils.GetRequest("SiteListNo", "post", 2, @"^[0-9]\d*$", "前台分页数填写出错");
                string SiteListNo2 = Utils.GetRequest("SiteListNo2", "post", 2, @"^[0-9]\d*$", "后台分页数填写出错");
                string SmallPay = Utils.GetRequest("xSmallPay", "post", 2, @"^[0-9]\d*$", "最小下注" + ub.Get("SiteBz") + "填写错误");
                string BigPay = Utils.GetRequest("xBigPay", "post", 2, @"^[0-9]\d*$", "最大下注" + ub.Get("SiteBz") + "填写错误");
                string Price = Utils.GetRequest("XK3Price", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + ub.Get("SiteBz") + "填写错误");
                string Maxnum = Utils.GetRequest("Maxnum", "post", 2, @"^[0-9]\d*$", "大小单双最大差值" + ub.Get("SiteBz") + "填写错误");
                string Expir = Utils.GetRequest("xExpir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string OnTime = Utils.GetRequest("xOnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string date = Utils.GetRequest("Xdate", "post", 2, @"^[0-9]\d*$", "系统回收" + ub.Get("SiteBz") + "填写错误");
                string Foot = Utils.GetRequest("XinKuai3Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string shouxu = Utils.GetRequest("shouxu", "post", 2, @"^[0-9]\d*$", "手续费填写错误");
                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }
                xml.dss["XinKuai3Name"] = Name;
                xml.dss["XinKuai3top1"] = XinKuai3top1;
                xml.dss["logo"] = Logo;
                xml.dss["xSec"] = Sec;
                xml.dss["SiteListNo"] = SiteListNo;
                xml.dss["SiteListNo2"] = SiteListNo2;
                xml.dss["xSmallPay"] = SmallPay;
                xml.dss["xBigPay"] = BigPay;
                xml.dss["Maxnum"] = Maxnum;
                xml.dss["XK3Price"] = Price;
                xml.dss["xExpir"] = Expir;
                xml.dss["Xdate"] = date;
                xml.dss["xOnTime"] = OnTime;
                xml.dss["XinKuai3Foot"] = Foot;
                xml.dss["shouxu"] = shouxu;
            }
            else
            {
                string XinKuai3Sum2 = Utils.GetRequest("XinKuai3Sum2", "post", 2, @"^[0-9]\d*$", "和值4、17赔率错误");
                string XSum1 = Utils.GetRequest("XSum1", "post", 2, @"^[0-9]\d*$", "和值5、16赔率错误");
                string XSum2 = Utils.GetRequest("XSum2", "post", 2, @"^[0-9]\d*$", "和值6、15赔率错误");
                string XSum3 = Utils.GetRequest("XSum3", "post", 2, @"^[0-9]\d*$", "和值7、14赔率错误");
                string XSum4 = Utils.GetRequest("XSum4", "post", 2, @"^[0-9]\d*$", "和值8、13赔率错误");
                string XSum5 = Utils.GetRequest("XSum5", "post", 2, @"^[0-9]\d*$", "和值9、12赔率错误");
                string XinKuai3Sum1 = Utils.GetRequest("XinKuai3Sum1", "post", 2, @"^[0-9]\d*$", "和值10、11赔率错误");
                string XinKuai3Three_Same_All = Utils.GetRequest("XinKuai3Three_Same_All", "post", 2, @"^[0-9]\d*$", "三同号通选赔率错误");
                string XinKuai3Three_Same_Single = Utils.GetRequest("XinKuai3Three_Same_Single", "post", 2, @"^[0-9]\d*$", "三同号单选赔率错误");
                string XinKuai3Three_Same_Not = Utils.GetRequest("XinKuai3Three_Same_Not", "post", 2, @"^[0-9]\d*$", "三不同号赔率错误");
                string XinKuai3Three_Continue_All = Utils.GetRequest("XinKuai3Three_Continue_All", "post", 2, @"^[0-9]\d*$", "三连号通选赔率错误");
                string XinKuai3Two_Same_All = Utils.GetRequest("XinKuai3Two_Same_All", "post", 2, @"^[0-9]\d*$", "二同号通选赔率错误");
                string XinKuai3Two_Same_Single = Utils.GetRequest("XinKuai3Two_Same_Single", "post", 2, @"^[0-9]\d*$", "二同号单选赔率错误");
                string XinKuai3Two_dissame = Utils.GetRequest("XinKuai3Two_dissame", "post", 2, @"^[0-9]\d*$", "二不同号赔率错误");
                string Xda = Utils.GetRequest("Xda", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "大的赔率错误");
                string Xxiao = Utils.GetRequest("Xxiao", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "小的赔率错误");
                string Xdan = Utils.GetRequest("Xdan", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "单的赔率错误");
                string Xshuang = Utils.GetRequest("Xshuang", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "双的赔率错误");
                string Xfudong = Utils.GetRequest("Xfudong", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "浮动的赔率错误");
                string Xda1 = Utils.GetRequest("Xda1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时大的赔率错误");
                string Xxiao1 = Utils.GetRequest("Xxiao1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时小的赔率错误");
                string Xdan1 = Utils.GetRequest("Xdan1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时单的赔率错误");
                string Xshuang1 = Utils.GetRequest("Xshuang1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时双的赔率错误");
                string Xoverpeilv = Utils.GetRequest("Xoverpeilv", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "限定连开最大的赔率错误");
                string cfnum = Utils.GetRequest("cfnum", "post", 2, @"^[0-9]\d*$", "X期连开才开起浮动");

                xml.dss["XinKuai3Sum2"] = XinKuai3Sum2;
                xml.dss["XSum1"] = XSum1;
                xml.dss["XSum2"] = XSum2;
                xml.dss["XSum3"] = XSum3;
                xml.dss["XSum4"] = XSum4;
                xml.dss["XSum5"] = XSum5;
                xml.dss["XinKuai3Sum1"] = XinKuai3Sum1;
                xml.dss["XinKuai3Three_Same_All"] = XinKuai3Three_Same_All;
                xml.dss["XinKuai3Three_Same_Single"] = XinKuai3Three_Same_Single;
                xml.dss["XinKuai3Three_Same_Not"] = XinKuai3Three_Same_Not;
                xml.dss["XinKuai3Three_Continue_All"] = XinKuai3Three_Continue_All;
                xml.dss["XinKuai3Two_Same_All"] = XinKuai3Two_Same_All;
                xml.dss["XinKuai3Two_Same_Single"] = XinKuai3Two_Same_Single;
                xml.dss["XinKuai3Two_dissame"] = XinKuai3Two_dissame;
                xml.dss["Xda"] = Xda;
                xml.dss["Xxiao"] = Xxiao;
                xml.dss["Xdan"] = Xdan;
                xml.dss["Xshuang"] = Xshuang;
                xml.dss["Xfudong"] = Xfudong;
                xml.dss["Xda1"] = Xda1;
                xml.dss["Xxiao1"] = Xxiao1;
                xml.dss["Xdan1"] = Xdan1;
                xml.dss["Xshuang1"] = Xshuang1;
                xml.dss["Xoverpeilv"] = Xoverpeilv;
                xml.dss["cfnum"] = cfnum;
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("新快3设置", "设置成功，正在返回..", Utils.getUrl("xk3.aspx?act=peizhi&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("新快3设置|");
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=peizhi&amp;ptype=1") + "\">赔率设置</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=peizhi&amp;ptype=0") + "\">新快3设置</a>");
                builder.Append("|赔率设置");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称：/,收取手续费：(千个点，0为不收)/,头部Ubb：/,游戏Logo路径：/,等待几秒开奖：/,前台分页行数：/,后台分页行数：/,最小下注" + ub.Get("SiteBz") + "：/,最大下注" + ub.Get("SiteBz") + "：/,每期每ID限购多少" + ub.Get("SiteBz") + "：（0为不限投）/,大小单双最大" + ub.Get("SiteBz") + "差值：（0为没有差值）/,下注防刷(秒)：/,游戏开奖时间：/,超过几天系统自动回收" + ub.Get("SiteBz") + "：/,底部Ubb：/,";
                string strName = "Name,shouxu,XinKuai3top1,Logo,xSec,SiteListNo,SiteListNo2,xSmallPay,xBigPay,XK3Price,Maxnum,xExpir,xOnTime,Xdate,XinKuai3Foot,backurl";
                string strType = "text,num,textarea,text,num,num,num,num,num,num,num,num,text,text,textarea,hidden";
                string strValu = "" + xml.dss["XinKuai3Name"] + "'" + xml.dss["shouxu"] + "'" + xml.dss["XinKuai3top1"] + "'" + xml.dss["logo"] + "'" + xml.dss["xSec"] + "'" + xml.dss["SiteListNo"] + "'" + xml.dss["SiteListNo2"] + "'" + xml.dss["xSmallPay"] + "'" + xml.dss["xBigPay"] + "'" + xml.dss["XK3Price"] + "'" + xml.dss["Maxnum"] + "'" + xml.dss["xExpir"] + "'" + xml.dss["xOnTime"] + "'" + xml.dss["Xdate"] + "'" + xml.dss["XinKuai3Foot"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,true,false,false,false,false,false,false,false,false,false,true,true,false";
                string strIdea = "/";
                string strOthe = "确定修改,xk3.aspx?act=peizhi,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />游戏开放时间填写格式为:09:26-22:26.<br/>大小单双最大差值不能大于最大下注" + ub.Get("SiteBz") + "。");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "和值4、17：,和值5、16：,和值6、15：,和值7、14：,和值8、13：,和值9、12：,和值10、11：,三同号通选：,三同号单选：,三不同号：,三连号通选：,二同号通选：,二同号单选：,二不同号：,X期连开才开起浮动：(系统不能设置超过6)/,大：,小：,单：,双：,实时大：,实时小：,实时单：,实时双：,大小单双浮动赔率：/,大小单双限定连开最大的赔率：/,,,";
                string strName = "XinKuai3Sum2,XSum1,XSum2,XSum3,XSum4,XSum5,XinKuai3Sum1,XinKuai3Three_Same_All,XinKuai3Three_Same_Single,XinKuai3Three_Same_Not,XinKuai3Three_Continue_All,XinKuai3Two_Same_All,XinKuai3Two_Same_Single,XinKuai3Two_dissame,cfnum,Xda,Xxiao,Xdan,Xshuang,Xda1,Xxiao1,Xdan1,Xshuang1,Xfudong,Xoverpeilv,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["XinKuai3Sum2"] + "'" + xml.dss["XSum1"] + "'" + xml.dss["XSum2"] + "'" + xml.dss["XSum3"] + "'" + xml.dss["XSum4"] + "'" + xml.dss["XSum5"] + "'" + xml.dss["XinKuai3Sum1"] + "'" + xml.dss["XinKuai3Three_Same_All"] + "'" + xml.dss["XinKuai3Three_Same_Single"] + "'" + xml.dss["XinKuai3Three_Same_Not"] + "'" + xml.dss["XinKuai3Three_Continue_All"] + "'" + xml.dss["XinKuai3Two_Same_All"] + "'" + xml.dss["XinKuai3Two_Same_Single"] + "'" + xml.dss["XinKuai3Two_dissame"] + "'" + xml.dss["cfnum"] + "'" + xml.dss["Xda"] + "'" + xml.dss["Xxiao"] + "'" + xml.dss["Xdan"] + "'" + xml.dss["Xshuang"] + "'" + xml.dss["Xda1"] + "'" + xml.dss["Xxiao1"] + "'" + xml.dss["Xdan1"] + "'" + xml.dss["Xshuang1"] + "'" + xml.dss["Xfudong"] + "'" + xml.dss["Xoverpeilv"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,xk3.aspx?act=peizhi,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />实时的大小单双赔率没有特殊情况不建议手动修改。");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("xk3.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //==========游戏维护==========================
    private void WeihuPage()
    {
        Master.Title = "" + GameName + "_游戏维护";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/xinkuai3.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "0");
            string XtestID = Utils.GetRequest("XtestID", "all", 1, @"^[^\^]{1,2000}$", "");
            string RoBotID = Utils.GetRequest("RoBotID", "post", 1, @"^[^\^]{1,2000}$", "");
            string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["XIsBot"].ToString());
            string RoBotCost = Utils.GetRequest("RoBotCost", "post", 1, @"^[^\^]{1,2000}$", "");
            string RoBotCount = Utils.GetRequest("RoBotCount", "post", 1, @"^[0-9]\d*$", xml.dss["XK3ROBOTBUY"].ToString());
            //

            xml.dss["XK3ROBOTID"] = RoBotID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["XIsBot"] = IsBot;
            xml.dss["XK3ROBOTCOST"] = RoBotCost.Replace("\r\n", "").Replace(" ", "");
            xml.dss["XK3ROBOTBUY"] = RoBotCount;
            xml.dss["XtestID"] = XtestID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["xk3Status"] = Status;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);


            //Utils.Error("" + xml.dss["XK3ROBOTID"] + "", "");
            Utils.Success("" + GameName + "_游戏维护", "修改成功，正在返回..", Utils.getUrl("xk3.aspx?act=weihu&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;游戏维护"));
            string strText = "游戏状态:/,机器人状态:/,测试ID:/,机器人ID:/,机器人投注金额设置:/,机器人每期下注数量:（0为不限制）/,";
            string strName = "Status,IsBot,XtestID,RoBotID,RoBotCost,RoBotCount,backurl";
            string strType = "select,select,big,big,big,text,hidden";
            string strValu = "" + xml.dss["xk3Status"] + "'" + xml.dss["XIsBot"].ToString() + "'" + xml.dss["XtestID"] + "'" + xml.dss["XK3ROBOTID"].ToString() + "'" + xml.dss["XK3ROBOTCOST"].ToString() + "'" + xml.dss["XK3ROBOTBUY"].ToString() + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|正常|1|维护|2|内测,0|关闭|1|开启,false,true,true,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,xk3.aspx?act=weihu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        string aa = xml.dss["XtestID"].ToString();
        string aa1 = xml.dss["XK3ROBOTID"].ToString();
        string[] sNum = Regex.Split(aa, "#");
        string[] sNum1 = Regex.Split(aa1, "#");

        string[] name = aa.Split('#');
        string[] name1 = aa1.Split('#');

        if (aa.Length > 0)
        {
            builder.Append("测试ID：《" + sNum.Length + "》个<br/>");
            for (int n = 0; n < name.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name[n] + ",");
                }
            }
        }

        if (aa1.Length > 0)
        {
            builder.Append("<br/>机器人：《" + sNum1.Length + "》个<br/>");
            for (int n = 0; n < name1.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name1[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name1[n] + ",");
                }
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=reset") + "\">[游戏重置]</a><br/>");
        builder.Append("温馨提示：<br/>1、多个试玩ID和机器人请用#分隔.");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========返赢返负===界面===================
    private void BackPage()
    {
        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知:,";
        string strName = "sTime,oTime,iTar,iPrice,text,act";
        string strType = "date,date,num,num,text,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave3";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,xk3.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知:,";
        strName = "sTime,oTime,iTar,iPrice,text,act";
        strType = "date,date,num,num,text,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave5";
        strEmpt = "false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,xk3.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackSavePage2(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "返币填写错误"));
        string text = Utils.GetRequest("text", "all", 2, @"^[^\^]{1,5000}$", "消息填写太多了");

        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");


        if (act == "backsave3")
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=back") + "\">返赢返负</a>&gt;返赢确认");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("返赢时间段：<b>" + sTime + "</b>到<b>" + oTime + "</b><br />");
            builder.Append("返赢千分比：" + iTar + "<br />");
            builder.Append("至少赢：" + iPrice + "币返还.<br />");
            builder.Append("消息通知：" + text + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知:,";
            string strName = "sTime,oTime,iTar,iPrice,text,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + text + "'backsave";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返赢,xk3.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (act == "backsave5")
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=back") + "\">返赢返负</a>&gt;返负确认");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("返负时间段：<b>" + sTime + "</b>到<b>" + oTime + "</b><br />");
            builder.Append("返负千分比：" + iTar + "<br />");
            builder.Append("至少负：" + iPrice + "币返还.<br />");
            builder.Append("消息通知：" + text + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知:,";
            string strName = "sTime,oTime,iTar,iPrice,text,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + text + "'backsave2";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返负,xk3.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx?act=back") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========返赢返负===执行===================
    private void BackSavePage(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "返币填写错误"));
        string text = Utils.GetRequest("text", "all", 2, @"^[^\^]{1,5000}$", "消息填写太多了");

        if (act == "backsave")
        {
            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("UsID,sum(GetMoney-PutGold) as WinCents", "Input_Time>='" + sTime + "'and Input_Time<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "新快三返赢");
                    //发内线
                    string strLog = text + "返还了：" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx]进入新快3[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("xk3.aspx?act=back"), "1");
        }
        else if (act == "backsave2")
        {
            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("UsID,sum(GetMoney-PutGold) as WinCents", "Input_Time>='" + sTime + "'and Input_Time<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "新快三返负");
                    //发内线
                    string strLog = text + "返还了：" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx]进入新快3[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("xk3.aspx?act=back"), "1");
        }
    }

    //==========用户排行==========================
    private void PaihangPage()
    {
        Master.Title = "" + GameName + "_用户排行";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;用户排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate2", "all", 1, DT.RegexTime, DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")));//"2014-01-01 00:00:00"DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate2", "all", 1, DT.RegexTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));//"2115-01-01 00:00:00"

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">净赚排行" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=paihang&amp;ptype=1&amp;id=" + ptype + "") + "\">净赚排行</a>" + "|");

        if (ptype == 2)
            builder.Append("<h style=\"color:red\">赚币排行" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=paihang&amp;ptype=2&amp;id=" + ptype + "") + "\">赚币排行</a>" + "|");
        if (ptype == 3)
            builder.Append("<h style=\"color:red\">游戏狂人" + "</h>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=paihang&amp;ptype=3&amp;id=" + ptype + "") + "\">游戏狂人</a>" + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //20160515 邵广林 排行榜去除测试id和机器人排行
        //string[] name = XK3ROBOTID.Split('#');//机器人
        //string a = "";
        //for (int n = 0; n < name.Length; n++)
        //{
        //    if (n == name.Length - 1)
        //    {
        //        a = a + name[n];
        //    }
        //    else
        //    {
        //        a = a + name[n] + ",";
        //    }
        //}
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
            if (Utils.GetDomain().Contains("kb288"))
            {
                if (b != "")
                {
                    strWhere = "UsID not IN (" + b + ") and Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and state>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";//and UsID not IN (" + a + "," + b + ") 
                }
                else
                {
                    strWhere = "Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and state>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";
                }
            }
            else
                strWhere = "Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and state>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";

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
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        else if (ptype == 2)
        {
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

            //if (startstate.ToString() == "2014-01-01 00:00:00")
            //{
            //    strWhere = "GetMoney>0  GROUP BY UsID ORDER BY Sum(GetMoney) DESC";//and UsID not IN (" + a + "," + b + ")
            //}
            //else
            if (Utils.GetDomain().Contains("kb288"))
            {
                if (b != "")
                {
                    strWhere = "GetMoney>0 and UsID not IN (" + b + ") and Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and state>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";//and UsID not IN (" + a + "," + b + ") 
                }
                else
                {
                    strWhere = "GetMoney>0 and Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and state>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";//and UsID not IN (" + a + "," + b + ") 
                }
            }
            else
                strWhere = "GetMoney>0 and Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and state>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";

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
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        else if (ptype == 3)
        {
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

            //if (startstate.ToString() == "2014-01-01 00:00:00")
            //{
            //    strWhere = "GROUP BY UsID ORDER BY count(UsID) DESC";//UsID not IN (" + a + "," + b + ") 
            //}
            //else
            if (Utils.GetDomain().Contains("kb288"))
            {
                if (b != "")
                {
                    strWhere = "Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' and UsID not IN (" + b + ") GROUP BY UsID ORDER BY count(UsID) DESC";// and UsID not IN (" + a + "," + b + ")
                }
                else
                {
                    strWhere = "Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' GROUP BY UsID ORDER BY count(UsID) DESC";// and UsID not IN (" + a + "," + b + ")
                }
            }
            else
                strWhere = "Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' GROUP BY UsID ORDER BY count(UsID) DESC";

            strWhere2 = "UsID,count(UsID) as bb";//TOP(100) 

            strWhere3 = "UsID,count(UsID) AS'bb' into #bang3";

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
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>共玩<h style=\"color:red\">" + usmoney + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        string strText = "开始日期：/,结束日期：/,";
        string strName = "startstate2,endstate2,backurl";
        string strType = "date,date,hidden";
        string strValu = string.Empty;
        //if (Utils.ToSChinese(ac) != "马上查询")
        {
            //strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'" + Utils.getPage(0) + "";
        }
        //else
        {
            strValu = "" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Utils.getPage(0) + "";
        }
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "马上查询,xk3.aspx?act=paihang&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));


        if (Utils.ToSChinese(ac) != "马上查询")
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("排行榜奖励提示：<br/>");
            builder.Append("如需发放奖励，请按日期查询.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append(wdy + " 的用户ID分别是：" + rewardid);
            builder.Append(Out.Tab("</div>", ""));
            string strText2 = ",,,,";
            string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden,hidden";
            string strValu2 = DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = wdy + "奖励发放,xk3.aspx?act=ReWard&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a><br/>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //==========用户购买情况和获奖数据情况========
    private void FenxiPage()
    {
        Master.Title = "" + GameName + "_下注情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;下注情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[1-2]$", "1"));
        int isRobot = int.Parse(Utils.GetRequest("isRobot", "all", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo2", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "ptype2", "isRobot", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">记录" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=1&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">记录</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">中奖" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">中奖</a>" + "|");
        }
        if (ptype == 3)
        {
            builder.Append("<h style=\"color:red\">过期" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=3&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">过期</a>" + "|");
        }
        if (ptype == 4)
        {
            builder.Append("<h style=\"color:red\">未中" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=4&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">未中</a>" + "|");
        }
        if (ptype == 5)
        {
            builder.Append("<h style=\"color:red\">未开" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=5&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">未开</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (ptype2 == 1)
            {
                builder.Append("<h style=\"color:red\">已兑奖" + "</h>" + "|");
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "&amp;ptype2=2") + "\">未兑奖</a>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "&amp;ptype2=1") + "\">已兑奖</a>" + "|");
                builder.Append("<h style=\"color:red\">未兑奖" + "</h>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        if (ptype == 1)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "isRobot=" + isRobot + "";
        }
        else if (ptype == 2)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    if (ptype2 == 1)
                    {
                        strWhere = "State=2 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                    }
                    else
                        strWhere = "State=1 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + " and GetMoney>0";
                }
                else
                {
                    if (ptype2 == 1)
                    {
                        strWhere = "State=2 and UsID=" + uid + " and isRobot=" + isRobot + "";
                    }
                    else
                    {
                        strWhere = "State=1 and UsID=" + uid + " and isRobot=" + isRobot + " and GetMoney>0";
                    }
                }

            }
            else if (qihaos > 0)
            {
                if (ptype2 == 1)
                {
                    strWhere = "State=2 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=1 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + " and GetMoney>0";
            }
            else
            {
                if (ptype2 == 1)
                {
                    strWhere = "State=2 and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=1 and isRobot=" + isRobot + " and GetMoney>0";
            }
        }
        else if (ptype == 3)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=3 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=3 and UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=3 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "State=3 and isRobot=" + isRobot + "";
        }
        else if (ptype == 4)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=1 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=1 and UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=1 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "State=1 and isRobot=" + isRobot + "";
        }
        else if (ptype == 5)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=0 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=0 and UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=0 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "State=0 and isRobot=" + isRobot + "";
        }

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bets(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3pay.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet n in listXK3pay)
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
                string Gettou = string.Empty;
                if (n.Play_Way == 1)
                {
                    Gettou = n.Sum;
                }
                if (n.Play_Way == 2)
                {
                    Gettou = n.Three_Same_All;
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
                    Gettou = n.Three_Continue_All;
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
                float _odds = 1;
                int _isRobot = 0;
                DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "id=" + n.ID + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _isRobot = Convert.ToInt32(ds.Tables[0].Rows[i]["isRobot"].ToString());
                    }
                }
                //获取id对应的用户名
                string mename = new BCW.BLL.User().GetUsName(n.UsID);
                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>." + (n.Lottery_issue) + "期");
                BCW.XinKuai3.Model.XK3_Internet_Data model_num1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(n.Lottery_issue);
                if (model_num1.Lottery_num != "")
                {
                    builder.Append("&lt;" + model_num1.Lottery_num + ">");
                }
                builder.Append("[" + OutType(n.Play_Way) + "]为{" + Gettou + "}");//DT.FormatDate(n.Input_Time, 1)
                //根据id，得到odds赔率
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                    }
                }
                if (_odds > 1)
                {
                    builder.Append("赔率[<h style=\"color:red\">" + _odds + "</h>]");
                }
                builder.Append("每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注/共投" + n.PutGold + ub.Get("SiteBz") + ".标识ID:" + n.ID + ".[" + n.Input_Time + "].");
                if (n.GetMoney > 0)
                {
                    if (n.State == 2)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(已领奖)");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                    else if (n.State == 3)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(过期未领奖)");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                    else
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(未领奖)");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }


                }
                if (n.GetMoney == 0)
                {
                    if (n.State == 0)
                    {
                        builder.Append("<h style=\"color:blue\">(未开奖)</h>");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                    else if (n.State == 1)
                    {
                        builder.Append("<h style=\"color:green\">(不中奖)</h>");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                }
                builder.Append(".<a href=\"" + Utils.getUrl("xk3.aspx?act=del&amp;id=" + n.ID + "&amp;a=1&amp;b=" + ptype + "") + "\">[退]</a>");
                if (Convert.ToInt32(ub.GetSub("xk3Status", xmlPath)) == 1)//维护
                {
                    builder.Append(".<a href=\"" + Utils.getUrl("xk3.aspx?act=xg&amp;id=" + n.ID + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;ptype=" + ptype + "") + "\">[改]</a>");
                }
                if (ptype == 5)
                    builder.Append(".<a href=\"" + Utils.getUrl("xk3.aspx?act=ck&amp;id=" + n.ID + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;ptype=" + ptype + "&amp;tt=" + isRobot + "") + "\">[重开]</a>");
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        string strText = "输入用户ID(可为空):/,输入彩票期号(可为空):/,机器人投注显示：/,";
        string strName = "uid,qihaos,isRobot,backurl";
        string strType = "num,num,select,hidden";
        string strValu = string.Empty;
        if (uid == 0)
        {
            if (qihaos == 0)
            {
                strValu = "''" + isRobot + "'" + Utils.getPage(0) + "";
            }
            else
            {
                strValu = "'" + qihaos + "'" + isRobot + "'" + Utils.getPage(0) + "";
            }
        }
        else
        {
            if (qihaos == 0)
            {
                strValu = "" + uid + "''" + isRobot + "'" + Utils.getPage(0) + "";
            }
            else
            {
                strValu = "" + uid + "'" + qihaos + "'" + isRobot + "'" + Utils.getPage(0) + "";
            }
        }
        string strEmpt = "true,true,0|关|1|开,false";
        string strIdea = "/";
        string strOthe = "搜一搜,xk3.aspx?act=fenxi&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<hr/>");
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========手动添加开奖数字==================
    private void AddPage()
    {
        Master.Title = "" + GameName + "_人工开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;人工开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Lottery_issue = (Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("需要手动开奖的期号为：" + Lottery_issue + "");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            string num = Utils.GetRequest("num", "post", 2, @"^[1-6][1-6][1-6]$", "填写出错");

            //加分隔符,
            string a = num[0] + "," + num[1] + "," + num[2];

            string[] str1 = a.Split(',');
            string t1 = str1[0];
            string t2 = str1[1];
            string t3 = str1[2];

            string _where2 = string.Empty;
            _where2 = "'" + Lottery_issue + "' Order by id desc";

            BCW.XinKuai3.Model.XK3_Internet_Data model_getTime = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all_num2(_where2);

            string datee = DateTime.ParseExact((("20" + Lottery_issue.Substring(0, 6)) + " 09:28:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
            //根据期号算时间,
            if (int.Parse(GetLastStr(Lottery_issue, 2)) < 10)//如果少于10
            {
                model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(Lottery_issue, 1))));
            }
            else
            {
                model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(Lottery_issue, 2))));
            }
            model.Lottery_issue = Lottery_issue;
            model.Lottery_num = a;
            //大小单双
            if (((t1 == t2) && (t1 == t3)))//大小双单通食
            {
                model.DaXiao = "0";
                model.DanShuang = "0";
            }
            else
            {
                //1大2小、1单2双
                if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                {
                    model.DaXiao = "2";//和值开出是4-10,即为小

                }
                else
                {
                    model.DaXiao = "1";//和值开出是11-17,即为大

                }
                if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                {
                    model.DanShuang = "1";//单数
                }
                else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                {
                    model.DanShuang = "2";//双数
                }
            }

            //和值
            string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
            model.Sum = sum1;
            //三同号通选+三同号单选
            if (((t1 == t2) && (t1 == t3)))
            {
                model.Three_Same_All = "1";
                model.Three_Same_Single = t1 + t2 + t3;
                //model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Same_All = "0";
                model.Three_Same_Single = "0";
                //model.Three_Continue_All = "0";
            }
            //三不同号
            if ((t1 != t2) && (t1 != t3) && (t2 != t3))
            {
                model.Three_Same_Not = t1 + t2 + t3;
            }
            else
            {
                model.Three_Same_Not = "0";
            }
            //三连号通选
            if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))//2=1+1、2=3-1
            {
                //model.Three_Continue_All = t1 + t2 + t3;
                model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Continue_All = "0";
            }
            //二同号复选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_All = t2 + t2;

            }
            else
            {
                model.Two_Same_All = "0";

            }
            //二同号单选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_Single = t1 + t2 + t3;
            }
            else if ((t1 == t2) && (t1 == t3))
            {
                model.Two_Same_Single = "0";
            }
            else
            {
                model.Two_Same_Single = "0";
            }
            //二不同号
            if ((t1 != t2) && (t2 != t3))
            {
                model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
            }
            else if ((t1 == t2) && (t2 != t3))
            {
                model.Two_dissame = t2 + t3;
            }
            else if ((t1 != t2) && (t2 == t3))
            {
                model.Two_dissame = t1 + t2;
            }
            else
            {
                model.Two_dissame = "0";
            }
            model.UpdateTime = DateTime.Now;
            new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num2(model);
            Open_price("");//返奖
            //change_peilv();//根据最近几期的大小单双开奖情况，实时变动赔率。
            Utils.Success("" + GameName + "_人工开奖", "人工开奖成功，正在返回..", Utils.getUrl("xk3.aspx?backurl=" + Utils.getPage(0) + ""), "1");
            //builder.Append("================" + a);
        }
        else
        {
            string strText = "输入开奖号码：/,";
            string strName = "num," + Utils.getPage(0) + "";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确定修改,xk3.aspx?act=add&amp;id=" + Lottery_issue + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("温馨提示：请在输入框输入需要开奖的号码。如：123<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========开始返彩==========================
    public void Open_price(string dd)
    {
        //检查数据库最后一期开奖号码
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.Model.XK3_Internet_Data();
        if (dd != "")
        {
            model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all_num2(dd);
        }
        else
        {
            model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast2();//网络开奖数据
        }
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;
        //Response.Write("===============数据库最后一期的开奖数据===============" + qihao + "：" + kai + "<br/>");
        if (kai != "")//如果开奖号码为空，则不返奖
        {
            int sum = Int32.Parse(model_1.Sum);
            int Odds = 0;//和值的倍数

            //检查投注表是否存在没开奖数据
            if (new BCW.XinKuai3.BLL.XK3_Bet().Exists_num(qihao))
            {
                //Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;==============有人下注，已自动兑奖==========<b></b><br /><br />");
                DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "State=0 and Lottery_issue=" + qihao + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //本地投注数据
                        int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                        int Play_Way = int.Parse(ds.Tables[0].Rows[i]["Play_Way"].ToString());
                        string Sum = ds.Tables[0].Rows[i]["Sum"].ToString();
                        string Three_Same_All = ds.Tables[0].Rows[i]["Three_Same_All"].ToString();
                        string Three_Same_Single = ds.Tables[0].Rows[i]["Three_Same_Single"].ToString();
                        string Three_Same_Not = ds.Tables[0].Rows[i]["Three_Same_Not"].ToString();
                        string Three_Continue_All = ds.Tables[0].Rows[i]["Three_Continue_All"].ToString();
                        string Two_Same_All = ds.Tables[0].Rows[i]["Two_Same_All"].ToString();
                        string Two_Same_Single = ds.Tables[0].Rows[i]["Two_Same_Single"].ToString();
                        string Two_dissame = ds.Tables[0].Rows[i]["Two_dissame"].ToString();
                        string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                        string DanTuo = ds.Tables[0].Rows[i]["DanTuo"].ToString();
                        int Zhu = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());
                        int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                        long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                        long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                        long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                        string DaXiao = (ds.Tables[0].Rows[i]["DaXiao"].ToString());
                        string DanShuang = (ds.Tables[0].Rows[i]["DanShuang"].ToString());
                        float _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                        string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名

                        if (Play_Way == 1)//和值
                        {
                            if ((sum == 4) || (sum == 17))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum2", xmlPath));
                            }
                            else if ((sum == 5) || (sum == 16))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum1", xmlPath));
                            }
                            else if ((sum == 6) || (sum == 15))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum2", xmlPath));
                            }
                            else if ((sum == 7) || (sum == 14))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum3", xmlPath));
                            }
                            else if ((sum == 8) || (sum == 13))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum4", xmlPath));
                            }
                            else if ((sum == 9) || (sum == 12))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum5", xmlPath));
                            }
                            else if ((sum == 10) || (sum == 11))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum1", xmlPath));
                            }

                            int a = 0;
                            string[] bd_sum = Sum.Split(',');
                            for (int bd = 0; bd < bd_sum.Length; bd++)
                            {
                                if (bd_sum[bd] == model_1.Sum)
                                {
                                    a++;
                                }
                            }
                            if (a == 1)
                            //if (Sum.Contains(model_1.Sum))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息

                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 2)//三同号通选
                        {
                            if (model_1.Three_Same_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 3)//三同号单选
                        {
                            if (Three_Same_Single.Contains(model_1.Three_Same_Single))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Single", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 4)//三不同号
                        {
                            if (Three_Same_Not.Contains(model_1.Three_Same_Not))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Not", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 5)//三连号通选
                        {
                            if (model_1.Three_Continue_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Continue_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 6)//二同号复选
                        {
                            if (Two_Same_All.Contains(model_1.Two_Same_All))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 7)//二同号单选
                        {
                            string[] a1 = Two_Same_Single.Split(',');//分割购买的数据 {221,223,224,551,553,554}
                            int e = 0;
                            for (int p = 0; p < a1.Length; p++)
                            {
                                //各自赋值给y
                                int y = Convert.ToInt32(a1[p]);
                                int y1 = y / 100;
                                int y2 = (y - y1 * 100) / 10;
                                int y3 = (y - y1 * 100 - y2 * 10);
                                int[] num3 = { y1, y2, y3 };

                                //冒泡排序 从小到大
                                for (int t = 0; t < 3; t++)
                                {
                                    for (int j = t + 1; j < 3; j++)
                                    {
                                        if (num3[j] < num3[t])
                                        {
                                            int temp = num3[t];
                                            num3[t] = num3[j];
                                            num3[j] = temp;
                                        }
                                    }
                                }
                                string num22 = string.Empty;
                                for (int w = 0; w < 3; w++)//遍历数组显示结果
                                {
                                    num22 = num22 + num3[w];
                                }
                                if (num22.Contains(model_1.Two_Same_Single))
                                {
                                    e++;
                                }
                                if (e > 0)
                                {
                                    break;
                                }
                            }
                            //if (Two_Same_Single.Contains(model_1.Two_Same_Single))
                            if (e > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_Single", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 8)//二不同号
                        {
                            string[] tt = (model_1.Two_dissame).Split(',');//开奖的数据
                            string[] bb = Two_dissame.Split(',');
                            int a = tt.Length;
                            int b = bb.Length;
                            int j = 0;

                            for (int y = 0; y <= tt.Length - 1; y++)
                            {
                                for (int ii = 0; ii <= bb.Length - 1; ii++)
                                {
                                    if (bb[ii].Contains(tt[y]))
                                    {
                                        j++;
                                    }
                                }
                            }
                            if (j > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_dissame", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds * j);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 9)//大小
                        {
                            if (DaXiao == (model_1.DaXiao))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 10)//单双
                        {
                            if (DanShuang == (model_1.DanShuang))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                            }
                        }
                        new BCW.XinKuai3.BLL.XK3_Bet().UpdateState(ID, 1);
                    }
                }

            }
        }
        else
        {
            //此次应该通知管理员：第几期有期号，而没开奖号码。
            Response.Write("<br/><br/><b>《《《请注意：第" + qihao + "期因没有开奖号码，返奖失败。。。》》》</b><br/><br/>");
        }
    }

    //根据最近几期的大小单双开奖情况，实时变动赔率。
    public void change_peilv()
    {
        //BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast2();//网络开奖数据--20160328
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast3();
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;

        ub xml = new ub();
        string xmlPath_update = "/Controls/xinkuai3.xml";
        Application.Remove(xmlPath_update);//清缓存
        xml.ReloadSub(xmlPath_update); //加载配置

        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_1.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                d1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList("TOP 1 *", "DaXiao!='' ORDER BY Lottery_time DESC");//id
                d2 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList2("TOP 1 *", "id!='' ORDER BY Lottery_time ASC ");//id!='' ORDER BY id ASC 

                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents1 = Convert.ToString(d1.Tables[0].Rows[i]["DaXiao"]);//最后一期的大小
                        Cents2 = Convert.ToString(d1.Tables[0].Rows[i]["DanShuang"]);//最后一期的单双
                    }
                    catch
                    {
                        Cents1 = "";
                        Cents2 = "";
                    }
                }
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = Convert.ToString(d2.Tables[0].Rows[i]["DaXiao"]);//倒数第2期的大小
                        Cents4 = Convert.ToString(d2.Tables[0].Rows[i]["DanShuang"]);//倒数第2期的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                    }
                }
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Xda1"] = xml.dss["Xda"];
                    xml.dss["Xxiao1"] = xml.dss["Xxiao"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "2" && Cents3 == "2")//如果连续2期开小
                    {
                        xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率增加
                        xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "1" && Cents3 == "1")//大
                    {
                        xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                        xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率增加
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Xdan1"] = xml.dss["Xdan"];
                    xml.dss["Xshuang1"] = xml.dss["Xshuang"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    if (Cents2 == "1" && Cents4 == "1")//如果连续2期开单
                    {
                        xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率增加
                        xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "2" && Cents4 == "2")//双
                    {
                        xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                        xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率增加
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Xda1"] = xml.dss["Xda"];
            xml.dss["Xxiao1"] = xml.dss["Xxiao"];
            xml.dss["Xdan1"] = xml.dss["Xdan"];
            xml.dss["Xshuang1"] = xml.dss["Xshuang"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
    }

    //==========紧急添加开奖数据==================
    private void Top_addPage()
    {
        Master.Title = "" + GameName + "_紧急添加开奖数据";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;紧急添加开奖数据");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        string ss = string.Empty;

        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string num1 = Utils.GetRequest("num1", "post", 2, @"^[1-9]\d{8,8}$", "填写开奖期号出错");//开奖期号
            string num2 = Utils.GetRequest("num2", "post", 2, @"^[1-6][1-6][1-6]$", "填写开奖号码出错");//开奖号码
            string inputdate = Utils.GetRequest("num3", "post", 1, @"^[0-9]\d*$", DateTime.Now.ToString("yyyyMMdd"));//搜索的日期
            //string num22 = num2[0] + "," + num2[1] + "," + num2[2];//4,1,1

            int y = Convert.ToInt32(num2);
            int y1 = y / 100;
            int y2 = (y - y1 * 100) / 10;
            int y3 = (y - y1 * 100 - y2 * 10);
            int[] num3 = { y1, y2, y3 };

            //冒泡排序 从小到大
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    if (num3[j] < num3[i])
                    {
                        int temp = num3[i];
                        num3[i] = num3[j];
                        num3[j] = temp;
                    }
                }
            }

            string num22 = string.Empty;
            for (int w = 0; w < 3; w++)//遍历数组显示结果
            {
                if (w == 2)
                    num22 = num22 + num3[w];
                else
                    num22 = num22 + num3[w] + ",";
            }

            string _where = string.Empty;
            _where = "where Lottery_issue='" + num1 + "'";
            int tpye1 = 0;

            BCW.XinKuai3.Model.XK3_Internet_Data model_select = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            if (model_select.Lottery_issue == "0")
            {
                model.Lottery_issue = num1;
                model.Lottery_num = num22;

                string datee = DateTime.ParseExact((("20" + num1.Substring(0, 6)) + " 09:28:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                //根据期号算时间,
                if (int.Parse(GetLastStr(num1, 2)) < 10)//如果少于10
                {
                    model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(num1, 1))));
                    model.UpdateTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(num1, 1))));
                }
                else
                {
                    model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(num1, 2))));
                    model.UpdateTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(num1, 2))));
                }
                //model.Lottery_time = DateTime.Now;
                //model.UpdateTime = DateTime.Now;
                new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model);
                tpye1 = 1;
            }
            else if ((model_select.Lottery_issue != "0") && (model_select.Lottery_num == ""))
            {
                model.Lottery_issue = num1;
                model.Lottery_num = num22;
                new BCW.XinKuai3.BLL.XK3_Internet_Data().update_num2(model);
                tpye1 = 2;
            }
            //开奖
            string[] str1 = num22.Split(',');
            string t1 = str1[0];
            string t2 = str1[1];
            string t3 = str1[2];

            string _where2 = string.Empty;
            _where2 = "'" + num1 + "' Order by id desc";

            BCW.XinKuai3.Model.XK3_Internet_Data model_getTime = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all_num2(_where2);
            try
            {
                DateTime getTime = model_getTime.Lottery_time;
                model.Lottery_time = getTime;
            }
            catch
            {
                model.Lottery_time = DateTime.Now;
            }
            model.Lottery_issue = num1;
            model.Lottery_num = num22;
            //大小单双
            if (((t1 == t2) && (t1 == t3)))//大小双单通食
            {
                model.DaXiao = "0";
                model.DanShuang = "0";
            }
            else
            {
                //1大2小、1单2双
                if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                {
                    model.DaXiao = "2";//和值开出是4-10,即为小

                }
                else
                {
                    model.DaXiao = "1";//和值开出是11-17,即为大

                }
                if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                {
                    model.DanShuang = "1";//单数
                }
                else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                {
                    model.DanShuang = "2";//双数
                }
            }

            //和值
            string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
            model.Sum = sum1;
            //三同号通选+三同号单选
            if (((t1 == t2) && (t1 == t3)))
            {
                model.Three_Same_All = "1";
                model.Three_Same_Single = t1 + t2 + t3;
                //model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Same_All = "0";
                model.Three_Same_Single = "0";
                //model.Three_Continue_All = "0";
            }
            //三不同号
            if ((t1 != t2) && (t1 != t3) && (t2 != t3))
            {
                model.Three_Same_Not = t1 + t2 + t3;
            }
            else
            {
                model.Three_Same_Not = "0";
            }
            //三连号通选
            if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))//2=1+1、2=3-1
            {
                //model.Three_Continue_All = t1 + t2 + t3;
                model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Continue_All = "0";
            }
            //二同号复选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_All = t2 + t2;

            }
            else
            {
                model.Two_Same_All = "0";

            }
            //二同号单选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_Single = t1 + t2 + t3;
            }
            else if ((t1 == t2) && (t1 == t3))
            {
                model.Two_Same_Single = "0";
            }
            else
            {
                model.Two_Same_Single = "0";
            }
            //二不同号
            if ((t1 != t2) && (t2 != t3))
            {
                model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
            }
            else if ((t1 == t2) && (t2 != t3))
            {
                model.Two_dissame = t2 + t3;
            }
            else if ((t1 != t2) && (t2 == t3))
            {
                model.Two_dissame = t1 + t2;
            }
            else
            {
                model.Two_dissame = "0";
            }
            model.UpdateTime = DateTime.Now;
            new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num2(model);
            //change_peilv();//根据最近几期的大小单双开奖情况，实时变动赔率。
            Open_price("");//返彩
            if (tpye1 == 1)
            {
                Utils.Success("" + GameName + "_紧急添加开奖数据", "开奖数据《已添加》成功，正在返回..", Utils.getUrl("xk3.aspx?act=Top_add&amp;inputdate=" + inputdate + ""), "1");
            }
            else if (tpye1 == 2)
            {
                Utils.Success("" + GameName + "_紧急添加开奖数据", "开奖数据《已更新开奖号码》成功，正在返回..", Utils.getUrl("xk3.aspx?act=Top_add&amp;inputdate=" + inputdate + ""), "1");
            }
            else
            {
                Utils.Success("" + GameName + "_紧急添加开奖数据", "添加失败：不能对有开奖号码的期号进行修改，正在返回..", Utils.getUrl("xk3.aspx?act=Top_add&amp;inputdate=" + inputdate + ""), "2");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=ck_jl") + "\">[查看重开记录]</a>");
            builder.Append(Out.Tab("</div>", ""));

            string searchday = (Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd")));
            string strText = "输入需要开奖的日期：格式（20160214）/,";
            string strName = "inputdate";
            string strType = "num";
            string strValu = searchday;
            string strEmpt = "";
            string strIdea = "";
            string strOthe = "查询开奖记录,xk3.aspx?act=Top_add,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br/>"));
            for (int a = 1; a < 79; a++)
            {
                string b = GetLastStr(searchday, 6) + 0;
                if (a < 10)
                {
                    b = b + 0;
                    BCW.XinKuai3.Model.XK3_Internet_Data bh = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(b + a.ToString());
                    if (bh.Lottery_num != "")
                    {
                        builder.Append(b + a.ToString());
                        builder.Append(".<b>[" + bh.Lottery_num + "]</b>" + "<br/>");
                    }
                    else
                    {
                        builder.Append("<form id=\"form10\" method=\"post\" action=\"xk3.aspx\">");
                        builder.Append((b + a.ToString()));
                        builder.Append("<input type=\"hidden\" name=\"num1\" Value=\"" + (b + a.ToString()) + "\"/>");
                        builder.Append("<input type=\"text\" name=\"num2\" Value=\"\"/>");
                        builder.Append("<input type=\"hidden\" name=\"num3\" Value=\"" + searchday + "\"/>");
                        string VE = ConfigHelper.GetConfigString("VE");
                        string SID = ConfigHelper.GetConfigString("SID");
                        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"Top_add\"/>");
                        builder.Append("<input name=\"ac\" type=\"submit\" class=\"btn-blue\" value=\"确定添加\"/><br />");
                        builder.Append("</form>");
                    }
                }
                else
                {
                    BCW.XinKuai3.Model.XK3_Internet_Data bh = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(b + a.ToString());
                    if (bh.Lottery_num != "")
                    {
                        ss = b + a.ToString();
                        builder.Append(b + a.ToString());
                        builder.Append(".<b>[" + bh.Lottery_num + "]</b>" + "<br/>");
                    }
                    else
                    {
                        builder.Append("<form id=\"form10\" method=\"post\" autocomplete=\"off\" action=\"xk3.aspx\">");
                        builder.Append((b + a.ToString()));
                        builder.Append("<input type=\"hidden\" name=\"num1\" Value=\"" + (b + a.ToString()) + "\"/>");
                        builder.Append("<input type=\"text\" name=\"num2\" Value=\"\"/>");
                        builder.Append("<input type=\"hidden\" name=\"num3\" Value=\"" + searchday + "\"/>");
                        string VE = ConfigHelper.GetConfigString("VE");
                        string SID = ConfigHelper.GetConfigString("SID");
                        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"Top_add\"/>");
                        builder.Append("<input name=\"ac\" type=\"submit\" class=\"btn-blue\" value=\"确定添加\"/><br />");
                        builder.Append("</form>");
                    }
                }
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("<b class=\"text-red\">特定号码开奖：</b>");
            builder.Append(Out.Tab("</div>", ""));
            strText = "开奖期号：/,开奖号码：/,,";
            strName = "num1,num2,hid,act";
            strType = "num,num,hidden,hidden";
            strValu = "'''" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            strOthe = "确定添加,xk3.aspx?act=Top_add,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("温馨提示：请在输入框输入需要开奖的号码。如：123<br/>对于已有开奖号码的期号，不能对其修改，也不能删除。<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //查看单注重开奖
    private void ck_jlPage()
    {
        Master.Title = "" + GameName + "_重开奖情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=Top_add") + "\">紧急添加开奖数据</a>&gt;重开奖情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">重开记录" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=ck_jl&amp;ptype=1") + "\">重开记录</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">负币记录" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=ck_jl&amp;ptype=2") + "\">负币记录</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 2)
        {
            strWhere = "BzType=10";

            // 开始读取列表
            IList<BCW.Model.Gameowe> listXK3pay = new BCW.BLL.Gameowe().GetGameowes(pageIndex, pageSize, strWhere, out recordCount);
            if (listXK3pay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Gameowe n in listXK3pay)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>：" + Out.SysUBB(n.Content) + "[" + n.AddTime + "]");
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
        }
        else
        {
            strWhere = "Types=10";
            // 开始读取列表
            IList<BCW.Model.Gamelog> listXK3pay = new BCW.BLL.Gamelog().GetGamelogs(pageIndex, pageSize, strWhere, out recordCount);
            if (listXK3pay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Gamelog n in listXK3pay)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.Content) + "[" + n.AddTime + "]");
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
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=Top_add") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========排行榜奖励发放====================
    private void ReWard()
    {
        Master.Title = "" + GameName + "_奖励发放";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=paihang") + "\">用户排行</a>&gt;奖励发放");
        builder.Append(Out.Tab("</div>", "<br />"));


        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
        //string startstate = (Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20150101"));
        //string endstate = (Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20501231"));
        int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
        string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");


        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string wdy = "";
        if (pageIndex == 1)
            wdy = "TOP10";
        else
            wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
        builder.Append(Out.Tab("<div>", ""));
        switch (ptype)
        {
            case 1:
                builder.Append("《净赚排行》" + wdy + "奖励发放：");
                break;
            case 2:
                builder.Append("《赚币排行》" + wdy + "奖励发放：");
                break;
            case 3:
                builder.Append("《游戏狂人》" + wdy + "奖励发放：");
                break;
        }
        builder.Append(Out.Tab("</div>", ""));

        int mzj = (pageIndex - 1) * 10;
        string[] IdRe = rewardid.Split('#');
        try
        {
            string strText2 = ",,,,TOP" + (mzj + 1) + "：" + IdRe[0] + "&nbsp;&nbsp;,,TOP" + (mzj + 2) + "：" + IdRe[1] + "&nbsp;&nbsp;,,TOP" + (mzj + 3) + "：" + IdRe[2] + "&nbsp;&nbsp;,,TOP" + (mzj + 4) + "：" + IdRe[3] + "&nbsp;&nbsp;,,TOP" + (mzj + 5) + "：" + IdRe[4] + "&nbsp;&nbsp;,,TOP" + (mzj + 6) + "：" + IdRe[5] + "&nbsp;&nbsp;,,TOP" + (mzj + 7) + "：" + IdRe[6] + "&nbsp;&nbsp;,,TOP" + (mzj + 8) + "：" + IdRe[7] + "&nbsp;&nbsp;,,TOP" + (mzj + 9) + "：" + IdRe[8] + "&nbsp;&nbsp;,,TOP" + pageIndex * 10 + "：" + IdRe[9] + "&nbsp;&nbsp;,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "提交,xk3.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("当页少于10人，无法发放！");
            builder.Append(Out.Tab("</div>", ""));

        }
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("xk3.aspx?act=paihang") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========排行榜奖励发放--界面====================
    private void ReWardCase()
    {
        int[] IdRe = new int[11];
        long[] Top = new long[11];
        IdRe[1] = int.Parse(Utils.GetRequest("IdRe1", "post", 1, "", "10086"));
        IdRe[2] = int.Parse(Utils.GetRequest("IdRe2", "post", 1, "", "10086"));
        IdRe[3] = int.Parse(Utils.GetRequest("IdRe3", "post", 1, "", "10086"));
        IdRe[4] = int.Parse(Utils.GetRequest("IdRe4", "post", 1, "", "10086"));
        IdRe[5] = int.Parse(Utils.GetRequest("IdRe5", "post", 1, "", "10086"));
        IdRe[6] = int.Parse(Utils.GetRequest("IdRe6", "post", 1, "", "10086"));
        IdRe[7] = int.Parse(Utils.GetRequest("IdRe7", "post", 1, "", "10086"));
        IdRe[8] = int.Parse(Utils.GetRequest("IdRe8", "post", 1, "", "10086"));
        IdRe[9] = int.Parse(Utils.GetRequest("IdRe9", "post", 1, "", "10086"));
        IdRe[10] = int.Parse(Utils.GetRequest("IdRe10", "post", 1, "", "10086"));
        Top[1] = Convert.ToInt64(Utils.GetRequest("top1", "post", 1, "", ""));
        Top[2] = Convert.ToInt64(Utils.GetRequest("top2", "post", 1, "", ""));
        Top[3] = Convert.ToInt64(Utils.GetRequest("top3", "post", 1, "", ""));
        Top[4] = Convert.ToInt64(Utils.GetRequest("top4", "post", 1, "", ""));
        Top[5] = Convert.ToInt64(Utils.GetRequest("top5", "post", 1, "", ""));
        Top[6] = Convert.ToInt64(Utils.GetRequest("top6", "post", 1, "", ""));
        Top[7] = Convert.ToInt64(Utils.GetRequest("top7", "post", 1, "", ""));
        Top[8] = Convert.ToInt64(Utils.GetRequest("top8", "post", 1, "", ""));
        Top[9] = Convert.ToInt64(Utils.GetRequest("top9", "post", 1, "", ""));
        Top[10] = Convert.ToInt64(Utils.GetRequest("top10", "post", 1, "", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        //string startstate = (Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20150101"));
        //string endstate = (Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20501231"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));

        string wdy = "";
        switch (ptype)
        {
            case 1:
                wdy = "净赚排行榜";
                break;
            case 2:
                wdy = "赚币排行榜";
                break;
            case 3:
                wdy = "游戏狂人榜";
                break;
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定发放")
        {
            for (int i = 1; i <= 10; i++)
            {
                if (Top[i] != 0)
                {
                    new BCW.BLL.User().UpdateiGold(IdRe[i], Top[i], "新快三排行榜奖励");
                    //发内线
                    string strLog = "您在 " + startstate + " 至 " + endstate + " 里在游戏《新快3》" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx]进入《新快3》[/url]";
                    new BCW.BLL.Guest().Add(0, IdRe[i], new BCW.BLL.User().GetUsName(IdRe[i]), strLog);
                    //动态
                    string mename = new BCW.BLL.User().GetUsName(IdRe[i]);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + IdRe[i] + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]《新快3》[/url]" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz");
                    new BCW.BLL.Action().Add(1001, 0, IdRe[i], "", wText);
                }
            }
            Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("xk3.aspx?act=paihang"), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_奖励发放";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3.aspx?act=paihang") + "\">用户排行</a>&gt;奖励发放");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("正在发放《" + wdy + "》奖励：");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("时间从：" + startstate + "到" + endstate + "<br/>");
            for (int j = 1; j <= 10; j++)
            {
                if (j == 10)
                {
                    builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "" + ub.Get("SiteBz") + "]");
                }
                else
                {
                    builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "" + ub.Get("SiteBz") + "]<br/>");
                }
            }

            string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Top[1] + "'" + IdRe[1] + "'" + Top[2] + "'" + IdRe[2] + "'" + Top[3] + "'" + IdRe[3] + "'" + Top[4] + "'" + IdRe[4] + "'" + Top[5] + "'" + IdRe[5] + "'" + Top[6] + "'" + IdRe[6] + "'" + Top[7] + "'" + IdRe[7] + "'" + Top[8] + "'" + IdRe[8] + "'" + Top[9] + "'" + IdRe[9] + "'" + Top[10] + "'" + IdRe[10];
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "确定发放,xk3.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));

            builder.Append(Out.Tab("</div>", ""));


            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("xk3.aspx?act=paihang") + "\">&lt;&lt;再看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }


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

    //根据最近几期的大小单双开奖情况，实时变动赔率。========试玩
    public void change_peilv_SWB()
    {
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast3();//网络开奖数据
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;

        ub xml = new ub();
        string xmlPath_update2 = "/Controls/xinkuai3_TRIAL_GAME.xml";
        Application.Remove(xmlPath_update2);//清缓存
        xml.ReloadSub(xmlPath_update2); //加载配置

        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_1.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                d1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList("TOP 1 *", "DaXiao!='' ORDER BY Lottery_time DESC");
                d2 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList2("TOP 1 *", "id!='' ORDER BY Lottery_time ASC ");

                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents1 = Convert.ToString(d1.Tables[0].Rows[i]["DaXiao"]);//最后一期的大小
                        Cents2 = Convert.ToString(d1.Tables[0].Rows[i]["DanShuang"]);//最后一期的单双
                    }
                    catch
                    {
                        Cents1 = "";
                        Cents2 = "";
                    }
                }
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = Convert.ToString(d2.Tables[0].Rows[i]["DaXiao"]);//倒数第2期的大小
                        Cents4 = Convert.ToString(d2.Tables[0].Rows[i]["DanShuang"]);//倒数第2期的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                    }
                }
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Xda1"] = xml.dss["Xda"];
                    xml.dss["Xxiao1"] = xml.dss["Xxiao"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "2" && Cents3 == "2")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Xxiao1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率增加
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "1" && Cents3 == "1")//大
                    {
                        if (Convert.ToDouble(xml.dss["Xda1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Xdan1"] = xml.dss["Xdan"];
                    xml.dss["Xshuang1"] = xml.dss["Xshuang"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    if (Cents2 == "1" && Cents4 == "1")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Xdan1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率增加
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "2" && Cents4 == "2")//双
                    {
                        if (Convert.ToDouble(xml.dss["Xshuang1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Xda1"] = xml.dss["Xda"];
            xml.dss["Xxiao1"] = xml.dss["Xxiao"];
            xml.dss["Xdan1"] = xml.dss["Xdan"];
            xml.dss["Xshuang1"] = xml.dss["Xshuang"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
    }

    //单注重开
    private void ckPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));
        int isRobot2 = int.Parse(Utils.GetRequest("tt", "all", 1, @"^[0-1]$", "0"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.XinKuai3.Model.XK3_Bet uu = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(id);
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum("'" + uu.Lottery_issue + "'");
        if (model.Lottery_num != "")//有开奖号码,直接帮兑奖
        {
            if (info == "1")
            {
                int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
                int uid1 = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
                int qihaos1 = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));
                int isRobot3 = int.Parse(Utils.GetRequest("tt", "all", 1, @"^[0-1]$", "0"));

                BCW.XinKuai3.Model.XK3_Bet aq = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(id);
                BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all_num2("'" + aq.Lottery_issue + "'");
                int sum = Int32.Parse(model_1.Sum);
                int Odds = 0;//和值的倍数
                DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "id=" + id + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //本地投注数据
                        int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                        int Play_Way = int.Parse(ds.Tables[0].Rows[i]["Play_Way"].ToString());
                        string Sum = ds.Tables[0].Rows[i]["Sum"].ToString();
                        string Three_Same_All = ds.Tables[0].Rows[i]["Three_Same_All"].ToString();
                        string Three_Same_Single = ds.Tables[0].Rows[i]["Three_Same_Single"].ToString();
                        string Three_Same_Not = ds.Tables[0].Rows[i]["Three_Same_Not"].ToString();
                        string Three_Continue_All = ds.Tables[0].Rows[i]["Three_Continue_All"].ToString();
                        string Two_Same_All = ds.Tables[0].Rows[i]["Two_Same_All"].ToString();
                        string Two_Same_Single = ds.Tables[0].Rows[i]["Two_Same_Single"].ToString();
                        string Two_dissame = ds.Tables[0].Rows[i]["Two_dissame"].ToString();
                        string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                        string DanTuo = ds.Tables[0].Rows[i]["DanTuo"].ToString();
                        int Zhu = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());
                        int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                        long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                        long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                        long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                        string DaXiao = (ds.Tables[0].Rows[i]["DaXiao"].ToString());
                        string DanShuang = (ds.Tables[0].Rows[i]["DanShuang"].ToString());
                        float _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                        string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名
                        int isRobot = int.Parse(ds.Tables[0].Rows[i]["isRobot"].ToString());//机器人

                        if (Play_Way == 1)//和值
                        {
                            if ((sum == 4) || (sum == 17))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum2", xmlPath));
                            }
                            else if ((sum == 5) || (sum == 16))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum1", xmlPath));
                            }
                            else if ((sum == 6) || (sum == 15))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum2", xmlPath));
                            }
                            else if ((sum == 7) || (sum == 14))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum3", xmlPath));
                            }
                            else if ((sum == 8) || (sum == 13))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum4", xmlPath));
                            }
                            else if ((sum == 9) || (sum == 12))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum5", xmlPath));
                            }
                            else if ((sum == 10) || (sum == 11))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum1", xmlPath));
                            }
                            int a = 0;
                            string[] bd_sum = Sum.Split(',');
                            for (int bd = 0; bd < bd_sum.Length; bd++)
                            {
                                if (bd_sum[bd] == model_1.Sum)
                                {
                                    a++;
                                }
                            }
                            if (a == 1)
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                    {
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    }
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 2)//三同号通选
                        {
                            if (model_1.Three_Same_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 3)//三同号单选
                        {
                            if (Three_Same_Single.Contains(model_1.Three_Same_Single))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Single", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 4)//三不同号
                        {
                            if (Three_Same_Not.Contains(model_1.Three_Same_Not))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Not", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 5)//三连号通选
                        {
                            if (model_1.Three_Continue_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Continue_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 6)//二同号复选
                        {
                            if (Two_Same_All.Contains(model_1.Two_Same_All))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 7)//二同号单选
                        {
                            string[] a1 = Two_Same_Single.Split(',');//分割购买的数据 {221,223,224,551,553,554}
                            int e = 0;
                            for (int p = 0; p < a1.Length; p++)
                            {
                                //各自赋值给y
                                int y = Convert.ToInt32(a1[p]);
                                int y1 = y / 100;
                                int y2 = (y - y1 * 100) / 10;
                                int y3 = (y - y1 * 100 - y2 * 10);
                                int[] num3 = { y1, y2, y3 };

                                //冒泡排序 从小到大
                                for (int t = 0; t < 3; t++)
                                {
                                    for (int j = t + 1; j < 3; j++)
                                    {
                                        if (num3[j] < num3[t])
                                        {
                                            int temp = num3[t];
                                            num3[t] = num3[j];
                                            num3[j] = temp;
                                        }
                                    }
                                }
                                string num22 = string.Empty;
                                for (int w = 0; w < 3; w++)//遍历数组显示结果
                                {
                                    num22 = num22 + num3[w];
                                }
                                if (num22.Contains(model_1.Two_Same_Single))
                                {
                                    e++;
                                }
                                if (e > 0)
                                {
                                    break;
                                }
                            }
                            //if (Two_Same_Single.Contains(model_1.Two_Same_Single))
                            if (e > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_Single", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 8)//二不同号
                        {
                            string[] tt = (model_1.Two_dissame).Split(',');//开奖的数据
                            string[] bb = Two_dissame.Split(',');//投注的数据
                            int a = tt.Length;
                            int b = bb.Length;
                            int j = 0;

                            for (int y = 0; y <= tt.Length - 1; y++)
                            {
                                for (int ii = 0; ii <= bb.Length - 1; ii++)
                                {
                                    if (bb[ii].Contains(tt[y]))
                                    {
                                        j++;
                                    }
                                }
                            }
                            if (j > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_dissame", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds * j);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        //1大2小、1单2双
                        if (Play_Way == 9)//大小
                        {
                            if (DaXiao == (model_1.DaXiao))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        if (Play_Way == 10)//单双
                        {
                            if (DanShuang == (model_1.DanShuang))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                if (WinCent > 0)
                                {
                                    new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                    if (isRobot == 0)
                                        new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了" + WinCent + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                }
                            }
                        }
                        new BCW.XinKuai3.BLL.XK3_Bet().UpdateState(ID, 1);
                    }
                }
                Utils.Success("单期开奖", "单期开奖成功", Utils.getUrl("xk3.aspx?act=fenxi&amp;uid=" + uid1 + "&amp;qihaos=" + qihaos1 + "&amp;ptype=" + ptype1 + "&amp;isRobot=" + isRobot3 + ""), "1");
            }
            else
            {
                Master.Title = "对ID：" + uu.UsID + "投注的第" + uu.Lottery_issue + "期开奖吗？";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定对ID：" + uu.UsID + "投注的第" + uu.Lottery_issue + "期开奖吗？");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=ck&amp;info=1&amp;id=" + id + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;ptype=" + ptype + "&amp;tt=" + isRobot2 + "") + "\">确定开奖</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        else
        {
            Utils.Error("系统还没有该期的开奖号码,请等待系统开奖.", "");
        }
    }

    //单注修改
    private void xgPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.XinKuai3.Model.XK3_Bet uu = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet(id);
        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum("'" + uu.Lottery_issue + "'");
        if (info == "1")
        {
            int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
            int uid1 = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
            int qihaos1 = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));

            int new_num = int.Parse(Utils.GetRequest("new_num", "all", 1, @"^[1-9]\d*$", "0"));

            new BCW.XinKuai3.BLL.XK3_Bet().update_zd("Lottery_issue=" + new_num + "", "id=" + id1 + "");
            Utils.Success("修改期号", "修改期号成功", Utils.getUrl("xk3.aspx?act=xg&amp;id=" + id1 + "&amp;uid=" + uid1 + "&amp;qihaos=" + qihaos1 + "&amp;ptype=" + ptype1 + ""), "1");
        }
        else
        {
            Master.Title = "对ID：" + uu.UsID + "投注的第" + uu.Lottery_issue + "期修改吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定对用户【" + uu.UsID + "】投注的第" + uu.Lottery_issue + "期修改吗？");
            builder.Append(Out.Tab("</div>", "<br />"));

            string strText = "原期号:/,新期号:/,";
            string strName = "old_num,new_num,backurl";
            string strType = "num,num,hidden";
            string strValu = "" + uu.Lottery_issue + "''" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "修改,xk3.aspx?act=xg&amp;info=1&amp;id=" + id + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3.aspx?act=fenxi&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;ptype=" + ptype + "") + "\">返回上一级>></a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
    }

}
