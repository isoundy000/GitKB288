using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_GuestSend 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_GuestSend
	{
		public tb_GuestSend()
		{}
		#region Model
		private int _id;
		private int? _manageid;
		private string _guestcontent;
		private string _senduidlist;
		private string _seeuidlist;
		private int? _sendcount;
		private int? _seecount;
		private string _sendtime;
		private DateTime? _senddatetime;
		private int? _sentday;
		private int? _allgold;
		private int? _hbcount;
		private string _hblist;
		private int? _nowgold;
		private int? _maxcount;
		private int? _getcount;
		private DateTime? _addtime;
		private DateTime? _overtime;
		private int? _isdone;
		private string _remark;
		private int? _issendcount;
		private string _notseenidlist;
		private int? _sendtype;
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
		public int? manageID
		{
			set{ _manageid=value;}
			get{return _manageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string guestContent
		{
			set{ _guestcontent=value;}
			get{return _guestcontent;}
		}
		/// <summary>
		/// 已发送的ID列
		/// </summary>
		public string sendUidList
		{
			set{ _senduidlist=value;}
			get{return _senduidlist;}
		}
		/// <summary>
		/// 已查看的的ID列
		/// </summary>
		public string seeUidList
		{
			set{ _seeuidlist=value;}
			get{return _seeuidlist;}
		}
		/// <summary>
		/// 已发送的人数
		/// </summary>
		public int? sendCount
		{
			set{ _sendcount=value;}
			get{return _sendcount;}
		}
		/// <summary>
		/// 当前已看人数=抽奖总数
		/// </summary>
		public int? seeCount
		{
			set{ _seecount=value;}
			get{return _seecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sendTime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? sendDateTime
		{
			set{ _senddatetime=value;}
			get{return _senddatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? sentDay
		{
			set{ _sentday=value;}
			get{return _sentday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? allgold
		{
			set{ _allgold=value;}
			get{return _allgold;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? hbCount
		{
			set{ _hbcount=value;}
			get{return _hbcount;}
		}
		/// <summary>
		/// 红包列
		/// </summary>
		public string hbList
		{
			set{ _hblist=value;}
			get{return _hblist;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? nowgold
		{
			set{ _nowgold=value;}
			get{return _nowgold;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? maxCount
		{
			set{ _maxcount=value;}
			get{return _maxcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? getCount
		{
			set{ _getcount=value;}
			get{return _getcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 当前发送第几次
		/// </summary>
		public int? isSendCount
		{
			set{ _issendcount=value;}
			get{return _issendcount;}
		}
		/// <summary>
		/// 剩余未查看的ID列，下一次将发送内线
		/// </summary>
		public string notSeenIDList
		{
			set{ _notseenidlist=value;}
			get{return _notseenidlist;}
		}
		/// <summary>
		/// 发布的类型（指定会员...）
		/// </summary>
		public int? sendtype
		{
			set{ _sendtype=value;}
			get{return _sendtype;}
		}
		#endregion Model

	}
}

