using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratPlay 的摘要说明。
	/// </summary>
	public class BaccaratPlay
	{
		private readonly BCW.Baccarat.DAL.BaccaratPlay dal=new BCW.Baccarat.DAL.BaccaratPlay();
		public BaccaratPlay()
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
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Baccarat.Model.BaccaratPlay model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratPlay model)
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
        /// 返回当前用户没结束的房间的个数
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratPlay Times(int UsID)
        {
            return dal.Times(UsID);
        }

        /// <summary>
        /// 返回固定用户ID的数据
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratPlay GetOwnMessage(int UsID)
        {
            return dal.GetOwnMessage(UsID);
        }

        /// <summary>
        /// 增加某房间的彩池的资金
        /// </summary>
        public void UpdateTotal(int RoomID, int RoomDoTotal)
        {
            dal.UpdateTotal(RoomID, RoomDoTotal);
        }

        /// <summary>
        /// 更新最高下注
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoHigh"></param>
        public void UpadteHigh(int RoomID, int RoomDoHigh)
        {
            dal.UpadteHigh(RoomID, RoomDoHigh);
        }

        /// <summary>
        /// 更新最低下注
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoLow"></param>
        public void UpdateLow(int RoomID, int RoomDoLow)
        {
            dal.UpdateLow(RoomID, RoomDoLow);
        }

        /// <summary>
        /// 直接封庄
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <param name="ActID"></param>
        public void UpdateRoom(int RoomID, int RoomDoTable, int ActID)
        {
            dal.UpdateRoom(RoomID, RoomDoTable, ActID);
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="title"></param>
        /// <param name="announces"></param>
        public void updateannounce(int RoomID, string title, string announces)
        {
            dal.updateannounce(RoomID, title, announces);
        }

        /// <summary>
        /// 更新房间结束后的信息
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="table"></param>
        /// <param name="actid"></param>
        public void updateActID(int roomid, int table, int actid)
        {
            dal.updateActID(roomid, table, actid);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Baccarat.Model.BaccaratPlay GetModel(int ID)
		{
			
			return dal.GetModel(ID);
		}

        /// <summary>
        /// 获取特定房间的对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratPlay GetPlay(int RoomID)
        {
            return dal.GetPlay(RoomID);
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
        public List<BCW.Baccarat.Model.BaccaratPlay> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public List<BCW.Baccarat.Model.BaccaratPlay> GetPlayList(string strWhere, string strOrder)
        {
            DataSet ds = dal.GetPlayList(strWhere, strOrder);
            return DataTableToList(ds.Tables[0]);
        }
        
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratPlay> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratPlay> modelList = new List<BCW.Baccarat.Model.BaccaratPlay>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratPlay model;
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
		/// <returns>IList BaccaratPlay</returns>
		public IList<BCW.Baccarat.Model.BaccaratPlay> GetBaccaratPlays(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetBaccaratPlays(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

