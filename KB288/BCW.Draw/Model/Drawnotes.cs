using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// ʵ����Drawnotes ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Drawnotes
	{
		public Drawnotes()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _jifen;
		private string _game;
		private string _gname;
		private long _gvalue;
		private long _jvalue;
		private long _change;
		private DateTime _date;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// ����
		/// </summary>
		public long jifen
		{
			set{ _jifen=value;}
			get{return _jifen;}
		}
		/// <summary>
		/// ��Դ����Ϸ
		/// </summary>
		public string game
		{
			set{ _game=value;}
			get{return _game;}
		}
		/// <summary>
		/// ��Դ�ı���
		/// </summary>
		public string gname
		{
			set{ _gname=value;}
			get{return _gname;}
		}
		/// <summary>
		/// �һ���ֵ
		/// </summary>
		public long gvalue
		{
			set{ _gvalue=value;}
			get{return _gvalue;}
		}
		/// <summary>
		/// ���ֶһ�ֵ
		/// </summary>
		public long jvalue
		{
			set{ _jvalue=value;}
			get{return _jvalue;}
		}
		/// <summary>
		/// �һ��Ļ���
		/// </summary>
		public long change
		{
			set{ _change=value;}
			get{return _change;}
		}
		/// <summary>
		/// �һ�ʱ��
		/// </summary>
		public DateTime date
		{
			set{ _date=value;}
			get{return _date;}
		}
		#endregion Model

	}
}

