using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_Question ��ժҪ˵����
    /// </summary>
    public class tb_Question
    {
        private readonly BCW.DAL.tb_Question dal = new BCW.DAL.tb_Question();
        public tb_Question()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(string account)
        {
            return dal.Exists(account);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
         /// <summary>
        /// �������뱣����Ϊ�ֻ�����λ
        /// </summary>
        public void UpdateAnswer(string Answer, string Mobile)
        {
            dal.UpdateAnswer(Answer, Mobile);
        }
        /// <summary>
        /// ������֤��
        /// </summary>
        public void UpdateCode(string code, string Mobile)
        {
            dal.UpdateCode(code,Mobile);
        }
        /// <summary>
        /// �����ֻ���
        /// </summary>
        public void UpdateMobile(int ID, string Mobile)
        {
            dal.UpdateMobile(ID, Mobile);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.tb_Question model)
        {
            dal.Add(model);
        }
        /// <summary>
        /// �����޸�����Ĵ���
        /// </summary>
        public void UpdateChangeCount(int changecount, int account)
        {
            dal.UpdateChangeCount(changecount, account);
        }
        /// <summary>
        /// ������һ���޸����������
        /// </summary>
        public void UpdateLastChange(int lastchange, string Mobile)
        {
            dal.UpdateLastChange(lastchange, Mobile);
        }
        /// <summary>
        /// ����ID����һ������
        /// </summary>
        public void Update(BCW.Model.tb_Question model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// �����˺ŵõ�һ���޸�����Ĵ���
        /// </summary>
        public int GetChangeCount(int account)
        {
            return dal.GetChangeCount(account);
        }
         /// <summary>
        /// ���ݺ���õ���֤��
        /// </summary>
        public string GetCode(string Mypmobile)
        {
           return  dal.GetCode(Mypmobile);
        }
        /// <summary>
        /// ���ݺ���õ�����һ���޸��������������һ���е�������
        /// </summary>
        public int GetLastChange(int account)
        {
            return dal.GetLastChange(account);
        }
        /// <summary>
        /// ���ݺ���õ��������
        /// </summary>
        public string GetQuestion(int account)
        {
            return dal.GetQuestion(account);
        }
        /// <summary>
        /// ���ݺ���õ���Ĵ�
        /// </summary>
        public string GetAnswer(int account)
        {
            return dal.GetAnswer(account);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string Mobile)
        {

            dal.Delete(Mobile);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_Question Gettb_Question(string Mobile)
        {

            return dal.Gettb_Question(Mobile);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.tb_Question> GetBrags(int SizeNum, string strWhere)
        {
            return dal.GetBrags(SizeNum, strWhere);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList tb_Question</returns>
        public IList<BCW.Model.tb_Question> Gettb_Questions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_Questions(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

