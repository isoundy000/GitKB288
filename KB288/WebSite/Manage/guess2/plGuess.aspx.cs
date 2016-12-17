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
/// 增加下注状态操作
/// 
/// 黄国军 20160324
/// </summary>
public partial class Manage_guess2_plGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "olddata":
                OldDataPage();
                break;
            case "back":
                BackPage();
                break;
            case "backsave":
                BackSavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }


    private void OldDataPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("球彩旧下注数据对照");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = BCW.Data.SqlHelper.Query("select * from OLDPAY where itypes=0");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string ID = ds.Tables[0].Rows[i]["ID"].ToString();
            string payusid = ds.Tables[0].Rows[i]["payusid"].ToString();
            string payusname = ds.Tables[0].Rows[i]["payusname"].ToString();
            string payview = ds.Tables[0].Rows[i]["payview"].ToString();
            string paytimes = ds.Tables[0].Rows[i]["paytimes"].ToString();
            builder.AppendFormat(Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{0}({1})[" + ID + "]") + ":{2}[{3}]<br />", payusname, payusid, Out.SysUBB(payview).Replace("/bbs/guess2/", ""), paytimes);
        }
    }

    private void ReloadPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "get", 2, @"^[1-7]*$|^100$", "选择无效"));
        string ok = Utils.GetRequest("ok", "all", 1, "", "");

        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();

        if (bll.GetModel(gid) == null)
        {
            Utils.Error("不存在的记录", "");
        }
        TPR2.Model.guess.BaList model = bll.GetModel(gid);

        #region 确认下注
        if (ok == "sure")
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "ID无效"));
            TPR2.Model.guess.BaPay bpmodel = new TPR2.BLL.guess.BaPay().GetModelIsCase(id);
            if (bpmodel != null)
            {
                new TPR2.BLL.guess.BaPay().UpdateSure(id);
            }
        }
        #endregion

        Master.Title = model.p_one + "VS" + model.p_two;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.p_one + "VS" + model.p_two);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (p == 1)
            builder.Append("让球盘:<b>上盘</b>下注列表");
        else if (p == 2)
            builder.Append("让球盘:<b>下盘</b>下注列表");
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
        else if (p == 100)
            builder.Append("波胆盘:下注列表");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "gid", "p" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        if (p < 100)
            strWhere += "itypes=0 and bcid=" + gid + " and PayType=" + p + "";
        else
            strWhere += "itypes=0 and bcid=" + gid + " and PayType>100";

        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPay.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPay n in listBaPay)
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
                string bzType = string.Empty;
                if (n.Types == 0)
                    bzType = ub.Get("SiteBz");
                else
                    bzType = ub.Get("SiteBz2");


                builder.AppendFormat(Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{0}({1})[" + n.ID + "]") + ":{2}[{3}]", n.payusname, n.payusid, Out.SysUBB(n.payview).Replace("/bbs/guess2/", ""), n.paytimes);
                builder.Append(Out.waplink(Utils.getUrl("plguess.aspx?act=back&amp;id=" + n.ID + "&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "[退]"));

                #region 下注状态
                //确定下注
                if (n.state > 0)
                {
                    if (n.sure == 1)
                    {
                        builder.Append(Out.waplink(Utils.getUrl("plguess.aspx?ok=sure&amp;p=" + p + "&amp;id=" + n.ID + "&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "[确]"));
                    }
                    else
                    {
                        builder.Append("[待]");
                    }
                }
                if (n.state == 0) { builder.Append("[成]"); }
                #endregion

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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void BackPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));

        if (!new TPR2.BLL.guess.BaList().Exists(gid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.BaPay model = new TPR2.BLL.guess.BaPay().GetModelIsCase(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "撤销押注并退回本金";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("撤销押注并退回本金");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "您的" + Out.SysUBB(model.payview).Replace("/", "／").Replace(",", "，") + "竞猜失败已作退回本金处理，/,,,,";
        string strName = "Content,gid,id,act,backurl";
        string strType = "text,hidden,hidden,hidden,hidden";
        string strValu = "'" + gid + "'" + id + "'backsave'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "本条撤销并退本金,plguess.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void BackSavePage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "post", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID无效"));

        if (!new TPR2.BLL.guess.BaList().Exists(gid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.BaPay model = new TPR2.BLL.guess.BaPay().GetModelIsCase(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,200}$", "原因限200字内，可以留空");
        string Msgtxt = string.Empty;
        if (Content == "")
        {
            Msgtxt = model.payview + "竞猜失败已作退回本金处理";

        }
        else
        {
            Msgtxt = model.payview + "竞猜失败已作退回本金处理，" + Content + "";
        }

        new BCW.BLL.Guest().Add(Convert.ToInt32(model.payusid), model.payusname, Msgtxt);
        //退本金
        if (model.Types == 0)
            new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.payusid), model.payusname, Convert.ToInt64(model.payCent), model.payview + "系统撤销押注并退回本金");
        else
            new BCW.BLL.User().UpdateiMoney(Convert.ToInt32(model.payusid), model.payusname, Convert.ToInt64(model.payCent), model.payview + "系统撤销押注并退回本金");

        //删除处理
        new TPR2.BLL.guess.BaPay().Delete(id);
        Utils.Success("撤销押注", "撤销押注并退回本金成功...", Utils.getPage("showguess.aspx?gid=" + gid + ""), "1");
    }
}
