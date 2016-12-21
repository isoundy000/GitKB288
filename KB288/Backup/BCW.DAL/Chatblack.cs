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
    /// 数据访问类Chatblack。
    /// </summary>
    public class Chatblack
    {
        public Chatblack()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Chatblack");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Chatblack");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ChatId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Chatblack");
            strSql.Append(" where ChatId=@ChatId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and ExitTime>@ExitTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime)};
            parameters[0].Value = ChatId;
            parameters[1].Value = UsID;
            parameters[2].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ChatId, int UsID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Chatblack");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and ChatId=@ChatId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and ExitTime>@ExitTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime)};
            parameters[0].Value = Types;
            parameters[1].Value = ChatId;
            parameters[2].Value = UsID;
            parameters[3].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Chatblack model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Chatblack(");
            strSql.Append("Types,UsID,UsName,ChatId,ChatName,BlackWhy,BlackDay,AdminUsID,ExitTime,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@ChatId,@ChatName,@BlackWhy,@BlackDay,@AdminUsID,@ExitTime,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@ChatName", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackWhy", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackDay", SqlDbType.Int,4),
					new SqlParameter("@AdminUsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.ChatId;
            parameters[4].Value = model.ChatName;
            parameters[5].Value = model.BlackWhy;
            parameters[6].Value = model.BlackDay;
            parameters[7].Value = model.AdminUsID;
            parameters[8].Value = model.ExitTime;
            parameters[9].Value = model.AddTime;

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
        public void Update(BCW.Model.Chatblack model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chatblack set ");
            strSql.Append("Types=@Types,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ChatId=@ChatId,");
            strSql.Append("ChatName=@ChatName,");
            strSql.Append("BlackWhy=@BlackWhy,");
            strSql.Append("BlackDay=@BlackDay,");
            strSql.Append("AdminUsID=@AdminUsID,");
            strSql.Append("ExitTime=@ExitTime,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@ChatName", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackWhy", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackDay", SqlDbType.Int,4),
					new SqlParameter("@AdminUsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.ChatId;
            parameters[5].Value = model.ChatName;
            parameters[6].Value = model.BlackWhy;
            parameters[7].Value = model.BlackDay;
            parameters[8].Value = model.AdminUsID;
            parameters[9].Value = model.ExitTime;
            parameters[10].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Chatblack ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteStr(int ChatId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Chatblack ");
            strSql.Append(" where ChatId=@ChatId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4)};
            parameters[0].Value = ChatId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 解除黑名单
        /// </summary>
        public void Delete(int ChatId, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Chatblack ");
            strSql.Append(" where ChatId=@ChatId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ChatId;
            parameters[1].Value = UsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Chatblack GetChatblack(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,ChatId,ChatName,BlackWhy,BlackDay,AdminUsID,ExitTime,AddTime from tb_Chatblack ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Chatblack model = new BCW.Model.Chatblack();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.ChatId = reader.GetInt32(4);
                    model.ChatName = reader.GetString(5);
                    model.BlackWhy = reader.GetString(6);
                    model.BlackDay = reader.GetInt32(7);
                    model.AdminUsID = reader.GetInt32(8);
                    model.ExitTime = reader.GetDateTime(9);
                    model.AddTime = reader.GetDateTime(10);
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
            strSql.Append(" FROM tb_Chatblack ");
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
        /// <returns>IList Chatblack</returns>
        public IList<BCW.Model.Chatblack> GetChatblacks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Chatblack> listChatblacks = new List<BCW.Model.Chatblack>();
            string sTable = "tb_Chatblack";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,ChatId,ChatName,BlackWhy,BlackDay,AdminUsID,ExitTime,AddTime";
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
                    return listChatblacks;
                }
                while (reader.Read())
                {
                    BCW.Model.Chatblack objChatblack = new BCW.Model.Chatblack();
                    objChatblack.ID = reader.GetInt32(0);
                    objChatblack.Types = reader.GetInt32(1);
                    objChatblack.UsID = reader.GetInt32(2);
                    objChatblack.UsName = reader.GetString(3);
                    objChatblack.ChatId = reader.GetInt32(4);
                    objChatblack.ChatName = reader.GetString(5);
                    objChatblack.BlackWhy = reader.GetString(6);
                    objChatblack.BlackDay = reader.GetInt32(7);
                    objChatblack.AdminUsID = reader.GetInt32(8);
                    objChatblack.ExitTime = reader.GetDateTime(9);
                    objChatblack.AddTime = reader.GetDateTime(10);
                    listChatblacks.Add(objChatblack);
                }
            }
            return listChatblacks;
        }

        #endregion  成员方法
    }
}

