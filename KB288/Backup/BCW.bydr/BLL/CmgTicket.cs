using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// 业务逻辑类CmgTicket 的摘要说明。
	/// </summary>
	public class CmgTicket
	{
		private readonly BCW.bydr.DAL.CmgTicket dal=new BCW.bydr.DAL.CmgTicket();
		public CmgTicket()
		{}
		#region  成员方法



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
        public bool ExistsBID(int Bid)
        {
            return dal.ExistsBID(Bid);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.bydr.Model.CmgTicket model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.bydr.Model.CmgTicket model)
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
		/// 得到一个对象实体
		/// </summary>
		public BCW.bydr.Model.CmgTicket GetCmgTicket(int ID)
		{
			
			return dal.GetCmgTicket(ID);
		}
         /// <summary>
        /// 得到全部收集币
        /// </summary>
        public long GettoplistColletGoldsum()
        {
            return dal.GettoplistColletGoldsum();
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
		/// <returns>IList CmgTicket</returns>
		public IList<BCW.bydr.Model.CmgTicket> GetCmgTickets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmgTickets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

