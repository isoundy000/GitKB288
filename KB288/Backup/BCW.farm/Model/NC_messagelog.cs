using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_messagelog 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_messagelog
    {
        public NC_messagelog()
        { }
        #region Model
        private int _id;
        private int _usid;
        private string _usname;
        private string _actext;
        private DateTime _addtime;
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
        /// 用户id
        /// </summary>
        public int UsId
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// 记录
        /// </summary>
        public string AcText
        {
            set { _actext = value; }
            get { return _actext; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

