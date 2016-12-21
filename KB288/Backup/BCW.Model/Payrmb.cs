using System;
/// <summary>
/// 增加IPS支付信息
/// 黄国军 20160507
/// </summary>
namespace BCW.Model
{
    /// <summary>
    /// 实体类Payrmb 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Payrmb
    {
        public Payrmb()
        { }
        #region Model
        private int _id;
        private int _usid;
        private string _usname;
        private int _types;
        private int _cardamt;
        private string _cardnum;
        private string _cardpwd;
        private string _cardorder;
        private int _state;
        private string _addusip;
        private DateTime _addtime;
        private string _merbillno;
        private decimal _amount;
        private string _gatewaytype;
        private string _attach;
        private int _billexp;
        private string _goodsname;
        private string _iscredit;
        private string _bankcode;
        private string _producttype;

        /// <summary>
        /// 产品类型 1个人银行 2企业银行
        /// </summary>
        public string ProductType
        {
            set { _producttype = value; }
            get { return _producttype; }
        }
        /// <summary>
        /// IPS唯一标识指定的银行编号
        /// </summary>
        public string BankCode
        {
            set { _bankcode = value; }
            get { return _bankcode; }
        }
        /// <summary>
        /// 直连选项 1
        /// </summary>
        public string IsCredit
        {
            set { _iscredit = value; }
            get { return _iscredit; }
        }
        /// <summary>
        /// 商户购买商品的商品名称
        /// </summary>
        public string GoodsName
        {
            set { _goodsname = value; }
            get { return _goodsname; }
        }

        /// <summary>
        /// 商户数据包 数字+字母 原封返回
        /// </summary>
        public string Attach
        {
            set { _attach = value; }
            get { return _attach; }
        }

        /// <summary>
        /// 支付方式 01#借记卡02#信用卡03#IPS账户支付 默认01
        /// </summary>
        public string GatewayType
        {
            set { _gatewaytype = value; }
            get { return _gatewaytype; }
        }

        /// <summary>
        /// 订单金额 必填 保留两位小数
        /// </summary>
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }

        /// <summary>
        /// 商户订单号 必填字母及数字
        /// </summary>
        public string MerBillNo
        {
            set { _merbillno = value; }
            get { return _merbillno; }
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// 充值类型（1移动/2联通/3电信 100环迅）
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 卡面值
        /// </summary>
        public int CardAmt
        {
            set { _cardamt = value; }
            get { return _cardamt; }
        }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNum
        {
            set { _cardnum = value; }
            get { return _cardnum; }
        }
        /// <summary>
        /// 卡密码
        /// </summary>
        public string CardPwd
        {
            set { _cardpwd = value; }
            get { return _cardpwd; }
        }
        /// <summary>
        /// 卡排行
        /// </summary>
        public string CardOrder
        {
            set { _cardorder = value; }
            get { return _cardorder; }
        }
        /// <summary>
        /// 充值结果
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 充值IP
        /// </summary>
        public string AddUsIP
        {
            set { _addusip = value; }
            get { return _addusip; }
        }
        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }

        /// <summary>
        /// 订单有效期 订单有效期以【小时】计算,没处理完则做失效处理
        /// </summary>
        public int BillEXP
        {
            set { _billexp = value; }
            get { return _billexp; }
        }
        #endregion Model

    }
}

