using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���ChatText ��ժҪ˵����
    /// </summary>
    public class ChatText
    {
        private readonly BCW.DAL.ChatText dal = new BCW.DAL.ChatText();
        public ChatText()
        { }
        #region  ��Ա����

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
        /// ����ĳ��ԱID�����������
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// ���㷢���������
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// ����ĳʱ��ε����ķ�����
        /// </summary>
        public int GetCount(DateTime dt)
        {
            return dal.GetCount(dt);
        }


        ///// <summary>
        ///// ����һ������
        ///// </summary>
        //public int  Add(BCW.Model.ChatText model)
        //{
        //    return dal.Add(model);
        //}
        /// <summary>
        /// ����һ������
        /// ��Ծ�齱���_����������
        /// </summary>
        public int Add(BCW.Model.ChatText model)
        {
            int id = dal.Add(model);
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action���
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action��俪��
            int UsId = model.UsID;
            string UsName = model.UsName;
            string Notes = "�İɿ���������";
            //��Ծ�齱����
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, UsId, UsName, Notes, id);
                    if (isHit == 1)
                    {
                        if (WinnersGuessOpen == "1")
                        {
                            new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);//�����ߵ���ID
                            //if (ActionOpen == "1")
                            //{
                            //    Add(UsId, ActionText);
                            //}
                        }
                    }
                    //return id;
                }
                catch
                {
                    // return id;
                }
            }
            else
            {

            }

            // ���ڽ� 20160910 �����İɷ��Ե�ֵ�齱�ӿ�
            try { new BCW.Draw.draw().AddjfbyTz(UsId, UsName, "�İɷ���"); }
            catch { }
            return id;

        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.ChatText model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// ɾ��ĳ����ĳID��������
        /// </summary>
        public void Delete(int ChatId, int UsID)
        {
            dal.Delete(ChatId, UsID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(int ChatId)
        {
            dal.DeleteStr(ChatId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// ɾ��ĳ������N��ǰ������
        /// </summary>
        public void DeleteStr(int ChatId, int Day)
        {
            dal.DeleteStr(ChatId, Day);
        }

        /// <summary>
        /// ��������ɾ������������
        /// </summary>
        public void DeleteStr2(string strWhere)
        {
            dal.DeleteStr2(strWhere);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.ChatText GetChatText(int ID)
        {
            return dal.GetChatText(ID);
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
        /// <returns>IList ChatText</returns>
        public IList<BCW.Model.ChatText> GetChatTexts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetChatTexts(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// �������а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.ChatText> GetChatTextsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetChatTextsTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

