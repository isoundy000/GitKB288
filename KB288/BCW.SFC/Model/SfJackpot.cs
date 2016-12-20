using System;
namespace BCW.SFC.Model
{
	/// <summary>
	/// 实体类SfJackpot 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SfJackpot
	{
		public SfJackpot()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _prize;
		private long _winprize;
		private string _other;
        private long _allmoney;
        private DateTime _AddTime;
        private int _CID;
        /// <summary>
        /// 
        /// </summary>
        public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int usID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long Prize
		{
			set{ _prize=value;}
			get{return _prize;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long WinPrize
		{
			set{ _winprize=value;}
			get{return _winprize;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string other
		{
			set{ _other=value;}
			get{return _other;}
		}
        /// <summary>
        /// 
        /// </summary>
        public long allmoney
        {
            set { _allmoney = value; }
            get { return _allmoney; }
        }
        public DateTime AddTime
        {
            set { _AddTime = value; }
            get { return _AddTime; }
        }
        public int CID
        {
            set { _CID = value; }
            get { return _CID; }
        }
        #endregion Model

    }
}

