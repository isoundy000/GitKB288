using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratHistory 的摘要说明。
	/// </summary>
	public class BaccaratHistory
	{
		private readonly BCW.Baccarat.DAL.BaccaratHistory dal=new BCW.Baccarat.DAL.BaccaratHistory();
		public BaccaratHistory()
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
		public bool Exists(int HID)
		{
			return dal.Exists(HID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Baccarat.Model.BaccaratHistory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratHistory model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int HID)
		{
			
			dal.Delete(HID);
		}

        /// <summary>
        /// 返回一条获奖数据信息
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomTable"></param>
        /// <param name="UsID"></param>
        /// <param name="BetType"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratHistory GetHistory(int RoomID, int RoomTable, int UsID, string BetType)
        {
            return dal.GetHistory(RoomID, RoomTable, UsID, BetType);
        }

        /// <summary>
        /// 得到特定房间ID和桌面table的最旧的下注时间
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public DateTime GetMinTime(int RoomID, int RoomDoTable)
        {
            return dal.GetMinTime(RoomID, RoomDoTable);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratHistory GetModel(int HID)
        {

            return dal.GetModel(HID);
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
        public List<BCW.Baccarat.Model.BaccaratHistory> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratHistory> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratHistory> modelList = new List<BCW.Baccarat.Model.BaccaratHistory>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratHistory model;
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
		/// <returns>IList BaccaratHistory</returns>
		public IList<BCW.Baccarat.Model.BaccaratHistory> GetBaccaratHistorys(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
            return dal.GetBaccaratHistorys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

