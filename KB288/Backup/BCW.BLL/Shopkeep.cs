using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Shopkeep 的摘要说明。
    /// </summary>
    public class Shopkeep
    {
        private readonly BCW.DAL.Shopkeep dal = new BCW.DAL.Shopkeep();
        public Shopkeep()
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int GiftId, int UsID)
        {
            return dal.Exists(GiftId, UsID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int ID, int UsID)
        {
            return dal.Exists1(ID, UsID);
        }
        /// <summary>
        /// 根据条件得到ID
        /// </summary>
        public int GetID(int GiftId, int UsID)
        {
            return dal.GetID(GiftId, UsID);
        }

        /// <summary>
        /// 计算某礼物-某用户今天购买数量
        /// </summary>
        public int GetTodayCount(int UsID, int GiftId)
        {
            return dal.GetTodayCount(UsID, GiftId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Shopkeep model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Shopkeep model)
        {
            dal.Update(model);
        }

        public void Update_ips(BCW.Model.Shopkeep model)
        {
            dal.Update_ips(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateTotal(int ID, int Total)
        {
            dal.UpdateTotal(ID, Total);
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
        public BCW.Model.Shopkeep GetShopkeep(int ID)
        {

            return dal.GetShopkeep(ID);
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
        /// <returns>IList Shopkeep</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeeps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShopkeeps(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeepsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShopkeepsTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeepsTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShopkeepsTop2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  成员方法
    }
}

