using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_market 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_market
    {
        public NC_market()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _daoju_id;
        private int _daoju_num;
        private long _daoju_price;
        private DateTime _add_time;
        private int _type;
        private string _daoju_name;
        private decimal _sui;
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
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 道具的ID
        /// </summary>
        public int daoju_id
        {
            set { _daoju_id = value; }
            get { return _daoju_id; }
        }
        /// <summary>
        /// 道具的数量
        /// </summary>
        public int daoju_num
        {
            set { _daoju_num = value; }
            get { return _daoju_num; }
        }
        /// <summary>
        /// 道具售价
        /// </summary>
        public long daoju_price
        {
            set { _daoju_price = value; }
            get { return _daoju_price; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 类型0为下架1为上架
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 道具名称
        /// </summary>
        public string daoju_name
        {
            set { _daoju_name = value; }
            get { return _daoju_name; }
        }
        /// <summary>
        /// 实时扣税
        /// </summary>
        public decimal sui
        {
            set { _sui = value; }
            get { return _sui; }
        }
        #endregion Model

    }
}

