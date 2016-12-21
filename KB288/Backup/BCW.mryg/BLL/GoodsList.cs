using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类GoodsList 的摘要说明。
	/// </summary>
	public class GoodsList
	{
		private readonly BCW.DAL.GoodsList dal=new BCW.DAL.GoodsList();
		public GoodsList()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long Id)
		{
			return dal.Exists(Id);
		}
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }
          /// <summary>
        /// 通过商品编号获取商品statue
        /// </summary>
        ///  
         public string Getstatue(long Id)
        {
            return dal.Getstatue(Id);
        }
        /// <summary>
        /// 通过商品编号获取商品名称
        /// </summary>
        ///  
       public string GetGoodsName(long Id)
        {
            return dal.GetGoodsName(Id);
        }
       /// <summary>
       /// 通过商品编号获取商品名称
       /// </summary>
       ///  
       public long Getperiods(long Id)
       {
           return dal.Getperiods(Id);
       }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.GoodsList model)
		{
			return dal.Add(model);
		}
              /// <summary>
        /// 更新一个数据
        /// </summary>
        public void UpdateGoodsType(long Id, int GoodsType)
        {
            dal.UpdateGoodsType(Id, GoodsType);
        }
         /// <summary>
        /// 更新一个数据
        /// </summary>
        public void UpdateisDone(long Id, int isDone)
        {
            dal.UpdateisDone(Id,isDone);
        }
        /// <summary>
        /// 更新一个数据
        /// </summary>
        public void UpdateNum(long Id,long number)
        {
            dal.UpdateNum(Id,number);
        }
        /// <summary>
        /// 更新一个数据
        /// </summary>
        public void UpdateYunGouMa(long Id, string StockYungouma)
        {
            dal.UpdateYunGouMa(Id, StockYungouma);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.GoodsList model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(long Id)
		{
			
			dal.Delete(Id);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.GoodsList GetGoodsList(long Id)
		{

            return dal.GetGoodsList(Id);
		}
        public string GetGoodsStatue(int Id)
        {
            return dal.Getstatue(Id);
        }
		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        /// <summary>
        /// 取得每页开奖记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList GoodsList</returns>
        public IList<BCW.Model.GoodsList> GetGoodsListsForGoodsOpen(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGoodsListsForGoodsOpen(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList GoodsList</returns>
		public IList<BCW.Model.GoodsList> GetGoodsLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetGoodsLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

