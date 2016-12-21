using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JS.Model;
namespace BCW.JS.BLL
{
	/// <summary>
	/// 业务逻辑类bossrobot 的摘要说明。
	/// </summary>
	public class bossrobot
	{
		private readonly BCW.JS.DAL.bossrobot dal=new BCW.JS.DAL.bossrobot();
		public bossrobot()
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
		public int  Add(BCW.JS.Model.bossrobot model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.JS.Model.bossrobot model)
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
		public BCW.JS.Model.bossrobot Getbossrobot(int ID)
		{
			
			return dal.Getbossrobot(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// me_得到设置机器人ID的总个数
        /// </summary>
        public int Get_num()
        {
            return dal.Get_num();
        }
        /// <summary>
        /// me_得到设置游戏的总个数
        /// </summary>
        public int Get_yx()
        {
            return dal.Get_yx();
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList bossrobot</returns>
        public IList<BCW.JS.Model.bossrobot> Getbossrobots(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getbossrobots(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

