using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Toplist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Toplist
	{
		public Toplist()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private long _wingold;
		private long _putgold;
		private int _winnum;
		private int _putnum;
		/// <summary>
		/// ��������ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsId
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
		/// Ӯ�ñ�
		/// </summary>
		public long WinGold
		{
			set{ _wingold=value;}
			get{return _wingold;}
		}
		/// <summary>
		/// Ͷ���
		/// </summary>
		public long PutGold
		{
			set{ _putgold=value;}
			get{return _putgold;}
		}
		/// <summary>
		/// Ӯ�Ĵ���
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int PutNum
		{
			set{ _putnum=value;}
			get{return _putnum;}
		}
		#endregion Model

	}
}

