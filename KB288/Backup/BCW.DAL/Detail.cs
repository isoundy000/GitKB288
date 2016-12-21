using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Detail。
    /// </summary>
    public class Detail
    {
        public Detail()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Detail");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Detail");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string Title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Detail");
            strSql.Append(" where Title=@Title ");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50)};
            parameters[0].Value = Title;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Detail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Detail(");
            strSql.Append("NodeId,Types,IsAd,Title,KeyWord,Model,Content,TarText,LanText,SafeText,LyText,UpText,IsVisa,Readcount,Recount,BzType,Cent,AddTime,UsID,Hidden)");
            strSql.Append(" values (");
            strSql.Append("@NodeId,@Types,@IsAd,@Title,@KeyWord,@Model,@Content,@TarText,@LanText,@SafeText,@LyText,@UpText,@IsVisa,@Readcount,@Recount,@BzType,@Cent,@AddTime,@UsID,@Hidden)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@IsAd", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@KeyWord", SqlDbType.NVarChar,500),
                    new SqlParameter("@Model", SqlDbType.NText),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@TarText", SqlDbType.NVarChar,50),
					new SqlParameter("@LanText", SqlDbType.NVarChar,50),
					new SqlParameter("@SafeText", SqlDbType.NVarChar,50),
					new SqlParameter("@LyText", SqlDbType.NVarChar,50),
					new SqlParameter("@UpText", SqlDbType.NVarChar,50),
					new SqlParameter("@IsVisa", SqlDbType.Int,4),
					new SqlParameter("@Readcount", SqlDbType.Int,4),
					new SqlParameter("@Recount", SqlDbType.Int,4),
                    new SqlParameter("@BzType", SqlDbType.Int,4),
                    new SqlParameter("@Cent", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@Hidden", SqlDbType.Int,4)};
            parameters[0].Value = model.NodeId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.IsAd;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.KeyWord;
            parameters[5].Value = model.Model;
            parameters[6].Value = model.Content;
            parameters[7].Value = model.TarText;
            parameters[8].Value = model.LanText;
            parameters[9].Value = model.SafeText;
            parameters[10].Value = model.LyText;
            parameters[11].Value = model.UpText;
            parameters[12].Value = model.IsVisa;
            parameters[13].Value = model.Readcount;
            parameters[14].Value = model.Recount;
            parameters[15].Value = model.BzType;
            parameters[16].Value = model.Cent;
            parameters[17].Value = model.AddTime;
            parameters[18].Value = model.UsID;
            parameters[19].Value = model.Hidden;

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
        public void Update(BCW.Model.Detail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Types=@Types,");
            strSql.Append("IsAd=@IsAd,");
            strSql.Append("Title=@Title,");
            strSql.Append("KeyWord=@KeyWord,");
            strSql.Append("Model=@Model,");
            strSql.Append("Content=@Content,");
            strSql.Append("TarText=@TarText,");
            strSql.Append("LanText=@LanText,");
            strSql.Append("SafeText=@SafeText,");
            strSql.Append("LyText=@LyText,");
            strSql.Append("UpText=@UpText,");
            strSql.Append("IsVisa=@IsVisa,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("Readcount=@Readcount,");
            strSql.Append("Recount=@Recount,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("Cent=@Cent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@IsAd", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@KeyWord", SqlDbType.NVarChar,500),
                    new SqlParameter("@Model", SqlDbType.NText),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@TarText", SqlDbType.NVarChar,50),
					new SqlParameter("@LanText", SqlDbType.NVarChar,50),
					new SqlParameter("@SafeText", SqlDbType.NVarChar,50),
					new SqlParameter("@LyText", SqlDbType.NVarChar,50),
					new SqlParameter("@UpText", SqlDbType.NVarChar,50),
					new SqlParameter("@IsVisa", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Readcount", SqlDbType.Int,4),
					new SqlParameter("@Recount", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.Int,4),
            	    new SqlParameter("@Cent", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Types;
            parameters[3].Value = model.IsAd;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.KeyWord;
            parameters[6].Value = model.Model;
            parameters[7].Value = model.Content;
            parameters[8].Value = model.TarText;
            parameters[9].Value = model.LanText;
            parameters[10].Value = model.SafeText;
            parameters[11].Value = model.LyText;
            parameters[12].Value = model.UpText;
            parameters[13].Value = model.IsVisa;
            parameters[14].Value = model.AddTime;
            parameters[15].Value = model.Readcount;
            parameters[16].Value = model.Recount;
            parameters[17].Value = model.BzType;
            parameters[18].Value = model.Cent;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新截图
        /// </summary>
        public void UpdatePics(int ID, string Pics)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("Pics=@Pics ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Pics", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = Pics;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新封面
        /// </summary>
        public void UpdateCover(int ID, string Cover)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("Cover=@Cover ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Cover", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Cover;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新点击ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClickID"></param>
        public void UpdateClickID(int ID,string ClickID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("ClickID=@ClickID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@ClickID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = ClickID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats,string ReLastIP)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("ReStats=@ReStats, ");
            strSql.Append("ReLastIP=@ReLastIP ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReStats", SqlDbType.NVarChar,50),
            		new SqlParameter("@ReLastIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = ReStats;
            parameters[2].Value = ReLastIP;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新点击数
        /// </summary>
        public void UpdateReadcount(int ID, int Readcount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("Readcount=Readcount+@Readcount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Readcount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Readcount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新评论条数
        /// </summary>
        public void UpdateRecount(int ID, int Recount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("Recount=Recount+@Recount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Recount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Recount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
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
        /// 批量更新节点
        /// </summary>
        public void UpdateNodeIds(int NewNodeId, int OrdNodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("NodeId=@NewNodeId ");
            strSql.Append(" where NodeId=@OrdNodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NewNodeId", SqlDbType.Int,4),
					new SqlParameter("@OrdNodeId", SqlDbType.Int,4)};
            parameters[0].Value = NewNodeId;
            parameters[1].Value = OrdNodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新购买ID
        /// </summary>
        public void UpdatePayId(int ID, string PayId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
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
        /// 审核文件
        /// </summary>
        public void UpdateHidden(int ID, int Hidden)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("Hidden=@Hidden");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Hidden", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Hidden;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除截图记录
        /// </summary>
        public void DeletePics(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Detail set ");
            strSql.Append("Pics=@Pics, ");
            strSql.Append("Cover=@Cover ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Pics", SqlDbType.NText),
					new SqlParameter("@Cover", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = "";
            parameters[2].Value = "";

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Detail ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除该节点下的所有文章
        /// </summary>
        public void DeleteNodeId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Detail ");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Detail GetDetail(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,NodeId,Types,IsAd,Title,KeyWord,Model,Pics,Content,TarText,LanText,SafeText,LyText,UpText,IsVisa,ReStats,ReLastIP,Readcount,Recount,Cent,BzType,PayId,AddTime,ClickID from tb_Detail ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Detail model = new BCW.Model.Detail();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["NodeId"].ToString() != "")
                {
                    model.NodeId = int.Parse(ds.Tables[0].Rows[0]["NodeId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Types"].ToString() != "")
                {
                    model.Types = int.Parse(ds.Tables[0].Rows[0]["Types"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsAd"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsAd"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsAd"].ToString().ToLower() == "true"))
                    {
                        model.IsAd = true;
                    }
                    else
                    {
                        model.IsAd = false;
                    }
                }
                model.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                model.KeyWord = ds.Tables[0].Rows[0]["KeyWord"].ToString();
                model.Model = ds.Tables[0].Rows[0]["Model"].ToString();
                model.Pics = ds.Tables[0].Rows[0]["Pics"].ToString();
                model.Content = ds.Tables[0].Rows[0]["Content"].ToString();
                model.TarText = ds.Tables[0].Rows[0]["TarText"].ToString();
                model.LanText = ds.Tables[0].Rows[0]["LanText"].ToString();
                model.SafeText = ds.Tables[0].Rows[0]["SafeText"].ToString();
                model.LyText = ds.Tables[0].Rows[0]["LyText"].ToString();
                model.UpText = ds.Tables[0].Rows[0]["UpText"].ToString();

                if (ds.Tables[0].Rows[0]["IsVisa"].ToString() != "")
                {
                    model.IsVisa = int.Parse(ds.Tables[0].Rows[0]["IsVisa"].ToString());
                }

                model.ReStats = ds.Tables[0].Rows[0]["ReStats"].ToString();
                model.ReLastIP = ds.Tables[0].Rows[0]["ReLastIP"].ToString();
                if (ds.Tables[0].Rows[0]["Readcount"].ToString() != "")
                {
                    model.Readcount = int.Parse(ds.Tables[0].Rows[0]["Readcount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Recount"].ToString() != "")
                {
                    model.Recount = int.Parse(ds.Tables[0].Rows[0]["Recount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Cent"].ToString() != "")
                {
                    model.Cent = int.Parse(ds.Tables[0].Rows[0]["Cent"].ToString());
                }
                model.BzType = int.Parse(ds.Tables[0].Rows[0]["BzType"].ToString());
                model.PayId = ds.Tables[0].Rows[0]["PayId"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                model.ClickID= ds.Tables[0].Rows[0]["ClickID"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到标题Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Detail ");
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
        /// 得到适用型号
        /// </summary>
        public string GetPhoneModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Model from tb_Detail ");
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
            strSql.Append("select NodeId from tb_Detail ");
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
            strSql.Append("select Types from tb_Detail ");
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
        /// 得到用户ID
        /// </summary>
        public int GetUsID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsID from tb_Detail ");
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
        /// 得到截图Pics
        /// </summary>
        public string GetPics(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Pics from tb_Detail ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到封面Cover
        /// </summary>
        public string GetCover(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Cover from tb_Detail ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_Detail ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得上(下)一条记录
        /// </summary>
        public BCW.Model.Detail GetPreviousNextDetail(int ID, int p_NodeId, bool p_next)
        {
            List<BCW.Model.Detail> listDetail = new List<BCW.Model.Detail>();

            // 搜索部分
            SqlParameter[] tmpSqlParam = {
                new SqlParameter("@NodeId", SqlDbType.Int, 4),
                new SqlParameter("@ID", SqlDbType.Int, 4)
            };

            string where = "";
            if (p_NodeId != 0)
            {
                where += " NodeId=@NodeId AND";
                tmpSqlParam[0].Value = p_NodeId;
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
                order = " and Hidden=0 ORDER BY ID DESC";
            }
            else
            {
                order = " and Hidden=0 ORDER BY ID ASC";
            }


            // 取出相关记录

            BCW.Model.Detail objDetail = new BCW.Model.Detail();

            string queryString = "SELECT TOP 1 ID, Title" +
                                " FROM tb_Detail" + where + order;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString.ToString(), SqlParam))
            {

                while (reader.Read())
                {

                    objDetail.ID = reader.GetInt32(0);
                    objDetail.Title = reader.GetString(1);
                }
            }

            return objDetail;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Detail</returns>
        public IList<BCW.Model.Detail> GetDetails(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Detail> listDetails = new List<BCW.Model.Detail>();
            string sTable = "tb_Detail";
            string sPkey = "id";
            string sField = "ID,IsAd,Title,Types,NodeId,Cover,AddTime,UsID,Hidden";
            string sCondition = strWhere;
            string sOrder = "AddTime DESC,ID DESC";
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

                    return listDetails;
                }

                while (reader.Read())
                {
                    BCW.Model.Detail objDetail = new BCW.Model.Detail();
                    objDetail.ID = reader.GetInt32(0);
                    objDetail.IsAd = reader.GetBoolean(1);
                    objDetail.Title = reader.GetString(2);
                    objDetail.Types = reader.GetInt32(3);
                    objDetail.NodeId = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        objDetail.Cover = reader.GetString(5);
                    objDetail.AddTime = reader.GetDateTime(6);
                    objDetail.UsID = reader.GetInt32(7);
                    objDetail.Hidden = reader.GetInt32(8);
                    listDetails.Add(objDetail);
                }
            }

            return listDetails;
        }
        #endregion  成员方法
    }
}

