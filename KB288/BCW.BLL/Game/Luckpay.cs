using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// 业务逻辑类Luckpay 的摘要说明。
    /// </summary>
    public class Luckpay
    {
        private readonly BCW.DAL.Game.Luckpay dal = new BCW.DAL.Game.Luckpay();
        public Luckpay()
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
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int LuckId)
        {
            return dal.ExistsState(LuckId);
        }
          /// <summary>
        /// 得到机器人投注次数
        /// </summary>
        public int GetRobotbuy(int usid,int luckid)
        {
            return dal.GetRobotbuy(usid,luckid);
        }
        /// <summary>
        /// 计算某期购买幸运数字的总币数
        /// </summary>
        public long GetSumBuyCent(int LuckId, string BuyNum)
        {
            return dal.GetSumBuyCent(LuckId, BuyNum);
        }
        /// <summary>
        /// 计算某期购买自选幸运数字的总币数
        /// </summary>
        public long GetSumBuyCentbychoose(int LuckId, string BuyNum)
        {
            return dal.GetSumBuyCentbychoose(LuckId, BuyNum);
        }
         /// <summary>
         /// 计算某期购买某类型的总币数
         /// </summary>
        public long GetSumBuyTypeCent(int LuckId, string BuyType)
        {
            return dal.GetSumBuyTypeCent(LuckId,BuyType);
        }

        /// <summary>
        /// 计算某期某ID购买的总币数
        /// </summary>
        public long GetSumBuyCent(int LuckId, int UsID)
        {
            return dal.GetSumBuyCent(LuckId, UsID);
        }
         /// <summary>
        /// 计算某期所有玩家下注金额
        /// </summary>
        public long GetAllBuyCent(int LuckId)
        {
            return dal.GetAllBuyCent(LuckId);
        }
        /// <summary>
        /// 计算某期购买人数
        /// </summary>
        public int GetCount(int LuckId)
        {
            return dal.GetCount(LuckId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Luckpay model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.Luckpay model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            dal.Update(ID, WinCent, State);
        }
        /// <summary>
        /// 更新未兑奖的
        /// </summary>
        public void UpdateOverDay(string AddTime)
        {
            dal.UpdateOverDay(AddTime);
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
        public BCW.Model.Game.Luckpay GetLuckpay(int ID)
        {

            return dal.GetLuckpay(ID);
        }
         /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
          return   dal.GetPrice(ziduan,strWhere);
        }
        /// <summary>
        /// 后台计算投注总币值,不计算系统号
        /// </summary>
        public long ManGetPrice(string ziduan, string strWhere)
        {
            return dal.ManGetPrice(ziduan, strWhere);
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
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
        /// <returns>IList Luckpay</returns>
        public IList<BCW.Model.Game.Luckpay> GetLuckpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetLuckpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.Model.Game.Luckpay> GetLuckpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetLuckpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

