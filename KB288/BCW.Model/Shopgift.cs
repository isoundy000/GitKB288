using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Shopgift ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Shopgift
    {
        public Shopgift()
        { }
        #region Model
        private int _id;
        private int _nodeid;
        private string _title;
        private string _notes;
        private string _pic;
        private string _prevpic;
        private int _bztype;
        private int _price;
        private int _paycount;
        private int _total;
        private string _para;
        private int _issex;
        private int _isvip;
        private int _isrecom;
        private DateTime _addtime;
        private string _ids;

        /// <summary>
        /// ����ID�б�
        /// </summary>
        public string IDS
        {
            set { _ids = value; }
            get { return _ids; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }
        /// <summary>
        /// �����ͼ
        /// </summary>
        public string Pic
        {
            set { _pic = value; }
            get { return _pic; }
        }
        /// <summary>
        /// ����Сͼ
        /// </summary>
        public string PrevPic
        {
            set { _prevpic = value; }
            get { return _prevpic; }
        }
        /// <summary>
        /// �������
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        /// <summary>
        /// �����
        /// </summary>
        public int Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// �ѳ�������
        /// </summary>
        public int PayCount
        {
            set { _paycount = value; }
            get { return _paycount; }
        }
        /// <summary>
        /// �ܳ�������(-1������.ǰ̨Ҳ������ʾʣ������)
        /// </summary>
        public int Total
        {
            set { _total = value; }
            get { return _total; }
        }
        /// <summary>
        /// ���Բ���������|����|����|�ǻ�|����|а��)д����:0|0|0|0|0|0��
        /// </summary>
        public string Para
        {
            set { _para = value; }
            get { return _para; }
        }
        /// <summary>
        /// �Ƿ������Ա�����(0������/1Ů��/2����)
        /// </summary>
        public int IsSex
        {
            set { _issex = value; }
            get { return _issex; }
        }
        /// <summary>
        /// �Ƿ�����VIP�ۿ�(0������/1����)
        /// </summary>
        public int IsVip
        {
            set { _isvip = value; }
            get { return _isvip; }
        }
        /// <summary>
        /// �Ƿ�Ʒ�Ƽ�(0������/1����)
        /// </summary>
        public int IsRecom
        {
            set { _isrecom = value; }
            get { return _isrecom; }
        }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        #endregion Model

    }
}

