using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Textdc ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Textdc
	{
		public Textdc()
		{}
		#region Model
		private int _id;
		private int _bid;
		private int _usid;
		private long _outcent;
		private long _accent;
		private int _isztid;
		private int _bztype;
		private int _state;
		private DateTime _addtime;
		private string _logtext;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
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
		/// ��ע��
		/// </summary>
		public long OutCent
		{
			set{ _outcent=value;}
			get{return _outcent;}
		}
		/// <summary>
		/// ȷ�ϱҶ����ΪׯӮ������Ϊ��Ӯ��
		/// </summary>
		public long AcCent
		{
			set{ _accent=value;}
			get{return _accent;}
		}
		/// <summary>
		/// 0����ID/1�м�ID
		/// </summary>
		public int IsZtid
		{
			set{ _isztid=value;}
			get{return _isztid;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ״̬��0δ����/1��Ӯȷ����/2ׯӮȷ����/3�ѽ�����
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
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
		/// ��־��¼��|�ֿ���
		/// </summary>
		public string LogText
		{
			set{ _logtext=value;}
			get{return _logtext;}
		}
		#endregion Model

	}
}

