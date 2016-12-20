using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_ZQCollection 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_ZQCollection
	{
		public tb_ZQCollection()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private int? _footballid;
		private int? _bianhao;
		private string _team1;
		private string _team2;
		private string _result;
		private int? _sendcount;
		private DateTime? _addtime;
		private int? _ident;
		private string _remark;
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
		public int? UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FootBallId
		{
			set{ _footballid=value;}
			get{return _footballid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Bianhao
		{
			set{ _bianhao=value;}
			get{return _bianhao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string team1
		{
			set{ _team1=value;}
			get{return _team1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string team2
		{
			set{ _team2=value;}
			get{return _team2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? sendCount
		{
			set{ _sendcount=value;}
			get{return _sendcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

