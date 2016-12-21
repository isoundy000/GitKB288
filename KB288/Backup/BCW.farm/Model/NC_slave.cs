using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_slave 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_slave
    {
        public NC_slave()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _slave_id;
        private int _punish;
        private int _pacify;
        private DateTime _updatetime;
        private int _tpye;
        private int _num;
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
        /// 奴隶ID
        /// </summary>
        public int slave_id
        {
            set { _slave_id = value; }
            get { return _slave_id; }
        }
        /// <summary>
        /// 惩罚
        /// </summary>
        public int punish
        {
            set { _punish = value; }
            get { return _punish; }
        }
        /// <summary>
        /// 安抚
        /// </summary>
        public int pacify
        {
            set { _pacify = value; }
            get { return _pacify; }
        }
        /// <summary>
        /// 抓捕时间
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 0为解除1为奴隶
        /// </summary>
        public int tpye
        {
            set { _tpye = value; }
            get { return _tpye; }
        }
        /// <summary>
        /// 奴隶次数
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
        }
        #endregion Model

    }
}

