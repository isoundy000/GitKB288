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
	/// 数据访问类tb_QuestionAnswer。
	/// </summary>
	public class tb_QuestionAnswer
	{
		public tb_QuestionAnswer()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_QuestionAnswer");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.tb_QuestionAnswer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_QuestionAnswer(");
			strSql.Append("usid,usname,questID,questtion,answer,isTrue,isHit,isGet,getType,getGold,isDone,addtime,needTime,isOver,ident)");
			strSql.Append(" values (");
			strSql.Append("@usid,@usname,@questID,@questtion,@answer,@isTrue,@isHit,@isGet,@getType,@getGold,@isDone,@addtime,@needTime,@isOver,@ident)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@usname", SqlDbType.NVarChar,50),
					new SqlParameter("@questID", SqlDbType.Int,4),
					new SqlParameter("@questtion", SqlDbType.NVarChar,200),
					new SqlParameter("@answer", SqlDbType.NVarChar,50),
					new SqlParameter("@isTrue", SqlDbType.Int,4),
					new SqlParameter("@isHit", SqlDbType.Int,4),
					new SqlParameter("@isGet", SqlDbType.Int,4),
					new SqlParameter("@getType", SqlDbType.NVarChar,50),
					new SqlParameter("@getGold", SqlDbType.BigInt,8),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@needTime", SqlDbType.Int,4),
					new SqlParameter("@isOver", SqlDbType.Int,4),
					new SqlParameter("@ident", SqlDbType.Int,4)};
			parameters[0].Value = model.usid;
			parameters[1].Value = model.usname;
			parameters[2].Value = model.questID;
			parameters[3].Value = model.questtion;
			parameters[4].Value = model.answer;
			parameters[5].Value = model.isTrue;
			parameters[6].Value = model.isHit;
			parameters[7].Value = model.isGet;
			parameters[8].Value = model.getType;
			parameters[9].Value = model.getGold;
			parameters[10].Value = model.isDone;
			parameters[11].Value = model.addtime;
			parameters[12].Value = model.needTime;
			parameters[13].Value = model.isOver;
			parameters[14].Value = model.ident;

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
		public void Update(BCW.Model.tb_QuestionAnswer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_QuestionAnswer set ");
			strSql.Append("usid=@usid,");
			strSql.Append("usname=@usname,");
			strSql.Append("questID=@questID,");
			strSql.Append("questtion=@questtion,");
			strSql.Append("answer=@answer,");
			strSql.Append("isTrue=@isTrue,");
			strSql.Append("isHit=@isHit,");
			strSql.Append("isGet=@isGet,");
			strSql.Append("getType=@getType,");
			strSql.Append("getGold=@getGold,");
			strSql.Append("isDone=@isDone,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("needTime=@needTime,");
			strSql.Append("isOver=@isOver,");
			strSql.Append("ident=@ident");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@usname", SqlDbType.NVarChar,50),
					new SqlParameter("@questID", SqlDbType.Int,4),
					new SqlParameter("@questtion", SqlDbType.NVarChar,200),
					new SqlParameter("@answer", SqlDbType.NVarChar,50),
					new SqlParameter("@isTrue", SqlDbType.Int,4),
					new SqlParameter("@isHit", SqlDbType.Int,4),
					new SqlParameter("@isGet", SqlDbType.Int,4),
					new SqlParameter("@getType", SqlDbType.NVarChar,50),
					new SqlParameter("@getGold", SqlDbType.BigInt,8),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@needTime", SqlDbType.Int,4),
					new SqlParameter("@isOver", SqlDbType.Int,4),
					new SqlParameter("@ident", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.usid;
			parameters[2].Value = model.usname;
			parameters[3].Value = model.questID;
			parameters[4].Value = model.questtion;
			parameters[5].Value = model.answer;
			parameters[6].Value = model.isTrue;
			parameters[7].Value = model.isHit;
			parameters[8].Value = model.isGet;
			parameters[9].Value = model.getType;
			parameters[10].Value = model.getGold;
			parameters[11].Value = model.isDone;
			parameters[12].Value = model.addtime;
			parameters[13].Value = model.needTime;
			parameters[14].Value = model.isOver;
			parameters[15].Value = model.ident;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_QuestionAnswer ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


        /// <summary>
        /// 得到每人获奖答题总次数
        /// </summary>
        public int GetAllCounts(int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(*) from tb_QuestionAnswer ");
            strSql.Append(" where usid=usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            //object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return Convert.ToInt32(obj);
            //}
            int count;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    count = reader.GetInt32(0);
                    return count;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 得到每人获奖答对题总次数
        /// </summary>
        public int GetTrueCounts(int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(*) from tb_QuestionAnswer ");
            strSql.Append(" where usid=usid and isTrue=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            //object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return Convert.ToInt32(obj);
            //}
            int count;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    count = reader.GetInt32(0);
                    return count;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 得到每人答错题总次数
        /// </summary>
        public int GetflaseCounts(int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(*) from tb_QuestionAnswer ");
            strSql.Append(" where usid=usid and isTrue=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            //object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return Convert.ToInt32(obj);
            //}
            int count;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    count = reader.GetInt32(0);
                    return count;
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
        public BCW.Model.tb_QuestionAnswer Gettb_QuestionAnswer(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,usid,usname,questID,questtion,answer,isTrue,isHit,isGet,getType,getGold,isDone,addtime,needTime,isOver,ident from tb_QuestionAnswer ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_QuestionAnswer model=new BCW.Model.tb_QuestionAnswer();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.usid = reader.GetInt32(1);
					model.usname = reader.GetString(2);
					model.questID = reader.GetInt32(3);
					model.questtion = reader.GetString(4);
					model.answer = reader.GetString(5);
					model.isTrue = reader.GetInt32(6);
					model.isHit = reader.GetInt32(7);
					model.isGet = reader.GetInt32(8);
					model.getType = reader.GetString(9);
					model.getGold = reader.GetInt64(10);
					model.isDone = reader.GetInt32(11);
					model.addtime = reader.GetDateTime(12);
					model.needTime = reader.GetInt32(13);
					model.isOver = reader.GetInt32(14);
					model.ident = reader.GetInt32(15);
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
			strSql.Append(" FROM tb_QuestionAnswer ");
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
		/// <returns>IList tb_QuestionAnswer</returns>
		public IList<BCW.Model.tb_QuestionAnswer> Gettb_QuestionAnswers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_QuestionAnswer> listtb_QuestionAnswers = new List<BCW.Model.tb_QuestionAnswer>();
			string sTable = "tb_QuestionAnswer";
			string sPkey = "id";
			string sField = "ID,usid,usname,questID,questtion,answer,isTrue,isHit,isGet,getType,getGold,isDone,addtime,needTime,isOver,ident";
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
					return listtb_QuestionAnswers;
				}
				while (reader.Read())
				{
						BCW.Model.tb_QuestionAnswer objtb_QuestionAnswer = new BCW.Model.tb_QuestionAnswer();
						objtb_QuestionAnswer.ID = reader.GetInt32(0);
						objtb_QuestionAnswer.usid = reader.GetInt32(1);
						objtb_QuestionAnswer.usname = reader.GetString(2);
						objtb_QuestionAnswer.questID = reader.GetInt32(3);
						objtb_QuestionAnswer.questtion = reader.GetString(4);
						objtb_QuestionAnswer.answer = reader.GetString(5);
						objtb_QuestionAnswer.isTrue = reader.GetInt32(6);
						objtb_QuestionAnswer.isHit = reader.GetInt32(7);
						objtb_QuestionAnswer.isGet = reader.GetInt32(8);
						objtb_QuestionAnswer.getType = reader.GetString(9);
						objtb_QuestionAnswer.getGold = reader.GetInt64(10);
						objtb_QuestionAnswer.isDone = reader.GetInt32(11);
						objtb_QuestionAnswer.addtime = reader.GetDateTime(12);
						objtb_QuestionAnswer.needTime = reader.GetInt32(13);
						objtb_QuestionAnswer.isOver = reader.GetInt32(14);
						objtb_QuestionAnswer.ident = reader.GetInt32(15);
						listtb_QuestionAnswers.Add(objtb_QuestionAnswer);
				}
			}
			return listtb_QuestionAnswers;
		}

		#endregion  成员方法
	}
}

