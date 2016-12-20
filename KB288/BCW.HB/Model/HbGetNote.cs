using System;
namespace BCW.HB.Model
{
    /// <summary>
    /// HbGetNote:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class HbGetNote
    {
        public HbGetNote()
        { }
        #region Model
        private int _id;
        private string _hbid;
        private string _getid;
        private long _getmoney;
        private DateTime _gettime;
        private int _ismax;
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
        public string HbID
        {
            set { _hbid = value; }
            get { return _hbid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetID
        {
            set { _getid = value; }
            get { return _getid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long GetMoney
        {
            set { _getmoney = value; }
            get { return _getmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime GetTime
        {
            set { _gettime = value; }
            get { return _gettime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsMax
        {
            set { _ismax = value; }
            get { return _ismax; }
        }
        #endregion Model

    }
}
