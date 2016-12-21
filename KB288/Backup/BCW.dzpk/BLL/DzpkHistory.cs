using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// 业务逻辑类DzpkHistory 的摘要说明。
	/// </summary>
	public class DzpkHistory
	{
		private readonly BCW.dzpk.DAL.DzpkHistory dal=new BCW.dzpk.DAL.DzpkHistory();
		public DzpkHistory()
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
		public int  Add(BCW.dzpk.Model.DzpkHistory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkHistory model)
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
		public BCW.dzpk.Model.DzpkHistory GetDzpkHistory(int ID)
		{
			
			return dal.GetDzpkHistory(ID);
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
		/// <returns>IList DzpkHistory</returns>
		public IList<BCW.dzpk.Model.DzpkHistory> GetDzpkHistorys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkHistorys(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

