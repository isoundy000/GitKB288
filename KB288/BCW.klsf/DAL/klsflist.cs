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
	/// ���ݷ�����klsflist��
	/// </summary>
	public class klsflist
	{
		public klsflist()
		{}
		#region  ��Ա����
        // <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_klsflist");
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_klsflist");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// �Ƿ���ڸ���
        /// </summary>
        public bool ExistsklsfId(int klsfId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_klsflist");
            strSql.Append(" where klsfId=@klsfId ");
            SqlParameter[] parameters = {
					new SqlParameter("@klsfId", SqlDbType.Int,4)};
            parameters[0].Value = klsfId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ����Ҫ���½���ļ�¼
        /// </summary>
        public bool ExistsUpdateResult()
        {
            int Sec = Utils.ParseInt(ub.GetSub("klsfSec", "/Controls/klsf.xml"));

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_klsflist");
            strSql.Append(" where State=0 and EndTime<'" + DateTime.Now.AddSeconds(Sec) + "'");

            return SqlHelper.Exists(strSql.ToString());
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.klsflist model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_klsflist(");
			strSql.Append("klsfId,Result,State,EndTime)");
			strSql.Append(" values (");
			strSql.Append("@klsfId,@Result,@State,@EndTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@klsfId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
			parameters[0].Value = model.klsfId;
			parameters[1].Value = model.Result;
			parameters[2].Value = model.State;
			parameters[3].Value = model.EndTime;

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
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.klsflist model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_klsflist set ");
			strSql.Append("klsfId=@klsfId,");
			strSql.Append("Result=@Result,");
			strSql.Append("State=@State,");
			strSql.Append("EndTime=@EndTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@klsfId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.klsfId;
			parameters[2].Value = model.Result;
			parameters[3].Value = model.State;
			parameters[4].Value = model.EndTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
		/// ���¿�������
		/// </summary>
        public void UpdateResult(string klsfId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_klsflist set ");
            strSql.Append("Result=@Result,");
            strSql.Append("State=1");
            strSql.Append(" where klsfId=@klsfId and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@klsfId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = klsfId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_klsflist ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        // me_��ʼ��ĳ���ݱ�
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.klsflist Getklsflist(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,klsfId,Result,State,EndTime from tb_klsflist ");
            strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.klsflist model=new BCW.Model.klsflist();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.klsfId = reader.GetInt32(1);
					model.Result = reader.GetString(2);
					model.State = reader.GetInt32(3);
					model.EndTime = reader.GetDateTime(4);
					return model;
				}
				else
				{
				return null;
				}
			}
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.klsflist Getklsflistbyklsfid(int klsfId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,klsfId,Result,State,EndTime from tb_klsflist ");
            strSql.Append(" where klsfId=@klsfId");
            SqlParameter[] parameters = {
					new SqlParameter("@klsfId", SqlDbType.Int,4)};
            parameters[0].Value = klsfId;

            BCW.Model.klsflist model = new BCW.Model.klsflist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.klsfId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
                    model.State = reader.GetInt32(3);
                    model.EndTime = reader.GetDateTime(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ����һ�ڵĶ���ʵ��
        /// </summary>
        public BCW.Model.klsflist GetklsflistLast()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,klsfId,Result,State,EndTime from tb_klsflist");
            strSql.Append(" Order by id desc ");

            BCW.Model.klsflist model = new BCW.Model.klsflist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.klsfId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
                    model.State = reader.GetInt32(3);
                    model.EndTime = reader.GetDateTime(4);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.klsfId = 0;
                    model.Result = "";
                    model.State = 0;
                    model.EndTime = DateTime.Now;
                    return model;
                }
            }
        }

        /// <summary>
        /// �õ����ڿ���
        /// </summary>
        public BCW.Model.klsflist GetklsflistLast2()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,klsfId,Result from tb_klsflist where State=1");
            strSql.Append(" Order by id desc ");

            BCW.Model.klsflist model = new BCW.Model.klsflist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.klsfId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// �����ںŵõ�id
        /// </summary>
        public int id(int klsfId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id  from tb_klsflist where klsfId=" + klsfId);
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_klsflist ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSClist</returns>
        public IList<BCW.Model.klsflist> Getklsflists(int SizeNum, string strWhere)
        {
            IList<BCW.Model.klsflist> listklsflists = new List<BCW.Model.klsflist>();
            string sTable = "tb_klsflist";
            string sPkey = "id";
            string sField = "ID,klsfId,Result,State,EndTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listklsflists;
                }
                while (reader.Read())
                {
                    BCW.Model.klsflist objklsflist = new BCW.Model.klsflist();
                    objklsflist.ID = reader.GetInt32(0);
                    objklsflist.klsfId = reader.GetInt32(1);
                    objklsflist.Result = reader.GetString(2);
                    objklsflist.State = reader.GetInt32(3);
                    objklsflist.EndTime = reader.GetDateTime(4);
                    listklsflists.Add(objklsflist);
                }
            }
            return listklsflists;
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList klsflist</returns>
		public IList<BCW.Model.klsflist> Getklsflists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.klsflist> listklsflists = new List<BCW.Model.klsflist>();
			string sTable = "tb_klsflist";
			string sPkey = "id";
			string sField = "ID,klsfId,Result,State,EndTime";
			string sCondition = strWhere;
			string sOrder = "ID Desc";
			int iSCounts = 0;
			using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
			{
				//������ҳ��
				if (p_recordCount > 0)
				{
					int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
				}
				else
				{
					return listklsflists;
				}
				while (reader.Read())
				{
						BCW.Model.klsflist objklsflist = new BCW.Model.klsflist();
						objklsflist.ID = reader.GetInt32(0);
						objklsflist.klsfId = reader.GetInt32(1);
						objklsflist.Result = reader.GetString(2);
                        objklsflist.State = reader.GetInt32(3);
						objklsflist.EndTime = reader.GetDateTime(4);
						listklsflists.Add(objklsflist);
				}
			}
			return listklsflists;
		}

		#endregion  ��Ա����
	}
}

