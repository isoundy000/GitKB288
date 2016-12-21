using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.bydr.DAL
{
	/// <summary>
	/// 数据访问类Cmg_notes。
	/// </summary>
	public class Cmg_notes
	{
		public Cmg_notes()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		  return SqlHelper.GetMaxID("ID", "tb_Cmg_notes"); 
		}
        public int GetMaxID(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select max(ID) from tb_Cmg_notes");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
					new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = usID;

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
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Cmg_notes");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int meid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Cmg_notes");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
					new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = meid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int n, int usid, string coID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select  top " + n);
            strSql.Append(" * into #b11 from tb_Cmg_notes where usID=" + usid + " order by ID desc");
            strSql.Append(" select count(*) from #b11 where coID=@coID");
            strSql.Append(" drop table #b11");
            SqlParameter[] parameters = {
					new SqlParameter("@coID", SqlDbType.NVarChar,50)};
            parameters[0].Value = coID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }



	    /// <summary>
		/// 增加一条数据
		/// </summary>
        public int Add(BCW.bydr.Model.Cmg_notes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Cmg_notes(");
            strSql.Append("AcolletGold,coID,Vit,usID,Stime,AllGold,changj,stype,Signtime,random,cxid)");
            strSql.Append(" values (");
            strSql.Append("@AcolletGold,@coID,@Vit,@usID,@Stime,@AllGold,@changj,@stype,@Signtime,@random,@cxid)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@AcolletGold", SqlDbType.BigInt,8),
					new SqlParameter("@coID", SqlDbType.NVarChar,50),
					new SqlParameter("@Vit", SqlDbType.Int,4),
					new SqlParameter("@usID", SqlDbType.Int,4),
					new SqlParameter("@Stime", SqlDbType.DateTime),
					new SqlParameter("@AllGold", SqlDbType.BigInt,8),
					new SqlParameter("@changj", SqlDbType.NVarChar,50),
					new SqlParameter("@stype", SqlDbType.Int,4),
					new SqlParameter("@Signtime", SqlDbType.DateTime),
                    new SqlParameter("@random",SqlDbType.Int,4),
                    new SqlParameter("@cxid",SqlDbType.Int,4)};
            parameters[0].Value = model.AcolletGold;
            parameters[1].Value = model.coID;
            parameters[2].Value = model.Vit;
            parameters[3].Value = model.usID;
            parameters[4].Value = model.Stime;
            parameters[5].Value = model.AllGold;
            parameters[6].Value = model.changj;
            parameters[7].Value = model.stype;
            parameters[8].Value = model.Signtime;
            parameters[9].Value = model.random;
            parameters[10].Value = model.cxid;

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
        public void Update(BCW.bydr.Model.Cmg_notes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_notes set ");
            strSql.Append("AcolletGold=@AcolletGold,");
            strSql.Append("coID=@coID,");
            strSql.Append("Vit=@Vit,");
            strSql.Append("usID=@usID,");
            strSql.Append("Stime=@Stime,");
            strSql.Append("AllGold=@AllGold,");
            strSql.Append("changj=@changj,");
            strSql.Append("stype=@stype,");
            strSql.Append("Signtime=@Signtime,");
            strSql.Append("random=@random,");
            strSql.Append("cxid=@cxid");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@AcolletGold", SqlDbType.BigInt,8),
					new SqlParameter("@coID", SqlDbType.NVarChar,50),
					new SqlParameter("@Vit", SqlDbType.Int,4),
					new SqlParameter("@usID", SqlDbType.Int,4),
					new SqlParameter("@Stime", SqlDbType.DateTime),
					new SqlParameter("@AllGold", SqlDbType.BigInt,8),
					new SqlParameter("@changj", SqlDbType.NVarChar,50),
					new SqlParameter("@stype", SqlDbType.Int,4),
					new SqlParameter("@Signtime", SqlDbType.DateTime),
                    new SqlParameter("@random",SqlDbType.Int,4),
                    new SqlParameter("@cxid",SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.AcolletGold;
            parameters[2].Value = model.coID;
            parameters[3].Value = model.Vit;
            parameters[4].Value = model.usID;
            parameters[5].Value = model.Stime;
            parameters[6].Value = model.AllGold;
            parameters[7].Value = model.changj;
            parameters[8].Value = model.stype;
            parameters[9].Value = model.Signtime;
            parameters[10].Value = model.random;
            parameters[11].Value = model.cxid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
       	/// <summary>
		/// 更新一条数据
		/// </summary>
        public void Updatestype(int ID, int stype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_notes set ");
            strSql.Append("stype=@stype");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {  
                                              new SqlParameter("@ID", SqlDbType.Int,4),
                                            new SqlParameter("@stype", SqlDbType.Int,4)};     
            parameters[0].Value = ID;
            parameters[1].Value = stype;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void updateAllGold(int ID, long AllGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_notes set ");
            strSql.Append("AllGold=AllGold+@AllGold");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {  
                                              new SqlParameter("@ID", SqlDbType.Int,4),
                                       new SqlParameter("@AllGold", SqlDbType.Int,4)};
                                         
            parameters[0].Value = ID;
            parameters[1].Value = AllGold;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void updateAllGold1(int ID, long AllGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_notes set ");
            strSql.Append("AllGold=@AllGold");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {  
                                              new SqlParameter("@ID", SqlDbType.Int,4),
                                               new SqlParameter("@AllGold", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = AllGold;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新体力
        /// </summary>
        public void UpdateVit(int ID, int Vit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_notes set ");
            strSql.Append("Vit=Vit+@Vit");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.Int,4),
                                            new SqlParameter("@Vit", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Vit;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新签到时间
        /// </summary>
        public void UpdateSigntime(int ID, DateTime Signtime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_notes set ");
            strSql.Append("Signtime=@Signtime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.Int,4),
                                            new SqlParameter("@Signtime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = Signtime;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Cmg_notes ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 删除一段时间的数据
        /// </summary>
        public void Delete1(string stime,string otime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Cmg_notes ");
            strSql.Append(" where  Stime>='" + (DateTime.ParseExact(stime, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Stime<='" + (DateTime.ParseExact(otime, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'");
            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.bydr.Model.Cmg_notes GetCmg_notes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,AcolletGold,coID,Vit,usID,Stime,AllGold,changj,stype,Signtime,random,cxid from tb_Cmg_notes ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.bydr.Model.Cmg_notes model = new BCW.bydr.Model.Cmg_notes();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.AcolletGold = reader.GetInt64(1);
                    model.coID = reader.GetString(2);
                    model.Vit = reader.GetInt32(3);
                    model.usID = reader.GetInt32(4);
                    model.Stime = reader.GetDateTime(5);
                    model.AllGold = reader.GetInt64(6);
                    model.changj = reader.GetString(7);
                    model.stype = reader.GetInt32(8);
                    model.Signtime = reader.GetDateTime(9);
                    model.random = reader.GetInt32(10);
                    model.cxid = reader.GetInt32(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 得到相同usID最后一个usID
        /// </summary>
        public BCW.bydr.Model.Cmg_notes GetusID(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 usID from tb_Cmg_notes");
            strSql.Append("order by usID desc");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
					new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = usID;

            BCW.bydr.Model.Cmg_notes model = new BCW.bydr.Model.Cmg_notes();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.AcolletGold = reader.GetInt64(1);
                    model.coID = reader.GetString(2);
                    model.Vit = reader.GetInt32(3);
                    model.usID = reader.GetInt32(4);
                    model.Stime = reader.GetDateTime(5);
                    model.AllGold = reader.GetInt64(6);
                    model.changj = reader.GetString(7);
                    model.stype = reader.GetInt32(8);
                    model.Signtime = reader.GetDateTime(9);
                    model.random = reader.GetInt32(10);
                    model.cxid = reader.GetInt32(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 通过字段得到id
        /// </summary>
        public int GetID1(int usID, int stype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_Cmg_notes");
            strSql.Append(" where stype=@stype ");
            strSql.Append("and usID=@usID");
            SqlParameter[] parameters = {
					new SqlParameter("@stype", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = stype;
            parameters[1].Value=usID;
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
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_Cmg_notes ");
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
		/// <returns>IList Cmg_notes</returns>
		public IList<BCW.bydr.Model.Cmg_notes> GetCmg_notess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.bydr.Model.Cmg_notes> listCmg_notess = new List<BCW.bydr.Model.Cmg_notes>();
			string sTable = "tb_Cmg_notes";
			string sPkey = "id";
			string sField = "ID,AcolletGold,coID,Vit,usID,Stime,AllGold,changj,stype,Signtime,random,cxid";
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
					return listCmg_notess;
				}
				while (reader.Read())
				{
						BCW.bydr.Model.Cmg_notes objCmg_notes = new BCW.bydr.Model.Cmg_notes();
						objCmg_notes.ID = reader.GetInt32(0);
						objCmg_notes.AcolletGold = reader.GetInt64(1);
						objCmg_notes.coID = reader.GetString(2);
						objCmg_notes.Vit = reader.GetInt32(3);
						objCmg_notes.usID = reader.GetInt32(4);
						objCmg_notes.Stime = reader.GetDateTime(5);
						objCmg_notes.AllGold = reader.GetInt64(6);
						objCmg_notes.changj = reader.GetString(7);
						objCmg_notes.stype = reader.GetInt32(8);
						objCmg_notes.Signtime = reader.GetDateTime(9);
                        objCmg_notes.random = reader.GetInt32(10);
                        objCmg_notes.cxid = reader.GetInt32(11);
						listCmg_notess.Add(objCmg_notes);
				}
			}
			return listCmg_notess;
		}

		#endregion  成员方法
	}
}

