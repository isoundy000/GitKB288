using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类GiftFlows 的摘要说明。
	/// </summary>
	public class GiftFlows
	{
		private readonly BCW.DAL.Game.GiftFlows dal=new BCW.DAL.Game.GiftFlows();
		public GiftFlows()
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
        public bool Exists(int Types, int UsID)
        {
            return dal.Exists(Types, UsID);
        }
 
        /// <summary>
        /// 是否存在该记录(规定数量)
        /// </summary>
        public bool Exists(int Types, int UsID, int Totall)
        {
            return dal.Exists(Types, UsID, Totall);
        }
                
        /// <summary>
        /// 是否存在该记录(N秒内)
        /// </summary>
        public bool ExistsSec(int Types, int UsID, int Sec)
        {
            return dal.ExistsSec(Types, UsID, Sec);
        }

        /// <summary>
        /// 计算不同物品有多少个类型
        /// </summary>
        public int GetTypesTotal(int UsID)
        {
            return dal.GetTypesTotal(UsID);
        }

        /// <summary>
        /// 计算某用户花的总量
        /// </summary>
        public int GetTotal(int UsID)
        {
            return dal.GetTotal(UsID);
        }

        /// <summary>
        /// 计算某用户花的剩余量
        /// </summary>
        public int GetTotall(int UsID)
        {
            return dal.GetTotall(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.GiftFlows model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.GiftFlows model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateTotall(int Types, int UsID, int Totall)
        {
           return dal.UpdateTotall(Types, UsID, Totall);
        }
               
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateTotall(int ID, int Totall)
        {
            return dal.UpdateTotall(ID, Totall);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Types,int ID)
		{
			
			dal.Delete(Types,ID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.GiftFlows GetGiftFlows(int Types,int ID)
		{
			
			return dal.GetGiftFlows(Types,ID);
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
		/// <returns>IList GiftFlows</returns>
		public IList<BCW.Model.Game.GiftFlows> GetGiftFlowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetGiftFlowss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
 
        /// <summary>
        /// 取到排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGiftFlowssTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取到排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGiftFlowssTop2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

