using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Spkblack ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Spkblack
	{
		public Spkblack()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private string _blackwhy;
		private int _blackday;
		private int _adminusid;
		private DateTime _exittime;
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
		/// ��ϷID��ʶ
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
		/// �Ӻ�ԭ��
		/// </summary>
		public string BlackWhy
		{
			set{ _blackwhy=value;}
			get{return _blackwhy;}
		}
		/// <summary>
		/// �Ӻ�����
		/// </summary>
		public int BlackDay
		{
			set{ _blackday=value;}
			get{return _blackday;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int AdminUsID
		{
			set{ _adminusid=value;}
			get{return _adminusid;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime ExitTime
		{
			set{ _exittime=value;}
			get{return _exittime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

