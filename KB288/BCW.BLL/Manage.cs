using System;
using System.Data;
using System.Collections.Generic;
using BCW.Model;
using BCW.Common;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Manage ��ժҪ˵����
	/// </summary>
	public class Manage
	{
		private readonly BCW.DAL.Manage dal=new BCW.DAL.Manage();
		public Manage()
		{}
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
        public bool ExistsKeys(string loginKeys)
        {
            return dal.ExistsKeys(loginKeys);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsUser(string sUser)
        {
            return dal.ExistsUser(sUser);
        }
               
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsUser(string sUser, int ID)
        {
            return dal.ExistsUser(sUser, ID);
        }

        /// <summary>
        /// ��ѯӰ�������
        /// </summary>
        /// <returns></returns>
        public int GetManageRow(BCW.Model.Manage model)
        {
            return dal.GetManageRow(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Manage model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ���º�̨��¼ʱ��/ip
        /// </summary>
        public void UpdateTimeIP(BCW.Model.Manage model)
        {
            dal.UpdateTimeIP(model);
        }

        /// <summary>
        /// ���º�̨Keys
        /// </summary>
        public void UpdateKeys(BCW.Model.Manage model)
        {
            dal.UpdateKeys(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Manage model)
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
        /// ����IDȡ�ù���ԱKeys
        /// </summary>
        public string GetKeys(int ID)
        {
            return dal.GetKeys(ID);
        }

        /// <summary>
        /// ����IDȡ�ù���ԱKeys
        /// </summary>
        public string GetKeys()
        {
            return dal.GetKeys();
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Manage GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        /// ����Keys�õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Manage GetModelByKeys(string sKeys)
        {
            return dal.GetModelByKeys(sKeys);
        }

        /// <summary>
        /// �����û�������õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Manage GetModelByModel(string sUser, string sPwd)
        {
            return dal.GetModelByModel(sUser, sPwd);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetManageList(string strField, string strWhere)
        {
            return dal.GetManageList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Manage</returns>
        public IList<BCW.Model.Manage> GetManages(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetManages(p_pageIndex, p_pageSize, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

