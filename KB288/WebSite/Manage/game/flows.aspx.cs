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

/// <summary>
/// 修改拾物奖励
/// 
/// 黄国军 20160328
/// 
/// 邵广林 20161013 增加拾物控制
/// </summary>
public partial class Manage_game_flows : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();
                break;
            case "ok":
                OkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "活动日志管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("活动日志管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=1") + "\">3个x33种共99个物品=8000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("2.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=2") + "\">2个x33种共66个物品=3000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("3.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=3") + "\">1个x33种共33个物品=300" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("4.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=4") + "\">不同物品32个物品=300" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("5.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=5") + "\">不同物品30个物品=200" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("6.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=6") + "\">不同物品25个物品=200" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("7.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=7") + "\">不同物品20个物品=80" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("8.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=8") + "\">不同物品15个物品=商城礼物1个</a><br />");
        builder.Append("9.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=9") + "\">不同物品10个物品=1000" + ub.Get("SiteBz2") + "</a><br />");

        builder.Append("10.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=10") + "\">物品总数5000个=8000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("11.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=11") + "\">物品总数3000个=5000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("12.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=12") + "\">物品总数2000个=二个月VIP</a><br />");
        builder.Append("13.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=13") + "\">物品总数1500个=一个月VIP</a><br />");

        builder.Append("14.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=14") + "\">物品总数1200个=1500" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("15.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=15") + "\">物品总数1000个=1200" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("16.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=16") + "\">物品总数800个=300" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("17.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=17") + "\">物品总数500个=2000" + ub.Get("SiteBz2") + "</a><br />");
        builder.Append("18.<a href=\"" + Utils.getUrl("flows.aspx?act=view&amp;ptype=18") + "\">物品总数300个=1000" + ub.Get("SiteBz2") + "</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        //邵广林 20161013 增加拾物控制
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/wap.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string shiwu = Utils.GetRequest("shiwu", "post", 2, @"^[0-2]$", "0");
            xml.dss["shiwu"] = shiwu;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("拾物设置", "修改成功，正在返回..", Utils.getUrl("flows.aspx"), "1");
        }
        else
        {
            string strText = "拾物状态：";
            string strName = "shiwu";
            string strType = "select";
            string strValu = "" + xml.dss["shiwu"] + "";
            string strEmpt = "0|正常|1|维护,";
            string strIdea = "/";
            string strOthe = "确定修改,flows.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ViewPage()
    {
        Master.Title = "日志列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("日志列表");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型错误"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "Types=" + ptype + "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.GiftChange> listGiftChange = new BCW.BLL.Game.GiftChange().GetGiftChanges(pageIndex, pageSize, strWhere, out recordCount);
        if (listGiftChange.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.GiftChange n in listGiftChange)
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

                builder.Append("兑换人:<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                builder.Append("<br />描述:" + ((n.Notes == "") ? "无" : n.Notes) + "(" + DT.FormatDate(n.AddTime, 0) + ")");
                if (n.Types == 1 || n.Types == 2 || n.Types == 10 || n.Types == 11)
                {
                    if (n.State == 0)
                        builder.Append("<br />兑换状态:<a href=\"" + Utils.getUrl("flows.aspx?act=ok&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未完成</a>");
                    else
                        builder.Append("<br />兑换状态:<a href=\"" + Utils.getUrl("flows.aspx?act=ok&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">已完成</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void OkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.GiftChange model = new BCW.BLL.Game.GiftChange().GetGiftChange(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string Message = Utils.GetRequest("Message", "post", 3, @"^[\s\S]{1,300}$", "内线内容最多300字");
            int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-1]\d*$", "选择状态错误"));
            if (Message != "")
            {
                //发送内线
                new BCW.BLL.Guest().Add(model.UsID, model.UsName, Message);
            }
            //更新状态
            new BCW.BLL.Game.GiftChange().UpdateState(id, State);

            Utils.Success("完成兑换操作", "完成兑换操作成功..", Utils.getUrl("flows.aspx?act=ok&amp;id=" + id + ""), "1");
        }
        else
        {
            Master.Title = "完成兑换";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("完成兑换");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "发送内线(留空则不发送内线):/,兑换状态:/,,,";
            string strName = "Message,State,id,act,info";
            string strType = "text,select,hidden,hidden,hidden";
            string strValu = "'" + model.State + "'" + id + "'ok'ok'";
            string strEmpt = "true,0|未完成|1|已完成,false,false,false";
            string strIdea = "/";
            string strOthe = "确定,flows.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
