using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���BankUser ��ժҪ˵����
	/// </summary>
	public class BankUser
	{
		private readonly BCW.DAL.BankUser dal=new BCW.DAL.BankUser();
		public BankUser()
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
        public bool Exists(int UsID)
		{
            return dal.Exists(UsID);
		}
               
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsBankName(string BankName)
        {
            return dal.ExistsBankName(BankName);
        }
   
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsZFBName(string ZFBName)
        {
            return dal.ExistsZFBName(ZFBName);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.BankUser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.BankUser model)
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
        public BCW.Model.BankUser GetBankUser(int UsID)
		{

            return dal.GetBankUser(UsID);
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
		/// <returns>IList BankUser</returns>
		public IList<BCW.Model.BankUser> GetBankUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBankUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

