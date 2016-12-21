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
    /// 数据访问类Dxdice。
    /// </summary>
    public class Dxdice
    {
        public Dxdice()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("UsID", "tb_Dxdice");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Dxdice");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Dxdice");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and ((IsWin>0 ");
            strSql.Append(" and State=1) OR State=3) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某状态的掷骰数量
        /// </summary>
        public int GetCountState(int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Dxdice");
            strSql.Append(" where State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = State;

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
        /// 计算某用户今天掷骰数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Dxdice");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 计算某用户今天掷骰记录数量
        /// </summary>
        public int GetCount2(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Dxdice");
            strSql.Append(" where ReID=@ReID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 计算今天掷骰数量
        /// </summary>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Dxdice");
            strSql.Append(" where Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
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
        /// 计算今天掷骰总币值
        /// </summary>
        public long GetPrice(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Price) from tb_Dxdice");
            strSql.Append(" where BzType=@BzType ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@BzType", SqlDbType.Int,4)};
            parameters[0].Value = Types;
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
        /// 计算掷骰总币值
        /// </summary>
        public long GetPrice(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Price) from tb_Dxdice");
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Dxdice model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Dxdice(");
            strSql.Append("Types,DxdiceA,DxdiceB,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,IsWin,State)");
            strSql.Append(" values (");
            strSql.Append("@Types,@DxdiceA,@DxdiceB,@StopTime,@UsID,@UsName,@ReID,@ReName,@Price,@BzType,@AddTime,@IsWin,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DxdiceA", SqlDbType.NVarChar,10),
					new SqlParameter("@DxdiceB", SqlDbType.NVarChar,10),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsWin", SqlDbType.TinyInt,1),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.DxdiceA;
            parameters[2].Value = model.DxdiceB;
            parameters[3].Value = model.StopTime;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.ReID;
            parameters[7].Value = model.ReName;
            parameters[8].Value = model.Price;
            parameters[9].Value = model.BzType;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.IsWin;
            parameters[12].Value = model.State;

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
        public void Update(BCW.Model.Game.Dxdice model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Dxdice set ");
            strSql.Append("Types=@Types,");
            strSql.Append("DxdiceA=@DxdiceA,");
            strSql.Append("DxdiceB=@DxdiceB,");
            strSql.Append("StopTime=@StopTime,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ReID=@ReID,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("Price=@Price,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("ReTime=@ReTime,");
            strSql.Append("State=@State");
            strSql.Append(" where UsID=@UsID and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DxdiceA", SqlDbType.NVarChar,10),
					new SqlParameter("@DxdiceB", SqlDbType.NVarChar,10),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ReTime", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.DxdiceA;
            parameters[3].Value = model.DxdiceB;
            parameters[4].Value = model.StopTime;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.ReID;
            parameters[8].Value = model.ReName;
            parameters[9].Value = model.Price;
            parameters[10].Value = model.BzType;
            parameters[11].Value = model.AddTime;
            parameters[12].Value = model.ReTime;
            parameters[13].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(BCW.Model.Game.Dxdice model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Dxdice set ");
            strSql.Append("ReID=@ReID,");
            strSql.Append("DxdiceB=@DxdiceB,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("ReTime=@ReTime,");
            strSql.Append("IsWin=@IsWin,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@DxdiceB", SqlDbType.NVarChar,50),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReTime", SqlDbType.DateTime),
					new SqlParameter("@IsWin", SqlDbType.TinyInt,1),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ReID;
            parameters[2].Value = model.DxdiceB;
            parameters[3].Value = model.ReName;
            parameters[4].Value = model.ReTime;
            parameters[5].Value = model.IsWin;
            parameters[6].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新为公共掷骰
        /// </summary>
        public void UpdateState2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Dxdice set ");
            strSql.Append("Types=@Types,");
            strSql.Append("ReID=@ReID,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;
            parameters[2].Value = 0;
            parameters[3].Value = "";
            parameters[4].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Dxdice set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Dxdice ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Dxdice GetDxdice(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,DxdiceA,DxdiceB,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,ReTime,IsWin,State from tb_Dxdice ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Dxdice model = new BCW.Model.Game.Dxdice();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.DxdiceA = reader.GetString(2);
                    model.DxdiceB = reader.GetString(3);
                    model.StopTime = reader.GetDateTime(4);
                    model.UsID = reader.GetInt32(5);
                    model.UsName = reader.GetString(6);
                    model.ReID = reader.GetInt32(7);
                    model.ReName = reader.GetString(8);
                    model.Price = reader.GetInt64(9);
                    model.BzType = reader.GetByte(10);
                    model.AddTime = reader.GetDateTime(11);
                    if (!reader.IsDBNull(12))
                        model.ReTime = reader.GetDateTime(12);
                    model.IsWin = reader.GetByte(13);
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Dxdice ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Dxdice</returns>
        public IList<BCW.Model.Game.Dxdice> GetDxdices(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Dxdice> listDxdices = new List<BCW.Model.Game.Dxdice>();
            string sTable = "tb_Dxdice";
            string sPkey = "id";
            string sField = "ID,UsName,Price,BzType";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listDxdices;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Dxdice objDxdice = new BCW.Model.Game.Dxdice();
                    objDxdice.ID = reader.GetInt32(0);
                    objDxdice.UsName = reader.GetString(1);
                    objDxdice.Price = reader.GetInt64(2);
                    objDxdice.BzType = reader.GetByte(3);

                    listDxdices.Add(objDxdice);
                }
            }
            return listDxdices;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Dxdice</returns>
        public IList<BCW.Model.Game.Dxdice> GetDxdices(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Dxdice> listDxdices = new List<BCW.Model.Game.Dxdice>();
            string sTable = "tb_Dxdice";
            string sPkey = "id";
            string sField = "ID,Types,DxdiceA,DxdiceB,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,IsWin,State";
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
                    return listDxdices;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Dxdice objDxdice = new BCW.Model.Game.Dxdice();
                    objDxdice.ID = reader.GetInt32(0);
                    objDxdice.Types = reader.GetInt32(1);
                    objDxdice.DxdiceA = reader.GetString(2);
                    objDxdice.DxdiceB = reader.GetString(3);
                    objDxdice.StopTime = reader.GetDateTime(4);
                    objDxdice.UsID = reader.GetInt32(5);
                    objDxdice.UsName = reader.GetString(6);
                    objDxdice.ReID = reader.GetInt32(7);
                    objDxdice.ReName = reader.GetString(8);
                    objDxdice.Price = reader.GetInt64(9);
                    objDxdice.BzType = reader.GetByte(10);
                    objDxdice.AddTime = reader.GetDateTime(11);
                    objDxdice.IsWin = reader.GetByte(12);
                    objDxdice.State = reader.GetByte(13);
                    listDxdices.Add(objDxdice);
                }
            }
            return listDxdices;
        }

        #endregion  成员方法
    }
}

