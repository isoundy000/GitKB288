using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Medalget ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Medalget
	{
		public Medalget()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private int _medalid;
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
		/// ѫ��ID
		/// </summary>
		public int MedalId
		{
			set{ _medalid=value;}
			get{return _medalid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
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

