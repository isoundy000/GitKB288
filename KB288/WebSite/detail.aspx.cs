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
/// 陈志基 修改点击支持反对可刷新问题
/// 2016/06/25
/// </summary>
public partial class detail : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/front.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Detail().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int pageIndex;
        int recordCount;
        int pageSize = Utils.ParseInt(ub.GetSub("FtTextDetailNum", xmlPath));
        int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-9]\d*$", "0"));
        string[] pageValUrl = { "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int pn = Utils.ParseInt(Request.QueryString["pn"]);
        if (pn == 0)
            pn = 1;

        int meid = 0;
        string act = Utils.GetRequest("act", "get", 1, "", "");
        BCW.Model.Detail model = new BCW.BLL.Detail().GetDetail(id);
        Master.Title = model.Title;
        string NodeTitle = new BCW.BLL.Topics().GetTitle(model.NodeId);
        //收费
        if (model.Types != 13)
        {
            if (model.Cent > 0)
            {
                if (meid == 0)
                {
                    meid = new BCW.User.Users().GetUsId();
                    if (meid == 0)
                        Utils.Login();//显示登录
                }
                string BT = string.Empty;
                string Bz = string.Empty;
                long megold = 0;
                if (model.Types == 11)
                {
                    BT = "文章";
                }
                else
                {
                    BT = "图片";
                }
                if (model.BzType == 0)
                {
                    megold = new BCW.BLL.User().GetGold(meid);
                    Bz = ub.Get("SiteBz");
                }
                else
                {
                    megold = new BCW.BLL.User().GetMoney(meid);
                    Bz = ub.Get("SiteBz2");
                }
                string payIDs = "|" + model.PayId + "|";
                if (act != "ok" && payIDs.IndexOf("|" + meid + "|") == -1)
                {
                    new Out().head(Utils.ForWordType("温馨提示"));
                    Response.Write(Out.Tab("<div class=\"text\">", ""));
                    Response.Write("" + BT + "收费" + model.Cent + "" + Bz + "，确定要浏览吗？扣费一次，永久浏览");
                    Response.Write(Out.Tab("</div>", "<br />"));
                    Response.Write(Out.Tab("<div>", ""));
                    Response.Write("您自带" + megold + "" + Bz + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay") + "\">[充值]</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("detail.aspx?act=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">马上进入浏览</a><br />");
                    Response.Write("<a href=\"" + Utils.getPage("list.aspx?id=" + model.NodeId + "") + "\">返回上级</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }
                if (payIDs.IndexOf("|" + meid + "|") == -1)
                {
                    if (megold < model.Cent)
                    {
                        Utils.Error("您的" + Bz + "不足", "");
                    }
                    //扣币
                    if (model.BzType == 0)
                        new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(model.Cent), "浏览收费" + BT + "");
                    else
                        new BCW.BLL.User().UpdateiMoney(meid, -Convert.ToInt64(model.Cent), "浏览收费" + BT + "");

                    //更新
                    payIDs = model.PayId + "|" + meid;
                    new BCW.BLL.Detail().UpdatePayId(id, payIDs);
                }
            }
        }
        if (model.Types == 11)
        {
            if (act.Contains("down"))
            {
                if (meid == 0)
                {
                    meid = new BCW.User.Users().GetUsId();
                    if (meid == 0)
                        Utils.Login();//显示登录
                }
                //下载文章
                BCW.User.Down.DownText(act, model);
            }

            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(12));
        }
        //顶部调用
        string TopUbb = string.Empty;
        if (model.Types == 11)
        {
            TopUbb = ub.GetSub("FtTextDetailTop", xmlPath);
        }
        else if (model.Types == 12)
        {
            TopUbb = ub.GetSub("FtPicDetailTop", xmlPath);
        }
        else if (model.Types == 13)
        {
            TopUbb = ub.GetSub("FtFileDetailTop", xmlPath);
        }
        if (TopUbb != "")
        {
            TopUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(TopUbb));
            if (TopUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(TopUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(TopUbb);
            }
        }
        if (model.Types == 11)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
            if (!Utils.GetDomain().Contains("boyi929"))
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?id=298") + "\">新闻</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("list.aspx?id=" + model.NodeId + "") + "\">" + new BCW.BLL.Topics().GetTitle(model.NodeId) + "</a>&gt;正文");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "</div>", "" + model.Title + ""));
        if (model.Types == 11)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + DT.FormatDate(model.AddTime, 1) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("", Out.LHr()));
        if (pageIndex == 1)
        {
            string Pics = string.Empty;
            Pics = model.Pics.Trim();
            if (!string.IsNullOrEmpty(Pics))
            {
                string[] txtPic = Pics.Split("#".ToCharArray());
                if (model.Types != 12)
                {
                    builder.Append(Out.Tab("<div>", "<br />"));

                    if (pn > txtPic.Length)
                        pn = txtPic.Length;
                    builder.Append("<img src=\"" + Out.SysUBB(txtPic[pn - 1]) + "\" alt=\"load\"/>");
                    if (pn < txtPic.Length)
                    {
                        builder.Append("<br /><a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "&amp;pn=" + (pn + 1) + "") + "\">下张</a> ");
                    }
                    if (pn > 1)
                    {
                        if (pn >= txtPic.Length)
                            builder.Append("<br />");
                        builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "&amp;pn=" + (pn - 1) + "") + "\">上张</a>");
                    }
                    if (txtPic.Length > 1)
                        builder.Append("(" + pn + "/" + txtPic.Length + ")");

                    builder.Append(Out.Tab("</div>", ""));
                }
            }

            if (model.Types == 13)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                if (!string.IsNullOrEmpty(model.TarText))
                {
                    builder.Append("[资费]:" + model.TarText + "<br />");
                }
                builder.Append("[分类]:<a href=\"" + Utils.getUrl("list.aspx?id=" + model.NodeId + "") + "\">" + NodeTitle + "</a><br />");
                builder.Append("[更新]:" + DT.FormatDate(model.AddTime, 11) + "<br />");
                if (!string.IsNullOrEmpty(model.LanText))
                {
                    builder.Append("[语言]:" + model.LanText + "<br />");
                }
                if (!string.IsNullOrEmpty(model.SafeText))
                {
                    builder.Append("[检查]:" + model.SafeText + "<br />");
                }
                if (model.IsVisa > 0)
                {
                    string IsVisa = string.Empty;
                    if (model.IsVisa == 1)
                        IsVisa = "未知";
                    else if (model.IsVisa == 1)
                        IsVisa = "需要";
                    else
                        IsVisa = "不需要";

                    builder.Append("[签证]:" + IsVisa + "<br />");
                }
                if (!string.IsNullOrEmpty(model.LyText))
                {
                    builder.Append("[来源]:" + model.LyText + "<br />");
                }
                string Content = string.Empty;
                if (pover == 0)
                {
                    if (model.Content.Length > 200)
                    {
                        Content = "" + Out.SysUBB(Utils.Left(model.Content, 200)) + "<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;pover=1&amp;&amp;backurl=" + Utils.PostPage(1) + "") + "\">详细&gt;&gt;</a>";
                    }
                    else
                    {
                        Content = Out.SysUBB(model.Content);
                    }
                }
                else
                {
                    Content = Out.SysUBB(model.Content);
                }
                builder.Append("[简介]:" + Content + "<br />");
                if (!string.IsNullOrEmpty(model.UpText))
                {
                    builder.Append("[更新]:" + model.UpText + "<br />");
                }
                if (!string.IsNullOrEmpty(model.Model))
                {
                    //机型适配
                    string PhoneBrand = "";
                    string PhoneModel = "";
                    string PhoneSystem = "";

                    if (Request.Cookies["BrandComment"] != null)
                    {
                        PhoneBrand = HttpUtility.UrlDecode(Request.Cookies["BrandComment"]["PhoneBrand"]);
                        PhoneModel = HttpUtility.UrlDecode(Request.Cookies["BrandComment"]["PhoneModel"]);
                        PhoneSystem = HttpUtility.UrlDecode(Request.Cookies["BrandComment"]["PhoneSystem"]);
                    }
                    builder.Append("<a href=\"" + Utils.getUrl("demore.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">适用机型</a>:");
                   
                    if (PhoneModel != "" && model.Model.Contains(PhoneBrand))
                    {
                        string Model = "," + model.Model + ",";
                        if (Model.IndexOf("," + PhoneModel + ",") != -1)
                        {
                            builder.Append("" + (PhoneBrand + PhoneModel) + "");
                        }
                        else if (Model.IndexOf("," + PhoneSystem + ",") != -1)
                        {
                            builder.Append("" + PhoneSystem + "");
                        }
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.PostPage(1) + "") + "\">未设置</a>");
                    }
                }
                builder.Append(Out.Tab("</div>", ""));
            }

            if (model.Types == 12 || model.Types == 13)
            {
                pageSize = Utils.ParseInt(ub.GetSub("FtPicListNum", xmlPath));
                if (model.Types == 13)
                    pageSize = 20;

                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                //查询条件
                string strWhere = "NodeId=" + id + "";
                // 开始读取列表
                IList<BCW.Model.File> listFile = new BCW.BLL.File().GetFiles(pageIndex, pageSize, strWhere, out recordCount);
                if (listFile.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.Model.File n in listFile)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));

                        if (model.Types == 12)
                        {
                            builder.Append("<img src=\"" + ((n.PrevFiles == "") ? n.Files : n.PrevFiles.ToString()) + "\" alt=\"load\"/><br /><a href=\"" + Utils.getUrl("" + n.Files + "") + "\">&gt;原图下载</a>.<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + n.PrevFiles + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">编辑</a>");
                        }
                        else
                        {
                            builder.Append("[格式:" + n.FileExt.Replace(".", "") + "/大小:" + BCW.Files.FileTool.GetContentLength(n.FileSize) + " " + n.Content + "]<br />");
                            builder.Append("<a href=\"" + Utils.getUrl("demore.aspx?act=down&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;免费下载到手机</a>");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));

                    }
                    //分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    builder.Append(Out.Tab("", "<br />"));
                }
            }

        }

        if (model.Types == 11)
        {
            //手工分页设置
            if (model.Content.IndexOf("##") != -1)
            {
                pageSize = 0;
            }
            string content = BasePage.MultiContent(model.Content, pageIndex, pageSize, pover, out recordCount);
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(Out.SysUBB(content.Replace("查看大图更多热图", "~摘自新华网~")).Replace("<br/>     <br/>", "<br />"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(BasePage.MultiContentPage(model.Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));

            builder.Append(Out.Tab("", "<br />"));
        }
        if (ub.GetSub("FtGNset", xmlPath) == "0")
        {
            builder.Append(Out.Tab("<div>", ""));
            if (model.Types == 11)
            {
                builder.Append("<a href=\"" + Utils.getUrl("demore.aspx?act=downtext&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">功能箱</a>.");
            }
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/favorites.aspx?act=addin&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + Utils.PostPage(1) + "") + "\">分享.举报</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("FtCKset", xmlPath) == "0")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=评论回复=");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //快速评论
        string ReText = ub.GetSub("FtKCset", xmlPath);
        if (ReText != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            ReText = ReText.Replace("#", "|");
            string ReStats = model.ReStats;
            if (string.IsNullOrEmpty(ReStats))
                ReStats = ReText;
            string sStats = string.Empty;
            //修改用户点击
            /*
            3*/
            if (v != 0)//点击支持反对路过选项进入
            {
                int myid = new BCW.User.Users().GetUsId();
                if (myid == 0)
                    Utils.Login();  //需要登录
               // builder.Append("clickid:" + model.ClickID + "<br/>");
                if (!("#" + model.ClickID + "#").Contains("#" + myid + "#"))//用户未点击过
                { 
                    string[] arrReStats = ReStats.Split("|".ToCharArray());
                    for (int i = 0; i < arrReStats.Length; i++)
                    {
                        if ((v - 1) == i)
                        {
                            sStats += "|" + Convert.ToInt32(Utils.ParseInt(arrReStats[i]) + 1);
                        }
                        else
                        {
                            sStats += "|" + Utils.ParseInt(arrReStats[i]);
                        }
                    }
                    sStats = Utils.Mid(sStats, 1, sStats.Length);
                    new BCW.BLL.Detail().UpdateReStats(id, sStats, Utils.GetUsIP());
                    string click = model.ClickID;
                    string saveid = string.Empty ;
                    if (string.IsNullOrEmpty(click))
                    {
                        saveid = saveid + myid;
                    }
                    else
                    {
                        saveid = myid + "#" + click;
                    }
                //    builder.Append(saveid+"<br/>");
                    new BCW.BLL.Detail().UpdateClickID(id, saveid);
                }
                else
                {
                    string[] arrReStats = ReStats.Split("|".ToCharArray());
                    for (int i = 0; i < arrReStats.Length; i++)
                    {                    
                        if (string.IsNullOrEmpty(sStats))
                        {
                            sStats += arrReStats[i];
                        }
                        else
                        {
                            sStats += "|" + Utils.ParseInt(arrReStats[i]);
                        }

                    }
                }
            }
            else
            {
                sStats = ReStats;
            }
            string[] arrText = ReText.Split("|".ToCharArray());
            string[] arrsStats = sStats.Split("|".ToCharArray());
            for (int i = 0; i < arrText.Length; i++)
            {
                string aStats = string.Empty;
                if (arrsStats.Length <= i)
                    aStats = "0";
                else
                    aStats = Utils.ParseInt(arrsStats[i].ToString()).ToString();

                builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;v=" + (i + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + Out.BasUBB(arrText[i].ToString()) + "(" + aStats + ")</a> ");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //更新人气点击
        new BCW.BLL.Detail().UpdateReadcount(id, 1);

        if (ub.GetSub("FtCKset", xmlPath) == "0")
        {
            string strText = ",,,";
            string strName = "Content,id,act,backurl";
            string strType = "text,hidden,hidden,hidden";
            string strValu = "'" + id + "'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,,,";
            string strIdea = "/";
            string strOthe = "发表评论,comment.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            DataSet ds = new BCW.BLL.Comment().GetList(id, 3, 0);
            builder.Append(Out.Tab("<div>", "<br />"));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.AppendFormat("{0}.{1}:{2}({3})", (i + 1), ds.Tables[0].Rows[i]["UserName"].ToString(), ds.Tables[0].Rows[i]["Content"].ToString(), DT.FormatDate(Convert.ToDateTime(ds.Tables[0].Rows[i]["AddTime"]), 1));
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ReText"].ToString()))
                    {
                        builder.Append(Out.Tab("<font color=\"red\">", ""));
                        builder.Append("<br />★管理员回复:" + ds.Tables[0].Rows[i]["ReText"].ToString() + "");
                        builder.Append(Out.Tab("</font>", ""));
                    }
                    builder.Append("<br />");
                }
            }
            builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看更多评论(" + model.Recount + "条)</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        // 下条
        builder.Append(Out.Tab("<div>", ""));
        BCW.Model.Detail x = new BCW.BLL.Detail().GetPreviousNextDetail(id, model.NodeId, true);
        if (!string.IsNullOrEmpty(x.Title))
        {
            builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + x.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一条:" + x.Title + "</a><br />");
        }
        // 上条
        BCW.Model.Detail s = new BCW.BLL.Detail().GetPreviousNextDetail(id, model.NodeId, false);
        if (!string.IsNullOrEmpty(s.Title))
        {
            builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + s.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上一条:" + s.Title + "</a><br />");
        }
        builder.Append(Out.Tab("</div>", ""));

        ////根据关键字取相关记录
        //if (model.Types == 11)
        //{
        //    string wheTabs = string.Empty;
        //    if (!string.IsNullOrEmpty(model.KeyWord))
        //    {
        //        string[] wheTab;
        //        wheTab = model.KeyWord.Split("#".ToCharArray());
        //        for (int i = 0; i < wheTab.Length; i++)
        //        {
        //            if (i > 0)
        //            {
        //                wheTabs = wheTabs + " OR ";
        //            }
        //            wheTabs = wheTabs + "'#'+KeyWord+'#' like '%#" + wheTab[i] + "#%'";
        //        }
        //    }
        //    ds = new BCW.BLL.Detail().GetList("ID,Title", "NodeId=" + model.NodeId + " AND Types=" + model.Types + " AND ID<>" + id + " AND (" + wheTabs + ") ORDER BY ID DESC");
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        builder.Append(Out.Tab("<div>", ""));
        //        builder.Append("=相关文章=");
        //        builder.Append(Out.Tab("</div>", ""));
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            builder.Append(Out.Tab("<div>", "<br />"));
        //            builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + ds.Tables[0].Rows[i]["ID"].ToString() + "") + "\">" + ds.Tables[0].Rows[i]["Title"].ToString() + "</a>");
        //            builder.Append(Out.Tab("</div>", ""));
        //        }
        //        if (ds.Tables[0].Rows.Count > 3)
        //        {
        //            builder.Append(Out.Tab("<div>", "<br />"));
        //            builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "") + "\">..更多相关(" + ds.Tables[0].Rows.Count + ")</a>");
        //            builder.Append(Out.Tab("</div>", ""));
        //        }
        //    }
        //}

        //底部调用
        string FootUbb = string.Empty;
        if (model.Types == 11)
        {
            FootUbb = ub.GetSub("FtTextDetailFoot", xmlPath);
        }
        else if (model.Types == 12)
        {
            FootUbb = ub.GetSub("FtPicDetailFoot", xmlPath);
        }
        else if (model.Types == 13)
        {
            FootUbb = ub.GetSub("FtFileDetailFoot", xmlPath);
        }
        if (FootUbb != "")
        {
            FootUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(FootUbb));
            if (FootUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(FootUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(TopUbb);
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("list.aspx?id=" + model.NodeId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("list.aspx?id=" + model.NodeId + "") + "\">" + new BCW.BLL.Topics().GetTitle(model.NodeId) + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
}