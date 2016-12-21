using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Group 的摘要说明。
    /// </summary>
    public class Group
    {
        private readonly BCW.DAL.Group dal = new BCW.DAL.Group();
        public Group()
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
        public bool ExistsUsID(int UsID)
        {
            return dal.ExistsUsID(UsID);
        }
             
        /// <summary>
        /// 得到某城市的圈子数
        /// </summary>
        public int GetGroupNum(string City)
        {
            return dal.GetGroupNum(City);
        }

        /// <summary>
        /// 得到某分类的圈子数
        /// </summary>
        public int GetGroupNum(int Types)
        {
            return dal.GetGroupNum(Types);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Group model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Group model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Group model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新过期时间(续费)
        /// </summary>
        public void UpdateExitTime(int ID, DateTime ExTime)
        {
            dal.UpdateExitTime(ID, ExTime);
        }

        /// <summary>
        /// 更新基金数目
        /// </summary>
        public void UpdateiCent(int ID, long iCent)
        {
            dal.UpdateiCent(ID, iCent);
        }
        
        /// <summary>
        /// 更新基金密码
        /// </summary>
        public void UpdateiCentPwd(int ID, string iCentPwd)
        {
            dal.UpdateiCentPwd(ID, iCentPwd);
        }

        /// <summary>
        /// 更新签到ID
        /// </summary>
        public void UpdateSignID(int ID, string SignID)
        {
            dal.UpdateSignID(ID, SignID);
        }

        /// <summary>
        /// 更新今天来访ID
        /// </summary>
        public void UpdateVisitId(int ID)
        {
            dal.UpdateVisitId(ID);
        } 
       
        /// <summary>
        /// 更新今天来访ID
        /// </summary>
        public void UpdateVisitId(int ID, string VisitId)
        {
            dal.UpdateVisitId(ID, VisitId);
        }

        /// <summary>
        /// 更新成员人数
        /// </summary>
        public void UpdateiTotal(int ID, int iTotal)
        {
            dal.UpdateiTotal(ID, iTotal);
        }
                
        /// <summary>
        /// 审核圈子
        /// </summary>
        public void UpdateStatus(int ID, int Status)
        {
            dal.UpdateStatus(ID, Status);
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
        public BCW.Model.Group GetGroup(int ID)
        {

            return dal.GetGroup(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Group GetGroupMe(int ID)
        {

            return dal.GetGroupMe(ID);
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }
        
        /// <summary>
        /// 得到UsID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }
               
        /// <summary>
        /// 得到ID
        /// </summary>
        public int GetID(int UsID)
        {
            return dal.GetID(UsID);
        }

        /// <summary>
        /// 得到ForumId
        /// </summary>
        public int GetForumId(int UsID)
        {
            return dal.GetForumId(UsID);
        }

        /// <summary>
        /// 得到SignID
        /// </summary>
        public string GetSignID(int ID)
        {
            return dal.GetSignID(ID);
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
        /// <returns>IList Group</returns>
        public IList<BCW.Model.Group> GetGroups(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetGroups(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

