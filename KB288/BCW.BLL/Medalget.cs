using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Medalget ��ժҪ˵����
	/// </summary>
	public class Medalget
	{
		private readonly BCW.DAL.Medalget dal=new BCW.DAL.Medalget();
		public Medalget()
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
		public bool Exists(int UsID,int ID)
		{
			return dal.Exists(UsID,ID);
		}
               
        /// <summary>
        /// ����ĳ�û�����
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Medalget model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Medalget model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int UsID,int ID)
		{
			
			dal.Delete(UsID,ID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Medalget GetMedalget(int UsID,int ID)
		{
			
			return dal.GetMedalget(UsID,ID);
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
		/// <returns>IList Medalget</returns>
		public IList<BCW.Model.Medalget> GetMedalgets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMedalgets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// ����ѫ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Medalget> GetMedalgetsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {

            return dal.GetMedalgetsTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

