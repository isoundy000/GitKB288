using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// ʵ����DrawDS ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class DrawDS
	{
		public DrawDS()
		{}
		#region Model
		private int _id;
		private int? _goodscounts;
		private string _gamename;
		private string _ds;
		private int? _dsid;
		private string _one;
		private string _two;
		private string _three;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// ��Ϸ��
		/// </summary>
		public string gamename
		{
			set{ _gamename=value;}
			get{return _gamename;}
		}
		/// <summary>
		/// ���߻���������
		/// </summary>
		public string DS
		{
			set{ _ds=value;}
			get{return _ds;}
		}
		/// <summary>
		/// ���߻������Ե�ID
		/// </summary>
		public int? DSID
		{
			set{ _dsid=value;}
			get{return _dsid;}
		}
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public string one
		{
			set{ _one=value;}
			get{return _one;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public string two
		{
			set{ _two=value;}
			get{return _two;}
		}
		/// <summary>
		/// ��Ʒ״̬
		/// </summary>
		public string three
		{
			set{ _three=value;}
			get{return _three;}
		}
		#endregion Model

	}
}

