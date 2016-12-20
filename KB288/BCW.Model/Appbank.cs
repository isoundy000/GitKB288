using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Appbank ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Appbank
	{
		public Appbank()
		{}
		#region Model
		private int _id;
		private int _types;
		private long _addgold;
		private int _usid;
		private string _usname;
		private string _mobile;
		private string _cardnum;
		private string _cardname;
		private string _cardaddress;
		private string _notes;
		private string _adminnotes;
		private int _state;
		private DateTime _addtime;
		private DateTime _retime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ���ͣ�Ĭ��0
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ��������Ŀ
		/// </summary>
		public long AddGold
		{
			set{ _addgold=value;}
			get{return _addgold;}
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
		/// �����ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// �ֻ���
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string CardNum
		{
			set{ _cardnum=value;}
			get{return _cardnum;}
		}
		/// <summary>
		/// ������
		/// </summary>
		public string CardName
		{
			set{ _cardname=value;}
			get{return _cardname;}
		}
		/// <summary>
		/// ������ַ
		/// </summary>
		public string CardAddress
		{
			set{ _cardaddress=value;}
			get{return _cardaddress;}
		}
		/// <summary>
		/// �����߱�ע
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// ����Ա��ע
		/// </summary>
		public string AdminNotes
		{
			set{ _adminnotes=value;}
			get{return _adminnotes;}
		}
		/// <summary>
		/// ״̬
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
		/// ����ʱ��
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		#endregion Model

	}
}

