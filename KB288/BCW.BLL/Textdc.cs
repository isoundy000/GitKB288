using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Textdc 的摘要说明。
	/// </summary>
	public class Textdc
	{
		private readonly BCW.DAL.Textdc dal=new BCW.DAL.Textdc();
		public Textdc()
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
        /// 是否存在未结束的闲家记录
        /// </summary>
        public bool Exists2(int BID)
        {
            return dal.Exists2(BID);
        }

        /// <summary>
        /// 计算某帖子竞猜的c闲家保证金总额
        /// </summary>
        public long GetCents(int BID, int IsZtid)
        {
            return dal.GetCents(BID, IsZtid);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Textdc model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Textdc model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOutCent(BCW.Model.Textdc model)
        {
            dal.UpdateOutCent(model);
        }
              
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(int ID, int State, long AcCent)
        {
            dal.UpdateState(ID, State, AcCent);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        public void UpdateLogText(int BID, int UsID, string LogText)
        {
            dal.UpdateLogText(BID, UsID, LogText);
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
		public BCW.Model.Textdc GetTextdc(int ID)
		{
			
			return dal.GetTextdc(ID);
		}
               
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int BID, int IsZtid)
        {

            return dal.GetTextdc(BID, IsZtid);
        }
               
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int BID, int IsZtid, int UsID)
        {

            return dal.GetTextdc(BID, IsZtid, UsID);
        }
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc2(int ID, int BID)
        {

            return dal.GetTextdc2(ID, BID);
        }
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public string GetLogText(int BID)
        {
            return dal.GetLogText(BID);
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
		/// <returns>IList Textdc</returns>
		public IList<BCW.Model.Textdc> GetTextdcs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetTextdcs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

