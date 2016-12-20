using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Dxdice 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Dxdice
	{
		public Dxdice()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _dxdicea;
		private string _dxdiceb;
		private DateTime _stoptime;
		private int _usid;
		private string _usname;
		private int _reid;
		private string _rename;
		private long _price;
		private int _bztype;
		private DateTime _addtime;
		private DateTime _retime;
		private int _iswin;
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
		/// 类型（0普通/1私人对战）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 掷骰第一点
		/// </summary>
		public string DxdiceA
		{
			set{ _dxdicea=value;}
			get{return _dxdicea;}
		}
		/// <summary>
		/// 掷骰第二点
		/// </summary>
		public string DxdiceB
		{
			set{ _dxdiceb=value;}
			get{return _dxdiceb;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime StopTime
		{
			set{ _stoptime=value;}
			get{return _stoptime;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 应战用户ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// 应战用户昵称
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// 挑战额
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
		/// 创建时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 应战时间
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// 庄家是否胜(1胜)
		/// </summary>
		public int IsWin
		{
			set{ _iswin=value;}
			get{return _iswin;}
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

