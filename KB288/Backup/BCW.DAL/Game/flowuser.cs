using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
	/// <summary>
	/// 数据访问类flowuser。
	/// </summary>
	public class flowuser
	{
		public flowuser()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_flowuser"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_flowuser");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 得到排名
        /// </summary>
        public int GetTop(int UsID, string Field)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(UsID) from tb_flowuser where " + Field + ">=(select " + Field + " from tb_flowuser");
            strSql.Append(" where UsID=@UsID) ");
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
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.flowuser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_flowuser(");
            strSql.Append("UsID,UsName,iFlows,Score,Score2,Score3,Score4,Score5,FlowStat,AddTime,iBw)");
			strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@iFlows,@Score,@Score2,@Score3,@Score4,@Score5,@FlowStat,@AddTime,@iBw)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@iFlows", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Score2", SqlDbType.Int,4),
					new SqlParameter("@Score3", SqlDbType.Int,4),
					new SqlParameter("@Score4", SqlDbType.Int,4),
					new SqlParameter("@Score5", SqlDbType.Int,4),
					new SqlParameter("@FlowStat", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@iBw", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.iFlows;
			parameters[3].Value = model.Score;
			parameters[4].Value = model.Score2;
			parameters[5].Value = model.Score3;
            parameters[6].Value = model.Score4;
            parameters[7].Value = model.Score5;
			parameters[8].Value = model.FlowStat;
			parameters[9].Value = model.AddTime;
            parameters[10].Value = 0;

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
		public void Update(BCW.Model.Game.flowuser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_flowuser set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("iFlows=@iFlows,");
			strSql.Append("Score=@Score,");
			strSql.Append("Score2=@Score2,");
			strSql.Append("Score3=@Score3,");
			strSql.Append("FlowStat=@FlowStat,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@iFlows", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Score2", SqlDbType.Int,4),
					new SqlParameter("@Score3", SqlDbType.Int,4),
					new SqlParameter("@FlowStat", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.iFlows;
			parameters[4].Value = model.Score;
			parameters[5].Value = model.Score2;
			parameters[6].Value = model.Score3;
			parameters[7].Value = model.FlowStat;
			parameters[8].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateScore(int UsID, int Types, int Score)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flowuser set ");
            if (Types == 1)
                strSql.Append("Score=Score+@Score");
            else if (Types == 2)
                strSql.Append("Score2=Score2+@Score");
            else if (Types == 3)
                strSql.Append("Score3=Score3+@Score");
            else if (Types == 4)
                strSql.Append("Score4=Score4+@Score");
            else if (Types == 5)
                strSql.Append("Score5=Score5+@Score");

            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Score;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新花盆数量
        /// </summary>
        public void UpdateiFlows(int UsID, int iFlows)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flowuser set ");
            strSql.Append("iFlows=@iFlows");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@iFlows", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = iFlows;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新今天被玩次数(iBw值为0则重置为0)
        /// </summary>
        public void UpdateiBw(int UsID, int iBw)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flowuser set ");
            if (iBw == 0)
                strSql.Append("iBw=@iBw,");
            else
                strSql.Append("iBw=iBw+@iBw,");

            strSql.Append("BwTime=@BwTime");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@iBw", SqlDbType.Int,4),
					new SqlParameter("@BwTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = iBw;
            parameters[2].Value = DateTime.Now.ToLongDateString();

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }



		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_flowuser ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 得到花盆数量
        /// </summary>
        public int GetiFlows(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 iFlows from tb_flowuser ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 得到技能积分
        /// </summary>
        public int GetScore(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Score from tb_flowuser ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 得到风采积分
        /// </summary>
        public int GetScore2(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Score2 from tb_flowuser ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
		public BCW.Model.Game.flowuser Getflowuser(int UsID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,iFlows,Score,Score2,Score3,Score4,Score5,FlowStat,AddTime,iBw,BwTime from tb_flowuser ");
            strSql.Append(" where UsID=@UsID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

			BCW.Model.Game.flowuser model=new BCW.Model.Game.flowuser();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.iFlows = reader.GetInt32(3);
					model.Score = reader.GetInt32(4);
					model.Score2 = reader.GetInt32(5);
					model.Score3 = reader.GetInt32(6);
                    model.Score4 = reader.GetInt32(7);
                    model.Score5 = reader.GetInt32(8);
					model.FlowStat = reader.GetString(9);
					model.AddTime = reader.GetDateTime(10);
                    model.iBw = reader.GetInt32(11);
                    if (!reader.IsDBNull(12))
                        model.BwTime = reader.GetDateTime(12);
                    else
                        model.BwTime = DateTime.Parse("1990-1-1");

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
			strSql.Append(" FROM tb_flowuser ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
		/// <returns>IList flowuser</returns>
		public IList<BCW.Model.Game.flowuser> GetflowusersTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.flowuser> listflowusers = new List<BCW.Model.Game.flowuser>();
			string sTable = "tb_flowuser";
			string sPkey = "id";
            string sField = "UsID,UsName," + strWhere + "";
			string sCondition = "";
            string sOrder = "" + strWhere + " Desc";
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
                    return listflowusers;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.flowuser objflowuser = new BCW.Model.Game.flowuser();
                    objflowuser.UsID = reader.GetInt32(0);
                    objflowuser.UsName = reader.GetString(1);
                    objflowuser.Score = reader.GetInt32(2);
                    listflowusers.Add(objflowuser);
                }
            }
			return listflowusers;
		}

		#endregion  成员方法
	}
}

