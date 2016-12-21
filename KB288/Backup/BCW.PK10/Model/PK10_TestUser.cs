using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace BCW.PK10.Model
{
    public class PK10_TestUser
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
        private int _igold;
        public int iGold
        {
            get { return _igold; }
            set { _igold = value; }
        }
        private string _settings;
        public string Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
    }
}
