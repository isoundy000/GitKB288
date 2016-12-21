using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Draw.DAL
{
	/// <summary>
	/// 数据访问类DrawJifen。
	/// </summary>
	public class DrawJifen
	{
		public DrawJifen()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_DrawJifen"); 
		}

        /// <summary>
        /// 得到用户积分
        /// </summary>
        public int GetJifen(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Jifen from tb_DrawJifen ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 得到用户昵称
        /// </summary>
        public string GetUsName(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsName from tb_user ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

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
        /// 更新用户积分
        /// </summary>
        public void UpdateJifen(int UsID, int Jifen)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawJifen set ");
            strSql.Append("Jifen=Jifen+@Jifen ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Jifen", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Jifen;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新累计签到天数
        /// </summary>
        public void UpdateQd(int UsID ,int Qd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawJifen set ");
            strSql.Append("Qd=@Qd ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
                                 new SqlParameter("@Qd", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Qd;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新连续签到天数
        /// </summary>
        public void UpdateQdweek(int UsID,int Qdweek)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawJifen set ");
            strSql.Append("Qdweek=@Qdweek ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
                                        new SqlParameter("@Qdweek", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Qdweek;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
    
        /// <summary>
        /// 更新签到时间
        /// </summary>
        public void UpdateQdTime(int UsID,DateTime QdTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawJifen set ");
            strSql.Append("QdTime=@QdTime ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
                                         new SqlParameter("@QdTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = QdTime;

         SqlHelperUser.GetSingle(strSql.ToString(), parameters);
   
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsUserID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DrawJifen");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该USID
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_DrawJifen");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Draw.Model.DrawJifen model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_DrawJifen(");
            strSql.Append("UsID,Jifen,Qd,Qdweek,QdTime)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@Jifen,@Qd,@Qdweek,@QdTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Jifen", SqlDbType.Int,4),
					new SqlParameter("@Qd", SqlDbType.Int,4),
					new SqlParameter("@Qdweek", SqlDbType.Int,4),
                                       			new SqlParameter("@QdTime", SqlDbType.DateTime) };
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.Jifen;
            parameters[2].Value = model.Qd;
            parameters[3].Value = model.Qdweek;
                parameters[4].Value = model.QdTime;

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
        public void Update(BCW.Draw.Model.DrawJifen model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawJifen set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("Jifen=@Jifen,");
            strSql.Append("Qd=@Qd,");
            strSql.Append("Qdweek=@Qdweek,");
              strSql.Append("QdTime=@QdTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Jifen", SqlDbType.Int,4),
					new SqlParameter("@Qd", SqlDbType.Int,4),
					new SqlParameter("@Qdweek", SqlDbType.Int,4),
                                        new SqlParameter("@QdTime",SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.Jifen;
            parameters[3].Value = model.Qd;
            parameters[4].Value = model.Qdweek;
               parameters[5].Value = model.QdTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DrawJifen ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 得到每日签到
        /// </summary>
        public int getsQd(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(Qd)  from tb_DrawJifen where UsID=" + UsID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Qd", SqlDbType.Int,4)};

            parameters[0].Value = 0;
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
        /// 得到连续签到
        /// </summary>
        public int getsQdweek(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(Qdweek)  from tb_DrawJifen where UsID=" + UsID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Qdweek", SqlDbType.Int,4)};

            parameters[0].Value = 0;
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
        /// 得到签到时间
        /// </summary>
        public DateTime getQdTime(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select QdTime from tb_DrawJifen ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDateTime(0);
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }
     
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawJifen GetDrawJifen(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,Jifen,Qd,Qdweek,QdTime from tb_DrawJifen ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Draw.Model.DrawJifen model = new BCW.Draw.Model.DrawJifen();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.Jifen = reader.GetInt32(2);
                    model.Qd = reader.GetInt32(3);
                    model.Qdweek = reader.GetInt32(4);
                    model.QdTime = reader.GetDateTime(5);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_DrawJifen ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
        /// <returns>IList tb_DrawJifen</returns>
        public IList<BCW.Draw.Model.DrawJifen> GetDrawJifens(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Draw.Model.DrawJifen> listtb_DrawJifens = new List<BCW.Draw.Model.DrawJifen>();
            string sTable = "tb_DrawJifen";
            string sPkey = "id";
            string sField = "ID,UsID,Jifen,Qd,Qdweek,QdTime";
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
                    return listtb_DrawJifens;
                }
                while (reader.Read())
                {
                    BCW.Draw.Model.DrawJifen objtb_DrawJifen = new BCW.Draw.Model.DrawJifen();
                    objtb_DrawJifen.ID = reader.GetInt32(0);
                    objtb_DrawJifen.UsID = reader.GetInt32(1);
                    objtb_DrawJifen.Jifen = reader.GetInt32(2);
                    objtb_DrawJifen.Qd = reader.GetInt32(3);
                    objtb_DrawJifen.Qdweek = reader.GetInt32(4);
                    objtb_DrawJifen.QdTime = reader.GetDateTime(5);
                    listtb_DrawJifens.Add(objtb_DrawJifen);
                }
            }
            return listtb_DrawJifens;
        }

        #endregion  成员方法
    }
}

