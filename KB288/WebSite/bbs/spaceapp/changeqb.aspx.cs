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

/// <summary>
/// 去除1010接收的内线信息
/// 
/// 黄国军 20160309
/// </summary>
public partial class bbs_spaceapp_changeqb : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (DateTime.Now > DateTime.Parse("2016-11-01"))
        {
            Utils.Error("功能已关闭,详情请联系站内客服", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "qb":
                QbPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "list":
                ListPage();
                break;
            case "bbs":
                BbsPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {


    }

    private void QbPage()
    {

        long bl = 2800;
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            bl = 9500;
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "" + ub.Get("SiteBz") + "兑换腾讯Q币";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "")
        {
            int qb = int.Parse(Utils.GetRequest("qb", "post", 2, @"^[1-9]\d*$", "Q币数量填写错误"));
            long qq = Int64.Parse(Utils.GetRequest("qq", "post", 2, @"^[0-9]{5,11}$", "QQ填写错误"));
            long qq2 = Int64.Parse(Utils.GetRequest("qq2", "post", 2, @"^[0-9]{5,11}$", "QQ填写错误"));
            if (qb < 5 || qb > 100)
            {
                Utils.Error("Q币数量限5-100", "");
            }
            if (!qq.Equals(qq2))
            {
                Utils.Error("QQ确认错误", "");
            }

            if (info == "ok2")
            {
                int myVipLeven = BCW.User.Users.VipLeven(meid);
                if (myVipLeven == 0)
                {
                    Utils.Error("必须是VIP会员才能继续...<br /><a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">马上开通VIP会员&gt;&gt;</a>", "");
                }

                long Price = Convert.ToInt64(qb * bl);
                if (new BCW.BLL.User().GetGold(meid) < Price)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                //每天最多100Q币
                int iPrice = new BCW.BLL.SellNum().GetSumBuyUID(2, meid);
                if (iPrice + qb > 100)
                {
                    Utils.Error("每ID每天最多可以兑换100Q币，你今天已兑换" + iPrice + "Q币", "");
                }
                //支付安全提示
                string[] p_pageArr = { "act", "qb", "qq", "qq2", "info", "backurl" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "post", false);

                //是否刷屏
                string appName = "LIGHT_CHANGEQB";
                int Expir = 60;
                BCW.User.Users.IsFresh(appName, Expir);

                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.SellNum model = new BCW.Model.SellNum();
                model.Types = 2;
                model.UsID = meid;
                model.UsName = mename;
                model.BuyUID = qb;
                model.Price = Price;
                model.Mobile = qq.ToString();
                model.State = 1;//1提交中/2已充值/3已评价
                model.AddTime = DateTime.Now;
                new BCW.BLL.SellNum().Add2(model);
                //扣币
                new BCW.BLL.User().UpdateiGold(meid, mename, -Price, "兑换" + qb + "个Q币");

                //动态记录
                new BCW.BLL.Action().Add(meid, mename, "在[URL=/bbs/spaceapp/changeqb.aspx?act=qb]兑换Q币处[/URL]兑换" + qb + "个Q币");

                new BCW.BLL.Guest().Add(10086, "Q币管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]兑换" + qb + "个Q币,请进入后台处理");
                //new BCW.BLL.Guest().Add(19611, "Q币管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]兑换" + qb + "个Q币,请进入后台处理");
                //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
                //{
                //    new BCW.BLL.Guest().Add(1010, "Q币管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]兑换" + qb + "个Q币,请进入后台处理");
                //}
                Utils.Success("兑换Q币", "兑换Q币已提交成功,很乐意为您服务,请等待充值完成回复...", Utils.getUrl("changeqb.aspx?act=mylist"), "2");

            }
            else
            {

                builder.Append(Out.Tab("<div>", ""));

                builder.Append("您将要兑换" + qb + "个Q币,需花费" + (qb * bl) + "" + ub.Get("SiteBz") + "");

                builder.Append("<br />您的QQ号:" + qq + "");

                builder.Append(Out.Tab("</div>", "<br />"));
                string strName = "qb,qq,qq2,act,info,backurl";
                string strValu = "" + qb + "'" + qq + "'" + qq2 + "'qb'ok2'" + Utils.getPage(0) + "";
                string strOthe = "确定兑换,changeqb.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=qb") + "\">&lt;&lt;重新选择</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

        }
        else
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("" + ub.Get("SiteBz") + "兑换腾讯Q币");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "请输入兑换Q币数量:/,请输入接受的QQ号:/,请再次输入接受的QQ号:/,,,";
            string strName = "qb,qq,qq2,act,info,backurl";
            string strType = "num,num,num,hidden,hidden,hidden";
            string strValu = "'''qb'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定兑换Q币,changeqb.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("每1Q币按" + bl + "" + ub.Get("SiteBz") + "计算<br />");
            builder.Append("每次至少需兑换5个Q币<br />");
            builder.Append("每天至多可兑换100Q币<br />");
            builder.Append("Q币将兑换到你填写的QQ号中<br />");
            builder.Append("一经确定兑换无法撤销,请注意<br />");
            builder.Append("兑换Q币需5分钟至24小时内到账");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=mylist") + "\">我的兑换记录&gt;&gt;</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=list") + "\">全部兑换记录&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("../uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void MyListPage()
    {

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的兑换话费记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=mylist&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("兑换中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=mylist&amp;ptype=1") + "\">兑换中</a>|");

        if (ptype == 2)
            builder.Append("已完成");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=mylist&amp;ptype=2") + "\">已完成</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=2 and UsID=" + meid + " and State<>9";
        if (ptype > 0)
        {
            if (ptype > 1)
                strWhere += " and State>=2";
            else
                strWhere += " and State=1";
        }

        // 开始读取专题
        IList<BCW.Model.SellNum> listSellNum = new BCW.BLL.SellNum().GetSellNums(pageIndex, pageSize, strWhere, out recordCount);
        if (listSellNum.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.SellNum n in listSellNum)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                if (n.State == 1)
                {
                    builder.Append("兑换" + n.BuyUID + "个Q币,花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                }
                else
                {
                    builder.Append("<b>已成功</b>兑换" + n.BuyUID + "个Q币,花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                    if (n.Notes != null)
                        builder.Append("<br />我的评价:" + n.Notes + "");
                    else
                    {
                        if (n.UsID == meid)
                            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=bbs&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[评价]</a>");
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
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=list") + "\">全部兑换记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("changeqb.aspx?act=qb") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void ListPage()
    {

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "全部兑换话费记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=list&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("兑换中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=list&amp;ptype=1") + "\">兑换中</a>|");

        if (ptype == 2)
            builder.Append("已完成");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=list&amp;ptype=2") + "\">已完成</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=2 and State<>9";

        if (ptype > 0)
        {
            if (ptype > 1)
                strWhere += " and State>=2";
            else
                strWhere += " and State=1";
        }

        // 开始读取专题
        IList<BCW.Model.SellNum> listSellNum = new BCW.BLL.SellNum().GetSellNums(pageIndex, pageSize, strWhere, out recordCount);
        if (listSellNum.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.SellNum n in listSellNum)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 1)
                {
                    builder.Append("兑换" + n.BuyUID + "个Q币,花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                }
                else
                {
                    builder.Append("<b>已成功</b>兑换" + n.BuyUID + "个Q币,花费" + n.Price + "" + ub.Get("SiteBz") + "|兑换时间" + DT.FormatDate(n.AddTime, 5) + "");
                    if (n.Notes != null)
                        builder.Append("<br />TA的评价:" + n.Notes + "");
                    else
                    {
                        if (n.UsID == meid)
                            builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=bbs&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[评价]</a>");

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
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=mylist") + "\">我的兑换Q币&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("changeqb.aspx?act=qb") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BbsPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "评价";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid || model.Types != 2)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string Notes = Utils.GetRequest("Notes", "all", 2, @"^[\s\S]{1,50}$", "评价内容限1-50字");

            new BCW.BLL.SellNum().UpdateNotes(id, Notes);
            Utils.Success("评价", "评价成功...", Utils.getPage("changeqb.aspx?act=mylist"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">评价</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("评价内容：");
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,,,,";
            string strName = "Notes,id,act,info,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'bbs'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定评价,changeqb.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getPage("changeqb.aspx?act=mylist") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
}
