using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_task 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_task
    {
        public NC_task()
        { }
        #region Model
        private int _id;
        private string _task_name;
        private string _task_contact;
        private int _task_id;
        private int _task_num;
        private int _task_grade;
        private string _task_jiangli;
        private DateTime _task_time;
        private int _task_type;
        private int _xiajia;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string task_name
        {
            set { _task_name = value; }
            get { return _task_name; }
        }
        /// <summary>
        /// 任务内容
        /// </summary>
        public string task_contact
        {
            set { _task_contact = value; }
            get { return _task_contact; }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int task_id
        {
            set { _task_id = value; }
            get { return _task_id; }
        }
        /// <summary>
        /// 完成任务数量
        /// </summary>
        public int task_num
        {
            set { _task_num = value; }
            get { return _task_num; }
        }
        /// <summary>
        /// 任务等级
        /// </summary>
        public int task_grade
        {
            set { _task_grade = value; }
            get { return _task_grade; }
        }
        /// <summary>
        /// 任务奖励
        /// </summary>
        public string task_jiangli
        {
            set { _task_jiangli = value; }
            get { return _task_jiangli; }
        }
        /// <summary>
        /// 任务完成最后时间
        /// </summary>
        public DateTime task_time
        {
            set { _task_time = value; }
            get { return _task_time; }
        }
        /// <summary>
        /// 任务类型(0日常1主线2活动)
        /// </summary>
        public int task_type
        {
            set { _task_type = value; }
            get { return _task_type; }
        }
        /// <summary>
        /// 0为未下架1为下架
        /// </summary>
        public int xiajia
        {
            set { _xiajia = value; }
            get { return _xiajia; }
        }
        #endregion Model

    }
}

