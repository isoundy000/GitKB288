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
    /// 数据访问类FComment。
    /// </summary>
    public class FComment
    {
        public FComment()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_FComment");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_FComment");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int DetailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_FComment");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and DetailId=@DetailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = DetailId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某会员ID发表的评论数
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_FComment");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        public int Add(BCW.Model.FComment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_FComment(");
            strSql.Append("Types,DetailId,Content,UsID,UsName,AddUsIP,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@DetailId,@Content,@UsID,@UsName,@AddUsIP,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.DetailId;
            parameters[2].Value = model.Content;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.AddUsIP;
            parameters[6].Value = model.AddTime;

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
        public void Update(BCW.Model.FComment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_FComment set ");
            strSql.Append("Types=@Types,");
            strSql.Append("DetailId=@DetailId,");
            strSql.Append("Content=@Content,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("AddUsIP=@AddUsIP,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.DetailId;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.AddUsIP;
            parameters[7].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新回复内容
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_FComment set ");
            strSql.Append("ReText=@ReText");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReText", SqlDbType.NVarChar,500)};
            parameters[0].Value = ID;
            parameters[1].Value = ReText;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_FComment ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Types, int DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_FComment ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and DetailId=@DetailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = DetailId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_FComment ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.FComment GetFComment(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,DetailId,Content,UsID,UsName,AddUsIP,AddTime,ReText from tb_FComment ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.FComment model = new BCW.Model.FComment();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.DetailId = reader.GetInt32(2);
                    model.Content = reader.GetString(3);
                    model.UsID = reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.AddUsIP = reader.GetString(6);
                    model.AddTime = reader.GetDateTime(7);

                    if (!reader.IsDBNull(8))
                        model.ReText = reader.GetString(8);

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
            strSql.Append(" FROM tb_FComment ");
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
        /// <returns>IList FComment</returns>
        public IList<BCW.Model.FComment> GetFComments(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.FComment> listFComments = new List<BCW.Model.FComment>();
            string sTable = "tb_FComment";
            string sPkey = "id";
            string sField = "ID,Types,DetailId,Content,UsID,UsName,AddUsIP,AddTime,ReText";
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
                    return listFComments;
                }
                while (reader.Read())
                {
                    BCW.Model.FComment objFComment = new BCW.Model.FComment();
                    objFComment.ID = reader.GetInt32(0);
                    objFComment.Types = reader.GetInt32(1);
                    objFComment.DetailId = reader.GetInt32(2);
                    objFComment.Content = reader.GetString(3);
                    objFComment.UsID = reader.GetInt32(4);
                    objFComment.UsName = reader.GetString(5);
                    objFComment.AddUsIP = reader.GetString(6);
                    objFComment.AddTime = reader.GetDateTime(7);

                    if (!reader.IsDBNull(8))
                        objFComment.ReText = reader.GetString(8);

                    listFComments.Add(objFComment);
                }
            }
            return listFComments;
        }

        #endregion  成员方法
    }
}

