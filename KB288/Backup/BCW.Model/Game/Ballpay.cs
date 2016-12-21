using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Ballpay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Ballpay
	{
		public Ballpay()
		{}
		#region Model
		private int _id;
		private int _ballid;
		private int _usid;
		private string _usname;
		private int _buynum;
		private int _buycount;
		private long _buycent;
		private long _wincent;
		private int _state;
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
		/// ����ID
		/// </summary>
		public int BallId
		{
			set{ _ballid=value;}
			get{return _ballid;}
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
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int BuyNum
		{
			set{ _buynum=value;}
			get{return _buynum;}
		}
		/// <summary>
		/// �ܹ������
		/// </summary>
		public int BuyCount
		{
			set{ _buycount=value;}
			get{return _buycount;}
		}
		/// <summary>
		/// �ܹ������
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
		/// <summary>
		/// Ӯȡ����
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ״̬��0δ����/1�ѿ���/2�Ѷҽ���
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
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

