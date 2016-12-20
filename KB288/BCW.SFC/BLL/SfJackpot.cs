using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SFC.Model;
namespace BCW.SFC.BLL
{
    /// <summary>
    /// 业务逻辑类SfJackpot 的摘要说明。
    /// </summary>
    public class SfJackpot
    {
        private readonly BCW.SFC.DAL.SfJackpot dal = new BCW.SFC.DAL.SfJackpot();
        public SfJackpot()
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
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int CID)
        {
            return dal.Exists2(CID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int CID)
        {
            return dal.Exists3(CID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists4(int CID)
        {
            return dal.Exists4(CID);
        }
        /// <summary>
        /// 是否存在首期记录
        /// </summary>
        public bool Exists4()
        {
            return dal.Exists4();
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsgun(int CID)
        {
            return dal.Existsgun(CID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int usid)
        {
            return dal.Exists1(usid);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.SFC.Model.SfJackpot model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.SFC.Model.SfJackpot model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// 更新usid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usid"></param>
        public void UpdateusID(int id, int usid)
        {
            dal.UpdateusID(id, usid);
        }

        /// <summary>
        /// 更新预售期的奖池
        /// </summary>
        /// <param name="allmoney"></param>
        /// <param name="CID"></param>
        public void updateallmoney(long allmoney, int CID, int usID, int id)
        {
            dal.updateallmoney(allmoney, CID, usID, id);
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
        public BCW.SFC.Model.SfJackpot GetSfJackpot(int id)
        {

            return dal.GetSfJackpot(id);
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }
        /// <summary>
        /// 得到一个WinCent不根据时间
        /// </summary>
        public long GetWinCentall()
        {
            return dal.GetWinCentall();
        }
        /// <summary>
        /// 得到一个allmoney
        /// </summary>
        public long Getallmoney(int CID)
        {
            return dal.Getallmoney(CID);
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }
        /// <summary>
        /// 得到一个WinCent不跟据时间
        /// </summary>
        public long GetPayCentall()
        {
            return dal.GetPayCentall();
        }
        /// <summary>
        /// 系统总投注额
        /// </summary>
        /// <returns></returns>
        public long SysPrice(int CID)
        {
            return dal.SysPrice(CID);
        }
        /// <summary>
        /// 查询该期奖池
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long SysNowprize(int CID)
        {
            return dal.SysNowprize(CID);
        }
        /// <summary>
        /// 得到一个GetWinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
        }
        /// <summary>
        /// 得到一个GetWinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            return dal.GetWinCentlast5();
        }
        /// <summary>
        /// 系统总回收额
        /// </summary>
        /// <returns></returns>
        public long SysWin(int CID)
        {
            return dal.SysWin(CID);
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
        /// <returns>IList SfJackpot</returns>
        public IList<BCW.SFC.Model.SfJackpot> GetSfJackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetSfJackpots(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

