using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Friend。
    /// </summary>
    public class Friend
    {
        public Friend()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Friend");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Friend");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int FriendId, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Friend");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and FriendId=@FriendId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@FriendId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;
            parameters[2].Value = FriendId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某分组好友数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Friend");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;
            parameters[2].Value = 0;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 计算某ID好友数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Friend");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 计算某ID粉丝数量
        /// </summary>
        public int GetFansCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Friend");
            strSql.Append(" where FriendID=@UsID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 2;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Friend model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Friend(");
            strSql.Append("Types,NodeId,UsID,UsName,FriendID,FriendName)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@UsID,@UsName,@FriendID,@FriendName)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@FriendID", SqlDbType.Int,4),
					new SqlParameter("@FriendName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.FriendID;
            parameters[5].Value = model.FriendName;

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
        /// 更新某分组好友成为默认分类
        /// </summary>
        public void UpdateNodeId(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Friend set ");
            strSql.Append("NodeId=0");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新最后联系时间
        /// </summary>
        public void UpdateTime(int UsID, int FriendID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Friend set ");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and FriendID=@FriendID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@FriendID", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = FriendID;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新正式好友
        /// </summary>
        public void UpdateTypes(int UsID, int FriendID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Friend set ");
            strSql.Append("Types=@Types");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and FriendID=@FriendID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@FriendID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = FriendID;
            parameters[2].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Friend model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Friend set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("FriendName=@FriendName");
            strSql.Append(" where FriendID=@FriendID ");
            SqlParameter[] parameters = {
					new SqlParameter("@FriendID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@FriendName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.FriendID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.FriendName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Friend ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UsID, int FriendID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Friend ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and FriendID=@FriendID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@FriendID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;
            parameters[2].Value = FriendID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Friend GetFriend(int FriendId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,NodeId,UsID,UsName,FriendID,FriendName,AddTime from tb_Friend ");
            strSql.Append(" where FriendId=@FriendId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FriendId", SqlDbType.Int,4)};
            parameters[0].Value = FriendId;

            BCW.Model.Friend model = new BCW.Model.Friend();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.NodeId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.FriendID = reader.GetInt32(5);
                    model.FriendName = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        model.AddTime = reader.GetDateTime(7);
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
            strSql.Append(" FROM tb_Friend ");
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
        /// <returns>IList Friend</returns>
        public IList<BCW.Model.Friend> GetFriends(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Friend> listFriends = new List<BCW.Model.Friend>();
            string sTable = "tb_Friend";
            string sPkey = "id";
            string sField = "ID,FriendID,FriendName,AddTime";
            string sCondition = strWhere;
            string sOrder = "AddTime Desc";
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
                    return listFriends;
                }
                while (reader.Read())
                {
                    BCW.Model.Friend objFriend = new BCW.Model.Friend();
                    objFriend.ID = reader.GetInt32(0);
                    objFriend.FriendID = reader.GetInt32(1);
                    objFriend.FriendName = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        objFriend.AddTime = reader.GetDateTime(3);
                    listFriends.Add(objFriend);
                }
            }
            return listFriends;
        }

        #endregion  成员方法
    }
}
