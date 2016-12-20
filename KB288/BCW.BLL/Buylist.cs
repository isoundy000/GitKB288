using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Buylist 的摘要说明。
    /// </summary>
    public class Buylist
    {
        private readonly BCW.DAL.Buylist dal = new BCW.DAL.Buylist();
        public Buylist()
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
        public bool Exists(int ID, int UserId)
        {
            return dal.Exists(ID, UserId);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UserId, int AcStats)
        {
            return dal.Exists(ID, UserId, AcStats);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Buylist model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Buylist model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新前台订单状态
        /// </summary>
        public void UpdateStats(BCW.Model.Buylist model)
        {
            dal.UpdateStats(model);
        }

        /// <summary>
        /// 更新用户订单
        /// </summary>
        public void UpdateBuy(BCW.Model.Buylist model)
        {
            dal.UpdateBuy(model);
        }

        /// <summary>
        /// 更新后台订单状态
        /// </summary>
        public void UpdateMStats(BCW.Model.Buylist model)
        {
            dal.UpdateMStats(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int GoodsId)
        {

            dal.Delete2(GoodsId);
        }
                
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete3(int NodeId)
        {

            dal.Delete3(NodeId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Buylist GetBuylist(int ID)
        {

            return dal.GetBuylist(ID, 0);
        }

        /// <summary>
        /// 得到一个实体
        /// </summary>
        public BCW.Model.Buylist GetBuylistMe(int ID)
        {
            return dal.GetBuylistMe(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Buylist GetBuylist(int ID, int UserId)
        {

            return dal.GetBuylist(ID, UserId);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int GoodsId, int TopNum)
        {
            return dal.GetList(GoodsId, TopNum);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Buylist</returns>
        public IList<BCW.Model.Buylist> GetBuylists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBuylists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}
