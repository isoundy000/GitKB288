using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_market 的摘要说明。
    /// </summary>
    public class NC_market
    {
        private readonly BCW.farm.DAL.NC_market dal = new BCW.farm.DAL.NC_market();
        public NC_market()
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
        public int Add(BCW.farm.Model.NC_market model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_market model)
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
        public BCW.farm.Model.NC_market GetNC_market(int ID)
        {

            return dal.GetNC_market(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //===============================

        /// <summary>
        /// me_得到摆摊次数
        /// </summary>
        public long Get_btcs(int usid)
        {
            return dal.Get_btcs(usid);
        }
        /// <summary>
        /// me_是否存在摆摊记录
        /// </summary>
        public bool Exists_baitan(int id, int UsID)
        {
            return dal.Exists_baitan(id, UsID);
        }
        /// <summary>
        /// me_是否存在摆摊记录
        /// </summary>
        public bool Exists_baitan(int id)
        {
            return dal.Exists_baitan(id);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_market(string strField, string strWhere)
        {
            return dal.update_market(strField, strWhere);
        }
        /// <summary>
        /// me_更新摊位道具的数量
        /// </summary>
        public void Update_twdj(int usid, int num, int huafei_id)
        {
            dal.Update_twdj(usid, num, huafei_id);
        }
        /// <summary>
        /// me_判断道具是否为0
        /// </summary>
        public BCW.farm.Model.NC_market Get_djsl(int meid, int huafei_id)
        {

            return dal.Get_djsl(meid, huafei_id);
        }
        //===============================


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_market</returns>
        public IList<BCW.farm.Model.NC_market> GetNC_markets(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_markets(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

