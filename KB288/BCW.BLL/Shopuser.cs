using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Shopuser 的摘要说明。
	/// </summary>
	public class Shopuser
	{
		private readonly BCW.DAL.Shopuser dal=new BCW.DAL.Shopuser();
		public Shopuser()
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
        public bool Exists(int UsID, int GiftId)
        {
            return dal.Exists(UsID, GiftId);
        }
        /// <summary>
        /// 是否存在该记录_农场
        /// </summary>
        public bool Exists_nc(int UsID, int GiftId)
        {
            return dal.Exists_nc(UsID, GiftId);
        }

        /// <summary>
        /// 计算某ID的礼物数
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }
        /// <summary>
        /// 邵广林 20160512
        /// 计算某ID的农场花数
        /// </summary>
        public int GetCount_nc(int UsID)
        {
            return dal.GetCount_nc(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(BCW.Model.Shopuser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Shopuser model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// 更新一条数据_农场标识pic=1更新  //邵广林 20160607
        /// </summary>
        public void Update_nc(BCW.Model.Shopuser model)
        {
            dal.Update_nc(model);
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
		public BCW.Model.Shopuser GetShopuser(int ID)
		{
			
			return dal.GetShopuser(ID);
		}
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Shopuser GetShopuser(int UsID, int GiftId)
        {

            return dal.GetShopuser(UsID, GiftId);
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
		/// <returns>IList Shopuser</returns>
		public IList<BCW.Model.Shopuser> GetShopusers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
            return dal.GetShopusers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

               
        /// <summary>
        /// 礼物达人榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopuser> GetShopusersTop(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetShopusersTop(p_pageIndex, p_pageSize, out p_recordCount);
        }

		#endregion  成员方法
	}
}

