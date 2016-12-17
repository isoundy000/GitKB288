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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class Manage_guestsend : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string manageName = Convert.ToString(ub.GetSub("guestmanageName", "/Controls/guestsend.xml"));
    protected string[] type1 = { "在线会员", "隐身会员", "所有会员", "指定会员", "一周内登录", "一个月内登录", "二个月内登录", "三个月内登录", "半年内登录", "一年内登录", "一年前登录", "未验证会员" };
    protected string type2 = "0|在线会员|1|隐身会员|2|所有会员|3|指定会员|4|一周内登录|5|一个月内登录|6|二个月内登录|7|三个月内登录|8|半年内登录|9|一年内登录|10|一年前登录|11|未验证会员";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
       // Master.Refresh = 3600;
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        switch (act)
        {
            case "view":
                SendListPage();
                break;
            case "add":
                SelectPage();
                break;
            case "send":
                AGuestSendPage();
                break;
            case "dele":
                DelePage();
                break;
            case "edit":
                EditPage();
                break;
            case "adata":
                AGuestSendPage();
                break;
            case "set":
                SetPage();
                break;
            case "deleList":
                DeleListPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "flashing":
                SendGuestTimer();
                break;
            case "data":
                DataPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    //定时发送的内线
    /// <summary>
    /// 检测guestsendlist表的guestsendID标识
    /// type=1时
    /// </summary>
    public void SendGuestTimer()
    {
        // string guestsendflash = ub.GetSub("guestsendflash", "/Controls/guestsend.xml");
        //if (guestsendflash != "0")
        //{
        //    return ; 
        //}
        string strWhere = " isDone=1 and  sentDay>=isSendCount ";
        //内线控制表
        DataSet dssend = new BCW.BLL.tb_GuestSend().GetList(" * ", strWhere);
        DataSet dssendList = null;
        BCW.Model.tb_GuestSend mm = new BCW.Model.tb_GuestSend();
        int guessCount = 0;
        if (dssend != null && dssend.Tables[0].Rows.Count > 0)
        {
            string username = "";
            string guestContent = "";
            int guestid = 0;
            BCW.Model.tb_GuestSendList list = new BCW.Model.tb_GuestSendList();
            for (int i = 0; i < dssend.Tables[0].Rows.Count; i++)
            {
                guestContent = dssend.Tables[0].Rows[i]["guestContent"].ToString();
                //发送列控制表
                dssendList = new BCW.BLL.tb_GuestSendList().GetList("  usid,guestsendID,type ", " type=1 and guestsendID=" + dssend.Tables[0].Rows[i]["ID"].ToString() + " group by usid,guestsendID,type ");
                int userid = 0;
                //同一小时内比较
                if (DateTime.Now.Hour.ToString() == (dssend.Tables[0].Rows[i]["sendTime"].ToString().Trim()))
                {
                    // builder.Append((DateTime.Now.Hour.ToString() + "==" + dssend.Tables[0].Rows[i]["sendTime"].ToString().Trim()));
                    mm = new BCW.BLL.tb_GuestSend().Gettb_GuestSend(Convert.ToInt32(dssend.Tables[0].Rows[i]["ID"]));
                    if (Convert.ToDateTime(mm.sendDateTime).Day == DateTime.Now.Day)
                    { continue; }
                    if (dssendList != null && dssendList.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dssendList.Tables[0].Rows.Count; j++)
                        {
                            if (!("#" + dssend.Tables[0].Rows[i]["seeUidList"].ToString()).Contains("#" + dssendList.Tables[0].Rows[j]["usid"].ToString() + "#"))
                            {
                                //为1时未阅读
                                if (dssendList.Tables[0].Rows[j]["type"].ToString().Trim() == "1")
                                {
                                    guessCount++;
                                    userid = Convert.ToInt32(dssendList.Tables[0].Rows[j]["usid"]);
                                    username = new BCW.BLL.User().GetUsName(userid);
                                    guestid = new BCW.BLL.Guest().AddNew(5, userid, username, guestContent);
                                    list.addtime = DateTime.Now;
                                    list.getGold = 0;
                                    list.guestID = guestid;
                                    list.guestsendID = Convert.ToInt32(dssendList.Tables[0].Rows[j]["guestsendID"]);
                                    list.idDone = 1;
                                    list.overtime = DateTime.Now.AddDays(30);
                                    list.remark = "";
                                    list.type = 1;//未阅读
                                    list.usid = userid;
                                    int aa = new BCW.BLL.tb_GuestSendList().Add(list);
                                }
                            }
                        }
                    }
                    mm.remark = " " + DateTime.Now.ToString("yyyy-MM-dd ") + mm.sendTime.ToString().Trim() + " 时内线手动刷新已发送";
                    mm.sendCount += guessCount;//总发送量
                    mm.isSendCount += 1;
                    mm.sendDateTime = DateTime.Now;
                    if (mm.isSendCount == mm.sentDay)
                    {
                        mm.overtime = DateTime.Now;
                        mm.isDone = 0;//正式停止标识s
                        mm.remark = " " + DateTime.Now.ToString("yyyy-MM-dd ") + mm.sendTime.ToString().Trim() + " 时手动内线已发送(已完结)";
                    }
                    //操作，放回guestsend
                    new BCW.BLL.tb_GuestSend().Update(mm);
                }
            }
        }
        Utils.Success("刷新成功,3s后返回", "检测定时内线刷新成功,当前发送" + guessCount + "条,3s后返回...", Utils.getUrl("guestsend.aspx?backurl=" + Utils.getPage(0) + ""), "3");
        //builder.Append("<script language=\"javascript\">window.history.back(-1);</script>");
    }

    //拼手气红包算法
    public string HaobaoSF(int number, long total)////number红包数——total红包总额
    {
        long money = 0;
        ////最小红包
        double min = 10;
        double max;
        int i = 1;
        ArrayList math = new ArrayList();
        string lists = "";
        long totals = total;
        while (i < number)
        {
            //保证即使一个红包是最大的了,后面剩下的红包,每个红包也不会小于最小值
            max = total - min * (number - i);
            int k = (number - i) / 2;
            //保证最后两个人拿的红包不超出剩余红包
            if (number - i <= 2)
            {
                k = number - i;
            }
            //最大的红包限定的平均线上下
            max = max / k;

            //保证每个红包大于最小值,又不会大于最大值;
            Random rdm = new Random();
            double s = rdm.NextDouble();
            money = Convert.ToInt64(min * 100 + s * (max * 100 - min * 100 + 1));
            money = money / 100;
            //保留两位小数
            while (money == 0)
            {
                s = rdm.NextDouble();
                money = Convert.ToInt32(min * 100 + s * (max * 100 - min * 100 + 1));
                money = money / 100;
            }
            if (money > totals / 2)
            {
                money = totals / 2;
            }
            if (money < 10)
            {
                money = 10;
            }
            total = total * 100 - money * 100;
            total = total / 100;
            math.Add(money);
            lists = lists + money.ToString() + "#";
            //Response.Write("第" + i + "个人拿到" + money + "剩下" + total + "<br />");
            i++;
            //最后一个人拿走剩下的红包
            if (i == number)
            {
                math.Add(total);
                lists = lists + total + "#";
                //Response.Write("第" + i + "个人拿到" + total + "剩下0" + "<br />");
            }
        }
        if (number == 1)
        {
            math.Add(total);
            lists = money.ToString();
            // Response.Write("第" + i + "个人拿到" + total + "剩下0" + "<br />");
        }
        long maxmoney = 0;
        for (int v = 0; v < math.Count; v++)
        {
            if (Convert.ToInt64(math[v].ToString()) > maxmoney)
            {
                maxmoney = Convert.ToInt64(math[v].ToString());
            }
        }
        //取数组中最大的一个值的索引
        // Response.Write("本轮发红包中第" + (math.IndexOf(maxmoney) + 1) + "个人手气最佳" + "<br />");
        lists = lists + (math.IndexOf(maxmoney) + 1).ToString();
        return lists;
    }

    //底部
    private void Bottom()
    {
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //首页
    private void ReloadPage()
    {
        //SendGuestTimer();
        Master.Title = manageName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(manageName);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div  class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=add") + "\">发布内线</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=view") + "\">内线管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=data") + "\">数据分析</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=set") + "\">发送配置</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=reset") + "\">重置数据</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=flashing&amp;") + "\">刷新发送</a><br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append("<script language=\"javascript\">history.go(-" + 1 + ");</script>");
    }

    //data 数据分析
    private void DataPage()
    {
        Master.Title = manageName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">" + manageName + "</a>&gt;数据分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        try
        {
            string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
            string down = Utils.GetRequest("down", "all", 1, @"^[^\^]{0,2000}$", "0");
            int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
            string strWhere = "";
            string strWhereAll = "";
            if (start != "0")
            {
                strWhere = " and  overtime> '" + Convert.ToDateTime(start) + "' and overtime< '" + Convert.ToDateTime(down) + "'";
                strWhereAll = "overtime> '" + Convert.ToDateTime(start) + "' and overtime< '" + Convert.ToDateTime(down) + "'";
                builder.Append(Out.Tab("<div  class=\"text\">", "<br />"));
                builder.Append("从" + start + "到" + down + "");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                start = DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd HH:mm:ss");
                down = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            builder.Append(Out.Tab("<div  class=\"text\">", ""));
            int listmax = 0;
            DataSet dsall = new BCW.BLL.tb_GuestSendList().GetList("  sum(getGold) as gold ", strWhereAll);
            DataSet dsCount = new BCW.BLL.tb_GuestSendList().GetList("ID ", " type=0" + strWhere);
            DataSet issee = new BCW.BLL.tb_GuestSendList().GetList("ID ", " type!=1" + strWhere);
            DataSet notsee = new BCW.BLL.tb_GuestSendList().GetList("ID ", " type=1 " + strWhere);
            DataSet notHit = new BCW.BLL.tb_GuestSendList().GetList("ID ", " type=4" + strWhere);
            DataSet dsHitHB = new BCW.BLL.tb_GuestSendList().GetList("ID ", " type=0" + strWhere);
            DataSet sendMax = new BCW.BLL.tb_GuestSendList().GetList("ID ", " " + strWhereAll);
            listmax = sendMax.Tables[0].Rows.Count;
            builder.Append("已发内线条数:<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=1&amp;") + "\"><font color=\"red\">" + listmax + "</font>" + "</a>条" + "<br/>");
            builder.Append("已阅内线条数:<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=1&amp;") + "\"><font color=\"red\">" + issee.Tables[0].Rows.Count + "</font>" + "</a>条" + "<br/>");
            builder.Append("未阅内线条数:<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=1&amp;") + "\"><font color=\"red\">" + ((notsee.Tables[0].Rows.Count)) + "</font>" + "</a>条" + "<br/>");
            builder.Append("已领红包个数:<a href =\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=2&amp;") + "\">" + "" + dsCount.Tables[0].Rows.Count + " </a>" + "个" + "<br/>");
            builder.Append("阅后无红包数:<font color=\"red\">" + notHit.Tables[0].Rows.Count + "</font>" + "个" + "<br/>");
            if (dsall.Tables[0].Rows.Count > 0)
                builder.Append("派送总红包额:<font color=\"red\">" + dsall.Tables[0].Rows[0]["gold"] + "</font>" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText1 = "开始时间:,结束时间:,,";
            string strName1 = "start,down,ptype,backurl";
            string strType1 = "text,text,hidden,hidden";
            string strValu1 = start + "'" + down + "'" + ptype + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,true,true,false";
            string strIdea1 = "/";
            string strOthe1 = "按时间搜索,guestsend.aspx?act=data&amp;ptype=" + ptype + "&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            builder.Append(Out.Tab("", "<br />"));
        }
        catch
        {
            builder.Append(Out.Tab("<div  class=\"text\">", "<br />"));
            builder.Append("暂无记录");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        Bottom();
    }

    //adata 内线发送一行记录
    private void AGuestSendPage()
    {
        Master.Title = manageName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">" + manageName + "</a>&gt;发送记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[0-9]\d*$", "0"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.tb_GuestSend().Exists(ID))
            Utils.Error("不存在该记录", "");
        BCW.Model.tb_GuestSend model = new BCW.BLL.tb_GuestSend().Gettb_GuestSend(ID);
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=edit&amp;ID=" + ID + "") + "\">编辑</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=dele&amp;ID=" + ID + "") + "\">删除</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=flashing&amp;ID=" + ID + "") + "\">发送</a><br/>");
        builder.Append("当前ID:" + model.ID + "" + "<br/>");
        builder.Append("");
        builder.Append("每天发送时间:" + model.sendTime + "时" + "<br/>");
        builder.Append("发送类型:" + type1[Convert.ToInt32(model.sendtype)] + "(" + model.sendCount + ")<br/>");
        builder.Append("发送内线总数:" + model.sendCount + "条" + "<br/>");
        builder.Append("已阅总数:" + model.seeCount + "条" + "<br/>");
        builder.Append("发送总天数:" + model.sentDay + "天" + "<br/>");
        builder.Append("当前发送次数:" + model.isSendCount + "次" + "<br/>");
        builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        builder.Append("红包总币数:" + model.allgold + "" + "<br/>");
        builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        builder.Append("红包剩余币数:" + model.nowgold + "" + "<br/>");
        builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        builder.Append("红包总个数:" + model.hbCount + "" + "<br/>");
        builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        string[] hb = model.hbList.Split('#');
        builder.Append("红包剩余数:" + (hb.Length - 1) + "" + "<br/>");
        string text1 = "已完结";
        if (model.isDone == 1)
        { text1 = "进行中"; }
        builder.Append("当前状态:" + model.remark + "[" + text1 + "]<br/>");
        builder.Append("最后一次发送时间:" + Convert.ToDateTime(model.sendDateTime).ToString("yyyy-MM-dd HH:mm") + "" + "<br/>");
        builder.Append("内线内容:");
        string text = "显示ˇ";
        if (type == 0)
        {
            text = "显示★";
            type = 1;
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=adata&amp;ID=" + ID + "&amp;type=" + type + "&amp;") + "\">" + text + "</a>");
        }
        else
        {
            text = "隐藏☆";
            type = 0;
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=adata&amp;ID=" + ID + "&amp;type=" + type + "&amp;") + "\">" + text + "</a>");
            builder.Append("<br/>" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.guestContent)));
        }
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = " guestsendID=" + ID + "  ";
        string[] pageValUrl = { "act", "ptype", "ID", "type" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        // guestsend开始读取列表
        IList<BCW.Model.tb_GuestSendList> listForumlog = new BCW.BLL.tb_GuestSendList().Gettb_GuestSendLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_GuestSendList n in listForumlog)
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
                builder.Append("" + k + ".");
                builder.Append("<a href =\"" + Utils.getUrl("./uinfo.aspx?uid=" + Convert.ToInt32(n.usid) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usid)) + "</a>" + "");

                builder.Append(getStatueType(Convert.ToInt32((n.type))));
                if (n.type == 0)
                {
                    builder.Append("<font color=\"red\">" + n.getGold + "</font>红包");
                    builder.Append("[" + Convert.ToDateTime(n.overtime).ToString("MM-dd HH:mm") + "]");
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
        Bottom();
    }

    //edit 编辑
    private void EditPage()
    {
        Master.Title = manageName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">" + manageName + "</a>&gt;编辑");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.tb_GuestSend().Exists(ID))
            Utils.Error("不存在该记录", "");
        BCW.Model.tb_GuestSend model = new BCW.BLL.tb_GuestSend().Gettb_GuestSend(ID);
        //builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=edit") + "\">编辑</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=dele") + "\">删除</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=send") + "\">发送</a><br/>");
        //builder.Append("当前ID:" + model.ID + "" + "<br/>");
        //builder.Append("每天发送时间:" + model.sendTime + "时" + "<br/>");
        //builder.Append("发送类型:" + type1[Convert.ToInt32(model.sendtype)] + "(" + model.sendCount + ")<br/>");
        //builder.Append("发送内线总数:" + model.sendCount + "条" + "<br/>");
        //builder.Append("已阅总数:" + model.seeCount + "条" + "<br/>");
        //builder.Append("发送总天数:" + model.sentDay + "天" + "<br/>");
        //builder.Append("当前发送次数:" + model.isSendCount + "次" + "<br/>");
        //builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        //builder.Append("红包总币数:" + model.allgold + "" + "<br/>");
        //builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        //builder.Append("红包剩余币数:" + model.nowgold + "" + "<br/>");
        //builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        //builder.Append("红包总个数:" + model.hbCount + "" + "<br/>");
        //builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
        //string[] hb = model.hbList.Split('#');
        //builder.Append("红包剩余数:" + (hb.Length - 1) + "" + "<br/>");
        //builder.Append("当前状态:" + model.remark + "<br/>");
        //builder.Append("最后一次发送时间:" + Convert.ToDateTime(model.sendDateTime).ToString("yyyy-MM-dd HH:mm") + "" + "<br/>");
        //builder.Append("内线内容:<br/>" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.guestContent)));
        string ac = Utils.GetRequest("ac", "all", 1, @"", "1");
        if (ac == "ok")
        {
            string sendCount = (Utils.GetRequest("sendCount", "all", 1, @"", "0"));//发送的内线ID
            string guestContent = Utils.GetRequest("guestContent", "all", 1, @"", "0");//内线内容
            string guestTimes = Utils.GetRequest("guestTimes", "all", 1, @"", "0");//发布时间点  
            string guestDays = Utils.GetRequest("guestDays", "all", 1, @"", "0");//天数
            string guestAllGold = Utils.GetRequest("guestAllGold", "all", 1, @"", "0");//总额
            string guestMaxCount = Utils.GetRequest("guestMaxCount", "all", 1, @"", "0");//最大中奖范围
            string guestHitCount = Utils.GetRequest("guestHitCount", "all", 1, @"", "0");   //中奖数
            string guestHbCount = Utils.GetRequest("guestHbCount", "all", 1, @"", "0");   //中奖数
            string isSendCount = Utils.GetRequest("isSendCount", "all", 1, @"", "0");   //中奖数
            string isDone = Utils.GetRequest("isDone", "all", 1, @"", "0");   //中奖数
            model.sendCount = Convert.ToInt32(sendCount);
            model.guestContent = guestContent;
            model.sendTime = guestTimes;
            model.sentDay = Convert.ToInt32(guestDays);
            model.allgold = Convert.ToInt32(guestAllGold);
            model.hbCount = Convert.ToInt32(guestHbCount);
            model.maxCount = Convert.ToInt32(guestMaxCount);
            model.getCount = Convert.ToInt32(guestHitCount);
            model.isSendCount = Convert.ToInt32(isSendCount);
            model.isDone = Convert.ToInt32(isDone);
            new BCW.BLL.tb_GuestSend().Update(model);
            Utils.Success("修改成功,3s后返回", "修改成功,3s后返回...", Utils.getUrl("guestsend.aspx?act=edit&amp;ID=" + ID + "&amp;backurl=" + Utils.getPage(0) + ""), "3");
        }
        else
        {
            string hour = "";
            for (int i = 0; i < 24; i++)
            {
                hour += i.ToString() + "|" + i.ToString() + ":00";
                if (i < 23)
                    hour += "|";
            }
            //builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            ////builder.Append("温馨提示:当前在线会员<font color=\"red\">" + ds.Tables[0].Rows.Count + "</font>个(不包括隐身会员)");
            //builder.Append(Out.Tab("</div>", ""));
            string strText = "当前ID:/,总发送内线量:/,内线内容:/,定时发送时间(每天24小时格式):/,发送天数:/,当前发送次数:/,红包总值:/,红包总个数:/,概率范围:/,范围内领红包数:/,";
            string strName = "ID,sendCount,guestContent,guestTimes,guestDays,isSendCount,guestAllGold,guestHbCount,guestMaxCount,guestHitCount,backurl";
            string strType = "text,text,big,select,text,text,text,text,text,text,hidden,";
            string strValu = model.ID + "'" + model.sendCount + "'" + model.guestContent + "'" + model.sendTime.ToString().Trim() + "'" + model.sentDay + "'" + model.isSendCount + "'" + model.allgold + "'" + model.hbCount + "'" + model.maxCount + "'" + model.getCount + "'" + Utils.getPage(0) + "";
            string strEmpt = type2 + ",true,true," + hour + ",true,true,true,true,true,false,true,";
            string strIdea = "/";
            string strOthe = "确定,guestsend.aspx?act=edit&amp;ac=ok&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append("<br/>");

        Bottom();

    }
    
    //dele 删除
    private void DelePage()
    {
        Master.Title = manageName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("guestsend.aspx") + "\">" + manageName + "</a>&gt;");
        builder.Append("删除记录");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ID = Convert.ToInt32(Utils.GetRequest("ID", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        if (ac == "ok")
        {
            if (ID == 0)
            {
                Utils.Error("ID错误！", "");
            }
            else
            {
                new BCW.BLL.tb_GuestSend().Delete(ID);
                Utils.Success("设置", "删除成功，正在返回..", Utils.getUrl("guestsend.aspx?act=view&amp;;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该记录吗？");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=dele&amp;ac=ok&amp;ID=" + ID + "&amp;ptype=" + ptype + "&amp;") + "\">确认删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;") + "\">取消</a><br/>");
            builder.Append("温馨提示：删除后该数据将不存在,用户已阅不进行派红包.");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(Out.Tab("", "<br />"));
        Bottom();
    }

    //deleList 删除
    private void DeleListPage()
    {
        Master.Title = manageName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("guestsend.aspx") + "\">" + manageName + "</a>&gt;");
        builder.Append("删除记录");
        builder.Append(Out.Tab("</div>", "<br/>"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ID = Convert.ToInt32(Utils.GetRequest("ID", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        if (ac == "ok")
        {
            if (ID == 0)
            {
                Utils.Error("ID错误！", "");
            }
            else
            {
                new BCW.BLL.tb_GuestSendList().Delete(ID);
                Utils.Success("设置", "删除成功，正在返回..", Utils.getUrl("guestsend.aspx?act=view&amp;ptype=" + 1 + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该记录吗？");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=deleList&amp;ac=ok&amp;ID=" + ID + "&amp;ptype=" + 1 + "&amp;") + "\">确认删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=" + 1 + "&amp;") + "\">取消</a><br/>");
            builder.Append("温馨提示：删除后该数据将不存在,用户已阅不进行派红包.");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(Out.Tab("", "<br />"));
        Bottom();
    }

    //view 按发布内容
    private void SendListPage()
    {
        Master.Title = manageName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">" + manageName + "</a>&gt;记录管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        if (ptype == 0)
        {
            builder.Append("按发布内容|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=0&amp;") + "\">按发布内容</a>|");
        }
        if (ptype == 1)
        {
            builder.Append("按阅读记录|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=1&amp;") + "\">按阅读记录</a>|");
        }
        if (ptype == 2)
        {
            builder.Append("按红包记录");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=view&amp;ptype=2&amp;") + "\">按红包记录</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "forumid", "bid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (ptype == 0)
        {
            //查询条件
            // guestsend开始读取列表
            IList<BCW.Model.tb_GuestSend> listForumlog = new BCW.BLL.tb_GuestSend().Gettb_GuestSends(pageIndex, pageSize, strWhere, out recordCount);
            if (listForumlog.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.tb_GuestSend n in listForumlog)
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
                    builder.Append("" + k + ".");
                    builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=adata&amp;ID=" + n.ID + "&amp;") + "\">" + strSub(n.guestContent.ToString()) + "</a>");
                    //builder.Append(strSub(n.guestContent));
                    builder.Append("[" + Convert.ToDateTime(n.addtime).ToString("MM-dd HH:mm") + "]");
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
        }
        else
        {
            if (ptype == 1)
            {
                strWhere = "      ";
                // guestsend开始读取列表
                IList<BCW.Model.tb_GuestSendList> listForumlog = new BCW.BLL.tb_GuestSendList().Gettb_GuestSendLists(pageIndex, pageSize, strWhere, out recordCount);
                if (listForumlog.Count > 0)
                {
                    int k  = 1;
                    foreach (BCW.Model.tb_GuestSendList n in listForumlog)
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
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        //builder.Append(new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usid)));                    
                        builder.Append("<a href =\"" + Utils.getUrl("./uinfo.aspx?uid=" + Convert.ToInt32(n.usid) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usid)) + "</a>" + "");
                        if (n.type == 0)
                        {
                            builder.Append(getStatueType(Convert.ToInt32((n.type))));
                            builder.Append("<font color=\"red\">" + n.getGold + "</font>");
                        }
                        else
                            if (n.type == 1)
                        {
                            builder.Append(getStatueType(Convert.ToInt32((n.type))));
                        }
                        else
                            if (n.type == 4)
                        {
                            builder.Append(getStatueType(Convert.ToInt32((n.type))));
                        }
                        else
                        {
                            builder.Append(getStatueType(Convert.ToInt32((n.type))));
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=adata&amp;ID=" + n.guestsendID + "&amp;") + "\">[标识" + n.guestsendID + "]</a>");
                        builder.Append("[" + Convert.ToDateTime(n.overtime).ToString("MM-dd HH:mm") + "]");
                        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=deleList&amp;ID=" + n.ID + "&amp;") + "\">[删]</a>");
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
            }
            else
            {
                strWhere = "   type=0 ";
                //guestsend开始读取列表
                IList<BCW.Model.tb_GuestSendList> listForumlog = new BCW.BLL.tb_GuestSendList().Gettb_GuestSendLists(pageIndex, pageSize, strWhere, out recordCount);
                if (listForumlog.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.Model.tb_GuestSendList n in listForumlog)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        builder.Append("<a href =\"" + Utils.getUrl("./uinfo.aspx?uid=" + Convert.ToInt32(n.usid) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usid)) + "</a>" + "");
                        builder.Append(getStatueType(Convert.ToInt32((n.type))));
                        if (n.getGold > 10000)
                        {
                            builder.Append("<font color=\"red\">" + n.getGold + "</font>");
                        }
                        else
                            builder.Append("<font color=\"green\">" + n.getGold + "</font>");
                        builder.Append("[" + Convert.ToDateTime(n.overtime).ToString("MM-dd HH:mm") + "]");
                        k++;
                        builder.Append("");
                        //builder.Append(n.guestsendID+"");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    //分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }
            }
        }
        builder.Append(Out.Tab("", "<br/>"));
        Bottom();
    }

    //add 选择发布
    protected void SelectPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();//记录发布者ID
        //if (ManageId != 1 && ManageId != 11)
        //{
        //    Utils.Error("只有系统管理员1号才可以进行此操作", "");
        //}
        Master.Title = manageName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">" + manageName + "</a>&gt;发布内线");
        builder.Append(Out.Tab("</div>", ""));
        int qId = int.Parse(Utils.GetRequest("qId", "all", 1, @"^[^\^]{0,200}$", "1"));
        string info = Utils.GetRequest("info", "all", 1, @"", "");
        string infonext = Utils.GetRequest("infonext", "all", 1, @"", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guestsend.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        //string guestContent = xml.dss["guestContent"].ToString();//内线内容
        //string guestAllGold = xml.dss["guestAllGold"].ToString();//最大红包额度
        //string guestMaxCount = xml.dss["guestMaxCount"].ToString();//最大领取人数
        //string guestHitCount = xml.dss["guestHitCount"].ToString();//最大领取人数
        //发布开始
        if (info == "ok")
        {
            int type = int.Parse(Utils.GetRequest("type", "all", 1, @"", "0"));
            string guestToUsid = (Utils.GetRequest("guestToUsid", "all", 1, @"", "0"));//发送的内线ID
            string guestContent = Utils.GetRequest("guestContent", "all", 1, @"", "0");//内线内容
            string guestTimes = Utils.GetRequest("guestTimes", "all", 1, @"", "0");//发布时间点  
            string guestDays = Utils.GetRequest("guestDays", "all", 1, @"", "0");//天数
            string guestAllGold = Utils.GetRequest("guestAllGold", "all", 1, @"", "0");//总额
            string guestMaxCount = Utils.GetRequest("guestMaxCount", "all", 1, @"", "0");//最大中奖范围
            string guestHitCount = Utils.GetRequest("guestHitCount", "all", 1, @"", "0");   //中奖数
            string guestHbCount = Utils.GetRequest("guestHbCount", "all", 1, @"", "0");   //中奖数
            string strWhere = "EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ";
            string selectsql = " * "; //ID UsName EndTime
            DataSet ds = new BCW.BLL.User().GetList(" * ", strWhere);

            if (type == 0)//在线会员
            {
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
                #region 
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        usid = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                //        usname = Convert.ToString(ds.Tables[0].Rows[i]["UsName"]);
                //        new BCW.BLL.Guest().AddNew(5, usid, usname, guestContent);
                //        uidlist += usid.ToString() + "#";
                //        guessCount++;
                //    }
                //    //Utils.Success("发送成功！", "向在线会员发送问答内线成功，当前已发送内线" + guessCount + "条,3s后返回", Utils.getUrl("guestsend.aspx?act=add&amp;"), "4");
                //    builder.Append("操作完成！");
                //}
                //else
                //{
                //    builder.Append("当前在线人数:" + ds.Tables[0].Rows.Count + "<br/>");
                //}
                #endregion

            }
            else
                if (type == 1)//隐身会员
            {
                //state 1 隐身 0在线
                //筛选
                //Utils.Error("隐身会员过多,暂不支持隐身会员！", "");
                strWhere = " state=1 ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else
                if (type == 2)//所有会员
            {
                Utils.Error("所有会员过多,暂不支持所有会员！", "");
                strWhere = " ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 4) //一周内登录
            {
                strWhere = "EndTime>='" + DateTime.Now.AddDays(-7) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 5) //一个月内登录
            {
                strWhere = "EndTime>='" + DateTime.Now.AddDays(-31) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 6) //二个月内登录
            {
                strWhere = "EndTime>='" + DateTime.Now.AddDays(-60) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 7) //三个月内登录
            {
                strWhere = "EndTime>='" + DateTime.Now.AddDays(-90) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 8) //半年内登录
            {
                strWhere = "EndTime>='" + DateTime.Now.AddDays(-180) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 9) //一年内登录
            {
                strWhere = "EndTime>='" + DateTime.Now.AddDays(-365) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 10) //一年前登录
            {
                strWhere = "EndTime<'" + DateTime.Now.AddDays(-365) + "' ";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }
            else if (type == 11) //未验证
            {
                strWhere = "IsVerify=0";
                ds = new BCW.BLL.User().GetList(selectsql, strWhere);
            }

            #region sql ok
            if (infonext == "ok")
            {
                xml.dss["guestToUsid"] = guestToUsid;
                xml.dss["guestContent"] = guestContent;
                xml.dss["guestToUsid"] = guestToUsid;
                xml.dss["guestTimes"] = guestTimes;
                xml.dss["guestDays"] = guestDays;
                xml.dss["guestAllGold"] = guestAllGold;
                xml.dss["guestMaxCount"] = guestMaxCount;
                xml.dss["guestHitCount"] = guestHitCount;
                xml.dss["guestHbCount"] = guestHbCount;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                //存起数据 控制器
                int usid = 0;
                string usname = "";
                int guessCount = 0;
                string uidlist = "";
                int guestid = 0;
                BCW.Model.tb_GuestSendList list = new BCW.Model.tb_GuestSendList();
                int maxSendID = new BCW.BLL.tb_GuestSend().GetMaxId();
                if (type == 3)//指定会员  "0|在线会员|1|隐身会员|2|所有会员|3|指定会员|4|一周内登录|5|一个月内登录|6|二个月内登录|7|三个月内登录|8|半年内登录|9|一年内登录|10|一年前登录"
                {
                    // Utils.Error(guestToUsid, "");
                    string[] guessid = guestToUsid.Split('#');
                    for (int i = 0; i < guessid.Length; i++)
                    {
                        usid = Convert.ToInt32(guessid[i]);
                        usname = new BCW.BLL.User().GetUsName(Convert.ToInt32(guessid[i]));
                        guestid = new BCW.BLL.Guest().AddNew(5, usid, usname, guestContent);
                        uidlist += usid.ToString() + "#";
                        guessCount++;//已发送内线的id
                        list.addtime = DateTime.Now;
                        list.getGold = 0;
                        list.guestID = guestid;
                        list.guestsendID = maxSendID;
                        list.idDone = 1;
                        list.overtime = DateTime.Now;
                        list.remark = "";
                        list.type = 1;//未阅读
                        list.usid = usid;
                        new BCW.BLL.tb_GuestSendList().Add(list);
                    }
                    // Utils.Error(uidlist, "");
                    //Utils.Success("发送成功！", "向指定会员发送问答内线成功，当前已发送内线" + guessCount + "条,3s后返回", Utils.getUrl("guestsend.aspx?act=add&amp;"), "4");
                    builder.Append("操作完成！");
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            usid = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                            usname = Convert.ToString(ds.Tables[0].Rows[i]["UsName"]);
                            guestid = new BCW.BLL.Guest().AddNew(5, usid, usname, guestContent);
                            uidlist += usid.ToString() + "#";
                            guessCount++;
                            //领奖记录列
                            list.addtime = DateTime.Now;
                            list.getGold = 0;
                            list.guestID = guestid;
                            list.guestsendID = maxSendID;
                            list.idDone = 1;
                            list.overtime = DateTime.Now;
                            list.remark = "";
                            list.type = 1;
                            list.usid = usid;
                            new BCW.BLL.tb_GuestSendList().Add(list);
                        }
                        //Utils.Error(uidlist,"");
                        //Utils.Success("发送成功！", "向在线会员发送问答内线成功，当前已发送内线" + guessCount + "条,3s后返回", Utils.getUrl("guestsend.aspx?act=add&amp;"), "4");
                        builder.Append("操作完成！");
                    }
                    else
                    {
                        builder.Append("当前在线人数:" + ds.Tables[0].Rows.Count + "<br/>");
                    }
                }
                //保存本次发送记录
                //guestsend
                //记录发布者ID ManageId
                BCW.Model.tb_GuestSend model = new BCW.Model.tb_GuestSend();
                model.addtime = DateTime.Now;
                model.allgold = Convert.ToInt32(guestAllGold);
                model.hbCount = Convert.ToInt32(guestHbCount);
                model.hbList = HaobaoSF(Convert.ToInt32(guestHbCount), Convert.ToInt32(guestAllGold));
                model.getCount = Convert.ToInt32(guestHitCount); ;//获得的数量
                model.guestContent = guestContent;
                model.isDone = 1;
                model.manageID = ManageId;
                model.maxCount = Convert.ToInt32(guestMaxCount);
                model.nowgold = Convert.ToInt32(guestAllGold);
                model.overtime = DateTime.Now.AddDays(Convert.ToInt32(guestDays));
                model.remark = "";
                model.seeCount = guessCount;
                model.seeUidList = "";
                model.sendCount = guessCount;
                model.sendDateTime = DateTime.Now;
                model.sendTime = guestTimes;
                model.sendUidList = uidlist;
                model.sentDay = Convert.ToInt32(guestDays);
                model.isSendCount = 1;
                model.sendtype = type;
                model.notSeenIDList = uidlist;
                int guestID = new BCW.BLL.tb_GuestSend().Add(model);
                Utils.Success("发送成功！", "向指定会员发送问答内线成功，当前已发送内线" + guessCount + "条下一次发送时间" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd ") + model.sendTime + ",3s后返回", Utils.getUrl("guestsend.aspx?act=add&amp;"), "4");
            }
            #endregion

            #region 确定发送
            else
            {
                string[] guessid = guestToUsid.Split('#');
                builder.Append(Out.Tab("<div>", "<br/>"));
                int sendCount = guessid.Length;
                if (type != 3)
                {
                    sendCount = ds.Tables[0].Rows.Count;
                }
                builder.Append("发布将向<font color=\"green\">" + type1[type] + "</font>(<font color=\"red\">" + sendCount + "</font>)发送内线.");
                if (sendCount > 5000)
                {
                    builder.Append("<font color=\"red\">注意:你将发送" + sendCount + "条的内线！</font>");
                    if (sendCount > 10000)
                        builder.Append("<font color=\"green\">发送量大于1万,请耐心等待.</font>");
                }
                builder.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"1\" style=\"border:1px #add9c0;align:center;\">");
                builder.Append("<tr><td>");
                builder.Append("发布类型:</td><td><font color=\"green\">" + type1[type] + "(<font color =\"red\">" + sendCount + "</font>)</font></td></tr>");
                builder.Append("<tr><td>内线语句:</td><td><font color=\"green\">" + BCW.User.AdminCall.AdminUBB(Out.SysUBB(guestContent)) + "</font></td></tr>");
                builder.Append("<tr><td>定时时间:</td><td><font color=\"green\">每天" + guestTimes + "点</font></td></tr>");
                builder.Append("<tr><td>发送天数:</td><td><font color=\"green\">连续" + guestDays + "天</font></td></tr>");
                builder.Append("<tr><td>红包总值:</td><td><font color=\"green\">" + guestAllGold + ub.Get("SiteBz") + guestHbCount + "个</font></td></tr>");
                builder.Append("<tr><td>概率范围:</td><td><font color=\"green\">" + guestMaxCount + "个</font></td></tr>");
                builder.Append("<tr><td>范围内领红包数:</td><td><font color=\"green\">" + guestHitCount + "个</font></td></tr>");
                builder.Append("</table>");
                //设置
                builder.Append(Out.Tab("</div>", ""));
                string strText = ":/,:/,:/,:/,:/,:/,:/,:/,:/,";
                string strName = "type,guestToUsid,guestContent,guestTimes,guestDays,guestAllGold,guestHbCount,guestMaxCount,guestHitCount,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,";
                string strValu = type + "'" + guestToUsid + "'" + guestContent + "'" + guestTimes + "'" + guestDays + "'" + guestAllGold + "'" + guestHbCount + "'" + guestMaxCount + "'" + guestHitCount + "'" + Utils.getPage(0) + "";
                string strEmpt = type2 + ",true,true,true,true,true,true,true,false,true,";
                string strIdea = "";
                string strOthe = "确定发送,guestsend.aspx?act=add&amp;info=ok&amp;infonext=ok&amp;qId=" + qId + "&amp;,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?act=add") + "\">返回上级</a><br />");
                builder.Append("温馨提示:当前设置表示每" + guestMaxCount + "将中奖" + guestHitCount + "个");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        #region 填写
        else
        {
            string hour = "";
            for (int i = 0; i < 24; i++)
            {
                hour += i.ToString() + "|" + i.ToString() + "";
                if (i < 23)
                    hour += "|";
            }
            string strWhere = "EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ";
            DataSet ds = new BCW.BLL.User().GetList(" * ", strWhere);
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("温馨提示:当前在线会员<font color=\"red\">" + ds.Tables[0].Rows.Count + "</font>个(不包括隐身会员)");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "选择发布:/,指定ID(#分隔|无则忽略):/,内线内容:/,定时发送时间(每天24小时格式):/,发送天数:/,红包总值:/,红包总个数:/,概率范围:/,范围内领红包数:/,";
            string strName = "type,guestToUsid,guestContent,guestTimes,guestDays,guestAllGold,guestHbCount,guestMaxCount,guestHitCount,backurl";
            string strType = "select,big,big,select,text,text,text,text,text,hidden,";
            string strValu = 0 + "'" + xml.dss["guestToUsid"] + "'" + xml.dss["guestContent"].ToString() + "'" + xml.dss["guestTimes"].ToString() + "'" + xml.dss["guestDays"].ToString() + "'" + xml.dss["guestAllGold"].ToString() + "'" + xml.dss["guestHbCount"] + "'" + xml.dss["guestMaxCount"].ToString() + "'" + xml.dss["guestHitCount"].ToString() + "'" + Utils.getPage(0) + "";
            string strEmpt = type2 + ",true,true," + hour + ",true,true,true,true,false,true,";
            string strIdea = "/";
            string strOthe = "确定,guestsend.aspx?act=add&amp;info=ok&amp;qId=" + qId + "&amp;,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //builder.Append(Out.Tab("<div>", "<br/>"));
            //builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">返回上级</a><br />");
            //builder.Append("温馨提示:当前设置表示" + "概率范围" + "将中奖" + "范围内领红包数" + "个");
            //builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
        builder.Append(Out.Tab("", "<br/>"));

        Bottom();
    }

    //set 设置
    protected void SetPage()
    {
        Master.Title = manageName + "设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getPage("guestsend.aspx") + "\">" + manageName + "</a>&gt;");
        builder.Append("设置");
        builder.Append(Out.Tab("</div>", ""));
        // builder.Append(Out.Tab("<div class=\"text\">", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guestsend.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string guestmanageName = Utils.GetRequest("guestmanageName", "post", 2, @"^[^\^]{1,20}$", "管理名称限1-20字内");
            string guestsendOpen = Utils.GetRequest("guestsendOpen", "post", 3, @"^[^\^]{1,2000}$", "开关选择错误");
            string guestsendflash = Utils.GetRequest("guestsendflash", "post", 3, @"^[^\^]{1,20000}$", "刷新选择错误");
            string guestsendflashTime = Utils.GetRequest("guestsendflashTime", "post", 3, @"^[^\^]{1,2000}$", "刷新时间错误");
            string guestsendMustHitID = Utils.GetRequest("guestsendMustHitID", "post", 3, @"^[^\^]{1,2000}$", "必中ID输入错误");
            string guestsendMustNotHit = Utils.GetRequest("guestsendMustNotHit", "post", 2, @"^[^\^]{1,20}$", "必不中ID输入错误");
            xml.dss["guestmanageName"] = guestmanageName;//管理名称
            xml.dss["guestsendOpen"] = guestsendOpen;//开关
            xml.dss["guestsendflash"] = guestsendflash;//刷新选择 0 1 2
            xml.dss["guestsendflashTime"] = guestsendflashTime;//时间
            xml.dss["guestsendMustHitID"] = guestsendMustHitID;//必中
            xml.dss["guestsendMustNotHit"] = guestsendMustNotHit;//必不中
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("guestsend.aspx?act=set&amp;backurl=" + Utils.getPage(1) + ""), "2");
        }
        else
        {
            string strText = "游戏名称:/,开放选择:/,刷新选择:/,刷新频率(分钟):/,必中红包ID(#分隔):/,必不中红包ID(#分隔):/,";
            string strName = "guestmanageName,guestsendOpen,guestsendflash,guestsendflashTime,guestsendMustHitID,guestsendMustNotHit,backurl";
            string strType = "text,select,select,text,big,big,hidden";
            string strValu = xml.dss["guestmanageName"] + "'" + xml.dss["guestsendOpen"] + "'" + xml.dss["guestsendflash"] + "'" + xml.dss["guestsendflashTime"] + "'" + xml.dss["guestsendMustHitID"] + "'" + xml.dss["guestsendMustNotHit"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,0|关闭|1|启用,0|正常刷新|1|停止刷新|2|手动刷新,true,true,true,";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guestsend.aspx?act=set,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("温馨提示:" + "输入的数字#号间隔即可，下一轮范围内派送额等于每等奖份数总和." + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("guestsend.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //reset 重置
    protected void ResetPage()
    {
        Master.Title = manageName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">游戏</a>&gt;");
        builder.Append(manageName + "管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
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
            builder.Append("确定重置" + manageName + "吗，重置后将删除所有记录！");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_GuestSend");
            new BCW.Data.SqlUp().ClearTable("tb_GuestSendList");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("guestsend.aspx"), "1");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("guestsend.aspx") + "\">" + manageName + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
    }

    public string getStatueType(int type)
    {
        string StrTppe = "";
        switch (type)
        {
            case 0:
                StrTppe = "已领";
                break;
            case 1:
                StrTppe = "未阅读";
                break;
            case 2:
                StrTppe = "已过期";
                break;
            case 4:
                StrTppe = "已阅无红包";
                break;
            default:
                StrTppe = "";
                break;
        }
        return StrTppe;
    }

    //超字符变省略号
    protected string strSub(string str)
    {
        int maxLength = 14;//限制最大字符数,如果str超出这个数字,则显示省略号
        str = str.Length > maxLength ? str.Substring(0, maxLength) + "..." : str;
        return str;
    }

}

