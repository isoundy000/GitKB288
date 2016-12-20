using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// 业务逻辑类Stkpay 的摘要说明。
    /// </summary>
    public class Stkpay
    {
        private readonly BCW.DAL.Game.Stkpay dal = new BCW.DAL.Game.Stkpay();
        public Stkpay()
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
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int StkId)
        {
            return dal.ExistsState(StkId);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int HorseId, int UsID, int bzType, int Types, decimal Odds)
        {
            return dal.Exists(HorseId, UsID, bzType, Types, Odds);
        }

        /// <summary>
        /// 计算某类型的投注额
        /// </summary>
        public int GetCent(int StkId, int Types)
        {
            return dal.GetCent(StkId, Types);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Stkpay model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.Stkpay model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State, int WinNum)
        {
            dal.Update(ID, WinCent, State, WinNum);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
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
        public BCW.Model.Game.Stkpay GetStkpay(int ID)
        {

            return dal.GetStkpay(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Stkpay GetStkpaybystkid(int StkId)
        {

            return dal.GetStkpaybystkid(StkId);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

        /// <summary>
        /// 得到一个bzType
        /// </summary>
        public int GetbzType(int ID)
        {
            return dal.GetbzType(ID);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetPayCentlast()
        {
            return dal.GetPayCentlast();
        }

        /// <summary>
        /// 得到一个WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
        }

        /// <summary>
        /// 得到一个WinCent5
        /// </summary>
        public long GetPayCentlast5()
        {
            return dal.GetPayCentlast5();
        }
        /// <summary>
        /// 得到一个WinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            return dal.GetWinCentlast5();
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }

        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }

        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int StkId)
        {
            return dal.GetSumPrices(UsID, StkId);
        }

        /// <summary>
        /// 某期某玩法某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int StkId, int Types)
        {
            return dal.GetSumPrices(UsID, StkId, Types);
        }

        /// <summary>
        /// 某期某投注方式共投了多少币
        /// </summary>
        public long GetSumPricesbytype(int Types, int StkId)
        {
            return dal.GetSumPricesbytype(Types, StkId);
        }

        /// <summary>
        /// 根据字段统计有多少条数据符合条件
        /// </summary>
        /// <param name="strWhere">统计条件</param>
        /// <returns>统计结果</returns>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        public int getState(int ID)
        {
            return dal.getState(ID);
        }

        /// <summary>
        /// 存在机器人
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int ID, int UsID)
        {
            return dal.ExistsReBot(ID, UsID);
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
        /// <returns>IList Stkpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStkpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStkpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

