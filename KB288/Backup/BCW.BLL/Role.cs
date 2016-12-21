using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Role ��ժҪ˵����
	/// </summary>
	public class Role
	{
		private readonly BCW.DAL.Role dal=new BCW.DAL.Role();
		public Role()
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
        /// �Ƿ����ΰ���
        /// </summary>
        public bool ExistsOver(int ID)
        {
            return dal.ExistsOver(ID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UsID, int ForumID)
        {
            return dal.Exists(UsID, ForumID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID, int UsID)
        {
            return dal.Exists2(ID, UsID);
        }   

        /// <summary>
        /// �Ƿ����Ա
        /// </summary>
        public bool IsAdmin(int UsID)
        {
            return dal.IsAdmin(UsID);
        }

        /// <summary>
        /// �Ƿ��ܰ���
        /// </summary>
        public bool IsMode(int UsID)
        {
            return dal.IsMode(UsID);
        }

        /// <summary>
        /// �Ƿ������
        /// </summary>
        public bool IsSubMode(int UsID)
        {
            return dal.IsSubMode(UsID);
        }
                
        /// <summary>
        /// �Ƿ����ܰ�������Ȩ��
        /// </summary>
        public bool IsSumMode(int UsID)
        {
            return dal.IsSumMode(UsID);
        }

        /// <summary>
        /// �Ƿ��а���������Ȩ��
        /// </summary>
        public bool IsAllMode(int UsID)
        {
            return dal.IsAllMode(UsID);
        }
        
        /// <summary>
        /// �Ƿ��а���������Ȩ��(������Ȧ�Ӱ���)
        /// </summary>
        public bool IsAllModeNoGroup(int UsID)
        {
            return dal.IsAllModeNoGroup(UsID);
        }
        
        /// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Role model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Role model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����Ϊ��������
        /// </summary>
        public void UpdateOver(int ID, int Status)
        {
            dal.UpdateOver(ID, Status);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                      
        /// <summary>
        /// �õ�һ��ForumID
        /// </summary>
        public int GetForumID(int ID)
        {
            return dal.GetForumID(ID);
        }

        /// <summary>
        /// �õ�һ��Rolece
        /// </summary>
        public string GetRolece(int UsID)
        {
            return dal.GetRolece(UsID);
        }
             
        /// <summary>
        /// �õ�һ��Rolece
        /// </summary>
        public string GetRolece(int UsID, int ForumID)
        {
            return dal.GetRolece(UsID, ForumID);
        }

        /// <summary>
        /// �õ�һ�������¼�����Rolece
        /// </summary>
        public string GetRoleces(int UsID, int ForumID)
        {
            return dal.GetRoleces(UsID, ForumID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Role GetRole(int ID)
		{
			
			return dal.GetRole(ID);
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
		/// <returns>IList Role</returns>
		public IList<BCW.Model.Role> GetRoles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetRoles(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// ȡ�ù���Ա��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Role</returns>
        public IList<BCW.Model.Role> GetRolesAdmin(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetRolesAdmin(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

