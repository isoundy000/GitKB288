using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Configuration;
using BCW.Data;

namespace BCW.PK10
{
    public abstract class MySqlHelper
    {
        public static string connectionString = PubConstant.ConnectionString;
        #region 执行SQL命令函数
        public static int ExecuteSql(string SQLString)
        {
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        rows=ExecuteSql(SQLString, conn,trans);
                        trans.Commit();
                    }
                    catch (SqlException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
                conn.Close();
            }
            return rows;
        }
        public static int ExecuteSql(string SQLString, SqlConnection conn,SqlTransaction trans)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, conn,trans))
            {
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
            }
        }
        public static object GetSingle(string SQLString)
        {
            object obj = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        obj= GetSingle(conn, trans, SQLString);
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                conn.Close();
            }
            return obj;
        }
        public static object GetSingle(SqlConnection conn, SqlTransaction trans,string SQLString)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, conn, trans))
            {
                try
                {
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        public static int InsertAndGetID(string SQLString, SqlConnection conn, SqlTransaction trans) //向数据表添加一很记录，并返回自增ID
        {
            int nID = 0;
            SQLString += ";select SCOPE_IDENTITY() as id";  //加入一句
            //返回自增ID（SCOPE_IDENTITY 和 @@IDENTITY 返回在当前会话中的任何表内所生成的最后一个标识值。但是，SCOPE_IDENTITY 只返回插入到当前作用域中的值；@@IDENTITY 不受限于特定的作用域。）
            //并发操作时，@@IDENTITY可能会取错。
            using (SqlCommand cmd = new SqlCommand(SQLString, conn, trans))
            {
                try
                {
                    SqlDataAdapter Ada = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    Ada.Fill(dt);
                    if(dt.Rows.Count>0)
                        nID = int.Parse(dt.Rows[0]["id"].ToString());
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
            }
            return nID;
        }
        public static void ExecuteSqlTran(ArrayList SQLStringList) //批处理（事务封装）
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }

        public static SqlDataReader GetPageData(string sTable, string sPkey, string sField,string sCondition, string sOrder, int currentPageIndex, int pageSize,out int recordCount)
        {
            string storedProcName = "dt_getbasepage";
            int iSCounts = 0; //由存储过程dt_getbasepage计算总记录数
            recordCount = 0; //初始化输出值
            //
            IDataParameter[] parameters = new SqlParameter[8];

            parameters[0] = new SqlParameter("@sTable", SqlDbType.NVarChar, 50);//--表名
            parameters[1] = new SqlParameter("@sPkey", SqlDbType.NVarChar, 50);//--主键(一定要有)
            parameters[2] = new SqlParameter("@sField", SqlDbType.NVarChar, 800);//--字段
            parameters[3] = new SqlParameter("@iPageCurr", SqlDbType.Int);//--当前页数
            parameters[4] = new SqlParameter("@iPageSize", SqlDbType.Int);//--每页记录数
            parameters[5] = new SqlParameter("@sCondition", SqlDbType.NVarChar, 800);//--条件(不需要where)
            parameters[6] = new SqlParameter("@sOrder", SqlDbType.NVarChar, 50);//--排序(不需要order by,需要asc和desc字符)
            parameters[7] = new SqlParameter("@Counts", SqlDbType.Int);//--输出总条数

            parameters[0].Value = sTable;
            parameters[1].Value = sPkey;
            parameters[2].Value = sField;
            parameters[3].Value = currentPageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = sCondition;
            parameters[6].Value = sOrder;
            parameters[7].Value = iSCounts;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                SqlDataReader returnReader;
                connection.Open();
                SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);

                command.Parameters.Add(new SqlParameter("ReturnValue",
                    SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                    false, 0, 0, string.Empty, DataRowVersion.Default, null));

                command.ExecuteNonQuery();

                recordCount = (int)command.Parameters["ReturnValue"].Value;

                command.CommandType = CommandType.StoredProcedure;
                returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                command.Parameters.Clear();
                return returnReader;
            }
            catch (Exception e)
            {
                string ex = e.Message;
                connection.Close();
                return null;
            }
        }
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }
        #endregion
        #region 读取数据函数
        public static DataTable GetTable(string cSQL)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        dt = GetTable(conn, trans, cSQL);
                        trans.Commit();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            return dt;
        }
        public static DataTable GetTable(SqlConnection conn, SqlTransaction trans, string cSQL)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                //command properties
                cmd.Transaction = trans;
                cmd.CommandText = cSQL;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw new Exception("读取数据失败！\n" + ex.Message);
            }
            return dt;
        }
        #endregion
    }
}
