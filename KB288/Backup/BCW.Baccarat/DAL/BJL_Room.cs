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
    /// ���ݷ�����BJL_Room��
    /// </summary>
    public class BJL_Room
    {
        public BJL_Room()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BJL_Room");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Room");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Room model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BJL_Room(");
            strSql.Append("UsID,Total,LowTotal,Title,contact,AddTime,state,zhui_Total,type,Total_Now,shouxufei,Click,BigPay)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@Total,@LowTotal,@Title,@contact,@AddTime,@state,@zhui_Total,@type,@Total_Now,@shouxufei,@Click,@BigPay)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@Total", SqlDbType.BigInt,8),
                    new SqlParameter("@LowTotal", SqlDbType.BigInt,8),
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@contact", SqlDbType.NVarChar,150),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@state", SqlDbType.Int,4),
                    new SqlParameter("@zhui_Total", SqlDbType.BigInt,8),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@Total_Now", SqlDbType.BigInt,8),
                    new SqlParameter("@shouxufei", SqlDbType.BigInt,8),
                    new SqlParameter("@Click", SqlDbType.Int,4),
                    new SqlParameter("@BigPay", SqlDbType.BigInt,8)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.Total;
            parameters[2].Value = model.LowTotal;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.contact;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.state;
            parameters[7].Value = model.zhui_Total;
            parameters[8].Value = model.type;
            parameters[9].Value = model.Total_Now;
            parameters[10].Value = model.shouxufei;
            parameters[11].Value = model.Click;
            parameters[12].Value = model.BigPay;

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
        public void Update(BCW.Baccarat.Model.BJL_Room model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BJL_Room set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("Total=@Total,");
            strSql.Append("LowTotal=@LowTotal,");
            strSql.Append("Title=@Title,");
            strSql.Append("contact=@contact,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("state=@state,");
            strSql.Append("zhui_Total=@zhui_Total,");
            strSql.Append("type=@type,");
            strSql.Append("Total_Now=@Total_Now,");
            strSql.Append("shouxufei=@shouxufei,");
            strSql.Append("Click=@Click,");
            strSql.Append("BigPay=@BigPay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@Total", SqlDbType.BigInt,8),
                    new SqlParameter("@LowTotal", SqlDbType.BigInt,8),
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@contact", SqlDbType.NVarChar,150),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@state", SqlDbType.Int,4),
                    new SqlParameter("@zhui_Total", SqlDbType.BigInt,8),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@Total_Now", SqlDbType.BigInt,8),
                    new SqlParameter("@shouxufei", SqlDbType.BigInt,8),
                    new SqlParameter("@Click", SqlDbType.Int,4),
                    new SqlParameter("@BigPay", SqlDbType.BigInt,8)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.Total;
            parameters[3].Value = model.LowTotal;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.contact;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.state;
            parameters[8].Value = model.zhui_Total;
            parameters[9].Value = model.type;
            parameters[10].Value = model.Total_Now;
            parameters[11].Value = model.shouxufei;
            parameters[12].Value = model.Click;
            parameters[13].Value = model.BigPay;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_Room ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Room GetBJL_Room(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_Room ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_Room model = new BCW.Baccarat.Model.BJL_Room();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Total = reader.GetInt64(2);
                    model.LowTotal = reader.GetInt64(3);
                    model.Title = reader.GetString(4);
                    model.contact = reader.GetString(5);
                    model.AddTime = reader.GetDateTime(6);
                    model.state = reader.GetInt32(7);
                    model.zhui_Total = reader.GetInt64(8);
                    model.type = reader.GetInt32(9);
                    model.Total_Now = reader.GetInt64(10);
                    model.shouxufei = reader.GetInt64(11);
                    model.Click = reader.GetInt32(12);
                    model.BigPay = reader.GetInt64(13);
                    model.Bigmoney = reader.GetInt64(14);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
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
            strSql.Append(" FROM tb_BJL_Room ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //-------------------------------------------------
        public DataSet GetList2(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_BJL_Room ");
            if (strWhere.Trim() != "")
            {
                strSql.Append("" + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
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
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(" + ziduan + ") from tb_BJL_Room");
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
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_BJL_Room SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        ///  me_����ĳ����Ĳʳ�
        /// </summary>
        public long Getcaichi(int RoomID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT top(1)Total_Now from tb_BJL_Room");
            strSql.Append(" where ID=@RoomID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// me_�õ���ׯ��
        /// </summary>
        public int Get_kz_num(int meid, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) FROM tb_BJL_Room WHERE UsID=@meid and state=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Room GetBJL_Room(int ID, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_Room ");
            strSql.Append(" where ID=@ID and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
            new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            BCW.Baccarat.Model.BJL_Room model = new BCW.Baccarat.Model.BJL_Room();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Total = reader.GetInt64(2);
                    model.LowTotal = reader.GetInt64(3);
                    model.Title = reader.GetString(4);
                    model.contact = reader.GetString(5);
                    model.AddTime = reader.GetDateTime(6);
                    model.state = reader.GetInt32(7);
                    model.zhui_Total = reader.GetInt64(8);
                    model.type = reader.GetInt32(9);
                    model.Total_Now = reader.GetInt64(10);
                    model.shouxufei = reader.GetInt64(11);
                    model.Click = reader.GetInt32(12);
                    model.BigPay = reader.GetInt64(13);
                    model.Bigmoney = reader.GetInt64(14);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
                }
            }
        }
        //-------------------------------------------------
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BJL_Room</returns>
        public IList<BCW.Baccarat.Model.BJL_Room> GetBJL_Rooms(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BJL_Room> listBJL_Rooms = new List<BCW.Baccarat.Model.BJL_Room>();
            string sTable = "tb_BJL_Room";
            string sPkey = "id";
            string sField = "*";
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
                    return listBJL_Rooms;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BJL_Room objBJL_Room = new BCW.Baccarat.Model.BJL_Room();
                    objBJL_Room.ID = reader.GetInt32(0);
                    objBJL_Room.UsID = reader.GetInt32(1);
                    objBJL_Room.Total = reader.GetInt64(2);
                    objBJL_Room.LowTotal = reader.GetInt64(3);
                    objBJL_Room.Title = reader.GetString(4);
                    objBJL_Room.contact = reader.GetString(5);
                    objBJL_Room.AddTime = reader.GetDateTime(6);
                    objBJL_Room.state = reader.GetInt32(7);
                    objBJL_Room.zhui_Total = reader.GetInt64(8);
                    objBJL_Room.type = reader.GetInt32(9);
                    objBJL_Room.Total_Now = reader.GetInt64(10);
                    objBJL_Room.shouxufei = reader.GetInt64(11);
                    objBJL_Room.Click = reader.GetInt32(12);
                    objBJL_Room.BigPay = reader.GetInt64(13);
                    objBJL_Room.Bigmoney = reader.GetInt64(14);
                    listBJL_Rooms.Add(objBJL_Room);
                }
            }
            return listBJL_Rooms;
        }

        #endregion  ��Ա����
    }
}

