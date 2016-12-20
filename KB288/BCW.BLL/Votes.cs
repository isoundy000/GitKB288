using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Votes 的摘要说明。
	/// </summary>
	public class Votes
	{
		private readonly BCW.DAL.Votes dal=new BCW.DAL.Votes();
		public Votes()
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
        /// 得到最大ID(系统发布)
        /// </summary>
        public int GetLastId()
        {
            return dal.GetLastId();
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
		public int  Add(BCW.Model.Votes model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Votes model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新投票
        /// </summary>
        public void UpdateVote(BCW.Model.Votes model)
        {
            dal.UpdateVote(model);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// 得到一个对象实体(帖子专用)
		/// </summary>
		public BCW.Model.Votes GetBbsVotes(int Types)
		{
			
			return dal.GetBbsVotes(Types);
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Votes GetVotes(int ID)
        {

            return dal.GetVotes(ID);
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
		/// <returns>IList Votes</returns>
		public IList<BCW.Model.Votes> GetVotess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVotess(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

