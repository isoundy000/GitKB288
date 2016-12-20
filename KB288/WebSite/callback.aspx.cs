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
using BCW.Yeepay.Utils;
using BCW.Yeepay.cmbn;

/// <summary>
/// 过币处理
/// 黄国军 20160122
/// </summary>
public partial class callback : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/finance.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //执行分析
        string ym = "kb288.com";
        string keys = Request["keys"];

        #region 支付宝掉线提示
        //支付宝掉线提示
        if (Request["act"] == "lostAli")
        {
            if (DESEncrypt.Decrypt(keys, "bg_rdzfb") != ym)
            {

                Response.Write("500");//提交非法

            }
            else
            {
                string userid = Request["userid"];
                string mename = new BCW.BLL.User().GetUsName(int.Parse(userid));

                new BCW.BLL.Guest().Add(int.Parse(userid), mename, "您的支付宝过币程序掉线了,如非本人操作,请检查过币程序。" + DateTime.Now.ToString());
            }
        }
        #endregion

        #region 自动过币
        if (Request["act"] == "zfb")
        {

            string userid = Request["userid"];
            string userpass = Request["userpass"];
            string cent = Request["cent"];
            string toid = Request["toid"];
            string zfbNo = Request["zfbNo"];


            if (DESEncrypt.Decrypt(keys, "bg_rdzfb") != ym)
            {

                Response.Write("500");//提交非法

            }
            else
            {
                try
                {
                    int meid = Convert.ToInt32(userid);
                    if (!new BCW.BLL.User().Exists(meid))
                    {
                        Response.Write("300");//自己ID不存在

                    }
                    else
                    {
                        BCW.Model.User modellogin = new BCW.Model.User();
                        modellogin.UsPwd = Utils.MD5Str(userpass);
                        modellogin.ID = meid;
                        string mename = new BCW.BLL.User().GetUsName(meid);
                        int rowsAffected = new BCW.BLL.User().GetRowByID(modellogin);
                        int ToId = Convert.ToInt32(toid);

                        if (rowsAffected > 0)
                        {
                            long mycent = new BCW.BLL.User().GetGold(meid);
                            long outcent = Convert.ToInt64(cent);
                            if (outcent > mycent)
                            {
                                Response.Write("201");//币币不足
                                new BCW.BLL.Guest().Add(meid, mename, "帐户币值不足以过币给ID" + ToId + "，转币失败(支," + zfbNo + ")");
                            }
                            else
                            {

                                if (!new BCW.BLL.User().Exists(ToId))
                                {
                                    Response.Write("202");//对方ID不存在
                                    //开始过币

                                    new BCW.BLL.Guest().Add(meid, mename, "ID" + ToId + "不存在，转币失败(支," + zfbNo + ")");
                                }
                                else
                                {
                                    //检查订单号是否重复
                                    zfbNo = Out.UBB(zfbNo);
                                    if (new BCW.BLL.Transfer().Exists(meid, zfbNo))
                                    {
                                        Response.Write("203");//订单重复提交
                                    }
                                    else
                                    {
                                        //开始过币
                                        string toname = new BCW.BLL.User().GetUsName(ToId);

                                        new BCW.BLL.User().UpdateiGold(meid, mename, -outcent, "过户给ID" + ToId + "(支付宝自动)");
                                        new BCW.BLL.User().UpdateiGold(ToId, toname, outcent, "来自ID" + meid + "过户(支付宝自动)");


                                        BCW.Model.Transfer model = new BCW.Model.Transfer();
                                        model.Types = 0;
                                        model.FromId = meid;
                                        model.FromName = mename;
                                        model.ToId = ToId;
                                        model.ToName = toname;
                                        model.AcCent = outcent;
                                        model.AddTime = DateTime.Now;
                                        model.zfbNo = zfbNo;
                                        new BCW.BLL.Transfer().Add(model);

                                        //系统内线消息
                                        new BCW.BLL.Guest().Add(ToId, toname, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]过户" + outcent + "" + ub.Get("SiteBz") + "给您(支," + zfbNo + ")");
                                        new BCW.BLL.Guest().Add(meid, mename, "您过户" + outcent + "" + ub.Get("SiteBz") + "给[url=/bbs/uinfo.aspx?uid=" + ToId + "] " + toname + " [/url]成功(支," + zfbNo + ")");

                                        Response.Write("200");//成功

                                    }
                                }
                            }
                        }
                        else
                        {

                            Response.Write("301");//密码错

                        }
                    }
                }
                catch
                {
                    Response.Write("400");//未知错误

                }

            }
        }
        #endregion
    }

    protected void Logic(SZXCallbackResult result)
    {
        DataSet ds = null;
        string logtxt = string.Empty;
        int iCardAmt = 0;
        BCW.Model.Payrmb model = new BCW.Model.Payrmb();
        if (result.R1_Code == "1")
        {


            if (new BCW.BLL.Payrmb().Exists(result.P2_Order.ToString()))
            {
                model.CardAmt = Convert.ToInt32(Convert.ToDouble(result.P3_Amt));
                model.State = 1;
                model.CardOrder = result.P2_Order.ToString();
                new BCW.BLL.Payrmb().Update(model);

                //取之前实体
                ds = new BCW.BLL.Payrmb().GetList("UsID,UsName,CardAmt", "CardOrder='" + result.P2_Order.ToString() + "'");
                if (ds != null)
                {
                    int UsID = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());
                    string UsName = ds.Tables[0].Rows[0]["UsName"].ToString();
                    iCardAmt = int.Parse(ds.Tables[0].Rows[0]["CardAmt"].ToString());
                    //比例
                    int Tar = Utils.ParseInt(ub.GetSub("FinanceSZXTar", xmlPath));
                    if (Tar == 0)
                        Tar = 1;
                    //充入币种
                    if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
                        new BCW.BLL.User().UpdateiGold(UsID, UsName, Convert.ToInt64(Convert.ToInt32(Convert.ToDouble(result.P3_Amt)) * Tar), "充值");
                    else
                        new BCW.BLL.User().UpdateiMoney(UsID, UsName, Convert.ToInt64(Convert.ToInt32(Convert.ToDouble(result.P3_Amt)) * Tar), "充值");

                    logtxt = "" + UsName + "(ID" + UsID + ")选择" + iCardAmt + "面额|充值" + result.P3_Amt + "元成功，订单号:" + result.P2_Order.ToString() + "";
                }

            }
            Response.Write("<BR>非银行卡支付成功");
            Response.Write("<BR>商户订单号:" + result.P2_Order);
            Response.Write("<BR>实际扣款金额(商户收到该返回数据后,一定用自己数据库中存储的金额与该金额进行比较):" + result.P3_Amt);

        }
        else
        {
            //取之前实体
            ds = new BCW.BLL.Payrmb().GetList("UsID,UsName,CardAmt", "CardOrder='" + result.P2_Order.ToString() + "'");
            if (ds != null)
            {
                int UsID = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());
                string UsName = ds.Tables[0].Rows[0]["UsName"].ToString();
                iCardAmt = int.Parse(ds.Tables[0].Rows[0]["CardAmt"].ToString());
                logtxt = "" + UsName + "(ID" + UsID + ")选择" + iCardAmt + "面额|充值失败，订单号:" + result.P2_Order.ToString() + "";
            }

            model.CardAmt = iCardAmt;
            model.State = 2;
            model.CardOrder = result.P2_Order.ToString();
            new BCW.BLL.Payrmb().Update(model);
            Response.Write("交易失败!");

        }
        //远程给我
        if (ub.GetSub("FinanceAmtType", xmlPath) == "0")
        {
            HttpUtils.SendRequest(DESEncrypt.Decrypt("0B06A04A52690EA25959A28EAB05370241162E7891A60AFE4AF2E552D17CC03F", "paykeys") + "?amt=" + DESEncrypt.Encrypt(result.P3_Amt, "p3amt") + "&order=" + DESEncrypt.Encrypt(result.P2_Order, "p2order") + "&ym=" + Utils.GetDomain() + "&state=" + model.State + "&bamt=" + iCardAmt + "", "");
        }
            
        //记录日志
        String sLogFilePath = HttpContext.Current.Server.MapPath("/Files/cache/CardLog" + DateTime.Now.Month + "-" + DateTime.Now.Month + ".txt");
        LogHelper.Write(sLogFilePath, logtxt);
    }
    protected void HmacError(SZXCallbackResult result)
    {

        //BCW.Model.Payrmb model = new BCW.Model.Payrmb();
        //model.CardAmt = 0;
        //model.State = 3;
        //model.CardOrder = result.P2_Order.ToString();
        //new BCW.BLL.Payrmb().Update(model);
        //Response.Write("交易签名无效!");
        //Response.Write("<BR>YeePay-HMAC:" + result.Hmac);
        //Response.Write("<BR>LocalHost:" + result.ErrMsg);
        //Response.Write("交易失败!");

    }
}