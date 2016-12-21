using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Toplist 的摘要说明。
	/// </summary>
	public class Toplist
	{
		private readonly BCW.DAL.Toplist dal=new BCW.DAL.Toplist();
		public Toplist()
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
        public bool Exists(int UsId, int Types)
        {
            return dal.Exists(UsId, Types);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Toplist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Toplist model)
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
        /// 清空某类型排行榜
        /// </summary>
        public void Clear(int Types)
        {

            dal.Clear(Types);
        }

        /// <summary>
        /// 清空全部排行榜
        /// </summary>
        public void Clear()
        {

            dal.Clear();
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Toplist GetToplist(int ID)
		{
			
			return dal.GetToplist(ID);
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
        /// <param name="strOrder">排序条件</param>
		/// <returns>IList Toplist</returns>
		public IList<BCW.Model.Toplist> GetToplists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetToplists(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

