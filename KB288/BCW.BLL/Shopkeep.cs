using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Shopkeep ��ժҪ˵����
    /// </summary>
    public class Shopkeep
    {
        private readonly BCW.DAL.Shopkeep dal = new BCW.DAL.Shopkeep();
        public Shopkeep()
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
        public bool Exists(int GiftId, int UsID)
        {
            return dal.Exists(GiftId, UsID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists1(int ID, int UsID)
        {
            return dal.Exists1(ID, UsID);
        }
        /// <summary>
        /// ���������õ�ID
        /// </summary>
        public int GetID(int GiftId, int UsID)
        {
            return dal.GetID(GiftId, UsID);
        }

        /// <summary>
        /// ����ĳ����-ĳ�û����칺������
        /// </summary>
        public int GetTodayCount(int UsID, int GiftId)
        {
            return dal.GetTodayCount(UsID, GiftId);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Shopkeep model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Shopkeep model)
        {
            dal.Update(model);
        }

        public void Update_ips(BCW.Model.Shopkeep model)
        {
            dal.Update_ips(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateTotal(int ID, int Total)
        {
            dal.UpdateTotal(ID, Total);
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
        public BCW.Model.Shopkeep GetShopkeep(int ID)
        {

            return dal.GetShopkeep(ID);
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
        /// <returns>IList Shopkeep</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeeps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShopkeeps(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        /// <summary>
        /// ���а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeepsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShopkeepsTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ���а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeepsTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShopkeepsTop2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  ��Ա����
    }
}

