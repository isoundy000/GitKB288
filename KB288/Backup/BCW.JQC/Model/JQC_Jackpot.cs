using System;
namespace BCW.JQC.Model
{
	/// <summary>
	/// ʵ����JQC_Jackpot ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class JQC_Jackpot
	{
		public JQC_Jackpot()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _inprize;
		private long _outprize;
		private long _jackpot;
		private DateTime _addtime;
		private int _phase;
		private int _type;
		private int _betid;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �û���
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// Ͷ����
		/// </summary>
		public long InPrize
		{
			set{ _inprize=value;}
			get{return _inprize;}
		}
		/// <summary>
		/// �ɽ���
		/// </summary>
		public long OutPrize
		{
			set{ _outprize=value;}
			get{return _outprize;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public long Jackpot
		{
			set{ _jackpot=value;}
			get{return _jackpot;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �ں�
		/// </summary>
		public int phase
		{
			set{ _phase=value;}
			get{return _phase;}
		}
        /// <summary>
        /// 0�����ע1�ɽ�2�˻�3ϵͳ��ȡ4ϵͳ����
        /// </summary>
        public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// ͶעID
		/// </summary>
		public int BetID
		{
			set{ _betid=value;}
			get{return _betid;}
		}
		#endregion Model

	}
}

