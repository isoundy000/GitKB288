using System;
namespace BCW.XinKuai3.Model
{
	/// <summary>
	/// ʵ����SWB ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SWB
	{
		public SWB()
		{}
		#region Model
		private int _userid;
		private long _xk3money;
		private DateTime _xk3isget;
		/// <summary>
		/// 
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long XK3Money
		{
			set{ _xk3money=value;}
			get{return _xk3money;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime XK3IsGet
		{
			set{ _xk3isget=value;}
			get{return _xk3isget;}
		}
		#endregion Model

	}
}

