using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using LHC.Model;
namespace LHC.BLL
{
	/// <summary>
	/// 业务逻辑类VoteNo49 的摘要说明。
	/// </summary>
	public class VoteNo49
	{
		private readonly LHC.DAL.VoteNo49 dal=new LHC.DAL.VoteNo49();
		public VoteNo49()
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
		public int  Add(LHC.Model.VoteNo49 model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LHC.Model.VoteNo49 model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int qiNo, long payCent, int payCount)
        {
            dal.Update(qiNo, payCent, payCount);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(int qiNo, long payCent2, int payCount2)
        {
            dal.Update2(qiNo, payCent2, payCount2);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOpen(LHC.Model.VoteNo49 model)
        {
            dal.UpdateOpen(model);
        }
   
        /// <summary>
        /// 更新该期为结束
        /// </summary>
        public void UpdateState(int qiNo)
        {
            dal.UpdateState(qiNo);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
               
        /// <summary>
        /// 得到一个qiNo
        /// </summary>
        public int GetqiNo(int ID)
        {
            return dal.GetqiNo(ID);
        }
                
        /// <summary>
        /// 得到一个payCount
        /// </summary>
        public int GetpayCount(int ID)
        {
            return dal.GetpayCount(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LHC.Model.VoteNo49 GetVoteNo49(int ID)
		{
			
			return dal.GetVoteNo49(ID);
		}
                
        /// <summary>
        /// 得到一个最新实体
        /// </summary>
        public LHC.Model.VoteNo49 GetVoteNo49New(int State)
        {
            return dal.GetVoteNo49New(State);
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
		/// <returns>IList VoteNo49</returns>
		public IList<LHC.Model.VoteNo49> GetVoteNo49s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVoteNo49s(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

