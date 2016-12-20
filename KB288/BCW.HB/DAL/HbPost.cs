using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.HB.DAL
{
    public partial class HbPost
    {
        public HbPost()
        { }

        #region 成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HbPost");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HbPost");
            strSql.Append(" where ID=@ID and State=@State");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID",SqlDbType.Int,4),
                    new SqlParameter("@State",SqlDbType.Int,4)

            };
            parameters[0].Value = ID;
            parameters[1].Value = State;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int UserID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HbPost");
            strSql.Append(" where UserID=@UserID and State=@State");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID",SqlDbType.Int,4),
				    new SqlParameter("@State",SqlDbType.Int,4)
					
			};
            parameters[0].Value = UserID;
            parameters[1].Value = State;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(DISTINCT ChatID)  from tb_HbPost where ChatID!=0 ");
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
        public int Add(BCW.HB.Model.HbPost model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_HbPost(");
            strSql.Append("UserID,num,money,RadomNum,PostTime,GetIDList,MaxRadom,Note,ChatID)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@num,@money,@RadomNum,@PostTime,@GetIDList,@MaxRadom,@Note,@ChatID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@num", SqlDbType.Int,4),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@RadomNum", SqlDbType.NVarChar,-1),
					new SqlParameter("@PostTime", SqlDbType.DateTime),
					new SqlParameter("@GetIDList", SqlDbType.NVarChar,-1),
					new SqlParameter("@MaxRadom", SqlDbType.BigInt,8),
					new SqlParameter("@Note", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ChatID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.num;
            parameters[2].Value = model.money;
            parameters[3].Value = model.RadomNum;
            parameters[4].Value = model.PostTime;
            parameters[5].Value = model.GetIDList;
            parameters[6].Value = model.MaxRadom;
            parameters[7].Value = model.Note;
            parameters[8].Value = model.ChatID;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HB.Model.HbPost model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HbPost set ");
            strSql.Append("UserID=@UserID,");
            strSql.Append("num=@num,");
            strSql.Append("money=@money,");
            strSql.Append("RadomNum=@RadomNum,");
            strSql.Append("PostTime=@PostTime,");
            strSql.Append("GetIDList=@GetIDList,");
            strSql.Append("MaxRadom=@MaxRadom,");
            strSql.Append("Note=@Note");
            strSql.Append("ChatID=@ChatID");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@num", SqlDbType.Int,4),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@RadomNum", SqlDbType.NVarChar,-1),
					new SqlParameter("@PostTime", SqlDbType.DateTime),
					new SqlParameter("@GetIDList", SqlDbType.NVarChar,-1),
					new SqlParameter("@MaxRadom", SqlDbType.BigInt,8),
					new SqlParameter("@Note", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ChatID", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.num;
            parameters[2].Value = model.money;
            parameters[3].Value = model.RadomNum;
            parameters[4].Value = model.PostTime;
            parameters[5].Value = model.GetIDList;
            parameters[6].Value = model.MaxRadom;
            parameters[7].Value = model.Note;
            parameters[8].Value = model.ChatID;
            parameters[9].Value = model.State;
            parameters[10].Value = model.ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新字段GetIDList
        /// </summary>
        public void UpdateGetIDList(int ID, string GetIDList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HbPost set ");
            strSql.Append(" GetIDList=@GetIDList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@GetIDList", SqlDbType.NVarChar,-1),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = GetIDList;
            parameters[1].Value = ID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新Chat表的IShb字段
        /// </summary>
        public void UpdateChatIsHb(int ID, int IsHb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append(" IShbchat=@IsHb ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@IsHb", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = IsHb;
            parameters[1].Value = ID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到聊天室是否私人
        /// </summary>
        public int GetChatGS(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IShbchat from tb_Chat ");
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
        /// 重置抢币钱数
        /// </summary>
        public void UpdateCb(int ID, string ChatCmoney, DateTime ChatCTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatCmoney=@ChatCmoney,");
            strSql.Append("ChatCTime=@ChatCTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@ChatCmoney", SqlDbType.NVarChar,500),
                    new SqlParameter("@ChatCTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatCmoney;
            parameters[2].Value = ChatCTime;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新Chat表的ChatCmoney字段
        /// </summary>
        public void UpdateChatCmoney(int ID, string ChatCmoney)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append(" ChatCmoney=@ChatCmoney ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ChatCmoney", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ChatCmoney;
            parameters[1].Value = ID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获得Chat数据列表
        /// </summary>
        public DataSet GetChatList(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,Types,ChatCmoney ");
            strSql.Append(" FROM tb_Chat ");
            strSql.Append(" where ID=" + ID);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 更新字段State
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HbPost set ");
            strSql.Append(" State=@State ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = State;
            parameters[1].Value = ID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 红包玩花样更新
        /// </summary>
        public void UpdateStyle(int ID, int Style,string Keys)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HbPost set ");
            strSql.Append(" Style=@Style");
            strSql.Append(",Keys=@Keys ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Style", SqlDbType.Int,4),
                    new SqlParameter("@Keys", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Style;
            parameters[1].Value = Keys;
            parameters[2].Value = ID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HbPost ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HbPost ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbPost GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UserID,num,money,RadomNum,PostTime,GetIDList,MaxRadom,Note,ChatID,State,Style,Keys from tb_HbPost ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            BCW.HB.Model.HbPost model = new BCW.HB.Model.HbPost();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbPost DataRowToModel(DataRow row)
        {
            BCW.HB.Model.HbPost model = new BCW.HB.Model.HbPost();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["num"] != null && row["num"].ToString() != "")
                {
                    model.num = int.Parse(row["num"].ToString());
                }
                if (row["money"] != null && row["money"].ToString() != "")
                {
                    model.money = long.Parse(row["money"].ToString());
                }
                if (row["RadomNum"] != null)
                {
                    model.RadomNum = row["RadomNum"].ToString();
                }
                if (row["PostTime"] != null && row["PostTime"].ToString() != "")
                {
                    model.PostTime = DateTime.Parse(row["PostTime"].ToString());
                }
                if (row["GetIDList"] != null)
                {
                    model.GetIDList = row["GetIDList"].ToString();
                }
                if (row["MaxRadom"] != null && row["MaxRadom"].ToString() != "")
                {
                    model.MaxRadom = long.Parse(row["MaxRadom"].ToString());
                }
                if (row["Note"] != null)
                {
                    model.Note = row["Note"].ToString();
                }
                if (row["ChatID"] != null && row["ChatID"].ToString() != "")
                {
                    model.ChatID = int.Parse(row["ChatID"].ToString());
                }
                if (row["State"] != null && row["State"].ToString() != "")
                {
                    model.State = int.Parse(row["State"].ToString());
                }
                if (row["Style"] != null && row["Style"].ToString() != "")
                {
                    model.Style = int.Parse(row["Style"].ToString());
                }
                if (row["Keys"] != null)
                {
                    model.Keys = row["Keys"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UserID,num,money,RadomNum,PostTime,GetIDList,MaxRadom,Note,ChatID,State,Style,Keys ");
            strSql.Append(" FROM tb_HbPost ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ID,UserID,num,money,RadomNum,PostTime,GetIDList,MaxRadom,Note,ChatID,State,Style,Keys ");
            strSql.Append(" FROM tb_HbPost ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_HbPost ");
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
        /// 获取手气发送总钱数
        /// </summary>
        public int PostMoney(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(money) FROM tb_HbPost ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere + " and RadomNum!='pt'");
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
        /// 获取普通发送总钱数
        /// </summary>
        public int PostMoney2(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(money*num) FROM tb_HbPost ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere+ " and RadomNum='pt'");
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_HbPost T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 分页条件获取次数排行榜数据列表
        /// </summary>
        public DataSet GetListByPage1(int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ChatID,Count(*) AS'sm' into #bang3 from tb_HbPost   where ChatID!=0  group by ChatID ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" select ROW_NUMBER() OVER (");
            strSql.Append("order by T.sm desc ");
            strSql.Append(")AS Row, T.*  from #bang3 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang3");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 分页条件获取钱数排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ChatID,Sum(Money) AS'sm' into #bang4 from tb_HbPost where ChatID!=0 and RadomNum!='pt' group by ChatID ");
            strSql.Append("insert into #bang4 select ChatID,Sum(Money*num)  AS'sm' from tb_HbPost where ChatID!=0 and RadomNum='pt' group by ChatID ");
            strSql.Append("select ChatID,sum(sm) AS'sm2'  into #bang5 from #bang4 group by ChatID order by sm2 ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.sm2 desc");
            strSql.Append(")AS Row, T.*  from #bang5 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append(" drop table #bang4 ");
            strSql.Append("drop table #bang5");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ChatText</returns>
        public IList<BCW.HB.Model.HbPost> GetListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.HB.Model.HbPost> listHbpost = new List<BCW.HB.Model.HbPost>();
            string sTable = "tb_HbPost";
            string sPkey = "ID";
            string sField = "ID,UserID,num,money,RadomNum,PostTime,GetIDlist,MaxRadom,Note,ChatID,State,Style,Keys";
            string sCondition = strWhere;
            string sOrder = "PostTime Desc";
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
                    return listHbpost;
                }
                while (reader.Read())
                {
                    BCW.HB.Model.HbPost objHbpost = new BCW.HB.Model.HbPost();
                    objHbpost.ID = reader.GetInt32(0);
                    objHbpost.UserID = reader.GetInt32(1);
                    objHbpost.num = reader.GetInt32(2);
                    objHbpost.money = reader.GetInt64(3);
                    objHbpost.RadomNum = reader.GetString(4);
                    objHbpost.PostTime = reader.GetDateTime(5);
                    objHbpost.GetIDList= reader.GetString(6);
                    objHbpost.MaxRadom= reader.GetInt64(7);
                    objHbpost.Note= reader.GetString(8);
                    objHbpost.ChatID= reader.GetInt32(9);
                    objHbpost.State= reader.GetInt32(10);
                    objHbpost.Style = reader.GetInt32(11);
                    objHbpost.Keys = reader.GetString(12);
                    listHbpost.Add(objHbpost);
                }
            }
            return listHbpost;
        }
       
        #endregion 成员方法
    }
}
