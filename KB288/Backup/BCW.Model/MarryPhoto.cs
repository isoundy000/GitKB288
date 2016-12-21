using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����MarryPhoto ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class MarryPhoto
	{
		public MarryPhoto()
		{}
		#region Model
		private int _id;
		private int _marryid;
		private int _usid;
		private string _usname;
		private string _prevfile;
		private string _actfile;
		private string _notes;
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
		/// �ϴ��û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �ϴ��û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// Сͼ��ַ
		/// </summary>
		public string PrevFile
		{
			set{ _prevfile=value;}
			get{return _prevfile;}
		}
		/// <summary>
		/// ��ͼ��ַ
		/// </summary>
		public string ActFile
		{
			set{ _actfile=value;}
			get{return _actfile;}
		}
		/// <summary>
		/// ͼƬ����
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// �ϴ�ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

