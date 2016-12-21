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
    /// 数据访问类Statinfo。
    /// </summary>
    public class Statinfo
    {
        public Statinfo()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Statinfo");
        }

        /// <summary>
        /// 是否存在该记录
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
        /// 根据条件得到记录数
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
        /// 根据条件得到IP数
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
        /// 增加一条数据
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
        /// 使用存储过程增加一条数据
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
        /// 更新一条数据
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
        /// 删除一组数据
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
        /// 得到一个对象实体
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strSql)
        {
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
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
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
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
                //计算总页数
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
        /// 取得IP记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetIPs(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listIP = new List<BCW.Model.Statinfo>();

            // 计算记录数
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

            // 取出相关记录

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
        /// 取得Browser记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetBrowsers(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listBrowser = new List<BCW.Model.Statinfo>();

            // 计算记录数
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

            // 取出相关记录

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
        /// 取得System记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetSystems(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listSystem = new List<BCW.Model.Statinfo>();

            // 计算记录数
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

            // 取出相关记录

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
        /// 取得PUrl记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Statinfo> GetPUrls(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Statinfo> listPurl = new List<BCW.Model.Statinfo>();

            // 计算记录数
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

            // 取出相关记录

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
        #endregion  成员方法
    }
}

