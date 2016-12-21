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
	/// 数据访问类tb_GuestSendList。
	/// </summary>
	public class tb_GuestSendList
	{
		public tb_GuestSendList()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_GuestSendList");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_GuestSendList");
        }
        /// <summary>
		/// 是否存在该记录usid guestsendID type
		/// </summary>
		public bool ExistsUidType(int usid,int guestsendID,int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GuestSendList");
            strSql.Append(" where usid=@usid and  guestsendID=@guestsendID and ( type!=1)");
            SqlParameter[] parameters = {
                   new SqlParameter("@usid", SqlDbType.Int,4),
                       new SqlParameter("@guestsendID", SqlDbType.Int,4) };
            parameters[0].Value = usid;
            parameters[1].Value = guestsendID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_GuestSendList model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_GuestSendList(");
			strSql.Append("usid,guestID,guestsendID,getGold,type,remark,idDone,addtime,overtime)");
			strSql.Append(" values (");
			strSql.Append("@usid,@guestID,@guestsendID,@getGold,@type,@remark,@idDone,@addtime,@overtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@guestID", SqlDbType.Int,4),
					new SqlParameter("@guestsendID", SqlDbType.Int,4),
					new SqlParameter("@getGold", SqlDbType.Int,4),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar,50),
					new SqlParameter("@idDone", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime)};
			parameters[0].Value = model.usid;
			parameters[1].Value = model.guestID;
			parameters[2].Value = model.guestsendID;
			parameters[3].Value = model.getGold;
			parameters[4].Value = model.type;
			parameters[5].Value = model.remark;
			parameters[6].Value = model.idDone;
			parameters[7].Value = model.addtime;
			parameters[8].Value = model.overtime;

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
		public void Update(BCW.Model.tb_GuestSendList model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_GuestSendList set ");
			strSql.Append("usid=@usid,");
			strSql.Append("guestID=@guestID,");
			strSql.Append("guestsendID=@guestsendID,");
			strSql.Append("getGold=@getGold,");
			strSql.Append("type=@type,");
			strSql.Append("remark=@remark,");
			strSql.Append("idDone=@idDone,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("overtime=@overtime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@guestID", SqlDbType.Int,4),
					new SqlParameter("@guestsendID", SqlDbType.Int,4),
					new SqlParameter("@getGold", SqlDbType.Int,4),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar,50),
					new SqlParameter("@idDone", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.usid;
			parameters[2].Value = model.guestID;
			parameters[3].Value = model.guestsendID;
			parameters[4].Value = model.getGold;
			parameters[5].Value = model.type;
			parameters[6].Value = model.remark;
			parameters[7].Value = model.idDone;
			parameters[8].Value = model.addtime;
			parameters[9].Value = model.overtime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_GuestSendList ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
		/// 从uid guestId 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_GuestSendList Gettb_GuestSendListForUsidGuestID(int usid, int guestID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,guestID,guestsendID,getGold,type,remark,idDone,addtime,overtime from tb_GuestSendList ");
            strSql.Append(" where usid=@usid and guestID=@guestID order by ID desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
              new SqlParameter("@guestID", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = guestID;

            BCW.Model.tb_GuestSendList model = new BCW.Model.tb_GuestSendList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.guestID = reader.GetInt32(2);
                    model.guestsendID = reader.GetInt32(3);
                    model.getGold = reader.GetInt32(4);
                    model.type = reader.GetInt32(5);
                    model.remark = reader.GetString(6);
                    model.idDone = reader.GetInt32(7);
                    model.addtime = reader.GetDateTime(8);
                    model.overtime = reader.GetDateTime(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_GuestSendList Gettb_GuestSendList(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,usid,guestID,guestsendID,getGold,type,remark,idDone,addtime,overtime from tb_GuestSendList ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_GuestSendList model=new BCW.Model.tb_GuestSendList();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.usid = reader.GetInt32(1);
					model.guestID = reader.GetInt32(2);
					model.guestsendID = reader.GetInt32(3);
					model.getGold = reader.GetInt32(4);
					model.type = reader.GetInt32(5);
					model.remark = reader.GetString(6);
					model.idDone = reader.GetInt32(7);
					model.addtime = reader.GetDateTime(8);
					model.overtime = reader.GetDateTime(9);
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
			strSql.Append(" FROM tb_GuestSendList ");
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
		/// <returns>IList tb_GuestSendList</returns>
		public IList<BCW.Model.tb_GuestSendList> Gettb_GuestSendLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_GuestSendList> listtb_GuestSendLists = new List<BCW.Model.tb_GuestSendList>();
			string sTable = "tb_GuestSendList";
			string sPkey = "id";
			string sField = "ID,usid,guestID,guestsendID,getGold,type,remark,idDone,addtime,overtime";
			string sCondition = strWhere;
			string sOrder = "overtime Desc";
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
					return listtb_GuestSendLists;
				}
				while (reader.Read())
				{
						BCW.Model.tb_GuestSendList objtb_GuestSendList = new BCW.Model.tb_GuestSendList();
						objtb_GuestSendList.ID = reader.GetInt32(0);
						objtb_GuestSendList.usid = reader.GetInt32(1);
						objtb_GuestSendList.guestID = reader.GetInt32(2);
						objtb_GuestSendList.guestsendID = reader.GetInt32(3);
						objtb_GuestSendList.getGold = reader.GetInt32(4);
						objtb_GuestSendList.type = reader.GetInt32(5);
						objtb_GuestSendList.remark = reader.GetString(6);
						objtb_GuestSendList.idDone = reader.GetInt32(7);
						objtb_GuestSendList.addtime = reader.GetDateTime(8);
						objtb_GuestSendList.overtime = reader.GetDateTime(9);
						listtb_GuestSendLists.Add(objtb_GuestSendList);
				}
			}
			return listtb_GuestSendLists;
		}

		#endregion  成员方法
	}
}

