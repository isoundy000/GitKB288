using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����flowmyzz ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class flowmyzz
	{
		public flowmyzz()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _zid;
		private string _ztitle;
		private int _znum;
		private int _usid;
		private string _usname;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// Types(0���ӣ�1�ʻ�)
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int zid
		{
			set{ _zid=value;}
			get{return _zid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string ztitle
		{
			set{ _ztitle=value;}
			get{return _ztitle;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int znum
		{
			set{ _znum=value;}
			get{return _znum;}
		}
		/// <summary>
		/// ��ԱID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ��Ա�ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		#endregion Model

	}
}

