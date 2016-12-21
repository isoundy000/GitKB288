using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Shopuser ��ժҪ˵����
	/// </summary>
	public class Shopuser
	{
		private readonly BCW.DAL.Shopuser dal=new BCW.DAL.Shopuser();
		public Shopuser()
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
        public bool Exists(int UsID, int GiftId)
        {
            return dal.Exists(UsID, GiftId);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼_ũ��
        /// </summary>
        public bool Exists_nc(int UsID, int GiftId)
        {
            return dal.Exists_nc(UsID, GiftId);
        }

        /// <summary>
        /// ����ĳID��������
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }
        /// <summary>
        /// �۹��� 20160512
        /// ����ĳID��ũ������
        /// </summary>
        public int GetCount_nc(int UsID)
        {
            return dal.GetCount_nc(UsID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int  Add(BCW.Model.Shopuser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Shopuser model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// ����һ������_ũ����ʶpic=1����  //�۹��� 20160607
        /// </summary>
        public void Update_nc(BCW.Model.Shopuser model)
        {
            dal.Update_nc(model);
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
		public BCW.Model.Shopuser GetShopuser(int ID)
		{
			
			return dal.GetShopuser(ID);
		}
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Shopuser GetShopuser(int UsID, int GiftId)
        {

            return dal.GetShopuser(UsID, GiftId);
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
		/// <returns>IList Shopuser</returns>
		public IList<BCW.Model.Shopuser> GetShopusers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
            return dal.GetShopusers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

               
        /// <summary>
        /// ������˰�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopuser> GetShopusersTop(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetShopusersTop(p_pageIndex, p_pageSize, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

