using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类tb_WinnersAward 的摘要说明。
    /// </summary>
    public class tb_WinnersAward
    {
        private readonly BCW.DAL.tb_WinnersAward dal = new BCW.DAL.tb_WinnersAward();
        public tb_WinnersAward()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            return dal.Exists(Id);
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
        public int Add(BCW.Model.tb_WinnersAward model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新isDone
        /// </summary>
        public void UpdateIsDone(int ID, int isDone)
        {
            dal.UpdateIsDone(ID, isDone);
        }
        /// <summary>
        /// 得到内线语句
        /// </summary>
        public string GetRemarks(int Id)
        {
            return dal.GetRemarks(Id);
        }
        /// <summary>
        /// 得到奖池类型都最新奖池
        /// </summary>
        public int GetMaxAwardForType(string type)
        {
            return dal.GetMaxAwardForType(type);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_WinnersAward model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Id)
        {

            dal.Delete(Id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_WinnersAward Gettb_WinnersAward(int Id)
        {

            return dal.Gettb_WinnersAward(Id);
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
        /// <returns>IList tb_WinnersAward</returns>
        public IList<BCW.Model.tb_WinnersAward> Gettb_WinnersAwards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_WinnersAwards(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

