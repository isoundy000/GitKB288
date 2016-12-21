using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
/// <summary>
/// 增加环迅支付接口
/// 
/// 黄国军 20160512
/// </summary>
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Payrmb 的摘要说明。
    /// </summary>
    public class Payrmb
    {
        private readonly BCW.DAL.Payrmb dal = new BCW.DAL.Payrmb();
        public Payrmb()
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
        public bool Exists(string CardOrder)
        {
            return dal.Exists(CardOrder);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Payrmb model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 充值返回更新标识
        /// </summary>
        /// <param name="model"></param>
        public void Update_ips(BCW.Model.Payrmb model)
        {
            dal.Update_ips(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Payrmb model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update3(BCW.Model.Payrmb model)
        {
            dal.Update3(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Payrmb model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// 得到用户ID
        /// </summary>
        public int GetUsID(string CardOrder)
        {

            return dal.GetUsID(CardOrder);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Payrmb GetPayrmb(int ID)
        {

            return dal.GetPayrmb(ID);
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
        /// <returns>IList Payrmb</returns>
        public IList<BCW.Model.Payrmb> GetPayrmbs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetPayrmbs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

