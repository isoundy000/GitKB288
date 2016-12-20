using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Chat ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ���������ͣ�0�ٷ�/1Ȧ��/2/ͬ��/3˽�ˣ�
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public string ChatName
		{
			set{ _chatname=value;}
			get{return _chatname;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public string ChatNotes
		{
			set{ _chatnotes=value;}
			get{return _chatnotes;}
		}
		/// <summary>
		/// ����ID�������#�ֿ���
		/// </summary>
		public string ChatSZ
		{
			set{ _chatsz=value;}
			get{return _chatsz;}
		}
		/// <summary>
		/// ��ϰ����ID�������#�ֿ���
		/// </summary>
		public string ChatJS
		{
			set{ _chatjs=value;}
			get{return _chatjs;}
		}
		/// <summary>
		/// �ٹ�ID�������#�ֿ���
		/// </summary>
		public string ChatLG
		{
			set{ _chatlg=value;}
			get{return _chatlg;}
		}
		/// <summary>
		/// ���ҵײ�UBB
		/// </summary>
		public string ChatFoot
		{
			set{ _chatfoot=value;}
			get{return _chatfoot;}
		}
		/// <summary>
		/// ���һ�����
		/// </summary>
		public long ChatCent
		{
			set{ _chatcent=value;}
			get{return _chatcent;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string CentPwd
		{
			set{ _centpwd=value;}
			get{return _centpwd;}
		}
		/// <summary>
		/// ��ǰ��������
		/// </summary>
		public int ChatOnLine
		{
			set{ _chatonline=value;}
			get{return _chatonline;}
		}
		/// <summary>
		/// ��߼�¼��������
		/// </summary>
		public int ChatTopLine
		{
			set{ _chattopline=value;}
			get{return _chattopline;}
		}
		/// <summary>
		/// �����һ���
		/// </summary>
		public decimal ChatScore
		{
			set{ _chatscore=value;}
			get{return _chatscore;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ����Ȧ�ӻ����ID
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public string ChatPwd
		{
			set{ _chatpwd=value;}
			get{return _chatpwd;}
		}
		/// <summary>
		/// ��������ID
		/// </summary>
		public string PwdID
		{
			set{ _pwdid=value;}
			get{return _pwdid;}
		}
		/// <summary>
		/// ���Ҵ���
		/// </summary>
		public string ChatCT
		{
			set{ _chatct=value;}
			get{return _chatct;}
		}
		/// <summary>
		/// �����������ֵ��10-20
		/// </summary>
		public string ChatCbig
		{
			set{ _chatcbig=value;}
			get{return _chatcbig;}
		}
		/// <summary>
		/// ���ҷ��������ֵ��1-5
		/// </summary>
		public string ChatCsmall
		{
			set{ _chatcsmall=value;}
			get{return _chatcsmall;}
		}
		/// <summary>
		/// ������ϸ����
		/// </summary>
		public string ChatCId
		{
			set{ _chatcid=value;}
			get{return _chatcid;}
		}
		/// <summary>
		/// ����ѭ��ʱ�䣨�룩
		/// </summary>
		public int ChatCon
		{
			set{ _chatcon=value;}
			get{return _chatcon;}
		}
		/// <summary>
		/// ���ҳ���ʱ��
		/// </summary>
		public DateTime ChatCTime
		{
			set{ _chatctime=value;}
			get{return _chatctime;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		#endregion Model

	}
}

