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
    /// 数据访问类File。
    /// </summary>
    public class File
    {
        public File()
        { }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_File");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某主题的文件数
        /// </summary>
        public int GetCount(int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_File");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = NodeId;

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
        public int Add(BCW.Model.File model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_File(");
            strSql.Append("Types,NodeId,Files,PrevFiles,Content,FileSize,FileExt,DownNum,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@Files,@PrevFiles,@Content,@FileSize,@FileExt,@DownNum,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Files", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevFiles", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,50),
					new SqlParameter("@FileSize", SqlDbType.BigInt,8),
					new SqlParameter("@FileExt", SqlDbType.NVarChar,50),
					new SqlParameter("@DownNum", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Files;
            parameters[3].Value = model.PrevFiles;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.FileSize;
            parameters[6].Value = model.FileExt;
            parameters[7].Value = model.DownNum;
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
        public void Update(BCW.Model.File model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_File set ");
            strSql.Append("Types=@Types,");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Files=@Files,");
            strSql.Append("PrevFiles=@PrevFiles,");
            strSql.Append("Content=@Content,");
            strSql.Append("FileSize=@FileSize,");
            strSql.Append("FileExt=@FileExt,");
            strSql.Append("DownNum=@DownNum,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Files", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevFiles", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,50),
					new SqlParameter("@FileSize", SqlDbType.BigInt,8),
					new SqlParameter("@FileExt", SqlDbType.NVarChar,50),
					new SqlParameter("@DownNum", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.Files;
            parameters[4].Value = model.PrevFiles;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.FileSize;
            parameters[7].Value = model.FileExt;
            parameters[8].Value = model.DownNum;
            parameters[9].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新下载次数
        /// </summary>
        public void UpdateDownNum(int ID, int DownNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_File set ");
            strSql.Append("DownNum=DownNum+@DownNum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@DownNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = DownNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_File ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete2(int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_File ");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.File GetFile(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,NodeId,Files,PrevFiles,Content,FileSize,FileExt,DownNum,AddTime from tb_File ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.File model = new BCW.Model.File();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.NodeId = reader.GetInt32(2);
                    model.Files = reader.GetString(3);
                    model.PrevFiles = reader.GetString(4);
                    model.Content = reader.GetString(5);
                    model.FileSize = reader.GetInt64(6);
                    model.FileExt = reader.GetString(7);
                    model.DownNum = reader.GetInt32(8);
                    model.AddTime = reader.GetDateTime(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个Files
        /// </summary>
        public string GetFiles(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Files from tb_File ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
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
            strSql.Append(" FROM tb_File ");
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
        /// <returns>IList File</returns>
        public IList<BCW.Model.File> GetFiles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.File> listFiles = new List<BCW.Model.File>();
            string sTable = "tb_File";
            string sPkey = "id";
            string sField = "ID,Types,NodeId,Files,PrevFiles,Content,FileSize,FileExt,DownNum,AddTime";
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
                    return listFiles;
                }
                while (reader.Read())
                {
                    BCW.Model.File objFile = new BCW.Model.File();
                    objFile.ID = reader.GetInt32(0);
                    objFile.Types = reader.GetInt32(1);
                    objFile.NodeId = reader.GetInt32(2);
                    objFile.Files = reader.GetString(3);
                    objFile.PrevFiles = reader.GetString(4);
                    objFile.Content = reader.GetString(5);
                    objFile.FileSize = reader.GetInt64(6);
                    objFile.FileExt = reader.GetString(7);
                    objFile.DownNum = reader.GetInt32(8);
                    objFile.AddTime = reader.GetDateTime(9);
                    listFiles.Add(objFile);
                }
            }
            return listFiles;
        }

        #endregion  成员方法
    }
}

