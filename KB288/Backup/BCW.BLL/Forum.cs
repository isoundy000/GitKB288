using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Forum 的摘要说明。
    /// </summary>
    public class Forum
    {
        private readonly BCW.DAL.Forum dal = new BCW.DAL.Forum();
        public Forum()
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
        public bool Exists2(int ID)
        {
            return dal.Exists2(ID);
        }

        /// <summary>
        /// 论坛里是否存在论坛
        /// </summary>
        public bool ExistsNodeId(int ID)
        {
            return dal.ExistsNodeId(ID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.Forum model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Forum model)
        {
            dal.Update(model);
        }
                
        /// <summary>
        /// 更新版规
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            dal.UpdateContent(ID, Content);
        }
                
        /// <summary>
        /// 更新底链
        /// </summary>
        public void UpdateFootUbb(int ID, string FootUbb)
        {
            dal.UpdateFootUbb(ID, FootUbb);
        }

        /// <summary>
        /// 更新NodeId
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// 更新下级ID集合
        /// </summary>
        public void UpdateDoNode(int ID, string DoNode)
        {
            dal.UpdateDoNode(ID, DoNode);
        }

        /// <summary>
        /// 更新在线人数
        /// </summary>
        public void UpdateLine(int ID, int Line)
        {
            dal.UpdateLine(ID, Line);
        }

        /// <summary>
        /// 在线人数减1
        /// </summary>
        public void UpdateLine(int ID)
        {
            dal.UpdateLine(ID);
        }
               
        /// <summary>
        /// 更新基金数目
        /// </summary>
        public void UpdateiCent(int ID, long iCent)
        {
            dal.UpdateiCent(ID, iCent);
        }
        /// <summary>
        /// 更新基金数目,并且写入实处明细
        /// </summary>
        public void UpdateiCent(int ID, int UID, string UsName, long iCent, int ToID,string AcText)
        {
            dal.UpdateiCent(ID, iCent);
            BCW.Model.Forumfund model = new BCW.Model.Forumfund();
            model.Types = 2;
            model.ForumId = ID;
            model.UsID = UID;
            model.UsName = UsName;
            model.PayCent = -iCent;
            model.Content = AcText;
            model.ToID = ToID;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Forumfund().Add(model);
          
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
        public BCW.Model.Forum GetForum(int ID)
        {

            return dal.GetForum(ID);
        }

        /// <summary>
        /// 得到一个名称、口号、在线
        /// </summary>
        public BCW.Model.Forum GetForumBasic(int ID)
        {
            return dal.GetForumBasic(ID);
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {

            return dal.GetTitle(ID);
        }

        /// <summary>
        /// 得到版规公告Content
        /// </summary>
        public string GetContent(int ID)
        {

            return dal.GetContent(ID);
        }
                
        /// <summary>
        /// 得到底链FootUbb
        /// </summary>
        public string GetFootUbb(int ID)
        {

            return dal.GetFootUbb(ID);
        }

        /// <summary>
        /// 得到NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {

            return dal.GetNodeId(ID);
        }
     
        /// <summary>
        /// 得到GroupId
        /// </summary>
        public int GetGroupId(int ID)
        {

            return dal.GetGroupId(ID);
        }
             
        /// <summary>
        /// 得到论坛基金
        /// </summary>
        public long GetiCent(int ID)
        {

            return dal.GetiCent(ID);
        }

        /// <summary>
        /// 得到Label
        /// </summary>
        public string GetLabel(int ID)
        {
            return dal.GetLabel(ID);
        }
                
        /// <summary>
        /// 得到DoNode下级ID
        /// </summary>
        public string GetDoNode(int ID)
        {
            return dal.GetDoNode(ID);
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
        /// <returns>IList Forum</returns>
        public IList<BCW.Model.Forum> GetForums(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetForums(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}
