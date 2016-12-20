using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Forumlog ��ժҪ˵����
    /// </summary>
    public class Forumlog
    {
        private readonly BCW.DAL.Forumlog dal = new BCW.DAL.Forumlog();
        public Forumlog()
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
        /// �õ���¼��
        /// </summary>
        public int GetCount(int BID, int ReID)
        {
            return dal.GetCount(BID, ReID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(int Types, int ForumID, string Content)
        {
            return dal.Add(Types, ForumID, Content);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(int Types, int ForumID, int BID, string Content)
        {
            return dal.Add(Types, ForumID, BID, Content);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(int Types, int ForumID, int BID, int ReID, string Content)
        {
            return dal.Add(Types, ForumID, BID, ReID, Content);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Forumlog model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Forumlog model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Forumlog GetForumlog(int ID)
        {

            return dal.GetForumlog(ID);
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
        /// <returns>IList Forumlog</returns>
        public IList<BCW.Model.Forumlog> GetForumlogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetForumlogs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
