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

public partial class Manage_guess_addGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "增加赛事";
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (ac == "确定添加" || ac == Utils.ToTChinese("确定添加"))
        {
            string p_title = Out.UBB(Utils.GetRequest("p_title", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9_\-\s]+$", "请正确填写联赛名称"));
            DateTime p_TPRtime = Utils.ParseTime(Utils.GetRequest("p_TPRtime", "post", 2, DT.RegexTime, "请正确填写联赛时间"));
            string p_one = Out.UBB(Utils.GetRequest("p_one", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9_\-\s]+$", "请正确填写上盘名称"));
            string p_two = Out.UBB(Utils.GetRequest("p_two", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9_\-\s]+$", "请正确填写下盘名称"));
            decimal p_one_lu = Convert.ToDecimal(Utils.GetRequest("p_one_lu", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "请正确填写上盘赔率,小数点后保留1-2位"));
            decimal p_two_lu = Convert.ToDecimal(Utils.GetRequest("p_two_lu", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "请正确填写下盘赔率,小数点后保留1-2位"));

            decimal p_big_lu = Convert.ToDecimal(Utils.GetRequest("p_big_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写大盘赔率,小数点后保留1-2位"));
            decimal p_small_lu = Convert.ToDecimal(Utils.GetRequest("p_small_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写小盘赔率,小数点后保留1-2位"));


            int p_pn = 0;
            decimal p_pk = 0;
            decimal p_dx_pk = 0;
            decimal p_bzs_lu = 0;
            decimal p_bzp_lu = 0;
            decimal p_bzx_lu = 0;
            if (ptype == 1)
            {//足球特征
                p_pn = Utils.ParseInt(Utils.GetRequest("p_pn", "post", 2, @"^[1-2]$", "请正确选择让球类型"));
                p_pk = Convert.ToDecimal(Utils.GetRequest("p_pk", "post", 2, @"^[0-9]{1,21}$", "请正确选择上下盘口"));
                p_dx_pk = Convert.ToDecimal(Utils.GetRequest("p_dx_pk", "post", 2, @"^[0-9]{1,19}$", "请正确选择大小盘口"));
                if (Request["p_bzs_lu"] != "")
                {
                    p_bzs_lu = Convert.ToDecimal(Utils.GetRequest("p_bzs_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写主胜赔率,小数点后保留1-2位"));
                    p_bzp_lu = Convert.ToDecimal(Utils.GetRequest("p_bzp_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写平手赔率,小数点后保留1-2位"));
                    p_bzx_lu = Convert.ToDecimal(Utils.GetRequest("p_bzx_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写客胜赔率,小数点后保留1-2位"));
                }
                else
                {
                    p_bzs_lu = 0;
                    p_bzp_lu = 0;
                    p_bzx_lu = 0;
                }
            }
            else
            {//篮球特征
                p_pk = Convert.ToDecimal(Utils.GetRequest("p_pk", "post", 2, @"^(-)?(\d)*(\.(\d){1})?$", "请正确填写上下盘口"));
                p_dx_pk = Convert.ToDecimal(Utils.GetRequest("p_dx_pk", "post", 2, @"^(-)?(\d)*(\.(\d){1})?$", "请正确填写大小盘口"));

            }
            int p_ison = Convert.ToInt32(Utils.GetRequest("p_ison", "post", 1, @"^[0-1]$", "0"));


            //联赛限制显示
            string xmlPath = "/Controls/guess.xml";
            string Levens = "";
            if (ptype == 1)
                Levens = "#" + ub.GetSub("SiteLeven1", xmlPath) + "#" + ub.GetSub("SiteLeven2", xmlPath) + "#" + ub.GetSub("SiteLeven3", xmlPath) + "#" + ub.GetSub("SiteLeven4", xmlPath) + "#";
            else
                Levens = "#" + ub.GetSub("SiteLevenb1", xmlPath) + "#" + ub.GetSub("SiteLevenb2", xmlPath) + "#" + ub.GetSub("SiteLevenb3", xmlPath) + "#" + ub.GetSub("SiteLevenb4", xmlPath) + "#";
            
            if (!Levens.Contains("#" + p_title + "#"))
            {
                Utils.Error("在四个联赛分级中不存在“" + p_title + "”联赛，请设置才能继续...", "");
            }


            //游戏日志记录
            int ManageId = new BCW.User.Manage().IsManageLogin();
            int gid = new TPR.BLL.guess.BaList().GetMaxId();
            string[] p_pageArr = { "ac", "p_title", "ptype", "p_one", "p_two", "p_pk", "p_dx_pk", "p_pn", "p_one_lu", "p_two_lu", "p_big_lu", "p_small_lu", "p_bzs_lu", "p_bzp_lu", "p_bzx_lu", "p_TPRtime", "p_ison" };
            BCW.User.GameLog.GameLogPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号增加赛事" + p_one + "VS" + p_two + "(" + gid + ")", gid);

            //写入数据
            TPR.Model.guess.BaList model = new TPR.Model.guess.BaList();
            model.p_id = 0;
            model.p_title = p_title;
            model.p_type = ptype;
            model.p_one = p_one;
            model.p_two = p_two;
            model.p_pk = p_pk;
            model.p_dx_pk = p_dx_pk;
            model.p_pn = p_pn;
            model.p_one_lu = p_one_lu;
            model.p_two_lu = p_two_lu;
            model.p_big_lu = p_big_lu;
            model.p_small_lu = p_small_lu;
            model.p_bzs_lu = p_bzs_lu;
            model.p_bzp_lu = p_bzp_lu;
            model.p_bzx_lu = p_bzx_lu;
            model.p_addtime = DateTime.Now;
            model.p_TPRtime = p_TPRtime;
            model.p_ison = p_ison;
            model.p_del = 0;
            new TPR.BLL.guess.BaList().Add(model);
            Utils.Success("添加赛事", "添加赛事成功..<br />" + Out.waplink(Utils.getUrl("addGuess.aspx"), "继续添加") + "", Utils.getUrl("default.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("增加赛事");
            builder.Append(Out.Tab("</div>", "<br />"));
            if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("增加:足球赛事 " + Out.waplink(Utils.getUrl("addGuess.aspx?ptype=2"), "切换篮球"));
                builder.Append(Out.Tab("</div>", ""));
                string strText = "联赛名称,联赛时间,上盘名称,上盘赔率,下盘名称,下盘赔率,让球类型,上下盘口,大盘赔率,小盘赔率,大小盘口,主胜赔率,平手赔率,客胜赔率,是否开走地,";
                string strName = "p_title,p_TPRtime,p_one,p_one_lu,p_two,p_two_lu,p_pn,p_pk,p_big_lu,p_small_lu,p_dx_pk,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_ison,ptype";
                string strType = "text,date,text,text,text,text,select,select,text,text,select,text,text,text,select,hidden";
                string strValu = "'" + DT.FormatDate(DateTime.Now.AddHours(5), 0) + "'''''1'1'''1''''0'" + ptype + "";
                string strEmpt = "flase,false,false,false,false,false,1|让球|2|受让,1|平手|2|平手/半球|3|半球|4|半球/一球|5|一球|6|一球/球半|7|球半|8|球半/二球|9|二球|10|二球/二球半|11|二球半|12|二球半/三球|13|三球|14|三球/三球半|15|三球半|16|三球半/四球|17|四球|18|四球/四球半|19|四球半|20|四球半/五球|21|五球,true,true,20|0.5|21|0.5/1.0|22|1.0|23|1.0/1.5|1|1.5|2|1.5/2.0|3|2.0|4|2/2.5|5|2.5|6|2.5/3.0|7|3.0|8|3/3.5|9|3.5|10|3.5/4.0|11|4.0|12|4/4.5|13|4.5|14|4.5/5.0|15|5.0|16|5/5.5|17|5.5|18|5.5/6.0|19|6.0|24|6.0/6.5|25|6.5|26|6.5/7.0|27|7.0|28|7.0/7.5|29|7.5|30|7.5/8.0|31|8.0|32|8.0/8.5|33|8.5|34|8.5/9.0|35|9.0|36|9.0/9.5|37|9.5|38|9.5/10.0|39|10.0,true,true,true,0|否|1|是,true";
                string strIdea = "/联赛时间格式:" + DT.FormatDate(DateTime.Now.AddHours(5), 0) + "/";
                string strOthe = "确定添加,addGuess.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("大小盘赔率与标准盘填写-1可隐藏，即不可下注大小盘与标准盘！");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("增加:足球赛事 " + Out.waplink(Utils.getUrl("addGuess.aspx?ptype=1"), "切换足球"));
                builder.Append(Out.Tab("</div>", ""));
                string strText = "联赛名称,联赛时间,上盘名称,上盘赔率,下盘名称,下盘赔率,上下盘口,大盘赔率,小盘赔率,大小盘口,";
                string strName = "p_title,p_TPRtime,p_one,p_one_lu,p_two,p_two_lu,p_pk,p_big_lu,p_small_lu,p_dx_pk,ptype";
                string strType = "text,date,text,text,text,text,text,text,text,text,hidden";
                string strValu = "'" + DateTime.Now.AddHours(5) + "'''''''''" + ptype + "";
                string strEmpt = "flase,false,false,false,false,false,false,true,true,true,true";
                string strIdea = "/联赛时间格式:" + DT.FormatDate(DateTime.Now.AddHours(5), 0) + "/";
                string strOthe = "确定添加,addGuess.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("大小盘赔率填写-1可隐藏大小盘，即不可下注大小盘！");
                builder.Append(Out.Tab("</div>", ""));
            }
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