using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_win 的摘要说明。
    /// </summary>
    public class NC_win
    {
        private readonly BCW.farm.DAL.NC_win dal = new BCW.farm.DAL.NC_win();
        public NC_win()
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
        public int Add(BCW.farm.Model.NC_win model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_win model)
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
        public BCW.farm.Model.NC_win GetNC_win(int ID)
        {
            return dal.GetNC_win(ID);
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
        /// me_随机得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_win GetNC_suiji(int ID)
        {
            return dal.GetNC_suiji(ID);
        }
        //===================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_win</returns>
        public IList<BCW.farm.Model.NC_win> GetNC_wins(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_wins(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

