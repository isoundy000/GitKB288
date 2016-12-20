using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类yg_BuyLists 的摘要说明。
	/// </summary>
	public class yg_BuyLists
	{
		private readonly BCW.DAL.yg_BuyLists dal=new BCW.DAL.yg_BuyLists();
		public yg_BuyLists()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long Id)
		{
			return dal.Exists(Id);
		}
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }
        /// <summary>
        /// 通过Id获得UserId
        /// </summary>
        public long GetUserId(long Id)
        {
            return dal.GetUserId(Id);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.yg_BuyLists model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.yg_BuyLists model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// 更新一Address==1
        /// </summary>
        public void UpdateAddress(long Id,string address)
        {
            dal.UpdateAddress(Id,address);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(long Id)
		{
			
			dal.Delete(Id);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.yg_BuyLists Getyg_BuyLists(long Id)
		{
			
			return dal.Getyg_BuyLists(Id);
		}
        /// <summary>
        /// 得到某goods的最新购买行
        /// </summary>
        public BCW.Model.yg_BuyLists Getyg_BuyListsForGoods(long Id)
        {

            return dal.Getyg_BuyListsForGoods(Id);
        }
        /// <summary>
        /// 得到某goods是否存在云购码
        /// </summary>
        public bool Getyg_BuyListsForYungouma(int Id, int GoodsNum)//
        {
            return dal.Getyg_BuyListsForYungouma(Id, GoodsNum);
        }
        /// <summary>
        /// //通过云购码判断返回列
        /// </summary>  
        public long GetUserId_yg_BuyListsForYungouma(int Id, int GoodsNum)
        {
            return dal.GetUserId_yg_BuyListsForYungouma(Id, GoodsNum);
        }
		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetListTop(string strField, string strWhere)
        {
            return dal.GetListTop(strField, strWhere);
        }
        /// <summary>
        /// 根据Id取此Id前的100条记录
        /// </summary>
        public DataSet GetListTopId(long Id)
        {
            return dal.GetListTopId(Id);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList yg_BuyLists</returns>
		public IList<BCW.Model.yg_BuyLists> Getyg_BuyListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getyg_BuyListss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}


		#endregion  成员方法
	}
}

