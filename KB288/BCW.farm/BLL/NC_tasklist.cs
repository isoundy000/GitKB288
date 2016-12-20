using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_tasklist ��ժҪ˵����
    /// </summary>
    public class NC_tasklist
    {
        private readonly BCW.farm.DAL.NC_tasklist dal = new BCW.farm.DAL.NC_tasklist();
        public NC_tasklist()
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
        public int Add(BCW.farm.Model.NC_tasklist model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_tasklist model)
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
        public BCW.farm.Model.NC_tasklist GetNC_tasklist(int ID)
        {

            return dal.GetNC_tasklist(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList1(string strField, string strWhere)
        {
            return dal.GetList1(strField, strWhere);
        }
        //=================
        /// <summary>
        /// me_����id��usid��ѯ����
        /// </summary>
        public BCW.farm.Model.NC_tasklist Get_renwu(int ID, int usid)
        {

            return dal.Get_renwu(ID, usid);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼--�ճ�
        /// </summary>
        public bool Exists_usid(int usid, int task_id)
        {
            return dal.Exists_usid(usid, task_id);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼--����
        /// </summary>
        public bool Exists_usid1(int usid, int task_id)
        {
            return dal.Exists_usid1(usid, task_id);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼--�
        /// </summary>
        public bool Exists_usid3(int usid, int task_id)
        {
            return dal.Exists_usid3(usid, task_id);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼--�13-��������
        /// </summary>
        public bool Exists_usid13(int usid, int task_id, int task_oknum)
        {
            return dal.Exists_usid13(usid, task_id, task_oknum);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼--����(�����)�۹��� 20160922
        /// </summary>
        public bool Exists_usid2(int usid, int task_id)
        {
            return dal.Exists_usid2(usid, task_id);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_renwu(string strField, string strWhere)
        {
            return dal.update_renwu(strField, strWhere);
        }
        //=================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_tasklist</returns>
        public IList<BCW.farm.Model.NC_tasklist> GetNC_tasklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_tasklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

