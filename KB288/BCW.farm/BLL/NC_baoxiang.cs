using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_baoxiang ��ժҪ˵����
    /// </summary>
    public class NC_baoxiang
    {
        private readonly BCW.farm.DAL.NC_baoxiang dal = new BCW.farm.DAL.NC_baoxiang();
        public NC_baoxiang()
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
        public int Add(BCW.farm.Model.NC_baoxiang model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_baoxiang model)
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
        public BCW.farm.Model.NC_baoxiang GetNC_baoxiang(int ID)
        {

            return dal.GetNC_baoxiang(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //===========================================
        /// <summary>
        /// me_��������б�
        /// </summary>
        public List<BCW.farm.Model.NC_baoxiang> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList("*", strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// me_��������б�
        /// </summary>
        public List<BCW.farm.Model.NC_baoxiang> DataTableToList(DataTable dt)
        {
            List<BCW.farm.Model.NC_baoxiang> modelList = new List<BCW.farm.Model.NC_baoxiang>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.farm.Model.NC_baoxiang model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
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
        /// me_��ѯ�м�������
        /// </summary>
        public BCW.farm.Model.NC_baoxiang Get_num(int id)
        {
            return dal.Get_num(id);
        }
        /// <summary>
        /// me_�ж��Ƿ�������ӻ��ߵ���id
        /// </summary>
        public bool Exists_bxzzdj(int ID, int type)
        {
            return dal.Exists_bxzzdj(ID, type);
        }
        //===========================================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_baoxiang</returns>
        public IList<BCW.farm.Model.NC_baoxiang> GetNC_baoxiangs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_baoxiangs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

