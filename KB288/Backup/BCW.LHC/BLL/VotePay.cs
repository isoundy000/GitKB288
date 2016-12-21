using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using LHC.Model;
namespace LHC.BLL
{
	/// <summary>
	/// 业务逻辑类VotePay 的摘要说明。
	/// </summary>
	public class VotePay
	{
		private readonly LHC.DAL.VotePay dal=new LHC.DAL.VotePay();
		public VotePay()
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
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
          
        /// <summary>
        /// 每特.码每期下注币数
        /// </summary>
        public long GetPayCent(int qiNo, int sNum)
        {
            return dal.GetPayCent(qiNo, sNum);
        }

        /// <summary>
        /// 每ID每期下注币数
        /// </summary>
        public long GetPayCent(int UsID, int qiNo, int bzType)
        {
            return dal.GetPayCent(UsID, qiNo, bzType);
        }
                
        /// <summary>
        /// 每期中奖币数
        /// </summary>
        public long GetwinCent(int qiNo, int bzType)
        {
            return dal.GetwinCent(qiNo, bzType);
        }
              
        /// <summary>
        /// 根据条件得到下注币数
        /// </summary>
        public long GetPayCent(string strWhere)
        {
            return dal.GetPayCent(strWhere);
        }
                
        /// <summary>
        /// 根据条件得到赢利币数
        /// </summary>
        public long GetwinCent(string strWhere)
        {
            return dal.GetwinCent(strWhere);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LHC.Model.VotePay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LHC.Model.VotePay model)
		{
			dal.Update(model);
		}
                       
        /// <summary>
        /// 更新该期为结束
        /// </summary>
        public void UpdateOver(int qiNo)
        {
            dal.UpdateOver(qiNo);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
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
		public LHC.Model.VotePay GetVotePay(int ID)
		{
			
			return dal.GetVotePay(ID);
		}
              
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

        /// <summary>
        /// 得到一个BzType
        /// </summary>
        public int GetBzType(int ID)
        {
            return dal.GetBzType(ID);
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
		/// <returns>IList VotePay</returns>
		public IList<LHC.Model.VotePay> GetVotePays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVotePays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

