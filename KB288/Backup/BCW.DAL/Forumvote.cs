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
    /// 数据访问类Forumvote。
    /// </summary>
    public class Forumvote
    {
        public Forumvote()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Forumvote");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumvote");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 某会员某论坛上次是否中奖
        /// </summary>
        public bool Exists(int ForumID, int BID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top 1 IsWin from tb_Forumvote");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and BID=@BID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" Order by id desc ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = BID;
            parameters[2].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在未开奖
        /// </summary>
        public bool ExistsKz()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(1) from tb_Forumvote");
            strSql.Append(" where state=0 ");

            return SqlHelper.Exists(strSql.ToString());
        }

        /// <summary>
        /// 某期是否存在未开奖
        /// </summary>
        public bool ExistsKz(int qiNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(1) from tb_Forumvote");
            strSql.Append(" where qiNum=@qiNum ");
            strSql.Append(" and state=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNum", SqlDbType.Int,4)};
            parameters[0].Value = qiNum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 某期是否已开奖
        /// </summary>
        public bool ExistsKz(int ForumID, int qiNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(1) from tb_Forumvote");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and qiNum=@qiNum ");
            strSql.Append(" and state=1 ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@qiNum", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = qiNum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某论坛本月某用户中奖数量
        /// </summary>
        public int GetMonthCount(int ForumID, int BID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Forumvote");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and BID=@BID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and IsWin=1 ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = BID;
            parameters[2].Value = UsID;

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
        public int Add(BCW.Model.Forumvote model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forumvote(");
            strSql.Append("Types,qiNum,ForumID,BID,UsID,UsName,Notes,AddTime,IsWin,state)");
            strSql.Append(" values (");
            strSql.Append("@Types,@qiNum,@ForumID,@BID,@UsID,@UsName,@Notes,@AddTime,@IsWin,@state)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@qiNum", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsWin", SqlDbType.TinyInt,1),
					new SqlParameter("@state", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.qiNum;
            parameters[2].Value = model.ForumID;
            parameters[3].Value = model.BID;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.IsWin;
            parameters[9].Value = model.state;

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
        public void UpdateNotes(int ID, string Notes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumvote set ");
            strSql.Append("Notes=@Notes");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200)};
            parameters[0].Value = ID;
            parameters[1].Value = Notes;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新是否中奖
        /// </summary>
        public void UpdateIsWin(int ID, int IsWin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumvote set ");
            strSql.Append("IsWin=@IsWin");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsWin", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = IsWin;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新某论坛本期全部已开奖
        /// </summary>
        public void UpdateState(int qiNum, int ForumID, int state,string sNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumvote set ");
            strSql.Append("state=@state,");
            strSql.Append(" AcTime=@AcTime,");
            strSql.Append(" sNum=@sNum ");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and qiNum=@qiNum ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@qiNum", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@AcTime", SqlDbType.DateTime),
					new SqlParameter("@sNum", SqlDbType.NVarChar,50)};
            parameters[0].Value = ForumID;
            parameters[1].Value = qiNum;
            parameters[2].Value = state;
            parameters[3].Value = DateTime.Now;
            parameters[4].Value = sNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Forumvote model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumvote set ");
            strSql.Append("Types=@Types,");
            strSql.Append("qiNum=@qiNum,");
            strSql.Append("ForumID=@ForumID,");
            strSql.Append("BID=@BID,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("IsWin=@IsWin,");
            strSql.Append("state=@state,");
            strSql.Append("AcTime=@AcTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@qiNum", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Notes", SqlDbType.NVarChar,200),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@IsWin", SqlDbType.TinyInt,1),
                    new SqlParameter("@state", SqlDbType.TinyInt,1),
                    new SqlParameter("@AcTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.qiNum;
            parameters[3].Value = model.ForumID;
            parameters[4].Value = model.BID;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.Notes;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.IsWin;
            parameters[10].Value = model.state;
            parameters[11].Value = model.AcTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumvote ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到某期开奖结果
        /// </summary>
        public string GetsNum(int ForumID, int qiNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top 1 sNum from tb_Forumvote");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and qiNum=@qiNum ");
            strSql.Append(" Order by id desc ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@qiNum", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = qiNum;

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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forumvote GetForumvote(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,qiNum,ForumID,BID,UsID,UsName,Notes,AddTime,IsWin,state,AcTime from tb_Forumvote ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Forumvote model = new BCW.Model.Forumvote();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.qiNum = reader.GetInt32(2);
                    model.ForumID = reader.GetInt32(3);
                    model.BID = reader.GetInt32(4);
                    model.UsID = reader.GetInt32(5);
                    model.UsName = reader.GetString(6);
                    model.Notes = reader.GetString(7);
                    model.AddTime = reader.GetDateTime(8);
                    model.IsWin = reader.GetByte(9);
                    model.state = reader.GetByte(10);
                    if (!reader.IsDBNull(11))
                        model.AcTime = reader.GetDateTime(11);
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
            strSql.Append(" FROM tb_Forumvote ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        /// <summary>
        /// 取得N条记录
        /// </summary>
        /// <param name="UsID">会员ID</param>
        /// <param name="BID">帖子ID</param>
        /// <param name="SizeNum">取N条</param>
        /// <returns></returns>
        public IList<BCW.Model.Forumvote> GetForumvotes(int UsID, int BID, int SizeNum)
        {
            IList<BCW.Model.Forumvote> listForumvotes = new List<BCW.Model.Forumvote>();

            string queryString = "SELECT Top " + SizeNum + " ID,qiNum,BID,Notes,IsWin,state,UsID FROM tb_Forumvote Where BID=" + BID + " and UsID=" + UsID + " ORDER BY ID DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                while (reader.Read())
                {
                    BCW.Model.Forumvote objForumvote = new BCW.Model.Forumvote();
                    objForumvote.ID = reader.GetInt32(0);
                    objForumvote.qiNum = reader.GetInt32(1);
                    objForumvote.BID = reader.GetInt32(2);
                    objForumvote.Notes = reader.GetString(3);
                    objForumvote.IsWin = reader.GetByte(4);
                    objForumvote.state = reader.GetByte(5);
                    objForumvote.UsID = reader.GetInt32(6);
                    listForumvotes.Add(objForumvote);
                }
            }

            return listForumvotes;
        }


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Forumvote</returns>
        public IList<BCW.Model.Forumvote> GetForumvotes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forumvote> listForumvotes = new List<BCW.Model.Forumvote>();
            string sTable = "tb_Forumvote";
            string sPkey = "id";
            string sField = "ID,Types,qiNum,ForumID,BID,UsID,UsName,Notes,AddTime,IsWin,state";
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
                    return listForumvotes;
                }
                while (reader.Read())
                {
                    BCW.Model.Forumvote objForumvote = new BCW.Model.Forumvote();
                    objForumvote.ID = reader.GetInt32(0);
                    objForumvote.Types = reader.GetInt32(1);
                    objForumvote.qiNum = reader.GetInt32(2);
                    objForumvote.ForumID = reader.GetInt32(3);
                    objForumvote.BID = reader.GetInt32(4);
                    objForumvote.UsID = reader.GetInt32(5);
                    objForumvote.UsName = reader.GetString(6);
                    objForumvote.Notes = reader.GetString(7);
                    objForumvote.AddTime = reader.GetDateTime(8);
                    objForumvote.IsWin = reader.GetByte(9);
                    objForumvote.state = reader.GetByte(10);
                    listForumvotes.Add(objForumvote);
                }
            }
            return listForumvotes;
        }

        #endregion  成员方法
    }
}

