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
using BCW.Service;
using BCW.Files;

/// <summary>
/// 修改回帖显示 陈志基 20160708
/// 
/// 修改结贴禁止显示添加下一期 2016 0421
/// 黄国军
/// 
/// 修改人陈志基 2016 0421
/// 修改表情点赞图片大小
/// </summary>

public partial class bbs_topic : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        int meid = new BCW.User.Users().GetUsId(); //得到会员ID        
        BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(forumid);
        //论坛限制性
        BCW.User.Users.ShowForumLimit(meid, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);

        string getTitle = model.Title;
        getTitle = Regex.Replace(getTitle, @"\[[\/红橙绿青蓝紫]+?\]", "");
        Master.Title = getTitle;



        //================================搜狐新闻入库2015-4-15
        if (model.HideType == 9)
        {
            if (model.Content.Contains("sohu.com"))
            {
                //builder.Append(model.Content);

                BCW.Collec.Collec Cn = new BCW.Collec.Collec();
                string txt = "";
                string NewsNextUrl = model.Content;
                while (NewsNextUrl.Length > 0)
                {
                    string str2 = new BCW.Service.GetNews().GetNewsXML2(NewsNextUrl);
                    string str = str2;

                    //取图片
                    if (model.Istxt == 0)
                    {
                        string photoUrl = "";
                        string Pic = "/files/Cache/pic/";
                        string WebUrl = "http://photocdn.sohu.com";
                        ArrayList bodyArray = Cn.ReplaceSaveRemoteFile(3, str, Pic, WebUrl, "1");
                        if (bodyArray.Count == 2)
                        {
                            photoUrl = bodyArray[1].ToString();

                            int width = Convert.ToInt32(ub.GetSub("UpaWidth", "/Controls/upfile.xml"));
                            int height = Convert.ToInt32(ub.GetSub("UpaHeight", "/Controls/upfile.xml"));

                            string[] fTemp = Regex.Split(photoUrl, "#");
                            int kk = 0;
                            for (int i = 0; i < fTemp.Length; i++)
                            {
                                BCW.Model.Upfile modelf = new BCW.Model.Upfile();
                                modelf.FileExt = System.IO.Path.GetExtension(fTemp[i]).ToLower();

                                string rep = modelf.FileExt.Replace(".", "s.");
                                string prevPath = System.Web.HttpContext.Current.Request.MapPath(fTemp[i].Replace(modelf.FileExt, rep));
                                try
                                {
                                    new BCW.Graph.ImageHelper().ResizeImage(System.Web.HttpContext.Current.Request.MapPath(fTemp[i]), prevPath, width, height, true);
                                    modelf.PrevFiles = fTemp[i].Replace(modelf.FileExt, rep);
                                }
                                catch
                                {
                                    modelf.PrevFiles = fTemp[i];
                                }
                                modelf.Types = 1;
                                modelf.NodeId = 0;
                                modelf.UsID = model.UsID;
                                modelf.ForumID = forumid;
                                modelf.BID = bid;
                                modelf.ReID = 0;
                                modelf.Files = fTemp[i];
                                modelf.Content = "";
                                modelf.FileSize = BCW.Files.FileTool.GetFileLength(fTemp[i]);
                                modelf.DownNum = 0;
                                modelf.Cent = 0;
                                modelf.IsVerify = 0;
                                modelf.AddTime = DateTime.Now;
                                new BCW.BLL.Upfile().Add(modelf);
                                kk++;

                                //builder.Append(fTemp[i] + "<br />");
                            }
                            //更新帖子文件数
                            new BCW.BLL.Text().UpdateFileNum(bid, kk);
                            model.FileNum = kk;
                        }
                        //更新已更新标识
                        new BCW.BLL.Text().UpdateIstxt(bid, 1);

                        //builder.Append(photoUrl);
                    }



                    str = str.Replace("</p>\r\n<p>", "[br]");
                    str = Regex.Replace(str, @"<SCRIPT>[\s\S]+?</SCRIPT>", "");
                    str = Regex.Replace(str, @"<script>[\s\S]+?</script>", "");
                    str = Regex.Replace(str, @"<div class=""sele-now top-pager-current"">[\s\S]+?</div>", "");
                    str = Regex.Replace(str, @"<div class=""sele-con top-pager-options"" style=""display:none"">[\s\S]+?</div>", "");
                    str = Regex.Replace(str, @"<div class=""pages"">[\s\S]+?</div>", "");

                    //去掉闭合标签br除外
                    str = Utils.GetTextFromHTML2(str).Replace("// ", "");
                    str = str.Replace("<br>", "<br/>");
                    //去掉重复<br/>
                    str = Regex.Replace(str, @"(<br\s/>){2,}", "<br />");

                    //去掉空白行
                    str = Regex.Replace(str, @"(\r\n){2,}", "");
                    //去掉空格、TAB
                    str = Regex.Replace(str, @"(\s){2,}", "");
                    //去掉垃圾
                    str = Regex.Replace(str, @"&[0-9\w]+;", "");
                    str = Regex.Replace(str, @"&#[0-9\w]+;", "");
                    str = Regex.Replace(str, @"<br />", "[br]");
                    str = Regex.Replace(str, @"<br/>", "[br]");
                    txt += str;

                    string NextPageRegex = @"(?:<a class=""pages-wd"" href=""[\s\S]+?"">上一页</a>[\s\S]+?)?<a class=""pages-wd"" href=""([\s\S]+?)"">下一页</a>";
                    NewsNextUrl = Cn.GetRegValue(NextPageRegex, str2);
                    //builder.Append(NewsNextUrl + "<br />");

                }
                model.Content = txt;



            }
        }
        //================================搜狐新闻入库2015-4-15


        //圈子限制性
        BCW.Model.Group modelgr = null;
        if (Forummodel.GroupId > 0)
        {
            modelgr = new BCW.BLL.Group().GetGroupMe(Forummodel.GroupId);
            if (modelgr == null)
            {
                Utils.Error("不存在的" + ub.GetSub("GroupName", "/Controls/group.xml") + "", "");
            }
            else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "已过期", "");
            }
            if (modelgr.ForumStatus == 2)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛已关闭", "");
            }
            if (modelgr.ForumStatus == 1)
            {
                if (meid == 0)
                    Utils.Login(model.Title);

                string GroupId = new BCW.BLL.User().GetGroupId(meid);
                if (GroupId.IndexOf("#" + Forummodel.GroupId + "#") == -1 && IsCTID(meid) == false)
                {
                    Utils.Error("非成员不能访问" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛！<br /><a href=\"" + Utils.getUrl("/bbs/group.aspx?act=addin&amp;id=" + Forummodel.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", "/Controls/group.xml") + "</a>", "");
                }
            }
        }

        //个性设置
        int FsPageSize = 500;
        int FsIsImg = 0;
        int FsIsReList = 0;
        int FsReList = 5;
        if (meid > 0)
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(meid);
            FsPageSize = BCW.User.Users.GetForumSet(ForumSet, 1);
            FsIsImg = BCW.User.Users.GetForumSet(ForumSet, 6);
            FsIsReList = BCW.User.Users.GetForumSet(ForumSet, 4);
            FsReList = BCW.User.Users.GetForumSet(ForumSet, 5);
        }
        if (model.Types == 4 && meid != model.UsID)
        {
            if (model.IsSeen == 1)
            {
                if (meid == 0)
                    Utils.Login(model.Title);
            }
            else if (model.IsSeen == 2)
            {
                if (!Utils.IsMobileUa())
                {
                    Utils.Error("请使用手机访问本帖", "");
                }
            }
            //等级限制访问帖子
            if (model.Price > 0)
            {
                if (meid == 0)
                    Utils.Login(model.Title);
                int Leven = new BCW.BLL.User().GetLeven(meid);
                if (Leven < model.Price)
                {
                    Utils.Error("等级必须达到" + model.Price + "级才能浏览本帖子", "");
                }
            }
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(1));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(3));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx") + "\">论坛</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + Forummodel.Title + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">看回复</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //心情图标
        if (!string.IsNullOrEmpty(model.ReStats))
        {
            string[] arrsStats = model.ReStats.Split("|".ToCharArray());
            int[] x = new int[arrsStats.Length];

            for (int i = 0; i < arrsStats.Length; i++)
            {
                x[i] = int.Parse(arrsStats[i]);
            }
            int max = x[0];
            int index = 0;
            for (int i = 1; i < x.Length; i++)
            {
                if (max < x[i])
                {
                    max = x[i];
                    index = i;
                }
            }
            if (int.Parse(arrsStats[index]) > Utils.ParseInt(ub.GetSub("BbsMood", xmlPath)))  //大于5就显示图片
            {
                builder.Append("<img src=\"/files/face/em" + index + ".gif\" alt=\"load\"/>");
            }
        }
        builder.Append("主题:");
        builder.Append(BCW.User.AppCase.CaseIsTop(model.IsTop));
        builder.Append(BCW.User.AppCase.CaseIsGood(model.IsGood));
        builder.Append(BCW.User.AppCase.CaseIsRecom(model.IsRecom));
        builder.Append(BCW.User.AppCase.CaseIsLock(model.IsLock));
        builder.Append(BCW.User.AppCase.CaseIsOver(model.IsOver));
        if (model.Types == 3)
        {
            builder.Append("<img src=\"/files/face/money.gif\" width=\"20\" height=\"25\"  alt=\"load\"/>"); //派币
        }
        else
        {
            builder.Append(BCW.User.AppCase.CaseText(model.Types));
        }
        builder.Append(BCW.User.AppCase.CaseLabel(model.LabelId, Forummodel.Label) + "" + Out.TitleUBB(model.Title) + "(阅" + model.ReadNum + ")");
        if (model.Types == 8)
        {
            builder.Append("[" + model.Gaddnum + "中" + model.Gwinnum + "]");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + BCW.User.Users.SetForumPic(model.UsID) + "<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(model.UsID, -1) + "</a>" + BCW.User.Users.SetAdmin(model.UsID) + "<br />");
        builder.Append("[" + DT.FormatDate(model.AddTime, 5) + "]");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", "<br />----------"));

        //高手论坛部分
        if (model.Types == 8)
        {
            if (model.UsID == meid)
            {
                DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
                int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
                if (GsqiNum != model.Gqinum && GsStopTime > DateTime.Now)
                {
                    if (model.IsOver == 0)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/Gsedit.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;添加本期</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("记录:连中" + model.Glznum + "期,月中" + model.Gmnum + "期,历史" + model.Gaddnum + "中" + model.Gwinnum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            //列出3条历史记录
            if (model.Gaddnum > 0)
            {
                IList<BCW.Model.Forumvote> listForumvote = new BCW.BLL.Forumvote().GetForumvotes(model.UsID, bid, 3);
                if (listForumvote.Count > 0)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    foreach (BCW.Model.Forumvote n in listForumvote)
                    {
                        builder.Append("[" + n.qiNum + "]期:" + n.Notes + "");
                        if (n.state == 0)
                        {
                            builder.Append("[开?]");
                            if (n.UsID == meid)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("/bbs/Gsedit.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">修改</a>");
                            }
                        }
                        else
                        {
                            if (n.IsWin == 0)
                                builder.Append("[未中]");
                            else
                                builder.Append("[中]");

                        }
                        builder.Append("<br />");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/Gslist.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;hid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;更多往期</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/Gstoplist.aspx?act=centlist&amp;forumid=" + forumid + "&amp;hid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">奖励记录</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", "<br />----------"));
        }


        int pageIndex;
        int recordCount;
        int pageSize = 0;
        //手工分页设置
        if (model.Content.IndexOf("##") == -1)
        {
            pageSize = FsPageSize;
        }
        string[] pageValUrl = { "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //收费/隐藏帖子
        int iTypes = 0;
        if (model.Types == 2)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            if (FsIsImg == 1)
                builder.Append(Utils.ClearImg(Out.SysUBB(model.HideContent)));
            else
                builder.Append(Out.SysUBB(model.HideContent));

            builder.Append(Out.Tab("</div>", ""));
        }
        if (model.Types == 2 && meid != model.UsID)
        {
            if (meid == 0)
                Utils.Login(model.Title);//显示登录

            if (model.Price > 0 && ("#" + model.PayID + "#").IndexOf("#" + meid + "#") == -1)
            {
                iTypes = 1;
            }
            if (model.HideType == 1)
            {
                if (("#" + model.ReplyID + "#").IndexOf("#" + meid + "#") == -1)
                {
                    if (iTypes == 1)
                        iTypes = 3;
                    else
                        iTypes = 2;
                }
            }
            if (iTypes == 1 || iTypes == 3)
            {
                long mecent = new BCW.BLL.User().GetGold(meid);
                if (act == "pay")
                {
                    if (("#" + model.PayID + "#").IndexOf("#" + meid + "#") == -1)
                    {
                        //支付安全提示
                        string[] p_pageArr = { "act", "forumid", "bid", "backurl" };
                        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "post", false);

                        //扣币
                        if (mecent < Convert.ToInt64(model.Price))
                        {
                            Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                        }
                        new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(model.Price), "浏览收费帖子");
                        //帖子得币
                        new BCW.BLL.User().UpdateiGold(model.UsID, Convert.ToInt64(model.Price), "收费帖子获得");
                        //更新购买
                        string PayID = string.Empty;
                        if (string.IsNullOrEmpty(model.PayID))
                            PayID = meid.ToString();
                        else
                            PayID = model.PayID + "#" + meid;

                        new BCW.BLL.Text().UpdatePayID(bid, PayID);
                        //给帖主发内线
                        new BCW.BLL.Guest().Add(model.UsID, model.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]购买了您的收费帖子[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + model.Title + "[/url]");
                        Utils.Success("购买成功", "恭喜，购买成功，正在返回..", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
                    }
                }
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<b>+|以下内容收费" + model.Price + "" + ub.Get("SiteBz") + "</b><br />");
                builder.Append("您自带" + mecent + "" + ub.Get("SiteBz") + "");
                builder.Append(Out.Tab("</div>", "<br />"));

                strName = "forumid,bid,backurl,act";
                strValu = "" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'pay";
                strOthe = "确定购买,topic.aspx,post,0,blue";
                builder.Append(Out.wapform(strName, strValu, strOthe));
            }
            else if (iTypes == 2)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<b>+|以下内容必须回帖才能看到</b>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        if (iTypes > 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("[隐藏内容]");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //复制内容
            if (act == "fzt")
            {
                if (meid == 0)
                    Utils.Login(model.Title);
                new BCW.BLL.User().UpdateCopytemp(meid, model.Content);
                Utils.Success("复制内容", "恭喜，复制内容成功，正在返回..", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
            if (act.Contains("down"))
            {
                if (meid == 0)
                    Utils.Login(model.Title);//显示登录
                //下载帖子
                BCW.User.Down.DownThread(act, model);
            }
            //内容显示
            else if (act == "all")
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                if (FsIsImg == 1)
                    builder.Append(Utils.ClearImg(Out.SysUBB(model.Content.Replace("##", ""))));
                else
                    builder.Append(Out.SysUBB(model.Content.Replace("##", "")));

                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                if (pageIndex == 1 && act != "all")
                {
                    //算出总页数
                    int pageTotal = BasePage.CalcPageCount(model.Content, model.Content.Length, pageSize, ref pageIndex);
                    if (pageTotal > 1)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=all&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">查看全文(共" + pageTotal + "页)</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }

                int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-9]\d*$", "0"));
                string content = BasePage.MultiContent(model.Content, pageIndex, pageSize, pover, out recordCount);
                builder.Append(Out.Tab("<div>", "<br />"));
                if (FsIsImg == 1)
                    builder.Append(Utils.ClearImg(Out.SysUBB(content)));
                else
                    builder.Append(Out.SysUBB(content));

                builder.Append(BasePage.MultiContentPage(model.Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        if (model.FileNum > 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            int pSize = 3;
            string strWhere = "Types=1 and BID=" + bid + " and ReID=0";
            IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pSize, strWhere);
            if (listUpfile.Count > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                foreach (BCW.Model.Upfile n in listUpfile)
                {
                    string Content = n.Content;
                    if (string.IsNullOrEmpty(Content))
                        Content = "无标题";

                    builder.Append("<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + n.Files + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + n.PrevFiles + "\" alt=\"load\"/></a><br />" + Content + "<br />");
                }
            }
            builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?act=file&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部附件(共" + model.FileNum + "个)</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", "<br />----------"));
        //投票项
        if (model.Types == 1 && pageIndex == 1)
        {
            BCW.Model.Votes votemodel = new BCW.BLL.Votes().GetBbsVotes(bid);
            if (!string.IsNullOrEmpty(votemodel.Vote))
            {
                string Votetxt = string.Empty;
                if (votemodel.VoteTiple == 0)
                    Votetxt = "单选";
                else
                    Votetxt = "多选";

                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("=" + Votetxt + "投票=");
                builder.Append(Out.Tab("</div>", ""));
                string[] vote = votemodel.Vote.Split("#".ToCharArray());
                string[] addvote = votemodel.AddVote.Split("#".ToCharArray());
                //取总投票数
                long voteNum = 0;
                bool isvote = false;
                if (votemodel.VoteType == 0)
                {
                    isvote = true;
                }
                else
                {
                    if (votemodel.VoteTiple == 0)
                    {
                        if (("#" + votemodel.VoteID + "#").Contains("#" + meid + "#"))
                            isvote = true;
                    }
                    else
                    {
                        string inum = "_" + meid;
                        if (("#" + votemodel.VoteID + "#").Contains("" + inum + "#"))
                            isvote = true;
                    }
                }

                for (int i = 0; i < addvote.Length; i++)
                {
                    voteNum += Convert.ToInt64(addvote[i]);
                }
                for (int i = 0; i < vote.Length; i++)
                {
                    if (vote[i] != null)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append((i + 1) + "." + vote[i] + "");
                        if (isvote)
                            builder.Append("(" + addvote[i] + ")");

                        builder.Append("<a href=\"" + Utils.getUrl("/votes.aspx?act=ok&amp;id=" + votemodel.ID + "&amp;voteid=" + i + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;投票</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
        }
        //派币帖
        else if (model.Types == 3)
        {
            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append(Out.Tab("<div>", "<br />"));
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                builder.Append("<b>红包:" + model.Prices + "" + bzText + "</b>(有效期15天)<br />");
            }
            else
            {
                builder.Append("<b>红包:" + model.Prices + "" + bzText + "</b>(有效期一周)<br />");
            }
            builder.Append("剩余:" + (model.Prices - model.Pricel) + "" + bzText + "<br />");
            if (model.Price2 == 0)
                builder.Append("每份:" + model.Price + "" + bzText + "");
            else
                builder.Append("每份:" + model.Price + "~" + model.Price2 + "" + bzText + "");
            if (!string.IsNullOrEmpty(model.PricesLimit))
            {
                builder.Append("<br/>回复内容：<b>\"" + model.PricesLimit + "\"</b>才能派币<br />");
            }


            if (model.PayCi != "0")
            {
                builder.Append("(楼尾" + model.PayCi + ")");
            }
            builder.Append(Out.Tab("</div>", "<br />----------"));
        }
        else if (model.Types == 7)
        {

            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<b>悬赏:" + model.Prices + "/");
            builder.Append("剩余:" + (model.Prices - model.Pricel) + "" + bzText + "</b><br />");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=rewardlist&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">悬赏名单</a>(有效期15天)");
            builder.Append(Out.Tab("</div>", "<br />----------"));

        }
        else if (model.Types == 6)
        {

            BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
            if (dc != null)
            {
                string dcText = string.Empty;
                if (dc.BzType == 0)
                    dcText = ub.Get("SiteBz");
                else
                    dcText = ub.Get("SiteBz2");

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<b>庄保证金:" + dc.OutCent + "" + dcText + " </b>");
                if (dc.UsID == meid)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=info2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">续币</a>");
                }
                BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc(bid, 1, meid);
                if (dc2 != null)
                {
                    builder.Append("<br /><b>我的保证金:" + dc2.OutCent + "" + dcText + "</b>");
                }
                builder.Append("<br /><a href=\"" + Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">全部闲家记录&gt;&gt;</a>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("dclist.aspx?act=info&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">我要应战&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", "<br />----------"));
            }

        }

        //操作日志
        int ForumlogCount = new BCW.BLL.Forumlog().GetCount(bid, 0);
        if (ForumlogCount > 0)
        {
            DataSet ds = new BCW.BLL.Forumlog().GetList("Top 1 Content,AddTime", "BID=" + bid + " and ReID=0 Order By ID Desc");
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("[" + DT.FormatDate(DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString()), 6) + "");
            builder.Append("" + Out.SysUBB(Out.RemoveForumLogUBB(ds.Tables[0].Rows[0]["Content"].ToString())) + "");
            if (ForumlogCount > 1)
                builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">..</a>");
            builder.Append("]");
            builder.Append(Out.Tab("</div>", ""));
        }
        //专题
        if (model.TsID > 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("专题:<a href=\"" + Utils.getUrl("forumts.aspx?act=view&amp;forumid=" + forumid + "&amp;id=" + model.TsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Forumts().GetTitle(model.TsID, forumid) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        BCW.Model.Text good = new BCW.BLL.Text().GetTextGoodReCom(5);
        if (good != null)
        {
            builder.Append(Out.Tab("<div>", "<br />"));

            if (good.IsGood == 1)
                builder.Append("[精]");
            else
                builder.Append("[荐]");

            builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + good.ForumId + "&amp;bid=" + good.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.TitleUBB(good.Title) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        //作者勋章签名等
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("荣誉:" + BCW.User.Users.SetLeven(model.UsID) + "");
        builder.Append("" + BCW.User.Users.SetMedal(model.UsID) + "");
        builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetVip(model.UsID) + "</a><br />");
        builder.Append("<small><a href=\"" + Utils.getUrl("myedit.aspx?act=qm&amp;backurl=" + Utils.PostPage(1) + "") + "\">签名</a>:");
        string Sign = new BCW.BLL.User().GetSign(model.UsID);
        if (!string.IsNullOrEmpty(Sign))
            builder.Append(Sign);
        else
            builder.Append("还没有个性签名..");
        builder.Append("</small>");
        builder.Append("<br />楼主的:<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=1&amp;uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">帖子.</a>");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">相册.</a>");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">日记</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //功能区
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgood&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">打赏</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=addok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("mood.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">心情</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">功能箱</a>");
        builder.Append(Out.Tab("</div>", ""));

        #region //版务管理区 发帖管理
        if (meid == model.UsID || new BCW.User.Role().IsUserRole(meid, forumid))
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (meid == model.UsID)
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;管理</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=edit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">编</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=fill&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">续</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=over&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">结</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;管理帖子</a>");

            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("", "<br/>"));
        #endregion
        #region //打赏区域
        BCW.Model.Textcent tt = new BCW.BLL.Textcent().GetTextcentLast(bid);
        if (tt != null)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>友友打赏</b>|<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行</a>");
            string bzText = string.Empty;
            if (tt.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append("<br />" + tt.UsName + ":打赏" + tt.Cents + "" + bzText + ".");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodlist&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多</a>");

            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        #endregion
        #region 这里开始修改点赞和心情
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("帖子心情<br/>");
        builder.Append(Out.Tab("</div>", ""));
        DataSet ds1 = new BCW.BLL.Text().GetList("ReStats,ReList", "ID=" + bid + "");
        string ReStats = ds1.Tables[0].Rows[0]["ReStats"].ToString();
        string ReList = "|" + ds1.Tables[0].Rows[0]["ReList"].ToString() + "|";
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));//4
        //  int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));//  2
        if (string.IsNullOrEmpty(ReStats))
            ReStats = "0|0|0|0|0";
        string sStats = string.Empty;
        //

        bool temp = false;  //判断修改标志  默认不可以修改
        string jungle = meid.ToString();
        string[] list = ds1.Tables[0].Rows[0]["ReList"].ToString().Split('|');
        for (int i = 0; i < list.Length; i++)
        {
            // builder.Append("list:"+jungle + "-");
            if (jungle.Trim() == list[i].Trim())//如果包含meid
            {
                temp = false;  //设置为不可以修改
            }
            else//如果不包含meid
            {
                temp = true;  //设置为可以修改
            }
        }
        bool test = !("|" + ReList + "|").Contains("|" + meid + "|");


        if (v != 0 && temp && test)//不包含这个ID
        {
            int meid1 = new BCW.User.Users().GetUsId();
            if (meid1 == 0)
                Utils.Login();

            if (v >= 1 && v <= 5)  //限定 V 的取值在 1-5之间才能修改数据
            {
                string[] arrReStats = ReStats.Split("|".ToCharArray());
                for (int i = 0; i < arrReStats.Length; i++) //5
                {
                    if ((v - 1) == i)
                    {
                        sStats += "|" + Convert.ToInt32(Convert.ToInt32(arrReStats[i]) + 1);//重写数据
                    }
                    else
                    {
                        sStats += "|" + arrReStats[i];//原有数据
                    }
                }
                sStats = Utils.Mid(sStats, 1, sStats.Length);
                string result = string.Empty;
                if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["ReList"].ToString()))//判断是否为空
                {
                    result = ds1.Tables[0].Rows[0]["ReList"].ToString() + meid;
                }
                else
                {
                    result = ds1.Tables[0].Rows[0]["ReList"].ToString() + "|" + meid;
                }
                //builder.Append(ds.Tables[0].Rows[0]["ReList"].ToString() + "<br/>");
                //builder.Append(result + "  写入数据库的数据result");
                // ReList = Utils.Mid(ReList + "|" + meid, 1, (ReList + "|" + meid).Length);
                new BCW.BLL.Text().UpdateReStats(bid, sStats, result);

            }
        }
        else
        {
            sStats = ReStats;

        }

        string ReText = "快乐|伤心|幽默|好帖";
        string[] arrText = ReText.Split("|".ToCharArray());
        string[] arrsStats1 = sStats.Split("|".ToCharArray());
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;v=" + (0 + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/face/em" + 0 + ".gif\"  alt=\"load\"/>" + arrText[0] + "(" + arrsStats1[0] + ")</a>");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;v=" + (1 + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/face/em" + 1 + ".gif\"  alt=\"load\"/>" + arrText[1] + "(" + arrsStats1[1] + ")</a>");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;v=" + (2 + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/face/em" + 2 + ".gif\"  alt=\"load\"/>" + arrText[2] + "(" + arrsStats1[2] + ")</a>");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;v=" + (3 + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/face/em" + 3 + ".gif\"  alt=\"load\"/>" + arrText[3] + "(" + arrsStats1[3] + ")</a>");
        // builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;v=" + (4 + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/face/em" + 4 + ".gif\" alt=\"load\"/>" + arrText[4] + "(" + arrsStats1[4] + ")</a>");

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forpraise&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"/files/face/em" + 5 + ".gif\"   alt=\"load\"/></a>");

        builder.Append("|<a href=\"" + Utils.getUrl("reply.aspx?act=praiseranking&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">" + "(" + new BCW.BLL.Text().GetPraise(bid) + ")</a>");
        string praiseid = new BCW.BLL.Text().GetPraiseID(bid);
        if (!string.IsNullOrEmpty(praiseid))  //判断是否有人点赞
        {
            string[] pid = praiseid.Split('#');
            if (pid.Length < 15)
            {
                for (int i = 0; i < pid.Length; i++)
                {
                    int ID = int.Parse(pid[i]);
                    //string image = new BCW.BLL.User().GetPhoto(ID);
                    //new BCW.Graph.ImageHelper().ResizeImage(image, Path, 25, 33, true);

                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + new BCW.BLL.User().GetPhoto(ID) + "\" width=\"25\" height=\"33\" alt=\"load\"/></a>");
                    // builder.Append("<br/><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + new BCW.BLL.User().GetPhoto(ID) + "\" width=\"2%\" height=\"4%\" alt=\"load\"/>" + new BCW.BLL.User().GetUsName(ID) + "</a>");
                }
            }
            else
            {
                for (int i = 0; i < 15; i++)
                {
                    int ID = int.Parse(pid[i]);
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + new BCW.BLL.User().GetPhoto(ID) + "\" width=\"25\" height=\"33\" alt=\"load\"/></a>");
                    //builder.Append("<br/><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + new BCW.BLL.User().GetPhoto(ID) + "\" width=\"2%\" height=\"4%\" alt=\"load\"/>" + new BCW.BLL.User().GetUsName(ID) + "</a>");
                }
            }
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));

        #endregion    修改结束
        //得到随机短语
        string ReplyValu = string.Empty;
        strText = ",,,,,,";
        strName = "Face,Content,forumid,bid,backurl,act";
        strType = "select,text,hidden,hidden,hidden,hidden";
        strValu = "0'" + ReplyValu + "'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'save";
        strEmpt = "0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,true,false,false,false,false";
        strIdea = "";
        strOthe = "回复,reply.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append("<br/>");
        //回复列表
        if (FsIsReList == 0)
        {
            int pSize = FsReList;
            string strWhere = "Bid=" + bid + " and IsDel=0";
            IList<BCW.Model.Reply> listReply = new BCW.BLL.Reply().GetReplysTop(pSize, strWhere);
            if (listReply.Count > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                foreach (BCW.Model.Reply n in listReply)
                {
                    n.UsName = BCW.User.Users.SetVipName(n.UsID, n.UsName, false);
                    builder.Append("<b>" + n.Floor + "楼</b>:");
                  
                    if (n.Content != "")
                        builder.Append(Out.SysUBB(Regex.Replace(Utils.Left(n.Content, 30), @"&(?![\#\w]{2,6};)", "&amp;")));
                    if (n.Content.Length > 30)
                        builder.Append("...");

                    if (n.FileNum > 0)
                        builder.Append("[附]");
                    builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.Floor + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;</a> ");
                    builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.Floor + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">评</a> ");

                    if (n.ReplyId > 0)//回复多少楼的，如果false就是直接回复帖
                    {
                        builder.Append("<br/>回复<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.ReplyId + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.ReplyId + "楼</a>内容:");
                        string neirong = new BCW.BLL.Reply().GetContent(bid, n.ReplyId);
                        if (neirong.Length > 15)
                        {
                            neirong = neirong.Substring(0, 15);
                            neirong += "...";
                            builder.Append(Out.SysUBB(neirong));
                        }
                        else
                        {
                            builder.Append(Out.SysUBB(neirong));

                        }
                    }
                    builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;" + n.UsName + "</a><font size=\" 1px\" >:" + DT.FormatDate(n.AddTime, 6) + "</font>");
                    //   需要修改textcent表增加回帖打赏字段
                    BCW.Model.Textcent tt1 = new BCW.BLL.Textcent().GetTextcentReply(n.UsID, n.Floor, bid);
                    if (tt1 != null)   //如果有该回帖有给打赏就显示打赏的
                    {
                        // builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("<br/><b>友友打赏</b>|<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行</a>");
                        string bzText = string.Empty;
                        if (tt1.BzType == 0)
                            bzText = ub.Get("SiteBz");
                        else
                            bzText = ub.Get("SiteBz2");

                        builder.Append("<br />" + tt1.UsName + ":打赏" + tt1.Cents + "" + bzText + ".");
                        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodlist&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多</a>");              
                    }
                    builder.Append("<br />----------------------------<br/>");
                }
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">更多回复(" + model.ReplyNum + ")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;只看楼主</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("还没有回帖，快抢沙发呀..");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复</a>");
        builder.Append("|<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">其它帖子</a>");
        builder.Append(Out.Tab("</div>", ""));

        //更新阅读数
        new BCW.BLL.Text().UpdateReadNum(bid, Utils.ParseInt(ub.GetSub("BbsClick", xmlPath)));
    }

    /// <summary>
    /// 穿透圈子限制ID
    /// </summary>
    private bool IsCTID(int meid)
    {
        bool Isvi = false;
        //能够穿透的ID
        string CTID = "#" + ub.GetSub("GroupCTID", "/Controls/group.xml") + "#";
        if (CTID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }

        return Isvi;
    }
}