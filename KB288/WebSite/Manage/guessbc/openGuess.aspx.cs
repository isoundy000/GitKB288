﻿using System;
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
using TPR3.Common.Guess;
public partial class Manage_guess3_openGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guessbc.xml";
    //获取走地（比分时段）
    private string footonce(int p_id, DateTime dt)
    {
        string DtString = string.Empty;

        return DtString;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        TPR3.BLL.guess.BaList bll = new TPR3.BLL.guess.BaList();

        if (bll.GetModel(gid) == null)
        {
            Utils.Error("不存在的记录", "");
        }
        TPR3.Model.guess.BaList model = bll.GetModel(gid);
        Master.Title = "开奖赛事" + model.p_one + "VS" + model.p_two;

        if (ac == "确定开奖" || ac == Utils.ToTChinese("确定开奖"))
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
                int ManageId = new BCW.User.Manage().IsManageLogin();
                string[] p_pageArr = { "ac", "gid", "resultone", "resulttwo", "p_one", "iType", "oTime", "pSms", "Info", "onceTime" };
                if (model.p_result_one != null && model.p_result_two != null)
                    BCW.User.GameLog.GameLogPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号重新开奖" + model.p_one + "VS" + model.p_two + "(" + gid + ")，比分" + resultone + ":" + resulttwo + "", gid);
                else
                    BCW.User.GameLog.GameLogPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号开奖" + model.p_one + "VS" + model.p_two + "(" + gid + ")，比分" + resultone + ":" + resulttwo + "", gid);


                int OnceMin = 0;//走地时间限制
             
                //是否走地赛事
                int ison = Convert.ToInt32(model.p_ison);

                //取得比分时间段
               
                string stronce = string.Empty;

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
                strppWhere = "bcid=" + gid + " and state=0";
                strWhere = "bcid=" + gid + " and state=0";
                if (iType == 2)
                {
                    strWhere += "and paytimes<='" + oTime + "'";
                    strppWhere += "and paytimes>'" + oTime + "'";
                }


                if (iType < 3)
                {

                    TPR3.Model.guess.BaOrder objBaOrder = new TPR3.Model.guess.BaOrder();

                    // 开始查询并更新之
                    IList<TPR3.Model.guess.BaPay> listBaPay = new TPR3.BLL.guess.BaPay().GetBaPays(1, 8000, strWhere, out recordCount);
                    if (listBaPay.Count > 0)
                    {
                        foreach (TPR3.Model.guess.BaPay n in listBaPay)
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
                                        new TPR3.BLL.guess.BaPay().UpdateCase(n, p_strVal, out p_intDuVal, out p_intWin);
                                        Iszd = 1;//走地模式
                                    }
                                    else if (n.PayType == 3 || n.PayType == 4)
                                    {
                                        new TPR3.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqdxCase(n), out p_intDuVal, out p_intWin);
                                    }
                                    else if (n.PayType == 5 || n.PayType == 6 || n.PayType == 7)
                                    {
                                        new TPR3.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqbzCase(n), out p_intDuVal, out p_intWin);
                                    }
                                    else
                                    {
                                        new TPR3.BLL.guess.BaPay().UpdateCase(n, ZqClass.getZqdsCase(n), out p_intDuVal, out p_intWin);
                                    }
                                }
                            }
                            else
                            {
                                if (n.PayType == 1 || n.PayType == 2)
                                    new TPR3.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqsxCase(n), out p_intDuVal, out p_intWin);
                                else if (n.PayType == 3 || n.PayType == 4)
                                    new TPR3.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqdxCase(n), out p_intDuVal, out p_intWin);
                                else
                                    new TPR3.BLL.guess.BaPay().UpdateCase(n, LqClass.getLqdsCase(n), out p_intDuVal, out p_intWin);
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
                                        new TPR3.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
                                        //发送内线
                                        string strLog = string.Empty;
                                        if (Iszd == 1)  //走地的内线提醒
                                            strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "(下注" + n.p_result_temp1 + ":" + n.p_result_temp2 + ")，赢了" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";
                                        else
                                            strLog = "" + n.payview + "[br]结果:" + resultone + ":" + resulttwo + "，赢了" + Convert.ToDouble(p_intDuVal) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";

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
                                        new TPR3.BLL.guess.BaOrder().UpdateOrder(objBaOrder);
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
                                new TPR3.BLL.guess.BaPay().UpdatePPCase(n);
                                //发送内线
                                if (Convert.ToInt32(n.itypes) == 0)
                                {          
                                    //发送内线
                                    string strLog = "" + n.payview + "[br]结果平盘，原因：走地赛事，系统将比分变动前后" + OnceMin + "秒钟的下注作平盘处理，返还" + Convert.ToDouble(n.payCent) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url][br]本场赛事变动时间如下:[br]" + stronce.Replace("|", "[br]") + "";
                                    new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                                }
                            }
                        }
                    }
                }
                if (iType == 2 || iType == 3)
                {
                    // 平盘返还
                    IList<TPR3.Model.guess.BaPay> listBaPay = new TPR3.BLL.guess.BaPay().GetBaPays(1, 8000, strppWhere, out recordCount);
                    if (listBaPay.Count > 0)
                    {
                        foreach (TPR3.Model.guess.BaPay n in listBaPay)
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

                            new TPR3.BLL.guess.BaPay().UpdatePPCase(n);

                            //发送内线
                            if (Convert.ToInt32(n.itypes) == 0)
                            {          
                                //发送内线
                                string strLog = "" + n.payview + "[br]结果平盘，原因：" + pSms + "，返还" + Convert.ToDouble(n.payCent) + "" + bzTypes + "[url=/bbs/guess2/caseGuess.aspx]马上兑奖[/url]";
                                new BCW.BLL.Guest().Add(1, Convert.ToInt32(n.payusid), n.payusname, strLog);
                            }
                        }
                    }
                }

                if (iType == 3)
                {
                    BCW.Data.SqlHelper.ExecuteSql("Update tb_ZBalist set p_id=0 where id=" + gid + "");
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
            string strOthe = "确定开奖,openGuess.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }
}
