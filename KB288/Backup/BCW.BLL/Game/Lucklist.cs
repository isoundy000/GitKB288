using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// 业务逻辑类Lucklist 的摘要说明。
    /// </summary>
    public class Lucklist
    {
        private readonly BCW.DAL.Game.Lucklist dal = new BCW.DAL.Game.Lucklist();
        public Lucklist()
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
        /// 统计数据
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            return dal.Exists();
        }
           /// <summary>
        /// 是否存在的期号
        /// </summary>
        public bool ExistsBJQH(int Bjkl8Qihao)
        {
            return dal.ExistsBJQH(Bjkl8Qihao);
        }
        /// <summary>
        /// 是否存在该时间段的期号
        /// </summary>
        public bool ExistsEndTime(string EndTime)
        {
            return dal.ExistsEndTime(EndTime);
        }
         /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existspanduan(string panduan)
        {
            return dal.Existspanduan(panduan);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
          /// <summary>
        /// 是否存在该抓取的期号
        /// </summary>
        public bool ExistsQihao(string Qihao)
        {
            return dal.ExistsQihao(Qihao);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Lucklist model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.Model.Game.Lucklist model)
        {
            return dal.Add2(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.Lucklist model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// 根据Bjkl8Qihao开奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update3(BCW.Model.Game.Lucklist model)
        {
           return dal.Update3(model);
        }
        /// <summary>
        /// 更新本期数据
        /// </summary>
        public int Update2(BCW.Model.Game.Lucklist model)
        {
            return dal.Update2(model);
        }

        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update(int ID, int SumNum)
        {
            dal.Update(ID, SumNum);
        }


        /// <summary>
        /// 更新奖池基金
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            dal.UpdatePool(ID, Pool);
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
        public BCW.Model.Game.Lucklist GetLucklist(int ID)
        {

            return dal.GetLucklist(ID);
        }
         /// <summary>
        /// 得到下一期未开奖的期数
        /// </summary>
        public BCW.Model.Game.Lucklist GetNextLucklist()
        {
            return dal.GetNextLucklist();
        }
          /// <summary>
        /// 根据时间获取最新一条数据
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistByTime()
        {
           return dal.GetLucklistByTime();
        }
         /// <summary>
        /// 得到本期对象实体
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistSecond()
        {
            return dal.GetLucklistSecond();
        }
           /// <summary>
        /// 得到最新一期的状态
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistState()
        {
            return dal.GetLucklistState();
        }
        /// <summary>
        /// 得到最近未开奖对象实体
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklist()
        {
            return dal.GetLucklist();
        }
        ///// <summary>
        ///// 得到本期对象实体
        ///// </summary>
        //public BCW.Model.Game.Lucklist GetLucklist1()
        //{

        //    return dal.GetLucklist1();
        //}



        /// <summary>
        /// 根据Bjkl8Qihao得到对应数据的ID
        /// </summary>
        public int GetID(int Bjkl8Qihao)
        {
            return dal.GetID(Bjkl8Qihao);
        }
        /// <summary>
        /// 得到当天的开始期数
        /// </summary>
        public string GetPanduan()
        {
            return dal.GetPanduan();
        }
        /// <summary>
        /// 得到奖池基金
        /// </summary>
        public long GetPool(int ID)
        {
            return dal.GetPool(ID);
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Lucklist</returns>
        public IList<BCW.Model.Game.Lucklist> GetLucklists(int SizeNum, string strWhere)
        {
            return dal.GetLucklists(SizeNum, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Lucklist</returns>
        public IList<BCW.Model.Game.Lucklist> GetLucklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetLucklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

