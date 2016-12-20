using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Friend ��ժҪ˵����
    /// </summary>
    public class Friend
    {
        private readonly BCW.DAL.Friend dal = new BCW.DAL.Friend();
        public Friend()
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
        public bool Exists(int UsID, int FriendId, int Types)
        {
            return dal.Exists(UsID, FriendId, Types);
        }

        /// <summary>
        /// ����ĳ�����������
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }

        /// <summary>
        /// ����ĳID��������
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// ����ĳID��˿����
        /// </summary>
        public int GetFansCount(int UsID)
        {
            return dal.GetFansCount(UsID);
        }

        /// <summary>
        /// ����һ������
        /// �Ӻ��Ѽ���齱��̬201605
        /// </summary>
        public int Add(BCW.Model.Friend model)
        {
           // return dal.Add(model);
            int ID = dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = model.UsID;
                string username = model.UsName;
                string Notes = "�Ӻ���";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
                return ID;
            }
            catch { return ID; }
        }

        /// <summary>
        /// ����ĳ������ѳ�ΪĬ�Ϸ���
        /// </summary>
        public void UpdateNodeId(int UsID, int NodeId)
        {
            dal.UpdateNodeId(UsID, NodeId);
        }

        /// <summary>
        /// ���������ϵʱ��
        /// </summary>
        public void UpdateTime(int UsID, int FriendID)
        {
            dal.UpdateTime(UsID, FriendID);
        }

        /// <summary>
        /// ������ʽ����
        /// </summary>
        public void UpdateTypes(int UsID, int FriendID)
        {
            dal.UpdateTypes(UsID, FriendID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Friend model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {

            dal.Delete(UsID, NodeId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int FriendID, int Types)
        {
            dal.Delete(UsID, FriendID, Types);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Friend GetFriend(int FriendId)
        {

            return dal.GetFriend(FriendId);
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
        /// <returns>IList Friend</returns>
        public IList<BCW.Model.Friend> GetFriends(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetFriends(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

