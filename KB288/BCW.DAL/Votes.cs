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
    /// 数据访问类Votes。
    /// </summary>
    public class Votes
    {
        public Votes()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Votes");
        }

        /// <summary>
        /// 得到最大ID(系统发布)
        /// </summary>
        public int GetLastId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID from tb_Votes ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" Order By ID Desc ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = 0;

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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Votes");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Votes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Votes(");
            strSql.Append("Types,UsID,Title,Content,Vote,AddVote,VoteType,VoteLeven,VoteTiple,Readcount,Status,VoteExTime,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@Title,@Content,@Vote,@AddVote,@VoteType,@VoteLeven,@VoteTiple,@Readcount,@Status,@VoteExTime,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Vote", SqlDbType.NText),
                    new SqlParameter("@AddVote", SqlDbType.NText),
					new SqlParameter("@VoteType", SqlDbType.TinyInt,1),
					new SqlParameter("@VoteLeven", SqlDbType.Int,4),
					new SqlParameter("@VoteTiple", SqlDbType.TinyInt,1),
					new SqlParameter("@Readcount", SqlDbType.Int,4),
                    new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@VoteExTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.Vote;
            parameters[5].Value = model.AddVote;
            parameters[6].Value = model.VoteType;
            parameters[7].Value = model.VoteLeven;
            parameters[8].Value = model.VoteTiple;
            parameters[9].Value = model.Readcount;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.VoteExTime;
            parameters[12].Value = model.AddTime;

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
        public void Update(BCW.Model.Votes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Votes set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("Vote=@Vote,");
            strSql.Append("VoteType=@VoteType,");
            strSql.Append("VoteLeven=@VoteLeven,");
            strSql.Append("VoteTiple=@VoteTiple,");
            strSql.Append("Readcount=@Readcount,");
            strSql.Append("Status=@Status,");
            strSql.Append("VoteExTime=@VoteExTime,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Vote", SqlDbType.NText),
					new SqlParameter("@VoteType", SqlDbType.TinyInt,1),
					new SqlParameter("@VoteLeven", SqlDbType.Int,4),
					new SqlParameter("@VoteTiple", SqlDbType.TinyInt,1),
					new SqlParameter("@Readcount", SqlDbType.Int,4),
				    new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@VoteExTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.Vote;
            parameters[5].Value = model.VoteType;
            parameters[6].Value = model.VoteLeven;
            parameters[7].Value = model.VoteTiple;
            parameters[8].Value = model.Readcount;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.VoteExTime;
            parameters[11].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新投票
        /// </summary>
        public void UpdateVote(BCW.Model.Votes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Votes set ");
            strSql.Append("AddVote=@AddVote,");
            strSql.Append("VoteID=@VoteID");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@AddVote", SqlDbType.NText),
					new SqlParameter("@VoteID", SqlDbType.NText)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.AddVote;
            parameters[2].Value = model.VoteID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Votes ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Votes GetBbsVotes(int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Vote,AddVote,VoteID,VoteType,VoteLeven,VoteTiple,VoteExTime from tb_Votes ");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

            BCW.Model.Votes model = new BCW.Model.Votes();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        model.Vote = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        model.AddVote = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.VoteID = reader.GetString(3);
                    model.VoteType = reader.GetByte(4);
                    model.VoteLeven = reader.GetInt32(5);
                    model.VoteTiple = reader.GetByte(6);
                    model.VoteExTime = reader.GetDateTime(7);
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
        public BCW.Model.Votes GetVotes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,Title,Content,Vote,AddVote,VoteID,VoteType,VoteLeven,VoteTiple,ReStats,Readcount,Status,VoteExTime,AddTime from tb_Votes ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Votes model = new BCW.Model.Votes();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    if (!reader.IsDBNull(3))
                        model.Title = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.Content = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        model.Vote = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        model.AddVote = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        model.VoteID = reader.GetString(7);
                    model.VoteType = reader.GetByte(8);
                    model.VoteLeven = reader.GetInt32(9);
                    model.VoteTiple = reader.GetByte(10);
                    if (!reader.IsDBNull(11))
                        model.ReStats = reader.GetString(11);
                    model.Readcount = reader.GetInt32(12);
                    model.Status = reader.GetByte(13);
                    model.VoteExTime = reader.GetDateTime(14);
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
            strSql.Append(" FROM tb_Votes ");
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
        /// <returns>IList Votes</returns>
        public IList<BCW.Model.Votes> GetVotess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Votes> listVotess = new List<BCW.Model.Votes>();
            string sTable = "tb_Votes";
            string sPkey = "id";
            string sField = "ID,UsID,Title,VoteTiple,AddTime";
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
                    return listVotess;
                }
                while (reader.Read())
                {
                    BCW.Model.Votes objVotes = new BCW.Model.Votes();
                    objVotes.ID = reader.GetInt32(0);
                    objVotes.UsID = reader.GetInt32(1);
                    objVotes.Title = reader.GetString(2);
                    objVotes.VoteTiple = reader.GetByte(3);
                    objVotes.AddTime = reader.GetDateTime(4);
                    listVotess.Add(objVotes);
                }
            }
            return listVotess;
        }

        #endregion  成员方法
    }
}

