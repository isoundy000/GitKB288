using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Panel 的摘要说明。
	/// </summary>
	public class Panel
	{
		private readonly BCW.DAL.Panel dal=new BCW.DAL.Panel();
		public Panel()
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
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Panel model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 更新横向竖向
        /// </summary>
        public void UpdateIsBr(int UsID, int IsBr)
        {
            dal.UpdateIsBr(UsID, IsBr);
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            dal.UpdatePaixu(ID, Paixu);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Panel model)
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
        /// 根据条件删除数据
        /// </summary>
        public void Delete(int UsID, string Title, string PUrl)
        {

            dal.Delete(UsID, Title, PUrl);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Panel GetPanel(int ID)
		{
			
			return dal.GetPanel(ID);
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
		/// <returns>IList Panel</returns>
		public IList<BCW.Model.Panel> GetPanels(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetPanels(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

