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
    /// 数据访问类Forum。
    /// </summary>
    public class Forum
    {
        public Forum()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Forum");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forum");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and IsActive=@IsActive ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@IsActive", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 论坛里是否存在论坛
        /// </summary>
        public bool ExistsNodeId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forum");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.Forum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forum(");
            strSql.Append("ID,NodeId,Title,Notes,Logo,Content,Label,Postlt,Replylt,Gradelt,Visitlt,ShowType,IsNode,IsActive,GroupId,VisitId,IsPc,Line,TopLine,TopTime,TopUbb,FootUbb,Paixu)");
            strSql.Append(" values (");
            strSql.Append("@ID,@NodeId,@Title,@Notes,@Logo,@Content,@Label,@Postlt,@Replylt,@Gradelt,@Visitlt,@ShowType,@IsNode,@IsActive,@GroupId,@VisitId,@IsNode,@Line,@TopLine,@TopTime,@TopUbb,@FootUbb,@Paixu)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@Logo", SqlDbType.NVarChar,200),
					new SqlParameter("@Content", SqlDbType.NVarChar,3000),
					new SqlParameter("@Label", SqlDbType.NVarChar,100),
					new SqlParameter("@Postlt", SqlDbType.Int,4),
					new SqlParameter("@Replylt", SqlDbType.Int,4),
					new SqlParameter("@Gradelt", SqlDbType.Int,4),
					new SqlParameter("@Visitlt", SqlDbType.Int,4),
					new SqlParameter("@ShowType", SqlDbType.Int,4),
					new SqlParameter("@IsNode", SqlDbType.Int,4),
					new SqlParameter("@IsActive", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@VisitId", SqlDbType.NText),
	                new SqlParameter("@IsPc", SqlDbType.Int,4),
	                new SqlParameter("@Line", SqlDbType.Int,4),
	                new SqlParameter("@TopLine", SqlDbType.Int,4),
	                new SqlParameter("@TopTime", SqlDbType.DateTime),
					new SqlParameter("@TopUbb", SqlDbType.NVarChar,800),
					new SqlParameter("@FootUbb", SqlDbType.NVarChar,800),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.Logo;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.Label;
            parameters[7].Value = model.Postlt;
            parameters[8].Value = model.Replylt;
            parameters[9].Value = model.Gradelt;
            parameters[10].Value = model.Visitlt;
            parameters[11].Value = model.ShowType;
            parameters[12].Value = model.IsNode;
            parameters[13].Value = model.IsActive;
            parameters[14].Value = model.GroupId;
            parameters[15].Value = model.VisitId;
            parameters[16].Value = model.IsPc;
            parameters[17].Value = 0;
            parameters[18].Value = 0;
            parameters[19].Value = DateTime.Now;
            parameters[20].Value = model.TopUbb;
            parameters[21].Value = model.FootUbb;
            parameters[22].Value = model.Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Forum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Title=@Title,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("Logo=@Logo,");
            strSql.Append("Label=@Label,");
            strSql.Append("Content=@Content,");
            strSql.Append("Postlt=@Postlt,");
            strSql.Append("Replylt=@Replylt,");
            strSql.Append("Gradelt=@Gradelt,");
            strSql.Append("Visitlt=@Visitlt,");
            strSql.Append("ShowType=@ShowType,");
            strSql.Append("IsNode=@IsNode,");
            strSql.Append("IsActive=@IsActive,");
            strSql.Append("GroupId=@GroupId,");
            strSql.Append("VisitId=@VisitId,");
            strSql.Append("IsPc=@IsPc,");
            strSql.Append("TopUbb=@TopUbb,");
            strSql.Append("FootUbb=@FootUbb,");
            strSql.Append("Paixu=@Paixu");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@Logo", SqlDbType.NVarChar,200),
					new SqlParameter("@Content", SqlDbType.NVarChar,3000),
					new SqlParameter("@Label", SqlDbType.NVarChar,100),
					new SqlParameter("@Postlt", SqlDbType.Int,4),
					new SqlParameter("@Replylt", SqlDbType.Int,4),
					new SqlParameter("@Gradelt", SqlDbType.Int,4),
					new SqlParameter("@Visitlt", SqlDbType.Int,4),
					new SqlParameter("@ShowType", SqlDbType.Int,4),
					new SqlParameter("@IsNode", SqlDbType.Int,4),
					new SqlParameter("@IsActive", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@VisitId", SqlDbType.NText),
	                new SqlParameter("@IsPc", SqlDbType.Int,4),
					new SqlParameter("@TopUbb", SqlDbType.NVarChar,800),
					new SqlParameter("@FootUbb", SqlDbType.NVarChar,800),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.Logo;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.Label;
            parameters[7].Value = model.Postlt;
            parameters[8].Value = model.Replylt;
            parameters[9].Value = model.Gradelt;
            parameters[10].Value = model.Visitlt;
            parameters[11].Value = model.ShowType;
            parameters[12].Value = model.IsNode;
            parameters[13].Value = model.IsActive;
            parameters[14].Value = model.GroupId;
            parameters[15].Value = model.VisitId;
            parameters[16].Value = model.IsPc;
            parameters[17].Value = model.TopUbb;
            parameters[18].Value = model.FootUbb;
            parameters[19].Value = model.Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新版规
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("Content=@Content");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,3000)};
            parameters[0].Value = ID;
            parameters[1].Value = Content;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新底链
        /// </summary>
        public void UpdateFootUbb(int ID, string FootUbb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("FootUbb=@FootUbb");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@FootUbb", SqlDbType.NVarChar,800)};
            parameters[0].Value = ID;
            parameters[1].Value = FootUbb;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新NodeId
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("NodeId=@NodeId");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新下级ID集合
        /// </summary>
        public void UpdateDoNode(int ID, string DoNode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("DoNode=@DoNode");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@DoNode", SqlDbType.NVarChar,200)};
            parameters[0].Value = ID;
            parameters[1].Value = DoNode;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新在线人数
        /// </summary>
        public void UpdateLine(int ID, int Line)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("Line=@Line,");
            strSql.Append("TopLine=");
            strSql.Append("case when TopLine<" + Line + " then " + Line + " else TopLine+0 END");
            strSql.Append(",TopTime=");
            strSql.Append("case when TopLine<" + Line + " then '" + DateTime.Now + "' else TopTime END");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Line", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Line;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 在线人数减1
        /// </summary>
        public void UpdateLine(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("Line=Line-1");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Line>@Line ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Line", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新基金数目
        /// </summary>
        public void UpdateiCent(int ID, long iCent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forum set ");
            strSql.Append("iCent=iCent+@iCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iCent", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = iCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forum GetForum(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,NodeId,DoNode,Title,Notes,Logo,Content,Label,Postlt,Replylt,Gradelt,Visitlt,ShowType,IsNode,IsActive,GroupId,VisitId,IsPc,Line,TopLine,TopUbb,FootUbb,Paixu from tb_Forum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Forum model = new BCW.Model.Forum();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.NodeId = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.DoNode = reader.GetString(2);
                    model.Title = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.Notes = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        model.Logo = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        model.Content = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        model.Label = reader.GetString(7);
                    model.Postlt = reader.GetInt32(8);
                    model.Replylt = reader.GetInt32(9);
                    model.Gradelt = reader.GetInt32(10);
                    model.Visitlt = reader.GetInt32(11);
                    model.ShowType = reader.GetInt32(12);
                    model.IsNode = reader.GetInt32(13);
                    model.IsActive = reader.GetInt32(14);
                    model.GroupId = reader.GetInt32(15);
                    if (!reader.IsDBNull(16))
                        model.VisitId = reader.GetString(16);
                    model.IsPc = reader.GetInt32(17);
                    model.Line = reader.GetInt32(18);
                    model.TopLine = reader.GetInt32(19);
                    if (!reader.IsDBNull(20))
                        model.TopUbb = reader.GetString(20);
                    if (!reader.IsDBNull(21))
                        model.FootUbb = reader.GetString(21);
                    model.Paixu = reader.GetInt32(22);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个名称、口号、在线
        /// </summary>
        public BCW.Model.Forum GetForumBasic(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title,Notes,Line,TopLine,TopTime,GroupId,iCent from tb_Forum ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and IsActive=@IsActive ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@IsActive", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;

            BCW.Model.Forum model = new BCW.Model.Forum();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Title = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        model.Notes = reader.GetString(1);
                    model.Line = reader.GetInt32(2);
                    model.TopLine = reader.GetInt32(3);
                    model.TopTime = reader.GetDateTime(4);
                    model.GroupId = reader.GetInt32(5);
                    model.iCent = reader.GetInt64(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Forum ");
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
        /// 得到版规公告Content
        /// </summary>
        public string GetContent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Content from tb_Forum ");
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
        /// 得到底链FootUbb
        /// </summary>
        public string GetFootUbb(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FootUbb from tb_Forum ");
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
        /// 得到节点NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NodeId from tb_Forum ");
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
        /// 得到GroupId
        /// </summary>
        public int GetGroupId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GroupId from tb_Forum ");
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
        /// 得到论坛基金
        /// </summary>
        public long GetiCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iCent from tb_Forum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// 得到Label
        /// </summary>
        public string GetLabel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Label from tb_Forum ");
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
        /// 得到DoNode下级ID
        /// </summary>
        public string GetDoNode(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DoNode from tb_Forum ");
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
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Forum ");
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
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Forum</returns>
        public IList<BCW.Model.Forum> GetForums(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forum> listForums = new List<BCW.Model.Forum>();
            string sTable = "tb_Forum";
            string sPkey = "id";
            string sField = "ID,Title,IsActive,GroupId,Paixu,Line,TopLine";
            string sCondition = strWhere;
            string sOrder = "Paixu Asc";
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
                    return listForums;
                }
                while (reader.Read())
                {
                    BCW.Model.Forum objForum = new BCW.Model.Forum();
                    objForum.ID = reader.GetInt32(0);
                    objForum.Title = reader.GetString(1);
                    objForum.IsActive = reader.GetInt32(2);
                    objForum.GroupId = reader.GetInt32(3);
                    objForum.Paixu = reader.GetInt32(4);
                    objForum.Line = reader.GetInt32(5);
                    objForum.TopLine = reader.GetInt32(6);
                    listForums.Add(objForum);
                }
            }
            return listForums;
        }

        #endregion  成员方法
    }
}
