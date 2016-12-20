using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类HcList 的摘要说明。
	/// </summary>
	public class HcList
	{
        /// <summary>
        /// wdy 20160524
        /// </summary>
		private readonly BCW.DAL.Game.HcList dal=new BCW.DAL.Game.HcList();
		public HcList()
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
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}
        /// <summary>
        /// 是否存在该开奖号码
        /// </summary>
        public bool Existsm(int num)
        {
            return dal.Existsm(num);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(BCW.Model.Game.HcList model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.HcList model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update1(int CID, long payCent, int payCount)
        {
            dal.Update1(CID, payCent, payCount);
        }

        /// <summary>
        /// 得到上一期CID
        /// </summary>
        public int CID()
        {
            return dal.CID();
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(int CID, long payCent, int payCount)
        {
            dal.Update2(CID, payCent, payCount);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateResult(int CID, int Result)
        {
            dal.UpdateResult(CID, Result);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
		{
			
			dal.Delete(id);
		}
        /// <summary>
        /// 初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.HcList GetHcList(int id)
		{
			
			return dal.GetHcList(id);
		}
        /// <summary>
        /// 得到机器人投注次数
        /// </summary>
        public int GetcountRebot(int usid)
        {
            return dal.GetcountRebot(usid);
        }

        /// <summary>
        /// 得到一个最新对象实体
        /// </summary>
        public BCW.Model.Game.HcList GetHcListNew(int State)
        {
            return dal.GetHcListNew(State);
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
		public DataSet GetList1(string strField, string strWhere)
        {
            return dal.GetList1(strField, strWhere);
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcList</returns>
        public IList<BCW.Model.Game.HcList> GetHcLists(int SizeNum, string strWhere)
        {
            return dal.GetHcLists(SizeNum, strWhere);
        }


		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList HcList</returns>
		public IList<BCW.Model.Game.HcList> GetHcLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetHcLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

