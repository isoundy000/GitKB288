using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_mydaoju ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_mydaoju
    {
        public NC_mydaoju()
        { }
        #region Model
        private int _id;
        private string _name;
        private int _num;
        private int _usid;
        private int _type;
        private int _zhonglei;
        private int _name_id;
        private int _huafei_id;
        private int _suoding;
        private string _picture;
        private int _iszengsong;
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
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
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
        /// 1Ϊ����2Ϊ����
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 1��ͨ2��3��4��
        /// </summary>
        public int zhonglei
        {
            set { _zhonglei = value; }
            get { return _zhonglei; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int name_id
        {
            set { _name_id = value; }
            get { return _name_id; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int huafei_id
        {
            set { _huafei_id = value; }
            get { return _huafei_id; }
        }
        /// <summary>
        /// 0������1����
        /// </summary>
        public int suoding
        {
            set { _suoding = value; }
            get { return _suoding; }
        }
        /// <summary>
        /// ͼƬ·��
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// 0Ϊ�Լ�1Ϊ����
        /// </summary>
        public int iszengsong
        {
            set { _iszengsong = value; }
            get { return _iszengsong; }
        }
        #endregion Model

    }
}

