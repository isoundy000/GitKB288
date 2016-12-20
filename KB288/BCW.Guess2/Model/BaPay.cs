using System;
/// <summary>
/// 更新人工确认标识
/// 增加字段sure
/// 黄国军 20160324
/// </summary>
namespace TPR2.Model.guess
{
    /// <summary>
    /// 实体类BaPay 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class BaPay
    {
        public BaPay()
        { }
        #region Model
        private int _id;
        private int? _types;
        private string _payview;
        private int? _payusid;
        private string _payusname;
        private int? _bcid;
        private int? _ptype;
        private int? _paytype;
        private decimal? _paycent;
        private decimal? _payonluone;
        private decimal? _payonlutwo;
        private decimal? _payonluthr;
        private decimal? _p_pk;
        private decimal? _p_dx_pk;
        private int? _p_pn;
        private int? _p_result_one;
        private int? _p_result_two;
        private int? _p_active;
        private int? _p_case;
        private decimal? _p_getmoney;
        private DateTime? _paytimes;
        private decimal _paycount;
        private decimal _paycents;
        private int? _p_result_temp1;
        private int? _p_result_temp2;
        private int? _itypes;
        private int _state;
        private DateTime _p_tprtime;
        private DateTime _p_oncetime2;
        private int _sure;
        private int _isrobot;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string payview
        {
            set { _payview = value; }
            get { return _payview; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? payusid
        {
            set { _payusid = value; }
            get { return _payusid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string payusname
        {
            set { _payusname = value; }
            get { return _payusname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? bcid
        {
            set { _bcid = value; }
            get { return _bcid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? pType
        {
            set { _ptype = value; }
            get { return _ptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? payCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? payonLuone
        {
            set { _payonluone = value; }
            get { return _payonluone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? payonLutwo
        {
            set { _payonlutwo = value; }
            get { return _payonlutwo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? payonLuthr
        {
            set { _payonluthr = value; }
            get { return _payonluthr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? p_pk
        {
            set { _p_pk = value; }
            get { return _p_pk; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? p_dx_pk
        {
            set { _p_dx_pk = value; }
            get { return _p_dx_pk; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_pn
        {
            set { _p_pn = value; }
            get { return _p_pn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_result_one
        {
            set { _p_result_one = value; }
            get { return _p_result_one; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_result_two
        {
            set { _p_result_two = value; }
            get { return _p_result_two; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_active
        {
            set { _p_active = value; }
            get { return _p_active; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_case
        {
            set { _p_case = value; }
            get { return _p_case; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? p_getMoney
        {
            set { _p_getmoney = value; }
            get { return _p_getmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? paytimes
        {
            set { _paytimes = value; }
            get { return _paytimes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal payCount
        {
            set { _paycount = value; }
            get { return _paycount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal payCents
        {
            set { _paycents = value; }
            get { return _paycents; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_result_temp1
        {
            set { _p_result_temp1 = value; }
            get { return _p_result_temp1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_result_temp2
        {
            set { _p_result_temp2 = value; }
            get { return _p_result_temp2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? itypes
        {
            set { _itypes = value; }
            get { return _itypes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int state
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// 人工确认字段
        /// </summary>
        public int sure
        {
            set { _sure = value; }
            get { return _sure; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime p_TPRtime
        {
            set { _p_tprtime = value; }
            get { return _p_tprtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime p_oncetime2
        {
            set { _p_oncetime2 = value; }
            get { return _p_oncetime2; }
        }
        public int isrobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
        #endregion Model

    }
}

