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

/// <summary>
/// 修改拾物奖励
/// 
/// 黄国军20160324
/// </summary>
public partial class bbs_game_Flows : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();         //抢啊
                break;
            case "top":
            case "me":
                TopPage(act);       //排行榜
                break;
            case "change":
                ChangePage();       //兑换礼品提示
                break;
            case "changeinfo":
                ChangeInfoPage();   //兑换过程
                break;
            case "changegift":
                ChangeGiftPage();   //兑换商城礼物
                break;
            default:
                ReloadPage();       //首页
                break;
        }
    }

    #region 抢啊 ViewPage
    private void ViewPage()
    {
        Master.Title = "抢啊~";
        if (Utils.GetBrowser() == "Opera" || Utils.GetBrowser() == "Mozilla" || Utils.GetBrowser() == "Firefox" || Utils.GetBrowser() == "IE")
        {
            Utils.Error("请使用手机进行拾物", "");
        }
        if (Utils.GetUA().Contains("Mozilla/4.0+") || Utils.GetUA().Contains("Opera") || Utils.GetUA().Contains("Explorer") || Utils.GetUA().Contains("MQQBrowser"))
        {
            Utils.Error("请使用手机进行拾物", "");
        }
        if (Utils.GetBrowser() == "IE" && Utils.GetUA().Contains("Mozilla/4.0"))
        {
            Utils.Error("请使用手机进行拾物", "");
        }
        if (Utils.GetBrowser() == "Chrome" && Utils.GetUA().Contains("Windows"))
        {
            Utils.Error("请使用手机进行拾物", "");
        }
        //string ipCity = new IPSearch().GetAddressWithIP(Utils.GetUsIP());
        //if (!string.IsNullOrEmpty(ipCity))
        //{
        //    ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
        //    if (!ipCity.Contains("移动"))
        //    {
        //        Utils.Error("请使用移动网络进行拾物", "");
        //    }
        //}

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string xmlPath = "/Controls/Flows.xml";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型错误"));
        if (ptype > 15)
        {
            Utils.Error("类型错误", "");
        }
        ub xml = new ub();
        HttpContext.Current.Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        DateTime SecTime = Convert.ToDateTime(ub.GetSub("FlowsGfSecTime" + ptype + "", xmlPath));
        if (SecTime < DateTime.Now)
        {
            //更新下一期抽奖时间(秒)
            xml.dss["FlowsGfSecTime" + ptype + ""] = DateTime.Now.AddDays(-365).ToString();
            try
            {
                System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            catch { }
            HttpContext.Current.Application.Remove(xmlPath);//清缓存

            builder.Append("到底你还是慢了半拍！物品不是被别人抢走就是溜走咯~~");
            builder.Append("<br /><a href=\"" + Utils.getPage("/bbs/default.aspx") + "\">继续去溜达&gt;&gt;</a>");
        }
        else
        {
            //验证防刷

            string verify = Utils.GetRequest("verify", "get", 2, @"^[\s\S]{1,}$", "验证码错误");
            verify = DESEncrypt.Decrypt(verify);
            if (verify == "")
            {
                Utils.Error("验证码有误,请点击以下返回", Utils.getPage("/bbs/default.aspx"));
            }
            if (!Utils.IsRegex(verify, @"^[0-9]\d*$"))
            {
                Utils.Error("验证码有误,请点击以下返回", Utils.getPage("/bbs/default.aspx"));
            }

            string meverify = new BCW.BLL.User().GetVerifys(meid);
            if (!string.IsNullOrEmpty(meverify))
            {
                if (verify.Equals(meverify))
                {
                    Utils.Error("验证码有误,请点击以下返回", Utils.getPage("/bbs/default.aspx"));
                }
            }
            //更新验证码
            new BCW.BLL.User().UpdateVerifys(meid, verify);

            string GfTitle = xml.dss["FlowsGfTitle" + ptype + ""].ToString();
            string GfImage = xml.dss["FlowsGfImage" + ptype + ""].ToString();
            //得到鲜花类型
            int FlowsType = BCW.User.Game.GiftFlows.GetNumByTitle(GfTitle);

            //防止同一会员同一类型多次拾物
            if (new BCW.BLL.Game.GiftFlows().ExistsSec(FlowsType, meid, 15))
            {
                Utils.Error("您已获得本轮奖励，请等待下一轮或到别处抢抢吧", Utils.getPage("/bbs/default.aspx"));
            }
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.Game.GiftFlows model = new BCW.Model.Game.GiftFlows();
            model.Types = FlowsType;
            model.UsID = meid;
            model.UsName = mename;
            model.Total = 1;
            model.Totall = 1;

            if (!new BCW.BLL.Game.GiftFlows().Exists(FlowsType, meid))
                new BCW.BLL.Game.GiftFlows().Add(model);
            else
                new BCW.BLL.Game.GiftFlows().Update(model);

            //记录动态
            string wText = "获得[url=/bbs/game/flows.aspx]" + GfTitle + "[img]/Files/gift/Flows/" + GfImage + "[/img][/url]";
            new BCW.BLL.Action().Add(19, 0, meid, mename, wText);

            builder.Append("恭喜恭喜~~你获得" + GfTitle + "<img src=\"/Files/gift/Flows/" + GfImage + "\" alt=\"" + GfTitle + "\"/>一个");
            builder.Append("<br /><a href=\"" + Utils.getPage("/bbs/default.aspx") + "\">继续去溜达&gt;&gt;</a>");

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">进入拾物首页</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=top") + "\">拾物排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 拾物活动首页 ReloadPage
    private void ReloadPage()
    {

        Master.Title = "拾物活动首页";
        builder.Append(Out.Tab("<div class=\"title\">拾物活动首页</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("排行:<a href=\"" + Utils.getUrl("flows.aspx?act=top&amp;ptype=1") + "\">总量榜</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=top&amp;ptype=2") + "\">分类榜</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=me") + "\">我的物品</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=change") + "\">兑换礼品</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【最新动态】");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取动态列表
        int SizeNum = 5;
        string strWhere = "";
        strWhere = "Types=19";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));

                if (n.UsId == 0)
                    builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(n.Notes));
                else
                    builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=19&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【拾物TOP 5】");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = BCW.Data.SqlHelper.Query("Select TOP 5 UsID, Sum(Total) as Total from tb_GiftFlows GROUP BY UsID ORDER BY Sum(Total) DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int usid = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                builder.Append(Out.Tab("<div>", ""));
                builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}") + "采到{3}个", (i + 1), usid, new BCW.BLL.User().GetUsName(usid), Convert.ToDouble(ds.Tables[0].Rows[i]["Total"].ToString()));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
            builder.Append("暂无数据记录..<br />");
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=输入ID查找=");
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,,";
        string strName = "uid,ptype,act";
        string strType = "num,hidden,hidden";
        string strValu = "'2'top";
        string strEmpt = "false,false";
        string strIdea = "";
        string strOthe = "查清单,flows.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(19, "flows.aspx", 5, 0)));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 排行榜 TopPage
    private void TopPage(string act)
    {
        Master.Title = "排行榜";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (act == "me")
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            uid = meid;
            ptype = 2;

            Master.Title = "我的物品";
        }
        builder.Append(Out.Tab("<div>", ""));
        if (uid == 0)
        {
            if (ptype == 1)
                builder.Append("总量榜|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=top&amp;ptype=1") + "\">总量榜</a>|");

            if (ptype == 2)
                builder.Append("分类榜");
            else
                builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=top&amp;ptype=2") + "\">分类榜</a>");
        }
        else
        {
            string UsName = new BCW.BLL.User().GetUsName(uid);
            if (UsName == "")
            {
                Utils.Error("不存在的会员", "");
            }
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>的物品");
            builder.Append("<br />总量:" + new BCW.BLL.Game.GiftFlows().GetTotal(uid) + "/剩余:" + new BCW.BLL.Game.GiftFlows().GetTotall(uid) + "个");
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        if (id > 0)
            strWhere = "Types=" + id + "";

        if (uid > 0)
            strWhere = "UsID=" + uid + "";

        string[] pageValUrl = { "act", "id", "uid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.GiftFlows> listGiftFlows = null;
        if (ptype == 1)
            listGiftFlows = new BCW.BLL.Game.GiftFlows().GetGiftFlowssTop(pageIndex, pageSize, strWhere, out recordCount);
        else
            listGiftFlows = new BCW.BLL.Game.GiftFlows().GetGiftFlowssTop2(pageIndex, pageSize, strWhere, out recordCount);

        if (listGiftFlows.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.GiftFlows n in listGiftFlows)
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
                if (ptype == 1)
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>" + n.Total + "个");
                else
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("flows.aspx?act=top&amp;ptype=1&amp;id=" + n.Types + "") + "\">" + BCW.User.Game.GiftFlows.GetTitleByNum(n.Types) + "</a>共" + n.Total + "个");
                    if (act == "me")
                    {
                        builder.Append("/剩" + n.Totall + "个");
                    }
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
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">返回拾物首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 兑换礼品提示 ChangePage
    private void ChangePage()
    {
        Master.Title = "兑换礼品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("+兑换礼品+");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=1") + "\">3个x33种共99个物品=8000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("2.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=2") + "\">2个x33种共66个物品=3000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("3.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=3") + "\">1个x33种共33个物品=300" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("4.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=4") + "\">不同物品32个物品=300" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("5.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=5") + "\">不同物品30个物品=200" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("6.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=6") + "\">不同物品25个物品=200" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("7.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=7") + "\">不同物品20个物品=80" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("8.<a href=\"" + Utils.getUrl("flows.aspx?act=changegift") + "\">不同物品15个物品=商城礼物1个</a><br />");
        builder.Append("9.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=9") + "\">不同物品10个物品=1000" + ub.Get("SiteBz2") + "</a><br />");

        builder.Append("10.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=10") + "\">物品总数5000个=8000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("11.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=11") + "\">物品总数3000个=5000" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("12.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=12") + "\">物品总数2000个=二个月VIP</a><br />");
        builder.Append("13.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=13") + "\">物品总数1500个=一个月VIP</a><br />");

        builder.Append("14.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=14") + "\">物品总数1200个=1500" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("15.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=15") + "\">物品总数1000个=1200" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("16.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=16") + "\">物品总数800个=300" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("17.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=17") + "\">物品总数500个=2000" + ub.Get("SiteBz2") + "</a><br />");
        builder.Append("18.<a href=\"" + Utils.getUrl("flows.aspx?act=changeinfo&amp;p=18") + "\">物品总数300个=1000" + ub.Get("SiteBz2") + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">返回拾物首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("/bbs/default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 兑换礼品 ChangeInfoPage
    private void ChangeInfoPage()
    {
        Master.Title = "兑换礼品";

        #region 判断登陆状态
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        #endregion

        #region 兑换类型判断
        int p = int.Parse(Utils.GetRequest("p", "all", 2, @"^[1-9]\d*$", "兑换类型错误"));
        if (p > 18)
        {
            Utils.Error("兑换类型错误", "");
        }

        if (p == 8)
        {
            Utils.Error("兑换类型错误", "");
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("您的物品总量:" + new BCW.BLL.Game.GiftFlows().GetTotal(meid) + "/剩余:" + new BCW.BLL.Game.GiftFlows().GetTotall(meid) + "个");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=me") + "\">&gt;&gt;详细</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        #endregion

        #region 判断数量
        bool Isbl = true;
        if (p == 1 || p == 2 || p == 3)
        {
            int num = 0;
            if (p == 1)
            {
                builder.Append("您选择兑换:3个x33种共99个物品=8000" + ub.Get("SiteBz") + "");
                num = 3;
            }
            else if (p == 2)
            {
                builder.Append("您选择兑换:2个x33种共66个物品=3000" + ub.Get("SiteBz") + "");
                num = 2;
            }
            else
            {
                builder.Append("您选择兑换:1个x33种共33个物品=300" + ub.Get("SiteBz") + "");
                num = 1;
            }
            //判断是否有33个不同物品
            for (int i = 1; i <= 33; i++)
            {
                if (!new BCW.BLL.Game.GiftFlows().Exists(i, meid, num))
                {
                    Isbl = false;
                    break;
                }
            }
        }
        else if (p == 4)
        {
            builder.Append("您选择兑换:不同物品32个物品=300" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 32)
                Isbl = false;
        }
        else if (p == 5)
        {
            builder.Append("您选择兑换:不同物品30个物品=200" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 30)
                Isbl = false;
        }
        else if (p == 6)
        {
            builder.Append("您选择兑换:不同物品25个物品=200" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 25)
                Isbl = false;
        }
        else if (p == 7)
        {
            builder.Append("您选择兑换:不同物品20个物品=80" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 20)
                Isbl = false;
        }
        else if (p == 8)
        {
            builder.Append("您选择兑换:不同物品15个物品=商城礼物1个");
            if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 15)
                Isbl = false;
        }
        else if (p == 9)
        {
            builder.Append("您选择兑换:不同物品10个物品=1000" + ub.Get("SiteBz2") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 10)
                Isbl = false;
        }
        else if (p == 10)
        {
            builder.Append("您选择兑换:物品总数5000个=8000" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 5000)
                Isbl = false;
        }
        else if (p == 11)
        {
            builder.Append("您选择兑换:物品总数3000个=5000" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 3000)
                Isbl = false;
        }
        else if (p == 12)
        {
            builder.Append("您选择兑换:物品总数2000个=二个月VIP");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 2000)
                Isbl = false;
        }
        else if (p == 13)
        {
            builder.Append("您选择兑换:物品总数1500个=一个月VIP");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 1500)
                Isbl = false;
        }
        else if (p == 14)
        {
            builder.Append("您选择兑换:物品总数1200个=1500" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 1200)
                Isbl = false;
        }
        else if (p == 15)
        {
            builder.Append("您选择兑换:物品总数1000个=1200" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 1000)
                Isbl = false;
        }
        else if (p == 16)
        {
            builder.Append("您选择兑换:物品总数800个=300" + ub.Get("SiteBz") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 800)
                Isbl = false;
        }
        else if (p == 17)
        {
            builder.Append("您选择兑换:物品总数500个=2000" + ub.Get("SiteBz2") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 500)
                Isbl = false;
        }
        else if (p == 18)
        {
            builder.Append("您选择兑换:物品总数300个=1000" + ub.Get("SiteBz2") + "");
            if (new BCW.BLL.Game.GiftFlows().GetTotall(meid) < 300)
                Isbl = false;
        }
        #endregion

        builder.Append(Out.Tab("</div>", Out.Hr()));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            #region 确认兑换
            if (!Isbl)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("很遗憾，您的物品量还没有达到兑换所需的条件");
                builder.Append(Out.Tab("</div>", ""));

            }
            else
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("恭喜，您的物品量已达到兑换所需的条件");
                builder.Append(Out.Tab("</div>", ""));
                //if (p == 1 || p == 2 || p == 10 | p == 11)
                //{
                //    string strText = "填写充值到的手机号:/,再次输入手机号;/,,,";
                //    string strName = "Notes,Notes2,p,act,info";
                //    string strType = "text,text,hidden,hidden,hidden";
                //    string strValu = "''" + p + "'changeinfo'ok";
                //    string strEmpt = "false,false,false,false,false";
                //    string strIdea = "/";
                //    string strOthe = "申请兑换,flows.aspx,post,1,red";
                //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                //}
                //else
                //{
                builder.Append(Out.Tab("", "<br />"));
                string strName = "p,act,info";
                string strValu = "" + p + "'changeinfo'ok";
                string strOthe = "确认兑换,flows.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
                //}
            }
            #endregion
        }
        else
        {
            #region 兑换过程
            if (!Isbl)
            {
                Utils.Error("很遗憾，您的物品量还没有达到兑换所需的条件", "");
            }

            #region 变量
            string mename = new BCW.BLL.User().GetUsName(meid);
            string Notes = string.Empty;
            int State = 1;
            //if (p == 1 || p == 2 || p == 10 || p == 11)
            //{
            //    Notes = Utils.GetRequest("Notes", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入手机号");
            //    string Notes2 = Utils.GetRequest("Notes2", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入手机号");
            //    if (Notes != Notes2)
            //    {
            //        Utils.Error("您的手机号输入不一致，请重新输入", "");
            //    }
            //    State = 0;
            //}
            #endregion

            #region 前三种兑换 33个物品一起减去相应数量
            if (p == 1 || p == 2 || p == 3)
            {
                int num = 0;
                if (p == 1)
                    num = 3;
                else if (p == 2)
                    num = 2;
                else
                    num = 1;

                //33个物品一起减去相应数量
                for (int i = 1; i <= 33; i++)
                {
                    new BCW.BLL.Game.GiftFlows().UpdateTotall(i, meid, -num);
                }
                if (p == 1)
                {
                    ///30000酷币改8000
                    new BCW.BLL.User().UpdateiGold(meid, mename, 8000, "兑换物品获得");
                }
                if (p == 2)
                {
                    ///20000酷币改3000
                    new BCW.BLL.User().UpdateiGold(meid, mename, 3000, "兑换物品获得");
                }
                if (p == 3)
                {
                    ///800酷币改300
                    new BCW.BLL.User().UpdateiGold(meid, mename, 300, "兑换物品获得");
                }
            }
            #endregion

            #region 4-9种兑换处理
            else if (p >= 4 && p <= 9)
            {
                int num = 0;
                if (p == 4)
                {
                    ///800酷币改300
                    new BCW.BLL.User().UpdateiGold(meid, mename, 300, "兑换物品获得");
                    num = 32;
                }
                else if (p == 5)
                {
                    ///600酷币改200
                    new BCW.BLL.User().UpdateiGold(meid, mename, 200, "兑换物品获得");
                    num = 30;
                }
                else if (p == 6)
                {
                    ///500酷币改200
                    new BCW.BLL.User().UpdateiGold(meid, mename, 200, "兑换物品获得");
                    num = 25;
                }
                else if (p == 7)
                {
                    ///100酷币改80
                    new BCW.BLL.User().UpdateiGold(meid, mename, 80, "兑换物品获得");
                    num = 20;
                }
                else if (p == 8)
                {
                    //商城礼品一个
                    num = 15;
                }
                else
                {
                    //1000爆谷
                    new BCW.BLL.User().UpdateiMoney(meid, mename, 1000, "兑换物品获得");
                    num = 10;
                }

                int k = 0;
                for (int i = 1; i <= 33; i++)
                {
                    int rac = new BCW.BLL.Game.GiftFlows().UpdateTotall(i, meid, -1);
                    if (rac > 0)
                        k++;

                    if (k == num)
                        break;
                }
            }
            #endregion

            #region 10-18兑换处理
            else
            {
                int num = 0;
                if (p == 10)
                {
                    ///50000酷币8000
                    new BCW.BLL.User().UpdateiGold(meid, mename, 8000, "兑换物品获得");
                    num = 5000;
                }
                else if (p == 11)
                {
                    ///30000酷币改5000
                    new BCW.BLL.User().UpdateiGold(meid, mename, 5000, "兑换物品获得");
                    num = 3000;
                }
                else if (p == 12)
                {
                    //VIP二个月
                    num = 2000;
                }
                else if (p == 13)
                {
                    //VIP一个月
                    num = 1500;
                }
                else if (p == 14)
                {
                    ///2000酷币改1500
                    new BCW.BLL.User().UpdateiGold(meid, mename, 1500, "兑换物品获得");
                    num = 1200;
                }
                else if (p == 15)
                {
                    ///1000酷币改1200
                    new BCW.BLL.User().UpdateiGold(meid, mename, 1200, "兑换物品获得");
                    num = 1000;
                }
                else if (p == 16)
                {
                    ///800酷币改300
                    new BCW.BLL.User().UpdateiGold(meid, mename, 300, "兑换物品获得");
                    num = 800;
                }
                else if (p == 17)
                {
                    ///10000爆谷改2000
                    new BCW.BLL.User().UpdateiMoney(meid, mename, 2000, "兑换物品获得");
                    num = 500;
                }
                else
                {
                    ///6000爆谷改1000
                    new BCW.BLL.User().UpdateiMoney(meid, mename, 1000, "兑换物品获得");
                    num = 300;
                }

                if (p == 12 || p == 13)//兑换VIP
                {
                    BCW.Model.User vipmodel = new BCW.BLL.User().GetVipData(meid);
                    int Grow = 0;
                    int Day = 0;
                    if (p == 12)
                    {
                        Grow = 8;
                        Day = 60;
                    }
                    else
                    {
                        Grow = 8;
                        Day = 30;
                    }
                    if (vipmodel.VipDate != null && vipmodel.VipDate > DateTime.Now)
                        new BCW.BLL.User().UpdateVipData(meid, Grow, vipmodel.VipDate.AddDays(Day));
                    else
                        new BCW.BLL.User().UpdateVipData(meid, Grow, DateTime.Now.AddDays(Day));

                    //清缓存
                    string CacheKey = CacheName.App_UserVip(meid);
                    DataCache.RemoveByPattern(CacheKey);
                }

                DataSet ds = BCW.Data.SqlHelper.Query("SELECT ID, Totall FROM tb_GiftFlows Where UsID=" + meid + " and Totall>0 ORDER by ID ASC");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        int getTotal = int.Parse(ds.Tables[0].Rows[i]["Totall"].ToString());

                        if (getTotal <= num)
                        {
                            new BCW.BLL.Game.GiftFlows().UpdateTotall(ID, -getTotal);
                        }
                        else
                        {
                            new BCW.BLL.Game.GiftFlows().UpdateTotall(ID, -num);
                        }
                        num = num - getTotal;

                        if (num <= 0)
                            break;
                    }
                }
            }
            #endregion

            #region 写入兑换记录
            //写入兑换记录
            BCW.Model.Game.GiftChange model = new BCW.Model.Game.GiftChange();
            model.UsID = meid;
            model.UsName = mename;
            model.Types = p;
            model.State = State;
            model.Notes = Notes;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Game.GiftChange().Add(model);

            //if (p == 1 || p == 2 || p == 10 || p == 11)
            //{
            //    //通知客服10086

            //    new BCW.BLL.Guest().Add(10086, "客服", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]兑换了周年拾物活动-话费卡，请在后台进行处理");

            //    Utils.Success("兑换礼品", "恭喜，兑换成功，礼品属于话费卡，请留意系统给您的完成充值的内线通知<br /><a href=\"" + Utils.getUrl("flows.aspx?act=change") + "\">&gt;&gt;继续兑换礼品</a>", Utils.getUrl("flows.aspx"), "3");
            //}
            //else
            //{
            Utils.Success("兑换礼品", "恭喜，兑换成功！<br /><a href=\"" + Utils.getUrl("flows.aspx?act=change") + "\">&gt;&gt;继续兑换礼品</a>", Utils.getUrl("flows.aspx"), "3");
            //}
            #endregion

            #endregion
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=change") + "\">&gt;&gt;重新选择兑换</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 兑换商城礼物 ChangeGiftPage
    private void ChangeGiftPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (new BCW.BLL.Game.GiftFlows().GetTypesTotal(meid) < 15)//15
        {
            Utils.Error("很遗憾，您的物品量还没有达到兑换所需的条件", "");
        }
        int NodeId = 27;
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info == "ok")
        {
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            BCW.Model.Shopgift model = new BCW.BLL.Shopgift().GetShopgift(id);
            int num = 1;
            if (model == null)
            {
                Utils.Error("不存在的商品记录", "");
            }
            if (model.NodeId != NodeId)
            {
                Utils.Error("不存在的记录", "");
            }
            if (model.Total != -1 && model.Total < num)
            {
                Utils.Error("商品库存不足", "");
            }
            //得到昵称
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.Shopkeep keep = new BCW.Model.Shopkeep();
            keep.GiftId = id;
            keep.Title = model.Title.Trim();
            keep.Pic = model.Pic;
            keep.PrevPic = model.PrevPic;
            keep.Notes = model.Notes;
            keep.IsSex = model.IsSex;
            keep.Para = model.Para;
            keep.UsID = meid;
            keep.UsName = mename;
            keep.Total = num;
            keep.TopTotal = num;
            keep.AddTime = DateTime.Now;
            //邵广林 20161128 增加27为空的字段
            keep.MerBillNo = "";
            keep.NodeId = NodeId;
            keep.GatewayType = "";
            keep.Attach = "";
            keep.GoodsName = "";
            keep.IsCredit = "";
            keep.BankCode = "";
            keep.ProductType = "";
            if (!new BCW.BLL.Shopkeep().Exists(id, meid))
            {
                new BCW.BLL.Shopkeep().Add(keep);
            }
            else
            {
                new BCW.BLL.Shopkeep().Update(keep);
            }
            //购买库存与出售数量
            int num2 = -num;
            if (model.Total == -1)
            {
                num2 = 0;
            }
            new BCW.BLL.Shopgift().Update(id, num, num2);
            //更新此分类出售数量
            new BCW.BLL.Shoplist().Update(id, num);

            //扣活动礼物
            int k = 0;
            for (int i = 1; i <= 33; i++)
            {
                int rac = new BCW.BLL.Game.GiftFlows().UpdateTotall(i, meid, -1);
                if (rac > 0)
                    k++;

                if (k == 15)
                    break;
            }
            //写入兑换记录
            BCW.Model.Game.GiftChange change = new BCW.Model.Game.GiftChange();
            change.UsID = meid;
            change.UsName = mename;
            change.Types = 8;
            change.State = 1;
            change.Notes = "兑换了" + model.Title.Trim() + "";
            change.AddTime = DateTime.Now;
            new BCW.BLL.Game.GiftChange().Add(change);

            string wText = "在[url=/bbs/game/flows.aspx]活动礼品[/url]兑换了[url=/bbs/bbsshop.aspx?act=giftview&amp;id=" + id + "]" + model.Title.Trim() + "[img]" + model.PrevPic + "[/img][/url]";
            new BCW.BLL.Action().Add(12, id, meid, mename, wText);

            Utils.Success("兑换商城礼物", "兑换商城礼物成功<br /><a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=store") + "\">&gt;我的储物箱</a>", Utils.getUrl("flows.aspx"), "3");

        }
        Master.Title = "活动礼品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;活动礼品");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = 5;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "NodeId=" + NodeId + "";
        strOrder = "ID DESC";

        // 开始读取专题
        IList<BCW.Model.Shopgift> listShopgift = new BCW.BLL.Shopgift().GetShopgifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listShopgift.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shopgift n in listShopgift)
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
                builder.Append("<img src=\"" + n.Pic + "\" alt=\"load\"/>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?act=giftview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=changegift&amp;info=ok&amp;&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[兑换]</a>");

                builder.Append("<br />" + OutMei(n.Para));

                //builder.Append(" 售价:" + n.Price + "");
                if (n.BzType == 0)
                    builder.Append(ub.Get("SiteBz"));
                else
                    builder.Append(ub.Get("SiteBz2"));

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

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx?act=change") + "\">&gt;&gt;重新选择兑换</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("flows.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 属性参数 OutMei
    private string OutMei(string Para)
    {
        //属性参数（积分|体力|魅力|智慧|威望|邪恶)写入如:0|0|0|0|0|0）
        string str = string.Empty;
        if (Para != "")
        {
            try
            {
                string[] name = { "积分", "体力", "魅力", "智慧", "威望", "邪恶" };
                string[] temp = Para.Split("|".ToCharArray());
                for (int i = 0; i < temp.Length; i++)
                {
                    int ii = Convert.ToInt32(temp[i]);
                    if (ii != 0)
                    {
                        if (ii > 0)
                            str += "," + name[i] + "+" + temp[i];
                        else
                            str += "," + name[i] + "" + temp[i];
                    }
                }
            }
            catch
            {
                str = ",参数错误";
            }
        }
        if (!string.IsNullOrEmpty(str))
            str = Utils.Mid(str, 1, str.Length);

        return str;
    }
    #endregion

}