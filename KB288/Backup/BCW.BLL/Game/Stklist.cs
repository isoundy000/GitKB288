using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Stklist 的摘要说明。
	/// </summary>
	public class Stklist
	{
		private readonly BCW.DAL.Game.Stklist dal=new BCW.DAL.Game.Stklist();
		public Stklist()
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
        public bool Exists()
        {
            return dal.Exists();
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(DateTime EndTime)
        {
            return dal.Exists(EndTime);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Stklist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Stklist model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update2(BCW.Model.Game.Stklist model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新总押注金额
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            dal.UpdatePool(ID, Pool);
        }

        /// <summary>
        /// 更新总押注金额2
        /// </summary>
        public void UpdateWinPool(int ID, long WinPool)
        {
            dal.UpdateWinPool(ID, WinPool);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DateTime GetEndTime(int ID)
        {
            return dal.GetEndTime(ID);
        }

        /// <summary>
        /// 得到一个本期实体
        /// </summary>
        public BCW.Model.Game.Stklist GetStklist()
        {

            return dal.GetStklist();
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Stklist GetStklist(int ID)
		{
			
			return dal.GetStklist(ID);
		}

        ///<summary>
        ///根据日期得到期数
        /// </summary>
        public int GetIDbyDate(string stk)
        {
            return dal.GetIDbyDate(stk);
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
		/// <returns>IList Stklist</returns>
		public IList<BCW.Model.Game.Stklist> GetStklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetStklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

