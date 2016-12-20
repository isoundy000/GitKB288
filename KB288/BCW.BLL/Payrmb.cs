using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
/// <summary>
/// ���ӻ�Ѹ֧���ӿ�
/// 
/// �ƹ��� 20160512
/// </summary>
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Payrmb ��ժҪ˵����
    /// </summary>
    public class Payrmb
    {
        private readonly BCW.DAL.Payrmb dal = new BCW.DAL.Payrmb();
        public Payrmb()
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
        public bool Exists(string CardOrder)
        {
            return dal.Exists(CardOrder);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Payrmb model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ��ֵ���ظ��±�ʶ
        /// </summary>
        /// <param name="model"></param>
        public void Update_ips(BCW.Model.Payrmb model)
        {
            dal.Update_ips(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Payrmb model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update3(BCW.Model.Payrmb model)
        {
            dal.Update3(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(BCW.Model.Payrmb model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// �õ��û�ID
        /// </summary>
        public int GetUsID(string CardOrder)
        {

            return dal.GetUsID(CardOrder);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Payrmb GetPayrmb(int ID)
        {

            return dal.GetPayrmb(ID);
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
        /// <returns>IList Payrmb</returns>
        public IList<BCW.Model.Payrmb> GetPayrmbs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetPayrmbs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

