using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_hecheng 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_hecheng
    {
        public NC_hecheng()
        { }
        #region Model
        private int _id;
        private string _title;
        private int _giftid;
        private string _prevpic;
        private int _usid;
        private int _num;
        private DateTime _addtime;
        private int _all_num;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 合成名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 合成前ID
        /// </summary>
        public int GiftId
        {
            set { _giftid = value; }
            get { return _giftid; }
        }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string PrevPic
        {
            set { _prevpic = value; }
            get { return _prevpic; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// 合成时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 总数量
        /// </summary>
        public int all_num
        {
            set { _all_num = value; }
            get { return _all_num; }
        }
        #endregion Model

    }
}

