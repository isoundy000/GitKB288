using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����GroupText ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class GroupText
	{
		public GroupText()
		{}
		#region Model
		private int _id;
		private int _groupid;
		private int _usid;
		private string _usname;
		private int _toid;
		private string _toname;
		private string _content;
		private int _iskiss;
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
		/// Ȧ��ID
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// �����û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �����û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ���Ը�ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// ���Ը��ǳ�
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// �Ƿ�����
		/// </summary>
		public int IsKiss
		{
			set{ _iskiss=value;}
			get{return _iskiss;}
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

