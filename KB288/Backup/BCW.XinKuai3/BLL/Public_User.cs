using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
	/// <summary>
	/// ҵ���߼���Public_User ��ժҪ˵����
	/// </summary>
	public class Public_User
	{
		private readonly BCW.XinKuai3.DAL.Public_User dal=new BCW.XinKuai3.DAL.Public_User();
		public Public_User()
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
		/// ����һ������
		/// </summary>
		public int  Add(BCW.XinKuai3.Model.Public_User model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.XinKuai3.Model.Public_User model)
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
		public BCW.XinKuai3.Model.Public_User GetPublic_User(int ID)
		{
			
			return dal.GetPublic_User(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// me_���¿����ע
        /// </summary>
        public void Update_1(int ID, string Settings, int type)
        {
            dal.Update_1(ID, Settings, type);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID ,int type)
        {
            return dal.Exists(ID, type);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Public_User</returns>
        public IList<BCW.XinKuai3.Model.Public_User> GetPublic_Users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetPublic_Users(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

