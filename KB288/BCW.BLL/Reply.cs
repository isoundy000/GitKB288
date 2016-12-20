using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Reply ��ժҪ˵����
    /// 
    /// ���ӵ�ֵ�齱������� ���ڽ� 20160823
    /// </summary>
    public class Reply
    {
        private readonly BCW.DAL.Reply dal = new BCW.DAL.Reply();
        public Reply()
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
        public bool Exists(int BID, int Floor)
        {
            return dal.Exists(BID, Floor);
        }

        /// <summary>
        /// ����ĳ��̳ĳ�û�ID���������
        /// </summary>
        public int GetCount(int UsID, int ForumId)
        {
            return dal.GetCount(UsID, ForumId);
        }
        /// <summary>
        /// ����ĳ����ĳ�û�ID����δ��ɾ���Ļ�����
        /// </summary>
        public int GetCountExist(int UsID, int BID)
        {
            return dal.GetCountExist(UsID, BID);
        }
        /// <summary>
        /// ����ĳ����ĳ�û�ID���������
        /// </summary>
        public int GetCount2(int UsID, int BID)
        {
            return dal.GetCount2(UsID, BID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Reply model)
        {
            int id= dal.Add(model);
            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�ظ�����";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }
            return id;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Reply model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int Bid, int Floor, string Content)
        {
            dal.Update(Bid, Floor, Content);
        }

        /// <summary>
        /// ����CentText
        /// </summary>
        public void UpdateCentText(int Bid, int Floor, string CentText)
        {
            dal.UpdateCentText(Bid, Floor, CentText);
        }

        /// <summary>
        /// ����ForumID
        /// </summary>
        public void UpdateForumID(int ID, int ForumID)
        {
            dal.UpdateForumID(ID, ForumID);
        }

        /// <summary>
        /// �������»���
        /// </summary>
        public void UpdateForumID2(int ForumID, int NewForumID)
        {
            dal.UpdateForumID2(ForumID, NewForumID);
        }

        /// <summary>
        /// ���»����ļ���
        /// </summary>
        public void UpdateFileNum(int ID, int FileNum)
        {
            dal.UpdateFileNum(ID, FileNum);
        }

        /// <summary>
        /// �����Ӿ�/�⾫
        /// </summary>
        public void UpdateIsGood(int Bid, int Floor, int IsGood)
        {
            dal.UpdateIsGood(Bid, Floor, IsGood);
        }

        /// <summary>
        /// �����ö�/ȥ��
        /// </summary>
        public void UpdateIsTop(int Bid, int Floor, int IsTop)
        {
            dal.UpdateIsTop(Bid, Floor, IsTop);
        }

        /// <summary>
        /// ����/ȡ������վ
        /// </summary>
        public void UpdateIsDel(int BID, int IsDel)
        {
            dal.UpdateIsDel(BID, IsDel);
        }

        /// <summary>
        /// ����/ȡ������վ
        /// </summary>
        public void UpdateIsDel1(int ID, int IsDel)
        {
            dal.UpdateIsDel1(ID, IsDel);
        }

        /// <summary>
        /// ����/ȡ������վ
        /// </summary>
        public void UpdateIsDel2(int BID, int Floor, int IsDel)
        {
            dal.UpdateIsDel2(BID, Floor, IsDel);
        }

        /// <summary>
        /// ����/ȡ������վ
        /// </summary>
        public void UpdateIsDel3(int BID, int UsID, int IsDel)
        {
            dal.UpdateIsDel3(BID, UsID, IsDel);
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(int Bid, int Floor)
        {
            dal.Delete(Bid, Floor);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete2(int Bid, int UsID)
        {
            dal.Delete2(Bid, UsID);
        }

        /// <summary>
        /// �õ���¥��
        /// </summary>
        public int GetFloor(int Bid)
        {

            return dal.GetFloor(Bid);
        }

        /// <summary>
        /// ����ID�õ�¥��
        /// </summary>
        public int GetFloor2(int ID)
        {

            return dal.GetFloor2(ID);
        }

        /// <summary>
        /// �õ�ĳ¥�������
        /// </summary>
        public string GetContent(int Bid, int Floor)
        {
            return dal.GetContent(Bid, Floor);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Reply GetReplyMe(int BID, int Floor)
        {
            return dal.GetReplyMe(BID, Floor);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Reply GetByID(int ID)
        {
            return dal.GetByID(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Reply GetReply(int Bid, int Floor)
        {

            return dal.GetReply(Bid, Floor);
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
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetReplys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        /// <summary>
        /// �������з�ҳ��¼ ��־�� 2016/08/10
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Reply> GetForumstats1(int p_pageIndex, int p_pageSize, string strWhere, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats1(p_pageIndex, p_pageSize, strWhere, showtype, out  p_recordCount);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplysMe(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetReplys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        /// <summary>
        /// ����������ѯ����
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<BCW.Model.Reply> GetReplysWhere(string strWhere)
        {
            return dal.GetReplysWhere(strWhere) ;
        }
        /// <summary>
        /// ����ҳ�������¼
        /// </summary>
        /// <param name="p_Size">��ʾ����</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Reply</returns>
        public IList<BCW.Model.Reply> GetReplysTop(int p_Size, string strWhere)
        {
            return dal.GetReplysTop(p_Size, strWhere);
        }
        #endregion  ��Ա����
    }
}