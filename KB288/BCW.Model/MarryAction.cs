using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����MarryAction ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class MarryAction
	{
		public MarryAction()
		{}
		#region Model
		private int _id;
		private int _marryid;
		private string _content;
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
		/// ���ID
		/// </summary>
		public int MarryId
		{
			set{ _marryid=value;}
			get{return _marryid;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
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

