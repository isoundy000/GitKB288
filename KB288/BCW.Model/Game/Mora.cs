using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Mora 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Mora
	{
		public Mora()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private int _truetype;
		private int _choosetype;
		private DateTime _stoptime;
		private int _usid;
		private string _usname;
		private int _reid;
		private string _rename;
		private long _price;
		private int _bztype;
		private DateTime _addtime;
		private DateTime _retime;
		private int _state;
	/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型(0公开对战/1私人对战)
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 吹牛内容
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 正确答案(1/2/3)
		/// </summary>
		public int TrueType
		{
			set{ _truetype=value;}
			get{return _truetype;}
		}
		/// <summary>
		/// 提交答案(1/2/3)
		/// </summary>
		public int ChooseType
		{
			set{ _choosetype=value;}
			get{return _choosetype;}
		}
		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime StopTime
		{
			set{ _stoptime=value;}
			get{return _stoptime;}
		}
		/// <summary>
		/// 发起ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 发起用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 挑战ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// 挑战用户昵称
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// 币值
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 币种
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// 发起时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 挑战时间
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

