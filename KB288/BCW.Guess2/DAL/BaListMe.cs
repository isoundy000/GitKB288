using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace TPR2.DAL.guess
{
    /// <summary>
    /// ���ݷ�����BaListMe��
    /// </summary>
    public class BaListMe
    {
        public BaListMe()
        { }
        #region  ��Ա����

        //---------------------------����Ͷעʹ��----------------------
        /// <summary>
        /// ������ȷ������Ͷע��ԱID
        /// </summary>
        public void Updatep_usid(int ID, string p_usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_usid=@p_usid");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@p_usid", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = p_usid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �õ�����Ͷע��ԱID
        /// </summary>
        public string Getp_usid(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_usid from tb_BaListMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }
        //---------------------------����Ͷעʹ��----------------------
        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BaListMe");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaListMe");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsByp_id(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaListMe");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ������ش�С��
        /// </summary>
        public bool ExistsDX(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaListMe");
            strSql.Append(" where p_big_lu=@p_big_lu ");
            strSql.Append(" and p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8)};
            parameters[0].Value = p_id;
            parameters[1].Value = -1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ������ر�׼��
        /// </summary>
        public bool ExistsBZ(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaListMe");
            strSql.Append(" where p_bzs_lu=@p_bzs_lu ");
            strSql.Append(" and p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8)};
            parameters[0].Value = p_id;
            parameters[1].Value = -1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ���ѯ�ļ�¼��
        /// </summary>
        public int GetCount(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_BaListMe");
            strSql.Append(" where p_active=@p_active ");
            strSql.Append(" and p_TPRtime>=@p_TPRtime");
            strSql.Append(" and p_title=@p_title ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_active", SqlDbType.Int),
                    new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
					new SqlParameter("@p_title", SqlDbType.NVarChar ,50)};
            parameters[0].Value = 0;
            parameters[1].Value = System.DateTime.Now;
            parameters[2].Value = model.p_title;
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
        /// �õ���ѯ�ļ�¼��
        /// </summary>
        public int GetCountByp_title(string p_title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_BaListMe");
            strSql.Append(" where p_title=@p_title ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_title", SqlDbType.NVarChar ,50)};
            parameters[0].Value = p_title;
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
        /// ���������õ�������ע��
        /// </summary>
        public int GetBaListMeCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_BaListMe where " + strWhere + "");

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
        /// ����һ������
        /// </summary>
        public int Add(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaListMe(");
            strSql.Append("p_id,p_title,p_type,p_one,p_two,p_pk,p_dx_pk,p_pn,p_one_lu,p_two_lu,p_big_lu,p_small_lu,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_addtime,p_TPRtime,p_ison,p_del,usid,payCent)");
            strSql.Append(" values (");
            strSql.Append("@p_id,@p_title,@p_type,@p_one,@p_two,@p_pk,@p_dx_pk,@p_pn,@p_one_lu,@p_two_lu,@p_big_lu,@p_small_lu,@p_bzs_lu,@p_bzp_lu,@p_bzx_lu,@p_addtime,@p_TPRtime,@p_ison,@p_del,@usid,@payCent)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
					new SqlParameter("@p_ison", SqlDbType.Int,4),
					new SqlParameter("@p_del", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.BigInt,8)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            parameters[3].Value = model.p_one;
            parameters[4].Value = model.p_two;
            parameters[5].Value = model.p_pk;
            parameters[6].Value = model.p_dx_pk;
            parameters[7].Value = model.p_pn;
            parameters[8].Value = model.p_one_lu;
            parameters[9].Value = model.p_two_lu;
            parameters[10].Value = model.p_big_lu;
            parameters[11].Value = model.p_small_lu;
            parameters[12].Value = model.p_bzs_lu;
            parameters[13].Value = model.p_bzp_lu;
            parameters[14].Value = model.p_bzx_lu;
            parameters[15].Value = model.p_addtime;
            parameters[16].Value = model.p_TPRtime;
            parameters[17].Value = model.p_ison;
            parameters[18].Value = model.p_del;
            parameters[19].Value = model.usid;
            parameters[20].Value = model.payCent;

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
        /// ��������һ������
        /// </summary>
        public int FootAdd(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaListMe(");
            strSql.Append("p_id,p_title,p_type,p_one,p_two,p_pk,p_pn,p_one_lu,p_two_lu,p_addtime,p_TPRtime,p_del,usid,payCent)");
            strSql.Append(" values (");
            strSql.Append("@p_id,@p_title,@p_type,@p_one,@p_two,@p_pk,@p_pn,@p_one_lu,@p_two_lu,@p_addtime,@p_TPRtime,@p_del,@usid,@payCent)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
					new SqlParameter("@p_del", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.BigInt,8)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            parameters[3].Value = model.p_one;
            parameters[4].Value = model.p_two;
            parameters[5].Value = model.p_pk;
            parameters[6].Value = model.p_pn;
            parameters[7].Value = model.p_one_lu;
            parameters[8].Value = model.p_two_lu;
            parameters[9].Value = model.p_addtime;
            parameters[10].Value = model.p_TPRtime;
            parameters[11].Value = model.p_del;
            parameters[12].Value = model.usid;
            parameters[13].Value = model.payCent;

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
        /// ����������������ʾ
        /// </summary>
        public void Updatep_del(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_del=@p_del");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@p_del", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_del;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ��������ץȡ�벻ץȡ
        /// </summary>
        public void Updatep_jc(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_jc=@p_jc");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@p_jc", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_jc;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �������½��״̬
        /// </summary>
        public void Updatep_zd(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_zd=@p_zd");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_zd", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_zd;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �������±ȷ�
        /// </summary>
        public void UpdateResult(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_result_one", SqlDbType.Int,4),
                    new SqlParameter("@p_result_two", SqlDbType.Int,4),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_result_one;
            parameters[2].Value = model.p_result_two;
            parameters[3].Value = model.p_active;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Զ��������±ȷ�
        /// </summary>
        public int UpdateZDResult(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_once=@p_once,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_one", SqlDbType.Int,4),
                    new SqlParameter("@p_result_two", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_result_one;
            parameters[2].Value = model.p_result_two;
            parameters[3].Value = model.p_once;
            parameters[4].Value = model.p_active;
            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ���¼�ʱ�ȷ�8��
        /// </summary>
        public int UpdateBoResult2(int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");
            strSql.Append(",p_temptime='" + DateTime.Now + "'");//��¼�ȷָ��µ�ʱ��

            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_result_temp1;
            parameters[2].Value = p_result_temp2;
            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���¼�ʱ�ȷ�8��
        /// </summary>
        public int UpdateBoResult3(int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_result_temp1;
            parameters[2].Value = p_result_temp2;
            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���½���ʱ�伯��
        /// </summary>
        public void UpdateOnce(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_once=@p_once");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_once;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ���½���ʱ�伯��
        /// </summary>
        public void UpdateOnce(int p_id, string p_once)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_once=@p_once");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_once;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_title=@p_title,");
            strSql.Append("p_type=@p_type,");
            strSql.Append("p_one=@p_one,");
            strSql.Append("p_two=@p_two,");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu,");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu,");
            strSql.Append("p_addtime=@p_addtime,");
            strSql.Append("p_TPRtime=@p_TPRtime,");
            strSql.Append("p_ison=@p_ison");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
            		new SqlParameter("@p_ison", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            parameters[3].Value = model.p_one;
            parameters[4].Value = model.p_two;
            parameters[5].Value = model.p_pk;
            parameters[6].Value = model.p_dx_pk;
            parameters[7].Value = model.p_pn;
            parameters[8].Value = model.p_one_lu;
            parameters[9].Value = model.p_two_lu;
            parameters[10].Value = model.p_big_lu;
            parameters[11].Value = model.p_small_lu;
            parameters[12].Value = model.p_bzs_lu;
            parameters[13].Value = model.p_bzp_lu;
            parameters[14].Value = model.p_bzx_lu;
            parameters[15].Value = model.p_addtime;
            parameters[16].Value = model.p_TPRtime;
            parameters[17].Value = model.p_ison;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(TPR2.Model.guess.BaListMe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_title=@p_title,");
            strSql.Append("p_type=@p_type,");
            strSql.Append("p_one=@p_one,");
            strSql.Append("p_two=@p_two,");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu,");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu,");
            strSql.Append("p_addtime=@p_addtime,");
            strSql.Append("p_TPRtime=@p_TPRtime,");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("payCent=@payCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
            		new SqlParameter("@p_ison", SqlDbType.Int,4),
            		new SqlParameter("@payCent", SqlDbType.BigInt,8)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            parameters[3].Value = model.p_one;
            parameters[4].Value = model.p_two;
            parameters[5].Value = model.p_pk;
            parameters[6].Value = model.p_dx_pk;
            parameters[7].Value = model.p_pn;
            parameters[8].Value = model.p_one_lu;
            parameters[9].Value = model.p_two_lu;
            parameters[10].Value = model.p_big_lu;
            parameters[11].Value = model.p_small_lu;
            parameters[12].Value = model.p_bzs_lu;
            parameters[13].Value = model.p_bzp_lu;
            parameters[14].Value = model.p_bzx_lu;
            parameters[15].Value = model.p_addtime;
            parameters[16].Value = model.p_TPRtime;
            parameters[17].Value = model.p_ison;
            parameters[18].Value = model.payCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void BasketUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_title=@p_title,");
            strSql.Append("p_type=@p_type,");
            strSql.Append("p_one=@p_one,");
            strSql.Append("p_two=@p_two,");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu,");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu,");
            strSql.Append("p_addtime=@p_addtime,");
            strSql.Append("p_TPRtime=@p_TPRtime");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc and p_ison<>2");//����ׯ�̶����ʵĲ�����
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
            		new SqlParameter("@p_jc", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            parameters[3].Value = model.p_one;
            parameters[4].Value = model.p_two;
            parameters[5].Value = model.p_pk;
            parameters[6].Value = model.p_dx_pk;
            parameters[7].Value = model.p_pn;
            parameters[8].Value = model.p_one_lu;
            parameters[9].Value = model.p_two_lu;
            parameters[10].Value = model.p_big_lu;
            parameters[11].Value = model.p_small_lu;
            parameters[12].Value = model.p_bzs_lu;
            parameters[13].Value = model.p_bzp_lu;
            parameters[14].Value = model.p_bzx_lu;
            parameters[15].Value = model.p_addtime;
            parameters[16].Value = model.p_TPRtime;
            parameters[17].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������������������
        /// </summary>
        public void FootUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_title=@p_title,");
            strSql.Append("p_type=@p_type,");
            strSql.Append("p_one=@p_one,");
            strSql.Append("p_two=@p_two,");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_addtime=@p_addtime,");
            strSql.Append("p_TPRtime=@p_TPRtime");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc and p_ison<>2");//����ׯ�̶����ʵĲ�����
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
            		new SqlParameter("@p_jc", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            parameters[3].Value = model.p_one;
            parameters[4].Value = model.p_two;
            parameters[5].Value = model.p_pk;
            parameters[6].Value = model.p_pn;
            parameters[7].Value = model.p_one_lu;
            parameters[8].Value = model.p_two_lu;
            parameters[9].Value = model.p_addtime;
            parameters[10].Value = model.p_TPRtime;
            parameters[11].Value = 0;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���´�С������
        /// </summary>
        public void FootdxUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc and p_ison<>2");//����ׯ�̶����ʵĲ�����
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
                    new SqlParameter("@p_jc", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_dx_pk;
            parameters[2].Value = model.p_big_lu;
            parameters[3].Value = model.p_small_lu;
            parameters[4].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���������׼������
        /// </summary>
        public void FootbzUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc and p_ison<>2");//����ׯ�̶����ʵĲ�����
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8),
                    new SqlParameter("@p_jc", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_bzs_lu;
            parameters[2].Value = model.p_bzp_lu;
            parameters[3].Value = model.p_bzx_lu;
            parameters[4].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����Ϊ�ߵ�ģʽ
        /// </summary>
        public void FootOnceType(int ID, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("p_oncetime=@p_oncetime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_oncetime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;
            parameters[2].Value = dt;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            if (Types == 1)
            {
                strSql.Append("p_isluckone=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime1='" + DateTime.Now + "'");//��¼�����̷��̵�ʱ��

            }
            else if (Types == 2)
            {
                strSql.Append("p_islucktwo=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime2='" + DateTime.Now + "'");//��¼��С�̷��̵�ʱ��
            }
            else
            {
                strSql.Append("p_isluckthr=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime3='" + DateTime.Now + "'");//��¼��׼�̷��̵�ʱ��
            }

            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck2(int id, int state, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            if (Types == 1)
            {
                strSql.Append("p_isluckone=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime1='" + DateTime.Now + "'");//��¼�����̷��̵�ʱ��

            }
            else if (Types == 2)
            {
                strSql.Append("p_islucktwo=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime2='" + DateTime.Now + "'");//��¼��С�̷��̵�ʱ��
            }
            else
            {
                strSql.Append("p_isluckthr=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime3='" + DateTime.Now + "'");//��¼��׼�̷��̵�ʱ��
            }

            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���¼�ʱ�ȷ֣��ߵ�ʹ�ã�
        /// </summary>
        public void FootOnceUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaListMe set ");
            strSql.Append("p_temptime=@p_temptime,");

            //if (Utils.GetTopDomain() == "tl88.cc")
            //{
                if (model.p_temptime != DateTime.Parse("1990-1-1"))
                    strSql.Append("p_temptimes = case when p_temptimes IS NULL then '" + model.p_temptime + "' else p_temptimes+'|" + model.p_temptime + "' END,");
            //}
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                   new SqlParameter("@p_temptime", SqlDbType.DateTime),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_temptime;
            parameters[2].Value = model.p_result_temp1;
            parameters[3].Value = model.p_result_temp2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaListMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaListMe ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�������
        /// </summary>
        public int GetPtype(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_type from tb_BaListMe ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
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
        /// �õ�����ʱ��
        /// </summary>
        public DateTime Getp_TPRtime(int p_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_TPRtime from tb_BaListMe ");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetDateTime(0);
                    else
                        return Convert.ToDateTime("2020-1-1");
                }
                else
                {
                    return Convert.ToDateTime("2020-1-1");
                }
            }
        }

        /// <summary>
        /// �õ�p_id/�ڶ��ֱ�׼�̸�����
        /// </summary>
        public int Getp_id(DateTime p_TPRtime, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id from tb_BaListMe ");
            strSql.Append(" where p_TPRtime=@p_TPRtime ");
            strSql.Append(" and p_one=@p_one ");
            strSql.Append(" and p_two=@p_two ");
            strSql.Append(" and p_bzs_lu<>@p_bzs_lu ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8)};
            parameters[0].Value = p_TPRtime;
            parameters[1].Value = p_one;
            parameters[2].Value = p_two;
            parameters[3].Value = -1;

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
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaListMe GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id,p_TPRtime,p_result_temp1,p_result_temp2 from tb_BaListMe ");
            strSql.Append(" where p_TPRtime=@p_TPRtime ");
            strSql.Append(" and p_one=@p_one ");
            strSql.Append(" and p_two=@p_two ");
            strSql.Append(" and p_active=@p_active ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = p_TPRtime;
            parameters[1].Value = p_one;
            parameters[2].Value = p_two;
            parameters[3].Value = 0;
            TPR2.Model.guess.BaListMe model = new TPR2.Model.guess.BaListMe();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.p_id = reader.GetInt32(1);
                    model.p_TPRtime = reader.GetDateTime(2);
                    if (reader.IsDBNull(3))
                        model.p_result_temp1 = null;
                    else
                        model.p_result_temp1 = reader.GetInt32(3);

                    if (reader.IsDBNull(4))
                        model.p_result_temp2 = null;
                    else
                        model.p_result_temp2 = reader.GetInt32(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaListMe GetFootOpen(string p_title, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,p_id,p_TPRtime,p_result_temp1,p_result_temp2 from tb_BaListMe ");
            strSql.Append(" where p_title=@p_title ");
            strSql.Append(" and p_one=@p_one ");
            strSql.Append(" and p_two=@p_two ");
            strSql.Append(" and p_active=@p_active ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = p_title;
            parameters[1].Value = p_one;
            parameters[2].Value = p_two;
            parameters[3].Value = 0;

            TPR2.Model.guess.BaListMe model = new TPR2.Model.guess.BaListMe();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.p_id = reader.GetInt32(1);
                    model.p_TPRtime = reader.GetDateTime(2);
                    if (reader.IsDBNull(3))
                        model.p_result_temp1 = null;
                    else
                        model.p_result_temp1 = reader.GetInt32(3);

                    if (reader.IsDBNull(4))
                        model.p_result_temp2 = null;
                    else
                        model.p_result_temp2 = reader.GetInt32(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ��Ƿ����ߵ�
        /// </summary>
        public int Getison(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_ison from tb_BaListMe ");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
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
        /// �õ��Ƿ����ߵ�
        /// </summary>
        public int Getisonht(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_ison from tb_BaListMe ");
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
        /// �õ��ߵظ���ʱ�伯��
        /// </summary>
        public string Getonce(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_once from tb_BaListMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "1999-1-1 11:11:11";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// �õ��ȷָ���ʱ�伯��
        /// </summary>
        public string Getp_temptimes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_temptimes from tb_BaListMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// �õ����±ȷָ���ʱ��
        /// </summary>
        public DateTime Getp_temptime(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_temptime from tb_BaListMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetDateTime(0);
                    else
                        return DateTime.Parse("1990-1-1");
                }
                else
                {
                    return DateTime.Parse("1990-1-1");
                }
            }
        }

        /// <summary>
        /// �õ�p_temptime_p_id
        /// </summary>
        public TPR2.Model.guess.BaListMe Getp_temptime_p_id(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id,p_temptime,p_temptime1,p_temptime2,p_temptime3 from tb_BaListMe ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR2.Model.guess.BaListMe model = new TPR2.Model.guess.BaListMe();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    model.p_id = reader.GetInt32(0);

                    if (!reader.IsDBNull(1))
                        model.p_temptime = reader.GetDateTime(1);
                    else
                        model.p_temptime = DateTime.Parse("1990-1-1");

                    if (!reader.IsDBNull(2))
                        model.p_temptime1 = reader.GetDateTime(2);
                    else
                        model.p_temptime1 = Convert.ToDateTime("1990-1-1");

                    if (!reader.IsDBNull(3))
                        model.p_temptime2 = reader.GetDateTime(3);
                    else
                        model.p_temptime2 = Convert.ToDateTime("1990-1-1");

                    if (!reader.IsDBNull(4))
                        model.p_temptime3 = reader.GetDateTime(4);
                    else
                        model.p_temptime3 = Convert.ToDateTime("1990-1-1");

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ���ʱ�ȷ�
        /// </summary>
        public TPR2.Model.guess.BaListMe GetTemp(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_result_temp1,p_result_temp2,p_temptime1,p_temptime2,p_temptime3,p_temptime  from tb_BaListMe ");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR2.Model.guess.BaListMe model = new TPR2.Model.guess.BaListMe();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    if (!reader.IsDBNull(0))
                        model.p_result_temp1 = reader.GetInt32(0);

                    if (!reader.IsDBNull(1))
                        model.p_result_temp2 = reader.GetInt32(1);

                    if (!reader.IsDBNull(2))
                        model.p_temptime1 = reader.GetDateTime(2);
                    else
                        model.p_temptime1 = Convert.ToDateTime("1990-1-1");

                    if (!reader.IsDBNull(3))
                        model.p_temptime2 = reader.GetDateTime(3);
                    else
                        model.p_temptime2 = Convert.ToDateTime("1990-1-1");

                    if (!reader.IsDBNull(4))
                        model.p_temptime3 = reader.GetDateTime(4);
                    else
                        model.p_temptime3 = Convert.ToDateTime("1990-1-1");

                    if (!reader.IsDBNull(5))
                        model.p_temptime = reader.GetDateTime(5);
                    else
                        model.p_temptime = DateTime.Parse("1990-1-1");

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
        public TPR2.Model.guess.BaListMe GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,p_id,p_title,p_type,p_one,p_two,p_pk,p_dx_pk,p_pn,p_one_lu,p_two_lu,p_big_lu,p_small_lu,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_addtime,p_TPRtime,p_result_one,p_result_two,p_active,p_del,p_jc,p_ison,p_result_temp1,p_result_temp2,p_oncetime,p_temptime,usid,payCent,p_isluckone,p_islucktwo,p_isluckthr,p_once,p_temptime1,p_temptime2,p_temptime3 from tb_BaListMe ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR2.Model.guess.BaListMe model = new TPR2.Model.guess.BaListMe();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_id"].ToString() != "")
                {
                    model.p_id = int.Parse(ds.Tables[0].Rows[0]["p_id"].ToString());
                }
                model.p_title = ds.Tables[0].Rows[0]["p_title"].ToString();
                if (ds.Tables[0].Rows[0]["p_type"].ToString() != "")
                {
                    model.p_type = int.Parse(ds.Tables[0].Rows[0]["p_type"].ToString());
                }
                model.p_one = ds.Tables[0].Rows[0]["p_one"].ToString();
                model.p_two = ds.Tables[0].Rows[0]["p_two"].ToString();
                if (ds.Tables[0].Rows[0]["p_pk"].ToString() != "")
                {
                    model.p_pk = decimal.Parse(ds.Tables[0].Rows[0]["p_pk"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_dx_pk"].ToString() != "")
                {
                    model.p_dx_pk = decimal.Parse(ds.Tables[0].Rows[0]["p_dx_pk"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_pn"].ToString() != "")
                {
                    model.p_pn = int.Parse(ds.Tables[0].Rows[0]["p_pn"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_one_lu"].ToString() != "")
                {
                    model.p_one_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_one_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_two_lu"].ToString() != "")
                {
                    model.p_two_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_two_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_big_lu"].ToString() != "")
                {
                    model.p_big_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_big_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_small_lu"].ToString() != "")
                {
                    model.p_small_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_small_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_bzs_lu"].ToString() != "")
                {
                    model.p_bzs_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_bzs_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_bzp_lu"].ToString() != "")
                {
                    model.p_bzp_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_bzp_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_bzx_lu"].ToString() != "")
                {
                    model.p_bzx_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_bzx_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_addtime"].ToString() != "")
                {
                    model.p_addtime = DateTime.Parse(ds.Tables[0].Rows[0]["p_addtime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_TPRtime"].ToString() != "")
                {
                    model.p_TPRtime = DateTime.Parse(ds.Tables[0].Rows[0]["p_TPRtime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_result_one"].ToString() != "")
                {
                    model.p_result_one = int.Parse(ds.Tables[0].Rows[0]["p_result_one"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_result_two"].ToString() != "")
                {
                    model.p_result_two = int.Parse(ds.Tables[0].Rows[0]["p_result_two"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_active"].ToString() != "")
                {
                    model.p_active = int.Parse(ds.Tables[0].Rows[0]["p_active"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_del"].ToString() != "")
                {
                    model.p_del = int.Parse(ds.Tables[0].Rows[0]["p_del"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_jc"].ToString() != "")
                {
                    model.p_jc = int.Parse(ds.Tables[0].Rows[0]["p_jc"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_ison"].ToString() != "")
                {
                    model.p_ison = int.Parse(ds.Tables[0].Rows[0]["p_ison"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_result_temp1"].ToString() != "")
                {
                    model.p_result_temp1 = int.Parse(ds.Tables[0].Rows[0]["p_result_temp1"].ToString());
                }
                else
                {
                    model.p_result_temp1 = 0;
                }
                if (ds.Tables[0].Rows[0]["p_result_temp2"].ToString() != "")
                {
                    model.p_result_temp2 = int.Parse(ds.Tables[0].Rows[0]["p_result_temp2"].ToString());
                }
                else
                {
                    model.p_result_temp2 = 0;
                }
                if (ds.Tables[0].Rows[0]["p_oncetime"].ToString() != "")
                {
                    model.p_oncetime = DateTime.Parse(ds.Tables[0].Rows[0]["p_oncetime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_temptime"].ToString() != "")
                {
                    model.p_temptime = DateTime.Parse(ds.Tables[0].Rows[0]["p_temptime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["usid"].ToString() != "")
                {
                    model.usid = int.Parse(ds.Tables[0].Rows[0]["usid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payCent"].ToString() != "")
                {
                    model.payCent = Int64.Parse(ds.Tables[0].Rows[0]["payCent"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_isluckone"].ToString() != "")
                {
                    model.p_isluckone = int.Parse(ds.Tables[0].Rows[0]["p_isluckone"].ToString());
                }
                else
                {
                    model.p_isluckone = 0;
                }
                if (ds.Tables[0].Rows[0]["p_islucktwo"].ToString() != "")
                {
                    model.p_islucktwo = int.Parse(ds.Tables[0].Rows[0]["p_islucktwo"].ToString());
                }
                else
                {
                    model.p_islucktwo = 0;
                }
                if (ds.Tables[0].Rows[0]["p_isluckthr"].ToString() != "")
                {
                    model.p_isluckthr = int.Parse(ds.Tables[0].Rows[0]["p_isluckthr"].ToString());
                }
                else
                {
                    model.p_isluckthr = 0;
                }
                if (ds.Tables[0].Rows[0]["p_once"].ToString() != "")
                {
                    model.p_once = ds.Tables[0].Rows[0]["p_once"].ToString();
                }
                if (ds.Tables[0].Rows[0]["p_temptime1"].ToString() != "")
                {
                    model.p_temptime1 = DateTime.Parse(ds.Tables[0].Rows[0]["p_temptime1"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_temptime2"].ToString() != "")
                {
                    model.p_temptime2 = DateTime.Parse(ds.Tables[0].Rows[0]["p_temptime2"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_temptime3"].ToString() != "")
                {
                    model.p_temptime3 = DateTime.Parse(ds.Tables[0].Rows[0]["p_temptime3"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList BaListMe</returns>
        public IList<TPR2.Model.guess.BaListMe> GetBaListMes(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaListMe> listBaListMes = new List<TPR2.Model.guess.BaListMe>();

            string sTable = "tb_BaListMe";
            string sPkey = "id";
            string sField = "id,p_title,p_one,p_two,p_tprtime,p_result_one,p_result_two,p_active,p_del,p_zd,p_type,p_ison,p_result_temp1,p_result_temp2,p_usid,payCent";
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

                    return listBaListMes;
                }

                while (reader.Read())
                {
                    TPR2.Model.guess.BaListMe objBaListMe = new TPR2.Model.guess.BaListMe();
                    objBaListMe.ID = reader.GetInt32(0);
                    objBaListMe.p_title = reader.GetString(1);
                    objBaListMe.p_one = reader.GetString(2);
                    objBaListMe.p_two = reader.GetString(3);
                    objBaListMe.p_TPRtime = reader.GetDateTime(4);

                    if (reader.IsDBNull(5))
                        objBaListMe.p_result_one = null;
                    else
                        objBaListMe.p_result_one = reader.GetInt32(5);

                    if (reader.IsDBNull(6))
                        objBaListMe.p_result_two = null;
                    else
                        objBaListMe.p_result_two = reader.GetInt32(6);

                    objBaListMe.p_active = reader.GetInt32(7);
                    objBaListMe.p_del = reader.GetInt32(8);
                    objBaListMe.p_zd = reader.GetInt32(9);
                    objBaListMe.p_type = reader.GetInt32(10);
                    objBaListMe.p_ison = reader.GetInt32(11);
                    if (reader.IsDBNull(12))
                        objBaListMe.p_result_temp1 = null;
                    else
                        objBaListMe.p_result_temp1 = reader.GetInt32(12);

                    if (reader.IsDBNull(13))
                        objBaListMe.p_result_temp2 = null;
                    else
                        objBaListMe.p_result_temp2 = reader.GetInt32(13);

                    if (!reader.IsDBNull(14))
                        objBaListMe.p_usid = reader.GetString(14);

                    objBaListMe.payCent = reader.GetInt64(15);
                    listBaListMes.Add(objBaListMe);


                }
            }

            return listBaListMes;
        }


        /// <summary>
        /// ȡ��������¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public List<TPR2.Model.guess.BaListMe> GetBaListMeLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            List<TPR2.Model.guess.BaListMe> listBaListMeLX = new List<TPR2.Model.guess.BaListMe>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT p_title) FROM tb_BaListMe where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBaListMeLX;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT DISTINCT p_title FROM tb_BaListMe where " + strWhere + "";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        TPR2.Model.guess.BaListMe objBaListMeLX = new TPR2.Model.guess.BaListMe();
                        objBaListMeLX.p_title = reader.GetString(0);
                        listBaListMeLX.Add(objBaListMeLX);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBaListMeLX;
        }
        /// <summary>
        /// ȡ��δ����������
        /// </summary>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public List<TPR2.Model.guess.BaListMe> GetBaListMeBF(string strWhere, out int p_recordCount)
        {
            List<TPR2.Model.guess.BaListMe> listBaListMeBF = new List<TPR2.Model.guess.BaListMe>();

            // �����¼��
            string countString = "SELECT COUNT(ID) FROM tb_BaListMe where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount <= 0)
            {
                return listBaListMeBF;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT ID,p_id,p_TPRtime,p_one FROM tb_BaListMe where " + strWhere + "";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int k = 0;
                while (reader.Read())
                {
                    TPR2.Model.guess.BaListMe objBaListMeBF = new TPR2.Model.guess.BaListMe();
                    objBaListMeBF.ID = reader.GetInt32(0);
                    objBaListMeBF.p_id = reader.GetInt32(1);
                    objBaListMeBF.p_TPRtime = reader.GetDateTime(2);
                    objBaListMeBF.p_one = reader.GetString(3);
                    listBaListMeBF.Add(objBaListMeBF);

                    if (k == p_recordCount)
                        break;

                    k++;
                }
            }

            return listBaListMeBF;
        }

        #endregion  ��Ա����
    }
}

