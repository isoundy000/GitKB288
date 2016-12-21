using System;
namespace BCW.dzpk.Model
{
	/// <summary>
	/// ʵ����DzpkHistory ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class DzpkHistory
	{
		public DzpkHistory()
		{}
		#region Model
		private int _id;
		private int _rmid;
		private int _usid;
		private int _rankchk;
		private string _pokercards;
		private string _pokerchips;
		private DateTime _timeout;
		private string _winner;
		private string _rmmake;
		private long _getmoney;
		private int? _ispayout;
		/// <summary>
		/// ��ʷID���Զ�����
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����ID ��Ӧ����ID
		/// </summary>
		public int RmID
		{
			set{ _rmid=value;}
			get{return _rmid;}
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
		/// ��ע���̱��0Ϊׯ�ң�1ΪСä��2Ϊ��ä��˳��ݼӣ���������������
		/// </summary>
		public int RankChk
		{
			set{ _rankchk=value;}
			get{return _rankchk;}
		}
		/// <summary>
		/// ���鷽ʽ��¼ÿһ�ַ��Ƶ���ϸ��Ϣ�ö��ŷָ���P��������
		/// </summary>
		public string PokerCards
		{
			set{ _pokercards=value;}
			get{return _pokercards;}
		}
		/// <summary>
		/// ÿ����ע�ĳ����������ִ����ע����0�������P��������
		/// </summary>
		public string PokerChips
		{
			set{ _pokerchips=value;}
			get{return _pokerchips;}
		}
		/// <summary>
		/// ������עʱ�䣬�����Ϸ���Ĳ���ʱ�䣬��������Ϊ�޲������޳�������
		/// </summary>
		public DateTime TimeOut
		{
			set{ _timeout=value;}
			get{return _timeout;}
		}
		/// <summary>
		/// ��ʷ�ж���¼������˲��㷨���
		/// </summary>
		public string Winner
		{
			set{ _winner=value;}
			get{return _winner;}
		}
		/// <summary>
		/// �����ǣ���ʷ����ͳ����
		/// </summary>
		public string RmMake
		{
			set{ _rmmake=value;}
			get{return _rmmake;}
		}
		/// <summary>
		/// �õ���Ǯ����
		/// </summary>
		public long GetMoney
		{
			set{ _getmoney=value;}
			get{return _getmoney;}
		}
		/// <summary>
		/// �Ƿ��ɲ� 0�� 1��
		/// </summary>
		public int? IsPayOut
		{
			set{ _ispayout=value;}
			get{return _ispayout;}
		}
		#endregion Model

	}
}

