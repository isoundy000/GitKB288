using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Forumstat 的摘要说明。
    /// </summary>
    public class Forumstat
    {
        private readonly BCW.DAL.Forumstat dal = new BCW.DAL.Forumstat();
        public Forumstat()
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
        /// 是否存在今天记录
        /// </summary>
        public bool ExistsToDay(int UsID, int ForumID)
        {
            return dal.ExistsToDay(UsID, ForumID);
        }

        /// <summary>
        /// 得到某ID某选项数量
        /// </summary>
        public int GetIDCount(int UsID, int Types)
        {
            return dal.GetIDCount(UsID, Types);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Forumstat model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int Types, int UsID, int ForumID)
        {
            dal.Update(Types, UsID, ForumID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update3(int Types, int UsID, int ForumID, DateTime AddTime)
        {
            dal.Update3(Types, UsID, ForumID, AddTime);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(int Types, int UsID, int ForumID, DateTime AddTime)
        {
            dal.Update2(Types, UsID, ForumID, AddTime);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Forumstat model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 批量更新统计
        /// </summary>
        public void UpdateForumID(int ForumID, int NewForumID)
        {
            dal.UpdateForumID(ForumID, NewForumID);
        }
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 删除排行残余数据（数据为0的记录）
        /// </summary>
        public void Delete()
        {
            dal.Delete();
        }

        /// <summary>
        /// 得到某选项今天数量
        /// </summary>
        public int GetCount(int UsID, int Types)
        {
            return dal.GetCount(UsID, Types);
        }

        /// <summary>
        /// 得到某ID本月发帖回帖数量(1发贴/2回帖)
        /// </summary>
        public int GetMonthCount(int UsID, int Types)
        {
            return dal.GetMonthCount(UsID, Types);
        }

        /// <summary>
        /// 得到某论坛某选项数量
        /// </summary>
        public int GetCount(int ForumID, int Types, int dType)
        {
            return dal.GetCount(0, 0, ForumID, Types, dType);
        }

        /// <summary>
        /// 得到某ID某论坛某选项数量
        /// </summary>
        public int GetCount(int UsID, int ForumID, int Types, int dType)
        {
            return dal.GetCount(0, UsID, ForumID, Types, dType);
        }

        /// <summary>
        /// 得到主题论坛或圈子论坛某选项数量(fg 0值统计全部，1值统计主题论坛，2值统计圈子论坛)
        /// 20160120 黄国军
        /// </summary>
        /// <param name="fg">类型</param>
        /// <param name="ForumID">论坛ID</param>
        /// <param name="Types">固定类型1帖子数2回帖数</param>
        /// <param name="dType">时间分类</param>
        /// <returns>返回统计数</returns>
        public int GetCount2(int fg, int ForumID, int Types, int dType)
        {
            return dal.GetCount(fg, 0, ForumID, Types, dType);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forumstat GetForumstat(int ID)
        {

            return dal.GetForumstat(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 论坛排行每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Forumstat</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, strWhere, strOrder, showtype, out p_recordCount);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Forumstat</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        /// <summary>
        /// 论坛排行分页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, int ptype, string sWhere, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, ptype, sWhere, out p_recordCount);
        }
        #endregion  成员方法
    }
}

