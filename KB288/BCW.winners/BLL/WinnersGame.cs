using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类tb_WinnersGame 的摘要说明。
    /// </summary>
    public class tb_WinnersGame
    {
        private readonly BCW.DAL.tb_WinnersGame dal = new BCW.DAL.tb_WinnersGame();
        public tb_WinnersGame()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// 是否存在该游戏
        /// </summary>
        public bool ExistsGameName(string GameName)
        {
            return dal.ExistsGameName(GameName);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_WinnersGame model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 通过Id更新
        /// price的值
        /// /// </summary>
        public void UpdatePrice(int ID, long price)
        {
            dal.UpdatePrice(ID, price);
        }

        /// <summary>
        /// 通过Id更新
        /// price的值
        /// /// </summary>
        public void UpdatePriceForIdent(string Ident, long price)
        {
            dal.UpdatePriceForIdent(Ident, price);
        }
        /// <summary>
        /// 通过Id更新
        /// price ptype的值
        /// /// </summary>
        public void UpdatePricePtypeForIdent(string Ident, long price, int ptype)
        {
            dal.UpdatePricePtypeForIdent(Ident, price, ptype);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_WinnersGame model)
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
        public BCW.Model.tb_WinnersGame Gettb_WinnersGame(int ID)
        {

            return dal.Gettb_WinnersGame(ID);
        }

        /// <summary>
        /// 从GameName得到一个最低投注
        /// </summary>
        public long GetPrice(string name)
        {
            return dal.GetPrice(name);
        }

        /// <summary>
        /// 从GameName得到一个最低投注
        /// </summary>
        public int GetPtype(string name)
        {
            return dal.GetPtype(name);
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
        /// <returns>IList tb_WinnersGame</returns>
        public IList<BCW.Model.tb_WinnersGame> Gettb_WinnersGames(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_WinnersGames(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

