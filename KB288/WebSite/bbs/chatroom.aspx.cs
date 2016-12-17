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
using BCW.JS;
using BCW.HB;
using BCW.Files;
//2016/5/4DSY

/// <summary>
/// 蒙宗将 20160822 撤掉抽奖值生成
/// </summary>

public partial class bbs_chatroom : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/chat.xml";
    protected string xmlPath2 = "/Controls/upfile.xml";
    protected string xmlPath3 = "/Controls/chathb.xml";

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
        if (ub.GetSub("ChatStatus", xmlPath) == "1")
        {
            Utils.Safe("红包群系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        #region 是否超管
        string ChatHID = ub.GetSub("ChatHID", xmlPath);
        string[] sNum = Regex.Split(ChatHID, "#");
        int ISH = 0;
        for (int a = 0; a < sNum.Length - 1; a++)
        {
            if (new BCW.User.Users().GetUsId() == int.Parse(sNum[a].Trim()))
            {
                ISH++;
            }
        }
        #endregion
        #region 进入权限判断
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群还没有开通.."));
        BCW.Model.Chat chatmodel = new BCW.BLL.Chat().GetChat(id);
        if (chatmodel.Types == 3)
        {
            bool i_i = new BCW.HB.BLL.ChatMe().Exists(id, meid);

            if (i_i || chatmodel.UsID == meid)
            {
                if (i_i)
                {
                    BCW.HB.Model.ChatMe xyd = new BCW.HB.BLL.ChatMe().GetModel(id, meid);
                    if (xyd.State == 0)
                    {
                        new BCW.HB.BLL.ChatMe().Update2(id, meid);
                    }
                }
            }
            else
            {

                if (ISH == 0)
                {
                    Utils.Error("你非本红包群酷友，不能进入。", "");
                }
            }
        }
        #endregion
        switch (act)
        {
            case "dz":
                DZPage();
                break;
            case "bq":
                BQPage();
                break;
            case "bq2":
                BQ2Page("bq2");
                break;
            case "add":
                AddPage();
                break;
            case "action":
                ActionPage();
                break;
            case "fresh":
                FreshPage();
                break;
            case "rember":
                Rember();
                break;
            case "invi":
                InviPage();
                break;
            case "case":
                CasePage();
                break;
            case "top":
                TopPage();
                break;
            case "statinfo":
                StatInfoPage();
                break;
            case "posph":
                PostPhoto();
                break;
            case "suss":
                UploadPage();
                break;
            case "suss2":
                SubmitPage();
                break;
            case "Chat":
                ChatPage();
                break;
            default:
                ReloadPage(act);
                break;
        }
    }

    private void ReloadPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群还没有开通.."));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "90"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int puthb = int.Parse(Utils.GetRequest("puthb", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        BCW.Model.Group modelgr = null;
        if (model.GroupId > 0)
        {
            modelgr = new BCW.BLL.Group().GetGroupMe(model.GroupId);
            if (modelgr == null)
            {
                Utils.Error("不存在的圈子", "");
            }
            else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
            {
                Utils.Error("圈子已过期", "");
            }
            if (modelgr.ChatStatus == 1)
            {
                Utils.Error("圈聊已关闭", "");
            }
        }

        Master.Title = model.ChatName;
        //密码进入
        if (model.Types == 3 && new BCW.User.CLimits().GetChatLimit(meid, model) <= 0)
        {
            if (!string.IsNullOrEmpty(model.ChatPwd))
            {
                string PwdID = "#" + model.PwdID + "#";
                if (PwdID.IndexOf("#" + meid + "#") == -1)
                {
                    if (act == "pwdok")
                    {
                        string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[A-Za-z0-9]{1,20}$", "密码填写错误");
                        if (Pwd != model.ChatPwd)
                        {
                            Utils.Error("密码填写错误", "");
                        }
                        new BCW.BLL.Chat().UpdatePwdID(id, model.PwdID + "#" + meid + "");
                    }
                    else
                    {
                        new Out().head(Utils.ForWordType("温馨提示"));
                        Response.Write(Out.Tab("<div class=\"title\">", ""));
                        Response.Write("本红包群已加密，请输入密码进入");
                        Response.Write(Out.Tab("</div>", "<br />"));
                        strText = ",,,";
                        strName = "Pwd,id,act";
                        strType = "stext,hidden,hidden";
                        strValu = "'" + id + "'pwdok";
                        strEmpt = "true,false,false";
                        strIdea = "";
                        strOthe = "马上进入,chatroom.aspx,post,3,red";
                        Response.Write(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                        Response.Write(Out.Tab("<div>", "<br />"));
                        Response.Write("<a href=\"" + Utils.getUrl("chat.aspx") + "\">&gt;返回红包群大厅</a>");
                        Response.Write(Out.Tab("</div>", ""));
                        Response.Write(new Out().foot());
                        Response.End();
                    }
                }
            }
        }

        new BCW.User.CLimits().CheckChatRole(id, meid, 2);//被踢出权限

        if (act == "save")
        {
            string ac = Utils.ToSChinese(Utils.GetRequest("ac", "post", 1, "", ""));

            if (ac == "动作")
            {
                DZPage();
                return;
            }
            else if (ac == "普通表情")
            {
                BQ2Page("bq2");
                return;
            }
            else if (ac == "VIP表情")
            {
                BQPage();
                return;
            }
            else if (ac == "发图片")
            {
                PostPhoto();
                return;
            }
            else if (ac == "送鲜花")
            {
                Utils.Error("此功能稍后开放", "");
            }
            else
            {
                BCW.User.Users.ShowVerifyRole("g", meid);//非验证会员提示
                new BCW.User.CLimits().CheckChatRole(id, meid, 1);//被禁言权限
                int ContentNum = Convert.ToInt32(ub.GetSub("ChatContentNum", xmlPath));
                string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1," + ContentNum + "}$", "发言内容限1-" + ContentNum + "字内");
                int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[0-9]\d*$", "0"));
                int IsSms = int.Parse(Utils.GetRequest("IsSms", "post", 1, @"^[0-1]$", "0"));
                int aa = int.Parse(Utils.GetRequest("aa", "all", 1, @"^[1-9]\d*$", "0"));
                int IsKiss = 0;
                if (ac == "悄悄话" && ToID > 0)
                {
                    IsKiss = 1;
                }
                if (Content.Contains("(Wo)"))
                {
                    Content = Content.Replace("(Wo)", "$N").Replace("(Ta)", "$n");
                }
                if (aa > 0 && aa < 63)
                {

                    Content = "" + BCW.User.ChatAction.GetAction2(aa)[1].Replace("(xxx)", "$n") + "";

                }
                if (Content.ToUpper().Contains("[V]") && Content.Contains("[/V]"))
                {
                    int VipLeven = BCW.User.Users.VipLeven(meid);
                    if (VipLeven == 0)
                    {
                        Utils.Error("限VIP会员使用", "");

                    }
                }

                if (aa > 0 && aa < 63)
                {

                    //1分钟内限发动作一次
                    string appName = "LIGHT_CHATROOM";
                    string CacheKey = appName + "_" + Utils.Mid(Utils.getstrU(), 0, Utils.getstrU().Length - 4);
                    object getObjCacheTime = DataCache.GetCache(CacheKey);
                    if (getObjCacheTime != null)
                    {
                        Utils.Error("1分钟内限发动作1次", "");
                    }
                    object ObjCacheTime = DateTime.Now;
                    DataCache.SetCache(CacheKey, ObjCacheTime, DateTime.Now.AddSeconds(60), TimeSpan.Zero);

                }

                //是否刷屏
                string appName2 = "LIGHT_CHATROOM2";
                int Expir = Convert.ToInt32(ub.GetSub("ChatExpir", xmlPath));
                BCW.User.Users.IsFresh(appName2, Expir);
                string ToName = string.Empty;
                if (ToID > 0)
                {
                    ToName = new BCW.BLL.User().GetUsName(ToID);
                }
                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.ChatText addmodel = new BCW.Model.ChatText();
                addmodel.ChatId = id;
                addmodel.Content = Content;
                addmodel.UsID = meid;
                addmodel.UsName = mename;
                addmodel.ToID = ToID;
                addmodel.ToName = ToName;
                addmodel.IsKiss = IsKiss;
                addmodel.AddTime = DateTime.Now;
                //20161020 sgl 
                int countss = new BCW.BLL.ChatText().GetCount("ChatId=" + id + " and UsID=" + meid + " and datediff(day,AddTime,getdate())=0");//ChatId=" + id + " and datediff(day,AddTime,getdate())=0
                if (Content.Trim() != "")
                {

                    if (countss <= Convert.ToInt32(ub.GetSub("ChatMost", xmlPath)))
                    {
                        string chatname = new BCW.BLL.Chat().GetChatName(id);
                        //2016/4/22
                        if (id == 29)
                        {
                            if (aa > 0 && aa < 63)
                            {
                            }
                            else if (Content.Contains("[F]") || Content.Contains("[V]"))
                            {

                            }
                            else
                            {
                                new BCW.BLL.User().UpdateiGold(meid, mename, 1, chatname + "红包群发言奖币！");
                            }
                        }
                        else
                        {
                            //除系统聊天室外其他聊天室的发言奖币！
                        }

                    }
                    new BCW.HB.BLL.ChatMe().Update(id, meid, 2);
                    int chattextid = new BCW.BLL.ChatText().Add(addmodel);

                    if (aa > 0 && aa < 63)
                    {
                        new BCW.HB.BLL.ChatMe().UpdateChatTextType(chattextid);
                    }
                    if (Content.Contains("[F]") || Content.Contains("[V]"))
                    {
                        new BCW.HB.BLL.ChatMe().UpdateChatTextType(chattextid);
                    }
                }


                if (ToID > 0)
                {
                    //ChatOutCent(id, meid, mename);
                }

                if (IsSms == 1 && ToID > 0)
                {
                    new BCW.BLL.Guest().Add(4, ToID, ToName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在红包群对你发言:" + Content + "[url=/bbs/chatroom.aspx?act=add&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;uid=" + meid + "]马上回复[/url]");
                }

                //if (Utils.getPage(0) != "")
                //{

                if (id == 29 && countss <= Convert.ToInt32(ub.GetSub("ChatMost", xmlPath)))
                {
                    if (aa > 0 && aa < 63)
                    {
                    }
                    else if (Content.Contains("[F]") || Content.Contains("[V]"))
                    {
                    }
                    else
                    {
                        Utils.Success("发言", "发言成功，奖励1" + ub.Get("SiteBz") + "，正在返回..", Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn), "1");
                    }

                }
                //}
            }
        }
        if (tm > 0)
        {
            Master.Refresh = tm;
            Master.Gourl = Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "");
        }

        //红包群抽奖动态
        try
        {
            DateTime dt = Convert.ToDateTime(ub.GetSub("ChatCentTime", xmlPath));
            if (dt.AddMinutes(5) > DateTime.Now)
            {
                string OutText = ub.GetSub("ChatOutText", xmlPath);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(OutText));
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch { }
        if (hbgn == 1)
        {
            PostHb(id, meid, tm, pn);
        }
        if (!string.IsNullOrEmpty(model.ChatNotes))
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (model.Types == 0)
                builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.ChatNotes)));
            else
                builder.Append(Out.SysUBB(model.ChatNotes));

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
        }
        BCW.Model.Chat chatmodel = new BCW.BLL.Chat().GetChat(id);

        //固定BackUrl
        string BackUrl = Server.UrlEncode("/bbs/chatroom.aspx?id=" + id + "&tm=" + tm + "&pn=" + pn + "");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">发言</a> ");
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">对话</a> ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">普通</a> ");

        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admin&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;backurl=" + BackUrl + "") + "\">聊务</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;backurl=" + BackUrl + "") + "\">基金</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a> <br />");
        if (puthb == 1)
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;puthb=0&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">聊天</a> ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;puthb=1&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">红包</a> ");
        if (hbgn != 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;hb=" + 1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">显示</a> ");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">隐藏</a> ");
        }
        builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;id=" + id + "&amp;hb=" + hbgn) + "\">列表</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb&amp;hb=" + hbgn + "&amp;chatid=" + id) + "\">记录</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=posph&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">图片</a> ");
        builder.Append(Out.Tab("</div>", "<br />"));

        strText = ",,,,,,";
        strName = "Content,id,tm,pn,act,hb,backurl";
        strType = "text,hidden,hidden,hidden,hidden,hidden,hidden";
        strValu = "'" + id + "'" + tm + "'" + pn + "'save'" + hbgn + "'" + Utils.getPage(0) + "";
        strEmpt = "true,false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "快速发言,chatroom.aspx,post,3,red";

        if (puthb == 1)
        {

            putshb(id, meid, tm, pn);
        }
        else
        {

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
            if (ptype > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                if (ptype == 1)
                    builder.Append("全部|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">全部</a>|");

                if (ptype == 2)
                    builder.Append("我对别人|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">我对别人</a>|");

                if (ptype == 3)
                    builder.Append("别人对我");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">别人对我</a>");

                builder.Append(Out.Tab("</div>", "<br />"));
            }

            //抢币功能
            DateTime ChatCTime = model.ChatCTime;
            if (!string.IsNullOrEmpty(model.ChatCT) && model.ChatCon > 0)
            {
                if (model.ChatCTime.Equals(Convert.ToDateTime("1990-1-1")))
                {
                    ChatCTime = DateTime.Now.AddSeconds(model.ChatCon);
                    new BCW.BLL.Chat().UpdateChatCTime(id, ChatCTime);
                }
                builder.Append(Out.Tab("<div>", Out.RHr()));
                if (DateTime.Now > ChatCTime && ("#" + model.ChatCId + "#").Contains("#0#"))
                {
                    string CText = Utils.ConvertSeparated(model.ChatCT, 1, "#");
                    string[] CTemp = CText.Split("#".ToCharArray());
                    string[] CId = model.ChatCId.Split("#".ToCharArray());
                    for (int i = 1; i <= CTemp.Length; i++)
                    {
                        if (CId[i - 1] != "0")
                            builder.Append(CTemp[i - 1]);
                        else
                            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=case&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;num=" + i + "&amp;verify=" + new Rand().RandNumer(4) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + CTemp[i - 1] + "</a>");

                        if (i != CTemp.Length)
                            builder.Append(".");
                    }
                    //2016120dsy
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 1; i <= CTemp.Length; i++)
                    {
                        if (CId[i - 1] != "0")
                        {
                            DataSet Cmoney = new BCW.HB.BLL.HbPost().GetChatList(id);
                            string cmoneys = Cmoney.Tables[0].Rows[0][2].ToString();
                            string[] cmone = cmoneys.Split("#".ToCharArray());
                            string Cname = new BCW.BLL.User().GetUsName(Convert.ToInt32(CId[i - 1]));
                            if (i <= cmone.Length)
                            {
                                sb.Append("<br/><div style=\"color:#A8A8A8\">" + Cname + "抢到了'" + CTemp[i - 1] + "'字，获得" + cmone[i - 1] + ub.Get("SiteBz") + "</div>");
                            }
                            //builder.Append("<br/>"+ Cname + "抢到了'"+ CTemp[i - 1] + "'字，获得145酷币");
                        }
                    }
                    if (sb.Length != 0)
                    {
                        string sbsb = new BCW.JS.somejs().topfloat(sb.ToString(), "qb", "30", "130");
                        if (hbgn == 1)
                        {
                            builder.Append(sbsb);
                        }

                    }
                }
                else
                {
                    int Minute = model.ChatCTime.Minute;
                    if (60 - Convert.ToInt32(model.ChatCon / 60) < Minute)
                    {
                        builder.Append("大量");
                    }
                    builder.Append("" + ub.Get("SiteBz") + "生成中,请等待...(还有" + DT.DateDiff(ChatCTime, DateTime.Now, 4) + "秒)");
                }
                builder.Append(Out.Tab("</div>", Out.RHr()));
            }
            int pageIndex;
            int recordCount;
            int pageSize = pn;
            string strWhere = "";
            string[] pageValUrl = { "ptype", "hb", "id", "tm", "pn", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (ptype == 1)
                strWhere = "ChatId=" + id + " and (UsID=" + meid + " OR ToID=" + meid + ")";
            else if (ptype == 2)
                strWhere = "ChatId=" + id + " and UsID=" + meid + "";
            else if (ptype == 3)
                strWhere = "ChatId=" + id + " and ToID=" + meid + "";
            else
                strWhere = "ChatId=" + id + "";
            // 开始读取列表
            IList<BCW.Model.ChatText> listChatText = new BCW.BLL.ChatText().GetChatTexts(pageIndex, pageSize, strWhere, out recordCount);
            if (listChatText.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.ChatText n in listChatText)
                {

                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    n.UsName = BCW.User.Users.SetVipName(n.UsID, n.UsName, false);
                    n.ToName = BCW.User.Users.SetVipName(n.ToID, n.ToName, false);

                    int sexnum = new BCW.BLL.User().GetSex(n.UsID);
                    if (sexnum == 1)
                    {
                        builder.Append("♀");
                    }
                    else if (sexnum == 2)
                    {
                        builder.Append("♂");
                    }
                    //builder.Append((pageIndex - 1) * pageSize + k + ".");
                    // builder.AppendFormat("[{0}]", DT.FormatDate(n.AddTime, 13));
                    int Isac = 0;
                    string Content = n.Content.Trim();
                    if (Content.Contains("<img"))
                    {
                        Content = Content.Replace("hb=0", "hb=" + hbgn);
                    }

                    if (Content.Contains("$n"))
                    {
                        string sToName = n.ToName;
                        if (sToName == "")
                            sToName = "大家";

                        Isac = 1;
                        Content = Content.Replace("$N", n.UsName).Replace("$n", sToName);
                    }
                    if (n.UsID == 0)
                    {
                        builder.Append("[" + n.UsName + "]" + Out.SysUBB(Content) + "");
                    }
                    else
                    {
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;uid={0}&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}</a>", n.UsID, n.UsName);

                        if (n.IsKiss == 1)
                        {
                            if (n.UsID == meid || n.ToID == meid)
                            {
                                if (Isac == 0)
                                    builder.AppendFormat("对<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;uid={0}&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}</a>说", n.ToID, n.ToName);
                                //builder.AppendFormat("对{0}说", n.ToName);
                                else
                                    builder.AppendFormat("对{0}", n.ToName);

                                builder.AppendFormat(":*{0}", Out.SysUBB(Content));
                            }
                            else
                            {
                                builder.AppendFormat("对{0}说:悄悄话..", n.ToName);
                            }
                        }
                        else
                        {
                            if (n.ToID == 0)
                            {
                                builder.AppendFormat(":{0}", Out.SysUBB(Content));
                            }
                            else
                            {
                                if (Isac == 0)
                                    builder.AppendFormat("对<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;uid={0}&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}</a>说", n.ToID, n.ToName);
                                // builder.AppendFormat("对{0}说", n.ToName);
                                else
                                    builder.AppendFormat("对{0}", n.ToName);

                                builder.AppendFormat(":{0}", Out.SysUBB(Content));

                            }
                        }
                        builder.AppendFormat("[{0}]", DT.FormatDate(n.AddTime, 13));
                    }
                    if (new BCW.User.CLimits().GetChatLimit(meid, model) > 0 && n.UsID > 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=checkadmin&amp;uid=" + n.UsID + "&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;backurl=" + BackUrl + "") + "\">[管]</a>");

                        //builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=checkout&amp;uid=" + n.UsID + "&amp;id=" + id + "&amp;backurl=" + BackUrl + "") + "\">[踢]</a>");
                        //builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=checkstop&amp;uid=" + n.UsID + "&amp;id=" + id + "&amp;backurl=" + BackUrl + "") + "\">[禁]</a>");
                        //builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=checkdel&amp;uid=" + n.UsID + "&amp;id=" + id + "&amp;backurl=" + BackUrl + "") + "\">[删]</a>");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }


            //更新红包群在线人数
            if (pageIndex == 1)
            {
                int chatId = new BCW.BLL.User().GetEndChatID(meid);
                if (chatId != id)
                {
                    //记录新进来的ID
                    //BCW.Model.ChatText addmodel = new BCW.Model.ChatText();
                    //addmodel.ChatId = id;
                    //addmodel.Content = "进入了红包群";
                    //addmodel.UsID = meid;
                    //addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                    //addmodel.ToID = 0;
                    //addmodel.ToName = "";
                    //addmodel.IsKiss = 0;
                    //addmodel.AddTime = DateTime.Now;
                    //new BCW.BLL.ChatText().Add(addmodel);
                    //进入新的红包群则之前红包群在线人数减1
                    new BCW.BLL.Chat().UpdateLine(chatId);
                }
                new BCW.BLL.User().UpdateEndChatID(meid, id);
                int ChatLine = new BCW.BLL.User().GetChatNum(id);
                if (ChatLine != model.ChatOnLine)
                    new BCW.BLL.Chat().UpdateLine(id, ChatLine);
            }
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));

        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">发言</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=fresh&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设置</a> ");
        if (model.Types == 3)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=rember&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">成员</a> ");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">分享</a> ");
        }
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=statinfo&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">统计</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=top&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + BackUrl + "") + "\">排行</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=exit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">离开</a> ");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (!string.IsNullOrEmpty(model.ChatFoot))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(model.ChatFoot));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=online&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本红包群在线(" + model.ChatOnLine + ")</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">红包群大厅</a>-<a href=\"" + Utils.getPage("chat.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        //------------------------//2016/3/1---------------------------------------//
        //更新红包群时间及过期红包
        DataSet Types = new BCW.HB.BLL.HbPost().GetChatList(id);
        object Type = Types.Tables[0].Rows[0][1];
        if (Type.ToString().Trim() != "0")
        {
            new BCW.BLL.Chat().UpdateExitTime(id, DateTime.Now.AddDays(30));
        }
        int HbTime = Utils.ParseInt(ub.GetSub("HbTime", xmlPath3));
        List<BCW.HB.Model.HbPost> hbpos = new BCW.HB.BLL.HbPost().GetModelList("State!=2");
        for (int iso = 0; iso < hbpos.Count; iso++)
        {
            if (hbpos[iso].PostTime.AddDays(HbTime) < DateTime.Now)
            {
                BCW.HB.Model.HbPost hbpo = new BCW.HB.BLL.HbPost().GetModel(hbpos[iso].ID);
                string ptname = new BCW.BLL.User().GetUsName(hbpo.UserID);
                if (hbpo.RadomNum.Trim() != "pt")
                {
                    MatchCollection leyr = Regex.Matches(hbpo.GetIDList, "#");
                    int ele = leyr.Count;
                    string[] symoney = hbpo.RadomNum.Split('#');
                    long symoneys = 0;
                    for (int ii = 0; ii < ele; ii++)
                    {
                        symoneys = symoneys + Convert.ToInt64(symoney[ii]);
                    }
                    long thmoney = hbpo.money - symoneys;
                    if (thmoney > 0)
                    {
                        new BCW.BLL.Guest().Add(1, hbpo.UserID, ptname, "您在" + hbpo.ChatID + "红包群" + hbpo.PostTime + "时候发的红包已过期！退回金额" + thmoney + ub.Get("SiteBz"));

                        new BCW.BLL.User().UpdateiGold(hbpo.UserID, ptname, thmoney, "红包" + hbpo.ID + "过期退回收入");
                    }
                }
                else
                {
                    MatchCollection leyr = Regex.Matches(hbpo.GetIDList, "#");
                    int ele = leyr.Count;
                    long thmoney = hbpo.money * (hbpo.num - ele);
                    if (thmoney > 0)
                    {
                        new BCW.BLL.Guest().Add(1, hbpo.UserID, ptname, "您在" + hbpo.ChatID + "红包群" + hbpo.PostTime + "时候发的红包已过期！退回金额" + thmoney + ub.Get("SiteBz"));
                        new BCW.BLL.User().UpdateiGold(hbpo.UserID, ptname, thmoney, "红包" + hbpo.ID + "过期退回收入");
                    }
                }
                new BCW.HB.BLL.HbPost().UpdateState(hbpos[iso].ID, 2);
            }
        }
    }
    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "红包群ID错误"));
        string ff = Utils.GetRequest("ff", "get", 1, @"^(v)?[0-9]\d*$", "");
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "10"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
        int aa = int.Parse(Utils.GetRequest("aa", "all", 1, @"^[1-9]\d*$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "" + model.ChatName + "发言";
        builder.Append(Out.Tab("<div class=\"title\">红包群发言</div>", ""));
        string Copytemp = string.Empty;
        if (ff != "")
        {
            if (ff.Contains("v"))
                Copytemp += "[V]" + ff + "[/V]";
            else
                Copytemp += "[F]" + ff + "[/F]";
        }
        if (aa > 0 && aa < 63)
        {
            if (uid == 0)
                Copytemp += "" + BCW.User.ChatAction.GetAction(aa)[1].Replace("$N", "(Wo)").Replace("$n", "(Ta)") + "";
            else if (uid != meid)
                Copytemp += "" + BCW.User.ChatAction.GetAction(aa)[2].Replace("$N", "(Wo)").Replace("$n", "(Ta)") + "";
            else if (uid == meid)
                Copytemp += "" + BCW.User.ChatAction.GetAction(aa)[3].Replace("$N", "(Wo)").Replace("$n", "(Ta)") + "";
        }
        if (uid == 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("发言内容(500字内):");
            builder.Append(Out.Tab("</div>", ""));
            string strSpeaker = string.Empty;
            DataSet ds = new BCW.BLL.User().GetList("TOP 10 ID,UsName", "EndChatID=" + id + " ORDER BY EndTime DESC");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strSpeaker += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + Out.UBB(ds.Tables[0].Rows[i]["UsName"].ToString()) + "";
                }
            }
            strSpeaker = "0|大家" + strSpeaker;
            strText = ",发言对象:/,,,,,,";
            strName = "Content,ToID,id,tm,pn,act,hb,backurl";
            strType = "text,select,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = "" + Copytemp + "'0'" + id + "'" + tm + "'" + pn + "'save'" + hbgn + "'" + Utils.getPage(0) + "";
            strEmpt = "true," + strSpeaker + ",false,false,false,false,false,false";
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));

            builder.Append("我对<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(uid) + "</a>说:");

            builder.Append(Out.Tab("</div>", ""));
            strText = "内容(50字内):/,内线通知:,,,,,,,";
            strName = "Content,IsSms,ToID,id,tm,pn,act,hb,backurl";
            strType = "text,select,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = "" + Copytemp + "'0'" + uid + "'" + id + "'" + tm + "'" + pn + "'save'" + hbgn + "'" + Utils.getPage(0) + "";
            strEmpt = "true,0|否|1|是,false,false,false,false,false,false,false";
        }
        strIdea = "/";
        strOthe = "确定发言|悄悄话|送鲜花/|动作|普通表情|VIP表情,chatroom.aspx,post,1,red|blue|other|other|other|other";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【常用动作】");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=1") + "\">求爱</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=2") + "\">沉思</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=3") + "\">痴缠</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=4") + "\">马桶</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=5") + "\">喝酒</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=6") + "\">刀劈</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=7") + "\">错爱</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=8") + "\">亲吻</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=9") + "\">招呼</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid + "&amp;aa=10") + "\">拔牙</a> ");
        builder.Append(Out.Tab("</div>", ""));
        if (meid != uid && uid != 0)
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=Chat&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + uid) + "\">与" + new BCW.BLL.User().GetUsName(uid) + "的聊天记录</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        //builder.Append("选择:<a href=\"" + Utils.getUrl("function.aspx?act=face2&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a>.");
        //builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=action&amp;backurl=" + Utils.PostPage(1) + "") + "\">动作</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">返回房间</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ActionPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "选择动作";
        Master.IsFoot = false;
        builder.Append(Out.Tab("<div class=\"title\">选择动作</div>", ""));
        string gUrl = Server.UrlDecode(Utils.getPage(0));
        gUrl = gUrl.Replace("&amp;", "&");
        gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}aa=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
        gUrl = Out.UBB(gUrl);

        int pageIndex;
        int recordCount;
        int pageSize = 20;
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        recordCount = 54;
        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        builder.Append(Out.Tab("<div>", ""));
        for (int i = 1; i <= recordCount; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                builder.Append("<a href=\"" + Utils.getUrl(gUrl + "&amp;aa=" + i + "") + "\">" + BCW.User.ChatAction.GetAction(i)[0] + "</a>\r\n");
                if ((k + 1) > 0 && (k + 1) != 20 && (k + 1) != 40 && (k + 1) % 5 == 0)
                    builder.Append("<br />");
            }
            if (k == endIndex)
                break;
            k++;
        }
        builder.Append(Out.Tab("</div>", ""));
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chat.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 刷新设置
    /// </summary>
    private void FreshPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "30"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[0-9]\d*$", "10"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[0-9]\d*$", "10"));
        Master.Title = "刷新设置";
        builder.Append(Out.Tab("<div class=\"title\">刷新设置</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("选择秒数:");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",页面条数:/,,,";
        strName = "tm,pn,id,act,hb";
        strType = "select,select,hidden,hidden,hidden";
        strValu = "" + tm + "'" + pn + "'" + id + "'savefresh'" + hbgn;
        strEmpt = "0|手动|5|5秒|10|10秒|15|15秒|20|20秒|30|30秒|40|40秒|50|50秒|60|60秒|90|90秒,6|6条|8|8条|10|10条|15|15条|20|20条|25|25条|30|30条,,,";
        strIdea = "/";
        strOthe = "确定,chatroom.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 成员
    /// </summary>
    private void Rember()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "30"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[0-9]\d*$", "10"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[0-9]\d*$", "10"));
        if (id == 29)
        {
            Utils.Error("ID错误", "");
        }
        BCW.Model.Chat ZC = new BCW.BLL.Chat().GetChat(id);
        Master.Title = ZC.ChatName + "全部群成员";
        builder.Append(Out.Tab("<div class=\"title\">" + ZC.ChatName + "全部群成员</div>", ""));
        string[] pageValUrl = { "act", "ips", "id", "hb", "tm", "pn", "backurl" };
        int pageSize = pn;
        int pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        List<BCW.HB.Model.ChatMe> member = new BCW.HB.BLL.ChatMe().GetModelList("ChatID=" + id + " and State!=0");
        int recordCount = member.Count;
        int jj = pageIndex * pageSize;
        if (pageIndex * pageSize > recordCount)
        {
            jj = recordCount;
        }

        string zname = new BCW.BLL.User().GetUsName(ZC.UsID);
        string zsrc = new BCW.BLL.User().GetPhoto(ZC.UsID);
        string zimgclass = "width:43px;height:43px;border-radius:50px;";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img  style=\"" + zimgclass + "\" src=\"" + zsrc + "\" alt=\"load\"/>");
        builder.AppendFormat("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;uid={0}&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}</a><b>[群主]</b>", ZC.UsID, zname);
        builder.Append(Out.Tab("</div>", "<br/>"));
        for (int i = (pageIndex - 1) * pageSize; i < jj; i++)
        {
            if (i % 2 == 0)
                builder.Append(Out.Tab("<div class=\"text\">", ""));
            else
                builder.Append(Out.Tab("<div>", ""));
            string name = new BCW.BLL.User().GetUsName(member[i].UserID);
            string src = new BCW.BLL.User().GetPhoto(member[i].UserID);
            string imgclass = "width:43px;height:43px;border-radius:50px;";
            builder.Append("<img  style=\"" + imgclass + "\" src=\"" + src + "\" alt=\"load\"/>");
            builder.AppendFormat("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;uid={0}&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}</a>", member[i].UserID, name);
            if (member[i].score == 0)
            {
                builder.Append("<新人>");
            }
            else
            {

                BCW.Model.Chat gl = new BCW.BLL.Chat().GetChat(id);
                #region 职位输出
                int mid = member[i].UserID;
                string[] sNum = Regex.Split(gl.ChatSZ, "#");
                string[] sNum2 = Regex.Split(gl.ChatJS, "#");
                string[] sNum3 = Regex.Split(gl.ChatLG, "#");
                try
                {
                    for (int a = 0; a < sNum.Length; a++)
                    {
                        if (mid == int.Parse(sNum[a].Trim()))
                        {
                            builder.Append("<b>[室主]</b>");
                        }
                    }
                }
                catch
                { }
                try
                {
                    for (int a = 0; a < sNum2.Length; a++)
                    {
                        if (mid == int.Parse(sNum2[a].Trim()))
                        {
                            builder.Append("<b>[见习室主]</b>");
                        }
                    }
                }
                catch
                { }
                try
                {
                    for (int a = 0; a < sNum3.Length; a++)
                    {
                        if (mid == int.Parse(sNum3[a].Trim()))
                        {
                            builder.Append("<b>[临管]</b>");
                        }
                    }
                }
                catch
                { }

                #endregion
            }

            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">返回房间</a>--");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=invi&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">邀请新成员>>></a> ");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 邀请
    /// </summary>
    private void InviPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "30"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[0-9]\d*$", "10"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[0-9]\d*$", "0"));
        string Content = Utils.GetRequest("Content", "post", 1, @"^[^\^]{1,50}$", "");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        Master.Title = "邀请新成员";
        builder.Append(Out.Tab("<div class=\"title\">邀请新成员</div>", ""));
        int invit = new BCW.HB.BLL.ChatMe().GetInvite(id);
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        #region 邀请权限判断
        if (invit == 0)
        {
            if (meid != model.UsID)
            {
                Utils.Error("您不是群主，没有邀请权限！", "");
            }
        }
        else if (invit == 1)
        {
            string[] sNum = Regex.Split(model.ChatSZ, "#");
            int sz = 0;
            try
            {
                for (int a = 0; a < sNum.Length; a++)
                {
                    if (meid == int.Parse(sNum[a].Trim()))
                    {
                        sz++;
                    }
                }
            }
            catch
            {

            }
            if (meid != model.UsID && sz == 0)
            {
                Utils.Error("您不是群主或室主，没有邀请权限！", "");
            }
        }
        else if (invit == 2)
        {
            string[] sNum = Regex.Split(model.ChatSZ, "#");
            int sz = 0;
            try
            {
                for (int a = 0; a < sNum.Length; a++)
                {
                    if (meid == int.Parse(sNum[a].Trim()))
                    {
                        sz++;
                    }
                }
            }
            catch
            {

            }

            string[] sNum2 = Regex.Split(model.ChatJS, "#");
            int jxsz = 0;
            try
            {
                for (int a = 0; a < sNum2.Length; a++)
                {
                    if (meid == int.Parse(sNum2[a].Trim()))
                    {
                        jxsz++;
                    }
                }
            }
            catch
            {

            }

            if (meid != model.UsID && sz == 0 && jxsz == 0)
            {
                Utils.Error("您不是群主、室主或见习室主，没有邀请权限！", "");
            }
        }
        else if (invit == 3)
        {
            string[] sNum = Regex.Split(model.ChatSZ, "#");
            int sz = 0;
            try
            {
                for (int a = 0; a < sNum.Length; a++)
                {
                    if (meid == int.Parse(sNum[a].Trim()))
                    {
                        sz++;
                    }
                }
            }
            catch
            {

            }

            string[] sNum2 = Regex.Split(model.ChatJS, "#");
            int jxsz = 0;
            try
            {
                for (int a = 0; a < sNum2.Length; a++)
                {
                    if (meid == int.Parse(sNum2[a].Trim()))
                    {
                        jxsz++;
                    }
                }
            }
            catch
            {

            }

            string[] sNum3 = Regex.Split(model.ChatLG, "#");
            int lg = 0;
            try
            {
                for (int a = 0; a < sNum3.Length; a++)
                {
                    if (meid == int.Parse(sNum3[a].Trim()))
                    {
                        lg++;
                    }
                }
            }
            catch
            {

            }

            if (meid != model.UsID && sz == 0 && jxsz == 0 && lg == 0)
            {
                Utils.Error("您不是群主、室主、见习室主或临管，没有邀请权限！", "");
            }
        }
        #endregion
        string hold = Utils.ToSChinese(Utils.GetRequest("hold", "all", 1, "", "1"));
        string delete = Utils.ToSChinese(Utils.GetRequest("delete", "all", 1, "", "1"));
        string share = Utils.ToSChinese(Utils.GetRequest("share", "all", 1, "", "1"));
        String toidnum = Utils.GetRequest("toidnum", "post", 1, @"^[1-9]\d*$", "0");
        string idlist = Utils.GetRequest("idlist", "all", 1, "", "1");
        int deleteid = int.Parse(Utils.GetRequest("deleteid", "all", 1, "", "1"));
        String VE;
        String SID;
        if (delete == "删" || delete == "取消")
        {

            BCW.HB.Model.Shared shaxp3 = new BCW.HB.Model.Shared();
            BCW.HB.Model.Shared shadu = new BCW.HB.BLL.Shared().GetModel(meid);
            shaxp3.SharedIDList = "";
            string[] delelist = shadu.SharedIDList.Split(',');
            for (int j = 0; j < delelist.Length; j++)
            {
                if (delelist[j] != deleteid.ToString())
                {
                    if (j != 0)
                    {
                        shaxp3.SharedIDList = shaxp3.SharedIDList + "," + delelist[j];
                    }
                    else
                    {
                        shaxp3.SharedIDList = delelist[j];
                    }
                }
            }
            shaxp3.UserID = meid;
            shaxp3.ShareUrl = shadu.ShareUrl;
            shaxp3.ShareContent = shadu.ShareContent;
            new BCW.HB.BLL.Shared().Update(shaxp3);
        }
        string getPage = Utils.getPage(1);
        if (getPage == "")
        {
            BCW.HB.Model.Shared getpa = new BCW.HB.BLL.Shared().GetModel(meid);
            getPage = getpa.ShareContent;
        }
        else
        {
            bool exitse5 = new BCW.HB.BLL.Shared().Exists(meid);
            BCW.HB.Model.Shared shaxp = new BCW.HB.Model.Shared();
            if (exitse5)
            {
                shaxp.UserID = meid;
                shaxp.SharedIDList = "";
                shaxp.ShareUrl = "";
                shaxp.ShareContent = getPage;
                new BCW.HB.BLL.Shared().Update(shaxp);
            }
            else
            {
                shaxp.UserID = meid;
                shaxp.SharedIDList = "";
                shaxp.ShareUrl = "";
                shaxp.ShareContent = getPage;
                new BCW.HB.BLL.Shared().Add(shaxp);
            }
        }
        string purl = Utils.GetRequest("purl", "post", 1, "", "");

        if (purl == "")
        {
            string Purl = Out.UBB(Utils.removeUVe(getPage));
            string Purls = "http://" + Utils.GetDomain() + "" + Purl + "";
            string Title = new BCW.BLL.Chat().GetChatName(id);
            Title = Utils.GetTitle(Title);
            purl = "[url=/bbs/chatroom.aspx?id=" + id + "]" + Title + "[/url]";
        }

        if (share == "立即邀请")
        {
            string[] toidlist = idlist.Split(',');
            int[] Toids = new int[toidlist.Length];
            int sum = 0;
            for (int i = 0; i < toidlist.Length; i++)
            {
                try
                {
                    Toids[i] = Convert.ToInt32(toidlist[i]);
                    if (Toids[i] == 0)
                    {
                        Utils.Error("收信ID错误", "");
                    }
                    if (!new BCW.BLL.User().Exists(Toids[i]))
                    {
                        Utils.Error("不存在的收信ID" + Toids[i], "");
                    }

                    //你是否是对方的黑名单
                    if (new BCW.BLL.Friend().Exists(Toids[i], meid, 1))
                    {
                        Utils.Error("对方已把您加入黑名单", "");
                    }
                    //对方是否拒绝接收分享内线
                    string ForumSet = new BCW.BLL.User().GetForumSet(Toids[i]);
                    int Nore = BCW.User.Users.GetForumSet(ForumSet, 14);
                    if (Nore == 1)
                    {
                        Utils.Error("对方已设置拒绝接收分享内线", "");
                    }
                    string UsName = new BCW.BLL.User().GetUsName(meid);
                    BCW.Model.Guest model2 = new BCW.Model.Guest();
                    model2.FromId = 0;
                    model2.FromName = UsName;
                    model2.ToId = Toids[i];
                    model2.ToName = new BCW.BLL.User().GetUsName(Toids[i]);
                    model2.Content = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + UsName + "[/url]邀请您加入" + purl + "：" + Content + "";
                    model2.TransId = 0;
                    //添加数据
                    bool ex = saveMe(id, Toids[i]);
                    new BCW.BLL.Guest().Add(model2);
                    new BCW.BLL.Friend().UpdateTime(meid, Toids[i]);
                    sum++;
                }
                catch
                {

                }


            }
            BCW.HB.Model.Shared share2 = new BCW.HB.BLL.Shared().GetModel(meid);
            BCW.HB.Model.Shared snull = new BCW.HB.Model.Shared();
            snull.UserID = share2.UserID;
            snull.SharedIDList = "";
            snull.ShareUrl = share2.ShareUrl;
            snull.ShareContent = share2.ShareContent;
            new BCW.HB.BLL.Shared().Update(snull);
            Utils.Success("邀请好友", "邀请" + sum + "位好友成功，正在返回..", Utils.getUrl("chatroom.aspx?act=rember&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        int pageIn = int.Parse(Utils.GetRequest("pageIn", "all", 1, @"^[0-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "act", "ab", "id", "ac", "ptt", "getPage", "backurl" };//分页回传
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (delete == "删" || delete == "取消")
        {
            pageIndex = pageIn;
        }
        if (hold == "选择")
        {
            int exnum = int.Parse(toidnum);
            bool exo = new BCW.BLL.User().Exists(exnum);
            if (exo == false)
            {
                Utils.Error("不存在的ID或输入无效！", "");
            }
            pageIndex = pageIn;
            bool exitse = new BCW.HB.BLL.Shared().Exists(meid);
            BCW.HB.Model.Shared sha = new BCW.HB.Model.Shared();
            if (exitse)
            {
                BCW.HB.Model.Shared shaex = new BCW.HB.BLL.Shared().GetModel(meid);
                sha.UserID = meid;
                if (shaex.SharedIDList.Trim() != "")
                {
                    #region 去重
                    sha.SharedIDList = shaex.SharedIDList + "," + toidnum;
                    string[] stringArray = sha.SharedIDList.Split(',');
                    List<string> listString = new List<string>();
                    foreach (string eachString in stringArray)
                    {
                        if (!listString.Contains(eachString))
                            listString.Add(eachString);
                    }
                    string SharedIDList2 = "";
                    foreach (string eachString in listString)
                    {
                        SharedIDList2 = SharedIDList2 + "," + eachString;
                    }
                    string[] SharedIDList3 = SharedIDList2.Split(',');
                    string SharedIDList4 = "";
                    for (int i = 0; i < SharedIDList3.Length; i++)
                    {
                        if (SharedIDList3[i].Trim() != "")
                        {
                            if (i != 1)
                            {
                                SharedIDList4 = SharedIDList4 + "," + SharedIDList3[i];
                            }
                            else
                            {
                                SharedIDList4 = SharedIDList3[i];
                            }
                        }
                    }
                    #endregion
                    sha.SharedIDList = SharedIDList4;
                }
                else
                {
                    sha.SharedIDList = toidnum;
                }
                sha.ShareUrl = purl;
                sha.ShareContent = getPage;
                if (toidnum.Trim() != "")
                {
                    new BCW.HB.BLL.Shared().Update(sha);
                }
            }
            else
            {
                sha.UserID = meid;
                sha.SharedIDList = toidnum;
                sha.ShareUrl = purl;
                sha.ShareContent = getPage;

                new BCW.HB.BLL.Shared().Add(sha);
            }
        }
        //查询条件
        strWhere = "UsID=" + meid + " and Types=0";
        // 开始读取列表
        IList<BCW.Model.Friend> listFriend = new BCW.BLL.Friend().GetFriends(pageIndex, pageSize, strWhere, out recordCount);
        if (listFriend.Count > 0)
        {
            int k = 1;
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<form id=\"forms\" method=\"post\" action=\"chatroom.aspx\">");
            builder.Append("<input type=\"num\" name=\"toidnum\" value=\"\" />");
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"invi\"/>");
            builder.Append("<input type=\"hidden\" name=\"ac\" value=\"分享\"/>");
            builder.Append("<input type=\"hidden\" name=\"pageIn\" value=\"" + pageIndex + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"getPage\" Value=\"" + getPage + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"id\" Value=\"" + id + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input  name=\"hold\" class=\"btn-red\" type=\"submit\" value=\"选择\"/>");
            builder.Append("</form>");
            builder.Append(Out.Tab("</div>", ""));
            foreach (BCW.Model.Friend n in listFriend)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", ""));
                }

                string sFriendName = n.FriendName;
                string nFriendName = new BCW.BLL.User().GetUsName(n.FriendID);
                if (sFriendName != nFriendName)
                    sFriendName = n.FriendName + "(" + nFriendName + ")";
                builder.Append("<form id=\"form" + n.FriendID + "\" method=\"post\" action=\"chatroom.aspx\">");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + sFriendName + "</a>(" + n.FriendID + ")");
                builder.Append("<input type=\"hidden\" name=\"toidnum\" value=\"" + n.FriendID + "\" />");
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"invi\"/>");
                builder.Append("<input type=\"hidden\" name=\"pageIn\" value=\"" + pageIndex + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"getPage\" Value=\"" + getPage + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input type=\"hidden\" name=\"id\" Value=\"" + id + "\"/>");
                #region 判断是否存在
                bool exitse10 = new BCW.HB.BLL.Shared().Exists(meid);
                if (exitse10)
                {
                    BCW.HB.Model.Shared shaexo = new BCW.HB.BLL.Shared().GetModel(meid);
                    string[] pdid = shaexo.SharedIDList.Split(',');
                    bool cz = false;
                    for (int ii = 0; ii < pdid.Length; ii++)
                    {
                        if (pdid[ii].Trim() == n.FriendID.ToString())
                        {
                            cz = true;
                        }
                    }
                    if (cz == false)
                    {
                        builder.Append("<input  name=\"hold\" type=\"submit\" value=\"选择\"/>");
                    }
                    else
                    {
                        builder.Append("<input type=\"hidden\" name=\"deleteid\" Value=\"" + n.FriendID + "\"/>");
                        builder.Append("<input  name=\"delete\" type=\"submit\" value=\"取消\"/>");
                    }
                }
                else
                {
                    builder.Append("<input  name=\"hold\" type=\"submit\" value=\"选择\"/>");
                }
                #endregion
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<b>已选择的好友:</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            bool exitse2 = new BCW.HB.BLL.Shared().Exists(meid);
            if (exitse2)
            {
                BCW.HB.Model.Shared yxshare = new BCW.HB.BLL.Shared().GetModel(meid);
                string[] xzs = yxshare.SharedIDList.Split(',');
                string fxname = "";
                for (int i = 0; i < xzs.Length; i++)
                {
                    if (xzs[i].Trim() != "")
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
                        int xzs2 = Convert.ToInt32(xzs[i]);
                        fxname = new BCW.BLL.User().GetUsName(xzs2);
                        builder.Append("<form id=\"form10" + i + "\" method=\"post\" action=\"chatroom.aspx\">");
                        builder.Append("<input type=\"hidden\" name=\"deleteid\" Value=\"" + xzs2 + "\"/>");
                        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"invi\"/>");
                        builder.Append("<input type=\"hidden\" name=\"pageIn\" value=\"" + pageIndex + "\"/>");
                        builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
                        builder.Append("<input type=\"hidden\" name=\"getPage\" Value=\"" + getPage + "\"/>");
                        builder.Append("<input type=\"hidden\" name=\"id\" Value=\"" + id + "\"/>");
                        VE = ConfigHelper.GetConfigString("VE");
                        SID = ConfigHelper.GetConfigString("SID");
                        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                        builder.Append(fxname + "(" + xzs2 + ")");
                        builder.Append("<input name=\"delete\" type=\"submit\" value=\"删\"/><br />");
                        builder.Append("</form>");
                        builder.Append(Out.Tab("</div>", ""));
                    }

                }
            }

            // 分页
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        BCW.HB.Model.Shared tjshare = new BCW.HB.BLL.Shared().GetModel(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<form id=\"form10\" method=\"post\" action=\"chatroom.aspx\">");
        builder.Append("<input type=\"hidden\" name=\"idlist\" Value=\"" + tjshare.SharedIDList + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"invi\"/>");
        builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
        builder.Append("<input type=\"hidden\" name=\"id\" Value=\"" + id + "\"/>");
        builder.Append("<b>附言:</b><br />");
        builder.Append("<input type=\"text\" name=\"Content\" Value=\"\"/><br />");
        VE = ConfigHelper.GetConfigString("VE");
        SID = ConfigHelper.GetConfigString("SID");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append("<input class=\"btn-blue\"  name=\"share\" type=\"submit\" value=\"立即邀请\"/>");
        builder.Append("</form>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">返回房间</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=rember&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级</a> ");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 顶部抢币
    /// </summary>
    private void CasePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        //验证防刷
        string verify = Utils.GetRequest("verify", "get", 2, @"^[0-9]\d*$", "验证码错误");
        string meverify = new BCW.BLL.User().GetVerifys(meid);
        if (!string.IsNullOrEmpty(meverify))
        {
            if (verify.Equals(meverify))
            {
                Utils.Error("验证码有误,请进入红包群抢币", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn));
            }
        }
        //更新验证码
        new BCW.BLL.User().UpdateVerifys(meid, verify);

        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn));
        }
        if (string.IsNullOrEmpty(model.ChatCT))
        {
            Utils.Error("此红包群还没有开通抢币", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn));
        }
        if (model.ChatCTime > DateTime.Now)
        {
            Utils.Error("此轮已经结束,请等待下轮抢币", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn));
        }
        string[] CTemp = model.ChatCId.Split("#".ToCharArray());
        if (CTemp.Length <= (num - 1))
        {
            Utils.Error("不存在的抢币记录", Utils.getPage("chatroom.aspx?id=" + id + ""));
        }
        if (("#" + model.ChatCId + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你已成功抢过一次了，请等待下一轮抢币", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn));
        }
        if (CTemp[num - 1].ToString() != "0")
        {
            Utils.Error("你晚了一步,此币已经被人抢走了", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn));
        }
        string snum = string.Empty;
        for (int i = 1; i <= CTemp.Length; i++)
        {
            if (i == num)
                snum += "#" + meid;
            else
                snum += "#" + CTemp[i - 1];
        }
        snum = Utils.Mid(snum, 1, snum.Length);


        //得到抢到的字
        string CText = Utils.ConvertSeparated(model.ChatCT, 1, "#");
        string[] strCTemp = CText.Split("#".ToCharArray());
        string ctext = strCTemp[num - 1];
        if (!("#" + snum + "#").Contains("#0#"))
        {
            //抢币标识
            string sCTemp = string.Empty;
            for (int i = 0; i < strCTemp.Length; i++)
            {
                sCTemp += "#0";
            }
            sCTemp = Utils.Mid(sCTemp, 1, sCTemp.Length);
            new BCW.BLL.Chat().UpdateCb(id, sCTemp, DateTime.Now.AddSeconds(model.ChatCon));
        }
        else
        {
            new BCW.BLL.Chat().UpdateChatCId(id, snum);
        }

        int Bb = 1;
        int Minute = model.ChatCTime.Minute;
        if (60 - Convert.ToInt32(model.ChatCon / 60) < Minute)
        {
            //整点奖币
            if (Utils.IsRegex(model.ChatCbig, @"^[1-9]\d*-[1-9]\d*$"))
            {
                string[] Cbig = model.ChatCbig.Split("-".ToCharArray());
                Bb = new Random().Next(Convert.ToInt32(Cbig[0]), Convert.ToInt32(Cbig[1]) + 1);
            }
        }
        else
        {
            //非整点奖币
            if (Utils.IsRegex(model.ChatCsmall, @"^[1-9]\d*-[1-9]\d*$"))
            {
                string[] Csmall = model.ChatCsmall.Split("-".ToCharArray());
                Bb = new Random().Next(Convert.ToInt32(Csmall[0]), Convert.ToInt32(Csmall[1]) + 1);
            }
        }
        string outText = string.Empty;
        string mename = new BCW.BLL.User().GetUsName(meid);

        //if (id == 6)
        //{
        //    string[] sNum = { "1", "2", "3" };
        //    Random rd = new Random();
        //    int icb = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
        //    if (icb == 1)
        //    {
        //        outText = "获得" + Bb + "" + ub.Get("SiteBz") + "";
        //        new BCW.BLL.User().UpdateiGold(meid, mename, Bb, "红包群抢" + ub.Get("SiteBz") + "");
        //    }
        //    else if (icb == 2)
        //    {
        //        int jf = rd.Next(1, 11);
        //        outText = "获得" + jf + "积分";
        //        new BCW.BLL.User().UpdateiScore(meid, Convert.ToInt64(jf));
        //    }
        //    else
        //    {
        //        outText = "获得" + (Bb * 2) + "" + ub.Get("SiteBz2") + "";
        //        new BCW.BLL.User().UpdateiMoney(meid, mename, Convert.ToInt64(Bb * 2), "红包群抢" + ub.Get("SiteBz2") + "");
        //    }
        //}
        //else
        //{
        if (Convert.ToInt64(Bb) > model.ChatCent)
        {
            Bb = 0;
            strText = "本红包群基金不足，抢不到" + ub.Get("SiteBz") + "啦";
        }
        else
        {
            outText = "获得" + Bb + "" + ub.Get("SiteBz") + "";
            new BCW.BLL.User().UpdateiGold(meid, mename, Bb, "红包群抢币");
            new BCW.BLL.Chat().UpdateChatCent(id, -Convert.ToInt64(Bb));
        }

        DataSet Cmoney = new BCW.HB.BLL.HbPost().GetChatList(id);
        string cmoneys = Cmoney.Tables[0].Rows[0][2].ToString();
        string[] cmone = cmoneys.Split("#".ToCharArray());
        string CM = string.Empty;
        builder.Append(cmoneys);
        for (int i = 1; i <= CTemp.Length; i++)
        {
            if (i == num)
                CM += "#" + Bb;
            else
                if (i <= cmone.Length)
            {
                CM += "#" + cmone[i - 1];
            }
        }
        CM = Utils.Mid(CM, 1, CM.Length);
        if (!("#" + snum + "#").Contains("#0#"))
        {
            //抢币标识
            string sCTemp = string.Empty;
            for (int i = 0; i < strCTemp.Length; i++)
            {
                sCTemp += "#0";
            }
            sCTemp = Utils.Mid(sCTemp, 1, sCTemp.Length);
            new BCW.HB.BLL.HbPost().UpdateCb(id, sCTemp, DateTime.Now.AddSeconds(model.ChatCon));
        }
        else
        {
            if (Bb != 0)
            {
                new BCW.HB.BLL.HbPost().UpdateChatCmoney(id, CM);
            }

        }
        //}
        //记录系统发言
        //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
        //{
        //    BCW.Model.ChatText addmodel = new BCW.Model.ChatText();
        //    addmodel.ChatId = id;
        //    addmodel.Content = "抢到了'" + ctext + "'字," + outText + "";
        //    addmodel.UsID = meid;
        //    addmodel.UsName = mename;
        //    addmodel.ToID = 0;
        //    addmodel.ToName = "";
        //    addmodel.IsKiss = 0;
        //    addmodel.AddTime = DateTime.Now;
        //    new BCW.BLL.ChatText().Add(addmodel);
        //}
        new BCW.HB.BLL.ChatMe().Update(id, meid);
        Utils.Success("抢币", "恭喜！抢到了“" + ctext + "”字，" + outText + "！" + strText + "", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn), "1");
    }

    /// <summary>
    /// 发言排行
    /// </summary>
    private void TopPage()
    {
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "30"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[0-9]\d*$", "10"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "发言排行";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=本红包群排行=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("排行:");

        if (showtype == 0)
            builder.Append("本周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=top&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">本周</a>|");

        if (showtype == 1)
            builder.Append("上周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=top&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">上周</a>|");

        if (showtype == 2)
            builder.Append("本月|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=top&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>|");

        if (showtype == 3)
            builder.Append("上月");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=top&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">上月</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "ChatId=" + id + " and ";

        //本周计算
        if (showtype == 0)
            strWhere += "AddTime>='" + DT.GetWeekStart() + "' AND AddTime< '" + DT.GetWeekOver() + "' AND UsID!=0 AND Type=0";

        //上周计算
        else if (showtype == 1)
            strWhere += "AddTime>='" + DateTime.Parse(DT.GetWeekStart()).AddDays(-7) + "' AND AddTime< '" + DT.GetWeekStart() + "' AND UsID!=0 AND Type=0";

        //本月计算
        else if (showtype == 2)
            strWhere += "Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "  AND UsID!=0 AND Type=0";
        //上月计算
        else if (showtype == 3)
        {
            DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
            int ForYear = ForDate.Year;
            int ForMonth = ForDate.Month;
            strWhere += "Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "  AND UsID!=0 AND Type=0";
        }

        string[] pageValUrl = { "act", "showtype", "id", "tm", "pn", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.ChatText> listChatText = new BCW.BLL.ChatText().GetChatTextsTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listChatText.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.ChatText n in listChatText)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>:" + n.ChatId + "条");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void StatInfoPage()
    {
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-3]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "30"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[0-9]\d*$", "10"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "本红包群统计";
        string Whe = "";
        Whe = "ChatId=" + id + " AND ";
        string M_Str_mindate;//用于存储最小日期
        string M_Str_maxdate;//用于存储最大日期
        //今日计算
        M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.AddDays(1).ToShortDateString() + " 0:00:00";
        int TodayCount = new BCW.BLL.ChatText().GetCount("" + Whe + " AddTime>='" + M_Str_mindate + "' AND AddTime< '" + M_Str_maxdate + "'  AND Type=0");
        //昨日计算
        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.ToShortDateString() + " 0:00:00";
        int YesterdayCount = new BCW.BLL.ChatText().GetCount("" + Whe + " AddTime>='" + M_Str_mindate + "' AND AddTime< '" + M_Str_maxdate + "'  AND Type=0");
        //本周计算
        int WeekCount = new BCW.BLL.ChatText().GetCount("" + Whe + " AddTime>='" + DT.GetWeekStart() + "' AND AddTime< '" + DT.GetWeekOver() + "'  AND Type=0");
        //上周计算
        int BfWeekCount = new BCW.BLL.ChatText().GetCount("" + Whe + " AddTime>='" + DateTime.Parse(DT.GetWeekStart()).AddDays(-7) + "' AND AddTime< '" + DT.GetWeekStart() + "'  AND Type=0");
        //本月计算
        int MonthCount = new BCW.BLL.ChatText().GetCount("" + Whe + " Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "  AND Type=0");
        //上月计算
        DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
        int ForYear = ForDate.Year;
        int ForMonth = ForDate.Month;
        int BfMonthCount = new BCW.BLL.ChatText().GetCount("" + Whe + " Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "  AND Type=0");
        builder.Append(Out.Tab("<div class=\"title\">本红包群统计</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今日记录：" + TodayCount + "");
        builder.Append("<br />昨日记录：" + YesterdayCount + "");
        builder.Append("<br />本周记录：" + WeekCount + "");
        builder.Append("<br />上周记录：" + BfWeekCount + "");
        builder.Append("<br />本月记录：" + MonthCount + "");
        builder.Append("<br />上月记录：" + BfMonthCount + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=top&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本红包群会员排行&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">&lt;&lt;返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 红包群奖励
    /// </summary>
    /// <param name="ptype"></param>
    /// <param name="meid"></param>
    public static void ChatOutCent(int id, int meid, string mename)
    {

        string xmlPath = "/Controls/chat.xml";
        DateTime dt = DateTime.Now;
        try
        {
            dt = Convert.ToDateTime(ub.GetSub("ChatCentTime", xmlPath));
        }
        catch
        {
            dt = DateTime.Parse("1990-1-1");
        }
        int dtMin = new Random().Next(20, 30);
        //在这个随机时间里的发言数量
        DateTime speakdt = DateTime.Now.AddMinutes(-dtMin);
        int speakCount = new BCW.BLL.ChatText().GetCount(speakdt);
        //这个时间的发言数量达20条才抽奖
        if (speakCount >= 10)
        {
            if (DateTime.Now > dt.AddMinutes(dtMin) || dt == DateTime.Parse("1990-1-1"))
            {
                string outText = "";
                Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
                int Types = 1;// rd.Next(1, 11);
                Random rd2 = new Random(unchecked((int)DateTime.Now.Ticks));
                if (Types == 1)
                {
                    long cent = rd2.Next(10, 1001);

                    new BCW.BLL.User().UpdateiGold(meid, mename, cent, "红包群抢" + ub.Get("SiteBz") + "");
                    outText = "恭喜" + mename + "在红包群发言抽奖获得" + cent + "" + ub.Get("SiteBz") + "！";
                }

                if (outText != "")
                {

                    BCW.Model.ChatText addmodel = new BCW.Model.ChatText();
                    addmodel.ChatId = id;
                    addmodel.Content = outText;
                    addmodel.UsID = 0;
                    addmodel.UsName = "系统";
                    addmodel.ToID = 0;
                    addmodel.ToName = "";
                    addmodel.IsKiss = 0;
                    addmodel.AddTime = DateTime.Now;
                    new BCW.BLL.ChatText().Add(addmodel);

                    //内线
                    new BCW.BLL.Guest().Add(meid, mename, outText.Replace(mename, "您" + DT.FormatDate(DateTime.Now, 13) + ""));


                    //写入配置
                    ub xml = new ub();
                    xml.ReloadSub(xmlPath); //加载配置
                    xml.dss["ChatCentTime"] = DateTime.Now;
                    xml.dss["ChatOutText"] = outText.Replace("抽奖", "").Replace("恭喜", DT.FormatDate(DateTime.Now, 13)).Replace("！", "");
                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    HttpContext.Current.Application.Remove(xmlPath);//清缓存
                }
            }
        }
    }

    private void DZPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]\d*$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "10"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "红包群发言-选择动作";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;uid=" + ToID + "&amp;hb=" + hbgn) + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=选择动作=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("招呼类|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=dz&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=0" + "&amp;hb=" + hbgn) + "\">招呼类</a>|");

        if (ptype == 1)
            builder.Append("恶搞类|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=dz&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=1" + "&amp;hb=" + hbgn) + "\">恶搞类</a>|");

        if (ptype == 2)
            builder.Append("示爱类");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=dz&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=2" + "&amp;hb=" + hbgn) + "\">示爱类</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));

        if (ptype == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=11" + "&amp;hb=" + hbgn) + "\">招呼</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=12" + "&amp;hb=" + hbgn) + "\">谨仰</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=13" + "&amp;hb=" + hbgn) + "\">问候</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=14" + "&amp;hb=" + hbgn) + "\">赞美</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=15" + "&amp;hb=" + hbgn) + "\">活泼</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=16" + "&amp;hb=" + hbgn) + "\">害羞</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=17" + "&amp;hb=" + hbgn) + "\">感慨</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=18" + "&amp;hb=" + hbgn) + "\">礼貌</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=19" + "&amp;hb=" + hbgn) + "\">快乐</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=20" + "&amp;hb=" + hbgn) + "\">低唱</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=21" + "&amp;hb=" + hbgn) + "\">思念</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=22" + "&amp;hb=" + hbgn) + "\">幸福</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=23" + "&amp;hb=" + hbgn) + "\">祈祷</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=24" + "&amp;hb=" + hbgn) + "\">诵念</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=25" + "&amp;hb=" + hbgn) + "\">安抚</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=26" + "&amp;hb=" + hbgn) + "\">闪电</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=27" + "&amp;hb=" + hbgn) + "\">爆发</a>");
        }
        else if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=28" + "&amp;hb=" + hbgn) + "\">拔牙</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=6" + "&amp;hb=" + hbgn) + "\">刀劈</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=29" + "&amp;hb=" + hbgn) + "\">喝酒</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=30" + "&amp;hb=" + hbgn) + "\">马桶</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=31" + "&amp;hb=" + hbgn) + "\">痴缠</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=32" + "&amp;hb=" + hbgn) + "\">沉思</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=33" + "&amp;hb=" + hbgn) + "\">臭美</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=34" + "&amp;hb=" + hbgn) + "\">自杀</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=35" + "&amp;hb=" + hbgn) + "\">喷嚏</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=36" + "&amp;hb=" + hbgn) + "\">吻别</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=37" + "&amp;hb=" + hbgn) + "\">肠子</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=38" + "&amp;hb=" + hbgn) + "\">教训</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=39" + "&amp;hb=" + hbgn) + "\">偷窥</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=40" + "&amp;hb=" + hbgn) + "\">调皮</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=41" + "&amp;hb=" + hbgn) + "\">性格</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=42" + "&amp;hb=" + hbgn) + "\">毒辣</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=43" + "&amp;hb=" + hbgn) + "\">风流</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=44" + "&amp;hb=" + hbgn) + "\">帅气</a>");

        }
        else if (ptype == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=45" + "&amp;hb=" + hbgn) + "\">放电</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=46" + "&amp;hb=" + hbgn) + "\">亲吻</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=47" + "&amp;hb=" + hbgn) + "\">错爱</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=48" + "&amp;hb=" + hbgn) + "\">求爱</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=49" + "&amp;hb=" + hbgn) + "\">亲嘴</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=50" + "&amp;hb=" + hbgn) + "\">送花</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=51" + "&amp;hb=" + hbgn) + "\">礼物</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=52" + "&amp;hb=" + hbgn) + "\">唱歌</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=53" + "&amp;hb=" + hbgn) + "\">温柔</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=54" + "&amp;hb=" + hbgn) + "\">嫦娥</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=55" + "&amp;hb=" + hbgn) + "\">求婚</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=56" + "&amp;hb=" + hbgn) + "\">深情</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=57" + "&amp;hb=" + hbgn) + "\">坦诚</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=58" + "&amp;hb=" + hbgn) + "\">倾慕</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=59" + "&amp;hb=" + hbgn) + "\">结婚</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=60" + "&amp;hb=" + hbgn) + "\">相思</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=61" + "&amp;hb=" + hbgn) + "\">召唤</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=save&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;aa=62" + "&amp;hb=" + hbgn) + "\">爱慕</a>");

        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;hb=" + hbgn) + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BQPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int VipLeven = BCW.User.Users.VipLeven(meid);
        if (VipLeven == 0)
        {
            Utils.Error("限VIP会员使用", "");

        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "10"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "红包群发言-选择VIP表情";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;uid=" + ToID + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=选择VIP表情=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));

        if (ptype == 0)
            builder.Append("普通 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=0" + "&amp;hb=" + hbgn) + "\">普通</a> ");


        if (ptype == 1)
            builder.Append("宠物 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=1" + "&amp;hb=" + hbgn) + "\">宠物</a> ");

        if (ptype == 2)
            builder.Append("动作 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=2" + "&amp;hb=" + hbgn) + "\">动作</a> ");

        if (ptype == 3)
            builder.Append("微笑 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=3" + "&amp;hb=" + hbgn) + "\">微笑</a> ");

        if (ptype == 4)
            builder.Append("花类<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=4" + "&amp;hb=" + hbgn) + "\">花类</a><br />");

        if (ptype == 5)
            builder.Append("可爱 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=5" + "&amp;hb=" + hbgn) + "\">可爱</a> ");

        if (ptype == 6)
            builder.Append("食品 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=6" + "&amp;hb=" + hbgn) + "\">食品</a> ");

        if (ptype == 7)
            builder.Append("手势 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=7" + "&amp;hb=" + hbgn) + "\">手势</a> ");

        if (ptype == 8)
            builder.Append("休闲 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=8" + "&amp;hb=" + hbgn) + "\">休闲</a> ");

        if (ptype == 9)
            builder.Append("饮料 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=9" + "&amp;hb=" + hbgn) + "\">饮料</a>");



        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int k = 1;
        int j = 10;
        if (ptype > 0)
        {
            k = 10 * ptype + 1;
            j = (j * (ptype + 1));
            if (j == 100)
                j = 99;
        }


        for (int i = k; i <= j; i++)
        {
            string str = i.ToString();
            if (i < 10)
                str = "0" + str;

            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;uid=" + ToID + "&amp;ff=v0" + str + "") + "\"><img src=\"/files/face/vip/v0" + str + ".gif\" alt=\"load\"/></a>");
        }
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "" + "&amp;hb=" + hbgn) + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BQ2Page(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "1"));

        int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "10"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "红包群发言-选择表情";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;uid=" + ToID + "" + "&amp;hb=" + hbgn) + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=选择表情=");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int kk = 0;
        int pageSize = 25;
        string[] pageValUrl = { "act", "ptype", "ToID", "id", "hb", "tm", "pn", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //总记录数
        if (ptype == 0)
        {
            kk = 1;
            recordCount = 24;
        }
        else
        {
            kk = 1001;
            recordCount = 13;
        }
        builder.Append(Out.Tab("<div>", ""));
        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;

        for (int i = 0; i < recordCount; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;uid=" + ToID + "&amp;ff=" + (kk + i) + "") + "\"><img src=\"/files/Face/" + (kk + i) + ".gif\" alt=\"load\"/></a>\r\n");

            }
            if (k == endIndex)
                break;
            k++;
        }
        builder.Append(Out.Tab("</div>", ""));

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq2&amp;id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换表情一</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=bq2&amp;id=" + id + "&amp;tm=" + tm + "&amp;hb=" + hbgn + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换表情二</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 发图片
    /// </summary>
    /// <param name="ptype"></param>
    /// <param name="meid"></param>
    private void PostPhoto()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int ptyp = int.Parse(Utils.GetRequest("ptyp", "all", 1, @"^[0-2]\d*$", "0"));
        int XcID = int.Parse(Utils.GetRequest("xcid", "all", 1, @"^[0-9]\d*$", "0"));
        int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "10"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "红包群发图片";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=选择图片=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptyp == 0)
            builder.Append("直接上传|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=posph" + "&amp;hb=" + hbgn + "&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=0") + "\">直接上传</a>|");

        if (ptyp == 1)
            builder.Append("从相册选择");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=posph" + "&amp;hb=" + hbgn + "&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=1") + "\">从相册选择</a>");
        int leibie = 1;
        int uid = meid;
        builder.Append(Out.Tab("</div>", "<br />"));
        if (ptyp == 0)
        {
            Master.Title = "发送图片";
            if (!Utils.Isie())
            {
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.Append("<a href=\"chatroom.aspx?act=posph&amp;id=" + id + "&amp;hb=1" + "&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
            }
            else
            {
                string maxfile = ub.GetSub("UpaMaxFileSize", xmlPath2);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("上传允许格式:.gif,.jpg,.jpeg,.png<br />上传后的图片将自动保存到您的红包群相册<br />");
                builder.Append("每个文件限" + maxfile + "K");
                builder.Append(Out.Tab("</div>", "<br />"));
                int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
                string sUpType = string.Empty;
                string sText = string.Empty;
                string sName = string.Empty;
                string sType = string.Empty;
                string sValu = string.Empty;
                string sEmpt = string.Empty;
                sText = "选择图片:/," + sUpType + "发言内容(30字内):/";
                sName = "file,stext";
                sType = "file" + ",text";
                sValu = "'";
                sEmpt = ",";
                strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
                strName = sName + Utils.Mid(strName, 1, strName.Length) + ",hb,leibie,act,id";
                strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
                strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + hbgn + "'" + leibie + "'upload'" + id + "";
                strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,";
                strIdea = "/";
                strOthe = "确定发送|reset,chatroom.aspx?act=suss,post,2,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            int pageIndex;
            int recordCount;
            int pageSize = 100000000;
            string strWhere = "";
            string[] pageValUrl = { "leibie", "ptyp", "xcid", "id", "hb", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "Leibie=" + leibie + " and UsID=" + uid + "";
            // 开始读取列表
            IList<BCW.Model.Upgroup> listUpgroup = new BCW.BLL.Upgroup().GetUpgroups(pageIndex, pageSize, strWhere, out recordCount);
            int k = 1;
            foreach (BCW.Model.Upgroup n in listUpgroup)
            {
                if (XcID == n.ID)
                {
                    builder.Append(n.Title + "(" + new BCW.BLL.Upfile().GetCount(uid, n.ID) + "张)");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=posph&amp;id=" + id + "&amp;" + "&amp;hb=" + hbgn + "tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=1" + "&amp;xcid=" + n.ID) + "\">" + n.Title + "(" + new BCW.BLL.Upfile().GetCount(uid, n.ID) + "张)</a>");

                }
                if (k % 4 == 0)
                {
                    builder.Append("<br />");
                }
                else
                {
                    builder.Append("|");
                }
                k++;
            }
            if (XcID == 0)
            {
                builder.Append("未归类相集(" + new BCW.BLL.Upfile().GetCount(uid, leibie, 0) + "张)");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=posph" + "&amp;hb=" + hbgn + "&amp;id=" + id + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=1" + "&amp;xcid=0" + "") + "\">未归类相集(" + new BCW.BLL.Upfile().GetCount(uid, leibie, 0) + ")</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        if (ptyp == 1)
        {
            //--------------------相册内容------------------------------------
            int pageIndex2;
            int recordCount2;
            int pageSize2 = 3;
            if (leibie > 1)
                pageSize2 = Convert.ToInt32(ub.Get("SiteListNo"));

            string strWhere2 = "";
            string[] pageValUrl2 = { "act", "ptyp", "xcid", "id", "backurl" };
            pageIndex2 = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex2 == 0)
                pageIndex2 = 1;
            //查询条件
            strWhere2 = "Types=" + leibie + " and UsID=" + uid + " and NodeId=" + XcID + "";
            // 开始读取列表
            IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex2, pageSize2, strWhere2, out recordCount2);
            builder.Append("<form id=\"form1\" method=\"post\" action=\"chatroom.aspx\">");
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
                    builder.AppendFormat("{0}.", (pageIndex2 - 1) * pageSize2 + k);
                    builder.Append("<a  target=\"_blank\" href=\"" + Utils.getUrl(n.Files) + "\">查看原图.</a>");
                    if (n.IsVerify != 1)
                        builder.Append("选择<input type=\"radio\" name=\"phid\" value=\"" + n.ID + "\" />");
                    k++;
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
                // 分页

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"suss2\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptyp\" Value=\"" + ptyp + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"id\" Value=\"" + id + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"hb\" Value=\"" + hbgn + "\"/>");
                builder.Append("发言内容(30字内):<br />");
                builder.Append("<input type=\"text\" name=\"stext\" Value=\"\"/><br />");
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确认发送\"/>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
                builder.Append(BasePage.MultiPage(pageIndex2, pageSize2, recordCount2, Utils.getPageUrl(), pageValUrl2, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
                builder.Append("</form>");
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void SubmitPage()
    {
        BCW.User.Users.IsFresh("发图", 5);
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string stext = Utils.GetRequest("stext", "all", 3, @"^[\s\S]{1,30}$", "发言长度超出限制");
        int photo = int.Parse(Utils.GetRequest("phid", "all", 2, @"^[0-9]\d*$", "图片ID选择错误"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        BCW.Model.Upfile mol = new BCW.BLL.Upfile().GetUpfile(photo, 1);
        BCW.Model.ChatText addmodel7 = new BCW.Model.ChatText();
        addmodel7.ChatId = id;
        addmodel7.Content = stext + "<br />[url=/showpic.aspx?pic=" + mol.Files + "&chatid=" + id + "&hb=0]<img  height=\"45px\" src=\"" + mol.Files + "\" alt=\"load\"/>[/url]";
        addmodel7.UsID = meid;
        addmodel7.UsName = new BCW.BLL.User().GetUsName(meid);
        addmodel7.ToID = 0;
        addmodel7.ToName = string.Empty;
        addmodel7.IsKiss = 0;
        addmodel7.AddTime = DateTime.Now;
        new BCW.BLL.ChatText().Add(addmodel7);
        new BCW.HB.BLL.ChatMe().Update(id, meid);
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn)), "1");
        }
        else
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn)), "0");
        }
    }
    private void UploadPage()
    {
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.User.Users.IsFresh("发图", 5);
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群ID错误"));
        string stext = Utils.GetRequest("stext", "all", 3, @"^[\s\S]{1,30}$", "发言长度超出限制");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        DataSet modelcount = new BCW.BLL.Upgroup().GetList("count(*)", "UsID=" + meid + " and Title='红包群相册'");
        int coo = Convert.ToInt32(modelcount.Tables[0].Rows[0][0]);
        if (coo == 0)
        {
            BCW.Model.Upgroup model = new BCW.Model.Upgroup();
            model.Leibie = leibie;
            model.Types = 0;
            model.PostType = 1;
            model.Title = "红包群相册";
            model.UsID = meid;
            model.IsReview = 0;
            model.Paixu = 0;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Upgroup().Add(model);
        }
        DataSet melbp = new BCW.BLL.Upgroup().GetList("*", "UsID=" + meid + " and Title='红包群相册'");
        NodeId = Convert.ToInt32(melbp.Tables[0].Rows[0][0]);

        //上传文件
        int kk = 0;
        int ios = 0;
        SaveFiles(meid, leibie, NodeId, out kk, out ios);
        BCW.Model.Upfile mol = new BCW.BLL.Upfile().GetUpfile(ios, 1);

        BCW.Model.ChatText addmodel7 = new BCW.Model.ChatText();
        addmodel7.ChatId = id;
        addmodel7.Content = stext + "<br />[url=/showpic.aspx?pic=" + mol.Files + "&chatid=" + id + "&hb=0]<img  height=\"45px\" src=\"" + mol.Files + "\" alt=\"load\"/>[/url]";
        addmodel7.UsID = meid;
        addmodel7.UsName = new BCW.BLL.User().GetUsName(meid);
        addmodel7.ToID = 0;
        addmodel7.ToName = string.Empty;
        addmodel7.IsKiss = 0;
        addmodel7.AddTime = DateTime.Now;
        new BCW.BLL.ChatText().Add(addmodel7);
        new BCW.HB.BLL.ChatMe().Update(id, meid);
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn)), "1");
        }
        else
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn)), "0");
        }
    }
    /// <summary>
    /// 上传文件
    /// </summary>
    private void SaveFiles(int meid, int leibie, int NodeId, out int kk, out int ios)
    {
        //允许上传数量
        int maxAddNum = 1;
        int AddNum = 0;
        if (maxAddNum > 0)
        {
            //计算今天上传数量
            AddNum = new BCW.BLL.Upfile().GetTodayCount(meid);
        }

        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        int j = 1;
        int k = 0;
        ios = 0;
        try
        {
            string GetFiles = string.Empty;
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
                int UpLength = 5000;

                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    //检查是否允许上传格式
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        Utils.Error("图片类型选择错误！", "");
                    }
                    //非法上传
                    if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                    {
                        Utils.Error("非法上传！", "");
                    }
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                    {
                        Utils.Error("图片过大！", "");
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = "/Files/bbs/" + meid + "/act/";
                    string prevPath = "/Files/bbs/" + meid + "/prev/";
                    int IsVerify = 0;

                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;

                        postedFile.SaveAs(SavePath);

                        #region 图片木马检测,包括TXT
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
                        #endregion

                        #region 缩略图生成
                        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
                        {
                            int ThumbType = 1;
                            int width = 56;
                            int height = 70;
                            try
                            {
                                bool pbool = false;
                                if (ThumbType == 1)
                                    pbool = true;
                                if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                {
                                    string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;
                                    if (fileExtension == ".gif")
                                    {
                                        if (ThumbType > 0)
                                            new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);
                                    }
                                    else
                                    {
                                        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                                        {
                                            if (ThumbType > 0)
                                                new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);

                                        }
                                    }

                                }
                            }
                            catch
                            {
                                Utils.Error("缩略图生成错误！", "");
                            }

                        }
                        #endregion

                        string Content = Utils.GetRequest("stext" + j + "", "post", 1, "", "");
                        if (!string.IsNullOrEmpty(Content))
                            Content = Utils.Left(Content, 30);
                        else
                            Content = "";

                        BCW.Model.Upfile model = new BCW.Model.Upfile();
                        model.Types = 1; // FileTool.GetExtType(fileExtension);
                        model.NodeId = NodeId;
                        model.UsID = meid;
                        model.ForumID = 0;
                        model.BID = 0;
                        model.ReID = 0;
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

                        ios = new BCW.BLL.Upfile().Add(model);
                        k++;
                    }
                    j++;
                }

            }

        }
        catch
        {

        }

        kk = k;
    }
    private static string ReplaceWap(string p_strUrl)
    {
        p_strUrl = p_strUrl.Replace("20a", "1a");

        return p_strUrl;
    }
    /// <summary>
    /// 发红包
    /// </summary>
    /// <param name="ptype"></param>
    /// <param name="meid"></param>
    public void PostHb(int id, int meid, int tm, int pn)
    {
        string strtest;
        int HbSpeed = Utils.ParseInt(ub.GetSub("HbSpeed", xmlPath3));
        if (new BCW.HB.BLL.HbPost().Exists(meid, 0))
        {
            List<BCW.HB.Model.HbPost> hbwillpost = new BCW.HB.BLL.HbPost().GetModelList("ChatID=" + id + " and State=0 and UserID=" + meid);
            for (int ips = 0; ips < hbwillpost.Count; ips++)
            {
                BCW.Model.ChatText addmodel7 = new BCW.Model.ChatText();
                addmodel7.ChatId = hbwillpost[ips].ChatID;
                switch (hbwillpost[ips].Style)
                {
                    case 10:
                        strtest = "[他们专属]";
                        break;
                    case 20:
                        strtest = "[拒绝他们]";
                        break;
                    case 1:
                        strtest = "[男女专属]";
                        break;
                    case 2:
                        strtest = "[口令]";
                        break;
                    default:
                        strtest = "";
                        break;
                }
                if (hbwillpost[ips].RadomNum.Trim() == "pt")
                {
                    addmodel7.Content = "发了一个[url=/bbs/chathb.aspx?act=gethb&amp;id=" + hbwillpost[ips].ID + "]普通红包[/url]" + strtest + ":" + hbwillpost[ips].Note;
                }
                else
                {
                    addmodel7.Content = "发了一个[url=/bbs/chathb.aspx?act=gethb&amp;id=" + hbwillpost[ips].ID + "]拼手气红包[/url]" + strtest + ":" + hbwillpost[ips].Note;
                }
                addmodel7.UsID = meid;
                addmodel7.UsName = new BCW.BLL.User().GetUsName(meid);
                addmodel7.ToID = 0;
                addmodel7.ToName = string.Empty;
                addmodel7.IsKiss = 0;
                addmodel7.AddTime = DateTime.Now;
                new BCW.BLL.ChatText().Add(addmodel7);
                new BCW.HB.BLL.ChatMe().Update(id, meid);
                new BCW.HB.BLL.HbPost().UpdateState(hbwillpost[ips].ID, 1);
            }
        }

        System.Text.StringBuilder builder2 = new System.Text.StringBuilder("");
        List<BCW.HB.Model.HbPost> hbpost = new BCW.HB.BLL.HbPost().GetModelList("ChatID=" + id + " and State=1");
        for (int iso = 0; iso < hbpost.Count; iso++)
        {
            switch (hbpost[iso].Style)
            {
                case 10:
                    strtest = "[他们专属]";
                    break;
                case 20:
                    strtest = "[拒绝他们]";
                    break;
                case 1:
                    strtest = "[男女专属]";
                    break;
                case 2:
                    strtest = "[口令]";
                    break;
                default:
                    strtest = "";
                    break;
            }
            string postname = new BCW.BLL.User().GetUsName(hbpost[iso].UserID);
            MatchCollection leyrm = Regex.Matches(hbpost[iso].GetIDList, "#");
            int elem = leyrm.Count;
            if (elem < hbpost[iso].num)
            {
                builder2.Append("<br/><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hbpost[iso].UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + postname + " </a>(" + hbpost[iso].UserID + ")<a href=\"" + Utils.getUrl("chathb.aspx?act=gethb&amp;id=" + hbpost[iso].ID) + "\">发了一个<img src=\"game/img/hb.gif\" height=\"21px\" width=\"14px\"  style=\"margin:auto\" alt=\"load\" />" + strtest + ":" + hbpost[iso].Note + "</a>");
            }
        }
        string lblblb = new BCW.JS.somejs().topfloat(builder2.ToString(), "marquee", "30", HbSpeed.ToString());
        if (builder2.ToString().Trim() == "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;puthb=1&amp;tm=" + tm + "&amp;hb=1&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">没有红包可以领取！快去发一个吧！</a> ");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            builder.Append(lblblb);
        }
    }
    public void putshb(int id, int meid, int tm, int pn)
    {
        strText = ",,,,,";
        strName = "id,tm,pn,act,backurl";
        strType = "hidden,hidden,hidden,hidden,hidden";
        strValu = id + "'" + tm + "'" + pn + "'save'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "我要发红包|红包玩花样,chathb.aspx,post,3,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }
    //保存到通讯录
    public bool saveMe(int id, int uid)
    {
        bool exo = new BCW.HB.BLL.ChatMe().Exists(id, uid);
        if (exo)
        {
            return false;
        }
        else
        {
            BCW.HB.Model.ChatMe chatme = new BCW.HB.Model.ChatMe();
            chatme.ChatID = id;
            chatme.UserID = uid;
            chatme.jointime = DateTime.Now;
            chatme.score = 0;
            chatme.State = 0;
            new BCW.HB.BLL.ChatMe().Add(chatme);
            return true;
        }
    }
    //与某人的对话记录
    private void ChatPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[1-9]\d*$", "0"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "0"));
        int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[1-9]\d*$", "会员ID错误"));
        string UsName = new BCW.BLL.User().GetUsName(ToID);
        if (UsName == "")
        {
            Utils.Error("不存在会员ID", "");
        }
        ///////////////////
        int uid = meid;
        string EnPurl = Utils.getPage(0);
        ///////////////////
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        Master.Title = "对话记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我与" + UsName + "对话记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "tm", "pn", "hbgn", "ToID", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "ChatId=" + id + " and (UsID= " + meid + " and ToID = " + ToID + ") OR (UsID = " + ToID + " and ToID = " + meid + ")";

        // 开始读取列表
        IList<BCW.Model.ChatText> listSpeak = new BCW.BLL.ChatText().GetChatTexts(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpeak.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.ChatText n in listSpeak)
            {

                string Notes = Regex.Replace(n.Content, @"(\[F\])(.*?)(\[\/F\])", @"<img src=""/Files/Face/$2.gif"" alt=""load""/>", RegexOptions.IgnoreCase);
                Notes = Notes.Replace("&", "&amp;").Replace("&amp;amp;", "&amp;");

                n.UsName = BCW.User.Users.SetUser(n.UsID, 4);
                n.ToName = BCW.User.Users.SetUser(n.ToID, 4);

                string GText = "";
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                }


                if (n.IsKiss == 0)
                {

                    if (n.ToID == 0)
                    {
                        if (n.UsID == uid)
                            builder.AppendFormat("{0}<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        else
                        {
                            builder.AppendFormat("{0}{2}</a>说:{3}[{4}]", GText, n.UsID, n.UsName, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        }
                    }
                    else
                    {
                        if (n.UsID == uid)
                            builder.AppendFormat("{0}<b>我</b>对<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        else if (n.ToID == uid)
                            builder.AppendFormat("{0}<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>对<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        else
                            builder.AppendFormat("{0}<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>对<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                    }

                }
                else if (n.IsKiss == 2)
                {
                    string MName = string.Empty;
                    string TName = string.Empty;
                    if (n.UsID == uid)
                        MName = "<b>我</b>";
                    else
                        MName = "<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>";

                    if (n.ToID == uid)
                        TName = "<b>我</b>";
                    else
                        TName = "<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a>";


                    builder.AppendFormat("{0}{1}[{2}]", GText, Notes.Trim().Replace("$N", MName).Replace("$n", TName), DT.FormatDate(n.AddTime, 6));
                }
                else
                {

                    if (n.UsID == uid)
                        builder.AppendFormat("{0}*<b>我</b>对<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                    else if (n.ToID == uid)
                        builder.AppendFormat("{0}*<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>对<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                    else
                        builder.AppendFormat("{0}*<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>对<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));

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

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?act=add&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;uid=" + ToID + "") + "\">继续对TA发言&gt;&gt;</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;uid=" + ToID + "") + "\">&lt;&lt;返回红包群</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}