using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类Shopkeep 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Shopkeep
    {
        public Shopkeep()
        { }
        #region Model
        private int _id;
        private int _giftid;
        private string _title;
        private string _notes;
        private string _pic;
        private string _prevpic;
        private int _usid;
        private string _usname;
        private int _total;
        private string _para;
        private int _issex;
        private DateTime _addtime;
        private int _toptotal;

        private int _state;
        private int _nodeid;
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
        /// 充值结果
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
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
        /// 订单有效期 订单有效期以【小时】计算,没处理完则做失效处理
        /// </summary>
        public int BillEXP
        {
            set { _billexp = value; }
            get { return _billexp; }
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
        /// 礼物ID
        /// </summary>
        public int GiftId
        {
            set { _giftid = value; }
            get { return _giftid; }
        }
        /// <summary>
        /// 礼物名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 礼物描述
        /// </summary>
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }
        /// <summary>
        /// 礼物大图
        /// </summary>
        public string Pic
        {
            set { _pic = value; }
            get { return _pic; }
        }
        /// <summary>
        /// 礼物小图
        /// </summary>
        public string PrevPic
        {
            set { _prevpic = value; }
            get { return _prevpic; }
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
        /// 库存个数
        /// </summary>
        public int Total
        {
            set { _total = value; }
            get { return _total; }
        }
        /// <summary>
        /// 属性参数（积分|体力|魅力|智慧|威望|邪恶)写入如:0|0|0|0|0|0）
        /// </summary>
        public string Para
        {
            set { _para = value; }
            get { return _para; }
        }
        /// <summary>
        /// 是否启用性别限制(0不启用/1女生/2男生)
        /// </summary>
        public int IsSex
        {
            set { _issex = value; }
            get { return _issex; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 购买的个数（作排行榜用）
        /// </summary>
        public int TopTotal
        {
            set { _toptotal = value; }
            get { return _toptotal; }
        }
        #endregion Model

    }
}

