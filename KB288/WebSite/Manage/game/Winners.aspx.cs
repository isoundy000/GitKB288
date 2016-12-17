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
using BCW.Files;
/// <summary>
/// 20160523
/// 20160607_修复一点界面细节
/// 20160607完善勾选功能
/// 20160622小时作为过期的单位，控制额度功能
/// 20160815 更新为5个奖池 可控 
/// 20160907 后台下拉选择修改
/// 20160910 恢复群聊获奖功能
/// </summary>
public partial class Manage_game_Winners : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/winners.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string GameName = Convert.ToString(ub.GetSub("WinnersName", "/Controls/winners.xml"));
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "addgame": //添加游戏
                WinnersAddGame();
                break;
            case "deletegame": //添加游戏
                WinnersDeleGame();
                break;
            case "little": //动作选择
                WinnersChooseLittle();
                break;
            case "setgold": //限额设定
                WinnersSetGold();
                break;
            case "notget"://未领取
                WinnersListNotGet();
                break;
            case "get"://已领取
                WinnersGet();
                break;
            case "pass"://已过期
                PassPage();
                break;
            case "award"://奖池数据
                WinnersAward();
                break;
            case "addaward"://生成奖池数据
                WinnersAddAward();
                break;
            case "set"://游戏设置
                WinnersSet();
                break;
            case "open"://游戏维护
                WinnersOpen();
                break;
            case "openChoose"://开关选择
                WinnersOpenChoose();
                break;
            case "reset"://重置游戏
                ResetPage();
                break;
            case "paijiang"://派奖
                PaiJiang();
                break;
            case "toplist"://排行榜单
                TopListPage();
                break;
            case "data"://数据分析
                DataPage();
                break;
            case "ceshi"://数据分析
                CeshiManagePage();
                break;
            case "list"://全部记录
                WinnersList();
                break;
            case "awardset"://奖池数据设置
                WinnersAwardSet();
                break;
            default:
                ReloadPage();  //主页
                break;
        }
    }

    //管理主页
    private void ReloadPage()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        //   builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append(GameName);
        builder.Append(Out.Tab("</div>", "<br/>"));
        Footer();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //管理底部
    private void Footer()
    {
        //builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        //builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【游戏管理】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=list") + "\">中奖记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">查看奖池</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset") + "\">奖池设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist") + "\">榜单管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=data") + "\">数据分析</a><br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【配置管理】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=openChoose") + "\">开放选择</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=little") + "\">动作勾选</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=setgold") + "\">游戏限额</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=open") + "\">测试维护</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=set") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append(Out.Tab("</div>", ""));
    }

    // 好运活跃抽奖排行榜 
    private void TopListPage()
    {
        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>" + "&gt;" + "排行榜管理");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        string str = "ID>0 group by UserId order by count desc ";

        if (ptype == 1)
        {
            builder.Append("总 榜|");
            str = " Ident=0 group by UserId order by count desc ";
            if (start.Length > 1)
            {
                try
                {
                    str = " Ident=0 and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "' group by UserId order by count desc";
                    if (uid > 0)
                    { str = " Ident=0 and UserId=" + uid + " and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "' group by UserId order by count desc"; }
                }
                catch { }

            }
            else
            {
                start = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=1&amp;") + "\">总 榜</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("今 日|");
            str = "Ident=0  and  Year(AddTime) = " + DateTime.Now.Year + "" + " and Month(AddTime) = " + DateTime.Now.Month + "and Day(AddTime) = " + (DateTime.Now.Day) + "group by UserId order by count desc ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=2&amp;") + "\">今 日</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("周 榜|");
            str = "Ident=0 and  datediff(week,[AddTime],getdate())=0" + "group by UserId order by count desc ";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=3&amp;") + "\">周 榜</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("月 榜");
            str = "Ident=0  and  Year(AddTime) = " + (DateTime.Now.Year) + " and Month(AddTime) = " + (DateTime.Now.Month) + "group by UserId order by count desc ";

        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=toplist&amp;ptype=4&amp;") + "\">月 榜</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        try
        {
            int sum;
            //string str = "ID>0 group by UserId order by count desc ";
            DataSet ds = new BCW.BLL.tb_WinnersLists().GetList("UserId,count(UserId)as count,sum(winGold)as money ", str);
            sum = 9;
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "uid", "start", "down" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
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
                for (int i = 0; i < skt; i++)
                {
                    if (i % 2 == 0)
                    { builder.Append(Out.Tab("<div >", "<br/>")); }
                    else
                    {
                        if (i == 1)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        else
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                    }
                    //if (sum < i)
                    //{ break; }
                    //else
                    {
                        builder.Append("<font  color=\"red\">" + "[" + (koo + i + 1) + "]" + "</font>" + ".");
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"]));
                        //    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[koo + i]["UserId"] + "") + "\">" + mename + "</a>");
                        //  builder.Append("(" + ds.Tables[0].Rows[koo + i]["UserId"] + ")");
                        builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UserId"])) + "</a>" + "抽奖");
                        builder.Append(ds.Tables[0].Rows[koo + i]["count"] + "次,");
                        builder.Append("获得" + ds.Tables[0].Rows[koo + i]["money"] + ub.Get("SiteBz"));
                        // builder.Append("<br/>");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    //  builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append("暂无相关排行.");
            }
        }
        catch { builder.Append("暂无相关排行."); }
        if (ptype == 1)
        {
            string strText1 = "用户:,开始:,结束:,";
            string strName1 = "uid,start,down,backurl";
            string strType1 = "num,text,text,hidden";
            string strValu1 = uid + "'" + start + "'" + down + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,true,true,false";
            string strIdea1 = "";
            string strOthe1 = "确定搜索,Winners.aspx?act=toplist&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        //底部
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:排行榜仅统计会员已领取的数据,未领取或者过期的数据不统计在内.<br/>");
        //builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    // 酷币测试版
    private void CeshiManagePage()
    {
        Master.Title = GameName + "测试管理";
        // builder.Append(Out.Div("title", GameName+"维护"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("测试管理");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/winners.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ceshi = Utils.GetRequest("ceshi", "post", 2, @"^[0-9]\d*$", "测试权限管理隔输入出错");
            string CeshiQualification = Utils.GetRequest("CeshiQualification", "post", 2, @"^[^\^]{1,2000}$", "添加测试号输入出错");
            xml.dss["ceshi"] = ceshi;//0酷币版测试状态1酷币版开放
            xml.dss["CeshiQualification"] = CeshiQualification;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("活跃抽奖设置", "设置成功，正在返回..", Utils.getUrl("Winners.aspx?act=ceshi"), "1");
        }
        else
        {

            string strText = "测试权限管理:/,添加测试号(#号结束):/,";
            string strName = "ceshi,CeshiQualification,backurl";
            string strType = "select,text,hidden";
            string strValu = xml.dss["ceshi"] + "'" + xml.dss["CeshiQualification"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|" + ub.Get("SiteBz") + "版测试号测试|1|" + ub.Get("SiteBz") + "版开放,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,Winners.aspx?act=ceshi,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
            string[] name = CeshiQualification.Split('#');
            // foreach (string n in imgNum)
            builder.Append("当前测试号:" + "<br/>");
            for (int n = 0; n < name.Length - 1; n++)
            {
                builder.Append(name[n] + ",");
                if (n > 2 && n % 5 == 0)
                { builder.Append("<br/>"); }
            }
            builder.Append(Out.Tab("</div>", ""));

        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //游戏维护
    private void WinnersOpen()
    {
        Master.Title = GameName + "维护";
        // builder.Append(Out.Div("title", GameName+"维护"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("测试与维护");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //string xmlPath = "/Controls/winners.xml";
        string WinnersStatus = Convert.ToString(ub.GetSub("WinnersStatus", xmlPath));
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (WinnersStatus == "2")
            {
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
                string CeshiQualification = Utils.GetRequest("CeshiQualification", "post", 2, @"^[^\^]{1,2000}$", "测试号添加出错");
                xml.dss["WinnersStatus"] = Status;
                xml.dss["CeshiQualification"] = CeshiQualification;

            }
            else
            {
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
                xml.dss["WinnersStatus"] = Status;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置", "设置成功，正在返回..", Utils.getUrl("Winners.aspx?act=open&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {

            if (WinnersStatus == "2")
            {
                string strText1 = "选择状态:/,添加测试号:/,";
                string strName1 = "Status,CeshiQualification,backurl";
                string strType1 = "select,textarea,hidden";
                string strValu1 = "" + xml.dss["WinnersStatus"] + "'" + xml.dss["CeshiQualification"] + "'" + Utils.getPage(0) + "";
                string strEmpt1 = "0|正常|1|维护|2|测试,true";
                string strIdea1 = "/";
                string strOthe1 = "确定修改|reset,Winners.aspx?act=open,post,1,red|blue";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            }
            else
            {
                string strText = "选择状态:/,";
                string strName = "Status,backurl";
                string strType = "select,hidden";
                string strValu = "" + xml.dss["WinnersStatus"] + "'" + "'" + Utils.getPage(0) + "";
                string strEmpt = "0|正常|1|维护|2|测试  ,";
                string strIdea = "/";
                string strOthe = "确定修改|reset,Winners.aspx?act=open,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", "<br/>"));
            if (xml.dss["WinnersStatus"].ToString() == "0")
            {
                builder.Append("<b>" + "游戏正在正常运行中..." + "</b>");
            }
            else if (xml.dss["WinnersStatus"].ToString() == "1")
            {
                builder.Append("<b>" + "游戏已维护." + "</b>");
            }
            else if (xml.dss["WinnersStatus"].ToString() == "2")
            {
                builder.Append("<b>" + "游戏正在测试中." + "</b><br/>");
                string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
                string[] name = CeshiQualification.Split('#');
                // foreach (string n in imgNum)
                builder.Append("当前测试号:" + "<br/>");
                for (int n = 0; n < name.Length - 1; n++)
                {
                    builder.Append(name[n] + ",");
                    if (n > 2 && n % 5 == 0)
                    { builder.Append("<br/>"); }
                }
            }
            builder.Append("<br/><a href=\"" + Utils.getUrl("Winners.aspx?act=ceshi") + "\">测试管理</a>");
            builder.Append("<br/><a href=\"" + Utils.getUrl("Winners.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            //   builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //删除游戏
    private void WinnersDeleGame()
    {
        Master.Title = GameName + "删除游戏";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=openChoose") + "\">开关选择</a>&gt;");
        builder.Append("删除游戏");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ID = Convert.ToInt32(Utils.GetRequest("ID", "all", 1, @"^[0-9]\d*$", "0"));
        if (ac == "ok")
        {
            if (ID == 0)
            {
                Utils.Error("ID错误！", "");
            }
            else
            {
                new BCW.BLL.tb_WinnersGame().Delete(ID);
                Utils.Success("设置", "删除成功，正在返回..", Utils.getUrl("Winners.aspx?act=addgame&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该游戏的限额吗？");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=deletegame&amp;ac=ok&amp;ID=" + ID + "") + "\">是</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addgame") + "\">否,返回添加游戏</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=setgold") + "\">否,返回限额设定</a><br />");
            builder.Append("温馨提示：删除后该游戏不限制即有活跃抽奖机会.");
        }



        builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //添加游戏
    private void WinnersAddGame()
    {
        Master.Title = GameName + "添加游戏";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=openChoose") + "\">开关选择</a>&gt;");
        builder.Append("添加游戏");
        builder.Append(Out.Tab("</div>", ""));
        //   builder.Append(new BCW.BLL.tb_WinnersGame().ExistsGameName("虚拟球类"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int english = int.Parse(Utils.GetRequest("english", "all", 1, "", "0"));
        string[] ttpye = { " 游戏区奖池1 ", " 游戏区奖池2", " 游戏区奖池3", " 论坛奖池4 ", " 奖池5" };
        string bydrname = Convert.ToString(ub.GetSub("bydrName", "/Controls/BYDR.xml"));//捕鱼达人
        string drawlife = ub.GetSub("DawnlifeName", "/Controls/Dawnlife.xml");//闯荡人生
        string dzpkName = ub.GetSub("DzpkName", "/Controls/dzpk.xml");//德州扑克
        string farmName = ub.GetSub("FarmName", "/Controls/farm.xml");//农场
        string hc1Name = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//好彩一
        string hp3 = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");//快乐扑克3
        string jqc = ub.GetSub("GameName", "/Controls/jqc.xml");//进球彩
        string kbyg = Convert.ToString(ub.GetSub("KbygName", "/Controls/myyg.xml"));//云购
        string klsf = ub.GetSub("klsfName", "/Controls/klsf.xml");//快乐十分
        string luck28 = ub.GetSub("Luck28Name", "/Controls/luck28.xml");//幸运28
        string pk10 = ub.GetSub("GameName", "/Controls/PK10.xml");//pk10
        string sfc = ub.GetSub("SFName", "/Controls/SFC.xml");//胜负彩
        string xk3 = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//新快3
        string sixchangban = ub.GetSub("BQCName", "/Controls/BQC.xml");//六场半
        string bjl = ub.GetSub("baccaratName", "/Controls/baccarat.xml");//百家乐
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string name = (Utils.GetRequest("name", "all", 2, @"^[^\^]{1,20}$", "游戏名字不能为空"));
            long price = Convert.ToInt64(Utils.GetRequest("price", "all", 2, @"^[0-9]\d*$", "最低下注不能为空"));
            BCW.Model.tb_WinnersGame model = new BCW.Model.tb_WinnersGame();
            int id = new BCW.BLL.tb_WinnersGame().GetMaxId();
            model.GameName = name;
            model.price = price;
            model.ptype = english;
            model.Ident = "game" + id.ToString();
            new BCW.BLL.tb_WinnersGame().Add(model);
            Utils.Success("设置", "添加成功，正在返回..", Utils.getUrl("Winners.aspx?act=addgame&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            string name = (Utils.GetRequest("name", "all", 1, @"^[^\^]{1,20}$", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("当前游戏名称<font color=\"blue\">(提示:红色已添加)</font>:<br/>" + isHaveGame("虚拟球类") + "|" + isHaveGame("点数挖宝") + "|" + isHaveGame(drawlife) + "|" + isHaveGame(dzpkName) + "<br/>");
            builder.Append(isHaveGame("大小掷骰") + "|" + isHaveGame("上证指数") + "|" + isHaveGame("跑马风云") + "|" + isHaveGame("大小庄家") + "<br/>");
            builder.Append(isHaveGame(farmName) + "|" + isHaveGame(hc1Name) + "|" + isHaveGame(hp3) + "|" + isHaveGame(jqc) + "<br/>");
            builder.Append(isHaveGame(klsf) + "|" + isHaveGame(kbyg) + "|" + isHaveGame(luck28) + "|" + isHaveGame(pk10) + "<br/>");
            builder.Append(isHaveGame(sfc) + "|" + isHaveGame(xk3) + "|" + isHaveGame(sixchangban) + "|" + isHaveGame(bjl) + "<br/>");
            builder.Append(isHaveGame(bydrname));
            builder.Append(Out.Tab("</div>", ""));
            string strText1 = "游戏名称:,最低下注:,所属分类:,";
            string strName1 = "name,price,english,backurl";
            string strType1 = "text,text,select,hidden";
            string strValu1 = "" + name + "'" + "'" + english + "'" + Utils.getPage(0);
            string strEmpt1 = "true,true,0|游戏区奖池1|1|游戏区奖池2|2|游戏区奖池3|3|论坛奖池4|4|奖池5,hidden";
            string strIdea1 = "/";
            string strOthe1 = "确定添加|reset,Winners.aspx?act=addgame,post,1,red|blue";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            DataSet ds = new BCW.BLL.tb_WinnersGame().GetList("*", "ID>0 order by ID desc");
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("[已有游戏(" + ds.Tables[0].Rows.Count + ")][最低下注][所属奖池][当前ID]");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("" + "<a href=\"" + Utils.getUrl("Winners.aspx?act=deletegame&amp;ID=" + Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]) + "") + "\">[删]</a>");
                    string s = Convert.ToString(ds.Tables[0].Rows[i]["GameName"]);
                    builder.Append(s + "：" + "<font color=\"red\">" + Convert.ToString(ds.Tables[0].Rows[i]["price"]) + "</font>、");
                    builder.Append("" + ttpye[Convert.ToInt32(ds.Tables[0].Rows[i]["ptype"])] + "、");
                    builder.Append("奖池ID");
                    builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;uid=" + new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(Convert.ToString(ds.Tables[0].Rows[i]["ptype"])) + "") + "\">" + new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(Convert.ToString(ds.Tables[0].Rows[i]["ptype"])) + "</a>");
                    builder.Append("<br/>");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=setgold") + "\">返回限额设定</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private string isHaveGame(string gamename)
    {
        if (new BCW.BLL.tb_WinnersGame().ExistsGameName(gamename))
        { return "<font color=\"red\">" + gamename + "</font>"; }
        else
        { return "<a href =\"" + Utils.getUrl("Winners.aspx?act=addgame&amp;name=" + gamename + "&amp;") + "\"><font color=\"green\">" + gamename + "</font></a>"; }
    }
    //限额设定
    private void WinnersSetGold()
    {
        //builder.Append(new BCW.BLL.tb_WinnersGame().GetPrice("虚拟球类"));
        Master.Title = GameName + "限额设定";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=openChoose") + "\">开关选择</a>&gt;");
        builder.Append("限额设定");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //  builder.Append("限额设定");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            DataSet ds = new BCW.BLL.tb_WinnersGame().GetList("*", "ID>0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string s = Convert.ToString(ds.Tables[0].Rows[i]["Ident"]);
                    long ss = Convert.ToInt64(Utils.GetRequest("" + s + "", "all", 1, "", ""));
                    int ptype = int.Parse(Utils.GetRequest("" + s + "1" + "", "all", 1, @"^[0-5]$", "1"));
                    //  Utils.Error( ""+ ptype + "","");
                    //  try
                    {
                        new BCW.BLL.tb_WinnersGame().UpdatePricePtypeForIdent(s, ss, ptype);
                    }
                    //  catch { }
                }
            }
            Utils.Success("设置", "设置成功，正在返回..", Utils.getUrl("Winners.aspx?act=setgold&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            DataSet ds = new BCW.BLL.tb_WinnersGame().GetList("*", "ID>0");
            string name = "";
            string name1 = "";
            string type = "";
            string value = "";
            string empt = "";
            //    builder.Append(ds.Tables[0].Rows.Count);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (!ds.Tables[0].Rows[i]["GameName"].ToString().Contains("总奖池"))
                    {
                        name += (ds.Tables[0].Rows[i]["GameName"]) + "最低金额:," + (ds.Tables[0].Rows[i]["GameName"]) + "奖池分类:," + "------------/";
                        name1 += (ds.Tables[0].Rows[i]["Ident"]) + "," + (ds.Tables[0].Rows[i]["Ident"].ToString() + "1") + ",";
                        type += "text,select,";
                        value += (ds.Tables[0].Rows[i]["price"]) + "'" + (ds.Tables[0].Rows[i]["ptype"]) + "'";
                        empt += "true,0|游戏区奖池1|1|游戏区奖池2|2|游戏区奖池3|3|论坛奖池4|4|奖池5,";
                    }
                }


                // name += "backurl" + ":,";
                name1 += "hidden";
                type += "hidden";
                value += Utils.getPage(0);
                string strText1 = "" + name + "";
                string strName1 = "" + name1 + "";
                string strType1 = "" + type + "";
                string strValu1 = "" + value + "";
                string strEmpt1 = "" + empt + "";
                string strIdea1 = "/";
                string strOthe1 = "确定修改|reset,Winners.aspx?act=setgold,post,1,red|blue";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("暂无限制的记录.");
                builder.Append(Out.Tab("</div>", ""));
            }
            //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a>");

        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addgame") + "\">添加游戏</a><br/>");
        builder.Append("温馨提示:设定未0则表示不限制");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //动作选择
    private void WinnersChooseLittle()
    {
        Master.Title = GameName + "开关选择";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=openChoose") + "\">开关选择</a>&gt;");
        builder.Append("游戏社区动作选择");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        //1全社区2社区3仅游戏
        int WinnersOpenChoose = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", "/Controls/winners.xml"));
        string[] Num = new string[50];
        string[] Num1 = new string[50];
        Num[1] = Utils.GetRequest("Num1", "post", 1, "", "");
        Num[2] = Utils.GetRequest("Num2", "post", 1, "", "");
        Num[3] = Utils.GetRequest("Num3", "post", 1, "", "");
        Num[4] = Utils.GetRequest("Num4", "post", 1, "", "");
        Num[5] = Utils.GetRequest("Num5", "post", 1, "", "");
        Num[6] = Utils.GetRequest("Num6", "post", 1, "", "");
        Num[7] = Utils.GetRequest("Num7", "post", 1, "", "");
        Num[8] = Utils.GetRequest("Num8", "post", 1, "", "");
        Num[9] = Utils.GetRequest("Num9", "post", 1, "", "");
        Num[10] = Utils.GetRequest("Num10", "post", 1, "", "");
        Num[11] = Utils.GetRequest("Num11", "post", 1, "", "");
        Num[12] = Utils.GetRequest("Num12", "post", 1, "", "");
        Num[13] = Utils.GetRequest("Num13", "post", 1, "", "");
        Num[14] = Utils.GetRequest("Num14", "post", 1, "", "");
        Num[15] = Utils.GetRequest("Num15", "post", 1, "", "");
        Num[16] = Utils.GetRequest("Num16", "post", 1, "", "");
        Num[17] = Utils.GetRequest("Num17", "post", 1, "", "");
        Num[18] = Utils.GetRequest("Num18", "post", 1, "", "");
        Num[19] = Utils.GetRequest("Num19", "post", 1, "", "");
        Num[20] = Utils.GetRequest("Num20", "post", 1, "", "");
        Num[21] = Utils.GetRequest("Num21", "post", 1, "", "");
        //
        Num1[1] = Utils.GetRequest("Numa1", "post", 1, "", "");
        Num1[2] = Utils.GetRequest("Numa2", "post", 1, "", "");
        Num1[3] = Utils.GetRequest("Numa3", "post", 1, "", "");
        Num1[4] = Utils.GetRequest("Numa4", "post", 1, "", "");
        Num1[5] = Utils.GetRequest("Numa5", "post", 1, "", "");
        Num1[6] = Utils.GetRequest("Numa6", "post", 1, "", "");
        Num1[7] = Utils.GetRequest("Numa7", "post", 1, "", "");
        Num1[8] = Utils.GetRequest("Numa8", "post", 1, "", "");
        Num1[9] = Utils.GetRequest("Numa9", "post", 1, "", "");
        Num1[10] = Utils.GetRequest("Numa10", "post", 1, "", "");
        Num1[11] = Utils.GetRequest("Numa11", "post", 1, "", "");
        Num1[12] = Utils.GetRequest("Numa12", "post", 1, "", "");
        Num1[13] = Utils.GetRequest("Numa13", "post", 1, "", "");
        Num1[14] = Utils.GetRequest("Numa14", "post", 1, "", "");
        Num1[15] = Utils.GetRequest("Numa15", "post", 1, "", "");
        Num1[16] = Utils.GetRequest("Numa16", "post", 1, "", "");
        Num1[17] = Utils.GetRequest("Numa17", "post", 1, "", "");
        Num1[18] = Utils.GetRequest("Numa18", "post", 1, "", "");
        Num1[19] = Utils.GetRequest("Numa19", "post", 1, "", "");
        Num1[20] = Utils.GetRequest("Numa20", "post", 1, "", "");
        Num1[21] = Utils.GetRequest("Numa21", "post", 1, "", "");
        Num1[22] = Utils.GetRequest("Numa22", "post", 1, "", "");
        Num1[23] = Utils.GetRequest("Numa23", "post", 1, "", "");
        Num1[24] = Utils.GetRequest("Numa24", "post", 1, "", "");
        Num1[25] = Utils.GetRequest("Numa25", "post", 1, "", "");
        Num1[26] = Utils.GetRequest("Numa26", "post", 1, "", "");
        Num1[27] = Utils.GetRequest("Numa27", "post", 1, "", "");
        if (info == "ok")
        {
            if (ac == "ok")
            {
                ub xml = new ub();
                string xmlPath = "/Controls/winners.xml";
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置 .
                string game = Utils.GetRequest("game", "all", 1, "", "");
                string bbs = Utils.GetRequest("bbs", "all", 1, "", "");
                //1全社区2社区3仅游戏
                if (WinnersOpenChoose == 1 || WinnersOpenChoose == 3)
                {
                    xml.dss["gameAction"] = game;//游戏
                }
                //1全社区2社区3仅游戏
                if (WinnersOpenChoose == 1 || WinnersOpenChoose == 2)
                {

                    xml.dss["bbsAction"] = bbs;//论坛
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("winners.aspx?act=little&amp;backurl=" + Utils.getPage(1) + ""), "1");

            }
            else
            {

                string game = "";
                string bbs = "";
                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                builder.Append("确定动作选择吗?" + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                //1全社区2社区3仅游戏
                if (WinnersOpenChoose == 1 || WinnersOpenChoose == 3)
                {
                    builder.Append("游戏区：" + "<br/>");
                    int k = 1;
                    for (int i = 1; i < 22; i++)
                    {
                        if (Num[i] != "")
                        {
                            game += Num[i];
                            game += "#";
                            builder.Append(Num[i] + ".");
                            if (k % 3 == 0)
                            { builder.Append("<br/>"); }
                            k++;
                        }
                    }
                }
                //1全社区2社区3仅游戏
                if (WinnersOpenChoose == 1 || WinnersOpenChoose == 2)
                {
                    builder.Append("<br/>" + "论坛区：" + "<br/>");
                    int j = 1;
                    for (int i = 1; i < 28; i++)
                    {
                        if (Num1[i] != "")
                        {
                            bbs += Num1[i];
                            bbs += "#";
                            builder.Append(Num1[i] + ".");
                            if (j % 3 == 0)
                            { builder.Append("<br/>"); }
                            j++;
                        }
                    }
                }
                builder.Append("<input type=\"hidden\" name=\"gameAction\" Value=\"" + game + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"bbsAction\" Value=\"" + bbs + "\"/>");
                builder.Append(Out.Tab("</div>", "<br/>"));
                string strText = ":,:,,,";
                string strName = "game,bbs,info,ac,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden";
                string strValu = game + "'" + bbs + "'" + "ok" + "'" + "ok" + "'" + Utils.getPage(0);
                string strEmpt = "true,false,false,false,false";
                string strIdea = "";
                string strOthe = "确定选择,Winners.aspx?act=little&amp;info=ok&amp;ac=ok&amp;,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<br/><a href=\"" + Utils.getUrl("Winners.aspx?act=little&amp;") + "\">重新选择</a>");
                builder.Append(Out.Tab("</div>", "<br/>"));

            }
        }
        else
        {
            string gameAction = (ub.GetSub("gameAction", xmlPath));
            string bbsAction = (ub.GetSub("bbsAction", xmlPath));
            string bydrname = Convert.ToString(ub.GetSub("bydrName", "/Controls/BYDR.xml"));//捕鱼达人
            string drawlife = ub.GetSub("DawnlifeName", "/Controls/Dawnlife.xml");//闯荡人生
            string dzpkName = ub.GetSub("DzpkName", "/Controls/dzpk.xml");//德州扑克
            string farmName = ub.GetSub("FarmName", "/Controls/farm.xml");//农场
            string hc1Name = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//好彩一
            string hp3 = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");//快乐扑克3
            //  string jqc = ub.GetSub("GameName", "/Controls/jqc.xml");//进球彩
            string kbyg = Convert.ToString(ub.GetSub("KbygName", "/Controls/myyg.xml"));//云购
            string klsf = ub.GetSub("klsfName", "/Controls/klsf.xml");//快乐十分
            string luck28 = ub.GetSub("Luck28Name", "/Controls/luck28.xml");//幸运28
            string pk10 = ub.GetSub("GameName", "/Controls/PK10.xml");//pk10
            string sfc = ub.GetSub("SFName", "/Controls/SFC.xml");//胜负彩
            string xk3 = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//新快3
            string sixchangban = ub.GetSub("BQCName", "/Controls/BQC.xml");//六场半
            string bjl = ub.GetSub("baccaratName", "/Controls/bjl.xml");//百家乐
            builder.Append("<form id=\"form1\" method=\"post\" action=\"Winners.aspx\">");
            //1全社区2社区3仅游戏
            if (WinnersOpenChoose == 1 || WinnersOpenChoose == 3)
            {
                if (WinnersOpenChoose == 1)
                { builder.Append("<div class=\"text\">= = =当前全网开放 请选择 可全勾选= = =<br/></div>"); }
                else { builder.Append("<div class=\"text\">= = =当前仅游戏区开放 请选择 可全勾选= = =<br/></div>"); }
                builder.Append("<table><tr><td>");

                if (gameAction.Contains("虚拟球类"))
                { builder.Append("<input type=\"checkbox\" name=\"Num1\" value=\"虚拟球类\" checked=\"true\"/> 虚拟球类"); }
                else
                { builder.Append("<input type=\"checkbox\" name=\"Num1\" value=\"虚拟球类\" /> 虚拟球类"); }
                builder.Append("</td><td>");
                if (gameAction.Contains("虚拟彩票"))
                { builder.Append("<input type=\"checkbox\" name=\"Num2\" value=\"虚拟彩票\" checked=\"true\" /> 虚拟彩票"); }
                else
                { builder.Append("<input type=\"checkbox\" name=\"Num2\" value=\"虚拟彩票\" /> 虚拟彩票"); }
                builder.Append("</td><td>");
                if (gameAction.Contains(luck28))
                { builder.Append("<input type=\"checkbox\" name=\"Num3\" value=\"" + luck28 + "\" checked=\"true\" /> " + luck28 + ""); }
                else
                { builder.Append("<input type=\"checkbox\" name=\"Num3\" value=\"" + luck28 + "\" /> " + luck28 + ""); }
                builder.Append("</td></tr><tr><td>");
                if (gameAction.Contains("点数挖宝"))
                { builder.Append("<input type=\"checkbox\" name=\"Num4\" value=\"点数挖宝\" checked=\"true\" /> 点数挖宝"); }
                else { builder.Append("<input type=\"checkbox\" name=\"Num4\" value=\"点数挖宝\" /> 点数挖宝"); }
                builder.Append("</td><td>");
                if (gameAction.Contains("大小庄家"))
                    builder.Append("<input type=\"checkbox\" name=\"Num5\" value=\"大小庄家\" checked=\"true\"/> 大小庄家");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num5\" value=\"大小庄家\" /> 大小庄家");
                builder.Append("</td><td>");
                if (gameAction.Contains("跑马风云"))
                    builder.Append("<input type=\"checkbox\" name=\"Num6\" value=\"跑马风云\" checked=\"true\" /> 跑马风云");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num6\" value=\"跑马风云\" /> 跑马风云");
                builder.Append("</td></tr><tr><td>");
                if (gameAction.Contains("上证指数"))
                    builder.Append("<input type=\"checkbox\" name=\"Num7\" value=\"上证指数\" checked=\"true\" /> 上证指数");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num7\" value=\"上证指数\" /> 上证指数");
                builder.Append("</td><td>");
                if (gameAction.Contains("大小掷骰"))
                    builder.Append("<input type=\"checkbox\" name=\"Num8\" value=\"大小掷骰\" checked=\"true\" /> 大小掷骰");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num8\" value=\"大小掷骰\" /> 大小掷骰");
                builder.Append("</td><td>");
                if (gameAction.Contains("欢乐竞拍"))
                    builder.Append("<input type=\"checkbox\" name=\"Num9\" value=\"欢乐竞拍\" checked=\"true\" /> 欢乐竞拍");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num9\" value=\"欢乐竞拍\" /> 欢乐竞拍");
                builder.Append("</td></tr><tr><td>");
                if (gameAction.Contains("时时彩票"))
                    builder.Append("<input type=\"checkbox\" name=\"Num10\" value=\"时时彩票\" checked=\"true\"  /> 时时彩票");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num10\" value=\"时时彩票\"  /> 时时彩票");
                builder.Append("</td><td>");
                if (gameAction.Contains("吹牛游戏"))
                    builder.Append("<input type=\"checkbox\" name=\"Num11\" value=\"吹牛游戏\" checked=\"true\" /> 吹牛游戏");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num11\" value=\"吹牛游戏\"  /> 吹牛游戏");
                builder.Append("</td><td>");
                if (gameAction.Contains(bydrname))
                    builder.Append("<input type=\"checkbox\" name=\"Num12\" value=\"" + bydrname + "\" checked=\"true\" /> " + bydrname + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num12\" value=\"" + bydrname + "\" /> " + bydrname + "");
                builder.Append("</td></tr><tr><td>");
                if (gameAction.Contains(drawlife))
                    builder.Append("<input type=\"checkbox\" name=\"Num13\" value=\"" + drawlife + "\" checked=\"true\" /> " + drawlife + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num13\" value=\"" + drawlife + "\"  /> " + drawlife + "");
                builder.Append("</td><td>");
                if (gameAction.Contains(xk3))
                    builder.Append("<input type=\"checkbox\" name=\"Num14\" value=\"" + xk3 + "\" checked=\"true\"  /> " + xk3 + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num14\" value=\"" + xk3 + "\"  /> " + xk3 + "");
                builder.Append("</td><td>");
                if (gameAction.Contains(hp3))
                    builder.Append("<input type=\"checkbox\" name=\"Num15\" value=\"" + hp3 + "\" checked=\"true\"  /> " + hp3 + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num15\" value=\"" + hp3 + "\"  /> " + hp3 + "");
                builder.Append("</td></tr><tr><td>");
                if (gameAction.Contains(farmName))
                    builder.Append("<input type=\"checkbox\" name=\"Num16\" value=\"" + farmName + "\"  checked=\"true\" /> " + farmName + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num16\" value=\"" + farmName + "\" /> " + farmName + "");
                builder.Append("</td><td>");
                if (gameAction.Contains(dzpkName))
                    builder.Append("<input type=\"checkbox\" name=\"Num17\" value=\"" + dzpkName + "\"  checked=\"true\" /> " + dzpkName + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num17\" value=\"" + dzpkName + "\" /> " + dzpkName + "");
                builder.Append("</td><td>");
                if (gameAction.Contains(bjl))
                    builder.Append("<input type=\"checkbox\" name=\"Num18\" value=\"" + bjl + "\"  checked=\"true\"/> " + bjl + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num18\" value=\"" + bjl + "\" /> " + bjl + "");
                builder.Append("</td></tr>");
                builder.Append("<tr><td>");
                if (gameAction.Contains(kbyg))
                    builder.Append("<input type=\"checkbox\" name=\"Num19\" value=\"" + kbyg + "\" checked=\"true\" /> " + kbyg + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num19\" value=\"" + kbyg + "\" /> " + kbyg + "");
                builder.Append("</td><td>");
                if (gameAction.Contains(hc1Name))
                    builder.Append("<input type=\"checkbox\" name=\"Num20\" value=\"" + hc1Name + "\" checked=\"true\" /> " + hc1Name + "");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num20\" value=\"" + hc1Name + "\" /> " + hc1Name + "");
                builder.Append("</td><td>");
                // string pk10 = ub.GetSub("GameName", "/Controls/PK10.xml");
                if (gameAction.Contains(pk10))
                    builder.Append("<input type=\"checkbox\" name=\"Num21\" value=\"" + pk10 + "\" checked=\"true\" /> " + pk10);
                else
                    builder.Append("<input type=\"checkbox\" name=\"Num21\" value=\"" + pk10 + "\" /> " + pk10);
                builder.Append("</td></tr>");
                builder.Append("</table>");
            }
            //社区
            //1全社区2社区3仅游戏
            if (WinnersOpenChoose == 1 || WinnersOpenChoose == 2)
            {
                if (WinnersOpenChoose == 1)
                {
                    builder.Append("<div class=\"text\">= = =当前全网开放 请选择 可全勾选= = =<br/></div>");
                }
                else { builder.Append("<div class=\"text\">= = =当前仅社区开放 请选择 可全勾选= = =<br/></div>"); }
                builder.Append("<table><tr><td>");
                if (bbsAction.Contains("发表帖子"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa1\" value=\"发表帖子\" checked=\"true\"/> 发表帖子");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa1\" value=\"发表帖子\" /> 发表帖子");
                builder.Append("</td><td>");
                if (bbsAction.Contains("回复帖子"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa2\" value=\"回复帖子\" checked=\"true\"/> 回复帖子");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa2\" value=\"回复帖子\" /> 回复帖子");
                builder.Append("</td><td>");
                if (bbsAction.Contains("帖子打赏"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa3\" value=\"帖子打赏\" checked=\"true\"/> 帖子打赏");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa3\" value=\"帖子打赏\" /> 帖子打赏");
                builder.Append("</td></tr><tr><td>");
                if (bbsAction.Contains("加精帖子"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa4\" value=\"加精帖子\" checked=\"true\"/> 加精帖子");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa4\" value=\"加精帖子\" /> 加精帖子");
                builder.Append("</td><td>");
                if (bbsAction.Contains("推荐帖子"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa5\" value=\"推荐帖子\" checked=\"true\"/> 推荐帖子");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa5\" value=\"推荐帖子\" /> 推荐帖子");
                builder.Append("</td><td>");
                if (bbsAction.Contains("设滚帖子"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa6\" value=\"设滚帖子\" checked=\"true\"/> 设滚帖子");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa6\" value=\"设滚帖子\" /> 设滚帖子");
                builder.Append("</td></tr><tr><td>");
                if (bbsAction.Contains("空间留言"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa7\" value=\"空间留言\" checked=\"true\"/> 空间留言");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa7\" value=\"空间留言\" /> 空间留言");
                builder.Append("</td><td>");
                if (bbsAction.Contains("购买VIP"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa8\" value=\"购买VIP\" checked=\"true\"/> 购买VIP");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa8\" value=\"购买VIP\" /> 购买VIP");
                builder.Append("</td><td>");
                if (bbsAction.Contains("购买靓号"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa9\" value=\"购买靓号\" checked=\"true\"/> 购买靓号");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa9\" value=\"购买靓号\" /> 购买靓号");
                builder.Append("</td></tr><tr><td>");
                if (bbsAction.Contains("上存照片"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa10\" value=\"上存照片\" checked=\"true\"/> 上存照片");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa10\" value=\"上存照片\" /> 上存照片");
                builder.Append("</td><td>");
                if (bbsAction.Contains("商城礼物"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa11\" value=\"商城礼物\" checked=\"true\"/> 商城礼物");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa11\" value=\"商城礼物\" /> 商城礼物");
                builder.Append("</td><td>");
                if (bbsAction.Contains("赠送礼物"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa12\" value=\"赠送礼物\" checked=\"true\"/> 赠送礼物");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa12\" value=\"赠送礼物\" /> 赠送礼物");
                if (bbsAction.Contains("空间签到"))
                    builder.Append("</td></tr><tr><td><input type=\"checkbox\"  name=\"Numa13\"  value=\"空间签到\" checked=\"true\"/> 空间签到");
                else
                    builder.Append("</td></tr><tr><td><input name=\"Numa13\" type=\"checkbox\"  value=\"空间签到\" /> 空间签到");
                builder.Append("</td><td>");
                if (bbsAction.Contains("过户币值"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa14\" value=\"过户币值\" checked=\"true\"/> 过户币值");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa14\" value=\"过户币值\" /> 过户币值");
                builder.Append("</td><td>");
                if (bbsAction.Contains("闲聊发言"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa15\" value=\"闲聊发言\" checked=\"true\"/> 闲聊发言");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa15\" value=\"闲聊发言\" /> 闲聊发言");
                builder.Append("</td></tr><tr><td>");
                if (bbsAction.Contains("发红包者"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa16\" value=\"发红包者\" checked=\"true\"/> 发红包者");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa16\" value=\"发红包者\" /> 发红包者");
                builder.Append("</td><td>");
                if (bbsAction.Contains("发喇叭者"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa17\" value=\"发喇叭者\" checked=\"true\"/> 发喇叭者");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa17\" value=\"发喇叭者\" /> 发喇叭者");
                builder.Append("</td><td>");
                if (bbsAction.Contains("婚恋结婚"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa18\" value=\"婚恋结婚\" checked=\"true\"/> 婚恋结婚");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa18\" value=\"婚恋结婚\" /> 婚恋结婚");
                builder.Append("</td></tr>");
                if (bbsAction.Contains("花园种花"))
                    builder.Append("<tr><td><input type=\"checkbox\" name=\"Numa19\" value=\"花园种花\" checked=\"true\"/> 花园种花");
                else
                    builder.Append("<tr><td><input type=\"checkbox\" name=\"Numa19\" value=\"花园种花\" /> 花园种花");
                builder.Append("</td><td>");
                if (bbsAction.Contains("注册会员"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa20\" value=\"注册会员\" checked=\"true\" /> 注册会员");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa20\" value=\"注册会员\" /> 注册会员");
                builder.Append("</td><td>");
                if (bbsAction.Contains("等级晋升"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa21\" value=\"等级晋升\" checked=\"true\"/> 等级晋升");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa21\" value=\"等级晋升\" /> 等级晋升");
                builder.Append("</td></tr><tr><td>");
                if (bbsAction.Contains("推荐注册"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa22\" value=\"推荐注册\" checked=\"true\"/> 推荐注册");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa22\" value=\"推荐注册\" /> 推荐注册");
                builder.Append("</td><td>");
                if (bbsAction.Contains("加为好友"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa23\" value=\"加为好友\" checked=\"true\" /> 加为好友");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa23\" value=\"加为好友\" /> 加为好友");
                builder.Append("</td><td>");
                if (bbsAction.Contains("书节上存"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa24\" value=\"书节上存\" checked=\"true\"/> 书节上存");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa24\" value=\"书节上存\" /> 书节上存");
                builder.Append("</td></tr><tr><td>");
                if (bbsAction.Contains("发表书评"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa25\" value=\"发表书评\" checked=\"true\"/> 发表书评");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa25\" value=\"发表书评\" /> 发表书评");
                builder.Append("</td><td>");
                if (bbsAction.Contains("聊吧"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa26\" value=\"聊吧\" checked=\"true\"/> 聊吧发言");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa26\" value=\"聊吧\" /> 聊吧发言");
                builder.Append("</td><td>");
                if (bbsAction.Contains("空间设置"))
                    builder.Append("<input type=\"checkbox\" name=\"Numa27\" value=\"空间设置\" checked=\"true\"/> 空间设置");
                else
                    builder.Append("<input type=\"checkbox\" name=\"Numa27\" value=\"空间设置\" /> 空间设置");
                builder.Append("</td></tr></table>");
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"little\"/>");
            builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok\"/>");
            builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"not\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("温馨提示：已勾选的表示已添加有抽奖机会.");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    //开关选择
    private void WinnersOpenChoose()
    {
        Master.Title = GameName + "开关选择";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("开关选择");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/winners.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string WinnersOpenOrClose = Utils.GetRequest("WinnersOpenOrClose", "post", 2, @"^[0-3]$", "状态选择错误");
            string WinnersOpenChoose = Utils.GetRequest("WinnersOpenChoose", "post", 2, @"^[0-3]$", "开放开关选择错误");
            string WinnersGuessOpen = Utils.GetRequest("GuessOpen", "post", 2, @"^[0-3]$", "内线开关选择错误");
            string ActionOpen = Utils.GetRequest("ActionOpen", "post", 2, @"^[0-3]$", "会员动态开关选择错误");
            //  string game = Utils.GetRequest("game", "post", 1, @"[\s\S]{0,2000}$", "");//游戏区触发抽奖的动作填写错误
            //  string bbs = Utils.GetRequest("bbs", "post", 1, @"^[^\^]{0,2000}$", "");//论坛区触发抽奖的动作填写错误
            string blackID = Utils.GetRequest("blackID", "post", 1, @"^[^\^]{0,2000}$", "");
            xml.dss["WinnersOpenChoose"] = WinnersOpenChoose;//开放选择(1全社区3游戏2友缘社区)
            xml.dss["WinnersOpenOrClose"] = WinnersOpenOrClose;//开放选择（派奖1开与0关）
            xml.dss["WinnersGuessOpen"] = WinnersGuessOpen;//开放选择(1全社区2游戏3友缘社区)
            xml.dss["ActionOpen"] = ActionOpen;//开放选择（派奖1开与0关）
            //xml.dss["gameAction"] = game;//游戏区触发抽奖的动作
            //xml.dss["bbsAction"] = bbs;//论坛区触发抽奖的动作
            xml.dss["blackID"] = blackID;//限制会员ID填写错误
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            //   Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("winners.aspx?act=openChoose&amp;backurl=" + Utils.getPage(1) + ""), "10");
            string where = "";
            if (WinnersOpenChoose == "1")
            { where = "全网"; }
            else if (WinnersOpenChoose == "2")
            { where = "社区论坛"; }
            else if (WinnersOpenChoose == "3")
            { where = "游戏"; }
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("设置成功!是否进行" + where + "动作选择?" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=little&amp;backurl=" + Utils.getPage(1) + "") + "\">是</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("winners.aspx?act=openChoose&amp;backurl=" + Utils.getPage(1) + "") + "\">返回开关选择</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //builder.Append(Out.Div("title", GameName + "设置"));
            string strText = "游戏开放选择:/,开放位置选择:/,发送内线开关:/,会员动态开关:/,游戏区动作:/,论坛区动作:/,限制中奖ID:/,";
            string strName = "WinnersOpenOrClose,WinnersOpenChoose,GuessOpen,ActionOpen,game,bbs,blackID,backurl";
            string strType = "select,select,select,select,hidden,hidden,big,hidden";
            string strValu = xml.dss["WinnersOpenOrClose"] + "'" + xml.dss["WinnersOpenChoose"] + "'" + xml.dss["WinnersGuessOpen"] + "'" + xml.dss["ActionOpen"] + "'" + xml.dss["gameAction"] + "'" + xml.dss["bbsAction"] + "'" + xml.dss["blackID"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|停止放送机会|1|开启放送机会,1|全网开放|2|仅社区|3|仅游戏,1|发送内线|2|关闭内线,1|开启会员动态|2|关闭会员动态,true,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,winners.aspx?act=openChoose,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            // builder.Append(Out.Tab("<div>", "<br />"));
            //builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=set") + "\">游戏配置</a><br />");
            //builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=open") + "\">测试维护</a>");
            //builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=little") + "\">动作选择</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=setgold") + "\">限额设定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", ""));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //派奖
    private void PaiJiang()
    {
        Master.Title = "兑奖中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("兑奖中心");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));//对应id
        int info = Utils.ParseInt(Utils.GetRequest("info", "get", 1, @"^[1-2]\d*$", "0"));//对应的商品编号
        int meid = new BCW.User.Users().GetUsId();
        BCW.Model.tb_WinnersLists model = new BCW.BLL.tb_WinnersLists().Gettb_WinnersLists(ptype);
        int GameId = 1016;
        if (info == 0)
        {
            Master.Title = "兑奖中心";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定派奖吗?" + "<br/>");
            builder.Append("当前会员: ");
            builder.Append("<a href =\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(model.UserId) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(model.UserId)) + "</a>");
            builder.Append("<br/>中奖位置: ");
            builder.AppendFormat(Out.SysUBB(model.Remarks).Replace("在", ""));
            builder.Append("<br/>获得等奖数: " + model.GetId);
            builder.Append("<br/>获得币值数: " + model.winGold);
            builder.Append("<br/>对应奖池ID: " + model.awardId);
            builder.Append("<br/>对应记录ID: " + model.Id);
            builder.Append("<br/>领奖状态: ");
            if (model.Ident == 1)
            {
                builder.Append("[未领]");
            }
            else
                if (model.Ident == 0)
                {
                    builder.Append("[已领]");
                }
                else
                {
                    builder.Append("[已过期]");
                }
            builder.Append("<br/>获奖时间: " + Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?info=1&amp;act=paijiang&amp;ptype=" + ptype + "&amp;") + "\">确定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            if (model.Id == ptype)
            {
                if (model.Ident == 0)
                { builder.Append("该记录已成功领取！"); }
                else if ((model.Ident == 2))
                { builder.Append("该记录因过期已无法领取！"); }
                else
                {
                    //builder.Append(model.Id + "," + ptype);
                    //  builder.Append(new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId)) + "("+model.UserId + ")在" + model.Remarks + "中获得了" + model.GetId + "等奖！" + "<br/>");
                    builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(model.AddTime)), model.UserId, new BCW.BLL.User().GetUsName(Convert.ToInt32(model.UserId)), Out.SysUBB(model.Remarks));
                    builder.Append("中获得了 " + model.GetId + " 等奖！奖金价值 " + model.winGold);
                    if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                    { builder.Append(ub.Get("SiteBz") + "<br/>"); }
                    else
                    { builder.Append(" 活跃值" + "<br/>"); }
                    long gold = Convert.ToInt64(model.winGold);
                    int userid = Convert.ToInt32(model.UserId);
                    if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                    {
                        try
                        {
                            new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 0);//0已领
                            new BCW.BLL.User().UpdateiGold(userid, gold, "活跃抽奖奖池ID" + model.awardId + "#编号" + model.Id);
                            builder.Append("派奖成功！");
                        }
                        catch { builder.Append("派奖失败,请重新派奖！"); }
                    }
                    else if (ub.GetSub("WinnersStatus", xmlPath) == "2")//测试
                    {
                        if (!new BCW.SWB.BLL().ExistsUserID(userid, GameId))//不存在用户记录直接领
                        {
                            //  BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
                            BCW.SWB.Model swbs = new BCW.SWB.Model();
                            swbs.UserID = userid;
                            swbs.UpdateTime = DateTime.Now;
                            swbs.Money = 0 + gold;
                            swbs.GameID = GameId;
                            swbs.Permission = 1;
                            try
                            {
                                new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 0);//0已领
                                int id = new BCW.SWB.BLL().Add(swbs);
                                builder.Append("派奖成功！");
                            }
                            catch { builder.Append("派奖失败！"); }
                        }
                        else
                        {
                            BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(userid, GameId);
                            builder.Append("当前测试币：" + swb.Money + "<BR/>");
                            swb.UpdateTime = DateTime.Now;
                            swb.Money += gold;
                            swb.Permission += 1;
                            builder.Append("领取后测试币：" + swb.Money + "<BR/>");
                            try
                            {
                                new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(model.Id), 0);//0已领
                                new BCW.SWB.BLL().Update(swb);
                                builder.Append("派奖成功！");
                            }
                            catch { builder.Append("派奖失败！"); }
                        }
                    }
                    else//维护
                    { Utils.Safe("此游戏"); }
                }
            }
            else
            {
                builder.Append("参数错误，兑奖失败！");
            }
            builder.Append("<br/><a href=\"" + Utils.getUrl("Winners.aspx?act=list&amp;backurl=" + Utils.getPage(0) + "") + "\">返回继续派奖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        //builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        //builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>" + "<br/>");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //数据分析
    private void DataPage()
    {
        Master.Title = "数据分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>&gt;数据分析");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            //今日
            string str = "Ident=0  and Year(AddTime) = " + DateTime.Now.Year + "" + " and Month(AddTime) = " + DateTime.Now.Month + "and Day(AddTime) = " + DateTime.Now.Day;
            DataSet ds = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allToday", str);
            DataSet ds1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as allTodayMan", str);
            //  builder.Append("[时间][价值][记录数]"+"<br/>"); 
            builder.Append("【今日派送】" + ds.Tables[0].Rows[0]["allToday"] + ub.Get("SiteBz"));
            // builder.Append("【今日派送】" + ds.Tables[0].Rows[0]["allToday"] + ub.Get("SiteBz") + "[" + ds1.Tables[0].Rows[0]["allTodayMan"] + "人]");
            builder.Append("<br/>");
        }
        catch
        { builder.Append("【今日派送】" + 0 + ub.Get("SiteBz")); }
        try
        {
            //昨日
            string strYesterday = "Ident=0 and Year(AddTime) = " + (DateTime.Now.Year) + "and Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + (DateTime.Now.Day - 1) + "";
            DataSet dsYesterday = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allYesterday", strYesterday);
            DataSet dsYesterday1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as allYesterdayMan", strYesterday);
            builder.Append("【昨日派送】" + dsYesterday.Tables[0].Rows[0]["allYesterday"] + ub.Get("SiteBz"));
            // builder.Append("【昨日派送】" + dsYesterday.Tables[0].Rows[0]["allYesterday"] + ub.Get("SiteBz") + "[" + dsYesterday1.Tables[0].Rows[0]["allYesterdayMan"] + "人]");
        }
        catch { builder.Append("【昨日派送】" + 0 + ub.Get("SiteBz")); }
        try
        {
            //本周
            builder.Append("<br/>");
            string strWeek = "Ident=0 and datediff(week,[AddTime],getdate())=0  ";// Year(AddTime) =+(DateTime.Now.Year) + " and Week(AddTime) = " + (DateTime.Now.DayOfWeek) + "";
            DataSet dsWeek = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allWeek", strWeek);
            DataSet dsWeek1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as allWeekMan", strWeek);
            builder.Append("【本周派送】" + dsWeek.Tables[0].Rows[0]["allWeek"] + ub.Get("SiteBz"));
            //   builder.Append("【本月派送】" + dsWeek.Tables[0].Rows[0]["allMonth"] + ub.Get("SiteBz") + "[" + dsWeek1.Tables[0].Rows[0]["allMonthMan"] + "]");
            builder.Append("<br/>");
        }
        catch { builder.Append("【本周派送】" + 0 + ub.Get("SiteBz")); }
        try
        {
            //本月
            string strMonth = "Ident=0 and Year(AddTime) = " + (DateTime.Now.Year) + " and Month(AddTime) = " + (DateTime.Now.Month) + "";
            DataSet dsMonth = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allMonth", strMonth);
            DataSet dsMonth1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as allMonthMan", strMonth);
            builder.Append("【本月派送】" + dsMonth.Tables[0].Rows[0]["allMonth"] + ub.Get("SiteBz"));
            //   builder.Append("【本月派送】" + dsMonth.Tables[0].Rows[0]["allMonth"] + ub.Get("SiteBz") + "[" + dsMonth1.Tables[0].Rows[0]["allMonthMan"] + "]");
            builder.Append("<br/>");
        }
        catch { builder.Append("【本月派送】" + 0 + ub.Get("SiteBz")); }
        try
        {
            string strAllnot = "Ident=1";
            DataSet dsAllnot = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allALLnot", strAllnot);
            DataSet dsAllnot1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as strAllnotMan", strAllnot);
            builder.Append("【未领取额】" + dsAllnot.Tables[0].Rows[0]["allALLnot"] + ub.Get("SiteBz"));
            // builder.Append("【总派送】" + dsAll.Tables[0].Rows[0]["allALL"] + ub.Get("SiteBz") + "[" + dsAllnot1.Tables[0].Rows[0]["strAllnotMan"] + "人]");
            builder.Append("<br/>");
        }
        catch
        { builder.Append("【未领取额】" + 0 + ub.Get("SiteBz")); }
        try
        {
            string strAllPass = "Ident=2";
            DataSet dsAllPass = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allALLPass", strAllPass);
            DataSet dsAllPass1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as strAllPassMan", strAllPass);
            builder.Append("【已过期额】" + dsAllPass.Tables[0].Rows[0]["allALLPass"] + ub.Get("SiteBz"));
            // builder.Append("【已过期额】" + dsAllPass.Tables[0].Rows[0]["allALLPass"] + ub.Get("SiteBz") + "[" + dsAllPass1.Tables[0].Rows[0]["strAllPassMan"] + "人]");
            builder.Append("<br/>");
        }
        catch { builder.Append("【已过期额】" + 0 + ub.Get("SiteBz")); }
        try
        {
            //总
            //  string strWhere = "";
            string strAll = "Ident=0";
            if (start != "0")
            {
                strAll = strAll + " and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
            }
            else
            {
                start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            DataSet dsAll = new BCW.BLL.tb_WinnersLists().GetList("sum(winGold) as allALL", strAll);
            DataSet dsAll1 = new BCW.BLL.tb_WinnersLists().GetList("count(*) as allALLMan", strAll);
            builder.Append("【总派送额】" + dsAll.Tables[0].Rows[0]["allALL"] + ub.Get("SiteBz"));
            //builder.Append("【总派送】" + dsAll.Tables[0].Rows[0]["allALL"] + ub.Get("SiteBz") + "[" + dsAll1.Tables[0].Rows[0]["allALLMan"] + "人]");
            builder.Append(Out.Tab("</div>", ""));
        }
        catch
        { builder.Append("【总派送额】" + 0 + ub.Get("SiteBz")); }
        string strText1 = "开始时间:,结束时间:,";
        string strName1 = "start,down,backurl";
        string strType1 = "text,text,hidden";
        string strValu1 = start + "'" + down + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,true，false";
        string strIdea1 = "/";
        string strOthe1 = "按时间搜索,Winners.aspx?act=data,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        //builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>");
        ////  builder.Append("<a href=\"" + Utils.getUrl("kbyg.aspx") + "\">" + GameName + "</a>");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //奖池数据
    private void WinnersAward()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=listsss;") + "\">" + GameName + "</a>&gt;");
        builder.Append("管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[0-9]\d*$", "0"));
        try
        {
            string text = "";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
            int ptype = 5;
            if (ptype == 1)
            {
                builder.Append("记录|");
                // BuyLists();
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=list&amp;") + "\">记录</a>|");

            }
            if (ptype == 2)
            {
                builder.Append("未领|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=notget") + "\">未领</a>|");
            }
            if (ptype == 3)
            {
                builder.Append("已领|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=get") + "\">已领</a>|");
            }
            if (ptype == 4)
            {
                builder.Append("过期|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=pass") + "\">过期</a>|");
            }
            if (ptype == 5)
            {
                builder.Append("奖池");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">奖池</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div  class=\"text\">", ""));
            string strWhere = "";
            int maxID = 0;//= new BCW.BLL.tb_WinnersAward().GetMaxId()-1;
            maxID = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(game.ToString());
            if (uid == 0)
            {

                strWhere = "awardId=" + maxID;
            }
            else
            {
                maxID = uid;
                strWhere = "awardId=" + uid;
            }
            BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(maxID);
            if (uid > 0)
            { game = int.Parse(model.getRedy); }
            if (game == 0)
            {
                builder.Append("<b>奖池一</b>|");
                text = "奖池一";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=0&amp;ptype=5&amp;") + "\">奖池一</a>|");
            if (game == 1)
            {
                builder.Append("<b>奖池二</b>|");
                text = "奖池二";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=1&amp;ptype=5&amp;") + "\">奖池二</a>|");
            if (game == 2)
            {
                builder.Append("<b>奖池三</b>|");
                text = "奖池三";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=2&amp;ptype=5&amp;") + "\">奖池三</a>|");
            if (game == 3)
            {
                builder.Append("<b>奖池四</b>|");
                text = "奖池四";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=3&amp;ptype=5&amp;") + "\">奖池四</a>|");
            if (game == 4)
            {
                builder.Append("<b>奖池五</b>");
                text = "奖池五";
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award&amp;game=4&amp;ptype=5&amp;") + "\">奖池五</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=" + game + "&amp;") + "\">配置" + text + "</a> | ");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addaward&amp;game=" + game + "&amp;") + "\">生成新" + text + "</a><br/>");

            string ttype = model.getRedy;
            string ttypetext = "";
            if (ttype == "0")
            {
                ttypetext = "奖池一";
            }
            if (ttype == "1")
            {
                ttypetext = "奖池二";
            }
            if (ttype == "2")
            {
                ttypetext = "奖池三";
            }
            if (ttype == "3")
            {
                ttypetext = "奖池四";
            }
            if (ttype == "4")
            {
                ttypetext = "奖池五";
            }
            builder.Append("当前奖池数据ID:" + model.Id + "<br/>");
            builder.Append("本轮总数：" + model.periods);
            // builder.Append("[表示每" + model.periods + "个玩家中将有" + model.awardNumber + "个获奖]"); 
            builder.Append("<br/>");
            builder.Append("本轮派送：" + model.awardNumber);
            builder.Append("<br/>");
            builder.Append("过期时间：" + model.identification + "小时");
            //builder.Append("[表示每" + model.periods + "个玩家中将有" + model.awardNumber + "个获奖]"); 
            builder.Append("<br/>");
            builder.Append("所属类型：" + ttypetext);
            builder.Append("<br/>");
            builder.Append("本轮剩余：" + model.awardNowCount + "[表示总量" + model.awardNumber + "剩余" + model.awardNowCount + "]" + "<br/>");
            string[] dengjiangshu = model.getWinNumber.Split('#');//数据等奖数处理提出处理 
            string[] dengjiangshuzong = model.winNowCount.Split('#');//数据等奖数处理提出处理    
            string[] awaard = model.award.Split('#');//数据等奖数处理提出处理    
            for (int j = 0; j < dengjiangshu.Length - 1; j++)
            {
                //if (dengjiangshu[j].Contains("-"))
                //{
                //    dengjiangshu[j] = "0";
                //}
                builder.Append("<b>" + (j + 1) + "</b>" + "等奖" + awaard[j] + " " + ub.Get("SiteBz") + ",总" + dengjiangshuzong[j] + "份,剩" + dengjiangshu[j] + "份.");
                builder.Append("<br/>");
            }

            builder.Append("会员进入数: " + model.isDone + " 中奖数: " + (model.awardNumber - model.awardNowCount) + "<br/>");

            // builder.Append("每等奖奖金：" + model.award); builder.Append("<br/>");
            //  builder.Append("内线语句：" + model.Remarks);
            builder.Append("");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("本期中奖记录列表:");
            builder.Append(Out.Tab("</div>", "<br/>"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            //  string strWhere = "";
            string[] pageValUrl = { "act", "backurl", "uid", "game" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                    builder.Append(k + ".");
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                    //  builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>(" + n.UserId + ")");
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                    builder.Append("中获得" + n.GetId + "等奖," + "价值" + n.winGold + "币" + "[奖池ID" + n.awardId + "#编号" + n.Id + "]" + Convert.ToDateTime(n.AddTime).ToString("MM-dd HH:mm:ss"));
                    if (n.Ident == 1)
                    { builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=paijiang&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[派]</a>"); }
                    else
                        if (n.Ident == 0)
                        { builder.Append("[已领]"); }
                        else
                        { builder.Append("[已过期]"); }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无相关记录.");
                builder.Append(Out.Tab("</div>", ""));
            }
            string strText = "输入奖池ID:/,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜该期记录,Winners.aspx?act=award,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("温馨提示:总数与派送数表示每" + model.periods + "个玩家中将有" + model.awardNumber + "个获奖");
            builder.Append(Out.Tab("</div>", ""));
        }
        catch
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("无相关记录.");
            builder.Append(Out.Tab("</div>", ""));
        }
        Footer();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //生成新一轮奖池数据
    private void WinnersAddAward()
    {
        Master.Title = GameName + "生成新奖池";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("生成新奖池");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string type = Utils.GetRequest("type", "all", 1, "", "");
        int MaxCount = Convert.ToInt32(ub.GetSub("MaxCount", xmlPath));//设置下一轮奖池最大份数
        int paisong = Convert.ToInt32(ub.GetSub("paisong", xmlPath));//设置下一轮奖池派送份数
        int CountList = Convert.ToInt32(ub.GetSub("CountList", xmlPath));//设置等奖数
        int baifenbi = Convert.ToInt32(ub.GetSub("baifenbi", xmlPath));//玩家中奖百分比
        string CountListNumber = (ub.GetSub("CountListNumber", xmlPath));//设置每等奖份数
        string ListIGold = (ub.GetSub("ListIGold", xmlPath));//设置每等奖金额
        int WinnersOpenChoose = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", xmlPath));//开放选择（int）
        string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
        int PassTime = Convert.ToInt32(ub.GetSub("PassTime", xmlPath));//设置领取过期时间(天)
        int maxID = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;
        BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(maxID);
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[0-9]\d*$", "0"));
        string text = "";
        if (game == 0)
        {
            text = "奖池一";
        }
        if (game == 1)
        {
            text = "奖池二";
        }
        if (game == 2)
        {
            text = "奖池三";
        }
        if (game == 3)
        {
            text = "奖池四";
        }
        if (game == 4)
        {
            text = "奖池五";
        }
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定生成<font color=\"red\">" + text + "</font>吗?");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string strText = ",";
            string strName = "type,backurl";
            string strType = "select,hidden";
            string strValu = game + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|游戏区奖池一|1|游戏区奖池二|2|游戏区奖池三|3|论坛奖池四|4|奖池五,false";
            string strIdea = "/";
            string strOthe = "确定生成,Winners.aspx?act=addaward&amp;info=ok&amp;game=" + game + "&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //  builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addaward&amp;info=ok&amp;") + "\">确定生成</a>");
            try
            {
                builder.Append(Out.Tab("", "<br/>"));
                builder.Append("当前奖池数据ID:" + model.Id + "<br/>");
                builder.Append("总数：" + model.periods + "<br/>");
                builder.Append("本轮剩余：" + model.awardNowCount + "<br/>");
                builder.Append("生成后奖池数据ID:" + (Convert.ToInt32(model.Id) + 1) + "<br/>");
            }
            catch { }
            if (game == 0)
            {
                text = "奖池一";
                PassTime = Convert.ToInt32(ub.GetSub("PassTime1", xmlPath));//设置领取过期时间(天)
                MaxCount = Convert.ToInt32(ub.GetSub("MaxCount1", xmlPath));//设置下一轮奖池最大份数
                paisong = Convert.ToInt32(ub.GetSub("paisong1", xmlPath));//设置下一轮奖池派送份数
                CountList = Convert.ToInt32(ub.GetSub("CountList1", xmlPath));//设置等奖数
                ListIGold = (ub.GetSub("ListIGold1", xmlPath));//设置每等奖金额
                CountListNumber = (ub.GetSub("CountListNumber1", xmlPath));//设置每等奖份数
            }
            if (game == 1)
            {
                text = "奖池二";
                PassTime = Convert.ToInt32(ub.GetSub("PassTime2", xmlPath));//设置领取过期时间(天)
                MaxCount = Convert.ToInt32(ub.GetSub("MaxCount2", xmlPath));//设置下一轮奖池最大份数
                paisong = Convert.ToInt32(ub.GetSub("paisong2", xmlPath));//设置下一轮奖池派送份数
                CountList = Convert.ToInt32(ub.GetSub("CountList2", xmlPath));//设置等奖数
                ListIGold = (ub.GetSub("ListIGold2", xmlPath));//设置每等奖金额
                CountListNumber = (ub.GetSub("CountListNumber2", xmlPath));//设置每等奖份数
            }
            if (game == 2)
            {
                text = "奖池三";
                PassTime = Convert.ToInt32(ub.GetSub("PassTime3", xmlPath));//设置领取过期时间(天)
                MaxCount = Convert.ToInt32(ub.GetSub("MaxCount3", xmlPath));//设置下一轮奖池最大份数
                paisong = Convert.ToInt32(ub.GetSub("paisong3", xmlPath));//设置下一轮奖池派送份数
                CountList = Convert.ToInt32(ub.GetSub("CountList3", xmlPath));//设置等奖数
                ListIGold = (ub.GetSub("ListIGold3", xmlPath));//设置每等奖金额
                CountListNumber = (ub.GetSub("CountListNumber3", xmlPath));//设置每等奖份数
            }
            if (game == 3)
            {
                text = "奖池四";
                PassTime = Convert.ToInt32(ub.GetSub("PassTime4", xmlPath));//设置领取过期时间(天)
                MaxCount = Convert.ToInt32(ub.GetSub("MaxCount4", xmlPath));//设置下一轮奖池最大份数
                paisong = Convert.ToInt32(ub.GetSub("paisong4", xmlPath));//设置下一轮奖池派送份数
                CountList = Convert.ToInt32(ub.GetSub("CountList4", xmlPath));//设置等奖数
                ListIGold = (ub.GetSub("ListIGold4", xmlPath));//设置每等奖金额
                CountListNumber = (ub.GetSub("CountListNumber4", xmlPath));//设置每等奖份数
            }
            if (game == 4)
            {
                text = "奖池五";
                PassTime = Convert.ToInt32(ub.GetSub("PassTime5", xmlPath));//设置领取过期时间(天)
                MaxCount = Convert.ToInt32(ub.GetSub("MaxCount5", xmlPath));//设置下一轮奖池最大份数
                paisong = Convert.ToInt32(ub.GetSub("paisong5", xmlPath));//设置下一轮奖池派送份数
                CountList = Convert.ToInt32(ub.GetSub("CountList5", xmlPath));//设置等奖数
                ListIGold = (ub.GetSub("ListIGold5", xmlPath));//设置每等奖金额
                CountListNumber = (ub.GetSub("CountListNumber5", xmlPath));//设置每等奖份数
            }
            builder.Append("下一轮奖池最大份数：" + "<font color=\"red\">" + MaxCount + "</font><br/>");
            builder.Append("下一轮奖池派送份数：" + "<font color=\"red\">" + paisong + "</font><br/>");
            builder.Append("下一轮奖池等奖量数：" + "<font color=\"red\">" + CountList + "</font><br/>");
            builder.Append("下一轮奖池等奖份数：" + "<font color=\"red\">" + CountListNumber + "</font><br/>");
            builder.Append("下一轮奖池等奖金额：" + "<font color=\"red\">" + ListIGold + "</font><br/>");
            builder.Append("下一轮领奖过期时间：" + PassTime + "小时" + "<br/>");
            builder.Append("下一轮奖池内线文字：" + TextForUbb + "");
            builder.Append("<br/><a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            try
            {

                {
                    AddAward(type);
                    Utils.Success("生成新奖池", "生成新奖池成功,3s后返回" + text + "...", Utils.getUrl("Winners.aspx?act=award&amp;game=" + game + "&amp;"), "3");
                }
            }
            catch
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("奖池生成失败,请重新生成！");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //全部中奖记录
    private void WinnersList()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=listsss;") + "\">" + GameName + "</a>&gt;");
        builder.Append("管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        int getID = Utils.ParseInt(Utils.GetRequest("getID", "all", 1, @"^[0-9]\d*$", "0"));//等奖数查找   
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=list;") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("未领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=notget") + "\">未领</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("已领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=get") + "\">已领</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("过期|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=pass") + "\">过期</a>|");
        }
        if (ptype == 5)
        {
            builder.Append("奖池");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">奖池</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
        //   int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string strWhere = "";
        //try
        //{

        //    if (start != "0")
        //    {
        //        strWhere = "AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
        //    }
        //    else
        //    {
        //        start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //        down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    }
        //}
        //catch { builder.Append("输入的时间格式有误."); }
        if (start.Length > 1 || uid > 0)
        {
            if (uid > 0)
            {
                strWhere = "UserId=" + uid + "and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
                if (getID != 0)
                {
                    strWhere += " and getID=" + getID;
                }
            }
            else
            {
                strWhere = "AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(down) + "'";
                if (getID != 0)
                {
                    strWhere += " and getID=" + getID;
                }
            }
        }
        else
        {
            start = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss");
            down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        //  string strWhere = "";
        //if (uid != 0)
        //{
        //    strWhere = "UserId=" + uid;
        //}
        string[] pageValUrl = { "act", "backurl", "uid", "start", "down", "ptype", "getID" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                builder.Append(k + ".");
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));//Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "")
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                //  builder.Append("中获得了" + n.GetId + "等奖");
                //  builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>("+n.UserId+")");
                if (n.GetId <= 3)
                {
                    builder.Append("获得 " + n.GetId + " 等奖," + "价值 <font color=\"red\">" + n.winGold + "</font>");
                }
                else
                builder.Append("获得 " + n.GetId + " 等奖," + "价值 " + n.winGold);
                if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                {
                    builder.Append(ub.Get("SiteBz"));
                }
                else
                {
                    builder.Append(" 活跃值");
                }
                builder.Append("[奖池" + n.awardId + "#" + "编号" + n.Id + "]");//+ Convert.ToDateTime(n.AddTime).ToString("MM-dd HH:mm"))
                if (n.Ident == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=paijiang&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[派]</a>");
                }
                else
                    if (n.Ident == 0)
                    {
                        builder.Append("[已领]");
                    }
                    else
                    {
                        builder.Append("[已过期]");
                    }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else { builder.Append("暂无相关记录."); }
        string strText1 = "用户:,等奖:,开始:,结束:,";
        string strName1 = "uid,getID,start,down,backurl";
        string strType1 = "num,select,text,text,hidden";
        string strValu1 = uid + "'" + getID + "'" + start + "'" + down + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,0|全部等奖|1|一等奖|2|二等奖|3|三等奖|4|四等奖|5|五等奖|6|六等奖|7|七等奖|8|八等奖|9|九等奖|10|十等奖,true,true,false";
        string strIdea1 = "";
        string strOthe1 = "确定搜索,Winners.aspx?act=list&amp;,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        builder.Append(Out.Tab("", "<br />"));
        Footer();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //中奖未领取记录
    private void WinnersListNotGet()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = 2;
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=list&amp;") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("未领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=notget") + "\">未领</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("已领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=get") + "\">已领</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("过期|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=pass") + "\">过期</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("奖池");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">奖池</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Ident=1";
        if (uid == 0)
        { strWhere = "Ident=1"; }
        else
        {
            strWhere = "UserId=" + uid;
        }
        string[] pageValUrl = { "act", "backurl", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                builder.Append(k + ".");
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                //   builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>");
                builder.Append("获得" + n.GetId + "等奖," + "价值" + n.winGold);
                if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                { builder.Append(ub.Get("SiteBz")); }
                else
                { builder.Append(" 活跃值"); }
                builder.Append("[奖池" + n.awardId + "#" + "编号" + n.Id + "]");
                // builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">[派]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=paijiang&amp;ptype=" + n.Id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[派]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else { builder.Append("暂无相关记录."); }
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜中奖记录,Winners.aspx?act=notget,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", "<br />"));
        Footer();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //中奖已领取记录
    private void WinnersGet()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = 3;
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=list&amp;") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("未领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=notget") + "\">未领</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("已领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=get") + "\">已领</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("过期|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=pass") + "\">过期</a>|");
        }
        if (ptype == 5)
        {
            builder.Append("奖池");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">奖池</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Ident=0";
        if (uid == 0)
        { strWhere = "Ident = 0"; }
        else
        {
            strWhere = "UserId=" + uid + "and Ident=0 ";
        }
        string[] pageValUrl = { "act", "backurl", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                builder.Append(k + ".");
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                //   builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">" + mename + "</a>");
                builder.Append("获得" + n.GetId + "等奖," + "价值" + n.winGold);
                if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                { builder.Append(ub.Get("SiteBz")); }
                else
                { builder.Append(" 活跃值"); }
                builder.Append("[奖池" + n.awardId + "#" + "编号" + n.Id + "]");
                if (n.Ident == 0)
                { builder.Append("[已领]"); }
                else if (n.Ident == 1)
                { builder.Append("[未领]"); }
                else if (n.Ident == 2)
                { builder.Append("[过期]"); }

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else { builder.Append("暂无相关记录."); }
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜中奖记录,Winners.aspx?act=get,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", "<br />"));
        Footer();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //已过期
    private void PassPage()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = 4;
        if (ptype == 1)
        {
            builder.Append("记录|");
            // BuyLists();
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=list&amp;") + "\">记录</a>|");

        }
        if (ptype == 2)
        {
            builder.Append("未领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=notget") + "\">未领</a>|");
        }
        if (ptype == 3)
        {
            builder.Append("已领|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=get") + "\">已领</a>|");
        }
        if (ptype == 4)
        {
            builder.Append("过期|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=pass") + "\">过期</a>|");
        }
        if (ptype == 5)
        {
            builder.Append("奖池");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=award") + "\">奖池</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        try
        {
            DataSet dtpass = new BCW.BLL.tb_WinnersLists().GetList("*", "Ident=1");
            DateTime dt = DateTime.Now;
            if (dtpass != null && dtpass.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dtpass.Tables[0].Rows.Count; i++)
                {
                    //  TimeSpan passtime = Convert.ToDateTime(dtpass.Tables[0].Rows[i]["AddTime"]).AddDays(Convert.ToInt32(dtpass.Tables[0].Rows[i]["overTime"])) - dt;
                    if (DateTime.Compare(Convert.ToDateTime(dtpass.Tables[0].Rows[i]["AddTime"]).AddDays(Convert.ToInt32(dtpass.Tables[0].Rows[i]["overTime"])), dt) <= 0)//已过期
                    {
                        new BCW.BLL.tb_WinnersLists().UpdateIdent(Convert.ToInt32(dtpass.Tables[0].Rows[i]["Id"]), 2);//更新过期标识
                    }
                }
            }
        }
        catch { }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        if (uid == 0)
        { strWhere = "Ident = 2"; }
        else
        {
            strWhere = "UserId=" + uid + " and Ident=2";
        }
        string[] pageValUrl = { "act", "backurl", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_WinnersLists> listSSCpay = new BCW.BLL.tb_WinnersLists().Gettb_WinnersListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;

            foreach (BCW.Model.tb_WinnersLists n in listSSCpay)
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
                builder.Append(k + ".");
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.UserId));
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
                builder.Append("获得" + n.GetId + "等奖," + "价值" + n.winGold);
                if (ub.GetSub("WinnersStatus", xmlPath) == "0")//正常
                { builder.Append(ub.Get("SiteBz")); }
                else
                { builder.Append(" 活跃值"); }
                builder.Append("[奖池" + n.awardId + "#" + "编号" + n.Id + "]");
                builder.Append("(已过期)");
                //   builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UserId + "") + "\">[派]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else { builder.Append("暂无相关记录."); }
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜中奖记录,Winners.aspx?act=pass,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", "<br />"));
        Footer();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //游戏配置
    private void WinnersSet()
    {
        Master.Title = GameName + "设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("游戏设置");
        builder.Append(Out.Tab("</div>", ""));
        // builder.Append(Out.Tab("<div class=\"text\">", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/winners.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string WinnersNotes = Utils.GetRequest("WinnersNotes", "post", 3, @"^[^\^]{1,2000}$", "得主限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,20000}$", "Logo地址限200字内");
            string WinnersGonggao = Utils.GetRequest("WinnersGonggao", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            //   string PassTime = Utils.GetRequest("PassTime", "post", 2, @"^[0-9]\d*$", "设置领取过期时间");
            //string MaxCount = Utils.GetRequest("MaxCount", "post", 4, @"^[1-9]\d*$", "设置下一轮奖池最大份数" + "填写错误(整数)");
            //string CountList = Utils.GetRequest("CountList", "post", 4, @"^[1-9]\d*$", "设置等奖数" + "填写错误");
            //// string baifenbi = Utils.GetRequest("baifenbi", "post", 2, @"^[1-9]\d*$", "玩家中奖百分比");
            ////   string WinnersTop = Utils.GetRequest("WinnersTop", "post", 3, @"^[\s\S]{1,2000}$", "游戏公告顶部Ubb限2000字内");
            //string paisong = Utils.GetRequest("paisong", "post", 2, @"^[1-9]\d*$", "设置下一轮奖池派送份数");
            ////   string WinnersOpenChoose = Utils.GetRequest("WinnersOpenChoose", "post", 2, @"^[0-3]$", "状态选择错误");
            ////   string WinnersOpenOrClose = Utils.GetRequest("WinnersOpenOrClose", "post", 2, @"^[0-1]$", "开放开关选择错误");
            //string CountListNumber = Utils.GetRequest("CountListNumber", "post", 2, @"^[^\^]{1,2000}$", "设置每等奖份数填写错误");
            //string ListIGold = Utils.GetRequest("ListIGold", "post", 2, @"^[^\^]{1,2000}$", "设置每等奖金额填写错误");
            string TextForUbb = Utils.GetRequest("TextForUbb", "post", 3, @"^[^\^]{1,2000}$", "设置内线提示的文字口号限2000字内");
            string maxGet = Utils.GetRequest("maxGet", "post", 2, @"^[^\^]{1,20}$", "每人每天最大获奖次数填写错误");
            string AllAwardCount = Utils.GetRequest("AllAwardCount", "post", 2, @"^[^\^]{1,20}$", "总奖池数填写错误");
            string AllAwardCountNow = Utils.GetRequest("AllAwardCountNow", "post", 2, @"^[^\^]{1,20}$", "最新总奖池数额填写错误");
            string OutText = Utils.GetRequest("OutText", "post", 1, @"^[^\^]{0,20000}$", "");
            xml.dss["WinnersName"] = Name;//游戏名称
            xml.dss["WinnersNotes"] = WinnersNotes;//游戏
            //  xml.dss["WinnersLogo"] = Logo;//Logo
            xml.dss["img"] = Logo;//Logo
            //   xml.dss["WinnersTop"] = WinnersTop;//游戏公告
            //  xml.dss["PassTime"] = PassTime;//设置领取过期时间
            xml.dss["WinnersGonggao"] = WinnersGonggao; // 公告        
            //xml.dss["MaxCount"] = MaxCount;//设置下一轮奖池最大份数
            //xml.dss["paisong"] = paisong;//设置下一轮奖池派送份数
            //xml.dss["CountList"] = CountList;//设置等奖数
            ////  xml.dss["baifenbi"] = baifenbi;//玩家中奖百分比
            //xml.dss["CountListNumber"] = CountListNumber;//设置每等奖份数
            //xml.dss["ListIGold"] = ListIGold;//设置每等奖金额
            xml.dss["TextForUbb"] = TextForUbb;//设置内线提示的文字
            xml.dss["AllAwardCount"] = AllAwardCount;//总奖池数 20160819
            xml.dss["AllAwardCountNow"] = AllAwardCountNow;//最新总奖池数 20160819
            xml.dss["maxGet"] = maxGet;//每人每天最大获取次数
            xml.dss["OutText"] = OutText;//提示的语句
            //  xml.dss["WinnersOpenChoose"] = WinnersOpenChoose;//开放选择(1全社区2游戏3友缘社区)
            // xml.dss["WinnersOpenOrClose"] = WinnersOpenOrClose;//开放选择（派奖1开与0关）
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("winners.aspx?act=set&amp;backurl=" + Utils.getPage(1) + ""), "2");
            // builder.Append("<br/>" + "设置成功!" + "<br/>");
            //  builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addaward") + "\">生成新奖池</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addaward") + "\">返回游戏配置</a><br/>");
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            string strText = "游戏名称:/,游戏Logo(可留空):/,游戏公告:/,上轮得主:/,内线提示的文字:/,总奖池:/,当前剩余总奖池:/,每人每天最大获奖次数:/,提示的语句(#分隔):/,";
            string strName = "Name,Logo,WinnersGonggao,WinnersNotes,TextForUbb,AllAwardCount,AllAwardCountNow,maxGet,OutText,backurl";
            string strType = "text,text,textarea,text,textarea,text,text,text,big,hidden";
            string strValu = xml.dss["WinnersName"] + "'" + xml.dss["img"] + "'" + xml.dss["WinnersGonggao"] + "'" + xml.dss["WinnersNotes"] + "'" + xml.dss["TextForUbb"] + "'" + xml.dss["AllAwardCount"] + "'" + xml.dss["AllAwardCountNow"] + "'" + xml.dss["maxGet"] + "'" + xml.dss["OutText"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,true,true,true,true,true,true,";
            string strIdea = "/";
            string strOthe = "确定修改|reset,Winners.aspx?act=set,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:" + "输入的数字#号间隔即可，下一轮范围内派送额等于每等奖份数总和." + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //游戏奖池配置设置
    private void WinnersAwardSet()
    {
        Master.Title = GameName + "奖池设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("奖池设置");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string text = "";
        int game = int.Parse(Utils.GetRequest("game", "all", 1, @"^[0-9]\d*$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/winners.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if ((ac) == "ok")
        {
            string PassTime = Utils.GetRequest("PassTime", "post", 2, @"^[0-9]\d*$", "设置领取过期时间");
            string MaxCount = Utils.GetRequest("MaxCount", "post", 4, @"^[1-9]\d*$", "设置下一轮奖池最大份数" + "填写错误(整数)");
            string CountList = Utils.GetRequest("CountList", "post", 4, @"^[1-9]\d*$", "设置等奖数" + "填写错误");
            // string baifenbi = Utils.GetRequest("baifenbi", "post", 2, @"^[1-9]\d*$", "玩家中奖百分比");
            // string WinnersTop = Utils.GetRequest("WinnersTop", "post", 3, @"^[\s\S]{1,2000}$", "游戏公告顶部Ubb限2000字内");
            string paisong = Utils.GetRequest("paisong", "post", 2, @"^[1-9]\d*$", "设置下一轮奖池派送份数");
            // string WinnersOpenChoose = Utils.GetRequest("WinnersOpenChoose", "post", 2, @"^[0-3]$", "状态选择错误");
            // string WinnersOpenOrClose = Utils.GetRequest("WinnersOpenOrClose", "post", 2, @"^[0-1]$", "开放开关选择错误");
            string CountListNumber = Utils.GetRequest("CountListNumber", "post", 2, @"^[^\^]{1,2000}$", "设置每等奖份数填写错误");
            string ListIGold = Utils.GetRequest("ListIGold", "post", 2, @"^[^\^]{1,2000}$", "设置每等奖金额填写错误");
            //string TextForUbb = Utils.GetRequest("TextForUbb", "post", 3, @"^[^\^]{1,2000}$", "设置内线提示的文字口号限2000字内");
            //string maxGet = Utils.GetRequest("maxGet", "post", 2, @"^[^\^]{1,20}$", "每人每天最大获奖次数填写错误");
            if (game == 0)
            {
                text = "奖池一";
                xml.dss["PassTime1"] = PassTime;//设置领取过期时间
                xml.dss["MaxCount1"] = MaxCount;//设置下一轮奖池最大份数
                xml.dss["paisong1"] = paisong;//设置下一轮奖池派送份数
                xml.dss["CountList1"] = CountList;//设置等奖数
                //  xml.dss["baifenbi"] = baifenbi;//玩家中奖百分比
                xml.dss["CountListNumber1"] = CountListNumber;//设置每等奖份数
                xml.dss["ListIGold1"] = ListIGold;//设置每等奖金额
            }
            if (game == 1)
            {
                text = "奖池二";
                xml.dss["PassTime2"] = PassTime;//设置领取过期时间
                xml.dss["MaxCount2"] = MaxCount;//设置下一轮奖池最大份数
                xml.dss["paisong2"] = paisong;//设置下一轮奖池派送份数
                xml.dss["CountList2"] = CountList;//设置等奖数
                //  xml.dss["baifenbi"] = baifenbi;//玩家中奖百分比
                xml.dss["CountListNumber2"] = CountListNumber;//设置每等奖份数
                xml.dss["ListIGold2"] = ListIGold;//设置每等奖金额
            }
            if (game == 2)
            {
                text = "奖池三";
                xml.dss["PassTime3"] = PassTime;//设置领取过期时间
                xml.dss["MaxCount3"] = MaxCount;//设置下一轮奖池最大份数
                xml.dss["paisong3"] = paisong;//设置下一轮奖池派送份数
                xml.dss["CountList3"] = CountList;//设置等奖数
                //  xml.dss["baifenbi"] = baifenbi;//玩家中奖百分比
                xml.dss["CountListNumber3"] = CountListNumber;//设置每等奖份数
                xml.dss["ListIGold3"] = ListIGold;//设置每等奖金额
                //xml.dss["TextForUbb3"] = TextForUbb;//设置内线提示的文字
                //xml.dss["maxGet3"] = maxGet;//每人每天最大获取次数
            }
            if (game == 3)
            {
                text = "奖池四";
                xml.dss["PassTime4"] = PassTime;//设置领取过期时间
                xml.dss["MaxCount4"] = MaxCount;//设置下一轮奖池最大份数
                xml.dss["paisong4"] = paisong;//设置下一轮奖池派送份数
                xml.dss["CountList4"] = CountList;//设置等奖数
                //  xml.dss["baifenbi"] = baifenbi;//玩家中奖百分比
                xml.dss["CountListNumber4"] = CountListNumber;//设置每等奖份数
                xml.dss["ListIGold4"] = ListIGold;//设置每等奖金额
                //xml.dss["TextForUbb4"] = TextForUbb;//设置内线提示的文字
                //xml.dss["maxGet4"] = maxGet;//每人每天最大获取次数
            }
            if (game == 4)
            {
                text = "奖池五";
                xml.dss["PassTime5"] = PassTime;//设置领取过期时间
                xml.dss["MaxCount5"] = MaxCount;//设置下一轮奖池最大份数
                xml.dss["paisong5"] = paisong;//设置下一轮奖池派送份数
                xml.dss["CountList5"] = CountList;//设置等奖数
                //  xml.dss["baifenbi"] = baifenbi;//玩家中奖百分比
                xml.dss["CountListNumber5"] = CountListNumber;//设置每等奖份数
                xml.dss["ListIGold5"] = ListIGold;//设置每等奖金额
                //xml.dss["TextForUbb5"] = TextForUbb;//设置内线提示的文字
                //xml.dss["maxGet5"] = maxGet;//每人每天最大获取次数
            }

            //  xml.dss["WinnersOpenChoose"] = WinnersOpenChoose;//开放选择(1全社区2游戏3友缘社区)
            // xml.dss["WinnersOpenOrClose"] = WinnersOpenOrClose;//开放选择（派奖1开与0关）
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            builder.Append(Out.Tab("<div class=\"text\">", ""));

            // Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("winners.aspx?act=set&amp;backurl=" + Utils.getPage(1) + ""), "1");
            builder.Append("<font color=\"red\">" + text + "配置设置成功!</font>" + " <br/>");
            //  builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append("是否生成<font color=\"red\">" + text + "</font>?" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=addaward&amp;game=" + game + "&amp;") + "\">是,前往生成新" + text + "</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=" + game + "&amp;") + "\">否,返回奖池配置</a><br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            string passtime = xml.dss["PassTime1"].ToString();
            string MaxCount = xml.dss["MaxCount1"].ToString();
            string paisong = xml.dss["paisong1"].ToString();
            string CountList = xml.dss["CountList1"].ToString();
            string CountListNumber = xml.dss["CountListNumber1"].ToString();
            string ListIGold = xml.dss["ListIGold1"].ToString();
            builder.Append(Out.Tab("<div  class=\"text\">", ""));
            if (game == 0)
            {
                builder.Append("<b>奖池一</b>|");
                text = "奖池一";
                passtime = xml.dss["PassTime1"].ToString();
                MaxCount = xml.dss["MaxCount1"].ToString();
                paisong = xml.dss["paisong1"].ToString();
                CountList = xml.dss["CountList1"].ToString();
                CountListNumber = xml.dss["CountListNumber1"].ToString();
                ListIGold = xml.dss["ListIGold1"].ToString();
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=0&amp;ptype=5&amp;") + "\">奖池一</a>|");
            if (game == 1)
            {
                builder.Append("<b>奖池二</b>|");
                text = "奖池二";
                passtime = xml.dss["PassTime2"].ToString();
                MaxCount = xml.dss["MaxCount2"].ToString();
                paisong = xml.dss["paisong2"].ToString();
                CountList = xml.dss["CountList2"].ToString();
                CountListNumber = xml.dss["CountListNumber2"].ToString();
                ListIGold = xml.dss["ListIGold2"].ToString();
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=1&amp;ptype=5&amp;") + "\">奖池二</a>|");
            if (game == 2)
            {
                builder.Append("<b>奖池三</b>|");
                text = "奖池三";
                passtime = xml.dss["PassTime3"].ToString();
                MaxCount = xml.dss["MaxCount3"].ToString();
                paisong = xml.dss["paisong3"].ToString();
                CountList = xml.dss["CountList3"].ToString();
                CountListNumber = xml.dss["CountListNumber3"].ToString();
                ListIGold = xml.dss["ListIGold3"].ToString();
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=2&amp;ptype=5&amp;") + "\">奖池三</a>|");
            if (game == 3)
            {
                builder.Append("<b>奖池四</b>|");
                text = "奖池四";
                passtime = xml.dss["PassTime4"].ToString();
                MaxCount = xml.dss["MaxCount4"].ToString();
                paisong = xml.dss["paisong4"].ToString();
                CountList = xml.dss["CountList4"].ToString();
                CountListNumber = xml.dss["CountListNumber4"].ToString();
                ListIGold = xml.dss["ListIGold4"].ToString();
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=3&amp;ptype=5&amp;") + "\">奖池四</a>|");
            if (game == 4)
            {
                builder.Append("<b>奖池五</b>");
                text = "奖池五";
                passtime = xml.dss["PassTime5"].ToString();
                MaxCount = xml.dss["MaxCount5"].ToString();
                paisong = xml.dss["paisong5"].ToString();
                CountList = xml.dss["CountList5"].ToString();
                CountListNumber = xml.dss["CountListNumber5"].ToString();
                ListIGold = xml.dss["ListIGold5"].ToString();
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=4&amp;ptype=5&amp;") + "\">奖池五</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "领奖过期时间(小时):/,下一轮奖池最大份数(总范围):/,下一轮派送额:/,等奖数(如10即1-10等奖):/,每等奖份数(#号隔开):/,每等奖金额(#号隔开):/,";
            string strName = "PassTime,MaxCount,paisong,CountList,CountListNumber,ListIGold,backurl";
            string strType = "text,num,num,num,textarea,textarea,hidden";
            string strValu = passtime + "'" + MaxCount + "'" + paisong + "'" + CountList + "'" + CountListNumber + "'" + ListIGold + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,true,true,true,";
            string strIdea = "/";
            string strOthe = "确定设置" + text + "|reset,Winners.aspx?act=awardset&amp;ac=ok&amp;game=" + game + "&amp;,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:" + "输入份数#号间隔,下一轮范围内派送额等于每等奖份数总和." + "<br />" + "奖池一, 二, 三,五可用于游戏奖池, 四为论坛区奖池");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("Winners.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //重置游戏
    private void ResetPage()
    {
        Master.Title = GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(GameName + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        // builder.Append(Out.Tab("<div class=\"text\">", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定重置" + GameName + "吗，重置后将删除所有记录！");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_WinnersLists");
            new BCW.Data.SqlUp().ClearTable("tb_WinnersAward");
            new BCW.Data.SqlUp().ClearTable("tb_WinnersGame");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("Winners.aspx"), "1");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("Winners.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    #region 生成新一轮奖池数据
    private void AddAward(string type)
    {
        // Utils.Error(""+type+"","");
        /// <summary>
        /// 生成最新一期奖池数据
        /// </summary>
        /// 
        try
        {
            //设置下一轮奖池最大份数
            int MaxCount = Convert.ToInt32(ub.GetSub("MaxCount", xmlPath));
            //设置下一轮奖池派送份数
            int paisong = Convert.ToInt32(ub.GetSub("paisong", xmlPath));
            //设置等奖数
            int CountList = Convert.ToInt32(ub.GetSub("CountList", xmlPath));
            //玩家中奖百分比
            int baifenbi = Convert.ToInt32(ub.GetSub("baifenbi", xmlPath));
            //设置每等奖份数
            string CountListNumber = (ub.GetSub("CountListNumber", xmlPath));
            //设置每等奖金额
            string ListIGold = (ub.GetSub("ListIGold", xmlPath));
            //开放选择（int）
            int WinnersOpenChoose = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", xmlPath));
            //设置内线提示的文字
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));
            //设置领取过期时间(天)
            int PassTime = Convert.ToInt32(ub.GetSub("PassTime", xmlPath));
            //当前ACTion最大ID
            int nowId = new BCW.BLL.Action().GetMaxId() - 1;
            Random ran = new Random();
            //当前派送量
            int awardNumber1 = paisong;
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            if (Convert.ToInt32(type) == 0)//加载奖池1配置
            {
                PassTime = Convert.ToInt32(xml.dss["PassTime1"]);
                MaxCount = Convert.ToInt32(xml.dss["MaxCount1"]);
                paisong = Convert.ToInt32(xml.dss["paisong1"]);
                CountList = Convert.ToInt32(xml.dss["CountList1"]);
                CountListNumber = xml.dss["CountListNumber1"].ToString();
                ListIGold = xml.dss["ListIGold1"].ToString();
            }
            if (Convert.ToInt32(type) == 1)//加载奖池2配置
            {
                PassTime = Convert.ToInt32(xml.dss["PassTime2"]);
                MaxCount = Convert.ToInt32(xml.dss["MaxCount2"]);
                paisong = Convert.ToInt32(xml.dss["paisong2"]);
                CountList = Convert.ToInt32(xml.dss["CountList2"]);
                CountListNumber = xml.dss["CountListNumber2"].ToString();
                ListIGold = xml.dss["ListIGold2"].ToString();
            }
            if (Convert.ToInt32(type) == 2)//加载奖池3配置
            {
                PassTime = Convert.ToInt32(xml.dss["PassTime3"]);
                MaxCount = Convert.ToInt32(xml.dss["MaxCount3"]);
                paisong = Convert.ToInt32(xml.dss["paisong3"]);
                CountList = Convert.ToInt32(xml.dss["CountList3"]);
                CountListNumber = xml.dss["CountListNumber3"].ToString();
                ListIGold = xml.dss["ListIGold3"].ToString();
            }
            if (Convert.ToInt32(type) == 3)//加载奖池4配置
            {
                PassTime = Convert.ToInt32(xml.dss["PassTime4"]);
                MaxCount = Convert.ToInt32(xml.dss["MaxCount4"]);
                paisong = Convert.ToInt32(xml.dss["paisong4"]);
                CountList = Convert.ToInt32(xml.dss["CountList4"]);
                CountListNumber = xml.dss["CountListNumber4"].ToString();
                ListIGold = xml.dss["ListIGold4"].ToString();
            }
            if (Convert.ToInt32(type) == 4)//加载奖池5配置
            {
                PassTime = Convert.ToInt32(xml.dss["PassTime5"]);
                MaxCount = Convert.ToInt32(xml.dss["MaxCount5"]);
                paisong = Convert.ToInt32(xml.dss["paisong5"]);
                CountList = Convert.ToInt32(xml.dss["CountList5"]);
                CountListNumber = xml.dss["CountListNumber5"].ToString();
                ListIGold = xml.dss["ListIGold5"].ToString();
            }

            string ttCountListNumber0 = CountListNumber + "#";
            string[] tepm_1 = (ttCountListNumber0).Split('#');//数据等奖数处理提出处理
            int sum = 0;
            for (int k = 0; k < tepm_1.Length - 1; k++)
            {
                sum += Convert.ToInt32(tepm_1[k]);
            }
            string[] dengjiangshu = ttCountListNumber0.Split('#');//数据等奖数处理提出处理  
            //Utils.Error("" + sum + "==" + paisong + "", "");
            if (sum != paisong)
            {
                Utils.Error("下一轮派送份数不等于每等奖派送份数和,奖池生成失败,请重新<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=" + type + "&amp;") + "\">配置</a>", "");
            }
            else
                if (MaxCount < paisong)
                {
                    Utils.Error("下一轮最大份数少于派送数,奖池生成失败,请重新<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=" + type + "&amp;") + "\">配置</a>！", "");
                }
                else if (PassTime < 0)
                {
                    Utils.Error("设置的下一轮奖池过期时间错误,奖池生成失败,请重新<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=" + type + "&amp;") + "\">配置</a>！", "");
                }
                else if ((dengjiangshu.Length - 1) != CountList)
                {
                    Utils.Error("设置的等奖数与#内的等奖数不等,奖池生成失败,请重新<a href=\"" + Utils.getUrl("Winners.aspx?act=awardset&amp;game=" + type + "&amp;") + "\">配置</a>！", "");
                }

            //不存在数据直接生成
            if (!new BCW.BLL.tb_WinnersAward().Exists(1))
            {
                BCW.Model.tb_WinnersAward tempAward = new BCW.Model.tb_WinnersAward();//奖池列
                string[] getWinN = (CountListNumber + "#").Split('#');//数据等奖数处理提出处理
                tempAward.periods = MaxCount;//奖池总数
                tempAward.addTime = DateTime.Now;//开始时间
                tempAward.awardNumber = paisong;//最大派出量
                tempAward.awardNowCount = paisong;//当前派出剩余量
                tempAward.award = ListIGold + "#";//设置每等奖份数  100#50#20...数量
                tempAward.getRedy = type;//012类型 游戏奖池 论坛奖池 总奖池
                tempAward.getUsId = "0";//获奖者，派送后递减ID
                tempAward.getWinNumber = CountListNumber + "#";//设置每等奖份数  数量,便于统计剩余等奖量 统计剩余用
                tempAward.identification = PassTime;//过期标识（单位天）
                tempAward.isDone = 0;//
                tempAward.isGet = 0;//
                tempAward.overTime = DateTime.Now;
                tempAward.Remarks = TextForUbb;//发内线的语句
                tempAward.winNowCount = CountListNumber + "#";//每等奖份数 不变
                tempAward.winNumber = CountList;//等奖份数，5即5等奖  
                int a = new BCW.BLL.tb_WinnersAward().Add(tempAward);//生成的新的中奖码保存
            }
            else//存在奖池数据
            {
                int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
                BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列 
                int awardNumber = MaxCount;//总奖数
                int NowNumber = paisong;//此次派出数量paisong
                // long start = System.currentTimeMillis(); 
                BCW.Model.tb_WinnersAward tempAward = new BCW.Model.tb_WinnersAward();//奖池列
                string ttCountListNumber = CountListNumber + "#";
                string[] getWinN = ttCountListNumber.Split('#');//数据等奖数处理提出处理
                tempAward.periods = MaxCount;//奖池总数
                tempAward.addTime = DateTime.Now;//开始时间
                tempAward.awardNumber = paisong;//最大派出量
                tempAward.awardNowCount = paisong;//当前派出剩余量
                tempAward.award = ListIGold + "#";//设置每等奖份数  100#50#20...数量
                tempAward.getRedy = type;//0 1 2类型 游戏奖池 论坛奖池 总奖池
                tempAward.getUsId = "0";//获奖者，派送后递减ID
                tempAward.getWinNumber = ttCountListNumber;//设置每等奖份数  数量,便于统计剩余等奖量 统计剩余用
                tempAward.identification = PassTime;//过期标识（单位天）
                tempAward.isDone = 0;//
                tempAward.isGet = 0;//
                tempAward.overTime = DateTime.Now;
                tempAward.Remarks = TextForUbb;//发内线的语句
                tempAward.winNowCount = ttCountListNumber;//每等奖份数 不变
                tempAward.winNumber = CountList;//等奖份数，5即5等奖  
                try
                {
                    int a = new BCW.BLL.tb_WinnersAward().Add(tempAward);//生成的新的中奖码保存
                    string gessText = "下一轮生成奖池数据成功，当前奖池ID" + a;
                    //   builder.Append("下一轮生成奖池数据成功"); 
                    new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);
                }
                catch (Exception ee)
                {
                    //  builder.Append(ee.ToString());
                    string gessText = "下一轮生成奖池数据失败！002" + ee;
                    new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);
                }
            }

        }
        catch (Exception ee)
        {
            //  builder.Append(ee.ToString());
            string gessText = "新生成奖池数据失败！001";
            //  new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);//异常报错
        }
    }
    #endregion

    #region 生成是否重复
    protected bool isGet(string r, string yungouma)
    {
        bool b = true;
        if (yungouma == "")
        { return b; }
        else
        {
            string[] yun = yungouma.Split(',');
            //foreach (string j in yun)
            for (int i = 0; i < yun.Length; i++)
            {
                // if (yun[i].ToString() == (r.ToString()))
                if (yungouma.IndexOf(r.ToString()) >= 0)
                {
                    b = false;
                }
            }
            return b;
        }

    }
    #endregion
}

