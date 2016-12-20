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
    /// 数据访问类Spkblack。
    /// </summary>
    public class Spkblack
    {
        public Spkblack()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Spkblack");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Spkblack");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Spkblack");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and ExitTime>=@ExitTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;
            parameters[2].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Spkblack model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Spkblack(");
            strSql.Append("Types,UsID,UsName,BlackWhy,BlackDay,AdminUsID,ExitTime,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@BlackWhy,@BlackDay,@AdminUsID,@ExitTime,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackWhy", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackDay", SqlDbType.Int,4),
					new SqlParameter("@AdminUsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.BlackWhy;
            parameters[4].Value = model.BlackDay;
            parameters[5].Value = model.AdminUsID;
            parameters[6].Value = model.ExitTime;
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
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Spkblack model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Spkblack set ");
            strSql.Append("Types=@Types,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
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
					new SqlParameter("@BlackWhy", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackDay", SqlDbType.Int,4),
					new SqlParameter("@AdminUsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.BlackWhy;
            parameters[5].Value = model.BlackDay;
            parameters[6].Value = model.AdminUsID;
            parameters[7].Value = model.ExitTime;
            parameters[8].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Spkblack ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UsID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Spkblack ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Spkblack GetSpkblack(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,BlackWhy,BlackDay,AdminUsID,ExitTime,AddTime from tb_Spkblack ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Spkblack model = new BCW.Model.Spkblack();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.BlackWhy = reader.GetString(4);
                    model.BlackDay = reader.GetInt32(5);
                    model.AdminUsID = reader.GetInt32(6);
                    model.ExitTime = reader.GetDateTime(7);
                    model.AddTime = reader.GetDateTime(8);
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
            strSql.Append(" FROM tb_Spkblack ");
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
        /// <returns>IList Spkblack</returns>
        public IList<BCW.Model.Spkblack> GetSpkblacks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Spkblack> listSpkblacks = new List<BCW.Model.Spkblack>();
            string sTable = "tb_Spkblack";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,BlackWhy,BlackDay,AdminUsID,ExitTime,AddTime";
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
                    return listSpkblacks;
                }
                while (reader.Read())
                {
                    BCW.Model.Spkblack objSpkblack = new BCW.Model.Spkblack();
                    objSpkblack.ID = reader.GetInt32(0);
                    objSpkblack.Types = reader.GetInt32(1);
                    objSpkblack.UsID = reader.GetInt32(2);
                    objSpkblack.UsName = reader.GetString(3);
                    objSpkblack.BlackWhy = reader.GetString(4);
                    objSpkblack.BlackDay = reader.GetInt32(5);
                    objSpkblack.AdminUsID = reader.GetInt32(6);
                    objSpkblack.ExitTime = reader.GetDateTime(7);
                    objSpkblack.AddTime = reader.GetDateTime(8);
                    listSpkblacks.Add(objSpkblack);
                }
            }
            return listSpkblacks;
        }

        #endregion  成员方法
    }
}

