using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类klsflist 的摘要说明。
	/// </summary>
	public class klsflist
	{
		private readonly BCW.DAL.klsflist dal=new BCW.DAL.klsflist();
		public klsflist()
		{}
		#region  成员方法
        // <summary>
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
        /// 是否存在该期
        /// </summary>
        public bool ExistsklsfId(int klsfId)
        {
            return dal.ExistsklsfId(klsfId);
        }

        /// <summary>
        /// 是否存在要更新结果的记录
        /// </summary>
        public bool ExistsUpdateResult()
        {
            return dal.ExistsUpdateResult();
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.klsflist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.klsflist model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新开奖数据
        /// </summary>
        public void UpdateResult(string klsfId, string Result)
        {
            dal.UpdateResult(klsfId, Result);
        }
        /// <summary>
        /// me_初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
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
		public BCW.Model.klsflist Getklsflist(int ID)
		{
			
			return dal.Getklsflist(ID);
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.klsflist Getklsflistbyklsfid(int klsfId)
        {

            return dal.Getklsflistbyklsfid(klsfId);
        }
        /// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Model.klsflist GetklsflistLast()
        {
            return dal.GetklsflistLast();
        }

        /// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Model.klsflist GetklsflistLast2()
        {
            return dal.GetklsflistLast2();
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
        /// 取得固定列表记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList klsflist</returns>
		public IList<BCW.Model.klsflist> Getklsflists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getklsflists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
		/// 取得记录
		/// </summary>
        /// <param name="SizeNum">返回记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList klsflist</returns>
        public IList<BCW.Model.klsflist> Getklsflists(int SizeNum, string strWhere)
        {
            return dal.Getklsflists(SizeNum, strWhere);
        }

		#endregion  成员方法
	}
}

