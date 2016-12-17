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
using BCW.Data;
public partial class bbs_game_free : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/free.xml";
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
        if (ub.GetSub("FreeStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":
                RulePage();
                break;
            case "rule2":
                Rule2Page();
                break;
            case "top":
                TopPage();
                break;
            case "me":
                MePage();
                break;
            case "more":
                MorePage();
                break;
            case "myck":
                MyckPage();
                break;
            case "zlist":
                ZlistPage();
                break;
            case "klist":
                KlistPage();
                break;
            case "myview":
                MyViewPage();
                break;
            case "myview2":
                MyView2Page();
                break;
            case "mlist":
                MlistPage();
                break;
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "add":
                AddPage();
                break;
            case "save1":
                Save1Page();
                break;
            case "save2":
                Save2Page();
                break;
            case "save3":
                Save3Page();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        new BCW.User.Free().FreePage();
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("FreeName", xmlPath);
        string Logo = ub.GetSub("FreeLogo", xmlPath);
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
        string Notes = ub.GetSub("FreeNotes", xmlPath);
        if (Notes != "")
            builder.Append(Notes + "<br />");

        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=add") + "\">当猜主开盘</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=me") + "\">我的猜猜</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=myck") + "\">我的下线(猜客)</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【最新猜猜】<a href=\"" + Utils.getUrl("free.aspx") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        IList<BCW.Model.Game.Free> listFree = new BCW.BLL.Game.Free().GetFrees(8, "state=0");
        if (listFree.Count > 0)
        {
            foreach (BCW.Model.Game.Free n in listFree)
            {
                builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + n.Price + "金)</a><br />");
            }
        }
        else
        {
            builder.Append("暂时无相关记录..<br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看更多猜猜</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【历史猜猜】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;ptype=1") + "\">足球</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;ptype=2") + "\">篮球</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;ptype=3") + "\">彩票</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;ptype=4") + "\">股票</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;ptype=5") + "\">体育综合</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=more&amp;ptype=6") + "\">日常生活</a>");
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(3, "free.aspx", 5, 0)));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=top") + "\">猜猜排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=rule") + "\">游戏规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string OpenId = ub.GetSub("FreeOpenId", xmlPath);
        if (OpenId != "")
        {
            if (!("#" + OpenId + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你还没有开盘的权限，开盘权限需要向管理员申请..", "");
            }
        }

        Master.Title = "我要开盘";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜猜乐开盘");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", "<br />"));
        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append("您目前自带" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));
        strText = "猜猜题目(20字内):/,输入详细描述(500字内):/,猜猜单份价格(" + ub.Get("SiteBz") + "):/,猜猜选项数:(最多" + ub.GetSub("FreeMaxNum", xmlPath) + "项)/,";
        strName = "Title,Content,Price,Num,act";
        strType = "text,textarea,num,num,hidden";
        strValu = "''100''save1";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "下一步,free.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=rule2") + "\">查看开盘示例</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Save1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "标题限1-20字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "内容限1-500字");
        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "单份价格填写错误"));
        int Num = int.Parse(Utils.GetRequest("Num", "post", 2, @"^[1-9]\d*$", "竞猜选项数填写错误"));

        //单份价格最小与最大限制
        if (Price < Utils.ParseInt(ub.GetSub("FreeSmallPay", xmlPath)) || Price > Utils.ParseInt(ub.GetSub("FreeBigPay", xmlPath)))
        {
            Utils.Error("单份价格限" + ub.GetSub("FreeSmallPay", xmlPath) + "至" + ub.GetSub("FreeBigPay", xmlPath) + "" + ub.Get("SiteBz") + "", "");
        }
        if (Num > Utils.ParseInt(ub.GetSub("FreeMaxNum", xmlPath)))
        {
            Utils.Error("竞猜选项数不能大于" + ub.GetSub("FreeMaxNum", xmlPath) + "", "");
        }
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        for (int i = 1; i <= Num; i++)
        {
            sText += "第" + i + "项盘口:,出售多少份:,盘口赔率:,";
            sName += "Name" + i + ",Count" + i + ",Odds" + i + ",";
            sType += "text,snum,stext,";
            sValu += "'10'1.90'";
            sEmpt += "false,false,false,";
        }
        Master.Title = "我要开盘";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜猜乐开盘");
        builder.Append(Out.Tab("</div>", ""));

        strText = sText + "输入竞猜购买截止时间:/,输入竞猜承诺开奖返彩时间:/,,,,,";
        strName = sName + "CloseTime,OpenTime,Title,Content,Price,Num,act";
        strType = sType + "date,date,hidden,hidden,hidden,hidden,hidden";
        strValu = sValu + "" + DT.FormatDate(DateTime.Now.AddHours(1), 0) + "'" + DT.FormatDate(DateTime.Now.AddHours(3), 0) + "'" + Title + "'" + Content + "'" + Price + "'" + Num + "'save2";
        strEmpt = sEmpt + "false,false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "提交预览,free.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:赔率最多支持保留三位小数,同时也支持整数");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=add") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Save2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "标题限1-20字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "内容限1-500字");
        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "单份价格填写错误"));
        int Num = int.Parse(Utils.GetRequest("Num", "post", 2, @"^[1-9]\d*$", "竞猜选项数填写错误"));
        DateTime CloseTime = Utils.ParseTime(Utils.GetRequest("CloseTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime OpenTime = Utils.ParseTime(Utils.GetRequest("OpenTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        //单份价格最小与最大限制
        if (Price < Utils.ParseInt(ub.GetSub("FreeSmallPay", xmlPath)) || Price > Utils.ParseInt(ub.GetSub("FreeBigPay", xmlPath)))
        {
            Utils.Error("单份价格限" + ub.GetSub("FreeSmallPay", xmlPath) + "至" + ub.GetSub("FreeBigPay", xmlPath) + "" + ub.Get("SiteBz") + "", "");
        }
        if (Num > Utils.ParseInt(ub.GetSub("FreeMaxNum", xmlPath)))
        {
            Utils.Error("竞猜选项数不能大于" + ub.GetSub("FreeMaxNum", xmlPath) + "", "");
        }
        if (CloseTime > OpenTime)
        {
            Utils.Error("开奖时间必须大于截止时间", "");
        }
        string pText = "";
        string pCount = "";
        string pOdds = "";
        for (int i = 1; i <= Num; i++)
        {
            string Name = Utils.GetRequest("Name" + i + "", "post", 2, @"^[\s\S]{1,20}$", "选项名称" + i + "填写错误");
            double Odds = Convert.ToDouble(Utils.GetRequest("Odds" + i + "", "post", 2, @"^(\d)*(\.(\d){0,3})?$", "第" + i + "选项赔率填写错误"));
            int Count = int.Parse(Utils.GetRequest("Count" + i + "", "post", 2, @"^[0-9]\d*$", "第" + i + "选项卖出份数填写错误"));

            if (Odds < 1.20 || Odds > 100.00)
            {
                Utils.Error("竞猜赔率不能小于1.20,不能大于100.00", "");
            }
            pText += "," + Name;
            pCount += "," + Count;
            pOdds += "," + Odds;
        }
        pText = Utils.Mid(pText, 1, pText.Length);
        pCount = Utils.Mid(pCount, 1, pCount.Length);
        pOdds = Utils.Mid(pOdds, 1, pOdds.Length);
        //计算花销
        long gold = new BCW.BLL.User().GetGold(meid);
        string[] arrText = pText.Split(",".ToCharArray());
        string[] arrCount = pCount.Split(",".ToCharArray());
        string[] arrOdds = pOdds.Split(",".ToCharArray());
        long pCent = 0;
        int psellCount = 0;
        for (int i = 0; i < Num; i++)
        {
            pCent += Convert.ToInt64(Price * Convert.ToDouble(arrOdds[i].ToString()) - Price) * Convert.ToInt64(arrCount[i].ToString());
            psellCount += Convert.ToInt32(arrCount[i].ToString());
        }
        if (gold < pCent)
        {
            Utils.Error("您的" + ub.Get("SiteBz") + "不足本次开销", "");
        }
        Master.Title = "我要开盘";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜猜乐开盘");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("预览题目<br />");
        builder.Append("你的竞猜题目为:" + Title + "<br />");
        builder.Append("详细描述:" + Content + "<br />");
        builder.Append("购买选项如下:<br />");
        for (int i = 0; i < Num; i++)
        {
            builder.Append("第" + (i + 1) + "项:" + arrText[i] + ",卖" + arrCount[i] + "份,赔率:" + arrOdds[i] + "<br />");
        }
        builder.Append("每份单价为:" + Price + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("卖出总份数为" + psellCount + "份,总保证金" + pCent + "" + ub.Get("SiteBz") + ",您还剩余" + (gold - pCent) + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("竞猜开奖后,系统会自动返还没有卖出份数的保证金<br />");
        builder.Append("截止购买时间为:" + DT.FormatDate(CloseTime, 0) + "<br />");
        builder.Append("承诺开奖时间为:" + DT.FormatDate(OpenTime, 0) + "");
        builder.Append(Out.Tab("</div>", ""));
        strText = "最后请选择类别:/,,,,,,,,,,";
        strName = "CclType,CloseTime,OpenTime,Title,Content,Price,Num,pText,pCount,pOdds,act";
        strType = "select,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
        strValu = "1'" + DT.FormatDate(CloseTime, 0) + "'" + DT.FormatDate(OpenTime, 0) + "'" + Title + "'" + Content + "'" + Price + "'" + Num + "'" + pText + "'" + pCount + "'" + pOdds + "'save3";
        strEmpt = "1|足球|2|篮球|3|彩票|4|股票|6|体育综合|7|日常生活,false,false,false,false,false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "确认创建,free.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=add") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Save3Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string OpenId = ub.GetSub("FreeOpenId", xmlPath);
        if (OpenId != "")
        {
            if (!("#" + OpenId + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你还没有开盘的权限，开盘权限需要向管理员申请..", "");
            }
        }

        string mename = new BCW.BLL.User().GetUsName(meid);
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "标题限1-20字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "内容限1-500字");
        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "单份价格填写错误"));
        int Num = int.Parse(Utils.GetRequest("Num", "post", 2, @"^[1-9]\d*$", "竞猜选项数填写错误"));
        DateTime CloseTime = Utils.ParseTime(Utils.GetRequest("CloseTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime OpenTime = Utils.ParseTime(Utils.GetRequest("OpenTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        string pText = Utils.GetRequest("pText", "post", 2, @"^[\s\S]{1,800}$", "选项填写错误");
        string pCount = Utils.GetRequest("pCount", "post", 2, @"^[\s\S]{1,800}$", "选项填写错误");
        string pOdds = Utils.GetRequest("pOdds", "post", 2, @"^[\s\S]{1,800}$", "选项填写错误");
        int CclType = int.Parse(Utils.GetRequest("CclType", "post", 2, @"^[1-7]$", "竞猜类别选择错误"));
        //单份价格最小与最大限制
        if (Price < Utils.ParseInt(ub.GetSub("FreeSmallPay", xmlPath)) || Price > Utils.ParseInt(ub.GetSub("FreeBigPay", xmlPath)))
        {
            Utils.Error("单份价格限" + ub.GetSub("FreeSmallPay", xmlPath) + "至" + ub.GetSub("FreeBigPay", xmlPath) + "" + ub.Get("SiteBz") + "", "");
        }
        if (Num > Utils.ParseInt(ub.GetSub("FreeMaxNum", xmlPath)))
        {
            Utils.Error("竞猜选项数不能大于" + ub.GetSub("FreeMaxNum", xmlPath) + "", "");
        }
        if (CloseTime > OpenTime)
        {
            Utils.Error("开奖时间必须大于截止时间", "");
        }
        //计算花销
        long gold = new BCW.BLL.User().GetGold(meid);
        string[] arrText = pText.Split(",".ToCharArray());
        string[] arrCount = pCount.Split(",".ToCharArray());
        string[] arrOdds = pOdds.Split(",".ToCharArray());
        long pCent = 0;
        int psellCount = 0;
        string pCount2 = "";
        for (int i = 0; i < Num; i++)
        {
            pCent += Convert.ToInt64(Price * Convert.ToDouble(arrOdds[i].ToString()) - Price) * Convert.ToInt64(arrCount[i].ToString());
            psellCount += Convert.ToInt32(arrCount[i].ToString());
            pCount2 += ",0";
        }
        pCount2 = Utils.Mid(pCount2, 1, pCount2.Length);
        if (gold < pCent)
        {
            Utils.Error("您的" + ub.Get("SiteBz") + "不足本次开销", "");
        }

        //支付安全提示
        string[] p_pageArr = { "act", "Title", "Content", "Price", "Num", "CloseTime", "OpenTime", "pText", "pCount", "pOdds", "CclType" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

        BCW.Model.Game.Free model = new BCW.Model.Game.Free();
        model.UserID = meid;
        model.UserName = mename;
        model.Title = Title;
        model.Content = Content;
        model.pText = pText;
        model.pCount = pCount;
        model.pCount2 = pCount2;
        model.pOdds = pOdds;
        model.pNum = Num;
        model.Price = Price;
        model.Counts = psellCount;
        model.CloseTime = CloseTime;
        model.OpenTime = OpenTime;
        model.OpenTime2 = DateTime.Now;
        model.CclType = CclType;
        model.AddTime = DateTime.Now;
        model.cclcent = pCent;
        int id = new BCW.BLL.Game.Free().Add(model);
        //扣币
        new BCW.BLL.User().UpdateiGold(meid, mename, -pCent, 3);
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]创建猜猜[url=/bbs/game/free.aspx?act=view&amp;id=" + id + "]" + Title + "[/url]";
        new BCW.BLL.Action().Add(3, id, 0, "", wText);
        Utils.Success("创建猜猜", "创建猜猜成功，正在返回..", Utils.getUrl("free.aspx"), "1");
    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string mename = new BCW.BLL.User().GetUsName(meid);
        long gold = new BCW.BLL.User().GetGold(meid);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Free model = new BCW.BLL.Game.Free().GetFree(id);
        if (model == null)
        {
            Utils.Error("不存在的猜猜记录", "");
        }
        //初始化
        string[] arrText = model.pText.Split(",".ToCharArray());
        string[] arrCount = model.pCount.Split(",".ToCharArray());
        string[] arrCount2 = model.pCount2.Split(",".ToCharArray());
        string[] arrOdds = model.pOdds.Split(",".ToCharArray());

        if (info == "ok")
        {
            int num = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "购买数量错误"));
            int i = int.Parse(Utils.GetRequest("i", "post", 2, @"^[0-9]\d*$", "选择选项错误"));

            if (i > arrCount.Length)
            {
                Utils.Error("选择选项错误", "");
            }

            if (Convert.ToInt32(arrCount[i]) - Convert.ToInt32(arrCount2[i]) < num)
            {
                Utils.Error("目前可购买" + (Convert.ToInt32(arrCount[i]) - Convert.ToInt32(arrCount2[i])) + "份", "");
            }

            if (model.CloseTime < DateTime.Now)
            {
                Utils.Error("此竞猜投注已经截止", "");
            }

            if (model.UserID == meid)
            {
                Utils.Error("不能购买自己的竞猜哦", "");
            }
            if (Convert.ToInt64(num * model.Price) > gold)
            {
                Utils.Error("你的币不够哦", "");
            }

            int myCount = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where Userid=" + meid + " and state=1"));
            if (myCount > 0)
            {
                Utils.Error("你还有未确认的猜猜，请确认后再进行投注", "");
            }
            //判断是否已下注
            int IsPay = 0;
            string cmdtxt = "";
            if (Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where Userid=" + meid + " and cclID=" + id + " and ccliType=" + i + "")) > 0)
            {
                IsPay = 1;
            }

            if (IsPay == 0)
            {
                //写入猜客进会员表
                if (Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freemoney where zid=" + model.UserID + " and kid=" + meid + "")) <= 0)
                {
                    SqlHelper.ExecuteSql("insert into tb_freemoney(zid,kid,kName,winmoney) values(" + model.UserID + "," + meid + ",'" + mename + "',0)");
                }

                cmdtxt = "insert into tb_freesell(cclUserID,cclUserName,cclID,UserID,UserName,Title,Content,Odds,Price,Counts,Counts2,CloseTime,OpenTime,ccliType,ccliName) values(" + model.UserID + ",'" + model.UserName + "'," + id + "," + meid + ",'" + mename + "','" + model.Title + "','" + model.Content + "'," + Convert.ToDouble(arrOdds[i]) + "," + model.Price + "," + Convert.ToInt32(arrCount[i]) + "," + num + ",'" + model.CloseTime + "','" + model.OpenTime + "'," + i + ",'" + arrText[i] + "')";
            }
            else
            {
                cmdtxt = "update tb_freesell set cclUserName='" + model.UserName + "',UserName='" + mename + "',Counts2=Counts2+" + num + " where Userid=" + meid + " and cclID=" + id + " and ccliType=" + i + "";

            }
            SqlHelper.ExecuteSql(cmdtxt);

            //更新购买数量
            string sCount = string.Empty;
            for (int j = 0; j < arrCount2.Length; j++)
            {
                if (j == i)
                {
                    sCount += "," + (Convert.ToInt32(arrCount2[j]) + num).ToString();
                }
                else
                    sCount += "," + arrCount2[j];
            }
            sCount = Utils.Mid(sCount, 1, sCount.Length);
            SqlHelper.ExecuteSql("update tb_free set Counts2=Counts2+" + num + ",pCount2='" + sCount + "' where ID=" + id + "");
            //扣币
            new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(num * model.Price), 3);
            //发信给猜主
            string OutMsg = "";
            if (model.Counts == (model.Counts2 + num))
            {
                OutMsg = "目前此竞猜已全部售完，";
            }
            new BCW.BLL.Guest().Add(1, model.UserID, model.UserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]购买了您的竞猜(" + model.Title + ")" + num + "份!" + OutMsg + "请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认!");

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]购买猜猜[url=/bbs/game/free.aspx?act=view&amp;id=" + id + "]" + model.Title + "[/url]" + num + "份";
            new BCW.BLL.Action().Add(3, id, 0, "", wText);
            Utils.Success("购买猜猜", "购买" + num + "份猜猜成功..", Utils.getUrl("free.aspx?act=view&amp;id=" + id + ""), "2");
        }
        else if (info == "open" || info == "open2")
        {
            string sText = Utils.GetRequest("sText", "post", 1, "", "");
            if (sText != "")
            {
                if (sText.Length > 30)
                {
                    Utils.Error("开奖说明不能多于30字", "");
                }
            }
            if (model.UserID != meid && Isfreeadmin(meid) == false)
            {
                Utils.Error("此竞猜不属于你的呀", "");
            }
            if (model.State != 0 && Isfreeadmin(meid) == false)
            {
                Utils.Error("此竞猜已开奖", "");
            }
            if (model.CloseTime > DateTime.Now && model.Counts != model.Counts2)
            {
                Utils.Error("此竞猜还不能开奖", "");
            }

            for (int i = 0; i < model.pNum; i++)
            {
                string a = Utils.GetRequest("stats" + i + "", "post", 2, @"^[1-5]$", "选择结果错误");
            }
            if (info != "open2")
            {
                Master.Title = "开奖";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确认后无法更改");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                string sName = string.Empty;
                string sValu = string.Empty;
                for (int i = 0; i < model.pNum; i++)
                {
                    int state = int.Parse(Utils.GetRequest("stats" + i + "", "post", 2, @"^[1-5]$", "选择结果错误"));
                    builder.Append("" + arrText[i] + "结果:" + cclCase(state) + "<br />");
                    sName += "stats" + i + ",";
                    sValu += "" + Request.Form["stats" + i + ""] + "'";
                }
                if (sText != "")
                {
                    builder.Append("开奖说明::" + sText + "");
                }
                builder.Append(Out.Tab("</div>", ""));
                string strName = sName + "sText,id,info,act";
                string strValu = sValu + "" + sText + "'" + id + "'open2'view";
                string strOthe = "/确认开奖,free.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
 
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
                builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string sState = string.Empty;
                for (int i = 0; i < model.pNum; i++)
                {
                    int state = int.Parse(Utils.GetRequest("stats" + i + "", "post", 2, @"^[1-5]$", "选择结果错误"));
                    SqlHelper.ExecuteSql("update tb_freesell set State=1,OpenStats=" + state + ",OpenTime2='" + DateTime.Now + "',OpenText='" + sText + "' where cclID=" + id + " and ccliType=" + i + "");
                    sState += "," + state;
                }
                sState = Utils.Mid(sState, 1, sState.Length);

                SqlHelper.ExecuteSql("update tb_free set State=1,pState='" + sState + "',OpenTime2='" + DateTime.Now + "',OpenText='" + sText + "' where ID=" + id + "");

                //给下级发信
                DataSet ds = SqlHelper.Query("select UserID,UserName from tb_freesell where cclID=" + id + " order by ID desc");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //发信给猜客
                        int UserID = int.Parse(ds.Tables[0].Rows[i]["UserID"].ToString());
                        string UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        new BCW.BLL.Guest().Add(1, UserID, UserName, "[url=/bbs/uinfo.aspx?uid=" + model.UserID + "]" + model.UserName + "[/url]的竞猜(" + model.Title + ")已经开奖，请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认!");
                    }
                }

                //如果没有售完则返还猜主
                if (model.Counts > model.Counts2)
                {
                    long pCent = 0;
                    for (int i = 0; i < model.pNum; i++)
                    {
                        pCent += Convert.ToInt64(model.Price * Convert.ToDouble(arrOdds[i].ToString()) - model.Price) * (Convert.ToInt64(arrCount[i]) - Convert.ToInt64(arrCount2[i]));
                    }
                    if (pCent != 0)
                    {
                        //返币
                        new BCW.BLL.User().UpdateiGold(model.UserID, model.UserName, pCent, 3);
                        new BCW.BLL.Guest().Add(1, model.UserID, model.UserName, "您的竞猜(" + model.Title + ")未售完并开奖，系统返还您" + pCent + "" + ub.Get("SiteBz") + "，请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认!");
                    }
                }
                //去掉投诉
                if (Isfreeadmin(meid) == true)
                {
                    SqlHelper.ExecuteSql("update tb_freesell set OpenStats=1 where cclID=" + id + " and OpenStats=6");
                }
                Utils.Success("猜猜开奖", "竞猜" + model.Title + "开奖成功..", Utils.getUrl("free.aspx?act=view&amp;id=" + id + ""), "2");
            }
        }
        else
        {
            Master.Title = "查看猜猜";
            builder.Append(Out.Tab("<div class=\"title\">查看猜猜</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("猜主:<a href='" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UserID + "") + "'>" + model.UserName + "</a><br />");
            builder.Append("竞猜题目:" + model.Title + "<br />");
            builder.Append("竞猜描述:" + model.Content + "<br />");
            builder.Append("猜主承诺开奖时间:" + DT.FormatDate(model.OpenTime, 7) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (model.State != 2)
            {
                if (model.CloseTime > DateTime.Now)
                {
                    builder.Append("还有" + DT.DateDiff(model.CloseTime, DateTime.Now) + "投注截止<br />");
                }
                else
                {
                    builder.Append("此竞猜投注已经截止<br />");
                }
                builder.Append("每份价格" + model.Price + "" + ub.Get("SiteBz") + "<br />");

                builder.Append("如果猜对,您每份将获得相应赔率的币币");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                if (model.UserID != meid)
                {
                    for (int i = 0; i < model.pNum; i++)
                    {
                        if (Convert.ToInt32(arrCount[i]) - Convert.ToInt32(arrCount2[i]) <= 0)
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("" + arrText[i] + "(" + arrOdds[i] + "):共" + Convert.ToInt32(arrCount[i]) + "份,剩余份数:已售完");
                            builder.Append(Out.Tab("</div>", "<br />"));
                        }
                        else if (model.CloseTime < DateTime.Now)
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("" + arrText[i] + "(" + arrOdds[i] + "):共" + Convert.ToInt32(arrCount[i]) + "份,售出:" + Convert.ToInt32(arrCount2[i]) + "份");
                            builder.Append(Out.Tab("</div>", "<br />"));
                        }
                        else
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("" + arrText[i] + "(" + arrOdds[i] + "),剩余份数:" + (Convert.ToInt32(arrCount[i]) - Convert.ToInt32(arrCount2[i])) + "");
                            builder.Append(Out.Tab("</div>", "<br />"));
                            strText = "份数:,,,,";
                            strName = "num,i,id,info,act";
                            strType = "snum,hidden,hidden,hidden,hidden";
                            strValu = "1'" + i + "'" + id + "'ok'view";
                            strEmpt = "false,false,false,false,false";
                            strIdea = "";
                            strOthe = "购买,free.aspx,post,3,red";
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                            builder.Append(Out.Tab("", "<br />"));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < model.pNum; i++)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        if (Convert.ToInt32(arrCount[i]) - Convert.ToInt32(arrCount2[i]) <= 0)
                        {
                            builder.Append("" + arrText[i] + "(" + arrOdds[i] + "):共" + Convert.ToInt32(arrCount[i]) + "份,剩余份数:已售完");
                        }
                        else
                        {
                            builder.Append("" + arrText[i] + "(" + arrOdds[i] + "):共" + Convert.ToInt32(arrCount[i]) + "份,售出:" + Convert.ToInt32(arrCount2[i]) + "份");
                        }
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("共" + model.Counts + "份，售出<a href='" + Utils.getUrl("free.aspx?act=mlist&amp;id=" + id + "") + "'>" + model.Counts2 + "份</a><br />");
                    builder.Append("您承诺开奖时间:" + DT.FormatDate(model.OpenTime, 7) + "");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }

                if ((model.UserID == meid || Isfreeadmin(meid) == true) && model.Counts2 == 0)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href='" + Utils.getUrl("free.aspx?act=del&amp;id=" + id + "") + "'>撤销这个竞猜</a>");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }

                if (model.CloseTime > DateTime.Now && model.Counts != model.Counts2)
                {
                    if (model.UserID != meid)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("系统已经扣除了猜主的保证金,请放心购买,如果认为猜主开奖出错,还可以向管理员投诉");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                }
                else
                {
                    if (model.UserID != meid)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        if (model.OpenTime > DateTime.Now)
                            builder.Append("距离猜主承诺开奖时间还有" + DT.DateDiff(model.OpenTime, DateTime.Now) + "");
                        else
                            builder.Append("开奖时间已到，请等待猜主开奖");

                        builder.Append(Out.Tab("</div>", "<br />"));
                    }

                    if (((model.State == 0 && model.UserID == meid) || (Isfreeadmin(meid) == true)) && model.Counts2 != 0)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        if (model.State != 0)
                        {
                            builder.Append("<b>此猜猜已开奖，如猜主开错，您可以进行更改</b><br />");
                        }
                        builder.Append("=开出结果=");
                        builder.Append(Out.Tab("</div>", ""));
                        string sText = string.Empty;
                        string sName = string.Empty;
                        string sType = string.Empty;
                        string sValu = string.Empty;
                        string sEmpt = string.Empty;
                        for (int i = 0; i < model.pNum; i++)
                        {
                            sText += "" + arrText[i] + "结果:,";
                            sName += "stats" + i + ",";
                            sType += "select,";
                            sValu += "1'";
                            sEmpt += "1|猜主全赢|2|平盘|3|猜客全赢|4|猜客输半|5|猜客赢半,";
                        }
                        strText = sText + "开奖说明:,,,";
                        strName = sName + "sText,id,info,act";
                        strType = sType + "text,hidden,hidden,hidden";
                        strValu = sValu + "'" + id + "'open'view";
                        strEmpt = sEmpt + "true,false,false,false";
                        strIdea = "/";
                        strOthe = "确定开奖,free.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                        builder.Append(Out.Tab("", "<br />"));
                    }
                }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("开奖时间:" + DT.FormatDate(model.OpenTime2, 0) + "<br />");
                builder.Append("猜主开奖说明" + model.OpenText + "<br />");

                builder.Append("总共有" + (model.Good + model.General + model.Poor) + "个玩家参与了这个竞猜,其中对猜主公平性," + model.Good + "人满意," + model.General + "人感觉一般," + model.Poor + "人不满意!");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您自带" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("free.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            Master.Title = "删除猜猜";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此猜猜记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            BCW.Model.Game.Free model = new BCW.BLL.Game.Free().GetFree(id);
            if (model == null)
            {
                Utils.Error("不存在的猜猜记录", "");
            }
            if ((model.UserID == meid || Isfreeadmin(meid) == true) && model.Counts2 == 0)
            {
                //删除
                new BCW.BLL.Game.Free().Delete(id);
                //返币给猜主
                new BCW.BLL.User().UpdateiGold(model.UserID, model.UserName, model.cclcent, 3);
                Utils.Success("删除猜猜", "删除猜猜成功" + model.UserID + "..", Utils.getUrl("free.aspx"), "1");
            }
            else
            {
                Utils.Error("不存在的猜猜记录或权限不足", "");
            }
           
        }
    }

    private void MlistPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Game.Free().Exists(id))
        {
            Utils.Error("不存在的猜猜记录", "");
        }
        Master.Title = "猜猜购买记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜猜购买记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "cclID=" + id + "";
        // 开始读取列表
        IList<BCW.Model.Game.Freesell> listFreesell = new BCW.BLL.Game.Freesell().GetFreesells(pageIndex, pageSize, strWhere, out recordCount);
        if (listFreesell.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Freesell n in listFreesell)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>购买<b>{3}</b>共{4}份", n.UserID, (pageIndex - 1) * pageSize + k, n.UserName, n.ccliName, n.Counts2);
                if (n.OpenStats == 1)
                    builder.Append("输了" + Convert.ToInt32(n.Price * n.Counts2) + "" + ub.Get("SiteBz") + "");
                else if (n.OpenStats == 2)
                    builder.Append("平盘返还" + (n.Counts2 * n.Price) + "" + ub.Get("SiteBz") + "");
                else if (n.OpenStats == 3)
                    builder.Append("赢了" + (n.Counts2 * n.Price * n.Odds) + "" + ub.Get("SiteBz") + "(含本金)");
                else if (n.OpenStats == 4)
                    builder.Append("输了" + Convert.ToInt32(n.Counts2 * (n.Price / 2)) + "" + ub.Get("SiteBz") + "");
                else if (n.OpenStats == 5)
                    builder.Append("赢了" + Convert.ToInt32(n.Counts2 * ((n.Price * n.Odds - n.Price) / 2 + n.Price)) + "" + ub.Get("SiteBz") + "");

                if (n.IsGood == 1)
                    builder.Append("已确认(满意)");
                else if (n.IsGood == 2)
                    builder.Append("已确认(一般)");
                else if (n.IsGood == 3)
                    builder.Append("已确认(不满意)");
                else
                    builder.Append("未确认");

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
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的猜猜";
        builder.Append(Out.Tab("<div class=\"title\">我的猜猜</div>", ""));
        //统计
        int myCount1 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_free where Userid=" + meid + " and state=0 and CloseTime>'" + DateTime.Now + "'"));
        int myCount2 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_free where Userid=" + meid + " and state=0 and CloseTime<'" + DateTime.Now + "'"));
        int myCount3 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_free where Userid=" + meid + " and state=1"));
        int myCount4 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_free where Userid=" + meid + " and state=2"));

        int myCount5 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where Userid=" + meid + " and state=0"));
        int myCount6 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where Userid=" + meid + " and state=1"));
        int myCount7 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where Userid=" + meid + " and state=2"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【我是猜主】<br />");
        builder.Append("1.<a href='" + Utils.getUrl("free.aspx?act=zlist&amp;ptype=1") + "'>正在进行的竞猜(" + myCount1 + ")</a><br />");
        builder.Append("2.<a href='" + Utils.getUrl("free.aspx?act=zlist&amp;ptype=2") + "'>等您开奖的竞猜(" + myCount2 + ")</a><br />");
        builder.Append("3.<a href='" + Utils.getUrl("free.aspx?act=zlist&amp;ptype=3") + "'>已开奖等猜客确认(" + myCount3 + ")</a><br />");
        builder.Append("4.<a href='" + Utils.getUrl("free.aspx?act=zlist&amp;ptype=4") + "'>完成的竞猜(" + myCount4 + ")</a><br />");
        builder.Append("【我是猜客】<br />");
        builder.Append("1.<a href='" + Utils.getUrl("free.aspx?act=klist&amp;ptype=1") + "'>等待猜主开奖(" + myCount5 + ")</a><br />");
        builder.Append("2.<a href='" + Utils.getUrl("free.aspx?act=klist&amp;ptype=2") + "'>已开奖等您确认(" + myCount6 + ")</a><br />");
        builder.Append("3.<a href='" + Utils.getUrl("free.aspx?act=klist&amp;ptype=3") + "'>完成的竞猜(" + myCount7 + ")</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href='" + Utils.getUrl("free.aspx?act=myck") + "'>我的猜客记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ZlistPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-4]$", "类型错误"));
        string sWhere = "";
        string sName = "";
        if (ptype == 1)
        {
            sName = "正在进行的竞猜";
            sWhere = "Userid=" + meid + " and state=0 and CloseTime>'" + DateTime.Now + "'";
        }
        else if (ptype == 2)
        {
            sName = "等您开奖的竞猜";
            sWhere = "Userid=" + meid + " and state=0 and CloseTime<'" + DateTime.Now + "'";
        }
        else if (ptype == 3)
        {
            sName = "已开奖等猜客确认";
            sWhere = "Userid=" + meid + " and state=1";
        }
        else if (ptype == 4)
        {
            sName = "完成的竞猜";
            sWhere = "Userid=" + meid + " and state=2";
        }

        Master.Title = sName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(sName);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = sWhere;
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Free> listFree = new BCW.BLL.Game.Free().GetFrees(pageIndex, pageSize, strWhere, out recordCount);
        if (listFree.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Free n in listFree)
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
                string surl = "";
                if (ptype > 2)
                    surl = "free.aspx?act=myview";
                else
                    surl = "free.aspx?act=view";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("" + surl + "&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">({1}){2}", n.ID, DT.FormatDate(n.AddTime, 4), n.Title);

                if (n.State == 2)
                {
                    builder.Append("(卖出" + n.Counts2 + "/共" + n.Counts + "份)");
                }
                builder.Append("</a>");
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
        builder.Append("<a href=\"" + Utils.getPage("free.aspx?act=me") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void KlistPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-3]$", "类型错误"));
        string sWhere = "";
        string sName = "";
        if (ptype == 1)
        {
            sName = "等待猜主开奖";
            sWhere = "Userid=" + meid + " and state=0";
        }
        else if (ptype == 2)
        {
            sName = "已开奖等您确认";
            sWhere = "Userid=" + meid + " and state=1";
        }
        else if (ptype == 3)
        {
            sName = "完成的竞猜";
            sWhere = "Userid=" + meid + " and state=2";
        }

        Master.Title = sName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(sName);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = sWhere;
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Freesell> listFreesell = new BCW.BLL.Game.Freesell().GetFreesells(pageIndex, pageSize, strWhere, out recordCount);
        if (listFreesell.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Freesell n in listFreesell)
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
                string surl = "";
                surl = "free.aspx?act=myview2";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("" + surl + "&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}", n.ID, n.Title);
                builder.Append("(买入" + n.ccliName + "共" + n.Counts2 + "份)" + cclCase(n.OpenStats) + "");
                builder.Append("</a>");
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
        builder.Append("<a href=\"" + Utils.getPage("free.aspx?act=me") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Free model = new BCW.BLL.Game.Free().GetFree(id);
        if (model == null)
        {
            Utils.Error("不存在的猜猜记录", "");
        }
        if (model.UserID != meid)
        {
            Utils.Error("不存在的猜猜记录", "");
        }
        Master.Title = "我的猜猜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看猜猜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("这是您创建的竞猜<br />");
        builder.Append("竞猜题目:" + model.Title + "<br />");
        builder.Append("竞猜描述:" + model.Content + "<br />");

        builder.Append("每份价格" + model.Price + "金币<br />");
        //初始化
        string[] arrText = model.pText.Split(",".ToCharArray());
        string[] arrCount = model.pCount.Split(",".ToCharArray());
        string[] arrCount2 = model.pCount2.Split(",".ToCharArray());
        string[] arrOdds = model.pOdds.Split(",".ToCharArray());
        string[] arrState = model.pState.Split(",".ToCharArray());
        for (int i = 0; i < model.pNum; i++)
        {
            builder.Append("" + arrText[i] + "(" + arrOdds[i] + "):共" + Convert.ToInt32(arrCount[i]) + "份,售出:" + Convert.ToInt32(arrCount2[i]) + "份<br />");

        }
        builder.Append("共" + model.Counts + "份，共售出" + model.Counts2 + "份<br />");

        if (model.State != 0)
        {
            builder.Append("开出结果如下:<br />");

            for (int i = 0; i < model.pNum; i++)
            {
                builder.Append("" + arrText[i] + "(" + arrOdds[i] + "):<br />");

                builder.Append("开出结果:" + cclCase(Convert.ToInt32(arrState[i])) + "<br />");
            }
            builder.Append("开奖时间:" + DT.FormatDate(model.OpenTime, 7) + "<br />");
            if (!string.IsNullOrEmpty(model.OpenText))
                builder.Append("开奖说明:" + model.OpenText + "<br />");
            else
                builder.Append("开奖说明:无<br />");

            if (model.State == 2)
                builder.Append("本竞猜盈利：" + model.cclstat + "(不含手续费)<br />");

            builder.Append("=购买玩家列表=<br />");
            DataSet ds = SqlHelper.Query("select top 5 ID,Title,Price,Counts2,Odds,OpenStats,UserID,UserName,IsGood,ccliName,State from tb_freesell  where cclID=" + id + " order by ID DEsc");
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("<a href='" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UserID"].ToString() + "") + "'>" + ds.Tables[0].Rows[i]["UserName"].ToString() + "</a>购买" + ds.Tables[0].Rows[i]["ccliName"].ToString() + "共" + ds.Tables[0].Rows[i]["Counts2"].ToString() + "份,");
                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["State"].ToString()) == 2)
                    {
                        int Price = Convert.ToInt32(ds.Tables[0].Rows[i]["Price"].ToString());
                        int Counts2 = Convert.ToInt32(ds.Tables[0].Rows[i]["Counts2"].ToString());
                        double Odds = Convert.ToDouble(ds.Tables[0].Rows[i]["Odds"].ToString());
                        int OpenStats = Convert.ToInt32(ds.Tables[0].Rows[i]["OpenStats"].ToString());

                        if (OpenStats == 1)
                            builder.Append("输了" + Convert.ToInt32(Price * Counts2) + "" + ub.Get("SiteBz") + "");
                        else if (OpenStats == 2)
                            builder.Append("平盘返还" + (Counts2 * Price) + "" + ub.Get("SiteBz") + "");
                        else if (OpenStats == 3)
                            builder.Append("赢了" + (Counts2 * Price * Odds) + "" + ub.Get("SiteBz") + "(含本金)");
                        else if (OpenStats == 4)
                            builder.Append("输了" + Convert.ToInt32(Counts2 * (Price / 2)) + "" + ub.Get("SiteBz") + "");
                        else if (OpenStats == 5)
                            builder.Append("赢了" + Convert.ToInt32(Counts2 * ((Price * Odds - Price) / 2 + Price)) + "" + ub.Get("SiteBz") + "");
                    }
                    string IsGoodText = "";
                    if (int.Parse(ds.Tables[0].Rows[i]["IsGood"].ToString()) == 1)
                        IsGoodText = "已确认(满意)";
                    else if (int.Parse(ds.Tables[0].Rows[i]["IsGood"].ToString()) == 2)
                        IsGoodText = "已确认(一般)";
                    else if (int.Parse(ds.Tables[0].Rows[i]["IsGood"].ToString()) == 3)
                        IsGoodText = "已确认(不满意)";
                    else
                        IsGoodText = "未确认";

                    builder.Append(IsGoodText + "<br />");

                }
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href='" + Utils.getUrl("free.aspx?act=mlist&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "'>更多购买用户</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("free.aspx?act=me") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

    }

    private void MyView2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string mename = new BCW.BLL.User().GetUsName(meid);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Freesell model = new BCW.BLL.Game.Freesell().GetFreesell(id);
        if (model == null)
        {
            Utils.Error("不存在的猜猜购买记录", "");
        }
        if (model.UserID != meid)
        {
            Utils.Error("不存在的猜猜购买记录", "");
        }
        Master.Title = "我的猜猜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看猜猜");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (info == "open")
        {
            if (model.State == 2)
            {
                Utils.Error("竞猜已确认成功", "");
            }
            int IsGood = int.Parse(Utils.GetRequest("IsGood", "post", 2, @"^[1-3]$", "选择结果错误"));
            string sText = Utils.GetRequest("sText", "all", 1, "", "");
            string sWhere = "";
            if (IsGood == 1)
            {
                sWhere = "Good=Good+1";
            }
            else if (IsGood == 2)
            {
                sWhere = "General=General+1";
            }
            else
            {
                sWhere = "Poor=Poor+1";
            }
            SqlHelper.ExecuteSql("update tb_freesell set State=2,IsGood=" + IsGood + ",Openbbs='" + sText + "',OpenbbsTime='" + DateTime.Now + "' where ID=" + id + "");
            SqlHelper.ExecuteSql("update tb_free set " + sWhere + " where ID=" + model.cclID + "");
            //判断该竞猜的猜客是否确认完毕
            int myCount1 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where cclID=" + model.cclID + ""));
            int myCount2 = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_freesell where cclID=" + model.cclID + " and State=2"));
            if (myCount1 == myCount2)
            {
                SqlHelper.ExecuteSql("update tb_free set State=2 where ID=" + model.cclID + "");
            }

            //写入猜主赚币排行榜WIN
            //统计猜主/猜客得到/失去的币
            if (model.OpenStats == 1)//猜主全赢
            {
                //计算猜主本金
                int bj = Convert.ToInt32(model.Counts2 * (model.Price * model.Odds - model.Price));

                int zWinPrice = Convert.ToInt32(model.Price * model.Counts2 - (model.Price * model.Counts2 * GetPL() / 100));
                int kLostPrice = Convert.ToInt32(model.Price * model.Counts2);
                int xPrice = model.Price * model.Counts2;
                //记录本竞猜输赢
                SqlHelper.ExecuteSql("update tb_free set cclstat=cclstat+" + xPrice + " where ID=" + model.cclID + "");
                //记录猜客输
                int imoney = kLostPrice;
                SqlHelper.ExecuteSql("update tb_freemoney set kName='" + mename + "',winmoney=winmoney-" + imoney + " where zid=" + model.cclUserID + " and kid=" + meid + "");
                //猜主加币
                new BCW.BLL.User().UpdateiGold(model.cclUserID, model.cclUserName, Convert.ToInt64(bj+zWinPrice), 3);
                //发信给猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经确认了您的竞猜(" + model.Title + ")，你赢" + (bj + zWinPrice) + "" + ub.Get("SiteBz") + "(含本金),他同时给了您评价!请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认!");

                Utils.Success("确认猜猜", "确认猜猜成功，您输了" + kLostPrice + "" + ub.Get("SiteBz") + "", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");

            }
            else if (model.OpenStats == 2)//平盘
            {
                int zbackPrice = Convert.ToInt32(model.Counts2 * (model.Price * model.Odds - model.Price));
                int kbackPrice = Convert.ToInt32(model.Price * model.Counts2);
                //双方返币
                new BCW.BLL.User().UpdateiGold(meid, mename, Convert.ToInt64(kbackPrice), 3);
                new BCW.BLL.User().UpdateiGold(model.cclUserID, model.cclUserName, Convert.ToInt64(zbackPrice), 3);
                //发信给猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经确认了您的竞猜(" + model.Title + ")，双方平盘，保证金" + zbackPrice + "" + ub.Get("SiteBz") + "已经退回您的帐户，Ta同时给了您评价！请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认！");
                Utils.Success("确认猜猜", "确认猜猜成功，平盘返还" + kbackPrice + "" + ub.Get("SiteBz") + "", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");

            }
            else if (model.OpenStats == 3)//猜客全赢
            {
                int kWinPrice = Convert.ToInt32(model.Counts2 * (model.Price * model.Odds) - (model.Counts2 * (model.Price * model.Odds) * GetPL() / 100));
                int zLostPrice = Convert.ToInt32(model.Counts2 * (model.Price * model.Odds));
                int xPrice = zLostPrice;
                //记录猜客赢
                int imoney = kWinPrice - model.Counts2 * model.Price;
                SqlHelper.ExecuteSql("update tb_freemoney set kName='" + mename + "',winmoney=winmoney+" + imoney + " where zid=" + model.cclUserID + " and kid=" + meid + "");
                //记录本竞猜输赢
                SqlHelper.ExecuteSql("update tb_free set cclstat=cclstat-" + xPrice + " where ID=" + model.cclID + "");
                //猜客加币
                new BCW.BLL.User().UpdateiGold(meid, mename, Convert.ToInt64(kWinPrice), 3);
                //发信给猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经确认了您的竞猜(" + model.Title + ")，你输" + zLostPrice + "" + ub.Get("SiteBz") + "，Ta同时给了您评价！请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认！");
                Utils.Success("确认猜猜", "确认猜猜成功，恭喜你赢了" + kWinPrice + "" + ub.Get("SiteBz") + "", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");
            }
            else if (model.OpenStats == 4)//猜客输半
            {
                int kLostPrice = Convert.ToInt32(model.Counts2 * (model.Price / 2));//猜客输半
                //int zWinPrice = Convert.ToInt32(model.Counts2 * model.Price) - kLostPrice;
                int zWinPrice = Convert.ToInt32(model.Counts2 * ((model.Price * model.Odds - model.Price) / 2 + model.Price));//猜主赢半
                int xPrice = Convert.ToInt32(model.Counts2 * ((model.Price * model.Odds - model.Price) / 2));
                //记录本竞猜输赢
                SqlHelper.ExecuteSql("update tb_free set cclstat=cclstat+" + xPrice + " where ID=" + model.cclID + "");
                //记录猜客输半
                int imoney = kLostPrice;
                SqlHelper.ExecuteSql("update tb_freemoney set kName='" + mename + "',winmoney=winmoney-" + imoney + " where zid=" + model.cclUserID + " and kid=" + meid + "");
                //猜主加币
                new BCW.BLL.User().UpdateiGold(model.cclUserID, model.cclUserName, Convert.ToInt64(zWinPrice), 3);
                //猜客加币
                new BCW.BLL.User().UpdateiGold(meid, mename, Convert.ToInt64(kLostPrice), 3);
                //发信给猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经确认了您的竞猜(" + model.Title + ")，对方输半，你赢" + (zWinPrice) + "" + ub.Get("SiteBz") + "(含本金)，Ta同时给了您评价！请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认！");
                Utils.Success("确认猜猜", "确认猜猜成功，结果输半,系统自动返还你" + kLostPrice + "" + ub.Get("SiteBz") + "", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");
            }

            else if (model.OpenStats == 5)//猜客赢半
            {
                //计算猜主本金
                int bj = Convert.ToInt32(model.Counts2 * (model.Price * model.Odds - model.Price));

                int kWinPrice = Convert.ToInt32(model.Counts2 * ((model.Price * model.Odds - model.Price) / 2 + model.Price));//猜客赢半
                //int zLostPrice = Convert.ToInt32(model.Counts2 * (model.Price * model.Odds) - kWinPrice);//返还一半给猜主
                int zLostPrice = Convert.ToInt32(bj / 2);//猜主输半

                int xPrice = zLostPrice;
                //记录本竞猜输赢
                SqlHelper.ExecuteSql("update tb_free set cclstat=cclstat-" + xPrice + " where ID=" + model.cclID + "");
                //记录猜客赢半
                int imoney = zLostPrice;
                SqlHelper.ExecuteSql("update tb_freemoney set kName='" + mename + "',winmoney=winmoney+" + imoney + " where zid=" + model.cclUserID + " and kid=" + meid + "");
                //猜主加币
                new BCW.BLL.User().UpdateiGold(model.cclUserID, model.cclUserName, Convert.ToInt64(zLostPrice), 3);
                //猜客加币
                new BCW.BLL.User().UpdateiGold(meid, mename, Convert.ToInt64(kWinPrice), 3);
                //发信给猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经确认了您的竞猜(" + model.Title + ")，对方赢半,返" + zLostPrice + "" + ub.Get("SiteBz") + "，Ta同时给了您评价！请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认！");
                Utils.Success("确认猜猜", "确认猜猜成功，结果赢半,返" + kWinPrice + "" + ub.Get("SiteBz") + "", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");
            }

        }
        else if (info == "ts")
        {
            //接收投诉ID
            int CpsId = Utils.ParseInt(ub.GetSub("FreeCpsId", xmlPath));
            if (model.OpenStats != 6)
            {
                SqlHelper.ExecuteSql("update tb_freesell set OpenStats=6 where ID=" + id + "");
                //发信给管理员和猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]对您的竞猜(" + model.Title + ")的开奖结果不认可并且管理员投诉,稍后将由管理员处理！请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认！");
                new BCW.BLL.Guest().Add(1, CpsId, "猜猜管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]对竞猜(" + model.Title + ")的开奖结果不认可并且向您投诉,请到[url=/bbs/game/free.aspx?act=view&amp;id=" + model.cclID + "]处理该竞猜[/url]！");
                Utils.Success("投诉猜猜", "投诉成功，请等待管理员处理..", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");
            }
            else
            {
                SqlHelper.ExecuteSql("update tb_freesell set OpenStats=1 where ID=" + id + "");
                //发信给管理员和猜主
                new BCW.BLL.Guest().Add(1, model.cclUserID, model.cclUserName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]对您的竞猜(" + model.Title + ")的开奖结果撤销了投诉！请到[url=/bbs/game/free.aspx?act=me]我的竞猜[/url]确认！");
                new BCW.BLL.Guest().Add(1, CpsId, "猜猜管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]对竞猜(" + model.Title + ")的开奖结果撤销了投诉！");
                Utils.Success("撤销投诉", "撤销投诉成功", Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("猜主:<a href='" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.cclUserID + "") + "'>" + model.cclUserName + "</a><br />");
            builder.Append("竞猜题目:<a href='" + Utils.getUrl("free.aspx?act=view&amp;id=" + model.cclID + "&amp;backurl=" + Utils.PostPage(1) + "") + "'>" + model.Title + "</a><br />");
            builder.Append("竞猜描述:" + model.Content + "<br />");
            builder.Append("购买竞猜选项:" + model.ccliName + "<br />");
            builder.Append("开奖时间:" + DT.FormatDate(model.OpenTime, 7) + "<br />");
            builder.Append("每份价格" + model.Price + "" + ub.Get("SiteBz") + "<br/>赔率" + Convert.ToDouble(model.Odds) + "(含本金)<br />");
            builder.Append("共" + model.Counts + "份，您购买" + model.Counts2 + "份");

            if (model.State == 0)
            {
                builder.Append("<br />请等待猜主开奖");
            }
            else if (model.State == 1 || model.State == 2)//等待猜客确认
            {
                string sResult = cclCase(model.OpenStats);

                builder.Append("<br />开奖结果:" + sResult + "");
                if (!string.IsNullOrEmpty(model.OpenText))
                    builder.Append("<br />开奖说明:" + model.OpenText + "");
                else
                    builder.Append("<br />开奖说明:无");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (model.State == 1)
            {
                strText = "评价公平性:,评价内容:,,,,";
                strName = "IsGood,sText,id,info,act";
                strType = "select,text,hidden,hidden,hidden";
                strValu = "1''" + id + "'open'myview2";
                strEmpt = "1|满意|2|一般|3|不满意,false,false,false,false";
                strIdea = "/";
                strOthe = "确认,free.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                if (model.OpenStats != 6)
                {
                    builder.Append(" 如果认为猜主开奖出错请不要确认,马上向管理员<a href='" + Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + "&amp;info=ts") + "'>投诉</a>");
                }
                else
                {
                    builder.Append(" 您已向管理员投诉,<a href='" + Utils.getUrl("free.aspx?act=myview2&amp;id=" + id + "&amp;info=ts") + "'>撤消投诉</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            if (model.State == 2)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("您已经确认本次竞猜结果,本次竞猜已经全部结束!<br />");
                if (model.OpenStats == 1)
                    builder.Append("猜主开" + cclCase(model.OpenStats) + ",您输了" + Convert.ToInt32(model.Price * model.Counts2) + "" + ub.Get("SiteBz") + ",下次再赢Ta吧!<br />");
                else if (model.OpenStats == 2)
                    builder.Append("猜主开" + cclCase(model.OpenStats) + ",系统返还您" + Convert.ToInt32(model.Counts2 * model.Price) + "" + ub.Get("SiteBz") + ",请再接再厉!<br />");
                else if (model.OpenStats == 3)
                    builder.Append("猜主开" + cclCase(model.OpenStats) + ",您赢了" + Convert.ToInt32(model.Counts2 * model.Price * model.Odds) + "" + ub.Get("SiteBz") + "(含本金),请不要沾沾自喜，继续努力!<br />");
                else if (model.OpenStats == 4)
                    builder.Append("猜主开" + cclCase(model.OpenStats) + ",您输了" + Convert.ToInt32(model.Counts2 * (model.Price / 2)) + "" + ub.Get("SiteBz") + "下次再赢Ta吧!<br />");
                else if (model.OpenStats == 5)
                    builder.Append("猜主开" + cclCase(model.OpenStats) + ",您赢了" + Convert.ToInt32(model.Counts2 * ((model.Price * model.Odds - model.Price) / 2 + model.Price)) + "" + ub.Get("SiteBz") + "请再接再厉!<br />");

                if (model.IsGood == 1)
                    builder.Append("您对猜主这次公平性表示(满意)!<br />");
                else if (model.IsGood == 2)
                    builder.Append("您对猜主这次公平性表示(一般)!<br />");
                else
                    builder.Append("您对猜主这次公平性表示(不满意)!<br />");

                if (!string.IsNullOrEmpty(model.Openbbs))
                    builder.Append("您的留言:" + model.Openbbs + "<br />");
                else
                    builder.Append("您的留言:无<br />");

                int Good = 0;
                int General = 0;
                int Poor = 0;
                DataSet ds = new BCW.BLL.Game.Free().GetList("Good,General,Poor", "ID=" + model.cclID + "");
                if (ds != null)
                {
                    Good = Convert.ToInt32(ds.Tables[0].Rows[0]["Good"].ToString());
                    General = Convert.ToInt32(ds.Tables[0].Rows[0]["General"].ToString());
                    Poor = Convert.ToInt32(ds.Tables[0].Rows[0]["Poor"].ToString());
                }
                builder.Append("总共有" + (Good + General + Poor) + "个玩家参与了这个竞猜<br />对猜主公平性:" + Good + "人满意," + General + "人感觉一般," + Poor + "人不满意!");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("free.aspx?act=me") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
            builder.Append(Out.Tab("</div>", ""));


        }
    }

    private void MyckPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的猜客";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我的猜客");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "zid=" + meid + "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Freemoney> listFreemoney = new BCW.BLL.Game.Freemoney().GetFreemoneys(pageIndex, pageSize, strWhere, out recordCount);
        if (listFreemoney.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Freemoney n in listFreemoney)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>", n.kid, n.kname);
                builder.Append("(盈利" + Convert.ToDouble(n.winmoney) + "" + ub.Get("SiteBz") + ")");
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
        builder.Append("<a href=\"" + Utils.getPage("free.aspx?act=me") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void MorePage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-7]$", "0"));
        string sType = string.Empty;
        if (ptype > 0)
        {
            if (ptype == 1)
                sType = "足球";
            else if (ptype == 2)
                sType = "篮球";
            else if ((ptype == 3))
                sType = "彩票";
            else if ((ptype == 4))
                sType = "股票";
            else if ((ptype == 5))
                sType = "体育综合";
            else
                sType = "日常生活";
        }
        else
        {
            sType = "最新";
        }
        Master.Title = "" + sType + "猜猜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + sType + "猜猜");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype > 0)
            strWhere = "CclType=" + ptype + " and state=2";
        else
            strWhere = "state=0";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Free> listFree = new BCW.BLL.Game.Free().GetFrees(pageIndex, pageSize, strWhere, out recordCount);
        if (listFree.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Free n in listFree)
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
                builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + n.Price + "金)</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "猜猜乐排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=3";
        if (ptype == 1)
            strOrder = "(WinGold+PutGold) Desc";
        else
            strOrder = "(WinNum-PutNum) Desc";

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
                    sText = "胜" + (n.WinNum - n.PutNum) + "次";

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
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RulePage()
    {
        Master.Title = "猜猜乐游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜猜乐游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(ub.GetSub("FreeRule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Rule2Page()
    {
        Master.Title = "猜猜乐开盘示例";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜猜乐开盘示例");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("猜主是要发布一件有确定结果的事情<br />可以开足球、篮球、彩票、股票、体育综合、日常生活等等的竞猜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=下面将以两个示例作为说明=<br />");
        builder.Append("<b>股票示例</b><br />");
        builder.Append("题目：明天上证指数尾数为1、3、5尾<br />");
        builder.Append("详细描述：不相信的友友的请投不相信，敢不敢？<br />");
        builder.Append("单份价格：100<br />");
        builder.Append("选项数：1<br />");
        builder.Append("第1项：不相信:10份,赔率1.9<br />");
        builder.Append("截止时间：" + DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append("开奖返彩时间：" + DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append("<b>篮球示例</b><br />");
        builder.Append("题目：纽约红牛VS曼彻斯特城<br />");
        builder.Append("详细描述：纽约红牛受让半球曼彻斯特城,大小球2.5<br />");
        builder.Append("单份价格：100<br />");
        builder.Append("选项数：4<br />");
        builder.Append("第1项：纽约红牛:10份,赔率1.9<br />");
        builder.Append("第2项：曼彻斯特城:12份,赔率1.98<br />");
        builder.Append("第3项：大球:15份,赔率1.98<br />");
        builder.Append("第4项：大球:18份,赔率1.88<br />");
        builder.Append("截止时间：" + DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append("开奖返彩时间：" + DateTime.Now.AddHours(3).ToString("yyyy-MM-dd HH:mm:ss") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("free.aspx?act=add") + "\">马上开盘</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("free.aspx") + "\">猜猜乐</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private string cclCase(int iType)
    {
        string Tname = string.Empty;
        if (iType == 1)
            Tname = "猜主全赢";
        else if (iType == 2)
            Tname = "平盘";
        else if (iType == 3)
            Tname = "猜客全赢";
        else if (iType == 4)
            Tname = "猜客输半";
        else if (iType == 5)
            Tname = "猜客赢半";
        return Tname;
    }

    /// <summary>
    /// 取手续费比率
    /// </summary>
    private int GetPL()
    {
        return Utils.ParseInt(ub.GetSub("FreeTax", xmlPath));
    }

    /// <summary>
    /// 接收投诉ID
    /// </summary>
    private bool Isfreeadmin(int meid)
    {
        bool bl = false;
        if (meid > 0)
        {
            int CpsId = Utils.ParseInt(ub.GetSub("FreeCpsId", xmlPath));
            if (CpsId.Equals(meid))
                bl = true;
        }
        return bl;
    }
}