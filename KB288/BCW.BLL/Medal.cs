using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Medal 的摘要说明。
	/// </summary>
	public class Medal
	{
		private readonly BCW.DAL.Medal dal=new BCW.DAL.Medal();
		public Medal()
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
        public bool ExistsVip(int UsID)
        {
            return dal.ExistsVip(UsID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }
        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTypes(int Types, int UsID)
        {
            return dal.ExistsTypes(Types, UsID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsForumId(int ID, int ForumId)
        {
            return dal.ExistsForumId(ID, ForumId);
        }
              
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPayIDtemp(int ForumId, int UsID)
        {
            return dal.ExistsPayIDtemp(ForumId, UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Medal model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Medal model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 更新会员勋章库存
        /// </summary>
        public void UpdateiCount(int ID, int iCount)
        {

            dal.UpdateiCount(ID, iCount);
        }
               
        /// <summary>
        /// 更新会临时勋章
        /// </summary>
        public void UpdatePayIDtemp(int ID, string PayIDtemp)
        {
            dal.UpdatePayIDtemp(ID, PayIDtemp);
        }

        /// <summary>
        /// 更新会员勋章
        /// </summary>
        public void UpdatePayID(int ID, string PayID, string PayExTime)
        {
            dal.UpdatePayID(ID, PayID, PayExTime);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
               
        /// <summary>
        /// 得到购买记录实体
        /// </summary>
        public string GetImageUrl(int ID)
        {

            return dal.GetImageUrl(ID);
        }
                       
        /// <summary>
        /// 得到购买记录实体
        /// </summary>
        public BCW.Model.Medal GetMedalMe(int ForumId, int UsID)
        {
            return dal.GetMedalMe(ForumId, UsID);
        }

        /// <summary>
        /// 得到论坛个性标识
        /// </summary>
        public BCW.Model.Medal GetMedalForum(int UsID)
        {

            return dal.GetMedalForum(UsID);
        }

        /// <summary>
        /// 得到购买记录实体
        /// </summary>
        public BCW.Model.Medal GetMedalMe(int ID)
        {

            return dal.GetMedalMe(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Medal GetMedal(int ID)
		{
			
			return dal.GetMedal(ID);
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
		/// <returns>IList Medal</returns>
		public IList<BCW.Model.Medal> GetMedals(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMedals(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

