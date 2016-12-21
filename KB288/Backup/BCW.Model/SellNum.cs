using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����SellNum ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SellNum
	{
		public SellNum()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private int _buyuid;
		private long _price;
		private int _state;
		private DateTime _addtime;
		private string _mobile;
		private string _notes;
		private int _tags;
		private DateTime _paytime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
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
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// �����ID
		/// </summary>
		public int BuyUID
		{
			set{ _buyuid=value;}
			get{return _buyuid;}
		}
		/// <summary>
		/// ID�۸�
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// ״̬(1��ѯ��/2�ѱ���/3������һ�/4�ѳɹ�)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// �ύʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �󶨵��ֻ���
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// ϵͳ��ע
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// QQ��������
		/// </summary>
		public int Tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
		/// <summary>
		/// �ɽ�ʱ��
		/// </summary>
		public DateTime PayTime
		{
			set{ _paytime=value;}
			get{return _paytime;}
		}
		#endregion Model

	}
}

