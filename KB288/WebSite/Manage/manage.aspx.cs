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

public partial class Manage_manage : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "后台帐号管理";
        builder.Append(Out.Tab("", ""));

        string act = Utils.GetRequest("act", "all", 1, "", "");
        if (act == "edit")
            GetEditManage();
        else if (act == "add")
            GetAddManage();
        else if (act == "del")
            GetDelUser();
        else if (act == "check")
            GetCheckManage();
        else
            GetUserList();
    }

    private void GetUserList()
    {
        builder.Append(Out.Div("title", "帐号列表"));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("manage.aspx?act=add") + "\">添加帐号</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取帐号
        int ManageId = new BCW.User.Manage().IsManageLogin();
        IList<BCW.Model.Manage> listManage = new BCW.BLL.Manage().GetManages(pageIndex, pageSize, out recordCount);
        if (listManage.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Manage n in listManage)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                else
                    builder.Append(Out.Tab("<div>", ""));


                if (ManageId == n.ID)
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("manage.aspx?act=edit&amp;aid={0}") + "\">[管理]&gt;{1}({0}号)</a><br />上次登录:{2}", n.ID, n.sUser, n.sTime);
                    if (ManageId == 1)
                    {
                        builder.Append("<br />登录IP:" + n.sUserIP + "");
                    }
                    builder.Append("<br />");
                    
                }
                else
                {
                    if (ManageId == 1)
                    {
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("manage.aspx?act=edit&amp;aid={0}") + "\">[管理]&gt;{1}({0}号)</a><br />上次登录:{2}", n.ID, n.sUser, n.sTime);

                        builder.Append("<br />登录IP:" + n.sUserIP + "");
                        builder.Append("<br />");
                    }
                }

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("text", "没有相关记录"));
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("manage.aspx?act=edit") + "\">修改我的帐号</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manage.aspx?act=check") + "\">后台登录设置</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void GetAddManage()
    {
        builder.Append(Out.Div("title", "后台帐号添加"));
        //读取我的帐号
        BCW.Model.Manage mymodel = new BCW.BLL.Manage().GetModel(BCW.User.Users.userId());
        if (mymodel.ID != 1)
        {
            Utils.Error("以你的权限还不能添加帐号", "");
        }


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "添加帐号")
        {
            string strUser = Out.UBB(Utils.GetRequest("strUser", "post", 2, @"^[(A-Za-z0-9)]{6,15}$", "用户名应该是由6-15字母、数字的组合"));
            string strPass = Out.UBB(Utils.GetRequest("strPass", "post", 2, @"^[(A-Za-z0-9)]{6,15}$", "密码应该是由6-15位字母、数字的组合"));
            string strPassr = Out.UBB(Utils.GetRequest("strPassr", "post", 2, @"^[(A-Za-z0-9)]{6,15}$", "确认密码应该是由6-15位字母、数字的组合"));
            //检查是否重复
            if (new BCW.BLL.Manage().ExistsUser(strUser))
            {
                Utils.Error("用户名" + strUser + "已存在", "");
            }
            if (strPass != strPassr)
            {
                Utils.Error("确认密码不正确", "");
            }

            BCW.Model.Manage model = new BCW.Model.Manage();
            model.sUser = strUser;
            model.sPwd = Utils.MD5(strPass);
            model.sKeys = "";
            model.sTime = DateTime.Now;
            new BCW.BLL.Manage().Add(model);
            int MaxId = new BCW.BLL.Manage().GetMaxId()-1;
            model.ID = MaxId;
            model.sKeys = BCW.User.Users.SetUserKeys(MaxId, strUser,new Rand().RandNum(10));
            model.sKeys = Utils.Mid(model.sKeys, 0, model.sKeys.Length - 4);
            new BCW.BLL.Manage().UpdateKeys(model);
            Utils.Success("添加帐号", "帐号添加成功..", Utils.getUrl("manage.aspx"), "1");
        }
        else
        {
            string strText = "*用户名/,*密码:/,*确认密码/,,";
            string strName = "strUser,strPass,strPassr,act";
            string strType = "text,password,password,hidden";
            string strValu = "'''add";
            string strEmpt = "false,false,false,";
            string strIdea = "/用户名与密码必须是字母和数字的组合/";
            string strOthe = "添加帐号|reset,manage.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("manage.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void GetEditManage()
    {
        //读取我的帐号
        BCW.BLL.Manage bll = new BCW.BLL.Manage();
        BCW.Model.Manage mymodel = bll.GetModel(BCW.User.Users.userId());

        int myaid = mymodel.ID;
        string myaUser = mymodel.sUser;
        int aid = 0;
        string aUser = "";
        aid = int.Parse(Utils.GetRequest("aid", "all", 1, @"^[0-9]\d*$", "0"));
        if (aid == 0)
        {
            aid = myaid;
            aUser = myaUser;
        }
        else
        {
            if (myaid != 1 && myaid != aid)
            {
                Utils.Error("以你的权限还不能修改他人帐号", "");
            }

            if (bll.GetModel(aid) == null)
            {
                Utils.Error("帐号不存在", "");
            }
            BCW.Model.Manage model = bll.GetModel(aid);
            aid = model.ID;
            aUser = model.sUser;
        }

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "修改帐号")
        {
            string strUser = Utils.GetRequest("strUser", "post", 2, @"^[(A-Za-z0-9)]{6,15}$", "用户名应该是由6-15字母、数字的组合");
            string strPass = Utils.GetRequest("strPass", "post", 2, @"^[(A-Za-z0-9)]{6,15}$", "密码应该是由6-15位字母、数字的组合");
            string strPassr = Utils.GetRequest("strPassr", "post", 2, @"^[(A-Za-z0-9)]{6,15}$", "确认密码应该是由6-15位字母、数字的组合");
            if (new BCW.BLL.Manage().ExistsUser(strUser, aid))
            {
                Utils.Error("用户名" + strUser + "已存在", "");
            }
            if (strPass != strPassr)
            {
                Utils.Error("确认密码不正确", "");
            }

            BCW.Model.Manage model = new BCW.Model.Manage();
            model.sUser = strUser;
            model.sPwd = Utils.MD5(strPass);
            model.ID = aid;
            new BCW.BLL.Manage().Update(model);
            Utils.Success("修改帐号", "帐号修改成功..", Utils.getUrl("manage.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "管理:" + aUser + ""));
            string strText = "*用户名/,*密码:/,*确认密码/,,";
            string strName = "strUser,strPass,strPassr,aid,act";
            string strType = "text,password,password,hidden,hidden";
            string strValu = "'''" + aid + "'edit";
            string strEmpt = "false,false,false,,";
            string strIdea = "/用户名与密码必须是字母和数字的组合/";
            string strOthe = "修改帐号|reset,manage.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("manage.aspx?act=del&amp;aid=" + aid + "") + "\">删除帐号</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manage.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void GetDelUser()
    {
        Master.Title = "后台帐号删除";
        //读取我的帐号
        int aid = 0;
        aid = int.Parse(Utils.GetRequest("aid", "all", 1, @"^[0-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");

        BCW.BLL.Manage bll = new BCW.BLL.Manage();
        BCW.Model.Manage mymodel = bll.GetModel(BCW.User.Users.userId());
        if (mymodel.ID != 1)
        {
            Utils.Error("以你的权限还不能删除帐号", "");
        }

        if (aid == 1)
        {
            Utils.Error("系统保留帐号不能删除", "");
        }

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此帐号吗<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("manage.aspx?info=ok&amp;act=del&amp;aid=" + aid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("manage.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.BLL.Manage().Delete(aid);
            Utils.Success("删除帐号", "删除修改成功..", Utils.getUrl("manage.aspx"), "1");
        }
    }

    private void GetCheckManage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("以你的权限还不能进行此设置", "");
        }

        Master.Title = "后台登录设置";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove("xml_wap");//清缓存
        xml.Reload(); //加载网站配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string LoginExpir = Utils.GetRequest("LoginExpir", "post", 2, @"^[0-9]\d*$", "超时时间填写错误");
            string VerifyIP = Utils.GetRequest("VerifyIP", "post", 2, @"^[0-1]$", "启用IP异常选择错误");
            xml.ds["SiteLoginExpir"] = LoginExpir;
            xml.ds["SiteVerifyIP"] = VerifyIP;
            System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
            Utils.Success("后台登录设置", "登录设置成功，正在返回..", Utils.getUrl("manage.aspx?act=check"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "后台登录设置"));

            string strText = "超时时间(分钟):/,启用IP异常:/,";
            string strName = "LoginExpir,VerifyIP,act";
            string strType = "num,select,hidden";
            string strValu = "" + xml.ds["SiteLoginExpir"] + "'" + xml.ds["SiteVerifyIP"] + "'check";
            string strEmpt = "false,0|不启用|1|启用,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,manage.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />当后台管理员登录后超出设置的时间则需输入密码继续管理,不启用请填0<br />启用IP异常则使用不同IP登录时需重新登录,非常安全");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("manage.aspx") + "\">帐号管理</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}