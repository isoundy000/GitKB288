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
    /// 数据访问类ChatText。
    /// </summary>
    public class ChatText
    {
        public ChatText()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_ChatText");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ChatText");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某会员ID发表的聊天数
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ChatText");
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
        /// 计算发表的聊天数
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ChatText");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// 计算某时间段的闲聊发表数
        /// </summary>
        public int GetCount(DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ChatText");
            strSql.Append(" where AddTime>@AddTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = dt;
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
        public int Add(BCW.Model.ChatText model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ChatText(");
            strSql.Append("ChatId,UsID,UsName,ToID,ToName,Content,IsKiss,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@ChatId,@UsID,@UsName,@ToID,@ToName,@Content,@IsKiss,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToID", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@IsKiss", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ChatId;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.ToID;
            parameters[4].Value = model.ToName;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.IsKiss;
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
        public void Update(BCW.Model.ChatText model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ChatText set ");
            strSql.Append("ChatId=@ChatId,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ToID=@ToID,");
            strSql.Append("ToName=@ToName,");
            strSql.Append("Content=@Content,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToID", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ChatId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.ToID;
            parameters[5].Value = model.ToName;
            parameters[6].Value = model.Content;
            parameters[7].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(int ChatId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatText ");
            strSql.Append(" where ChatId=@ChatId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4)};
            parameters[0].Value = ChatId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatText ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除某聊天室N天前的数据
        /// </summary>
        public void DeleteStr(int ChatId, int Day)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatText ");
            strSql.Append(" where ChatId=@ChatId ");
            strSql.Append(" and AddTime<'" + DateTime.Now.AddDays(-Day) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4)};
            parameters[0].Value = ChatId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件删除聊天室数据
        /// </summary>
        public void DeleteStr2(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatText ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatText ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除某聊室某ID聊天数据
        /// </summary>
        public void Delete(int ChatId, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ChatText ");
            strSql.Append(" where ChatId=@ChatId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ChatId;
            parameters[1].Value = UsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.ChatText GetChatText(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ChatId,UsID,UsName,ToID,ToName,Content,AddTime from tb_ChatText ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.ChatText model = new BCW.Model.ChatText();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.ChatId = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.ToID = reader.GetInt32(4);
                    model.ToName = reader.GetString(5);
                    model.Content = reader.GetString(6);
                    model.AddTime = reader.GetDateTime(7);
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
            strSql.Append(" FROM tb_ChatText ");
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
        /// <returns>IList ChatText</returns>
        public IList<BCW.Model.ChatText> GetChatTexts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.ChatText> listChatTexts = new List<BCW.Model.ChatText>();
            string sTable = "tb_ChatText";
            string sPkey = "id";
            string sField = "ID,ChatId,UsID,UsName,ToID,ToName,Content,IsKiss,AddTime";
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
                    return listChatTexts;
                }
                while (reader.Read())
                {
                    BCW.Model.ChatText objChatText = new BCW.Model.ChatText();
                    objChatText.ID = reader.GetInt32(0);
                    objChatText.ChatId = reader.GetInt32(1);
                    objChatText.UsID = reader.GetInt32(2);
                    objChatText.UsName = reader.GetString(3);
                    objChatText.ToID = reader.GetInt32(4);
                    objChatText.ToName = reader.GetString(5);
                    objChatText.Content = reader.GetString(6);
                    objChatText.IsKiss = reader.GetByte(7);
                    objChatText.AddTime = reader.GetDateTime(8);
                    listChatTexts.Add(objChatText);
                }
            }
            return listChatTexts;
        }

        /// <summary>
        /// 发言排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.ChatText> GetChatTextsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.ChatText> listChatText = new List<BCW.Model.ChatText>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_ChatText Where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 100)
                    pageCount = 100;
            }
            else
            {
                return listChatText;
            }

            // 取出相关记录

            string queryString = "SELECT UsID, COUNT(ID) FROM tb_ChatText Where " + strWhere + " GROUP BY UsID ORDER BY COUNT(ID) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.ChatText objChatText = new BCW.Model.ChatText();
                        objChatText.UsID = reader.GetInt32(0);
                        objChatText.ChatId = reader.GetInt32(1);
                        listChatText.Add(objChatText);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listChatText;
        }


        #endregion  成员方法
    }
}

