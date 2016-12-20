using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���GroupText ��ժҪ˵����
	/// </summary>
	public class GroupText
	{
		private readonly BCW.DAL.GroupText dal=new BCW.DAL.GroupText();
		public GroupText()
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
        public bool Exists(int ID, int GroupId)
        {
            return dal.Exists(ID, GroupId);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.GroupText model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.GroupText model)
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
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.GroupText GetGroupText(int ID)
		{
			
			return dal.GetGroupText(ID);
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
		/// <returns>IList GroupText</returns>
		public IList<BCW.Model.GroupText> GetGroupTexts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetGroupTexts(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

