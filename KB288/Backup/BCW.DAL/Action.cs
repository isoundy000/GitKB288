using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
/// <summary>
/// 修改动态 屏蔽球赛动态
/// 黄国军  20160617
/// </summary>
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Action。
    /// </summary>
    public class Action
    {
        public Action()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Action");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Action");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Action model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Action(");
            strSql.Append("Types,NodeId,UsId,UsName,Notes,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@UsId,@UsName,@Notes,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.AddTime;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(int Types, int NodeId, int UsId, string UsName, string Notes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Action(");
            strSql.Append("Types,NodeId,UsId,UsName,Notes,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@UsId,@UsName,@Notes,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = Types;
            parameters[1].Value = NodeId;
            parameters[2].Value = UsId;
            parameters[3].Value = UsName;
            parameters[4].Value = Notes;
            parameters[5].Value = DateTime.Now;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Action model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Action set ");
            strSql.Append("Types=@Types,");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("UsId=@UsId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.UsId;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Notes;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Action ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 清空一组数据
        /// </summary>
        public void Clear(int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Action ");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 清空一组数据
        /// </summary>
        public void Clear(int Types, int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Action ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void Clear()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Action ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Action GetAction(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,NodeId,UsId,UsName,Notes,AddTime from tb_Action ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Action model = new BCW.Model.Action();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.NodeId = reader.GetInt32(2);
                    model.UsId = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Notes = reader.GetString(5);
                    model.AddTime = reader.GetDateTime(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Action ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Action> listActions = new List<BCW.Model.Action>();
            string sTable = "tb_Action";
            string sPkey = "id";
            string sField = "ID,Types,UsId,UsName,Notes,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listActions;
                }
                while (reader.Read())
                {
                    BCW.Model.Action objAction = new BCW.Model.Action();
                    objAction.ID = reader.GetInt32(0);
                    objAction.Types = reader.GetInt32(1);
                    objAction.UsId = reader.GetInt32(2);
                    objAction.UsName = reader.GetString(3);
                    objAction.Notes = reader.GetString(4);
                    objAction.AddTime = reader.GetDateTime(5);
                    listActions.Add(objAction);
                }
            }
            return listActions;
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActions(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Action> listActions = new List<BCW.Model.Action>();
            string sTable = "tb_Action";
            string sPkey = "id";
            string sField = "ID,UsId,UsName,Notes,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listActions;
                }
                while (reader.Read())
                {
                    BCW.Model.Action objAction = new BCW.Model.Action();
                    objAction.ID = reader.GetInt32(0);
                    objAction.UsId = reader.GetInt32(1);
                    objAction.UsName = reader.GetString(2);
                    objAction.Notes = reader.GetString(3);
                    objAction.AddTime = reader.GetDateTime(4);
                    listActions.Add(objAction);
                }
            }
            return listActions;
        }


        /// <summary>
        /// 取得好友动态记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActionsFriend(int Types, int uid, int SizeNum)
        {
            IList<BCW.Model.Action> listActions = new List<BCW.Model.Action>();

            //string queryString = "SELECT Top " + SizeNum + " a.UsId,a.UsName,a.Notes,a.AddTime FROM tb_Action a Where a.UsId in (Select b.FriendId from tb_Friend b Where b.UsID=" + uid + " and b.FriendId=a.UsId and Types=" + Types + ") ORDER BY a.AddTime DESC";
            ///取消球赛动态
            string queryString = "SELECT Top " + SizeNum + " a.UsId,a.UsName,a.Notes,a.AddTime FROM tb_Action a Where a.UsId in (Select b.FriendId from tb_Friend b Where b.UsID=" + uid + " and b.FriendId=a.UsId and Types=" + Types + ") AND (Types <> 5) AND (Types <> 23) ORDER BY a.AddTime DESC";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                while (reader.Read())
                {
                    BCW.Model.Action objAction = new BCW.Model.Action();
                    objAction.UsId = reader.GetInt32(0);
                    objAction.UsName = reader.GetString(1);
                    objAction.Notes = reader.GetString(2);
                    objAction.AddTime = reader.GetDateTime(3);
                    listActions.Add(objAction);
                }
            }

            return listActions;
        }

        /// <summary>
        /// 取得好友动态记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActionsFriend(int Types, int uid, int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Action> listActions = new List<BCW.Model.Action>();
            // 计算记录数
            string countString = "SELECT COUNT(a.ID) FROM tb_Action a Where a.UsId in (Select b.FriendId from tb_Friend b Where b.UsID=" + uid + " and b.FriendId=a.UsId and Types=" + Types + " and notes not like '%guess2%' and notes not like '%bbsshop%')";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listActions;
            }
            string queryString = "SELECT a.UsId,a.UsName,a.Notes,a.AddTime FROM tb_Action a Where a.UsId in (Select b.FriendId from tb_Friend b Where b.UsID=" + uid + " and b.FriendId=a.UsId and Types=" + Types + ") and notes not like '%guess2%' and notes not like '%bbsshop%' ORDER BY a.AddTime DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Action objAction = new BCW.Model.Action();
                        objAction.UsId = reader.GetInt32(0);
                        objAction.UsName = reader.GetString(1);
                        objAction.Notes = reader.GetString(2);
                        objAction.AddTime = reader.GetDateTime(3);
                        listActions.Add(objAction);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listActions;
        }


        #endregion  成员方法
    }
}

