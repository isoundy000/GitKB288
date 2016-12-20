using System;
namespace BCW.BQC.Model
{
	/// <summary>
	/// 实体类BQCJackpot 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BQCJackpot
	{
		public BQCJackpot()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private long _prize;
		private long _winprize;
		private string _other;
        private long _allmoney;
		private DateTime? _addtime;
		private int? _cid;
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
		public int? usID
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
        ///<summary>
        ///结余
		/// </summary>
        public long allmoney
        {
            set { _allmoney = value; }
            get { return _allmoney;}
        }
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CID
		{
			set{ _cid=value;}
			get{return _cid;}
		}
		#endregion Model

	}
}

