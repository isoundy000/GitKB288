﻿using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SSE.Model;
namespace BCW.SSE.BLL
{
	/// <summary>
	/// SsePrizePoolChang
	/// </summary>
	public partial class SsePrizePoolChang
	{
		private readonly BCW.SSE.DAL.SsePrizePoolChang dal=new BCW.SSE.DAL.SsePrizePoolChang();
		public SsePrizePoolChang()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.SSE.Model.SsePrizePoolChang model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(BCW.SSE.Model.SsePrizePoolChang model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.SSE.Model.SsePrizePoolChang GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public BCW.SSE.Model.SsePrizePoolChang GetModelByCache(int id)
		{
			
			string CacheKey = "SsePrizePoolChangModel-" + id;
			object objModel = BCW.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(id);
					if (objModel != null)
					{
                        int ModelCache = BCW.Common.ConfigHelper.GetConfigInt( "ModelCache" );
                        BCW.Common.DataCache.SetCache( CacheKey, objModel, DateTime.Now.AddMinutes( ModelCache ), TimeSpan.Zero );
					}
				}
				catch{}
			}
			return (BCW.SSE.Model.SsePrizePoolChang)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<BCW.SSE.Model.SsePrizePoolChang> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<BCW.SSE.Model.SsePrizePoolChang> DataTableToList(DataTable dt)
		{
			List<BCW.SSE.Model.SsePrizePoolChang> modelList = new List<BCW.SSE.Model.SsePrizePoolChang>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				BCW.SSE.Model.SsePrizePoolChang model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.SSE.Model.SsePrizePoolChang> GetSsePrizePoolChangePages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            return dal.GetSsePrizePoolChangePages( p_pageIndex, p_pageSize, strWhere, out p_recordCount );
        }


		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

        //获取奖池开奖前的池底金额

        public decimal GetBeforeOpenPrizeVal( int _sseNo )
        {
            DataSet _ds = GetList( 1, string.Format( "sseNo = {0} and  (operType=0 or operType=2)", _sseNo ), "id desc" );
            if (_ds.Tables[0].Rows.Count>0)
                return  decimal.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "totalMoney" ].ToString());
            return 0;
        }
             


		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

