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
    /// 数据访问类tb_QuestionsAnswerCtr。
    /// </summary>
    public class tb_QuestionsAnswerCtr
    {
        public tb_QuestionsAnswerCtr()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_QuestionsAnswerCtr");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool ExistsID(int uid, int contrID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_QuestionsAnswerCtr");
            strSql.Append(" where uid=@uid and contrID=@contrID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4),
            new SqlParameter("@contrID", SqlDbType.Int,4)};
            parameters[0].Value = uid;
            parameters[1].Value = contrID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_QuestionsAnswerCtr model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_QuestionsAnswerCtr(");
            strSql.Append("uid,contrID,List,answerList,count,now,awardtype,awardId,explain,awardgold,trueCount,flaseCount,ishit,isDone,addtime,overtime)");
            strSql.Append(" values (");
            strSql.Append("@uid,@contrID,@List,@answerList,@count,@now,@awardtype,@awardId,@explain,@awardgold,@trueCount,@flaseCount,@ishit,@isDone,@addtime,@overtime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4),
                    new SqlParameter("@contrID", SqlDbType.Int,4),
                    new SqlParameter("@List", SqlDbType.NVarChar,3000),
                    new SqlParameter("@answerList", SqlDbType.NVarChar,3000),
                    new SqlParameter("@count", SqlDbType.Int,4),
                    new SqlParameter("@now", SqlDbType.Int,4),
                    new SqlParameter("@awardtype", SqlDbType.Int,4),
                    new SqlParameter("@awardId", SqlDbType.Int,4),
                    new SqlParameter("@explain", SqlDbType.NVarChar,200),
                    new SqlParameter("@awardgold", SqlDbType.Int,4),
                    new SqlParameter("@trueCount", SqlDbType.Int,4),
                    new SqlParameter("@flaseCount", SqlDbType.Int,4),
                    new SqlParameter("@ishit", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.Int,4),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@overtime", SqlDbType.DateTime)};
            parameters[0].Value = model.uid;
            parameters[1].Value = model.contrID;
            parameters[2].Value = model.List;
            parameters[3].Value = model.answerList;
            parameters[4].Value = model.count;
            parameters[5].Value = model.now;
            parameters[6].Value = model.awardtype;
            parameters[7].Value = model.awardId;
            parameters[8].Value = model.explain;
            parameters[9].Value = model.awardgold;
            parameters[10].Value = model.trueCount;
            parameters[11].Value = model.flaseCount;
            parameters[12].Value = model.ishit;
            parameters[13].Value = model.isDone;
            parameters[14].Value = model.addtime;
            parameters[15].Value = model.overtime;

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
        public void Update(BCW.Model.tb_QuestionsAnswerCtr model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuestionsAnswerCtr set ");
            strSql.Append("uid=@uid,");
            strSql.Append("contrID=@contrID,");
            strSql.Append("List=@List,");
            strSql.Append("answerList=@answerList,");
            strSql.Append("count=@count,");
            strSql.Append("now=@now,");
            strSql.Append("awardtype=@awardtype,");
            strSql.Append("awardId=@awardId,");
            strSql.Append("explain=@explain,");
            strSql.Append("awardgold=@awardgold,");
            strSql.Append("trueCount=@trueCount,");
            strSql.Append("flaseCount=@flaseCount,");
            strSql.Append("ishit=@ishit,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("overtime=@overtime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@uid", SqlDbType.Int,4),
                    new SqlParameter("@contrID", SqlDbType.Int,4),
                    new SqlParameter("@List", SqlDbType.NVarChar,3000),
                    new SqlParameter("@answerList", SqlDbType.NVarChar,3000),
                    new SqlParameter("@count", SqlDbType.Int,4),
                    new SqlParameter("@now", SqlDbType.Int,4),
                    new SqlParameter("@awardtype", SqlDbType.Int,4),
                    new SqlParameter("@awardId", SqlDbType.Int,4),
                    new SqlParameter("@explain", SqlDbType.NVarChar,200),
                    new SqlParameter("@awardgold", SqlDbType.Int,4),
                    new SqlParameter("@trueCount", SqlDbType.Int,4),
                    new SqlParameter("@flaseCount", SqlDbType.Int,4),
                    new SqlParameter("@ishit", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.Int,4),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@overtime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.uid;
            parameters[2].Value = model.contrID;
            parameters[3].Value = model.List;
            parameters[4].Value = model.answerList;
            parameters[5].Value = model.count;
            parameters[6].Value = model.now;
            parameters[7].Value = model.awardtype;
            parameters[8].Value = model.awardId;
            parameters[9].Value = model.explain;
            parameters[10].Value = model.awardgold;
            parameters[11].Value = model.trueCount;
            parameters[12].Value = model.flaseCount;
            parameters[13].Value = model.ishit;
            parameters[14].Value = model.isDone;
            parameters[15].Value = model.addtime;
            parameters[16].Value = model.overtime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_QuestionsAnswerCtr ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到每人答题总奖励
        /// </summary>
        public int GetAllAwardGold(int uid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(awardgold) from tb_QuestionsAnswerCtr ");
            strSql.Append(" where uid=uid and ishit=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4)};
            parameters[0].Value = uid;
            //object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return Convert.ToInt32(obj);
            //}
            int count;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    count = reader.GetInt32(0);
                    return count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 从uid  得到一个最新对象
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="contrID"></param>
        /// <returns></returns>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtrByUid(int uid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,uid,contrID,List,answerList,count,now,awardtype,awardId,explain,awardgold,trueCount,flaseCount,ishit,isDone,addtime,overtime from tb_QuestionsAnswerCtr ");
            strSql.Append(" where uid=@uid  and ishit!=5 order by ID desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4)};
            parameters[0].Value = uid;
            BCW.Model.tb_QuestionsAnswerCtr model = new BCW.Model.tb_QuestionsAnswerCtr();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.uid = reader.GetInt32(1);
                    model.contrID = reader.GetInt32(2);
                    model.List = reader.GetString(3);
                    model.answerList = reader.GetString(4);
                    model.count = reader.GetInt32(5);
                    model.now = reader.GetInt32(6);
                    model.awardtype = reader.GetInt32(7);
                    model.awardId = reader.GetInt32(8);
                    model.explain = reader.GetString(9);
                    model.awardgold = reader.GetInt32(10);
                    model.trueCount = reader.GetInt32(11);
                    model.flaseCount = reader.GetInt32(12);
                    model.ishit = reader.GetInt32(13);
                    model.isDone = reader.GetInt32(14);
                    model.addtime = reader.GetDateTime(15);
                    model.overtime = reader.GetDateTime(16);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// 从uid contrID 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtrByUidCid(int uid, int contrID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,uid,contrID,List,answerList,count,now,awardtype,awardId,explain,awardgold,trueCount,flaseCount,ishit,isDone,addtime,overtime from tb_QuestionsAnswerCtr ");
            strSql.Append(" where uid=@uid and contrID=@contrID order by ID desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4),
            new SqlParameter("@contrID", SqlDbType.Int,4)};
            parameters[0].Value = uid;
            parameters[1].Value = contrID;

            BCW.Model.tb_QuestionsAnswerCtr model = new BCW.Model.tb_QuestionsAnswerCtr();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.uid = reader.GetInt32(1);
                    model.contrID = reader.GetInt32(2);
                    model.List = reader.GetString(3);
                    model.answerList = reader.GetString(4);
                    model.count = reader.GetInt32(5);
                    model.now = reader.GetInt32(6);
                    model.awardtype = reader.GetInt32(7);
                    model.awardId = reader.GetInt32(8);
                    model.explain = reader.GetString(9);
                    model.awardgold = reader.GetInt32(10);
                    model.trueCount = reader.GetInt32(11);
                    model.flaseCount = reader.GetInt32(12);
                    model.ishit = reader.GetInt32(13);
                    model.isDone = reader.GetInt32(14);
                    model.addtime = reader.GetDateTime(15);
                    model.overtime = reader.GetDateTime(16);
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
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtr(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,uid,contrID,List,answerList,count,now,awardtype,awardId,explain,awardgold,trueCount,flaseCount,ishit,isDone,addtime,overtime from tb_QuestionsAnswerCtr ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.tb_QuestionsAnswerCtr model = new BCW.Model.tb_QuestionsAnswerCtr();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.uid = reader.GetInt32(1);
                    model.contrID = reader.GetInt32(2);
                    model.List = reader.GetString(3);
                    model.answerList = reader.GetString(4);
                    model.count = reader.GetInt32(5);
                    model.now = reader.GetInt32(6);
                    model.awardtype = reader.GetInt32(7);
                    model.awardId = reader.GetInt32(8);
                    model.explain = reader.GetString(9);
                    model.awardgold = reader.GetInt32(10);
                    model.trueCount = reader.GetInt32(11);
                    model.flaseCount = reader.GetInt32(12);
                    model.ishit = reader.GetInt32(13);
                    model.isDone = reader.GetInt32(14);
                    model.addtime = reader.GetDateTime(15);
                    model.overtime = reader.GetDateTime(16);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_QuestionsAnswerCtr ");
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
        /// <returns>IList tb_QuestionsAnswerCtr</returns>
        public IList<BCW.Model.tb_QuestionsAnswerCtr> Gettb_QuestionsAnswerCtrs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_QuestionsAnswerCtr> listtb_QuestionsAnswerCtrs = new List<BCW.Model.tb_QuestionsAnswerCtr>();
            string sTable = "tb_QuestionsAnswerCtr";
            string sPkey = "id";
            string sField = "ID,uid,contrID,List,answerList,count,now,awardtype,awardId,explain,awardgold,trueCount,flaseCount,ishit,isDone,addtime,overtime";
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
                    return listtb_QuestionsAnswerCtrs;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_QuestionsAnswerCtr objtb_QuestionsAnswerCtr = new BCW.Model.tb_QuestionsAnswerCtr();
                    objtb_QuestionsAnswerCtr.ID = reader.GetInt32(0);
                    objtb_QuestionsAnswerCtr.uid = reader.GetInt32(1);
                    objtb_QuestionsAnswerCtr.contrID = reader.GetInt32(2);
                    objtb_QuestionsAnswerCtr.List = reader.GetString(3);
                    objtb_QuestionsAnswerCtr.answerList = reader.GetString(4);
                    objtb_QuestionsAnswerCtr.count = reader.GetInt32(5);
                    objtb_QuestionsAnswerCtr.now = reader.GetInt32(6);
                    objtb_QuestionsAnswerCtr.awardtype = reader.GetInt32(7);
                    objtb_QuestionsAnswerCtr.awardId = reader.GetInt32(8);
                    objtb_QuestionsAnswerCtr.explain = reader.GetString(9);
                    objtb_QuestionsAnswerCtr.awardgold = reader.GetInt32(10);
                    objtb_QuestionsAnswerCtr.trueCount = reader.GetInt32(11);
                    objtb_QuestionsAnswerCtr.flaseCount = reader.GetInt32(12);
                    objtb_QuestionsAnswerCtr.ishit = reader.GetInt32(13);
                    objtb_QuestionsAnswerCtr.isDone = reader.GetInt32(14);
                    objtb_QuestionsAnswerCtr.addtime = reader.GetDateTime(15);
                    objtb_QuestionsAnswerCtr.overtime = reader.GetDateTime(16);
                    listtb_QuestionsAnswerCtrs.Add(objtb_QuestionsAnswerCtr);
                }
            }
            return listtb_QuestionsAnswerCtrs;
        }

        #endregion  成员方法
    }
}

