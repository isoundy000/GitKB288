using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.JQC.DAL
{
    /// <summary>
    /// 数据访问类JQC_Bet。
    /// </summary>
    public class JQC_Bet
    {
        public JQC_Bet()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_JQC_Bet");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Bet");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Bet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_JQC_Bet(");
            strSql.Append("UsID,Lottery_issue,VoteNum,Zhu,Zhu_money,PutGold,GetMoney,State,VoteRate,Input_Time,isRobot)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@Lottery_issue,@VoteNum,@Zhu,@Zhu_money,@PutGold,@GetMoney,@State,@VoteRate,@Input_Time,@isRobot)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4),
                    new SqlParameter("@VoteNum", SqlDbType.VarChar,200),
                    new SqlParameter("@Zhu", SqlDbType.Int,4),
                    new SqlParameter("@Zhu_money", SqlDbType.Int,4),
                    new SqlParameter("@PutGold", SqlDbType.BigInt,8),
                    new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@VoteRate", SqlDbType.Int,4),
                    new SqlParameter("@Input_Time", SqlDbType.DateTime),
                    new SqlParameter("@isRobot", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.Lottery_issue;
            parameters[2].Value = model.VoteNum;
            parameters[3].Value = model.Zhu;
            parameters[4].Value = model.Zhu_money;
            parameters[5].Value = model.PutGold;
            parameters[6].Value = model.GetMoney;
            parameters[7].Value = model.State;
            parameters[8].Value = model.VoteRate;
            parameters[9].Value = model.Input_Time;
            parameters[10].Value = model.isRobot;

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
        public void Update(BCW.JQC.Model.JQC_Bet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_JQC_Bet set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("Lottery_issue=@Lottery_issue,");
            strSql.Append("VoteNum=@VoteNum,");
            strSql.Append("Zhu=@Zhu,");
            strSql.Append("Zhu_money=@Zhu_money,");
            strSql.Append("PutGold=@PutGold,");
            strSql.Append("GetMoney=@GetMoney,");
            strSql.Append("State=@State,");
            strSql.Append("VoteRate=@VoteRate,");
            strSql.Append("Input_Time=@Input_Time,");
            strSql.Append("isRobot=@isRobot");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4),
                    new SqlParameter("@VoteNum", SqlDbType.VarChar,200),
                    new SqlParameter("@Zhu", SqlDbType.Int,4),
                    new SqlParameter("@Zhu_money", SqlDbType.Int,4),
                    new SqlParameter("@PutGold", SqlDbType.BigInt,8),
                    new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@VoteRate", SqlDbType.Int,4),
                    new SqlParameter("@Input_Time", SqlDbType.DateTime),
                    new SqlParameter("@isRobot", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.Lottery_issue;
            parameters[3].Value = model.VoteNum;
            parameters[4].Value = model.Zhu;
            parameters[5].Value = model.Zhu_money;
            parameters[6].Value = model.PutGold;
            parameters[7].Value = model.GetMoney;
            parameters[8].Value = model.State;
            parameters[9].Value = model.VoteRate;
            parameters[10].Value = model.Input_Time;
            parameters[11].Value = model.isRobot;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_JQC_Bet ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Bet GetJQC_Bet(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_JQC_Bet ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.JQC.Model.JQC_Bet model = new BCW.JQC.Model.JQC_Bet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Lottery_issue = reader.GetInt32(2);
                    model.VoteNum = reader.GetString(3);
                    model.Zhu = reader.GetInt32(4);
                    model.Zhu_money = reader.GetInt32(5);
                    model.PutGold = reader.GetInt64(6);
                    model.GetMoney = reader.GetInt64(7);
                    model.State = reader.GetInt32(8);
                    model.VoteRate = reader.GetInt32(9);
                    model.Input_Time = reader.GetDateTime(10);
                    model.isRobot = reader.GetInt32(11);
                    model.Prize1 = reader.GetInt32(12);
                    model.Prize2 = reader.GetInt32(13);
                    model.Prize3 = reader.GetInt32(14);
                    model.Prize4 = reader.GetInt32(15);
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
            strSql.Append(" FROM tb_JQC_Bet ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //==
        /// <summary>
        ///  me_后台分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + s1 + " from tb_JQC_Bet " + s2 + " ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.bb desc");
            strSql.Append(")AS Row, T.*  from #bang3 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang3");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Bet GetNC_suiji()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 * FROM (select top 15  * from  tb_JQC_Bet WHERE GetMoney>0 ORDER BY ID DESC) AS b order by newid()");

            BCW.JQC.Model.JQC_Bet model = new BCW.JQC.Model.JQC_Bet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Lottery_issue = reader.GetInt32(2);
                    model.VoteNum = reader.GetString(3);
                    model.Zhu = reader.GetInt32(4);
                    model.Zhu_money = reader.GetInt32(5);
                    model.PutGold = reader.GetInt64(6);
                    model.GetMoney = reader.GetInt64(7);
                    model.State = reader.GetInt32(8);
                    model.VoteRate = reader.GetInt32(9);
                    model.Input_Time = reader.GetDateTime(10);
                    model.isRobot = reader.GetInt32(11);
                    model.Prize1 = reader.GetInt32(12);
                    model.Prize2 = reader.GetInt32(13);
                    model.Prize3 = reader.GetInt32(14);
                    model.Prize4 = reader.GetInt32(15);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_是否存在开奖后没有返奖的
        /// </summary>
        public bool Exists_num(int Lottery_issue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Bet");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            strSql.Append(" and State=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4)};
            parameters[0].Value = Lottery_issue;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Bet");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and State=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_得到该期总中奖人数
        /// </summary>
        public int count_zhu(int Lottery_issue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) FROM tb_JQC_Bet WHERE GetMoney>0 AND Lottery_issue='" + Lottery_issue + "'");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4)};
            parameters[0].Value = Lottery_issue;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// me_得到该期某注的中奖人数
        /// </summary>
        public int count_renshu(int Lottery_issue, string Prize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(" + Prize + "*VoteRate) FROM tb_JQC_Bet WHERE Lottery_issue=" + Lottery_issue + " AND " + Prize + ">0");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4)};
            parameters[0].Value = Lottery_issue;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// me_更新中奖状态
        /// </summary>
        public void Update_win(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_JQC_Bet set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4)};

            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_GetMoney(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_JQC_Bet SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        //me_查询机器人购买次数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_JQC_Bet ");
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
        /// me_根据所点的历史记录，通过开奖期号查询相应的投注情况
        /// </summary>
        public BCW.JQC.Model.JQC_Bet Get_tounum(int Lottery_issue)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as a from tb_JQC_Bet ");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4)};
            parameters[0].Value = Lottery_issue;

            BCW.JQC.Model.JQC_Bet model = new BCW.JQC.Model.JQC_Bet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.a = reader.GetInt32(0);
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
		public BCW.JQC.Model.JQC_Bet Get_qihao(int Lottery_issue)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_JQC_Bet ");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4)};
            parameters[0].Value = Lottery_issue;

            BCW.JQC.Model.JQC_Bet model = new BCW.JQC.Model.JQC_Bet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Lottery_issue = reader.GetInt32(2);
                    model.VoteNum = reader.GetString(3);
                    model.Zhu = reader.GetInt32(4);
                    model.Zhu_money = reader.GetInt32(5);
                    model.PutGold = reader.GetInt64(6);
                    model.GetMoney = reader.GetInt64(7);
                    model.State = reader.GetInt32(8);
                    model.VoteRate = reader.GetInt32(9);
                    model.Input_Time = reader.GetDateTime(10);
                    model.isRobot = reader.GetInt32(11);
                    model.Prize1 = reader.GetInt32(12);
                    model.Prize2 = reader.GetInt32(13);
                    model.Prize3 = reader.GetInt32(14);
                    model.Prize4 = reader.GetInt32(15);
                    return model;
                }
                else
                {
                    model.Lottery_issue = 0;
                    return model;
                }
            }
        }
        /// <summary>
        //me_查询机器人购买次数
        /// </summary>
        public long Get_paijiang(int Lottery_issue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(GetMoney) FROM tb_JQC_Bet WHERE Lottery_issue=@Lottery_issue");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.Int,4)};
            parameters[0].Value = Lottery_issue;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_JQC_Bet");
            strSql.Append(" where " + strWhere + " ");
            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        //==



        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList JQC_Bet</returns>
        public IList<BCW.JQC.Model.JQC_Bet> GetJQC_Bets(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.JQC.Model.JQC_Bet> listJQC_Bets = new List<BCW.JQC.Model.JQC_Bet>();
            string sTable = "tb_JQC_Bet";
            string sPkey = "id";
            string sField = "*";
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
                    return listJQC_Bets;
                }
                while (reader.Read())
                {
                    BCW.JQC.Model.JQC_Bet objJQC_Bet = new BCW.JQC.Model.JQC_Bet();
                    objJQC_Bet.ID = reader.GetInt32(0);
                    objJQC_Bet.UsID = reader.GetInt32(1);
                    objJQC_Bet.Lottery_issue = reader.GetInt32(2);
                    objJQC_Bet.VoteNum = reader.GetString(3);
                    objJQC_Bet.Zhu = reader.GetInt32(4);
                    objJQC_Bet.Zhu_money = reader.GetInt32(5);
                    objJQC_Bet.PutGold = reader.GetInt64(6);
                    objJQC_Bet.GetMoney = reader.GetInt64(7);
                    objJQC_Bet.State = reader.GetInt32(8);
                    objJQC_Bet.VoteRate = reader.GetInt32(9);
                    objJQC_Bet.Input_Time = reader.GetDateTime(10);
                    objJQC_Bet.isRobot = reader.GetInt32(11);
                    objJQC_Bet.Prize1 = reader.GetInt32(12);
                    objJQC_Bet.Prize2 = reader.GetInt32(13);
                    objJQC_Bet.Prize3 = reader.GetInt32(14);
                    objJQC_Bet.Prize4 = reader.GetInt32(15);
                    listJQC_Bets.Add(objJQC_Bet);
                }
            }
            return listJQC_Bets;
        }

        #endregion  成员方法
    }
}

