using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_daoju ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_daoju
    {
        public NC_daoju()
        { }
        #region Model
        private int _id;
        private string _name;
        private long _price;
        private string _note;
        private string _picture;
        private int _time;
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
        /// ��������
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// ���߼۸�
        /// </summary>
        public long price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// ����˵��
        /// </summary>
        public string note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// ����ͼƬ·��
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// ʩ�ʼ���ʱ��
        /// </summary>
        public int time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 0һ��1���ʩ��10����
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

