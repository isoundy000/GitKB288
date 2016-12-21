using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Forum ��ժҪ˵����
    /// </summary>
    public class Forum
    {
        private readonly BCW.DAL.Forum dal = new BCW.DAL.Forum();
        public Forum()
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
        /// ��̳���Ƿ������̳
        /// </summary>
        public bool ExistsNodeId(int ID)
        {
            return dal.ExistsNodeId(ID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.Forum model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Forum model)
        {
            dal.Update(model);
        }
                
        /// <summary>
        /// ���°��
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            dal.UpdateContent(ID, Content);
        }
                
        /// <summary>
        /// ���µ���
        /// </summary>
        public void UpdateFootUbb(int ID, string FootUbb)
        {
            dal.UpdateFootUbb(ID, FootUbb);
        }

        /// <summary>
        /// ����NodeId
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// �����¼�ID����
        /// </summary>
        public void UpdateDoNode(int ID, string DoNode)
        {
            dal.UpdateDoNode(ID, DoNode);
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
        /// ���»�����Ŀ
        /// </summary>
        public void UpdateiCent(int ID, long iCent)
        {
            dal.UpdateiCent(ID, iCent);
        }
        /// <summary>
        /// ���»�����Ŀ,����д��ʵ����ϸ
        /// </summary>
        public void UpdateiCent(int ID, int UID, string UsName, long iCent, int ToID,string AcText)
        {
            dal.UpdateiCent(ID, iCent);
            BCW.Model.Forumfund model = new BCW.Model.Forumfund();
            model.Types = 2;
            model.ForumId = ID;
            model.UsID = UID;
            model.UsName = UsName;
            model.PayCent = -iCent;
            model.Content = AcText;
            model.ToID = ToID;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Forumfund().Add(model);
          
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Forum GetForum(int ID)
        {

            return dal.GetForum(ID);
        }

        /// <summary>
        /// �õ�һ�����ơ��ںš�����
        /// </summary>
        public BCW.Model.Forum GetForumBasic(int ID)
        {
            return dal.GetForumBasic(ID);
        }

        /// <summary>
        /// �õ�Title
        /// </summary>
        public string GetTitle(int ID)
        {

            return dal.GetTitle(ID);
        }

        /// <summary>
        /// �õ���湫��Content
        /// </summary>
        public string GetContent(int ID)
        {

            return dal.GetContent(ID);
        }
                
        /// <summary>
        /// �õ�����FootUbb
        /// </summary>
        public string GetFootUbb(int ID)
        {

            return dal.GetFootUbb(ID);
        }

        /// <summary>
        /// �õ�NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {

            return dal.GetNodeId(ID);
        }
     
        /// <summary>
        /// �õ�GroupId
        /// </summary>
        public int GetGroupId(int ID)
        {

            return dal.GetGroupId(ID);
        }
             
        /// <summary>
        /// �õ���̳����
        /// </summary>
        public long GetiCent(int ID)
        {

            return dal.GetiCent(ID);
        }

        /// <summary>
        /// �õ�Label
        /// </summary>
        public string GetLabel(int ID)
        {
            return dal.GetLabel(ID);
        }
                
        /// <summary>
        /// �õ�DoNode�¼�ID
        /// </summary>
        public string GetDoNode(int ID)
        {
            return dal.GetDoNode(ID);
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
        /// <returns>IList Forum</returns>
        public IList<BCW.Model.Forum> GetForums(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetForums(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
