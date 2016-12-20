using System;
namespace BCW.bydr.Model
{
    /// <summary>
    /// 实体类Cmg_notes 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Cmg_notes
    {
        public Cmg_notes()
        { }
        #region Model
        private int _id;
        private long _acolletgold;
        private string _coid;
        private int _vit;
        private int _usid;
        private DateTime _stime;
        private long _allgold;
        private string _changj;
        private int _stype;
        private DateTime _Signtime;
        private int _random;
        private int _cxid;
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
        public long AcolletGold
        {
            set { _acolletgold = value; }
            get { return _acolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string coID
        {
            set { _coid = value; }
            get { return _coid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Vit
        {
            set { _vit = value; }
            get { return _vit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int usID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Stime
        {
            set { _stime = value; }
            get { return _stime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AllGold
        {
            set { _allgold = value; }
            get { return _allgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string changj
        {
            set { _changj = value; }
            get { return _changj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int stype
        {
            set { _stype = value; }
            get { return _stype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Signtime
        {
            set { _Signtime = value; }
            get { return _Signtime; }
        }
        /// <summary>
        ///道具随机值
        /// </summary>
        public int random
        {
            set { _random = value; }
            get { return _random; }
        }
        /// <summary>
        ///查询id
        /// </summary>
        public int cxid
        {
            set { _cxid = value; }
            get { return _cxid; }
        }
        #endregion Model

    }
}

