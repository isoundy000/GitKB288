using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// ҵ���߼���Cmg_buyuDonation ��ժҪ˵����
	/// </summary>
	public class Cmg_buyuDonation
	{
		private readonly BCW.bydr.DAL.Cmg_buyuDonation dal=new BCW.bydr.DAL.Cmg_buyuDonation();
		public Cmg_buyuDonation()
		{}
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
        /// </summary>
        public bool Exists1()
        {
            return dal.Exists1();
        }
         /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId(); 
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.bydr.Model.Cmg_buyuDonation model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.bydr.Model.Cmg_buyuDonation model)
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
		public BCW.bydr.Model.Cmg_buyuDonation GetCmg_buyuDonation(int ID)
		{
			
			return dal.GetCmg_buyuDonation(ID);
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
		/// <returns>IList Cmg_buyuDonation</returns>
		public IList<BCW.bydr.Model.Cmg_buyuDonation> GetCmg_buyuDonations(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmg_buyuDonations(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

