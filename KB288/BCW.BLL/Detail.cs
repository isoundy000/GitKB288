using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Detail ��ժҪ˵����
    /// </summary>
    public class Detail
    {
        private readonly BCW.DAL.Detail dal = new BCW.DAL.Detail();
        public Detail()
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
        public bool Exists(string Title)
        {
            return dal.Exists(Title);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Detail model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Detail model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ���½�ͼ
        /// </summary>
        public void UpdatePics(int ID, string Pics)
        {
            dal.UpdatePics(ID, Pics);
        }
 
        /// <summary>
        /// ���·���
        /// </summary>
        public void UpdateCover(int ID, string Cover)
        {
            dal.UpdateCover(ID, Cover);
        }

        /// <summary>
        /// ������ϸͳ��
        /// </summary>
        public void UpdateReStats(int ID, string ReStats,string ReLastIP)
        {
            dal.UpdateReStats(ID, ReStats, ReLastIP);
        }
        /// <summary>
        /// ���µ��ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClickID"></param>
        public void UpdateClickID(int ID, string ClickID)
        {
            dal.UpdateClickID(ID,ClickID);
        }
        /// <summary>
        /// ���µ����
        /// </summary>
        public void UpdateReadcount(int ID, int Readcount)
        {
            dal.UpdateReadcount(ID, Readcount);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateRecount(int ID, int Recount)
        {
            dal.UpdateRecount(ID, Recount);
        }

        /// <summary>
        /// ���½ڵ�
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// �������½ڵ�
        /// </summary>
        public void UpdateNodeIds(int NewNodeId, int OrdNodeId)
        {
            dal.UpdateNodeIds(NewNodeId, OrdNodeId);
        }
               
        /// <summary>
        /// ���¹���ID
        /// </summary>
        public void UpdatePayId(int ID, string PayId)
        {
            dal.UpdatePayId(ID, PayId);
        }
                
        /// <summary>
        /// ����ļ�
        /// </summary>
        public void UpdateHidden(int ID, int Hidden)
        {
            dal.UpdateHidden(ID, Hidden);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// ɾ����ͼ
        /// </summary>
        public void DeletePics(int ID)
        {

            dal.DeletePics(ID);
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        public void DeleteNodeId(int ID)
        {

            dal.DeleteNodeId(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Detail GetDetail(int ID)
        {

            return dal.GetDetail(ID);
        }

        /// <summary>
        /// �õ�Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// �õ����û���Model
        /// </summary>
        public string GetPhoneModel(int ID)
        {
            return dal.GetPhoneModel(ID);
        }

        /// <summary>
        /// �õ��ڵ�NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// �õ�����Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }
               
        /// <summary>
        /// �õ��û�ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }

        /// <summary>
        /// �õ��ļ�Pics
        /// </summary>
        public string GetPics(int ID)
        {
            return dal.GetPics(ID);
        }
                
        /// <summary>
        /// �õ�����Cover
        /// </summary>
        public string GetCover(int ID)
        {
            return dal.GetCover(ID);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ȡ����(��)һ����¼
        /// </summary>
        public BCW.Model.Detail GetPreviousNextDetail(int ID, int p_NodeId, bool p_next)
        {
            return dal.GetPreviousNextDetail(ID, p_NodeId, p_next);
        }

        ///// <summary>
        ///// ȡ��ȫ���������
        ///// </summary>
        ///// <param name="p_pageIndex">��ǰҳ</param>
        ///// <param name="p_pageSize">��ҳ��С</param>
        ///// <param name="p_recordCount">�����ܼ�¼��</param>
        ///// <returns>IList Detail</returns>
        //public IList<BCW.Model.Detail> GetDetails(int p_pageIndex, int p_pageSize, string strWhere, string Key, out int p_recordCount)
        //{
        //    return dal.GetDetails(p_pageIndex, p_pageSize, strWhere, Key, out p_recordCount);
        //}

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Detail</returns>
        public IList<BCW.Model.Detail> GetDetails(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetDetails(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

