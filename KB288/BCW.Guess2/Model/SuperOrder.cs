using System;
namespace TPR2.Model.guess
{
	/// <summary>
	/// ʵ����SuperOrder ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SuperOrder
	{
		public SuperOrder()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private decimal _cent;
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
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Cent
		{
			set{ _cent=value;}
			get{return _cent;}
		}
		#endregion Model

	}
}

