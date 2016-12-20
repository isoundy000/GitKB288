using BCW.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/// <summary>
/// 增加刷新页面
/// 黄国军 20160623
/// 
/// 处理环迅支付页面
/// 黄国军 20160521
/// </summary>
public partial class Merchanturl : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/finance.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string ok = (Utils.GetRequest("paymentResult", "post", 3, "", ""));
        //ok = "&lt;Ips&gt;&lt;GateWayRsp&gt;&lt;head&gt;&lt;ReferenceID&gt;&lt;/ReferenceID&gt;&lt;RspCode&gt;000000&lt;/RspCode&gt;&lt;RspMsg&gt;&lt;![CDATA[交易成功！]]&gt;&lt;/RspMsg&gt;&lt;ReqDate&gt;20160510162828&lt;/ReqDate&gt;&lt;RspDate&gt;20160510162923&lt;/RspDate&gt;&lt;Signature&gt;bfae8062b497bc90786369c0ad29900f&lt;/Signature&gt;&lt;/head&gt;&lt;body&gt;&lt;MerBillNo&gt;1004275401&lt;/MerBillNo&gt;&lt;CurrencyType&gt;156&lt;/CurrencyType&gt;&lt;Amount&gt;1&lt;/Amount&gt;&lt;Date&gt;20160510&lt;/Date&gt;&lt;Status&gt;Y&lt;/Status&gt;&lt;Msg&gt;&lt;![CDATA[支付成功！]]&gt;&lt;/Msg&gt;&lt;Attach&gt;&lt;![CDATA[test1004275401]]&gt;&lt;/Attach&gt;&lt;IpsBillNo&gt;BO2016051016282858336&lt;/IpsBillNo&gt;&lt;IpsTradeNo&gt;2016051004052853828&lt;/IpsTradeNo&gt;&lt;RetEncodeType&gt;17&lt;/RetEncodeType&gt;&lt;BankBillNo&gt;717176790&lt;/BankBillNo&gt;&lt;ResultType&gt;0&lt;/ResultType&gt;&lt;IpsBillTime&gt;20160510162921&lt;/IpsBillTime&gt;&lt;/body&gt;&lt;/GateWayRsp&gt;&lt;/Ips&gt;";
        if (ok != "")
        {

            ok = ok.Replace("&lt;![CDATA[", "").Replace("]]&gt;", "");
            ok = Server.HtmlDecode(ok);
            BCW.IPSPay.IPSPayMent.updateorder(ok, true);
        }
        else
        {
            #region 环迅刷新数据,防止用户完成充值没返回到网站
            //网上充值检测
            //环迅刷新数据,防止用户完成充值没返回到网站
            DataSet ds = new BCW.BLL.Payrmb().GetList("*", "(Types = 100) AND (State = 0)");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    #region 处理过程 超过3小时则失败
                    int dCount = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < dCount; i++)
                    {
                        //获取MODEL
                        BCW.Model.Payrmb n = new BCW.BLL.Payrmb().GetPayrmb(int.Parse(ds.Tables[0].Rows[i]["id"].ToString()));
                        //订单超时处理
                        if (n.State == 0)
                        {
                            if (n.AddTime.AddHours(n.BillEXP) < DateTime.Now)
                            {
                                n.State = 2;
                                new BCW.BLL.Payrmb().Update_ips(n);
                            }
                        }
                        //环迅接口 Webserver
                        cn.com.ips.newpay.WSOrderQuery WSorder = new cn.com.ips.newpay.WSOrderQuery();
                        //变量定义
                        string SignatureOrder = "", bodystr = "", Resultstr = "";
                        //数字前面和Body内容组成
                        bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByChkOrder(n, ref SignatureOrder);
                        //完整的发送提交字符
                        string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost_ByOrder(DateTime.Now.ToString("yyyyMMddHHmmss"), SignatureOrder, bodystr);
                        //返回的数据
                        Resultstr = WSorder.getOrderByMerBillNo(pGateWayReqstr);
                        //更新结果
                        BCW.IPSPay.IPSPayMent.updateorder(Resultstr, false);
                    }
                    #endregion
                }
            }

            //商城充值检测
            DataSet ds1 = new BCW.BLL.Shopkeep().GetList("*", "  (NodeId = 28) AND (State = 0) ");
            if (ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    #region 处理过程 超过3小时则失败
                    int dCount = ds1.Tables[0].Rows.Count;
                    for (int i = 0; i < dCount; i++)
                    {
                        //获取MODEL
                        BCW.Model.Shopkeep n = new BCW.BLL.Shopkeep().GetShopkeep(int.Parse(ds1.Tables[0].Rows[i]["id"].ToString()));
                        if (n.State == 0)
                        {
                            if (n.AddTime.AddHours(n.BillEXP) < DateTime.Now)
                            {
                                n.State = 2;
                                n.GatewayType = n.GatewayType;
                                n.Attach = n.Attach;
                                n.BankCode = n.BankCode;
                                n.ProductType = n.ProductType;
                                new BCW.BLL.Shopkeep().Update_ips(n);
                            }
                            cn.com.ips.newpay.WSOrderQuery WSorder = new cn.com.ips.newpay.WSOrderQuery();
                            string SignatureOrder = "", bodystr = "", Resultstr = ""; ;
                            bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByChkOrderByShop(n, ref SignatureOrder);
                            string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost_ByOrder(DateTime.Now.ToString("yyyyMMddHHmmss"), SignatureOrder, bodystr);
                            Resultstr = WSorder.getOrderByMerBillNo(pGateWayReqstr);
                            BCW.IPSPay.IPSPayMent.updateorder(Resultstr, false);
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }


    }
}