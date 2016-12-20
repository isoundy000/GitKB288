using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Frigroup 的摘要说明。
	/// </summary>
	public class Frigroup
	{
		private readonly BCW.DAL.Frigroup dal=new BCW.DAL.Frigroup();
		public Frigroup()
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID, int Types)
        {
            return dal.Exists(ID, UsID, Types);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTitle(int UsID, string Title, int Types)
        {
            return dal.ExistsTitle(UsID, Title, Types);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Frigroup model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Frigroup model)
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
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID, int UsID, int Types)
        {
            return dal.GetTitle(ID, UsID, Types);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Frigroup GetFrigroup(int ID)
		{
			
			return dal.GetFrigroup(ID);
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
		/// <returns>IList Frigroup</returns>
		public IList<BCW.Model.Frigroup> GetFrigroups(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetFrigroups(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

