using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_ZQChact 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_ZQChact
	{
		public tb_ZQChact()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private int? _tofootid;
		private string _textcontent;
		private int? _tousid;
		private int? _ishit;
		private DateTime? _addtime;
		private string _ident;
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
		public int? toFootID
		{
			set{ _tofootid=value;}
			get{return _tofootid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TextContent
		{
			set{ _textcontent=value;}
			get{return _textcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? toUsId
		{
			set{ _tousid=value;}
			get{return _tousid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isHit
		{
			set{ _ishit=value;}
			get{return _ishit;}
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
		public string ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		#endregion Model

	}
}

