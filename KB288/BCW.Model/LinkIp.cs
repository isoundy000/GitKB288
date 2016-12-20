using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����LinkIp ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class LinkIp
	{
		public LinkIp()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _addusip;
		private string _addusua;
		private string _adduspage;
		private int _linkid;
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
		/// IP���ͣ�0����IP|1����IP��
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// IP��ַ
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// UA�����
		/// </summary>
		public string AddUsUA
		{
			set{ _addusua=value;}
			get{return _addusua;}
		}
		/// <summary>
		/// ��Դ��ַ
		/// </summary>
		public string AddUsPage
		{
			set{ _adduspage=value;}
			get{return _adduspage;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int LinkId
		{
			set{ _linkid=value;}
			get{return _linkid;}
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

