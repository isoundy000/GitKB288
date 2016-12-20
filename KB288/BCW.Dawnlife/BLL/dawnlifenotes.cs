using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类dawnlifenotes 的摘要说明。
	/// </summary>
	public class dawnlifenotes
	{
		private readonly BCW.DAL.dawnlifenotes dal=new BCW.DAL.dawnlifenotes();
		public dawnlifenotes()
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
		public int  Add(BCW.Model.dawnlifenotes model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.dawnlifenotes model)
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
		public BCW.Model.dawnlifenotes Getdawnlifenotes(int ID)
		{
			
			return dal.Getdawnlifenotes(ID);
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
		/// <returns>IList dawnlifenotes</returns>
		public IList<BCW.Model.dawnlifenotes> Getdawnlifenotess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getdawnlifenotess(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        public IList<BCW.Model.dawnlifenotes> Getdawnlifenotess2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder,out int p_recordCount)
        {
            return dal.Getdawnlifenotess2(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
        }

		#endregion  成员方法
	}
}

