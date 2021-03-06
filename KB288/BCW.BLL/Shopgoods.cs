using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Shop.Model;
namespace BCW.Shop.BLL
{
	/// <summary>
	/// 业务逻辑类Shopgoods 的摘要说明。
	/// </summary>
	public class Shopgoods
	{
		private readonly BCW.Shop.DAL.Shopgoods dal=new BCW.Shop.DAL.Shopgoods();
		public Shopgoods()
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
        public bool Existsgd(int ShopGiftId, int UsID)
        {
            return dal.Existsgd(ShopGiftId, UsID);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Shop.Model.Shopgoods model)
		{
			return dal.Add(model);
		}
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMessagebyid(int ID, string RealName, string Address, string Phone, string Email, string Message)
        {
            dal.UpdateMessagebyid(ID, RealName, Address, Phone, Email, Message);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMessagebyID(int ID, string Express, string Expressnum,DateTime SendTime)
        {
            dal.UpdateMessagebyID(ID, Express, Expressnum, SendTime);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateReceivebyID(int ID,DateTime ReceiveTime)
        {
            dal.UpdateReceivebyID(ID, ReceiveTime);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Shop.Model.Shopgoods model)
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
		/// 得到一个对象实体
		/// </summary>
		public BCW.Shop.Model.Shopgoods GetShopgoods(int ID)
		{
			
			return dal.GetShopgoods(ID);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Shop.Model.Shopgoods GetShopgoods1(int ShopGiftId)
        {

            return dal.GetShopgoods1(ShopGiftId);
        }
		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ID)
        {
            return dal.Exists(UsID, ID);
        }
		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Shopgoods</returns>
		public IList<BCW.Shop.Model.Shopgoods> GetShopgoodss(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			return dal.GetShopgoodss(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

