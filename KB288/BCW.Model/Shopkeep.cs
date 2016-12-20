using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Shopkeep ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Shopkeep
    {
        public Shopkeep()
        { }
        #region Model
        private int _id;
        private int _giftid;
        private string _title;
        private string _notes;
        private string _pic;
        private string _prevpic;
        private int _usid;
        private string _usname;
        private int _total;
        private string _para;
        private int _issex;
        private DateTime _addtime;
        private int _toptotal;

        private int _state;
        private int _nodeid;
        private string _merbillno;
        private decimal _amount;
        private string _gatewaytype;
        private string _attach;
        private int _billexp;
        private string _goodsname;
        private string _iscredit;
        private string _bankcode;
        private string _producttype;

        /// <summary>
        /// ��ֵ���
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
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
        /// ��Ʒ���� 1�������� 2��ҵ����
        /// </summary>
        public string ProductType
        {
            set { _producttype = value; }
            get { return _producttype; }
        }
        /// <summary>
        /// IPSΨһ��ʶָ�������б��
        /// </summary>
        public string BankCode
        {
            set { _bankcode = value; }
            get { return _bankcode; }
        }
        /// <summary>
        /// ֱ��ѡ�� 1
        /// </summary>
        public string IsCredit
        {
            set { _iscredit = value; }
            get { return _iscredit; }
        }
        /// <summary>
        /// �̻�������Ʒ����Ʒ����
        /// </summary>
        public string GoodsName
        {
            set { _goodsname = value; }
            get { return _goodsname; }
        }

        /// <summary>
        /// �̻����ݰ� ����+��ĸ ԭ�ⷵ��
        /// </summary>
        public string Attach
        {
            set { _attach = value; }
            get { return _attach; }
        }

        /// <summary>
        /// ֧����ʽ 01#��ǿ�02#���ÿ�03#IPS�˻�֧�� Ĭ��01
        /// </summary>
        public string GatewayType
        {
            set { _gatewaytype = value; }
            get { return _gatewaytype; }
        }

        /// <summary>
        /// ������� ���� ������λС��
        /// </summary>
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }

        /// <summary>
        /// �̻������� ������ĸ������
        /// </summary>
        public string MerBillNo
        {
            set { _merbillno = value; }
            get { return _merbillno; }
        }

        /// <summary>
        /// ������Ч�� ������Ч���ԡ�Сʱ������,û����������ʧЧ����
        /// </summary>
        public int BillEXP
        {
            set { _billexp = value; }
            get { return _billexp; }
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
        public int GiftId
        {
            set { _giftid = value; }
            get { return _giftid; }
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
        /// �û�ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// �û��ǳ�
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// ������
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
        /// ���ʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// ����ĸ����������а��ã�
        /// </summary>
        public int TopTotal
        {
            set { _toptotal = value; }
            get { return _toptotal; }
        }
        #endregion Model

    }
}

