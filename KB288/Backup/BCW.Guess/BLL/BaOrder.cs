using System;
using System.Data;
using System.Collections.Generic;
using TPR.Model.guess;
namespace TPR.BLL.guess
{
    /// <summary>
    /// 业务逻辑类BaOrder 的摘要说明。
    /// </summary>
    public class BaOrder
    {
        private readonly TPR.DAL.guess.BaOrder dal = new TPR.DAL.guess.BaOrder();
        public BaOrder()
        { }
        #region  成员方法

        /// <summary>
        /// 增加/更新排行榜
        /// </summary>
        public void UpdateOrder(TPR.Model.guess.BaOrder model)
        {
            dal.UpdateOrder(model);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr()
        {
            dal.DeleteStr();
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaOrderList(string strField, string strWhere)
        {
            return dal.GetBaOrderList(strField, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList</returns>
        public IList<TPR.Model.guess.BaOrder> GetBaOrders(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBaOrders(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}
