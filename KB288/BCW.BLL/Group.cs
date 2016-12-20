using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Group ��ժҪ˵����
    /// </summary>
    public class Group
    {
        private readonly BCW.DAL.Group dal = new BCW.DAL.Group();
        public Group()
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
        public bool ExistsUsID(int UsID)
        {
            return dal.ExistsUsID(UsID);
        }
             
        /// <summary>
        /// �õ�ĳ���е�Ȧ����
        /// </summary>
        public int GetGroupNum(string City)
        {
            return dal.GetGroupNum(City);
        }

        /// <summary>
        /// �õ�ĳ�����Ȧ����
        /// </summary>
        public int GetGroupNum(int Types)
        {
            return dal.GetGroupNum(Types);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Group model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Group model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(BCW.Model.Group model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ���¹���ʱ��(����)
        /// </summary>
        public void UpdateExitTime(int ID, DateTime ExTime)
        {
            dal.UpdateExitTime(ID, ExTime);
        }

        /// <summary>
        /// ���»�����Ŀ
        /// </summary>
        public void UpdateiCent(int ID, long iCent)
        {
            dal.UpdateiCent(ID, iCent);
        }
        
        /// <summary>
        /// ���»�������
        /// </summary>
        public void UpdateiCentPwd(int ID, string iCentPwd)
        {
            dal.UpdateiCentPwd(ID, iCentPwd);
        }

        /// <summary>
        /// ����ǩ��ID
        /// </summary>
        public void UpdateSignID(int ID, string SignID)
        {
            dal.UpdateSignID(ID, SignID);
        }

        /// <summary>
        /// ���½�������ID
        /// </summary>
        public void UpdateVisitId(int ID)
        {
            dal.UpdateVisitId(ID);
        } 
       
        /// <summary>
        /// ���½�������ID
        /// </summary>
        public void UpdateVisitId(int ID, string VisitId)
        {
            dal.UpdateVisitId(ID, VisitId);
        }

        /// <summary>
        /// ���³�Ա����
        /// </summary>
        public void UpdateiTotal(int ID, int iTotal)
        {
            dal.UpdateiTotal(ID, iTotal);
        }
                
        /// <summary>
        /// ���Ȧ��
        /// </summary>
        public void UpdateStatus(int ID, int Status)
        {
            dal.UpdateStatus(ID, Status);
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
        public BCW.Model.Group GetGroup(int ID)
        {

            return dal.GetGroup(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Group GetGroupMe(int ID)
        {

            return dal.GetGroupMe(ID);
        }

        /// <summary>
        /// �õ�Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }
        
        /// <summary>
        /// �õ�UsID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }
               
        /// <summary>
        /// �õ�ID
        /// </summary>
        public int GetID(int UsID)
        {
            return dal.GetID(UsID);
        }

        /// <summary>
        /// �õ�ForumId
        /// </summary>
        public int GetForumId(int UsID)
        {
            return dal.GetForumId(UsID);
        }

        /// <summary>
        /// �õ�SignID
        /// </summary>
        public string GetSignID(int ID)
        {
            return dal.GetSignID(ID);
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
        /// <returns>IList Group</returns>
        public IList<BCW.Model.Group> GetGroups(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetGroups(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

