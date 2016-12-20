using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Contact 的摘要说明。
	/// </summary>
	public class Contact
	{
		private readonly BCW.DAL.Contact dal=new BCW.DAL.Contact();
		public Contact()
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
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        public bool Exists2(int NodeId, int UsID)
        {
            return dal.Exists2(NodeId, UsID);
        }

        /// <summary>
        /// 计算某分组联系人数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Contact model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Contact model)
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
        /// 得到NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Contact GetContact(int ID)
		{
			
			return dal.GetContact(ID);
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
		/// <returns>IList Contact</returns>
		public IList<BCW.Model.Contact> GetContacts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetContacts(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

