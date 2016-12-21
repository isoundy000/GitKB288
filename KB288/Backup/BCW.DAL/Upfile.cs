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
    /// 数据访问类Upfile。
    /// </summary>
    public class Upfile
    {
        public Upfile()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Upfile");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Upfile");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Upfile");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
 					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Types;
            parameters[2].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某用户今天上传文件数量
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Upfile");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
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
        /// 计算某用户文件数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Upfile");
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
        /// 计算某用户某相集相片数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Upfile");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

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
        /// 计算某用户未归类相片数量
        /// </summary>
        public int GetCount(int UsID, int Types, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Upfile");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;
            parameters[2].Value = NodeId;

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
        public int Add(BCW.Model.Upfile model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Upfile(");
            strSql.Append("Types,NodeId,UsID,ForumID,BID,ReID,Files,PrevFiles,Content,FileSize,FileExt,DownNum,Cent,IsVerify,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@UsID,@ForumID,@BID,@ReID,@Files,@PrevFiles,@Content,@FileSize,@FileExt,@DownNum,@Cent,@IsVerify,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@Files", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevFiles", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,50),
					new SqlParameter("@FileSize", SqlDbType.BigInt,8),
					new SqlParameter("@FileExt", SqlDbType.NVarChar,50),
					new SqlParameter("@DownNum", SqlDbType.Int,4),
					new SqlParameter("@Cent", SqlDbType.Int,4),
					new SqlParameter("@IsVerify", SqlDbType.TinyInt,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.ForumID;
            parameters[4].Value = model.BID;
            parameters[5].Value = model.ReID;
            parameters[6].Value = model.Files;
            parameters[7].Value = model.PrevFiles;
            parameters[8].Value = model.Content;
            parameters[9].Value = model.FileSize;
            parameters[10].Value = model.FileExt;
            parameters[11].Value = model.DownNum;
            parameters[12].Value = model.Cent;
            parameters[13].Value = model.IsVerify;
            parameters[14].Value = model.AddTime;

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
        public void Update(BCW.Model.Upfile model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upfile set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("Files=@Files,");
            strSql.Append("PrevFiles=@PrevFiles,");
            strSql.Append("Content=@Content,");
            strSql.Append("Isverify=@Isverify,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Files", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevFiles", SqlDbType.NVarChar,100),
					new SqlParameter("@Content", SqlDbType.NVarChar,50),
					new SqlParameter("@Isverify", SqlDbType.TinyInt,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.Files;
            parameters[3].Value = model.PrevFiles;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.IsVerify;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新描述信息
        /// </summary>
        public void Update(int ID, string Content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upfile set ");
            strSql.Append("Content=@Content");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Content;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 审核文件
        /// </summary>
        public void UpdateIsVerify(int ID, int IsVerify)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upfile set ");
            strSql.Append("IsVerify=@IsVerify");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsVerify", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsVerify;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新下载次数
        /// </summary>
        public void UpdateDownNum(int ID, int DownNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upfile set ");
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
        /// 更新某相集相片成为默认分类
        /// </summary>
        public void UpdateNodeIds(int UsID, int Types, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upfile set ");
            strSql.Append("NodeId=0");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 转移文件
        /// </summary>
        public void UpdateNodeId(int UsID, int ID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Upfile set ");
            strSql.Append("NodeId=@NodeId");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Upfile ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(int UsID, int Types, int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Upfile ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Upfile ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Content from tb_Upfile ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                     string getTitle= reader.GetString(0);
                     if (string.IsNullOrEmpty(getTitle))
                         getTitle = "无标题";

                     return getTitle;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到一个Files
        /// </summary>
        public string GetFiles(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Files from tb_Upfile ");
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
                    return "无标题";
                }
            }
        }

        /// <summary>
        /// 得到一个用户ID
        /// </summary>
        public int GetUsID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsID from tb_Upfile ");
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
        /// 得到一个Types
        /// </summary>
        public int GetTypes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types from tb_Upfile ");
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
        /// 得到一个NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 NodeId from tb_Upfile ");
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
                    return -1;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Upfile GetUpfileMe(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsID,BID,Files from tb_Upfile ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Upfile model = new BCW.Model.Upfile();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsID = reader.GetInt32(0);
                    model.BID = reader.GetInt32(1);
                    model.Files = reader.GetString(2);
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
        public BCW.Model.Upfile GetUpfile(int ID)
        {
            return GetUpfile(ID, -1);
        }
        public BCW.Model.Upfile GetUpfile(int ID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,NodeId,UsID,ForumID,BID,ReID,Files,PrevFiles,Content,FileSize,FileExt,DownNum,Cent,IsVerify,AddTime from tb_Upfile ");
            strSql.Append(" where ID=@ID ");
            if (Types != -1)
            {
                strSql.Append(" and Types=" + Types + " ");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Upfile model = new BCW.Model.Upfile();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.NodeId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.ForumID = reader.GetInt32(4);
                    model.BID = reader.GetInt32(5);
                    model.ReID = reader.GetInt32(6);
                    model.Files = reader.GetString(7);
                    model.PrevFiles = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        model.Content = reader.GetString(9);
                    else
                        model.Content = "无标题";
                    model.FileSize = reader.GetInt64(10);
                    model.FileExt = reader.GetString(11);
                    model.DownNum = reader.GetInt32(12);
                    model.Cent = reader.GetInt32(13);
                    model.IsVerify = reader.GetByte(14);
                    model.AddTime = reader.GetDateTime(15);
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
            strSql.Append(" FROM tb_Upfile ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Upfile</returns>
        public IList<BCW.Model.Upfile> GetUpfiles(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Upfile> listUpfiles = new List<BCW.Model.Upfile>();
            string sTable = "tb_Upfile";
            string sPkey = "id";
            string sField = "PrevFiles,Files,Content";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listUpfiles;
                }
                while (reader.Read())
                {
                    BCW.Model.Upfile objUpfile = new BCW.Model.Upfile();
                    objUpfile.PrevFiles = reader.GetString(0);
                    objUpfile.Files = reader.GetString(1);
                    objUpfile.Content = reader.GetString(2);
                    listUpfiles.Add(objUpfile);
                }
            }
            return listUpfiles;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Upfile</returns>
        public IList<BCW.Model.Upfile> GetUpfiles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Upfile> listUpfiles = new List<BCW.Model.Upfile>();
            string sTable = "tb_Upfile";
            string sPkey = "id";
            string sField = "ID,Types,NodeId,UsID,ForumID,BID,ReID,Files,PrevFiles,Content,FileSize,FileExt,DownNum,IsVerify,AddTime";
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
                    return listUpfiles;
                }
                while (reader.Read())
                {
                    BCW.Model.Upfile objUpfile = new BCW.Model.Upfile();
                    objUpfile.ID = reader.GetInt32(0);
                    objUpfile.Types = reader.GetInt32(1);
                    objUpfile.NodeId = reader.GetInt32(2);
                    objUpfile.UsID = reader.GetInt32(3);
                    objUpfile.ForumID = reader.GetInt32(4);
                    objUpfile.BID = reader.GetInt32(5);
                    objUpfile.ReID = reader.GetInt32(6);
                    objUpfile.Files = reader.GetString(7);
                    objUpfile.PrevFiles = reader.GetString(8);
                    objUpfile.Content = reader.GetString(9);
                    objUpfile.FileSize = reader.GetInt64(10);
                    objUpfile.FileExt = reader.GetString(11);
                    objUpfile.DownNum = reader.GetInt32(12);
                    objUpfile.IsVerify = reader.GetByte(13);
                    objUpfile.AddTime = reader.GetDateTime(14);
                    listUpfiles.Add(objUpfile);
                }
            }
            return listUpfiles;
        }

        #endregion  成员方法
    }
}
