using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_slave ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// �û�ID
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// ū��ID
        /// </summary>
        public int slave_id
        {
            set { _slave_id = value; }
            get { return _slave_id; }
        }
        /// <summary>
        /// �ͷ�
        /// </summary>
        public int punish
        {
            set { _punish = value; }
            get { return _punish; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int pacify
        {
            set { _pacify = value; }
            get { return _pacify; }
        }
        /// <summary>
        /// ץ��ʱ��
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 0Ϊ���1Ϊū��
        /// </summary>
        public int tpye
        {
            set { _tpye = value; }
            get { return _tpye; }
        }
        /// <summary>
        /// ū������
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
        }
        #endregion Model

    }
}

