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
using TPR2.Common;

public partial class bbs_guess_kzlist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "kz":
                KzPage();
                break;
            case "view":
                ViewPage();
                break;
            case "pay":
                PayPage();
                break;
            case "plist":
                PListPage();
                break;
            case "wlist":
                WListPage();
                break;
            case "del":
                DelPage();
                break;
            case "mykz":
                MyKzPage();
                break;
            case "isdel":
                IsDelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "个人庄列表";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
        {
            if (uid == 0)
            {
                builder.Append("【全部个人庄】");
                builder.Append(Out.waplink(Utils.getUrl("kzcaseGuess.aspx"), "闲兑奖") + "|");
                builder.Append("" + Out.waplink(Utils.getUrl("kzcaseGuess2.aspx"), "庄兑奖") + "");
            }
            else
            {
                builder.Append("【" + new BCW.BLL.User().GetUsName(uid) + "-个人庄】");
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx"), "全部") + " ");
            }
        }
        else
        {
            builder.Append("【我的个人庄】<br />");

            if (showtype == 0)
                builder.Append("未结束|");
            else
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?ptype=1&amp;showtype=0"), "未结束") + "|");

            if (showtype == 1)
                builder.Append("历史");
            else
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?ptype=1&amp;showtype=1"), "历史") + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        int pageIndex;
        int recordCount;
        string strDay = "";
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 0)
            strWhere = "p_del=0";
        else
            strWhere = "p_del<=1";

        if (uid > 0)
            strWhere += " and usid = " + uid + "";
        else
        {
            if (ptype == 1)
                strWhere += " and usid = " + meid + "";
        }

        if (ptype == 1)
        {
            if (showtype == 0)
                strWhere += "and p_active=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR p_ison=1)";
            else
                strWhere += "";

        }
        else
        {
            strWhere += "and p_active=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR p_ison=1)";

        }
        string strOrder = "p_TPRtime DESC,ID DESC";
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaListMe> listBaListMe = new TPR2.BLL.guess.BaListMe().GetBaListMes(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBaListMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaListMe n in listBaListMe)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string IsNEW = "";
                if (n.p_ison == 1)
                    IsNEW = "走地";
                else if (n.p_ison == 2)
                    IsNEW = "固定水位";
                else
                    IsNEW = "自动水位";

                if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                    builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                builder.AppendFormat("<a href=\"" + Utils.getUrl("kzlist.aspx?act=view&amp;gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}VS{4}[{5}]</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two, IsNEW);
                if (n.p_active == 0)
                {
                    long getMaxNum = new TPR2.BLL.guess.BaPayMe().GetBaPayMeCent(n.ID, Convert.ToInt32(n.p_type));
                    builder.Append("(还可以下注" + (n.payCent - getMaxNum) + "" + ub.Get("SiteBz") + ")");
                }
                if (n.p_active > 0)
                {
                    if (n.p_active == 2)
                        builder.Append("(已平盘)");
                    else
                    {
                        if (n.p_result_one != null && n.p_result_two != null)
                            builder.Append("(比分:" + n.p_result_one + ":" + n.p_result_two + ")");
                    }
                }

                builder.Append(Out.Tab("</div>", ""));

                strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
                k++;
            }

            // 分页

            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (ptype == 0)
        {
            string strText = "输入会员ID:/,";
            string strName = "uid,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜个人庄,kzlist.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 0)
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?ptype=1"), "我是庄家&gt;&gt;") + "<br />");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx"), "全部个人庄&gt;&gt;") + "<br />");

        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=kz"), "我要开庄&gt;&gt;") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "竞猜ID无效"));

        TPR2.BLL.guess.BaListMe bll = new TPR2.BLL.guess.BaListMe();
        TPR2.Model.guess.BaListMe st = bll.GetModel(gid);
        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //-----------------------------立即更新水位---------------------------------
        if (st.p_active == 0)
        {
            if (st.p_type == 1)
            {
                if (st.p_ison == 1)
                    new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), true);
                else
                    new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), false);
            }
            else
            {
                if (st.p_ison == 1)
                    new TPR2.Collec.Basketbo().GetBoView(Convert.ToInt32(st.p_id), true);
                else
                    new TPR2.Collec.Basketbo().GetBoView(Convert.ToInt32(st.p_id), false);
            }
        }
        TPR2.Model.guess.BaListMe model = bll.GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = model.p_one + "VS" + model.p_two;

        if (Request["info"] == "once")
        {
            //builder.Append(Out.Tab("<div class=\"title\">", ""));
            //builder.Append("走地比分参考");
            //builder.Append(Out.Tab("</div>", "<br />"));
            //builder.Append(Out.Tab("<div>", ""));
            //string stronce = new TPR2.BLL.guess.BaListMe().Getp_temptimes(gid);
            //if (stronce != "")
            //{
            //    builder.Append("" + stronce.Replace("|", "<br />") + "");
            //}
            //else
            //{
            //    builder.Append("暂无比分记录..");
            //}
            //builder.Append(Out.Tab("</div>", Out.Hr()));
            //builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "返回上级") + "<br />");
        }
        else
        {
            //根据会员本身的ID取会员实体
            int meid = new BCW.User.Users().GetUsId();
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(model.usid) + "(" + model.usid + ")</a>");

            builder.Append("<br /><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=4&amp;fly=" + model.p_title + "") + "\">" + model.p_title + "</a>:" + model.p_one + "VS" + model.p_two + "");

            builder.Append("<br />开赛:" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0));
            //if (model.p_result_one != null && model.p_result_two != null)
            //{
            //    if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
            //        builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=view&amp;info=once&amp;gid=" + model.ID + ""), "完场比分：" + model.p_result_one + ":" + model.p_result_two + ""));
            //    else
            //        builder.Append("<br />完场比分：" + model.p_result_one + ":" + model.p_result_two + "");
            //}
            //else
            //{
            //    if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
            //        builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=view&amp;info=once&amp;gid=" + model.ID + ""), "即时比分：" + model.p_result_temp1 + ":" + model.p_result_temp2 + ""));
            //}

            if (model.p_result_one != null && model.p_result_two != null)
            {
                builder.Append("<br />完场比分:" + model.p_result_one + ":" + model.p_result_two + "");
            }
            else
            {
                builder.Append("<br />比赛状态:" + model.p_once + "");
                builder.Append("<br />即时比分:" + model.p_result_temp1 + ":" + model.p_result_temp2 + "");


            }

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("〓让球盘〓");
            if (model.p_ison == 1 && model.p_isluckone == 1 && model.p_active == 0)
                builder.Append("<b>(封)</b>");

            if (model.p_one_lu != -1)
                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=1"), model.p_one + "(" + Convert.ToDouble(model.p_one_lu) + ")"));
            else
                builder.Append(model.p_one);

            if (model.usid == meid)
            {
                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=1"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 1));

            }
            if (model.p_type == 1)
                builder.Append("<br />" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
            else
                builder.Append("<br />" + Convert.ToDouble(model.p_pk) + "");

            if (model.p_two_lu != -1)
                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=2"), model.p_two + "(" + Convert.ToDouble(model.p_two_lu) + ")"));
            else
                builder.Append("<br />" + model.p_two);

            if (model.usid == meid)
            {
                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=2"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 2));
            }
            if (model.p_dx_pk != 0)
            {
                builder.Append("<br />〓大小盘〓<br />");
                if (model.p_ison == 1 && model.p_islucktwo == 1 && model.p_active == 0)
                    builder.Append("<b>(封)</b>");

                if (model.usid == meid)
                {
                    builder.AppendFormat("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=3"), "{0}注") + "{1}", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 3), "|");
                }
                if (model.p_big_lu != -1)
                    builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=3"), "大(" + Convert.ToDouble(model.p_big_lu) + ")"));
                else
                    builder.Append("大|");

                if (model.p_type == 1)
                    builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
                else
                    builder.Append(Convert.ToDouble(model.p_dx_pk));

                if (model.p_small_lu != -1)
                    builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=4"), "小(" + Convert.ToDouble(model.p_small_lu) + ")"));
                else
                    builder.Append("|小");

                if (model.usid == meid)
                {
                    builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=4"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 4));
                }
            }

            builder.Append("<br />〓标准盘〓");
            if (model.p_ison == 1 && model.p_isluckthr == 1 && model.p_active == 0)
                builder.Append("<b>(封)</b>");

            if (model.p_bzs_lu != -1)
                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=5"), "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")"));
            else
                builder.Append("<br />主胜");

            if (model.usid == meid)
            {
                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=5"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 5));
            }
            if (model.p_bzp_lu != -1)
                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=6"), "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")"));
            else
                builder.Append("<br />平手");

            if (model.usid == meid)
            {
                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=6"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 6));
            }
            if (model.p_bzx_lu != -1)
                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=7"), "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")"));
            else
                builder.Append("<br />客胜");

            if (model.usid == meid)
            {
                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=7"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 7));
            }

            builder.Append(Out.Tab("</div>", ""));
            if (model.usid == meid)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("【" + Out.waplink(Utils.getUrl("kzopenguess.aspx?gid=" + gid + ""), "开奖") + " ");
                builder.Append("" + Out.waplink(Utils.getUrl("kzguess2.aspx?gid=" + gid + ""), "编辑") + " ");
                builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=del&amp;gid=" + gid + ""), "删除") + " ");
                if (model.p_del == 0)
                    builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=isdel&amp;gid=" + gid + "&amp;ptype=1"), "隐藏") + " ");
                else
                    builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=isdel&amp;gid=" + gid + "&amp;ptype=0"), "显示") + " ");

                builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=isdel&amp;gid=" + gid + "&amp;ptype=2"), "封盘") + " ");

                builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=wlist&amp;gid=" + gid + ""), "记录") + "】");

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx"), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        long gold = new BCW.BLL.User().GetGold(meid);
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "all", 2, @"^[1-7]*$", "选择无效"));

        TPR2.BLL.guess.BaListMe BaListbll = new TPR2.BLL.guess.BaListMe();

        TPR2.Model.guess.BaListMe st = BaListbll.GetModel(gid);
        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //-----------------------------立即更新水位---------------------------------
        if (st.p_active == 0)
        {
            if (st.p_type == 1)
            {
                if (st.p_ison == 1)
                    new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), true);
                else
                    new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), false);
            }
            else
            {
                if (st.p_ison == 1)
                    new TPR2.Collec.Basketbo().GetBoView(Convert.ToInt32(st.p_id), true);
                else
                    new TPR2.Collec.Basketbo().GetBoView(Convert.ToInt32(st.p_id), false);
            }
        }

        TPR2.Model.guess.BaListMe modelBaList = BaListbll.GetModel(gid);

        if (modelBaList.p_del == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        if (modelBaList.p_active != 0)
        {
            Utils.Error("已完场或已结束", "");
        }
        Master.Title = modelBaList.p_one + "VS" + modelBaList.p_two;

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        if (ac != "")
        {
            if (p == 1 && modelBaList.p_one_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }
            if (p == 2 && modelBaList.p_two_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }
            if (p == 3 && modelBaList.p_big_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }
            if (p == 4 && modelBaList.p_small_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }
            if (p == 5 && modelBaList.p_bzs_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }
            if (p == 6 && modelBaList.p_bzp_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }
            if (p == 7 && modelBaList.p_bzx_lu == -1)
            {
                Utils.Error("该类型当前不允许下注", "");
            }

            if (p == 1 || p == 2)
            {
                if (modelBaList.p_ison == 1 && modelBaList.p_isluckone == 1)
                {
                    Utils.Error("上下盘已封，请稍后再试", "");
                }

            }

            if (p == 3 || p == 4)
            {
                if (modelBaList.p_dx_pk == 0)
                {
                    Utils.Error("不存在的大小球下注", "");
                }
                if (modelBaList.p_ison == 1 && modelBaList.p_islucktwo == 1)
                {
                    Utils.Error("大小盘已封，请稍后再试", "");
                }
            }
            if (p > 4)
            {
                if (modelBaList.p_ison == 1 && modelBaList.p_isluckthr == 1)
                {
                    Utils.Error("标准盘已封，请稍后再试", "");
                }
            }
            int payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
            if (modelBaList.p_ison != 1)//走地不限制
            {
                if (modelBaList.p_TPRtime <= DateTime.Now)
                {
                    Utils.Error("开赛时间已到，暂停下注", "");
                }
            }
            if (modelBaList.p_ison == 1)
            {
                if (Convert.ToDateTime(modelBaList.p_temptime) != DateTime.Parse("1990-1-1"))
                {
                    if (modelBaList.p_type == 1)
                    {
                        if (DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStemp", xmlPath))))
                        {
                            Utils.Error("比分正在变化或水位大幅度调整中，请稍后下注", "");
                        }
                    }
                    else
                    {
                        if (DateTime.Now < Convert.ToDateTime(modelBaList.p_temptime).AddSeconds(Convert.ToInt32(ub.GetSub("SiteStempb", xmlPath))))
                        {
                            Utils.Error("水位大幅度调整中，请稍后下注", "");
                        }
                    }
                }
                if (DateTime.Now > Convert.ToDateTime(modelBaList.p_oncetime))
                {
                    Utils.Error("已封盘，暂停下注", "");
                }
                if (modelBaList.p_once == "完")
                {
                    Utils.Error("已封盘，暂停下注", "");
                }
            }
            if (gold < Convert.ToInt64(payCent))
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不够此次下注", "");
            }
            //支付安全提示
            string[] p_pageArr = { "act", "ac", "gid", "payCent", "p" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            //是否刷屏
            long small = Convert.ToInt64(ub.GetSub("SiteSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("SiteBigPay", xmlPath));
            string appName = "LIGHT_GUESS";
            int Expir = Utils.ParseInt(ub.GetSub("SiteExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir, Convert.ToInt64(payCent), small, big);

            long setMaxNum = modelBaList.payCent;
            long getMaxNum = new TPR2.BLL.guess.BaPayMe().GetBaPayMeCent(gid, Convert.ToInt32(modelBaList.p_type));
            if (getMaxNum + Convert.ToInt64(payCent) > setMaxNum)
            {
                if (getMaxNum >= setMaxNum)
                {
                    Utils.Error("庄家限制本场下注上限" + setMaxNum + "" + ub.Get("SiteBz") + "，欢迎在下场下注", "");
                }
                else
                {
                    Utils.Error("庄家限制本场下注上限" + setMaxNum + "" + ub.Get("SiteBz") + "，你现在最多可以下注" + (setMaxNum - getMaxNum) + "" + ub.Get("SiteBz") + "", "");
                }
            }

            //计算赔付并扣除庄家该下注的押金
            long zPrice = 0;
            if (p == 1)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_one_lu - payCent);
            else if (p == 2)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_two_lu - payCent);
            else if (p == 3)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_big_lu - payCent);
            else if (p == 4)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_small_lu - payCent);
            else if (p == 5)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_bzs_lu - payCent);
            else if (p == 6)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_bzp_lu - payCent);
            else if (p == 7)
                zPrice = Convert.ToInt64(payCent * modelBaList.p_bzx_lu - payCent);

            long tagold = new BCW.BLL.User().GetGold(modelBaList.usid);
            if (tagold < zPrice)
            {
                Utils.Error("庄家" + ub.Get("SiteBz") + "不足，无法下注", "");
            }
            new BCW.BLL.User().UpdateiGold(modelBaList.usid, -zPrice, "个人庄扣除赔付保证金");

            //组合显示串
            string payview = "";
            if (modelBaList.p_type == 1)
            {
                if (p == 1 || p == 2)
                    payview += "[url=/bbs/guess2/kzlist.aspx?act=view&amp;gid=" + gid + "]" + modelBaList.p_one + "(" + GCK.getZqPn(Convert.ToInt32(modelBaList.p_pn)) + GCK.getPkName(Convert.ToInt32(modelBaList.p_pk)) + ")" + modelBaList.p_two + "[/url]";
                else if (p == 3 || p == 4)
                    payview += "[url=/bbs/guess2/kzlist.aspx?act=view&amp;gid=" + gid + "]" + modelBaList.p_one + "(大小球" + GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk)) + ")" + modelBaList.p_two + "[/url]";
                else
                    payview += "[url=/bbs/guess2/kzlist.aspx?act=view&amp;gid=" + gid + "]" + modelBaList.p_one + "(主" + Convert.ToDouble(modelBaList.p_bzs_lu) + "|平" + Convert.ToDouble(modelBaList.p_bzp_lu) + "|客" + Convert.ToDouble(modelBaList.p_bzx_lu) + ")" + modelBaList.p_two + "[/url]";
            }
            else
            {
                if (p == 1 || p == 2)
                    payview += "[url=/bbs/guess2/kzlist.aspx?act=view&amp;gid=" + gid + "]" + modelBaList.p_one + "(" + Convert.ToDouble(modelBaList.p_pk) + ")" + modelBaList.p_two + "[/url]";
                else
                    payview += "[url=/bbs/guess2/kzlist.aspx?act=view&amp;gid=" + gid + "]" + modelBaList.p_one + "(大小球" + Convert.ToDouble(modelBaList.p_dx_pk).ToString() + ")" + modelBaList.p_two + "[/url]";

            }
            string Sison = string.Empty;
            if (modelBaList.p_ison == 1)//购买走地赛事时显示当时比分
            {
                Sison = "(" + modelBaList.p_result_temp1 + ":" + modelBaList.p_result_temp2 + ")";
            }
            if (p == 1)
                payview += "押" + modelBaList.p_one + "(" + Convert.ToDouble(modelBaList.p_one_lu) + ")" + Sison + ",投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";
            if (p == 2)
                payview += "押" + modelBaList.p_two + "(" + Convert.ToDouble(modelBaList.p_two_lu) + ")" + Sison + ",投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";
            if (p == 3)
                payview += "押大球(" + Convert.ToDouble(modelBaList.p_big_lu) + "),投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";
            if (p == 4)
                payview += "押小球(" + Convert.ToDouble(modelBaList.p_small_lu) + "),投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";
            if (p == 5)
                payview += "押主胜(" + Convert.ToDouble(modelBaList.p_bzs_lu) + "),投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";
            if (p == 6)
                payview += "押平手(" + Convert.ToDouble(modelBaList.p_bzp_lu) + "),投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";
            if (p == 7)
                payview += "押客胜(" + Convert.ToDouble(modelBaList.p_bzx_lu) + "),投" + payCent + "" + Utils.ToSChinese(ac).Replace("下注", "") + "";


            //写入bapay
            TPR2.Model.guess.BaPayMe model = new TPR2.Model.guess.BaPayMe();
            model.Types = 0;
            model.payview = payview;
            model.payusid = meid;
            model.payusname = new BCW.BLL.User().GetUsName(meid);
            model.bcid = gid;
            model.pType = modelBaList.p_type;
            model.PayType = p;
            model.payCent = payCent;

            if (p == 1 || p == 2)
            {
                model.payonLuone = modelBaList.p_one_lu;
                model.payonLutwo = modelBaList.p_two_lu;
                model.payonLuthr = 0;
            }
            else if (p == 3 || p == 4)
            {
                model.payonLuone = modelBaList.p_big_lu;
                model.payonLutwo = modelBaList.p_small_lu;
                model.payonLuthr = 0;
            }
            else
            {
                model.payonLuone = modelBaList.p_bzs_lu;
                model.payonLutwo = modelBaList.p_bzp_lu;
                model.payonLuthr = modelBaList.p_bzx_lu;

            }
            model.p_pk = modelBaList.p_pk;
            if (string.IsNullOrEmpty(modelBaList.p_dx_pk.ToString()))
                modelBaList.p_dx_pk = 0;

            model.p_dx_pk = modelBaList.p_dx_pk;
            model.p_pn = modelBaList.p_pn;
            model.paytimes = DateTime.Now;
            if (modelBaList.p_type == 1)
            {
                model.p_result_temp1 = modelBaList.p_result_temp1;
                model.p_result_temp2 = modelBaList.p_result_temp2;
            }
            else
            {
                model.p_result_temp1 = 0;
                model.p_result_temp2 = 0;
            }
            model.itypes = 0;
            model.usid = modelBaList.usid;
            model.DiffPrice = zPrice;
            if (modelBaList.p_ison == 1)
            {
                model.state = 1;
            }
            int pid = new TPR2.BLL.guess.BaPayMe().Add(model);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(payCent), "个人庄下注记录" + gid + "-" + pid + "");
            if (modelBaList.p_ison == 1)
            {
                Utils.Success("走地下注", "恭喜，下注成功，数秒后您可以查看您的" + Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + "确认你的本次下次是否成功", Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "5");
            }
            else
            {
                Utils.Success("下注", "恭喜，下注成功..", Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "1");
            }
        }
        else
        {
            string getdxpk;
            if (modelBaList.p_type == 1)
                getdxpk = GCK.getDxPkName(Convert.ToInt32(modelBaList.p_dx_pk));
            else
                getdxpk = Convert.ToDouble(modelBaList.p_dx_pk).ToString();

            builder.Append(Out.Tab("<div class=\"title\">", ""));

            if (p == 1)
                builder.Append("上下盘:" + modelBaList.p_one + "赔率:" + Convert.ToDouble(modelBaList.p_one_lu) + "含本金");
            else if (p == 2)
                builder.Append("上下盘:" + modelBaList.p_two + "赔率:" + Convert.ToDouble(modelBaList.p_two_lu) + "含本金");
            else if (p == 3)
                builder.Append("大小盘:大&gt;" + getdxpk + "赔率:" + Convert.ToDouble(modelBaList.p_big_lu) + "含本金");
            else if (p == 4)
                builder.Append("大小盘:小&gt;" + getdxpk + "赔率:" + Convert.ToDouble(modelBaList.p_small_lu) + "含本金");
            else if (p == 5)
                builder.Append("标准盘:主胜&gt;" + modelBaList.p_one + "赔率:" + Convert.ToDouble(modelBaList.p_bzs_lu) + "含本金");
            else if (p == 6)
                builder.Append("标准盘:平手&gt; 赔率:" + Convert.ToDouble(modelBaList.p_bzp_lu) + "含本金");
            else if (p == 7)
                builder.Append("标准盘:客胜&gt;" + modelBaList.p_two + "赔率:" + Convert.ToDouble(modelBaList.p_bzx_lu) + "含本金");

            builder.Append(Out.Tab("</div>", ""));

            string strText = "下注,,,";
            string strName = "payCent,gid,p,act";
            string strType = "num,hidden,hidden,hidden";
            string strValu = "'" + gid + "'" + p + "'pay";
            string strEmpt = "true,true,true,false";
            string strIdea = "/限" + ub.GetSub("SiteSmallPay", xmlPath) + "-" + ub.GetSub("SiteBigPay", xmlPath) + "" + ub.Get("SiteBz") + "/";
            string strOthe = string.Empty;
            strOthe = "" + ub.Get("SiteBz") + "下注,kzlist.aspx,post,0,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");

            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "取消下注"));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx"), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "all", 2, @"^[1-7]*$", "选择无效"));

        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.usid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = model.p_one + "VS" + model.p_two;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.p_one + "VS" + model.p_two);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (p == 1)
            builder.Append("让球盘:<b>主队</b>下注列表");
        else if (p == 2)
            builder.Append("让球盘:<b>客队</b>下注列表");
        else if (p == 3)
            builder.Append("大小盘:<b>大</b>下注列表");
        else if (p == 4)
            builder.Append("大小盘:<b>小</b>下注列表");
        else if (p == 5)
            builder.Append("标准盘:<b>主胜</b>下注列表");
        else if (p == 6)
            builder.Append("标准盘:<b>平手</b>下注列表");
        else if (p == 7)
            builder.Append("标准盘:<b>客胜</b>下注列表");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "gid", "p" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        strWhere += "bcid=" + gid + " and PayType=" + p + "";

        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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

                builder.AppendFormat(Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), ""+n.ID+".{0}({1})") + ":{2}[{3}]", n.payusname, n.payusid, Out.SysUBB(n.payview), n.paytimes);

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx?act=view&amp;gid=" + gid + ""), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void WListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.usid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = model.p_one + "VS" + model.p_two + "-记录";

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
        {

            builder.Append("下注记录|" + Out.waplink(Utils.getUrl("kzlist.aspx?act=wlist&amp;gid=" + gid + "&amp;ptype=1"), "赢输记录"));
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=wlist&amp;gid=" + gid + "&amp;ptype=0"), "下注记录") + "|赢输记录");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "gid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        strWhere += "bcid=" + gid + "";
        if (ptype == 1)
            strWhere += "and p_active<>0";
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMeViews(pageIndex, pageSize, strWhere, ptype, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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
                if (ptype == 0)
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}币,共{3}注", n.payusname, n.payusid, Convert.ToDouble(n.payCents), n.payCount);
                }
                else
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}币,返{3},盈利{4}", n.payusname, n.payusid, Convert.ToDouble(n.payCents), Convert.ToDouble(n.payCount), Convert.ToDouble(n.payCount - n.payCents));

                }
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            if (ptype == 0)
                builder.Append(Out.Div("div", "没有相关记录.."));
            else
                builder.Append(Out.Div("div", "没有记录或赛事并没有结束.."));

        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx?act=view&amp;gid=" + gid + ""), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.usid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "删除记录";
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("只能删除没有押注记录的赛事，删除成功后将退回您的最大押注金额，确定要删除吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?info=ok&amp;act=del&amp;gid=" + gid + ""), "确定删除") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "先留着吧.."));
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            long getMaxNum = new TPR2.BLL.guess.BaPayMe().GetBaPayMeCent(gid, Convert.ToInt32(model.p_type));
            if (getMaxNum > 0)
            {
                Utils.Error("此赛事已经存在押注记录，不能删除", "");
            }
            new TPR2.BLL.guess.BaListMe().Delete(gid);
            Utils.Success("删除", "删除赛事成功..", Utils.getUrl("kzlist.aspx"), "1");
        }
    }

    private void MyKzPage()
    {

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));

        int p_type = Utils.ParseInt(Utils.GetRequest("p_type", "get", 1, @"^[0-2]$", "0"));

        int paytype = Utils.ParseInt(Utils.GetRequest("paytype", "get", 1, @"^[0-3]$", "0"));

        string strTitle = "";
        if (ptype == 1)
            strTitle = "个人庄-未开投注";
        else
            strTitle = "个人庄-历史投注";

        Master.Title = strTitle;

        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));

        builder.Append("球:");
        if (p_type == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=0&amp;paytype=" + paytype + ""), "全部") + " ");

        if (p_type == 1)
            builder.Append("足球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=1&amp;paytype=" + paytype + ""), "足球") + " ");

        if (p_type == 2)
            builder.Append("篮球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=2&amp;paytype=" + paytype + ""), "篮球") + " ");

        builder.Append("<br />盘:");

        if (paytype == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=0"), "全部") + " ");

        if (paytype == 1)
            builder.Append("让球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=1"), "让球") + " ");

        if (paytype == 2)
            builder.Append("大小 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=2"), "大小") + " ");

        if (p_type != 2)
        {
            if (paytype == 3)
                builder.Append("标准 ");
            else
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=3"), "标准") + " ");
        }
        builder.Append(Out.Tab("</div >", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "ptype", "p_type", "paytype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        if (ptype == 1)
            strWhere += "p_active=0 and payusid=" + meid + " and itypes=0 ";
        else
            strWhere += "p_active>0 and payusid=" + meid + " and itypes=0 ";

        if (p_type != 0)
            strWhere += "and ptype=" + p_type + "";

        if (paytype != 0)
        {
            if (paytype == 1)
                strWhere += "and (paytype=1 or paytype=2)";
            if (paytype == 2)
                strWhere += "and (paytype=3 or paytype=4)";
            if (paytype == 3)
                strWhere += "and (paytype=5 or paytype=6 or paytype=7)";
        }
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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

                if (n.p_active == 0)
                {
                    builder.AppendFormat("{0}<br />时间:{1}", Out.SysUBB(n.payview), DT.FormatDate(Convert.ToDateTime(n.paytimes), 0));
                    if (n.state == 1)
                        builder.Append("*待确认");
                }
                else if (n.p_active == 2)
                {
                    builder.AppendFormat("{0},平盘<br />时间:{1}", Out.SysUBB(n.payview), DT.FormatDate(Convert.ToDateTime(n.paytimes), 0));
                    builder.AppendFormat(" 返{0}币", Convert.ToDouble(n.p_getMoney));
                }
                else
                {
                    builder.AppendFormat("{0},结果{1}:{2}<br />时间:{3}", Out.SysUBB(n.payview), n.p_result_one, n.p_result_two, DT.FormatDate(Convert.ToDateTime(n.paytimes), 0));

                    if (Convert.ToInt32(n.p_getMoney) > 0)
                    {
                        builder.AppendFormat(" 返{0}币", Convert.ToDouble(n.p_getMoney));
                    }
                    else
                    {
                        builder.AppendFormat(" 输{0}币", Convert.ToDouble(n.payCent));
                    }
                }

                builder.Append(Out.Tab("</div>", ""));

                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx"), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void KzPage()
    {
        Master.Title = "我要开庄";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-5]$", "0"));
        string fly = "";
        if (ptype == 4)
            fly = Out.UBB(Utils.GetRequest("fly", "get", 2, @"^.+?$", "请选择联赛"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 0)
            builder.Append("全部 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx?act=kz") + "\">全部</a> ");

        if (ptype == 1)
            builder.Append("足球 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx?act=kz&amp;ptype=1") + "\">足球</a> ");

        if (ptype == 2)
            builder.Append("篮球 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx?act=kz&amp;ptype=2") + "\">篮球</a> ");

        if (ptype == 3)
            builder.Append("联赛 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx?act=kz&amp;ptype=3") + "\">联赛</a> ");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strDay = "";
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "fly" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype != 3)
        {
            if (ptype > 0 && ptype < 4)
                strWhere = "p_type=" + ptype + " and p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0))";
            else if (ptype == 4)
                strWhere = "p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0)) and p_title like '%" + fly + "%'";
            else if (ptype == 5)
                strWhere = "p_active=0 and p_del=0 and p_ison=1 and p_isondel=0";
            else if (ptype == 6)
                strWhere = "p_active=0 and p_del=0 and p_TPRtime >= '" + System.DateTime.Now + "' and p_score IS NOT NULL AND p_score<>''";
            else
                strWhere = "p_active=0 and p_del=0  and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0))";

            string strOrder = "p_TPRtime ASC,ID ASC";
            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listBaList.Count > 0)
            {
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaList)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                        builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("showGuess.aspx?gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}VS{4}</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two);
                    builder.Append("[" + Out.waplink(Utils.getPage("kzguess.aspx?gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "坐庄") + "]");

                    builder.Append(Out.Tab("</div>", ""));

                    strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        else
        {

            string strLXWhere = "p_active=0 and p_TPRtime >= '" + System.DateTime.Now + "'";

            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaListLX = new TPR2.BLL.guess.BaList().GetBaListLX(pageIndex, pageSize, strLXWhere, out recordCount);
            if (listBaListLX.Count > 0)
            {
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaListLX)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("kzlist.aspx?act=kz&amp;ptype=4&amp;fly={0}") + "\">-{1}(共{2}场)</a>", n.p_title, n.p_title, new TPR2.BLL.guess.BaList().GetCount(n));
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

        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx"), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=mykz&amp;ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));


    }

    private void IsDelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[0-2]$", "类型错误"));
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.usid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (ptype == 0)
        {
            model.ID = gid;
            model.p_del = 0;
            new TPR2.BLL.guess.BaListMe().Updatep_del(model);
            Utils.Success("显示赛事", "显示赛事成功..", Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "1");
        }
        else if (ptype == 1)
        {
            model.ID = gid;
            model.p_del = 1;
            new TPR2.BLL.guess.BaListMe().Updatep_del(model);
            Utils.Success("隐藏赛事", "隐藏赛事成功..", Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "1");
        }
        else
        {
            new TPR2.BLL.guess.BaListMe().Updatep_isluck2(gid, 1, 1);
            new TPR2.BLL.guess.BaListMe().Updatep_isluck2(gid, 1, 2);
            new TPR2.BLL.guess.BaListMe().Updatep_isluck2(gid, 1, 3);
            Utils.Success("封盘", "封盘成功..", Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "1");
        }
    }

}