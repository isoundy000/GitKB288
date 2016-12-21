using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Blacklist 的摘要说明。
	/// </summary>
	public class Blacklist
	{
		private readonly BCW.DAL.Blacklist dal=new BCW.DAL.Blacklist();
		public Blacklist()
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
        public bool Exists(int UsID, int ForumID)
        {
            return dal.Exists(UsID, ForumID);
        }
                
        /// <summary>
        /// 是否存在该权限记录
        /// </summary>
        public bool ExistsRole(int UsID, string BlackRole)
        {
            return dal.ExistsRole(UsID, BlackRole);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Blacklist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Blacklist model)
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
        /// 删除一条数据
        /// </summary>
        public void Delete(int UsID, int ForumID)
        {
            dal.Delete(UsID, ForumID);
        }
               
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteRole(int UsID, string BlackRole)
        {
            dal.DeleteRole(UsID, BlackRole);
        }

        /// <summary>
        /// 得到一个Role
        /// </summary>
        public string GetRole(int UsID, int ForumID)
        {
            return dal.GetRole(UsID, ForumID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Blacklist GetBlacklist(int ID)
		{
			
			return dal.GetBlacklist(ID);
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
		/// <returns>IList Blacklist</returns>
		public IList<BCW.Model.Blacklist> GetBlacklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBlacklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

