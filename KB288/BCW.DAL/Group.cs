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
    /// 数据访问类Group。
    /// </summary>
    public class Group
    {
        public Group()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Group");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Group");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Group");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到某城市的圈子数
        /// </summary>
        public int GetGroupNum(string City)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_Group");
            strSql.Append(" where City='" + City + "'");

            object obj = SqlHelperUser.GetSingle(strSql.ToString());
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
        /// 得到某分类的圈子数
        /// </summary>
        public int GetGroupNum(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_Group");
            strSql.Append(" where Types=" + Types + "");

            object obj = SqlHelperUser.GetSingle(strSql.ToString());
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
        public int Add(BCW.Model.Group model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Group(");
            strSql.Append("Types,Title,City,Logo,Notes,Content,UsID,iCent,iTotal,iClick,InType,ForumId,ChatId,ForumStatus,ChatStatus,SignCent,Status,ExTime,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@City,@Logo,@Notes,@Content,@UsID,@iCent,@iTotal,@iClick,@InType,@ForumId,@ChatId,@ForumStatus,@ChatStatus,@SignCent,@Status,@ExTime,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@Logo", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,800),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@iCent", SqlDbType.BigInt,8),
					new SqlParameter("@iTotal", SqlDbType.Int,4),
					new SqlParameter("@iClick", SqlDbType.Int,4),
					new SqlParameter("@InType", SqlDbType.TinyInt,1),
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@ForumStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@ChatStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@SignCent", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@ExTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.City;
            parameters[3].Value = model.Logo;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.UsID;
            parameters[7].Value = 0;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = model.InType;
            parameters[11].Value = 0;
            parameters[12].Value = 0;
            parameters[13].Value = 0;
            parameters[14].Value = 0;
            parameters[15].Value = 0;
            parameters[16].Value = model.Status;
            parameters[17].Value = model.ExTime;
            parameters[18].Value = model.AddTime;

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
        public void Update(BCW.Model.Group model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("City=@City,");
            strSql.Append("Logo=@Logo,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("Content=@Content,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("iCent=@iCent,");
            strSql.Append("InType=@InType,");
            strSql.Append("ForumId=@ForumId,");
            strSql.Append("ChatId=@ChatId,");
            strSql.Append("ForumStatus=@ForumStatus,");
            strSql.Append("ChatStatus=@ChatStatus,");
            strSql.Append("SignCent=@SignCent,");
            strSql.Append("Status=@Status,");
            strSql.Append("ExTime=@ExTime,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@Logo", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,800),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@iCent", SqlDbType.BigInt,8),
					new SqlParameter("@InType", SqlDbType.TinyInt,1),
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@ForumStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@ChatStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@SignCent", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@ExTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.City;
            parameters[4].Value = model.Logo;
            parameters[5].Value = model.Notes;
            parameters[6].Value = model.Content;
            parameters[7].Value = model.UsID;
            parameters[8].Value = model.iCent;
            parameters[9].Value = model.InType;
            parameters[10].Value = model.ForumId;
            parameters[11].Value = model.ChatId;
            parameters[12].Value = model.ForumStatus;
            parameters[13].Value = model.ChatStatus;
            parameters[14].Value = model.SignCent;
            parameters[15].Value = model.Status;
            parameters[16].Value = model.ExTime;
            parameters[17].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Group model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("Logo=@Logo,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("InType=@InType,");
            strSql.Append("ForumStatus=@ForumStatus,");
            strSql.Append("ChatStatus=@ChatStatus,");
            strSql.Append("SignCent=@SignCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Logo", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@InType", SqlDbType.TinyInt,1),
					new SqlParameter("@ForumStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@ChatStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@SignCent", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Logo;
            parameters[2].Value = model.Notes;
            parameters[3].Value = model.InType;
            parameters[4].Value = model.ForumStatus;
            parameters[5].Value = model.ChatStatus;
            parameters[6].Value = model.SignCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新过期时间(续费)
        /// </summary>
        public void UpdateExitTime(int ID, DateTime ExTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("ExTime=@ExTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ExTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新基金数目
        /// </summary>
        public void UpdateiCent(int ID, long iCent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
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
        /// 更新基金密码
        /// </summary>
        public void UpdateiCentPwd(int ID, string iCentPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("iCentPwd=@iCentPwd");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iCentPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = iCentPwd;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新成员人数
        /// </summary>
        public void UpdateiTotal(int ID, int iTotal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("iTotal=iTotal+@iTotal");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iTotal", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = iTotal;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新今天来访ID
        /// </summary>
        public void UpdateVisitId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("VisitId=@VisitId");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@VisitId", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = "";

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新今天来访ID
        /// </summary>
        public void UpdateVisitId(int ID, string VisitId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("iClick=iClick+1,");
            strSql.Append("VisitId=@VisitId,");
            strSql.Append("VisitTime=@VisitTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@VisitId", SqlDbType.NText),
					new SqlParameter("@VisitTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = VisitId;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新签到ID
        /// </summary>
        public void UpdateSignID(int ID, string SignID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("SignID=@SignID,");
            strSql.Append("SignTime=@SignTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SignID", SqlDbType.NText),
					new SqlParameter("@SignTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = SignID;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 审核圈子
        /// </summary>
        public void UpdateStatus(int ID, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Group set ");
            strSql.Append("Status=@Status");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = Status;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Group ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Group GetGroup(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,Title,City,Logo,Notes,Content,UsID,iCent,iTotal,VisitId,VisitTime,iClick,InType,ForumId,ChatId,ForumStatus,ChatStatus,SignID,SignTime,SignCent,iCentPwd,Status,ExTime,AddTime from tb_Group ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Group model = new BCW.Model.Group();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.City = reader.GetString(3);
                    model.Logo = reader.GetString(4);
                    model.Notes = reader.GetString(5);
                    model.Content = reader.GetString(6);
                    model.UsID = reader.GetInt32(7);
                    model.iCent = reader.GetInt64(8);
                    model.iTotal = reader.GetInt32(9);
                    if (!reader.IsDBNull(10))
                        model.VisitId = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        model.VisitTime = reader.GetDateTime(11);
                    model.iClick = reader.GetInt32(12);
                    model.InType = reader.GetByte(13);
                    model.ForumId = reader.GetInt32(14);
                    model.ChatId = reader.GetInt32(15);
                    model.ForumStatus = reader.GetByte(16);
                    model.ChatStatus = reader.GetByte(17);
                    if (!reader.IsDBNull(18))
                        model.SignID = reader.GetString(18);
                    if (!reader.IsDBNull(19))
                        model.SignTime = reader.GetDateTime(19);
                    model.SignCent = reader.GetInt32(20);
                    if (!reader.IsDBNull(21))
                        model.iCentPwd = reader.GetString(21);
                    model.Status = reader.GetByte(22);
                    model.ExTime = reader.GetDateTime(23);
                    model.AddTime = reader.GetDateTime(24);
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
        public BCW.Model.Group GetGroupMe(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title,ForumId,ChatId,ForumStatus,ChatStatus,ExTime from tb_Group ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Group model = new BCW.Model.Group();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Title = reader.GetString(0);
                    model.ForumId = reader.GetInt32(1);
                    model.ChatId = reader.GetInt32(2);
                    model.ForumStatus = reader.GetByte(3);
                    model.ChatStatus = reader.GetByte(4);
                    model.ExTime = reader.GetDateTime(5);
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
            strSql.Append("select  top 1 Title from tb_Group ");
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
        /// 得到UsID
        /// </summary>
        public int GetUsID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsID from tb_Group ");
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
        /// 得到ID
        /// </summary>
        public int GetID(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID from tb_Group ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 得到ForumId
        /// </summary>
        public int GetForumId(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ForumId from tb_Group ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// 得到SignID
        /// </summary>
        public string GetSignID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 SignID from tb_Group ");
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
            strSql.Append(" FROM tb_Group ");
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Group</returns>
        public IList<BCW.Model.Group> GetGroups(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Group> listGroups = new List<BCW.Model.Group>();
            string sTable = "tb_Group";
            string sPkey = "id";
            string sField = "ID,Types,Title,iCent,iTotal,iClick";
            string sCondition = strWhere;
            string sOrder = strOrder;
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
                    return listGroups;
                }
                while (reader.Read())
                {
                    BCW.Model.Group objGroup = new BCW.Model.Group();
                    objGroup.ID = reader.GetInt32(0);
                    objGroup.Types = reader.GetInt32(1);
                    objGroup.Title = reader.GetString(2);
                    objGroup.iCent = reader.GetInt64(3);
                    objGroup.iTotal = reader.GetInt32(4);
                    objGroup.iClick = reader.GetInt32(5);
                    listGroups.Add(objGroup);
                }
            }
            return listGroups;
        }

        #endregion  成员方法
    }
}
