using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Dicepay 的摘要说明。
	/// </summary>
	public class Dicepay
	{
		private readonly BCW.DAL.Game.Dicepay dal=new BCW.DAL.Game.Dicepay();
		public Dicepay()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
                   
        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int DiceId)
        {
            return dal.ExistsState(DiceId);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int DiceId, int UsID, int bzType, int Types, int BuyNum)
        {
            return dal.Exists(DiceId, UsID, bzType, Types, BuyNum);
        }
                
        /// <summary>
        /// 计算某期购买人数
        /// </summary>
        public int GetCount(int DiceId)
        {
            return dal.GetCount(DiceId);
        }

        /// <summary>
        /// 计算某期某选项购买人数
        /// </summary>
        public int GetCount(int DiceId, int Types, int BuyNum)
        {
            return dal.GetCount(DiceId, Types, BuyNum);
        }
                
        /// <summary>
        /// 计算某期某选项购买币数
        /// </summary>
        public long GetSumBuyCent(int DiceId, int bzType, int Types, int BuyNum)
        {
            return dal.GetSumBuyCent(DiceId, bzType, Types, BuyNum);
        }

        /// <summary>
        /// 计算某期某选项购买人数
        /// </summary>
        public int GetCount(int DiceId, int Types, string BuyNum)
        {
            return dal.GetCount(DiceId, Types, BuyNum);
        }

        /// <summary>
        /// 计算某期某选项购买币数
        /// </summary>
        public long GetSumBuyCent(int DiceId, int bzType, int Types, string BuyNum)
        {
            return dal.GetSumBuyCent(DiceId, bzType, Types, BuyNum);
        }
        
        /// <summary>
        /// 计算今天下注总币额
        /// </summary>
        public long GetSumBuyCent(int BzType)
        {
            return dal.GetSumBuyCent(BzType);
        }

        /// <summary>
        /// 计算今天下注返彩总币额
        /// </summary>
        public long GetSumWinCent(int BzType)
        {
            return dal.GetSumWinCent(BzType);
        }
        
        /// <summary>
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumBuyCent(string strWhere)
        {
            return dal.GetSumBuyCent(strWhere);
        }
               
        /// <summary>
        /// 根据条件计算返彩值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Dicepay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Dicepay model)
		{
			dal.Update(model);
		}
                       
        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            dal.Update(ID, WinCent, State);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
                
        /// <summary>
        /// 得到一个bzType
        /// </summary>
        public int GetbzType(int ID)
        {
            return dal.GetbzType(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Dicepay GetDicepay(int ID)
		{
			
			return dal.GetDicepay(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Dicepay</returns>
		public IList<BCW.Model.Game.Dicepay> GetDicepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDicepays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
                /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Dicepay</returns>
        public IList<BCW.Model.Game.Dicepay> GetDicepaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetDicepaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  成员方法
	}
}

