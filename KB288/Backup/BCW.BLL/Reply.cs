using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Reply 的摘要说明。
    /// 
    /// 增加点值抽奖回帖入口 蒙宗将 20160823
    /// </summary>
    public class Reply
    {
        private readonly BCW.DAL.Reply dal = new BCW.DAL.Reply();
        public Reply()
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
        public bool Exists(int BID, int Floor)
        {
            return dal.Exists(BID, Floor);
        }

        /// <summary>
        /// 根据某论坛某用户ID计算回帖数
        /// </summary>
        public int GetCount(int UsID, int ForumId)
        {
            return dal.GetCount(UsID, ForumId);
        }
        /// <summary>
        /// 根据某帖子某用户ID计算未被删除的回帖数
        /// </summary>
        public int GetCountExist(int UsID, int BID)
        {
            return dal.GetCountExist(UsID, BID);
        }
        /// <summary>
        /// 根据某帖子某用户ID计算回帖数
        /// </summary>
        public int GetCount2(int UsID, int BID)
        {
            return dal.GetCount2(UsID, BID);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Reply model)
        {
            int id= dal.Add(model);
            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "回复帖子";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }
            return id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Reply model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int Bid, int Floor, string Content)
        {
            dal.Update(Bid, Floor, Content);
        }

        /// <summary>
        /// 更新CentText
        /// </summary>
        public void UpdateCentText(int Bid, int Floor, string CentText)
        {
            dal.UpdateCentText(Bid, Floor, CentText);
        }

        /// <summary>
        /// 更新ForumID
        /// </summary>
        public void UpdateForumID(int ID, int ForumID)
        {
            dal.UpdateForumID(ID, ForumID);
        }

        /// <summary>
        /// 批量更新回帖
        /// </summary>
        public void UpdateForumID2(int ForumID, int NewForumID)
        {
            dal.UpdateForumID2(ForumID, NewForumID);
        }

        /// <summary>
        /// 更新回帖文件数
        /// </summary>
        public void UpdateFileNum(int ID, int FileNum)
        {
            dal.UpdateFileNum(ID, FileNum);
        }

        /// <summary>
        /// 回帖加精/解精
        /// </summary>
        public void UpdateIsGood(int Bid, int Floor, int IsGood)
        {
            dal.UpdateIsGood(Bid, Floor, IsGood);
        }

        /// <summary>
        /// 回帖置顶/去顶
        /// </summary>
        public void UpdateIsTop(int Bid, int Floor, int IsTop)
        {
            dal.UpdateIsTop(Bid, Floor, IsTop);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel(int BID, int IsDel)
        {
            dal.UpdateIsDel(BID, IsDel);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel1(int ID, int IsDel)
        {
            dal.UpdateIsDel1(ID, IsDel);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel2(int BID, int Floor, int IsDel)
        {
            dal.UpdateIsDel2(BID, Floor, IsDel);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel3(int BID, int UsID, int IsDel)
        {
            dal.UpdateIsDel3(BID, UsID, IsDel);
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
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Bid, int Floor)
        {
            dal.Delete(Bid, Floor);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int Bid, int UsID)
        {
            dal.Delete2(Bid, UsID);
        }

        /// <summary>
        /// 得到新楼层
        /// </summary>
        public int GetFloor(int Bid)
        {

            return dal.GetFloor(Bid);
        }

        /// <summary>
        /// 根据ID得到楼层
        /// </summary>
        public int GetFloor2(int ID)
        {

            return dal.GetFloor2(ID);
        }

        /// <summary>
        /// 得到某楼层的内容
        /// </summary>
        public string GetContent(int Bid, int Floor)
        {
            return dal.GetContent(Bid, Floor);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Reply GetReplyMe(int BID, int Floor)
        {
            return dal.GetReplyMe(BID, Floor);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Reply GetByID(int ID)
        {
            return dal.GetByID(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Reply GetReply(int Bid, int Floor)
        {

            return dal.GetReply(Bid, Floor);
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetReplys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        /// <summary>
        /// 帖子排行分页记录 陈志基 2016/08/10
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Reply> GetForumstats1(int p_pageIndex, int p_pageSize, string strWhere, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats1(p_pageIndex, p_pageSize, strWhere, showtype, out  p_recordCount);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplysMe(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetReplys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<BCW.Model.Reply> GetReplysWhere(string strWhere)
        {
            return dal.GetReplysWhere(strWhere) ;
        }
        /// <summary>
        /// 帖子页面回帖记录
        /// </summary>
        /// <param name="p_Size">显示条数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplysTop(int p_Size, string strWhere)
        {
            return dal.GetReplysTop(p_Size, strWhere);
        }
        #endregion  成员方法
    }
}