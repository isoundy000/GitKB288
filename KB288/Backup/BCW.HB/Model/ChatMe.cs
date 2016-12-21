using System;
namespace BCW.HB.Model
{
    /// <summary>
    /// ChatMe:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ChatMe
    {
        public ChatMe()
        { }
        #region Model
        private int _id;
        private int _chatid;
        private int _userid;
        private int _state = 0;
        private DateTime _jointime = DateTime.Now;
        private decimal _score = 0M;
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
        public int ChatID
        {
            set { _chatid = value; }
            get { return _chatid; }
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
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime jointime
        {
            set { _jointime = value; }
            get { return _jointime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal score
        {
            set { _score = value; }
            get { return _score; }
        }
        #endregion Model

    }
}

