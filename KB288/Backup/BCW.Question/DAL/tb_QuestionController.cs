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
	/// 数据访问类tb_QuestionController。
	/// </summary>
	public class tb_QuestionController
	{
		public tb_QuestionController()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_QuestionController");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        //得到最大奖金
        public int GetMaxAwardForID(int  ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select award from tb_QuestionController ");
            strSql.Append(" where ID=@ID  ");
            SqlParameter[] parameters = {
                };
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_QuestionController model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_QuestionController(");
			strSql.Append("muid,uid,type,count,List,uided,acount,wcount,ycount,ubbtext,passtime,awardtype,awardptype,award,addtime,overtime,remark)");
			strSql.Append(" values (");
			strSql.Append("@muid,@uid,@type,@count,@List,@uided,@acount,@wcount,@ycount,@ubbtext,@passtime,@awardtype,@awardptype,@award,@addtime,@overtime,@remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@muid", SqlDbType.Int,4),
					new SqlParameter("@uid", SqlDbType.NVarChar),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@List", SqlDbType.NVarChar,500),
					new SqlParameter("@uided", SqlDbType.NVarChar),
					new SqlParameter("@acount", SqlDbType.Int,4),
					new SqlParameter("@wcount", SqlDbType.Int,4),
					new SqlParameter("@ycount", SqlDbType.NChar,10),
					new SqlParameter("@ubbtext", SqlDbType.NVarChar,4000),
					new SqlParameter("@passtime", SqlDbType.Int,4),
					new SqlParameter("@awardtype", SqlDbType.Int,4),
					new SqlParameter("@awardptype", SqlDbType.Int,4),
					new SqlParameter("@award", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime),
					new SqlParameter("@remark", SqlDbType.NVarChar,20)};
			parameters[0].Value = model.muid;
			parameters[1].Value = model.uid;
			parameters[2].Value = model.type;
			parameters[3].Value = model.count;
			parameters[4].Value = model.List;
			parameters[5].Value = model.uided;
			parameters[6].Value = model.acount;
			parameters[7].Value = model.wcount;
			parameters[8].Value = model.ycount;
			parameters[9].Value = model.ubbtext;
			parameters[10].Value = model.passtime;
			parameters[11].Value = model.awardtype;
			parameters[12].Value = model.awardptype;
			parameters[13].Value = model.award;
			parameters[14].Value = model.addtime;
			parameters[15].Value = model.overtime;
			parameters[16].Value = model.remark;

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
		public void Update(BCW.Model.tb_QuestionController model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_QuestionController set ");
			strSql.Append("muid=@muid,");
			strSql.Append("uid=@uid,");
			strSql.Append("type=@type,");
			strSql.Append("count=@count,");
			strSql.Append("List=@List,");
			strSql.Append("uided=@uided,");
			strSql.Append("acount=@acount,");
			strSql.Append("wcount=@wcount,");
			strSql.Append("ycount=@ycount,");
			strSql.Append("ubbtext=@ubbtext,");
			strSql.Append("passtime=@passtime,");
			strSql.Append("awardtype=@awardtype,");
			strSql.Append("awardptype=@awardptype,");
			strSql.Append("award=@award,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("overtime=@overtime,");
			strSql.Append("remark=@remark");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@muid", SqlDbType.Int,4),
					new SqlParameter("@uid", SqlDbType.NVarChar),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@count", SqlDbType.Int,4),
					new SqlParameter("@List", SqlDbType.NVarChar,500),
					new SqlParameter("@uided", SqlDbType.NVarChar),
					new SqlParameter("@acount", SqlDbType.Int,4),
					new SqlParameter("@wcount", SqlDbType.Int,4),
					new SqlParameter("@ycount", SqlDbType.NChar,10),
					new SqlParameter("@ubbtext", SqlDbType.NVarChar,4000),
					new SqlParameter("@passtime", SqlDbType.Int,4),
					new SqlParameter("@awardtype", SqlDbType.Int,4),
					new SqlParameter("@awardptype", SqlDbType.Int,4),
					new SqlParameter("@award", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@overtime", SqlDbType.DateTime),
					new SqlParameter("@remark", SqlDbType.NVarChar,20)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.muid;
			parameters[2].Value = model.uid;
			parameters[3].Value = model.type;
			parameters[4].Value = model.count;
			parameters[5].Value = model.List;
			parameters[6].Value = model.uided;
			parameters[7].Value = model.acount;
			parameters[8].Value = model.wcount;
			parameters[9].Value = model.ycount;
			parameters[10].Value = model.ubbtext;
			parameters[11].Value = model.passtime;
			parameters[12].Value = model.awardtype;
			parameters[13].Value = model.awardptype;
			parameters[14].Value = model.award;
			parameters[15].Value = model.addtime;
			parameters[16].Value = model.overtime;
			parameters[17].Value = model.remark;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_QuestionController ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionController Gettb_QuestionController(int uid, int contrID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,muid,uid,type,count,List,uided,acount,wcount,ycount,ubbtext,passtime,awardtype,awardptype,award,addtime,overtime,remark from tb_QuestionController ");
            strSql.Append(" where uid=@uid and contrID=@contrID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4),
             new SqlParameter("@contrID", SqlDbType.Int,4)};
            parameters[0].Value = uid;
            parameters[1].Value = contrID;

            BCW.Model.tb_QuestionController model = new BCW.Model.tb_QuestionController();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.muid = reader.GetInt32(1);
                    model.uid = reader.GetString(2);
                    model.type = reader.GetInt32(3);
                    model.count = reader.GetInt32(4);
                    model.List = reader.GetString(5);
                    model.uided = reader.GetString(6);
                    model.acount = reader.GetInt32(7);
                    model.wcount = reader.GetInt32(8);
                    model.ycount = reader.GetString(9);
                    model.ubbtext = reader.GetString(10);
                    model.passtime = reader.GetInt32(11);
                    model.awardtype = reader.GetInt32(12);
                    model.awardptype = reader.GetInt32(13);
                    model.award = reader.GetInt32(14);
                    model.addtime = reader.GetDateTime(15);
                    model.overtime = reader.GetDateTime(16);
                    model.remark = reader.GetString(17);
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
        public BCW.Model.tb_QuestionController Gettb_QuestionController(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,muid,uid,type,count,List,uided,acount,wcount,ycount,ubbtext,passtime,awardtype,awardptype,award,addtime,overtime,remark from tb_QuestionController ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_QuestionController model=new BCW.Model.tb_QuestionController();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.muid = reader.GetInt32(1);
					model.uid = reader.GetString(2);
					model.type = reader.GetInt32(3);
					model.count = reader.GetInt32(4);
					model.List = reader.GetString(5);
					model.uided = reader.GetString(6);
					model.acount = reader.GetInt32(7);
					model.wcount = reader.GetInt32(8);
					model.ycount = reader.GetString(9);
					model.ubbtext = reader.GetString(10);
					model.passtime = reader.GetInt32(11);
					model.awardtype = reader.GetInt32(12);
					model.awardptype = reader.GetInt32(13);
					model.award = reader.GetInt32(14);
					model.addtime = reader.GetDateTime(15);
					model.overtime = reader.GetDateTime(16);
					model.remark = reader.GetString(17);
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
			strSql.Append(" FROM tb_QuestionController ");
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
		/// <returns>IList tb_QuestionController</returns>
		public IList<BCW.Model.tb_QuestionController> Gettb_QuestionControllers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_QuestionController> listtb_QuestionControllers = new List<BCW.Model.tb_QuestionController>();
			string sTable = "tb_QuestionController";
			string sPkey = "id";
			string sField = "ID,muid,uid,type,count,List,uided,acount,wcount,ycount,ubbtext,passtime,awardtype,awardptype,award,addtime,overtime,remark";
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
					return listtb_QuestionControllers;
				}
				while (reader.Read())
				{
						BCW.Model.tb_QuestionController objtb_QuestionController = new BCW.Model.tb_QuestionController();
						objtb_QuestionController.ID = reader.GetInt32(0);
						objtb_QuestionController.muid = reader.GetInt32(1);
						objtb_QuestionController.uid = reader.GetString(2);
						objtb_QuestionController.type = reader.GetInt32(3);
						objtb_QuestionController.count = reader.GetInt32(4);
						objtb_QuestionController.List = reader.GetString(5);
						objtb_QuestionController.uided = reader.GetString(6);
						objtb_QuestionController.acount = reader.GetInt32(7);
						objtb_QuestionController.wcount = reader.GetInt32(8);
						objtb_QuestionController.ycount = reader.GetString(9);
						objtb_QuestionController.ubbtext = reader.GetString(10);
						objtb_QuestionController.passtime = reader.GetInt32(11);
						objtb_QuestionController.awardtype = reader.GetInt32(12);
						objtb_QuestionController.awardptype = reader.GetInt32(13);
						objtb_QuestionController.award = reader.GetInt32(14);
						objtb_QuestionController.addtime = reader.GetDateTime(15);
						objtb_QuestionController.overtime = reader.GetDateTime(16);
						objtb_QuestionController.remark = reader.GetString(17);
						listtb_QuestionControllers.Add(objtb_QuestionController);
				}
			}
			return listtb_QuestionControllers;
		}

		#endregion  成员方法
	}
}

