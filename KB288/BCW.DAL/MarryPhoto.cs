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
	/// ���ݷ�����MarryPhoto��
	/// </summary>
	public class MarryPhoto
	{
		public MarryPhoto()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_MarryPhoto"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_MarryPhoto");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// ����ĳ��԰��Ƭ����
        /// </summary>
        public int GetCount(int MarryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_MarryPhoto");
            strSql.Append(" where MarryId=@MarryId ");
            SqlParameter[] parameters = {
					new SqlParameter("@MarryId", SqlDbType.Int,4)};
            parameters[0].Value = MarryId;

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
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.MarryPhoto model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_MarryPhoto(");
			strSql.Append("MarryId,UsID,UsName,PrevFile,ActFile,Notes,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@MarryId,@UsID,@UsName,@PrevFile,@ActFile,@Notes,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@MarryId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@PrevFile", SqlDbType.NVarChar,100),
					new SqlParameter("@ActFile", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.MarryId;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.PrevFile;
			parameters[4].Value = model.ActFile;
			parameters[5].Value = model.Notes;
			parameters[6].Value = model.AddTime;

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
		public void Update(BCW.Model.MarryPhoto model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_MarryPhoto set ");
			strSql.Append("MarryId=@MarryId,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("PrevFile=@PrevFile,");
			strSql.Append("ActFile=@ActFile,");
			strSql.Append("Notes=@Notes,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@MarryId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@PrevFile", SqlDbType.NVarChar,100),
					new SqlParameter("@ActFile", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.MarryId;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.PrevFile;
			parameters[5].Value = model.ActFile;
			parameters[6].Value = model.Notes;
			parameters[7].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_MarryPhoto ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.MarryPhoto GetMarryPhoto(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,MarryId,UsID,UsName,PrevFile,ActFile,Notes,AddTime from tb_MarryPhoto ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.MarryPhoto model=new BCW.Model.MarryPhoto();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.MarryId = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.UsName = reader.GetString(3);
					model.PrevFile = reader.GetString(4);
					model.ActFile = reader.GetString(5);
					model.Notes = reader.GetString(6);
					model.AddTime = reader.GetDateTime(7);
					return model;
				}
				else
				{
				return null;
				}
			}
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_MarryPhoto ");
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
        /// <returns>IList MarryPhoto</returns>
        public IList<BCW.Model.MarryPhoto> GetMarryPhotos(int SizeNum, string strWhere)
        {
            IList<BCW.Model.MarryPhoto> listMarryPhotos = new List<BCW.Model.MarryPhoto>();
            string sTable = "tb_MarryPhoto";
            string sPkey = "id";
            string sField = "ID,MarryId,PrevFile";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listMarryPhotos;
                }
                while (reader.Read())
                {
                    BCW.Model.MarryPhoto objMarryPhoto = new BCW.Model.MarryPhoto();
                    objMarryPhoto.ID = reader.GetInt32(0);
                    objMarryPhoto.MarryId = reader.GetInt32(1);
                    objMarryPhoto.PrevFile = reader.GetString(2);
                    listMarryPhotos.Add(objMarryPhoto);
                }
            }
            return listMarryPhotos;
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList MarryPhoto</returns>
		public IList<BCW.Model.MarryPhoto> GetMarryPhotos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.MarryPhoto> listMarryPhotos = new List<BCW.Model.MarryPhoto>();
			string sTable = "tb_MarryPhoto";
			string sPkey = "id";
			string sField = "ID,MarryId,UsID,UsName,PrevFile,ActFile,Notes,AddTime";
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
					return listMarryPhotos;
				}
				while (reader.Read())
				{
						BCW.Model.MarryPhoto objMarryPhoto = new BCW.Model.MarryPhoto();
						objMarryPhoto.ID = reader.GetInt32(0);
						objMarryPhoto.MarryId = reader.GetInt32(1);
						objMarryPhoto.UsID = reader.GetInt32(2);
						objMarryPhoto.UsName = reader.GetString(3);
						objMarryPhoto.PrevFile = reader.GetString(4);
						objMarryPhoto.ActFile = reader.GetString(5);
						objMarryPhoto.Notes = reader.GetString(6);
						objMarryPhoto.AddTime = reader.GetDateTime(7);
						listMarryPhotos.Add(objMarryPhoto);
				}
			}
			return listMarryPhotos;
		}

		#endregion  ��Ա����
	}
}

