using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类klsfpay 的摘要说明。
	/// </summary>
	public class klsfpay
	{
		private readonly BCW.DAL.klsfpay dal=new BCW.DAL.klsfpay();
		public klsfpay()
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
        /// 根据字段统计有多少条数据符合条件
        /// </summary>
        /// <param name="strWhere">统计条件</param>
        /// <returns>统计结果</returns>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
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
            return dal.ExistsState(ID,UsID);
        }

        /// <summary>
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            return dal.GetSumPrices(strWhere);
        }

        /// <summary>
        /// 根据条件计算赢取币值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.klsfpay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.klsfpay model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID,State);
        }

        /// <summary>
        /// 更新开奖数据
        /// </summary>
        public void UpdateResult(string klsfId, string Result)
        {
            dal.UpdateResult(klsfId, Result);
        }

        /// <summary>
        /// 更新游戏开奖得币
        /// </summary>
        public void UpdateWinCent(int ID, long WinCent, string WinNotes)
        {
            dal.UpdateWinCent(ID,WinCent,WinNotes);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int klsfId)
        {
            return dal.GetSumPrices(UsID,klsfId);
        }
        ///<summary>
        ///某期某种投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes(int Types, int klsfId)
        {
            return dal.GetSumPricebyTypes(Types, klsfId);
        }

        /// <summary>
        /// 根据ID得到klsfId
        /// </summary>
        public int GetklsfId(int ID)
        {
            return dal.GetklsfId(ID);
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
		public BCW.Model.klsfpay Getklsfpay(int ID)
		{
			return dal.Getklsfpay(ID);
		}
        /// <summary>
        /// 存在机器人
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int ID, int UsID)
        {
            return dal.ExistsReBot(ID, UsID);
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
		/// <returns>IList klsfpay</returns>
		public IList<BCW.Model.klsfpay> Getklsfpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getklsfpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

         /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.klsfpay> GetklsfpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetklsfpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

