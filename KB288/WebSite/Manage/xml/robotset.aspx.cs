using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;

public partial class Manage_xml_robotset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                addPage();
                break;
            case "select":
                selectPage();
                break;
            case "del":
                delPage();
                break;
            case "look":
                checkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "系统号ID设置";
        builder.Append(Out.Div("title", "系统号ID设置"));

        builder.Append(Out.Tab("<div>", "<br/>"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">确定增加" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?ptype=1") + "\">确定增加</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">确定查询" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?ptype=2") + "\">确定查询</a>" + "");
        }
        builder.Append(Out.Tab("</div>", ""));
        string strText = string.Empty;
        if (ptype == 1)
            strText = "增加ID:/";
        else
            strText = "查询ID:/";
        string strName = "ID";
        string strType = "text";
        string strValu = "";
        string strEmpt = "true";
        string strIdea = "/";
        string strOthe = string.Empty;
        if (ptype == 1)
            strOthe = "确定增加,robotset.aspx?act=add,post,1,red";
        else
            strOthe = "确定查询,robotset.aspx?act=select,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        DataSet ds = new BCW.BLL.User().GetList("COUNT(*) AS aa", "IsSpier=1");
        int _num = 0;
        try
        {
            _num = int.Parse(ds.Tables[0].Rows[0]["aa"].ToString());
        }
        catch { }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("目前系统号ID总数为：" + _num + "<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?act=look") + "\">查看操作记录>></a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void addPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 26 && ManageId != 9)
        {
            Utils.Error("只有系统管理员1号、9号、26号才可以进行此操作", "");
        }

        int ID = Utils.ParseInt(Utils.GetRequest("ID", "all", 2, @"^[1-9]\d*$", "输入用户ID无效"));
        if (new BCW.BLL.User().Exists(ID))
        {
            DataSet ds = new BCW.BLL.User().GetList("IsSpier", "ID=" + ID + "");
            if (int.Parse(ds.Tables[0].Rows[0]["IsSpier"].ToString()) == 0)
            {
                new BCW.BLL.User().update_ziduan("IsSpier=1", "ID=" + ID + "");
                BCW.Model.xitonghao aa = new BCW.Model.xitonghao();
                aa.UsID = ManageId;
                aa.caoID = ID;
                aa.AddTime = DateTime.Now;
                aa.IP = Utils.GetUsIP();
                aa.type = 0;
                new BCW.BLL.xitonghao().Add(aa);
                Utils.Success("设置", "设置成功,正在返回.....", Utils.getUrl("robotset.aspx"), "1");
            }
            else
            {
                Utils.Success("设置", "该ID已为系统号,不用重复设置,正在返回.....", Utils.getUrl("robotset.aspx"), "2");
            }
        }
        else
            Utils.Error("不存在的会员.添加失败.", "");

    }

    private void selectPage()
    {
        int ID = Utils.ParseInt(Utils.GetRequest("ID", "all", 2, @"^[1-9]\d*$", "输入用户ID无效"));
        if (new BCW.BLL.User().Exists(ID))
        {
            Master.Title = "系统号ID设置";
            builder.Append(Out.Div("title", "系统号ID查询"));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("ID：" + ID + "<br/>");
            builder.Append("用户名：" + new BCW.BLL.User().GetUsName(ID) + "<br/>");
            builder.Append("手机：" + new BCW.BLL.User().GetMobile(ID) + "<br/>");
            DataSet ds = new BCW.BLL.User().GetList("IsSpier", "ID=" + ID + "");
            if (int.Parse(ds.Tables[0].Rows[0]["IsSpier"].ToString()) == 0)
                builder.Append("是否为系统号：不是<br/>");
            else
                builder.Append("是否为系统号：是<br/>");
            builder.Append("操作：<a href=\"" + Utils.getUrl("robotset.aspx?act=del&amp;id=" + ID + "") + "\">删除</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
            Utils.Error("不存在的会员.查询失败.", "");

    }

    private void delPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 26 && ManageId != 9)
        {
            Utils.Error("只有系统管理员1号、9号、26号才可以进行此操作", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int ID = int.Parse(Utils.GetRequest("ID", "all", 2, @"^[1-9]\d*$", "ID错误"));
            if (new BCW.BLL.User().Exists(ID))
            {
                DataSet ds = new BCW.BLL.User().GetList("IsSpier", "ID=" + ID + "");
                if (int.Parse(ds.Tables[0].Rows[0]["IsSpier"].ToString()) == 0)
                    Utils.Error("该ID已不是系统号,不用重复操作.", "");
                else
                {
                    Master.Title = "删除该ID为系统号吗";
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    builder.Append("删除该ID为系统号吗");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?info=ok&amp;act=del&amp;id=" + ID + "") + "\">确定删除</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?act=select&amp;id=" + ID + "") + "\">先留着吧..</a>");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
            }
            else
                Utils.Error("不存在的会员.操作失败.", "");
        }
        else
        {
            int ID = int.Parse(Utils.GetRequest("ID", "get", 2, @"^[1-9]\d*$", "ID错误"));
            new BCW.BLL.User().update_ziduan("IsSpier=0", "ID=" + ID + "");
            BCW.Model.xitonghao aa = new BCW.Model.xitonghao();
            aa.UsID = ManageId;
            aa.caoID = ID;
            aa.AddTime = DateTime.Now;
            aa.IP = Utils.GetUsIP();
            aa.type = 1;
            new BCW.BLL.xitonghao().Add(aa);
            Utils.Success("删除", "删除系统号成功..", Utils.getPage("robotset.aspx?act=select&amp;ID=" + ID + ""), "1");
        }
    }

    private void checkPage()
    {
        Master.Title = "操作记录";
        builder.Append(Out.Div("title", "操作记录"));
        builder.Append(Out.Tab("<div>", "<br/>"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">增加操作" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?act=look&amp;ptype=1&amp;uid=" + uid + "") + "\">增加操作</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">删除操作" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx?act=look&amp;ptype=2&amp;uid=" + uid + "") + "\">删除操作</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "type=0";
        else
            strWhere += "type=1";

        if (uid > 0)
            strWhere += "and caoID=" + uid + "";

        string[] pageValUrl = { "act", "ptype", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        IList<BCW.Model.xitonghao> list = new BCW.BLL.xitonghao().Getxitonghaos(pageIndex, pageSize, strWhere, out recordCount);
        if (list.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.xitonghao n in list)
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
                if (n.type == 0)
                    builder.Append("管理员" + n.UsID + "号增加系统号:" + n.caoID + "。操作IP:" + n.IP + "[" + n.AddTime + "]");
                else
                    builder.Append("管理员" + n.UsID + "号取消系统号:" + n.caoID + "。操作IP:" + n.IP + "[" + n.AddTime + "]");
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

        string strValu = ",";
        string strText = "输入被操作ID:/,";
        string strName = "uid,act";
        string strType = "num,hidden";
        if (uid > 0)
            strValu = "" + uid + "'look";
        else
            strValu = "'look";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "查询,robotset.aspx?ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("robotset.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
