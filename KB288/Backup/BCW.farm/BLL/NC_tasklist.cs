using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_tasklist 的摘要说明。
    /// </summary>
    public class NC_tasklist
    {
        private readonly BCW.farm.DAL.NC_tasklist dal = new BCW.farm.DAL.NC_tasklist();
        public NC_tasklist()
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_tasklist model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_tasklist model)
        {
            dal.Update(model);
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
        public BCW.farm.Model.NC_tasklist GetNC_tasklist(int ID)
        {

            return dal.GetNC_tasklist(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList1(string strField, string strWhere)
        {
            return dal.GetList1(strField, strWhere);
        }
        //=================
        /// <summary>
        /// me_根据id和usid查询任务
        /// </summary>
        public BCW.farm.Model.NC_tasklist Get_renwu(int ID, int usid)
        {

            return dal.Get_renwu(ID, usid);
        }
        /// <summary>
        /// me_是否存在该记录--日常
        /// </summary>
        public bool Exists_usid(int usid, int task_id)
        {
            return dal.Exists_usid(usid, task_id);
        }
        /// <summary>
        /// me_是否存在该记录--主线
        /// </summary>
        public bool Exists_usid1(int usid, int task_id)
        {
            return dal.Exists_usid1(usid, task_id);
        }
        /// <summary>
        /// me_是否存在该记录--活动
        /// </summary>
        public bool Exists_usid3(int usid, int task_id)
        {
            return dal.Exists_usid3(usid, task_id);
        }
        /// <summary>
        /// me_是否存在该记录--活动13-消费馈赠
        /// </summary>
        public bool Exists_usid13(int usid, int task_id, int task_oknum)
        {
            return dal.Exists_usid13(usid, task_id, task_oknum);
        }
        /// <summary>
        /// me_是否存在该记录--主线(已完成)邵广林 20160922
        /// </summary>
        public bool Exists_usid2(int usid, int task_id)
        {
            return dal.Exists_usid2(usid, task_id);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_renwu(string strField, string strWhere)
        {
            return dal.update_renwu(strField, strWhere);
        }
        //=================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_tasklist</returns>
        public IList<BCW.farm.Model.NC_tasklist> GetNC_tasklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_tasklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

