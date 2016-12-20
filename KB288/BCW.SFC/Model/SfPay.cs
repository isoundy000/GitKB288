using System;
namespace BCW.SFC.Model
{
	/// <summary>
	/// ʵ����SfPay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SfPay
	{
		public SfPay()
		{}
		#region Model
		private int _id;
		private int _cid;
		private int? _usid;
		private string _vote;
		private int? _votenum;
		private int? _override;
		private int? _paycent;
		private long _paycents;
		private int? _state;
		private long _wincent;
		private DateTime? _addtime;
		private int? _isprize;
		private int _isspier;
        private string _change;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �ں�
		/// </summary>
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? usID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// Ͷע
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// Ͷע��
		/// </summary>
		public int? VoteNum
		{
			set{ _votenum=value;}
			get{return _votenum;}
		}
		/// <summary>
		/// Ͷע����
		/// </summary>
		public int? OverRide
		{
			set{ _override=value;}
			get{return _override;}
		}
		/// <summary>
		/// ��ע���
		/// </summary>
		public int? PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// ����ע��
		/// </summary>
		public long PayCents
		{
			set{ _paycents=value;}
			get{return _paycents;}
		}
		/// <summary>
		/// �ҽ���0,1��
		/// </summary>
		public int? State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// �н����
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �н���0δ�н���1�ж��ȣ�2��һ�ȣ�
		/// </summary>
		public int? IsPrize
		{
			set{ _isprize=value;}
			get{return _isprize;}
		}
		/// <summary>
		/// �������жϣ�0���ǣ�1�ǣ�
		/// </summary>
		public int IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
        /// <summary>
		/// ��ֹ����������ֶ�
		/// </summary>
		public string change
        {
            set { _change = value; }
            get { return _change; }
        }
        #endregion Model

    }
}

