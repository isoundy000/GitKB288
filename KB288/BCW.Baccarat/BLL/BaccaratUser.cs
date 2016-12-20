using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratUser 的摘要说明。
	/// </summary>
	public class BaccaratUser
	{
		private readonly BCW.Baccarat.DAL.BaccaratUser dal=new BCW.Baccarat.DAL.BaccaratUser();
		public BaccaratUser()
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
        /// 判断是否存在该用户记录
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public bool ExistsUser(int UsID)
        {
            return dal.ExistsUser(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratUser model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratUser model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 更新用户开庄权限
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="RoomTimes"></param>
        /// <returns></returns>
        public bool UpdateTimes(int UsID, int RoomTimes)
        {
            return dal.UpdateTimes(UsID, RoomTimes);
        }

        /// <summary>
        /// 更新用户发牌设置
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="SetID"></param>
        /// <returns></returns>
        public bool UpdateSet(int UsID, int SetID)
        {
            return dal.UpdateSet(UsID, SetID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUser GetModel(int ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUser GetUser(int UsID)
        {
            return dal.GetUser(UsID);
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
        public List<BCW.Baccarat.Model.BaccaratUser> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratUser> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratUser> modelList = new List<BCW.Baccarat.Model.BaccaratUser>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratUser model;
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
		/// <returns>IList BaccaratUser</returns>
		public IList<BCW.Baccarat.Model.BaccaratUser> GetBaccaratUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

