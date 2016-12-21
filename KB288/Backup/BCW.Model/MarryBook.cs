using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����MarryBook ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class MarryBook
	{
		public MarryBook()
		{}
		#region Model
		private int _id;
		private int _marryid;
		private int _reid;
		private string _rename;
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
		/// ����ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// �����ǳ�
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
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
		/// �ҵ�ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

