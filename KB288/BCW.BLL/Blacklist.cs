using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Blacklist ��ժҪ˵����
	/// </summary>
	public class Blacklist
	{
		private readonly BCW.DAL.Blacklist dal=new BCW.DAL.Blacklist();
		public Blacklist()
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
        public bool Exists(int UsID, int ForumID)
        {
            return dal.Exists(UsID, ForumID);
        }
                
        /// <summary>
        /// �Ƿ���ڸ�Ȩ�޼�¼
        /// </summary>
        public bool ExistsRole(int UsID, string BlackRole)
        {
            return dal.ExistsRole(UsID, BlackRole);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Blacklist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Blacklist model)
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
        public void Delete(int UsID, int ForumID)
        {
            dal.Delete(UsID, ForumID);
        }
               
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteRole(int UsID, string BlackRole)
        {
            dal.DeleteRole(UsID, BlackRole);
        }

        /// <summary>
        /// �õ�һ��Role
        /// </summary>
        public string GetRole(int UsID, int ForumID)
        {
            return dal.GetRole(UsID, ForumID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Blacklist GetBlacklist(int ID)
		{
			
			return dal.GetBlacklist(ID);
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
		/// <returns>IList Blacklist</returns>
		public IList<BCW.Model.Blacklist> GetBlacklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBlacklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

