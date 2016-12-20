using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// 业务逻辑类Favorites 的摘要说明。
	/// </summary>
	public class Favorites
	{
		private readonly Book.DAL.Favorites dal=new Book.DAL.Favorites();
		public Favorites()
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
        /// 是否存在书架记录
        /// </summary>
        public bool Exists2(int usid, int nid, int sid)
        {
            return dal.Exists2(usid, nid, sid);
        }

        /// <summary>
        /// 是否存在书签记录
        /// </summary>
        public bool Exists3(int usid, int nid, int sid, string purl)
        {
            return dal.Exists3(usid, nid, sid, purl);
        }

            
        /// <summary>
        /// 计算某书架收藏数量
        /// </summary>
        public int GetCount(int favid)
        {
            return dal.GetCount(favid);
        }
            
        /// <summary>
        /// 计算某用户书签收藏数量
        /// </summary>
        public int GetCount(int usid, int types)
        {
            return dal.GetCount(usid, types);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Book.Model.Favorites model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Book.Model.Favorites model)
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
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.Favorites GetFavorites(int id)
		{
			
			return dal.GetFavorites(id);
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
		/// <returns>IList Favorites</returns>
		public IList<Book.Model.Favorites> GetFavoritess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetFavoritess(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

