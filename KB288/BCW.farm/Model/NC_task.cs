using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_task ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// ��������
        /// </summary>
        public string task_name
        {
            set { _task_name = value; }
            get { return _task_name; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string task_contact
        {
            set { _task_contact = value; }
            get { return _task_contact; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int task_id
        {
            set { _task_id = value; }
            get { return _task_id; }
        }
        /// <summary>
        /// �����������
        /// </summary>
        public int task_num
        {
            set { _task_num = value; }
            get { return _task_num; }
        }
        /// <summary>
        /// ����ȼ�
        /// </summary>
        public int task_grade
        {
            set { _task_grade = value; }
            get { return _task_grade; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string task_jiangli
        {
            set { _task_jiangli = value; }
            get { return _task_jiangli; }
        }
        /// <summary>
        /// ����������ʱ��
        /// </summary>
        public DateTime task_time
        {
            set { _task_time = value; }
            get { return _task_time; }
        }
        /// <summary>
        /// ��������(0�ճ�1����2�)
        /// </summary>
        public int task_type
        {
            set { _task_type = value; }
            get { return _task_type; }
        }
        /// <summary>
        /// 0Ϊδ�¼�1Ϊ�¼�
        /// </summary>
        public int xiajia
        {
            set { _xiajia = value; }
            get { return _xiajia; }
        }
        #endregion Model

    }
}

