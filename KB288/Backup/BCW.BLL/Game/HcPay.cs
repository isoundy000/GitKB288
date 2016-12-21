using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类HcPay 的摘要说明。
	/// </summary>
	public class HcPay
	{
		private readonly BCW.DAL.Game.HcPay dal=new BCW.DAL.Game.HcPay();
		public HcPay()
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
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}
             
        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            return dal.ExistsState(id, UsID);
        }
                
        /// <summary>
        /// 每ID每期下注币数
        /// </summary>
        public long GetPayCent(int UsID, int CID)
        {
            return dal.GetPayCent(UsID, CID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.HcPay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.HcPay model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int id)
        {
            dal.UpdateState(id);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
                
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.HcPay GetHcPay(int id)
		{
			
			return dal.GetHcPay(id);
		}
                
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        /// <summary>
        /// 得到一个WinCent1
        /// </summary>
        public long GetWinCent1(string time1, string time2)
        {
            return dal.GetWinCent1(time1,time2);
        }
        /// <summary>
        /// 得到一个GetPayCent1
        /// </summary>
        public long GetPayCent1(string time1, string time2)
        {
            return dal.GetPayCent1(time1, time2);
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
		/// <returns>IList HcPay</returns>
		public IList<BCW.Model.Game.HcPay> GetHcPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetHcPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
           
        
        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.Model.Game.HcPay> GetHcPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetHcPaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

