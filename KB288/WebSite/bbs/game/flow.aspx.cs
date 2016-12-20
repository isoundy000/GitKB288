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

public partial class bbs_game_flow : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/flow.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = ub.GetSub("FlowName", xmlPath);
        //维护提示
        if (ub.GetSub("FlowStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "getall":
                GetAllPage();
                break;
            case "bw":
                BwPage();
                break;
            case "put":
                PutPage();
                break;
            case "friend":
                FriendPage();
                break;
            case "putlist":
                PutListPage();
                break;
            case "putok":
                PutOkPage();
                break;
            case "house":
                HousePage();
                break;
            case "shop":
                ShopPage();
                break;
            case "pay":
                PayPage();
                break;
            case "prop":
                PropPage();
                break;
            case "propac":
                PropAcPage();
                break;
            case "help":
                HelpPage();
                break;
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Weather();//更新天气

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        bool Isme = true;
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string UsName = "";
        if (uid > 0 && uid != meid)
        {
            UsName = new BCW.BLL.User().GetUsName(uid);
            if (UsName == "")
                Utils.Error("不存在的会员", "");

            Isme = false;
        }
        else
        {
            UsName = new BCW.BLL.User().GetUsName(meid);
            uid = meid;
        }
        CheckPage(uid);

        string Logo = ub.GetSub("FlowLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string Notes = ub.GetSub("FlowNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", ""));
        }

        //读取实体
        BCW.Model.Game.flowuser model = new BCW.BLL.Game.flowuser().Getflowuser(uid);
        if (model == null)
        {
            model = new BCW.Model.Game.flowuser();
            model.iFlows = 9;
            model.Score = 0;
            model.Score2 = 0;
            model.Score3 = 0;
            model.Score4 = 0;
            model.Score5 = 0;
            model.UsID = meid;
            model.UsName = new BCW.BLL.User().GetUsName(uid);
            model.FlowStat = "";
            model.AddTime = DateTime.Now;
            new BCW.BLL.Game.flowuser().Add(model);
        }

        int rows = 0;
        DataSet ds = new BCW.BLL.Game.flows().GetList("ID,ztitle", "UsID=" + uid + "");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            rows = ds.Tables[0].Rows.Count;
        }
        //自动添加花盆

        int j = model.iFlows;
        if (rows < j)
        {
            for (int i = 0; i < (j - rows); i++)
            {
                BCW.Model.Game.flows m = new BCW.Model.Game.flows();
                m.UsID = uid;
                m.zid = 0;
                m.ztitle = "空花盆";
                m.water = 0;
                m.weeds = 0;
                m.worm = 0;
                m.state = 0;
                m.cnum = 0;
                m.checkuid = 0;
                m.cnum2 = 0;
                new BCW.BLL.Game.flows().Add(m);
            }
            ds = new BCW.BLL.Game.flows().GetList("ID,ztitle", "UsID=" + uid + "");
        }
        //分析积分XML
        int score = new BCW.BLL.Game.flowuser().GetScore(uid);
        int score2 = new BCW.BLL.Game.flowuser().GetScore2(uid);

        int leven = 0;
        int leven2 = 0;
        string title = "花农";
        string title2 = "绿叶";
        for (int i = 1; i <= 40; i++)
        {
            string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
            string[] temp = str.Split("|".ToCharArray());
            int Iscore = Convert.ToInt32(temp[0]);
            if (score >= Iscore)
            {
                leven = i;
                title = temp[1];
            }
            if (score2 >= Iscore)
            {
                leven2 = i;
                title2 = temp[2];
            }
        }

        if (Isme)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("天气:<img src=\"/Files/sys/game/flow/weather/" + ub.GetSub("FlowWeather", xmlPath) + ".gif\" alt=\"load\"/>" + ub.GetSub("FlowWeather", xmlPath) + " 水份:" + ub.GetSub("FlowWater", xmlPath) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=putlist") + "\">我的花盆(" + new BCW.BLL.Game.flows().GetCount(uid, -1) + "/" + j + ")</a>|<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;backurl=" + Utils.PostPage(1) + "") + "\">风云榜</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/Files/sys/game/flow/home.gif\" alt=\"load\"/><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>的花谷");
            builder.Append("<br />技能等级:<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + leven + "级(" + title + ")</a>");
            builder.Append("<br />风采等级:<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + leven2 + "级(" + title2 + ")</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));
        if (!Isme)
        {
            builder.Append("天气:<img src=\"/Files/sys/game/flow/weather/" + ub.GetSub("FlowWeather", xmlPath) + ".gif\" alt=\"load\"/>" + ub.GetSub("FlowWeather", xmlPath) + " 水份:" + ub.GetSub("FlowWater", xmlPath) + "<br />");

            if (new BCW.BLL.Friend().Exists(meid, uid, 0))
            {
                builder.Append("<small><a href=\"" + Utils.getUrl("flow.aspx?act=bw&amp;uid=" + uid + "&amp;p=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">种草</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=bw&amp;uid=" + uid + "&amp;p=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">放虫</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">帮收</a></small>");
            }
            builder.Append("<br />TA的花盆:<br />");

        }

        int pageIndex;
        int recordCount;
        int pageSize = 9;
        string[] pageValUrl = { "p", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //总记录数
        recordCount = j;

        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        for (int i = 1; i <= recordCount; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                int id = int.Parse(ds.Tables[0].Rows[i - 1]["ID"].ToString());
                string ztitle = ds.Tables[0].Rows[i - 1]["ztitle"].ToString();
                string strName = "flowid,p,uid,act,backurl";
                string strValu = "" + id + "'" + uid + "'1'put'" + Utils.PostPage(0) + "";
                if (ztitle == "空花盆")
                    strValu = "" + id + "'1'" + uid + "'put'" + Utils.PostPage(0) + "";

                string strOthe = "" + ztitle + ",flow.aspx,post,3,input2";
                if (ztitle == "空花盆" && meid != uid)
                {
                    builder.Append("空花盆");
                }
                else
                {
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                if (i % 3 == 0 && i % 9 != 0)
                    builder.Append("<br />");
                else if (i % 3 != 0 && i % 9 != 0)
                    builder.Append("-");
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img src=\"/Files/sys/game/flow/hr.gif\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (Isme)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;p=1") + "\">一键播种</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=getall") + "\">一键收获</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/Files/sys/game/flow/home.gif\" alt=\"load\"/>");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?uid=" + uid + "") + "\">" + UsName + "</a>的花谷<br />");
            builder.Append("技能等级:<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=1") + "\">" + leven + "级(" + title + ")</a><br />");
            builder.Append("风采等级:<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=2") + "\">" + leven2 + "级(" + title2 + ")</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=house") + "\">仓库</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=shop") + "\">商店</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop") + "\">道具</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=friend") + "\">好友</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=help") + "\">帮助</a>");
            builder.Append(Out.Tab("</div>", Out.Hr()));
        }
        else
        {
            builder.Append("<img src=\"/Files/sys/game/flow/h.gif\" alt=\"load\"/>");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">我的百花谷</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=house") + "\">仓库</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=friend") + "\">好友</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=help") + "\">帮助</a>");
            builder.Append(Out.Tab("</div>", Out.Hr()));
        }

        if (Isme)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("我的动态|");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=40&amp;backurl=" + Utils.PostPage(1) + "") + "\">世界动态</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("TA动态|");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=40&amp;backurl=" + Utils.PostPage(1) + "") + "\">世界动态</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        // 开始读取动态列表
        int SizeNum = 3;
        string strWhere = "Types=40";
        strWhere += "and UsID=" + uid + "";

        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        if (listAction.Count > 0)
        {
            int kk = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                ForNotes = ForNotes.Replace("百花谷", "");
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                kk++;
            }
            if (kk > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=40&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多&gt;&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(20, "flow.aspx?uid=" + uid + "", 5, 0)));
        //游戏底部Ubb
        string Foot = ub.GetSub("FlowFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">婚姻殿堂</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void GetAllPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-一键收获";
        //如果没有一键收获的道具
        DataSet dss = new BCW.BLL.Game.flowmyprop().GetList("id", "UsID=" + meid + " and did=6 and ExTime>'" + DateTime.Now + "'");
        if (dss == null || dss.Tables[0].Rows.Count == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("没有正在使用的一键收获道具..<br />");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop") + "\">进入道具市场&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            CheckPage(meid);
            DataSet ds = new BCW.BLL.Game.flows().GetList("id,zid,ztitle,weeds,worm,water,cnum,AddTime", "UsID=" + meid + " and state=4");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string OutText = "";
                string mename = new BCW.BLL.User().GetUsName(meid);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                    int zid = int.Parse(ds.Tables[0].Rows[i]["zid"].ToString());
                    string ztitle = ds.Tables[0].Rows[i]["ztitle"].ToString();
                    int weeds = int.Parse(ds.Tables[0].Rows[i]["weeds"].ToString());
                    int worm = int.Parse(ds.Tables[0].Rows[i]["worm"].ToString());
                    int water = int.Parse(ds.Tables[0].Rows[i]["water"].ToString());
                    int cnum = int.Parse(ds.Tables[0].Rows[i]["cnum"].ToString());
                    DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());

                    //--------------逻辑--------------
                    int ExTime = 0;
                    if (weeds > 0)
                    {
                        ExTime = Convert.ToInt32(weeds * 15);
                    }
                    int gNum = cnum;

                    if (worm > 0)
                    {
                        if (AddTime.AddMinutes(300 + ExTime + 60) < DateTime.Now)
                        {
                            gNum = gNum - worm;
                            if (gNum < 0)
                                gNum = 0;
                        }
                    }

                    if (water < 50)
                    {
                        if (AddTime.AddMinutes(300 + ExTime + 60) < DateTime.Now)
                        {
                            long hour = DT.DateDiff(DateTime.Now, AddTime.AddMinutes(300 + ExTime + 60), 2);
                            int hour2 = Convert.ToInt32(hour);
                            gNum = gNum - hour2;
                            if (gNum < 0)
                                gNum = 0;

                        }
                    }
                    


                    //--------------逻辑--------------
                    //如果有增产肥料道具则计算得到多少花卉
                    DataSet ds2 = new BCW.BLL.Game.flowmyprop().GetList("id,znum", "UsID=" + meid + " and did=5 and znum>0");
                    if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                    {
                        int zid2 = int.Parse(ds2.Tables[0].Rows[0]["id"].ToString());
                        int znum = int.Parse(ds2.Tables[0].Rows[0]["znum"].ToString());
                        gNum += znum * 15;
                        new BCW.BLL.Game.flowmyprop().Update(zid2, -znum);
                    }
                    //收获花卉
                    BCW.Model.Game.flowmyzz m = new BCW.Model.Game.flowmyzz();
                    m.UsID = meid;
                    m.UsName = mename;
                    m.zid = zid;
                    m.znum = gNum;
                    m.ztitle = ztitle;
                    m.Types = 1;
                    if (!new BCW.BLL.Game.flowmyzz().Exists(meid, m.Types, zid))
                    {
                        new BCW.BLL.Game.flowmyzz().Add(m);
                    }
                    else
                    {
                        new BCW.BLL.Game.flowmyzz().Update(m);
                    }
                    //增加技能积分
                    new BCW.BLL.Game.flowuser().UpdateScore(meid, 1, 5);
                    //增加花产量
                    new BCW.BLL.Game.flowuser().UpdateScore(meid, 3, gNum);
                    OutText += "," + ztitle + "" + gNum + "个,技术积分+5";

                    //收获后更新成空花盆
                    BCW.Model.Game.flows model = new BCW.Model.Game.flows();
                    model.ID = id;
                    model.state = 0;
                    model.zid = 0;
                    model.ztitle = "空花盆";
                    model.UsID = meid;
                    model.cnum = 0;
                    model.checkuid = 0;
                    model.AddTime = DateTime.Now;
                    new BCW.BLL.Game.flows().Update(model);
                }
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]对所有花盆进行一键收获";
                new BCW.BLL.Action().Add(40, 0, meid, "", wText);

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("收获成功" + OutText + "");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂时没有成熟的花卉！");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=house") + "\">我的仓库</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BwPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Friend().Exists(meid, uid, 0))
        {
            Utils.Error("非好友不能搞破坏哦", "");
        }
        if (!new BCW.BLL.Game.flows().Exists(uid))
        {
            Utils.Error("没有花卉不能搞破坏啊", "");
        }
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[1-2]$", "1"));
        BCW.Model.Game.flowuser model = new BCW.BLL.Game.flowuser().Getflowuser(uid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        if (p == 1)
        {
            //如果有杂草克星道具
            DataSet ds = new BCW.BLL.Game.flowmyprop().GetList("id", "UsID=" + uid + " and did=2 and ExTime>'" + DateTime.Now + "'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Utils.Error("主人有杂草克星道具保护，不能种草破坏", "");
            }
        }
        else
        {
            //如果有防虫电网道具
            DataSet ds = new BCW.BLL.Game.flowmyprop().GetList("id", "UsID=" + uid + " and did=3 and ExTime>'" + DateTime.Now + "'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Utils.Error("主人有防虫电网道具保护，不能放虫破坏", "");
            }
        }


        if (model.BwTime.ToLongDateString() != DateTime.Now.ToLongDateString())
        {
            new BCW.BLL.Game.flowuser().UpdateiBw(uid, 0);
        }
        else
        {
            if (model.iBw >= 80)
            {
                Utils.Error("该会员今天被搞破坏超80次啦，手下留情啊", "");
            }
            new BCW.BLL.Game.flowuser().UpdateiBw(uid, 1);
        }

        if (p == 1)
        {
            //TA的所有花盆增加一个杂草|放1颗草一个小时后会使对方的花卉成长时间延长15分钟
            new BCW.BLL.Game.flows().Updateweeds2(uid, 1);


            Master.Title = "百花谷-种草";
            builder.Append("<img src=\"/Files/sys/game/flow/bw1.gif\" alt=\"load\"/><br />你好坏啊在花盆里种杂草！<br />");
            builder.Append("<a href=\"" + Utils.getPage("flow.aspx?uid=" + uid + "") + "\">返回上级</a><br />");
            builder.Append("注:此功能针对花谷所有花盆");
        }
        else if (p == 2)
        {
            //TA的所有花盆增加一个害虫|放1只虫花卉成熟一个小时后会使产量减少1个
            new BCW.BLL.Game.flows().Updateworm2(uid, 1);


            Master.Title = "百花谷-放虫";
            builder.Append("<img src=\"/Files/sys/game/flow/bw2.gif\" alt=\"load\"/><br />嘿嘿...你将害虫放进花盆里了，够阴险<br />");
            builder.Append("<a href=\"" + Utils.getPage("flow.aspx?uid=" + uid + "") + "\">返回上级</a><br />");
            builder.Append("注:此功能针对花谷所有花盆");
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?uid=" + uid + "") + "\">返回TA的百花谷</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));


    }

    private void FriendPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-好友";
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-1]$", "0"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (p == 0)
            builder.Append("已成熟|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=friend&amp;p=0") + "\">已成熟</a>|");

        if (p == 1)
            builder.Append("已加入");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=friend&amp;p=1") + "\">已加入</a>");

        builder.Append("|邀请");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (p == 0)
            strWhere += " and State=4";

        string[] pageValUrl = { "act", "p", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.flows> listflows = new BCW.BLL.Game.flows().Getflowss2(pageIndex, pageSize, strWhere, out recordCount, meid);
        if (listflows.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.flows n in listflows)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?uid=" + n.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>");
                if (p == 0)
                    builder.Append("(成熟)");

                builder.Append(Out.Tab("</div>", ""));

                k++;

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">我的百花谷</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PutPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-播种";
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

        int flowid = int.Parse(Utils.GetRequest("flowid", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.Game.flows model = new BCW.BLL.Game.flows().Getflows(flowid);
        //if (model == null)
        //{
        //    Utils.Error("不存在的记录", "");
        //}

        if (model != null&&model.state != 0)
        {

            CheckPage(model.UsID);

            //--------------逻辑--------------
            int ExTime = 0;
            if (model.weeds > 0)
            {
                ExTime = Convert.ToInt32(model.weeds * 15);
            }
            int gNum = model.cnum;

            if (model.state == 4)
            {
                if (model.worm > 0)
                {
                    if (model.AddTime.AddMinutes(300 + ExTime + 60) < DateTime.Now)
                    {
                        gNum = gNum - model.worm;
                        if (gNum < 0)
                            gNum = 0;
                    }
                }
                if (model.water < 50)
                {
                    if (model.AddTime.AddMinutes(300 + ExTime + 60) < DateTime.Now)
                    {
                        long hour = DT.DateDiff(DateTime.Now, model.AddTime.AddMinutes(300 + ExTime + 60), 2);
                        int hour2 = Convert.ToInt32(hour);
                        gNum = gNum - hour2;
                        if (gNum < 0)
                            gNum = 0;
                        
                    }
                }
            }
            //--------------逻辑--------------

            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {
                if (model.UsID != meid)
                {
                    if (!new BCW.BLL.Friend().Exists(meid, model.UsID, 0))
                    {
                        Utils.Error("不存在的记录", "");
                    }
               
                }
                if (model.zid == 0)
                {
                    Utils.Error("此花盆没有可收获的花卉啊", "");
                }
                if (model.state != 4)
                {
                    Utils.Error("此花盆尚未成熟，不能收获", "");
                }

                //如果有增产肥料道具则计算得到多少花卉
                DataSet ds = new BCW.BLL.Game.flowmyprop().GetList("id,znum", "UsID=" + model.UsID + " and did=5 and znum>0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int zid = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    int znum = int.Parse(ds.Tables[0].Rows[0]["znum"].ToString());
                    gNum += znum * 15;
                    new BCW.BLL.Game.flowmyprop().Update(zid, -znum);
                }

                //收获花卉
                string mename = new BCW.BLL.User().GetUsName(model.UsID);
                BCW.Model.Game.flowmyzz m = new BCW.Model.Game.flowmyzz();
                m.UsID = model.UsID;
                m.UsName = mename;
                m.zid = model.zid;
                m.znum = gNum;
                m.ztitle = model.ztitle;
                m.Types = 1;
                if (!new BCW.BLL.Game.flowmyzz().Exists(model.UsID, m.Types, model.zid))
                {
                    new BCW.BLL.Game.flowmyzz().Add(m);
                }
                else
                {
                    new BCW.BLL.Game.flowmyzz().Update(m);
                }
                //增加技能积分
                new BCW.BLL.Game.flowuser().UpdateScore(model.UsID, 1, 5);
                //增加花产量
                new BCW.BLL.Game.flowuser().UpdateScore(model.UsID, 3, gNum);
            
                 string wText = "";
                 string wText2 = "";
                 if (model.UsID != meid)
                 {
                     wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]帮好友" + mename + "收获花卉";
                     wText2 = "成功收获" + model.ztitle + "" + gNum + "个,好友技能积分+5";
                 }
                 else
                 {
                     wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]对一个花卉进行了收获";
                     wText2 = "成功收获" + model.ztitle + "" + gNum + "个,技能积分+5";
                 
                 }
                new BCW.BLL.Action().Add(40, 0, meid, "", wText);

                Master.Title = "百花谷-收获";
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/game/flow/get.gif\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(wText2);
                builder.Append(Out.Tab("</div>", "<br />"));
                // 下条
                builder.Append(Out.Tab("<div>", ""));
                BCW.Model.Game.flows xx = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, model.UsID, true);
                if (xx.ID > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + xx.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一个</a> ");
                }
                // 上条
                BCW.Model.Game.flows ss = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, model.UsID, false);
                if (ss.ID > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + ss.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上一个</a>");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                //收获后更新成空花盆
                model.state = 0;
                model.zid = 0;
                model.ztitle = "空花盆";
                model.UsID = model.UsID;
                model.cnum = 0;
                model.checkuid = 0;
                model.water = 0;
                model.AddTime = DateTime.Now;
                new BCW.BLL.Game.flows().Update(model);

                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getPage("flow.aspx?uid=" + uid + "") + "\">返回上级</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else if (info == "ok1")
            {
                if (model.UsID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
                if (model.worm == 0)
                {
                    Utils.Error("此花盆已经没害虫了", "");
                }
                new BCW.BLL.Game.flows().Updateworm(model.ID, -model.worm);
                //增加技能积分
                new BCW.BLL.Game.flowuser().UpdateScore(meid, 1, 5);

                Utils.Success("除虫", "成功除虫" + model.worm + "只！技能积分+5", Utils.getUrl("flow.aspx?act=put&amp;flowid=" + flowid + ""), "1");


            }
            else if (info == "ok2")
            {
                if (model.UsID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
                if (model.weeds == 0)
                {
                    Utils.Error("此花盆已经没杂草了", "");
                }
                new BCW.BLL.Game.flows().Updateweeds(model.ID, -model.weeds);
                //增加技能积分
                new BCW.BLL.Game.flowuser().UpdateScore(meid, 1, 5);

                Utils.Success("除虫", "成功除草" + model.weeds + "棵！技能积分+5", Utils.getUrl("flow.aspx?act=put&amp;flowid=" + flowid + ""), "1");
            }
            else if (info == "ok3" || info == "ok4")
            {
                if (model.UsID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
                if (info == "ok3" && model.water >= 50)
                {
                    Utils.Error("此花盆水份正常，不需要浇水啦", "");
                }
                if (info == "ok4" && model.water <= 80)
                {
                    Utils.Error("此花盆水份正常，不需要日照啦", "");
                }

                new BCW.BLL.Game.flows().Updatewater(model.ID, 60);

                //增加技能积分
                new BCW.BLL.Game.flowuser().UpdateScore(meid, 1, 5);

                if (info == "ok3")
                {
                    Utils.Success("浇水", "成功浇水！技能积分+5", Utils.getUrl("flow.aspx?act=put&amp;flowid=" + flowid + ""), "1");
                }
                else
                {
                    Utils.Success("日照", "成功日照！技能积分+5", Utils.getUrl("flow.aspx?act=put&amp;flowid=" + flowid + ""), "1");
                
                }
            }
            else
            {

                Master.Title = "百花谷-花盆";
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/game/flow/" + model.ztitle + "/" + model.state + ".gif\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("品种:" + model.ztitle + "");
                string sState = "";
                if (model.state == 1)
                    sState = "种子";
                else if (model.state == 2)
                    sState = "发芽";
                else if (model.state == 3)
                    sState = "生长";
                else
                    sState = "成熟";

                builder.Append("<br />状态:" + sState + "");

                if (model.state == 4)
                {
                    builder.Append("<br />这个花盆可以进行收获了");
                }
                else
                {
                    builder.Append("<br />距离收获还有" + DT.DateDiff(DateTime.Now, model.AddTime.AddMinutes(300 + ExTime)) + "");
                }

                builder.Append("<br />产量:" + model.cnum + "个(可收" + gNum + "个)");
                if (model.UsID == meid && model.state == 4)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;info=ok&amp;flowid=" + flowid + "") + "\">收获</a>]");
                }
                else
                {
                    builder.Append("[收获]");
                }

                if (model.water >= 50 && model.water <= 80)
                {
                    builder.Append("<br />水分:正常[日照]");
                }
                else if (model.water < 50)
                {
                    builder.Append("<br />水分:" + model.water + "");
                    if (model.UsID == meid)
                    {
                        builder.Append("[<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;info=ok3&amp;flowid=" + flowid + "") + "\">浇水</a>]");
                    }
                    else
                    {
                        builder.Append("[浇水]");
                    }
                }
                else if (model.water > 80)
                {
                    builder.Append("<br />水分:" + model.water + "");
                    if (model.UsID == meid)
                    {
                        builder.Append("[<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;info=ok4&amp;flowid=" + flowid + "") + "\">日照</a>]");
                    }
                    else
                    {
                        builder.Append("[日照]");
                    }
                }
                builder.Append("<br />害虫:" + model.worm + "只");
                if (model.UsID == meid && model.worm > 0)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;info=ok1&amp;flowid=" + flowid + "") + "\">除虫</a>]");
                }
                else
                {
                    builder.Append("[除虫]");
                }
                builder.Append("<br />杂草:" + model.weeds + "棵");
                if (model.UsID == meid && model.weeds > 0)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;info=ok2&amp;flowid=" + flowid + "") + "\">除草</a>]");
                }
                else
                {
                    builder.Append("[除草]");
                }

                //好友帮收
                bool p_ismy = true;
                if (model.state == 4 && model.UsID != meid)
                {
                    if (new BCW.BLL.Friend().Exists(meid, model.UsID, 0))
                    {
                        builder.Append("<br />[<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;info=ok&amp;flowid=" + flowid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">帮收</a>]");
                    }
                    p_ismy = false;
                }

                builder.Append(Out.Tab("</div>", "<br />"));

                // 下条
                builder.Append(Out.Tab("<div>", ""));
                BCW.Model.Game.flows xx = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, model.UsID, true, p_ismy);
                if (xx.ID > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + xx.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一个</a> ");
                }
                // 上条
                BCW.Model.Game.flows ss = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, model.UsID, false, p_ismy);
                if (ss.ID > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + ss.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上一个</a>");
                }
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getPage("flow.aspx?uid=" + uid + "") + "\">返回上级</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
 
            if (p == 0)
            {
                if (model == null)
                {
                    Utils.Error("不存在的记录", "");
                }
                if (model.UsID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
                Master.Title = "百花谷-花盆";
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/game/flow/空花盆.gif\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("空闲的花盆<br />水分:正常");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;p=1&amp;flowid=" + flowid + "") + "\">播种</a>");
                builder.Append(Out.Tab("</div>", ""));
                // 下条
                builder.Append(Out.Tab("<div>", "<br />"));
                BCW.Model.Game.flows xx = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, meid, true);
                if (xx.ID > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + xx.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一个</a> ");
                }
                // 上条
                BCW.Model.Game.flows ss = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, meid, false);
                if (ss.ID > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + ss.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上一个</a>");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", Out.RHr()));
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=house") + "\">我的仓库</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/game/flow/zz.gif\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("请选择要添加的种子");
                int zero = 0;//空花盆数量
                if (flowid == 0)
                {
                    zero = new BCW.BLL.Game.flows().GetCount(meid, 0);
                    builder.Append("<br />你有空闲花盆"+zero+"个");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = string.Empty;
                strWhere = "UsID=" + meid + " and Types=0 and znum>0";

                string[] pageValUrl = { "act", "flowid", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                // 开始读取列表
                IList<BCW.Model.Game.flowmyzz> listflowmyzz = new BCW.BLL.Game.flowmyzz().Getflowmyzzs(pageIndex, pageSize, strWhere, out recordCount);
                if (listflowmyzz.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.Model.Game.flowmyzz n in listflowmyzz)
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
                        builder.Append("" + n.ztitle + ":" + n.znum + "个");
                        if (flowid > 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=putok&amp;flowid=" + flowid + "&amp;id=" + n.zid + "") + "\">[播种]</a>");
                        }
                        else
                        {
                            if (n.znum >= zero)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=putok&amp;id=" + n.zid + "&amp;p=1") + "\">[播种]</a>");
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
                    builder.Append(Out.Div("div", "没有相关记录.."));
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=shop") + "\">去购买种子</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            builder.Append(Out.Tab("", Out.Hr()));

        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PutListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();


        CheckPage(meid);


        Master.Title = "百花谷";
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (p == 0)
            builder.Append("可收获(" + new BCW.BLL.Game.flows().GetCount(meid, 4) + ")|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=putlist&amp;p=0") + "\">可收获(" + new BCW.BLL.Game.flows().GetCount(meid, 4) + ")</a>|");

        if (p == 1)
            builder.Append("生长中(" + new BCW.BLL.Game.flows().GetCount(meid, -2) + ")|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=putlist&amp;p=1") + "\">生长中(" + new BCW.BLL.Game.flows().GetCount(meid, -2) + ")</a>|");

        if (p == 2)
            builder.Append("空花盆(" + new BCW.BLL.Game.flows().GetCount(meid, 0) + ")");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=putlist&amp;p=2") + "\">空花盆(" + new BCW.BLL.Game.flows().GetCount(meid, 0) + ")</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (p == 0)
            strWhere = "UsID=" + meid + " and State=4";
        else if (p == 1)
            strWhere = "UsID=" + meid + " and State>0 and State<4";
        else if (p == 2)
            strWhere = "UsID=" + meid + " and State=0";

        string[] pageValUrl = { "act", "p", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.flows> listflows = new BCW.BLL.Game.flows().Getflowss(pageIndex, pageSize, strWhere, out recordCount);
        if (listflows.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.flows n in listflows)
            {
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + n.ID + "") + "\">" + n.ztitle + "</a>");
                if (k % 3 == 0 && k % 21 != 0)
                    builder.Append("<br />");
                else if (k % 3 != 0 && k % 21 != 0)
                    builder.Append("-");
                k++;

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">我的百花谷</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PutOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-1]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));//种子ID
        BCW.Model.Game.flowmyzz m = new BCW.BLL.Game.flowmyzz().Getflowmyzz(meid, 0, id);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.znum == 0)
        {
            Utils.Error("不存在的记录", "");
        }
        if (p == 0)
        {
            Master.Title = "百花谷-播种";
            int flowid = int.Parse(Utils.GetRequest("flowid", "all", 2, @"^[1-9]\d*$", "ID错误"));//花盆ID


            BCW.Model.Game.flows model = new BCW.BLL.Game.flows().Getflows(flowid);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            if (model.UsID != meid)
            {
                Utils.Error("不存在的记录", "");
            }
            if (model.state != 0)
            {
                Utils.Error("此花盆已播种<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + flowid + "") + "\">进入花盆</a>", "");
            }

            //播种
            model.state = 1;
            model.zid = id;
            model.ztitle = m.ztitle;
            model.UsID = meid;
            model.cnum = 10;
            model.water = 60;//默认水份
            model.AddTime = DateTime.Now;
            new BCW.BLL.Game.flows().Update(model);
            //扣除种子量
            new BCW.BLL.Game.flowmyzz().Updateznum(meid, id, 0, -1);
            //增加技能积分
            new BCW.BLL.Game.flowuser().UpdateScore(meid, 1, 5);

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + m.UsName + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]一个花盆上种植了" + model.ztitle + "";
            new BCW.BLL.Action().Add(40, 0, meid, "", wText);

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/Files/sys/game/flow/zz.gif\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("勤劳的你成功在这块闲置的沃土上进行了播种,技能积分+5");
            builder.Append(Out.Tab("</div>", ""));

            // 下条
            builder.Append(Out.Tab("<div>", "<br />"));
            BCW.Model.Game.flows xx = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, meid, true);
            if (xx.ID > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + xx.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一个</a><br />");
            }
            // 上条
            BCW.Model.Game.flows ss = new BCW.BLL.Game.flows().GetPreviousNextflows(flowid, meid, false);
            if (ss.ID > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + ss.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上一个</a><br />");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=put&amp;flowid=" + flowid + "") + "\">返回花盆</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //如果没有一键播种的道具
            DataSet dss = new BCW.BLL.Game.flowmyprop().GetList("id", "UsID=" + meid + " and did=7 and ExTime>'" + DateTime.Now + "'");
            if (dss == null || dss.Tables[0].Rows.Count == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("没有正在使用的一键播种道具..<br />");
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop") + "\">进入道具市场&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                Master.Title = "百花谷-一键播种";
                int zero = new BCW.BLL.Game.flows().GetCount(meid, 0);
                if (zero == 0)
                {
                    Utils.Error("现在没有空花盆呀", "");
                }
                if (m.znum < zero)
                {
                    Utils.Error("种子不够", "");
                }
                DataSet ds = new BCW.BLL.Game.flows().GetList("id", "UsID=" + meid + " and state=0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int flowid = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());//花盆ID
                        //播种
                        BCW.Model.Game.flows model = new BCW.Model.Game.flows();
                        model.ID = flowid;
                        model.state = 1;
                        model.zid = id;
                        model.ztitle = m.ztitle;
                        model.UsID = meid;
                        model.cnum = 10;
                        model.water = 60;//默认水份
                        model.AddTime = DateTime.Now;
                        new BCW.BLL.Game.flows().Update(model);
                    }
                    //扣除种子量
                    new BCW.BLL.Game.flowmyzz().Updateznum(meid, id, 0, -zero);
                    //增加技能积分
                    new BCW.BLL.Game.flowuser().UpdateScore(meid, 1, Convert.ToInt32(5 * zero));
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + m.UsName + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]一键种植了" + zero + "棵" + m.ztitle + "";
                    new BCW.BLL.Action().Add(40, 0, meid, "", wText);

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<img src=\"/Files/sys/game/flow/zz.gif\" alt=\"load\"/>");
                    builder.Append(Out.Tab("</div>", "<br />"));

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("勤劳的你成功在这块闲置的沃土上进行了一键播种" + zero + "棵" + m.ztitle + ",技能积分+" + Convert.ToInt32(5 * zero) + "");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">返回百花谷</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }




    private void HousePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-仓库";
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (p == 1)
            builder.Append("我的花卉|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=house&amp;p=1") + "\">花卉</a>|");

        if (p == 0)
            builder.Append("我的种子");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=house&amp;p=0") + "\">种子</a>");

        //if (p == 2)
        //    builder.Append("|我的花篮");
        //else
        //    builder.Append("|<a href=\"" + Utils.getUrl("flow.aspx?act=house&amp;p=2") + "\">花篮</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;

        strWhere = "UsID=" + meid + " and Types=" + p + " and znum>0";

        string[] pageValUrl = { "act", "p", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.flowmyzz> listflowmyzz = new BCW.BLL.Game.flowmyzz().Getflowmyzzs(pageIndex, pageSize, strWhere, out recordCount);
        if (listflowmyzz.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.flowmyzz n in listflowmyzz)
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

                if (p == 1)
                {
                    builder.Append("" + n.ztitle + ":" + n.znum + "朵[赠送]");
                    // builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;info=view&amp;id=" + n.ID + "") + "\">[赠送]</a>");

                }
              else
                    builder.Append("" + n.ztitle + ":" + n.znum + "个");

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
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ShopPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-商店";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("市场种子及单价:");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        int Price = Convert.ToInt32(ub.GetSub("FlowzPrice", xmlPath));

        // 开始读取列表
        IList<BCW.Model.Game.flowzz> listflowzz = new BCW.BLL.Game.flowzz().Getflowzzs(pageIndex, pageSize, strWhere, out recordCount);
        if (listflowzz.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.flowzz n in listflowzz)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                builder.Append("" + n.Title + "(" + n.Leven + "级)<br />单价:" + Price + "" + ub.Get("SiteBz") + "/个");
                builder.Append("[<a href=\"" + Utils.getUrl("flow.aspx?act=pay&amp;id=" + n.ID + "") + "\">购买</a>]");
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
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //if (!new BCW.BLL.Marry().ExistsMarry(meid))
        //{
        //    Utils.Error("仅限结婚的夫妻种花", "");
        //}
        int Price = Convert.ToInt32(ub.GetSub("FlowzPrice", xmlPath));
        Master.Title = "购买种子";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "种子ID错误"));
        BCW.Model.Game.flowzz model = new BCW.BLL.Game.flowzz().Getflowzz(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        long gold = new BCW.BLL.User().GetGold(meid);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            model.Price = Convert.ToInt32(ub.GetSub("FlowzPrice", xmlPath));

            int num = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "购买数量填写错误"));
            if (num > 99)
                Utils.Error("每次最多购买99个", "");

            //技能等级
            int leven = 0;
            //分析积分XML
            int score = new BCW.BLL.Game.flowuser().GetScore(meid);
            for (int i = 1; i <= 40; i++)
            {
                string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
                string[] temp = str.Split("|".ToCharArray());
                int Iscore = Convert.ToInt32(temp[0]);
                if (score >= Iscore)
                {
                    leven = i;
                }

            }

            if (model.Leven > leven)
            {
                Utils.Error("" + model.Title + "需要技能等级达到" + model.Leven + "级才能购买", "");
            }
            if (Convert.ToInt64(Price * num) > gold)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            //写入记录
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.Game.flowmyzz m = new BCW.Model.Game.flowmyzz();
            m.UsID = meid;
            m.UsName = mename;
            m.zid = id;
            m.znum = num;
            m.ztitle = model.Title;
            m.Types = 0;
            if (!new BCW.BLL.Game.flowmyzz().Exists(meid, m.Types, id))
            {
                new BCW.BLL.Game.flowmyzz().Add(m);
            }
            else
            {
                new BCW.BLL.Game.flowmyzz().Update(m);
            }
            //扣币
            new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(Price * num), "购买种子");

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]商店购买了" + num + "个种子";
            new BCW.BLL.Action().Add(40, 0, meid, "", wText);

            Utils.Success("购买种子", "恭喜，成功购买" + num + "个" + model.Title + "种子<br /><a href=\"" + Utils.getUrl("flow.aspx?act=shop") + "\">继续购买</a>", Utils.getUrl("flow.aspx"), "5");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("购买种子");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + model.Title + ":" + Price + "" + ub.Get("SiteBz") + "/个<br />");
            builder.Append("预计产量:" + model.cNum + "<br />");
            builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / model.Price) + "个");
            builder.Append(Out.Tab("</div>", ""));

            strText = "输入购买数量:,,,,";
            strName = "num,id,act,info";
            strType = "snum,hidden,hidden,hidden";
            strValu = "1'" + id + "'pay'ok";
            strEmpt = "false,false,false,false";
            strIdea = "(1-99)'''|/";
            strOthe = "确定购买,flow.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=shop") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void PropPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //if (!new BCW.BLL.Marry().ExistsMarry(meid))
        //{
        //    Utils.Error("仅限结婚的夫妻种花", "");
        //}

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-7]$", "0"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            if (id == 0)
                Utils.Error("种子ID错误", "");

            long gold = new BCW.BLL.User().GetGold(meid);
            string pay = Utils.GetRequest("pay", "all", 1, "", "");
            long price = 0;
            if (id <= 3)
            {
                price = Convert.ToInt64(ub.GetSub("FlowdPrice", xmlPath));
            }
            else
            {
                price = Convert.ToInt64(ub.GetSub("FlowdPrice2", xmlPath));
            }
            if (pay == "ok")
            {
                int num = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "购买数量填写错误"));

                if (Convert.ToInt64(price * num) > gold)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                //写入记录
                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.Game.flowmyprop m = new BCW.Model.Game.flowmyprop();
                m.UsID = meid;
                m.did = id;
                m.dnum = num;
                m.Title = GetPropTitle(id);
                m.ExTime = DateTime.Parse("1990-1-1");
                if (!new BCW.BLL.Game.flowmyprop().Exists(meid, id))
                {
                    new BCW.BLL.Game.flowmyprop().Add(m);
                }
                else
                {
                    new BCW.BLL.Game.flowmyprop().Update(m);
                }
                //扣币
                new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(price * num), "购买道具");

                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/flow.aspx]百花谷[/url]购买" + num + "张道具";
                new BCW.BLL.Action().Add(40, 0, meid, "", wText);

                Utils.Success("购买种子", "恭喜，成功购买" + num + "张" + GetPropTitle(id) + "-道具<br /><a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;p=1") + "\">继续购买</a>", Utils.getUrl("flow.aspx?act=prop"), "3");

            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("购买道具");
                builder.Append(Out.Tab("</div>", "<br />"));
                if (id == 1)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:灌溉系统<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:针对花谷所有花盆的水分起到预防干旱的作用,24小时内保持现有水分");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else if (id == 2)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:杂草克星<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:24小时内防止杂草破坏花卉");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else if (id == 3)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:防虫电网<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:24小时内防止害虫破坏花卉");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else if (id == 4)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:催化肥料<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:使用后花盆里的鲜花可提前1小时收获");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else if (id == 5)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:增产肥料<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:使用后收获时每个道具可多收15朵鲜花");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else if (id == 6)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:一键收获<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:一键收获所有花盆里成熟的鲜花,24小时有效");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else if (id == 7)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("名称:一键播种<br />价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("你自带" + ub.Get("SiteBz") + "" + gold + ",可以买" + (gold / price) + "个<br />");
                    builder.Append("作用:可以针对百花谷所有花盆一次全部播种,24小时有效");
                    builder.Append(Out.Tab("</div>", ""));
                }
                strText = "输入购买数量:,,,,,";
                strName = "num,id,act,info,pay";
                strType = "snum,hidden,hidden,hidden,hidden";
                strValu = "1'" + id + "'prop'ok'ok";
                strEmpt = "false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定购买,flow.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;p=1") + "\">返回上级</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {

            Master.Title = "百花谷-道具";
            int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-2]$", "0"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (p == 0)
                builder.Append("我的道具|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;p=0") + "\">我的道具</a>|");

            if (p == 1)
                builder.Append("道具市场");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;p=1") + "\">道具市场</a>");

            builder.Append(Out.Tab("</div>", ""));
            if (info == "view")
            {

                int pid = int.Parse(Utils.GetRequest("pid", "all", 2, @"^[1-9]\d*$", "ID错误"));
                BCW.Model.Game.flowmyprop model = new BCW.BLL.Game.flowmyprop().Getflowmyprop(pid);
                if (model == null)
                {
                    Utils.Error("不存在的记录", "");
                }
                if (model.UsID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("" + GetPropTitle(model.did) + ":剩" + new BCW.BLL.Game.flowmyprop().Getdnum(pid) + "张<br />");
                builder.Append("作用:" + GetPropOn(model.did) + "");
                if (model.did == 4)
                {
                    builder.Append("(立即生效)");
                }
                builder.Append(Out.Tab("</div>", ""));

                if (model.did == 5)
                {

                    strText = "输入使用数量:/,,";
                    strName = "znum,pid,act";
                    strType = "num,hidden,hidden";
                    strValu = "1'" + pid + "'propac";
                    strEmpt = "false,false,false";
                    strIdea = "/";
                    strOthe = "立即使用,flow.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
                else
                {
                    builder.Append(Out.Tab("", "<br />"));
                    strName = "pid,act";
                    strValu = "" + pid + "'propac";
                    strOthe = "立即使用,flow.aspx,post,0,red";

                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;p=1") + "\">继续购买</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop") + "\">返回上级</a>");
                builder.Append(Out.Tab("</div>", ""));

            }
            else
            {
                if (p == 0 || p == 2)
                {

                    if (p == 2)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("正在使用的道具");
                        builder.Append(Out.Tab("</div>", ""));
                    
                    }
                    int pageIndex;
                    int recordCount;
                    int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                    string strWhere = string.Empty;
                    if (p == 0)
                        strWhere = "UsID=" + meid + " and dnum>0";
                    else
                        strWhere = "UsID=" + meid + " and (ExTime>'" + DateTime.Now + "' or znum>0)";

                    string[] pageValUrl = { "act", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;

                    // 开始读取列表
                    IList<BCW.Model.Game.flowmyprop> listflowmyprop = new BCW.BLL.Game.flowmyprop().Getflowmyprops(pageIndex, pageSize, strWhere, out recordCount);
                    if (listflowmyprop.Count > 0)
                    {
                        int k = 1;
                        foreach (BCW.Model.Game.flowmyprop n in listflowmyprop)
                        {
                            if (k % 2 == 0)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                            else
                            {
                                if (k == 1)
                                    builder.Append(Out.Tab("<div>", "<br />"));
                                else
                                    builder.Append(Out.Tab("<div>", "<br />"));
                            }
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                            if (p == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;info=view&amp;pid=" + n.ID + "") + "\">" + GetPropTitle(n.did) + "</a>(" + n.dnum + ")");
                            else
                            {
                                builder.Append("" + GetPropTitle(n.did) + "");
                                if (n.znum > 0)
                                    builder.Append("(正在使用" + n.znum + "个)");
                                else
                                    builder.Append("(还有" + DT.DateDiff2(DateTime.Now, n.ExTime) + "过期)");
                            }
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }

                        // 分页
                        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    }
                    else
                    {
                        builder.Append(Out.Tab("", "<br />"));
                        builder.Append(Out.Div("div", "没有相关记录.."));
                    }

                }
                else
                {

                    for (int i = 1; i <= 7; i++)
                    {
                        long price = 0;
                        if (i <= 3)
                        {
                            price = Convert.ToInt64(ub.GetSub("FlowdPrice", xmlPath));
                        }
                        else
                        {
                            price = Convert.ToInt64(ub.GetSub("FlowdPrice2", xmlPath));
                        }

                        builder.Append(Out.Tab("<div>", Out.Hr()));
                        builder.Append("" + i + "." + GetPropTitle(i) + "");
                        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;info=ok&amp;id=" + i + "") + "\">[购买]</a><br />");
                        builder.Append("价格:" + price + "" + ub.Get("SiteBz") + "<br />");
                        builder.Append("作用:" + GetPropOn(i) + "");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=prop&amp;p=2") + "\">正使用的道具</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void PropAcPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = int.Parse(Utils.GetRequest("pid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int znum = int.Parse(Utils.GetRequest("znum", "all", 1, @"^[0-9]\d*$", "0"));
        BCW.Model.Game.flowmyprop model = new BCW.BLL.Game.flowmyprop().Getflowmyprop(pid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.did != 5)
        {
            if (model.ExTime > DateTime.Now)
            {
                Utils.Error("此道具正在使用中", "");

            }
            if (model.dnum <= 0)
            {
                Utils.Error("不存在的记录", "");

            }
        }
        else
        {
            if (znum == 0)
            {
                Utils.Error("道具数量填写错误", "");

            }
            if (model.dnum < znum)
            {
                Utils.Error("道具数量不足", "");

            }
        }
        DateTime extime = DateTime.Now;
        int num = 1;
        if (model.did == 1)//灌溉系统
        {
            extime = DateTime.Now.AddHours(24);
        }
        else if (model.did == 2)//杂草克星
        {
            extime = DateTime.Now.AddHours(24);
        }
        else if (model.did == 3)//防虫电网
        {
            extime = DateTime.Now.AddHours(24);
        }
        else if (model.did == 4)//催化肥料
        {
            extime = DateTime.Now;
            //立即执行提前60分钟收获
            DataSet ds = new BCW.BLL.Game.flows().GetList("ID,AddTime", "UsID=" + meid + " and  State>0 and State<4");
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Utils.Error("没有需要使用催化肥料的花盆.", "");
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int fid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                DateTime fAddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());

                new BCW.BLL.Game.flows().UpdateAddTime(fid, fAddTime.AddMinutes(-60));
            }
        }
        else if (model.did == 5)//增产肥料
        {
            extime = DateTime.Now;
            num = znum;
        }
        else if (model.did == 6)//一键收获
        {
            extime = DateTime.Now.AddHours(24);
        }
        else if (model.did == 7)//一键播种
        {
            extime = DateTime.Now.AddHours(24);
        }
        if (model.did == 5)
            new BCW.BLL.Game.flowmyprop().Update(pid, -num, extime, num);
        else
            new BCW.BLL.Game.flowmyprop().Update(pid, -num, extime, 0);


        Utils.Success("使用道具", "恭喜，成功使用" + num + "张" + GetPropTitle(model.did) + "-道具", Utils.getUrl("flow.aspx?act=prop"), "3");

    }

    private string GetPropTitle(int id)
    {
        string title = "";
        if (id == 1)
            title = "灌溉系统";
        else if (id == 2)
            title = "杂草克星";
        else if (id == 3)
            title = "防虫电网";
        else if (id == 4)
            title = "催化肥料";
        else if (id == 5)
            title = "增产肥料";
        else if (id == 6)
            title = "一键收获";
        else if (id == 7)
            title = "一键播种";

        return title;
    }

    private string GetPropOn(int id)
    {
        string title = "";
        if (id == 1)
            title = "针对花谷所有花盆的水分起到预防干旱的作用,24小时内保持现有水分";
        else if (id == 2)
            title = "24小时内防止杂草破坏花卉";
        else if (id == 3)
            title = "24小时内防止害虫破坏花卉";
        else if (id == 4)
            title = "使用后花盆里的鲜花可提前1小时收获";
        else if (id == 5)
            title = "使用后收获时每个道具可多收15朵鲜花";
        else if (id == 6)
            title = "一键收获所有花盆里成熟的鲜花,24小时有效";
        else if (id == 7)
            title = "可以针对百花谷所有花盆一次全部播种,24小时有效";

        return title;
    }




    private void HelpPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-帮助";
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-4]$", "0"));


        if (p == 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("百花谷-帮助");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("您可以拥有自己的花谷,在花盆里栽种获得花卉,可以帮忙照料好友的花卉,也可以搞破坏<br />");
            builder.Append("1.花盆的扩建<br />");
            builder.Append("初始9个花盆,<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">技能等级</a>每升一级可多扩建一个花盆(20级后每升一级可新增2个花盆,系统在你升级时自动增加)<br />");
            builder.Append("2.鲜花的产量<br />");
            builder.Append("每个种子可产10朵鲜花,<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">风采等级</a>每升一级，产量可增加一个<br />");
            builder.Append("3.搞破坏<br />");
            builder.Append("去好友花盆里放1颗草一个小时后会使对方的花卉成长时间延长15分钟,放1只虫花卉成熟一个小时后会使产量减少1个.搞破坏每天只能在80次以内<br />");
            builder.Append("当花盆水份在50%以下的时候,成熟的花卉每60分钟会使产量减一<br />");
            builder.Append("可以使用道具来防备各种破坏<br />");
            builder.Append("4.成长将采用时间成长机制,成长分4个过程<br />");
            builder.Append("第一阶段:种子<br />");
            builder.Append("至下一阶段需要时间:60分钟<br />");
            builder.Append("第二阶段:发芽<br />");
            builder.Append("至下一阶段需要时间:120分钟<br />");
            builder.Append("第三阶段:生长<br />");
            builder.Append("至下一阶段需要时间:120分钟<br />");
            builder.Append("第四阶段:成熟,可以收获花卉了<br />");
            builder.Append("5.工作属性获得及消耗<br />");
            builder.Append("以下操作以一块地为单位:<br />");
            builder.Append("播种 技能积分+5点<br />");
            builder.Append("浇水 技能积分+5点<br />");
            builder.Append("除草 技能积分+5点<br />");
            builder.Append("驱虫 技能积分+5点<br />");
            builder.Append("日照 技能积分+5点<br />");
            builder.Append("收获 技能积分+5点<br />");
            builder.Append("6.其他<br />");
            builder.Append("天气每两个小时改变一次,花盆正常水分:50-80;大于80需要日照,小于50需要浇水");
            builder.Append(Out.Tab("</div>", ""));

        }
        else if (p == 1)
        {
            int score = new BCW.BLL.Game.flowuser().GetScore(meid);

            //分析积分XML
            int leven = 0;
            string title = "花农";
            for (int i = 1; i <= 40; i++)
            {
                string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
                string[] temp = str.Split("|".ToCharArray());
                int score2 = Convert.ToInt32(temp[0]);
                if (score >= score2)
                {
                    leven = i;
                    title = temp[1];
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("技能等级:" + leven + "级<br />");
            builder.Append("技能称号:" + title + "<br />");
            builder.Append("技能积分:" + score + "分");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=3&amp;backurl=" + Utils.getPage(0) + "") + "\">技能等级说明</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;backurl=" + Utils.getPage(0) + "") + "\">排行</a><br />");
            builder.Append("技能等级每升一级可新增一个花盆(20级后新增2个)<br />");
            builder.Append("每天技能积分上限:100分<br />");
            builder.Append("技能等级 积分 称号");
            builder.Append(Out.Tab("</div>", ""));

            int pageIndex;
            int recordCount;
            int pageSize = 7;
            string[] pageValUrl = { "act", "p", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = 40;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 1; i <= recordCount; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
                    string[] temp = str.Split("|".ToCharArray());
                    int iscore = Convert.ToInt32(temp[0]);
                    string stitle = temp[1];

                    builder.Append("" + i + "级 " + iscore + " " + stitle + "");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else if (p == 2)
        {
            int score = new BCW.BLL.Game.flowuser().GetScore2(meid);

            //分析积分XML
            int leven = 0;
            string title = "绿叶";
            for (int i = 1; i <= 40; i++)
            {
                string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
                string[] temp = str.Split("|".ToCharArray());
                int score2 = Convert.ToInt32(temp[0]);
                if (score >= score2)
                {
                    leven = i;
                    title = temp[2];
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("风采等级:" + leven + "级<br />");
            builder.Append("风采称号:" + title + "<br />");
            builder.Append("风采积分:" + score + "分");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=help&amp;p=4&amp;backurl=" + Utils.getPage(0) + "") + "\">风采等级说明</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;backurl=" + Utils.getPage(0) + "") + "\">排行</a><br />");
            builder.Append("风采等级每升一级，产量可增加一个<br />");
            builder.Append("每天风采积分无上限<br />");
            builder.Append("风采等级 积分 称号");
            builder.Append(Out.Tab("</div>", ""));

            int pageIndex;
            int recordCount;
            int pageSize = 7;
            string[] pageValUrl = { "act", "p", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = 40;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 1; i <= recordCount; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
                    string[] temp = str.Split("|".ToCharArray());
                    int iscore = Convert.ToInt32(temp[0]);
                    string stitle = temp[2];

                    builder.Append("" + i + "级 " + iscore + " " + stitle + "");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else if (p == 3)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("可通过以下操作增加技能点：<br />");
            builder.Append("播种 技能积分+5点<br />");
            builder.Append("浇水 技能积分+5点<br />");
            builder.Append("除草 技能积分+5点<br />");
            builder.Append("驱虫 技能积分+5点<br />");
            builder.Append("日照 技能积分+5点<br />");
            builder.Append("收获 技能积分+5点");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (p == 4)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("风采等级获取规则：<br />");
            builder.Append("一见你就笑：送出者风采积分+5，接收者，风采积分+3<br />");
            builder.Append("节日快乐：送出者风采积分+5，接收者，风采积分+3<br />");
            builder.Append("心只有你：送出者风采积分+10，接收者，风采积分+6<br />");
            builder.Append("天长地久：送出者风采积分+15，接收者，风采积分+9<br />");
            builder.Append("荣辱与共：送出者风采积分+20，接收者，风采积分+12<br />");
            builder.Append("早安宝贝：送出者风采积分+25，接收者，风采积分+15<br />");
            builder.Append("长命百岁：送出者风采积分+30，接收者，风采积分+18<br />");
            builder.Append("情比金坚：送出者风采积分+35，接收者，风采积分+21<br />");
            builder.Append("生日快乐：送出者风采积分+40，接收者，风采积分+24<br />");
            builder.Append("新春吉祥：送出者风采积分+45，接收者，风采积分+27<br />");
            builder.Append("一生有你：送出者风采积分+50，接收者，风采积分+30<br />");
            builder.Append("在天愿作比翼鸟：送出者风采积分+100，接收者，风采积分+60<br />");
            builder.Append("在地愿为连理枝：送出者风采积分+100，接收者，风采积分+60");
            builder.Append(Out.Tab("</div>", ""));

        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("flow.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage()
    {

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "百花谷-风云榜";
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("范围:");
        if (p == 0)
            builder.Append("全国|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;p=0&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>|");

        if (p == 1)
            builder.Append("自己");
        else
            builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;p=1&amp;backurl=" + Utils.getPage(0) + "") + "\">自己</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int Score3 = 0;
        int Score = 0;
        int Score2 = 0;
        int Score4 = 0;
        int Score5 = 0;
        BCW.Model.Game.flowuser model = new BCW.Model.Game.flowuser();
        model = new BCW.BLL.Game.flowuser().Getflowuser(meid);
        if (model != null)
        {
            Score3 = model.Score3;
            Score = model.Score;
            Score2 = model.Score2;
            Score4 = model.Score4;
            Score5 = model.Score5;
        }

        if (p == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("我的花谷产量:" + Score3 + "朵<br />");
            builder.Append("我的技能积分:" + Score + "<br />");
            builder.Append("我的风采积分:" + Score2 + "<br />");
            builder.Append("我送花篮数量:" + Score4 + "个<br />");
            builder.Append("我收花篮数量:" + Score5 + "个");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("产量|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">产量</a>|");

            if (ptype == 2)
                builder.Append("技能|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">技能</a>|");

            if (ptype == 3)
                builder.Append("风采|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">风采</a>|");

            if (ptype == 4)
                builder.Append("慷慨|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">慷慨</a>|");

            if (ptype == 5)
                builder.Append("人气");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?act=top&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">人气</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("你的花卉产量" + Score3 + "朵,排名第" + new BCW.BLL.Game.flowuser().GetTop(meid, "Score3") + "");
            else if (ptype == 2)
                builder.Append("你的技能积分" + Score + "分,排名第" + new BCW.BLL.Game.flowuser().GetTop(meid, "Score") + "");
            else if (ptype == 3)
                builder.Append("你的风采积分" + Score2 + "分,排名第" + new BCW.BLL.Game.flowuser().GetTop(meid, "Score2") + "");
            else if (ptype == 4)
                builder.Append("你的送出花篮" + Score4 + "个,排名第" + new BCW.BLL.Game.flowuser().GetTop(meid, "Score4") + "");
            else if (ptype == 5)
                builder.Append("你的收到花篮" + Score5 + "个,排名第" + new BCW.BLL.Game.flowuser().GetTop(meid, "Score5") + "");

            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            if (ptype == 1)
                strWhere = "Score3";
            else if (ptype == 2)
                strWhere = "Score";
            else if (ptype == 3)
                strWhere = "Score2";
            else if (ptype == 4)
                strWhere = "Score4";
            else if (ptype == 5)
                strWhere = "Score5";

            string[] pageValUrl = { "act", "p", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // 开始读取列表
            IList<BCW.Model.Game.flowuser> listflowuser = new BCW.BLL.Game.flowuser().GetflowusersTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listflowuser.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Game.flowuser n in listflowuser)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    builder.Append("<a href=\"" + Utils.getUrl("flow.aspx?uid=" + n.UsID + "") + "\">" + n.UsName + "</a>");
                    if (ptype == 1)
                        builder.Append("产量" + n.Score + "朵");
                    else if (ptype == 2)
                        builder.Append("积分" + n.Score + "分");
                    else if (ptype == 3)
                        builder.Append("积分" + n.Score + "分");
                    else if (ptype == 4)
                        builder.Append("送出花篮" + n.Score + "个");
                    else if (ptype == 5)
                        builder.Append("接收花篮" + n.Score + "个");

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
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("flow.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/marry.aspx") + "\">结婚</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flow.aspx") + "\">百花谷</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 更新花卉成长过程
    /// </summary>
    private void CheckPage(int uid)
    {

        DataSet ds = new BCW.BLL.Game.flows().GetList("id,AddTime,weeds", "UsID=" + uid + " and State>0 and State<4");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                int weeds = int.Parse(ds.Tables[0].Rows[i]["weeds"].ToString());
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());

                //种子生长过程
                int state = 0;
                int ExTime = 0;
                if (weeds > 0)
                    ExTime = Convert.ToInt32(weeds * 15);

                if (AddTime.AddMinutes(300 + ExTime) < DateTime.Now)
                {
                    state = 4;
                }
                else if (AddTime.AddMinutes(180) < DateTime.Now)
                {
                    state = 3;
                }
                else if (AddTime.AddMinutes(60) < DateTime.Now)
                {
                    state = 2;
                }
                if (state > 0)
                {
                    new BCW.BLL.Game.flows().Update(id, state);
                }
 
            }
        }
        //自动升级花盆数量
        //技能等级
        int leven = 0;
        //分析积分XML
        int score = new BCW.BLL.Game.flowuser().GetScore(uid);
        for (int i = 1; i <= 40; i++)
        {
            string str = ub.GetSub("FlowLeven" + i + "", xmlPath);
            string[] temp = str.Split("|".ToCharArray());
            int Iscore = Convert.ToInt32(temp[0]);
            if (score >= Iscore)
            {
                leven = i;
            }
        }
        int iFlows = new BCW.BLL.Game.flowuser().GetiFlows(uid);
        int num = 9;
        if (leven > 0 && leven <= 20)
            num = leven + num;
        else if (leven > 20)
            num = (leven - 20) * 2 + 29;


        int j = num;
        if (iFlows < j)
        {
            for (int i = 0; i < (j - iFlows); i++)
            {
                BCW.Model.Game.flows m = new BCW.Model.Game.flows();
                m.UsID = uid;
                m.zid = 0;
                m.ztitle = "空花盆";
                m.water = 0;
                m.weeds = 0;
                m.worm = 0;
                m.state = 0;
                m.cnum = 0;
                m.checkuid = 0;
                m.cnum2 = 0;
                new BCW.BLL.Game.flows().Add(m);

            }
            new BCW.BLL.Game.flowuser().UpdateiFlows(uid, num);

        }

    }

    /// <summary>
    /// 更新天气
    /// </summary>
    /// <returns></returns>
    private void Weather()
    {

        DateTime WeatherTime = DateTime.Parse("1990-1-1");
        try { WeatherTime = DateTime.Parse(ub.GetSub("FlowWeatherTime", xmlPath)); }
        catch { }
        if (DateTime.Now > WeatherTime.AddHours(2))
        {
    

            string weather = "";
            string water = "";

            weather = "晴朗,多云,小雨,雷阵雨,小雪,阴转晴,晴朗,多云,小雨,雷阵雨,小雪,阴转晴,晴朗,多云,小雨,雷阵雨,小雪,阴转晴";
            water = "-20,-10,20,10,20,0,-20,-10,10,10,20,-10,-20,-10,10,20,20,0";
        
            Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            int k = r.Next(1,100);
            if (k < 20)
            {
                weather = "晴朗,多云,阴转晴";
                water = "0,0,0";
            }
            else if (k < 50)
            {
                weather = "晴朗,多云,阴转晴";
                water = "-20,-10,-20";
            }

            string[] weatherTemp = Regex.Split(weather, ",");
            string[] waterTemp = Regex.Split(water, ",");

            Random rd = new Random();
            int ra = rd.Next(weatherTemp.Length);
            weather = weatherTemp[ra];
            water = waterTemp[ra];
            int iwater = Convert.ToInt32(water);

            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
             
            xml.dss["FlowWeather"] = weather;
            xml.dss["FlowWater"] = iwater;
            xml.dss["FlowWeatherTime"] = DateTime.Now.ToString();
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            Application.Remove(xmlPath);//清缓存
            int getwater = Convert.ToInt32(ub.GetSub("FlowWater", xmlPath));
            DataSet ds = new BCW.BLL.Game.flows().GetList("id,AddTime,weeds,water,usid", "State>0 and State<4");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                    int usid = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
                    int mywater = int.Parse(ds.Tables[0].Rows[i]["water"].ToString());
            
                    //如果没有水分道具则更新水分
                    DataSet dss = new BCW.BLL.Game.flowmyprop().GetList("id", "UsID=" + usid + " and did=1 and ExTime>'" + DateTime.Now + "'");
                    if (dss == null || dss.Tables[0].Rows.Count == 0)
                    {

                        mywater = mywater + getwater;
                        if (mywater > 100)
                        {
                            mywater = 100;
                        }
                        if (mywater < 0)
                        {
                            mywater = 0;
                        }
                        new BCW.BLL.Game.flows().Updatewater(id, mywater);
                    }
                }
            }
        }


        
    }
}
