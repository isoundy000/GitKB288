using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_baoxiang 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_baoxiang
    {
        public NC_baoxiang()
        { }
        #region Model
        private int _id;
        private string _prize;
        private string _picture;
        private int _daoju_id;
        private int _type;
        private int _aa;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }

        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }

        /// <summary>
        /// 奖品
        /// </summary>
        public string prize
        {
            set { _prize = value; }
            get { return _prize; }
        }
        /// <summary>
        /// 图片
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
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
        /// 1种子2道具
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

