using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Dicelist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Dicelist
	{
		public Dicelist()
		{}
		#region Model
		private int _id;
		private int _winnum;
		private DateTime _begintime;
		private DateTime _endtime;
		private long _pool;
		private int _wincount;
		private long _winpool;
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
		/// ��ʼ��Ϸʱ��
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// ��Ϸ��������ʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// ������Ѻע��
		/// </summary>
		public long Pool
		{
			set{ _pool=value;}
			get{return _pool;}
		}
		/// <summary>
		/// �����н�����
		/// </summary>
		public int WinCount
		{
			set{ _wincount=value;}
			get{return _wincount;}
		}
		/// <summary>
		/// ������Ѻע��2
		/// </summary>
		public long WinPool
		{
			set{ _winpool=value;}
			get{return _winpool;}
		}
		/// <summary>
		/// ״̬ 0/δ����/1�ѿ���
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

