using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.HB.DAL
{
    /// <summary>
    /// 数据访问类:ChatMe
    /// </summary>
    public partial class ChatMe
    {
        public ChatMe()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_ChatMe");
        }
        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ChatMe");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)            };
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ChatID, int meid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ChatMe");
            strSql.Append(" where ChatID=@ChatID and UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
            };
            parameters[0].Value = ChatID;
            parameters[1].Value = meid;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HB.Model.ChatMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ChatMe(");
            strSql.Append("ChatID,UserID,State,jointime,score)");
            strSql.Append(" values (");
            strSql.Append("@ChatID,@UserID,@State,@jointime,@score)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@jointime", SqlDbType.DateTime),
                    new SqlParameter("@score", SqlDbType.Decimal,9)};
            parameters[0].Value = model.ChatID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.State;
            parameters[3].Value = model.jointime;
            parameters[4].Value = model.score;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HB.Model.ChatMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ChatMe set ");
            strSql.Append("ChatID=@ChatID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("State=@State,");
            strSql.Append("jointime=@jointime,");
            strSql.Append("score=@score");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@jointime", SqlDbType.DateTime),
                    new SqlParameter("@score", SqlDbType.Decimal,9),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.ChatID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.State;
            parameters[3].Value = model.jointime;
            parameters[4].Value = model.score;
            parameters[5].Value = model.ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(int chatid,int uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ChatMe set ");
            strSql.Append("score=score+1");
            strSql.Append(" where ChatID=@ChatID and UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = chatid;
            parameters[1].Value = uid;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新ChatTextType
        /// </summary>
        public bool UpdateChatTextType(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ChatText set ");
            strSql.Append("Type=1");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
       /// 更新SpeakTextType
        /// </summary>
        public bool UpdateSpeakType(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Speak set ");
            strSql.Append("Type=1");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(int chatid, int uid,int score)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ChatMe set ");
            strSql.Append("score=score+@score");
            strSql.Append(" where ChatID=@ChatID and UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@score", SqlDbType.Int,4)};
            parameters[0].Value = chatid;
            parameters[1].Value = uid;
            parameters[2].Value = score;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update2(int chatid, int uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ChatMe set ");
            strSql.Append("State=1");
            strSql.Append(" where ChatID=@ChatID and UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = chatid;
            parameters[1].Value = uid;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)            };
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID,int meid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatMe ");
            strSql.Append(" where ChatID=@ChatID and UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete2(int ChatID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatMe ");
            strSql.Append(" where ChatID=@ChatID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4)            };
            parameters[0].Value = ChatID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatMe ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.ChatMe GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ChatID,UserID,State,jointime,score from tb_ChatMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)            };
            parameters[0].Value = ID;

            BCW.HB.Model.ChatMe model = new BCW.HB.Model.ChatMe();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.ChatMe GetModel(int ID,int uid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ChatID,UserID,State,jointime,score from tb_ChatMe ");
            strSql.Append(" where ChatID=@ChatID and UserID=@uid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                     new SqlParameter("@uid", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;
            parameters[1].Value = uid;
            BCW.HB.Model.ChatMe model = new BCW.HB.Model.ChatMe();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.ChatMe DataRowToModel(DataRow row)
        {
            BCW.HB.Model.ChatMe model = new BCW.HB.Model.ChatMe();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["ChatID"] != null && row["ChatID"].ToString() != "")
                {
                    model.ChatID = int.Parse(row["ChatID"].ToString());
                }
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["State"] != null && row["State"].ToString() != "")
                {
                    model.State = int.Parse(row["State"].ToString());
                }
                if (row["jointime"] != null && row["jointime"].ToString() != "")
                {
                    model.jointime = DateTime.Parse(row["jointime"].ToString());
                }
                if (row["score"] != null && row["score"].ToString() != "")
                {
                    model.score = decimal.Parse(row["score"].ToString());
                }
            }
            return model;
        }

        /// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ChatID,UserID,State,jointime,score ");
            strSql.Append(" FROM tb_ChatMe ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append("  order by score desc");
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ID,ChatID,UserID,State,jointime,score ");
            strSql.Append(" FROM tb_ChatMe ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_ChatMe ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_ChatMe T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }


        #endregion  BasicMethod
        #region  聊天室相关方法
        /// <summary>
        /// 更新邀请权限 
        /// |0|仅群主可以邀请
        /// |1|仅群主和室主可以邀请
        /// |2|仅群主、室主和见习室主可以邀请
        /// |3|仅群主、室主、见习室主和临管可以邀请
        /// |4|所有成员可以邀请
        /// </summary>
        public bool UpdateInvite(int Invite,int Chatid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("Invite=@Invite");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Invite", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Invite;
            parameters[1].Value = Chatid;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到邀请权限 
        /// |0|仅群主可以邀请
        /// |1|仅群主和室主可以邀请
        /// |2|仅群主、室主和见习室主可以邀请
        /// |3|仅群主、室主、见习室主和临管可以邀请
        /// |4|所有成员可以邀请
        /// </summary>
        public int GetInvite(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Invite from tb_Chat ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;

                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
    }
}

