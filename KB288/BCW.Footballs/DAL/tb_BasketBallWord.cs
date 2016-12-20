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
	/// 数据访问类tb_BasketBallWord。
	/// </summary>
	public class tb_BasketBallWord
	{
		public tb_BasketBallWord()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BasketBallWord");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsName_enOne(int last)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BasketBallWord");
            strSql.Append(" where last=@last ");
            SqlParameter[] parameters = {
                    new SqlParameter("@last", SqlDbType.Int,4)};
            parameters[0].Value = last;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsName(int name_enId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BasketBallWord");
            strSql.Append(" where name_enId=@name_enId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_enId", SqlDbType.Int,4)};
            parameters[0].Value = name_enId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_BasketBallWord model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_BasketBallWord(");
			strSql.Append("name_enId,hometeam,guestteam,listContent,whichTeam,addtime,last,isSame)");
			strSql.Append(" values (");
			strSql.Append("@name_enId,@hometeam,@guestteam,@listContent,@whichTeam,@addtime,@last,@isSame)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@name_enId", SqlDbType.Int,4),
					new SqlParameter("@hometeam", SqlDbType.NChar,100),
					new SqlParameter("@guestteam", SqlDbType.NChar,100),
					new SqlParameter("@listContent", SqlDbType.NVarChar,4000),
					new SqlParameter("@whichTeam", SqlDbType.NVarChar,100),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@last", SqlDbType.Int,4),
					new SqlParameter("@isSame", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.name_enId;
			parameters[1].Value = model.hometeam;
			parameters[2].Value = model.guestteam;
			parameters[3].Value = model.listContent;
			parameters[4].Value = model.whichTeam;
			parameters[5].Value = model.addtime;
			parameters[6].Value = model.last;
			parameters[7].Value = model.isSame;

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
		public void Update(BCW.Model.tb_BasketBallWord model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_BasketBallWord set ");
			strSql.Append("name_enId=@name_enId,");
			strSql.Append("hometeam=@hometeam,");
			strSql.Append("guestteam=@guestteam,");
			strSql.Append("listContent=@listContent,");
			strSql.Append("whichTeam=@whichTeam,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("last=@last,");
			strSql.Append("isSame=@isSame");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@name_enId", SqlDbType.Int,4),
					new SqlParameter("@hometeam", SqlDbType.NChar,100),
					new SqlParameter("@guestteam", SqlDbType.NChar,100),
					new SqlParameter("@listContent", SqlDbType.NVarChar,4000),
					new SqlParameter("@whichTeam", SqlDbType.NVarChar,100),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@last", SqlDbType.Int,4),
					new SqlParameter("@isSame", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.name_enId;
			parameters[2].Value = model.hometeam;
			parameters[3].Value = model.guestteam;
			parameters[4].Value = model.listContent;
			parameters[5].Value = model.whichTeam;
			parameters[6].Value = model.addtime;
			parameters[7].Value = model.last;
			parameters[8].Value = model.isSame;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BasketBallWord ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_BasketBallWord Gettb_BasketBallWord(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,name_enId,hometeam,guestteam,listContent,whichTeam,addtime,last,isSame from tb_BasketBallWord ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_BasketBallWord model=new BCW.Model.tb_BasketBallWord();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.name_enId = reader.GetInt32(1);
					model.hometeam = reader.GetString(2);
					model.guestteam = reader.GetString(3);
					model.listContent = reader.GetString(4);
					model.whichTeam = reader.GetString(5);
					model.addtime = reader.GetDateTime(6);
					model.last = reader.GetInt32(7);
					model.isSame = reader.GetString(8);
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
			strSql.Append(" FROM tb_BasketBallWord ");
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
		/// <returns>IList tb_BasketBallWord</returns>
		public IList<BCW.Model.tb_BasketBallWord> Gettb_BasketBallWords(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_BasketBallWord> listtb_BasketBallWords = new List<BCW.Model.tb_BasketBallWord>();
			string sTable = "tb_BasketBallWord";
			string sPkey = "id";
			string sField = "ID,name_enId,hometeam,guestteam,listContent,whichTeam,addtime,last,isSame";
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
					return listtb_BasketBallWords;
				}
				while (reader.Read())
				{
						BCW.Model.tb_BasketBallWord objtb_BasketBallWord = new BCW.Model.tb_BasketBallWord();
						objtb_BasketBallWord.ID = reader.GetInt32(0);
						objtb_BasketBallWord.name_enId = reader.GetInt32(1);
						objtb_BasketBallWord.hometeam = reader.GetString(2);
						objtb_BasketBallWord.guestteam = reader.GetString(3);
						objtb_BasketBallWord.listContent = reader.GetString(4);
						objtb_BasketBallWord.whichTeam = reader.GetString(5);
						objtb_BasketBallWord.addtime = reader.GetDateTime(6);
						objtb_BasketBallWord.last = reader.GetInt32(7);
						objtb_BasketBallWord.isSame = reader.GetString(8);
						listtb_BasketBallWords.Add(objtb_BasketBallWord);
				}
			}
			return listtb_BasketBallWords;
		}

		#endregion  成员方法
	}
}

