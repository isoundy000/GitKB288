using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Upgroup 的摘要说明。
	/// </summary>
	public class Upgroup
	{
		private readonly BCW.DAL.Upgroup dal=new BCW.DAL.Upgroup();
		public Upgroup()
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
        public bool Exists(int ID, int UsID, int Leibie)
        {
            return dal.Exists(ID, UsID, Leibie);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTitle(int UsID, string Title, int Leibie)
        {
            return dal.ExistsTitle(UsID, Title, Leibie);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Upgroup model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Upgroup model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新封面
        /// </summary>
        public void UpdateNode(int ID, string Node)
        {
            dal.UpdateNode(ID, Node);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// 得到一个Title
        /// </summary>
        public string GetTitle(int ID, int UsID)
        {
            return dal.GetTitle(ID, UsID);
        }
               
        /// <summary>
        /// 得到一个Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }
        /// <summary>
        /// 得到一个IsReview
        /// </summary>
        public int GetIsReview(int ID)
        {
            return dal.GetIsReview(ID);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Upgroup GetUpgroup(int ID)
		{
			
			return dal.GetUpgroup(ID);
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
		/// <returns>IList Upgroup</returns>
		public IList<BCW.Model.Upgroup> GetUpgroups(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetUpgroups(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

