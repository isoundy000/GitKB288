using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;

namespace BCW.Data
{
    public class SqlUp
    {

        /// <summary>
        /// 执行备份数据库
        /// </summary>
        /// <param name="DataName"></param>
        /// <param name="DataPath"></param>
        public void BackUp(string DataName, string DataPath)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("backup ");
            strSql.Append(" database " + DataName + " ");
            strSql.Append(" to disk='" + HttpContext.Current.Server.MapPath(DataPath) + "' ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 执行还原数据库
        /// </summary>
        /// <param name="DataName"></param>
        /// <param name="DataPath"></param>
        public void UpBack(string DataName, string DataPath)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("use master restore ");
            strSql.Append(" database " + DataName + " ");
            strSql.Append(" from disk='" + HttpContext.Current.Server.MapPath(DataPath) + "' ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 清空数据库日志
        /// </summary>
        /// <param name="DataName"></param>
        public void ClearDataLog(string DataName)
        {
            try
            {
                string cmdtxt1 = "";
                string cmdtxt2 = "";
                string cmdtxt3 = "";

                cmdtxt1 = "DUMP TRANSACTION [" + DataName + "] WITH NO_LOG";

                cmdtxt2 = "BACKUP LOG [" + DataName + "] WITH NO_LOG";

                cmdtxt3 = "DBCC SHRINKDATABASE([" + DataName + "])";

                SqlHelper.ExecuteSql(cmdtxt1);

                SqlHelper.ExecuteSql(cmdtxt2);

                SqlHelper.ExecuteSql(cmdtxt3);
            }
            catch { }
        }

        /// <summary>
        /// 初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
 
        }

        /// <summary>
        /// 初始化全部数据表
        /// </summary>
        public void ClearTable()
        {
 
        }

    }
}
