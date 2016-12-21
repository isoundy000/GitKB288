using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类User 。(属性说明自动提取数据库字段的描述信息)
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
        /// 选择打赏方式 0选择 1自身财富 2基金
        /// </summary>
        public int PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// 首次打赏的时间
        /// </summary>
        public DateTime TimeLimit
        {
            set { _timelimit = value; }
            get { return _timelimit; }
        }
        /// <summary>
        /// 会员ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string UsPwd
        {
            set { _uspwd = value; }
            get { return _uspwd; }
        }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string UsPled
        {
            set { _uspled = value; }
            get { return _uspled; }
        }
        /// <summary>
        /// 管理密码
        /// </summary>
        public string UsAdmin
        {
            set { _usadmin = value; }
            get { return _usadmin; }
        }
        /// <summary>
        /// 用户密钥
        /// </summary>
        public string UsKey
        {
            set { _uskey = value; }
            get { return _uskey; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo
        {
            set { _photo = value; }
            get { return _photo; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegIP
        {
            set { _regip = value; }
            get { return _regip; }
        }
        /// <summary>
        /// 最后IP
        /// </summary>
        public string EndIP
        {
            set { _endip = value; }
            get { return _endip; }
        }
        /// <summary>
        /// 最后在线时间
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 最后在线时间（隐身会员）
        /// </summary>
        public DateTime EndTime2
        {
            set { _endtime2 = value; }
            get { return _endtime2; }
        }
        /// <summary>
        /// 最后浏览页面
        /// </summary>
        public string EndPage
        {
            set { _endpage = value; }
            get { return _endpage; }
        }
        /// <summary>
        /// 个性签名（用##分开）
        /// </summary>
        public string Sign
        {
            set { _sign = value; }
            get { return _sign; }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birth
        {
            set { _birth = value; }
            get { return _birth; }
        }
        /// <summary>
        /// 虚拟币
        /// </summary>
        public long iGold
        {
            set { _igold = value; }
            get { return _igold; }
        }
        /// <summary>
        /// 银行币
        /// </summary>
        public long iBank
        {
            set { _ibank = value; }
            get { return _ibank; }
        }
        /// <summary>
        /// 充值币
        /// </summary>
        public long iMoney
        {
            set { _imoney = value; }
            get { return _imoney; }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public long iScore
        {
            set { _iscore = value; }
            get { return _iscore; }
        }
        /// <summary>
        /// 等级
        /// </summary>
        public int Leven
        {
            set { _leven = value; }
            get { return _leven; }
        }
        /// <summary>
        /// 人气
        /// </summary>
        public int Click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// 在线时间（分钟）
        /// </summary>
        public int OnTime
        {
            set { _ontime = value; }
            get { return _ontime; }
        }
        /// <summary>
        /// 角色权限KEY
        /// </summary>
        public string AdminKey
        {
            set { _adminkey = value; }
            get { return _adminkey; }
        }
        /// <summary>
        /// 状态（0正常/1隐身）
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 属性（人气#体力#魅力#智慧#威望#邪恶）
        /// </summary>
        public string Paras
        {
            set { _paras = value; }
            get { return _paras; }
        }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public int InviteNum
        {
            set { _invitenum = value; }
            get { return _invitenum; }
        }
        /// <summary>
        /// 复制历史
        /// </summary>
        public string CopyTemp
        {
            set { _copytemp = value; }
            get { return _copytemp; }
        }
        /// <summary>
        /// 论坛参数设置
        /// </summary>
        public string ForumSet
        {
            set { _forumset = value; }
            get { return _forumset; }
        }
        /// <summary>
        /// 访问足迹(##分开)
        /// </summary>
        public string VisitHy
        {
            set { _visithy = value; }
            get { return _visithy; }
        }
        /// <summary>
        /// 新消息条数
        /// </summary>
        public int GutNum
        {
            set { _gutnum = value; }
            get { return _gutnum; }
        }
        /// <summary>
        /// 论坛个性标志
        /// </summary>
        public string Limit
        {
            set { _limit = value; }
            get { return _limit; }
        }
        /// <summary>
        /// 签到总天数
        /// </summary>
        public int SignTotal
        {
            set { _signtotal = value; }
            get { return _signtotal; }
        }
        /// <summary>
        /// 签到连续天数
        /// </summary>
        public int SignKeep
        {
            set { _signkeep = value; }
            get { return _signkeep; }
        }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SignTime
        {
            set { _signtime = value; }
            get { return _signtime; }
        }
        /// <summary>
        /// VIP成长值
        /// </summary>
        public int VipGrow
        {
            set { _vipgrow = value; }
            get { return _vipgrow; }
        }
        /// <summary>
        /// VIP每天成长点
        /// </summary>
        public int VipDayGrow
        {
            set { _vipdaygrow = value; }
            get { return _vipdaygrow; }
        }
        /// <summary>
        /// VIP过期时间
        /// </summary>
        public DateTime VipDate
        {
            set { _vipdate = value; }
            get { return _vipdate; }
        }
        /// <summary>
        /// 每天更新时间
        /// </summary>
        public DateTime UpdateDayTime
        {
            set { _updatedaytime = value; }
            get { return _updatedaytime; }
        }
        /// <summary>
        /// 用户是否已验证(0未验证/1已验证/2未通过邮箱验证，默认1天过期，过期后自动删除)
        /// </summary>
        public int IsVerify
        {
            set { _isverify = value; }
            get { return _isverify; }
        }
        /// <summary>
        /// 用户帐户是否被冻结(币只能进，不能出)
        /// </summary>
        public int IsFreeze
        {
            set { _isfreeze = value; }
            get { return _isfreeze; }
        }
        /// <summary>
        /// 139提醒邮箱
        /// </summary>
        public string SmsEmail
        {
            set { _smsemail = value; }
            get { return _smsemail; }
        }
        /// <summary>
        /// 社区UBB身份
        /// </summary>
        public string UsUbb
        {
            set { _usubb = value; }
            get { return _usubb; }
        }
        /// <summary>
        /// 聊吧在线房间ID
        /// </summary>
        public int EndChatID
        {
            set { _endChatID = value; }
            get { return _endChatID; }
        }
        /// <summary>
        /// 0会员1系统号
        /// </summary>
        public int IsSpier
        {
            set { _isSpier = value; }
            get { return _isSpier; }
        }
        #endregion Model

    }
}

