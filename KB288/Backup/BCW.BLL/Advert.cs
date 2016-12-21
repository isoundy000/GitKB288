using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Advert 的摘要说明。
	/// </summary>
	public class Advert
	{
		private readonly BCW.DAL.Advert dal=new BCW.DAL.Advert();
		public Advert()
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
        public bool Exists2(int ID)
        {
            return dal.Exists2(ID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Advert model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Advert model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// 更新点击
        /// </summary>
        public void UpdateClick(int ID)
        {
            dal.UpdateClick(ID);
        }

        /// <summary>
        /// 更新今天来访ID
        /// </summary>
        public void UpdateClickID(int ID)
        {
            dal.UpdateClickID(ID);
        }

        /// <summary>
        /// 更新今天来访ID
        /// </summary>
        public void UpdateClickID(int ID, string ClickID)
        {
            dal.UpdateClickID(ID, ClickID);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// 随机得到一个广告
        /// </summary>
        public BCW.Model.Advert GetAdvert()
        {
            return dal.GetAdvert();
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Advert GetAdvert(int ID)
		{
			
			return dal.GetAdvert(ID);
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
		/// <returns>IList Advert</returns>
		public IList<BCW.Model.Advert> GetAdverts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetAdverts(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

