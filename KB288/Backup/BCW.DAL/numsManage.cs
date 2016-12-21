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
	/// 数据访问类tb_numsManage。
	/// </summary>
	public class numsManage
	{
		public numsManage()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_numsManage");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool ExistsByUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_numsManage");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.numsManage model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_numsManage(");
			strSql.Append("loginTime,UsID,Pwd,Question,answer)");
			strSql.Append(" values (");
			strSql.Append("@loginTime,@UsID,@Pwd,@Question,@answer)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@loginTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Pwd", SqlDbType.VarChar,20),
                    new SqlParameter("@Question", SqlDbType.VarChar,20),
                    new SqlParameter("@answer", SqlDbType.VarChar,20)
            };
			parameters[0].Value = model.loginTime;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.Pwd;
            parameters[3].Value = model.Question;
            parameters[4].Value = model.answer;

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
		public void Update(BCW.Model.numsManage model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_numsManage set ");
			strSql.Append("loginTime=@loginTime,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Pwd=@Pwd");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@loginTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Pwd", SqlDbType.VarChar,20)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.loginTime;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.Pwd;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
		/// 更新一条数据
		/// </summary>
		public void UpdateByUI(BCW.Model.numsManage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_numsManage set ");
            strSql.Append("loginTime=@loginTime,");
            strSql.Append("Question=@Question,");
            strSql.Append("answer=@answer,");
            strSql.Append("Pwd=@Pwd");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                new SqlParameter("@loginTime", SqlDbType.DateTime),
                new SqlParameter("@Question", SqlDbType.VarChar,20),
                new SqlParameter("@answer", SqlDbType.VarChar,20),
                new SqlParameter("@Pwd", SqlDbType.VarChar,20),
                new SqlParameter("@UsID",SqlDbType.Int,4)};
            parameters[0].Value = model.loginTime;
            parameters[1].Value = model.Question;
            parameters[2].Value = model.answer;
            parameters[3].Value = model.Pwd;
            parameters[4].Value = model.UsID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from numsManage ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 更新管理密码
        /// </summary>
        public void UpdatePwd(int UsID,string Pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_numsManage set ");
            strSql.Append("Pwd=@Pwd");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                new SqlParameter("@UsID", SqlDbType.Int,4),
                new SqlParameter("@Pwd", SqlDbType.VarChar,20)};
            parameters[0].Value = UsID;
            parameters[1].Value = Pwd;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新在线时间
        /// </summary>
        public void UpdateTime(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_numsManage set ");
            strSql.Append("loginTime=@loginTime ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@loginTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = DateTime.Now;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.numsManage GetByUsID(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,loginTime,UsID,Pwd,Question,answer from tb_numsManage ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            BCW.Model.numsManage model = new BCW.Model.numsManage();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.loginTime = reader.GetDateTime(1);
                    model.UsID = reader.GetInt32(2);
                    model.Pwd = reader.GetString(3);
                    model.Question = reader.GetString(4);
                    model.answer = reader.GetString(5);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.numsManage Gettb_numsManage(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,loginTime,UsID,Pwd from tb_numsManage ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.numsManage model =new BCW.Model.numsManage();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.loginTime = reader.GetDateTime(1);
					model.UsID = reader.GetInt32(2);
					model.Pwd = reader.GetString(3);
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
			strSql.Append(" FROM tb_numsManage ");
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
		/// <returns>IList tb_numsManage</returns>
		public IList<BCW.Model.numsManage> Gettb_numsManages(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.numsManage> listtb_numsManages = new List<BCW.Model.numsManage>();
			string sTable = "tb_numsManage";
			string sPkey = "id";
			string sField = "ID,loginTime,UsID,Pwd";
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
					return listtb_numsManages;
				}
				while (reader.Read())
				{
						BCW.Model.numsManage objtb_numsManage = new BCW.Model.numsManage();
						objtb_numsManage.ID = reader.GetInt32(0);
						objtb_numsManage.loginTime = reader.GetDateTime(1);
						objtb_numsManage.UsID = reader.GetInt32(2);
						objtb_numsManage.Pwd = reader.GetString(3);
						listtb_numsManages.Add(objtb_numsManage);
				}
			}
			return listtb_numsManages;
		}

		#endregion  成员方法
	}
}

