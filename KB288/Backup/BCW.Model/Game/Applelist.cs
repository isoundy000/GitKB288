using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Applelist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Applelist
	{
		public Applelist()
		{}
		#region Model
		private int _id;
		private int _pingguo;
		private int _mugua;
		private int _xigua;
		private int _mangguo;
		private int _shuangxing;
		private int _jinzhong;
		private int _shuangqi;
		private int _yuanbao;
		private DateTime _endtime;
		private string _opentext;
		private long _paycent;
		private long _wincent;
		private int _wincount;
		private int _state;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ƻ����ע
		/// </summary>
		public int PingGuo
		{
			set{ _pingguo=value;}
			get{return _pingguo;}
		}
		/// <summary>
		/// ľ����ע
		/// </summary>
		public int MuGua
		{
			set{ _mugua=value;}
			get{return _mugua;}
		}
		/// <summary>
		/// ������ע
		/// </summary>
		public int XiGua
		{
			set{ _xigua=value;}
			get{return _xigua;}
		}
		/// <summary>
		/// â����ע
		/// </summary>
		public int MangGuo
		{
			set{ _mangguo=value;}
			get{return _mangguo;}
		}
		/// <summary>
		/// ˫����ע
		/// </summary>
		public int ShuangXing
		{
			set{ _shuangxing=value;}
			get{return _shuangxing;}
		}
		/// <summary>
		/// ������ע
		/// </summary>
		public int JinZhong
		{
			set{ _jinzhong=value;}
			get{return _jinzhong;}
		}
		/// <summary>
		/// ˫����ע
		/// </summary>
		public int ShuangQi
		{
			set{ _shuangqi=value;}
			get{return _shuangqi;}
		}
		/// <summary>
		/// Ԫ����ע
		/// </summary>
		public int YuanBao
		{
			set{ _yuanbao=value;}
			get{return _yuanbao;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// ������â��[С]��
		/// </summary>
		public string OpenText
		{
			set{ _opentext=value;}
			get{return _opentext;}
		}
		/// <summary>
		/// Ѻע�Ҷ�
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// �н��Ҷ�
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// �н�ע��
		/// </summary>
		public int WinCount
		{
			set{ _wincount=value;}
			get{return _wincount;}
		}
		/// <summary>
		/// ״̬(0������Ϸ/1�ѽ���)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

