using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Network 的摘要说明。
	/// </summary>
	public class Network
	{
		private readonly BCW.DAL.Network dal=new BCW.DAL.Network();
		public Network()
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
        public bool Exists()
        {
            return dal.Exists();
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
        public bool Exists(int ID, int UsId)
        {
            return dal.Exists(ID, UsId);
        }
                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsGroupChat(int Types, int GroupId)
        {
            return dal.ExistsGroupChat(Types, GroupId);
        }
        
        /// <summary>
        /// 计算某用户今天广播数量
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Network model)
		{
			return dal.Add(model);
		}
               
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOnIDs(int ID, string OnIDs)
        {
            dal.UpdateOnIDs(ID, OnIDs);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOther(BCW.Model.Network model)
        {
            dal.UpdateOther(model);
        }
               
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateGroupChat(BCW.Model.Network model)
        {
            dal.UpdateGroupChat(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Network model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int ID, DateTime OverTime)
        {
            dal.Update(ID, OverTime);
        }
                
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateBasic(BCW.Model.Network model)
        {
            dal.UpdateBasic(model);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Delete()
        {

            dal.Delete();
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Network GetNetwork(int ID)
		{
			
			return dal.GetNetwork(ID);
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
		/// <returns>IList Network</returns>
		public IList<BCW.Model.Network> GetNetworks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetNetworks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

