using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����SSClist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SSClist
	{
		public SSClist()
		{}
		#region Model
		private int _id;
		private int _sscid;
		private string _result;
		private string _notes;
		private int _state;
		private DateTime _endtime;
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
		public int SSCId
		{
			set{ _sscid=value;}
			get{return _sscid;}
		}
		/// <summary>
		/// �������:1 2 3 4 5
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// ״̬��0������1���н����
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ������ֹʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

