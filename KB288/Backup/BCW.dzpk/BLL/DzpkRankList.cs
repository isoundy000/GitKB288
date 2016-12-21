using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// 达人排行榜
	/// </summary>
	public class DzpkRankList
	{
		private readonly BCW.dzpk.DAL.DzpkRankList dal=new BCW.dzpk.DAL.DzpkRankList();
		public DzpkRankList()
		{}
		#region  成员方法
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
		public int  Add(BCW.dzpk.Model.DzpkRankList model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkRankList model)
		{
			dal.Update(model);
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
		public BCW.dzpk.Model.DzpkRankList GetDzpkRankList(int ID)
		{
			
			return dal.GetDzpkRankList(ID);
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
		/// <returns>IList DzpkRankList</returns>
		public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkRankLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
		/// 取得排行榜合计的每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList DzpkRankList</returns>
        public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists_Total(int p_pageIndex, int p_pageSize, string strWhere,string Sort,string OrderBy, out int p_recordCount,out string strsql)
		{
            return dal.GetDzpkRankLists_Total(p_pageIndex, p_pageSize, strWhere, Sort, OrderBy, out p_recordCount, out strsql);
		}

        public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists_Total_All(string strWhere, string Sort, string OrderBy)
        {
            return dal.GetDzpkRankLists_Total_All(strWhere, Sort, OrderBy);
        }

        #endregion  成员方法
    }
}

