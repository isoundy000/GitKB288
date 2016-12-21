using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// 业务逻辑类DzpkAct 的摘要说明。
	/// </summary>
	public class DzpkAct
	{
		private readonly BCW.dzpk.DAL.DzpkAct dal=new BCW.dzpk.DAL.DzpkAct();
		public DzpkAct()
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
		public int  Add(BCW.dzpk.Model.DzpkAct model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkAct model)
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
		public BCW.dzpk.Model.DzpkAct GetDzpkAct(int ID)
		{
			
			return dal.GetDzpkAct(ID);
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
		/// <returns>IList DzpkAct</returns>
		public IList<BCW.dzpk.Model.DzpkAct> GetDzpkActs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkActs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList DzpkAct</returns>
        public IList<BCW.dzpk.Model.DzpkAct> GetDzpkActs(int p_pageIndex, int p_pageSize, string strWhere,string sOrder, out int p_recordCount)
        {
            return dal.GetDzpkActs(p_pageIndex, p_pageSize, strWhere, sOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

