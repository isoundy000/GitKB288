using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Spkblack ��ժҪ˵����
    /// </summary>
    public class Spkblack
    {
        private readonly BCW.DAL.Spkblack dal = new BCW.DAL.Spkblack();
        public Spkblack()
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
        public bool Exists(int UsID, int Types)
        {
            return dal.Exists(UsID, Types);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Spkblack model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Spkblack model)
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int Types)
        {

            dal.Delete(UsID, Types);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Spkblack GetSpkblack(int ID)
        {

            return dal.GetSpkblack(ID);
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
        /// <returns>IList Spkblack</returns>
        public IList<BCW.Model.Spkblack> GetSpkblacks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpkblacks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

