using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
namespace BCW.ssc.BLL
{
	/// <summary>
	/// 业务逻辑类SSClist 的摘要说明。
	/// </summary>
	public class SSClist
	{
		private readonly BCW.ssc.DAL.SSClist dal=new BCW.ssc.DAL.SSClist();
		public SSClist()
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
        /// 是否存在该期
        /// </summary>
        public bool ExistsSSCId(int SSCId)
        {
            return dal.ExistsSSCId(SSCId);
        }

        /// <summary>
        /// 是否存在该期且未更新开奖
        /// </summary>
        public bool ExistsSSCId(int SSCId,int k)
        {
            return dal.ExistsSSCId(SSCId,k);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistbySSCId(int SSCId)
        {

            return dal.GetSSClistbySSCId(SSCId);
        }

        /// <summary>
        /// 根据期号得到id
        /// </summary>
        /// <param name="SSCId"></param>
        /// <returns></returns>
        public int id(int SSCId)
         {
        return dal.id(SSCId);
         }

        /// <summary>
        /// 根据期号得到开奖类型和开奖时间
        /// </summary>
        /// <param name="SSCId"></param>
        /// <returns></returns>
        public string GetStateTime(int SSCId)
        {
            return dal.GetStateTime(SSCId);
        }
                
        /// <summary>
        /// 是否存在要更新结果的记录
        /// </summary>
        public bool ExistsUpdateResult()
        {
            return dal.ExistsUpdateResult();
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.ssc.Model.SSClist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.ssc.Model.SSClist model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新开奖结果
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            dal.UpdateResult(SSCId, Result);
        }

        /// <summary>
        /// 更新开奖类型和开奖时间
        /// </summary>
        public void UpdateStateTime(int SSCId, string Result)
        {
            dal.UpdateStateTime(SSCId, Result);
        }

        /// <summary>
        /// 更新开奖结果
        /// </summary>
        public void UpdateResult1(int SSCId, string Result)
        {
            dal.UpdateResult1(SSCId, Result);
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
		public BCW.ssc.Model.SSClist GetSSClist(int ID)
		{
			
			return dal.GetSSClist(ID);
		}

        
        /// <summary>
        /// 得到最后一期对象实体
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistLast()
        {

            return dal.GetSSClistLast();
        }
                
        /// <summary>
        /// 得到上期开奖
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistLast2()
        {
            return dal.GetSSClistLast2();
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
               
        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSClist</returns>
        public IList<BCW.ssc.Model.SSClist> GetSSClists(int SizeNum, string strWhere)
        {
            return dal.GetSSClists(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList SSClist</returns>
		public IList<BCW.ssc.Model.SSClist> GetSSClists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSSClists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

