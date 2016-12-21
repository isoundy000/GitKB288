/**  版本信息模板在安装目录下，可自行修改。
* tb_IPSGiveLog.cs
*
* 功 能： N/A
* 类 名： tb_IPSGiveLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/7/16 14:57:17   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace BCW.IPSPay.Model
{
	/// <summary>
	/// tb_IPSGiveLog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tb_IPSGiveLog
	{
		public tb_IPSGiveLog()
		{}
		#region Model
		private int _id;
		private int _manageid;
		private string _bznote;
		private decimal _getmoney;
		private decimal _g_arge=10M;
		private int _g_type=0;
		private DateTime _addtime= DateTime.Now;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 管理员操作ID
		/// </summary>
		public int ManageID
		{
			set{ _manageid=value;}
			get{return _manageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BzNote
		{
			set{ _bznote=value;}
			get{return _bznote;}
		}
		/// <summary>
		/// 提取金额,必须大于手续费
		/// </summary>
		public decimal GetMoney
		{
			set{ _getmoney=value;}
			get{return _getmoney;}
		}
		/// <summary>
		/// 提现手续费 默认10元一次
		/// </summary>
		public decimal G_arge
		{
			set{ _g_arge=value;}
			get{return _g_arge;}
		}
		/// <summary>
		/// 提取类型0 充值 1商城
		/// </summary>
		public int G_type
		{
			set{ _g_type=value;}
			get{return _g_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

