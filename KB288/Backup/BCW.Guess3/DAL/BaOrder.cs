using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Common;
using BCW.Data;
namespace TPR3.DAL.guess
{
	/// <summary>
	/// ���ݷ�����BaOrder��
	/// </summary>
	public class BaOrder
	{
		public BaOrder()
		{}
		#region  ��Ա����

        /// <summary>
        /// �������а�
        /// </summary>
        /// <param name="iUsid"></param>
        /// <param name="iCent"></param>
        public void UpdateOrder(TPR3.Model.guess.BaOrder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID ");
            strSql.Append(" FROM tb_ZBaOrder ");
            strSql.Append(" where Orderusid=@Orderusid");
            SqlParameter[] parameters = {
					new SqlParameter("@Orderusid", SqlDbType.Int,4)};
            parameters[0].Value = model.Orderusid;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                Add(model);
            }
            else
            {
                Update(model);
            }

        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(TPR3.Model.guess.BaOrder model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_ZBaOrder(");
            strSql.Append("Orderusid,Orderusname,Orderbanum,Orderfanum,Orderjbnum,Orderstats)");
			strSql.Append(" values (");
            strSql.Append("@Orderusid,@Orderusname,@Orderbanum,@Orderfanum,@Orderjbnum,@Orderstats)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Orderusid", SqlDbType.Int,4),
					new SqlParameter("@Orderusname", SqlDbType.NVarChar,50),
					new SqlParameter("@Orderbanum", SqlDbType.Money,8),
                    new SqlParameter("@Orderfanum", SqlDbType.Money,8),
					new SqlParameter("@Orderjbnum", SqlDbType.Money,8),
					new SqlParameter("@Orderstats", SqlDbType.Int,4)};
			parameters[0].Value = model.Orderusid;
            parameters[1].Value = model.Orderusname;
			parameters[2].Value = model.Orderbanum;
            parameters[3].Value = model.Orderfanum;
			parameters[4].Value = model.Orderjbnum;
			parameters[5].Value = model.Orderstats;

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
		public void Update(TPR3.Model.guess.BaOrder model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_ZBaOrder set ");
            strSql.Append("Orderusname=@Orderusname,");
            strSql.Append("Orderbanum=Orderbanum+@Orderbanum,");
            strSql.Append("Orderfanum=Orderfanum+@Orderfanum,");
            strSql.Append("Orderjbnum=Orderjbnum+@Orderjbnum,");
			strSql.Append("Orderstats=@Orderstats");
            strSql.Append(" where Orderusid=@Orderusid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Orderusid", SqlDbType.Int,4),
                    new SqlParameter("@Orderusname", SqlDbType.NVarChar,50),
					new SqlParameter("@Orderbanum", SqlDbType.Money,8),
                    new SqlParameter("@Orderfanum", SqlDbType.Money,8),
					new SqlParameter("@Orderjbnum", SqlDbType.Money,8),
					new SqlParameter("@Orderstats", SqlDbType.Int,4)};
			parameters[0].Value = model.Orderusid;
            parameters[1].Value = model.Orderusname;
			parameters[2].Value = model.Orderbanum;
            parameters[3].Value = model.Orderfanum;
			parameters[4].Value = model.Orderjbnum;
			parameters[5].Value = model.Orderstats;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZBaOrder ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaOrderList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_ZBaOrder ");
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
        /// <returns>IList BaOrder</returns>
        public IList<TPR3.Model.guess.BaOrder> GetBaOrders(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaOrder> listBaOrders = new List<TPR3.Model.guess.BaOrder>();

            string sTable = "tb_ZBaOrder";
            string sPkey = "id";
            string sField = "ID,Orderusid,Orderusname,Orderbanum,Orderfanum,Orderjbnum";
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

                    return listBaOrders;
                }

                while (reader.Read())
                {
                    TPR3.Model.guess.BaOrder objBaOrder = new TPR3.Model.guess.BaOrder();
                    objBaOrder.ID = reader.GetInt32(0);
                    objBaOrder.Orderusid = reader.GetInt32(1);
                    objBaOrder.Orderusname = reader.GetString(2);
                    objBaOrder.Orderbanum = reader.GetDecimal(3);
                    objBaOrder.Orderfanum = reader.GetDecimal(4);
                    objBaOrder.Orderjbnum = reader.GetDecimal(5);
                    listBaOrders.Add(objBaOrder);

                }
            }

            return listBaOrders;
        }

		#endregion  ��Ա����
	}
}

