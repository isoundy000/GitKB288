using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Marry。
    /// </summary>
    public class Marry
    {
        public Marry()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("UsID", "tb_Marry");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ReID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ReID=@ReID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ReID;
            parameters[2].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types>0 and ((UsID=@UsID ");
            strSql.Append(" and ReID=@ReID) ");
            strSql.Append(" or (UsID=@ReID ");
            strSql.Append(" and ReID=@UsID))");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ReID;

            return SqlHelper.Exists(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types=0 and ((UsID=@UsID ");
            strSql.Append(" and ReID=@ReID) ");
            strSql.Append(" or (UsID=@ReID ");
            strSql.Append(" and ReID=@UsID))");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ReID;

            return SqlHelper.Exists(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int UsID, int ReID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types=" + Types + " and ((UsID=@UsID ");
            strSql.Append(" and ReID=@ReID) ");
            strSql.Append(" or (UsID=@ReID ");
            strSql.Append(" and ReID=@UsID))");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ReID;

            return SqlHelper.Exists(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int UsID, int ReID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types=0 and State=@State and ((UsID=@UsID ");
            strSql.Append(" and ReID=@ReID) ");
            strSql.Append(" or (UsID=@ReID ");
            strSql.Append(" and ReID=@UsID))");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = State;
            parameters[1].Value = UsID;
            parameters[2].Value = ReID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists4(int UsID, int ReID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types=1 and State=@State and ((UsID=@UsID ");
            strSql.Append(" and ReID=@ReID) ");
            strSql.Append(" or (UsID=@ReID ");
            strSql.Append(" and ReID=@UsID))");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = State;
            parameters[1].Value = UsID;
            parameters[2].Value = ReID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在恋爱记录
        /// </summary>
        public bool ExistsLove(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types=0 and (UsID=@UsID ");
            strSql.Append(" or ReID=@UsID) ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);

        }
        /// <summary>
        /// 是否存在结婚记录
        /// </summary>
        public bool ExistsMarry(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types=1 and (UsID=@UsID ");
            strSql.Append(" or ReID=@UsID) ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 某会员是否存在非离婚的记录
        /// </summary>
        public bool ExistsLostMarry(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Marry");
            strSql.Append(" where Types<>2 and (UsID=@UsID ");
            strSql.Append(" or ReID=@UsID) ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Marry model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Marry(");
            strSql.Append("Types,UsID,UsName,UsSex,ReID,ReName,ReSex,Oath,IsParty,AddTime,AcUsID,State,LoveStat,HomeName)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@UsSex,@ReID,@ReName,@ReSex,@Oath,@IsParty,@AddTime,@AcUsID,@State,@LoveStat,@HomeName)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@UsSex", SqlDbType.TinyInt,1),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReSex", SqlDbType.TinyInt,1),
					new SqlParameter("@Oath", SqlDbType.NVarChar,50),
					new SqlParameter("@IsParty", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@AcUsID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@LoveStat", SqlDbType.NVarChar,50),
					new SqlParameter("@HomeName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.UsSex;
            parameters[4].Value = model.ReID;
            parameters[5].Value = model.ReName;
            parameters[6].Value = model.ReSex;
            parameters[7].Value = model.Oath;
            parameters[8].Value = model.IsParty;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.AcUsID;
            parameters[11].Value = model.State;
            parameters[12].Value = "0#0#0#0#0#0#0#0#0#0#1990-1-1#1990-1-1";
            parameters[13].Value = "浪漫花园";

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
        public void Update(BCW.Model.Marry model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("Types=@Types,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("UsSex=@UsSex,");
            strSql.Append("ReID=@ReID,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("ReSex=@ReSex,");
            strSql.Append("Oath=@Oath,");
            strSql.Append("IsParty=@IsParty,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("AcUsID=@AcUsID,");
            strSql.Append("AcTime=@AcTime");
            strSql.Append(" where UsID=@UsID and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@UsSex", SqlDbType.TinyInt,1),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReSex", SqlDbType.TinyInt,1),
					new SqlParameter("@Oath", SqlDbType.NVarChar,50),
					new SqlParameter("@IsParty", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@AcUsID", SqlDbType.Int,4),
					new SqlParameter("@AcTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.UsSex;
            parameters[5].Value = model.ReID;
            parameters[6].Value = model.ReName;
            parameters[7].Value = model.ReSex;
            parameters[8].Value = model.Oath;
            parameters[9].Value = model.IsParty;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.AcUsID;
            parameters[12].Value = model.AcTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新HomeClick
        /// </summary>
        public void UpdateHomeClick(int ID, int HomeClick)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("HomeClick=HomeClick+@HomeClick");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@HomeClick", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = HomeClick;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新LoveStat
        /// </summary>
        public void UpdateLoveStat(int ID, string LoveStat)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("LoveStat=@LoveStat");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LoveStat", SqlDbType.NVarChar,200)};
            parameters[0].Value = ID;
            parameters[1].Value = LoveStat;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新花园名称
        /// </summary>
        public void UpdateHomeName(int ID, string HomeName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("HomeName=@HomeName");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@HomeName", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = HomeName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新男誓言
        /// </summary>
        public void UpdateOath(int ID, string Oath)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("Oath=@Oath");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Oath", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Oath;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新女誓言
        /// </summary>
        public void UpdateOath2(int ID, string Oath2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("Oath2=@Oath2");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Oath2", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Oath2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新FlowStat、FlowTimes和鲜花数量
        /// </summary>
        public void UpdateFlowStat(int ID, string FlowStat, string FlowTimes, int FlowNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("FlowStat=@FlowStat,");
            strSql.Append("FlowTimes=@FlowTimes,");
            strSql.Append("FlowNum=FlowNum+@FlowNum");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@FlowStat", SqlDbType.NVarChar,500),
					new SqlParameter("@FlowTimes", SqlDbType.NVarChar,50),
					new SqlParameter("@FlowNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = FlowStat;
            parameters[2].Value = FlowTimes;
            parameters[3].Value = FlowNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 成为恋人
        /// </summary>
        public void UpdateLove(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("Types=@Types,AcTime='" + DateTime.Now + "' ");
            strSql.Append(" where UsID=@UsID and ReID=@ReID and Types=-1");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = 0;
            parameters[1].Value = UsID;
            parameters[2].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 成为夫妻
        /// </summary>
        public void UpdateMarry(int UsID, int ReID, string Oath)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("State=@State, ");
            strSql.Append("Oath=@Oath ");
            strSql.Append(" where ((UsID=@UsID and ReID=@ReID) or (UsID=@ReID and ReID=@UsID)) and Types=0");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Oath", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = 1;
            parameters[1].Value = Oath;
            parameters[2].Value = UsID;
            parameters[3].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 成为夫妻
        /// </summary>
        public void UpdateMarry(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("Types=@Types, ");
            strSql.Append("AcUsID=" + UsID + ", ");
            strSql.Append("AcTime='" + DateTime.Now + "', ");
            strSql.Append("State=0 ");
            strSql.Append(" where ((UsID=@UsID and ReID=@ReID) or (UsID=@ReID and ReID=@UsID)) and Types=0");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = 1;
            parameters[1].Value = UsID;
            parameters[2].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 取消求婚请求
        /// </summary>
        public void UpdateMarry2(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("State=0 ");
            strSql.Append(" where ((UsID=@UsID and ReID=@ReID) or (UsID=@ReID and ReID=@UsID)) and Types=0");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 成为离婚
        /// </summary>
        public void UpdateLost(int UsID, int ReID, string Oath2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("State=@State, ");
            strSql.Append("Oath2=@Oath2 ");
            strSql.Append(" where ((UsID=@UsID and ReID=@ReID) or (UsID=@ReID and ReID=@UsID)) and Types=1");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@Oath2", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = 1;
            parameters[1].Value = Oath2;
            parameters[2].Value = UsID;
            parameters[3].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 成为离婚
        /// </summary>
        public void UpdateLost(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("Types=@Types, ");
            strSql.Append("AcUsID=" + UsID + ", ");
            strSql.Append("AcTime='" + DateTime.Now + "', ");
            strSql.Append("State=0 ");
            strSql.Append(" where ((UsID=@UsID and ReID=@ReID) or (UsID=@ReID and ReID=@UsID)) and Types=1");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = 2;
            parameters[1].Value = UsID;
            parameters[2].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 取消离婚请求
        /// </summary>
        public void UpdateLost2(int UsID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("State=0 ");
            strSql.Append(" where ((UsID=@UsID and ReID=@ReID) or (UsID=@ReID and ReID=@UsID)) and Types=1");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ReID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新结婚证地址
        /// </summary>
        public void UpdateMarryPk(int ID, string MarryPk)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Marry set ");
            strSql.Append("MarryPk=@MarryPk ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@MarryPk", SqlDbType.NVarChar,100)};
            parameters[0].Value = ID;
            parameters[1].Value = MarryPk;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Marry ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Marry ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到鲜花排名
        /// </summary>
        public int GetFlowNumTop(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_Marry where FlowNum>=(select FlowNum from tb_Marry");
            strSql.Append(" where ID=@ID) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Marry GetMarry(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,UsSex,ReID,ReName,ReSex,Oath,Oath2,IsParty,AddTime,AcUsID,AcTime,State,LoveStat,HomeName,FlowNum,HomeClick,FlowStat,FlowTimes,MarryPk from tb_Marry ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Marry model = new BCW.Model.Marry();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.UsSex = reader.GetByte(4);
                    model.ReID = reader.GetInt32(5);
                    model.ReName = reader.GetString(6);
                    model.ReSex = reader.GetByte(7);
                    if (!reader.IsDBNull(8))
                        model.Oath = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        model.Oath2 = reader.GetString(9);

                    model.IsParty = reader.GetByte(10);
                    model.AddTime = reader.GetDateTime(11);
                    model.AcUsID = reader.GetInt32(12);
                    if (!reader.IsDBNull(13))
                        model.AcTime = reader.GetDateTime(13);

                    model.State = reader.GetByte(14);

                    if (!reader.IsDBNull(15))
                        model.LoveStat = reader.GetString(15);

                    if (!reader.IsDBNull(16))
                        model.HomeName = reader.GetString(16);

                    model.FlowNum = reader.GetInt32(17);
                    model.HomeClick = reader.GetInt32(18);
                    if (!reader.IsDBNull(19))
                        model.FlowStat = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                        model.FlowTimes = reader.GetString(20);
                    if (!reader.IsDBNull(21))
                        model.MarryPk = reader.GetString(21);
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
            strSql.Append(" FROM tb_Marry ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得排行榜记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Marry</returns>
        public IList<BCW.Model.Marry> GetMarrysTop(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Marry> listMarrys = new List<BCW.Model.Marry>();
            string sTable = "tb_Marry";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,ReID,ReName,HomeName,FlowNum,HomeClick";
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
                    return listMarrys;
                }
                while (reader.Read())
                {
                    BCW.Model.Marry objMarry = new BCW.Model.Marry();
                    objMarry.ID = reader.GetInt32(0);
                    objMarry.UsID = reader.GetInt32(1);
                    objMarry.UsName = reader.GetString(2);
                    objMarry.ReID = reader.GetInt32(3);
                    objMarry.ReName = reader.GetString(4);
                    objMarry.HomeName = reader.GetString(5);
                    objMarry.FlowNum = reader.GetInt32(6);
                    objMarry.HomeClick = reader.GetInt32(7);
 
                    listMarrys.Add(objMarry);
                }
            }
            return listMarrys;
        }


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Marry</returns>
        public IList<BCW.Model.Marry> GetMarrys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Marry> listMarrys = new List<BCW.Model.Marry>();
            string sTable = "tb_Marry";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,ReID,ReName,AcUsID,AcTime,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
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
                    return listMarrys;
                }
                while (reader.Read())
                {
                    BCW.Model.Marry objMarry = new BCW.Model.Marry();
                    objMarry.ID = reader.GetInt32(0);
                    objMarry.Types = reader.GetInt32(1);
                    objMarry.UsID = reader.GetInt32(2);
                    objMarry.UsName = reader.GetString(3);
                    objMarry.ReID = reader.GetInt32(4);
                    objMarry.ReName = reader.GetString(5);
                    objMarry.AcUsID = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                        objMarry.AcTime = reader.GetDateTime(7);

                    objMarry.AddTime = reader.GetDateTime(8);
                    listMarrys.Add(objMarry);
                }
            }
            return listMarrys;
        }

        #endregion  成员方法
    }
}

