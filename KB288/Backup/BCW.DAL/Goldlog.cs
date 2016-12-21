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
    /// 数据访问类Goldlog。
    /// </summary>
    public class Goldlog
    {
        public Goldlog()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Goldlog");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Goldlog");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// N秒内，此ID是否消费过
        /// </summary>
        public bool ExistsUsID(int UsID, int Sec)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Goldlog");
            strSql.Append(" where UsID=@UsID and AddTime>'" + DateTime.Now.AddSeconds(-Sec) + "'");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Goldlog model)
        {
            //----------------------写入日志文件作永久保存
            try
            {
                int uid = model.UsId;
                string Path = "/log/gold/" + uid + "/" + DESEncrypt.Encrypt(uid.ToString(), "kubaoLogenpt") + "/" + DateTime.Now.Year + "/";
                if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Path)) == false)//如果不存在就创建文件夹
                {
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(Path));
                }
                string FilePath = System.Web.HttpContext.Current.Server.MapPath(Path + "" + DateTime.Now.Month + "_" + model.Types + ".log");
                LogHelper.WriteGoldLog(FilePath, DT.FormatDate(model.AddTime, 0) + "#" + model.AcText + "#" + model.AcGold + "#" + model.AfterGold + "");
            }
            catch { }
            //----------------------写入日志文件作永久保存

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Goldlog(");
            strSql.Append("Types,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag)");
            strSql.Append(" values (");
            strSql.Append("@Types,@PUrl,@UsId,@UsName,@AcGold,@AfterGold,@AcText,@AddTime,@BbTag)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AcGold", SqlDbType.BigInt,8),
					new SqlParameter("@AfterGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AcText", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@BbTag", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.PUrl;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.AcGold;
            parameters[5].Value = model.AfterGold;
            parameters[6].Value = model.AcText;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.BbTag;

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
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Goldlog ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Goldlog GetGoldlog(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,PUrl,UsId,UsName,AcGold,AfterGold,AddTime,BbTag from tb_Goldlog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Goldlog model = new BCW.Model.Goldlog();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.PUrl = reader.GetString(1);
                    model.UsId = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.AcGold = reader.GetInt64(4);
                    model.AfterGold = reader.GetInt64(5);
                    model.AddTime = reader.GetDateTime(6);
                    model.BbTag = reader.GetByte(7);
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
            strSql.Append(" FROM tb_Goldlog ");
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
        /// <returns>IList Goldlog</returns>
        public IList<BCW.Model.Goldlog> GetGoldlogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Goldlog> listGoldlogs = new List<BCW.Model.Goldlog>();
            string sTable = "tb_Goldlog";
            string sPkey = "id";
            string sField = "ID,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag";
            string sCondition = strWhere;
            string sOrder = "AddTime Desc,ID Desc";
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
                    return listGoldlogs;
                }
                while (reader.Read())
                {
                    BCW.Model.Goldlog objGoldlog = new BCW.Model.Goldlog();
                    objGoldlog.ID = reader.GetInt32(0);
                    objGoldlog.PUrl = reader.GetString(1);
                    objGoldlog.UsId = reader.GetInt32(2);
                    objGoldlog.UsName = reader.GetString(3);
                    objGoldlog.AcGold = reader.GetInt64(4);
                    objGoldlog.AfterGold = reader.GetInt64(5);
                    objGoldlog.AcText = reader.GetString(6);
                    objGoldlog.AddTime = reader.GetDateTime(7);
                    objGoldlog.BbTag = reader.GetByte(8);
                    listGoldlogs.Add(objGoldlog);
                }
            }
            return listGoldlogs;
        }
        /// <summary>
        /// 消息日志重复查询记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Goldlog> GetGoldlogsCF(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Goldlog> listGoldlog = new List<BCW.Model.Goldlog>();
            string strWhe = string.Empty;
            if (strWhere != "")
                strWhe += "where " + strWhere;

            // 计算记录数
            string countString = "select usid,AcText,AcGold,CONVERT(varchar(100),addtime, 23) from tb_Goldlog " + strWhe + " group by usid,AcText,AcGold,CONVERT(varchar(100),addtime, 23) having count(CONVERT(varchar(100),addtime, 23))>1";
            DataSet ds = SqlHelper.Query(countString);
            p_recordCount = 0;

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    p_recordCount = Convert.ToInt32((ds.Tables[0].Rows.Count));
                }
            }

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listGoldlog;
            }
            // 取出相关记录

            string queryString = "select usid,AcText,AcGold,CONVERT(varchar(100),addtime, 23) as addtime from tb_Goldlog " + strWhe + " group by usid,AcText,AcGold,CONVERT(varchar(100),addtime, 23) having count(CONVERT(varchar(100),addtime, 23))>1 order by CONVERT(varchar(100),addtime, 23) desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Goldlog objGoldlog = new BCW.Model.Goldlog();
                        objGoldlog.UsId = reader.GetInt32(0);
                        objGoldlog.AcText = reader.GetString(1);
                        objGoldlog.AcGold = reader.GetInt64(2);
                        objGoldlog.AddTime = DateTime.Parse(reader.GetString(3));
                        listGoldlog.Add(objGoldlog);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listGoldlog;
        }

        #endregion  成员方法
    }
}