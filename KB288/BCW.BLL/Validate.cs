using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_Validate ��ժҪ˵����
    /// </summary>
    public class tb_Validate
    {
        private readonly BCW.DAL.tb_Validate dal = new BCW.DAL.tb_Validate();
        public tb_Validate()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// type 1 ע�ᣬ2 �޸����� 3 �޸��ܱ� 4�޸��ֻ�
        ///      5 ��������
        /// </summary>
        public bool ExistsPhone(string Phone,int type)
        {
            return dal.ExistsPhone(Phone, type);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsPhone(string Phone)
        {
            return dal.ExistsPhone(Phone);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.tb_Validate model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateFlag(int Flag, int ID)
        {
            dal.UpdateFlag(Flag,ID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_Validate model)
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
        public BCW.Model.tb_Validate Gettb_Validate(int ID)
        {

            return dal.Gettb_Validate(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_Validate Gettb_Validate(string Phone, int type)
        {
            
            return dal.Gettb_Validate(Phone,  type);
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
        /// <returns>IList tb_Validate</returns>
        public IList<BCW.Model.tb_Validate> Gettb_Validates(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_Validates(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

