using System;
namespace TPR2.Model.guess
{
    /// <summary>
    /// 实体类Super 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Super
    {
        public Super()
        { }
        #region Model
        private int _id;
        private int _usid;
        private string _usname;
        private string _bid;
        private string _pid;
        private string _sp;
        private string _title;
        private string _times;
        private string _sttitle;
        private string _odds;
        private decimal? _paycent;
        private int _status;
        private int _isopen;
        private decimal? _getmoney;
        private string _getodds;
        private DateTime? _addtime;
        private int _p_isfs;
        private int? _p_case;
        private int _p_Auto;

        /// <summary>
        /// 自动开奖标识 0自动,1人工
        /// </summary>
        public int p_Auto
        {
            set { _p_Auto = value; }
            get { return _p_Auto; }
        }

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
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BID
        {
            set { _bid = value; }
            get { return _bid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PID
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SP
        {
            set { _sp = value; }
            get { return _sp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Times
        {
            set { _times = value; }
            get { return _times; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StTitle
        {
            set { _sttitle = value; }
            get { return _sttitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PayCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsOpen
        {
            set { _isopen = value; }
            get { return _isopen; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? getMoney
        {
            set { _getmoney = value; }
            get { return _getmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string getOdds
        {
            set { _getodds = value; }
            get { return _getodds; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int p_isfs
        {
            set { _p_isfs = value; }
            get { return _p_isfs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? p_case
        {
            set { _p_case = value; }
            get { return _p_case; }
        }
        #endregion Model

    }
}

