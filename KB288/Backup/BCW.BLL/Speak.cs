using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Speak ��ժҪ˵����
    /// 
    /// ���ӵ�ֵ�齱������� ���ڽ� 20160823 
    /// </summary>
    public class Speak
    {
        private readonly BCW.DAL.Speak dal = new BCW.DAL.Speak();
        public Speak()
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
        /// ������������
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// ����ĳ��ԱID�����������
        /// </summary>
        public int GetCount(int UsId)
        {
            return dal.GetCount(UsId);
        }
               
        /// <summary>
        /// ����ĳʱ��ε����ķ�����
        /// </summary>
        public int GetCount(DateTime dt)
        {
            return dal.GetCount(dt);
        }
                
        /// <summary>
        /// ���������������ķ�����
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// �������ӻ�Ծ�齱���
        /// 20160602Ҧ־��
        /// </summary>
        public int Add(BCW.Model.Speak model)
        {
            int ID= dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = model.UsId;
                string username = model.UsName;
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
            catch {  }

            try
            {
                int usid = model.UsId;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "���ķ���";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }
            return ID;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Speak model)
        {
            dal.Update(model);
        }
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        public void Clear(int Types)
        {

            dal.Clear(Types);
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        public void Clear(int Types, int NodeId)
        {

            dal.Clear(Types, NodeId);
        }

        /// <summary>
        /// ���ȫ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Speak GetSpeak(int ID)
        {

            return dal.GetSpeak(ID);
        }

        /// <summary>
        /// �õ�һ��IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

        /// <summary>
        /// �õ���Ա��һ���������ݺ�ʱ��
        /// </summary>
        public BCW.Model.Speak GetNotesAddTime(int UsId)
        {

            return dal.GetNotesAddTime(UsId);
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
        /// <returns>IList Speak</returns>
        public IList<BCW.Model.Speak> GetSpeaks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpeaks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Speak</returns>
        public IList<BCW.Model.Speak> GetSpeaks(int SizeNum, string strWhere)
        {
            return dal.GetSpeaks(SizeNum, strWhere);
        }
                
        /// <summary>
        /// �������а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Speak> GetSpeaksTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpeaksTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// �������а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Speak> GetSpeaksTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpeaksTop2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  ��Ա����
    }
}

