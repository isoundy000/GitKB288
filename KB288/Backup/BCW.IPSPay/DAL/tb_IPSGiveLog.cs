/**  版本信息模板在安装目录下，可自行修改。
* tb_IPSGiveLog.cs
*
* 功 能： N/A
* 类 名： tb_IPSGiveLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/7/16 14:57:17   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using System.Collections.Generic;
using BCW.Common;

namespace BCW.IPSPay.DAL
{
	/// <summary>
	/// 数据访问类:tb_IPSGiveLog
	/// </summary>
	public partial class tb_IPSGiveLog
	{
		public tb_IPSGiveLog()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.IPSPay.Model.tb_IPSGiveLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_IPSGiveLog(");
			strSql.Append("ManageID,BzNote,GetMoney,G_arge,G_type,addtime)");
			strSql.Append(" values (");
			strSql.Append("@ManageID,@BzNote,@GetMoney,@G_arge,@G_type,@addtime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ManageID", SqlDbType.Int,4),
					new SqlParameter("@BzNote", SqlDbType.NVarChar,300),
					new SqlParameter("@GetMoney", SqlDbType.Decimal,9),
					new SqlParameter("@G_arge", SqlDbType.Decimal,9),
					new SqlParameter("@G_type", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime)};
			parameters[0].Value = model.ManageID;
			parameters[1].Value = model.BzNote;
			parameters[2].Value = model.GetMoney;
			parameters[3].Value = model.G_arge;
			parameters[4].Value = model.G_type;
			parameters[5].Value = model.addtime;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(BCW.IPSPay.Model.tb_IPSGiveLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_IPSGiveLog set ");
			strSql.Append("ManageID=@ManageID,");
			strSql.Append("BzNote=@BzNote,");
			strSql.Append("GetMoney=@GetMoney,");
			strSql.Append("G_arge=@G_arge,");
			strSql.Append("G_type=@G_type,");
			strSql.Append("addtime=@addtime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@ManageID", SqlDbType.Int,4),
					new SqlParameter("@BzNote", SqlDbType.NVarChar,300),
					new SqlParameter("@GetMoney", SqlDbType.Decimal,9),
					new SqlParameter("@G_arge", SqlDbType.Decimal,9),
					new SqlParameter("@G_type", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.ManageID;
			parameters[1].Value = model.BzNote;
			parameters[2].Value = model.GetMoney;
			parameters[3].Value = model.G_arge;
			parameters[4].Value = model.G_type;
			parameters[5].Value = model.addtime;
			parameters[6].Value = model.id;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
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
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_IPSGiveLog ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_IPSGiveLog ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=SqlHelper.ExecuteSql(strSql.ToString());
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
		public BCW.IPSPay.Model.tb_IPSGiveLog GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,ManageID,BzNote,GetMoney,G_arge,G_type,addtime from tb_IPSGiveLog ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			BCW.IPSPay.Model.tb_IPSGiveLog model=new BCW.IPSPay.Model.tb_IPSGiveLog();
			DataSet ds=SqlHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public BCW.IPSPay.Model.tb_IPSGiveLog DataRowToModel(DataRow row)
		{
			BCW.IPSPay.Model.tb_IPSGiveLog model=new BCW.IPSPay.Model.tb_IPSGiveLog();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["ManageID"]!=null && row["ManageID"].ToString()!="")
				{
					model.ManageID=int.Parse(row["ManageID"].ToString());
				}
				if(row["BzNote"]!=null)
				{
					model.BzNote=row["BzNote"].ToString();
				}
				if(row["GetMoney"]!=null && row["GetMoney"].ToString()!="")
				{
					model.GetMoney=decimal.Parse(row["GetMoney"].ToString());
				}
				if(row["G_arge"]!=null && row["G_arge"].ToString()!="")
				{
					model.G_arge=decimal.Parse(row["G_arge"].ToString());
				}
				if(row["G_type"]!=null && row["G_type"].ToString()!="")
				{
					model.G_type=int.Parse(row["G_type"].ToString());
				}
				if(row["addtime"]!=null && row["addtime"].ToString()!="")
				{
					model.addtime=DateTime.Parse(row["addtime"].ToString());
				}
			}
			return model;
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Goldlog</returns>
        public IList<BCW.IPSPay.Model.tb_IPSGiveLog> GetIPSGiveLogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.IPSPay.Model.tb_IPSGiveLog> listIPSGiveLogs = new List<BCW.IPSPay.Model.tb_IPSGiveLog>();
            string sTable = "tb_IPSGiveLog";
            string sPkey = "id";
            string sField = " id, ManageID, BzNote, GetMoney, G_arge, G_type, addtime";
            string sCondition = strWhere;
            string sOrder = "addtime Desc";
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
                    return listIPSGiveLogs;
                }
                while (reader.Read())
                {
                    BCW.IPSPay.Model.tb_IPSGiveLog objGiveLog = new BCW.IPSPay.Model.tb_IPSGiveLog();
                    objGiveLog.id = reader.GetInt32(0);
                    objGiveLog.ManageID = reader.GetInt32(1);
                    objGiveLog.BzNote = reader.GetString(2);
                    objGiveLog.GetMoney = reader.GetDecimal(3);
                    objGiveLog.G_arge = reader.GetDecimal(4);
                    objGiveLog.G_type = reader.GetInt32(5);                    
                    objGiveLog.addtime = reader.GetDateTime(6);
                    listIPSGiveLogs.Add(objGiveLog);
                }
            }
            return listIPSGiveLogs;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,ManageID,BzNote,GetMoney,G_arge,G_type,addtime ");
			strSql.Append(" FROM tb_IPSGiveLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// 获得总取款
        /// </summary>
        public double GetTotal(int G_type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(GetMoney) AS total ");
            strSql.Append(" FROM tb_IPSGiveLog ");
            strSql.Append(" where G_type = " + G_type.ToString());
            DataSet ds = SqlHelper.Query(strSql.ToString());
            double total = 0;
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string tl = ds.Tables[0].Rows[0]["total"].ToString();
                        if (tl != "")
                        {
                            total = double.Parse(tl);
                        }
                        return total;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" id,ManageID,BzNote,GetMoney,G_arge,G_type,addtime ");
			strSql.Append(" FROM tb_IPSGiveLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_IPSGiveLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from tb_IPSGiveLog T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return SqlHelper.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "tb_IPSGiveLog";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return SqlHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

