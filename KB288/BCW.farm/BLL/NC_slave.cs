using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_slave ��ժҪ˵����
    /// </summary>
    public class NC_slave
    {
        private readonly BCW.farm.DAL.NC_slave dal = new BCW.farm.DAL.NC_slave();
        public NC_slave()
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
        public int Add(BCW.farm.Model.NC_slave model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_slave model)
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
        public BCW.farm.Model.NC_slave GetNC_slave(int ID)
        {

            return dal.GetNC_slave(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //===================================
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_ziduan(string strField, string strWhere)
        {
            return dal.update_ziduan(strField, strWhere);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_slave GetNCslave(int meid, int usid)
        {

            return dal.GetNCslave(meid, usid);
        }
        /// <summary>
        /// me_�Ƿ���ڸ�ū����¼
        /// </summary>
        public bool Exists_nl(int slave_id, int usid)
        {
            return dal.Exists_nl(slave_id, usid);
        }
        /// <summary>
        /// me_�Ƿ���ڸ�ū���Ѿ����ڼ�¼
        /// </summary>
        public bool Exists_nl2(int slave_id, int usid)
        {
            return dal.Exists_nl2(slave_id, usid);
        }
        /// <summary>
        /// me_����ū����Ϣ
        /// </summary>
        public void Update_nl(BCW.farm.Model.NC_slave model)
        {
            dal.Update_nl(model);
        }
        //===================================



        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_slave</returns>
        public IList<BCW.farm.Model.NC_slave> GetNC_slaves(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_slaves(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

