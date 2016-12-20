using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// 业务逻辑类Contents 的摘要说明。
	/// </summary>
	public class Contents
	{
		private readonly Book.DAL.Contents dal=new Book.DAL.Contents();
		public Contents()
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
        /// 恢复和删除
        /// </summary>
        public void Updateisdel(int id, int isdel)
        {
            dal.Updateisdel(id, isdel);
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

        /// <summary>
        /// 计算未审核的章节数量
        /// </summary>
        public int GetCount(int shi)
        {
            return dal.GetCount(shi);
        }
        
        /// <summary>
        /// 计算某书本章节或分卷数量types(0单节/1分卷)
        /// </summary>
        public int GetCount(int shi, int jid, int types)
        {
            return dal.GetCount(shi, jid, types);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Book.Model.Contents model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Book.Model.Contents model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 审核章节
        /// </summary>
        public void Updatestate(int id, int state)
        {
            dal.Updatestate(id, state);
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
        /// 得到书卷名称
        /// </summary>
        public string GetTitle(int id)
        {
            return dal.GetTitle(id);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.Contents GetContents(int id)
		{
			
			return dal.GetContents(id);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 取得上(下)一条记录
        /// </summary>
        public Book.Model.Contents GetPreviousNextContents(int ID, int shi, int jid, bool p_next)
        {
            return dal.GetPreviousNextContents(ID, shi, jid, p_next);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Contents</returns>
		public IList<Book.Model.Contents> GetContentss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetContentss(p_pageIndex, p_pageSize, strWhere, "pid desc", out p_recordCount);
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Contents</returns>
        public IList<Book.Model.Contents> GetContentss(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetContentss(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

		#endregion  成员方法
	}
}

