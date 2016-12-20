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
    /// 数据访问类Chat。
    /// </summary>
    public class Chat
    {
        public Chat()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Chat");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Chat");
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
            strSql.Append("select count(1) from tb_Chat");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ExTime>=@ExTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Chat");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ExTime>=@ExTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 存在几条记录
        ///  2016/3/1 戴少宇
        /// </summary>
        public int Exists4(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Chat");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ExTime>=@ExTime ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = DateTime.Now;
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Chat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Chat(");
            strSql.Append("Types,ChatName,ChatNotes,ChatSZ,ChatJS,ChatLG,ChatFoot,ChatCent,ChatOnLine,ChatTopLine,ChatScore,UsID,GroupId,ChatCon,Paixu,AddTime,ExTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@ChatName,@ChatNotes,@ChatSZ,@ChatJS,@ChatLG,@ChatFoot,@ChatCent,@ChatOnLine,@ChatTopLine,@ChatScore,@UsID,@GroupId,@ChatCon,@Paixu,@AddTime,@ExTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ChatName", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatNotes", SqlDbType.NVarChar,200),
					new SqlParameter("@ChatSZ", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatJS", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatLG", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatFoot", SqlDbType.NText),
					new SqlParameter("@ChatCent", SqlDbType.BigInt,8),
					new SqlParameter("@ChatOnLine", SqlDbType.Int,4),
					new SqlParameter("@ChatTopLine", SqlDbType.Int,4),
					new SqlParameter("@ChatScore", SqlDbType.Money,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@ChatCon", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.ChatName;
            parameters[2].Value = model.ChatNotes;
            parameters[3].Value = model.ChatSZ;
            parameters[4].Value = model.ChatJS;
            parameters[5].Value = model.ChatLG;
            parameters[6].Value = model.ChatFoot;
            parameters[7].Value = 0;
            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = model.UsID;
            parameters[12].Value = model.GroupId;
            parameters[13].Value = 0;
            parameters[14].Value = model.Paixu;
            parameters[15].Value = model.AddTime;
            parameters[16].Value = model.ExTime;

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
        public void Update(BCW.Model.Chat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("Types=@Types,");
            strSql.Append("ChatName=@ChatName,");
            strSql.Append("ChatNotes=@ChatNotes,");
            strSql.Append("ChatSZ=@ChatSZ,");
            strSql.Append("ChatJS=@ChatJS,");
            strSql.Append("ChatLG=@ChatLG,");
            strSql.Append("ChatFoot=@ChatFoot,");
            strSql.Append("ChatCent=@ChatCent,");
            strSql.Append("CentPwd=@CentPwd,");
            strSql.Append("ChatTopLine=@ChatTopLine,");
            strSql.Append("ChatScore=@ChatScore,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("GroupId=@GroupId,");
            strSql.Append("ChatPwd=@ChatPwd,");
            strSql.Append("ChatCT=@ChatCT,");
            strSql.Append("ChatCbig=@ChatCbig,");
            strSql.Append("ChatCsmall=@ChatCsmall,");
            strSql.Append("ChatCId=@ChatCId,");
            strSql.Append("ChatCon=@ChatCon,");
            strSql.Append("Paixu=@Paixu,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("ExTime=@ExTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ChatName", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatNotes", SqlDbType.NVarChar,200),
					new SqlParameter("@ChatSZ", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatJS", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatLG", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatFoot", SqlDbType.NText),
					new SqlParameter("@ChatCent", SqlDbType.BigInt,8),
					new SqlParameter("@CentPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatTopLine", SqlDbType.Int,4),
					new SqlParameter("@ChatScore", SqlDbType.Money,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@ChatPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCT", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCbig", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCsmall", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCId", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatCon", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.ChatName;
            parameters[3].Value = model.ChatNotes;
            parameters[4].Value = model.ChatSZ;
            parameters[5].Value = model.ChatJS;
            parameters[6].Value = model.ChatLG;
            parameters[7].Value = model.ChatFoot;
            parameters[8].Value = model.ChatCent;
            parameters[9].Value = model.CentPwd;
            parameters[10].Value = model.ChatTopLine;
            parameters[11].Value = model.ChatScore;
            parameters[12].Value = model.UsID;
            parameters[13].Value = model.GroupId;
            parameters[14].Value = model.ChatPwd;
            parameters[15].Value = model.ChatCT;
            parameters[16].Value = model.ChatCbig;
            parameters[17].Value = model.ChatCsmall;
            parameters[18].Value = model.ChatCId;
            parameters[19].Value = model.ChatCon;
            parameters[20].Value = model.Paixu;
            parameters[21].Value = model.AddTime;
            parameters[22].Value = model.ExTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新抢币设置
        /// </summary>
        public void UpdateCb(BCW.Model.Chat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatCT=@ChatCT,");
            strSql.Append("ChatCbig=@ChatCbig,");
            strSql.Append("ChatCsmall=@ChatCsmall,");
            strSql.Append("ChatCId=@ChatCId,");
            strSql.Append("ChatCon=@ChatCon");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatCT", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCbig", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCsmall", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatCId", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatCon", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ChatCT;
            parameters[2].Value = model.ChatCbig;
            parameters[3].Value = model.ChatCsmall;
            parameters[4].Value = model.ChatCId;
            parameters[5].Value = model.ChatCon;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateBasic(BCW.Model.Chat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatName=@ChatName,");
            strSql.Append("ChatNotes=@ChatNotes,");
            strSql.Append("ChatFoot=@ChatFoot");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatName", SqlDbType.NVarChar,50),
					new SqlParameter("@ChatNotes", SqlDbType.NVarChar,200),
					new SqlParameter("@ChatFoot", SqlDbType.NText)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ChatName;
            parameters[2].Value = model.ChatNotes;
            parameters[3].Value = model.ChatFoot;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateAdmin(BCW.Model.Chat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatSZ=@ChatSZ,");
            strSql.Append("ChatJS=@ChatJS,");
            strSql.Append("ChatLG=@ChatLG");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatSZ", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatJS", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatLG", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ChatSZ;
            parameters[2].Value = model.ChatJS;
            parameters[3].Value = model.ChatLG;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateAdmin2(BCW.Model.Chat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatJS=@ChatJS,");
            strSql.Append("ChatLG=@ChatLG");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatJS", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatLG", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ChatJS;
            parameters[2].Value = model.ChatLG;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新基金数目
        /// </summary>
        public void UpdateChatCent(int ID, long ChatCent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatCent=ChatCent+@ChatCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatCent", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新基金密码
        /// </summary>
        public void UpdateCentPwd(int ID, string CentPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("CentPwd=@CentPwd");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@CentPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = CentPwd;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新在线人数
        /// </summary>
        public void UpdateLine(int ID, int Line)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatOnLine=@ChatOnLine,");
            strSql.Append("ChatTopLine=");
            strSql.Append("case when ChatTopLine<" + Line + " then " + Line + " else ChatTopLine+0 END");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatOnLine", SqlDbType.Int,4)};
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
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatOnLine=ChatOnLine-1");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ChatOnLine>@ChatOnLine ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatOnLine", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新聊室密码
        /// </summary>
        public void UpdateChatPwd(int ID, string ChatPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatPwd=@ChatPwd");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatPwd;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新使用密码进入的ID
        /// </summary>
        public void UpdatePwdID(int ID, string PwdID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("PwdID=@PwdID");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PwdID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = PwdID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新过期时间(续费)
        /// </summary>
        public void UpdateExitTime(int ID, DateTime ExTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
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
        /// 更新抢币结束时间
        /// </summary>
        public void UpdateChatCTime(int ID, DateTime ChatCTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatCTime=@ChatCTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatCTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatCTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新抢币购买ID
        /// </summary>
        public void UpdateChatCId(int ID, string ChatCId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatCId=@ChatCId");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatCId", SqlDbType.NVarChar,500)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatCId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 重置抢币
        /// </summary>
        public void UpdateCb(int ID, string ChatCId, DateTime ChatCTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Chat set ");
            strSql.Append("ChatCId=@ChatCId,");
            strSql.Append("ChatCTime=@ChatCTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ChatCId", SqlDbType.NVarChar,500),
					new SqlParameter("@ChatCTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ChatCId;
            parameters[2].Value = ChatCTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Chat ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到聊天室密码
        /// </summary>
        public string GetChatPwd(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ChatPwd from tb_Chat ");
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
        /// 得到聊天室名称
        /// </summary>
        public string GetChatName(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ChatName from tb_Chat ");
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
        /// 得到聊天室类型
        /// </summary>
        public int GetChatType(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types from tb_Chat ");
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
        public BCW.Model.Chat GetChatBasic(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types,ChatName,ExTime from tb_Chat ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Chat model = new BCW.Model.Chat();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Types = reader.GetInt32(0);
                    model.ChatName = reader.GetString(1);
                    model.ExTime = reader.GetDateTime(2);
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
        public BCW.Model.Chat GetChat(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,ChatName,ChatNotes,ChatSZ,ChatJS,ChatLG,ChatFoot,ChatCent,CentPwd,ChatOnLine,ChatTopLine,ChatScore,UsID,GroupId,ChatPwd,PwdID,ChatCT,ChatCbig,ChatCsmall,ChatCId,ChatCon,ChatCTime,Paixu,AddTime,ExTime from tb_Chat ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Chat model = new BCW.Model.Chat();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.ChatName = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.ChatNotes = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.ChatSZ = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        model.ChatJS = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        model.ChatLG = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        model.ChatFoot = reader.GetString(7);
                    model.ChatCent = reader.GetInt64(8);
                    if (!reader.IsDBNull(9))
                        model.CentPwd = reader.GetString(9);
                    model.ChatOnLine = reader.GetInt32(10);
                    model.ChatTopLine = reader.GetInt32(11);
                    model.ChatScore = reader.GetDecimal(12);
                    model.UsID = reader.GetInt32(13);
                    model.GroupId = reader.GetInt32(14);
                    if (!reader.IsDBNull(15))
                        model.ChatPwd = reader.GetString(15);
                    if (!reader.IsDBNull(16))
                        model.PwdID = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                        model.ChatCT = reader.GetString(17);
                    if (!reader.IsDBNull(18))
                        model.ChatCbig = reader.GetString(18);
                    if (!reader.IsDBNull(19))
                        model.ChatCsmall = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                        model.ChatCId = reader.GetString(20);
                    model.ChatCon = reader.GetInt32(21);
                    if (!reader.IsDBNull(22))
                        model.ChatCTime = reader.GetDateTime(22);
                    else
                        model.ChatCTime = Convert.ToDateTime("1990-1-1");

                    model.Paixu = reader.GetInt32(23);
                    model.AddTime = reader.GetDateTime(24);
                    model.ExTime = reader.GetDateTime(25);
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
            strSql.Append(" FROM tb_Chat ");
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
        /// <returns>IList Chat</returns>
        public IList<BCW.Model.Chat> GetChats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Chat> listChats = new List<BCW.Model.Chat>();
            string sTable = "tb_Chat";
            string sPkey = "id";
            string sField = "ID,ChatName,ChatOnLine,ChatTopLine,ChatScore,Paixu,Types,ExTime";
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
                    return listChats;
                }
                while (reader.Read())
                {
                    BCW.Model.Chat objChat = new BCW.Model.Chat();
                    objChat.ID = reader.GetInt32(0);
                    objChat.ChatName = reader.GetString(1);
                    objChat.ChatOnLine = reader.GetInt32(2);
                    objChat.ChatTopLine = reader.GetInt32(3);
                    objChat.ChatScore = reader.GetDecimal(4);
                    objChat.Paixu = reader.GetInt32(5);
                    objChat.Types = reader.GetInt32(6);
                    objChat.ExTime = reader.GetDateTime(7);
                    listChats.Add(objChat);
                }
            }
            return listChats;
        }

        #endregion  成员方法
    }
}

