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
/// 修改会员发喇叭,版主以上可支持UBB
/// 黄国军修改 20160203
/// </summary>
public partial class bbs_network : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/network.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("NetworkStatus", xmlPath) == "1")
        {
            Utils.Safe("广播系统");
        }
        Master.Title = ub.GetSub("NetworkName", xmlPath);

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();          //发布广播
                break;
            case "addsave":         //保存页
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            //case "fill":
            //    FillPage();
            //    break;
            //case "fillsave":
            //    FillSavePage();
            //    break;
            case "me":
                ReloadPage("me");
                break;
            case "del":
                DelPage();
                break;
            default:
                ReloadPage("");
                break;
        }
    }
    private void ReloadPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = ub.GetSub("NetworkName", xmlPath);
        if (ub.GetSub("NetworkLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("NetworkLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("NetworkNotes", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + ub.GetSub("NetworkNotes", xmlPath) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
            builder.Append("正在广播|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=" + act + "&amp;ptype=1") + "\">正在</a>|");

        if (ptype == 2)
            builder.Append("过期广播|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=" + act + "&amp;ptype=2") + "\">过期</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=add") + "\">发布</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types<=1 and  ";
        if (act == "me")
        {
            if (meid == 0)
                Utils.Login();

            strWhere += "UsID=" + meid + "AND ";
        }
        if (ptype == 1)
            strWhere += " OverTime>='" + DateTime.Now + "'";
        else
            strWhere += " OverTime<'" + DateTime.Now + "'";

        // 开始读取列表
        IList<BCW.Model.Network> listNetwork = new BCW.BLL.Network().GetNetworks(pageIndex, pageSize, strWhere, out recordCount);
        if (listNetwork.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Network n in listNetwork)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");

                if (n.IsUbb == 1 || Utils.GetTopDomain().Contains("th"))
                {
                    builder.Append(Out.SysUBB(n.Title));
                }
                else
                {
                    if (new BCW.BLL.Role().IsAllMode(n.UsID))
                        builder.Append(Out.SysUBB(n.Title));
                    else
                        builder.Append(n.Title);
                }
                if (ptype == 1)
                    builder.Append("(" + DT.DateDiff2(DateTime.Now, n.OverTime) + "后过期)");
                else
                    builder.Append("(" + DT.DateDiff2(n.OverTime, DateTime.Now) + "前过期)");

                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>(" + DT.FormatDate(n.AddTime, 6) + ")");
                if (n.UsID == meid)
                    builder.Append("|<a href=\"" + Utils.getUrl("network.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">修改</a>");

                //if (n.Types == 1)
                //    builder.Append("|<a href=\"" + Utils.getUrl("network.aspx?act=fill&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">延时</a>");

                if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_NetWork, meid))
                {
                    builder.Append("|<a href=\"" + Utils.getUrl("network.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a>");
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        if (act == "me")
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx") + "\">所有广播</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=me") + "\">我的广播</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    #region 发布广播 AddPage
    /// <summary>
    /// 发布广播
    /// </summary>
    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!Utils.GetTopDomain().Contains("th") && new BCW.BLL.Role().IsAllMode(meid))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您是版主以上权限，广播内容支持Ubb");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布广播");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "广播内容:限" + ub.GetSub("NetworksLength", xmlPath) + "-" + ub.GetSub("NetworkbLength", xmlPath) + "字/,显示时长:限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟/,允许其它用户延时:/,";
        string strName = "Title,Times,Types,act";
        string strType = "text,num,hidden,hidden";
        string strValu = "''0'addsave";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "发布|reset,network.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("network.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("network.aspx?act=me") + "\">我的广播</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 广播保存 AddSavePage
    /// <summary>
    /// 保存页
    /// </summary>
    private void AddSavePage()
    {
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int RegDay = Utils.ParseInt(ub.GetSub("NetworkRegDay", xmlPath));
        int Grade = Utils.ParseInt(ub.GetSub("NetworkGrade", xmlPath));
        if (RegDay > 0 || Grade > 0)
        {
            DataSet ds = new BCW.BLL.User().GetList("RegTime,Leven", "id=" + meid + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime RegTime = DateTime.Parse(ds.Tables[0].Rows[0]["RegTime"].ToString());
                int Leven = int.Parse(ds.Tables[0].Rows[0]["Leven"].ToString());
                if (RegDay > 0 && RegTime > DateTime.Now.AddDays(-RegDay))
                {
                    Utils.Error("注册不到" + RegDay + "天不能发布广播", "");
                }

                if (Grade > 0 && Leven < Grade)
                {
                    Utils.Error("发布广播需要等级" + Grade + "级，您目前等级" + Leven + "级", "");
                }
            }
        }
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_NetWork, meid);//会员本身权限
        string mename = new BCW.BLL.User().GetUsName(meid);
        long megold = new BCW.BLL.User().GetGold(meid);

        //每分钟收费多少
        int bMinute = Convert.ToInt32(ub.GetSub("NetworkiGold", xmlPath));

        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{" + ub.GetSub("NetworksLength", xmlPath) + "," + ub.GetSub("NetworkbLength", xmlPath) + "}$", "内容限" + ub.GetSub("NetworksLength", xmlPath) + "-" + ub.GetSub("NetworkbLength", xmlPath) + "字");
        int Times = int.Parse(Utils.GetRequest("Times", "post", 2, @"^[0-9]\d*$", "显示时长限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟"));
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-9]\d*$", "允许其它用户延时选择出错"));
        if (Times < 1 || Times > Convert.ToInt32(ub.GetSub("NetworkbMinute", xmlPath)))
        {
            Utils.Error("显示时长限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟", "");
        }
        if (info != "ok")
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定发布");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("内容：" + Title + "");
            builder.Append("<br />时长：" + Times + "分钟");
            //if(Types==1)
            //    builder.Append("|允许其它用户延时");
            //else
            //    builder.Append("|不允许其它用户延时");

            builder.Append("<br />收费：" + (Times * bMinute) + "" + ub.Get("SiteBz") + "");
            builder.Append("<br />自带：" + megold + "" + ub.Get("SiteBz") + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            string strName = "Title,Times,Types,info,act";
            string strValu = "" + Title + "'" + Times + "'" + Types + "'ok'addsave";
            string strOthe = "确定发布,network.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("network.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //扣币
            if (megold < Convert.ToInt64(Times * bMinute))
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }

            //支付安全提示
            string[] p_pageArr = { "act", "info", "Title", "Times", "Types" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            //{
            // int TodayCount = new BCW.BLL.Network().GetTodayCount(meid);
            //if (TodayCount >= 30)
            //{
            //  Utils.Error("为了版面清洁，每天每ID只可以发表30条广播", "");
            //  }
            //  }

            new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(Times * bMinute), "发布广播");

            BCW.Model.Network model = new BCW.Model.Network();
            model.Title = Title;
            model.Types = Types;
            model.UsID = meid;
            model.UsName = mename;
            model.OverTime = DateTime.Now.AddMinutes(Times);
            model.AddTime = DateTime.Now;
            model.OnIDs = "";

            if (!Utils.GetTopDomain().Contains("th") && new BCW.BLL.Role().IsAllMode(meid))
            {
                //版主以上权限可支持UBB
                model.IsUbb = 1;
            }
            else
            {
                model.IsUbb = 0;
            }
            new BCW.BLL.Network().Add(model);

            Utils.Success("发布广播", "发布成功，正在返回..", Utils.getUrl("network.aspx"), "1");
        }
    }
    #endregion

    private void EditPage()
    {

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!new BCW.BLL.Network().Exists(id, meid))
        {
            Utils.Error("不存在的记录", "");
        }
        //读取实体
        BCW.Model.Network model = new BCW.BLL.Network().GetNetwork(id);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改广播");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "广播内容:限" + ub.GetSub("NetworksLength", xmlPath) + "-" + ub.GetSub("NetworkbLength", xmlPath) + "字/,允许其它用户延时:/,,,";
        string strName = "Title,Types,id,act,backurl";
        string strType = "text,hidden,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Types + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改|reset,network.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("network.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_NetWork, meid);//会员本身权限
        string mename = new BCW.BLL.User().GetUsName(meid);

        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{" + ub.GetSub("NetworksLength", xmlPath) + "," + ub.GetSub("NetworkbLength", xmlPath) + "}$", "内容限" + ub.GetSub("NetworksLength", xmlPath) + "-" + ub.GetSub("NetworkbLength", xmlPath) + "字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-9]\d*$", "允许其它用户延时选择出错"));

        BCW.Model.Network model = new BCW.Model.Network();
        model.ID = id;
        model.Title = Title;
        model.Types = Types;
        model.UsID = meid;
        model.UsName = mename;
        new BCW.BLL.Network().UpdateBasic(model);

        Utils.Success("修改广播", "修改广播成功，正在返回..", Utils.getPage("network.aspx"), "1");
    }

    private void FillPage()
    {

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!new BCW.BLL.Network().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("延时广播");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "显示时长:限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟/,,,";
        string strName = "Times,id,act,backurl";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "1'" + id + "'fillsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定延时|reset,network.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("network.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FillSavePage()
    {
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        long megold = new BCW.BLL.User().GetGold(meid);

        //每分钟收费多少
        int iGold = Convert.ToInt32(ub.GetSub("NetworkiGold", xmlPath));

        int Times = int.Parse(Utils.GetRequest("Times", "post", 2, @"^[0-9]\d*$", "显示时长限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟"));
        if (Times < 1 || Times > Convert.ToInt32(ub.GetSub("NetworkbMinute", xmlPath)))
        {
            Utils.Error("显示时长限1-" + ub.GetSub("NetworkbMinute", xmlPath) + "分钟", "");
        }

        if (info != "ok")
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定延时");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("时长：" + Times + "分钟");

            builder.Append("<br />收费：" + (Times * iGold) + "" + ub.Get("SiteBz") + "");
            builder.Append("<br />自带：" + megold + "" + ub.Get("SiteBz") + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            string strName = "Times,info,id,act,backurl";
            string strValu = "" + Times + "'ok'" + id + "'fillsave'" + Utils.getPage(0) + "";
            string strOthe = "确定延时,network.aspx,post,0,other";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("network.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //扣币
            if (megold < Convert.ToInt64(Times * iGold))
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            BCW.Model.Network model = new BCW.BLL.Network().GetNetwork(id);
            if (model == null)
            {
                Utils.Error("不存在的广播记录", "");
            }
            if (model.Types == 0 && model.UsID != meid)
            {
                Utils.Error("此广播不允许其它用户延时", "");
            }
            //未过期不能延时
            if (model.OverTime > DateTime.Now)
            {
                Utils.Error("未过期的广播不能延时", "");
            }

            new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(Times * iGold), "发布广播");
            if (model.OverTime > DateTime.Now)
                new BCW.BLL.Network().Update(id, model.OverTime.AddMinutes(Times));
            else
                new BCW.BLL.Network().Update(id, DateTime.Now.AddMinutes(Times));

            Utils.Success("延时广播", "延时广播成功，正在返回..", Utils.getPage("network.aspx"), "1");
        }
    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_NetWork, meid))
        {
            Utils.Error("你的权限不足", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除广播";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此广播记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("network.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("network.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Network().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Network().Delete(id);
            Utils.Success("删除广播", "删除广播成功..", Utils.getPage("network.aspx"), "1");
        }
    }
}