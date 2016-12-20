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

public partial class Manage_game_hc1 : System.Web.UI.Page
{
    /// <summary>
    /// wdy 20160521 界面修改
    /// sgl 20160926 修改后台机器人界面显示
    /// </summary>
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/hc1.xml";
    ub xml = new ub();
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
            case "backsave1":
            case "backsave3":
                BackSave1Page(act);
                break;

            case "get":
                GetPage();
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "open":
                OpenPage();
                break;
            case "opensave":
                OpenSavePage();
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
            case "view":
                ViewPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "top":
                TopPage();
                break;
            case "rebot":
                ReBot();
                break;
            case "stat":
                StatPage();
                break;
            case "Top10":
                Top10Page();
                break;
            case "reward":
                RewardPage();
                break;
            case "reward1":
                Reward1Page();
                break;
            case "peilv":
                PeilvPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }


    private void GetPage()
    {
        Master.Title = "开奖参考";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("开奖参考");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        //wdy
        builder.Append("期号：" + new BCW.Service.GetHc1().GetStageS());
        builder.Append("开奖号码：" + new BCW.Service.GetHc1().Getnumber());
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ReloadPage()
    {
        Master.Title = "好彩一管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("好彩一");
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
        IList<BCW.Model.Game.HcList> listHcList = new BCW.BLL.Game.HcList().GetHcLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcList.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.HcList n in listHcList)
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
                builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=edit&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                    builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("hc1.aspx?act=view&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a> <a href=\"" + Utils.getUrl("hc1.aspx?act=open&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开奖</a>|<a href=\"" + Utils.getUrl("hc1.aspx?act=get") + "\">开奖参考</a>");
                else
                    builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("hc1.aspx?act=view&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Result + "," + OutSx(n.Result) + "," + OutSj(n.Result) + "," + OutFw(n.Result) + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=add") + "\">开通下期</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=Top10") + "\">排行奖励</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=stat") + "\">盈利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/hc1set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=top") + "\">游戏排行</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=rebot") + "\">机器人设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=peilv") + "\">大小单双赔率还原值设置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void PeilvPage()
    {
        Master.Title = "赔率自动还原设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("好彩一");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/hc1.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string odds5 = Utils.GetRequest("odds5", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "赔率填写错误");
            string odds6 = Utils.GetRequest("odds6", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            string odds7 = Utils.GetRequest("odds7", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            string odds8 = Utils.GetRequest("odds8", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            string odds10 = Utils.GetRequest("odds10", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            string odds11 = Utils.GetRequest("odds11", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            string odds12 = Utils.GetRequest("odds12", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            string odds13 = Utils.GetRequest("odds13", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
            xml.dss["Hc1odds5"] = odds5;
            xml.dss["Hc1odds6"] = odds6;
            xml.dss["Hc1odds7"] = odds7;
            xml.dss["Hc1odds8"] = odds8;
            xml.dss["Hc1odds10"] = odds10;
            xml.dss["Hc1odds11"] = odds11;
            xml.dss["Hc1odds12"] = odds12;
            xml.dss["Hc1odds13"] = odds13;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("好彩一设置", "设置成功，正在返回..", Utils.getUrl("hc1.aspx?act=peilv " + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            //{
            //    Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "hc1.aspx?ve=2a&amp;u=" + Utils.getstrU() + "", "1");
            //}
            string strText = "大数赔率还原值:/,小数赔率还原值:/,单数赔率还原值:/,双数赔率还原值:/,尾数大数赔率还原值:/,尾数小数赔率还原值:/,尾数单数赔率还原值:/,尾数双数赔率还原值:/,";
            string strName = "odds5,odds6,odds7,odds8,odds10,odds11,odds12,odds13,backurl";
            string strType = "text,text,text,text,text,text,text,text,hidden";
            string strValu = "" + xml.dss["Hc1odds5"] + "'" + xml.dss["Hc1odds6"] + "'" + xml.dss["Hc1odds7"] + "'" + xml.dss["Hc1odds8"] + "'" + xml.dss["Hc1odds10"] + "'" + xml.dss["Hc1odds11"] + "'" + xml.dss["Hc1odds12"] + "'" + xml.dss["Hc1odds13"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,hc1.aspx?act=peilv,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    private void BackPage()
    {
        Master.Title = "好彩一返赢返负";
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
        string strOthe = "马上返赢,hc1.aspx,post,1,red";

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
        strOthe = "马上返负,hc1.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a>");
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

            builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("返赢时间段：" + sTime + "到" + oTime + "<br />");
            builder.Append("返赢千分比：" + iTar + "<br />");
            builder.Append("至少赢：" + iPrice + "币返<br />");

            builder.Append(Out.Tab("</div>", ""));
            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime.AddDays(-10), 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave1";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "马上返赢,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));

        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("返负时间段：" + sTime + "到" + oTime + "<br />");
            builder.Append("返负千分比：" + iTar + "<br />");
            builder.Append("至少：" + iPrice + "币返<br />");

            builder.Append(Out.Tab("</div>", ""));
            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime.AddDays(-10), 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave3";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "马上返负,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BackSave1Page(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        if (act == "backsave1")
        {
            DataSet ds = new BCW.BLL.Game.HcPay().GetList("UsID,sum(WinCent-PayCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "好彩一返赢");
                    //发内线
                    string strLog = "根据你上期好彩一排行榜上的赢利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/hc1.aspx]进入好彩一[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("hc1.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.BLL.Game.HcPay().GetList("UsID,sum(WinCent-PayCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "好彩一返负");
                    //发内线
                    string strLog = "根据你上期好彩一排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/hc1.aspx]进入好彩一[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("hc1.aspx"), "1");
        }


    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.CID + "期好彩一";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "CID=" + model.CID + "";
        else
            strWhere += "CID=" + model.CID + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.HcPay> listHcPay = new BCW.BLL.Game.HcPay().GetHcPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHcPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "," + OutSx(model.Result) + "," + OutSj(model.Result) + "," + OutFw(model.Result) + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.HcPay n in listHcPay)
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
                if (n.State == 0)
                    builder.Append("<b>[" + ForType(n.Types) + "]</b>押" + n.Vote + "/每项下注" + n.PayCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 13) + "]" + " 标识ID" + n.id);
                else if (n.State == 1)
                {
                    builder.Append("<b>[" + ForType(n.Types) + "]</b>押" + n.Vote + "/每项下注" + n.PayCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]" + " 标识ID" + n.id);
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                    builder.Append("<b>[" + ForType(n.Types) + "]</b>押" + n.Vote + "/每项下注" + n.PayCent + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]" + " 标识ID" + n.id);


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出号码:<b>" + model.Result + "," + OutSx(model.Result) + "," + OutSj(model.Result) + "," + OutFw(model.Result) + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "编辑好彩一";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑第" + model.CID + "期好彩一");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "期数:/,截止时间:/,,,";
        string strName = "CID,EndTime,id,act,backurl";
        string strType = "num,date,hidden,hidden,hidden";
        string strValu = "" + model.CID + "'" + DT.FormatDate(model.EndTime, 0) + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,hc1.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int CID = int.Parse(Utils.GetRequest("CID", "post", 2, @"^[0-9]\d*$", "期数填写错误"));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));


        BCW.Model.Game.HcList model = new BCW.Model.Game.HcList();

        model.id = id;
        model.CID = CID;
        model.EndTime = EndTime;

        new BCW.BLL.Game.HcList().Update(model);
        Utils.Success("编辑第" + CID + "期", "编辑第" + CID + "期成功..", Utils.getUrl("hc1.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "删除第" + model.CID + "期";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定第" + model.CID + "期记录吗.删除同时将会删除该期的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            new BCW.BLL.Game.HcList().Delete(id);
            new BCW.BLL.Game.HcPay().Delete("CID=" + model.CID + "");
            Utils.Success("删除第" + model.CID + "期", "删除第" + model.CID + "期成功..", Utils.getPage("hc1.aspx"), "1");
        }
    }

    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置好彩一游戏吗，重置后，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.BLL.Game.HcList().ClearTable("tb_HcList");
            new BCW.BLL.Game.HcList().ClearTable("tb_HcPay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("hc1.aspx"), "1");
        }
    }

    private void AddPage()
    {
        BCW.Model.Game.HcList m = new BCW.BLL.Game.HcList().GetHcListNew(0);
        if (m != null)
        {
            Utils.Error("上一期未结束，还不能开通下期", "");
        }
        Master.Title = "开通投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("开通新一期投注");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "开通期数:,截止时间:,,";
        string strName = "CID,EndTime,act,backurl";
        string strType = "num,date,hidden,hidden";
        string strValu = "'" + DT.FormatDate(DateTime.Parse("19:00:00").AddDays(1), 0) + "'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开通|reset,hc1.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        BCW.Model.Game.HcList m = new BCW.BLL.Game.HcList().GetHcListNew(0);
        if (m != null)
        {
            Utils.Error("上一期未结束，还不能开通下期", "");
        }
        int CID = int.Parse(Utils.GetRequest("CID", "post", 2, @"^[1-9]\d*$", "期数填写错误"));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

        BCW.Model.Game.HcList model = new BCW.Model.Game.HcList();

        model.CID = CID;
        model.Result = "";
        model.Notes = "";
        model.payCent = 0;
        model.payCount = 0;
        model.EndTime = EndTime;
        model.State = 0;
        model.payCent2 = 0;
        model.payCount2 = 0;
        new BCW.BLL.Game.HcList().Add(model);
        Utils.Success("开通投注", "开通第" + CID + "期投注成功..", Utils.getUrl("hc1.aspx"), "1");
    }

    private void OpenPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "好彩一开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("第" + model.CID + "期好彩一开奖");
        builder.Append(Out.Tab("</div>", ""));

        if (model.State == 1)
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId != 1)
            {
                Utils.Error("本期已开奖", "");
            }
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("本期已开奖，要重新开奖吗");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "开出数字:/,,,";
        string strName = "Result,id,act,backurl";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "" + model.Result + "'" + id + "'opensave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开奖|addsave,hc1.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void OpenSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int Result = int.Parse(Utils.GetRequest("Result", "post", 2, @"^[1-9]\d*$", "开出数字限1-36"));
        if (Result < 1 || Result > 36)
        {
            Utils.Error("下注数字限1-36", "");
        }
        BCW.Model.Game.HcList model = new BCW.BLL.Game.HcList().GetHcList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //遍历当期所有投注
        DataSet ds = new BCW.BLL.Game.HcPay().GetList("id,Types,UsID,UsName,Vote,PayCent", "CID=" + model.CID + " and State=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int pid = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                string Vote = ds.Tables[0].Rows[i]["Vote"].ToString();
                long PayCent = Int64.Parse(ds.Tables[0].Rows[i]["PayCent"].ToString());

                double Odds = 0;//赔率
                bool IsWin = false;

                if (Types == 1)//选号开奖
                {
                    if (("," + Vote + ",").Contains("," + Result + ","))
                    {
                        IsWin = true;
                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds1", xmlPath));
                    }
                }
                else if (Types == 2)//生肖开奖
                {
                    string[] VoteTemp = Vote.Split(",".ToCharArray());
                    for (int k = 0; k < VoteTemp.Length; k++)
                    {
                        string sVote = OutSxNum(VoteTemp[k]);
                        if (("," + sVote + ",").Contains("," + Result + ","))
                        {
                            IsWin = true;
                            Odds = Convert.ToDouble(ub.GetSub("Hc1odds2", xmlPath));
                            break;
                        }
                    }
                }
                else if (Types == 3)//方位开奖
                {
                    string[] VoteTemp = Vote.Split(",".ToCharArray());
                    for (int k = 0; k < VoteTemp.Length; k++)
                    {
                        string sVote = OutFwNum(VoteTemp[k]);
                        if (("," + sVote + ",").Contains("," + Result + ","))
                        {
                            IsWin = true;
                            Odds = Convert.ToDouble(ub.GetSub("Hc1odds3", xmlPath));
                            break;
                        }
                    }
                }
                else if (Types == 4)//四季开奖
                {
                    string[] VoteTemp = Vote.Split(",".ToCharArray());
                    for (int k = 0; k < VoteTemp.Length; k++)
                    {
                        string sVote = OutSjNum(VoteTemp[k]);
                        if (("," + sVote + ",").Contains("," + Result + ","))
                        {
                            IsWin = true;
                            Odds = Convert.ToDouble(ub.GetSub("Hc1odds4", xmlPath));
                            break;
                        }
                    }
                }
                else if (Types == 5)//大小单双
                {

                    if (("," + OutDXDS(Result) + ",").Contains("," + Vote + ","))
                    {
                        IsWin = true;
                        if (Vote == "大")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1da", xmlPath));
                        else if (Vote == "小")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1xiao", xmlPath));
                        else if (Vote == "单")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1dan", xmlPath));
                        else if (Vote == "双")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1shuang", xmlPath));

                    }
                }
                else if (Types == 6)//六肖
                {
                    if (("," + Vote + ",").Contains("," + OutSx(Result.ToString()) + ","))
                    {
                        IsWin = true;
                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds9", xmlPath));
                    }
                }
                else if (Types == 7)//尾数大小
                {
                    string wsdx = "";
                    int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
                    if (ws < 5)
                        wsdx = "小";
                    else
                        wsdx = "大";

                    if (wsdx == Vote)
                    {
                        IsWin = true;
                        if (Vote == "大")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1wsda", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("Hc1wsxiao", xmlPath));
                    }
                }
                else if (Types == 8)//尾数单双
                {
                    string wsds = "";
                    int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
                    if (ws % 2 == 0)
                        wsds = "双";
                    else
                        wsds = "单";

                    if (wsds == Vote)
                    {
                        IsWin = true;
                        if (Vote == "单")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1wsdan", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("Hc1wsshuang", xmlPath));
                    }
                }
                else if (Types == 9)//家禽野兽
                {
                    string jq = "牛,马,羊,鸡,狗,猪";
                    string ys = "鼠,猴,兔,虎,龙,蛇.";

                    string sVote = "";
                    if (Vote == "家禽")
                        sVote = jq;
                    else
                        sVote = ys;

                    if (("," + sVote + ",").Contains("," + OutSx(Result.ToString()) + ","))
                    {
                        IsWin = true;
                        if (Vote == "家禽")
                            Odds = Convert.ToDouble(ub.GetSub("Hc1odds14", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("Hc1odds15", xmlPath));
                    }
                }
                //else if (Types == 10)//自选不中
                //{
                //    if (!("," + Vote + ",").Contains("," + Result + ","))
                //    {
                //        IsWin = true;
                //        int cNum = Utils.GetStringNum(Vote, ",") + 1;
                //        if (cNum == 5)
                //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds16", xmlPath));
                //        else if (cNum == 6)
                //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds17", xmlPath));
                //        else if (cNum == 7)
                //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds18", xmlPath));
                //        else if (cNum == 8)
                //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds19", xmlPath));
                //        else if (cNum == 9)
                //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds20", xmlPath));
                //        else if (cNum == 10)
                //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds21", xmlPath));
                //    }
                //}

                if (IsWin == true)
                {
                    BCW.Data.SqlHelper.ExecuteSql("update tb_HcPay set WinCent=" + Convert.ToInt64(PayCent * Odds) + " Where id=" + pid + "");
                    //发送内线
                    new BCW.BLL.Guest().Add(1, UsID, UsName, "恭喜您在第" + model.CID + "期好彩一《" + ForType(Types) + "》投注，中奖" + Convert.ToInt64(PayCent * Odds) + "" + ub.Get("SiteBz") + "，开奖为" + Result + "-" + OutSx(Result.ToString()) + "-" + OutSj(Result.ToString()) + "-" + OutFw(Result.ToString()) + "[URL=/bbs/game/hc1.aspx?act=case]马上兑奖[/URL]");
                }
            }
        }
        //完成返彩后正式更新该期为结束
        BCW.Data.SqlHelper.ExecuteSql("update tb_HcList set Result='" + Result + "',State=1 Where CID=" + model.CID + "");
        BCW.Data.SqlHelper.ExecuteSql("update tb_HcPay set State=1 Where CID=" + model.CID + "");
        change_peilv();//赔率浮动
        Utils.Success("第" + model.CID + "期开奖", "第" + model.CID + "期开奖" + Result + "成功..", Utils.getUrl("hc1.aspx"), "1");
    }
    //大小单双和尾数单双赔率浮动
    public void change_peilv()
    {
        BCW.Model.Game.HcList model_2 = new BCW.BLL.Game.HcList().GetHcListNew(1);//开奖的的最新数据                                                                     
        string qihao = Convert.ToString(model_2.CID);
        string kai = model_2.Result;//开奖结果
                                    // Response.Write("开奖数据：" + model_2.CID + "<br />");
        ub xml = new ub();
        string xmlPath_update = "/Controls/hc1.xml";
        Application.Remove(xmlPath_update);//清缓存
        xml.ReloadSub(xmlPath_update); //加载配置
        #region 期号获取赔率浮动   
        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_2.CID.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                //获取最后一期期号
                d1 = new BCW.BLL.Game.HcList().GetList("TOP 1 *", "CID!='' and State=1 ORDER BY ID DESC");
                //获取倒数第二期期号
                d2 = new BCW.BLL.Game.HcList().GetList1("TOP 1 *", "id!='' and State=1 ORDER BY id ASC ");
                OutDX(Convert.ToInt32(model_2.Result));//最后一期大小
                OutDS(Convert.ToInt32(model_2.Result));//最后一期单双
                OutwsDX(Convert.ToInt32(model_2.Result));//最后一期尾数大小
                OutwsDS(Convert.ToInt32(model_2.Result));//最后一期尾数单双

                //  Response.Write("倒数第二期期号:" + "<b>" + OutDX(Convert.ToInt32(model_2.Result)) + "</b><br />");
                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                string Cents5 = "";
                string Cents6 = "";
                string Cents7 = "";
                string Cents8 = "";
                Cents1 = OutDX(Convert.ToInt32(model_2.Result));//最后一期的大小
                Cents2 = OutDS(Convert.ToInt32(model_2.Result));//最后一期的单双
                Cents5 = OutwsDX(Convert.ToInt32(model_2.Result));//最后一期尾数大小              
                Cents6 = OutwsDS(Convert.ToInt32(model_2.Result));//最后一期尾数单双

                //Response.Write("最后一期大小:" + "<b>" + Cents1 + "</b><br />");
                //    Response.Write("最后一期单双:" + "<b>" + Cents2 + "</b><br />");
                //    Response.Write("最后一期尾数大小:" + "<b>" + Cents5 + "</b><br />");
                //    Response.Write("最后一期尾数单双:" + "<b>" + Cents6 + "</b><br />");
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = OutDX(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期的大小
                        Cents4 = OutDS(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期的单双
                        Cents7 = OutwsDX(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期尾数的大小
                        Cents8 = OutwsDS(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期尾数的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                        Cents7 = "";
                        Cents8 = "";
                    }
                }
                //Response.Write("倒数第二期大小:" + "<b>" + Cents3 + "</b><br />");
                //Response.Write("倒数第二期单双:" + "<b>" + Cents4 + "</b><br />");
                //Response.Write("倒数第二期尾数大小:" + "<b>" + Cents7 + "</b><br />");
                //Response.Write("倒数第二期尾数单双:" + "<b>" + Cents8 + "</b><br />");
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Hc1da"] = xml.dss["Hc1odds5"];//da
                    xml.dss["Hc1xiao"] = xml.dss["Hc1odds6"];//xiao
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "小" && Cents3 == "小")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Hc1xiao"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {

                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率增加
                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "大" && Cents3 == "大")//大
                    {
                        if (Convert.ToDouble(xml.dss["Hc1da"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少        
                        }
                        else
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents5 != Cents7)//如果连续2期不相等，还原赔率----尾数大小
                {
                    xml.dss["Hc1wsda"] = xml.dss["Hc1odds10"];//da
                    xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];//xiao
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {

                    if (Cents5 == "小" && Cents7 == "小")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsxiao"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {

                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少

                        }
                        else
                        {
                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率增加
                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents5 == "大" && Cents7 == "大")//大
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsda"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {

                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少

                        }
                        else
                        {
                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Hc1dan"] = xml.dss["Hc1odds7"];
                    xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "单" && Cents4 == "单")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Hc1dan"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少                         
                        }
                        else
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率增加
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "双" && Cents4 == "双")//双
                    {
                        if (Convert.ToDouble(xml.dss["Hc1shuang"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents6 != Cents8)//如果连续2期不相等，还原赔率----尾数单双
                {
                    xml.dss["Hc1wsdan"] = xml.dss["Hc1odds12"];
                    xml.dss["Hc1wsshuang"] = xml.dss["Hc1odds13"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "单" && Cents4 == "单")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsdan"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率增加
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "双" && Cents4 == "双")//双
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsshuang"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Hc1da"] = xml.dss["Hc1odds5"];
            xml.dss["Hc1xiao"] = xml.dss["Hc1odds6"];
            xml.dss["Hc1dan"] = xml.dss["Hc1odds7"];
            xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
            xml.dss["Hc1wsda"] = xml.dss["Hc1odds10"];
            xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];
            xml.dss["Hc1wsdan"] = xml.dss["Hc1odds12"];
            xml.dss["Hc1wsshuang"] = xml.dss["Hc1odds13"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }

        #endregion
    }

    private void StatPage()
    {
        Master.Title = "好彩1赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=赢利分析=");
        builder.Append(Out.Tab("</div>", "<br />"));

        //今天本金与赢利

        long TodayBuyCent = new BCW.BLL.Game.HcPay().GetPayCent1(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
        long TodayWinCent = new BCW.BLL.Game.HcPay().GetWinCent1(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
        //昨天本金与赢利
        long YesBuyCent = new BCW.BLL.Game.HcPay().GetPayCent1(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");
        long YesWinCent = new BCW.BLL.Game.HcPay().GetWinCent1(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");
        //本月本金与赢利
        string strdatetime1 = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00";
        string strdatetime2 = DateTime.Parse(strdatetime1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";

        long MonthBuyCent = new BCW.BLL.Game.HcPay().GetPayCent1(strdatetime1, strdatetime2);
        long MonthWinCent = new BCW.BLL.Game.HcPay().GetWinCent1(strdatetime1, strdatetime2);

        //上月本金与赢利
        string strdatetime3 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01 00:00:00";
        string strdatetime4 = DateTime.Parse(strdatetime1).AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
        long Month2BuyCent = new BCW.BLL.Game.HcPay().GetPayCent1(strdatetime3, strdatetime4);
        long Month2WinCent = new BCW.BLL.Game.HcPay().GetWinCent1(strdatetime3, strdatetime4);

        //总本金与赢利
        long BuyCent = new BCW.BLL.Game.HcPay().GetPayCent1("", "");
        long WinCent = new BCW.BLL.Game.HcPay().GetWinCent1("", "");


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
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {

        Master.Title = "好彩一排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("好彩一排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //开始期数
        int number1 = int.Parse(Utils.GetRequest("number1", "all", 1, @"^[1-9]\d*$", "20159999"));
        //结束期数
        int number2 = int.Parse(Utils.GetRequest("number2", "all", 1, @"^[1-9]\d*$", "20159999"));
        if (Utils.ToSChinese(ac) == "确认搜索")
        {
            //开始时间
            string searchday1 = Utils.GetRequest("sTime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "1");
            //结束时间
            string searchday2 = Utils.GetRequest("oTime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "1");
            //查询条件  

            if (searchday1 == "")
            {
                searchday1 = DateTime.Now.ToString("yyyyMMdd") + "00:00:00";
            }
            if (searchday2 == "")
            {
                searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd") + "23:59:59";
            }

            string strText = "开始期数:/,结束期数:/,";
            string strName = "number1,number2,act";
            string strType = "num,num,hidden";
            string strValu = "" + "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,hc1.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            //判断期数是否输入期数
            if (number1 == 20159999 || number2 == 20159999)
            {
                //判断是否输入正确的时间格式
                if (Convert.ToInt32(searchday1) == 1 || Convert.ToInt32(searchday2) == 1)
                {
                    Utils.Error("输入时间格式错误！！", "");
                }
                else
                {
                    searchday1 = DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 00:00:00";
                    searchday2 = DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 23:59:59";
                    strWhere = "PayCents>0 and State>0 and AddTime between '" + searchday1 + "' and  '" + searchday2 + "'";
                }
            }
            else
            {
                strWhere = "PayCents>0 and CID>=" + number1 + " and CID<=" + number2;
            }
            string[] pageValUrl = { "act", "ac", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            DataSet ds = new BCW.BLL.Game.HcPay().GetList(" UsID,sum(WinCent-PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);//用户id
                    int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);//币额
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    //   builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;usid=" + UsID + "&amp;ptype=1") + "\">[" + id + "种果实]</a>");
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>赢" + id + "" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无数据!"));
            }
        }
        else
        {
            string strText = "开始期数:/,结束期数:/,";
            string strName = "number1,number2,act";
            string strType = "num,num,hidden";
            string strValu = "" + "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,hc1.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            strText = "开始日期:/,结束日期:/,";
            strName = "stime,otime,act";
            strType = "num,num,hidden";
            strValu = "" + "'" + "'" + Utils.getPage(0) + "";
            strEmpt = "true,true,false";
            strIdea = "/";
            strOthe = "确认搜索,hc1.aspx?act=top,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "PayCents>0";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            DataSet ds = new BCW.BLL.Game.HcPay().GetList(" UsID,sum(WinCent-PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);//用户id
                    int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);//用户id
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    //   builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;usid=" + UsID + "&amp;ptype=1") + "\">[" + id + "种果实]</a>");
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>赢" + id + "" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无数据!"));
            }
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //排行榜前10名奖励
    private void Top10Page()
    {
        Master.Title = "排行榜奖励";
        Master.Title = "好彩一排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("好彩一排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append("根据日期搜索排行榜前10名：<br />");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", "20150000"));
        int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", "20159999"));
        string rewardid = "";

        if (Utils.ToSChinese(ac) == "确认搜索")
        {
            string searchday1 = Utils.GetRequest("sTime", "all", 2, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
            string searchday2 = Utils.GetRequest("oTime", "all", 2, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
            //查询条件  

            if (searchday1 == "")
            {
                searchday1 = DateTime.Now.ToString("yyyyMMdd");
            }
            if (searchday2 == "")
            {
                searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            }

            string strText = "开始日期:/,结束日期:/,";
            string strName = "stime,otime,act";
            string strType = "num,num,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,hc1.aspx?act=Top10,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(".<br />");
            DataSet ds = new BCW.BLL.Game.HcPay().GetList("Top 10 UsID,sum(WinCent-PayCents)as bbb ", "State>0 and AddTime between '" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and  '" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'GROUP BY UsID order by sum(WinCent-PayCents) DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;
                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    builder.Append("Top" + (i + 1) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString())) + "</a>" + "净赚" + (Convert.ToInt64(ds.Tables[0].Rows[i]["bbb"]) + ub.Get("SiteBz") + "<br />"));
                }
            }
            string strText2 = ",,,,";
            string strName2 = "stime,otime,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden";
            string strValu2 = searchday1 + "'" + searchday2 + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = "奖励发放,hc1.aspx?act=reward,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            builder.Append("<br />");
        }
        else
        {
            try
            {
                string strText = "开始日期（输入格式如20150121是2015-01-21）:/,结束日期:/,";
                string strName = "stime,otime,act";
                string strType = "num,num,hidden";
                string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,false";
                string strIdea = "/";
                string strOthe = "确认搜索,hc1.aspx?act=Top10,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append("<br />");
            }
            catch
            {
                builder.Append("有问题！");
            }
            DataSet ds = new BCW.BLL.Game.HcPay().GetList("Top 10 UsID,sum(WinCent-PayCents)as bbb ", "State>0 and AddTime   between '" + (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and  '" + (DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'GROUP BY UsID order by sum(WinCent-PayCents) DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;
                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    builder.Append("Top" + (i + 1) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString())) + "</a>" + "净赚" + (Convert.ToInt64(ds.Tables[0].Rows[i]["bbb"]) + ub.Get("SiteBz") + "<br />"));
                }
            }

            string strText2 = ",,,,";
            string strName2 = "stime,otime,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden";
            string strValu2 = DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + rewardid + "'reward";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = "奖励发放,hc1.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            builder.Append("<br />");
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void RewardPage()
    {
        Master.Title = "奖励发放";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        builder.Append("奖励发放<br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append("发放id：");

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string searchday1 = Utils.GetRequest("stime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
        string searchday2 = Utils.GetRequest("otime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
        //查询条件  


        DataSet ds = new BCW.BLL.Game.HcPay().GetList("Top 10 UsID,sum(WinCent-PayCents)as bbb ", "State>0 and AddTime between '" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and  '" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'GROUP BY UsID order by sum(WinCent-PayCents) DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int TodayBuy = 0;
                try
                {
                    TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                }
                catch
                {
                    continue;
                }
                builder.Append(" " + ds.Tables[0].Rows[i]["UsID"] + "#");
            }
        }
        //查询条件  
        if (Utils.ToSChinese(ac) == "确认")
        {
            int[] IdRe = new int[10];
            int[] Top = new int[10];
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;

                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    Top[i] = TodayBuy;
                }
                IdRe[0] = int.Parse(Utils.GetRequest("top1", "post", 1, "", ""));
                IdRe[1] = int.Parse(Utils.GetRequest("top2", "post", 1, "", ""));
                IdRe[2] = int.Parse(Utils.GetRequest("top3", "post", 1, "", ""));
                IdRe[3] = int.Parse(Utils.GetRequest("top4", "post", 1, "", ""));
                IdRe[4] = int.Parse(Utils.GetRequest("top5", "post", 1, "", ""));
                IdRe[5] = int.Parse(Utils.GetRequest("top6", "post", 1, "", ""));
                IdRe[6] = int.Parse(Utils.GetRequest("top7", "post", 1, "", ""));
                IdRe[7] = int.Parse(Utils.GetRequest("top8", "post", 1, "", ""));
                IdRe[8] = int.Parse(Utils.GetRequest("top9", "post", 1, "", ""));
                IdRe[9] = int.Parse(Utils.GetRequest("top10", "post", 1, "", ""));
                builder.Append("<br />");
                for (int i = 0; i < 10; i++)
                {

                    if (IdRe[i] != 0)
                    {
                        builder.Append("【第" + (i + 1) + "名】：");
                        builder.Append(Top[i] + " 获得：");
                        builder.Append(IdRe[i] + "币<br />");
                    }
                    else
                    {
                        builder.Append("【第" + (i + 1) + "名】：获得0币，请返回重新输入！<br />");
                    }

                }
            }
            string strText = ":/," + ":/," + "Top" + (1) + ":/," + "Top" + (2) + ":/," + "Top" + (3) + ":/," + "Top" + (4) + ":/," + "Top" + (5) + ":/," + "Top" + (6) + ":/," + "Top" + (7) + ":/," + "Top" + (8) + ":/," + "Top" + (9) + ":/," + "Top" + (10) + ":/,";
            string strName = "stime,otime,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,backurl";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = searchday1 + "'" + searchday2 + "'" + IdRe[0] + "'" + IdRe[1] + "'" + IdRe[2] + "'" + IdRe[3] + "'" + IdRe[4] + "'" + IdRe[5] + "'" + IdRe[6] + "'" + IdRe[7] + "'" + IdRe[8] + "'" + IdRe[9] + "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "ture,ture,true,true,true,true,true,true,true,true,true,true,false";
            string strIdea = "/";
            string strOthe = "确认发放,hc1.aspx?act=reward1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            strText = ":/," + ":/," + "Top" + (1) + ":/," + "Top" + (2) + ":/," + "Top" + (3) + ":/," + "Top" + (4) + ":/," + "Top" + (5) + ":/," + "Top" + (6) + ":/," + "Top" + (7) + ":/," + "Top" + (8) + ":/," + "Top" + (9) + ":/," + "Top" + (10) + ":/,";
            strName = "stime,otime,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,backurl";
            strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = searchday1 + "'" + searchday2 + "'" + IdRe[0] + "'" + IdRe[1] + "'" + IdRe[2] + "'" + IdRe[3] + "'" + IdRe[4] + "'" + IdRe[5] + "'" + IdRe[6] + "'" + IdRe[7] + "'" + IdRe[8] + "'" + IdRe[9] + "'" + "'" + Utils.getPage(0) + "";
            strEmpt = "ture,ture,true,true,true,true,true,true,true,true,true,true,false";
            strIdea = "/";
            strOthe = "返回,hc1.aspx?act=reward,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");

        }
        else
        {
            int[] IdRe = new int[10];
            int[] Top = new int[10];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int TodayBuy = 0;

                    try
                    {
                        TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    }
                    catch
                    {
                        continue;
                    }
                    Top[i] = TodayBuy;
                }

                string strText = ":/," + ":/," + "Top" + (1) + ":/," + "Top" + (2) + ":/," + "Top" + (3) + ":/," + "Top" + (4) + ":/," + "Top" + (5) + ":/," + "Top" + (6) + ":/," + "Top" + (7) + ":/," + "Top" + (8) + ":/," + "Top" + (9) + ":/," + "Top" + (10) + ":/,";
                string strName = "stime,otime,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,backurl";
                string strType = "hidden,hidden,num,num,num,num,num,num,num,num,num,num,hidden";
                string strValu = searchday1 + "'" + searchday2 + "'" + IdRe[0] + "'" + IdRe[1] + "'" + IdRe[2] + "'" + IdRe[3] + "'" + IdRe[4] + "'" + IdRe[5] + "'" + IdRe[6] + "'" + IdRe[7] + "'" + IdRe[8] + "'" + IdRe[9] + "'" + "'" + Utils.getPage(0) + "";
                string strEmpt = "ture,ture,true,true,true,true,true,true,true,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确认,hc1.aspx?act=reward,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append("<br />");
            }
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void Reward1Page()
    {
        string searchday1 = Utils.GetRequest("stime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "开始时间填写错误");
        string searchday2 = Utils.GetRequest("otime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "结束时间填写错误");
        //查询条件  
        DataSet ds = new BCW.BLL.Game.HcPay().GetList("Top 10 UsID,sum(WinCent-PayCents)as bbb ", "State>0 and AddTime between '" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and  '" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'GROUP BY UsID order by sum(WinCent-PayCents) DESC");
        int[] IdRe = new int[10];
        int[] Top = new int[10];
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                int TodayBuy = 0;

                try
                {
                    TodayBuy = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                }
                catch
                {
                    continue;
                }
                Top[i] = TodayBuy;
            }
            IdRe[0] = int.Parse(Utils.GetRequest("top1", "post", 1, "", ""));
            IdRe[1] = int.Parse(Utils.GetRequest("top2", "post", 1, "", ""));
            IdRe[2] = int.Parse(Utils.GetRequest("top3", "post", 1, "", ""));
            IdRe[3] = int.Parse(Utils.GetRequest("top4", "post", 1, "", ""));
            IdRe[4] = int.Parse(Utils.GetRequest("top5", "post", 1, "", ""));
            IdRe[5] = int.Parse(Utils.GetRequest("top6", "post", 1, "", ""));
            IdRe[6] = int.Parse(Utils.GetRequest("top7", "post", 1, "", ""));
            IdRe[7] = int.Parse(Utils.GetRequest("top8", "post", 1, "", ""));
            IdRe[8] = int.Parse(Utils.GetRequest("top9", "post", 1, "", ""));
            IdRe[9] = int.Parse(Utils.GetRequest("top10", "post", 1, "", ""));
            builder.Append("<br />");
            for (int i = 0; i < 10; i++)
            {

                if (IdRe[i] != 0)
                {
                    new BCW.BLL.User().UpdateiGold(Top[i], IdRe[i], "好彩1排行榜奖励");
                    //发内线
                    string strLog = "您在" + DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "到" + DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "的好彩1排行榜" + "上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + IdRe[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/hc1.aspx]进入好彩1[/url]";
                    new BCW.BLL.Guest().Add(0, Top[i], new BCW.BLL.User().GetUsName(Top[i]), strLog);
                    //动态
                    string mename = new BCW.BLL.User().GetUsName(Top[i]);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + Top[i] + "]" + mename + "[/url]在[url=/bbs/game/hc1.aspx]好彩1[/url]" + "上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + IdRe[i] + "" + ub.Get("SiteBz");
                    new BCW.BLL.Action().Add(26, 0, Top[i], mename, wText);
                }
                else
                {

                }
            }
        }
        Utils.Success("好彩1游戏奖励派发", "派发成功，正在返回..", Utils.getUrl("hc1.aspx?backurl=" + Utils.getPage(0) + ""), "1");
    }
    private string ForType(int Types)
    {

        string TyName = string.Empty;
        if (Types == 1)
            TyName = "选号玩法";
        else if (Types == 2)
            TyName = "生肖玩法";
        else if (Types == 3)
            TyName = "方位玩法";
        else if (Types == 4)
            TyName = "四季玩法";
        else if (Types == 5)
            TyName = "大小单双";
        else if (Types == 6)
            TyName = "六肖中奖";
        else if (Types == 7)
            TyName = "尾数大小";
        else if (Types == 8)
            TyName = "尾数单双";
        else if (Types == 9)
            TyName = "家禽野兽";
        else if (Types == 10)
            TyName = "自选不中";

        return TyName;
    }

    private string OutSxNum(string sx)
    {
        string sNum = "";
        if (sx == "鼠")
            sNum = ",1,13,25,";
        else if (sx == "牛")
            sNum = ",2,14,26,";
        else if (sx == "虎")
            sNum = ",3,15,27,";
        else if (sx == "兔")
            sNum = ",4,16,28,";
        else if (sx == "龙")
            sNum = ",5,17,29,";
        else if (sx == "蛇")
            sNum = ",6,18,30,";
        else if (sx == "马")
            sNum = ",7,19,31,";
        else if (sx == "羊")
            sNum = ",8,20,32,";
        else if (sx == "猴")
            sNum = ",9,21,33,";
        else if (sx == "鸡")
            sNum = ",10,22,34,";
        else if (sx == "狗")
            sNum = ",11,23,35,";
        else if (sx == "猪")
            sNum = ",12,24,36,";

        return sNum;
    }

    private string OutFwNum(string fw)
    {
        string sNum = "";
        if (fw == "东")
            sNum = ",1,3,5,7,9,11,13,15,17,";
        else if (fw == "南")
            sNum = ",2,4,6,8,10,12,14,16,18,";
        else if (fw == "西")
            sNum = ",19,21,23,25,27,29,31,33,35,";
        else if (fw == "北")
            sNum = ",20,22,24,26,28,30,32,34,36,";

        return sNum;
    }

    private string OutSjNum(string sj)
    {
        string sNum = "";
        if (sj == "春")
            sNum = ",1,2,3,4,5,6,7,8,9,";
        else if (sj == "夏")
            sNum = ",10,11,12,13,14,15,16,17,18,";
        else if (sj == "秋")
            sNum = ",19,20,21,22,23,24,25,26,27,";
        else if (sj == "冬")
            sNum = ",28,29,30,31,32,33,34,35,36,";

        return sNum;
    }
    private string OutSx(string Result)
    {
        string sx = "";
        if ((",1,13,25,").Contains("," + Result + ","))
            sx = "鼠";
        else if ((",2,14,26,").Contains("," + Result + ","))
            sx = "牛";
        else if ((",3,15,27,").Contains("," + Result + ","))
            sx = "虎";
        else if ((",4,16,28,").Contains("," + Result + ","))
            sx = "兔";
        else if ((",5,17,29,").Contains("," + Result + ","))
            sx = "龙";
        else if ((",6,18,30,").Contains("," + Result + ","))
            sx = "蛇";
        else if ((",7,19,31,").Contains("," + Result + ","))
            sx = "马";
        else if ((",8,20,32,").Contains("," + Result + ","))
            sx = "羊";
        else if ((",9,21,33,").Contains("," + Result + ","))
            sx = "猴";
        else if ((",10,22,34,").Contains("," + Result + ","))
            sx = "鸡";
        else if ((",11,23,35,").Contains("," + Result + ","))
            sx = "狗";
        else if ((",12,24,36,").Contains("," + Result + ","))
            sx = "猪";

        return sx;
    }

    private string OutFw(string Result)
    {
        string fw = "";
        if ((",1,3,5,7,9,11,13,15,17,").Contains("," + Result + ","))
            fw = "东";
        else if ((",2,4,6,8,10,12,14,16,18,").Contains("," + Result + ","))
            fw = "南";
        else if ((",19,21,23,25,27,29,31,33,35,").Contains("," + Result + ","))
            fw = "西";
        else if ((",20,22,24,26,28,30,32,34,36,").Contains("," + Result + ","))
            fw = "北";

        return fw;
    }

    private string OutSj(string Result)
    {
        string sj = "";
        if ((",1,2,3,4,5,6,7,8,9,").Contains("," + Result + ","))
            sj = "春";
        if ((",10,11,12,13,14,15,16,17,18,").Contains("," + Result + ","))
            sj = "夏";
        if ((",19,20,21,22,23,24,25,26,27,").Contains("," + Result + ","))
            sj = "秋";
        if ((",28,29,30,31,32,33,34,35,36,").Contains("," + Result + ","))
            sj = "冬";

        return sj;
    }

    private string OutDXDS(int Result)
    {
        string dxds = "";
        int i = Result;
        if (i <= 18)
            dxds += "小,";
        else
            dxds += "大,";

        if (i % 2 == 0)
            dxds += "双";
        else
            dxds += "单";

        return dxds;
    }
    private string OutDS(int Result)
    {
        string dxds = "";
        int i = Result;
        if (i % 2 == 0)
            dxds += "双";
        else
            dxds += "单";
        return dxds;

    }
    private string OutDX(int Result)
    {
        string dxds = "";

        int i = Result;
        if (i <= 18)
            dxds += "小";
        else
            dxds += "大";
        return dxds;
    }
    private string OutwsDX(int Result)
    {
        string wsdx = "";
        int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
        if (ws < 5)
            wsdx = "小";
        else
            wsdx = "大";
        return wsdx;
    }
    private string OutwsDS(int Result)
    {
        string wsds = "";
        int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
        if (ws % 2 == 0)
            wsds = "双";
        else
            wsds = "单";
        return wsds;
    }


    //机器人ID设置
    private void ReBot()
    {
        Master.Title = "机器人设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("hc1.aspx") + "\">好彩1</a>&gt;");
        builder.Append("机器人设置");
        builder.Append(Out.Tab("</div>", ""));

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string RoBotID = Utils.GetRequest("RoBotID", "post", 1, "", xml.dss["hc1ROBOTID"].ToString());
            string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["hc1IsBot"].ToString());
            string RoBotCost = Utils.GetRequest("RoBotCost", "post", 1, @"^[0-9]\d*$", xml.dss["hc1ROBOTCOST"].ToString());
            string RoBotCount = Utils.GetRequest("RoBotCount", "post", 1, @"^[0-9]\d*$", xml.dss["hc1ROBOTBUY"].ToString());
            xml.dss["hc1ROBOTID"] = RoBotID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["hc1IsBot"] = IsBot;
            xml.dss["hc1ROBOTCOST"] = RoBotCost;
            xml.dss["hc1ROBOTBUY"] = RoBotCount;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("修改机器人", "修改成功，正在返回..", Utils.getUrl("hc1.aspx?act=rebot"), "1");
        }
        else
        {
            string strText = "机器人ID:/,机器人状态:/,机器人单注投注倍数:/,机器人每期购买订单数:/";
            string strName = "RoBotID,IsBot,RoBotCost,RoBotCount";
            string strType = "big,select,text,text";
            string strValu = xml.dss["hc1ROBOTID"].ToString() + "'" + xml.dss["hc1IsBot"].ToString() + "'" + xml.dss["hc1ROBOTCOST"].ToString() + "'" + xml.dss["hc1ROBOTBUY"].ToString();
            string strEmpt = "true,0|关闭|1|开启,true,false";
            string strIdea = "/";
            string strOthe = "确定修改,hc1.aspx?act=rebot,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        //builder.Append("<br />");
        //builder.Append("温馨提示:多个机器人ID请用#分隔。<br />");
        //builder.Append("当前机器人ID为：<b style=\"color:red\">" + xml.dss["hc1ROBOTID"].ToString() + "</b><br />");
        //if (xml.dss["hc1IsBot"].ToString() == "0")
        //{
        //    builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
        //}
        //else
        //{
        //    builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
        //}
        //builder.Append("当前机器人单注投注倍数：<b style=\"color:red\">" + xml.dss["hc1ROBOTCOST"].ToString() + "</b><br />");
        //builder.Append("当前机器人每期购买彩票数：<b style=\"color:red\">" + xml.dss["hc1ROBOTBUY"].ToString() + "</b><br />");
        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("hc1.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}