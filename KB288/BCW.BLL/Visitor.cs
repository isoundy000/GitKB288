using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Visitor 的摘要说明。
	/// </summary>
	public class Visitor
	{
		private readonly BCW.DAL.Visitor dal=new BCW.DAL.Visitor();
		public Visitor()
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
        public bool ExistsVisitId(int UsID, int VisitId)
        {
            return dal.ExistsVisitId(UsID, VisitId);
        }
                
        /// <summary>
        /// 计算今天人气
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Visitor model)
		{
			//return dal.Add(model);
            int ID = dal.Add(model);
            try
            {
                int usid = model.VisitId;
                string username = model.VisitName;
                string Notes = "探访友友";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                return ID;
            }
            catch { return ID; }
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Visitor model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int UsID, int VisitId)
        {
            dal.Update(UsID, VisitId);
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
		public BCW.Model.Visitor GetVisitor(int ID)
		{
			
			return dal.GetVisitor(ID);
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
		/// <returns>IList Visitor</returns>
		public IList<BCW.Model.Visitor> GetVisitors(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVisitors(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

