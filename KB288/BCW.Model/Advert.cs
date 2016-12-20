using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Advert 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Advert
	{
		public Advert()
		{}
		#region Model
		private int _id;
		private string _title;
		private string _adurl;
		private DateTime _starttime;
		private DateTime _overtime;
		private int _status;
		private int _igold;
		private int _click;
		private DateTime _clicktime;
		private string _clickid;
		private int _adtype;
		private int _urltype;
		private DateTime _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 广告标题识别用
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 广告支持UBB和WML
		/// </summary>
		public string AdUrl
		{
			set{ _adurl=value;}
			get{return _adurl;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 正常与暂停
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 打工得币
		/// </summary>
		public int iGold
		{
			set{ _igold=value;}
			get{return _igold;}
		}
		/// <summary>
		/// 点击
		/// </summary>
		public int Click
		{
			set{ _click=value;}
			get{return _click;}
		}
		/// <summary>
		/// 点击时间
		/// </summary>
		public DateTime ClickTime
		{
			set{ _clicktime=value;}
			get{return _clicktime;}
		}
		/// <summary>
		/// 点击ID
		/// </summary>
		public string ClickID
		{
			set{ _clickid=value;}
			get{return _clickid;}
		}
		/// <summary>
		/// 广告送币性质
		/// </summary>
		public int adType
		{
			set{ _adtype=value;}
			get{return _adtype;}
		}
		/// <summary>
		/// 广告地址类型(0地址/1UBB/2WML)
		/// </summary>
		public int UrlType
		{
			set{ _urltype=value;}
			get{return _urltype;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

