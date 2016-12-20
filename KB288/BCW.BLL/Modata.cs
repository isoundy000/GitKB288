using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Modata 的摘要说明。
	/// </summary>
	public class Modata
	{
		private readonly BCW.DAL.Modata dal=new BCW.DAL.Modata();
		public Modata()
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
        /// 得到最大Types
        /// </summary>
        public int GetMaxTypes()
        {
            return dal.GetMaxTypes();
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
        public bool Exists2(int Types)
        {
            return dal.Exists2(Types);
        }

        /// <summary>
        /// 得到总记录数
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Model.Modata model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Modata model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 增加选定次数
        /// </summary>
        public void UpdatePhoneClick(int ID)
        {
            dal.UpdatePhoneClick(ID);
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
		public BCW.Model.Modata GetModata(int ID)
		{
			
			return dal.GetModata(ID);
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Modata GetModata2(string Model)
        {

            return dal.GetModata2(Model);
        }
     
        /// <summary>
        /// 得到一个Types
        /// </summary>
        public int GetTypes(string Brand)
        {
            return dal.GetTypes(Brand);
        }
               
        /// <summary>
        /// 根据Types得到一个Brand
        /// </summary>
        public string GetPhoneBrand(int Types)
        {
            return dal.GetPhoneBrand(Types);
        }

        /// <summary>
        /// 根据Types得到一个Model
        /// </summary>
        public string GetPhoneModel(int Types)
        {
            return dal.GetPhoneModel(Types);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 取得品牌记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Modata</returns>
        public IList<BCW.Model.Modata> GetBrand(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBrand(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Modata</returns>
		public IList<BCW.Model.Modata> GetModatas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetModatas(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

