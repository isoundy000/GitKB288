using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����xitonghao ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class xitonghao
	{
		public xitonghao()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _ip;
		private int _type;
		private DateTime? _addtime;
		private int _caoid;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IP
		{
			set{ _ip=value;}
			get{return _ip;}
		}
		/// <summary>
		/// 0����1ɾ��
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ��������ID
		/// </summary>
		public int caoID
		{
			set{ _caoid=value;}
			get{return _caoid;}
		}
		#endregion Model

	}
}

