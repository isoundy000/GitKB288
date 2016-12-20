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
	/// 数据访问类CmgDaoju。
	/// </summary>
	public class CmgDaoju
	{
		public CmgDaoju()
		{}
		#region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_CmgDaoju");
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_CmgDaoju");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.bydr.Model.CmgDaoju model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_CmgDaoju(");
			strSql.Append("Daoju,Tianyuan,Xiaoxi,Heliu,Caoc,Huayuan,Senlin,changj1,changj2,Changjing)");
			strSql.Append(" values (");
			strSql.Append("@Daoju,@Tianyuan,@Xiaoxi,@Heliu,@Caoc,@Huayuan,@Senlin,@changj1,@changj2,@Changjing)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Daoju", SqlDbType.NVarChar,50),
					new SqlParameter("@Tianyuan", SqlDbType.BigInt,8),
					new SqlParameter("@Xiaoxi", SqlDbType.BigInt,8),
					new SqlParameter("@Heliu", SqlDbType.BigInt,8),
					new SqlParameter("@Caoc", SqlDbType.BigInt,8),
					new SqlParameter("@Huayuan", SqlDbType.BigInt,8),
					new SqlParameter("@Senlin", SqlDbType.BigInt,8),
					new SqlParameter("@changj1", SqlDbType.NVarChar,50),
					new SqlParameter("@changj2", SqlDbType.BigInt,8),
					new SqlParameter("@Changjing", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.Daoju;
			parameters[1].Value = model.Tianyuan;
			parameters[2].Value = model.Xiaoxi;
			parameters[3].Value = model.Heliu;
			parameters[4].Value = model.Caoc;
			parameters[5].Value = model.Huayuan;
			parameters[6].Value = model.Senlin;
			parameters[7].Value = model.changj1;
			parameters[8].Value = model.changj2;
			parameters[9].Value = model.Changjing;

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
		public void Update(BCW.bydr.Model.CmgDaoju model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_CmgDaoju set ");
			strSql.Append("Daoju=@Daoju,");
			strSql.Append("Tianyuan=@Tianyuan,");
			strSql.Append("Xiaoxi=@Xiaoxi,");
			strSql.Append("Heliu=@Heliu,");
			strSql.Append("Caoc=@Caoc,");
			strSql.Append("Huayuan=@Huayuan,");
			strSql.Append("Senlin=@Senlin,");
			strSql.Append("changj1=@changj1,");
			strSql.Append("changj2=@changj2,");
			strSql.Append("Changjing=@Changjing");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Daoju", SqlDbType.NVarChar,50),
					new SqlParameter("@Tianyuan", SqlDbType.BigInt,8),
					new SqlParameter("@Xiaoxi", SqlDbType.BigInt,8),
					new SqlParameter("@Heliu", SqlDbType.BigInt,8),
					new SqlParameter("@Caoc", SqlDbType.BigInt,8),
					new SqlParameter("@Huayuan", SqlDbType.BigInt,8),
					new SqlParameter("@Senlin", SqlDbType.BigInt,8),
					new SqlParameter("@changj1", SqlDbType.NVarChar,50),
					new SqlParameter("@changj2", SqlDbType.BigInt,8),
					new SqlParameter("@Changjing", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Daoju;
			parameters[2].Value = model.Tianyuan;
			parameters[3].Value = model.Xiaoxi;
			parameters[4].Value = model.Heliu;
			parameters[5].Value = model.Caoc;
			parameters[6].Value = model.Huayuan;
			parameters[7].Value = model.Senlin;
			parameters[8].Value = model.changj1;
			parameters[9].Value = model.changj2;
			parameters[10].Value = model.Changjing;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_CmgDaoju ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.bydr.Model.CmgDaoju GetCmgDaoju(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Daoju,Tianyuan,Xiaoxi,Heliu,Caoc,Huayuan,Senlin,changj1,changj2,Changjing from tb_CmgDaoju ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.bydr.Model.CmgDaoju model=new BCW.bydr.Model.CmgDaoju();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Daoju = reader.GetString(1);
					model.Tianyuan = reader.GetInt64(2);
					model.Xiaoxi = reader.GetInt64(3);
					model.Heliu = reader.GetInt64(4);
					model.Caoc = reader.GetInt64(5);
					model.Huayuan = reader.GetInt64(6);
					model.Senlin = reader.GetInt64(7);
                    model.changj1 = reader.GetString(8);
					model.changj2 = reader.GetInt64(9);
					model.Changjing = reader.GetString(10);
					return model;
				}
				else
				{
				return null;
				}
			}
		}
        /// <summary>
        /// 得到鱼的名称
        /// </summary>
        public string GetyuName(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Daoju from tb_CmgDaoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_CmgDaoju ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}


        ///// <summary>
        ///// 根据场景查询入场费用
        ///// </summary>
        //public int Getchangj2(int changj2, string Changjing)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select * from tb_CmgDaoju");
        //    strSql.Append(" where Changjing=@Changjing ");
          
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@changj2", SqlDbType.Int,4)};
        //    parameters[0].Value = 0;

        //    object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
        //    if (obj == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return Convert.ToInt32(obj);
        //    }

        //}

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList CmgDaoju</returns>
		public IList<BCW.bydr.Model.CmgDaoju> GetCmgDaojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.bydr.Model.CmgDaoju> listCmgDaojus = new List<BCW.bydr.Model.CmgDaoju>();
			string sTable = "tb_CmgDaoju";
			string sPkey = "id";
			string sField = "ID,Daoju,Tianyuan,Xiaoxi,Heliu,Caoc,Huayuan,Senlin,changj1,changj2,Changjing";
			string sCondition = strWhere;
			string sOrder = "ID";
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
					return listCmgDaojus;
				}
				while (reader.Read())
				{
						BCW.bydr.Model.CmgDaoju objCmgDaoju = new BCW.bydr.Model.CmgDaoju();
						objCmgDaoju.ID = reader.GetInt32(0);
						objCmgDaoju.Daoju = reader.GetString(1);
						objCmgDaoju.Tianyuan = reader.GetInt64(2);
						objCmgDaoju.Xiaoxi = reader.GetInt64(3);
						objCmgDaoju.Heliu = reader.GetInt64(4);
						objCmgDaoju.Caoc = reader.GetInt64(5);
						objCmgDaoju.Huayuan = reader.GetInt64(6);
						objCmgDaoju.Senlin = reader.GetInt64(7);
                        objCmgDaoju.changj1 = reader.GetString(8);
						objCmgDaoju.changj2 = reader.GetInt64(9);
						objCmgDaoju.Changjing = reader.GetString(10);
						listCmgDaojus.Add(objCmgDaoju);
				}
			}
			return listCmgDaojus;
		}

		#endregion  成员方法
	}
}

