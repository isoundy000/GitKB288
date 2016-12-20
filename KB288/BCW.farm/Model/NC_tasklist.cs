using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_tasklist 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_tasklist
    {
        public NC_tasklist()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _task_id;
        private int _task_oknum;
        private DateTime _task_time;
        private DateTime _task_oktime;
        private int _task_type;
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
        /// 
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int task_id
        {
            set { _task_id = value; }
            get { return _task_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int task_oknum
        {
            set { _task_oknum = value; }
            get { return _task_oknum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime task_time
        {
            set { _task_time = value; }
            get { return _task_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime task_oktime
        {
            set { _task_oktime = value; }
            get { return _task_oktime; }
        }
        /// <summary>
        /// 0日常1主线2活动
        /// </summary>
        public int task_type
        {
            set { _task_type = value; }
            get { return _task_type; }
        }
        /// <summary>
        /// 0未完成1完成
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

