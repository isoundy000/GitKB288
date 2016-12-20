using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Modata ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Modata
	{
		public Modata()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _phonebrand;
		private string _phonemodel;
		private string _phonesystem;
		private string _phonesize;
		private int _phoneclick;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// Ʒ������ID
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// Ʒ������
		/// </summary>
		public string PhoneBrand
		{
			set{ _phonebrand=value;}
			get{return _phonebrand;}
		}
		/// <summary>
		/// �ֻ��ͺ�
		/// </summary>
		public string PhoneModel
		{
			set{ _phonemodel=value;}
			get{return _phonemodel;}
		}
		/// <summary>
		/// ����ϵͳ
		/// </summary>
		public string PhoneSystem
		{
			set{ _phonesystem=value;}
			get{return _phonesystem;}
		}
		/// <summary>
		/// ��Ļ�ֱ���
		/// </summary>
		public string PhoneSize
		{
			set{ _phonesize=value;}
			get{return _phonesize;}
		}
		/// <summary>
		/// ѡ������
		/// </summary>
		public int PhoneClick
		{
			set{ _phoneclick=value;}
			get{return _phoneclick;}
		}
		#endregion Model

	}
}

