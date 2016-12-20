using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// 业务逻辑类BJL_Card 的摘要说明。
    /// </summary>
    public class BJL_Card
    {
        private readonly BCW.Baccarat.DAL.BJL_Card dal = new BCW.Baccarat.DAL.BJL_Card();
        public BJL_Card()
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Card model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Card model)
        {
            dal.Update(model);
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
        public BCW.Baccarat.Model.BJL_Card GetBJL_Card(int ID)
        {

            return dal.GetBJL_Card(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //============================================
        /// <summary>
        /// 是否存在某房间某桌面的扑克牌
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public bool ExistsCard(int RoomID, int RoomDoTable)
        {
            return dal.ExistsCard(RoomID, RoomDoTable);
        }
        /// <summary>
        ///得到特定房间ID和桌面table的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetCardMessage(int RoomID, int RoomDoTable)
        {
            return dal.GetCardMessage(RoomID, RoomDoTable);
        }
        //============================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_Card</returns>
        public IList<BCW.Baccarat.Model.BJL_Card> GetBJL_Cards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBJL_Cards(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

