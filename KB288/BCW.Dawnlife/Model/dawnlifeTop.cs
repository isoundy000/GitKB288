using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����dawnlifeTop ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class dawnlifeTop
	{
		public dawnlifeTop()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private DateTime? _date;
		private string _city;
		private string _usname;
		private long _coin;
		private long _money;
		private int? _sum;
        private int? _cum;
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
		public int? UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long coin
		{
			set{ _coin=value;}
			get{return _coin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long money
		{
			set{ _money=value;}
			get{return _money;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? sum
		{
			set{ _sum=value;}
			get{return _sum;}
		}
		#endregion Model
        /// <summary>
		/// 
		/// </summary>
		public int? cum
		{
			set{ _cum=value;}
			get{return _cum;}
		}
	
	}
}

