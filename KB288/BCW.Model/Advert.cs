using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Advert ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Advert
	{
		public Advert()
		{}
		#region Model
		private int _id;
		private string _title;
		private string _adurl;
		private DateTime _starttime;
		private DateTime _overtime;
		private int _status;
		private int _igold;
		private int _click;
		private DateTime _clicktime;
		private string _clickid;
		private int _adtype;
		private int _urltype;
		private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ������ʶ����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ���֧��UBB��WML
		/// </summary>
		public string AdUrl
		{
			set{ _adurl=value;}
			get{return _adurl;}
		}
		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		public DateTime StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// ��������ͣ
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// �򹤵ñ�
		/// </summary>
		public int iGold
		{
			set{ _igold=value;}
			get{return _igold;}
		}
		/// <summary>
		/// ���
		/// </summary>
		public int Click
		{
			set{ _click=value;}
			get{return _click;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime ClickTime
		{
			set{ _clicktime=value;}
			get{return _clicktime;}
		}
		/// <summary>
		/// ���ID
		/// </summary>
		public string ClickID
		{
			set{ _clickid=value;}
			get{return _clickid;}
		}
		/// <summary>
		/// ����ͱ�����
		/// </summary>
		public int adType
		{
			set{ _adtype=value;}
			get{return _adtype;}
		}
		/// <summary>
		/// ����ַ����(0��ַ/1UBB/2WML)
		/// </summary>
		public int UrlType
		{
			set{ _urltype=value;}
			get{return _urltype;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

