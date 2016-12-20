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
using System.Text.RegularExpressions;

public partial class _Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected System.Text.StringBuilder builderIndex = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int kid = int.Parse(Utils.GetRequest("kid", "get", 1, @"^[0-9]\d*$", "0"));
        string strWhere = string.Empty;
        int meid = new BCW.User.Users().GetUsId();
        BCW.Model.Topics model = null;        
        if (id != 0)
        {
            if (!new BCW.BLL.Topics().ExistsIdLeibie(id, 0))
            {
                Utils.Error("不存在的记录", "");
            }
            if (meid == 0)
                strWhere = "NodeId=" + id + " AND Hidden=0 ORDER BY Paixu ASC";
            else
                strWhere = "NodeId=" + id + " AND Hidden<=1 ORDER BY Paixu ASC";            

            model = new BCW.BLL.Topics().GetTopics(id);
            Master.Title = model.Title;

            //----------------业务处理开始
            bool IsTs = false;
            if (model.Cent != 0)
            {

                if (model.SellTypes == 0)//按次收费
                {
                    string payIDs = "|" + model.PayId + "|";
                    if (payIDs.IndexOf("|" + meid + "|") == -1)
                    {
                        IsTs = true;
                    }
                }

                else if (model.SellTypes == 1 || model.SellTypes == 2)//包周包月
                {
                    if (!new BCW.BLL.Order().Exists(id, meid, DateTime.Now))
                    {
                        IsTs = true;
                    }
                }
            }
            if (model.VipLeven != 0)
            {
                if (meid == 0)
                    Utils.Login();//显示登录

                int VipLeven = BCW.User.Users.VipLeven(meid);
                if (VipLeven < model.VipLeven)
                {
                    Utils.Error("本页面限VIP等级" + model.VipLeven + "级进入<br />您的VIP等级为<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=addvip") + "\">" + VipLeven + "级</a>", "");
                }
            }
            
            if (IsTs == true)
            {
                if (meid == 0)
                    Utils.Login();//显示登录

                long Cent = Convert.ToInt64(model.Cent);
                //取用户信息
                string Bz = string.Empty;
                long megold = 0;
                if (model.BzType == 0)
                {
                    megold = new BCW.BLL.User().GetGold(meid);
                    Bz = ub.Get("SiteBz");
                }
                else
                {
                    megold = new BCW.BLL.User().GetMoney(meid);
                    Bz = ub.Get("SiteBz2");
                }
                string act = Utils.GetRequest("act", "get", 1, "", "");
                if (act != "ok")
                {
                    new Out().head(Utils.ForWordType("温馨提示"));
                    Response.Write(Out.Tab("<div class=\"text\">", ""));
                    if (model.SellTypes == 0)
                    {
                        Response.Write("本页内容收费" + Cent + "" + Bz + "，扣费一次，永久浏览");

                    }
                    else if (model.SellTypes == 1)
                    {
                        Response.Write("本页内容为包周业务，,如果您是此内容的包周会员，可以免费进入，否则将扣除您的" + Cent + "" + Bz + "，您将免费浏览本页面为一周时间");
                    }
                    else if (model.SellTypes == 2)
                    {
                        Response.Write("本页内容为包月业务，,如果您是此内容的包月会员，可以免费进入，否则将扣除您的" + Cent + "" + Bz + "，您将免费浏览本页面为一个月时间");
                    }
                    Response.Write(Out.Tab("</div>", "<br />"));
                    Response.Write(Out.Tab("<div>", ""));
                    Response.Write("您自带" + megold + "" + Bz + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay") + "\">[充值]</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("default.aspx?act=ok&amp;id=" + id + "") + "\">马上进入浏览</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回上级</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }
                
                //支付安全提示
                string[] p_pageArr = { "act", "id" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

                if (model.SellTypes == 0)//按次收费
                {
                    string payIDs = "|" + model.PayId + "|";
                    if (payIDs.IndexOf("|" + meid + "|") == -1)
                    {
                        if (megold < Cent)
                        {
                            Utils.Error("您的" + Bz + "不足", Utils.getUrl("default.aspx?id=" + id + ""));
                        }
                        //扣币
                        if (model.BzType == 0)
                            new BCW.BLL.User().UpdateiGold(meid, -Cent, "浏览收费页面");
                        else
                            new BCW.BLL.User().UpdateiMoney(meid, -Cent, "浏览收费页面");

                        //更新
                        payIDs = model.PayId + "|" + meid;
                        new BCW.BLL.Topics().UpdatePayId(id, payIDs);
                    }
                }

                else if (model.SellTypes == 1 || model.SellTypes == 2)//包周包月
                {
                    int iDays = 0;
                    if (model.SellTypes == 1)
                        iDays = 7;
                    else
                        iDays = 30;

                    if (!new BCW.BLL.Order().Exists(id, meid, DateTime.Now))
                    {
                        if (megold < Cent)
                        {
                            Utils.Error("您的" + Bz + "不足", "");
                        }
                        //扣币
                        if (model.BzType == 0)
                            new BCW.BLL.User().UpdateiGold(meid, -Cent, "浏览收费页面");
                        else
                            new BCW.BLL.User().UpdateiMoney(meid, -Cent, "浏览收费页面");

                        //增加/更新
                        BCW.Model.Order modelorder = new BCW.Model.Order();
                        modelorder.UsId = meid;
                        modelorder.UsName = new BCW.BLL.User().GetUsName(meid);
                        modelorder.TopicId = id;
                        modelorder.SellTypes = model.SellTypes;
                        modelorder.Title = model.Title;
                        modelorder.AddTime = DateTime.Now;
                        modelorder.ExTime = DateTime.Now.AddDays(iDays);
                        if (!new BCW.BLL.Order().Exists(id, meid))
                        {
                            new BCW.BLL.Order().Add(modelorder);
                        }
                        else
                        {
                            new BCW.BLL.Order().Update(modelorder);
                        }
                    }
                }
            }
            //----------------业务处理结束

            //----------------密码访问开始
            string pwd = Utils.GetRequest("pwd", "post", 1, "", "");
            if (!string.IsNullOrEmpty(model.InPwd) && pwd != model.InPwd)
            {
                new Out().head(Utils.ForWordType("温馨提示"));
                Response.Write(Out.Tab("<div class=\"title\">", ""));
                Response.Write("本页面内容已加密");
                Response.Write(Out.Tab("</div>", ""));
                string strText = "输入密码:/,,";
                string strName = "pwd,id,backurl";
                string strType = "password,hidden,hidden,hidden";
                string strValu = "'" + id + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false";
                string strIdea = "/";
                string strOthe = "确认访问,default.aspx,post,1,red";

                Response.Write(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                Response.Write(Out.Tab("<div>", ""));
                Response.Write(" <a href=\"" + Utils.getUrl("default.aspx") + "\">取消</a>");
                Response.Write(Out.Tab("</div>", ""));
                Response.Write(new Out().foot());
                Response.End();
            }

            //----------------密码访问结束

            //----------------限制手机访问开始
            if (model.IsPc == 1)
            {
                if (!Utils.IsMobileUa())
                {
                    Utils.Error("请使用手机访问本页", "");
                }
            }
            //----------------限制手机访问结束
            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
            //builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "</div>", ""));

        }
        else
        {
            if (meid == 0)
                strWhere = "NodeId=0 AND Leibie=0 AND Hidden=0 ORDER BY Paixu ASC";
            else
                strWhere = "NodeId=0 AND Leibie=0 AND Hidden<=1 ORDER BY Paixu ASC";

            Master.Title = ub.Get("SiteName");

            string Logo = ub.Get("SiteLogo");
            if (!string.IsNullOrEmpty(Logo))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            //顶部滚动
            builder.Append(BCW.User.Master.OutTopRand(1));
            
        }

        //20151222 黄国军查阅
        //读取排序
        DataSet ds = new BCW.BLL.Topics().GetList(strWhere);
        
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            if (id != 0)
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            else
            {
                builder.Append(Out.Div("div", "网站正在建设中.."));
            }
        }
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string ID = ds.Tables[0].Rows[i]["ID"].ToString();
            string Types = ds.Tables[0].Rows[i]["Types"].ToString();
            string Title = ds.Tables[0].Rows[i]["Title"].ToString();
            string Content = ds.Tables[0].Rows[i]["Content"].ToString();
            string IsBr = ds.Tables[0].Rows[i]["IsBr"].ToString();
            int VipLeven = Utils.ParseInt(ds.Tables[0].Rows[i]["VipLeven"].ToString());
            string Br = string.Empty;
            if (IsBr == "0" && IsVipSeen(meid, VipLeven) == true)
                Br = Convert.ToChar(10).ToString();
           
            switch (Types)
            {
                case "1":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("default.aspx?id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "2":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Content)));
                    break;
                case "3":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append("<img src=\"" + Content + "\" alt=\"load\"/>");
                    break;
                case "4":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append("<a href=\"" + BCW.User.AdminCall.AdminUBB(Utils.SetUrl(Content)) + "\">" + Title + "</a>");
                    break;
                case "5":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append(Out.WmlDecode(Content));
                    break;
                case "6":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + Content + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "10":
                    builderIndex.Append(BCW.User.AdminCall.ShowAdvert());
                    break;
                case "11":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("list.aspx?id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "12":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("list.aspx?id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "13":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("list.aspx?id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "14":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("shop.aspx?id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                default:
                    builderIndex.Append("");
                    break;
            }

            builderIndex.Append(Br);
        }
        
        if (!Utils.Isie())
        {
            builder.Append(builderIndex.ToString().Replace((Convert.ToChar(10).ToString()), "<br />"));
        }
        else
        {
            //20151222 黄国军 注释
            //首页游戏列表输出
            string[] txtIndex = builderIndex.ToString().Split((Convert.ToChar(10).ToString()).ToCharArray());
            
            for (int i = 0; i < txtIndex.Length; i++)
            {
                
                // 输出列表的格式
                if (txtIndex[i].IndexOf("</div>") == -1)
                {                    
                    if ((i + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div>", ""));
                }
                builder.Append(txtIndex[i].ToString());
                if (txtIndex[i].IndexOf("</div>") == -1)
                {
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }

        if (id != 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
            if (model.NodeId != 0)
            {
                builder.Append("-<a href=\"" + Utils.getUrl("default.aspx?id=" + model.NodeId + "") + "\">" + new BCW.BLL.Topics().GetTitle(model.NodeId) + "</a>");
            }
            builder.Append(Out.Tab("</div>", ""));

        }
        
        //-----------友链链入开始
        if (kid != 0)
        {
            if (new BCW.BLL.Link().Exists(kid))
            {
                //统计链入
                string xmlPath = "/Controls/link.xml";
                if (ub.GetSub("LinkIsPc", xmlPath) == "0")
                {
                    if (Utils.IsMobileUa())
                    {
                        new BCW.BLL.Link().UpdateLinkIn(kid);
                        if (ub.GetSub("LinkGoUrl", xmlPath) != "")
                        {
                            Response.Redirect(ub.GetSub("LinkGoUrl", xmlPath));
                        }
                    }
                }
                else
                {
                    new BCW.BLL.Link().UpdateLinkIn(kid);
                    if (ub.GetSub("LinkGoUrl", xmlPath) != "")
                    {
                        Response.Redirect(ub.GetSub("LinkGoUrl", xmlPath));
                    }
                }
            }
        }
        //-----------友链链入结束
    }

    /// <summary>
    /// VIP等级可见
    /// </summary>
    private bool IsVipSeen(int meid, int VipLeven)
    {
        bool bl = true;
        if (VipLeven != 0)
        {
            if (meid == 0)
            {
                bl = false;
            }
            else
            {
                int myVipLeven = BCW.User.Users.VipLeven(meid);
                if (myVipLeven < VipLeven)
                {
                    bl = false;
                }
            }
        }
        return bl;
    }

}