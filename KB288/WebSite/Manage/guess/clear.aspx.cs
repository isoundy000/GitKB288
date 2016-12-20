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

public partial class Manage_guess_clear : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "清空记录";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        if (act == "actopok")
        {
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                int ManageId = new BCW.User.Manage().IsManageLogin();
                if (ManageId != 1 && ManageId != 11 )
                {
                    Utils.Error("你的权限不足", "");
                }
                DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
                DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
                int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "返负千分比填写错误"));
                int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少输多少币才返填写错误"));

                DataSet ds = new TPR.BLL.guess.BaPay().GetBaPayList("payusid,sum(p_getMoney-payCent) as payCents", "paytimes>='" + sTime + "'and paytimes<'" + oTime + "' and Types=0 group by payusid");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["payCents"]);
                    if (Cents < 0 && Cents < (-iPrice))
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["payusid"]);
                        long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        new BCW.BLL.User().UpdateiGold(usid, cent, "竞猜返负");
                        //发内线
                        string strLog = "根据你上期竞猜排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/guess/default.aspx]进入球彩竞猜[/url]";
                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                    }
                }

                Utils.Success("返负操作", "返负操作成功", Utils.getUrl("clear.aspx"), "1");

            }
        }
        else if (act == "actopok2")
        {
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                int ManageId = new BCW.User.Manage().IsManageLogin();
                if (ManageId != 1 && ManageId != 11 )
                {
                    Utils.Error("你的权限不足", "");
                }
                DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
                DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
                int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "返赢千分比填写错误"));
                int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少赢多少币才返填写错误"));

                DataSet ds = new TPR.BLL.guess.BaPay().GetBaPayList("payusid,sum(p_getMoney-payCent) as payCents", "paytimes>='" + sTime + "'and paytimes<'" + oTime + "' and Types=0 group by payusid");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["payCents"]);
                    if (Cents > 0 && Cents >= iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["payusid"]);
                        long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                        new BCW.BLL.User().UpdateiGold(usid, cent, "竞猜返赢");
                        //发内线
                        string strLog = "根据你上期竞猜排行榜上的赢利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/guess/default.aspx]进入球彩竞猜[/url]";

                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                    }
                }

                Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("clear.aspx"), "1");

            }
        }
        else if (act == "actspaceguess")//清空已截止的、无投注记录的赛事
        {
            int k = 0;
            DataSet ds = BCW.Data.SqlHelper.Query("select ID from tb_balist where p_TPRTime<'" + DateTime.Now + "' and p_active=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());

                    int pCount = new TPR.BLL.guess.BaPay().GetBaPayCount("bcid=" + ID + "");
                    if (pCount == 0)
                    {
                        new TPR.BLL.guess.BaList().Delete(ID);
                        k++;
                    }
                }
            }
            Utils.Success("清空记录", "清空无投注赛事成功", Utils.getUrl("default.aspx"), "1");
        }
        else
        {
            string ac = Utils.GetRequest("ac", "post", 1, "", "");
            string Info = Utils.GetRequest("Info", "post", 1, "", "");
            if (ac == "清空记录" || ac == Utils.ToTChinese("清空记录"))
            {
                int Ptype, Ltype, Ttype, iTset;
                string sTime = "";
                string oTime = "";
                Ptype = Utils.ParseInt(Utils.GetRequest("Ptype", "post", 2, @"^[0-2]$", "球类选择无效"));
                Ltype = Utils.ParseInt(Utils.GetRequest("Ltype", "post", 2, @"^[1-9]$", "类型选择无效"));
                Ttype = Utils.ParseInt(Utils.GetRequest("Ttype", "post", 2, @"^[1-5]+$", "限制选择无效"));
                iTset = Utils.ParseInt(Utils.GetRequest("iTset", "post", 2, @"^[1-3]+$", "局域选择无效"));
                if (Ttype == 4)
                {
                    sTime = Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效");
                    oTime = Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效");

                }

                if (Info == "ok")
                {
                    string cText = string.Empty;
                    if (Ltype == 1)
                    {
                        cText = "清空竞猜排行榜";
                    }
                    else if (Ltype == 2)
                    {
                        cText = "清空竞猜投注记录";
                    }
                    else if (Ltype == 3)
                    {
                        cText = "清空竞猜赛事记录";
                    }
                    else if (Ltype == 4)
                    {
                        cText = "清空竞猜串串记录";
                    }
                    else if (Ltype == 5)
                    {
                        cText = "清空竞猜串串排行榜";
                    }
                    //游戏日志记录
                    int ManageId = new BCW.User.Manage().IsManageLogin();
                    int gid = new TPR.BLL.guess.BaList().GetMaxId();
                    string[] p_pageArr = { "ac", "Ltype", "Ptype", "Ttype", "sTime", "oTime", "iTset", "Info" };
                    BCW.User.GameLog.GameLogPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号" + cText + "", gid);

                    //组合条件查询
                    string strWhere = "";

                    if (Ltype == 1)//排行榜记录
                    {
                        //------------------------返负彩
                        if (Convert.ToInt32(ub.GetSub("SiteLostPrice", xmlPath)) > 0)
                        {
                            DataSet ds = new TPR.BLL.guess.BaOrder().GetBaOrderList("orderusid,orderusname,orderjbnum", "Orderjbnum < 0 and Orderjbnum <= " + (-Convert.ToInt32(ub.GetSub("SiteLostPrice", xmlPath))) + "");
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                int cent = Convert.ToInt32((-Convert.ToInt32(ds.Tables[0].Rows[i]["orderjbnum"])) * (Convert.ToDouble(ub.GetSub("SiteLostPL", xmlPath)) * 0.01));

                                //操作币
                                new BCW.BLL.User().UpdateiGold(Convert.ToInt32(ds.Tables[0].Rows[i]["orderusid"]), cent, "");
                                //发内线
                                string strLog = "根据你上期竞猜排行榜上的盈利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/guess/default.aspx]进入球彩竞猜[/url]";

                                new BCW.BLL.Guest().Add(0, Convert.ToInt32(ds.Tables[0].Rows[i]["orderusid"]), ds.Tables[0].Rows[i]["orderusname"].ToString(), strLog);
                            }
                        }
                        //------------------------返负彩
                        new TPR.BLL.guess.BaOrder().DeleteStr();
                    }
                    else if (Ltype == 2)//投注记录
                    {
                        if (Ptype != 0)
                        {
                            strWhere += "pType=" + Ptype + " ";
                        }
                        else
                        {
                            strWhere += "pType>0 ";
                        }

                        if (Ttype == 1)
                        {
                            strWhere += "and paytimes='" + DateTime.Now.ToLongDateString() + "'";
                        }
                        else if (Ttype == 2)
                        {
                            strWhere += "and paytimes>='" + DateTime.Now.AddDays(-7) + "'";
                        }
                        else if (Ttype == 3)
                        {
                            strWhere += "and paytimes>='" + DateTime.Now.AddDays(-30) + "'";
                        }
                        else if (Ttype == 4)
                        {
                            if (iTset == 1)
                            {
                                strWhere += "and paytimes>='" + sTime + "' and paytimes<='" + oTime + "'";
                            }
                            else if (iTset == 2)
                            {
                                strWhere += "and paytimes>'" + sTime + "' and paytimes<'" + oTime + "'";
                            }
                            else if (iTset == 3)
                            {
                                strWhere += "and paytimes NOT BETWEEN '" + sTime + "' and '" + oTime + "'";
                            }
                        }
                        new TPR.BLL.guess.BaPay().DeleteStr(strWhere);
                    }
                    else if (Ltype == 3)//赛事记录
                    {
                        if (Ptype != 0)
                        {
                            strWhere += "p_type=" + Ptype + " ";
                        }
                        else
                        {
                            strWhere += "p_type>0 ";
                        }

                        if (Ttype == 1)
                        {
                            strWhere += "and p_addtime='" + DateTime.Now.ToLongDateString() + "'";
                        }
                        else if (Ttype == 2)
                        {
                            strWhere += "and p_addtime>='" + DateTime.Now.AddDays(-7) + "'";
                        }
                        else if (Ttype == 3)
                        {
                            strWhere += "and p_addtime>='" + DateTime.Now.AddDays(-30) + "'";
                        }
                        else if (Ttype == 4)
                        {
                            if (iTset == 1)
                            {
                                strWhere += "and p_addtime>='" + sTime + "' and p_addtime<='" + oTime + "'";
                            }
                            else if (iTset == 2)
                            {
                                strWhere += "and p_addtime>'" + sTime + "' and p_addtime<'" + oTime + "'";
                            }
                            else if (iTset == 3)
                            {
                                strWhere += "and p_addtime NOT BETWEEN '" + sTime + "' and '" + oTime + "'";
                            }
                        }

                        new TPR.BLL.guess.BaList().DeleteStr(strWhere);
                    }
                    else if (Ltype == 4)//串串记录
                    {

                        if (Ttype == 1)
                        {
                            strWhere += "AddTime='" + DateTime.Now.ToLongDateString() + "'";
                        }
                        else if (Ttype == 2)
                        {
                            strWhere += "AddTime>='" + DateTime.Now.AddDays(-7) + "'";
                        }
                        else if (Ttype == 3)
                        {
                            strWhere += "AddTime>='" + DateTime.Now.AddDays(-30) + "'";
                        }
                        else if (Ttype == 4)
                        {
                            if (iTset == 1)
                            {
                                strWhere += "AddTime>='" + sTime + "' and AddTime<='" + oTime + "'";
                            }
                            else if (iTset == 2)
                            {
                                strWhere += "AddTime>'" + sTime + "' and AddTime<'" + oTime + "'";
                            }
                            else if (iTset == 3)
                            {
                                strWhere += "AddTime NOT BETWEEN '" + sTime + "' and '" + oTime + "'";
                            }
                        }
                        new TPR.BLL.guess.Super().DeleteStr(strWhere);
                    }

                    else if (Ltype == 5)//串串排行榜
                    {
                        new TPR.BLL.guess.SuperOrder().DeleteStr();
                    }
                    Utils.Success("清空记录", "清空记录成功", Utils.getUrl("clear.aspx"), "1");
                }
                else
                {
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    if (Ltype == 1)
                    {
                        builder.Append("请确认清空排行榜,清空的同时将返负彩,负彩相关设置请在系统参数中配置");
                    }
                    else if (Ltype == 2)
                    {
                        builder.Append("请确认清空投注记录");
                    }
                    else if (Ltype == 3)
                    {
                        builder.Append("请确认清空赛事记录");
                    }
                    else if (Ltype == 4)
                    {
                        builder.Append("请确认清空串串记录");
                    }
                    else if (Ltype == 5)
                    {
                        builder.Append("请确认清空串串排行榜");
                    }

                    builder.Append(Out.Tab("</div>", "<br />"));
                    string strName = "Ltype,Ptype,Ttype,sTime,oTime,iTset,Info";
                    string strValu = "" + Ltype + "'" + Ptype + "'" + Ttype + "'" + sTime + "'" + oTime + "'" + iTset + "'ok";
                    string strOthe = "清空记录,clear.aspx,post,1,red";

                    builder.Append(Out.wapform(strName, strValu, strOthe));
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append(Out.waplink(Utils.getUrl("clear.aspx"), "先留着吧.."));
                    builder.Append(Out.Tab("</div>", "<br />"));
                }

            }
            else if (act == "actop")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("返负点操作");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "开始时间:,结束时间:,返负千分比:,至少输多少才返:,";
                string strName = "sTime,oTime,iTar,iPrice,act";
                string strType = "date,date,num,num,hidden";
                string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''actopok";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "马上返负,clear.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else if (act == "actop2")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("返赢点操作");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
                string strName = "sTime,oTime,iTar,iPrice,act";
                string strType = "date,date,num,num,hidden";
                string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''actopok2";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "马上返赢,clear.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("清空历史记录");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "*选择清空内容:/,球类:,限制:,开始时间:,结束时间:,时间局域:";
                string strName = "Ltype,Ptype,Ttype,sTime,oTime,iTset";
                string strType = "select,select,select,date,date,select";
                string strValu = "1'0'5'" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'1";
                string strEmpt = "1|排行榜记录|2|投注记录|3|赛事记录|4|串串记录|5|串串排行榜,0|全部|1|足球|2|篮球,1|今天所有|2|一个星期|3|一个月内|4|指定时间|5|所有时间,true,true,1|=&lt;&gt;=两者之间|2|&lt;&gt;介负于两者|3|=&gt;&lt;=两者之外";
                string strIdea = "/温馨提示:选择限定时间需填写时间类型;/排行榜记录不用选择时间限制;/闲聊、消费记录不用选择球类/";
                string strOthe = "清空记录,clear.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("clear.aspx?act=actop") + "\">返负点操作</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("clear.aspx?act=actop2") + "\">返赢点操作</a><br />");
                }
                builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
    }
}
