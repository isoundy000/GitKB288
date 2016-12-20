using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类FComment 的摘要说明。
    /// </summary>
    public class FComment
    {
        private readonly BCW.DAL.FComment dal = new BCW.DAL.FComment();
        public FComment()
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
        public bool Exists(int ID, int DetailId)
        {
            return dal.Exists(ID, DetailId);
        }
               
        /// <summary>
        /// 计算某会员ID发表的评论数
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.FComment model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.FComment model)
        {
            dal.Update(model);
        }
                
        /// <summary>
        /// 更新回复内容
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            dal.UpdateReText(ID, ReText);
        }
        
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Types, int DetailId)
        {
            dal.Delete(Types, DetailId);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.FComment GetFComment(int ID)
        {

            return dal.GetFComment(ID);
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
        /// <returns>IList FComment</returns>
        public IList<BCW.Model.FComment> GetFComments(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetFComments(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}
