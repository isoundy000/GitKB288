using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Medalget 的摘要说明。
	/// </summary>
	public class Medalget
	{
		private readonly BCW.DAL.Medalget dal=new BCW.DAL.Medalget();
		public Medalget()
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
		public bool Exists(int UsID,int ID)
		{
			return dal.Exists(UsID,ID);
		}
               
        /// <summary>
        /// 计算某用户数量
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Medalget model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Medalget model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UsID,int ID)
		{
			
			dal.Delete(UsID,ID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Medalget GetMedalget(int UsID,int ID)
		{
			
			return dal.GetMedalget(UsID,ID);
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
		/// <returns>IList Medalget</returns>
		public IList<BCW.Model.Medalget> GetMedalgets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMedalgets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// 获授勋章排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Medalget> GetMedalgetsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {

            return dal.GetMedalgetsTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

