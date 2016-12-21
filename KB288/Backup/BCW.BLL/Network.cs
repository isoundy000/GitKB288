using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Network ��ժҪ˵����
	/// </summary>
	public class Network
	{
		private readonly BCW.DAL.Network dal=new BCW.DAL.Network();
		public Network()
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
        public bool Exists()
        {
            return dal.Exists();
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
        public bool Exists(int ID, int UsId)
        {
            return dal.Exists(ID, UsId);
        }
                
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsGroupChat(int Types, int GroupId)
        {
            return dal.ExistsGroupChat(Types, GroupId);
        }
        
        /// <summary>
        /// ����ĳ�û�����㲥����
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Network model)
		{
			return dal.Add(model);
		}
               
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateOnIDs(int ID, string OnIDs)
        {
            dal.UpdateOnIDs(ID, OnIDs);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateOther(BCW.Model.Network model)
        {
            dal.UpdateOther(model);
        }
               
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateGroupChat(BCW.Model.Network model)
        {
            dal.UpdateGroupChat(model);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Network model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int ID, DateTime OverTime)
        {
            dal.Update(ID, OverTime);
        }
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateBasic(BCW.Model.Network model)
        {
            dal.UpdateBasic(model);
        }

        /// <summary>
        /// �������
        /// </summary>
        public void Delete()
        {

            dal.Delete();
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
		public BCW.Model.Network GetNetwork(int ID)
		{
			
			return dal.GetNetwork(ID);
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
		/// <returns>IList Network</returns>
		public IList<BCW.Model.Network> GetNetworks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetNetworks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

