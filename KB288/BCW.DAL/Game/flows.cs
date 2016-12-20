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
	/// 数据访问类flows。
	/// </summary>
	public class flows
	{
		public flows()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_flows"); 
		}

        /// <summary>
        /// 是否存在花盆记录
        /// </summary>
        public bool Exists(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_flows");
            strSql.Append(" where UsID=@UsID and State>0");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
        public bool Exists(int ID, int UsID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_flows");
			strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
			parameters[0].Value = ID;
            parameters[1].Value = UsID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 计算某会员(空\非空\成熟)花盆数量
        /// </summary>
        public int GetCount(int UsID,int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_flows");
            strSql.Append(" where UsID=@UsID ");
            if (State == -1)
                strSql.Append(" and State>0 ");
            else if (State == -2)
                strSql.Append(" and State>0 and State<4");
            else
                strSql.Append(" and State=" + State + " ");

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
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.flows model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_flows(");
			strSql.Append("UsID,zid,ztitle,cnum,water,worm,weeds,state,cnum2,checkuid)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@zid,@ztitle,@cnum,@water,@worm,@weeds,@state,@cnum2,@checkuid)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@ztitle", SqlDbType.NVarChar,50),
					new SqlParameter("@cnum", SqlDbType.Int,4),
					new SqlParameter("@water", SqlDbType.Int,4),
					new SqlParameter("@worm", SqlDbType.Int,4),
					new SqlParameter("@weeds", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@cnum2", SqlDbType.Int,4),
					new SqlParameter("@checkuid", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.zid;
			parameters[2].Value = model.ztitle;
			parameters[3].Value = model.cnum;
			parameters[4].Value = model.water;
			parameters[5].Value = model.worm;
			parameters[6].Value = model.weeds;
			parameters[7].Value = model.state;
			parameters[8].Value = model.cnum2;
			parameters[9].Value = model.checkuid;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public void Update(BCW.Model.Game.flows model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_flows set ");
			strSql.Append("zid=@zid,");
			strSql.Append("ztitle=@ztitle,");
			strSql.Append("cnum=@cnum,");
			strSql.Append("state=@state,");
			strSql.Append("AddTime=@AddTime,");
            strSql.Append("water=@water");
            if (model.ztitle == "空花盆")
            {
                strSql.Append(",worm=0");
                strSql.Append(",weeds=0");
            }
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@ztitle", SqlDbType.NVarChar,50),
					new SqlParameter("@cnum", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@water", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.zid;
			parameters[2].Value = model.ztitle;
			parameters[3].Value = model.cnum;
			parameters[4].Value = model.state;
			parameters[5].Value = model.AddTime;
            parameters[6].Value = model.water;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int ID, int state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("state=@state");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = state;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新AddTime用作道具提前收获
        /// </summary>
        public void UpdateAddTime(int ID, DateTime AddTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新水分
        /// </summary>
        public void Updatewater(int ID, int water)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("water=@water");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@water", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = water;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新杂草
        /// </summary>
        public void Updateweeds(int ID, int weeds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("weeds=weeds+@weeds");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@weeds", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = weeds;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新杂草
        /// </summary>
        public void Updateweeds2(int UsID, int weeds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("weeds=weeds+@weeds");
            strSql.Append(" where UsID=@UsID and State>0 and State<4");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@weeds", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = weeds;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新害虫
        /// </summary>
        public void Updateworm(int ID, int worm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("worm=worm+@worm");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@worm", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = worm;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新害虫
        /// </summary>
        public void Updateworm2(int UsID, int worm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flows set ");
            strSql.Append("worm=worm+@worm");
            strSql.Append(" where UsID=@UsID and State>0 and State<4");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@worm", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = worm;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_flows ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.flows Getflows(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,zid,ztitle,cnum,water,worm,weeds,state,cnum2,checkuid,AddTime from tb_flows ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Game.flows model=new BCW.Model.Game.flows();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.zid = reader.GetInt32(2);
					model.ztitle = reader.GetString(3);
					model.cnum = reader.GetInt32(4);
					model.water = reader.GetInt32(5);
					model.worm = reader.GetInt32(6);
					model.weeds = reader.GetInt32(7);
					model.state = reader.GetInt32(8);
					model.cnum2 = reader.GetInt32(9);
					model.checkuid = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.AddTime = reader.GetDateTime(11);
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
			strSql.Append(" FROM tb_flows ");
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
		/// <returns>IList flows</returns>
		public IList<BCW.Model.Game.flows> Getflowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.flows> listflowss = new List<BCW.Model.Game.flows>();
			string sTable = "tb_flows";
			string sPkey = "id";
			string sField = "ID,UsID,zid,ztitle,cnum,water,worm,weeds,state,cnum2,checkuid,AddTime";
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
					return listflowss;
				}
				while (reader.Read())
				{
						BCW.Model.Game.flows objflows = new BCW.Model.Game.flows();
						objflows.ID = reader.GetInt32(0);
						objflows.UsID = reader.GetInt32(1);
						objflows.zid = reader.GetInt32(2);
						objflows.ztitle = reader.GetString(3);
						objflows.cnum = reader.GetInt32(4);
						objflows.water = reader.GetInt32(5);
						objflows.worm = reader.GetInt32(6);
						objflows.weeds = reader.GetInt32(7);
						objflows.state = reader.GetInt32(8);
						objflows.cnum2 = reader.GetInt32(9);
						objflows.checkuid = reader.GetInt32(10);
                        if (!reader.IsDBNull(11))
                            objflows.AddTime = reader.GetDateTime(11);
                        listflowss.Add(objflows);
				}
			}
			return listflowss;
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList flows</returns>
        public IList<BCW.Model.Game.flows> Getflowss2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount,int uid)
        {
            IList<BCW.Model.Game.flows> listflowss = new List<BCW.Model.Game.flows>();
            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT a.UsID) FROM tb_flows a Where a.UsID in (Select b.FriendId from tb_Friend b Where b.UsID=" + uid + " and b.FriendId=a.UsID and Types=0) " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listflowss;
            }
            string queryString = "SELECT DISTINCT a.UsID FROM tb_flows a Where a.UsID in (Select b.FriendId from tb_Friend b Where b.UsID=" + uid + " and b.FriendId=a.UsID and Types=0) " + strWhere + " ";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.flows objflows = new BCW.Model.Game.flows();
                        objflows.UsID = reader.GetInt32(0);
                        listflowss.Add(objflows);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }
            return listflowss;
        }

        /// <summary>
        /// 取得上(下)一条记录
        /// </summary>
        public BCW.Model.Game.flows GetPreviousNextflows(int ID, int UsID, bool p_next,bool p_ismy)
        {
            List<BCW.Model.Game.flows> listflows = new List<BCW.Model.Game.flows>();

            // 搜索部分
            SqlParameter[] tmpSqlParam = {
                new SqlParameter("@UsID", SqlDbType.Int, 4),
                new SqlParameter("@ID", SqlDbType.Int, 4)
            };

            string where = "";
            if (UsID != 0)
            {
                where += " UsID=@UsID AND";
                tmpSqlParam[0].Value = UsID;
            }

            where += !p_next ? " ID<@ID AND" : " ID>@ID AND";
            tmpSqlParam[1].Value = ID;

            if (where != "")
                where = " WHERE" + where.Substring(0, where.Length - 4);


            // 重新整理 SqlParameter 顺序
            int i = 0;
            SqlParameter[] SqlParam = new SqlParameter[2];
            foreach (SqlParameter p in tmpSqlParam)
            {
                if (p.Value != null)
                {
                    SqlParam[i] = new SqlParameter();
                    SqlParam[i].ParameterName = p.ParameterName;
                    SqlParam[i].SqlDbType = p.SqlDbType;
                    SqlParam[i].Size = p.Size;
                    SqlParam[i].Value = p.Value;
                    i++;
                }
            }

            string order = string.Empty;
            if (!p_next)
            {
                order = " ORDER BY ID DESC";
            }
            else
            {
                order = " ORDER BY ID ASC";
            }


            // 取出相关记录
            string strWhe = "";
            if (!p_ismy)
            {
                strWhe = " and State>0";
            }
            BCW.Model.Game.flows objflows = new BCW.Model.Game.flows();

            string queryString = "SELECT TOP 1 ID" +
                                " FROM tb_flows" + where + "" + strWhe + "" + order;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString.ToString(), SqlParam))
            {

                while (reader.Read())
                {

                    objflows.ID = reader.GetInt32(0);
                }
            }

            return objflows;
        }

		#endregion  成员方法
	}
}

