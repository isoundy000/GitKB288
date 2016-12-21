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
	/// 数据访问类Contact。
	/// </summary>
	public class Contact
	{
		public Contact()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Contact"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Contact");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Contact");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        public bool Exists2(int NodeId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Contact");
            strSql.Append(" where NodeId=@NodeId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = NodeId;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某分组联系人数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Contact");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
		public int Add(BCW.Model.Contact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Contact(");
			strSql.Append("NodeId,UsID,Name,Mobile,HomePhone,OfficePhone,Fax,Email,Company,Posit,Content)");
			strSql.Append(" values (");
			strSql.Append("@NodeId,@UsID,@Name,@Mobile,@HomePhone,@OfficePhone,@Fax,@Email,@Company,@Posit,@Content)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@HomePhone", SqlDbType.NVarChar,50),
					new SqlParameter("@OfficePhone", SqlDbType.NVarChar,50),
					new SqlParameter("@Fax", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@Company", SqlDbType.NVarChar,50),
					new SqlParameter("@Posit", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.NodeId;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.Name;
			parameters[3].Value = model.Mobile;
			parameters[4].Value = model.HomePhone;
			parameters[5].Value = model.OfficePhone;
			parameters[6].Value = model.Fax;
			parameters[7].Value = model.Email;
			parameters[8].Value = model.Company;
			parameters[9].Value = model.Posit;
			parameters[10].Value = model.Content;

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
		public void Update(BCW.Model.Contact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Contact set ");
			strSql.Append("NodeId=@NodeId,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Name=@Name,");
			strSql.Append("Mobile=@Mobile,");
			strSql.Append("HomePhone=@HomePhone,");
			strSql.Append("OfficePhone=@OfficePhone,");
			strSql.Append("Fax=@Fax,");
			strSql.Append("Email=@Email,");
			strSql.Append("Company=@Company,");
			strSql.Append("Posit=@Posit,");
			strSql.Append("Content=@Content");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@HomePhone", SqlDbType.NVarChar,50),
					new SqlParameter("@OfficePhone", SqlDbType.NVarChar,50),
					new SqlParameter("@Fax", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@Company", SqlDbType.NVarChar,50),
					new SqlParameter("@Posit", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.NodeId;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.Name;
			parameters[4].Value = model.Mobile;
			parameters[5].Value = model.HomePhone;
			parameters[6].Value = model.OfficePhone;
			parameters[7].Value = model.Fax;
			parameters[8].Value = model.Email;
			parameters[9].Value = model.Company;
			parameters[10].Value = model.Posit;
			parameters[11].Value = model.Content;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Contact ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 得到NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 NodeId from tb_Contact ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
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
		public BCW.Model.Contact GetContact(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,NodeId,UsID,Name,Mobile,HomePhone,OfficePhone,Fax,Email,Company,Posit,Content from tb_Contact ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Contact model=new BCW.Model.Contact();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.NodeId = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.Name = reader.GetString(3);
					model.Mobile = reader.GetString(4);
					model.HomePhone = reader.GetString(5);
					model.OfficePhone = reader.GetString(6);
					model.Fax = reader.GetString(7);
					model.Email = reader.GetString(8);
					model.Company = reader.GetString(9);
					model.Posit = reader.GetString(10);
					model.Content = reader.GetString(11);
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
			strSql.Append(" FROM tb_Contact ");
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
		/// <returns>IList Contact</returns>
		public IList<BCW.Model.Contact> GetContacts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Contact> listContacts = new List<BCW.Model.Contact>();
			string sTable = "tb_Contact";
			string sPkey = "id";
			string sField = "ID,NodeId,UsID,Name,Mobile,HomePhone,OfficePhone,Fax,Email,Company,Posit,Content";
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
					return listContacts;
				}
				while (reader.Read())
				{
						BCW.Model.Contact objContact = new BCW.Model.Contact();
						objContact.ID = reader.GetInt32(0);
						objContact.NodeId = reader.GetInt32(1);
						objContact.UsID = reader.GetInt32(2);
						objContact.Name = reader.GetString(3);
						objContact.Mobile = reader.GetString(4);
						objContact.HomePhone = reader.GetString(5);
						objContact.OfficePhone = reader.GetString(6);
						objContact.Fax = reader.GetString(7);
						objContact.Email = reader.GetString(8);
						objContact.Company = reader.GetString(9);
						objContact.Posit = reader.GetString(10);
						objContact.Content = reader.GetString(11);
						listContacts.Add(objContact);
				}
			}
			return listContacts;
		}

		#endregion  成员方法
	}
}

