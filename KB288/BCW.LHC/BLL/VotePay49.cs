using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using LHC.Model;

/// <summary>
/// =================================
/// �޸����а�����
/// �ƹ��� 20160612
/// =================================
/// </summary>
namespace LHC.BLL
{
    /// <summary>
    /// ҵ���߼���VotePay49 ��ժҪ˵����
    /// </summary>
    public class VotePay49
    {
        private readonly LHC.DAL.VotePay49 dal = new LHC.DAL.VotePay49();
        public VotePay49()
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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// ÿ��.��ÿ����ע����
        /// </summary>
        public long GetPayCent(int qiNo, int sNum)
        {
            return dal.GetPayCent(qiNo, sNum);
        }

        /// <summary>
        /// ÿIDÿ����ע����
        /// </summary>
        public long GetPayCent(int UsID, int qiNo, int bzType)
        {
            return dal.GetPayCent(UsID, qiNo, bzType);
        }

        /// <summary>
        /// ÿ���н�����
        /// </summary>
        public long GetwinCent(int qiNo, int bzType)
        {
            return dal.GetwinCent(qiNo, bzType);
        }

        /// <summary>
        /// ���������õ���ע����
        /// </summary>
        public long GetPayCent(string strWhere)
        {
            return dal.GetPayCent(strWhere);
        }

        /// <summary>
        /// ���������õ�Ӯ������
        /// </summary>
        public long GetwinCent(string strWhere)
        {
            return dal.GetwinCent(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LHC.Model.VotePay49 model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LHC.Model.VotePay49 model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ���¸���Ϊ����
        /// </summary>
        public void UpdateOver(int qiNo)
        {
            dal.UpdateOver(qiNo);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
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
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LHC.Model.VotePay49 GetVotePay49(int ID)
        {

            return dal.GetVotePay49(ID);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

        /// <summary>
        /// �õ�һ��BzType
        /// </summary>
        public int GetBzType(int ID)
        {
            return dal.GetBzType(ID);
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
        /// <returns>IList VotePay49</returns>
        public IList<LHC.Model.VotePay49> GetVotePay49s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetVotePay49s(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼ Ӯ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList VotePay49</returns>
        public IList<LHC.Model.VotePay49> GetVotePay49s_px(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetVotePay49s_px(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

