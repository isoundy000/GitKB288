using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_WinnersLists 的摘要说明。
	/// </summary>
	public class tb_WinnersLists
	{
		private readonly BCW.DAL.tb_WinnersLists dal=new BCW.DAL.tb_WinnersLists();
		public tb_WinnersLists()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long Id)
		{
			return dal.Exists(Id);
		}
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
        public bool ExistsUserID(int ID)
        {
            return dal.ExistsUserID(ID);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.tb_WinnersLists model)
		{
			return dal.Add(model);
		}
        /// <summary>
        /// 得到每人每天获奖次数
        /// </summary>
        public int GetMaxCounts(int UserID)
        {
            return dal.GetMaxCounts(UserID);
        }
        /// <summary>
        /// 得到该用户上次数据的次数（isGet）字段的一行
        /// </summary>
        public BCW.Model.tb_WinnersLists GetLastIsGet(int Id)
        {
            return dal.GetLastIsGet(Id);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_WinnersLists model)
		{
			dal.Update(model);
		}
        /// <summary>
		/// 更新一条数据
		/// </summary>
        public void UpdateIdent(int ID,int Ident)
		{
            dal.UpdateIdent(ID,Ident);
		}
        

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(long Id)
		{
			
			dal.Delete(Id);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_WinnersLists Gettb_WinnersLists(long Id)
		{
			
			return dal.Gettb_WinnersLists(Id);
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
		/// <returns>IList tb_WinnersLists</returns>
		public IList<BCW.Model.tb_WinnersLists> Gettb_WinnersListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_WinnersListss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

