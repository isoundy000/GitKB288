using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Statinfo 的摘要说明。
    /// </summary>
    public class Statinfo
    {
        private readonly BCW.DAL.Statinfo dal = new BCW.DAL.Statinfo();
        public Statinfo()
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
        /// 根据条件得到记录数
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// 根据条件得到记录数
        /// </summary>
        public int GetIPCount(string strWhere)
        {
            return dal.GetIPCount(strWhere);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Statinfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Statinfo model)
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
        public BCW.Model.Statinfo GetStatinfo(int ID)
        {

            return dal.GetStatinfo(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strSql)
        {
            return dal.GetList(strSql);
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
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetStatinfos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStatinfos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得IP记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetIPs(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetIPs(p_pageIndex, p_pageSize, out p_recordCount);
        }

        /// <summary>
        /// 取得Browser记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetBrowsers(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetBrowsers(p_pageIndex, p_pageSize, out p_recordCount);
        }

        /// <summary>
        /// 取得System记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetSystems(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetSystems(p_pageIndex, p_pageSize, out p_recordCount);
        }

        /// <summary>
        /// 取得Purl记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetPUrls(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetPUrls(p_pageIndex, p_pageSize, out p_recordCount);
        }
        #endregion  成员方法
    }
}
