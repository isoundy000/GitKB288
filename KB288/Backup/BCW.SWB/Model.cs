using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.SWB
{  /// <summary>
   /// Demo:实体类(属性说明自动提取数据库字段的描述信息)
   /// </summary>
    [Serializable]
    public partial class Model
    {
        public Model()
        { }
        #region Model
        private int _id;
        private int _userid;
        private long _money = 0;
        private int _gameid = 0;
        private DateTime _updatetime = DateTime.Now;
        private int _permission = 0;
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
        public long Money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GameID
        {
            set { _gameid = value; }
            get { return _gameid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Permission
        {
            set { _permission = value; }
            get { return _permission; }
        }
        #endregion Model

    }
}
