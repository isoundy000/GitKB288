using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace TPR3.DAL.guess
{
    /// <summary>
    /// 数据访问类BaList。
    /// </summary>
    public class BaList
    {
        public BaList()
        { }
        #region  成员方法

        /// <summary>
        /// 是不是走地
        /// </summary>
        public bool Existsp_ison(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaList");
            strSql.Append(" where p_id=@p_id and p_ison=1");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
  
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu,");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_se=0");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_pk;
            parameters[2].Value = model.p_dx_pk;
            parameters[3].Value = model.p_pn;
            parameters[4].Value = model.p_one_lu;
            parameters[5].Value = model.p_two_lu;
            parameters[6].Value = model.p_big_lu;
            parameters[7].Value = model.p_small_lu;
            parameters[8].Value = model.p_bzs_lu;
            parameters[9].Value = model.p_bzp_lu;
            parameters[10].Value = model.p_bzx_lu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf1(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_active=0");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_pk;
            parameters[2].Value = model.p_pn;
            parameters[3].Value = model.p_one_lu;
            parameters[4].Value = model.p_two_lu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf2(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_active=0");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_dx_pk;
            parameters[2].Value = model.p_big_lu;
            parameters[3].Value = model.p_small_lu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf3(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_active=0");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_bzs_lu;
            parameters[2].Value = model.p_bzp_lu;
            parameters[3].Value = model.p_bzx_lu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新篮球让球盘
        /// </summary>
        public void BasketUpdateYp(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set "); ;
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_pk;
            parameters[2].Value = model.p_pn;
            parameters[3].Value = model.p_one_lu;
            parameters[4].Value = model.p_two_lu;
            parameters[5].Value = 0;
            parameters[6].Value = model.p_se;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);


        }

        /// <summary>
        /// 更新篮球大小盘
        /// </summary>
        public void BasketUpdateDx(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set "); ;
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_dx_pk;
            parameters[2].Value = model.p_big_lu;
            parameters[3].Value = model.p_small_lu;
            parameters[4].Value = 0;
            parameters[5].Value = model.p_se;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);


        }

        /// <summary>
        /// 更新为滚球模式（篮球使用）
        /// </summary>
        public void FootOnceType3(int p_id, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("p_oncetime=@p_oncetime");
            strSql.Append(" where p_id=@p_id and p_se=0 and p_ison=0");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_oncetime", SqlDbType.DateTime)};
            parameters[0].Value = p_id;
            parameters[1].Value = 1;
            parameters[2].Value = dt;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到联赛名称
        /// </summary>
        public string Getp_title(int p_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_title from tb_ZBaList ");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

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
        //---------------------------超级投注使用----------------------
        /// <summary>
        /// 更新已确定超级投注会员ID
        /// </summary>
        public void Updatep_usid(int ID, string p_usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 得到超级投注会员ID
        /// </summary>
        public string Getp_usid(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_usid from tb_ZBaList ");
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
        //---------------------------超级投注使用----------------------


        /// <summary>
        /// 得到p_temptime_p_id
        /// </summary>
        public TPR3.Model.guess.BaList Getp_temptime_p_id(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id,p_temptime,p_temptime1,p_temptime2,p_temptime3,p_se from tb_ZBaList ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
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

                    model.p_se = reader.GetInt32(5);

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_ZBaList");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaList");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByp_id(int p_id, int p_se)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaList");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_se=@p_se ");

            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_se;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否已隐藏大小盘
        /// </summary>
        public bool ExistsDX(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaList");
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
        /// 是否已隐藏标准盘
        /// </summary>
        public bool ExistsBZ(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaList");
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
        /// 得到查询的记录数
        /// </summary>
        public int GetCount(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_ZBaList");
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
        /// 得到查询的记录数
        /// </summary>
        public int GetCountByp_title(string p_title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_ZBaList");
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
        /// 根据条件得到赛事总注数
        /// </summary>
        public int GetBaListCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_ZBaList where " + strWhere + "");

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
        /// 增加一条数据
        /// </summary>
        public int Add(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ZBaList(");
            strSql.Append("p_id,p_title,p_type,p_one,p_two,p_pk,p_dx_pk,p_pn,p_one_lu,p_two_lu,p_big_lu,p_small_lu,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_addtime,p_TPRtime,p_ison,p_del,p_d_lu,p_s_lu,p_se)");
            strSql.Append(" values (");
            strSql.Append("@p_id,@p_title,@p_type,@p_one,@p_two,@p_pk,@p_dx_pk,@p_pn,@p_one_lu,@p_two_lu,@p_big_lu,@p_small_lu,@p_bzs_lu,@p_bzp_lu,@p_bzx_lu,@p_addtime,@p_TPRtime,@p_ison,@p_del,@p_d_lu,@p_s_lu,@p_se)");
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
					new SqlParameter("@p_d_lu", SqlDbType.Money,8),
					new SqlParameter("@p_s_lu", SqlDbType.Money,8),
					new SqlParameter("@p_se", SqlDbType.Int,4)};
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
            parameters[19].Value = model.p_d_lu;
            parameters[20].Value = model.p_s_lu;
            parameters[21].Value = model.p_se;

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
        /// 增加足球一条数据
        /// </summary>
        public int FootAdd(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ZBaList(");
            strSql.Append("p_id,p_title,p_type,p_one,p_two,p_pk,p_pn,p_one_lu,p_two_lu,p_addtime,p_TPRtime,p_del,p_d_lu,p_s_lu)");
            strSql.Append(" values (");
            strSql.Append("@p_id,@p_title,@p_type,@p_one,@p_two,@p_pk,@p_pn,@p_one_lu,@p_two_lu,@p_addtime,@p_TPRtime,@p_del,@p_d_lu,@p_s_lu)");
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
					new SqlParameter("@p_d_lu", SqlDbType.Money,8),
					new SqlParameter("@p_s_lu", SqlDbType.Money,8)};
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
            parameters[12].Value = model.p_d_lu;
            parameters[13].Value = model.p_s_lu;

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
        /// 更新赛事隐藏与显示
        /// </summary>
        public void Updatep_del(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 更新赛事状态结果
        /// </summary>
        public void Updatep_active(int ID, int p_active)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = p_active;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 更新赛事抓取与不抓取
        /// </summary>
        public void Updatep_jc(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 更新赛事结果状态
        /// </summary>
        public void Updatep_zd(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 更新赛事比分
        /// </summary>
        public void UpdateResult(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 自动更新赛事比分
        /// </summary>
        public int UpdateZDResult(TPR3.Model.guess.BaList model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_once=@p_once,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_se=@p_se ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_one", SqlDbType.Int,4),
                    new SqlParameter("@p_result_two", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200),
					new SqlParameter("@p_se", SqlDbType.Int,4),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_result_one;
            parameters[2].Value = model.p_result_two;
            parameters[3].Value = model.p_once;
            parameters[4].Value = model.p_se;
            parameters[5].Value = model.p_active;
            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新完场比分8波
        /// </summary>
        public int UpdateBoResult(int p_id, int p_result_one, int p_result_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_active=0 and p_se=0 and p_result_one IS NULL AND p_result_two IS NULL");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_one", SqlDbType.Int,4),
                    new SqlParameter("@p_result_two", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_result_one;
            parameters[2].Value = p_result_two;
            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新即时比分8波
        /// </summary>
        public void UpdateBoResult2(int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");

            strSql.Append(",p_temptime='" + DateTime.Now + "'");//记录比分更新的时间

            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_result_temp1;
            parameters[2].Value = p_result_temp2;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新即时比分8波
        /// </summary>
        public void UpdateBoResult3(int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新进球时间集合
        /// </summary>
        public void UpdateOnce(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 更新进球时间集合
        /// </summary>
        public void UpdateOnce(int p_id, string p_once)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_once=@p_once");
            try
            {
                if (Utils.ParseInt(p_once.Replace("'", "").Replace("+", "")) >= 90 || p_once.Contains("加"))
                {
                    strSql.Append(",p_isondel=1");//隐藏此滚球比赛

                    strSql.Append(",p_isluckone=1");

                    strSql.Append(",p_islucktwo=1");

                    strSql.Append(",p_isluckthr=1");
                }
            }
            catch { }

            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_once;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
            strSql.Append("p_d_lu=@p_d_lu,");
            strSql.Append("p_s_lu=@p_s_lu");
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
					new SqlParameter("@p_d_lu", SqlDbType.Money,8),
					new SqlParameter("@p_s_lu", SqlDbType.Money,8)};
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
            parameters[18].Value = model.p_d_lu;
            parameters[19].Value = model.p_s_lu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void BasketUpdate(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_title=@p_title,");
            strSql.Append("p_type=@p_type,");
            //strSql.Append("p_one=@p_one,");
            //strSql.Append("p_two=@p_two,");
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
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
					//new SqlParameter("@p_one", SqlDbType.NVarChar,50),
					//new SqlParameter("@p_two", SqlDbType.NVarChar,50),
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
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            //parameters[3].Value = model.p_one;
            //parameters[4].Value = model.p_two;
            parameters[3].Value = model.p_pk;
            parameters[4].Value = model.p_dx_pk;
            parameters[5].Value = model.p_pn;
            parameters[6].Value = model.p_one_lu;
            parameters[7].Value = model.p_two_lu;
            parameters[8].Value = model.p_big_lu;
            parameters[9].Value = model.p_small_lu;
            parameters[10].Value = model.p_bzs_lu;
            parameters[11].Value = model.p_bzp_lu;
            parameters[12].Value = model.p_bzx_lu;
            parameters[13].Value = model.p_addtime;
            parameters[14].Value = model.p_TPRtime;
            parameters[15].Value = 0;
            parameters[16].Value = model.p_se;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void BasketUpdateOdds(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu,");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
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
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_pk;
            parameters[2].Value = model.p_dx_pk;
            parameters[3].Value = model.p_pn;
            parameters[4].Value = model.p_one_lu;
            parameters[5].Value = model.p_two_lu;
            parameters[6].Value = model.p_big_lu;
            parameters[7].Value = model.p_small_lu;
            parameters[8].Value = model.p_bzs_lu;
            parameters[9].Value = model.p_bzp_lu;
            parameters[10].Value = model.p_bzx_lu;
            parameters[11].Value = 0;
            parameters[12].Value = model.p_se;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新足球让球盘数据
        /// </summary>
        public void FootUpdate(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_title=@p_title,");
            strSql.Append("p_type=@p_type,");
            //strSql.Append("p_one=@p_one,");
            //strSql.Append("p_two=@p_two,");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu,");
            strSql.Append("p_addtime=@p_addtime,");
            strSql.Append("p_TPRtime=@p_TPRtime");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_title", SqlDbType.NVarChar,50),
					new SqlParameter("@p_type", SqlDbType.Int,4),
                    //new SqlParameter("@p_one", SqlDbType.NVarChar,50),
                    //new SqlParameter("@p_two", SqlDbType.NVarChar,50),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
					new SqlParameter("@p_addtime", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime),
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_title;
            parameters[2].Value = model.p_type;
            //parameters[3].Value = model.p_one;
            //parameters[4].Value = model.p_two;
            parameters[3].Value = model.p_pk;
            parameters[4].Value = model.p_pn;
            parameters[5].Value = model.p_one_lu;
            parameters[6].Value = model.p_two_lu;
            parameters[7].Value = model.p_addtime;
            parameters[8].Value = model.p_TPRtime;
            parameters[9].Value = 0;
            parameters[10].Value = model.p_se;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新足球亚盘数据
        /// </summary>
        public void FootypUpdate(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@p_one_lu", SqlDbType.Money,8),
					new SqlParameter("@p_two_lu", SqlDbType.Money,8),
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_pk;
            parameters[2].Value = model.p_pn;
            parameters[3].Value = model.p_one_lu;
            parameters[4].Value = model.p_two_lu;
            parameters[5].Value = 0;
            parameters[6].Value = model.p_se;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新大小盘数据
        /// </summary>
        public void FootdxUpdate(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_big_lu", SqlDbType.Money,8),
					new SqlParameter("@p_small_lu", SqlDbType.Money,8),
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_dx_pk;
            parameters[2].Value = model.p_big_lu;
            parameters[3].Value = model.p_small_lu;
            parameters[4].Value = 0;
            parameters[5].Value = model.p_se;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        
        }

        /// <summary>
        /// 更新足球标准盘数据
        /// </summary>
        public void FootbzUpdate(TPR3.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc");
            strSql.Append(" and p_se=@p_se");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_bzs_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzp_lu", SqlDbType.Money,8),
					new SqlParameter("@p_bzx_lu", SqlDbType.Money,8),
            		new SqlParameter("@p_jc", SqlDbType.Int,4),
            		new SqlParameter("@p_se", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_bzs_lu;
            parameters[2].Value = model.p_bzp_lu;
            parameters[3].Value = model.p_bzx_lu;
            parameters[4].Value = 0;
            parameters[5].Value = model.p_se;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);


        }

        /// <summary>
        /// 更新为走地模式
        /// </summary>
        public void FootOnceType(int ID, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
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
        /// 更新为滚球模式
        /// </summary>
        public void FootOnceType2(int p_id, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("p_oncetime=@p_oncetime");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_oncetime", SqlDbType.DateTime)};
            parameters[0].Value = p_id;
            parameters[1].Value = 1;
            parameters[2].Value = dt;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新红牌
        /// </summary>
        public void UpdateRed(int p_id, int p_zred, int p_kred)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_zred=@p_zred,");
            strSql.Append("p_kred=@p_kred");
            strSql.Append(",p_temptime1='" + DateTime.Now + "'");
            strSql.Append(",p_temptime2='" + DateTime.Now + "'");
            strSql.Append(",p_temptime3='" + DateTime.Now + "'");
            strSql.Append(",p_redtime='" + DateTime.Now + "'");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_zred", SqlDbType.Int,4),
					new SqlParameter("@p_kred", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_zred;
            parameters[2].Value = p_kred;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新黄牌
        /// </summary>
        public void UpdateYellow(int p_id, int p_zyellow, int p_kyellow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_zyellow=@p_zyellow,");
            strSql.Append("p_kyellow=@p_kyellow");
            strSql.Append(",p_temptime1='" + DateTime.Now + "'");
            strSql.Append(",p_temptime2='" + DateTime.Now + "'");
            strSql.Append(",p_temptime3='" + DateTime.Now + "'");
            strSql.Append(",p_yellowtime='" + DateTime.Now + "'");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4),
					new SqlParameter("@p_zyellow", SqlDbType.Int,4),
					new SqlParameter("@p_kyellow", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_zyellow;
            parameters[2].Value = p_kyellow;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }


        /// <summary>
        /// 更新是否封盘
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types, DateTime p_temptime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            if (Types == 1)
            {
                strSql.Append("p_isluckone=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime1='" + p_temptime + "'");//记录让球盘封盘的时间

            }
            else if (Types == 2)
            {
                strSql.Append("p_islucktwo=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime2='" + p_temptime + "'");//记录大小盘封盘的时间
            }
            else
            {
                strSql.Append("p_isluckthr=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime3='" + p_temptime + "'");//记录标准盘封盘的时间
            }

            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新是否封盘
        /// </summary>
        public void Updatep_isluck2(int id, int state, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            if (Types == 1)
            {
                strSql.Append("p_isluckone=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime1='" + DateTime.Now + "'");//记录让球盘封盘的时间

            }
            else if (Types == 2)
            {
                strSql.Append("p_islucktwo=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime2='" + DateTime.Now + "'");//记录大小盘封盘的时间
            }
            else
            {
                strSql.Append("p_isluckthr=" + state + "");
                if (state == 1)
                    strSql.Append(",p_temptime3='" + DateTime.Now + "'");//记录标准盘封盘的时间
            }

            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        
        }

        /// <summary>
        /// 更新即时比分（走地使用）
        /// </summary>
        public void FootOnceUpdate(TPR3.Model.guess.BaList model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaList set ");
            strSql.Append("p_temptime=@p_temptime,");

            if (model.p_temptime != DateTime.Parse("1990-1-1"))
                strSql.Append("p_temptimes = case when p_temptimes IS NULL then '" + model.p_temptime + "' else p_temptimes+'|" + model.p_temptime + "' END,");

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
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZBaList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZBaList ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到球类型
        /// </summary>
        public int GetPtype(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_type from tb_ZBaList ");
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
        /// 得到p_id
        /// </summary>
        public int Getp_id(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id from tb_ZBaList ");
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
        /// 得到p_id/第二种标准盘更新用
        /// </summary>
        public int Getp_id(DateTime p_TPRtime, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id from tb_ZBaList ");
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
        /// 得到篮球开奖所需的实体
        /// </summary>
        public TPR3.Model.guess.BaList GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id,p_TPRtime,p_result_temp1,p_result_temp2 from tb_ZBaList ");
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
            TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
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
        /// 得到足球开奖所需的实体
        /// </summary>
        public TPR3.Model.guess.BaList GetFootOpen(string p_title, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,p_id,p_TPRtime,p_result_temp1,p_result_temp2 from tb_ZBaList ");
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

            TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
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
        /// 得到是否开启走地
        /// </summary>
        public int Getison(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_ison from tb_ZBaList ");
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
        /// 得到是否开启走地
        /// </summary>
        public int Getisonht(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_ison from tb_ZBaList ");
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
        /// 得到走地更新时间集合
        /// </summary>
        public string Getonce(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_once from tb_ZBaList ");
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
        /// 得到比分更新时间集合
        /// </summary>
        public string Getp_temptimes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_temptimes from tb_ZBaList ");
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
        /// 得到最新比分更新时间
        /// </summary>
        public DateTime Getp_temptime(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_temptime from tb_ZBaList ");
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
        /// 得到联赛时间
        /// </summary>
        public DateTime Getp_TPRtime(int p_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_TPRtime from tb_ZBaList ");
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
        /// 得到即时比分和封盘时间
        /// </summary>
        public TPR3.Model.guess.BaList GetTemp(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_result_temp1,p_result_temp2,p_temptime1,p_temptime2,p_temptime3,p_temptime from tb_ZBaList ");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
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
        /// 得到红牌黄牌、即时比分
        /// </summary>
        public TPR3.Model.guess.BaList GetRedYellow(int p_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_zred,p_kred,p_zyellow,p_kyellow,p_result_temp1,p_result_temp2 from tb_ZBaList ");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.p_zred = reader.GetInt32(0);
                    model.p_kred = reader.GetInt32(1);
                    model.p_zyellow = reader.GetInt32(2);
                    model.p_kyellow = reader.GetInt32(3);

                    if (!reader.IsDBNull(4))
                        model.p_result_temp1 = reader.GetInt32(4);

                    if (!reader.IsDBNull(5))
                        model.p_result_temp2 = reader.GetInt32(5);

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TPR3.Model.guess.BaList GetModel(int ID,int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,p_id,p_title,p_type,p_one,p_two,p_pk,p_dx_pk,p_pn,p_one_lu,p_two_lu,p_big_lu,p_small_lu,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_addtime,p_TPRtime,p_result_one,p_result_two,p_active,p_del,p_jc,p_ison,p_result_temp1,p_result_temp2,p_oncetime,p_temptime,p_isluckone,p_islucktwo,p_isluckthr,p_once,p_d_lu,p_s_lu,p_temptime1,p_temptime2,p_temptime3,p_se,p_zred,p_kred,p_zyellow,p_kyellow,p_redtime,p_yellowtime from tb_ZBaList ");

            if (Types == -1)
                strSql.Append(" where ID=" + ID + " ");
            else
                strSql.Append(" where p_id=" + ID + " and p_se=" + Types + "");
         

            TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
            DataSet ds = SqlHelper.Query(strSql.ToString());
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
                else
                {
                    model.p_temptime = DateTime.Parse("1990-1-1");
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
                if (ds.Tables[0].Rows[0]["p_d_lu"].ToString() != "")
                {
                    model.p_d_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_d_lu"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_s_lu"].ToString() != "")
                {
                    model.p_s_lu = decimal.Parse(ds.Tables[0].Rows[0]["p_s_lu"].ToString());
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
                if (ds.Tables[0].Rows[0]["p_se"].ToString() != "")
                {
                    model.p_se = int.Parse(ds.Tables[0].Rows[0]["p_se"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_zred"].ToString() != "")
                {
                    model.p_zred = int.Parse(ds.Tables[0].Rows[0]["p_zred"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_kred"].ToString() != "")
                {
                    model.p_kred = int.Parse(ds.Tables[0].Rows[0]["p_kred"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_zyellow"].ToString() != "")
                {
                    model.p_zyellow = int.Parse(ds.Tables[0].Rows[0]["p_zyellow"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_kyellow"].ToString() != "")
                {
                    model.p_kyellow = int.Parse(ds.Tables[0].Rows[0]["p_kyellow"].ToString());
                }
                if (ds.Tables[0].Rows[0]["p_redtime"].ToString() != "")
                {
                    model.p_redtime = DateTime.Parse(ds.Tables[0].Rows[0]["p_redtime"].ToString());
                }
                else
                {
                    model.p_redtime = DateTime.Parse("1990-1-1");
                }
                if (ds.Tables[0].Rows[0]["p_yellowtime"].ToString() != "")
                {
                    model.p_yellowtime = DateTime.Parse(ds.Tables[0].Rows[0]["p_yellowtime"].ToString());
                }
                else
                {
                    model.p_yellowtime = DateTime.Parse("1990-1-1");
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaListList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_ZBaList ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaList</returns>
        public IList<TPR3.Model.guess.BaList> GetBaLists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaList> listBaLists = new List<TPR3.Model.guess.BaList>();

            string sTable = "tb_ZBaList";
            string sPkey = "id";
            string sField = "id,p_title,p_one,p_two,p_tprtime,p_result_one,p_result_two,p_active,p_del,p_zd,p_type,p_ison,p_result_temp1,p_result_temp2,p_usid,p_once,p_se,p_zred,p_kred,p_zyellow,p_kyellow,p_id";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts = 0;

            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {

                //计算总页数

                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {

                    return listBaLists;
                }

                while (reader.Read())
                {
                    TPR3.Model.guess.BaList objBaList = new TPR3.Model.guess.BaList();
                    objBaList.ID = reader.GetInt32(0);
                    objBaList.p_title = reader.GetString(1);
                    objBaList.p_one = reader.GetString(2);
                    objBaList.p_two = reader.GetString(3);
                    objBaList.p_TPRtime = reader.GetDateTime(4);

                    if (reader.IsDBNull(5))
                        objBaList.p_result_one = null;
                    else
                        objBaList.p_result_one = reader.GetInt32(5);

                    if (reader.IsDBNull(6))
                        objBaList.p_result_two = null;
                    else
                        objBaList.p_result_two = reader.GetInt32(6);

                    objBaList.p_active = reader.GetInt32(7);
                    objBaList.p_del = reader.GetInt32(8);
                    objBaList.p_zd = reader.GetInt32(9);
                    objBaList.p_type = reader.GetInt32(10);
                    objBaList.p_ison = reader.GetInt32(11);
                    if (reader.IsDBNull(12))
                        objBaList.p_result_temp1 = null;
                    else
                        objBaList.p_result_temp1 = reader.GetInt32(12);

                    if (reader.IsDBNull(13))
                        objBaList.p_result_temp2 = null;
                    else
                        objBaList.p_result_temp2 = reader.GetInt32(13);

                    if (!reader.IsDBNull(14))
                        objBaList.p_usid = reader.GetString(14);

                    if (!reader.IsDBNull(15))
                        objBaList.p_once = reader.GetString(15);

                    objBaList.p_se = reader.GetInt32(16);
                    objBaList.p_zred = reader.GetInt32(17);
                    objBaList.p_kred = reader.GetInt32(18);
                    objBaList.p_zyellow = reader.GetInt32(19);
                    objBaList.p_kyellow = reader.GetInt32(20);
                    objBaList.p_id = reader.GetInt32(21);
                    listBaLists.Add(objBaList);


                }
            }

            return listBaLists;
        }


        /// <summary>
        /// 取得联赛记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public List<TPR3.Model.guess.BaList> GetBaListLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            List<TPR3.Model.guess.BaList> listBaListLX = new List<TPR3.Model.guess.BaList>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT p_title) FROM tb_ZBaList where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBaListLX;
            }

            // 取出相关记录

            string queryString = "SELECT DISTINCT p_title FROM tb_ZBaList where " + strWhere + "";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        TPR3.Model.guess.BaList objBaListLX = new TPR3.Model.guess.BaList();
                        objBaListLX.p_title = reader.GetString(0);
                        listBaListLX.Add(objBaListLX);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBaListLX;
        }
        /// <summary>
        /// 取得未开奖的赛事
        /// </summary>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>List</returns>
        public List<TPR3.Model.guess.BaList> GetBaListBF(string strWhere, out int p_recordCount)
        {
            List<TPR3.Model.guess.BaList> listBaListBF = new List<TPR3.Model.guess.BaList>();

            // 计算记录数
            string countString = "SELECT COUNT(ID) FROM tb_ZBaList where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount <= 0)
            {
                return listBaListBF;
            }

            // 取出相关记录

            string queryString = "SELECT ID,p_id,p_TPRtime,p_one,p_result_one,p_result_two,p_ison,p_se FROM tb_ZBaList where " + strWhere + "";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int k = 0;
                while (reader.Read())
                {
                    TPR3.Model.guess.BaList objBaListBF = new TPR3.Model.guess.BaList();
                    objBaListBF.ID = reader.GetInt32(0);
                    objBaListBF.p_id = reader.GetInt32(1);
                    objBaListBF.p_TPRtime = reader.GetDateTime(2);
                    objBaListBF.p_one = reader.GetString(3);
                    if (reader.IsDBNull(4))
                        objBaListBF.p_result_one = null;
                    else
                        objBaListBF.p_result_one = reader.GetInt32(4);

                    if (reader.IsDBNull(5))
                        objBaListBF.p_result_two = null;
                    else
                        objBaListBF.p_result_two = reader.GetInt32(5);

                    objBaListBF.p_ison = reader.GetInt32(6);
                    objBaListBF.p_se = reader.GetInt32(7);

                    listBaListBF.Add(objBaListBF);

                    if (k == p_recordCount)
                        break;

                    k++;
                }
            }

            return listBaListBF;
        }

        #endregion  成员方法
    }
}

