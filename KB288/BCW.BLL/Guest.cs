using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Guest ��ժҪ˵����
    /// </summary>
    public class Guest
    {
        private readonly BCW.DAL.Guest dal = new BCW.DAL.Guest();
        public Guest()
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
        /// �õ�Types
        /// </summary>
        public void UpdateTypes(int ID, int Types)
        {
            dal.UpdateTypes(ID, Types);
        }
        /// <summary>
        /// �õ�Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// ����һ��ϵͳ��Ϣ
        /// ���߹��� 20161024
        /// </summary>
        public int AddNew(int Types, int ToId, string ToName, string Content)
        {
            return dal.Add(Types, ToId, ToName, Content);
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
        public bool ExistsFrom(int ID, int UsID)
        {
            return dal.ExistsFrom(ID, UsID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsTo(int ID, int UsID)
        {
            return dal.ExistsTo(ID, UsID);
        }

        /// <summary>
        /// ���������δ����ϵͳ��Ϣ
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// ���������δ��ϵͳ��Ϣ
        /// </summary>
        public int GetXCount(int UsID)
        {
            return dal.GetXCount(UsID);
        }

        /// <summary>
        /// ����ĳID��Ϣ������
        /// </summary>
        public int GetIDCount(int UsID)
        {
            return dal.GetIDCount(UsID);
        }

        /// <summary>
        /// ����һ������
        /// ���ӷ����߻�Ծ�齱���--Ҧ־��
        /// ɾȥ���20160608
        /// </summary>
        public int Add(BCW.Model.Guest model)
        {
            return dal.Add(model);
            //int ID = dal.Add(model);
            //try
            //{
            //    string xmlPath = "/Controls/winners.xml";
            //    string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
            //    string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
            //    string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
            //    string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
            //    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
            //    int usid = model.FromId;
            //    string username = model.FromName;
            //    string Notes = "������";
            //    int id = new BCW.BLL.Action().GetMaxId();
            //    int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
            //    if (isHit == 1)
            //    {
            //        if (WinnersGuessOpen == "1")
            //        {
            //            new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
            //        }
            //    }
            //    return ID;
            //}
            //catch { return ID; }
        }

        /// <summary>
        /// ����һ��ϵͳ��Ϣ
        /// </summary>
        public int Add(int ToId, string ToName, string Content)
        {
            return dal.Add(0, ToId, ToName, Content);
        }

        /// <summary>
        /// ����һ��ϵͳ��Ϣ
        /// </summary>
        public int Add(int Types, int ToId, string ToName, string Content)
        {
            bool flag = true;
            string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
            if (ForumSet != "")
            {
                if (Types == 0)//ϵͳ��Ϣ
                {
                    int xTotal = GetForumSet(ForumSet, 15);
                    if (xTotal == 1)
                        flag = false;
                }
                else if (Types == 1)//��ϷPK�����Ϣ
                {
                    int gTotal = GetForumSet(ForumSet, 16);
                    if (gTotal == 1)
                        flag = false;
                }
                else if (Types == 2)//�Ƽ�����������Ϣ
                {
                    int tTotal = GetForumSet(ForumSet, 14);
                    if (tTotal == 1)
                        flag = false;
                }
                else if (Types == 3)//�ռ�����������Ϣ
                {
                    int bTotal = GetForumSet(ForumSet, 20);
                    if (bTotal == 1)
                        flag = false;
                }
                else if (Types == 4)//����˽��������Ϣ/�����ߵسɹ�ʧ������
                {
                    int sTotal = GetForumSet(ForumSet, 33);
                    if (sTotal == 1)
                        flag = false;
                }
                if (flag)
                {
                    return dal.Add(Types, ToId, ToName, Content);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetForumSet(string ForumSet, int iType)
        {
            string[] forumset = ForumSet.Split(",".ToCharArray());

            string[] fs = forumset[iType].ToString().Split("|".ToCharArray());

            return Convert.ToInt32(fs[1]);
        }

        /// <summary>
        /// ����Ϊ�ղ�
        /// </summary>
        public void UpdateIsKeep(int ID, int UsID)
        {

            dal.UpdateIsKeep(ID, UsID);
        }

        /// <summary>
        /// ����Ϊ�Ѷ�
        /// </summary>
        public void UpdateIsSeen(int ID)
        {

            dal.UpdateIsSeen(ID);
        }

        /// <summary>
        /// ����Ϊ�Ѷ�
        /// </summary>
        public void UpdateIsSeenAll(int UsID, int ptype)
        {

            dal.UpdateIsSeenAll(UsID, ptype);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateFDel(int ID)
        {

            dal.UpdateFDel(ID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateTDel(int ID)
        {

            dal.UpdateTDel(ID);
        }

        /// <summary>
        /// �����Ѷ�����
        /// </summary>
        public void UpdateTDel2(int UsID)
        {

            dal.UpdateTDel2(UsID);
        }

        /// <summary>
        /// �����ѷ�����
        /// </summary>
        public void UpdateFDel2(int UsID)
        {

            dal.UpdateFDel2(UsID);
        }

        /// <summary>
        /// ɾ���Ѷ�ϵͳ��Ϣ
        /// </summary>
        public void UpdateXDel2(int UsID)
        {
            dal.UpdateXDel2(UsID);
        }


        /// <summary>
        /// �����ղ�����
        /// </summary>
        public void UpdateKDel2(int UsID)
        {

            dal.UpdateKDel2(UsID);
        }

        /// <summary>
        ///  ɾ���Ի�(����)
        /// </summary>
        public void UpdateChatFDel(int UsID, int Hid)
        {

            dal.UpdateChatFDel(UsID, Hid);
        }

        /// <summary>
        ///  ɾ���Ի�(����)
        /// </summary>
        public void UpdateChatTDel(int UsID, int Hid)
        {

            dal.UpdateChatTDel(UsID, Hid);
        }

        /// <summary>
        ///  ɾ����������Ϣ
        /// </summary>
        public void UpdateSClear(int UsID)
        {

            dal.UpdateSClear(UsID);
        }

        /// <summary>
        ///  ɾ��ϵͳ��Ϣ
        /// </summary>
        public int UpdateXClear(int UsID)
        {

            return dal.UpdateXClear(UsID);
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
        /// �õ�����ID
        /// </summary>
        public int GetToId(int ID)
        {
            return dal.GetToId(ID);
        }

        /// <summary>
        /// �õ��Ƿ��Ѷ�
        /// </summary>
        public int GetIsSeen(int ID)
        {
            return dal.GetIsSeen(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Guest GetGuest(int ID)
        {

            return dal.GetGuest(ID);
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
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuests(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGuests(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼Asc
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuestsAsc(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGuestsAsc(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// ȡ��ÿҳID����
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuestsID(int p_pageIndex, int p_pageSize, string strWhere)
        {
            return dal.GetGuestsID(p_pageIndex, p_pageSize, strWhere);
        }

        /// <summary>
        /// �洢���̷�ҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Guest> GetGuests(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetGuests(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        #endregion  ��Ա����
    }
}