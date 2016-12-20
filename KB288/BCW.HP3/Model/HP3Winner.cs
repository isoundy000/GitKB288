using System;
namespace BCW.HP3.Model
{
	/// <summary>
	/// HP3Winner:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class HP3Winner
	{
		public HP3Winner()
		{}
		#region Model
		private int _id;
		private int _winuserid;
        private string _windate;
        private long _winmoney;
        private int _winbool;
        private int _winzhu = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int WinUserID
		{
			set{ _winuserid=value;}
			get{return _winuserid;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string WinDate
        {
            set { _windate = value; }
            get { return _windate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long WinMoney
        {
            set { _winmoney = value; }
            get { return _winmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int WinBool
        {
            set { _winbool = value; }
            get { return _winbool; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int WinZhu
        {
            set { _winzhu = value; }
            get { return _winzhu; }
        }
        #endregion Model

    }
}

