using System;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using BCW.Common;

namespace BCW.Data
{
    
    public class PubConstant
    {        
        /// <summary>
        /// 获取连接字符串（主站使用）
        /// </summary>
        public static string ConnectionString
        {           
            get 
            {
                string DataBaseServer = GetConnectionString("DataBaseServer"); 
                string DataBaseName = GetConnectionString("DataBaseName");
                string DataBaseUid = GetConnectionString("DataBaseUid");
                string DataBasePwd = GetConnectionString("DataBasePwd");

                //string _connectionString = "server=" + DataBaseServer + ";database=" + DataBaseName + ";uid=" + DataBaseUid + ";pwd=" + DataBasePwd + ";";
                string _connectionString = "data source=" + DataBaseServer + ";UID=" + DataBaseUid + ";pwd=" + DataBasePwd + ";Initial Catalog=" + DataBaseName + ";";
                return _connectionString;
            }
        }

        /// <summary>
        /// 获取连接字符串（共享会员数据使用）
        /// </summary>
        public static string ConnectionStringUser
        {
            get
            {
                string DataBaseServer = string.Empty;
                DataBaseServer = GetConnectionString("DataBaseServerUser");
                if (!string.IsNullOrEmpty(DataBaseServer))
                {
                    string DataBaseName = GetConnectionString("DataBaseNameUser");
                    string DataBaseUid = GetConnectionString("DataBaseUidUser");
                    string DataBasePwd = GetConnectionString("DataBasePwdUser");

                    string _connectionString = "server=" + DataBaseServer + ";database=" + DataBaseName + ";uid=" + DataBaseUid + ";pwd=" + DataBasePwd + ";";
                    return _connectionString;
                }
                else
                {
                    return ConnectionString;
                }
            }
        }



        /// <summary>
        /// 获取连接字符串（共享会员数据使用）
        /// </summary>
        public static string ConnectionStringBook
        {
            get
            {
                string DataBaseServer = string.Empty;
                DataBaseServer = GetConnectionString("DataBaseServerBook");
                if (!string.IsNullOrEmpty(DataBaseServer))
                {

                    string DataBaseName = GetConnectionString("DataBaseNameBook");
                    string DataBaseUid = GetConnectionString("DataBaseUidBook");
                    string DataBasePwd = GetConnectionString("DataBasePwdBook");

                    string _connectionString = "server=" + DataBaseServer + ";database=" + DataBaseName + ";uid=" + DataBaseUid + ";pwd=" + DataBasePwd + ";";
                    return _connectionString;
                }
                else
                {
                    return ConnectionString;
                }
            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];

            return connectionString;
        }

    }
}
