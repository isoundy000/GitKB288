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

public partial class Manage_group : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/group.xml";
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
            case "verify":
                VerifyPage();
                break;
            case "del":
                DelPage();
                break;
            case "chat":
                ChatPage();
                break;
            case "deltext":
                DelTextPage();
                break;
            case "delchat":
                DelChatPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        string keyword = Utils.GetRequest("keyword", "all", 1, @"^[^\^]{1,10}$", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + ub.GetSub("GroupName", xmlPath) + "管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("已开通|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?ptype=0") + "\">已开通</a>|");

        if (ptype == 1)
            builder.Append("审核" + ub.GetSub("GroupName", xmlPath) + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?ptype=1") + "\">审核" + ub.GetSub("GroupName", xmlPath) + "</a>|");

        if (ptype == 2)
            builder.Append("已过期");
        else
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?ptype=2") + "\">已过期</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "ptype", "keyword", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (keyword != "")
        {
            strWhere = "Title LIKE '%" + keyword + "%'";
        }
        else
        {
            if (ptype == 2)
                strWhere = "Status=0 and ExTime<>'1990-1-1' and ExTime<'" + DateTime.Now + "'";
            else
                strWhere = "Status=" + ptype + "";
        }
        //排序条件
        strOrder = "ID Desc";
        // 开始读取列表
        IList<BCW.Model.Group> listGroup = new BCW.BLL.Group().GetGroups(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listGroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Group n in listGroup)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("group.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}(ID:{0})</a>", n.ID, ((pageIndex - 1) * pageSize + k), n.Title);
                if (ptype == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=verify&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核]</a>");
                }
                else {
                    builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=chat&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[聊天]</a>");
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
        string strText = "输入关键字:/,";
        string strName = "keyword,act";
        string strType = "text,hidden";
        string strValu = "'search";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜" + ub.GetSub("GroupName", xmlPath) + ",group.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=add") + "\">添加" + ub.GetSub("GroupName", xmlPath) + "</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ChatPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        Master.Title = "查看" + ub.GetSub("GroupName", xmlPath) + "发言";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看" + ub.GetSub("GroupName", xmlPath) + "发言");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "GroupId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.GroupText> listGroupText = new BCW.BLL.GroupText().GetGroupTexts(pageIndex, pageSize, strWhere, out recordCount);
        if (listGroupText.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.GroupText n in listGroupText)
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
               
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=deltext&amp;id=" + n.GroupId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");
                builder.Append(((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>:" + Out.SysUBB(n.Content) + "(" + DT.FormatDate(n.AddTime, 10) + ")");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=delchat&amp;id=" + id + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx") + "\">返回" + ub.GetSub("GroupName", xmlPath) + "</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelTextPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.GroupText().Exists(bid, id))
        {
            Utils.Error("不存在的发言记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.BLL.GroupText().Delete(bid);
            Utils.Success("删除发言", "删除发言成功..", Utils.getUrl("group.aspx?act=chat&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            Master.Title = "删除发言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此发言?");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=deltext&amp;id=" + id + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=chat&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0)+"") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void DelChatPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Group().Exists(id))
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.BLL.GroupText().Delete("GroupId=" + id + "");
            Utils.Success("清空发言", "清空发言成功..", Utils.getUrl("group.aspx?act=chat&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            Master.Title = "清空发言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空此" + ub.GetSub("GroupName", xmlPath) + "发言?");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=delchat&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=chat&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void AddPage()
    {
        Master.Title = "添加" + ub.GetSub("GroupName", xmlPath) + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加" + ub.GetSub("GroupName", xmlPath) + "");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "" + ub.GetSub("GroupName", xmlPath) + "名称:/," + ub.GetSub("GroupName", xmlPath) + "类型:/,所属城市(如广州):/," + ub.GetSub("GroupName", xmlPath) + "徽章(可空):/," + ub.GetSub("GroupName", xmlPath) + "宣言(可空):/,创建原因(可空):/,创建ID:/,加入" + ub.GetSub("GroupName", xmlPath) + "限制:/,过期时间(永不过期请填写0):/,,";
        string strName = "Title,Types,City,Logo,Notes,Content,UsID,InType,ExTime,act,backurl";
        string strType = "text,select,text,text,text,textarea,snum,select,date,hidden,hidden";
        string strValu = "'1''''''0'" + DT.FormatDate(DateTime.Now, 0) + "'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,1|地区|2|社会|3|打工|4|情感|5|生活|6|娱乐|7|音乐|8|彩票|9|军事|10|时尚|11|数码|12|体育|13|家族|14|文学|15|购物|16|汽车|17|财经|18|动漫|19|校园|20|其它,false,true,true,true,flase,0|不限制|1|需要验证|2|禁止加入,false,false";
        string strIdea = "/";
        string strOthe = "添加" + ub.GetSub("GroupName", xmlPath) + "|reset,group.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "" + ub.GetSub("GroupName", xmlPath) + "名称限30字内");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "类型选择错误"));
        string City = Utils.GetRequest("City", "post", 2, @"^[^\^]{2,8}$", "城市限2-8字");
        string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,100}$", "徽章图片地址限100字内，可留空");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "宣言限50字内，可留空");
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,800}$", "创建原因限800字内，可留空");
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "创建ID填写错误"));
        int InType = int.Parse(Utils.GetRequest("InType", "post", 2, @"^[0-2]$", "加入" + ub.GetSub("GroupName", xmlPath) + "限制选择错误"));
        string sExTime = Utils.GetRequest("ExTime", "post", 1, "", "");
        DateTime ExTime = DateTime.Parse("1990-01-01 00:00:00");
        if (sExTime != "0")
            ExTime = Utils.ParseTime(Utils.GetRequest("ExTime", "post", 2, DT.RegexTime, "过期时间填写错误，永不过期请填写0"));

        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.BLL.Group().ExistsUsID(UsID))
        {
            Utils.Error("ID" + UsID + "已是其它" + ub.GetSub("GroupName", xmlPath) + "的圈主..", "");
        }
        string allCity = "#" + BCW.User.City.GetCity() + "#";
        if (allCity.IndexOf("#" + City + "#") == -1)
        {
            Utils.Error("不存在的城市名称“" + City + "”", "");
        }
        BCW.Model.Group model = new BCW.Model.Group();
        model.Title = Title;
        model.Types = Types;
        model.City = City;
        model.Logo = Logo;
        model.Notes = Notes;
        model.Content = Content;
        model.UsID = UsID;
        model.InType = InType;
        model.Status = 0;
        model.ExTime = ExTime;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Group().Add(model);
        Utils.Success("添加" + ub.GetSub("GroupName", xmlPath) + "", "添加" + ub.GetSub("GroupName", xmlPath) + "成功..", Utils.getUrl("group.aspx"), "1");
    }

    private void EditPage()
    {
        Master.Title = "编辑" + ub.GetSub("GroupName", xmlPath) + "";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑" + ub.GetSub("GroupName", xmlPath) + "");
        builder.Append(Out.Tab("</div>", ""));
        string ExTime = model.ExTime.ToString();
        if (DT.FormatDate(model.ExTime, 0) == "1990-01-01 00:00:00")
        {
            ExTime = "0";
        }

        string strText = "基金数目:" + model.iCent + "/" + ub.GetSub("GroupName", xmlPath) + "名称:/," + ub.GetSub("GroupName", xmlPath) + "类型:/,所属城市(如广州):/," + ub.GetSub("GroupName", xmlPath) + "徽章(可空):/," + ub.GetSub("GroupName", xmlPath) + "宣言(可空):/,创建原因(可空):/,创建ID:/,基金数目:/,加入" + ub.GetSub("GroupName", xmlPath) + "限制:/,关联论坛ID:/,论坛设置:/,非成员发言性质:/,签到得币(从基金扣):/,过期时间(永不过期请填写0):/,创建时间:/,,,";
        string strName = "Title,Types,City,Logo,Notes,Content,UsID,iCent,InType,ForumId,ForumStatus,ChatStatus,SignCent,ExTime,AddTime,id,act,backurl";
        string strType = "text,select,text,text,text,textarea,snum,hidden,select,snum,select,select,snum,date,date,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Types + "'" + model.City + "'" + model.Logo + "'" + model.Notes + "'" + model.Content + "'" + model.UsID + "'" + model.iCent + "'" + model.InType + "'" + model.ForumId + "'" + model.ForumStatus + "'" + model.ChatStatus + "'" + model.SignCent + "'" + ExTime + "'" + DT.FormatDate(model.AddTime, 0) + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,1|地区|2|社会|3|打工|4|情感|5|生活|6|娱乐|7|音乐|8|博彩|9|军事|10|时尚|11|数码|12|体育|13|家族|14|文学|15|购物|16|汽车|17|财经|18|动漫|19|校园|20|其它,false,true,true,true,flase,false,0|不限制|1|需要验证|2|禁止加入,false,0|开放非成员|1|禁止非成员|2|暂停论坛,0|禁非成员|1|可以浏览|2|允许发言,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "编辑" + ub.GetSub("GroupName", xmlPath) + "|reset,group.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=del&amp;id=" + id + "") + "\">删除" + ub.GetSub("GroupName", xmlPath) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "" + ub.GetSub("GroupName", xmlPath) + "名称限30字内");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "类型选择错误"));
        string City = Utils.GetRequest("City", "post", 2, @"^[^\^]{2,8}$", "城市限2-8字");
        string Logo = Utils.GetRequest("Logo", "post", 3, @"^.+?.(gif|jpg|bmp|jpeg|png)$", "请输入正确的徽章图片地址，可留空");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "宣言限50字内，可留空");
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,800}$", "创建原因限800字内，可留空");
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "创建ID填写错误"));
        //int iCent = int.Parse(Utils.GetRequest("iCent", "post", 2, @"^[0-9]\d*$", "基金填写错误"));
        int InType = int.Parse(Utils.GetRequest("InType", "post", 2, @"^[0-2]$", "加入" + ub.GetSub("GroupName", xmlPath) + "限制选择错误"));
        int ForumId = int.Parse(Utils.GetRequest("ForumId", "post", 2, @"^[0-9]\d*$", "关联论坛ID填写错误"));
        int ForumStatus = int.Parse(Utils.GetRequest("ForumStatus", "post", 2, @"^[0-2]$", "论坛设置选择错误"));
        int ChatStatus = int.Parse(Utils.GetRequest("ChatStatus", "post", 2, @"^[0-2]$", "发言性质选择错误"));
        int SignCent = int.Parse(Utils.GetRequest("SignCent", "post", 2, @"^[0-9]\d*$", "签到得币填写错误"));
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        string sExTime = Utils.GetRequest("ExTime", "post", 1, "", "");
        DateTime ExTime = DateTime.Parse("1990-01-01 00:00:00");
        if (sExTime != "0")
            ExTime = Utils.ParseTime(Utils.GetRequest("ExTime", "post", 2, DT.RegexTime, "过期时间填写错误，永不过期请填写0"));


        BCW.Model.Group m = new BCW.BLL.Group().GetGroup(id);
        if (m == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "记录", "");
        }

        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        int uid = new BCW.BLL.Group().GetUsID(id);
        if (UsID != uid && new BCW.BLL.Group().ExistsUsID(UsID))
        {
            Utils.Error("ID" + UsID + "已是其它" + ub.GetSub("GroupName", xmlPath) + "的圈主..", "");
        }
        string allCity = "#" + BCW.User.City.GetCity() + "#";
        if (allCity.IndexOf("#" + City + "#") == -1)
        {
            Utils.Error("不存在的城市名称", "");
        }
        BCW.Model.Group model = new BCW.Model.Group();
        model.ID = id;
        model.Title = Title;
        model.Types = Types;
        model.City = City;
        model.Logo = Logo;
        model.Notes = Notes;
        model.Content = Content;
        model.UsID = UsID;
        model.iCent = m.iCent;
        model.InType = InType;
        model.ForumId = ForumId;
        model.ChatId = 0;
        model.ForumStatus = ForumStatus;
        model.ChatStatus = ChatStatus;
        model.SignCent = SignCent;
        model.ExTime = ExTime;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Group().Update(model);
        Utils.Success("编辑" + ub.GetSub("GroupName", xmlPath) + "", "编辑" + ub.GetSub("GroupName", xmlPath) + "成功..", Utils.getUrl("group.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void VerifyPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "审核" + ub.GetSub("GroupName", xmlPath) + "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定审核此" + ub.GetSub("GroupName", xmlPath) + "吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=verify&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">同意创圈</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok2&amp;act=verify&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">撤销该申请</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("group.aspx?ptype=1") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
            if (model == null)
            {
                Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "记录", "");
            }
            if (model.Status != 1)
            {
                Utils.Error("不存在的审核记录", "");
            }
            if (info == "ok")
            {
                new BCW.BLL.Group().UpdateStatus(id, 0);
                Utils.Success("审核" + ub.GetSub("GroupName", xmlPath) + "", "审核" + ub.GetSub("GroupName", xmlPath) + "成功..", Utils.getPage("group.aspx"), "1");
            }
            else if (info == "ok2")
            {                
                //操作币
                new BCW.BLL.User().UpdateiGold(model.UsID, Convert.ToInt64(ub.GetSub("GroupAddPrice", xmlPath)), "被撤销申请" + ub.GetSub("GroupName", xmlPath) + "返还");
                new BCW.BLL.Guest().Add(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), "系统管理员已撤销您的" + ub.GetSub("GroupName", xmlPath) + "《" + model.Title + "》申请，开圈费已返还到您的帐上,申请创圈失败！");
                new BCW.BLL.Group().Delete(id);
                Utils.Success("撤销" + ub.GetSub("GroupName", xmlPath) + "申请", "撤销" + ub.GetSub("GroupName", xmlPath) + "申请成功，系统已通过内线方式通知申请人并返还了开圈费用..", Utils.getPage("group.aspx"), "2");
            }
           
        }
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除" + ub.GetSub("GroupName", xmlPath) + "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此" + ub.GetSub("GroupName", xmlPath) + "吗.删除同时将会删除此" + ub.GetSub("GroupName", xmlPath) + "的日志和加入的成员记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Group().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Group().Delete(id);
            new BCW.BLL.Grouplog().DeleteStr(id);
            //删除会员的加入记录
            DataSet ds = new BCW.BLL.User().GetList("ID,GroupId", "GroupId LIKE '%#" + id + "#%'");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int uid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    string GroupId = ds.Tables[0].Rows[i]["GroupId"].ToString();
                    new BCW.BLL.User().UpdateGroupId(uid, GroupId.Replace("#" + id + "#", ""));

                }
            }
            Utils.Success("删除" + ub.GetSub("GroupName", xmlPath) + "", "删除" + ub.GetSub("GroupName", xmlPath) + "成功..", Utils.getPage("group.aspx"), "1");
        }
    }
}