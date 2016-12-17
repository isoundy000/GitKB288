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

public partial class Manage_game_free : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "del":
                DelPage();
                break;
            case "delsell":
                DelsellPage();
                break;
            case "paylist":
                PaylistPage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "clear":
                ClearPage();
                break;
            case "clearok":
                ClearOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "猜猜乐管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("猜猜乐");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("正在进行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?ptype=0") + "\">正在进行</a>|");

        if (ptype == 1)
            builder.Append("已结束");
        else
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?ptype=1") + "\">已结束</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (ptype == 0)
            strWhere = "State<>2";
        else
            strWhere = "State=2";

        // 开始读取列表
        IList<BCW.Model.Game.Free> listFree = new BCW.BLL.Game.Free().GetFrees(pageIndex, pageSize, strWhere, out recordCount);
        if (listFree.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Free n in listFree)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("free.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("free.aspx?act=paylist&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.Title);
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
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=clear") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void PaylistPage()
    {
        Master.Title = "查看购买记录";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "猜猜ID错误"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看购买记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "cclID=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Game.Freesell> listFreesell = new BCW.BLL.Game.Freesell().GetFreesells(pageIndex, pageSize, strWhere, out recordCount);
        if (listFreesell.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Freesell n in listFreesell)
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
                builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=delsell&amp;id=" + id + "&amp;sellid=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>购买{3}份，每份{4}" + ub.Get("SiteBz") + "", (pageIndex - 1) * pageSize + k, n.UserID, n.UserName, n.Counts2, n.Price);
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
        builder.Append("<a href=\"" + Utils.getPage("free.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }


    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "聊天室ID错误"));
        Master.Title = "编辑猜猜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑猜猜");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Free model = new BCW.BLL.Game.Free().GetFree(id);
        if (model == null)
        {
            Utils.Error("不存在的猜猜记录", "");
        }
        string strText = "猜猜乐题目:/,猜猜乐描述:/,用户ID:/,用户昵称:/,猜猜单份价格:/,截止时间:/,承诺开奖时间:/,猜猜状态:/,猜猜类型:/,,,";
        string strName = "Title,Content,UserID,UserName,Price,CloseTime,OpenTime,State,CclType,id,act,backurl";
        string strType = "text,textarea,num,text,num,date,date,select,select,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Content + "'" + model.UserID + "'" + model.UserName + "'" + model.Price + "'" + model.CloseTime + "'" + model.OpenTime + "'" + model.State + "'" + model.CclType + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,0|正在进行|1|等待确认|2|已结束,1|足球|2|篮球|3|彩票|4|股票|6|体育综合|7|日常生活,false,false,false";
        string strIdea = "/";
        string strOthe = "编辑猜猜|reset,free.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("free.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此猜猜</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "猜猜ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "猜猜题目限20字内");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "猜猜描述限500字内");
        int UserID = int.Parse(Utils.GetRequest("UserID", "post", 2, @"^[1-9]\d*$", "用户ID填写错误"));
        string UserName = Utils.GetRequest("UserName", "post", 2, @"^[\s\S]{1,10}$", "昵称填写错误");
        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "猜猜单份价格填写错误"));
        DateTime CloseTime = Utils.ParseTime(Utils.GetRequest("CloseTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime OpenTime = Utils.ParseTime(Utils.GetRequest("OpenTime", "post", 2, DT.RegexTime, "承诺开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-2]$", "猜猜状态填写错误"));
        int CclType = int.Parse(Utils.GetRequest("CclType", "post", 2, @"^[1-7]$", "猜猜类型填写错误"));

        if (!new BCW.BLL.Game.Free().Exists(id))
        {
            Utils.Error("不存在的猜猜记录", "");
        }

        if (!new BCW.BLL.User().Exists(UserID))
        {
            Utils.Error("不存在的用户ID", "");
        }

        BCW.Model.Game.Free model = new BCW.Model.Game.Free();
        model.ID = id;
        model.Title = Title;
        model.Content = Content;
        model.UserID = UserID;
        model.UserName = UserName;
        model.Price = Price;
        model.CloseTime = CloseTime;
        model.OpenTime = OpenTime;
        model.State = State;
        model.CclType = CclType;
        new BCW.BLL.Game.Free().Update(model);
        Utils.Success("编辑猜猜乐", "编辑猜猜乐成功..", Utils.getUrl("free.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }


    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除猜猜";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此猜猜.删除同时将会删除此猜猜的购买记录.为了避免争议，强烈建议在猜猜完成后再删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Free().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Free().Delete(id);
            //new BCW.BLL.Game.Freesell().DeleteStr("cclID=" + id + "");//级联删除已代替
            Utils.Success("删除猜猜", "删除猜猜成功..", Utils.getPage("free.aspx"), "1");
        }
    }

    private void DelsellPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int sellid = int.Parse(Utils.GetRequest("sellid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除猜猜购买记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此购买记录吗，删除后无法恢复，如果猜猜未结束，建议删除后返还币给该会员");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?info=ok&amp;act=delsell&amp;sellid=" + sellid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=paylist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Freesell().Exists(sellid))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Freesell().Delete(sellid);
            Utils.Success("删除购买记录", "删除购买记录成功..", Utils.getUrl("free.aspx?act=paylist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    /// <summary>
    /// 清空猜猜记录
    /// </summary>
    private void ClearPage()
    {
        Master.Title = "清空已结束的猜猜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("清空已结束的猜猜");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=clearok&amp;ptype=1") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=clearok&amp;ptype=2") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=clearok&amp;ptype=3") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=clearok&amp;ptype=4") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 清空猜猜记录
    /// </summary>
    private void ClearOkPage()
    {
        Master.Title = "清空已结束的猜猜";
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("free.aspx?act=clear") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getUrl("free.aspx?info=acok&amp;act=" + act + "&amp;ptype=" + ptype + ""), "2");
            }
            else
            {
                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    new BCW.BLL.Game.Free().DeleteStr("AddTime<'" + M_Str_mindate + "' and State=2");
                }
                else if (ptype == 2)
                {
                    //保留本周计算
                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Tuesday:
                            M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Wednesday:
                            M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Thursday:
                            M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Friday:
                            M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Saturday:
                            M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Sunday:
                            M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + " 0:00:00";
                            break;
                    }
                    new BCW.BLL.Game.Free().DeleteStr("AddTime<'" + M_Str_mindate + "' and State=2");
                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    new BCW.BLL.Game.Free().DeleteStr("AddTime<'" + M_Str_mindate + "' and State=2");
                }
                else if (ptype == 4)
                {
                    new BCW.BLL.Game.Free().DeleteStr("State=2");
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("free.aspx?act=clear"), "2");
            }
        }
    }
}