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
    /// 数据访问类Mora。
    /// </summary>
    public class Mora
    {
        public Mora()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Mora");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Mora");
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
            strSql.Append("select count(1) from tb_Mora");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and (State=3 OR State=5 OR State=6) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 计算某用户今天猜拳数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Mora");
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
        /// 计算某用户今天猜拳记录数量
        /// </summary>
        public int GetCount2(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Mora");
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
        /// 计算今天猜拳数量
        /// </summary>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Mora");
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
        /// 计算今天猜拳总币值
        /// </summary>
        public long GetPrice(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Price) from tb_Mora");
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
        /// 计算猜拳总币值
        /// </summary>
        public long GetPrice(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Price) from tb_Mora");
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
        public int Add(BCW.Model.Game.Mora model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Mora(");
            strSql.Append("Types,Title,TrueType,ChooseType,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,State)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@TrueType,@ChooseType,@StopTime,@UsID,@UsName,@ReID,@ReName,@Price,@BzType,@AddTime,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@TrueType", SqlDbType.TinyInt,1),
					new SqlParameter("@ChooseType", SqlDbType.TinyInt,1),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.TrueType;
            parameters[3].Value = model.ChooseType;
            parameters[4].Value = model.StopTime;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.ReID;
            parameters[8].Value = model.ReName;
            parameters[9].Value = model.Price;
            parameters[10].Value = model.BzType;
            parameters[11].Value = model.AddTime;
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
        public void Update(BCW.Model.Game.Mora model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mora set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("TrueType=@TrueType,");
            strSql.Append("ChooseType=@ChooseType,");
            strSql.Append("StopTime=@StopTime,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ReID=@ReID,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("Price=@Price,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("ReTime=@ReTime,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@TrueType", SqlDbType.TinyInt,1),
					new SqlParameter("@ChooseType", SqlDbType.TinyInt,1),
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
            parameters[2].Value = model.Title;
            parameters[3].Value = model.TrueType;
            parameters[4].Value = model.ChooseType;
            parameters[5].Value = model.StopTime;
            parameters[6].Value = model.UsID;
            parameters[7].Value = model.UsName;
            parameters[8].Value = model.ReID;
            parameters[9].Value = model.ReName;
            parameters[10].Value = model.Price;
            parameters[11].Value = model.BzType;
            parameters[12].Value = model.AddTime;
            parameters[13].Value = model.ReTime;
            parameters[14].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(BCW.Model.Game.Mora model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mora set ");
            strSql.Append("ChooseType=@ChooseType,");
            strSql.Append("ReID=@ReID,");
            strSql.Append("ReName=@ReName,");
            strSql.Append("ReTime=@ReTime,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChooseType", SqlDbType.TinyInt,1),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReTime", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ChooseType;
            parameters[2].Value = model.ReID;
            parameters[3].Value = model.ReName;
            parameters[4].Value = model.ReTime;
            parameters[5].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新为公共猜拳
        /// </summary>
        public void UpdateState2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mora set ");
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
        /// 更新庄家出拳
        /// </summary>
        public void UpdateTrueType(int ID, int TrueType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mora set ");
            strSql.Append("TrueType=@TrueType");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TrueType", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = TrueType;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mora set ");
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
            strSql.Append("delete from tb_Mora ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Mora GetMora(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,Title,TrueType,ChooseType,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,ReTime,State from tb_Mora ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Mora model = new BCW.Model.Game.Mora();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.TrueType = reader.GetByte(3);
                    model.ChooseType = reader.GetByte(4);
                    model.StopTime = reader.GetDateTime(5);
                    model.UsID = reader.GetInt32(6);
                    model.UsName = reader.GetString(7);
                    model.ReID = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        model.ReName = reader.GetString(9);
                    model.Price = reader.GetInt64(10);
                    model.BzType = reader.GetByte(11);
                    model.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.ReTime = reader.GetDateTime(13);

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
            strSql.Append(" FROM tb_Mora ");
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
        /// <returns>IList Mora</returns>
        public IList<BCW.Model.Game.Mora> GetMoras(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Mora> listMoras = new List<BCW.Model.Game.Mora>();
            string sTable = "tb_Mora";
            string sPkey = "id";
            string sField = "ID,Title,Price,BzType";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listMoras;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Mora objMora = new BCW.Model.Game.Mora();
                    objMora.ID = reader.GetInt32(0);
                    objMora.Title = reader.GetString(1);
                    objMora.Price = reader.GetInt64(2);
                    objMora.BzType = reader.GetByte(3);

                    listMoras.Add(objMora);
                }
            }
            return listMoras;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Mora</returns>
        public IList<BCW.Model.Game.Mora> GetMoras(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Mora> listMoras = new List<BCW.Model.Game.Mora>();
            string sTable = "tb_Mora";
            string sPkey = "id";
            string sField = "ID,Types,Title,TrueType,ChooseType,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,ReTime,State";
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
                    return listMoras;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Mora objMora = new BCW.Model.Game.Mora();
                    objMora.ID = reader.GetInt32(0);
                    objMora.Types = reader.GetInt32(1);
                    objMora.Title = reader.GetString(2);
                    objMora.TrueType = reader.GetByte(3);
                    objMora.ChooseType = reader.GetByte(4);
                    objMora.StopTime = reader.GetDateTime(5);
                    objMora.UsID = reader.GetInt32(6);
                    objMora.UsName = reader.GetString(7);
                    objMora.ReID = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        objMora.ReName = reader.GetString(9);
                    objMora.Price = reader.GetInt64(10);
                    objMora.BzType = reader.GetByte(11);
                    objMora.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        objMora.ReTime = reader.GetDateTime(13);
                    objMora.State = reader.GetByte(14);
                    listMoras.Add(objMora);
                }
            }
            return listMoras;
        }

        #endregion  成员方法
    }
}
