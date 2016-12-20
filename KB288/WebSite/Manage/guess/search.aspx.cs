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

public partial class Manage_guess_search : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "搜索用户下注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索下注");
        int UsID = Utils.ParseInt(Utils.GetRequest("UsID", "all", 1, @"^[0-9]\d*$", "会员ID无效"));
        if (UsID != 0)
        {
            string UsName = new BCW.BLL.User().GetUsName(UsID);
            if (UsName == "")
                UsName = "不存在的用户";

            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + UsID + ")</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "搜索下注" || ac == Utils.ToTChinese("搜索下注"))
        {
            int Ptype, Ltype, Xtype, Ttype, iTset;
            string sTime = "";
            string oTime = "";
            Ptype = Utils.ParseInt(Utils.GetRequest("Ptype", "all", 2, @"^[0-2]$", "球类选择无效"));
            Ltype = Utils.ParseInt(Utils.GetRequest("Ltype", "all", 2, @"^[0-3]$", "类型选择无效"));
            Xtype = Utils.ParseInt(Utils.GetRequest("Xtype", "all", 2, @"^[0-2]+$", "性质选择无效"));
            Ttype = Utils.ParseInt(Utils.GetRequest("Ttype", "all", 2, @"^[1-5]+$", "限制选择无效"));
            iTset = Utils.ParseInt(Utils.GetRequest("iTset", "all", 2, @"^[1-3]+$", "局域选择无效"));
            if (Ttype == 4)
            {
                sTime = Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效");
                oTime = Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效");

            }

            int pageSize = 10;
            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "UsID", "Ptype", "Ltype", "Xtype", "Ttype", "sTime", "oTime", "iTset", "ac" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //组件条件查询
            string strWhere = "";
            if (Ptype != 0)
                strWhere += "ptype=" + Ptype + " and itypes=0";
            else
                strWhere += "itypes=0 ";

            if (UsID != 0)
                strWhere += "and payusid=" + UsID + " ";
            if (Ltype != 3)
                strWhere += "and p_active=" + Ltype + " ";

            if (Xtype == 1)
                strWhere += "and p_getMoney>0 ";
            else if (Xtype == 2)
                strWhere += "and p_getMoney=0 ";
            if (Ttype == 1)
                strWhere += "and paytimes='" + DateTime.Now.ToLongDateString() + "'";
            else if (Ttype == 2)
                strWhere += "and paytimes>='" + DateTime.Now.AddDays(-7) + "'";
            else if (Ttype == 3)
                strWhere += "and paytimes>='" + DateTime.Now.AddDays(-30) + "'";
            else if (Ttype == 4)
            {
                if (iTset == 1)
                    strWhere += "and paytimes>='" + sTime + "' and paytimes<='" + oTime + "'";
                else if (iTset == 2)
                    strWhere += "and paytimes>'" + sTime + "' and paytimes<'" + oTime + "'";
                else if (iTset == 3)
                    strWhere += "and paytimes NOT BETWEEN '" + sTime + "' and '" + oTime + "'";
            }
            // 开始读取竞猜
            IList<TPR.Model.guess.BaPay> listBaPay = new TPR.BLL.guess.BaPay().GetBaPays(pageIndex, pageSize, strWhere, out recordCount);
            if (listBaPay.Count > 0)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("共有" + recordCount + "条结果");
                builder.Append(Out.Tab("</div>", "<br />"));
                int k = 1;
                foreach (TPR.Model.guess.BaPay n in listBaPay)
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
                    if (UsID == 0)
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.payusname + "(" + n.payusid + ")</a>:");

                    builder.AppendFormat("{0}<br />时间:{1}", Out.SysUBB(n.payview), n.paytimes);
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
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(Out.waplink(Utils.getUrl("search.aspx"), "继续搜索"));
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string strText = "*输入会员ID:/,球类:,类型:,性质:,限制:,开始时间:,结束时间:,时间局域:";
            string strName = "UsID,Ptype,Ltype,Xtype,Ttype,sTime,oTime,iTset";
            string strType = "text,select,select,select,select,date,date,select";
            string strValu = "0'0'3'0'5'" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'1";
            string strEmpt = "true,0|全部|1|足球|2|篮球,3|全部下注|0|未开下注|1|已开下注|2|平盘下注,0|全部下注|1|中奖下注|2|未中下注,1|今天所有|2|一个星期|3|一个月内|4|指定时间|5|不限时间,true,true,1|=&lt;&gt;=两者之间|2|&lt;&gt;介负于两者|3|=&gt;&lt;=两者之外";
            string strIdea = "/温馨提示:ID为0时搜索所有用户,选择限定时间需填写时间类型/";
            string strOthe = "搜索下注,search.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
