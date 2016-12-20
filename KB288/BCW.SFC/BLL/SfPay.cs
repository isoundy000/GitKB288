using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SFC.Model;
namespace BCW.SFC.BLL
{
    /// <summary>
    /// 业务逻辑类SfPay 的摘要说明。
    /// </summary>
    public class SfPay
    {
        private readonly BCW.SFC.DAL.SfPay dal = new BCW.SFC.DAL.SfPay();
        public SfPay()
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
        /// 得到最大ID
        /// </summary>
        public int GetMaxId(int usid)
        {
            return dal.GetMaxId(usid);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int CID)
        {
            return dal.Exists1(CID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int id, int IsPrize)
        {
            return dal.Exists1(id, IsPrize);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CID, int IsPrize)
        {
            return dal.Exists(CID, IsPrize);
        }
        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            return dal.ExistsState(id, UsID);
        }
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int id)
        {
            dal.UpdateState(id);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.SFC.Model.SfPay model)
        {
            dal.Add(model);
        }
        /// <summary>
        /// 得到中奖注数
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize(int CID, int IsPrize)
        {
            return dal.countPrize(CID, IsPrize);
        }
        /// <summary>
        /// 得到中奖的币
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Isprize"></param>
        /// <returns></returns>
        public long Gold(int CID, int Isprize)
        {
            return dal.Gold(CID, Isprize);
        }
        /// <summary>
        /// 获取投注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum(int ID, int CID)
        {
            return dal.VoteNum(ID, CID);
        }

        /// <summary>
        /// 获取中单式奖注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DanVoteNum(int i, int CID)
        {
            return dal.DanVoteNum(i, CID);
        }
        /// <summary>
        /// 总下注额
        /// </summary>
        /// <returns></returns>
        public long AllPrice()
        {
            return dal.AllPrice();
        }
        /// <summary>
        /// 总下注额
        /// </summary>
        /// <returns></returns>
        public long AllPrice(int CID)
        {
            return dal.AllPrice(CID);
        }

        public int GetMaxCID()
        {
            return dal.GetMaxCID();
        }
        /// <summary>
        /// 存在机器人
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int id, int usID)
        {
            return dal.ExistsReBot(id, usID);
        }
        ///机器人通过ID更新数据
        public bool RoBotByID(int id)
        {
            return dal.RoBotByID(id);
        }
        /// <summary>
        /// me_查询机器人购买次数
        /// </summary>
        public int GetSFCRobotCount(string strWhere)
        {
            return dal.GetSFCRobotCount(strWhere);
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList1(string strField)
        {
            return dal.GetList1(strField);
        }
        /// <summary>
        /// 总中奖额
        /// </summary>
        /// <returns></returns>
        public long AllWinCent()
        {
            return dal.AllWinCent();
        }
        public long AllWinCentbyCID(int CID)
        {
            return dal.AllWinCentbyCID(CID);
        }
        /// <summary>
        /// 获得每期下注总额
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long PayCents(int CID)
        {
            return dal.PayCents(CID);
        }
        /// <summary>
        /// 获得每期下注总额
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public int VoteNum(int CID)
        {
            return dal.VoteNum(CID);
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        /// <summary>
        /// 根据ID得到CID
        /// </summary>
        public int GetCID(int ID)
        {
            return dal.GetCID(ID);
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.SFC.Model.SfPay model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateChange(int id, string i)
        {
            dal.UpdateChange(id, i);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateResult(int id, string i)
        {
            dal.UpdateResult(id, i);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {
            dal.Delete(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.SFC.Model.SfPay GetSfPay(int id)
        {

            return dal.GetSfPay(id);
        }
        /// <summary>
        /// 得到一个WinCent5
        /// </summary>
        public long GetPayCentlast5()
        {
            return dal.GetPayCentlast5();
        }
        /// <summary>
        /// 得到一个WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
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
        public long GetPayCentlast()
        {
            return dal.GetPayCentlast();
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        public DataSet GetList(int CID)
        {
            return dal.GetList(CID);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SfPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSfPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSfPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SfPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSfPays1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetSfPays1(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSFPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSFPaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  成员方法
    }
}

