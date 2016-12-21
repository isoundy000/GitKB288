using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Forumlog 的摘要说明。
    /// </summary>
    public class Forumlog
    {
        private readonly BCW.DAL.Forumlog dal = new BCW.DAL.Forumlog();
        public Forumlog()
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
        /// 得到记录数
        /// </summary>
        public int GetCount(int BID, int ReID)
        {
            return dal.GetCount(BID, ReID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(int Types, int ForumID, string Content)
        {
            return dal.Add(Types, ForumID, Content);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(int Types, int ForumID, int BID, string Content)
        {
            return dal.Add(Types, ForumID, BID, Content);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(int Types, int ForumID, int BID, int ReID, string Content)
        {
            return dal.Add(Types, ForumID, BID, ReID, Content);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Forumlog model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Forumlog model)
        {
            dal.Update(model);
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
        public BCW.Model.Forumlog GetForumlog(int ID)
        {

            return dal.GetForumlog(ID);
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
        /// <returns>IList Forumlog</returns>
        public IList<BCW.Model.Forumlog> GetForumlogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetForumlogs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}
