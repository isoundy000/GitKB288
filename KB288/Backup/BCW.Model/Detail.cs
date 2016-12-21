using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Detail ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Detail
    {
        public Detail()
        { }
        #region Model
        private int _id;
        private int _nodeid;
        private int _types;
        private bool _isad;
        private string _title;
        private string _keyword;
        private string _model;
        private string _pics;
        private string _cover;
        private string _content;
        private string _tartext;
        private string _lantext;
        private string _safetext;
        private string _lytext;
        private string _uptext;
        private int _isvisa;
        private string _restats;
        private string _relastip;
        private int _readcount;
        private int _recount;
        private int _bztype;
        private int _cent;
        private string _payid;
        private DateTime _addtime;
        private int _usid;
        private int _hidden;
        private string _clickid;
        /// <summary>
        /// ϸ��ID
        /// </summary>  
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        ///�����id 
        /// </summary>
        public string ClickID
        {
            set { _clickid = value; }
            get { return _clickid; }
        }
        /// <summary>
        /// ר��ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
        /// <summary>
        /// ר������
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        public bool IsAd
        {
            set { _isad = value; }
            get { return _isad; }
        }
        /// <summary>
        /// ϸ������
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// ϸ�ڹؼ���
        /// </summary>
        public string KeyWord
        {
            set { _keyword = value; }
            get { return _keyword; }
        }
        /// <summary>
        /// ���û���
        /// </summary>
        public string Model
        {
            set { _model = value; }
            get { return _model; }
        }
        /// <summary>
        /// ϸ�ڽ�ͼ
        /// </summary>
        public string Pics
        {
            set { _pics = value; }
            get { return _pics; }
        }
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        public string Cover
        {
            set { _cover = value; }
            get { return _cover; }
        }
        /// <summary>
        /// ϸ������
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// �ʷ�˵��
        /// </summary>
        public string TarText
        {
            set { _tartext = value; }
            get { return _tartext; }
        }
        /// <summary>
        /// ����˵��
        /// </summary>
        public string LanText
        {
            set { _lantext = value; }
            get { return _lantext; }
        }
        /// <summary>
        /// ���˵��
        /// </summary>
        public string SafeText
        {
            set { _safetext = value; }
            get { return _safetext; }
        }
        /// <summary>
        /// ��Դ˵��
        /// </summary>
        public string LyText
        {
            set { _lytext = value; }
            get { return _lytext; }
        }
        /// <summary>
        /// ����˵��
        /// </summary>
        public string UpText
        {
            set { _uptext = value; }
            get { return _uptext; }
        }
        /// <summary>
        /// �Ƿ���ǩ֤(1|δ֪|2|��Ҫ|3|����Ҫ)
        /// </summary>
        public int IsVisa
        {
            set { _isvisa = value; }
            get { return _isvisa; }
        }
        /// <summary>
        /// ϸ��ͳ��
        /// </summary>
        public string ReStats
        {
            set { _restats = value; }
            get { return _restats; }
        }
        /// <summary>
        /// ϸ��ͳ�����ip
        /// </summary>
        public string ReLastIP
        {
            set { _relastip = value; }
            get { return _relastip; }
        }
        /// <summary>
        /// �Ķ���¼��
        /// </summary>
        public int Readcount
        {
            set { _readcount = value; }
            get { return _readcount; }
        }
        /// <summary>
        /// ���ۼ�¼��
        /// </summary>
        public int Recount
        {
            set { _recount = value; }
            get { return _recount; }
        }
        /// <summary>
        /// �շѱ���
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        /// <summary>
        /// �շѱ���
        /// </summary>
        public int Cent
        {
            set { _cent = value; }
            get { return _cent; }
        }
        /// <summary>
        /// �ѹ����UID
        /// </summary>
        public string PayId
        {
            set { _payid = value; }
            get { return _payid; }
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
        /// ����û�ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public int Hidden
        {
            set { _hidden = value; }
            get { return _hidden; }
        }
        #endregion Model

    }
}

