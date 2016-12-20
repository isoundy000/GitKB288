using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using TPR.Model.guess;
/// <summary>
/// 串串重开奖兑奖复位
/// 黄国军 20160715
/// </summary>
namespace TPR.BLL.guess
{
    /// <summary>
    /// 业务逻辑类Super 的摘要说明。
    /// </summary>
    public class Super
    {
        private readonly TPR.DAL.guess.Super dal = new TPR.DAL.guess.Super();
        public Super()
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
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsIsCase(int ID, int payusid)
        {
            return dal.ExistsIsCase(ID, payusid);
        }

        /// <summary>
        /// 更新用户兑奖
        /// </summary>
        public void UpdateIsCase(int ID)
        {
            dal.UpdateIsCase(ID);
        }

        public void UpdateNotCase(int ID)
        {
            dal.UpdateNotCase(ID);
        }
        /// <summary>
        /// 根据条件得到赛事下注总注数
        /// </summary>
        public int GetSuperCount(string strWhere)
        {
            return dal.GetSuperCount(strWhere);
        }

        /// <summary>
        /// 根据条件得到赛事下注总金额
        /// </summary>
        public long GetSuperpayCent(string strWhere)
        {
            return dal.GetSuperpayCent(strWhere);
        }

        /// <summary>
        /// 根据条件得到赛事下注盈利额
        /// </summary>
        public long GetSupergetMoney(string strWhere)
        {
            return dal.GetSupergetMoney(strWhere);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(TPR.Model.guess.Super model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(TPR.Model.guess.Super model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(TPR.Model.guess.Super model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update3(TPR.Model.guess.Super model)
        {
            dal.Update3(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update4(TPR.Model.guess.Super model)
        {
            dal.Update4(model);
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
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到一个ID
        /// </summary>
        public int GetSuperID(int UsID)
        {
            return dal.GetSuperID(UsID);
        }


        /// <summary>
        /// 得到一个BID集合
        /// </summary>
        public string GetSuperBID(int ID)
        {
            return dal.GetSuperBID(ID);
        }

        /// <summary>
        /// 得到一个getMoney
        /// </summary>
        public decimal GetgetMoney(int ID)
        {
            return dal.GetgetMoney(ID);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TPR.Model.guess.Super GetSuper(int ID)
        {

            return dal.GetSuper(ID);
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
        /// <returns>IList Super</returns>
        public IList<TPR.Model.guess.Super> GetSupers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSupers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

