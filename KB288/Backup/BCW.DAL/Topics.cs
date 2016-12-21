using System;
using System.Web;
using System.Data;
using System.Text;
using System.Collections.Generic;
using BCW.Common;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using BCW.Data;

namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Topics。
    /// </summary>
    public class Topics
    {
        public Topics()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Topics");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Topics");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该页面菜单
        /// </summary>
        public bool ExistsTypes(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Topics");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Types=@Types");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 菜单里是否存在页面菜单
        /// </summary>
        public bool ExistsTypesIn(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Topics");
            strSql.Append(" where NodeId=@NodeId ");
            strSql.Append(" and (Types=1 or Types>10)");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsIdTypes(int ID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Topics");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Types;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsIdLeibie(int ID, int Leibie)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Topics");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Leibie=@Leibie ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Leibie", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Leibie;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.Topics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Topics(");
            strSql.Append("ID,Leibie,NodeId,Types,Title,Content,IsBr,IsPc,Cent,SellTypes,InPwd,BzType,VipLeven,Paixu,Hidden)");
            strSql.Append(" values (");
            strSql.Append("@ID,@Leibie,@NodeId,@Types,@Title,@Content,@IsBr,@IsPc,@Cent,@SellTypes,@InPwd,@BzType,@VipLeven,@Paixu,@Hidden)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Leibie", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@IsBr", SqlDbType.Int,4),
					new SqlParameter("@IsPc", SqlDbType.Int,4),
					new SqlParameter("@Cent", SqlDbType.Int,4),
					new SqlParameter("@SellTypes", SqlDbType.Int,4),
                    new SqlParameter("@InPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@BzType", SqlDbType.Int,4),
					new SqlParameter("@VipLeven", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@Hidden", SqlDbType.Int,4)};
            parameters[0].Value = GetMaxId();//得到此时的ID
            parameters[1].Value = model.Leibie;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.Types;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.IsBr;
            parameters[7].Value = model.IsPc;
            parameters[8].Value = model.Cent;
            parameters[9].Value = model.SellTypes;
            parameters[10].Value = model.InPwd;
            parameters[11].Value = model.BzType;
            parameters[12].Value = model.VipLeven;
            parameters[13].Value = model.Paixu;
            parameters[14].Value = model.Hidden;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Topics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Topics set ");
            strSql.Append("Leibie=@Leibie,");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("IsBr=@IsBr,");
            strSql.Append("IsPc=@IsPc,");
            strSql.Append("Cent=@Cent,");
            strSql.Append("SellTypes=@SellTypes,");
            strSql.Append("InPwd=@InPwd,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("VipLeven=@VipLeven,");
            strSql.Append("Paixu=@Paixu,");
            strSql.Append("Hidden=@Hidden");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Leibie", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@IsBr", SqlDbType.Int,4),
					new SqlParameter("@IsPc", SqlDbType.Int,4),
					new SqlParameter("@Cent", SqlDbType.Int,4),
					new SqlParameter("@SellTypes", SqlDbType.Int,4),
                    new SqlParameter("@InPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@BzType", SqlDbType.Int,4),
					new SqlParameter("@VipLeven", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@Hidden", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Leibie;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.Types;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.IsBr;
            parameters[7].Value = model.IsPc;
            parameters[8].Value = model.Cent;
            parameters[9].Value = model.SellTypes;
            parameters[10].Value = model.InPwd;
            parameters[11].Value = model.BzType;
            parameters[12].Value = model.VipLeven;
            parameters[13].Value = model.Paixu;
            parameters[14].Value = model.Hidden;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Topics set ");
            strSql.Append("NodeId=@NodeId ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Topics set ");
            strSql.Append("Paixu=@Paixu ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新购买ID
        /// </summary>
        public void UpdatePayId(int ID, string PayId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Topics set ");
            strSql.Append("PayId=@PayId ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PayId", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = PayId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Topics ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除该页面菜单下的所有子菜单
        /// </summary>
        public void DeleteNodeId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Topics ");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个页面内容
        /// </summary>
        public BCW.Model.Topics GetTopics(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Leibie,NodeId,Types,Title,Content,IsBr,IsPc,Cent,SellTypes,InPwd,PayId,BzType,VipLeven,Paixu,Hidden from tb_Topics ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.Topics model = new BCW.Model.Topics();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {

                if (reader.HasRows)
                {
                    reader.Read();
                    model.Leibie = reader.GetInt32(0);
                    model.NodeId = reader.GetInt32(1);
                    model.Types = reader.GetInt32(2);
                    model.Title = reader.GetString(3);
                    model.Content = reader.GetString(4);
                    model.IsBr = reader.GetInt32(5);
                    model.IsPc = reader.GetInt32(6);
                    model.Cent = reader.GetInt32(7);
                    model.SellTypes = reader.GetInt32(8);
                    model.InPwd = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        model.PayId = reader.GetString(10);
                    model.BzType = reader.GetInt32(11);
                    model.VipLeven = reader.GetInt32(12);
                    model.Paixu = reader.GetInt32(13);
                    model.Hidden = reader.GetInt32(14);
                    return model;
                }
                else
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 得到节点名称
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Topics ");
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
        /// 得到节点NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NodeId from tb_Topics ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

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
        /// 得到类型Types
        /// </summary>
        public int GetTypes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Types from tb_Topics ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

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
        /// 得到大类型Leibie
        /// </summary>
        public int GetLeibie(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Leibie from tb_Topics ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NodeId,Title,Content,Types,IsBr,VipLeven,Paixu");
            strSql.Append(" FROM tb_Topics ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Topics ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Topics> GetTopicss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {

            IList<BCW.Model.Topics> listTopicss = new List<BCW.Model.Topics>();

            string sTable = "tb_Topics";
            string sPkey = "id";
            string sField = "ID,NodeId,Title,Types,Paixu,Hidden";
            string sCondition = strWhere;
            string sOrder = "Paixu ASC";
            int iSCounts = 0;

            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {

                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {

                    return listTopicss;
                }

                while (reader.Read())
                {
                    BCW.Model.Topics objTopics = new BCW.Model.Topics();
                    objTopics.ID = reader.GetInt32(0);
                    objTopics.NodeId = reader.GetInt32(1);
                    objTopics.Title = reader.GetString(2);
                    objTopics.Types = reader.GetInt32(3);
                    objTopics.Paixu = reader.GetInt32(4);
                    objTopics.Hidden = reader.GetInt32(5);
                    listTopicss.Add(objTopics);


                }
            }
            return listTopicss;
        }

        #endregion  成员方法
    }


}
