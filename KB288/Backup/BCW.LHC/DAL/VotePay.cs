using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace LHC.DAL
{
    /// <summary>
    /// 数据访问类VotePay。
    /// </summary>
    public class VotePay
    {
        public VotePay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_VotePay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_VotePay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_VotePay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and winCent>@winCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@winCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 每ID每期下注币数
        /// </summary>
        public long GetPayCent(int UsID, int qiNo, int bzType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_VotePay");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and qiNo=@qiNo ");
            strSql.Append(" and BzType=@BzType ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = qiNo;
            parameters[2].Value = bzType;
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
        /// 每期中奖币数
        /// </summary>
        public long GetwinCent(int qiNo, int bzType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(winCent) from tb_VotePay");
            strSql.Append(" where winCent>@winCent ");
            strSql.Append(" and qiNo=@qiNo ");
            strSql.Append(" and BzType=@BzType ");
            SqlParameter[] parameters = {
					new SqlParameter("@winCent", SqlDbType.BigInt,8),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = 0;
            parameters[1].Value = qiNo;
            parameters[2].Value = bzType;
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
        /// 每特.码每期下注币数
        /// </summary>
        public long GetPayCent(int qiNo, int sNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_VotePay");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and qiNo=@qiNo ");
            strSql.Append(" and BzType=@BzType  and ','+Vote+',' Like '%," + sNum + ",%'");

            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = 1;
            parameters[1].Value = qiNo;
            parameters[2].Value = 0;
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
        /// 根据条件得到下注币数
        /// </summary>
        public long GetPayCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_VotePay");
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
        /// 根据条件得到赢利币数
        /// </summary>
        public long GetwinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(winCent) from tb_VotePay");
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
        /// 增加一条数据
        /// </summary>
        public int Add(LHC.Model.VotePay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_VotePay(");
            strSql.Append("Types,qiNo,UsID,UsName,Vote,payCent,winCent,State,AddTime,BzType)");
            strSql.Append(" values (");
            strSql.Append("@Types,@qiNo,@UsID,@UsName,@Vote,@payCent,@winCent,@State,@AddTime,@BzType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Vote", SqlDbType.NVarChar,200),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@winCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.qiNo;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Vote;
            parameters[5].Value = model.payCent;
            parameters[6].Value = model.winCent;
            parameters[7].Value = model.State;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.BzType;

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
        public void Update(LHC.Model.VotePay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VotePay set ");
            strSql.Append("Types=@Types,");
            strSql.Append("qiNo=@qiNo,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Vote=@Vote,");
            strSql.Append("payCent=@payCent,");
            strSql.Append("winCent=@winCent,");
            strSql.Append("State=@State,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("BzType=@BzType");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Vote", SqlDbType.NVarChar,200),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@winCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.qiNo;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Vote;
            parameters[6].Value = model.payCent;
            parameters[7].Value = model.winCent;
            parameters[8].Value = model.State;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.BzType;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新该期为结束
        /// </summary>
        public void UpdateOver(int qiNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VotePay set ");
            strSql.Append("State=@State");
            strSql.Append(" where qiNo=@qiNo and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = qiNo;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VotePay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_VotePay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_VotePay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LHC.Model.VotePay GetVotePay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,qiNo,UsID,UsName,Vote,payCent,winCent,State,AddTime,BzType from tb_VotePay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            LHC.Model.VotePay model = new LHC.Model.VotePay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.qiNo = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Vote = reader.GetString(5);
                    model.payCent = reader.GetInt64(6);
                    model.winCent = reader.GetInt64(7);
                    model.State = reader.GetInt32(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.BzType = reader.GetByte(10);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 winCent from tb_VotePay ");
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
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 得到一个BzType
        /// </summary>
        public int GetBzType(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BzType from tb_VotePay ");
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_VotePay ");
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
        /// <returns>IList VotePay</returns>
        public IList<LHC.Model.VotePay> GetVotePays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<LHC.Model.VotePay> listVotePays = new List<LHC.Model.VotePay>();
            string sTable = "tb_VotePay";
            string sPkey = "id";
            string sField = "ID,Types,qiNo,UsID,UsName,Vote,payCent,winCent,State,AddTime,BzType";
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
                    return listVotePays;
                }
                while (reader.Read())
                {
                    LHC.Model.VotePay objVotePay = new LHC.Model.VotePay();
                    objVotePay.ID = reader.GetInt32(0);
                    objVotePay.Types = reader.GetInt32(1);
                    objVotePay.qiNo = reader.GetInt32(2);
                    objVotePay.UsID = reader.GetInt32(3);
                    objVotePay.UsName = reader.GetString(4);
                    objVotePay.Vote = reader.GetString(5);
                    objVotePay.payCent = reader.GetInt64(6);
                    objVotePay.winCent = reader.GetInt64(7);
                    objVotePay.State = reader.GetByte(8);
                    objVotePay.AddTime = reader.GetDateTime(9);
                    objVotePay.BzType = reader.GetByte(10);
                    listVotePays.Add(objVotePay);
                }
            }
            return listVotePays;
        }

        #endregion  成员方法
    }
}

