using System;
using System.Data;
using System.Collections.Generic;
using BCW.Model;
using BCW.Common;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Manage 的摘要说明。
	/// </summary>
	public class Manage
	{
		private readonly BCW.DAL.Manage dal=new BCW.DAL.Manage();
		public Manage()
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
        public bool ExistsKeys(string loginKeys)
        {
            return dal.ExistsKeys(loginKeys);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsUser(string sUser)
        {
            return dal.ExistsUser(sUser);
        }
               
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsUser(string sUser, int ID)
        {
            return dal.ExistsUser(sUser, ID);
        }

        /// <summary>
        /// 查询影响的行数
        /// </summary>
        /// <returns></returns>
        public int GetManageRow(BCW.Model.Manage model)
        {
            return dal.GetManageRow(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Manage model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新后台登录时间/ip
        /// </summary>
        public void UpdateTimeIP(BCW.Model.Manage model)
        {
            dal.UpdateTimeIP(model);
        }

        /// <summary>
        /// 更新后台Keys
        /// </summary>
        public void UpdateKeys(BCW.Model.Manage model)
        {
            dal.UpdateKeys(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Manage model)
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
        /// 根据ID取得管理员Keys
        /// </summary>
        public string GetKeys(int ID)
        {
            return dal.GetKeys(ID);
        }

        /// <summary>
        /// 根据ID取得管理员Keys
        /// </summary>
        public string GetKeys()
        {
            return dal.GetKeys();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Manage GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        /// 根据Keys得到一个对象实体
        /// </summary>
        public BCW.Model.Manage GetModelByKeys(string sKeys)
        {
            return dal.GetModelByKeys(sKeys);
        }

        /// <summary>
        /// 根据用户和密码得到一个对象实体
        /// </summary>
        public BCW.Model.Manage GetModelByModel(string sUser, string sPwd)
        {
            return dal.GetModelByModel(sUser, sPwd);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetManageList(string strField, string strWhere)
        {
            return dal.GetManageList(strField, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Manage</returns>
        public IList<BCW.Model.Manage> GetManages(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetManages(p_pageIndex, p_pageSize, out p_recordCount);
        }

		#endregion  成员方法
	}
}

