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
    /// 数据访问类Vbook。
    /// </summary>
    public class Vbook
    {
        public Vbook()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Vbook");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Vbook");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.Model.Vbook model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Vbook(");
            strSql.Append("Types,Title,Content,SyText,Face,UsID,UsName,AddUsIP,AddTime,ReName,ReText,ReTime,VPwd)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@Content,@SyText,@Face,@UsID,@UsName,@AddUsIP,@AddTime,@ReName,@ReText,@ReTime,@VPwd)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@SyText", SqlDbType.NVarChar,200),
					new SqlParameter("@Face", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReText", SqlDbType.NVarChar,500),
					new SqlParameter("@ReTime", SqlDbType.DateTime),
					new SqlParameter("@VPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Content;
            parameters[3].Value = model.SyText;
            parameters[4].Value = model.Face;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.AddUsIP;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.ReName;
            parameters[10].Value = model.ReText;
            parameters[11].Value = model.ReTime;
            parameters[12].Value = model.VPwd;

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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Vbook model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Vbook(");
            strSql.Append("Types,Title,Content,SyText,Face,UsID,UsName,AddUsIP,AddTime,VPwd)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@Content,@SyText,@Face,@UsID,@UsName,@AddUsIP,@AddTime,@VPwd)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@SyText", SqlDbType.NVarChar,200),
					new SqlParameter("@Face", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@VPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Content;
            parameters[3].Value = model.SyText;
            parameters[4].Value = model.Face;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.AddUsIP;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.VPwd;

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
        public void Update(BCW.Model.Vbook model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Vbook set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("SyText=@SyText,");
            strSql.Append("Face=@Face,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("ReText=@ReText,");
            strSql.Append("ReTime=@ReTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@SyText", SqlDbType.NVarChar,200),
					new SqlParameter("@Face", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReText", SqlDbType.NVarChar,500),
					new SqlParameter("@ReTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.SyText;
            parameters[5].Value = model.Face;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.ReName;
            parameters[8].Value = model.ReText;
            parameters[9].Value = model.ReTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Vbook ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Vbook GetVbook(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,Title,Content,SyText,Face,UsID,UsName,AddUsIP,AddTime,ReName,ReText,ReTime,Notes,VPwd from tb_Vbook ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Vbook model = new BCW.Model.Vbook();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.Content = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.SyText = reader.GetString(4);
                    model.Face = reader.GetInt32(5);
                    model.UsID = reader.GetInt32(6);
                    model.UsName = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        model.AddUsIP = reader.GetString(8);
                    model.AddTime = reader.GetDateTime(9);
                    if (!reader.IsDBNull(10))
                        model.ReName = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        model.ReText = reader.GetString(11);
                    if (!reader.IsDBNull(12))
                        model.ReTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.Notes = reader.GetString(13);
                    if (!reader.IsDBNull(14))
                        model.VPwd = reader.GetString(14);
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
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,Types,Title,Content,SyText,Face,UsID,UsName,AddUsIP,AddTime,ReName,ReText,ReTime,Notes ");
            strSql.Append(" FROM tb_Vbook ");
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
        /// <returns>IList Vbook</returns>
        public IList<BCW.Model.Vbook> GetVbooks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Vbook> listVbooks = new List<BCW.Model.Vbook>();
            string sTable = "tb_Vbook";
            string sPkey = "id";
            string sField = "ID,Title,VPwd,ReText";
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
                    return listVbooks;
                }
                while (reader.Read())
                {
                    BCW.Model.Vbook objVbook = new BCW.Model.Vbook();
                    objVbook.ID = reader.GetInt32(0);
                    objVbook.Title = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        objVbook.VPwd = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        objVbook.ReText = reader.GetString(3);
                    listVbooks.Add(objVbook);
                }
            }
            return listVbooks;
        }

        #endregion  成员方法
    }
}
