using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Chatblack 的摘要说明。
	/// </summary>
	public class Chatblack
	{
		private readonly BCW.DAL.Chatblack dal=new BCW.DAL.Chatblack();
		public Chatblack()
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
        public bool Exists(int ChatId, int UsID)
        {
            return dal.Exists(ChatId, UsID);
        }
                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ChatId, int UsID, int Types)
        {
            return dal.Exists(ChatId, UsID, Types);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Chatblack model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Chatblack model)
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
		/// 删除一条数据
		/// </summary>
		public void DeleteStr(int ChatId)
		{
			
			dal.DeleteStr(ChatId);
		}

        /// <summary>
        /// 解除黑名单
        /// </summary>
        public void Delete(int ChatId, int UsID)
        {
            dal.Delete(ChatId, UsID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Chatblack GetChatblack(int ID)
		{
			
			return dal.GetChatblack(ID);
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
		/// <returns>IList Chatblack</returns>
		public IList<BCW.Model.Chatblack> GetChatblacks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetChatblacks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

