using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.JS.DAL
{
	/// <summary>
	/// ���ݷ�����bossrobot��
	/// </summary>
	public class bossrobot
	{
		public bossrobot()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_bossrobot"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_bossrobot");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.JS.Model.bossrobot model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_bossrobot(");
			strSql.Append("GameName,GameID,robotnum,type)");
			strSql.Append(" values (");
			strSql.Append("@GameName,@GameID,@robotnum,@type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GameName", SqlDbType.NVarChar,50),
					new SqlParameter("@GameID", SqlDbType.Int,4),
					new SqlParameter("@robotnum", SqlDbType.Int,4),
					new SqlParameter("@type", SqlDbType.Int,4)};
			parameters[0].Value = model.GameName;
			parameters[1].Value = model.GameID;
			parameters[2].Value = model.robotnum;
			parameters[3].Value = model.type;

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
		public void Update(BCW.JS.Model.bossrobot model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_bossrobot set ");
			strSql.Append("GameName=@GameName,");
			strSql.Append("GameID=@GameID,");
			strSql.Append("robotnum=@robotnum,");
			strSql.Append("type=@type");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GameName", SqlDbType.NVarChar,50),
					new SqlParameter("@GameID", SqlDbType.Int,4),
					new SqlParameter("@robotnum", SqlDbType.Int,4),
					new SqlParameter("@type", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.GameName;
			parameters[2].Value = model.GameID;
			parameters[3].Value = model.robotnum;
			parameters[4].Value = model.type;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_bossrobot ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.JS.Model.bossrobot Getbossrobot(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,GameName,GameID,robotnum,type from tb_bossrobot ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.JS.Model.bossrobot model=new BCW.JS.Model.bossrobot();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.GameName = reader.GetString(1);
					model.GameID = reader.GetInt32(2);
					model.robotnum = reader.GetInt32(3);
					model.type = reader.GetInt32(4);
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
			strSql.Append(" FROM tb_bossrobot ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_bossrobot SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// me_�õ����û�����ID���ܸ���
        /// </summary>
        public int Get_num()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(robotnum) as a FROM tb_bossrobot WHERE type=1");
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                try
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
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///  me_�õ�������Ϸ���ܸ���
        /// </summary>
        public int Get_yx()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as a FROM tb_bossrobot WHERE type=1");
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                try
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
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList bossrobot</returns>
        public IList<BCW.JS.Model.bossrobot> Getbossrobots(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.JS.Model.bossrobot> listbossrobots = new List<BCW.JS.Model.bossrobot>();
			string sTable = "tb_bossrobot";
			string sPkey = "id";
			string sField = "ID,GameName,GameID,robotnum,type";
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
					return listbossrobots;
				}
				while (reader.Read())
				{
						BCW.JS.Model.bossrobot objbossrobot = new BCW.JS.Model.bossrobot();
						objbossrobot.ID = reader.GetInt32(0);
						objbossrobot.GameName = reader.GetString(1);
						objbossrobot.GameID = reader.GetInt32(2);
						objbossrobot.robotnum = reader.GetInt32(3);
						objbossrobot.type = reader.GetInt32(4);
						listbossrobots.Add(objbossrobot);
				}
			}
			return listbossrobots;
		}

		#endregion  ��Ա����
	}
}

