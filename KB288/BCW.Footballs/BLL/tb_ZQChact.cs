using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_ZQChact 的摘要说明。
	/// </summary>
	public class tb_ZQChact
	{
		private readonly BCW.DAL.tb_ZQChact dal=new BCW.DAL.tb_ZQChact();
		public tb_ZQChact()
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
		/// 发言是否大于设定秒数
		/// </summary>
		public int GetSecond(int UsId)
        {
            return dal.GetSecond(UsId);
        }
        /// <summary>
        /// 一场球赛的发言总量
        /// </summary>
        public int GetCountForId(int UsId)
        {
            return dal.GetCountForId(UsId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.tb_ZQChact model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_ZQChact model)
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
		public BCW.Model.tb_ZQChact Gettb_ZQChact(int ID)
		{
			
			return dal.Gettb_ZQChact(ID);
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
		/// <returns>IList tb_ZQChact</returns>
		public IList<BCW.Model.tb_ZQChact> Gettb_ZQChacts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_ZQChacts(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

