using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_BasketBallWord 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_BasketBallWord
	{
		public tb_BasketBallWord()
		{}
		#region Model
		private int _id;
		private int _name_enid;
		private string _hometeam;
		private string _guestteam;
		private string _listcontent;
		private string _whichteam;
		private DateTime _addtime;
		private int _last;
		private string _issame;
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
		public int name_enId
		{
			set{ _name_enid=value;}
			get{return _name_enid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string hometeam
		{
			set{ _hometeam=value;}
			get{return _hometeam;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestteam
		{
			set{ _guestteam=value;}
			get{return _guestteam;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string listContent
		{
			set{ _listcontent=value;}
			get{return _listcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string whichTeam
		{
			set{ _whichteam=value;}
			get{return _whichteam;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int last
		{
			set{ _last=value;}
			get{return _last;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string isSame
		{
			set{ _issame=value;}
			get{return _issame;}
		}
		#endregion Model

	}
}

