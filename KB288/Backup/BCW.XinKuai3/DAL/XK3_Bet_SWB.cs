using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.XinKuai3.DAL
{
	/// <summary>
	/// 数据访问类XK3_Bet_SWB。
	/// </summary>
	public class XK3_Bet_SWB
	{
		public XK3_Bet_SWB()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_XK3_Bet_SWB"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_XK3_Bet_SWB");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.XinKuai3.Model.XK3_Bet_SWB model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_XK3_Bet_SWB(");
			strSql.Append("UsID,Play_Way,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,Input_Time,DanTuo,Zhu,GetMoney,PutGold,State,Lottery_issue,Zhu_money,DaXiao,DanShuang,Odds)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@Play_Way,@Sum,@Three_Same_All,@Three_Same_Single,@Three_Same_Not,@Three_Continue_All,@Two_Same_All,@Two_Same_Single,@Two_dissame,@Input_Time,@DanTuo,@Zhu,@GetMoney,@PutGold,@State,@Lottery_issue,@Zhu_money,@DaXiao,@DanShuang,@Odds)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Play_Way", SqlDbType.Int,4),
					new SqlParameter("@Sum", SqlDbType.VarChar,60),
					new SqlParameter("@Three_Same_All", SqlDbType.VarChar,30),
					new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,30),
					new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,100),
					new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,30),
					new SqlParameter("@Two_Same_All", SqlDbType.VarChar,30),
					new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,100),
					new SqlParameter("@Two_dissame", SqlDbType.VarChar,100),
					new SqlParameter("@Input_Time", SqlDbType.DateTime),
					new SqlParameter("@DanTuo", SqlDbType.VarChar,20),
					new SqlParameter("@Zhu", SqlDbType.Int,4),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@PutGold", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
					new SqlParameter("@Zhu_money", SqlDbType.BigInt,8),
					new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
					new SqlParameter("@DanShuang", SqlDbType.VarChar,10),
					new SqlParameter("@Odds", SqlDbType.Float,8)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.Play_Way;
			parameters[2].Value = model.Sum;
			parameters[3].Value = model.Three_Same_All;
			parameters[4].Value = model.Three_Same_Single;
			parameters[5].Value = model.Three_Same_Not;
			parameters[6].Value = model.Three_Continue_All;
			parameters[7].Value = model.Two_Same_All;
			parameters[8].Value = model.Two_Same_Single;
			parameters[9].Value = model.Two_dissame;
			parameters[10].Value = model.Input_Time;
			parameters[11].Value = model.DanTuo;
			parameters[12].Value = model.Zhu;
			parameters[13].Value = model.GetMoney;
			parameters[14].Value = model.PutGold;
			parameters[15].Value = model.State;
			parameters[16].Value = model.Lottery_issue;
			parameters[17].Value = model.Zhu_money;
			parameters[18].Value = model.DaXiao;
			parameters[19].Value = model.DanShuang;
			parameters[20].Value = model.Odds;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public void Update(BCW.XinKuai3.Model.XK3_Bet_SWB model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_XK3_Bet_SWB set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Play_Way=@Play_Way,");
			strSql.Append("Sum=@Sum,");
			strSql.Append("Three_Same_All=@Three_Same_All,");
			strSql.Append("Three_Same_Single=@Three_Same_Single,");
			strSql.Append("Three_Same_Not=@Three_Same_Not,");
			strSql.Append("Three_Continue_All=@Three_Continue_All,");
			strSql.Append("Two_Same_All=@Two_Same_All,");
			strSql.Append("Two_Same_Single=@Two_Same_Single,");
			strSql.Append("Two_dissame=@Two_dissame,");
			strSql.Append("Input_Time=@Input_Time,");
			strSql.Append("DanTuo=@DanTuo,");
			strSql.Append("Zhu=@Zhu,");
			strSql.Append("GetMoney=@GetMoney,");
			strSql.Append("PutGold=@PutGold,");
			strSql.Append("State=@State,");
			strSql.Append("Lottery_issue=@Lottery_issue,");
			strSql.Append("Zhu_money=@Zhu_money,");
			strSql.Append("DaXiao=@DaXiao,");
			strSql.Append("DanShuang=@DanShuang,");
			strSql.Append("Odds=@Odds");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Play_Way", SqlDbType.Int,4),
					new SqlParameter("@Sum", SqlDbType.VarChar,60),
					new SqlParameter("@Three_Same_All", SqlDbType.VarChar,30),
					new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,30),
					new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,100),
					new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,30),
					new SqlParameter("@Two_Same_All", SqlDbType.VarChar,30),
					new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,100),
					new SqlParameter("@Two_dissame", SqlDbType.VarChar,100),
					new SqlParameter("@Input_Time", SqlDbType.DateTime),
					new SqlParameter("@DanTuo", SqlDbType.VarChar,20),
					new SqlParameter("@Zhu", SqlDbType.Int,4),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@PutGold", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
					new SqlParameter("@Zhu_money", SqlDbType.BigInt,8),
					new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
					new SqlParameter("@DanShuang", SqlDbType.VarChar,10),
					new SqlParameter("@Odds", SqlDbType.Float,8)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.Play_Way;
			parameters[3].Value = model.Sum;
			parameters[4].Value = model.Three_Same_All;
			parameters[5].Value = model.Three_Same_Single;
			parameters[6].Value = model.Three_Same_Not;
			parameters[7].Value = model.Three_Continue_All;
			parameters[8].Value = model.Two_Same_All;
			parameters[9].Value = model.Two_Same_Single;
			parameters[10].Value = model.Two_dissame;
			parameters[11].Value = model.Input_Time;
			parameters[12].Value = model.DanTuo;
			parameters[13].Value = model.Zhu;
			parameters[14].Value = model.GetMoney;
			parameters[15].Value = model.PutGold;
			parameters[16].Value = model.State;
			parameters[17].Value = model.Lottery_issue;
			parameters[18].Value = model.Zhu_money;
			parameters[19].Value = model.DaXiao;
			parameters[20].Value = model.DanShuang;
			parameters[21].Value = model.Odds;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_XK3_Bet_SWB ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// me_得到一个对象实体
		/// </summary>
		public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,Play_Way,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,Input_Time,DanTuo,Zhu,GetMoney,PutGold,State,Lottery_issue,Zhu_money,DaXiao,DanShuang from tb_XK3_Bet_SWB ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.XinKuai3.Model.XK3_Bet_SWB model=new BCW.XinKuai3.Model.XK3_Bet_SWB();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.Play_Way = reader.GetInt32(2);
					model.Sum = reader.GetString(3);
					model.Three_Same_All = reader.GetString(4);
					model.Three_Same_Single = reader.GetString(5);
					model.Three_Same_Not = reader.GetString(6);
					model.Three_Continue_All = reader.GetString(7);
					model.Two_Same_All = reader.GetString(8);
					model.Two_Same_Single = reader.GetString(9);
					model.Two_dissame = reader.GetString(10);
					model.Input_Time = reader.GetDateTime(11);
					model.DanTuo = reader.GetString(12);
					model.Zhu = reader.GetInt32(13);
					model.GetMoney = reader.GetInt64(14);
					model.PutGold = reader.GetInt64(15);
					model.State = reader.GetInt32(16);
					model.Lottery_issue = reader.GetString(17);
					model.Zhu_money = reader.GetInt64(18);
					model.DaXiao = reader.GetString(19);
					model.DanShuang = reader.GetString(20);
					//model.Odds = reader.GetDecimal(21);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_XK3_Bet_SWB ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}


        //==========================================
        /// <summary>
        /// me_机器人增加一条数据
        /// </summary>
        public int Add_Robot(BCW.XinKuai3.Model.XK3_Bet_SWB model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_XK3_Bet_SWB(");
            strSql.Append("UsID,Play_Way,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,Input_Time,DanTuo,Zhu,GetMoney,PutGold,State,Lottery_issue,Zhu_money,DaXiao,DanShuang,Odds,isRobot)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@Play_Way,@Sum,@Three_Same_All,@Three_Same_Single,@Three_Same_Not,@Three_Continue_All,@Two_Same_All,@Two_Same_Single,@Two_dissame,@Input_Time,@DanTuo,@Zhu,@GetMoney,@PutGold,@State,@Lottery_issue,@Zhu_money,@DaXiao,@DanShuang,@Odds,@isRobot)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Play_Way", SqlDbType.Int,4),
					new SqlParameter("@Sum", SqlDbType.VarChar,60),
					new SqlParameter("@Three_Same_All", SqlDbType.VarChar,30),
					new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,30),
					new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,100),
					new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,30),
					new SqlParameter("@Two_Same_All", SqlDbType.VarChar,30),
					new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,100),
					new SqlParameter("@Two_dissame", SqlDbType.VarChar,100),
					new SqlParameter("@Input_Time", SqlDbType.DateTime),
					new SqlParameter("@DanTuo", SqlDbType.VarChar,20),
					new SqlParameter("@Zhu", SqlDbType.Int,4),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@PutGold", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
					new SqlParameter("@Zhu_money", SqlDbType.BigInt,8),
					new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
					new SqlParameter("@DanShuang", SqlDbType.VarChar,10),//isRobot
					new SqlParameter("@Odds", SqlDbType.Float,8),
                    new SqlParameter("@isRobot", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.Play_Way;
            parameters[2].Value = model.Sum;
            parameters[3].Value = model.Three_Same_All;
            parameters[4].Value = model.Three_Same_Single;
            parameters[5].Value = model.Three_Same_Not;
            parameters[6].Value = model.Three_Continue_All;
            parameters[7].Value = model.Two_Same_All;
            parameters[8].Value = model.Two_Same_Single;
            parameters[9].Value = model.Two_dissame;
            parameters[10].Value = model.Input_Time;
            parameters[11].Value = model.DanTuo;
            parameters[12].Value = model.Zhu;
            parameters[13].Value = model.GetMoney;
            parameters[14].Value = model.PutGold;
            parameters[15].Value = model.State;
            parameters[16].Value = model.Lottery_issue;
            parameters[17].Value = model.Zhu_money;
            parameters[18].Value = model.DaXiao;
            parameters[19].Value = model.DanShuang;
            parameters[20].Value = model.Odds;
            parameters[21].Value = model.isRobot;

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
        /// me_更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Bet_SWB set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_XK3_Bet_SWB");
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
        /// me_是否存在开奖后没有返奖的
        /// </summary>
        public bool Exists_num(string Lottery_issue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_XK3_Bet_SWB");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            strSql.Append(" and State=0 ");
            SqlParameter[] parameters = {
        			new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20)};
            parameters[0].Value = Lottery_issue;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_更新中奖状态
        /// </summary>
        public void Update_win(int ID, long GetMoney)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Bet_SWB set ");
            strSql.Append("GetMoney=@GetMoney");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@GetMoney", SqlDbType.BigInt,8)};

            parameters[0].Value = ID;
            parameters[1].Value = GetMoney;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据所点的历史记录，通过开奖期号查询相应的投注情况
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB_num(string Lottery_issue)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * from tb_XK3_Bet_SWB ");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
					new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20)};
            parameters[0].Value = Lottery_issue;

            BCW.XinKuai3.Model.XK3_Bet_SWB model = new BCW.XinKuai3.Model.XK3_Bet_SWB();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Play_Way = reader.GetInt32(2);
                    model.Sum = reader.GetString(3);
                    model.Three_Same_All = reader.GetString(4);
                    model.Three_Same_Single = reader.GetString(5);
                    model.Three_Same_Not = reader.GetString(6);
                    model.Three_Continue_All = reader.GetString(7);
                    model.Two_Same_All = reader.GetString(8);
                    model.Two_Same_Single = reader.GetString(9);
                    model.Two_dissame = reader.GetString(10);
                    model.Input_Time = reader.GetDateTime(11);
                    model.DanTuo = reader.GetString(12);
                    model.Zhu = reader.GetInt32(13);
                    model.GetMoney = reader.GetInt64(14);
                    model.PutGold = reader.GetInt64(15);
                    model.State = reader.GetInt32(16);
                    model.Lottery_issue = reader.GetString(17);
                    model.Zhu_money = reader.GetInt64(18);
                    model.DaXiao = reader.GetString(19);
                    model.DanShuang = reader.GetString(20);
                    //model.Odds = reader.GetDecimal(21);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_查找中奖后，超7天未领奖的id
        /// </summary>
        public void UpdateExceed_num(string _where)
        {
            StringBuilder strSql = new StringBuilder();
            string sd_where = _where;
            strSql.Append("UPDATE tb_XK3_Bet_SWB SET state=3");
            strSql.Append(sd_where);

            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan,string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum("+ziduan+") from tb_XK3_Bet_SWB");
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
        /// <summary>
        /// me_根据所点的历史记录，通过开奖期号查询相应的投注情况
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB_hounum(string Lottery_issue)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_XK3_Bet_SWB ");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
					new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20)};
            parameters[0].Value = Lottery_issue;

            BCW.XinKuai3.Model.XK3_Bet_SWB model = new BCW.XinKuai3.Model.XK3_Bet_SWB();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        //me_查询机器人购买次数
        public int GetXK3_Bet_SWB_GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_XK3_Bet_SWB ");
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
        // me_初始化某数据表
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }
        //me_取得每个玩家投注的记录进行排序11
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum1(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.XinKuai3.Model.XK3_Bet_SWB> listXK3_Bet_SWBs = new List<BCW.XinKuai3.Model.XK3_Bet_SWB>();
            string sTable = "tb_XK3_Bet_SWB";
            string sPkey = "";
            string sField = "UsID,Sum(GetMoney-PutGold) as bb";
            string sCondition = strWhere;
            string sOrder = "";
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
                    return listXK3_Bet_SWBs;
                }
                while (reader.Read())
                {
                    BCW.XinKuai3.Model.XK3_Bet_SWB objXK3_Bet_SWB = new BCW.XinKuai3.Model.XK3_Bet_SWB();
                    objXK3_Bet_SWB.UsID = reader.GetInt32(0);
                    objXK3_Bet_SWB.bb = reader.GetInt64(1);
                    listXK3_Bet_SWBs.Add(objXK3_Bet_SWB);
                }
            }
            return listXK3_Bet_SWBs;
        }
        //me_取得每个玩家投注的记录进行排序22
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.XinKuai3.Model.XK3_Bet_SWB> listXK3_Bet_SWBs = new List<BCW.XinKuai3.Model.XK3_Bet_SWB>();
            string sTable = "tb_XK3_Bet_SWB";
            string sPkey = "";
            string sField = "UsID,Sum(GetMoney) as bb";
            string sCondition = strWhere;
            string sOrder = "";
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
                    return listXK3_Bet_SWBs;
                }
                while (reader.Read())
                {
                    BCW.XinKuai3.Model.XK3_Bet_SWB objXK3_Bet_SWB = new BCW.XinKuai3.Model.XK3_Bet_SWB();
                    objXK3_Bet_SWB.UsID = reader.GetInt32(0);
                    objXK3_Bet_SWB.bb = reader.GetInt64(1);
                    listXK3_Bet_SWBs.Add(objXK3_Bet_SWB);
                }
            }
            return listXK3_Bet_SWBs;
        }
        //me_取得每个玩家投注的记录进行排序33
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum3(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.XinKuai3.Model.XK3_Bet_SWB> listXK3_Bet_SWBs = new List<BCW.XinKuai3.Model.XK3_Bet_SWB>();
            string sTable = "tb_XK3_Bet_SWB";
            string sPkey = "";
            string sField = "UsID,count(UsID) as bb";
            string sCondition = strWhere;
            string sOrder = "";
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
                    return listXK3_Bet_SWBs;
                }
                while (reader.Read())
                {
                    BCW.XinKuai3.Model.XK3_Bet_SWB objXK3_Bet_SWB = new BCW.XinKuai3.Model.XK3_Bet_SWB();
                    objXK3_Bet_SWB.UsID = reader.GetInt32(0);
                    objXK3_Bet_SWB.bb = reader.GetInt32(1);
                    listXK3_Bet_SWBs.Add(objXK3_Bet_SWB);
                }
            }
            return listXK3_Bet_SWBs;
        }


        /// <summary>
        ///  me_后台分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  "+ s1 +" from tb_XK3_Bet_SWB "+ s2 +" ");
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
		/// me_取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList XK3_Bet_SWB</returns>
		public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWBs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.XinKuai3.Model.XK3_Bet_SWB> listXK3_Bet_SWBs = new List<BCW.XinKuai3.Model.XK3_Bet_SWB>();
			string sTable = "tb_XK3_Bet_SWB";
			string sPkey = "id";
			string sField = "ID,UsID,Play_Way,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,Input_Time,DanTuo,Zhu,GetMoney,PutGold,State,Lottery_issue,Zhu_money,DaXiao,DanShuang";
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
					return listXK3_Bet_SWBs;
				}
				while (reader.Read())
				{
						BCW.XinKuai3.Model.XK3_Bet_SWB objXK3_Bet_SWB = new BCW.XinKuai3.Model.XK3_Bet_SWB();
						objXK3_Bet_SWB.ID = reader.GetInt32(0);
						objXK3_Bet_SWB.UsID = reader.GetInt32(1);
						objXK3_Bet_SWB.Play_Way = reader.GetInt32(2);
						objXK3_Bet_SWB.Sum = reader.GetString(3);
						objXK3_Bet_SWB.Three_Same_All = reader.GetString(4);
						objXK3_Bet_SWB.Three_Same_Single = reader.GetString(5);
						objXK3_Bet_SWB.Three_Same_Not = reader.GetString(6);
						objXK3_Bet_SWB.Three_Continue_All = reader.GetString(7);
						objXK3_Bet_SWB.Two_Same_All = reader.GetString(8);
						objXK3_Bet_SWB.Two_Same_Single = reader.GetString(9);
						objXK3_Bet_SWB.Two_dissame = reader.GetString(10);
						objXK3_Bet_SWB.Input_Time = reader.GetDateTime(11);
						objXK3_Bet_SWB.DanTuo = reader.GetString(12);
						objXK3_Bet_SWB.Zhu = reader.GetInt32(13);
						objXK3_Bet_SWB.GetMoney = reader.GetInt64(14);
						objXK3_Bet_SWB.PutGold = reader.GetInt64(15);
						objXK3_Bet_SWB.State = reader.GetInt32(16);
						objXK3_Bet_SWB.Lottery_issue = reader.GetString(17);
						objXK3_Bet_SWB.Zhu_money = reader.GetInt64(18);
						objXK3_Bet_SWB.DaXiao = reader.GetString(19);
						objXK3_Bet_SWB.DanShuang = reader.GetString(20);
						//objXK3_Bet_SWB.Odds = reader.GetDecimal(21);
						listXK3_Bet_SWBs.Add(objXK3_Bet_SWB);
				}
			}
			return listXK3_Bet_SWBs;
		}

		#endregion  成员方法
	}

    //==========================================
}

