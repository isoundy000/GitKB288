using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Collec
{
	/// <summary>
	/// 数据访问类Collecdata。
	/// </summary>
	public class Collecdata
	{
		public Collecdata()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Collecdata"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Collecdata");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 计算某采集项目的采集数
        /// </summary>
        public int GetCount(int ItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Collecdata");
            strSql.Append(" where ItemId=@ItemId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.Int,4)};
            parameters[0].Value = ItemId;
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
		public int Add(BCW.Model.Collec.Collecdata model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Collecdata(");
			strSql.Append("ItemId,Types,NodeId,Title,KeyWord,Content,Pics,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@ItemId,@Types,@NodeId,@Title,@KeyWord,@Content,@Pics,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWord", SqlDbType.NVarChar,200),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Pics", SqlDbType.NText),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ItemId;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.NodeId;
			parameters[3].Value = model.Title;
			parameters[4].Value = model.KeyWord;
			parameters[5].Value = model.Content;
			parameters[6].Value = model.Pics;
			parameters[7].Value = model.AddTime;

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
		public void Update(BCW.Model.Collec.Collecdata model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Collecdata set ");
			strSql.Append("ItemId=@ItemId,");
			strSql.Append("Types=@Types,");
			strSql.Append("NodeId=@NodeId,");
			strSql.Append("Title=@Title,");
			strSql.Append("KeyWord=@KeyWord,");
			strSql.Append("Content=@Content,");
			strSql.Append("Pics=@Pics,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ItemId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWord", SqlDbType.NVarChar,200),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Pics", SqlDbType.NText),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.ItemId;
			parameters[2].Value = model.Types;
			parameters[3].Value = model.NodeId;
			parameters[4].Value = model.Title;
			parameters[5].Value = model.KeyWord;
			parameters[6].Value = model.Content;
			parameters[7].Value = model.Pics;
			parameters[8].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Collecdata ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Collecdata ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Collec.Collecdata GetCollecdata(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,ItemId,Types,NodeId,Title,KeyWord,Content,Pics,AddTime from tb_Collecdata ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Collec.Collecdata model=new BCW.Model.Collec.Collecdata();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.ItemId = reader.GetInt32(1);
					model.Types = reader.GetInt32(2);
					model.NodeId = reader.GetInt32(3);
					model.Title = reader.GetString(4);
					model.KeyWord = reader.GetString(5);
					model.Content = reader.GetString(6);
					model.Pics = reader.GetString(7);
					model.AddTime = reader.GetDateTime(8);
					return model;
				}
				else
				{
				return null;
				}
			}
		}

        /// <summary>
        /// 得到一个Pics
        /// </summary>
        public string GetPics(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Pics from tb_Collecdata ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_Collecdata ");
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
		/// <returns>IList Collecdata</returns>
		public IList<BCW.Model.Collec.Collecdata> GetCollecdatas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Collec.Collecdata> listCollecdatas = new List<BCW.Model.Collec.Collecdata>();
			string sTable = "tb_Collecdata";
			string sPkey = "id";
			string sField = "ID,ItemId,Types,NodeId,Title,KeyWord,Content,Pics,AddTime";
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
					return listCollecdatas;
				}
				while (reader.Read())
				{
						BCW.Model.Collec.Collecdata objCollecdata = new BCW.Model.Collec.Collecdata();
						objCollecdata.ID = reader.GetInt32(0);
						objCollecdata.ItemId = reader.GetInt32(1);
						objCollecdata.Types = reader.GetInt32(2);
						objCollecdata.NodeId = reader.GetInt32(3);
						objCollecdata.Title = reader.GetString(4);
						objCollecdata.KeyWord = reader.GetString(5);
						objCollecdata.Content = reader.GetString(6);
						objCollecdata.Pics = reader.GetString(7);
						objCollecdata.AddTime = reader.GetDateTime(8);
						listCollecdatas.Add(objCollecdata);
				}
			}
			return listCollecdatas;
		}

		#endregion  成员方法
	}
}

