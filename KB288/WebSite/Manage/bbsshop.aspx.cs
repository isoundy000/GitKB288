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
using BCW.Files;

/// <summary>
/// 商城购买购买历史
/// 黄国军 20160527
/// 
/// 增加28类别商品购买日志页
/// 黄国军 20160523
/// 
/// 增加RMB商品 类别28
/// 黄国军
/// 
/// 蒙宗将 16/8/29 增加实物类
/// </summary>
public partial class Manage_bbsshop : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "view":                //商品列表页
                ViewPage();
                break;
            case "addgift":             //添加商品页
                AddGiftPage();
                break;
            case "addgiftsave":         //保存商品页
                AddGiftSavePage();
                break;
            case "editgift":            //编辑商品页
                EditGiftPage();
                break;
            case "BuyHistory":          //购买历史
                BuyHistoryPage();
                break;
            case "paisong":
                PaisongPage();//商品派送
                break;
            case "paisongxinxi":
                PaisongXXPage();//商品派送信息
                break;
            case "xinxifankui":
                FankuiPage();//信息反馈
                break;
            case "goodsview":
                GoodsviewPage();//商品信息
                break;
            case "editgiftsave":
                EditGiftSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "delgift":
                DelGiftPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    #region 社区商城管理 ReloadPage
    /// <summary>
    /// 社区商城管理
    /// </summary>
    private void ReloadPage()
    {
        Master.Title = "社区商城管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">社区商城管理</a> ");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("礼物专线|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?ptype=0") + "\">礼物专线</a>|");

        //if (ptype == 1)
        //    builder.Append("道具|");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?ptype=1") + "\">道具</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("medal.aspx") + "\">勋章</a>");
        //builder.Append("|<a href=\"" + Utils.getUrl("bbsshop.aspx?ptype=3") + "\">靓号</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=" + ptype + "";

        // 开始读取专题
        IList<BCW.Model.Shoplist> listShoplist = new BCW.BLL.Shoplist().GetShoplists(pageIndex, pageSize, strWhere, out recordCount);
        if (listShoplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Shoplist n in listShoplist)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("bbsshop.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}(ID:{0})</a>", n.ID, n.Paixu, n.Title);

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

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=BuyHistory") + "\">购买日志</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=add") + "\">添加分类</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong") + "\">实物派送</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 购买日志 BuyHistoryPage
    /// <summary>
    /// 购买日志
    /// </summary>
    private void BuyHistoryPage()
    {
        Master.Title = "购买日志";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("购买日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "NodeId = 28";
        string[] pageValUrl = { "act", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = " AND UsID=" + uid + "";


        IList<BCW.Model.Shopkeep> listShopkeep = new BCW.BLL.Shopkeep().GetShopkeeps(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopkeep.Count > 0)
        {
            #region 商品列表
            int k = 1;
            foreach (BCW.Model.Shopkeep n in listShopkeep)
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
                builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftinfo&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "x" + n.Total + "</a>");

                if (n.NodeId == 28)
                {
                    builder.Append("<br />" + k + ".订单号:" + n.MerBillNo);
                    if (n.State == 0)
                    {
                        cn.com.ips.newpay.WSOrderQuery WSorder = new cn.com.ips.newpay.WSOrderQuery();
                        string SignatureOrder = "", bodystr = "", Resultstr = ""; ;
                        bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByChkOrderByShop(n, ref SignatureOrder);
                        string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost_ByOrder(DateTime.Now.ToString("yyyyMMddHHmmss"), SignatureOrder, bodystr);
                        Resultstr = WSorder.getOrderByMerBillNo(pGateWayReqstr);
                        BCW.IPSPay.IPSPayMent.updateorder(Resultstr, false);
                        builder.Append("<br />订单待支付");
                    }
                    else if (n.State == 2)
                    {
                        builder.Append("<br />[已失败]");
                    }
                    else
                    {
                        builder.Append("<br />[交易成功]");
                        string nBankName = BCW.IPSPay.IPSPayMent.GetBankNameByCode(n.BankCode);
                        builder.Append("(" + nBankName + ")");
                    }
                    builder.Append("|" + n.UsName + "(" + n.UsID + ")|" + n.AddTime.ToString());
                }
                else
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/bbsshop.aspx?act=proxy&amp;id=" + n.ID + "") + "") + "\">[送礼]</a>");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            #endregion
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        //string strText = "输入用户ID:/,,";
        //string strName = "uid,act";
        //string strType = "num,hidden";
        //string strValu = "'" + act + "";
        //string strEmpt = "true,false";
        //string strIdea = "/";
        //string strOthe = "搜日志,forumlog.aspx,post,1,red";
        //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    private void AddPage()
    {
        Master.Title = "添加商品分类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加商品分类");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "分类名称:/,分类属性/,后台排序:/,,";
        string strName = "Title,Types,Paixu,act,backurl";
        string strType = "text,select,num,hidden,hidden";
        string strValu = "'0'0'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,0|礼物|1|道具,false,false";
        string strIdea = "/";
        string strOthe = "添加分类|reset,bbsshop.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void AddSavePage()
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "请输入20字内的标题");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]$", "分类属性选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));

        BCW.Model.Shoplist model = new BCW.Model.Shoplist();
        model.Title = Title;
        model.Types = Types;
        model.Paixu = Paixu;
        model.PayCount = 0;
        new BCW.BLL.Shoplist().Add(model);
        Utils.Success("添加分类", "添加分类成功..", Utils.getPage("bbsshop.aspx"), "1");
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "分类ID错误"));
        BCW.Model.Shoplist model = new BCW.BLL.Shoplist().GetShoplist(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Master.Title = "编辑商品分类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑商品分类");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "分类名称:/,分类属性/,已出售量:/,后台排序:/,,,";
        string strName = "Title,Types,PayCount,Paixu,id,act,backurl";
        string strType = "text,select,num,num,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Types + "'" + model.PayCount + "'" + model.Paixu + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,0|礼物|1|道具,false,false,false,false";
        string strIdea = "/";
        string strOthe = "编辑分类|reset,bbsshop.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=del&amp;id=" + id + "") + "\">删除分类</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "分类ID错误"));
        if (!new BCW.BLL.Shoplist().Exists(id))
        {
            Utils.Error("不存在的分类", "");
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "请输入20字内的标题");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]$", "分类属性选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));
        int PayCount = int.Parse(Utils.GetRequest("PayCount", "post", 2, @"^[0-9]\d*$", "已出售量填写错误"));

        BCW.Model.Shoplist model = new BCW.Model.Shoplist();
        model.ID = id;
        model.Title = Title;
        model.Types = Types;
        model.Paixu = Paixu;
        model.PayCount = PayCount;
        new BCW.BLL.Shoplist().Update(model);
        Utils.Success("编辑分类", "编辑分类成功..", Utils.getPage("bbsshop.aspx"), "1");
    }

    #region 商品列表 ViewPage
    /// <summary>
    /// 商品列表
    /// </summary>
    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "分类ID错误"));
        string Title = new BCW.BLL.Shoplist().GetTitle(id);
        if (Title == "")
        {
            Utils.Error("不存在的分类", "");
        }

        Master.Title = Title;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Title + "管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "id" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "NodeId=" + id + "";
        strOrder = "ID Desc";
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
                builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=editgift&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");
                if (id != 28)
                {
                    if (n.Total != -1)
                        builder.Append("(" + n.PayCount + "/" + n.Total + ")");
                    else
                        builder.Append("(" + n.PayCount + "/∞)");

                    builder.Append("<br />" + OutMei(n.Para));

                    builder.Append(" 售价:" + n.Price + "");
                    if (n.BzType == 0)
                        builder.Append(ub.Get("SiteBz"));
                    else
                        builder.Append(ub.Get("SiteBz2"));
                }
                else
                {
                    if (n.Total != -1)
                        builder.Append("(" + n.PayCount + "/" + n.Total + ")");
                    else
                        builder.Append("(" + n.PayCount + "/∞)");

                    builder.Append("<br />+" + n.Para + ub.Get("SiteBz"));

                    builder.Append(" 售价:" + n.Price + "");

                    builder.Append("元");

                    builder.Append("<br />" + n.IDS);
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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=addgift&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加商品</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 添加商品页 AddGiftPage
    /// <summary>
    /// 添加商品页
    /// </summary>
    private void AddGiftPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "分类ID错误"));
        BCW.Model.Shoplist model = new BCW.BLL.Shoplist().GetShoplist(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Master.Title = "添加商品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + model.Title + "</a>&gt;添加商品");
        builder.Append(Out.Tab("</div>", ""));
        if (id != 28)
        {
            string strText = "商品名称:/,商品描述/,商品大图:/,商品小图:/,奖励参数:/,库存总量(填写-1则无限):/,商品售价:/,售价币种:/,赠送对象:/,是否VIP打折:/,是否推荐:/,,,";
            string strName = "Title,Notes,Pic,PrevPic,Para,Total,Price,BzType,IsSex,IsVip,IsRecom,id,act,backurl";
            string strType = string.Empty;
            if (Utils.Isie())
                strType = "text,text,file,file,text,text,num,select,select,select,select,hidden,hidden,hidden";
            else
                strType = "text,text,text,text,text,text,num,select,select,select,select,hidden,hidden,hidden";

            string strValu = "'''''0'0'0'0'0'0'" + id + "'addgiftsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",0|不限|1|女生|2|男生,0|不打折|1|打折,0|不推荐|1|推荐,false,false,false";
            string strIdea = "/";
            string strOthe = "";
            if (Utils.Isie())
                strOthe = "上传商品|reset,bbsshop.aspx,post,2,red|blue";
            else
                strOthe = "添加商品|reset,bbsshop.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string strText = "商品名称:/,商品描述/,商品大图:/,商品小图:/,获得" + ub.Get("SiteBz") + "(一元可换):/,可见ID(#区分，留空则所有人可见):/,库存总量(填写-1则无限):/,商品售价:/,售价币种:/,赠送对象:/,是否VIP打折:/,是否推荐:/,,,";
            string strName = "Title,Notes,Pic,PrevPic,Para,IDS,Total,Price,BzType,IsSex,IsVip,IsRecom,id,act,backurl";
            string strType = string.Empty;
            if (Utils.Isie())
                strType = "text,text,file,file,text,text,text,num,select,select,select,select,hidden,hidden,hidden";
            else
                strType = "text,text,text,text,text,text,text,num,select,select,select,select,hidden,hidden,hidden";

            string strValu = "''''''0'0'0'0'0'0'" + id + "'addgiftsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,0|元,0|不限|1|女生|2|男生,0|不打折|1|打折,0|不推荐|1|推荐,false,false,false";
            string strIdea = "/";
            string strOthe = "";
            if (Utils.Isie())
                strOthe = "上传商品|reset,bbsshop.aspx,post,2,red|blue";
            else
                strOthe = "添加商品|reset,bbsshop.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        if (id != 28)
        {
            builder.Append("温馨提示:<br />切换wap2.0可以上传商品图片.<br />奖励参数共有六项配置,请根据实际需要配置,填写方法:<br />例子：1|2|3|4|5|6<br />意思为:" + OutMei("1|2|3|4|5|6") + "<br />如某项值为0，则不奖励此项，同时支持负数:<br />例子：0|0|5|0|0|-5<br />意思为:" + OutMei("0|0|5|0|0|-5") + "<br />");
        }
        else
        {
            builder.Append("温馨提示:<br />切换wap2.0可以上传商品图片.<br />获得" + ub.Get("SiteBz") + "(一元可换):<br />意思为:充值一次金额兑换的币值<br />如售价为1元,获得币为2500,则购买一个商品扣取1元账户增加2500币<br />如售价为2元,获得币还是2500,则购买一个商品扣2元账户增加同样是2500币<br />");
            builder.Append("可见ID:<br />即指定的ID才可见到该商品,留空则为不限制。<br />多个用#号分隔<br />");
        }
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 保存商品 AddGiftSavePage
    /// <summary>
    /// 保存商品 AddGiftSavePage
    /// </summary>
    private void AddGiftSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "分类ID错误"));
        if (!new BCW.BLL.Shoplist().Exists(id))
        {
            Utils.Error("不存在的分类", "");
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "请输入20字内的商品名称");
        string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,200}$", "请输入200字内的描述");
        string Para = string.Empty;
        //ID为28的物品为人民币充值物品 必须判定1元转换成多少币
        if (id == 28)
        {
            Para = Utils.GetRequest("Para", "post", 2, @"^[\s\S]{1,100}$", "获得" + ub.Get("SiteBz") + "填写错误");
        }
        else
        {
            Para = Utils.GetRequest("Para", "post", 2, @"^[\s\S]{1,100}$", "奖励参数填写错误");
        }
        string ids = Utils.GetRequest("IDS", "post", 3, @"^[\s\S]{1,200}$", "");

        string sTotal = Utils.GetRequest("Total", "post", 1, "", "0");
        int Total = 0;
        if (sTotal != "-1")
            Total = int.Parse(Utils.GetRequest("Total", "post", 2, @"^[0-9]\d*$", "库存总量填写错误，不限库存请填写-1"));
        else
            Total = -1;

        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "售价填写错误"));
        int BzType = int.Parse(Utils.GetRequest("BzType", "post", 2, @"^[0-1]$", "售价币种选择错误"));
        int IsSex = int.Parse(Utils.GetRequest("IsSex", "post", 2, @"^[0-2]$", "赠送对象选择错误"));
        int IsVip = int.Parse(Utils.GetRequest("IsVip", "post", 2, @"^[0-1]$", "是否VIP打折选择错误"));
        int IsRecom = int.Parse(Utils.GetRequest("IsRecom", "post", 2, @"^[0-1]$", "是否推荐选择错误"));

        string Pic = "";
        string PrevPic = "";
        if (Utils.Isie())
        {
            //遍历File表单元素
            string rNum = DT.getDateTimeNum();
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
                int UpLength = 30;
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    //检查是否允许上传格式
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        Utils.Error("图片图片格式只允许" + UpExt + "..", "");
                    }
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                    {
                        Utils.Error("图片大小限" + UpLength + "K内", "");
                    }
                    string DirPath = "/Files/gift/";
                    string prevDirPath = string.Empty;

                    //生成随机文件名
                    if (iFile == 0)
                        fileName = "big_" + rNum + fileExtension;
                    else
                        fileName = "small_" + rNum + fileExtension;

                    string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                    postedFile.SaveAs(SavePath);
                    if (iFile == 0)
                        Pic = DirPath + fileName;
                    else
                        PrevPic = DirPath + fileName;
                }
            }
        }
        else
        {
            Pic = Utils.GetRequest("Pic", "post", 2, @"^[\s\S]{1,100}$", "请输入100字符内的大图地址，强烈建议使用本站图片");
            PrevPic = Utils.GetRequest("PrevPic", "post", 2, @"^[\s\S]{1,100}$", "请输入100字符内的时小图地址，强烈建议使用本站图片");
        }
        BCW.Model.Shopgift model = new BCW.Model.Shopgift();
        model.NodeId = id;
        model.Title = Title;
        model.Notes = Notes;
        model.Pic = Pic;
        model.PrevPic = PrevPic;
        model.Para = Para;
        model.Total = Total;
        model.Price = Price;
        model.BzType = BzType;
        model.IsSex = IsSex;
        model.IsVip = IsVip;
        model.IsRecom = IsRecom;
        model.AddTime = DateTime.Now;
        model.IDS = ids;
        new BCW.BLL.Shopgift().Add(model);
        Utils.Success("添加商品", "添加商品成功..", Utils.getPage("bbsshop.aspx?act=view&amp;id=" + id + ""), "1");
    }
    #endregion

    #region 编辑商品页 EditGiftPage
    private void EditGiftPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "分类ID错误"));
        BCW.Model.Shopgift model = new BCW.BLL.Shopgift().GetShopgift(id);
        if (model == null)
        {
            Utils.Error("不存在的商品", "");
        }
        Master.Title = "编辑商品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=view&amp;id=" + model.NodeId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.Shoplist().GetTitle(model.NodeId) + "</a>&gt;编辑商品");
        builder.Append(Out.Tab("</div>", ""));
        //列出分类
        string strEmpty = string.Empty;
        DataSet ds = new BCW.BLL.Shoplist().GetList("ID,Title", "Types>=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strEmpty += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"];
            }
            if (!string.IsNullOrEmpty(strEmpty))
            {
                strEmpty = Utils.Mid(strEmpty, 1, strEmpty.Length);
            }
        }
        else
        {
            Utils.Error("请先添加商品分类", "");
        }

        if (model.NodeId != 28)
        {
            string strText = "商品名称:/,商品描述/,商品大图:/,商品小图:/,奖励参数:/,库存总量(填写-1则无限):/,已售总量:/,商品售价:/,售价币种:/,赠送对象:/,是否VIP打折:/,是否推荐:/,商品分类:/,,,";
            string strName = "Title,Notes,Pic,PrevPic,Para,Total,PayCount,Price,BzType,IsSex,IsVip,IsRecom,NodeId,id,act,backurl";
            string strType = "text,text,text,text,text,text,num,num,select,select,select,select,select,hidden,hidden,hidden";
            string strValu = "" + model.Title + "'" + model.Notes + "'" + model.Pic + "'" + model.PrevPic + "'" + model.Para + "'" + model.Total + "'" + model.PayCount + "'" + model.Price + "'" + model.BzType + "'" + model.IsSex + "'" + model.IsVip + "'" + model.IsRecom + "'" + model.NodeId + "'" + id + "'editgiftsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",0|不限|1|女生|2|男生,0|不打折|1|打折,0|不推荐|1|推荐," + strEmpty + ",false,false,false";
            string strIdea = "/";
            string strOthe = "编辑商品|reset,bbsshop.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string strText = "商品名称:/,商品描述/,商品大图:/,商品小图:/,获得" + ub.Get("SiteBz") + "(一元可换):/,可见ID(#区分，留空则所有人可见):/,库存总量(填写-1则无限):/,已售总量:/,商品售价:/,售价币种:/,赠送对象:/,是否VIP打折:/,是否推荐:/,商品分类:/,,,";
            string strName = "Title,Notes,Pic,PrevPic,Para,IDS,Total,PayCount,Price,BzType,IsSex,IsVip,IsRecom,NodeId,id,act,backurl";
            string strType = "text,text,text,text,text,text,text,num,num,select,select,select,select,select,hidden,hidden,hidden";
            string strValu = "" + model.Title + "'" + model.Notes + "'" + model.Pic + "'" + model.PrevPic + "'" + model.Para + "'" + model.IDS + "'" + model.Total + "'" + model.PayCount + "'" + model.Price + "'" + model.BzType + "'" + model.IsSex + "'" + model.IsVip + "'" + model.IsRecom + "'" + model.NodeId + "'" + id + "'editgiftsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,0|元,0|不限|1|女生|2|男生,0|不打折|1|打折,0|不推荐|1|推荐," + strEmpty + ",false,false,false";
            string strIdea = "/";
            string strOthe = "编辑商品|reset,bbsshop.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (model.NodeId != 28)
        {
            builder.Append("温馨提示:<br />奖励参数共有六项配置,请根据实际需要配置,填写方法:<br />例子：1|2|3|4|5|6<br />意思为:" + OutMei("1|2|3|4|5|6") + "<br />如某项值为0，则不奖励此项，同时支持负数:<br />例子：0|0|5|0|0|-5<br />意思为:" + OutMei("0|0|5|0|0|-5") + "<br />");
        }
        else
        {
            builder.Append("温馨提示:<br />获得" + ub.Get("SiteBz") + "(一元可换):<br />意思为:充值一次金额兑换的币值<br />如售价为1元,获得币为2500,则购买一个商品扣取1元账户增加2500币<br />如售价为2元,获得币还是2500,则购买一个商品扣2元账户增加同样是2500币<br />");
            builder.Append("可见ID:<br />即指定的ID才可见到该商品,留空则为不限制。<br />多个用#号分隔<br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=delgift&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除商品</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 保存物品 EditGiftSavePage
    private void EditGiftSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "分类ID错误"));
        if (!new BCW.BLL.Shopgift().Exists(id))
        {
            Utils.Error("不存在的商品", "");
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "请输入20字内的商品名称");
        string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,200}$", "请输入200字内的描述");
        string Pic = Utils.GetRequest("Pic", "post", 2, @"^[\s\S]{1,100}$", "请输入100字符内的大图地址，强烈建议使用本站图片");
        string PrevPic = Utils.GetRequest("PrevPic", "post", 2, @"^[\s\S]{1,100}$", "请输入100字符内的时小图地址，强烈建议使用本站图片");
        string Para = Utils.GetRequest("Para", "post", 2, @"^[\s\S]{1,100}$", "奖励参数填写错误");
        string sTotal = Utils.GetRequest("Total", "post", 1, "", "0");
        string IDS = Utils.GetRequest("IDS", "post", 1, "", "");
        int Total = 0;
        if (sTotal != "-1")
            Total = int.Parse(Utils.GetRequest("Total", "post", 2, @"^[0-9]\d*$", "库存总量填写错误，不限库存请填写-1"));
        else
            Total = -1;

        int PayCount = int.Parse(Utils.GetRequest("PayCount", "post", 2, @"^[0-9]\d*$", "已售总量填写错误"));
        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "售价填写错误"));
        int BzType = int.Parse(Utils.GetRequest("BzType", "post", 2, @"^[0-1]$", "售价币种选择错误"));
        int IsSex = int.Parse(Utils.GetRequest("IsSex", "post", 2, @"^[0-2]$", "赠送对象选择错误"));
        int IsVip = int.Parse(Utils.GetRequest("IsVip", "post", 2, @"^[0-1]$", "是否VIP打折选择错误"));
        int IsRecom = int.Parse(Utils.GetRequest("IsRecom", "post", 2, @"^[0-1]$", "是否推荐选择错误"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[0-9]\d*$", "商品分类选择错误"));

        BCW.Model.Shopgift model = new BCW.Model.Shopgift();
        model.ID = id;
        model.NodeId = NodeId;
        model.Title = Title;
        model.Notes = Notes;
        model.Pic = Pic;
        model.PrevPic = PrevPic;
        model.Para = Para;
        model.PayCount = PayCount;
        model.Total = Total;
        model.Price = Price;
        model.BzType = BzType;
        model.IsSex = IsSex;
        model.IsVip = IsVip;
        model.IsRecom = IsRecom;
        model.IDS = IDS;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Shopgift().Update(model);
        Utils.Success("编辑商品", "编辑商品成功..", Utils.getUrl("bbsshop.aspx?act=editgift&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
    #endregion

    /// <summary>
    /// 我的实物信息
    /// </summary>
    private void GoodsviewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //  int ID = int.Parse(Utils.GetRequest("ID", "all", 1, @"^[1-9]\d*$", "0"));
        int ID = int.Parse(Utils.GetRequest("ID", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.Shop.BLL.Shopgoods().Exists(meid, ID))
        {
            Utils.Error("不存在的商品记录", "");
        }
        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods(ID);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (model.GiftId != 29)
        {
            Utils.Error("不存在的商品记录", "");
        }
        Master.Title = "我的储物箱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=store") + "\">储物箱</a>&gt;" + model.Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("==商品详情==");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("<img src=\"" + model.PrevPic + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("商品 ID ：" + model.ShopGiftId + "<br />");
        builder.Append("商品名称：" + model.Title + "<br />");
        builder.Append("商品类型：" + new BCW.BLL.Shoplist().GetTitle(model.GiftId) + "<br />");
        builder.Append("购买单号：" + model.ID + "<br />");
        builder.Append("购买数量：" + model.Num + "<br />");
        builder.Append("购买时间：" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        builder.Append("收件姓名：" + model.RealName + "<br />");
        builder.Append("收件地址：" + model.Address + "<br />");
        builder.Append("联系方式：" + model.Phone + "<br />");
        if (model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
        {
            if (model.SendTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
            {
                builder.Append("商品状态：商品正在准备中...");
            }
            else
            {
                builder.Append("商品状态：商品已发货<br />");
                builder.Append("快递公司：" + model.Express + "<br />");
                builder.Append("快递单号：" + model.Expressnum + "");
            }
        }
        else
        {
            builder.Append("商品状态：商品已送达<br />");
            builder.Append("快递公司：" + model.Express + "<br />");
            builder.Append("快递单号：" + model.Expressnum + "");
        }

        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bbsshop.aspx?act=storemy&amp;ID=" + model.ShopGiftId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Utils.getPage(0) + "") + "\">商城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //商品派送
    private void PaisongPage()
    {
        Master.Title = "商品派送管理中心";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理</a>&gt;商品派送 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "2"));

        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">未确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">未确认信息</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">已确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已确认信息</a>" + "|");

        if (ptype == 3)
            builder.Append("<b style=\"color:black\">已发货" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">已发货</a>" + "|");
        if (ptype == 4)
            builder.Append("<b style=\"color:black\">已送达" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">已送达</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件
        if (ptype == 1)
        {
            strWhere = " Address='" + "" + "'";
            strOrder = "ID Desc";
        }
        if (ptype == 2)
        {
            strWhere = " Address!='" + "" + "' and SendTime ='" + Convert.ToDateTime("2000-10-10 00:00:00.00") + "'";
            strOrder = "ID Desc";
        }
        if (ptype == 3)
        {
            strWhere = " SendTime !='" + Convert.ToDateTime("2000-10-10 00:00:00.00") + "' and ReceiveTime ='" + Convert.ToDateTime("2000-10-10 00:00:00.00") + "'";
            strOrder = "ID Desc";
        }
        if (ptype == 4)
        {
            strWhere = "ReceiveTime !='" + Convert.ToDateTime("2000-10-10 00:00:00.00") + "'";
            strOrder = "ID Desc";
        }

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Shop.Model.Shopgoods> listdrawuser = new BCW.Shop.BLL.Shopgoods().GetShopgoodss(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdrawuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Shop.Model.Shopgoods n in listdrawuser)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;

                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "(" + n.UsID + ")" + "</a>" + "购买商品" + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisongxinxi&amp;counts=" + n.ID + "") + "\">单号:" + n.ID + "|" + n.Title + "</a>";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong&amp;ptype=" + ptype + "&amp;counts=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("==单号查询==");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "输入购买单号:/,";
        string strName = "counts,backurl";
        string strType = "text,hidden";
        string strValu = "" + counts + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜商品信息,bbsshop.aspx?act=paisongxinxi&amp;u=" + Utils.getstrU() + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("==收货超时设置==");
        builder.Append(Out.Tab("</div>", ""));

        ub xml = new ub();
        string xmlPath = "/Controls/bbsshop.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string BbsshopReceiveTime = Utils.GetRequest("BbsshopReceiveTime", "all", 2, @"^[0-9]\d*$", "天数填写错误");
            string BbsshopMaxnum = Utils.GetRequest("BbsshopMaxnum", "all", 2, @"^[0-9]\d*$", "单次购买上限填写错误");

            xml.dss["BbsshopReceiveTime"] = BbsshopReceiveTime;
            xml.dss["BbsshopMaxnum"] = BbsshopMaxnum;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("商城管理", "设置成功，正在返回..", Utils.getUrl("bbsshop.aspx?act=paisong"), "2");
        }
        else
        {
            string strText1 = "默认发货几天后自动更新商品状态为送达:/,实物单次购买最大上限:/,";
            string strName1 = "BbsshopReceiveTime,BbsshopMaxnum";
            string strType1 = "num,num";
            string strValu1 = "" + xml.dss["BbsshopReceiveTime"] + "'" + xml.dss["BbsshopMaxnum"] + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "false,false";
            string strIdea1 = "/";
            string strOthe1 = "确定修改,bbsshop.aspx?act=paisong,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));

        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("说明：默认发货几天后会自动更新商品状态为送达状态主要是应对用户已经收到商品但也没有在商城的储物箱确认收货造成商品状态一直处于发货中的情况，天数根据实际情况设置，参考淘宝等默认发货后十天时间为限，如果用户超时不进行确认收货，那么系统默认更新为已经送达");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //商品派送信息
    private void PaisongXXPage()
    {
        Master.Title = "商品派送管理中心";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        //  int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong") + "\">商品派送</a>&gt;商品派送信息 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods(counts);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (model.GiftId != 29)
        {
            Utils.Error("不存在的商品记录", "");
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【商品详情】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("<img src=\"" + model.PrevPic + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("商品 ID ：" + model.ShopGiftId + "<br />");
        builder.Append("商品名称：" + model.Title + "<br />");
        builder.Append("商品类型：" + new BCW.BLL.Shoplist().GetTitle(model.GiftId) + "<br />");
        builder.Append("购买单号：" + model.ID + "<br />");
        builder.Append("购买数量：" + model.Num + "<br />");
        builder.Append("购买时间：" + Convert.ToDateTime(model.BuyTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");

        if (model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
        {
            if (model.SendTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
            {
                builder.Append("商品状态：等待发货...");
            }
            else
            {
                builder.Append("商品状态：商品已发货<br />");
                builder.Append("快递公司：" + model.Express + "<br />");
                builder.Append("快递单号：" + model.Expressnum + "<br />");
                builder.Append("寄送时间：" + Convert.ToDateTime(model.SendTime).ToString("yyyy-MM-dd HH:mm:ss") + "");
            }
        }
        else
        {
            builder.Append("商品状态：商品已送达<br />");
            builder.Append("快递公司：" + model.Express + "<br />");
            builder.Append("快递单号：" + model.Expressnum + "<br />");
            builder.Append("寄送时间：" + Convert.ToDateTime(model.SendTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
            builder.Append("送达时间：" + Convert.ToDateTime(model.ReceiveTime).ToString("yyyy-MM-dd HH:mm:ss") + "");
        }

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h style=\"color:red\">【用户信息】</h><br />");
        builder.Append("<h style=\"color:blue\">用户  ID：</h>" + model.UsID + "<br />");
        builder.Append("<h style=\"color:blue\">收件姓名：</h>" + model.RealName + "<br />");
        builder.Append("<h style=\"color:blue\">收件地址：</h>" + model.Address + "<br />");
        builder.Append("<h style=\"color:blue\">联系方式：</h>" + model.Phone + "<br />");
        builder.Append("<h style=\"color:blue\">联系邮箱：</h>" + model.Email + "<br />");
        builder.Append("<h style=\"color:blue\">用户留言：</h>" + model.Message + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (model.Address != "" && model.SendTime == Convert.ToDateTime("2000-10-10 00:00:00.00") && model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h><br />");
            builder.Append("若已查看用户信息，并已经将奖品送出（已快递），则进行快递信息录入，以便用户查询<br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=xinxifankui&amp;counts=" + model.ID + "") + "\">&gt;&gt;&gt;信息反馈&lt;&lt;&lt;</a>");
        }
        if (model.SendTime != Convert.ToDateTime("2000-10-10 00:00:00.00") && model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00.00"))
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h><br />");
            builder.Append("若用户已经收到奖品并反馈信息，则进行信息录入，更改奖品状态<br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=xinxifankui&amp;counts=" + model.ID + "") + "\">&gt;&gt;&gt;信息反馈&lt;&lt;&lt;</a>");
        }
        if (model.ReceiveTime != Convert.ToDateTime("2000-10-10 00:00:00.00"))
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h>");
            builder.Append("<br />奖品已完成送达，不再需要信息反馈");
        }
        if (model.Address == "")
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h>");
            builder.Append("<br />请等待用户确认信息");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //信息反馈
    private void FankuiPage()
    {


        Master.Title = "商品派送管理中心";
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        BCW.Shop.Model.Shopgoods model = new BCW.Shop.BLL.Shopgoods().GetShopgoods(counts);
        if (model == null)
        {
            Utils.Error("不存在的商品记录", "");
        }
        if (model.GiftId != 29)
        {
            Utils.Error("不存在的商品记录", "");
        }
        ub xml = new ub();
        string xmlPath = "/Controls/bbsshop.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理</a>&gt;<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong") + "\">商品派送</a>&gt;信息反馈 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append("【商品详情】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Title + "<br />");
        builder.Append("购买单号：" + model.ID);
        builder.Append("<br />");
        builder.Append("<img src=\"" + model.PrevPic + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("注意：填写快递信息时请确认购买单号是否正确，以免信息录错");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (model.SendTime == Convert.ToDateTime("2000-10-10 00:00:00") && model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00"))
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");

            if (Utils.ToSChinese(ac) == "确定录入")
            {
                string Express = Utils.GetRequest("Express", "post", 2, @"^[^\^]{1,200}$", "快递公司填写出错");
                string Expressnum = Utils.GetRequest("Expressnum", "post", 2, @"^[^\^]{1,200}$", "快递单号填写错误");

                new BCW.Shop.BLL.Shopgoods().UpdateMessagebyID(model.ID, Express, Expressnum, DateTime.Now);  //根据购买单号更新快递信息

                //发内线
                string strLog = "根据你在商城购买的单号为" + model.ID + "的商品" + model.Title + "，你的商品已经发货（" + Express + ":" + Expressnum + "），请耐心等待并注意查收" + "[url=/bbs/bbsshop.aspx]进入商城[/url]";
                new BCW.BLL.Guest().Add(0, model.UsID, new BCW.BLL.User().GetUsName(model.UsID), strLog);

                Utils.Success("商城管理", "商品反馈信息成功，正在返回..", Utils.getUrl("bbsshop.aspx?act=paisong&amp;counts=" + model.ID + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                string strText = "快递公司:/,快递单号:/,";
                string strName = "Express,Expressnum";
                string strType = "text,text";
                string strValu = "" + xml.dss["Express"] + "'" + xml.dss["Expressnum"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "确定录入,bbsshop.aspx?act=xinxifankui&amp;counts=" + model.ID + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
        }

        if (model.SendTime != Convert.ToDateTime("2000-10-10 00:00:00") && model.ReceiveTime == Convert.ToDateTime("2000-10-10 00:00:00"))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【信息录入】<br />");
            builder.Append("若收到用户确定收货的的信息，则修改奖品状态为已送达（此时表示该编号的奖品已经完成）");
            builder.Append(Out.Tab("</div>", "<br />"));

            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定送达")
            {

                try
                {
                    new BCW.Shop.BLL.Shopgoods().UpdateReceivebyID(model.ID, DateTime.Now);
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改成功!" + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=paisong&amp;backurl=" + Utils.PostPage(1) + "") + "\"><br />返回商品派送</a>");
                    builder.Append(Out.Tab("</div>", ""));

                }
                catch
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改失败!" + "<a href=\"" + Utils.getUrl("bbsshop.aspx?act=xinxifankui&amp;counts=" + counts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }

            }
            else
            {
                builder.Append(Out.Tab("</div>", ""));
                string Text = ",";
                string Name = "hidden";
                string Type = "hidden";
                string Valu = "" + "" + "'" + Utils.getPage(0) + "";
                string Empt = "1|派送中|2|已送达";
                string Idea = "/";
                string Othe = "确定送达,bbsshop.aspx?act=xinxifankui&amp;counts=" + counts + "&amp;,post,1,red";
                builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));

            }
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx") + "\">商城管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


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

    private void DelGiftPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除商品";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此商品记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?info=ok&amp;act=delgift&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=editgift&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            BCW.Model.Shopgift model = new BCW.BLL.Shopgift().GetShopgift(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            //删除附图
            BCW.Files.FileTool.DeleteFile(model.Pic);
            if (model.NodeId == 29)
            {

            }
            else
            {
                BCW.Files.FileTool.DeleteFile(model.PrevPic);
            }
            //删除
            new BCW.BLL.Shopgift().Delete(id);
            Utils.Success("删除商品", "删除商品成功..", Utils.getPage("bbsshop.aspx?id=" + id + ""), "1");
        }
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除分类";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此分类记录吗，删除此分类将使分类下的商品一并删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bbsshop.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Shoplist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Shoplist().Delete(id);
            DataSet ds = new BCW.BLL.Shopgift().GetList("Pic,PrevPic", "NodeId=" + id + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BCW.Files.FileTool.DeleteFile(ds.Tables[0].Rows[i]["Pic"].ToString());
                    BCW.Files.FileTool.DeleteFile(ds.Tables[0].Rows[i]["PrevPic"].ToString());
                }
            }
            new BCW.BLL.Shopgift().Delete("NodeId=" + id + "");

            Utils.Success("删除分类", "删除分类成功..", Utils.getUrl("bbsshop.aspx"), "1");
        }
    }
}
