using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /**
     * 
     * �޸��� ��־�� 2016/4/9
     * �޸��� Ҧ־�� 2016/5/28
     * �������ӻ�Ծ�齱����¼�
     * �޸���  ��־�� 2016/08/11
     * ��������ͳ�Ʒ���
     * �������Ӳ�����ֵ�齱��� ���ڽ� 2016/08/23
      **/

    /// <summary>
    /// ҵ���߼���Text ��ժҪ˵����
    /// </summary>
    public class Text
    {
        private readonly BCW.DAL.Text dal = new BCW.DAL.Text();
        public Text()
        { }
        #region  ��Ա����

        /// <summary>
        /// ���������Ѳɼ��ı�ʶ
        /// </summary>
        public void UpdateIstxt(int ID, int Istxt)
        {

            dal.UpdateIstxt(ID, Istxt);
        }
        //------------------------������̳ʹ��-------------------------



        /// <summary>
        /// �������н��������С�����
        /// </summary>
        public void UpdategGsNum(int ID, int gwinnum, int glznum, int gmnum)
        {
            dal.UpdategGsNum(ID, gwinnum, glznum, gmnum);
        }

        /// <summary>
        /// �õ����еĴ���
        /// </summary>
        public int Getglznum(int ID)
        {
            return dal.Getglznum(ID);
        }
        /// <summary>
        /// �õ�������
        /// </summary>
        public int GetPraise(int ID)
        {
            return dal.GetPraise(ID);
        }
        ///// <summary>
        ///// �õ�������ID
        ///// </summary>
        //public int GetPraiseID(int ID)
        //{
        //    return dal.GetPraiseID(ID);
        //}
        /// <summary>
        /// ������һ�ڵ�����
        /// </summary>
        public void UpdateGqinum(int ID, int Gqinum)
        {
            dal.UpdateGqinum(ID, Gqinum);
        }
        //------------------------������̳ʹ��-------------------------

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
        /// �Ƿ���ڸü�¼(����վ)
        /// </summary>
        public bool Exists(int ID, int ForumID)
        {
            return dal.Exists(ID, ForumID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID, int ForumID)
        {
            return dal.Exists2(ID, ForumID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID, int ForumID, int UsID)
        {
            return dal.Exists2(ID, ForumID, UsID);
        }

        /// <summary>
        /// ��Ա��ĳ������̳�Ƿ���ڷ���
        /// </summary>
        public int GetRaceBID(int ForumID, int UsID)
        {
            return dal.GetRaceBID(ForumID, UsID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Text model)
        {
            int id= dal.Add(model);

            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "��������";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }

            return id;
        }
        /// <summary>
        /// ����һ���ɱ���
        /// </summary>
        public int AddPricesLimit(BCW.Model.Text model)
        {
            int id = dal.AddPricesLimit(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = (model.UsID);
                string username = model.UsName;
                string Notes = "���ӱ�";
                int ID = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, ID);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }

            }
            catch {  }

            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "��������";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }  
            
            return id;
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Text model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(BCW.Model.Text model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            dal.UpdateContent(ID, Content);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateTypes(int ID, int Types)
        {
            dal.UpdateTypes(ID, Types);
        }
        /// <summary>
        /// ���µ�����
        /// </summary>
        public void UpdatePraise(int ID, int Praise)
        {
            dal.UpdatePraise(ID, Praise);
        }

        // /// <summary>
        ///// ���µ�����ID
        ///// </summary>
        //public void UpdatePraiseID(int ID, int PraiseID)
        //{
        //    dal.UpdatePraiseID(ID,PraiseID);
        //}
        /// <summary>
        /// ������ϸͳ��
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReList)
        {
            dal.UpdateReStats(ID, ReStats, ReList);
        }

        /// <summary>
        /// ���¹���ID
        /// </summary>
        public void UpdatePayID(int ID, string PayID)
        {
            dal.UpdatePayID(ID, PayID);
        }

        /// <summary>
        /// ���»ظ�ID
        /// </summary>
        public void UpdateReplyID(int ID, string ReplyID)
        {
            dal.UpdateReplyID(ID, ReplyID);
        }
        /// <summary>
        /// �������ɱ�ID
        /// </summary>
        public void UpdateIsPriceID(int ID, string IsPriceID)
        {
            dal.UpdateIsPriceID(ID, IsPriceID);
        }
        /// <summary>
        /// ���µ���ID
        /// </summary>
        public void UpdatePraiseID(int ID, string PraiseID)
        {
            dal.UpdatePraiseID(ID, PraiseID);
        }

        /// <summary>
        /// �������ɶ��ٱ�
        /// </summary>
        public void UpdatePricel(int ID, long Price)
        {
            dal.UpdatePricel(ID, Price);
        }

        /// <summary>
        /// ���»ظ���
        /// </summary>
        public void UpdateReplyNum(int ID, int ReplyNum)
        {
            dal.UpdateReplyNum(ID, ReplyNum);
        }

        /// <summary>
        /// �����Ķ���
        /// </summary>
        public void UpdateReadNum(int ID, int ReadNum)
        {
            dal.UpdateReadNum(ID, ReadNum);
        }

        /// <summary>
        /// ����ר���ʶ
        /// </summary>
        public void UpdateTsID(int ID, int TsID)
        {
            dal.UpdateTsID(ID, TsID);
        }

        /// <summary>
        /// ��������ר���ʶ
        /// </summary>
        public void UpdateTsID2(int TsID, int NewTsID)
        {
            dal.UpdateTsID2(TsID, NewTsID);
        }

        /// <summary>
        /// ����ת������
        /// </summary>
        public void UpdateForumID2(int ForumID, int NewForumID)
        {
            dal.UpdateForumID2(ForumID, NewForumID);
        }

        /// <summary>
        /// ���������ļ���
        /// </summary>
        public void UpdateFileNum(int ID, int FileNum)
        {
            dal.UpdateFileNum(ID, FileNum);
        }

        //-------------------------------------��������-------------------------------------------

        /// <summary>
        /// ����/ȡ������վ
        /// </summary>
        public void UpdateIsDel(int ID, int IsDel)
        {
            dal.UpdateIsDel(ID, IsDel);
        }

        ///// <summary>
        ///// �Ӿ�/�⾫
        ///// </summary>
        //public void UpdateIsGood(int ID, int IsGood)
        //{
        //    dal.UpdateIsGood(ID, IsGood);
        //}
        ///// <summary>
        ///// �Ƽ�/���Ƽ�
        ///// </summary>
        //public void UpdateIsRecom(int ID, int IsRecom)
        //{
        //    dal.UpdateIsRecom(ID, IsRecom);
        //} 

        /// <summary>
        /// �ö�/ȥ��/�̵�/ȥ��
        /// </summary>
        //public void UpdateIsTop(int ID, int IsTop)
        //{
        //    dal.UpdateIsTop(ID, IsTop);
        //}

        /// <summary>
        /// �Ӿ�/�⾫�����Ծ�齱--20160523Ҧ־��
        /// </summary>
        /// <summary>
        /// �Ӿ�/�⾫
        /// </summary>
        public void UpdateIsGood(int ID, int IsGood)
        {
            dal.UpdateIsGood(ID, IsGood);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "���ӼӾ�";
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

            try
            {
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "���ӼӾ�";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }
        }
        /// <summary>
        /// �Ƽ�/���Ƽ�--�����Ծ�齱20160523Ҧ־��
        /// </summary>
        public void UpdateIsRecom(int ID, int IsRecom)
        {
            dal.UpdateIsRecom(ID, IsRecom);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�����Ƽ�";
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

            try
            {
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�����Ƽ�";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { } 
        }

        /// <summary>
        /// �ö�/ȥ��/�̵�/ȥ��--�����Ծ�齱20160523Ҧ־��
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�����ö�/ȥ��/�̵�/ȥ��";
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
        /// ����/����
        /// </summary>
        public void UpdateIsLock(int ID, int IsLock)
        {
            dal.UpdateIsLock(ID, IsLock);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "��������/����";
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
        public void UpdateIsOver(int ID, int IsOver)
        {
            dal.UpdateIsOver(ID, IsOver);
        }

        ///// <summary>
        ///// ���ù���
        ///// </summary>
        //public void UpdateIsFlow(int ID, int IsFlow)
        //{
        //    dal.UpdateIsFlow(ID, IsFlow);
        //}
        /// <summary>
        /// ���ù���---�����Ծ�齱20160523Ҧ־��
        /// </summary>
        public void UpdateIsFlow(int ID, int IsFlow)
        {
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
            dal.UpdateIsFlow(ID, IsFlow);
            try
            {
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�������";
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

            try
            {
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�������";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }

        }
        /// <summary>
        /// ת��
        /// </summary>
        public void UpdateForumID(int ID, int ForumID)
        {
            dal.UpdateForumID(ID, ForumID);
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetIsLock(int ID)
        {
            return dal.GetIsLock(ID);
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetIsGood(int ID)
        {
            return dal.GetIsGood(ID);
        }
        /// <summary>
        /// ���е���ʱ��
        /// ��־��  2016 3/28
        /// </summary>
        public void UpdatePraiseTime(int ID, DateTime PraiseTime)
        {
            dal.UpdatePraiseTime(ID, PraiseTime);
        }
        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetIsOver(int ID)
        {
            return dal.GetIsOver(ID);
        }

        /// <summary>
        /// �õ����Ӹ�����
        /// </summary>
        public int GetFileNum(int ID)
        {
            return dal.GetFileNum(ID);
        }
        //-------------------------------------��������-------------------------------------------


        /// <summary>
        /// ���¹���ʱ��
        /// </summary>
        public void UpdateFlowTime(int ID, DateTime FlowTime, int IsFlow)
        {
            dal.UpdateFlowTime(ID, FlowTime, IsFlow);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// ��ջ���վ����
        /// </summary>
        public void Delete()
        {

            dal.Delete();
        }

        /// <summary>
        /// �õ����ӱ���
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// �õ������û�ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }

        /// <summary>
        /// �õ�����Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// �õ�IsFlow
        /// </summary>
        public int GetIsFlow(int ID)
        {
            return dal.GetIsFlow(ID);
        }

        /// <summary>
        /// �õ��ظ�ID
        /// </summary>
        public string GetReplyID(int ID)
        {
            return dal.GetReplyID(ID);
        }
        /// <summary>
        /// �õ�����ID
        /// </summary>
        public string GetPraiseID(int ID)
        {
            return dal.GetPraiseID(ID);
        }

        /// <summary>
        /// �õ��ɱ���ʵ��
        /// </summary>
        public BCW.Model.Text GetPriceModel(int ID)
        {

            return dal.GetPriceModel(ID);
        }

        /// <summary>
        /// ����õ�һ������������
        /// </summary>
        public BCW.Model.Text GetTextFlow()
        {
            return dal.GetTextFlow();
        }

        /// <summary>
        /// ����õ�ĳ��̳һ�Ź�����
        /// </summary>
        public BCW.Model.Text GetTextFlow(int ForumId)
        {

            return dal.GetTextFlow(ForumId);
        }

        /// <summary>
        /// ����õ�N���ڵ�һ���Ƽ��򾫻���
        /// </summary>
        public BCW.Model.Text GetTextGoodReCom(int Day)
        {

            return dal.GetTextGoodReCom(Day);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Text GetTextMe(int ID)
        {

            return dal.GetTextMe(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Text GetText(int ID)
        {

            return dal.GetText(ID);
        }
        /// <summary>
        /// �������з�ҳ��¼ ��־�� 2016/08/10
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Text> GetForumstats1(int p_pageIndex, int p_pageSize, string strWhere, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats1(p_pageIndex, p_pageSize, strWhere, showtype, out p_recordCount);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTexts(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetTexts(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTextsMe(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetTextsMe(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }


        #region ��̳�������з�ҳ��¼ ���￪ʼ�޸�
        /// <summary>
        /// ��̳�������з�ҳ��¼ ��־�� 20160324
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Text> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, strWhere, strOrder, showtype, out p_recordCount);
        }
        #endregion

        /// <summary>
        /// ȡ�ø�����̳���а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTextsGs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetTextsGs(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);

        }

        #endregion  ��Ա����
    }
}
