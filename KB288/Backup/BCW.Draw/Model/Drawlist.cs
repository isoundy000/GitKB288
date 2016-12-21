using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// ʵ����Drawlist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Drawlist
	{
		public Drawlist()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private DateTime? _time;
		private int? _type;
		private int? _goodscounts;
		private int? _jifen;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int? UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ʱ��
		/// </summary>
		public DateTime? Time
		{
			set{ _time=value;}
			get{return _time;}
		}
		/// <summary>
		/// ��ʽ����
		/// </summary>
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// ��Ʒ���
		/// </summary>
		public int? GoodsCounts
		{
			set{ _goodscounts=value;}
			get{return _goodscounts;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int? Jifen
		{
			set{ _jifen=value;}
			get{return _jifen;}
		}
		#endregion Model

	}
}

