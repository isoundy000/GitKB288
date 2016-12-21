using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���User ��ժҪ˵����
    
    /// ������֧���ֶ�
    /// �ƹ��� 20160611
    
    /// ��ӻ�Ծ�齱���201605Ҧ־��
    /// 
    /// ���ӵ�ֵ�齱��ڣ�������ѣ� 20160823 ���ڽ�
   /// </summary>
    public class User
    {
        private readonly BCW.DAL.User dal = new BCW.DAL.User();
        public User()
        { }
        #region  ��Ա����
        /// <summary>
        /// �õ�ǰ̨������ĳ�ʱʱ��
        /// </summary>
        public DateTime GetManAcTime(int ID)
        {
            return dal.GetManAcTime(ID);
        }
        /// <summary>
        /// ����ǰ̨������ĳ�ʱʱ��
        /// </summary>
        public void UpdateManAcTime(int ID, DateTime ManAcTime)
        {
            dal.UpdateManAcTime(ID, ManAcTime);
        }
         /// <summary>
        /// ���²Ƹ�/����ǰ֧������0ѡ��/1�Ƹ�/2����
        /// </summary>
        public void UpdatePayType(int ID, int PayType)
        {
            dal.UpdatePayType(ID, PayType);
        }
         /// <summary>
        /// ���²Ƹ�/����ǰ֧��ʱ��
        /// </summary>
        public void UpdateTimeLimit(int ID, DateTime TimeLimit)
        {
            dal.UpdateTimeLimit(ID, TimeLimit);
        }
        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsID(long ID)
        {
            return dal.ExistsID(ID);
        }
        /// <summary>
        /// �Ƿ���ڸ��ֻ��ż�¼
        /// </summary>
        public bool Exists(string Mobile)
        {
            return dal.Exists(Mobile);
        }

        /// <summary>
        /// �Ƿ���ڸ������¼
        /// </summary>
        public bool ExistsEmail(string Email)
        {
            return dal.ExistsEmail(Email);
        }

        /// <summary>
        /// �Ƿ���ڸ��û��ǳƼ�¼
        /// </summary>
        public bool ExistsUsName(string UsName)
        {
            return dal.ExistsUsName(UsName);
        }

        /// <summary>
        /// �Ƿ���ڸ��û��ǳƼ�¼
        /// </summary>
        public bool ExistsUsName(string UsName, int ID)
        {
            return dal.ExistsUsName(UsName, ID);
        }

        /// <summary>
        /// �Ƿ���ڸ��ֻ��ź����䣨�һ����룩
        /// </summary>
        public bool Exists(string Mobile, string Email)
        {
            return dal.Exists(Mobile, Email);
        }

        /// <summary>
        /// ����ID�������ѯӰ�������
        /// </summary>
        public int GetRowByID(BCW.Model.User model)
        {
            return dal.GetRowByID(model);
        }
        /// <summary>
        /// �����ֻ��ź������ѯӰ�������
        /// </summary>
        public int GetRowByMobile(BCW.Model.User model)
        {
            return dal.GetRowByMobile(model);
        }

        /// <summary>
        /// �õ�ĳ��̳����������
        /// </summary>
        public int GetForumNum(int ForumID)
        {
            return dal.GetForumNum(ForumID);
        }

        /// <summary>
        /// �õ�ĳȦ�ӵ���������
        /// </summary>
        public int GetGroupNum(int GroupId)
        {
            return dal.GetGroupNum(GroupId);
        }

        /// <summary>
        /// �õ�ĳ�����ҵ���������
        /// </summary>
        public int GetChatNum(int ChatID)
        {
            return dal.GetChatNum(ChatID);
        }

        /// <summary>
        /// �õ�����������������
        /// </summary>
        public int GetChatNum()
        {
            return dal.GetChatNum();
        }

        /// <summary>
        /// �õ���������������
        /// </summary>
        public int GetSpeakNum()
        {
            return dal.GetSpeakNum();
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetNum(int Types)
        {
            return dal.GetNum(Types);
        }

        /// <summary>
        /// �õ���Ա����
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetClickTop(int ID)
        {
            return dal.GetClickTop(ID);
        }

        /// <summary>
        /// �õ��Ƿ������ID(0��/1��)
        /// </summary>
        public int GetIsSpier(int ID)
        {
            return dal.GetIsSpier(ID);
        }

        /// <summary>
        /// �õ����һ��������ID
        /// </summary>
        public int GetIsSpierRandID()
        {
            return dal.GetIsSpierRandID();
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.User model)
        {
            dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = model.ID;
                string username = model.UsName; ;
                string Notes = "��ע���Ա";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.User model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// �޸Ļ�������
        /// </summary>
        public void UpdateEditBasic(BCW.Model.User model)
        {
            dal.UpdateEditBasic(model);
        }

        ///// <summary>
        ///// ��������Ϣ����
        ///// </summary>
        //public void UpdateGutNum(int ID)
        //{
        //    dal.UpdateGutNum(ID);
        //}

        ///// <summary>
        ///// ��������Ϣ����
        ///// </summary>
        //public void UpdateGutNum(int ID, int GutNum)
        //{
        //    dal.UpdateGutNum(ID, GutNum);
        //}

        /// <summary>
        /// �������ʱ��/IP
        /// </summary>
        public void UpdateIpTime(int ID)
        {
            dal.UpdateIpTime(ID);
        }

        /// <summary>
        /// ��������ʱ��
        /// </summary>
        public void UpdateTime(int ID)
        {
            dal.UpdateTime(ID);
        }

        /// <summary>
        /// �����㼣
        /// </summary>
        public void UpdateVisitHy(int ID, string VisitHy)
        {
            dal.UpdateVisitHy(ID, VisitHy);
        }

        /// <summary>
        /// ��������ʱ��
        /// </summary>
        public void UpdateTime(int ID, int OnTime)
        {
            dal.UpdateTime(ID, OnTime);
        }

        /// <summary>
        /// �������������̳ID
        /// </summary>
        public void UpdateEndForumID(int ID, int ForumID)
        {
            dal.UpdateEndForumID(ID, ForumID);
        }

        /// <summary>
        /// �����������������ID
        /// </summary>
        public void UpdateEndChatID(int ID, int ChatID)
        {
            dal.UpdateEndChatID(ID, ChatID);
        }

        /// <summary>
        /// ���������������ID
        /// </summary>
        public void UpdateEndSpeakID(int ID, int SpeakID)
        {
            dal.UpdateEndSpeakID(ID, SpeakID);
        }

        /// <summary>
        /// �����ǳ�
        /// </summary>
        public void UpdateUsName(int ID, string UsName)
        {
            dal.UpdateUsName(ID, UsName);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username = UsName;
                string Notes = "�ռ������ǳ�";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// �����ֻ���
        /// </summary>
        public void UpdateMobile(int ID, string Mobile)
        {
            dal.UpdateMobile(ID, Mobile);
        }

        /// <summary>
        /// ����139��������
        /// </summary>
        public void UpdateSmsEmail(int ID, string SmsEmail)
        {
            dal.UpdateSmsEmail(ID, SmsEmail);
        }

        /// <summary>
        /// ���¸���ǩ��
        /// </summary>
        public void UpdateSign(int ID, string Sign)
        {
            dal.UpdateSign(ID, Sign);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username = dal.GetUsName(ID);
                string Notes = "�ռ����ø���ǩ��";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ���µ�¼����
        /// </summary>
        public void UpdateUsPwd(int ID, string UsPwd)
        {
            dal.UpdateUsPwd(ID, UsPwd);
        }

        /// <summary>
        /// ���µ�¼����
        /// </summary>
        public void UpdateUsPwd(string Mobile, string UsPwd)
        {
            dal.UpdateUsPwd(Mobile, UsPwd);
        }

        /// <summary>
        /// �����û��ܳ�
        /// </summary>
        public void UpdateUsKey(int ID, string UsKey)
        {
            dal.UpdateUsKey(ID, UsKey);
        }

        /// <summary>
        /// ����֧������
        /// </summary>
        public void UpdateUsPled(int ID, string UsPled)
        {
            dal.UpdateUsPled(ID, UsPled);
        }

        /// <summary>
        /// ����֧������
        /// </summary>
        public void UpdateUsPled(string Mobile, string UsPled)
        {
            dal.UpdateUsPled(Mobile, UsPled);
        }

        /// <summary>
        /// ���¹�������
        /// </summary>
        public void UpdateUsAdmin(int ID, string UsAdmin)
        {
            dal.UpdateUsAdmin(ID, UsAdmin);
        }

        /// <summary>
        /// ���µ�¼����
        /// </summary>
        public void UpdatePhoto(int ID, string Photo)
        {
            dal.UpdatePhoto(ID, Photo);
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username = dal.GetUsName(ID);
                string Notes = "�ռ����ø���״̬";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateClick(int ID, int Click)
        {
            dal.UpdateClick(ID, Click);
        }

        /// <summary>
        /// ���������ȼ�
        /// </summary>
        public void UpdateLeven(int ID, int Leven, long iScore)
        {
            dal.UpdateLeven(ID, Leven, iScore);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "����";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// ����VIP��Ϣ
        /// </summary>
        public void UpdateVipData(int ID, int VipDayGrow, DateTime VipDate)
        {
            dal.UpdateVipData(ID, VipDayGrow, VipDate); 
             try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid =ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "VIP";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ����VIP��Ϣ
        /// ����vip���뵽��Ծ�齱���--Ҧ־��
        /// </summary>
        public void UpdateVipData(int ID, int VipDayGrow, DateTime VipDate, int VipGrow)
        {
            dal.UpdateVipData(ID, VipDayGrow, VipDate, VipGrow);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "VIP";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// ����VIP�ɳ���
        /// </summary>
        public void UpdateVipGrow(int ID, int VipGrow)
        {
            dal.UpdateVipGrow(ID, VipGrow);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateParas(int ID, string Paras)
        {
            dal.UpdateParas(ID, Paras);
        }

        /// <summary>
        /// ���¸�������
        /// </summary>
        public void UpdateForumSet(int ID, string ForumSet)
        {
            dal.UpdateForumSet(ID, ForumSet);
        }

        /// <summary>
        /// ���¸�����ʷ
        /// </summary>
        public void UpdateCopytemp(int ID, string Copytemp)
        {
            dal.UpdateCopytemp(ID, Copytemp);
        }

        /// <summary>
        /// ����Ȧ��ID
        /// </summary>
        public void UpdateGroupId(int ID, string GroupId)
        {
            dal.UpdateGroupId(ID, GroupId);
        }

        /// <summary>
        /// ������֤��
        /// </summary>
        public void UpdateVerifys(int ID, string Verifys)
        {
            dal.UpdateVerifys(ID, Verifys);
        }


        /// <summary>
        /// �����Ƽ���ID
        /// </summary>
        public void UpdateInviteNum(int id, int InviteNum)
        {
            dal.UpdateInviteNum(id, InviteNum);
        }
        /// <summary>
        /// ����Ϊ��֤��Ա
        /// </summary>
        public void UpdateIsVerify(string Mobile, int IsVerify)
        {
            dal.UpdateIsVerify(Mobile, IsVerify);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = dal.GetID(Mobile);
                string username = dal.GetUsName(usid); ;
                string Notes = "��֤";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }

        }

        /// <summary>
        /// �����ԱID
        /// </summary>
        public void UpdateIsFreeze(int ID, int IsFreeze)
        {
            dal.UpdateIsFreeze(ID, IsFreeze);
        }

        /// <summary>
        /// ����ǩ����Ϣ
        /// </summary>
        public void UpdateSingData(int ID, int SignTotal, int SignKeep)
        {
            dal.UpdateSingData(ID, SignTotal, SignKeep);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "�ռ�ǩ��";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ����Ȩ��
        /// </summary>
        public void UpdateLimit(int ID, string Limit)
        {
            dal.UpdateLimit(ID, Limit);
        }

        /// <summary>
        /// ����UsUbb
        /// </summary>
        public void UpdateUsUbb(int ID, string UsUbb)
        {
            dal.UpdateUsUbb(ID, UsUbb);
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        public void UpdateiScore(int ID, long iScore)
        {
            dal.UpdateiScore(ID, iScore);
        }

        /// <summary>
        /// �����ƹ�ӵ��
        /// </summary>
        public void UpdateiFcGold(int ID, long iFcGold)
        {
            dal.UpdateiFcGold(ID, iFcGold);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = ID;
                string username =dal.GetUsName(ID);
                string Notes = "�ƹ�";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// �õ�ID
        /// </summary>
        public int GetID(string Mobile)
        {
            return dal.GetID(Mobile);
        }

        /// <summary>
        /// �õ��ֻ���
        /// </summary>
        public string GetMobile(int ID)
        {
            return dal.GetMobile(ID);
        }

        /// <summary>
        /// �õ���¼����
        /// </summary>
        public string GetUsPwd(int ID)
        {
            return dal.GetUsPwd(ID);
        }
        /// <summary>
        /// �õ���¼����
        /// </summary>
        public string GetUsPwd(int ID, string Mobile)
        {
            return dal.GetUsPwd(ID, Mobile);
        }


        /// <summary>
        /// �õ����IP
        /// </summary>
        public BCW.Model.User GetEndIpTime(int ID)
        {
            return dal.GetEndIpTime(ID);
        }

        /// <summary>
        /// �õ�֧������
        /// </summary>
        public string GetUsPled(int ID)
        {
            return dal.GetUsPled(ID);
        }

        /// <summary>
        /// �õ�֧������
        /// </summary>
        public string GetUsPled(int ID, string Mobile)
        {
            return dal.GetUsPled(ID, Mobile);
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public string GetUsAdmin(int ID)
        {
            return dal.GetUsAdmin(ID);
        }

        /// <summary>
        /// �õ�����
        /// </summary>
        public string GetParas(int ID)
        {
            return dal.GetParas(ID);
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public string GetForumSet(int ID)
        {
            return dal.GetForumSet(ID);
        }

        /// <summary>
        /// �õ�139�ֻ�����
        /// </summary>
        public string GetSmsEmail(int ID)
        {
            return dal.GetSmsEmail(ID);
        }

        /// <summary>
        /// �õ�����UBB���
        /// </summary>
        public string GetUsUbb(int ID)
        {
            return dal.GetUsUbb(ID);
        }

        /// <summary>
        /// �õ�������ʷ
        /// </summary>
        public string GetCopytemp(int ID)
        {
            return dal.GetCopytemp(ID);
        }

        /// <summary>
        /// �õ��㼣
        /// </summary>
        public string GetVisitHy(int ID)
        {
            return dal.GetVisitHy(ID);
        }

        /// <summary>
        /// �õ������Ȧ��ID
        /// </summary>
        public string GetGroupId(int ID)
        {
            return dal.GetGroupId(ID);
        }

        /// <summary>
        /// �õ���֤��
        /// </summary>
        public string GetVerifys(int ID)
        {
            return dal.GetVerifys(ID);
        }

        /// <summary>
        /// �õ�ͷ��
        /// </summary>
        public string GetPhoto(int ID)
        {
            return dal.GetPhoto(ID);
        }

        ///// <summary>
        ///// �õ�����Ϣ����
        ///// </summary>
        //public int GetGutNum(int ID)
        //{
        //    return dal.GetGutNum(ID);
        //}

        /// <summary>
        /// �õ��Ƽ��Լ���ID
        /// </summary>
        public int GetInviteNum(int ID)
        {
            return dal.GetInviteNum(ID);
        }

        /// <summary>
        /// �õ��ƹ�ӵ��
        /// </summary>
        public long GetFcGold(int ID)
        {
            return dal.GetFcGold(ID);
        }

        /// <summary>
        /// �õ�Ȩ��
        /// </summary>
        public string GetLimit(int ID)
        {
            return dal.GetLimit(ID);
        }

        /// <summary>
        /// �õ��Ƿ�����֤(0δ��֤/1����֤)
        /// </summary>
        public int GetIsVerify(int ID)
        {
            return dal.GetIsVerify(ID);
        }

        /// <summary>
        /// �õ��ʻ��Ƿ��Ѷ���
        /// </summary>
        public int GetIsFreeze(int ID)
        {
            return dal.GetIsFreeze(ID);
        }

        /// <summary>
        /// �õ�Ȩ��ʵ�壬�ӻ����С�
        /// </summary>
        public string GetLimitByCache(int ID)
        {
            string CacheKey = CacheName.App_LimitModel(ID);
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetLimit(ID);
                    if (objModel != null)
                    {
                        int ModelCache = CacheTime.LimitExpir;
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return objModel.ToString();
        }

        /// <summary>
        /// �õ��û���
        /// </summary>
        public long GetGold(int ID)
        {
            return dal.GetGold(ID);
        }
         /// <summary>
        /// �õ�����֧����ʱ��
        /// </summary>
        public BCW.Model.User GetTimeLimit(int ID)
        {
           return  dal.GetTimeLimit(ID);
        }
         /// <summary>
        /// �õ��û�֧������ 1�Ƹ�  2����
        /// </summary>
        public int GetPayType(int ID)
        {
            return dal.GetPayType(ID);
        }
        /// <summary>
        /// �õ��û�Ԫ
        /// </summary>
        public long GetMoney(int ID)
        {
            return dal.GetMoney(ID);
        }

        /// <summary>
        /// �õ��û����б�
        /// </summary>
        public long GetBank(int ID)
        {
            return dal.GetBank(ID);
        }

        /// <summary>
        /// �õ��û��ȼ�
        /// </summary>
        public int GetLeven(int ID)
        {
            return dal.GetLeven(ID);
        }

        /// <summary>
        /// �õ��û��ǳ�
        /// </summary>
        public string GetUsName(int ID)
        {
            return dal.GetUsName(ID);
        }

        /// <summary>
        /// �õ��˻���֧�����
        /// </summary>
        public string GetUsISGive(int ID)
        {
            return dal.GetUsISGive(ID);
        }

        /// <summary>
        /// �����˻���֧�����
        /// </summary>
        public void SetUsISGive(int ID, double ISGive)
        {
            dal.SetUsISGive(ID, ISGive);
        }

        /// <summary>
        /// �õ��û�ǩ��
        /// </summary>
        public string GetSign(int ID)
        {
            return dal.GetSign(ID);
        }

        /// <summary>
        /// �õ��û�״̬
        /// </summary>
        public int GetState(int ID)
        {
            return dal.GetState(ID);
        }

        /// <summary>
        /// �õ��û��Ա�
        /// </summary>
        public int GetSex(int ID)
        {
            return dal.GetSex(ID);
        }

        /// <summary>
        /// �õ������̳ID
        /// </summary>
        public int GetEndForumID(int ID)
        {
            return dal.GetEndForumID(ID);
        }

        /// <summary>
        /// �õ����������ID
        /// </summary>
        public int GetEndChatID(int ID)
        {
            return dal.GetEndChatID(ID);
        }

        /// <summary>
        /// �õ��������ID
        /// </summary>
        public int GetEndSpeakID(int ID)
        {
            return dal.GetEndSpeakID(ID);
        }

        /// <summary>
        /// �õ�ǩ����Ϣ
        /// </summary>
        public BCW.Model.User GetSignData(int ID)
        {
            return dal.GetSignData(ID);
        }

        /// <summary>
        /// �õ�VIP��Ϣ
        /// </summary>
        public BCW.Model.User GetVipData(int ID)
        {
            return dal.GetVipData(ID);
        }

        /// <summary>
        /// �õ������û��ǳ���ʾ�ı�ʶ
        /// </summary>
        public BCW.Model.User GetShowName(int ID)
        {
            return dal.GetShowName(ID);
        }

        /// <summary>
        /// �õ��û�������Ϣ
        /// </summary>
        public BCW.Model.User GetBasic(int ID)
        {
            return dal.GetBasic(ID);
        }

        /// <summary>
        /// �õ��޸ĵĻ�����Ϣ
        /// </summary>
        public BCW.Model.User GetEditBasic(int ID)
        {
            return dal.GetEditBasic(ID);
        }

        /// <summary>
        /// �õ��һ�����Ļ�����Ϣ
        /// </summary>
        public BCW.Model.User GetPwdBasic(string Mobile)
        {
            return dal.GetPwdBasic(Mobile);
        }

        /// <summary>
        /// �õ�Uskey/UsPwd
        /// </summary>
        public BCW.Model.User GetKey(int ID)
        {
            return dal.GetKey(ID);
        }
        /// <summary>
        /// �õ�Uskey/UsPwd
        /// </summary>
        public BCW.Model.User GetKey(string Mobile)
        {
            return dal.GetKey(Mobile);
        }

        /// <summary>
        /// �õ����ߵĻ�����Ϣ
        /// </summary>
        public BCW.Model.User GetOnlineBasic(int ID)
        {
            return dal.GetOnlineBasic(ID);
        }
        //------------------------------����-------------------------------------
        /// <summary>
        /// �����û������
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="iGold">������</param>
        public void UpdateiGold(int ID, long iGold)
        {
            dal.UpdateiGold(ID, iGold);
        }
        /// <summary>
        /// �����û����б�
        /// </summary>
        public void UpdateiBank(int ID, long iBank)
        {
            dal.UpdateiBank(ID, iBank);
        }

        /// <summary>
        /// �����û�����Ԫ
        /// </summary>
        public void UpdateiMoney(int ID, long iMoney)
        {
            dal.UpdateiMoney(ID, iMoney);
        }
        //------------------------------����-------------------------------------
        /// <summary>
        /// �����û������/�������Ѽ�¼
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="UsName">�û��ǳ�</param>
        /// <param name="iGold">������</param>
        /// <param name="AcText">˵��</param>
        public void UpdateiGold(int ID, long iGold, string AcText)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGold(ID, UsName, iGold, AcText);
        }

        public void UpdateiGold(int ID, string UsName, long iGold, string AcText)
        {
            //�����û������
            dal.UpdateiGold(ID, iGold);
            //�������Ѽ�¼
            BCW.Model.Goldlog model = new BCW.Model.Goldlog();
            model.BbTag = 0;
            model.Types = 0;
            model.PUrl = Utils.getPageUrl();//�������ļ���
            model.UsId = ID;
            model.UsName = UsName;
            model.AcGold = iGold;
            model.AfterGold = GetGold(ID);//���º�ı���
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Goldlog().Add(model);

            #region ��ֵ�齱�ӿ� 16/08/15
            try
            {
                new BCW.Draw.draw().AddjfbyiGold(ID, UsName, iGold, AcText);
            }
            catch { }
            #endregion
        }

        public void UpdateiGold(int ID, long iGold, string AcText, int Types)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGold(ID, UsName, iGold, AcText, Types);
        }

        public void UpdateiGold(int ID, string UsName, long iGold, string AcText, int Types)
        {
            UpdateiGold(ID, UsName, iGold, AcText);
            //�������а�
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }

        /// <summary>
        ///  �����ֶ��޸������б� �۹��� 20161107 
        /// </summary>
        public DataSet update_ziduan(string strField, string strWhere)
        {
            return dal.update_ziduan(strField, strWhere);
        }


        /// <summary>
        /// �����û������/�������а�/�������Ѽ�¼
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="UsName">�û��ǳ�</param>
        /// <param name="iGold">������</param>
        /// <param name="Types">���а�����</param>
        public void UpdateiGold(int ID, long iGold, int Types)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGold(ID, UsName, iGold, Types);
        }
        public void UpdateiGold(int ID, string UsName, long iGold, int Types)
        {
            //�����û������
            string AcText = string.Empty;
            if (Types == 1)
                AcText = "����ʯͷ��Ϸ����";
            else if (Types == 2)
                AcText = "789��Ϸ����";
            else if (Types == 3)
                AcText = "�²�����Ϸ����";
            else if (Types == 4)
                AcText = "����28��Ϸ����";
            else if (Types == 5)
                AcText = "����Ͷע��Ϸ����";
            else if (Types == 6)
                AcText = "����������";
            else if (Types == 7)
                AcText = "���ţ����";
            else if (Types == 8)
                AcText = "��ȭ����";
            else if (Types == 9)
                AcText = "��Сׯ����";
            else if (Types == 10)
                AcText = "��������";
            else if (Types == 11)
                AcText = "ʱʱ������";
            else if (Types == 22)
                AcText = "�¿�3����";
            else if (Types == 25)
                AcText = "�����˿�3����";
            else if (Types == 26)
                AcText = "��������";

            UpdateiGold(ID, UsName, iGold, AcText);
            //�������а�
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }

        #region ��Сׯ�ø��½�� UpdateiGold
        /// <summary>
        /// ��Сׯ�ø�������
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UsName"></param>
        /// <param name="iGold"></param>
        /// <param name="Types"></param>
        /// <param name="AcText"></param>
        public void UpdateiGold(int ID, string UsName, long iGold, int Types, string AcText)
        {
            //�����û������
            UpdateiGold(ID, UsName, iGold, AcText);
            //�������а�
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }
        #endregion

        /// <summary>
        /// ���¸������а�
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="UsName">�û��ǳ�</param>
        /// <param name="iGold">������</param>
        /// <param name="Types">���а�����</param>
        public void UpdateiGoldTop(int ID, long iGold, int Types)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGoldTop(ID, UsName, iGold, Types);
        }
        public void UpdateiGoldTop(int ID, string UsName, long iGold, int Types)
        {
            //�������а�
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }

        /// <summary>
        /// �����û�����Ԫ/�������Ѽ�¼
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="UsName">�û��ǳ�</param>
        /// <param name="iGold">������</param>
        /// <param name="AcText">˵��</param>
        public void UpdateiMoney(int ID, long iMoney, string AcText)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiMoney(ID, UsName, iMoney, AcText);
        }

        public void UpdateiMoney(int ID, string UsName, long iMoney, string AcText)
        {
            //�����û������
            dal.UpdateiMoney(ID, iMoney);
            //�������Ѽ�¼
            BCW.Model.Goldlog model = new BCW.Model.Goldlog();
            model.BbTag = 0;
            model.Types = 1;
            model.PUrl = Utils.getPageUrl();//�������ļ���
            model.UsId = ID;
            model.UsName = UsName;
            model.AcGold = iMoney;
            model.AfterGold = GetMoney(ID);//���º��Ԫ��
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Goldlog().Add(model);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼����̨�б�ʹ�ã�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">���з�ʽ</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsersManage(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetUsersManage(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
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
            return dal.GetUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
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
            return dal.GetUsers(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
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
            return dal.GetInvites(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
