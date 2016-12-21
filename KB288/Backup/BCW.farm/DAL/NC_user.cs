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
    /// 数据访问类NC_user。
    /// </summary>
    public class NC_user
    {
        public NC_user()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_user");
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_user(");
            strSql.Append("usid,Grade,Goid,Experience,ischan,iszhong,iscao,iswater,isinsect,isshou,isshifei,SignTime,SignTotal,SignKeep,tuditpye)");
            strSql.Append(" values (");
            strSql.Append("@usid,@Grade,@Goid,@Experience,@ischan,@iszhong,@iscao,@iswater,@isinsect,@isshou,@isshifei,@SignTime,@SignTotal,@SignKeep,@tuditpye)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@Grade", SqlDbType.Int,4),
                    new SqlParameter("@Goid", SqlDbType.BigInt,8),
                    new SqlParameter("@Experience", SqlDbType.BigInt,8),
                    new SqlParameter("@ischan", SqlDbType.Int,4),
                    new SqlParameter("@iszhong", SqlDbType.Int,4),
                    new SqlParameter("@iscao", SqlDbType.Int,4),
                    new SqlParameter("@iswater", SqlDbType.Int,4),
                    new SqlParameter("@isinsect", SqlDbType.Int,4),
                    new SqlParameter("@isshou", SqlDbType.Int,4),
                    new SqlParameter("@isshifei", SqlDbType.Int,4),
                    new SqlParameter("@SignTime", SqlDbType.DateTime),
                    new SqlParameter("@SignTotal", SqlDbType.Int,4),
                    new SqlParameter("@SignKeep", SqlDbType.Int,4),
                    new SqlParameter("@tuditpye", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.Grade;
            parameters[2].Value = model.Goid;
            parameters[3].Value = model.Experience;
            parameters[4].Value = model.ischan;
            parameters[5].Value = model.iszhong;
            parameters[6].Value = model.iscao;
            parameters[7].Value = model.iswater;
            parameters[8].Value = model.isinsect;
            parameters[9].Value = model.isshou;
            parameters[10].Value = model.isshifei;
            parameters[11].Value = model.SignTime;
            parameters[12].Value = model.SignTotal;
            parameters[13].Value = model.SignKeep;
            parameters[14].Value = model.tuditpye;

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
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("usid=@usid,");
            strSql.Append("Grade=@Grade,");
            strSql.Append("Goid=@Goid,");
            strSql.Append("Experience=@Experience,");
            strSql.Append("ischan=@ischan,");
            strSql.Append("iszhong=@iszhong,");
            strSql.Append("iscao=@iscao,");
            strSql.Append("iswater=@iswater,");
            strSql.Append("isinsect=@isinsect,");
            strSql.Append("isshou=@isshou,");
            strSql.Append("isshifei=@isshifei,");
            strSql.Append("SignTime=@SignTime,");
            strSql.Append("SignTotal=@SignTotal,");
            strSql.Append("SignKeep=@SignKeep,");
            strSql.Append("tuditpye=@tuditpye");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@Grade", SqlDbType.Int,4),
                    new SqlParameter("@Goid", SqlDbType.BigInt,8),
                    new SqlParameter("@Experience", SqlDbType.BigInt,8),
                    new SqlParameter("@ischan", SqlDbType.Int,4),
                    new SqlParameter("@iszhong", SqlDbType.Int,4),
                    new SqlParameter("@iscao", SqlDbType.Int,4),
                    new SqlParameter("@iswater", SqlDbType.Int,4),
                    new SqlParameter("@isinsect", SqlDbType.Int,4),
                    new SqlParameter("@isshou", SqlDbType.Int,4),
                    new SqlParameter("@isshifei", SqlDbType.Int,4),
                    new SqlParameter("@SignTime", SqlDbType.DateTime),
                    new SqlParameter("@SignTotal", SqlDbType.Int,4),
                    new SqlParameter("@SignKeep", SqlDbType.Int,4),
                    new SqlParameter("@tuditpye", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.Grade;
            parameters[3].Value = model.Goid;
            parameters[4].Value = model.Experience;
            parameters[5].Value = model.ischan;
            parameters[6].Value = model.iszhong;
            parameters[7].Value = model.iscao;
            parameters[8].Value = model.iswater;
            parameters[9].Value = model.isinsect;
            parameters[10].Value = model.isshou;
            parameters[11].Value = model.isshifei;
            parameters[12].Value = model.SignTime;
            parameters[13].Value = model.SignTotal;
            parameters[14].Value = model.SignKeep;
            parameters[15].Value = model.tuditpye;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_user ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_user GetNC_user(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,Grade,Goid,Experience,ischan,iszhong,iscao,iswater,isinsect,isshou,isshifei,SignTime,SignTotal,SignKeep,tuditpye,big_bozhong,big_bangmang,big_shihuai,updatetime from tb_NC_user ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_user model = new BCW.farm.Model.NC_user();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.Grade = reader.GetInt32(2);
                    model.Goid = reader.GetInt64(3);
                    model.Experience = reader.GetInt64(4);
                    model.ischan = reader.GetInt32(5);
                    model.iszhong = reader.GetInt32(6);
                    model.iscao = reader.GetInt32(7);
                    model.iswater = reader.GetInt32(8);
                    model.isinsect = reader.GetInt32(9);
                    model.isshou = reader.GetInt32(10);
                    model.isshifei = reader.GetInt32(11);
                    model.SignTime = reader.GetDateTime(12);
                    model.SignTotal = reader.GetInt32(13);
                    model.SignKeep = reader.GetInt32(14);
                    model.tuditpye = reader.GetInt32(15);
                    model.big_bozhong = reader.GetInt32(16);
                    model.big_bangmang = reader.GetInt32(17);
                    model.big_shihuai = reader.GetInt32(18);
                    model.updatetime = reader.GetDateTime(19);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_NC_user ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        ///// <summary>
        ///// 取得每页记录
        ///// </summary>
        ///// <param name="p_pageIndex">当前页</param>
        ///// <param name="p_pageSize">分页大小</param>
        ///// <param name="p_recordCount">返回总记录数</param>
        ///// <param name="strWhere">查询条件</param>
        ///// <returns>IList NC_user</returns>
        //public IList<BCW.farm.Model.NC_user> GetNC_users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        //{
        //    IList<BCW.farm.Model.NC_user> listNC_users = new List<BCW.farm.Model.NC_user>();
        //    string sTable = "tb_NC_user";
        //    string sPkey = "id";
        //    string sField = "ID,usid,Grade,Goid,Experience,ischan,iszhong,iscao,iswater,isinsect,isshou,isshifei,SignTime,SignTotal,SignKeep,tuditpye";
        //    string sCondition = strWhere;
        //    string sOrder = "ID Desc";
        //    int iSCounts = 0;
        //    using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
        //    {
        //        //计算总页数
        //        if (p_recordCount > 0)
        //        {
        //            int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
        //        }
        //        else
        //        {
        //            return listNC_users;
        //        }
        //        while (reader.Read())
        //        {
        //                BCW.farm.Model.NC_user objNC_user = new BCW.farm.Model.NC_user();
        //                objNC_user.ID = reader.GetInt32(0);
        //                objNC_user.usid = reader.GetInt32(1);
        //                objNC_user.Grade = reader.GetInt32(2);
        //                objNC_user.Goid = reader.GetInt64(3);
        //                objNC_user.Experience = reader.GetInt64(4);
        //                objNC_user.ischan = reader.GetInt32(5);
        //                objNC_user.iszhong = reader.GetInt32(6);
        //                objNC_user.iscao = reader.GetInt32(7);
        //                objNC_user.iswater = reader.GetInt32(8);
        //                objNC_user.isinsect = reader.GetInt32(9);
        //                objNC_user.isshou = reader.GetInt32(10);
        //                objNC_user.isshifei = reader.GetInt32(11);
        //                objNC_user.SignTime = reader.GetDateTime(12);
        //                objNC_user.SignTotal = reader.GetInt32(13);
        //                objNC_user.SignKeep = reader.GetInt32(14);
        //                objNC_user.tuditpye = reader.GetInt32(15);
        //                listNC_users.Add(objNC_user);
        //        }
        //    }
        //    return listNC_users;
        //}


        //====================================
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_user SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        ///   me_更新土地类型
        /// </summary>
        public void Update_tdlx(int usid, int tuditpye)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("tuditpye=@tuditpye ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tuditpye", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = tuditpye;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_增加一条数据
        /// </summary>
        public int Add_1(BCW.farm.Model.NC_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_user(");
            strSql.Append("usid,Grade,Goid,Experience,ischan,iszhong,iscao,iswater,isinsect,isshou,isshifei)");
            strSql.Append(" values (");
            strSql.Append("@usid,@Grade,@Goid,@Experience,@ischan,@iszhong,@iscao,@iswater,@isinsect,@isshou,@isshifei)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@Grade", SqlDbType.Int,4),
                    new SqlParameter("@Goid", SqlDbType.BigInt,8),
                    new SqlParameter("@Experience", SqlDbType.BigInt,8),
                    new SqlParameter("@ischan", SqlDbType.Int,4),
                    new SqlParameter("@iszhong", SqlDbType.Int,4),
                    new SqlParameter("@iscao", SqlDbType.Int,4),
                    new SqlParameter("@iswater", SqlDbType.Int,4),
                    new SqlParameter("@isinsect", SqlDbType.Int,4),
                    new SqlParameter("@isshou", SqlDbType.Int,4),
                    new SqlParameter("@isshifei", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.Grade;
            parameters[2].Value = model.Goid;
            parameters[3].Value = model.Experience;
            parameters[4].Value = model.ischan;
            parameters[5].Value = model.iszhong;
            parameters[6].Value = model.iscao;
            parameters[7].Value = model.iswater;
            parameters[8].Value = model.isinsect;
            parameters[9].Value = model.isshou;
            parameters[10].Value = model.isshifei;

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
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_user Get_user(int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            BCW.farm.Model.NC_user model = new BCW.farm.Model.NC_user();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.Grade = reader.GetInt32(2);
                    model.Goid = reader.GetInt64(3);
                    model.Experience = reader.GetInt64(4);
                    model.ischan = reader.GetInt32(5);
                    model.iszhong = reader.GetInt32(6);
                    model.iscao = reader.GetInt32(7);
                    model.iswater = reader.GetInt32(8);
                    model.isinsect = reader.GetInt32(9);
                    model.isshou = reader.GetInt32(10);
                    model.isshifei = reader.GetInt32(11);
                    model.SignTime = reader.GetDateTime(12);
                    model.SignTotal = reader.GetInt32(13);
                    model.SignKeep = reader.GetInt32(14);
                    model.tuditpye = reader.GetInt32(15);
                    model.big_bozhong = reader.GetInt32(16);
                    model.big_bangmang = reader.GetInt32(17);
                    model.big_shihuai = reader.GetInt32(18);
                    model.updatetime = reader.GetDateTime(19);
                    model.isbaitan = reader.GetInt32(20);
                    model.shoutype = reader.GetInt32(21);
                    model.big_shifei = reader.GetInt32(22);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_user");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_更新用户经验
        /// </summary>
        public void Update_Experience(int usid, long experience)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("Experience=Experience+@experience ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@experience", SqlDbType.BigInt,8)};
            parameters[0].Value = usid;
            parameters[1].Value = experience;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_更新施肥次数
        /// </summary>
        public void Update_shifeinum(int usid, int big_shifei)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("big_shifei=big_shifei+@big_shifei ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@big_shifei", SqlDbType.BigInt,8)};
            parameters[0].Value = usid;
            parameters[1].Value = big_shifei;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_更新用户等级——加
        /// </summary>
        public void Update_dengji(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("grade=grade+1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_更新用户等级——减
        /// </summary>
        public void Update_dengji2(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("grade=grade-1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_更新用户金币
        /// </summary>
        public void Update_jinbi(int usid, long iGoid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("Goid=Goid+@iGoid ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@iGoid", SqlDbType.BigInt,8)};
            parameters[0].Value = usid;
            parameters[1].Value = iGoid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_得到用户币
        /// </summary>
        public long GetGold(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Goid from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到使用化肥次数
        /// </summary>
        public int Get_hfnum(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_shifei from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到种草放虫次数 邵广林 20160826
        /// </summary>
        public int Get_zcfcnum(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_zfcishu from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到除草除虫次数 邵广林 20160826
        /// </summary>
        public int Get_ccccnum(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_cccishu from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到是否可以摆摊
        /// </summary>
        public long Get_baitang(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select isbaitan from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到等级
        /// </summary>
        public long GetGrade(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Grade from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到收割状态
        /// </summary>
        public int Getshoutype(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select shoutype from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到经验
        /// </summary>
        public long Getjingyan(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Experience from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到帮忙的经验
        /// </summary>
        public long Get_bmjingyan(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_bangmang from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到播种的经验
        /// </summary>
        public long Get_bzjingyan(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_bozhong from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到使坏的经验
        /// </summary>
        public long Get_shjingyan(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_shihuai from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到自己操作的经验
        /// </summary>
        public long Get_zjjingyan(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select big_zjcaozuo from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_得到农场寄语
        /// </summary>
        public string Get_jiyu(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select jiyu from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// me_是否一键除草
        /// </summary>
        public long Getchucao(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iscao from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_是否一键浇水
        /// </summary>
        public long Getjiaoshui(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iswater from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        /// me_是否一键除虫
        /// </summary>
        public long Getchuchong(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select isinsect from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        ///  me_开通一键除草
        /// </summary>
        public void Update_chucao_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("iscao=1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_开通一键浇水
        /// </summary>
        public void Update_jiaoshui_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("iswater=1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_开通一键除虫
        /// </summary>
        public void Update_chuchong_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("isinsect=1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否一键收获
        /// </summary>
        public long Getshou(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select isshou from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        ///  me_开通一键收获
        /// </summary>
        public void Update_shouhuo_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("isshou=1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否一键铲地
        /// </summary>
        public long Getchandi(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ischan from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        ///  me_开通一键铲地
        /// </summary>
        public void Update_chandi_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("ischan=1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否一键施肥
        /// </summary>
        public long Getshifei(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select isshifei from tb_NC_user ");
            strSql.Append(" where usid=@usid ");
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
        ///  me_开通一键施肥
        /// </summary>
        public void Update_shifei_1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("isshifei=1 ");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_得到签到信息
        /// </summary>
        public BCW.farm.Model.NC_user GetSignData(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SignTotal,SignKeep,SignTime from tb_NC_user ");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.farm.Model.NC_user model = new BCW.farm.Model.NC_user();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.SignTotal = reader.GetInt32(0);
                    model.SignKeep = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.SignTime = reader.GetDateTime(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_更新签到信息
        /// </summary>
        public void UpdateSingData(int ID, int SignTotal, int SignKeep)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_user set ");
            strSql.Append("SignTotal=@SignTotal, ");
            strSql.Append("SignKeep=@SignKeep, ");
            strSql.Append("SignTime=@SignTime ");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@SignTotal", SqlDbType.Int,4),
                    new SqlParameter("@SignKeep", SqlDbType.Int,4),
                    new SqlParameter("@SignTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = SignTotal;
            parameters[2].Value = SignKeep;
            parameters[3].Value = DateTime.Now;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_会员排行榜使用
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList User</returns>
        public IList<BCW.farm.Model.NC_user> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_user> listUsers = new List<BCW.farm.Model.NC_user>();
            string sTable = "tb_NC_user";
            string sPkey = "id";
            string sField = "*";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelperUser.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    if (p_recordCount > 100)
                        p_recordCount = 100;
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listUsers;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_user objNC_user = new BCW.farm.Model.NC_user();
                    objNC_user.ID = reader.GetInt32(0);
                    objNC_user.usid = reader.GetInt32(1);
                    objNC_user.Grade = reader.GetInt32(2);
                    objNC_user.Goid = reader.GetInt64(3);
                    objNC_user.Experience = reader.GetInt64(4);
                    objNC_user.ischan = reader.GetInt32(5);
                    objNC_user.iszhong = reader.GetInt32(6);
                    objNC_user.iscao = reader.GetInt32(7);
                    objNC_user.iswater = reader.GetInt32(8);
                    objNC_user.isinsect = reader.GetInt32(9);
                    objNC_user.isshou = reader.GetInt32(10);
                    objNC_user.isshifei = reader.GetInt32(11);
                    objNC_user.SignTime = reader.GetDateTime(12);
                    objNC_user.SignTotal = reader.GetInt32(13);
                    objNC_user.SignKeep = reader.GetInt32(14);
                    objNC_user.tuditpye = reader.GetInt32(15);
                    listUsers.Add(objNC_user);
                }
            }
            return listUsers;
        }
        //====================================



        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_user</returns>
        public IList<BCW.farm.Model.NC_user> GetNC_users(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_user> listNC_users = new List<BCW.farm.Model.NC_user>();
            string sTable = "tb_NC_user";
            string sPkey = "id";
            string sField = "*";
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
                    return listNC_users;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_user objNC_user = new BCW.farm.Model.NC_user();
                    objNC_user.ID = reader.GetInt32(0);
                    objNC_user.usid = reader.GetInt32(1);
                    objNC_user.Grade = reader.GetInt32(2);
                    objNC_user.Goid = reader.GetInt64(3);
                    objNC_user.Experience = reader.GetInt64(4);
                    objNC_user.ischan = reader.GetInt32(5);
                    objNC_user.iszhong = reader.GetInt32(6);
                    objNC_user.iscao = reader.GetInt32(7);
                    objNC_user.iswater = reader.GetInt32(8);
                    objNC_user.isinsect = reader.GetInt32(9);
                    objNC_user.isshou = reader.GetInt32(10);
                    objNC_user.isshifei = reader.GetInt32(11);
                    objNC_user.SignTime = reader.GetDateTime(12);
                    objNC_user.SignTotal = reader.GetInt32(13);
                    objNC_user.SignKeep = reader.GetInt32(14);
                    objNC_user.tuditpye = reader.GetInt32(15);
                    objNC_user.big_bozhong = reader.GetInt32(16);
                    objNC_user.big_bangmang = reader.GetInt32(17);
                    objNC_user.big_shihuai = reader.GetInt32(18);
                    objNC_user.updatetime = reader.GetDateTime(19);
                    objNC_user.isbaitan = reader.GetInt32(20);
                    objNC_user.shoutype = reader.GetInt32(21);
                    objNC_user.big_shifei = reader.GetInt32(22);
                    objNC_user.big_zjcaozuo = reader.GetInt32(23);
                    objNC_user.big_cccishu = reader.GetInt32(24);
                    objNC_user.big_zfcishu = reader.GetInt32(25);
                    objNC_user.zengsongnum = reader.GetInt32(26);
                    objNC_user.jiyu = reader.GetString(27);
                    listNC_users.Add(objNC_user);
                }
            }
            return listNC_users;
        }

        #endregion  成员方法
    }
}

