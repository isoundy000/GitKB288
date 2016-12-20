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
	/// 数据访问类tb_QuestionsList。
	/// </summary>
	public class tb_QuestionsList
	{
		public tb_QuestionsList()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_QuestionsList");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.tb_QuestionsList model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_QuestionsList(");
			strSql.Append("question,chooseA,chooseB,chooseC,chooseD,answer,styleID,style,type,deficult,awardId,awardType,awardGold,title,img,hit,statue,trueAnswer,falseAnswer,remarks,indent,addtime)");
			strSql.Append(" values (");
			strSql.Append("@question,@chooseA,@chooseB,@chooseC,@chooseD,@answer,@styleID,@style,@type,@deficult,@awardId,@awardType,@awardGold,@title,@img,@hit,@statue,@trueAnswer,@falseAnswer,@remarks,@indent,@addtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@question", SqlDbType.NVarChar,500),
					new SqlParameter("@chooseA", SqlDbType.NVarChar,200),
					new SqlParameter("@chooseB", SqlDbType.NVarChar,200),
					new SqlParameter("@chooseC", SqlDbType.NVarChar,200),
					new SqlParameter("@chooseD", SqlDbType.NVarChar,200),
					new SqlParameter("@answer", SqlDbType.NVarChar,200),
					new SqlParameter("@styleID", SqlDbType.NChar,50),
					new SqlParameter("@style", SqlDbType.Int,4),
					new SqlParameter("@type", SqlDbType.NChar,50),
					new SqlParameter("@deficult", SqlDbType.Int,4),
					new SqlParameter("@awardId", SqlDbType.Int,4),
					new SqlParameter("@awardType", SqlDbType.NVarChar,100),
					new SqlParameter("@awardGold", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.NVarChar,100),
					new SqlParameter("@img", SqlDbType.NVarChar),
					new SqlParameter("@hit", SqlDbType.Int,4),
					new SqlParameter("@statue", SqlDbType.Int,4),
					new SqlParameter("@trueAnswer", SqlDbType.Int,4),
					new SqlParameter("@falseAnswer", SqlDbType.Int,4),
					new SqlParameter("@remarks", SqlDbType.NVarChar,100),
					new SqlParameter("@indent", SqlDbType.NVarChar,100),
					new SqlParameter("@addtime", SqlDbType.DateTime)};
			parameters[0].Value = model.question;
			parameters[1].Value = model.chooseA;
			parameters[2].Value = model.chooseB;
			parameters[3].Value = model.chooseC;
			parameters[4].Value = model.chooseD;
			parameters[5].Value = model.answer;
			parameters[6].Value = model.styleID;
			parameters[7].Value = model.style;
			parameters[8].Value = model.type;
			parameters[9].Value = model.deficult;
			parameters[10].Value = model.awardId;
			parameters[11].Value = model.awardType;
			parameters[12].Value = model.awardGold;
			parameters[13].Value = model.title;
			parameters[14].Value = model.img;
			parameters[15].Value = model.hit;
			parameters[16].Value = model.statue;
			parameters[17].Value = model.trueAnswer;
			parameters[18].Value = model.falseAnswer;
			parameters[19].Value = model.remarks;
			parameters[20].Value = model.indent;
			parameters[21].Value = model.addtime;

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
		public void Update(BCW.Model.tb_QuestionsList model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_QuestionsList set ");
			strSql.Append("question=@question,");
			strSql.Append("chooseA=@chooseA,");
			strSql.Append("chooseB=@chooseB,");
			strSql.Append("chooseC=@chooseC,");
			strSql.Append("chooseD=@chooseD,");
			strSql.Append("answer=@answer,");
			strSql.Append("styleID=@styleID,");
			strSql.Append("style=@style,");
			strSql.Append("type=@type,");
			strSql.Append("deficult=@deficult,");
			strSql.Append("awardId=@awardId,");
			strSql.Append("awardType=@awardType,");
			strSql.Append("awardGold=@awardGold,");
			strSql.Append("title=@title,");
			strSql.Append("img=@img,");
			strSql.Append("hit=@hit,");
			strSql.Append("statue=@statue,");
			strSql.Append("trueAnswer=@trueAnswer,");
			strSql.Append("falseAnswer=@falseAnswer,");
			strSql.Append("remarks=@remarks,");
			strSql.Append("indent=@indent,");
			strSql.Append("addtime=@addtime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@question", SqlDbType.NVarChar,500),
					new SqlParameter("@chooseA", SqlDbType.NVarChar,200),
					new SqlParameter("@chooseB", SqlDbType.NVarChar,200),
					new SqlParameter("@chooseC", SqlDbType.NVarChar,200),
					new SqlParameter("@chooseD", SqlDbType.NVarChar,200),
					new SqlParameter("@answer", SqlDbType.NVarChar,200),
					new SqlParameter("@styleID", SqlDbType.NChar,50),
					new SqlParameter("@style", SqlDbType.Int,4),
					new SqlParameter("@type", SqlDbType.NChar,50),
					new SqlParameter("@deficult", SqlDbType.Int,4),
					new SqlParameter("@awardId", SqlDbType.Int,4),
					new SqlParameter("@awardType", SqlDbType.NVarChar,100),
					new SqlParameter("@awardGold", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.NVarChar,100),
					new SqlParameter("@img", SqlDbType.NVarChar),
					new SqlParameter("@hit", SqlDbType.Int,4),
					new SqlParameter("@statue", SqlDbType.Int,4),
					new SqlParameter("@trueAnswer", SqlDbType.Int,4),
					new SqlParameter("@falseAnswer", SqlDbType.Int,4),
					new SqlParameter("@remarks", SqlDbType.NVarChar,100),
					new SqlParameter("@indent", SqlDbType.NVarChar,100),
					new SqlParameter("@addtime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.question;
			parameters[2].Value = model.chooseA;
			parameters[3].Value = model.chooseB;
			parameters[4].Value = model.chooseC;
			parameters[5].Value = model.chooseD;
			parameters[6].Value = model.answer;
			parameters[7].Value = model.styleID;
			parameters[8].Value = model.style;
			parameters[9].Value = model.type;
			parameters[10].Value = model.deficult;
			parameters[11].Value = model.awardId;
			parameters[12].Value = model.awardType;
			parameters[13].Value = model.awardGold;
			parameters[14].Value = model.title;
			parameters[15].Value = model.img;
			parameters[16].Value = model.hit;
			parameters[17].Value = model.statue;
			parameters[18].Value = model.trueAnswer;
			parameters[19].Value = model.falseAnswer;
			parameters[20].Value = model.remarks;
			parameters[21].Value = model.indent;
			parameters[22].Value = model.addtime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_QuestionsList ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_QuestionsList Gettb_QuestionsList(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,question,chooseA,chooseB,chooseC,chooseD,answer,styleID,style,type,deficult,awardId,awardType,awardGold,title,img,hit,statue,trueAnswer,falseAnswer,remarks,indent,addtime from tb_QuestionsList ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_QuestionsList model=new BCW.Model.tb_QuestionsList();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.question = reader.GetString(1);
					model.chooseA = reader.GetString(2);
					model.chooseB = reader.GetString(3);
					model.chooseC = reader.GetString(4);
					model.chooseD = reader.GetString(5);
					model.answer = reader.GetString(6);
					model.styleID = reader.GetString(7);
					model.style = reader.GetInt32(8);
					model.type = reader.GetString(9);
					model.deficult = reader.GetInt32(10);
					model.awardId = reader.GetInt32(11);
					model.awardType = reader.GetString(12);
					model.awardGold = reader.GetInt32(13);
					model.title = reader.GetString(14);
					model.img = reader.GetString(15);
					model.hit = reader.GetInt32(16);
					model.statue = reader.GetInt32(17);
					model.trueAnswer = reader.GetInt32(18);
					model.falseAnswer = reader.GetInt32(19);
					model.remarks = reader.GetString(20);
					model.indent = reader.GetString(21);
					model.addtime = reader.GetDateTime(22);
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
			strSql.Append(" FROM tb_QuestionsList ");
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
		/// <returns>IList tb_QuestionsList</returns>
		public IList<BCW.Model.tb_QuestionsList> Gettb_QuestionsLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_QuestionsList> listtb_QuestionsLists = new List<BCW.Model.tb_QuestionsList>();
			string sTable = "tb_QuestionsList";
			string sPkey = "id";
			string sField = "ID,question,chooseA,chooseB,chooseC,chooseD,answer,styleID,style,type,deficult,awardId,awardType,awardGold,title,img,hit,statue,trueAnswer,falseAnswer,remarks,indent,addtime";
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
					return listtb_QuestionsLists;
				}
				while (reader.Read())
				{
						BCW.Model.tb_QuestionsList objtb_QuestionsList = new BCW.Model.tb_QuestionsList();
						objtb_QuestionsList.ID = reader.GetInt32(0);
						objtb_QuestionsList.question = reader.GetString(1);
						objtb_QuestionsList.chooseA = reader.GetString(2);
						objtb_QuestionsList.chooseB = reader.GetString(3);
						objtb_QuestionsList.chooseC = reader.GetString(4);
						objtb_QuestionsList.chooseD = reader.GetString(5);
						objtb_QuestionsList.answer = reader.GetString(6);
						objtb_QuestionsList.styleID = reader.GetString(7);
						objtb_QuestionsList.style = reader.GetInt32(8);
						objtb_QuestionsList.type = reader.GetString(9);
						objtb_QuestionsList.deficult = reader.GetInt32(10);
						objtb_QuestionsList.awardId = reader.GetInt32(11);
						objtb_QuestionsList.awardType = reader.GetString(12);
						objtb_QuestionsList.awardGold = reader.GetInt32(13);
						objtb_QuestionsList.title = reader.GetString(14);
						objtb_QuestionsList.img = reader.GetString(15);
						objtb_QuestionsList.hit = reader.GetInt32(16);
						objtb_QuestionsList.statue = reader.GetInt32(17);
						objtb_QuestionsList.trueAnswer = reader.GetInt32(18);
						objtb_QuestionsList.falseAnswer = reader.GetInt32(19);
						objtb_QuestionsList.remarks = reader.GetString(20);
						objtb_QuestionsList.indent = reader.GetString(21);
						objtb_QuestionsList.addtime = reader.GetDateTime(22);
						listtb_QuestionsLists.Add(objtb_QuestionsList);
				}
			}
			return listtb_QuestionsLists;
		}

		#endregion  成员方法
	}
}

