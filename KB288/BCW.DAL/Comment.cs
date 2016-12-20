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
    /// 数据访问类Comment。
    /// </summary>
    public class Comment
    {
        public Comment()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Comment");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Comment");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某会员ID发表的评论数
        /// </summary>
        public int GetCount(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Comment");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
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
        public int Add(BCW.Model.Comment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Comment(");
            strSql.Append("NodeId,Types,DetailId,UserId,UserName,Face,Content,AddUsIP,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@NodeId,@Types,@DetailId,@UserId,@UserName,@Face,@Content,@AddUsIP,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
				    new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Face", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.NodeId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.DetailId;
            parameters[3].Value = model.UserId;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Face;
            parameters[6].Value = model.Content;
            parameters[7].Value = model.AddUsIP;
            parameters[8].Value = model.AddTime;

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
        public void Update(BCW.Model.Comment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Comment set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Types=@Types,");
            strSql.Append("DetailId=@DetailId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Face=@Face,");
            strSql.Append("Content=@Content,");
            strSql.Append("AddUsIP=@AddUsIP,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("ReText=@ReText");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Face", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ReText", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Types;
            parameters[3].Value = model.DetailId;
            parameters[4].Value = model.UserId;
            parameters[5].Value = model.UserName;
            parameters[6].Value = model.Face;
            parameters[7].Value = model.Content;
            parameters[8].Value = model.AddUsIP;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.ReText;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新回复内容
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Comment set ");
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
            strSql.Append("delete from tb_Comment ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Comment ");
            strSql.Append(" where DetailId=@DetailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DetailId", SqlDbType.Int,4)};
            parameters[0].Value = DetailId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete3(int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Comment ");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Comment ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到DetailId
        /// </summary>
        public int GetDetailId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DetailId from tb_Comment ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Comment GetCommentMe(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DetailId,Types from tb_Comment ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Comment model = new BCW.Model.Comment();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.DetailId = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int DetailId, int TopNum, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP " + TopNum + " ID,UserName,Content,AddTime,ReText ");
            strSql.Append(" FROM tb_Comment ");
            strSql.Append(" where DetailId=@DetailId ");
            if (Types == 0)
            {
                strSql.Append(" and Types<>@Types ");
            }
            else
            {
                strSql.Append(" and Types=@Types ");
            }
            strSql.Append(" Order By ID DESC");
            SqlParameter[] parameters = {
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = DetailId;
            parameters[1].Value = 14;

            return SqlHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Comment</returns>
        public IList<BCW.Model.Comment> GetComments(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Comment> listComments = new List<BCW.Model.Comment>();

            string sTable = "tb_Comment";
            string sPkey = "id";
            string sField = "ID,DetailId,UserId,UserName,Face,Content,AddUsIP,AddTime,ReText,Types";
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

                    return listComments;
                }

                while (reader.Read())
                {
                    BCW.Model.Comment objComment = new BCW.Model.Comment();
                    objComment.ID = reader.GetInt32(0);
                    objComment.DetailId = reader.GetInt32(1);
                    objComment.UserId = reader.GetInt32(2);
                    objComment.UserName = reader.GetString(3);
                    objComment.Face = reader.GetInt32(4);
                    objComment.Content = reader.GetString(5);
                    objComment.AddUsIP = reader.GetString(6);
                    objComment.AddTime = reader.GetDateTime(7);
                    if (!reader.IsDBNull(8))
                        objComment.ReText = reader.GetString(8);
                    objComment.Types = reader.GetInt32(9);
                    listComments.Add(objComment);

                }
            }

            return listComments;
        }
        #endregion  成员方法
    }
}
