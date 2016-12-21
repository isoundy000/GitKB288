using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_daoju_use 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_daoju_use
    {
        public NC_daoju_use()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _daoju_id;
        private DateTime _updatetime;
        private int _type;
        private int _tudi;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 道具ID
        /// </summary>
        public int daoju_id
        {
            set { _daoju_id = value; }
            get { return _daoju_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 0为过时1为正使用
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int tudi
        {
            set { _tudi = value; }
            get { return _tudi; }
        }
        #endregion Model

    }
}

