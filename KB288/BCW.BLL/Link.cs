using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Link 的摘要说明。
	/// </summary>
	public class Link
	{
		private readonly BCW.DAL.Link dal=new BCW.DAL.Link();
		public Link()
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
        public bool Exists(string LinkUrl)
        {
            return dal.Exists(LinkUrl);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Model.Link model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Link model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReLastIP)
        {
            dal.UpdateReStats(ID, ReStats, ReLastIP);
        }

        /// <summary>
        /// 更新链入
        /// </summary>
        public void UpdateLinkIn(int ID)
        {
            dal.UpdateLinkIn(ID);
        }

        /// <summary>
        /// 更新链出
        /// </summary>
        public void UpdateLinkOut(int ID)
        {
            dal.UpdateLinkOut(ID);
        }

        /// <summary>
        /// 更新链出
        /// </summary>
        public void UpdateHidden(int ID)
        {
            dal.UpdateHidden(ID);
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
        public BCW.Model.Link GetLink(int ID)
        {

            return dal.GetLink(ID);
        }

        /// <summary>
        /// 得到友链全称
        /// </summary>
        public string GetLinkName(int ID)
        {

            return dal.GetLinkName(ID);
        }

		/// <summary>
		/// 得到友链地址
		/// </summary>
		public string GetLinkUrl(int ID)
		{
			
			return dal.GetLinkUrl(ID);
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int TopNum, string strWhere)
        {
            return dal.GetList(TopNum, strWhere);
        }

        /// <summary>
        /// 获得数据列表
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
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Link> GetLinks(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetLinks(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

		#endregion  成员方法
	}
}

