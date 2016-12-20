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

public partial class Manage_moderator : System.Web.UI.Page
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
            case "add2":
                Add2Page();
                break;
            case "save":
                SavePage();
                break;
            case "save2":
                Save2Page();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "rolece":
                RolecePage();
                break;
            case "honor":
                HonorPage();
                break;
            case "delhonor":
                DelHonorPage();
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
        Master.Title = "版主管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("版主管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("正在上任|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?ptype=1") + "\">上任</a>|");

        if (ptype == 2)
            builder.Append("历任版主|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?ptype=2") + "\">历任</a>|");

        if (ptype == 3)
            builder.Append("暂停版主");
        else
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?ptype=3") + "\">暂停</a>");

        //if (ptype == 4)
        //    builder.Append("荣誉版主");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?ptype=4") + "\">荣誉</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 200; Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1)
            strWhere = "(OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";
        else if (ptype == 2)
            strWhere = "OverTime<'" + DateTime.Now + "' and OverTime<>'1990-1-1 00:00:00' and Status<>1";
        else if (ptype == 3)
            strWhere = "Status=1";
        //else if (ptype == 4)
        //    strWhere = "Status=2";

        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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

                string sInclude = string.Empty;
                if (n.Include == 1)
                    sInclude = "(含下级版块)";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("moderator.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}/{2}</a>", n.UsID, n.UsName, n.RoleName);

                if (n.ForumID == 0)
                    builder.Append("/管辖:全区版块");
                else if (n.ForumID > 0)
                    builder.Append("/管辖:<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + n.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ForumName + "" + sInclude + "</a>");

                if (ptype == 1)
                {
                    int gNum = Utils.GetStringNum(n.Rolece, ";");
                    builder.Append("/<a href=\"" + Utils.getUrl("moderator.aspx?act=rolece&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + (gNum+1) + "/40</a>");
                    builder.Append("上任:" + DT.DateDiff2(DateTime.Now, n.StartTime) + "");

                    //任职记录
                    if (n.ForumID > 0)
                    {
                        new BCW.BLL.Forumlog().Add(14, n.ForumID, "[url=/bbs/uinfo.aspx?uid=" + n.UsID + "]" + n.UsName + "(" + n.UsID + ")[/url]上任时间：" + DT.FormatDate(n.StartTime, 11) + "");
                    }

                }
                //else if (ptype == 2)
                //{
                //    if (n.Status == 2)
                //        builder.Append("[已是荣誉版主]");
                //    else
                //        builder.Append("[<a href=\"" + Utils.getUrl("moderator.aspx?act=honor&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">设为荣誉版主</a>]");
                
                //}
                //else if (ptype == 4)
                //{
                //    builder.Append("[<a href=\"" + Utils.getUrl("moderator.aspx?act=delhonor&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">取消荣誉版主</a>]");

                //}

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
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=add") + "\">添加版主</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=add2") + "\">添加管理员</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void AddPage()
    {
        Master.Title = "添加版主";
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "-1"));
        int uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[0-9]\d*$", "0"));
        if (id == -1)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请选择论坛版块");
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            string strWhere = string.Empty;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "uid" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "";

            // 开始读取论坛
            IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
            if (listForum.Count > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.AppendFormat("<a href=\"" + Utils.getUrl("moderator.aspx?act=add&amp;id=0&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">版块总版主</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                int k = 1;
                foreach (BCW.Model.Forum n in listForum)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div class=\"text\">", ""));
                        else
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    }
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("moderator.aspx?act=add&amp;id={0}&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}(ID{0})</a>", n.ID, n.Title);
                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("text", "没有相关记录"));
            }
        }
        else
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (id == 0)
            {
                builder.Append("添加总版主");
            }
            else
            {
                string ForumName = new BCW.BLL.Forum().GetTitle(id);
                if (ForumName == "")
                    Utils.Error("不存在的版块ID", "");

                builder.Append("添加" + ForumName + "版主");
            }
            builder.Append(Out.Tab("</div>", ""));

            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            string RoleString = BCW.User.Role.GetRoleString();
            if (id > 0)
            {
                sText = "权限包括下级版:/,";
                sName = "Include,";
                sType = "select,";
                sValu = "0'";
                sEmpt = "0|不含|1|包含,";
                RoleString = RoleString.Replace("|z|设置版主", "");
            }

            string strText = "输入用户ID:/,选择权限，可多选/,期限(天|填0则无限期):/,职称(如:正版|副版|见习版):/," + sText + ",,";
            string strName = "UsID,Role,rDay,rName," + sName + "id,act,backurl";
            string strType = "num,multiple,num,text," + sType + "hidden,hidden,hidden";
            string strValu = "" + uid + "''0''" + sValu + "" + id + "'save'" + Utils.getPage(0) + "";
            string strEmpt = "false," + RoleString + ",false,false," + sEmpt + "false,false,false";
            string strIdea = "/";
            string strOthe = "添加版主|reset,moderator.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />");
            builder.Append("锁定会员将使会员失去所有的权限.<br />");
            builder.Append("总版权限将有所有论坛的管理区域.<br />");
            builder.Append("设定版主功能需有该版块权限才有此权限");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("moderator.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Add2Page()
    {
        Master.Title = "添加管理员";
        int uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加管理员");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入用户ID:/,选择全局权限，可多选/,选择总版权限，可不选或多选/,期限(天|填0则无限期):/,职称(如:系管|社管|巡警):/,,";
        string strName = "UsID,Roleall,Role,rDay,rName,act,backurl";
        string strType = "num,multiple,multiple,num,text,hidden,hidden";
        string strValu = "" + uid + "'''0''save2'" + Utils.getPage(0) + "";
        string strEmpt = "false," + BCW.User.Limits.GetLimitString() + "," + BCW.User.Role.GetRoleString() + ",false,false,false,false";
        string strIdea = "/";
        string strOthe = "添加管理员|reset,moderator.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />");
        builder.Append("锁定会员将使会员失去所有的权限.<br />");
        builder.Append("总版权限将有所有论坛的管理区域.<br />");
        builder.Append("设定版主功能需有该版块权限才有此权限");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("moderator.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Save2Page()
    {
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string Roleall = Utils.GetRequest("Roleall", "post", 2, @"^[\w((;|,)\w)?]+$", "选择全局权限错误");
        Roleall = Roleall.Replace(",", ";");
        string Role = Utils.GetRequest("Role", "post", 3, @"^[\w((;|,)\w)?]+$", "选择权限错误");
        Role = Role.Replace(",", ";");
        string GetRole = Roleall;
        if (Role != "")
            GetRole = Roleall + ";" + Role;

        int rDay = int.Parse(Utils.GetRequest("rDay", "post", 2, @"^[0-9]\d*$", "期限填写错误"));
        string rName = Utils.GetRequest("rName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "职称限10字内，不能使用特别字符");
        int Include = int.Parse(Utils.GetRequest("Include", "post", 1, @"^[0-1]$", "0"));
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的会员ID", "");
        }

        if (new BCW.BLL.Role().Exists(UsID, -1))
        {
            Utils.Error("ID:" + UsID + "已经是管理员", "");
        }
        if (new BCW.BLL.Role().Exists(UsID, 0))
        {
            Utils.Error("ID:" + UsID + "已经是总版，管理员与总版不能同时兼职", "");
        }

        string UsName = new BCW.BLL.User().GetUsName(UsID);
        BCW.Model.Role model = new BCW.Model.Role();
        model.UsID = UsID;
        model.UsName = UsName;
        model.ForumID = -1;
        model.ForumName = "";
        model.Rolece = GetRole;
        model.RoleName = rName;
        model.StartTime = DateTime.Now;
        if (rDay == 0)
            model.OverTime = DateTime.Parse("1990-1-1 00:00:00");
        else
            model.OverTime = DateTime.Now.AddDays(rDay);

        model.Status = 0;
        new BCW.BLL.Role().Add(model);

        //发送内线给版主
        new BCW.BLL.Guest().Add(UsID, UsName, "系统管理员将您设为管理员");

        Utils.Success("添加管理员", "恭喜，添加管理员成功，正在返回..", Utils.getPage("moderator.aspx"), "1");

    }

    private void SavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string Role = Utils.GetRequest("Role", "post", 2, @"^[\w((;|,)\w)?]+$", "选择权限错误");
        Role = Role.Replace(",", ";");
        int rDay = int.Parse(Utils.GetRequest("rDay", "post", 2, @"^[0-9]\d*$", "期限填写错误"));
        string rName = Utils.GetRequest("rName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "职称限10字内，不能使用特别字符");
        int Include = int.Parse(Utils.GetRequest("Include", "post", 1, @"^[0-1]$", "0"));
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        string ForumName = string.Empty;
        if (id > 0)
        {
            ForumName = new BCW.BLL.Forum().GetTitle(id);
            if (ForumName == "")
            {
                Utils.Error("不存在的版块ID", "");
            }
            if (new BCW.BLL.Role().Exists(UsID, id))
            {
                Utils.Error("ID:" + UsID + "已经是" + ForumName + "的版主", "");
            }
        }
        else
        {
            if (new BCW.BLL.Role().Exists(UsID, 0))
            {
                Utils.Error("ID:" + UsID + "已经是总版主", "");
            }
            if (new BCW.BLL.Role().Exists(UsID, -1))
            {
                Utils.Error("ID:" + UsID + "已经是管理员，管理员与总版不能同时兼职", "");
            }
        }

        string UsName = new BCW.BLL.User().GetUsName(UsID);
        BCW.Model.Role model = new BCW.Model.Role();
        model.UsID = UsID;
        model.UsName = UsName;
        model.ForumID = id;
        model.ForumName = ForumName;
        model.Rolece = Role;
        model.RoleName = rName;
        model.StartTime = DateTime.Now;
        if (rDay == 0)
            model.OverTime = DateTime.Parse("1990-1-1 00:00:00");
        else
            model.OverTime = DateTime.Now.AddDays(rDay);

        model.Include = Include;
        model.Status = 0;
        new BCW.BLL.Role().Add(model);

        //发送内线给版主
        if (id > 0)
        {
            new BCW.BLL.Guest().Add(UsID, UsName, "系统管理员将您设为论坛“" + ForumName + "”的版主");
            //任职记录
            new BCW.BLL.Forumlog().Add(14, id, "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + UsName + "(" + UsID + ")[/url]上任时间：" + DT.FormatDate(DateTime.Now, 11) + "");
        
        }
        Utils.Success("添加版主", "恭喜，添加版主成功，正在返回..", Utils.getPage("moderator.aspx"), "1");

    }

    private void RolecePage()
    {
        Master.Title = "查看详细权限";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Role().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.Model.Role model = new BCW.BLL.Role().GetRole(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + model.UsName + "权限列表:");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));
        int gNum = Utils.GetStringNum(model.Rolece, ";");
        builder.Append(Out.Tab("<div>", ""));

        string sInclude = string.Empty;
        if (model.Include == 1)
            sInclude = "(含下级版块)";

        if (model.ForumID == -1)
            builder.Append("管辖:全站");
        else if (model.ForumID == 0)
            builder.Append("管辖:全区版块");
        else
            builder.Append("管辖:<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + model.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ForumName + "</a>" + sInclude + "");


        builder.Append("<br />拥有权限(" + (gNum+1) + "/40)");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + BCW.User.Role.OutRoleString(model.Rolece).Replace(" ", "<br />") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("moderator.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        Master.Title = "编辑版主";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Role().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Role model = new BCW.BLL.Role().GetRole(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (model.ForumID == -1)
        {
            builder.Append("编辑管理员权限");
        }
        else if (model.ForumID == 0)
        {
            builder.Append("编辑总版主权限");
        }
        else
        {
            builder.Append("编辑版主权限");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("用户:<a href=\"" + Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(ID" + model.UsID + ")</a><br />");

        string sInclude = string.Empty;
        if (model.Include == 1)
            sInclude = "(含下级版块)";
        if (model.ForumID == -1)
            builder.Append("管辖:全站");
        else if (model.ForumID == 0)
            builder.Append("管辖:全区版块");
        else
            builder.Append("管辖:<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + model.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ForumName + "</a>" + sInclude + "");

        int gNum = Utils.GetStringNum(model.Rolece, ";");
        builder.Append("<br />当前拥有权限(" + (gNum+1) + "/40)");
        builder.Append(Out.Tab("</div>", ""));

        string oTime = model.OverTime.ToString();
        if (model.OverTime.ToString() == "1990-1-1 0:00:00")
            oTime = "0";
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;
        string RoleString = BCW.User.Role.GetRoleString();
        if (model.ForumID > 0)
        {
            sText = "权限包括下级版:/,";
            sName = "Include,";
            sType = "select,";
            sValu = "" + model.Include + "'";
            sEmpt = "0|不含|1|包含,";
            RoleString = RoleString.Replace("|z|设置版主", "");
        }
        if (model.ForumID == -1)
        {
            strText = "选择全局权限，可多选/,选择总版权限，可不选或多选/,上任时间:/,卸任时间(填0则无限期):/,职称(如:正版|副版|见习版):/," + sText + "权限状态:/,,,";
            strName = "Roleall,Role,sDay,rDay,rName," + sName + "Status,id,act,backurl";
            strType = "multiple,multiple,date,text,text," + sType + "select,hidden,hidden,hidden";
            strValu = "" + BCW.User.Limits.GetLimitValu(model.Rolece) + "'" + BCW.User.Role.GetRoleValu(model.Rolece) + "'" + model.StartTime + "'" + oTime + "'" + model.RoleName + "'" + sValu + "" + model.Status + "'" + id + "'editsave'" + Utils.getPage(0) + "";
            strEmpt = "" + BCW.User.Limits.GetLimitString() + "," + BCW.User.Role.GetRoleString() + ",false,false,false," + sEmpt + "0|正常|1|暂停,false,false,false";
            strIdea = "/";
            strOthe = "编辑管理员|reset,moderator.aspx,post,1,red|blue";
        }
        else
        {
            strText = "选择权限，可多选/,上任时间:/,卸任时间(填0则无限期):/,职称(如:正版|副版|见习版):/," + sText + "权限状态:/,,,";
            strName = "Role,sDay,rDay,rName," + sName + "Status,id,act,backurl";
            strType = "multiple,date,text,text," + sType + "select,hidden,hidden,hidden";
            strValu = "" + model.Rolece + "'" + model.StartTime + "'" + oTime + "'" + model.RoleName + "'" + sValu + "" + model.Status + "'" + id + "'editsave'" + Utils.getPage(0) + "";
            strEmpt = "" + RoleString + ",false,false,false," + sEmpt + "0|正常|1|暂停,false,false,false";
            strIdea = "/";
            strOthe = "编辑版主|reset,moderator.aspx,post,1,red|blue";
        }
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />");
        builder.Append("锁定会员将使会员失去所有的权限.<br />");
        builder.Append("总版权限将有所有论坛的管理区域.<br />");
        builder.Append("设定版主功能需有该版块权限才有此权限");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">撤除此版主</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("moderator.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "post", 1, @"", ""));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string GetRole = string.Empty;
        if (ac == "编辑管理员")
        {
            string Roleall = Utils.GetRequest("Roleall", "post", 3, @"^[\w((;|,)\w)?]+$", "选择全局权限错误");
            Roleall = Roleall.Replace(",", ";");
            string Role = Utils.GetRequest("Role", "post", 3, @"^[\w((;|,)\w)?]+$", "选择权限错误");
            Role = Role.Replace(",", ";");
            GetRole = Roleall + ";" + Role;
            if (Roleall == "")
                GetRole = Utils.Mid(GetRole, 1, GetRole.Length);
        }
        else
        {
            string Role = Utils.GetRequest("Role", "post", 2, @"^[\w((;|,)\w)?]+$", "选择权限错误");
            Role = Role.Replace(",", ";");
            GetRole = Role;
        }
        DateTime sDay = Utils.ParseTime(Utils.GetRequest("sDay", "post", 2, DT.RegexTime, "上任时间填写错误"));
        string rDay = Utils.GetRequest("rDay", "post", 2, "", "");
        string rName = Utils.GetRequest("rName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "职称限10字内，不能使用特别字符");
        int Include = int.Parse(Utils.GetRequest("Include", "post", 1, @"^[0-1]$", "0"));
        int Status = int.Parse(Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "权限状态选择错误"));
        if (rDay == "0")
        {
            rDay = "1990-1-1 00:00:00";
        }
        else
        {
            if (!Utils.IsRegex(rDay, DT.RegexTime))
            {
                Utils.Error("卸任时间填写错误", "");
            }
        }
        BCW.Model.Role m = new BCW.BLL.Role().GetRole(id);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //取用户昵称
        string UsName = new BCW.BLL.User().GetUsName(m.UsID);
        //取论坛名称
        string ForumName = new BCW.BLL.Forum().GetTitle(m.ForumID);

        BCW.Model.Role model = new BCW.Model.Role();
        model.ID = id;
        model.UsName = UsName;
        model.Rolece = GetRole;
        model.RoleName = rName;
        model.ForumName = ForumName;
        model.StartTime = sDay;
        model.OverTime = DateTime.Parse(rDay);
        model.Include = Include;
        model.Status = Status;
        new BCW.BLL.Role().Update(model);
 
        if (ac == "编辑管理员")
        {
            new BCW.BLL.Guest().Add(m.UsID, UsName, "系统管理员编辑您的管理员权限");

            Utils.Success("编辑管理员", "恭喜，编辑管理员成功，正在返回..", Utils.getPage("moderator.aspx"), "1");
        }
        else
        {
            new BCW.BLL.Guest().Add(m.UsID, UsName, "系统管理员编辑您在论坛“" + ForumName + "”的版主权限");

            Utils.Success("编辑版主", "恭喜，编辑版主成功，正在返回..", Utils.getPage("moderator.aspx"), "1");
        }
    }

    private void HonorPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "设为荣誉版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将此版主设为荣誉版主吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?info=ok&amp;act=honor&amp;id=" + id + "") + "\">确定设置</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("moderator.aspx?ptype=2") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Role().ExistsOver(id))
            {
                Utils.Error("只有是历任版主才能设置荣誉版主", "");
            }
            //设置
            new BCW.BLL.Role().UpdateOver(id, 2);
            Utils.Success("设置荣誉版主", "设置荣誉版主成功..", Utils.getPage("moderator.aspx"), "1");
        }
    }

    private void DelHonorPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "取消荣誉版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将此版主取消荣誉版主吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?info=ok&amp;act=delhonor&amp;id=" + id + "") + "\">确定取消</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("moderator.aspx?ptype=4") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //取消
            new BCW.BLL.Role().UpdateOver(id, 0);
            Utils.Success("取消荣誉版主", "取消荣誉版主成功..", Utils.getPage("moderator.aspx"), "1");
        }
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "撤除版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定撤除此版主吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("moderator.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Role().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.Model.Role model = new BCW.BLL.Role().GetRole(id);
            //撤除
            new BCW.BLL.Role().Delete(id);
            //发送内线给版主
            if (model.ForumID > 0)
            {
                new BCW.BLL.Guest().Add(model.UsID, model.UsName, "系统管理员撤除您的版主权限");
                //任职记录
                new BCW.BLL.Forumlog().Add(14, id, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "(" + model.UsID + ")[/url]离任时间：" + DT.FormatDate(DateTime.Now, 11) + "");
        
            }
            else
            {
                new BCW.BLL.Guest().Add(model.UsID, model.UsName, "系统管理员撤除您的管理员权限");
            }
            Utils.Success("撤除版主", "撤除版主成功..", Utils.getUrl("moderator.aspx"), "1");
        }
    }
}
