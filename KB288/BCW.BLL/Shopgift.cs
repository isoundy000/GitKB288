using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Shopgift 的摘要说明。
    /// </summary>
    public class Shopgift
    {
        private readonly BCW.DAL.Shopgift dal = new BCW.DAL.Shopgift();
        public Shopgift()
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
        public bool Exists(int ID, int NodeId)
        {
            return dal.Exists(ID, NodeId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Shopgift model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Shopgift model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新购买量
        /// </summary>
        public void Update(int ID, int PayCount, int Total)
        {
            dal.Update(ID, PayCount, Total);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Shopgift GetShopgift(int ID)
        {

            return dal.GetShopgift(ID);
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Shopgift</returns>
        public IList<BCW.Model.Shopgift> GetShopgifts(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetShopgifts(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

