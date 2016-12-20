using System;
namespace BCW.Baccarat.Model
{
    /// <summary>
    /// 实体类BJL_Card 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class BJL_Card
    {
        public BJL_Card()
        { }
        #region Model
        private int _id;
        private int _roomid;
        private int _roomtable;
        private string _bankerpoker;
        private int _bankerpoint;
        private string _hunterpoker;
        private int _hunterpoint;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 房间ID
        /// </summary>
        public int RoomID
        {
            set { _roomid = value; }
            get { return _roomid; }
        }
        /// <summary>
        /// 局数
        /// </summary>
        public int RoomTable
        {
            set { _roomtable = value; }
            get { return _roomtable; }
        }
        /// <summary>
        /// 庄结果
        /// </summary>
        public string BankerPoker
        {
            set { _bankerpoker = value; }
            get { return _bankerpoker; }
        }
        /// <summary>
        /// 庄点
        /// </summary>
        public int BankerPoint
        {
            set { _bankerpoint = value; }
            get { return _bankerpoint; }
        }
        /// <summary>
        /// 闲结果
        /// </summary>
        public string HunterPoker
        {
            set { _hunterpoker = value; }
            get { return _hunterpoker; }
        }
        /// <summary>
        /// 闲点
        /// </summary>
        public int HunterPoint
        {
            set { _hunterpoint = value; }
            get { return _hunterpoint; }
        }
        #endregion Model

    }
}

