using System;
using System.Data;
using System.Collections.Generic;
using TPR3.Model.guess;
namespace TPR3.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaOrder ��ժҪ˵����
    /// </summary>
    public class BaOrder
    {
        private readonly TPR3.DAL.guess.BaOrder dal = new TPR3.DAL.guess.BaOrder();
        public BaOrder()
        { }
        #region  ��Ա����

        /// <summary>
        /// ����/�������а�
        /// </summary>
        public void UpdateOrder(TPR3.Model.guess.BaOrder model)
        {
            dal.UpdateOrder(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr()
        {
            dal.DeleteStr();
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaOrderList(string strField, string strWhere)
        {
            return dal.GetBaOrderList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR3.Model.guess.BaOrder> GetBaOrders(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBaOrders(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
