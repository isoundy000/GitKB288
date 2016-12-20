using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace BCW.PK10.Model
{
    //tb_PK10_Buy
    public class PK10_Buy
    {

        /// <summary>
        /// ID
        /// </summary>		
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 期号ID
        /// </summary>		
        private int _listid;
        public int ListID
        {
            get { return _listid; }
            set { _listid = value; }
        }
        /// <summary>
        /// uID
        /// </summary>		
        private int _uid;
        public int uID
        {
            get { return _uid; }
            set { _uid = value; }
        }
        /// <summary>
        /// uName
        /// </summary>		
        private string _uname;
        public string uName
        {
            get { return _uname; }
            set { _uname = value; }
        }
        /// <summary>
        /// BuyTime
        /// </summary>		
        private DateTime _buytime;
        public DateTime BuyTime
        {
            get { return _buytime; }
            set { _buytime = value; }
        }
        /// <summary>
        /// BuyType
        /// </summary>		
        private int _buytype;
        public int BuyType
        {
            get { return _buytype; }
            set { _buytype = value; }
        }
        /// <summary>
        /// 号码组合类型（0单式1复式）
        /// </summary>		
        private decimal _numtype;
        public decimal NumType
        {
            get { return _numtype; }
            set { _numtype = value; }
        }
        /// <summary>
        /// 下注的号码组合
        /// </summary>		
        private string _nums;
        public string Nums
        {
            get { return _nums; }
            set { _nums = value; }
        }
        /// <summary>
        /// 拆分出的每一注列表
        /// </summary>		
        private string _numsdetail;
        public string NumsDetail
        {
            get { return _numsdetail; }
            set { _numsdetail = value; }
        }
        /// <summary>
        /// 下注描述
        /// </summary>		
        private string _buydescript;
        public string BuyDescript
        {
            get { return _buydescript; }
            set { _buydescript = value; }
        }
        /// <summary>
        /// 每注单价
        /// </summary>		
        private int _buyprice;
        public int BuyPrice
        {
            get { return _buyprice; }
            set { _buyprice = value; }
        }
        /// <summary>
        /// 下注注数
        /// </summary>		
        private int _buycount;
        public int BuyCount
        {
            get { return _buycount; }
            set { _buycount = value; }
        }
        /// <summary>
        /// 下注倍数
        /// </summary>		
        private int _buymulti;
        public int BuyMulti
        {
            get { return _buymulti; }
            set { _buymulti = value; }
        }
        /// <summary>
        /// 付款金额
        /// </summary>		
        private int _paymoney;
        public int PayMoney
        {
            get { return _paymoney; }
            set { _paymoney = value; }
        }
        /// <summary>
        /// 开奖号码
        /// </summary>		
        private string _listno;
        public string ListNo
        {
            get { return _listno; }
            set { _listno = value; }
        }
        /// <summary>
        /// 开奖号码
        /// </summary>		
        private string _listnums;
        public string ListNums
        {
            get { return _listnums; }
            set { _listnums = value; }
        }
        /// <summary>
        /// 中奖号码（取最大的）
        /// </summary>		
        private string _winnums;
        public string WinNums
        {
            get { return _winnums; }
            set { _winnums = value; }
        }
        /// <summary>
        /// 匹配中奖号码个数
        /// </summary>		
        private int _winnumscount;
        public int WinNumsCount
        {
            get { return _winnumscount; }
            set { _winnumscount = value; }
        }
        /// <summary>
        /// 赔率或者单注奖金
        /// </summary>		
        private decimal  _winprice;
        public decimal  WinPrice
        {
            get { return _winprice; }
            set { _winprice = value; }
        }
        /// <summary>
        /// 本单中奖总金额
        /// </summary>		
        private int _winmoney;
        public int WinMoney
        {
            get { return _winmoney; }
            set { _winmoney = value; }
        }
        /// <summary>
        /// 兑奖金额
        /// </summary>		
        private int _casemoney;
        public int CaseMoney
        {
            get { return _casemoney; }
            set { _casemoney = value; }
        }
        /// <summary>
        /// CaseFlag
        /// </summary>		
        private decimal _caseflag;
        public decimal CaseFlag
        {
            get { return _caseflag; }
            set { _caseflag = value; }
        }
        /// <summary>
        /// CaseTime
        /// </summary>		
        private DateTime _casetime;
        public DateTime CaseTime
        {
            get { return _casetime; }
            set { _casetime = value; }
        }
        /// <summary>
        /// 费用总金额
        /// </summary>		
        private int _charges;
        public int Charges
        {
            get { return _charges; }
            set { _charges = value; }
        }
        /// <summary>
        /// 测试下单标志
        /// </summary>		
        private decimal _istest;
        public decimal isTest
        {
            get { return _istest; }
            set { _istest = value; }
        }
        /// <summary>
        /// 机器人下单标志
        /// </summary>		
        private decimal _isrobot;
        public decimal isRobot
        {
            get { return _isrobot; }
            set { _isrobot = value; }
        }
        /// <summary>
        /// 有效日期
        /// </summary>		
        private DateTime _validdate;
        public DateTime ValidDate
        {
            get { return _validdate; }
            set { _validdate = value; }
        }
        /// <summary>
        /// 有效标志
        /// </summary>		
        private int _validflag;
        public int ValidFlag
        {
            get { return _validflag; }
            set { _validflag = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _rate;
        public string Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }
    }
}

