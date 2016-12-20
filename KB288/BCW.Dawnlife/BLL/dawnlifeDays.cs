using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类dawnlifeDays 的摘要说明。
	/// </summary>
	public class dawnlifeDays
	{
		private readonly BCW.DAL.dawnlifeDays dal=new BCW.DAL.dawnlifeDays();
		public dawnlifeDays()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.dawnlifeDays model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.dawnlifeDays model)
		{
			dal.Update(model);
		}

        public void Updategoods(int ID,string goods)
        {
            dal.Updategoods(ID,goods);
        }
        public void Updateprice(int ID, string price)
        {
            dal.Updateprice(ID, price);
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
		public BCW.Model.dawnlifeDays GetdawnlifeDays(int ID)
		{
			
			return dal.GetdawnlifeDays(ID);
		}


        /// 根据查询影响的行数
        /// </summary>
        public int GetRowByUsID(int UsID, int day,long coin)
        {
            return dal.GetRowByUsID(UsID, day,coin );
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// 根据查询影响的行数
        /// </summary>
        public int GetDayByUsID(int UsID)
        {
            return dal.GetDayByUsID(UsID);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList dawnlifeDays</returns>
		public IList<BCW.Model.dawnlifeDays> GetdawnlifeDayss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetdawnlifeDayss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

