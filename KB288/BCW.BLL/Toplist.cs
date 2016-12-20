using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Toplist ��ժҪ˵����
	/// </summary>
	public class Toplist
	{
		private readonly BCW.DAL.Toplist dal=new BCW.DAL.Toplist();
		public Toplist()
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
        public bool Exists(int UsId, int Types)
        {
            return dal.Exists(UsId, Types);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Toplist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Toplist model)
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
        /// ���ĳ�������а�
        /// </summary>
        public void Clear(int Types)
        {

            dal.Clear(Types);
        }

        /// <summary>
        /// ���ȫ�����а�
        /// </summary>
        public void Clear()
        {

            dal.Clear();
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Toplist GetToplist(int ID)
		{
			
			return dal.GetToplist(ID);
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
        /// <param name="strOrder">��������</param>
		/// <returns>IList Toplist</returns>
		public IList<BCW.Model.Toplist> GetToplists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetToplists(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

