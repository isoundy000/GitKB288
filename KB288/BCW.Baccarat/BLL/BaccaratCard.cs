using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratCard 的摘要说明。
	/// </summary>
	public class BaccaratCard
	{
		private readonly BCW.Baccarat.DAL.BaccaratCard dal=new BCW.Baccarat.DAL.BaccaratCard();
		public BaccaratCard()
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
        /// 清除表记录
        /// </summary>
        /// <param name="TableName"></param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }

        /// <summary>
        /// 是否存在某房间某桌面的扑克牌
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public bool ExistsCard(int RoomID, int RoomDoTable)
        {
            return dal.ExistsCard(RoomID, RoomDoTable);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(BCW.Baccarat.Model.BaccaratCard model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratCard model)
        {
            return dal.Update(model);
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
        public BCW.Baccarat.Model.BaccaratCard GetModel(int ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        ///得到特定房间ID和桌面table的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BaccaratCard GetCardMessage(int RoomID, int RoomDoTable)
        {
            return dal.GetCardMessage(RoomID, RoomDoTable);
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
        public List<BCW.Baccarat.Model.BaccaratCard> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratCard> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratCard> modelList = new List<BCW.Baccarat.Model.BaccaratCard>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratCard model;
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
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Card</returns>
        public IList<BCW.Baccarat.Model.BaccaratCard> GetCards(int SizeNum, string strWhere)
        {
            return dal.GetCards(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList BaccaratCard</returns>
		public IList<BCW.Baccarat.Model.BaccaratCard> GetBaccaratCards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratCards(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

