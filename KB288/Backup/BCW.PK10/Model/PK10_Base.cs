using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BCW.PK10.Model
{
    public class PK10_Base
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
        /// 当前销售日期
        /// </summary>		
        private DateTime _currentsaledate;
        public DateTime CurrentSaleDate
        {
            get { return _currentsaledate; }
            set { _currentsaledate = value; }
        }
        /// <summary>
        /// GetOpenDataNo
        /// </summary>		
        private string _getopendatano;
        public string GetOpenDataNo
        {
            get { return _getopendatano; }
            set { _getopendatano = value; }
        }
        /// <summary>
        /// 读取开奖的开始时间
        /// </summary>		
        private DateTime _getopendatabegintime;
        public DateTime GetOpenDataBeginTime
        {
            get { return _getopendatabegintime; }
            set { _getopendatabegintime = value; }
        }
        /// <summary>
        /// 读取开奖的截止时间
        /// </summary>		
        private DateTime _getopendataendtime;
        public DateTime GetOpenDataEndTime
        {
            get { return _getopendataendtime; }
            set { _getopendataendtime = value; }
        }

    }
}
