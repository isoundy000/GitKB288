using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����flowzz ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class flowzz
	{
		public flowzz()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _cnum;
		private int _price;
		private int _leven;
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
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �ɲ����ٶ仨
		/// </summary>
		public int cNum
		{
			set{ _cnum=value;}
			get{return _cnum;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// ��Ҫ���ٵȼ��ſ��Թ���
		/// </summary>
		public int Leven
		{
			set{ _leven=value;}
			get{return _leven;}
		}
		#endregion Model

	}
}

