using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Forumvote 的摘要说明。
	/// </summary>
	public class Forumvote
	{
		private readonly BCW.DAL.Forumvote dal=new BCW.DAL.Forumvote();
		public Forumvote()
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
        /// 某会员某论坛上次是否中奖
        /// </summary>
        public bool Exists(int ForumID, int BID, int UsID)
        {
            return dal.Exists(ForumID, BID, UsID);
        }

        /// <summary>
        /// 计算某论坛本月某用户中奖数量
        /// </summary>
        public int GetMonthCount(int ForumID, int BID, int UsID)
        {
            return dal.GetMonthCount(ForumID, BID, UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Forumvote model)
		{
			return dal.Add(model);
		}
                
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateNotes(int ID, string Notes)
        {
            dal.UpdateNotes(ID, Notes);
        }
        
        /// <summary>
        /// 更新开奖与是否中奖
        /// </summary>
        public void UpdateIsWin(int ID, int IsWin)
        {
            dal.UpdateIsWin(ID, IsWin);
        }
        
        /// <summary>
        /// 是否存在未开奖
        /// </summary>
        public bool ExistsKz()
        {
            return dal.ExistsKz();
        }
                           
        /// <summary>
        /// 某期是否存在未开奖
        /// </summary>
        public bool ExistsKz(int qiNum)
        {
            return dal.ExistsKz(qiNum);
        }

        /// <summary>
        /// 某期是否已开奖
        /// </summary>
        public bool ExistsKz(int ForumID, int qiNum)
        {
            return dal.ExistsKz(ForumID, qiNum);
        }

        /// <summary>
        /// 更新某论坛本期全部已开奖
        /// </summary>
        public void UpdateState(int qiNum, int ForumID, int state, string sNum)
        {
            dal.UpdateState(qiNum, ForumID, state, sNum);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Forumvote model)
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
        /// 得到某期开奖结果
        /// </summary>
        public string GetsNum(int ForumID, int qiNum)
        {
            return dal.GetsNum(ForumID, qiNum);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Forumvote GetForumvote(int ID)
		{
			
			return dal.GetForumvote(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 取得N条记录
        /// </summary>
        /// <param name="UsID">会员ID</param>
        /// <param name="BID">帖子ID</param>
        /// <param name="SizeNum">取N条</param>
        /// <returns></returns>
        public IList<BCW.Model.Forumvote> GetForumvotes(int UsID, int BID, int SizeNum)
        {
            return dal.GetForumvotes(UsID, BID, SizeNum);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Forumvote</returns>
		public IList<BCW.Model.Forumvote> GetForumvotes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetForumvotes(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

