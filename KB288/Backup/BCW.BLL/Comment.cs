using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Comment 的摘要说明。
	/// </summary>
	public class Comment
	{
		private readonly BCW.DAL.Comment dal=new BCW.DAL.Comment();
		public Comment()
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
        /// 计算某会员ID发表的评论数
        /// </summary>
        public int GetCount(int UserId)
        {
            return dal.GetCount(UserId);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Comment model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Comment model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新回复内容
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            dal.UpdateReText(ID, ReText);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int DetailId)
        {

            dal.Delete2(DetailId);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete3(int NodeId)
        {

            dal.Delete3(NodeId);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到DetailId
        /// </summary>
        public int GetDetailId(int ID)
        {
            return dal.GetDetailId(ID);
        }
                       
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Comment GetCommentMe(int ID)
        {

            return dal.GetCommentMe(ID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int DetailId, int TopNum, int Types)
        {
            return dal.GetList(DetailId, TopNum, Types);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Comment> GetComments(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetComments(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  成员方法
	}
}

