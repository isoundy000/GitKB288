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

public partial class bbs_game_stone : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/stone.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;

    /// <summary>
    /// 取剪刀类型
    /// </summary>
    /// <param name="st"></param>
    /// <returns></returns>
    private string GetStName(int st)
    {
        string stName = string.Empty;
        if (st == 1)
            stName = "剪刀";
        else if (st == 2)
            stName = "石头";
        else
            stName = "布";
        return stName;
    }

    /// <summary>
    /// 取税率
    /// </summary>
    /// <returns></returns>
    private double GetTax()
    {
        double Tax = 0;
        try
        {
            Tax = Convert.ToDouble(ub.GetSub("StoneTax", xmlPath));
        }
        catch { }
        return Convert.ToDouble(Tax * 0.01);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("StoneStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();
                break;
            case "fresh":
                FreshPage();
                break;
            case "sit":
                SitPage();
                break;
            case "exit":
                ExitPage();
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

    /// <summary>
    /// 游戏首页
    /// </summary>
    private void ReloadPage()
    {
        //定期清离桌用户
        DataSet ds = new BCW.BLL.Game.Stone().GetList("ID", "Types<>4 and (OneTime<'" + DateTime.Now.AddHours(-2) + "' OR TwoTime<'" + DateTime.Now.AddHours(-2) + "' OR ThrTime<'" + DateTime.Now.AddHours(-2) + "')");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                new BCW.BLL.Game.Stone().UpdateClear(id);
            }
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "4"));
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("StoneName",xmlPath);
        string Logo = ub.GetSub("StoneLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string Notes = ub.GetSub("StoneNotes", xmlPath);
        if (Notes != "")
            builder.Append(Notes + "<br />");

        if (ptype == 4)
            builder.Append("混战区|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=4") + "\">混战</a>|");

        if (ptype == 1)
            builder.Append("二人对决|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=1") + "\">二人</a>|");

        if (ptype == 2)
            builder.Append("三人对决|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=2") + "\">三人</a>|");

        if (ptype == 3)
            builder.Append("生死战");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=3") + "\">生死</a>");


        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype"};
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=" + ptype + "";

        // 开始读取列表
        IList<BCW.Model.Game.Stone> listStone = new BCW.BLL.Game.Stone().GetStones(pageIndex, pageSize, strWhere, out recordCount);
        if (listStone.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stone n in listStone)
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
                    if (ptype != 4)
                        rText += "房间空闲中...";
                    else
                        rText += "混战空闲中...";
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
                            if (sTime.AddMinutes(2) < DateTime.Now)
                            {
                                string Lines = n.Lines.ToString().Replace("#" + arrLines[i], "");
                                Lines = Lines.Replace(arrLines[i] + "#", "");
                                Lines = Lines.Replace(arrLines[i], "");
                                new BCW.BLL.Game.Stone().UpdateLines(n.ID, Lines);
                            }
                        }
                        catch { }
                    }
                    //重新计算在线人数
                    string OverLines = new BCW.BLL.Game.Stone().GetLines(n.ID);
                    if (OverLines != "")
                    {
                        string[] arrOverLines = OverLines.Split("#".ToCharArray());
                        new BCW.BLL.Game.Stone().UpdateOnline(n.ID, arrOverLines.Length);
                    }
                    else
                    {
                        new BCW.BLL.Game.Stone().UpdateOnline(n.ID, 0);
                    }
                }

                builder.AppendFormat("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id={0}&amp;tm=30") + "\">{1}</a>({2})", n.ID, n.StName, n.Online);
               
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
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(1, "stone.aspx", 5, 0)));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=top") + "\">剪刀排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=help") + "\">游戏规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回游戏中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 游戏玩法规则
    /// </summary>
    private void HelpPage()
    {
        Master.Title = "剪刀石头布玩法规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=玩法规则=");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append(Out.SysUBB(ub.GetSub("StoneRule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">剪刀</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    /// <summary>
    /// 游戏桌面
    /// </summary>
    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)//登录
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));

        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //更新在线人数
        string lines = new BCW.BLL.Game.Stone().GetLines(id);
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
            new BCW.BLL.Game.Stone().UpdateLines(id, Lines);
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
                    if (sTime.AddMinutes(2) < DateTime.Now)
                    {
                        string NewLinks = lines.ToString().Replace("#" + arrLines[i], "");
                        NewLinks = NewLinks.Replace(arrLines[i] + "#", "");
                        NewLinks = NewLinks.Replace(arrLines[i], "");
                        new BCW.BLL.Game.Stone().UpdateLines(id, NewLinks);
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
        string OverLines = new BCW.BLL.Game.Stone().GetLines(id);
        if (OverLines != "")
        {
            string[] arrOverLines = OverLines.Split("#".ToCharArray());
            new BCW.BLL.Game.Stone().UpdateOnline(id, arrOverLines.Length);
        }
        else
        {
            new BCW.BLL.Game.Stone().UpdateOnline(id, 0);
        }

        BCW.Model.Game.Stone model = new BCW.BLL.Game.Stone().GetStone(id);

        int ptype = model.Types;
        string pName = string.Empty;
        if (ptype == 1)
            pName = "二人对决";
        else if (ptype == 2)
            pName = "三人对决";
        else if (ptype == 3)
            pName = "生死战";
        else
            pName = "混战";
        Master.Title = pName + "|" + model.StName + "";
        Master.Refresh = tm;
        Master.Gourl = Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));


        //是否已经坐下,返回坐下的位置值
        int Pn = 0;
        int Num = 1;
        int istat = 0;
        DataSet ds = new BCW.BLL.Game.Stone().GetList("OneUsId,TwoUsId,ThrUsId,OneStat,TwoStat,ThrStat", "ID=" + id + " and (OneUsId=" + meid + " OR TwoUsId=" + meid + " OR ThrUsId=" + meid + ")");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Pn = 0;
        }
        else
        {
            int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
            int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
            int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());
            int oneStat = int.Parse(ds.Tables[0].Rows[0]["OneStat"].ToString());
            int twoStat = int.Parse(ds.Tables[0].Rows[0]["TwoStat"].ToString());
            int thrStat = int.Parse(ds.Tables[0].Rows[0]["ThrStat"].ToString());

            if (oneUsId == meid)
            {
                Pn = 1;
                istat = oneStat;
            }
            else if (twoUsId == meid)
            {
                Pn = 2;
                istat = twoStat;
            }
            else
            {
                Pn = 3;
                istat = thrStat;
            }
            //第几次出手
            if (model.OneShot != 0)
                Num++;
            if (model.TwoShot != 0)
                Num++;
            if (model.ThrShot != 0)
                Num++;
        }
        string pnText = string.Empty;
        string NextName = string.Empty;
        if (model.Types == 1)
        {
            if (model.OneUsId == 0 && model.TwoUsId == 0)
            {
                pnText = "：等待双方加入";
            }
            else if (model.OneUsId == 0)
            {
                pnText = "：等待1号加入";
            }
            else if (model.TwoUsId == 0)
            {
                pnText = "：等待2号加入";
            }
            //等待出手
            if (string.IsNullOrEmpty(pnText))
            {
                if (model.ShotTypes == 1 && Num == 1)
                {
                    if (model.NextShot == 1)
                        NextName = model.OneUsName;
                    else if (model.NextShot == 2)
                        NextName = model.TwoUsName;
                    else
                        NextName = model.ThrUsName;
                    pnText = "：等待" + NextName + "出手";
                }
                else
                {
                    if (model.OneShot == 0 && model.TwoShot == 0)
                    {
                        pnText = "：等待双方出手";
                    }
                    else if (model.OneShot == 0)
                    {
                        pnText = "：等待" + model.OneUsName + "出手";
                    }
                    else if (model.TwoShot == 0)
                    {
                        pnText = "：等待" + model.TwoUsName + "出手";
                    }
                }
            }
            builder.Append("第" + model.PkCount + "局" + pnText + "");
        }
        else if (model.Types == 2 || (model.Types == 3 && model.IsStatus == 0))
        {
            if (model.OneUsId == 0 && model.TwoUsId == 0 && model.ThrUsId == 0)
            {
                pnText = "：等待三方加入";
            }
            else if (model.OneUsId == 0 && model.TwoUsId == 0)
            {
                pnText = "：等待1号、2号加入";
            }
            else if (model.OneUsId == 0 && model.ThrUsId == 0)
            {
                pnText = "：等待1号、3号加入";
            }
            else if (model.TwoUsId == 0 && model.ThrUsId == 0)
            {
                pnText = "：等待2号、3号加入";
            }
            else if (model.OneUsId == 0)
            {
                pnText = "：等待1号加入";
            }
            else if (model.TwoUsId == 0)
            {
                pnText = "：等待2号加入";
            }
            else if (model.ThrUsId == 0)
            {
                pnText = "：等待3号加入";
            }
            //等待出手
            if (string.IsNullOrEmpty(pnText))
            {
                if (model.ShotTypes == 1 && Num == 1)
                {
                    if (model.NextShot == 1)
                        NextName = model.OneUsName;
                    else if (model.NextShot == 2)
                        NextName = model.TwoUsName;
                    else
                        NextName = model.ThrUsName;
                    pnText = "：等待" + NextName + "出手";
                }
                else
                {
                    if (model.OneShot == 0 && model.TwoShot == 0 && model.ThrShot == 0)
                    {
                        pnText = "：等待三方出手";
                    }
                    else if (model.OneShot == 0 && model.TwoShot == 0)
                    {
                        pnText = "：等待" + model.OneUsName + "、" + model.TwoUsName + "出手";
                    }
                    else if (model.OneShot == 0 && model.ThrShot == 0)
                    {
                        pnText = "：等待" + model.OneUsName + "、" + model.ThrUsName + "出手";
                    }
                    else if (model.TwoShot == 0 && model.ThrShot == 0)
                    {
                        pnText = "：等待" + model.TwoUsName + "、" + model.ThrUsName + "出手";
                    }
                    else if (model.OneShot == 0)
                    {
                        pnText = "：等待" + model.OneUsName + "出手";
                    }
                    else if (model.TwoShot == 0)
                    {
                        pnText = "：等待" + model.TwoUsName + "出手";
                    }
                    else if (model.ThrShot == 0)
                    {
                        pnText = "：等待" + model.ThrUsName + "出手";
                    }
                }
            }
            builder.Append("第" + model.PkCount + "局" + pnText + "");
        }
        else if (model.Types == 3 && model.IsStatus == 1)
        {
            //ab PK
            if (model.OneStat != 0 && model.TwoStat != 0)
            {
                pnText = "决战局：" + model.OneUsName + "PK" + model.TwoUsName + "";
                if (model.OneShot == 0 && model.TwoShot == 0)
                {
                    pnText += "，等待双方出手";
                }
                else if (model.OneShot == 0)
                {
                    pnText += "，等待" + model.OneUsName + "出手";
                }
                else if (model.TwoShot == 0)
                {
                    pnText += "，等待" + model.TwoUsName + "出手";
                }
            }
            //bc PK
            else if (model.TwoStat != 0 && model.ThrStat != 0)
            {
                pnText = "决战局：" + model.TwoUsName + "PK" + model.ThrUsName + "";
                if (model.TwoShot == 0 && model.ThrShot == 0)
                {
                    pnText += "，等待双方出手";
                }
                else if (model.TwoShot == 0)
                {
                    pnText += "，等待" + model.TwoUsName + "出手";
                }
                else if (model.ThrShot == 0)
                {
                    pnText += "，等待" + model.ThrUsName + "出手";
                }
            }
            //ac PK
            else if (model.OneStat != 0 && model.ThrStat != 0)
            {
                pnText = "决战局：" + model.OneUsName + "PK" + model.ThrUsName + "";
                if (model.OneShot == 0 && model.ThrShot == 0)
                {
                    pnText += "，等待双方出手";
                }
                else if (model.OneShot == 0)
                {
                    pnText += "，等待" + model.OneUsName + "出手";
                }
                else if (model.ThrShot == 0)
                {
                    pnText += "，等待" + model.ThrUsName + "出手";
                }
            }
            builder.Append("" + pnText + "");
        }
        else if (model.Types == 4)
        {
            if (model.OneUsId == 0)
            {
                pnText += "等待你发起挑战！";
            }
            else
            {
                if (model.OneUsId != meid)
                    pnText += "等待你来应战！";
                else
                    pnText += "等待友友来应战！";
            }
            builder.Append("" + pnText + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=" + ptype + "") + "\">" + pName + "</a> ");
        if (tm != 0)
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "") + "\">手动</a> ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "") + "\">刷新</a> ");

        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=fresh&amp;id=" + id + "&amp;tm=5") + "\">设置</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=online&amp;id=" + id + "&amp;tm=" + tm + "") + "\">在线(" + model.Online + ")</a> ");

        builder.Append(Out.Tab("</div>", "<br />"));
        if (model.Types != 4)
        {
            //对决类型
            int z = 0;
            if (ptype == 1)
            {
                z = 2;
            }
            else
            {
                z = 3;
            }

            for (int i = 0; i < z; i++)
            {
                builder.Append(Out.Tab("<div>", ""));
                string sName = string.Empty;
                int stuid = 0;
                int shot = 0;
                DateTime dt = DateTime.Now;
                int stat = 0;
                if (i == 0)
                {
                    stuid = model.OneUsId;
                    sName = model.OneUsName;
                    shot = model.OneShot;
                    dt = Convert.ToDateTime(model.OneTime);
                    stat = model.OneStat;
                }
                else if (i == 1)
                {
                    stuid = model.TwoUsId;
                    sName = model.TwoUsName;
                    shot = model.TwoShot;
                    dt = Convert.ToDateTime(model.TwoTime);
                    stat = model.TwoStat;
                }
                else
                {
                    stuid = model.ThrUsId;
                    sName = model.ThrUsName;
                    shot = model.ThrShot;
                    dt = Convert.ToDateTime(model.ThrTime);
                    stat = model.ThrStat;
                }
                if (string.IsNullOrEmpty(sName))
                    sName = "空位等待..";
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + stuid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[" + (i + 1) + "]" + sName + "</a>");

                if (stuid == 0)
                {
                    if (model.OneUsId != meid && model.TwoUsId != meid && model.ThrUsId != meid)
                    {
                        strName = "id,v,tm,act";
                        strValu = "" + id + "'" + (i + 1) + "'" + tm + "'sit";
                        strOthe = "坐下,stone.aspx,post,0,blue";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                    }
                }
                else
                {
                    //是否已全坐下位置
                    bool IsMr = false;
                    if (model.Types == 1)
                    {
                        if (model.OneUsId != 0 && model.TwoUsId != 0)
                            IsMr = true;
                    }
                    else if (model.Types == 2 || model.Types == 3)
                    {
                        if (model.OneUsId != 0 && model.TwoUsId != 0 && model.ThrUsId != 0)
                        {
                            IsMr = true;
                        }
                    }

                    if (shot == 0)
                    {
                        //决战备战用户
                        if (model.IsStatus != 0 && stat == 0)
                        {
                            builder.Append(":备战中");
                        }
                        else
                        {
                            if (IsMr == true)
                            {
                                if (model.ShotTypes == 1 && Num == 1 && model.NextShot != (i + 1))
                                {
                                    builder.Append(":等待出手");
                                }
                                else
                                {

                                    if (DT.DateDiff(DateTime.Now, dt, 4) > model.Expir)
                                    {
                                        builder.Append(":未出手|超时<a href=\"" + Utils.getUrl("stone.aspx?act=kick&amp;id=" + id + "&amp;uid=" + stuid + "") + "\">[踢]</a>");
                                    }
                                    else
                                    {
                                        builder.Append(":未出手|" + (model.Expir - DT.DateDiff(DateTime.Now, dt, 4)) + "秒");
                                    }
                                }
                            }
                            else
                            {
                                builder.Append(":未出手");
                            }
                        }
                    }
                    else
                    {
                        string stName = string.Empty;
                        if (stuid == meid)
                        {
                            if (shot == 1)
                                stName = "剪刀";
                            else if (shot == 2)
                                stName = "石头";
                            else
                                stName = "布";
                        }
                        else
                        {
                            stName = "出手";
                        }
                        builder.Append(":<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + stuid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + stName + "" + model.PayCent + "" + ub.Get("SiteBz") + "</a>");

                    }
                    if (stuid == meid)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=exit&amp;id=" + id + "") + "\">[退]</a>");
                    }
                }
                builder.Append(Out.Tab("</div>", "<br />"));
            }


            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (Num == 1)
                builder.Append("等待下注,超时" + model.Expir + "秒");
            else
                builder.Append("下注" + model.PayCent + "" + ub.Get("SiteBz") + ",超时" + model.Expir + "秒");
            builder.Append(Out.Tab("</div>", ""));
            //如果还没有出手
            //决战备战用户
            if (model.IsStatus != 0 && istat == 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("等待再次开战..");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                if (Pn > 0)
                {
                    if (!new BCW.BLL.Game.Stone().Exists(id, meid, Pn))
                    {
                        if (model.ShotTypes == 0)
                        {
                            builder.Append(Out.Tab("", "<br />"));
                            if (Num == 1)
                            {
                                strText = "下注:,/选择:,,,,,";
                                strName = "payCent,st,Num,id,ptype,tm,act";
                                strType = "num,select,hidden,hidden,hidden,hidden,hidden";
                                strValu = "'1'" + Num + "'" + id + "'" + ptype + "'" + tm + "'shot";
                                strEmpt = "false,1|剪刀|2|石头|3|布,false,false,false,false,false";
                                strIdea = "";
                                strOthe = "出手,stone.aspx,post,3,other";
                            }
                            else
                            {
                                strText = "选择:,,,,,";
                                strName = "st,Num,id,ptype,tm,act";
                                strType = "select,hidden,hidden,hidden,hidden,hidden";
                                strValu = "1'" + Num + "'" + id + "'" + ptype + "'" + tm + "'shot";
                                strEmpt = "1|剪刀|2|石头|3|布,false,false,false,false,false";
                                strIdea = "";
                                strOthe = "应战,stone.aspx,post,3,other";
                            }
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                        }
                        else
                        {

                            if (Num == 1 && model.NextShot == Pn)
                            {
                                builder.Append(Out.Tab("", "<br />"));
                                strText = "下注:,/选择:,,,,,";
                                strName = "payCent,st,Num,id,ptype,tm,act";
                                strType = "num,select,hidden,hidden,hidden,hidden,hidden";
                                strValu = "'1'" + Num + "'" + id + "'" + ptype + "'" + tm + "'shot";
                                strEmpt = "false,1|剪刀|2|石头|3|布,false,false,false,false,false";
                                strIdea = "";
                                strOthe = "出手,stone.aspx,post,3,other";
                                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                            }
                            else if (Num > 1)
                            {
                                builder.Append(Out.Tab("", "<br />"));
                                strText = "选择:,,,,,";
                                strName = "st,Num,id,ptype,tm,act";
                                strType = "select,hidden,hidden,hidden,hidden,hidden";
                                strValu = "1'" + Num + "'" + id + "'" + ptype + "'" + tm + "'shot";
                                strEmpt = "1|剪刀|2|石头|3|布,false,false,false,false,false";
                                strIdea = "";
                                strOthe = "应战,stone.aspx,post,3,other";
                                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                            }
                        }

                    }
                }
            }
        }
        else  //混战开始
        {
            if (model.OneUsId == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("还没人发起混战..");
                builder.Append(Out.Tab("</div>", "<br />"));
                strText = "下注:,/选择:,,,,,";
                strName = "payCent,st,Num,id,ptype,tm,act";
                strType = "num,select,hidden,hidden,hidden,hidden,hidden";
                strValu = "'1'" + Num + "'" + id + "'" + ptype + "'" + tm + "'shot";
                if (IsStone(meid))
                    strEmpt = "false,1|剪刀|2|石头|3|布|4|全赢|5|平手|6|全输,false,false,false,false,false";
                else
                    strEmpt = "false,1|剪刀|2|石头|3|布,false,false,false,false,false";

                strIdea = "";
                strOthe = "出手,stone.aspx,post,3,other";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));

                if (model.OneUsId != meid)
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.OneUsId + "") + "\">[擂主]" + model.OneUsName + ":" + model.PayCent + "" + ub.Get("SiteBz") + "</a>");
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.OneUsId + "") + "\">[擂主]" + model.OneUsName + ":" + GetStName(model.OneShot) + "" + model.PayCent + "" + ub.Get("SiteBz") + "</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=exit&amp;id=" + id + "") + "\">[退]</a>");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                if (model.OneUsId != meid)
                {
                    strText = "选择:,,,,,";
                    strName = "st,Num,id,ptype,tm,act";
                    strType = "select,hidden,hidden,hidden,hidden,hidden";
                    strValu = "1'2'" + id + "'" + ptype + "'" + tm + "'shot";
                    strEmpt = "1|剪刀|2|石头|3|布,false,false,false,false,false";

                    strIdea = "";
                    strOthe = "应战,stone.aspx,post,3,other";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
        }


        // 开始读取动态列表
        int SizeNum = 6;
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, "Types=1 and NodeId=" + id + "");
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", Out.Hr()));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime) , n.Notes);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=dt&amp;id=" + id + "&amp;tm=" + tm + "") + "\">更多游戏动态</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        //用户币
        long Gold = new BCW.BLL.User().GetGold(meid);

        builder.Append("你目前自带" + Utils.ConvertGold(Gold) + "" + ub.Get("SiteBz") + "<br />");
        if (model.ShotTypes == 0)
            builder.Append("自由出手:");
        else
            builder.Append("轮流出手:");

        builder.Append(model.SmallPay + "-" + model.BigPay);

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?ptype=" + ptype + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">剪刀</a>");
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
        strOthe = "确定,stone.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", " "));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">剪刀</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "剪刀石头布排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("剪刀赌神|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=top&amp;ptype=1") + "\">剪刀赌神</a>|");

        if (ptype == 2)
            builder.Append("剪刀狂人");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=top&amp;ptype=2") + "\">剪刀狂人</a>");
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
        strWhere = "Types=1";
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
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">剪刀</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 游戏动态
    /// </summary>
    private void DtPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        string StName = new BCW.BLL.Game.Stone().GetStName(id);
        Master.Title = "" + StName + "动态";
        builder.Append(Out.Tab("<div class=\"title\">" + StName + "动态</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = {"id","tm","act"};
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=1 and NodeId=" + id + "";

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
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">剪刀</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 在线用户
    /// </summary>
    private void OnlinePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int tm = int.Parse(Utils.GetRequest("tm", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.Stone model = new BCW.BLL.Game.Stone().GetStone(id);
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
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stone.aspx") + "\">剪刀</a>");
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
        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //如果已有用户加入
        if (new BCW.BLL.Game.Stone().Exists(id, v))
        {
            Utils.Error("此位置已有用户加入了哦，请选择其它位置加入", "");
        }
        //如果已加入了其它位置的游戏
        int StoneId = new BCW.BLL.Game.Stone().GetStoneId(meid);
        if (StoneId != 0)
        {
            Utils.Error("您正在其它房间对战，不能重复加入<br /><a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + StoneId + "") + "\">回我的房间</a>", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
        }

        //写入位置
        string mename = new BCW.BLL.User().GetUsName(meid);
        BCW.Model.Game.Stone model = new BCW.Model.Game.Stone();
        if (v == 1)
        {
            model.ID = id;
            model.OneUsId = meid;
            model.OneUsName = mename;
            model.OneTime = DateTime.Now;
            new BCW.BLL.Game.Stone().UpdateOne(model);
            new BCW.BLL.Game.Stone().UpdateLLone(id);
        }
        else if (v == 2)
        {
            model.ID = id;
            model.TwoUsId = meid;
            model.TwoUsName = mename;
            model.TwoTime = DateTime.Now;
            new BCW.BLL.Game.Stone().UpdateTwo(model);
            new BCW.BLL.Game.Stone().UpdateLLtwo(id);
        }
        else
        {
            model.ID = id;
            model.ThrUsId = meid;
            model.ThrUsName = mename;
            model.ThrTime = DateTime.Now;
            new BCW.BLL.Game.Stone().UpdateThr(model);
            new BCW.BLL.Game.Stone().UpdateLLthr(id);
        }
        //重新读取实体
        BCW.Model.Game.Stone remodel = new BCW.BLL.Game.Stone().GetStone(id);
        if (remodel.Types == 1)//二人对决
        {
            if (remodel.OneUsId != 0 && remodel.TwoUsId != 0)
            {
                new BCW.BLL.Game.Stone().UpdateTime(id);
            }
        }
        if (remodel.Types == 2 || remodel.Types == 3)//三人对决
        {
            if (remodel.OneUsId != 0 && remodel.TwoUsId != 0 && remodel.ThrUsId != 0)
            {
                new BCW.BLL.Game.Stone().UpdateTime(id);
            }
        }
        string Notes = "" + mename + "加入了对战";
        new BCW.BLL.Action().Add(1, id, 0, "", Notes);
        Utils.Success("加入游戏", "加入游戏成功..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""), "1");
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
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定退出游戏吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?info=ok&amp;act=exit&amp;id=" + id + "&amp;tm=" + tm + "") + "\">确定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Game.Stone().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            DataSet ds = new BCW.BLL.Game.Stone().GetList("OneUsId,TwoUsId,ThrUsId,OneUsName,TwoUsName,ThrUsName,OneShot,TwoShot,ThrShot,PayCent,OneStat,TwoStat,ThrStat", "ID=" + id + " and (OneUsId=" + meid + " OR TwoUsId=" + meid + " OR ThrUsId=" + meid + ")");
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Utils.Error("你已经退出游戏", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
            }
            else
            {
                int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
                int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
                int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());
                string oneUsName = ds.Tables[0].Rows[0]["OneUsName"].ToString();
                string twoUsName = ds.Tables[0].Rows[0]["TwoUsName"].ToString();
                string thrUsName = ds.Tables[0].Rows[0]["ThrUsName"].ToString();

                int oneShot = int.Parse(ds.Tables[0].Rows[0]["OneShot"].ToString());
                int twoShot = int.Parse(ds.Tables[0].Rows[0]["TwoShot"].ToString());
                int thrShot = int.Parse(ds.Tables[0].Rows[0]["ThrShot"].ToString());
                int payCent = int.Parse(ds.Tables[0].Rows[0]["PayCent"].ToString());
                int oneStat = int.Parse(ds.Tables[0].Rows[0]["OneStat"].ToString());
                int twoStat = int.Parse(ds.Tables[0].Rows[0]["TwoStat"].ToString());
                int thrStat = int.Parse(ds.Tables[0].Rows[0]["ThrStat"].ToString());
                if (oneUsId == meid)
                {
                    new BCW.BLL.Game.Stone().UpdateOneExit(id);
                    if (oneStat != 0)
                    {
                        //币归2号
                        if (oneStat != 0 && twoStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //币归3号
                        else if (oneStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //先扣币
                        new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, -Convert.ToInt64(payCent), 1);
                        //重置桌子与停止PK
                        new BCW.BLL.Game.Stone().UpdateShot(id);
                        new BCW.BLL.Game.Stone().UpdateStat(id);
                    }
                    //如果已经出手，则重新退币
                    if (oneShot != 0)
                    {
                        new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, Convert.ToInt64(payCent), 1);
                    }
                }
                else if (twoUsId == meid)
                {
                    new BCW.BLL.Game.Stone().UpdateTwoExit(id);
                    if (twoStat != 0)
                    {
                        //币归1号
                        if (oneStat != 0 && twoStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //币归3号
                        else if (twoStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //先扣币
                        new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, -Convert.ToInt64(payCent), 1);
                        //重置桌子与停止PK
                        new BCW.BLL.Game.Stone().UpdateShot(id);
                        new BCW.BLL.Game.Stone().UpdateStat(id);
                    }
                    //如果已经出手，则重新退币
                    if (twoShot != 0)
                    {
                        new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, Convert.ToInt64(payCent), 1);
                    }
                }
                else if (thrUsId == meid)
                {
                    new BCW.BLL.Game.Stone().UpdateThrExit(id);
                    if (thrStat != 0)
                    {
                        //币归1号
                        if (oneStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //币归2号
                        else if (twoStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //先扣币
                        new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, -Convert.ToInt64(payCent), 1);
                        //重置桌子与停止PK
                        new BCW.BLL.Game.Stone().UpdateShot(id);
                        new BCW.BLL.Game.Stone().UpdateStat(id);
                    }
                    //如果已经出手，则重新退币
                    if (thrShot != 0)
                    {
                        new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, Convert.ToInt64(payCent), 1);
                    }
                }
            }

            string mename = new BCW.BLL.User().GetUsName(meid);
            string Notes = "" + mename + "退出了游戏";
            new BCW.BLL.Action().Add(1, id, 0, "", Notes);
            Utils.Success("退出游戏", "退出游戏成功..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""), "1");
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
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将其踢出游戏吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?info=ok&amp;act=kick&amp;id=" + id + "&amp;uid=" + uid + "&amp;tm=" + tm + "") + "\">确定</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {

            if (!new BCW.BLL.Game.Stone().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            DataSet ds = new BCW.BLL.Game.Stone().GetList("OneUsId,TwoUsId,ThrUsId,OneUsName,TwoUsName,ThrUsName,OneShot,TwoShot,ThrShot,Expir,OneTime,TwoTime,ThrTime,PayCent,Types,OneStat,TwoStat,ThrStat", "ID=" + id + " and (OneUsId=" + uid + " OR TwoUsId=" + uid + " OR ThrUsId=" + uid + ")");
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Utils.Error("TA已经退出游戏", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
            }
            else
            {
                int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
                int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
                int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());
                string oneUsName = ds.Tables[0].Rows[0]["OneUsName"].ToString();
                string twoUsName = ds.Tables[0].Rows[0]["TwoUsName"].ToString();
                string thrUsName = ds.Tables[0].Rows[0]["ThrUsName"].ToString();

                int oneShot = int.Parse(ds.Tables[0].Rows[0]["OneShot"].ToString());
                int twoShot = int.Parse(ds.Tables[0].Rows[0]["TwoShot"].ToString());
                int thrShot = int.Parse(ds.Tables[0].Rows[0]["ThrShot"].ToString());

                //超时时间
                int ExpirTime = int.Parse(ds.Tables[0].Rows[0]["Expir"].ToString());

                DateTime oneTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["OneTime"]);
                DateTime twoTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["TwoTime"]);
                DateTime thrTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["ThrTime"]);

                int payCent = int.Parse(ds.Tables[0].Rows[0]["PayCent"].ToString());
                int Types = int.Parse(ds.Tables[0].Rows[0]["Types"].ToString());
                int oneStat = int.Parse(ds.Tables[0].Rows[0]["OneStat"].ToString());
                int twoStat = int.Parse(ds.Tables[0].Rows[0]["TwoStat"].ToString());
                int thrStat = int.Parse(ds.Tables[0].Rows[0]["ThrStat"].ToString());
                //是否已全坐下位置
                bool IsMr = false;
                if (Types == 1)
                {
                    if (oneUsId != 0 && twoUsId != 0)
                        IsMr = true;
                }
                else if (Types == 2 || Types == 3)
                {
                    if (oneUsId != 0 && twoUsId != 0 && thrUsId != 0)
                    {
                        IsMr = true;
                    }
                }
                if (IsMr == false)
                {
                    Utils.Error("游戏还没有开始呢..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
                }

                if (oneUsId == uid)
                {
                    //如果未超时
                    if (DT.DateDiff(DateTime.Now, oneTime, 4) < ExpirTime)
                    {
                        Utils.Error("还未超时，不能踢出..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
                    }

                    new BCW.BLL.Game.Stone().UpdateOneExit(id);
                    if (oneStat != 0)
                    {
                        //币归2号
                        if (oneStat != 0 && twoStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //币归3号
                        else if (oneStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //先扣币
                        new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, -Convert.ToInt64(payCent), 1);
                        //重置桌子与停止PK
                        new BCW.BLL.Game.Stone().UpdateShot(id);
                        new BCW.BLL.Game.Stone().UpdateStat(id);
                    }
                }
                else if (twoUsId == uid)
                {
                    //如果未超时
                    if (DT.DateDiff(DateTime.Now, twoTime, 4) < ExpirTime)
                    {
                        Utils.Error("还未超时，不能踢出..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
                    }
                    new BCW.BLL.Game.Stone().UpdateTwoExit(id);
                    if (twoStat != 0)
                    {
                        //币归1号
                        if (oneStat != 0 && twoStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //币归3号
                        else if (twoStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //先扣币
                        new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, -Convert.ToInt64(payCent), 1);
                        //重置桌子与停止PK
                        new BCW.BLL.Game.Stone().UpdateShot(id);
                        new BCW.BLL.Game.Stone().UpdateStat(id);
                    }
                }
                else if (thrUsId == uid)
                {
                    //如果未超时
                    if (DT.DateDiff(DateTime.Now, oneTime, 4) < ExpirTime)
                    {
                        Utils.Error("还未超时，不能踢出..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
                    }
                    new BCW.BLL.Game.Stone().UpdateThrExit(id);
                    if (thrStat != 0)
                    {
                        //币归1号
                        if (oneStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(oneUsId, oneUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //币归2号
                        else if (twoStat != 0 && thrStat != 0)
                        {
                            new BCW.BLL.User().UpdateiGold(twoUsId, twoUsName, Convert.ToInt64(payCent - payCent * GetTax()), 1);
                        }
                        //先扣币
                        new BCW.BLL.User().UpdateiGold(thrUsId, thrUsName, -Convert.ToInt64(payCent), 1);
                        //重置桌子与停止PK
                        new BCW.BLL.Game.Stone().UpdateShot(id);
                        new BCW.BLL.Game.Stone().UpdateStat(id);
                    }
                }
            }

            string mename = new BCW.BLL.User().GetUsName(uid);
            string Notes = "" + mename + "被踢出了游戏";
            new BCW.BLL.Action().Add(1, id, 0, "", Notes);
            Utils.Success("踢出游戏", "踢出" + mename + "成功..", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""), "1");
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
        int st = 0;
        if (IsStone(meid))
            st = int.Parse(Utils.GetRequest("st", "post", 2, @"^[1-6]$", "出手错了"));
        else
            st = int.Parse(Utils.GetRequest("st", "post", 2, @"^[1-3]$", "出手错了"));

        int payCent = int.Parse(Utils.GetRequest("payCent", "post", 1, @"^[1-9]\d*$", "0"));
        int Num = int.Parse(Utils.GetRequest("Num", "post", 2, @"^[1-3]$", "出手错了"));
        if (!new BCW.BLL.Game.Stone().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.Stone model = new BCW.BLL.Game.Stone().GetStone(id);
        int Pn = 0;
        if (model.Types != 4)
        {

            //是否已经坐下,返回坐下的位置值
            DataSet ds = new BCW.BLL.Game.Stone().GetList("OneUsId,TwoUsId,ThrUsId,IsStatus,OneStat,TwoStat,ThrStat", "ID=" + id + " and (OneUsId=" + meid + " OR TwoUsId=" + meid + " OR ThrUsId=" + meid + ")");
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Utils.Error("您还没坐下..", "");
            }
          
            int oneUsId = int.Parse(ds.Tables[0].Rows[0]["OneUsId"].ToString());
            int twoUsId = int.Parse(ds.Tables[0].Rows[0]["TwoUsId"].ToString());
            int thrUsId = int.Parse(ds.Tables[0].Rows[0]["ThrUsId"].ToString());

            int isstatus = int.Parse(ds.Tables[0].Rows[0]["IsStatus"].ToString());
            int oneStat = int.Parse(ds.Tables[0].Rows[0]["OneStat"].ToString());
            int twoStat = int.Parse(ds.Tables[0].Rows[0]["TwoStat"].ToString());
            int thrStat = int.Parse(ds.Tables[0].Rows[0]["ThrStat"].ToString());

            if (oneUsId == meid)
            {
                Pn = 1;
                if (isstatus != 0)
                {
                    if (oneStat == 0)
                    {
                        Utils.Error("别人PK中，请你稍后..", "");
                    }
                }
            }
            else if (twoUsId == meid)
            {
                Pn = 2;
                if (isstatus != 0)
                {
                    if (twoStat == 0)
                    {
                        Utils.Error("别人PK中，请你稍后..", "");
                    }
                }
            }
            else
            {
                Pn = 3;
                if (isstatus != 0)
                {
                    if (thrStat == 0)
                    {
                        Utils.Error("别人PK中，请你稍后..", "");
                    }
                }
            }
        }
        //第几次出手
        int iNum = 1;
        if (model.OneShot != 0)
            iNum++;
        if (model.TwoShot != 0)
            iNum++;
        if (model.ThrShot != 0)
            iNum++;

        //轮流出手限制
        if (iNum == 1)
        {
            if (model.ShotTypes == 1)
            {
                if (model.NextShot != Pn)
                {
                    string NextName = string.Empty;
                    if (model.NextShot == 1)
                        NextName = model.OneUsName;
                    else if (model.NextShot == 2)
                        NextName = model.TwoUsName;
                    else
                        NextName = model.ThrUsName;

                    Utils.Error("请等待" + NextName + "出手，大家轮流出手嘛", "");
                }
            }
        }
        //如果此顺序(Num)已有人出手
        if (Num != iNum)
        {
            Utils.Error("您慢了半拍，已经有人抢先出手了", Utils.getUrl("stone.aspx?act=view&amp;id=" + id + ""));
        }

        //如果已经出手
        if (new BCW.BLL.Game.Stone().Exists(id, meid, Pn))
        {
            Utils.Error("您已出手，不用重复出手", "");
        }

        if (iNum == 1)
        {
            if (payCent < model.SmallPay || payCent > model.BigPay)
            {
                Utils.Error("出手限" + model.SmallPay + "-" + model.BigPay + "" + ub.Get("SiteBz") + "", "");
            }
        }
        //扣币
        int Cent = 0;
        if (payCent == 0)
            Cent = model.PayCent;
        else
            Cent = payCent;
        //用户币数
        long Gold = new BCW.BLL.User().GetGold(meid);
        if (Gold < Convert.ToInt64(Cent))
        {
            Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
        }
        //支付安全提示
        string[] p_pageArr = { "act", "id", "tm", "st", "PayCent", "Num" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

        //扣币
        new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(Cent), 1);

        //写入位置
        BCW.Model.Game.Stone addmodel = new BCW.Model.Game.Stone();
        if (model.Types != 4)
        {
            if (Pn == 1)
            {
                addmodel.ID = id;
                addmodel.OneShot = st;
                new BCW.BLL.Game.Stone().UpdateOneShot(addmodel);
            }
            else if (Pn == 2)
            {
                addmodel.ID = id;
                addmodel.TwoShot = st;
                new BCW.BLL.Game.Stone().UpdateTwoShot(addmodel);
            }
            else
            {
                addmodel.ID = id;
                addmodel.ThrShot = st;
                new BCW.BLL.Game.Stone().UpdateThrShot(addmodel);
            }
        }
        else
        { 
        //混战
            if (iNum == 1)
            {
                addmodel.ID = id;
                addmodel.OneUsId = meid;
                addmodel.OneUsName = mename;
                addmodel.OneShot = st;
                new BCW.BLL.Game.Stone().UpdateOneShot2(addmodel);
            }
            else
            {
                if (st > 3)
                {
                    Utils.Error("出手错了", "");
                }
                addmodel.ID = id;
                addmodel.TwoUsId = meid;
                addmodel.TwoUsName = mename;
                //管理员出手特别处理
                int newst = 1;
                if (model.OneShot == 4)//赢
                {
                    if (st == 1)
                        newst = 2;
                    else if (st == 2)
                        newst = 3;
                    else if (st == 3)
                        newst = 1;
                }
                else if (model.OneShot == 5)//平
                {
                    if (st == 1)
                        newst = 1;
                    else if (st == 2)
                        newst = 2;
                    else if (st == 3)
                        newst = 3;
                }
                else if (model.OneShot == 6)//输
                {
                    if (st == 1)
                        newst = 3;
                    else if (st == 2)
                        newst = 1;
                    else if (st == 3)
                        newst = 2;
                }
                if (model.OneShot > 3)
                {
                    BCW.Data.SqlHelper.ExecuteSql("update tb_Stone set OneShot=" + newst + " where id=" + id + "");
                }
                
                addmodel.TwoShot = st;
                new BCW.BLL.Game.Stone().UpdateTwoShot2(addmodel);
            }

        }
        //更新下注额
        if (iNum == 1 && payCent != 0)
        {
            new BCW.BLL.Game.Stone().UpdatePayCent(id, payCent);
            //如果是轮流出手，则决定下一轮谁出手,按1、2、3排队
            if (model.ShotTypes == 1)
            {
                int iNext = model.NextShot + 1;
                if (model.Types == 1)
                {
                    if (iNext > 2)
                        iNext = 1;
                }
                else if (model.Types == 2 || (model.Types == 3 && model.IsStatus == 0))
                {
                    if (iNext > 3)
                        iNext = 1;
                }
                else
                {
                    if (iNext > 2)
                        iNext = 1;
                }
                new BCW.BLL.Game.Stone().UpdateNextShot(id, iNext);
                //同时为后面桌位更新超时时间
                new BCW.BLL.Game.Stone().UpdateTime(id);
            }
        }


        string winText = string.Empty;

        //如果满场则开奖,先读取实体
        BCW.Model.Game.Stone remodel = new BCW.BLL.Game.Stone().GetStone(id);
        int win = 0;
        string forname = string.Empty;
        int a = remodel.OneShot;
        int b = remodel.TwoShot;
        int c = remodel.ThrShot;

        //两人对决开奖
        if (model.Types == 1|| model.Types==4)
        {
            if (a != 0 && b != 0) //满足条件
            {
                if (a == b|| a.Equals(b))
                {
                    win = 2;//平局
                    //双方返币
                    new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent), 1);
                    winText = "好险，大家都是出" + GetStName(remodel.OneShot) + ",平局";
                }
                else if ((a == 2 && b == 1) || (a == 1 && b == 3) || (a == 3 && b == 2))
                {
                    if (remodel.OneUsId == meid)
                    {
                        win = 1; //赢了
                        forname = remodel.OneUsName;
                        new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                        winText = "恭喜！你出" + GetStName(st) + "战胜了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    else
                    {
                        win = 3;//输了
                        forname = remodel.TwoUsName;
                        new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                        winText = "遗憾！你出" + GetStName(st) + "负了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                }
                else
                {
                    if (remodel.OneUsId == meid)
                    {
                        win = 3;//输了
                        forname = remodel.OneUsName;
                        new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                        winText = "遗憾！你出" + GetStName(st) + "负了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    else
                    {
                        win = 1;//赢了
                        forname = remodel.TwoUsName;
                        new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                        winText = "恭喜！你出" + GetStName(st) + "战胜了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")，你赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                }
                //重置桌子,如果是混战则清空桌面
                if (model.Types == 4)
                    new BCW.BLL.Game.Stone().UpdateClear(id);
                else
                    new BCW.BLL.Game.Stone().UpdateShot(id);
                //记录系统消息
                string wText = string.Empty;
                if (win == 1)
                    wText = "" + forname + "赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                else if (win == 2)
                    wText = "平局";
                else
                    wText = "" + forname + "输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";

                string Notes = "" + remodel.OneUsName + "[" + GetStName(remodel.OneShot) + "VS" + GetStName(remodel.TwoShot) + "]" + remodel.TwoUsName + "," + wText + "";
                new BCW.BLL.Action().Add(1, id, 0, "", Notes);
            }
        }
            
        //三人对决开奖
        else if (remodel.Types == 2 || (remodel.Types == 3 && remodel.IsStatus == 0))
        {
            if (a != 0 && b != 0 && c != 0) //满足条件
            {
                if (a == b && b == c)
                {
                    win = 2;//平局
                    //三方返币
                    new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent), 1);
                    winText = "好险，大家都是出" + GetStName(remodel.OneShot) + ",平局";
                }
                //a全赢
                else if ((a == 2 && b == 1 && c == 1) || (a == 1 && b == 3 && c == 3) || (a == 3 && b == 2 && c == 2))
                {
                    new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent * 3 - (remodel.PayCent*2) * GetTax()), 1);
                    if (remodel.OneUsId == meid)
                    {
                        win = 6; //双赢了
                        forname = remodel.OneUsName;
                        winText = "恭喜！你出" + GetStName(st) + "战胜了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")与" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.TwoUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.TwoUsName;
                        winText = "遗憾！你出" + GetStName(st) + "与" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")都负了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.ThrUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.ThrUsName;
                        winText = "遗憾！你出" + GetStName(st) + "与" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")都负了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                }
                //b全赢
                else if ((b == 2 && a == 1 && c == 1) || (b == 1 && a == 3 && c == 3) || (b == 3 && a == 2 && c == 2))
                {
                    new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent * 3 - (remodel.PayCent*2) * GetTax()), 1);
                    if (remodel.TwoUsId == meid)
                    {
                        win = 6; //双赢了
                        forname = remodel.TwoUsName;
                        winText = "恭喜！你出" + GetStName(st) + "战胜了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")与" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.OneUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.OneUsName;
                        winText = "遗憾！你出" + GetStName(st) + "与" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")都负了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.ThrUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.ThrUsName;
                        winText = "遗憾！你出" + GetStName(st) + "与" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")都负了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                }
                //c全赢
                else if ((c == 2 && a == 1 && b == 1) || (c == 1 && a == 3 && b == 3) || (c == 3 && a == 2 && b == 2))
                {
                    new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent * 3 - (remodel.PayCent*2) * GetTax()), 1);
                    if (remodel.ThrUsId == meid)
                    {
                        win = 6; //双赢了
                        forname = remodel.ThrUsName;
                        winText = "恭喜！你出" + GetStName(st) + "战胜了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")与" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.OneUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.OneUsName;
                        winText = "遗憾！你出" + GetStName(st) + "与" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")都负了" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.TwoUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.TwoUsName;
                        winText = "遗憾！你出" + GetStName(st) + "与" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")都负了" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                }
                //a与b赢c
                else if ((a == 2 && b == 2 && c == 1) || (a == 1 && b == 1 && c == 3) || (a == 3 && b == 3 && c == 2))
                {
                    new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + remodel.PayCent * 0.5), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + remodel.PayCent * 0.5), 1);

                    if (remodel.OneUsId == meid)
                    {
                        win = 5; //赢半
                        forname = remodel.OneUsName + "与" + remodel.TwoUsName;
                        winText = "恭喜！你与" + remodel.TwoUsName + "出" + GetStName(st) + "战胜了" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.TwoUsId == meid)
                    {
                        win = 5; //赢半
                        forname = remodel.OneUsName + "与" + remodel.TwoUsName;
                        winText = "恭喜！你与" + remodel.OneUsName + "出" + GetStName(st) + "战胜了" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.ThrUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.ThrUsName;
                        winText = "遗憾！你出" + GetStName(st) + "负了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")与" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }

                    //a、b继续战斗标识
                    if (remodel.Types == 3)
                    {
                        new BCW.BLL.Game.Stone().UpdatePKab(id);
                    }
                }

                //b与c赢a
                else if ((b == 2 && a == 1 && c == 2) || (b == 1 && a == 3 && c == 1) || (b == 3 && a == 2 && c == 3))
                {
                    new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent + remodel.PayCent * 0.5), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + remodel.PayCent * 0.5), 1);

                    if (remodel.TwoUsId == meid)
                    {
                        win = 5; //赢半
                        forname = remodel.TwoUsName + "与" + remodel.ThrUsName;
                        winText = "恭喜！你与" + remodel.ThrUsName + "出" + GetStName(st) + "战胜了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")，你赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.ThrUsId == meid)
                    {
                        win = 5; //赢半
                        forname = remodel.TwoUsName + "与" + remodel.ThrUsName;
                        winText = "恭喜！你与" + remodel.TwoUsName + "出" + GetStName(st) + "战胜了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")，你赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.OneUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.OneUsName;
                        winText = "遗憾！你出" + GetStName(st) + "负了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")与" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    //b、c继续战斗标识
                    if (remodel.Types == 3)
                    {
                        new BCW.BLL.Game.Stone().UpdatePKbc(id);
                    }
                }

                //c与a赢b
                else if ((c == 2 && a == 2 && b == 1) || (c == 1 && a == 1 && b == 3) || (c == 3 && a == 3 && b == 2))
                {
                    new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent + remodel.PayCent * 0.5), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent + remodel.PayCent * 0.5), 1);

                    if (remodel.OneUsId == meid)
                    {
                        win = 5; //赢半
                        forname = remodel.OneUsName + "与" + remodel.ThrUsName;
                        winText = "恭喜！你与" + remodel.ThrUsName + "出" + GetStName(st) + "战胜了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.ThrUsId == meid)
                    {
                        win = 5; //赢半
                        forname = remodel.OneUsName + "与" + remodel.ThrUsName;
                        winText = "恭喜！你与" + remodel.OneUsName + "出" + GetStName(st) + "战胜了" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")，你赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                    }
                    else if (remodel.TwoUsId == meid)
                    {
                        win = 3; //输了
                        forname = remodel.OneUsName;
                        winText = "遗憾！你出" + GetStName(st) + "负了" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")与" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    }
                    //a、c继续战斗标识
                    if (remodel.Types == 3)
                    {
                        new BCW.BLL.Game.Stone().UpdatePKac(id);
                    }
                }

                else//出手剪刀石头布全有了！
                {
                    win = 4;//三分天下
                    //三方返币
                    new BCW.BLL.User().UpdateiGold(remodel.OneUsId, remodel.OneUsName, Convert.ToInt64(remodel.PayCent), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.TwoUsId, remodel.TwoUsName, Convert.ToInt64(remodel.PayCent), 1);
                    new BCW.BLL.User().UpdateiGold(remodel.ThrUsId, remodel.ThrUsName, Convert.ToInt64(remodel.PayCent), 1);
                    winText = "真是未出茅庐而定天下三分！剪刀、石头、布全有了，此局平局";
                }

                //重置桌子
                new BCW.BLL.Game.Stone().UpdateShot(id);
                //记录系统消息
                string wText = string.Empty;
                if (win == 1)
                    wText = "" + forname + "赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                else if (win == 2)
                    wText = "平局";
                else if (win == 3)
                    wText = "" + forname + "输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                else if (win == 4)
                    wText = "三分天下，平局";
                else if (win == 5)
                    wText = "" + forname + "赢了" + (remodel.PayCent * 0.5) + "" + ub.Get("SiteBz") + "";
                else if (win == 6)
                    wText = "" + forname + "赢了" + (remodel.PayCent * 2) + "" + ub.Get("SiteBz") + "";

                string Notes = "" + remodel.OneUsName + "(" + GetStName(remodel.OneShot) + ")VS" + remodel.TwoUsName + "(" + GetStName(remodel.TwoShot) + ")VS" + remodel.ThrUsName + "(" + GetStName(remodel.ThrShot) + ")," + wText + "";
                new BCW.BLL.Action().Add(1, id, 0, "", Notes);
            }
        }
        else
        {
            //2人PK
            if (remodel.IsStatus == 1)
            {
                string oneName = string.Empty;
                string twoName = string.Empty;
                int one = 0;
                int two = 0;
                int oneusid = 0;
                int twousid = 0;
                bool PK = false;
                if (remodel.OneShot != 0 && remodel.TwoShot != 0)
                {
                    one = remodel.OneShot;
                    two = remodel.TwoShot;
                    oneName = remodel.OneUsName;
                    twoName = remodel.TwoUsName;
                    oneusid = remodel.OneUsId;
                    twousid = remodel.TwoUsId;
                    PK = true;
                }
                else if (remodel.TwoShot != 0 && remodel.ThrShot != 0)
                {
                    one = remodel.TwoShot;
                    two = remodel.ThrShot;
                    oneName = remodel.TwoUsName;
                    twoName = remodel.ThrUsName;
                    oneusid = remodel.TwoUsId;
                    twousid = remodel.ThrUsId;
                    PK = true;
                }

                else if (remodel.OneShot != 0 && remodel.ThrShot != 0)
                {
                    one = remodel.OneShot;
                    two = remodel.ThrShot;
                    oneName = remodel.OneUsName;
                    twoName = remodel.ThrUsName;
                    oneusid = remodel.OneUsId;
                    twousid = remodel.ThrUsId;
                    PK = true;
                }
                if (PK == true)
                {
                    if (one == two || one.Equals(two))
                    {
                        win = 2;//平局
                        //双方返币
                        new BCW.BLL.User().UpdateiGold(oneusid, oneName, Convert.ToInt64(remodel.PayCent), 1);
                        new BCW.BLL.User().UpdateiGold(twousid, twoName, Convert.ToInt64(remodel.PayCent), 1);
                        winText = "惊险，大家都是出" + GetStName(one) + ",决战平局,马上进新决战！";
                    }
                    else if ((one == 2 && two == 1) || (one == 1 && two == 3) || (one == 3 && two == 2))
                    {
                        if (oneusid == meid)
                        {
                            win = 1; //赢了
                            forname = oneName;
                            new BCW.BLL.User().UpdateiGold(oneusid, oneName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                            winText = "恭喜！你出" + GetStName(st) + "战胜了" + twoName + "(" + GetStName(two) + ")，你赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                        }
                        else
                        {
                            win = 3;//输了
                            forname = twoName;
                            new BCW.BLL.User().UpdateiGold(twousid, twoName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                            winText = "遗憾！你出" + GetStName(st) + "负了" + oneName + "(" + GetStName(one) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                        }
                    }
                    else
                    {
                        if (one == meid)
                        {
                            win = 3;//输了
                            forname = oneName;
                            new BCW.BLL.User().UpdateiGold(oneusid, oneName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                            winText = "遗憾！你出" + GetStName(st) + "负了" + twoName + "(" + GetStName(two) + ")，你输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                        }
                        else
                        {
                            win = 1;//赢了
                            forname = twoName;
                            new BCW.BLL.User().UpdateiGold(twousid, twoName, Convert.ToInt64(remodel.PayCent * 2 - remodel.PayCent * GetTax()), 1);
                            winText = "恭喜！你出" + GetStName(st) + "战胜了" + oneName + "(" + GetStName(one) + ")，你赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                        }
                    }

                    //如果不平局则停止PK
                    if (win != 2)
                        new BCW.BLL.Game.Stone().UpdateStat(id);

                    //重置桌子
                    new BCW.BLL.Game.Stone().UpdateShot(id);

                    //记录系统消息
                    string wText = string.Empty;
                    if (win == 1)
                        wText = "" + forname + "赢了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";
                    else if (win == 2)
                        wText = "决战平局";
                    else
                        wText = "" + forname + "输了" + remodel.PayCent + "" + ub.Get("SiteBz") + "";

                    string Notes = "决战:" + oneName + "[" + GetStName(one) + "VS" + GetStName(two) + "]" + twoName + "," + wText + "";
                    new BCW.BLL.Action().Add(1, id, 0, "", Notes);
                }
            }
        }
        if (string.IsNullOrEmpty(winText))
        {
            winText = "" + GetStName(st) + "出手成功，等待对方应战！";
            string Notes2 = "" + mename + "下注" + remodel.PayCent + "" + ub.Get("SiteBz") + "出手";
            if (model.Types == 4)
                new BCW.BLL.Action().Add(1, id, meid, "混战", Notes2);
            else
                new BCW.BLL.Action().Add(1, id, meid, "第" + model.PkCount + "局", Notes2);
        }

        Utils.Success("" + GetStName(st) + "出手", winText, Utils.getUrl("stone.aspx?act=view&amp;id=" + id + "&amp;tm=" + tm + ""), "3");
    }

    private bool IsStone(int uid)
    {
        if (Utils.GetTopDomain().Equals("qyh.cc"))
        {
            string CheatID = "#" + ub.GetSub("StoneCheatID", xmlPath) + "#";
            if (CheatID.Contains("#" + uid + "#"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}