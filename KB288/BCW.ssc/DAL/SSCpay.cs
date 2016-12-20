using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.ssc.DAL
{
    /// <summary>
    /// 数据访问类SSCpay。
    /// </summary>
    public class SSCpay
    {
        public SSCpay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_SSCpay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSCpay");
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
            strSql.Append("select count(1) from tb_SSCpay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在机器人
        /// </summary>
        public bool ExistsReBot(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSCpay");
            strSql.Append(" where ID=@ID and UsID=@UsID and IsSpier=1");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Prices) from tb_SSCpay");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        /// 根据条件计算返彩值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(WinCent) from tb_SSCpay");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        public int Add(BCW.ssc.Model.SSCpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SSCpay(");
            strSql.Append("Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,Odds,WinNotes)");
            strSql.Append(" values (");
            strSql.Append("@Types,@SSCId,@UsID,@UsName,@Price,@iCount,@Notes,@Result,@Prices,@WinCent,@State,@AddTime,@Odds,@WinNotes)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                                       new SqlParameter("@Odds", SqlDbType.Decimal,9),
                                      new SqlParameter("@WinNotes", SqlDbType.NVarChar)  };
            parameters[0].Value = model.Types;
            parameters[1].Value = model.SSCId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.iCount;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.Result;
            parameters[8].Value = model.Prices;
            parameters[9].Value = model.WinCent;
            parameters[10].Value = model.State;
            parameters[11].Value = model.AddTime;
            parameters[12].Value = model.Odds;
            parameters[13].Value = model.WinNotes;

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
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.ssc.Model.SSCpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SSCpay(");
            strSql.Append("Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,IsSpier,Odds,WinNotes)");
            strSql.Append(" values (");
            strSql.Append("@Types,@SSCId,@UsID,@UsName,@Price,@iCount,@Notes,@Result,@Prices,@WinCent,@State,@AddTime,@IsSpier,@Odds,@WinNotes)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsSpier", SqlDbType.TinyInt,1),
                                        new SqlParameter("@Odds", SqlDbType.Decimal,9),
                                     new SqlParameter("@WinNotes", SqlDbType.NVarChar)  };
            parameters[0].Value = model.Types;
            parameters[1].Value = model.SSCId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.iCount;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.Result;
            parameters[8].Value = model.Prices;
            parameters[9].Value = model.WinCent;
            parameters[10].Value = model.State;
            parameters[11].Value = model.AddTime;
            parameters[12].Value = model.IsSpier;
            parameters[13].Value = model.Odds;
            parameters[14].Value = model.WinNotes;

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
        public void Update(BCW.ssc.Model.SSCpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("Types=@Types,");
            strSql.Append("SSCId=@SSCId,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Price=@Price,");
            strSql.Append("iCount=@iCount,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("Result=@Result,");
            strSql.Append("Prices=@Prices,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("Odds=@Odds");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                                        new SqlParameter("@Odds", SqlDbType.Decimal,9)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.SSCId;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Price;
            parameters[6].Value = model.iCount;
            parameters[7].Value = model.Notes;
            parameters[8].Value = model.Result;
            parameters[9].Value = model.Prices;
            parameters[10].Value = model.WinCent;
            parameters[11].Value = model.State;
            parameters[12].Value = model.AddTime;
            parameters[13].Value = model.Odds;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID and State<>2");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState1(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新赢钱
        /// </summary>
        public void UpdateWincent(int ID, long WinCent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("WinCent=@WinCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@WinCent", SqlDbType.Int,8)};
            parameters[0].Value = ID;
            parameters[1].Value = WinCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新时时猜开奖结果
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("Result=@Result");
            strSql.Append(" where SSCId=@SSCId and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = SSCId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新时时猜开奖结果
        /// </summary>
        public void UpdateResult1(int SSCId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("Result=@Result");
            strSql.Append(" where SSCId=@SSCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = SSCId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新时时猜开奖结果
        /// </summary>
        public void UpdateWinNotes(int ID, string WinNotes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("WinNotes=@WinNotes");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNotes", SqlDbType.NVarChar)};
            parameters[0].Value = ID;
            parameters[1].Value = WinNotes;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新游戏开奖得币
        /// </summary>
        public void UpdateWinCent(int ID, long WinCent, string WinNotes)
        {
            string oldNotes = GetWinNotes(ID) + "#";

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("WinCent=WinCent+@WinCent,");
            strSql.Append("WinNotes=@WinNotes");
            strSql.Append(" where ID=@ID and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinNotes", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = WinCent;
            parameters[2].Value = WinNotes;//oldNotes + 

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        ///某期某种投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes(int Types, int SSCId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(Prices) from tb_SSCpay ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and SSCId=@SSCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = SSCId;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///某期有牛无牛投了多少币
        /// </summary>
        public long GetSumPriceby23(int Types, int SSCId,int X)
        {
            if (X == 1)//有牛
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='有牛' ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else //无牛
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='无牛'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        ///某期五门投了多少币
        /// </summary>
        public long GetSumPriceby27(int Types, int SSCId, int X)
        {
            if (X == 1)//一门
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='一门' ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if(X==2) //二门
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='二门'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (X == 3) //三门
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='三门'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (X == 4) //四门
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='四门'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else  //五门
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='五门'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        ///某期大小投了多少币
        /// </summary>
        public long GetSumPricebyDX(int Types, int SSCId, int X)
        {
            if (X == 1)//大
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='大' ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else //小
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='小'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        ///某期单双投了多少币
        /// </summary>
        public long GetSumPricebyDS(int Types, int SSCId, int X)
        {
            if (X == 1)//单
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='单' ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else //双
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='双'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        ///某期单双投了多少币
        /// </summary>
        public long GetSumPricebyHD(int Types, int SSCId, int X)
        {
            if (X == 1)//大单
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='大单' ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else //大双
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='大双'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        ///某期单双投了多少币
        /// </summary>
        public long GetSumPricebyHDx(int Types, int SSCId, int X)
        {
            if (X == 1)//小单
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='小单' ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else //小双
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  Sum(Prices) from tb_SSCpay ");
                strSql.Append(" where Types=@Types ");
                strSql.Append(" and SSCId=@SSCId and Notes='小双'  ");
                SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
                parameters[0].Value = Types;
                parameters[1].Value = SSCId;
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return reader.GetInt64(0);
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SSCpay ");
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
            strSql.Append("delete from tb_SSCpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(Prices) from tb_SSCpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and SSCId=@SSCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = SSCId;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 某期某投注方式某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId,int ptype)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(Prices) from tb_SSCpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and SSCId=@SSCId  and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
                                       new SqlParameter("@Types", SqlDbType.Int,4) };
            parameters[0].Value = UsID;
            parameters[1].Value = SSCId;
            parameters[2].Value = ptype;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_SSCpay ");
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
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到一个State
        /// </summary>
        public long GetState(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 State from tb_SSCpay ");
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
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到一个IsSpier
        /// </summary>
        public int GetIsSpier(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IsSpier from tb_SSCpay ");
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
                        return reader.GetByte(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到一个WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinNotes from tb_SSCpay ");
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
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_SSCpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.ssc.Model.SSCpay GetSSCpay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,WinNotes,Odds from tb_SSCpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.ssc.Model.SSCpay model = new BCW.ssc.Model.SSCpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.SSCId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Price = reader.GetInt64(5);
                    model.iCount = reader.GetInt32(6);
                    model.Notes = reader.GetString(7);
                    model.Result = reader.GetString(8);
                    model.Prices = reader.GetInt64(9);
                    model.WinCent = reader.GetInt64(10);
                    model.State = reader.GetByte(11);
                    model.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.WinNotes = reader.GetString(13);
                    model.Odds = reader.GetDecimal(14);

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
            strSql.Append(" FROM tb_SSCpay ");
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
        /// <returns>IList SSCpay</returns>
        public IList<BCW.ssc.Model.SSCpay> GetSSCpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.ssc.Model.SSCpay> listSSCpays = new List<BCW.ssc.Model.SSCpay>();
            string sTable = "tb_SSCpay";
            string sPkey = "id";
            string sField = "ID,Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,Odds,WinNotes";
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
                    return listSSCpays;
                }
                while (reader.Read())
                { 
                     try
                    {
                    BCW.ssc.Model.SSCpay objSSCpay = new BCW.ssc.Model.SSCpay();
                    objSSCpay.ID = reader.GetInt32(0);
                    objSSCpay.Types = reader.GetInt32(1);
                    objSSCpay.SSCId = reader.GetInt32(2);
                    objSSCpay.UsID = reader.GetInt32(3);
                    objSSCpay.UsName = reader.GetString(4);
                    objSSCpay.Price = reader.GetInt64(5);
                    objSSCpay.iCount = reader.GetInt32(6);
                    objSSCpay.Notes = reader.GetString(7);
                    objSSCpay.Result = reader.GetString(8);
                    objSSCpay.Prices = reader.GetInt64(9);
                    objSSCpay.WinCent = reader.GetInt64(10);
                    objSSCpay.State = reader.GetByte(11);
                    objSSCpay.AddTime = reader.GetDateTime(12);
                    objSSCpay.Odds = reader.GetDecimal(13);
                    objSSCpay.WinNotes = reader.GetString(14);
                  
                        listSSCpays.Add(objSSCpay);
                    }
                    catch { }
                }
            }
            return listSSCpays;
        }

        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.ssc.Model.SSCpay> GetSSCpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.ssc.Model.SSCpay> listSSCpayTop = new List<BCW.ssc.Model.SSCpay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_SSCpay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listSSCpayTop;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "SELECT UsID,sum(WinCent-Prices) as WinCents FROM tb_SSCpay where " + strWhere + " group by UsID Order by sum(WinCent-Prices) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.ssc.Model.SSCpay objSSCpay = new BCW.ssc.Model.SSCpay();
                        objSSCpay.UsID = reader.GetInt32(0);
                        objSSCpay.WinCent = reader.GetInt64(1);
                        listSSCpayTop.Add(objSSCpay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listSSCpayTop;
        }


        #endregion  成员方法
    }
}

