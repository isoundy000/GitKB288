using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.MobileSlider.Model;
namespace BCW.MobileSlider.BLL
{
	/// <summary>
	/// MobileSlider
	/// </summary>
	public partial class MobileSlider
	{
		private readonly BCW.MobileSlider.DAL.MobileSlider dal=new BCW.MobileSlider.DAL.MobileSlider();
		public MobileSlider()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.MobileSlider.Model.MobileSlider model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(BCW.MobileSlider.Model.MobileSlider model)
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
		public BCW.MobileSlider.Model.MobileSlider GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public BCW.MobileSlider.Model.MobileSlider GetModelByCache(int id)
		{
			
			string CacheKey = "MobileSliderModel-" + id;
			object objModel = BCW.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(id);
					if (objModel != null)
					{
						int ModelCache = BCW.Common.ConfigHelper.GetConfigInt("ModelCache");
						BCW.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (BCW.MobileSlider.Model.MobileSlider)objModel;
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
		public List<BCW.MobileSlider.Model.MobileSlider> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<BCW.MobileSlider.Model.MobileSlider> DataTableToList(DataTable dt)
		{
			List<BCW.MobileSlider.Model.MobileSlider> modelList = new List<BCW.MobileSlider.Model.MobileSlider>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				BCW.MobileSlider.Model.MobileSlider model;
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

