using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
using System.Web;
namespace BCW.Draw.DAL
{
    /// <summary>
    /// 数据访问类Goldlog。
    /// </summary>
    public class DrawJifenlog
    {
        public DrawJifenlog()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_DrawJifenlog");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_DrawJifenlog");
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
            strSql.Append("select count(1) from tb_DrawJifenlog");
            strSql.Append(" where UsID=@UsID and AddTime>'" + DateTime.Now.AddSeconds(-Sec) + "'");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Draw.Model.DrawJifenlog model)
        {
            //----------------------写入日志文件作永久保存
            //try
            //{
            //    int uid = model.UsId;
            //    string Path = "/log/gold/" + uid + "/" + DESEncrypt.Encrypt(uid.ToString(), "kubaoLogenpt") + "/" + DateTime.Now.Year + "/";
            //    if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Path)) == false)//如果不存在就创建文件夹
            //    {
            //        System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(Path));
            //    }
            //    string FilePath = System.Web.HttpContext.Current.Server.MapPath(Path + "" + DateTime.Now.Month + "_" + model.Types + ".log");
            //    LogHelper.WriteGoldLog(FilePath, DT.FormatDate(model.AddTime, 0) + "#" + model.AcText + "#" + model.AcGold + "#" + model.AfterGold + "");
            //}
            //catch { }
            //----------------------写入日志文件作永久保存

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_DrawJifenlog(");
            strSql.Append("Types,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@PUrl,@UsId,@UsName,@AcGold,@AfterGold,@AcText,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AcGold", SqlDbType.BigInt,8),
					new SqlParameter("@AfterGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AcText", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.PUrl;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.AcGold;
            parameters[5].Value = model.AfterGold;
            parameters[6].Value = model.AcText;
            parameters[7].Value = model.AddTime;
     

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
            strSql.Append("delete from tb_DrawJifenlog ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 得到一天内某个用户是经过社区生成抽奖点值的值
        /// </summary>
        public int GetJfbyTz(int UsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(AcGold) from tb_DrawJifenlog  ");

            strSql.Append(" where UsId= " + UsId + " and datediff(d,AddTime,getdate())=0 and AcText like '%社区%' ");

            SqlParameter[] parameters = {
                    new SqlParameter("@AcGold", SqlDbType.Int)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到一天内某个用户是经过帖子生成抽奖点值的值
        /// </summary>
        public int GetJfbyGame(int UsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(AcGold) from tb_DrawJifenlog  ");

            strSql.Append(" where UsId= " + UsId + " and datediff(d,AddTime,getdate())=0 and AcText like '%消费%' ");

            SqlParameter[] parameters = {
                    new SqlParameter("@AcGold", SqlDbType.Int)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到一天内某个用户是经过帖子生成抽奖点值的值
        /// </summary>
        public int GetJfbyCz(int UsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(AcGold) from tb_DrawJifenlog  ");

            strSql.Append(" where UsId= " + UsId + " and datediff(d,AddTime,getdate())=0 and AcText like '%网上充值%' ");

            SqlParameter[] parameters = {
                    new SqlParameter("@AcGold", SqlDbType.Int)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawJifenlog GetJifenlog(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,PUrl,UsId,UsName,AcGold,AfterGold,AddTime from tb_DrawJifenlog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Draw.Model.DrawJifenlog model = new BCW.Draw.Model.DrawJifenlog();
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
            strSql.Append(" FROM tb_DrawJifenlog ");
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
        public IList<BCW.Draw.Model.DrawJifenlog> GetJifenlogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Draw.Model.DrawJifenlog> listJifenlogs = new List<BCW.Draw.Model.DrawJifenlog>();
            string sTable = "tb_DrawJifenlog";
            string sPkey = "id";
            string sField = "ID,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime";
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
                    return listJifenlogs;
                }
                while (reader.Read())
                {
                    BCW.Draw.Model.DrawJifenlog objDrawJifenlog = new BCW.Draw.Model.DrawJifenlog();
                    objDrawJifenlog.ID = reader.GetInt32(0);
                    objDrawJifenlog.PUrl = reader.GetString(1);
                    objDrawJifenlog.UsId = reader.GetInt32(2);
                    objDrawJifenlog.UsName = reader.GetString(3);
                    objDrawJifenlog.AcGold = reader.GetInt64(4);
                    objDrawJifenlog.AfterGold = reader.GetInt64(5);
                    objDrawJifenlog.AcText = reader.GetString(6);
                    objDrawJifenlog.AddTime = reader.GetDateTime(7);
                    listJifenlogs.Add(objDrawJifenlog);
                }
            }
            return listJifenlogs;
        }
        /// <summary>
        /// 消息日志重复查询记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Draw.Model.DrawJifenlog> GetJifenlogsCF(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Draw.Model.DrawJifenlog> listJifenlog = new List<BCW.Draw.Model.DrawJifenlog>();
            string strWhe = string.Empty;
            if (strWhere != "")
                strWhe += "where " + strWhere;

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_DrawJifenlog " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listJifenlog;
            }
            // 取出相关记录

            string queryString = "select usid,AcText,AcGold,addtime from tb_DrawJifenlog " + strWhe + " group by usid,AcText,AcGold,addtime having count(*)>1";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Draw.Model.DrawJifenlog objJifenlog = new BCW.Draw.Model.DrawJifenlog();
                        objJifenlog.UsId = reader.GetInt32(0);
                        objJifenlog.AcText = reader.GetString(1);
                        objJifenlog.AcGold = reader.GetInt64(2);
                        objJifenlog.AddTime = reader.GetDateTime(3);
                        listJifenlog.Add(objJifenlog);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listJifenlog;
        }

        #endregion  成员方法
    }
}