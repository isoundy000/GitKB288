using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratTop 的摘要说明。
	/// </summary>
	public class BaccaratTop
	{
		private readonly BCW.Baccarat.DAL.BaccaratTop dal=new BCW.Baccarat.DAL.BaccaratTop();
		public BaccaratTop()
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
        /// 清除表记录
        /// </summary>
        /// <param name="TableName"></param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int TopID)
		{
			return dal.Exists(TopID);
		}

        /// <summary>
        /// 是否存在某用户的数据
        /// </summary>
        public bool ExistsUser(int UsID)
        {
            return dal.ExistsUser(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratTop model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratTop model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 更新某一个表的某一特定字段
        /// </summary>
        public void UpdateTop(int UsID, int TopBonusSum)
        {
            dal.UpdateTop(UsID, TopBonusSum);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int TopID)
		{
			
			dal.Delete(TopID);
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratTop GetModel(int TopID)
        {

            return dal.GetModel(TopID);
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratTop> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratTop> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratTop> modelList = new List<BCW.Baccarat.Model.BaccaratTop>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratTop model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        // <summary>
        /// 获取排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns></returns>
        public IList<BCW.Baccarat.Model.BaccaratTop> GetUserTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetUserTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList BaccaratTop</returns>
		public IList<BCW.Baccarat.Model.BaccaratTop> GetBaccaratTops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratTops(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

