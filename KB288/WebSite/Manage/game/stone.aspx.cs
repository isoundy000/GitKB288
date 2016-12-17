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

public partial class Manage_game_stone : System.Web.UI.Page
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
        Master.Title = "剪刀管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;石头剪刀布");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 1)
            builder.Append("二人对决|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=1") + "\">二人</a>|");

        if (ptype == 2)
            builder.Append("三人对决|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=2") + "\">三人</a>|");

        if (ptype == 3)
            builder.Append("生死战|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=3") + "\">生死</a>|");

        if (ptype == 4)
            builder.Append("混战区");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=4") + "\">混战</a>");

        builder.Append("<br /><a href=\"" + Utils.getUrl("stone.aspx?act=top") + "\">排行榜单</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=speak") + "\">闲聊</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=msg") + "\">动作</a>");

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
        IList<BCW.Model.Game.Stone> listStone = new BCW.BLL.Game.Stone().GetStones(pageIndex, pageSize, strWhere, out recordCount);
        if (listStone.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stone n in listStone)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("stone.aspx?act=edit&amp;id={0}") + "\">[管理]&gt;</a>", n.ID);
                builder.AppendFormat("{0}", n.StName);
                builder.Append("<br />");
                builder.AppendFormat("<a href=\"" + Utils.getUrl("stone.aspx?act=speak&amp;id={0}") + "\">闲聊</a>&gt;", n.ID);
                builder.AppendFormat("<a href=\"" + Utils.getUrl("stone.aspx?act=msg&amp;id={0}") + "\">动作记录</a>", n.ID);
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
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=add") + "\">添加房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/stoneset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
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
            builder.Append("剪刀赌神|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=top&amp;ptype=1") + "\">剪刀赌神</a>|");

        if (ptype == 2)
            builder.Append("剪刀狂人");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=top&amp;ptype=2") + "\">剪刀狂人</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=clear") + "\">清空排行榜</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">返回上一级</a><br />");
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
            if (!new BCW.BLL.Game.Stone().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            StName = new BCW.BLL.Game.Stone().GetStName(id);
        }

        Master.Title = "" + StName + "闲聊记录";
        builder.Append(Out.Tab("<div class=\"title\">" + StName + "闲聊记录</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id","act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id != 0)
            strWhere = "Types=1 and NodeId=" + id + "";
        else
            strWhere = "Types=1";

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
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=clearspeak&amp;id=" + id + "") + "\">清空闲聊</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">返回上一级</a><br />");
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
            if (!new BCW.BLL.Game.Stone().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            StName = new BCW.BLL.Game.Stone().GetStName(id);
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
            strWhere = "Types=1 and NodeId=" + id + "";
        else
            strWhere = "Types=1";

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
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=clearmsg&amp;id=" + id + "") + "\">清空动作</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 修改房间
    /// </summary>
    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.Stone model = new BCW.BLL.Game.Stone().GetStone(id);
        Master.Title = "修改" + model.StName + "房间";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改" + model.StName + "房间");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "房间名称:/,房间类型:/,出手方式:/,最小下注额:/,最大下注额:/,超时秒数:/,,";
        string strName = "StName,Types,ShotTypes,SmallPay,BigPay,Expir,id,act";
        string strType = "text,select,select,num,num,num,hidden,hidden";
        string strValu = "" + model.StName + "'" + model.Types + "'" + model.ShotTypes + "'" + model.SmallPay + "'" + model.BigPay + "'" + model.Expir + "'" + id + "'editsave";
        string strEmpt = "false,1|二人对决|2|三人对决|3|生死战|4|混战区,0|自由出手|1|轮流出手,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改|reset,stone.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=del&amp;id=" + id + "") + "\">删除房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string StName = Utils.GetRequest("StName", "post", 2, @"^[\s\S]{1,20}$", "名称限1-20字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-9]\d*$", "ID错误"));
        int ShotTypes = int.Parse(Utils.GetRequest("ShotTypes", "post", 2, @"^[0-1]\d*$", "出手方式选择错误"));
        int SmallPay = int.Parse(Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误"));
        int BigPay = int.Parse(Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误"));
        int Expir = int.Parse(Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "超时时间填写错误"));

        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.Stone model = new BCW.Model.Game.Stone();
        model.ID = id;
        model.StName = StName;
        model.Types = Types;
        model.ShotTypes = ShotTypes;
        model.SmallPay = SmallPay;
        model.BigPay = BigPay;
        model.Expir = Expir;
        new BCW.BLL.Game.Stone().Update(model);

        Utils.Success("修改成功", "修改房间成功..", Utils.getUrl("stone.aspx"), "1");
    }

    private void AddPage()
    {
        Master.Title = "添加房间";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加房间");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "房间名称:/,房间类型:/,出手方式:/,最小下注额:/,最大下注额:/,超时秒数:/,,";
        string strName = "StName,Types,ShotTypes,SmallPay,BigPay,Expir,act";
        string strType = "text,select,select,num,num,num,hidden";
        string strValu = "'1'0''''addsave";
        string strEmpt = "false,1|二人对决|2|三人对决|3|生死战|4|混战区,0|自由出手|1|轮流出手,false,false,false";
        string strIdea = "/";
        string strOthe = "添加|reset,stone.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">返回上一级</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string StName = Utils.GetRequest("StName", "post", 2, @"^[\s\S]{1,20}$", "名称限1-20字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-9]\d*$", "ID错误"));
        int ShotTypes = int.Parse(Utils.GetRequest("ShotTypes", "post", 2, @"^[0-1]\d*$", "出手方式选择错误"));
        int SmallPay = int.Parse(Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误"));
        int BigPay = int.Parse(Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误"));
        int Expir = int.Parse(Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "超时时间填写错误"));

        BCW.Model.Game.Stone model = new BCW.Model.Game.Stone();
        model.StName = StName;
        model.Types = Types;
        model.ShotTypes = ShotTypes;
        model.SmallPay = SmallPay;
        model.BigPay = BigPay;
        model.Expir = Expir;
        model.PayCent = 0;
        model.OneUsId = 0;
        model.TwoUsId = 0;
        model.ThrUsId = 0;
        model.OneShot = 0;
        model.TwoShot = 0;
        model.ThrShot = 0;
        model.Online = 0;
        model.OneStat = 0;
        model.TwoStat = 0;
        model.ThrStat = 0;
        model.IsStatus = 0;
        model.NextShot = 1;
        model.PkCount = 1;

        new BCW.BLL.Game.Stone().Add(model);

        Utils.Success("添加成功", "添加房间成功..", Utils.getUrl("stone.aspx"), "1");
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
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("stone.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Stone().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            //删除
            new BCW.BLL.Game.Stone().Delete(id);
            Utils.Success("删除房间", "删除房间成功..", Utils.getPage("stone.aspx"), "1");
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
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?info=ok&amp;act=clear") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("stone.aspx?act=top") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            new BCW.BLL.Toplist().Clear(1);
            Utils.Success("清空排行榜", "清空排行榜成功..", Utils.getPage("stone.aspx?act=top"), "1");
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
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?info=ok&amp;act=clearspeak&amp;id=" + id + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("stone.aspx?act=speak&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            if (id != 0)
                new BCW.BLL.Speak().Clear(1, id);
            else
                new BCW.BLL.Speak().Clear(1);

            Utils.Success("清空闲聊记录", "清空闲聊记录成功..", Utils.getPage("stone.aspx?act=speak&amp;id=" + id + ""), "1");
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
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?info=ok&amp;act=clearmsg&amp;id=" + id + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("stone.aspx?act=msg&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //清空
            if (id != 0)
                new BCW.BLL.Action().Clear(1, id);
            else
                new BCW.BLL.Action().Clear(1);

            Utils.Success("清空系统动作记录", "清空系统动作记录成功..", Utils.getPage("stone.aspx?act=msg&amp;id=" + id + ""), "1");
        }
    }
}