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
using BCW.Files;
public partial class Manage_class : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "网站设计中心";

        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "view":
                GetClassView(act);
                break;
            case "del":
            case "dels":
                GetClassDel(act);
                break;
            case "move":
                GetClassMove(act);
                break;
            case "move2":
                GetClassMove2(act);
                break;
            case "sort":
                GetClassSort();
                break;
            case "del2":
                GetDel2();
                break;
            case "delpic":
                GetPicDel();
                break;
            case "delfile":
                GetFileDel();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    /// <summary>
    /// 网站设计中心
    /// </summary>
    private void ReloadPage()
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("网站设计中心");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (leibie == 0)
            builder.Append("首页设计|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=0") + "\">首页</a>|");

        if (leibie == 1)
            builder.Append("社区设计|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=1") + "\">社区</a>|");

        if (leibie == 2)
            builder.Append("论坛设计");
        else
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=2") + "\">论坛</a>");

        //if (leibie == 3)
        //    builder.Append("游戏设计");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=3") + "\">游戏</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + leibie + "") + "\">[添加菜单]</a>");
        builder.Append(Out.Tab("</div>", "<br />~~~~~~"));
        int pageIndex;
        int recordCount;
        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        int pageSize = 20;
        string strWhere = "";
        string[] pageValUrl = { "leibie" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "NodeId=0 and Leibie=" + leibie + "";

        // 开始读取专题
        IList<BCW.Model.Topics> listTopics = new BCW.BLL.Topics().GetTopicss(pageIndex, pageSize, strWhere, out recordCount);
        if (listTopics.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Topics n in listTopics)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string IsHidden = string.Empty;
                if (n.Hidden == 1)
                    IsHidden = "＾";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id={0}") + "\">[" + BCW.User.AppCase.CaseTopics(n.Types) + "]&gt;" + IsHidden + "</a>{1}.{2}", n.ID, n.Paixu, n.Title);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("text", "没有菜单记录"));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        if (leibie == 0)
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx?backurl=" + Utils.PostPage(1) + "") + "\">预览首页</a><br />");
        else if (leibie == 1)
            builder.Append("<a href=\"" + Utils.getUrl("../bbs/default.aspx?backurl=" + Utils.PostPage(1) + "") + "\">预览社区</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("../bbs/forum.aspx?backurl=" + Utils.PostPage(1) + "") + "\">预览论坛</a><br />");

        if (Utils.GetTopDomain().Equals("tl88.cc"))
            builder.Append("<a href=\"" + Utils.getUrl("filepass.aspx") + "\">审核文件</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "&amp;act=sort") + "\">高级排序菜单</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 查看菜单
    /// </summary>
    private void GetClassView(string act)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        if (!new BCW.BLL.Topics().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        //读取菜单
        BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);

        builder.Append(Out.Tab("<div class=\"title\">查看菜单</div>", ""));
        if (model.Types == 1 || model.Types > 10)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));

            if (model.Types == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?leibie=" + model.Leibie + "&amp;nid=" + id + "") + "\">添加</a>.<a href=\"" + Utils.getUrl("../default.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            }
            else if (model.Types == 11)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=text&amp;nid=" + id + "&amp;ptype=11") + "\">添加</a>.<a href=\"" + Utils.getUrl("../list.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            }
            else if (model.Types == 12)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=pic&amp;nid=" + id + "&amp;ptype=12") + "\">添加</a>.<a href=\"" + Utils.getUrl("../list.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            }
            else if (model.Types == 13)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=soft&amp;nid=" + id + "&amp;ptype=13") + "\">添加</a>.<a href=\"" + Utils.getUrl("../list.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            }
            else if (model.Types == 14)
            {
                builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=shop&amp;nid=" + id + "&amp;ptype=14") + "\">添加</a>.<a href=\"" + Utils.getUrl("../shop.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));

        if (model.Types == 6)
            builder.Append("项目名称:" + model.Title + "(ID:" + model.Content + ")<br />");
        else
            builder.Append("项目名称:" + model.Title + "<br />");

        builder.Append("项目类型:" + BCW.User.AppCase.CaseTopics(model.Types) + "<br />");
        builder.Append("项目编号:" + id + "<br />");
        if (model.IsBr == 0)
            builder.Append("项目换行:换行<br />");
        else
            builder.Append("项目换行:不换行<br />");

        builder.Append("项目排序:" + model.Paixu + "<br />");
        if (model.Types == 1)
        {
            if (model.Cent != 0)
            {
                string centType = "";
                if (model.SellTypes == 0)
                {
                    centType = "按次计费";
                }
                else if (model.SellTypes == 1)
                {
                    centType = "包周计费";
                }
                else
                {
                    centType = "包月计费";
                }
                if (model.BzType == 0)
                    builder.Append("是否收费:收费" + model.Cent + "" + ub.Get("SiteBz") + "<br />");
                else
                    builder.Append("是否收费:收费" + model.Cent + "" + ub.Get("SiteBz2") + "<br />");

                builder.Append("消费方式:" + centType + "<br />");
            }
            else
            {
                builder.Append("是否收费:免费<br />");
            }
            if (model.IsPc == 0)
            {
                builder.Append("浏览器限制:不限制<br />");
            }
            else
            {
                builder.Append("浏览器限制:仅限手机<br />");
            }
            if (model.VipLeven > 0)
            {
                builder.Append("VIP等级限制:" + model.VipLeven + "级<br />");
            }
            else
            {
                builder.Append("VIP等级限制:不限<br />");
            }
            if (!string.IsNullOrEmpty(model.InPwd))
            {
                builder.Append("页面密码:" + model.InPwd + "<br />");
            }
            else
            {
                builder.Append("页面密码:无密码<br />");
            }
        }
        else if (model.Types <= 5)
        {
            if (model.VipLeven > 0)
            {
                builder.Append("VIP等级可见:" + model.VipLeven + "级<br />");
            }
            else
            {
                builder.Append("VIP等级可见:不限<br />");
            }
        }
        if (model.Hidden == 0)
        {
            builder.Append("项目状态:正常显示");
        }
        else if (model.Hidden == 1)
        {
            builder.Append("项目状态:登录可见");
        }
        else
        {
            builder.Append("项目状态:隐藏显示");
        }
        builder.Append(Out.Tab("</div>", ""));

        if (model.Types == 1)
        {
            int pageIndex;
            int recordCount;
            //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            int pageSize = 20;
            string strWhere = "";
            string[] pageValUrl = { "act", "id" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "NodeId=" + id + "";

            // 开始读取专题
            IList<BCW.Model.Topics> listTopics = new BCW.BLL.Topics().GetTopicss(pageIndex, pageSize, strWhere, out recordCount);
            if (listTopics.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Topics n in listTopics)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string IsHidden = string.Empty;
                    if (n.Hidden == 2)
                        IsHidden = "＾";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id={0}") + "\">[" + BCW.User.AppCase.CaseTopics(n.Types) + "]&gt;" + IsHidden + "</a>{1}.{2}", n.ID, n.Paixu, n.Title);

                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append(Out.Div("text", "没有菜单记录"));
            }
        }
        else if (model.Types > 10 && model.Types<14)
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "id" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "NodeId=" + id + "";

            // 开始读取文章
            IList<BCW.Model.Detail> listDetail = new BCW.BLL.Detail().GetDetails(pageIndex, pageSize, strWhere, out recordCount);
            if (listDetail.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Detail n in listDetail)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string sIsAd = "";
                    if (n.IsAd)
                    {
                        if (model.Types == 11)
                            sIsAd = "文章";
                        else if (model.Types == 12)
                            sIsAd = "图片";
                        else if (model.Types == 13)
                            sIsAd = "文件";
                    }
                    else
                    {
                        sIsAd = "广告";
                    }
                    string IsHidden = string.Empty;
                    if (n.Hidden == 1)
                        IsHidden = "＾";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("classact.aspx?act=info&amp;ptype={0}&amp;nid={1}&amp;id={2}") + "\">[" + sIsAd + "]&gt;</a><a href=\"" + Utils.getUrl("/detail.aspx?id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + IsHidden + "{3}</a><a href=\"" + Utils.getUrl("commentary.aspx?act=comment&amp;id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[评论]</a><a href=\"" + Utils.getUrl("class.aspx?act=del2&amp;ptype={0}&amp;id={2}") + "\">[删]</a><a href=\"" + Utils.getUrl("class.aspx?act=move2&amp;ptype={0}&amp;id={2}") + "\">[移]</a>", n.Types, n.NodeId, n.ID, n.Title);

                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append(Out.Div("text", "没有相关记录"));
            }
        }
        else if (model.Types == 14)
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "id" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "NodeId=" + id + "";

            // 开始读取商品
            IList<BCW.Model.Goods> listGoods = new BCW.BLL.Goods().GetGoodss(pageIndex, pageSize, strWhere, out recordCount);
            if (listGoods.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Goods n in listGoods)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string sIsAd = "";
                    if (n.IsAd)
                    {
                        sIsAd = "商品";
                    }
                    else
                    {
                        sIsAd = "广告";
                    }
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("classact.aspx?act=info&amp;ptype={0}&amp;nid={1}&amp;id={2}") + "\">[" + sIsAd + "]&gt;</a><a href=\"" + Utils.getUrl("/shopdetail.aspx?id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a><a href=\"" + Utils.getUrl("shopbuy.aspx?id={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[订单]</a><a href=\"" + Utils.getUrl("class.aspx?act=del2&amp;ptype={0}&amp;id={2}") + "\">[删]</a><a href=\"" + Utils.getUrl("class.aspx?act=move2&amp;ptype={0}&amp;id={2}") + "\">[移]</a>", 14, n.NodeId, n.ID, n.Title);

                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append(Out.Div("text", "没有相关记录"));
            }
        }
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?ptype=" + model.Types + "&amp;id=" + id + "") + "\">编辑菜单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=move&amp;id=" + id + "") + "\">移动菜单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=del&amp;id=" + id + "") + "\">删除菜单</a><br />");
        string dText = string.Empty;
        if (model.Types == 11)
            dText = "文章";
        else if (model.Types == 12)
            dText = "图片";
        else if (model.Types == 13)
            dText = "文件";
        if (!string.IsNullOrEmpty(dText))
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=dels&amp;id=" + id + "") + "\">清空" + dText + "</a><br />");

        if (model.Types > 10)
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=move2&amp;ptype=" + model.Types + "&amp;hid=" + id + "") + "\">合并菜单</a><br />");
        }
        if (model.NodeId != 0)
        {
            builder.Append("<a href=\"" + Utils.getPage("class.aspx?act=view&amp;id=" + model.NodeId + "") + "\">返回上一级</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("class.aspx?leibie=" + model.Leibie + "") + "\">返回上一级</a><br />");
        }
        if (model.Types == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + model.Leibie + "&amp;act=sort&amp;id=" + id + "") + "\">高级排序菜单</a><br />");
        }
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
 
    private void GetClassDel(string act)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (!new BCW.BLL.Topics().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        //读取类型
        int ptype = new BCW.BLL.Topics().GetTypes(id);
        builder.Append(Out.Tab("<div class=\"title\">删除菜单</div>", ""));
        if (info != "ok")
        {
            if (ptype > 10)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("确定删除此" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "菜单吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=move2&amp;ptype=" + ptype + "&amp;hid=" + id + "") + "\">先转移" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "记录(合并)</a><br />");
                if (act == "del")
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除,包括" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "记录</a><br />");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=dels&amp;id=" + id + "") + "\">确定清空" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "记录</a><br />");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("确定删除此菜单吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            }

            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx") + "\">返回设计中心</a><br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //读取节点NodeId
            int nid = new BCW.BLL.Topics().GetNodeId(id);
            int leibie = new BCW.BLL.Topics().GetLeibie(id);
            if (ptype ==14)
            {
                //删除此菜单
                new BCW.BLL.Topics().Delete(id);
                //删除菜单的附件
                DataSet ds = new BCW.BLL.Goods().GetList("NodeId=" + id + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sFile = ds.Tables[0].Rows[i]["Files"].ToString();
                        string sCover = ds.Tables[0].Rows[i]["Cover"].ToString();
                        if (sCover != "")
                        {
                            BCW.Files.FileTool.DeleteFile(sCover);
                        }
                        string[] sTemp = sFile.Split("#".ToCharArray());
                        for (int k = 0; k < sTemp.Length; k++)
                        {
                            BCW.Files.FileTool.DeleteFile(sTemp[k].ToString());
                        }
                    }
                }
                //删除此菜单下的记录
                new BCW.BLL.Goods().DeleteNodeId(id);
                //删除订单记录
                new BCW.BLL.Buylist().Delete3(id);
            }
            if (ptype > 10 && ptype < 14)
            {
                //删除此菜单
                if (act == "del")
                    new BCW.BLL.Topics().Delete(id);

                //删除菜单的附件
                DataSet ds = new BCW.BLL.Detail().GetList("ID,Pics,Cover", "NodeId=" + id + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int NodeId = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        string sPic = ds.Tables[0].Rows[i]["Pics"].ToString();
                        string sCover = ds.Tables[0].Rows[i]["Cover"].ToString();
                        if (sCover != "")
                        {
                            BCW.Files.FileTool.DeleteFile(sCover);
                        }
                        string[] sTemp = sPic.Split("#".ToCharArray());
                        for (int k = 0; k < sTemp.Length; k++)
                        {
                            BCW.Files.FileTool.DeleteFile(sTemp[k].ToString());
                        }
                        //删除图片和文件下的文件
                        if (ptype == 12 || ptype == 13)
                        {
                            DataSet ds2 = new BCW.BLL.File().GetList("Files,PrevFiles", "NodeId=" + NodeId + "");
                            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {
                                    string Files = ds2.Tables[0].Rows[j]["Files"].ToString();
                                    string PrevFiles = ds2.Tables[0].Rows[j]["PrevFiles"].ToString();
                                    BCW.Files.FileTool.DeleteFile(Files);
                                    if (PrevFiles != "")
                                        BCW.Files.FileTool.DeleteFile(PrevFiles);
                                }
                            }
                            new BCW.BLL.File().Delete2(NodeId);
                        }
                    }
                }
                //删除此菜单下的记录
                new BCW.BLL.Detail().DeleteNodeId(id);
                //删除评论记录
                new BCW.BLL.Comment().Delete3(id);
            }
            else
            {
                if (new BCW.BLL.Topics().ExistsTypesIn(id))
                {
                    Utils.Error("此页面菜单里面存在子页面菜单，请先删除子页面菜单", "");
                }
                //删除此菜单
                new BCW.BLL.Topics().Delete(id);
                //删除此菜单下的其它菜单
                new BCW.BLL.Topics().DeleteNodeId(id);
            }
            builder.Append(Out.Tab("<div>", ""));
            if (act == "del")
            {
                builder.Append("删除菜单成功");
            }
            else
            {
                nid = id;
                builder.Append("清空" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "成功");
            }

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            if (nid != 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + nid + "") + "\">返回上一级</a><br />");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回上一级</a><br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 移动菜单
    /// </summary>
    private void GetClassMove(string act)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 1, @"^-?\d+$", "0"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (!new BCW.BLL.Topics().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int leibie = new BCW.BLL.Topics().GetLeibie(id);
        builder.Append(Out.Tab("<div class=\"title\">移动菜单</div>", ""));
        if (nid == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请选择移动到的页面菜单");
            builder.Append(Out.Tab("</div>", "<br />---------<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "leibie", "act", "id" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "Types=1 and Leibie=" + leibie + "";
            builder.Append(Out.Tab("<div>", ""));
            if (leibie == 1)
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=move&amp;nid=-1&amp;id=" + id + "") + "\">移到社区首页←</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=move&amp;nid=-1&amp;id=" + id + "") + "\">移到首页←</a>");

            builder.Append(Out.Tab("</div>", ""));
            // 开始读取专题
            IList<BCW.Model.Topics> listTopics = new BCW.BLL.Topics().GetTopicss(pageIndex, pageSize, strWhere, out recordCount);
            if (listTopics.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Topics n in listTopics)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id={0}") + "\">{1}</a>.<a href=\"" + Utils.getUrl("class.aspx?act=move&amp;nid={0}&amp;id=" + id + "") + "\">[移]</a>", n.ID, n.Title, id);

                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append(Out.Div("text", "没有菜单记录"));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        }
        else
        {
            if (info != "ok")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("确定移动到此菜单吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=move&amp;nid=" + nid + "&amp;id=" + id + "") + "\">确定移动</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=move&amp;id=" + id + "") + "\">先不移吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            }
            else
            {
                //首页标识
                if (nid == -1)
                {
                    nid = 0;
                }
                else
                {
                    //是否存在此页面菜单
                    if (!new BCW.BLL.Topics().ExistsTypes(nid))
                    {
                        Utils.Error("指定的页面菜单不存在", "");
                    }
                }
                //读取节点原NodeId
                int Ordnid = new BCW.BLL.Topics().GetNodeId(id);
                //读取节点原NodeId
                leibie = new BCW.BLL.Topics().GetLeibie(id);
                if (Ordnid == nid)
                {
                    Utils.Error("不同移动到同一菜单下", "");
                }

                //移动菜单
                new BCW.BLL.Topics().UpdateNodeId(id, nid);

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("移动菜单成功");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                if (nid != 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + nid + "") + "\">进入新菜单</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">进入新菜单</a><br />");
                }
                if (Ordnid != 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + Ordnid + "") + "\">返回原菜单</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回原菜单</a><br />");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    /// <summary>
    /// 移动记录
    /// </summary>
    private void GetClassMove2(string act)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "0"));
        int id = 0;
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^11|12|13|14$", "类型无效"));

        string MoveName = "合并";
        if (hid == 0)
        {
            id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
            MoveName = "移动";
        }
        int nid = int.Parse(Utils.GetRequest("nid", "get", 1, @"^-?\d+$", "0"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int Ordnid = 0;
        if (id != 0)
        {
            if (ptype == 14)
            {
                if (!new BCW.BLL.Goods().Exists(id))
                {
                    Utils.Error("不存在的记录", "");
                }
                //读取节点NodeId
                Ordnid = new BCW.BLL.Goods().GetNodeId(id);
            }
            else
            {
                if (!new BCW.BLL.Detail().Exists(id))
                {
                    Utils.Error("不存在的记录", "");
                }
                //读取节点NodeId
                Ordnid = new BCW.BLL.Detail().GetNodeId(id);
            }
        }
 
        builder.Append(Out.Tab("<div class=\"title\">" + MoveName + "菜单</div>", ""));
        if (nid == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请选择" + MoveName + "到的菜单");
            builder.Append(Out.Tab("</div>", "<br />---------"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "hid", "ptype","id" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "Types=" + ptype + "";
            // 开始读取专题
            IList<BCW.Model.Topics> listTopics = new BCW.BLL.Topics().GetTopicss(pageIndex, pageSize, strWhere, out recordCount);
            if (listTopics.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Topics n in listTopics)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    if (hid == 0)
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id={0}") + "\">{1}</a>.<a href=\"" + Utils.getUrl("class.aspx?act=move2&amp;nid={0}&amp;ptype=" + ptype + "&amp;id=" + id + "") + "\">[移]</a>", n.ID, n.Title, id);
                    else
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id={0}") + "\">{1}</a>.<a href=\"" + Utils.getUrl("class.aspx?act=move2&amp;nid={0}&amp;ptype=" + ptype + "&amp;hid=" + hid + "") + "\">[合并]</a>", n.ID, n.Title, id);
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append(Out.Div("text", "没有菜单记录"));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        }
        else
        {
            if (info != "ok")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("确定" + MoveName + "到此菜单吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                if (hid == 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=move2&amp;nid=" + nid + "&amp;ptype=" + ptype + "&amp;id=" + id + "") + "\">确定" + MoveName + "</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + Ordnid + "") + "\">先不" + MoveName + "吧..</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=move2&amp;nid=" + nid + "&amp;ptype=" + ptype + "&amp;hid=" + hid + "") + "\">确定" + MoveName + "</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + hid + "") + "\">先不" + MoveName + "吧..</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            }
            else
            {
                if (hid == 0)
                {
                    //是否存在此文章菜单
                    if (!new BCW.BLL.Topics().ExistsIdTypes(nid, ptype))
                    {
                        Utils.Error("指定的菜单不存在", "");
                    }
                    if (Ordnid == nid)
                    {
                        Utils.Error("不同" + MoveName + "到同一菜单下", "");
                    }
                    //移动菜单
                    if (ptype == 14)
                    {
                        new BCW.BLL.Goods().UpdateNodeId(id, nid);
                    }
                    else
                    {
                        new BCW.BLL.Detail().UpdateNodeId(id, nid);
                    }
                }
                else
                {
                    //是否存在此文章菜单
                    if (!new BCW.BLL.Topics().ExistsIdTypes(nid, ptype))
                    {
                        Utils.Error("指定的菜单不存在", "");
                    }
                    //是否存在此菜单
                    if (!new BCW.BLL.Topics().ExistsIdTypes(hid, ptype))
                    {
                        Utils.Error("原菜单不存在", "");
                    }
                    if (hid == nid)
                    {
                        Utils.Error("不同" + MoveName + "到同一菜单下", "");
                    }
                    //合并菜单
                    if (ptype == 14)
                    {
                        new BCW.BLL.Goods().UpdateNodeIds(nid, hid);
                    }
                    else
                    {
                        new BCW.BLL.Detail().UpdateNodeIds(nid, hid);
                    }
                }
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("" + MoveName + "成功");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + nid + "") + "\">进入新菜单</a><br />");
                if (hid == 0)
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + Ordnid + "") + "\">返回原菜单</a><br />");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + hid + "") + "\">返回原菜单</a><br />");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        if (hid == 0)
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + Ordnid + "") + "\">返回上一级</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + hid + "") + "\">返回上一级</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    /// <summary>
    /// 排序菜单
    /// </summary>
    private void GetClassSort()
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 2, @"^[0-9]\d*$", "类型错误"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (id != 0)
        {
            if (!new BCW.BLL.Topics().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">菜单高级排序</div>", ""));
        if (string.IsNullOrEmpty(info))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请选择排序类型:");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "&amp;info=reorder&amp;act=sort&amp;id=" + id + "") + "\">编号渐进排序</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "&amp;info=addsort&amp;act=sort&amp;id=" + id + "") + "\">编号自定义排序</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string strWhere = "";
            if (id == 0)//首页重新排序
            {
                strWhere = "Leibie=" + leibie + "and NodeId=0 Order By Paixu Asc";
            }
            else //页面菜单排序
            {
                strWhere = "Leibie=" + leibie + "and NodeId=" + id + " Order By Paixu Asc";
            }

            if (info == "reorder")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("编号渐进排序:");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "排序渐进:,,,,";
                string strName = "num,leibie,info,act,id";
                string strType = "snum,hidden,hidden,hidden,hidden";
                string strValu = "10'" + leibie + "'ok'sort'" + id + "";
                string strEmpt = "false,,,,";
                string strIdea = "/温馨提示:为了日后更好排版,建议10以上/";
                string strOthe = "渐进排序|reset,class.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "&amp;act=sort&amp;id=" + id + "") + "\">重选</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (info == "addsort")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("请输入编号,从小到大排序");
                builder.Append(Out.Tab("</div>", "<br />---------"));

                DataSet ds = new BCW.BLL.Topics().GetList(strWhere);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    Utils.Error("你还没有添加任何菜单..", "");
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string y = ",";
                    strText = strText + y + ds.Tables[0].Rows[i]["Paixu"].ToString() + "." + ds.Tables[0].Rows[i]["Title"].ToString();
                    strName = strName + y + "Norder" + i;
                    strType = strType + y + "snum";
                    strValu = strValu + "'" + ds.Tables[0].Rows[i]["Paixu"].ToString();
                    strEmpt = strEmpt + y + "false";
                }
                strText = Utils.Mid(strText, 1, strText.Length) + ",,,,";
                strName = Utils.Mid(strName, 1, strName.Length) + ",leibie,info,act,id";
                strType = Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
                strValu = Utils.Mid(strValu, 1, strValu.Length) + "'" + leibie + "'ok'sort'" + id + "";
                strEmpt = Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
                strIdea = "/";
                strOthe = "自定义排序|reset,class.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "&amp;act=sort&amp;id=" + id + "") + "\">重选</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (info == "ok")
            {
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                if (Utils.ToSChinese(ac) == "渐进排序")
                {
                    int num = int.Parse(Utils.GetRequest("num", "post", 2, @"^[0-9]\d*$", "渐进数目必须是数字"));

                    DataSet ds = new BCW.BLL.Topics().GetList(strWhere);
                    if (ds == null || ds.Tables[0].Rows.Count == 0)
                    {
                        Utils.Error("你还没有添加任何菜单..", "");
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        new BCW.BLL.Topics().UpdatePaixu(Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]), i * num);
                    }
                }
                else if (Utils.ToSChinese(ac) == "自定义排序")
                {
                    DataSet ds = new BCW.BLL.Topics().GetList(strWhere);
                    if (ds == null || ds.Tables[0].Rows.Count == 0)
                    {
                        Utils.Error("你还没有添加任何菜单..", "");
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        new BCW.BLL.Topics().UpdatePaixu(Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]), Convert.ToInt32(Request["Norder" + i + ""]));
                    }
                }
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("排序操作成功");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (id != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?leibie=" + leibie + "") + "\">返回上一级</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a><br />");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 删除文章/图片/文件/商品
    /// </summary>
    private void GetDel2()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^11|12|13|14$", "类型无效"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int nid = 0;
        if (ptype == 14)
        {
            if (!new BCW.BLL.Goods().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //读取节点NodeId
            nid = new BCW.BLL.Goods().GetNodeId(id);
        }
        else
        {
            if (!new BCW.BLL.Detail().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //读取节点NodeId
            nid = new BCW.BLL.Detail().GetNodeId(id);
        }

        builder.Append(Out.Tab("<div class=\"title\">删除" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "</div>", ""));

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除此" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "吗,删除" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "的同时将删除该" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "已上传的文件");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=del2&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("class.aspx?act=view&amp;id=" + nid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx") + "\">返回设计中心</a><br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //删除菜单下的文件
            if (ptype == 14)
            {
                string sFile = new BCW.BLL.Goods().GetFiles(id);
                string sCover = new BCW.BLL.Goods().GetCover(id);
                if (sCover != "")
                {
                    BCW.Files.FileTool.DeleteFile(sCover);
                }
                string[] sTemp = sFile.Split("#".ToCharArray());
                for (int i = 0; i < sTemp.Length; i++)
                {
                    BCW.Files.FileTool.DeleteFile(sTemp[i].ToString());
                }
                //删除此记录
                new BCW.BLL.Goods().Delete(id);
                //删除订单记录
                new BCW.BLL.Buylist().Delete2(id);

            }
            else
            {            
                //删除菜单下的截图
                string sPic = new BCW.BLL.Detail().GetPics(id);
                string sCover = new BCW.BLL.Detail().GetCover(id);
                if (sCover != "")
                {
                    BCW.Files.FileTool.DeleteFile(sCover);
                }
                string[] sTemp = sPic.Split("#".ToCharArray());
                for (int i = 0; i < sTemp.Length; i++)
                {
                    BCW.Files.FileTool.DeleteFile(sTemp[i].ToString());
                }
                //删除菜单下的文件
                if (ptype == 12 || ptype == 13)
                {
                    DataSet ds = new BCW.BLL.File().GetList("Files,PrevFiles", "NodeId=" + id + "");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string Files = ds.Tables[0].Rows[i]["Files"].ToString();
                            string PrevFiles = ds.Tables[0].Rows[i]["PrevFiles"].ToString();
                            BCW.Files.FileTool.DeleteFile(Files);
                            if (PrevFiles != "")
                                BCW.Files.FileTool.DeleteFile(PrevFiles);
                        }
                    }
                    new BCW.BLL.File().Delete2(id);
                }
                //删除此记录
                new BCW.BLL.Detail().Delete(id);
                //删除评论记录
                new BCW.BLL.Comment().Delete2(id);
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("删除" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "成功");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("class.aspx?act=view&amp;id=" + nid + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 删除已上传的附件
    /// </summary>
    private void GetFileDel()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^11|12|13|14$", "类型无效"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int nid = 0;
        if (ptype == 14)
        {
            if (!new BCW.BLL.Goods().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //读取节点NodeId
            nid = new BCW.BLL.Goods().GetNodeId(id);
        }
        else
        {
            if (!new BCW.BLL.Detail().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //读取节点NodeId
            nid = new BCW.BLL.Detail().GetNodeId(id);
        }
        builder.Append(Out.Tab("<div class=\"title\">删除已上传的附件</div>", ""));

        string actType = string.Empty;
        if (ptype == 11)
            actType = "text";
        else if (ptype == 12)
            actType = "pic";
        else if (ptype == 13)
            actType = "soft";
        else if (ptype == 14)
            actType = "shop";
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除该" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "已上传的文件吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=delfile&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + actType + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx") + "\">返回设计中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            string sFile = string.Empty;
            if (ptype == 11)
            {
                //删除菜单下的截图
                string sPic = new BCW.BLL.Detail().GetPics(id);
                string sCover = new BCW.BLL.Detail().GetCover(id);
                if (sCover != "")
                {
                    BCW.Files.FileTool.DeleteFile(sCover);
                }
                string[] sTemp = sPic.Split("#".ToCharArray());
                for (int i = 0; i < sTemp.Length; i++)
                {
                    BCW.Files.FileTool.DeleteFile(sTemp[i].ToString());
                }
                //删除表里的截图记录
                new BCW.BLL.Detail().DeletePics(id);
            }
            else if (ptype == 14)
            {
                //删除菜单下的文件
                sFile = new BCW.BLL.Goods().GetFiles(id);
                string sCover = new BCW.BLL.Goods().GetCover(id);
                if (sCover != "")
                {
                    BCW.Files.FileTool.DeleteFile(sCover);
                }
                string[] sTemp = sFile.Split("#".ToCharArray());
                for (int i = 0; i < sTemp.Length; i++)
                {
                    BCW.Files.FileTool.DeleteFile(sTemp[i].ToString());
                }
                //删除表里的文件记录
                new BCW.BLL.Goods().DeleteFiles(id);
            }
            else
            {
                //删除菜单下的文件
                DataSet ds = new BCW.BLL.File().GetList("Files,PrevFiles", "NodeId=" + id + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string Files = ds.Tables[0].Rows[i]["Files"].ToString();
                        string PrevFiles = ds.Tables[0].Rows[i]["PrevFiles"].ToString();
                        BCW.Files.FileTool.DeleteFile(Files);
                        if (PrevFiles != "")
                            BCW.Files.FileTool.DeleteFile(PrevFiles);
                    }
                }
                new BCW.BLL.File().Delete2(id);
                string sCover = new BCW.BLL.Detail().GetCover(id);
                if (sCover != "")
                {
                    BCW.Files.FileTool.DeleteFile(sCover);
                }
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("删除已上传的文件成功");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + actType + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 删除文件已上传的截图
    /// </summary>
    private void GetPicDel()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-9]\d*$", "类型无效"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (!new BCW.BLL.Detail().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">删除已上传的截图</div>", ""));
        //读取节点NodeId
        int nid = new BCW.BLL.Detail().GetNodeId(id);
        string actType = string.Empty;
        if (ptype == 13)
            actType = "soft";

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除该" + Utils.Left(BCW.User.AppCase.CaseTopics(ptype), 2) + "已上传的截图吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("class.aspx?info=ok&amp;act=delpic&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + actType + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        }
        else
        {
            //删除菜单下的截图
            string sPic = new BCW.BLL.Detail().GetPics(id);
            string sCover = new BCW.BLL.Detail().GetCover(id);
            if (sCover != "")
            {
                BCW.Files.FileTool.DeleteFile(sCover);
            }
            string[] sTemp = sPic.Split("#".ToCharArray());
            for (int i = 0; i < sTemp.Length; i++)
            {
                BCW.Files.FileTool.DeleteFile(sTemp[i].ToString());
            }
            //删除表里的截图记录
            new BCW.BLL.Detail().DeletePics(id);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("删除已上传的截图成功");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("classact.aspx?act=" + actType + "&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("class.aspx") + "\">返回设计中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}