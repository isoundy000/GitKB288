using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

/// <summary>
/// ������֧���ֶ�
/// �ƹ��� 20160611
/// </summary>
namespace BCW.DAL
{
    /// <summary>
    /// ���ݷ�����User��
    /// </summary>
    public class User
    {
        public User()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ�ǰ̨������ĳ�ʱʱ��
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
        /// ����ǰ̨������ĳ�ʱʱ��
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
        /// ���²Ƹ�/����ǰ֧��ʱ��
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
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperUser.GetMaxID("ID", "tb_User");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// �Ƿ���ڸü�¼
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
        /// �Ƿ���ڸ��ֻ��ż�¼
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
        /// �Ƿ���ڸ������¼
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
        /// �Ƿ���ڸ��û��ǳƼ�¼
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
        /// �Ƿ���ڸ��û��ǳƼ�¼
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
        /// �Ƿ���ڸ��ֻ��ź����䣨�һ����룩
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
        /// ����ID�������ѯӰ�������
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
        /// �����ֻ��ź������ѯӰ�������
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
        /// �õ�ĳ��̳����������
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
        /// �õ�ĳȦ�ӵ���������
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
        /// �õ�ĳ�����ҵ���������
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
        /// �õ�����������������
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
        /// �õ���������������
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
        /// �õ���������
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
        /// �õ���Ա����
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
        /// �õ���������
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
        /// �õ��Ƿ������ID(0��/1��)
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
        /// �õ����һ��������ID
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
        /// ����һ������
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
            parameters[21].Value = "����|0,����|0,�ǻ�|0,����|0,а��|0";
            parameters[22].Value = model.InviteNum;
            parameters[23].Value = "�����б�����|8,������������|500,�����б�����|8,�����б���������|8,���ӻ���|0,���ӻ�������|5,����ͼƬ|0,����ͼƬ|0,�������|0,���п���|0,����ȡϢʱ��|" + DateTime.Now + ",������֤|0,����Ⱥ��|0,Ȧ��Ⱥ��|0,�Ƽ�����|0,ϵͳ��Ϣ|0,��Ϸϵͳ��Ϣ|0,�Ǻ�����Ϣ|0,δ��֤�ֻ���|0,����Ȩ��|0,����֪ͨ|0,Ȧ������|0,֧��ʱ��|" + DateTime.Now + ",֧����ʱ|0,�Ʋ���ʾ|0,IP�䶯ʱ��|" + DateTime.Now + ",IP�䶯��ʱ|0,˽������|0,ϵͳ��������|0,����䶯ʱ��|" + DateTime.Now + ",����ˢ������|60,�����б�������|10,�龰|-1,��������|0";
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
        /// ����һ������
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
        /// �޸Ļ�������
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
        ///// ��������Ϣ����
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
        ///// ��������Ϣ����
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
        /// �������ʱ��/IP
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
        /// ��������ʱ��
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
        /// �����㼣
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

        //��ѯ�Ƿ�Ϊϵͳ��
        //�۹��� 20161116
        public bool isIsSpier(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User WHERE ID=" + ID + " AND IsSpier=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
        //���һ�����ǻ����˵�¼��IP
        //�۹��� 20161116
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
        /// ��������ʱ��
        /// //�۹��� 20161116
        /// </summary>
        public void UpdateTime(int ID, int OnTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_User set ");
            strSql.Append("EndTime=@EndTime, ");
            strSql.Append("OnTime=OnTime+@OnTime ");
            //�ж�ID�Ƿ�Ϊ�����˲����ֻ�����Ϊ1510758��ͷ��
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
            if (isIsSpier(ID))//������
                parameters[3].Value = EndIP;
            else
                parameters[3].Value = Utils.GetUsIP();
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        ///// <summary>
        ///// ��������ʱ�䡪��������
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
        /// �������������̳ID
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
        /// �����������������ID
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
        /// ���������������ID
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
        /// �����û������
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
        /// �����û�����
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
        /// �����û����б�
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
        /// ��������Ԫ
        /// </summary>
        public void UpdateiMoney(int ID, long iMoney)
        {
            //Utils.Error("" + ub.Get("SiteBz2") + "��ͣʹ�ã�δ��" + ub.Get("SiteBz2") + "�һ���" + ub.Get("SiteBz") + "�ģ�ϵͳ��11��20���Զ������һ������ռ��������<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=exchange") + "\">" + ub.Get("SiteBz2") + "�һ�" + ub.Get("SiteBz") + "</a>,�һ�ʱ�䵽2012-11-19��24��ֹ", "");

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
        /// �����ƹ�ӵ��
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
        /// �����ǳ�
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
        /// �����ֻ���
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
        /// ����139��������
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
        /// ���¸���ǩ��
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
        /// ���µ�¼����
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
        /// ���µ�¼����
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
        /// �����û��ܳ�
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
        /// ����֧������
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
        /// ����֧������
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
        /// ���¹�������
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
        /// ����ͷ��
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
        /// �����ֶ��޸������б� �۹��� 20161107 
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
        /// ����״̬
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
        /// ��������
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
        /// ���²Ƹ�/����ǰ֧������0ѡ��/1�Ƹ�/2����
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
        /// ���µȼ�����
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
        /// ��������
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
        /// ���¸�������
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
        /// ���¸�����ʷ
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
        /// ����Ȧ��ID
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
        /// ������֤��
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
        /// �����Ƽ���ID
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
        /// ����Ϊ��֤��Ա
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
        /// �����ԱID
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
        /// ����ǩ����Ϣ
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
        /// ����VIP��Ϣ
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
        /// ����VIP��Ϣ
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
        /// ����VIP�ɳ���
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
        /// ����Ȩ��
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
        /// ����UsUbb
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
        /// ɾ��һ������
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
        /// �õ��û�UsKey/UsPwd
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
        /// �õ��û�UsKey/UsPwd
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
        /// �õ��ֻ���
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
        /// �õ��ֻ���
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
        /// �õ���¼����
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
        /// �õ���¼����
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
        /// �õ�֧������
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
        /// �õ�֧������
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
        /// �õ���������
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
        /// �õ����IP/���ʱ��
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
        /// �õ�����֧����ʱ��
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
        /// �õ��û���
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
        /// �õ�Money
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
        /// �õ��û����б�
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
        /// �õ��û��ȼ�
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
        /// �õ��û�֧������ 1�Ƹ�  2����
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
        /// �õ��û��ǳ�
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
        /// �õ��˻���֧�����
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
        /// �����˻���֧�����
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
        /// �õ��û�ǩ��
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
        /// �õ��û�״̬
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
        /// �õ�����
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
                        return "����|0,����|0,�ǻ�|0,����|0,а��|0";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// �õ���������
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
        /// �õ�139�ֻ�����
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
        /// �õ�����UBB���
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
        /// �õ�������ʷ
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
        /// �õ��㼣
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
        /// �õ������Ȧ��ID
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
        /// �õ���֤��
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
        /// �õ�ͷ��
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
        ///// �õ�����Ϣ����
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
        /// �õ��Ƽ��Լ���ID
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
        /// �õ��ƹ�ӵ��
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
        /// �õ�Ȩ��
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
        /// �õ��Ƿ�����֤(0δ��֤/1����֤)
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
        /// �õ��ʻ��Ƿ��Ѷ���
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
        /// �õ��û��Ա�
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
        /// �õ������̳ID
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
        /// �õ����������ID
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
        /// �õ��������ID
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
        /// �õ�ǩ����Ϣ
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
        /// �õ�VIP��Ϣ
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
        /// �õ������û��ǳ���ʾ�ı�ʶ
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
        /// �õ��û�������Ϣ
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
        /// �õ��޸ĵĻ�����Ϣ
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
        /// �õ����ߵĻ�����Ϣ
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
        /// �õ��һ�����Ļ�����Ϣ
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
        /// �����ֶ�ȡ�����б�
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
        /// ȡ��ÿҳ��¼������ҳ��ʹ�ã�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">���з�ʽ</param>
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
                //������ҳ��
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
        /// ȡ��ÿҳ��¼������/����ҳ��ʹ�ã�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
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
                //������ҳ��
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
        /// ��Ա���а�ʹ��
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
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
                //������ҳ��
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
        /// �Ƽ���Ա���а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.User> GetInvites(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.User> listUser = new List<BCW.Model.User>();

            //// �����¼��
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

            // ȡ����ؼ�¼

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

        #endregion  ��Ա����
    }
}
