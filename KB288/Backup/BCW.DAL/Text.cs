using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{ 
    /**
     * 
     * 
     * 
     * 修改人 陈志基  2016/4/9
      **/
    /// <summary>
    /// 数据访问类Text。
    /// </summary>
    public class Text
    {
        public Text()
        { }
        #region  成员方法


        /// <summary>
        /// 更新新闻已采集的标识
        /// </summary>
        public void UpdateIstxt(int ID, int Istxt)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" Istxt=@Istxt ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Istxt", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Istxt;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        //------------------------高手论坛使用-------------------------



        /// <summary>
        /// 更新总中奖数和连中、月中
        /// </summary>
        public void UpdategGsNum(int ID, int gwinnum, int glznum, int gmnum)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" gwinnum=gwinnum+@gwinnum, ");
            strSql.Append(" glznum=glznum+@glznum, ");
            strSql.Append(" gmnum=@gmnum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@gwinnum", SqlDbType.Int,4),
				new SqlParameter("@glznum", SqlDbType.Int,4),
				new SqlParameter("@gmnum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = gwinnum;
            parameters[2].Value = glznum;
            parameters[3].Value = gmnum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到连中的次数
        /// </summary>
        public int Getglznum(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select glznum from tb_Text ");
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
        /// 得到点赞数
        /// </summary>
        public int GetPraise(int ID )
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Praise from tb_Text ");
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
        ///// <summary>
        ///// 得到点赞人ID
        ///// </summary>
        //public int GetPraiseID(int ID)
        //{

        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select PraiseID from tb_Text ");
        //    strSql.Append(" where ID=@ID ");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4)};
        //    parameters[0].Value = ID;

        //    using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
        //    {
        //        if (reader.HasRows)
        //        {
        //            reader.Read();
        //            return reader.GetInt32(0);
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}
        /// <summary>
        /// 更新新一期的期数
        /// </summary>
        public void UpdateGqinum(int ID, int Gqinum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("Gqinum=@Gqinum, ");
            strSql.Append("Gaddnum=Gaddnum+1 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Gqinum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Gqinum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        //------------------------高手论坛使用-------------------------

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Text");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Text");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录(回收站)
        /// </summary>
        public bool Exists(int ID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Text");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and IsDel=@IsDel ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;
            parameters[2].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Text");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and IsDel=@IsDel ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;
            parameters[2].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int ForumID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Text");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and IsDel=@IsDel ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@IsDel", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;
            parameters[2].Value = UsID;
            parameters[3].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 会员在某高手论坛是否存在发帖，返回帖子ID
        /// </summary>
        public int GetRaceBID(int ForumID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_Text");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Gaddnum<>0 and IsDel=0");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = UsID;

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
        public int Add(BCW.Model.Text model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Text(");
            strSql.Append("ForumId,Types,LabelId,Title,Content,HideContent,UsID,UsName,ReplyNum,ReadNum,IsGood,IsLock,IsTop,Prices,Price,Price2,Pricel,BzType,HideType,PayCi,IsSeen,IsOver,IsDel,FileNum,TsID,IsFlow,AddTime,ReTime,Gaddnum,Gqinum)");
            strSql.Append(" values (");
            strSql.Append("@ForumId,@Types,@LabelId,@Title,@Content,@HideContent,@UsID,@UsName,@ReplyNum,@ReadNum,@IsGood,@IsLock,@IsTop,@Prices,@Price,@Price2,@Pricel,@BzType,@HideType,@PayCi,@IsSeen,@IsOver,@IsDel,@FileNum,@TsID,@IsFlow,@AddTime,@ReTime,@Gaddnum,@Gqinum)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.TinyInt,1),
					new SqlParameter("@LabelId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@HideContent", SqlDbType.NText),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReplyNum", SqlDbType.Int,4),
					new SqlParameter("@ReadNum", SqlDbType.Int,4),
					new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
					new SqlParameter("@IsLock", SqlDbType.TinyInt,1),
					new SqlParameter("@IsTop", SqlDbType.Int,4),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Price2", SqlDbType.Int,4),
                    new SqlParameter("@Pricel", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@HideType", SqlDbType.TinyInt,1),
					new SqlParameter("@PayCi", SqlDbType.NVarChar,50),
					new SqlParameter("@IsSeen", SqlDbType.TinyInt,1),
					new SqlParameter("@IsOver", SqlDbType.TinyInt,1),
					new SqlParameter("@IsDel", SqlDbType.TinyInt,1),
					new SqlParameter("@FileNum", SqlDbType.Int,4),
					new SqlParameter("@TsID", SqlDbType.Int,4),
					new SqlParameter("@IsFlow", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ReTime", SqlDbType.DateTime),
					new SqlParameter("@Gaddnum", SqlDbType.Int,4),      
					new SqlParameter("@Gqinum", SqlDbType.Int,4)};
        
            parameters[0].Value = model.ForumId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.LabelId;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.HideContent;
            parameters[6].Value = model.UsID;
            parameters[7].Value = model.UsName;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = 0;
            parameters[12].Value = 0;
            parameters[13].Value = model.Prices;
            parameters[14].Value = model.Price;
            parameters[15].Value = model.Price2;
            parameters[16].Value = 0;
            parameters[17].Value = model.BzType;
            parameters[18].Value = model.HideType;
            parameters[19].Value = model.PayCi;
            parameters[20].Value = model.IsSeen;
            parameters[21].Value = 0;
            parameters[22].Value = 0;
            parameters[23].Value = 0;
            parameters[24].Value = 0;
            parameters[25].Value = 0;
            parameters[26].Value = model.AddTime;
            parameters[27].Value = model.ReTime;
            parameters[28].Value = model.Gaddnum;
            parameters[29].Value = model.Gqinum;
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
        /// 增加一条派币帖
        /// </summary>
        public int AddPricesLimit(BCW.Model.Text model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Text(");
            strSql.Append("ForumId,Types,LabelId,Title,Content,HideContent,UsID,UsName,ReplyNum,ReadNum,IsGood,IsLock,IsTop,Prices,Price,Price2,Pricel,BzType,HideType,PayCi,IsSeen,IsOver,IsDel,FileNum,TsID,IsFlow,AddTime,ReTime,Gaddnum,Gqinum,PricesLimit)");
            strSql.Append(" values (");
            strSql.Append("@ForumId,@Types,@LabelId,@Title,@Content,@HideContent,@UsID,@UsName,@ReplyNum,@ReadNum,@IsGood,@IsLock,@IsTop,@Prices,@Price,@Price2,@Pricel,@BzType,@HideType,@PayCi,@IsSeen,@IsOver,@IsDel,@FileNum,@TsID,@IsFlow,@AddTime,@ReTime,@Gaddnum,@Gqinum,@PricesLimit)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.TinyInt,1),
					new SqlParameter("@LabelId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@HideContent", SqlDbType.NText),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReplyNum", SqlDbType.Int,4),
					new SqlParameter("@ReadNum", SqlDbType.Int,4),
					new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
					new SqlParameter("@IsLock", SqlDbType.TinyInt,1),
					new SqlParameter("@IsTop", SqlDbType.Int,4),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Price2", SqlDbType.Int,4),
                    new SqlParameter("@Pricel", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@HideType", SqlDbType.TinyInt,1),
					new SqlParameter("@PayCi", SqlDbType.NVarChar,50),
					new SqlParameter("@IsSeen", SqlDbType.TinyInt,1),
					new SqlParameter("@IsOver", SqlDbType.TinyInt,1),
					new SqlParameter("@IsDel", SqlDbType.TinyInt,1),
					new SqlParameter("@FileNum", SqlDbType.Int,4),
					new SqlParameter("@TsID", SqlDbType.Int,4),
					new SqlParameter("@IsFlow", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ReTime", SqlDbType.DateTime),
					new SqlParameter("@Gaddnum", SqlDbType.Int,4),      
					new SqlParameter("@Gqinum", SqlDbType.Int,4),
                    new SqlParameter("@PricesLimit", SqlDbType.NVarChar,50)};

            parameters[0].Value = model.ForumId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.LabelId;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.HideContent;
            parameters[6].Value = model.UsID;
            parameters[7].Value = model.UsName;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = 0;
            parameters[12].Value = 0;
            parameters[13].Value = model.Prices;
            parameters[14].Value = model.Price;
            parameters[15].Value = model.Price2;
            parameters[16].Value = 0;
            parameters[17].Value = model.BzType;
            parameters[18].Value = model.HideType;
            parameters[19].Value = model.PayCi;
            parameters[20].Value = model.IsSeen;
            parameters[21].Value = 0;
            parameters[22].Value = 0;
            parameters[23].Value = 0;
            parameters[24].Value = 0;
            parameters[25].Value = 0;
            parameters[26].Value = model.AddTime;
            parameters[27].Value = model.ReTime;
            parameters[28].Value = model.Gaddnum;
            parameters[29].Value = model.Gqinum;
            parameters[30].Value = model.PricesLimit;
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
        public void Update(BCW.Model.Text model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("ReTime=@ReTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@ReTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Content;
            parameters[3].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Text model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("ForumId=@ForumId,");
            strSql.Append("LabelId=@LabelId,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ReadNum=@ReadNum,");
            strSql.Append("IsGood=@IsGood,");
            strSql.Append("IsRecom=@IsRecom,");
            strSql.Append("IsLock=@IsLock,");
            strSql.Append("IsTop=@IsTop,");
            strSql.Append("IsOver=@IsOver,");
            strSql.Append("IsDel=@IsDel,");
            strSql.Append("ReTime=@ReTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@LabelId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReadNum", SqlDbType.Int,4),
					new SqlParameter("@IsGood", SqlDbType.TinyInt,1),
					new SqlParameter("@IsRecom", SqlDbType.TinyInt,1),
					new SqlParameter("@IsLock", SqlDbType.TinyInt,1),
					new SqlParameter("@IsTop", SqlDbType.Int,4),
					new SqlParameter("@IsOver", SqlDbType.TinyInt,1),
					new SqlParameter("@IsDel", SqlDbType.TinyInt,1),
					new SqlParameter("@ReTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ForumId;
            parameters[2].Value = model.LabelId;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.ReadNum;
            parameters[8].Value = model.IsGood;
            parameters[9].Value = model.IsRecom;
            parameters[10].Value = model.IsLock;
            parameters[11].Value = model.IsTop;
            parameters[12].Value = model.IsOver;
            parameters[13].Value = model.IsDel;
            parameters[14].Value = model.ReTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新帖子类型
        /// </summary>
        public void UpdateTypes(int ID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("Types=@Types ");
            strSql.Append(" where ID=@ID and (Types=0 or Types=5)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Types;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新点赞数
        /// </summary>
        public void UpdatePraise(int ID, int Praise)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("Praise=@Praise ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Praise", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Praise;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        ///// <summary>
        ///// 更新点赞人ID
        ///// </summary>
        //public void UpdatePraiseID(int ID, int PraiseID)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update tb_Text set ");
        //    strSql.Append("PraiseID=@PraiseID ");
        //    strSql.Append(" where ID=@ID");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4),
        //            new SqlParameter("@PraiseID", SqlDbType.Int,4)};
        //    parameters[0].Value = ID;
        //    parameters[1].Value = PraiseID;

        //    SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        //}
        /// <summary>
        /// 更新贴子内容
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("Content=@Content, ");
            strSql.Append("ReTime=@ReTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@ReTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = Content;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("ReStats=@ReStats, ");
            strSql.Append("ReList=@ReList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReStats", SqlDbType.NVarChar,50),
            		new SqlParameter("@ReList", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = ReStats;
            parameters[2].Value = ReList;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新购买ID
        /// </summary>
        public void UpdatePayID(int ID, string PayID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("PayID=@PayID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@PayID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = PayID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新回复ID
        /// </summary>
        public void UpdateReplyID(int ID, string ReplyID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("ReplyID=@ReplyID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@ReplyID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = ReplyID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新已派币ID
        /// </summary>
        public void UpdateIsPriceID(int ID, string IsPriceID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("IsPriceID=@IsPriceID");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@IsPriceID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = IsPriceID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新点赞ID
        /// </summary>
        public void UpdatePraiseID(int ID, string PraiseID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("PraiseID=@PraiseID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@PraiseID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = PraiseID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新已派多少币
        /// </summary>
        public void UpdatePricel(int ID, long Price)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("Pricel=Pricel+@Pricel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Pricel", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = Price;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新回复数
        /// </summary>
        public void UpdateReplyNum(int ID, int ReplyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("ReplyNum=ReplyNum+@ReplyNum, ");
            //strSql.Append("ReTime=@ReTime ");
            strSql.Append("ReTime = case when AddTime>'" + DateTime.Now.AddMonths(-6) + "' then '" + DateTime.Now + "' else ReTime END");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@ReplyNum", SqlDbType.Int,4),
            		new SqlParameter("@ReTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ReplyNum;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新阅读数
        /// </summary>
        public void UpdateReadNum(int ID, int ReadNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("ReadNum=ReadNum+@ReadNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@ReadNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ReadNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新专题标识
        /// </summary>
        public void UpdateTsID(int ID, int TsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("TsID=@TsID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@TsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = TsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 批量更新专题标识
        /// </summary>
        public void UpdateTsID2(int TsID, int NewTsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("TsID=@NewTsID ");
            strSql.Append(" where TsID=@TsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@TsID", SqlDbType.Int,4),
            		new SqlParameter("@NewTsID", SqlDbType.Int,4)};
            parameters[0].Value = TsID;
            parameters[1].Value = NewTsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 批量转移帖子
        /// </summary>
        public void UpdateForumID2(int ForumID, int NewForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("ForumID=@NewForumID ");
            strSql.Append(" where ForumID=@ForumID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
            		new SqlParameter("@NewForumID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = NewForumID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新帖子文件数
        /// </summary>
        public void UpdateFileNum(int ID, int FileNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("FileNum=FileNum+@FileNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@FileNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = FileNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        //-------------------------------------版主管理-------------------------------------------
        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel(int ID, int IsDel)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsDel=@IsDel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsDel;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 加精/解精
        /// </summary>
        public void UpdateIsGood(int ID, int IsGood)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsGood=@IsGood ");
            //如果加精则提升帖子（改变回复时间）
            if (IsGood == 1)
                strSql.Append(",ReTime='" + DateTime.Now + "' ");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsGood", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsGood;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 推荐/解推荐
        /// </summary>
        public void UpdateIsRecom(int ID, int IsRecom)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsRecom=@IsRecom ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsRecom", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsRecom;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 置顶/去顶/固底/去底
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsTop=@IsTop ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsTop", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = IsTop;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 锁定/解锁
        /// </summary>
        public void UpdateIsLock(int ID, int IsLock)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsLock=@IsLock ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsLock", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsLock;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 结束帖子
        /// </summary>
        public void UpdateIsOver(int ID, int IsOver)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsOver=@IsOver ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsOver", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsOver;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 设置滚动
        /// </summary>
        public void UpdateIsFlow(int ID, int IsFlow)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" IsFlow=@IsFlow ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsFlow", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsFlow;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 转移
        /// </summary>
        public void UpdateForumID(int ID, int ForumID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set");
            strSql.Append(" ForumID=@ForumID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到置项类型
        /// </summary>
        public int GetIsTop(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsTop from tb_Text ");
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
        /// 得到锁定类型
        /// </summary>
        public int GetIsLock(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsLock from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到精华类型
        /// </summary>
        public int GetIsGood(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsGood from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到结帖类型
        /// </summary>
        public int GetIsOver(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsOver from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 更行点赞时间
        /// 陈志基  2016 3/28
        /// </summary>
        public void UpdatePraiseTime(int ID, DateTime PraiseTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("PraiseTime=@PraiseTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@PraiseTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = PraiseTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到帖子附件数
        /// </summary>
        public int GetFileNum(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FileNum from tb_Text ");
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

        //-------------------------------------版主管理-------------------------------------------


        /// <summary>
        /// 更新滚动时间
        /// </summary>
        public void UpdateFlowTime(int ID, DateTime FlowTime, int IsFlow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Text set ");
            strSql.Append("FlowTime=@FlowTime, ");
            strSql.Append("IsFlow=@IsFlow ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@FlowTime", SqlDbType.DateTime),
            		new SqlParameter("@IsFlow", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = FlowTime;
            parameters[2].Value = IsFlow;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Text ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// 删除回收站数据
        /// </summary>
        public void Delete()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Text ");
            strSql.Append(" where IsDel=@IsDel ");
            SqlParameter[] parameters = {
					new SqlParameter("@IsDel", SqlDbType.Int,4)};
            parameters[0].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到帖子标题
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Text ");
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
        /// 得到帖子Types
        /// </summary>
        public int GetTypes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Types from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到IsFlow
        /// </summary>
        public int GetIsFlow(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsFlow from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到帖主用户ID
        /// </summary>
        public int GetUsID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsID from tb_Text ");
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
        /// 得到回复ID
        /// </summary>
        public string GetReplyID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ReplyID from tb_Text ");
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
        /// 得到点赞ID
        /// </summary>
        public string GetPraiseID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PraiseID from tb_Text ");
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
        /// 得到派币帖实体
        /// </summary>
        public BCW.Model.Text GetPriceModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ReplyID,Prices,Price,Price2,Pricel,BzType,PayCi,UsID from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Text model = new BCW.Model.Text();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        model.ReplyID = reader.GetString(0);
                    model.Prices = reader.GetInt64(1);
                    model.Price = reader.GetInt32(2);
                    model.Price2 = reader.GetInt32(3);
                    model.Pricel = reader.GetInt64(4);
                    model.BzType = reader.GetByte(5);
                    model.PayCi = reader.GetString(6);
                    model.UsID = reader.GetInt32(7);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 随机得到一张社区滚动帖
        /// </summary>
        public BCW.Model.Text GetTextFlow()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,forumid from tb_Text ");
            strSql.Append(" where IsFlow=2 and FlowTime>'" + DateTime.Now + "' Order by NEWID()");

            BCW.Model.Text model = new BCW.Model.Text();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.ForumId = reader.GetInt32(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 随机得到某论坛一张滚动帖
        /// </summary>
        public BCW.Model.Text GetTextFlow(int ForumId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title from tb_Text ");
            strSql.Append(" where ForumId=@ForumId and IsFlow=1 Order by NEWID()");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumId", SqlDbType.Int,4)};
            parameters[0].Value = ForumId;

            BCW.Model.Text model = new BCW.Model.Text();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 随机得到N天内的一张推荐或精华帖
        /// </summary>
        public BCW.Model.Text GetTextGoodReCom(int Day)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ForumId,Title,IsGood,IsRecom from tb_Text ");
            strSql.Append(" where AddTime>'" + DateTime.Now.AddDays(-Day) + "' and (IsGood=1 OR IsRecom=1) Order by NEWID()");

            BCW.Model.Text model = new BCW.Model.Text();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.ForumId = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.IsGood = reader.GetByte(3);
                    model.IsRecom = reader.GetByte(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个实体
        /// </summary>
        public BCW.Model.Text GetTextMe(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsID,UsName,Title,Content,IsGood,IsRecom,IsLock,IsTop from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Text model = new BCW.Model.Text();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsID = reader.GetInt32(0);
                    model.UsName = reader.GetString(1);
                    model.Title = reader.GetString(2);
                    model.Content = reader.GetString(3);
                    model.IsGood = reader.GetByte(4);
                    model.IsRecom = reader.GetByte(5);
                    model.IsLock = reader.GetByte(6);
                    model.IsTop = reader.GetInt32(7);
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
        public BCW.Model.Text GetText(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ForumId,Types,LabelId,Title,Content,HideContent,UsID,UsName,ReplyNum,ReplyID,ReadNum,IsGood,IsRecom,IsLock,IsTop,Prices,Price,Price2,Pricel,BzType,HideType,PayID,PayCi,IsSeen,IsOver,IsDel,ReStats,ReList,FileNum,TsID,IsFlow,AddTime,ReTime,Gaddnum,Gwinnum,Glznum,Gmnum,Gqinum,Istxt,Praise,PraiseID,PricesLimit,IsPriceID from tb_Text ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Text model = new BCW.Model.Text();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.ForumId = reader.GetInt32(1);
                    model.Types = reader.GetByte(2);
                    model.LabelId = reader.GetInt32(3);
                    model.Title = reader.GetString(4);
                    model.Content = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        model.HideContent = reader.GetString(6);
                    model.UsID = reader.GetInt32(7);
                    model.UsName = reader.GetString(8);
                    model.ReplyNum = reader.GetInt32(9);
                    if (!reader.IsDBNull(10))
                        model.ReplyID = reader.GetString(10);
                    model.ReadNum = reader.GetInt32(11);
                    model.IsGood = reader.GetByte(12);
                    model.IsRecom = reader.GetByte(13);
                    model.IsLock = reader.GetByte(14);
                    model.IsTop = reader.GetInt32(15);
                    model.Prices = reader.GetInt64(16);
                    model.Price = reader.GetInt32(17);
                    model.Price2 = reader.GetInt32(18);
                    model.Pricel = reader.GetInt64(19);
                    model.BzType = reader.GetByte(20);

                    model.HideType = reader.GetByte(21);
                    if (!reader.IsDBNull(22))
                        model.PayID = reader.GetString(22);
                    model.PayCi = reader.GetString(23);
                    model.IsSeen = reader.GetByte(24);
                    model.IsOver = reader.GetByte(25);
                    model.IsDel = reader.GetByte(26);
                    if (!reader.IsDBNull(27))
                        model.ReStats = reader.GetString(27);
                    if (!reader.IsDBNull(28))
                        model.ReList = reader.GetString(28);
                    model.FileNum = reader.GetInt32(29);
                    model.TsID = reader.GetInt32(30);
                    model.IsFlow = reader.GetByte(31);
                    model.AddTime = reader.GetDateTime(32);
                    model.ReTime = reader.GetDateTime(33);
                    model.Gaddnum = reader.GetInt32(34);
                    model.Gwinnum = reader.GetInt32(35);
                    model.Glznum = reader.GetInt32(36);
                    model.Gmnum = reader.GetInt32(37);
                    model.Gqinum = reader.GetInt32(38);
                    if (!reader.IsDBNull(39))
                        model.Istxt = reader.GetInt32(39);
                    else
                        model.Istxt = 0;
                    model.Praise = reader.GetInt32(40);
                    if (!reader.IsDBNull(41))
                        model.PraiseID = reader.GetString(41);
                    if (!reader.IsDBNull(42))
                    model.PricesLimit = reader.GetString(42);
                    if (!reader.IsDBNull(43)) 
                        model.IsPriceID = reader.GetString(43);
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
            strSql.Append(" FROM tb_Text ");
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
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTexts(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Text> listTexts = new List<BCW.Model.Text>();
            string sTable = "tb_Text";
            string sPkey = "id";
            string sField = "ID,ForumId,Types,LabelId,Title,UsID,UsName,ReplyNum,ReadNum,IsGood,IsRecom,IsLock,IsTop,IsOver,AddTime,Gaddnum,Gwinnum";
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
                    return listTexts;
                }
                while (reader.Read())
                {
                    BCW.Model.Text objText = new BCW.Model.Text();
                    objText.ID = reader.GetInt32(0);
                    objText.ForumId = reader.GetInt32(1);
                    objText.Types = reader.GetByte(2);
                    objText.LabelId = reader.GetInt32(3);
                    objText.Title = Out.TitleUBB(reader.GetString(4));
                    objText.UsID = reader.GetInt32(5);
                    objText.UsName = reader.GetString(6);
                    objText.ReplyNum = reader.GetInt32(7);
                    objText.ReadNum = reader.GetInt32(8);
                    objText.IsGood = reader.GetByte(9);
                    objText.IsRecom = reader.GetByte(10);
                    objText.IsLock = reader.GetByte(11);
                    objText.IsTop = reader.GetInt32(12);
                    objText.IsOver = reader.GetByte(13);
                    objText.AddTime = reader.GetDateTime(14);
                    objText.Gaddnum = reader.GetInt32(15);
                    objText.Gwinnum = reader.GetInt32(16);
                    listTexts.Add(objText);
                }
            }
            return listTexts;
        }
        /// <summary>
        /// 帖子排行分页记录 陈志基 2016/08/10
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Text> GetForumstats1(int p_pageIndex, int p_pageSize, string strWhere, int showtype, out int p_recordCount)
        {
            IList<BCW.Model.Text> listForumstat = new List<BCW.Model.Text>();
            string strWhe = string.Empty;
            if (strWhere != "" || showtype > 1)
                strWhe += " where ";

            if (strWhere != "")
                strWhe += strWhere;

            if (strWhere != "" && showtype > 1)
                strWhe += " and ";

            if (showtype == 2)  //本周
            {
                #region 本周
                string M_Str_mindate = string.Empty;
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                strWhe += " AddTime>='" + M_Str_mindate + "'";
                #endregion
            }
            else if (showtype == 3) //本月
            {
                #region 本月
                strWhe += " Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "";
                #endregion
            }
            else if (showtype == 4) //上月
            {
                #region 上月
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
                int ForYear = ForDate.Year;
                int ForMonth = ForDate.Month;
                strWhe += " Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "";
                #endregion
            }
            else if (showtype == 5) //上周
            {
                #region 上周
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToShortDateString());
                string M_Str_mindate = string.Empty;
                string M_Str_Maxdate = string.Empty;

                switch (ForDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = ForDate.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = ForDate.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = ForDate.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = ForDate.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = ForDate.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = ForDate.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = ForDate.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                M_Str_Maxdate = DateTime.Parse(M_Str_mindate).AddDays(6).ToShortDateString();
                strWhe += " AddTime between '" + M_Str_mindate + " 00:00:00' AND '" + M_Str_Maxdate + " 23:59:59'";
                #endregion
            }
            strWhe += "  and  IsDel=0";
            #region 计算记录数
            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Text " + strWhe + "";
            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 100)
                p_recordCount = 100;
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listForumstat;
            }
            #endregion

            #region 取出相关记录数
            // 取出相关记录
            string queryString = "SELECT TOP 100 UsID,COUNT(UsID) FROM tb_Text " + strWhe + " GROUP BY UsID ORDER BY COUNT(UsID) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Text objForumstat = new BCW.Model.Text();
                        objForumstat.UsID = reader.GetInt32(0);
                        //objForumstat.UsName = reader.GetString(1);
                        objForumstat.ReadNum = reader.GetInt32(1);//用ReadNum代替返回值

                        listForumstat.Add(objForumstat);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }
            #endregion

            return listForumstat;
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTextsMe(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Text> listTexts = new List<BCW.Model.Text>();
            string sTable = "tb_Text";
            string sPkey = "id";
            string sField = "ID,Forumid,Title,UsID,UsName,AddTime";
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
                    return listTexts;
                }
                while (reader.Read())
                {
                    BCW.Model.Text objText = new BCW.Model.Text();
                    objText.ID = reader.GetInt32(0);
                    objText.ForumId = reader.GetInt32(1);
                    objText.Title = Out.TitleUBB(reader.GetString(2));
                    objText.UsID = reader.GetInt32(3);
                    objText.UsName = reader.GetString(4);
                    objText.AddTime = reader.GetDateTime(5);
                    listTexts.Add(objText);
                }
            }
            return listTexts;
        }

        #region 论坛点赞排行分页记录 这里开始修改
        /// <summary>
        /// 论坛点赞排行分页记录 陈志基 20160324
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Text> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int showtype, out int p_recordCount)
        {
            IList<BCW.Model.Text> listForumstat = new List<BCW.Model.Text>();
            string strWhe = string.Empty;
            if (strWhere != "" || showtype > 1)
                strWhe += " where ";

            if (strWhere != "")
                strWhe += strWhere;

            if (strWhere != "" && showtype > 1)
                strWhe += " and ";

            if (showtype == 5) //本月
            {
                #region 本月
                strWhe += " Year(PraiseTime)=" + DateTime.Now.Year + " and Month(PraiseTime)=" + DateTime.Now.Month + "";
                #endregion
            }

            if (showtype == 6) //本年
            {
                #region 本月
                strWhe += " Year(PraiseTime)=" + DateTime.Now.Year;
                #endregion
            }


            #region 计算记录数
            // 计算记录数
           // string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Text " + strWhe + "";
            string countString = "SELECT UsID,sum(Praise) FROM tb_Text"+strWhe+" GROUP BY UsID ORDER BY sum(Praise) DESC";
            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 100)
                p_recordCount = 100;
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listForumstat;
            }
            #endregion

            #region 取出相关记录数
            // 取出相关记录
            string queryString = "SELECT TOP 100 UsID,sum(Praise) FROM tb_Text " + strWhe + " GROUP BY UsID ORDER BY sum(Praise) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Text objForumstat = new BCW.Model.Text();
                        objForumstat.UsID = reader.GetInt32(0);
                        //objForumstat.UsName = reader.GetString(1);
                        objForumstat.Praise = reader.GetInt32(1);

                        listForumstat.Add(objForumstat);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }
            #endregion

            return listForumstat;
        }
        #endregion


        /// <summary>
        /// 取得高手论坛排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTextsGs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Text> listTexts = new List<BCW.Model.Text>();
            string sTable = "tb_Text";
            string sPkey = "id";
            string sField = "ID,Forumid,Title,AddTime,Gaddnum,Gwinnum,Glznum,Gmnum,UsID,UsName";
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
                    return listTexts;
                }
                while (reader.Read())
                {
                    BCW.Model.Text objText = new BCW.Model.Text();
                    objText.ID = reader.GetInt32(0);
                    objText.ForumId = reader.GetInt32(1);
                    objText.Title = reader.GetString(2);
                    objText.AddTime = reader.GetDateTime(3);
                    objText.Gaddnum = reader.GetInt32(4);
                    objText.Gwinnum = reader.GetInt32(5);
                    objText.Glznum = reader.GetInt32(6);
                    objText.Gmnum = reader.GetInt32(7);
                    objText.UsID = reader.GetInt32(8);
                    objText.UsName = reader.GetString(9);
                    listTexts.Add(objText);
                }
            }
            return listTexts;
        }

        #endregion  成员方法
    }
}
