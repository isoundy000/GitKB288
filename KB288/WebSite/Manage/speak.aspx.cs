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

public partial class Manage_speak : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string[] sName = { };
    protected string[] sUrl = { };
    protected void Page_Load(object sender, EventArgs e)
    {
        string endName = string.Empty;
        string endUrl = string.Empty;

        string GameName = new BCW.User.Game.GameRole().GameName();
        string[] gTemp = GameName.Split('#');

        endName = gTemp[0];
        endUrl = gTemp[2];
        sName = endName.Split(",".ToCharArray());
        sUrl = endUrl.Split(",".ToCharArray());

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "list":
                ListPage();
                break;
            case "clear":
                ClearPage();
                break;
            case "clearok":
                ClearOkPage();
                break;
            case "del":
                DelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "闲聊管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("闲聊管理");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (pageIndex == 1)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("0.<a href=\"" + Utils.getUrl("speak.aspx?act=list&amp;ptype=0&amp;id=0") + "\">单独闲聊</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        //总记录数
        recordCount = sName.Length;

        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        for (int i = 0; i < sName.Length; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                if ((k + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("speak.aspx?act=list&amp;ptype=" + (i + 1) + "&amp;id=" + (Convert.ToInt32(sUrl[i])) + "") + "\">" + sName[i].ToString() + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=clear&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ListPage()
    {
        Master.Title = "闲聊管理";
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (sName.Length < ptype)
        {
            Utils.Error("不存在的类型", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype > 0)
            builder.Append("" + sName[ptype - 1] + "闲聊");
        else
            builder.Append("单独闲聊");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "id", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere += "usid=" + uid + "";

        if (uid > 0 && id > 0)
            strWhere += " and ";

        //if (ptype > 0)
            strWhere += "Types=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Speak> listSpeak = new BCW.BLL.Speak().GetSpeaks(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpeak.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Speak n in listSpeak)
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

                builder.AppendFormat("[{0}]<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>说:{3}", BCW.User.AppCase.CaseAction(n.Types), n.UsId, n.UsName, Out.UBB(n.Notes));

                if (n.ToId > 0)
                    builder.Append("→<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ToId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a>");
                
                builder.Append("(" + DT.FormatDate(n.AddTime, 5) + ")");

                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=del&amp;ptype=" + n.Types + "&amp;uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");

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
        string strText = "输入用户ID:/,,,";
        string strName = "uid,ptype,id,act";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "'" + ptype + "'" + id + "'list";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "搜闲聊,speak.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=clear&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    /// <summary>
    /// 清空闲聊记录
    /// </summary>
    private void ClearPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int otype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
        if (id > 0)
            Master.Title = "清空" + sName[otype - 1] + "闲聊";
        else
            Master.Title = "清空全部闲聊";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择清空选项");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=clearok&amp;&amp;otype=" + otype + "&amp;id=" + id + "&amp;ptype=1") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=clearok&amp;otype=" + otype + "&amp;id=" + id + "&amp;ptype=2") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=clearok&amp;otype=" + otype + "&amp;id=" + id + "&amp;ptype=3") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=clearok&amp;otype=" + otype + "&amp;id=" + id + "&amp;ptype=4") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx") + "\">闲聊管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 清空闲聊记录
    /// </summary>
    private void ClearOkPage()
    {
        Master.Title = "清空闲聊";
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int otype = int.Parse(Utils.GetRequest("otype", "get", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        string M_Str_mindate = string.Empty;
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (id > 0)
                builder.Append("确定要清空" + sName[otype - 1] + "闲聊吗");
            else
                builder.Append("确定要清空单独闲聊吗");

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?info=ok&amp;act=" + act + "&amp;otype=" + otype + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("speak.aspx?act=clear") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getPage("speak.aspx?info=acok&amp;act=" + act + "&amp;otype=" + otype + "&amp;id=" + id + "&amp;ptype=" + ptype + ""), "2");
            }
            else
            {
                string sWhe = string.Empty;
                string sWhe2 = string.Empty;
                //if (otype > 0)
                //{
                    sWhe = " and Types=" + id + "";
                    sWhe2 = " Types=" + id + "";
                //}
   
                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    new BCW.BLL.Speak().Delete("AddTime<'" + M_Str_mindate + "' " + sWhe + "");
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
                    new BCW.BLL.Speak().Delete("AddTime<'" + M_Str_mindate + "' " + sWhe + "");
                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    new BCW.BLL.Speak().Delete("AddTime<'" + M_Str_mindate + "' " + sWhe + "");
                }
                else if (ptype == 4)
                {
                    new BCW.BLL.Speak().Delete("" + sWhe2 + "");
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("speak.aspx?act=clear"), "2");
            }
        }
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (info == "")
        {
            Master.Title = "删除闲聊";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除TA的闲聊记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?info=ok&amp;act=del&amp;ptype=" + ptype + "&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除(仅此游戏)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?info=ok2&amp;act=del&amp;ptype=" + ptype + "&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除(所有游戏)</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("speak.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.User().Exists(uid))
            {
                Utils.Error("不存在的会员", "");
            }
            //删除
            if (info == "ok2")
                new BCW.BLL.Speak().Delete("UsId=" + uid + "");
            else
                new BCW.BLL.Speak().Delete("Types=" + ptype + " and UsId=" + uid + "");

            Utils.Success("删除闲聊", "删除TA的闲聊成功..", Utils.getPage("speak.aspx"), "1");
        }
    }
}
