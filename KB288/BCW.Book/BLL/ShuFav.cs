using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// 业务逻辑类ShuFav 的摘要说明。
	/// </summary>
	public class ShuFav
	{
		private readonly Book.DAL.ShuFav dal=new Book.DAL.ShuFav();
		public ShuFav()
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
        /// 计算某用户书架数量
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Book.Model.ShuFav model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Book.Model.ShuFav model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.ShuFav GetShuFav(int id)
		{
			
			return dal.GetShuFav(id);
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
		/// <returns>IList ShuFav</returns>
		public IList<Book.Model.ShuFav> GetShuFavs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuFavs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

