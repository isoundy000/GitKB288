using System;
namespace TPR2.Model.guess
{
	/// <summary>
	/// 实体类Tempbakbf 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Tempbakbf
	{
		public Tempbakbf()
		{}
		#region Model
		private int _id;
		private int _p_id;
		private string _name_ch;
		private string _name_en;
		private string _sclasstype;
		private string _color;
		private DateTime _matchtime;
		private string _matchstate;
		private string _remaintime;
		private string _hometeamid;
		private string _hometeam;
		private string _hometeam_f;
		private string _hometeam_e;
		private string _guestteamid;
		private string _guestteam;
		private string _guestteam_f;
		private string _guestteam_e;
		private string _homescore;
		private string _guestscore;
		private string _homeone;
		private string _guestone;
		private string _hometwo;
		private string _guesttwo;
		private string _homethree;
		private string _guestthre;
		private string _homefour;
		private string _guestfour;
		private string _addtime;
		private string _homeaddtime1;
		private string _guestaddtime1;
		private string _homeaddtime2;
		private string _guestaddtime2;
		private string _homeaddtime3;
		private string _guestaddtime3;
		private string _addtechnic;
		private string _tv;
		private string _explain;
		private string _explain2;
		private string _类型;
		private int _联赛id;
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
		public int p_id
		{
			set{ _p_id=value;}
			get{return _p_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name_ch
		{
			set{ _name_ch=value;}
			get{return _name_ch;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name_en
		{
			set{ _name_en=value;}
			get{return _name_en;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sclassType
		{
			set{ _sclasstype=value;}
			get{return _sclasstype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string color
		{
			set{ _color=value;}
			get{return _color;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime matchtime
		{
			set{ _matchtime=value;}
			get{return _matchtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string matchstate
		{
			set{ _matchstate=value;}
			get{return _matchstate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remaintime
		{
			set{ _remaintime=value;}
			get{return _remaintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string hometeamID
		{
			set{ _hometeamid=value;}
			get{return _hometeamid;}
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
		public string hometeam_F
		{
			set{ _hometeam_f=value;}
			get{return _hometeam_f;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string hometeam_E
		{
			set{ _hometeam_e=value;}
			get{return _hometeam_e;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestteamID
		{
			set{ _guestteamid=value;}
			get{return _guestteamid;}
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
		public string guestteam_F
		{
			set{ _guestteam_f=value;}
			get{return _guestteam_f;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestteam_E
		{
			set{ _guestteam_e=value;}
			get{return _guestteam_e;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string homescore
		{
			set{ _homescore=value;}
			get{return _homescore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestscore
		{
			set{ _guestscore=value;}
			get{return _guestscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string homeone
		{
			set{ _homeone=value;}
			get{return _homeone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestone
		{
			set{ _guestone=value;}
			get{return _guestone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string hometwo
		{
			set{ _hometwo=value;}
			get{return _hometwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guesttwo
		{
			set{ _guesttwo=value;}
			get{return _guesttwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string homethree
		{
			set{ _homethree=value;}
			get{return _homethree;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestthre
		{
			set{ _guestthre=value;}
			get{return _guestthre;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string homefour
		{
			set{ _homefour=value;}
			get{return _homefour;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestfour
		{
			set{ _guestfour=value;}
			get{return _guestfour;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HomeAddtime1
		{
			set{ _homeaddtime1=value;}
			get{return _homeaddtime1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GuestAddTime1
		{
			set{ _guestaddtime1=value;}
			get{return _guestaddtime1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HomeAddtime2
		{
			set{ _homeaddtime2=value;}
			get{return _homeaddtime2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GuestAddTime2
		{
			set{ _guestaddtime2=value;}
			get{return _guestaddtime2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HomeAddtime3
		{
			set{ _homeaddtime3=value;}
			get{return _homeaddtime3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GuestAddTime3
		{
			set{ _guestaddtime3=value;}
			get{return _guestaddtime3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string addTechnic
		{
			set{ _addtechnic=value;}
			get{return _addtechnic;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string tv
		{
			set{ _tv=value;}
			get{return _tv;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string explain
		{
			set{ _explain=value;}
			get{return _explain;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string explain2
		{
			set{ _explain2=value;}
			get{return _explain2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string 类型
		{
			set{ _类型=value;}
			get{return _类型;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int 联赛ID
		{
			set{ _联赛id=value;}
			get{return _联赛id;}
		}
		#endregion Model

	}
}

