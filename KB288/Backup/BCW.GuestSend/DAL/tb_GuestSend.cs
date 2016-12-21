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
	/// 数据访问类tb_GuestSend。
	/// </summary>
	public class tb_GuestSend
	{
		public tb_GuestSend()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_GuestSend");
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
            return SqlHelper.GetMaxID("ID", "tb_GuestSend");
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_GuestSend model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_GuestSend(");
			strSql.Append("manageID,guestContent,sendUidList,seeUidList,sendCount,seeCount,sendTime,sendDateTime,sentDay,allgold,hbCount,hbList,nowgold,maxCount,getCount,addtime,overtime,isDone,remark,isSendCount,notSeenIDList,sendtype)");
			strSql.Append(" values (");
			strSql.Append("@manageID,@guestContent,@sendUidList,@seeUidList,@sendCount,@seeCount,@sendTime,@sendDateTime,@sentDay,@allgold,@hbCount,@hbList,@nowgold,@maxCount,@getCount,@addtime,@overtime,@isDone,@remark,@isSendCount,@notSeenIDList,@sendtype)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@manageID", SqlDbType.Int,4),
					new SqlParameter("@guestContent", SqlDbType.NVarChar),
					new SqlParameter("@sendUidList", SqlDbType.NVarChar),
					new SqlParameter("@seeUidList", SqlDbType.NVarChar),
					new SqlParameter("@sendCount", SqlDbType.Int,4),
					new SqlParameter("@seeCount", SqlDbType.Int,4),
					new SqlParameter("@sendTime", SqlDbType.Char,30),
					new SqlParameter("@sendDateTime", SqlDbType.DateTime),
					new SqlParameter("@sentDay", SqlDbType.Int,4),
					new SqlParameter("@allgold", SqlDbType.Int,4),
					new SqlParameter("@hbCount", SqlDbType.Int,4),
					new SqlParameter("@hbList", SqlDbType.NVarChar),
					new SqlParameter("@nowgold", SqlDbType.Int,4),
					new SqlParameter("@maxCount", SqlDbType.Int,4),
					new SqlParameter("@getCount", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar),
					new SqlParameter("@isSendCount", SqlDbType.Int,4),
					new SqlParameter("@notSeenIDList", SqlDbType.NVarChar),
					new SqlParameter("@sendtype", SqlDbType.Int,4)};
			parameters[0].Value = model.manageID;
			parameters[1].Value = model.guestContent;
			parameters[2].Value = model.sendUidList;
			parameters[3].Value = model.seeUidList;
			parameters[4].Value = model.sendCount;
			parameters[5].Value = model.seeCount;
			parameters[6].Value = model.sendTime;
			parameters[7].Value = model.sendDateTime;
			parameters[8].Value = model.sentDay;
			parameters[9].Value = model.allgold;
			parameters[10].Value = model.hbCount;
			parameters[11].Value = model.hbList;
			parameters[12].Value = model.nowgold;
			parameters[13].Value = model.maxCount;
			parameters[14].Value = model.getCount;
			parameters[15].Value = model.addtime;
			parameters[16].Value = model.overtime;
			parameters[17].Value = model.isDone;
			parameters[18].Value = model.remark;
			parameters[19].Value = model.isSendCount;
			parameters[20].Value = model.notSeenIDList;
			parameters[21].Value = model.sendtype;

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
		public void Update(BCW.Model.tb_GuestSend model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_GuestSend set ");
			strSql.Append("manageID=@manageID,");
			strSql.Append("guestContent=@guestContent,");
			strSql.Append("sendUidList=@sendUidList,");
			strSql.Append("seeUidList=@seeUidList,");
			strSql.Append("sendCount=@sendCount,");
			strSql.Append("seeCount=@seeCount,");
			strSql.Append("sendTime=@sendTime,");
			strSql.Append("sendDateTime=@sendDateTime,");
			strSql.Append("sentDay=@sentDay,");
			strSql.Append("allgold=@allgold,");
			strSql.Append("hbCount=@hbCount,");
			strSql.Append("hbList=@hbList,");
			strSql.Append("nowgold=@nowgold,");
			strSql.Append("maxCount=@maxCount,");
			strSql.Append("getCount=@getCount,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("overtime=@overtime,");
			strSql.Append("isDone=@isDone,");
			strSql.Append("remark=@remark,");
			strSql.Append("isSendCount=@isSendCount,");
			strSql.Append("notSeenIDList=@notSeenIDList,");
			strSql.Append("sendtype=@sendtype");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@manageID", SqlDbType.Int,4),
					new SqlParameter("@guestContent", SqlDbType.NVarChar),
					new SqlParameter("@sendUidList", SqlDbType.NVarChar),
					new SqlParameter("@seeUidList", SqlDbType.NVarChar),
					new SqlParameter("@sendCount", SqlDbType.Int,4),
					new SqlParameter("@seeCount", SqlDbType.Int,4),
					new SqlParameter("@sendTime", SqlDbType.Char,30),
					new SqlParameter("@sendDateTime", SqlDbType.DateTime),
					new SqlParameter("@sentDay", SqlDbType.Int,4),
					new SqlParameter("@allgold", SqlDbType.Int,4),
					new SqlParameter("@hbCount", SqlDbType.Int,4),
					new SqlParameter("@hbList", SqlDbType.NVarChar),
					new SqlParameter("@nowgold", SqlDbType.Int,4),
					new SqlParameter("@maxCount", SqlDbType.Int,4),
					new SqlParameter("@getCount", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar),
					new SqlParameter("@isSendCount", SqlDbType.Int,4),
					new SqlParameter("@notSeenIDList", SqlDbType.NVarChar),
					new SqlParameter("@sendtype", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.manageID;
			parameters[2].Value = model.guestContent;
			parameters[3].Value = model.sendUidList;
			parameters[4].Value = model.seeUidList;
			parameters[5].Value = model.sendCount;
			parameters[6].Value = model.seeCount;
			parameters[7].Value = model.sendTime;
			parameters[8].Value = model.sendDateTime;
			parameters[9].Value = model.sentDay;
			parameters[10].Value = model.allgold;
			parameters[11].Value = model.hbCount;
			parameters[12].Value = model.hbList;
			parameters[13].Value = model.nowgold;
			parameters[14].Value = model.maxCount;
			parameters[15].Value = model.getCount;
			parameters[16].Value = model.addtime;
			parameters[17].Value = model.overtime;
			parameters[18].Value = model.isDone;
			parameters[19].Value = model.remark;
			parameters[20].Value = model.isSendCount;
			parameters[21].Value = model.notSeenIDList;
			parameters[22].Value = model.sendtype;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_GuestSend ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_GuestSend Gettb_GuestSend(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,manageID,guestContent,sendUidList,seeUidList,sendCount,seeCount,sendTime,sendDateTime,sentDay,allgold,hbCount,hbList,nowgold,maxCount,getCount,addtime,overtime,isDone,remark,isSendCount,notSeenIDList,sendtype from tb_GuestSend ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_GuestSend model=new BCW.Model.tb_GuestSend();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.manageID = reader.GetInt32(1);
					model.guestContent = reader.GetString(2);
					model.sendUidList = reader.GetString(3);
					model.seeUidList = reader.GetString(4);
					model.sendCount = reader.GetInt32(5);
					model.seeCount = reader.GetInt32(6);
					model.sendTime = reader.GetString(7);
					model.sendDateTime = reader.GetDateTime(8);
					model.sentDay = reader.GetInt32(9);
					model.allgold = reader.GetInt32(10);
					model.hbCount = reader.GetInt32(11);
					model.hbList = reader.GetString(12);
					model.nowgold = reader.GetInt32(13);
					model.maxCount = reader.GetInt32(14);
					model.getCount = reader.GetInt32(15);
					model.addtime = reader.GetDateTime(16);
					model.overtime = reader.GetDateTime(17);
					model.isDone = reader.GetInt32(18);
					model.remark = reader.GetString(19);
					model.isSendCount = reader.GetInt32(20);
					model.notSeenIDList = reader.GetString(21);
					model.sendtype = reader.GetInt32(22);
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
			strSql.Append(" FROM tb_GuestSend ");
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
		/// <returns>IList tb_GuestSend</returns>
		public IList<BCW.Model.tb_GuestSend> Gettb_GuestSends(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_GuestSend> listtb_GuestSends = new List<BCW.Model.tb_GuestSend>();
			string sTable = "tb_GuestSend";
			string sPkey = "id";
			string sField = "ID,manageID,guestContent,sendUidList,seeUidList,sendCount,seeCount,sendTime,sendDateTime,sentDay,allgold,hbCount,hbList,nowgold,maxCount,getCount,addtime,overtime,isDone,remark,isSendCount,notSeenIDList,sendtype";
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
					return listtb_GuestSends;
				}
				while (reader.Read())
				{
						BCW.Model.tb_GuestSend objtb_GuestSend = new BCW.Model.tb_GuestSend();
						objtb_GuestSend.ID = reader.GetInt32(0);
						objtb_GuestSend.manageID = reader.GetInt32(1);
						objtb_GuestSend.guestContent = reader.GetString(2);
						objtb_GuestSend.sendUidList = reader.GetString(3);
						objtb_GuestSend.seeUidList = reader.GetString(4);
						objtb_GuestSend.sendCount = reader.GetInt32(5);
						objtb_GuestSend.seeCount = reader.GetInt32(6);
						objtb_GuestSend.sendTime = reader.GetString(7);
						objtb_GuestSend.sendDateTime = reader.GetDateTime(8);
						objtb_GuestSend.sentDay = reader.GetInt32(9);
						objtb_GuestSend.allgold = reader.GetInt32(10);
						objtb_GuestSend.hbCount = reader.GetInt32(11);
						objtb_GuestSend.hbList = reader.GetString(12);
						objtb_GuestSend.nowgold = reader.GetInt32(13);
						objtb_GuestSend.maxCount = reader.GetInt32(14);
						objtb_GuestSend.getCount = reader.GetInt32(15);
						objtb_GuestSend.addtime = reader.GetDateTime(16);
						objtb_GuestSend.overtime = reader.GetDateTime(17);
						objtb_GuestSend.isDone = reader.GetInt32(18);
						objtb_GuestSend.remark = reader.GetString(19);
						objtb_GuestSend.isSendCount = reader.GetInt32(20);
						objtb_GuestSend.notSeenIDList = reader.GetString(21);
						objtb_GuestSend.sendtype = reader.GetInt32(22);
						listtb_GuestSends.Add(objtb_GuestSend);
				}
			}
			return listtb_GuestSends;
		}

		#endregion  成员方法
	}
}

