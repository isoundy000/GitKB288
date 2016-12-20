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
    /// 数据访问类Brag。
    /// </summary>
    public class Brag
    {
        public Brag()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Brag");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Brag");
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
            strSql.Append("select count(1) from tb_Brag");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and ((TrueType<>ChooseType ");
            strSql.Append(" and State=1) OR State=3) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 计算某状态的吹牛数量
        /// </summary>
        public int GetCountState(int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Brag");
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
        /// 计算某用户今天吹牛数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Brag");
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
        /// 计算某用户今天吹牛记录数量
        /// </summary>
        public int GetCount2(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Brag");
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
        /// 计算今天吹牛数量
        /// </summary>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Brag");
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
        /// 计算今天吹牛总币值
        /// </summary>
        public long GetPrice(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Price) from tb_Brag");
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
        /// 计算吹牛总币值
        /// </summary>
        public long GetPrice(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Price) from tb_Brag");
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
        public int Add(BCW.Model.Game.Brag model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Brag(");
            strSql.Append("Types,Title,BragA,BragB,TrueType,ChooseType,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,State)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@BragA,@BragB,@TrueType,@ChooseType,@StopTime,@UsID,@UsName,@ReID,@ReName,@Price,@BzType,@AddTime,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@BragA", SqlDbType.NVarChar,10),
					new SqlParameter("@BragB", SqlDbType.NVarChar,10),
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
            parameters[2].Value = model.BragA;
            parameters[3].Value = model.BragB;
            parameters[4].Value = model.TrueType;
            parameters[5].Value = model.ChooseType;
            parameters[6].Value = model.StopTime;
            parameters[7].Value = model.UsID;
            parameters[8].Value = model.UsName;
            parameters[9].Value = model.ReID;
            parameters[10].Value = model.ReName;
            parameters[11].Value = model.Price;
            parameters[12].Value = model.BzType;
            parameters[13].Value = model.AddTime;
            parameters[14].Value = model.State;

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
        public void Update(BCW.Model.Game.Brag model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Brag set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("BragA=@BragA,");
            strSql.Append("BragB=@BragB,");
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
					new SqlParameter("@BragA", SqlDbType.NVarChar,10),
					new SqlParameter("@BragB", SqlDbType.NVarChar,10),
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
            parameters[3].Value = model.BragA;
            parameters[4].Value = model.BragB;
            parameters[5].Value = model.TrueType;
            parameters[6].Value = model.ChooseType;
            parameters[7].Value = model.StopTime;
            parameters[8].Value = model.UsID;
            parameters[9].Value = model.UsName;
            parameters[10].Value = model.ReID;
            parameters[11].Value = model.ReName;
            parameters[12].Value = model.Price;
            parameters[13].Value = model.BzType;
            parameters[14].Value = model.AddTime;
            parameters[15].Value = model.ReTime;
            parameters[16].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(BCW.Model.Game.Brag model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Brag set ");
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
        /// 更新为公共吹牛
        /// </summary>
        public void UpdateState2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Brag set ");
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
            strSql.Append("update tb_Brag set ");
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
            strSql.Append("delete from tb_Brag ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Brag GetBrag(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,Title,BragA,BragB,TrueType,ChooseType,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,ReTime,State from tb_Brag ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Brag model = new BCW.Model.Game.Brag();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.BragA = reader.GetString(3);
                    model.BragB = reader.GetString(4);
                    model.TrueType = reader.GetByte(5);
                    model.ChooseType = reader.GetByte(6);
                    model.StopTime = reader.GetDateTime(7);
                    model.UsID = reader.GetInt32(8);
                    model.UsName = reader.GetString(9);
                    model.ReID = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.ReName = reader.GetString(11);
                    model.Price = reader.GetInt64(12);
                    model.BzType = reader.GetByte(13);
                    model.AddTime = reader.GetDateTime(14);
                    if (!reader.IsDBNull(15))
                        model.ReTime = reader.GetDateTime(15);

                    model.State = reader.GetByte(16);
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
            strSql.Append(" FROM tb_Brag ");
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
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.Game.Brag> GetBrags(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Brag> listBrags = new List<BCW.Model.Game.Brag>();
            string sTable = "tb_Brag";
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
                    return listBrags;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Brag objBrag = new BCW.Model.Game.Brag();
                    objBrag.ID = reader.GetInt32(0);
                    objBrag.Title = reader.GetString(1);
                    objBrag.Price = reader.GetInt64(2);
                    objBrag.BzType = reader.GetByte(3);

                    listBrags.Add(objBrag);
                }
            }
            return listBrags;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.Game.Brag> GetBrags(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Brag> listBrags = new List<BCW.Model.Game.Brag>();
            string sTable = "tb_Brag";
            string sPkey = "id";
            string sField = "ID,Types,Title,BragA,BragB,TrueType,ChooseType,StopTime,UsID,UsName,ReID,ReName,Price,BzType,AddTime,ReTime,State";
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
                    return listBrags;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Brag objBrag = new BCW.Model.Game.Brag();
                    objBrag.ID = reader.GetInt32(0);
                    objBrag.Types = reader.GetInt32(1);
                    objBrag.Title = reader.GetString(2);
                    objBrag.BragA = reader.GetString(3);
                    objBrag.BragB = reader.GetString(4);
                    objBrag.TrueType = reader.GetByte(5);
                    objBrag.ChooseType = reader.GetByte(6);
                    objBrag.StopTime = reader.GetDateTime(7);
                    objBrag.UsID = reader.GetInt32(8);
                    objBrag.UsName = reader.GetString(9);
                    objBrag.ReID = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        objBrag.ReName = reader.GetString(11);
                    objBrag.Price = reader.GetInt64(12);
                    objBrag.BzType = reader.GetByte(13);
                    objBrag.AddTime = reader.GetDateTime(14);
                    if (!reader.IsDBNull(15))
                        objBrag.ReTime = reader.GetDateTime(15);
                    objBrag.State = reader.GetByte(16);
                    listBrags.Add(objBrag);
                }
            }
            return listBrags;
        }

        #endregion  成员方法
    }
}
