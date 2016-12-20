using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// 业务逻辑类BJL_user 的摘要说明。
    /// </summary>
    public class BJL_user
    {
        private readonly BCW.Baccarat.DAL.BJL_user dal = new BCW.Baccarat.DAL.BJL_user();
        public BJL_user()
        { }
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
        public int Add(BCW.Baccarat.Model.BJL_user model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_user model)
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
        public BCW.Baccarat.Model.BJL_user GetBJL_user(int ID)
        {

            return dal.GetBJL_user(ID);
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
        public bool Exists_user(int ID)
        {
            return dal.Exists_user(ID);
        }

        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_user GetBJL_setshow(int ID)
        {

            return dal.GetBJL_setshow(ID);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_user</returns>
        public IList<BCW.Baccarat.Model.BJL_user> GetBJL_users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBJL_users(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

