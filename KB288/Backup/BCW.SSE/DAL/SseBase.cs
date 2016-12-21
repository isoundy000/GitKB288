using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BCW.Data;
using BCW.Common;

namespace BCW.SSE.DAL
{
	/// <summary>
	/// 数据访问类:SseBase
	/// </summary>
	public partial class SseBase
	{
		public SseBase()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.SSE.Model.SseBase model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SseBase(");
			strSql.Append("sseNo,closePrice,bz)");
			strSql.Append(" values (");
			strSql.Append("@sseNo,@closePrice,@bz)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@closePrice", SqlDbType.Decimal,9),
					new SqlParameter("@bz", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.sseNo;
			parameters[1].Value = model.closePrice;
			parameters[2].Value = model.bz;

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
		public bool Update(BCW.SSE.Model.SseBase model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SseBase set ");
			strSql.Append("sseNo=@sseNo,");
			strSql.Append("closePrice=@closePrice,");
			strSql.Append("bz=@bz");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@closePrice", SqlDbType.Decimal,9),
					new SqlParameter("@bz", SqlDbType.NVarChar,200),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.sseNo;
			parameters[1].Value = model.closePrice;
			parameters[2].Value = model.bz;
			parameters[3].Value = model.id;

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
			strSql.Append("delete from tb_SseBase ");
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
			strSql.Append("delete from tb_SseBase ");
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
		public BCW.SSE.Model.SseBase GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,sseNo,closePrice,bz from tb_SseBase ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			BCW.SSE.Model.SseBase model=new BCW.SSE.Model.SseBase();
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
		public BCW.SSE.Model.SseBase DataRowToModel(DataRow row)
		{
			BCW.SSE.Model.SseBase model=new BCW.SSE.Model.SseBase();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["sseNo"]!=null && row["sseNo"].ToString()!="")
				{
					model.sseNo=int.Parse(row["sseNo"].ToString());
				}
				if(row["closePrice"]!=null && row["closePrice"].ToString()!="")
				{
					model.closePrice=decimal.Parse(row["closePrice"].ToString());
				}
				if(row["bz"]!=null)
				{
					model.bz=row["bz"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,sseNo,closePrice,bz ");
			strSql.Append(" FROM tb_SseBase ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
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
			strSql.Append(" id,sseNo,closePrice,bz ");
			strSql.Append(" FROM tb_SseBase ");
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
			strSql.Append("select count(1) FROM tb_SseBase ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = BCW.Data.SqlHelper.GetSingle(strSql.ToString());
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
			strSql.Append(")AS Row, T.*  from tb_SseBase T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return SqlHelper.Query(strSql.ToString());
		}


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.SSE.Model.SseBase> GetSseBasePages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<BCW.SSE.Model.SseBase> listSseBase = new List<BCW.SSE.Model.SseBase>();
            string sTable = "tb_SseBase";
            string sPkey = "id";
            string sField = "id,sseNo,closePrice,bz";
            string sCondition = strWhere;
            string sOrder = "sseNo Desc";
            int iSCounts = 0;
            using( SqlDataReader reader = SqlHelper.RunProcedureMe( sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount ) )
            {
                //计算总页数
                if( p_recordCount > 0 )
                {
                    int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
                }
                else
                {
                    return listSseBase;
                }
                while( reader.Read() )
                {
                    BCW.SSE.Model.SseBase objSseBase = new BCW.SSE.Model.SseBase();
                    objSseBase.id = reader.GetInt32( 0 );
                    objSseBase.sseNo = reader.GetInt32( 1 );
                    objSseBase.closePrice = reader.GetDecimal( 2 );
                    objSseBase.bz = reader.GetString( 3 );


                    listSseBase.Add( objSseBase );

                }
            }
            return listSseBase;
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
			parameters[0].Value = "tb_SseBase";
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

