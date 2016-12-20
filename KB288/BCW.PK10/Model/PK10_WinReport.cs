using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.PK10.Model
{
    public class PK10_WinReport
    {
        //
        private int _paycount;
        public int PayCount
        {
            get { return _paycount; }
            set { _paycount = value; }
        }
        //
        private long _paymoney;
        public long PayMoney
        {
            get { return _paymoney; }
            set { _paymoney = value; }
        }
        //
        private int _wincount;
        public int WinCount
        {
            get { return _wincount; }
            set { _wincount = value; }
        }
        //
        private long _winmoney;
        public long WinMoney
        {
            get { return _winmoney; }
            set { _winmoney = value; }
        }
        //
        private int _casecount;
        public int CaseCount
        {
            get { return _casecount; }
            set { _casecount = value; }
        }
        //
        private long _casemoney;
        public long CaseMoney
        {
            get { return _casemoney; }
            set { _casemoney = value; }
        }
        //
        private long _charges;
        public long Charges
        {
            get { return _charges; }
            set { _charges = value; }
        }
        //
    }
}
