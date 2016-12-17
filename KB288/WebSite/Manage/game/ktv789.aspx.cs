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

public partial class Manage_game_ktv789 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
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
            case "clear":
                ClearPage();
                break;
            case "top":
                TopPage();
                break;
            case "speak":
                SpeakPage();
                break;
            case "clearspeak":
                ClearSpeakPage();
                break;
            case "msg":
                MsgPage();
                break;
            case "clearmsg":
                ClearMsgPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    /// <summary>
    /// 游戏首页
    /// </summary>
    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
        Master.Title = "Ktv789管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;Ktv789");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 1)
            builder.Append("初级对决|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=1") + "\">初级</a>|");

        if (ptype == 2)
            builder.Append("中级对决|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=2") + "\">中级</a>|");

        if (ptype == 3)
            builder.Append("终级对决");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=3") + "\">终级</a>");


        builder.Append("<br /><a href=\"" + Utils.getUrl("ktv789.aspx?act=top") + "\">排行榜单</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=speak") + "\">闲聊</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=msg") + "\">动作</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=" + ptype + "";

        // 开始读取列表
        IList<BCW.Model.Game.ktv789> listktv789 = new BCW.BLL.Game.ktv789().Getktv789s(pageIndex, pageSize, strWhere, out recordCount);
        if (listktv789.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.ktv789 n in listktv789)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("ktv789.aspx?act=edit&amp;id={0}") + "\">[管理]&gt;</a>", n.ID);
                builder.AppendFormat("{0}", n.StName);
                builder.Append("<br />");
                builder.AppendFormat("<a href=\"" + Utils.getUrl("ktv789.aspx?act=speak&amp;id={0}") + "\">闲聊</a>&gt;", n.ID);
                builder.AppendFormat("<a href=\"" + Utils.getUrl("ktv789.aspx?act=msg&amp;id={0}") + "\">动作记录</a>", n.ID);
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
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=add") + "\">添加房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/ktv789set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("789赌神|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=top&amp;ptype=1") + "\">789赌神</a>|");

        if (ptype == 2)
            builder.Append("789狂人");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=top&amp;ptype=2") + "\">789狂人</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=1";
        if (ptype == 1)
            strOrder = "(WinGold+PutGold) Desc";
        else
            strOrder = "PutNum Desc";

        // 开始读取列表
        IList<BCW.Model.Toplist> listToplist = new BCW.BLL.Toplist().GetToplists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listToplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Toplist n in listToplist)
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
                string sText = string.Empty;
                if (ptype == 1)
                    sText = "净赢" + (n.WinGold - n.PutGold) + "" + ub.Get("SiteBz") + "";
                else
                    sText = "共出手" + n.PutNum + "次";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}") + "\">{1}</a>{2}", n.UsId, n.UsName, sText);
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
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=clear") + "\">清空排行榜</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    /// <summary>
    /// 闲聊记录
    /// </summary>
    private void SpeakPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string StName = string.Empty;
        if (id != 0)
        {
            if (!new BCW.BLL.Game.ktv789().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            StName = new BCW.BLL.Game.ktv789().GetStName(id);
        }

        Master.Title = "" + StName + "闲聊记录";
        builder.Append(Out.Tab("<div class=\"title\">" + StName + "闲聊记录</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id", "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id != 0)
            strWhere = "Types=2 and NodeId=" + id + "";
        else
            strWhere = "Types=2";

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

                builder.AppendFormat("{0}{1}({2}前)", "系统:", n.Notes, DT.DateDiff(DateTime.Now, n.AddTime));
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
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=clearspeak&amp;id=" + id + "") + "\">清空闲聊</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 系统动作记录
    /// </summary>
    private void MsgPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string StName = string.Empty;
        if (id != 0)
        {
            if (!new BCW.BLL.Game.ktv789().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            StName = new BCW.BLL.Game.ktv789().GetStName(id);
        }

        Master.Title = "" + StName + "系统动作记录";
        builder.Append(Out.Tab("<div class=\"title\">" + StName + "系统动作记录</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id", "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id != 0)
            strWhere = "Types=2 and NodeId=" + id + "";
        else
            strWhere = "Types=2";

        // 开始读取列表
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(pageIndex, pageSize, strWhere, out recordCount);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
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

                builder.AppendFormat("{0}{1}({2}前)", "系统:", n.Notes, DT.DateDiff(DateTime.Now, n.AddTime));
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
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=clearmsg&amp;id=" + id + "") + "\">清空动作</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 修改房间
    /// </summary>
    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Game.ktv789().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.ktv789 model = new BCW.BLL.Game.ktv789().Getktv789(id);
        Master.Title = "修改" + model.StName + "房间";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改" + model.StName + "房间");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "房间名称:/,房间类型:/,游戏性质:/,每场下注额:/,超时秒数:/,,";
        string strName = "StName,Types,GoldType,PayCent,Expir,id,act";
        string strType = "text,select,select,num,num,hidden,hidden";
        string strValu = "" + model.StName + "'" + model.Types + "'" + model.GoldType + "'" + model.PayCent + "'" + model.Expir + "'" + id + "'editsave";
        string strEmpt = "false,1|初级对决|2|中级对决|3|终级对决,0|正常游戏|1|奖币游戏,false,false,false";
        string strIdea = "/";
        string strOthe = "修改|reset,ktv789.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=del&amp;id=" + id + "") + "\">删除房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string StName = Utils.GetRequest("StName", "post", 2, @"^[\s\S]{1,20}$", "名称限1-20字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[1-3]\d*$", "选择房间类型错误"));
        int GoldType = int.Parse(Utils.GetRequest("GoldType", "post", 2, @"^[0-1]\d*$", "出手方式选择错误"));
        int PayCent = int.Parse(Utils.GetRequest("PayCent", "post", 2, @"^[0-9]\d*$", "每场下注额填写错误"));
        int Expir = int.Parse(Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "超时时间填写错误"));

        if (!new BCW.BLL.Game.ktv789().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.ktv789 model = new BCW.Model.Game.ktv789();
        model.ID = id;
        model.StName = StName;
        model.Types = Types;
        model.GoldType = GoldType;
        model.PayCent = PayCent;
        model.Expir = Expir;
        new BCW.BLL.Game.ktv789().Update(model);

        Utils.Success("修改成功", "修改房间成功..", Utils.getUrl("ktv789.aspx"), "1");
    }

    private void AddPage()
    {
        Master.Title = "添加房间";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加房间");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "房间名称:/,房间类型:/,游戏性质:/,每场下注额:/,超时秒数:/,,";
        string strName = "StName,Types,GoldType,PayCent,Expir,act";
        string strType = "text,select,select,num,num,hidden";
        string strValu = "'1'0'''addsave";
        string strEmpt = "false,1|初级对决|2|中级对决|3|终级对决,0|正常游戏|1|奖币游戏,false,false,false";
        string strIdea = "/";
        string strOthe = "添加|reset,ktv789.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />游戏性质为奖币游戏时,玩游戏将不会扣取会员币，只奖不扣");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">返回上一级</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string StName = Utils.GetRequest("StName", "post", 2, @"^[\s\S]{1,20}$", "名称限1-20字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[1-3]\d*$", "选择房间类型错误"));
        int GoldType = int.Parse(Utils.GetRequest("GoldType", "post", 2, @"^[0-1]\d*$", "出手方式选择错误"));
        int PayCent = int.Parse(Utils.GetRequest("PayCent", "post", 2, @"^[0-9]\d*$", "每场下注额填写错误"));
        int Expir = int.Parse(Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "超时时间填写错误"));

        BCW.Model.Game.ktv789 model = new BCW.Model.Game.ktv789();
        model.StName = StName;
        model.Types = Types;
        model.GoldType = GoldType;
        model.PayCent = PayCent;
        model.Expir = Expir;
        model.OneUsId = 0;
        model.TwoUsId = 0;
        model.ThrUsId = 0;
        model.Online = 0;
        model.NextShot = 1;
        model.PkCount = 1;

        new BCW.BLL.Game.ktv789().Add(model);

        Utils.Success("添加成功", "添加房间成功..", Utils.getUrl("ktv789.aspx"), "1");
    }

    private void DelPage()
    {
        Master.Title = "删除房间";


        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此房间吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("ktv789.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.ktv789().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            //删除
            new BCW.BLL.Game.ktv789().Delete(id);
            Utils.Success("删除房间", "删除房间成功..", Utils.getPage("ktv789.aspx"), "1");
        }
    }

    private void ClearPage()
    {
        Master.Title = "清空排行榜";

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空排行榜吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?info=ok&amp;act=clear") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("ktv789.aspx?act=top") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            new BCW.BLL.Toplist().Clear(2);
            Utils.Success("清空排行榜", "清空排行榜成功..", Utils.getPage("ktv789.aspx?act=top"), "1");
        }
    }

    private void ClearSpeakPage()
    {
        Master.Title = "清空闲聊";
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定闲聊记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?info=ok&amp;act=clearspeak&amp;id=" + id + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("ktv789.aspx?act=speak&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            if (id != 0)
                new BCW.BLL.Speak().Clear(2, id);
            else
                new BCW.BLL.Speak().Clear(2);

            Utils.Success("清空闲聊记录", "清空闲聊记录成功..", Utils.getPage("ktv789.aspx?act=speak&amp;id=" + id + ""), "1");
        }
    }

    private void ClearMsgPage()
    {
        Master.Title = "清空系统动作";
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定系统动作记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?info=ok&amp;act=clearmsg&amp;id=" + id + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("ktv789.aspx?act=msg&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            if (id != 0)
                new BCW.BLL.Action().Clear(2, id);
            else
                new BCW.BLL.Action().Clear(2);

            Utils.Success("清空系统动作记录", "清空系统动作记录成功..", Utils.getPage("ktv789.aspx?act=msg&amp;id=" + id + ""), "1");
        }
    }
}
