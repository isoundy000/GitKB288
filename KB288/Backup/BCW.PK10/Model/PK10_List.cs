using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace BCW.PK10.Model
{
    //tb_PK10_List
    /// <summary>
    /// 开奖号码/期号列表
    /// </summary>
    public class PK10_List
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
        /// Date
        /// </summary>		
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        /// <summary>
        /// 期号
        /// </summary>		
        private string _no;
        public string No
        {
            get { return _no; }
            set { _no = value; }
        }

        /// <summary>
        /// 开奖号码
        /// </summary>		
        ///
        private string _nums;
        public string Nums
        {
            get { return _nums; }
            set { _nums = value; }
        }
        /// <summary>
        /// 开始下注时间
        /// </summary>		
        private DateTime _begintime;
        public DateTime BeginTime
        {
            get { return _begintime; }
            set { _begintime = value; }
        }
        /// <summary>
        /// 截至下注时间
        /// </summary>		
        private DateTime _endtime;
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        /// <summary>
        /// 本期失效日期（过了兑奖日期）
        /// </summary>		
        private DateTime _validdate;
        public DateTime ValidDate
        {
            get { return _validdate; }
            set { _validdate = value; }
        }
        /// <summary>
        /// 本期已经失效（过了兑奖日期）
        /// </summary>		
        private int _validflag;
        public int ValidFlag
        {
            get { return _validflag; }
            set { _validflag = value; }
        }
        /// <summary>
        /// 已开奖标志
        /// </summary>		
        private decimal _openflag;
        public decimal OpenFlag
        {
            get { return _openflag; }
            set { _openflag = value; }
        }
        /// <summary>
        /// 已经计算中奖情况的标志
        /// </summary>		
        private decimal _calcflag;
        public decimal CalcFlag
        {
            get { return _calcflag; }
            set { _calcflag = value; }
        }
        /// <summary>
        /// 本期总付款金额
        /// </summary>		
        private int _paymoney;
        public int PayMoney
        {
            get { return _paymoney; }
            set { _paymoney = value; }
        }
        /// <summary>
        /// 本期总中奖金额
        /// </summary>		
        private int _winmoney;
        public int WinMoney
        {
            get { return _winmoney; }
            set { _winmoney = value; }
        }
        /// <summary>
        /// 本期总兑奖金额
        /// </summary>		
        private int _casemoney;
        public int CaseMoney
        {
            get { return _casemoney; }
            set { _casemoney = value; }
        }
        /// <summary>
        /// 本期下注单数
        /// </summary>		
        private int _paycount;
        public int PayCount
        {
            get { return _paycount; }
            set { _paycount = value; }
        }
        /// <summary>
        /// 本期赢注单数
        /// </summary>		
        private int _wincount;
        public int WinCount
        {
            get { return _wincount; }
            set { _wincount = value; }
        }
        /// <summary>
        /// 本期已兑奖单数
        /// </summary>		
        private int _casecount;
        public int CaseCount
        {
            get { return _casecount; }
            set { _casecount = value; }
        }
    }
}

