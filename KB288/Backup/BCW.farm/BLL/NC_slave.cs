using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_slave 的摘要说明。
    /// </summary>
    public class NC_slave
    {
        private readonly BCW.farm.DAL.NC_slave dal = new BCW.farm.DAL.NC_slave();
        public NC_slave()
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
        public int Add(BCW.farm.Model.NC_slave model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_slave model)
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
        public BCW.farm.Model.NC_slave GetNC_slave(int ID)
        {

            return dal.GetNC_slave(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //===================================
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_ziduan(string strField, string strWhere)
        {
            return dal.update_ziduan(strField, strWhere);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_slave GetNCslave(int meid, int usid)
        {

            return dal.GetNCslave(meid, usid);
        }
        /// <summary>
        /// me_是否存在该奴隶记录
        /// </summary>
        public bool Exists_nl(int slave_id, int usid)
        {
            return dal.Exists_nl(slave_id, usid);
        }
        /// <summary>
        /// me_是否存在该奴隶已经过期记录
        /// </summary>
        public bool Exists_nl2(int slave_id, int usid)
        {
            return dal.Exists_nl2(slave_id, usid);
        }
        /// <summary>
        /// me_更新奴隶信息
        /// </summary>
        public void Update_nl(BCW.farm.Model.NC_slave model)
        {
            dal.Update_nl(model);
        }
        //===================================



        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_slave</returns>
        public IList<BCW.farm.Model.NC_slave> GetNC_slaves(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_slaves(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

