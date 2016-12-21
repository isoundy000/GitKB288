using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_mydaoju 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_mydaoju
    {
        public NC_mydaoju()
        { }
        #region Model
        private int _id;
        private string _name;
        private int _num;
        private int _usid;
        private int _type;
        private int _zhonglei;
        private int _name_id;
        private int _huafei_id;
        private int _suoding;
        private string _picture;
        private int _iszengsong;
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
        /// 道具数量
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
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
        /// 1为种子2为肥料
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 1普通2红3黑4金
        /// </summary>
        public int zhonglei
        {
            set { _zhonglei = value; }
            get { return _zhonglei; }
        }
        /// <summary>
        /// 作物ID
        /// </summary>
        public int name_id
        {
            set { _name_id = value; }
            get { return _name_id; }
        }
        /// <summary>
        /// 化肥ID
        /// </summary>
        public int huafei_id
        {
            set { _huafei_id = value; }
            get { return _huafei_id; }
        }
        /// <summary>
        /// 0不锁定1锁定
        /// </summary>
        public int suoding
        {
            set { _suoding = value; }
            get { return _suoding; }
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// 0为自己1为赠送
        /// </summary>
        public int iszengsong
        {
            set { _iszengsong = value; }
            get { return _iszengsong; }
        }
        #endregion Model

    }
}

