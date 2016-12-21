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
    /// 数据访问类tb_BasketBallList。
    /// </summary>
    public class tb_BasketBallList
    {
        public tb_BasketBallList()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BasketBallList");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsName(int name_en)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BasketBallList");
            strSql.Append(" where name_en=@name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int,4)};
            parameters[0].Value = name_en;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_BasketBallList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BasketBallList(");
            strSql.Append("name_en,classType,matchtime,matchstate,remaintime,hometeamID,hometeam,guestteamID,guestteam,homescore,guestscore,homeone,guestone,hometwo,guesttwo,homethree,guestthree,homefour,guestfour,addtime,addTechnic,explain,explain2,contentList,isHidden,connectId,isDone,result,tv,homeEurope,guestEurope,team1,team2)");
            strSql.Append(" values (");
            strSql.Append("@name_en,@classType,@matchtime,@matchstate,@remaintime,@hometeamID,@hometeam,@guestteamID,@guestteam,@homescore,@guestscore,@homeone,@guestone,@hometwo,@guesttwo,@homethree,@guestthree,@homefour,@guestfour,@addtime,@addTechnic,@explain,@explain2,@contentList,@isHidden,@connectId,@isDone,@result,@tv,@homeEurope,@guestEurope,@team1,@team2)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int,4),
                    new SqlParameter("@classType", SqlDbType.NVarChar,50),
                    new SqlParameter("@matchtime", SqlDbType.DateTime),
                    new SqlParameter("@matchstate", SqlDbType.NChar,20),
                    new SqlParameter("@remaintime", SqlDbType.DateTime),
                    new SqlParameter("@hometeamID", SqlDbType.Int,4),
                    new SqlParameter("@hometeam", SqlDbType.NVarChar,500),
                    new SqlParameter("@guestteamID", SqlDbType.Int,4),
                    new SqlParameter("@guestteam", SqlDbType.NVarChar,500),
                    new SqlParameter("@homescore", SqlDbType.Int,4),
                    new SqlParameter("@guestscore", SqlDbType.Int,4),
                    new SqlParameter("@homeone", SqlDbType.NChar,30),
                    new SqlParameter("@guestone", SqlDbType.NChar,30),
                    new SqlParameter("@hometwo", SqlDbType.NChar,30),
                    new SqlParameter("@guesttwo", SqlDbType.NChar,30),
                    new SqlParameter("@homethree", SqlDbType.NChar,30),
                    new SqlParameter("@guestthree", SqlDbType.NChar,30),
                    new SqlParameter("@homefour", SqlDbType.NChar,30),
                    new SqlParameter("@guestfour", SqlDbType.NChar,30),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@addTechnic", SqlDbType.NChar,30),
                    new SqlParameter("@explain", SqlDbType.NVarChar),
                    new SqlParameter("@explain2", SqlDbType.NVarChar),
                    new SqlParameter("@contentList", SqlDbType.VarChar),
                    new SqlParameter("@isHidden", SqlDbType.Int,4),
                    new SqlParameter("@connectId", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.NChar,30),
                    new SqlParameter("@result", SqlDbType.NVarChar,50),
                    new SqlParameter("@tv", SqlDbType.NVarChar,800),
                    new SqlParameter("@homeEurope", SqlDbType.NChar,50),
                    new SqlParameter("@guestEurope", SqlDbType.NChar,50),
                    new SqlParameter("@team1", SqlDbType.NVarChar,100),
                    new SqlParameter("@team2", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.name_en;
            parameters[1].Value = model.classType;
            parameters[2].Value = model.matchtime;
            parameters[3].Value = model.matchstate;
            parameters[4].Value = model.remaintime;
            parameters[5].Value = model.hometeamID;
            parameters[6].Value = model.hometeam;
            parameters[7].Value = model.guestteamID;
            parameters[8].Value = model.guestteam;
            parameters[9].Value = model.homescore;
            parameters[10].Value = model.guestscore;
            parameters[11].Value = model.homeone;
            parameters[12].Value = model.guestone;
            parameters[13].Value = model.hometwo;
            parameters[14].Value = model.guesttwo;
            parameters[15].Value = model.homethree;
            parameters[16].Value = model.guestthree;
            parameters[17].Value = model.homefour;
            parameters[18].Value = model.guestfour;
            parameters[19].Value = model.addtime;
            parameters[20].Value = model.addTechnic;
            parameters[21].Value = model.explain;
            parameters[22].Value = model.explain2;
            parameters[23].Value = model.contentList;
            parameters[24].Value = model.isHidden;
            parameters[25].Value = model.connectId;
            parameters[26].Value = model.isDone;
            parameters[27].Value = model.result;
            parameters[28].Value = model.tv;
            parameters[29].Value = model.homeEurope;
            parameters[30].Value = model.guestEurope;
            parameters[31].Value = model.team1;
            parameters[32].Value = model.team2;

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
        /// 更新connectId
        /// 与官网的球赛关联
        /// ID本库ID；LinkId官网ID
        /// </summary>
        public void UpdateConnectId(int ID, int LinkId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" connectId= @LinkId ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@LinkId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = LinkId;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新matchstate
        /// </summary>
        public void Updatematchstate(int ID, string matchstate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" matchstate= @matchstate ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                      new SqlParameter("@matchstate", SqlDbType.NChar,20)};
            parameters[0].Value = ID;
            parameters[1].Value = matchstate;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新Europe
        /// </summary>
        public void UpdateEurope(int ID, string homeEurope, string guestEurope)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" homeEurope= @homeEurope ,");
            strSql.Append(" guestEurope= @guestEurope ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@homeEurope", SqlDbType.NChar,50),
                    new SqlParameter("@guestEurope", SqlDbType.NChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = homeEurope;
            parameters[2].Value = guestEurope;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新比分主客比分 
        /// </summary>
        public void UpdateScore(int ID, int homescore, int guestscore)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" homescore= @homescore ,");
            strSql.Append(" guestscore= @guestscore ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                     new SqlParameter("@homescore", SqlDbType.Int,4),
                    new SqlParameter("@guestscore", SqlDbType.Int,4) };
            parameters[0].Value = ID;
            parameters[1].Value = homescore;
            parameters[2].Value = guestscore;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新比分1-4节主客比分 
        /// </summary>
        public void UpdateOneScore(int ID, string homeone, string guestone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" homeone= @homeone ,");
            strSql.Append(" guestone= @guestone ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                     new SqlParameter("@homeone", SqlDbType.NChar,30),
                    new SqlParameter("@guestone", SqlDbType.NChar,30) };
            parameters[0].Value = ID;
            parameters[1].Value = homeone;
            parameters[2].Value = guestone;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新比分2节主客比分 
        /// </summary>
        public void UpdateTwoScore(int ID, string hometwo, string guesttwo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" hometwo= @hometwo ,");
            strSql.Append(" guesttwo= @guesttwo ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                     new SqlParameter("@hometwo", SqlDbType.NChar,30),
                    new SqlParameter("@guesttwo", SqlDbType.NChar,30) };
            parameters[0].Value = ID;
            parameters[1].Value = hometwo;
            parameters[2].Value = guesttwo;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新比分3节主客比分 
        /// </summary>
        public void UpdateThreeScore(int ID, string homethree, string guestthree)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" homethree= @homethree ,");
            strSql.Append(" guestthree= @guestthree ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                     new SqlParameter("@homethree", SqlDbType.NChar,30),
                    new SqlParameter("@guestthree", SqlDbType.NChar,30) };
            parameters[0].Value = ID;
            parameters[1].Value = homethree;
            parameters[2].Value = guestthree;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新比分4节主客比分 
        /// </summary>
        public void UpdateFourScore(int ID, string homefour, string guestfour)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" homefour= @homefour ,");
            strSql.Append(" guestfour= @guestfour ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                     new SqlParameter("@homefour", SqlDbType.NChar,30),
                    new SqlParameter("@guestfour", SqlDbType.NChar,30) };
            parameters[0].Value = ID;
            parameters[1].Value = homefour;
            parameters[2].Value = guestfour;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新进球数据explain 1
        /// </summary>
        public void UpdateExplain(int ID, string explain, string explain2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" explain= @explain ,");
            strSql.Append(" explain2= @explain2 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@explain", SqlDbType.NVarChar),
                    new SqlParameter("@explain2", SqlDbType.NVarChar),};
            parameters[0].Value = ID;
            parameters[1].Value = explain;
            parameters[2].Value = explain;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新result
        /// </summary>
        public void UpdateResult(int ID, string result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" result= @result ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@result", SqlDbType.NVarChar)};
            parameters[0].Value = ID;
            parameters[1].Value = result;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新isHidden 球赛显示与隐藏
        /// </summary>
        public void UpdateHidden(int ID, int isHidden)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" isHidden= @isHidden ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@isHidden", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = isHidden;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新isDone
        /// </summary>
        public void UpdateisDone(int ID, string isDone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append(" isDone= @isDone ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.NChar,30)};
            parameters[0].Value = ID;
            parameters[1].Value = isDone;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新contentList
        /// </summary>
        public void UpdatecontentList(int ID, string contentList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append("contentList=@contentList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@contentList", SqlDbType.Char,500)};
            parameters[0].Value = ID;
            parameters[1].Value = contentList;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_BasketBallList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append("name_en=@name_en,");
            strSql.Append("classType=@classType,");
            strSql.Append("matchtime=@matchtime,");
            strSql.Append("matchstate=@matchstate,");
            strSql.Append("remaintime=@remaintime,");
            strSql.Append("hometeamID=@hometeamID,");
            strSql.Append("hometeam=@hometeam,");
            strSql.Append("guestteamID=@guestteamID,");
            strSql.Append("guestteam=@guestteam,");
            strSql.Append("homescore=@homescore,");
            strSql.Append("guestscore=@guestscore,");
            strSql.Append("homeone=@homeone,");
            strSql.Append("guestone=@guestone,");
            strSql.Append("hometwo=@hometwo,");
            strSql.Append("guesttwo=@guesttwo,");
            strSql.Append("homethree=@homethree,");
            strSql.Append("guestthree=@guestthree,");
            strSql.Append("homefour=@homefour,");
            strSql.Append("guestfour=@guestfour,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("addTechnic=@addTechnic,");
            strSql.Append("explain=@explain,");
            strSql.Append("explain2=@explain2,");
            strSql.Append("contentList=@contentList,");
            strSql.Append("isHidden=@isHidden,");
            strSql.Append("connectId=@connectId,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("result=@result,");
            strSql.Append("tv=@tv,");
            strSql.Append("homeEurope=@homeEurope,");
            strSql.Append("guestEurope=@guestEurope,");
            strSql.Append("team1=@team1,");
            strSql.Append("team2=@team2 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@name_en", SqlDbType.Int,4),
                    new SqlParameter("@classType", SqlDbType.NVarChar,50),
                    new SqlParameter("@matchtime", SqlDbType.DateTime),
                    new SqlParameter("@matchstate", SqlDbType.NChar,20),
                    new SqlParameter("@remaintime", SqlDbType.DateTime),
                    new SqlParameter("@hometeamID", SqlDbType.Int,4),
                    new SqlParameter("@hometeam", SqlDbType.NVarChar,500),
                    new SqlParameter("@guestteamID", SqlDbType.Int,4),
                    new SqlParameter("@guestteam", SqlDbType.NVarChar,500),
                    new SqlParameter("@homescore", SqlDbType.Int,4),
                    new SqlParameter("@guestscore", SqlDbType.Int,4),
                    new SqlParameter("@homeone", SqlDbType.NChar,30),
                    new SqlParameter("@guestone", SqlDbType.NChar,30),
                    new SqlParameter("@hometwo", SqlDbType.NChar,30),
                    new SqlParameter("@guesttwo", SqlDbType.NChar,30),
                    new SqlParameter("@homethree", SqlDbType.NChar,30),
                    new SqlParameter("@guestthree", SqlDbType.NChar,30),
                    new SqlParameter("@homefour", SqlDbType.NChar,30),
                    new SqlParameter("@guestfour", SqlDbType.NChar,30),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@addTechnic", SqlDbType.NChar,30),
                    new SqlParameter("@explain", SqlDbType.NVarChar),
                    new SqlParameter("@explain2", SqlDbType.NVarChar),
                    new SqlParameter("@contentList", SqlDbType.VarChar),
                    new SqlParameter("@isHidden", SqlDbType.Int,4),
                    new SqlParameter("@connectId", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.NChar,30),
                    new SqlParameter("@result", SqlDbType.NVarChar,50),
                    new SqlParameter("@tv", SqlDbType.NVarChar,800),
                    new SqlParameter("@homeEurope", SqlDbType.NChar,50),
                    new SqlParameter("@guestEurope", SqlDbType.NChar,50),
                    new SqlParameter("@team1", SqlDbType.NVarChar,100),
                    new SqlParameter("@team2", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.name_en;
            parameters[2].Value = model.classType;
            parameters[3].Value = model.matchtime;
            parameters[4].Value = model.matchstate;
            parameters[5].Value = model.remaintime;
            parameters[6].Value = model.hometeamID;
            parameters[7].Value = model.hometeam;
            parameters[8].Value = model.guestteamID;
            parameters[9].Value = model.guestteam;
            parameters[10].Value = model.homescore;
            parameters[11].Value = model.guestscore;
            parameters[12].Value = model.homeone;
            parameters[13].Value = model.guestone;
            parameters[14].Value = model.hometwo;
            parameters[15].Value = model.guesttwo;
            parameters[16].Value = model.homethree;
            parameters[17].Value = model.guestthree;
            parameters[18].Value = model.homefour;
            parameters[19].Value = model.guestfour;
            parameters[20].Value = model.addtime;
            parameters[21].Value = model.addTechnic;
            parameters[22].Value = model.explain;
            parameters[23].Value = model.explain2;
            parameters[24].Value = model.contentList;
            parameters[25].Value = model.isHidden;
            parameters[26].Value = model.connectId;
            parameters[27].Value = model.isDone;
            parameters[28].Value = model.result;
            parameters[29].Value = model.tv;
            parameters[30].Value = model.homeEurope;
            parameters[31].Value = model.guestEurope;
            parameters[32].Value = model.team1;
            parameters[33].Value = model.team2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateName_en(BCW.Model.tb_BasketBallList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append("name_en=@name_en,");
            strSql.Append("classType=@classType,");
            strSql.Append("matchtime=@matchtime,");
            strSql.Append("matchstate=@matchstate,");
            strSql.Append("remaintime=@remaintime,");
            strSql.Append("hometeamID=@hometeamID,");
            strSql.Append("hometeam=@hometeam,");
            strSql.Append("guestteamID=@guestteamID,");
            strSql.Append("guestteam=@guestteam,");
            strSql.Append("homescore=@homescore,");
            strSql.Append("guestscore=@guestscore,");
            strSql.Append("homeone=@homeone,");
            strSql.Append("guestone=@guestone,");
            strSql.Append("hometwo=@hometwo,");
            strSql.Append("guesttwo=@guesttwo,");
            strSql.Append("homethree=@homethree,");
            strSql.Append("guestthree=@guestthree,");
            strSql.Append("homefour=@homefour,");
            strSql.Append("guestfour=@guestfour,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("addTechnic=@addTechnic,");
            strSql.Append("explain=@explain,");
            strSql.Append("explain2=@explain2,");
            strSql.Append("contentList=@contentList,");
            strSql.Append("isHidden=@isHidden,");
            strSql.Append("connectId=@connectId,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("result=@result,");
            strSql.Append("tv=@tv,");
            strSql.Append("homeEurope=@homeEurope,");
            strSql.Append("guestEurope=@guestEurope,");
            strSql.Append("team1=@team1,");
            strSql.Append("team2=@team2");
            strSql.Append(" where name_en=@name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int,4),
                    new SqlParameter("@classType", SqlDbType.NVarChar,50),
                    new SqlParameter("@matchtime", SqlDbType.DateTime),
                    new SqlParameter("@matchstate", SqlDbType.NChar,20),
                    new SqlParameter("@remaintime", SqlDbType.DateTime),
                    new SqlParameter("@hometeamID", SqlDbType.Int,4),
                    new SqlParameter("@hometeam", SqlDbType.NVarChar,500),
                    new SqlParameter("@guestteamID", SqlDbType.Int,4),
                    new SqlParameter("@guestteam", SqlDbType.NVarChar,500),
                    new SqlParameter("@homescore", SqlDbType.Int,4),
                    new SqlParameter("@guestscore", SqlDbType.Int,4),
                    new SqlParameter("@homeone", SqlDbType.NChar,30),
                    new SqlParameter("@guestone", SqlDbType.NChar,30),
                    new SqlParameter("@hometwo", SqlDbType.NChar,30),
                    new SqlParameter("@guesttwo", SqlDbType.NChar,30),
                    new SqlParameter("@homethree", SqlDbType.NChar,30),
                    new SqlParameter("@guestthree", SqlDbType.NChar,30),
                    new SqlParameter("@homefour", SqlDbType.NChar,30),
                    new SqlParameter("@guestfour", SqlDbType.NChar,30),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@addTechnic", SqlDbType.NChar,30),
                    new SqlParameter("@explain", SqlDbType.NVarChar),
                    new SqlParameter("@explain2", SqlDbType.NVarChar),
                    new SqlParameter("@contentList", SqlDbType.VarChar),
                    new SqlParameter("@isHidden", SqlDbType.Int,4),
                    new SqlParameter("@connectId", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.NChar,30),
                    new SqlParameter("@result", SqlDbType.NVarChar,50),
                    new SqlParameter("@tv", SqlDbType.NVarChar,800),
                    new SqlParameter("@homeEurope", SqlDbType.NChar,50),
                    new SqlParameter("@guestEurope", SqlDbType.NChar,50),
                    new SqlParameter("@team1", SqlDbType.NVarChar,100),
                    new SqlParameter("@team2", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.name_en;
            parameters[1].Value = model.classType;
            parameters[2].Value = model.matchtime;
            parameters[3].Value = model.matchstate;
            parameters[4].Value = model.remaintime;
            parameters[5].Value = model.hometeamID;
            parameters[6].Value = model.hometeam;
            parameters[7].Value = model.guestteamID;
            parameters[8].Value = model.guestteam;
            parameters[9].Value = model.homescore;
            parameters[10].Value = model.guestscore;
            parameters[11].Value = model.homeone;
            parameters[12].Value = model.guestone;
            parameters[13].Value = model.hometwo;
            parameters[14].Value = model.guesttwo;
            parameters[15].Value = model.homethree;
            parameters[16].Value = model.guestthree;
            parameters[17].Value = model.homefour;
            parameters[18].Value = model.guestfour;
            parameters[19].Value = model.addtime;
            parameters[20].Value = model.addTechnic;
            parameters[21].Value = model.explain;
            parameters[22].Value = model.explain2;
            parameters[23].Value = model.contentList;
            parameters[24].Value = model.isHidden;
            parameters[25].Value = model.connectId;
            parameters[26].Value = model.isDone;
            parameters[27].Value = model.result;
            parameters[28].Value = model.tv;
            parameters[29].Value = model.homeEurope;
            parameters[30].Value = model.guestEurope;
            parameters[31].Value = model.team1;
            parameters[32].Value = model.team2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Updatename_en1(BCW.Model.tb_BasketBallList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append("name_en=@name_en,");
            strSql.Append("matchstate=@matchstate,");
            strSql.Append("homescore=@homescore,");
            strSql.Append("guestscore=@guestscore,");
            strSql.Append("homeone=@homeone,");
            strSql.Append("guestone=@guestone,");
            strSql.Append("hometwo=@hometwo,");
            strSql.Append("guesttwo=@guesttwo,");
            strSql.Append("homethree=@homethree,");
            strSql.Append("guestthree=@guestthree,");
            strSql.Append("homefour=@homefour,");
            strSql.Append("guestfour=@guestfour,");
            strSql.Append("explain=@explain,");
            strSql.Append("explain2=@explain2,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("tv=@tv,");
            strSql.Append("homeEurope=@homeEurope,");
            strSql.Append("guestEurope=@guestEurope");
            strSql.Append(" where name_en=@name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int,4),
                    new SqlParameter("@matchstate", SqlDbType.NChar,20),
                    new SqlParameter("@homescore", SqlDbType.Int,4),
                    new SqlParameter("@guestscore", SqlDbType.Int,4),
                    new SqlParameter("@homeone", SqlDbType.NChar,30),
                    new SqlParameter("@guestone", SqlDbType.NChar,30),
                    new SqlParameter("@hometwo", SqlDbType.NChar,30),
                    new SqlParameter("@guesttwo", SqlDbType.NChar,30),
                    new SqlParameter("@homethree", SqlDbType.NChar,30),
                    new SqlParameter("@guestthree", SqlDbType.NChar,30),
                    new SqlParameter("@homefour", SqlDbType.NChar,30),
                    new SqlParameter("@guestfour", SqlDbType.NChar,30),
                    new SqlParameter("@explain", SqlDbType.NVarChar),
                    new SqlParameter("@explain2", SqlDbType.NVarChar),
                    new SqlParameter("@isDone", SqlDbType.NChar,30),
                    new SqlParameter("@tv", SqlDbType.NVarChar,800),
                    new SqlParameter("@homeEurope", SqlDbType.NChar,50),
                    new SqlParameter("@guestEurope", SqlDbType.NChar,50)};
            parameters[0].Value = model.name_en;
            parameters[1].Value = model.matchstate;
            parameters[2].Value = model.homescore;
            parameters[3].Value = model.guestscore;
            parameters[4].Value = model.homeone;
            parameters[5].Value = model.guestone;
            parameters[6].Value = model.hometwo;
            parameters[7].Value = model.guesttwo;
            parameters[8].Value = model.homethree;
            parameters[9].Value = model.guestthree;
            parameters[10].Value = model.homefour;
            parameters[11].Value = model.guestfour;
            parameters[12].Value = model.explain;
            parameters[13].Value = model.explain2;
            parameters[14].Value = model.isDone;
            parameters[15].Value = model.tv;
            parameters[16].Value = model.homeEurope;
            parameters[17].Value = model.guestEurope;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新 数据
        /// </summary>
        public void Updatename_en2(BCW.Model.tb_BasketBallList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BasketBallList set ");
            strSql.Append("name_en=@name_en,");
            strSql.Append("matchtime=@matchtime,");
            strSql.Append("matchstate=@matchstate,");
            strSql.Append("remaintime=@remaintime,");
            strSql.Append("homescore=@homescore,");
            strSql.Append("guestscore=@guestscore,");
            strSql.Append("homeone=@homeone,");
            strSql.Append("guestone=@guestone,");
            strSql.Append("hometwo=@hometwo,");
            strSql.Append("guesttwo=@guesttwo,");
            strSql.Append("homethree=@homethree,");
            strSql.Append("guestthree=@guestthree,");
            strSql.Append("homefour=@homefour,");
            strSql.Append("guestfour=@guestfour,");
            strSql.Append("addTechnic=@addTechnic,");
            strSql.Append("explain=@explain,");
            strSql.Append("explain2=@explain2,");
            strSql.Append("contentList=@contentList,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("result=@result,");
            strSql.Append("tv=@tv,");
            strSql.Append("homeEurope=@homeEurope,");
            strSql.Append("guestEurope=@guestEurope,");
            strSql.Append("team1=@team1,");
            strSql.Append("team2=@team2");
            strSql.Append(" where name_en=@name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int,4),
                    new SqlParameter("@matchtime", SqlDbType.DateTime),
                    new SqlParameter("@matchstate", SqlDbType.NChar,20),
                    new SqlParameter("@remaintime", SqlDbType.DateTime),
                    new SqlParameter("@homescore", SqlDbType.Int,4),
                    new SqlParameter("@guestscore", SqlDbType.Int,4),
                    new SqlParameter("@homeone", SqlDbType.NChar,30),
                    new SqlParameter("@guestone", SqlDbType.NChar,30),
                    new SqlParameter("@hometwo", SqlDbType.NChar,30),
                    new SqlParameter("@guesttwo", SqlDbType.NChar,30),
                    new SqlParameter("@homethree", SqlDbType.NChar,30),
                    new SqlParameter("@guestthree", SqlDbType.NChar,30),
                    new SqlParameter("@homefour", SqlDbType.NChar,30),
                    new SqlParameter("@guestfour", SqlDbType.NChar,30),
                    new SqlParameter("@addTechnic", SqlDbType.NChar,30),
                    new SqlParameter("@explain", SqlDbType.NVarChar),
                    new SqlParameter("@explain2", SqlDbType.NVarChar),
                    new SqlParameter("@contentList", SqlDbType.VarChar),
                    new SqlParameter("@isDone", SqlDbType.NChar,30),
                    new SqlParameter("@result", SqlDbType.NVarChar,50),
                    new SqlParameter("@tv", SqlDbType.NVarChar,800),
                    new SqlParameter("@homeEurope", SqlDbType.NChar,50),
                    new SqlParameter("@guestEurope", SqlDbType.NChar,50),
                    new SqlParameter("@team1", SqlDbType.NVarChar,100),
                    new SqlParameter("@team2", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.name_en;
            parameters[1].Value = model.matchtime;
            parameters[2].Value = model.matchstate;
            parameters[3].Value = model.remaintime;
            parameters[4].Value = model.homescore;
            parameters[5].Value = model.guestscore;
            parameters[6].Value = model.homeone;
            parameters[7].Value = model.guestone;
            parameters[8].Value = model.hometwo;
            parameters[9].Value = model.guesttwo;
            parameters[10].Value = model.homethree;
            parameters[11].Value = model.guestthree;
            parameters[12].Value = model.homefour;
            parameters[13].Value = model.guestfour;
            parameters[14].Value = model.addTechnic;
            parameters[15].Value = model.explain;
            parameters[16].Value = model.explain2;
            parameters[17].Value = model.contentList;
            parameters[18].Value = model.isDone;
            parameters[19].Value = model.result;
            parameters[20].Value = model.tv;
            parameters[21].Value = model.homeEurope;
            parameters[22].Value = model.guestEurope;
            parameters[23].Value = model.team1;
            parameters[24].Value = model.team2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BasketBallList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// matchstate For Id 
        ///获得win7中的球赛编号
        /// </summary>
        public string GetStateFromId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select matchstate from tb_BasketBallList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "0";
                }
                else
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 得到name_en For Id 
        ///获得win7中的球赛编号
        /// </summary>
        public int GetName_enFromId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select name_en from tb_BasketBallList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到name_en For Id 
        ///获得win7中的球赛编号
        /// </summary>
        public int GetIDFromName_en(int name_en)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_BasketBallList ");
            strSql.Append(" where name_en=@name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int)};
            parameters[0].Value = name_en;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// 得到HomeScore For Id 
        ///获得球赛的主队比分
        /// </summary>
        public int GetHomeScoreFromId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select homescore from tb_BasketBallList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到GuestScore For Id 
        ///获得球赛的客队的当前比分
        /// </summary>
        public int GetGuestScoreFromId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select guestscore from tb_BasketBallList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到Id
        /// </summary>
        public int GetIdFromName_en(int name_en)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_BasketBallList ");
            strSql.Append(" where name_en=@name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_en", SqlDbType.Int)};
            parameters[0].Value = name_en;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_BasketBallList Gettb_BasketBallListForName_en(int Name_en)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,name_en,classType,matchtime,matchstate,remaintime,hometeamID,hometeam,guestteamID,guestteam,homescore,guestscore,homeone,guestone,hometwo,guesttwo,homethree,guestthree,homefour,guestfour,addtime,addTechnic,explain,explain2,contentList,isHidden,connectId,isDone,result,tv,homeEurope,guestEurope,team1,team2 from tb_BasketBallList ");
            strSql.Append(" where Name_en=@Name_en ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Name_en", SqlDbType.Int,4)};
            parameters[0].Value = Name_en;

            BCW.Model.tb_BasketBallList model = new BCW.Model.tb_BasketBallList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name_en = reader.GetInt32(1);
                    model.classType = reader.GetString(2);
                    model.matchtime = reader.GetDateTime(3);
                    model.matchstate = reader.GetString(4);
                    model.remaintime = reader.GetDateTime(5);
                    model.hometeamID = reader.GetInt32(6);
                    model.hometeam = reader.GetString(7);
                    model.guestteamID = reader.GetInt32(8);
                    model.guestteam = reader.GetString(9);
                    model.homescore = reader.GetInt32(10);
                    model.guestscore = reader.GetInt32(11);
                    model.homeone = reader.GetString(12);
                    model.guestone = reader.GetString(13);
                    model.hometwo = reader.GetString(14);
                    model.guesttwo = reader.GetString(15);
                    model.homethree = reader.GetString(16);
                    model.guestthree = reader.GetString(17);
                    model.homefour = reader.GetString(18);
                    model.guestfour = reader.GetString(19);
                    model.addtime = reader.GetDateTime(20);
                    model.addTechnic = reader.GetString(21);
                    model.explain = reader.GetString(22);
                    model.explain2 = reader.GetString(23);
                    model.contentList = reader.GetString(24);
                    model.isHidden = reader.GetInt32(25);
                    model.connectId = reader.GetInt32(26);
                    model.isDone = reader.GetString(27);
                    model.result = reader.GetString(28);
                    model.tv = reader.GetString(29);
                    model.homeEurope = reader.GetString(30);
                    model.guestEurope = reader.GetString(31);
                    model.team1 = reader.GetString(32);
                    model.team2 = reader.GetString(33);
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
        public BCW.Model.tb_BasketBallList Gettb_BasketBallList(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,name_en,classType,matchtime,matchstate,remaintime,hometeamID,hometeam,guestteamID,guestteam,homescore,guestscore,homeone,guestone,hometwo,guesttwo,homethree,guestthree,homefour,guestfour,addtime,addTechnic,explain,explain2,contentList,isHidden,connectId,isDone,result,tv,homeEurope,guestEurope,team1,team2 from tb_BasketBallList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.tb_BasketBallList model = new BCW.Model.tb_BasketBallList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name_en = reader.GetInt32(1);
                    model.classType = reader.GetString(2);
                    model.matchtime = reader.GetDateTime(3);
                    model.matchstate = reader.GetString(4);
                    model.remaintime = reader.GetDateTime(5);
                    model.hometeamID = reader.GetInt32(6);
                    model.hometeam = reader.GetString(7);
                    model.guestteamID = reader.GetInt32(8);
                    model.guestteam = reader.GetString(9);
                    model.homescore = reader.GetInt32(10);
                    model.guestscore = reader.GetInt32(11);
                    model.homeone = reader.GetString(12);
                    model.guestone = reader.GetString(13);
                    model.hometwo = reader.GetString(14);
                    model.guesttwo = reader.GetString(15);
                    model.homethree = reader.GetString(16);
                    model.guestthree = reader.GetString(17);
                    model.homefour = reader.GetString(18);
                    model.guestfour = reader.GetString(19);
                    model.addtime = reader.GetDateTime(20);
                    model.addTechnic = reader.GetString(21);
                    model.explain = reader.GetString(22);
                    model.explain2 = reader.GetString(23);
                    model.contentList = reader.GetString(24);
                    model.isHidden = reader.GetInt32(25);
                    model.connectId = reader.GetInt32(26);
                    model.isDone = reader.GetString(27);
                    model.result = reader.GetString(28);
                    model.tv = reader.GetString(29);
                    model.homeEurope = reader.GetString(30);
                    model.guestEurope = reader.GetString(31);
                    model.team1 = reader.GetString(32);
                    model.team2 = reader.GetString(33);
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
            strSql.Append(" FROM tb_BasketBallList ");
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
        /// <returns>IList tb_BasketBallList</returns>
        public IList<BCW.Model.tb_BasketBallList> Gettb_BasketBallLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_BasketBallList> listtb_BasketBallLists = new List<BCW.Model.tb_BasketBallList>();
            string sTable = "tb_BasketBallList";
            string sPkey = "id";
            string sField = "ID,name_en,classType,matchtime,matchstate,remaintime,hometeamID,hometeam,guestteamID,guestteam,homescore,guestscore,homeone,guestone,hometwo,guesttwo,homethree,guestthree,homefour,guestfour,addtime,addTechnic,explain,explain2,contentList,isHidden,connectId,isDone,result,tv,homeEurope,guestEurope,team1,team2";
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
                    return listtb_BasketBallLists;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_BasketBallList objtb_BasketBallList = new BCW.Model.tb_BasketBallList();
                    objtb_BasketBallList.ID = reader.GetInt32(0);
                    objtb_BasketBallList.name_en = reader.GetInt32(1);
                    objtb_BasketBallList.classType = reader.GetString(2);
                    objtb_BasketBallList.matchtime = reader.GetDateTime(3);
                    objtb_BasketBallList.matchstate = reader.GetString(4);
                    objtb_BasketBallList.remaintime = reader.GetDateTime(5);
                    objtb_BasketBallList.hometeamID = reader.GetInt32(6);
                    objtb_BasketBallList.hometeam = reader.GetString(7);
                    objtb_BasketBallList.guestteamID = reader.GetInt32(8);
                    objtb_BasketBallList.guestteam = reader.GetString(9);
                    objtb_BasketBallList.homescore = reader.GetInt32(10);
                    objtb_BasketBallList.guestscore = reader.GetInt32(11);
                    objtb_BasketBallList.homeone = reader.GetString(12);
                    objtb_BasketBallList.guestone = reader.GetString(13);
                    objtb_BasketBallList.hometwo = reader.GetString(14);
                    objtb_BasketBallList.guesttwo = reader.GetString(15);
                    objtb_BasketBallList.homethree = reader.GetString(16);
                    objtb_BasketBallList.guestthree = reader.GetString(17);
                    objtb_BasketBallList.homefour = reader.GetString(18);
                    objtb_BasketBallList.guestfour = reader.GetString(19);
                    objtb_BasketBallList.addtime = reader.GetDateTime(20);
                    objtb_BasketBallList.addTechnic = reader.GetString(21);
                    objtb_BasketBallList.explain = reader.GetString(22);
                    objtb_BasketBallList.explain2 = reader.GetString(23);
                    objtb_BasketBallList.contentList = reader.GetString(24);
                    objtb_BasketBallList.isHidden = reader.GetInt32(25);
                    objtb_BasketBallList.connectId = reader.GetInt32(26);
                    objtb_BasketBallList.isDone = reader.GetString(27);
                    objtb_BasketBallList.result = reader.GetString(28);
                    objtb_BasketBallList.tv = reader.GetString(29);
                    objtb_BasketBallList.homeEurope = reader.GetString(30);
                    objtb_BasketBallList.guestEurope = reader.GetString(31);
                    objtb_BasketBallList.team1 = reader.GetString(32);
                    objtb_BasketBallList.team2 = reader.GetString(33);
                    listtb_BasketBallLists.Add(objtb_BasketBallList);
                }
            }
            return listtb_BasketBallLists;
        }

        #endregion  成员方法
    }
}

