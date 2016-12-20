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
    /// ���ݷ�����BJL_Card��
    /// </summary>
    public class BJL_Card
    {
        public BJL_Card()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BJL_Card");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Card");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Card model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BJL_Card(");
            strSql.Append("RoomID,RoomTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint)");
            strSql.Append(" values (");
            strSql.Append("@RoomID,@RoomTable,@BankerPoker,@BankerPoint,@HunterPoker,@HunterPoint)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4)};
            parameters[0].Value = model.RoomID;
            parameters[1].Value = model.RoomTable;
            parameters[2].Value = model.BankerPoker;
            parameters[3].Value = model.BankerPoint;
            parameters[4].Value = model.HunterPoker;
            parameters[5].Value = model.HunterPoint;

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
        public void Update(BCW.Baccarat.Model.BJL_Card model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BJL_Card set ");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomTable=@RoomTable,");
            strSql.Append("BankerPoker=@BankerPoker,");
            strSql.Append("BankerPoint=@BankerPoint,");
            strSql.Append("HunterPoker=@HunterPoker,");
            strSql.Append("HunterPoint=@HunterPoint");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.RoomID;
            parameters[2].Value = model.RoomTable;
            parameters[3].Value = model.BankerPoker;
            parameters[4].Value = model.BankerPoint;
            parameters[5].Value = model.HunterPoker;
            parameters[6].Value = model.HunterPoint;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_Card ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetBJL_Card(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,RoomID,RoomTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint from tb_BJL_Card ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_Card model = new BCW.Baccarat.Model.BJL_Card();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.RoomID = reader.GetInt32(1);
                    model.RoomTable = reader.GetInt32(2);
                    model.BankerPoker = reader.GetString(3);
                    model.BankerPoint = reader.GetInt32(4);
                    model.HunterPoker = reader.GetString(5);
                    model.HunterPoint = reader.GetInt32(6);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_BJL_Card ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //===========================================

        /// <summary>
        /// �ж��Ƿ����ĳ�����������˿���
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public bool ExistsCard(int RoomID, int RoomTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Card");
            strSql.Append(" where RoomID=@RoomID and RoomTable=@RoomTable");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomTable;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///�õ��ض�����ID������table�����µ�����
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetCardMessage(int RoomID, int RoomTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BJL_Card ");
            strSql.Append("where RoomID=@RoomID and RoomTable=@RoomTable");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomTable;
            BCW.Baccarat.Model.BJL_Card model = new BCW.Baccarat.Model.BJL_Card();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.RoomID = reader.GetInt32(1);
                    model.RoomTable = reader.GetInt32(2);
                    model.BankerPoker = reader.GetString(3);
                    model.BankerPoint = reader.GetInt32(4);
                    model.HunterPoker = reader.GetString(5);
                    model.HunterPoint = reader.GetInt32(6);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
                }
            }
        }
        //==========================================


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BJL_Card</returns>
        public IList<BCW.Baccarat.Model.BJL_Card> GetBJL_Cards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BJL_Card> listBJL_Cards = new List<BCW.Baccarat.Model.BJL_Card>();
            string sTable = "tb_BJL_Card";
            string sPkey = "id";
            string sField = "ID,RoomID,RoomTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint";
            string sCondition = strWhere;
            string sOrder = "RoomTable Desc";
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
                    return listBJL_Cards;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BJL_Card objBJL_Card = new BCW.Baccarat.Model.BJL_Card();
                    objBJL_Card.ID = reader.GetInt32(0);
                    objBJL_Card.RoomID = reader.GetInt32(1);
                    objBJL_Card.RoomTable = reader.GetInt32(2);
                    objBJL_Card.BankerPoker = reader.GetString(3);
                    objBJL_Card.BankerPoint = reader.GetInt32(4);
                    objBJL_Card.HunterPoker = reader.GetString(5);
                    objBJL_Card.HunterPoint = reader.GetInt32(6);
                    listBJL_Cards.Add(objBJL_Card);
                }
            }
            return listBJL_Cards;
        }

        #endregion  ��Ա����
    }
}

