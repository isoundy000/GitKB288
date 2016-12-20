using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Baccarat.DAL
{
	/// <summary>
	/// ���ݷ�����BaccaratTop��
	/// </summary>
	public class BaccaratTop
	{
		public BaccaratTop()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("TopID", "tb_BaccaratTop"); 
		}

        /// <summary>
        /// ������¼
        /// </summary>
        /// <param name="TableName"></param>
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
        public bool Exists(int TopID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratTop");
            strSql.Append(" where TopID=@TopID");
            SqlParameter[] parameters = {
					new SqlParameter("@TopID", SqlDbType.Int,4)
			};
            parameters[0].Value = TopID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ����ĳ�û�������
        /// </summary>
        public bool ExistsUser(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratTop");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


		/// <summary>
		/// ����һ������
		/// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratTop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaccaratTop(");
            strSql.Append("UsID,UsName,Topdate,TopBonusSum,RoomID,RoomTable)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@Topdate,@TopBonusSum,@RoomID,@RoomTable)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Topdate", SqlDbType.DateTime),
					new SqlParameter("@TopBonusSum", SqlDbType.Int,4),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomTable", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Topdate;
            parameters[3].Value = model.TopBonusSum;
            parameters[4].Value = model.RoomID;
            parameters[5].Value = model.RoomTable;

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
        public bool Update(BCW.Baccarat.Model.BaccaratTop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratTop set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Topdate=@Topdate,");
            strSql.Append("TopBonusSum=@TopBonusSum,");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomTable=@RoomTable");
            strSql.Append(" where TopID=@TopID");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Topdate", SqlDbType.DateTime),
					new SqlParameter("@TopBonusSum", SqlDbType.Int,4),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomTable", SqlDbType.Int,4),
					new SqlParameter("@TopID", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Topdate;
            parameters[3].Value = model.TopBonusSum;
            parameters[4].Value = model.RoomID;
            parameters[5].Value = model.RoomTable;
            parameters[6].Value = model.TopID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ����ĳһ�����ĳһ�ض��ֶ�
        /// </summary>
        public void UpdateTop(int UsID, int TopBonusSum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratTop set ");
            strSql.Append("TopBonusSum=TopBonusSum+@TopBonusSum and Topdate=getdate() ");
            strSql.Append("where UsID=@UsID");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@TopBonusSum", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = TopBonusSum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int TopID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratTop ");
            strSql.Append(" where TopID=@TopID");
            SqlParameter[] parameters = {
					new SqlParameter("@TopID", SqlDbType.Int,4)
			};
            parameters[0].Value = TopID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ����ɾ������
        /// </summary>
        public bool DeleteList(string TopIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratTop ");
            strSql.Append(" where TopID in (" + TopIDlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public BCW.Baccarat.Model.BaccaratTop GetModel(int TopID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TopID,UsID,UsName,Topdate,TopBonusSum,RoomID,RoomTable from tb_BaccaratTop ");
            strSql.Append(" where TopID=@TopID");
            SqlParameter[] parameters = {
					new SqlParameter("@TopID", SqlDbType.Int,4)
			};
            parameters[0].Value = TopID;

            BCW.Baccarat.Model.BaccaratTop model = new BCW.Baccarat.Model.BaccaratTop();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratTop DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratTop model = new BCW.Baccarat.Model.BaccaratTop();
            if (row != null)
            {
                if (row["TopID"] != null && row["TopID"].ToString() != "")
                {
                    model.TopID = int.Parse(row["TopID"].ToString());
                }
                if (row["UsID"] != null && row["UsID"].ToString() != "")
                {
                    model.UsID = int.Parse(row["UsID"].ToString());
                }
                if (row["UsName"] != null)
                {
                    model.UsName = row["UsName"].ToString();
                }
                if (row["Topdate"] != null && row["Topdate"].ToString() != "")
                {
                    model.Topdate = DateTime.Parse(row["Topdate"].ToString());
                }
                if (row["TopBonusSum"] != null && row["TopBonusSum"].ToString() != "")
                {
                    model.TopBonusSum = int.Parse(row["TopBonusSum"].ToString());
                }
                if (row["RoomID"] != null && row["RoomID"].ToString() != "")
                {
                    model.RoomID = int.Parse(row["RoomID"].ToString());
                }
                if (row["RoomTable"] != null && row["RoomTable"].ToString() != "")
                {
                    model.RoomTable = int.Parse(row["RoomTable"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TopID,UsID,UsName,Topdate,TopBonusSum,RoomID,RoomTable ");
            strSql.Append(" FROM tb_BaccaratTop ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" TopID,UsID,UsName,Topdate,TopBonusSum,RoomID,RoomTable ");
            strSql.Append(" FROM tb_BaccaratTop ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// ��ȡ��¼����
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_BaccaratTop ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// ��ҳ��ȡ�����б�
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.TopID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_BaccaratTop T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_BaccaratTop ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// ��ȡ���м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns></returns>
        public IList<BCW.Baccarat.Model.BaccaratTop> GetUserTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BaccaratTop> listUserTop = new List<BCW.Baccarat.Model.BaccaratTop>();

            // �����û�ID����
            string countString = "select COUNT(distinct UsID) from tb_BaccaratTop where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 100)
                p_recordCount = 100;
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listUserTop;
            }

            //ȡ����ؼ�¼
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT UsID,SUM(TopBonusSum) as aa FROM tb_BaccaratTop where ");
            strSql.Append(strWhere + " group by UsID order by aa DESC");
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Baccarat.Model.BaccaratTop objbjlPay = new BCW.Baccarat.Model.BaccaratTop();
                        objbjlPay.UsID = reader.GetInt32(0);
                        objbjlPay.aa = reader.GetInt32(1);
                        listUserTop.Add(objbjlPay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listUserTop;
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList BaccaratTop</returns>
		public IList<BCW.Baccarat.Model.BaccaratTop> GetBaccaratTops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratTop> listBaccaratTops = new List<BCW.Baccarat.Model.BaccaratTop>();
			string sTable = "tb_BaccaratTop";
			string sPkey = "id";
			string sField = "*";
			string sCondition = strWhere;
            string sOrder = "TopBonusSum Desc";
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
					return listBaccaratTops;
				}
				while (reader.Read())
				{
						BCW.Baccarat.Model.BaccaratTop objBaccaratTop = new BCW.Baccarat.Model.BaccaratTop();
						objBaccaratTop.TopID = reader.GetInt32(0);
						objBaccaratTop.UsID = reader.GetInt32(1);
						objBaccaratTop.UsName = reader.GetString(2);
						objBaccaratTop.Topdate = reader.GetDateTime(3);
						objBaccaratTop.TopBonusSum = reader.GetInt32(4);
                        objBaccaratTop.RoomID = reader.GetInt32(4);
                        objBaccaratTop.RoomTable=reader.GetInt32(4);
						listBaccaratTops.Add(objBaccaratTop);
				}
			}
			return listBaccaratTops;
		}

		#endregion  ��Ա����
	}
}

