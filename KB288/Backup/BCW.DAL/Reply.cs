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
    /// 数据访问类Reply。
    /// </summary>
    public class Reply
    {
        public Reply()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Reply");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Reply");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BID, int Floor)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Reply");
            strSql.Append(" where BID=@BID ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4)};
            parameters[0].Value = BID;
            parameters[1].Value = Floor;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据某论坛某用户ID计算回帖数
        /// </summary>
        public int GetCount(int UsID, int ForumId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Reply");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumId=@ForumId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumId;

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
        /// 根据某帖子某用户ID计算未被删除的回帖数
        /// </summary>
        public int GetCountExist(int UsID, int BID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Reply");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and BID=@BID and IsDel=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@BID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = BID;

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
        /// 根据某帖子某用户ID计算回帖数
        /// </summary>
        public int GetCount2(int UsID, int BID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Reply");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and BID=@BID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@BID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = BID;

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
        public int Add(BCW.Model.Reply model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Reply(");
            strSql.Append("Floor,ForumId,Bid,UsID,UsName,Content,FileNum,IsGood,IsTop,IsDel,ReplyId,AddTime,CentText)");
            strSql.Append(" values (");
            strSql.Append("@Floor,@ForumId,@Bid,@UsID,@UsName,@Content,@FileNum,@IsGood,@IsTop,@IsDel,@ReplyId,@AddTime,@CentText)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Floor", SqlDbType.Int,4),
                    new SqlParameter("@ForumId", SqlDbType.Int,4),
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Content", SqlDbType.NVarChar,500),
                    new SqlParameter("@FileNum", SqlDbType.Int,4),
                    new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsTop", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@ReplyId", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@CentText", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Floor;
            parameters[1].Value = model.ForumId;
            parameters[2].Value = model.Bid;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.FileNum;
            parameters[7].Value = 0;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = model.ReplyId;
            parameters[11].Value = model.AddTime;
            parameters[12].Value = model.CentText;

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
        public void Update(BCW.Model.Reply model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Content=@Content,");
            strSql.Append("IsGood=@IsGood,");
            strSql.Append("IsTop=@IsTop,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Content", SqlDbType.NVarChar,500),
                    new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsTop", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.IsGood;
            parameters[5].Value = model.IsTop;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int Bid, int Floor, string Content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("Content=@Content");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4),
                    new SqlParameter("@Content", SqlDbType.NVarChar,500)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;
            parameters[2].Value = Content;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新CentText
        /// </summary>
        public void UpdateCentText(int Bid, int Floor, string CentText)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("CentText=@CentText");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4),
                    new SqlParameter("@CentText", SqlDbType.NVarChar,50)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;
            parameters[2].Value = CentText;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新ForumID
        /// </summary>
        public void UpdateForumID(int ID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("ForumID=@ForumID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 批量转移回帖
        /// </summary>
        public void UpdateForumID2(int ForumID, int NewForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("ForumID=@NewForumID ");
            strSql.Append(" where ForumID=@ForumID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@NewForumID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = NewForumID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新回帖文件数
        /// </summary>
        public void UpdateFileNum(int ID, int FileNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("FileNum=FileNum+@FileNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@FileNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = FileNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 回帖加精/解精
        /// </summary>
        public void UpdateIsGood(int Bid, int Floor, int IsGood)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("IsGood=@IsGood ");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4),
                    new SqlParameter("@IsGood", SqlDbType.TinyInt,1)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;
            parameters[2].Value = IsGood;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 回帖置顶/去顶
        /// </summary>
        public void UpdateIsTop(int Bid, int Floor, int IsTop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set ");
            strSql.Append("IsTop=@IsTop ");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4),
                    new SqlParameter("@IsTop", SqlDbType.Int,1)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;
            parameters[2].Value = IsTop;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel(int BID, int IsDel)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set");
            strSql.Append(" IsDel=@IsDel ");
            strSql.Append(" where BID=@BID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = BID;
            parameters[1].Value = IsDel;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel1(int ID, int IsDel)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set");
            strSql.Append(" IsDel=@IsDel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsDel;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel2(int BID, int Floor, int IsDel)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set");
            strSql.Append(" IsDel=@IsDel ");
            strSql.Append(" where BID=@BID ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = BID;
            parameters[1].Value = Floor;
            parameters[2].Value = IsDel;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel3(int BID, int UsID, int IsDel)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Reply set");
            strSql.Append(" IsDel=@IsDel ");
            strSql.Append(" where BID=@BID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = BID;
            parameters[1].Value = UsID;
            parameters[2].Value = IsDel;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Reply ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Reply ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Bid, int Floor)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Reply ");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int Bid, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Reply ");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Bid;
            parameters[1].Value = UsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到新楼层
        /// </summary>
        public int GetFloor(int Bid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Floor from tb_Reply ");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" Order By ID Desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4)};
            parameters[0].Value = Bid;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0) + 1;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// 根据ID得到楼层
        /// </summary>
        public int GetFloor2(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Floor from tb_Reply ");
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
        /// 得到某楼层内容
        /// </summary>
        public string GetContent(int Bid, int Floor)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Content from tb_Reply ");
            strSql.Append(" where Bid=@Bid ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;

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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Reply GetByID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  UsID,UsName,Content,IsGood,IsTop,CentText,AddTime,ForumId,IsDel,Bid from tb_Reply ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Reply model = new BCW.Model.Reply();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsID = reader.GetInt32(0);
                    model.UsName = reader.GetString(1);
                    model.Content = reader.GetString(2);
                    model.IsGood = reader.GetByte(3);
                    model.IsTop = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        model.CentText = reader.GetString(5);
                    else
                        model.CentText = "";
                    model.AddTime = reader.GetDateTime(6);
                    model.ForumId = reader.GetInt32(7);
                    model.IsDel = reader.GetByte(8);
                    model.Bid = reader.GetInt32(9);

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
        public BCW.Model.Reply GetReplyMe(int BID, int Floor)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsID,UsName,Content,IsGood,IsTop,CentText,AddTime,ForumId from tb_Reply ");
            strSql.Append(" where BID=@BID ");
            strSql.Append(" and Floor=@Floor ");
            SqlParameter[] parameters = {
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4)};
            parameters[0].Value = BID;
            parameters[1].Value = Floor;

            BCW.Model.Reply model = new BCW.Model.Reply();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsID = reader.GetInt32(0);
                    model.UsName = reader.GetString(1);
                    model.Content = reader.GetString(2);
                    model.IsGood = reader.GetByte(3);
                    model.IsTop = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        model.CentText = reader.GetString(5);
                    else
                        model.CentText = "";
                    model.AddTime = reader.GetDateTime(6);
                    model.ForumId = reader.GetInt32(7);


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
        public BCW.Model.Reply GetReply(int Bid, int Floor)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Floor,Bid,UsID,UsName,Content,FileNum,IsGood,IsTop,IsDel,ReplyId,ReStats,AddTime,CentText from tb_Reply ");
            if (Floor == 0)
                strSql.Append(" where ID=@Bid ");
            else
            {
                strSql.Append(" where Bid=@Bid ");
                strSql.Append(" and Floor=@Floor ");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@Floor", SqlDbType.Int,4)};
            parameters[0].Value = Bid;
            parameters[1].Value = Floor;

            BCW.Model.Reply model = new BCW.Model.Reply();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Floor = reader.GetInt32(1);
                    model.Bid = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Content = reader.GetString(5);
                    model.FileNum = reader.GetInt32(6);
                    model.IsGood = reader.GetByte(7);
                    model.IsTop = reader.GetInt32(8);
                    model.IsDel = reader.GetByte(9);
                    model.ReplyId = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.ReStats = reader.GetString(11);
                    model.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.CentText = reader.GetString(13);
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
            strSql.Append(" FROM tb_Reply ");
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
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Reply> listReplys = new List<BCW.Model.Reply>();
            string sTable = "tb_Reply";
            string sPkey = "id";
            string sField = "ID,Floor,ForumId,Bid,UsID,UsName,Content,FileNum,IsGood,IsTop,IsDel,ReplyId,ReStats,AddTime,CentText";
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
                    return listReplys;
                }
                while (reader.Read())
                {
                    BCW.Model.Reply objReply = new BCW.Model.Reply();
                    objReply.ID = reader.GetInt32(0);
                    objReply.Floor = reader.GetInt32(1);
                    objReply.ForumId = reader.GetInt32(2);
                    objReply.Bid = reader.GetInt32(3);
                    objReply.UsID = reader.GetInt32(4);
                    objReply.UsName = reader.GetString(5);
                    objReply.Content = reader.GetString(6);
                    objReply.FileNum = reader.GetInt32(7);
                    objReply.IsGood = reader.GetByte(8);
                    objReply.IsTop = reader.GetInt32(9);
                    objReply.IsDel = reader.GetByte(10);
                    objReply.ReplyId = reader.GetInt32(11);
                    if (!reader.IsDBNull(12))
                        objReply.ReStats = reader.GetString(12);
                    objReply.AddTime = reader.GetDateTime(13);
                    if (!reader.IsDBNull(14))
                        objReply.CentText = reader.GetString(14);

                    listReplys.Add(objReply);
                }
            }
            return listReplys;
        }

        /// <summary>
        /// 帖子排行分页记录 陈志基 2016/08/10
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Reply> GetForumstats1(int p_pageIndex, int p_pageSize, string strWhere, int showtype, out int p_recordCount)
        {
            IList<BCW.Model.Reply> listForumstat = new List<BCW.Model.Reply>();
            string strWhe = string.Empty;
            if (strWhere != "" || showtype > 1)
                strWhe += " where ";

            if (strWhere != "")
                strWhe += strWhere;

            if (strWhere != "" && showtype > 1)
                strWhe += " and ";

            if (showtype == 2)  //本周
            {
                #region 本周
                string M_Str_mindate = string.Empty;
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                strWhe += " AddTime>='" + M_Str_mindate + "'";
                #endregion
            }
            else if (showtype == 3) //本月
            {
                #region 本月
                strWhe += " Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "";
                #endregion
            }
            else if (showtype == 4) //上月
            {
                #region 上月
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
                int ForYear = ForDate.Year;
                int ForMonth = ForDate.Month;
                strWhe += " Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "";
                #endregion
            }
            else if (showtype == 5) //上周
            {
                #region 上周
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToShortDateString());
                string M_Str_mindate = string.Empty;
                string M_Str_Maxdate = string.Empty;

                switch (ForDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = ForDate.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = ForDate.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = ForDate.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = ForDate.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = ForDate.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = ForDate.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = ForDate.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                M_Str_Maxdate = DateTime.Parse(M_Str_mindate).AddDays(6).ToShortDateString();
                strWhe += " AddTime between '" + M_Str_mindate + " 00:00:00' AND '" + M_Str_Maxdate + " 23:59:59'";
                #endregion
            }
            strWhe += "  and  IsDel=0 ";
            #region 计算记录数
            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Reply " + strWhe + "";
            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 100)
                p_recordCount = 100;
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listForumstat;
            }
            #endregion

            #region 取出相关记录数
            // 取出相关记录
            string queryString = "SELECT TOP 100 UsID,COUNT(UsID) FROM tb_Reply " + strWhe + " GROUP BY UsID ORDER BY COUNT(UsID) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Reply objForumstat = new BCW.Model.Reply();
                        objForumstat.UsID = reader.GetInt32(0);
                        //objForumstat.UsName = reader.GetString(1);
                        objForumstat.Floor = reader.GetInt32(1);//用ReadNum代替返回值

                        listForumstat.Add(objForumstat);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }
            #endregion

            return listForumstat;
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplysMe(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Reply> listReplys = new List<BCW.Model.Reply>();
            string sTable = "tb_Reply";
            string sPkey = "id";
            string sField = "ID,Bid,Content,AddTime";
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
                    return listReplys;
                }
                while (reader.Read())
                {
                    BCW.Model.Reply objReply = new BCW.Model.Reply();
                    objReply.ID = reader.GetInt32(0);
                    objReply.Bid = reader.GetInt32(1);
                    objReply.Content = reader.GetString(2);
                    objReply.AddTime = reader.GetDateTime(3);
                    listReplys.Add(objReply);
                }
            }
            return listReplys;
        }
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<BCW.Model.Reply> GetReplysWhere(string strWhere)
        {
            List<BCW.Model.Reply> listReplys = new List<BCW.Model.Reply>();
            // 取出相关记录

            string queryString = "SELECT Floor,UsName,Content,FileNum,ReplyId,AddTime,UsID,ForumId,IsDel FROM tb_Reply where " + strWhere + " Order BY ID Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                while (reader.Read())
                {
                    BCW.Model.Reply objReply = new BCW.Model.Reply();
                    objReply.Floor = reader.GetInt32(0);
                    objReply.UsName = reader.GetString(1);
                    objReply.Content = reader.GetString(2);
                    objReply.FileNum = reader.GetInt32(3);
                    objReply.ReplyId = reader.GetInt32(4);
                    objReply.AddTime = reader.GetDateTime(5);
                    objReply.UsID = reader.GetInt32(6);
                    objReply.ForumId = reader.GetInt32(7);
                    objReply.IsDel = reader.GetByte(8);
                    listReplys.Add(objReply);
                }
            }
            return listReplys;
        }
        /// <summary>
        /// 帖子页面回帖记录
        /// </summary>
        /// <param name="p_Size">显示条数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplysTop(int p_Size, string strWhere)
        {
            IList<BCW.Model.Reply> listReplys = new List<BCW.Model.Reply>();
            // 取出相关记录

            string queryString = "SELECT TOP " + p_Size + " Floor,UsName,Content,FileNum,ReplyId,AddTime,UsID FROM tb_Reply where " + strWhere + " Order BY ID Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                while (reader.Read())
                {
                    BCW.Model.Reply objReply = new BCW.Model.Reply();
                    objReply.Floor = reader.GetInt32(0);
                    objReply.UsName = reader.GetString(1);
                    objReply.Content = reader.GetString(2);
                    objReply.FileNum = reader.GetInt32(3);
                    objReply.ReplyId = reader.GetInt32(4);
                    objReply.AddTime = reader.GetDateTime(5);
                    objReply.UsID = reader.GetInt32(6);
                    listReplys.Add(objReply);
                }
            }
            return listReplys;
        }

        #endregion  成员方法
    }
}
