using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Forumfund ��ժҪ˵����
	/// </summary>
	public class Forumfund
	{
		private readonly BCW.DAL.Forumfund dal=new BCW.DAL.Forumfund();
		public Forumfund()
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
		public int  Add(BCW.Model.Forumfund model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Forumfund model)
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
		public void DeleteStr(int GroupId)
		{
			dal.DeleteStr(GroupId);
		}
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Forumfund GetForumfund(int ID)
		{
			
			return dal.GetForumfund(ID);
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
		/// <returns>IList Forumfund</returns>
		public IList<BCW.Model.Forumfund> GetForumfunds(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetForumfunds(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

