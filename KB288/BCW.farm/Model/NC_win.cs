using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_win 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_win
    {
        public NC_win()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _prize_id;
        private string _prize_name;
        private DateTime _addtime;
        private int _prize_type;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 奖品ID
        /// </summary>
        public int prize_id
        {
            set { _prize_id = value; }
            get { return _prize_id; }
        }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string prize_name
        {
            set { _prize_name = value; }
            get { return _prize_name; }
        }
        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 抽奖的类型0为币1为种子2为道具
        /// </summary>
        public int prize_type
        {
            set { _prize_type = value; }
            get { return _prize_type; }
        }
        #endregion Model

    }
}

