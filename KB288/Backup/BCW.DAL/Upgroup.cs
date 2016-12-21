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
    /// 数据访问类Upgroup。
    /// </summary>
    public class Upgroup
    {
        public Upgroup()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Upgroup");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Upgroup");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID, int Leibie)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Upgroup");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Leibie=@Leibie ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Leibie", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = Leibie;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTitle(int UsID, string Title, int Leibie)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Upgroup");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Title=@Title ");
            strSql.Append(" and Leibie=@Leibie ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Leibie", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Title;
            parameters[2].Value = Leibie;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Upgroup model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Upgroup(");
            strSql.Append("Leibie,Types,PostType,Title,UsID,IsReview,Paixu,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Leibie,@Types,@PostType,@Title,@UsID,@IsReview,@Paixu,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Leibie", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@PostType", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IsReview", SqlDbType.TinyInt,1),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Leibie;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.PostType;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.IsReview;
            parameters[6].Value = model.Paixu;
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
        public void Update(BCW.Model.Upgroup model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upgroup set ");
            strSql.Append("Types=@Types,");
            strSql.Append("PostType=@PostType,");
            strSql.Append("Title=@Title,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("IsReview=@IsReview,");
            strSql.Append("Paixu=@Paixu");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@PostType", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IsReview", SqlDbType.TinyInt,1),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.PostType;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.IsReview;
            parameters[6].Value = model.Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新封面
        /// </summary>
        public void UpdateNode(int ID, string Node)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upgroup set ");
            strSql.Append("Node=@Node");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Node", SqlDbType.NVarChar,100)};
            parameters[0].Value = ID;
            parameters[1].Value = Node;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Upgroup ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个Title
        /// </summary>
        public string GetTitle(int ID, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title from tb_Upgroup ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

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
        /// 得到一个Types
        /// </summary>
        public int GetTypes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types from tb_Upgroup ");
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
        /// 得到一个IsReview
        /// </summary>
        public int GetIsReview(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IsReview from tb_Upgroup ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
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
        public BCW.Model.Upgroup GetUpgroup(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Leibie,Types,PostType,Title,UsID,IsReview,Paixu,AddTime from tb_Upgroup ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Upgroup model = new BCW.Model.Upgroup();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Leibie = reader.GetInt32(1);
                    model.Types = reader.GetInt32(2);
                    model.PostType = reader.GetInt32(3);
                    model.Title = reader.GetString(4);
                    model.UsID = reader.GetInt32(5);
                    model.IsReview = reader.GetByte(6);
                    model.Paixu = reader.GetInt32(7);
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
            strSql.Append(" FROM tb_Upgroup ");
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
        /// <returns>IList Upgroup</returns>
        public IList<BCW.Model.Upgroup> GetUpgroups(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Upgroup> listUpgroups = new List<BCW.Model.Upgroup>();
            string sTable = "tb_Upgroup";
            string sPkey = "id";
            string sField = "ID,Types,Title,Node,UsID,Paixu";
            string sCondition = strWhere;
            string sOrder = "Paixu ASC";
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
                    return listUpgroups;
                }
                while (reader.Read())
                {
                    BCW.Model.Upgroup objUpgroup = new BCW.Model.Upgroup();
                    objUpgroup.ID = reader.GetInt32(0);
                    objUpgroup.Types = reader.GetInt32(1);
                    objUpgroup.Title = reader.GetString(2);
                    if(!reader.IsDBNull(3))
                        objUpgroup.Node = reader.GetString(3);

                    objUpgroup.UsID = reader.GetInt32(4);
                    objUpgroup.Paixu = reader.GetInt32(5);
                    listUpgroups.Add(objUpgroup);
                }
            }
            return listUpgroups;
        }

        #endregion  成员方法
    }
}
