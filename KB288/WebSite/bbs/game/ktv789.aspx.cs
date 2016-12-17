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

public partial class bbs_game_ktv789 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/ktv789.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    /// <summary>
    /// 取税率
    /// </summary>
    /// <returns></returns>
    private double GetTax()
    {
        double Tax = 0;
        try
        {
            Tax = Convert.ToDouble(ub.GetSub("ktv789Tax", xmlPath));
            Tax = 5;
        }
        catch { }
        return Convert.ToDouble(Tax * 0.01);
    }
    protected void Page_Load(object sender, EventArgs e)
    {      
        //维护提示
        if (ub.GetSub("Ktv789Status", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        Master.Title = "Ktv789";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "sit":
                SitPage();
                break;
            case "exit":
                ExitPage();
                break;
            case "fresh":
                FreshPage();
                break;
            case "kick":
                KickPage();
                break;
            case "shot":
                ShotPage();
                break;
            case "dt":
                DtPage();
                break;
            case "online":
                OnlinePage();
                break;
            case "view":
                ViewPage();
                break;
            case "top":
                TopPage();
                break;
            case "help":
                HelpPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("Ktv789Name", xmlPath);
        string Logo = ub.GetSub("Ktv789Logo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        //定期清离桌用户
        DataSet ds = new BCW.BLL.Game.ktv789().GetList("ID", "(OneTime<'" + DateTime.Now.AddHours(-2) + "' OR TwoTime<'" + DateTime.Now.AddHours(-2) + "' OR ThrTime<'" + DateTime.Now.AddHours(-2) + "')");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                new BCW.BLL.Game.ktv789().UpdateClear(id);
            }
        }

        builder.Append(Out.Tab("<div>", ""));
        string Notes = ub.GetSub("Ktv789Notes", xmlPath);
        if (Notes != "")
            builder.Append(Notes + "<br />");

        if (ptype == 1)
            builder.Append("初级对战|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=1") + "\">初级</a>|");

        if (ptype == 2)
            builder.Append("中级对战|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=2") + "\">中级</a>|");

        if (ptype == 3)
            builder.Append("终级对战");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=3") + "\">终级</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=" + ptype + "";

        // 开始读取列表
        IList<BCW.Model.Game.ktv789> listktv789 = new BCW.BLL.Game.ktv789().Getktv789s(pageIndex, pageSize, strWhere, out recordCount);
        if (listktv789.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.ktv789 n in listktv789)
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
                string rText = string.Empty;
                if (n.OneUsId != 0)
                {
                    rText += n.OneUsName;
                }
                if (n.TwoUsId != 0)
                {
                    rText += "VS" + n.TwoUsName;
                }
                if (n.ThrUsId != 0)
                {
                    rText += "VS" + n.ThrUsName;
                }

                if (rText == "" || rText == null)
                {
                    rText += "房间空闲中...";
                }
                //更新在线人数
                if (!string.IsNullOrEmpty(n.Lines))
                {
                    string[] arrLines = n.Lines.ToString().Split("#".ToCharArray());
                    for (int i = 0; i < arrLines.Length; i++)
                    {
                        try
                        {
                            DateTime sTime = Convert.ToDateTime(arrLines[i].Split("!".ToCharArray())[1]);
                            if (sTime.AddSeconds(120) < DateTime.Now)
                            {
                                string Lines = n.Lines.ToString().Replace("#" + arrLines[i], "");
                                Lines = Lines.Replace(arrLines[i] + "#", "");
                                Lines = Lines.Replace(arrLines[i], "");
                                new BCW.BLL.Game.ktv789().UpdateLines(n.ID, Lines);
                            }
                        }
                        catch { }
                    }
                    //重新计算在线人数
                    string OverLines = new BCW.BLL.Game.ktv789().GetLines(n.ID);
                    if (OverLines != "")
                    {
                        string[] arrOverLines = OverLines.Split("#".ToCharArray());
                        new BCW.BLL.Game.ktv789().UpdateOnline(n.ID, arrOverLines.Length);
                    }
                    else
                    {
                        new BCW.BLL.Game.ktv789().UpdateOnline(n.ID, 0);
                    }
                }
                string GoldTypeName = string.Empty;
                if (n.GoldType == 1)
                {
                    GoldTypeName = "(奖)";
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id={0}&amp;tm=30") + "\">" + GoldTypeName + "{1}({2}{3})</a>({4})", n.ID, n.StName, n.PayCent, ub.Get("SiteBz"), n.Online);

                builder.Append("" + rText);

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

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(2, "ktv789.aspx", 5, 0)));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=top") + "\">排行榜单</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=help") + "\">游戏规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 游戏玩法规则
    /// </summary>
    private void HelpPage()
    {
        Master.Title = "Ktv789玩法规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("Ktv789玩法规则");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append(Out.SysUBB(ub.GetSub("Ktv789Rule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">KTV</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)//登录
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));

        if (!new BCW.BLL.Game.ktv789().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //更新在线人数
        string lines = new BCW.BLL.Game.ktv789().GetLines(id);
        mename = mename.Replace("#", "").Replace("@", "").Replace("!", "");
        string Lines = "";
        if (lines != "")
            Lines = "#" + lines + "#";

        if (Lines.IndexOf("#" + meid + "@") == -1)
        {
            if (lines == "")
                Lines = "";
            else
                Lines = lines + "#";

            Lines = Lines + "" + meid + "@" + mename + "!" + DateTime.Now + "";
            new BCW.BLL.Game.ktv789().UpdateLines(id, Lines);
        }
        if (!string.IsNullOrEmpty(lines))
        {
            string[] arrLines = lines.ToString().Split("#".ToCharArray());
            for (int i = 0; i < arrLines.Length; i++)
            {

                try
                {
                    string[] sTemp = arrLines[i].Split("!".ToCharArray());
                    //string sID = sTemp[0].ToString();
                    DateTime sTime = Convert.ToDateTime(sTemp[1]);
                    if (sTime.AddSeconds(120) < DateTime.Now)
                    {
                        string NewLinks = lines.ToString().Replace("#" + arrLines[i], "");
                        NewLinks = NewLinks.Replace(arrLines[i] + "#", "");
                        NewLinks = NewLinks.Replace(arrLines[i], "");
                        new BCW.BLL.Game.ktv789().UpdateLines(id, NewLinks);
                    }
                    else
                    {
                        //更新在线时间
                    }

                }
                catch { }
            }
        }
        //重新计算在线人数
        string OverLines = new BCW.BLL.Game.ktv789().GetLines(id);
        if (OverLines != "")
        {
            string[] arrOverLines = OverLines.Split("#".ToCharArray());
            new BCW.BLL.Game.ktv789().UpdateOnline(id, arrOverLines.Length);
        }
        else
        {
            new BCW.BLL.Game.ktv789().UpdateOnline(id, 0);
        }
        BCW.Model.Game.ktv789 model = new BCW.BLL.Game.ktv789().Getktv789(id);
        int ptype = model.Types;
        string pName = string.Empty;
        if (ptype == 1)
            pName = "初级";
        else if (ptype == 2)
            pName = "中级";
        else
            pName = "终级";

        Master.Title = pName + "|" + model.StName + "";
        Master.Refresh = tm;
        Master.Gourl = Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "");
        //是否已经坐下,返回坐下的位置值
        int Pn = 0;
        DataSet ds = new BCW.BLL.Game.ktv789().GetList("OneUsId,TwoUsId,ThrUsId", "ID=" + id + " and (OneUsId=" + meid + " OR TwoUsId=" + meid + " OR ThrUsId=" + meid + ")");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Pn = 0;
        }
        else
        {
            int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
            int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
            int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());

            if (oneUsId == meid)
            {
                Pn = 1;
            }
            else if (twoUsId == meid)
            {
                Pn = 2;
            }
            else
            {
                Pn = 3;
            }

        }
        string pnText = string.Empty;
        string NextName = string.Empty;
        if (model.OneUsId == 0 && model.TwoUsId == 0 && model.ThrUsId == 0)
        {
            pnText = "等待三方加入";
        }
        else if (model.OneUsId == 0 && model.TwoUsId == 0)
        {
            pnText = "等待1号、2号加入";
        }
        else if (model.OneUsId == 0 && model.ThrUsId == 0)
        {
            pnText = "等待1号、3号加入";
        }
        else if (model.TwoUsId == 0 && model.ThrUsId == 0)
        {
            pnText = "等待2号、3号加入";
        }
        else if (model.OneUsId == 0)
        {
            pnText = "等待1号加入";
        }
        else if (model.TwoUsId == 0)
        {
            pnText = "等待2号加入";
        }
        else if (model.ThrUsId == 0)
        {
            pnText = "等待3号加入";
        }
        //等待出手
        if (string.IsNullOrEmpty(pnText))
        {
            if (model.NextShot == 1)
                NextName = model.OneUsName;
            else if (model.NextShot == 2)
                NextName = model.TwoUsName;
            else
                NextName = model.ThrUsName;
            pnText = "等待" + NextName + "出手";

        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("第" + model.PkCount + "局：" + pnText + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("ktv789.aspx?ptype=" + ptype + "") + "\">" + pName + "</a> ");
        if (tm != 0)
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "") + "\">手动</a> ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "") + "\">刷新</a> ");

        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=fresh&amp;id=" + id + "&amp;tm=5") + "\">设置</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=online&amp;id=" + id + "&amp;tm=" + tm + "") + "\">在线(" + model.Online + ")</a> ");

        builder.Append(Out.Tab("</div>", "<br />"));
        for (int i = 0; i < 3; i++)
        {
            builder.Append(Out.Tab("<div>", ""));
            string sName = string.Empty;
            int stuid = 0;
            string shot = string.Empty;
            DateTime dt = DateTime.Now;
            if (i == 0)
            {
                stuid = model.OneUsId;
                sName = model.OneUsName;
                shot = model.OneShot;
                dt = Convert.ToDateTime(model.OneTime);
            }
            else if (i == 1)
            {
                stuid = model.TwoUsId;
                sName = model.TwoUsName;
                shot = model.TwoShot;
                dt = Convert.ToDateTime(model.TwoTime);
            }
            else
            {
                stuid = model.ThrUsId;
                sName = model.ThrUsName;
                shot = model.ThrShot;
                dt = Convert.ToDateTime(model.ThrTime);
            }
            if (string.IsNullOrEmpty(sName))
                sName = "空位等待..";
            builder.Append("<a href=\"#card" + (i + 1) + "\">[" + (i + 1) + "]" + sName + "</a>");


            if (stuid == 0)
            {
                if (model.OneUsId != meid && model.TwoUsId != meid && model.ThrUsId != meid)
                {
                    strName = "id,v,tm,act";
                    strValu = "" + id + "'" + (i + 1) + "'" + tm + "'sit";
                    strOthe = "坐下,ktv789.aspx,post,0,other";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
            }
            else
            {
                //是否已全坐下位置
                bool IsMr = false;

                if (model.OneUsId != 0 && model.TwoUsId != 0 && model.ThrUsId != 0)
                {
                    IsMr = true;
                }
                if (string.IsNullOrEmpty(shot))
                {
                    if (IsMr == true)
                    {
                        if (DT.DateDiff(DateTime.Now, dt, 4) > model.Expir)
                        {
                            builder.Append(":未出手|超时<a href=\"" + Utils.getUrl("ktv789.aspx?act=kick&amp;id=" + id + "&amp;uid=" + stuid + "") + "\">[踢]</a>");
                        }
                        else
                        {
                            builder.Append(":未出手|" + (model.Expir - DT.DateDiff(DateTime.Now, dt, 4)) + "秒");
                        }

                    }
                    else
                    {
                        builder.Append(":未出手");
                    }
                }
                else
                {
                    builder.Append(":<a href=\"#card" + (i + 1) + "\">" + shot + "</a>");

                }
                if (stuid == meid)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=exit&amp;id=" + id + "") + "\">[退]</a>");
                }
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("等待摇骰,超时" + model.Expir + "秒");
        builder.Append(Out.Tab("</div>", ""));

        if (Pn > 0 && model.OneUsId != 0 && model.TwoUsId != 0 && model.ThrUsId != 0)
        {
            if (!new BCW.BLL.Game.ktv789().Exists(id, meid, Pn))
            {
                if (model.NextShot == Pn)
                {
                    strName = "id,ptype,tm,act";
                    strValu = "" + id + "'" + ptype + "'" + tm + "'shot";
                    strOthe = "摇骰,ktv789.aspx,post,0,other";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                    builder.Append(Out.Tab("", "<br />"));
                }
            }
        }

        // 开始读取动态列表
        int SizeNum = 6;
        string strWhere = "";
        strWhere = "Types=2 and NodeId=" + id + "";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", Out.Hr()));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), n.Notes);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=dt&amp;id=" + id + "&amp;tm=" + tm + "") + "\">更多游戏动态</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?ptype=" + ptype + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">KTV</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 刷新设置
    /// </summary>
    private void FreshPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "刷新设置";
        builder.Append(Out.Tab("<div class=\"title\">刷新设置</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("选择秒数:");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",,,";
        strName = "tm,id,act";
        strType = "select,hidden,hidden";
        strValu = "'" + id + "'view'";
        strEmpt = "5|5秒|10|10秒|15|15秒|20|20秒|30|30秒|40|40秒|50|50秒|60|60秒|90|90秒,,";
        strIdea = "/";
        strOthe = "确定,ktv789.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">KTV</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "789石头布排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("789赌神|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=top&amp;ptype=1") + "\">789赌神</a>|");

        if (ptype == 2)
            builder.Append("789狂人");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=top&amp;ptype=2") + "\">789狂人</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=2";
        if (ptype == 1)
            strOrder = "(WinGold+PutGold) Desc";
        else
            strOrder = "PutNum Desc";

        // 开始读取列表
        IList<BCW.Model.Toplist> listToplist = new BCW.BLL.Toplist().GetToplists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listToplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Toplist n in listToplist)
            {
                    n.UsName = BCW.User.Users.SetVipName(n.UsId, n.UsName, false);
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string sText = string.Empty;
                if (ptype == 1)
                    sText = "净赢" + (n.WinGold + n.PutGold) + "" + ub.Get("SiteBz") + "";
                else
                    sText = "共出手" + n.PutNum + "次";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}</a>{2}", n.UsId, n.UsName, sText);
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
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">KTV</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 游戏动态
    /// </summary>
    private void DtPage()
    {
        string StName = string.Empty;
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        if (id != 0)
        {
            if (!new BCW.BLL.Game.ktv789().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            StName = new BCW.BLL.Game.ktv789().GetStName(id);
        }
        Master.Title = "" + StName + "动态";
        builder.Append(Out.Tab("<div class=\"title\">" + StName + "动态</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id", "tm", "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id != 0)
            strWhere = "Types=2 and NodeId=" + id + "";
        else
            strWhere = "Types=2";

        // 开始读取列表
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(pageIndex, pageSize, strWhere, out recordCount);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
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

                builder.AppendFormat("{0}{1}({2}前)", "系统:", n.Notes, DT.DateDiff(DateTime.Now, n.AddTime));
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
        if (id != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">KTV</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 在线用户
    /// </summary>
    private void OnlinePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Game.ktv789().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.ktv789 model = new BCW.BLL.Game.ktv789().Getktv789(id);
        Master.Title = "" + model.StName + "在线用户";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("在线用户");
        builder.Append(Out.Tab("</div>", ""));

        if (!string.IsNullOrEmpty(model.Lines))
        {
            string[] arrLines = model.Lines.ToString().Split("#".ToCharArray());

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = arrLines.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < arrLines.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                    //取用户信息
                    string IDs = arrLines[i].Split("!".ToCharArray())[0];
                    string[] sID = IDs.Split("@".ToCharArray());
                    if (sID != null)
                    {
                        builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + sID[0] + "") + "\">" + sID[1].ToString() + "</a>");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx") + "\">KTV</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 坐下桌子
    /// </summary>
    private void SitPage()
    {
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        int v = int.Parse(Utils.GetRequest("v", "post", 2, @"^[1-3]$", "你坐错位置啦"));
        if (!new BCW.BLL.Game.ktv789().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //如果已有用户加入
        if (new BCW.BLL.Game.ktv789().Exists(id, v))
        {
            Utils.Error("此位置已有用户加入了哦，请选择其它位置加入", "");
        }
        //如果已加入了其它位置的游戏
        int ktv789Id = new BCW.BLL.Game.ktv789().Getktv789Id(meid);
        if (ktv789Id != 0)
        {
            Utils.Error("您正在其它房间对战，不能重复加入<br /><a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + ktv789Id + "") + "\">回我的房间</a>", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
        }

        //写入位置
        string mename = new BCW.BLL.User().GetUsName(meid);
        BCW.Model.Game.ktv789 model = new BCW.Model.Game.ktv789();
        if (v == 1)
        {
            model.ID = id;
            model.OneUsId = meid;
            model.OneUsName = mename;
            model.OneTime = DateTime.Now;
            new BCW.BLL.Game.ktv789().UpdateOne(model);
            new BCW.BLL.Game.ktv789().UpdateLLone(id);
        }
        else if (v == 2)
        {
            model.ID = id;
            model.TwoUsId = meid;
            model.TwoUsName = mename;
            model.TwoTime = DateTime.Now;
            new BCW.BLL.Game.ktv789().UpdateTwo(model);
            new BCW.BLL.Game.ktv789().UpdateLLtwo(id);
        }
        else
        {
            model.ID = id;
            model.ThrUsId = meid;
            model.ThrUsName = mename;
            model.ThrTime = DateTime.Now;
            new BCW.BLL.Game.ktv789().UpdateThr(model);
            new BCW.BLL.Game.ktv789().UpdateLLthr(id);
        }
        //重新读取实体
        BCW.Model.Game.ktv789 remodel = new BCW.BLL.Game.ktv789().Getktv789(id);
        if (remodel.Types == 1)//二人对决
        {
            if (remodel.OneUsId != 0 && remodel.TwoUsId != 0)
            {
                new BCW.BLL.Game.ktv789().UpdateTime(id);
            }
        }
        if (remodel.Types == 2 || remodel.Types == 3)//三人对决
        {
            if (remodel.OneUsId != 0 && remodel.TwoUsId != 0 && remodel.ThrUsId != 0)
            {
                new BCW.BLL.Game.ktv789().UpdateTime(id);
            }
        }
        string Notes = "" + mename + "加入了对战";
        new BCW.BLL.Action().Add(2, id, 0, "", Notes);
        Utils.Success("加入游戏", "加入游戏成功..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""), "1");
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    private void ExitPage()
    {
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int tm = int.Parse(Utils.GetRequest("tm", "get", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));

        if (info != "ok")
        {
            Master.Title = "退出游戏";
            Master.IsFoot = false;
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定退出游戏吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?info=ok&amp;act=exit&amp;id=" + id + "&amp;tm=" + tm + "") + "\">确定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Game.ktv789().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            DataSet ds = new BCW.BLL.Game.ktv789().GetList("OneUsId,TwoUsId,ThrUsId,OneUsName,TwoUsName,ThrUsName,OneShot,TwoShot,ThrShot,PayCent", "ID=" + id + " and (OneUsId=" + meid + " OR TwoUsId=" + meid + " OR ThrUsId=" + meid + ")");
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Utils.Error("你已经退出游戏", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
            }
            else
            {
                int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
                int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
                int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());
                string oneUsName = ds.Tables[0].Rows[0]["OneUsName"].ToString();
                string twoUsName = ds.Tables[0].Rows[0]["TwoUsName"].ToString();
                string thrUsName = ds.Tables[0].Rows[0]["ThrUsName"].ToString();

                string oneShot = ds.Tables[0].Rows[0]["OneShot"].ToString();
                string twoShot = ds.Tables[0].Rows[0]["TwoShot"].ToString();
                string thrShot = ds.Tables[0].Rows[0]["ThrShot"].ToString();
                int payCent = int.Parse(ds.Tables[0].Rows[0]["PayCent"].ToString());
                if (oneUsId == meid)
                {
                    new BCW.BLL.Game.ktv789().UpdateOneExit(id);
                    //如果已经出手，则重新退币
                    if (!string.IsNullOrEmpty(oneShot))
                    {
                        new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, Convert.ToInt64(payCent), 2);
                    }
                }
                else if (twoUsId == meid)
                {
                    new BCW.BLL.Game.ktv789().UpdateTwoExit(id);
                    //如果已经出手，则重新退币
                    if (!string.IsNullOrEmpty(twoShot))
                    {
                        new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, Convert.ToInt64(payCent), 2);
                    }
                }
                else if (thrUsId == meid)
                {
                    new BCW.BLL.Game.ktv789().UpdateThrExit(id);
                    //如果已经出手，则重新退币
                    if (!string.IsNullOrEmpty(thrShot))
                    {
                        new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, Convert.ToInt64(payCent), 2);
                    }
                }
            }

            string mename = new BCW.BLL.User().GetUsName(meid);
            string Notes = "" + mename + "退出了游戏";
            new BCW.BLL.Action().Add(2, id, 0, "", Notes);
            Utils.Success("退出游戏", "退出游戏成功..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }

    /// <summary>
    /// 踢出游戏
    /// </summary>
    private void KickPage()
    {
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int tm = int.Parse(Utils.GetRequest("tm", "get", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "get", 2, @"^[0-9]\d*$", "UID错误"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (info != "ok")
        {
            Master.Title = "踢出游戏";
            Master.IsFoot = false;
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将其踢出游戏吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?info=ok&amp;act=kick&amp;id=" + id + "&amp;uid=" + uid + "&amp;tm=" + tm + "") + "\">确定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {

            if (!new BCW.BLL.Game.ktv789().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            DataSet ds = new BCW.BLL.Game.ktv789().GetList("OneUsId,TwoUsId,ThrUsId,OneUsName,TwoUsName,ThrUsName,OneShot,TwoShot,ThrShot,Expir,OneTime,TwoTime,ThrTime,PayCent,Types", "ID=" + id + " and (OneUsId=" + uid + " OR TwoUsId=" + uid + " OR ThrUsId=" + uid + ")");
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Utils.Error("TA已经退出游戏", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
            }
            else
            {
                int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
                int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
                int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());
                string oneUsName = ds.Tables[0].Rows[0]["OneUsName"].ToString();
                string twoUsName = ds.Tables[0].Rows[0]["TwoUsName"].ToString();
                string thrUsName = ds.Tables[0].Rows[0]["ThrUsName"].ToString();

                string oneShot = ds.Tables[0].Rows[0]["OneShot"].ToString();
                string twoShot = ds.Tables[0].Rows[0]["TwoShot"].ToString();
                string thrShot = ds.Tables[0].Rows[0]["ThrShot"].ToString();

                //超时时间
                int ExpirTime = int.Parse(ds.Tables[0].Rows[0]["Expir"].ToString());

                DateTime oneTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["OneTime"]);
                DateTime twoTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["TwoTime"]);
                DateTime thrTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["ThrTime"]);

                int payCent = int.Parse(ds.Tables[0].Rows[0]["PayCent"].ToString());
                int Types = int.Parse(ds.Tables[0].Rows[0]["Types"].ToString());
                //是否已全坐下位置
                bool IsMr = false;

                if (oneUsId != 0 && twoUsId != 0 && thrUsId != 0)
                {
                    IsMr = true;
                }

                if (IsMr == false)
                {
                    Utils.Error("游戏还没有开始呢..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
                }

                if (oneUsId == uid)
                {
                    //如果未超时
                    if (DT.DateDiff(DateTime.Now, oneTime, 4) < ExpirTime)
                    {
                        Utils.Error("还未超时，不能踢出..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
                    }

                    new BCW.BLL.Game.ktv789().UpdateOneExit(id);
                }
                else if (twoUsId == uid)
                {
                    //如果未超时
                    if (DT.DateDiff(DateTime.Now, twoTime, 4) < ExpirTime)
                    {
                        Utils.Error("还未超时，不能踢出..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
                    }
                    new BCW.BLL.Game.ktv789().UpdateTwoExit(id);
                }
                else if (thrUsId == uid)
                {
                    //如果未超时
                    if (DT.DateDiff(DateTime.Now, oneTime, 4) < ExpirTime)
                    {
                        Utils.Error("还未超时，不能踢出..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""));
                    }
                    new BCW.BLL.Game.ktv789().UpdateThrExit(id);
                }
            }

            string mename = new BCW.BLL.User().GetUsName(uid);
            string Notes = "" + mename + "被踢出了游戏";
            new BCW.BLL.Action().Add(2, id, 0, "", Notes);
            Utils.Success("踢出游戏", "踢出" + mename + "成功..", Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }


    /// <summary>
    /// 出手动作
    /// </summary>
    private void ShotPage()
    {
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Game.ktv789().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.ktv789 model = new BCW.BLL.Game.ktv789().Getktv789(id);
        int Pn = 0;

        //是否已经坐下,返回坐下的位置值
        DataSet ds = new BCW.BLL.Game.ktv789().GetList("OneUsId,TwoUsId,ThrUsId", "ID=" + id + " and (OneUsId=" + meid + " OR TwoUsId=" + meid + " OR ThrUsId=" + meid + ")");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("您还没坐下..", "");
        }

        int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
        int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
        int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());

        if (oneUsId == meid)
        {
            Pn = 1;
        }
        else if (twoUsId == meid)
        {
            Pn = 2;
        }
        else
        {
            Pn = 3;
        }


        if (model.NextShot != Pn)
        {
            string NextName = string.Empty;
            if (model.NextShot == 1)
                NextName = model.OneUsName;
            else if (model.NextShot == 2)
                NextName = model.TwoUsName;
            else
                NextName = model.ThrUsName;

            Utils.Error("请等待" + NextName + "出手", "");
        }


        //如果已经出手
        if (new BCW.BLL.Game.ktv789().Exists(id, meid, Pn))
        {
            Utils.Error("您已出手，不用重复出手", "");
        }
        if (model.GoldType == 0)
        {
            //用户币数
            long Gold = new BCW.BLL.User().GetGold(meid);
            if (Gold < Convert.ToInt64(model.PayCent))
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }
            //支付安全提示
            string[] p_pageArr = { "act", "id", "tm" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            //扣币
            new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(model.PayCent), 2);
        }
        //取随机数
        int Ran = new Random().Next(11, 66);
        int Ran1 = int.Parse(Utils.Left(Ran.ToString(), 1));
        int Ran2 = int.Parse(Utils.Right(Ran.ToString(), 1));
        string st = Ran1.ToString() + "+" + Ran2.ToString();

        //写入位置
        BCW.Model.Game.ktv789 addmodel = new BCW.Model.Game.ktv789();
        if (Pn == 1)
        {
            addmodel.ID = id;
            addmodel.OneShot = st;
            new BCW.BLL.Game.ktv789().UpdateOneShot(addmodel);
        }
        else if (Pn == 2)
        {
            addmodel.ID = id;
            addmodel.TwoShot = st;
            new BCW.BLL.Game.ktv789().UpdateTwoShot(addmodel);
        }
        else
        {
            addmodel.ID = id;
            addmodel.ThrShot = st;
            new BCW.BLL.Game.ktv789().UpdateThrShot(addmodel);
        }
        BCW.Model.Game.ktv789 remodel = new BCW.BLL.Game.ktv789().Getktv789(id);
        //结果逻辑
        int iRan = Ran1 + Ran2;
        string winText = string.Empty;
        string wText = string.Empty;
        string forname = string.Empty;
        int NextShotType = 0;
        if (Ran1 == Ran2)//同色全赢
        {
            if (Pn == 1)
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent * 3 - (remodel.PayCent * 2) * GetTax()), 2);
            }
            else if (Pn == 2)
            {
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent * 3 - (remodel.PayCent * 2) * GetTax()), 2);

            }
            else
            {
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent * 3 - (remodel.PayCent * 2) * GetTax()), 2);

            }
            winText = "恭喜！骰子同色" + (Ran1) + "+" + (Ran2) + ",赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";
            wText = "" + mename + "(" + (Ran1) + "+" + (Ran2) + ")赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";
            NextShotType = Pn;
        }
        else if (iRan == 7)//上一家全输,其它两家各赢一半币
        {
            if (Pn == 1)//3桌全输
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                forname = remodel.ThrUsName;
                NextShotType = 3;
            }
            else if (Pn == 2)//1桌全输
            {
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                forname = remodel.OneUsName;
                NextShotType = 1;
            }
            else//2桌全输
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                forname = remodel.TwoUsName;
                NextShotType = 2;
            }
            winText = "恭喜！骰子" + (Ran1 + Ran2) + "(" + (Ran1) + "+" + (Ran2) + "),您与下一玩家分别赢了上玩家" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
            wText = "" + mename + "(" + (Ran1) + "+" + (Ran2) + ")与" + forname + "赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";
        }
        else if (iRan == 8)//输半
        {
            if (Pn == 1)
            {
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.25) - (remodel.PayCent * 0.25) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.25) - (remodel.PayCent * 0.25) * GetTax()), 2);

            }
            else if (Pn == 2)
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.25) - (remodel.PayCent * 0.25) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.25) - (remodel.PayCent * 0.25) * GetTax()), 2);

            }
            else
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.25) - (remodel.PayCent * 0.25) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.25) - (remodel.PayCent * 0.25) * GetTax()), 2);

            }
            winText = "很遗憾！骰子" + (Ran1 + Ran2) + "(" + (Ran1) + "+" + (Ran2) + "),两位玩家分别赢了您的" + (remodel.PayCent * 0.25) + "" + ub.Get("SiteBz") + "";
            wText = "" + mename + "(" + (Ran1) + "+" + (Ran2) + ")输了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
            NextShotType = Pn;
        }
        else if (iRan == 9)//全输
        {
            if (Pn == 1)
            {
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);

            }
            else if (Pn == 2)
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);

            }
            else if (Pn == 3)
            {
                new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);
                new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + (remodel.PayCent * 0.5) - (remodel.PayCent * 0.5) * GetTax()), 2);

            }
            winText = "很遗憾！骰子" + (Ran1 + Ran2) + "(" + (Ran1) + "+" + (Ran2) + "),两位玩家分别赢了您的" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
            wText = "" + mename + "(" + (Ran1) + "+" + (Ran2) + ")输了" + (remodel.PayCent * 1) + "" + ub.Get("SiteBz") + "";
            NextShotType = Pn;
        }
        else //轮下位
        {
            if (!string.IsNullOrEmpty(remodel.OneShot) && !string.IsNullOrEmpty(remodel.TwoShot) && !string.IsNullOrEmpty(remodel.ThrShot))
            {
                //重置桌子
                new BCW.BLL.Game.ktv789().UpdateShot(id);
                if (Pn == 1)
                {
                    NextShotType = 2;
                }
                else if (Pn == 2)
                {
                    NextShotType = 3;
                }
                else
                {
                    NextShotType = 1;
                }
            }
        }


        if (!string.IsNullOrEmpty(winText))
        {
            //记录系统消息
            new BCW.BLL.Action().Add(2, id, 0, "", wText);
            //重置桌子
            new BCW.BLL.Game.ktv789().UpdateShot(id);

        }
        else
        {
            if (Pn == 1)
            {
                NextShotType = 2;
            }
            else if (Pn == 2)
            {
                NextShotType = 3;
            }
            else
            {
                NextShotType = 1;
            }
            winText = "摇出骰子" + (Ran1 + Ran2) + "(" + (Ran1) + "+" + (Ran2) + "),请等待下家摇骰..";
        }

        //更新下一位出手
        new BCW.BLL.Game.ktv789().UpdateNextShot(id, NextShotType);
        Utils.Success("摇出骰子", winText, Utils.getUrl("ktv789.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + ""), "3");
    }
}