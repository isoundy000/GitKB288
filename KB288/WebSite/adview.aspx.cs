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

public partial class adview : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        if (id > 0)
        {
            if (!new BCW.BLL.Advert().Exists2(id))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.Model.Advert model = new BCW.BLL.Advert().GetAdvert(id);
            //按天/按周计算
            if (meid > 0)
            {
                if (model.iGold > 0)
                {
                    if (model.adType <= 1)
                    {
                        int Day = 1;
                        if (model.adType == 1)
                            Day = 7;

                        //根据时限清空点击ID
                        string ClickID = model.ClickID;
                        if (string.IsNullOrEmpty(model.ClickTime.ToString()) || DT.TwoDateDiff(DateTime.Now, model.ClickTime) >= Day)
                        {
                            new BCW.BLL.Advert().UpdateClickID(id);
                            ClickID = string.Empty;
                        }

                        if (string.IsNullOrEmpty(ClickID) || ClickID.IndexOf("#" + meid + "#") == -1)
                        {
                            new BCW.BLL.Advert().UpdateClickID(id, ClickID + "#" + meid + "#");
                            //得币
                            new BCW.BLL.User().UpdateiGold(meid, Convert.ToInt64(model.iGold), "点广告得币");
                            //内线
                            if (ub.GetSub("BbsIsAdMsg", xmlPath) == "1")
                            {
                                new BCW.BLL.Guest().Add(meid, new BCW.BLL.User().GetUsName(meid), "支持站长,点击广告！恭喜您获得" + model.iGold + "" + ub.Get("SiteBz") + "，天天点击天天有惊喜~");
                            }
                        }
                    }
                    else//按次计算
                    {
                        new BCW.BLL.Advert().UpdateClickID(id, "");
                        //得币
                        new BCW.BLL.User().UpdateiGold(meid, Convert.ToInt64(model.iGold), "点广告得币");
                        //内线
                        if (ub.GetSub("BbsIsAdMsg", xmlPath) == "1")
                        {
                            new BCW.BLL.Guest().Add(meid, new BCW.BLL.User().GetUsName(meid), "支持站长,点击广告！恭喜您获得" + model.iGold + "" + ub.Get("SiteBz") + "，天天点击天天有惊喜~");
                        }
                    }
                }
            }
            //跳转广告
            new BCW.BLL.Advert().UpdateClick(id);
            Response.Redirect(model.AdUrl.Replace("&amp;", "&"));
        }
        else
        {
            Master.Title = "打工送币";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=打工送币=");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            strWhere = "Status=0 and UrlType=0 and StartTime<='" + DateTime.Now + "' and OverTime>='" + DateTime.Now + "'";

            // 开始读取列表
            IList<BCW.Model.Advert> listAdvert = new BCW.BLL.Advert().GetAdverts(pageIndex, pageSize, strWhere, out recordCount);
            if (listAdvert.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Advert n in listAdvert)
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
                    string adType = string.Empty;
                    if (n.adType == 0)
                        adType = "/天";
                    else if (n.adType == 1)
                        adType = "/周";
                    else
                        adType = "";

                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("adview.aspx?id={1}") + "\">{2}</a>(" + n.iGold + "" + ub.Get("SiteBz") + "" + adType + "/次)", (pageIndex - 1) * pageSize + k, n.ID, n.Title);
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
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("支持站长,请点击广告,天天送你币！");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
}