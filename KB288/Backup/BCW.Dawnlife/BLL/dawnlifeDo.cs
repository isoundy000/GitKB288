using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类dawnlifeDo 的摘要说明。
	/// </summary>
	public class dawnlifeDo
	{
		private readonly BCW.DAL.dawnlifeDo dal=new BCW.DAL.dawnlifeDo();
		public dawnlifeDo()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.dawnlifeDo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.dawnlifeDo model)
		{
			dal.Update(model);
		}
        public void UpdateStock(int ID, int stock)
        {
            dal.UpdateStock(ID, stock);
        }
        /// 根据查询影响的行数
        /// </summary>
        public int GetRowByUsID(int UsID, long coin)
        {
            return dal.GetRowByUsID(UsID, coin);
        }
        /// 根据查询影响的行数
        /// </summary>
        public int GetByUsID(int UsID)
        {
            return dal.GetByUsID(UsID);
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
		public BCW.Model.dawnlifeDo GetdawnlifeDo(int ID)
		{
			
			return dal.GetdawnlifeDo(ID);
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
		/// <returns>IList dawnlifeDo</returns>
		public IList<BCW.Model.dawnlifeDo> GetdawnlifeDos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetdawnlifeDos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

