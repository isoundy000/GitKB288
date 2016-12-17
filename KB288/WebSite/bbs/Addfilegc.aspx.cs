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

public partial class bbs_Addfilegc : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/upfile.xml";
    protected string xmlPath2 = "/Controls/bbs.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        Response.Write("<head>");
        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        Response.Write("<meta name=\"viewport\" content=\"width=device-width; initial-scale=1.0; minimum-scale=1.0; maximum-scale=2.0; user-scalable=0;\" />");
        Response.Write("<meta http-equiv=\"Cache-Control\" content=\"max-age=0\"/>");
        Response.Write("<title>WAP2.0文件上传</title>");
        Response.Write("</head>");
        Response.Write("<body>");

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Response.Write("不存在的论坛或此论坛已暂停使用<br/>");
            Response.Write(Out.back("返回上一级"));
            Response.Write("</body></html>");
            Response.End();
        }
        if (bid != 0)
        {
            if (!new BCW.BLL.Text().Exists2(bid, forumid))
            {
                Response.Write("帖子不存在或已被删除<br/>");
                Response.Write(Out.back("返回上一级"));
                Response.Write("</body></html>");
                Response.End();
            }
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
        {
            Response.Write("请您先<a href=\"" + Utils.getUrl("/login.aspx") + "\">登录</a>!<br/>");
            Response.Write(Out.back("返回上一级"));
            Response.Write("</body></html>");
            Response.End();
        }

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

        int max = Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));
        string maxfile = ub.GetSub("UpaMaxFileSize", xmlPath);
        string fileExt = ub.GetSub("UpaFileExt", xmlPath);
        Response.Write(Out.Tab("<div>", ""));
        Response.Write("上传允许格式:" + fileExt + "<br />");
        Response.Write("每个文件限" + maxfile + "K");

        Response.Write("<form name=\"form2\" method=\"post\" action=\"" + Utils.getUrl("addfilegc.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;act=upload&amp;ac=" + act + "&amp;backurl=" + Utils.getPage(0) + "") + "\" enctype=\"multipart/form-data\">");

        if (act != "fill")
        {
            if (bid != 0)
            {
                Response.Write("内容(300字内):<br/><textarea  name=\"Content\"  cols=\"20\" rows=\"8\">" + Copytemp + "</textarea><br/>");
            }
            else
            {
                Response.Write("主题(30字内):<input name=\"Title\" maxlength=\"100\" value=\"\"/><br/>");
                Response.Write("内容(5000字内):<br/><textarea name=\"Content\"  cols=\"20\" rows=\"8\">" + Copytemp + "</textarea><br/>");

            }
        }

        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");

        Response.Write("选择文件(第1个必须选择,否则无法上传)<br/>");
        Response.Write("第1个文件:<br/><input type=\"file\" name=\"filename1\"/><BR/>");
        Response.Write("描述1(30字内):<br/><input name=\"stext1\" maxlength=\"30\" value=\"\"/><br/><br/>");

        Response.Write("第2个文件:<br/><input type=\"file\" name=\"filename2\"/><BR/>");
        Response.Write("描述2(30字内):<br/><input name=\"stext2\" maxlength=\"30\" value=\"\"/><br/><br/>");

        Response.Write("第3个文件:<br/><input type=\"file\" name=\"filename3\"/><BR/>");
        Response.Write("描述3(30字内):<br/><input name=\"stext3\" maxlength=\"30\" value=\"\"/><br/><br/>");

        //Response.Write("<input name=\"forumid\" type=\"hidden\" value=\"" + forumid + "\"/>");
        //Response.Write("<input name=\"bid\" type=\"hidden\" value=\"" + bid + "\"/>");
        //Response.Write("<input name=\"act\" type=\"hidden\" value=\"upload\"/>");
        //Response.Write("<input type=\"hidden\" name=\"" + VE + "\" value=\"" + Utils.getstrVe() + "\"/>");
        //Response.Write("<input type=\"hidden\" name=\"" + SID + "\" value=\"" + Utils.getstrU() + "\"/>");
        //Response.Write("<input name=\"backurl\" type=\"hidden\" value=\"" + Utils.getPage(0) + "\"/>");
        if (act != "fill")
        {
            //Response.Write("<input name=\"ac\" type=\"hidden\" value=\"上传\"/>");
            Response.Write("<input type=\"submit\" value=\"上传文件\"/>");
        }
        else
        {
            //Response.Write("<input name=\"ac\" type=\"hidden\" value=\"续传\"/>");
            Response.Write("<input type=\"submit\" value=\"续传文件\"/>");
        }
        Response.Write("</form>");


        if (bid == 0)
        {
            Response.Write("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回之前页面</a>");
        }
        else
        {
            Response.Write("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">返回之前页面</a>");
        }
        Response.Write("<br/><a href=\"" + Utils.getUrl("default.aspx") + "\">返回社区首页</a>");
        Response.Write("<br/>=上传帮助说明= ");
        Response.Write("<br/>1.建议所上传文件名和目录组合是字母与数字. ");
        Response.Write("<br/>2.上传文件过大可能造成上传失败. ");
        Response.Write("<br/>3.只有部分手机支持上传,详情请联系您的手机供应商. ");
        Response.Write("<br/>4.文件上传需要时间较长,请耐心等待. ");
        Response.Write("<br/>5.如果上传失败,建议您尝试更换手机的上网端口80或改成9201.");
        Response.Write("</body></html>");
    }


    /// <summary>
    /// 上传文件页面
    /// </summary>
    private void UploadPage(int forumid, int bid)
    {
        //string ac = Utils.ToSChinese(Utils.GetRequest("ac", "post", 1, "", ""));
        string ac = Utils.GetRequest("ac", "get", 1, "", "");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("f", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Upfile, meid);//会员上传权限

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

        if (ac == "fill" || bid == 0)//发帖
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
        if (ac == "fill")
        {
            //上传文件
            SaveFiles(meid, forumid, bid, reid, out kk);
            string strOut = string.Empty;
            if (kk < 0)
            {
                if (kk == -999)
                    kk = 0;

                strOut = "，部分文件超出今天上传数量导致上传失败";
                kk = Math.Abs(kk);
            }
            //更新帖子文件数
            new BCW.BLL.Text().UpdateFileNum(bid, kk);
            new BCW.BLL.Text().UpdateTypes(bid, ptype);
            //记录日志
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]对主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + new BCW.BLL.Text().GetTitle(bid) + "》[/url]追加" + kk + "个文件!";
            new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);

            Response.Write("追加" + kk + "个文件成功" + strOut + "");
            Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")) + "\">返回之前页面</a>");
            Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回社区首页</a>");
            Response.Write("</body></html>");
            Response.End();

        }
        else
        {
            string mename = new BCW.BLL.User().GetUsName(meid);
            string Title = string.Empty;
            string Content = string.Empty;
            if (bid == 0)
            {
                Title = Utils.GetRequest("Title", "post", 1, "", "");
                if (!Utils.IsRegex(Title, @"^[\s\S]{" + ub.GetSub("BbsThreadMin", xmlPath2) + "," + ub.GetSub("BbsThreadMax", xmlPath2) + "}$"))
                {
                    Response.Write("标题限" + ub.GetSub("BbsThreadMin", xmlPath2) + "-" + ub.GetSub("BbsThreadMax", xmlPath2) + "字<br/>");
                    Response.Write(Out.back("返回上一级"));
                    Response.Write("</body></html>");
                    Response.End();
                }
                Title = Title.Replace(char.ConvertFromUtf32(10), "").Replace(char.ConvertFromUtf32(13), "");
                Content = Utils.GetRequest("Content", "post", 1, "", "");
                if (!Utils.IsRegex(Content, @"^[\s\S]{" + ub.GetSub("BbsContentMin", xmlPath2) + "," + ub.GetSub("BbsContentMax", xmlPath2) + "}$"))
                {
                    Response.Write("请输入" + ub.GetSub("BbsContentMin", xmlPath2) + "-" + ub.GetSub("BbsContentMax", xmlPath2) + "字的内容<br/>");
                    Response.Write(Out.back("返回上一级"));
                    Response.Write("</body></html>");
                    Response.End();
                }

            }
            else
            {
                Content = Utils.GetRequest("Content", "post", 1, "", "");
                if (!Utils.IsRegex(Content, @"^[\s\S]{1," + ub.GetSub("BbsReplyMax", xmlPath2) + "}$"))
                {
                    Response.Write("请输入不超" + ub.GetSub("BbsReplyMax", xmlPath2) + "字的回帖内容<br/>");
                    Response.Write(Out.back("返回上一级"));
                    Response.Write("</body></html>");
                    Response.End();
                }
            }

            if (bid == 0)
            {

                int ThreadNum = Utils.ParseInt(ub.GetSub("BbsThreadNum", xmlPath));
                if (ThreadNum > 0)
                {
                    int ToDayCount = new BCW.BLL.Forumstat().GetCount(meid, 1);//今天发布帖子数
                    if (ToDayCount >= ThreadNum)
                    {
                        Response.Write("系统限每天每ID限发帖子" + ThreadNum + "帖<br/>");
                        Response.Write(Out.back("返回上一级"));
                        Response.Write("</body></html>");
                        Response.End();
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

                    strOut = "，部分文件超出今天上传数量导致上传失败";
                    kk = Math.Abs(kk);
                }
                //更新帖子文件数
                new BCW.BLL.Text().UpdateFileNum(bid, kk);
                //论坛统计
                BCW.User.Users.UpdateForumStat(1, meid, mename, forumid);
                //动态记录
                new BCW.BLL.Action().Add(meid, mename, "在" + Forummodel.Title + "发表了文件帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + Title + "[/URL]");
                      
                //积分操作/论坛统计/圈子论坛不进行任何奖励
                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Text, meid);
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
                Response.Write("上传" + kk + "个文件成功" + strOut + "");
                Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("forum.aspx?forumid=" + forumid + "")) + "\">返回之前页面</a>");
                Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回社区首页</a>");
                Response.Write("</body></html>");
                Response.End();
            }
            else
            {
                int ReplyNum = Utils.ParseInt(ub.GetSub("BbsReplyNum", xmlPath));
                if (ReplyNum > 0)
                {
                    int ToDayCount = new BCW.BLL.Forumstat().GetCount(meid, 2);//今天发布回帖数
                    if (ToDayCount >= ReplyNum)
                    {
                        Response.Write("系统限每天每ID限回帖" + ReplyNum + "次<br/>");
                        Response.Write(Out.back("返回上一级"));
                        Response.Write("</body></html>");
                        Response.End();

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
                //动态记录
                new BCW.BLL.Action().Add(meid, mename, "在" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/URL]");
                     
                //积分操作/论坛统计/圈子论坛不进行任何奖励
                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Reply, meid);
                }
                Response.Write("回复" + kk + "个文件成功！" + strOut + "" + PbCent + "");
                Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")) + "\">返回主题帖子</a>");
                Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")) + "\">进入回复列表</a>");
                Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回社区首页</a>");
                Response.Write("</body></html>");
                Response.End();
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
            for (int iFile = files.Count - 1; iFile > -1; iFile--)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                string UpExt = ub.GetSub("UpaFileExt", xmlPath);
                int UpLength = Convert.ToInt32(ub.GetSub("UpaMaxFileSize", xmlPath));
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
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
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
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        postedFile.SaveAs(SavePath);

                        //=============================图片木马检测,包括TXT===========================
                        string vSavePath = SavePath;
                        if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
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
                        int Verify = Utils.ParseInt(ub.GetSub("UpIsVerify", xmlPath));
                        //缩略图生成
                        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
                        {
                            int ThumbType = Convert.ToInt32(ub.GetSub("UpaThumbType", xmlPath));
                            int width = Convert.ToInt32(ub.GetSub("UpaWidth", xmlPath));
                            int height = Convert.ToInt32(ub.GetSub("UpaHeight", xmlPath));
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

                                            IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));
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
                                            if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg")
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