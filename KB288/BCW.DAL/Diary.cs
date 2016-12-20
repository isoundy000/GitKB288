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
    /// 数据访问类Diary。
    /// </summary>
    public class Diary
    {
        public Diary()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Diary");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Diary");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Diary");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某用户日记数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Diary");
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
        /// 计算某用户今天日记数量
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Diary");
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
        /// 计算某用户某分组日记数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Diary");
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Diary model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Diary(");
            strSql.Append("NodeId,Title,Weather,Content,UsID,UsName,IsTop,IsGood,ReplyNum,ReadNum,AddUsIP,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@NodeId,@Title,@Weather,@Content,@UsID,@UsName,@IsTop,@IsGood,@ReplyNum,@ReadNum,@AddUsIP,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Weather", SqlDbType.NVarChar,8),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1),
					new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
					new SqlParameter("@ReplyNum", SqlDbType.Int,4),
					new SqlParameter("@ReadNum", SqlDbType.Int,4),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.NodeId;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Weather;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = 0;
            parameters[7].Value = 0;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = model.AddUsIP;
            parameters[11].Value = model.AddTime;

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
        public void Update(BCW.Model.Diary model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Title=@Title,");
            strSql.Append("Weather=@Weather,");
            strSql.Append("Content=@Content,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Weather", SqlDbType.NVarChar,8),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Weather;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Diary model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set ");
            strSql.Append("Title=@Title,");
            strSql.Append("Weather=@Weather,");
            strSql.Append("Content=@Content,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("IsTop=@IsTop,");
            strSql.Append("IsGood=@IsGood,");
            strSql.Append("ReadNum=@ReadNum,");
            strSql.Append("AddUsIP=@AddUsIP,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Weather", SqlDbType.NVarChar,8),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1),
					new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
					new SqlParameter("@ReadNum", SqlDbType.Int,4),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Weather;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.IsTop;
            parameters[7].Value = model.IsGood;
            parameters[8].Value = model.ReadNum;
            parameters[9].Value = model.AddUsIP;
            parameters[10].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新日记内容
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set ");
            strSql.Append("Content=@Content ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Content", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = Content;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新回复数
        /// </summary>
        public void UpdateReplyNum(int ID, int ReplyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set ");
            strSql.Append("ReplyNum=ReplyNum+@ReplyNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@ReplyNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ReplyNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新阅读数
        /// </summary>
        public void UpdateReadNum(int ID, int ReadNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set ");
            strSql.Append("ReadNum=ReadNum+@ReadNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@ReadNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ReadNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 置顶/去顶
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set");
            strSql.Append(" IsTop=@IsTop ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsTop;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新某分组日记成为默认分类
        /// </summary>
        public void UpdateNodeId(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Diary set ");
            strSql.Append("NodeId=0");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Diary ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Diary ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Diary ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Diary ");
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
        /// 得到一个用户ID
        /// </summary>
        public int GetUsID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsID from tb_Diary ");
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
            strSql.Append("select  top 1 NodeId from tb_Diary ");
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
        /// 得到IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsTop from tb_Diary ");
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
        public BCW.Model.Diary GetDiary(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,NodeId,Title,Weather,Content,UsID,UsName,IsTop,IsGood,ReplyNum,ReadNum,AddUsIP,AddTime from tb_Diary ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Diary model = new BCW.Model.Diary();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.NodeId = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.Weather = reader.GetString(3);
                    model.Content = reader.GetString(4);
                    model.UsID = reader.GetInt32(5);
                    model.UsName = reader.GetString(6);
                    model.IsTop = reader.GetByte(7);
                    model.IsGood = reader.GetByte(8);
                    model.ReplyNum = reader.GetInt32(9);
                    model.ReadNum = reader.GetInt32(10);
                    model.AddUsIP = reader.GetString(11);
                    model.AddTime = reader.GetDateTime(12);
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
            strSql.Append(" FROM tb_Diary ");
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Diary</returns>
        public IList<BCW.Model.Diary> GetDiarys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Diary> listDiarys = new List<BCW.Model.Diary>();
            string sTable = "tb_Diary";
            string sPkey = "id";
            string sField = "ID,Title,IsTop,IsGood,ReplyNum,ReadNum,AddTime";
            string sCondition = strWhere;
            string sOrder = strOrder;
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
                    return listDiarys;
                }
                while (reader.Read())
                {
                    BCW.Model.Diary objDiary = new BCW.Model.Diary();
                    objDiary.ID = reader.GetInt32(0);
                    objDiary.Title = reader.GetString(1);
                    objDiary.IsTop = reader.GetByte(2);
                    objDiary.IsGood = reader.GetByte(3);
                    objDiary.ReplyNum = reader.GetInt32(4);
                    objDiary.ReadNum = reader.GetInt32(5);
                    objDiary.AddTime = reader.GetDateTime(6);
                    listDiarys.Add(objDiary);
                }
            }
            return listDiarys;
        }

        /// <summary>
        /// 显示N条新日记
        /// </summary>
        /// <param name="p_Size">显示条数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Diary</returns>
        public IList<BCW.Model.Diary> GetDiarysTop(int p_Size, string strWhere)
        {
            IList<BCW.Model.Diary> listDiarys = new List<BCW.Model.Diary>();
            // 取出相关记录

            string queryString = "SELECT TOP " + p_Size + " ID,Title,IsTop,IsGood,ReplyNum,ReadNum,AddTime FROM tb_Diary where " + strWhere + " Order BY IsTop Desc,ID Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                while (reader.Read())
                {
                    BCW.Model.Diary objDiary = new BCW.Model.Diary();
                    objDiary.ID = reader.GetInt32(0);
                    objDiary.Title = reader.GetString(1);
                    objDiary.IsTop = reader.GetByte(2);
                    objDiary.IsGood = reader.GetByte(3);
                    objDiary.ReplyNum = reader.GetInt32(4);
                    objDiary.ReadNum = reader.GetInt32(5);
                    objDiary.AddTime = reader.GetDateTime(6);
                    listDiarys.Add(objDiary);
                }
            }
            return listDiarys;
        }

        #endregion  成员方法
    }
}
