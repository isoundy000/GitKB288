using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Collec;
namespace BCW.BLL.Collec
{
	/// <summary>
	/// 业务逻辑类CollecItem 的摘要说明。
	/// </summary>
	public class CollecItem
	{
		private readonly BCW.DAL.Collec.CollecItem dal=new BCW.DAL.Collec.CollecItem();
		public CollecItem()
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
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Collec.CollecItem model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Collec.CollecItem model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// 列表设置
        /// </summary>
        public void UpdateListSet(BCW.Model.Collec.CollecItem model)
        {
            dal.UpdateListSet(model);
        }
                
        /// <summary>
        /// 链接设置
        /// </summary>
        public void UpdateLinkSet(BCW.Model.Collec.CollecItem model)
        {
            dal.UpdateLinkSet(model);
        }
        
        /// <summary>
        /// 正文设置
        /// </summary>
        public void UpdateContentSet(BCW.Model.Collec.CollecItem model)
        {
            dal.UpdateContentSet(model);
        }
                
        /// <summary>
        /// 更新1可用/0不可用
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
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
		public BCW.Model.Collec.CollecItem GetCollecItem(int ID)
		{
			
			return dal.GetCollecItem(ID);
		}
                
        /// <summary>
        /// 得到一个WebEncode
        /// </summary>
        public int GetWebEncode(int ID)
        {
            return dal.GetWebEncode(ID);
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
		/// <returns>IList CollecItem</returns>
		public IList<BCW.Model.Collec.CollecItem> GetCollecItems(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCollecItems(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

