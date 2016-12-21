using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
    /// <summary>
    /// ���ݷ�����Bslist��
    /// </summary>
    public class Bslist
    {
        public Bslist()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Bslist");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Bslist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Bslist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Bslist(");
            strSql.Append("Title,Money,SmallPay,BigPay,UsID,UsName,Click,BzType,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Title,@Money,@SmallPay,@BigPay,@UsID,@UsName,@Click,@BzType,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Money", SqlDbType.BigInt,8),
					new SqlParameter("@SmallPay", SqlDbType.BigInt,8),
					new SqlParameter("@BigPay", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Money;
            parameters[2].Value = model.SmallPay;
            parameters[3].Value = model.BigPay;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Click;
            parameters[7].Value = model.BzType;
            parameters[8].Value = model.AddTime;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        public void Update(BCW.Model.Game.Bslist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Title=@Title,");
            strSql.Append("SmallPay=@SmallPay,");
            strSql.Append("BigPay=@BigPay,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Click=@Click,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SmallPay", SqlDbType.BigInt,8),
					new SqlParameter("@BigPay", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.SmallPay;
            parameters[3].Value = model.BigPay;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Click;
            parameters[7].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateBasic(BCW.Model.Game.Bslist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Title=@Title,");
            strSql.Append("SmallPay=@SmallPay,");
            strSql.Append("BigPay=@BigPay,");
            strSql.Append("UsName=@UsName");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SmallPay", SqlDbType.BigInt,8),
					new SqlParameter("@BigPay", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.SmallPay;
            parameters[3].Value = model.BigPay;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMoney(int ID, long Money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Money=Money+@Money");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Money", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = Money;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateClick(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Click=Click+@Click");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Bslist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Bslist GetBslist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,Money,SmallPay,BigPay,UsID,UsName,Click,BzType,AddTime from tb_Bslist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Bslist model = new BCW.Model.Game.Bslist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.Money = reader.GetInt64(2);
                    model.SmallPay = reader.GetInt64(3);
                    model.BigPay = reader.GetInt64(4);
                    model.UsID = reader.GetInt32(5);
                    model.UsName = reader.GetString(6);
                    model.Click = reader.GetInt32(7);
                    model.BzType = reader.GetByte(8);
                    model.AddTime = reader.GetDateTime(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ�һ��GetID
        /// </summary>
        public int GetID(int UsID, int BzType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID from tb_Bslist ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and BzType=@BzType ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.Int,4)};

            parameters[0].Value = UsID;
            parameters[1].Value = BzType;

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
        /// �õ�һ��Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title from tb_Bslist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};

            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Bslist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Bslist</returns>
        public IList<BCW.Model.Game.Bslist> GetBslists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Game.Bslist> listBslists = new List<BCW.Model.Game.Bslist>();
            string sTable = "tb_Bslist";
            string sPkey = "id";
            string sField = "ID,Title,Money,Click,BzType,SmallPay,BigPay";
            string sCondition = strWhere;
            string sOrder = strOrder;
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
                    return listBslists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Bslist objBslist = new BCW.Model.Game.Bslist();
                    objBslist.ID = reader.GetInt32(0);
                    objBslist.Title = reader.GetString(1);
                    objBslist.Money = reader.GetInt64(2);
                    objBslist.Click = reader.GetInt32(3);
                    objBslist.BzType = reader.GetByte(4);
                    objBslist.SmallPay = reader.GetInt64(5);
                    objBslist.BigPay = reader.GetInt64(6);
                    listBslists.Add(objBslist);
                }
            }
            return listBslists;
        }

        #endregion  ��Ա����
    }
}
