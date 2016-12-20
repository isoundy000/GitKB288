using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// 业务逻辑类Brag 的摘要说明。
    /// </summary>
    public class Brag
    {
        private readonly BCW.DAL.Game.Brag dal = new BCW.DAL.Game.Brag();
        public Brag()
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
        /// 计算某状态的吹牛数量
        /// </summary>
        public int GetCountState(int State)
        {
            return dal.GetCountState(State);
        }

        /// <summary>
        /// 计算某用户今天吹牛数量
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// 计算某用户今天吹牛数量
        /// </summary>
        public int GetCount2(int UsID)
        {
            return dal.GetCount2(UsID);
        }

        /// <summary>
        /// 计算今天吹牛数量
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// 计算今天吹牛总币值
        /// </summary>
        public long GetPrice(int Types)
        {
            return dal.GetPrice(Types);
        }
        
        /// <summary>
        /// 计算吹牛总币值
        /// </summary>
        public long GetPrice(string strWhere)
        {
            return dal.GetPrice(strWhere);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Brag model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.Brag model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(BCW.Model.Game.Brag model)
        {
            dal.UpdateState(model);
        }

        /// <summary>
        /// 更新为公共吹牛
        /// </summary>
        public void UpdateState2(int ID)
        {
            dal.UpdateState2(ID);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
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
        public BCW.Model.Game.Brag GetBrag(int ID)
        {

            return dal.GetBrag(ID);
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
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.Game.Brag> GetBrags(int SizeNum, string strWhere)
        {
            return dal.GetBrags(SizeNum, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.Game.Brag> GetBrags(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBrags(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

