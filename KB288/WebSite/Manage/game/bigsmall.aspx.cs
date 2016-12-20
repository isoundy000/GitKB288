using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;

public partial class Manage_game_bigsmall : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
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
            case "clear":
                ClearPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "大小庄管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("大小庄");
        builder.Append(Out.Tab("</div>", "<br />"));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        if (uid > 0)
            strWhere += "usid=" + uid + "";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Bslist> listBslist = new BCW.BLL.Game.Bslist().GetBslists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBslist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Bslist n in listBslist)
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
                builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
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
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜庄,bigsmall.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=stat") + "\">赢利分析</a>|<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=9&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=clear") + "\">清空下注</a><br />");
        //builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/bigsmallset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>");
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId == 1 || ManageId == 11)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("../xml/bigsmallset.aspx?act=ok&amp;backurl=" + Utils.PostPage(1) + "") + "\">上帝之手</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void BackPage()
    {
        Master.Title = "大小庄返赢返负";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("返赢返负操作");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "返赢千分比:,至少赢多少才返:,";
        string strName = "iTar,iPrice,act";
        string strType = "num,num,hidden";
        string strValu = "''backsave";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,bigsmall.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "返负千分比:,至少负多少才返:,";
        strName = "iTar,iPrice,act";
        strType = "num,num,hidden";
        strValu = "''backsave2";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "马上返负,bigsmall.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bigsmall.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackSavePage(string act)
    {

        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("权限不足", "");
        }

        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));

        if (act == "backsave")
        {
            DataSet ds = new BCW.BLL.Toplist().GetList("UsID,sum(WinGold + PutGold) as WinCents", "Types=9 group by UsID order by sum(WinGold + PutGold) desc");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "大小庄返赢");
                    //发内线
                    string strLog = "根据你上期大小庄排行榜上的赢利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/bigsmall.aspx]进入大小庄[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("bigsmall.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.BLL.Toplist().GetList("UsID,sum(WinGold + PutGold) as WinCents", "Types=9 group by UsID order by sum(WinGold + PutGold) desc");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "大小庄返负");
                    //发内线
                    string strLog = "根据你上期大小庄排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/bigsmall.aspx]进入大小庄[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("bigsmall.aspx"), "1");
        }


    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "大小庄《" + model.Title + "》";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "BsId=" + id + "";
        else
            strWhere += "BsId=" + id + " and winCent>0";

        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Bspay> listBspay = new BCW.BLL.Game.Bspay().GetBspays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBspay.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Game.Bspay n in listBspay)
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

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                if (n.WinCent > 0)
                {
                    builder.Append("下注" + n.PayCent + "" + bzText + "，结果");
                    if (n.BetType == 1)
                        builder.Append("(庄小/闲小)");
                    else
                        builder.Append("(庄大/闲大)");

                    builder.Append("赢" + n.WinCent + "" + bzText + "[" + DT.FormatDate(n.AddTime, 0) + "]");
                    
                }
                else
                {
                    builder.Append("下注" + n.PayCent + "" + bzText + "，结果");
                    if (n.BetType == 1)
                        builder.Append("(庄大/闲小)");
                    else
                        builder.Append("(庄小/闲大)");

                    builder.Append("输" + n.WinCent + "" + bzText + "[" + DT.FormatDate(n.AddTime, 0) + "]");
                }
                builder.Append("IP:" + n.UsIP + "|UA:" + n.UsUA + "");

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
        if (pageIndex == 1)
        {
            long zWin = new BCW.BLL.Game.Bspay().GetWinCent("BsId=" + id + " and WinCent < 0");
            long zLost = new BCW.BLL.Game.Bspay().GetWinCent("BsId=" + id + " and WinCent > 0");
            long Win = (-zWin) - zLost;

            //庄胜与闲胜赔率
            string xmlPath = "/Controls/bigsmall.xml";
            double ZTar = Convert.ToDouble(ub.GetSub("BsZTar", xmlPath));
            double XTar = Convert.ToDouble(ub.GetSub("BsXTar", xmlPath));
            //今天赢利
            long WinCent = new BCW.BLL.Game.Bspay().GetWinCent("BsId=" + id + " and WinCent > 0");
            long WinCent2 = new BCW.BLL.Game.Bspay().GetWinCent("BsId=" + id + " and WinCent < 0");
            long TWin = Convert.ToInt64(WinCent * XTar * 0.01);
            long TWin2 = Convert.ToInt64(-WinCent2 * ZTar * 0.01);

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("本庄庄家赢利" + Win + "<br />");
            builder.Append("本站在此庄赢利" + (TWin + TWin2) + "");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bigsmall.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "大小庄ID错误"));
        Master.Title = "编辑大小庄";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑大小庄");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string strText = "庄名:/,最小下注:/,最大下注:/,用户ID:/,用户昵称:/,人气:/,创建时间:/,,,";
        string strName = "Title,SmallPay,BigPay,UsID,UsName,Click,AddTime,id,act,backurl";
        string strType = "text,num,num,num,text,num,date,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.SmallPay + "'" + model.BigPay + "'" + model.UsID + "'" + model.UsName + "'" + model.Click + "'" + model.AddTime + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,bigsmall.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bigsmall.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除记录</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,15}$", "庄名限1-15个字符");
        long SmallPay = Int64.Parse(Utils.GetRequest("SmallPay", "post", 4, @"^[1-9]\d*$", "最小下注填写错误"));
        long BigPay = Int64.Parse(Utils.GetRequest("BigPay", "post", 4, @"^[1-9]\d*$", "最大下注填写错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[\s\S]{1,10}$", "昵称限1-10个字符");
        int Click = int.Parse(Utils.GetRequest("Click", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        if (BigPay < SmallPay)
        {
            Utils.Error("最小下注不能大于最大下注", "");
        }
        if (!new BCW.BLL.Game.Bslist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的会员记录", "");
        }

        BCW.Model.Game.Bslist model = new BCW.Model.Game.Bslist();
        model.ID = id;
        model.Title = Title;
        model.SmallPay = SmallPay;
        model.BigPay = BigPay;
        model.UsID = UsID;
        model.UsName = UsName;
        model.Click = Click;
        model.AddTime = AddTime;
        new BCW.BLL.Game.Bslist().Update(model);
        Utils.Success("编辑大小庄", "编辑大小庄成功..", Utils.getUrl("bigsmall.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除大小庄";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗.删除同时将会删除大小庄的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Bslist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Bslist().Delete(id);
            new BCW.BLL.Game.Bspay().Delete("BsId=" + id + "");
            Utils.Success("删除大小庄", "删除成功..", Utils.getPage("bigsmall.aspx"), "1");
        }
    }

    private void ClearPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "清空下注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空下注记录吗，清空下注记录将清空排行榜与赢利分析");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?info=ok&amp;act=clear") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Bspay");
            Utils.Success("清空下注", "清空下注记录成功..", Utils.getUrl("bigsmall.aspx"), "1");
        }
    }

    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置大小庄游戏吗，重置后将删除所有个人庄和下注记录");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Bslist");
            new BCW.Data.SqlUp().ClearTable("tb_Bspay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("bigsmall.aspx"), "1");
        }
    }

    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        //庄胜与闲胜赔率
        string xmlPath = "/Controls/bigsmall.xml";
        double ZTar = Convert.ToDouble(ub.GetSub("BsZTar", xmlPath));
        double XTar = Convert.ToDouble(ub.GetSub("BsXTar", xmlPath));
        //今天赢利
        long TodayWinCent = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "  and WinCent > 0 and BzType=0");
        long TodayWinCent2 = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "  and WinCent < 0 and BzType=0");
        long TodayWin = Convert.ToInt64(TodayWinCent * XTar * 0.01);
        long TodayWin2 = Convert.ToInt64(-TodayWinCent2 * ZTar * 0.01);
        //昨天赢利
        long YesWinCent = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + "  and WinCent > 0 and BzType=0");
        long YesWinCent2 = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + "  and WinCent < 0 and BzType=0");
        long YesWin = Convert.ToInt64(YesWinCent * XTar * 0.01);
        long YesWin2 = Convert.ToInt64(-YesWinCent2 * ZTar * 0.01);
        //本月赢利
        long MonthWinCent = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + "  and WinCent > 0 and BzType=0");
        long MonthWinCent2 = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + "  and WinCent < 0 and BzType=0");
        long MonthWin = Convert.ToInt64(MonthWinCent * XTar * 0.01);
        long MonthWin2 = Convert.ToInt64(-MonthWinCent2 * ZTar * 0.01);

        //上月赢利
        long Month2WinCent = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + "  and WinCent > 0 and BzType=0");
        long Month2WinCent2 = new BCW.BLL.Game.Bspay().GetWinCent("Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + "  and WinCent < 0 and BzType=0");
        long Month2Win = Convert.ToInt64(Month2WinCent * XTar * 0.01);
        long Month2Win2 = Convert.ToInt64(-Month2WinCent2 * ZTar * 0.01);

        long WinCent = new BCW.BLL.Game.Bspay().GetWinCent("WinCent > 0 and BzType=0");
        long WinCent2 = new BCW.BLL.Game.Bspay().GetWinCent("WinCent < 0 and BzType=0");
        long Win = Convert.ToInt64(WinCent * XTar * 0.01);
        long Win2 = Convert.ToInt64(-WinCent2 * ZTar * 0.01);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利:" + (TodayWin + TodayWin2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利:" + (YesWin + YesWin2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月赢利:" + (MonthWin + MonthWin2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利:" + (Month2Win + Month2Win2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("赢利总计:" + (Win + Win2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bigsmall.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private bool Isbz()
    {
        return true;

        //if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        //    return true;
        //else
        //    return false;
    }
}