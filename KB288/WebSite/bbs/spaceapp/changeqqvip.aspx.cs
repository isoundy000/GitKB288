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
public partial class bbs_spaceapp_changeqqvip : System.Web.UI.Page
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
            case "qqvip":
                QqVipPage();
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
        Master.Title = "开通腾讯QQ服务";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择QQ服务");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=1") + "\"><img src=\"qq/1.jpg\" alt=\"load\"/>" + OutType(1) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=2") + "\"><img src=\"qq/2.jpg\" alt=\"load\"/>" + OutType(2) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=3") + "\"><img src=\"qq/3.jpg\" alt=\"load\"/>" + OutType(3) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=4") + "\"><img src=\"qq/4.jpg\" alt=\"load\"/>" + OutType(4) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=5") + "\"><img src=\"qq/5.jpg\" alt=\"load\"/>" + OutType(5) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=6") + "\"><img src=\"qq/6.jpg\" alt=\"load\"/>" + OutType(6) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=7") + "\"><img src=\"qq/7.jpg\" alt=\"load\"/>" + OutType(7) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=8") + "\"><img src=\"qq/8.jpg\" alt=\"load\"/>" + OutType(8) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=9") + "\"><img src=\"qq/9.jpg\" alt=\"load\"/>" + OutType(9) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=10") + "\"><img src=\"qq/10.jpg\" alt=\"load\"/>" + OutType(10) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=11") + "\"><img src=\"qq/11.jpg\" alt=\"load\"/>" + OutType(11) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=12") + "\"><img src=\"qq/12.jpg\" alt=\"load\"/>" + OutType(12) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=13") + "\"><img src=\"qq/13.jpg\" alt=\"load\"/>" + OutType(13) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=14") + "\"><img src=\"qq/14.jpg\" alt=\"load\"/>" + OutType(14) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=15") + "\"><img src=\"qq/15.jpg\" alt=\"load\"/>" + OutType(15) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=16") + "\"><img src=\"qq/16.jpg\" alt=\"load\"/>" + OutType(16) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=17") + "\"><img src=\"qq/17.jpg\" alt=\"load\"/>" + OutType(17) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=18") + "\"><img src=\"qq/18.jpg\" alt=\"load\"/>" + OutType(18) + "</a>");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qqvip&amp;ptype=19") + "\"><img src=\"qq/19.jpg\" alt=\"load\"/>" + OutType(19) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("每个QQ每个服务在6个月内最多只能开通12个月.<br />");
        builder.Append("每个ID每月只能为2个QQ号进行开通服务.<br />");
        builder.Append("开通QQ服务需5分钟至24小时内完成.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=mylist") + "\">我的开通记录&gt;&gt;</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list") + "\">全部开通记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void QqVipPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "开通腾讯QQ服务";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[1-9]\d*$", "开通类型错误"));
        if (ptype < 1 || ptype > 19)
        {
            Utils.Error("开通类型错误", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "")
        {
            int month = int.Parse(Utils.GetRequest("month", "post", 2, @"^[1-9]\d*$", "开通月份填写错误"));
            long qq = Int64.Parse(Utils.GetRequest("qq", "post", 2, @"^[0-9]{5,11}$", "QQ填写错误"));
            long qq2 = Int64.Parse(Utils.GetRequest("qq2", "post", 2, @"^[0-9]{5,11}$", "QQ填写错误"));
            if (month < 1 || month > 12)
            {
                Utils.Error("开通月份限1-12月", "");
            }
            if (!qq.Equals(qq2))
            {
                Utils.Error("QQ确认错误", "");
            }
            long Price = Convert.ToInt64(month * OutPrice(ptype));

            if (info == "ok2")
            {
                int myVipLeven = BCW.User.Users.VipLeven(meid);
                if (myVipLeven == 0)
                {
                    Utils.Error("必须是VIP会员才能继续...<br /><a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">马上开通VIP会员&gt;&gt;</a>", "");
                }

                if (new BCW.BLL.User().GetGold(meid) < Price)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                //每个QQ每个服务在6个月内最多只能开通12个月
                int GetMonth = new BCW.BLL.SellNum().GetSumBuyUIDQQ(ptype, qq.ToString(), meid);
                if (GetMonth + month > 12)
                {
                    Utils.Error("每个QQ每项服务最多开通12个月，QQ" + qq + "当前还可以开通“" + OutType(ptype) + "”" + (12 - GetMonth) + "个月", "");
                }

                //每个ID每30天内只能为2个QQ号进行开通服务
                int GetQQCount = new BCW.BLL.SellNum().GetSumQQCount(meid);
                if (GetQQCount >= 2)
                {
                    Utils.Error("每个ID在30天内只能为2个QQ号进行开通服务", "");

                }
                //支付安全提示
                string[] p_pageArr = { "act", "month", "qq", "qq2", "ptype", "info", "backurl" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "post", false);

                //是否刷屏
                string appName = "LIGHT_CHANGEQQVIP";
                int Expir = 60;
                BCW.User.Users.IsFresh(appName, Expir);

                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.SellNum model = new BCW.Model.SellNum();
                model.Types = 3;
                model.UsID = meid;
                model.UsName = mename;
                model.BuyUID = month;
                model.Price = Price;
                model.Mobile = qq.ToString() + "";
                model.State = 1;//1提交中/2已充值/3已评价
                model.AddTime = DateTime.Now;
                int id = new BCW.BLL.SellNum().Add2(model);
                //更新QQ服务类型
                new BCW.BLL.SellNum().UpdateTags(id, ptype);

                //扣币
                new BCW.BLL.User().UpdateiGold(meid, mename, -Price, "开通" + OutType(ptype) + "" + month + "个月");
                //动态记录
                new BCW.BLL.Action().Add(meid, mename, "在[URL=/bbs/spaceapp/changeqqvip.aspx]QQ特权处[/URL]开通" + OutType(ptype) + "" + month + "个月");

                new BCW.BLL.Guest().Add(10086, "QQ管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]开通" + OutType(ptype) + "" + month + "个月,请进入后台处理");
                //new BCW.BLL.Guest().Add(19611, "QQ管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]开通" + OutType(ptype) + "" + month + "个月,请进入后台处理");
                //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
                //{
                //    new BCW.BLL.Guest().Add(1010, "QQ管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]开通" + OutType(ptype) + "" + month + "个月,请进入后台处理");
                //}

                Utils.Success("开通" + OutType(ptype) + "", "开通" + OutType(ptype) + "" + month + "个月已提交成功,很乐意为您服务,请等待开通完成回复...", Utils.getUrl("changeqqvip.aspx?act=mylist"), "2");

            }
            else
            {

                builder.Append(Out.Tab("<div>", ""));

                builder.Append("您选择<img src=\"qq/" + ptype + ".jpg\" alt=\"load\"/>" + OutType(ptype) + "(开通" + month + "个月),需花费" + Price + "" + ub.Get("SiteBz") + "");

                builder.Append("<br />您的QQ号:" + qq + "");

                builder.Append(Out.Tab("</div>", "<br />"));
                string strName = "month,qq,qq2,ptype,act,info,backurl";
                string strValu = "" + month + "'" + qq + "'" + qq2 + "'" + ptype + "'qqvip'ok2'" + Utils.getPage(0) + "";
                string strOthe = "确定兑换,changeqqvip.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx") + "\">&lt;&lt;重新选择类型</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

        }
        else
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("您选择<img src=\"qq/" + ptype + ".jpg\" alt=\"load\"/>" + OutType(ptype) + "/" + OutPrice(ptype) + "" + ub.Get("SiteBz") + "/月");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开通QQ号:/,确认QQ号:/,开通时长:/,,,,";
            string strName = "qq,qq2,month,ptype,act,info,backurl";
            string strType = "num,num,num,hidden,hidden,hidden,hidden";
            string strValu = "''1'" + ptype + "'qqvip'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,false,false,false,false";
            string strIdea = "''个月''''|/";
            string strOthe = "确认开通,changeqqvip.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx") + "\">&lt;&lt;重新选择类型</a>");
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

        Master.Title = "我的QQ服务开通记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=mylist&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("兑换中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=mylist&amp;ptype=1") + "\">兑换中</a>|");

        if (ptype == 2)
            builder.Append("已完成");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=mylist&amp;ptype=2") + "\">已完成</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=3 and UsID=" + meid + " and State<>9";
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


                int Types = n.Tags;
                if (n.State == 1)
                {
                    builder.Append("开通QQ:" + n.Mobile + "|" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");
                }
                else
                {
                    builder.Append("<b>已成功</b>开通QQ:" + n.Mobile + "|" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");
                    if (n.Notes != null)
                        builder.Append("<br />我的评价:" + n.Notes + "");
                    else
                    {
                        if (n.UsID == meid)
                            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=bbs&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[评价]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list") + "\">全部开通记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("changeqqvip.aspx?act=qb") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void ListPage()
    {

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "全部QQ服务开通记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("兑换中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=1") + "\">兑换中</a>|");

        if (ptype == 2)
            builder.Append("已完成");
        else
            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=list&amp;ptype=2") + "\">已完成</a>");

        builder.Append(Out.Tab("<div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=3 and State<>9";

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
                int Types = n.Tags;

                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 1)
                {
                    builder.Append("开通" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");
                }
                else
                {
                    builder.Append("<b>已成功</b>开通" + OutType(Types) + "(" + n.BuyUID + "个月),花费" + n.Price + "" + ub.Get("SiteBz") + "|操作时间" + DT.FormatDate(n.AddTime, 5) + "");
                    if (n.Notes != null)
                        builder.Append("<br />TA的评价:" + n.Notes + "");
                    else
                    {
                        if (n.UsID == meid)
                            builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=bbs&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[评价]</a>");

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
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=mylist") + "\">我的开通记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("changeqqvip.aspx?act=qb") + "\">上级</a>-");
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
        if (model.UsID != meid || model.Types != 3)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string Notes = Utils.GetRequest("Notes", "all", 2, @"^[\s\S]{1,50}$", "评价内容限1-50字");

            new BCW.BLL.SellNum().UpdateNotes(id, Notes);
            Utils.Success("评价", "评价成功...", Utils.getPage("changeqqvip.aspx?act=mylist"), "2");

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
            string strOthe = "确定评价,changeqqvip.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getPage("changeqqvip.aspx?act=mylist") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private string OutType(int Types)
    {
        string p_str = string.Empty;
        if (Types == 1)
            p_str = "QQ会员";
        else if (Types == 2)
            p_str = "黄钻贵族";
        else if (Types == 3)
            p_str = "蓝钻贵族";
        else if (Types == 4)
            p_str = "超级QQ";
        else if (Types == 5)
            p_str = "红钻贵族";
        else if (Types == 6)
            p_str = "绿钻贵族";
        else if (Types == 7)
            p_str = "QQ炫舞紫钻";
        else if (Types == 8)
            p_str = "AVA精英";
        else if (Types == 9)
            p_str = "CF会员";
        else if (Types == 10)
            p_str = "西游VIP";
        else if (Types == 11)
            p_str = "洛克王国VIP";
        else if (Types == 12)
            p_str = "DNF黑钻";
        else if (Types == 13)
            p_str = "QQ音速";
        else if (Types == 14)
            p_str = "QQ飞车紫钻";
        else if (Types == 15)
            p_str = "读书VIP";
        else if (Types == 16)
            p_str = "QQ堂紫钻";
        else if (Types == 17)
            p_str = "寻仙VIP";
        else if (Types == 18)
            p_str = "QQ宠物粉钻";
        else if (Types == 19)
            p_str = "财付通SVIP";

        return p_str;
    }

    private int OutPrice(int Types)
    {
        int Price = Utils.ParseInt(ub.GetSub("QQVIPM" + Types + "", "/Controls/qqvip.xml"));
        return Price;
    }
}
