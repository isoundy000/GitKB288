using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_daoju 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_daoju
    {
        public NC_daoju()
        { }
        #region Model
        private int _id;
        private string _name;
        private long _price;
        private string _note;
        private string _picture;
        private int _time;
        private int _type;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 道具名称
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 道具价格
        /// </summary>
        public long price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 道具说明
        /// </summary>
        public string note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 化肥图片路径
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// 施肥减少时间
        /// </summary>
        public int time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 0一次1多次施肥10宝箱
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

