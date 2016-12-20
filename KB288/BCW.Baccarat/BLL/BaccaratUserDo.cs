using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratUserDo 的摘要说明。
	/// </summary>
	public class BaccaratUserDo
	{
		private readonly BCW.Baccarat.DAL.BaccaratUserDo dal=new BCW.Baccarat.DAL.BaccaratUserDo();
		public BaccaratUserDo()
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
		public bool Exists(int DoID)
		{
			return dal.Exists(DoID);
		}

        /// <summary>
        /// 某用户在某一个房间的桌面是否存在了该类型的数据
        /// </summary>
        public bool ExistsMessage(int UsID, int RoomID, int RoomDoTable, string BetTypes)
        {
            return dal.ExistsMessage(UsID, RoomID, RoomDoTable, BetTypes);
        }

        /// <summary>
        /// 获取房间的最新台面
        /// </summary>
        /// <param name="RoomID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratUserDo NewRoomMessage(int RoomID)
        {
            return dal.NewRoomMessage(RoomID);
        }

        /// <summary>
        /// 获取在确定用户、房间、桌面和下注类型的数据类型
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetBetTypesMessage(int UsID, int RoomID, int RoomDoTable, string BetTypes)
        {
            return dal.GetBetTypesMessage(UsID, RoomID, RoomDoTable, BetTypes);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratUserDo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratUserDo model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新某一个表的某一特定字段
        /// </summary>
        public DataSet UpdateBonus(string strField, string strWhere)
        {
            return dal.UpdateBonus(strField, strWhere);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int DoID)
		{
			
			dal.Delete(DoID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Baccarat.Model.BaccaratUserDo GetBaccaratUserDo(int DoID)
		{
			
			return dal.GetBaccaratUserDo(DoID);
		}

        /// <summary>
        /// 返回具体信息
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="RoomID"></param>
        /// <param name="RoomTable"></param>
        /// <param name="BetType"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratUserDo GetUserDo(int UsID, int RoomID, int RoomTable, string BetType)
        {
            return dal.GetUserDo(UsID, RoomID, RoomTable, BetType);
        }

        /// <summary>
        /// 获取特定房间的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetRoomMessage(int RoomID)
        {
            return dal.GetRoomMessage(RoomID);
        }

        /// <summary>
        ///得到特定房间ID和桌面table的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetRoomtableMessage(int RoomID,int RoomDoTable)
        {
            return dal.GetRoomtableMessage(RoomID, RoomDoTable);
        }

        /// <summary>
       /// 获取特定用户ID在特定房间ID和台数Table的所有数据
       /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetUserMessage(int UsID, int RoomID, int RoomDoTable)
        {
            return dal.GetUserMessage(UsID, RoomID, RoomDoTable);
        }

        /// <summary>
        ///得到特定房间ID和桌面table的最新的下注时间
        /// </summary>
        public DateTime GetMaxBetTime(int RoomID,int RoomDoTable,int type)
        {
            return dal.GetMaxBetTime(RoomID, RoomDoTable,type);
        }

        /// <summary>
        ///得到特定房间ID和桌面table的最旧的下注时间
        /// </summary>
        public DateTime GetMinBetTime(int RoomID, int RoomDoTable, int type)
        {
            return dal.GetMinBetTime(RoomID, RoomDoTable,type);
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratUserDo> List(string strWhere)
        {
            DataSet da = dal.GetList(strWhere);
            return DataTableToList(da.Tables[0]);
            
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratUserDo> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratUserDo> modelList = new List<BCW.Baccarat.Model.BaccaratUserDo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratUserDo model;
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
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList BaccaratUserDo</returns>
		public IList<BCW.Baccarat.Model.BaccaratUserDo> GetBaccaratUserDos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratUserDos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
        public List<int> GetListBang(int RoomID, int RoomDoTable)
        {
            return dal.GetListBang(RoomID, RoomDoTable);
        }
        #endregion  成员方法
    }
}

