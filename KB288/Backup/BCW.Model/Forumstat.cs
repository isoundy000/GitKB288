using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Forumstat ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Forumstat
	{
		public Forumstat()
		{}
		#region Model
		private int _id;
		private int _forumid;
		private int _usid;
		private string _usname;
		private int _ttotal;
		private int _rtotal;
		private int _gtotal;
		private int _jtotal;
		private int _hjtotal;
		private int _zttotal;
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
		/// ��̳ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
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
		/// ��������
		/// </summary>
		public int tTotal
		{
			set{ _ttotal=value;}
			get{return _ttotal;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int rTotal
		{
			set{ _rtotal=value;}
			get{return _rtotal;}
		}
		/// <summary>
		/// ���ӱ��Ӿ�����
		/// </summary>
		public int gTotal
		{
			set{ _gtotal=value;}
			get{return _gtotal;}
		}
		/// <summary>
		/// ���ӱ��Ƽ�����
		/// </summary>
		public int jTotal
		{
			set{ _jtotal=value;}
			get{return _jtotal;}
		}
		/// <summary>
		/// �����Ӿ�����
		/// </summary>
		public int hjTotal
		{
			set{ _hjtotal=value;}
			get{return _hjtotal;}
		}
		/// <summary>
		/// ��ר������
		/// </summary>
		public int ztTotal
		{
			set{ _zttotal=value;}
			get{return _zttotal;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

