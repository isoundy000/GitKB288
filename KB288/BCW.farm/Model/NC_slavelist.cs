using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_slavelist 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_slavelist
    {
        public NC_slavelist()
        { }
        #region Model
        private int _id;
        private string _contact;
        private int _ingold;
        private int _outgold;
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
        /// 内容
        /// </summary>
        public string contact
        {
            set { _contact = value; }
            get { return _contact; }
        }
        /// <summary>
        /// 添加金币
        /// </summary>
        public int inGold
        {
            set { _ingold = value; }
            get { return _ingold; }
        }
        /// <summary>
        /// 减去金币
        /// </summary>
        public int outGold
        {
            set { _outgold = value; }
            get { return _outgold; }
        }
        /// <summary>
        /// 0惩罚1安抚
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

