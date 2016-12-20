using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.farm.DAL
{
    /// <summary>
    /// ���ݷ�����NC_tudi��
    /// </summary>
    public class NC_tudi
    {
        public NC_tudi()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_tudi");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_tudi model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_tudi(");
            strSql.Append("usid,tudi,tudi_type,zuowu,zuowu_ji,iscao,iswater,isinsect,ischandi,output,zuowu_experience,harvest,updatetime,zuowu_time,isshifei)");
            strSql.Append(" values (");
            strSql.Append("@usid,@tudi,@tudi_type,@zuowu,@zuowu_ji,@iscao,@iswater,@isinsect,@ischandi,@output,@zuowu_experience,@harvest,@updatetime,@zuowu_time,@isshifei)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@tudi_type", SqlDbType.Int,4),
                    new SqlParameter("@zuowu", SqlDbType.VarChar,20),
                    new SqlParameter("@zuowu_ji", SqlDbType.Int,4),
                    new SqlParameter("@iscao", SqlDbType.Int,4),
                    new SqlParameter("@iswater", SqlDbType.Int,4),
                    new SqlParameter("@isinsect", SqlDbType.Int,4),
                    new SqlParameter("@ischandi", SqlDbType.Int,4),
                    new SqlParameter("@output", SqlDbType.VarChar,30),
                    new SqlParameter("@zuowu_experience", SqlDbType.Int,4),
                    new SqlParameter("@harvest", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@zuowu_time", SqlDbType.Int,4),
                    new SqlParameter("@isshifei", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.tudi;
            parameters[2].Value = model.tudi_type;
            parameters[3].Value = model.zuowu;
            parameters[4].Value = model.zuowu_ji;
            parameters[5].Value = model.iscao;
            parameters[6].Value = model.iswater;
            parameters[7].Value = model.isinsect;
            parameters[8].Value = model.ischandi;
            parameters[9].Value = model.output;
            parameters[10].Value = model.zuowu_experience;
            parameters[11].Value = model.harvest;
            parameters[12].Value = model.updatetime;
            parameters[13].Value = model.zuowu_time;
            parameters[14].Value = model.isshifei;

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
        public void Update(BCW.farm.Model.NC_tudi model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_tudi set ");
            strSql.Append("usid=@usid,");
            strSql.Append("tudi=@tudi,");
            strSql.Append("tudi_type=@tudi_type,");
            strSql.Append("zuowu=@zuowu,");
            strSql.Append("zuowu_ji=@zuowu_ji,");
            strSql.Append("iscao=@iscao,");
            strSql.Append("iswater=@iswater,");
            strSql.Append("isinsect=@isinsect,");
            strSql.Append("ischandi=@ischandi,");
            strSql.Append("output=@output,");
            strSql.Append("zuowu_experience=@zuowu_experience,");
            strSql.Append("harvest=@harvest,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("zuowu_time=@zuowu_time,");
            strSql.Append("isshifei=@isshifei");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@tudi_type", SqlDbType.Int,4),
                    new SqlParameter("@zuowu", SqlDbType.VarChar,20),
                    new SqlParameter("@zuowu_ji", SqlDbType.Int,4),
                    new SqlParameter("@iscao", SqlDbType.Int,4),
                    new SqlParameter("@iswater", SqlDbType.Int,4),
                    new SqlParameter("@isinsect", SqlDbType.Int,4),
                    new SqlParameter("@ischandi", SqlDbType.Int,4),
                    new SqlParameter("@output", SqlDbType.VarChar,30),
                    new SqlParameter("@zuowu_experience", SqlDbType.Int,4),
                    new SqlParameter("@harvest", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@zuowu_time", SqlDbType.Int,4),
                    new SqlParameter("@isshifei", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.tudi;
            parameters[3].Value = model.tudi_type;
            parameters[4].Value = model.zuowu;
            parameters[5].Value = model.zuowu_ji;
            parameters[6].Value = model.iscao;
            parameters[7].Value = model.iswater;
            parameters[8].Value = model.isinsect;
            parameters[9].Value = model.ischandi;
            parameters[10].Value = model.output;
            parameters[11].Value = model.zuowu_experience;
            parameters[12].Value = model.harvest;
            parameters[13].Value = model.updatetime;
            parameters[14].Value = model.zuowu_time;
            parameters[15].Value = model.isshifei;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_tudi ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_tudi GetNC_tudi(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,tudi,tudi_type,zuowu,zuowu_ji,iscao,iswater,isinsect,ischandi,output,zuowu_experience,harvest,updatetime,zuowu_time,isshifei from tb_NC_tudi ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.tudi = reader.GetInt32(2);
                    model.tudi_type = reader.GetInt32(3);
                    model.zuowu = reader.GetString(4);
                    model.zuowu_ji = reader.GetInt32(5);
                    model.iscao = reader.GetInt32(6);
                    model.iswater = reader.GetInt32(7);
                    model.isinsect = reader.GetInt32(8);
                    model.ischandi = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.zuowu_experience = reader.GetInt32(11);
                    model.harvest = reader.GetInt32(12);
                    model.updatetime = reader.GetDateTime(13);
                    model.zuowu_time = reader.GetInt32(14);
                    model.isshifei = reader.GetInt32(15);
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
            strSql.Append(" FROM tb_NC_tudi ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //=================================
        /// <summary>
        /// me_�����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strname, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM " + strname + "");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_�������Ƿ������
        /// </summary>
        public bool Exists_jin(int usid, int tudi)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from tb_NC_tudi");
            strSql.Append(" where ");
            strSql.Append("usid=@usid AND tudi_type=4 AND tudi=@tudi");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = tudi;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�������Ƿ������
        /// </summary>
        public bool Exists_hei(int usid, int tudi)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from tb_NC_tudi");
            strSql.Append(" where ");
            strSql.Append("usid=@usid AND tudi_type=3 AND tudi=@tudi");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = tudi;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�������Ƿ������
        /// </summary>
        public bool Exists_hong(int usid, int tudi)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from tb_NC_tudi");
            strSql.Append(" where ");
            strSql.Append("usid=@usid AND tudi_type=2 AND tudi=@tudi");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = tudi;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_����usid��ѯ�м��������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_htd(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid and tudi_type=2");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_����usid��ѯ�м��������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_heitd(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid and tudi_type=3");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_����usid��ѯ�м��������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_jtd(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid and tudi_type=4");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_����һ������
        /// </summary>
        public void Update_1(BCW.farm.Model.NC_tudi model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_tudi set ");
            //strSql.Append("usid=@usid,");
            //strSql.Append("tudi=@tudi,");
            //strSql.Append("tudi_type=@tudi_type,");
            strSql.Append("zuowu=@zuowu,");
            strSql.Append("zuowu_ji=@zuowu_ji,");
            strSql.Append("iscao=@iscao,");
            strSql.Append("iswater=@iswater,");
            strSql.Append("isinsect=@isinsect,");
            strSql.Append("ischandi=@ischandi,");
            strSql.Append("output=@output,");
            strSql.Append("zuowu_experience=@zuowu_experience,");
            strSql.Append("harvest=@harvest,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("zuowu_time=@zuowu_time,");
            strSql.Append("isshifei=@isshifei,");
            strSql.Append("z_caotime=getdate(),z_chongtime=getdate(),z_shuitime=getdate() ");
            strSql.Append(" where usid=@usid and tudi=@tudi");
            SqlParameter[] parameters = {
					//new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4),
					//new SqlParameter("@tudi_type", SqlDbType.Int,4),
					new SqlParameter("@zuowu", SqlDbType.VarChar,20),
                    new SqlParameter("@zuowu_ji", SqlDbType.Int,4),
                    new SqlParameter("@iscao", SqlDbType.Int,4),
                    new SqlParameter("@iswater", SqlDbType.Int,4),
                    new SqlParameter("@isinsect", SqlDbType.Int,4),
                    new SqlParameter("@ischandi", SqlDbType.Int,4),
                    new SqlParameter("@output", SqlDbType.VarChar,30),
                    new SqlParameter("@zuowu_experience", SqlDbType.Int,4),
                    new SqlParameter("@harvest", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@zuowu_time", SqlDbType.Int,4),
                    new SqlParameter("@isshifei", SqlDbType.Int,4)};
            //parameters[0].Value = model.ID;
            parameters[0].Value = model.usid;
            parameters[1].Value = model.tudi;
            //parameters[3].Value = model.tudi_type;
            parameters[2].Value = model.zuowu;
            parameters[3].Value = model.zuowu_ji;
            parameters[4].Value = model.iscao;
            parameters[5].Value = model.iswater;
            parameters[6].Value = model.isinsect;
            parameters[7].Value = model.ischandi;
            parameters[8].Value = model.output;
            parameters[9].Value = model.zuowu_experience;
            parameters[10].Value = model.harvest;
            parameters[11].Value = model.updatetime;
            parameters[12].Value = model.zuowu_time;
            parameters[13].Value = model.isshifei;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_tudi Getusid(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_tudi ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.tudi = reader.GetInt32(2);
                    model.tudi_type = reader.GetInt32(3);
                    model.zuowu = reader.GetString(4);
                    model.zuowu_ji = reader.GetInt32(5);
                    model.iscao = reader.GetInt32(6);
                    model.iswater = reader.GetInt32(7);
                    model.isinsect = reader.GetInt32(8);
                    model.ischandi = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.zuowu_experience = reader.GetInt32(11);
                    model.harvest = reader.GetInt32(12);
                    model.updatetime = reader.GetDateTime(13);
                    model.zuowu_time = reader.GetInt32(14);
                    model.isshifei = reader.GetInt32(15);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_����usid��ѯ�м�������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_tudinum(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        ///  me_����usid��ѯ�м�������
        /// </summary>
        public long Get_xianjing(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi ");
            strSql.Append(" where usid=@usid AND xianjing=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

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
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_tudi(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_tudi SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_�Ƿ�������س��ݼ�¼
        /// </summary>
        public bool Exists_chucao(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND iscao=1 and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�ж������Ƿ���ڿ��ֲ�/�ų�
        /// </summary>
        public bool Exists_zhongcao(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID and ischandi=1 AND updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������س��ݼ�¼_һ��
        /// </summary>
        public bool Exists_chucao_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi ");
            strSql.Append("where usid=@usid AND iscao=1 and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������ؽ�ˮ��¼
        /// </summary>
        public bool Exists_jiaoshui(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND iswater=1 and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������ؽ�ˮ��¼_һ��
        /// </summary>
        public bool Exists_jiaoshui_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from tb_NC_tudi");
            strSql.Append(" where ");
            strSql.Append("usid=@usid AND iswater=1 and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������س����¼
        /// </summary>
        public bool Exists_chuchong(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND isinsect=1 and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������س����¼_һ��
        /// </summary>
        public bool Exists_chuchong_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from tb_NC_tudi");
            strSql.Append(" where ");
            strSql.Append("usid=@usid AND isinsect=1 and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ����һ���ջ��¼_һ��
        /// </summary>
        public bool Exists_shouhuo_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi ");
            strSql.Append("where usid=@usid AND updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���͵ȡ
        /// </summary>
        public BCW.farm.Model.NC_tudi tou_tudinum1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid AND updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_��ѯ��͵�������ؿ���
        /// </summary>
        public BCW.farm.Model.NC_tudi tou_tudinum2(int usid, int meid_usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid AND updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1 AND touID like '%," + @meid_usid + ",%'");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@meid_usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = meid_usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)����
        /// </summary>
        public BCW.farm.Model.NC_tudi cao_tudinum1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid AND iscao=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)��ˮ
        /// </summary>
        public BCW.farm.Model.NC_tudi shui_tudinum1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid AND iswater=1 and ischandi=1 and zuowu!=''");//and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE())   �۹��� 20160601 �޸ĳ�����Խ�ˮ
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)����
        /// </summary>
        public BCW.farm.Model.NC_tudi chong_tudinum1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid AND isinsect=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)�Ų�
        /// </summary>
        public BCW.farm.Model.NC_tudi fangcao_num1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu!='' and iscao=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)�ų�
        /// </summary>
        public BCW.farm.Model.NC_tudi fangcao_num2(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_tudi");
            strSql.Append(" where usid=@usid and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu!='' and isinsect=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        ///me_�Ƿ�������ؼ�¼
        /// </summary>
        public bool Exists_tudi(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_�Ƿ����һ�����ؼ�¼_һ��
        /// </summary>
        public bool Exists_chandi_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi ");
            strSql.Append("where usid=@usid and ischandi=2");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_�ж��Ƿ��Լ��ֵĲ�
        /// </summary>
        public bool Exists_zcao(int tudi, int UsID, int meid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND caoID=@meid and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;
            parameters[2].Value = meid;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_�ж��Ƿ��Լ��ŵĳ�
        /// </summary>
        public bool Exists_zchong(int tudi, int UsID, int meid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND chongID=@meid and ischandi=1 and zuowu!=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;
            parameters[2].Value = meid;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������ؿɲ��ؼ�¼
        /// </summary>
        public bool Exists_chandi(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND ischandi=2");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ����һ�����ּ�¼_һ��
        /// </summary>
        public bool Exists_bozhong_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi ");
            strSql.Append("where usid=@usid and ischandi=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ��������ʩ�ʼ�¼_һ��
        /// </summary>
        public bool Exists_shifei_1(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where ");
            strSql.Append(" UsID=@UsID AND updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu_ji!=0 and ischandi=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ�������ؿ�ʩ�ʼ�¼
        /// </summary>
        public bool Exists_shifei(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and zuowu_ji!=0 and ischandi=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ��������
        /// </summary>
        public bool Exists_xianjing(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND xianjing=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ��������
        /// </summary>
        public bool Exists_xianjing2(int tudi, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi");
            strSql.Append(" where tudi=@tudi ");
            strSql.Append(" and UsID=@UsID AND xianjing=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_����usid��ѯ�м�����Բ��ֵ�����
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_tudinum_bz(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_tudi");
            strSql.Append(" where usid=@usid and zuowu=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.tudi = reader.GetInt32(2);
                    model.tudi_type = reader.GetInt32(3);
                    model.zuowu = reader.GetString(4);
                    model.zuowu_ji = reader.GetInt32(5);
                    model.iscao = reader.GetInt32(6);
                    model.iswater = reader.GetInt32(7);
                    model.isinsect = reader.GetInt32(8);
                    model.ischandi = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.zuowu_experience = reader.GetInt32(11);
                    model.harvest = reader.GetInt32(12);
                    model.updatetime = reader.GetDateTime(13);
                    model.zuowu_time = reader.GetInt32(14);
                    model.isshifei = reader.GetInt32(15);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// me_�Ƿ���ڸ�ID�ĸ����ؿ���ֲ
        /// </summary>
        public bool Exists_zhongzhi(int tudi, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi ");
            strSql.Append("where usid=@usid and tudi=@tudi and zuowu=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@tudi", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = tudi;
            parameters[1].Value = usid;


            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// me_�Ƿ���ڸ�ID�ĸ����ؿ��ջ�
        /// </summary>
        public bool Exists_shouhuo(int tudi, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tudi ");
            strSql.Append("where usid=@usid and tudi=@tudi and zuowu!='' and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = tudi;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// me_����usid�����صõ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_td(int usid, int tudi)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_tudi ");
            strSql.Append(" where usid=@usid and tudi=@tudi");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = tudi;

            BCW.farm.Model.NC_tudi model = new BCW.farm.Model.NC_tudi();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.tudi = reader.GetInt32(2);
                    model.tudi_type = reader.GetInt32(3);
                    model.zuowu = reader.GetString(4);
                    model.zuowu_ji = reader.GetInt32(5);
                    model.iscao = reader.GetInt32(6);
                    model.iswater = reader.GetInt32(7);
                    model.isinsect = reader.GetInt32(8);
                    model.ischandi = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.zuowu_experience = reader.GetInt32(11);
                    model.harvest = reader.GetInt32(12);
                    model.updatetime = reader.GetDateTime(13);
                    model.zuowu_time = reader.GetInt32(14);
                    model.isshifei = reader.GetInt32(15);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_tudi</returns>
        public IList<BCW.farm.Model.NC_tudi> GetNC_tudis(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_tudi> listNC_tudis = new List<BCW.farm.Model.NC_tudi>();
            string sTable = "tb_NC_tudi";
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
                    return listNC_tudis;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_tudi objNC_tudi = new BCW.farm.Model.NC_tudi();
                    objNC_tudi.ID = reader.GetInt32(0);
                    objNC_tudi.usid = reader.GetInt32(1);
                    objNC_tudi.tudi = reader.GetInt32(2);
                    objNC_tudi.tudi_type = reader.GetInt32(3);
                    objNC_tudi.zuowu = reader.GetString(4);
                    objNC_tudi.zuowu_ji = reader.GetInt32(5);
                    objNC_tudi.iscao = reader.GetInt32(6);
                    objNC_tudi.iswater = reader.GetInt32(7);
                    objNC_tudi.isinsect = reader.GetInt32(8);
                    objNC_tudi.ischandi = reader.GetInt32(9);
                    objNC_tudi.output = reader.GetString(10);
                    objNC_tudi.zuowu_experience = reader.GetInt32(11);
                    objNC_tudi.harvest = reader.GetInt32(12);
                    objNC_tudi.updatetime = reader.GetDateTime(13);
                    objNC_tudi.zuowu_time = reader.GetInt32(14);
                    objNC_tudi.isshifei = reader.GetInt32(15);
                    objNC_tudi.touID = reader.GetString(16);
                    objNC_tudi.xianjing = reader.GetInt32(17);
                    objNC_tudi.caoID = reader.GetString(18);
                    objNC_tudi.chongID = reader.GetString(19);
                    objNC_tudi.z_caotime = reader.GetDateTime(20);
                    objNC_tudi.z_chongtime = reader.GetDateTime(21);
                    objNC_tudi.z_shuitime = reader.GetDateTime(22);
                    listNC_tudis.Add(objNC_tudi);
                }
            }
            return listNC_tudis;
        }
        //=================================
        #endregion  ��Ա����
    }
}

