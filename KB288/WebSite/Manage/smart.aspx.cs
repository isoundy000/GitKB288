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

public partial class Manage_smart : System.Web.UI.Page
{
    //下次升级可以设定多少天内的数据,已做好准备_0_  标题文字数量
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string[] TextData = { "最新文章", "最热文章", "随机文章" };
    protected string[] PicData = { "最新图片", "最热图片", "随机图片" };
    protected string[] FileData = { "最新文件", "最热文件", "随机文件" };
    protected string[] ShopData = { "最新商品", "最热商品", "随机商品" };
    protected string[] AdvertData = { "最新广告", "最热广告", "随机广告" };
    protected string[] LinkData = { "最新链入", "最多链入", "随机友链", "最多链出", "推荐友链" };
    protected string[] ThreadData = { "最新帖子", "最热帖子", "随机帖子", "最新精华", "最热精华", "随机精华", "最新回复", "最新推荐", "最热推荐", "随机推荐" };
    protected string[] DiaryData = { "最新日记", "最热日记", "随机日记" };
    protected string[] AlbumsData = { "最新相片", "最热相片", "随机相片" };
    protected string[] ActionData = { "全部动态", "会员动态", "游戏动态" };
    protected string[] UserData = { "最新会员", "人气会员", "随机会员", "" + ub.Get("SiteBz") + "排行", "存款排行", "总币排行", "" + ub.Get("SiteBz2") + "排行", "积分排行", "等级排行", "VIP排行", "签到排行", "在线排行" };
    protected string[] ChatData = { "最新聊天" };
    protected string[] SearchData = { "文章资讯", "文件资源", "图片资源", "商品资源", "论坛帖子", "会员记录", "城市会员", "圈子记录", "论坛记录", "聊室记录" };
    protected string[] SpeakData = { "最新闲聊" };

    protected string[] TextRows = { "文字标题", "封面截图", "人气数量", "评论数量", "发表时间" };
    protected string[] PicRows = { "文字标题", "封面截图", "人气数量", "评论数量", "发表时间" };
    protected string[] FileRows = { "文字标题", "封面截图", "人气数量", "评论数量", "发表时间" };
    protected string[] ShopRows = { "文字标题", "封面截图", "人气数量", "评价数量", "发表时间", "商品价格", "已出售数" };
    protected string[] AdvertRows = { "文字标题", "点击数量", "奖励币数" };
    protected string[] LinkRows = { "友链全称", "友链简称", "友链简介", "链入数量", "链出数量", "链入时间", "链出时间" };
    protected string[] ThreadRows = { "文字标题", "人气数量", "评论数量", "发表时间" };
    protected string[] DiaryRows = { "文字标题", "人气数量", "评论数量", "发表时间" };
    protected string[] AlbumsRows = { "文字标题", "人气数量", "相片大图", "相片小图" };
    protected string[] ActionRows = { "作者", "事情", "时间" };
    protected string[] UserRows = { "昵称", "ID号", "勋章", "VIP图", "等级", "" + ub.Get("SiteBz") + "", "存款", "总币", "" + ub.Get("SiteBz2") + "", "积分","签到次数","在线时长" };
    protected string[] ChatRows = { "昵称", "ID号", "发言内容", "发表时间", "聊室名称" };
    protected string[] SpeakRows = { "昵称", "ID号", "闲聊内容", "发表时间", "闲聊名称" };
    protected void Page_Load(object sender, EventArgs e)
    {
        //Smart Call
        Master.Title = "UBB智能调用";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "add2":
                Add2Page();
                break;
            case "add3":
                Add3Page();
                break;
            case "add4":
                Add4Page();
                break;
            case "add5":
                Add5Page();
                break;
            case "add6":
                Add6Page();
                break;
            case "addok":
                AddOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择调用类型");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=addok&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">特殊调用</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">文章栏目</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">图片栏目</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">文件栏目</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">商品栏目</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">广告记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">友链记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">论坛帖子</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=15&amp;backurl=" + Utils.getPage(0) + "") + "\">圈坛帖子</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">日记记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">相片记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=10&amp;backurl=" + Utils.getPage(0) + "") + "\">动态记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=11&amp;backurl=" + Utils.getPage(0) + "") + "\">会员记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=12&amp;backurl=" + Utils.getPage(0) + "") + "\">聊天记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=13&amp;backurl=" + Utils.getPage(0) + "") + "\">搜框调用</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=14&amp;backurl=" + Utils.getPage(0) + "") + "\">闲聊记录</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("class.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddOkPage()
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("特殊UBB调用");
        builder.Append(Out.Tab("</div>", ""));

        string[] sName = BCW.User.AdminCall.CallName;
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "leibie", "nid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

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

                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=2&amp;content=call" + i + "") + "\">" + sName[i].ToString() + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型选择错误"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择调用类型");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string[] Data = { };
        if (ptype == 1)
        {
            Data = TextData;
        }
        else if (ptype == 2)
        {
            Data = PicData;
        }
        else if (ptype == 3)
        {
            Data = FileData;
        }
        else if (ptype == 4)
        {
            Data = ShopData;
        }
        else if (ptype == 5)
        {
            Data = AdvertData;
        }
        else if (ptype == 6)
        {
            Data = LinkData;
        }
        else if (ptype == 7)
        {
            Data = ThreadData;
        }
        else if (ptype == 15)
        {
            Data = ThreadData;
        }
        else if (ptype == 8)
        {
            Data = DiaryData;
        }
        else if (ptype == 9)
        {
            Data = AlbumsData;
        }
        else if (ptype == 10)
        {
            Data = ActionData;
        }
        else if (ptype == 11)
        {
            Data = UserData;
        }
        else if (ptype == 12)
        {
            Data = ChatData;
        }
        else if (ptype == 13)
        {
            Data = SearchData;
        }
        else if (ptype == 14)
        {
            Data = SpeakData;
        }
        for (int i = 0; i < Data.Length; i++)
        {
            builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?act=add2&amp;leibie=" + leibie + "&amp;nid=" + nid + "&amp;ptype=" + ptype + "&amp;p=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + Data[i].ToString() + "</a><br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?leibie=" + leibie + "&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Add2Page()
    {
        string RadioType = "multiple";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型选择错误"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        int p = int.Parse(Utils.GetRequest("p", "get", 2, @"^[0-9]\d*$", "类型选择错误"));
        string strEmptyValue = string.Empty;
        DataSet ds = null;
        if (ptype <= 4)
        {
            //查询条件
            string strWhere = string.Empty;
            if (ptype == 1)
                strWhere = "Types=11";
            else if (ptype == 2)
                strWhere = "Types=12";
            else if (ptype == 3)
                strWhere = "Types=13";
            else if (ptype == 4)
                strWhere = "Types=14";

            ds = new BCW.BLL.Topics().GetList("ID,Title", strWhere);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strEmptyValue += "|" + ds.Tables[0].Rows[i]["ID"].ToString() + "|" + ds.Tables[0].Rows[i]["Title"].ToString() + "";

                }
                strEmptyValue = "0|全部栏目" + strEmptyValue;
            }
            else
            {
                strEmptyValue = "0|全部栏目";
            }
        }
        else if (ptype == 5)
        {
            strEmptyValue = "0|全部栏目";
        }
        else if (ptype == 6)
        {
            strEmptyValue = "0|全部分类|" + ub.GetSub("LinkLeibie", "/Controls/link.xml") + "";
        }
        else if (ptype == 7 || ptype == 15)//论坛
        {
            if (ptype == 7)
                ds = new BCW.BLL.Forum().GetList("ID,Title", "IsActive=0");
            else
                ds = new BCW.BLL.Forum().GetList("ID,Title", "IsActive=0 and GroupId>0");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strEmptyValue += "|" + ds.Tables[0].Rows[i]["ID"].ToString() + "|" + ds.Tables[0].Rows[i]["Title"].ToString() + "";

                }
                strEmptyValue = "0|全部论坛" + strEmptyValue;
            }
            else
            {
                strEmptyValue = "0|全部论坛";
            }
        }
        else if (ptype == 8 || ptype == 9)
        {
            strEmptyValue = "0|全部分类";
        }
        else if (ptype == 10)
        {
            if (p != 2)
            {
                strEmptyValue = "0|全部记录";
            }
            else
            {
                RadioType = "select";
                strEmptyValue = "0|全部游戏|1|剪刀石头|2|ktv789|3|猜猜乐|4|竞拍|6|幸运二八|8|疯狂彩球|9|挖宝竞猜|10|跑马游戏|11|上证竞猜|12|社区商城";
                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
                {
                    strEmptyValue += "|13|大小庄";
                    strEmptyValue += "|14|疯狂吹牛";
                }
                strEmptyValue += "|15|结婚系统";
                if (Utils.GetDomain().Contains("168yy") || Utils.GetDomain().Contains("tl88"))
                {
                    strEmptyValue += "|16|猜拳游戏";
                }
            }

        }
        else if (ptype == 11)
        {
            strEmptyValue = "0|全部记录";
        }
        else if (ptype == 12)
        {
            ds = new BCW.BLL.Chat().GetList("ID,ChatName", "Types=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strEmptyValue += "|" + ds.Tables[0].Rows[i]["ID"].ToString() + "|" + ds.Tables[0].Rows[i]["ChatName"].ToString() + "";

                }
                strEmptyValue = "0|全部聊室" + strEmptyValue;
            }
            else
            {
                strEmptyValue = "0|全部聊室";
            }
        
        }
        else if (ptype == 13)
        {
            if (p < 4)
            {
                //查询条件
                string strWhere = string.Empty;
                if (p == 0)
                    strWhere = "Types=11";
                else if (p == 1)
                    strWhere = "Types=13";
                else if (p == 2)
                    strWhere = "Types=12";
                else if (p == 3)
                    strWhere = "Types=14";

                ds = new BCW.BLL.Topics().GetList("ID,Title", strWhere);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strEmptyValue += "|" + ds.Tables[0].Rows[i]["ID"].ToString() + "|" + ds.Tables[0].Rows[i]["Title"].ToString().Replace("|", "") + "";

                    }
                    strEmptyValue = "0|全部栏目" + strEmptyValue;
                }
                else
                {
                    strEmptyValue = "0|全部栏目";
                }
            }
            else
            {
                strEmptyValue = "0|全部记录";
            }
        }
        else if (ptype == 14)
        {
            strEmptyValue = "0|全部记录";
        }
        builder.Append(Out.Tab("<div class=\"title\">选择调用栏目</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("选择调用栏目:");
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,,,,,,";
        string strName = "NodeId,ptype,p,leibie,nid,act,backurl";
        string strType = "" + RadioType + ",hidden,hidden,hidden,hidden,hidden,hidden";
        string strValu="";
        if (ptype != 13)
            strValu = "0'" + ptype + "'" + p + "'" + leibie + "'" + nid + "'add3'" + Utils.getPage(0) + "";
        else
            strValu = "0'" + ptype + "'" + p + "'" + leibie + "'" + nid + "'add6'" + Utils.getPage(0) + "";

        string strEmpt = "" + strEmptyValue + ",false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "下一步,smart.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void Add3Page()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-9]\d*$", "类型选择错误"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        int p = int.Parse(Utils.GetRequest("p", "post", 2, @"^[0-9]\d*$", "类型选择错误"));
        string NodeId = Utils.GetRequest("NodeId", "post", 2, @"^[\w((;|,)\w)?]+$", "选择调用栏目错误，不能全不选");
        NodeId = NodeId.Replace(",", ";");

        builder.Append(Out.Tab("<div class=\"title\">选择显示选项</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("选择调用选项:");
        builder.Append(Out.Tab("</div>", ""));

        string[] strRows = { };

        if (ptype == 1)
            strRows = TextRows;
        else if (ptype == 2)
            strRows = PicRows;
        else if (ptype == 3)
            strRows = FileRows;
        else if (ptype == 4)
            strRows = ShopRows;
        else if (ptype == 5)
            strRows = AdvertRows;
        else if (ptype == 6)
            strRows = LinkRows;
        else if (ptype == 7)
            strRows = ThreadRows;
        else if (ptype == 15)
            strRows = ThreadRows;
        else if (ptype == 8)
            strRows = DiaryRows;
        else if (ptype == 9)
            strRows = AlbumsRows;
        else if (ptype == 10)
            strRows = ActionRows;
        else if (ptype == 11)
            strRows = UserRows;
        else if (ptype == 12)
            strRows = ChatRows;
        else if (ptype == 14)
            strRows = SpeakRows;

        string strEmptyValue = string.Empty;
        for (int i = 0; i < strRows.Length; i++)
        {
            strEmptyValue += "|" + i + "|" + strRows[i].ToString();
        }
        strEmptyValue = Utils.Mid(strEmptyValue, 1, strEmptyValue.Length);
        string strText = ",调用条数:/,,,,,,,,";
        string strName = "NodeRows,Num,ptype,p,NodeId,leibie,nid,act,backurl";
        string strType = "multiple,num,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
        string strValu = "0''" + ptype + "'" + p + "'" + NodeId + "'" + leibie + "'" + nid + "'add4'" + Utils.getPage(0) + "";
        string strEmpt = "" + strEmptyValue + ",false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "下一步,smart.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void Add4Page()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-9]\d*$", "类型选择错误"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        int p = int.Parse(Utils.GetRequest("p", "post", 2, @"^[0-9]\d*$", "类型选择错误"));
        string NodeId = Utils.GetRequest("NodeId", "post", 2, @"^[\w((;|,)\w)?]+$", "选择调用栏目错误，不能全不选");
        string NodeRows = Utils.GetRequest("NodeRows", "post", 2, @"^[\w((;|,)\w)?]+$", "选择调用选项错误，不能全不选");
        int Num = int.Parse(Utils.GetRequest("Num", "post", 2, @"^[1-9]\d*$", "调用条数填写错误"));
        NodeId = NodeId.Replace(",", ";");
        NodeRows = NodeRows.Replace(",", ";");

        builder.Append(Out.Tab("<div class=\"title\">生成智能UBB</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("以下是得到的模型效果，您可以调整位置(需要换行请使用“[br]”)再生成智能UBB.");
        builder.Append(Out.Tab("</div>", ""));

        string ubbContent = string.Empty;
        string[] strRows = { };

        if (ptype == 1)
            strRows = TextRows;
        else if (ptype == 2)
            strRows = PicRows;
        else if (ptype == 3)
            strRows = FileRows;
        else if (ptype == 4)
            strRows = ShopRows;
        else if (ptype == 5)
            strRows = AdvertRows;
        else if (ptype == 6)
            strRows = LinkRows;
        else if (ptype == 7)
            strRows = ThreadRows;
        else if (ptype == 15)
            strRows = ThreadRows;
        else if (ptype == 8)
            strRows = DiaryRows;
        else if (ptype == 9)
            strRows = AlbumsRows;
        else if (ptype == 10)
            strRows = ActionRows;
        else if (ptype == 11)
            strRows = UserRows;
        else if (ptype == 12)
            strRows = ChatRows;
        else if (ptype == 14)
            strRows = SpeakRows;

        string[] strNodeRows = NodeRows.Split(";".ToCharArray());
        string strEmptyValue = string.Empty;
        for (int i = 0; i < strNodeRows.Length; i++)
        {
            strEmptyValue += "|" + strRows[int.Parse(strNodeRows[i])].ToString();
        }
        strEmptyValue = Utils.Mid(strEmptyValue, 1, strEmptyValue.Length);

        string BT = string.Empty;
        if (strEmptyValue.Contains("文字标题"))
            BT = "{文字标题}";

        string PIC = string.Empty;
        if (strEmptyValue.Contains("封面截图"))
            PIC = "{封面截图}";

        string RQ = string.Empty;
        if (strEmptyValue.Contains("人气数量"))
            RQ = "{人气数量}";

        string PL = string.Empty;
        if (strEmptyValue.Contains("评论数量"))
            PL = "{评论数量}";
        else if (strEmptyValue.Contains("评价数量"))
            PL = "{评价数量}";
        else if (strEmptyValue.Contains("点击数量"))
            PL = "{点击数量}";

        string SJ = string.Empty;
        if (strEmptyValue.Contains("发表时间"))
            SJ = "{发表时间}";

        string JG = string.Empty;
        if (strEmptyValue.Contains("商品价格"))
            JG = "{商品价格}";

        string SELL = string.Empty;
        if (strEmptyValue.Contains("已出售数"))
            SELL = "{已出售数}";

        string GOLD = string.Empty;
        if (strEmptyValue.Contains("奖励币数"))
            GOLD = "{奖励币数}";

        //友链部分
        string LINK = string.Empty;
        if (strEmptyValue.Contains("友链全称"))
            LINK += "{友链全称}";

        if (strEmptyValue.Contains("友链简称"))
            LINK += "{友链简称}";

        if (strEmptyValue.Contains("友链简介"))
            LINK += "{友链简介}";

        if (strEmptyValue.Contains("链入数量"))
            LINK += "{链入数量}";

        if (strEmptyValue.Contains("链出数量"))
            LINK += "{链出数量}";

        if (strEmptyValue.Contains("链入时间"))
            LINK += "{链入时间}";

        if (strEmptyValue.Contains("链出时间"))
            LINK += "{链出时间}";

        //相册部分
        string Albums = string.Empty;
        if (strEmptyValue.Contains("相片大图"))
            Albums += "{相片大图}";
        if (strEmptyValue.Contains("相片小图"))
            Albums += "{相片小图}";

        //动态部分
        string Action1 = string.Empty;
        if (strEmptyValue.Contains("作者"))
            Action1 += "{作者}";
        string Action2 = string.Empty;
        if (strEmptyValue.Contains("事情"))
            Action2 += "{事情}";
        string Action3 = string.Empty;
        if (strEmptyValue.Contains("时间"))
            Action3 += "{时间}";

        //会员部分
        string USER = string.Empty;
        if (strEmptyValue.Contains("昵称"))
            USER += "{昵称}";
        if (strEmptyValue.Contains("ID号"))
            USER += "{ID号}";
        if (strEmptyValue.Contains("勋章"))
            USER += "{勋章}";
        if (strEmptyValue.Contains("VIP图"))
            USER += "{VIP图}";
        if (strEmptyValue.Contains("等级"))
            USER += "{等级}";
        if (strEmptyValue.Contains("" + ub.Get("SiteBz") + ""))
            USER += "{" + ub.Get("SiteBz") + "}";
        if (strEmptyValue.Contains("存款"))
            USER += "{存款}";
        if (strEmptyValue.Contains("总币"))
            USER += "{总币}";
        if (strEmptyValue.Contains("" + ub.Get("SiteBz2") + ""))
            USER += "{" + ub.Get("SiteBz2") + "}";
        if (strEmptyValue.Contains("积分"))
            USER += "{积分}";
        if (strEmptyValue.Contains("签到次数"))
            USER += "{签到次数}";
        if (strEmptyValue.Contains("在线时长"))
            USER += "{在线时长}";

        //聊室部分
        string CHATUSER = string.Empty;
        if (strEmptyValue.Contains("昵称"))
            CHATUSER += "{昵称}";
        string CHATID = string.Empty;
        if (strEmptyValue.Contains("ID号"))
            CHATID += "{ID号}";
        string CHATCONTENT = string.Empty;
        if (strEmptyValue.Contains("发言内容"))
            CHATCONTENT += "{发言内容}";
        string CHATNAME = string.Empty;
        if (strEmptyValue.Contains("聊室名称"))
            CHATNAME += "在{聊室名称}";
        //闲聊部分
        string SPEAKUSER = string.Empty;
        if (strEmptyValue.Contains("昵称"))
            SPEAKUSER += "{昵称}";
        string SPEAKID = string.Empty;
        if (strEmptyValue.Contains("ID号"))
            SPEAKID += "{ID号}";
        string SPEAKCONTENT = string.Empty;
        if (strEmptyValue.Contains("闲聊内容"))
            SPEAKCONTENT += "{闲聊内容}";
        string SPEAKNAME = string.Empty;
        if (strEmptyValue.Contains("闲聊名称"))
            SPEAKNAME += "在{闲聊名称}";

        string strUbb = string.Empty;
        if (ptype <= 5 || ptype == 7 || ptype == 15 || ptype == 8 || ptype == 9)
        {
            if (string.IsNullOrEmpty(BT))
                strUbb = "" + BT + "" + RQ + "" + PL + "" + SJ + "" + JG + "" + SELL + "" + GOLD + "" + PIC + "" + LINK + "" + Albums + "[br]";
            else
                strUbb = "[URL=default.aspx?id=内容ID]" + BT + "[/URL]" + RQ + "" + PL + "" + SJ + "" + JG + "" + SELL + "" + GOLD + "" + PIC + "" + LINK + "" + Albums + "[br]";
        }
        else if (ptype == 6)
        {
            if (!strEmptyValue.Contains("友链全称") && !strEmptyValue.Contains("友链简称"))
            {
                strUbb = "" + LINK + "[br]";
            }
            else
            {
                strUbb = "[URL=default.aspx?id=内容ID]" + LINK + "[/URL][br]";
            }
        }
        else if (ptype == 10)
        {
            strUbb = "" + Action3 + "[URL=/bbs/uinfo.aspx?uid=作者ID]" + Action1 + "[/URL]" + Action2 + "[br]";
        }
        else if (ptype == 11)
        {
            strUbb = "[URL=/bbs/uinfo.aspx?uid=会员ID]" + USER + "[/URL][br]";
        }
        else if (ptype == 12)
        {
            strUbb = "[URL=/bbs/uinfo.aspx?uid=会员ID]" + CHATUSER + "" + CHATID + "[/URL]" + CHATNAME + "" + CHATCONTENT + "［" + SJ + "］[br]";
        }
        else if (ptype == 13)
        {
            strUbb = "[input=10]请输入搜索关键字[/input][br][ancho=地址_post][pd=变量名]变量值[/pd]搜索[/ancho]";
        }
        else if (ptype == 14)
        {
            strUbb = "[URL=/bbs/uinfo.aspx?uid=会员ID]" + SPEAKUSER + "" + SPEAKID + "[/URL]" + SPEAKNAME + "" + SPEAKCONTENT + "［" + SJ + "］[br]";
        }
        string strText = ",,,,,,,,";
        string strName = "strUbb,ptype,p,Num,NodeId,leibie,nid,act,backurl";
        string strType = "big,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + strUbb + "'" + ptype + "'" + p + "'" + Num + "'" + NodeId + "'" + leibie + "'" + nid + "'add5'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "生成智能UBB,smart.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Add5Page()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-9]\d*$", "类型选择错误"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        int p = int.Parse(Utils.GetRequest("p", "post", 2, @"^[0-9]\d*$", "类型选择错误"));
        string NodeId = Utils.GetRequest("NodeId", "post", 2, @"^[\w((;|,)\w)?]+$", "选择调用栏目错误，不能全不选");
        string strUbb = Utils.GetRequest("strUbb", "post", 2, @"^[\s\S]{1,500}$", "调用参数错误");
        int Num = int.Parse(Utils.GetRequest("Num", "post", 2, @"^[1-9]\d*$", "调用条数填写错误"));
        NodeId = NodeId.Replace(",", ";");
        NodeId = NodeId.Replace(";", "_");

        builder.Append(Out.Tab("<div class=\"title\">生成智能UBB</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("生成智能UBB结果:");
        builder.Append(Out.Tab("</div>", ""));

        strUbb = strUbb.Replace("{文字标题}", "{BT}");
        strUbb = strUbb.Replace("{人气数量}", "{RQ}");
        strUbb = strUbb.Replace("{评论数量}", "{PL}");
        strUbb = strUbb.Replace("{发表时间}", "{SJ}");
        strUbb = strUbb.Replace("{封面截图}", "{PIC}");
        strUbb = strUbb.Replace("{评价数量}", "{PL}");
        strUbb = strUbb.Replace("{商品价格}", "{JG}");
        strUbb = strUbb.Replace("{已出售数}", "{SELL}");
        strUbb = strUbb.Replace("{点击数量}", "{RQ}");
        strUbb = strUbb.Replace("{奖励币数}", "{GOLD}");
        //友链部分
        strUbb = strUbb.Replace("{友链全称}", "{LN}");
        strUbb = strUbb.Replace("{友链简称}", "{LT}");
        strUbb = strUbb.Replace("{友链简介}", "{LJ}");
        strUbb = strUbb.Replace("{链入数量}", "{LR}");
        strUbb = strUbb.Replace("{链出数量}", "{LC}");
        strUbb = strUbb.Replace("{链入时间}", "{RT}");
        strUbb = strUbb.Replace("{链出时间}", "{CT}");
        //相册部分
        strUbb = strUbb.Replace("{相片大图}", "{BG}");
        strUbb = strUbb.Replace("{相片小图}", "{SL}");
        //动态部分
        strUbb = strUbb.Replace("{作者}", "{ZZ}");
        strUbb = strUbb.Replace("{事情}", "{SQ}");
        strUbb = strUbb.Replace("{时间}", "{SJ}");
        //会员部分
        strUbb = strUbb.Replace("{昵称}", "{NC}");
        strUbb = strUbb.Replace("{ID号}", "({ID})");
        strUbb = strUbb.Replace("{勋章}", "{XZ}");
        strUbb = strUbb.Replace("{VIP图}", "{VIP}");
        strUbb = strUbb.Replace("{等级}", "{DJ}");
        strUbb = strUbb.Replace("{" + ub.Get("SiteBz") + "}", "{BB}");
        strUbb = strUbb.Replace("{存款}", "{BK}");
        strUbb = strUbb.Replace("{总币}", "{BS}");
        strUbb = strUbb.Replace("{" + ub.Get("SiteBz2") + "}", "{YY}");
        strUbb = strUbb.Replace("{积分}", "{JF}");
        strUbb = strUbb.Replace("{签到次数}", "{SN}");
        strUbb = strUbb.Replace("{在线时长}", "{SC}");
        //聊天部分
        strUbb = strUbb.Replace("{发言内容}", "{FY}");
        strUbb = strUbb.Replace("{聊室名称}", "{LS}");

        strUbb = strUbb.Replace("{闲聊内容}", "{XY}");
        strUbb = strUbb.Replace("{闲聊名称}", "{XL}");
        strUbb = strUbb.Replace("[br]", "{BR}");
        strUbb = Regex.Replace(strUbb, @"(\[URL=(.[^\]]*)\])(.[^\[]*)(\[\/URL\])", @"a_$3_a", RegexOptions.IgnoreCase);
        string ubbContent = "[CODE=" + ptype + "_" + p + "_" + Num + "_0_" + strUbb + "#" + NodeId + "]";

        string strText = ",,,,";
        string strName = "content,leibie,nid,ptype";
        string strType = "big,hidden,hidden,hidden";
        string strValu = "" + ubbContent + "'" + leibie + "'" + nid + "'2";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "使用智能UBB,classact.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Add6Page()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-9]\d*$", "类型选择错误"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-9]\d*$", "0"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        int p = int.Parse(Utils.GetRequest("p", "post", 2, @"^[0-9]\d*$", "类型选择错误"));
        string NodeId = Utils.GetRequest("NodeId", "post", 2, @"^[\w((;|,)\w)?]+$", "选择调用栏目错误，不能全不选");
        NodeId = NodeId.Replace(",", ";");

        builder.Append(Out.Tab("<div class=\"title\">生成智能UBB</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("以下是得到的模型效果，您可以调整位置(需要换行请使用“[br]”)再生成智能UBB.");
        builder.Append(Out.Tab("</div>", ""));

        string Node = "[hidden=pt]" + p + "_" + NodeId + "[/hidden]";
        if (Node.Contains("_0;"))
            Node = "[hidden=pt]" + p + "_0[/hidden]";

        string strUbb = "[form=/search.aspx_post][input=keyword_15]请输入搜索关键字[/input][br]#" + Node + "[postfield=keyword]￥(keyword)[/postfield]#搜索#red[/form]";

        string strText = ",,,,";
        string strName = "content,leibie,nid,ptype";
        string strType = "big,hidden,hidden,hidden";
        string strValu = "" + strUbb.Replace(char.ConvertFromUtf32(34), "") + "'" + leibie + "'" + nid + "'2";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "使用智能UBB,classact.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("smart.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}