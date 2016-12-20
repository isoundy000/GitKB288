using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Detail 的摘要说明。
    /// </summary>
    public class Detail
    {
        private readonly BCW.DAL.Detail dal = new BCW.DAL.Detail();
        public Detail()
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
        public int Add(BCW.Model.Detail model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Detail model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新截图
        /// </summary>
        public void UpdatePics(int ID, string Pics)
        {
            dal.UpdatePics(ID, Pics);
        }
 
        /// <summary>
        /// 更新封面
        /// </summary>
        public void UpdateCover(int ID, string Cover)
        {
            dal.UpdateCover(ID, Cover);
        }

        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats,string ReLastIP)
        {
            dal.UpdateReStats(ID, ReStats, ReLastIP);
        }
        /// <summary>
        /// 更新点击ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClickID"></param>
        public void UpdateClickID(int ID, string ClickID)
        {
            dal.UpdateClickID(ID,ClickID);
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
        /// 更新购买ID
        /// </summary>
        public void UpdatePayId(int ID, string PayId)
        {
            dal.UpdatePayId(ID, PayId);
        }
                
        /// <summary>
        /// 审核文件
        /// </summary>
        public void UpdateHidden(int ID, int Hidden)
        {
            dal.UpdateHidden(ID, Hidden);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 删除截图
        /// </summary>
        public void DeletePics(int ID)
        {

            dal.DeletePics(ID);
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
        public BCW.Model.Detail GetDetail(int ID)
        {

            return dal.GetDetail(ID);
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// 得到适用机型Model
        /// </summary>
        public string GetPhoneModel(int ID)
        {
            return dal.GetPhoneModel(ID);
        }

        /// <summary>
        /// 得到节点NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// 得到类型Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }
               
        /// <summary>
        /// 得到用户ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }

        /// <summary>
        /// 得到文件Pics
        /// </summary>
        public string GetPics(int ID)
        {
            return dal.GetPics(ID);
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
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 取得上(下)一条记录
        /// </summary>
        public BCW.Model.Detail GetPreviousNextDetail(int ID, int p_NodeId, bool p_next)
        {
            return dal.GetPreviousNextDetail(ID, p_NodeId, p_next);
        }

        ///// <summary>
        ///// 取得全文索引结果
        ///// </summary>
        ///// <param name="p_pageIndex">当前页</param>
        ///// <param name="p_pageSize">分页大小</param>
        ///// <param name="p_recordCount">返回总记录数</param>
        ///// <returns>IList Detail</returns>
        //public IList<BCW.Model.Detail> GetDetails(int p_pageIndex, int p_pageSize, string strWhere, string Key, out int p_recordCount)
        //{
        //    return dal.GetDetails(p_pageIndex, p_pageSize, strWhere, Key, out p_recordCount);
        //}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Detail</returns>
        public IList<BCW.Model.Detail> GetDetails(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetDetails(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

