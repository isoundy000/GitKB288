using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// 业务逻辑类ShuSelf 的摘要说明。
	/// </summary>
	public class ShuSelf
	{
		private readonly Book.DAL.ShuSelf dal=new Book.DAL.ShuSelf();
		public ShuSelf()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(Book.Model.ShuSelf model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Book.Model.ShuSelf model)
		{
			dal.Update(model);
		}
             
        /// <summary>
        /// 更新默认字数
        /// </summary>
        public void UpdatePageNum(int aid, int pagenum)
        {
            dal.UpdatePageNum(aid, pagenum);
        }
              
        /// <summary>
        /// 写入提醒ID
        /// </summary>
        public void Updategxids(int aid, string gxids)
        {
            dal.Updategxids(aid, gxids);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
              
        /// <summary>
        /// 得到默认字数
        /// </summary>
        public int GetPageNum(int aid)
        {
            return dal.GetPageNum(aid);
        }
          
        /// <summary>
        /// 得到提醒的书本ID
        /// </summary>
        public string Getgxids(int aid)
        {
            return dal.Getgxids(aid);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.ShuSelf GetShuSelf(int aid)
		{
			
			return dal.GetShuSelf(aid);
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
		/// <returns>IList ShuSelf</returns>
		public IList<Book.Model.ShuSelf> GetShuSelfs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuSelfs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

