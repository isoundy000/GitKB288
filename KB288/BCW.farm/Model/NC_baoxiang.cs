using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_baoxiang ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_baoxiang
    {
        public NC_baoxiang()
        { }
        #region Model
        private int _id;
        private string _prize;
        private string _picture;
        private int _daoju_id;
        private int _type;
        private int _aa;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }

        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }

        /// <summary>
        /// ��Ʒ
        /// </summary>
        public string prize
        {
            set { _prize = value; }
            get { return _prize; }
        }
        /// <summary>
        /// ͼƬ
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int daoju_id
        {
            set { _daoju_id = value; }
            get { return _daoju_id; }
        }
        /// <summary>
        /// 1����2����
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

