using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_tasklist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// 0�ճ�1����2�
        /// </summary>
        public int task_type
        {
            set { _task_type = value; }
            get { return _task_type; }
        }
        /// <summary>
        /// 0δ���1���
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

