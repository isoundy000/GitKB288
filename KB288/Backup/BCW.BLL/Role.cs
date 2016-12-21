using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Role 的摘要说明。
	/// </summary>
	public class Role
	{
		private readonly BCW.DAL.Role dal=new BCW.DAL.Role();
		public Role()
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
        /// 是否历任版主
        /// </summary>
        public bool ExistsOver(int ID)
        {
            return dal.ExistsOver(ID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ForumID)
        {
            return dal.Exists(UsID, ForumID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int UsID)
        {
            return dal.Exists2(ID, UsID);
        }   

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin(int UsID)
        {
            return dal.IsAdmin(UsID);
        }

        /// <summary>
        /// 是否总版主
        /// </summary>
        public bool IsMode(int UsID)
        {
            return dal.IsMode(UsID);
        }

        /// <summary>
        /// 是否版块版主
        /// </summary>
        public bool IsSubMode(int UsID)
        {
            return dal.IsSubMode(UsID);
        }
                
        /// <summary>
        /// 是否有总版主以上权限
        /// </summary>
        public bool IsSumMode(int UsID)
        {
            return dal.IsSumMode(UsID);
        }

        /// <summary>
        /// 是否有版块版主以上权限
        /// </summary>
        public bool IsAllMode(int UsID)
        {
            return dal.IsAllMode(UsID);
        }
        
        /// <summary>
        /// 是否有版块版主以上权限(不包括圈子版主)
        /// </summary>
        public bool IsAllModeNoGroup(int UsID)
        {
            return dal.IsAllModeNoGroup(UsID);
        }
        
        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Role model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Role model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新为荣誉版主
        /// </summary>
        public void UpdateOver(int ID, int Status)
        {
            dal.UpdateOver(ID, Status);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                      
        /// <summary>
        /// 得到一个ForumID
        /// </summary>
        public int GetForumID(int ID)
        {
            return dal.GetForumID(ID);
        }

        /// <summary>
        /// 得到一个Rolece
        /// </summary>
        public string GetRolece(int UsID)
        {
            return dal.GetRolece(UsID);
        }
             
        /// <summary>
        /// 得到一个Rolece
        /// </summary>
        public string GetRolece(int UsID, int ForumID)
        {
            return dal.GetRolece(UsID, ForumID);
        }

        /// <summary>
        /// 得到一个包含下级版块的Rolece
        /// </summary>
        public string GetRoleces(int UsID, int ForumID)
        {
            return dal.GetRoleces(UsID, ForumID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Role GetRole(int ID)
		{
			
			return dal.GetRole(ID);
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
		/// <returns>IList Role</returns>
		public IList<BCW.Model.Role> GetRoles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetRoles(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// 取得管理员记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Role</returns>
        public IList<BCW.Model.Role> GetRolesAdmin(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetRolesAdmin(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

