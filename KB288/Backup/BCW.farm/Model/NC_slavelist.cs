using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_slavelist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_slavelist
    {
        public NC_slavelist()
        { }
        #region Model
        private int _id;
        private string _contact;
        private int _ingold;
        private int _outgold;
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
        /// ����
        /// </summary>
        public string contact
        {
            set { _contact = value; }
            get { return _contact; }
        }
        /// <summary>
        /// ��ӽ��
        /// </summary>
        public int inGold
        {
            set { _ingold = value; }
            get { return _ingold; }
        }
        /// <summary>
        /// ��ȥ���
        /// </summary>
        public int outGold
        {
            set { _outgold = value; }
            get { return _outgold; }
        }
        /// <summary>
        /// 0�ͷ�1����
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

