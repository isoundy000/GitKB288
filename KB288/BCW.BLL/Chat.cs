using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Chat ��ժҪ˵����
    /// </summary>
    public class Chat
    {
        private readonly BCW.DAL.Chat dal = new BCW.DAL.Chat();
        public Chat()
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID)
        {
            return dal.Exists2(ID);
        }
                
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists3(int UsID)
        {
            return dal.Exists3(UsID);
        }
        /// <summary>
        /// ���ڼ�����¼
        /// 2016/3/1 ������
        /// </summary>
        public int Exists4(int UsID)
        {
            return dal.Exists4(UsID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Chat model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Chat model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateBasic(BCW.Model.Chat model)
        {
            dal.UpdateBasic(model);
        }
                
        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateCb(BCW.Model.Chat model)
        {
            dal.UpdateCb(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateAdmin(BCW.Model.Chat model)
        {
            dal.UpdateAdmin(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateAdmin2(BCW.Model.Chat model)
        {
            dal.UpdateAdmin2(model);
        }

        /// <summary>
        /// ���»�����Ŀ
        /// </summary>
        public void UpdateChatCent(int ID, long ChatCent)
        {
            dal.UpdateChatCent(ID, ChatCent);
        }

        /// <summary>
        /// ���»�������
        /// </summary>
        public void UpdateCentPwd(int ID, string CentPwd)
        {
            dal.UpdateCentPwd(ID, CentPwd);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateLine(int ID, int Line)
        {
            dal.UpdateLine(ID, Line);
        }

        /// <summary>
        /// ����������1
        /// </summary>
        public void UpdateLine(int ID)
        {
            dal.UpdateLine(ID);
        }
                
        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateChatPwd(int ID, string ChatPwd)
        {
            dal.UpdateChatPwd(ID, ChatPwd);
        }
                
        /// <summary>
        /// ����ʹ����������ID
        /// </summary>
        public void UpdatePwdID(int ID, string PwdID)
        {
            dal.UpdatePwdID(ID, PwdID);
        }
        
        /// <summary>
        /// ���¹���ʱ��(����)
        /// </summary>
        public void UpdateExitTime(int ID, DateTime ExTime)
        {
            dal.UpdateExitTime(ID, ExTime);
        }
                
        /// <summary>
        /// �������ҽ���ʱ��
        /// </summary>
        public void UpdateChatCTime(int ID, DateTime ChatCTime)
        {
            dal.UpdateChatCTime(ID, ChatCTime);
        }
               
        /// <summary>
        /// �������ҹ���ID
        /// </summary>
        public void UpdateChatCId(int ID, string ChatCId)
        {
            dal.UpdateChatCId(ID, ChatCId);
        }
                
        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateCb(int ID, string ChatCId, DateTime ChatCTime)
        {
            dal.UpdateCb(ID, ChatCId, ChatCTime);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// �õ�����������
        /// </summary>
        public string GetChatPwd(int ID)
        {
            return dal.GetChatPwd(ID);
        }
                
        /// <summary>
        /// �õ�����������
        /// </summary>
        public string GetChatName(int ID)
        {
            return dal.GetChatName(ID);
        }
        /// <summary>
        /// �õ�����������
        /// </summary>
        public int GetChatType(int ID)
        {
            return dal.GetChatType(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Chat GetChat(int ID)
        {

            return dal.GetChat(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Chat GetChatBasic(int ID)
        {
            return dal.GetChatBasic(ID);
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
        /// <returns>IList Chat</returns>
        public IList<BCW.Model.Chat> GetChats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetChats(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

