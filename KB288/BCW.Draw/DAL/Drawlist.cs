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
	/// 数据访问类Drawlist。
	/// </summary>
	public class Drawlist
	{
		public Drawlist()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Drawlist"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Drawlist");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 计算jifen总币值
        /// </summary>
        public int GetJifensum(string Jifen, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(" + Jifen + ") from tb_Drawlist");
            strSql.Append(" where " + strWhere + " ");
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
		public int Add(BCW.Draw.Model.Drawlist model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Drawlist(");
			strSql.Append("UsID,Time,Type,GoodsCounts,Jifen)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@Time,@Type,@GoodsCounts,@Jifen)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Time", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Jifen", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.Time;
			parameters[2].Value = model.Type;
			parameters[3].Value = model.GoodsCounts;
			parameters[4].Value = model.Jifen;

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
		public void Update(BCW.Draw.Model.Drawlist model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Drawlist set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Time=@Time,");
			strSql.Append("Type=@Type,");
			strSql.Append("GoodsCounts=@GoodsCounts,");
			strSql.Append("Jifen=@Jifen");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Time", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Jifen", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.Time;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.GoodsCounts;
			parameters[5].Value = model.Jifen;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Drawlist ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.Drawlist GetDrawlist(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,Time,Type,GoodsCounts,Jifen from tb_Drawlist ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Draw.Model.Drawlist model=new BCW.Draw.Model.Drawlist();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.Time = reader.GetDateTime(2);
					model.Type = reader.GetInt32(3);
					model.GoodsCounts = reader.GetInt32(4);
					model.Jifen = reader.GetInt32(5);
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
			strSql.Append(" FROM tb_Drawlist ");
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
		/// <returns>IList Drawlist</returns>
		public IList<BCW.Draw.Model.Drawlist> GetDrawlists(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			IList<BCW.Draw.Model.Drawlist> listDrawlists = new List<BCW.Draw.Model.Drawlist>();
			string sTable = "tb_Drawlist";
			string sPkey = "id";
			string sField = "ID,UsID,Time,Type,GoodsCounts,Jifen";
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
					return listDrawlists;
				}
				while (reader.Read())
				{
						BCW.Draw.Model.Drawlist objDrawlist = new BCW.Draw.Model.Drawlist();
						objDrawlist.ID = reader.GetInt32(0);
						objDrawlist.UsID = reader.GetInt32(1);
						objDrawlist.Time = reader.GetDateTime(2);
						objDrawlist.Type = reader.GetInt32(3);
						objDrawlist.GoodsCounts = reader.GetInt32(4);
						objDrawlist.Jifen = reader.GetInt32(5);
						listDrawlists.Add(objDrawlist);
				}
			}
			return listDrawlists;
		}

		#endregion  成员方法
	}
}

