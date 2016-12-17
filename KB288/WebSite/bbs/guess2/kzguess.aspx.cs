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
using TPR2.Common;

public partial class bbs_guess_kzguess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {        
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int VipLeven = BCW.User.Users.VipLeven(meid);
        if (VipLeven == 0)
        {
            Utils.Error("必须要是VIP会员才可以开庄", "");
        }

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "0"));

        TPR2.Model.guess.BaList model = new TPR2.BLL.guess.BaList().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.p_del == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        //if (model.p_TPRtime <= DateTime.Now)
        //{
        //    Utils.Error("开赛时间已到，暂停开庄", "");
        //}

        Master.Title = model.p_one + "VS" + model.p_two;
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定开庄")
        {
            long payCent = Int64.Parse(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "接受投注总额填写错误"));

            long gold = new BCW.BLL.User().GetGold(meid);
            long xCent = Convert.ToInt64(ub.GetSub("SitexCent", xmlPath));
            if (gold < xCent)
            {
                Utils.Error("自带的" + ub.Get("SiteBz") + "必须达到" + xCent + "才可以开庄", "");
            }
            if (gold < payCent)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }

            TPR2.Model.guess.BaListMe m = new TPR2.Model.guess.BaListMe();
            if (ptype == 1)
            {
                DateTime oncetime = DateTime.Now;
                int p_ison = Utils.ParseInt(Utils.GetRequest("p_ison", "post", 1, @"^[0-2]$", "0"));
                if (p_ison == 1)
                {
                    oncetime = Utils.ParseTime(Utils.GetRequest("oncetime", "post", 2, DT.RegexTime, "请正确填写封盘时间"));

                    if (Convert.ToDateTime(model.p_TPRtime) > oncetime)
                    {
                        Utils.Error("封盘时间应大于开赛时间", "");

                    }

                }
                else
                {
                    if (model.p_TPRtime <= DateTime.Now)
                    {
                        Utils.Error("开打的比赛必须选择走地模式来开庄", "");
                    }
                }
                //写入数据
                m.p_id = model.p_id;
                m.p_title = model.p_title;
                m.p_type = model.p_type;
                m.p_one = model.p_one;
                m.p_two = model.p_two;
                m.p_pk = model.p_pk;
                m.p_dx_pk = model.p_dx_pk;
                m.p_pn = model.p_pn;
                m.p_one_lu = model.p_one_lu;
                m.p_two_lu = model.p_two_lu;
                m.p_big_lu = model.p_big_lu;
                m.p_small_lu = model.p_small_lu;
                m.p_bzs_lu = model.p_bzs_lu;
                m.p_bzp_lu = model.p_bzp_lu;
                m.p_bzx_lu = model.p_bzx_lu;
                m.p_addtime = DateTime.Now;
                m.p_TPRtime = model.p_TPRtime;
                m.p_ison = p_ison;//赔率标识(0自动更新|1走地|2固定赔率)
                m.p_del = 0;
                m.usid = meid;
                m.payCent = payCent;

                int newId = new TPR2.BLL.guess.BaListMe().Add(m);
                if (p_ison == 1)
                {
                    new TPR2.BLL.guess.BaListMe().FootOnceType(newId, oncetime);
                }
                //同步即时比分
                BCW.Data.SqlHelper.ExecuteSql("update tb_BaListMe set p_result_temp1=" + model.p_result_temp1 + ",p_result_temp2=" + model.p_result_temp2 + " where id=" + newId + "");


                Utils.Success("开庄", "开庄成功..<br />" + Out.waplink(Utils.getUrl("kzlist.aspx"), "我的开庄列表") + "", Utils.getUrl("kzlist.aspx"), "1");
            }
            else
            {
                string p_title = Out.UBB(Utils.GetRequest("p_title", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9_\-\s]+$", "请正确填写联赛名称"));
                DateTime p_TPRtime = Utils.ParseTime(Utils.GetRequest("p_TPRtime", "post", 2, DT.RegexTime, "请正确填写联赛时间"));
                string p_one = Out.UBB(Utils.GetRequest("p_one", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9_\-\s\(\)]+$", "请正确填写上盘名称"));
                string p_two = Out.UBB(Utils.GetRequest("p_two", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9_\-\s\(\)]+$", "请正确填写下盘名称"));


                decimal p_one_lu = Convert.ToDecimal(Utils.GetRequest("p_one_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写上盘赔率,小数点后保留1-2位"));
                decimal p_two_lu = Convert.ToDecimal(Utils.GetRequest("p_two_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写下盘赔率,小数点后保留1-2位"));

                decimal p_big_lu = Convert.ToDecimal(Utils.GetRequest("p_big_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写大盘赔率,小数点后保留1-2位"));
                decimal p_small_lu = Convert.ToDecimal(Utils.GetRequest("p_small_lu", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写小盘赔率,小数点后保留1-2位"));

                int p_pn = 0;
                decimal p_pk = 0;
                decimal p_dx_pk = 0;
                decimal p_bzs_lu = 0;
                decimal p_bzp_lu = 0;
                decimal p_bzx_lu = 0;

                if (model.p_type == 1)
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
                //写入数据
                m.p_id = model.p_id;
                m.p_title = p_title;
                m.p_type = model.p_type;
                m.p_one = p_one;
                m.p_two = p_two;
                m.p_pk = p_pk;
                m.p_dx_pk = p_dx_pk;
                m.p_pn = p_pn;
                m.p_one_lu = p_one_lu;
                m.p_two_lu = p_two_lu;
                m.p_big_lu = p_big_lu;
                m.p_small_lu = p_small_lu;
                m.p_bzs_lu = p_bzs_lu;
                m.p_bzp_lu = p_bzp_lu;
                m.p_bzx_lu = p_bzx_lu;
                m.p_addtime = DateTime.Now;
                m.p_TPRtime = p_TPRtime;
                m.p_ison = 2;//赔率标识(0自动更新|1走地|2固定赔率)
                m.p_del = 0;
                m.usid = meid;
                m.payCent = payCent;

                int newId = new TPR2.BLL.guess.BaListMe().Add(m);
                //同步即时比分
                BCW.Data.SqlHelper.ExecuteSql("update tb_BaListMe set p_result_temp1=" + model.p_result_temp1 + ",p_result_temp2=" + model.p_result_temp2 + " where id=" + newId + "");


                Utils.Success("开庄", "开庄成功..<br />" + Out.waplink(Utils.getUrl("kzlist.aspx"), "我的开庄列表") + "", Utils.getUrl("kzlist.aspx"), "1");


            }

        }
        else
        {

            if (ptype == 0)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("我来坐庄：" + model.p_one + "VS" + model.p_two + "");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append(Out.waplink(Utils.getUrl("kzguess.aspx?gid=" + gid + "&amp;ptype=1"), "自动同步水位") + "<br />赔率与系统水位实时同步，节省您的时间<br />");
                builder.Append(Out.waplink(Utils.getUrl("kzguess.aspx?gid=" + gid + "&amp;ptype=2"), "固定水位开庄") + "<br />完全按照您设置的赔率开庄，当然您可以随时更新赔率<br />");
                builder.Append(Out.waplink(Utils.getPage("default.aspx"), "取消开庄") + "");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("我来坐庄：" + model.p_one + "VS" + model.p_two + "");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "接受投注总额:/,,";
                string strName = "payCent,gid,ptype";
                string strType = "num,hidden,hidden";
                string strValu = "'" + gid + "'" + ptype + "";
                string strEmpt = "true,true,true";
                if (model.p_type == 1)
                {
                    strText += ",走地模式:,走地封盘时间:";
                    strName += ",p_ison,oncetime";
                    strType += ",select,date";
                    strValu += "'0'" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime).AddMinutes(100), 0) + "";
                    strEmpt += ",0|否|1|是,true";
                }

                string strIdea = "/系统不会扣除你的接受投注总额，买家下注时则通过计算赔付进行扣除相应的押金/";
                if (model.p_type == 1)
                {
                    strIdea += "当您选择走地模式时，必须填写走地封盘时间/";
                }
                string strOthe = string.Empty;
                //if (Utils.GetDomain().Contains("168yy.cc") || Utils.GetDomain().Contains("tl88.cc"))
                //    strOthe = "" + ub.Get("SiteBz") + "开庄|" + ub.Get("SiteBz2") + "开庄,kzguess.aspx,post,0,red|blue";
                //else
                strOthe = "确定开庄,kzguess.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
            else if (ptype == 2)
            {

                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("我来坐庄：" + model.p_one + "VS" + model.p_two + "");
                builder.Append(Out.Tab("</div>", ""));
                if (model.p_type == 1)
                {
                    string strText = "联赛名称,联赛时间,上盘名称,上盘赔率,下盘名称,下盘赔率,让球类型,上下盘口,大盘赔率,小盘赔率,大小盘口,标准主胜,标准平手,标准客胜,接受投注总额:/,,,";
                    string strName = "p_title,p_TPRtime,p_one,p_one_lu,p_two,p_two_lu,p_pn,p_pk,p_big_lu,p_small_lu,p_dx_pk,p_bzs_lu,p_bzp_lu,p_bzx_lu,payCent,ptype,gid,backurl";
                    string strType = "text,text,text,text,text,text,select,select,text,text,select,text,text,text,num,hidden,hidden,hidden";
                    string strValu = "" + model.p_title + "'" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0) + "'" + model.p_one + "'" + Convert.ToDouble(model.p_one_lu) + "'" + model.p_two + "'" + Convert.ToDouble(model.p_two_lu) + "'" + model.p_pn + "'" + Convert.ToDouble(model.p_pk) + "'" + Convert.ToDouble(model.p_big_lu) + "'" + Convert.ToDouble(model.p_small_lu) + "'" + Convert.ToDouble(model.p_dx_pk) + "'" + Convert.ToDouble(model.p_bzs_lu) + "'" + Convert.ToDouble(model.p_bzp_lu) + "'" + Convert.ToDouble(model.p_bzx_lu) + "'0'" + ptype + "'" + gid + "'" + Utils.getPage(0) + "";
                    string strEmpt = "flase,false,false,false,false,false,1|让球|2|受让,1|平手|2|平手/半球|3|半球|4|半球/一球|5|一球|6|一球/球半|7|球半|8|球半/二球|9|二球|10|二球/二球半|11|二球半|12|二球半/三球|13|三球|14|三球/三球半|15|三球半|16|三球半/四球|17|四球|18|四球/四球半|19|四球半|20|四球半/五球|21|五球,true,true,20|0.5|21|0.5/1.0|22|1.0|23|1.0/1.5|1|1.5|2|1.5/2.0|3|2.0|4|2/2.5|5|2.5|6|2.5/3.0|7|3.0|8|3/3.5|9|3.5|10|3.5/4.0|11|4.0|12|4/4.5|13|4.5|14|4.5/5.0|15|5.0|16|5/5.5|17|5.5|18|5.5/6.0|19|6.0|24|6.0/6.5|25|6.5|26|6.5/7.0|27|7.0|28|7.0/7.5|29|7.5|30|7.5/8.0|31|8.0|32|8.0/8.5|33|8.5|34|8.5/9.0|35|9.0|36|9.0/9.5|37|9.5|38|9.5/10.0|39|10.0,true,true,true,false,true,true,false";
                    string strIdea = "/";
                    string strOthe = "";
                    //if (Utils.GetDomain().Contains("168yy.cc") || Utils.GetDomain().Contains("tl88.cc"))
                    //    strOthe = "" + ub.Get("SiteBz") + "开庄|" + ub.Get("SiteBz2") + "开庄,kzguess.aspx,post,0,red|blue";
                    //else
                    strOthe = "确定开庄,kzguess.aspx,post,1,red";

                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("系统不会扣除你的接受投注总额，买家下注时则通过计算赔付进行扣除相应的押金<br />赔率填写-1可隐藏某选项下注，即不可下注该选项！");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    string strText = "联赛名称,联赛时间,上盘名称,上盘赔率,下盘名称,下盘赔率,上下盘口,大盘赔率,小盘赔率,大小盘口,接受投注总额:/,,,";
                    string strName = "p_title,p_TPRtime,p_one,p_one_lu,p_two,p_two_lu,p_pk,p_big_lu,p_small_lu,p_dx_pk,payCent,ptype,gid,backurl";
                    string strType = "text,text,text,text,text,text,text,text,text,text,num,hidden,hidden,hidden";
                    string strValu = "" + model.p_title + "'" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0) + "'" + model.p_one + "'" + Convert.ToDouble(model.p_one_lu) + "'" + model.p_two + "'" + Convert.ToDouble(model.p_two_lu) + "'" + Convert.ToDouble(model.p_pk) + "'" + Convert.ToDouble(model.p_big_lu) + "'" + Convert.ToDouble(model.p_small_lu) + "'" + Convert.ToDouble(model.p_dx_pk) + "'0'" + ptype + "'" + gid + "'" + Utils.getPage(0) + "";
                    string strEmpt = "flase,false,false,false,false,false,false,true,true,true,false,true,false,false";
                    string strIdea = "/";
                    string strOthe = string.Empty;
                    //if (Utils.GetDomain().Contains("168yy.cc") || Utils.GetDomain().Contains("tl88.cc"))
                    //    strOthe = "" + ub.Get("SiteBz") + "开庄|" + ub.Get("SiteBz2") + "开庄,kzguess.aspx,post,0,red|blue";
                    //else
                    strOthe = "确定开庄,kzguess.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("系统不会扣除你的接受投注总额，买家下注时则通过计算赔付进行扣除相应的押金<br />赔率填写-1可隐藏某选项下注，即不可下注该选项！");
                    builder.Append(Out.Tab("</div>", ""));
                }
  
            }
            if (ptype > 0)
            {
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.waplink(Utils.getUrl("kzguess.aspx?gid=" + gid + "&amp;backurl=" + Utils.getPage(0) + ""), "&lt;&lt;切换开庄方式") + " ");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
            builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
            builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
}
