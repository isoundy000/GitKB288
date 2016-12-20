using System;
namespace BCW.HB.Model
{
    /// <summary>
    /// HbPost:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class HbPost
    {
        public HbPost()
        { }
        #region Model
        private int _id;
        private int _userid;
        private int _num;
        private long _money;
        private string _radomnum;
        private DateTime _posttime;
        private string _getidlist;
        private long _maxradom;
        private string _note;
        private int _chatid;
        private int _state = 0;
        private int _style = 7;
        private string _keys;
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
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RadomNum
        {
            set { _radomnum = value; }
            get { return _radomnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PostTime
        {
            set { _posttime = value; }
            get { return _posttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetIDList
        {
            set { _getidlist = value; }
            get { return _getidlist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long MaxRadom
        {
            set { _maxradom = value; }
            get { return _maxradom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ChatID
        {
            set { _chatid = value; }
            get { return _chatid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Style
        {
            set { _style = value; }
            get { return _style; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        #endregion Model
    }
}
