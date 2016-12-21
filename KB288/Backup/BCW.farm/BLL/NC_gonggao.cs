using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_gonggao ��ժҪ˵����
    /// </summary>
    public class NC_gonggao
    {
        private readonly BCW.farm.DAL.NC_gonggao dal = new BCW.farm.DAL.NC_gonggao();
        public NC_gonggao()
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
        public int Add(BCW.farm.Model.NC_gonggao model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_gonggao model)
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
        public BCW.farm.Model.NC_gonggao GetNC_gonggao(int ID)
        {

            return dal.GetNC_gonggao(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //=======================
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_gonggao(string strField, string strWhere)
        {
            return dal.update_gonggao(strField, strWhere);
        }
        //=======================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_gonggao</returns>
        public IList<BCW.farm.Model.NC_gonggao> GetNC_gonggaos(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_gonggaos(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

