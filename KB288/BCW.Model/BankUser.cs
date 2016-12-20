using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类BankUser 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BankUser
	{
		public BankUser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _bankname;
		private string _banktitle1;
		private string _bankno1;
		private string _bankadd1;
		private string _banktitle2;
		private string _bankno2;
		private string _bankadd2;
		private string _banktitle3;
		private string _bankno3;
		private string _bankadd3;
		private string _banktitle4;
		private string _bankno4;
		private string _bankadd4;
		private string _zfbname;
		private string _zfbno;
		private int _state;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 自增ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 姓名
		/// </summary>
		public string BankName
		{
			set{ _bankname=value;}
			get{return _bankname;}
		}
		/// <summary>
		/// 银行名称1
		/// </summary>
		public string BankTitle1
		{
			set{ _banktitle1=value;}
			get{return _banktitle1;}
		}
		/// <summary>
		/// 银行帐号1
		/// </summary>
		public string BankNo1
		{
			set{ _bankno1=value;}
			get{return _bankno1;}
		}
		/// <summary>
		/// 银行开户1
		/// </summary>
		public string BankAdd1
		{
			set{ _bankadd1=value;}
			get{return _bankadd1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankTitle2
		{
			set{ _banktitle2=value;}
			get{return _banktitle2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankNo2
		{
			set{ _bankno2=value;}
			get{return _bankno2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankAdd2
		{
			set{ _bankadd2=value;}
			get{return _bankadd2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankTitle3
		{
			set{ _banktitle3=value;}
			get{return _banktitle3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankNo3
		{
			set{ _bankno3=value;}
			get{return _bankno3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankAdd3
		{
			set{ _bankadd3=value;}
			get{return _bankadd3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankTitle4
		{
			set{ _banktitle4=value;}
			get{return _banktitle4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankNo4
		{
			set{ _bankno4=value;}
			get{return _bankno4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BankAdd4
		{
			set{ _bankadd4=value;}
			get{return _bankadd4;}
		}
		/// <summary>
		/// 支付宝名称
		/// </summary>
		public string ZFBName
		{
			set{ _zfbname=value;}
			get{return _zfbname;}
		}
		/// <summary>
		/// 支付宝帐号
		/// </summary>
		public string ZFBNo
		{
			set{ _zfbno=value;}
			get{return _zfbno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

