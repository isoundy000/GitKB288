using System;
using System.Configuration;

namespace BCW.Update
{
    
    public class PubConstant
    {        
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {           
            get 
            {
                string DataBaseServer = GetConnectionString("DataBaseServer"); 
                string DataBaseName = GetConnectionString("DataBaseName");
                string DataBaseUid = GetConnectionString("DataBaseUid");
                string DataBasePwd = GetConnectionString("DataBasePwd");

                string _connectionString = "server=" + DataBaseServer + ";database=" + DataBaseName + ";uid=" + DataBaseUid + ";pwd=" + DataBasePwd + ";";   

                return _connectionString; 
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
            string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
         
            return connectionString;
        }
    }
}
