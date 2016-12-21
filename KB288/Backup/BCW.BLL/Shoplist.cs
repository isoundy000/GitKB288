using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Shoplist ��ժҪ˵����
	/// </summary>
	public class Shoplist
	{
		private readonly BCW.DAL.Shoplist dal=new BCW.DAL.Shoplist();
		public Shoplist()
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
		public int  Add(BCW.Model.Shoplist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Shoplist model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int ID, int PayCount)
        {
            dal.Update(ID, PayCount);
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
		public BCW.Model.Shoplist GetShoplist(int ID)
		{
			
			return dal.GetShoplist(ID);
		}
               
        /// <summary>
        /// �õ�Title
        /// </summary>
        public string GetTitle(int ID)
        {

            return dal.GetTitle(ID);
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
		/// <returns>IList Shoplist</returns>
		public IList<BCW.Model.Shoplist> GetShoplists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShoplists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

