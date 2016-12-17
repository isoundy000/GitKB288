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
public partial class bbs_spaceapp_sellnum : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "uidbuy":
                UIDBuyPage();
                break;
            case "uidok":
                UIDOkPage();
                break;
            case "uidlist":
                UIDListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {


    }
    private void UIDBuyPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "兑换靓号";
        builder.Append(Out.Tab("<div class=\"title\">兑换靓号</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok" || info == "ok2")
        {
            int uid = int.Parse(Utils.GetRequest("uid", "post", 2, @"^[1-9]\d*$", "请输入4-8位的ID进行查找"));

            if (uid < 1000 || uid > 99999999)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("输入4-8位的ID进行查找!<br /><a href=\"" + Utils.getUrl("sellnum.aspx?act=uidbuy&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;重新查找</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {

                if (new BCW.BLL.SellNum().Exists(0, uid, 3))
                {
                    Utils.Error("此靓号已被别人抢先兑换了", "");
                }
                else if (new BCW.BLL.User().Exists(uid))
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("很遗憾！您输入的ID" + uid + "已被他人使用!<br /><a href=\"" + Utils.getUrl("sellnum.aspx?act=uidbuy&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;重新查找</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    if (info == "ok")
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("恭喜！你查找的ID:" + uid + "未被使用<br />喜欢这个靓号吗？点击下一步查询所需花费的" + ub.Get("SiteBz") + "");
                        builder.Append(Out.Tab("</div>", "<br />"));
                        string strName = "uid,act,info,backurl";
                        string strValu = "" + uid + "'uidbuy'ok2'" + Utils.getPage(0) + "";
                        string strOthe = "下一步,sellnum.aspx,post,0,red";

                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidbuy&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;换个更靓的</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else if (info == "ok2")
                    {
                        //int myVipLeven = BCW.User.Users.VipLeven(meid);
                        //if (myVipLeven == 0)
                        //{
                        //    Utils.Error("必须是VIP会员才能继续...<br /><a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">马上开通VIP会员&gt;&gt;</a>", "");
                        //}

                        if (new BCW.BLL.SellNum().GetCount(0, meid) >= 5)
                        {
                            Utils.Error("每ID每天只能查询5次报价...", "");
                        }
                        //if (new BCW.BLL.SellNum().Exists(0, uid, 1))
                        //{
                        //    Utils.Error("ID:" + uid + "已被提交给系统...", "");
                        //}

                        long Price = 0;
                        int State = 1;
                        DataSet ds = new BCW.BLL.SellNum().GetList("Price", "BuyUID=" + uid + " and State=2");
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            Price = Int64.Parse(ds.Tables[0].Rows[0]["Price"].ToString());
                            State = 2;
                        }

                        string mename = new BCW.BLL.User().GetUsName(meid);
                        BCW.Model.SellNum model = new BCW.Model.SellNum();
                        model.Types = 0;
                        model.UsID = meid;
                        model.UsName = mename;
                        model.BuyUID = uid;
                        model.Price = Price;
                        model.State = State;
                        model.AddTime = DateTime.Now;
                        new BCW.BLL.SellNum().Add(model);

                        //动态记录
                        new BCW.BLL.Action().Add(meid, mename, "在[URL=/bbs/spaceapp/sellnum.aspx?act=uidbuy]兑换靓号处[/URL]查询靓号ID" + uid + "的价格");

                        //发信息通知管理员
                        if (State == 1)
                        {
                            new BCW.BLL.Guest().Add(10086, "靓号管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]查询ID" + uid + "的价格,请进入后台处理");
                            new BCW.BLL.Guest().Add(19611, "靓号管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]查询ID" + uid + "的价格,请进入后台处理");
                            //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
                            //{
                            //    new BCW.BLL.Guest().Add(1010, "靓号管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]查询ID" + uid + "的价格,请进入后台处理");
                            //}
                            Utils.Success("靓号", "靓号提交成功，系统正在计算所需花费的的" + ub.Get("SiteBz") + "，请等待系统回复...", Utils.getUrl("sellnum.aspx?act=uidlist"), "2");
                        }
                        else
                        {
                            Utils.Success("靓号", "靓号提交成功，系统报价" + Price + "" + ub.Get("SiteBz") + "，正在进入靓号记录...", Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=2"), "2");
                        }
                    }
                }
            }
        }
        else
        {

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("人靠衣装马靠鞍,ID号也一样哦~");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = "输入你喜欢的ID(4-8位):/,,,,";
            string strName = "uid,act,info,backurl";
            string strType = "num,hidden,hidden,hidden";
            string strValu = "'uidbuy'ok'" + Utils.PostPage(1) + "";
            string strEmpt = "true,false,false,false";
            string strIdea = "";
            string strOthe = "查找,sellnum.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist") + "\">我的靓号记录</a><br />");
            builder.Append("温馨提示:<br />在兑换成功7天内可以用支出" + ub.Get("SiteBz") + "的ID内线<a href=\"" + Utils.getUrl("/bbs/guest.aspx?act=add&amp;hid=10086") + "\">客服(10086)</a>把现用的ID资料(包括发帖、回帖、" + ub.Get("SiteBz") + "等)全部转移到兑换的新ID中,超过7天不再受理.");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("../uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void UIDOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "兑换靓号";

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.SellNum model = new BCW.BLL.SellNum().GetSellNum(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 1)
        {
            Utils.Error("请等待系统报价", "");
        }
        if (model.State == 3)
        {
            Utils.Error("已申请兑换，请等待系统回复", "");
        }
        if (model.State == 4)
        {
            Utils.Error("此条记录已完成", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string mobile = Utils.GetRequest("mobile", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请正确输入十一位数的手机号码");

            if (new BCW.BLL.SellNum().Exists(0, model.BuyUID, 3))
            {
                Utils.Error("此靓号已被别人抢先兑换了", "");
            }
            //再次确定此号已注册
            if (new BCW.BLL.User().Exists(model.BuyUID))
            {
                Utils.Error("此靓号已被别人抢先注册了", "");
            }

            if (new BCW.BLL.User().Exists(mobile))
            {
                Utils.Error("此手机号" + mobile + "已绑定在其它ID上", "");
            }
            long gold = new BCW.BLL.User().GetGold(meid);
            if (gold < model.Price)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            //支付安全提示
            string[] p_pageArr = { "act", "mobile", "id", "info", "backurl" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "post", false);

            new BCW.BLL.SellNum().UpdateState3(id, mobile);
            //扣币
            string mename = new BCW.BLL.User().GetUsName(meid);
            new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, "兑换靓号ID" + model.BuyUID + "");

            //发信息通知管理员
            new BCW.BLL.Guest().Add(10086, "靓号管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]兑换ID" + model.BuyUID + ",请进入后台处理");
            new BCW.BLL.Guest().Add(19611, "靓号管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]兑换ID" + model.BuyUID + ",请进入后台处理");
            //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            //{
            //    new BCW.BLL.Guest().Add(1010, "靓号管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]兑换ID" + model.BuyUID + ",请进入后台处理");
            //}
            //动态记录
            new BCW.BLL.Action().Add(meid, mename, "在[URL=/bbs/spaceapp/sellnum.aspx?act=uidbuy]兑换靓号处[/URL]成功兑换靓号ID" + model.BuyUID + "");


            Utils.Success("靓号", "靓号兑换成功，请等待系统回复...", Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=3"), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">兑换靓号</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你要兑换的ID:" + model.BuyUID + "，兑换价为" + model.Price + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = "输入你要绑定的手机号:/,,,,";
            string strName = "mobile,id,act,info,backurl";
            string strType = "num,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'uidok'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "";
            string strOthe = "确定兑换,sellnum.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("温馨提示:<br />在兑换成功7天内可以用支出" + ub.Get("SiteBz") + "的ID内线<a href=\"" + Utils.getUrl("/bbs/guest.aspx?act=add&amp;hid=10086") + "\">客服(10086)</a>把现用的ID资料(包括发帖、回帖、" + ub.Get("SiteBz") + "等)全部转移到兑换的新ID中,超过7天不再受理.");
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist") + "\">我的靓号记录</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("../uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void UIDListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的靓号记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("查询中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=1") + "\">查询中</a>|");

        if (ptype == 2)
            builder.Append("已报价|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=2") + "\">已报价</a>|");

        if (ptype == 3)
            builder.Append("已成交");
        else
            builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidlist&amp;ptype=3") + "\">已成交</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=0 and UsID=" + meid + " and State<>9";

        if (ptype > 0 && ptype < 3)
            strWhere += " and State=" + ptype + "";
        else if (ptype >= 3)
            strWhere += " and State>=3";


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
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (n.State == 1)
                {
                    builder.Append("ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[未报价]");
                }
                else if (n.State == 2)
                {
                    builder.Append("ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                    builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidok&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">兑换</a>");
                }
                else if (n.State == 3)
                {
                    builder.Append("ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                    builder.Append("兑换时间" + DT.FormatDate(n.PayTime, 5) + "[等待系统回复]");
                }
                else
                {
                    builder.Append("ID号:" + n.BuyUID + "|提交时间" + DT.FormatDate(n.AddTime, 5) + "[报价" + n.Price + "" + ub.Get("SiteBz") + "]");
                    builder.Append("<br />绑定手机号:" + BCW.User.Users.FormatMobile(n.Mobile) + "");
                    //builder.Append("<br />备注:" + n.Notes + "");
                    builder.Append("<br />成交时间" + DT.FormatDate(n.PayTime, 5) + "");

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
        builder.Append("<a href=\"" + Utils.getUrl("sellnum.aspx?act=uidbuy") + "\">查找喜欢的ID&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("../uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
}
