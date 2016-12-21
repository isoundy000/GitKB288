using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
/// <summary>
/// 修正人工封盘或系统封盘功能 黄国军 20161117 1610行
/// 修改设置是否自动开奖功能 黄国军 20160323
/// </summary>
namespace TPR2.DAL.guess
{
    /// <summary>
    /// 数据访问类BaList。
    /// </summary>
    public class BaList
    {
        public BaList()
        { }
        #region  成员方法

        public IList<TPR2.Model.guess.TBaListNew_History> GetHistory(int p_id, int ptype)
        {

            IList<TPR2.Model.guess.TBaListNew_History> listHistory = new List<TPR2.Model.guess.TBaListNew_History>();
            string sTable = "tb_TBaListNew_History";
            string sField = " id, Source, p_id, type, downloadtime, downloadtime_local, result, remark, v1, vs, v2, zdflag, lockflag";
            string sOrder = "downloadtime Desc";
            string sql = "SELECT " + sField + " FROM " + sTable + " WHERE p_id=" + p_id + " AND type=" + ptype + " Order by " + sOrder;
            DataSet ds = SqlHelper.Query(sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            TPR2.Model.guess.TBaListNew_History objHis = new TPR2.Model.guess.TBaListNew_History();
                            objHis.id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                            objHis.Source = int.Parse(ds.Tables[0].Rows[i]["Source"].ToString());
                            objHis.p_id = int.Parse(ds.Tables[0].Rows[i]["p_id"].ToString());
                            objHis.type = int.Parse(ds.Tables[0].Rows[i]["type"].ToString());
                            objHis.downloadtime = DateTime.Parse(ds.Tables[0].Rows[i]["downloadtime"].ToString());
                            objHis.result = ds.Tables[0].Rows[i]["result"].ToString();
                            objHis.remark = ds.Tables[0].Rows[i]["remark"].ToString();
                            objHis.v1 = decimal.Parse(ds.Tables[0].Rows[i]["v1"].ToString());
                            objHis.v2 = decimal.Parse(ds.Tables[0].Rows[i]["v2"].ToString());
                            objHis.vs = decimal.Parse(ds.Tables[0].Rows[i]["vs"].ToString());
                            objHis.zdflag = int.Parse(ds.Tables[0].Rows[i]["zdflag"].ToString());
                            objHis.lockflag = int.Parse(ds.Tables[0].Rows[i]["lockflag"].ToString());
                            listHistory.Add(objHis);
                        }
                    }
                }
            }
            return listHistory;
        }

        /// <summary>
        /// 得到是否单节半场比赛
        /// </summary>
        public int Getp_basketve(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_basketve from tb_TBaList ");
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
        /// 更新受限ID
        /// </summary>
        public void UpdatexID(int ID, string xID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            if (Types == 0)
                strSql.Append("xID0=@xID");
            else if (Types == 1)
                strSql.Append("xID1=@xID");
            else if (Types == 2)
                strSql.Append("xID2=@xID");
            else if (Types == 3)
                strSql.Append("xID3=@xID");
            else if (Types == 4)
                strSql.Append("xID4=@xID");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@xID", SqlDbType.NVarChar,2000)};
            parameters[0].Value = ID;
            parameters[1].Value = xID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新全局封盘
        /// </summary>
        public void Updatep_isluck(int ID, int p_isluck)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_isluck=@p_isluck");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_isluck", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = p_isluck;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateOnce(int p_basketve, int p_id, string p_once)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_once=@p_once");
            strSql.Append(" where p_id=@p_id and p_basketve=" + p_basketve + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_once;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            new TPR2.DAL.guess.BaListMe().UpdateOnce(p_id, p_once);

        }
        /// <summary>
        /// 更新单节完场比分
        /// </summary>
        public int UpdateBoResult(int p_basketve, int p_id, int p_result_one, int p_result_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two");

            strSql.Append(" where p_id=@p_id and p_basketve=" + p_basketve + "");
            strSql.Append(" and p_active=0 and p_result_one IS NULL AND p_result_two IS NULL");
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
        /// 更新单节即时比分
        /// </summary>
        public void UpdateBoResult3(int p_basketve, int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");
            strSql.Append(" where p_id=@p_id and p_basketve=" + p_basketve + "");
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
        /// 更新一条数据（单节使用）
        /// </summary>
        public void BasketUpdateOdds(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append(" and p_basketve=@p_basketve");
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
                    new SqlParameter("@p_basketve", SqlDbType.Int,4)};
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
            parameters[12].Value = model.p_basketve;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }
        /// <summary>
        /// 是不是走地
        /// </summary>
        public bool Existsp_ison(int p_id, int p_basketve)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TBaList");
            strSql.Append(" where p_id=@p_id and p_basketve=@p_basketve and p_ison=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_basketve", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_basketve;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新为滚球模式(足球半场)
        /// </summary>
        public void FootOnceType4(int p_id, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("p_oncetime=@p_oncetime");
            //strSql.Append(",p_del=1");
            strSql.Append(" where p_id=@p_id and p_ison=0 and p_basketve=9");
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
        /// 更新即时比分8波半场
        /// </summary>
        public void UpdateBoResultHalf(int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");

            strSql.Append(",p_temptime='" + DateTime.Now + "'");//记录比分更新的时间

            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_active=0  and p_basketve=9");
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
        /// 自动更新赛事比分
        /// </summary>
        public int UpdateZDResult2(TPR2.Model.guess.BaList model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_once=@p_once,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_basketve=@p_basketve ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_one", SqlDbType.Int,4),
                    new SqlParameter("@p_result_two", SqlDbType.Int,4),
                    new SqlParameter("@p_once", SqlDbType.NVarChar,200),
                    new SqlParameter("@p_basketve", SqlDbType.Int,4),
                    new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = model.p_id;
            parameters[1].Value = model.p_result_one;
            parameters[2].Value = model.p_result_two;
            parameters[3].Value = model.p_once;
            parameters[4].Value = model.p_basketve;
            parameters[5].Value = model.p_active;
            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append(" and p_basketve=9 and p_active=0");
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
        public void UpdateFalf1(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_pk=@p_pk,");
            strSql.Append("p_pn=@p_pn,");
            strSql.Append("p_one_lu=@p_one_lu,");
            strSql.Append("p_two_lu=@p_two_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_basketve=9 and p_active=0");
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
        public void UpdateFalf2(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_basketve=9 and p_active=0");
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
        public void UpdateFalf3(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=0");
            strSql.Append(" and p_basketve=9 and p_active=0");
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
        /// 更新主队红牌数量
        /// </summary>
        public void Updatep_hp_one(int p_id, int p_hp_one)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_hp_one=@p_hp_one");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_hp_one", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_hp_one;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新客队红牌数量
        /// </summary>
        public void Updatep_hp_two(int p_id, int p_hp_two)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_hp_two=@p_hp_two");
            strSql.Append(" where p_id=@p_id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_hp_two", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_hp_two;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        //---------------------------超级投注使用----------------------
        /// <summary>
        /// 更新已确定超级投注会员ID
        /// </summary>
        public void Updatep_usid(int ID, string p_usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append("select p_usid from tb_TBaList ");
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
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_TBaList");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TBaList");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否已开奖
        /// </summary>
        public bool ExistsIsOpen(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TBaList");
            strSql.Append(" where ID=@ID and p_active>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByp_id(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TBaList");
            strSql.Append(" where p_id=@p_id  and p_basketve=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByp_id(int p_id, int p_basketve)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TBaList");
            strSql.Append(" where p_id=@p_id and p_basketve=@p_basketve");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_basketve", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_basketve;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否已隐藏大小盘
        /// </summary>
        public bool ExistsDX(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TBaList");
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
            strSql.Append("select count(1) from tb_TBaList");
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
        /// 得到usid每场比赛的次数_邵广林20160815
        /// </summary>
        public int Getzqcount(int payusid, int bcid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT count(1) FROM tb_TBaPay WHERE payusid=@payusid AND bcid=@bcid");
            SqlParameter[] parameters = {
                    new SqlParameter("@payusid", SqlDbType.Int,4),
                    new SqlParameter("@bcid", SqlDbType.Int,4)};

            parameters[0].Value = payusid;
            parameters[1].Value = bcid;
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
        /// 得到查询的记录数
        /// </summary>
        public int GetCount(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_TBaList");
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
            strSql.Append("select count(ID) from tb_TBaList");
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
            strSql.Append("SELECT Count(ID) from tb_TBaList where " + strWhere + "");

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
        public int Add(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_TBaList(");
            strSql.Append("p_id,p_title,p_type,p_one,p_two,p_pk,p_dx_pk,p_pn,p_one_lu,p_two_lu,p_big_lu,p_small_lu,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_addtime,p_TPRtime,p_ison,p_del,p_basketve)");
            strSql.Append(" values (");
            strSql.Append("@p_id,@p_title,@p_type,@p_one,@p_two,@p_pk,@p_dx_pk,@p_pn,@p_one_lu,@p_two_lu,@p_big_lu,@p_small_lu,@p_bzs_lu,@p_bzp_lu,@p_bzx_lu,@p_addtime,@p_TPRtime,@p_ison,@p_del,@p_basketve)");
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
                    new SqlParameter("@p_basketve", SqlDbType.Int,4)};
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
            parameters[19].Value = model.p_basketve;

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
        public int FootAdd(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_TBaList(");
            strSql.Append("p_id,p_title,p_type,p_one,p_two,p_pk,p_pn,p_one_lu,p_two_lu,p_addtime,p_TPRtime,p_del)");
            strSql.Append(" values (");
            strSql.Append("@p_id,@p_title,@p_type,@p_one,@p_two,@p_pk,@p_pn,@p_one_lu,@p_two_lu,@p_addtime,@p_TPRtime,@p_del)");
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
                    new SqlParameter("@p_del", SqlDbType.Int,4)};
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
        public void Updatep_del(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_active=@p_active");
            strSql.Append(",p_opentime='" + DateTime.Now + "'");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = p_active;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新波胆
        /// </summary>
        public void Updatep_score(int ID, string p_score)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_score=@p_score");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_score", SqlDbType.NVarChar,500)};
            parameters[0].Value = ID;
            parameters[1].Value = p_score;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新波胆
        /// </summary>
        public void Updatep_score2(int p_id, string p_score)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_score=@p_score");
            strSql.Append(" where p_id=@p_id and p_jc=0 and p_basketve=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_score", SqlDbType.NVarChar,500)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_score;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新赛事抓取与不抓取
        /// </summary>
        public void Updatep_jc(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
        /// 更新赛事开奖与不开奖
        /// </summary>
        public void Updatep_dr(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_dr=@p_dr");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_dr", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_dr;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新赛事结果状态
        /// </summary>
        public void Updatep_zd(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
        public void UpdateResult(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
        public int UpdateZDResult(TPR2.Model.guess.BaList model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_once=@p_once,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_basketve=0 ");
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
        /// 更新完场比分8波
        /// </summary>
        public int UpdateBoResult(int p_id, int p_result_one, int p_result_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_basketve=0 ");
            strSql.Append(" and p_active=0 ");
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
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");

            strSql.Append(",p_temptime='" + DateTime.Now + "'");//记录比分更新的时间

            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_basketve=0 ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_result_temp1;
            parameters[2].Value = p_result_temp2;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            new TPR2.DAL.guess.BaListMe().UpdateBoResult2(p_id, p_result_temp1, p_result_temp2);
        }

        /// <summary>
        /// 更新即时比分8波
        /// </summary>
        public void UpdateBoResult3(int p_id, int p_result_temp1, int p_result_temp2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_basketve=0 ");
            strSql.Append(" and p_active=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4)};
            parameters[0].Value = p_id;
            parameters[1].Value = p_result_temp1;
            parameters[2].Value = p_result_temp2;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            new TPR2.DAL.guess.BaListMe().UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);
        }

        /// <summary>
        /// 更新进球时间集合
        /// </summary>
        public void UpdateOnce(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
        /// 更新比赛状态
        /// </summary>
        public void UpdateOnce(int p_id, string p_once)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_once=@p_once");

            try
            {
                if (Utils.ParseInt(p_once.Replace("'", "").Replace("+", "")) >= 90 || p_once.Contains("加"))
                {
                    strSql.Append(",p_isondel=1");//隐藏此滚球比赛
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

            new TPR2.DAL.guess.BaListMe().UpdateOnce(p_id, p_once);
        }



        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append("p_del=@p_del");
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
                    new SqlParameter("@p_del", SqlDbType.Int,4)};
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
            parameters[18].Value = model.p_del;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新特殊数据
        /// </summary>
        public void Update2(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("NeedU=@NeedU");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@NeedU", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NeedU;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void BasketUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append(" and p_basketve=@p_basketve");
            strSql.Append(" and p_jc=@p_jc");
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
                    new SqlParameter("@p_basketve", SqlDbType.Int,4),
                    new SqlParameter("@p_jc", SqlDbType.Int,4)};
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
            parameters[15].Value = model.p_basketve;
            parameters[16].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            //更新水位变化时间
            Updatep_updatetime(Convert.ToInt32(model.p_id));

            new TPR2.DAL.guess.BaListMe().BasketUpdate(model);
        }

        /// <summary>
        /// 更新足球让球盘数据
        /// </summary>
        public void FootUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            strSql.Append(" and p_jc=@p_jc and p_basketve=0");
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
                    new SqlParameter("@p_jc", SqlDbType.Int,4)};
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
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            //更新水位变化时间
            Updatep_updatetime(Convert.ToInt32(model.p_id));

            new TPR2.DAL.guess.BaListMe().FootUpdate(model);
        }

        /// <summary>
        /// 更新大小盘数据
        /// </summary>
        public void FootdxUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_dx_pk=@p_dx_pk,");
            strSql.Append("p_big_lu=@p_big_lu,");
            strSql.Append("p_small_lu=@p_small_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc and p_basketve=0");
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
            //更新水位变化时间
            Updatep_updatetime(Convert.ToInt32(model.p_id));

            new TPR2.DAL.guess.BaListMe().FootdxUpdate(model);
        }

        /// <summary>
        /// 更新足球标准盘数据
        /// </summary>
        public void FootbzUpdate(TPR2.Model.guess.BaList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_bzs_lu=@p_bzs_lu,");
            strSql.Append("p_bzp_lu=@p_bzp_lu,");
            strSql.Append("p_bzx_lu=@p_bzx_lu");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_jc=@p_jc and p_basketve=0");
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

            //更新水位变化时间
            Updatep_updatetime(Convert.ToInt32(model.p_id));

            new TPR2.DAL.guess.BaListMe().FootbzUpdate(model);
        }

        /// <summary>
        /// 更新手工开奖时间
        /// </summary>
        public void Updatep_opentime(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_opentime=@p_opentime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_opentime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新滚球比赛的水位变化时间
        /// </summary>
        public void Updatep_updatetime(int p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_updatetime=@p_updatetime");
            strSql.Append(" where p_id=@p_id ");
            strSql.Append(" and p_ison=@p_ison ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_updatetime", SqlDbType.DateTime)};
            parameters[0].Value = p_id;
            parameters[1].Value = 1;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新滚球比赛的水位变化时间
        /// </summary>
        public void Updatep_updatetime2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_updatetime=@p_updatetime");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and p_ison=@p_ison ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_updatetime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新为滚球模式
        /// </summary>
        public void FootOnceType(int ID, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
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
            //更新水位变化时间
            Updatep_updatetime2(ID);
        }

        /// <summary>
        /// 更新为滚球模式（足球使用）
        /// </summary>
        public void FootOnceType2(int p_id, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("p_oncetime=@p_oncetime");
            strSql.Append(" where p_id=@p_id and p_ison=0 and p_basketve=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_oncetime", SqlDbType.DateTime)};
            parameters[0].Value = p_id;
            parameters[1].Value = 1;
            parameters[2].Value = dt;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            //更新水位变化时间
            Updatep_updatetime(Convert.ToInt32(p_id));
        }

        /// <summary>
        /// 更新为滚球模式（篮球使用）
        /// </summary>
        public void FootOnceType3(int p_id, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_ison=@p_ison,");
            strSql.Append("p_oncetime=@p_oncetime");
            strSql.Append(" where p_id=@p_id and p_ison=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4),
                    new SqlParameter("@p_ison", SqlDbType.Int,4),
                    new SqlParameter("@p_oncetime", SqlDbType.DateTime)};
            parameters[0].Value = p_id;
            parameters[1].Value = 1;
            parameters[2].Value = dt.AddMinutes(20);

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新是否封盘
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types, int p_basketve, DateTime p_temptime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            if (Types == 1)
            {
                strSql.Append("p_isluckone=" + state + "");
                if (state > 1)
                    strSql.Append(",p_temptime1='" + p_temptime + "'");//记录让球盘封盘的时间

            }
            else if (Types == 2)
            {
                strSql.Append("p_islucktwo=" + state + "");
                if (state > 1)
                    strSql.Append(",p_temptime2='" + p_temptime + "'");//记录大小盘封盘的时间
            }
            else
            {
                strSql.Append("p_isluckthr=" + state + "");
                if (state > 1)
                    strSql.Append(",p_temptime3='" + p_temptime + "'");//记录标准盘封盘的时间
            }

            strSql.Append(" where p_id=@p_id and p_basketve=" + p_basketve + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            new TPR2.DAL.guess.BaListMe().Updatep_isluck(p_id, state, Types);
        }

        /// <summary>
        /// 更新是否封盘 20161117 黄国军
        /// </summary>
        public void Updatep_isluck2(int id, int state, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            if (Types == 1)
            {
                strSql.Append("p_isluckone=" + state + "");
                if (state > 1)
                    strSql.Append(",p_temptime1='" + DateTime.Now + "'");//记录让球盘封盘的时间

            }
            else if (Types == 2)
            {
                strSql.Append("p_islucktwo=" + state + "");
                if (state > 1)
                    strSql.Append(",p_temptime2='" + DateTime.Now + "'");//记录大小盘封盘的时间
            }
            else
            {
                strSql.Append("p_isluckthr=" + state + "");
                if (state > 1)
                    strSql.Append(",p_temptime3='" + DateTime.Now + "'");//记录标准盘封盘的时间
            }

            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            new TPR2.DAL.guess.BaListMe().Updatep_isluck2(id, state, Types);
        }

        /// <summary>
        /// 更新即时比分（滚球使用）
        /// </summary>
        public void FootOnceUpdate(TPR2.Model.guess.BaList model)
        {
            //个人庄同步比分
            new TPR2.BLL.guess.BaListMe().FootOnceUpdate(model);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TBaList set ");
            strSql.Append("p_temptime=@p_temptime,");

            if (model.p_temptime != DateTime.Parse("1990-1-1"))
                strSql.Append("p_temptimes = case when p_temptimes IS NULL then '" + model.p_temptime + "' else p_temptimes+'|" + model.p_temptime + "' END,");

            strSql.Append("p_result_temp1=@p_result_temp1,");
            strSql.Append("p_result_temp2=@p_result_temp2");
            strSql.Append(" where p_id=@p_id and p_basketve=0 ");
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
            strSql.Append("delete from tb_TBaList ");
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
            strSql.Append("delete from tb_TBaList ");
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
            strSql.Append("select p_type from tb_TBaList ");
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
        /// 得到p_id/第二种标准盘更新用
        /// </summary>
        public int Getp_id(DateTime p_TPRtime, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id from tb_TBaList ");
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
        public TPR2.Model.guess.BaList GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id,p_TPRtime,p_result_temp1,p_result_temp2 from tb_TBaList ");
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
            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
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
        public TPR2.Model.guess.BaList GetFootOpen(string p_title, string p_one, string p_two)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,p_id,p_TPRtime,p_result_temp1,p_result_temp2 from tb_TBaList ");
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

            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
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
        /// 得到是否开启滚球
        /// </summary>
        public int Getison(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_ison from tb_TBaList ");
            strSql.Append(" where p_id=@p_id  and p_basketve=0");
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
        /// 得到是否开启滚球
        /// </summary>
        public int Getisonht(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_ison from tb_TBaList ");
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
        /// 得到滚球更新时间集合
        /// </summary>
        public string Getonce(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_once from tb_TBaList ");
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
            strSql.Append("select p_temptimes from tb_TBaList ");
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
            strSql.Append("select p_temptime from tb_TBaList ");
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
        /// 得到p_temptime_p_id
        /// </summary>
        public TPR2.Model.guess.BaList Getp_temptime_p_id(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_id,p_temptime,p_temptime1,p_temptime2,p_temptime3 from tb_TBaList ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
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
        /// 得到比赛ID
        /// </summary>
        public int GetID(int p_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_TBaList ");
            strSql.Append(" where p_id=@p_id and p_basketve=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

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
        /// 得到联赛时间
        /// </summary>
        public DateTime Getp_TPRtime(int p_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_TPRtime from tb_TBaList ");
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
        public TPR2.Model.guess.BaList GetTemp(int p_id, int p_basketve)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p_result_temp1,p_result_temp2,p_temptime1,p_temptime2,p_temptime3,p_temptime from tb_TBaList ");
            strSql.Append(" where p_id=@p_id and p_basketve=" + p_basketve + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@p_id", SqlDbType.Int,4)};
            parameters[0].Value = p_id;

            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
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
        /// 得到一个对象实体
        /// </summary>
        public TPR2.Model.guess.BaList GetModel(int ID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,p_dr,p_id,p_title,p_type,p_one,p_two,p_pk,p_dx_pk,p_pn,p_one_lu,p_two_lu,p_big_lu,p_small_lu,p_bzs_lu,p_bzp_lu,p_bzx_lu,p_addtime,p_TPRtime,p_result_one,p_result_two,p_active,p_del,p_jc,p_ison,p_result_temp1,p_result_temp2,p_oncetime,p_temptime,p_isluckone,p_islucktwo,p_isluckthr,p_once,p_isondel,p_temptime1,p_temptime2,p_temptime3,p_score,p_opentime,p_hp_one,p_hp_two,p_basketve,xID0,xID1,xID2,xID3,xID4,p_isluck,NeedU from tb_TBaList ");

            if (Types == 0)
                strSql.Append(" where ID=" + ID + " ");
            else
                strSql.Append(" where p_id=" + ID + " ");


            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
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
                ///自动开奖字段
                if (ds.Tables[0].Rows[0]["p_dr"].ToString() != "")
                {
                    model.p_dr = int.Parse(ds.Tables[0].Rows[0]["p_dr"].ToString());
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
                if (ds.Tables[0].Rows[0]["p_isondel"].ToString() != "")
                {
                    model.p_isondel = int.Parse(ds.Tables[0].Rows[0]["p_isondel"].ToString());
                }
                else
                {
                    model.p_isondel = 0;
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
                model.p_score = ds.Tables[0].Rows[0]["p_score"].ToString();
                if (ds.Tables[0].Rows[0]["p_opentime"].ToString() != "")
                {
                    model.p_opentime = DateTime.Parse(ds.Tables[0].Rows[0]["p_opentime"].ToString());
                }
                else
                {
                    model.p_opentime = DateTime.Parse("1990-1-1");
                }
                if (ds.Tables[0].Rows[0]["p_hp_one"].ToString() != "")
                {
                    model.p_hp_one = int.Parse(ds.Tables[0].Rows[0]["p_hp_one"].ToString());
                }
                else
                {
                    model.p_hp_one = 0;
                }
                if (ds.Tables[0].Rows[0]["p_hp_two"].ToString() != "")
                {
                    model.p_hp_two = int.Parse(ds.Tables[0].Rows[0]["p_hp_two"].ToString());
                }
                else
                {
                    model.p_hp_two = 0;
                }
                model.p_basketve = int.Parse(ds.Tables[0].Rows[0]["p_basketve"].ToString());
                model.xID0 = ds.Tables[0].Rows[0]["xID0"].ToString();
                model.xID1 = ds.Tables[0].Rows[0]["xID1"].ToString();
                model.xID2 = ds.Tables[0].Rows[0]["xID2"].ToString();
                model.xID3 = ds.Tables[0].Rows[0]["xID3"].ToString();
                model.xID4 = ds.Tables[0].Rows[0]["xID4"].ToString();
                model.NeedU = int.Parse(ds.Tables[0].Rows[0]["NeedU"].ToString());
                if (ds.Tables[0].Rows[0]["p_isluck"].ToString() != "")
                {
                    model.p_isluck = int.Parse(ds.Tables[0].Rows[0]["p_isluck"].ToString());
                }
                else
                {
                    model.p_isluck = 0;
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取球赛封盘标识 1为系统封盘 2为人工封盘
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_basketve"></param>
        /// <returns></returns>
        public TPR2.Model.guess.BaList Getluck(int p_id, int p_basketve)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 p_temptime,p_isluckone,p_islucktwo,p_isluckthr from tb_TBaList ");
            strSql.Append(" where p_id=" + p_id + " and p_basketve=" + p_basketve);

            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
            DataSet ds = SqlHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {

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
            strSql.Append(" FROM tb_TBaList ");
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
        public IList<TPR2.Model.guess.BaList> GetBaLists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaList> listBaLists = new List<TPR2.Model.guess.BaList>();

            string sTable = "tb_TBaList";
            string sPkey = "id";
            string sField = "id,p_title,p_one,p_two,p_tprtime,p_result_one,p_result_two,p_active,p_del,p_zd,p_type,p_ison,p_result_temp1,p_result_temp2,p_usid,p_hp_one,p_hp_two,p_once,p_basketve,xID0,xID1,xID2,xID3,xID4,p_isluck";
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
                    TPR2.Model.guess.BaList objBaList = new TPR2.Model.guess.BaList();
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

                    if (reader.IsDBNull(15))
                        objBaList.p_hp_one = 0;
                    else
                        objBaList.p_hp_one = reader.GetInt32(15);

                    if (reader.IsDBNull(16))
                        objBaList.p_hp_two = 0;
                    else
                        objBaList.p_hp_two = reader.GetInt32(16);

                    if (!reader.IsDBNull(17))
                        objBaList.p_once = reader.GetString(17);

                    objBaList.p_basketve = reader.GetInt32(18);

                    if (reader.IsDBNull(19))
                        objBaList.xID0 = "";
                    else
                        objBaList.xID0 = reader.GetString(19);

                    if (reader.IsDBNull(20))
                        objBaList.xID1 = "";
                    else
                        objBaList.xID1 = reader.GetString(20);

                    if (reader.IsDBNull(21))
                        objBaList.xID2 = "";
                    else
                        objBaList.xID2 = reader.GetString(21);

                    if (reader.IsDBNull(22))
                        objBaList.xID3 = "";
                    else
                        objBaList.xID3 = reader.GetString(22);

                    if (reader.IsDBNull(23))
                        objBaList.xID4 = "";
                    else
                        objBaList.xID4 = reader.GetString(23);

                    objBaList.p_isluck = reader.GetInt32(24);
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
        public List<TPR2.Model.guess.BaList> GetBaListLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            List<TPR2.Model.guess.BaList> listBaListLX = new List<TPR2.Model.guess.BaList>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT p_title) FROM tb_TBaList where " + strWhere + "";

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

            string queryString = "SELECT DISTINCT p_title FROM tb_TBaList where " + strWhere + "";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        TPR2.Model.guess.BaList objBaListLX = new TPR2.Model.guess.BaList();
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
        public List<TPR2.Model.guess.BaList> GetBaListBF(string strWhere, out int p_recordCount)
        {
            List<TPR2.Model.guess.BaList> listBaListBF = new List<TPR2.Model.guess.BaList>();

            // 计算记录数
            string countString = "SELECT COUNT(ID) FROM tb_TBaList where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount <= 0)
            {
                return listBaListBF;
            }

            // 取出相关记录

            string queryString = "SELECT ID,p_id,p_TPRtime,p_one,p_result_one,p_result_two,p_ison,p_basketve,p_dr FROM tb_TBaList where " + strWhere + "";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int k = 0;
                while (reader.Read())
                {
                    TPR2.Model.guess.BaList objBaListBF = new TPR2.Model.guess.BaList();
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
                    objBaListBF.p_basketve = reader.GetInt32(7);
                    objBaListBF.p_dr = reader.GetInt32(8);
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

