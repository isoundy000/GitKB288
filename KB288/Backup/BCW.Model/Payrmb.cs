using System;
/// <summary>
/// ����IPS֧����Ϣ
/// �ƹ��� 20160507
/// </summary>
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Payrmb ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Payrmb
    {
        public Payrmb()
        { }
        #region Model
        private int _id;
        private int _usid;
        private string _usname;
        private int _types;
        private int _cardamt;
        private string _cardnum;
        private string _cardpwd;
        private string _cardorder;
        private int _state;
        private string _addusip;
        private DateTime _addtime;
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
        /// ����ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
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
        /// ��ֵ���ͣ�1�ƶ�/2��ͨ/3���� 100��Ѹ��
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int CardAmt
        {
            set { _cardamt = value; }
            get { return _cardamt; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string CardNum
        {
            set { _cardnum = value; }
            get { return _cardnum; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string CardPwd
        {
            set { _cardpwd = value; }
            get { return _cardpwd; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string CardOrder
        {
            set { _cardorder = value; }
            get { return _cardorder; }
        }
        /// <summary>
        /// ��ֵ���
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// ��ֵIP
        /// </summary>
        public string AddUsIP
        {
            set { _addusip = value; }
            get { return _addusip; }
        }
        /// <summary>
        /// ��ֵʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }

        /// <summary>
        /// ������Ч�� ������Ч���ԡ�Сʱ������,û����������ʧЧ����
        /// </summary>
        public int BillEXP
        {
            set { _billexp = value; }
            get { return _billexp; }
        }
        #endregion Model

    }
}

