using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_gonggao ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_gonggao
    {
        public NC_gonggao()
        { }
        #region Model
        private int _id;
        private string _title;
        private string _contact;
        private DateTime _updatetime;
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
        public string title
        {
            set { _title = value; }
            get { return _title; }
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
        /// 
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

