using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using TPR.Model.guess;
/// <summary>
/// �����ؿ����ҽ���λ
/// �ƹ��� 20160715
/// </summary>
namespace TPR.BLL.guess
{
    /// <summary>
    /// ҵ���߼���Super ��ժҪ˵����
    /// </summary>
    public class Super
    {
        private readonly TPR.DAL.guess.Super dal = new TPR.DAL.guess.Super();
        public Super()
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
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsIsCase(int ID, int payusid)
        {
            return dal.ExistsIsCase(ID, payusid);
        }

        /// <summary>
        /// �����û��ҽ�
        /// </summary>
        public void UpdateIsCase(int ID)
        {
            dal.UpdateIsCase(ID);
        }

        public void UpdateNotCase(int ID)
        {
            dal.UpdateNotCase(ID);
        }
        /// <summary>
        /// ���������õ�������ע��ע��
        /// </summary>
        public int GetSuperCount(string strWhere)
        {
            return dal.GetSuperCount(strWhere);
        }

        /// <summary>
        /// ���������õ�������ע�ܽ��
        /// </summary>
        public long GetSuperpayCent(string strWhere)
        {
            return dal.GetSuperpayCent(strWhere);
        }

        /// <summary>
        /// ���������õ�������עӯ����
        /// </summary>
        public long GetSupergetMoney(string strWhere)
        {
            return dal.GetSupergetMoney(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(TPR.Model.guess.Super model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(TPR.Model.guess.Super model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(TPR.Model.guess.Super model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update3(TPR.Model.guess.Super model)
        {
            dal.Update3(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update4(TPR.Model.guess.Super model)
        {
            dal.Update4(model);
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
        /// �õ�һ��ID
        /// </summary>
        public int GetSuperID(int UsID)
        {
            return dal.GetSuperID(UsID);
        }


        /// <summary>
        /// �õ�һ��BID����
        /// </summary>
        public string GetSuperBID(int ID)
        {
            return dal.GetSuperBID(ID);
        }

        /// <summary>
        /// �õ�һ��getMoney
        /// </summary>
        public decimal GetgetMoney(int ID)
        {
            return dal.GetgetMoney(ID);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR.Model.guess.Super GetSuper(int ID)
        {

            return dal.GetSuper(ID);
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
        /// <returns>IList Super</returns>
        public IList<TPR.Model.guess.Super> GetSupers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSupers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

