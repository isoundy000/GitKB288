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
	/// 数据访问类tb_BasketBallCollect。
	/// </summary>
	public class tb_BasketBallCollect
	{
		public tb_BasketBallCollect()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BasketBallCollect");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsUsIdAndBaskId(int UsId, int BaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BasketBallCollect");
            strSql.Append(" where UsId=@UsId and BasketBallId=@BasketBallId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsId", SqlDbType.Int,4),
            new SqlParameter("@BasketBallId", SqlDbType.Int,4)};
            parameters[0].Value = UsId;
            parameters[1].Value = BaskId;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_BasketBallCollect model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_BasketBallCollect(");
			strSql.Append("UsId,UsName,BasketBallId,Bianhao,team1,team2,result,sendCount,AddTime,ident,Remark)");
			strSql.Append(" values (");
			strSql.Append("@UsId,@UsName,@BasketBallId,@Bianhao,@team1,@team2,@result,@sendCount,@AddTime,@ident,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NChar,100),
					new SqlParameter("@BasketBallId", SqlDbType.Int,4),
					new SqlParameter("@Bianhao", SqlDbType.Int,4),
					new SqlParameter("@team1", SqlDbType.NChar,200),
					new SqlParameter("@team2", SqlDbType.NChar,200),
					new SqlParameter("@result", SqlDbType.NChar,50),
					new SqlParameter("@sendCount", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ident", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NChar,500)};
			parameters[0].Value = model.UsId;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.BasketBallId;
			parameters[3].Value = model.Bianhao;
			parameters[4].Value = model.team1;
			parameters[5].Value = model.team2;
			parameters[6].Value = model.result;
			parameters[7].Value = model.sendCount;
			parameters[8].Value = model.AddTime;
			parameters[9].Value = model.ident;
			parameters[10].Value = model.Remark;

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
		public void Update(BCW.Model.tb_BasketBallCollect model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_BasketBallCollect set ");
			strSql.Append("UsId=@UsId,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("BasketBallId=@BasketBallId,");
			strSql.Append("Bianhao=@Bianhao,");
			strSql.Append("team1=@team1,");
			strSql.Append("team2=@team2,");
			strSql.Append("result=@result,");
			strSql.Append("sendCount=@sendCount,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("ident=@ident,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NChar,100),
					new SqlParameter("@BasketBallId", SqlDbType.Int,4),
					new SqlParameter("@Bianhao", SqlDbType.Int,4),
					new SqlParameter("@team1", SqlDbType.NChar,200),
					new SqlParameter("@team2", SqlDbType.NChar,200),
					new SqlParameter("@result", SqlDbType.NChar,50),
					new SqlParameter("@sendCount", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ident", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NChar,500)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsId;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.BasketBallId;
			parameters[4].Value = model.Bianhao;
			parameters[5].Value = model.team1;
			parameters[6].Value = model.team2;
			parameters[7].Value = model.result;
			parameters[8].Value = model.sendCount;
			parameters[9].Value = model.AddTime;
			parameters[10].Value = model.ident;
			parameters[11].Value = model.Remark;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BasketBallCollect ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID, int UsId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BasketBallCollect ");
            strSql.Append(" where BasketBallId=@ID and UsId=@UsId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
            new SqlParameter("@UsId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsId;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_BasketBallCollect Gettb_BasketBallCollect(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsId,UsName,BasketBallId,Bianhao,team1,team2,result,sendCount,AddTime,ident,Remark from tb_BasketBallCollect ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_BasketBallCollect model=new BCW.Model.tb_BasketBallCollect();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsId = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.BasketBallId = reader.GetInt32(3);
					model.Bianhao = reader.GetInt32(4);
					model.team1 = reader.GetString(5);
					model.team2 = reader.GetString(6);
					model.result = reader.GetString(7);
					model.sendCount = reader.GetInt32(8);
					model.AddTime = reader.GetDateTime(9);
					model.ident = reader.GetInt32(10);
					model.Remark = reader.GetString(11);
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
			strSql.Append(" FROM tb_BasketBallCollect ");
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
		/// <returns>IList tb_BasketBallCollect</returns>
		public IList<BCW.Model.tb_BasketBallCollect> Gettb_BasketBallCollects(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_BasketBallCollect> listtb_BasketBallCollects = new List<BCW.Model.tb_BasketBallCollect>();
			string sTable = "tb_BasketBallCollect";
			string sPkey = "id";
			string sField = "ID,UsId,UsName,BasketBallId,Bianhao,team1,team2,result,sendCount,AddTime,ident,Remark";
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
					return listtb_BasketBallCollects;
				}
				while (reader.Read())
				{
						BCW.Model.tb_BasketBallCollect objtb_BasketBallCollect = new BCW.Model.tb_BasketBallCollect();
						objtb_BasketBallCollect.ID = reader.GetInt32(0);
						objtb_BasketBallCollect.UsId = reader.GetInt32(1);
						objtb_BasketBallCollect.UsName = reader.GetString(2);
						objtb_BasketBallCollect.BasketBallId = reader.GetInt32(3);
						objtb_BasketBallCollect.Bianhao = reader.GetInt32(4);
						objtb_BasketBallCollect.team1 = reader.GetString(5);
						objtb_BasketBallCollect.team2 = reader.GetString(6);
						objtb_BasketBallCollect.result = reader.GetString(7);
						objtb_BasketBallCollect.sendCount = reader.GetInt32(8);
						objtb_BasketBallCollect.AddTime = reader.GetDateTime(9);
						objtb_BasketBallCollect.ident = reader.GetInt32(10);
						objtb_BasketBallCollect.Remark = reader.GetString(11);
						listtb_BasketBallCollects.Add(objtb_BasketBallCollect);
				}
			}
			return listtb_BasketBallCollects;
		}

		#endregion  成员方法
	}
}

