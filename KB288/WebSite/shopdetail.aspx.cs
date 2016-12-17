using System;
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
public partial class shopdetail : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/front.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));
        int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-1]$", "0"));
        if (!new BCW.BLL.Goods().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int pageIndex;
        int recordCount;
        int pageSize = 300;
        string[] pageValUrl = { "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int pn = Utils.ParseInt(Request.QueryString["pn"]);
        if (pn == 0)
            pn = 1;

        BCW.Model.Goods model = new BCW.BLL.Goods().GetGoods(id);
        Master.Title = model.Title;

        //顶部调用
        string TopUbb = TopUbb = ub.GetSub("FtShopDetailTop", xmlPath);
        if (TopUbb != "")
        {
            TopUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(TopUbb));
            if (TopUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(TopUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(TopUbb);
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.Title);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        if (pageIndex == 1 && pover == 0)
        {
            string sFile = model.Files.Trim();

            if (!string.IsNullOrEmpty(sFile))
            {
                string[] txtPic = sFile.Split("#".ToCharArray());
                if (pn > txtPic.Length)
                    pn = txtPic.Length;

                builder.Append("<img src=\"" + Out.SysUBB(txtPic[pn - 1]) + "\" alt=\"load\"/><br />");

                if (pn < txtPic.Length)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "&amp;pn=" + (pn + 1) + "") + "\">下张</a> ");
                }
                if (pn > 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "&amp;pn=" + (pn - 1) + "") + "\">上张</a>");
                }
                if (txtPic.Length > 1)
                    builder.Append("(" + pn + "/" + txtPic.Length + ")<br />");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        if (pover == 1)
        {
            string content = BasePage.MultiContent(model.Content, pageIndex, pageSize, pover, out recordCount);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(content));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(BasePage.MultiContentPage(model.Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;返回查看商品</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", Out.RHr()));
            builder.Append("市场价：￥" + Convert.ToDouble(model.CityMoney) + "元");
            builder.Append("<br />会员价：");

            if (model.PostType == 0)
                builder.Append("￥" + Convert.ToDouble(model.VipMoney) + "元");
            else if (model.PostType == 1)
                builder.Append(Convert.ToDouble(model.VipMoney) + "" + ub.Get("SiteBz"));
            else
                builder.Append(Convert.ToDouble(model.VipMoney) + "" + ub.Get("SiteBz2"));

            builder.Append("<br />库存：" + (model.StockCount - model.SellCount) + "/已售" + model.SellCount + "件");
            if (model.PayType == 0 || model.PayType == 2)
            {
                if (model.PayType == 0)
                    builder.Append("<br />送货：货到付款");
                else
                    builder.Append("<br />送货：先付款后发货");

                if (!string.IsNullOrEmpty(model.PostMoney) && model.PostMoney.Contains("|"))
                {
                    string[] sTemp = model.PostMoney.Split("|".ToCharArray());
                    string postMoney = string.Empty;
                    for (int j = 0; j < sTemp.Length; j++)
                    {
                        if (j % 2 == 0)
                        {
                            postMoney += "/" + sTemp[j + 1].ToString() + ":" + sTemp[j] + "元";
                        }
                    }
                    builder.Append("<br />邮费：" + Utils.Mid(postMoney, 1, postMoney.Length) + "");
                }
                else
                {
                    builder.Append("<br />邮费：卖家包邮");
                }
            }
            else
            {
                builder.Append("<br />送货：当面交易");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (!string.IsNullOrEmpty(model.Config))
            {
                builder.Append("【产品属性】<br />");
                builder.Append(Out.SysUBB(model.Config));
            }
            builder.Append("联系方式：" + BCW.User.AdminCall.AdminUBB(Out.BasUBB(model.Mobile)) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[立即订购]</a>");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/favorites.aspx?act=addin&amp;backurl=" + Utils.PostPage(1) + "") + "\">[加入收藏]</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;pover=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[商品详情]</a>");
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=payment&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[付款方式]</a>");

            builder.Append("<br /><a href=\"" + Utils.getUrl("shopbuy.aspx?act=recom&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐本商品有奖&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            //互动块记录
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("shopbuy.aspx?act=info&amp;id=" + id + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;查看评价</a>.<a href=\"" + Utils.getUrl("shopbuy.aspx?act=info&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">成交(" + model.Paycount + ")</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = ",,,,";
            string strName = "Content,id,ptype,act,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'14'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,,,,";
            string strIdea = "/";
            string strOthe = "评论咨询,comment.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            int meid = new BCW.User.Users().GetUsId();
            DataSet ds = new BCW.BLL.Comment().GetList(id, 3, 14);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.AppendFormat("{0}.{1}:{2}({3})", (i + 1), ds.Tables[0].Rows[i]["UserName"].ToString(), ds.Tables[0].Rows[i]["Content"].ToString(), DT.FormatDate(Convert.ToDateTime(ds.Tables[0].Rows[i]["AddTime"]), 1));
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ReText"].ToString()))
                    {
                        builder.Append(Out.Tab("<font color=\"red\">", ""));
                        builder.Append("<br />★管理员回复:" + ds.Tables[0].Rows[i]["ReText"].ToString() + "");
                        builder.Append(Out.Tab("</font>", ""));
                    }
                    builder.Append("<br />");
                }
            }
            builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?ptype=14&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看更多评论(" + model.Recount + "条)</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            //底部调用
            string FootUbb = FootUbb = ub.GetSub("FtShopDetailFoot", xmlPath);
            if (FootUbb != "")
            {
                FootUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(FootUbb));
                if (FootUbb.IndexOf("</div>") == -1)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append(FootUbb);
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    builder.Append(TopUbb);
                }
            }
            builder.Append(Out.Tab("<div>", Out.RHr()));
            builder.Append("<a href=\"" + Utils.getUrl("myshop.aspx?backurl=" + Utils.PostPage(1) + "") + "\">我的订单记录</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
            if (model.NodeId != 0)
            {
                string nodeTitle = new BCW.BLL.Topics().GetTitle(model.NodeId);
                if (nodeTitle != "")
                    builder.Append("-<a href=\"" + Utils.getUrl("shop.aspx?id=" + model.NodeId + "") + "\">" + nodeTitle + "</a>");
            }
            builder.Append(Out.Tab("</div>", ""));

        }
        //更新人气点击
        new BCW.BLL.Goods().UpdateReadcount(id, 1);
    }
}