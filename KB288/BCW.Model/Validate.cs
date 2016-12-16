using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����tb_Validate ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class tb_Validate
    {
        public tb_Validate()
        { }
        #region Model
        private int _id;
        private string _phone;
        private string _ip;
        private DateTime _time;
        private int _flag;
        private DateTime _codetime;
        private string _mescode;
        private int _type;
        private int _source;
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
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// IP��ַ
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// ��ȡ�ֻ���֤��ʱ��
        /// </summary>
        public DateTime Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// �ܷ��ȡ��ʾ 1�� 0����
        /// </summary>
        public int Flag
        {
            set { _flag = value; }
            get { return _flag; }
        }
        /// <summary>
        /// ��֤�����ʱ��
        /// </summary>
        public DateTime codeTime
        {
            set { _codetime = value; }
            get { return _codetime; }
        }
        /// <summary>
        /// ��֤��
        /// </summary>
        public string mesCode
        {
            set { _mescode = value; }
            get { return _mescode; }
        }
        /// <summary>
        /// ��������   1 ע�ᣬ2 �޸����� 3 �޸��ܱ� 4�޸��ֻ� 5 ��������
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        /// ��ȡ��Դ   0  PC��   1 �ֻ���
        /// </summary>
        public int source
       {
            set { _source = value; }
            get { return _source; }
        }
        #endregion Model

    }
}

