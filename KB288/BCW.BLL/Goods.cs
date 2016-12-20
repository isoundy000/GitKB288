using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Goods 的摘要说明。
    /// </summary>
    public class Goods
    {
        private readonly BCW.DAL.Goods dal = new BCW.DAL.Goods();
        public Goods()
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string Title)
        {
            return dal.Exists(Title);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Goods model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Goods model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新出售数量
        /// </summary>
        public void UpdateSellCount(int ID, int SellCount)
        {
            dal.UpdateSellCount(ID, SellCount);
        }

        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReLastIP)
        {
            dal.UpdateReStats(ID, ReStats, ReLastIP);
        }

        /// <summary>
        /// 更新点击数
        /// </summary>
        public void UpdateReadcount(int ID, int Readcount)
        {
            dal.UpdateReadcount(ID, Readcount);
        }

        /// <summary>
        /// 更新评论条数
        /// </summary>
        public void UpdateRecount(int ID, int Recount)
        {
            dal.UpdateRecount(ID, Recount);
        }

        /// <summary>
        /// 更新购买人数
        /// </summary>
        public void UpdatePaycount(int ID, int Paycount)
        {
            dal.UpdatePaycount(ID, Paycount);
        }

        /// <summary>
        /// 更新评价人数
        /// </summary>
        public void UpdateEvcount(int ID, int Evcount)
        {
            dal.UpdateEvcount(ID, Evcount);
        }

        /// <summary>
        /// 更新文件
        /// </summary>
        public void UpdateFiles(int ID, string Files)
        {
            dal.UpdateFiles(ID, Files);
        }

        /// <summary>
        /// 更新封面
        /// </summary>
        public void UpdateCover(int ID, string Cover)
        {
            dal.UpdateCover(ID, Cover);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// 批量更新节点
        /// </summary>
        public void UpdateNodeIds(int NewNodeId, int OrdNodeId)
        {
            dal.UpdateNodeIds(NewNodeId, OrdNodeId);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        public void DeleteFiles(int ID)
        {

            dal.DeleteFiles(ID);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public void DeleteNodeId(int ID)
        {

            dal.DeleteNodeId(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Goods GetGoods(int ID)
        {

            return dal.GetGoods(ID);
        }

        /// <summary>
        /// 得到Title标题
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// 得到节点NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// 得到文件Files
        /// </summary>
        public string GetFiles(int ID)
        {
            return dal.GetFiles(ID);
        }

        /// <summary>
        /// 得到封面Cover
        /// </summary>
        public string GetCover(int ID)
        {
            return dal.GetCover(ID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
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
        /// <returns>IList Goods</returns>
        public IList<BCW.Model.Goods> GetGoodss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGoodss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

