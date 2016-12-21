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
	/// 数据访问类SysTemp。
	/// </summary>
	public class SysTemp
	{
		public SysTemp()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_SysTemp"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_SysTemp");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.SysTemp model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SysTemp(");
			strSql.Append("GuessOddsTime)");
			strSql.Append(" values (");
			strSql.Append("@GuessOddsTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GuessOddsTime", SqlDbType.DateTime)};
			parameters[0].Value = model.GuessOddsTime;

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
        /// 更新GuessOddsTime
		/// </summary>
        public void UpdateGuessOddsTime(int ID, DateTime GuessOddsTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SysTemp set ");
            strSql.Append("GuessOddsTime=@GuessOddsTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GuessOddsTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = GuessOddsTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SysTemp ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
        /// 得到GuessOddsTime
		/// </summary>
        public DateTime GetGuessOddsTime(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 GuessOddsTime from tb_SysTemp ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
                    return reader.GetDateTime(0);
				}
				else
				{
                    return DateTime.Parse("1990-1-1");
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
			strSql.Append(" FROM tb_SysTemp ");
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
		/// <returns>IList SysTemp</returns>
		public IList<BCW.Model.SysTemp> GetSysTemps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.SysTemp> listSysTemps = new List<BCW.Model.SysTemp>();
			string sTable = "tb_SysTemp";
			string sPkey = "id";
			string sField = "ID,GuessOddsTime";
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
					return listSysTemps;
				}
				while (reader.Read())
				{
						BCW.Model.SysTemp objSysTemp = new BCW.Model.SysTemp();
						objSysTemp.ID = reader.GetInt32(0);
						objSysTemp.GuessOddsTime = reader.GetDateTime(1);
						listSysTemps.Add(objSysTemp);
				}
			}
			return listSysTemps;
		}

		#endregion  成员方法
	}
}

