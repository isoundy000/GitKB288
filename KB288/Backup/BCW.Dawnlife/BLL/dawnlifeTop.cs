using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类dawnlifeTop 的摘要说明。
	/// </summary>
	public class dawnlifeTop
	{
		private readonly BCW.DAL.dawnlifeTop dal=new BCW.DAL.dawnlifeTop();
		public dawnlifeTop()
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
        public bool Existscoin(long coin)
        {
            return dal.Existscoin(coin);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.dawnlifeTop model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.dawnlifeTop model)
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
		public BCW.Model.dawnlifeTop GetdawnlifeTop(int ID)
		{
			
			return dal.GetdawnlifeTop(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        //更新一个字段
        public void Updatesum(int ID, int sum)
        {
            dal.Updatesum(ID, sum);
        }

        //更新一个字段
        public void Updatecum(int ID, int cum)
        {
            dal.Updatecum(ID, cum);
        }
        /// 根据查询影响的行数
        /// </summary>
        public int GetRowByUsID(int UsID, long coin,long money)
        {
            return dal.GetRowByUsID(UsID, coin, money);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList dawnlifeTop</returns>
		public IList<BCW.Model.dawnlifeTop> GetdawnlifeTops(int p_pageIndex, int p_pageSize, string strWhere,string strOrder,out int p_recordCount)
		{
			return dal.GetdawnlifeTops(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList dawnlifeTop</returns>
        public IList<BCW.Model.dawnlifeTop> GetdawnlifeTops1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int iSCounts, out int p_recordCount)
        {
            return dal.GetdawnlifeTops1(p_pageIndex, p_pageSize, strWhere, strOrder, iSCounts, out p_recordCount);
        }


		#endregion  成员方法
	}
}

