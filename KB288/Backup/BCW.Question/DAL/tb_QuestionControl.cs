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
	/// 数据访问类tb_QuestionControl。
	/// </summary>
	public class tb_QuestionControl
	{
		public tb_QuestionControl()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_QuestionControl");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_QuestionControl model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_QuestionControl(");
			strSql.Append("usid,usname,qList,qCount,qNow,isTrue,isFalse,answerList,answerJudge,addtime,overtime,isOver,isDone,remark)");
			strSql.Append(" values (");
			strSql.Append("@usid,@usname,@qList,@qCount,@qNow,@isTrue,@isFalse,@answerList,@answerJudge,@addtime,@overtime,@isOver,@isDone,@remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@usname", SqlDbType.NVarChar,50),
					new SqlParameter("@qList", SqlDbType.NVarChar),
					new SqlParameter("@qCount", SqlDbType.Int,4),
					new SqlParameter("@qNow", SqlDbType.Int,4),
					new SqlParameter("@isTrue", SqlDbType.Int,4),
					new SqlParameter("@isFalse", SqlDbType.Int,4),
					new SqlParameter("@answerList", SqlDbType.NVarChar),
					new SqlParameter("@answerJudge", SqlDbType.NVarChar,200),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime),
					new SqlParameter("@isOver", SqlDbType.Int,4),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.usid;
			parameters[1].Value = model.usname;
			parameters[2].Value = model.qList;
			parameters[3].Value = model.qCount;
			parameters[4].Value = model.qNow;
			parameters[5].Value = model.isTrue;
			parameters[6].Value = model.isFalse;
			parameters[7].Value = model.answerList;
			parameters[8].Value = model.answerJudge;
			parameters[9].Value = model.addtime;
			parameters[10].Value = model.overtime;
			parameters[11].Value = model.isOver;
			parameters[12].Value = model.isDone;
			parameters[13].Value = model.remark;

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
        ///更新isFalse
        /// </summary>
        public void UpdateIsFlase(int ID, int isFalse)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuestionControl set ");
            strSql.Append("isFalse=@isFalse ");
            strSql.Append("where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@isFalse", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = isFalse;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        ///更新isTrue
        /// </summary>
        public void UpdateIsTrue(int ID, int isTrue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuestionControl set ");
            strSql.Append("isTrue=@isTrue ");
            strSql.Append("where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@isTrue", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = isTrue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///更新judge
        /// </summary>
        //public void UpdateJudge(int ID, string answerJudge)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update tb_QuestionControl set ");
        //    strSql.Append("answerJudge=@answerJudge ");
        //    strSql.Append("where ID=@ID ");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4),
        //            new SqlParameter("@answerJudge", SqlDbType.NVarChar)};
        //    parameters[0].Value = ID;
        //    parameters[1].Value = answerJudge;
        //    SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        //}


        /// <summary>
        ///更新isDone
        /// </summary>
        public void UpdateIsDone(int ID, int isDone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuestionControl set ");
            strSql.Append("isDone=@isDone ");
            strSql.Append("where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = isDone;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///更新answerList
        /// </summary>
        public void UpdateAnswerList(int ID, string answerList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuestionControl set ");
            strSql.Append("answerList=@answerList ");
            strSql.Append("where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@answerList", SqlDbType.NVarChar)};
            parameters[0].Value = ID;
            parameters[1].Value = answerList;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        ///更新qNow
        /// </summary>
        public void UpdateqNow(int ID, int qNow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuestionControl set ");
            strSql.Append("qNow=@qNow ");
            strSql.Append("where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@qNow", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = qNow;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_QuestionControl model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_QuestionControl set ");
			strSql.Append("usid=@usid,");
			strSql.Append("usname=@usname,");
			strSql.Append("qList=@qList,");
			strSql.Append("qCount=@qCount,");
			strSql.Append("qNow=@qNow,");
			strSql.Append("isTrue=@isTrue,");
			strSql.Append("isFalse=@isFalse,");
			strSql.Append("answerList=@answerList,");
			strSql.Append("answerJudge=@answerJudge,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("overtime=@overtime,");
			strSql.Append("isOver=@isOver,");
			strSql.Append("isDone=@isDone,");
			strSql.Append("remark=@remark");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@usname", SqlDbType.NVarChar,50),
					new SqlParameter("@qList", SqlDbType.NVarChar),
					new SqlParameter("@qCount", SqlDbType.Int,4),
					new SqlParameter("@qNow", SqlDbType.Int,4),
					new SqlParameter("@isTrue", SqlDbType.Int,4),
					new SqlParameter("@isFalse", SqlDbType.Int,4),
					new SqlParameter("@answerList", SqlDbType.NVarChar),
					new SqlParameter("@answerJudge", SqlDbType.NVarChar,200),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime),
					new SqlParameter("@isOver", SqlDbType.Int,4),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.usid;
			parameters[2].Value = model.usname;
			parameters[3].Value = model.qList;
			parameters[4].Value = model.qCount;
			parameters[5].Value = model.qNow;
			parameters[6].Value = model.isTrue;
			parameters[7].Value = model.isFalse;
			parameters[8].Value = model.answerList;
			parameters[9].Value = model.answerJudge;
			parameters[10].Value = model.addtime;
			parameters[11].Value = model.overtime;
			parameters[12].Value = model.isOver;
			parameters[13].Value = model.isDone;
			parameters[14].Value = model.remark;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_QuestionControl ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 得到uid最新记录
        /// 返回ID
        /// </summary>
        public int GetIDForUsid(int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 ID from tb_QuestionControl ");
            strSql.Append(" where usid=@usid order by ID desc ");
            SqlParameter[] parameters = {
                 new SqlParameter("@usid", SqlDbType.Int)};
            parameters[0].Value = usid;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 5;
                }
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionControl Gettb_QuestionControl(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,usid,usname,qList,qCount,qNow,isTrue,isFalse,answerList,answerJudge,addtime,overtime,isOver,isDone,remark from tb_QuestionControl ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_QuestionControl model=new BCW.Model.tb_QuestionControl();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.usid = reader.GetInt32(1);
					model.usname = reader.GetString(2);
					model.qList = reader.GetString(3);
					model.qCount = reader.GetInt32(4);
					model.qNow = reader.GetInt32(5);
					model.isTrue = reader.GetInt32(6);
					model.isFalse = reader.GetInt32(7);
					model.answerList = reader.GetString(8);
					model.answerJudge = reader.GetString(9);
					model.addtime = reader.GetDateTime(10);
					model.overtime = reader.GetDateTime(11);
					model.isOver = reader.GetInt32(12);
					model.isDone = reader.GetInt32(13);
					model.remark = reader.GetString(14);
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
			strSql.Append(" FROM tb_QuestionControl ");
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
		/// <returns>IList tb_QuestionControl</returns>
		public IList<BCW.Model.tb_QuestionControl> Gettb_QuestionControls(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_QuestionControl> listtb_QuestionControls = new List<BCW.Model.tb_QuestionControl>();
			string sTable = "tb_QuestionControl";
			string sPkey = "id";
			string sField = "ID,usid,usname,qList,qCount,qNow,isTrue,isFalse,answerList,answerJudge,addtime,overtime,isOver,isDone,remark";
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
					return listtb_QuestionControls;
				}
				while (reader.Read())
				{
						BCW.Model.tb_QuestionControl objtb_QuestionControl = new BCW.Model.tb_QuestionControl();
						objtb_QuestionControl.ID = reader.GetInt32(0);
						objtb_QuestionControl.usid = reader.GetInt32(1);
						objtb_QuestionControl.usname = reader.GetString(2);
						objtb_QuestionControl.qList = reader.GetString(3);
						objtb_QuestionControl.qCount = reader.GetInt32(4);
						objtb_QuestionControl.qNow = reader.GetInt32(5);
						objtb_QuestionControl.isTrue = reader.GetInt32(6);
						objtb_QuestionControl.isFalse = reader.GetInt32(7);
						objtb_QuestionControl.answerList = reader.GetString(8);
						objtb_QuestionControl.answerJudge = reader.GetString(9);
						objtb_QuestionControl.addtime = reader.GetDateTime(10);
						objtb_QuestionControl.overtime = reader.GetDateTime(11);
						objtb_QuestionControl.isOver = reader.GetInt32(12);
						objtb_QuestionControl.isDone = reader.GetInt32(13);
						objtb_QuestionControl.remark = reader.GetString(14);
						listtb_QuestionControls.Add(objtb_QuestionControl);
				}
			}
			return listtb_QuestionControls;
		}

		#endregion  成员方法
	}
}

