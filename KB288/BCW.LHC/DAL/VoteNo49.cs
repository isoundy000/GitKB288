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
    /// 数据访问类VoteNo49。
    /// </summary>
    public class VoteNo49
    {
        public VoteNo49()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_VoteNo49");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_VoteNo49");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LHC.Model.VoteNo49 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_VoteNo49(");
            strSql.Append("qiNo,ExTime,payCent,payCount,sNum,pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,State,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@qiNo,@ExTime,@payCent,@payCount,@sNum,@pNum1,@pNum2,@pNum3,@pNum4,@pNum5,@pNum6,@State,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@payCount", SqlDbType.Int,4),
					new SqlParameter("@sNum", SqlDbType.Int,4),
					new SqlParameter("@pNum1", SqlDbType.Int,4),
					new SqlParameter("@pNum2", SqlDbType.Int,4),
					new SqlParameter("@pNum3", SqlDbType.Int,4),
					new SqlParameter("@pNum4", SqlDbType.Int,4),
					new SqlParameter("@pNum5", SqlDbType.Int,4),
					new SqlParameter("@pNum6", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.qiNo;
            parameters[1].Value = model.ExTime;
            parameters[2].Value = model.payCent;
            parameters[3].Value = model.payCount;
            parameters[4].Value = model.sNum;
            parameters[5].Value = model.pNum1;
            parameters[6].Value = model.pNum2;
            parameters[7].Value = model.pNum3;
            parameters[8].Value = model.pNum4;
            parameters[9].Value = model.pNum5;
            parameters[10].Value = model.pNum6;
            parameters[11].Value = model.State;
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
        public void Update(LHC.Model.VoteNo49 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VoteNo49 set ");
            strSql.Append("ExTime=@ExTime,");
            strSql.Append("payCent=@payCent,");
            strSql.Append("payCount=@payCount,");
            strSql.Append("payCent2=@payCent2,");
            strSql.Append("payCount2=@payCount2,");
            strSql.Append("sNum=@sNum,");
            strSql.Append("pNum1=@pNum1,");
            strSql.Append("pNum2=@pNum2,");
            strSql.Append("pNum3=@pNum3,");
            strSql.Append("pNum4=@pNum4,");
            strSql.Append("pNum5=@pNum5,");
            strSql.Append("pNum6=@pNum6,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@payCount", SqlDbType.Int,4),
					new SqlParameter("@payCent2", SqlDbType.BigInt,8),
					new SqlParameter("@payCount2", SqlDbType.Int,4),
					new SqlParameter("@sNum", SqlDbType.Int,4),
					new SqlParameter("@pNum1", SqlDbType.Int,4),
					new SqlParameter("@pNum2", SqlDbType.Int,4),
					new SqlParameter("@pNum3", SqlDbType.Int,4),
					new SqlParameter("@pNum4", SqlDbType.Int,4),
					new SqlParameter("@pNum5", SqlDbType.Int,4),
					new SqlParameter("@pNum6", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ExTime;
            parameters[2].Value = model.payCent;
            parameters[3].Value = model.payCount;
            parameters[4].Value = model.payCent2;
            parameters[5].Value = model.payCount2;
            parameters[6].Value = model.sNum;
            parameters[7].Value = model.pNum1;
            parameters[8].Value = model.pNum2;
            parameters[9].Value = model.pNum3;
            parameters[10].Value = model.pNum4;
            parameters[11].Value = model.pNum5;
            parameters[12].Value = model.pNum6;
            parameters[13].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOpen(LHC.Model.VoteNo49 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VoteNo49 set ");
            strSql.Append("sNum=@sNum,");
            strSql.Append("pNum1=@pNum1,");
            strSql.Append("pNum2=@pNum2,");
            strSql.Append("pNum3=@pNum3,");
            strSql.Append("pNum4=@pNum4,");
            strSql.Append("pNum5=@pNum5,");
            strSql.Append("pNum6=@pNum6");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@sNum", SqlDbType.Int,4),
					new SqlParameter("@pNum1", SqlDbType.Int,4),
					new SqlParameter("@pNum2", SqlDbType.Int,4),
					new SqlParameter("@pNum3", SqlDbType.Int,4),
					new SqlParameter("@pNum4", SqlDbType.Int,4),
					new SqlParameter("@pNum5", SqlDbType.Int,4),
					new SqlParameter("@pNum6", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.sNum;
            parameters[2].Value = model.pNum1;
            parameters[3].Value = model.pNum2;
            parameters[4].Value = model.pNum3;
            parameters[5].Value = model.pNum4;
            parameters[6].Value = model.pNum5;
            parameters[7].Value = model.pNum6;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int qiNo, long payCent, int payCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VoteNo49 set ");
            strSql.Append("payCent=payCent+@payCent,");
            strSql.Append("payCount=payCount+@payCount");
            strSql.Append(" where qiNo=@qiNo ");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@payCount", SqlDbType.Int,4)};
            parameters[0].Value = qiNo;
            parameters[1].Value = payCent;
            parameters[2].Value = payCount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(int qiNo, long payCent2, int payCount2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VoteNo49 set ");
            strSql.Append("payCent2=payCent2+@payCent2,");
            strSql.Append("payCount2=payCount2+@payCount2");
            strSql.Append(" where qiNo=@qiNo ");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@payCent2", SqlDbType.BigInt,8),
					new SqlParameter("@payCount2", SqlDbType.Int,4)};
            parameters[0].Value = qiNo;
            parameters[1].Value = payCent2;
            parameters[2].Value = payCount2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新该期为结束
        /// </summary>
        public void UpdateState(int qiNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VoteNo49 set ");
            strSql.Append("State=@State");
            strSql.Append(" where qiNo=@qiNo ");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = qiNo;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_VoteNo49 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个qiNo
        /// </summary>
        public int GetqiNo(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 qiNo from tb_VoteNo49 ");
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
        /// 得到一个payCount
        /// </summary>
        public int GetpayCount(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 payCount from tb_VoteNo49 ");
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
        /// 得到一个对象实体
        /// </summary>
        public LHC.Model.VoteNo49 GetVoteNo49(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,qiNo,ExTime,payCent,payCount,payCent2,payCount2,sNum,pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,State,AddTime from tb_VoteNo49 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            LHC.Model.VoteNo49 model = new LHC.Model.VoteNo49();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.qiNo = reader.GetInt32(1);
                    model.ExTime = reader.GetDateTime(2);
                    model.payCent = reader.GetInt64(3);
                    model.payCount = reader.GetInt32(4);
                    model.payCent2 = reader.GetInt64(5);
                    model.payCount2 = reader.GetInt32(6);
                    model.sNum = reader.GetInt32(7);
                    model.pNum1 = reader.GetInt32(8);
                    model.pNum2 = reader.GetInt32(9);
                    model.pNum3 = reader.GetInt32(10);
                    model.pNum4 = reader.GetInt32(11);
                    model.pNum5 = reader.GetInt32(12);
                    model.pNum6 = reader.GetInt32(13);
                    model.State = reader.GetByte(14);
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
        /// 得到一个最新实体
        /// </summary>
        public LHC.Model.VoteNo49 GetVoteNo49New(int State)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,qiNo,ExTime,payCent,payCount,payCent2,payCount2,sNum,pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,State,AddTime from tb_VoteNo49 ");
            strSql.Append(" Where State=@State Order BY ID DESC");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = State;

            LHC.Model.VoteNo49 model = new LHC.Model.VoteNo49();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.qiNo = reader.GetInt32(1);
                    model.ExTime = reader.GetDateTime(2);
                    model.payCent = reader.GetInt64(3);
                    model.payCount = reader.GetInt32(4);
                    model.payCent2 = reader.GetInt64(5);
                    model.payCount2 = reader.GetInt32(6);
                    model.sNum = reader.GetInt32(7);
                    model.pNum1 = reader.GetInt32(8);
                    model.pNum2 = reader.GetInt32(9);
                    model.pNum3 = reader.GetInt32(10);
                    model.pNum4 = reader.GetInt32(11);
                    model.pNum5 = reader.GetInt32(12);
                    model.pNum6 = reader.GetInt32(13);
                    model.State = reader.GetByte(14);
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
            strSql.Append(" FROM tb_VoteNo49 ");
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
        /// <returns>IList VoteNo49</returns>
        public IList<LHC.Model.VoteNo49> GetVoteNo49s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<LHC.Model.VoteNo49> listVoteNo49s = new List<LHC.Model.VoteNo49>();
            string sTable = "tb_VoteNo49";
            string sPkey = "id";
            string sField = "ID,qiNo,ExTime,payCent,payCount,sNum,pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,State,AddTime";
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
                    return listVoteNo49s;
                }
                while (reader.Read())
                {
                    LHC.Model.VoteNo49 objVoteNo49 = new LHC.Model.VoteNo49();
                    objVoteNo49.ID = reader.GetInt32(0);
                    objVoteNo49.qiNo = reader.GetInt32(1);
                    objVoteNo49.ExTime = reader.GetDateTime(2);
                    objVoteNo49.payCent = reader.GetInt64(3);
                    objVoteNo49.payCount = reader.GetInt32(4);
                    objVoteNo49.sNum = reader.GetInt32(5);
                    objVoteNo49.pNum1 = reader.GetInt32(6);
                    objVoteNo49.pNum2 = reader.GetInt32(7);
                    objVoteNo49.pNum3 = reader.GetInt32(8);
                    objVoteNo49.pNum4 = reader.GetInt32(9);
                    objVoteNo49.pNum5 = reader.GetInt32(10);
                    objVoteNo49.pNum6 = reader.GetInt32(11);
                    objVoteNo49.State = reader.GetByte(12);
                    objVoteNo49.AddTime = reader.GetDateTime(13);
                    listVoteNo49s.Add(objVoteNo49);
                }
            }
            return listVoteNo49s;
        }

        #endregion  成员方法
    }
}
