using System;
namespace BCW.bydr.Model
{
    /// <summary>
    /// 实体类CmgTicket 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class CmgTicket
    {
        public CmgTicket()
        { }
        #region Model
        private int _id;
        private long _colletgold;
        private int? _bid;
        private DateTime? _time;
        private int? _usid;
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
        public long ColletGold
        {
            set { _colletgold = value; }
            get { return _colletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Bid
        {
            set { _bid = value; }
            get { return _bid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? usID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        #endregion Model

    }
}

