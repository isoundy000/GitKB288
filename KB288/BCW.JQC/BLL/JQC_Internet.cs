using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JQC.Model;
namespace BCW.JQC.BLL
{
    /// <summary>
    /// ҵ���߼���JQC_Internet ��ժҪ˵����
    /// </summary>
    public class JQC_Internet
    {
        private readonly BCW.JQC.DAL.JQC_Internet dal = new BCW.JQC.DAL.JQC_Internet();
        public JQC_Internet()
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
        /// ����һ������
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Internet model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.JQC.Model.JQC_Internet model)
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet(int ID)
        {
            return dal.GetJQC_Internet(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet2(int ID)
        {
            return dal.GetJQC_Internet2(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }


        //====================================
        /// <summary>
        /// me_����һ������
        /// </summary>
        public void Update_ht(BCW.JQC.Model.JQC_Internet model)
        {
            dal.Update_ht(model);
        }
        /// <summary>
        /// me_��ʼ��ĳ���ݱ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }
        /// <summary>
        /// me_�õ����һ���ѿ��������ݡ���20160805
        /// </summary>
        public int Get_kaiID()
        {
            return dal.Get_kaiID();
        }
        /// <summary>
        /// me_�õ����һ��δ���������ݡ���20160714
        /// </summary>
        public int Get_newID()
        {
            return dal.Get_newID();
        }
        /// <summary>
        /// me_��̨���ز�ѯ����20160808
        /// </summary>
        public int Get_oldID()
        {
            return dal.Get_oldID();
        }
        /// <summary>
        /// me_��̨���ز�ѯ����20160808
        /// </summary>
        public int Get_oldID2()
        {
            return dal.Get_oldID2();
        }
        /// <summary>
        /// me_�Ƿ����δ�������ں� 20161013�۹���
        /// </summary>
        public bool Exists_wei(int ID)
        {
            return dal.Exists_wei(ID);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼���������ں�
        /// </summary>
        public bool Exists_phase(int ID)
        {
            return dal.Exists_phase(ID);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼�����Ƿ񿪽�
        /// </summary>
        public bool Exists_Result(int ID)
        {
            return dal.Exists_Result(ID);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet Update_Result(string strField, string strWhere)
        {
            return dal.Update_Result(strField, strWhere);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet_model(string where)
        {
            return dal.GetJQC_Internet_model(where);
        }
        /// <summary>
        /// me_�õ�һ�������������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Internet Get_kainum(int Lottery_issue)
        {
            return dal.Get_kainum(Lottery_issue);
        }
        /// <summary>
        /// me_�õ�ÿע���
        /// </summary>
        public string get_zhumeney(int Lottery_issue)
        {
            return dal.get_zhumeney(Lottery_issue);
        }
        /// <summary>
        /// me_�õ�ÿ�ڽ���
        /// </summary>
        public int get_jiangchi(int Lottery_issue)
        {
            return dal.get_jiangchi(Lottery_issue);
        }
        //====================================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList JQC_Internet</returns>
        public IList<BCW.JQC.Model.JQC_Internet> GetJQC_Internets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetJQC_Internets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

