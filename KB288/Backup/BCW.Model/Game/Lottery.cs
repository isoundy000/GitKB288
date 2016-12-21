using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Lottery ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Lottery
	{
		public Lottery()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private long _paycent;
		private long _outcent;
		private string _outgift;
		private int _usid;
		private string _usname;
		private int _freshsec;
		private int _freshmin;
		private int _wincount;
		private DateTime _actime;
		private DateTime _addtime;
		private DateTime _endtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// �齱����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �齱�ܽ��
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// ֧���˶���
		/// </summary>
		public long OutCent
		{
			set{ _outcent=value;}
			get{return _outcent;}
		}
		/// <summary>
		/// ���������
		/// </summary>
		public string OutGift
		{
			set{ _outgift=value;}
			get{return _outgift;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ÿ�ֳ齱ʱ��(��)
		/// </summary>
		public int FreshSec
		{
			set{ _freshsec=value;}
			get{return _freshsec;}
		}
		/// <summary>
		/// ÿ�ֽ�����N���Ӻ������һ��
		/// </summary>
		public int FreshMin
		{
			set{ _freshmin=value;}
			get{return _freshmin;}
		}
		/// <summary>
		/// �н���
		/// </summary>
		public int WinCount
		{
			set{ _wincount=value;}
			get{return _wincount;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AcTime
		{
			set{ _actime=value;}
			get{return _actime;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

