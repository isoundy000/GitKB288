using System;
using System.Configuration;

namespace BCW.Update
{
    
    public class PubConstant
    {        
        /// <summary>
        /// ��ȡ�����ַ���
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
        /// �õ�web.config������������ݿ������ַ�����
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
