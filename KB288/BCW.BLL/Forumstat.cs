using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Forumstat ��ժҪ˵����
    /// </summary>
    public class Forumstat
    {
        private readonly BCW.DAL.Forumstat dal = new BCW.DAL.Forumstat();
        public Forumstat()
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
        /// �Ƿ���ڽ����¼
        /// </summary>
        public bool ExistsToDay(int UsID, int ForumID)
        {
            return dal.ExistsToDay(UsID, ForumID);
        }

        /// <summary>
        /// �õ�ĳIDĳѡ������
        /// </summary>
        public int GetIDCount(int UsID, int Types)
        {
            return dal.GetIDCount(UsID, Types);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Forumstat model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int Types, int UsID, int ForumID)
        {
            dal.Update(Types, UsID, ForumID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update3(int Types, int UsID, int ForumID, DateTime AddTime)
        {
            dal.Update3(Types, UsID, ForumID, AddTime);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(int Types, int UsID, int ForumID, DateTime AddTime)
        {
            dal.Update2(Types, UsID, ForumID, AddTime);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Forumstat model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ��������ͳ��
        /// </summary>
        public void UpdateForumID(int ForumID, int NewForumID)
        {
            dal.UpdateForumID(ForumID, NewForumID);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// ɾ�����в������ݣ�����Ϊ0�ļ�¼��
        /// </summary>
        public void Delete()
        {
            dal.Delete();
        }

        /// <summary>
        /// �õ�ĳѡ���������
        /// </summary>
        public int GetCount(int UsID, int Types)
        {
            return dal.GetCount(UsID, Types);
        }

        /// <summary>
        /// �õ�ĳID���·�����������(1����/2����)
        /// </summary>
        public int GetMonthCount(int UsID, int Types)
        {
            return dal.GetMonthCount(UsID, Types);
        }

        /// <summary>
        /// �õ�ĳ��̳ĳѡ������
        /// </summary>
        public int GetCount(int ForumID, int Types, int dType)
        {
            return dal.GetCount(0, 0, ForumID, Types, dType);
        }

        /// <summary>
        /// �õ�ĳIDĳ��̳ĳѡ������
        /// </summary>
        public int GetCount(int UsID, int ForumID, int Types, int dType)
        {
            return dal.GetCount(0, UsID, ForumID, Types, dType);
        }

        /// <summary>
        /// �õ�������̳��Ȧ����̳ĳѡ������(fg 0ֵͳ��ȫ����1ֵͳ��������̳��2ֵͳ��Ȧ����̳)
        /// 20160120 �ƹ���
        /// </summary>
        /// <param name="fg">����</param>
        /// <param name="ForumID">��̳ID</param>
        /// <param name="Types">�̶�����1������2������</param>
        /// <param name="dType">ʱ�����</param>
        /// <returns>����ͳ����</returns>
        public int GetCount2(int fg, int ForumID, int Types, int dType)
        {
            return dal.GetCount(fg, 0, ForumID, Types, dType);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Forumstat GetForumstat(int ID)
        {

            return dal.GetForumstat(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ��̳����ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Forumstat</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, strWhere, strOrder, showtype, out p_recordCount);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Forumstat</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        /// <summary>
        /// ��̳���з�ҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, int ptype, string sWhere, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, ptype, sWhere, out p_recordCount);
        }
        #endregion  ��Ա����
    }
}

