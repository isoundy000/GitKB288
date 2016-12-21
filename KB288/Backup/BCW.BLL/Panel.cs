using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Panel ��ժҪ˵����
	/// </summary>
	public class Panel
	{
		private readonly BCW.DAL.Panel dal=new BCW.DAL.Panel();
		public Panel()
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
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Panel model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// ���º�������
        /// </summary>
        public void UpdateIsBr(int UsID, int IsBr)
        {
            dal.UpdateIsBr(UsID, IsBr);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            dal.UpdatePaixu(ID, Paixu);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Panel model)
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
        /// ��������ɾ������
        /// </summary>
        public void Delete(int UsID, string Title, string PUrl)
        {

            dal.Delete(UsID, Title, PUrl);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Panel GetPanel(int ID)
		{
			
			return dal.GetPanel(ID);
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
		/// <returns>IList Panel</returns>
		public IList<BCW.Model.Panel> GetPanels(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetPanels(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

