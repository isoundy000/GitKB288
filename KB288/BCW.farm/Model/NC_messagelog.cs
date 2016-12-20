using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_messagelog ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_messagelog
    {
        public NC_messagelog()
        { }
        #region Model
        private int _id;
        private int _usid;
        private string _usname;
        private string _actext;
        private DateTime _addtime;
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
        /// �û�id
        /// </summary>
        public int UsId
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// �û���
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// ��¼
        /// </summary>
        public string AcText
        {
            set { _actext = value; }
            get { return _actext; }
        }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

