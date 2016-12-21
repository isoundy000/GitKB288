using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Applelist 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 苹果下注
		/// </summary>
		public int PingGuo
		{
			set{ _pingguo=value;}
			get{return _pingguo;}
		}
		/// <summary>
		/// 木瓜下注
		/// </summary>
		public int MuGua
		{
			set{ _mugua=value;}
			get{return _mugua;}
		}
		/// <summary>
		/// 西瓜下注
		/// </summary>
		public int XiGua
		{
			set{ _xigua=value;}
			get{return _xigua;}
		}
		/// <summary>
		/// 芒果下注
		/// </summary>
		public int MangGuo
		{
			set{ _mangguo=value;}
			get{return _mangguo;}
		}
		/// <summary>
		/// 双星下注
		/// </summary>
		public int ShuangXing
		{
			set{ _shuangxing=value;}
			get{return _shuangxing;}
		}
		/// <summary>
		/// 金钟下注
		/// </summary>
		public int JinZhong
		{
			set{ _jinzhong=value;}
			get{return _jinzhong;}
		}
		/// <summary>
		/// 双七下注
		/// </summary>
		public int ShuangQi
		{
			set{ _shuangqi=value;}
			get{return _shuangqi;}
		}
		/// <summary>
		/// 元宝下注
		/// </summary>
		public int YuanBao
		{
			set{ _yuanbao=value;}
			get{return _yuanbao;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 开奖（芒果[小]）
		/// </summary>
		public string OpenText
		{
			set{ _opentext=value;}
			get{return _opentext;}
		}
		/// <summary>
		/// 押注币额
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 中奖币额
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 中奖注数
		/// </summary>
		public int WinCount
		{
			set{ _wincount=value;}
			get{return _wincount;}
		}
		/// <summary>
		/// 状态(0正在游戏/1已结束)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

