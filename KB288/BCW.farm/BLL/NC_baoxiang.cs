using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_baoxiang 的摘要说明。
    /// </summary>
    public class NC_baoxiang
    {
        private readonly BCW.farm.DAL.NC_baoxiang dal = new BCW.farm.DAL.NC_baoxiang();
        public NC_baoxiang()
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
        public int Add(BCW.farm.Model.NC_baoxiang model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_baoxiang model)
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
        public BCW.farm.Model.NC_baoxiang GetNC_baoxiang(int ID)
        {

            return dal.GetNC_baoxiang(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //===========================================
        /// <summary>
        /// me_获得数据列表
        /// </summary>
        public List<BCW.farm.Model.NC_baoxiang> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList("*", strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// me_获得数据列表
        /// </summary>
        public List<BCW.farm.Model.NC_baoxiang> DataTableToList(DataTable dt)
        {
            List<BCW.farm.Model.NC_baoxiang> modelList = new List<BCW.farm.Model.NC_baoxiang>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.farm.Model.NC_baoxiang model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
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
        /// me_查询有几个道具
        /// </summary>
        public BCW.farm.Model.NC_baoxiang Get_num(int id)
        {
            return dal.Get_num(id);
        }
        /// <summary>
        /// me_判断是否存在种子或者道具id
        /// </summary>
        public bool Exists_bxzzdj(int ID, int type)
        {
            return dal.Exists_bxzzdj(ID, type);
        }
        //===========================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_baoxiang</returns>
        public IList<BCW.farm.Model.NC_baoxiang> GetNC_baoxiangs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_baoxiangs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

