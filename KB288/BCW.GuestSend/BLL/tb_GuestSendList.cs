using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_GuestSendList ��ժҪ˵����
    /// </summary>
    public class tb_GuestSendList
    {
        private readonly BCW.DAL.tb_GuestSendList dal = new BCW.DAL.tb_GuestSendList();
        public tb_GuestSendList()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }
        /// <summary>
        /// �Ƿ���ڸü�¼usid guestsendID type
        /// </summary>
        public bool ExistsUidType(int usid, int guestsendID, int type)
        {
            return dal.ExistsUidType(usid, guestsendID, type);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.tb_GuestSendList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_GuestSendList model)
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
        /// ��uid guestId �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_GuestSendList Gettb_GuestSendListForUsidGuestID(int usid, int guestID)
        {
            return dal.Gettb_GuestSendListForUsidGuestID(usid,guestID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_GuestSendList Gettb_GuestSendList(int ID)
        {

            return dal.Gettb_GuestSendList(ID);
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
        /// <returns>IList tb_GuestSendList</returns>
        public IList<BCW.Model.tb_GuestSendList> Gettb_GuestSendLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_GuestSendLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

