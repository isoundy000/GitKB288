using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Ballpay 的摘要说明。
	/// </summary>
	public class Ballpay
	{
		private readonly BCW.DAL.Game.Ballpay dal=new BCW.DAL.Game.Ballpay();
		public Ballpay()
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
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
                
        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int BallId)
        {
            return dal.ExistsState(BallId);
        }
                
        /// <summary>
        /// 是否存在应数字购买记录
        /// </summary>
        public bool ExistsBuyNum(int BallId, int BuyNum, int UsID)
        {
            return dal.ExistsBuyNum(BallId, BuyNum, UsID);
        }

        /// <summary>
        /// 计算某期某数字的购买数量
        /// </summary>
        public int GetCount(int BallId, int BuyNum)
        {
            return dal.GetCount(BallId, BuyNum);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Ballpay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Ballpay model)
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
        /// 得到一个BuyCount
        /// </summary>
        public int GetBuyCount(int BallId,int UsID)
        {

            return dal.GetBuyCount(BallId, UsID);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Ballpay GetBallpay(int ID)
		{
			
			return dal.GetBallpay(ID);
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
		/// <returns>IList Ballpay</returns>
		public IList<BCW.Model.Game.Ballpay> GetBallpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBallpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

