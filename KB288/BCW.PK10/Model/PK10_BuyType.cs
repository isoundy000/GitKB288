using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace BCW.PK10.Model
{
    //tb_PK10_BuyType
    public class PK10_BuyType
    {

        /// <summary>
        /// 投注类型ID
        /// </summary>		
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 投注类型ParentID
        /// </summary>		
        private int _parentid;
        public int ParentID
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        /// <summary>
        /// 类型(1号码，2大小，3单双，4龙虎，5任选，6和值）
        /// </summary>		
        private int _type;
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 顺序号
        /// </summary>		
        private int _no;
        public int No
        {
            get { return _no; }
            set { _no = value; }
        }
        /// <summary>
        /// 投注类型名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 投注类型名称
        /// </summary>		
        private string _descript;
        public string Descript
        {
            get { return _descript; }
            set { _descript = value; }
        }
        /// <summary>
        /// 第几位数
        /// </summary>		
        private int _numid;
        public int NumID
        {
            get { return _numid; }
            set { _numid = value; }
        }
        /// <summary>
        /// 单注的号码个数
        /// </summary>		
        private int _numscount;
        public int NumsCount
        {
            get { return _numscount; }
            set { _numscount = value; }
        }
        /// <summary>
        /// 可以复选的标志
        /// </summary>		
        private decimal  _multiselect;
        public decimal MultiSelect
        {
            get { return _multiselect; }
            set { _multiselect = value; }
        }
        /// <summary>
        /// 匹配0个号码的奖金
        /// </summary>		
        private decimal  _d0;
        public decimal d0
        {
            get { return _d0; }
            set { _d0 = value; }
        }
        /// <summary>
        /// 匹配1个号码的奖金
        /// </summary>		
        private decimal _d1;
        public decimal d1
        {
            get { return _d1; }
            set { _d1 = value; }
        }
        /// <summary>
        /// 匹配2个号码的奖金
        /// </summary>		
        private decimal _d2;
        public decimal d2
        {
            get { return _d2; }
            set { _d2 = value; }
        }
        /// <summary>
        /// 匹配3个号码的奖金
        /// </summary>		
        private decimal _d3;
        public decimal d3
        {
            get { return _d3; }
            set { _d3 = value; }
        }
        /// <summary>
        /// 匹配4个号码的奖金
        /// </summary>		
        private decimal _d4;
        public decimal d4
        {
            get { return _d4; }
            set { _d4 = value; }
        }
        /// <summary>
        /// 匹配5个号码的奖金
        /// </summary>		
        private decimal _d5;
        public decimal d5
        {
            get { return _d5; }
            set { _d5 = value; }
        }
        /// <summary>
        /// 匹配6个号码的奖金
        /// </summary>		
        private decimal _d6;
        public decimal d6
        {
            get { return _d6; }
            set { _d6 = value; }
        }
        /// <summary>
        /// 匹配7个号码的奖金
        /// </summary>		
        private decimal _d7;
        public decimal d7
        {
            get { return _d7; }
            set { _d7 = value; }
        }
        /// <summary>
        /// 匹配8个号码的奖金
        /// </summary>		
        private decimal _d8;
        public decimal d8
        {
            get { return _d8; }
            set { _d8 = value; }
        }
        /// <summary>
        /// 匹配9个号码的奖金
        /// </summary>		
        private decimal _d9;
        public decimal d9
        {
            get { return _d9; }
            set { _d9 = value; }
        }
        /// <summary>
        /// 匹配10个号码的奖金
        /// </summary>		
        private decimal _d10;
        public decimal d10
        {
            get { return _d10; }
            set { _d10 = value; }
        }
        /// <summary>
        /// 下注的受限总额( 如果是对冲型的，则表示浮动额度)
        /// </summary>		
        private int _payimit;
        public int PayLimit
        {
            get { return _payimit; }
            set { _payimit = value; }
        }
        /// <summary>
        /// 0表示一般类型,1表示可对冲类型(如大小单双)
        /// </summary>		
        private decimal _paylimittype;
        public decimal PayLimitType
        {
            get { return _paylimittype; }
            set { _paylimittype = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private decimal _RateFlag;
        public decimal RateFlag
        {
            get { return _RateFlag; }
            set { _RateFlag = value; }
        }
        private decimal _RateBeginStep;
        public decimal RateBeginStep
        {
            get { return _RateBeginStep; }
            set { _RateBeginStep = value; }
        }
        private decimal _RateStepChange;
        public decimal RateStepChange
        {
            get { return _RateStepChange; }
            set { _RateStepChange = value; }
        }
        private decimal _RateMin;
        public decimal RateMin
        {
            get { return _RateMin; }
            set { _RateMin = value; }
        }
        private decimal _RateMax;
        public decimal RateMax
        {
            get { return _RateMax; }
            set { _RateMax = value; }
        }
        private decimal _norobot;
        public decimal NoRobot
        {
            get { return _norobot; }
            set { _norobot = value; }
        }
        /// <summary>
        /// 每用户下注的受限总额
        /// </summary>		
        private int _payimitUser;
        public int PayLimitUser
        {
            get { return _payimitUser; }
            set { _payimitUser = value; }
        }
    }
}

