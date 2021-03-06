﻿using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SSE.Model;
namespace BCW.SSE.BLL
{
	/// <summary>
	/// SseFastVal
	/// </summary>
	public partial class SseFastVal
	{
		private readonly BCW.SSE.DAL.SseFastVal dal=new BCW.SSE.DAL.SseFastVal();
		public SseFastVal()
		{}
		#region  BasicMethod

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
		public bool Exists(int userId)
		{
			return dal.Exists(userId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(BCW.SSE.Model.SseFastVal model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(BCW.SSE.Model.SseFastVal model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int userId)
		{
			
			return dal.Delete(userId);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.SSE.Model.SseFastVal GetModel(int userId)
		{
			
			return dal.GetModel(userId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public BCW.SSE.Model.SseFastVal GetModelByCache(int userId)
		{
			
			string CacheKey = "SseFastValModel-" + userId;
            object objModel = BCW.Common.DataCache.GetCache( CacheKey );
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(userId);
					if (objModel != null)
					{
                        int ModelCache = BCW.Common.ConfigHelper.GetConfigInt( "ModelCache" );
                        BCW.Common.DataCache.SetCache( CacheKey, objModel, DateTime.Now.AddMinutes( ModelCache ), TimeSpan.Zero );
					}
				}
				catch{}
			}
			return (BCW.SSE.Model.SseFastVal)objModel;
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
		public List<BCW.SSE.Model.SseFastVal> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<BCW.SSE.Model.SseFastVal> DataTableToList(DataTable dt)
		{
			List<BCW.SSE.Model.SseFastVal> modelList = new List<BCW.SSE.Model.SseFastVal>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				BCW.SSE.Model.SseFastVal model;
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
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

