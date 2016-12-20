using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_gonggao 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_gonggao
    {
        public NC_gonggao()
        { }
        #region Model
        private int _id;
        private string _title;
        private string _contact;
        private DateTime _updatetime;
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
        /// 标题
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
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
        /// 
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

