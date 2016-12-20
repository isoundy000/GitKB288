using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace BCW.PK10.Model
{
    public class PK10_Top
    {
        /// <summary>
        /// No
        /// </summary>		
        private int _no;
        public int No
        {
            get { return _no; }
            set { _no = value; }
        }
        /// <summary>
        /// UID
        /// </summary>		
        private int _usid;
        public int UsID
        {
            get { return _usid; }
            set { _usid = value; }
        }
        /// <summary>
        /// UsName
        /// </summary>		
        private string _usname;
        public string UsName
        {
            get { return _usname; }
            set { _usname = value; }
        }
        /// <summary>
        /// 已开售标志（包括正在销售）
        /// </summary>	
        private long _igold;
        public long iGold
        {
            get { return _igold; }
            set { _igold = value; }
        }
    }
}
