using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Shopuser 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Shopuser
	{
		public Shopuser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _giftid;
		private string _prevpic;
		private string _gifttitle;
		private int _total;
		private DateTime _addtime;
        private string _pic;
        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 礼物名称
		/// </summary>
		public string GiftTitle
		{
			set{ _gifttitle=value;}
			get{return _gifttitle;}
		}
		/// <summary>
		/// 礼物ID
		/// </summary>
		public int GiftId
		{
			set{ _giftid=value;}
			get{return _giftid;}
		}
		/// <summary>
		/// 礼物小图
		/// </summary>
		public string PrevPic
		{
			set{ _prevpic=value;}
			get{return _prevpic;}
		}
		/// <summary>
		/// 数量
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        /// <summary>
		/// 0为商城1为农场  //邵广林 20160607
		/// </summary>
		public string PIC
        {
            set { _pic = value; }
            get { return _pic; }
        }
        #endregion Model

    }
}

