using System;
namespace LHC.Model
{
    /// <summary>
    /// 实体类VotePay49 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class VotePay49
    {
        public VotePay49()
        { }
        #region Model
        private int _id;
        private int _types;
        private int _qino;
        private int _usid;
        private string _usname;
        private string _vote;
        private long _paycent;
        private long _wincent;
        private int _state;
        private DateTime _addtime;
        private int _bztype;
        private int _payNum;
        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 下注数
        /// </summary>
        public int PayNum
        {
            set { _payNum = value; }
            get { return _payNum; }
        }
        /// <summary>
        /// 投注类型
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 期数
        /// </summary>
        public int qiNo
        {
            set { _qino = value; }
            get { return _qino; }
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
        /// 投注内容
        /// </summary>
        public string Vote
        {
            set { _vote = value; }
            get { return _vote; }
        }
        /// <summary>
        /// 下注额
        /// </summary>
        public long payCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// 赢币额
        /// </summary>
        public long winCent
        {
            set { _wincent = value; }
            get { return _wincent; }
        }
        /// <summary>
        /// 状态（0未开奖/1已开奖/2已兑奖）
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 投注时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 币种(0虚拟币/1虚拟元)
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        #endregion Model

    }
}

