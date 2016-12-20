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
    /// 数据访问类Medal。
    /// </summary>
    public class Medal
    {
        public Medal()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Medal");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medal");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsVip(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medal");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and PayID LIKE '%#" + UsID + "#%'");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = -1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medal");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and PayID LIKE '%#" + UsID + "#%'");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTypes(int Types, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medal");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and PayID LIKE '%#" + UsID + "#%'");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsForumId(int ID, int ForumId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medal");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ForumId=@ForumId");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPayIDtemp(int ForumId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medal");
            strSql.Append(" where ForumId=@ForumId ");
            strSql.Append(" and PayIDtemp LIKE '%#" + UsID + "#%'");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = ForumId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Medal model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Medal(");
            strSql.Append("Types,Title,ImageUrl,Notes,iCent,iCount,iDay,Paixu,ForumId)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@ImageUrl,@Notes,@iCent,@iCount,@iDay,@Paixu,@ForumId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,10),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@iCent", SqlDbType.Int,4),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@iDay", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.ImageUrl;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.iCent;
            parameters[5].Value = model.iCount;
            parameters[6].Value = model.iDay;
            parameters[7].Value = model.Paixu;
            parameters[8].Value = model.ForumId;

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
        public void Update(BCW.Model.Medal model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Medal set ");
            strSql.Append("Title=@Title,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("iCent=@iCent,");
            strSql.Append("iCount=@iCount,");
            strSql.Append("iDay=@iDay,");
            strSql.Append("Paixu=@Paixu,");
            strSql.Append("ForumId=@ForumId");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,10),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@iCent", SqlDbType.Int,4),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@iDay", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.ImageUrl;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.iCent;
            parameters[5].Value = model.iCount;
            parameters[6].Value = model.iDay;
            parameters[7].Value = model.Paixu;
            parameters[8].Value = model.ForumId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新会员勋章
        /// </summary>
        public void UpdatePayID(int ID, string PayID, string PayExTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Medal set ");
            strSql.Append("PayID=@PayID,");
            strSql.Append("PayExTime=@PayExTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PayID", SqlDbType.NText),
					new SqlParameter("@PayExTime", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = PayID;
            parameters[2].Value = PayExTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新会临时勋章
        /// </summary>
        public void UpdatePayIDtemp(int ID, string PayIDtemp)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Medal set ");
            strSql.Append("PayIDtemp=@PayIDtemp");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PayIDtemp", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = PayIDtemp;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新会员勋章库存
        /// </summary>
        public void UpdateiCount(int ID, int iCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Medal set ");
            strSql.Append("iCount=iCount+@iCount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iCount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = iCount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Medal ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到购买记录实体
        /// </summary>
        public string GetImageUrl(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ImageUrl from tb_Medal ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 得到论坛个性标识
        /// </summary>
        public BCW.Model.Medal GetMedalForum(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title,ImageUrl,ForumId from tb_Medal ");
            strSql.Append(" where Types=@Types and PayID LIKE '%#" + UsID + "#%'");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = 2;

            BCW.Model.Medal model = new BCW.Model.Medal();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        model.Title = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        model.ImageUrl = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        model.ForumId = reader.GetInt32(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到购买记录实体
        /// </summary>
        public BCW.Model.Medal GetMedalMe(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PayID,PayExTime from tb_Medal ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Medal model = new BCW.Model.Medal();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        model.PayID = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        model.PayExTime = reader.GetString(1);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到购买记录实体
        /// </summary>
        public BCW.Model.Medal GetMedalMe(int ForumId, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PayID,PayExTime,ID,PayIDtemp from tb_Medal ");
            strSql.Append(" where ForumId=@ForumId and PayIDtemp LIKE '%#" + UsID + "#%'");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = ForumId;

            BCW.Model.Medal model = new BCW.Model.Medal();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        model.PayID = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        model.PayExTime = reader.GetString(1);

                    model.ID = reader.GetInt32(2);

                    if (!reader.IsDBNull(3))
                        model.PayIDtemp = reader.GetString(3);

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Medal GetMedal(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,Title,ImageUrl,Notes,iCent,iCount,iDay,Paixu,PayID,PayExTime,ForumId,PayIDtemp from tb_Medal ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Medal model = new BCW.Model.Medal();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.ImageUrl = reader.GetString(3);
                    model.Notes = reader.GetString(4);
                    model.iCent = reader.GetInt32(5);
                    model.iCount = reader.GetInt32(6);
                    model.iDay = reader.GetInt32(7);
                    model.Paixu = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        model.PayID = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        model.PayExTime = reader.GetString(10);

                    if (!reader.IsDBNull(11))
                        model.ForumId = reader.GetInt32(11);
                    else
                        model.ForumId = 0;

                    if (!reader.IsDBNull(12))
                        model.PayIDtemp = reader.GetString(12);
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
            strSql.Append(" FROM tb_Medal ");
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
        /// <returns>IList Medal</returns>
        public IList<BCW.Model.Medal> GetMedals(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Medal> listMedals = new List<BCW.Model.Medal>();
            string sTable = "tb_Medal";
            string sPkey = "id";
            string sField = "ID,Title,ImageUrl,Notes,iDay,iCount,iCent";
            string sCondition = strWhere;
            string sOrder = "Paixu Asc";
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
                    return listMedals;
                }
                while (reader.Read())
                {
                    BCW.Model.Medal objMedal = new BCW.Model.Medal();
                    objMedal.ID = reader.GetInt32(0);
                    objMedal.Title = reader.GetString(1);
                    objMedal.ImageUrl = reader.GetString(2);
                    objMedal.Notes = reader.GetString(3);
                    objMedal.iDay = reader.GetInt32(4);
                    objMedal.iCount = reader.GetInt32(5);
                    objMedal.iCent = reader.GetInt32(6);
                    listMedals.Add(objMedal);
                }
            }
            return listMedals;
        }

        #endregion  成员方法
    }
}

