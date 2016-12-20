using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_Goldlog 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_Goldlog
    {
        public NC_Goldlog()
        { }
        #region Model
        private int _id;
        private int _types;
        private string _purl;
        private int _usid;
        private string _usname;
        private long _acgold;
        private long _aftergold;
        private string _actext;
        private DateTime _addtime;
        private int _bbtag;
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
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PUrl
        {
            set { _purl = value; }
            get { return _purl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UsId
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
        public long AcGold
        {
            set { _acgold = value; }
            get { return _acgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AfterGold
        {
            set { _aftergold = value; }
            get { return _aftergold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AcText
        {
            set { _actext = value; }
            get { return _actext; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BbTag
        {
            set { _bbtag = value; }
            get { return _bbtag; }
        }
        #endregion Model

    }
}

