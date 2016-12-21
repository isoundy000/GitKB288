using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Medal ��ժҪ˵����
	/// </summary>
	public class Medal
	{
		private readonly BCW.DAL.Medal dal=new BCW.DAL.Medal();
		public Medal()
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
        public bool ExistsVip(int UsID)
        {
            return dal.ExistsVip(UsID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }
        
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsTypes(int Types, int UsID)
        {
            return dal.ExistsTypes(Types, UsID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsForumId(int ID, int ForumId)
        {
            return dal.ExistsForumId(ID, ForumId);
        }
              
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsPayIDtemp(int ForumId, int UsID)
        {
            return dal.ExistsPayIDtemp(ForumId, UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Medal model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Medal model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// ���»�Աѫ�¿��
        /// </summary>
        public void UpdateiCount(int ID, int iCount)
        {

            dal.UpdateiCount(ID, iCount);
        }
               
        /// <summary>
        /// ���»���ʱѫ��
        /// </summary>
        public void UpdatePayIDtemp(int ID, string PayIDtemp)
        {
            dal.UpdatePayIDtemp(ID, PayIDtemp);
        }

        /// <summary>
        /// ���»�Աѫ��
        /// </summary>
        public void UpdatePayID(int ID, string PayID, string PayExTime)
        {
            dal.UpdatePayID(ID, PayID, PayExTime);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
               
        /// <summary>
        /// �õ������¼ʵ��
        /// </summary>
        public string GetImageUrl(int ID)
        {

            return dal.GetImageUrl(ID);
        }
                       
        /// <summary>
        /// �õ������¼ʵ��
        /// </summary>
        public BCW.Model.Medal GetMedalMe(int ForumId, int UsID)
        {
            return dal.GetMedalMe(ForumId, UsID);
        }

        /// <summary>
        /// �õ���̳���Ա�ʶ
        /// </summary>
        public BCW.Model.Medal GetMedalForum(int UsID)
        {

            return dal.GetMedalForum(UsID);
        }

        /// <summary>
        /// �õ������¼ʵ��
        /// </summary>
        public BCW.Model.Medal GetMedalMe(int ID)
        {

            return dal.GetMedalMe(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Medal GetMedal(int ID)
		{
			
			return dal.GetMedal(ID);
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
		/// <returns>IList Medal</returns>
		public IList<BCW.Model.Medal> GetMedals(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMedals(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

