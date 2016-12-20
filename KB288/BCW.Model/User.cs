using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����User ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class User
    {
        public User()
        { }
        #region Model
        private int _id;
        private string _mobile;
        private string _usname;
        private string _uspwd;
        private string _uspled;
        private string _usadmin;
        private string _uskey;
        private string _email;
        private string _photo;
        private int _sex;
        private DateTime _regtime;
        private string _regip;
        private string _endip;
        private DateTime _endtime;
        private DateTime _endtime2;
        private string _endpage;
        private string _sign;
        private string _city;
        private DateTime _birth;
        private long _igold;
        private long _ibank;
        private long _imoney;
        private long _iscore;
        private int _leven;
        private int _click;
        private int _ontime;
        private string _adminkey;
        private int _state;
        private string _paras;
        private int _invitenum;
        private string _copytemp;
        private string _forumset;
        private string _visithy;
        private int _gutnum;
        private string _limit;
        private int _signtotal;
        private int _signkeep;
        private DateTime _signtime;
        private int _vipgrow;
        private int _vipdaygrow;
        private DateTime _vipdate;
        private DateTime _updatedaytime;
        private int _isverify;
        private int _isfreeze;
        private string _smsemail;
        private string _usubb;
        private DateTime _timelimit;
        private int _paytype;
        private int _endChatID;
        private int _isSpier;
        /// <summary>
        /// ѡ����ͷ�ʽ 0ѡ�� 1����Ƹ� 2����
        /// </summary>
        public int PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// �״δ��͵�ʱ��
        /// </summary>
        public DateTime TimeLimit
        {
            set { _timelimit = value; }
            get { return _timelimit; }
        }
        /// <summary>
        /// ��ԱID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// �ֻ���
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// �ǳ�
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string UsPwd
        {
            set { _uspwd = value; }
            get { return _uspwd; }
        }
        /// <summary>
        /// ֧������
        /// </summary>
        public string UsPled
        {
            set { _uspled = value; }
            get { return _uspled; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string UsAdmin
        {
            set { _usadmin = value; }
            get { return _usadmin; }
        }
        /// <summary>
        /// �û���Կ
        /// </summary>
        public string UsKey
        {
            set { _uskey = value; }
            get { return _uskey; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// ͷ��
        /// </summary>
        public string Photo
        {
            set { _photo = value; }
            get { return _photo; }
        }
        /// <summary>
        /// �Ա�
        /// </summary>
        public int Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// ע��ʱ��
        /// </summary>
        public DateTime RegTime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// ע��IP
        /// </summary>
        public string RegIP
        {
            set { _regip = value; }
            get { return _regip; }
        }
        /// <summary>
        /// ���IP
        /// </summary>
        public string EndIP
        {
            set { _endip = value; }
            get { return _endip; }
        }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// �������ʱ�䣨�����Ա��
        /// </summary>
        public DateTime EndTime2
        {
            set { _endtime2 = value; }
            get { return _endtime2; }
        }
        /// <summary>
        /// ������ҳ��
        /// </summary>
        public string EndPage
        {
            set { _endpage = value; }
            get { return _endpage; }
        }
        /// <summary>
        /// ����ǩ������##�ֿ���
        /// </summary>
        public string Sign
        {
            set { _sign = value; }
            get { return _sign; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public DateTime Birth
        {
            set { _birth = value; }
            get { return _birth; }
        }
        /// <summary>
        /// �����
        /// </summary>
        public long iGold
        {
            set { _igold = value; }
            get { return _igold; }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        public long iBank
        {
            set { _ibank = value; }
            get { return _ibank; }
        }
        /// <summary>
        /// ��ֵ��
        /// </summary>
        public long iMoney
        {
            set { _imoney = value; }
            get { return _imoney; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public long iScore
        {
            set { _iscore = value; }
            get { return _iscore; }
        }
        /// <summary>
        /// �ȼ�
        /// </summary>
        public int Leven
        {
            set { _leven = value; }
            get { return _leven; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// ����ʱ�䣨���ӣ�
        /// </summary>
        public int OnTime
        {
            set { _ontime = value; }
            get { return _ontime; }
        }
        /// <summary>
        /// ��ɫȨ��KEY
        /// </summary>
        public string AdminKey
        {
            set { _adminkey = value; }
            get { return _adminkey; }
        }
        /// <summary>
        /// ״̬��0����/1����
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// ���ԣ�����#����#����#�ǻ�#����#а��
        /// </summary>
        public string Paras
        {
            set { _paras = value; }
            get { return _paras; }
        }
        /// <summary>
        /// �Ƽ���ID
        /// </summary>
        public int InviteNum
        {
            set { _invitenum = value; }
            get { return _invitenum; }
        }
        /// <summary>
        /// ������ʷ
        /// </summary>
        public string CopyTemp
        {
            set { _copytemp = value; }
            get { return _copytemp; }
        }
        /// <summary>
        /// ��̳��������
        /// </summary>
        public string ForumSet
        {
            set { _forumset = value; }
            get { return _forumset; }
        }
        /// <summary>
        /// �����㼣(##�ֿ�)
        /// </summary>
        public string VisitHy
        {
            set { _visithy = value; }
            get { return _visithy; }
        }
        /// <summary>
        /// ����Ϣ����
        /// </summary>
        public int GutNum
        {
            set { _gutnum = value; }
            get { return _gutnum; }
        }
        /// <summary>
        /// ��̳���Ա�־
        /// </summary>
        public string Limit
        {
            set { _limit = value; }
            get { return _limit; }
        }
        /// <summary>
        /// ǩ��������
        /// </summary>
        public int SignTotal
        {
            set { _signtotal = value; }
            get { return _signtotal; }
        }
        /// <summary>
        /// ǩ����������
        /// </summary>
        public int SignKeep
        {
            set { _signkeep = value; }
            get { return _signkeep; }
        }
        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        public DateTime SignTime
        {
            set { _signtime = value; }
            get { return _signtime; }
        }
        /// <summary>
        /// VIP�ɳ�ֵ
        /// </summary>
        public int VipGrow
        {
            set { _vipgrow = value; }
            get { return _vipgrow; }
        }
        /// <summary>
        /// VIPÿ��ɳ���
        /// </summary>
        public int VipDayGrow
        {
            set { _vipdaygrow = value; }
            get { return _vipdaygrow; }
        }
        /// <summary>
        /// VIP����ʱ��
        /// </summary>
        public DateTime VipDate
        {
            set { _vipdate = value; }
            get { return _vipdate; }
        }
        /// <summary>
        /// ÿ�����ʱ��
        /// </summary>
        public DateTime UpdateDayTime
        {
            set { _updatedaytime = value; }
            get { return _updatedaytime; }
        }
        /// <summary>
        /// �û��Ƿ�����֤(0δ��֤/1����֤/2δͨ��������֤��Ĭ��1����ڣ����ں��Զ�ɾ��)
        /// </summary>
        public int IsVerify
        {
            set { _isverify = value; }
            get { return _isverify; }
        }
        /// <summary>
        /// �û��ʻ��Ƿ񱻶���(��ֻ�ܽ������ܳ�)
        /// </summary>
        public int IsFreeze
        {
            set { _isfreeze = value; }
            get { return _isfreeze; }
        }
        /// <summary>
        /// 139��������
        /// </summary>
        public string SmsEmail
        {
            set { _smsemail = value; }
            get { return _smsemail; }
        }
        /// <summary>
        /// ����UBB���
        /// </summary>
        public string UsUbb
        {
            set { _usubb = value; }
            get { return _usubb; }
        }
        /// <summary>
        /// �İ����߷���ID
        /// </summary>
        public int EndChatID
        {
            set { _endChatID = value; }
            get { return _endChatID; }
        }
        /// <summary>
        /// 0��Ա1ϵͳ��
        /// </summary>
        public int IsSpier
        {
            set { _isSpier = value; }
            get { return _isSpier; }
        }
        #endregion Model

    }
}

