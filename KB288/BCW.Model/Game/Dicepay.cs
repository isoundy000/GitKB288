using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Dicepay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Dicepay
	{
		public Dicepay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _diceid;
		private int _usid;
		private string _usname;
		private int _buynum;
		private int _buycount;
		private long _buycent;
		private long _wincent;
		private int _state;
		private int _bztype;
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
		/// Ѻע���� 1Ѻ��С/2Ѻ��˫/3Ѻ�ܺ�/4Ѻ����/5Ѻ���� 
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int DiceId
		{
			set{ _diceid=value;}
			get{return _diceid;}
		}
		/// <summary>
		/// ��ԱID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ��Ա�ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// Ѻעѡ�� ��С�͵�˫��1|2��ʾ
		/// </summary>
		public int BuyNum
		{
			set{ _buynum=value;}
			get{return _buynum;}
		}
		/// <summary>
		/// Ѻ����ѡ����ٷ�
		/// </summary>
		public int BuyCount
		{
			set{ _buycount=value;}
			get{return _buycount;}
		}
		/// <summary>
		/// Ѻ���ٱ�
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
		/// <summary>
		/// Ӯ���ٱ�
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ״̬ 0δ����|1�ѿ���|2�Ѷҽ�
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ״̬ 0���|1��ֵ��
		/// </summary>
		public int bzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// Ѻעʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

