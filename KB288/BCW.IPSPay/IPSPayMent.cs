using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using BCW.Common;
using System.Text.RegularExpressions;
/// <summary>
/// 增加中介专用链支付
/// 黄国军 20160526
/// 
/// 增加商城商品在线支付
/// 黄国军 20160521
/// 
/// 环迅支付提交类
/// 黄国军 20160506
/// 
/// 增加网上充值产生点值抽奖接口
/// 蒙宗将 20160823
/// </summary>
namespace BCW.IPSPay
{
    public class IPSPayMent
    {
        protected static string xmlPath = "/Controls/finance.xml";
        public IPSPayMent() { }

        public static String[] BankCodes = { "1100", "1101", "1102", "1107", "1108", "1109", "1114", "1119", "1106" };
        public static String[] BankNames = { "工商银行", "农业银行", "招商银行", "中国银行", "交通银行", "浦发银行", "广发银行", "邮储银行", "中国建设银行" };
        //string BankCodestr = "|1103|关业银行|1104|中信银行|1110|民生银行|1111|华夏银行|1112|光大银行|1113|北京银行|1114|广发银行|1115|南京银行|";
        //BankCodestr += "1116|上海银行|1117|杭州银行|1118|宁波银行|1119|邮储银行|1120|浙商银行|1121|平安银行|1122|东亚银行|1123|渤海银行|1124|北京农商行|1127|浙江泰隆商业银行|1106|中国建设银行";

        #region 变量定义
        // 交易的请求地址
        /// <summary>
        /// 此地址是转接用于提交到环迅平台注册的地址,由此地址做提交到API中
        /// </summary>
        public static string IPS_URL_CHN = "";

        /// <summary>
        /// IPS页面跳转地址,指定地址接受提交
        /// </summary>
        public static string IPS_POST_URL = "http://ips.rdfmy.top/bbs/";

        /// <summary>
        /// IPS提交地址
        /// </summary>
        public static string IPS_URL_IPS = "https://newpay.ips.com.cn/psfp-entry/gateway/payment.do";
        /// <summary>
        /// IPS订单查询地址
        /// </summary>
        public static string IPS_URL_ORDER = "https://newpay.ips.com.cn/psfp-entry/services/order?wsdl";

        public static string IpsKey = "vips_kb288";
        /// <summary>
        /// 商户编号
        /// </summary>
        //private static string merchantId = "179771";
        private static string merchantId = "181931";
        /// <summary>
        /// 商户名
        /// </summary>
        private static string MerName = "ccss";
        /// <summary>
        /// 交易账户号
        /// </summary>
        //private static string Account = "1797710017";
        private static string Account = "1819310010";
        /// <summary>
        /// 版本号
        /// </summary>
        private static string Ver = "v1.0.0";
        /// <summary>
        /// 数字签名
        /// </summary>
        //private static string Sign = "upjgy4DzRYMcBydnaBDmhUwvY3oa8FYJpNBFZmLqGgytOAmMr4JxoB71hSoo3KssnK6SOXsTyQxwPz90oxB4DpMPl0CvONewFOFAX91lN5PBmDlEbT9KomVakoArNwcU";
        private static string Sign = "AQdNRC2vTNYGbWj56nnv9EhEh7SaBShnqnvB0CAXX869pZepqTlVS8mnY1sE30gvKBeZceHXqRK8DbZevctEyX3u6GjdkkDQeh0jW59yfOuUcm0QWKB961MfhXuzqqOw";
        /// <summary>
        /// 支付成功返回的处理页面
        /// </summary>
        private static string Merchanturl = "Merchanturl.aspx";
        /// <summary>
        /// 支付失败返回的处理页面
        /// </summary>
        private static string FailUrl = "FailUrl.aspx";
        #endregion

        #region 根据银行代码返回银行名字 GetBankNameByCode
        /// <summary>
        /// 根据银行代码返回银行名字
        /// </summary>
        /// <param name="BkCode"></param>
        /// <returns></returns>
        public static string GetBankNameByCode(string BkCode)
        {
            string nBankName = "";
            for (int i = 0; i < BankCodes.Length; i++)
            {
                if (BkCode == BankCodes[i])
                {
                    nBankName = BankNames[i];
                    break;
                }
            }
            return nBankName;
        }
        #endregion

        #region 环迅支付提交 IPSPayMentPost
        /// <summary>
        /// 环迅支付提交 隐含字符
        /// </summary>
        /// <param name="Ver">版本号 可选</param>
        /// <param name="MerCode">商户号 必填</param>
        /// <param name="MerName">商户名 可选</param>
        /// <param name="Account">交易账户号 必填</param>
        /// <param name="MsgId">消息唯一标识,交易必填,查询可选</param>
        /// <param name="ReqDate">商户请求时间 必填 格式yyyyMMddHHmmss</param>
        /// <param name="Signature">数字签名 必填</param>
        /// <param name="iBody">body字符串</param>
        /// <returns></returns>
        public static string IPSPayMentPost(string MsgId, string ReqDate, string Signature, string iBody)
        {
            //提交报文
            string pGateWayReqstr = "<Ips><GateWayReq><head>";
            pGateWayReqstr += string.Format("<Version>{0}</Version>", Ver);                                     //版本号 可选
            pGateWayReqstr += string.Format("<MerCode>{0}</MerCode>", merchantId);                              //商户号 必填
            pGateWayReqstr += string.Format("<MerName>{0}</MerName>", MerName);                                 //商户名 可选
            pGateWayReqstr += string.Format("<Account>{0}</Account>", Account);                                 //交易账户号 必填
            pGateWayReqstr += string.Format("<MsgId>{0}</MsgId>", MsgId);                                       //消息唯一标识,交易必填,查询可选
            pGateWayReqstr += string.Format("<ReqDate>{0}</ReqDate>", ReqDate);                                 //商户请求时间 必填 格式 yyyyMMddHHmmss
            pGateWayReqstr += string.Format("<Signature>{0}</Signature></head>", Signature);                    //数字签名 必填 
            //签名根据body中的RetEncodeTypeRetEncodeTypeRetEncodeType RetEncodeTypeRetEncodeTypeRetEncodeType RetEncodeTypeRetEncodeTypeRetEncodeTypeRetEncodeType值
            //决 定Body节点丌为空 对< body >……</ body > 节点字符串 + 商户号 + 商户数字证书迚行签名（包括body标签）；body节点为空时 使用商户号+商户证书的迚行签名
            //注意：+号在此处代表字符串连接。
            pGateWayReqstr += iBody;
            pGateWayReqstr += string.Format("</GateWayReq></Ips>");
            return pGateWayReqstr;
        }
        #endregion

        #region 环迅支付查询表头 IPSPayMentPost_ByOrder
        /// <summary>
        /// 环迅支付查询表头
        /// </summary>
        /// <param name="ReqDate"></param>
        /// <param name="Signature"></param>
        /// <param name="iBody"></param>
        /// <returns></returns>
        public static string IPSPayMentPost_ByOrder(string ReqDate, string Signature, string iBody)
        {
            //提交报文
            string pGateWayReqstr = "<Ips><OrderQueryReq><head>";
            pGateWayReqstr += string.Format("<Version>{0}</Version>", Ver);                                     //版本号 可选
            pGateWayReqstr += string.Format("<MerCode>{0}</MerCode>", merchantId);                              //商户号 必填
            pGateWayReqstr += string.Format("<MerName>{0}</MerName>", MerName);                                 //商户名 可选
            pGateWayReqstr += string.Format("<Account>{0}</Account>", Account);                                 //交易账户号 必填
            pGateWayReqstr += string.Format("<ReqDate>{0}</ReqDate>", ReqDate);                                 //商户请求时间 必填 格式 yyyyMMddHHmmss
            pGateWayReqstr += string.Format("<Signature>{0}</Signature></head>", Signature);                    //数字签名 必填 
            //签名根据body中的RetEncodeTypeRetEncodeTypeRetEncodeType RetEncodeTypeRetEncodeTypeRetEncodeType RetEncodeTypeRetEncodeTypeRetEncodeTypeRetEncodeType值
            //决 定Body节点丌为空 对< body >……</ body > 节点字符串 + 商户号 + 商户数字证书迚行签名（包括body标签）；body节点为空时 使用商户号+商户证书的迚行签名
            //注意：+号在此处代表字符串连接。
            pGateWayReqstr += iBody;
            pGateWayReqstr += string.Format("</OrderQueryReq></Ips>");
            return pGateWayReqstr;
        }
        #endregion

        #region 获得【网上支付】数字签名和body字符串 GetSignature
        /// <summary>
        /// 获得数字签名和body字符串
        /// </summary>
        /// <param name="model">订单实体</param>
        /// <param name="iBankCode">银行标识</param>
        /// <param name="iGatewayType">支付类型</param>
        /// <param name="usUrl">用户页面</param>
        /// <param name="Signature">返回数字签名</param>
        public static string GetSignature(string DomUrl, BCW.Model.Payrmb model, string iBankCode, string iGatewayType, string usUrl, ref string Signature)
        {
            string pBody = "";
            if (model != null)
            {
                //string DomUrl = "http://" + Utils.GetDomain() + "/";
                string attachstr = model.ID + "," + model.UsID + "," + model.GoodsName + "," + iBankCode + "," + usUrl;
                attachstr = BCW.Common.DESEncrypt.Encrypt(attachstr, IpsKey);
                pBody += string.Format("<body><MerBillNo>{0}</MerBillNo>", model.MerBillNo);            //商户订单号 必填字母及数字
                pBody += string.Format("<Amount>{0}</Amount>", model.Amount.ToString("0.00"));          //订单金额 必填 保留两位小数
                pBody += string.Format("<Date>{0}</Date>", model.AddTime.ToString("yyyyMMdd"));         //订单日期 格式 yyyyMMdd
                pBody += string.Format("<CurrencyType>{0}</CurrencyType>", "156");                       //币种 人民币:156
                pBody += string.Format("<GatewayType>{0}</GatewayType>", iGatewayType);                 //支付方式 01#借记卡02#信用卡03#IPS账户支付 默认01
                pBody += string.Format("<Lang>{0}</Lang>", "GB");                                       //语言 GB
                pBody += string.Format("<Merchanturl>{0}</Merchanturl>", DomUrl + Merchanturl);         //支付成功返回的商户URL
                pBody += string.Format("<FailUrl>{0}</FailUrl>", DomUrl + FailUrl);                     //支付失败返回的商户URL
                pBody += string.Format("<Attach>{0}</Attach>", attachstr);                              //商户数据包 数字+字母 原封返回 目前输入的是订单ID+USID
                pBody += string.Format("<OrderEncodeType>{0}</OrderEncodeType>", "5");                  //订单支付加密方式 5采用MD5加密
                pBody += string.Format("<RetEncodeType>{0}</RetEncodeType>", "17");                     //交易接口返回加密方式 17采用MD5摘要加密
                pBody += string.Format("<RetType>{0}</RetType>", "1");                                  //返回方式 1:S2S返回
                pBody += string.Format("<ServerUrl>{0}</ServerUrl>", Merchanturl);                      //异步S2S
                pBody += string.Format("<BillEXP>{0}</BillEXP>", model.BillEXP);                        //订单有效期 订单有效期以【小时】计算,没处理完则做失效处理
                pBody += string.Format("<GoodsName>{0}</GoodsName>", model.GoodsName);                  //商户购买商品的商品名称
                pBody += string.Format("<IsCredit>{0}</IsCredit>", "");                                //直连选项 1 环迅收款台
                pBody += string.Format("<BankCode>{0}</BankCode>", "");                          //IPS唯一标识指定的银行编号 00018
                pBody += string.Format("<ProductType>{0}</ProductType>", "1");                          //产品类型 1个人银行 2企业银行
                pBody += string.Format("</body>");
            }
            //生成数字签名 <body>……</body>节点字符串+商户号+商户数字证书迚行签名（包括body标签）
            if (pBody != "")
            {
                string Rs = pBody + merchantId + Sign;
                MD5CryptoServiceProvider hashmd5;
                hashmd5 = new MD5CryptoServiceProvider();
                Signature = StringToMD5Hash(Rs);
            }
            return pBody;
        }
        #endregion

        #region 获得【商城物品】数字签名和body字符串 GetSignatureByShop
        /// <summary>
        /// 获得数字签名和body字符串
        /// </summary>
        /// <param name="model">订单实体</param>
        /// <param name="iBankCode">银行标识</param>
        /// <param name="iGatewayType">支付类型</param>
        /// <param name="usUrl">用户页面</param>
        /// <param name="Signature">返回数字签名</param>
        public static string GetSignatureByShop(string DomUrl, BCW.Model.Shopkeep model, string iBankCode, string iGatewayType, string usUrl, ref string Signature)
        {
            string pBody = "";
            if (model != null)
            {
                //string DomUrl = "http://" + Utils.GetDomain() + "/";
                string attachstr = model.ID + "," + model.UsID + "," + model.GoodsName + "," + iBankCode + "," + usUrl;
                attachstr = BCW.Common.DESEncrypt.Encrypt(attachstr, IpsKey);
                pBody += string.Format("<body><MerBillNo>{0}</MerBillNo>", model.MerBillNo);            //商户订单号 必填字母及数字
                pBody += string.Format("<Amount>{0}</Amount>", model.Amount.ToString("0.00"));          //订单金额 必填 保留两位小数
                pBody += string.Format("<Date>{0}</Date>", model.AddTime.ToString("yyyyMMdd"));         //订单日期 格式 yyyyMMdd
                pBody += string.Format("<CurrencyType>{0}</CurrencyType>", "156");                       //币种 人民币:156
                pBody += string.Format("<GatewayType>{0}</GatewayType>", iGatewayType);                 //支付方式 01#借记卡02#信用卡03#IPS账户支付 默认01
                pBody += string.Format("<Lang>{0}</Lang>", "GB");                                       //语言 GB
                pBody += string.Format("<Merchanturl>{0}</Merchanturl>", DomUrl + Merchanturl);         //支付成功返回的商户URL
                pBody += string.Format("<FailUrl>{0}</FailUrl>", DomUrl + FailUrl);                     //支付失败返回的商户URL
                pBody += string.Format("<Attach>{0}</Attach>", attachstr);                              //商户数据包 数字+字母 原封返回 目前输入的是订单ID+USID
                pBody += string.Format("<OrderEncodeType>{0}</OrderEncodeType>", "5");                  //订单支付加密方式 5采用MD5加密
                pBody += string.Format("<RetEncodeType>{0}</RetEncodeType>", "17");                     //交易接口返回加密方式 17采用MD5摘要加密
                pBody += string.Format("<RetType>{0}</RetType>", "1");                                  //返回方式 1:S2S返回
                pBody += string.Format("<ServerUrl>{0}</ServerUrl>", Merchanturl);                      //异步S2S
                pBody += string.Format("<BillEXP>{0}</BillEXP>", model.BillEXP);                        //订单有效期 订单有效期以【小时】计算,没处理完则做失效处理
                pBody += string.Format("<GoodsName>{0}</GoodsName>", model.GoodsName);                  //商户购买商品的商品名称
                pBody += string.Format("<IsCredit>{0}</IsCredit>", "2");                                //直连选项 1
                pBody += string.Format("<BankCode>{0}</BankCode>", iBankCode);                          //IPS唯一标识指定的银行编号 00018
                pBody += string.Format("<ProductType>{0}</ProductType>", "1");                          //产品类型 1个人银行 2企业银行
                pBody += string.Format("</body>");
            }
            //生成数字签名 <body>……</body>节点字符串+商户号+商户数字证书迚行签名（包括body标签）
            if (pBody != "")
            {
                string Rs = pBody + merchantId + Sign;
                MD5CryptoServiceProvider hashmd5;
                hashmd5 = new MD5CryptoServiceProvider();
                Signature = StringToMD5Hash(Rs);
            }
            return pBody;
        }
        #endregion

        #region 按商户订单查询 GetSignatureByChkOrder
        /// <summary>
        /// 按商户订单查询
        /// </summary>
        /// <param name="model">订单实体</param>
        /// <param name="SignatureOrder"></param>
        /// <returns></returns>
        public static string GetSignatureByChkOrder(BCW.Model.Payrmb model, ref string SignatureOrder)
        {
            string pBody = "";
            if (model != null)
            {
                pBody += string.Format("<body><MerBillNo>{0}</MerBillNo>", model.MerBillNo);            //商户订单号 必填字母及数字
                pBody += string.Format("<Date>{0}</Date>", model.AddTime.ToString("yyyyMMdd"));         //订单日期 格式 yyyyMMdd
                pBody += string.Format("<Amount>{0}</Amount>", model.Amount.ToString("0.00"));          //订单金额 必填 保留两位小数                
                pBody += string.Format("</body>");
            }
            //生成数字签名 <body>……</body>节点字符串+商户号+商户数字证书迚行签名（包括body标签）
            if (pBody != "")
            {
                string Rs = pBody + merchantId + Sign;
                MD5CryptoServiceProvider hashmd5;
                hashmd5 = new MD5CryptoServiceProvider();
                SignatureOrder = StringToMD5Hash(Rs);
            }
            return pBody;
        }
        #endregion

        #region 【商城】按商户订单查询 GetSignatureByChkOrderByShop
        /// <summary>
        /// 按商户订单查询
        /// </summary>
        /// <param name="model">订单实体</param>
        /// <param name="SignatureOrder"></param>
        /// <returns></returns>
        public static string GetSignatureByChkOrderByShop(BCW.Model.Shopkeep model, ref string SignatureOrder)
        {
            string pBody = "";
            if (model != null)
            {
                pBody += string.Format("<body><MerBillNo>{0}</MerBillNo>", model.MerBillNo);            //商户订单号 必填字母及数字
                pBody += string.Format("<Date>{0}</Date>", model.AddTime.ToString("yyyyMMdd"));         //订单日期 格式 yyyyMMdd
                pBody += string.Format("<Amount>{0}</Amount>", model.Amount.ToString("0.00"));          //订单金额 必填 保留两位小数                
                pBody += string.Format("</body>");
            }
            //生成数字签名 <body>……</body>节点字符串+商户号+商户数字证书迚行签名（包括body标签）
            if (pBody != "")
            {
                string Rs = pBody + merchantId + Sign;
                MD5CryptoServiceProvider hashmd5;
                hashmd5 = new MD5CryptoServiceProvider();
                SignatureOrder = StringToMD5Hash(Rs);
            }
            return pBody;
        }
        #endregion

        #region MD5　32位加密 StringToMD5Hash
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToMD5Hash(string inputString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
        #endregion

        #region 处理环迅接口返回数据 updateorder
        /// <summary>
        /// 处理环迅接口返回数据
        /// </summary>
        /// <param name="Resultstr">返回字符串</param>
        /// <param name="isTips">是否提示处理状态</param>
        public static void updateorder(string Resultstr, bool isTips)
        {
            if (Resultstr != "")
            {
                #region 返回处理
                Resultstr = Resultstr.Replace("<![CDATA[", "").Replace("]]>", "");
                string MerBillNostr = "";   //单号
                string CurrencyType = "";   //币种
                decimal Amount = 0;         //订单金额
                string date = "";           //订单日期
                string Status = "";         //交易状态
                string RspCode = "";        //返回码
                string Attach = "";         //商户数据包
                string IpsBillNo = "";      //IPS订单号
                string IpsTradeNo = "";     //交易流水号
                string BankBillNo = "";     //银行订单号

                #region 获取订单号 MerBillNostr
                //获取订单号
                string strpattern = @"<MerBillNo>([\s\S]{1,20})</MerBillNo>";//单号
                Match mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    MerBillNostr = mtitle.Groups[1].Value;
                }
                #endregion

                if (MerBillNostr != "")
                {
                    #region 获取订单金额 Amount
                    strpattern = @"<Amount>([\s\S]{1,10})</Amount>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        Amount = decimal.Parse(mtitle.Groups[1].Value);
                    }
                    #endregion

                    #region 获取订单日期 date
                    strpattern = @"<date>([\s\S]{1,10})</date>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        date = mtitle.Groups[1].Value;
                    }
                    #endregion

                    #region 获取支付币种 CurrencyType
                    strpattern = @"<CurrencyType>([\s\S]{1,10})</CurrencyType>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        CurrencyType = mtitle.Groups[1].Value;
                    }
                    #endregion

                    #region 交易状态 Status
                    //Y#交易成功；N#交易失败；P#交易处理中
                    strpattern = @"<Status>([\s\S]{1,10})</Status>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        Status = mtitle.Groups[1].Value;
                    }
                    #endregion

                    #region 商户数据包 Attach
                    strpattern = @"<Attach>([\s\S]*)</Attach>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        Attach = mtitle.Groups[1].Value;
                    }
                    #endregion

                    #region 商户数据包 Attach
                    strpattern = @"<BankBillNo>([\s\S]*)</BankBillNo>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        BankBillNo = mtitle.Groups[1].Value;
                    }
                    #endregion

                    #region 返回码 RspCode
                    strpattern = @"<RspCode>([\s\S]*)</RspCode>";
                    mtitle = Regex.Match(Resultstr, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        RspCode = mtitle.Groups[1].Value;
                    }
                    #endregion

                    #region 充值到账户
                    try
                    {
                        Attach = BCW.Common.DESEncrypt.Decrypt(Attach, BCW.IPSPay.IPSPayMent.IpsKey);
                    }
                    catch { }

                    if (Attach.Split(',').Length == 5)
                    {
                        int id = 0;
                        try
                        {
                            id = int.Parse(Attach.Split(',')[0].ToString());
                        }
                        catch { }
                        if (id != 0)
                        {
                            ///shop开头的订单,是属于商城的订单,无任何字符开头的订单,是属于网上充值的订单
                            int typeMer = 0;//订单类型0默认订单 1商城订单
                            if (MerBillNostr.StartsWith("Shop")) { typeMer = 1; }
                            else if (MerBillNostr.Contains("AGENC")) { typeMer = 2; }
                            switch (typeMer)
                            {
                                case 2:
                                    {
                                        #region 更新到中介充值数据库
                                        BCW.Model.Payrmb model = new BCW.BLL.Payrmb().GetPayrmb(id);
                                        if (model != null && model.State == 0)
                                        {
                                            //对比订单 
                                            if (model.MerBillNo == MerBillNostr)
                                            {
                                                ////对比日期
                                                //if (model.AddTime.ToString("yyyyMMdd") == date)
                                                //{
                                                //对比金额
                                                if (model.Amount.ToString("0.00") == Amount.ToString("0.00"))
                                                {
                                                    //以上都对,则更新数据库
                                                    if (Status == "Y")
                                                    {
                                                        //成功
                                                        model.State = 1;
                                                    }
                                                    else
                                                    {
                                                        //错误
                                                        model.State = 2;
                                                    }
                                                    model.GatewayType = CurrencyType;
                                                    model.Attach = Attach;
                                                    model.BankCode = Attach.Split(',')[3].ToString();
                                                    model.ProductType = "1";
                                                    new BCW.BLL.Payrmb().Update_ips(model);

                                                    #region 中介过币
                                                    //开始过币
                                                    string toname = new BCW.BLL.User().GetUsName(model.UsID);
                                                    string Coin = ub.GetSub("FinanceSZXTar", xmlPath);
                                                    long uGoid = long.Parse((model.Amount * decimal.Parse(Coin)).ToString("0"));
                                                    string[] sArray = Regex.Split(MerBillNostr, "AGENC", RegexOptions.IgnoreCase);
                                                    int Cid = int.Parse(sArray[0]);
                                                    string mename = new BCW.BLL.User().GetUsName(Cid);
                                                    new BCW.BLL.User().UpdateiGold(Cid, mename, -uGoid, "过户给ID" + model.UsID + "(中介专用网充链接)");
                                                    new BCW.BLL.User().UpdateiGold(model.UsID, toname, uGoid, "来自ID" + Cid + "过户(中介专用网充链接)");


                                                    BCW.Model.Transfer T_model = new BCW.Model.Transfer();
                                                    T_model.Types = 0;
                                                    T_model.FromId = Cid;
                                                    T_model.FromName = mename;
                                                    T_model.ToId = model.UsID;
                                                    T_model.ToName = toname;
                                                    T_model.AcCent = uGoid;
                                                    T_model.AddTime = DateTime.Now;
                                                    T_model.zfbNo = MerBillNostr;
                                                    new BCW.BLL.Transfer().Add(T_model);

                                                    //系统内线消息
                                                    new BCW.BLL.Guest().Add(model.UsID, toname, "[url=/bbs/uinfo.aspx?uid=" + Cid + "]" + mename + "[/url]过户" + uGoid + "" + ub.Get("SiteBz") + "给您(网," + MerBillNostr + ")");
                                                    new BCW.BLL.Guest().Add(Cid, mename, "您过户" + uGoid + "" + ub.Get("SiteBz") + "给[url=/bbs/uinfo.aspx?uid=" + model.UsID + "] " + toname + " [/url]成功(网," + MerBillNostr + ")");

                                                    #endregion

                                                    if (isTips)
                                                    {
                                                        new BCW.BLL.Guest().Add(10086, "客服", "ID：" + model.UsID + "商城购物支付,订单支付成功,单号:" + model.MerBillNo);
                                                        Utils.Success("充值成功", "充值成功", Attach.Split(',')[4].ToString(), "1");
                                                    }
                                                }
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            if (isTips)
                                                Utils.Success("订单已处理", "订单已处理", Attach.Split(',')[4].ToString(), "1");
                                        }
                                        #endregion
                                    }
                                    break;
                                case 1:
                                    {
                                        #region 更新商城订单
                                        BCW.Model.Shopkeep model = new BCW.BLL.Shopkeep().GetShopkeep(id);
                                        if (model != null && model.State == 0)
                                        {
                                            //对比订单 
                                            if (model.MerBillNo == MerBillNostr)
                                            {
                                                ////对比日期
                                                //if (model.AddTime.ToString("yyyyMMdd") == date)
                                                //{
                                                //对比金额
                                                if (model.Amount.ToString("0.00") == Amount.ToString("0.00"))
                                                {
                                                    //以上都对,则更新数据库
                                                    if (Status == "Y")
                                                    {
                                                        //成功
                                                        model.State = 1;
                                                    }
                                                    else
                                                    {
                                                        //错误
                                                        model.State = 2;
                                                    }
                                                    model.GatewayType = CurrencyType;
                                                    model.Attach = Attach;
                                                    model.BankCode = Attach.Split(',')[3].ToString();
                                                    model.ProductType = "1";
                                                    new BCW.BLL.Shopkeep().Update_ips(model);
                                                    string Coin = model.Para;
                                                    long uGoid = long.Parse((model.Total * decimal.Parse(Coin)).ToString("0"));
                                                    //充值酷币
                                                    new BCW.BLL.User().UpdateiGold(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), uGoid, "商品" + model.Title + "(" + model.GiftId + ")支付成功,订单号:" + MerBillNostr);
                                                    new BCW.BLL.Guest().Add(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), "支付订单:" + model.MerBillNo + "成功[url=" + Attach.Split(',')[4].ToString() + "]查看[/url]");
                                                    if (isTips)
                                                    {
                                                        new BCW.BLL.Guest().Add(10086, "客服", "ID：" + model.UsID + "商城购物支付,订单支付成功,单号:" + model.MerBillNo);
                                                        Utils.Success("支付成功", "支付成功", Attach.Split(',')[4].ToString(), "1");
                                                    }
                                                }
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            if (isTips)
                                                Utils.Success("订单已处理", "订单已处理", Attach.Split(',')[4].ToString(), "1");
                                        }
                                        #endregion
                                    }
                                    break;
                                case 0:
                                    {
                                        #region 更新到网上支付数据库
                                        BCW.Model.Payrmb model = new BCW.BLL.Payrmb().GetPayrmb(id);
                                        if (model != null && model.State == 0)
                                        {
                                            //对比订单 
                                            if (model.MerBillNo == MerBillNostr)
                                            {
                                                ////对比日期
                                                //if (model.AddTime.ToString("yyyyMMdd") == date)
                                                //{
                                                //对比金额
                                                if (model.Amount.ToString("0.00") == Amount.ToString("0.00"))
                                                {
                                                    //以上都对,则更新数据库
                                                    if (Status == "Y")
                                                    {
                                                        //成功
                                                        model.State = 1;
                                                    }
                                                    else
                                                    {
                                                        //错误
                                                        model.State = 2;
                                                    }
                                                    model.GatewayType = CurrencyType;
                                                    model.Attach = Attach;
                                                    model.BankCode = Attach.Split(',')[3].ToString();
                                                    model.ProductType = "1";
                                                    new BCW.BLL.Payrmb().Update_ips(model);
                                                    if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
                                                    {
                                                        string Coin = ub.GetSub("FinanceSZXTar", xmlPath);
                                                        long uGoid = long.Parse((model.Amount * decimal.Parse(Coin)).ToString("0"));
                                                        //充值酷币
                                                        new BCW.BLL.User().UpdateiGold(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), uGoid, "网上支付成功,订单号:" + MerBillNostr);
                                                        new BCW.BLL.Guest().Add(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), "网上支付订单:" + model.MerBillNo + "成功[url=" + Attach.Split(',')[4].ToString() + "]查看[/url]");

                                                        //点值抽奖
                                                        try
                                                        {
                                                            new BCW.Draw.draw().Addjfbychongzhi(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), model.Amount, "网上充值" + model.Amount + "奖励");
                                                        }
                                                        catch { }

                                                    }
                                                    else
                                                    { }
                                                    if (isTips)
                                                    {
                                                        new BCW.BLL.Guest().Add(10086, "客服", "ID：" + model.UsID + "商城购物支付,订单支付成功,单号:" + model.MerBillNo);
                                                        Utils.Success("充值成功", "充值成功", Attach.Split(',')[4].ToString(), "1");
                                                    }
                                                }
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            if (isTips)
                                                Utils.Success("订单已处理", "订单已处理", Attach.Split(',')[4].ToString(), "1");
                                        }
                                        #endregion
                                    }
                                    break;
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                //Utils.Success("数据错误", "提交数据错误", "Default.aspx", "1");
            }
        }
        #endregion
    }
}
