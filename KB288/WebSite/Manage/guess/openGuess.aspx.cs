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
using TPR.Common.Guess;
public partial class Manage_guess_openGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess.xml";
    //获取走地（比分时段）
    private string footonce(int p_id, DateTime dt)
    {
        string DtString = string.Empty;
        IList<TPR.Model.guess.BaList> listonce = new TPR.Collec.Once().GetOnce2(p_id);
        if (listonce.Count > 0)
        {
            foreach (TPR.Model.guess.BaList n in listonce)
            {
                if (Convert.ToInt32(n.p_once) > 45)
                    DtString += "|" + dt.AddMinutes(Convert.ToInt32(n.p_once) + 16);
                else
                    DtString += "|" + dt.AddMinutes(Convert.ToInt32(n.p_once));
            }
            DtString = Utils.Mid(DtString, 1, DtString.Length);
        }
        return DtString;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        TPR.BLL.guess.BaList bll = new TPR.BLL.guess.BaList();

        if (bll.GetModel(gid) == null)
        {
            Utils.Error("不存在的记录", "");
        }
        TPR.Model.guess.BaList model = bll.GetModel(gid);
        Master.Title = "开奖赛事" + model.p_one + "VS" + model.p_two;

        if (Utils.ToSChinese(ac) == "确定开奖" || Utils.ToSChinese(ac) == "确定重开奖")
        {
            int resultone = Utils.ParseInt(Utils.GetRequest("resultone", "post", 2, @"^[0-9]*$", "请正确输入比分"));
            int resulttwo = Utils.ParseInt(Utils.GetRequest("resulttwo", "post", 2, @"^[0-9]*$", "请正确输入比分"));
            int iType = Utils.ParseInt(Utils.GetRequest("iType", "post", 2, @"^[1-3]*$", "请正确选择开奖模式"));

            DateTime oTime = DateTime.Now;
            string pSms = "";
            if (iType == 2)
            {
                oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "请正确填写截止时间"));
                pSms = Out.UBB(Utils.GetRequest("pSms", "post", 2, @"^[\s\S]{2,20}$", "请输入2-20字的平盘原因"));
            }
            else if (iType == 3)
            {
                pSms = Out.UBB(Utils.GetRequest("pSms", "post", 2, @"^[\s\S]{2,20}$", "请输入2-20字的平盘原因"));
            }

            string Info = Utils.GetRequest("Info", "post", 1, "", "");
            string onceTime = Utils.GetRequest("onceTime", "post", 1, "", "");
            if (Info == "ok")
            {
                //游戏日志记录
                int ManageId = new BCW.User.Manage().IsManageLogin();
                string[] p_pageArr = { "ac", "gid", "resultone", "resulttwo", "p_one", "iType", "oTime", "pSms", "Info", "onceTime" };
                if (model.p_result_one != null && model.p_result_two != null)
                    BCW.User.GameLog.GameLogPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号重新开奖" + model.p_one + "VS" + model.p_two + "(" + gid + ")，比分" + resultone + ":" + resulttwo + "", gid);
                else
                    BCW.User.GameLog.GameLogPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号开奖" + model.p_one + "VS" + model.p_two + "(" + gid + ")，比分" + resultone + ":" + resulttwo + "", gid);

                //重开奖处理：
                if (model.p_result_one != null && model.p_result_two != null)
                {
                    DataSet ds = new TPR.BLL.guess.BaPay().GetBaPayList("pType,payview,payusid,payusname,payCent,p_getMoney,types", "bcid=" + gid + " and p_case=1 and itypes=0 ");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int pType = int.Parse(ds.Tables[0].Rows[i]["pType"].ToString());
                            string payview = ds.Tables[0].Rows[i]["payview"].ToString();
                            int payusid = int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString());
                            string payusname = ds.Tables[0].Rows[i]["payusname"].ToString();
                            long payCent = Convert.ToInt64(Convert.ToDecimal(ds.Tables[0].Rows[i]["payCent"].ToString()));
                            long p_getMoney = Convert.ToInt64(Convert.ToDecimal(ds.Tables[0].Rows[i]["p_getMoney"].ToString()));
                            int bzType = int.Parse(ds.Tables[0].Rows[i]["types"].ToString());
                            long gold = 0;
                            long cMoney = 0;//差多少
                            long sMoney = 0;//实扣
                            if (bzType == 0)
                                gold = new BCW.BLL.User().GetGold(payusid);
                            else
                                gold = new BCW.BLL.User().GetMoney(payusid);

                            if (p_getMoney > gold)
                            {
                                cMoney = p_getMoney - gold;
                                sMoney = gold;
                            }
                            else
                            {
                                sMoney = p_getMoney;
                            }

                            //重开奖的在本场没兑奖时就没显示在欠币日志，

                            //操作币并内线通知
                            if (bzType == 0)
                            {
                                new BCW.BLL.User().UpdateiGold(payusid, payusname, -sMoney, "球彩重开奖，扣除已兑奖" + ub.Get("SiteBz") + "");
                                //发送内线
                                string strGuess = "球彩重开奖，你欠下系统的" + p_getMoney + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额，实扣" + sMoney + "" + ub.Get("SiteBz") + ".[br]如果您的" + ub.Get("SiteBz") + "不足，系统将您帐户冻结，直到成功扣除为止。[br]" +  payview + "(原开奖" + model.p_result_one + ":" + model.p_result_two + "|新开奖" + resultone + ":" + resulttwo + ")";
                                new BCW.BLL.Guest().Add(1, payusid, payusname, strGuess);
                            }
                            else
                            {
                                new BCW.BLL.User().UpdateiMoney(payusid, payusname, -sMoney, "球彩重开奖，扣除已兑奖" + ub.Get("SiteBz2") + "");
                                //发送内线
                                string strGuess = "球彩重开奖，你欠下系统的" + p_getMoney + "" + ub.Get("SiteBz2") + ".[br]根据您的帐户数额，实扣" + sMoney + "" + ub.Get("SiteBz2") + ".[br]如果您的" + ub.Get("SiteBz2") + "不足，系统将您帐户冻结，直到成功扣除为止。[br]" +  payview + "(原开奖" + model.p_result_one + ":" + model.p_result_two + "|新开奖" + resultone + ":" + resulttwo + ")";
                                new BCW.BLL.Guest().Add(1, payusid, payusname, strGuess);
                            }
                            //如果币不够扣则记录日志并冻结IsFreeze
                            if (cMoney > 0)
                            {
                                BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                                owe.Types = 0;
                                owe.UsID = payusid;
                                owe.UsName = payusname;
                                owe.Content = "" + payview + "(原开奖" + model.p_result_one + ":" + model.p_result_two + "|新开奖" + resultone + ":" + resulttwo + ")";
                                owe.OweCent = cMoney;
                                owe.BzType = bzType;
                                owe.EnId = gid;
                                owe.AddTime = DateTime.Now;
                                new BCW.BLL.Gameowe().Add(owe);

                                new BCW.BLL.User().UpdateIsFreeze(payusid, 1);
                            }

                            //取消得到的排行
                            TPR.Model.guess.BaOrder objBaOrder = new TPR.Model.guess.BaOrder();
                            objBaOrder.Orderusid = payusid;
                            objBaOrder.Orderusname = payusname;
                            objBaOrder.Orderfanum = 0;
                            objBaOrder.Orderjbnum = -(p_getMoney - payCent);
                            objBaOrder.Orderbanum = -1;
                            objBaOrder.Orderstats = pType;
                            new TPR.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                        }
                    }
                }
     
                int OnceMin = Convert.ToInt32(ub.GetSub("SiteOnce", xmlPath));//走地时间限制

                //是否走地赛事
                int ison = Convert.ToInt32(model.p_ison);

                //取得比分时间段

                string stronce = string.Empty;
                if (ison == 1)
                {
                    stronce = onceTime;

                    if (string.IsNullOrEmpty(stronce))
                    {
                        stronce = footonce(Convert.ToInt32(model.p_id), Convert.ToDateTime(model.p_TPRtime));
                        model.ID = gid;
                        model.p_once = stronce;
                        new TPR.BLL.guess.BaList().UpdateOnce(model);
                    }
                    stronce = stronce.Replace("#", "|");
                }


                //更新比分
                model.p_result_one = resultone;
                model.p_result_two = resulttwo;

                if (iType == 3)
                    model.p_active = 2;//平盘标识
                else
                    model.p_active = 1;

                bll.UpdateResult(model);

                int recordCount = 0;
                int p_intWin = 0;
                decimal p_intDuVal = 0;
                //组合查询条件
                string strppWhere = "";
                string strWhere = "";
                strppWhere = "bcid=" + gid + "";
                strWhere = "bcid=" + gid + "";
                if (iType == 2)
                {
                    strWhere += "and paytimes<='" + oTime + "'";
                    strppWhere += "and paytimes>'" + oTime + "'";
                }


                if (iType < 3)
                {

                    TPR.Model.guess.BaOrder objBaOrder = new TPR.Model.guess.BaOrder();

                    // 开始查询并更新之
                    IList<TPR.Model.guess.BaPay> listBaPay = new TPR.BLL.guess.BaPay().GetBaPays(1, 8000, strWhere, out recordCount);
                    if (listBaPay.Count > 0)
                    {
                        foreach (TPR.Model.guess.BaPay n in listBaPay)
                        {
                            int Iszd = 0;
                            n.p_result_one = resultone;
                            n.p_result_two = resulttwo;
                            n.p_active = 1;
                            //币种
                            string bzTypes = string.Empty;
                            if (n.Types == 0)
                                bzTypes = ub.Get("SiteBz");
                            else
                                bzTypes = ub.Get("SiteBz2");

                            if (model.p_type == 1)
                            {
                                //是否可以平盘了
                                if (n.PayType <= 4)
                                {
                                    if (!string.IsNullOrEmpty(stronce))
                                    {
                                        string[] Sonce = stronce.Split("|".ToCharArray());

                                        for (int i = 0; i < Sonce.Length; i++)
                                        {
                                            if (Convert.ToDateTime(Sonce[i]).AddSeconds(OnceMin) > Convert.ToDateTime(n.paytimes) && Convert.ToDateTime(Sonce[i]).AddSeconds(-OnceMin) < Convert.ToDateTime(n.paytimes))
                                            {
                                                Iszd = 2;//平盘标识
                                            }
                                        }
                                    }
                                }
                                if (Iszd == 0)
                                {
                                    if (n.PayType == 1 || n.PayType == 2)
                                    {
                                        string p_strVal = string.Empty;
                                        if (ison == 1)//如果是走地模式
                                        {
                                            n.p_result_one = resultone - Convert.ToInt32(n.p_result_temp1);
                                            n.p_result_two = resulttwo - Convert.ToInt32(n.p_result_temp2);
                                            p_strVal = ZqClass.getZqsxCase(n);
                                            //重新取值
                                            n.p_result_one = resultone;
                                            n.p_result_two = resulttwo;
                                        }
                                        else
                                        {
                                            p_strVal = ZqClass.getZqsxCase(n);
                                        }
                                        new TPR.BLL.guess.BaPay().UpdateCase(n, p_strVal, out p_intDuVal, out p_intWin);
                                        Iszd = 1;//走地模式
                                    }
                                    else if (n.PayType == 3 || n.PayType == 4)
                                    {
                                        new TPR.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqdxCase(n), out p_intDuVal, out p_intWin);
                                    }
                                    else
                                    {
                                        new TPR.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqbzCase(n), out p_intDuVal, out p_intWin);
                                    }
                                }
                            }
                            else
                            {
                                if (n.PayType == 1 || n.PayType == 2)
                                    new TPR.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqsxCase(n), out p_intDuVal, out p_intWin);
                                else
                                    new TPR.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqdxCase(n), out p_intDuVal, out p_intWin);
                            }
                            if (Iszd != 2)
                            {
                                if (Convert.ToInt32(n.itypes) == 0)
                                {
                                    if (p_intWin == 1)
                                    {
                                        //更新排行榜:赢

                                        objBaOrder.Orderusid = n.payusid;
                                        objBaOrder.Orderusname = n.payusname;

                                        if (p_intDuVal == n.payCent)
                                        {
                                            objBaOrder.Orderbanum = 0;
                                            objBaOrder.Orderjbnum = 0;
                                        }
                                        else
                                        {
                                            objBaOrder.Orderbanum = 1;
                                            objBaOrder.Orderjbnum = p_intDuVal - n.payCent;
                                        }
                                        objBaOrder.Orderfanum = 0;

                                        objBaOrder.Orderstats = n.pType;
                                        new TPR.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                                        //发送内线
                                        string strLog = string.Empty;
                                        if (Iszd == 1)  //走地的内线提醒
                                            strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "(下注" + n.p_result_temp1 + ":" + n.p_result_temp2 + ")，系统返" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess/caseGuess.aspx]马上兑奖[/url]";
                                        else
                                            strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "，系统返" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess/caseGuess.aspx]马上兑奖[/url]";

                                        new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                                    }
                                    else
                                    {
                                        //更新排行榜:输

                                        objBaOrder.Orderusid = n.payusid;
                                        objBaOrder.Orderusname = n.payusname;
                                        objBaOrder.Orderbanum = 0;

                                        objBaOrder.Orderfanum = 1;

                                        objBaOrder.Orderjbnum = -n.payCent;
                                        objBaOrder.Orderbanum = 0;

                                        objBaOrder.Orderstats = n.pType;
                                        new TPR.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                                    }
                                }
                            }
                            else
                            {
                                //平盘
                                n.p_result_one = resultone;
                                n.p_result_two = resulttwo;
                                n.p_active = 2;
                                n.p_getMoney = n.payCent;
                                new TPR.BLL.guess.BaPay().UpdatePPCase(n);
                                //发送内线
                                if (Convert.ToInt32(n.itypes) == 0)
                                {
                                    //发送内线
                                    string strLog = "" + n.payview + "[br]结果平盘，原因：走地赛事，系统将比分变动前后" + OnceMin + "秒钟的下注作平盘处理，返还" + Convert.ToDouble(n.payCent) + "" + bzTypes + "[url=/bbs/guess/caseGuess.aspx]马上兑奖[/url][br]本场赛事变动时间如下:[br]" + stronce.Replace("|", "[br]") + "";
                                    new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                                }
                            }
                        }
                    }
                }
                if (iType == 2 || iType == 3)
                {
                    // 平盘返还
                    IList<TPR.Model.guess.BaPay> listBaPay = new TPR.BLL.guess.BaPay().GetBaPays(1, 8000, strppWhere, out recordCount);
                    if (listBaPay.Count > 0)
                    {
                        foreach (TPR.Model.guess.BaPay n in listBaPay)
                        {
                            n.p_result_one = resultone;
                            n.p_result_two = resulttwo;
                            n.p_active = 2;
                            n.p_getMoney = n.payCent;
                            //币种
                            string bzTypes = string.Empty;
                            if (n.Types == 0)
                                bzTypes = ub.Get("SiteBz");
                            else
                                bzTypes = ub.Get("SiteBz2");

                            new TPR.BLL.guess.BaPay().UpdatePPCase(n);

                            //发送内线
                            if (Convert.ToInt32(n.itypes) == 0)
                            {
                                //发送内线
                                string strLog = "" + n.payview + "[br]结果平盘，原因：" + pSms + "，系统返还" + Convert.ToDouble(n.payCent) + "" + bzTypes + "[url=/bbs/guess/caseGuess.aspx]马上兑奖[/url]";
                                new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                            }
                        }
                    }
                }

                if (iType == 3)
                {
                    BCW.Data.SqlHelper.ExecuteSql("Update tb_Balist set p_id=0 where id=" + gid + "");
                    Utils.Success("开奖", "操作平盘成功..", Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "1");
                }
                else
                {
                    Utils.Success("开奖", "开奖" + resultone + ":" + resulttwo + "成功..", Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "1");
                }

            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                if (iType == 1)
                {
                    builder.Append("请确认比分" + resultone + ":" + resulttwo + "");
                }
                else if (iType == 2)
                {
                    builder.Append("请确认比分" + resultone + ":" + resulttwo + "," + DT.FormatDate(oTime, 0) + "");
                }
                else
                {
                    builder.Append("请确认平盘.");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                string strName = "resultone,resulttwo,iType,oTime,pSms,onceTime,gid,Info";
                string strValu = "" + resultone + "'" + resulttwo + "'" + iType + "'" + DT.FormatDate(oTime, 0) + "'" + pSms + "'" + onceTime + "'" + gid + "'ok";
                string strOthe = "确定开奖,openGuess.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "再看看吧.."));
                builder.Append(Out.Tab("</div>", "<br />"));

            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("开奖赛事" + model.p_one + "VS" + model.p_two);
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("开赛:" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0));
            builder.Append(Out.Tab("</div>", ""));

            string strText = string.Empty;
            string strName = string.Empty;
            string strType = string.Empty;
            string strValu = string.Empty;
            string strEmpt = string.Empty;
            if (model.p_ison == 0)
            {
                strText = "*填写比分/,比/,开奖模式,截止时间,操作原因,";
                strName = "resultone,resulttwo,iType,oTime,pSms,gid";
                strType = "num,num,select,date,text,hidden";
                strValu = "0'0'1'" + DT.FormatDate(DateTime.Now.AddHours(-10), 0) + "''" + gid + "";
                strEmpt = "false,false,1|正常模式|2|截时模式|3|平盘模式,true,true,";
            }
            else
            {
                strText = "*填写比分/,比/,开奖模式,截止时间,操作原因,走地比分时间(格式2010-10-7 1:00:00#2010-10-7 1:20:00)/,";
                strName = "resultone,resulttwo,iType,oTime,pSms,onceTime,gid";
                strType = "num,num,select,date,text,textarea,hidden";
                strValu = "0'0'1'" + DT.FormatDate(DateTime.Now.AddHours(-10), 0) + "'''" + gid + "";
                strEmpt = "false,false,1|正常模式|2|截时模式|3|平盘模式,true,true,true,";
            }
            string strIdea = "/提示：非正常模式开奖时请输入操作原因；/当选择截时模式时，即该时间前的作正常开奖，时间后的作平盘返还；/当你选择平盘模式时，比分写成0:0即可/";
            string strOthe = "";
            if (model.p_result_one != null && model.p_result_two != null)
                strOthe = "确定重开奖,openGuess.aspx,post,1,red";
            else
                strOthe = "确定开奖,openGuess.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            if (model.p_result_one != null && model.p_result_two != null)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("重要：重开奖系统自动扣回已经兑奖的币并进行新一轮的开奖,如果币不够扣，即自动禁该会员的金融系统并记录<a href=\"" + Utils.getUrl("../default.aspx") + "\">欠币日志</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
            {
                string stronce = new TPR.BLL.guess.BaList().Getp_temptimes(gid);
                if (stronce != "")
                {
                    builder.Append("走地比分参考:<br />" + stronce.Replace("|", "<br />") + "");
                }
            }
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }
}