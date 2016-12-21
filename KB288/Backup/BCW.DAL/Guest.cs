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
    /// 数据访问类Guest。
    /// </summary>
    public class Guest
    {
        public Guest()
        { }
        #region  成员方法

        /// <summary>
        /// 得到个性设置
        /// </summary>
        public int GetForumSet(string ForumSet, int iType)
        {
            if (string.IsNullOrEmpty(ForumSet))
                return 0;

            string[] forumset = ForumSet.Split(",".ToCharArray());

            string[] fs = forumset[iType].ToString().Split("|".ToCharArray());

            return Convert.ToInt32(fs[1]);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperUser.GetMaxID("ID", "tb_Guest");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Guest");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsFrom(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Guest");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and FromId=@FromId ");
            strSql.Append(" and FDel=@FDel ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@FDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTo(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Guest");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ToId=@ToId ");
            strSql.Append(" and TDel=@TDel ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算多少条未读非系统消息
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Guest");
            strSql.Append(" where ToID=@ToID ");
            strSql.Append(" and FromId>0 ");
            strSql.Append(" and IsSeen=@IsSeen ");
            strSql.Append(" and TDel=0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ToID", SqlDbType.Int,4),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
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
        /// 计算多少条未读系统消息
        /// </summary>
        public int GetXCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Guest");
            strSql.Append(" where ToID=@ToID ");
            strSql.Append(" and FromId=@FromId ");
            strSql.Append(" and IsSeen=@IsSeen ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ToID", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = 0;
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
        /// 计算某ID消息发送数
        /// </summary>
        public int GetIDCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Guest");
            strSql.Append(" where FromId=@FromId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@FromId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Guest model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Guest(");
            strSql.Append("Types,FromId,FromName,ToId,ToName,Content,IsSeen,IsKeep,FDel,TDel,TransId,AddUsIP,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@FromId,@FromName,@ToId,@ToName,@Content,@IsSeen,@IsKeep,@FDel,@TDel,@TransId,@AddUsIP,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Types", SqlDbType.TinyInt,1),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@FromName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@ToName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Content", SqlDbType.NVarChar,1000),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsKeep", SqlDbType.TinyInt,1),
                    new SqlParameter("@FDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@TransId", SqlDbType.Int,4),
                    new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = 0;
            parameters[1].Value = model.FromId;
            parameters[2].Value = model.FromName;
            parameters[3].Value = model.ToId;
            parameters[4].Value = model.ToName;
            parameters[5].Value = model.Content;
            parameters[6].Value = 0;
            parameters[7].Value = 0;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = model.TransId;
            parameters[11].Value = Utils.GetUsIP();
            parameters[12].Value = DateTime.Now;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);

            string ForumSet = new BCW.DAL.User().GetForumSet(model.ToId);
            string SmsEmail = new BCW.DAL.User().GetSmsEmail(model.ToId);
            int IsSms1 = GetForumSet(ForumSet, 27);

            if (IsSms1 == 1 && SmsEmail != "" && !Utils.getPageUrl().Contains("getover.aspx"))
            {
                //设定参数
                //string EmailFrom = "kb288sp2@163.com";
                //string EmailFromUser = "kb288sp2";
                //string EmailFromPwd = "kb288sp2opklnm";
                //string EmailFromHost = "smtp.163.com";
                //string EmailFromPort = "25";
                string xmlPath = "/Controls/email.xml";
                string EmailFrom = ub.GetSub("EmailFrom", xmlPath);
                string EmailFromUser = ub.GetSub("EmailFromUser", xmlPath);
                string EmailFromPwd = ub.GetSub("EmailFromPwd", xmlPath);
                string EmailFromHost = ub.GetSub("EmailFromHost", xmlPath);
                string EmailFromPort = ub.GetSub("EmailFromPort", xmlPath);


                // 发件人地址
                string From = EmailFrom;
                // 收件人地址
                string To = SmsEmail;
                // 邮件主题
                string Subject = "来自-" + model.FromName + "(" + model.FromId + ")";
                // 邮件正文
                string Body = "" + model.FromName + "(" + model.FromId + "):";
                if (!SmsEmail.Contains("@139.com"))
                {
                    Body = "" + model.FromName + "(" + model.FromId + ")给您的内线:<BR>" + model.Content + "";
                }
                // 邮件主机地址
                string Host = EmailFromHost;
                // 邮件主机端口
                int Port = Utils.ParseInt(EmailFromPort);
                // 登录帐号
                string UserName = EmailFromUser;
                // 登录密码
                string Password = EmailFromPwd;
                //附件地址
                string FilePath = "";
                if (EmailFrom != "")
                    new SendMail().Send(From, To, Subject, Body, Host, Port, UserName, Password, FilePath);
            }

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
        /// 增加一条系统消息
        /// </summary>
        public int Add(int Types, int ToId, string ToName, string Content)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Guest(");
            strSql.Append("Types,FromId,FromName,ToId,ToName,Content,IsSeen,IsKeep,FDel,TDel,TransId,AddUsIP,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@FromId,@FromName,@ToId,@ToName,@Content,@IsSeen,@IsKeep,@FDel,@TDel,@TransId,@AddUsIP,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Types", SqlDbType.TinyInt,1),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@FromName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@ToName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Content", SqlDbType.NVarChar,1000),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsKeep", SqlDbType.TinyInt,1),
                    new SqlParameter("@FDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@TransId", SqlDbType.Int,4),
                    new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = Types;
            parameters[1].Value = 0;
            parameters[2].Value = "系统消息";
            parameters[3].Value = ToId;
            parameters[4].Value = ToName;
            parameters[5].Value = Content;
            parameters[6].Value = 0;
            parameters[7].Value = 0;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = Utils.GetUsIP();
            parameters[12].Value = DateTime.Now;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (Types != 4)
            {
                string ForumSet = new BCW.DAL.User().GetForumSet(ToId);
                string SmsEmail = new BCW.DAL.User().GetSmsEmail(ToId);
                int IsSms2 = GetForumSet(ForumSet, 28);

                if (IsSms2 == 1 && SmsEmail != "" && !Utils.getPageUrl().Contains("getover.aspx"))
                {
                    //设定参数
                    //string EmailFrom = "kb288sp2@163.com";
                    //string EmailFromUser = "kb288sp2";
                    //string EmailFromPwd = "kb288sp2opklnm";
                    //string EmailFromHost = "smtp.163.com";
                    //string EmailFromPort = "25";
                    string xmlPath = "/Controls/email.xml";
                    string EmailFrom = ub.GetSub("EmailFrom", xmlPath);
                    string EmailFromUser = ub.GetSub("EmailFromUser", xmlPath);
                    string EmailFromPwd = ub.GetSub("EmailFromPwd", xmlPath);
                    string EmailFromHost = ub.GetSub("EmailFromHost", xmlPath);
                    string EmailFromPort = ub.GetSub("EmailFromPort", xmlPath);

                    // 发件人地址
                    string From = EmailFrom;
                    // 收件人地址
                    string To = SmsEmail;
                    // 邮件主题
                    string Subject = "来自-系统内线";
                    // 邮件正文
                    string Body = "c";
                    if (!SmsEmail.Contains("@139.com"))
                    {
                        Body = "系统内线<BR>" + Content + "";
                    }
                    // 邮件主机地址
                    string Host = EmailFromHost;
                    // 邮件主机端口
                    int Port = Utils.ParseInt(EmailFromPort);
                    // 登录帐号
                    string UserName = EmailFromUser;
                    // 登录密码
                    string Password = EmailFromPwd;
                    //附件地址
                    string FilePath = "";
                    if (EmailFrom != "")
                        new SendMail().Send(From, To, Subject, Body, Host, Port, UserName, Password, FilePath);
                }
            }
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
        /// 更新为收藏
        /// </summary>
        public void UpdateIsKeep(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("IsKeep=@IsKeep ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ToID=@ToID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@IsKeep", SqlDbType.TinyInt,1),
                    new SqlParameter("@ToID", SqlDbType.Int,8)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;
            parameters[2].Value = UsID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新为已读
        /// </summary>
        public void UpdateIsSeen(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("IsSeen=@IsSeen ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新为已读
        /// </summary>
        public void UpdateIsSeenAll(int UsID, int ptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("IsSeen=@IsSeen ");
            strSql.Append(" where ToId=@ToId ");
            if (ptype == 1)
                strSql.Append(" and FromId=0 ");
            else
                strSql.Append(" and FromId>0 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 隐藏一条数据
        /// </summary>
        public void UpdateFDel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("FDel=@FDel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@FDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 隐藏一条数据
        /// </summary>
        public void UpdateTDel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("TDel=@TDel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 隐藏已读数据
        /// </summary>
        public void UpdateTDel2(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("TDel=@TDel ");
            strSql.Append(" where ToId=@ToId ");
            strSql.Append(" and FromId>@FromId ");
            strSql.Append(" and IsSeen=@IsSeen ");
            strSql.Append(" and IsKeep=@IsKeep ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsKeep", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = 1;
            parameters[3].Value = 1;
            parameters[4].Value = 0;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除已读系统消息
        /// </summary>
        public void UpdateXDel2(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Guest ");
            strSql.Append(" where ToId=@ToId ");
            strSql.Append(" and FromId=@FromId ");
            strSql.Append(" and IsSeen=@IsSeen ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 隐藏已发数据
        /// </summary>
        public void UpdateFDel2(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("FDel=@FDel ");
            strSql.Append(" where FromId=@FromId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@FDel", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 隐藏收藏数据
        /// </summary>
        public void UpdateKDel2(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("TDel=@TDel ");
            strSql.Append(" where ToId=@ToId ");
            strSql.Append(" and IsKeep=@IsKeep ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@IsKeep", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = 1;
            parameters[2].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除对话(发送)
        /// </summary>
        public void UpdateChatFDel(int UsID, int Hid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update tb_Guest set ");
            strSql.Append("FDel=@FDel ");
            strSql.Append(" where FromId=@FromId ");
            strSql.Append(" and ToId=@ToId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@FDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@ToId", SqlDbType.Int,4)};
            parameters[0].Value = 1;
            parameters[1].Value = UsID;
            parameters[2].Value = Hid;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除对话(接收)
        /// </summary>
        public void UpdateChatTDel(int UsID, int Hid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update tb_Guest set ");
            strSql.Append("TDel=@TDel ");
            strSql.Append(" where ToId=@ToId ");
            strSql.Append(" and FromId=@FromId ");
            strSql.Append(" and IsSeen=@IsSeen ");
            SqlParameter[] parameters = {
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4),
                    new SqlParameter("@IsSeen", SqlDbType.TinyInt,4)};
            parameters[0].Value = 1;
            parameters[1].Value = UsID;
            parameters[2].Value = Hid;
            parameters[3].Value = 1;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除收信箱所有消息
        /// </summary>
        public void UpdateSClear(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update tb_Guest set ");
            strSql.Append("TDel=@TDel ");
            strSql.Append(" where ToId=@ToId ");
            strSql.Append(" and FromId>@FromId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@TDel", SqlDbType.TinyInt,1),
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4)};
            parameters[0].Value = 1;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除系统消息
        /// </summary>
        public int UpdateXClear(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Guest ");
            strSql.Append(" where ToId=@ToId ");
            strSql.Append(" and FromId=@FromId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ToId", SqlDbType.Int,4),
                    new SqlParameter("@FromId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;

            return SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到Types
        /// </summary>
        public int GetTypes(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types from tb_Guest ");
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
        /// 更新为Type yzg 20161111
        /// </summary>
        public void UpdateTypes(int ID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Guest set ");
            strSql.Append("Types=@Types ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Types;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Guest ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Guest ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelperUser.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到接收ID
        /// </summary>
        public int GetToId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ToId from tb_Guest ");
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
        /// 得到是否已读
        /// </summary>
        public int GetIsSeen(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IsSeen from tb_Guest ");
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Guest GetGuest(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,FromId,FromName,ToId,ToName,Content,IsSeen,TransId,AddUsIP,AddTime from tb_Guest ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Guest model = new BCW.Model.Guest();
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetByte(1);
                    model.FromId = reader.GetInt32(2);
                    model.FromName = reader.GetString(3);
                    model.ToId = reader.GetInt32(4);
                    model.ToName = reader.GetString(5);
                    model.Content = reader.GetString(6);
                    model.IsSeen = reader.GetByte(7);
                    model.TransId = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        model.AddUsIP = reader.GetString(9);
                    model.AddTime = reader.GetDateTime(10);
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
            strSql.Append(" FROM tb_Guest ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelperUser.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuests(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Guest> listGuests = new List<BCW.Model.Guest>();
            string sTable = "tb_Guest";
            string sPkey = "id";
            string sField = "ID,FromId,FromName,ToId,ToName,Content,IsSeen,AddTime";
            string sCondition = strWhere;
            string sOrder = "AddTime Desc";
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
                    return listGuests;
                }
                while (reader.Read())
                {
                    BCW.Model.Guest objGuest = new BCW.Model.Guest();
                    objGuest.ID = reader.GetInt32(0);
                    objGuest.FromId = reader.GetInt32(1);
                    objGuest.FromName = reader.GetString(2);
                    objGuest.ToId = reader.GetInt32(3);
                    objGuest.ToName = reader.GetString(4);
                    objGuest.Content = reader.GetString(5);
                    objGuest.IsSeen = reader.GetByte(6);
                    objGuest.AddTime = reader.GetDateTime(7);
                    listGuests.Add(objGuest);
                }
            }
            return listGuests;
        }
        /// <summary>
        /// 取得每页记录Asc
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuestsAsc(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Guest> listGuests = new List<BCW.Model.Guest>();
            string sTable = "tb_Guest";
            string sPkey = "id";
            string sField = "ID,FromId,FromName,ToId,ToName,Content,IsSeen,AddTime";
            string sCondition = strWhere;
            string sOrder = "AddTime asc";
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
                    return listGuests;
                }
                while (reader.Read())
                {
                    BCW.Model.Guest objGuest = new BCW.Model.Guest();
                    objGuest.ID = reader.GetInt32(0);
                    objGuest.FromId = reader.GetInt32(1);
                    objGuest.FromName = reader.GetString(2);
                    objGuest.ToId = reader.GetInt32(3);
                    objGuest.ToName = reader.GetString(4);
                    objGuest.Content = reader.GetString(5);
                    objGuest.IsSeen = reader.GetByte(6);
                    objGuest.AddTime = reader.GetDateTime(7);
                    listGuests.Add(objGuest);
                }
            }
            return listGuests;
        }
        /// <summary>
        /// 取得每页ID集合
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ID</returns>
        public IList<BCW.Model.Guest> GetGuestsID(int p_pageIndex, int p_pageSize, string strWhere)
        {
            IList<BCW.Model.Guest> listGuests = new List<BCW.Model.Guest>();
            string sTable = "tb_Guest";
            string sPkey = "id";
            string sField = "ID";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = p_pageSize;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelperUser.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                while (reader.Read())
                {
                    BCW.Model.Guest objGuest = new BCW.Model.Guest();
                    objGuest.ID = reader.GetInt32(0);
                    listGuests.Add(objGuest);
                }
            }
            return listGuests;
        }

        /// <summary>
        /// 聊天模式分页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Guest> GetGuests(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Guest> listGuest = new List<BCW.Model.Guest>();

            // 计算记录数
            string countString = "SELECT COUNT(ID) FROM tb_Guest Where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelperUser.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listGuest;
            }

            // 取出相关记录

            string queryString = "SELECT ID,FromId,Content,IsSeen,TransId,AddTime FROM tb_Guest Where " + strWhere + " ORDER BY " + strOrder + "";

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Guest objGuest = new BCW.Model.Guest();
                        objGuest.ID = reader.GetInt32(0);
                        objGuest.FromId = reader.GetInt32(1);
                        objGuest.Content = reader.GetString(2);
                        objGuest.IsSeen = reader.GetByte(3);
                        objGuest.TransId = reader.GetInt32(4);
                        objGuest.AddTime = reader.GetDateTime(5);
                        listGuest.Add(objGuest);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listGuest;
        }

        #endregion  成员方法
    }
}
