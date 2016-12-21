using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Chat 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Chat
	{
		public Chat()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _chatname;
		private string _chatnotes;
		private string _chatsz;
		private string _chatjs;
		private string _chatlg;
		private string _chatfoot;
		private long _chatcent;
		private string _centpwd;
		private int _chatonline;
		private int _chattopline;
		private decimal _chatscore;
		private int _usid;
		private int _groupid;
		private string _chatpwd;
		private string _pwdid;
		private string _chatct;
		private string _chatcbig;
		private string _chatcsmall;
		private string _chatcid;
		private int _chatcon;
		private DateTime _chatctime;
		private int _paixu;
		private DateTime _addtime;
		private DateTime _extime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 聊天室类型（0官方/1圈子/2/同城/3私人）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 聊天室名称
		/// </summary>
		public string ChatName
		{
			set{ _chatname=value;}
			get{return _chatname;}
		}
		/// <summary>
		/// 聊天室主题
		/// </summary>
		public string ChatNotes
		{
			set{ _chatnotes=value;}
			get{return _chatnotes;}
		}
		/// <summary>
		/// 室主ID（多个用#分开）
		/// </summary>
		public string ChatSZ
		{
			set{ _chatsz=value;}
			get{return _chatsz;}
		}
		/// <summary>
		/// 见习室主ID（多个用#分开）
		/// </summary>
		public string ChatJS
		{
			set{ _chatjs=value;}
			get{return _chatjs;}
		}
		/// <summary>
		/// 临管ID（多个用#分开）
		/// </summary>
		public string ChatLG
		{
			set{ _chatlg=value;}
			get{return _chatlg;}
		}
		/// <summary>
		/// 聊室底部UBB
		/// </summary>
		public string ChatFoot
		{
			set{ _chatfoot=value;}
			get{return _chatfoot;}
		}
		/// <summary>
		/// 聊室基金数
		/// </summary>
		public long ChatCent
		{
			set{ _chatcent=value;}
			get{return _chatcent;}
		}
		/// <summary>
		/// 基金密码
		/// </summary>
		public string CentPwd
		{
			set{ _centpwd=value;}
			get{return _centpwd;}
		}
		/// <summary>
		/// 当前在线人数
		/// </summary>
		public int ChatOnLine
		{
			set{ _chatonline=value;}
			get{return _chatonline;}
		}
		/// <summary>
		/// 最高记录在线人数
		/// </summary>
		public int ChatTopLine
		{
			set{ _chattopline=value;}
			get{return _chattopline;}
		}
		/// <summary>
		/// 聊天室积分
		/// </summary>
		public decimal ChatScore
		{
			set{ _chatscore=value;}
			get{return _chatscore;}
		}
		/// <summary>
		/// 创建ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 关联圈子或城市ID
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 聊天室密码
		/// </summary>
		public string ChatPwd
		{
			set{ _chatpwd=value;}
			get{return _chatpwd;}
		}
		/// <summary>
		/// 密码进入的ID
		/// </summary>
		public string PwdID
		{
			set{ _pwdid=value;}
			get{return _pwdid;}
		}
		/// <summary>
		/// 抢币词语
		/// </summary>
		public string ChatCT
		{
			set{ _chatct=value;}
			get{return _chatct;}
		}
		/// <summary>
		/// 抢币整点随机值如10-20
		/// </summary>
		public string ChatCbig
		{
			set{ _chatcbig=value;}
			get{return _chatcbig;}
		}
		/// <summary>
		/// 抢币非整点随机值如1-5
		/// </summary>
		public string ChatCsmall
		{
			set{ _chatcsmall=value;}
			get{return _chatcsmall;}
		}
		/// <summary>
		/// 抢币详细操作
		/// </summary>
		public string ChatCId
		{
			set{ _chatcid=value;}
			get{return _chatcid;}
		}
		/// <summary>
		/// 抢币循环时间（秒）
		/// </summary>
		public int ChatCon
		{
			set{ _chatcon=value;}
			get{return _chatcon;}
		}
		/// <summary>
		/// 抢币出币时间
		/// </summary>
		public DateTime ChatCTime
		{
			set{ _chatctime=value;}
			get{return _chatctime;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
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
		/// 过期时间
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		#endregion Model

	}
}

