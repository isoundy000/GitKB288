using System;
namespace LHC.Model
{
	/// <summary>
	/// ʵ����VoteNo ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class VoteNo
	{
		public VoteNo()
		{}
		#region Model
		private int _id;
		private int _qino;
		private DateTime _extime;
		private long _paycent;
		private int _paycount;
		private long _paycent2;
		private int _paycount2;
		private int _snum;
		private int _pnum1;
		private int _pnum2;
		private int _pnum3;
		private int _pnum4;
		private int _pnum5;
		private int _pnum6;
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
		/// ����
		/// </summary>
		public int qiNo
		{
			set{ _qino=value;}
			get{return _qino;}
		}
		/// <summary>
		/// ��ֹʱ��
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		/// <summary>
		/// ��ע�ܶ�
		/// </summary>
		public long payCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// ��ע����
		/// </summary>
		public int payCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// ��ע�ܶ�
		/// </summary>
		public long payCent2
		{
			set{ _paycent2=value;}
			get{return _paycent2;}
		}
		/// <summary>
		/// ��ע����
		/// </summary>
		public int payCount2
		{
			set{ _paycount2=value;}
			get{return _paycount2;}
		}
		/// <summary>
		/// �ر����
		/// </summary>
		public int sNum
		{
			set{ _snum=value;}
			get{return _snum;}
		}
		/// <summary>
		/// ��ͨ����1
		/// </summary>
		public int pNum1
		{
			set{ _pnum1=value;}
			get{return _pnum1;}
		}
		/// <summary>
		/// ��ͨ����2
		/// </summary>
		public int pNum2
		{
			set{ _pnum2=value;}
			get{return _pnum2;}
		}
		/// <summary>
		/// ��ͨ����3
		/// </summary>
		public int pNum3
		{
			set{ _pnum3=value;}
			get{return _pnum3;}
		}
		/// <summary>
		/// ��ͨ����4
		/// </summary>
		public int pNum4
		{
			set{ _pnum4=value;}
			get{return _pnum4;}
		}
		/// <summary>
		/// ��ͨ����5
		/// </summary>
		public int pNum5
		{
			set{ _pnum5=value;}
			get{return _pnum5;}
		}
		/// <summary>
		/// ��ͨ����6
		/// </summary>
		public int pNum6
		{
			set{ _pnum6=value;}
			get{return _pnum6;}
		}
		/// <summary>
		/// ״̬(0δ����/1�ѿ���)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

