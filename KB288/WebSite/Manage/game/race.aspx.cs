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

public partial class Manage_game_race : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "over":
                OverPage();
                break;
            case "pass":
                PassPage();
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
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "竞拍管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "3"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("竞拍");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 3)
            builder.Append("待确认|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?ptype=3") + "\">待确认</a>|");

        if (ptype == 0)
            builder.Append("未审核|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?ptype=0") + "\">未审核</a>|");

        if (ptype == 1)
            builder.Append("进行中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?ptype=1") + "\">进行中</a>|");

        if (ptype == 2)
            builder.Append("已结束");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?ptype=2") + "\">已结束</a>");

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
        if (ptype == 1)
            strWhere = "paytype=1 and totime>'" + DateTime.Now + "'";
        else if (ptype == 3)
            strWhere = "paytype=1 and totime<'" + DateTime.Now + "'";
        else
            strWhere = "paytype=" + ptype + "";

        // 开始读取列表
        IList<BCW.Model.Game.Race> listRace = new BCW.BLL.Game.Race().GetRaces(pageIndex, pageSize, strWhere, out recordCount);
        if (listRace.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Race n in listRace)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("race.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("/bbs/game/race.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.title);
                if (ptype == 0)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("race.aspx?act=pass&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核</a>]");
                }
                else if (ptype == 1 || ptype == 3)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("race.aspx?act=over&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">确定结果</a>]");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../xml/raceset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "竞拍ID错误"));
        Master.Title = "编辑竞拍";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑竞拍");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Race model = new BCW.BLL.Game.Race().GetRace(id);
        if (model == null)
        {
            Utils.Error("不存在的竞拍记录", "");
        }
        string strText = "竞拍标题:/,竞拍描述:/,物品截图(可空):/,保密内容(管理员和竞拍得主可见):/,用户ID:/,用户昵称:/,币种类型:/,起拍价:/,截止时间:/,发布时间:/,最高价:/,最高价用户ID:/,最高价用户昵称:/,竞拍状态:/,,,";
        string strName = "Title,Content,FileUrl,PContent,UserID,UserName,Types,Price,Totime,Writetime,topPrice,WinID,WinName,State,id,act,backurl";
        string strType = "text,textarea,text,text,num,text,select,num,date,date,num,num,text,select,hidden,hidden,hidden";
        string strValu = "" + model.title + "'" + model.content + "'" + model.fileurl + "'" + model.pcontent + "'" + model.userid + "'" + model.username + "'"+model.Types+"'" + model.price + "'" + DT.FormatDate(model.totime,0) + "'" + DT.FormatDate(model.writetime,0) + "'" + model.topPrice + "'" + model.winID + "'" + model.winName + "'" + model.paytype + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,true,false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",false,false,false,false,false,true,0|未审核|1|进行中|2|已结束,false,false,false";
        string strIdea = "/";
        string strOthe = "编辑竞拍|reset,race.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("race.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此竞拍</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "竞拍ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "标题限1-20字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,1000}$", "物品描述限1-1000字");
        string FileUrl = Utils.GetRequest("FileUrl", "post", 3, @"^[\s\S]{1,100}$", "截图地址长度限100字符，可留空");
        string PContent = Utils.GetRequest("PContent", "post", 2, @"^[\s\S]{1,300}$", "保密内容限1-300字");
        int UserID = int.Parse(Utils.GetRequest("UserID", "post", 2, @"^[0-9]\d*$", "用户ID填写错误"));
        string UserName = Utils.GetRequest("UserName", "post", 3, @"^[\s\S]{1,10}$", "昵称填写错误");
        long Price = Int64.Parse(Utils.GetRequest("Price", "post", 4, @"^[0-9]\d*$", "起拍价填写错误"));
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-1]$", "币种类型错误"));
        DateTime Totime = Utils.ParseTime(Utils.GetRequest("Totime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime Writetime = Utils.ParseTime(Utils.GetRequest("Writetime", "post", 2, DT.RegexTime, "发布时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        long topPrice = Int64.Parse(Utils.GetRequest("topPrice", "post", 4, @"^[0-9]\d*$", "最高价填写错误"));
        int WinID = int.Parse(Utils.GetRequest("WinID", "post", 2, @"^[0-9]\d*$", "最高价用户ID填写错误"));
        string WinName = Utils.GetRequest("WinName", "post", 3, @"^[\s\S]{1,10}$", "最高价昵称填写错误");
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-2]$", "竞拍状态选择错误"));

        if (!new BCW.BLL.Game.Race().Exists(id))
        {
            Utils.Error("不存在的竞拍记录", "");
        }

        BCW.Model.Game.Race model = new BCW.Model.Game.Race();
        model.ID = id;
        model.Types = Types;
        model.title = Title;
        model.content = Content;
        model.fileurl = FileUrl;
        model.pcontent = PContent;
        model.userid = UserID;
        model.username = UserName;
        model.price = Price;
        model.totime = Totime;
        model.writetime = Writetime;
        model.topPrice = topPrice;
        model.winID = WinID;
        model.winName = WinName;
        model.paytype = State;
        new BCW.BLL.Game.Race().Update(model);
        Utils.Success("编辑竞拍", "编辑竞拍成功..", Utils.getUrl("race.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void PassPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "竞拍ID错误"));
        if (!new BCW.BLL.Game.Race().Exists(id, -1, 0))
        {
            Utils.Error("不存在的竞拍记录或已经审核", "");
        }
        new BCW.BLL.Game.Race().Updatepaytype(id, 1);
        Utils.Success("审核竞拍", "审核竞拍成功..", Utils.getPage("race.aspx"), "1");
    }

    private void OverPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Race model = new BCW.BLL.Game.Race().GetRace(id);
        if (model == null)
        {
            Utils.Error("不存在的竞拍记录", "");
        }
        string bzText = string.Empty;
        if (model.Types == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        if (info == "")
        {
            Master.Title = "确认竞拍结果";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确认结果后，最高价数量的" + bzText + "将会转入发布竞拍者的帐上并内线");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "最高价:/,最高价用户ID:/,最高价用户昵称:/,,,,";
            string strName = "topPrice,WinID,WinName,id,act,info,backurl";
            string strType = "num,num,text,hidden,hidden,hidden,hidden";
            string strValu = "" + model.topPrice + "'" + model.winID + "'" + model.winName + "'" + id + "'over'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认结果并结束竞拍,race.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("race.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Race().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            long topPrice = Int64.Parse(Utils.GetRequest("topPrice", "post", 4, @"^[0-9]\d*$", "最高价错误"));
            int WinID = int.Parse(Utils.GetRequest("WinID", "post", 2, @"^[0-9]\d*$", "最高价用户ID错误"));
            string WinName = Utils.GetRequest("WinName", "post", 2, @"^[\s\S]{1,10}$", "最高价用户昵称错误");
            if (!new BCW.BLL.User().Exists(WinID))
            {
                Utils.Error("不存在的会员", "");
            }
            new BCW.BLL.Game.Race().UpdatetopPrice(id, topPrice, WinID, WinName, 2);
            //转入币
            //手续费
            long Tax = Convert.ToInt64(Utils.ParseInt(ub.GetSub("RaceTax", "/Controls/race.xml")) * 0.01 * topPrice);
            long payCent = Convert.ToInt64(topPrice - Tax);
            if (model.Types == 0)
                new BCW.BLL.User().UpdateiGold(model.userid, model.username, payCent, "竞拍物品获得");
            else
                new BCW.BLL.User().UpdateiMoney(model.userid, model.username, payCent, "竞拍物品获得");

            //发内线给发布竞拍者
            new BCW.BLL.Guest().Add(model.userid, model.username, "[URL=/bbs/uinfo.aspx?uid=" + WinID + "]" + WinName + "[/URL]以" + topPrice + "" + bzText + "成功竞拍您的[URL=/bbs/game/race.aspx?act=view&amp;id=" + id + "]" + model.title + "[/URL]，竞拍" + topPrice + "" + bzText + "(不含手续费)已转入您的帐上");
            Utils.Success("确认结束竞拍", "确认结束竞拍成功，系统已发送内线至发布竞拍者..", Utils.getPage("race.aspx"), "1");
        }
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除竞拍";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此竞拍.删除同时将会删除此竞拍的出价记录.为了避免争议，强烈建议在竞拍完成后再删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Race().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Race().Delete(id);
            //new BCW.BLL.Game.Freesell().DeleteStr("cclID=" + id + "");//级联删除已代替
            Utils.Success("删除竞拍", "删除竞拍成功..", Utils.getPage("race.aspx"), "1");
        }
    }
}
