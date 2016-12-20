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

public partial class Manage_guess_plGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]*$", "0"));
        //if (id > 0)
        //{
        //    TPR.Model.guess.BaPay pay = new TPR.BLL.guess.BaPay().GetModel(id);
        //    if (pay == null)
        //    {
        //        Utils.Error("不存在的记录", "");
        //    }
        //    Master.Title = "修改会员下注";
        //    string info = Utils.GetRequest("info", "all", 1, "", "");
        //    if (info != "")
        //    {
        //        if (info == "ok1")
        //        {
        //            DateTime paytimes = Utils.ParseTime(Utils.GetRequest("paytimes", "post", 2, DT.RegexTime, "请正确填写下注时间"));
        //            decimal payonLuone = Convert.ToDecimal(Utils.GetRequest("payonLuone", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写上盘赔率,小数点后保留1-2位"));
        //            decimal payonLutwo = Convert.ToDecimal(Utils.GetRequest("payonLutwo", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写上盘赔率,小数点后保留1-2位"));
        //            int p_pn = Utils.ParseInt(Utils.GetRequest("p_pn", "post", 2, @"^[1-2]$", "请正确选择让球类型"));
        //            decimal p_pk = Convert.ToDecimal(Utils.GetRequest("p_pk", "post", 2, @"^[0-9]{1,21}$", "请正确选择上下盘口"));

        //            string cmd = "update tb_BaPay set paytimes='" + paytimes + "',payonLuone=" + payonLuone + ",payonLutwo=" + payonLutwo + ",p_pn=" + p_pn + ",p_pk=" + p_pk + " where id=" + id + "";
        //            BCW.Data.SqlHelper.ExecuteSql(cmd);
        //        }
        //        else if (info == "ok2")
        //        {
        //            DateTime paytimes = Utils.ParseTime(Utils.GetRequest("paytimes", "post", 2, DT.RegexTime, "请正确填写下注时间"));
        //            decimal payonLuone = Convert.ToDecimal(Utils.GetRequest("payonLuone", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写大盘赔率,小数点后保留1-2位"));
        //            decimal payonLutwo = Convert.ToDecimal(Utils.GetRequest("payonLutwo", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写小盘赔率,小数点后保留1-2位"));
        //            decimal p_dx_pk = Convert.ToDecimal(Utils.GetRequest("p_dx_pk", "post", 2, @"^[0-9]{1,21}$", "请正确选择大小盘口"));

        //            string cmd = "update tb_BaPay set paytimes='" + paytimes + "',payonLuone=" + payonLuone + ",payonLutwo=" + payonLutwo + ",p_dx_pk=" + p_dx_pk + " where id=" + id + "";
        //            BCW.Data.SqlHelper.ExecuteSql(cmd);
        //        }
        //        else if (info == "ok3")
        //        {
        //            DateTime paytimes = Utils.ParseTime(Utils.GetRequest("paytimes", "post", 2, DT.RegexTime, "请正确填写下注时间"));
        //            decimal payonLuone = Convert.ToDecimal(Utils.GetRequest("payonLuone", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写主胜赔率,小数点后保留1-2位"));
        //            decimal payonLutwo = Convert.ToDecimal(Utils.GetRequest("payonLutwo", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写平手赔率,小数点后保留1-2位"));
        //            decimal payonLuthr = Convert.ToDecimal(Utils.GetRequest("payonLuthr", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写客胜赔率,小数点后保留1-2位"));

        //            string cmd = "update tb_BaPay set paytimes='" + paytimes + "',payonLuone=" + payonLuone + ",payonLutwo=" + payonLutwo + ",payonLuthr=" + payonLuthr + " where id=" + id + "";
        //            BCW.Data.SqlHelper.ExecuteSql(cmd);
        //        }
        //        if (info == "ok4")
        //        {
        //            DateTime paytimes = Utils.ParseTime(Utils.GetRequest("paytimes", "post", 2, DT.RegexTime, "请正确填写下注时间"));
        //            decimal payonLuone = Convert.ToDecimal(Utils.GetRequest("payonLuone", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写上盘赔率,小数点后保留1-2位"));
        //            decimal payonLutwo = Convert.ToDecimal(Utils.GetRequest("payonLutwo", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写上盘赔率,小数点后保留1-2位"));
        //            decimal p_pk = Convert.ToDecimal(Utils.GetRequest("p_pk", "post", 2, @"^(-)?(\d)*(\.(\d){1})?$", "请正确填写上下盘口"));

        //            string cmd = "update tb_BaPay set paytimes='" + paytimes + "',payonLuone=" + payonLuone + ",payonLutwo=" + payonLutwo + ",p_pk=" + p_pk + " where id=" + id + "";
        //            BCW.Data.SqlHelper.ExecuteSql(cmd);
        //        }
        //        else if (info == "ok5")
        //        {
        //            DateTime paytimes = Utils.ParseTime(Utils.GetRequest("paytimes", "post", 2, DT.RegexTime, "请正确填写下注时间"));
        //            decimal payonLuone = Convert.ToDecimal(Utils.GetRequest("payonLuone", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写大盘赔率,小数点后保留1-2位"));
        //            decimal payonLutwo = Convert.ToDecimal(Utils.GetRequest("payonLutwo", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写小盘赔率,小数点后保留1-2位"));
        //            decimal p_dx_pk = Convert.ToDecimal(Utils.GetRequest("p_dx_pk", "post", 2, @"^(-)?(\d)*(\.(\d){1})?$", "请正确填写大小盘口"));

        //            string cmd = "update tb_BaPay set paytimes='" + paytimes + "',payonLuone=" + payonLuone + ",payonLutwo=" + payonLutwo + ",p_dx_pk=" + p_dx_pk + " where id=" + id + "";
        //            BCW.Data.SqlHelper.ExecuteSql(cmd);
        //        }
        //        Utils.Success("修改下注", "修改下注成功..", Utils.getUrl("plGuess.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        //    }
        //    else
        //    {
        //        builder.Append(Out.Tab("<div class=\"title\">", ""));
        //        builder.Append("修改会员下注");
        //        builder.Append(Out.Tab("</div>", ""));
        //        if (pay.pType == 1)
        //        {
        //            if (pay.PayType == 1 || pay.PayType == 2)
        //            {
        //                string strText = "下注时间:/,上盘赔率,下盘赔率,让球类型,上下盘口,,,";
        //                string strName = "paytimes,payonLuone,payonLutwo,p_pn,p_pk,id,info,backurl";
        //                string strType = "date,text,text,select,select,hidden,hidden,hidden";
        //                string strValu = "" + DT.FormatDate(Convert.ToDateTime(pay.paytimes), 0) + "'" + Convert.ToDouble(pay.payonLuone) + "'" + Convert.ToDouble(pay.payonLutwo) + "'" + pay.p_pn + "'" + Convert.ToDouble(pay.p_pk) + "'" + id + "'ok1'" + Utils.getPage(0) + "";
        //                string strEmpt = "false,false,false,1|让球|2|受让,1|平手|2|平手/半球|3|半球|4|半球/一球|5|一球|6|一球/球半|7|球半|8|球半/二球|9|二球|10|二球/二球半|11|二球半|12|二球半/三球|13|三球|14|三球/三球半|15|三球半|16|三球半/四球|17|四球|18|四球/四球半|19|四球半|20|四球半/五球|21|五球,false,false,false";
        //                string strIdea = "/";
        //                string strOthe = "确定修改,plGuess.aspx,post,1,red";
        //                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //            }
        //            else if (pay.PayType == 3 || pay.PayType == 4)
        //            {
        //                string strText = "下注时间:/,大盘赔率,小盘赔率,大小盘口,,,";
        //                string strName = "paytimes,payonLuone,payonLutwo,p_dx_pk,id,info,backurl";
        //                string strType = "date,text,text,select,hidden,hidden,hidden";
        //                string strValu = "" + DT.FormatDate(Convert.ToDateTime(pay.paytimes), 0) + "'" + Convert.ToDouble(pay.payonLuone) + "'" + Convert.ToDouble(pay.payonLutwo) + "'" + Convert.ToDouble(pay.p_dx_pk) + "'" + id + "'ok2'" + Utils.getPage(0) + "";
        //                string strEmpt = "false,false,false,20|0.5|21|0.5/1.0|22|1.0|23|1.0/1.5|1|1.5|2|1.5/2.0|3|2.0|4|2/2.5|5|2.5|6|2.5/3.0|7|3.0|8|3/3.5|9|3.5|10|3.5/4.0|11|4.0|12|4/4.5|13|4.5|14|4.5/5.0|15|5.0|16|5/5.5|17|5.5|18|5.5/6.0|19|6.0|24|6.0/6.5|25|6.5|26|6.5/7.0|27|7.0|28|7.0/7.5|29|7.5|30|7.5/8.0|31|8.0|32|8.0/8.5|33|8.5|34|8.5/9.0|35|9.0|36|9.0/9.5|37|9.5|38|9.5/10.0|39|10.0,false,false,false";
        //                string strIdea = "/";
        //                string strOthe = "确定修改,plGuess.aspx,post,1,red";
        //                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //            }
        //            else
        //            {
        //                string strText = "下注时间:/,标准主胜,标准平手,标准客胜,,,,";
        //                string strName = "paytimes,payonLuone,payonLutwo,payonLuthr,id,info,backurl";
        //                string strType = "date,text,text,text,hidden,hidden,hidden";
        //                string strValu = "" + DT.FormatDate(Convert.ToDateTime(pay.paytimes), 0) + "'" + Convert.ToDouble(pay.payonLuone) + "'" + Convert.ToDouble(pay.payonLutwo) + "'" + Convert.ToDouble(pay.payonLuthr) + "'" + id + "'ok3'" + Utils.getPage(0) + "";
        //                string strEmpt = "false,false,false,false,false,false,false";
        //                string strIdea = "/";
        //                string strOthe = "确定修改,plGuess.aspx,post,1,red";
        //                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //            }
        //        }
        //        else
        //        {
        //            if (pay.PayType == 1 || pay.PayType == 2)
        //            {
        //                string strText = "下注时间:/,上盘赔率,下盘赔率,上下盘口,,,";
        //                string strName = "paytimes,payonLuone,payonLutwo,p_pk,id,info,backurl";
        //                string strType = "date,text,text,text,hidden,hidden,hidden";
        //                string strValu = "" + DT.FormatDate(Convert.ToDateTime(pay.paytimes), 0) + "'" + Convert.ToDouble(pay.payonLuone) + "'" + Convert.ToDouble(pay.payonLutwo) + "'" + Convert.ToDouble(pay.p_pk) + "'" + id + "'ok4'" + Utils.getPage(0) + "";
        //                string strEmpt = "false,false,false,false,false,false,false";
        //                string strIdea = "/";
        //                string strOthe = "确定修改,plGuess.aspx,post,1,red";
        //                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //            }
        //            else
        //            {
        //                string strText = "下注时间:/,大盘赔率,小盘赔率,大小盘口,,,";
        //                string strName = "paytimes,payonLuone,payonLutwo,p_dx_pk,id,info,backurl";
        //                string strType = "date,text,text,text,hidden,hidden,hidden";
        //                string strValu = "" + DT.FormatDate(Convert.ToDateTime(pay.paytimes), 0) + "'" + Convert.ToDouble(pay.payonLuone) + "'" + Convert.ToDouble(pay.payonLutwo) + "'" + Convert.ToDouble(pay.p_dx_pk) + "'" + id + "'ok5'" + Utils.getPage(0) + "";
        //                string strEmpt = "false,false,false,false,false,false,false";
        //                string strIdea = "/";
        //                string strOthe = "确定修改,plGuess.aspx,post,1,red";
        //                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //            }
        //        }
        //        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        //        builder.Append(Out.Tab("<div>", ""));
        //        builder.Append(Out.waplink(Utils.getPage("default,aspx"), "返回上一级"));
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //}
        //else
        //{
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "竞猜ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "get", 2, @"^[1-7]*$", "选择无效"));
        TPR.BLL.guess.BaList bll = new TPR.BLL.guess.BaList();

        if (bll.GetModel(gid) == null)
        {
            Utils.Error("不存在的记录", "");
        }
        TPR.Model.guess.BaList model = bll.GetModel(gid);

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
        string[] pageValUrl = { "gid", "p" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        strWhere += "bcid=" + gid + " and PayType=" + p + "";

        // 开始读取竞猜
        IList<TPR.Model.guess.BaPay> listBaPay = new TPR.BLL.guess.BaPay().GetBaPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPay.Count > 0)
        {
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

                //builder.AppendFormat(Out.waplink(Utils.getUrl("plGuess.aspx?id={0}&amp;backurl=" + Utils.PostPage(1) + ""), "[管理]") + "&gt;", n.ID);

                builder.AppendFormat(Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{0}({1})["+n.ID+"]") + ":{2}[{3}]", n.payusname, n.payusid, Out.SysUBB(n.payview), n.paytimes);

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
}
