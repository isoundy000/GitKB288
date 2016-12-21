using System;
using System.Data;
using System.Collections.Generic;
using BCW.HB.Model;
namespace BCW.HB.BLL
{
    /// <summary>
    /// HbGetNote
    /// </summary>
    public partial class HbGetNote
    {
        private readonly BCW.HB.DAL.HbGetNote dal = new BCW.HB.DAL.HbGetNote();
        public HbGetNote()
        { }
        #region  BasicMethod
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
        public bool Exists2(int GetID,int HbID)
        {
            return dal.Exists2(GetID,HbID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HB.Model.HbGetNote model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HB.Model.HbGetNote model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            return dal.DeleteList(IDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbGetNote GetModel(int ID)
        {

            return dal.GetModel(ID);
        }
          /// <summary>
        ///通过GetID 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbGetNote GetModelByGetID(int GetID,int HbID)
        {
            return dal.GetModelByGetID(GetID,HbID);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.HB.Model.HbGetNote> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.HB.Model.HbGetNote> DataTableToList(DataTable dt)
        {
            List<BCW.HB.Model.HbGetNote> modelList = new List<BCW.HB.Model.HbGetNote>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.HB.Model.HbGetNote model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 获取抢到的总钱数
        /// </summary>
        public long GetAllMoney(string strWhere)
        {
            return dal.GetAllMoney(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ChatText</returns>
        public IList<BCW.HB.Model.HbGetNote> GetListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetListByPage(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  BasicMethod
    }
}


