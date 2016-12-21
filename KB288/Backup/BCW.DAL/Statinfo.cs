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
    /// ���ݷ�����Statinfo��
    /// </summary>
    public class Statinfo
    {
        public Statinfo()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Statinfo");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Statinfo");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���������õ���¼��
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Statinfo");

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
        /// ���������õ�IP��
        /// </summary>
        public int GetIPCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(DISTINCT IP) from tb_Statinfo");

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
        /// ����һ������
        /// </summary>
        //public int Add(BCW.Model.Statinfo model)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("insert into tb_Statinfo(");
        //    strSql.Append("IP,PUrl,Browser,System,AddTime)");
        //    strSql.Append(" values (");
        //    strSql.Append("@IP,@PUrl,@Browser,@System,@AddTime)");
        //    strSql.Append(";select @@IDENTITY");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@IP", SqlDbType.VarChar,15),
        //            new SqlParameter("@PUrl", SqlDbType.VarChar,50),
        //            new SqlParameter("@Browser", SqlDbType.VarChar,100),
        //            new SqlParameter("@System", SqlDbType.VarChar,50),
        //            new SqlParameter("@AddTime", SqlDbType.DateTime)};
        //    parameters[0].Value = model.IP;
        //    parameters[1].Value = model.PUrl;
        //    parameters[2].Value = model.Browser;
        //    parameters[3].Value = model.System;
        //    parameters[4].Value = model.AddTime;

        //    object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
        //    if (obj == null)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return Convert.ToInt32(obj);
        //    }
        //}

        /// <summary>
        /// ʹ�ô洢��������һ������
        /// </summary>
        public int Add(BCW.Model.Statinfo model)
        {
            IDataParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@IP", SqlDbType.VarChar, 15);
            parameters[1] = new SqlParameter("@PUrl", SqlDbType.VarChar, 50);
            parameters[2] = new SqlParameter("@Browser", SqlDbType.VarChar, 100);
            parameters[3] = new SqlParameter("@System", SqlDbType.VarChar, 100);
            parameters[4] = new SqlParameter("@AddTime", SqlDbType.DateTime);

            parameters[0].Value = model.IP;
            parameters[1].Value = model.PUrl;
            parameters[2].Value = model.Browser;
            parameters[3].Value = model.System;
            parameters[4].Value = model.AddTime;

            int rowsAffected = SqlHelper.ExecuteRunProcedure("UP_tb_Statinfo_ADD", parameters);
            return rowsAffected;
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Statinfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Statinfo set ");
            strSql.Append("IP=@IP,");
            strSql.Append("PUrl=@PUrl,");
            strSql.Append("Browser=@Browser,");
            strSql.Append("System=@System,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,15),
					new SqlParameter("@PUrl", SqlDbType.VarChar,50),
					new SqlParameter("@Browser", SqlDbType.VarChar,50),
					new SqlParameter("@System", SqlDbType.VarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.IP;
            parameters[2].Value = model.PUrl;
            parameters[3].Value = model.Browser;
            parameters[4].Value = model.System;
            parameters[5].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Statinfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Statinfo GetStatinfo(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,IP,PUrl,Browser,System,AddTime from tb_Statinfo ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Statinfo model = new BCW.Model.Statinfo();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.IP = reader.GetString(1);
                    model.PUrl = reader.GetString(2);
                    model.Browser = reader.GetString(3);
                    model.System = reader.GetString(4);
                    model.AddTime = reader.GetDateTime(5);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strSql)
        {
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Statinfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetStatinfos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listStatinfos = new List<BCW.Model.Statinfo>();
            string sTable = "tb_Statinfo";
            string sPkey = "id";
            string sField = "ID,IP,PUrl,Browser,System,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listStatinfos;
                }
                while (reader.Read())
                {
                    BCW.Model.Statinfo objStatinfo = new BCW.Model.Statinfo();
                    objStatinfo.ID = reader.GetInt32(0);
                    objStatinfo.IP = reader.GetString(1);
                    objStatinfo.PUrl = reader.GetString(2);
                    objStatinfo.Browser = reader.GetString(3);
                    objStatinfo.System = reader.GetString(4);
                    objStatinfo.AddTime = reader.GetDateTime(5);
                    listStatinfos.Add(objStatinfo);
                }
            }
            return listStatinfos;
        }

        /// <summary>
        /// ȡ��IP��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetIPs(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listIP = new List<BCW.Model.Statinfo>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT IP) FROM tb_Statinfo";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listIP;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT COUNT(*) AS count, IP FROM tb_Statinfo GROUP BY IP ORDER BY count Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Statinfo objIP = new BCW.Model.Statinfo();
                        objIP.IpCount = reader.GetInt32(0);
                        objIP.IP = reader.GetString(1);
                        listIP.Add(objIP);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listIP;
        }

        /// <summary>
        /// ȡ��Browser��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetBrowsers(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listBrowser = new List<BCW.Model.Statinfo>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT Browser) FROM tb_Statinfo";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBrowser;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT COUNT(*) AS count, Browser FROM tb_Statinfo GROUP BY Browser ORDER BY count Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Statinfo objBrowser = new BCW.Model.Statinfo();
                        objBrowser.BrowserCount = reader.GetInt32(0);
                        objBrowser.Browser = reader.GetString(1);
                        listBrowser.Add(objBrowser);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBrowser;
        }

        /// <summary>
        /// ȡ��System��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetSystems(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listSystem = new List<BCW.Model.Statinfo>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT System) FROM tb_Statinfo";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listSystem;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT COUNT(*) AS count, System FROM tb_Statinfo GROUP BY System ORDER BY count Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Statinfo objSystem = new BCW.Model.Statinfo();
                        objSystem.SystemCount = reader.GetInt32(0);
                        objSystem.System = reader.GetString(1);
                        listSystem.Add(objSystem);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listSystem;
        }

        /// <summary>
        /// ȡ��PUrl��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetPUrls(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listPurl = new List<BCW.Model.Statinfo>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT Purl) FROM tb_Statinfo";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listPurl;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT COUNT(*) AS count, Purl FROM tb_Statinfo GROUP BY Purl ORDER BY count Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Statinfo objPurl = new BCW.Model.Statinfo();
                        objPurl.PUrlCount = reader.GetInt32(0);
                        objPurl.PUrl = reader.GetString(1);
                        listPurl.Add(objPurl);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listPurl;
        }
        #endregion  ��Ա����
    }
}

