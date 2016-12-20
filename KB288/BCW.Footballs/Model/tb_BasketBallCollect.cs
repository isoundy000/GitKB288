using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_BasketBallCollect ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_BasketBallCollect
	{
		public tb_BasketBallCollect()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private int? _basketballid;
		private int? _bianhao;
		private string _team1;
		private string _team2;
		private string _result;
		private int? _sendcount;
		private DateTime? _addtime;
		private int? _ident;
		private string _remark;
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
		public int? UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BasketBallId
		{
			set{ _basketballid=value;}
			get{return _basketballid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Bianhao
		{
			set{ _bianhao=value;}
			get{return _bianhao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string team1
		{
			set{ _team1=value;}
			get{return _team1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string team2
		{
			set{ _team2=value;}
			get{return _team2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? sendCount
		{
			set{ _sendcount=value;}
			get{return _sendcount;}
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
		public int? ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

