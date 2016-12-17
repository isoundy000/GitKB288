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

public partial class Manage_game_horse : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "top":
                TopPage();
                break;
            case "back":
                BackPage();
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "view":
                ViewPage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "stat":
                StatPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "跑马管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("跑马");
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
        IList<BCW.Model.Game.Horselist> listHorselist = new BCW.BLL.Game.Horselist().GetHorselists(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorselist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Horselist n in listHorselist)
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
                builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                    builder.Append("第" + n.ID + "局开出:<a href=\"" + Utils.getUrl("horse.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a>");
                else
                    builder.Append("第" + n.ID + "局开出:<a href=\"" + Utils.getUrl("horse.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Utils.ConvertSeparated(n.WinNum.ToString(), 1, "-") + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=top") + "\">游戏排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/horseset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void TopPage()
    {
        string strTitle = "跑马排行榜";
        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "bzType=0 and State>0";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Horsepay> listHorsepay = new BCW.BLL.Game.Horsepay().GetHorsepaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorsepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Horsepay n in listHorsepay)
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackPage()
    {
        Master.Title = "跑马返赢返负";
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
        string strOthe = "马上返赢,horse.aspx,post,1,red";

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
        strOthe = "马上返负,horse.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackSavePage(string act)
    {

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));

        if (act == "backsave")
        {
            DataSet ds = new BCW.BLL.Game.Horsepay().GetList("UsID,sum(WinCent-BuyCent) as WinCents", "AddTime>='" + sTime + "'and AddTime<'" + oTime + "' and bzType=0 and State>0 and IsSpier=0 group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "跑马返赢");
                    //发内线
                    string strLog = "根据你上期跑马排行榜上的赢利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/horse.aspx]进入跑马[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("horse.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.BLL.Game.Horsepay().GetList("UsID,sum(WinCent-BuyCent) as WinCents", "AddTime>='" + sTime + "'and AddTime<'" + oTime + "' and bzType=0 and State>0 and IsSpier=0 group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "跑马返负");
                    //发内线
                    string strLog = "根据你上期跑马排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/horse.aspx]进入跑马[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("horse.aspx"), "1");
        }


    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Horselist model = new BCW.BLL.Game.Horselist().GetHorselist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + id + "局跑马";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "HorseId=" + id + "";
        else
            strWhere += "HorseId=" + id + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Horsepay> listHorsepay = new BCW.BLL.Game.Horsepay().GetHorsepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorsepay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "局开出:<b>" + Utils.ConvertSeparated(model.WinNum.ToString(), 1, ",") + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Horsepay n in listHorsepay)
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

                string sText = "押[" + Utils.ConvertSeparated(n.Types.ToString(), 1, "-") + "]";

                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                if (n.State == 0)
                    builder.Append(sText + "，共" + n.BuyCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                else if (n.State == 1)
                {
                    builder.Append(sText + "，共" + n.BuyCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                }
                else
                    builder.Append(sText + "，共" + n.BuyCent + "" + bzTypes + "，赢" + n.WinCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]");


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "局开出:<b>" + Utils.ConvertSeparated(model.WinNum.ToString(), 1, ",") + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
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
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "跑马ID错误"));
        Master.Title = "编辑跑马";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑第" + id + "局跑马");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Horselist model = new BCW.BLL.Game.Horselist().GetHorselist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("当开出数字非0时，开奖时则使用该数字作为开奖结果");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "开出数字(如填写12即开出[1-2]):/,开盘时间:/,开奖时间:/,,,";
        string strName = "WinNum,BeginTime,EndTime,id,act,backurl";
        string strType = "num,date,date,hidden,hidden,hidden";
        string strValu = "" + model.WinNum + "'" + model.BeginTime + "'" + model.EndTime + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,horse.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本局</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
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
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int WinNum = int.Parse(Utils.GetRequest("WinNum", "post", 2, @"^[0-9]\d*$", "开出数字填写错误"));
        DateTime BeginTime = Utils.ParseTime(Utils.GetRequest("BeginTime", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

        if (!new BCW.BLL.Game.Horselist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //记录日志
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
            LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/编辑跑马期数:" + id + "|是否预设:" + WinNum + "");
        }

        BCW.Model.Game.Horselist model = new BCW.Model.Game.Horselist();
        model.ID = id;
        model.WinNum = WinNum;
        model.BeginTime = BeginTime;
        model.EndTime = EndTime;
        new BCW.BLL.Game.Horselist().Update(model);
        Utils.Success("编辑第" + id + "局", "编辑第" + id + "局成功..", Utils.getUrl("horse.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
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
            Master.Title = "删除第" + id + "局";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定第" + id + "局记录吗.删除同时将会删除该局的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Horselist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            //记录日志
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/删除跑马期数:" + id + "");
            }

            new BCW.BLL.Game.Horselist().Delete(id);
            new BCW.BLL.Game.Horsepay().Delete("HorseId=" + id + "");
            Utils.Success("删除第" + id + "局", "删除第" + id + "局成功..", Utils.getPage("horse.aspx"), "1");
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
            builder.Append("确定重置跑马游戏吗，重置后，将重新从第一局开始，所有记录(局数和下注记录)将会全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Horselist");
            new BCW.Data.SqlUp().ClearTable("tb_Horsepay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("horse.aspx"), "1");
        }
    }

    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("赢利分析(不计机器人)");
        builder.Append(Out.Tab("</div>", "<br />"));

        //今天本金与赢利
        long TodayBuyCent = new BCW.BLL.Game.Horsepay().GetSumBuyCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
        long TodayWinCent = new BCW.BLL.Game.Horsepay().GetSumWinCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

        //昨天本金与赢利
        long YesBuyCent = new BCW.BLL.Game.Horsepay().GetSumBuyCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
        long YesWinCent = new BCW.BLL.Game.Horsepay().GetSumWinCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");

        //本月本金与赢利
        long MonthBuyCent = new BCW.BLL.Game.Horsepay().GetSumBuyCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
        long MonthWinCent = new BCW.BLL.Game.Horsepay().GetSumWinCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");

        //上月本金与赢利
        long Month2BuyCent = new BCW.BLL.Game.Horsepay().GetSumBuyCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");
        long Month2WinCent = new BCW.BLL.Game.Horsepay().GetSumWinCent("BzType=0 and State>0 and IsSpier=0 and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");

        //总本金与赢利
        long BuyCent = new BCW.BLL.Game.Horsepay().GetSumBuyCent("BzType=0 and State>0 and IsSpier=0");
        long WinCent = new BCW.BLL.Game.Horsepay().GetSumWinCent("BzType=0 and State>0 and IsSpier=0");


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天下注:" + TodayBuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("今天返彩:" + TodayWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("今天净赢:" + (TodayBuyCent - TodayWinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天下注:" + YesBuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("昨天返彩:" + YesWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("昨天净赢:" + (YesBuyCent - YesWinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月下注:" + MonthBuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("本月返彩:" + MonthWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("本月净赢:" + (MonthBuyCent - MonthWinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月下注:" + Month2BuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("上月返彩:" + Month2WinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("上月净赢:" + (Month2BuyCent - Month2WinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总计下注:" + BuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("总计返彩:" + WinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("总计净赢:" + (BuyCent - WinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
