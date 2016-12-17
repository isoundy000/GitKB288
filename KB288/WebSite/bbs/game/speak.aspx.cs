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
using BCW.User;
using BCW.Files;

/// <summary>
/// 闲聊奖励 黄国军 20160813
/// 全屏聊天分类 设置 黄国军20160330
/// 蒙宗将  20160822 撤掉点值抽奖
///</summary>


public partial class bbs_game_speak : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/spkadmin.xml";
    protected string xmlPath3 = "/Controls/chathb.xml";

    //2016/5/4DSY

    #region 数组,按排序输出
    protected string[] sName = {
        "闲聊",//0
        "多人剪刀",//1
        "Ktv789",//2
        "猜猜乐",//3
        "欢乐竞拍",//4
        "竞猜",//5
        "幸运28",//6
        "虚拟投注",//7
        "疯狂彩球",//8
        "挖宝",//9
        "跑马",//10
        "上证",//11
        "",//12
        "大小庄",//13
        "吹牛",//14
        "",//15
        "猜拳",//16
        "苹果机",//17
        "掷骰",//18
        "拾物",//19
        "水果",//20
        "直播",//21
        "时时彩",//22
        "竞猜",//23
        "好彩一彩",//24
        "百花谷",//25
        "捕鱼达人",//26
        "快3挖宝",//27
        "快乐扑克3",//28
        "闯荡全城",//29
        "开心农场",//30
        "德州扑克",//31
        "PK10",//32
        "快乐十分",//33
        "胜负彩",//34
        "进球彩",//35
        "半全场",//36
        "点值抽奖",//37
        "活跃抽奖",//38
        "酷友云购",//39
        "问答",//40
        "百家欢乐"//41
    };
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "游戏闲聊";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        if (sName.Length < ptype)
        {
            Utils.Error("不存在的类型", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (ac == Utils.ToSChinese("动作"))
            act = "action";

        switch (act)
        {
            case "adminset":
                AdminSetPage(ptype);
                break;
            case "online":
                OnLinePage(ptype);
                break;
            case "admin":
                AdminPage(ptype);
                break;
            case "addblack":
                AddBlackPage(ptype);
                break;
            case "log":
                LogPage(ptype);
                break;
            case "blist":
                BlistPage(ptype);
                break;
            case "delblack":
                DelBlackPage(ptype);
                break;
            case "spkadmin":
                SpkAdminPage(ptype);
                break;
            case "spkadminsub":
                SpkAdminSubPage(ptype);
                break;
            case "del":
                DelPage(ptype);
                break;
            case "spkadd":
                SpkAddPage(ptype);
                break;
            case "action":
                ActionPage(ptype);
                break;
            case "fresh":
                FreshPage(ptype);
                break;
            case "statinfo":
                StatInfoPage(ptype);
                break;
            case "top":
                StatTopPage(ptype);
                break;
            case "top2":
                StatTop2Page(ptype);
                break;
            case "chat":
                ChatPage(ptype);
                break;
            case "posph":
                PostPhoto(ptype);
                break;
            case "suss":
                UploadPage(ptype);
                break;
            case "suss2":
                SubmitPage(ptype);
                break;
            default:
                ReloadPage(ptype);
                break;
        }
    }

    private void ReloadPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        //闲聊在线人数
        //int SpeakId = new BCW.BLL.User().GetEndSpeakID(meid);
        if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
        {
            new BCW.BLL.User().UpdateEndSpeakID(meid, ptype);
        }
        ///////////////////
        int uid = meid;
        int Types = ptype;
        string EnPurl = Utils.getPage(0);
        ///////////////////
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        int tm = BCW.User.Users.GetForumSet(ForumSet, 30);
        int pn = BCW.User.Users.GetForumSet(ForumSet, 31);
        int puthb = int.Parse(Utils.GetRequest("puthb", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = pn;
        if (pageSize < 6)
            pageSize = 6;

        string strWhere = "";
        string[] pageValUrl = { "ptype", "showtype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (tm > 0)
        {
            Master.Refresh = tm;
            Master.Gourl = Utils.getUrl("speak.aspx?id=" + id + "&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "");
        }
        #region 红包展示2016/4/9
        // if (hbgn == 1)
        // {
        PostHb(id, meid, tm, pn);
        // }

        #endregion
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (id == 0)
        {
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            {
                if (showtype == 0)
                    builder.Append("游戏闲聊|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏闲聊</a>|");

            }
            else
            {
                if (showtype == 0)
                    builder.Append("全部|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">全部</a>|");

                if (showtype == 1)
                    builder.Append("" + sName[ptype] + "|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + sName[ptype] + "</a>|");
            }

            if (showtype == 2)
                builder.Append("对聊");
            else
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">对聊</a>");


            builder.Append("|<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a>");

        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上一级</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (id == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (puthb == 1)
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;puthb=0&amp;tm=" + tm + "&amp;hb=1&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">闲聊</a> ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;puthb=1&amp;tm=" + tm + "&amp;hb=1&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">红包</a> ");
            //if (hbgn != 1)
            //{
            //    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + 1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">显示</a> ");
            //}
            //else
            //{
            //    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">隐藏</a> ");
            //}
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/chathb.aspx?act=hblb" + "&amp;speakid=" + ptype + "&amp;hb=" + hbgn) + "\">列表</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/chathb.aspx?act=myhb&amp;hb=" + hbgn + "&amp;speakid=" + ptype + "&amp;chatid=" + id) + "\">记录</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=posph&amp;id=" + id + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;ptype=" + ptype + "&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">图片</a> ");
            builder.Append(Out.Tab("</div>", "<br />"));


            string SpeakContent = Utils.GetRequest("SpeakContent", "post", 1, "", "");
            string strText = ",,,,,";
            string strName = "SpeakContent,Face,hb,ptype,showtype,backurl";
            string strType = "stext,select,hidden,hidden,hidden,hidden";
            string strValu = "'0'" + ptype + "'" + hbgn + "'" + showtype + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,false,false,false,false";
            string strIdea = "/";
            string strOthe = "[发言]|动作,speak.aspx,post,3,red|other";
            if (puthb == 1)
            {

                putshb(id, meid, tm, pn);
            }
            else
            {
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));
            if (Face > 0 & Face < 27)
                SpeakContent = "[F]" + Face + "[/F]" + SpeakContent;

            string mename = new BCW.BLL.User().GetUsName(meid);

            int aa = int.Parse(Utils.GetRequest("aa", "all", 1, @"^[1-9]\d*$", "0"));//sgl 把原aa改为接收face

            if (aa > 0 && aa < 55)
            {
                SpeakContent = "" + BCW.User.ChatAction.GetAction(aa)[1];
            }

            //写入闲聊记录
            if (SpeakContent != "")
            {
                //是否刷屏
                if (aa > 0 && aa < 55)
                {
                    //1分钟内限发动作一次
                    string appName = "LIGHT_SPEAK_ZD";
                    string CacheKey = appName + "_" + Utils.Mid(Utils.getstrU(), 0, Utils.getstrU().Length - 4);
                    object getObjCacheTime = DataCache.GetCache(CacheKey);
                    if (getObjCacheTime != null)
                    {
                        Utils.Error("1分钟内限发动作1次", Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""));
                    }
                    object ObjCacheTime = DateTime.Now;
                    DataCache.SetCache(CacheKey, ObjCacheTime, DateTime.Now.AddSeconds(60), TimeSpan.Zero);

                }
                else
                {
                    string appName = "LIGHT_SPEAK";
                    int Expir = Convert.ToInt32(ub.GetSub("BbsSpeakExpir", "/Controls/bbs.xml"));
                    BCW.User.Users.IsFresh(appName, Expir);

                }
                if (meid == 0)
                    Utils.Login();

                BCW.User.Users.ShowVerifyRole("h", meid);//非验证会员提示
                new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Speak, meid);//会员本身权限

                if (new BCW.BLL.Spkblack().Exists(meid, ptype))
                {
                    Utils.Error("你已在本游戏黑名单里", "");
                }
                //5分钟内相同发言则跳过
                bool isEquals = false;
                BCW.Model.Speak m = new BCW.BLL.Speak().GetNotesAddTime(meid);
                if (m != null)
                {
                    if (DateTime.Now <= m.AddTime.AddMinutes(5))
                    {
                        if (m.Notes.Equals(Utils.Left(SpeakContent, 120)))
                        {
                            isEquals = true;
                        }
                    }
                }
                if (!isEquals)
                {
                    BCW.Model.Speak addmodel = new BCW.Model.Speak();
                    addmodel.Types = ptype;
                    addmodel.NodeId = 0;
                    addmodel.UsId = meid;
                    addmodel.UsName = mename;
                    addmodel.ToId = 0;
                    addmodel.ToName = "";
                    addmodel.Notes = Utils.Left(SpeakContent, 520);
                    addmodel.AddTime = DateTime.Now;
                    if (aa > 0 && aa < 55)
                    {
                        addmodel.IsKiss = 2;
                    }
                    else
                    {
                        addmodel.IsKiss = 0;
                    }
                    string z1 = "";//20161020sgl
                    int speakid = new BCW.BLL.Speak().Add(addmodel);
                    if (aa > 0 && aa < 55)
                    {
                        new BCW.HB.BLL.ChatMe().UpdateSpeakType(speakid);
                    }
                    else
                    {
                        //闲聊奖励派币
                        if (Face > 0 & Face < 27)//单发表情
                        {
                            z1 = "";
                        }
                        else
                        {
                            int countss = new BCW.BLL.Speak().GetCount("UsId=" + meid + " AND Notes NOT LIKE '%[F]%' and Types=0 and datediff(day,AddTime,getdate())=0");//sgl20161020 AND Notes NOT LIKE '%[F]%'
                            if (addmodel.Notes.Trim() != "")
                            {

                                if (countss <= Convert.ToInt32(ub.GetSub("SpkLimit", xmlPath)))
                                {
                                    z1 = "闲聊发言奖币" + Convert.ToInt32(ub.GetSub("SpkOne", xmlPath)) + ",";//20161020sgl
                                    new BCW.BLL.User().UpdateiGold(meid, mename, Convert.ToInt32(ub.GetSub("SpkOne", xmlPath)), "闲聊发言奖币！");
                                }
                            }
                        }
                    }

                    //闲聊奖励
                    // BCW.User.Users.SpeakOutCent(ptype, meid, mename);
                    Utils.Success("发言", "发言成功，" + z1 + "正在返回..", Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

                }
            }
            builder.Append(Out.Tab("", "<br />"));
        }

        //查询条件
        if (id > 0)
        {
            strWhere += "Types=" + ptype + " and NodeId=" + id + "";
        }
        else
        {
            if (showtype == 1)
                strWhere += "Types=" + ptype + " and NodeId=0";
            else if (showtype == 2)
            {
                if (meid == 0)
                    Utils.Login();
                strWhere += "NodeId=0 and (UsId=" + meid + " OR ToId=" + meid + ")";
            }
        }
        bool bl2 = false;
        if (meid > 0)
        {
            string Role = new BCW.BLL.Role().GetRolece(meid);//闲聊总管
            if (Role.Contains("N"))
                bl2 = true;
        }
        // 开始读取列表
        IList<BCW.Model.Speak> listSpeak = new BCW.BLL.Speak().GetSpeaks(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpeak.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Speak n in listSpeak)
            {

                string Notes = Regex.Replace(n.Notes, @"(\[F\])(.*?)(\[\/F\])", @"<img src=""/Files/Face/$2.gif"" alt=""load""/>", RegexOptions.IgnoreCase);
                Notes = Notes.Replace("&", "&amp;").Replace("&amp;amp;", "&amp;");
                Notes = Out.TitleUBB(Notes);
                n.UsName = BCW.User.Users.SetUser(n.UsId, 4);
                n.ToName = BCW.User.Users.SetUser(n.ToId, 4);
                string GText = "";
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                {
                    GText = "[" + AppCase.CaseAction(n.Types) + "]";
                }


                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                }

                if (n.IsTop == 1)
                {
                    builder.Append("<b>[顶]</b>");
                }
                if (id > 0)
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>{2}(3)", n.UsId, n.UsName, Notes, DT.FormatDate(n.AddTime, 13));
                }
                else
                {
                    bool bl = new BCW.User.Role().IsSpkRole(n.Types, meid);

                    if (n.IsKiss == 0)
                    {

                        if (n.UsId == 0)
                            builder.AppendFormat("{0}{1}[系统/{2}]", GText, Notes, DT.FormatDate(n.AddTime, 13));
                        else
                        {
                            if (n.ToId == 0)
                            {
                                if (n.UsId == uid)
                                    builder.AppendFormat("{0}<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                                else
                                {
                                    builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid={1}&amp;v=1&amp;backurl=" + EnPurl + "") + "\">{2}</a>说:{3}[{4}]", GText, n.UsId, n.UsName, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                                }
                            }
                            else
                            {
                                if (n.UsId == uid)
                                    builder.AppendFormat("{0}<b>我</b>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                                else if (n.ToId == uid)
                                    builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                                else
                                    builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                            }
                        }
                    }
                    else if (n.IsKiss == 2)
                    {
                        string MName = string.Empty;
                        string TName = string.Empty;
                        if (n.UsId == uid)
                            MName = "<b>我</b>";
                        else
                            MName = "<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>";

                        if (n.ToId == uid)
                            TName = "<b>我</b>";
                        else
                            TName = "<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>";


                        builder.AppendFormat("{0}{1}[{2}]", GText, Notes.Trim().Replace("$N", MName).Replace("$n", TName), DT.FormatDate(n.AddTime, 13));
                    }
                    else
                    {
                        if (n.UsId == uid || n.ToId == uid || bl2 == true)
                        {
                            if (n.UsId == uid)
                                builder.AppendFormat("{0}*<b>我</b>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                            else if (n.ToId == uid)
                                builder.AppendFormat("{0}*<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));
                            else
                                builder.AppendFormat("{0}*<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 13));

                        }
                        else
                        {
                            builder.AppendFormat("{0}*秘密闲聊[{1}]", GText, DT.FormatDate(n.AddTime, 13));
                        }
                    }

                    if (bl == true && n.UsId > 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=del&amp;&amp;id=" + n.ID + "&amp;uid=" + n.UsId + "&amp;ptype=" + n.Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;v=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[管]</a>");
                    }
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            if (id == 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=online&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">当前闲聊在线(" + new BCW.BLL.User().GetSpeakNum() + ")</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">聊务</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=log&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">日志</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">对聊</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=fresh&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设置</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=statinfo&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">统计</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a>");
                builder.Append(Out.Tab("</div>", ""));
                if (ptype == 0)
                {
                    builder.Append(Out.Tab("", "<br />"));
                    string purl = "[url=/bbs/game/speak.aspx?ptype=0]游戏闲聊[/url]";
                    string strName = "purl,act,backurl";
                    string strValu = "" + purl + "'recommend'" + Utils.PostPage(1) + "";
                    string strOthe = "分享给好友,/bbs/guest.aspx,post,1,other";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">&lt;&lt;返回游戏</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">&lt;&lt;返回" + sName[ptype] + "</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void OnLinePage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "闲聊在线";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("闲聊在线聊友");
        builder.Append(Out.Tab("</div>", "<br />"));

        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += "EndSpeakID>=0 and EndSpeakTime>='" + DateTime.Now.AddMinutes(-5) + "'";

        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                if (n.State == 1)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".隐身会员");
                else
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;uid=" + n.ID + "&amp;v=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + BCW.User.Users.SetUser(n.ID) + "(" + n.ID + ")</a>");

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
        builder.Append("<a href=\"" + Utils.getPage("/bbs/game/default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">闲聊</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ChatPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[1-9]\d*$", "会员ID错误"));
        string UsName = new BCW.BLL.User().GetUsName(hid);
        if (UsName == "")
        {
            Utils.Error("不存在会员ID", "");
        }
        ///////////////////
        int uid = meid;
        int Types = ptype;
        string EnPurl = Utils.getPage(0);
        ///////////////////
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        Master.Title = "对话记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我与<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>对话记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "NodeId=0 and (UsId = " + meid + " and ToId = " + hid + ") OR (UsId = " + hid + " and ToId = " + meid + ")";

        // 开始读取列表
        IList<BCW.Model.Speak> listSpeak = new BCW.BLL.Speak().GetSpeaks(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpeak.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Speak n in listSpeak)
            {

                string Notes = Regex.Replace(n.Notes, @"(\[F\])(.*?)(\[\/F\])", @"<img src=""/Files/Face/$2.gif"" alt=""load""/>", RegexOptions.IgnoreCase);
                Notes = Notes.Replace("&", "&amp;").Replace("&amp;amp;", "&amp;");

                n.UsName = BCW.User.Users.SetUser(n.UsId, 4);
                n.ToName = BCW.User.Users.SetUser(n.ToId, 4);

                string GText = "";
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                {
                    GText = AppCase.CaseAction(n.Types);
                }

                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                }

                if (n.IsTop == 1)
                {
                    builder.Append("<b>[顶]</b>");
                }
                if (n.IsKiss == 0)
                {

                    if (n.ToId == 0)
                    {
                        if (n.UsId == uid)
                            builder.AppendFormat("{0}<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        else
                        {
                            builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;uid={1}&amp;v=1&amp;backurl=" + EnPurl + "") + "\">{2}</a>说:{3}[{4}]", GText, n.UsId, n.UsName, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        }
                    }
                    else
                    {
                        if (n.UsId == uid)
                            builder.AppendFormat("{0}<b>我</b>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        else if (n.ToId == uid)
                            builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                        else
                            builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                    }

                }
                else if (n.IsKiss == 2)
                {
                    string MName = string.Empty;
                    string TName = string.Empty;
                    if (n.UsId == uid)
                        MName = "<b>我</b>";
                    else
                        MName = "<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>";

                    if (n.ToId == uid)
                        TName = "<b>我</b>";
                    else
                        TName = "<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>";


                    builder.AppendFormat("{0}{1}[{2}]", GText, Notes.Trim().Replace("$N", MName).Replace("$n", TName), DT.FormatDate(n.AddTime, 6));
                }
                else
                {

                    if (n.UsId == uid)
                        builder.AppendFormat("{0}*<b>我</b>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                    else if (n.ToId == uid)
                        builder.AppendFormat("{0}*<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<b>我</b>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));
                    else
                        builder.AppendFormat("{0}*<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.UsName + "</a>对<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + Types + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;uid=" + n.ToId + "&amp;v=1&amp;backurl=" + EnPurl + "") + "\">" + n.ToName + "</a>说:{1}[{2}]", GText, Out.SysUBB(Notes), DT.FormatDate(n.AddTime, 6));

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
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=spkadd&amp;uid=" + hid + "&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续对TA发言&gt;&gt;</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminPage(int ptype)
    {
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = "闲聊聊务";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
        {
            builder.Append("闲聊聊务");
        }
        else
        {
            builder.Append("" + BCW.User.AppCase.CaseAction(ptype) + "&gt;聊务");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=log&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">聊务公开</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=blist&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">黑名单房</a><br />");
        if (new BCW.User.Role().IsSpkRole(ptype, meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=addblack&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">加黑会员</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=spkadminsub&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">闲聊管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=spkadmin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">闲聊总管</a>");
        builder.Append(Out.Tab("</div>", ""));
        if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
        {
            string Role = new BCW.BLL.Role().GetRolece(meid);//总权限
            if (Role.Contains("N"))
            {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=adminset&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设置闲聊管理</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn) + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminSetPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        string Role = new BCW.BLL.Role().GetRolece(meid);//总权限
        if (!Role.Contains("N"))
        {
            Utils.Error("非闲聊总管不能设置闲聊管理员", "");
        }
        Master.Title = "闲聊管理员设置";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/spkadmin.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string SpkAdmin = Utils.GetRequest("SpkAdmin", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个管理员ID请用#分隔，可以留空");

            xml.dss["SpkAdmin0"] = SpkAdmin;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("闲聊管理员设置", "设置闲聊管理员成功，正在返回..", Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "闲聊管理员设置"));

            string strText = "管理员ID:/,,,,";
            string strName = "SpkAdmin,ptype,act,hb,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "" + xml.dss["SpkAdmin0"] + "'" + ptype + "'adminset'" + hbgn + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,speak.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:多个管理员ID请用#分隔");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void LogPage(int ptype)
    {
        Master.Title = "聊务公开";
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + BCW.User.AppCase.CaseAction(ptype) + "&gt;聊务公开");
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
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            strWhere = "ForumID=" + ptype + " and Types=12";
        }
        else
        {
            strWhere = "Types=12";
        }

        // 开始读取列表
        IList<BCW.Model.Forumlog> listForumlog = new BCW.BLL.Forumlog().GetForumlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forumlog n in listForumlog)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.Content) + "(" + DT.FormatDate(n.AddTime, 1) + ")");
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
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BlistPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "黑名单管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + BCW.User.AppCase.CaseAction(ptype) + "&gt;黑名单");
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
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            strWhere += "Types=" + ptype + " and ";
        }

        strWhere += "ExitTime>='" + DateTime.Now + "'";

        // 开始读取列表
        IList<BCW.Model.Spkblack> listSpkblack = new BCW.BLL.Spkblack().GetSpkblacks(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpkblack.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Spkblack n in listSpkblack)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                builder.Append("禁言:" + n.BlackDay + "天/理由:" + n.BlackWhy + "[" + DT.FormatDate(n.AddTime, 5) + "]<br />");
                builder.Append("操作ID:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.AdminUsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.AdminUsID + "</a>");
                if (new BCW.User.Role().IsSpkRole(ptype, meid))
                {
                    builder.Append(".<a href=\"" + Utils.getUrl("speak.aspx?act=delblack&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[解黑]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddBlackPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        if (!new BCW.User.Role().IsSpkRole(ptype, meid))
        {
            Utils.Error("你的权限不足", "");
        }

        string mename = new BCW.BLL.User().GetUsName(meid);
        Master.Title = "加黑会员";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int v = int.Parse(Utils.GetRequest("v", "all", 1, @"^[0-1]$", "0"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string UsName = new BCW.BLL.User().GetUsName(uid);
            if (UsName == "")
            {
                Utils.Error("不存在的会员", "");
            }
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                if (new BCW.BLL.Spkblack().Exists(uid, ptype))
                {
                    Utils.Error("此ID已在本游戏黑名单里，如要再次加黑，请先解除再进行加黑", "");
                }
            }
            else
            {
                if (new BCW.BLL.Spkblack().Exists(uid, 0))
                {
                    Utils.Error("此ID已在游戏黑名单里，如要再次加黑，请先解除再进行加黑", "");
                }
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,50}$", "理由限50字内，可留空");
            int rDay = int.Parse(Utils.GetRequest("rDay", "post", 2, @"^1|2|3|5|10|15|30$", "期限选择错误"));
            BCW.Model.Spkblack model = new BCW.Model.Spkblack();
            model.UsID = uid;
            model.UsName = UsName;
            model.Types = ptype;
            model.BlackWhy = Why;
            model.BlackDay = rDay;
            model.AdminUsID = meid;
            model.ExitTime = DateTime.Now.AddDays(rDay);
            model.AddTime = DateTime.Now;
            new BCW.BLL.Spkblack().Add(model);
            //记录日志
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]禁言" + rDay + "天";
            string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将你在游戏闲聊禁言" + rDay + "天";

            if (!string.IsNullOrEmpty(Why))
            {
                strLog += ",理由:" + Why + "";
                strLog2 += ",理由:" + Why + "";
            }

            new BCW.BLL.Forumlog().Add(12, ptype, strLog);
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog2);
            if (v == 1)
                Utils.Success("添加黑名单", "添加黑名单" + UsName + "(" + uid + ")成功，正在返回..", Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            else
                Utils.Success("添加黑名单", "添加黑名单" + UsName + "(" + uid + ")成功，正在返回..", Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + ""), "1");

        }
        else
        {
            if (uid != 0)
            {
                string UsName = new BCW.BLL.User().GetUsName(uid);
                if (UsName == "")
                {
                    Utils.Error("不存在的会员", "");
                }
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("加黑会员:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "理由:,期限:,,,,,,,";
                string strName = "Why,rDay,ptype,showtype,v,uid,act,info,backurl";
                string strType = "text,select,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "'1'" + ptype + "'" + showtype + "'" + v + "'" + uid + "'addblack'ok'" + Utils.getPage(0) + "";
                string strEmpt = "true,1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定加黑,speak.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("加黑会员");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "会员ID:,理由:,期限:,,,,,,,";
                string strName = "uid,Why,rDay,ptype,showtype,v,act,info,backurl";
                string strType = "snum,text,select,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "''1'" + ptype + "'" + showtype + "'" + v + "'addblack'ok'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定加黑,speak.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            if (v == 1)
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            else
                builder.Append("<a href=\"" + Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "") + "\">再看看吧..</a>");

            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void DelBlackPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        if (!new BCW.User.Role().IsSpkRole(ptype, meid))
        {
            Utils.Error("你的权限不足", "");
        }
        string mename = new BCW.BLL.User().GetUsName(meid);
        Master.Title = "解除黑会员";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
        {
            Utils.Error("不存在的会员", "");
        }
        if (info == "ok")
        {
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                if (!new BCW.BLL.Spkblack().Exists(uid, ptype))
                {
                    Utils.Error("不存在的黑名单记录", "");
                }
            }
            else
            {
                if (!new BCW.BLL.Spkblack().Exists(uid, 0))
                {
                    Utils.Error("不存在的黑名单记录", "");
                }
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,50}$", "理由限50字内，可留空");
            new BCW.BLL.Spkblack().Delete(uid, ptype);
            //记录日志
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]解除黑名单";
            string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将你在游戏闲聊中解除黑名单";

            if (!string.IsNullOrEmpty(Why))
            {
                strLog += ",理由:" + Why + "";
                strLog2 += ",理由:" + Why + "";
            }

            new BCW.BLL.Forumlog().Add(12, ptype, strLog);
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog2);
            Utils.Success("解除黑名单", "解除黑名单" + UsName + "(" + uid + ")成功，正在返回..", Utils.getUrl("speak.aspx?act=blist&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("解除会员:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "理由:,,,,,";
            string strName = "Why,ptype,uid,act,info,backurl";
            string strType = "text,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + ptype + "'" + uid + "'delblack'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定解黑,speak.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=blist&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void SpkAdminSubPage(int ptype)
    {
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "闲聊管理员";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + BCW.User.AppCase.CaseAction(ptype) + "&gt;管理员");
        builder.Append(Out.Tab("</div>", ""));
        string SpkAdmin = "";
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            SpkAdmin = ub.GetSub("SpkAdmin" + ptype + "", xmlPath);
        }
        else
        {
            SpkAdmin = ub.GetSub("SpkAdmin0", xmlPath);
        }
        if (SpkAdmin != "")
        {
            string[] sName = Regex.Split(SpkAdmin, "#");
            int pageIndex;
            int recordCount;
            int pageSize = 15;
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = sName.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < sName.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + sName[i] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(sName[i])) + "</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        }
        else
        {
            builder.Append("还没有管理员，欢迎申请..");
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void SpkAdminPage(int ptype)
    {
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "闲聊总管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;闲聊总管");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Status=0 and Rolece collate Chinese_PRC_CS_AS_WS like '%N%'";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRolesAdmin(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>");

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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=admin&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        if (!new BCW.User.Role().IsSpkRole(ptype, meid))
        {
            Utils.Error("你的权限不足", "");
        }
        string mename = new BCW.BLL.User().GetUsName(meid);
        Master.Title = "管理闲聊记录";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "闲聊ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int v = int.Parse(Utils.GetRequest("v", "all", 1, @"^[0-1]$", "0"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
        {
            Utils.Error("不存在的会员", "");
        }
        if (!new BCW.BLL.Speak().Exists(id))
        {
            Utils.Error("不存在的闲聊记录", "");
        }
        //是否置顶
        int IsTop = new BCW.BLL.Speak().GetIsTop(id);

        if (info == "ok")
        {
            int btype = int.Parse(Utils.GetRequest("btype", "post", 2, @"^[0-2]$", "执行类型错误"));
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,50}$", "理由限50字内，可留空");
            string bText = "某条";
            if (btype == 0)
            {
                new BCW.BLL.Speak().Delete("Types=" + ptype + " and ID=" + id + "");
                bText = "某条";
            }
            else if (btype == 1)
            {
                new BCW.BLL.Speak().Delete("Types=" + ptype + " and UsID=" + uid + "");
            }
            if (btype != 2)
            {
                //记录日志
                string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]的" + bText + "闲聊记录删除";
                string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将你在游戏闲聊中的" + bText + "闲聊记录删除";

                if (!string.IsNullOrEmpty(Why))
                {
                    strLog += ",理由:" + Why + "";
                    strLog2 += ",理由:" + Why + "";
                }

                new BCW.BLL.Forumlog().Add(12, ptype, strLog);
                new BCW.BLL.Guest().Add(0, uid, UsName, strLog2);
                if (v == 1)
                    Utils.Success("删除闲聊记录", "删除" + UsName + "(" + uid + ")" + bText + "闲聊记录成功，正在返回..", Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
                else
                    Utils.Success("删除闲聊记录", "删除" + UsName + "(" + uid + ")" + bText + "闲聊记录成功，正在返回..", Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + ""), "1");
            }
            else//置顶本条
            {
                if (IsTop == 0)
                {
                    new BCW.BLL.Speak().UpdateIsTop(id, 1);
                    bText = "置顶";
                }
                else
                {
                    new BCW.BLL.Speak().UpdateIsTop(id, 0);
                    bText = "去顶";
                }
                //记录日志
                string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]的某条闲聊记录" + bText + "";
                string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将你在游戏闲聊中的某条闲聊记录" + bText + "";

                if (!string.IsNullOrEmpty(Why))
                {
                    strLog += ",理由:" + Why + "";
                    strLog2 += ",理由:" + Why + "";
                }

                new BCW.BLL.Forumlog().Add(12, ptype, strLog);
                new BCW.BLL.Guest().Add(0, uid, UsName, strLog2);
                if (v == 1)
                    Utils.Success("闲聊" + bText + "", "闲聊" + bText + "成功，正在返回..", Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
                else
                    Utils.Success("闲聊" + bText + "", "闲聊" + bText + "成功，正在返回..", Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("闲聊对象:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a><a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?act=addblack&amp;uid=" + uid + "&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;v=" + v + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[加黑]</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "操作:,理由:,,,,,,,,";
            string strName = "btype,Why,ptype,showtype,v,id,uid,act,info,backurl";
            string strType = "select,text,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "2''" + ptype + "'" + showtype + "'" + v + "'" + id + "'" + uid + "'del'ok'" + Utils.getPage(0) + "";
            string strEmpt = "";
            if (IsTop == 0)
                strEmpt = "0|删除本条|1|删除TA所有|2|置顶本条,true,false,false,false,false,false,false,false,false";
            else
                strEmpt = "0|删除本条|1|删除TA所有|2|去顶本条,true,false,false,false,false,false,false,false,false";

            string strIdea = "/";
            string strOthe = "确定执行,speak.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            if (v == 1)
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            else
                builder.Append("<a href=\"" + Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "") + "\">再看看吧..</a>");

            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void SpkAddPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "闲聊发言";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int v = int.Parse(Utils.GetRequest("v", "all", 1, @"^[0-1]$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string sk = Utils.GetRequest("sk", "all", 1, "", "");
        string UsName = new BCW.BLL.User().GetUsName(uid);
        string z1 = "";//20161020sgl
        if (UsName == "")
        {
            Utils.Error("不存在的会员", "");
        }
        if (info == "ok")
        {
            int IsKiss = int.Parse(Utils.GetRequest("IsKiss", "post", 1, @"^[0-1]$", "0"));
            string SpeakContent = Utils.GetRequest("SpeakContent", "post", 1, "", "");
            int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));
            if (Face > 0 & Face < 27)
                SpeakContent = "[F]" + Face + "[/F]" + SpeakContent;

            int aa = int.Parse(Utils.GetRequest("aa", "all", 1, @"^[1-9]\d*$", "0"));
            if (aa > 0 && aa < 55)
            {
                if (uid == 0)
                    SpeakContent = "" + BCW.User.ChatAction.GetAction(aa)[1];
                else if (uid != meid)
                    SpeakContent = "" + BCW.User.ChatAction.GetAction(aa)[2];
                else if (uid == meid)
                    SpeakContent = "" + BCW.User.ChatAction.GetAction(aa)[3];
            }

            if (SpeakContent == "")
            {
                Utils.Error("内容限50字内", "");
            }

            //是否刷屏
            if (aa > 0 && aa < 55)
            {
                //1分钟内限发动作一次
                string appName = "LIGHT_SPEAK_ZD";
                string CacheKey = appName + "_" + Utils.Mid(Utils.getstrU(), 0, Utils.getstrU().Length - 4);
                object getObjCacheTime = DataCache.GetCache(CacheKey);
                if (getObjCacheTime != null)
                {
                    Utils.Error("1分钟内限发动作1次", "");
                }
                object ObjCacheTime = DateTime.Now;
                DataCache.SetCache(CacheKey, ObjCacheTime, DateTime.Now.AddSeconds(60), TimeSpan.Zero);

            }
            else
            {
                string appName = "LIGHT_SPEAK";
                int Expir = Convert.ToInt32(ub.GetSub("BbsSpeakExpir", "/Controls/bbs.xml"));
                BCW.User.Users.IsFresh(appName, Expir);

            }

            BCW.User.Users.ShowVerifyRole("h", meid);//非验证会员提示
            new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Speak, meid);//会员本身权限

            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            {
                if (new BCW.BLL.Spkblack().Exists(meid, 0))
                {
                    Utils.Error("你已在本游戏黑名单里", "");
                }
            }
            else
            {
                if (new BCW.BLL.Spkblack().Exists(meid, ptype))
                {
                    Utils.Error("你已在本游戏黑名单里", "");
                }
            }
            if (uid == meid)
            {
                Utils.Error("不能对自己发言", "");
            }
            BCW.Model.Speak addmodel = new BCW.Model.Speak();
            addmodel.Types = ptype;
            addmodel.NodeId = 0;
            addmodel.UsId = meid;
            addmodel.UsName = mename;
            addmodel.ToId = uid;
            addmodel.ToName = UsName;
            addmodel.Notes = Utils.Left(SpeakContent, 520);
            addmodel.AddTime = DateTime.Now;
            if (aa > 0 && aa < 55)
            {
                addmodel.IsKiss = 2;
            }
            else
            {
                addmodel.IsKiss = IsKiss;
            }

            //2016/10/20 sgl
            int speakid = new BCW.BLL.Speak().Add(addmodel);
            if (aa > 0 && aa < 55)
            {
                new BCW.HB.BLL.ChatMe().UpdateSpeakType(speakid);
            }
            else
            {
                if (Face > 0 & Face < 27)//发表情
                {
                    z1 = "";
                }
                else
                {
                    //闲聊奖励派币
                    int countss = new BCW.BLL.Speak().GetCount("UsId=" + meid + " AND Notes NOT LIKE '%[F]%' and Types=0 and datediff(day,AddTime,getdate())=0");
                    if (addmodel.Notes.Trim() != "")
                    {

                        if (countss <= Convert.ToInt32(ub.GetSub("SpkLimit", xmlPath)))
                        {
                            z1 = "对聊发言奖币" + Convert.ToInt32(ub.GetSub("SpkOne", xmlPath)) + ",";//20161020sgl
                            new BCW.BLL.User().UpdateiGold(meid, mename, Convert.ToInt32(ub.GetSub("SpkOne", xmlPath)), "对聊发言奖币！");
                        }
                    }
                }

            }

            //闲聊奖励2016/4/23 Daisy
            // BCW.User.Users.SpeakOutCent(ptype, meid, mename);


            //内线通知开启对聊提醒的会员
            string sIskiss = "";
            if (addmodel.IsKiss == 1)
                sIskiss = "秘密";

            new BCW.BLL.Guest().Add(4, uid, UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在闲聊对你" + sIskiss + "发言:" + SpeakContent.Replace("$", "") + "[url=/bbs/game/speak.aspx?act=spkadd&amp;ptype=" + ptype + "&amp;showtype=2&amp;uid=" + meid + "&amp;v=1]马上回复[/url]");

            if (v == 1)
                Utils.Success("发言", "发言成功，" + z1 + "正在返回..", Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            else
                Utils.Success("发言", "发言成功，" + z1 + "正在返回..", Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + ""), "1");

        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("与会员:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>对聊");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "内容:,表情:,性质:,,,,,,,,";
        string strName = "SpeakContent,Face,IsKiss,ptype,showtype,uid,sk,v,act,info,backurl";
        string strType = "text,select,select,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
        string strValu = "'0'0'" + ptype + "'" + showtype + "'" + uid + "'" + sk + "'" + v + "'spkadd'ok'" + Utils.getPage(0) + "";
        string strEmpt = "true,0|选择|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,0|公聊|1|秘密,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定发言|动作,speak.aspx,post,1,red|other";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=chat&amp;hid=" + uid + "&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">与TA的对话记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/guest.aspx?act=add&amp;hid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">给TA发内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ActionPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "选择动作";
        Master.IsFoot = false;
        builder.Append(Out.Tab("<div class=\"title\">选择动作</div>", ""));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int v = int.Parse(Utils.GetRequest("v", "all", 1, @"^[0-1]$", "0"));
        if (uid > 0)
        {
            if (!new BCW.BLL.User().Exists(uid))
            {
                Utils.Error("不存在的会员", "");
            }
        }
        int pageIndex;
        int recordCount;
        int pageSize = 20;
        string[] pageValUrl = { "ac", "ptype", "showtype", "uid", "hb", "v", "backurl" };
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
                if (uid > 0)
                    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=spkadd&amp;info=ok&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;v=" + v + "&amp;uid=" + uid + "&amp;aa=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + BCW.User.ChatAction.GetAction(i)[0] + "</a>\r\n");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;v=" + v + "&amp;aa=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + BCW.User.ChatAction.GetAction(i)[0] + "</a>\r\n");

                if ((k + 1) > 0 && (k + 1) != 20 && (k + 1) != 40 && (k + 1) % 5 == 0)
                    builder.Append("<br />");
            }
            if (k == endIndex)
                break;
            k++;
        }
        builder.Append(Out.Tab("</div>", ""));
        //分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    /// <summary>
    /// 刷新设置
    /// </summary>
    private void FreshPage(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "30"));
            int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));
            int IsMsg = int.Parse(Utils.GetRequest("IsMsg", "all", 1, @"^[0-1]$", "0"));
            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 30)
                {
                    sforumsets += "," + sfs[0] + "|" + tm;
                }
                else if (i == 31)
                {
                    sforumsets += "," + sfs[0] + "|" + pn;
                }
                else if (i == 33)
                {
                    sforumsets += "," + sfs[0] + "|" + IsMsg;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(meid, sforumsets);

            Utils.Success("刷新设置", "恭喜，刷新设置成功！正在返回...", Utils.getUrl("speak.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
        else
        {
            Master.Title = "刷新设置";
            builder.Append(Out.Tab("<div class=\"title\">刷新设置</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择秒数:");
            builder.Append(Out.Tab("</div>", ""));
            int tm = BCW.User.Users.GetForumSet(ForumSet, 30);
            int pn = BCW.User.Users.GetForumSet(ForumSet, 31);
            int IsMsg = BCW.User.Users.GetForumSet(ForumSet, 33);

            string strText = ",页面条数:/,对聊提醒:/,,,,,,";
            string strName = "tm,pn,IsMsg,id,ptype,showtype,act,info,backurl";
            string strType = "select,select,select,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + tm + "'" + pn + "'" + IsMsg + "'" + id + "'" + ptype + "'" + showtype + "'fresh'ok'" + Utils.getPage(0) + "";
            string strEmpt = "0|手动|5|5秒|10|10秒|15|15秒|20|20秒|30|30秒|40|40秒|50|50秒|60|60秒|90|90秒,6|6条|8|8条|10|10条|15|15条|20|20条|25|25条|30|30条,0|是|1|否,,,,,,";
            string strIdea = "/";
            string strOthe = "确定,speak.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?id=" + id + "&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void StatInfoPage(int ptype)
    {
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-2]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "" + sName[ptype] + "闲聊统计";

        string Whe = "";
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            Whe = " and Types=" + ptype + "";
        }



        string M_Str_mindate;//用于存储最小日期
        string M_Str_maxdate;//用于存储最大日期
        //今日计算
        M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.AddDays(1).ToShortDateString() + " 0:00:00";
        int TodayCount = new BCW.BLL.Speak().GetCount("AddTime>='" + M_Str_mindate + "' AND AddTime< '" + M_Str_maxdate + "' " + Whe + " AND Type=0 ");

        //昨日计算
        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.ToShortDateString() + " 0:00:00";
        int YesterdayCount = new BCW.BLL.Speak().GetCount("AddTime>='" + M_Str_mindate + "' AND AddTime< '" + M_Str_maxdate + "'  " + Whe + " AND Type=0 ");

        //本周计算
        int WeekCount = new BCW.BLL.Speak().GetCount("AddTime>='" + DT.GetWeekStart() + "' AND AddTime< '" + DT.GetWeekOver() + "'  " + Whe + " AND Type=0 ");
        //上周计算
        int BfWeekCount = new BCW.BLL.Speak().GetCount("AddTime>='" + DateTime.Parse(DT.GetWeekStart()).AddDays(-7) + "' AND AddTime< '" + DT.GetWeekStart() + "'  " + Whe + " AND Type=0 ");
        //本月计算
        int MonthCount = new BCW.BLL.Speak().GetCount("Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + " " + Whe + " AND Type=0 ");
        //上月计算
        DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
        int ForYear = ForDate.Year;
        int ForMonth = ForDate.Month;
        int BfMonthCount = new BCW.BLL.Speak().GetCount("Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + " " + Whe + " AND Type=0 ");

        builder.Append(Out.Tab("<div class=\"title\">" + sName[ptype] + "闲聊统计</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今日记录：" + TodayCount + "");
        builder.Append("<br />昨日记录：" + YesterdayCount + "");
        builder.Append("<br />本周记录：" + WeekCount + "");
        builder.Append("<br />上周记录：" + BfWeekCount + "");
        builder.Append("<br />本月记录：" + MonthCount + "");
        builder.Append("<br />上月记录：" + BfMonthCount + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏闲聊排行榜&gt;&gt;</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top2&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">会员闲聊排行榜&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?id=" + id + "&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void StatTopPage(int ptype)
    {
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-3]$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "闲聊排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("游戏闲聊排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("排行:");

        if (showtype == 0)
            builder.Append("本周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">本周</a>|");

        if (showtype == 1)
            builder.Append("上周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">上周</a>|");

        if (showtype == 2)
            builder.Append("本月|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>|");

        if (showtype == 3)
            builder.Append("上月");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">上月</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";

        //本周计算
        if (showtype == 0)
            strWhere = "AddTime>='" + DT.GetWeekStart() + "' AND AddTime< '" + DT.GetWeekOver() + "'  AND Type=0 ";

        //上周计算
        else if (showtype == 1)
            strWhere = "AddTime>='" + DateTime.Parse(DT.GetWeekStart()).AddDays(-7) + "' AND AddTime< '" + DT.GetWeekStart() + "'  AND Type=0 ";

        //本月计算
        else if (showtype == 2)
            strWhere = "Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + " AND Type=0 ";
        //上月计算
        else if (showtype == 3)
        {
            DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
            int ForYear = ForDate.Year;
            int ForMonth = ForDate.Month;
            strWhere = "Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + " AND Type=0 ";
        }

        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Speak> listSpeak = new BCW.BLL.Speak().GetSpeaksTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpeak.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Speak n in listSpeak)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]" + BCW.User.AppCase.CaseAction(n.ID) + ":" + n.Types + "条");

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
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void StatTop2Page(int ptype)
    {
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-3]$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "闲聊排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("会员闲聊排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("排行:");

        if (showtype == 0)
            builder.Append("本周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top2&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">本周</a>|");

        if (showtype == 1)
            builder.Append("上周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top2&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">上周</a>|");

        if (showtype == 2)
            builder.Append("本月|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top2&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>|");

        if (showtype == 3)
            builder.Append("上月");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=top2&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">上月</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";

        //本周计算
        if (showtype == 0)
            strWhere = "UsId>0 and AddTime>='" + DT.GetWeekStart() + "' AND AddTime< '" + DT.GetWeekOver() + "'  AND Type=0 ";

        //上周计算
        else if (showtype == 1)
            strWhere = "UsId>0 and AddTime>='" + DateTime.Parse(DT.GetWeekStart()).AddDays(-7) + "' AND AddTime< '" + DT.GetWeekStart() + "'  AND Type=0 ";

        //本月计算
        else if (showtype == 2)
            strWhere = "UsId>0 and Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + " AND Type=0 ";
        //上月计算
        else if (showtype == 3)
        {
            DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
            int ForYear = ForDate.Year;
            int ForMonth = ForDate.Month;
            strWhere = "UsId>0 and Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + " AND Type=0 ";
        }

        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Speak> listSpeak = new BCW.BLL.Speak().GetSpeaksTop2(pageIndex, pageSize, strWhere, out recordCount);
        if (listSpeak.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Speak n in listSpeak)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsId) + "</a>:" + n.Types + "条");

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
        builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    /// <summary>
    /// 发红包
    /// </summary>
    /// <param name="ptype"></param>
    /// <param name="meid"></param>
    public void PostHb(int id, int meid, int tm, int pn)
    {
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;
        string strtest;
        int HbSpeed = Utils.ParseInt(ub.GetSub("HbSpeed", xmlPath3));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        if (new BCW.HB.BLL.HbPost().Exists(meid, 0))
        {
            List<BCW.HB.Model.HbPost> hbwillpost = new BCW.HB.BLL.HbPost().GetModelList("ChatID=" + id + " and State=0 and UserID=" + meid);
            for (int ips = 0; ips < hbwillpost.Count; ips++)
            {
                BCW.Model.Speak addmodel = new BCW.Model.Speak();
                addmodel.Types = hbwillpost[ips].ChatID;
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
                addmodel.NodeId = ptype;
                addmodel.UsId = meid;
                addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                addmodel.ToId = 0;
                addmodel.ToName = "";
                addmodel.AddTime = DateTime.Now;
                addmodel.IsKiss = 0;
                if (hbwillpost[ips].RadomNum.Trim() == "pt")
                {
                    addmodel.Notes = Utils.Left("发了一个[url=/bbs/chathb.aspx?act=gethb&amp;id=" + hbwillpost[ips].ID + "]普通红包[/url]" + strtest + ":" + hbwillpost[ips].Note, 120);
                }
                else
                {
                    addmodel.Notes = Utils.Left("发了一个[url=/bbs/chathb.aspx?act=gethb&amp;id=" + hbwillpost[ips].ID + "]拼手气红包[/url]" + strtest + ":" + hbwillpost[ips].Note, 120);
                }
                new BCW.BLL.Speak().Add(addmodel);
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
                builder2.Append("<br/><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + hbpost[iso].UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + postname + " </a>(" + hbpost[iso].UserID + ")<a href=\"" + Utils.getUrl("/bbs/chathb.aspx?act=gethb&amp;id=" + hbpost[iso].ID) + "\">发了一个<img src=\"/bbs/game/img/hb.gif\" height=\"21px\" width=\"14px\"  style=\"margin:auto\" alt=\"load\" />" + strtest + ":" + hbpost[iso].Note + "</a>");
            }

        }
        string lblblb = new BCW.JS.somejs().topfloat(builder2.ToString(), "marquee", "30", HbSpeed.ToString());
        if (builder2.ToString().Trim() == "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;puthb=1&amp;tm=" + tm + "&amp;hb=1&amp;pn=" + pn + "&amp;backurl=" + Utils.getPage(0) + "") + "\">没有红包可以领取！快去发一个吧！</a> ");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            builder.Append(lblblb);
        }
    }
    public void putshb(int id, int meid, int tm, int pn)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string strText = ",,,,,";
        string strName = "id,tm,pn,speakid,act,backurl";
        string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
        string strValu = id + "'" + tm + "'" + pn + "'" + ptype + "'save'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "我要发红包|红包玩花样,/bbs/chathb.aspx,post,3,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }
    /// <summary>
    /// 发图片
    /// </summary>
    /// <param name="ptype"></param>
    /// <param name="meid"></param>
    private void PostPhoto(int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int ptyp = int.Parse(Utils.GetRequest("ptyp", "all", 1, @"^[0-2]\d*$", "0"));
        int XcID = int.Parse(Utils.GetRequest("xcid", "all", 1, @"^[0-9]\d*$", "0"));
        int ToID = int.Parse(Utils.GetRequest("ToID", "all", 1, @"^[0-9]\d*$", "0"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "10"));
        int pn = int.Parse(Utils.GetRequest("pn", "all", 1, @"^[1-9]\d*$", "10"));

        Master.Title = "游戏闲聊发图片";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=选择图片=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptyp == 0)
            builder.Append("直接上传|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=posph" + "&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=0") + "\">直接上传</a>|");

        if (ptyp == 1)
            builder.Append("从相册选择");
        else
            builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=posph" + "&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=1") + "\">从相册选择</a>");
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
                builder.Append("<a href=\"speak.aspx?act=posph&amp;ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
            }
            else
            {
                string maxfile = "5000";
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("上传允许格式:.gif,.jpg,.jpeg,.png<br />上传后的图片将自动保存到您的游戏闲聊相册<br />");
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
                string strText = string.Empty;
                string strName = string.Empty;
                string strType = string.Empty;
                string strValu = string.Empty;
                string strEmpt = string.Empty;
                string strIdea = string.Empty;
                string strOthe = string.Empty;
                strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
                strName = sName + Utils.Mid(strName, 1, strName.Length) + ",hb,leibie,act,ptype";
                strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
                strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + hbgn + "'" + leibie + "'upload'" + ptype + "";
                strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,";
                strIdea = "/";
                strOthe = "确定发送|reset,speak.aspx?act=suss,post,2,red|blue";
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
                    builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=posph&amp;ptype=" + ptype + "&amp;" + "&amp;hb=" + hbgn + "tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=1" + "&amp;xcid=" + n.ID) + "\">" + n.Title + "(" + new BCW.BLL.Upfile().GetCount(uid, n.ID) + "张)</a>");

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
                builder.Append("<a href=\"" + Utils.getUrl("speak.aspx?act=posph" + "&amp;hb=" + hbgn + "&amp;ptype=" + ptype + "&amp;tm=" + tm + "&amp;pn=" + pn + "&amp;ToID=" + ToID + "&amp;ptyp=1" + "&amp;xcid=0" + "") + "\">未归类相集(" + new BCW.BLL.Upfile().GetCount(uid, leibie, 0) + ")</a>");
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
            builder.Append("<form id=\"form1\" method=\"post\" action=\"speak.aspx\">");
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
                    builder.Append("<a target=\"_blank\" href=\"" + Utils.getUrl(n.Files) + "\">查看原图.</a>");
                    if (n.IsVerify != 1)
                        builder.Append("选择<input type=\"radio\" name=\"phid\" value=\"" + n.ID + "\" />");
                    k++;
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
                // 分页

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"suss2\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptyp\" Value=\"" + ptyp + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
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
        builder.Append("<a href=\"" + Utils.getPage("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn + "&amp;tm=" + tm + "&amp;pn=" + pn + "") + "\">&gt;返回房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void SubmitPage(int ptype)
    {
        BCW.User.Users.IsFresh("发图", 5);
        string stext = Utils.GetRequest("stext", "all", 3, @"^[\s\S]{1,30}$", "发言长度超出限制");
        int photo = int.Parse(Utils.GetRequest("phid", "all", 2, @"^[0-9]\d*$", "图片ID选择错误"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        BCW.Model.Upfile mol = new BCW.BLL.Upfile().GetUpfile(photo, 1);
        BCW.Model.Speak addmodel = new BCW.Model.Speak();
        addmodel.NodeId = ptype;
        addmodel.UsId = meid;
        addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
        addmodel.ToId = 0;
        addmodel.ToName = "";
        addmodel.AddTime = DateTime.Now;
        addmodel.IsKiss = 0;
        addmodel.Notes = stext + "<br />[url=/showpic.aspx?pic=" + mol.Files + "&speak=" + ptype + "]<img  height=\"45px\" src=\"" + mol.Files + "\" alt=\"load\"/>[/url]";
        new BCW.BLL.Speak().Add(addmodel);
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn)), "1");
        }
        else
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn)), "0");
        }
    }
    private void UploadPage(int ptype)
    {
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.User.Users.IsFresh("发图", 5);
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "all", 1, @"^[0-9]\d*$", "0"));
        string stext = Utils.GetRequest("stext", "all", 3, @"^[\s\S]{1,30}$", "发言长度超出限制");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        DataSet modelcount = new BCW.BLL.Upgroup().GetList("count(*)", "UsID=" + meid + " and Title='闲聊相册'");
        int coo = Convert.ToInt32(modelcount.Tables[0].Rows[0][0]);
        if (coo == 0)
        {
            BCW.Model.Upgroup model = new BCW.Model.Upgroup();
            model.Leibie = leibie;
            model.Types = 0;
            model.PostType = 1;
            model.Title = "闲聊相册";
            model.UsID = meid;
            model.IsReview = 0;
            model.Paixu = 0;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Upgroup().Add(model);
        }
        DataSet melbp = new BCW.BLL.Upgroup().GetList("*", "UsID=" + meid + " and Title='闲聊相册'");
        NodeId = Convert.ToInt32(melbp.Tables[0].Rows[0][0]);

        //上传文件
        int kk = 0;
        int ios = 0;
        SaveFiles(meid, leibie, NodeId, out kk, out ios);
        BCW.Model.Upfile mol = new BCW.BLL.Upfile().GetUpfile(ios, 1);
        BCW.Model.Speak addmodel = new BCW.Model.Speak();
        addmodel.NodeId = ptype;
        addmodel.UsId = meid;
        addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
        addmodel.ToId = 0;
        addmodel.ToName = "";
        addmodel.AddTime = DateTime.Now;
        addmodel.IsKiss = 0;
        addmodel.Notes = stext + "<br />[url=/showpic.aspx?pic=" + mol.Files + "&speak=" + ptype + "]<img  height=\"45px\" src=\"" + mol.Files + "\" alt=\"load\"/>[/url]";
        new BCW.BLL.Speak().Add(addmodel);
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn)), "1");
        }
        else
        {
            Utils.Success("发送图片", "图片发送成功！", ReplaceWap(Utils.getUrl("speak.aspx?ptype=" + ptype + "&amp;hb=" + hbgn)), "0");
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
}