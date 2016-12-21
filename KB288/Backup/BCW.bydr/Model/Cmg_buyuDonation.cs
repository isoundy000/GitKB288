using System;
namespace BCW.bydr.Model
{
    /// <summary>
    /// 实体类Cmg_buyuDonation 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Cmg_buyuDonation
    {
        public Cmg_buyuDonation()
        { }
        #region Model
        private int _id;
        private int? _usid;
        private DateTime? _time;
        private DateTime? _ctime;
        private long _donation;
        private int? _stype;
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
        public int? usID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Ctime
        {
            set { _ctime = value; }
            get { return _ctime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Donation
        {
            set { _donation = value; }
            get { return _donation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? stype
        {
            set { _stype = value; }
            get { return _stype; }
        }
        #endregion Model

    }
}

