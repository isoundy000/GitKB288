using System;
namespace BCW.farm.Model
{
	/// <summary>
	/// ʵ����NC_zhitiao ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class NC_zhitiao
	{
		public NC_zhitiao()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _contact;
		private DateTime _addtime;
		private int _type;
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
		/// ����
		/// </summary>
		public string contact
		{
			set{ _contact=value;}
			get{return _contact;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 0Ͷ��1����2ͨ��
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

