using System;
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

public partial class bbs_addThread20 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/upfile.xml";
    protected string xmlPath2 = "/Controls/bbs.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "上传文件";
        new Out().SetPageNoCache();
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Error("不存在的论坛或此论坛已暂停使用", "");
        }
        if (bid != 0)
        {
            if (!new BCW.BLL.Text().Exists2(bid, forumid))
            {
                Utils.Error("帖子不存在或已被删除", "");
            }
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "upload":
                UploadPage(forumid, bid);
                break;
            default:
                ReloadPage(act, meid, forumid, bid);
                break;
        }

    }

    private void ReloadPage(string act, int uid, int forumid, int bid)
    {
        //复制内容
        int copy = int.Parse(Utils.GetRequest("copy", "get", 1, @"^[0-1]$", "0"));
        string Copytemp = string.Empty;
        if (copy == 1)
            Copytemp = new BCW.BLL.User().GetCopytemp(uid);

        if (!Utils.Isie())
        {
            builder.Append(Out.Tab("<div>", ""));
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append("<a href=\"addThread20.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (bid == 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>");
                builder.Append("&gt;上传文件");
            }
            else
            {
                if (act != "fill")
                    builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>&gt;文件回复");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>&gt;追加文件");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int max = Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));//最大文件数
            string maxfile = ub.GetSub("UpaMaxFileSize", xmlPath);//最大文件容量
            string fileExt = ub.GetSub("UpaFileExt", xmlPath);//文件格式
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上传允许格式:" + fileExt + "<br />");
            builder.Append("每个文件限" + maxfile + "K");
            builder.Append(Out.Tab("</div>", "<br />"));

            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
            if (num > max)
                num = max;

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上传:");
            for (int i = 1; i <= max; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("addThread20.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;num=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>" + i + "</b></a> ");
            }
            builder.Append("个");
            builder.Append(Out.Tab("</div>", ""));
            string sUpType = string.Empty;
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            if (act != "fill")
            {
                if (bid != 0)
                {
                    sText = "内容(300字内):/,";
                    sName = "Content,";
                    sType = "big,";
                    sValu = "" + Copytemp + "'";
                    sEmpt = "false,";
                }
                else
                {
                    sText = "主题(30字内):,内容(5000字内):/,";
                    sName = "Title,Content,";
                    sType = "text,big,";
                    sValu = "'" + Copytemp + "'";
                    sEmpt = "false,false,";
                }
            }
            for (int i = 0; i < num; i++)
            {
                string y = ",";
                if (num == 1)
                {
                    strText = strText + y + "选择" + sUpType + "附件:/," + sUpType + "附件描述(30字内):/";
                }
                else
                {
                    strText = strText + y + "" + sUpType + "第" + (i + 1) + "个附件:/," + sUpType + "附件描述" + (i + 1) + ":/";
                }
                strName = strName + y + "file" + (i + 1) + y + "stext" + (i + 1);
                strType = strType + y + "file" + y + "text";
                strValu = strValu + "''";
                strEmpt = strEmpt + y + y;
            }

            strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
            strName = sName + Utils.Mid(strName, 1, strName.Length) + ",forumid,bid,act,backurl";
            strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
            strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + forumid + "'" + bid + "'upload'" + Utils.getPage(0) + "";
            strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
            strIdea = "/";
            if (act != "fill")
                strOthe = "发表文本|上传|reset,addThread20.aspx,post,2,red|blue|blue";
            else
                strOthe = "续传|reset,addThread20.aspx,post,2,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            if (act != "fill")
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("addThread20.aspx?forumid=" + forumid + "&amp;copy=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[粘贴]</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getPage("forum.aspx?forumid=" + forumid + "")) + "\">取消发表</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 上传文件页面
    /// </summary>
    private void UploadPage(int forumid, int bid)
    {
        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "post", 1, "", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (ac != "发表文本")
        {
            BCW.User.Users.ShowVerifyRole("f", meid);//非验证会员提示
            new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Upfile, meid);//会员上传权限
        }
        //论坛限制性
        BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(forumid);
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
                    Utils.Login();

                string GroupId = new BCW.BLL.User().GetGroupId(meid);
                if (GroupId.IndexOf("#" + Forummodel.GroupId + "#") == -1 && IsCTID(meid) == false)
                {
                    Utils.Error("非成员不能访问" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛！<br /><a href=\"" + Utils.getUrl("/bbs/group.aspx?act=addin&amp;id=" + Forummodel.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", "/Controls/group.xml") + "</a>", "");
                }
            }
        }
        BCW.User.Users.ShowForumLimit(meid, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
        if (ac == "续传" || bid == 0)//发帖
        {
            //是否刷屏
            string appName = "LIGHT_THREAD";
            int Expir = Convert.ToInt32(ub.GetSub("BbsThreadExpir", xmlPath2));
            BCW.User.Users.IsFresh(appName, Expir);

            BCW.User.Users.ShowVerifyRole("a", meid);//非验证会员提示
            new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Text, meid);//会员本身权限
            new BCW.User.FLimits().CheckUserFLimit(BCW.User.FLimits.enumRole.Role_Text, meid, forumid);//版块内权限
            BCW.User.Users.ShowAddThread(meid, Forummodel.Postlt);
        }
        else//回帖
        {
            //是否刷屏
            string appName = "LIGHT_REPLY";
            int Expir = Convert.ToInt32(ub.GetSub("BbsReplyExpir", xmlPath2));
            BCW.User.Users.IsFresh(appName, Expir);

            BCW.User.Users.ShowVerifyRole("b", meid);//非验证会员提示
            new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Reply, meid);//会员本身权限
            new BCW.User.FLimits().CheckUserFLimit(BCW.User.FLimits.enumRole.Role_Reply, meid, forumid);//版块内权限
            BCW.User.Users.ShowAddReply(meid, Forummodel.Replylt);
        }
        int kk = 0;
        int reid = 0;
        int ptype = 5;//标识为附件帖子
        if (ac == "续传")
        {
            //上传文件
            SaveFiles(meid, forumid, bid, reid, out kk);
            string strOut = string.Empty;
            if (kk < 0)
            {
                if (kk == -999)
                    kk = 0;

                strOut = "部分文件超出今天上传数量导致上传失败，";
                kk = Math.Abs(kk);
            }
            //更新帖子文件数
            new BCW.BLL.Text().UpdateFileNum(bid, kk);
            new BCW.BLL.Text().UpdateTypes(bid, ptype);
            //记录日志
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]对主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + new BCW.BLL.Text().GetTitle(bid) + "》[/url]追加" + kk + "个文件!";
            new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);
            Utils.Success("追加文件", "追加" + kk + "个文件成功，" + strOut + "正在返回..", ReplaceWap(Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")), "2");
        }
        else
        {
            string mename = new BCW.BLL.User().GetUsName(meid);
            string Title = string.Empty;
            string Content = string.Empty;
            if (bid == 0)
            {
                Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{" + ub.GetSub("BbsThreadMin", xmlPath2) + "," + ub.GetSub("BbsThreadMax", xmlPath2) + "}$", "标题限" + ub.GetSub("BbsThreadMin", xmlPath2) + "-" + ub.GetSub("BbsThreadMax", xmlPath2) + "字");
                Title = Title.Replace(char.ConvertFromUtf32(10), "").Replace(char.ConvertFromUtf32(13), "");
                Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{" + ub.GetSub("BbsContentMin", xmlPath2) + "," + ub.GetSub("BbsContentMax", xmlPath2) + "}$", "请输入" + ub.GetSub("BbsContentMin", xmlPath2) + "-" + ub.GetSub("BbsContentMax", xmlPath2) + "字的内容");
            }
            else
            {
                Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ub.GetSub("BbsReplyMax", xmlPath2) + "}$", "请输入不超" + ub.GetSub("BbsReplyMax", xmlPath2) + "字的回帖内容");
            }

            if (bid == 0)
            {

                int ThreadNum = Utils.ParseInt(ub.GetSub("BbsThreadNum", xmlPath));
                if (ThreadNum > 0)
                {
                    int ToDayCount = new BCW.BLL.Forumstat().GetCount(meid, 1);//今天发布帖子数
                    if (ToDayCount >= ThreadNum)
                    {
                        Utils.Error("系统限每天每ID限发帖子" + ThreadNum + "帖", "");
                    }
                }

                int Price = 0;
                int Prices = 0;
                int HideType = 0;
                int IsSeen = 0;
                string PayCi = string.Empty;

                BCW.Model.Text addmodel = new BCW.Model.Text();
                addmodel.ForumId = forumid;
                addmodel.Types = ptype;
                addmodel.Title = Title;
                addmodel.Content = Content;
                addmodel.HideContent = "";
                addmodel.UsID = meid;
                addmodel.UsName = mename;
                addmodel.Price = Price;
                addmodel.Prices = Prices;
                addmodel.HideType = HideType;
                addmodel.PayCi = PayCi;
                addmodel.IsSeen = IsSeen;
                addmodel.AddTime = DateTime.Now;
                addmodel.ReTime = DateTime.Now;
                bid = new BCW.BLL.Text().Add(addmodel);
                //上传文件
                SaveFiles(meid, forumid, bid, reid, out kk);
                string strOut = string.Empty;
                if (kk < 0)
                {
                    if (kk == -999)
                        kk = 0;

                    strOut = "部分文件超出今天上传数量导致上传失败，";
                    kk = Math.Abs(kk);
                }
                //更新帖子文件数
                new BCW.BLL.Text().UpdateFileNum(bid, kk);
                //论坛统计
                BCW.User.Users.UpdateForumStat(1, meid, mename, forumid);

                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);

                //动态记录
                if (GroupId == 0)
                {
                    new BCW.BLL.Action().Add(-1, 0, meid, mename, "在" + Forummodel.Title + "发表了文件帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + Title + "[/URL]");
                }
                else
                {
                    new BCW.BLL.Action().Add(-2, 0, meid, mename, "在圈坛-" + Forummodel.Title + "发表了文件帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + Title + "[/URL]");                
                }
                //积分操作/论坛统计/圈子论坛不进行任何奖励

                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Text, meid, true);
                }
                else
                {
                    if (!Utils.GetDomain().Contains("th"))
                        new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Text, meid, false);
                }
                if (kk == 0)
                    new BCW.BLL.Text().UpdateTypes(bid, 0);//去掉附件帖标识
                #region  这里开始修改提醒ID 发内线
                string remind = ub.GetSub("remindid" + forumid, "/Controls/bbs.xml");//获取XML的值
                if (remind != "")  //如果有提醒ID
                {
                    string[] IDS = remind.Split('#');
                    for (int i = 0; i < IDS.Length; i++)
                    {
                        if (GroupId != 0)
                        {
                            new BCW.BLL.Guest().Add(0, int.Parse(IDS[i]), new BCW.BLL.User().GetUsName(int.Parse(IDS[i])), "请注意!用户[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]在圈坛-" + Forummodel.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + Title + "[/URL]的帖子");
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(0, int.Parse(IDS[i]), new BCW.BLL.User().GetUsName(int.Parse(IDS[i])), "请注意!用户[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]在" + Forummodel.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + Title + "[/URL]的帖子");
                        }
                    }
                }
                #endregion
                Utils.Success("上传文件", "上传" + kk + "个文件成功，" + strOut + "正在返回..", ReplaceWap(Utils.getPage("forum.aspx?forumid=" + forumid + "")), "2");
            }
            else
            {
                int ReplyNum = Utils.ParseInt(ub.GetSub("BbsReplyNum", xmlPath));
                if (ReplyNum > 0)
                {
                    int ToDayCount = new BCW.BLL.Forumstat().GetCount(meid, 2);//今天发布回帖数
                    if (ToDayCount >= ReplyNum)
                    {
                        Utils.Error("系统限每天每ID限回帖" + ReplyNum + "次", "");
                    }
                }

                int Floor = new BCW.BLL.Reply().GetFloor(bid);

                //派币帖
                string CentText = string.Empty;
                string PbCent = string.Empty;
                int iTypes = new BCW.BLL.Text().GetTypes(bid);
                if (iTypes == 3)
                {
                    BCW.Model.Text p = new BCW.BLL.Text().GetPriceModel(bid);
                    if (p.Prices - p.Pricel > 0)
                    {
                        string bzText = string.Empty;
                        if (p.BzType == 0)
                            bzText = ub.Get("SiteBz");
                        else
                            bzText = ub.Get("SiteBz2");

                        long zPrice = 0;
                        if (p.Price2 > 0)
                            zPrice = Convert.ToInt64(new Random().Next(p.Price, (p.Price2 + 1)));//随机得到奖币值
                        else
                            zPrice = Convert.ToInt64(p.Price);

                        long GetPrice = 0;
                        if (p.Prices - p.Pricel < zPrice)
                            GetPrice = p.Prices - p.Pricel;
                        else
                            GetPrice = zPrice;

                        if (p.PayCi == "0")
                        {
                            if (("#" + p.ReplyID + "#").IndexOf("#" + meid + "#") == -1)
                            {
                                if (p.BzType == 0)
                                    new BCW.BLL.User().UpdateiGold(meid, mename, GetPrice, "派币帖回帖获得");
                                else
                                    new BCW.BLL.User().UpdateiMoney(meid, mename, GetPrice, "派币帖回帖获得");

                                //更新已派
                                new BCW.BLL.Text().UpdatePricel(bid, GetPrice);
                                CentText = "" + GetPrice + "" + bzText + "";
                                PbCent = "楼主派" + GetPrice + "" + bzText + "";
                            }
                        }
                        else
                        {
                            if (("#" + p.PayCi + "#").IndexOf("#" + Utils.Right(Floor.ToString(), 1) + "#") != -1)
                            {
                                if (p.BzType == 0)
                                    new BCW.BLL.User().UpdateiGold(meid, mename, GetPrice, "派币帖回帖获得");
                                else
                                    new BCW.BLL.User().UpdateiMoney(meid, mename, GetPrice, "派币帖回帖获得");

                                //更新已派
                                new BCW.BLL.Text().UpdatePricel(bid, GetPrice);
                                CentText = "" + GetPrice + "" + bzText + "";
                                PbCent = "踩中楼层" + Utils.Right(Floor.ToString(), 1) + "尾，楼主派" + GetPrice + "" + bzText + "";
                            }
                        }
                    }
                }

                BCW.Model.Reply model = new BCW.Model.Reply();
                model.Floor = Floor;
                model.ForumId = forumid;
                model.Bid = bid;
                model.UsID = meid;
                model.UsName = mename;
                model.Content = Content;
                model.FileNum = 0;
                model.ReplyId = 0;
                model.AddTime = DateTime.Now;
                model.CentText = CentText;
                reid = new BCW.BLL.Reply().Add(model);

                //更新回复ID
                string sPayID = new BCW.BLL.Text().GetReplyID(bid);
                if (("#" + sPayID + "#").IndexOf("#" + meid + "#") == -1)
                {
                    string ReplyID = string.Empty;
                    if (string.IsNullOrEmpty(sPayID))
                        ReplyID = meid.ToString();
                    else
                        ReplyID = sPayID + "#" + meid;
                    new BCW.BLL.Text().UpdateReplyID(bid, ReplyID);
                }
                //更新回复数
                new BCW.BLL.Text().UpdateReplyNum(bid, 1);
                //上传文件
                SaveFiles(meid, forumid, bid, reid, out kk);
                string strOut = string.Empty;
                if (kk < 0)
                {
                    if (kk == -999)
                        kk = 0;

                    strOut = "部分文件超出今天上传数量导致上传失败！";
                    kk = Math.Abs(kk);
                }
                //更新回复文件数
                new BCW.BLL.Reply().UpdateFileNum(reid, kk);
                //论坛统计
                BCW.User.Users.UpdateForumStat(2, meid, mename, forumid);

                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);

                //动态记录
                if (GroupId == 0)
                {
                    new BCW.BLL.Action().Add(-1,0,meid, mename, "在" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/URL]");
                }
                else
                {
                    new BCW.BLL.Action().Add(-2, 0, meid, mename, "在圈坛-" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/URL]");
                
                }
                //积分操作/论坛统计/圈子论坛不进行任何奖励
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Reply, meid, true);
                }
                else
                {
                    if (!Utils.GetDomain().Contains("th"))
                        new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Reply, meid, false);
                }
                Utils.Success("文件回帖", "回复" + kk + "个文件成功！" + strOut + "" + PbCent + "<br /><a href=\"" + ReplaceWap(Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")) + "\">返回主题</a><br /><a href=\"" + ReplaceWap(Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")) + "\">回复列表</a>", ReplaceWap(Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")), "2");

            }
        }
    }
    /// <summary>
    /// 上传文件
    /// </summary>
    private void SaveFiles(int meid, int forumid, int bid, int reid, out int kk)
    {
        //允许上传数量
        int maxAddNum = Convert.ToInt32(ub.GetSub("UpAddNum", xmlPath));
        int AddNum = 0;
        if (maxAddNum > 0)
        {
            //计算今天上传数量
            AddNum = new BCW.BLL.Upfile().GetTodayCount(meid);
        }
        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        //int j = 1;
        int j = files.Count;
        int k = 0;
        try
        {
            string GetFiles = string.Empty;
            for (int iFile = files.Count-1; iFile > -1; iFile--)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);//上传的文件名字
                string UpExt = ub.GetSub("UpaFileExt", xmlPath);//文件格式a
                int UpLength = Convert.ToInt32(ub.GetSub("UpaMaxFileSize", xmlPath));//文件大小限制
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    //检查是否允许上传格式
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        continue;
                    }
                    //非法上传
                    if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                    {
                        continue;
                    }
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))   //超过文件大小限制
                    {
                        continue;
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = "/Files/bbs/" + meid + "/act/";
                    string prevPath = "/Files/bbs/" + meid + "/prev/";
                    int IsVerify = 0;
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {                    
                        //上传数量限制
                        if (maxAddNum > 0)
                        {
                            if (maxAddNum <= (AddNum + k))
                            {
                                k = -k;
                                if (k == 0)
                                    k = -999;
                                break;
                            }
                        }
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;//现在系统时间+数组下标+文件后缀名
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        postedFile.SaveAs(SavePath);

                        //=============================图片木马检测,包括TXT===========================
                        string vSavePath = SavePath;
                        if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")  //加点
                        {
                            bool IsPass = true;
                            System.IO.StreamReader sr = new System.IO.StreamReader(vSavePath, System.Text.Encoding.Default);
                            string strContent = sr.ReadToEnd().ToLower();
                            sr.Close();
                            string str = "system.|request|javascript|script |script>|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                            foreach (string s in str.Split('|'))
                            {
                                if (strContent.IndexOf(s) != -1)
                                {
                                    System.IO.File.Delete(vSavePath);
                                    IsPass = false;
                                    break;
                                }
                            }
                            if (IsPass == false)
                                continue;
                        }
                        //=============================图片木马检测,包括TXT===========================

                        //审核要求指示
                        int Verify = Utils.ParseInt(ub.GetSub("UpIsVerify", xmlPath));  //0
                        //缩略图生成
                        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
                        {
                            int ThumbType = Convert.ToInt32(ub.GetSub("UpaThumbType", xmlPath));   // 1
                            int width = Convert.ToInt32(ub.GetSub("UpaWidth", xmlPath));  //56
                            int height = Convert.ToInt32(ub.GetSub("UpaHeight", xmlPath));   //70
                            if (ThumbType > 0)
                            {
                                try
                                {
                                    bool pbool = false;
                                    if (ThumbType == 1)
                                        pbool = true;
                                    if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                    {
                                        string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;

                                        int IsThumb = 0;
                                        if (fileExtension == ".gif")
                                        {
                                            if (ThumbType > 0)
                                                new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                            IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));  //0
                                            if (IsThumb > 0)
                                            {
                                                if (IsThumb == 1)
                                                    new BCW.Graph.GifHelper().SmartWaterMark(SavePath, "", ub.GetSub("UpaWord", xmlPath), ub.GetSub("UpaWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//文字水印
                                                else
                                                    new BCW.Graph.GifHelper().WaterMark(SavePath, "", Server.MapPath(ub.GetSub("UpaWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpaTran", xmlPath)));//图片水印
                                            }
                                        }
                                        else
                                        {
                                            if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                                            {
                                                if (ThumbType > 0)
                                                    new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                                IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));
                                                if (IsThumb > 0)
                                                {
                                                    if (IsThumb == 1)
                                                        new BCW.Graph.ImageHelper().WaterMark(SavePath, "", ub.GetSub("UpaWord", xmlPath), ub.GetSub("UpaWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//文字水印
                                                    else
                                                        new BCW.Graph.ImageHelper().WaterMark(SavePath, "", Server.MapPath(ub.GetSub("UpaWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpaTran", xmlPath)));//图片水印
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }

                            //图片审核
                            if (Verify > 0)
                                IsVerify = 1;
                        }
                        else
                        {
                            //文件审核
                            if (Verify > 1)
                                IsVerify = 1;
                        }

                        string Content = Utils.GetRequest("stext" + j + "", "post", 1, "", "");
                        if (!string.IsNullOrEmpty(Content))
                            Content = Utils.Left(Content, 30);
                        else
                            Content = "";

                        BCW.Model.Upfile model = new BCW.Model.Upfile();
                        model.Types = FileTool.GetExtType(fileExtension);
                        model.NodeId = 0;
                        model.UsID = meid;
                        model.ForumID = forumid;
                        model.BID = bid;
                        model.ReID = reid;
                        model.Files = DirPath + fileName;
                        if (string.IsNullOrEmpty(prevDirPath))
                            model.PrevFiles = model.Files;
                        else
                            model.PrevFiles = prevDirPath + fileName;

                        model.Content = Content;
                        model.FileSize = Convert.ToInt64(postedFile.ContentLength);
                        model.FileExt = fileExtension;
                        model.DownNum = 0;
                        model.Cent = 0;
                        model.IsVerify = IsVerify;
                        model.AddTime = DateTime.Now;
                        new BCW.BLL.Upfile().Add(model);
                        k++;
                    }
                    //j++;
                    j--;
                }
            }

        }
        catch { }
        kk = k;
    }

    private static string ReplaceWap(string p_strUrl)
    {
        p_strUrl = p_strUrl.Replace("20a", "1a");

        return p_strUrl;
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