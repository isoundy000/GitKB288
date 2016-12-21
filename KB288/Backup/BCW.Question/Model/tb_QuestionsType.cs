using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_QuestionsType ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_QuestionsType
	{
		public tb_QuestionsType()
		{}
		#region Model
		private int _id;
		private string _name;
		private int? _rank;
		private int? _statue;
		private int? _ident;
		private DateTime? _addtime;
		private string _remark;
		private string _type;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int? rank
		{
			set{ _rank=value;}
			get{return _rank;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int? statue
		{
			set{ _statue=value;}
			get{return _statue;}
		}
		/// <summary>
		/// ��ʶ����ID
		/// </summary>
		public int? ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

