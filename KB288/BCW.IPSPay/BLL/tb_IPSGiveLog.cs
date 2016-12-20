/**  版本信息模板在安装目录下，可自行修改。
* tb_IPSGiveLog.cs
*
* 功 能： N/A
* 类 名： tb_IPSGiveLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/7/16 14:57:17   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using BCW.IPSPay.Model;
namespace BCW.IPSPay.BLL
{
	/// <summary>
	/// tb_IPSGiveLog
	/// </summary>
	public partial class tb_IPSGiveLog
	{
		private readonly BCW.IPSPay.DAL.tb_IPSGiveLog dal=new BCW.IPSPay.DAL.tb_IPSGiveLog();
		public tb_IPSGiveLog()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.IPSPay.Model.tb_IPSGiveLog model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(BCW.IPSPay.Model.tb_IPSGiveLog model)
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
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.IPSPay.Model.tb_IPSGiveLog GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		public double GetTotal(int G_type)
        {
            return dal.GetTotal(G_type);
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
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList GetIPSGiveLogs</returns>
		public IList<BCW.IPSPay.Model.tb_IPSGiveLog> GetIPSGiveLogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetIPSGiveLogs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.IPSPay.Model.tb_IPSGiveLog> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<BCW.IPSPay.Model.tb_IPSGiveLog> DataTableToList(DataTable dt)
		{
			List<BCW.IPSPay.Model.tb_IPSGiveLog> modelList = new List<BCW.IPSPay.Model.tb_IPSGiveLog>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				BCW.IPSPay.Model.tb_IPSGiveLog model;
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

