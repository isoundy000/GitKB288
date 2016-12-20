using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// 业务逻辑类BaccaratRoom 的摘要说明。
	/// </summary>
	public class BaccaratRoom
	{
		private readonly BCW.Baccarat.DAL.BaccaratRoom dal=new BCW.Baccarat.DAL.BaccaratRoom();
		public BaccaratRoom()
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
		public bool Exists(int RoomID)
		{
			return dal.Exists(RoomID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratRoom model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratRoom model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RoomID)
		{
			
			dal.Delete(RoomID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Baccarat.Model.BaccaratRoom GetBaccaratRoom(int RoomID)
		{
			
			return dal.GetBaccaratRoom(RoomID);
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
		/// <returns>IList BaccaratRoom</returns>
        public IList<BCW.Baccarat.Model.BaccaratRoom> GetBaccaratRooms(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
            return dal.GetBaccaratRooms(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

