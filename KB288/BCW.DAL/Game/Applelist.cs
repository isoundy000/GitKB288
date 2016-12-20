using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
    /// <summary>
    /// 数据访问类Applelist。
    /// </summary>
    public class Applelist
    {
        public Applelist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("State", "tb_Applelist");
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Applelist");
            strSql.Append(" where State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Applelist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int State, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Applelist");
            strSql.Append(" where State=@State and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = State;
            parameters[1].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Applelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Applelist(");
            strSql.Append("PingGuo,MuGua,XiGua,MangGuo,ShuangXing,JinZhong,ShuangQi,YuanBao,EndTime,OpenText,PayCent,WinCent,WinCount,State)");
            strSql.Append(" values (");
            strSql.Append("@PingGuo,@MuGua,@XiGua,@MangGuo,@ShuangXing,@JinZhong,@ShuangQi,@YuanBao,@EndTime,@OpenText,@PayCent,@WinCent,@WinCount,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@PingGuo", SqlDbType.Int,4),
					new SqlParameter("@MuGua", SqlDbType.Int,4),
					new SqlParameter("@XiGua", SqlDbType.Int,4),
					new SqlParameter("@MangGuo", SqlDbType.Int,4),
					new SqlParameter("@ShuangXing", SqlDbType.Int,4),
					new SqlParameter("@JinZhong", SqlDbType.Int,4),
					new SqlParameter("@ShuangQi", SqlDbType.Int,4),
					new SqlParameter("@YuanBao", SqlDbType.Int,4),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@OpenText", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.PingGuo;
            parameters[1].Value = model.MuGua;
            parameters[2].Value = model.XiGua;
            parameters[3].Value = model.MangGuo;
            parameters[4].Value = model.ShuangXing;
            parameters[5].Value = model.JinZhong;
            parameters[6].Value = model.ShuangQi;
            parameters[7].Value = model.YuanBao;
            parameters[8].Value = model.EndTime;
            parameters[9].Value = model.OpenText;
            parameters[10].Value = model.PayCent;
            parameters[11].Value = model.WinCent;
            parameters[12].Value = model.WinCount;
            parameters[13].Value = model.State;

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
        public void Update(BCW.Model.Game.Applelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applelist set ");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("OpenText=@OpenText");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@OpenText", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.EndTime;
            parameters[2].Value = model.OpenText;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新总下注额
        /// </summary>
        public void Update(int ID, long PayCent, int Types, int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applelist set ");
            strSql.Append("PayCent=PayCent+@PayCent,");
            if (Types == 1)
                strSql.Append("PingGuo=PingGuo+@Num");
            else if (Types == 2)
                strSql.Append("MuGua=MuGua+@Num");
            else if (Types == 3)
                strSql.Append("XiGua=XiGua+@Num");
            else if (Types == 4)
                strSql.Append("MangGuo=MangGuo+@Num");
            else if (Types == 5)
                strSql.Append("ShuangXing=ShuangXing+@Num");
            else if (Types == 6)
                strSql.Append("JinZhong=JinZhong+@Num");
            else if (Types == 7)
                strSql.Append("ShuangQi=ShuangQi+@Num");
            else if (Types == 8)
                strSql.Append("YuanBao=YuanBao+@Num");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Num", SqlDbType.Int,4),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = Num;
            parameters[2].Value = PayCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update2(BCW.Model.Game.Applelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applelist set ");
            strSql.Append("WinCount=@WinCount,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.WinCount;
            parameters[2].Value = model.WinCent;
            parameters[3].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Applelist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Applelist GetApplelist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,PingGuo,MuGua,XiGua,MangGuo,ShuangXing,JinZhong,ShuangQi,YuanBao,EndTime,OpenText,PayCent,WinCent,WinCount,State from tb_Applelist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Applelist model = new BCW.Model.Game.Applelist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.PingGuo = reader.GetInt32(1);
                    model.MuGua = reader.GetInt32(2);
                    model.XiGua = reader.GetInt32(3);
                    model.MangGuo = reader.GetInt32(4);
                    model.ShuangXing = reader.GetInt32(5);
                    model.JinZhong = reader.GetInt32(6);
                    model.ShuangQi = reader.GetInt32(7);
                    model.YuanBao = reader.GetInt32(8);
                    model.EndTime = reader.GetDateTime(9);
                    model.OpenText = reader.GetString(10);
                    model.PayCent = reader.GetInt64(11);
                    model.WinCent = reader.GetInt64(12);
                    model.WinCount = reader.GetInt32(13);
                    model.State = reader.GetByte(14);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个本期实体
        /// </summary>
        public BCW.Model.Game.Applelist GetApplelistBQ(int State)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,PingGuo,MuGua,XiGua,MangGuo,ShuangXing,JinZhong,ShuangQi,YuanBao,EndTime,OpenText,PayCent,WinCent,WinCount,State from tb_Applelist ");
            strSql.Append(" where State=" + State + "");
            strSql.Append(" Order by ID DESC");

            BCW.Model.Game.Applelist model = new BCW.Model.Game.Applelist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.PingGuo = reader.GetInt32(1);
                    model.MuGua = reader.GetInt32(2);
                    model.XiGua = reader.GetInt32(3);
                    model.MangGuo = reader.GetInt32(4);
                    model.ShuangXing = reader.GetInt32(5);
                    model.JinZhong = reader.GetInt32(6);
                    model.ShuangQi = reader.GetInt32(7);
                    model.YuanBao = reader.GetInt32(8);
                    model.EndTime = reader.GetDateTime(9);
                    model.OpenText = reader.GetString(10);
                    model.PayCent = reader.GetInt64(11);
                    model.WinCent = reader.GetInt64(12);
                    model.WinCount = reader.GetInt32(13);
                    model.State = reader.GetByte(14);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.PingGuo = 0;
                    model.MuGua = 0;
                    model.XiGua = 0;
                    model.MangGuo = 0;
                    model.ShuangXing = 0;
                    model.JinZhong = 0;
                    model.ShuangQi = 0;
                    model.YuanBao = 0;
                    model.EndTime = DateTime.Now;
                    model.OpenText = "";
                    model.PayCent = 0;
                    model.WinCent = 0;
                    model.WinCount = 0;
                    model.State = 0;
                    return model;
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
            strSql.Append(" FROM tb_Applelist ");
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
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Applelist</returns>
        public IList<BCW.Model.Game.Applelist> GetApplelists(int p_pageIndex, int p_pageSize, string strWhere, int Num, out int p_recordCount)
        {
            IList<BCW.Model.Game.Applelist> listApplelists = new List<BCW.Model.Game.Applelist>();
            string sTable = "tb_Applelist";
            string sPkey = "id";
            string sField = "ID,EndTime,OpenText,State";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = 0;
            if (Num > 0)
                iSCounts = Num;

            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listApplelists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Applelist objApplelist = new BCW.Model.Game.Applelist();
                    objApplelist.ID = reader.GetInt32(0);
                    objApplelist.EndTime = reader.GetDateTime(1);
                    objApplelist.OpenText = reader.GetString(2);
                    objApplelist.State = reader.GetByte(3);
                    listApplelists.Add(objApplelist);
                }
            }
            return listApplelists;
        }

        #endregion  成员方法
    }
}

