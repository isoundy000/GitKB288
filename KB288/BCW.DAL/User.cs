using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

/// <summary>
/// 增加已支付字段
/// 黄国军 20160611
/// </summary>
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类User。
    /// </summary>
    public class User
    {
        public User()
        { }
        #region  成员方法

        /// <summary>
        /// 得到前台设计中心超时时间
        /// </summary>
        public DateTime GetManAcTime(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ManAcTime from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetDateTime(0);
                    else
                        return Convert.ToDateTime("1990-1-1");
                }
                else
                {
                    return Convert.ToDateTime("1990-1-1");
                }
            }
        }

        /// <summary>
        /// 更新前台设计中心超时时间
        /// </summary>
        public void UpdateManAcTime(int ID, DateTime ManAcTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("ManAcTime=@ManAcTime ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ManAcTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ManAcTime;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新财富/基金当前支付时间
        /// </summary>
        public void UpdateTimeLimit(int ID, DateTime TimeLimit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("TimeLimit=@TimeLimit ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TimeLimit", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = TimeLimit;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperUser.GetMaxID("ID", "tb_User");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsID(long ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该手机号记录
        /// </summary>
        public bool Exists(string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar)};
            parameters[0].Value = Mobile;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该邮箱记录
        /// </summary>
        public bool ExistsEmail(string Email)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where Email=@Email ");
            SqlParameter[] parameters = {
					new SqlParameter("@Email", SqlDbType.NVarChar)};
            parameters[0].Value = Email;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该用户昵称记录
        /// </summary>
        public bool ExistsUsName(string UsName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where UsName=@UsName ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsName", SqlDbType.NVarChar)};
            parameters[0].Value = UsName;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该用户昵称记录
        /// </summary>
        public bool ExistsUsName(string UsName, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where UsName=@UsName ");
            strSql.Append(" and ID<>@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsName", SqlDbType.NVarChar),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = UsName;
            parameters[1].Value = ID;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该手机号和邮箱（找回密码）
        /// </summary>
        public bool Exists(string Mobile, string Email)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where Mobile=@Mobile ");
            strSql.Append(" and Email=@Email ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;
            parameters[1].Value = Email;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据ID和密码查询影响的行数
        /// </summary>
        /// <returns></returns>
        public int GetRowByID(BCW.Model.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID from tb_User where ");
            strSql.Append("ID=@ID ");
            strSql.Append("and UsPwd=@UsPwd");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsPwd;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 根据手机号和密码查询影响的行数
        /// </summary>
        /// <returns></returns>
        public int GetRowByMobile(BCW.Model.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID from tb_User where ");
            strSql.Append("Mobile=@Mobile ");
            strSql.Append("and UsPwd=@UsPwd");
            SqlParameter[] parameters = {
                    new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@UsPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Mobile;
            parameters[1].Value = model.UsPwd;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到某论坛的在线人数
        /// </summary>
        public int GetForumNum(int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");
            strSql.Append(" where EndForumID=@EndForumID ");
            strSql.Append(" and EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@EndForumID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到某圈子的在线人数
        /// </summary>
        public int GetGroupNum(int GroupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");
            strSql.Append(" where GroupId LIKE '%#" + GroupId + "#%'");
            strSql.Append(" and EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ");

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
        /// 得到某聊天室的在线人数
        /// </summary>
        public int GetChatNum(int ChatID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");
            strSql.Append(" where EndChatID=@EndChatID ");
            strSql.Append(" and EndTime>='" + DateTime.Now.AddMinutes(-5) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@EndChatID", SqlDbType.Int,4)};
            parameters[0].Value = ChatID;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到聊天室总在线人数
        /// </summary>
        public int GetChatNum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");
            strSql.Append(" where EndChatID>@EndChatID ");
            strSql.Append(" and EndTime>='" + DateTime.Now.AddMinutes(-5) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@EndChatID", SqlDbType.Int,4)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到闲聊总在线人数
        /// </summary>
        public int GetSpeakNum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");
            strSql.Append(" where EndSpeakID>=@EndSpeakID ");
            strSql.Append(" and EndSpeakTime>='" + DateTime.Now.AddMinutes(-5) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@EndSpeakID", SqlDbType.Int,4)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到在线人数
        /// </summary>
        public int GetNum(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");
            if (Types == 1)
                strSql.Append(" where Sex<=@Sex and ");
            else if (Types == 2)
                strSql.Append(" where Sex=@Sex and ");
            else if (Types == 3)
            {
                strSql.Append(" where IsSpier=@Sex and ");
                Types = 1;
            }
            else
                strSql.Append(" where");

            strSql.Append(" EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sex", SqlDbType.TinyInt,1)};
            parameters[0].Value = Types;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到会员总数
        /// </summary>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User");

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
        /// 得到人气排名
        /// </summary>
        public int GetClickTop(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(ID) from tb_User where Click>=(select Click from tb_User");
            strSql.Append(" where ID=@ID) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到是否机器人ID(0否/1是)
        /// </summary>
        public int GetIsSpier(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsSpier from tb_User");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 得到随机一个机器人ID
        /// </summary>
        public int GetIsSpierRandID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top 1 ID from tb_User where IsSpier=1 Order by NEWID()");

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
        public void Add(BCW.Model.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_User(");
            strSql.Append("ID,Mobile,UsName,UsPwd,UsKey,Photo,Sex,RegTime,RegIP,EndTime,EndForumID,Sign,Birth,iGold,iBank,iMoney,iScore,Leven,Click,OnTime,State,Paras,InviteNum,ForumSet,SignTotal,SignKeep,VipGrow,VipDayGrow,IsVerify,Email,EndChatID,IsSpier)");
            strSql.Append(" values (");
            strSql.Append("@ID,@Mobile,@UsName,@UsPwd,@UsKey,@Photo,@Sex,@RegTime,@RegIP,@EndTime,@EndForumID,@Sign,@Birth,@iGold,@iBank,@iMoney,@iScore,@Leven,@Click,@OnTime,@State,@Paras,@InviteNum,@ForumSet,@SignTotal,@SignKeep,@VipGrow,@VipDayGrow,@IsVerify,@Email,@EndChatID,@IsSpier)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@UsPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@UsKey", SqlDbType.NVarChar,50),
					new SqlParameter("@Photo", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@RegTime", SqlDbType.DateTime),
					new SqlParameter("@RegIP", SqlDbType.NVarChar,50),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@EndForumID", SqlDbType.Int,4),
					new SqlParameter("@Sign", SqlDbType.NText),
					new SqlParameter("@Birth", SqlDbType.DateTime),
					new SqlParameter("@iGold", SqlDbType.BigInt,8),
					new SqlParameter("@iBank", SqlDbType.BigInt,8),
					new SqlParameter("@iMoney", SqlDbType.BigInt,8),
					new SqlParameter("@iScore", SqlDbType.BigInt,8),
					new SqlParameter("@Leven", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@OnTime", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@Paras", SqlDbType.NVarChar,200),
					new SqlParameter("@InviteNum", SqlDbType.Int,4),
					new SqlParameter("@ForumSet", SqlDbType.NVarChar,500),
					new SqlParameter("@SignTotal", SqlDbType.Int,4),
					new SqlParameter("@SignKeep", SqlDbType.Int,4),
					new SqlParameter("@VipGrow", SqlDbType.Int,4),
					new SqlParameter("@VipDayGrow", SqlDbType.Int,4),
					new SqlParameter("@IsVerify", SqlDbType.Int,4),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
                    new SqlParameter("@EndChatID", SqlDbType.Int,4),
                    new SqlParameter("@IsSpier", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Mobile;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.UsPwd;
            parameters[4].Value = model.UsKey;
            parameters[5].Value = model.Photo;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.RegTime;
            parameters[8].Value = model.RegIP;
            parameters[9].Value = model.EndTime;
            parameters[10].Value = 0;
            parameters[11].Value = model.Sign;
            parameters[12].Value = model.Birth;
            parameters[13].Value = 0;
            parameters[14].Value = 0;
            parameters[15].Value = 0;
            parameters[16].Value = 0;
            parameters[17].Value = 0;
            parameters[18].Value = 0;
            parameters[19].Value = 0;
            parameters[20].Value = 0;
            parameters[21].Value = "体力|0,魅力|0,智慧|0,威望|0,邪恶|0";
            parameters[22].Value = model.InviteNum;
            parameters[23].Value = "帖子列表条数|8,帖子内容字数|500,回帖列表条数|8,回帖列表内容字数|8,帖子回帖|0,帖子回帖条数|5,帖子图片|0,回帖图片|0,控制面板|0,银行开户|0,银行取息时间|" + DateTime.Now + ",好友验证|0,好友群发|0,圈内群发|0,推荐邀请|0,系统消息|0,游戏系统消息|0,非好友消息|0,未验证手机号|0,留言权限|0,留言通知|0,圈聊提醒|0,支付时间|" + DateTime.Now + ",支付超时|0,财产显示|0,IP变动时间|" + DateTime.Now + ",IP变动超时|0,私信提醒|0,系统内线提醒|0,管理变动时间|" + DateTime.Now + ",闲聊刷新秒数|60,闲聊列表数条数|10,情景|-1,闲聊提醒|0";
            parameters[24].Value = model.SignTotal;
            parameters[25].Value = model.SignKeep;
            parameters[26].Value = 0;
            parameters[27].Value = 0;
            parameters[28].Value = model.IsVerify;
            parameters[29].Value = model.Email;
            parameters[30].Value = model.EndChatID;
            parameters[31].Value = model.IsSpier;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Mobile=@Mobile,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Email=@Email,");
            strSql.Append("Sex=@Sex,");
            strSql.Append("RegTime=@RegTime,");
            strSql.Append("RegIP=@RegIP,");
            strSql.Append("City=@City,");
            strSql.Append("Birth=@Birth,");
            strSql.Append("iScore=@iScore,");
            strSql.Append("Leven=@Leven,");
            strSql.Append("Click=@Click,");
            strSql.Append("OnTime=@OnTime,");
            strSql.Append("State=@State,");
            strSql.Append("SignTotal=@SignTotal,");
            strSql.Append("IsVerify=@IsVerify");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.TinyInt,1),
					new SqlParameter("@RegTime", SqlDbType.DateTime),
					new SqlParameter("@RegIP", SqlDbType.NVarChar,50),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@Birth", SqlDbType.SmallDateTime),
					new SqlParameter("@iScore", SqlDbType.BigInt,8),
					new SqlParameter("@Leven", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@OnTime", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@SignTotal", SqlDbType.Int,4),
					new SqlParameter("@IsVerify", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Mobile;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Email;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.RegTime;
            parameters[6].Value = model.RegIP;
            parameters[7].Value = model.City;
            parameters[8].Value = model.Birth;
            parameters[9].Value = model.iScore;
            parameters[10].Value = model.Leven;
            parameters[11].Value = model.Click;
            parameters[12].Value = model.OnTime;
            parameters[13].Value = model.State;
            parameters[14].Value = model.SignTotal;
            parameters[15].Value = model.IsVerify;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 修改基本资料
        /// </summary>
        public void UpdateEditBasic(BCW.Model.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Email=@Email,");
            strSql.Append("Sex=@Sex,");
            strSql.Append("City=@City,");
            strSql.Append("Birth=@Birth,");
            strSql.Append("Sign=@Sign");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@Birth", SqlDbType.SmallDateTime),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@Sign", SqlDbType.NText)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Email;
            parameters[3].Value = model.Sex;
            parameters[4].Value = model.Birth;
            parameters[5].Value = model.City;
            parameters[6].Value = model.Sign;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        ///// <summary>
        ///// 更新新消息条数
        ///// </summary>
        //public void UpdateGutNum(int ID)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update tb_User set ");
        //    strSql.Append("GutNum=@GutNum ");
        //    strSql.Append(" where ID=@ID ");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4),
        //            new SqlParameter("@GutNum", SqlDbType.Int,4)};
        //    parameters[0].Value = ID;
        //    parameters[1].Value = 0;

        //    SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        //}

        ///// <summary>
        ///// 更新新消息条数
        ///// </summary>
        //public void UpdateGutNum(int ID, int GutNum)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update tb_User set ");
        //    strSql.Append("GutNum=GutNum+@GutNum ");
        //    strSql.Append(" where ID=@ID ");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4),
        //            new SqlParameter("@GutNum", SqlDbType.Int,4)};
        //    parameters[0].Value = ID;
        //    parameters[1].Value = GutNum;

        //    SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        //}

        /// <summary>
        /// 更新最后时间/IP
        /// </summary>
        public void UpdateIpTime(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("EndIP=@EndIP, ");
            strSql.Append("EndTime=@EndTime ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@EndIP", SqlDbType.NVarChar,50),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = Utils.GetUsIP();
            parameters[2].Value = DateTime.Now;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新在线时间
        /// </summary>
        public void UpdateTime(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");

            //string UsIP = Utils.DelLastChar(Utils.GetUsIP(), ".");
            //UsIP = Utils.DelLastChar(UsIP, ".");            
            //if (UsIP != "121.14")
            //{
                strSql.Append("EndIP='192.168.1.116', ");
            //}

            strSql.Append("EndTime='" + DateTime.Now + "',");

            strSql.Append("EndTime2=");
            strSql.Append("case when State=0 then '" + DateTime.Now + "' else EndTime2 END");
            strSql.Append(",OnTime=OnTime+@OnTime ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@OnTime", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新足迹
        /// </summary>
        public void UpdateVisitHy(int ID, string VisitHy)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("VisitHy=@VisitHy ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@VisitHy", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = VisitHy;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        //查询是否为系统号
        //邵广林 20161116
        public bool isIsSpier(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User WHERE ID=" + ID + " AND IsSpier=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
        //随机一个不是机器人登录的IP
        //邵广林 20161116
        public string GetIsSpierIP()
        {
            StringBuilder strSql = new StringBuilder();
            string ip = "";
            int lip = 1;
            try
            {
                ip = ub.Get("SetMobile");
                lip = ip.Length;
            }
            catch { }
            strSql.Append("SELECT TOP(1)EndIP FROM tb_User WHERE left(mobile," + lip + ")!='" + ip + "' AND EndIP!='' AND IsSpier=0 AND EndIP NOT LIKE'%192.168.1%' order by newid()");
            object obj = SqlHelperUser.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return "113.102.127.166";
            }
            else
            {
                return obj.ToString();
            }
        }

        /// <summary>
        /// 更新在线时间
        /// //邵广林 20161116
        /// </summary>
        public void UpdateTime(int ID, int OnTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("EndTime=@EndTime, ");
            strSql.Append("OnTime=OnTime+@OnTime ");
            //判断ID是否为机器人并且手机号码为1510758开头的
            string EndIP = "";
            if (isIsSpier(ID))
            {
                EndIP = GetIsSpierIP();
                strSql.Append(",EndIP='" + EndIP + "' ");
            }
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@OnTime", SqlDbType.Int,4),
                    new SqlParameter("@EndIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = OnTime;
            if (isIsSpier(ID))//机器人
                parameters[3].Value = EndIP;
            else
                parameters[3].Value = Utils.GetUsIP();
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        ///// <summary>
        ///// 更新在线时间————旧
        ///// </summary>
        //public void UpdateTime(int ID, int OnTime)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update tb_User set ");
        //    strSql.Append("EndTime=@EndTime, ");
        //    strSql.Append("OnTime=OnTime+@OnTime ");
        //    strSql.Append(" where ID=@ID ");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4),
        //            new SqlParameter("@EndTime", SqlDbType.DateTime),
        //            new SqlParameter("@OnTime", SqlDbType.Int,4)};
        //    parameters[0].Value = ID;
        //    parameters[1].Value = DateTime.Now;
        //    parameters[2].Value = OnTime;

        //    SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        //}

        /// <summary>
        /// 更新最后在线论坛ID
        /// </summary>
        public void UpdateEndForumID(int ID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("EndForumID=@EndForumID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@EndForumID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新最后在线聊天室ID
        /// </summary>
        public void UpdateEndChatID(int ID, int ChatID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("EndChatID=@EndChatID ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@EndChatID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新最后在线闲聊ID
        /// </summary>
        public void UpdateEndSpeakID(int ID, int SpeakID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("EndSpeakID=@EndSpeakID, ");
            strSql.Append("EndSpeakTime=@EndSpeakTime ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@EndSpeakID", SqlDbType.Int,4),
					new SqlParameter("@EndSpeakTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = SpeakID;
            parameters[2].Value = DateTime.Now;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户虚拟币
        /// </summary>
        public void UpdateiGold(int ID, long iGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("iGold=iGold+@iGold ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iGold", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = iGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        public void UpdateiScore(int ID, long iScore)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("iScore=iScore+@iScore ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iScore", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = iScore;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户银行币
        /// </summary>
        public void UpdateiBank(int ID, long iBank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("iBank=iBank+@iBank ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iBank", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = iBank;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新虚拟元
        /// </summary>
        public void UpdateiMoney(int ID, long iMoney)
        {
            //Utils.Error("" + ub.Get("SiteBz2") + "暂停使用，未把" + ub.Get("SiteBz2") + "兑换成" + ub.Get("SiteBz") + "的，系统于11月20日自动处理，兑换请进入空间金融中心<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=exchange") + "\">" + ub.Get("SiteBz2") + "兑换" + ub.Get("SiteBz") + "</a>,兑换时间到2012-11-19日24点止", "");

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("iMoney=iMoney+@iMoney ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iMoney", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = iMoney;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新推广拥金
        /// </summary>
        public void UpdateiFcGold(int ID, long iFcGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("iFcGold=iFcGold+@iFcGold");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@iFcGold", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = iFcGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新昵称
        /// </summary>
        public void UpdateUsName(int ID, string UsName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsName=@UsName ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = UsName;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新手机号
        /// </summary>
        public void UpdateMobile(int ID, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Mobile=@Mobile ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Mobile;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新139提醒邮箱
        /// </summary>
        public void UpdateSmsEmail(int ID, string SmsEmail)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("SmsEmail=@SmsEmail ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SmsEmail", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = SmsEmail;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新个性签名
        /// </summary>
        public void UpdateSign(int ID, string Sign)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Sign=@Sign ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Sign", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = Sign;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新登录密码
        /// </summary>
        public void UpdateUsPwd(int ID, string UsPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsPwd=@UsPwd ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = UsPwd;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新登录密码
        /// </summary>
        public void UpdateUsPwd(string Mobile, string UsPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsPwd=@UsPwd ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@UsPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;
            parameters[1].Value = UsPwd;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户密匙
        /// </summary>
        public void UpdateUsKey(int ID, string UsKey)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsKey=@UsKey ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsKey", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = UsKey;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新支付密码
        /// </summary>
        public void UpdateUsPled(int ID, string UsPled)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsPled=@UsPled ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsPled", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = UsPled;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新支付密码
        /// </summary>
        public void UpdateUsPled(string Mobile, string UsPled)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsPled=@UsPled ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@UsPled", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;
            parameters[1].Value = UsPled;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新管理密码
        /// </summary>
        public void UpdateUsAdmin(int ID, string UsAdmin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsAdmin=@UsAdmin ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsAdmin", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = UsAdmin;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        public void UpdatePhoto(int ID, string Photo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Photo=@Photo ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Photo", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Photo;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据字段修改数据列表 邵广林 20161107 
        /// </summary>
        public DataSet update_ziduan(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_User SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("State=@State ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新人气
        /// </summary>
        public void UpdateClick(int ID, int Click)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Click=Click+@Click ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Click;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新财富/基金当前支付类型0选择/1财富/2基金
        /// </summary>
        public void UpdatePayType(int ID, int PayType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("PayType=@PayType ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = PayType;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新等级升级
        /// </summary>
        public void UpdateLeven(int ID, int Leven, long iScore)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Leven=@Leven, ");
            strSql.Append("iScore=iScore-@iScore ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
			     	new SqlParameter("@Leven", SqlDbType.Int,4),
					new SqlParameter("@iScore", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = Leven;
            parameters[2].Value = iScore;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        public void UpdateParas(int ID, string Paras)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Paras=@Paras ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Paras", SqlDbType.NVarChar,200)};
            parameters[0].Value = ID;
            parameters[1].Value = Paras;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新个性设置
        /// </summary>
        public void UpdateForumSet(int ID, string ForumSet)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("ForumSet=@ForumSet ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumSet", SqlDbType.NVarChar,500)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumSet;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新复制历史
        /// </summary>
        public void UpdateCopytemp(int ID, string Copytemp)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Copytemp=@Copytemp ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Copytemp", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = Copytemp;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新圈子ID
        /// </summary>
        public void UpdateGroupId(int ID, string GroupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("GroupId=@GroupId ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.NVarChar,200)};
            parameters[0].Value = ID;
            parameters[1].Value = GroupId;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        public void UpdateVerifys(int ID, string Verifys)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Verifys=@Verifys ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Verifys", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Verifys;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新推荐人ID
        /// </summary>
        public void UpdateInviteNum(int ID, int InviteNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("InviteNum=@InviteNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@InviteNum", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = InviteNum;
            parameters[1].Value = ID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新为验证会员
        /// </summary>
        public void UpdateIsVerify(string Mobile, int IsVerify)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("IsVerify=@IsVerify ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@IsVerify", SqlDbType.TinyInt,1),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = IsVerify;
            parameters[1].Value = Mobile;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 冻结会员ID
        /// </summary>
        public void UpdateIsFreeze(int ID, int IsFreeze)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("IsFreeze=@IsFreeze ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@IsFreeze", SqlDbType.TinyInt,1),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = IsFreeze;
            parameters[1].Value = ID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新签到信息
        /// </summary>
        public void UpdateSingData(int ID, int SignTotal, int SignKeep)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("SignTotal=@SignTotal, ");
            strSql.Append("SignKeep=@SignKeep, ");
            strSql.Append("SignTime=@SignTime ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SignTotal", SqlDbType.Int,4),
					new SqlParameter("@SignKeep", SqlDbType.Int,4),
					new SqlParameter("@SignTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = SignTotal;
            parameters[2].Value = SignKeep;
            parameters[3].Value = DateTime.Now;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新VIP信息
        /// </summary>
        public void UpdateVipData(int ID, int VipDayGrow, DateTime VipDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("VipDayGrow=@VipDayGrow, ");
            strSql.Append("VipDate=@VipDate ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@VipDayGrow", SqlDbType.Int,4),
					new SqlParameter("@VipDate", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = VipDayGrow;
            parameters[2].Value = VipDate;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新VIP信息
        /// </summary>
        public void UpdateVipData(int ID, int VipDayGrow, DateTime VipDate, int VipGrow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("VipDayGrow=@VipDayGrow, ");
            strSql.Append("VipDate=@VipDate, ");
            strSql.Append("VipGrow=@VipGrow ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@VipDayGrow", SqlDbType.Int,4),
					new SqlParameter("@VipDate", SqlDbType.DateTime),
					new SqlParameter("@VipGrow", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = VipDayGrow;
            parameters[2].Value = VipDate;
            parameters[3].Value = VipGrow;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新VIP成长点
        /// </summary>
        public void UpdateVipGrow(int ID, int VipGrow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("VipGrow=VipGrow+@VipGrow, ");
            strSql.Append("UpdateDayTime=@UpdateDayTime ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@VipGrow", SqlDbType.Int,4),
					new SqlParameter("@UpdateDayTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = VipGrow;
            parameters[2].Value = DateTime.Now;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        public void UpdateLimit(int ID, string Limit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("Limit=@Limit ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Limit", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Limit;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新UsUbb
        /// </summary>
        public void UpdateUsUbb(int ID, string UsUbb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("UsUbb=@UsUbb ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsUbb", SqlDbType.NVarChar,800)};
            parameters[0].Value = ID;
            parameters[1].Value = UsUbb;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到用户UsKey/UsPwd
        /// </summary>
        public BCW.Model.User GetKey(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UsKey,UsPwd,IsVerify from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsKey = reader.GetString(1);
                    model.UsPwd = reader.GetString(2);
                    model.IsVerify = reader.GetByte(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到用户UsKey/UsPwd
        /// </summary>
        public BCW.Model.User GetKey(string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UsKey,UsPwd,IsVerify from tb_User ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsKey = reader.GetString(1);
                    model.UsPwd = reader.GetString(2);
                    model.IsVerify = reader.GetByte(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// 得到手机号
        /// </summary>
        public int GetID(string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_User ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到手机号
        /// </summary>
        public string GetMobile(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Mobile from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到登录密码
        /// </summary>
        public string GetUsPwd(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsPwd from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到登录密码
        /// </summary>
        public string GetUsPwd(int ID, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsPwd from tb_User ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Mobile;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到支付密码
        /// </summary>
        public string GetUsPled(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsPled from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到支付密码
        /// </summary>
        public string GetUsPled(int ID, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsPled from tb_User ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Mobile;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到管理密码
        /// </summary>
        public string GetUsAdmin(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsAdmin from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到最后IP/最后时间
        /// </summary>
        public BCW.Model.User GetEndIpTime(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EndIP,ForumSet,Limit,EndTime from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.User model = new BCW.Model.User();
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        model.EndIP = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        model.ForumSet = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        model.Limit = reader.GetString(2);

                    model.EndTime = reader.GetDateTime(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// 得到打赏支付的时间
        /// </summary>
        public BCW.Model.User GetTimeLimit(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TimeLimit from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.User model = new BCW.Model.User();
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.TimeLimit = reader.GetDateTime(0);
                    
                    return model;

                }
                else
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// 得到用户币
        /// </summary>
        public long GetGold(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iGold from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到Money
        /// </summary>
        public long GetMoney(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iMoney from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用户银行币
        /// </summary>
        public long GetBank(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iBank from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用户等级
        /// </summary>
        public int GetLeven(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Leven from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用户支付方法 1财富  2基金
        /// </summary>
        public int GetPayType(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PayType from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用户昵称
        /// </summary>
        public string GetUsName(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsName from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到账户已支付金额
        /// </summary>
        public string GetUsISGive(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ISGive from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.IsDBNull(0))
                    {
                        return "0";
                    }
                    else
                    {
                        return reader.GetDecimal(0).ToString("0.00");
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 设置账户已支付金额
        /// </summary>
        public void SetUsISGive(int ID, double ISGive)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("ISGive=@ISGive ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@ISGive", SqlDbType.Decimal)};
            parameters[0].Value = ID;
            parameters[1].Value = ISGive;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到用户签名
        /// </summary>
        public string GetSign(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sign from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用户状态
        /// </summary>
        public int GetState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到属性
        /// </summary>
        public string GetParas(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Paras from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "体力|0,魅力|0,智慧|0,威望|0,邪恶|0";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到个性设置
        /// </summary>
        public string GetForumSet(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ForumSet from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到139手机邮箱
        /// </summary>
        public string GetSmsEmail(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SmsEmail from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到社区UBB身份
        /// </summary>
        public string GetUsUbb(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsUbb from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到复制历史
        /// </summary>
        public string GetCopytemp(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Copytemp from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到足迹
        /// </summary>
        public string GetVisitHy(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select VisitHy from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到加入的圈子ID
        /// </summary>
        public string GetGroupId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GroupId from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到验证码
        /// </summary>
        public string GetVerifys(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Verifys from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到头像
        /// </summary>
        public string GetPhoto(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Photo from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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

        ///// <summary>
        ///// 得到新消息条数
        ///// </summary>
        //public int GetGutNum(int ID)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select GutNum from tb_User ");
        //    strSql.Append(" where ID=@ID ");
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@ID", SqlDbType.Int,4)};
        //    parameters[0].Value = ID;

        //    using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到推荐自己的ID
        /// </summary>
        public int GetInviteNum(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select InviteNum from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到推广拥金
        /// </summary>
        public long GetFcGold(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select iFcGold from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到权限
        /// </summary>
        public string GetLimit(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Limit from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到是否已验证(0未验证/1已验证)
        /// </summary>
        public int GetIsVerify(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsVerify from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到帐户是否已冻结
        /// </summary>
        public int GetIsFreeze(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsFreeze from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用户性别
        /// </summary>
        public int GetSex(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sex from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到最后论坛ID
        /// </summary>
        public int GetEndForumID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EndForumID from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到最后聊天室ID
        /// </summary>
        public int GetEndChatID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EndChatID from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到最后闲聊ID
        /// </summary>
        public int GetEndSpeakID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EndSpeakID from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到签到信息
        /// </summary>
        public BCW.Model.User GetSignData(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SignTotal,SignKeep,SignTime from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.SignTotal = reader.GetInt32(0);
                    model.SignKeep = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.SignTime = reader.GetDateTime(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到VIP信息
        /// </summary>
        public BCW.Model.User GetVipData(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select VipGrow,VipDayGrow,VipDate,UpdateDayTime from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.VipGrow = reader.GetInt32(0);
                    model.VipDayGrow = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.VipDate = reader.GetDateTime(2);
                    if (!reader.IsDBNull(3))
                        model.UpdateDayTime = reader.GetDateTime(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到构造用户昵称显示的标识
        /// </summary>
        public BCW.Model.User GetShowName(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsName,Leven,Sex,State,EndTime,Limit from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsName = reader.GetString(0);
                    model.Leven = reader.GetInt32(1);
                    model.Sex = reader.GetByte(2);
                    model.State = reader.GetByte(3);
                    model.EndTime = reader.GetDateTime(4);
                    if (!reader.IsDBNull(5))
                        model.Limit = reader.GetString(5);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到用户基本信息
        /// </summary>
        public BCW.Model.User GetBasic(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Mobile,UsName,Email,Photo,Sex,RegTime,RegIP,City,Birth,Sign,iGold,iBank,iMoney,iScore,Leven,Click,OnTime,State,SignTotal,EndTime,Paras,IsVerify,EndIP,Limit,ForumSet,IsFreeze,EndTime2,UsUbb from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Mobile = reader.GetString(0);
                    model.UsName = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        model.Email = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.Photo = reader.GetString(3);
                    model.Sex = reader.GetByte(4);
                    model.RegTime = reader.GetDateTime(5);
                    model.RegIP = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        model.City = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        model.Birth = reader.GetDateTime(8);
                    if (!reader.IsDBNull(9))
                        model.Sign = reader.GetString(9);
                    model.iGold = reader.GetInt64(10);
                    model.iBank = reader.GetInt64(11);
                    model.iMoney = reader.GetInt64(12);
                    model.iScore = reader.GetInt64(13);
                    model.Leven = reader.GetInt32(14);
                    model.Click = reader.GetInt32(15);
                    model.OnTime = reader.GetInt32(16);
                    model.State = reader.GetByte(17);
                    model.SignTotal = reader.GetInt32(18);
                    model.EndTime = reader.GetDateTime(19);
                    if (!reader.IsDBNull(20))
                        model.Paras = reader.GetString(20);
                    model.IsVerify = reader.GetByte(21);
                    if (!reader.IsDBNull(22))
                        model.EndIP = reader.GetString(22);

                    if (!reader.IsDBNull(23))
                        model.Limit = reader.GetString(23);
                    else
                        model.Limit = "";

                    model.ForumSet = reader.GetString(24);
                    model.IsFreeze = reader.GetByte(25);
                    if (!reader.IsDBNull(26))
                        model.EndTime2 = reader.GetDateTime(26);
                    else
                        model.EndTime2 = DateTime.Parse("1990-1-1");

                    if (!reader.IsDBNull(27))
                        model.UsUbb = reader.GetString(27);
                    else
                        model.UsUbb = "";

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到修改的基本信息
        /// </summary>
        public BCW.Model.User GetEditBasic(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsName,Email,Sex,Birth,City,Sign from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsName = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        model.Email = reader.GetString(1);
                    model.Sex = reader.GetByte(2);
                    model.Birth = reader.GetDateTime(3);
                    if (!reader.IsDBNull(4))
                        model.City = reader.GetString(4);
                    model.Sign = reader.GetString(5);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到在线的基本信息
        /// </summary>
        public BCW.Model.User GetOnlineBasic(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State,EndTime from tb_User ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.State = reader.GetByte(0);
                    model.EndTime = reader.GetDateTime(1);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到找回密码的基本信息
        /// </summary>
        public BCW.Model.User GetPwdBasic(string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UsName,UsPwd,UsKey from tb_User ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;
            BCW.Model.User model = new BCW.Model.User();

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsName = reader.GetString(1);
                    model.UsPwd = reader.GetString(2);
                    model.UsKey = reader.GetString(3);
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
            strSql.Append(" FROM tb_User ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelperUser.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录（后面页面使用）
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排列方式</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsersManage(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.User> listUsers = new List<BCW.Model.User>();
            string sTable = "tb_User";
            string sPkey = "id";
            string sField = "ID,UsName,State";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelperUser.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listUsers;
                }
                while (reader.Read())
                {
                    BCW.Model.User objUser = new BCW.Model.User();
                    objUser.ID = reader.GetInt32(0);
                    objUser.UsName = reader.GetString(1);
                    objUser.State = reader.GetByte(2);
                    listUsers.Add(objUser);
                }
            }
            return listUsers;
        }

        /// <summary>
        /// 取得每页记录（搜索/在线页面使用）
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.User> listUsers = new List<BCW.Model.User>();
            string sTable = "tb_User";
            string sPkey = "id";
            string sField = "ID,UsName,State";
            string sCondition = strWhere;
            string sOrder = "EndTime Desc";
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelperUser.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listUsers;
                }
                while (reader.Read())
                {
                    BCW.Model.User objUser = new BCW.Model.User();
                    objUser.ID = reader.GetInt32(0);
                    objUser.UsName = reader.GetString(1);
                    objUser.State = reader.GetByte(2);
                    listUsers.Add(objUser);
                }
            }
            return listUsers;
        }

        /// <summary>
        /// 会员排行榜使用
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.User> listUsers = new List<BCW.Model.User>();
            string sTable = "tb_User";
            string sPkey = "id";
            string sField = "ID,UsName," + strOrder.Replace(" Desc", "").Replace(" Asc", "") + "";
            string sCondition = strWhere;
            string sOrder = strOrder + ",ID DESC";
            int iSCounts = 0;

            using (SqlDataReader reader = SqlHelperUser.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    if (!Utils.getPageUrl().Contains("bbsstat.aspx"))
                    {
                        if (p_recordCount > 20)
                            p_recordCount = 20;
                    }

                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listUsers;
                }

                strOrder = strOrder.Replace(" Desc", "").Replace(" Asc", "");
                while (reader.Read())
                {
                    BCW.Model.User objUser = new BCW.Model.User();
                    objUser.ID = reader.GetInt32(0);
                    objUser.UsName = reader.GetString(1);
                    if (strOrder == "iGold")
                        objUser.iGold = reader.GetInt64(2);
                    else if (strOrder == "iBank")
                        objUser.iBank = reader.GetInt64(2);
                    else if (strOrder == "iMoney")
                        objUser.iMoney = reader.GetInt64(2);
                    else if (strOrder == "iScore")
                        objUser.iScore = reader.GetInt64(2);
                    else
                    {
                        if (strOrder != "RegTime")
                            objUser.Click = reader.GetInt32(2);
                        else
                            objUser.Click = reader.GetInt32(0);
                    }
                    listUsers.Add(objUser);
                }
            }
            return listUsers;
        }

        /// <summary>
        /// 推荐会员排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.User> GetInvites(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.User> listUser = new List<BCW.Model.User>();

            //// 计算记录数
            //string countString = "SELECT COUNT(DISTINCT InviteNum) FROM tb_User Where " + strWhere + "";

            //p_recordCount = Convert.ToInt32(SqlHelperUser.GetSingle(countString));
            //if (p_recordCount > 0)
            //{
            //int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            //}
            //else
            //{
            //    return listUser;
            //}

            // 取出相关记录

            p_recordCount = 20;
            int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);

            string queryString = "SELECT TOP 20 InviteNum, COUNT(InviteNum) FROM tb_User Where " + strWhere + " GROUP BY InviteNum ORDER BY COUNT(InviteNum) DESC";

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.User objUser = new BCW.Model.User();
                        objUser.ID = reader.GetInt32(0);
                        objUser.InviteNum = reader.GetInt32(1);
                        listUser.Add(objUser);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listUser;
        }

        #endregion  成员方法
    }
}
