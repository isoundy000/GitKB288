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
	/// 数据访问类BankUser。
	/// </summary>
	public class BankUser
	{
		public BankUser()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_BankUser"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UsID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BankUser");
            strSql.Append(" where UsID=@UsID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsBankName(string BankName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BankUser");
            strSql.Append(" where BankName=@BankName ");
            SqlParameter[] parameters = {
					new SqlParameter("@BankName", SqlDbType.NVarChar,50)};
            parameters[0].Value = BankName;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsZFBName(string ZFBName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BankUser");
            strSql.Append(" where ZFBName=@ZFBName ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZFBName", SqlDbType.NVarChar,50)};
            parameters[0].Value = ZFBName;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.BankUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_BankUser(");
			strSql.Append("UsID,BankName,BankTitle1,BankNo1,BankAdd1,BankTitle2,BankNo2,BankAdd2,BankTitle3,BankNo3,BankAdd3,BankTitle4,BankNo4,BankAdd4,ZFBName,ZFBNo,State)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@BankName,@BankTitle1,@BankNo1,@BankAdd1,@BankTitle2,@BankNo2,@BankAdd2,@BankTitle3,@BankNo3,@BankAdd3,@BankTitle4,@BankNo4,@BankAdd4,@ZFBName,@ZFBNo,@State)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BankName", SqlDbType.NVarChar,50),
					new SqlParameter("@BankTitle1", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo1", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd1", SqlDbType.NVarChar,100),
					new SqlParameter("@BankTitle2", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo2", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd2", SqlDbType.NVarChar,100),
					new SqlParameter("@BankTitle3", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo3", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd3", SqlDbType.NVarChar,100),
					new SqlParameter("@BankTitle4", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo4", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd4", SqlDbType.NVarChar,100),
					new SqlParameter("@ZFBName", SqlDbType.NVarChar,50),
					new SqlParameter("@ZFBNo", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.BankName;
			parameters[2].Value = model.BankTitle1;
			parameters[3].Value = model.BankNo1;
			parameters[4].Value = model.BankAdd1;
			parameters[5].Value = model.BankTitle2;
			parameters[6].Value = model.BankNo2;
			parameters[7].Value = model.BankAdd2;
			parameters[8].Value = model.BankTitle3;
			parameters[9].Value = model.BankNo3;
			parameters[10].Value = model.BankAdd3;
			parameters[11].Value = model.BankTitle4;
			parameters[12].Value = model.BankNo4;
			parameters[13].Value = model.BankAdd4;
			parameters[14].Value = model.ZFBName;
			parameters[15].Value = model.ZFBNo;
			parameters[16].Value = model.State;

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
		public void Update(BCW.Model.BankUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_BankUser set ");
			strSql.Append("BankName=@BankName,");
			strSql.Append("BankTitle1=@BankTitle1,");
			strSql.Append("BankNo1=@BankNo1,");
			strSql.Append("BankAdd1=@BankAdd1,");
			strSql.Append("BankTitle2=@BankTitle2,");
			strSql.Append("BankNo2=@BankNo2,");
			strSql.Append("BankAdd2=@BankAdd2,");
			strSql.Append("BankTitle3=@BankTitle3,");
			strSql.Append("BankNo3=@BankNo3,");
			strSql.Append("BankAdd3=@BankAdd3,");
			strSql.Append("BankTitle4=@BankTitle4,");
			strSql.Append("BankNo4=@BankNo4,");
			strSql.Append("BankAdd4=@BankAdd4,");
			strSql.Append("ZFBName=@ZFBName,");
			strSql.Append("ZFBNo=@ZFBNo,");
			strSql.Append("State=@State");
            strSql.Append(" where UsID=@UsID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BankName", SqlDbType.NVarChar,50),
					new SqlParameter("@BankTitle1", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo1", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd1", SqlDbType.NVarChar,100),
					new SqlParameter("@BankTitle2", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo2", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd2", SqlDbType.NVarChar,100),
					new SqlParameter("@BankTitle3", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo3", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd3", SqlDbType.NVarChar,100),
					new SqlParameter("@BankTitle4", SqlDbType.NVarChar,50),
					new SqlParameter("@BankNo4", SqlDbType.NVarChar,50),
					new SqlParameter("@BankAdd4", SqlDbType.NVarChar,100),
					new SqlParameter("@ZFBName", SqlDbType.NVarChar,50),
					new SqlParameter("@ZFBNo", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.BankName;
			parameters[2].Value = model.BankTitle1;
			parameters[3].Value = model.BankNo1;
			parameters[4].Value = model.BankAdd1;
			parameters[5].Value = model.BankTitle2;
			parameters[6].Value = model.BankNo2;
			parameters[7].Value = model.BankAdd2;
			parameters[8].Value = model.BankTitle3;
			parameters[9].Value = model.BankNo3;
			parameters[10].Value = model.BankAdd3;
			parameters[11].Value = model.BankTitle4;
			parameters[12].Value = model.BankNo4;
			parameters[13].Value = model.BankAdd4;
			parameters[14].Value = model.ZFBName;
			parameters[15].Value = model.ZFBNo;
			parameters[16].Value = model.State;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BankUser ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.BankUser GetBankUser(int UsID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,BankName,BankTitle1,BankNo1,BankAdd1,BankTitle2,BankNo2,BankAdd2,BankTitle3,BankNo3,BankAdd3,BankTitle4,BankNo4,BankAdd4,ZFBName,ZFBNo,State from tb_BankUser ");
            strSql.Append(" where UsID=@UsID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

			BCW.Model.BankUser model=new BCW.Model.BankUser();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.BankName = reader.GetString(2);
					model.BankTitle1 = reader.GetString(3);
					model.BankNo1 = reader.GetString(4);
					model.BankAdd1 = reader.GetString(5);
					model.BankTitle2 = reader.GetString(6);
					model.BankNo2 = reader.GetString(7);
					model.BankAdd2 = reader.GetString(8);
					model.BankTitle3 = reader.GetString(9);
					model.BankNo3 = reader.GetString(10);
					model.BankAdd3 = reader.GetString(11);
					model.BankTitle4 = reader.GetString(12);
					model.BankNo4 = reader.GetString(13);
					model.BankAdd4 = reader.GetString(14);
					model.ZFBName = reader.GetString(15);
					model.ZFBNo = reader.GetString(16);
					model.State = reader.GetInt32(17);
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
			strSql.Append(" FROM tb_BankUser ");
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
		/// <returns>IList BankUser</returns>
		public IList<BCW.Model.BankUser> GetBankUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.BankUser> listBankUsers = new List<BCW.Model.BankUser>();
			string sTable = "tb_BankUser";
			string sPkey = "id";
			string sField = "ID,UsID,BankName,BankTitle1,BankNo1,BankAdd1,BankTitle2,BankNo2,BankAdd2,BankTitle3,BankNo3,BankAdd3,BankTitle4,BankNo4,BankAdd4,ZFBName,ZFBNo,State";
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
					return listBankUsers;
				}
				while (reader.Read())
				{
						BCW.Model.BankUser objBankUser = new BCW.Model.BankUser();
						objBankUser.ID = reader.GetInt32(0);
						objBankUser.UsID = reader.GetInt32(1);
						objBankUser.BankName = reader.GetString(2);
						objBankUser.BankTitle1 = reader.GetString(3);
						objBankUser.BankNo1 = reader.GetString(4);
						objBankUser.BankAdd1 = reader.GetString(5);
						objBankUser.BankTitle2 = reader.GetString(6);
						objBankUser.BankNo2 = reader.GetString(7);
						objBankUser.BankAdd2 = reader.GetString(8);
						objBankUser.BankTitle3 = reader.GetString(9);
						objBankUser.BankNo3 = reader.GetString(10);
						objBankUser.BankAdd3 = reader.GetString(11);
						objBankUser.BankTitle4 = reader.GetString(12);
						objBankUser.BankNo4 = reader.GetString(13);
						objBankUser.BankAdd4 = reader.GetString(14);
						objBankUser.ZFBName = reader.GetString(15);
						objBankUser.ZFBNo = reader.GetString(16);
						objBankUser.State = reader.GetInt32(17);
						listBankUsers.Add(objBankUser);
				}
			}
			return listBankUsers;
		}

		#endregion  成员方法
	}
}

