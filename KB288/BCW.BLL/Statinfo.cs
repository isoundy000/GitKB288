using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Statinfo ��ժҪ˵����
    /// </summary>
    public class Statinfo
    {
        private readonly BCW.DAL.Statinfo dal = new BCW.DAL.Statinfo();
        public Statinfo()
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
        /// ���������õ���¼��
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// ���������õ���¼��
        /// </summary>
        public int GetIPCount(string strWhere)
        {
            return dal.GetIPCount(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Statinfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Statinfo model)
        {
            dal.Update(model);
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
        public BCW.Model.Statinfo GetStatinfo(int ID)
        {

            return dal.GetStatinfo(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strSql)
        {
            return dal.GetList(strSql);
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
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetStatinfos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStatinfos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��IP��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetIPs(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetIPs(p_pageIndex, p_pageSize, out p_recordCount);
        }

        /// <summary>
        /// ȡ��Browser��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetBrowsers(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetBrowsers(p_pageIndex, p_pageSize, out p_recordCount);
        }

        /// <summary>
        /// ȡ��System��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetSystems(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetSystems(p_pageIndex, p_pageSize, out p_recordCount);
        }

        /// <summary>
        /// ȡ��Purl��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Statinfo</returns>
        public IList<BCW.Model.Statinfo> GetPUrls(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetPUrls(p_pageIndex, p_pageSize, out p_recordCount);
        }
        #endregion  ��Ա����
    }
}
