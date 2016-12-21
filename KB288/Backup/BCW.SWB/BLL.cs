using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.SWB
{
    public partial  class BLL
    {
        private readonly BCW.SWB.DAL dal = new BCW.SWB.DAL();
        public BLL()
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
        /// 是否存在该用户
        /// </summary>
        public bool ExistsUserID(int UserID, int GameId)
        {
            return dal.ExistsUserID(UserID, GameId);
        }
        //new add
        /// <summary>
        /// 得到一个钱数
        /// </summary>
        public long GeUserGold(int UserID, int GameId)
        {

            return dal.GeUserGold(UserID, GameId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.SWB.Model GetModelForUserId(int UserID, int GameId)
        {

            return dal.GetModelForUserId(UserID, GameId);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.SWB.Model model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.SWB.Model model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 由用户ID更新钱币
        /// </summary>
        public void UpdateMoney(int UserID, long Money, int GameID)
        {
            dal.UpdateMoney(UserID,Money,GameID);
        }
        /// <summary>
        /// 由用户ID更新领钱时间
        /// </summary>
        public void UpdateTime(int UserID, DateTime UpdateTime, int GameID)
        {
            dal.UpdateTime(UserID,UpdateTime,GameID);
        }
        /// <summary>
        /// 由用户ID更新权限
        /// </summary>
        public void UpdatePermission(int UserID, int Permission, int GameID)
        {
            dal.UpdatePermission(UserID,Permission,GameID);
        }
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        public bool DeleteWhere(string wheres)
        {
            return dal.DeleteWhere(wheres);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.SWB.Model GetModel(int ID)
        {

            return dal.GetModel(ID);
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
        public List<BCW.SWB.Model> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.SWB.Model> DataTableToList(DataTable dt)
        {
            List<BCW.SWB.Model> modelList = new List<BCW.SWB.Model>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.SWB.Model model;
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
        public IList<BCW.SWB.Model> GetListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetListByPage(p_pageIndex,p_pageSize,strWhere, out p_recordCount);
        }
            #endregion  BasicMethod
        }

}
