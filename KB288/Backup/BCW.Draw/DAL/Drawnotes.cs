using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Draw.DAL
{
	/// <summary>
	/// 数据访问类Drawnotes。
	/// </summary>
	public class Drawnotes
	{
		public Drawnotes()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Drawnotes"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Drawnotes");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Draw.Model.Drawnotes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Drawnotes(");
			strSql.Append("UsID,jifen,game,gname,gvalue,jvalue,change,date)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@jifen,@game,@gname,@gvalue,@jvalue,@change,@date)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@jifen", SqlDbType.BigInt,8),
					new SqlParameter("@game", SqlDbType.NVarChar,50),
					new SqlParameter("@gname", SqlDbType.NVarChar,50),
					new SqlParameter("@gvalue", SqlDbType.BigInt,8),
					new SqlParameter("@jvalue", SqlDbType.BigInt,8),
					new SqlParameter("@change", SqlDbType.BigInt,8),
					new SqlParameter("@date", SqlDbType.DateTime)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.jifen;
			parameters[2].Value = model.game;
			parameters[3].Value = model.gname;
			parameters[4].Value = model.gvalue;
			parameters[5].Value = model.jvalue;
			parameters[6].Value = model.change;
			parameters[7].Value = model.date;

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
		public void Update(BCW.Draw.Model.Drawnotes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Drawnotes set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("jifen=@jifen,");
			strSql.Append("game=@game,");
			strSql.Append("gname=@gname,");
			strSql.Append("gvalue=@gvalue,");
			strSql.Append("jvalue=@jvalue,");
			strSql.Append("change=@change,");
			strSql.Append("date=@date");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@jifen", SqlDbType.BigInt,8),
					new SqlParameter("@game", SqlDbType.NVarChar,50),
					new SqlParameter("@gname", SqlDbType.NVarChar,50),
					new SqlParameter("@gvalue", SqlDbType.BigInt,8),
					new SqlParameter("@jvalue", SqlDbType.BigInt,8),
					new SqlParameter("@change", SqlDbType.BigInt,8),
					new SqlParameter("@date", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.jifen;
			parameters[3].Value = model.game;
			parameters[4].Value = model.gname;
			parameters[5].Value = model.gvalue;
			parameters[6].Value = model.jvalue;
			parameters[7].Value = model.change;
			parameters[8].Value = model.date;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Drawnotes ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.Drawnotes GetDrawnotes(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,jifen,game,gname,gvalue,jvalue,change,date from tb_Drawnotes ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Draw.Model.Drawnotes model=new BCW.Draw.Model.Drawnotes();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.jifen = reader.GetInt64(2);
					model.game = reader.GetString(3);
					model.gname = reader.GetString(4);
					model.gvalue = reader.GetInt64(5);
					model.jvalue = reader.GetInt64(6);
					model.change = reader.GetInt64(7);
					model.date = reader.GetDateTime(8);
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
			strSql.Append(" FROM tb_Drawnotes ");
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
		/// <returns>IList Drawnotes</returns>
		public IList<BCW.Draw.Model.Drawnotes> GetDrawnotess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Draw.Model.Drawnotes> listDrawnotess = new List<BCW.Draw.Model.Drawnotes>();
			string sTable = "tb_Drawnotes";
			string sPkey = "id";
			string sField = "ID,UsID,jifen,game,gname,gvalue,jvalue,change,date";
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
					return listDrawnotess;
				}
				while (reader.Read())
				{
						BCW.Draw.Model.Drawnotes objDrawnotes = new BCW.Draw.Model.Drawnotes();
						objDrawnotes.ID = reader.GetInt32(0);
						objDrawnotes.UsID = reader.GetInt32(1);
						objDrawnotes.jifen = reader.GetInt64(2);
						objDrawnotes.game = reader.GetString(3);
						objDrawnotes.gname = reader.GetString(4);
						objDrawnotes.gvalue = reader.GetInt64(5);
						objDrawnotes.jvalue = reader.GetInt64(6);
						objDrawnotes.change = reader.GetInt64(7);
						objDrawnotes.date = reader.GetDateTime(8);
						listDrawnotess.Add(objDrawnotes);
				}
			}
			return listDrawnotess;
		}

		#endregion  成员方法
	}
}

