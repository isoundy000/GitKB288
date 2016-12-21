using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// 业务逻辑类ShuMu 的摘要说明。
	/// </summary>
	public class ShuMu
	{
		private readonly Book.DAL.ShuMu dal=new Book.DAL.ShuMu();
		public ShuMu()
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
        /// 是否存在某分类记录
        /// </summary>
        public bool ExistsNode(int nid)
        {
            return dal.ExistsNode(nid);
        }
               
        /// <summary>
        /// 计算未审核的书本数量
        /// </summary>
        public int GetCount(int nid)
        {
            return dal.GetCount(nid);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Book.Model.ShuMu model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Book.Model.ShuMu model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据(后台使用)
        /// </summary>
        public void Update2(Book.Model.ShuMu model)
        {
            dal.Update2(model);
        }
               
        /// <summary>
        /// 推荐书本
        /// </summary>
        public void UpdateGood(int id)
        {
            dal.UpdateGood(id);
        }
              
        /// <summary>
        /// 恢复和删除
        /// </summary>
        public void Updateisdel(int id, int isdel)
        {
            dal.Updateisdel(id, isdel);
        }

        /// <summary>
        /// 增加人气
        /// </summary>
        public void UpdateClick(int id)
        {
            dal.UpdateClick(id);
        }

        /// <summary>
        /// 审核书本
        /// </summary>
        public void Updatestate(int id, int state)
        {
            dal.Updatestate(id, state);
        }

        /// <summary>
        /// 写入更新时间
        /// </summary>
        public void Updategxtime(int id)
        {
            dal.Updategxtime(id);
        }
              
        /// <summary>
        /// 写入提醒ID
        /// </summary>
        public void Updategxids(int id, string gxids)
        {
            dal.Updategxids(id, gxids);
        }
                   
        /// <summary>
        /// 写入书评数目
        /// </summary>
        public void Updatepl(int id, int pl)
        {
            dal.Updatepl(id, pl);
        }

        /// <summary>
        /// 更新大类ID（转移用）
        /// </summary>
        public void Updatenid(int id, int nid)
        {
            dal.Updatenid(id, nid);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
        
        /// <summary>
        /// 得到大类ID
        /// </summary>
        public int Getnid(int id)
        {
            return dal.Getnid(id);
        }
             
        /// <summary>
        /// 得到提醒的会员ID
        /// </summary>
        public string Getgxids(int id)
        {
            return dal.Getgxids(id);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.ShuMu GetShuMu(int id)
		{
			
			return dal.GetShuMu(id);
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
		/// <returns>IList ShuMu</returns>
		public IList<Book.Model.ShuMu> GetShuMus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuMus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMus(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetShuMus(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
               
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMusTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShuMusTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

