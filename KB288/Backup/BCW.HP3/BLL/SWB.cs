using System;
using System.Data;
using System.Collections.Generic;
using BCW.HP3.Model;

namespace BCW.HP3.BLL
{
    /// <summary>
    /// SWB
    /// </summary>
    public partial class SWB
    {
        private readonly BCW.HP3.DAL.SWB dal = new BCW.HP3.DAL.SWB();
        public SWB()
        { }
        #region  BasicMethod

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
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(BCW.HP3.Model.SWB model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HP3.Model.SWB model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 由用户ID更新HP3钱币
        /// </summary>
        public void UpdateHP3Money(int UserID, long HP3Money)
        {
            dal.UpdateHP3Money(UserID,HP3Money);
        }
        /// <summary>
        /// 由用户ID更新HP3领钱次数
        /// </summary>
        public void UpdateHP3IsGet(int UserID,DateTime HP3IsGet)
        {
            dal.UpdateHP3IsGet(UserID,HP3IsGet);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserID)
        {

            return dal.Delete(UserID);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string UserIDlist)
        {
            return dal.DeleteList(UserIDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.SWB GetModel(int UserID)
        {

            return dal.GetModel(UserID);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得发奖数据列表
        /// </summary>
        public DataSet ReWardList(string strWhere1, string strWhere2, string strWhere3)
        {
            return dal.ReWardList(strWhere1,strWhere2,strWhere3);
        }
        /// <summary>
        /// 获取发奖记录总数
        /// </summary>
        public int ReWardCount(string strWhere1, string strWhere2, string strWhere3)
        {
            return dal.ReWardCount(strWhere1, strWhere2, strWhere3);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        #endregion  BasicMethod
    }
}