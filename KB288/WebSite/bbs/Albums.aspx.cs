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
using BCW.Files;

public partial class bbs_Albums : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/albums.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("AlbumsStatus", xmlPath) == "1")
        {
            Utils.Safe("相册系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        if (uid > 0 && act == "")
        {
            act = "me";
        }
        switch (act)
        {
            case "me":
                MePage(uid, leibie);
                break;
            case "list":
                ListPage(uid, leibie);
                break;
            case "newlist":
                NewListPage();
                break;
            case "view":
                ViewPage(leibie);
                break;
            case "add":
                AddPage(leibie);
                break;
            case "albumsadm":
                AlbumsAdminPage(leibie);
                break;
            case "cover":
                CoverPage(leibie);
                break;
            case "photo":
                PhotoPage(leibie);
                break;
            case "move":
                MovePage(leibie);
                break;
            case "edit":
                EditPage(leibie);
                break;
            case "del":
                DelPage(leibie);
                break;
            case "delcomment":
                DelCommentPage(leibie);
                break;
            case "admin":
                AdminPage(leibie);
                break;
            case "group":
                GroupPage(leibie);
                break;
            case "addgroup":
                AddGroupPage(leibie);
                break;
            case "groupsave":
                GroupSavePage(leibie);
                break;
            case "editgroup":
                EditGroupPage(leibie);
                break;
            case "editgroupsave":
                EditGroupSavePage(leibie);
                break;
            case "delgroup":
                DelGroupPage(leibie);
                break;
            case "verify":
                VerifyPage();
                break;
            case "verifypage":
                VerifypagePage();
                break;
            case "down":
                DownPage(leibie);
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("AlbumsName", xmlPath);
        if (ub.GetSub("AlbumsLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("AlbumsLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(13));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(6));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?uid=" + meid + "") + "\">我的相册</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=add&amp;backurl=" + Utils.PostPage(1) + "") + "\">我要上传</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【精品相集】");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = null;
        int TopNum = Convert.ToInt32(ub.GetSub("AlbumsTopNum", xmlPath));
        int NewNum = Convert.ToInt32(ub.GetSub("AlbumsNewNum", xmlPath));
        //随机一张图片
        ds = new BCW.BLL.Upfile().GetList("TOP 1 ID,PrevFiles", "Types=1 and ((Select b.Types from tb_Upgroup b where b.ID=NodeId)=0 OR NodeId=0) and ForumId=0 and IsVerify=0  ORDER BY NewID()");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            string PrevFiles = ds.Tables[0].Rows[0]["PrevFiles"].ToString();
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=1&amp;&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + PrevFiles + "\" alt=\"load\"/></a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //相集分类
        ds = new BCW.BLL.Upgroup().GetList("TOP " + TopNum + " ID,UsID,Title", "Leibie=1 and Types=0 and Node IS NOT NULL ORDER BY NewID()");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int uid = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Title"].ToString();
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=list&amp;leibie=1&amp;uid=" + uid + "&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx") + "\">&gt;&gt;更多精品相集</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //最新上传相片
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【最新上传】");
        builder.Append(Out.Tab("</div>", "<br />"));
        ds = new BCW.BLL.Upfile().GetList("TOP " + NewNum + " ID,Content,AddTime", "Types=1 and ((Select b.Types from tb_Upgroup b where b.ID=NodeId)=0 OR NodeId=0) and ForumId=0  and IsVerify=0 ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Content"].ToString();
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                if (string.IsNullOrEmpty(Title))
                    Title = "无标题";

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=1&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>(" + DT.FormatDate(AddTime, 9) + ")");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=newlist") + "\">&gt;&gt;更多相片</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">日记</a>");
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid))
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("albums.aspx?act=verify") + "\">=审核相册=</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MePage(int uid, int leibie)
    {
        string UsName = new BCW.BLL.User().GetUsName(uid);
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        if (UsName == "")
        {
            Utils.Error("不存在的会员ID", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(13));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(6));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid == meid)
        {
            new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
            Master.Title = "我的" + strLeibie + "";
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=add&amp;leibie=" + leibie + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">上传" + strLeibie.Replace("册", "片") + "</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=admin&amp;leibie=" + leibie + "") + "\">管理" + strLeibie + "</a>");
        }
        else
        {
            Master.Title = "" + UsName + "的" + strLeibie + "";
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + meid + "") + "\">&gt;&gt;进入我的" + strLeibie + "</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>的" + strLeibie + "=");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 4;
        string strWhere = "";
        string[] pageValUrl = { "leibie", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Leibie=" + leibie + " and UsID=" + uid + "";

        // 开始读取列表
        IList<BCW.Model.Upgroup> listUpgroup = new BCW.BLL.Upgroup().GetUpgroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpgroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Upgroup n in listUpgroup)
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
                string aText = string.Empty;

                if (leibie == 1)
                {

                    if (!string.IsNullOrEmpty(n.Node) && meid == uid)
                    {
                        aText = "<img src=\"" + n.Node + "\" alt=\"load\"/>";
                    }
                    else
                    {
                        if (n.Types == 0)
                            aText = "<img src=\"/Files/sys/Albums/public.gif\" alt=\"load\"/>";
                        else if (n.Types == 1)
                            aText = "<img src=\"/Files/sys/Albums/friend.gif\" alt=\"load\"/>";
                        else if (n.Types == 2)
                            aText = "<img src=\"/Files/sys/Albums/private.gif\" alt=\"load\"/>";

                    }

                    builder.Append("" + aText + "<br /><a href=\"" + Utils.getUrl("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + uid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Upfile().GetCount(uid, n.ID) + "张)</a>");
                }
                else
                {
                    aText = "(公)";
                    if (n.Types == 1)
                        aText = "(友)";
                    else if (n.Types == 2)
                        aText = "(私)";
                    builder.Append("" + aText + "<a href=\"" + Utils.getUrl("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + uid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + new BCW.BLL.Upfile().GetCount(uid, n.ID) + ")");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            if (leibie == 1)
                builder.Append(Out.Div("div", "还没有添加相集.."));
            else
                builder.Append(Out.Div("div", "还没有添加分组.."));
        }


        builder.Append(Out.Tab("<div>", "<br />"));
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + uid + "&amp;id=0&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;未归类" + Title + "(" + new BCW.BLL.Upfile().GetCount(uid, leibie, 0) + ")</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (meid == uid)
            builder.Append("我的:");
        else
            builder.Append("主人:");

        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + uid + "") + "\">日记</a>");
        if (leibie != 1)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=1&amp;uid=" + uid + "") + "\">相册</a>");
        if (leibie != 2)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=2&amp;uid=" + uid + "") + "\">音乐</a>");
        if (leibie != 3)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=3&amp;uid=" + uid + "") + "\">视频</a>");
        if (leibie != 4)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=4&amp;uid=" + uid + "") + "\">资源</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("albums.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage(int uid, int leibie)
    {
        string strTitle = "文件夹";
        if (leibie == 1)
            strTitle = "相集";

        int meid = new BCW.User.Users().GetUsId();
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
        {
            Utils.Error("不存在的会员ID", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "-1"));
        string Title = string.Empty;
        if (id > 0)
        {
            Title = new BCW.BLL.Upgroup().GetTitle(id, uid);
            if (Title == "")
            {
                Utils.Error("不存在的" + strTitle + "", "");
            }
        }
        else if (id == 0)
        {
            Title = "未归类" + strTitle + "";
        }
        else
        {
            Title = "所有" + BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片") + "";

        }
        Master.Title = "" + UsName + "的" + BCW.User.AppCase.CaseAlbums(leibie) + "";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(13));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(6));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + uid + "") + "\">" + UsName + "的" + BCW.User.AppCase.CaseAlbums(leibie) + "</a>&gt;" + Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 3;
        if (leibie > 1)
            pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        string strWhere = "";
        string[] pageValUrl = { "act", "leibie", "uid", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        //是否为好友/管理员
        bool Isfri = true;
        string strWhe = string.Empty;
        if (meid != uid)
        {
            Isfri = new BCW.BLL.Friend().Exists(uid, meid, 0);
            if (Isfri == false)
            {
                Isfri = new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid);
            }
            //相关限制性
            if (Isfri == true)
            {
                strWhe = " and (Select b.Types from tb_Upgroup b where b.ID=NodeId)<=1 ";
            }
            else
            {
                strWhe = " and (Select b.Types from tb_Upgroup b where b.ID=NodeId)=0 ";
            }
        }
        if (id == 0)
        {
            strWhere = "Types=" + leibie + " and UsID=" + uid + " and NodeId=" + id + "";
        }
        else
        {
            if (id != -1)
                strWhere = "Types=" + leibie + " " + strWhe + " and UsID=" + uid + " and NodeId=" + id + "";
            else
                strWhere = "Types=" + leibie + "  " + strWhe + " and UsID=" + uid + "";
        }
        // 开始读取列表
        IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpfile.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Upfile n in listUpfile)
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

                if (n.Types == 1)
                {
                    if (n.IsVerify == 1)
                        builder.Append("<img src=\"/Files/sys/Albums/check.gif\" alt=\"load\"/><br />");
                    else
                        builder.Append("<img src=\"" + n.PrevFiles + "\" alt=\"load\"/><br />");
                }
                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                //string Content = n.Content;
                //if (string.IsNullOrEmpty(Content))
                //    Content = "无标题";
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">详细.</a>");
                if (leibie > 1)
                {
                    builder.Append("(下载" + n.DownNum + ")");
                }
                if (n.UsID.Equals(meid))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=photo&amp;leibie=" + leibie + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">设头像.</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">管理</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl(n.Files) + "\">下载</a>");
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

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (meid == uid)
            builder.Append("我的:");
        else
            builder.Append("主人:");

        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + uid + "") + "\">日记</a>");
        if (leibie != 1)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=1&amp;uid=" + uid + "") + "\">相册</a>");
        if (leibie != 2)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=2&amp;uid=" + uid + "") + "\">音乐</a>");
        if (leibie != 3)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=3&amp;uid=" + uid + "") + "\">视频</a>");
        if (leibie != 4)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=4&amp;uid=" + uid + "") + "\">资源</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + uid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void NewListPage()
    {
        Master.Title = "最新相片";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(13));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(6));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("更多最新相片");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 3;
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "Types=1";
        // 开始读取列表
        IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpfile.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Upfile n in listUpfile)
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
                builder.Append("<img src=\"" + n.PrevFiles + "\" alt=\"load\"/><br />");
                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                //string Content = n.Content;
                //if (string.IsNullOrEmpty(Content))
                //    Content = "无标题";
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=1&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">详细.</a>");
                builder.Append("<a href=\"" + Utils.getUrl(n.Files) + "\">下载</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx") + "\">相册首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void ViewPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片");
        int meid = new BCW.User.Users().GetUsId();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, leibie);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "" + new BCW.BLL.User().GetUsName(model.UsID) + "的" + strLeibie;
        //查看权限(1好友可见/2私人)
        if (model.UsID != meid)
        {

            bool Isfri = new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid);
            if (Isfri == false)
            {
                int Types = new BCW.BLL.Upgroup().GetTypes(model.NodeId);
                if (Types == 2)
                {
                    Utils.Error("此" + strLeibie + "属<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(model.UsID) + "</a>私人" + strLeibie + "..", "");
                }
                else if (Types == 1)
                {
                    if (!new BCW.BLL.Friend().Exists(model.UsID, meid, 0))
                    {
                        Utils.Error("此" + strLeibie + "仅限<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(model.UsID) + "</a>的好友才可以查看..", "");
                    }
                }
            }
        }

        //更新相片浏览次数
        if (leibie == 1)
        {
            new BCW.BLL.Upfile().UpdateDownNum(id, 1);
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(13));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(6));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + model.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(model.UsID) + "的" + BCW.User.AppCase.CaseAlbums(leibie) + "</a>&gt;查看相片");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (model.Types == 1)
        {
            if (model.IsVerify == 1)
                builder.Append("<img src=\"/Files/sys/Albums/check.gif\" alt=\"load\"/>");
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + model.PrevFiles + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + model.PrevFiles + "\" alt=\"load\"/></a><br />");
                builder.Append("<a href=\"" + model.Files + "\">原图下载(" + BCW.Files.FileTool.GetContentLength(model.FileSize) + ")</a>");
            }
        }
        else
        {
            if (model.IsVerify == 1)
                builder.Append("<img src=\"/Files/sys/Albums/check_small.gif\" alt=\"load\"/><br />文件审核中..");
            else
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=down&amp;leibie=" + leibie + "&amp;id=" + model.ID + "") + "\">免费下载(" + BCW.Files.FileTool.GetContentLength(model.FileSize) + ")</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上传日期:" + DT.FormatDate(model.AddTime, 0) + "<br />");
        builder.Append("文件描述:" + model.Content + "<br />");
        builder.Append("文件格式:" + model.FileExt + "<br />");
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";
        if (model.NodeId == 0)
            builder.Append("" + Title + ":未归类");
        else
            builder.Append("" + Title + ":" + new BCW.BLL.Upgroup().GetTitle(model.NodeId, model.UsID) + "");

        builder.Append(Out.Tab("</div>", ""));

        if (model.UsID.Equals(meid))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;管理文件</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid))
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=del&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;删除" + strLeibie + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("", "<br />"));

        string strName = "purl,act,backurl";
        string strValu = "'recommend'" + Utils.PostPage(1) + "";
        string strOthe = "&gt;分享给好友,/bbs/guest.aspx,post,1,other";
        builder.Append(Out.wapform(strName, strValu, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        int ReplyNum = 0;
        DataSet ds = new BCW.BLL.FComment().GetList("UsName,Content,AddTime,ReText", "Types=" + leibie + " and DetailId=" + model.ID + " ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            ReplyNum = ds.Tables[0].Rows.Count;
            int k = ReplyNum;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if ((i + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                else
                    builder.Append(Out.Tab("<div>", ""));

                string strUsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                string Content = ds.Tables[0].Rows[i]["Content"].ToString();
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                string ReText = ds.Tables[0].Rows[i]["ReText"].ToString();
                string strContent = Regex.Replace(Utils.Left(Content, 30), @"&(?![\#\w]{2,6};)", "&amp;");
                if (Content.Length > 30)
                    strContent = strContent + "..";

                builder.AppendFormat("{0}楼.{1}:{2}({3})", k, strUsName, Out.SysUBB(strContent), DT.FormatDate(AddTime, 1));
                if (!string.IsNullOrEmpty(ReText))
                {
                    builder.Append(Out.Tab("<font color=\"red\">", ""));
                    builder.Append("<br />★主人回复:" + ReText + "");
                    builder.Append(Out.Tab("</font>", ""));
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                if ((i + 1) >= 3)//显示条数
                    break;
                k = k - 1;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "还没有评论，快抢沙发呀.."));
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("fcomment.aspx?act=replylist&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看评论(" + ReplyNum + "条)</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //得到随机短语
        string ReplyValu = string.Empty;
        ds = new BCW.BLL.Submit().GetList("TOP 1 Content", "UsID=" + meid + " and Types=0 ORDER BY NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            ReplyValu = ds.Tables[0].Rows[0]["Content"].ToString();
        }
        strText = ",,,,,";
        strName = "Content,id,leibie,backurl,act";
        strType = "stext,hidden,hidden,hidden,hidden";
        strValu = "" + ReplyValu + "'" + id + "'" + leibie + "'" + Utils.PostPage(1) + "'replysave";
        strEmpt = "true,false,false,false,false";
        strIdea = "";
        strOthe = "快速评论,fcomment.aspx,post,3,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (meid == model.UsID)
            builder.Append("我的:");
        else
            builder.Append("主人:");

        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + model.UsID + "") + "\">日记</a>");
        if (leibie != 1)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=1&amp;uid=" + model.UsID + "") + "\">相册</a>");
        if (leibie != 2)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=2&amp;uid=" + model.UsID + "") + "\">音乐</a>");
        if (leibie != 3)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=3&amp;uid=" + model.UsID + "") + "\">视频</a>");
        if (leibie != 4)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=4&amp;uid=" + model.UsID + "") + "\">资源</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + model.UsID + "&amp;id=" + model.NodeId + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        Master.Title = "上传" + strLeibie.Replace("册", "片") + "";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + meid + "") + "\">我的" + strLeibie + "</a>&gt;上传" + strLeibie.Replace("册", "片") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请选择上传方式：<br />");
        builder.Append("1.<a href=\"" + Utils.getUrl("addfile.aspx?act=upfile&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "") + "\">手机WAP2.0上传</a><br />");
        builder.Append("1.<a href=\"" + Utils.getUrl("addfilegc2.aspx?act=upfile&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "") + "\">手机WAP2.0上传(低端机)</a><br />");
        builder.Append("2.<a href=\"" + Utils.getUrl("addfile.aspx?act=collec&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "") + "\">输入文件地址上传(限图片)</a><br />");
        builder.Append("3.发彩信上传(未开放)");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("albums.aspx?leibie=" + leibie + "&amp;uid=" + meid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AlbumsAdminPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片");
        Master.Title = "管理" + strLeibie + "";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, leibie);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理" + strLeibie + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (leibie == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=cover&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">设为封面</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=photo&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">设为头像</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=move&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">转移" + strLeibie + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=edit&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">编辑" + strLeibie + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=del&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">删除" + strLeibie + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=delcomment&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">清空评论</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void CoverPage(int leibie)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, 1);
        if (model == null || model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.IsVerify == 1)
        {
            Utils.Error("此相片未通过审核呢", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "设置封面";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将此相片设置成封面吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=cover&amp;info=ok&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定设置</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //得到原图
            string OldFiles = model.Files; ;
            //新图路径
            int NodeId = model.NodeId;
            string NewFiles = "/Files/bbs/cover/" + NodeId + ".jpg";
            //缩放成固定比例
            new BCW.Graph.ImageHelper().ResizeImage(Server.MapPath(OldFiles), Server.MapPath(NewFiles), 112, 116, false);
            //画图片边框
            new BCW.Graph.ImageHelper().BorderImage(Server.MapPath(NewFiles), "", 0);
            new BCW.BLL.Upgroup().UpdateNode(NodeId, NewFiles);
            Utils.Success("设置封面", "设置封面成功，正在返回..<br /><a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + ""), "3");

        }
    }

    private void PhotoPage(int leibie)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, 1);
        if (model == null || model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.IsVerify == 1)
        {
            Utils.Error("此相片未通过审核呢", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "设置头像";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将此相片的设置成头像吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=photo&amp;info=ok&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定设置</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string DirPath = string.Empty;
            string prevDirPath = string.Empty;
            string Path = "/Files/bbs/" + meid + "/tx/";
            string prevPath = Path;
            string SavePath = System.Web.HttpContext.Current.Request.MapPath(model.Files);
            string wh = new BCW.Graph.ImageHelper().GetPicxywh(SavePath, 0);

            wh = wh.Replace("像素", "");
            string[] whTemp = wh.Split('*');
            int w = Utils.ParseInt(whTemp[0]);
            int h = Utils.ParseInt(whTemp[1]);
            if (w > 240 || h > 320)
            {


                //------缩放图片(比例缩放)-----
                int width = 240;
                int height = 320;
                try
                {
                    bool pbool = true;

                    if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                    {
                        string prevSavePath = SavePath.Replace("\\act\\", "\\tx\\");

                        if (prevSavePath.Contains(".gif"))
                        {
                            new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);
                        }
                        else
                        {
                            new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);

                        }
                    }
                    new BCW.BLL.User().UpdatePhoto(meid, model.Files.Replace("/act/", "/tx/"));
                }
                catch (Exception ex)
                {

                    Utils.Error("头像设置失败，头像图片格式也许已损坏，可以重试一下" + ex.Message.ToString() + "", "");
                }
            }
            else
            {
                new BCW.BLL.User().UpdatePhoto(meid, model.Files);
            }


            //动态记录
            new BCW.BLL.Action().Add(meid, "在空间设置了[URL=/bbs/uinfo.aspx?uid=" + meid + "]新的头像[/URL]");
            Utils.Success("设置头像", "设置头像成功，正在返回..<br /><a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">继续管理</a>", Utils.getPage("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + ""), "3");

        }
    }

    private void MovePage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片");
        Master.Title = "管理" + strLeibie + "";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int newid = int.Parse(Utils.GetRequest("newid", "get", 1, @"^[0-9]\d*$", "-1"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, leibie);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (newid != -1)
        {
            new BCW.BLL.Upfile().UpdateNodeId(meid, id, newid);
            Utils.Success("转移" + strLeibie + "", "转移" + strLeibie + "成功，正在返回..<br /><a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + ""), "3");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请选择移动到的分类");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=move&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;newid=0&amp;backurl=" + Utils.getPage(0) + "") + "\">转移到未分类分组</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "leibie", "id" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "Leibie=" + leibie + " and UsID=" + meid + "";

            // 开始读取列表
            IList<BCW.Model.Upgroup> listUpgroup = new BCW.BLL.Upgroup().GetUpgroups(pageIndex, pageSize, strWhere, out recordCount);
            if (listUpgroup.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Upgroup n in listUpgroup)
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

                    builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + n.UsID + "&amp;id=" + n.ID + "") + "\">" + n.Title + "</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=move&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;newid=" + n.ID + "") + "\">[移动]</a>");

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

    }

    private void EditPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片");
        Master.Title = "编辑" + strLeibie + "";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, leibie);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("编辑" + strLeibie + "");
            builder.Append(Out.Tab("</div>", ""));

            strText = "" + strLeibie + "描述:/,,,,,";
            strName = "Content,leibie,id,act,info,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "" + model.Content + "'" + leibie + "'" + id + "'edit'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,albums.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,50}$", "描述最多50字");
            new BCW.BLL.Upfile().Update(id, Content);
            Utils.Success("编辑" + strLeibie + "", "编辑" + strLeibie + "成功，正在返回..<br /><a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">继续管理</a>", Utils.getPage("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + ""), "3");

        }
    }

    private void DelPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, leibie);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, meid))
        {
            if (model.UsID != meid)
            {
                Utils.Error("不存在的记录", "");
            }
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "删除" + strLeibie + "";
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此" + strLeibie + "吗.");
            if (model.BID > 0)
                builder.Append("(论坛附件)");

            builder.Append(Out.Tab("</div>", ""));
            if (model.UsID != meid && model.BID > 0)
            {
                strText = "理由:,,,,,,";
                strName = "Why,leibie,id,act,info,backurl";
                strType = "text,hidden,hidden,hidden,hidden,hidden";
                strValu = "'" + leibie + "'" + id + "'del'ok'" + Utils.getPage(0) + "";
                strEmpt = "true,false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定删除,filelist.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getPage("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=del&amp;info=ok&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            new BCW.BLL.Upfile().Delete(id);
            new BCW.BLL.FComment().Delete(leibie, id);
            //删除文件
            BCW.Files.FileTool.DeleteFile(model.Files);
            if (!string.IsNullOrEmpty(model.PrevFiles))
                BCW.Files.FileTool.DeleteFile(model.PrevFiles);

            //关联帖子回帖减去文件数
            string strRe = string.Empty;
            if (model.ReID > 0)
            {
                new BCW.BLL.Reply().UpdateFileNum(model.ReID, -1);
                strRe = "(回复ID:" + model.ReID + ")";
            }
            else if (model.BID > 0)
            {
                new BCW.BLL.Text().UpdateFileNum(model.BID, -1);
                int FileNum = new BCW.BLL.Text().GetFileNum(model.BID);
                if (FileNum == 0)
                {
                    //去掉附件帖标识
                    new BCW.BLL.Text().UpdateTypes(model.BID, 0);
                }
            }
            //记录日志
            if (model.BID > 0)
            {
                string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
                string strLog = string.Empty;
                if (model.UsID != meid)
                    strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + new BCW.BLL.User().GetUsName(model.UsID) + "[/url]的主题[url=/bbs/topic.aspx?forumid=" + model.ForumID + "&amp;bid=" + model.BID + "]《" + new BCW.BLL.Text().GetTitle(model.BID) + "》[/url]" + strRe + "附件被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]删除!";
                else
                    strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]自删主题[url=/bbs/topic.aspx?forumid=" + model.ForumID + "&amp;bid=" + model.BID + "]《" + new BCW.BLL.Text().GetTitle(model.BID) + "》[/url]的" + strRe + "附件!";

                new BCW.BLL.Forumlog().Add(7, model.ForumID, strLog);
            }
            Utils.Success("删除" + strLeibie + "", "删除" + strLeibie + "成功，正在返回..", Utils.getPage("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + model.UsID + ""), "1");
        }
    }

    private void DelCommentPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id, leibie);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "清空" + strLeibie + "评论";
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空此" + strLeibie + "的评论吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=delcomment&amp;info=ok&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.FComment().Delete(leibie, id);
            Utils.Success("清空" + strLeibie + "评论", "清空" + strLeibie + "评论成功，正在返回..<br /><a href=\"" + Utils.getUrl("albums.aspx?act=albumsadm&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + ""), "3");
        }
    }

    private void AdminPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        Master.Title = "管理" + strLeibie + "";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=add&amp;leibie=" + leibie + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">上传新" + strLeibie.Replace("册", "片") + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=addgroup&amp;leibie=" + leibie + "") + "\">新建" + Title + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=group&amp;leibie=" + leibie + "") + "\">编辑" + Title + "</a><br />");
        builder.Append("设定皮肤");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + meid + "") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=" + leibie + "&amp;uid=" + meid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddGroupPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        Master.Title = "添加" + Title + "";

        builder.Append(Out.Tab("<div class=\"title\">添加" + Title + "</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + Title + "名称:");
        builder.Append(Out.Tab("</div>", ""));

        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        if (leibie == 1)
        {
            sText = ",相集分类:/";
            sName = ",PostType";
            sType = ",select";
            sValu = "'1";
            sEmpt = ",1|生活休闲|2|自拍自秀|3|人物明星|4|动物生物|5|旅游名胜|6|搞笑幽默|7|鲜花植物";
        }
        strText = "," + Title + "性质:/" + sText + ",允许评论:/,排序:/,,,,";
        strName = "Title,Types" + sName + ",IsReview,Paixu,leibie,act,backurl";
        strType = "text,select" + sType + ",select,snum,hidden,hidden,hidden";
        strValu = "'0" + sValu + "'0'0'" + leibie + "'groupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,0|对外公开|1|好友可见|2|个人隐私" + sEmpt + ",0|允许|1|不允许,false,false,false,false";
        strIdea = "/";
        strOthe = "添加" + Title + ",albums.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=group&amp;leibie=" + leibie + "&amp;uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=admin&amp;leibie=" + leibie + "") + "\">" + strLeibie + "管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupSavePage(int leibie)
    {
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        string sTitle = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "" + Title + "名称限1-10字，不能使用特殊字符");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]$", "" + Title + "性质选择错误"));
        int PostType = int.Parse(Utils.GetRequest("PostType", "post", 1, @"^[1-7]$", "0"));
        int IsReview = int.Parse(Utils.GetRequest("IsReview", "post", 2, @"^[0-1]$", "允许评论选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));
        if (leibie == 1)
        {
            if (PostType == 0)
            {
                Utils.Error("相集分类选择错误", "");
            }
        }
        if (new BCW.BLL.Upgroup().ExistsTitle(meid, Title, leibie))
        {
            Utils.Error("此" + Title + "名称已存在", "");
        }
        BCW.Model.Upgroup model = new BCW.Model.Upgroup();
        model.Leibie = leibie;
        model.Types = Types;
        model.PostType = PostType;
        model.Title = sTitle;
        model.UsID = meid;
        model.IsReview = IsReview;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Upgroup().Add(model);

        Utils.Success("添加" + Title + "", "添加" + Title + "成功，正在返回..", Utils.getUrl("albums.aspx?act=group&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void GroupPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        Master.Title = "" + Title + "管理";
        builder.Append(Out.Tab("<div class=\"title\">" + Title + "管理</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=addgroup&amp;leibie=" + leibie + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">新建" + Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "leibie", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Leibie=" + leibie + " and UsID=" + meid + "";

        // 开始读取列表
        IList<BCW.Model.Upgroup> listUpgroup = new BCW.BLL.Upgroup().GetUpgroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpgroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Upgroup n in listUpgroup)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string aText = "(公)";
                if (n.Types == 1)
                    aText = "(友)";
                else if (n.Types == 2)
                    aText = "(私)";

                builder.Append("" + aText + "<a href=\"" + Utils.getUrl("albums.aspx?act=list&amp;leibie=" + leibie + "&amp;uid=" + n.UsID + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Upfile().GetCount(meid, n.ID) + ")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=editgroup&amp;leibie=" + leibie + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[编辑]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=delgroup&amp;leibie=" + leibie + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=admin&amp;leibie=" + leibie + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=admin&amp;leibie=" + leibie + "") + "\">" + strLeibie + "管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditGroupPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        Master.Title = "编辑" + Title + "";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "" + Title + "ID错误"));
        if (!new BCW.BLL.Upgroup().Exists(id, meid, leibie))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Upgroup model = new BCW.BLL.Upgroup().GetUpgroup(id);
        builder.Append(Out.Tab("<div class=\"title\">编辑" + Title + "</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + Title + "名称:");
        builder.Append(Out.Tab("</div>", ""));
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        if (leibie == 1)
        {
            sText = ",相集分类:/";
            sName = ",PostType";
            sType = ",select";
            sValu = "'" + model.PostType + "";
            sEmpt = ",1|生活休闲|2|自拍自秀|3|人物明星|4|动物生物|5|旅游名胜|6|搞笑幽默|7|鲜花植物";
        }
        strText = "," + Title + "性质:/" + sText + ",允许评论:/,排序:/,,,,";
        strName = "Title,Types" + sName + ",IsReview,Paixu,id,leibie,act,backurl";
        strType = "text,select" + sType + ",select,snum,hidden,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.Types + "" + sValu + "'" + model.IsReview + "'" + model.Paixu + "'" + id + "'" + leibie + "'editgroupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,0|对外公开|1|好友可见|2|个人隐私" + sEmpt + ",0|允许|1|不允许,false,false,false,false,false";
        strIdea = "/";
        strOthe = "编辑" + Title + ",albums.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=group&amp;leibie=" + leibie + "&amp;uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=admin&amp;leibie=" + leibie + "") + "\">" + strLeibie + "管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditGroupSavePage(int leibie)
    {
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string sTitle = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "" + Title + "名称限1-10字，不能使用特殊字符");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]\d*$", "" + Title + "性质选择错误"));
        int PostType = int.Parse(Utils.GetRequest("PostType", "post", 1, @"^[1-7]\d*$", "0"));
        int IsReview = int.Parse(Utils.GetRequest("IsReview", "post", 2, @"^[0-1]$", "允许评论选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));
        if (leibie == 1)
        {
            if (PostType == 0)
            {
                Utils.Error("相集分类选择错误", "");
            }
        }
        if (!new BCW.BLL.Upgroup().Exists(id, meid, leibie))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Upgroup model = new BCW.Model.Upgroup();
        model.ID = id;
        model.Types = Types;
        model.PostType = PostType;
        model.Title = sTitle;
        model.UsID = meid;
        model.IsReview = IsReview;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Upgroup().Update(model);

        Utils.Success("编辑" + Title + "", "编辑" + Title + "成功，正在返回..", Utils.getUrl("albums.aspx?act=group&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelGroupPage(int leibie)
    {
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        string Title = "文件夹";
        if (leibie == 1)
            Title = "相集";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Albums, meid);//相册权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Upgroup().Exists(id, meid, leibie))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除" + Title + "";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此" + Title + "吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=delgroup&amp;info=ok&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=group&amp;leibie=" + leibie + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示：删除后本组" + strLeibie.Replace("册", "片") + "将移到未归类" + Title + ".");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Upgroup().Delete(id);
            new BCW.BLL.Upfile().UpdateNodeIds(meid, leibie, id);

            Utils.Success("删除" + Title + "", "删除" + Title + "成功，正在返回..", Utils.getUrl("albums.aspx?act=group&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void VerifyPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Albums, meid);

        Master.Title = "未审核的文件";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
            builder.Append("图片文件|<a href=\"" + Utils.getUrl("albums.aspx?act=verify&amp;ptype=2") + "\">其它文件</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=verify&amp;ptype=1") + "\">图片文件</a>|其它文件");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 3;
        if (ptype > 1)
            pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1)
            strWhere = "Types=1 and IsVerify=1";
        else
            strWhere = "Types>1 and IsVerify=1";

        // 开始读取列表
        IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpfile.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Upfile n in listUpfile)
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

                if (n.Types == 1)
                {
                    builder.Append("<img src=\"" + n.PrevFiles + "\" alt=\"load\"/><br />");
                }
                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                string Content = n.Content;
                if (string.IsNullOrEmpty(Content))
                    Content = "无标题";

                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=down&amp;leibie=" + n.Types + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Content + "(" + n.FileExt + "/" + BCW.Files.FileTool.GetContentLength(n.FileSize) + ")</a>");

                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=del&amp;leibie=" + n.Types + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=verifypage&amp;ptype=" + ptype + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核本页文件</a><br />");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void VerifypagePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Albums, meid);

        Master.Title = "审核本页文件";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-2]\d*$", "类型错误"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定审核本页文件吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=verifypage&amp;info=ok&amp;ptype=" + ptype + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定审核</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=verify&amp;ptype=" + ptype + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=verify") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = 3;
            if (ptype > 1)
                pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

            string strWhere = "";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;

            if (ptype == 1)
                strWhere = "Types=1 and IsVerify=1";
            else
                strWhere = "Types>1 and IsVerify=1";

            // 开始读取列表
            IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
            if (listUpfile.Count > 0)
            {
                foreach (BCW.Model.Upfile n in listUpfile)
                {
                    new BCW.BLL.Upfile().UpdateIsVerify(n.ID, 0);
                }
            }

            Utils.Success("审核本页文件", "审核本页文件成功，正在返回..", Utils.getUrl("albums.aspx?act=verify&amp;ptype=" + ptype + ""), "1");
        }
    }


    private void DownPage(int leibie)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Upfile().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info == "")
        {
            Master.Title = "下载" + BCW.User.AppCase.CaseAlbums(leibie).Replace("相册", "图片") + "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定下载此" + BCW.User.AppCase.CaseAlbums(leibie).Replace("相册", "图片") + "吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=down&amp;info=ok&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定下载</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string FileName = new BCW.BLL.Upfile().GetFiles(id);
            //更新下载次数
            new BCW.BLL.Upfile().UpdateDownNum(id, 1);
            BCW.User.Down.ShowMsg(FileName);
        }
    }
}