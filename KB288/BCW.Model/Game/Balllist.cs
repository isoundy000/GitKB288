using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Balllist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Balllist
	{
		public Balllist()
		{}
		#region Model
		private int _id;
		private int _winnum;
		private int _outnum;
		private int _addnum;
		private int _icent;
		private int _odds;
		private DateTime _begintime;
		private DateTime _endtime;
		private long _pool;
		private long _beforepool;
		private int _state;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int OutNum
		{
			set{ _outnum=value;}
			get{return _outnum;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public int AddNum
		{
			set{ _addnum=value;}
			get{return _addnum;}
		}
		/// <summary>
		/// ÿ�ݱ���
		/// </summary>
		public int iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		/// <summary>
		/// ÿ�ݱ���
		/// </summary>
		public int Odds
		{
			set{ _odds=value;}
			get{return _odds;}
		}
		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public long Pool
		{
			set{ _pool=value;}
			get{return _pool;}
		}
		/// <summary>
		/// ���½���
		/// </summary>
		public long BeforePool
		{
			set{ _beforepool=value;}
			get{return _beforepool;}
		}
		/// <summary>
		/// ״̬��0������/1�ѽ�����
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

