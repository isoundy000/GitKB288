using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_Help ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_Help
	{
		public tb_Help()
		{}
		#region Model
		private int _id;
		private string _title;
		private string _explain;
		private string _linkname;
		private int _haslink;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Explain
		{
			set{ _explain=value;}
			get{return _explain;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LinkName
		{
			set{ _linkname=value;}
			get{return _linkname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int HasLink
		{
			set{ _haslink=value;}
			get{return _haslink;}
		}
		#endregion Model

	}
}

