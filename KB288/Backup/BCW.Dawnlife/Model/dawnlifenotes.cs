using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����dawnlifenotes ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class dawnlifenotes
	{
		public dawnlifenotes()
		{}
		#region Model
		private int _id;
		private int _coin;
		private int _usid;
		private int _city;
		private int _area;
		private long _money;
		private long _debt;
		private string _buy;
		private string _sell;
		private long _price;
		private long _num;
		private DateTime _date;
        private int _day;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public int coin
		{
			set{ _coin=value;}
			get{return _coin;}
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
		/// ����
		/// </summary>
		public int city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public long money
		{
			set{ _money=value;}
			get{return _money;}
		}
		/// <summary>
		/// Ƿ��
		/// </summary>
		public long debt
		{
			set{ _debt=value;}
			get{return _debt;}
		}
		/// <summary>
		/// ������Ʒ
		/// </summary>
		public string buy
		{
			set{ _buy=value;}
			get{return _buy;}
		}
		/// <summary>
		/// ������Ʒ
		/// </summary>
		public string sell
		{
			set{ _sell=value;}
			get{return _sell;}
		}
		/// <summary>
		/// �ۼ�
		/// </summary>
		public long price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public long num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// ʱ��
		/// </summary>
		public DateTime date
		{
			set{ _date=value;}
			get{return _date;}
		}
        /// <summary>
        /// ����
        /// </summary>
        public int day
        {
            set { _day = value; }
            get { return _day; }
        }
		#endregion Model

	}
}

