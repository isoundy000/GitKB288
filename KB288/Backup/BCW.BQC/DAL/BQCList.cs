using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.BQC.DAL
{
    /// <summary>
    /// ���ݷ�����BQCList��
    /// </summary>
    public class BQCList
    {
        public BQCList()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("id", "tb_BQCList");
        }

        /// <summary>
        /// �����ںŵõ�id
        /// </summary>
        public int id(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id  from tb_BQCList where CID=" + CID);
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
        /// �õ���һ��CID
        /// </summary>
        public int CIDnew()
        {
            return SqlHelper.GetMaxID("CID", "tb_BQCList");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCList");
            strSql.Append(" where CID=@CID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existslist(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCList");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ�����м�¼
        /// </summary>
        public bool Existsjilu()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCList");

            return SqlHelper.Exists(strSql.ToString());
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existsysprize(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCList");
            strSql.Append(" where CID<@CID and sysprizestatue=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ����ϵͳͶע��¼
        /// </summary>
        public bool ExistsSysprize(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCList");
            strSql.Append(" where CID=@CID and  sysprizestatue ='1' ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ���ǰ�����ܶ�
        /// </summary>
        public long nowprize(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select nowprize  from tb_BQCList where CID=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@nowprize", SqlDbType.Int,8)};

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
        /// ����ϵͳͶע״̬
        /// </summary>
        public void UpdateSysprizestatue(int CID, int sysprizestatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("sysprizestatue=@sysprizestatue where ");
            strSql.Append("CID=@CID");
            SqlParameter[] parameters = {
                    new SqlParameter("@sysprizestatue", SqlDbType.Int),
                                        new SqlParameter("@CID",SqlDbType.Int)};
            parameters[0].Value = sysprizestatue;
            parameters[1].Value = CID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ����ϵͳͶע
        /// </summary>
        public void UpdateSysstaprize(int CID, int sysprizestatue, long sysprize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("sysprizestatue=@sysprizestatue, ");
            strSql.Append(" sysprize=@sysprize where ");
            strSql.Append("CID=@CID");
            SqlParameter[] parameters = {
                    new SqlParameter("@sysprizestatue", SqlDbType.Int),
                      new SqlParameter("@sysprize", SqlDbType.Int),
                                        new SqlParameter("@CID",SqlDbType.Int)};
            parameters[0].Value = sysprizestatue;
            parameters[1].Value = sysprize;
            parameters[2].Value = CID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ���µ��ڽ��ؽ���
        /// </summary>
        public void UpdateNextprize(int CID, long nextprize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("nextprize=@nextprize where ");
            strSql.Append("CID=@CID");
            SqlParameter[] parameters = {
                    new SqlParameter("@nextprize", SqlDbType.Int),
                                        new SqlParameter("@CID",SqlDbType.Int)};
            parameters[0].Value = nextprize;
            parameters[1].Value = CID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ���µ���ϵͳ��ȡ����
        /// </summary>
        public void Updatesysdayprize(int CID, long sysdayprize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("sysdayprize=@sysdayprize where ");
            strSql.Append("CID=@CID");
            SqlParameter[] parameters = {
                    new SqlParameter("@sysdayprize", SqlDbType.Int),
                                        new SqlParameter("@CID",SqlDbType.Int)};
            parameters[0].Value = sysdayprize;
            parameters[1].Value = CID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸ�����
        /// </summary>
        public bool ExistsCID(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCList");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �õ��������
        /// </summary>
        public string result(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  result from tb_BQCList where CID=" + CID + " order by id desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@result", SqlDbType.Int,4)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }

            else
            {
                return Convert.ToString(obj);
            }
        }
        /// <summary>
        /// ������ע�ܶ�
        /// </summary>
        /// <param name="PayCent"></param>
        /// <param name="CID"></param>
        public void UpdatePayCent(long PayCent, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("PayCent=@PayCent ");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCent", SqlDbType.BigInt,8),
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = PayCent;
            parameters[1].Value = CID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������ע��
        /// </summary>
        /// <param name="PayCount"></param>
        /// <param name="CID"></param>
        public void UpdatePayCount(int PayCount, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("PayCount=@PayCount ");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCount", SqlDbType.Int,4),
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = PayCount;
            parameters[1].Value = CID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        public void updateNowprize(long nowprize, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("nowprize=@nowprize ");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@nowprize", SqlDbType.BigInt,8),
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = nowprize;
            parameters[1].Value = CID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateResult(int id, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("Result=@Result where ");
            strSql.Append("id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Result", SqlDbType.NVarChar),
                                        new SqlParameter("@id",SqlDbType.Int)};
            parameters[0].Value = Result;
            parameters[1].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ָ�������ı������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindResultByPhase(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Result from tb_BQCList where CID=@CID");
            SqlParameter parameters = new SqlParameter("@CID", SqlDbType.Int, 4);

            parameters.Value = CID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }

            else
            {
                return Convert.ToString(obj);
            }
        }
        /// <summary>
        /// �õ����ڽ��ؽ���
        /// </summary>
        public int getnextprize(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select nextprize  from tb_BQCList where CID=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@nextprize", SqlDbType.Int,8)};

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
        /// �õ�����Ͷ���ʶ
        /// </summary>
        public int getsysstate(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(sysprizestatue)  from tb_BQCList where CID<=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@sysprizestatue", SqlDbType.Int,4)};

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
        /// �õ�һ��GetSysPaylast
        /// </summary>
        public long GetSysPaylast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysprize) from tb_BQCList ");

            strSql.Append(" where CID=(select Top(1) CID from tb_BQCList where State=1 Order by CID Desc) and sysprizestatue!=2 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@sysprize", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_BQCList ");
            strSql.Append(" where " + strWhere + " ");
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
        /// �õ�һ��GetSysPaylast5
        /// </summary>
        public long GetSysPaylast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysprize) from tb_BQCList ");

            strSql.Append(" where CID in (select Top(5) CID from tb_BQCList where State=1 Order by CID Desc) and sysprizestatue!=2 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@sysprize", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ��GetSysPaylast
        /// </summary>
        public long GetSysdayprizelast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysdayprize) from tb_BQCList ");

            strSql.Append(" where CID=(select Top(1) CID from tb_BQCList where State=1 Order by CID Desc) ");

            SqlParameter[] parameters = {
                    new SqlParameter("@sysdayprize", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ��GetSysPaylast5
        /// </summary>
        public long GetSysdayprizelast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysdayprize) from tb_BQCList ");

            strSql.Append(" where CID in (select Top(5) CID from tb_BQCList where State=1 Order by CID Desc) ");

            SqlParameter[] parameters = {
                    new SqlParameter("@sysdayprize", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�����ϵͳͶע
        /// </summary>
        public long getsysprize(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sysprize  from tb_BQCList where CID=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@sysprize", SqlDbType.Int,8)};

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
        /// �õ����ڿ���״̬
        /// </summary>
        public int getState(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State  from tb_BQCList where CID=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@State", SqlDbType.Int,8)};

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
        /// �õ�����ϵͳͶע״̬
        /// </summary>
        public int getsysprizestatue(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sysprizestatue  from tb_BQCList where CID=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@sysprizestatue", SqlDbType.Int,8)};

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
        /// �õ�һ��GetSyspayall������ʱ��
        /// </summary>
        public long GetSyspayall()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysprize) from tb_BQCList ");
            strSql.Append(" where State=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Prize", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ��GetSysdayprizeall������ʱ��
        /// </summary>
        public long GetSysdayprizeall()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysdayprize) from tb_BQCList ");
            strSql.Append(" where State=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@sysdayprize", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ����һ������
        /// </summary>
        public int Add(BCW.BQC.Model.BQCList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BQCList(");
            strSql.Append("CID,Match,Team_Home,Team_Away,Start_time,Score,Result,State,PayCent,PayCount,EndTime,other,Sale_StartTime,nowprize,nextprize,sysprize,sysprizestatue,sysdayprize)");
            strSql.Append(" values (");
            strSql.Append("@CID,@Match,@Team_Home,@Team_Away,@Start_time,@Score,@Result,@State,@PayCent,@PayCount,@EndTime,@other,@Sale_StartTime,@nowprize,@nextprize,@sysprize,@sysprizestatue,@sysdayprize)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@Match", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Home", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Away", SqlDbType.NVarChar,300),
                    new SqlParameter("@Start_time", SqlDbType.NVarChar,300),
                    new SqlParameter("@Score", SqlDbType.NVarChar,300),
                    new SqlParameter("@Result", SqlDbType.NVarChar,100),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@PayCent", SqlDbType.BigInt,8),
                    new SqlParameter("@PayCount", SqlDbType.Int,4),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@other", SqlDbType.NChar,10),
                    new SqlParameter("@Sale_StartTime", SqlDbType.DateTime),
         new SqlParameter("@nowprize", SqlDbType.BigInt,8),
              	new SqlParameter("@nextprize", SqlDbType.BigInt,8),
					new SqlParameter("@sysprize", SqlDbType.BigInt,8),
					new SqlParameter("@sysprizestatue", SqlDbType.Int,4),
					new SqlParameter("@sysdayprize", SqlDbType.BigInt,8)};
            parameters[0].Value = model.CID;
            parameters[1].Value = model.Match;
            parameters[2].Value = model.Team_Home;
            parameters[3].Value = model.Team_Away;
            parameters[4].Value = model.Start_time;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.Result;
            parameters[7].Value = model.State;
            parameters[8].Value = model.PayCent;
            parameters[9].Value = model.PayCount;
            parameters[10].Value = model.EndTime;
            parameters[11].Value = model.other;
            parameters[12].Value = model.Sale_StartTime;
            parameters[13].Value = model.nowprize;
            parameters[14].Value = model.nextprize;
            parameters[15].Value = model.sysprize;
            parameters[16].Value = model.sysprizestatue;
            parameters[17].Value = model.sysdayprize;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMatchs(Model.BQCList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("Score=@Score,");
            strSql.Append("Result=@Result,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("Sale_StartTime=@Sale_StartTime,");
            strSql.Append("Match=@Match,");
            strSql.Append("Team_Home=@Team_Home,");
            strSql.Append("Team_Away=@Team_Away,");
            strSql.Append("Start_time=@Start_time");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@Score", SqlDbType.NVarChar,300),
                    new SqlParameter("@Result", SqlDbType.NVarChar,100),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@Sale_StartTime", SqlDbType.DateTime),
                     new SqlParameter("@Match", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Home", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Away", SqlDbType.NVarChar,300),
                    new SqlParameter("@Start_time", SqlDbType.NVarChar,300)
                   };
            parameters[0].Value = model.CID;
            parameters[1].Value = model.Score;
            parameters[2].Value = model.Result;
            parameters[3].Value = model.EndTime;
            parameters[4].Value = model.Sale_StartTime;
            parameters[5].Value = model.Match;
            parameters[6].Value = model.Team_Home;
            parameters[7].Value = model.Team_Away;
            parameters[8].Value = model.Start_time;

            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���¿���״̬
        /// </summary>
        /// <param name="state"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public int UpdateState(int state, int cid)
        {
            string sql = "update tb_BQCList set State=@State where CID=@CID";
            SqlParameter[] paremeters = {
                new SqlParameter("@State",SqlDbType.Int,4),
                new SqlParameter("@CID",SqlDbType.Int,4)
            };
            paremeters[0].Value = state;
            paremeters[1].Value = cid;
            return SqlHelper.ExecuteSql(sql, paremeters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.BQC.Model.BQCList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCList set ");
            strSql.Append("CID=@CID,");
            strSql.Append("Match=@Match,");
            strSql.Append("Team_Home=@Team_Home,");
            strSql.Append("Team_Away=@Team_Away,");
            strSql.Append("Start_time=@Start_time,");
            strSql.Append("Score=@Score,");
            strSql.Append("Result=@Result,");
            strSql.Append("State=@State,");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("PayCount=@PayCount,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("other=@other,");
            strSql.Append("Sale_StartTime=@Sale_StartTime,");
            strSql.Append("nowprize=@nowprize,");
            strSql.Append("nextprize=@nextprize,");
            strSql.Append("sysprize=@sysprize,");
            strSql.Append("sysprizestatue=@sysprizestatue,");
            strSql.Append("sysdayprize=@sysdayprize");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@Match", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Home", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Away", SqlDbType.NVarChar,300),
                    new SqlParameter("@Start_time", SqlDbType.NVarChar,300),
                    new SqlParameter("@Score", SqlDbType.NVarChar,300),
                    new SqlParameter("@Result", SqlDbType.NVarChar,100),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@PayCent", SqlDbType.BigInt,8),
                    new SqlParameter("@PayCount", SqlDbType.Int,4),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@other", SqlDbType.NChar,10),
                    new SqlParameter("@Sale_StartTime", SqlDbType.DateTime),
                    new SqlParameter("@id", SqlDbType.Int,4),
                	new SqlParameter("@nowprize", SqlDbType.BigInt,8),
					new SqlParameter("@nextprize", SqlDbType.BigInt,8),
					new SqlParameter("@sysprize", SqlDbType.BigInt,8),
					new SqlParameter("@sysprizestatue", SqlDbType.Int,4),
					new SqlParameter("@sysdayprize", SqlDbType.BigInt,8)};
            parameters[0].Value = model.CID;
            parameters[1].Value = model.Match;
            parameters[2].Value = model.Team_Home;
            parameters[3].Value = model.Team_Away;
            parameters[4].Value = model.Start_time;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.Result;
            parameters[7].Value = model.State;
            parameters[8].Value = model.PayCent;
            parameters[9].Value = model.PayCount;
            parameters[10].Value = model.EndTime;
            parameters[11].Value = model.other;
            parameters[12].Value = model.Sale_StartTime;
            parameters[13].Value = model.id;
            parameters[14].Value = model.nowprize;
            parameters[15].Value = model.nextprize;
            parameters[16].Value = model.sysprize;
            parameters[17].Value = model.sysprizestatue;
            parameters[18].Value = model.sysdayprize;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BQCList ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ����������ں�
        /// </summary>
        public int CID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 CID from tb_BQCList where State=1 order by CID desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};

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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.BQC.Model.BQCList GetBQCList(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CID,Match,Team_Home,Team_Away,Start_time,Score,Result,State,PayCent,PayCount,EndTime,other,Sale_StartTime,id,nowprize,nextprize,sysprize,sysprizestatue,sysdayprize from tb_BQCList ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.BQC.Model.BQCList model = new BCW.BQC.Model.BQCList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.CID = reader.GetInt32(0);
                    model.Match = reader.GetString(1);
                    model.Team_Home = reader.GetString(2);
                    model.Team_Away = reader.GetString(3);
                    model.Start_time = reader.GetString(4);
                    model.Score = reader.GetString(5);
                    model.Result = reader.GetString(6);
                    model.State = reader.GetInt32(7);
                    model.PayCent = reader.GetInt64(8);
                    model.PayCount = reader.GetInt32(9);
                    model.EndTime = reader.GetDateTime(10);
                    model.other = reader.GetString(11);
                    model.Sale_StartTime = reader.GetDateTime(12);
                    model.id = reader.GetInt32(13);
                    model.nowprize = reader.GetInt64(14);
                    model.nextprize = reader.GetInt64(15);
                    model.sysprize = reader.GetInt64(16);
                    model.sysprizestatue = reader.GetInt32(17);
                    model.sysdayprize = reader.GetInt64(18);
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
        public BCW.BQC.Model.BQCList GetBQCList1(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CID,Match,Team_Home,Team_Away,Start_time,Score,Result,State,PayCent,PayCount,EndTime,other,Sale_StartTime,id,nowprize,nextprize,sysprize,sysprizestatue,sysdayprize  from tb_BQCList ");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.BQC.Model.BQCList model = new BCW.BQC.Model.BQCList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.CID = reader.GetInt32(0);
                    model.Match = reader.GetString(1);
                    model.Team_Home = reader.GetString(2);
                    model.Team_Away = reader.GetString(3);
                    model.Start_time = reader.GetString(4);
                    model.Score = reader.GetString(5);
                    model.Result = reader.GetString(6);
                    model.State = reader.GetInt32(7);
                    model.PayCent = reader.GetInt64(8);
                    model.PayCount = reader.GetInt32(9);
                    model.EndTime = reader.GetDateTime(10);
                    model.other = reader.GetString(11);
                    model.Sale_StartTime = reader.GetDateTime(12);
                    model.id = reader.GetInt32(13);
                    model.nowprize = reader.GetInt64(14);
                    model.nextprize = reader.GetInt64(15);
                    model.sysprize = reader.GetInt64(16);
                    model.sysprizestatue = reader.GetInt32(17);
                    model.sysdayprize = reader.GetInt64(18);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �����ںŵõ�һ������ʵ��
        /// </summary>
        public Model.BQCList GetBQCListByCID(int CID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,CID,Match,Team_Home,Team_Away,Start_time,Score,Result,State,PayCent,PayCount,EndTime,other,Sale_StartTime,nowprize,nextprize,sysprize,sysprizestatue,sysdayprize  from tb_BQCList ");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            Model.BQCList model = new Model.BQCList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.CID = reader.GetInt32(1);
                    model.Match = reader.GetString(2);
                    model.Team_Home = reader.GetString(3);
                    model.Team_Away = reader.GetString(4);
                    model.Start_time = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        model.Score = reader.GetString(6);
                    model.Result = reader.GetString(7);
                    model.State = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        model.PayCent = reader.GetInt64(9);
                    if (!reader.IsDBNull(10))
                        model.PayCount = reader.GetInt32(10);
                    model.EndTime = reader.GetDateTime(11);
                    if (!reader.IsDBNull(12))
                        model.other = reader.GetString(12);
                    if (!reader.IsDBNull(13))
                        model.Sale_StartTime = reader.GetDateTime(13);
                    if (!reader.IsDBNull(14))
                        model.nowprize = reader.GetInt64(14);
                    model.nextprize = reader.GetInt64(15);
                    model.sysprize = reader.GetInt64(16);
                    model.sysprizestatue = reader.GetInt32(17);
                    model.sysdayprize = reader.GetInt64(18);
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
            strSql.Append(" FROM tb_BQCList ");
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
        /// <returns>IList BQCList</returns>
        public IList<BCW.BQC.Model.BQCList> GetBQCLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.BQC.Model.BQCList> listBQCLists = new List<BCW.BQC.Model.BQCList>();
            string sTable = "tb_BQCList";
            string sPkey = "id";
            string sField = "CID,Match,Team_Home,Team_Away,Start_time,Score,Result,State,PayCent,PayCount,EndTime,other,Sale_StartTime,id,nowprize,nextprize,sysprize,sysprizestatue,sysdayprize";
            string sCondition = strWhere;
            string sOrder = "CID Desc";
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
                    return listBQCLists;
                }
                while (reader.Read())
                {
                    BCW.BQC.Model.BQCList objBQCList = new BCW.BQC.Model.BQCList();
                    objBQCList.CID = reader.GetInt32(0);
                    objBQCList.Match = reader.GetString(1);
                    objBQCList.Team_Home = reader.GetString(2);
                    objBQCList.Team_Away = reader.GetString(3);
                    objBQCList.Start_time = reader.GetString(4);
                    objBQCList.Score = reader.GetString(5);
                    objBQCList.Result = reader.GetString(6);
                    objBQCList.State = reader.GetInt32(7);
                    objBQCList.PayCent = reader.GetInt64(8);
                    objBQCList.PayCount = reader.GetInt32(9);
                    objBQCList.EndTime = reader.GetDateTime(10);
                    objBQCList.other = reader.GetString(11);
                    objBQCList.Sale_StartTime = reader.GetDateTime(12);
                    objBQCList.id = reader.GetInt32(13);
                    objBQCList.nowprize = reader.GetInt64(14);
                    objBQCList.nextprize = reader.GetInt64(15);
                    objBQCList.sysprize = reader.GetInt64(16);
                    objBQCList.sysprizestatue = reader.GetInt32(17);
                    objBQCList.sysdayprize = reader.GetInt64(18);
                    listBQCLists.Add(objBQCList);
                }
            }
            return listBQCLists;
        }

        #endregion  ��Ա����
    }
}

